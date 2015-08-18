// <copyright file="MethodMatchSearch.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Browsers
{
    using System;
    using System.Diagnostics.Contracts;
    using Attributes;
    using Inspection;

    /// <summary>
    /// Represents a comparer which matches proxy types to target types.
    /// </summary>
    public class MethodMatchSearch : IMethodMatchSearch
    {
        /// <summary>
        /// Finds, within the target type, a type which corresponds to the proxy method.
        /// </summary>
        /// <param name="proxy">The proxy method.</param>
        /// <param name="targetType">The type expected to contain the target method.</param>
        /// <returns>The method from the target type which matches the specified proxy method.</returns>
        /// <exception cref="MatchNotFoundException">The match doesn't exist.</exception>
        public IMethod FindMatch(IMethod proxy, IType targetType)
        {
            throw new NotImplementedException();
        }
    }
}
