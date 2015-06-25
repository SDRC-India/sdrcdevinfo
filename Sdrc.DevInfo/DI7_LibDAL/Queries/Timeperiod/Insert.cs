using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Timeperiod
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "timeperiod";

        #endregion

        #endregion

        #region "-- Internal --"

        /// <summary>
        /// Inserts timperiod into TimeperiodTable
        /// </summary>
        /// <param name="DataPrefix">DataPrefix like UT_</param>
        /// <param name="timeperiodValue">timeperiod</param>
        public static string InsertTimeperiod(string DataPrefix, string timeperiodValue)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + DataPrefix + Insert.TableName + " (TimePeriod)  Values('" + timeperiodValue + "')";

            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert StartDate column into Timeperiod table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7StartDateColumn(string dataPrefix)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + new DITables(dataPrefix, string.Empty).TimePeriod + " ADD COLUMN  " + DIColumns.Timeperiods.StartDate + " DateTime";


            return RetVal;
        }


        /// <summary>
        /// Add column EndDate to Timeperiod
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <returns></returns>
        public static string InsertDI7EndDateColumn(string dataPrefix)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + new DITables(dataPrefix, string.Empty).TimePeriod + " ADD COLUMN  " + DIColumns.Timeperiods.EndDate + " DateTime";


            return RetVal;
        }

        /// <summary>
        /// add Perodicity column in UT_TimePeriod column
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="languageCode"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7PerodicityColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + new DITables(dataPrefix, string.Empty).TimePeriod + " ADD COLUMN  " + DIColumns.Timeperiods.Periodicity + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(25) ";
                }
                else
                {
                    RetVal += " varchar(25) ";
                }
            }
            else
            {
                RetVal += " Text(25) ";
            }

            return RetVal;
        }

        /// <summary>
        /// Alter Periodicity datatype as Text
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string AlterPerodicityColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "ALTER TABLE " + new DITables(dataPrefix, string.Empty).TimePeriod + " ALTER COLUMN  " + DIColumns.Timeperiods.Periodicity + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(25) ";
                }
                else
                {
                    RetVal += " varchar(25) ";
                }
            }
            else
            {
                RetVal += " Text(25) ";
            }

            return RetVal;
        }


        #endregion
    }
}
