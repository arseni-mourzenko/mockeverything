// <copyright file="DefaultAssemblyResolver.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection.MonoCecil
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using Mono.Cecil;

    /// <summary>
    /// Represents the assembly resolver which searched for assemblies in the specified directories.
    /// </summary>
    internal class DefaultAssemblyResolver : BaseAssemblyResolver
    {
        /// <summary>
        /// The paths of directories to search.
        /// </summary>
        private readonly IEnumerable<string> paths;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultAssemblyResolver"/> class.
        /// </summary>
        /// <param name="paths">The paths of directories to search.</param>
        public DefaultAssemblyResolver(params string[] paths)
        {
            Contract.Requires(paths != null);

            this.paths = paths;
        }

        /// <summary>
        /// Attempts to resolve the assembly by searching for it in the specified directories.
        /// </summary>
        /// <param name="name">The reference name of the assembly.</param>
        /// <returns>The definition of the assembly.</returns>
        /// <exception cref="AssemblyResolutionException">The assembly wasn't found in any of the listed directories.</exception>
        public override AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            try
            {
                return base.Resolve(name);
            }
            catch (AssemblyResolutionException)
            {
                Trace.WriteLine("Second-chance attempt to resolve assembly " + name.FullName + "...");
                var definitions = from dirPath in this.paths
                                  let filePath = Path.Combine(dirPath, name.Name + ".dll")
                                  where File.Exists(filePath)
                                  let definition = AssemblyDefinition.ReadAssembly(filePath)
                                  where definition.FullName == name.FullName
                                  select definition;

                var match = definitions.SingleOrDefault();
                if (match != null)
                {
                    Trace.WriteLine("Assembly " + name.FullName + " was resolved.");
                    return match;
                }

                Trace.WriteLine("Failed to resolve assembly " + name.FullName + ".");
                throw;
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
            Contract.Invariant(this.paths != null);
        }
    }
}
