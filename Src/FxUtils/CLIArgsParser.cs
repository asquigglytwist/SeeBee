using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.FxUtils
{
    public class CLIArgsParser
    {
        #region Constructor
        public CLIArgsParser(string argsDelimiter = "/", char optionDelimiter = ':', bool treatQuotedStringsAsOne = true)
        {
            if (string.IsNullOrWhiteSpace(argsDelimiter))
            {
                throw new ArgumentException("ArgsDelimiter can't be null or empty.");
            }
            ArgsDelimiter = argsDelimiter;
            OptionDelimiter = optionDelimiter;
            TreatQuotedStringsAsOne = treatQuotedStringsAsOne;
        }
        #endregion

        #region Properties
        public string ArgsDelimiter { get; protected set; }
        public char OptionDelimiter { get; protected set; }
        public bool TreatQuotedStringsAsOne { get; protected set; }
        #endregion

        #region Methods
        public void Parse(string args, ref Dictionary<string, string> cliArgs)
        {
        }
        #endregion
    }
}
