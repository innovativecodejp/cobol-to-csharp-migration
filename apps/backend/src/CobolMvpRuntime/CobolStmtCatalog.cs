using System;
using System.Collections.Generic;
using System.IO;

namespace CobolMvpRuntime
{
    // Minimal statement metadata holder used by trace emission.
    internal sealed class CobolStatement
    {
        internal CobolStatement(string key, int line, int? col)
        {
            Key = key;
            Line = line;
            Col = col;
            StmtId = StmtIdFactory.Create(line, col);
        }

        internal string Key { get; }

        internal int Line { get; }

        internal int? Col { get; }

        internal string StmtId { get; }
    }

    internal static class CobolStmtCatalog
    {
        private static readonly string[] StatementKeys = { "START", "READ", "IF", "MOVE", "WRITE", "DISPLAY" };

        internal static Dictionary<string, CobolStatement> LoadFromSource(string cobolSourcePath)
        {
            var result = new Dictionary<string, CobolStatement>(StringComparer.OrdinalIgnoreCase);
            string[] lines = File.ReadAllLines(cobolSourcePath);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                foreach (string key in StatementKeys)
                {
                    if (result.ContainsKey(key))
                    {
                        continue;
                    }

                    if (ContainsKeyword(line, key))
                    {
                        int col = line.IndexOf(key, StringComparison.Ordinal);
                        result[key] = new CobolStatement(key, i + 1, col >= 0 ? col + 1 : (int?)null);
                    }
                }
            }

            return result;
        }

        private static bool ContainsKeyword(string line, string keyword)
        {
            if (string.IsNullOrEmpty(line))
            {
                return false;
            }

            int index = line.IndexOf(keyword, StringComparison.Ordinal);
            if (index < 0)
            {
                return false;
            }

            bool leftBoundary = index == 0 || !char.IsLetterOrDigit(line[index - 1]);
            int rightIndex = index + keyword.Length;
            bool rightBoundary = rightIndex >= line.Length || !char.IsLetterOrDigit(line[rightIndex]);
            return leftBoundary && rightBoundary;
        }
    }
}
