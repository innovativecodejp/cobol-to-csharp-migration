using System;
using System.IO;
using System.Text;
using CobolMvpRuntime;
using Xunit;

namespace CobolToCsharpMigration.Tests;

public class CobolMvpRuntimeMvp14Tests
{
    [Theory]
    [InlineData("1", "expected1.txt")]
    [InlineData("2", "expected2.txt")]
    public void Run_Mvp14Samples_MatchesExpectedStdout(string caseNo, string expectedFile)
    {
        string backendRoot = GetBackendRoot();
        string repoRoot = Path.GetFullPath(Path.Combine(backendRoot, "..", ".."));
        string expectedPath = Path.Combine(repoRoot, "samples", "data", "mvp14", expectedFile);

        string tempDir = Path.Combine(Path.GetTempPath(), "mvp14_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        string indexPath = Path.Combine(tempDir, "mvp14-index.dat");
        if (File.Exists(indexPath))
        {
            File.Delete(indexPath);
        }

        string? originalIndex = Environment.GetEnvironmentVariable("MVP14_INDEX");
        string? originalCase = Environment.GetEnvironmentVariable("MVP14_CASE");
        try
        {
            Environment.SetEnvironmentVariable("MVP14_INDEX", indexPath);
            Environment.SetEnvironmentVariable("MVP14_CASE", caseNo);

            var stdout = new StringWriter();
            stdout.NewLine = "\r\n";
            Mvp14Program.Run(stdout);

            string expected = NormalizeNewLine(File.ReadAllText(expectedPath, Encoding.ASCII));
            string actual = NormalizeNewLine(stdout.ToString());
            Assert.Equal(expected, actual);
        }
        finally
        {
            Environment.SetEnvironmentVariable("MVP14_INDEX", originalIndex);
            Environment.SetEnvironmentVariable("MVP14_CASE", originalCase);
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
