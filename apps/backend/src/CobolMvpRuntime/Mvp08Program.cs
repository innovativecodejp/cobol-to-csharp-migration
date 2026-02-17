using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    // MVP08 target: verify EVALUATE TRUE ALSO TRUE
    internal static class Mvp08Program
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
            int commaIndex = input.IndexOf(',');
            if (commaIndex <= 0 || commaIndex >= input.Length - 1)
            {
                throw new InvalidOperationException("Input must be AGE,GENDER format.");
            }

            int wsAge = int.Parse(input.Substring(0, commaIndex));
            string wsGender = input.Substring(commaIndex + 1, 1);
            string wsClass;

            if ((wsAge >= 20) && (wsGender == "M"))
            {
                wsClass = "ADULT-M";
            }
            else if ((wsAge >= 20) && (wsGender == "F"))
            {
                wsClass = "ADULT-F";
            }
            else if ((wsAge < 20) && (wsGender == "M"))
            {
                wsClass = "MINOR-M";
            }
            else if ((wsAge < 20) && (wsGender == "F"))
            {
                wsClass = "MINOR-F";
            }
            else
            {
                wsClass = "OTHER";
            }

            return "AGE=" + wsAge + "|GENDER=" + wsGender + "|CLASS=" + wsClass;
        }
    }
}
