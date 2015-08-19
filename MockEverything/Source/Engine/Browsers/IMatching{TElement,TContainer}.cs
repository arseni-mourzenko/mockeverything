// <copyright file="IMatching{TElement,TContainer}.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Browsers
{
    /// <summary>
    /// Represents a comparer which matches proxy elements to target elements.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements to match.</typeparam>
    /// <typeparam name="TContainer">The type of the container which is used to search for matches.</typeparam>
    public interface IMatching<TElement, TContainer>
    {
        /// <summary>
        /// Finds, within the proxy container, an element which corresponds to the target element.
        /// </summary>
        /// <param name="proxy">The proxy element.</param>
        /// <param name="targetContainer">The container expected to contain the target element.</param>
        /// <returns>The element from the target container which matches the specified target element.</returns>
        /// <exception cref="MatchNotFoundException">The match doesn't exist.</exception>
        TElement FindMatch(TElement proxy, TContainer targetContainer);
    }
}
