using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using Microsoft.VisualBasic;

using DevInfo.Lib.DI_LibDAL.Connection;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.IO;
using System.Xml;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Metadata;
using SpreadsheetGear;
using DevInfo.Lib.DI_LibBAL.Controls.MetadataEditorBAL;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{/// <summary>
    /// Provides methods for creating and importing metadata into database.
    /// </summary>
    public class DI7MetaDataBuilder : ProcessEventCreator
    {
        #region "-- Private --"

        #region "-- Variables --"

        protected DIConnection DBConnection;
        protected DIQueries DBQueries;

        private const int ElementSheetIndex = 0;
        private const int ElementGIdRowIndex = 2;
        private const int ElementGIdColumnIndex = 0;
        private const int ElementNameRowIndex = 1;
        private const int ElementNameColumnIndex = 0;

        #endregion

        #region "-- New/Dispose --"
        public DI7MetaDataBuilder(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }

        #endregion

        #region "-- Methods --"


        private string ReplaceInvalidString(string paraText)
        {
            string RetVal = paraText;
            int i = 0;
            int CurrIndex = 0;
            string ConcatenatedParaNodeText = string.Empty;
            string ParaNodeText = string.Empty;
            try
            {

                ParaNodeText = paraText;
                // add para node into concatenated category value
                ParaNodeText = ParaNodeText.TrimStart();
                ParaNodeText = ParaNodeText.TrimEnd();

                //--Replace  - {{~}} special character from paranode text
                ParaNodeText = ParaNodeText.Replace(" - " + DI7MetaDataBuilder.NewLineSeparator, "");


                ParaNodeText = ParaNodeText.Replace(DI7MetaDataBuilder.NewLineSeparator, Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace(" - " + DI7MetaDataBuilder.NewLineSeparator, "");
                ParaNodeText = ParaNodeText.Replace(" " + DI7MetaDataBuilder.NewLineSeparator + " ", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace("  " + DI7MetaDataBuilder.NewLineSeparator + "  ", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace(DI7MetaDataBuilder.NewLineSeparator + " ", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace(" " + DI7MetaDataBuilder.NewLineSeparator, Microsoft.VisualBasic.ControlChars.NewLine);

                //Concatenate ParaNodeText
                ConcatenatedParaNodeText = ConcatenatedParaNodeText + Microsoft.VisualBasic.ControlChars.NewLine + ParaNodeText;
                //Format ConcatenatedParaNodeText
                ConcatenatedParaNodeText.Replace(DI7MetaDataBuilder.NewLineSeparator, Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace(" " + DI7MetaDataBuilder.NewLineSeparator + " ", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace("  " + DI7MetaDataBuilder.NewLineSeparator + "  ", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace(DI7MetaDataBuilder.NewLineSeparator + " ", Microsoft.VisualBasic.ControlChars.NewLine);
                ParaNodeText = ParaNodeText.Replace(" " + DI7MetaDataBuilder.NewLineSeparator, Microsoft.VisualBasic.ControlChars.NewLine);


                RetVal = ParaNodeText;
            }
            catch (Exception ex)
            { }

            return RetVal;
        }

        private string ReplaceNewLineInMetadataReport(string metadataReport)
        {
            return metadataReport.Replace(Microsoft.VisualBasic.ControlChars.NewLine, DI7MetaDataBuilder.NewLineSeparator);
        }

        private Dictionary<string, string> GetMetadataCategoryNameGIDCollection(MetadataElementType mdType)
        {
            Dictionary<string, string> RetVal = new Dictionary<string, string>();

            DataTable Table = this.DBConnection.ExecuteDataTable(this.DBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.None, string.Empty));
            if (mdType == MetadataElementType.Indicator)
            {
                Table.DefaultView.RowFilter = Metadata_Category.CategoryType + "='I'";
            }
            else if (mdType == MetadataElementType.Area)
            {
                Table.DefaultView.RowFilter = Metadata_Category.CategoryType + "='A'";
            }
            else
            {
                Table.DefaultView.RowFilter = Metadata_Category.CategoryType + "='S'";
            }

            Table = Table.DefaultView.ToTable();

            foreach (DataRow Row in Table.Rows)
            {
                if (!RetVal.ContainsKey(Convert.ToString(Row[Metadata_Category.CategoryGId])))
                {
                    RetVal.Add(Convert.ToString(Row[Metadata_Category.CategoryGId]), Convert.ToString(Row[Metadata_Category.CategoryName]));
                }
            }

            return RetVal;
        }

        private void AddNameAttribteInAttributeSet(XmlDocument RetVal, Dictionary<string, string> MDCatGIDandNameColl, XmlNode Node)
        {
            foreach (XmlNode ReportNode in Node.ChildNodes)
            {
                if (ReportNode.Name == ATTRIBUTESET_ELEMENT)
                {
                    foreach (XmlNode ReportAttNode in ReportNode.ChildNodes)
                    {
                        if (MDCatGIDandNameColl.ContainsKey(ReportAttNode.Attributes[ID_ATTRIBUTE].Value))
                        {
                            XmlAttribute NameAtt = RetVal.CreateAttribute(NAME_ATTRIBUTE);
                            NameAtt.Value = MDCatGIDandNameColl[ReportAttNode.Attributes[ID_ATTRIBUTE].Value.ToString()];
                            ReportAttNode.Attributes.Append(NameAtt);
                        }
                        else
                        {
                            XmlAttribute NewAtt = RetVal.CreateAttribute(NAME_ATTRIBUTE);
                            NewAtt.Value = ReportAttNode.Attributes[ID_ATTRIBUTE].Value;
                            ReportAttNode.Attributes.Append(NewAtt);
                        }
                        if (ReportAttNode.ChildNodes.Count > 0)
                        {
                            this.AddNameAttribteInAttributeSet(RetVal, MDCatGIDandNameColl, ReportAttNode);
                        }
                    }
                }
            }
        }

        private bool IsMetadataNameAttributeExists(XmlDocument xmlDoc)
        {
            bool RetVal = false;
            XmlNodeList NodeListObj = null;

            try
            {
                XmlNamespaceManager nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
                nsManager.AddNamespace("generic", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic");
                nsManager.AddNamespace("message", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");
                NodeListObj = xmlDoc.SelectNodes(METADATASET_XPATH, nsManager);

                foreach (XmlNode Node in NodeListObj[0].ChildNodes)
                {
                    if (Node.Name == REPORT_ELEMENT)
                    {
                        foreach (XmlNode ReportNode in Node.ChildNodes)
                        {
                            if (ReportNode.Name == ATTRIBUTESET_ELEMENT)
                            {
                                foreach (XmlNode ReportAttNode in ReportNode.ChildNodes)
                                {
                                    try
                                    {
                                        string CatName = ReportAttNode.Attributes[NAME_ATTRIBUTE].ToString();
                                        RetVal = true;
                                    }
                                    catch (Exception)
                                    {
                                        RetVal = false;
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { }

            return RetVal;
        }

        #endregion

        #endregion

        #region -- Public --

        #region -- Variable/Constants --
        public const string NewLineSeparator = "{{~}}";
        public const string METADATASET_XPATH = "/message:GenericMetadata/message:MetadataSet";
        public const string REPORT_ELEMENT = "Report";
        public const string ATTRIBUTESET_ELEMENT = "AttributeSet";
        public const string ID_ATTRIBUTE = "id";
        public const string NAME_ATTRIBUTE = "name";
        public const string COMMONTEXT_ELEMENT = "common:Text";
        public const string XMLLANG_ATTRIBUTE = "xml:lang";

        #endregion

        #region -- Methods --

        #region -- DML --

        /// <summary> 
        /// Update Metadata text inside database 
        /// </summary> 
        /// <param name="metadataType">Metadata Type. For IC types Sector may be set as code remains for all IC types</param> 
        /// <param name="elementNId">IndicatorNId, LayerNId, ICNId</param> 
        /// <param name="metadata">Metadata content as String</param> 
        /// <returns>Bool value to indicate success or failure</returns> 
        public bool InsertORUpdateMetadataInfo(MetaDataType metadataType, string categoryNid, int elementNId, string metadata)
        {
            bool RetVal = false;

            try
            {
                string SqlQuery = string.Empty;

                switch (metadataType)
                {
                    case MetaDataType.Indicator:
                    case MetaDataType.Map:
                    case MetaDataType.Source:

                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataReport.Delete.DeleteMetadataReport(this.DBQueries.TablesName.MetadataReport, elementNId.ToString(), categoryNid));
                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataReport.Insert.InsertMetadataReport(this.DBQueries.TablesName.MetadataReport, elementNId.ToString(), categoryNid, metadata));
                        break;


                    case MetaDataType.Sector:
                    case MetaDataType.Goal:
                    case MetaDataType.CF:
                    case MetaDataType.Theme:

                    case MetaDataType.Institution:
                    case MetaDataType.Convention:

                        this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Update.UpdateICInfo(this.DBQueries.DataPrefix, this.DBQueries.LanguageCode, metadata, elementNId));
                        break;

                }
                RetVal = true;
            }


            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Imports metadata from the source database into current database
        /// </summary>
        /// <param name="srcConnection"></param>
        /// <param name="srcQueries"></param>
        /// <param name="srcElementNid"></param>
        /// <param name="trgElementNid"></param>
        /// <param name="categoryType"></param>
        /// <param name="metadataType"></param>
        /// <param name="iconType"></param>
        public void ImportMetadata(DIConnection srcConnection, DIQueries srcQueries, int srcElementNid, int trgElementNid, MetadataElementType categoryType, MetaDataType metadataType, IconElementType iconType)
        {
            string SrcCategoryGID = string.Empty;
            string MetadataText = string.Empty;
            int TrgCategoryNid = 0;
            DataTable TrgCategoryTable = null;
            Dictionary<String, String> OldIconNIdnNewIconNId = new Dictionary<string, string>();

            try
            {
                // Get target Category table
                TrgCategoryTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Metadata_Category.GetMetadataCategories(FilterFieldType.Type, DIQueries.MetadataElementTypeText[categoryType]));

                // update metadata icon in DICon table
                OldIconNIdnNewIconNId = DIIcons.ImportElement(srcElementNid, trgElementNid, iconType, srcQueries, srcConnection, this.DBQueries, this.DBConnection);



                //get source metadata reports
                foreach (DataRow SrcRow in srcConnection.ExecuteDataTable(srcQueries.MetadataReport.GetMetadataReportsByTargetNid(srcElementNid.ToString(), categoryType)).Rows)
                {
                    SrcCategoryGID = Convert.ToString(SrcRow[Metadata_Category.CategoryGId]);

                    // check source category GID exists in current database
                    // Import metadta report only if category exists in current database 
                    foreach (DataRow TrgCategoryRow in TrgCategoryTable.Select(Metadata_Category.CategoryGId + "='" + DIQueries.RemoveQuotesForSqlQuery(SrcCategoryGID) + "' "))
                    {
                        MetadataText = Convert.ToString(SrcRow[MetadataReport.Metadata]);

                        // Update IconNids in metadata if exists
                        foreach (string OldIconName in OldIconNIdnNewIconNId.Keys)
                        {
                            MetadataText = MetadataText.Replace(OldIconName, OldIconNIdnNewIconNId[OldIconName].ToString());
                        }

                        TrgCategoryNid = Convert.ToInt32(TrgCategoryRow[Metadata_Category.CategoryNId]);
                        this.InsertORUpdateMetadataInfo(metadataType, TrgCategoryNid.ToString(), trgElementNid, MetadataText);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        public void DeleteMetadataReports(MetadataElementType categoryType)
        {
            DITables TableNames;


            try
            {
                // Step1: Delete records from metadata Report table
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    // Get table name
                    TableNames = new DITables(this.DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());

                    // delete records
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataReport.Delete.DeleteMetadataReportByCategory(TableNames, categoryType));

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        #endregion

        #region -- Import Metadata --

        /// <summary>
        /// Imports indicator metadata from template/database
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        /// <param name="selectedNIDs"></param>
        /// <param name="selectionCount"></param>
        /// <param name="metadataType"></param>
        public void ImportIndicatorMetadata(string dataPrefix, DIConnection sourceDBConnection, DIQueries sourceDBQueries, string selectedNIDs, int selectionCount, MetaDataType metadataType)
        {
            DataTable TempDataTable;
            int CurrentRecordIndex = 0;
            DI7MetadataCategoryBuilder MetadataCategoryBuilderObj;
            IndicatorInfo SourceIndicatorInfo = new IndicatorInfo();
            IndicatorBuilder IndBuilder = null;

            try
            {
                this.RaiseStartProcessEvent();
                IndBuilder = new IndicatorBuilder(this.DBConnection, this.DBQueries);

                // 1. import metadta categories
                // import indicator metadata categories
                MetadataCategoryBuilderObj = new DI7MetadataCategoryBuilder(this.DBConnection, this.DBQueries);
                MetadataCategoryBuilderObj.ImportAllMetadataCategories(sourceDBConnection, sourceDBQueries, MetadataElementType.Indicator);


                // 2. Imort metadata reports
                if (selectionCount == -1)
                {
                    // -- GET ALL 
                    TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Indicators.GetIndicator(FilterFieldType.None, string.Empty, FieldSelection.Heavy));
                }
                else
                {
                    // -- GET SELECTED 
                    TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Indicators.GetIndicator(FilterFieldType.NId, selectedNIDs, FieldSelection.Heavy));
                }


                //////// -- Initialize Progress Bar 
                this.RaiseBeforeProcessEvent(TempDataTable.Rows.Count);
                foreach (DataRow Row in TempDataTable.Rows)
                {
                    CurrentRecordIndex++;

                    // get source indicator info
                    SourceIndicatorInfo = new IndicatorInfo();
                    SourceIndicatorInfo.Nid = Convert.ToInt32(Row[Indicator.IndicatorNId]);
                    SourceIndicatorInfo.GID = Convert.ToString(Row[Indicator.IndicatorGId]);
                    SourceIndicatorInfo.Name = Convert.ToString(Row[Indicator.IndicatorName]);
                    SourceIndicatorInfo.Global = Convert.ToBoolean(Row[Indicator.IndicatorGlobal]);

                    //import metadata
                    IndBuilder.ImportIndicatorMetadata(SourceIndicatorInfo, SourceIndicatorInfo.Nid, sourceDBQueries, sourceDBConnection);

                    // -- Increemnt the Progress Bar Value 
                    this.RaiseProcessInfoEvent(CurrentRecordIndex);

                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            finally
            {
                this.RaiseEndProcessEvent();
            }

        }


        /// <summary>
        /// Imports source metadata from template/database
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        /// <param name="selectedNIDs"></param>
        /// <param name="selectionCount"></param>
        /// <param name="metadataType"></param>
        public void ImportSourceMetadata(string dataPrefix, DIConnection sourceDBConnection, DIQueries sourceDBQueries, string selectedNIDs, int selectionCount, MetaDataType metadataType)
        {
            DataTable TempDataTable;
            ICType ClassificationType;
            int CurrentRecordIndex = 0;
            int SrcElementNid;
            int TrgElementNid;
            DI7MetadataCategoryBuilder SourceMetadataCategoryBuilder;

            SourceBuilder ICBuilder;

            try
            {
                this.RaiseStartProcessEvent();


                ICBuilder = new SourceBuilder(this.DBConnection, this.DBQueries);

                // import source categories
                SourceMetadataCategoryBuilder = new DI7MetadataCategoryBuilder(this.DBConnection, this.DBQueries);
                SourceMetadataCategoryBuilder.ImportAllMetadataCategories(sourceDBConnection, sourceDBQueries, MetadataElementType.Source);


                ClassificationType = ICType.Source;
                if (selectionCount == -1)
                {
                    // -- GET ALL 
                    TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, ClassificationType, FieldSelection.Heavy));
                }
                else
                {
                    // -- GET SELECTED 
                    TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, selectedNIDs, ClassificationType, FieldSelection.Heavy));
                }



                ////// -- Initialize Progress Bar 
                this.RaiseBeforeProcessEvent(TempDataTable.Rows.Count);
                CurrentRecordIndex = 0;

                foreach (DataRow Row in TempDataTable.Rows)
                {
                    CurrentRecordIndex++;
                    SrcElementNid = Convert.ToInt32(Row[IndicatorClassifications.ICNId]);
                    TrgElementNid = ICBuilder.CheckSourceExists(Convert.ToString(Row[IndicatorClassifications.ICName]));

                    // import source metadadta
                    if (TrgElementNid > 0)
                    {
                        this.ImportMetadata(sourceDBConnection, sourceDBQueries, SrcElementNid, TrgElementNid, MetadataElementType.Source, MetaDataType.Source, IconElementType.MetadataSource);

                    }

                    ////// -- Increemnt the Progress Bar Value 
                    this.RaiseProcessInfoEvent(CurrentRecordIndex);

                }
                // -- Dispose the Data Table object 
                TempDataTable.Dispose();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        /// <summary>
        /// Imports area metadata from template/database
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        /// <param name="selectedNIDs"></param>
        /// <param name="selectionCount"></param>
        /// <param name="metadataType"></param>
        public void ImportAreaMetadata(string dataPrefix, DIConnection sourceDBConnection, DIQueries sourceDBQueries, string selectedNIDs, int selectionCount, MetaDataType metadataType)
        {
            DataTable TempDataTable;
            int CurrentRecordIndex = 0;
            DI7MetadataCategoryBuilder AreaMetadataCategoryBuilder;
            DataTable TempTargetTable;

            try
            {
                this.RaiseStartProcessEvent();

                // import area categories
                AreaMetadataCategoryBuilder = new DI7MetadataCategoryBuilder(this.DBConnection, this.DBQueries);
                AreaMetadataCategoryBuilder.ImportAllMetadataCategories(sourceDBConnection, sourceDBQueries, MetadataElementType.Area);


                if (selectionCount == -1)
                {
                    // -- GET ALL 
                    TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetAreaMapMetadata(string.Empty));
                }
                else
                {
                    // -- GET SELECTED 
                    TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Area.GetAreaMapMetadata(selectedNIDs));
                }



                // -- Initialize Progress Bar 
                this.RaiseBeforeProcessEvent(TempDataTable.Rows.Count);
                CurrentRecordIndex = 0;

                //import area metadata
                foreach (DataRow Row in TempDataTable.Rows)
                {

                    CurrentRecordIndex++;
                    //-- Get Target table against LayerName 
                    TempTargetTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Area.GetAreaMapMetadataByName(
                       Row[Area_Map_Metadata.LayerName].ToString()));


                    foreach (DataRow TrgRow in TempTargetTable.Rows)
                    {
                        this.ImportMetadata(sourceDBConnection, sourceDBQueries, Convert.ToInt32(Row[Area_Map_Layer.LayerNId]), Convert.ToInt32(TrgRow[Area_Map_Layer.LayerNId]), MetadataElementType.Area, MetaDataType.Map, IconElementType.MetadataArea);
                        break;
                    }

                    // -- Increemnt the Progress Bar Value 
                    this.RaiseProcessInfoEvent(CurrentRecordIndex);

                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            finally
            {
                this.RaiseEndProcessEvent();
            }
        }


        /// <summary>
        /// To Import metadata information 
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        /// <param name="selectedNIDs"></param>
        /// <param name="selectionCount"></param>
        /// <param name="metadataType"> </param>
        public void ImportICMetadata(string dataPrefix, DIConnection sourceDBConnection, DIQueries sourceDBQueries, string selectedNIDs, int selectionCount, MetaDataType metadataType, ICType classificationType)
        {

            string MetadataInfo = string.Empty;
            DataTable TempDataTable;
            DataTable TempTargetIdTable = new DataTable();
            IndicatorClassificationBuilder ICBuilder;
            int TargetICNid = 0;

            int CurrentRecordIndex = 0;
            this.RaiseStartProcessEvent();

            try
            {
                ICBuilder = new IndicatorClassificationBuilder(this.DBConnection, this.DBQueries);

                if (selectionCount == -1)
                {
                    // -- GET ALL 
                    TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.None, string.Empty, classificationType, FieldSelection.Heavy));
                }
                else
                {
                    // -- GET SELECTED 
                    TempDataTable = sourceDBConnection.ExecuteDataTable(sourceDBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, selectedNIDs, classificationType, FieldSelection.Heavy));
                }



                // -- Initialize Progress Bar 
                this.RaiseBeforeProcessEvent(TempDataTable.Rows.Count);
                CurrentRecordIndex = 0;

                for (int Index = 0; Index <= TempDataTable.Rows.Count - 1; Index++)
                {
                    CurrentRecordIndex++;

                    if (!Information.IsDBNull(TempDataTable.Rows[Index][IndicatorClassifications.ICInfo]))
                    {
                        if (!(TempDataTable.Rows[Index][IndicatorClassifications.ICInfo].ToString().Length == 0))
                        {

                            //-- Retrieve Metadata Information 
                            MetadataInfo = TempDataTable.Rows[Index][IndicatorClassifications.ICInfo].ToString();

                            // Get Target IC NID by Source IC GID
                            TargetICNid = ICBuilder.GetNidByGID(Convert.ToString(TempDataTable.Rows[Index][IndicatorClassifications.ICGId]), classificationType);

                            if (TargetICNid > 0)
                            {
                                //-- Update Metadata 
                                ICBuilder.UpdateICInfo(TargetICNid, MetadataInfo);
                            }
                        }
                    }

                    // -- Increemnt the Progress Bar Value 
                    this.RaiseProcessInfoEvent(CurrentRecordIndex);
                }

                // -- Dispose the Data Table object 
                TempDataTable.Dispose();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }



        }



        /// <summary>
        /// To import metadata from RTF file for all indicator classifications except source
        /// </summary>
        /// /// <param name="metadataInfo"></param>
        /// <param name="metaDataType"></param>
        /// <param name="elementNid"></param>        
        public void ImportMetadataFromRTFFile(string metadataInfo, MetaDataType metadataType, int elementNId)
        {
            IndicatorClassificationBuilder ICBuilder = new IndicatorClassificationBuilder(this.DBConnection, this.DBQueries);

            try
            {
                ICBuilder.UpdateICInfo(elementNId, metadataInfo);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }

        #region -- Import from Excel --

        public int GetElementNidByGID(string elementGID, MetaDataType metadataType)
        {
            int RetVal = 0;
            string Query = string.Empty;
            string NIDColumnName = string.Empty;

            try
            {
                elementGID = DIQueries.RemoveQuotesForSqlQuery(elementGID);

                switch (metadataType)
                {
                    case MetaDataType.Indicator:
                        Query = this.DBQueries.Indicators.GetIndicator(FilterFieldType.GId, "'" + elementGID + "'", FieldSelection.NId);
                        NIDColumnName = Indicator.IndicatorNId;
                        break;

                    case MetaDataType.Map:
                        Query = this.DBQueries.Area.GetAreaMapLayer(DevInfo.Lib.DI_LibDAL.Queries.Area.Select.MapFilterFieldType.LayerName, "'" + elementGID + "'", FieldSelection.NId);
                        NIDColumnName = Area_Map_Layer.LayerNId;
                        break;

                    case MetaDataType.Source:
                        Query = this.DBQueries.Source.GetSource(FilterFieldType.Name, "'" + elementGID + "'", FieldSelection.NId, false);
                        NIDColumnName = IndicatorClassifications.ICNId;
                        break;
                    //case MetaDataType.Sector:
                    //    break;
                    //case MetaDataType.Goal:
                    //    break;
                    //case MetaDataType.CF:
                    //    break;
                    //case MetaDataType.Theme:
                    //    break;
                    //case MetaDataType.Institution:
                    //    break;
                    //case MetaDataType.Convention:
                    //    break;
                    //case MetaDataType.IndicatorClassification:
                    //    break;
                    default:
                        break;
                }

                if (!string.IsNullOrEmpty(Query))
                {
                    foreach (DataRow Row in this.DBConnection.ExecuteDataTable(Query).Rows)
                    {
                        RetVal = Convert.ToInt32(Row[NIDColumnName]);
                        break;
                    }

                }

            }
            catch (Exception)
            {
                RetVal = 0;
            }
            return RetVal;
        }


        public bool ImportMetataFromExcel(MetadataElementType categoryType, MetaDataType metadataType, int elementNId, string xlsFileNameWPath, string xsltFldrPath)
        {
            bool RetVal = true;
            string MetadataText = string.Empty;
            string FirstColumnValue = string.Empty;
            string SecondColumnValue = string.Empty;
            string ThirdColumnValue = string.Empty;
            string ElementName = string.Empty;
            string ElementGID = string.Empty;
            DataTable ExcelDataTable = null;
            DataRow Row;

            DI7MetaDataBuilder MDBuilder;
            DI7MetadataCategoryBuilder MDCategoryBuilder;
            DI7MetadataCategoryInfo MetadataCategory;

            DIExcel ExcelFile = null;

            try
            {
                // -- Get data table from excel file
                ExcelFile = new DIExcel(xlsFileNameWPath);
                ExcelDataTable = ExcelFile.GetDataTableFromSheet(ExcelFile.GetSheetName(0));

                // -- create database builder objects
                MDCategoryBuilder = new DI7MetadataCategoryBuilder(this.DBConnection, this.DBQueries);
                MDBuilder = new DI7MetaDataBuilder(this.DBConnection, this.DBQueries);

                // -- import metadata reports with category
                for (int RowIndex = 1; RowIndex < ExcelDataTable.Rows.Count; RowIndex++)
                {
                    Row = ExcelDataTable.Rows[RowIndex];
                    FirstColumnValue = Convert.ToString(Row[0]);
                    SecondColumnValue = Convert.ToString(Row[1]);
                    ThirdColumnValue = Convert.ToString(Row[2]);

                    // get element name
                    if (string.IsNullOrEmpty(ElementName))
                    {
                        if (string.IsNullOrEmpty(FirstColumnValue))
                        {
                            break;
                        }
                        else
                        {
                            ElementName = FirstColumnValue;
                            continue;
                        }
                    }

                    // get element gid/id
                    if (string.IsNullOrEmpty(ElementGID))
                    {
                        if (string.IsNullOrEmpty(FirstColumnValue))
                        {
                            break;
                        }
                        else
                        {
                            ElementGID = FirstColumnValue;

                            // get element nid by element gid
                            if (metadataType == MetaDataType.Source)
                            {
                                elementNId = this.GetElementNidByGID(ElementName, metadataType);
                            }
                            else
                            {
                                elementNId = this.GetElementNidByGID(ElementGID, metadataType);
                            }

                            // Skip title row by incrementing  row index 
                            RowIndex++;

                            continue;
                        }
                    }

                    // continue if row is blank
                    if (string.IsNullOrEmpty(FirstColumnValue) && string.IsNullOrEmpty(SecondColumnValue) && string.IsNullOrEmpty(ThirdColumnValue))
                    {
                        // reset element  value
                        elementNId = 0;
                        ElementName = string.Empty;
                        ElementGID = string.Empty;
                        continue;
                    }
                    else if (elementNId > 0)
                    {
                        // import metadata report with metadata category

                        // get metadata category and metedata report 
                        MetadataCategory = new DI7MetadataCategoryInfo();
                        MetadataCategory.CategoryName = SecondColumnValue;
                        MetadataCategory.CategoryGID = FirstColumnValue;
                        MetadataCategory.CategoryType = DIQueries.MetadataElementTypeText[categoryType];

                        // import metadata category
                        MetadataCategory.CategoryNId = MDCategoryBuilder.CheckNInsertCategory(MetadataCategory);

                        // import metadata report
                        if (MetadataCategory.CategoryNId > 0)
                        {
                            MDBuilder.InsertORUpdateMetadataInfo(metadataType, MetadataCategory.CategoryNId.ToString(), elementNId, this.ReplaceNewLineInMetadataReport(ThirdColumnValue));
                        }
                    }

                }
            }
            catch (Exception)
            {
                RetVal = false;
            }
            finally
            {
                if (ExcelFile != null)
                {
                    ExcelFile.Close();
                }
            }

            return RetVal;
        }


        #endregion

        /// <summary>
        /// Import Metdata report
        /// </summary>
        /// <param name="xmlFileNameWPath"></param>
        /// <param name="mdType"></param>
        /// <param name="selectedNidInTrgDatabase"></param>
        /// <param name="selectedFiles"></param>
        /// <param name="XsltFolderPath"></param>
        public void ImportMetadataFromXML(string xmlString, MetadataElementType metadataElementType, int selectedNidInTrgDatabase, string XsltFolderPath)
        {
            XmlDocument XmlDoc = new XmlDocument();
            DI7MetadataCategoryBuilder MDBuilder = new DI7MetadataCategoryBuilder(this.DBConnection, this.DBQueries);

            XmlDoc.LoadXml(xmlString);
            try
            {
                //--  check that Value/Name attribute exists or not
                if (this.IsMetadataNameAttributeExists(XmlDoc))
                {

                    this.CheckNInsertMetadataReportXml(XmlDoc, selectedNidInTrgDatabase, true, DIQueries.MetadataElementTypeText[metadataElementType]);
                }
                else
                {
                    // if not then use ID for mapping .
                    // If category ID exists in trg database then only import metadata report
                    this.CheckNInsertMetadataReportXml(XmlDoc, selectedNidInTrgDatabase, false, DIQueries.MetadataElementTypeText[metadataElementType]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void CheckNInsertMetadataReportXml(XmlDocument XmlDoc, int targetNid, bool isNameAttributeExist, string metadataTypeText)
        {
            XmlNamespaceManager nsManager = new XmlNamespaceManager(XmlDoc.NameTable);
            DI7MetadataCategoryBuilder MDBuilder = new DI7MetadataCategoryBuilder(this.DBConnection, this.DBQueries);

            nsManager.AddNamespace("generic", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic");
            nsManager.AddNamespace("message", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");
            XmlNodeList NodeListObj = null;


            try
            {
                //-- this code not wotk due to "name" attribute 
                //SDMXObjectModel.Message.GenericMetadataType Obj = new SDMXObjectModel.Message.GenericMetadataType();
                //Obj = (SDMXObjectModel.Message.GenericMetadataType)SDMXObjectModel.Deserializer.LoadFromXmlDocument(typeof(SDMXObjectModel.Message.GenericMetadataType), XmlDoc);

                NodeListObj = XmlDoc.SelectNodes(METADATASET_XPATH, nsManager);
                foreach (XmlNode Node in NodeListObj[0].ChildNodes)
                {
                    if (Node.Name == REPORT_ELEMENT)
                    {
                        foreach (XmlNode ReportNode in Node.ChildNodes)
                        {
                            if (ReportNode.Name == ATTRIBUTESET_ELEMENT)
                            {
                                this.InsertMetadataReportFromXML(targetNid, isNameAttributeExist, MDBuilder, ReportNode, metadataTypeText, -1);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void InsertMetadataReportFromXML(int targetNid, bool isNameAttributeExist, DI7MetadataCategoryBuilder MDBuilder, XmlNode ReportNode, string metadataTypeText, int categoryParentNId)
        {
            string MDCategotyID = string.Empty;
            string MDCategotyName = string.Empty;
            DI7MetadataCategoryInfo MDCateInfo = null;
            int CategotyNid = -1;

            foreach (XmlNode ReportAttNode in ReportNode.ChildNodes)
            {
                if (ReportAttNode.Name == ATTRIBUTESET_ELEMENT)
                {
                    this.InsertMetadataReportFromXML(targetNid, isNameAttributeExist, MDBuilder, ReportAttNode, metadataTypeText, categoryParentNId);
                }

                if (ReportAttNode.Name == "ReportedAttribute")
                {

                    MDCategotyID = Convert.ToString(ReportAttNode.Attributes[ID_ATTRIBUTE].Value);

                    if (isNameAttributeExist)
                    {
                        MDCategotyName = Convert.ToString(ReportAttNode.Attributes[NAME_ATTRIBUTE].Value);
                    }
                    else
                    {
                        MDCategotyName = MDCategotyID;
                    }

                    try
                    {
                        if (!string.IsNullOrEmpty(ReportAttNode.InnerXml) && Convert.ToString(ReportAttNode[COMMONTEXT_ELEMENT].Attributes[XMLLANG_ATTRIBUTE].Value).Trim('_') == this.DBQueries.LanguageCode.Trim('_'))
                        {
                            string MetadataReport = Convert.ToString(ReportAttNode[COMMONTEXT_ELEMENT].InnerText);

                            if (!string.IsNullOrEmpty(MetadataReport))
                            {
                                MDCateInfo = new DI7MetadataCategoryInfo();
                                MDCateInfo.CategoryGID = MDCategotyID;
                                MDCateInfo.CategoryName = MDCategotyName;
                                MDCateInfo.CategoryType = metadataTypeText;
                                MDCateInfo.ParentNid = categoryParentNId;

                                CategotyNid = MDBuilder.CheckNInsertCategory(MDCateInfo);
                                //-- 
                                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataReport.Insert.InsertMetadataReport(this.DBQueries.TablesName.MetadataReport, targetNid.ToString(), CategotyNid.ToString(), MetadataReport));

                                if (ReportAttNode.ChildNodes.Count > 0)
                                {
                                    this.InsertMetadataReportFromXML(targetNid, isNameAttributeExist, MDBuilder, ReportAttNode, metadataTypeText, CategotyNid);
                                }
                            }
                            else
                            {
                                //this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataReport.Delete.DeleteMetadataReport(this.DBQueries.TablesName.MetadataReport, targetNid.ToString(), CategotyNid.ToString()));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }

        }


        #endregion

        #region "-- Export --"

        public XmlDocument GetMetadataReportWCategoryName(string mdXml, MetadataElementType mdType)
        {
            XmlDocument RetVal = new XmlDocument();
            RetVal.XmlResolver = null;
            RetVal.LoadXml(mdXml);
            Dictionary<string, string> MDCatGIDandNameColl = null;

            try
            {
                XmlNamespaceManager nsManager = new XmlNamespaceManager(RetVal.NameTable);
                nsManager.AddNamespace("generic", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/data/generic");
                nsManager.AddNamespace("message", "http://www.sdmx.org/resources/sdmxml/schemas/v2_1/message");

                //-- Get Metadata Name and CategoryID
                MDCatGIDandNameColl = this.GetMetadataCategoryNameGIDCollection(mdType);

                XmlNodeList NodeListObj = RetVal.SelectNodes(METADATASET_XPATH, nsManager);

                foreach (XmlNode Node in NodeListObj[0].ChildNodes)
                {
                    if (Node.Name == REPORT_ELEMENT)
                    {
                        this.AddNameAttribteInAttributeSet(RetVal, MDCatGIDandNameColl, Node);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }


        #endregion

        #endregion



        #endregion

    }
}




