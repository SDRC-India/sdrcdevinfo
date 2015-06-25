using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.SDMXUser
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {
        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Get Query to Update Sender
        /// </summary>
        /// <param name="isSender"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="contactName"></param>
        /// <param name="role"></param>
        /// <param name="department"></param>
        /// <param name="telephone"></param>
        /// <param name="email"></param>
        /// <param name="fax"></param>
        /// <returns></returns>
        public static string UpdateSender(string TablesName,bool isSender, string id, string name, string contactName, string role, string department, string telephone, string email, string fax)
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("UPDATE " + TablesName);
            RetVal.Append(" SET " + DIColumns.SDMXUser.ID + "='" + DIQueries.RemoveQuotesForSqlQuery(id) + "'");
            RetVal.Append("," + DIColumns.SDMXUser.Name + "='" + DIQueries.RemoveQuotesForSqlQuery(name) + "'");
            RetVal.Append("," + DIColumns.SDMXUser.ContactName + "='" + DIQueries.RemoveQuotesForSqlQuery(contactName) + "'");
            RetVal.Append("," + DIColumns.SDMXUser.Department + "='" + DIQueries.RemoveQuotesForSqlQuery(department) + "'");
            RetVal.Append("," + DIColumns.SDMXUser.Role + "='" + DIQueries.RemoveQuotesForSqlQuery(role) + "'");
            RetVal.Append("," + DIColumns.SDMXUser.Email + "='" + DIQueries.RemoveQuotesForSqlQuery(email) + "'");
            RetVal.Append("," + DIColumns.SDMXUser.Telephone + "='" + DIQueries.RemoveQuotesForSqlQuery(telephone) + "'");
            RetVal.Append("," + DIColumns.SDMXUser.Fax + "='" + DIQueries.RemoveQuotesForSqlQuery(fax) + "'");

            RetVal.Append(" WHERE ");
            RetVal.Append(DIColumns.SDMXUser.IsSender + " = " + DIConnection.GetBoolValue(isSender));

            return RetVal.ToString();
        }

        #endregion

        #endregion
    }
}
