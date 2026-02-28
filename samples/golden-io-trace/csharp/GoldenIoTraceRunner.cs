using System;
using System.IO;
using System.Text;
using CobolMvpRuntime;

namespace GoldenIoTraceSample
{
    internal static class GoldenIoTraceRunner
    {
        internal static void Run(string inputPath, string outputPath, string tracePath)
        {
            MigrationTrace.Start(new TraceOptions
            {
                Enabled = true,
                OutputPath = tracePath,
                RunId = "GOLDEN-0001",
                ResetOnStart = true
            });

            int recNo = 0;

            // START event (golden sample uses fixed key)
            MigrationTrace.LogStart("IDX1", "0003", "OK", ("STMT", "S001"));

            using (var reader = new StreamReader(inputPath, Encoding.ASCII))
            using (var writer = new StreamWriter(outputPath, false, Encoding.ASCII))
            {
                writer.NewLine = "\r\n";

                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        MigrationTrace.LogRead("IN1", "EOF", ("STMT", "R999"));
                        break;
                    }

                    recNo += 1;
                    MigrationTrace.LogRead("IN1", "OK", ("STMT", "R001"), ("RECNO", recNo.ToString("D4")));

                    bool isA = line.StartsWith("A", StringComparison.Ordinal);
                    MigrationTrace.Log("IF", ("STMT", "I001"), ("COND", "FIRST==A"), ("RESULT", isA ? "TRUE" : "FALSE"));

                    MigrationTrace.Log("ASSIGN", ("STMT", "M001"), ("VAR", "OUT-REC"), ("VAL", line));

                    writer.WriteLine(line);
                    MigrationTrace.LogWrite("OUT1", ("STMT", "W001"), ("RECNO", recNo.ToString("D4")));
                }
            }

            MigrationTrace.Log("DISPLAY", ("STMT", "D001"), ("TEXT", "DONE"));
            MigrationTrace.Stop();
        }
    }
}
