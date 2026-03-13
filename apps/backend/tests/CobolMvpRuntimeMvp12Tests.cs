using System;
using System.IO;
using System.Text;
using CobolMvpRuntime;
using Xunit;

namespace CobolToCsharpMigration.Tests;

public class CobolMvpRuntimeMvp12Tests
{
    [Theory]
    [InlineData("input_empty.txt", "expected_empty.txt")]
    [InlineData("input_1line.txt", "expected_1line.txt")]
    [InlineData("input.txt", "expected.txt")]
    public void Run_Mvp12Sample_MatchesExpectedStdout(string inputFile, string expectedFile)
    {
        string backendRoot = GetBackendRoot();
        string repoRoot = Path.GetFullPath(Path.Combine(backendRoot, "..", ".."));

        string sampleInputPath = Path.Combine(repoRoot, "samples", "data", "mvp12", inputFile);
        string expectedPath = Path.Combine(repoRoot, "samples", "data", "mvp12", expectedFile);

        string tempDir = Path.Combine(Path.GetTempPath(), "mvp12_test_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        string runtimeInputPath = Path.Combine(tempDir, "mvp12-input.txt");
        File.Copy(sampleInputPath, runtimeInputPath);

        string? original = Environment.GetEnvironmentVariable("MVP12_INPUT");
        try
        {
            Environment.SetEnvironmentVariable("MVP12_INPUT", runtimeInputPath);

            var output = new StringWriter();
            output.NewLine = "\r\n";
            Mvp12Program.Run(output);

            string expected = NormalizeNewLine(File.ReadAllText(expectedPath, Encoding.ASCII));
            string actual = NormalizeNewLine(output.ToString());
            Assert.Equal(expected, actual);
        }
        finally
        {
            Environment.SetEnvironmentVariable("MVP12_INPUT", original);
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }

    [Fact]
    public void SequentialFileReader_CurrentRecord_AfterReadNext_ReturnsCorrectValue()
    {
        string backendRoot = GetBackendRoot();
        string repoRoot = Path.GetFullPath(Path.Combine(backendRoot, "..", ".."));
        string inputPath = Path.Combine(repoRoot, "samples", "data", "mvp12", "input.txt");

        using var reader = new SequentialFileReader(inputPath);
        Assert.Equal(string.Empty, reader.CurrentRecord);

        Assert.True(reader.ReadNext());
        Assert.Equal("AAA", reader.CurrentRecord);

        Assert.True(reader.ReadNext());
        Assert.Equal("BBBB", reader.CurrentRecord);

        Assert.True(reader.ReadNext());
        Assert.Equal("CC", reader.CurrentRecord);

        Assert.False(reader.ReadNext());
        Assert.Equal(string.Empty, reader.CurrentRecord);
    }

    [Fact]
    public void SequentialFileReader_ReadInto_ThrowsNotSupportedException()
    {
        string backendRoot = GetBackendRoot();
        string repoRoot = Path.GetFullPath(Path.Combine(backendRoot, "..", ".."));
        string inputPath = Path.Combine(repoRoot, "samples", "data", "mvp12", "input.txt");

        using var reader = new SequentialFileReader(inputPath);
        var ex = Assert.Throws<NotSupportedException>(() => reader.ReadInto());
        Assert.Contains("READ INTO", ex.Message);
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
