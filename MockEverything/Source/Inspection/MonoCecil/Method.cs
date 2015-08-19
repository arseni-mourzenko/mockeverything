// <copyright file="Method.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection.MonoCecil
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Mono.Cecil;

    /// <summary>
    /// Represents a method from an assembly loaded using Mono.Cecil.
    /// </summary>
    public class Method : IMethod
    {
        /// <summary>
        /// The underlying definition of the method.
        /// </summary>
        private readonly MethodDefinition definition;

        /// <summary>
        /// Initializes a new instance of the <see cref="Method"/> class.
        /// </summary>
        /// <param name="definition">The Mono.Cecil definition of a method.</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "The arguments are validated through Code Contracts.")]
        public Method(MethodDefinition definition)
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
        /// Gets the parameters of the method.
        /// </summary>
        public IEnumerable<Parameter> Parameters
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<Parameter>>() != null);

                return this.definition.Parameters.Select(this.GenerateParameter);
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

        /// <summary>
        /// Gets the generic types of the method. If the method is not generic, the enumeration yields no results.
        /// </summary>
        /// <remarks>
        /// The generic aspect of the container class has no effect on the result.
        /// </remarks>
        public IEnumerable<string> GenericTypes
        {
            get
            {
                Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

                foreach (var parameter in this.definition.GenericParameters)
                {
                    yield return string.Join(",", this.DescribeGenericParameter(parameter).OrderBy(c => c));
                }
            }
        }

        /// <summary>
        /// Compares this object to the specified object to determine if both represent the same method.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><see langword="true"/> if the object represent the same method; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            var other = (Method)obj;
            return
                other.Name == this.Name &&
                other.ReturnType == this.ReturnType &&
                other.Parameters.SequenceEqual(this.Parameters) &&
                other.GenericTypes.SequenceEqual(this.GenericTypes);
        }

        /// <summary>
        /// Generates the hash code of this object.
        /// </summary>
        /// <returns>The hash code of the object.</returns>
        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 31 + this.Name.GetHashCode();
            hash = hash * 31 + this.ReturnType.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Creates a parameter from a Mono.Cecil definition of a parameter.
        /// </summary>
        /// <param name="definition">Mono.Cecil definition of a parameter.</param>
        /// <returns>The corresponding parameter object.</returns>
        private Parameter GenerateParameter(ParameterDefinition definition)
        {
            Contract.Requires(definition != null);
            Contract.Ensures(Contract.Result<Parameter>() != null);

            var variant = ParameterVariant.In;
            if (definition.IsOut)
            {
                variant = definition.IsIn ? ParameterVariant.Ref : ParameterVariant.Out;
            }

            return new Parameter(ParameterVariant.In, new Type(definition.ParameterType.Resolve()));
        }

        /// <summary>
        /// Enumerates the elements which will be part of the specification of a generic type.
        /// </summary>
        /// <param name="parameter">The generic parameter.</param>
        /// <returns>Zero or more elements to include in the specification.</returns>
        private IEnumerable<string> DescribeGenericParameter(GenericParameter parameter)
        {
            Contract.Requires(parameter != null);
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

            if (parameter.HasDefaultConstructorConstraint)
            {
                yield return "new()";
            }

            foreach (var constraint in parameter.Constraints)
            {
                yield return constraint.FullName;
            }
        }
    }
}
