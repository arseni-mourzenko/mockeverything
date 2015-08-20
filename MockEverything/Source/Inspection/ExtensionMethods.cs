// <copyright file="ExtensionMethods.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using Mono.Cecil;

    /// <summary>
    /// Contains the extension methods used in the current project.
    /// </summary>
    internal static class ExtensionMethods
    {
        /// <summary>
        /// Enumerates the elements which will be part of the specification of a generic type.
        /// </summary>
        /// <param name="parameter">The generic parameter.</param>
        /// <returns>Zero or more elements to include in the specification.</returns>
        public static IEnumerable<string> Describe(this GenericParameter parameter)
        {
            Contract.Requires(parameter != null);
            Contract.Ensures(Contract.Result<IEnumerable<string>>() != null);

            if (parameter.HasDefaultConstructorConstraint)
            {
                yield return "new()";
            }

            foreach (var constraint in parameter.Constraints)
            {
                yield return constraint.FullName;
            }
        }
    }
}
