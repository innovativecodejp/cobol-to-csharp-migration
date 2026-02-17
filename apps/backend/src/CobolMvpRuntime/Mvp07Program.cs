using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    // MVP07 target: verify EVALUATE minimal branch set
    // Multiple WHEN + THRU + OTHER
    internal static class Mvp07Program
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
            int wsVal = int.Parse(input);
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

            return "VAL=" + wsVal + "|RANGE=" + wsRange;
        }
    }
}
