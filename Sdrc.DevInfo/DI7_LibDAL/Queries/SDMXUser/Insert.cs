using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.SDMXUser
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
        public static string CreateTable(string tableName,bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;

            if (forOnlineDB)
            {

                if (serverType == DIServerType.MySql)
                {
                    RetVal = "CREATE TABLE " + tableName +
                        " (" + DIColumns.SDMXUser.SenderNId + " int(4) NOT NULL AUTO_INCREMENT ,PRIMARY KEY (" + DIColumns.SDMXUser.SenderNId + ")," +
                        DIColumns.SDMXUser.IsSender + " bit ,"
                        + DIColumns.SDMXUser.ID + " varchar(255), "
                        + DIColumns.SDMXUser.Name + " varchar(255)), " + DIColumns.SDMXUser.ContactName + " varchar(255)), "
                        + DIColumns.SDMXUser.Department + " varchar(255)), " + DIColumns.SDMXUser.Email + " varchar(255)), "
                        + DIColumns.SDMXUser.Telephone + " varchar(50)), "
                        + DIColumns.SDMXUser.Role + " varchar(255)), " + DIColumns.SDMXUser.Fax + " varchar(50))";
                }
                else
                {
                    RetVal = "CREATE TABLE " + tableName
                        + "(" + DIColumns.SDMXUser.SenderNId + "  int Identity(1,1) primary key," +
                        DIColumns.SDMXUser.IsSender + " bit ,"
                        + DIColumns.SDMXUser.ID + " nvarchar(255), "
                        + DIColumns.SDMXUser.Name + "   nvarchar(255)), " + DIColumns.SDMXUser.ContactName + " nvarchar(255)), "
                        + DIColumns.SDMXUser.Department + "   nvarchar(255)), " + DIColumns.SDMXUser.Email + " nvarchar(255)), "
                        + DIColumns.SDMXUser.Telephone + "   nvarchar(50)), "
                        + DIColumns.SDMXUser.Role + " nvarchar(255)), " + DIColumns.SDMXUser.Fax + " nvarchar(50))";
                }
            }
            else
            {
                RetVal = "CREATE TABLE " + tableName
                        + " (" + DIColumns.SDMXUser.SenderNId + " counter primary key, "
                        + DIColumns.SDMXUser.IsSender + " bit ,"
                        + DIColumns.SDMXUser.ID + " Text(255), "
                        + DIColumns.SDMXUser.Name + " Text(255), " + DIColumns.SDMXUser.ContactName + " Text(255), "
                        + DIColumns.SDMXUser.Department + " Text(255), " + DIColumns.SDMXUser.Email + " Text(255), "
                        + DIColumns.SDMXUser.Telephone + " Text(50), "
                        + DIColumns.SDMXUser.Role + " Text(255), " + DIColumns.SDMXUser.Fax + " Text(50))";
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
        public static string InsertSender(string TablesName, bool isSender, string id, string name, string contactName, string role, string department, string telephone, string email, string fax)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + TablesName + "(" + DIColumns.SDMXUser.IsSender + ","
            + DIColumns.SDMXUser.ID + "," + DIColumns.SDMXUser.Name + "," + DIColumns.SDMXUser.ContactName + "," + DIColumns.SDMXUser.Department + ","
            + DIColumns.SDMXUser.Role + "," + DIColumns.SDMXUser.Email + "," + DIColumns.SDMXUser.Telephone + "," + DIColumns.SDMXUser.Fax + ") "
            + " VALUES(" + DIConnection.GetBoolValue(isSender) + ",'" + DIQueries.RemoveQuotesForSqlQuery(id) + "','"
            + DIQueries.RemoveQuotesForSqlQuery(name) + "','" + DIQueries.RemoveQuotesForSqlQuery(contactName) + "','"
            + DIQueries.RemoveQuotesForSqlQuery(department) + "','" + DIQueries.RemoveQuotesForSqlQuery(role) + "','"
            + DIQueries.RemoveQuotesForSqlQuery(email) + "','" + DIQueries.RemoveQuotesForSqlQuery(telephone) + "','"
            + DIQueries.RemoveQuotesForSqlQuery(fax) + "')";

            return RetVal;
        }

        #endregion

        #endregion
    }
}
