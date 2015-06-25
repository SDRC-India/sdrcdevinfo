using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Collections;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// Provides method to work with pref file.
    /// </summary>
    public class DIPrefFile
    {
       
        #region "-- Private --"

        #region "-- Constants --"
            internal const string SectionTagName = "sections/section";
            internal const string ValueAttributeName = "value";
            internal const string KeyAttributeName = "key";
            internal const string ItemElementName = "item";
            internal const string SectionElementName = "section";
            internal const string NameAttribute = "name";

        #endregion

        #region "-- Variables --"
        private static string XmlFileName = string.Empty;
        private static XmlDocument XMLDoc = new XmlDocument();

        #endregion

        #region "-- Methods --"
        private static XmlNode GetSectionNode(string sectionName)
        {
            XmlNode RetVal = null;

            try
            {
                XmlNodeList Nodes = XMLDoc.SelectNodes(DIPrefFile.SectionTagName);

                foreach (XmlNode Node in Nodes)
                {

                    try
                    {
                        // check section exists
                        if (Node.Attributes["name"].Value.ToString().ToUpper() == sectionName.ToUpper())
                        {
                            RetVal = Node;
                        }
                    }
                    catch (Exception)
                    {
                        // do nothing						
                    }
                }
            }
            catch (Exception)
            {
                //throw;
                RetVal = null;
            }

            return RetVal;
        }

        #endregion

        #endregion

        #region	"--	Public --"

        ///	<summary>
        ///	To open	connection with	pref file
        ///	</summary>
        ///	<param name="fileNameWPath">file name with path</param>
        public static void Open(string fileNameWPath)
        {

            DIPrefFile.XmlFileName = fileNameWPath;
            XMLDoc = new XmlDocument();
            XMLDoc.Load(fileNameWPath);
        }

        ///	<summary>
        ///	Returns	value from pref	file
        ///	</summary>
        ///	<param name="sectionName">section name </param>
        ///	<param name="key">key</param>
        ///	<returns>string</returns>
        public static string GetValue(string sectionName, string key)
        {
            string RetVal = string.Empty;
            XmlNode SectionNode;

            try
            {
                if (DIPrefFile.GetSectionNode(sectionName) != null)
                {
                    SectionNode = DIPrefFile.GetSectionNode(sectionName);

                    foreach (XmlNode ChildNode in SectionNode.ChildNodes)
                    {

                        try
                        {
                            if (ChildNode.Attributes["key"].Value.ToString().ToUpper() == key.ToUpper())
                            {
                                RetVal = ChildNode.Attributes["value"].Value.ToString();
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            //do nothing 
                        }
                    }
                }
            }
            catch (Exception)
            {
                //throw;
                RetVal = string.Empty;
            }

            return RetVal;
        }

        ///	<summary>
        ///	To add an item element under given section name.
        ///	</summary>
        ///	<param name="sectionName">Section name</param>
        ///	<param name="key">Key string</param>
        ///	<param name="value">value string</param>
        ///	<returns>True/False.</returns>
        public static bool AddValue(string sectionName, string key, string value)
        {
            bool RetVal = false;
            XmlNode SectionNode;
            XmlNode NewNode;
            try
            {
                if (DIPrefFile.GetSectionNode(sectionName) == null)
                {
                    DIPrefFile.AddSection(sectionName);
                }

                if (DIPrefFile.GetSectionNode(sectionName) != null)
                {
                    SectionNode = DIPrefFile.GetSectionNode(sectionName);
                    NewNode = XMLDoc.CreateNode(XmlNodeType.Element, DIPrefFile.ItemElementName, string.Empty);
                    XmlAttribute KeyAttribute = XMLDoc.CreateAttribute(string.Empty, DIPrefFile.KeyAttributeName, string.Empty);
                    KeyAttribute.Value = key;

                    XmlAttribute ValueAttribute = XMLDoc.CreateAttribute(string.Empty, DIPrefFile.ValueAttributeName, string.Empty);
                    ValueAttribute.Value = value;

                    NewNode.Attributes.Append(KeyAttribute);
                    NewNode.Attributes.Append(ValueAttribute);
                    SectionNode.AppendChild(NewNode);
                    XMLDoc.Save(DIPrefFile.XmlFileName);
                    RetVal = true;
                }
            }
            catch (Exception)
            {
                //throw;
                RetVal = false;
            }

            return RetVal;
        }


        ///	<summary>
        ///	To add section .
        ///	</summary>
        ///	<param name="sectionName">Section name</param>
        public static bool AddSection(string sectionName)
        {
            bool RetVal = false;
            XmlNode SectionNode;
            XmlNode NewNode;
            try
            {
                if (DIPrefFile.GetSectionNode(sectionName) == null)
                {
                    if (XMLDoc.SelectNodes("sections").Count > 0)
                    {
                        SectionNode = XMLDoc.SelectNodes("sections")[0];

                        NewNode = XMLDoc.CreateNode(XmlNodeType.Element, DIPrefFile.SectionElementName, string.Empty);

                        XmlAttribute NameAttribute = XMLDoc.CreateAttribute(string.Empty, DIPrefFile.NameAttribute, string.Empty);
                        NameAttribute.Value = sectionName;

                        NewNode.Attributes.Append(NameAttribute);
                        SectionNode.AppendChild(NewNode);

                        XMLDoc.Save(DIPrefFile.XmlFileName);
                        RetVal = true;
                    }
                }
            }
            catch (Exception)
            {
                //throw;
                RetVal = false;
            }

            return RetVal;
        }




        ///	<summary>
        ///	To close connection.
        ///	</summary>
        public static void Close()
        {
            XMLDoc.Save(DIPrefFile.XmlFileName);

        }

        ///	<summary>
        ///	Returns	all	the	keys under a given section name
        ///	</summary>
        ///	<param name="sectionName">Name of section</param>
        ///	<returns>array list	containing all keys</returns>
        public static ArrayList GetAllSectionKeys(string sectionName)
        {
            ArrayList RetVal = new ArrayList();
            RetVal.Clear();
            XmlNode SectionNode;
            try
            {
                //Check	whether	section	lies in	XML	document
                if (DIPrefFile.GetSectionNode(sectionName) != null)
                {
                    SectionNode = DIPrefFile.GetSectionNode(sectionName);

                    //Add keys to arraylist
                    foreach (XmlNode ChildNode in SectionNode.ChildNodes)
                    {
                        try
                        {
                            RetVal.Add(ChildNode.Attributes["key"].Value.ToString());
                        }
                        catch (Exception)
                        {

                            // do nothing
                        }
                    }
                }
            }
            catch (Exception)
            {
                RetVal.Clear();
            }
            return RetVal;
        }


        ///	<summary>
        ///	Deletes	a XMLNode under	a given	section	name
        ///	</summary>
        ///	<param name="sectionName">section name string</param>
        ///	<param name="key">key name string</param>
        ///	<returns>true/false</returns>
        public static bool DeleteKey(string sectionName, string key)
        {
            bool RetVal = false;
            XmlNode SectionNode;

            try
            {
                //Check	whether	section	lies in	XML	document
                if (DIPrefFile.GetSectionNode(sectionName) != null)
                {
                    SectionNode = DIPrefFile.GetSectionNode(sectionName);

                    //iterate in all node under	a section and delete the selected one.
                    foreach (XmlNode ChildNode in SectionNode.ChildNodes)
                    {
                        try
                        {
                            if (ChildNode.Attributes["key"].Value.ToString().ToUpper() == key.ToUpper())
                            {
                                SectionNode.RemoveChild(ChildNode);
                                XMLDoc.Save(DIPrefFile.XmlFileName);
                                //returns true if node gets	deleted
                                RetVal = true;
                            }
                        }
                        catch (Exception)
                        {
                            //do nothing
                        }
                    }
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }
            return RetVal;
        }

        ///	<summary>
        ///	Updates	value of a node
        ///	</summary>
        ///	<param name="sectionName">Section name</param>
        ///	<param name="key">Key string</param>
        ///	<param name="value">value string</param>
        ///	<returns>True/False.</returns>
        public static bool UpdateValue(string sectionName, string key, string value)
        {
            bool RetVal = false;
            XmlNode SectionNode;

            try
            {
                //Check	whether	section	lies in	XML	document
                if (DIPrefFile.GetSectionNode(sectionName) != null)
                {
                    SectionNode = DIPrefFile.GetSectionNode(sectionName);

                    //iterate in all node under	a section and update the selected one.
                    foreach (XmlNode ChildNode in SectionNode.ChildNodes)
                    {
                        try
                        {
                            if (ChildNode.Attributes["key"].Value.ToString().ToUpper() == key.ToUpper())
                            {
                                ChildNode.Attributes["value"].Value = value;
                                XMLDoc.Save(DIPrefFile.XmlFileName);
                                //returns true if node gets	updated
                                RetVal = true;
                            }
                        }
                        catch (Exception)
                        {
                            //do nothing
                        }
                    }
                }


                //Add value,if value doesn't exist 
                if (RetVal == false)
                {
                    RetVal = DIPrefFile.AddValue(sectionName, key, value);
                }

            }
            catch (Exception)
            {
                RetVal = false;
            }
            return RetVal;
        }

        #endregion

    }

}
