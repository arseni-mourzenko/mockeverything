// <copyright file="TargetMethodType.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Attributes
{
    /// <summary>
    /// Indicates the type of the target method.
    /// </summary>
    public enum TargetMethodType
    {
        /// <summary>
        /// The method is an instance method.
        /// </summary>
        Instance,

        /// <summary>
        /// The method is a static method.
        /// </summary>
        Static,
    }
}
