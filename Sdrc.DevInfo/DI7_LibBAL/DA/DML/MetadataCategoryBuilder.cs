using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.Metadata;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provide methods to Add,update,delete records in MetadataCategory table
    /// Update Metadata from mask Files
    /// </summary>
    public class MetadataCategoryBuilder
    {

        #region "-- private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Retruns nid only if name exists in the database
        /// </summary>
        /// <returns></returns>
        private int GetNidByName(string name, string categoryType)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            DataTable Table = null;
            try
            {
                SqlQuery = this.DBQueries.Metadata_Category.GetMetadataCategoriesByCategoryname(name, categoryType);

                Table = this.DBConnection.ExecuteDataTable(SqlQuery);

                if (Table.Rows.Count > 0)
                {
                    RetVal = Convert.ToInt32(Table.Rows[0][Metadata_Category.CategoryNId]);
                }

            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Retruns nid only if name exists in the database
        /// </summary>
        /// <returns></returns>
        private int GetNidByName(string name, DIQueries dbQueries)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = dbQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.Name, "'" + DIQueries.RemoveQuotesForSqlQuery(name) + "'");
                RetVal = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(SqlQuery));
            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }

        /// <summary>
        /// Insert MetadataCategory record into database
        /// </summary>
        /// <param name="MetadataCategoryInfo">object of IndicatorInfo</param>
        /// <returns>Ture/False. Return true after successful insertion otherwise false</returns>
        private bool InsertIntoDatabase(MetadataCategoryInfo MetadataCategoryInfo)
        {
            bool RetVal = false;
            string MetadataCategoryName = MetadataCategoryInfo.CategoryName;
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string MetadataCategoryForDatabase = string.Empty;
            DITables TablesName;
            int LastOrder = 0;
            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                //get and set category order
                // get max category order from database/template
                try
                {
                    LastOrder = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.Metadata_Category.GetMaxMetadataCategoryOrder(MetadataCategoryInfo.CategoryType)));
                }
                catch (Exception)
                {
                }
                LastOrder += 1;
                MetadataCategoryInfo.CategoryOrder = LastOrder;


                // insert metadata category into database/template
                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {

                    LanguageCode = languageRow[Language.LanguageCode].ToString();
                    TablesName = new DITables(this.DBQueries.DataPrefix, LanguageCode);

                    //if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    //{
                    MetadataCategoryForDatabase = MetadataCategoryName;
                    //}
                    //else
                    //{
                    //    MetadataCategoryForDatabase = Constants.PrefixForNewValue + MetadataCategoryName;
                    //}

                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Insert.InsertMetadataCategory(TablesName.MetadataCategory, MetadataCategoryForDatabase, MetadataCategoryInfo.CategoryType, MetadataCategoryInfo.CategoryOrder));
                }

                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;

        }


        private bool InsertIntoDatabaseForCurrLangOnly(MetadataCategoryInfo MetadataCategoryInfo)
        {
            bool RetVal = false;
            string MetadataCategoryName = MetadataCategoryInfo.CategoryName;
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string MetadataCategoryForDatabase = string.Empty;
            DITables TablesName;
            int LastOrder = 0;
            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                //get and set category order
                // get max category order from database/template
                try
                {
                    LastOrder = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.Metadata_Category.GetMaxMetadataCategoryOrder(MetadataCategoryInfo.CategoryType)));
                }
                catch (Exception)
                {
                }
                LastOrder += 1;
                MetadataCategoryInfo.CategoryOrder = LastOrder;

                // insert metadata category into database/template
                MetadataCategoryForDatabase = MetadataCategoryName;

                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Insert.InsertMetadataCategory(this.DBQueries.TablesName.MetadataCategory, MetadataCategoryForDatabase, MetadataCategoryInfo.CategoryType, MetadataCategoryInfo.CategoryOrder));


                RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;

        }


        private int CheckNUpdateMetadataCategoryInLangTable(MetadataCategoryInfo metadataCategoryInfo, string languageCode, string defaultLanguageCategoryName)
        {
            int RetVal = 0;
            DITables TablesName;
            DIQueries NewDBQueries = null;
            string SqlQuery = string.Empty;

            try
            {
                NewDBQueries = new DIQueries(this.DBQueries.DataPrefix, languageCode);
                TablesName = new DITables(this.DBQueries.DataPrefix, languageCode);

                // check Metadata Category exists or not
                RetVal = this.GetNidByName(metadataCategoryInfo.CategoryName, NewDBQueries);

                metadataCategoryInfo.CategoryNId = this.GetNidByName(defaultLanguageCategoryName, NewDBQueries);
                // if Metadata Category does not exist then create it.
                if (RetVal <= 0 && metadataCategoryInfo.CategoryNId > 0)
                {

                    SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Update.UpdateMetadataCategory(TablesName.MetadataCategory, metadataCategoryInfo.CategoryNId, DICommon.RemoveQuotes(metadataCategoryInfo.CategoryName), metadataCategoryInfo.CategoryType, metadataCategoryInfo.CategoryOrder);

                    this.DBConnection.ExecuteNonQuery(SqlQuery);

                    RetVal = this.GetNidByName(metadataCategoryInfo.CategoryName, metadataCategoryInfo.CategoryType);

                }

                // add indicator information 
                metadataCategoryInfo.CategoryNId = RetVal;

            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }


        #region "-- Convert Metadata --"

        private void ConvertIndicatorMetadata()
        {
            DataTable IndicatorTable = null;
            string IndicatorMetadata = string.Empty;

            IndicatorTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Indicators.GetIndicator(FilterFieldType.Search, Indicator.IndicatorInfo + " IS NOT NULL AND " + Indicator.IndicatorInfo + " <> '' ", FieldSelection.Heavy));

            foreach (DataRow Row in IndicatorTable.Rows)
            {
                IndicatorMetadata = Convert.ToString(Row[Indicator.IndicatorInfo]);
                if (!string.IsNullOrEmpty(IndicatorMetadata))
                {

                    IndicatorMetadata = DICommon.CheckNConvertMetadataXml(IndicatorMetadata);

                    // Update IndicatorInfo(Metadata)
                    this.DBConnection.ExecuteNonQuery(DALQueries.Indicator.Update.UpdateIndicatorInfo(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, DICommon.RemoveQuotes(IndicatorMetadata), FilterFieldType.NId, Convert.ToString(Row[Indicator.IndicatorNId])));

                }
            }

        }

        private void ConvertICMetadata()
        {
            DataTable ICTable = null;
            string ICInfo = string.Empty;
            // Get Indicator Metadata Table
            ICTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetICForSearch(FilterFieldType.Search, IndicatorClassifications.ICInfo + " IS NOT NULL AND " + IndicatorClassifications.ICInfo + " <> '' ", FieldSelection.Heavy));

            // Update each Indicator Row where Metadata exists
            foreach (DataRow Row in ICTable.Rows)
            {
                ICInfo = Convert.ToString(Row[IndicatorClassifications.ICInfo]);
                if (!string.IsNullOrEmpty(ICInfo))
                {
                    ICInfo = MetadataConverter.ConvertXml(ICInfo);

                    // Update IndicatorClassificationInfo(Source Metadata)
                    this.DBConnection.ExecuteNonQuery(DALQueries.IndicatorClassification.Update.UpdateICInfo(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, DICommon.RemoveQuotes(ICInfo), ICType.Source, Convert.ToInt32(Row[IndicatorClassifications.ICNId])));
                }
            }
        }

        private void ConvertAreaMapMetadata()
        {
            DataTable MapTable = null;
            string MetadataText = string.Empty;

            // Get Area_Map Metadata Table
            MapTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaMapMetadata(FilterFieldType.Search, Area_Map_Metadata.MetadataText + " IS NOT NULL AND " + Area_Map_Metadata.MetadataText + " <> '' ", FieldSelection.Heavy));

            foreach (DataRow Row in MapTable.Rows)
            {
                MetadataText = Convert.ToString(Row[Area_Map_Metadata.MetadataText]);
                if (!string.IsNullOrEmpty(MetadataText))
                {

                    MetadataText = DICommon.CheckNConvertMetadataXml(MetadataText);

                    // Update IndicatorClassificationInfo(Metadata)
                    this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.Area.UpdateAreaMetadataInfo(DICommon.RemoveQuotes(MetadataText), Convert.ToString(Row[Area_Map_Metadata.LayerName])));

                }
            }
        }

        private void UpdateXsltMetadata()
        {
            DataTable MapTable = null;
            string MetadataText = string.Empty;

            // Get Area_Map Metadata Table
            MapTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Xslt.GetXSLT(FilterFieldType.Search, XSLT.XSLTText + " IS NOT NULL OR " + XSLT.XSLTText + " <> '' "));

            foreach (DataRow Row in MapTable.Rows)
            {
                MetadataText = Convert.ToString(Row[XSLT.XSLTText]);
                if (!string.IsNullOrEmpty(MetadataText))
                {
                    MetadataText = MetadataConverter.ConvertXml(MetadataText);

                    // Update IndicatorClassificationInfo(Metadata)
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Xslt.Update.UpdateXSLT(this.DBQueries.DataPrefix, DICommon.RemoveQuotes(MetadataText), Convert.ToString(Row[XSLT.XSLTFile])));
                }
            }
        }

        #endregion

        private void UpdateXsltFromResource()
        {
            string DefaultXsltText = string.Empty;
            DataTable XsltTable = null;

            XsltTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Xslt.GetXSLT(FilterFieldType.None, string.Empty));

            // Get Xslt Text from Resource File
            DefaultXsltText = Resource1.XSLTFile2;

            foreach (DataRow Row in XsltTable.Rows)
            {
                // Update XSLT Text value to Resource File Xslt Text
                this.DBConnection.ExecuteNonQuery(DALQueries.Xslt.Update.UpdateXSLT(this.DBQueries.DataPrefix, DICommon.RemoveQuotes(DefaultXsltText), Convert.ToString(Row[XSLT.XSLTFile])));

            }
        }

        private string GetMaskFolderPath()
        {
            string RetVal = string.Empty;
            string AppStartPath = Application.StartupPath;
            XmlDocument XmlDoc = new XmlDocument();

            // 1. Read Adaptation File if exist (for User Interface)
            if (File.Exists(Path.Combine(AppStartPath, Constants.MetadataCategory.DIAdaptationsFileName)))
            {
                // Load AdaptionFile
                XmlDoc.Load(Path.Combine(AppStartPath, Constants.MetadataCategory.DIAdaptationsFileName));
                // Get AdaptionFolderPath Element Value
                RetVal = XmlDoc.GetElementsByTagName(Constants.MetadataCategory.DIAdaptationElementName)[0][Constants.MetadataCategory.DIAdaptionFooderElementName].InnerText;
                // Get Mask Folder Path
                RetVal = Path.Combine(RetVal, Constants.MetadataCategory.MaskFolderPath);
            }

            // 2. Get Mask Folder Path if exist (for Data Admin )
            else if (Directory.Exists(Path.Combine(AppStartPath, Constants.MetadataCategory.MaskFolderPath)))
            {
                RetVal = Path.Combine(AppStartPath, Constants.MetadataCategory.MaskFolderPath);
            }

            return RetVal;
        }

        private void ReadMaskMetadataNInsertIntoCategoryTable(XmlDocument XmlDoc, CategoryType categoryType)
        {
            int CategoryOrder = 0;
            XmlNodeList RootNodeList;
            string DefaultCategoryLanguageValue = string.Empty;
            MetadataCategoryInfo MetadataInfo;
            string CategoryTypeText = string.Empty;

            // Get Mask Type like "I" for Indicator, "A" for Map and "S" for Source
            CategoryTypeText = this.GetCategoryTypeText(categoryType);

            // Get Root Element NodeList 
            RootNodeList = XmlDoc.SelectNodes(Constants.MetadataCategory.RootElementName);

            // Get first recrod from "root/Input<n>" Node and insert into all available Metadata_Category language table
            // Check Category In each "root/Input<n>" Node
            foreach (XmlElement InputNodeList in RootNodeList.Item(0).ChildNodes)
            {
                CategoryOrder++;
                // Check category In Input Node and insert metdata category language value into all metadata category language tables
                for (int i = 0; i < InputNodeList.ChildNodes.Count; i++)
                {
                    if (InputNodeList.ChildNodes[i].Name == Constants.MetadataCategory.CaptionElementName)
                    {
                        // set category value into default category langauge value
                        DefaultCategoryLanguageValue = InputNodeList.ChildNodes[i].InnerText;

                        // check xml languagecode is default language
                        if (InputNodeList.ChildNodes[i].Attributes[Constants.MetadataCategory.LangAttributeName].Value.ToUpper() == this.DBQueries.LanguageCode.Replace("_", "").ToUpper())
                        {

                            MetadataInfo = new MetadataCategoryInfo();

                            // Set MetadataCategoryInfo Value
                            MetadataInfo.CategoryName = InputNodeList.ChildNodes[i].InnerText;
                            MetadataInfo.CategoryType = CategoryTypeText;
                            MetadataInfo.CategoryOrder = CategoryOrder;

                            // Add MetadataCategory Into all metdata category language tables
                            this.CheckNCreateMetadataCategoryForCurrentLanguageOnly(MetadataInfo);
                            break;
                        }
                    }

                }

                // update metadata category language value into their respective metadata category language table


                for (int i = 0; i < InputNodeList.ChildNodes.Count; i++)
                {
                    if (InputNodeList.ChildNodes[i].Name == Constants.MetadataCategory.CaptionElementName)
                    {
                        // Check lang attribute is valid language or not
                        if (this.DBConnection.IsValidDILanguage(this.DBQueries.DataPrefix, InputNodeList.ChildNodes[i].Attributes[Constants.MetadataCategory.LangAttributeName].Value))
                        {

                            MetadataInfo = new MetadataCategoryInfo();
                            // Set MetadataCategoryInfo Value
                            MetadataInfo.CategoryName = InputNodeList.ChildNodes[i].InnerText;
                            MetadataInfo.CategoryType = CategoryTypeText;
                            MetadataInfo.CategoryOrder = CategoryOrder;

                            if (MetadataInfo.CategoryName != DefaultCategoryLanguageValue)
                            {
                                // update MetadataCategory Into database
                                this.CheckNUpdateMetadataCategoryInLangTable(MetadataInfo, "_" + InputNodeList.ChildNodes[i].Attributes[Constants.MetadataCategory.LangAttributeName].Value, DefaultCategoryLanguageValue);
                            }

                        }
                    }

                }
            }

        }

        private bool InsertXmlMetadataIntoCategoryTable(List<string> categoryList, CategoryType categoryType)
        {
            bool RetVal = false;
            int CategoryOrder = 0;
            MetadataCategoryInfo MetadataInfo;
            string CategoryTypeText = this.GetCategoryTypeText(categoryType);

            foreach (string CategoryItem in categoryList)
            {
                CategoryOrder++;
                MetadataInfo = new MetadataCategoryInfo();
                MetadataInfo.CategoryName = CategoryItem;
                MetadataInfo.CategoryType = CategoryTypeText;
                MetadataInfo.CategoryOrder = CategoryOrder;
                // Add MetadataCategory Into all metdata category language tables
                this.CheckNCreateMetadataCategoryForCurrentLanguageOnly(MetadataInfo);
                RetVal = true;
            }
            return RetVal;
        }

        private bool GetMetadataNUpdateMetadataCategory(CategoryType categoryMaskType)
        {
            bool RetVal = false;
            string InfoString = string.Empty;
            XmlDocument XmlDoc = new XmlDocument();
            string CategoryType = string.Empty;
            List<string> MetadataCategoryList = null;

            try
            {
                //-- Get Xml from Database
                MetadataCategoryList = this.GetCategoriesFromDatabase(categoryMaskType);

                //-- Update MetadataCategory  If Ccategory Found in Database else update fro Mask File
                if (MetadataCategoryList != null && MetadataCategoryList.Count > 0)
                {
                    RetVal = this.InsertXmlMetadataIntoCategoryTable(MetadataCategoryList, categoryMaskType);

                }
                else
                {
                    //-- Get XMLDoc for Mask Files
                    XmlDoc = this.GetMaskFileXmlDocument(categoryMaskType);
                    //-- Update Metadata_Category Table from Mask Xml
                    this.ReadMaskMetadataNInsertIntoCategoryTable(XmlDoc, categoryMaskType);
                    RetVal = true;
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        private XmlDocument GetMaskFileXmlDocument(CategoryType categoryMaskType)
        {
            string MetadataXmlFilePath = string.Empty;
            XmlDocument RetVal = new XmlDocument();
            string InfoString = string.Empty;
            string MaskFolderPath = string.Empty;

            // 1. Get Mask File Folder Path
            MaskFolderPath = this.GetMaskFolderPath();

            // Get Mask File For Indicator, Map and Source
            switch (categoryMaskType)
            {
                case CategoryType.INDICATOR:        // Get Mask File path or Default xml For Indicator
                    MetadataXmlFilePath = Path.Combine(MaskFolderPath, Constants.MetadataCategory.IndicatorMaskFileName);

                    break;
                case CategoryType.MAP:          // Get Mask File path or Default xml For Area Map
                    MetadataXmlFilePath = Path.Combine(MaskFolderPath, Constants.MetadataCategory.AreaMapMaskFileName);

                    break;
                case CategoryType.SOURCE:       // Get Mask File path or Default xml For Source
                    MetadataXmlFilePath = Path.Combine(MaskFolderPath, Constants.MetadataCategory.SourceMaskFileName);

                    break;
                default:
                    break;
            }

            // Load Default mask xml if Mask file not exist
            if (File.Exists(MetadataXmlFilePath))
            {
                RetVal.Load(MetadataXmlFilePath);
                if (!this.ISValidMaskFileFormat(RetVal.InnerXml))
                {
                    RetVal = null;
                }
            }

            return RetVal;
        }

        private List<string> GetCategoriesFromDatabase(CategoryType categoryType)
        {
            List<string> RetVal = null;
            DataTable Table = null;
            string InfoColName = string.Empty;

            // Get Info for Indicator, Map, Source
            switch (categoryType)
            {
                case CategoryType.INDICATOR:    // Get Info Table for Indictor
                    Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Indicators.GetIndicator(FilterFieldType.Search, Indicator.IndicatorInfo + " IS NOT NULL AND " + Indicator.IndicatorInfo + " <> '' ", FieldSelection.Heavy));
                    InfoColName = Indicator.IndicatorInfo;
                    break;
                case CategoryType.MAP:          // Get Info Table for Map
                    Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaMapMetadata(FilterFieldType.Search, Area_Map_Metadata.MetadataText + " IS NOT NULL AND " + Area_Map_Metadata.MetadataText + " <> '' ", FieldSelection.Heavy));
                    InfoColName = Area_Map_Metadata.MetadataText;

                    break;
                case CategoryType.SOURCE:       // Get Info Table for Source
                    Table = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetIC(FilterFieldType.Search, IndicatorClassifications.ICInfo + " IS NOT NULL AND " + IndicatorClassifications.ICInfo + " <> '' ", FieldSelection.Heavy));
                    InfoColName = IndicatorClassifications.ICInfo;
                    break;
                default:
                    break;
            }

            // Get First avilable Info Value
            if (Table.Rows.Count > 0)
            {
                RetVal = this.GetCategoriesListFromTable(Table, InfoColName);
            }

            return RetVal;
        }

        private List<string> GetCategoriesListFromTable(DataTable table, string infoColumn)
        {
            List<string> RetVal = new List<string>();
            string XmlInfoString = string.Empty;
            string CategoryValue = string.Empty;
            int CategoryOrder = 0;
            XmlDocument XmlDoc = new XmlDocument();
            XmlNodeList MetadataNodeList;

            foreach (DataRow Row in table.Rows)
            {
                CategoryOrder = 0;
                // Get Metadata 
                XmlInfoString = Convert.ToString(Row[infoColumn]);

                if (this.ISValidInfoXmlFormat(XmlInfoString))
                {
                    XmlDoc.LoadXml(XmlInfoString);
                    // Get metadata Element NodeList 
                    MetadataNodeList = XmlDoc.SelectNodes(Constants.MetadataCategory.MetadataCategoryNodePath);

                    // Check Category In each "Category" Element
                    foreach (XmlNode InputNodeList in MetadataNodeList)
                    {
                        // Check attribute Name is exist or not                    
                        if (InputNodeList.Attributes[0].Name == Constants.MetadataCategory.NameAttribute)
                        {

                            CategoryValue = Convert.ToString(InputNodeList.Attributes[Constants.MetadataCategory.NameAttribute].Value);

                            // Add MetadataCategory
                            if (!this.IsCategoryExistInCollection(RetVal, CategoryValue.Trim()))
                            {
                                if (CategoryOrder < RetVal.Count)
                                {
                                    RetVal.Insert(CategoryOrder, CategoryValue.Trim());

                                }
                                else
                                {
                                    RetVal.Add(CategoryValue.Trim());

                                }
                            }
                            CategoryOrder++;
                        }
                    }
                }
            }

            return RetVal;
        }

        private bool IsCategoryExistInCollection(List<string> categoryList, string categoryVal)
        {
            bool RetVal = false;

            foreach (string Item in categoryList)
            {
                if (Item.ToLower() == categoryVal.ToLower())
                {
                    RetVal = true;
                    break;
                }
            }

            return RetVal;
        }

        private string GetCategoryTypeText(CategoryType categoryMaskType)
        {
            string RetVal = string.Empty;

            switch (categoryMaskType)
            {
                case CategoryType.INDICATOR:
                    RetVal = "I";
                    break;
                case CategoryType.MAP:
                    RetVal = "A";
                    break;
                case CategoryType.SOURCE:
                    RetVal = "S";
                    break;
            }


            return RetVal;

        }

        #endregion

        #endregion

        #region "-- public --"

        #region "-- ENUM --"
        /// <summary>
        /// Enumerator For Mask TYpe for Indicator,Map,Source
        /// </summary>
        internal enum CategoryType
        {
            INDICATOR,
            MAP,
            SOURCE
        }

        #endregion

        #region "-- Variables --"

        #endregion

        #region "-- New/Dispose --"
        /// <summary>
        /// Constructor for MetadataCategoryBuilder Class
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="queries"></param>
        public MetadataCategoryBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
        }

        #endregion

        #region "-- Methods --"

        #region "-- MetadataCategory Table --"

        /// <summary>
        /// Returns true if Metadata_Category table exists otherwise false
        /// </summary>
        /// <returns></returns>
        public bool IsMetadataCategoryTableExists()
        {
            bool RetVal = false;

            try
            {
                //-- Check the existence of the table 
                if (this.DBConnection.ExecuteDataTable("SELECT count(*) FROM " + this.DBQueries.TablesName.MetadataCategory + " WHERE 1=1") != null)
                    RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        #endregion

        #region "-- Metadata Category Creation --"

        /// <summary>
        /// return all records of Metadata_Category table
        /// </summary>
        /// <returns></returns>
        public DataTable GetAllRecordsFromMetadataCategory()
        {
            DataTable RetVal = null;

            try
            {
                //-- Get Metadata_Category records table 
                RetVal = this.DBConnection.ExecuteDataTable(this.DBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.None, string.Empty));

            }
            catch (Exception)
            {
                RetVal = null;
            }

            return RetVal;
        }

        /// <summary>
        /// Check existance of Metadata Category record into database if false then create MetadataCategory record 
        /// </summary>
        /// <param name="metadataCategoryInfo">object of MetadataCategoryInfo</param>
        /// <returns>MetadataCategoryNid</returns>
        public int CheckNCreateMetadataCategory(MetadataCategoryInfo metadataCategoryInfo)
        {
            int RetVal = 0;

            try
            {
                // check Metadata Category exists or not
                RetVal = this.GetNidByName(metadataCategoryInfo.CategoryName, metadataCategoryInfo.CategoryType);

                // if Metadata Category does not exist then create it.
                if (RetVal <= 0)
                {
                    // insert Metadata Category
                    if (this.InsertIntoDatabase(metadataCategoryInfo))
                    {
                        RetVal = this.GetNidByName(metadataCategoryInfo.CategoryName, metadataCategoryInfo.CategoryType);
                    }
                }

                // add indicator information 
                metadataCategoryInfo.CategoryNId = RetVal;

            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Check existance of Metadata Category record into database if false then create MetadataCategory record 
        /// </summary>
        /// <param name="metadataCategoryInfo">object of MetadataCategoryInfo</param>
        /// <returns>MetadataCategoryNid</returns>
        public int CheckNCreateMetadataCategoryForCurrentLanguageOnly(MetadataCategoryInfo metadataCategoryInfo)
        {
            int RetVal = 0;

            try
            {
                // check Metadata Category exists or not
                RetVal = this.GetNidByName(metadataCategoryInfo.CategoryName, metadataCategoryInfo.CategoryType);

                // if Metadata Category does not exist then create it.
                if (RetVal <= 0)
                {
                    // insert Metadata Category
                    if (this.InsertIntoDatabaseForCurrLangOnly(metadataCategoryInfo))
                    {
                        RetVal = this.GetNidByName(metadataCategoryInfo.CategoryName, metadataCategoryInfo.CategoryType);
                    }
                }

                // add indicator information 
                metadataCategoryInfo.CategoryNId = RetVal;

            }
            catch (Exception)
            {
                RetVal = 0;
            }

            return RetVal;
        }

        /// <summary>
        /// Updates the Category information into database on the basis of CategoryNId
        /// </summary>
        /// <param name="categoryName"></param>
        /// <param name="categoryType"></param>
        /// <param name="categoryOrder"></param>
        /// <param name="categoryNId"></param>
        public void UpdateMetadataCategory(string categoryName, string categoryType, int categoryOrder, int categoryNId)
        {

            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Update.UpdateMetadataCategory(this.DBQueries.TablesName.MetadataCategory, categoryNId, categoryName, categoryType, categoryOrder);

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Returns instance of MetadataCategoryInfo details for filerText.
        /// </summary>
        /// <param name="filterClause"></param>
        /// <param name="filterText"></param>
        /// <returns></returns>
        public MetadataCategoryInfo GetMetadataCategoryInfo(FilterFieldType filterClause, string filterText)
        {
            string Query = string.Empty;
            MetadataCategoryInfo RetVal = new MetadataCategoryInfo();
            DataTable MetadataCategoryTable;
            try
            {
                //get Metadata_Category information
                Query = this.DBQueries.Metadata_Category.GetMetadataCategories(filterClause, filterText);
                MetadataCategoryTable = this.DBConnection.ExecuteDataTable(Query);

                //set Metadata Category info
                if (MetadataCategoryTable != null)
                {
                    if (MetadataCategoryTable.Rows.Count > 0)
                    {
                        RetVal.CategoryName = MetadataCategoryTable.Rows[0][Metadata_Category.CategoryName].ToString();
                        RetVal.CategoryType = Convert.ToString(MetadataCategoryTable.Rows[0][Metadata_Category.CategoryType]);
                        RetVal.CategoryOrder = Convert.ToInt32(MetadataCategoryTable.Rows[0][Metadata_Category.CategoryOrder]);
                        RetVal.CategoryNId = Convert.ToInt32(MetadataCategoryTable.Rows[0][Metadata_Category.CategoryNId]);

                    }
                }
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Deletes MetadataCategory
        /// </summary>
        /// <param name="indicatorNids"></param>
        public void DeleteMetadataCategory(string metadataCategoryNids)
        {
            DITables TableNames;

            try
            {

                // Step 1: Delete records from Indicator table for each Language
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    // Get Language Table Name for Indicator
                    TableNames = new DITables(this.DBQueries.DataPrefix, "_" + Row[Language.LanguageCode].ToString());
                    // Delete MetadataCategory
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Delete.DeleteMetadataCategory(TableNames.MetadataCategory, metadataCategoryNids));

                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        #endregion

        #region "-- Update Metadata --"

        /// <summary>
        /// Converts Indicator, Area Map and Indicator Classification Metadata,
        /// And Update XSLT Text from Resource File
        /// </summary>
        public void ConvertIndicatorMapICAndXsltMetadataIntoNewFormat()
        {
            // Update Indicator Table Metadata
            this.ConvertIndicatorMetadata();

            // Update Indicator Classification Table
            this.ConvertICMetadata();

            // Update Area Map Metadata
            this.ConvertAreaMapMetadata();

            // Update Default Value of Xslt from Resource File
            this.UpdateXsltFromResource();

        }

        /// <summary>
        /// Records addded to Metadata Category table from ([DA Root]\Bin\Templates\Metadata\Mask           /// OR [DI Adaptation]\Bin\Templates\Metadata\Mask)  xml files
        /// <param name="templateFolder">[DA Root]\Bin\Templates\ OR [DI Adaptation]\Bin\Templates\</param>
        /// </summary>
        /// <param name="adaptationPath"></param>
        public void UpdateMetadataCategoryTableFromMaskFile()
        {
            // 2. Load Metadata and Update MetadataCategory Table for Indicator
            this.GetMetadataNUpdateMetadataCategory(CategoryType.INDICATOR);


            // 3. Load Metadata and Update MetadataCategory Table for Area Map
            this.GetMetadataNUpdateMetadataCategory(CategoryType.MAP);

            // 4. Load Metadata and Update MetadataCategory Table for Source
            this.GetMetadataNUpdateMetadataCategory(CategoryType.SOURCE);

        }

        /// <summary>
        /// return true if xmlstring is of Correct format.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public bool ISValidMaskFileFormat(string xmlString)
        {
            bool RetVal = false;

            XmlDocument XmlDoc = new XmlDocument();


            try
            {
                XmlDoc.LoadXml(xmlString);
                // Get Root Element NodeList 
                XmlNodeList RootNodeList = XmlDoc.SelectNodes(Constants.MetadataCategory.RootElementName);
                // Check Category In each "root/Input<n>" Node
                foreach (XmlElement InputNodeList in RootNodeList.Item(0).ChildNodes)
                {
                    // Check category In Input Node and insert metdata category language value into all metadata category language tables
                    for (int i = 0; i < InputNodeList.ChildNodes.Count; i++)
                    {
                        if (InputNodeList.ChildNodes[i].Name == Constants.MetadataCategory.CaptionElementName)
                        {
                            RetVal = true;
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// return true if xmlstring is of Correct format.
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public bool ISValidInfoXmlFormat(string xmlString)
        {
            bool RetVal = false;

            XmlDocument XmlDoc = new XmlDocument();


            try
            {
                XmlDoc.LoadXml(xmlString);
                // Get Root Element NodeList 
                XmlNodeList MetadataNodeList = XmlDoc.SelectNodes(Constants.MetadataCategory.Metadata);
                // Check Category In each "root/Input<n>" Node
                foreach (XmlElement InputNodeList in MetadataNodeList.Item(0).ChildNodes)
                {
                    // Check attribute Name is exist or not                    
                    if (InputNodeList.Attributes[0].Name == Constants.MetadataCategory.NameAttribute)
                    {
                        RetVal = true;
                    }
                }

            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }


        public void ReadXmlAndUpdateCategoryTable(string metadataXmlInfo, string metadataTypeText)
        {
            int CategoryOrder = 0;
            XmlDocument XmlDoc = new XmlDocument();
            XmlNodeList RootNodeList;
            string DefaultCategoryLanguageValue = string.Empty;
            MetadataCategoryInfo MetadataInfo;
            string CategoryForDatabase = string.Empty;
            DITables Tables = null;
            DIQueries TempDBQueries = null;

            if (!string.IsNullOrEmpty(metadataXmlInfo.Trim()))
            {
                XmlDoc.LoadXml(metadataXmlInfo);
                // Get "Metadata" Root Element NodeList 
                RootNodeList = XmlDoc.SelectNodes(Constants.MetadataCategory.MetadataCategoryNodePath);

                // Get first recrod from "metadata/Category" Node and insert into all available Metadata_Category language table
                // Check Category In each "metadata/Category" Node


                for (int Index = 0; Index < RootNodeList.Count; Index++)//reach (XmlElement InputNodeList in RootNodeList.Count)
                {
                    MetadataInfo = new MetadataCategoryInfo();
                    // Set MetadataCategoryInfo Value
                    MetadataInfo.CategoryName = DICommon.RemoveQuotes(Convert.ToString(RootNodeList[Index].Attributes["name"].Value));


                    //-- Get Max Category Order
                    CategoryOrder = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.Metadata_Category.GetMaxMetadataCategoryOrder(metadataTypeText)));
                    MetadataInfo.CategoryOrder = CategoryOrder;
                    MetadataInfo.CategoryType = metadataTypeText;
                    // Add MetadataCategory Into all metdata category language tables
                    this.CheckNCreateMetadataCategory(MetadataInfo);

                }
            }
        }


        #endregion


        /// <summary>
        /// Returns true if given category already exists.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryType"></param>
        /// <param name="currentNid">New Mode: '-1' , Edit mode: actual nid</param>
        /// <returns></returns>
        public bool IsAlreadyExists(string name, string categoryType, int currentNid)
        {
            bool RetVal = false;
            int FoundNid = 0;
            try
            {
                FoundNid = this.GetNidByName(name, categoryType);
                if (FoundNid > 0 && FoundNid != currentNid)
                {
                    RetVal = true;
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }


        #endregion

        #endregion

        #region "-- Static --"

        /// <summary>
        /// return Default Xsl string from Resource
        /// </summary>
        /// <returns></returns>
        public static string GetMetadataXslt()
        {
            string RetVal = string.Empty;
            // Get Xsl string from resource File.
            RetVal = Resource1.XSLTFile2;

            return RetVal;
        }

        #endregion
    }
}
