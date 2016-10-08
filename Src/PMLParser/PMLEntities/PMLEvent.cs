using System;
using System.Xml;
using SeeBee.FxUtils.Utils;

namespace SeeBee.PMLParser.PMLEntities
{
    public class PMLEvent
    {
        #region Members
        int pathIndex;
        PMLStackFrame[] callStack;
        #endregion

        #region Properties
        internal int ProcessIndex { get; private set; }
        internal DateTime TimeOfDay { get; private set; }
        internal string ProcessName { get; private set; }
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
                return PMLAnalyzer.GetFilePath(pathIndex);
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
            this.ProcessIndex = XMLUtils.ParseTagContentAsInt(eventXMLDoc, TagNames.Event_ProcessIndex);
            this.TimeOfDay = XMLUtils.ParseTagContentAsFileTime(eventXMLDoc, TagNames.Event_TimeOfDay);
            this.ProcessName = XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Process_Name);
            this.PID = XMLUtils.ParseTagContentAsInt(eventXMLDoc, TagNames.Event_PID);
            this.TID = XMLUtils.ParseTagContentAsInt(eventXMLDoc, TagNames.Event_TID);
            this.Integrity = XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Integrity).ToProcessIntegrityLevel();
            this.Sequence = XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Sequence);
            this.Virtualized = XMLUtils.ParseTagContentAsBoolean(eventXMLDoc, TagNames.Event_Virtualized);
            this.Operation = XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Operation);
            this.pathIndex = PMLAnalyzer.AddFilePathToList(XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Path));
            this.Result = XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Result);
            this.Detail = XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Detail);
            this.callStack = PMLStackFrame.LoadStackFrames(eventXMLDoc);
#if DEBUG
            Console.WriteLine("Stack:\n-------------------------------------------------------------");
            foreach (var stackFrame in this.callStack)
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
            return this.TimeOfDay.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj);
        }

        public bool Equals(PMLEvent otherEvent)
        {
            return (this.TimeOfDay.Equals(otherEvent.TimeOfDay)
                && (this.ProcessIndex == otherEvent.ProcessIndex)
                && (this.TID == otherEvent.TID)
                && (this.Operation.Equals(otherEvent.Operation, StringComparison.CurrentCultureIgnoreCase))
                && (this.Detail.Equals(otherEvent.Detail, StringComparison.CurrentCultureIgnoreCase)));
        }

        public override string ToString()
        {
            return string.Format("Thread {0} of Process {1} [PID: {2}] performed {3} at {4} on {5} and result was {6}.{7}Details:{8}{7}",
                this.TID,
                this.ProcessName,
                this.PID,
                this.Operation,
                this.TimeOfDay,
                this.Path,
                this.Result,
                Environment.NewLine,
                this.Detail);
        }
        #endregion
    }
}
