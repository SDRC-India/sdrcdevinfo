using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries;
using DIColumns = DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Xml;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public class MetadataManager : MetaDataBuilder
    {
        #region "-- Private --"

        /// <summary>
        /// Returns categories table from database for the given metadata type
        /// </summary>
        /// <param name="metadataType"></param>
        /// <returns></returns>
        private DataTable GetCategoryDataTableFrmDB(MetaDataType metadataType)
        {
            DataTable RetVal = null;
            MetadataElementType MDElementType = MetadataElementType.Indicator;
            string ColumnInfo = string.Empty;

            switch (metadataType)
            {
                case MetaDataType.Indicator:
                    MDElementType = MetadataElementType.Indicator;
                    break;

                case MetaDataType.Map:
                    MDElementType = MetadataElementType.Area;
                    break;

                case MetaDataType.Source:
                    MDElementType = MetadataElementType.Source;
                    break;

                default:
                    break;
            }


            RetVal = this.DBConnection.ExecuteDataTable(this.DBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.Type, DIQueries.MetadataElementTypeText[MDElementType]));

            return RetVal;
        }

        /// <summary>
        /// Returns MetadaataInfo object from the given metadata text
        /// </summary>
        /// <param name="mdElementType"></param>
        /// <returns></returns>
        private MetadataInfo GetMetadataInfoFrmMetadataText(string metadataText, MetadataElementType mdElementType)
        {

            MetadataInfo RetVal = new MetadataInfo();
            CategoryInfo Category;
            XmlDocument XmlDoc;

            //string ConcatinatedParaNodeText = string.Empty;
            //XmlElement CategoryNode = default(XmlElement);
            //XmlNode CategoryXmlNode = null;
            //DataTable CategoryTable;
            //XmlElement RootNode = default(XmlElement);

            //XmlDocument XmldocumentNew = new XmlDocument();
            //XmlDeclaration xmlDeclaration = default(XmlDeclaration);
            //string XmlString = string.Empty;
            //String ConcatenatedParaNodeText = string.Empty;
            //string CategoryParaXml;

            try
            {
                //xmlDeclaration = XmldocumentNew.CreateXmlDeclaration("1.0", "utf-8", null);
                //RootNode = XmldocumentNew.CreateElement(MetadataManagerConstants.Metadata);
                //XmldocumentNew.InsertBefore(xmlDeclaration, XmldocumentNew.DocumentElement);
                //XmldocumentNew.AppendChild(RootNode);

                //--Load xml from database.
                XmlDoc = new XmlDocument();
                XmlDoc.LoadXml(metadataText);

                //--get categories 
                foreach (XmlNode Node in XmlDoc.SelectNodes("metadata/Category"))
                {

                    Category = new CategoryInfo();
                    Category.CategoryName = Convert.ToString(Node.Attributes["name"].Value);
                    Category.CategoryType = DIQueries.MetadataElementTypeText[mdElementType];

                    // get metadata text from xml's Para node
                    if (Node.ChildNodes.Count > 0)
                    {
                        Category.MetadataText = Convert.ToString(Node.ChildNodes[0].InnerXml);
                    }

                    // add category into metadataInfo object
                    if (!RetVal.Categories.ContainsKey(Category.CategoryName))
                    {
                        RetVal.Categories.Add(Category.CategoryName, Category);
                    }
                }

                //foreach (DataRow CategoryNameRow in CategoryTable.Rows)
                //{
                //    Category = new CategoryInfo();
                //    Category.CategoryName = Convert.ToString(CategoryNameRow[DIColumns.Metadata_Category.CategoryName]);
                //    Category.CategoryType = metadataType.ToString();

                //    //--Create new xmlelement as Category
                //    CategoryNode = XmldocumentNew.CreateElement(MetadataManagerConstants.Category);

                //    //--Set attribute values of category element.
                //    CategoryNode.SetAttribute(MetadataManagerConstants.NameAttribute, Category.CategoryName);

                //    //--Select Category node with appropriate attribute name(category name)
                //    CategoryXmlNode = XmlDoc.SelectSingleNode(MetadataManagerConstants.CategoryName + Category.CategoryName + MetadataManagerConstants.ClosingSymbol);

                //    //--Add in category collection
                //    if (CategoryXmlNode != null)
                //    {
                //        CategoryParaXml = CategoryXmlNode.InnerXml;
                //        Category.MetadataText = CategoryXmlNode.InnerXml;

                //        //--check for category in collection
                //        if (!RetVal.Categories.ContainsKey(Category.CategoryName))
                //        {
                //            RetVal.Categories.Add(Category.CategoryName, Category);
                //        }
                //    }
                //}
                //return RetVal;
            }
            catch
            {
                RetVal = null;
            }

            return RetVal;
        }


        /// <summary>
        /// Updates metadata object using latest category table and old categories collection
        /// </summary>
        /// <param name="metadataObject"></param>
        /// <param name="oldCategories">oldCategories collection (key = nid and value = name)</param>        
        /// <param name="categoryTable">latest category table</param>
        /// <returns></returns>
        private MetadataInfo UpdateCategryInMetadataInfo(MetadataInfo metadataObject, Dictionary<int, string> oldCategories, DataTable categoryTable)
        {
            MetadataInfo RetVal = new MetadataInfo();
            CategoryInfo CategoryObject;

            foreach (DataRow categoryRow in categoryTable.Rows)
            {


                // create new category object for each category 
                CategoryObject = new CategoryInfo();
                CategoryObject.CategoryName = Convert.ToString(categoryRow[Metadata_Category.CategoryName]);
                CategoryObject.CategoryNId = Convert.ToInt32(categoryRow[Metadata_Category.CategoryNId]);
                CategoryObject.MetadataText = string.Empty;

                // check for key existance
                if (oldCategories.ContainsKey(CategoryObject.CategoryNId))
                {
                    // check old category name exists in metadata object as metadata object (or metadata text) build on Old categories name
                    if (metadataObject.Categories.ContainsKey(oldCategories[CategoryObject.CategoryNId]))
                    {
                        //-- if category exists in metadata text then update metadata text into category object
                        CategoryObject.MetadataText = metadataObject.Categories[oldCategories[CategoryObject.CategoryNId]].MetadataText;
                    }
                }

                //-- add category into Metadata info object
                if (!RetVal.Categories.ContainsKey(CategoryObject.CategoryName))
                {
                    RetVal.Categories.Add(CategoryObject.CategoryName, CategoryObject);
                }
               
            }
            return RetVal;
        }


        /// <summary>
        /// Returns newmetadata string from collection
        /// </summary>
        /// <param name="metadataObject"></param>
        /// <returns></returns>
        private string GetNewMetadataText(MetadataInfo metadataObject)
        {

            //--Retreive from collection
            string RetVal = String.Empty;
            XmlDeclaration xmlDeclaration = default(XmlDeclaration);
            XmlElement RootNode = default(XmlElement);

            //--Construct xml.
            XmlDocument xmldocNew = new XmlDocument();
            XmlElement CategoryNode = default(XmlElement);
            XmlElement ParaNode = default(XmlElement);

            xmlDeclaration = xmldocNew.CreateXmlDeclaration("1.0", "utf-8", null);
            RootNode = xmldocNew.CreateElement(MetadataManagerConstants.Metadata);
            xmldocNew.InsertBefore(xmlDeclaration, xmldocNew.DocumentElement);
            xmldocNew.AppendChild(RootNode);

            try
            {
                //--construct xml tree 
                foreach (CategoryInfo Category in metadataObject.Categories.Values)
                {

                    //--Create new xmlelement as Category
                    CategoryNode = xmldocNew.CreateElement(MetadataManagerConstants.Category);

                    //--Set attribute values of category element.
                    CategoryNode.SetAttribute(MetadataManagerConstants.NameAttribute, Category.CategoryName);

                    // Create para node
                    ParaNode = xmldocNew.CreateElement(MetadataManagerConstants.Para);
                    ParaNode.InnerXml = Category.MetadataText;

                    // append it into Category node
                    CategoryNode.AppendChild(ParaNode);

                    // --Append it into metadata node
                    xmldocNew.DocumentElement.AppendChild(CategoryNode);
                }

                //--return xml string.
                RetVal = xmldocNew.InnerXml;
            }
            catch
            {
                RetVal = String.Empty;
            }

            return RetVal;
        }

        ///// <summary>
        ///// Save metadata into database
        ///// </summary>
        ///// <param name="mdType"></param>
        ///// <param name="elementNid"></param>
        ///// <param name="metadataString"></param>
        //private void SaveMetadataIntoDatabase(MetaDataType mdType, int elementNid, string metadataString)
        //{

        //}

        #endregion

        #region "-- Public --"

        #region "-- Properties --"


        ///// <summary>
        ///// stores updated categories
        ///// </summary> 
        //private Dictionary<int, string> _UpdatedCategories = new Dictionary<int, string>();
        //public Dictionary<int, string> UpdatedCategories
        //{
        //    get { return this._UpdatedCategories; }
        //    set { this._UpdatedCategories = value; }
        //}
        //private DataTable CategoryTable;


        //private DIConnection _DBConnection;
        ///// <summary>
        ///// Gets or sets instance of DIConnection
        ///// </summary>
        //public DIConnection DBConnection
        //{
        //    get { return this._DBConnection; }
        //    set { this._DBConnection = value; }

        //}

        //private DIQueries _DBQueries;
        ///// <summary>
        ///// Gets or sets instance of DIQueries
        ///// </summary>
        //public DIQueries DBQueries
        //{
        //    get { return this._DBQueries; }
        //    set { this._DBQueries = value; }
        //}

        #endregion

        #region "-- Methods --"

        ///// <summary>
        ///// Removes categories from collection.
        ///// </summary>
        //public void RemoveCategoriesFromCollection()
        //{
        //    this.UpdatedCategories.Clear();
        //}

        /// <summary>
        /// Update category in metadata in database.
        /// </summary>
        /// <param name="oldCategories"></param>
        public void UpdateCategoryInMetadataIntoDB(Dictionary<int, string> oldCategories, MetaDataType mdType)
        {
            DataTable Table = null;
            MetadataInfo MetadataInfoObject = null;
            MetadataElementType MDElementType = MetadataElementType.Indicator;
            string MetadataColumnName = string.Empty;
            string MetadataString = string.Empty;
            string ElementNidColumnName = string.Empty;
            int ElementNid = 0;
            string MetadataText = string.Empty;
            DataTable CategoryTable;

            MetaDataBuilder MDBuilder = new MetaDataBuilder(this.DBConnection, this.DBQueries);

            // 1. get category table order by category_order 
            CategoryTable = this.GetCategoryDataTableFrmDB(mdType);

            // 2. Get indicators/Area/Source from database where metadata text is not null or empty       
            switch (mdType)
            {

                case MetaDataType.Indicator:
                    MDElementType = MetadataElementType.Indicator;
                    MetadataColumnName = DIColumns.Indicator.IndicatorInfo;
                    ElementNidColumnName = DIColumns.Indicator.IndicatorNId;

                    Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Indicators.GetIndicator(FilterFieldType.None, String.Empty, FieldSelection.Heavy));
                    break;

                case MetaDataType.Map:
                    MDElementType = MetadataElementType.Area;
                    MetadataColumnName = DIColumns.Area_Map_Metadata.MetadataText;
                    ElementNidColumnName = DIColumns.Area_Map_Metadata.MetadataNId;

                    Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaMapMetadata(string.Empty));
                    break;

                case MetaDataType.Source:
                    MDElementType = MetadataElementType.Source;
                    MetadataColumnName = DIColumns.IndicatorClassifications.ICInfo;
                    ElementNidColumnName = DIColumns.IndicatorClassifications.ICNId;

                    Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, ICType.Source, FieldSelection.Heavy));
                    break;

                default:
                    break;
            }


            // update metedata into database
            foreach (DataRow IndicatorRow in Table.Rows)
            {
                // get metadata from row
                MetadataString = Convert.ToString(IndicatorRow[MetadataColumnName]);
                if (string.IsNullOrEmpty(MetadataString))
                {

                    XmlDocument XmlDoc = new XmlDocument();
                    DevInfo.Lib.DI_LibBAL.Metadata.MetadataConverter.InsertRootNode(XmlDoc);
                    MetadataString = XmlDoc.InnerXml;
                }
                //--check metadata string.
                if (!string.IsNullOrEmpty(MetadataString))
                {

                    // get metadataInfo object from metadata text
                    MetadataInfoObject = this.GetMetadataInfoFrmMetadataText(MetadataString, MDElementType);

                    if (MetadataInfoObject != null)
                    {

                        //-- update metadata categories into indicator metadataInfo object
                        MetadataInfoObject = this.UpdateCategryInMetadataInfo(MetadataInfoObject, oldCategories, CategoryTable);

                        ElementNid = Convert.ToInt32(IndicatorRow[ElementNidColumnName]);

                        // get new metadata text and update it into database
                        MetadataText = this.GetNewMetadataText(MetadataInfoObject);

                        MDBuilder.UpdateMetadataInfo(mdType, string.Empty, ElementNid, MetadataText);

                        #region"-- to be deleted --"
                        //switch (mdType)
                        //{
                        //    //-if Indicator
                        //    case MetaDataType.Indicator:
                        //        MetadataInfoObject = this.UpdateCategryInMetadataInfo(MetadataInfoObject, oldCategories, DIQueries.MetadataElementTypeText[MetadataElementType.Indicator]);
                        //        ElementNid = Convert.ToInt32(IndicatorRow[DIColumns.Indicator.IndicatorNId]);
                        //        MetadataText = this.GetNewMetadataText(MetadataInfoObject);
                        //        this.SaveMetadataIntoDatabase(MetaDataType.Indicator, ElementNid, MetadataText);
                        //        break;

                        //    //-If map
                        //    case MetaDataType.Map:
                        //        MetadataInfoObject = this.UpdateCategryInMetadataInfo(MetadataInfoObject, oldCategories, DIQueries.MetadataElementTypeText[MetadataElementType.Area]);
                        //        ElementNid = Convert.ToInt32(IndicatorRow[DIColumns.Area_Map_Metadata .MetadataNId ]);
                        //        MetadataText = this.GetNewMetadataText(MetadataInfoObject);
                        //        this.SaveMetadataIntoDatabase(MetaDataType.Map, ElementNid, MetadataText);
                        //        break;

                        //    //-If source.
                        //    case MetaDataType.Source:
                        //        MetadataInfoObject = this.UpdateCategryInMetadataInfo(MetadataInfoObject, oldCategories, DIQueries.MetadataElementTypeText[MetadataElementType.Source]);
                        //        ElementNid = Convert.ToInt32(IndicatorRow[DIColumns.IndicatorClassifications.ICNId]);
                        //        MetadataText = this.GetNewMetadataText(MetadataInfoObject);
                        //        this.SaveMetadataIntoDatabase(MetaDataType.Source, ElementNid, MetadataText);
                        //        break;
                        //    default:
                        //        break;
                        //}
                        #endregion
                    }

                    //else

                    ////--save metadata into database.
                    //{
                    //    switch (mdType)
                    //    {
                    //        case MetaDataType.Indicator:
                    //            ElementNid = Convert.ToInt32(IndicatorRow[DIColumns.Indicator.IndicatorNId]);
                    //            MetadataText = "";
                    //            this.SaveMetadataIntoDatabase(MetaDataType.Indicator, ElementNid, MetadataText);
                    //            break;
                    //        case MetaDataType.Map:
                    //            ElementNid = Convert.ToInt32(IndicatorRow[DIColumns .Area_Map_Metadata.MetadataNId]);
                    //            MetadataText = "";
                    //            this.SaveMetadataIntoDatabase(MetaDataType.Map, ElementNid, MetadataText);
                    //            break;
                    //        case MetaDataType.Source:
                    //            ElementNid = Convert.ToInt32(IndicatorRow[DIColumns .IndicatorClassifications .ICNId ]);
                    //            MetadataText = "";
                    //            this.SaveMetadataIntoDatabase(MetaDataType.Source, ElementNid, MetadataText);
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}
                }
            }
        }

        ///// <summary>
        /////Add categories in categoriesInfo
        ///// </summary>
        ///// <param name="categoryTable"></param>
        //public void BuildUpdatedCateogriesInfo(DataTable categoryTable)
        //{
        //    foreach (DataRow CategoryName in categoryTable.Rows)
        //    {
        //        this.UpdatedCategories.Add(Convert.ToInt32(CategoryName[DIColumns.Metadata_Category.CategoryNId]), Convert.ToString(CategoryName[DIColumns.Metadata_Category.CategoryName]));
        //    }
        //}


        #endregion

        #region "-- New/Dispose --"

        public MetadataManager(DIConnection connection, DIQueries queries)
            : base(connection, queries)
        {
            // do nothing
        }

        #endregion
        #endregion
    }
}

