// <copyright file="ITampering.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Tampering
{
    using System;
    using Browsers;
    using Inspection;

    /// <summary>
    /// Represents the tampering which executes the steps required to create a tampered assembly from a proxy and a target.
    /// </summary>
    public interface ITampering
    {
        /// <summary>
        /// Gets or sets the pair of proxy and target assemblies.
        /// </summary>
        Pair<IAssembly> Pair { get; set; }

        /// <summary>
        /// Gets or sets the version to set to the resulting assembly. If <see langword="null"/>, the version won't be changed and will correspond to the version of the target assembly.
        /// </summary>
        Version ResultVersion { get; set; }

        /// <summary>
        /// Merges the proxy and the target assemblies and tampers the resulting one.
        /// </summary>
        /// <returns>The resulting assembly.</returns>
        IAssembly Tamper();
    }
}