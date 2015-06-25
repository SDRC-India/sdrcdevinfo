using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using System.Data.Common;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;


namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public class NotesBuilder
    {


        #region "-- private --"

        #region "-- Variables --"
        
        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion
        
        #region "-- Methods --"

        #endregion

        #endregion

        #region "-- Public / Friend --"
               
        #region "-- Variables and Properties --"

        #endregion

        #region "-- New/Dispose --"
        
        public NotesBuilder (DIConnection dbConection,DIQueries dbQueries )
        {
            this.DBConnection = dbConection;
            this.DBQueries = dbQueries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Insert Commnets For Each Language
        /// </summary>
        /// <param name="commentsInfo">Object of CommentsInfo</param>
        public void InsertNotes(NotesInfo commentsInfo)
        {
            string LanguageCode=string.Empty;
            string DefaultLanguageCode=string.Empty;
            string Comments=string.Empty;
            string SqlQuery = string.Empty;
            string NotesTableName=string.Empty;

            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;

                foreach (DataRow languageRow in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {

                    LanguageCode = languageRow[Language.LanguageCode].ToString();

                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    {
                        Comments = commentsInfo.Notes;
                    }
                    else
                    {
                        Comments = Constants.PrefixForNewValue + commentsInfo.Notes;
                    }

                    NotesTableName = this.DBQueries.TablesName.Notes.Replace(DefaultLanguageCode, "_" + LanguageCode);

                    SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Notes.Insert.InsertNotes(NotesTableName, Comments , commentsInfo.Classification_NId.ToString(), commentsInfo.Profile_NId.ToString(), commentsInfo.Notes_DateTime, commentsInfo.Notes_Approved);

                    this.DBConnection.ExecuteNonQuery(SqlQuery);
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
        }
        
        /// <summary>
        /// Update Comments in each language
        /// </summary>
        /// <param name="commentsInfo"></param>
        public int UpdateComments(NotesInfo commentsInfo)
        {
            int RetVal=0;
            string SqlQuery = string.Empty;
            DITables TableNames;
            try
            {

                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Notes.Update.UpdateNotes(this.DBQueries.TablesName.Notes, commentsInfo.Notes, commentsInfo.Classification_NId.ToString(), commentsInfo.Notes_Approved, commentsInfo.Notes_NId);

                RetVal = this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            return RetVal;
        }
        
        /// <summary>
        /// Get CommentsInfo for CommentsNId
        /// </summary>
        /// <param name="notesNid"></param>
        /// <returns></returns>
        public NotesInfo GetNotesByNotesNid(string notesNid)
        {
            NotesInfo RetVal=null;
            DbDataReader DataReader=null;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.DBQueries.Notes.GetNotes(String.Empty, notesNid, string.Empty, string.Empty, CheckedStatus.False, FieldSelection.Light);

                DataReader = (DbDataReader)this.DBConnection.ExecuteReader(SqlQuery);
                if (DataReader.HasRows)
                {
                    RetVal = new NotesInfo();
                    while (DataReader.Read())
                    {
                        RetVal.Notes_NId = (int)DataReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes.NotesNId];
                        RetVal.Profile_NId = (int)DataReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes.ProfileNId];
                        RetVal.Classification_NId = (int)DataReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes.ClassificationNId];
                        RetVal.Notes = Convert.ToString(DataReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes.Note]);
                        RetVal.Notes_DateTime = Convert.ToString(DataReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes.NotesDateTime]);
                        RetVal.Notes_Approved = (int)DataReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes.NotesApproved];
                    }
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            finally 
            {
                if (!DataReader.IsClosed)
                    DataReader.Close();
            }
            return RetVal;
        }

        /// <summary>
        /// Get CommentsInfo for CommentsNId
        /// </summary>
        /// <param name="notesNid"></param>
        /// <returns></returns>
        public DataTable GetNotesByNotesNid(string dataNIds, string notesNIds, string profileNIds, string classificationNIds, CheckedStatus NotesApproved, FieldSelection fieldSelection)
        {
            DataTable RetVal = null;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.DBQueries.Notes.GetNotes( dataNIds, notesNIds, profileNIds, classificationNIds, NotesApproved, FieldSelection.Heavy );

                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
           
            return RetVal;
        }

        /// <summary>
        /// Delete Comments by Notes_Nids from each language
        /// </summary>
        /// <param name="notesNids">Comma Seperated Nids</param>
        public int DeleteComments(string notesNids)
        {
            int RetVal = 0;
            string LanguageCode = string.Empty;
            DITables TableNames;
            string SqlQuery = string.Empty;
            try
            {
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode =Convert.ToString( Row[Language.LanguageCode]);

                    TableNames = new DITables(this.DBQueries.DataPrefix, "_" + LanguageCode);

                    RetVal=this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.Notes.Delete.DeleteFrmNotes(TableNames.Notes, notesNids));
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            return RetVal;
        }

        #endregion

        #endregion

    }
}
