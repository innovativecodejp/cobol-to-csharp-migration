using System;
using System.IO;

namespace CobolMvpRuntime
{
    // MVP12 target: verify sequential file input core
    // READ ... AT END / NOT AT END with EOF control
    // Conversion: if (!reader.ReadNext()) { /* AT END */ } else { /* NOT AT END */ }
    internal static class Mvp12Program
    {
        internal static void Run(TextWriter writer)
        {
            string envPath = Environment.GetEnvironmentVariable("MVP12_INPUT");
            string inputPath = string.IsNullOrWhiteSpace(envPath)
                ? Path.Combine(Environment.CurrentDirectory, "mvp12-input.txt")
                : envPath;

            int lineNo = 0;

            using (var reader = new SequentialFileReader(inputPath))
            {
                while (true)
                {
                    if (!reader.ReadNext())
                    {
                        // AT END
                        break;
                    }

                    // NOT AT END
                    lineNo += 1;
                    writer.WriteLine("LINE=" + lineNo.ToString("D4") + "|TEXT=" + reader.CurrentRecord);
                }
            }

            writer.WriteLine("COUNT=" + lineNo.ToString("D4"));
        }
    }
}
