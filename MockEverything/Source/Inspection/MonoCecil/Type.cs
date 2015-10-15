// <copyright file="Type.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection.MonoCecil
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Reflection;
    using Mono.Cecil;
    using Mono.Collections.Generic;

    /// <summary>
    /// Represents a type from an assembly loaded using Mono.Cecil.
    /// </summary>
    [CLSCompliant(false)]
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
        /// Gets the generic types of the class. If the class is not generic, the enumeration yields no results.
        /// </summary>
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
        /// Compares this object to the specified object to determine if both represent the same type.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns><see langword="true"/> if the object represent the same type; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is IType))
            {
                return false;
            }

            var other = (IType)obj;
            return this.FullName == other.FullName && this.GenericTypes.SequenceEqual(other.GenericTypes);
        }

        /// <summary>
        /// Generates the hash code of this object.
        /// </summary>
        /// <returns>The hash code of the object.</returns>
        [ExcludeFromCodeCoverage]
        public override int GetHashCode()
        {
            var hash = 17;
            hash = (hash * 31) + this.FullName.GetHashCode();
            return hash;
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

            var match = this.FindAttribute(typeof(TAttribute).FullName);
            return this.InvokeConstructor<TAttribute>(match.ConstructorArguments);
        }

        /// <summary>
        /// Lists the values of the arguments which are expected to be passed to an attribute constructor.
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <returns>Zero or more objects.</returns>
        /// <exception cref="AttributeNotFoundException">The attribute cannot be found.</exception>
        public IEnumerable<object> FindAttributeValues<TAttribute>() where TAttribute : System.Attribute
        {
            Contract.Ensures(Contract.Result<IEnumerable<object>>() != null);

            var match = this.FindAttribute(typeof(TAttribute).FullName);
            return match.ConstructorArguments.Select(c => c.Value);
        }

        /// <summary>
        /// Finds the attribute of the specified type.
        /// </summary>
        /// <param name="fullName">The full name of the attribute.</param>
        /// <returns>The Mono.Cecil representation of the attribute.</returns>
        private CustomAttribute FindAttribute(string fullName)
        {
            Contract.Requires(fullName != null);
            Contract.Ensures(Contract.Result<CustomAttribute>() != null);

            var match = this.definition.CustomAttributes.SingleOrDefault(a => a.AttributeType.FullName == fullName);
            if (match == null)
            {
                throw new AttributeNotFoundException();
            }

            return match;
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
                attributeArguments.Select(argument => argument.Type.FullName),
                attributeArguments.Select(this.TransformCustomAttributeArgumentValue));
        }

        /// <summary>
        /// Transforms the value of a custom attribute argument, if needed, for instance by changing its type. This method should be used when loading the custom attribute arguments in order to use them to invoke a constructor.
        /// </summary>
        /// <param name="argument">The attribute argument structure containing the value to eventually transform.</param>
        /// <returns>The transformed value, or <c>argument.Value</c>.</returns>
        private object TransformCustomAttributeArgumentValue(CustomAttributeArgument argument)
        {
            if (argument.Type.FullName == typeof(System.Type).FullName)
            {
                // It seems that Mono.Cecil considers the case of types as special, and uses an instance of `Mono.Cecil.TypeDefinition` class as a value, instead of the expected `System.Type`. Once we start invoking a constructor, it then fails with `ArgumentException` because of this type mismatch. This happens with attributes such as `ProxyOf(typeof(Something))`.
                var wronglyTypedValue = (TypeDefinition)argument.Value;
                return new Type(wronglyTypedValue).ToSystemType();
            }

            return argument.Value;
        }

        /// <summary>
        /// Creates a <see cref="System.Type"/> representation of the current type.
        /// </summary>
        /// <returns>The <see cref="System.Type"/> representation of the current type.</returns>
        private System.Type ToSystemType()
        {
            Contract.Ensures(Contract.Result<System.Type>() != null);

            var assembly = System.Reflection.Assembly.Load(this.definition.Module.Assembly.FullName);
            return assembly.GetType(this.definition.FullName);
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

            var ownerProperty = this.FindPropertyOfGetterOrSetter(methodDefinition);
            return methodDefinition.CustomAttributes.Concat(ownerProperty.CustomAttributes);
        }

        /// <summary>
        /// Retrieves a property which contains a specific getter or setter.
        /// </summary>
        /// <param name="methodDefinition">The Mono.Cecil's method definition of a getter or a setter.</param>
        /// <returns>The matching property.</returns>
        /// <exception cref="System.InvalidOperationException">The property with this name doesn't exist.</exception>
        private Mono.Cecil.PropertyDefinition FindPropertyOfGetterOrSetter(Mono.Cecil.MethodDefinition methodDefinition)
        {
            Contract.Requires(methodDefinition != null);
            Contract.Requires(methodDefinition.IsGetter || methodDefinition.IsSetter);
            Contract.Ensures(Contract.Result<Mono.Cecil.PropertyDefinition>() != null);

            return this.definition.Properties.Single(property => property.GetMethod == methodDefinition || property.SetMethod == methodDefinition);
        }

        /// <summary>
        /// Determines whether two series of parameters should be considered as equal.
        /// </summary>
        /// <param name="first">The first series.</param>
        /// <param name="second">The second series.</param>
        /// <returns><see langword="true"/> if two series look equal; otherwise, <see langword="false"/>.</returns>
        private bool AreParametersEqual(IEnumerable<ParameterDefinition> first, IEnumerable<ParameterDefinition> second)
        {
            Contract.Requires(first != null);
            Contract.Requires(second != null);

            if (first.Any())
            {
                return first.Select(p => p.ParameterType).SequenceEqual(second.Select(p => p.ParameterType));
            }

            return !second.Any();
        }
    }
}