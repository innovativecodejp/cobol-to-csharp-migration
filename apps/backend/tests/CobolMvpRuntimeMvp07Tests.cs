using System;
using System.IO;
using System.Text;
using CobolMvpRuntime;
using Xunit;

namespace CobolToCsharpMigration.Tests;

public class CobolMvpRuntimeMvp07Tests
{
    [Theory]
    [InlineData("input1.txt", "expected1.txt")]
    [InlineData("input2.txt", "expected2.txt")]
    public void ProcessFile_Mvp07Samples_MatchesExpectedOutput(string inputFile, string expectedFile)
    {
        string backendRoot = GetBackendRoot();
        string repoRoot = Path.GetFullPath(Path.Combine(backendRoot, "..", ".."));

        string inPath = Path.Combine(repoRoot, "samples", "data", "mvp07", inputFile);
        string expectedPath = Path.Combine(repoRoot, "samples", "data", "mvp07", expectedFile);
        string outPath = Path.Combine(Path.GetTempPath(), "mvp07_test_out_" + Guid.NewGuid().ToString("N") + ".txt");

        try
        {
            Mvp07Program.ProcessFile(inPath, outPath);

            string expected = NormalizeNewLine(File.ReadAllText(expectedPath, Encoding.ASCII));
            string actual = NormalizeNewLine(File.ReadAllText(outPath, Encoding.ASCII));
            Assert.Equal(expected, actual);
        }
        finally
        {
            if (File.Exists(outPath))
            {
                File.Delete(outPath);
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
