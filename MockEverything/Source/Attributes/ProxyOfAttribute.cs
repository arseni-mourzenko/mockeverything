// <copyright file="ProxyOfAttribute.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Attributes
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents an attribute which indicates that a class is a proxy of another class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class ProxyOfAttribute : Attribute
    {
        /// <summary>
        /// The type of the class affected by the proxy.
        /// </summary>
        private readonly Type targetType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyOfAttribute"/> class.
        /// </summary>
        /// <param name="targetType">The type of the class affected by the proxy.</param>
        public ProxyOfAttribute(Type targetType)
        {
            Contract.Requires(targetType != null);

            this.targetType = targetType;
        }

        /// <summary>
        /// Gets the type of the class affected by the proxy.
        /// </summary>
        public Type TargetType
        {
            get
            {
                Contract.Ensures(Contract.Result<Type>() != null);

                return this.targetType;
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
            Contract.Invariant(this.targetType != null);
        }
    }
}
