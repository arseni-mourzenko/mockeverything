// <copyright file="ProxyMethodAttribute.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Attributes
{
    using System;

    /// <summary>
    /// Represents an attribute which indicates that a constructor, method or property is a proxy of another constructor, method or property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property)]
    public class ProxyMethodAttribute : Attribute
    {
        /// <summary>
        /// The type of the target method.
        /// </summary>
        private readonly TargetMethodType methodType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyMethodAttribute"/> class.
        /// </summary>
        /// <param name="methodType">The type of the target method.</param>
        public ProxyMethodAttribute(TargetMethodType methodType)
        {
            this.methodType = methodType;
        }

        /// <summary>
        /// Gets the type of the target method.
        /// </summary>
        public TargetMethodType MethodType
        {
            get
            {
                return this.methodType;
            }
        }
    }
}
