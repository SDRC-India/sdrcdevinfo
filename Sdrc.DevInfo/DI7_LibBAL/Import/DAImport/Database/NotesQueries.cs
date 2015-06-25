using System;
using System.Collections.Generic;
using System.Text;

using System.Data.OleDb;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Queries.Notes;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Database
{
    /// <summary>
    /// Provides collection of SQL queries used for importing Notes from database or template
    /// </summary>
    internal class NotesQueries
    {
        # region " -- Private -- "

        # region " -- Variables -- "

        private DIQueries DBQueries;

        # endregion

        # endregion

        # region " -- Internal -- "

        # region " -- New /Dispose -- "

        internal NotesQueries(DIQueries DBQueries)
        {
            this.DBQueries = DBQueries;
        }


        # endregion

        #region "-- Methods --"

        internal string Import_Prefix()
        {
            string _SQLString = string.Empty;
            _SQLString = "SELECT DB_Available_Databases.AvlDB_Prefix  " + " FROM DB_Available_Databases Where DB_Available_Databases.AvlDB_Default= -1 ";
            return _SQLString;
        }

        internal string GetNotesDataTable(string sTRGTblPrefix)
        {
            string RetVal = string.Empty;
            RetVal = "Select count(*) from " + sTRGTblPrefix + "Notes_Data";
            return RetVal;
        }

        internal string GetDistinctSourceFileFromTempDataTable()
        {
            string RetVal = string.Empty;
            RetVal = "Select Distinct " + Constants.Log.SkippedSourceFileColumnName + " from " + Constants.TempDataTableName; ;
            return RetVal;
        }

        internal string GetDefaultLanguages(bool getAllColumns, string tablePrefix)
        {
            string RetVal = string.Empty;
            if (getAllColumns == true)
            {
                RetVal = "Select * from [" + tablePrefix + "Language]";
            }
            else
            {
                RetVal = "Select " + Language.LanguageName + ", " + Language.LanguageCode + ", " + Language.LanguageDefault + " from [" + tablePrefix + "Language]";
            }
            return RetVal;
        }

        internal string IMP_Update_UTNotesData(string sTblPrefix)
        {
            string RetVal = string.Empty;

            //_SQLString = "UPDATE (" & sTblPrefix & "Notes_Data AS N INNER JOIN _Data ON N.Data_NId = [_Data].Data_NId) " _
            //            & "INNER JOIN " & sTblPrefix & "Data DD ON ([_Data].Source_NId = DD.Source_NId) AND " _
            //            & " ([_Data].FootNote_NId = DD.FootNote_NId) AND " _
            //            & " ([_Data].Area_NId = DD.Area_NId) AND " _
            //            & " ([_Data].TimePeriod_NId = DD.TimePeriod_NId) AND " _
            //            & " ([_Data].IUSNId = DD.IUSNId) SET N.Data_Nid=DD.Data_Nid"
            RetVal = "UPDATE " + sTblPrefix + "Notes_Data AS N INNER JOIN " + "(" + Constants.TempDataTableName + " AS TD INNER JOIN " + sTblPrefix + "Data AS DD ON (TD.IUSNId = DD.IUSNId) AND " + "(TD.TimePeriod_NId = DD.TimePeriod_NId) AND (TD.Area_NId = DD.Area_NId) AND " + " (TD.FootNote_NId = DD.FootNote_NId) AND (TD.Source_NId = DD.Source_NId)) " + " ON N.Data_NId = DD.Data_NId SET N.Data_NId = DD.Data_Nid;";
            return RetVal;
        }
        
        internal string check_language_exists(string lng_code, string lng_code2, bool bDI4, string tablePrefix)
        {
            string RetVal = string.Empty;
            RetVal = "";
            RetVal = "SELECT COUNT(*) as cc FROM ";
            RetVal = RetVal + "[" + tablePrefix + "LANGUAGE] WHERE ";
            // -- check src and trg lng exists or not ,used when importing from a Database/Template file
            if (lng_code2.Length > 0)
            {
                if (bDI4)
                {
                    RetVal = RetVal + " LanguageCode IN ('" + lng_code.ToLower() + "','" + lng_code2.ToLower() + "') ";
                }
                else
                {
                    RetVal = RetVal + " Language_Code IN ('" + lng_code.ToLower() + "','" + lng_code2.ToLower() + "') ";
                }
            }
            else
            {
                if (bDI4)
                {
                    RetVal = RetVal + " LanguageCode ='" + lng_code.ToLower() + "'";
                }
                else
                {
                    RetVal = RetVal + " Language_Code ='" + lng_code.ToLower() + "'";
                }
            }
            return RetVal;
        }

        internal string IMP_Create_NotesTbl()
        {
            return "Select *, 0 as Mapped into [_Notes] FROM lnk_Notes_data";
        }

        internal string IMP_Update_Notes(string sTblPrefix)
        {
            string _SQLString = string.Empty;

            _SQLString = "UPDATE (_Notes AS N INNER JOIN " + Constants.TempDataTableName + " AS TD ON N.Data_NId = TD." + Constants.Old_Data_Nid + ") " + "INNER JOIN " + sTblPrefix + "Data DD ON (TD.Source_NId = DD.Source_NId) AND " + " (TD.FootNote_NId = DD.FootNote_NId) AND " + " (TD.Area_NId = DD.Area_NId) AND " + " (TD.TimePeriod_NId = DD.TimePeriod_NId) AND " + " (TD." + Constants.NewIUSColumnName + " = DD.IUSNId) SET N.Data_Nid=DD.Data_Nid, Mapped=1 ;";
            return _SQLString;
        }

        internal string IMP_Update_AlreadyExistsNotes(string sTblPrefix, string sLngCode)
        {
            string _SQLString = string.Empty;
            _SQLString = "UPDATE (lnk_Notes_Classification AS TCN INNER JOIN " + " (lnk_Notes AS TN INNER JOIN _Notes AS TND ON TN.Notes_Nid = TND.Notes_NId) " + " ON TCN.Classification_Nid = TN.Classification_NId) " + " INNER JOIN lnk_Notes_profile AS TNP ON TN.Profile_NId = TNP.Profile_NId, " + sTblPrefix + "Notes_Data AS ND INNER JOIN (( " + sTblPrefix + "Notes_Profile AS NP INNER JOIN " + sTblPrefix + "Notes" + sLngCode + " AS N ON  " + " NP.Profile_NId = N.Profile_NId) INNER JOIN " + sTblPrefix + "Notes_Classification" + sLngCode + " AS NC ON " + " N.Classification_NId = NC.Classification_Nid) ON ND.Notes_NId = N.Notes_Nid " + " SET TND.MAPPED = 0 " + " WHERE (((TCN.Classification_Name)=[NC].[Classification_Name]) AND ((TN.Notes)=[N].[Notes]) AND ((TNP.Profile_EMail)=[NP].[Profile_Email]) AND ((TND.Data_NId)=[ND].[DATA_NID]))";
            return _SQLString;
        }

        internal string IMP_InsertInNotesDataForDA(string sTblPrefix)
        {
            string _SQLString = string.Empty;
            _SQLString = " INSERT INTO  " + sTblPrefix + "Notes_Data  (DATA_NID,NOTES_NID) " + " SELECT TND.DATA_NID,TN.NEW_NOTES_NID FROM Temp_Notes AS TN " + " INNER JOIN _Notes AS TND ON TN.Notes_Nid = TND.Notes_NId";
            return _SQLString;
        }

        internal string IMP_DeleteUnMatchedNotes()
        {
            string _SQLString = string.Empty;
            _SQLString = "Delete * from _Notes where Mapped <>1";
            return _SQLString;
        }
        
        internal string IMP_DeleteMatchedFrmTempNotes()
        {
            string RetVal = string.Empty;
            RetVal = "DELETE * FROM Temp_Notes AS TN WHERE TN.MAPPED=1;";
            return RetVal;
        }

        internal string IMP_Create_Notes_Data(string sTblPrefix)
        {
            string RetVal = string.Empty;
            RetVal = "Select Notes_Data_Nid, Notes_Nid, Data_Nid into " + sTblPrefix + "Notes_Data  from _Notes ";
            return RetVal;
        }

        internal string IMP_Create_Notes_profile(string sTblPrefix)
        {
            string RetVal = string.Empty;
            RetVal = "Select * into " + sTblPrefix + "Notes_profile  from lnk_Notes_profile ";
            return RetVal;
        }

        internal string IMP_Create_Notes(string sTblPrefix, string sLngCode)
        {
            string RetVal = string.Empty;
            RetVal = "Select * into " + sTblPrefix + "Notes" + sLngCode + " from lnk_Notes ";
            return RetVal;
        }

        internal string IMP_Create_Notes_Classification(string sTblPrefix, string sLngCode)
        {
            string RetVal = string.Empty;
            RetVal = "Select * into " + sTblPrefix + "Notes_Classification" + sLngCode + "  from lnk_Notes_Classification ";
            return RetVal;
        }

        internal string IMP_DeleteUnMatchedFrm_Notes(string sTblPrefix, string sLngCode)
        {
            string RetVal = string.Empty;
            RetVal = " DELETE N.* FROM " + sTblPrefix + "Notes" + sLngCode + " N  where N.Notes_Nid not  in (select distinct ND.Notes_Nid from " + sTblPrefix + "Notes_Data ND)";
            return RetVal;
        }

        internal string IMP_CreateTemp_Notes()
        {
            string RetVal = string.Empty;
            RetVal = "Select *,0 as  New_Notes_Nid, 0 as New_Profile_Nid, 0 as New_Classification_Nid,0 as Mapped into Temp_Notes from lnk_Notes ";
            return RetVal;
        }

        internal string IMP_CreateTemp_Notes_Classification()
        {
            string RetVal = string.Empty;
            RetVal = "Select *, 0 as New_Classification_Nid into Temp_Notes_Classification from lnk_Notes_Classification ";
            return RetVal;
        }

        internal string IMP_CreateTemp_Notes_Profile()
        {
            string RetVal = string.Empty;
            RetVal = "Select *, 0 as New_Profile_Nid into Temp_Notes_Profile from lnk_Notes_profile ";
            return RetVal;
        }

        internal string IMP_DeleteUnMatchedFrm_Temp_Notes()
        {
            string _SQLString = string.Empty;
            _SQLString = " DELETE T.* FROM Temp_Notes T where T.Notes_Nid not in (select distinct ND.Notes_Nid from _Notes ND)";
            return _SQLString;
        }

        internal string IMP_MatchNotes(string sTblPrefix, string sLngCode)
        {
            string _SQLString = string.Empty;
            _SQLString = " UPDATE (Temp_Notes AS TN INNER JOIN Temp_Notes_Classification AS TNC " + " ON TN.Classification_NId = TNC.Classification_Nid) INNER JOIN Temp_Notes_Profile AS " + " TNP ON TN.Profile_NId = TNP.Profile_NId, " + sTblPrefix + "Notes_Profile AS NP INNER JOIN " + " (" + sTblPrefix + "Notes" + sLngCode + " AS N INNER JOIN " + sTblPrefix + "Notes_Classification" + sLngCode + " AS NC ON N.Classification_NId = NC.Classification_Nid) " + " ON NP.Profile_NId = N.Profile_NId SET TN.New_Notes_Nid= N.Notes_Nid , " + " TN.New_Profile_Nid=NP.Profile_Nid," + " TN.New_Classification_Nid=NC.Classification_Nid, " + " TN.Mapped = 1 WHERE (((TN.Notes)=[N].[NOTES]) AND " + " ((TNC.Classification_Name)=[NC].[Classification_Name]) AND " + " ((TNP.Profile_EMail)=[NP].[Profile_Email]));";
            return _SQLString;
        }

        internal string IMP_UpdatedMatchedNotes(string sTblPrefix, string sLngCode)
        {
            string _SQLString = string.Empty;
            _SQLString = " UPDATE Temp_Notes AS TN INNER JOIN " + sTblPrefix + "Notes" + sLngCode + " AS N  " + " ON (TN.New_Notes_Nid = N.Notes_Nid) AND (TN.New_Profile_Nid = N.Profile_NId) " + " AND (TN.New_Classification_Nid = N.Classification_NId)" + " SET N.Notes = TN.Notes where TN.Mapped=1;";
            return _SQLString;
        }

        internal string IMP_InsertNotesProfile(string sTblPrefix)
        {
            string _SQLString = string.Empty;
            _SQLString = " INSERT INTO " + sTblPrefix + "Notes_Profile (profile_name,profile_email,profile_country,profile_org , profile_org_type) " + " SELECT  tnp.profile_name, tnp.profile_email, tnp.profile_country, tnp.profile_org, tnp.profile_org_type FROM  Temp_Notes_Profile TNP " + " WHERE  tnp.profile_email NOT IN (SELECT DISTINCT profile_email from " + sTblPrefix + "Notes_profile ) ";
            return _SQLString;
        }

        internal string IMP_InsertNotesClassification(string sTblPrefix, string sLngCode)
        {
            string _SQLString = string.Empty;
            _SQLString = " Insert into " + sTblPrefix + "Notes_Classification" + sLngCode + "  (Classification_name)  " + " SELECT TNC.Classification_name  FROM  Temp_Notes_Classification TNC " + " WHERE TNC.Classification_name  NOT IN (SELECT DISTINCT  Classification_name from " + sTblPrefix + "Notes_Classification" + sLngCode + ") ";
            return _SQLString;
        }

        internal string IMP_UpdateNewNId_NotesClassification(string sTblPrefix, string sLngCode)
        {
            string _SQLString = string.Empty;
            _SQLString = " UPDATE Temp_Notes_Classification AS TNC INNER JOIN  " + sTblPrefix + "Notes_Classification" + sLngCode + " AS NC " + "ON TNC.Classification_Name = NC.Classification_Name  SET TNC.NEW_Classification_Nid =NC.Classification_Nid ";
            return _SQLString;
        }

        internal string IMP_UpdateNewClassificationNid_TempNotes()
        {
            string _SQLString = string.Empty;
            _SQLString = " UPDATE Temp_Notes AS TN  INNER JOIN   Temp_Notes_Classification AS TNC ON TN.Classification_Nid = TNC.Classification_NId " + " SET TN.NEW_Classification_Nid = TNC.NEW_Classification_NId ";
            return _SQLString;
        }

        internal string IMP_UpdateNewNId_NotesProfile(string sTblPrefix)
        {
            string _SQLString = string.Empty;
            _SQLString = " UPDATE Temp_Notes_Profile AS TNP INNER JOIN  " + sTblPrefix + "Notes_Profile  AS NP " + "ON TNP.PROFILE_Email = NP.PROFILE_Email  SET TNP.NEW_PROFILE_Nid =NP.Profile_Nid ";
            return _SQLString;
        }

        internal string IMP_Update_NewNotesNid(string sTblPrefix, string sLngCode, string dbName)
        {
            string _SQLString = string.Empty;
            _SQLString = " UPDATE " + sTblPrefix + "Notes" + sLngCode + " AS N INNER JOIN Temp_Notes AS TN ON " + "(N.Classification_NId = TN.New_Classification_Nid) AND (N.Profile_NId = TN.New_Profile_Nid) " + " SET TN.New_Notes_Nid = N.Notes_Nid , TN.Mapped=1 ";

            if ((dbName.Trim()).Length > 0)
            {
                _SQLString += " , TN.DATABASENAME = '" + System.IO.Path.GetFileName(dbName) + "'";

            }

            _SQLString += " where N.Notes=TN.notes";

            return _SQLString;
        }

        internal string IMP_UpdateNewProfileNid_TempNotes()
        {
            string _SQLString = string.Empty;
            _SQLString = " UPDATE Temp_Notes AS TN  INNER JOIN   Temp_Notes_Profile AS TNP ON TN.Profile_Nid = TNP.Profile_NId " + " SET TN.NEW_Profile_Nid = TNP.NEW_Profile_NId ";
            return _SQLString;
        }

        internal string IMP_InsertInNotes(string sTblPrefix, string sLngCode)
        {
            string _SQLString = string.Empty;
            //-- query changed on 1Mar,2007: "Notes_Approved" column added
            _SQLString = " Insert into " + sTblPrefix + "Notes" + sLngCode + " ( Profile_Nid,Classification_Nid,Notes, Notes_DateTime,Notes_Approved ) " + " SELECT TN.NEW_Profile_Nid, TN.NEW_Classification_Nid, TN.Notes, TN.Notes_DateTime ,TN.Notes_Approved FROM  Temp_Notes TN " + " WHERE(TN.Mapped = 0) ";
            //and (TN.New_Profile_NID <> TN.Profile_Nid and TN.New_Classification_Nid <> TN.Classification_Nid)"
            return _SQLString;
        }
        
        internal string CreateTempNotesProfileTable(string sourceDBFileName, string sourceNoteDataTable, string targetTempTable)
        {
            string _SQLString = string.Empty;

            _SQLString = " Select * Into " + targetTempTable +
                " from [MS Access;Database=" + sourceDBFileName + ";pwd=" + Constants.DBPassword + ";].[" + sourceNoteDataTable + "]";
            return _SQLString;
        }

        internal string CreateTempNoteDataTable(string sourceDBFileName, string sourceNoteDataTable, string targetTempTable)
        {
            string _SQLString = string.Empty;

            _SQLString = " Select * Into " + targetTempTable +
                " from [MS Access;Database=" + sourceDBFileName + ";pwd=" + Constants.DBPassword + ";].[" + sourceNoteDataTable + "]";
            return _SQLString;
        }

        internal string DropTempNoteData()
        {
            string RetVal = string.Empty;

            RetVal = "Drop table lnk_Notes";
            return RetVal;
        }

        internal string DropTempNoteClassification()
        {
            string RetVal = string.Empty;

            RetVal = " Drop table lnk_Notes_Classification";
            return RetVal;
        }

        # endregion

        # endregion
    }

}
