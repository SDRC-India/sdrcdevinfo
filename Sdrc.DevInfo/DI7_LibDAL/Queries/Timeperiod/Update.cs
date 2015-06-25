using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Timeperiod
{

    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public class Update
    {

        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "timeperiod";

        #endregion

        #endregion

        #region "-- Public --"

        /// <summary>
        /// Update Timeperiod Value for Timeperiod_Nid
        /// </summary>
        /// <param name="dataPrefix"> DataPrefix </param>
        /// <param name="timeperiodNid">Timeperiod NId</param>
        /// <param name="timeValue">Timeperiod Value</param>
        /// <returns></returns>
        public static string UpdateTimeperiod(string dataPrefix, string timeperiodNid, string timeperiodValue)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName + " SET " + DIColumns.Timeperiods.TimePeriod
                      + " = '" + timeperiodValue + "' WHERE " + DIColumns.Timeperiods.TimePeriodNId + " = " + timeperiodNid;

            return RetVal;

        }

        /// <summary>
        /// Update Timeperiod Value for Timeperiod_Nid
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="timeperiodNid"></param>
        /// <param name="timeperiodValue"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="priodicityValue"></param>
        /// <returns></returns>
        public static string UpdateTimeperiod(string dataPrefix, string timeperiodNid, string timeperiodValue, string startDate, string endDate, string priodicityValue)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + dataPrefix + Update.TableName
                + " SET " + DIColumns.Timeperiods.TimePeriod + " = '" + timeperiodValue + "',"
                      + DIColumns.Timeperiods.StartDate + " = '" + startDate + "',"
                      + DIColumns.Timeperiods.EndDate + " = '" + endDate + "',"
                      + DIColumns.Timeperiods.Periodicity + " = '" + priodicityValue + "'"
                      + " WHERE " + DIColumns.Timeperiods.TimePeriodNId + " = " + timeperiodNid;

            return RetVal;

        }

        #endregion

    }
}
