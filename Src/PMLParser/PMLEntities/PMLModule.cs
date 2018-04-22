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
        protected internal const string UnknownValue = "Unknown";
        private int pathIndex;
        private string summary;
        internal protected static PMLModule System = new PMLModule(DateTime.MinValue, 0, 0, "System", UnknownValue, UnknownValue, UnknownValue);
        #endregion

        #region Static Methods
        internal static HashSet<int> LoadModules(XmlDocument processXMLDoc)
        {
            ModuleList.AddModuleToList(System);
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

        internal static PMLModule CreateUnknownModule(string modulePath)
        {
            return new PMLModule(DateTime.MinValue, 0, 0, modulePath, UnknownValue, UnknownValue, UnknownValue);
        }
        #endregion

        #region Constructor(s)
        private PMLModule(DateTime timeStamp, long baseAddress, long size, string path, string version, string company, string description)
        {
            TimeStamp = timeStamp;
            BaseAddress = baseAddress;
            Size = size;
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("A module's path cannot be null or empty.");
            }
            pathIndex = FilePathList.AddFilePathToList(path);
            if (string.IsNullOrWhiteSpace(version))
            {
                Version = PMLModule.UnknownValue;
            }
            else
            {
                Version = version;
            }
            if (string.IsNullOrWhiteSpace(company))
            {
                Company = PMLModule.UnknownValue;
            }
            else
            {
                Company = company;
            }
            if (string.IsNullOrWhiteSpace(description))
            {
                Description = PMLModule.UnknownValue;
            }
            else
            {
                Description = description;
            }
            summary =
#if DEBUG
                "[PMLModule]:\n" +
#endif
                string.Format("Module - [{0}] [Version = {1};  Size {2}], located at \"{3}\", from [{4}], was loaded at [{5}] into address 0x{6}.",
                Description, Version, NumberUtils.FormatNumberAsFileSize(Size), Path,
                Company, TimeStamp, NumberUtils.LongToHexString(BaseAddress));
        }

        internal PMLModule(string path, XmlElement module) :
            this(XMLUtils.ParseTagContentAsFileTime(module, ProcMonXMLTagNames.Module_Timestamp),
            (XMLUtils.GetInnerText(module, ProcMonXMLTagNames.Module_BaseAddress)).HexStringToLong(),
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
            return Path.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as PMLModule;
            if ((Size == otherObj.Size) && Path.Equals(otherObj.Path, StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return summary;
        }
        #endregion
    }
}
