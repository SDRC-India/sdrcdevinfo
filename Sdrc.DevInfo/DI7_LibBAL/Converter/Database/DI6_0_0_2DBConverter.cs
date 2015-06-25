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
    /// Helps in converting DevInfo database into DevInfo 6_0_0_2 format
    /// </summary>
    public class DI6_0_0_2DBConverter : DI6DBConverter
    {

        #region --  Private --

        #region --  Methods --

        #region -- version --

        
        private void CreateDBMetaTable(bool forOnlineDB)
        {
            string DataPrefix = string.Empty;
            string LanguageCode = string.Empty;
            DITables TableNames;
            DBMetadataTableBuilder DBMetadataTblBuilder;
            DIQueries TempQueries;
            try
            {
                // create table for all dataset
                foreach (DataRow DataPrefixRow in this._DBConnection.DIDataSets().Rows)
                {
                    DataPrefix = DataPrefixRow[DBAvailableDatabases.AvlDBPrefix].ToString() + "_";

                    // create table for all available langauges
                    foreach (DataRow LanguageRow in this._DBConnection.DILanguages(DataPrefix).Rows)
                    {
                        LanguageCode = "_" + LanguageRow[Language.LanguageCode].ToString();

                        // check table already exists or not
                        TempQueries = new DIQueries(DataPrefix, LanguageCode);
                        DBMetadataTblBuilder = new DBMetadataTableBuilder(this._DBConnection, TempQueries);
                        if (DBMetadataTblBuilder.IsDBMetadataTableExists() == false)
                        {
                            TableNames = new DITables(DataPrefix, LanguageCode);
                            this._DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.DBMetadata.Insert.CreateTable(TableNames.DBMetadata,
                                forOnlineDB, this._DBConnection.ConnectionStringParameters.ServerType));
                        }

                    }

                    // insert blank records with counts
                    // reset DBMetadata builder with main DIQuerie's object
                    DBMetadataTblBuilder = new DBMetadataTableBuilder(this._DBConnection, this._DBQueries);
                    DBMetadataTblBuilder.InsertRecord(string.Empty, string.Empty, string.Empty, string.Empty,
                        string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
                    DBMetadataTblBuilder.GetNUpdateCounts();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }

        }

        #endregion

        #endregion

        #endregion

        #region -- internal/public --


        #region -- New/Dispose --

        public DI6_0_0_2DBConverter(DIConnection dbConnection, DIQueries dbQueries)
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

            DBMetadataTableBuilder DBMetadataBuilder = new DBMetadataTableBuilder(this._DBConnection, this._DBQueries);
            RetVal = DBMetadataBuilder.IsDBMetadataTableExists();

            return RetVal;
        }

        /// <summary>
        /// Converts DevInfo Database into DevInfo6.0.0.2 format
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <returns></returns>
        public override bool DoConversion(bool forOnlineDB)
        {
            bool RetVal = false;
            int TotalSteps = 2;
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
                    VersionBuilder.InsertVersionInfo(Constants.Versions.DI6_0_0_2, Constants.VersionsChangedDates.DI6_0_0_2, Constants.VersionComments.DI6_0_0_2);
                   
                        this.RaiseProcessStartedEvent(TotalSteps);

                        // Step2: create DB_MetaData table
                        this.CreateDBMetaTable(forOnlineDB);
                        this.RaiseProcessInfoEvent(1);


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
