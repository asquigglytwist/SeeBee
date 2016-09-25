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
                        source.Name.Equals(TagNames.Process_Process, StringComparison.CurrentCultureIgnoreCase))
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

        internal IEnumerable<PMLEvent> LoadEvents(string xmlFilePath)
        {
            using (XmlReader source = XmlReader.Create(xmlFilePath))
            {
                while (source.Read())
                {
                    if (source.NodeType == XmlNodeType.Element &&
                        source.Name.Equals(TagNames.Event_Event, StringComparison.CurrentCultureIgnoreCase))
                    {
                        using (XmlReader eventListReader = source.ReadSubtree())
                        {
                            if (null != eventListReader)
                            {
                                var processedEvent = new PMLEvent(eventListReader);
#if DEBUG
                                Console.WriteLine(processedEvent);
#endif
                                yield return processedEvent;
                            }
                        }
                    }
                }
                source.Close();
            }
        }
    }
}
