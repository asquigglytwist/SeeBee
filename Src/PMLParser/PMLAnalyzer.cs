using System.IO;
using SeeBee.FxUtils;

namespace SeeBee.PMLParser
{
    public class PMLAnalyzer
    {
        public PMLAnalyzer(string procMonExeLocation, string pmlFileFirst, string pmlFileSecond = null)
        {
            if (!FSUtils.FileExists(procMonExeLocation))
            {
                throw new FileNotFoundException("SeeBee was not able to, either find or access the ProcMon executable (file).", ProcMonEXELocation);
            }
            ProcMonEXELocation = procMonExeLocation;

            if (!FSUtils.FileExists(pmlFileFirst))
            {
                throw new FileNotFoundException("SeeBee was not able to, either find or access the ProcMon Log file (first).", pmlFileFirst);
            }
            PMLFileFirst = pmlFileFirst;

            if(!string.IsNullOrEmpty(pmlFileSecond) && !FSUtils.FileExists(pmlFileSecond))
            {
                throw new FileNotFoundException("SeeBee was not able to, either find or access the ProcMon Log file (second).", pmlFileSecond);
            }
            else
            {
                PMLFileSecond = pmlFileSecond;
            }
        }

        public string ProcMonEXELocation { get; private set; }
        public string PMLFileFirst { get; private set; }
        public string XMLFileFirst { get; private set; }
        public string PMLFileSecond { get; private set; }
        public string XMLFileSecond { get; private set; }

        public bool Init()
        {
            string xmlFilePath;
            if (Convert() && !string.IsNullOrEmpty(XMLFileFirst))
            {
                XMLProcessor processList = new XMLProcessor();
                processList.LoadProcesses();
                return true;
            }
            return false;
        }

        private bool Convert(bool useFirstPMLFile = true)
        {
            PMLToXMLConverter converter;
            if (useFirstPMLFile)
            {
                converter = new PMLToXMLConverter(ProcMonEXELocation, PMLFileFirst);
            }
            else
            {
                converter = new PMLToXMLConverter(ProcMonEXELocation, PMLFileSecond);
            }
            if (converter.Convert())
            {
                if (useFirstPMLFile)
                {
                    XMLFileFirst = converter.XMLFile;
                }
                else
                {
                    XMLFileSecond = converter.XMLFile;
                }
                return true;
            }
            return false;
        }
    }
}
