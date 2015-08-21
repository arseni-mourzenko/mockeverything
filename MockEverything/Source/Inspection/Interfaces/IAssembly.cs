// <copyright file="IAssembly.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an assembly.
    /// </summary>
    public interface IAssembly
    {
        /// <summary>
        /// Gets the full name of the assembly.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the full path to the file containing the assembly.
        /// </summary>
        string FilePath { get; }

        /// <summary>
        /// Finds all types in the assembly.
        /// </summary>
        /// <param name="type">The type of the members to include in the result.</param>
        /// <param name="expectedAttributes">The types of attributes the types to return should have.</param>
        /// <returns>Zero or more types.</returns>
        IEnumerable<IType> FindTypes(MemberType type = MemberType.All, params Type[] expectedAttributes);

        /// <summary>
        /// Finds a type with the specified name.
        /// </summary>
        /// <param name="fullName">The full name of the type, which contains the namespace, followed by a dot, followed by the short name of the type.</param>
        /// <returns>The corresponding type.</returns>
        /// <exception cref="TypeNotFoundException">The type with the specified time cannot be found.</exception>
        IType FindType(string fullName);

        /// <summary>
        /// Changes the version of the assembly by setting the specified one instead.
        /// </summary>
        /// <param name="version">The version to set.</param>
        void AlterVersion(Version version);

        /// <summary>
        /// Replaces the public key of the current assembly by the key of the specified model assembly.
        /// </summary>
        /// <remarks>
        /// <para>Implementers are allowed to accept only assemblies of the same type to be passed as a parameter, and throw <see cref="NotImplementedException"/> if the type of the model assemblies is different.</para>
        /// <para>Implementers should not set a contract requiring the model assembly to be of the same type, since it would violate the interface and would be difficult to impossible to enforce by the callers.</para>
        /// </remarks>
        /// <param name="model">The assembly containing the public key which should be copied to this assembly.</param>
        /// <exception cref="NotImplementedException">The type of the model assembly doesn't match the type of the current object.</exception>
        void ReplacePublicKey(IAssembly model);

        /// <summary>
        /// Saves the assembly to a file.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        void Save(string path);
    }
}
