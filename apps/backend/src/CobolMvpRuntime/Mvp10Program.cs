using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    // MVP10 target: verify PERFORM UNTIL (test-before loop)
    internal static class Mvp10Program
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
            int wsI = 1;
            int wsSum = 0;

            while (wsI <= wsN)
            {
                wsSum += wsI;
                wsI += 1;
            }

            return "N=" + wsN + "|SUM=" + wsSum;
        }
    }
}
