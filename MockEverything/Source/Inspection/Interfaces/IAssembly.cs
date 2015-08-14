// <copyright file="IAssembly.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents an assembly.
    /// </summary>
    public interface IAssembly
    {
        /// <summary>
        /// Finds all types in the assembly.
        /// </summary>
        /// <returns>Zero or more types.</returns>
        IEnumerable<IType> FindTypes();
    }
}
