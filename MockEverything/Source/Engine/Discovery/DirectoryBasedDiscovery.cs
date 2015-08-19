// <copyright file="DirectoryBasedDiscovery.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Discovery
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Browsers;
    using Inspection;
    using Inspection.MonoCecil;

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

            var lengthToRemove = proxyPath.Length - DirectoryBasedDiscovery.ProxySuffix.Length + 2;

            Contract.Assume(lengthToRemove > 0);
            Contract.Assume(lengthToRemove < proxyPath.Length);

            var targetPath = proxyPath.Substring(0, lengthToRemove) + DirectoryBasedDiscovery.TargetSuffix;
            if (!this.dataAccess.FileExists(targetPath))
            {
                throw new MatchNotFoundException();
            }

            return new Pair<IAssembly>(
                proxy: this.dataAccess.LoadAssembly(proxyPath),
                target: this.dataAccess.LoadAssembly(targetPath));
        }
    }
}
