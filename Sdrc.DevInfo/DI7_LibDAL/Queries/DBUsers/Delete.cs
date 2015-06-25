using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.DBUsers
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public static class Delete
    {
        
        #region "-- Public --"

        public static string DeleteUser(string userTable,int iUserNId)
        {
            string RetVal = string.Empty;
            StringBuilder sSql = new StringBuilder();

            sSql.Append("DELETE FROM " + userTable );
            sSql.Append(" WHERE "+ DIColumns.DBUser.UserNid + "=" + iUserNId);

            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;
            return RetVal;
        }


        /// <summary>
        /// Return Sql Query for Delete records from UserAccess Table based on UserNid and DataPrefix
        /// </summary>
        /// <param name="userNId"></param>
        /// <param name="dataPrefix"></param>
        /// <param name="tablesName"></param>        
        /// <returns></returns>
        public static string DeleteUserAccess(int userNId, string dataPrefix, DITables tablesName)
        {
            string RetVal = null;
            StringBuilder sSql = new StringBuilder();
            //DITables TablesName;
            sSql.Append("DELETE FROM " + tablesName.DBUserAccess);
            sSql.Append(" WHERE " + DIColumns.DBUserAccess.UserNId + "=" + userNId + " AND " + DIColumns.DBUserAccess.DBPrefix + "= '" + dataPrefix + "'");

            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;

            return RetVal;
        }
        public static string DeleteDIUser(string userTable, int iUserNId)
        {
            string RetVal = string.Empty;
            StringBuilder sSql = new StringBuilder();

            sSql.Append("DELETE FROM " + userTable);
            sSql.Append(" WHERE " + DIColumns.DIUser.UserNId + "=" + iUserNId);

            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;
            return RetVal;
        }

        #endregion
    }

}