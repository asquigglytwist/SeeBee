using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.PMLParser
{
    public class PMLAnalyzer
    {
        public bool Analyze(string[] args)
        {
            string xmlFilePath;
            if (Conversion(out xmlFilePath, args))// && !string.IsNullOrEmpty(xmlFilePath))
            {
                if (!string.IsNullOrEmpty(xmlFilePath))
                {
                    XMLProcessor processList = new XMLProcessor();
                    processList.ListProcesses(xmlFilePath);
                    return true;
                }
            }
            return false;
        }

        private bool Conversion(out string xmlFile, string[] args)
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
            PMLToXMLConverter converter = new PMLToXMLConverter(procMonExeLocation, pmlFile);
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
