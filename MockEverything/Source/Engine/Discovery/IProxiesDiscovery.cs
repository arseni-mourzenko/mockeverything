// <copyright file="IProxiesDiscovery.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Discovery
{
    using System.Collections.Generic;
    using Browsers;
    using Inspection;

    /// <summary>
    /// Represents a discovery of the proxy assemblies.
    /// </summary>
    public interface IProxiesDiscovery
    {
        /// <summary>
        /// Lists the pairs, each pair containing a proxy assembly and its corresponding target assembly.
        /// </summary>
        /// <returns>Zero or more pairs.</returns>
        IEnumerable<Pair<IAssembly>> FindAssemblies();
    }
}
