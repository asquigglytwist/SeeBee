using System;
using System.Collections.Generic;
using System.Xml;

namespace SeeBee.PMLParser
{
    internal class ConvertedXMLProcessor
    {
        internal IEnumerable<PMLProcess> LoadProcesses(string xmlFilePath)
        {
            using (XmlReader source = XmlReader.Create(xmlFilePath))
            {
                while (source.Read())
                {
                    if (source.NodeType == XmlNodeType.Element &&
                        source.Name.Equals(TagNames.Process_Process))
                    {
                        using (XmlReader processListReader = source.ReadSubtree())
                        {
                            if (null != processListReader)
                            {
                                var process = new PMLProcess(processListReader);
#if DEBUG
                                Console.WriteLine(process);
#endif
                                yield return process;
                            }
                        }
                    }
                }
                source.Close();
            }
        }
    }
}
