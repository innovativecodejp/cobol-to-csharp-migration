using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    // MVP11 target: verify PERFORM VARYING (FROM/BY/UNTIL)
    internal static class Mvp11Program
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
            int wsN = int.Parse(input);
            int wsSum = 0;

            for (int wsI = 1; wsI <= wsN; wsI += 2)
            {
                wsSum += wsI;
            }

            return "N=" + wsN + "|SUM=" + wsSum;
        }
    }
}
