using System;
using System.Text;
using SeeBee.FxUtils;
using SeeBee.FxUtils.AuthentiCode;

namespace SeeBee.PMLParser
{
    internal class PMLToXMLConverter
    {
        #region Members
        bool shouldRetryOnceOnFailure;
        ProcessEx procMon;
        #endregion

        #region Constructor
        internal PMLToXMLConverter(string procMonExeLocation, string pmlFile, bool shouldRetryOnceOnFailure = false)
        {
            if (!AuthentiCodeTools.IsTrusted(procMonExeLocation))
            {
                throw new ArgumentException(string.Format("ProcessMonitor (ProcMon) executable at location {0} is not a trusted binary.", procMonExeLocation));
            }
            this.shouldRetryOnceOnFailure = shouldRetryOnceOnFailure;
            PMLFile = pmlFile;
            // [BIB]:  http://stackoverflow.com/questions/5608980/how-to-ensure-a-timestamp-is-always-unique
            XMLFile = string.Format("{0}_{1}.xml", FSUtils.GetFileName(pmlFile, true), DateTime.UtcNow.Ticks);
            XMLFile = FSUtils.PathCombine(FSUtils.WritableLocationForTempFile, XMLFile);

            StringBuilder sbArgs = new StringBuilder("/quiet /minimized /saveas2 ");
            sbArgs.Append(XMLFile);
            sbArgs.Append(" /openlog ");
            sbArgs.Append(PMLFile);
            // [BIB]:  http://forum.sysinternals.com/forum_posts.asp?TID=13843&PID=74632&title=producing-csv-from-process-monitor-via-script#74632
            procMon = new ProcessEx(procMonExeLocation, sbArgs.ToString());
        }
        #endregion

        internal bool Convert()
        {
            procMon.Start();
            if (0 == procMon.WaitForExitCode())
            {
                return true;
            }
            if (shouldRetryOnceOnFailure)
            {
                if (0 == procMon.WaitForExitCode())
                {
                    return true;
                }
            }
            return false;
        }

        #region Properties
        internal string PMLFile
        {
            get;
            private set;
        }

        // [BIB]:  http://stackoverflow.com/a/2719761
        internal string XMLFile
        {
            get;
            private set;
        }
        #endregion
    }
}
