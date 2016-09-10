using System;
using System.Collections.Generic;
using System.Xml;

namespace SeeBee.PMLParser
{
    internal class XMLProcessor
    {
        internal IEnumerable<ProcessEntry> ListProcesses(string xmlFilePath)
        {
            using (XmlReader source = XmlReader.Create(xmlFilePath))
            {
                while (source.Read())
                {
                    if (source.NodeType == XmlNodeType.Element &&
                        source.Name == "process")
                    {
                        using (XmlReader processListReader = source.ReadSubtree())
                        {
                            if (null != processListReader)
                            {
                                XmlDocument processListDoc = new XmlDocument();
                                processListDoc.Load(processListReader);
                                int processId, parentProcessId;
                                int.TryParse(processListDoc.GetElementsByTagName("ProcessId")[0].InnerText, out processId);
                                int.TryParse(processListDoc.GetElementsByTagName("ParentProcessId")[0].InnerText, out parentProcessId);
                                long createTime, finishTime;
                                long.TryParse(processListDoc.GetElementsByTagName("CreateTime")[0].InnerText, out createTime);
                                long.TryParse(processListDoc.GetElementsByTagName("FinishTime")[0].InnerText, out finishTime);
                                bool isVirtualized, is64Bit;
                                bool.TryParse(processListDoc.GetElementsByTagName("IsVirtualized")[0].InnerText, out isVirtualized);
                                bool.TryParse(processListDoc.GetElementsByTagName("Is64bit")[0].InnerText, out is64Bit);
                                string integrityLevelString = processListDoc.GetElementsByTagName("Integrity")[0].InnerText;
                                ProcessIntegrityLevel integrityLevel;
                                if (integrityLevelString.Equals("High", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    integrityLevel = ProcessIntegrityLevel.High;
                                }
                                else if (integrityLevelString.Equals("Medium", StringComparison.CurrentCultureIgnoreCase))
                                {
                                    integrityLevel = ProcessIntegrityLevel.Medium;
                                }
                                else
                                {
                                    integrityLevel = ProcessIntegrityLevel.Low;
                                }
                                yield return new ProcessEntry
                                {
                                    ProcessId = processId,
                                    ParentProcessId = parentProcessId,
                                    AuthenticationId = processListDoc.GetElementsByTagName("AuthenticationId")[0].InnerText,
                                    CreateTime = createTime,
                                    IsVirtualized = isVirtualized,
                                    Is64bit = is64Bit,
                                    ProcessIntegrity = integrityLevel,
                                    ProcessName = processListDoc.GetElementsByTagName("ProcessName")[0].InnerText,
                                    CommandLine = processListDoc.GetElementsByTagName("CommandLine")[0].InnerText
                                };
                            }
                        }
                    }
                }
            }
        }
    }
}
