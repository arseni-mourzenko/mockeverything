// <copyright file="Parameter.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection
{
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents the method parameter.
    /// </summary>
    public class Parameter
    {
        /// <summary>
        /// The variant of the parameter which differentiates <c>in</c>, <c>out</c>, <c>ref</c> <c>and params</c> parameters.
        /// </summary>
        private readonly ParameterVariant variant;

        /// <summary>
        /// The type of the parameter.
        /// </summary>
        private readonly IType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameter"/> class.
        /// </summary>
        /// <param name="variant">The variant of the parameter which differentiates <c>in</c>, <c>out</c>, <c>ref</c> <c>and params</c> parameters.</param>
        /// <param name="type">The type of the parameter.</param>
        public Parameter(ParameterVariant variant, IType type)
        {
            Contract.Requires(type != null);

            this.variant = variant;
            this.type = type;
        }

        /// <summary>
        /// Gets the variant of the parameter which differentiates <c>in</c>, <c>out</c>, <c>ref</c> <c>and params</c> parameters.
        /// </summary>
        public ParameterVariant Variant
        {
            get
            {
                return this.variant;
            }
        }

        /// <summary>
        /// Gets the type of the parameter.
        /// </summary>
        private IType Type
        {
            get
            {
                Contract.Ensures(Contract.Result<IType>() != null);

                return this.type;
            }
        }

        /// <summary>
        /// Provides the invariant contracts for the fields and properties of this object.
        /// </summary>
        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Required for code contracts.")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.type != null);
        }
    }
}