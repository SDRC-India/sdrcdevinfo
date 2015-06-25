using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.DA.DML;

using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Xml;
using DevInfo.Lib.DI_LibBAL.Controls.MetadataEditorBAL;

namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{
    public class DI7ToDI6Converter : DBConverter
    {
        #region "-- private --"

        #region "-- Variables --"

        List<string> AllLanguageCodes = new List<string>();

        #endregion

        #region "-- Methods --"

        private bool UpdateDBSchema(bool forOnlineDB)
        {
            bool RetVal = false;
            DITables TablesName = this.DBQueries.TablesName;
            string dataType = string.Empty;

            try
            {
                //Step 1 Merge datavalues into single column data_value(memo)
                this.RaiseProcessInfoEvent(3);
                this.MergeDataValueIntoSingleColumn();

                //Step 2 Convert DI7 metadata into DI6 metadata [Info] column update
                this.RaiseProcessInfoEvent(4);
                this.ConvertDI7MetadataIntoDI6Metadata();

                //Step 3 Remove extra columns and table [ut_sdmx ,ut_metadata report]
                this.RaiseProcessInfoEvent(5);
                this.RemoveExtraColumnsNTable();

                //Step 4 Update xslt into ut_xslt add BAL resource XSLT_DI6
                this.RaiseProcessInfoEvent(6);
                this.UpdateXsltIntoXsltTable();

                //Step 5 Rename icius table into ut_Indicator_Classifications_IUS
                this.RaiseProcessInfoEvent(7);
                this.RenameIndicatorClassificationIUSTable(false, TablesName, DIServerType.MsAccess);

                //Step 5 Compact database but don't call SeparateTextualandNemericData method
                this.RaiseProcessInfoEvent(8);
               // this.CompactDatabase();

                RetVal = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RetVal;
        }

        private void SetAllLanguageCodes()
        {
            foreach (DataRow row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
            {
                AllLanguageCodes.Add(Convert.ToString(row[Language.LanguageCode]));
            }
        }

        private void RemoveExtraColumnsNTable()
        {
            DIQueries queries;
            foreach (string LanguageCode in AllLanguageCodes)
            {
                queries = new DIQueries(this.DBQueries.DataPrefix, LanguageCode);

                //Remove IC Columns
                this.DBConnection.DropIndividualColumnOfTable(queries.TablesName.IndicatorClassifications, IndicatorClassifications.ISBN);
                this.DBConnection.DropIndividualColumnOfTable(queries.TablesName.IndicatorClassifications, IndicatorClassifications.Nature);

                //Remove area extra columns
                this.DBConnection.DropIndividualColumnOfTable(queries.TablesName.Area, Area.AreaShortName);

                //Remove Metadata Category extra columns
                this.DBConnection.DropIndividualColumnOfTable(queries.TablesName.MetadataCategory, Metadata_Category.ParentCategoryNId);
                this.DBConnection.DropIndividualColumnOfTable(queries.TablesName.MetadataCategory, Metadata_Category.CategoryGId);
                this.DBConnection.DropIndividualColumnOfTable(queries.TablesName.MetadataCategory, Metadata_Category.CategoryDescription);
                this.DBConnection.DropIndividualColumnOfTable(queries.TablesName.MetadataCategory, Metadata_Category.IsPresentational);
                this.DBConnection.DropIndividualColumnOfTable(queries.TablesName.MetadataCategory, Metadata_Category.IsMandatory);

                //Drop Metadata Report Table
                this._DBConnection.DropTable(queries.TablesName.MetadataReport);

                //Drop sdmx user table
                this._DBConnection.DropTable(this.DBQueries.TablesName.SDMXUser);
            }

            //Remove Timeperiod Columns
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.TimePeriod, Timeperiods.StartDate);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.TimePeriod, Timeperiods.EndDate);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.TimePeriod, Timeperiods.Periodicity);

            //Remove IUS columns
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.IndicatorUnitSubgroup, Indicator_Unit_Subgroup.IsDefaultSubgroup);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.IndicatorUnitSubgroup, Indicator_Unit_Subgroup.AvlMinDataValue);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.IndicatorUnitSubgroup, Indicator_Unit_Subgroup.AvlMaxDataValue);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.IndicatorUnitSubgroup, Indicator_Unit_Subgroup.AvlMinTimePeriod);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.IndicatorUnitSubgroup, Indicator_Unit_Subgroup.AvlMaxTimePeriod);

            //Remove Data columns
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, DevInfo.Lib.DI_LibBAL.DA.DML.Constants.Data.Orginal_Textual_Data_valueColumn);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, DevInfo.Lib.DI_LibBAL.DA.DML.Constants.Data.Orginal_Data_valueColumn);
            
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, Data.IsTextualData);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, Data.IsMRD);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, Data.IsPlannedValue);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, Data.IUNId);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, Data.ConfidenceIntervalLower);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, Data.ConfidenceIntervalUpper);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, Data.MultipleSource);
        }

        private void MergeDataValueIntoSingleColumn()
        {
            //Step 1 Update data_value column as memo in msaccess and in sql database nvarchar(4000)
            DIDataValueHelper.MergeTextualandNumericDataValueColumn(this._DBConnection, this._DBQueries);
        }

        private void UpdateXsltIntoXsltTable()
        {
            string SqlQuery = string.Empty;
            SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Xslt.Update.UpdateXSLT(this.DBQueries.DataPrefix, DICommon.RemoveQuotes(DI_LibBAL.Resource1.XSLT_DI6));


            try
            {
                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
            
        }

        private void CompactDatabase()
        {
            DIDatabase.CompactDataBase(ref this._DBConnection, this._DBQueries, this._FilenameWPath, true, false);
        }

        private void RenameIndicatorClassificationIUSTable(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {
            try
            {
                if (DICommon.ISColumnExistInTable(this.DBConnection, " top 1 * ", this._DBQueries.DataPrefix + "ic_ius"))
                {
                    //-- Carate New IC_IUS table from existing table
                    this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.CreateOldICIUSTableFromExisting(this._DBQueries.DataPrefix));

                    //-- Delete new table
                    this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Delete.DeleteNewICIUSTable(this._DBQueries.DataPrefix));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool AlterColumn(string columnName, string tableName, string dataType)
        {
            bool RetVal = false;
            string SqlQuery = string.Empty;

            SqlQuery = "ALTER TABLE " + tableName + " ALTER COLUMN " + columnName + " " + dataType;

            try
            {
                this._DBConnection.ExecuteDataTable(SqlQuery);

                RetVal = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RetVal;
        }

        #region "-- Metadata --"

        private void ConvertDI7MetadataIntoDI6Metadata()
        {
            MetadataCategoryBuilder metadataCategoryBuilder;
            DIQueries queries;

            foreach (string LanguageCode in AllLanguageCodes)
            {
                queries = new DIQueries(this.DBQueries.DataPrefix, LanguageCode);
                metadataCategoryBuilder = new MetadataCategoryBuilder(this._DBConnection, queries);

                //for Indicator, Area, Source
                this.UpdateMetadataInfoByCategoryType(MetadataElementType.Indicator, metadataCategoryBuilder, queries);

                this.UpdateMetadataInfoByCategoryType(MetadataElementType.Area, metadataCategoryBuilder, queries);

                this.UpdateMetadataInfoByCategoryType(MetadataElementType.Source, metadataCategoryBuilder, queries);
            }
        }

        private void UpdateMetadataInfoByCategoryType(MetadataElementType categoryType, MetadataCategoryBuilder metadataCategoryBuilder, DIQueries queries)
        {
            List<string> TargetNids;
            DataTable SrcCategoryTable = null;
            StringBuilder Metadata_Category_Info = new StringBuilder();

            SrcCategoryTable = this._DBConnection.ExecuteDataTable(queries.MetadataReport.GetAllMetadataReportsByCategoryType(categoryType));
            TargetNids = DICommon.GetCommaSeperatedListOfGivenColumn(SrcCategoryTable, MetadataReport.TargetNid, false, string.Empty);

            //for every target like for indicator one there is many metadata category may be definition, Classification, Method of Computation
            foreach (string targetNid in TargetNids)
            {
                Metadata_Category_Info.Remove(0, Metadata_Category_Info.Length);

                //Step 1 Create metadata Info in xml format
                this.CreateMetadataXML(SrcCategoryTable.Select(MetadataReport.TargetNid + "='" + targetNid + "'"), ref Metadata_Category_Info, metadataCategoryBuilder, DIQueries.MetadataElementTypeText[categoryType].Trim("'".ToCharArray()));

                //Step 2 Update metadata Info in xml format
                this.UpdateMetadataInfo(queries, targetNid, categoryType, Metadata_Category_Info.ToString());
            }

        }

        private void UpdateMetadataInfo(DIQueries queries, string targetNid, MetadataElementType categoryType, String metadataCategoryInfo)
        {
            switch (categoryType)
            {
                case MetadataElementType.Indicator:
                    this._DBConnection.ExecuteNonQuery("update " + queries.TablesName.Indicator + " set " + Indicator.IndicatorInfo + "='" + DIQueries.RemoveQuotesForSqlQuery(metadataCategoryInfo) + "'where " + Indicator.IndicatorNId + "=" + targetNid);
                    break;
                case MetadataElementType.Area:
                    this._DBConnection.ExecuteNonQuery("update " + queries.TablesName.AreaMapMetadata + " set " + Area_Map_Metadata.MetadataText + "='" + DIQueries.RemoveQuotesForSqlQuery(metadataCategoryInfo) + "' where " + Area_Map_Layer.LayerNId + "=" + targetNid);
                    break;
                case MetadataElementType.Source:
                    this._DBConnection.ExecuteNonQuery("update " + queries.TablesName.IndicatorClassifications + " set " + IndicatorClassifications.ICInfo + "='" + DIQueries.RemoveQuotesForSqlQuery(metadataCategoryInfo) + "'where " + IndicatorClassifications.ICNId + "=" + targetNid);
                    break;
                default:
                    break;
            }
        }

        private void CreateMetadataXML(DataRow[] metadataCategoryRows, ref StringBuilder xmlInfo, MetadataCategoryBuilder metadataCategoryBuilder, string categoryType)
        {

            string MetadataCategoryName = string.Empty;
            try
            {
                xmlInfo = new StringBuilder();
                xmlInfo.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                xmlInfo.Append("<metadata>");
                foreach (DataRow metadataCategoryRow in metadataCategoryRows)
                {
                    MetadataCategoryName = Convert.ToString(metadataCategoryRow[Metadata_Category.CategoryName]);

                    //Step 1 Check and create metadata category 
                    if (!metadataCategoryBuilder.IsAlreadyExists(MetadataCategoryName, categoryType, -1))
                    {
                        MetadataCategoryInfo metadataCategoryInfo = new MetadataCategoryInfo();
                        metadataCategoryInfo.CategoryName = MetadataCategoryName;
                        metadataCategoryInfo.CategoryType = categoryType;

                        metadataCategoryBuilder.CheckNCreateMetadataCategory(metadataCategoryInfo);
                    }

                    //Step 2 Add category into metadata xml
                    xmlInfo.Append("<Category name=\"" + MetadataCategoryName + "\">");
                    xmlInfo.Append("<para>" + Convert.ToString(metadataCategoryRow[MetadataReport.Metadata]) + "</para></Category>");
                }

                xmlInfo.Append("</metadata>");
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #endregion

        #endregion

        #region "-- public --"

        #region "-- new/dispose --"

        public DI7ToDI6Converter(DIConnection dbConnection, DIQueries dbQueries)
            : base(dbConnection, dbQueries)
        {
            //do nothing
        }

        #endregion

        #region "-- Methods --"

        public override bool IsValidDB(bool forOnlineDB)
        {
            bool RetVal = false;

            try
            {
                // check DI7_0_0_1 version exists in dbVersion table
                if (this._DBConnection.ExecuteDataTable(this._DBQueries.DBVersion.GetRecords(Constants.Versions.DI7_0_0_1)).Rows.Count > 0)
                {
                    RetVal = true;
                }
            }
            catch (Exception)
            {
            }

            if (!RetVal)
            {
                // set file post fix if given file is not in latest format
                this._DBFilePostfix = Constants.DBFilePostFix.DI7_0_0_0;
            }

            return RetVal;
        }

        public override bool DoConversion(bool forOnlineDB)
        {
            bool RetVal = false;
            int TotalSteps = 10;
            DBVersionBuilder VersionBuilder;

            // Do the conversion only if database has different Schema
            try
            {
                if (DICommon.IsDI7Database(this.DBConnection.ConnectionStringParameters.DbName))
                {
                    this._DBQueries = new DIQueries(this._DBQueries.DataPrefix, this._DBQueries.LanguageCode);

                    if (this._ConvertDatabase)
                    {
                        this.RaiseProcessStartedEvent(TotalSteps);

                        this.RaiseProcessInfoEvent(1);

                        this.SetAllLanguageCodes();

                        this.RaiseProcessInfoEvent(2);
                        if (this.UpdateDBSchema(forOnlineDB))
                        {
                            this.RaiseProcessInfoEvent(9);

                            // Insert version info into database after conversion
                            VersionBuilder = new DBVersionBuilder(this._DBConnection, this._DBQueries);

                            //Delete newer version from table keep 6.0.0.5 record and delete which greater than 6.0.0.5 as like 7 version
                            VersionBuilder.DeleteVersionsFromVersionNumberToEnd(Constants.Versions.DI6_0_0_5);

                            this.RaiseProcessInfoEvent(10);

                            RetVal = true;
                        }
                    }
                }
                else
                {
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        public void Dispose()
        {
            base.Dispose();
        }

        #endregion

        #endregion
    }
}
