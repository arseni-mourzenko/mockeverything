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
        /// Gets the full name of the type. This name contains a namespace, followed by a dot, followed by the short name. It doesn't mention the assembly.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the generic types of the class. If the class is not generic, the enumeration yields no results.
        /// </summary>
        IEnumerable<string> GenericTypes { get; }

        /// <summary>
        /// Finds all methods of the type.
        /// </summary>
        /// <param name="type">The type of the members to include in the result.</param>
        /// <param name="expectedAttributes">The types of attributes the methods to return should have.</param>
        /// <returns>Zero or more methods.</returns>
        IEnumerable<IMethod> FindMethods(MemberType type = MemberType.All, params Type[] expectedAttributes);

        /// <summary>
        /// Finds the attribute of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns>The instance of the attribute.</returns>
        /// <exception cref="AttributeNotFoundException">The attribute cannot be found.</exception>
        TAttribute FindAttribute<TAttribute>() where TAttribute : System.Attribute;
    }
}