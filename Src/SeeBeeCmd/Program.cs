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
            args = new string[] { "pm", @"C:\T\SeeBee\Procmon.exe", "in", @"C:\T\SeeBee\Logfile.PML" };
            for (int i = 0; i < args.Length; i++)
            {
                Console.Write("Arg at {0};", i);
                int j = args[i].IndexOf(':');
                if (j > -1)
                {
                    Console.WriteLine("\nOption:   {0};\nArgument:   {1}", args[i].Substring(0, j), args[i].Substring(j + 1));
                }
                else
                {
                    Console.WriteLine(args[i]);
                }
            }
#endif
            bool processingPMLResult;
            List<string> errorMsgs = PMLAnalyzer.InitAndAnalyze(out processingPMLResult, args);
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
