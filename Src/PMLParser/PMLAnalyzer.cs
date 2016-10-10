using System.Collections.Generic;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.Analysis;
using SeeBee.PMLParser.ConfigManager;
using SeeBee.PMLParser.Conversion;
using SeeBee.PMLParser.PMLEntities;

namespace SeeBee.PMLParser
{
    public static class PMLAnalyzer
    {
        #region Members
        static PMLFile processedPMLFile;
        #endregion

        #region Private Methods
        private static bool Convert(string pmlFile, out string xmlFile)
        {
            PMLToXMLConverter converter;
            converter = new PMLToXMLConverter(CommandProcessor.ProcMonExePath, pmlFile);
            if (converter.Convert())
            {
                xmlFile = converter.XMLFile;
                return true;
            }
            xmlFile = null;
            return false;
        }
        #endregion
        
        #region Internal Methods
        internal static List<string> Init(string[] args)
        {
            List<string> returnValue = null;
            returnValue = CommandProcessor.ParseCommandLine(args);
            return returnValue;
        }

        internal static bool ProcessPMLFile()
        {
            string xmlFile;
            if (Convert(CommandProcessor.InputFilePath, out xmlFile) && !string.IsNullOrWhiteSpace(xmlFile))
            {
                processedPMLFile = ConvertedXMLProcessor.PopulateProcessesAndEvents(xmlFile);
                FSUtils.FileDelete(xmlFile);
                return true;
            }
            return false;
        }
        #endregion

        #region Public APIs
        public static List<string> InitAndAnalyze(out bool processingPMLResult, string[] args)
        {
            List<string> resultMsgs = Init(args);
            processingPMLResult = false;
            if (resultMsgs.Count == 0)
            {
                processingPMLResult = ProcessPMLFile();
            }
            return resultMsgs;
        }
        #endregion
    }
}
