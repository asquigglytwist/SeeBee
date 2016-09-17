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

        public override int GetHashCode()
        {
            return this.Name.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Name.Equals(obj);
        }

        public override string ToString()
        {
            StringBuilder buffer = new StringBuilder(string.Format("{0}{1}{2}",
                (this.IsRequired ? "" : "["), CLIArgsParser.DefaultOptionDelimiter, this.Name));
            if (!string.IsNullOrWhiteSpace(this.ShortVersion))
            {
                buffer.AppendFormat(" [or {0}{1}]", CLIArgsParser.DefaultOptionDelimiter, this.ShortVersion);
            }
            for (int i = 0; i < this.ParameterNames.Length; i++)
            {
                buffer.AppendFormat(" {0}", this.ParameterNames[i]);
            }
            if (this.NestedArgument != null)
            {
                buffer.AppendFormat(" {0}{1}", this.NestedArgument.ToShortString());
            }
            buffer.Append((this.IsRequired ? "" : "]"));
            return this.ToString();
        }

        public string ToShortString()
        {
            return string.Format("{0}{1}{2}", (this.IsRequired ? "" : "["), this.Name, (this.IsRequired ? "" : "]"));
        }
    }
}
