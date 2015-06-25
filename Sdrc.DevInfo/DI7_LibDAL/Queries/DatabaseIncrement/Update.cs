using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.DatabaseIncrement
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {
        #region "-- Private --"

        #region "-- Variables --"

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- New / Dispose --"

        #endregion

        #region "-- Methods --"

        public static string UpdateMapped_GID(string tableName, string columnName, string value)
        {
            string RetVal = string.Empty;
            RetVal = "update " + tableName + " set " + columnName + " ='true'";
            RetVal += " where ";

            return RetVal;
        }

        #endregion

        #endregion

    }
}