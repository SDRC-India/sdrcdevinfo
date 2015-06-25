using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Metadata;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{

    /// <summary>
    /// Helps in converting DevInfo database into DevInfo 6_0_0_5 format
    /// </summary>
    public class DI7_0_0_0DBConverter : DI6_0_0_5DBConverter
    {

        #region "-- private --"

        #region "-- Variables --"


        #endregion

        #region "-- Methods --"

        private void AddMissingMetadataCategoryTables(string tableName)
        {
            if (!this._MissingMetadataCategoryTables.Contains(tableName))
            {
                this._MissingMetadataCategoryTables.Add(tableName);
            }
        }

        private bool ISColumnExist(string columnName, string tableName)
        {
            bool RetVal = true;
            string SqlQuery = string.Empty;

            SqlQuery = "SELECT " + columnName + " FROM " + tableName;

            try
            {
                this._DBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;

        }


        #region "-- DBSchema changes to gain performance --"

        private void UpdateDBSchema(bool forOnlineDB)
        {
            string DataPrefix = string.Empty;
            string LanguageCode = string.Empty;
            DITables TablesName = this.DBQueries.TablesName;
            DIServerType ServerType;

            try
            {
                ServerType = this.DBConnection.ConnectionStringParameters.ServerType;

                // step1: add columns in all dataset
                foreach (DataRow DataPrefixRow in this._DBConnection.DIDataSets().Rows)
                {
                    DataPrefix = DataPrefixRow[DBAvailableDatabases.AvlDBPrefix].ToString() + "_";

                    // add columns in all available langauges
                    foreach (DataRow LanguageRow in this._DBConnection.DILanguages(DataPrefix).Rows)
                    {
                        LanguageCode = "_" + LanguageRow[Language.LanguageCode].ToString();

                        // create tables name
                        TablesName = new DITables(this.DBQueries.DataPrefix, LanguageCode);

                        // insert new columns into indicator_classification table
                        this.AddColumnsIntoIndicatorClassificationsTable(forOnlineDB, TablesName, ServerType);

                        // insert new columns into indicator table
                        this.AddColumnsIntoMetadataCategoryTable(forOnlineDB, TablesName, ServerType);

                        // insert new column "AreaShortName" into area table 
                        this.AddColumnIntoAreaTable(forOnlineDB, TablesName, ServerType);

                        if (!this.ISColumnExist("*", TablesName.MetadataReport))
                        {
                            //-- Create Metedata Report table
                            this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataReport.Insert.CreateTable(TablesName.MetadataReport, forOnlineDB, ServerType));
                        }

                    }
                    this.RaiseProcessInfoEvent(2);

                    // add columns into StartDate,EndDate,Perodicity into Timeperiod table 
                    this.AddColumnIntoTimeperiodTable(forOnlineDB, TablesName, ServerType);

                    this.RaiseProcessInfoEvent(3);

                    // insert new column IsDefaultSubgroup,AvlMinDataValue,AvlMaxDataValue,AvlMinTimePeriod,AvlMaxTimePeriod into IUS table
                    this.AddColumnsIntoIUSTable(forOnlineDB, TablesName, ServerType);

                    //-- Update Default Subgroup
                    this.UpdateDefaultSubgroupValue();

                    this.RaiseProcessInfoEvent(4);

                    // insert new column Textual_Data_Value,IsTextualData,IsMRD,IsPlannedValue,IUNId,ConfidenceIntervalUpper,
                    // ConfidenceIntervalLower into data table
                    this.AddColumnIntoDataTable(forOnlineDB, TablesName, ServerType);

                    this.RaiseProcessInfoEvent(5);

                    //update parent nid and category gid into metadata category table
                    this.UpdateCategoryTable();
                    // update metadata and xslt
                    this.ConvertMetadataXmlIntoDatabse();

                    this.RaiseProcessInfoEvent(6);
                    // Update Textual And Numeric Data
                    this.UpdateTextualAndNumericData(forOnlineDB, TablesName, ServerType);

                    this.RaiseProcessInfoEvent(7);
                    //-- Rename IC_IUS table
                    this.RenameIndicatorClassificationIUSTable(forOnlineDB, TablesName, ServerType);

                    this.RaiseProcessInfoEvent(8);

                    this.CreateSDMXUserTable(forOnlineDB);
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void CreateSDMXUserTable(bool forOnlineDB)
        {
            try
            {
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.SDMXUser.Insert.CreateTable(this.DBQueries.TablesName.SDMXUser, forOnlineDB, this.DBConnection.ConnectionStringParameters.ServerType));

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }

        private void RenameIndicatorClassificationIUSTable(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {
            try
            {
                if (!this.ISColumnExist(" * ", tablesName.IndicatorClassificationsIUS))
                {
                    //-- Carate New IC_IUS table from existing table
                    this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.CreateNewICIUSTableFromExisting(this._DBQueries.DataPrefix));

                    //-- Delete old table
                    this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Delete.DeleteOldICIUSTable(this._DBQueries.DataPrefix));
                }


            }
            catch (Exception ex)
            {
            }


        }


        private void UpdateTextualAndNumericData(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {
            string SqlQuery = string.Empty;
            const string DataValueTempColumn = "Data_Value_Temp";
            try
            {
                //-- Create Temp Column 
                SqlQuery = "ALTER TABLE " + this.DBQueries.TablesName.Data + " ADD COLUMN " + DataValueTempColumn + " Double";
                this.DBConnection.ExecuteNonQuery(SqlQuery);

                SqlQuery = "UPDATE  " + this.DBQueries.TablesName.Data + " SET " + Data.IsTextualData + "=1";
                this.DBConnection.ExecuteNonQuery(SqlQuery);

                SqlQuery = "UPDATE  " + this.DBQueries.TablesName.Data + " SET " + Data.IsTextualData + "=0 WHERE ISNUMERIC(" + Data.DataValue + ")";
                this.DBConnection.ExecuteNonQuery(SqlQuery);

                SqlQuery = "UPDATE  " + this.DBQueries.TablesName.Data + " SET " + Data.TextualDataValue + "=" + Data.DataValue + " WHERE " + Data.IsTextualData + "<>0";
                this.DBConnection.ExecuteNonQuery(SqlQuery);

                //-- Update Numeric data
                SqlQuery = "UPDATE  " + this.DBQueries.TablesName.Data + " SET " + DataValueTempColumn + " =" + Data.DataValue + " WHERE " + Data.IsTextualData + " = 0";
                this.DBConnection.ExecuteNonQuery(SqlQuery);


                //-- Drop  DataValue col
                SqlQuery = "ALTER TABLE " + this.DBQueries.TablesName.Data + " DROP COLUMN " + Data.DataValue;
                this.DBConnection.ExecuteNonQuery(SqlQuery);

                //-- Drop  DataValue col
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Update.AlterDataValueColumnDataTypeToDouble(tablesName, forOnlineDB, serverType));

                //-- Update Numeric column into
                SqlQuery = "UPDATE  " + this.DBQueries.TablesName.Data + " SET " + Data.DataValue + " =" + DataValueTempColumn;
                this.DBConnection.ExecuteNonQuery(SqlQuery);
                //-- DROp Temp Column 
                SqlQuery = "ALTER TABLE " + this.DBQueries.TablesName.Data + " DROP COLUMN " + DataValueTempColumn;
                this.DBConnection.ExecuteNonQuery(SqlQuery);

                //#region "//-- Exception handled:File sharing lock count exceeded. Increase MaxLocksPerFile registry entry. --"

                //DIConnectionDetails Obj = this._DBConnection.ConnectionStringParameters;
                //if (this._DBConnection != null)
                //{
                //    this._DBConnection.Dispose();
                //}
                //this._DBConnection = new DIConnection(Obj);

                //#endregion



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void AddColumnsIntoIUSTable(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {
            try
            {
                if (!this.ISColumnExist(Indicator_Unit_Subgroup.IsDefaultSubgroup, tablesName.IndicatorUnitSubgroup))
                {
                    // add ISDefaultsubgroup column
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IUS.Insert.InsertDI7IsDefaultSubgroupColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Indicator_Unit_Subgroup.AvlMinDataValue, tablesName.IndicatorUnitSubgroup))
                {
                    // add AvlMinDataValue column
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IUS.Insert.InsertDI7AvlMinDataValueColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Indicator_Unit_Subgroup.AvlMaxDataValue, tablesName.IndicatorUnitSubgroup))
                {
                    // Add AvlMaxDataValue Column
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IUS.Insert.InsertDI7AvlMaxDataValueColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Indicator_Unit_Subgroup.AvlMinTimePeriod, tablesName.IndicatorUnitSubgroup))
                {
                    // Add AvlMinTimePeriod Column
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IUS.Insert.InsertDI7AvlMinTimePeriodColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Indicator_Unit_Subgroup.AvlMaxTimePeriod, tablesName.IndicatorUnitSubgroup))
                {
                    // Add AvlMaxTimePeriod Column
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IUS.Insert.InsertDI7AvlMaxTimePeriodColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void AddColumnsIntoIndicatorClassificationsTable(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {
            try
            {
                if (!this.ISColumnExist(IndicatorClassifications.ISBN, tablesName.IndicatorClassifications))
                {
                    // Add ISBN Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.InsertDI7ISBNColumns(tablesName, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(IndicatorClassifications.Nature, tablesName.IndicatorClassifications))
                {
                    // Add Nature Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.InsertDI7NatureColumns(tablesName, forOnlineDB, serverType));
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }


        private void CheckNCreateMetadataCategoryTable(bool forOnlineDB, DITables tablesName)
        {
            string DataPrefix = string.Empty;
            string LanguageCode = string.Empty;
            MetadataCategoryBuilder MetadataCategoryBuilderObj = null;
            DIQueries TempQueries;

            try
            {
                // check table already exists or not
                TempQueries = new DIQueries(tablesName.CurrentDataPrefix, tablesName.CurrentLanguageCode);
                MetadataCategoryBuilderObj = new MetadataCategoryBuilder(this._DBConnection, TempQueries);

                if (MetadataCategoryBuilderObj.IsMetadataCategoryTableExists() == false)
                {
                    this.AddMissingMetadataCategoryTables(tablesName.MetadataCategory);

                    this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Insert.CreateTable(tablesName.MetadataCategory, forOnlineDB, this._DBConnection.ConnectionStringParameters.ServerType));

                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }


        private void AddColumnsIntoMetadataCategoryTable(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {
            try
            {

                if (!this.ISColumnExist(Metadata_Category.ParentCategoryNId, tablesName.MetadataCategory))
                {
                    // check and create metadata category table
                    this.CheckNCreateMetadataCategoryTable(forOnlineDB, tablesName);

                    // Add ParentCategoryNId Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Insert.InsertParentCategoryNIdColumn(tablesName, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Metadata_Category.CategoryGId, tablesName.MetadataCategory))
                {
                    // Add CategoryGId Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Insert.InsertCategoryGIdColumn(tablesName, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Metadata_Category.CategoryDescription, tablesName.MetadataCategory))
                {
                    // Add CategoryDescription Column
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Insert.InsertCategoryDescriptionColumn(tablesName, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Metadata_Category.IsPresentational, tablesName.MetadataCategory))
                {
                    // Add  IsPresentational Column
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Insert.InsertIsPresentationalColumn(tablesName, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Metadata_Category.IsMandatory, tablesName.MetadataCategory))
                {
                    // Add IsMandatory Column
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Insert.InsertIsMandatoryColumn(tablesName, forOnlineDB, serverType));
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void AddColumnIntoAreaTable(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {
            try
            {
                if (!this.ISColumnExist(Area.AreaShortName, tablesName.Area))
                {
                    // Add Data_Exist Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Insert.AddAreaShortNameColumn(tablesName, forOnlineDB, serverType));
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void AddColumnIntoDataTable(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {

            try
            {
                if (!this.ISColumnExist(Data.TextualDataValue, tablesName.Data))
                {
                    // Add Textual_Data_Value Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Insert.InsertDI7TextualDataValueColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Data.IsTextualData, tablesName.Data))
                {
                    // Add IsTextualData Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Insert.InsertDI7IsTextualDataColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Data.IsMRD, tablesName.Data))
                {
                    // Add IsMRD Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Insert.InsertDI7IsMRDColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Data.IsPlannedValue, tablesName.Data))
                {
                    // Add IsPlannedValue Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Insert.InsertDI7IsPlannedValueColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Data.IUNId, tablesName.Data))
                {
                    // Add IUNId Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Insert.InsertDI7IUNIdColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Data.ConfidenceIntervalUpper, tablesName.Data))
                {
                    // Add ConfidenceIntervalUpper Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Insert.InsertDI7ConfidenceIntervalUpperColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Data.ConfidenceIntervalLower, tablesName.Data))
                {
                    // Add ConfidenceIntervalLower Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Insert.InsertDI7ConfidenceIntervalLowerColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

                if (!this.ISColumnExist(Data.MultipleSource, tablesName.Data))
                {
                    // Add MultipleSource Column 
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Insert.InsertDI7MultipleSourceColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }

        private void AddColumnIntoTimeperiodTable(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {
            try
            {
                // Add StartDate Column 
                if (!this.ISColumnExist(Timeperiods.StartDate, tablesName.TimePeriod))
                {
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Timeperiod.Insert.InsertDI7StartDateColumn(this._DBQueries.DataPrefix));
                }

                // Add EndDate Column 
                if (!this.ISColumnExist(Timeperiods.EndDate, tablesName.TimePeriod))
                {
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Timeperiod.Insert.InsertDI7EndDateColumn(this._DBQueries.DataPrefix));
                }

                // Add Perodicity Column 
                if (!this.ISColumnExist(Timeperiods.Periodicity, tablesName.TimePeriod))
                {
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Timeperiod.Insert.InsertDI7PerodicityColumn(this._DBQueries.DataPrefix, forOnlineDB, serverType));
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }

        private void UpdateCategoryTable()
        {

            MetadataCategoryBuilder CategoryBuilder;
            DIQueries TempDBQueries = null;
            string DataPrefix = string.Empty;
            string LanguageCode = string.Empty;
            string CategoryNid = string.Empty;
            string CategoryGID = string.Empty;

            string SourceCategoryTable = string.Empty;
            DataTable CategoryTable;

            try
            {
                // step1: create table for all dataset
                foreach (DataRow DataPrefixRow in this._DBConnection.DIDataSets().Rows)
                {
                    DataPrefix = DataPrefixRow[DBAvailableDatabases.AvlDBPrefix].ToString() + "_";

                    // get language for all available languages
                    foreach (DataRow LanguageRow in this._DBConnection.DILanguages(DataPrefix).Rows)
                    {
                        LanguageCode = "_" + LanguageRow[Language.LanguageCode].ToString();

                        // check table already exists or not
                        TempDBQueries = new DIQueries(DataPrefix, LanguageCode);

                        if (string.IsNullOrEmpty(SourceCategoryTable))
                        {
                            // update GID in only one language table and for other  language tables, use this language table
                            CategoryBuilder = new MetadataCategoryBuilder(this._DBConnection, TempDBQueries);

                            CategoryTable = CategoryBuilder.GetAllRecordsFromMetadataCategory();

                            foreach (DataRow Row in CategoryTable.Rows)
                            {
                                //update metdata category table (set parent_nid to -1 and update gids)            
                                //CategoryGID = Convert.ToString(Row[Metadata_Category.CategoryName]).ToUpper().Replace(" ", "_");
                                //CategoryGID = MetaDataBuilder.GetNewMetaDataCategoryGID();

                                CategoryNid = Convert.ToString(Row[Metadata_Category.CategoryNId]);
                                CategoryGID = DICommon.GetValidGIdForSDMXRule(Convert.ToString(Row[Metadata_Category.CategoryName]).ToUpper()) + "_" + CategoryNid;

                                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Update.UpdateMetadataCategory(TempDBQueries.TablesName.MetadataCategory, Convert.ToInt32(CategoryNid), CategoryGID, "-1", false, false));
                            }

                            SourceCategoryTable = TempDBQueries.TablesName.MetadataCategory;
                        }
                        else
                        {
                            // use first language table  to update other language table
                            this.DBConnection.ExecuteNonQuery("UPDATE " + SourceCategoryTable + " AS src INNER JOIN " + TempDBQueries.TablesName.MetadataCategory + " AS trg ON src." + Metadata_Category.CategoryNId + " = trg." + Metadata_Category.CategoryNId + "  SET trg." + Metadata_Category.CategoryGId + "=src." + Metadata_Category.CategoryGId + " and trg." + Metadata_Category.ParentCategoryNId + "=src." + Metadata_Category.ParentCategoryNId + ";");
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void ConvertMetadataXmlIntoDatabse()
        {
            DI7MetadataConverter MetadataConverter = new DI7MetadataConverter(this._DBConnection, this._DBQueries);
            DIQueries TempDBQueries = null;
            string DataPrefix = string.Empty;
            string LanguageCode = string.Empty;
            try
            {
                // step1: create table for all dataset
                foreach (DataRow DataPrefixRow in this._DBConnection.DIDataSets().Rows)
                {
                    DataPrefix = DataPrefixRow[DBAvailableDatabases.AvlDBPrefix].ToString() + "_";

                    // get language for all available langauges
                    foreach (DataRow LanguageRow in this._DBConnection.DILanguages(DataPrefix).Rows)
                    {
                        LanguageCode = "_" + LanguageRow[Language.LanguageCode].ToString();

                        // check table already exists or not
                        TempDBQueries = new DIQueries(DataPrefix, LanguageCode);

                        MetadataConverter = new DI7MetadataConverter(this._DBConnection, TempDBQueries);

                        MetadataConverter.WrongMetdataFoundEvent += new WrongMetadataFound(MetadataConverter_WrongMetdataFoundEvent);

                        // convert Indicator,Map and Source Info
                        MetadataConverter.ConvertIndicatorMapICAndXsltMetadataIntoNewFormat();

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        void MetadataConverter_WrongMetdataFoundEvent(MetadataElementType elementType, string name, string GID)
        {
            // check n create list object
            if (this._WrongMetadataElementList == null)
            {
                this._WrongMetadataElementList = new SortedDictionary<MetadataElementType, List<string>>();
            }

            // check n create element type key into dictionary object
            if (!this._WrongMetadataElementList.ContainsKey(elementType))
            {
                this._WrongMetadataElementList.Add(elementType, new List<string>());
            }

            // add name into list if doesnt exist
            if (!this._WrongMetadataElementList[elementType].Contains(name))
            {
                this._WrongMetadataElementList[elementType].Add(name);
            }
        }



        private void UpdateDefaultSubgroupValue()
        {
            try
            {
                IUSBuilder IUSbuilderObj = new IUSBuilder(this.DBConnection, this.DBQueries);

                IUSbuilderObj.UpdateISDefaultSubgroups();
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }

        #endregion


        #region "-- Database value updation for DI7 --"


        #endregion

        #endregion

        #endregion

        #region "-- public --"

        #region "-- new/dispose --"

        public DI7_0_0_0DBConverter(DIConnection dbConnection, DIQueries dbQueries)
            : base(dbConnection, dbQueries)
        {
            //donothing
        }

        #endregion

        #region "-- Methods --"


        /// <summary>
        /// Returns true/false. True if Database is in valid format otherwise false.
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public override bool IsValidDB(bool forOnlineDB)
        {
            bool RetVal = false;

            try
            {
                // check 6.0.1.5 version exists in dbVersion table
                if (this._DBConnection.ExecuteDataTable(this._DBQueries.DBVersion.GetRecords(Constants.Versions.DI7_0_0_0)).Rows.Count > 0)
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

        /// <summary>
        /// Converts DevInfo Database into DevInfo6.0.0.5 format
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public override bool DoConversion(bool forOnlineDB)
        {
            bool RetVal = false;
            int TotalSteps = 9;
            DBVersionBuilder VersionBuilder;
            DIDatabase DBDatabase;
            System.Globalization.CultureInfo oldCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            // Do the conversion only if database has different Schema
            try
            {

                if (!this.IsValidDB(forOnlineDB))
                {
                    DITables.ICIUSTableName = "Indicator_Classifications_IUS";
                    this._DBQueries = new DIQueries(this._DBQueries.DataPrefix, this._DBQueries.LanguageCode);
                    if (!base.IsValidDB(forOnlineDB))
                    {
                        RetVal = base.DoConversion(forOnlineDB);
                    }

                    DITables.ICIUSTableName = "IC_IUS";
                    this._DBQueries = new DIQueries(this._DBQueries.DataPrefix, this._DBQueries.LanguageCode);

                    if (this._ConvertDatabase)
                    {
                        this.RaiseProcessStartedEvent(TotalSteps);

                        this.RaiseProcessInfoEvent(0);

                        // Step 1: insert version info into database
                        VersionBuilder = new DBVersionBuilder(this._DBConnection, this._DBQueries);
                        VersionBuilder.InsertVersionInfo(Constants.Versions.DI7_0_0_0, Constants.VersionsChangedDates.DI7_0_0_0, Constants.VersionComments.DI7_0_0_0);

                        this.RaiseProcessInfoEvent(1);


                        // Step 2: Add new columns into IC, Indicator, IUS & area table and create document table
                        this.UpdateDBSchema(forOnlineDB);
                    }

                    RetVal = true;

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
            finally
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI;
            }

            return RetVal;
        }


        #endregion


        #endregion
    }
}
