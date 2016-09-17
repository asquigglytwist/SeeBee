using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.FxUtils
{
    public class CLIArgument
    {
        #region Constructor
        public CLIArgument(string shortVersion, string name, bool isRequired, string[] parameterNames, string explanation,
            bool isCaseSensitive = false, string sampleUsage = null, CLIArgument nestedArgument = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Argument's name cannot be null or empty.");
            }
            ShortVersion = shortVersion;
            Name = name;
            IsRequired = isRequired;
            ParameterNames = parameterNames;
            Explanation = explanation;

            IsCaseSensitive = isCaseSensitive;
            SampleUsage = sampleUsage;
            NestedArgument = nestedArgument;
        }
        #endregion

        #region Properties
        public string ShortVersion { get; private set; }
        public string Name { get; private set; }
        public bool IsRequired { get; private set; }
        public string[] ParameterNames { get; private set; }
        public string Explanation { get; private set; }

        public bool IsCaseSensitive { get; private set; }
        public string SampleUsage { get; private set; }
        public CLIArgument NestedArgument { get; private set; }
        #endregion
    }
}
