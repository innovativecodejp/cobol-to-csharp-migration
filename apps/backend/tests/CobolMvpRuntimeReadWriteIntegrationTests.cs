using System;
using System.IO;
using System.Text;
using CobolMvpRuntime;
using Xunit;

namespace CobolToCsharpMigration.Tests;

/// <summary>
/// MVP14 Integration: READ -> WRITE end-to-end verification.
/// Proves MVP12 (SequentialFileReader) + MVP13 (SequentialFileWriter) work together.
/// </summary>
public class CobolMvpRuntimeReadWriteIntegrationTests
{
    [Fact]
    public void ProcessFile_EmptyInput_ProducesEmptyOutput()
    {
        string backendRoot = GetBackendRoot();
        string repoRoot = Path.GetFullPath(Path.Combine(backendRoot, "..", ".."));
        string inputPath = Path.Combine(repoRoot, "samples", "data", "mvp13", "input_empty.txt");
        string expectedPath = Path.Combine(repoRoot, "samples", "data", "mvp13", "expected_empty.txt");

        string outPath = Path.Combine(Path.GetTempPath(), "mvp14_int_" + Guid.NewGuid().ToString("N") + ".txt");
        try
        {
            Mvp13Program.ProcessFile(inputPath, outPath);

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

    [Fact]
    public void ProcessFile_SingleLine_Passthrough()
    {
        string backendRoot = GetBackendRoot();
        string repoRoot = Path.GetFullPath(Path.Combine(backendRoot, "..", ".."));
        string inputPath = Path.Combine(repoRoot, "samples", "data", "mvp13", "input_1line.txt");
        string expectedPath = Path.Combine(repoRoot, "samples", "data", "mvp13", "expected_1line.txt");

        string outPath = Path.Combine(Path.GetTempPath(), "mvp14_int_" + Guid.NewGuid().ToString("N") + ".txt");
        try
        {
            Mvp13Program.ProcessFile(inputPath, outPath);

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

    [Fact]
    public void ProcessFile_MultipleLines_Passthrough()
    {
        string backendRoot = GetBackendRoot();
        string repoRoot = Path.GetFullPath(Path.Combine(backendRoot, "..", ".."));
        string inputPath = Path.Combine(repoRoot, "samples", "data", "mvp13", "input_multiline.txt");
        string expectedPath = Path.Combine(repoRoot, "samples", "data", "mvp13", "expected_multiline.txt");

        string outPath = Path.Combine(Path.GetTempPath(), "mvp14_int_" + Guid.NewGuid().ToString("N") + ".txt");
        try
        {
            Mvp13Program.ProcessFile(inputPath, outPath);

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

    [Fact]
    public void ProcessFile_DoesNotLoopForever_OnEof()
    {
        string backendRoot = GetBackendRoot();
        string repoRoot = Path.GetFullPath(Path.Combine(backendRoot, "..", ".."));
        string inputPath = Path.Combine(repoRoot, "samples", "data", "mvp13", "input_empty.txt");

        string outPath = Path.Combine(Path.GetTempPath(), "mvp14_int_" + Guid.NewGuid().ToString("N") + ".txt");
        try
        {
            Mvp13Program.ProcessFile(inputPath, outPath);

            string actual = File.ReadAllText(outPath, Encoding.ASCII);
            Assert.Equal(string.Empty, actual);
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
