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
                        source.Name == "process")
                    {
                        using (XmlReader processListReader = source.ReadSubtree())
                        {
                            if (null != processListReader)
                            {
                                yield return new PMLProcess(processListReader);
                            }
                        }
                    }
                }
            }
        }
    }
}
