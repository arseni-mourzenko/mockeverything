﻿// <copyright file="ITypeMatchSearch.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Browsers
{
    using Inspection;

    /// <summary>
    /// Represents a comparer which matches proxy types to target types.
    /// </summary>
    public interface ITypeMatchSearch : IMatchSearch<IType, IAssembly>
    {
    }
}
