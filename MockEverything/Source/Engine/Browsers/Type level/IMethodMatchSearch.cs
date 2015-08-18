﻿// <copyright file="IMethodMatchSearch.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Browsers
{
    using Inspection;

    /// <summary>
    /// Represents a comparer which matches proxy methods to target methods.
    /// </summary>
    public interface IMethodMatchSearch : IMatchSearch<IMethod, IType>
    {
    }
}
