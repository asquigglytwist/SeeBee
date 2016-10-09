namespace SeeBee.PMLParser.PMLEntities
{
    internal static class TagNames
    {
        #region Tags for Processes
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
        #endregion

        #region Tags for Modules
        internal const string Module_Timestamp = "Timestamp";
        internal const string Module_XPathInXML = "/process/modulelist/module";
        internal const string Module_Path = "Path";
        internal const string Module_BaseAddress = "BaseAddress";
        internal const string Module_Size = "Size";
        internal const string Module_Version = "Version";
        internal const string Module_Company = "Company";
        internal const string Module_Description = "Description";
        #endregion

        #region Tags for Events
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
        #endregion

        #region Tags for StackFrames
        internal const string StackFrame_Address = "address";
        internal const string StackFrame_Path = "path";
        internal const string StackFrame_Location = "location";
        internal const string StackFrame_XPathInXML = "/event/stack/frame";
        #endregion
    }
}
