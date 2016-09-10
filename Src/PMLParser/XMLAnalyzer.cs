using System;
using System.Collections.Generic;
using System.Xml;

namespace SeeBee.PMLParser
{
    public class XMLAnalyzer
    {
        public /*IEnumerable<ProcessEntry>*/ void ListProcesses(string xmlFilePath)
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
#if DEBUG
                                System.IO.File.AppendAllText(@"C:\T\SeeBee\Test.log", processListDoc.InnerXml);
                                System.IO.File.AppendAllText(@"C:\T\SeeBee\Test.log", "--------------\n--------------\n--------------\n--------------\n--------------\n");
                                Console.WriteLine();
                                Console.WriteLine();
                                Console.WriteLine();
                                Console.WriteLine();
                                Console.WriteLine(processListDoc.GetElementsByTagName("ProcessId")[0].InnerText);
                                Console.WriteLine();
                                Console.WriteLine();
                                Console.WriteLine();
                                Console.WriteLine();
#endif
                                int processId, parentProcessId;
                                int.TryParse(processListDoc.GetElementsByTagName("ProcessId")[0].InnerText, out processId);
                                int.TryParse(processListDoc.GetElementsByTagName("ParentProcessId")[0].InnerText, out parentProcessId);
                                //yield return new ProcessEntry
                                //{
                                //    ProcessId = processId,
                                //    ParentProcessId = parentProcessId
                                //};
                            }
                        }
                    }
                }
            }
        }
    }
}
