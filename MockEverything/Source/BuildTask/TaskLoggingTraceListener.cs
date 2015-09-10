namespace MockEverything.BuildTask
{
    using System.Diagnostics;
    using System.Diagnostics.Contracts;
    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    internal class TaskLoggingTraceListener : TraceListener
    {
        private readonly TaskLoggingHelper log;

        public TaskLoggingTraceListener(TaskLoggingHelper log)
        {
            Contract.Requires(log != null);

            this.log = log;
        }

        public override void Write(string message)
        {
            this.log.LogMessage(MessageImportance.Normal, message);
        }

        public override void WriteLine(string message)
        {
            this.log.LogMessage(MessageImportance.Normal, message);
        }
    }
}
