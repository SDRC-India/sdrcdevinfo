using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.DBMetadata
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
        public static string CreateTable(string tableName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;

            if (forOnlineDB)
            {

                if (serverType == DIServerType.MySql)
                {
                    RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.DBMetaData.NID + " int(4) NOT NULL AUTO_INCREMENT ,PRIMARY KEY (" + DIColumns.DBMetaData.NID + ")," +
                        DIColumns.DBMetaData.Description + " ntext, " +
                        DIColumns.DBMetaData.PublisherName + " varchar(50), " +
                        DIColumns.DBMetaData.PublisherDate + "  DateTime," +
                        DIColumns.DBMetaData.PublisherCountry + " varchar(50)," +
                        DIColumns.DBMetaData.PublisherRegion + " varchar(50)," +
                        DIColumns.DBMetaData.PublisherOffice + " varchar(50)," +
                        DIColumns.DBMetaData.AreaCount + " int(4) ," +
                        DIColumns.DBMetaData.IndicatorCount + " int(4)," +
                        DIColumns.DBMetaData.IUSCount + " int(4)," +
                        DIColumns.DBMetaData.TimeperiodCount + " int(4)," +
                        DIColumns.DBMetaData.SourceCount + "  int(4)," +
                        DIColumns.DBMetaData.DataCount + " int(4))";
                }
                else
                {
                    RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.DBMetaData.NID + " int Identity(1,1) primary key," +
                    DIColumns.DBMetaData.Description + " ntext, " +
                        DIColumns.DBMetaData.PublisherName + " varchar(50), " +
                        DIColumns.DBMetaData.PublisherDate + "  DateTime," +
                        DIColumns.DBMetaData.PublisherCountry + " varchar(50)," +
                        DIColumns.DBMetaData.PublisherRegion + " varchar(50)," +
                        DIColumns.DBMetaData.PublisherOffice + " varchar(50)," +
                        DIColumns.DBMetaData.AreaCount + " int ," +
                        DIColumns.DBMetaData.IndicatorCount + " int," +
                        DIColumns.DBMetaData.IUSCount + " int," +
                        DIColumns.DBMetaData.TimeperiodCount + " int," +
                        DIColumns.DBMetaData.SourceCount + "  int," +
                        DIColumns.DBMetaData.DataCount + " int)";
                }

            }
            else
            {
                RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.DBMetaData.NID + " counter primary key," +
                    DIColumns.DBMetaData.Description + " MEMO, " +
                     DIColumns.DBMetaData.PublisherName + " text(50), " +
                     DIColumns.DBMetaData.PublisherDate + "  DateTime," +
                     DIColumns.DBMetaData.PublisherCountry + " text(50)," +
                     DIColumns.DBMetaData.PublisherRegion + " text(50)," +
                     DIColumns.DBMetaData.PublisherOffice + " text(50)," +
                     DIColumns.DBMetaData.AreaCount + " number ," +
                     DIColumns.DBMetaData.IndicatorCount + " number," +
                     DIColumns.DBMetaData.IUSCount + " number," +
                     DIColumns.DBMetaData.TimeperiodCount + " number," +
                     DIColumns.DBMetaData.SourceCount + "  number," +
                     DIColumns.DBMetaData.DataCount + " number)";
            }

            return RetVal;
        }

        /// <summary>
        /// Returns insert query
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
        public static string InsertRecord(string tableName, string description, string publisherName, string publisherDate,
            string publisherCountry, string publisherRegion, string publisherOffice,
            string areaCount, string indicatorCount, string IUSCount, string timeperiodCount,string sourceCount, string dataCount)
        {
            string RetVal = string.Empty;

            RetVal = "Insert INTO " + tableName + " (" +
                DIColumns.DBMetaData.Description + " , " +
                 DIColumns.DBMetaData.PublisherName + " , " ;

                 if(!string.IsNullOrEmpty(publisherDate))
                 {
                 RetVal += DIColumns.DBMetaData.PublisherDate + "  ," ;
                 }

                 RetVal += DIColumns.DBMetaData.PublisherCountry + " ," +
                 DIColumns.DBMetaData.PublisherRegion + " ," +
                 DIColumns.DBMetaData.PublisherOffice + " ," +
                 DIColumns.DBMetaData.AreaCount + "  ," +
                 DIColumns.DBMetaData.IndicatorCount + " ," +
                 DIColumns.DBMetaData.IUSCount + " ," +
                 DIColumns.DBMetaData.TimeperiodCount + " ," +
                 DIColumns.DBMetaData.SourceCount + "  ," +
                 DIColumns.DBMetaData.DataCount + " ) VALUES ('" +
                 description + "','" +
                publisherName + "','" ;

                if(!string.IsNullOrEmpty(publisherDate))
                 {
                     RetVal += publisherDate + "','";
                }


                RetVal += publisherCountry + "','" +
                publisherRegion + "','" +
                publisherOffice + "'," +
                areaCount + "," +
                indicatorCount + "," +
                IUSCount + "," +
                timeperiodCount + "," +
                sourceCount+","+
                dataCount + ")";

            return RetVal;

        }


        #endregion

        #endregion


    }
}
