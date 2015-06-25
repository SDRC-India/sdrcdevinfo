
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Xsl;
using System.Xml.XPath;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Metadata
{
    /// <summary>
    /// Helps in converting  old/existing metadata xml structure into new format
    /// </summary>
    public static class MetadataConverter
    {
        #region "-- Private --"
        
        #region "-- Methods --"

        
        /// <summary>
        /// Converts RowData nodes into para nodes
        /// </summary>
        /// <param name="row1Node"></param>
        /// <param name="newXmlDocument"></param>
        private static void ConvertRowDataNodes(XmlNode row1Node, XmlDocument newXmlDocument, XmlNode CategoryNode)
        {
            XmlNode FieldValueNode;
            XmlElement ParaNode;

            XmlText CategoryText;
            int RowDataNodesCount = 0;
            string RowDataInnerText = string.Empty;

            //Select node with name FLD_VAL.
            FieldValueNode = row1Node.SelectSingleNode(MetaDataConstants.FieldValueTagName);
            
            if (FieldValueNode != null)
            {
                RowDataNodesCount = FieldValueNode.ChildNodes.Count;
            }

            // create para element                
            ParaNode = newXmlDocument.CreateElement(MetaDataConstants.ParaTagName);

            // Converts each rowdata nodes into para nodes.
            for (int NodeIndex = 0; NodeIndex < RowDataNodesCount; NodeIndex++)
            {
                string RowDataText = string.Empty;

                //  get row data inner text
                RowDataText= FieldValueNode.ChildNodes.Item(NodeIndex).InnerText;

                if (RowDataText.Trim() == "-")
                {
                    RowDataText = RowDataText.Replace(Environment.NewLine + "-", "").Trim();
                }

                if (!string.IsNullOrEmpty(RowDataText))
                {
                    // add speical character for multiple para nodes handling
                    if (!string.IsNullOrEmpty(RowDataInnerText))
                    {
                        RowDataInnerText += MetadataManagerConstants.SpacebarSymbol + MetadataManagerConstants.ReplaceableSymbol + MetadataManagerConstants.SpacebarSymbol;            
                    }

                    RowDataInnerText += RowDataText;
                }

                // get row data inner text
                //RowDataInnerText = FieldValueNode.ChildNodes.Item(NodeIndex).InnerText;

                //MetadataEditorControlConstants.SpacebarSymbol + MetadataEditorControlConstants.ReplaceableSymbol + MetadataEditorControlConstants.SpacebarSymbol;            
            }

            // create text node
            CategoryText = newXmlDocument.CreateTextNode(RowDataInnerText);

            //Append as para node under category node
            CategoryNode.AppendChild(ParaNode);

            //Append category text in paraNode 
            ParaNode.AppendChild(CategoryText);
        }

       

        /// <summary>
        /// Converts Row1 nodes into Categoy nodes and set it's attribute value with Row1FieldName
        /// </summary>
        /// <param name="newXmlDoc"></param>
        /// <param name="oldXmlDoc"></param>
        private static void ConvertRow1Nodes(XmlDocument newXmlDoc, XmlDocument oldXmlDoc)
        {
            XmlElement CategoryNode;
            XmlNodeList Row1Nodes;
            string Row1FieldName;


            // Find Nodelist for all Row1 Element in xml document to be converted.
            Row1Nodes = oldXmlDoc.SelectNodes("//Row1");
            foreach (XmlNode Row1Node in Row1Nodes)
            {
                //Find Inner text of first child under Row1 xml Element
                Row1FieldName = Row1Node.FirstChild.InnerText;
                Row1FieldName = Row1FieldName.TrimEnd();
                Row1FieldName = Row1FieldName.TrimStart();
                //Create new xmlelement as Category
                CategoryNode = newXmlDoc.CreateElement(MetaDataConstants.CategoryTagName);

                //Set attribute values of category element.
                CategoryNode.SetAttribute(MetaDataConstants.NameAttribute, Row1FieldName);

                //Append it under metadata node
                newXmlDoc.DocumentElement.AppendChild(CategoryNode);

                // convert RowData nodes into para nodes
                MetadataConverter.ConvertRowDataNodes(Row1Node, newXmlDoc,CategoryNode);

            }
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Inserts RootNode into given xml document
        /// </summary>
        /// <param name="retVal"></param>
        internal static void InsertRootNode(XmlDocument newXmlDoc)
        {
            XmlNode RootNode;
            XmlDeclaration XmlDeclarationObj;

            RootNode = newXmlDoc.CreateElement(MetaDataConstants.MetadataTagName);

            // append root note
            XmlDeclarationObj = newXmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            newXmlDoc.InsertBefore(XmlDeclarationObj, newXmlDoc.DocumentElement);
            newXmlDoc.AppendChild(RootNode);

        }

        /// <summary>
        /// Converts metadata xml document into new format
        /// </summary>
        /// <param name="xmlFilePath">existing metadata xml file path</param>
        /// <param name="outputFileNamePath">Path for new metdata xml file path</param>
        public static void ConvertXml(String xmlFilePath, string outputFileNamePath)
        {
            XmlDocument NewXmlDoc = null;
            XmlDocument OldXmlDoc = null;
            
            //check for file existance and Type
            if (File.Exists(xmlFilePath) && Path.GetExtension(xmlFilePath) == DICommon.FileExtension.XML)
            {

                NewXmlDoc = new XmlDocument();

                //load xml document for conversion
                OldXmlDoc = new XmlDocument();
                OldXmlDoc.Load(xmlFilePath);

                // create root node 
                MetadataConverter.InsertRootNode(NewXmlDoc);

                // convert row1 nodes into Category & para nodes
                MetadataConverter.ConvertRow1Nodes(NewXmlDoc, OldXmlDoc);

                if (File.Exists(outputFileNamePath))
                {
                    File.Delete(outputFileNamePath);
                }

                //Save converted xml at specified location
                NewXmlDoc.Save(outputFileNamePath);
            }
        }

        /// <summary>
        /// Returns new converted metadata xml string. 
        /// </summary>
        /// <param name="metadataXml">Old metadata xml string</param>
        /// <returns></returns>
        public static string ConvertXml(string metadataXml)
        {
            string RetVal=string.Empty;
            XmlDocument NewXmlDoc = null;
            XmlDocument OldXmlDoc = null;
            
            NewXmlDoc = new XmlDocument();

            //load xml document for conversion
            OldXmlDoc = new XmlDocument();
           OldXmlDoc.LoadXml(metadataXml);

            // create root node 
            MetadataConverter.InsertRootNode(NewXmlDoc);

            // convert row1 nodes into Category & para nodes
            MetadataConverter.ConvertRow1Nodes(NewXmlDoc, OldXmlDoc);
                      
            RetVal = NewXmlDoc.InnerXml;
            return RetVal;
        }


        #endregion

        #endregion

    }

}

