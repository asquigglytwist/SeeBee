using System;
using System.Collections.Generic;
using System.Xml;

namespace SeeBee.PMLParser
{
    internal class XMLProcessor
    {
        internal IEnumerable<PMLProcess> LoadProcesses(string xmlFilePath)
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
                                int processId, parentProcessId, processIndex, parentProcessIndex;
                                int.TryParse(processListDoc.GetElementsByTagName("ProcessId")[0].InnerText, out processId);
                                int.TryParse(processListDoc.GetElementsByTagName("ParentProcessId")[0].InnerText, out parentProcessId);
                                int.TryParse(processListDoc.GetElementsByTagName("ProcessIndex")[0].InnerText, out processIndex);
                                int.TryParse(processListDoc.GetElementsByTagName("ParentProcessIndex")[0].InnerText, out parentProcessIndex);
                                long createTime, finishTime;
                                long.TryParse(processListDoc.GetElementsByTagName("CreateTime")[0].InnerText, out createTime);
                                long.TryParse(processListDoc.GetElementsByTagName("FinishTime")[0].InnerText, out finishTime);
                                bool isVirtualized, is64Bit;
                                string tempString = processListDoc.GetElementsByTagName("IsVirtualized")[0].InnerText;
                                if (tempString.Equals("0"))
                                {
                                    isVirtualized = false;
                                }
                                else
                                {
                                    isVirtualized = true;
                                }
                                tempString = processListDoc.GetElementsByTagName("Is64bit")[0].InnerText;
                                if (tempString.Equals("0"))
                                {
                                    is64Bit = false;
                                }
                                else
                                {
                                    is64Bit = true;
                                }
                                tempString = processListDoc.GetElementsByTagName("Integrity")[0].InnerText;
                                ProcessIntegrityLevel integrityLevel;
                                if (tempString.Equals("High", StringComparison.CurrentCultureIgnoreCase))
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
                                yield return new PMLProcess
                                {
                                    ProcessId = processId,
                                    ParentProcessId = parentProcessId,
                                    ProcessIndex = processIndex,
                                    ParentProcessIndex = parentProcessIndex,
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
