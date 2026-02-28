using System;

namespace CobolMvpRuntime
{
    internal sealed class TraceOptions
    {
        internal bool Enabled { get; set; } = false;

        internal string OutputPath { get; set; } = "trace.log";

        internal string RunId { get; set; } = string.Empty;

        internal bool ResetOnStart { get; set; } = false;

        internal static string GenerateRunId()
        {
            return DateTime.Now.ToString("yyyyMMdd-HHmmss");
        }
    }
}
