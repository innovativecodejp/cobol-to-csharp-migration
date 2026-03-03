using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    internal static class GoldenIoTraceRunner
    {
        internal static void Run(string inputPath, string outputPath, string tracePath, string cobolSourcePath)
        {
            var stmt = CobolStmtCatalog.LoadFromSource(cobolSourcePath);

            MigrationTrace.Start(new TraceOptions
            {
                Enabled = true,
                OutputPath = tracePath,
                RunId = "GOLDEN-0001",
                ResetOnStart = true
            });

            int recNo = 0;

            MigrationTrace.LogStart("IDX1", "0003", "OK", ("STMT", GetStmt(stmt, "START")));

            using (var reader = new StreamReader(inputPath, Encoding.ASCII))
            using (var writer = new StreamWriter(outputPath, false, Encoding.ASCII))
            {
                writer.NewLine = "\r\n";

                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        MigrationTrace.LogRead("IN1", "EOF", ("STMT", GetStmt(stmt, "READ")));
                        break;
                    }

                    recNo += 1;
                    MigrationTrace.LogRead(
                        "IN1",
                        "OK",
                        ("STMT", GetStmt(stmt, "READ")),
                        ("RECNO", recNo.ToString("D4"))
                    );

                    bool isA = line.StartsWith("A", StringComparison.Ordinal);
                    MigrationTrace.Log(
                        "IF",
                        ("STMT", GetStmt(stmt, "IF")),
                        ("COND", "FIRST==A"),
                        ("RESULT", isA ? "TRUE" : "FALSE")
                    );

                    MigrationTrace.Log(
                        "ASSIGN",
                        ("STMT", GetStmt(stmt, "MOVE")),
                        ("VAR", "OUT-REC"),
                        ("VAL", line)
                    );

                    writer.WriteLine(line);
                    MigrationTrace.LogWrite(
                        "OUT1",
                        ("STMT", GetStmt(stmt, "WRITE")),
                        ("RECNO", recNo.ToString("D4"))
                    );
                }
            }

            MigrationTrace.Log("DISPLAY", ("STMT", GetStmt(stmt, "DISPLAY")), ("TEXT", "DONE"));
            MigrationTrace.Stop();
        }

        private static string GetStmt(Dictionary<string, CobolStatement> stmtCatalog, string key)
        {
            CobolStatement statement;
            if (stmtCatalog.TryGetValue(key, out statement))
            {
                return statement.StmtId;
            }

            return StmtIdFactory.Create(1);
        }
    }
}
