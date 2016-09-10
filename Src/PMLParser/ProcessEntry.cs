using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.PMLParser
{
    public class ProcessEntry
    {
        public int ProcessId { get; set; }
        public int ParentProcessId { get; set; }
        public string AuthenticationId { get; set; }
        public long CreateTime { get; set; }
        public long FinishTime { get; set; }
        public bool IsVirtualized { get; set; }
        public bool Is64bit { get; set; }
        public ProcessIntegrityLevel ProcessIntegriy { get; set; }
    }

    public enum ProcessIntegrityLevel
    {
        Low,
        Medium,
        High
    }
}
