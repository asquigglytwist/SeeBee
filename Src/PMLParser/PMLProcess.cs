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
        internal string ProcessName { get; set; }
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
            string tempString = processListDoc.GetElementsByTagName("Integrity")[0].InnerText;
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

            HashSet<int> processModuleList = new HashSet<int>();
            var modules = processListDoc.SelectNodes("/process/modulelist/module");
#if DEBUG
            Console.WriteLine("# of module nodes is {0}.", modules.Count);
#endif
            foreach (XmlElement module in modules)
            {
//#if DEBUG
//                Console.WriteLine(module.GetElementsByTagName("Path")[0].InnerText);
//#endif
                string path = module.GetElementsByTagName("Path")[0].InnerText;
                int index = PMLAnalyzer.LocateModuleInList(path);
                if (-1 == index)
                {
                    long timeStamp = XMLUtils.ParseTagContentAsLong(module, "Timestamp"),
                        size = XMLUtils.ParseTagContentAsLong(module, "Size");
                    long baseAddress = StringUtils.HexStringToLong(XMLUtils.GetInnerText(module, "BaseAddress"));
                    string version = XMLUtils.GetInnerText(module, "Version"),
                        company = XMLUtils.GetInnerText(module, "Company"),
                        description = XMLUtils.GetInnerText(module, "Description");
                    index = PMLAnalyzer.AddModuleToList(new PMLModule(timeStamp, baseAddress, size, path, version, company, description));
                }
                if (-1 != index)
                {
                    processModuleList.Add(index);
                }
            }
            // Actual object creation i.e., assigning values to members
            ProcessId = processId;
            ParentProcessId = parentProcessId;
            ProcessIndex = processIndex;
            ParentProcessIndex = parentProcessIndex;
            AuthenticationId = processListDoc.GetElementsByTagName("AuthenticationId")[0].InnerText;
            CreateTime = createTime;
            IsVirtualized = isVirtualized;
            Is64bit = is64Bit;
            ProcessIntegrity = integrityLevel;
            ProcessName = processListDoc.GetElementsByTagName("ProcessName")[0].InnerText;
            CommandLine = processListDoc.GetElementsByTagName("CommandLine")[0].InnerText;
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
