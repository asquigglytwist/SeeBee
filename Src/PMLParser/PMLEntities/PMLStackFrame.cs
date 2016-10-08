using System;
using System.Collections.Generic;
using System.Xml;
using SeeBee.FxUtils;

namespace SeeBee.PMLParser.PMLEntities
{
    internal class PMLStackFrame
    {
        int pathIndex;
        internal const string UnknownStringValue = "<unknown>";

        #region Static Methods
        internal static PMLStackFrame[] LoadStackFrames(XmlDocument eventXMLDoc)
        {
            var stackFrames = eventXMLDoc.SelectNodes(TagNames.StackFrame_XPathInXML);
            PMLStackFrame[] eventStackFrames = new PMLStackFrame[stackFrames.Count];
            int i = 0;
            foreach (XmlElement frame in stackFrames)
            {
                eventStackFrames[i++] = new PMLStackFrame(frame);
            }
            return eventStackFrames;
        }
        #endregion

        #region Constructor
        private PMLStackFrame(long address, string path, string location)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                //throw new ArgumentException("A StackFrame cannot have null or empty path.");
                path = UnknownStringValue;
            }
            if (string.IsNullOrWhiteSpace(location))
            {
                //throw new ArgumentException("A StackFrame cannot have null or empty location.");
                location = UnknownStringValue;
            }
            this.Address = address;
            this.pathIndex = PMLAnalyzer.AddFilePathToList(path);
            this.Location = location;
        }

        internal PMLStackFrame(XmlElement frame) :
            this(StringUtils.HexStringToLong(XMLUtils.GetInnerText(frame, TagNames.StackFrame_Address)),
            XMLUtils.GetInnerText(frame, TagNames.StackFrame_Path),
            XMLUtils.GetInnerText(frame, TagNames.StackFrame_Location))
        {
        }
        #endregion

        #region Properties
        internal long Address { get; private set; }
        internal string Path
        {
            get
            {
                return PMLAnalyzer.GetFilePath(pathIndex);
            }
        }
        internal string Location { get; private set; }
        #endregion

        #region System.Object
        public override int GetHashCode()
        {
            return (this.Address+this.Location).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj);
        }

        public bool Equals(PMLStackFrame otherStackFrame)
        {
            return (this.Address==otherStackFrame.Address)
                && (this.pathIndex==otherStackFrame.pathIndex)
                && (this.Location.Equals(otherStackFrame.Location, StringComparison.CurrentCultureIgnoreCase));
        }

        public override string ToString()
        {
            return string.Format("{0} at {1}.", this.Path, this.Address);
        }
        #endregion
    }
}
