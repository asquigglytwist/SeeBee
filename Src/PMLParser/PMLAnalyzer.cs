using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SeeBee.FxUtils;

namespace SeeBee.PMLParser
{
    public static class PMLAnalyzer
    {
        #region Members
        static List<string> globalOwnerList = new List<string>();
        static List<PMLModule> globalModuleList = new List<PMLModule>();
        internal static CLIArgument[] cliKnownArgs;
        #endregion

        #region Private Methods
        static PMLAnalyzer()
        {
            globalModuleList.Add(PMLModule.System);
            globalOwnerList.Add("NT AUTHORITY\\SYSTEM");
            InitAllCLIArgs();
        }

        private static void InitAllCLIArgs()
        {
            cliKnownArgs = new CLIArgument[]
            {
                new CLIArgument("pm", "procmon", true, new string[] { "filename" }, "Path to the ProcMon (Process Monitor) executable.", @"in C:\SysInternals\ProcMon\ProcMon.exe"),
                new CLIArgument("in", "inputfile", true, new string[] { "filename" }, "The input Process Monitor Log (PML) file that is to be processed.", @"in C:\Logs\LogFile.PML"),
                new CLIArgument("out", "outputfile", false, new string[] { "filename" }, "Location for the output XML file to be stored.", @"out C:\Logs\AfterConversion.XML"),
                new CLIArgument("pid", "processid", true, new string[] { "pid" }, "PID of the process to be filtered.", "pid 1234"),
                new CLIArgument("im", "image", false, new string[] { "imagename" }, "Image name of the process to be filtered.", "im explorer.exe"),
                new CLIArgument("ip", "ignorepass", false, null, "Ignores all opertaions for which the result is SUCCESS.", "ip"),
                new CLIArgument(null, "result", false, new string[] { "result" }, "Filters results based on the value specified.  Accepts \"pass\" and \"fail\" as the only valid values.", "result pass"),
                new CLIArgument("fi", "filter", false, new string[] { "field", "operator", "value" }, "Filters results based on the value specified.  Accepts \"pass\" and \"fail\" as the only valid values.", "result pass")
            };
#if DEBUG
            foreach (var arg in cliKnownArgs)
            {
                Console.WriteLine(arg.ToString());
            }
#endif
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
        internal static int LocateOwnerInList(string owner)
        {
            return globalOwnerList.FindIndex(o => o.Equals(owner, StringComparison.CurrentCultureIgnoreCase));
        }

        internal static int AddOwnerToList(string owner)
        {
            globalOwnerList.Add(owner);
            return globalOwnerList.Count - 1;
        }

        internal static string GetOwnerName(int index)
        {
            return globalOwnerList[index];
        }

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

        public static void Init(string[] args)
        {
            CLIArgsParser argsParser = new CLIArgsParser();
            Dictionary<string, List<string>> parsedArguments = new Dictionary<string, List<string>>();
#if DEBUG
            args = new string[] { "pm", @"C:\T\SeeBee\Procmon.exe", "in", @"C:\T\SeeBee\Logfile.PML", "pid", "*", "ip" };
#endif
            argsParser.Parse(args, cliKnownArgs, parsedArguments);
            ProcMonEXELocation = parsedArguments[cliKnownArgs[0].Name].First();
            if (!FSUtils.FileExists(ProcMonEXELocation))
            {
                throw new FileNotFoundException("Not able to, either find or access the ProcMon executable (file).", ProcMonEXELocation);
            }
            PMLFile = parsedArguments[cliKnownArgs[1].Name].First();
            if (!FSUtils.FileExists(PMLFile))
            {
                throw new FileNotFoundException("Not able to, either find or access the ProcMon Logs file.", PMLFile);
            }
        }

        public static bool ProcessPMLFile()
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
                File.Delete(xmlFile);
                return true;
            }
            return false;
        }
        #endregion

        #region Public Methods
        public static string ProcMonEXELocation { get; private set; }
        public static string PMLFile { get; private set; }
        #endregion
    }
}
