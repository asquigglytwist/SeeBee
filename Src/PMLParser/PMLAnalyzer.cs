using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.Analysis;
using SeeBee.PMLParser.ConfigManager;
using SeeBee.PMLParser.Conversion;
using SeeBee.PMLParser.ManagedLists;
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
        internal static string Init(string[] args)
        {
            ModuleList.AddModuleToList(PMLModule.System);
            string returnValue = null;
#if DEBUG
            args = new string[] { "pm", @"C:\T\SeeBee\Procmon.exe", "in", @"C:\T\SeeBee\Logfile.PML" };
#endif
            returnValue = CommandProcessor.ParseCommandLine(args);
            return returnValue;
        }

        internal static bool ProcessPMLFile()
        {
            string xmlFile;
            if (Convert(CommandProcessor.PMLFilePath, out xmlFile) && !string.IsNullOrWhiteSpace(xmlFile))
            {
                processedPMLFile = ConvertedXMLProcessor.PopulateProcessesAndEvents(xmlFile);
                FSUtils.FileDelete(xmlFile);
                return true;
            }
            return false;
        }
        #endregion

        #region Public APIs
        public static string InitAndAnalyze(out bool processingPMLResult,string[] args)
        {
            string resultMsg = Init(args);
            processingPMLResult = false;
            if (string.IsNullOrWhiteSpace(resultMsg))
            {
                processingPMLResult = ProcessPMLFile();
            }
            return resultMsg;
        }
        #endregion
    }
}
