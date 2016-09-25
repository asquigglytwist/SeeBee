using System;
using System.Xml;
using SeeBee.FxUtils;

namespace SeeBee.PMLParser
{
    public class PMLEvent
    {
        #region Properties
        internal int ProcessIndex { get; private set;}
        internal DateTime TimeOfDay { get; private set;}
        internal string Process_Name { get; private set;}
        internal int PID { get; private set;}
        internal int TID { get; private set;}
        internal ProcessIntegrityLevel Integrity { get; private set;}
        internal string Sequence { get; private set;}
        internal bool Virtualized { get; private set;}
        internal string Operation { get; private set;}
        internal string Path { get; private set;}
        internal string Result { get; private set;}
        internal string Detail { get; private set; }
        #endregion

        #region Constructor
        internal PMLEvent(XmlReader eventListReader)
        {
            XmlDocument eventXMLDoc = new XmlDocument();
            eventXMLDoc.Load(eventListReader);
            ProcessIndex = XMLUtils.ParseTagContentAsInt(eventXMLDoc, TagNames.Event_ProcessIndex);
            TimeOfDay = XMLUtils.ParseTagContentAsFileTime(eventXMLDoc, TagNames.Event_TimeOfDay);
            Process_Name = XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Process_Name);
            PID = XMLUtils.ParseTagContentAsInt(eventXMLDoc, TagNames.Event_PID);
            TID = XMLUtils.ParseTagContentAsInt(eventXMLDoc, TagNames.Event_TID);
            Integrity = ProcessIntegrityLevelStrings.ParseString(XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Integrity));
            Sequence = XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Sequence);
            Virtualized = XMLUtils.ParseTagContentAsBoolean(eventXMLDoc, TagNames.Event_Virtualized);
            Operation = XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Operation);
            Path = XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Path);
            Result = XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Result);
            Detail = XMLUtils.GetInnerText(eventXMLDoc, TagNames.Event_Detail);
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
                this.Process_Name,
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
