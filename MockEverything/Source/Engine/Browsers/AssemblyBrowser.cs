// <copyright file="AssemblyBrowser.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Browsers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using Inspection;

    /// <summary>
    /// Represents a browser which is able to find the proxy types in a proxy assembly, matching them with the corresponding types from the target assembly.
    /// </summary>
    public class AssemblyBrowser
    {
        /// <summary>
        /// The proxy assembly.
        /// </summary>
        private readonly IAssembly proxy;

        /// <summary>
        /// The corresponding target assembly.
        /// </summary>
        private readonly IAssembly target;

        /// <summary>
        /// The searcher which, for every proxy type found, finds the corresponding target type.
        /// </summary>
        private readonly ITypeMatchSearch matchSearch;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyBrowser"/> class.
        /// </summary>
        /// <param name="proxy">The proxy assembly.</param>
        /// <param name="target">The corresponding target assembly.</param>
        /// <param name="matchSearch">The searcher which, for every proxy type found, finds the corresponding target type.</param>
        public AssemblyBrowser(IAssembly proxy, IAssembly target, ITypeMatchSearch matchSearch)
        {
            Contract.Requires(proxy != null);
            Contract.Requires(target != null);
            Contract.Requires(matchSearch != null);

            this.proxy = proxy;
            this.target = target;
            this.matchSearch = matchSearch;
        }

        /// <summary>
        /// Finds the proxy types, indicating, for each type, its corresponding type from the target assembly.
        /// </summary>
        /// <returns>The pairs matching the proxy type to the target type.</returns>
        public IEnumerable<TypeMatch> FindTypes()
        {
            Contract.Ensures(Contract.Result<IEnumerable<TypeMatch>>() != null);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Provides the invariant contracts for the fields and properties of this object.
        /// </summary>
        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for code contracts.")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.proxy != null);
            Contract.Invariant(this.target != null);
            Contract.Invariant(this.matchSearch != null);
        }
    }
}