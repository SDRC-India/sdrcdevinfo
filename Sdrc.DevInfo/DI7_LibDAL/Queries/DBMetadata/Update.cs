using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;

namespace DevInfo.Lib.DI_LibDAL.Queries.DBMetadata
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {

        #region "-- Public --"
        /// <summary>
        /// Returns update query
        /// </summary>
        /// <param name="description"></param>
        /// <param name="publisherName"></param>
        /// <param name="publisherDate"></param>
        /// <param name="publisherCountry"></param>
        /// <param name="publisherRegion"></param>
        /// <param name="publisherOffice"></param>
        /// <param name="areaCount"></param>
        /// <param name="indicatorCount"></param>
        /// <param name="IUSCount"></param>
        /// <param name="timeperiodCount"></param>
        /// <param name="dataCount"></param>
        /// <returns></returns>
        public static string UpdateRecord(string tableName, int NID, string description, string publisherName, string publisherDate,
            string publisherCountry, string publisherRegion, string publisherOffice,
            string areaCount, string indicatorCount, string IUSCount, string timeperiodCount,string sourceCount, string dataCount)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + tableName + " SET " +
                DIColumns.DBMetaData.Description + " ='" +description+"',"+
                 DIColumns.DBMetaData.PublisherName + "='" + publisherName + "'," +
                 DIColumns.DBMetaData.PublisherDate + " ='" + publisherDate + "'," +
                 DIColumns.DBMetaData.PublisherCountry + " ='" + publisherCountry + "'," +
                 DIColumns.DBMetaData.PublisherRegion + "  ='" + publisherRegion + "'," +
                 DIColumns.DBMetaData.PublisherOffice + "  ='" + publisherOffice + "'," +
                 DIColumns.DBMetaData.AreaCount + " =" + areaCount + "," +
                 DIColumns.DBMetaData.IndicatorCount + " =" + indicatorCount + "," +
                 DIColumns.DBMetaData.IUSCount + " =" + IUSCount + "," +
                 DIColumns.DBMetaData.TimeperiodCount + " =" + timeperiodCount + "," +
                 DIColumns.DBMetaData.SourceCount + "  =" + sourceCount+","+
                 DIColumns.DBMetaData.DataCount + " =" + dataCount + " WHERE " + DIColumns.DBMetaData.NID + "=" + NID;

            return RetVal;
        }


        /// <summary>
        /// Returns update query
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="NID"></param>
        /// <param name="description"></param>
       /// <param name="areaCount"></param>
        /// <param name="indicatorCount"></param>
        /// <param name="IUSCount"></param>
        /// <param name="timeperiodCount"></param>
        /// <param name="sourceCount"></param>
        /// <param name="dataCount"></param>
        /// <returns></returns>
        public static string UpdateCounts(string tableName, int NID, 
            string areaCount, string indicatorCount, string IUSCount, string timeperiodCount, string sourceCount, string dataCount)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + tableName + " SET " +
                DIColumns.DBMetaData.PublisherDate +"='"+ System.DateTime.Now.ToString("yyyy-MM-dd") +"'," +
                 DIColumns.DBMetaData.AreaCount + " =" + areaCount + "," +
                 DIColumns.DBMetaData.IndicatorCount + " =" + indicatorCount + "," +
                 DIColumns.DBMetaData.IUSCount + " =" + IUSCount + "," +
                 DIColumns.DBMetaData.TimeperiodCount + " =" + timeperiodCount + "," +
                 DIColumns.DBMetaData.SourceCount + "  =" + sourceCount + "," +
                 DIColumns.DBMetaData.DataCount + " =" + dataCount;
            if (NID > 0)
            {
               RetVal +=" WHERE " + DIColumns.DBMetaData.NID + "=" + NID;
            }

            return RetVal;
        }

        #endregion

    }
}
