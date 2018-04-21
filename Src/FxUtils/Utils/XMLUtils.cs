using System;
using System.Xml;

namespace SeeBee.FxUtils.Utils
{
    public static class XMLUtils
    {
        // XmlDocument version
        public static int ParseTagContentAsInt(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return GetInnerText(xmlDoc, tagName, nodeIndex, throwOnFailure).StringToInt(throwOnFailure);
        }

        public static long ParseTagContentAsLong(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return GetInnerText(xmlDoc, tagName, nodeIndex, throwOnFailure).StringToLong(throwOnFailure);
        }

        public static bool ParseTagContentAsBoolean(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return GetInnerText(xmlDoc, tagName, nodeIndex, throwOnFailure).StringToBoolean();
        }

        public static DateTime ParseTagContentAsFileTime(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return DateTime.FromFileTime(GetInnerText(xmlDoc, tagName, nodeIndex, throwOnFailure).StringToLong(throwOnFailure));
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

        public static string GetOuterXML(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            XmlNodeList nodeList = xmlDoc.GetElementsByTagName(tagName);
            if (nodeList.Count > nodeIndex && nodeList[nodeIndex] != null)
            {
                return nodeList[nodeIndex].OuterXml;
            }
            else if (throwOnFailure)
            {
                throw new IndexOutOfRangeException(
                    string.Format("Index {0} is out of range for NodeList of size {1} when getting OuterXML of tag {2}.",
                    nodeIndex,
                    nodeList.Count,
                    tagName));
            }
            return null;
        }

        // XmlElement version
        public static int ParseTagContentAsInt(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return GetInnerText(xmlElement, tagName, nodeIndex, throwOnFailure).StringToInt(throwOnFailure);
        }

        public static long ParseTagContentAsLong(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return GetInnerText(xmlElement, tagName, nodeIndex, throwOnFailure).StringToLong(throwOnFailure);
        }

        public static bool ParseTagContentAsBoolean(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return (GetInnerText(xmlElement, tagName, nodeIndex, throwOnFailure)).StringToBoolean();
        }

        public static DateTime ParseTagContentAsFileTime(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            return DateTime.FromFileTime(GetInnerText(xmlElement, tagName, nodeIndex, throwOnFailure).StringToLong(throwOnFailure));
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

        public static string GetOuterXML(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            XmlNodeList nodeList = xmlElement.GetElementsByTagName(tagName);
            if (nodeList.Count > nodeIndex && nodeList[nodeIndex] != null)
            {
                return nodeList[nodeIndex].OuterXml;
            }
            else if (throwOnFailure)
            {
                throw new IndexOutOfRangeException(
                    string.Format("Index {0} is out of range for NodeList of size {1} when getting OuterXML of tag {2}.",
                    nodeIndex,
                    nodeList.Count,
                    tagName));
            }
            return null;
        }
    }
}
