using System;
using System.IO;
using System.Text;

namespace CobolMvpRuntime
{
    // MVP09 target: verify PERFORM paragraph and PERFORM THRU
    internal static class Mvp09Program
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
            int wsFlag = int.Parse(input);
            string wsMark = " ";
            char[] wsOut = { ' ', ' ', ' ' };

            // 1) IF WS-FLAG = 1 THEN PERFORM PARA-MARK END-IF
            if (wsFlag == 1)
            {
                ParaMark(ref wsMark);
            }

            // 2) PERFORM PARA-A THRU PARA-C-EXIT
            PerformParaAThruParaCExit(wsOut);

            // 3) DISPLAY equivalent
            return "MARK=" + wsMark + "|SEQ=" + new string(wsOut);
        }

        private static void ParaMark(ref string wsMark)
        {
            wsMark = "Y";
        }

        private static void PerformParaAThruParaCExit(char[] wsOut)
        {
            ParaA(wsOut);
            ParaB(wsOut);
            ParaC(wsOut);
            ParaCExit();
        }

        private static void ParaA(char[] wsOut)
        {
            wsOut[0] = 'A';
        }

        private static void ParaB(char[] wsOut)
        {
            wsOut[1] = 'B';
        }

        private static void ParaC(char[] wsOut)
        {
            wsOut[2] = 'C';
        }

        private static void ParaCExit()
        {
        }
    }
}
