using System;
using System.Collections.Generic;
using System.Xml;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.ManagedLists;

namespace SeeBee.PMLParser.PMLEntities
{
    /// <summary>
    /// Represents a PML Module
    /// </summary>
    internal class PMLModule
    {
        #region Members
        internal protected const string UnknownValue = "Unknown";
        private int pathIndex;
        private string summary;
        internal protected static PMLModule System = new PMLModule(DateTime.MinValue, 0, 0, "System", UnknownValue, UnknownValue, UnknownValue);
        #endregion

        #region Static Methods
        internal static HashSet<int> LoadModules(XmlDocument processXMLDoc)
        {
            HashSet<int> processModuleList = new HashSet<int>();
            var modules = processXMLDoc.SelectNodes(ProcMonXMLTagNames.Module_XPathInXML);
            foreach (XmlElement module in modules)
            {
                string path = module.GetElementsByTagName(ProcMonXMLTagNames.Module_Path)[0].InnerText;
                int moduleIndex = ModuleList.LocateModuleInList(path);
                if (-1 == moduleIndex)
                {
                    var tempModule = new PMLModule(path, module);
                    moduleIndex = ModuleList.AddModuleToList(tempModule);
                }
                if (-1 != moduleIndex)
                {
                    processModuleList.Add(moduleIndex);
                }
            }
            return processModuleList;
        }
        #endregion

        #region Constructor(s)
        private PMLModule(DateTime timeStamp, long baseAddress, long size, string path, string version, string company, string description)
        {
            this.TimeStamp = timeStamp;
            this.BaseAddress = baseAddress;
            this.Size = size;
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("A module's path cannot be null or empty.");
            }
            this.pathIndex = FilePathList.AddFilePathToList(path);
            if (string.IsNullOrWhiteSpace(version))
            {
                this.Version = PMLModule.UnknownValue;
            }
            else
            {
                this.Version = version;
            }
            if (string.IsNullOrWhiteSpace(company))
            {
                this.Company = PMLModule.UnknownValue;
            }
            else
            {
                this.Company = company;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                this.Description = PMLModule.UnknownValue;
            }
            else
            {
                this.Description = description;
            }
            this.summary = string.Format("Module - [{0}] [Version = {1};  Size {2}], located at \"{3}\", from [{4}], was loaded at [{5}] into address 0x{6}.",
                this.Description, this.Version, NumberUtils.FormatNumberAsFileSize(this.Size), this.Path,
                this.Company, this.TimeStamp, NumberUtils.LongToHexString(this.BaseAddress));
        }

        internal PMLModule(string path, XmlElement module) :
            this(XMLUtils.ParseTagContentAsFileTime(module, ProcMonXMLTagNames.Module_Timestamp),
            StringUtils.HexStringToLong(XMLUtils.GetInnerText(module, ProcMonXMLTagNames.Module_BaseAddress)),
            XMLUtils.ParseTagContentAsLong(module, ProcMonXMLTagNames.Module_Size),
            path,
            XMLUtils.GetInnerText(module, ProcMonXMLTagNames.Module_Version),
            XMLUtils.GetInnerText(module, ProcMonXMLTagNames.Module_Company),
            XMLUtils.GetInnerText(module, ProcMonXMLTagNames.Module_Description))
        {
        }
        #endregion

        #region Properties
        internal int ModuleIndex { get; private set; }
        internal DateTime TimeStamp { get; private set; }
        internal long BaseAddress { get; private set; }
        internal long Size { get; private set; }
        internal string Path
        {
            get
            {
                return FilePathList.GetFilePath(pathIndex);
            }
        }
        internal string Version { get; private set; }
        internal string Company { get; private set; }
        internal string Description { get; private set; }
        #endregion

        #region System.Object
        public override int GetHashCode()
        {
            return this.Path.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as PMLModule;
            if ((this.Size == otherObj.Size) && this.Path.Equals(otherObj.Path, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return this.summary;
        }
        #endregion
    }
}
