using System;
using SeeBee.PMLParser;

namespace SeeBee.SeeBeeCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            PMLAnalyzer pmlAnalyzer = new PMLAnalyzer();
            pmlAnalyzer.Analyze(args);
#if DEBUG
            Console.ReadKey(true);
#endif
        }
    }
}
