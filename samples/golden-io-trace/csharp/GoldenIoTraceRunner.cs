using CobolMvpRuntime;

namespace GoldenIoTraceSample
{
    internal static class GoldenIoTraceRunner
    {
        internal static void Run(string inputPath, string outputPath, string tracePath, string cobolSourcePath)
        {
            CobolMvpRuntime.GoldenIoTraceRunner.Run(inputPath, outputPath, tracePath, cobolSourcePath);
        }
    }
}
