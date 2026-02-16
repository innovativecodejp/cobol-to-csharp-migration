using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    // MVP03 target: verify UNSTRING derived features
    // DELIMITED BY ALL SPACE / COUNT / POINTER / TALLYING
    internal static class Mvp03Program
    {
        internal static void ProcessFile(string inPath, string outPath)
        {
            using (var reader = new StreamReader(inPath, Encoding.ASCII))
            using (var writer = new StreamWriter(outPath, false, Encoding.ASCII))
            {
                writer.NewLine = "\r\n";
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    writer.WriteLine(TransformRecord(line));
                }
            }
        }

        internal static string TransformRecord(string input)
        {
            int wsPtr = 1; // COBOL pointer is 1-based.
            int wsDelimCount = 0; // TALLYING IN: number of receiving fields acted upon.

            string wsA = string.Empty;
            string wsB = string.Empty;
            string wsC = string.Empty;

            int wsLenA = 0;
            int wsLenB = 0;
            int wsLenC = 0;

            wsA = ReadTokenDelimitedByAllSpace(input, ref wsPtr, out wsLenA);
            wsDelimCount++;

            wsB = ReadTokenDelimitedByAllSpace(input, ref wsPtr, out wsLenB);
            wsDelimCount++;

            wsC = ReadTokenDelimitedByAllSpace(input, ref wsPtr, out wsLenC);
            wsDelimCount++;

            return "A=" + wsA
                + "|LA=" + wsLenA
                + "|B=" + wsB
                + "|LB=" + wsLenB
                + "|C=" + wsC
                + "|LC=" + wsLenC
                + "|PTR=" + wsPtr
                + "|DC=" + wsDelimCount;
        }

        private static string ReadTokenDelimitedByAllSpace(string input, ref int pointer, out int length)
        {
            if (pointer < 1)
            {
                throw new InvalidOperationException("Pointer must be 1 or greater.");
            }

            if (pointer > input.Length)
            {
                length = 0;
                return string.Empty;
            }

            int index = pointer - 1;
            int start = index;

            while (index < input.Length && input[index] != ' ')
            {
                index++;
            }

            string token = input.Substring(start, index - start);
            length = token.Length;

            // DELIMITED BY ALL SPACE: collapse contiguous spaces as one delimiter group.
            while (index < input.Length && input[index] == ' ')
            {
                index++;
            }

            pointer = index + 1;
            return token;
        }
    }
}
