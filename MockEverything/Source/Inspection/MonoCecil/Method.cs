﻿// <copyright file="Method.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection.MonoCecil
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a method from an assembly loaded using Mono.Cecil.
    /// </summary>
    public class Method : IMethod
    {
        /// <summary>
        /// The short name of the method. This name doesn't contain any reference to the type, namespace or assembly.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the <see cref="Method"/> class.
        /// </summary>
        /// <param name="definition">The Mono.Cecil definition of a method.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "The arguments are validated through Code Contracts.")]
        public Method(Mono.Cecil.MethodDefinition definition)
        {
            Contract.Requires(definition != null);

            this.name = definition.Name;
        }

        /// <summary>
        /// Gets the short name of the method. This name doesn't contain any reference to the type, namespace or assembly.
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
