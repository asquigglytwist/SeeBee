using System;
using System.Xml;

namespace SeeBee.FxUtils
{
    public static class XMLUtils
    {
        // XmlDocument version
        public static int ParseTagContentAsInt(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return StringUtils.StringToInt(GetInnerText(xmlDoc, tagName, nodeIndex, throwOnFailure), throwOnFailure);
        }

        public static long ParseTagContentAsLong(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return StringUtils.StringToLong(GetInnerText(xmlDoc, tagName, nodeIndex, throwOnFailure), throwOnFailure);
        }

        public static bool ParseTagContentAsBoolean(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return StringUtils.StringToBoolean(GetInnerText(xmlDoc, tagName, nodeIndex, throwOnFailure));
        }

        public static DateTime ParseTagContentAsFileTime(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return DateTime.FromFileTime(StringUtils.StringToLong(GetInnerText(xmlDoc, tagName, nodeIndex, throwOnFailure), throwOnFailure));
        }

        public static string GetInnerText(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName(tagName);
            if (nodeList.Count > nodeIndex && nodeList[nodeIndex] != null)
            {
                return nodeList[nodeIndex].InnerText;
            }
            else if(throwOnFailure)
            {
                throw new IndexOutOfRangeException(
                    string.Format("Index {0} is out of range for NodeList of size {1} when getting InnerText of tag {2}.",
                    nodeIndex,
                    nodeList.Count,
                    tagName));
            }
            return null;
        }

        // XmlElement version
        public static int ParseTagContentAsInt(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return StringUtils.StringToInt(GetInnerText(xmlElement, tagName, nodeIndex, throwOnFailure), throwOnFailure);
        }

        public static long ParseTagContentAsLong(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return StringUtils.StringToLong(GetInnerText(xmlElement, tagName, nodeIndex, throwOnFailure), throwOnFailure);
        }

        public static bool ParseTagContentAsBoolean(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return StringUtils.StringToBoolean(GetInnerText(xmlElement, tagName, nodeIndex, throwOnFailure));
        }

        public static DateTime ParseTagContentAsFileTime(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return DateTime.FromFileTime(StringUtils.StringToLong(GetInnerText(xmlElement, tagName, nodeIndex, throwOnFailure), throwOnFailure));
        }

        public static string GetInnerText(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            XmlNodeList nodeList = xmlElement.GetElementsByTagName(tagName);
            if (nodeList.Count > nodeIndex && nodeList[nodeIndex] != null)
            {
                return nodeList[nodeIndex].InnerText;
            }
            else if (throwOnFailure)
            {
                throw new IndexOutOfRangeException(
                    string.Format("Index {0} is out of range for NodeList of size {1} when getting InnerText of tag {2}.",
                    nodeIndex,
                    nodeList.Count,
                    tagName));
            }
            return null;
        }
    }
}
