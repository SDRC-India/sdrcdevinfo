using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Timeperiod
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public class Delete
    {

        #region "-- Private --"

        #region "-- Variables --"

        private DITables TablesName;

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- New / Dispose --"

        public Delete(DITables tablesName)
        {
            this.TablesName = tablesName;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns query to delete records from Timeperiod table
        /// </summary>
        /// <param name="nids">Comma separated Timeperiod_NIds which may be blank </param>
        /// <returns></returns>
        public string DeleteRecords(string nids)
        {

            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.TimePeriod;

            if (!string.IsNullOrEmpty(nids))
            {
                RetVal += " Where " + DIColumns.Timeperiods.TimePeriodNId+ " IN (" + nids + ")";
            }
            return RetVal;
        }

        /// <summary>
        /// Returns sqlquery to delete records from the given table where timeperiods are not in a valid DevInfo format.
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string DeleteInvalidTimeperiod(string tableName)
        {
            string RetVal = string.Empty;

            RetVal = " DELETE FROM " + tableName + " AS TD WHERE   (NOT ( " +
            " ( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=4 AND TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____' AND ISNUMERIC( TD." + DIColumns.Timeperiods.TimePeriod + ") ) OR  " +
            " ( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=7    AND  TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____.__'   AND ISNUMERIC( TD." + DIColumns.Timeperiods.TimePeriod + ") )  OR  " +
            " ( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=10   AND  TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____.__.__'   AND ISNUMERIC( LEFT(TD." + DIColumns.Timeperiods.TimePeriod + ",7))  AND ISNUMERIC( RIGHT(TD." + DIColumns.Timeperiods.TimePeriod + ",2)) )  OR  " +
            "( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=9     AND  TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____-____'  AND ISNUMERIC(LEFT(TD." + DIColumns.Timeperiods.TimePeriod + ",4)) AND  ISNUMERIC( RIGHT(TD." + DIColumns.Timeperiods.TimePeriod + ",4) ))   OR  " +
            " ( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=15   AND  TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____.__-____.__'  AND ISNUMERIC( LEFT(TD." + DIColumns.Timeperiods.TimePeriod + ",7))  AND ISNUMERIC( RIGHT(TD." + DIColumns.Timeperiods.TimePeriod + ",7))  ) OR  " +
            " ( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=21   AND  TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____.__.__-____.__.__'  AND ISNUMERIC(LEFT( TD." + DIColumns.Timeperiods.TimePeriod + ",7))  AND ISNUMERIC(RIGHT( TD." + DIColumns.Timeperiods.TimePeriod + ",5))  )  )           )";

            return RetVal;
        }


        public string DeleteInvalidTimeperiodForOnline(string tableName)
        {
            string RetVal = string.Empty;

            RetVal = " DELETE FROM " + tableName + " WHERE   (NOT  " +         
              
            " ( Len(" + DIColumns.Timeperiods.TimePeriod + ")=4 OR NOT (" + DIColumns.Timeperiods.TimePeriod + " LIKE  '____' OR (CAST(" + DIColumns.Timeperiods.TimePeriod + " AS numeric(18)) = " + DIColumns.Timeperiods.TimePeriod + ")))" +


                      " AND (NOT (LEN(" + DIColumns.Timeperiods.TimePeriod + ") = 7) OR" +
                      " NOT (" + DIColumns.Timeperiods.TimePeriod + " LIKE '____.__') OR " +
                      " NOT (CAST(" + DIColumns.Timeperiods.TimePeriod + " AS numeric(18)) = " + DIColumns.Timeperiods.TimePeriod + ")) AND (NOT (LEN(" + DIColumns.Timeperiods.TimePeriod + ") = 10) OR " +
                      " NOT (" + DIColumns.Timeperiods.TimePeriod + " LIKE '____.__.__') OR " +
                      " NOT (CAST(LEFT(" + DIColumns.Timeperiods.TimePeriod + ", 7) AS numeric(18, 2)) = LEFT(" + DIColumns.Timeperiods.TimePeriod + ", 7)) OR " +
                      " NOT (CAST(RIGHT(" + DIColumns.Timeperiods.TimePeriod + ", 2) AS numeric(18)) = RIGHT(" + DIColumns.Timeperiods.TimePeriod + ", 2))) AND (NOT (LEN(" + DIColumns.Timeperiods.TimePeriod + ") = 9) OR " +
                      " NOT (" + DIColumns.Timeperiods.TimePeriod + " LIKE '____-____') OR " +
                       " NOT (CAST(LEFT(" + DIColumns.Timeperiods.TimePeriod + ", 4) AS numeric(18)) = LEFT(" + DIColumns.Timeperiods.TimePeriod + ", 4)) OR " +
                      " NOT (CAST(RIGHT(" + DIColumns.Timeperiods.TimePeriod + ", 4) AS numeric(18)) = RIGHT(" + DIColumns.Timeperiods.TimePeriod + ", 4))) AND (NOT (LEN(" + DIColumns.Timeperiods.TimePeriod + ") = 15) OR " +
                      " NOT (" + DIColumns.Timeperiods.TimePeriod + " LIKE '____.__-____.__') OR " +
                      " NOT (CAST(LEFT(" + DIColumns.Timeperiods.TimePeriod + ", 7) AS numeric(18, 2)) = LEFT(" + DIColumns.Timeperiods.TimePeriod + ", 7)) OR " +
                      " NOT (CAST(RIGHT(" + DIColumns.Timeperiods.TimePeriod + ", 7) AS numeric(18, 2)) = RIGHT(" + DIColumns.Timeperiods.TimePeriod + ", 7))) AND (NOT (LEN(" + DIColumns.Timeperiods.TimePeriod + ") = 21) OR " +
                      " NOT (" + DIColumns.Timeperiods.TimePeriod + " LIKE '____.__.__-____.__.__') OR " +
                      " NOT (CAST(LEFT(" + DIColumns.Timeperiods.TimePeriod + ", 7) AS numeric(18, 2)) = LEFT(" + DIColumns.Timeperiods.TimePeriod + ", 7)) OR " +
                      " NOT (CAST(RIGHT(" + DIColumns.Timeperiods.TimePeriod + ", 5) AS numeric(18)) = RIGHT(" + DIColumns.Timeperiods.TimePeriod + ", 5))))";

            return RetVal;
        }

        #endregion

        #endregion

    }
}
