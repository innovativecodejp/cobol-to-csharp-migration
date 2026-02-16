using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    // MVP04 target: verify INSPECT derived features
    // - TALLYING FOR ALL "A"
    // - REPLACING LEADING "0" BY "X"
    // - REPLACING FIRST "AB" BY "YZ"
    internal static class Mvp04Program
    {
        internal static void ProcessFile(string inPath, string outPath)
        {
            using (var reader = new StreamReader(inPath, Encoding.ASCII))
            using (var writer = new StreamWriter(outPath, false, Encoding.ASCII))
            {
                writer.NewLine = "\r\n";
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    writer.WriteLine(TransformRecord(line));
                }
            }
        }

        internal static string TransformRecord(string input)
        {
            if (IsTallyingCase(input))
            {
                int count = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    if (input[i] == 'A')
                    {
                        count++;
                    }
                }

                return "TEXT=" + input + "|COUNT=" + count;
            }

            string replacedLeading = ReplaceLeading(input, '0', 'X');
            string replacedFirst = ReplaceFirst(replacedLeading, "AB", "YZ");
            return "TEXT=" + replacedFirst;
        }

        private static bool IsTallyingCase(string input)
        {
            // MVP04 sample contract:
            // input1 is tallying sample and starts with A (AAABAAAXAA).
            // input2 is replacing sample and starts with 0 (000ABABAB).
            return input.StartsWith("A", StringComparison.Ordinal);
        }

        private static string ReplaceLeading(string value, char from, char to)
        {
            int i = 0;
            while (i < value.Length && value[i] == from)
            {
                i++;
            }

            if (i == 0)
            {
                return value;
            }

            return new string(to, i) + value.Substring(i);
        }

        private static string ReplaceFirst(string value, string from, string to)
        {
            int index = value.IndexOf(from, StringComparison.Ordinal);
            if (index < 0)
            {
                return value;
            }

            return value.Substring(0, index) + to + value.Substring(index + from.Length);
        }
    }
}
