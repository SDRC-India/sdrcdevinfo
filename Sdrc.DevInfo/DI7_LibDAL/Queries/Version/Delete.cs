using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.DBVersion
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public class Delete
    {

        #region "-- Private --"

        #region "-- Variables --"

        private DITables TablesName;

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- New / Dispose --"

        public Delete(DITables tablesName)
        {
            this.TablesName = tablesName;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Deletes duplicate records from DBversion table
        /// </summary>
        /// <returns></returns>
        public string DeleteDuplicateRecords()
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + this.TablesName.DBVersion + " WHERE " +
                DIColumns.DBVersion.VersionNId + " NOT IN ( SELECT MIN(" +
               DIColumns.DBVersion.VersionNId + ") FROM " + this.TablesName.DBVersion + " GROUP BY " + DIColumns.DBVersion.VersionNumber + " )";
            return RetVal;
        }

        /// <summary>
        /// Get query to delete version entry after 6 [In 7 to 6 conversion it will delete all version from which is greater than 6 [to delete all 7th version]]
        /// </summary>
        /// <param name="VersionNumber"></param>
        /// <returns></returns>
        public string DeleteVersionsFromVersionNumberToEnd(string VersionNumber)
        {
            string RetVal = string.Empty;

            RetVal = "delete from " + this.TablesName.DBVersion + " where " + DIColumns.DBVersion.VersionNId
                + ">( "
                + " select top 1 " + DIColumns.DBVersion.VersionNId
                + " from " + this.TablesName.DBVersion
                + " where " + DIColumns.DBVersion.VersionNumber
                + " ='" + VersionNumber + "'"
                + " )";

            return RetVal;
        }

        #endregion

        #endregion
    }
}
