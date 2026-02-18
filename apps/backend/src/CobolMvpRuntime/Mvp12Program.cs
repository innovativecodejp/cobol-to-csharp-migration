using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    // MVP12 target: verify sequential file input core
    // READ ... AT END with EOF control
    internal static class Mvp12Program
    {
        internal static void Run(TextWriter writer)
        {
            string envPath = Environment.GetEnvironmentVariable("MVP12_INPUT");
            string inputPath = string.IsNullOrWhiteSpace(envPath)
                ? Path.Combine(Environment.CurrentDirectory, "mvp12-input.txt")
                : envPath;

            int lineNo = 0;
            bool eof = false;

            using (var reader = new StreamReader(inputPath, Encoding.ASCII))
            {
                while (!eof)
                {
                    string record = reader.ReadLine();
                    if (record == null)
                    {
                        eof = true;
                    }
                    else
                    {
                        lineNo += 1;
                        writer.WriteLine("LINE=" + lineNo.ToString("D4") + "|TEXT=" + record);
                    }
                }
            }

            writer.WriteLine("COUNT=" + lineNo.ToString("D4"));
        }
    }
}
