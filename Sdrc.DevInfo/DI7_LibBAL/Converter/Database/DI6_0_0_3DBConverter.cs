using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using DevInfo.Lib.DI_LibDAL;
using DALQueries = DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Converter.Database;

namespace DevInfo.Lib.DI_LibBAL.Converter.Database
{
    /// <summary>
    /// Helps in converting DevInfo database into DevInfo 6_0_0_3 format
    /// </summary>
    public class DI6_0_0_3DBConverter : DI6_0_0_2DBConverter
    {

        #region --  Private --

        #region --  Methods --

        #region -- version --


        private void CreateNUpdateRecommendedSourcesTable(bool forOnlineDB)
        {
            string DataPrefix = string.Empty;
            string LanguageCode = string.Empty;
            DITables TableNames;
            RecommendedSourcesBuilder RecommendedSourcesTblBuilder;
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
                        RecommendedSourcesTblBuilder = new RecommendedSourcesBuilder(this._DBConnection, TempQueries);

                        if (RecommendedSourcesTblBuilder.IsRecommendedSourcesTableExists() == false)
                        {
                            TableNames = new DITables(DataPrefix, LanguageCode);
                            this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.RecommendedSources.Insert.CreateTable(TableNames.RecommendedSources,
                                forOnlineDB, this._DBConnection.ConnectionStringParameters.ServerType));

                            // step2: insert IC_IUS_Label into recommendedsource table from IC_IUS table
                            this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.RecommendedSources.Insert.InsertAllLabelFrmICIUS(TableNames));
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        private void AddICIUSOrderColumn(bool forOnlineDB)
        {
            try
            {
                // add ICIUSOrder Column in UT_Data table
                this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Insert.InsertICIUSOrderColumn(this._DBQueries.DataPrefix, forOnlineDB,this._DBConnection.ConnectionStringParameters.ServerType));

            }
            catch (Exception ex)
            {                
                throw new ApplicationException(ex.ToString());
            }
        }

        private void UpdateICIUSOrderInDataTable()
        {
            try
            {
                // Update ICIUSORder into data table from IC_IUS table
                this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Data.Update.UpdateICIUSOrderIntoDataTableFrmICIUS(this._DBQueries.TablesName));
            }
            catch(Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        #endregion

        #endregion

        #endregion

        #region -- internal/public --


        #region -- New/Dispose --

        public DI6_0_0_3DBConverter(DIConnection dbConnection, DIQueries dbQueries)
            : base(dbConnection, dbQueries)
        {
            //donothing
        }

        #endregion


        #region -- Methods --
        /// <summary>
        /// Returns true/false. True if Database is in valid format otherwise false.
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public override bool IsValidDB(bool forOnlineDB)
        {
            bool RetVal = false;

            // check RecommendedSources table exists or  not
            RecommendedSourcesBuilder RecommendedSrcBuilder = new RecommendedSourcesBuilder(this._DBConnection, this._DBQueries);
            RetVal = RecommendedSrcBuilder.IsRecommendedSourcesTableExists();

            return RetVal;
        }

        /// <summary>
        /// Converts DevInfo Database into DevInfo6.0.0.3 format
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public override bool DoConversion(bool forOnlineDB)
        {
            bool RetVal = false;
            int TotalSteps = 4;
            DBVersionBuilder VersionBuilder;

            //do the conversion only if database has different shcema
            try
            {
                if (!this.IsValidDB(forOnlineDB))
                {
                    if (!base.IsValidDB(forOnlineDB))
                    {
                        RetVal = base.DoConversion(forOnlineDB);
                    }

                    // Step1: insert version info into database
                    VersionBuilder = new DBVersionBuilder(this._DBConnection, this._DBQueries);
                    VersionBuilder.InsertVersionInfo(Constants.Versions.DI6_0_0_3, Constants.VersionsChangedDates.DI6_0_0_3, Constants.VersionComments.DI6_0_0_3);

                    this.RaiseProcessStartedEvent(TotalSteps);

                    // Step2: create Recommended sources table and insert IC_IUS_Label into RecommendedSources table from IC_IUS table
                    this.CreateNUpdateRecommendedSourcesTable(forOnlineDB);
                    this.RaiseProcessInfoEvent(1);

                    // Step3: add ICIUSOrder column in UT_Data table
                    this.AddICIUSOrderColumn(forOnlineDB);
                    this.RaiseProcessInfoEvent(2);

                    // Step4: update ICIUSOrder into UT_Data table from IC_IUS table
                    this.UpdateICIUSOrderInDataTable();
                    this.RaiseProcessInfoEvent(3);

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
                RetVal = false;
            }

            return RetVal;
        }

        #endregion

        #endregion
    }

}
