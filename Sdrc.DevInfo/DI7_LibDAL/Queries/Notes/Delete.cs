using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Notes
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public static class Delete
    {
        
        /// <summary>
        /// Returns query to delete notes data
        /// </summary>
        /// <param name="TableName">like UT_Note_en</param>
        /// <param name="nids">Comma separated NotesNId which may be blank</param>
        /// <returns></returns>
        public static string DeleteFrmNotes(string TableName, string nids)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName;

            if (!string.IsNullOrEmpty(nids))
            {

                RetVal = RetVal + " where " + DIColumns.Notes.NotesNId + " IN( " + nids + " )";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns query to delete notes
        /// </summary>
        /// <param name="TableName">like UT_Note_Data</param>
        /// <param name="dataNIds">Comma separated DataNids</param>
        /// <returns></returns>
        public static string DeleteFrmNotesData(string TableName, string notesNId)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName ;

            if (!string.IsNullOrEmpty(notesNId))
                {
                    RetVal = RetVal + " WHERE " + DIColumns.Notes_Data.NotesNId + " IN( " + notesNId + " )";
                }
            return RetVal;
        }

        /// <summary>
        /// Returns query to delete notes
        /// </summary>
        /// <param name="TableName">like UT_Note_Data</param>
        /// <param name="NIds">Comma separated DataNids</param>
        /// <returns></returns>
        public static string DeleteFrmNotesDataByDataNIds(string TableName, string NIds)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName;

            if (!string.IsNullOrEmpty(NIds))
            {
                RetVal = RetVal + " WHERE " + DIColumns.Notes_Data.DataNId + " IN( " + NIds + " )";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns query to delete records from Notes_Classification
        /// </summary>
        /// <param name="TableName">like UT_Note_Classification_en</param>
        /// <param name="classificationNIDs ">Comma separated nids which may be blank</param>
        /// <returns></returns>
        public static string DeleteFrmNotesClassification(string TableName, string classificationNIDs)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName ;
                if(!string.IsNullOrEmpty(classificationNIDs))
                {
                    RetVal = RetVal + " WHERE " + DIColumns.Notes_Classification.ClassificationNId + " IN( " + classificationNIDs + " )";
                }

            return RetVal;
        }


        /// <summary>
        /// Returns query to delete records from Notes_Profile
        /// </summary>
        /// <param name="TableName">like UT_Note_Profile</param>
        /// <param name="profileNIDs">Comma separated nids which may be blank</param>
        /// <returns></returns>
        public static string DeleteFrmNotesProfile(string TableName, string profileNIDs)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + TableName;
            if (!string.IsNullOrEmpty(profileNIDs))
            {
             RetVal  = RetVal    + " WHERE " + DIColumns.Notes_Profile.ProfileNId + " IN( " + profileNIDs + " )";
            }
            return RetVal;
        }
    }
}
