using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.SDMXUser
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


        internal Select(DITables tablesName)
        {
            this.TablesName = tablesName;
        }

        #endregion


        #region "-- Methods --"
        
        /// <summary>
        /// Get query to Get Sender information
        /// </summary>
        /// <param name="isSender">True for Sender and False for Receiver</param>
        /// <returns></returns>
        public string GetSender(bool isSender)
        {
            StringBuilder RetVal = new StringBuilder();


            RetVal.Append("SELECT " + DIColumns.SDMXUser.SenderNId + "," + DIColumns.SDMXUser.ID + "," + DIColumns.SDMXUser.Name + "," + DIColumns.SDMXUser.ContactName + "," + DIColumns.SDMXUser.Department + "," + DIColumns.SDMXUser.Role + "," + DIColumns.SDMXUser.Email + "," + DIColumns.SDMXUser.Telephone + "," + DIColumns.SDMXUser.Fax);
            RetVal.Append(" FROM " + this.TablesName.SDMXUser);
            RetVal.Append(" WHERE "+ DIColumns.SDMXUser.IsSender + "=" + DIConnection.GetBoolValue(isSender));
            

            return RetVal.ToString();
        }


        #endregion

        #endregion

    }
}
