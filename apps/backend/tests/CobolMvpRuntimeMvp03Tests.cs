using System;
using System.IO;
using System.Text;
using CobolMvpRuntime;
using Xunit;

namespace CobolToCsharpMigration.Tests;

public class CobolMvpRuntimeMvp03Tests
{
    [Theory]
    [InlineData("AAA   BBB  CCCC", "A=AAA|LA=3|B=BBB|LB=3|C=CCCC|LC=4|PTR=16|DC=3")]
    public void TransformRecord_KnownInput_ReturnsExpected(string input, string expected)
    {
        string result = Mvp03Program.TransformRecord(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void ProcessFile_Mvp03Sample_MatchesExpectedOutput()
    {
        string backendRoot = GetBackendRoot();
        string repoRoot = Path.GetFullPath(Path.Combine(backendRoot, "..", ".."));

        string inPath = Path.Combine(repoRoot, "samples", "data", "mvp03", "input.txt");
        string expectedPath = Path.Combine(repoRoot, "samples", "data", "mvp03", "expected.txt");
        string outPath = Path.Combine(Path.GetTempPath(), "mvp03_test_out_" + Guid.NewGuid().ToString("N") + ".txt");

        try
        {
            Mvp03Program.ProcessFile(inPath, outPath);

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
