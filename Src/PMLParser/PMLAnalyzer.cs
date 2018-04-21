using System.Collections.Generic;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.Analysis;
using SeeBee.PMLParser.ConfigManager;
using SeeBee.PMLParser.Conversion;
using SeeBee.PMLParser.PMLEntities;

using CommandProcessorOutput = System.Tuple<System.Collections.Generic.List<string>, string, string, string>;

namespace SeeBee.PMLParser
{
    public static class PMLAnalyzer
    {
        #region Members
        static string procMonExePath, inputFilePath;
        static PMLFile parsedPMLFile;
        #endregion

        #region Private Methods
        private static bool Convert(string pmlFile, out string xmlFile)
        {
            PMLToXMLConverter converter;
            converter = new PMLToXMLConverter(procMonExePath, pmlFile);
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
            CommandProcessorOutput returnValue = CommandProcessor.ParseCommandLine(args);
            procMonExePath = returnValue.Item2;
            inputFilePath = returnValue.Item3;
            return returnValue.Item1;
        }

        internal static bool ProcessPMLFile()
        {
            var fileToParse = inputFilePath;
            var didCoversionHappen = false;
            if (inputFilePath.EndsWith(".pml", System.StringComparison.CurrentCultureIgnoreCase))
            {
                didCoversionHappen = Convert(inputFilePath, out var outputXmlFile) && !string.IsNullOrWhiteSpace(outputXmlFile);
                fileToParse = outputXmlFile;
            }
            parsedPMLFile = ConvertedXMLProcessor.PopulateProcessesAndEvents(fileToParse);
            if (didCoversionHappen)
            {
                FSUtils.FileDelete(fileToParse);
            }
            return (parsedPMLFile == null);
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
