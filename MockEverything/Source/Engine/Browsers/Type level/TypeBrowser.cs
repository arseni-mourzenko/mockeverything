// <copyright file="TypeBrowser.cs">
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
    /// Represents a browser which is able to find the proxy methods of a proxy type, matching them with the corresponding types from the target type.
    /// </summary>
    public class TypeBrowser : Browser<IMethod, IType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBrowser"/> class.
        /// </summary>
        /// <param name="proxy">The proxy type.</param>
        /// <param name="target">The corresponding target type.</param>
        /// <param name="matchSearch">The searcher which, for every proxy method found, finds the corresponding target method.</param>
        public TypeBrowser(IType proxy, IType target, IMatchSearch<IMethod, IType> matchSearch) : base(proxy, target, matchSearch)
        {
            Contract.Requires(proxy != null);
            Contract.Requires(target != null);
            Contract.Requires(matchSearch != null);
        }

        /// <summary>
        /// Lists all the methods from the proxy type which are proxies themselves.
        /// </summary>
        /// <returns>Zero or more elements.</returns>
        protected override IEnumerable<IMethod> FindAllInProxy()
        {
            return this.Proxy.FindMethods(MemberType.All, typeof(ProxyMethodAttribute));
        }
    }
}
