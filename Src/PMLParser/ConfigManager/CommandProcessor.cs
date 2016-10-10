using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeeBee.FxUtils.CLIArgs;
using SeeBee.FxUtils.Utils;

namespace SeeBee.PMLParser.ConfigManager
{
    internal static class CommandProcessor
    {
        #region Members
        private static CLIArgument ProcMonExe = new CLIArgument("pm", "procmon", true, new string[] { "filename" }, "Path to the ProcMon (Process Monitor) executable.", @"in C:\SysInternals\ProcMon\ProcMon.exe"),
            InFilePath = new CLIArgument("in", "inputfile", true, new string[] { "filename" }, "The input Process Monitor Log (PML) file that is to be processed.", @"in C:\Logs\LogFile.PML"),
            OutFilePath = new CLIArgument("out", "outputfile", false, new string[] { "filename" }, "Location for the output XML file to be stored.", @"out C:\Logs\AfterConversion.XML"),
            Config = new CLIArgument("c", "config", true, null, "Input configuration file that dictates SeeBee's behavior.");
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

        internal static List<string> ParseCommandLine(string[] args)
        {
            List<string> cliParserOutput = argsParser.Parse(args, InitAllCLIArgs(), parsedArguments);
            if (cliParserOutput.Count == 0)
            {
                ProcMonExePath = parsedArguments[ProcMonExe.Name].First();
                FSUtils.FileExists(ProcMonExePath, "Not able to, either find or access the ProcMon executable (file).");
                InputFilePath = parsedArguments[InFilePath.Name].First();
                FSUtils.FileExists(InputFilePath, "Not able to, either find or access the ProcMon Logs file.");
                //AppConfigFilePath = parsedArguments[Config.Name].First();
                //FSUtils.FileExists(AppConfigFilePath, "Application Configuration File was not found.");
            }
            return cliParserOutput;
        }

        #region Properties
        internal static string ProcMonExePath { get; private set; }
        internal static string InputFilePath { get; private set; }
        internal static string AppConfigFilePath { get; private set; }
        #endregion
    }
}
