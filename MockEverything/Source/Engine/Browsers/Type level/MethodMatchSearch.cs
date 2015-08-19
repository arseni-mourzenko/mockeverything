// <copyright file="MethodMatchSearch.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Browsers
{
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Inspection;

    /// <summary>
    /// Represents a comparer which matches proxy types to target types.
    /// </summary>
    public class MethodMatchSearch : IMatching<IMethod, IType>
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
            Contract.Requires(proxy != null);
            Contract.Requires(targetType != null);
            Contract.Ensures(Contract.Result<IMethod>() != null);

            var match = targetType.FindMethods().SingleOrDefault(m => this.IsMatch(m, proxy));
            if (match == null)
            {
                throw new MatchNotFoundException();
            }

            return match;
        }

        /// <summary>
        /// Checks whether the two methods are a match, in other words that one can be a proxy of another.
        /// </summary>
        /// <param name="first">The first method.</param>
        /// <param name="second">The second method.</param>
        /// <returns><see langword="true"/> if there is a match; otherwise, <see langword="false"/>.</returns>
        private bool IsMatch(IMethod first, IMethod second)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);

            return
                first.Name == second.Name &&
                first.ReturnType.FullName == second.ReturnType.FullName &&
                first.GenericTypes.SequenceEqual(second.GenericTypes);
        }
    }
}
