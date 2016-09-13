using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.PMLParser
{
    internal class PMLProcess
    {
        public int ProcessId { get; set; }
        public int ProcessIndex { get; set; }
        public int ParentProcessId { get; set; }
        public int ParentProcessIndex { get; set; }
        public string AuthenticationId { get; set; }
        public long CreateTime { get; set; }
        public long FinishTime { get; set; }
        public bool IsVirtualized { get; set; }
        public bool Is64bit { get; set; }
        public ProcessIntegrityLevel ProcessIntegrity { get; set; }
        public string ProcessName { get; set; }
        public string CommandLine { get; set; }
    }

    internal enum ProcessIntegrityLevel
    {
        Low,
        Medium,
        High
    }
}
