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
            string innerText = GetInnerText(xmlDoc, tagName, nodeIndex);
            return StringUtils.StringToInt(innerText, throwOnFailure);
        }

        public static long ParseTagContentAsLong(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            string innerText = GetInnerText(xmlDoc, tagName, nodeIndex);
            return StringUtils.StringToLong(innerText, throwOnFailure);
        }

        public static bool ParseTagContentAsBoolean(XmlDocument xmlDoc, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            string innerText = GetInnerText(xmlDoc, tagName, nodeIndex);
            return StringUtils.StringToBoolean(innerText, throwOnFailure);
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
            string innerText = GetInnerText(xmlElement, tagName, nodeIndex);
            return StringUtils.StringToInt(innerText, throwOnFailure);
        }

        public static long ParseTagContentAsLong(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            string innerText = GetInnerText(xmlElement, tagName, nodeIndex);
            return StringUtils.StringToLong(innerText, throwOnFailure);
        }

        public static bool ParseTagContentAsBoolean(XmlElement xmlElement, string tagName, int nodeIndex = 0, bool throwOnFailure = false)
        {
            string innerText = GetInnerText(xmlElement, tagName, nodeIndex);
            return StringUtils.StringToBoolean(innerText, throwOnFailure);
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
