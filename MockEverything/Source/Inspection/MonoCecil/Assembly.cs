// <copyright file="Assembly.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection.MonoCecil
{
    using System;
    using System.Collections.Generic;
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
        /// <returns>Zero or more types.</returns>
        public IEnumerable<IType> FindTypes()
        {
            return this.underlyingAssembly.Value.Modules.SelectMany(t => t.Types).Select(t => new Type(t));
        }
    }
}
