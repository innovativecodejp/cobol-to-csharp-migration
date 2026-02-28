using System;
using System.IO;
using System.Text;
using CobolMvpRuntime;
using Xunit;

namespace CobolToCsharpMigration.Tests;

public class MigrationTraceTests
{
    [Fact]
    public void Trace_Enabled_WritesStepwiseRecords()
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
            Assert.Contains("COND=A==A", lines[1]);
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
    public void Trace_Disabled_DoesNotWriteFile()
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

            MigrationTrace.LogAssign("WS-COUNT", "1");
            MigrationTrace.LogIf("A==A", true);
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
