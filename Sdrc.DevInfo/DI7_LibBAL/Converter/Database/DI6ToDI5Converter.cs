


using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using UpdateQueries = DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{
    /// <summary>
    /// Use this class to convert DevInfo 6.0 database into DevInfo 5.0 format
    /// </summary>
    public class DI6ToDI5Converter
    {
        #region "-- Private --"

        #region "-- Constants --"
        // 1-Sex, 2-Location, 3-Age, 4-Others

        private const int AgeNId = 3;
        private const int SexNId = 1;
        private const int LocationNId = 2;
        private const int OthersNId = 4;


        private const string SubgroupAgeType = "Age";
        private const string SubgroupSexType = "Sex";
        private const string SubgroupLocationType = "Location";
        private const string SubgroupOthersType = "Others";

        #endregion

        #region "-- Variables --"

        private DIConnection DBConnection = null;
        private DIQueries DBQueries = null;
        private string FilenameWPath = string.Empty;
                private int versionCheck = 0;

        #endregion

        #region "-- Methods --"

        private void UpdateDataTable()
        {
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, Data.IndicatorNId);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, Data.UnitNId);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, Data.SubgroupValNId);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, Data.ICIUSOrder);
        }

        private void RemoveColumnsFrmICIUS()
        {
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.IndicatorClassificationsIUS, IndicatorClassificationsIUS.ICIUSLabel);
            this.DBConnection.DropIndividualColumnOfTable(this.DBQueries.TablesName.IndicatorClassificationsIUS, IndicatorClassificationsIUS.ICIUSOrder);
        }

        private void DeleteVersiontable()
        {
            this.DBConnection.DropTable(this.DBQueries.TablesName.DBVersion);
        }
            

        #region "-- Change no c1 --"

        private void DeleteNewLanguageBasedTable()
        {
           
            // get data prefix
            string DataPrefix = this.DBConnection.DIDataSetDefault();

            // For Language Code
            string LanguageCode = string.Empty;

            DITables TableNames = null;

            foreach (DataRow LanguageRow in this.DBConnection.DILanguages(DataPrefix).Rows)
            {

                LanguageCode = "_" + LanguageRow[Language.LanguageCode].ToString();

                 // Getting Table With Language Code 
                TableNames = new DITables(DataPrefix, LanguageCode);

                // delete DBMetadata table
                this.DBConnection.DropTable(TableNames.DBMetadata);

                // delete recommendedSources table
                this.DBConnection.DropTable(TableNames.RecommendedSources);
            }

        }

        #endregion

        private void UpdateSubgroups()
        {
            // subgroup & subgroup_type table 
            this.UpdateSubgroupNSubgroupType();

            // subgroup_val          
            this.UpdateSubgroupVal();
        }

        private void UpdateSubgroupNSubgroupType()
        {
            int SexNidInDI6 = 1;
            int AgeNidInDI6 = 2;
            int LocationNidInDI6 = 3;
            int OthersNidInDI6 = 4;

            string DataPrefix = string.Empty;
            string LangaugeCode = string.Empty;

            DataTable SubgroupTable;
            DITables TableNames = null;

            try
            {
                // Step1: get sex,age,location and other's nid from DI6 database
                SexNidInDI6 = this.GetSubgroupTypeNid("SEX");
                OthersNidInDI6 = this.GetSubgroupTypeNid("OTHERS");
                AgeNidInDI6 = this.GetSubgroupTypeNid("AGE");
                LocationNidInDI6 = this.GetSubgroupTypeNid("LOCATION");

                // Step2: replace age, sex, others and location Nids in subgroup table with predefined nids            
                DataPrefix = this.DBConnection.DIDataSetDefault();

                foreach (DataRow Row in this.DBConnection.DILanguages(DataPrefix).Rows)
                {
                    LangaugeCode ="_"+ Row[Language.LanguageCode].ToString();

                    this.DBConnection.ExecuteNonQuery(UpdateQueries.Subgroup.Update.UpdateSubgroupTypeInSubgroupTable(DataPrefix, LangaugeCode,
                        SexNidInDI6, SexNId));
                    this.DBConnection.ExecuteNonQuery(UpdateQueries.Subgroup.Update.UpdateSubgroupTypeInSubgroupTable(DataPrefix, LangaugeCode,
                       AgeNidInDI6, AgeNId));
                    this.DBConnection.ExecuteNonQuery(UpdateQueries.Subgroup.Update.UpdateSubgroupTypeInSubgroupTable(DataPrefix, LangaugeCode,
                       LocationNidInDI6, LocationNId));
                    this.DBConnection.ExecuteNonQuery(UpdateQueries.Subgroup.Update.UpdateSubgroupTypeInSubgroupTable(DataPrefix, LangaugeCode,
                       OthersNidInDI6, OthersNId));


                    // Step3: replace remaining nids with subgroup_other_Nid  (subgroup)
                    this.DBConnection.ExecuteNonQuery(UpdateQueries.Subgroup.Update.UpdateOtherTypesInSubgroupTable(DataPrefix, LangaugeCode,
                    AgeNId, SexNId, LocationNId, OthersNId));

                    // Step3: delete subgroup_type table
                    TableNames = new DITables(DataPrefix, LangaugeCode);
                    this.DBConnection.DropTable(TableNames.SubgroupType);
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private int GetSubgroupTypeNid(string typeName)
        {
            int RetVal = -1;
            DataTable Table;

            try
            {
                Table = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.GId, "'"+ typeName+"'" ));

                if (Table.Rows.Count > 0)
                {
                    RetVal = Convert.ToInt32(Table.Rows[0][SubgroupTypes.SubgroupTypeNId]);
                }
            }
            catch (Exception ex)
            {
                RetVal = -1;
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        private void UpdateSubgroupVal()
        {
            DataTable SubgroupValTable;
            DataTable AssociatedSubgroupTable;
            int SubgroupValNId = -1;
            string UpdateColumnName = string.Empty;
            string DataPrefix = string.Empty;
            string LangaugeCode = string.Empty;
            DITables TablesName = null;

            try
            {
                // get data prefix
                DataPrefix = this.DBConnection.DIDataSetDefault();

                foreach (DataRow LanguageRow in this.DBConnection.DILanguages(DataPrefix).Rows)
                {
                    // get language code
                    LangaugeCode = "_" + LanguageRow[Language.LanguageCode].ToString();

                    // set table names
                    TablesName = new DITables(DataPrefix, LangaugeCode);

                    // Step1: add Subgroup_age,Subgroup_sex,Subgroup_location,Subgroup_others columns in subgroup_Val table
                    this.DBConnection.AddColumn(TablesName.SubgroupVals, SubgroupValRemovedColumns.SubgroupValAge, "int", "0");
                    this.DBConnection.AddColumn(TablesName.SubgroupVals, SubgroupValRemovedColumns.SubgroupValSex, "int", "0");
                    this.DBConnection.AddColumn(TablesName.SubgroupVals, SubgroupValRemovedColumns.SubgroupValLocation, "int", "0");
                    this.DBConnection.AddColumn(TablesName.SubgroupVals, SubgroupValRemovedColumns.SubgroupValOthers, "int", "0");


                    // get subgroupval from database
                    SubgroupValTable = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupVals.GetSubgroupVals());

                    // Step2: Do the following for each subgroup_val record
                    // Step 2(a): On the basis of subgroup_val_Nid, update age,sex,location and others column using subgroup_Val_subgroup table
                    foreach (DataRow Row in SubgroupValTable.Rows)
                    {


                        SubgroupValNId = Convert.ToInt32(Row[SubgroupVals.SubgroupValNId]);

                        AssociatedSubgroupTable = this.DBConnection.ExecuteDataTable(this.DBQueries.Subgroup.GetSubgroupNIdNType(SubgroupValNId));

                        // set  age, sex , others and location to zero 
                       this.DBConnection.ExecuteNonQuery(UpdateQueries.SubgroupVal.Update.SetAgeSexLocationNOthersToZero(DataPrefix,LangaugeCode,SubgroupValNId));

                        if (AssociatedSubgroupTable.Rows.Count > 0)
                        {
                            foreach (DataRow SGRow in AssociatedSubgroupTable.Rows)
                            {
                                switch (Convert.ToInt32(SGRow[Subgroup.SubgroupType]))
                                {
                                    case DI6ToDI5Converter.AgeNId:
                                        UpdateColumnName = SubgroupValRemovedColumns.SubgroupValAge;
                                        break;

                                    case DI6ToDI5Converter.SexNId:
                                        UpdateColumnName = SubgroupValRemovedColumns.SubgroupValSex;
                                        break;

                                    case DI6ToDI5Converter.LocationNId:
                                        UpdateColumnName = SubgroupValRemovedColumns.SubgroupValLocation;
                                        break;

                                    case DI6ToDI5Converter.OthersNId:
                                        UpdateColumnName = SubgroupValRemovedColumns.SubgroupValOthers;
                                        break;

                                    default:
                                        UpdateColumnName = string.Empty;
                                        break;
                                }

                                if (!string.IsNullOrEmpty(UpdateColumnName))
                                {
                                this.DBConnection.ExecuteNonQuery(UpdateQueries.SubgroupVal.Update.UpdateAgeSexLocationNOthersInSubgroupVal(
                                        DataPrefix, LangaugeCode, UpdateColumnName, Convert.ToInt32(SGRow[Subgroup.SubgroupNId]), SubgroupValNId));
                                }
                            }

                        }
                    }

                }

                //Step 3: Drop Subgroupvalssubgrouptable
                this.DBConnection.DropTable(this.DBQueries.TablesName.SubgroupValsSubgroup);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void InsertDbName(string dbNamewithPath)
        {
            string DataPrefix = string.Empty;

            string dbName = string.Empty;

            //Getting DBInformation from Path
            FileInfo FileInformation = new FileInfo(dbNamewithPath);

            //Getting DBName from Path
            dbName = FileInformation.Name;

            try
            {
                DataPrefix=this.DBConnection.DIDataSetDefault();
                this.DBConnection.InsertNewDBFileName(DataPrefix,dbName);
            }
            catch (Exception e)
            { }
        
        
        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- New/Dispose --"

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fileNameWPath"></param>
        public DI6ToDI5Converter(string fileNameWPath)
        {
            this.DBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, fileNameWPath, string.Empty, string.Empty);
            this.DBQueries = new DIQueries(this.DBConnection.DIDataSetDefault(), this.DBConnection.DILanguageCodeDefault("UT_"));

            
            this.FilenameWPath = fileNameWPath;
        }


        #region IDisposable Members
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            if (this.DBConnection != null)
            {
                this.DBConnection.Dispose();
                this.DBQueries.Dispose();


            }
        }

        #endregion

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Converts the database into DevInfo 5.0 format
        /// </summary>
        /// <returns></returns>
        public bool ProcessConversion()
        {
            bool RetVal = false;
           
            
            try
            {
                // Step1: delete version table
                this.DeleteVersiontable();
                
                //Change No. C2

                // Step2: delete language based table: DBMetaData, RecommendedSources
                this.DeleteNewLanguageBasedTable();

                // Step3: remove IC_IUS_Order and IC_IUS_Lable columns from IC_IUS table
                this.RemoveColumnsFrmICIUS();

                // Step4: remove Indicator_nid,subgroup_val_nid, unit_nid from Data table           
                this.UpdateDataTable();

                // Step5: update subgroups
                this.UpdateSubgroups();

                //Step6: Update Details for databasename in database
                this.InsertDbName(FilenameWPath);

                RetVal = true;
            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }

        #endregion

        #endregion


    }
}
