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
    using System.Reflection;
    using Mono.Cecil;

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
                Contract.Ensures(Contract.Result<string>() != null);
                Contract.Ensures(!Contract.Result<string>().Contains('.'));

                return this.definition.Name;
            }
        }

        /// <summary>
        /// Gets the full name of the type. This name contains a namespace, followed by a dot, followed by the short name. It doesn't mention the assembly.
        /// </summary>
        public string FullName
        {
            get
            {
                Contract.Ensures(Contract.Result<string>() != null);
                Contract.Ensures(Contract.Result<string>().Contains('.'));

                return this.definition.FullName;
            }
        }

        /// <summary>
        /// Finds all methods of the type.
        /// </summary>
        /// <param name="type">The type of the members to include in the result.</param>
        /// <param name="expectedAttributes">The types of attributes the methods to return should have.</param>
        /// <returns>Zero or more methods.</returns>
        public IEnumerable<IMethod> FindMethods(MemberType type = MemberType.All, params System.Type[] expectedAttributes)
        {
            Contract.Ensures(Contract.Result<IEnumerable<IMethod>>() != null);

            return from definition in this.definition.Methods
                   where this.MatchTypeFilter(definition, type)
                   where this.MatchAttributesFilter(definition, expectedAttributes)
                   select new Method(definition);
        }

        /// <summary>
        /// Finds the attribute of the specified type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns>The instance of the attribute.</returns>
        /// <exception cref="AttributeNotFoundException">The attribute cannot be found.</exception>
        public TAttribute FindAttribute<TAttribute>() where TAttribute : System.Attribute
        {
            Contract.Ensures(Contract.Result<TAttribute>() != null);

            var match = this.definition.CustomAttributes.SingleOrDefault(a => a.AttributeType.FullName == typeof(TAttribute).FullName);
            if (match == null)
            {
                throw new AttributeNotFoundException();
            }

            Contract.Assert(match != null);
            return this.InvokeConstructor<TAttribute>(match.ConstructorArguments);
        }

        /// <summary>
        /// Invokes, for a specific attribute, a constructor containing the specific arguments.
        /// </summary>
        /// <typeparam name="T">The type of the attribute containing the constructor.</typeparam>
        /// <param name="attributeArguments">The arguments to use when calling the constructor.</param>
        /// <returns>A new instance of the specified attribute.</returns>
        private T InvokeConstructor<T>(IEnumerable<CustomAttributeArgument> attributeArguments) where T : System.Attribute
        {
            Contract.Requires(attributeArguments != null);
            Contract.Ensures(Contract.Result<T>() != null);

            return this.InvokeConstructor<T>(
                from argument in attributeArguments select argument.Type.FullName,
                from argument in attributeArguments select argument.Value);
        }

        /// <summary>
        /// Invokes a constructor containing the specific arguments.
        /// </summary>
        /// <typeparam name="T">The type containing the constructor.</typeparam>
        /// <param name="argumentTypes">The full names of the types of arguments the constructor should take.</param>
        /// <param name="argumentValues">The values of the arguments to pass to the constructor.</param>
        /// <returns>A new instance of the specified class.</returns>
        private T InvokeConstructor<T>(IEnumerable<string> argumentTypes, IEnumerable<object> argumentValues)
        {
            Contract.Requires(argumentTypes != null);
            Contract.Requires(argumentValues != null);
            Contract.Requires(argumentTypes.Count() == argumentValues.Count());
            Contract.Ensures(Contract.Result<T>() != null);

            var constructor = typeof(T).GetConstructors().SingleOrDefault(c => this.FindTypesOfParameters(c).SequenceEqual(argumentTypes));
            if (constructor == null)
            {
                throw new TypeNotFoundException();
            }

            Contract.Assert(constructor != null);
            return (T)constructor.Invoke(argumentValues.ToArray());
        }

        /// <summary>
        /// Returns the full names of parameters required by a given constructor.
        /// </summary>
        /// <param name="info">The constructor info.</param>
        /// <returns>Zero or more full names.</returns>
        private IEnumerable<string> FindTypesOfParameters(ConstructorInfo info)
        {
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

            return from parameter in info.GetParameters() select parameter.ParameterType.FullName;
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
        /// <param name="methodDefinition">The Mono.Cecil's method definition of a method.</param>
        /// <param name="attributes">The types of attributes the method should contain.</param>
        /// <returns><see langword="true"/> if the method contains all of the specified attributes; otherwise, <see langword="false"/>.</returns>
        private bool MatchAttributesFilter(Mono.Cecil.MethodDefinition methodDefinition, ICollection<System.Type> attributes)
        {
            Contract.Requires(methodDefinition != null);
            Contract.Requires(attributes != null);

            var actualAttributes = new HashSet<string>(
                from a in this.FindAttributesOfMethod(methodDefinition) select a.FullName);

            return attributes.All(attribute => actualAttributes.Contains(attribute.FullName));
        }

        /// <summary>
        /// Finds all the attributes of a method, including the attributes of the corresponding property if the method is a getter or a setter.
        /// </summary>
        /// <param name="methodDefinition">The Mono.Cecil's method definition of a method.</param>
        /// <returns>Zero or more types if attributes. Duplicates are removed.</returns>
        private IEnumerable<Mono.Cecil.TypeReference> FindAttributesOfMethod(Mono.Cecil.MethodDefinition methodDefinition)
        {
            Contract.Requires(methodDefinition != null);
            Contract.Ensures(Contract.Result<IEnumerable<Mono.Cecil.TypeReference>>() != null);

            var inProperty = methodDefinition.IsGetter || methodDefinition.IsSetter;
            var attributes = inProperty ?
                this.FindAttributesOfGetterOrSetter(methodDefinition) :
                methodDefinition.CustomAttributes;

            return attributes.Select(a => a.AttributeType).Distinct();
        }

        /// <summary>
        /// Finds all the attributes of a getter or a setter, that is its own attributes and the ones of its property.
        /// </summary>
        /// <param name="methodDefinition">The Mono.Cecil's method definition of a getter or a setter.</param>
        /// <returns>Zero or more types if attributes. May include duplicates.</returns>
        private IEnumerable<Mono.Cecil.CustomAttribute> FindAttributesOfGetterOrSetter(Mono.Cecil.MethodDefinition methodDefinition)
        {
            Contract.Requires(methodDefinition != null);
            Contract.Requires(methodDefinition.IsGetter || methodDefinition.IsSetter);
            Contract.Ensures(Contract.Result<IEnumerable<Mono.Cecil.TypeReference>>() != null);

            var propertyName = methodDefinition.Name.Substring(4);
            return methodDefinition.CustomAttributes.Concat(this.FindPropertyByName(propertyName).CustomAttributes);
        }

        /// <summary>
        /// Retrieves a property by its name.
        /// </summary>
        /// <param name="name">The short name of the property. This name doesn't contain any reference to the type, namespace or assembly.</param>
        /// <returns>The matching property.</returns>
        /// <exception cref="System.InvalidOperationException">The property with this name doesn't exist.</exception>
        private Mono.Cecil.PropertyDefinition FindPropertyByName(string name)
        {
            Contract.Requires(name != null);
            Contract.Ensures(Contract.Result<Mono.Cecil.PropertyDefinition>() != null);

            return this.definition.Properties.Single(p => p.Name == name);
        }
    }
}