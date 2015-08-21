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
    using Mono.Collections.Generic;

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
                    yield return string.Join(",", parameter.Describe().OrderBy(c => c));
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
            if (obj == null || !(obj is IMethod))
            {
                return false;
            }

            var other = (IMethod)obj;
            return
                this.Name == other.Name &&
                this.ReturnType.Equals(other.ReturnType) &&
                this.Parameters.SequenceEqual(other.Parameters) &&
                this.GenericTypes.SequenceEqual(other.GenericTypes);
        }

        /// <summary>
        /// Generates the hash code of this object.
        /// </summary>
        /// <returns>The hash code of the object.</returns>
        public override int GetHashCode()
        {
            var hash = 17;
            hash = (hash * 31) + this.Name.GetHashCode();
            hash = (hash * 31) + this.ReturnType.GetHashCode();
            return hash;
        }

        /// <summary>
        /// Replaces the body of this method by a body of the specified one.
        /// </summary>
        /// <param name="other">The method to use as a replacement.</param>
        /// <exception cref="System.NotImplementedException">The other method is not an instance of the <see cref="Method"/> class.</exception>
        public void ReplaceBody(IMethod other)
        {
            Contract.Requires(other != null);

            var otherMethod = other as Method;
            if (otherMethod == null)
            {
                throw new System.NotImplementedException();
            }

            this.ReplaceBody(otherMethod.definition);
        }

        /// <summary>
        /// Replaces the body of this method by a body of the specified method definition.
        /// </summary>
        /// <param name="definition">The method to use as a replacement.</param>
        private void ReplaceBody(MethodDefinition definition)
        {
            Contract.Requires(definition != null);

            var source = definition.Body;
            var destination = this.definition.Body;

            this.ReplaceCollectionContents(source.Variables, destination.Variables);
            this.ReplaceCollectionContents(source.ExceptionHandlers, destination.ExceptionHandlers);
            this.ReplaceCollectionContents(source.Instructions, destination.Instructions);
        }

        /// <summary>
        /// Replaces the contents of a collection by the contents of a sequence.
        /// </summary>
        /// <typeparam name="TElement">The type of the elements in the collection.</typeparam>
        /// <param name="source">The source sequence.</param>
        /// <param name="destination">The collection to modify.</param>
        private void ReplaceCollectionContents<TElement>(IEnumerable<TElement> source, Collection<TElement> destination)
        {
            Contract.Requires(source != null);
            Contract.Requires(destination != null);

            while (destination.Any())
            {
                destination.RemoveAt(0);
            }

            foreach (var element in source)
            {
                destination.Add(element);
            }
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
                variant = ParameterVariant.Out;
            }
            else if (definition.ParameterType.IsByReference)
            {
                variant = ParameterVariant.Ref;
            }
            else if (definition.CustomAttributes.Any(a => a.AttributeType.FullName == typeof(System.ParamArrayAttribute).FullName))
            {
                variant = ParameterVariant.Params;
            }

            return new Parameter(variant, new Type(definition.ParameterType.Resolve()));
        }
    }
}