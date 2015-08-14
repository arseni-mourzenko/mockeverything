// <copyright file="Assembly.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection.MonoCecil
{
    using System;

    /// <summary>
    /// Represents a type from an assembly loaded using Mono.Cecil.
    /// </summary>
    public class Type : IType
    {
        /// <summary>
        /// The short name of the type. This name doesn't contain any reference to the namespace or the assembly.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Type"/> class.
        /// </summary>
        /// <param name="definition">The Mono.Cecil definition of a type.</param>
        public Type(Mono.Cecil.TypeDefinition definition)
        {
            this.name = definition.Name;
        }

        /// <summary>
        /// Gets the short name of the type. This name doesn't contain any reference to the namespace or the assembly.
        /// </summary>
        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }
}
