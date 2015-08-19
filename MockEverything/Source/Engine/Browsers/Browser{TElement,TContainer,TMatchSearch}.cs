// <copyright file="Browser{TElement,TContainer,TMatchSearch}.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Browsers
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Represents a browser which is able to find the proxy entries in a proxy container, matching them with the corresponding entries from the target container.
    /// </summary>
    /// <typeparam name="TElement">The type of the element which has to be found within the container.</typeparam>
    /// <typeparam name="TContainer">The type of the container containing the elements.</typeparam>
    /// <typeparam name="TMatchSearch">The type of the matching mechanism which is able to determine that a given proxy element corresponds to a given target element.</typeparam>
    public abstract class Browser<TElement, TContainer, TMatchSearch>
        where TMatchSearch : IMatchSearch<TElement, TContainer>
    {
        /// <summary>
        /// The proxy container.
        /// </summary>
        private readonly TContainer proxy;

        /// <summary>
        /// The corresponding target container.
        /// </summary>
        private readonly TContainer target;

        /// <summary>
        /// The searcher which, for every proxy type found, finds the corresponding target type.
        /// </summary>
        private readonly TMatchSearch matchSearch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Browser{TElement,TContainer,TMatchSearch}"/> class.
        /// </summary>
        /// <param name="proxy">The proxy assembly.</param>
        /// <param name="target">The corresponding target assembly.</param>
        /// <param name="matchSearch">The searcher which, for every proxy type found, finds the corresponding target type.</param>
        protected Browser(TContainer proxy, TContainer target, TMatchSearch matchSearch)
        {
            Contract.Requires(proxy != null);
            Contract.Requires(target != null);
            Contract.Requires(matchSearch != null);

            this.proxy = proxy;
            this.target = target;
            this.matchSearch = matchSearch;
        }

        /// <summary>
        /// Gets the proxy container.
        /// </summary>
        protected TContainer Proxy
        {
            get
            {
                Contract.Ensures(Contract.Result<TContainer>() != null);

                return this.proxy;
            }
        }

        /// <summary>
        /// Finds the proxy types, indicating, for each type, its corresponding type from the target assembly.
        /// </summary>
        /// <returns>The pairs matching the proxy type to the target type.</returns>
        public IEnumerable<Pair<TElement>> FindTypes()
        {
            Contract.Ensures(Contract.Result<IEnumerable<Pair<TElement>>>() != null);

            return this.FindAllInProxy().Select(this.CreateMatch);
        }

        /// <summary>
        /// Lists all the entries from the proxy container which are proxies themselves.
        /// </summary>
        /// <returns>Zero or more elements.</returns>
        protected abstract IEnumerable<TElement> FindAllInProxy();

        /// <summary>
        /// Creates a pair of matching types.
        /// </summary>
        /// <param name="proxyType">The proxy type.</param>
        /// <returns>A pair of matching types.</returns>
        private Pair<TElement> CreateMatch(TElement proxyType)
        {
            Contract.Requires(proxyType != null);
            Contract.Ensures(Contract.Result<Pair<TElement>>() != null);

            var targetType = this.matchSearch.FindMatch(proxyType, this.target);
            Contract.Assume(targetType != null);
            return new Pair<TElement>(proxyType, targetType);
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
