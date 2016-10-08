﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SeeBee.FxUtils;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.Conversion;
using SeeBee.PMLParser.PMLEntities;

namespace SeeBee.PMLParser
{
    public static class PMLAnalyzer
    {
        #region Members
        static IndexedStringCollection OwnerList = new IndexedStringCollection();
        static IndexedStringCollection KnownFilePaths = new IndexedStringCollection();
        static List<PMLModule> globalModuleList = new List<PMLModule>();
        static CLIArgument ProcMonExe = new CLIArgument("pm", "procmon", true, new string[] { "filename" }, "Path to the ProcMon (Process Monitor) executable.", @"in C:\SysInternals\ProcMon\ProcMon.exe"),
            InFilePath = new CLIArgument("in", "inputfile", true, new string[] { "filename" }, "The input Process Monitor Log (PML) file that is to be processed.", @"in C:\Logs\LogFile.PML"),
            OutFilePath = new CLIArgument("out", "outputfile", false, new string[] { "filename" }, "Location for the output XML file to be stored.", @"out C:\Logs\AfterConversion.XML"),
            ProcessId = new CLIArgument("pid", "processid", true, new string[] { "pid" }, "PID of the process to be filtered.", "pid 1234"),
            ImageName = new CLIArgument("im", "image", false, new string[] { "imagename" }, "Image name of the process to be filtered.", "im explorer.exe"),
            IgnorePass = new CLIArgument("ip", "ignorepass", false, null, "Ignores all opertaions for which the result is SUCCESS.", "ip"),
            Result = new CLIArgument(null, "result", false, new string[] { "result" }, "Filters results based on the value specified.  Accepts \"pass\" and \"fail\" as the only valid values.", "result pass"),
            Filter = new CLIArgument("fi", "filter", false, new string[] { "field", "operator", "value" }, "Filters results based on the value specified.  Accepts \"pass\" and \"fail\" as the only valid values.", "result pass");
        #endregion

        #region Private Methods
        private static CLIArgument[] InitAllCLIArgs()
        {
            CLIArgument[] cliKnownArgs = new CLIArgument[]
            {
                ProcMonExe,
                InFilePath,
                OutFilePath,
                ProcessId,
                ImageName,
                IgnorePass,
                Result,
                Filter
            };
            return cliKnownArgs;
        }

        private static bool Convert(string pmlFile, out string xmlFile)
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
        #endregion
        
        #region Internal Methods
        #region Owner List
        internal static int LocateOwnerInList(string owner)
        {
            return OwnerList.LocateString(owner);
        }

        internal static int AddOwnerToList(string owner)
        {
            return OwnerList.Add(owner);
        }

        internal static string GetOwnerName(int index)
        {
            return OwnerList.StringAt(index);
        }
        #endregion

        #region FilePath List
        internal static int LocateFilePathInList(string filePath)
        {
            return KnownFilePaths.LocateString(filePath);
        }

        internal static int AddFilePathToList(string filePath)
        {
            return KnownFilePaths.Add(filePath);
        }

        internal static string GetFilePath(int index)
        {
            return KnownFilePaths.StringAt(index);
        }
        #endregion

        #region Module List
        internal static int LocateModuleInList(string modulePath)
        {
            return globalModuleList.FindIndex(module => module.Path.Equals(modulePath, StringComparison.CurrentCultureIgnoreCase));
        }

        internal static int AddModuleToList(PMLModule module)
        {
            globalModuleList.Add(module);
            return globalModuleList.Count - 1;
        }

        internal static string GetModuleDescription(int index)
        {
            return globalModuleList[index].Description;
        }
        #endregion

        internal static string Init(string[] args)
        {
            globalModuleList.Add(PMLModule.System);
            OwnerList.Add("NT AUTHORITY\\SYSTEM");

            string returnValue = null;
            CLIArgsParser argsParser = new CLIArgsParser();
            Dictionary<string, List<string>> parsedArguments = new Dictionary<string, List<string>>();
#if DEBUG
            args = new string[] { "pm", @"C:\T\SeeBee\Procmon.exe", "in", @"C:\T\SeeBee\Logfile.PML", "pid", "*", "ip" };
#endif
            returnValue = argsParser.Parse(args, InitAllCLIArgs(), parsedArguments);
            if (string.IsNullOrWhiteSpace(returnValue))
            {
                ProcMonEXELocation = parsedArguments[ProcMonExe.Name].First();
                if (!FSUtils.FileExists(ProcMonEXELocation))
                {
                    throw new FileNotFoundException("Not able to, either find or access the ProcMon executable (file).", ProcMonEXELocation);
                }
                PMLFile = parsedArguments[InFilePath.Name].First();
                if (!FSUtils.FileExists(PMLFile))
                {
                    throw new FileNotFoundException("Not able to, either find or access the ProcMon Logs file.", PMLFile);
                }
            }
            return returnValue;
        }

        internal static bool ProcessPMLFile()
        {
            if (!FSUtils.FileExists(PMLFile))
            {
                throw new FileNotFoundException("Not able to, either find or access the ProcMon Log file.", PMLFile);
            }
            string xmlFile;
            if (Convert(PMLFile, out xmlFile) && !string.IsNullOrWhiteSpace(xmlFile))
            {
                ConvertedXMLProcessor processList = new ConvertedXMLProcessor();
                var processes = from p in processList.LoadProcesses(xmlFile) where (!string.IsNullOrWhiteSpace(p.ProcessName)) select p;
#if DEBUG
                Console.WriteLine("# of Processes that match the criteria {0}.", processes.Count());
#endif
                var events = from e in processList.LoadEvents(xmlFile) where (!string.IsNullOrWhiteSpace(e.TimeOfDay.ToString())) select e;
#if DEBUG
                Console.WriteLine("# of Events that match the criteria {0}.", events.Count());
#endif
                File.Delete(xmlFile);
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
        
        public static string ProcMonEXELocation { get; private set; }
        public static string PMLFile { get; private set; }
        #endregion
    }
}
