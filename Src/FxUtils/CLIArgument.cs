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
            string sampleUsage = null, bool isCaseSensitive = false, CLIArgument nestedArgument = null)
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

            SampleUsage = sampleUsage;
            IsCaseSensitive = isCaseSensitive;
            NestedArgument = nestedArgument;
            if (this.Equals(NestedArgument))
            {
                throw new ArgumentException(string.Format("Argument cannot match NestedArgument for command {0}.", Name));
            }
        }
        #endregion

        #region Properties
        public string ShortVersion { get; private set; }
        public string Name { get; private set; }
        public bool IsRequired { get; private set; }
        public string[] ParameterNames { get; private set; }
        public string Explanation { get; private set; }

        public string SampleUsage { get; private set; }
        public bool IsCaseSensitive { get; private set; }
        public CLIArgument NestedArgument { get; private set; }
        #endregion

        #region System.Object
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
            if (this.ParameterNames != null)
            {
                for (int i = 0; i < this.ParameterNames.Length; i++)
                {
                    buffer.AppendFormat(" {0}", this.ParameterNames[i]);
                }
            }
            if (this.NestedArgument != null)
            {
                buffer.AppendFormat(" {0}{1}", this.NestedArgument.ToShortString());
            }
            buffer.Append((this.IsRequired ? "" : "]"));
            return buffer.ToString();
        }
        #endregion

        public string ToShortString()
        {
            return string.Format("{0}{1}{2}", (this.IsRequired ? "" : "["), this.Name, (this.IsRequired ? "" : "]"));
        }

        public string ToLongString()
        {
            StringBuilder buffer = new StringBuilder(string.Format("{0}{1}{2}",
                (this.IsRequired ? "" : "["), CLIArgsParser.DefaultOptionDelimiter, this.Name));
            if (!string.IsNullOrWhiteSpace(this.ShortVersion))
            {
                buffer.AppendFormat(" [or {0}{1}]", CLIArgsParser.DefaultOptionDelimiter, this.ShortVersion);
            }
            if (this.ParameterNames != null)
            {
                for (int i = 0; i < this.ParameterNames.Length; i++)
                {
                    buffer.AppendFormat(" {0}", this.ParameterNames[i]);
                }
            }
            if (this.NestedArgument != null)
            {
                buffer.AppendFormat(" {0}{1}", this.NestedArgument.ToString());
            }
            buffer.Append((this.IsRequired ? "" : "]"));
            if (Explanation != null)
            {
                buffer.AppendFormat("{0}A brief explanation:{0}\t{1}{0}", Environment.NewLine, Explanation);
            }
            if (SampleUsage != null)
            {
                buffer.AppendFormat("An example of the command's usage:{0}\t{1}{0}", Environment.NewLine, SampleUsage);
            }
            return buffer.ToString();
        }
    }
}
