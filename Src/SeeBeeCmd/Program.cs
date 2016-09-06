using System;
using SeeBee.PMLParser;

namespace SeeBee.SeeBeeCmd
{
    class Program
    {
        static void Main(string[] args)
        {
            string xmlFilePath;
            if(Conversion(out xmlFilePath, args))// && !string.IsNullOrEmpty(xmlFilePath))
            {
                if (!string.IsNullOrEmpty(xmlFilePath))
                {
                    PMLParser.XMLAnalyzer processList = new PMLParser.XMLAnalyzer();
                    processList.ListProcesses(xmlFilePath);
                }
            }
#if DEBUG
            Console.ReadKey(true);
#endif
        }

        static bool Conversion(out string xmlFile, string[] args)
        {
            string procMonExeLocation, pmlFile;
            xmlFile = null;
#if DEBUG
            procMonExeLocation = @"C:\T\SeeBee\Procmon.exe";
            pmlFile = @"C:\T\SeeBee\Logfile.PML";
#else
            if (args.Length > 1 && !string.IsNullOrEmpty(args[0]) && !string.IsNullOrEmpty(args[1]))
            {
                procMonExeLocation = args[0];
                pmlFile = args[1];
#endif
                PMLParser.PMLToXMLConverter converter = new PMLParser.PMLToXMLConverter(procMonExeLocation, pmlFile);
                converter.Convert();
                xmlFile = converter.XMLFile;
                return true;
#if !DEBUG
            }
            return false;
#endif
        }
    }
}
