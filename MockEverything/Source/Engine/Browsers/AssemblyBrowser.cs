// <copyright file="AssemblyBrowser.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Browsers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Inspection;

    /// <summary>
    /// Represents a browser which is able to find the proxy types in a proxy assembly, matching them with the corresponding types from the target assembly.
    /// </summary>
    public class AssemblyBrowser
    {
        /// <summary>
        /// The proxy assembly.
        /// </summary>
        private readonly IAssembly proxy;

        /// <summary>
        /// The corresponding target assembly.
        /// </summary>
        private readonly IAssembly target;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyBrowser"/> class.
        /// </summary>
        /// <param name="proxy">The proxy assembly.</param>
        /// <param name="target">The corresponding target assembly.</param>
        public AssemblyBrowser(IAssembly proxy, IAssembly target)
        {
            Contract.Requires(proxy != null);
            Contract.Requires(target != null);

            this.proxy = proxy;
            this.target = target;
        }

        /// <summary>
        /// Finds the proxy types, indicating, for each type, its corresponding type from the target assembly.
        /// </summary>
        /// <returns>The pairs matching the proxy type to the target type.</returns>
        public IEnumerable<TypeMatch> FindTypes()
        {
            Contract.Ensures(Contract.Result<IEnumerable<TypeMatch>>() != null);

            throw new NotImplementedException();
        }
    }
}