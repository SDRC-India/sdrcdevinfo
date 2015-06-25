using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.Timeperiod
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
        /// Returns sql query to get Invalid timeperiod value 
        /// </summary>
        /// <remarks>This methods is used in import process, so dont change it </remarks>
        /// <param name="tableName">tablename </param>
        /// <returns></returns>
        public string GetInvalidTimeperiods(string tableName)
        {
            string RetVal = string.Empty;

            RetVal = " SELECT * FROM " + tableName + " AS TD WHERE   (NOT ( " +
            " ( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=4 AND TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____' AND ISNUMERIC( TD." + DIColumns.Timeperiods.TimePeriod + ") ) OR  " +
            " ( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=7    AND  TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____.__'   AND ISNUMERIC( TD." + DIColumns.Timeperiods.TimePeriod + ") )  OR  " +
            " ( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=10   AND  TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____.__.__'   AND ISNUMERIC( LEFT(TD." + DIColumns.Timeperiods.TimePeriod + ",7))  AND ISNUMERIC( RIGHT(TD." + DIColumns.Timeperiods.TimePeriod + ",2)) )  OR  " +
            "( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=9     AND  TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____-____'  AND ISNUMERIC(LEFT(TD." + DIColumns.Timeperiods.TimePeriod + ",4)) AND  ISNUMERIC( RIGHT(TD." + DIColumns.Timeperiods.TimePeriod + ",4) ))   OR  " +
            " ( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=15   AND  TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____.__-____.__'  AND ISNUMERIC( LEFT(TD." + DIColumns.Timeperiods.TimePeriod + ",7))  AND ISNUMERIC( RIGHT(TD." + DIColumns.Timeperiods.TimePeriod + ",7))  ) OR  " +
            " ( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=21   AND  TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____.__.__-____.__.__'  AND ISNUMERIC(LEFT( TD." + DIColumns.Timeperiods.TimePeriod + ",7))  AND ISNUMERIC(RIGHT( TD." + DIColumns.Timeperiods.TimePeriod + ",5))  )  )           )";

            //)";

            return RetVal;
        }

        public string GetInvalidTimeperiodsForOnline(string tableName)
        {
            string RetVal = string.Empty;

            RetVal = " SELECT * FROM " + tableName + " AS TD WHERE   (NOT " +
                //  (NOT (LEN(TimePeriod) = 4) OR  NOT (TimePeriod LIKE '____') OR   NOT (CAST(TimePeriod AS numeric(18)) = TimePeriod))
            " ( Len(TD." + DIColumns.Timeperiods.TimePeriod + ")=4 OR NOT (TD." + DIColumns.Timeperiods.TimePeriod + " LIKE  '____' OR (CAST(" + DIColumns.Timeperiods.TimePeriod + " AS numeric(18)) = " + DIColumns.Timeperiods.TimePeriod + ")))" +


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

        /// <summary>
        /// Get query to return TimePeriod missing Data.
        /// </summary>
        /// <returns></returns>
        public string GetTimePeriodsWithoutData()
        {
            string RetVal = string.Empty;

            RetVal = "SELECT " + DIColumns.Timeperiods.TimePeriod
                    + " FROM " + this.TablesName.TimePeriod
                    + " WHERE " + DIColumns.Timeperiods.TimePeriodNId + " NOT IN ( SELECT "
                    + DIColumns.Timeperiods.TimePeriodNId + " FROM " + this.TablesName.TimePeriod + ")";

            return RetVal;
        }

        /// <summary>
        /// AutoSelect TimePeriod based on Indiactor and Area selection
        /// </summary>
        /// <param name="indicatorNIds">commma delimited Indicator NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectByIndicatorArea(string indicatorNIds, string areaNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT DISTINCT T." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod);
            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D," + this.TablesName.TimePeriod + " AS T");

            if (indicatorNIds.Length > 0)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I," + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            }

            if (areaNIds.Length > 0)
            {
                sbQuery.Append("," + this.TablesName.Area + " AS A");
            }

            sbQuery.Append(" WHERE T." + DIColumns.Timeperiods.TimePeriodNId + " = D." + DIColumns.Data.TimePeriodNId);

            if (indicatorNIds.Length > 0)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId + " AND I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " AND I." + DIColumns.Indicator.IndicatorNId + " IN (" + indicatorNIds + ")");
            }

            if (areaNIds.Length > 0)
            {
                sbQuery.Append(" AND A." + DIColumns.Area.AreaNId + " = D." + DIColumns.Data.AreaNId + " AND A." + DIColumns.Area.AreaNId + " IN (" + areaNIds + ")");
            }

            sbQuery.Append(" ORDER BY T." + DIColumns.Timeperiods.TimePeriod + " DESC");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// AutoSelect TimePeriod based on Indiactor, Area, and source selection
        /// </summary>
        /// <param name="indicatorNIds">commma delimited Indicator NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="sourceNids">commma delimited Source NIds which may be blank</param>
        /// <param name="IUSNIds">commma delimited IUSNIds which may be blank </param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectByIndicatorAreaSource(string indicatorNIds, string areaNIds, string sourceNIds,string IUSNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT DISTINCT T." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod);
            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D," + this.TablesName.TimePeriod + " AS T");

            if (indicatorNIds.Length > 0)
            {
                sbQuery.Append("," + this.TablesName.Indicator + " AS I," + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            }

            sbQuery.Append(" WHERE T." + DIColumns.Timeperiods.TimePeriodNId + " = D." + DIColumns.Data.TimePeriodNId);

            if (indicatorNIds.Length > 0)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId + " AND I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " AND I." + DIColumns.Indicator.IndicatorNId + " IN (" + indicatorNIds + ") ");
            }

            if (areaNIds.Length > 0)
            {
                sbQuery.Append("  AND D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ") ");
            }

            if (sourceNIds.Length > 0)
            {
                sbQuery.Append("  AND D." + DIColumns.Data.SourceNId + " IN (" + sourceNIds + ") ");
            }

            if (IUSNIds.Length > 0)
            {
                sbQuery.Append("  AND D." + DIColumns.Data.IUSNId + " IN (" + IUSNIds + ") ");
            }

            sbQuery.Append(" ORDER BY T." + DIColumns.Timeperiods.TimePeriod + " DESC");

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// AutoSelect TimePeriod based on IUS and Area selection
        /// </summary>
        /// <param name="IUSNIds">commma delimited IUS NIds which may be blank</param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectByIUSArea(string IUSNIds, string areaNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT DISTINCT T." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod);
            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D," + this.TablesName.TimePeriod + " AS T");

            if (IUSNIds.Length > 0)
                sbQuery.Append("," + this.TablesName.IndicatorUnitSubgroup + " AS IUS");

            if (areaNIds.Length > 0)
            {
                sbQuery.Append("," + this.TablesName.Area + " AS A");
            }

            sbQuery.Append(" WHERE T." + DIColumns.Timeperiods.TimePeriodNId + " = D." + DIColumns.Data.TimePeriodNId);

            if (IUSNIds.Length > 0)
            {
                sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " = D." + DIColumns.Data.IUSNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + IUSNIds + ")");
            }
            if (areaNIds.Length > 0)
            {
                sbQuery.Append(" AND A." + DIColumns.Area.AreaNId + " = D." + DIColumns.Data.AreaNId + " AND A." + DIColumns.Area.AreaNId + " IN (" + areaNIds + ")");
            }

            sbQuery.Append(" ORDER BY T." + DIColumns.Timeperiods.TimePeriod + " DESC");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Auto Select IUS on the basis of Indicator NIds
        /// </summary>
        /// <param name="indicator_NId">Comma seprated indicator NIds</param>
        /// <returns></returns>
        public string GetAutoSelectIUSByIndicatorNId(string indicator_NId)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);

            sbQuery.Append(" FROM " + TablesName.Indicator + " I," + TablesName.IndicatorUnitSubgroup + " IUS");

            sbQuery.Append(" WHERE I." + DIColumns.Indicator.IndicatorNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId);
            sbQuery.Append(" AND I." + DIColumns.Indicator.IndicatorNId + " IN (" + indicator_NId + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get timeperiod on the basis of Area and IUS NIds.
        /// </summary>
        /// <param name="areaNIds">Comma seprated area NIds</param>
        /// <param name="iusNIds">Comma seprated indicator NIds</param>        
        /// <returns></returns>
        public string GetTimePeriodsNidsByAreaIUS(string areaNIds, string iusNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            String sWhere = string.Empty;

            sbQuery.Append("SELECT DISTINCT D." + DIColumns.Data.TimePeriodNId);
            sbQuery.Append(" FROM " + TablesName.Data + " D ");
            if (!string.IsNullOrEmpty(iusNIds) || !string.IsNullOrEmpty(areaNIds))
            {
                sbQuery.Append(" WHERE ");
            }

            // IUSNIds
            if (!string.IsNullOrEmpty(iusNIds))
            {
                if (sWhere.Length > 0)
                {
                    sWhere += " AND ";
                }
                sWhere += " D." + DIColumns.Data.IUSNId + " IN (" + iusNIds + ")";
            }
            // Area NIDs
            if (!string.IsNullOrEmpty(areaNIds))
            {
                if (sWhere.Length > 0)
                {
                    sWhere += " AND ";
                }
                sWhere += " D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ")";
            }
            sbQuery.Append(sWhere);

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get  TimePeriodNId, TimePeriod from TimePeriod table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId, Name, Search, NIdNotIn, NameNotIn</param>
        /// <param name="filterText">For FilterFieldType "Search" FilterText should be Timeperiods.TimePeriod LIKE '%2005%' or Timeperiods.TimePeriod LIKE '%1999-2001%'</param>
        /// <returns></returns>
        public string GetTimePeriod(FilterFieldType filterFieldType, string filterText)
        {
            return GetTimePeriod(filterFieldType, filterText, "");
        }

        /// <summary>
        /// Get  TimePeriodNId, TimePeriod from TimePeriod table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId, Name, Search, NIdNotIn, NameNotIn</param>
        /// <param name="filterText">For FilterFieldType "Search" FilterText should be Timeperiods.TimePeriod LIKE '%2005%' or Timeperiods.TimePeriod LIKE '%1999-2001%'</param>
        /// <param name="orderBy">order by clause like "TimePeriod DESC"</param>
        /// <returns></returns>
        public string GetTimePeriod(FilterFieldType filterFieldType, string filterText, string orderBy)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Timeperiods.TimePeriodNId + "," + DIColumns.Timeperiods.TimePeriod);

            sbQuery.Append(" FROM " + this.TablesName.TimePeriod);

            if (filterFieldType != FilterFieldType.None)
                sbQuery.Append(" WHERE ");

            if (filterText.Length > 0)
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DIColumns.Timeperiods.TimePeriodNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.ID:
                        break;
                    case FilterFieldType.GId:
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DIColumns.Timeperiods.TimePeriod + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(filterText);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(DIColumns.Timeperiods.TimePeriodNId + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DIColumns.Timeperiods.TimePeriod + " NOT IN (" + filterText + ")");
                        break;
                    default:
                        break;
                }
            }

            if (orderBy.Length > 0)
            {
                sbQuery.Append(" ORDER BY " + orderBy);
            }
            else
            {
                // sbQuery.Append(" ORDER BY " + DIColumns.Timeperiods.TimePeriod + " DESC");             
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get the timperiod and timeperiod NIds on the basis of timeperiod NIds
        /// </summary>
        /// <param name="timeperiodNIds"></param>
        /// <returns></returns>
        public string GetTimePeriod(string timeperiodNIds)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Timeperiods.TimePeriodNId + "," + DIColumns.Timeperiods.TimePeriod);

            sbQuery.Append(" FROM " + this.TablesName.TimePeriod);

            sbQuery.Append(" WHERE ");

            if (!string.IsNullOrEmpty(timeperiodNIds))
            {
                sbQuery.Append(DIColumns.Timeperiods.TimePeriodNId + " IN (" + timeperiodNIds + ")");
            }
            else
            {
                sbQuery.Append(DIColumns.Timeperiods.TimePeriodNId + " < 0");
            }
            //sbQuery.Append(" ORDER BY " + DIColumns.Timeperiods.TimePeriod + " DESC");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get master list of TimePeriodNId and TimePeriod in ascending order
        /// </summary>
        /// <returns></returns>
        public string GetAvailableTimePeriod()
        {
            string RetVal = "";
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT " + DIColumns.Timeperiods.TimePeriodNId + "," + DIColumns.Timeperiods.TimePeriod + " FROM " + this.TablesName.TimePeriod + " ORDER BY " + DIColumns.Timeperiods.TimePeriod + " ASC");
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get TimePeriods between a given range
        /// </summary>
        /// <param name="fromTimePeriod"></param>
        /// <param name="toTimePeriod"></param>
        /// <returns></returns>
        public string GetTimePeriodRange(string fromTimePeriod, string toTimePeriod)
        {
            string RetVal = "";
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT " + DIColumns.Timeperiods.TimePeriodNId + "," + DIColumns.Timeperiods.TimePeriod);
            sbQuery.Append(" FROM " + this.TablesName.TimePeriod);
            sbQuery.Append(" WHERE " + DIColumns.Timeperiods.TimePeriod + " BETWEEN '" + fromTimePeriod + "' AND '" + toTimePeriod + "'");
            sbQuery.Append(" ORDER BY " + DIColumns.Timeperiods.TimePeriod + " ASC");

            RetVal = sbQuery.ToString();
            return RetVal;

        }


        /// <summary>
        /// AutoSelect TimePeriod based on Indiactor, Area, and source selection
        /// </summary>
        /// <param name="indicatorNIds">commma delimited Indicator/IUS NIds which may be blank</param>
        /// <param name="showIUS"></param>
        /// <param name="areaNIds">commma delimited Area NIds which may be blank</param>
        /// <param name="sourceNIds">commma delimited Source NIds which may be blank</param>
        /// <returns>Sql query string</returns>
        public string GetAutoSelectTimeperiod(string indicatorNIds, bool showIUS, string areaNIds, string sourceNIds)
        {
            string RetVal = string.Empty;

            // Timeperiod Table
            RetVal = "SELECT  T." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod + " FROM " + this.TablesName.TimePeriod + " AS T ";


            // Data table
            RetVal += " WHERE  EXISTS ( SELECT *  FROM  " + this.TablesName.Data + " AS D WHERE  T." + DIColumns.Timeperiods.TimePeriodNId + "= D." + DIColumns.Data.TimePeriodNId;


            //if indicator nids is given
            if (!string.IsNullOrEmpty(indicatorNIds))
            {
                if (!showIUS)
                {
                    //RetVal += " AND EXISTS ";

                    ////Indicator
                    //RetVal += " ( SELECT * FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS WHERE D." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND EXISTS ";

                    //RetVal += " ( SELECT * FROM " + this.TablesName.Indicator + " AS I  WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "= I." + DIColumns.Indicator.IndicatorNId + " AND I." + DIColumns.Indicator.IndicatorNId + " IN(" + indicatorNIds + ") ";

                    //RetVal += " )) ";

                    RetVal += " AND D." + DIColumns.Data.IndicatorNId + " IN( " + indicatorNIds + " ) ";
                }
                else
                {
                    //RetVal += " AND EXISTS ";

                    ////Indicator
                    //RetVal += " ( SELECT * FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS WHERE D." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN(" + indicatorNIds + ") ";

                    //RetVal += " ) ";

                    RetVal += " AND D." + DIColumns.Data.IUSNId + " IN( " + indicatorNIds + " ) ";
                }
            }


            if (!string.IsNullOrEmpty(areaNIds))
            {
                RetVal += " AND D." + DIColumns.Data.AreaNId + " IN( " + areaNIds + " ) ";
            }

            if (!string.IsNullOrEmpty(sourceNIds))
            {
                RetVal += " AND D." + DIColumns.Data.SourceNId + " IN( " + sourceNIds + " ) ";
            }

            RetVal += ") ORDER BY T." + DIColumns.Timeperiods.TimePeriod + " DESC";

            return RetVal;
        }

        /// <summary>
        /// Get timeperiods between from and to time periods. if toTimeperiod is empty, it will return Timperiods greater then fromTimeperiods
        /// </summary>
        /// <param name="fromTimperiod"></param>
        /// <param name="toTimeperiod"></param>
        /// <returns></returns>
        public string GetTimePeriodsBetween(string fromTimperiod, string toTimeperiod)
        {
            string RetVal = string.Empty;

            try
            {
                RetVal = " SELECT * FROM " + this.TablesName.TimePeriod + " T ";
                if (!string.IsNullOrEmpty(toTimeperiod))
                {
                    RetVal += " WHERE T." + DIColumns.Timeperiods.TimePeriod + " BETWEEN '" + fromTimperiod + "' and '" + toTimeperiod + "'";
                }
                else if (!string.IsNullOrEmpty(fromTimperiod))
                {
                    RetVal += " WHERE T." + DIColumns.Timeperiods.TimePeriod + " >= '" + fromTimperiod + "'";
                }
            }
            catch (Exception)
            {
            }

            return RetVal;
        }

        /// <summary>
        /// Get auto selected timeperiods between from and to time periods. if toTimeperiod is empty, it will return Timperiods greater then fromTimeperiods
        /// </summary>
        /// <param name="fromTimperiod"></param>
        /// <param name="toTimeperiod"></param>
        /// <param name="userSelection"></param>
        /// <returns></returns>
        public string GetAutoSelectedTimePeriodsRange(string fromTimperiod, string toTimeperiod, UserSelection.UserSelection userSelection)
        {
            string RetVal = string.Empty;

            try
            {
                RetVal = " SELECT DISTINCT T." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod;
                RetVal += " FROM " + this.TablesName.TimePeriod + " T," + this.TablesName.Data + " D";

                RetVal += " WHERE T." + DIColumns.Timeperiods.TimePeriodNId + " = D." + DIColumns.Data.TimePeriodNId;

                if (!string.IsNullOrEmpty(userSelection.IndicatorNIds))
                {
                    if (userSelection.ShowIUS)
                    {
                        RetVal += " AND D." + DIColumns.Data.IUSNId + " IN (" + userSelection.IndicatorNIds + ")";
                    }
                    else
                    {
                        RetVal += " AND D." + DIColumns.Data.IndicatorNId + " IN (" + userSelection.IndicatorNIds + ")";
                    }
                }

                if (!string.IsNullOrEmpty(userSelection.AreaNIds))
                {
                    RetVal += " AND D." + DIColumns.Data.AreaNId + " IN (" + userSelection.AreaNIds + ")";
                }

                if (!string.IsNullOrEmpty(userSelection.SourceNIds))
                {
                    RetVal += " AND D." + DIColumns.Data.SourceNId + " IN (" + userSelection.SourceNIds + ")";
                }

                if (!string.IsNullOrEmpty(toTimeperiod))
                {
                    RetVal += " AND T." + DIColumns.Timeperiods.TimePeriod + " BETWEEN '" + fromTimperiod + "' and '" + toTimeperiod + "'";
                }
                else if (!string.IsNullOrEmpty(fromTimperiod))
                {
                    RetVal += " AND T." + DIColumns.Timeperiods.TimePeriod + " >= '" + fromTimperiod + "'";
                }
            }
            catch (Exception)
            {
            }

            return RetVal;
        }

        /// <summary>
        /// Get Duplicate Timeperiods
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateTimeperiod()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            SbQuery.Append("SELECT " + DIColumns.Timeperiods.TimePeriod + " FROM " + this.TablesName.TimePeriod);

            SbQuery.Append(" GROUP BY " + DIColumns.Timeperiods.TimePeriod + " HAVING COUNT(*)>1");

            RetVal = SbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Select timeperiod from its maximum nid
        /// </summary>
        /// <returns></returns>
        public string GetTimePeriodFromMaximumNId()
        {
            string RetVal = string.Empty;
            try
            {
                RetVal = "SELECT * FROM " + this.TablesName.TimePeriod + " WHERE " + DIColumns.Timeperiods.TimePeriodNId + " IN (SELECT MAX( " + DIColumns.Timeperiods.TimePeriodNId + ") FROM " + this.TablesName.TimePeriod + " )";
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Get the Duplicate TimePeriod By Nid
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateTimePeriodByNid()
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.Timeperiods.TimePeriodNId + "," + DIColumns.Timeperiods.TimePeriod + " FROM " + this.TablesName.TimePeriod + " WHERE " + DIColumns.Timeperiods.TimePeriodNId + " IN(");

            SqlQuery.Append(" SELECT " + DIColumns.Timeperiods.TimePeriodNId + " FROM ");

            SqlQuery.Append(this.TablesName.TimePeriod + " GROUP BY " + DIColumns.Timeperiods.TimePeriodNId);

            SqlQuery.Append(" HAVING COUNT(*) >1 )");
            return SqlQuery.ToString();
        }
        #endregion

        #endregion

    }
}
