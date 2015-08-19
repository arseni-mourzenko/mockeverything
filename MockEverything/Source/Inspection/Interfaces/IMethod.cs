// <copyright file="IMethod.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a type from an assembly.
    /// </summary>
    public interface IMethod
    {
        /// <summary>
        /// Gets the short name of the method. This name doesn't contain any reference to the type, namespace or assembly.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the return type of the method.
        /// </summary>
        IType ReturnType { get; }

        /// <summary>
        /// <para>Gets the generic types of the method. If the method is not generic, the enumeration yields no results.</para>
        /// <para>Every line contains a specification of a generic type. For a generic type with no constraints, the specification is an empty string. Otherwise, the specification contains sorted comma-separated values, every value corresponding to the full name of the type. The <see href="https://msdn.microsoft.com/en-us/library/sd2w2ew5.aspx">new constraint</see> is represented by the string "new()".</para>
        /// <para>For example, the method <c><![CDATA[void Demo<T1, T2>() where T2: IComparable, new()]]></c> has the following specification: "new(),System.IComparable".</para>
        /// </summary>
        /// <remarks>
        /// The generic aspect of the container class has no effect on the result.
        /// </remarks>
        IEnumerable<string> GenericTypes { get; }
    }
}
