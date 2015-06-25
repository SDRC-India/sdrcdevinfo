using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

/// <summary>
/// Provides methods to read xml file
/// </summary>
public class XmlFileReader
{
    #region "-- Private --"

    #region "-- Variables --"

    private string FileNameWPath = string.Empty;
    private XmlDocument XMLFile;

    #endregion

    #region "-- New/Dispose --"

    #endregion

    #region "-- Methods --"

    #endregion

    #endregion

    #region "-- Public --"

    #region "-- Variables --"

    #endregion

    #region "-- New/Dispose --"

    public XmlFileReader(string filenameWPath)
    {
        try
        {
            this.FileNameWPath = filenameWPath;
            this.XMLFile = new XmlDocument();
            this.XMLFile.Load(this.FileNameWPath);
        }
        catch (Exception ex)
        {
           
        }
    }

    #endregion

    #region "-- Methods --"

    /// <summary>
    /// Returns all values
    /// </summary>
    /// <param name="parentElementName"></param>
    /// <returns></returns>
    public List<string> GetValues(string parentElementName, string attributeName)
    {
        List<string> RetVal = new List<string>();
        string Value = string.Empty;

        if (XMLFile.SelectNodes(parentElementName).Count > 0)
        {
            foreach (XmlNode Node in XMLFile.SelectNodes(parentElementName)[0].ChildNodes)
            {
                Value = Node.Attributes[attributeName].Value;
                if (!RetVal.Contains(Value))
                {
                    RetVal.Add(Value);
                }
            }
        }
        return RetVal;
    }


    /// <summary>
    /// Returns attribute's value for the given element
    /// </summary>
    /// <param name="parentelementName"></param>
    /// <param name="nameAttribute"></param>
    /// <param name="name"></param>
    /// <param name="valueAttribute"></param>
    /// <returns></returns>
    public string GetValue(string parentElementName, string nameAttribute, string name, string valueAttribute)
    {
        string RetVal = string.Empty;

        if (XMLFile.SelectNodes(parentElementName).Count > 0)
        {
            foreach (XmlNode Node in XMLFile.SelectNodes(parentElementName)[0].ChildNodes)
            {
                if (Node.Attributes[nameAttribute].Value.ToUpper() == name.ToUpper())
                {
                    RetVal = Node.Attributes[valueAttribute].Value;
                    break;
                }
            }
        }

        return RetVal;
    }

    public string GetXMLNodeValue(string nodePath)
    {
        string RetVal = string.Empty;

        XmlNode XMLNode = XMLFile.SelectSingleNode(nodePath);
        if (XMLNode != null)
        {
            RetVal = XMLNode.FirstChild.Value;
        }

        return RetVal;
    }

    public string GetXMLNodeAttributeValue(string nodePath, string ValueAttributeName)
    {
        string RetVal = string.Empty;

        XmlNode XMLNode = XMLFile.SelectSingleNode(nodePath);
        if (XMLNode != null)
        {
            RetVal = XMLNode.Attributes[ValueAttributeName].Value;
        }

        return RetVal;
    }

    public static string ReadXML(string xmlFileNameWPath, string xPath)
    {
        string RetVal = string.Empty;

        XmlNode xmlNode = null;
        XmlElement Element = null;
        XmlElement root = null;
        XmlDocument doc = null;
        XmlTextReader reader = null;

        try
        {
            //load document at once
            reader = new XmlTextReader(xmlFileNameWPath);
            doc = new XmlDocument();
            doc.Load(reader);
            reader.Close();

            root = doc.DocumentElement;
            if (!string.IsNullOrEmpty(xPath))
            {
                xmlNode = root.SelectSingleNode(xPath);
                if (xmlNode != null)
                {
                    RetVal = xmlNode.InnerXml;
                }
            }
            else
            {
                RetVal = doc.InnerXml;
            }
        }
        catch (Exception ex)
        {
            throw;
        }

        return RetVal;
    }

    #endregion

    #endregion

}