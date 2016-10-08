using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeeBee.FxUtils.CLIArgs;
using SeeBee.FxUtils.Utils;
using System.IO;

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

        internal static string ParseCommandLine(string[] args)
        {
            string cliParserOutput = null;
            argsParser.Parse(args, InitAllCLIArgs(), parsedArguments);
            if (string.IsNullOrWhiteSpace(cliParserOutput))
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
            return cliParserOutput;
        }

        #region Properties
        internal static string ProcMonEXELocation { get; private set; }
        internal static string PMLFile { get; private set; }
        #endregion
    }
}
