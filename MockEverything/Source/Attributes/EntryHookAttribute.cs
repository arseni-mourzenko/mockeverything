// <copyright file="EntryHookAttribute.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.Attributes
{
    using System;

    /// <summary>
    /// Represents an attribute which indicates that a decorated method should be called before the invocation of every method and property within the proxy class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class EntryHookAttribute : Attribute
    {
    }
}