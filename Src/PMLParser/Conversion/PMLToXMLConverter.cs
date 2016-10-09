using System;
using System.Text;
using SeeBee.FxUtils;
using SeeBee.FxUtils.AuthentiCode;
using SeeBee.FxUtils.Utils;

namespace SeeBee.PMLParser.Conversion
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
            if (!SignerInfo.IsSignedBy(procMonExeLocation, "CN=Microsoft Code Signing PCA, O=Microsoft Corporation, L=Redmond, S=Washington, C=US", true))
            {
                throw new ArgumentException(string.Format("ProcessMonitor (ProcMon) executable at location {0} does not meet expected digital signing requirements.", procMonExeLocation));
            }
            this.shouldRetryOnceOnFailure = shouldRetryOnceOnFailure;
            PMLFile = pmlFile;
            XMLFile = FSUtils.CreateOuputFileNameFromInput(pmlFile, ".xml");

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
