using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    // MVP14 target: verify START KEY >= and READ NEXT
    internal static class Mvp14Program
    {
        internal static void Run(TextWriter writer)
        {
            string envPath = Environment.GetEnvironmentVariable("MVP14_INDEX");
            string indexPath = string.IsNullOrWhiteSpace(envPath)
                ? Path.Combine(Environment.CurrentDirectory, "mvp14-index.dat")
                : envPath;

            string caseRaw = Environment.GetEnvironmentVariable("MVP14_CASE");
            int caseNo;
            if (!int.TryParse(caseRaw, out caseNo))
            {
                caseNo = 1;
            }

            // A) Build minimal indexed source in key order.
            using (var outWriter = new StreamWriter(indexPath, false, Encoding.ASCII))
            {
                outWriter.WriteLine("0001|AAA");
                outWriter.WriteLine("0003|CCC");
                outWriter.WriteLine("0005|EEE");
            }

            // B) START ... KEY >= ... then READ NEXT.
            int startKey = caseNo == 2 ? 2 : 3;
            var records = new List<Record>();
            using (var inReader = new StreamReader(indexPath, Encoding.ASCII))
            {
                while (true)
                {
                    string line = inReader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }

                    string[] parts = line.Split('|');
                    if (parts.Length != 2)
                    {
                        continue;
                    }

                    int key;
                    if (!int.TryParse(parts[0], out key))
                    {
                        continue;
                    }

                    records.Add(new Record(key, parts[1]));
                }
            }

            int startIndex = -1;
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].Key >= startKey)
                {
                    startIndex = i;
                    break;
                }
            }

            if (startIndex >= 0)
            {
                for (int i = startIndex; i < records.Count; i++)
                {
                    writer.WriteLine("KEY=" + records[i].Key.ToString("D4") + "|TEXT=" + records[i].Text);
                }
            }

            writer.WriteLine("DONE");
        }

        private sealed class Record
        {
            internal Record(int key, string text)
            {
                Key = key;
                Text = text;
            }

            internal int Key { get; }

            internal string Text { get; }
        }
    }
}
