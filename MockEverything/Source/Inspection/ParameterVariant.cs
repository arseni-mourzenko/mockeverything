// <copyright file="ParameterVariant.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection
{
    /// <summary>
    /// Represents the variant of the parameter.
    /// </summary>
    public enum ParameterVariant
    {
        /// <summary>
        /// The parameter causes the argument to be passed by value.
        /// </summary>
        In,

        /// <summary>
        /// The parameter causes the argument to be passed by reference, the argument being eventually not initialized before calling the method.
        /// </summary>
        Out,

        /// <summary>
        /// The parameter causes the argument to be passed by reference, the argument being necessarily initialized before calling the method.
        /// </summary>
        Ref,

        /// <summary>
        /// The parameter enables to receive a variable number of arguments.
        /// </summary>
        Params
    }
}
