using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Notes
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {
        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Returns alter query to insert Notes_Approved column into notes table
        /// </summary>
        /// <param name="notesTableName"></param>
        /// <returns></returns>
        public static string AlterTable(string notesTableName)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + notesTableName  + " ADD COLUMN Notes_Approved BIT 0";

            return RetVal;
        }

        /// <summary>
        /// Update Notes Tables for NotesNid
        /// </summary>
        /// <param name="notesTableName">Notes Table Name with DatPrefix and Languagecode like UT_Notes_en </param>
        /// <param name="notes">Coments</param>
        /// <param name="classificationNId"></param>
        /// <param name="isApproved"></param>
        /// <param name="notesNid"></param>
        /// <returns></returns>
        public static string UpdateNotes(string notesTableName, string comments, string classificationNId,int isApproved, int notesNid)
        {
            string RetVal = string.Empty;
            
            RetVal = "UPDATE " + notesTableName + " SET " + DIColumns.Notes.Note + " = '" + comments + "',"
                + DIColumns.Notes.ClassificationNId + "= " + classificationNId + ", " + DIColumns.Notes.NotesApproved + "= " + isApproved
                + " WHERE " + DIColumns.Notes.NotesNId + "=" + notesNid;

            return RetVal;
        }

        /// <summary>
        /// Get Query to Update Notes Profiles
        /// </summary>
        /// <param name="notesProfileTable">e.g- UT_Notes_Profile</param>
        /// <param name="notesProfileNid"></param>
        /// <param name="profileName"></param>
        /// <param name="profileEMail"></param>
        /// <param name="profileCountry"></param>
        /// <param name="profileOrg"></param>
        /// <param name="profileOrgType"></param>
        /// <returns></returns>
        public static string UpdateNotesProfiles(string notesProfileTable,int notesProfileNid,string profileName, string profileEMail, string profileCountry, string profileOrg, string profileOrgType)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + notesProfileTable + " SET " + DIColumns.Notes_Profile.ProfileName + " = '" + profileName + "',"
                + DIColumns.Notes_Profile.ProfileEMail + "= '" + profileEMail + "', " + DIColumns.Notes_Profile.ProfileCountry + "= '" 
                + profileCountry + "', " + DIColumns.Notes_Profile.ProfileOrg + "= '" + profileOrg + "', " + DIColumns.Notes_Profile.ProfileOrgType + "= '" + profileOrgType 
                + "' WHERE " + DIColumns.Notes_Profile.ProfileNId + "=" + notesProfileNid;

            return RetVal;
        }
        
        /// <summary>
        /// Get Query to Update Notes data
        /// </summary>
        /// <param name="notesDataTable"></param>
        /// <param name="notesDataNid"></param>
        /// <param name="notesNid"></param>
        /// <param name="dataNid"></param>
        /// <returns></returns>
        public static string UpdateNotesData(string notesDataTable,int notesDataNid, int notesNid, int dataNid)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + notesDataTable + " SET " + DIColumns.Notes_Data.NotesNId + " = " + notesNid + ","
                + DIColumns.Notes_Data.DataNId + "= " + dataNid + " WHERE " + DIColumns.Notes_Data.NotesDataNId + "=" + notesDataNid;

            return RetVal;
        }

        /// <summary>
        ///  Get Query to Update Notes Classification
        /// </summary>
        /// <param name="notesClassificationTable"></param>
        /// <param name="classificationNid"></param>
        /// <param name="classificationName"></param>
        /// <returns></returns>
        public static string UpdateNotesClassification(string notesClassificationTable, int classificationNid,string classificationName)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + notesClassificationTable + " SET " + DIColumns.Notes_Classification.ClassificationName + " = '" + classificationName + "' WHERE " + DIColumns.Notes_Classification.ClassificationNId + "=" + classificationNid;

            return RetVal;
        }

        
        #endregion

        #endregion
    }
}
