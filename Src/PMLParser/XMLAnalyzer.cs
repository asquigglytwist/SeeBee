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
                                System.IO.StringWriter stringWriter = new System.IO.StringWriter();
                                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                                processListDoc.WriteTo(xmlTextWriter);
                                System.IO.File.AppendAllText(@"C:\T\SeeBee\Test.log", stringWriter.ToString());
                                System.IO.File.AppendAllText(@"C:\T\SeeBee\Test.log", "--------------\n--------------\n--------------\n--------------\n--------------\n--------------\n");
                                Console.WriteLine(stringWriter.ToString());
                                Console.WriteLine();
                                Console.WriteLine();
                                Console.WriteLine();
                                Console.WriteLine();
#endif
                            }
                        }
    //                    int processId, parentProcessId;
    //                    int.TryParse(source.GetAttribute("ProcessId"), out processId);
    //                    int.TryParse(source.GetAttribute("ParentProcessId"), out parentProcessId);
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
