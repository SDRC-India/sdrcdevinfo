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

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Database
{
    /// <summary>
    /// This class provides methods used for inserting Notes from specified Database or Template.
    /// </summary>
    internal class NotesImporter
    {
        # region " -- Internal -- "

        # region " -- Variables / Properties -- "

        private DIConnection SourceDBConnection = null;
        private DIQueries SourceDBQueries = null;
        private string SourceDatabaseFolder = string.Empty;

        /// <summary>
        /// Instance of NOtesQuery Class. Containing all required queries for Notes processing.
        /// </summary>
        private NotesQueries NotesQuery;


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
        internal NotesImporter(DIConnection targetConnection, DIQueries targetQueries, string sourceDatabaseFolder)
        {
            this._TargetDBConnection = targetConnection;
            this._TargetDBQueries = targetQueries;
            this.SourceDatabaseFolder = sourceDatabaseFolder;

             
        }

        # endregion

        # region " -- Methods -- "

        internal void ImportNotes()
        {
            string SourceDBFile = string.Empty;

            this.NotesQuery = new NotesQueries(this._TargetDBQueries);
            //get all source database name from TempDataTable.
            DataTable DTSourceFile = this._TargetDBConnection.ExecuteDataTable(this.NotesQuery.GetDistinctSourceFileFromTempDataTable());
         
                //for each source database, import notes
                foreach (DataRow Drow in DTSourceFile.Rows)
                {
                    // Assign SourceDBfileName.
                    SourceDBFile = Drow[0].ToString();

                    // Make Connection with SourceDBFile
                    this.ConnectToSourceDB(Path.Combine(this.SourceDatabaseFolder, SourceDBFile));

                    // Import Notes from SourceDBFile
                    if (this.SourceDBConnection != null)
                    {
                        this.ImportNotes(Path.Combine(this.SourceDatabaseFolder, SourceDBFile),  ref this.SourceDBConnection, ref this.SourceDBQueries);
                    }
                
            }
            this._TargetDBConnection.Dispose();
            this._TargetDBQueries = null;
        }

        # endregion

        # endregion


        # region " -- Private-- "

        # region " -- Methods-- "
        private void ConnectToSourceDB(string sourceDBFileNameWPath)
        {
            this.SourceDBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess, string.Empty,
            string.Empty, sourceDBFileNameWPath, string.Empty, Constants.DBPassword));
            string DatasetPrefix = this.SourceDBConnection.DIDataSetDefault();
            string LanguageCode = this.SourceDBConnection.DILanguageCodeDefault(DatasetPrefix);
            this.SourceDBQueries = new DIQueries(DatasetPrefix, LanguageCode);
        }

        private void ImportNotes(string sourceDBFileNameWPath, ref DIConnection sourceDBconnection, ref DIQueries sourceDBQueries)
        {
            string TargetDBPrefix = "";
            string sSrcDB_Prefix = "";
            string SourceDBPrefix = sSrcDB_Prefix;
            DataView DVLngs = null;
            DataView DvSrc;
            string LanguageCode = "";
            string TableName = "";
            bool IsMainTableCreated = false;
            string sqlString = "";
            bool IsNotesTblExistsInTarget = false;
            bool IsUpdatedNotesData = false;

            try
            {
                //sourceDBQueries.TablePrefix = sourceDBconnection.ExecuteScalarSqlQuery(this.NotesQuery.Import_Prefix()).ToString();
                TargetDBPrefix = this._TargetDBConnection.DIDataSetDefault();

                sSrcDB_Prefix = TargetDBPrefix;
                SourceDBPrefix = sSrcDB_Prefix;

                //-- Get all Languages from Imp_To.mdb(target Database)
                DVLngs = this._TargetDBConnection.ExecuteDataTable(this.NotesQuery.GetDefaultLanguages(false, TargetDBPrefix)).DefaultView;

                //-- check Notes table exists in target database or not
                IsNotesTblExistsInTarget = this.IsNotesDataTblExists(TargetDBPrefix, ref this._TargetDBConnection);

                if (IsNotesTblExistsInTarget)
                {
                    //-- update Data_Nid in UT_Notes_Data table 
                    sqlString = this.NotesQuery.IMP_Update_UTNotesData(TargetDBPrefix);
                    this._TargetDBConnection.ExecuteNonQuery(sqlString);
                }

                
                foreach (DataRowView drvLng in  DVLngs)
                {

                    //-- check language exists in source database
                    LanguageCode = drvLng[Language.LanguageCode].ToString();
                    if ((int)sourceDBconnection.ExecuteScalarSqlQuery(this.NotesQuery.check_language_exists(LanguageCode, "", false, SourceDBPrefix)) > 0)
                    {

                        try
                        {
                            //-- Step 1. create Temp tables for Notes_Data & Notes_profile :
                            //--------------------------------------------------------------------
                            if (IsMainTableCreated == false)
                            {
                                sourceDBconnection.Dispose();

                                //-- Notes_Data
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.CreateTempNoteDataTable(sourceDBFileNameWPath, sSrcDB_Prefix + "Notes_Data", "lnk_Notes_Data"));
                                //-- Notes_profile
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.CreateTempNotesProfileTable(sourceDBFileNameWPath, sSrcDB_Prefix + "Notes_profile", "lnk_Notes_profile"));
                                //-- create Temp_Notes_Profile table
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_CreateTemp_Notes_Profile());
                                IsMainTableCreated = true;
                                sourceDBconnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, sourceDBFileNameWPath, string.Empty, Constants.DBPassword);
                            }



                            //-- Step 2. create _Notes  table :  ( Select *, 0 as _mapped into [_Notes] FROM lnk_Notes_data)
                            try
                            {
                                this._TargetDBConnection.ExecuteNonQuery("Drop table _Notes");
                            }
                            catch(Exception )
                            {
                            }
                            sqlString = this.NotesQuery.IMP_Create_NotesTbl();
                            this._TargetDBConnection.ExecuteNonQuery(sqlString);

                            //-- Step 3. .UPDATE (_Notes AS N INNER JOIN _Data ON N.Data_NId = [_Data].Data_NId) 
                            //-- INNER JOIN UT_Data ON ([_Data].Source_NId = UT_Data.Source_NId) AND 
                            //-- ([_Data].FootNote_NId = UT_Data.FootNote_NId) AND ([_Data].Area_NId = UT_Data.Area_NId) AND
                            //-- ([_Data].TimePeriod_NId = UT_Data.TimePeriod_NId) AND ([_Data].IUSNId = UT_Data.IUSNId) 
                            //-- SET N.Data_Nid=UT_Data.Data_Nid, Mapped=1 ;
                            sqlString = this.NotesQuery.IMP_Update_Notes(TargetDBPrefix);
                            this._TargetDBConnection.ExecuteNonQuery(sqlString);

                            //-- Step 4. Delete * from _Notes where Mapped <>1
                            sqlString = this.NotesQuery.IMP_DeleteUnMatchedNotes();
                            this._TargetDBConnection.ExecuteNonQuery(sqlString);

                            if (IsNotesTblExistsInTarget == false)
                            {
                                //-- Step 5. if Notes_Data doesn't exists in Target database then create all notes tables                         
                                this.CreateNotesTableForAllLngs(sourceDBFileNameWPath, TargetDBPrefix, sSrcDB_Prefix, ref sourceDBconnection, ref sourceDBQueries);
                                break; // TODO: might not be correct. Was : Exit For
                            }

                            else
                            {   //temp
                                //this.DisposeSrcConnection(ref sourceDBconnection);

                                LanguageCode = "_" + LanguageCode;
                                //-- create lngBsd link tables: lnk_Notes, lnk_Notes_Classification
                                CreateNotesLinkTables(sourceDBFileNameWPath, sSrcDB_Prefix, LanguageCode);

                                //-- create lngBsd temp tables :Temp_Notes, Temp_Notes_Classification
                                CreateTempNotesTables();

                                //-- set mapped to 0 in _Notes table where notes are already exists in UT_Notes_Data table
                                sqlString = this.NotesQuery.IMP_Update_AlreadyExistsNotes(TargetDBPrefix, LanguageCode);
                                this._TargetDBConnection.ExecuteNonQuery(sqlString);

                                //-- Step 4. Delete * from _Notes where Mapped <>1
                                sqlString = this.NotesQuery.IMP_DeleteUnMatchedNotes();
                                this._TargetDBConnection.ExecuteNonQuery(sqlString);

                                //-- delete unmatched records frm Temp_Notes table (which are not in _Notes)
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_DeleteUnMatchedFrm_Temp_Notes());

                                //-- bug fixed on 16Jan,2007: allow to insert duplicate values
                                //-- check Notes with Profile_Email and Classification_Name already exists in target database 
                                //-- and update temp_notes table
                                ////this._TargetDBConnection.ExecuteNonQuery(this._TargetDBQueries.IMP_MatchNotes(sTRGTblPrefix, sLngCode))

                                //-- if exists then overwrite, otherwise create new record
                                //-- update UT_notes_en N set N.Notes= T. where mapped =1 in temp_notes t
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_UpdatedMatchedNotes(TargetDBPrefix, LanguageCode));

                                //-- delete records from Temp_Notes table where mapped =1 (means already exists in target database)
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_DeleteMatchedFrmTempNotes());

                                //-- update Notes_Data and Notes_Profile table
                                //-- Bulk Insert : UT_Notes_Profile_en
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_InsertNotesProfile(TargetDBPrefix));

                                //-- Bulk Insert : UT_Notes_Classification_en
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_InsertNotesClassification(TargetDBPrefix, LanguageCode));

                                //-- insert New_Classification_Nid  in Temp_Notes_Classification and Temp_Notes
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_UpdateNewNId_NotesClassification(TargetDBPrefix, LanguageCode));
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_UpdateNewClassificationNid_TempNotes());

                                //-- insert New_Profile_Nid in Temp_Notes_Profile and Temp_Notes
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_UpdateNewNId_NotesProfile(TargetDBPrefix));
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_UpdateNewProfileNid_TempNotes());


                                //-- Bulk Insert : UT_Notes_en
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_InsertInNotes(TargetDBPrefix, LanguageCode));

                                //-- insert new notes_nid in temp_notes table 
                                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_Update_NewNotesNid(TargetDBPrefix, LanguageCode, ""));

                                if (IsUpdatedNotesData == false)
                                {
                                    //-- bug fixed on 5July,2007: Changed method name in DAQuery (IMP_InsertInNotesDataForDA)
                                    //-- insert new records in UT_Notes_Data table 
                                    this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_InsertInNotesDataForDA(TargetDBPrefix));
                                    IsUpdatedNotesData = true;
                                }

                                //-- delete lnk_Notes, lnk_Notes_Classification, Temp_Notes, Temp_Notes_Classification, Temp_Notes_Profile
                                DropTbls();

                            }
                        }
                        catch (Exception ex)
                        {
                            //MessageBoxControl.ShowMsg(ex);
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
            catch (Exception )
            {
            }
        }

        private void CreateSrcConnection(ref DIConnection odt_db, string sSrcDB)
        {
            if ((odt_db == null))
            {
                odt_db = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty,  sSrcDB, string.Empty, Constants.DBPassword);
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

        private void CreateNotesTableForAllLngs(string sSrcDB, string sTRGTblPrefix, string sSrcDB_Prefix, ref DIConnection sourceDIConnection, ref DIQueries sourceDbQueries)
        {
            DataView dvLngs = null;
            string sLngCode = "";

            try
            {
                //-- create common Notes table 
                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_Create_Notes_Data(sTRGTblPrefix));
                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_Create_Notes_profile(sTRGTblPrefix));

                //-- Get all Languages from Imp_To.mdb(target Database)
                dvLngs = this._TargetDBConnection.ExecuteDataTable(this.NotesQuery.GetDefaultLanguages(false, sTRGTblPrefix)).DefaultView;
                foreach (DataRowView drvLng in dvLngs)
                {
                    //-- check language exists in source database
                    sLngCode = drvLng[Language.LanguageCode].ToString();

                    this.CreateSrcConnection(ref sourceDIConnection, sSrcDB);
                    if ((int)sourceDIConnection.ExecuteScalarSqlQuery(this.NotesQuery.check_language_exists(sLngCode, "", false, sTRGTblPrefix)) > 0)
                    {
                        sLngCode = "_" + sLngCode;
                        this.DisposeSrcConnection(ref sourceDIConnection);

                        //-- create LngBsd Notes link tables
                        this.CreateNotesLinkTables(sSrcDB, sSrcDB_Prefix, sLngCode);

                        //-- Create LngBsd Notes tables : UT_Notes_en, UT_Notes_Classification_en
                        this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_Create_Notes(sTRGTblPrefix, sLngCode));
                        this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_Create_Notes_Classification(sTRGTblPrefix, sLngCode));

                        //-- Delete unmatched records from UT_Notes_en table
                        this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_DeleteUnMatchedFrm_Notes(sTRGTblPrefix, sLngCode));

                        //-- drop link tables
                        DropLngBasedNotesLinkTbl();
                    }
                }

                //-- drop _notes table
                this._TargetDBConnection.ExecuteNonQuery("Drop table _Notes");
            }
            catch (Exception )
            {
            }
            finally
            {
                if ((dvLngs != null))
                    dvLngs.Dispose();

                dvLngs = null;
            }

        }

        private void CreateNotesLinkTables(string sSrcDB, string sSrcDB_Prefix, string sLngCode)
        {
            try
            {
                //--------------------------------------------------------------------
                //-- create LinkTables
                //-- Notes
                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.CreateTempNoteDataTable(sSrcDB, sSrcDB_Prefix + "Notes" + sLngCode, "lnk_Notes"));
                //-- Notes_Classification
                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.CreateTempNoteDataTable(sSrcDB, sSrcDB_Prefix + "Notes_Classification" + sLngCode, "lnk_Notes_Classification"));
            }
            //--------------------------------------------------------------------
            catch (Exception ex)
            {
                //ShowErrorMessage(ex);
            }
        }

        private void CreateTempNotesTables()
        {
            try
            {
                //-- Create Temp LngBsd Notes tables : Temp_Notes, Temp_Notes_Classification
                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_CreateTemp_Notes());
                this._TargetDBConnection.ExecuteNonQuery(this.NotesQuery.IMP_CreateTemp_Notes_Classification());
            }
            catch (Exception ex)
            {
               // Mess (ex);
            }
        }

        private void DropTbls()
        {
            //-- delete lnk_Notes, lnk_Notes_Classification, Temp_Notes, Temp_Notes_Classification
            try
            {
                this._TargetDBConnection.ExecuteNonQuery("Drop table lnk_Notes");
                this._TargetDBConnection.ExecuteNonQuery("Drop table lnk_Notes_Classification");
                this._TargetDBConnection.ExecuteNonQuery("Drop table Temp_Notes");
                this._TargetDBConnection.ExecuteNonQuery("Drop table Temp_Notes_Classification");
            }
            //this._TargetDBConnection.ExecuteNonQuery("Drop table Temp_Notes_Profile")
            catch (Exception ex)
            {
            }
        }

        private void DropLngBasedTempNotesTbl()
        {
            try
            {
                this._TargetDBConnection.ExecuteNonQuery("Drop table Temp_Notes");
                this._TargetDBConnection.ExecuteNonQuery("Drop table Temp_Notes_Classification");
            }
            catch (Exception )
            {
            }
        }

        private void DropLngBasedNotesLinkTbl()
        {
            try
            {
                this._TargetDBConnection.ExecuteNonQuery("Drop table lnk_Notes");
                this._TargetDBConnection.ExecuteNonQuery("Drop table lnk_Notes_Classification");
            }
            catch (Exception )
            {
            }
        }

        private bool IsNotesDataTblExists(string sTRGTblPrefix, ref DIConnection oConnection)
        {
            bool RetVal = false;
            try
            {
                if ((oConnection.ExecuteDataTable(this.NotesQuery.GetNotesDataTable(sTRGTblPrefix)) != null))
                {
                    RetVal = true;
                }
            }
            catch (Exception )
            {
                RetVal = false;
            }
            return RetVal;
        }

        private bool IsNotesTblExists(ref DIConnection oDT_DB, ref DIQueries oDA_Queries)
        {
            string sTblName = "";
            bool bReturn = true;
            string sTablePrefix = "";

            //-- check tables exists in source database
            try
            {
                sTablePrefix = oDT_DB.ExecuteScalarSqlQuery(this.NotesQuery.Import_Prefix()).ToString() + "_";

                //-- Get all Languages
                DataView oDV = oDT_DB.ExecuteDataTable(this.NotesQuery.GetDefaultLanguages( false, sTablePrefix)).DefaultView;
                if (oDV.Count > 0)
                {

                    //-- Check availability of table
                    sTblName = sTablePrefix + "Notes_" + oDV[0]["Language_Code"].ToString();
                    try
                    {
                        if (oDT_DB.ExecuteScalarSqlQuery("SELECT count(*) FROM " + sTblName + " WHERE 1=1") == null)
                        {
                            bReturn = false;
                            goto endoffunc_;
                        }
                    }
                    catch (Exception )
                    {
                        bReturn = false;
                        goto endoffunc_;
                    }

                    bReturn = true;
                }
            }
            catch (Exception )
            {
                bReturn = false;
            }
        endoffunc_:
            return bReturn;
        }

        # endregion

        # endregion
    }
}
