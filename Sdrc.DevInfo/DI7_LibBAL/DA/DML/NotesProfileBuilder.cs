using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using System.Data.Common;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public class NotesProfileBuilder
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

        public NotesProfileBuilder(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Insert Notes Profiles 
        /// </summary>
        /// <param name="notesProfilesInfo"></param>
        /// <returns></returns>
        public int InsertNotesProfile(NotesProfilesInfo notesProfilesInfo)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            
            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Notes.Insert.InsertNotesProfiles(this.DBQueries.TablesName.NotesProfile,notesProfilesInfo.Profile_Name,notesProfilesInfo.Profile_EMail,notesProfilesInfo.Profile_Country,notesProfilesInfo.ProfileOrganization,notesProfilesInfo.Profile_Org_Type);

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            
                RetVal=this.DBConnection.GetNewId();
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        /// <summary>
        /// Update Notes Profiles and return Count Of Records Updated
        /// </summary>
        /// <param name="notesProfileInfo"></param>
        /// <returns >Count Of Records Updated</returns>
        public int UpdateNotesProfiles(NotesProfilesInfo notesProfileInfo)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Notes.Update.UpdateNotesProfiles(this.DBQueries.TablesName.NotesProfile,notesProfileInfo.Profile_NId, notesProfileInfo.Profile_Name, notesProfileInfo.Profile_EMail, notesProfileInfo.Profile_Country, notesProfileInfo.ProfileOrganization, notesProfileInfo.Profile_Org_Type);

                RetVal=this.DBConnection.ExecuteNonQuery(SqlQuery);

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }
        
       /// <summary>
        /// Delete Notes for Notes Profile Nid
       /// </summary>
       /// <param name="notesProfileNids"></param>
       /// <returns>Count Of Records Deleted</returns>
        public int DeleteNotesProfiles(string notesProfileNids)
        {
            int RetVal = 0;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.Notes.Delete.DeleteFrmNotesProfile(this.DBQueries.TablesName.NotesProfile, notesProfileNids);

                RetVal = this.DBConnection.ExecuteNonQuery(SqlQuery);

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        /// Get NotesProfilesInfo for NotesProfileNid
        /// </summary>
        /// <param name="notesProfileNid"></param>
        /// <returns></returns>
        public NotesProfilesInfo GetNotesProfilesInfo(string notesProfileNid)
        {
            NotesProfilesInfo RetVal = null;
            string SqlQuery = string.Empty;
            DbDataReader DBReader = null;
            try
            {
                SqlQuery = this.DBQueries.Notes.GetNotesProfiles(FilterFieldType.NId, notesProfileNid);
                DBReader = (DbDataReader)this.DBConnection.ExecuteReader(SqlQuery);
                if (DBReader.HasRows)
                {
                    RetVal = new NotesProfilesInfo();
                    while (DBReader.Read())
                    {
                        RetVal.Profile_NId = (int)DBReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Profile.ProfileNId];
                        RetVal.Profile_Name = Convert.ToString(DBReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Profile.ProfileName]);
                        RetVal.Profile_EMail = Convert.ToString(DBReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Profile.ProfileEMail]);
                        RetVal.Profile_Country = Convert.ToString(DBReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Profile.ProfileCountry]);
                        RetVal.ProfileOrganization = Convert.ToString(DBReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Profile.ProfileOrg]);
                        RetVal.Profile_Org_Type = Convert.ToString(DBReader[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Notes_Profile.ProfileOrgType]);
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

        #endregion

        #endregion

    }
}
