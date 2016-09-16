using System;
using System.Collections.Generic;
using System.Xml;
using SeeBee.FxUtils;

namespace SeeBee.PMLParser
{
    internal class PMLProcess
    {
        internal int ProcessId { get; set; }
        internal int ProcessIndex { get; set; }
        internal int ParentProcessId { get; set; }
        internal int ParentProcessIndex { get; set; }
        internal string AuthenticationId { get; set; }
        internal long CreateTime { get; set; }
        internal long FinishTime { get; set; }
        internal bool IsVirtualized { get; set; }
        internal bool Is64bit { get; set; }
        internal ProcessIntegrityLevel ProcessIntegrity { get; set; }
        internal int OwnerIndex { get; set; }
        internal string ProcessName { get; set; }
        internal int ImageIndex { get; set; }
        internal string CommandLine { get; set; }
        internal HashSet<int> ModuleList { get; set; }

        internal PMLProcess(XmlReader processListReader)
        {
            XmlDocument processXMLDoc = new XmlDocument();
            processXMLDoc.Load(processListReader);
            int processId = XMLUtils.ParseTagContentAsInt(processXMLDoc, "ProcessId"),
                parentProcessId = XMLUtils.ParseTagContentAsInt(processXMLDoc, "ParentProcessId"),
                processIndex = XMLUtils.ParseTagContentAsInt(processXMLDoc, "ProcessIndex"),
                parentProcessIndex = XMLUtils.ParseTagContentAsInt(processXMLDoc, "ParentProcessIndex");
            long createTime = XMLUtils.ParseTagContentAsLong(processXMLDoc, "CreateTime"),
                finishTime = XMLUtils.ParseTagContentAsLong(processXMLDoc, "FinishTime");
            bool isVirtualized = XMLUtils.ParseTagContentAsBoolean(processXMLDoc, "IsVirtualized"),
                is64Bit = XMLUtils.ParseTagContentAsBoolean(processXMLDoc, "Is64bit");
            string tempString = XMLUtils.GetInnerText(processXMLDoc, "Integrity");
            ProcessIntegrityLevel integrityLevel;
            if (tempString.Equals("System", StringComparison.CurrentCultureIgnoreCase))
            {
                integrityLevel = ProcessIntegrityLevel.System;
            }
            else if (tempString.Equals("High", StringComparison.CurrentCultureIgnoreCase))
            {
                integrityLevel = ProcessIntegrityLevel.High;
            }
            else if (tempString.Equals("Medium", StringComparison.CurrentCultureIgnoreCase))
            {
                integrityLevel = ProcessIntegrityLevel.Medium;
            }
            else
            {
                integrityLevel = ProcessIntegrityLevel.Low;
            }
            tempString = XMLUtils.GetInnerText(processXMLDoc, "Owner");
            int ownerIndex = PMLAnalyzer.LocateOwnerInList(tempString);
            if (-1 == ownerIndex)
            {
                ownerIndex = PMLAnalyzer.AddOwnerToList(tempString);
            }

            // Actual object creation i.e., assigning values to members
            ProcessId = processId;
            ParentProcessId = parentProcessId;
            ProcessIndex = processIndex;
            ParentProcessIndex = parentProcessIndex;
            AuthenticationId = XMLUtils.GetInnerText(processXMLDoc, "AuthenticationId");
            CreateTime = createTime;
            IsVirtualized = isVirtualized;
            Is64bit = is64Bit;
            ProcessIntegrity = integrityLevel;
            OwnerIndex = ownerIndex;
            ProcessName = XMLUtils.GetInnerText(processXMLDoc, "ProcessName");
            ImageIndex = PMLAnalyzer.LocateModuleInList(XMLUtils.GetInnerText(processXMLDoc, "ImagePath"));
            CommandLine = XMLUtils.GetInnerText(processXMLDoc, "CommandLine");
            ModuleList = PMLModule.LoadModules(processXMLDoc);
        }
    }

    internal enum ProcessIntegrityLevel
    {
        None,
        Low,
        Medium,
        High,
        System
    }
}
