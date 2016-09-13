using System.IO;
using SeeBee.FxUtils;

namespace SeeBee.PMLParser
{
    public class PMLAnalyzer
    {
        public PMLAnalyzer(string procMonExeLocation)
        {
            if (!FSUtils.FileExists(procMonExeLocation))
            {
                throw new FileNotFoundException("SeeBee was not able to, either find or access the ProcMon executable (file).", ProcMonEXELocation);
            }
            ProcMonEXELocation = procMonExeLocation;
        }

        public string ProcMonEXELocation { get; private set; }

        public bool ProcessPMLFile(string pmlFile)
        {
            if (!FSUtils.FileExists(pmlFile))
            {
                throw new FileNotFoundException("SeeBee was not able to, either find or access the ProcMon Log file.", pmlFile);
            }
            string xmlFile;
            if (Convert(pmlFile, out xmlFile) && !string.IsNullOrEmpty(xmlFile))
            {
                XMLProcessor processList = new XMLProcessor();
                processList.LoadProcesses(xmlFile);
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
