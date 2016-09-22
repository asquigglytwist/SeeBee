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
        public void Parse(string[] args, CLIArgument[] cliArgs, Dictionary<string, List<string>> parsedArguments)
        {
            for (int i = 0; i < args.Length; i++)
            {
                List<string> paramsList = null;
                var cliArg = (from arg in cliArgs
                              where (arg.Name.Equals(args[i]) ||
                              (arg.ShortVersion != null && arg.ShortVersion.Equals(args[i])))
                              select arg).FirstOrDefault();
                if (cliArg.ParameterNames != null)
                {
                    paramsList = new List<string>(cliArg.ParameterNames.Length);
                    for (int j = 0; j < cliArg.ParameterNames.Length; j++)
                    {
                        paramsList.Add(args[i + j + 1]);
                    }
                    i += cliArg.ParameterNames.Length;
                }
                parsedArguments[cliArg.Name] = paramsList;
            }
        }
        #endregion
    }
}
