// <copyright file="DirectoryAccess.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Discovery
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Inspection;
    using Inspection.MonoCecil;

    /// <summary>
    /// Represents directory access based on .NET Framework file system methods.
    /// </summary>
    public class DirectoryAccess : IDirectoryAccess
    {
        /// <summary>
        /// Lists the full paths of the files within a directory where the paths end by the specified suffix.
        /// </summary>
        /// <param name="directoryPath">The full path to the directory.</param>
        /// <param name="suffix">The suffix of the names.</param>
        /// <returns>Zero or more absolute paths.</returns>
        public IEnumerable<string> ListFilesEndingBy(string directoryPath, string suffix)
        {
            Contract.Requires(directoryPath != null);
            Contract.Requires(suffix != null);
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

            return Directory.EnumerateFiles(directoryPath, "*" + suffix);
        }

        /// <summary>
        /// Finds whether a file exists.
        /// </summary>
        /// <param name="fullPath">The full path to the file.</param>
        /// <returns><see langword="true"/> if the file exists; otherwise, <see langword="false"/>.</returns>
        public bool FileExists(string fullPath)
        {
            Contract.Requires(fullPath != null);

            return File.Exists(fullPath);
        }

        /// <summary>
        /// Loads an assembly from a file.
        /// </summary>
        /// <param name="fullPath">The full path to the file.</param>
        /// <returns>The assembly object.</returns>
        public IAssembly LoadAssembly(string fullPath)
        {
            Contract.Requires(fullPath != null);
            Contract.Ensures(Contract.Result<IAssembly>() != null);

            return new Assembly(fullPath);
        }
    }
}
