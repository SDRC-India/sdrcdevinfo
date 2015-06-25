using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using System.Data;
using System.Data.Common;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public class NotesDataBuilder
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
        
        public NotesDataBuilder(DIConnection dbConnection,DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Insert Notes Data and return Notes_Data_Nid
        /// </summary>
        /// <param name="notesDataInfo"></param>
        /// <returns></returns>
        public int InserNotesData(NotesDataInfo notesDataInfo)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            
            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Notes.Insert.InsertNotesData(this.DBQueries.TablesName.NotesData, notesDataInfo.Notes_NId, notesDataInfo.Data_NId );

                this.DBConnection.ExecuteNonQuery(SqlQuery);
                RetVal = this.DBConnection.GetNewId();
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);                
            }
            return RetVal;
        }

        /// <summary>
        /// Update Notes data
        /// </summary>
        /// <param name="notesDataInfo"></param>
        public int UpdateNotesData(NotesDataInfo notesDataInfo)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Notes.Update.UpdateNotesData(this.DBQueries.TablesName.NotesData, notesDataInfo.Notes_Data_NId, notesDataInfo.Notes_NId, notesDataInfo.Data_NId);

                RetVal = this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        /// Delete Notes Data for NotesNId
        /// </summary>
        /// <param name="notesNId">Comma Seperated Notes_NId</param>
        public int DeleteNotesData(string notesNId)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;

            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Notes.Delete.DeleteFrmNotesData(this.DBQueries.TablesName.NotesData,notesNId);

                RetVal=this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        /// Get NotesDataInfo for a Notes_Data_nid
        /// </summary>
        /// <param name="notesDataNid"></param>
        /// <returns></returns>
        public NotesDataInfo GetNotesDataInfo(int notesDataNid)
        {
            NotesDataInfo RetVal = null;
            string SqlQuery = string.Empty;
            DbDataReader DBReader = null;
            try
            {
                SqlQuery = this.DBQueries.Notes.GetNoteData(notesDataNid.ToString());
                DBReader = (DbDataReader)this.DBConnection.ExecuteReader(SqlQuery);
                if (DBReader.HasRows)
                {
                    RetVal = new NotesDataInfo();
                    while (DBReader.Read())
                    {
                        RetVal.Notes_Data_NId = (int)DBReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Data.NotesDataNId];
                        RetVal.Notes_NId = Convert.ToInt32(DBReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Data.NotesNId]);
                        RetVal.Data_NId = Convert.ToInt32(DBReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Data.DataNId]);
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            finally 
            {
                if (!DBReader.IsClosed) { DBReader.Close(); }
            } 
            return RetVal;

        }

        /// <summary>
        /// Get DataTable of Notes Data for DataNid or ProfileNid or classificationNid and for approved or all Comments Data
        /// </summary>
        /// <returns></returns>
        public DataTable GetNotesData(string notesNId, string dataNId, string profileNIds, string classificationNIds, CheckedStatus notesApproved)
        {
            DataTable RetVal = null;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.DBQueries.Notes.GetNotes_Data( notesNId,  dataNId,  profileNIds,  classificationNIds,  notesApproved);

                RetVal = this.DBConnection.ExecuteDataTable(SqlQuery);
                
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        #endregion

        #endregion

    }
}
