// <copyright file="IMethod.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a type from an assembly.
    /// </summary>
    public interface IMethod
    {
        /// <summary>
        /// Gets the short name of the method. This name doesn't contain any reference to the type, namespace or assembly.
        /// </summary>
        string Name { get; }
    }
}
