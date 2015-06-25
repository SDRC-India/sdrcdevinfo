using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.OleDb;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Queries.Notes;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;

using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using System.IO;
using DevInfo.Lib.DI_LibBAL.DA.DML;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Database
{
    /// <summary>
    /// This class provides methods used for inserting recommendedsources from specified Database or Template.
    /// </summary>
    internal class RecommendedSourceImporter
    {
        # region " -- Internal -- "

        # region " -- Variables / Properties -- "

        private DIConnection SourceDBConnection = null;
        private DIQueries SourceDBQueries = null;
        private string SourceDatabaseFolder = string.Empty;

        /// <summary>
        /// Instance of RecommendedQuery Class. Containing all required queries for Notes processing.
        /// </summary>
        private RecommendedSourceQueries RecommendedSrcQuery;


        private DIConnection _TargetDBConnection;
        /// <summary>
        /// Sets target database connection
        /// </summary>
        internal DIConnection TargetDBConnection
        {
            set
            {
                this._TargetDBConnection = value;
            }
        }


        private DIQueries _TargetDBQueries;
        /// <summary>
        /// Sets target database queries
        /// </summary>
        internal DIQueries TargetDBQueries
        {
            set
            {
                this._TargetDBQueries = value;
            }
        }

        # endregion


        # region " -- New /Dipsose -- "
        internal RecommendedSourceImporter(DIConnection targetConnection, DIQueries targetQueries, string sourceDatabaseFolder)
        {
            this._TargetDBConnection = targetConnection;
            this._TargetDBQueries = targetQueries;
            this.SourceDatabaseFolder = sourceDatabaseFolder;
        }

        # endregion

        # region " -- Methods -- "

        internal void ImportRecommendedSources()
        {
            //string SourceDBFile = string.Empty;
            //DataTable TrgTempDataTable = null;

            //try
            //{
            //    this.RecommendedSrcQuery = new RecommendedSourceQueries(this._TargetDBQueries);

            //    //get all source database name from TempDataTable.
            //    DataTable DTSourceFile = this._TargetDBConnection.ExecuteDataTable(this.RecommendedSrcQuery.GetDistinctSourceFileFromTempDataTable());


            //    // get  tempDataTable
            //    TrgTempDataTable = this._TargetDBConnection.ExecuteDataTable("Select * from " + DevInfo.Lib.DI_LibBAL.Import.DAImport.Common.Constants.TempDataTableName + ")");

            //    //for each source database, import recommended sources
            //    foreach (DataRow Drow in DTSourceFile.Rows)
            //    {
            //        // Assign SourceDBfileName.
            //        SourceDBFile = Drow[0].ToString();

            //        // Make Connection with SourceDBFile
            //        this.ConnectToSourceDB(Path.Combine(this.SourceDatabaseFolder, SourceDBFile));

            //        // Import ICIUSLabel from SourceDBFile
            //        if (this.SourceDBConnection != null)
            //        {
            //            this.ImportICIUSLabel(Path.Combine(this.SourceDatabaseFolder, SourceDBFile), ref this.SourceDBConnection, ref this.SourceDBQueries, TrgTempDataTable);
            //        }

            //    }
            //    this._TargetDBConnection.Dispose();
            //    this._TargetDBQueries = null;
            //}
            //catch (Exception ex)
            //{                
            //    throw new ApplicationException(ex.ToString());
            //}
        }

        # endregion

        # endregion


        # region " -- Private-- "

        # region " -- Methods-- "
        private void ConnectToSourceDB(string sourceDBFileNameWPath)
        {
            this.SourceDBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess, string.Empty,
            string.Empty, sourceDBFileNameWPath, string.Empty, string.Empty));
            string DatasetPrefix = this.SourceDBConnection.DIDataSetDefault();
            string LanguageCode = this.SourceDBConnection.DILanguageCodeDefault(DatasetPrefix);
            this.SourceDBQueries = new DIQueries(DatasetPrefix, LanguageCode);
        }

        private void ImportICIUSLabel(string sourceDBFileNameWPath, ref DIConnection sourceDBconnection, ref DIQueries sourceDBQueries, DataTable trgTempDataTable)
        {
            string TargetDBPrefix = string.Empty;
            string sSrcDB_Prefix = string.Empty;
            string SourceDBPrefix = sSrcDB_Prefix;
            string LanguageCode = string.Empty;
            string TableName = string.Empty;
            string sqlString = string.Empty;

            DataView DVLngs = null;
            DataView DvSrc;

            DataTable SrcRecommendedSourceTbl;

            bool IsRecommendedSrcTblExistsInTarget = false;
            
            try
            {                
                TargetDBPrefix = this._TargetDBConnection.DIDataSetDefault();

                sSrcDB_Prefix = TargetDBPrefix;
                SourceDBPrefix = sSrcDB_Prefix;

                //-- Get all Languages from Imp_To.mdb(target Database)
                DVLngs = this._TargetDBConnection.DILanguages(this._TargetDBQueries.DataPrefix).DefaultView;

                //-- check RecommendedSource table exists in target database or not
                IsRecommendedSrcTblExistsInTarget = new RecommendedSourcesBuilder(this._TargetDBConnection, this._TargetDBQueries).IsRecommendedSourcesTableExists();

                if (IsRecommendedSrcTblExistsInTarget)
                {
                    foreach (DataRowView drvLng in DVLngs)
                    {
                        

                        //-- check language exists in source database
                        LanguageCode = drvLng[Language.LanguageCode].ToString();
                        if(sourceDBconnection.IsValidDILanguage(this._TargetDBQueries.DataPrefix, LanguageCode))
                        {
                            this.SourceDBQueries=new DIQueries(this._TargetDBQueries.DataPrefix,LanguageCode);

                            try
                            {
                                // get all records from UT_recommendedSources 
                                SrcRecommendedSourceTbl=this.SourceDBConnection.ExecuteDataTable(this.SourceDBQueries.RecommendedSources.GetRecommendedSources(string.Empty));
                                

                                // insert or update UT_RecommendedSource table in TargetDatabase                                

                            }
                            catch (Exception ex)
                            {
                                //MessageBoxControl.ShowMsg(ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if ((DVLngs != null))
                    DVLngs.Dispose();

                DVLngs = null;

                if (sourceDBconnection != null)
                {
                    this.DisposeSrcConnection(ref sourceDBconnection);
                }
            }

            //-- drop common link table
            try
            {
                this._TargetDBConnection.ExecuteNonQuery("Drop table lnk_Notes_Data");
                this._TargetDBConnection.ExecuteNonQuery("Drop table lnk_Notes_profile");
                this._TargetDBConnection.ExecuteNonQuery("Drop table Temp_Notes_profile");
                this._TargetDBConnection.ExecuteNonQuery("Drop table _Notes");
            }
            catch (Exception)
            {
            }
        }

        private void CreateSrcConnection(ref DIConnection odt_db, string sSrcDB)
        {
            if ((odt_db == null))
            {
                odt_db = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, sSrcDB, string.Empty, string.Empty);
            }
        }
        private void DisposeSrcConnection(ref DIConnection oDT_DB)
        {
            if ((oDT_DB != null))
            {
                oDT_DB.Dispose();
                oDT_DB = null;
            }
        }

        # endregion

        # endregion
    }
}
