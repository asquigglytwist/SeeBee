using System;
using System.IO;
using System.Linq;
using SeeBee.FxUtils;
using System.Collections.Generic;

namespace SeeBee.PMLParser
{
    public class PMLAnalyzer
    {
        internal static List<PMLModule> globalModuleList;

        public PMLAnalyzer(string procMonExeLocation)
        {
            if (!FSUtils.FileExists(procMonExeLocation))
            {
                throw new FileNotFoundException("Not able to, either find or access the ProcMon executable (file).", ProcMonEXELocation);
            }
            ProcMonEXELocation = procMonExeLocation;
        }

        public string ProcMonEXELocation { get; private set; }

        public bool ProcessPMLFile(string pmlFile)
        {
            if (!FSUtils.FileExists(pmlFile))
            {
                throw new FileNotFoundException("Not able to, either find or access the ProcMon Log file.", pmlFile);
            }
            string xmlFile;
            if (Convert(pmlFile, out xmlFile) && !string.IsNullOrEmpty(xmlFile))
            {
                ConvertedXMLProcessor processList = new ConvertedXMLProcessor();
                var processes = from p in processList.LoadProcesses(xmlFile) where (!string.IsNullOrEmpty(p.ProcessName)) select p;
#if DEBUG
                Console.WriteLine("# of Processes that match the criteria {0}.", processes.Count());
#endif
                File.Delete(xmlFile);
                return true;
            }
            return false;
        }

        private bool Convert(string pmlFile, out string xmlFile)
        {
            PMLToXMLConverter converter;
            converter = new PMLToXMLConverter(ProcMonEXELocation, pmlFile);
            if (converter.Convert())
            {
                xmlFile = converter.XMLFile;
                return true;
            }
            xmlFile = null;
            return false;
        }
    }
}
