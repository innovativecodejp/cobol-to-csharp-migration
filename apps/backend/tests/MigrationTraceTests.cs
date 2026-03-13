using System;
using System.IO;
using System.Text;
using CobolMvpRuntime;
using Xunit;

namespace CobolToCsharpMigration.Tests;

[Collection("MigrationTrace")]
public class MigrationTraceTests
{
    [Fact]
    public void MigrationTraceTests_AssignIf_WritesStepwiseRecords()
    {
        string tempFile = Path.GetTempFileName();
        try
        {
            MigrationTrace.Start(new TraceOptions
            {
                Enabled = true,
                OutputPath = tempFile,
                RunId = "20260301-0001",
                ResetOnStart = true
            });

            MigrationTrace.LogAssign("WS-COUNT", "1");
            MigrationTrace.LogIf("A==A", true);
            MigrationTrace.Stop();

            string[] lines = File.ReadAllLines(tempFile, Encoding.UTF8);
            Assert.Equal(2, lines.Length);

            Assert.Contains("RUN=20260301-0001", lines[0]);
            Assert.Contains("STEP=000001", lines[0]);
            Assert.Contains("TYPE=ASSIGN", lines[0]);
            Assert.Contains("VAR=WS-COUNT", lines[0]);
            Assert.Contains("VAL=1", lines[0]);

            Assert.Contains("RUN=20260301-0001", lines[1]);
            Assert.Contains("STEP=000002", lines[1]);
            Assert.Contains("TYPE=IF", lines[1]);
            Assert.Contains("COND=A\\=\\=A", lines[1]);
            Assert.Contains("RESULT=TRUE", lines[1]);
        }
        finally
        {
            MigrationTrace.Stop();
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    [Fact]
    public void MigrationTraceTests_IoLog_WritesExpectedLines()
    {
        string tempFile = Path.GetTempFileName();
        try
        {
            MigrationTrace.Start(new TraceOptions
            {
                Enabled = true,
                OutputPath = tempFile,
                RunId = "20260301-0003",
                ResetOnStart = true
            });

            MigrationTrace.LogRead("INPUT1", "OK", ("STMT", "R001"), ("RECNO", "1"));
            MigrationTrace.LogWrite("OUT1", ("STMT", "W001"), ("RECNO", "1"), ("LEN", "40"));
            MigrationTrace.LogStart("IDX1", "K=0001", "OK", ("STMT", "S001"));
            MigrationTrace.Stop();

            string[] lines = File.ReadAllLines(tempFile, Encoding.UTF8);
            Assert.Equal(3, lines.Length);

            Assert.Contains("STEP=000001", lines[0]);
            Assert.Contains("TYPE=READ", lines[0]);
            Assert.Contains("FILE=INPUT1", lines[0]);
            Assert.Contains("RESULT=OK", lines[0]);
            Assert.Contains("STMT=R001", lines[0]);
            Assert.Contains("RECNO=1", lines[0]);

            Assert.Contains("STEP=000002", lines[1]);
            Assert.Contains("TYPE=WRITE", lines[1]);
            Assert.Contains("FILE=OUT1", lines[1]);
            Assert.Contains("STMT=W001", lines[1]);
            Assert.Contains("RECNO=1", lines[1]);
            Assert.Contains("LEN=40", lines[1]);

            Assert.Contains("STEP=000003", lines[2]);
            Assert.Contains("TYPE=START", lines[2]);
            Assert.Contains("FILE=IDX1", lines[2]);
            Assert.Contains("KEY=K\\=0001", lines[2]);
            Assert.Contains("RESULT=OK", lines[2]);
            Assert.Contains("STMT=S001", lines[2]);
        }
        finally
        {
            MigrationTrace.Stop();
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }

    [Fact]
    public void MigrationTraceTests_Disabled_DoesNotWrite()
    {
        string tempFile = Path.Combine(Path.GetTempPath(), "trace_test_" + Guid.NewGuid().ToString("N") + ".log");
        try
        {
            MigrationTrace.Start(new TraceOptions
            {
                Enabled = false,
                OutputPath = tempFile,
                RunId = "20260301-0002",
                ResetOnStart = true
            });

            MigrationTrace.LogRead("INPUT1", "OK", ("RECNO", "1"));
            MigrationTrace.Stop();

            if (File.Exists(tempFile))
            {
                string body = File.ReadAllText(tempFile, Encoding.UTF8);
                Assert.Equal(string.Empty, body);
            }
        }
        finally
        {
            MigrationTrace.Stop();
            if (File.Exists(tempFile))
            {
                File.Delete(tempFile);
            }
        }
    }
}
