// <copyright file="Pair{TElement}.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Browsers
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a pair of elements, one from the proxy container, the other from the target container.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements which form a pair.</typeparam>
    public class Pair<TElement>
    {
        /// <summary>
        /// The proxy element.
        /// </summary>
        private readonly TElement proxy;

        /// <summary>
        /// The corresponding target element.
        /// </summary>
        private readonly TElement target;

        /// <summary>
        /// Initializes a new instance of the <see cref="Pair{TElement}"/> class.
        /// </summary>
        /// <param name="proxy">The proxy element.</param>
        /// <param name="target">The corresponding target element.</param>
        public Pair(TElement proxy, TElement target)
        {
            Contract.Requires(proxy != null);
            Contract.Requires(target != null);

            this.proxy = proxy;
            this.target = target;
        }

        /// <summary>
        /// Gets the proxy element.
        /// </summary>
        public TElement Proxy
        {
            get
            {
                Contract.Ensures(Contract.Result<TElement>() != null);

                return this.proxy;
            }
        }

        /// <summary>
        /// Gets the corresponding target element.
        /// </summary>
        public TElement Target
        {
            get
            {
                Contract.Ensures(Contract.Result<TElement>() != null);

                return this.target;
            }
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
        }
    }
}