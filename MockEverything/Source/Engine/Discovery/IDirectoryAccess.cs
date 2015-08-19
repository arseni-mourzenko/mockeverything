// <copyright file="IDirectoryAccess.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Engine.Discovery
{
    using System.Collections.Generic;
    using Inspection;

    /// <summary>
    /// Represents directory access which makes it possible to perform basic operations.
    /// </summary>
    public interface IDirectoryAccess
    {
        /// <summary>
        /// Lists the full paths of the files within a directory where the paths end by the specified suffix.
        /// </summary>
        /// <param name="directoryPath">The full path to the directory.</param>
        /// <param name="suffix">The suffix of the names.</param>
        /// <returns>Zero or more absolute paths.</returns>
        IEnumerable<string> ListFilesEndingBy(string directoryPath, string suffix);

        /// <summary>
        /// Finds whether a file exists.
        /// </summary>
        /// <param name="fullPath">The full path to the file.</param>
        /// <returns><see langword="true"/> if the file exists; otherwise, <see langword="false"/>.</returns>
        bool FileExists(string fullPath);

        /// <summary>
        /// Loads an assembly from a file.
        /// </summary>
        /// <param name="fullPath">The full path to the file.</param>
        /// <returns>The assembly object.</returns>
        IAssembly LoadAssembly(string fullPath);
    }
}
