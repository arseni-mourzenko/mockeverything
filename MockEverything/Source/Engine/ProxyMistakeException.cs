// <copyright file="ProxyMistakeException.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine
{
    using System;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// Represents an exception thrown when a proxy assembly contains a pattern which may indicate a error.
    /// </summary>
    public class ProxyMistakeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProxyMistakeException"/> class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public ProxyMistakeException(string message) : base(message)
        {
            Contract.Requires(message != null);
        }
    }
}