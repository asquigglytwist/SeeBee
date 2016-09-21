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
        public void Parse(string[] args, ref CLIArgument[] cliArgs)
        {
            for (int i = 0; i < args.Length; i++)
            {
                CLIArgument cliArg = (CLIArgument)from arg in cliArgs
                                                  where (arg.Name.Equals(args[i]) || arg.ShortVersion.Equals(args[i]))
                                                  select arg;
                if (cliArg.ParameterNames != null)
                {
                    for (int j = 0; j < cliArg.ParameterNames.Length; j++)
                    {
                    }
                }
            }
        }
        #endregion
    }
}
