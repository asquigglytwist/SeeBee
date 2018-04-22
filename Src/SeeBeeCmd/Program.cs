using System;
using SeeBee.PMLParser;
using System.Collections.Generic;

namespace SeeBee.SeeBeeCmd
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            args = new string[] { "pm", @"C:\T\SeeBee\Procmon.exe", "in", @"C:\T\SeeBee\Logfile.XML", "c", @"C:\T\SeeBee\SeeBee.sbc" };
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine("Arg at {0};\t{1}", i, args[i]);
            }
#endif
            List<string> errorMsgs = PMLAnalyzer.InitAndAnalyze(out bool processingPMLResult, args);
            if (errorMsgs.Count != 0)
            {
                // [BIB]:  http://stackoverflow.com/questions/759133/how-to-display-list-items-on-console-window-in-c-sharp
                errorMsgs.ForEach(Console.WriteLine);
            }
#if DEBUG
            Console.ReadKey(true);
#endif
        }
    }
}
