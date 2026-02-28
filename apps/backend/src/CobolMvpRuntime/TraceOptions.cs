using System;

namespace CobolMvpRuntime
{
    internal sealed class TraceOptions
    {
        internal bool Enabled { get; set; } = false;

        internal string OutputPath { get; set; } = "trace.log";

        internal string RunId { get; set; } = string.Empty;

        // When false, trace APIs are no-op and output file is not created.
        internal bool ResetOnStart { get; set; } = false;

        internal static string GenerateRunId()
        {
            return DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }
    }
}
