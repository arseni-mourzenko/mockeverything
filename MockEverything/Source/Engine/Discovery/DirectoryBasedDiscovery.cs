// <copyright file="DirectoryBasedDiscovery.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Discovery
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using Browsers;
    using Inspection;

    /// <summary>
    /// Represents a discovery which searches for proxy assemblies and their targets within a directory.
    /// </summary>
    public class DirectoryBasedDiscovery : IProxiesDiscovery
    {
        /// <summary>
        /// The ending of the names of proxy files.
        /// </summary>
        private const string ProxySuffix = ".Proxies.dll";

        /// <summary>
        /// The ending of the names of target files.
        /// </summary>
        private const string TargetSuffix = ".dll";

        /// <summary>
        /// The access to the file system.
        /// </summary>
        private readonly IDirectoryAccess dataAccess;

        /// <summary>
        /// The path of the directory containing the proxy assemblies and their targets.
        /// </summary>
        private readonly string directoryPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryBasedDiscovery"/> class.
        /// </summary>
        /// <param name="dataAccess">The access to the file system.</param>
        /// <param name="directoryPath">The path of the directory containing the proxy assemblies and their targets.</param>
        public DirectoryBasedDiscovery(IDirectoryAccess dataAccess, string directoryPath)
        {
            Contract.Requires(directoryPath != null);

            this.dataAccess = dataAccess;
            this.directoryPath = directoryPath;
        }

        /// <summary>
        /// Lists the pairs, each pair containing a proxy assembly and its corresponding target assembly.
        /// </summary>
        /// <returns>Zero or more pairs.</returns>
        public IEnumerable<Pair<IAssembly>> FindAssemblies()
        {
            Contract.Ensures(Contract.Result<IEnumerable<Pair<IAssembly>>>() != null);

            return this.dataAccess.ListFilesEndingBy(this.directoryPath, DirectoryBasedDiscovery.ProxySuffix).Select(this.CreatePair);
        }

        /// <summary>
        /// Creates a pair of assemblies, matching the proxy assembly to the target one.
        /// </summary>
        /// <param name="proxyPath">The full path to the proxy assembly.</param>
        /// <returns>The pair associating the proxy and the target assembly.</returns>
        private Pair<IAssembly> CreatePair(string proxyPath)
        {
            Contract.Requires(proxyPath != null);
            Contract.Ensures(Contract.Result<Pair<IAssembly>>() != null);

            var proxy = this.dataAccess.LoadAssembly(proxyPath);
            return new Pair<IAssembly>(proxy: proxy, target: this.FindTarget(proxy));
        }

        /// <summary>
        /// Finds a target assembly corresponding to the proxy assembly.
        /// </summary>
        /// <param name="proxy">The proxy assembly.</param>
        /// <returns>The corresponding target assembly.</returns>
        /// <exception cref="MatchNotFoundException">The target assembly file doesn't exist.</exception>
        private IAssembly FindTarget(IAssembly proxy)
        {
            Contract.Requires(proxy != null);
            Contract.Ensures(Contract.Result<IAssembly>() != null);

            Trace.WriteLine(string.Format("Exploring proxy {0}.", proxy.FullName));

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(this.ResolveAssembly);

            var pathsOfTargetAssemblies = from type in Assembly.Load(File.ReadAllBytes(proxy.FilePath)).GetTypes()
                                          let attr = type.GetCustomAttribute<ProxyOfAttribute>()
                                          where attr != null
                                          let targetAssembly = attr.TargetType.Assembly
                                          select new Uri(targetAssembly.CodeBase).AbsolutePath;

            var distinctPaths = pathsOfTargetAssemblies.Distinct().ToList();
            switch (distinctPaths.Count)
            {
                case 0:
                    throw new MatchNotFoundException();

                case 1:
                    return this.dataAccess.LoadAssembly(distinctPaths.Single());

                default:
                    throw new MultipleTargetsException();
            }
        }

        /// <summary>
        /// Attempts to resolve the assembly by searching within the current directory.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="args">The event arguments.</param>
        /// <returns>The assembly, or <see langword="null"/> if the assembly doesn't exist.</returns>
        private Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            var folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
            return File.Exists(assemblyPath) ? Assembly.LoadFrom(assemblyPath) : null;
        }
    }
}