using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeeBee.FxUtils.CLIArgs;
using SeeBee.FxUtils.Utils;

using CommandProcessorOutput = System.Tuple<System.Collections.Generic.List<string>, string, string, string>;

namespace SeeBee.PMLParser.ConfigManager
{
    internal static class CommandProcessor
    {
        #region Members
        private static CLIArgument ProcMonExe = new CLIArgument("pm", "procmon", true, new string[] { "filename" }, "Path to the ProcMon (Process Monitor) executable.", @"in C:\SysInternals\ProcMon\ProcMon.exe"),
            InFilePath = new CLIArgument("in", "inputfile", true, new string[] { "filename" }, "The input Process Monitor Log (PML) file that is to be processed.", @"in C:\Logs\LogFile.PML"),
            OutFilePath = new CLIArgument("out", "outputfile", false, new string[] { "filename" }, "Location for the output XML file to be stored.", @"out C:\Logs\AfterConversion.XML"),
            Config = new CLIArgument("c", "config", true, new string[] { "filename" }, "Input configuration file that dictates SeeBee's behavior.");
        private static CLIArgsParser argsParser = new CLIArgsParser();
        private static Dictionary<string, List<string>> parsedArguments = new Dictionary<string, List<string>>();
        #endregion

        private static CLIArgument[] InitAllCLIArgs()
        {
            CLIArgument[] cliKnownArgs = new CLIArgument[]
            {
                ProcMonExe,
                InFilePath,
                OutFilePath,
                Config
            };
            return cliKnownArgs;
        }

        internal static CommandProcessorOutput ParseCommandLine(string[] args)
        {
            List<string> cliParserOutput = argsParser.Parse(args, InitAllCLIArgs(), parsedArguments);
            string procMonExePath = null, inputFilePath = null;
            if (cliParserOutput.Count == 0)
            {
                List<string> tempList;
                if (parsedArguments.TryGetValue(ProcMonExe.Name, out tempList))
                {
                    procMonExePath = tempList.First();
                }
                FSUtils.FileExists(procMonExePath, "Not able to, either find or access the ProcMon executable (file).");
                if (parsedArguments.TryGetValue(InFilePath.Name, out tempList))
                {
                    inputFilePath = tempList.First();
                }
                FSUtils.FileExists(inputFilePath, "Not able to, either find or access the ProcMon Logs file.");
                if (parsedArguments.TryGetValue(Config.Name, out tempList))
                {
                    AppConfigFilePath = tempList.First();
                }
                FSUtils.FileExists(AppConfigFilePath, "Application Configuration File was not found.");
            }
            return Tuple.Create(cliParserOutput, procMonExePath, inputFilePath, AppConfigFilePath);
        }

        #region Properties
        internal static string AppConfigFilePath { get; private set; }
        #endregion
    }
}
