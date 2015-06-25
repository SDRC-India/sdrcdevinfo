using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.DML
{
    public class SenderBuilder
    {

        #region "-- Private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;

        #endregion

        #region "-- Methods --"

        private bool ISSenderExist(bool isSender)
        {
            bool RetVal = false;
            DataTable Table = null;

            Table = this.DBConnection.ExecuteDataTable(this.DBQueries.SDMXUser.GetSender(isSender));
            if (Table.Rows.Count > 0)
            {
                RetVal = true;
            }

            return RetVal;
        }

        #endregion

        #endregion

        #region "-- public --"

        #region "-- Variables --"

        #endregion

        #region "-- new/dispose --"

        public SenderBuilder(DIConnection connection, DIQueries queries)
        {
            this.DBConnection = connection;
            this.DBQueries = queries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Get Sender Information (True for Sender and False for Receiver)       
        /// </summary>
        /// <param name="isSender">True for Sender and False for Receiver</param>
        /// <returns></returns>
        public SenderInfo GetSenderInfo(bool isSender)
        {
            SenderInfo RetVal = new SenderInfo();

            DataTable Table = null;
            Table = this.DBConnection.ExecuteDataTable(this.DBQueries.SDMXUser.GetSender(isSender));

            if (Table.Rows.Count > 0)
            {
                RetVal.ID = Convert.ToString(Table.Rows[0][SDMXUser.ID]);
                RetVal.SenderName = Convert.ToString(Table.Rows[0][SDMXUser.Name]);
                RetVal.ContactName = Convert.ToString(Table.Rows[0][SDMXUser.ContactName]);
                RetVal.Department = Convert.ToString(Table.Rows[0][SDMXUser.Department]);
                RetVal.Role = Convert.ToString(Table.Rows[0][SDMXUser.Role]);
                RetVal.Telephone = Convert.ToString(Table.Rows[0][SDMXUser.Telephone]);
                RetVal.Fax = Convert.ToString(Table.Rows[0][SDMXUser.Fax]);
                RetVal.Email = Convert.ToString(Table.Rows[0][SDMXUser.Email]);
            }

            return RetVal;
        }

        /// <summary>
        /// Check and Create or Update Sender information
        /// </summary>
        /// <param name="senderInfoObj"></param>
        /// <param name="isSender"></param>
        public void CheckAndUpdateSender(SenderInfo senderInfoObj, bool isSender)
        {
            string SqlQuery = string.Empty;

            if (this.ISSenderExist(isSender))
            {
                //-- UPDATE
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.SDMXUser.Update.UpdateSender(this.DBQueries.TablesName.SDMXUser, isSender, DICommon.RemoveQuotes(senderInfoObj.ID), DICommon.RemoveQuotes(senderInfoObj.SenderName), DICommon.RemoveQuotes(senderInfoObj.ContactName), DICommon.RemoveQuotes(senderInfoObj.Role), DICommon.RemoveQuotes(senderInfoObj.Department), DICommon.RemoveQuotes(senderInfoObj.Telephone), DICommon.RemoveQuotes(senderInfoObj.Email), DICommon.RemoveQuotes(senderInfoObj.Fax));

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
            else
            {
                //-- INSERT
                SqlQuery = DevInfo.Lib.DI_LibDAL.Queries.SDMXUser.Insert.InsertSender(this.DBQueries.TablesName.SDMXUser, isSender, DICommon.RemoveQuotes(senderInfoObj.ID), DICommon.RemoveQuotes(senderInfoObj.SenderName), DICommon.RemoveQuotes(senderInfoObj.ContactName), DICommon.RemoveQuotes(senderInfoObj.Role), DICommon.RemoveQuotes(senderInfoObj.Department), DICommon.RemoveQuotes(senderInfoObj.Telephone), DICommon.RemoveQuotes(senderInfoObj.Email), DICommon.RemoveQuotes(senderInfoObj.Fax));

                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }            

        }


        #endregion

        #endregion





    }
}
