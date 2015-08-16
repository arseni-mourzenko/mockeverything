// <copyright file="Type.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection.MonoCecil
{
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
        /// <param name="type">The type of the members to include in the result.</param>
        /// <param name="expectedAttributes">The types of attributes the methods to return should have.</param>
        /// <returns>Zero or more types.</returns>
        public IEnumerable<IMethod> FindTypes(MemberType type = MemberType.All, params System.Type[] expectedAttributes)
        {
            Contract.Ensures(Contract.Result<IEnumerable<IMethod>>() != null);

            return from definition in this.definition.Methods
                   where this.MatchTypeFilter(definition, type)
                   where this.MatchAttributesFilter(definition, expectedAttributes)
                   select new Method(definition);
        }

        /// <summary>
        /// Determines whether a method, specified by a Mono.Cecil's method definition, matches a filter.
        /// </summary>
        /// <param name="methodDefinition">The Mono.Cecil's method definition of a method.</param>
        /// <param name="filter">The filter which indicates which types should match.</param>
        /// <returns><see langword="true"/> if the method matches the filter; otherwise, <see langword="false"/>.</returns>
        private bool MatchTypeFilter(Mono.Cecil.MethodDefinition methodDefinition, MemberType filter)
        {
            Contract.Requires(methodDefinition != null);

            return filter.HasFlag(methodDefinition.IsStatic ? MemberType.Static : MemberType.Instance);
        }

        /// <summary>
        /// Determines whether a method, specified by a Mono.Cecil's type definition, contains all of the specified attributes.
        /// </summary>
        /// <remarks>
        /// If the type contains additional attributes, not specified in the list of attributes, it will have no effect on the result of this method.
        /// </remarks>
        /// <param name="methodDefinition">The Mono.Cecil's method definition of a type.</param>
        /// <param name="attributes">The types of attributes the method should contain.</param>
        /// <returns><see langword="true"/> if the method contains all of the specified attributes; otherwise, <see langword="false"/>.</returns>
        private bool MatchAttributesFilter(Mono.Cecil.MethodDefinition methodDefinition, ICollection<System.Type> attributes)
        {
            Contract.Requires(methodDefinition != null);
            Contract.Requires(attributes != null);

            var actualAttributes = new HashSet<string>(
                from a in methodDefinition.CustomAttributes select a.AttributeType.FullName);

            return attributes.All(attribute => actualAttributes.Contains(attribute.FullName));
        }
    }
}