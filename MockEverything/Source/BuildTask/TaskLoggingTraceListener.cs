// <copyright file="TaskLoggingTraceListener.cs">
//      Copyright (c) Arseni Mourzenko 2015. The code is distributed under the MIT License.
// </copyright>
// <author id="5c2316d3-622a-4a8d-816d-5054a48f415f">Arseni Mourzenko</author>

namespace MockEverything.BuildTask
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Represents the trace listener which, in a context of a build task, will forward the trace messages to the build log.
    /// </summary>
    /// <example>
    /// <para>The following piece of code shows a possible usage of the listener.</para>
    /// <code>
    /// <![CDATA[
    /// Trace.Listeners.Add(new TaskLoggingTraceListener(this.Log));
    /// ]]>
    /// </code>
    /// </example>
    internal class TaskLoggingTraceListener : TraceListener
    {
        /// <summary>
        /// The build task logging helper.
        /// </summary>
        private readonly TaskLoggingHelper log;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskLoggingTraceListener"/> class.
        /// </summary>
        /// <param name="log">The build task logging helper.</param>
        public TaskLoggingTraceListener(TaskLoggingHelper log)
        {
            Contract.Requires(log != null);

            this.log = log;
        }

        /// <summary>
        /// Writes the specified message to the listener.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void Write(string message)
        {
            this.log.LogMessage(MessageImportance.Normal, message);
        }

        /// <summary>
        /// Writes the specified message to the listener, followed by a line terminator.
        /// </summary>
        /// <param name="message">A message to write.</param>
        public override void WriteLine(string message)
        {
            this.log.LogMessage(MessageImportance.Normal, message);
        }
    }
}
