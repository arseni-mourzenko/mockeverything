// <copyright file="Method.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection.MonoCecil
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents a method from an assembly loaded using Mono.Cecil.
    /// </summary>
    public class Method : IMethod
    {
        /// <summary>
        /// The underlying definition of the method.
        /// </summary>
        private readonly Mono.Cecil.MethodDefinition definition;

        /// <summary>
        /// Initializes a new instance of the <see cref="Method"/> class.
        /// </summary>
        /// <param name="definition">The Mono.Cecil definition of a method.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "The arguments are validated through Code Contracts.")]
        public Method(Mono.Cecil.MethodDefinition definition)
        {
            Contract.Requires(definition != null);

            this.definition = definition;
        }

        /// <summary>
        /// Gets the short name of the method. This name doesn't contain any reference to the type, namespace or assembly.
        /// </summary>
        public string Name
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);

                return this.definition.Name;
            }
        }

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        public IType ReturnType
        {
            get
            {
                Contract.Ensures(Contract.Result<IType>() != null);

                return new Type(this.definition.ReturnType.Resolve());
            }
        }
    }
}
