using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.PMLParser
{
    internal enum ProcessIntegrityLevel
    {
        None,
        Low,
        Medium,
        High,
        System
    }

    internal static class ProcessIntegrityLevelStrings
    {
        internal static string System = "System";
        internal static string High = "High";
        internal static string Medium = "Medium";
        internal static string Low = "Low";

        internal static ProcessIntegrityLevel ParseString(string tempString)
        {
            ProcessIntegrityLevel integrityLevel = ProcessIntegrityLevel.None;
            if (tempString.Equals(ProcessIntegrityLevelStrings.System, StringComparison.CurrentCultureIgnoreCase))
            {
                integrityLevel = ProcessIntegrityLevel.System;
            }
            else if (tempString.Equals(ProcessIntegrityLevelStrings.High, StringComparison.CurrentCultureIgnoreCase))
            {
                integrityLevel = ProcessIntegrityLevel.High;
            }
            else if (tempString.Equals(ProcessIntegrityLevelStrings.Medium, StringComparison.CurrentCultureIgnoreCase))
            {
                integrityLevel = ProcessIntegrityLevel.Medium;
            }
            else if (tempString.Equals(ProcessIntegrityLevelStrings.Low, StringComparison.CurrentCultureIgnoreCase))
            {
                integrityLevel = ProcessIntegrityLevel.Low;
            }
            return integrityLevel;
        }
    }
}
