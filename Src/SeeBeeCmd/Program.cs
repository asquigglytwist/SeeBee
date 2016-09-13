using System;
using SeeBee.PMLParser;

namespace SeeBee.SeeBeeCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            PMLAnalyzer pmlAnalyzer;
#if !DEBUG
            pmlAnalyzer = new PMLAnalyzer();
            pmlAnalyzer.Init(args);
#else
            pmlAnalyzer = new PMLAnalyzer(@"C:\T\SeeBee\Procmon.exe");
            pmlAnalyzer.Init(@"C:\T\SeeBee\Logfile.PML");
            Console.ReadKey(true);
#endif
        }
    }
}
