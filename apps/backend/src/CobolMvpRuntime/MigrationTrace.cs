using System;
using System.Collections.Generic;
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

        internal static void LogRead(string file, string result, params (string Key, string Val)[] extra)
        {
            Log("READ", MergeFields(("FILE", file ?? string.Empty), ("RESULT", result ?? string.Empty), extra));
        }

        internal static void LogWrite(string file, params (string Key, string Val)[] extra)
        {
            Log("WRITE", MergeFields(("FILE", file ?? string.Empty), extra));
        }

        internal static void LogStart(string file, string key, string result, params (string Key, string Val)[] extra)
        {
            Log(
                "START",
                MergeFields(
                    ("FILE", file ?? string.Empty),
                    ("KEY", key ?? string.Empty),
                    ("RESULT", result ?? string.Empty),
                    extra
                )
            );
        }

        internal static void Stop()
        {
            lock (SyncRoot)
            {
                _started = false;
            }
        }

        private static (string Key, string Val)[] MergeFields(
            (string Key, string Val) required,
            params (string Key, string Val)[] extra)
        {
            var list = new List<(string Key, string Val)> { required };
            if (extra != null && extra.Length > 0)
            {
                list.AddRange(extra);
            }

            return list.ToArray();
        }

        private static (string Key, string Val)[] MergeFields(
            (string Key, string Val) required1,
            (string Key, string Val) required2,
            params (string Key, string Val)[] extra)
        {
            var list = new List<(string Key, string Val)> { required1, required2 };
            if (extra != null && extra.Length > 0)
            {
                list.AddRange(extra);
            }

            return list.ToArray();
        }

        private static (string Key, string Val)[] MergeFields(
            (string Key, string Val) required1,
            (string Key, string Val) required2,
            (string Key, string Val) required3,
            params (string Key, string Val)[] extra)
        {
            var list = new List<(string Key, string Val)> { required1, required2, required3 };
            if (extra != null && extra.Length > 0)
            {
                list.AddRange(extra);
            }

            return list.ToArray();
        }
    }
}
