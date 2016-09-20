using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SeeBee.FxUtils;

namespace SeeBee.PMLParser
{
    public class PMLAnalyzer
    {
        internal static List<string> globalOwnerList = new List<string>();
        internal static List<PMLModule> globalModuleList = new List<PMLModule>();

        static PMLAnalyzer()
        {
            globalModuleList.Add(PMLModule.System);
            globalOwnerList.Add("NT AUTHORITY\\SYSTEM");
            InitAllCLIArgs();
        }

        private static void InitAllCLIArgs()
        {
            CLIArgument[] cliArgs = new CLIArgument[]
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
            foreach (var arg in cliArgs)
            {
                Console.WriteLine(arg.ToString());
            }
#endif
        }

        internal static int LocateOwnerInList(string owner)
        {
            return globalOwnerList.FindIndex(o => o.Equals(owner, StringComparison.CurrentCultureIgnoreCase));
        }

        internal static int AddOwnerToList(string owner)
        {
            globalOwnerList.Add(owner);
            return globalOwnerList.Count - 1;
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
            if (Convert(pmlFile, out xmlFile) && !string.IsNullOrWhiteSpace(xmlFile))
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
