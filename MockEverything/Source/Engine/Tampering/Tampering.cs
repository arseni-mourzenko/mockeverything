// <copyright file="Tampering.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Tampering
{
    using System;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using Attributes;
    using Browsers;
    using ILRepacking;
    using Inspection;
    using Inspection.MonoCecil;

    /// <summary>
    /// Represents the tampering which executes the steps required to create a tampered assembly from a proxy and a target.
    /// </summary>
    public class Tampering : ITampering
    {
        /// <summary>
        /// Gets or sets the pair of proxy and target assemblies.
        /// </summary>
        public Pair<IAssembly> Pair { get; set; }

        /// <summary>
        /// Gets or sets the version to set to the resulting assembly. If <see langword="null"/>, the version won't be changed and will correspond to the version of the target assembly.
        /// </summary>
        public Version ResultVersion { get; set; }

        /// <summary>
        /// Gets the full path of the directory containing the executing assembly.
        /// </summary>
        private string CurrentDirectoryPath
        {
            get
            {
                var codebase = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var path = Uri.UnescapeDataString(new UriBuilder(codebase).Path);
                return Path.GetDirectoryName(path);
            }
        }

        /// <summary>
        /// Merges the proxy and the target assemblies and tampers the resulting one.
        /// </summary>
        /// <param name="dependenciesLocations">The paths to the directories which may contain the dependencies.</param>
        /// <returns>The resulting assembly.</returns>
        public IAssembly Tamper(params string[] dependenciesLocations)
        {
            Contract.Ensures(Contract.Result<IAssembly>() != null);

            var tempDirectoryPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var tempMergedAssemblyPath = Path.Combine(tempDirectoryPath, Path.GetFileName(this.Pair.Target.FilePath));

            Trace.WriteLine(string.Format("Tampering assemblies using temporary file {0}.", tempMergedAssemblyPath));

            Directory.CreateDirectory(tempDirectoryPath);

            Trace.WriteLine("Performing the merging.");
            this.Merge(tempMergedAssemblyPath);
            var result = new Assembly(tempMergedAssemblyPath, dependenciesLocations);

            Trace.WriteLine("Checking for mistakes.");
            this.CheckForMistakes(this.Pair.Proxy);

            Trace.WriteLine("Altering the version, if needed.");
            this.AlterVersion(result);

            Trace.WriteLine("Replacing the public key, if needed.");
            result.ReplacePublicKey(this.Pair.Target);

            Trace.WriteLine("Rewriting the bodies of the methods within the target assembly.");
            this.Rewrite(result);

            Trace.WriteLine("Tampering finished.");
            return result;
        }

        /// <summary>
        /// Runs basic checks which search for common patterns which can cause difficult to debug issues.
        /// </summary>
        /// <param name="proxy">The proxy assembly.</param>
        /// <exception cref="ProxyMistakeException">The assembly contains a suspicious pattern.</exception>
        private void CheckForMistakes(IAssembly proxy)
        {
            Contract.Requires(proxy != null);

            this.CheckForInstanceTypes(proxy);
            this.CheckForNonPublicMethods(proxy);
        }

        /// <summary>
        /// Ensures that the proxy assembly contains no instance types. Instance types don't make much sense in a context of a proxy, and so indicate with high probability a typo.
        /// </summary>
        /// <param name="proxy">The proxy assembly.</param>
        /// <exception cref="ProxyMistakeException">The assembly contains instance types.</exception>
        private void CheckForInstanceTypes(IAssembly proxy)
        {
            Contract.Requires(proxy != null);

            var instanceTypes = proxy.FindTypes(MemberType.Instance).Select(t => t.FullName).Except(new[] { "<Module>" }).ToList();
            switch (instanceTypes.Count)
            {
                case 0:
                    break;

                case 1:
                    throw new ProxyMistakeException(string.Format("The proxy assembly is not expected to contain instance types. The following instance type was found: {0}.", instanceTypes.Single()));

                default:
                    throw new ProxyMistakeException(string.Format("The proxy assembly is not expected to contain instance types. The following instance types were found: {0}.", string.Join("; ", instanceTypes)));
            }
        }

        /// <summary>
        /// Ensures that the proxy assembly contains no private, protected or internal methods. Those methods are dangerous in this context, since a call to a non-public method from a proxy one will compile (since both methods are in the same class), but then result in a runtime exception (since the method will be in a different class).
        /// </summary>
        /// <param name="proxy">The proxy assembly.</param>
        /// <exception cref="ProxyMistakeException">The assembly contains non-public methods.</exception>
        private void CheckForNonPublicMethods(IAssembly proxy)
        {
            var privateMethods = proxy
                .FindTypes(MemberType.Static, typeof(ProxyOfAttribute))
                .SelectMany(t => t.FindMethods(MemberType.Static).Select(m => new { FullName = t.FullName + ":" + m.Name, IsPublic = m.IsPublic }))
                .Where(m => !m.IsPublic)
                .Select(m => m.FullName)
                .ToList();

            switch (privateMethods.Count)
            {
                case 0:
                    break;

                case 1:
                    throw new ProxyMistakeException(string.Format("The proxy assembly is not expected to contain private methods, since after the merge, they could be called from a tampered class. The following private method was found: {0}.", privateMethods.Single()));

                default:
                    throw new ProxyMistakeException(string.Format("The proxy assembly is not expected to contain private methods, since after the merge, they could be called from a tampered class. The following private methods were found: {0}.", string.Join("; ", privateMethods)));
            }
        }

        /// <summary>
        /// Rewrites the target methods by the proxy ones.
        /// </summary>
        /// <param name="assembly">The assembly to modify.</param>
        private void Rewrite(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            var types = new AssemblyBrowser(assembly, assembly, new TypeMatching()).FindTypes().ToList();
            foreach (var type in types)
            {
                var entryHook = type.Proxy.FindMethods(MemberType.Static, typeof(EntryHookAttribute)).SingleOrDefault();

                Trace.WriteLine(string.Format("Rewriting type {0}.", string.Join(", ", type.Target.FullName)));
                var pairs = new TypeBrowser(type.Proxy, type.Target, new MethodMatching()).FindTypes();

                foreach (var pair in pairs)
                {
                    Trace.WriteLine(string.Format("Rewriting pair {0}.", pair.Target.Name));
                    pair.Target.ReplaceBody(pair.Proxy, entryHook);
                }
            }
        }

        /// <summary>
        /// Changes the version of the assembly, if a specific version is set.
        /// </summary>
        /// <param name="assembly">The assembly to modify.</param>
        private void AlterVersion(Assembly assembly)
        {
            Contract.Requires(assembly != null);

            if (this.ResultVersion != null)
            {
                assembly.AlterVersion(this.ResultVersion);
            }
        }

        /// <summary>
        /// Merges the proxy and the target assemblies.
        /// </summary>
        /// <param name="outputPath">The path of the resulting assembly.</param>
        private void Merge(string outputPath)
        {
            Contract.Requires(outputPath != null);

            var options = new RepackOptions
            {
                InputAssemblies = new[] { this.Pair.Target.FilePath, this.Pair.Proxy.FilePath },
                OutputFile = outputPath,
                TargetKind = ILRepack.Kind.Dll,
                SearchDirectories = new[] { this.Pair.Target.FilePath, this.Pair.Proxy.FilePath }.Select(Path.GetDirectoryName),
                KeyFile = Path.Combine(this.CurrentDirectoryPath, "MockEverything.snk"),
            };

            // The `Log` property of `RepackOptions` being gracefully ignored by the library (see ILRepack/RepackLogger.cs, l. 16), the only way to get rid of the logging is to create a logger which does nothing.
            new ILRepack(options, new NullLogger()).Repack();
        }

        /// <summary>
        /// Represents the logger which silently swallows the log messages.
        /// </summary>
        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "*", Justification = "The methods come from the interface; documenting them would lead to the duplication of the documentation.")]
        private class NullLogger : ILogger
        {
            public bool ShouldLogVerbose { get; set; }

            public void DuplicateIgnored(string ignoredType, object ignoredObject)
            {
            }

            public void Error(string msg)
            {
            }

            public void Info(string msg)
            {
            }

            public void Log(object str)
            {
            }

            public void Verbose(string msg)
            {
            }

            public void Warn(string msg)
            {
            }
        }
    }
}