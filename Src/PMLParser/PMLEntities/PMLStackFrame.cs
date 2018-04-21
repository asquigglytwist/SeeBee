using System;
using System.Xml;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.ManagedLists;

namespace SeeBee.PMLParser.PMLEntities
{
    /// <summary>
    /// Represents a PML Stack Frame
    /// </summary>
    internal class PMLStackFrame
    {
        int pathIndex;
        internal const string UnknownStringValue = "<unknown>";

        #region Static Methods
        internal static PMLStackFrame[] LoadStackFrames(XmlDocument eventXMLDoc)
        {
            var stackFrames = eventXMLDoc.SelectNodes(ProcMonXMLTagNames.StackFrame_XPathInXML);
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
            Address = address;
            pathIndex = FilePathList.AddFilePathToList(path);
            Location = location;
        }

        internal PMLStackFrame(XmlElement frame) :
            this(StringUtils.HexStringToLong(XMLUtils.GetInnerText(frame, ProcMonXMLTagNames.StackFrame_Address)),
            XMLUtils.GetInnerText(frame, ProcMonXMLTagNames.StackFrame_Path),
            XMLUtils.GetInnerText(frame, ProcMonXMLTagNames.StackFrame_Location))
        {
        }
        #endregion

        #region Properties
        internal long Address { get; private set; }
        internal string Path
        {
            get
            {
                return FilePathList.GetFilePath(pathIndex);
            }
        }
        internal string Location { get; private set; }
        #endregion

        #region System.Object
        public override int GetHashCode()
        {
            return (Address + Location).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return Equals(obj);
        }

        public bool Equals(PMLStackFrame otherStackFrame)
        {
            return (Address == otherStackFrame.Address)
                && (pathIndex == otherStackFrame.pathIndex)
                && (Location.Equals(otherStackFrame.Location, StringComparison.CurrentCultureIgnoreCase));
        }

        public override string ToString()
        {
            return
#if DEBUG
                "[PMLStackFrame]:\t" +
#endif
                string.Format("{0} at 0x{1:X16}.", Path, Address);
        }
        #endregion
    }
}
