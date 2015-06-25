using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Xml;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Metadata
{
    public delegate void WrongMetadataFound(MetadataElementType elementType, string name,string GID);    

    public class DI7MetadataConverter
    {
        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;
        private DataTable CategroyTable;

        public event WrongMetadataFound WrongMetdataFoundEvent;
        
        #endregion

        #region "-- New/Dispose --"

        public DI7MetadataConverter(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
            
            // get category table
            this.CategroyTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.None, string.Empty));

        }

        #endregion

        #region "-- Convert Metadata --"

        private void RaiseWrongMetadataFoundEvent(MetadataElementType elementType, string name, string GID)        
        {
            if (this.WrongMetdataFoundEvent != null)
            {
                this.WrongMetdataFoundEvent(elementType, name, GID);
            }
        }               

        private void ConvertIndicatorMetadata()
        {
            DataTable IndicatorTable = null;
            string IndicatorMetadata = string.Empty;
            string IndicatorNid = string.Empty;
            
            // get indicators where metadata exists
            IndicatorTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Indicators.GetIndicator(FilterFieldType.Search, Indicator.IndicatorInfo + " IS NOT NULL AND " + Indicator.IndicatorInfo + " <> '' ", FieldSelection.Heavy));

            foreach (DataRow Row in IndicatorTable.Rows)
            {
                IndicatorMetadata = Convert.ToString(Row[Indicator.IndicatorInfo]);
                IndicatorNid = Convert.ToString(Row[Indicator.IndicatorNId]);

                if (!string.IsNullOrEmpty(IndicatorMetadata))
                {
                    try
                    {
                        this.InsertMetadataIntoMetadataReportTable(IndicatorMetadata, IndicatorNid, MetadataElementType.Indicator);

                        // Update IndicatorInfo(Metadata) to empty
                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Update.UpdateIndicatorInfo(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, string.Empty, FilterFieldType.NId, IndicatorNid));
                    }
                    catch (Exception)
                    {
                        this.RaiseWrongMetadataFoundEvent(MetadataElementType.Indicator, Convert.ToString(Row[Indicator.IndicatorName]), Convert.ToString(Row[Indicator.IndicatorGId]));                        
                    }
                }
            }
        }
        
        private void InsertMetadataIntoMetadataReportTable(string metadataXML, string targetNid, MetadataElementType categoryType)
        {
            string CategoryName = string.Empty;
            string CategroyNid = string.Empty;
            XmlDocument XmlDoc = new XmlDocument();
            string CategoryNid = string.Empty;
            string MetadataText = string.Empty;
            
            try
            {
                // 1. create xml document object using metadata

                //load xml doc if xml text is not null or empty
                if (!string.IsNullOrEmpty(Convert.ToString(metadataXML).Trim()))
                {
                    XmlDoc.LoadXml(metadataXML);

                    // 2. Get Category and Category metadata 
                    foreach (XmlNode CategoryxmlNode in XmlDoc.SelectNodes(DevInfo.Lib.DI_LibBAL.DA.DML.Constants.MetadataCategory.MetadataCategoryNodePath))
                    {
                        if (CategoryxmlNode.Attributes.Count > 0)
                        {
                            CategoryName = CategoryxmlNode.Attributes[0].Value;
                            MetadataText = CategoryxmlNode.InnerText;
                            if (!string.IsNullOrEmpty(MetadataText.Trim()))
                            {

                                // 3. check category exists in Category table or not                    
                                foreach (DataRow CategoryRow in this.CategroyTable.Select(Metadata_Category.CategoryName + " ='" + DICommon.RemoveQuotes(CategoryName) + "' and " + Metadata_Category.CategoryType + " =" + DIQueries.MetadataElementTypeText[categoryType] + " "))
                                {
                                    CategoryNid = Convert.ToString(CategoryRow[Metadata_Category.CategoryNId]);

                                    // 4. If exists, then insert record into MetadataReport table 
                                    if (Convert.ToString(CategoryRow[Metadata_Category.CategoryName]) == CategoryName)
                                    {
                                        // delete old record from metadata report
                                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataReport.Delete.DeleteMetadataReport(this.DBQueries.TablesName.MetadataReport, targetNid, CategoryNid));

                                        // insert record into metadata report table
                                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataReport.Insert.InsertMetadataReport(this.DBQueries.TablesName.MetadataReport, targetNid, CategoryNid, MetadataText));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        private void ConvertICMetadata()
        {
            DataTable ICTable = null;
            string ICInfo = string.Empty;
            string SourceNid = string.Empty;
           
            // Get Source Metadata Table
            ICTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IndicatorClassification.GetICForSearch(FilterFieldType.Search, IndicatorClassifications.ICInfo + " IS NOT NULL AND " + IndicatorClassifications.ICInfo + " <> ''  AND " + IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[ICType.Source], FieldSelection.Heavy));

           
            // Update each Indicator Row where Metadata exists
            foreach (DataRow Row in ICTable.Rows)
            {
                ICInfo = Convert.ToString(Row[IndicatorClassifications.ICInfo]);
                SourceNid = Convert.ToString(Row[IndicatorClassifications.ICNId]);

                if (!string.IsNullOrEmpty(ICInfo))
                {
                    try
                    {
                        this.InsertMetadataIntoMetadataReportTable(ICInfo, SourceNid, MetadataElementType.Source);

                        // Update IndicatorClassificationInfo(Source Metadata)
                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Update.UpdateICInfo(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, string.Empty, ICType.Source, Convert.ToInt32(SourceNid)));
                    }
                    catch (Exception)
                    {                        
                        //raise event
                        this.RaiseWrongMetadataFoundEvent(MetadataElementType.Source, Convert.ToString(Row[IndicatorClassifications.ICName]), Convert.ToString(Row[IndicatorClassifications.ICGId]));                        
                    }
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
                    try
                    {
                        this.InsertMetadataIntoMetadataReportTable(MetadataText, Convert.ToString(Row[Area_Map_Metadata.LayerNId]), MetadataElementType.Area);

                        // Update IndicatorClassificationInfo(Metadata)
                        this.DBConnection.ExecuteNonQuery(this.DBQueries.Update.Area.UpdateAreaMetadataInfo(DICommon.RemoveQuotes(string.Empty), DICommon.RemoveQuotes(Convert.ToString(Row[Area_Map_Metadata.LayerName]))));
                    }
                    catch (Exception)
                    {                        
// rasise event
                        this.RaiseWrongMetadataFoundEvent(MetadataElementType.Area,  Convert.ToString(Row[Area_Map_Metadata.LayerName]),Convert.ToString(Row[Area_Map_Metadata.LayerNId]));                        
                    }
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
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Xslt.Update.UpdateXSLT(this.DBQueries.DataPrefix, DICommon.RemoveQuotes(DefaultXsltText), Convert.ToString(Row[XSLT.XSLTFile])));

            }
        }

        #endregion

        #region "-- Methods --"

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


        #endregion



    }
}
