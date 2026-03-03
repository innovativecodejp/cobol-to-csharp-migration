using System;
using System.IO;
using System.Text;
using CobolMvpRuntime;
using Xunit;

namespace CobolToCsharpMigration.Tests;

public class GoldenIoTraceRunnerTests
{
    [Fact]
    public void GeneratedCode_EmitsStmtInTrace()
    {
        string backendRoot = GetBackendRoot();
        string repoRoot = Path.GetFullPath(Path.Combine(backendRoot, "..", ".."));

        string cobolSourcePath = Path.Combine(repoRoot, "samples", "golden-io-trace", "cobol", "GoldenIoTrace.cbl");
        string tempDir = Path.Combine(Path.GetTempPath(), "golden_trace_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);

        string inputPath = Path.Combine(tempDir, "input.txt");
        string outputPath = Path.Combine(tempDir, "output.txt");
        string tracePath = Path.Combine(tempDir, "trace.log");

        File.WriteAllLines(inputPath, new[] { "AAA", "BBBB" }, Encoding.ASCII);

        try
        {
            GoldenIoTraceRunner.Run(inputPath, outputPath, tracePath, cobolSourcePath);

            string[] lines = File.ReadAllLines(tracePath, Encoding.UTF8);
            Assert.NotEmpty(lines);
            Assert.Contains(lines, line => line.Contains("STMT=L000056C016"));
            Assert.Contains(lines, line => line.Contains("TYPE=READ"));
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    private static string GetBackendRoot()
    {
        DirectoryInfo? current = new DirectoryInfo(AppContext.BaseDirectory);
        while (current != null)
        {
            if (File.Exists(Path.Combine(current.FullName, "CobolToCsharpMigration.sln"))
                && Directory.Exists(Path.Combine(current.FullName, "src")))
            {
                return current.FullName;
            }

            current = current.Parent;
        }

        throw new InvalidOperationException("Could not locate backend root directory.");
    }
}
