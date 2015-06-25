using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Notes
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Returns query to create UT_notes_Data table
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <param name="tablePrefix"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string CreateNotesData(bool forOnlineDB, string tablePrefix, DIServerType serverType)
        {
            string RetVal = string.Empty;
            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal = "CREATE TABLE " + tablePrefix + "Notes_Data (Notes_Data_NId int(4) NOT NULL AUTO_INCREMENT,PRIMARY KEY (Notes_Data_NId),Notes_NId int(4),Data_NId int(4))";
                }
                else
                {
                    RetVal = "CREATE TABLE " + tablePrefix + "Notes_Data (Notes_Data_NId int Identity(1,1) primary key,Notes_NId int,Data_NId int)";
                }
            }
            else
            {
                RetVal = "CREATE TABLE " + tablePrefix + "Notes_Data (Notes_Data_NId counter primary key,Notes_NId number,Data_NId number)";

            }
            return RetVal;
        }

        /// <summary>
        /// Returns query to create UT_CreateProfile table
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <param name="tablePrefix"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string CreateProfile(bool forOnlineDB, string tablePrefix, DIServerType serverType)
        {
            string RetVal = string.Empty;
            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal = "CREATE TABLE " + tablePrefix + "Notes_Profile (Profile_NId int(4) NOT NULL AUTO_INCREMENT,PRIMARY KEY (Profile_NId),Profile_Name varchar(100),Profile_EMail varchar(150),Profile_Country varchar(100),Profile_Org varchar(100),Profile_Org_Type varchar(50))";
                }
                else
                {
                    RetVal = "CREATE TABLE " + tablePrefix + "Notes_Profile (Profile_NId int Identity(1,1) primary key,Profile_Name varchar(100),Profile_EMail varchar(150),Profile_Country varchar(100),Profile_Org varchar(100),Profile_Org_Type varchar(50))";
                }
            }

            else
            {
                RetVal = "CREATE TABLE " + tablePrefix + "Notes_Profile (Profile_NId counter primary key,Profile_Name Text(100),Profile_EMail Text(150),Profile_Country text(100),Profile_Org Text(100),Profile_Org_Type Text(50))";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns a query to create Notes table
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <param name="notesTableName"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string CreateNotes(bool forOnlineDB, string notesTableName, DIServerType serverType)
        {
            string RetVal = string.Empty;

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal = "CREATE TABLE " + notesTableName + " (Notes_Nid int(4) NOT NULL AUTO_INCREMENT ,PRIMARY KEY (Notes_Nid),Profile_NId int(4), Classification_NId int(4),Notes LongText,Notes_DateTime DateTime,Notes_Approved TINYINT)";
                }
                else
                {
                    RetVal = "CREATE TABLE " + notesTableName + " (Notes_Nid int Identity(1,1) primary key,Profile_NId int, Classification_NId int, Notes nvarchar(100),Notes_DateTime DateTime ,Notes_Approved bit)";
                }
            }
            else
            {
                RetVal = "CREATE TABLE " + notesTableName + " (Notes_Nid counter primary key,Profile_NId number, Classification_NId number, Notes memo,Notes_DateTime DateTime,Notes_Approved Bit)";
            }
            return RetVal;

        }

        /// <summary>
        /// Returns a query to create Notes_Classification table
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <param name="notesClassificationTableName"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string CreateClassification(bool forOnlineDB, string notesClassificationTableName, DIServerType serverType)
        {
            string RetVal = string.Empty;

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal = "CREATE TABLE " + notesClassificationTableName + " (Classification_Nid int(4) NOT NULL AUTO_INCREMENT,PRIMARY KEY (Classification_Nid),Classification_Name varchar(255))";
                }
                else
                {
                    RetVal = "CREATE TABLE " + notesClassificationTableName + " (Classification_Nid int Identity(1,1) primary key,Classification_Name varchar(255))";
                }
            }

            else
            {
                RetVal = "CREATE TABLE " + notesClassificationTableName + " (Classification_Nid counter primary key,Classification_Name Text(150))";
            }

            return RetVal;
        }
        
        /// <summary>
        /// Get Query to Insert Notes Into Notes Table
        /// </summary>
        /// <param name="notesTable">Notes TAble Like UT_Notes_en </param>
        /// <param name="comments"></param>
        /// <param name="classificationNid"></param>
        /// <param name="profileNid"></param>
        /// <param name="notesDate"></param>
        /// <param name="isApproved"></param>
        /// <returns></returns>
        public static string InsertNotes(string notesTable,string comments,string classificationNid,string profileNid, string notesDate,int isApproved)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO  " + notesTable + "(" + DIColumns.Notes.Note + "," + DIColumns.Notes.ClassificationNId + "," 
                    + DIColumns.Notes.ProfileNId + "," + DIColumns.Notes.NotesDateTime + "," + DIColumns.Notes.NotesApproved
                    + ") VALUES('" + comments + "'," + classificationNid + "," + profileNid + ",'" + notesDate + "'," + isApproved + ")";
            
            return RetVal;

        }

        /// <summary>
        /// Get Query For Insert Notes Profile Details
        /// </summary>
        /// <param name="notesProfileTable">oteProfile Table Name</param>
        /// <param name="profileName"></param>
        /// <param name="profileEMail"></param>
        /// <param name="profileCountry"></param>
        /// <param name="profileOrg"></param>
        /// <param name="profileOrgType"></param>
        /// <returns></returns>
        public static string InsertNotesProfiles(string notesProfileTable, string profileName, string profileEMail, string profileCountry, string profileOrg, string profileOrgType)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO  " + notesProfileTable + "(" + DIColumns.Notes_Profile.ProfileName + "," + DIColumns.Notes_Profile.ProfileEMail + ","
                    + DIColumns.Notes_Profile.ProfileCountry  + "," + DIColumns.Notes_Profile.ProfileOrg  + "," + DIColumns.Notes_Profile.ProfileOrgType 
                    + ") VALUES('" + profileName + "','" + profileEMail + "','" + profileCountry + "','" + profileOrg  + "','" + profileOrgType + "')";

            return RetVal;

        }

        /// <summary>
        /// Get Query To Insert Notes Data Details
        /// </summary>
        /// <param name="notesProfileTable">NoteData Table Name</param>
        /// <param name="profileName"></param>
        /// <param name="profileEMail"></param>
        /// <param name="profileCountry"></param>
        /// <param name="profileOrg"></param>
        /// <param name="profileOrgType"></param>
        /// <returns></returns>
        public static string InsertNotesData(string notesDataTable, int notesNid, int dataNid )
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO  " + notesDataTable + "(" + DIColumns.Notes_Data.NotesNId + "," + DIColumns.Notes_Data.DataNId 
                    + ") VALUES(" + notesNid + "," + dataNid + ")";

            return RetVal;

        }

        /// <summary>
        /// Get Query To Insert Notes Classification
        /// </summary>
        /// <param name="notesClassificationTable">NotesClassifaication Table Name</param>
        /// <param name="classificationName">Notes ClassificationName</param>
       /// <returns></returns>
        public static string InsertNotesClassification(string notesClassificationTable, string classificationName)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO  " + notesClassificationTable + "(" + DIColumns.Notes_Classification.ClassificationName + ") VALUES('" + classificationName + "')";

            return RetVal;

        }

        #endregion

        #endregion
    }
}
