using System;
using SeeBee.PMLParser;

namespace SeeBee.SeeBeeCmd
{
    class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
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
            string errorMsg = PMLAnalyzer.InitAndAnalyze(out processingPMLResult, args);
#if DEBUG
            Console.ReadKey(true);
#endif
        }
    }
}
