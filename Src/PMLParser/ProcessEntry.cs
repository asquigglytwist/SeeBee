using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.PMLParser
{
    public class ProcessEntry
    {
        public int ProcessId
        {
            get;
            set;
        }
        public int ParentProcessId
        {
            get;
            set;
        }
    }
}
