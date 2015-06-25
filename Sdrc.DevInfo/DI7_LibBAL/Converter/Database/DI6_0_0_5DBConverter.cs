using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using System.Data;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{
    /// <summary>
    /// Helps in converting DevInfo database into DevInfo 6_0_0_5 format
    /// </summary>
    public class DI6_0_0_5DBConverter : DI6_0_0_4DBConverter
    {

        #region "-- private --"

        #region "-- Variables --"

        #endregion

        #region "-- Methods --"

        private void CreateMetadatCategoryTable(bool forOnlineDB)
        {
            string DataPrefix = string.Empty;
            string LanguageCode = string.Empty;
            DITables TableNames;
            MetadataCategoryBuilder MetadataCategoryBuilderObj = null;
            DIQueries TempQueries;

            try
            {
                // step1: create table for all dataset
                foreach (DataRow DataPrefixRow in this._DBConnection.DIDataSets().Rows)
                {
                    DataPrefix = DataPrefixRow[DBAvailableDatabases.AvlDBPrefix].ToString() + "_";

                    // create table for all available langauges
                    foreach (DataRow LanguageRow in this._DBConnection.DILanguages(DataPrefix).Rows)
                    {
                        LanguageCode = "_" + LanguageRow[Language.LanguageCode].ToString();

                        // check table already exists or not
                        TempQueries = new DIQueries(DataPrefix, LanguageCode);
                        MetadataCategoryBuilderObj = new MetadataCategoryBuilder(this._DBConnection, TempQueries);

                        if (MetadataCategoryBuilderObj.IsMetadataCategoryTableExists() == false)
                        {
                            TableNames = new DITables(DataPrefix, LanguageCode);

                            this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.MetadataCategory.Insert.CreateTable(TableNames.MetadataCategory,
                                forOnlineDB, this._DBConnection.ConnectionStringParameters.ServerType));

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        private void ConvertMetataCategoryIntoDatabase()
        {
            MetadataCategoryBuilder MetadataCatBuilder = new MetadataCategoryBuilder(this._DBConnection, this._DBQueries);
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
                        MetadataCatBuilder = new MetadataCategoryBuilder(this._DBConnection, TempDBQueries);

                        // Populate Metadata_Category Table from mask file or default values
                        MetadataCatBuilder.UpdateMetadataCategoryTableFromMaskFile();
                    }
                }

            }
            catch
            {
            }
        }

        private void ConvertMetadataXmlIntoDatabse()
        {
            MetadataCategoryBuilder MetadataCatBuilder = new MetadataCategoryBuilder(this._DBConnection, this._DBQueries);
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

                        MetadataCatBuilder = new MetadataCategoryBuilder(this._DBConnection, TempDBQueries);

                        // convert Indicator,Map and Source Info
                        MetadataCatBuilder.ConvertIndicatorMapICAndXsltMetadataIntoNewFormat();
                    }
                }

            }
            catch (Exception)
            {
            }
        }

        private void AddOrderColumn(bool forOnlineDB, DIServerType serverType)
        {
            string DataPrefix = string.Empty;
            string LanguageCode = string.Empty;
            DIQueries TempQueries;

            try
            {
                // step1: create table for all dataset
                foreach (DataRow DataPrefixRow in this._DBConnection.DIDataSets().Rows)
                {
                    DataPrefix = DataPrefixRow[DBAvailableDatabases.AvlDBPrefix].ToString() + "_";

                    // create table for all available langauges
                    foreach (DataRow LanguageRow in this._DBConnection.DILanguages(DataPrefix).Rows)
                    {
                        LanguageCode = "_" + LanguageRow[Language.LanguageCode].ToString();

                        // check table already exists or not
                        TempQueries = new DIQueries(DataPrefix, LanguageCode);

                        // Add Order Column Into Subgroup_Val Table
                        this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.SubgroupVal.Insert.AddOrderColumn(TempQueries.DataPrefix, TempQueries.LanguageCode, forOnlineDB, serverType));

                        // Add Order Column Into Subgroup Table
                        this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Subgroup.Insert.AddOrderColumn(TempQueries.DataPrefix, TempQueries.LanguageCode, forOnlineDB, serverType));

                        // Add Order Column Into IC
                        this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.AddOrderColumn(TempQueries.DataPrefix, TempQueries.LanguageCode, forOnlineDB, serverType));

                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void UpdateSortOrderIntoSubgroupVal()
        {
            DataView Table = null;
            int Order = 1;
            DIQueries TempQueries;
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
                        TempQueries = new DIQueries(DataPrefix, LanguageCode);
                        // Get SubgroupVal
                        Table = this._DBConnection.ExecuteDataTable(TempQueries.SubgroupVals.GetSubgroupVals()).DefaultView;
                        Table.Sort = SubgroupVals.SubgroupVal + " Asc";

                        // Update SUbgroupVal Order Column
                        foreach (DataRowView Row in Table)
                        {
                            this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.SubgroupVal.Update.UpdateSubgroupValOrder(TempQueries.DataPrefix, TempQueries.LanguageCode, Convert.ToInt32(Row[SubgroupVals.SubgroupValNId]), Order++));

                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void UpdateSortOrderIntoSubgroup()
        {
            DataView Table = null;
            DIQueries TempQueries;
            int SubgroupType = 0;
            int Order = 1;
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
                        TempQueries = new DIQueries(DataPrefix, LanguageCode);

                        // Get Subgroup Dimension Values
                        Table = this._DBConnection.ExecuteDataTable(TempQueries.Subgroup.GetSubgroup(FilterFieldType.None, string.Empty)).DefaultView;

                        // Sort By SubgroupType and Subgroup
                        Table.Sort = Subgroup.SubgroupType + " Asc," + Subgroup.SubgroupName + " Asc";

                        // Update SubgroupVal Order Column
                        foreach (DataRowView Row in Table)
                        {
                            // Reset Subgroup Order id SubgroupType changes
                            if (SubgroupType != Convert.ToInt32(Row[Subgroup.SubgroupType]))
                            {
                                Order = 1;
                                SubgroupType = Convert.ToInt32(Row[Subgroup.SubgroupType]);
                            }

                            this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Subgroup.Update.UpdateSubgroupOrderByNId(TempQueries.DataPrefix, TempQueries.LanguageCode, Convert.ToInt32(Row[Subgroup.SubgroupNId]), Order++));

                        }

                    }
                }
            }
            catch (Exception)
            {
            }

        }

        private bool UpdateSortOrderIntoIC(int icParentNId, ICType icType)
        {
            bool RetVal = true;
            DataView Table = null;
            DIQueries TempQueries;
            int ParentNId = 0;
            int ICOrder = 1;
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
                        TempQueries = new DIQueries(DataPrefix, LanguageCode);


                        Table = this.DBConnection.ExecuteDataTable(TempQueries.IndicatorClassification.GetIC(FilterFieldType.ParentNId, icParentNId.ToString(), icType, FieldSelection.Light)).DefaultView;

                        if (Table.Count > 0)
                        {
                            Table.Sort = IndicatorClassifications.ICName + " Asc";
                            // Get each Child ICNId
                            foreach (DataRowView ICRow in Table)
                            {
                                ParentNId = Convert.ToInt32(ICRow[IndicatorClassifications.ICNId]);
                                RetVal = this.UpdateSortOrderIntoIC(ParentNId, icType);
                                // Set Sort Order
                                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Update.UpdateICOrder(TempQueries.DataPrefix, TempQueries.LanguageCode, ICOrder++, ParentNId));
                                // Update ICIUS_IUSNId Order
                                this.UpdateSortOrderIntoICIUS(ParentNId, icType);
                            }
                        }
                        else
                        {
                            RetVal = false;
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

        private void UpdateSortOrderIntoICIUS(int icNId, ICType icType)
        {
            DataView Table = null;
            int Order = 1;

            Table = this._DBConnection.ExecuteDataTable(this._DBQueries.IUS.GetDistinctIUSByIC(icType, icNId, FieldSelection.Light)).DefaultView;

            Table.Sort = Indicator.IndicatorName + " Asc," + Unit.UnitName + " Asc," + SubgroupVals.SubgroupVal + " Asc";

            foreach (DataRowView Row in Table)
            {
                this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Update.UdpateICIUSOrder(this._DBQueries.DataPrefix, Order++, Convert.ToInt32(Row[IndicatorClassificationsIUS.ICIUSNId])));
            }

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
                        this.AddColumnsIntoIndicatorTable(forOnlineDB, TablesName, ServerType);

                        // insert new column "Data_Exist" into area table 
                        this.AddColumnIntoAreaTable(forOnlineDB, TablesName, ServerType);
                    }

                    // add columns into indicator_unit_subgroup table
                    this.AddColumnsIntoIUSTable(forOnlineDB, TablesName, ServerType);

                    // add document table
                    this.CreateDocumentTable(forOnlineDB, TablesName, ServerType);
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void AddColumnsIntoIUSTable(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {
            try
            {
                // add subgroup_nids column
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IUS.Insert.InsertSubgroupNidsColumn(tablesName, forOnlineDB, serverType));

                ////// add subgroup_type_nids column
                ////this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IUS.Insert.InsertSubgroupTypeNidsColumn(tablesName, forOnlineDB, serverType));

                // add data_exist column
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IUS.Insert.InsertDataExistColumn(tablesName, forOnlineDB, serverType));
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

                // Add IC_short_name Column 
                if (!DICommon.ISColumnExistInTable(this._DBConnection, IndicatorClassifications.ICShortName, tablesName.IndicatorClassifications))
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.InsertICShortNameColumn(tablesName, forOnlineDB, serverType));

                // Add publisher Column 
                if (!DICommon.ISColumnExistInTable(this._DBConnection, IndicatorClassifications.Publisher, tablesName.IndicatorClassifications))
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.InsertPublisherColumn(tablesName, forOnlineDB, serverType));

                // Add title Column 
                if (!DICommon.ISColumnExistInTable(this._DBConnection, IndicatorClassifications.Title, tablesName.IndicatorClassifications))
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.InsertTitleColumn(tablesName, forOnlineDB, serverType));

                // Add DIYear Column 
                if (!DICommon.ISColumnExistInTable(this._DBConnection, IndicatorClassifications.DIYear, tablesName.IndicatorClassifications))
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.InsertDIYearColumn(tablesName, forOnlineDB, serverType));

                // Add SourceLink1 Column 
                if (!DICommon.ISColumnExistInTable(this._DBConnection, IndicatorClassifications.SourceLink1, tablesName.IndicatorClassifications))
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.InsertSourceLink1Column(tablesName, forOnlineDB, serverType));

                // Add SourceLink2 Column 
                if (!DICommon.ISColumnExistInTable(this._DBConnection, IndicatorClassifications.SourceLink2, tablesName.IndicatorClassifications))
                    this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.InsertSourceLink2Column(tablesName, forOnlineDB, serverType));

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void AddColumnsIntoIndicatorTable(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {
            try
            {
                // Add short_name Column 
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Insert.InsertShortNameColumn(tablesName, forOnlineDB, serverType));

                // Add keywords Column 
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Insert.InsertKeywordsColumn(tablesName, forOnlineDB, serverType));

                // Add indicator_order Column 
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Insert.InsertIndicatorOrderColumn(tablesName, forOnlineDB, serverType));

                // Add Data exist Column 
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Insert.InsertDataExistColumn(tablesName, forOnlineDB, serverType));

                // Add HighIsGood Column 
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Indicator.Insert.InsertHighIsGoodColumn(tablesName, forOnlineDB, serverType));

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
                // Add Data_Exist Column 
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Area.Insert.InsertDataExistColumn(tablesName, forOnlineDB, serverType));
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }


        private void CreateDocumentTable(bool forOnlineDB, DITables tablesName, DIServerType serverType)
        {
            try
            {
                // create document table
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.DIDocument.Insert.CreateTable(tablesName.Document, forOnlineDB, serverType));
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        #endregion

        #endregion

        #endregion

        #region "-- public --"

        #region "-- new/dispose --"

        public DI6_0_0_5DBConverter(DIConnection dbConnection, DIQueries dbQueries)
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
                if (this._DBConnection.ExecuteDataTable(this._DBQueries.DBVersion.GetRecords(Constants.Versions.DI6_0_0_5)).Rows.Count > 0)
                {
                    RetVal = true;
                }
            }
            catch (Exception)
            {
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
            int TotalSteps = 7;
            DBVersionBuilder VersionBuilder;
            DIDatabase DBDatabase;

            // Do the conversion only if database has different Schema
            try
            {
                if (!this.IsValidDB(forOnlineDB))
                {
                    if (!base.IsValidDB(forOnlineDB))
                    {
                        RetVal = base.DoConversion(forOnlineDB);
                    }

                    // Step 1: insert version info into database
                    VersionBuilder = new DBVersionBuilder(this._DBConnection, this._DBQueries);
                    VersionBuilder.InsertVersionInfo(Constants.Versions.DI6_0_0_5, Constants.VersionsChangedDates.DI6_0_0_5, Constants.VersionComments.DI6_0_0_5);

                    this.RaiseProcessStartedEvent(TotalSteps);

                    // Step 2: Add new columns into IC, Indicator, IUS & area table and create document table
                    this.UpdateDBSchema(forOnlineDB);

                    // Step 3: Create Metadata_Category Table
                    this.CreateMetadatCategoryTable(forOnlineDB);
                    this.RaiseProcessInfoEvent(1);

                    // Step 4: Add Order Column into Subgroup, SubgrouVal,IC 
                    this.AddOrderColumn(forOnlineDB, this._DBConnection.ConnectionStringParameters.ServerType);
                    this.RaiseProcessInfoEvent(2);

                    // Step 5: Convert Metadata Values of Indicator,Area,Source
                    this.ConvertMetadataXmlIntoDatabse();
                    this.RaiseProcessInfoEvent(3);

                    // Step 6: Update Metadata Values of Indicator,Area,Source and Set Category values from Mask Files
                    this.ConvertMetataCategoryIntoDatabase();
                    this.RaiseProcessInfoEvent(4);

                    // Step 7: Update Order value SubgroupVal
                    this.UpdateSortOrderIntoSubgroupVal();
                    this.RaiseProcessInfoEvent(5);

                    // Step 8: Update Sortt Column for Subgroup Dimension Values
                    this.UpdateSortOrderIntoSubgroup();
                    this.RaiseProcessInfoEvent(6);

                    // Step 9: Update Order value for Indicator Classification(for Sector,Goal,Source... etc. and IC_IUS_Order IN IC_IUS table
                    this.UpdateSortOrderIntoIC(-1, ICType.Sector);
                    this.UpdateSortOrderIntoIC(-1, ICType.Source);
                    this.UpdateSortOrderIntoIC(-1, ICType.Theme);
                    this.UpdateSortOrderIntoIC(-1, ICType.Goal);
                    this.UpdateSortOrderIntoIC(-1, ICType.Institution);
                    this.UpdateSortOrderIntoIC(-1, ICType.Convention);
                    this.UpdateSortOrderIntoIC(-1, ICType.CF);
                    this.RaiseProcessInfoEvent(7);

                    RetVal = true;

                    // Step 10: update auto calcualted fields
                    DBDatabase = new DIDatabase(this._DBConnection, this._DBQueries);
                    DBDatabase.UpdateAutoCalculatedFieldsInTables();
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


        #endregion


        #endregion
    }
}
