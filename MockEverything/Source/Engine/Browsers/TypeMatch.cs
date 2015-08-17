// <copyright file="TypeMatch.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Browsers
{
    using System.Diagnostics.Contracts;
    using Inspection;

    /// <summary>
    /// Represents a match between a type from the proxy assembly with a type from the target assembly.
    /// </summary>
    public class TypeMatch : Match<IType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMatch"/> class.
        /// </summary>
        /// <param name="proxy">The proxy type.</param>
        /// <param name="target">The corresponding target type.</param>
        public TypeMatch(IType proxy, IType target) : base(proxy, target)
        {
            Contract.Requires(proxy != null);
            Contract.Requires(target != null);
        }
    }
}
