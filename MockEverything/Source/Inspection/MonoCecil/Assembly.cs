// <copyright file="Assembly.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection.MonoCecil
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Represents an assembly loaded using Mono.Cecil.
    /// </summary>
    public class Assembly : IAssembly
    {
        /// <summary>
        /// The full path to the file containing the assembly.
        /// </summary>
        private readonly string path;

        /// <summary>
        /// The underlying Mono.Cecil assembly instance, loaded on demand.
        /// </summary>
        private readonly Lazy<Mono.Cecil.AssemblyDefinition> underlyingAssembly;

        /// <summary>
        /// Initializes a new instance of the <see cref="Assembly"/> class.
        /// </summary>
        /// <param name="path">The full path to the file containing the assembly.</param>
        public Assembly(string path)
        {
            this.path = path;
            this.underlyingAssembly = new Lazy<Mono.Cecil.AssemblyDefinition>(() => Mono.Cecil.AssemblyDefinition.ReadAssembly(this.path));
        }

        /// <summary>
        /// Finds all types in the assembly.
        /// </summary>
        /// <param name="type">The type of the members to include in the result.</param>
        /// <returns>Zero or more types.</returns>
        public IEnumerable<IType> FindTypes(MemberType type = MemberType.All)
        {
            Contract.Ensures(Contract.Result<IEnumerable<IType>>() != null);

            return from definition in this.FindTypeDefinitions()
                   where this.MatchTypeFilter(definition, type)
                   select new Type(definition);
        }

        /// <summary>
        /// Finds all the types within an assembly and returns them under a form of Mono.Cecil's definitions of types.
        /// </summary>
        /// <returns>Zero or more definitions of types.</returns>
        private IEnumerable<Mono.Cecil.TypeDefinition> FindTypeDefinitions()
        {
            Contract.Ensures(Contract.Result<IEnumerable<Mono.Cecil.TypeDefinition>>() != null);

            return this.underlyingAssembly
                .Value
                .Modules
                .SelectMany(t => t.Types);
        }

        /// <summary>
        /// Determines whether a type, specified by a Mono.Cecil's type definition, matches a filter.
        /// </summary>
        /// <param name="typeDefinition">The Mono.Cecil's type definition of a type.</param>
        /// <param name="filter">The filter which indicates which types should match.</param>
        /// <returns><see langword="true"/> if the type matches the filter; otherwise, <see langword="false"/>.</returns>
        private bool MatchTypeFilter(Mono.Cecil.TypeDefinition typeDefinition, MemberType filter)
        {
            Contract.Requires(typeDefinition != null);

            return filter.HasFlag(this.IsTypeStatic(typeDefinition) ? MemberType.Static : MemberType.Instance);
        }

        /// <summary>
        /// Determines whether a type, specified by a Mono.Cecil's type definition, is static.
        /// </summary>
        /// <param name="typeDefinition">The Mono.Cecil's type definition of a type.</param>
        /// <returns><see langword="true"/> if the type is static; otherwise, <see langword="false"/>.</returns>
        private bool IsTypeStatic(Mono.Cecil.TypeDefinition typeDefinition)
        {
            Contract.Requires(typeDefinition != null);

            // There is no notion of static classes at IL level. A static class is simply an abstract sealed class.
            return typeDefinition.IsAbstract && typeDefinition.IsSealed;
        }
    }
}
