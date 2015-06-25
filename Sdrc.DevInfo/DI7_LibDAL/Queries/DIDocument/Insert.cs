using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.DIDocument
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {

        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "document";

        #endregion

        #endregion

        #region "-- Internal --"

        /// <summary>
        /// Returns a query to create Document table
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string CreateTable(string tableName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;


            if (forOnlineDB)
            {

                if (serverType == DIServerType.MySql)
                {
                    RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.DIDocument.DocumentNid + " int(4) NOT NULL AUTO_INCREMENT ,PRIMARY KEY (" + DIColumns.DIDocument.DocumentNid + ")," +
                        DIColumns.DIDocument.DocumentType + " varchar(4), " +
                        DIColumns.DIDocument.ElementType + " varchar(2)," +
                        DIColumns.DIDocument.ElementNid + " int(4), " +
                        DIColumns.DIDocument.ElementDocument + " LongText )";
                }
                else
                {
                    RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.DIDocument.DocumentNid + "  int Identity(1,1) primary key," +
                        DIColumns.DIDocument.DocumentType + " varchar(4), " +
                        DIColumns.DIDocument.ElementType + " varchar(2)," +
                        DIColumns.DIDocument.ElementNid + " int ," +
                        DIColumns.DIDocument.ElementDocument + " image )";
                }

            }
            else
            {
                RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.DIDocument.DocumentNid + " counter primary key, " +
                       DIColumns.DIDocument.DocumentType + " text(4), " +
                       DIColumns.DIDocument.ElementType + " text(2)," +
                        DIColumns.DIDocument.ElementNid + " number ," +
                        DIColumns.DIDocument.ElementDocument + " OLEObject )";
            }

            return RetVal;
        }

        #endregion


    }
}
