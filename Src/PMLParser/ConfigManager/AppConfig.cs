using System;
using System.Collections.Generic;
using System.Xml;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.ManagedLists;

namespace SeeBee.PMLParser.ConfigManager
{
    internal class AppConfig
    {
        public List<int> IgnoredProcessNames
        { get; private set; }

        internal AppConfig(string configFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(configFile);
            string[] csvProcessNames = XMLUtils.GetInnerText(doc, AppConfigPropertyNames.IgnoredProcessNames).Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            IgnoredProcessNames = new List<int>(csvProcessNames.Length);
            foreach (string s in csvProcessNames)
            {
                IgnoredProcessNames.Add(ProcessNameList.AddProcessNameToList(s));
            }
        }
    }
}
