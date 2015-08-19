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
        public IType Type
        {
            get
            {
                Contract.Ensures(Contract.Result<IType>() != null);

                return this.type;
            }
        }

        /// <summary>
        /// Compares this object to the specified object to determine if both represent the same parameter.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><see langword="true"/> if the object represent the same parameter; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            var other = (Parameter)obj;
            return other.Variant == this.Variant && this.Type.Equals(other.Type);
        }

        /// <summary>
        /// Generates the hash code of this object.
        /// </summary>
        /// <returns>The hash code of the object.</returns>
        public override int GetHashCode()
        {
            var hash = 17;
            hash = (hash * 31) + this.Variant.GetHashCode();
            hash = (hash * 31) + this.Type.GetHashCode();
            return hash;
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