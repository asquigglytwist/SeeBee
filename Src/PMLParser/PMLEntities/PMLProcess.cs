﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.ManagedLists;

namespace SeeBee.PMLParser.PMLEntities
{
    /// <summary>
    /// Represents a PML Process
    /// </summary>
    internal class PMLProcess : IPMLEntity
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
            string tempString = XMLUtils.GetInnerText(processXMLDoc, ProcMonXMLTagNames.Process_Owner);

            // Actual object creation i.e., assigning values to members
            ProcessId = XMLUtils.ParseTagContentAsInt(processXMLDoc, ProcMonXMLTagNames.Process_ProcessId);
            ParentProcessId = XMLUtils.ParseTagContentAsInt(processXMLDoc, ProcMonXMLTagNames.Process_ParentProcessId);
            ProcessIndex = XMLUtils.ParseTagContentAsInt(processXMLDoc, ProcMonXMLTagNames.Process_ProcessIndex);
            ParentProcessIndex = XMLUtils.ParseTagContentAsInt(processXMLDoc, ProcMonXMLTagNames.Process_ParentProcessIndex);
            AuthenticationId = XMLUtils.GetInnerText(processXMLDoc, ProcMonXMLTagNames.Process_AuthenticationId);
            CreateTime = XMLUtils.ParseTagContentAsFileTime(processXMLDoc, ProcMonXMLTagNames.Process_CreateTime);
            FinishTime = XMLUtils.ParseTagContentAsFileTime(processXMLDoc, ProcMonXMLTagNames.Process_FinishTime);
            IsVirtualized = XMLUtils.ParseTagContentAsBoolean(processXMLDoc, ProcMonXMLTagNames.Process_IsVirtualized);
            Is64bit = XMLUtils.ParseTagContentAsBoolean(processXMLDoc, ProcMonXMLTagNames.Process_Is64bit);
            ProcessIntegrity = ProcessIntegrityLevelExtensions.ToProcessIntegrityLevel(XMLUtils.GetInnerText(processXMLDoc, ProcMonXMLTagNames.Process_Integrity));
            OwnerIndex = OwnerList.AddOwnerToList(tempString);
            ProcessNameIndex = ProcessNameList.AddProcessNameToList(XMLUtils.GetInnerText(processXMLDoc, ProcMonXMLTagNames.Process_ProcessName));
            CommandLine = (XMLUtils.GetInnerText(processXMLDoc, ProcMonXMLTagNames.Process_CommandLine)).HTMLUnEscape().Trim();
            LoadedModuleList = PMLModule.LoadModules(processXMLDoc);
            var image = XMLUtils.GetInnerText(processXMLDoc, ProcMonXMLTagNames.Process_ImagePath);
            ImageIndex = ModuleList.LocateInOrAddToModuleList(image);

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
            summary =
#if DEBUG
                "[PMLProcess]:\n" +
#endif
                buffer.ToString();
        }
        #endregion

        public override string ToString()
        {
            return summary;
        }
    }
}
