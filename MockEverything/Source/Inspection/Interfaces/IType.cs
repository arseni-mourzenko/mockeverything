// <copyright file="IType.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a type from an assembly.
    /// </summary>
    public interface IType
    {
        /// <summary>
        /// Gets the short name of the type. This name doesn't contain any reference to the namespace or the assembly.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Finds all types in the assembly.
        /// </summary>
        /// <param name="type">The type of the members to include in the result.</param>
        /// <param name="expectedAttributes">The types of attributes the methods to return should have.</param>
        /// <returns>Zero or more types.</returns>
        IEnumerable<IMethod> FindTypes(MemberType type = MemberType.All, params Type[] expectedAttributes);
    }
}
