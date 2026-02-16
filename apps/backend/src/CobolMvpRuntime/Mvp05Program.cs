using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    // MVP05 target: verify INSPECT CONVERTING
    // CONVERTING "ABCDEFGHIJKLMNOPQRSTUVWXYZ" TO "abcdefghijklmnopqrstuvwxyz"
    internal static class Mvp05Program
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
            var chars = input.ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                char c = chars[i];
                if (c >= 'A' && c <= 'Z')
                {
                    chars[i] = (char)(c - 'A' + 'a');
                }
            }

            return "TEXT=" + new string(chars);
        }
    }
}
