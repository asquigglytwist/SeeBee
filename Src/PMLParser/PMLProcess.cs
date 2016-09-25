﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SeeBee.FxUtils;

namespace SeeBee.PMLParser
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
        internal string ProcessName { get; set; }
        internal int ImageIndex { get; set; }
        internal string CommandLine { get; set; }
        internal HashSet<int> ModuleList { get; set; }
        #endregion

        #region Constructor
        internal PMLProcess(XmlReader processListReader)
        {
            XmlDocument processXMLDoc = new XmlDocument();
            processXMLDoc.Load(processListReader);
            int processId = XMLUtils.ParseTagContentAsInt(processXMLDoc, TagNames.Process_ProcessId),
                parentProcessId = XMLUtils.ParseTagContentAsInt(processXMLDoc, TagNames.Process_ParentProcessId),
                processIndex = XMLUtils.ParseTagContentAsInt(processXMLDoc, TagNames.Process_ProcessIndex),
                parentProcessIndex = XMLUtils.ParseTagContentAsInt(processXMLDoc, TagNames.Process_ParentProcessIndex);
            DateTime createTime = XMLUtils.ParseTagContentAsFileTime(processXMLDoc, TagNames.Process_CreateTime),
                finishTime = XMLUtils.ParseTagContentAsFileTime(processXMLDoc, TagNames.Process_FinishTime);
            bool isVirtualized = XMLUtils.ParseTagContentAsBoolean(processXMLDoc, TagNames.Process_IsVirtualized),
                is64Bit = XMLUtils.ParseTagContentAsBoolean(processXMLDoc, TagNames.Process_Is64bit);
            string tempString = XMLUtils.GetInnerText(processXMLDoc, TagNames.Process_Owner);
            int ownerIndex = PMLAnalyzer.LocateOwnerInList(tempString);
            if (-1 == ownerIndex)
            {
                ownerIndex = PMLAnalyzer.AddOwnerToList(tempString);
            }

            // Actual object creation i.e., assigning values to members
            ProcessId = processId;
            ParentProcessId = parentProcessId;
            ProcessIndex = processIndex;
            ParentProcessIndex = parentProcessIndex;
            AuthenticationId = XMLUtils.GetInnerText(processXMLDoc, TagNames.Process_AuthenticationId);
            CreateTime = createTime;
            FinishTime = finishTime;
            IsVirtualized = isVirtualized;
            Is64bit = is64Bit;
            ProcessIntegrity = ProcessIntegrityLevelStrings.ParseString(XMLUtils.GetInnerText(processXMLDoc, TagNames.Process_Integrity));
            OwnerIndex = ownerIndex;
            ProcessName = XMLUtils.GetInnerText(processXMLDoc, TagNames.Process_ProcessName);
            CommandLine = StringUtils.HTMLUnEscape(XMLUtils.GetInnerText(processXMLDoc, TagNames.Process_CommandLine)).Trim();
            ModuleList = PMLModule.LoadModules(processXMLDoc);
            ImageIndex = PMLAnalyzer.LocateModuleInList(XMLUtils.GetInnerText(processXMLDoc, TagNames.Process_ImagePath));

            StringBuilder buffer = new StringBuilder(string.Format(
                "{0}{1} Process - {2} [{3}] with ID = {4} was created at {5} with {6} integrity, which loaded {7} modules, as a child of {8} by {9}",
                (IsVirtualized ? "Virtualized " : ""),
                (Is64bit ? "64-Bit" : "32-Bit"),
                ProcessName,
                PMLAnalyzer.GetModuleDescription(ImageIndex),
                ProcessId,
                CreateTime,
                ProcessIntegrity,
                ModuleList.Count,
                ParentProcessId,
                PMLAnalyzer.GetOwnerName(OwnerIndex)
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
            summary = buffer.ToString();
        }
        #endregion

        public override string ToString()
        {
            return summary;
        }
    }
}
