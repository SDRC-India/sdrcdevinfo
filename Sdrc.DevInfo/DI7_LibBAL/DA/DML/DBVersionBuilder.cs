using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    /// <summary>
    /// Provides method for inseting, deleting and updating records in DBVersion table
    /// </summary>
    public class DBVersionBuilder
    {
        #region "-- Private --"

        #region "-- Variables --"

        DIConnection DBConnection;
        DIQueries DBQueries;

        #endregion

        #endregion

        #region "-- public --"

        #region "-- New/Dispose --"

        /// <summary>
        /// Consturctor
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        public DBVersionBuilder(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Inserts records int DBVersion table
        /// </summary>
        /// <param name="versionNumber"></param>
        /// <param name="versionChangeDate"></param>
        /// <param name="versionComments"></param>
        public void InsertVersionInfo(string versionNumber, string versionChangeDate, string versionComments)
        {
            try
            {
                this.DBConnection.ExecuteNonQuery(DevInfo.Lib.DI_LibDAL.Queries.DBVersion.Insert.InsertVersionInfo(versionNumber, versionChangeDate, versionComments));

            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Returns true if DBVersion table exists otherwise false
        /// </summary>
        /// <returns></returns>
        public bool IsVersionTableExists()
        {
            bool RetVal = false;

            try
            {
                //-- Check the existence of the table 
                if (this.DBConnection.ExecuteDataTable("SELECT count(*) FROM " + this.DBQueries.TablesName.DBVersion + " WHERE 1=1") != null)
                    RetVal = true;
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        // Deletes duplicate records
        public void DeleteDuplicateRecords()
        {
            try
            {
                this.DBConnection.ExecuteNonQuery(this.DBQueries.Delete.Version.DeleteDuplicateRecords());
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        /// <summary>
        /// Delete version entry after 6
        /// </summary>
        public void DeleteVersionsFromVersionNumberToEnd(string VersionNumber)
        {
            try
            {
                this.DBConnection.ExecuteNonQuery(this.DBQueries.Delete.Version.DeleteVersionsFromVersionNumberToEnd(VersionNumber));
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        #endregion

        #endregion

    }
}
