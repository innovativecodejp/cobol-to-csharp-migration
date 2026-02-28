using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;

namespace CobolMvpRuntime
{
    internal static class MigrationTrace
    {
        private static readonly object SyncRoot = new object();
        private static readonly UTF8Encoding Utf8NoBom = new UTF8Encoding(false);

        private static TraceOptions _options = new TraceOptions();
        private static int _stepCounter;
        private static bool _started;

        internal static void Start(TraceOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var normalized = new TraceOptions
            {
                Enabled = options.Enabled,
                OutputPath = string.IsNullOrWhiteSpace(options.OutputPath) ? "trace.log" : options.OutputPath,
                RunId = string.IsNullOrWhiteSpace(options.RunId) ? TraceOptions.GenerateRunId() : options.RunId,
                ResetOnStart = options.ResetOnStart
            };

            lock (SyncRoot)
            {
                _options = normalized;
                _stepCounter = 0;
                _started = true;

                if (!_options.Enabled)
                {
                    return;
                }

                string directory = Path.GetDirectoryName(_options.OutputPath);
                if (!string.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (_options.ResetOnStart)
                {
                    File.WriteAllText(_options.OutputPath, string.Empty, Utf8NoBom);
                }
            }
        }

        internal static string NextStep()
        {
            int step = Interlocked.Increment(ref _stepCounter);
            return step.ToString("D6", CultureInfo.InvariantCulture);
        }

        internal static void LogAssign(string varName, string value)
        {
            Log(
                "ASSIGN",
                ("VAR", varName ?? string.Empty),
                ("VAL", value ?? string.Empty)
            );
        }

        internal static void LogIf(string conditionText, bool resultBool)
        {
            Log(
                "IF",
                ("COND", conditionText ?? string.Empty),
                ("RESULT", resultBool ? "TRUE" : "FALSE")
            );
        }

        internal static void Log(string type, params (string Key, string Val)[] kvs)
        {
            lock (SyncRoot)
            {
                if (!_started || !_options.Enabled)
                {
                    return;
                }

                string step = NextStep();
                string line = TraceRecord.Build(_options.RunId, step, type ?? string.Empty, kvs);
                File.AppendAllText(_options.OutputPath, line + Environment.NewLine, Utf8NoBom);
            }
        }

        internal static void Stop()
        {
            lock (SyncRoot)
            {
                _started = false;
            }
        }
    }
}
