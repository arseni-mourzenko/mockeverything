// <copyright file="DefaultAssemblyResolver.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection.MonoCecil
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using Mono.Cecil;

    internal class DefaultAssemblyResolver : BaseAssemblyResolver
    {
        private readonly IEnumerable<string> paths;

        public DefaultAssemblyResolver(params string[] paths)
        {
            this.paths = paths;
        }

        public override AssemblyDefinition Resolve(AssemblyNameReference name)
        {
            try
            {
                return base.Resolve(name);
            }
            catch (AssemblyResolutionException)
            {
                Trace.WriteLine("Trying to resolve assembly " + name.FullName);
                foreach (var path in this.paths)
                {
                    var filePath = Path.Combine(path, name.Name + ".dll");
                    Trace.WriteLine("Searching for " + filePath);
                    if (File.Exists(filePath))
                    {
                        var definition = AssemblyDefinition.ReadAssembly(filePath);
                        Trace.WriteLine("Comparing to " + definition.FullName);
                        if (definition.FullName == name.FullName)
                        {
                            return definition;
                        }
                    }
                }

                throw;
            }
        }
    }
}
