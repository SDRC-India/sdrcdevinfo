using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.DBVersion
{
    /// <summary>
    /// Provides sql queries to get records
    /// </summary>
    public class Select
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DITables TablesName;

        #endregion


        #region "-- New / Dispose --"

        private Select()
        {
            // don't implement this method
        }

        #endregion

        #endregion

        #region "-- Public / Internal --"

        #region "-- New / Dispose --"


        public Select(DITables tablesName)
        {
            this.TablesName = tablesName;
        }

        #endregion


        #region "-- Methods --"

        /// <summary>
        /// Returns query to get records for the given version number
        /// </summary>
        /// <param name="versionNumber"></param>
        /// <returns></returns>
        public string GetRecords(string versionNumber)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT * FROM " + this.TablesName.DBVersion + " WHERE " + DIColumns.DBVersion.VersionNumber + "='" + versionNumber +"'";

            return RetVal;
        }
        public string GetRecords()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT * FROM " + this.TablesName.DBVersion ;

            return RetVal;
        }


        #endregion

        #endregion

    }
}
