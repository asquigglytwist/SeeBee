using System;
using System.Collections.Generic;
using System.Xml;
using SeeBee.FxUtils.Utils;
using SeeBee.PMLParser.ManagedLists;

namespace SeeBee.PMLParser.ConfigManager
{
    internal class AppConfig
    {
        List<int> ignoredProcessNames;

        internal AppConfig(string configFile)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(configFile);
            string[] csvProcessNames = XMLUtils.GetInnerText(doc, AppConfigPropertyNames.IgnoredProcessNames).Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries);
            ignoredProcessNames = new List<int>(csvProcessNames.Length);
            foreach (string s in csvProcessNames)
            {
                ignoredProcessNames.Add(ProcessNameList.AddProcessNameToList(s));
            }
        }
    }
}
