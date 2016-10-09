using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.ManagedLists;

namespace SeeBee.PMLParser.PMLEntities
{
    internal class PMLProcess
    {
        private string summary;

        #region Properties
        internal int ProcessId { get; set; }
        internal int ProcessIndex { get; set; }
        internal int ParentProcessId { get; set; }
        internal int ParentProcessIndex { get; set; }
        internal string AuthenticationId { get; set; }
        internal DateTime CreateTime { get; set; }
        internal DateTime FinishTime { get; set; }
        internal bool IsVirtualized { get; set; }
        internal bool Is64bit { get; set; }
        internal ProcessIntegrityLevel ProcessIntegrity { get; set; }
        internal int OwnerIndex { get; set; }
        internal int ProcessNameIndex { get; set; }
        internal int ImageIndex { get; set; }
        internal string CommandLine { get; set; }
        internal HashSet<int> LoadedModuleList { get; set; }
        #endregion

        #region Constructor
        internal PMLProcess(XmlReader processListReader)
        {
            XmlDocument processXMLDoc = new XmlDocument();
            processXMLDoc.Load(processListReader);
            string tempString = XMLUtils.GetInnerText(processXMLDoc, TagNames.Process_Owner);

            // Actual object creation i.e., assigning values to members
            this.ProcessId = XMLUtils.ParseTagContentAsInt(processXMLDoc, TagNames.Process_ProcessId);
            this.ParentProcessId = XMLUtils.ParseTagContentAsInt(processXMLDoc, TagNames.Process_ParentProcessId);
            this.ProcessIndex = XMLUtils.ParseTagContentAsInt(processXMLDoc, TagNames.Process_ProcessIndex);
            this.ParentProcessIndex = XMLUtils.ParseTagContentAsInt(processXMLDoc, TagNames.Process_ParentProcessIndex);
            this.AuthenticationId = XMLUtils.GetInnerText(processXMLDoc, TagNames.Process_AuthenticationId);
            this.CreateTime = XMLUtils.ParseTagContentAsFileTime(processXMLDoc, TagNames.Process_CreateTime);
            this.FinishTime = XMLUtils.ParseTagContentAsFileTime(processXMLDoc, TagNames.Process_FinishTime);
            this.IsVirtualized = XMLUtils.ParseTagContentAsBoolean(processXMLDoc, TagNames.Process_IsVirtualized);
            this.Is64bit = XMLUtils.ParseTagContentAsBoolean(processXMLDoc, TagNames.Process_Is64bit);
            this.ProcessIntegrity = ProcessIntegrityLevelExtensions.ToProcessIntegrityLevel(XMLUtils.GetInnerText(processXMLDoc, TagNames.Process_Integrity));
            this.OwnerIndex = OwnerList.AddOwnerToList(tempString);
            this.ProcessNameIndex = ProcessNameList.AddProcessNameToList(XMLUtils.GetInnerText(processXMLDoc, TagNames.Process_ProcessName));
            this.CommandLine = StringUtils.HTMLUnEscape(XMLUtils.GetInnerText(processXMLDoc, TagNames.Process_CommandLine)).Trim();
            this.LoadedModuleList = PMLModule.LoadModules(processXMLDoc);
            this.ImageIndex = ModuleList.LocateModuleInList(XMLUtils.GetInnerText(processXMLDoc, TagNames.Process_ImagePath));

            StringBuilder buffer = new StringBuilder(string.Format(
                "{0}{1} Process - {2} [{3}] with ID = {4} was created at {5} with {6} integrity, which loaded {7} modules, as a child of {8} by {9}",
                (IsVirtualized ? "Virtualized " : ""),
                (Is64bit ? "64-Bit" : "32-Bit"),
                ProcessNameList.GetProcessName(ProcessNameIndex),
                ModuleList.GetModuleDescription(ImageIndex),
                ProcessId,
                CreateTime,
                ProcessIntegrity,
                LoadedModuleList.Count,
                ParentProcessId,
                OwnerList.GetOwnerName(OwnerIndex)
                ));
            if (!string.IsNullOrWhiteSpace(CommandLine))
            {
                buffer.AppendFormat(", using the command line {0}", CommandLine);
            }
            buffer.Append(" ");
            if (FinishTime <= CreateTime)
            {
                buffer.Append("and is running.");
            }
            else
            {
                buffer.AppendFormat("and ended at {0}.", FinishTime);
            }
            this.summary = buffer.ToString();
        }
        #endregion

        public override string ToString()
        {
            return summary;
        }
    }
}
