using System;

namespace SeeBee.PMLParser.PMLEntities
{
    #region ProcessIntegrityLevel
    /// <summary>
    /// Enumeration of Process Integrity Levels
    /// </summary>
    internal enum ProcessIntegrityLevel
    {
        None,
        Low,
        Medium,
        High,
        System
    }
    #endregion

    #region ProcessIntegrityLevelExtensions
    // [BIB]:  https://msdn.microsoft.com/en-us/library/bb383974.aspx
    internal static class ProcessIntegrityLevelExtensions
    {
        internal static string System = "System";
        internal static string High = "High";
        internal static string Medium = "Medium";
        internal static string Low = "Low";

        public static ProcessIntegrityLevel ToProcessIntegrityLevel(this string tempString)
        {
            ProcessIntegrityLevel integrityLevel = ProcessIntegrityLevel.None;
            if (tempString.Equals(ProcessIntegrityLevelExtensions.System, StringComparison.CurrentCultureIgnoreCase))
            {
                integrityLevel = ProcessIntegrityLevel.System;
            }
            else if (tempString.Equals(ProcessIntegrityLevelExtensions.High, StringComparison.CurrentCultureIgnoreCase))
            {
                integrityLevel = ProcessIntegrityLevel.High;
            }
            else if (tempString.Equals(ProcessIntegrityLevelExtensions.Medium, StringComparison.CurrentCultureIgnoreCase))
            {
                integrityLevel = ProcessIntegrityLevel.Medium;
            }
            else if (tempString.Equals(ProcessIntegrityLevelExtensions.Low, StringComparison.CurrentCultureIgnoreCase))
            {
                integrityLevel = ProcessIntegrityLevel.Low;
            }
            return integrityLevel;
        }
    }
    #endregion
}
