// <copyright file="IMethod.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Inspection
{
    using System;
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
        /// Gets the parameters of the method.
        /// </summary>
        IEnumerable<Parameter> Parameters { get; }

        /// <summary>
        /// <para>Gets the generic types of the method. If the method is not generic, the enumeration yields no results.</para>
        /// <para>Every line contains a specification of a generic type. For a generic type with no constraints, the specification is an empty string. Otherwise, the specification contains sorted comma-separated values, every value corresponding to the full name of the type. The <see href="https://msdn.microsoft.com/en-us/library/sd2w2ew5.aspx">new constraint</see> is represented by the string "new()".</para>
        /// <para>For example, the method <c><![CDATA[void Demo<T1, T2>() where T2: IComparable, new()]]></c> has the following specification: "new(),System.IComparable".</para>
        /// </summary>
        /// <remarks>
        /// The generic aspect of the container class has no effect on the result.
        /// </remarks>
        IEnumerable<string> GenericTypes { get; }

        /// <summary>
        /// Gets a value indicating whether the method is declared as public.
        /// </summary>
        bool IsPublic { get; }

        /// <summary>
        /// Replaces the body of this method by a body of the specified one.
        /// </summary>
        /// <remarks>
        /// <para>Implementers are allowed to accept only methods of the same type to be passed as a parameter, and throw <see cref="NotImplementedException"/> if the type of the other method is different.</para>
        /// <para>Implementers should not set a contract requiring the other method to be of the same type, since it would violate the interface and would be difficult to impossible to enforce by the callers.</para>
        /// </remarks>
        /// <param name="other">The method to use as a replacement.</param>
        /// <exception cref="NotImplementedException">The type of the other method doesn't match the type of the current object.</exception>
        void ReplaceBody(IMethod other);
    }
}
