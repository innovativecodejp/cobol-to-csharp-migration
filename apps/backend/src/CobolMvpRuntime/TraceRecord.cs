using System.Text;

namespace CobolMvpRuntime
{
    internal static class TraceRecord
    {
        internal static string Build(string runId, string step, string type, params (string Key, string Val)[] kvs)
        {
            var sb = new StringBuilder();
            sb.Append("RUN=").Append(Escape(runId));
            sb.Append("|STEP=").Append(Escape(step));
            sb.Append("|TYPE=").Append(Escape(type));

            if (kvs != null)
            {
                foreach (var kv in kvs)
                {
                    sb.Append("|")
                      .Append(kv.Key)
                      .Append("=")
                      .Append(Escape(kv.Val));
                }
            }

            return sb.ToString();
        }

        private static string Escape(string value)
        {
            if (value == null)
            {
                return string.Empty;
            }

            return value.Replace("\r", "\\r").Replace("\n", "\\n");
        }
    }
}
