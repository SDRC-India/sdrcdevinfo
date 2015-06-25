using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Data;
using System.Data.Common;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public class NotesClassificationBuilder
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

        public NotesClassificationBuilder(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Insert Notes Classification by NotesClassificationInfo
        /// </summary>
        /// <param name="notesClassInfo">NotesClassificationInfo Object</param>
        /// <returns >Return Inserted Notes Nid</returns>
        public int InsertNotesClassification(NotesClassificationInfo notesClassInfo)
        {
            int RetVal = 0;
            DITables TableNames;
            string LanguageCode = string.Empty;
            string DefaultLanguageCode = string.Empty;
            string ClassName = string.Empty;
            string SqlQuery = string.Empty;
            try
            {
                DefaultLanguageCode = this.DBQueries.LanguageCode;
                //-- Insert Classification for Each Language
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = Convert.ToString(Row[Language.LanguageCode]);

                    if (LanguageCode == DefaultLanguageCode.Replace("_", String.Empty))
                    {
                        ClassName = notesClassInfo.Classification_Name;
                    }
                    else
                    {
                        ClassName = Constants.PrefixForNewValue + notesClassInfo.Classification_Name;
                    }

                    TableNames = new DITables(this.DBQueries.DataPrefix, "_" + LanguageCode);

                    SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Notes.Insert.InsertNotesClassification(TableNames.NotesClassification, ClassName);

                    this.DBConnection.ExecuteNonQuery(SqlQuery);
                }

                RetVal = this.DBConnection.GetNewId();

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        ///Update Notes Classification 
        /// </summary>
        /// <param name="notesClassInfo"></param>
        /// <returns >Count Of Records Updated</returns>
        public int UpdateNotesClassification(NotesClassificationInfo notesClassInfo)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Notes.Update.UpdateNotesClassification(this.DBQueries.TablesName.NotesClassification, notesClassInfo.Classification_NId, notesClassInfo.Classification_Name);

                RetVal = this.DBConnection.ExecuteNonQuery(SqlQuery);

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        /// Delete Notes Classificaton by Classification Nid
        /// </summary>
        /// <param name="classificationNids">Comma separated NIDs</param>
        /// <returns >Count Of Records Updated</returns>
        public int DeleteNotesClassification(string classificationNids)
        {
            int RetVal = 0;
            string LanguageCode = string.Empty;
            DITables TableNames;
            string SqlQuery = string.Empty;
            string AssociatedNotesNids=string.Empty;

            NotesBuilder NoteBuilder=new NotesBuilder(this.DBConnection,this.DBQueries);

            try
            {
                foreach (DataRow Row in this.DBConnection.DILanguages(this.DBQueries.DataPrefix).Rows)
                {
                    LanguageCode = Convert.ToString(Row[Language.LanguageCode]);

                    TableNames = new DITables(this.DBQueries.DataPrefix, "_" + LanguageCode);

                    SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Notes.Delete.DeleteFrmNotesClassification(TableNames.NotesClassification, classificationNids);
                    RetVal = this.DBConnection.ExecuteNonQuery(SqlQuery);

                    AssociatedNotesNids = DIConnection.GetDelimitedValuesFromDataTable(NoteBuilder.GetNotesByNotesNid(string.Empty, string.Empty, string.Empty, classificationNids, CheckedStatus.All,FieldSelection.Light), Notes.NotesNId);

                    NoteBuilder.DeleteComments(AssociatedNotesNids);
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }
            return RetVal;
        }

        /// <summary>
        /// Get ClassificationInfo for ClassificationNids
        /// </summary>
        /// <param name="classificationNids"></param>
        /// <returns></returns>
        public List<NotesClassificationInfo> GetNotesClassification(string classificationNids)
        {
            List<NotesClassificationInfo> RetVal = null;
            NotesClassificationInfo ClassificationInfo;
            DbDataReader DataReader = null;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.DBQueries.Notes.GetAllNotesClassification(classificationNids);

                DataReader = (DbDataReader)this.DBConnection.ExecuteReader(SqlQuery);
                if (DataReader.HasRows)
                {
                    RetVal = new List<NotesClassificationInfo>();

                    while (DataReader.Read())
                    {
                        ClassificationInfo = new NotesClassificationInfo();
                        ClassificationInfo.Classification_NId = (int)DataReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Classification.ClassificationNId];
                        ClassificationInfo.Classification_Name = Convert.ToString(DataReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Classification.ClassificationName]);

                        RetVal.Add(ClassificationInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (!DataReader.IsClosed)
                    DataReader.Close();
            }

            return RetVal;
        }


        /// <summary>
        /// Get ClassificationInfo for Classification name
        /// </summary>
        /// <param name="classificationNids"></param>
        /// <returns></returns>
        public NotesClassificationInfo GetNotesClassificationNidBYName(string classificationName)
        {
            NotesClassificationInfo RetVal = null;
            DbDataReader DataReader = null;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this.DBQueries.Notes.GetNotesClassificationBYName(classificationName);

                DataReader = (DbDataReader)this.DBConnection.ExecuteReader(SqlQuery);
                if (DataReader.HasRows)
                {
                    RetVal = new NotesClassificationInfo();
                    while (DataReader.Read())
                    {
                        RetVal.Classification_NId = (int)DataReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Classification.ClassificationNId];
                        RetVal.Classification_Name = Convert.ToString(DataReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Classification.ClassificationName]);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (!DataReader.IsClosed)
                    DataReader.Close();
            }
            return RetVal;
        }


        #endregion

        #endregion

    }
}
