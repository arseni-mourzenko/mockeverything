// <copyright file="ProxyMistakeException.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Runtime.Serialization;
    using System.Security.Permissions;

    /// <summary>
    /// Represents an exception thrown when a proxy assembly contains a pattern which may indicate a error.
    /// </summary>
    [Serializable]
    public class ProxyMistakeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyMistakeException"/> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public ProxyMistakeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyMistakeException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        [ExcludeFromCodeCoverage]
        public ProxyMistakeException(string message)
            : base(message)
        {
            Contract.Requires(message != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyMistakeException"/> class.
        /// </summary>
        /// <param name="message">A message that describes the current exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        [ExcludeFromCodeCoverage]
        public ProxyMistakeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyMistakeException"/> class with the specified serialization information and context.
        /// </summary>
        /// <param name="info">The data necessary to serialize or deserialize an object.</param>
        /// <param name="context">Description of the source and destination of the specified serialized stream.</param>
        [ExcludeFromCodeCoverage]
        protected ProxyMistakeException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="System.Runtime.Serialization.SerializationInfo"/> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="System.ArgumentNullException">The info parameter is a null reference (Nothing in Visual Basic).</exception>
        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        [ExcludeFromCodeCoverage]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}