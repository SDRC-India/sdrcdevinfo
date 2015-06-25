using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.SDMXUser
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
        public string DeleteRecord(bool isSender)
        {
            string RetVal = string.Empty;

            RetVal = "DELETE FROM " + this.TablesName.SDMXUser + " WHERE " +
                DIColumns.SDMXUser.IsSender + " = " + DIConnection.GetBoolValue(isSender);

            return RetVal;
        }

        #endregion

        #endregion
    }
}
