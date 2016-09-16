using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace SeeBee.FxUtils
{
    public static class XMLUtils
    {
        // XmlDocument version
        public static int ParseTagContentAsInt(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return StringUtils.StringToInt(GetInnerText(xmlDoc, tagName, nodeIndex), throwOnFailure);
        }

        public static long ParseTagContentAsLong(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return StringUtils.StringToLong(GetInnerText(xmlDoc, tagName, nodeIndex), throwOnFailure);
        }

        public static bool ParseTagContentAsBoolean(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return StringUtils.StringToBoolean(GetInnerText(xmlDoc, tagName, nodeIndex), throwOnFailure);
        }

        public static DateTime ParseTagContentAsFileTime(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return DateTime.FromFileTime(StringUtils.StringToLong(GetInnerText(xmlDoc, tagName, nodeIndex), throwOnFailure));
        }

        public static string GetInnerText(XmlDocument xmlDoc, string tagName, int nodeIndex = 0)
        {
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName(tagName);
            if (nodeList.Count > nodeIndex && nodeList[nodeIndex] != null)
            {
                return nodeList[nodeIndex].InnerText;
            }
            return null;
        }

        // XmlElement version
        public static int ParseTagContentAsInt(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return StringUtils.StringToInt(GetInnerText(xmlElement, tagName, nodeIndex), throwOnFailure);
        }

        public static long ParseTagContentAsLong(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return StringUtils.StringToLong(GetInnerText(xmlElement, tagName, nodeIndex), throwOnFailure);
        }

        public static bool ParseTagContentAsBoolean(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return StringUtils.StringToBoolean(GetInnerText(xmlElement, tagName, nodeIndex), throwOnFailure);
        }

        public static DateTime ParseTagContentAsFileTime(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return DateTime.FromFileTime(StringUtils.StringToLong(GetInnerText(xmlElement, tagName, nodeIndex), throwOnFailure));
        }

        public static string GetInnerText(XmlElement xmlElement, string tagName, int nodeIndex = 0)
        {
            XmlNodeList nodeList = xmlElement.GetElementsByTagName(tagName);
            if (nodeList.Count > nodeIndex && nodeList[nodeIndex] != null)
            {
                return nodeList[nodeIndex].InnerText;
            }
            return null;
        }
    }
}
