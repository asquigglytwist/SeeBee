using System;
using System.Diagnostics;

namespace SeeBee.FxUtils
{
    public class ProcessEx
    {
        public ProcessEx(string fileName, string args, ProcessWindowStyle windowStyle = ProcessWindowStyle.Hidden)
        {
            Process = new Process();
            Process.StartInfo.FileName = fileName;
            Process.StartInfo.Arguments = args;
            Process.StartInfo.WindowStyle = windowStyle;
        }

        public bool Start()
        {
            try
            {
                return Process.Start();
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        public int WaitForExitCode(int waitForMilliSec = 30000)
        {
            if (Process.WaitForExit(waitForMilliSec))
            {
                if (Process.HasExited)
                {
                    return Process.ExitCode;
                }
            }
            return 0;
        }

        public Process Process
        {
            get;
            private set;
        }
    }
}
