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
            XmlDocument processListDoc = new XmlDocument();
            processListDoc.Load(processListReader);
            int processId = XMLUtils.ParseTagContentAsInt(processListDoc, "ProcessId"),
                parentProcessId = XMLUtils.ParseTagContentAsInt(processListDoc, "ParentProcessId"),
                processIndex = XMLUtils.ParseTagContentAsInt(processListDoc, "ProcessIndex"),
                parentProcessIndex = XMLUtils.ParseTagContentAsInt(processListDoc, "ParentProcessIndex");
            long createTime = XMLUtils.ParseTagContentAsLong(processListDoc, "CreateTime"),
                finishTime = XMLUtils.ParseTagContentAsLong(processListDoc, "FinishTime");
            bool isVirtualized = XMLUtils.ParseTagContentAsBoolean(processListDoc, "IsVirtualized"),
                is64Bit = XMLUtils.ParseTagContentAsBoolean(processListDoc, "Is64bit");
            string tempString = XMLUtils.GetInnerText(processListDoc, "Integrity");
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
            tempString = XMLUtils.GetInnerText(processListDoc, "Owner");
            int ownerIndex = PMLAnalyzer.LocateOwnerInList(tempString);
            if (-1 == ownerIndex)
            {
                ownerIndex = PMLAnalyzer.AddOwnerToList(tempString);
            }

            // Handling ModuleList
            HashSet<int> processModuleList = new HashSet<int>();
            var modules = processListDoc.SelectNodes("/process/modulelist/module");
            foreach (XmlElement module in modules)
            {
                string path = module.GetElementsByTagName("Path")[0].InnerText;
                int moduleIndex = PMLAnalyzer.LocateModuleInList(path);
                if (-1 == moduleIndex)
                {
                    long timeStamp = XMLUtils.ParseTagContentAsLong(module, "Timestamp"),
                        size = XMLUtils.ParseTagContentAsLong(module, "Size");
                    long baseAddress = StringUtils.HexStringToLong(XMLUtils.GetInnerText(module, "BaseAddress"));
                    string version = XMLUtils.GetInnerText(module, "Version"),
                        company = XMLUtils.GetInnerText(module, "Company"),
                        description = XMLUtils.GetInnerText(module, "Description");
                    moduleIndex = PMLAnalyzer.AddModuleToList(new PMLModule(timeStamp, baseAddress, size, path, version, company, description));
                }
                if (-1 != moduleIndex)
                {
                    processModuleList.Add(moduleIndex);
                }
            }

            // Actual object creation i.e., assigning values to members
            ProcessId = processId;
            ParentProcessId = parentProcessId;
            ProcessIndex = processIndex;
            ParentProcessIndex = parentProcessIndex;
            AuthenticationId = XMLUtils.GetInnerText(processListDoc, "AuthenticationId");
            CreateTime = createTime;
            IsVirtualized = isVirtualized;
            Is64bit = is64Bit;
            ProcessIntegrity = integrityLevel;
            OwnerIndex = ownerIndex;
            ProcessName = XMLUtils.GetInnerText(processListDoc, "ProcessName");
            ImageIndex = PMLAnalyzer.LocateModuleInList(XMLUtils.GetInnerText(processListDoc, "ImagePath"));
            CommandLine = XMLUtils.GetInnerText(processListDoc, "CommandLine");
            ModuleList = processModuleList;
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
