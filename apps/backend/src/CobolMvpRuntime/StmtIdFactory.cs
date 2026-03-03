using System.Globalization;

namespace CobolMvpRuntime
{
    internal static class StmtIdFactory
    {
        internal static string Create(int line, int? col = null)
        {
            if (line < 1)
            {
                line = 1;
            }

            string linePart = "L" + line.ToString("D6", CultureInfo.InvariantCulture);
            if (!col.HasValue || col.Value < 1)
            {
                return linePart;
            }

            return linePart + "C" + col.Value.ToString("D3", CultureInfo.InvariantCulture);
        }
    }
}
