// <copyright file="Tampering.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Tampering
{
    using System;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
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
        /// Merges the proxy and the target assemblies and tampers the resulting one.
        /// </summary>
        /// <returns>The resulting assembly.</returns>
        public IAssembly Tamper()
        {
            Contract.Ensures(Contract.Result<IAssembly>() != null);

            var tempMergedAssemblyPath = Path.GetTempFileName();
            var result = new Assembly(tempMergedAssemblyPath);

            var types = new AssemblyBrowser(result, result, new TypeMatching()).FindTypes();
            var pairs = types.SelectMany(t => new TypeBrowser(t.Proxy, t.Target, new MethodMatching()).FindTypes());

            foreach (var pair in pairs)
            {
                pair.Target.ReplaceBody(pair.Proxy);
            }

            if (this.ResultVersion != null)
            {
                result.AlterVersion(this.ResultVersion);
            }

            return result;
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

            new ILRepack(options).Repack();
        }

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
    }
}