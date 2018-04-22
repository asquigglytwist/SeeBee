using System;
using System.Xml;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.Analysis;
using SeeBee.PMLParser.ManagedLists;

namespace SeeBee.PMLParser.PMLEntities
{
    /// <summary>
    /// Represents a PML Event
    /// </summary>
    public class PMLEvent : IPMLEntity
    {
        #region Members
        int pathIndex;
        #endregion

        internal PMLStackFrame[] CallStack { get; private set; }
        #region Properties
        internal int ProcessIndex { get; private set; }
        internal DateTime TimeOfDay { get; private set; }
        internal int ProcessNameIndex { get; private set; }
        internal int PID { get; private set; }
        internal int TID { get; private set; }
        internal ProcessIntegrityLevel Integrity { get; private set; }
        internal string Sequence { get; private set; }
        internal bool Virtualized { get; private set; }
        internal string Operation { get; private set; }
        internal string Path
        {
            get
            {
                return FilePathList.GetFilePath(pathIndex);
            }
        }
        internal string Result { get; private set; }
        internal string Detail { get; private set; }
        #endregion

        #region Constructor
        internal PMLEvent(XmlReader eventListReader)
        {
            XmlDocument eventXMLDoc = new XmlDocument();
            eventXMLDoc.Load(eventListReader);
            ProcessIndex = XMLUtils.ParseTagContentAsInt(eventXMLDoc, ProcMonXMLTagNames.Event_ProcessIndex);
            TimeOfDay = XMLUtils.ParseTagContentAsFileTime(eventXMLDoc, ProcMonXMLTagNames.Event_TimeOfDay);
            var procName = XMLUtils.GetInnerText(eventXMLDoc, ProcMonXMLTagNames.Event_Process_Name);
            ProcessNameIndex = ProcessNameList.AddProcessNameToList(procName);
            PID = XMLUtils.ParseTagContentAsInt(eventXMLDoc, ProcMonXMLTagNames.Event_PID);
            TID = XMLUtils.ParseTagContentAsInt(eventXMLDoc, ProcMonXMLTagNames.Event_TID);
            var proc = ConvertedXMLProcessor.FindProcessByPID(PID);
            var temp = XMLUtils.GetInnerText(eventXMLDoc, ProcMonXMLTagNames.Event_Integrity);
            if (string.IsNullOrEmpty(temp))
            {
                Integrity = proc.ProcessIntegrity;
            }
            else
            {
                Integrity = temp.ToProcessIntegrityLevel();
            }
            Sequence = XMLUtils.GetInnerText(eventXMLDoc, ProcMonXMLTagNames.Event_Sequence);
            temp = XMLUtils.GetInnerText(eventXMLDoc, ProcMonXMLTagNames.Event_Virtualized);
            if (string.IsNullOrEmpty(temp))
            {
                Virtualized = proc.IsVirtualized;
            }
            else
            {
                Virtualized = temp.StringToBoolean();
            }
            //Virtualized = XMLUtils.ParseTagContentAsBoolean(eventXMLDoc, ProcMonXMLTagNames.Event_Virtualized);
            Operation = XMLUtils.GetInnerText(eventXMLDoc, ProcMonXMLTagNames.Event_Operation);
            pathIndex = FilePathList.AddFilePathToList(XMLUtils.GetInnerText(eventXMLDoc, ProcMonXMLTagNames.Event_Path));
            Result = XMLUtils.GetInnerText(eventXMLDoc, ProcMonXMLTagNames.Event_Result);
            Detail = XMLUtils.GetInnerText(eventXMLDoc, ProcMonXMLTagNames.Event_Detail);
            CallStack = PMLStackFrame.LoadStackFrames(eventXMLDoc);
#if DEBUG
            Console.WriteLine("Stack:\n-------------------------------------------------------------");
            foreach (var stackFrame in CallStack)
            {
                Console.WriteLine(stackFrame);
            }
            Console.WriteLine("-------------------------------------------------------------\n");
#endif
        }
        #endregion

        #region System.Object
        public override int GetHashCode()
        {
            return TimeOfDay.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj);
        }

        public bool Equals(PMLEvent otherEvent)
        {
            return (TimeOfDay.Equals(otherEvent.TimeOfDay)
                && (ProcessIndex == otherEvent.ProcessIndex)
                && (TID == otherEvent.TID)
                && (Operation.Equals(otherEvent.Operation, StringComparison.CurrentCultureIgnoreCase))
                && (Detail.Equals(otherEvent.Detail, StringComparison.CurrentCultureIgnoreCase)));
        }

        public override string ToString()
        {
            return
#if DEBUG
                "[PMLEvent]:\n" +
#endif
                string.Format("Thread {0} of Process {1} [PID: {2}] performed {3} at {4} on {5} and result was {6}.{7}Details:{8}{7}",
                TID,
                ProcessNameList.GetProcessName(ProcessNameIndex),
                PID,
                Operation,
                TimeOfDay,
                Path,
                Result,
                Environment.NewLine,
                Detail);
        }
        #endregion
    }
}
