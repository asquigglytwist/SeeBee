using System;
using System.Collections.Generic;
using System.Xml;
using SeeBee.FxUtils;

namespace SeeBee.PMLParser
{
    public class PMLModule
    {
        internal protected const string UnknownValue = "Unknown";

        internal static HashSet<int> LoadModules(XmlDocument processXMLDoc)
        {
            HashSet<int> processModuleList = new HashSet<int>();
            var modules = processXMLDoc.SelectNodes("/process/modulelist/module");
            foreach (XmlElement module in modules)
            {
                string path = module.GetElementsByTagName("Path")[0].InnerText;
                int moduleIndex = PMLAnalyzer.LocateModuleInList(path);
                if (-1 == moduleIndex)
                {
                    var tempModule = new PMLModule(path, module);
#if DEBUG
                    Console.WriteLine(tempModule);
#endif
                    moduleIndex = PMLAnalyzer.AddModuleToList(tempModule);
                }
                if (-1 != moduleIndex)
                {
                    processModuleList.Add(moduleIndex);
                }
            }
            return processModuleList;
        }

        private PMLModule(DateTime timeStamp, long baseAddress, long size, string path, string version, string company, string description)
        {
#if DEBUG
            Console.WriteLine("{0} was loaded at {1}.", path, timeStamp);
#endif
            TimeStamp = timeStamp;
            BaseAddress = baseAddress;
            Size = size;
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException("A module's path cannot be null or empty.");
            }
            Path = path;
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
        }

        internal PMLModule(string path, XmlElement module) :
            this(XMLUtils.ParseTagContentAsFileTime(module, "Timestamp"),
            StringUtils.HexStringToLong(XMLUtils.GetInnerText(module, "BaseAddress")),
            XMLUtils.ParseTagContentAsLong(module, "Size"),
            path,
            XMLUtils.GetInnerText(module, "Version"),
            XMLUtils.GetInnerText(module, "Company"),
            XMLUtils.GetInnerText(module, "Description"))
        {
        }

        internal int ModuleIndex { get; private set; }
        internal DateTime TimeStamp { get; private set; }
        internal long BaseAddress { get; private set; }
        internal long Size { get; private set; }
        internal string Path { get; private set; }
        internal string Version { get; private set; }
        internal string Company { get; private set; }
        internal string Description { get; private set; }

        public override int GetHashCode()
        {
            return this.Path.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var otherObj = obj as PMLModule;
            if ((this.Size == otherObj.Size) && this.Path.Equals(otherObj.Path))
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return String.Format("Module - {0} [version = {1}] of size {2} located at {3}, from {4}, was loaded at {5} into address 0x{6}.",
                Description, Version, Size, Path, Company, TimeStamp, NumberUtils.LongToHexString(BaseAddress));
        }
    }
}
