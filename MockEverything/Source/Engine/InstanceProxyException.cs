// <copyright file="InstanceProxyException.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using Inspection;

    /// <summary>
    /// Represents an exception thrown when a proxy assembly contains non-static proxy classes.
    /// </summary>
    public class InstanceProxyException : Exception
    {
        /// <summary>
        /// The custom error message.
        /// </summary>
        private readonly string message;

        /// <summary>
        /// The instance types which are declared as proxies.
        /// </summary>
        private readonly IEnumerable<IType> types;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProxyException"/> class.
        /// </summary>
        public InstanceProxyException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProxyException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public InstanceProxyException(string message) : base(message)
        {
            Contract.Requires(message != null);

            this.message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceProxyException"/> class containing specified types which were problematic.
        /// </summary>
        /// <param name="types">The problematic types.</param>
        public InstanceProxyException(IEnumerable<IType> types)
        {
            Contract.Requires(types != null);

            this.types = types;
        }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public override string Message
        {
            get
            {
                if (this.message != null)
                {
                    return this.message;
                }

                if (this.types != null)
                {
                    var single = this.types.SingleOrDefault();
                    if (single != null)
                    {
                        return string.Format("The proxy assembly contains the following instance class declared as proxy: {0}. Either declare this class as static or remove the proxy attribute.", single.FullName);
                    }

                    return string.Format("The proxy assembly contains the following instance classes declared as proxies: {0}. Either declare those classes as static or remove the proxy attribute.", string.Join(", ", this.types.Select(c => c.FullName)));
                }

                return base.Message;
            }
        }

        /// <summary>
        /// Gets the instance types which are declared as proxies.
        /// </summary>
        public IEnumerable<IType> Types
        {
            get
            {
                return this.types;
            }
        }
    }
}
