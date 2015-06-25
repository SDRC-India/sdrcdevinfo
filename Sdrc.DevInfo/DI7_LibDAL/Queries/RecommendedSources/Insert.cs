using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.RecommendedSources
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Methods --"

        /// <summary>
        /// Returns a query to create RecommendedSources table
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
                    RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.RecommendedSources.NId + " int(4) NOT NULL AUTO_INCREMENT ,PRIMARY KEY (" + DIColumns.RecommendedSources.NId + ")," +
                        DIColumns.RecommendedSources.DataNId + " int(4), " +
                        DIColumns.RecommendedSources.ICIUSLabel + " varchar(255))";
                }
                else
                {
                    RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.RecommendedSources.NId + "  int Identity(1,1) primary key," +
                        DIColumns.RecommendedSources.DataNId + " int, " +
                        DIColumns.RecommendedSources.ICIUSLabel + " varchar(255))";
                }

            }
            else
            {
                RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.RecommendedSources.NId + " counter primary key, " +
                       DIColumns.RecommendedSources.DataNId + " Long, " +
                       DIColumns.RecommendedSources.ICIUSLabel + " text(255))";
            }

            return RetVal;
        }


        /// <summary>
        /// Insert Recommended Sources into RecomendedSources Table
        /// </summary>
        /// <param name="dataNid"></param>
        /// <param name="icIUSLabel"></param>
        /// <returns></returns>
        public static string InsertRecommendedSource(string tableName, int dataNid, string icIUSLabel)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + tableName + "(" + DIColumns.RecommendedSources.DataNId + "," 
                    + DIColumns.RecommendedSources.ICIUSLabel +")"
                    + " VALUES('" + dataNid + "','" + DIQueries.RemoveQuotes(icIUSLabel) +"')";

            return RetVal;
        }

        /// <summary>
        /// Inserts all recommended sources' label into UT_RecommendedSources_en table from IndicatorClassification_IUS table
        /// </summary>
        /// <param name="tablesName"></param>
        /// <returns></returns>
        public static string InsertAllLabelFrmICIUS(DITables tablesName)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + tablesName.RecommendedSources +" ("+ DIColumns.RecommendedSources.DataNId +","+ DIColumns.RecommendedSources.ICIUSLabel +") "
                    + " SELECT D." + DIColumns.Data.DataNId + ", ICIUS."+ DIColumns.IndicatorClassificationsIUS.ICIUSLabel
                    + " FROM " + tablesName.Data +" D,"+ tablesName.IndicatorClassificationsIUS +" ICIUS "
                    + " WHERE ICIUS."+ DIColumns.IndicatorClassificationsIUS.IUSNId +"=D."+ DIColumns.Data.IUSNId
                    + " AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + "= D."+ DIColumns.Data.SourceNId 
                    + " AND ICIUS."+ DIColumns.IndicatorClassificationsIUS.ICIUSLabel +"  is not NULL";

            return RetVal;
        }

        
        #endregion

        #endregion


    }
}
