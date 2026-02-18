using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    // MVP13 target: verify sequential file output core
    // SELECT/FD/WRITE without READ derivatives
    internal static class Mvp13Program
    {
        internal static void Run(TextWriter writer)
        {
            string envPath = Environment.GetEnvironmentVariable("MVP13_OUTPUT");
            string outputPath = string.IsNullOrWhiteSpace(envPath)
                ? Path.Combine(Environment.CurrentDirectory, "mvp13-output.txt")
                : envPath;

            using (var fileWriter = new StreamWriter(outputPath, false, Encoding.ASCII))
            {
                for (int i = 1; i <= 3; i++)
                {
                    fileWriter.WriteLine("REC=" + i.ToString("D4"));
                }
            }

            writer.WriteLine("WROTE=0003");
        }
    }
}
