// <copyright file="InstanceProxyException.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using Inspection;

    /// <summary>
    /// Represents an exception thrown when a proxy assembly contains non-static proxy classes.
    /// </summary>
    [Serializable]
    public class InstanceProxyException : Exception
    {
        /// <summary>
        /// The full names of the instance types which are declared as proxies.
        /// </summary>
        private readonly ICollection<string> namesOfTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProxyException"/> class.
        /// </summary>
        public InstanceProxyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProxyException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InstanceProxyException(string message)
            : base(message)
        {
            Contract.Requires(message != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProxyException"/> class containing specified types which were problematic.
        /// </summary>
        /// <param name="types">The problematic types.</param>
        public InstanceProxyException(ICollection<IType> types)
        {
            Contract.Requires(types != null);
            Contract.Requires(types.Count > 0);

            this.namesOfTypes = types.Select(t => t.FullName).ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProxyException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the current exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        [ExcludeFromCodeCoverage]
        public InstanceProxyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProxyException"/> class with the specified serialization information and context.
        /// </summary>
        /// <param name="info">The data necessary to serialize or deserialize an object.</param>
        /// <param name="context">Description of the source and destination of the specified serialized stream.</param>
        protected InstanceProxyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            if (info != null)
            {
                this.namesOfTypes = (ICollection<string>)info.GetValue("types", typeof(ICollection<string>));
            }
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        [Pure]
        public override string Message
        {
            get
            {
                if (this.namesOfTypes == null)
                {
                    return base.Message;
                }

                if (this.namesOfTypes.Count == 1)
                {
                    return string.Format("The proxy assembly contains the following instance class declared as proxy: {0}. Either declare this class as static or remove the proxy attribute.", this.namesOfTypes.Single());
                }

                return string.Format("The proxy assembly contains the following instance classes declared as proxies: {0}. Either declare those classes as static or remove the proxy attribute.", string.Join(", ", this.namesOfTypes));
            }
        }

        /// <summary>
        /// Gets the instance types which are declared as proxies.
        /// </summary>
        public IEnumerable<string> NamesOfTypes
        {
            get
            {
                return this.namesOfTypes;
            }
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The info parameter is a null reference (Nothing in Visual Basic).</exception>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            if (info != null)
            {
                info.AddValue("types", this.namesOfTypes);
            }
        }

        /// <summary>
        /// Provides the invariant contracts for the fields and properties of this object.
        /// </summary>
        [ContractInvariantMethod]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Required for code contracts.")]
        private void ObjectInvariant()
        {
            Contract.Invariant(this.namesOfTypes == null || this.namesOfTypes.Count > 0);
        }
    }
}
