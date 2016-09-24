using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.FxUtils
{
    public class CLIArgsParser
    {
        public const string DefaultOptionDelimiter = "/";

        #region Constructor
        public CLIArgsParser(string optionDelimiter = DefaultOptionDelimiter)
        {
            if (string.IsNullOrWhiteSpace(optionDelimiter))
            {
                throw new ArgumentException("ArgsDelimiter can't be null or empty.");
            }
            ArgsDelimiter = optionDelimiter;
        }
        #endregion

        #region Properties
        public string ArgsDelimiter { get; protected set; }
        #endregion

        #region Methods
        public string Parse(string[] args, CLIArgument[] cliArgs, Dictionary<string, List<string>> parsedArguments)
        {
            string errorMsg = string.Empty;
            for (int i = 0; i < args.Length; i++)
            {
                List<string> paramsList = null;
                var cliArg = (from arg in cliArgs
                              where (arg.Name.Equals(args[i]) ||
                              (arg.ShortVersion != null && arg.ShortVersion.Equals(args[i])))
                              select arg).FirstOrDefault();
                if (cliArg == null)
                {
                    if (!string.IsNullOrWhiteSpace(errorMsg))
                    {
                        errorMsg += Environment.NewLine;
                    }
                    errorMsg += string.Format("Unable to parse / recognize Command Line Argument \"{0}\" at position {1}.", args[i], i);
                    continue;
                }
                if (cliArg.ParameterNames != null)
                {
                    paramsList = new List<string>(cliArg.ParameterNames.Length);
                    for (int j = 0; j < cliArg.ParameterNames.Length; j++)
                    {
                        try
                        {
                            paramsList.Add(args[i + j + 1]);
                        }
                        catch(IndexOutOfRangeException)
                        {
                            if (!string.IsNullOrWhiteSpace(errorMsg))
                            {
                                errorMsg += Environment.NewLine;
                            }
                            errorMsg += string.Format("Unable to parse Command Line Argument \"{0}\" at position {1}; Not enough arguments.  {2}For more details on the usage of the parameter,{2}{3}", args[i], i, Environment.NewLine, cliArg.ToLongString());
                        }
                    }
                    i += cliArg.ParameterNames.Length;
                }
                parsedArguments[cliArg.Name] = paramsList;
            }
            return errorMsg;
        }
        #endregion
    }
}
