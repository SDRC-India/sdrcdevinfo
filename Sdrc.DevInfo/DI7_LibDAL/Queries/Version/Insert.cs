using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.DBVersion
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Returns a query to create DBVersion table
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string CreateTable(bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;

            if (forOnlineDB)
            {
             
                if (serverType == DIServerType.MySql)
                {
                    RetVal = "CREATE TABLE " + new DITables(string.Empty, string.Empty).DBVersion + " (" + DIColumns.DBVersion.VersionNId + " int(4) NOT NULL AUTO_INCREMENT ,PRIMARY KEY (" + DIColumns.DBVersion.VersionNId + ")," + DIColumns.DBVersion.VersionNumber + " varchar(50), " + DIColumns.DBVersion.VersionChangeDate + " varchar(50), " + DIColumns.DBVersion.VersionComments + "   varchar(255))";
                }
                else
                {
                    RetVal = "CREATE TABLE " + new DITables(string.Empty, string.Empty).DBVersion + " (" + DIColumns.DBVersion.VersionNId + " int Identity(1,1) primary key," + DIColumns.DBVersion.VersionNumber + "  varchar(50),  " + DIColumns.DBVersion.VersionChangeDate + " varchar(50) , " + DIColumns.DBVersion.VersionComments + "   varchar(255))";
                }
            }
            else
            {
                RetVal = "CREATE TABLE " + new DITables(string.Empty, string.Empty).DBVersion + " (" + DIColumns.DBVersion.VersionNId + " counter primary key," + DIColumns.DBVersion.VersionNumber + "  text(50), " + DIColumns.DBVersion.VersionChangeDate + " text(50), " + DIColumns.DBVersion.VersionComments + "  text(255))";
            }
            return RetVal;

        }

        /// <summary>
        /// Returns query to insert version info into DBVersion table
        /// </summary>
        /// <param name="versionNumber"></param>
        /// <param name="versionChangeDate"></param>
        /// <param name="versionComments"></param>
        /// <returns></returns>
        public static string InsertVersionInfo(string versionNumber, string versionChangeDate, string versionComments)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + new DITables(string.Empty,string.Empty).DBVersion  + "(" + DIColumns.DBVersion.VersionNumber+ ","
            + DIColumns.DBVersion.VersionChangeDate+ "," + DIColumns.DBVersion.VersionComments+  ") "
            + " VALUES('" + versionNumber + "','" + versionChangeDate + "','"+ versionComments + "')";

            return RetVal;
        }

        #endregion

        #endregion
    }
}
