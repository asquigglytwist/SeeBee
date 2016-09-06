using System;
using System.Text;
using SeeBee.FxUtils;

namespace SeeBee.PMLParser
{
    public class PMLToXMLConverter
    {
        bool shouldRetryOnceOnFailure;
        ProcessEx procMon;

        #region Constructor
        public PMLToXMLConverter(string procMonExeLocation, string pmlFile, bool shouldRetryOnceOnFailure = false)
        {
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

        public bool Convert()
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
        public string PMLFile
        {
            get;
            private set;
        }

        // [BIB]:  http://stackoverflow.com/a/2719761
        public string XMLFile
        {
            get;
            private set;
        }
        #endregion
    }
}
