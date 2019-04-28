using Microsoft.RuleEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ESBFrameWorkDemo.UtilityHelper
{
    public class XmlHelper
    {
        public string statusCode { get; set; }
        public string statusDesc { get; set; }
        public bool isRuleFired { get; set; }
        public bool haltRuleExection { get; set; }
        public string executionOutCome { get; set; }

        public XmlHelper()
        {
            statusCode = string.Empty;
            statusDesc = string.Empty;
            isRuleFired = false;
            haltRuleExection = false;
            executionOutCome = string.Empty;
        }

        public static string NodeCount(TypedXmlDocument document, string xpath)
        {
            int count = document.Document.SelectNodes(xpath, document.NamespaceManager).Count;
            return count.ToString();
        }

        public static void AddAttribute(TypedXmlDocument document, string xpath, string attributeName, object attributeValue)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            if (string.IsNullOrEmpty(xpath))
                throw new ArgumentNullException("xpath");
            if (string.IsNullOrEmpty(attributeName))
                throw new ArgumentNullException("attributeName");
            if (attributeValue == null)
                throw new ArgumentNullException("attributeValue");
            XmlElement xmlElement = LocateXPath(document, xpath) as XmlElement;
            if (xmlElement == null)
                return;
            xmlElement.SetAttribute(attributeName, attributeValue.ToString());
            document.MarkAsChanged();
        }

        public static void AddNode(TypedXmlDocument document, string xpath, string nodeName, string nodeNamespace)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            if (string.IsNullOrEmpty(xpath))
                throw new ArgumentNullException("xpath");
            if (string.IsNullOrEmpty(nodeName))
                throw new ArgumentNullException("nodeName");
            if (string.IsNullOrEmpty(nodeNamespace))
                throw new ArgumentNullException("nodeNamespace");
            XmlNode xmlNode = LocateXPath(document, xpath);
            if (xmlNode == null)
                return;
            XmlDocument xmlDocument = xmlNode.OwnerDocument;
            if (xmlDocument == null)
            {
                xmlDocument = document.Document as XmlDocument;
                if (xmlDocument == null)
                    return;
            }
            XmlElement element = xmlDocument.CreateElement(nodeName, nodeNamespace);
            xmlNode.InsertBefore((XmlNode)element, (XmlNode)null);
            document.MarkAsChanged();
        }

        public static void AddNode(TypedXmlDocument document, string xpath, string nodeName)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            if (string.IsNullOrEmpty(xpath))
                throw new ArgumentNullException("xpath");
            if (string.IsNullOrEmpty(nodeName))
                throw new ArgumentNullException("nodeName");
            XmlNode xmlNode = LocateXPath(document, xpath);
            if (xmlNode == null)
                return;
            XmlDocument xmlDocument = xmlNode.OwnerDocument;
            if (xmlDocument == null)
            {
                xmlDocument = document.Document as XmlDocument;
                if (xmlDocument == null)
                    return;
            }

            XmlElement element = xmlDocument.CreateElement(xmlDocument.DocumentElement.Prefix, nodeName, xmlDocument.DocumentElement.NamespaceURI);
            xmlNode.InsertBefore((XmlNode)element, (XmlNode)null);
            document.MarkAsChanged();
        }

        public static void AddNodeWithValue(TypedXmlDocument document, string xpath, string nodeName, string nodeNamespace, object nodeValue)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            if (string.IsNullOrEmpty(xpath))
                throw new ArgumentNullException("xpath");
            if (string.IsNullOrEmpty(nodeName))
                throw new ArgumentNullException("nodeName");
            if (string.IsNullOrEmpty(nodeNamespace))
                throw new ArgumentNullException("nodeNamespace");
            if (nodeValue == null)
                throw new ArgumentNullException("nodeValue");
            XmlNode xmlNode = LocateXPath(document, xpath);
            if (xmlNode == null)
                return;
            XmlDocument xmlDocument = xmlNode.OwnerDocument;
            if (xmlDocument == null)
            {
                xmlDocument = document.Document as XmlDocument;
                if (xmlDocument == null)
                    return;
            }
            XmlElement element = xmlDocument.CreateElement(xmlDocument.DocumentElement.Prefix, nodeName, xmlDocument.DocumentElement.NamespaceURI);
            element.InnerText = nodeValue.ToString();
            xmlNode.InsertBefore((XmlNode)element, (XmlNode)null);
            document.MarkAsChanged();
        }

        public static void AddNodeWithValue(TypedXmlDocument document, string xpath, string nodeName, object nodeValue)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            if (string.IsNullOrEmpty(xpath))
                throw new ArgumentNullException("xpath");
            if (string.IsNullOrEmpty(nodeName))
                throw new ArgumentNullException("nodeName");
            if (nodeValue == null)
                throw new ArgumentNullException("nodeValue");
            XmlNode xmlNode = LocateXPath(document, xpath);
            if (xmlNode == null)
                return;
            XmlDocument xmlDocument = xmlNode.OwnerDocument;
            if (xmlDocument == null)
            {
                xmlDocument = document.Document as XmlDocument;
                if (xmlDocument == null)
                    return;
            }
            XmlElement element = xmlDocument.CreateElement(xmlDocument.DocumentElement.Prefix, nodeName, xmlDocument.DocumentElement.NamespaceURI);
            element.InnerText = nodeValue.ToString();
            xmlNode.InsertBefore((XmlNode)element, (XmlNode)null);
            document.MarkAsChanged();
        }

        public static void AddNodeIfNotThere(TypedXmlDocument document, string xpath, string nodeName, string nodeNamespace)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            if (string.IsNullOrEmpty(xpath))
                throw new ArgumentNullException("xpath");
            if (string.IsNullOrEmpty(nodeName))
                throw new ArgumentNullException("nodeName");
            if (string.IsNullOrEmpty(nodeNamespace))
                throw new ArgumentNullException("nodeNamespace");
            XmlNode xmlNode1 = LocateXPath(document, xpath);
            if (xmlNode1 == null)
                return;
            foreach (XmlNode xmlNode2 in xmlNode1.ChildNodes)
            {
                if (xmlNode2.LocalName == nodeName && xmlNode2.NamespaceURI == nodeNamespace)
                    return;
            }
            XmlDocument xmlDocument = xmlNode1.OwnerDocument;
            if (xmlDocument == null)
            {
                xmlDocument = document.Document as XmlDocument;
                if (xmlDocument == null)
                    return;
            }
            XmlElement element = xmlDocument.CreateElement(nodeName, nodeNamespace);
            xmlNode1.InsertBefore((XmlNode)element, (XmlNode)null);
            document.MarkAsChanged();
        }

        public static void AddNodeIfNotThere(TypedXmlDocument document, string xpath, string nodeName)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            if (string.IsNullOrEmpty(xpath))
                throw new ArgumentNullException("xpath");
            if (string.IsNullOrEmpty(nodeName))
                throw new ArgumentNullException("nodeName");
            XmlNode xmlNode1 = LocateXPath(document, xpath);
            if (xmlNode1 == null)
                return;
            foreach (XmlNode xmlNode2 in xmlNode1.ChildNodes)
            {
                if (xmlNode2.LocalName == nodeName)
                    return;
            }
            XmlDocument xmlDocument = xmlNode1.OwnerDocument;
            if (xmlDocument == null)
            {
                xmlDocument = document.Document as XmlDocument;
                if (xmlDocument == null)
                    return;
            }
            XmlElement element = xmlDocument.CreateElement(nodeName);
            xmlNode1.InsertBefore((XmlNode)element, (XmlNode)null);
            document.MarkAsChanged();
        }

        internal static XmlNode LocateXPath(TypedXmlDocument document, string xpath)
        {
            XmlNode document1 = document.Document;
            if (document1 == null)
                return (XmlNode)null;
            if (string.IsNullOrEmpty(xpath))
                return document1;
            else
                return document1.SelectSingleNode(xpath, document.NamespaceManager);

        }
    }
}
