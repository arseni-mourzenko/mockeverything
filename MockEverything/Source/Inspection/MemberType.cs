// <copyright file="MemberType.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection
{
    using System;

    /// <summary>
    /// Represents a type of a member, that is a type or a method.
    /// </summary>
    [Flags]
    public enum MemberType
    {
        /// <summary>
        /// Indicates that a member is non-static.
        /// </summary>
        Instance = 1,

        /// <summary>
        /// Indicates that a member is static.
        /// </summary>
        Static = 2,

        /// <summary>
        /// Indicates that a member is either static or non-static.
        /// </summary>
        All = 3,
    }
}
