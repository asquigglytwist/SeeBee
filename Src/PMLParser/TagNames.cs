using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.PMLParser
{
    internal static class TagNames
    {
        internal const string Process_Process = "process";
        internal const string Process_ProcessId = "ProcessId";
        internal const string Process_ParentProcessId = "ParentProcessId";
        internal const string Process_ProcessIndex = "ProcessIndex";
        internal const string Process_ParentProcessIndex = "ParentProcessIndex";
        internal const string Process_CreateTime = "CreateTime";
        internal const string Process_FinishTime = "FinishTime";
        internal const string Process_IsVirtualized = "IsVirtualized";
        internal const string Process_Is64bit = "Is64bit";
        internal const string Process_Integrity = "Integrity";
        internal const string Process_Owner = "Owner";
        internal const string Process_AuthenticationId = "AuthenticationId";
        internal const string Process_ProcessName = "ProcessName";
        internal const string Process_CommandLine = "CommandLine";
        internal const string Process_ImagePath = "ImagePath";
    }
}
