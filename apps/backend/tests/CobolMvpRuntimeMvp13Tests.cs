using System;
using System.IO;
using System.Text;
using CobolMvpRuntime;
using Xunit;

namespace CobolToCsharpMigration.Tests;

public class CobolMvpRuntimeMvp13Tests
{
    [Fact]
    public void Run_Mvp13Sample_WritesExpectedOutputFile()
    {
        string backendRoot = GetBackendRoot();
        string repoRoot = Path.GetFullPath(Path.Combine(backendRoot, "..", ".."));
        string expectedPath = Path.Combine(repoRoot, "samples", "data", "mvp13", "expected.txt");

        string tempDir = Path.Combine(Path.GetTempPath(), "mvp13_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        string outputPath = Path.Combine(tempDir, "mvp13-output.txt");
        if (File.Exists(outputPath))
        {
            File.Delete(outputPath);
        }

        string? original = Environment.GetEnvironmentVariable("MVP13_OUTPUT");
        try
        {
            Environment.SetEnvironmentVariable("MVP13_OUTPUT", outputPath);

            var stdout = new StringWriter();
            stdout.NewLine = "\r\n";
            Mvp13Program.Run(stdout);

            string expected = NormalizeNewLine(File.ReadAllText(expectedPath, Encoding.ASCII));
            string actual = NormalizeNewLine(File.ReadAllText(outputPath, Encoding.ASCII));
            Assert.Equal(expected, actual);
        }
        finally
        {
            Environment.SetEnvironmentVariable("MVP13_OUTPUT", original);
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    private static string NormalizeNewLine(string value)
    {
        return value.Replace("\r\n", "\n");
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
