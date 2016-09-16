using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SeeBee.FxUtils;

namespace SeeBee.PMLParser
{
    public class PMLModule
    {
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
                    moduleIndex = PMLAnalyzer.AddModuleToList(tempModule);
                }
                if (-1 != moduleIndex)
                {
                    processModuleList.Add(moduleIndex);
                }
            }
            return processModuleList;
        }

        private PMLModule(long timeStamp, long baseAddress, long size, string path, string version, string company, string description)
        {
#if DEBUG
            Console.WriteLine("{0} was loaded at {1}.", path, DateTime.FromFileTime(timeStamp));
#endif
            TimeStamp = DateTime.FromFileTime(timeStamp);
            BaseAddress = baseAddress;
            Size = size;
            Path = path;
            Version = version;
            Company = company;
            Description = description;
        }

        internal PMLModule(string path, XmlElement module) :
            this(XMLUtils.ParseTagContentAsLong(module, "Timestamp"),
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
    }
}
