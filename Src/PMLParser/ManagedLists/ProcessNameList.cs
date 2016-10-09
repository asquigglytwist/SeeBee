using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeeBee.FxUtils;

namespace SeeBee.PMLParser.ManagedLists
{
    class ProcessNameList
    {
        private static IndexedStringCollection knownProcessNames = new IndexedStringCollection();

        #region Static Constructor
        static ProcessNameList()
        {
            ProcessNameList.AddProcessNameToList("System");
        }
        #endregion

        #region Methods
        internal static int LocateProcessNameInList(string processName)
        {
            return knownProcessNames.LocateString(processName);
        }

        internal static int AddProcessNameToList(string processName)
        {
            return knownProcessNames.Add(processName);
        }

        internal static string GetProcessName(int index)
        {
            return knownProcessNames.StringAt(index);
        }
        #endregion
    }
}
