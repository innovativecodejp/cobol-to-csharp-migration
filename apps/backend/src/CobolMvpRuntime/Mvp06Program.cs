using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    // MVP06 target: verify EVALUATE derived features
    // - Multiple WHEN
    // - THRU
    // - ALSO (EVALUATE TRUE ALSO TRUE)
    internal static class Mvp06Program
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
            int commaIndex = input.IndexOf(',');
            if (commaIndex <= 0 || commaIndex >= input.Length - 1)
            {
                throw new InvalidOperationException("Input must be MODE,VAL format.");
            }

            int wsMode = int.Parse(input.Substring(0, commaIndex));
            int wsVal = int.Parse(input.Substring(commaIndex + 1));

            // THRU branch (EVALUATE WS-MODE / EVALUATE WS-VAL)
            if (wsMode == 1)
            {
                string wsRange;
                if (wsVal >= 0 && wsVal <= 9)
                {
                    wsRange = "0-9";
                }
                else if (wsVal >= 10 && wsVal <= 19)
                {
                    wsRange = "10-19";
                }
                else
                {
                    wsRange = "OTHER";
                }

                return "MODE=" + wsMode + "|VAL=" + wsVal + "|RANGE=" + wsRange;
            }

            // ALSO branch (EVALUATE TRUE ALSO TRUE)
            string wsCase;
            if ((wsMode == 2) && (wsVal >= 200))
            {
                wsCase = "VIP";
            }
            else if ((wsMode == 2) && (wsVal < 200))
            {
                wsCase = "NORMAL";
            }
            else
            {
                wsCase = "N/A";
            }

            return "MODE=" + wsMode + "|VAL=" + wsVal + "|CASE=" + wsCase;
        }
    }
}
