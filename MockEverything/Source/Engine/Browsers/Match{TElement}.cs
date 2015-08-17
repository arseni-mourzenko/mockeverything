// <copyright file="Match{TElement}.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Browsers
{
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a match between an element from the proxy assembly with an element from the target assembly.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements to match.</typeparam>
    public class Match<TElement>
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
        /// Initializes a new instance of the <see cref="Match{TElement}"/> class.
        /// </summary>
        /// <param name="proxy">The proxy element.</param>
        /// <param name="target">The corresponding target element.</param>
        public Match(TElement proxy, TElement target)
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
    }
}