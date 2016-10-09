using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using SeeBee.PMLParser.PMLEntities;

namespace SeeBee.PMLParser.Analysis
{
    internal static class ConvertedXMLProcessor
    {
        #region Private Methods
        private static IEnumerable<PMLProcess> LoadProcesses(string xmlFilePath)
        {
            using (XmlReader source = XmlReader.Create(xmlFilePath))
            {
                while (source.Read())
                {
                    if (source.NodeType == XmlNodeType.Element &&
                        source.Name.Equals(ProcMonXMLTagNames.Process_Process, StringComparison.CurrentCultureIgnoreCase))
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

        private static IEnumerable<PMLEvent> LoadEvents(string xmlFilePath)
        {
            using (XmlReader source = XmlReader.Create(xmlFilePath))
            {
                while (source.Read())
                {
                    if (source.NodeType == XmlNodeType.Element &&
                        source.Name.Equals(ProcMonXMLTagNames.Event_Event, StringComparison.CurrentCultureIgnoreCase))
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
        #endregion

        #region Public Methods
        // [BIB]:  http://stackoverflow.com/questions/1516876/when-to-use-ref-vs-out
        internal static PMLFile PopulateProcessesAndEvents(string xmlFilePath)
        {
            var procs = from p in ConvertedXMLProcessor.LoadProcesses(xmlFilePath) where (!string.IsNullOrWhiteSpace(p.ProcessNameIndex.ToString())) select p;
            var evts = from e in ConvertedXMLProcessor.LoadEvents(xmlFilePath) where (!string.IsNullOrWhiteSpace(e.TimeOfDay.ToString())) select e;
            PMLProcess[] processes = procs.ToArray();
            PMLEvent[] events = evts.ToArray();
            var pmlFile = new PMLFile(processes, events);
#if DEBUG
            System.Console.WriteLine("# of Processes that match the criteria {0}.", processes.Length);
            System.Console.WriteLine("# of Events that match the criteria {0}.", events.Length);
#endif
            return pmlFile;
        }
        #endregion
    }
}
