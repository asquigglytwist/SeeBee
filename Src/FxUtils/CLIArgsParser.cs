using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.FxUtils
{
    public class CLIArgsParser
    {
        #region Constructor
        public CLIArgsParser(string optionDelimiter = "/")
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
        public void Parse(string[] args, ref Dictionary<string, string> cliArgs)
        {
        }
        #endregion
    }
}
