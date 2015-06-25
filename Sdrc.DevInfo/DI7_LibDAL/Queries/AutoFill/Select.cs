using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.UserSelection;

namespace DevInfo.Lib.DI_LibDAL.Queries.AutoFill
{
    /// <summary>
    /// Provides sql queries to get auto records (means records where data exists)
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

        #region "-- Method --"
        /// <summary>
        /// Get generic FROM sql syntax for data table
        /// </summary>
        /// <returns></returns>
        private string GetGenericFromClause()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // FROM Clause
            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D ");
            sbQuery.Append("," + this.TablesName.Indicator + " AS I," + this.TablesName.Unit + " AS U," + this.TablesName.SubgroupVals + " AS SGV");
            sbQuery.Append("," + this.TablesName.TimePeriod + " AS T," + this.TablesName.Area + " AS A");
            sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");


            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get generic WHERE sql syntax for data table
        /// </summary>
        /// <returns></returns>
        private string GetGenericWhereClause()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // WHERE Clause
            sbQuery.Append(" WHERE ");
            sbQuery.Append(" D." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND D." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND D." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " = T." + DIColumns.Timeperiods.TimePeriodNId);
            sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " = A." + DIColumns.Area.AreaNId);
            sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);

            RetVal = sbQuery.ToString();

            return RetVal;
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
    
        ///////// <summary>
        ///////// if userSelections.IndicatorNIds.length ==0 & userSelections.ICNIds.length>0 then
        ///////// Selection will include Area NIds,timeperiod NIds, source NIds & IC NIds
        ///////// </summary>
        ///////// <param name="userSelections"></param>
        ///////// <returns></returns>
        //////public string GetAutoAreaTimeperiodByIUS(ICType icType,UserSelection.UserSelection userSelections)
        //////{
        //////    string RetVal = string.Empty;
        //////    StringBuilder sbQuery = new StringBuilder();

        //////    sbQuery.Append("SELECT D." + DIColumns.Data.IndicatorNId + ",D." + DIColumns.Data.UnitNId + ",D." + DIColumns.Data.SubgroupValNId + ",D." + DIColumns.Data.AreaNId + ",D." + DIColumns.Data.TimePeriodNId + ",D." + DIColumns.Data.SourceNId); 
        //////    sbQuery.Append(",IC." + DIColumns.IndicatorClassifications.ICNId);

        //////    sbQuery.Append(" FROM " + this.TablesName.Data + " AS D");
        //////    sbQuery.Append("," + this.TablesName.TimePeriod + " AS T," + this.TablesName.Area + " AS A");
        //////    sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");
        //////    sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");

        //////    sbQuery.Append(" WHERE ");
        //////    sbQuery.Append(" D." + DIColumns.Data.TimePeriodNId + " = T." + DIColumns.Timeperiods.TimePeriodNId);
        //////    sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " = A." + DIColumns.Area.AreaNId);
        //////    sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId  + "=IC." + DIColumns.IndicatorClassifications.ICNId);
        //////    sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + "=ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId);

        //////    if (userSelections.ICNIds.Length > 0)
        //////    {
        //////        sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[icType]);
        //////        sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN(" + userSelections.ICNIds + ")");
        //////    }

        //////    RetVal = sbQuery.ToString();
        //////    return RetVal;
        //////}

        /// <summary>
        ///// Selection will include IndicatorNid/IUS Area NIds,timeperiod NIds, source NIds 
        ///// </summary>
        ///// <param name="userSelections"></param>
        ///// <returns></returns>
        //public string GetAutoAreaTimeperiodExceptIC(UserSelection.UserSelection userSelections)
        //{
        //    string RetVal = string.Empty;
        //    StringBuilder sbQuery = new StringBuilder();

        //    sbQuery.Append("SELECT D." + DIColumns.Data.IndicatorNId + ",D." + DIColumns.Data.UnitNId + ",D." + DIColumns.Data.SubgroupValNId + ",D." + DIColumns.Data.AreaNId + ",D." + DIColumns.Data.TimePeriodNId + ",D." + DIColumns.Data.SourceNId);

        //    sbQuery.Append(" FROM " + this.TablesName.Data + " AS D");
        //    sbQuery.Append("," + this.TablesName.TimePeriod + " AS T," + this.TablesName.Area + " AS A");

        //    sbQuery.Append(" WHERE ");
        //    sbQuery.Append(" D." + DIColumns.Data.TimePeriodNId + " = T." + DIColumns.Timeperiods.TimePeriodNId);
        //    sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " = A." + DIColumns.Area.AreaNId);

        //    RetVal = sbQuery.ToString();
        //    return RetVal;
        //}

        ///// <summary>
        /// Get IUS for the given userselection & selectedICType
        /// </summary>
        /// <param name="selectedICType"></param>
        /// <param name="userSelections"></param>
        /// <returns></returns>
        public string GetAutoDistinctIUS(ICType selectedICType, UserSelection.UserSelection userSelections)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT  I." + DIColumns.Indicator.IndicatorName + ", I." + DIColumns.Indicator.IndicatorGlobal + ", U." + DIColumns.Unit.UnitName + ", U." + DIColumns.Unit.UnitGlobal + ", S." + DIColumns.SubgroupVals.SubgroupVal + ", S." + DIColumns.SubgroupVals.SubgroupValGlobal + ", I." + DIColumns.Indicator.IndicatorNId + ", U." + DIColumns.Unit.UnitNId + ", S." + DIColumns.SubgroupVals.SubgroupValNId + ", IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);

            sbQuery.Append(" FROM " + this.TablesName.IndicatorUnitSubgroup + " AS IUS");
            sbQuery.Append("," + this.TablesName.Indicator + " AS I");
            sbQuery.Append("," + this.TablesName.Unit + " AS U");
            sbQuery.Append("," + this.TablesName.SubgroupVals + " AS S");

            sbQuery.Append(" WHERE ");

            if (userSelections.ICNIds.Length > 0 && string.IsNullOrEmpty(userSelections.IndicatorNIds))
            {
                sbQuery.Append(" EXISTS ( Select * FROM " + this.TablesName.IndicatorClassifications + " AS IC," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");

                sbQuery.Append(" WHERE  ICIUS." + DIColumns.IndicatorClassifications.ICNId + " =IC." + DIColumns.IndicatorClassifications.ICNId);
                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " =IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId);


                if (userSelections.ICNIds != "-1")
                {
                    sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN(" + userSelections.ICNIds + ")");
                }

                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[selectedICType] + " ) AND ");


            }


            sbQuery.Append(" IUS." + DIColumns.Data.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND IUS." + DIColumns.Data.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND IUS." + DIColumns.Data.SubgroupValNId + " = S." + DIColumns.SubgroupVals.SubgroupValNId);

            sbQuery.Append(" AND EXISTS ( SELECT * FROM " + TablesName.Data + " D WHERE D." + DIColumns.Data.IUSNId + "=" + "IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " ");

            if (userSelections.AreaNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " IN(" + userSelections.AreaNIds + ")");
            }
            if (userSelections.TimePeriodNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN(" + userSelections.TimePeriodNIds + ")");
            }
            if (userSelections.SourceNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN(" + userSelections.SourceNIds + ")");
            }

            if (userSelections.ShowIUS & userSelections.IndicatorNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN(" + userSelections.IndicatorNIds + ")");
            }
            else if (userSelections.IndicatorNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.IndicatorNId + " IN(" + userSelections.IndicatorNIds + ")");
            }

            sbQuery.Append(")");


            RetVal = sbQuery.ToString();
            return RetVal;

        }

        /// <summary>
        /// Get AreaBy ICNid or AreaNids
        /// </summary>
        /// <param name="selectedICType"></param>
        /// <param name="userSelections"></param>
        /// <returns></returns>
        public string GetAutoDistinctArea(ICType selectedICType, UserSelection.UserSelection userSelections)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT  A." + DIColumns.Data.AreaNId + ", A." + DIColumns.Area.AreaGlobal + ", A." + DIColumns.Area.AreaName);
            sbQuery.Append(" FROM " + this.TablesName.Area + " AS A ");
            sbQuery.Append(" WHERE  EXISTS ( Select * FROM " + this.TablesName.Data + " AS D ");

            if (userSelections.ICNIds.Length > 0 && string.IsNullOrEmpty(userSelections.IndicatorNIds))
            {
                sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");
                sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            }


            sbQuery.Append(" WHERE D." + DIColumns.Data.AreaNId + " = A." + DIColumns.Area.AreaNId);

            if (userSelections.AreaNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " IN(" + userSelections.AreaNIds + ")");
            }
            if (userSelections.TimePeriodNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN(" + userSelections.TimePeriodNIds + ")");
            }
            if (userSelections.SourceNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN(" + userSelections.SourceNIds + ")");
            }

            if (userSelections.ShowIUS & userSelections.IndicatorNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN(" + userSelections.IndicatorNIds + ")");
            }
            else if (userSelections.IndicatorNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.IndicatorNId + " IN(" + userSelections.IndicatorNIds + ")");
            }

            if (userSelections.ICNIds.Length > 0 && string.IsNullOrEmpty(userSelections.IndicatorNIds))
            {

                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " =IC." + DIColumns.IndicatorClassifications.ICNId);
                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " =D." + DIColumns.Data.IUSNId);


                if (userSelections.ICNIds != "-1")
                {
                    sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN(" + userSelections.ICNIds + ")");
                }

                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[selectedICType]);

            }

            sbQuery.Append(") ORDER BY A." + DIColumns.Area.AreaName + " DESC ");

            RetVal = sbQuery.ToString();
            return RetVal;

        }

        /// <summary>
        /// Get Timeperiod By ICNId ICNId or TimeperiodNids
        /// </summary>
        /// <param name="selectedICType"></param>
        /// <param name="userSelections"></param>
        /// <returns></returns>
        public string GetAutoDistinctTimeperiod(ICType selectedICType, UserSelection.UserSelection userSelections)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT Distinct D." + DIColumns.Data.TimePeriodNId + ", T." + DIColumns.Timeperiods.TimePeriod);

            sbQuery.Append(" FROM " + this.TablesName.Data + " AS D");
            sbQuery.Append("," + this.TablesName.TimePeriod + " AS T");

            if (userSelections.ICNIds.Length > 0 && string.IsNullOrEmpty(userSelections.IndicatorNIds))
            {
                sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC");
                sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS");
            }

            sbQuery.Append(" WHERE ");
            sbQuery.Append(" D." + DIColumns.Data.TimePeriodNId + " = T." + DIColumns.Timeperiods.TimePeriodNId);

            if (userSelections.AreaNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " IN(" + userSelections.AreaNIds + ")");
            }
            if (userSelections.TimePeriodNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN(" + userSelections.TimePeriodNIds + ")");
            }
            if (userSelections.SourceNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN(" + userSelections.SourceNIds + ")");
            }

            if (userSelections.ShowIUS & userSelections.IndicatorNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN(" + userSelections.IndicatorNIds + ")");
            }
            else if (userSelections.IndicatorNIds.Length > 0)
            {
                sbQuery.Append(" AND D." + DIColumns.Data.IndicatorNId + " IN(" + userSelections.IndicatorNIds + ")");
            }

            if (userSelections.ICNIds.Length > 0 && string.IsNullOrEmpty(userSelections.IndicatorNIds))
            {
                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.ICNId + " =IC." + DIColumns.IndicatorClassifications.ICNId);
                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " =D." + DIColumns.Data.IUSNId);

                if (userSelections.ICNIds != "-1")
                {
                    sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICNId + " IN(" + userSelections.ICNIds + ")");
                }

                sbQuery.Append(" AND IC." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[selectedICType]);

            }

            RetVal = sbQuery.ToString();
            return RetVal;

        }

        #endregion

        #endregion

    }
}

