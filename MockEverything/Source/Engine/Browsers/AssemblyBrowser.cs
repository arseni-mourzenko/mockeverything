// <copyright file="AssemblyBrowser.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Browsers
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Attributes;
    using Inspection;

    /// <summary>
    /// Represents a browser which is able to find the proxy types in a proxy assembly, matching them with the corresponding types from the target assembly.
    /// </summary>
    public class AssemblyBrowser : Browser<IType, IAssembly>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyBrowser"/> class.
        /// </summary>
        /// <param name="proxy">The proxy assembly.</param>
        /// <param name="target">The corresponding target assembly.</param>
        /// <param name="matchSearch">The searcher which, for every proxy type found, finds the corresponding target type.</param>
        public AssemblyBrowser(IAssembly proxy, IAssembly target, IMatching<IType, IAssembly> matchSearch) : base(proxy, target, matchSearch)
        {
            Contract.Requires(proxy != null);
            Contract.Requires(target != null);
            Contract.Requires(matchSearch != null);
        }

        /// <summary>
        /// Lists all the types from the proxy assembly which are proxies themselves.
        /// </summary>
        /// <returns>Zero or more elements.</returns>
        protected override IEnumerable<IType> FindAllInProxy()
        {
            return this.Proxy.FindTypes(MemberType.Static, typeof(ProxyOfAttribute));
        }
    }
}