// <copyright file="Type.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection.MonoCecil
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;

    /// <summary>
    /// Represents a type from an assembly loaded using Mono.Cecil.
    /// </summary>
    public class Type : IType
    {
        /// <summary>
        /// The Mono.Cecil definition of a type.
        /// </summary>
        private readonly Mono.Cecil.TypeDefinition definition;

        /// <summary>
        /// Initializes a new instance of the <see cref="Type"/> class.
        /// </summary>
        /// <param name="definition">The Mono.Cecil definition of a type.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "The arguments are validated through Code Contracts.")]
        public Type(Mono.Cecil.TypeDefinition definition)
        {
            Contract.Requires(definition != null);

            this.definition = definition;
        }

        /// <summary>
        /// Gets the short name of the type. This name doesn't contain any reference to the namespace or the assembly.
        /// </summary>
        public string Name
        {
            get
            {
                return this.definition.Name;
            }
        }

        /// <summary>
        /// Finds all types in the assembly.
        /// </summary>
        /// <returns>Zero or more types.</returns>
        public IEnumerable<IMethod> FindTypes()
        {
            return this.definition.Methods.Select(m => new Method(m));
        }
    }
}