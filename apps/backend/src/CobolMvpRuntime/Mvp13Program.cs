using System;
using System.IO;

namespace CobolMvpRuntime
{
    // MVP13 target: verify sequential file output core
    // SELECT/FD/OPEN OUTPUT/WRITE/CLOSE
    // Conversion: outfile.WriteLine(record);
    internal static class Mvp13Program
    {
        internal static void Run(TextWriter writer)
        {
            string envPath = Environment.GetEnvironmentVariable("MVP13_OUTPUT");
            string outputPath = string.IsNullOrWhiteSpace(envPath)
                ? Path.Combine(Environment.CurrentDirectory, "mvp13-output.txt")
                : envPath;

            using (var fileWriter = new SequentialFileWriter(outputPath))
            {
                for (int i = 1; i <= 3; i++)
                {
                    fileWriter.WriteLine("REC=" + i.ToString("D4"));
                }
            }

            writer.WriteLine("WROTE=0003");
        }

        /// <summary>
        /// READ from input, WRITE to output. MVP12 + MVP13 integration.
        /// </summary>
        internal static void ProcessFile(string inPath, string outPath)
        {
            using (var reader = new SequentialFileReader(inPath))
            using (var writer = new SequentialFileWriter(outPath))
            {
                while (reader.ReadNext())
                {
                    writer.WriteLine(reader.CurrentRecord);
                }
            }
        }
    }
}
