using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SeeBee.PMLParser
{
    internal static class TagNames
    {
        // Tag Names used for PML's Processes
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

        // Tag Names used for PML's Events
        internal const string Event_Event = "event";
        internal const string Event_ProcessIndex = "ProcessIndex";
        internal const string Event_TimeOfDay = "Time_of_Day";
        internal const string Event_Process_Name="Process_Name";
        internal const string Event_PID="PID";
        internal const string Event_TID="TID";
        internal const string Event_Integrity="Integrity";
        internal const string Event_Sequence="Sequence";
        internal const string Event_Virtualized="Virtualized";
        internal const string Event_Operation="Operation";
        internal const string Event_Path="Path";
        internal const string Event_Result="Result";
        internal const string Event_Detail = "Detail";
    }
}
