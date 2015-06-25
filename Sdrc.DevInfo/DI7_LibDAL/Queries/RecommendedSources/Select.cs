using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.UserSelection;

namespace DevInfo.Lib.DI_LibDAL.Queries.RecommendedSources
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

        /// <summary>
        /// Gets DataNIds form data table based on Indiactor, Timeperiod, Area and Source selection
        /// </summary>
        /// <param name="fieldSelection">NId, Name, Light, Heavy</param>
        /// <param name="dataNIdOnly">select DataNId, IUSNId, SourceNId columns from data table only</param>
        /// <returns></returns>
        public string GetAllRecordsFromData(FieldSelection fieldSelection, bool dataNIdOnly,string searchText)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            // SELECT Clause

            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    if (dataNIdOnly)
                    {
                        sbQuery.Append("SELECT D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Data.SourceNId);
                    }
                    else
                    {
                        sbQuery.Append("SELECT D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Data.TimePeriodNId + ",D." + DIColumns.Data.AreaNId + ",D." + DIColumns.Data.DataValue + ",D." + DIColumns.Data.SourceNId);
                    }

                    break;
                case FieldSelection.Name:
                    sbQuery.Append("SELECT I." + DIColumns.Indicator.IndicatorName + ",U." + DIColumns.Unit.UnitName + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",T." + DIColumns.Timeperiods.TimePeriod + ",A." + DIColumns.Area.AreaName + ",D." + DIColumns.Data.DataValue + ",IC." + DIColumns.IndicatorClassifications.ICName);
                    break;
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append("SELECT D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorName + ",D." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",D." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal  + ",D." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaName +",T." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod + ",D." + DIColumns.Data.SourceNId + ",IC." + DIColumns.IndicatorClassifications.ICName + ",D." + DIColumns.Data.ICIUSOrder );
                   break;
                default:
                    break;
            }

            // FROM Clause
            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append(" FROM " + this.TablesName.Data + " AS D");
                    break;

                case FieldSelection.Name:
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericFromClause());
                    break;

                default:
                    break;
            }

            // WHERE Clause
            switch (fieldSelection)
            {
                case FieldSelection.NId:
                    sbQuery.Append(" WHERE 1=1 ");
                    break;
                case FieldSelection.Name:
                case FieldSelection.Light:
                case FieldSelection.Heavy:
                    sbQuery.Append(this.GetGenericWhereClause());
                    break;
                default:
                    break;
            }

            //sbQuery.Append(" AND D." + searchText);

            RetVal = sbQuery.ToString();
            return RetVal;

        }
        /// <summary>
        /// Rerturns sql query to get recommended sources for the given Nids.Any NIds can be empty.
        /// </summary>
        /// <param name="IUSNIds"></param>
        /// <param name="areaNIds"></param>
        /// <param name="timeperiodNIds"></param>
        /// <param name="sourceNIds"></param>
        /// <param name="ICNIds"></param>
        /// <param name="selectedICType"></param>
        /// <returns></returns>
        public string GetAllRecordsByIUSTimeperiodAreaNSource(string IUSNIds, string areaNIds, string timeperiodNIds, string sourceNIds,string ICNIds ,ICType selectedICType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT D." + DIColumns.Data.DataNId + ",D." + DIColumns.Data.IUSNId + ",D." + DIColumns.Indicator.IndicatorNId + ",I." + DIColumns.Indicator.IndicatorGlobal + ",I." + DIColumns.Indicator.IndicatorName + ",D." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitGlobal + ",U." + DIColumns.Unit.UnitName + ",D." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",D." + DIColumns.Area.AreaNId + ",A." + DIColumns.Area.AreaGlobal + ",A." + DIColumns.Area.AreaName + ",T." + DIColumns.Timeperiods.TimePeriodNId + ",T." + DIColumns.Timeperiods.TimePeriod + ",D." + DIColumns.Data.SourceNId + ",IC." + DIColumns.IndicatorClassifications.ICGlobal + ",IC." + DIColumns.IndicatorClassifications.ICName + ",D." + DIColumns.Data.ICIUSOrder);

            sbQuery.Append(this.GetGenericFromClause());

            if (ICNIds.Length > 0 && string.IsNullOrEmpty(IUSNIds))
            {
                sbQuery.Append("," + this.TablesName.IndicatorClassifications + " AS IC1 ");
                sbQuery.Append("," + this.TablesName.IndicatorClassificationsIUS + " AS ICIUS ");
            }

            sbQuery.Append(" WHERE ");
            sbQuery.Append(" D." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = I." + DIColumns.Indicator.IndicatorNId);
            sbQuery.Append(" AND D." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = U." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND D." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId);
            sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " = T." + DIColumns.Timeperiods.TimePeriodNId);
            sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " = A." + DIColumns.Area.AreaNId);
            sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " = IC." + DIColumns.IndicatorClassifications.ICNId);
            
            if (!string.IsNullOrEmpty(IUSNIds))
            {
                sbQuery.Append(" AND D." + DIColumns.Data.IUSNId + " IN(" + IUSNIds + ")");
            }
            if (!string.IsNullOrEmpty(areaNIds))
            {
                sbQuery.Append(" AND D." + DIColumns.Data.AreaNId + " IN(" + areaNIds + ")");
            }
            if (!string.IsNullOrEmpty(timeperiodNIds))
            {
                sbQuery.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN(" + timeperiodNIds + ")");
            }
            if (!string.IsNullOrEmpty(sourceNIds))
            {
                sbQuery.Append(" AND D." + DIColumns.Data.SourceNId + " IN(" + sourceNIds + ")");
            }


            if (ICNIds.Length > 0 && string.IsNullOrEmpty(IUSNIds))
            {
                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassifications.ICNId + " =IC1." + DIColumns.IndicatorClassifications.ICNId);
                sbQuery.Append(" AND ICIUS." + DIColumns.IndicatorClassificationsIUS.IUSNId + " =D." + DIColumns.Data.IUSNId);


                if (ICNIds != "-1")
                {
                    sbQuery.Append(" AND IC1." + DIColumns.IndicatorClassifications.ICNId + " IN(" + ICNIds + ")");
                }

                sbQuery.Append(" AND IC1." + DIColumns.IndicatorClassifications.ICType + "=" + DIQueries.ICTypeText[selectedICType]);
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Recommended Source DataNid,Rank and Label for given
        /// </summary>
        /// <param name="dataNids"></param>
        /// <returns></returns>
        public string GetRecommendedSources( string dataNids)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.RecommendedSources.NId + "," + DIColumns.RecommendedSources.DataNId + "," + DIColumns.RecommendedSources.ICIUSLabel + " From " + this.TablesName.RecommendedSources);
            
            if (!string.IsNullOrEmpty(dataNids))
            { 
             sbQuery.Append(" WHERE "+ DIColumns.RecommendedSources.DataNId + " IN ("+ dataNids +")");
            }

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Returns query to get records for entries of recommended sources 
        /// </summary>
        /// <returns></returns>
        public string GetForRecommendedSourceEntries(string IUSNIds, string timeperodNIds, string areaNIds, string icNIds)
        {
            string RetVal = string.Empty;

            StringBuilder sSql = new System.Text.StringBuilder();

            sSql.Append("SELECT DISTINCT D." + DIColumns.Data.ICIUSOrder + ",D." + DIColumns.Data.IUSNId + ",");
            sSql.Append(" D." + DIColumns.Data.AreaNId + ", D." + DIColumns.Data.SourceNId + ", D." + DIColumns.Data.TimePeriodNId + ", ");
            sSql.Append(" I." + DIColumns.Indicator.IndicatorName + ", U." + DIColumns.Unit.UnitName + ", S." + DIColumns.SubgroupVals.SubgroupVal + ", IC." + DIColumns.IndicatorClassifications.ICName + ", ");
            sSql.Append(" I." + DIColumns.Indicator.IndicatorGlobal + ", U." + DIColumns.Unit.UnitGlobal + ", S." + DIColumns.SubgroupVals.SubgroupValGlobal + ", IC." + DIColumns.IndicatorClassifications.ICGlobal + ", ");
            sSql.Append(" IC." + DIColumns.IndicatorClassifications.ICNId + ", D." + DIColumns.Data.DataNId);

            sSql.Append(" FROM " + this.TablesName.Data + " D, ");
            sSql.Append(this.TablesName.Indicator + " I, ");
            sSql.Append(this.TablesName.Unit + " U, ");
            sSql.Append(this.TablesName.SubgroupVals + " S, ");
            sSql.Append(this.TablesName.IndicatorClassifications + " IC ");

            sSql.Append(" WHERE ");
            sSql.Append(" I." + DIColumns.Indicator.IndicatorNId + "= D." + DIColumns.Data.IndicatorNId + " AND ");
            sSql.Append(" U." + DIColumns.Unit.UnitNId + "= D." + DIColumns.Data.UnitNId + " AND ");
            sSql.Append(" S." + DIColumns.SubgroupVals.SubgroupValNId + "= D." + DIColumns.SubgroupVals.SubgroupValNId + " AND ");
            sSql.Append(" IC." + DIColumns.IndicatorClassifications.ICNId + "= D." + DIColumns.Data.SourceNId);

            if (!string.IsNullOrEmpty(IUSNIds.Trim()))
            {
                sSql.Append(" AND D." + DIColumns.Data.IUSNId + " IN (" + IUSNIds + ")");
            }

            if (!string.IsNullOrEmpty(timeperodNIds.Trim()))
            {
                sSql.Append(" AND D." + DIColumns.Data.TimePeriodNId + " IN (" + timeperodNIds + ")");
            }

            if (!string.IsNullOrEmpty(areaNIds.Trim()))
            {
                sSql.Append(" AND D." + DIColumns.Data.AreaNId + " IN (" + areaNIds + ")");
            }

            if (!string.IsNullOrEmpty(icNIds.Trim()))
            {
                sSql.Append(" AND D." + DIColumns.Data.SourceNId + " IN (" + icNIds + ")");
            }

            sSql.Append(" ORDER BY I." + DIColumns.Indicator.IndicatorName + " Asc, U." + DIColumns.Unit.UnitName + " Asc, S." + DIColumns.SubgroupVals.SubgroupVal + " Asc ");

            RetVal = sSql.ToString();

            return RetVal;
        }

        /// <summary>
        /// Query return the auto selected distinct ranks
        /// </summary>
        /// <param name="dataNIDs"></param>
        /// <returns></returns>
        public string GetAutoSelectedDistinctRank(string dataNIDs)
        {
            string RetVal = string.Empty;
            try
            {
                RetVal = "SELECT DISTINCT D." + DIColumns.Data.ICIUSOrder;

                RetVal += " FROM " + this.TablesName.Data + " D";

                RetVal += " WHERE D." + DIColumns.Data.DataNId + " IN (" + dataNIDs + ")";
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Query return the auto selected distinct labels
        /// </summary>
        /// <param name="dataNIDs"></param>
        /// <returns></returns>
        public string GetAutoSelectedDistinctLabel(string dataNIDs)
        {
            string RetVal = string.Empty;
            try
            {
                RetVal = "SELECT DISTINCT RS." + DIColumns.RecommendedSources.ICIUSLabel;

                RetVal += " FROM " + this.TablesName.RecommendedSources + " RS";

                RetVal += " WHERE RS." + DIColumns.RecommendedSources.DataNId + " IN (" + dataNIDs + ")";
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Query return the DataNIDs by RANK
        /// </summary>
        /// <param name="ranks"></param>
        /// <returns></returns>
        public string GetDataNIDsByRank(string ranks)
        {
            string RetVal = string.Empty;
            try
            {
                RetVal = "SELECT DISTINCT D." + DIColumns.Data.DataNId;

                RetVal += " FROM " + this.TablesName.Data + " D ";

                RetVal += " WHERE D." + DIColumns.Data.ICIUSOrder + " IN (" + ranks + ")";
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Query return the DataNIDs by LABEL
        /// </summary>
        /// <param name="labels"></param>
        /// <returns></returns>
        public string GetDataNIDsByLabel(string labels)
        {
            string RetVal = string.Empty;
            try
            {
                RetVal = "SELECT DISTINCT RS." + DIColumns.RecommendedSources.DataNId;

                RetVal += " FROM " + this.TablesName.RecommendedSources + " RS ";

                RetVal += " WHERE RS." + DIColumns.RecommendedSources.ICIUSLabel + " LIKE '" + labels + "'";
            }
            catch (Exception)
            {
            }
            return RetVal;
        }


        #endregion

        #endregion

    }
}

