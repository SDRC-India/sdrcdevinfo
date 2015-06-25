using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Unit
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
        /// Get  NId, Name, GID, Global from Unit table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId,GId,Name,Search,NIdNotIn,NameNotIn</param>
        /// <param name="filterText">For FilterFieldType "Search" FilterText should be Unit.UnitName LIKE '%Number%' or Unit.UnitName LIKE '%Percent%'</param>
        /// <returns></returns>
        public string GetUnit(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Unit.UnitNId + "," + DIColumns.Unit.UnitName + "," + DIColumns.Unit.UnitGId + "," + DIColumns.Unit.UnitGlobal);

            sbQuery.Append(" FROM " + this.TablesName.Unit);

            if (filterFieldType != FilterFieldType.None)
                sbQuery.Append(" WHERE ");

            if (filterText.Length > 0)
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DIColumns.Unit.UnitNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.ID:
                        break;
                    case FilterFieldType.GId:
                        sbQuery.Append(DIColumns.Unit.UnitGId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DIColumns.Unit.UnitName + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Type:
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(filterText);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(DIColumns.Unit.UnitNId + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DIColumns.Unit.UnitName + " NOT IN (" + filterText + ")");
                        break;
                    default:
                        break;
                }
            }
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get IndGId_UnitGId for IndicatorNId_UnitNIds 
        /// </summary>
        /// <param name="IndicatorNId_UnitNIds"></param>
        /// <param name="DIServerType">Different SQL syntax based on datatype</param>
        /// <returns></returns>
        internal string GetIndGId_UnitGIds(string IndicatorNId_UnitNIds, DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorGId + ",U." + DIColumns.Unit.UnitGId, ",", Delimiter.TEXT_SEPARATOR, DIServerType));

            sbQuery.Append(" FROM " + this.TablesName.Indicator + " AS I," + this.TablesName.Unit + " AS U");

            sbQuery.Append(" WHERE (" + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorNId + ",U." + DIColumns.Unit.UnitNId, ",", Delimiter.NUMERIC_SEPARATOR, DIServerType) + ") IN (" + IndicatorNId_UnitNIds + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        internal string GetIndicatorNId_UnitNIds(string IndGId_UnitGIds, DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorNId + ",U." + DIColumns.Unit.UnitNId, ",", Delimiter.NUMERIC_SEPARATOR, DIServerType));

            sbQuery.Append(" FROM " + this.TablesName.Indicator + " AS I," + this.TablesName.Unit + " AS U");

            sbQuery.Append(" WHERE (" + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorGId + ",U." + DIColumns.Unit.UnitGId, ",", Delimiter.TEXT_SEPARATOR, DIServerType) + ") IN (" + IndGId_UnitGIds + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get the Units on the basis of IUS
        /// </summary>
        /// <param name="iusNId"></param>
        /// <param name="showIUS"></param>
        /// <returns></returns>
        public string GetUnits(string iusNId)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",U." + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGlobal + ",U." + DIColumns.Unit.UnitGId);
            sbQuery.Append(" FROM " + TablesName.IndicatorUnitSubgroup + " IUS," + TablesName.Unit + " U");
            sbQuery.Append(" WHERE U." + DIColumns.Unit.UnitNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId);

            if (string.IsNullOrEmpty(iusNId))
            {
                iusNId = "-1";
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + iusNId + ")");
            sbQuery.Append(" ORDER BY U." + DIColumns.Unit.UnitName);
            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Nid and Name where two language table have different Gid with same Nid
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="langCode"></param>
        /// <returns></returns>
        public string GetUnmatchedGidForLanguage(string dataPrefix, string langCode)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            DITables table = new DITables(dataPrefix, langCode);

            sbQuery.Append("SELECT U." + DIColumns.Unit.UnitNId + " AS " + DIColumns.Unit.UnitNId + ",U." + DIColumns.Unit.UnitName + " AS " + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitGId + " AS " + DIColumns.Unit.UnitGId);
            sbQuery.Append(", U1." + DIColumns.Unit.UnitNId + ",U1." + DIColumns.Unit.UnitName + ",U1." + DIColumns.Unit.UnitGId);

            sbQuery.Append(" FROM " + this.TablesName.Unit + " U," + table.Unit + " U1 ");

            sbQuery.Append(" WHERE ");
            sbQuery.Append(" U." + DIColumns.Unit.UnitNId + "= U1." + DIColumns.Unit.UnitNId);
            sbQuery.Append(" AND U." + DIColumns.Unit.UnitGId + "<> U1." + DIColumns.Unit.UnitGId);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        #region "-- Exchange --"

        /// <summary>
        /// Returns query to get units for the given indicator
        /// </summary>
        /// <param name="indicatorNid"></param>
        /// <returns></returns>
        public string GetUnitByIndicator(int indicatorNid)
        {
            string RetVal = string.Empty;

            RetVal = "Select Distinct U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitNId +
                ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal + " From " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.Unit + " U"
                 + " Where IUS." + DIColumns.Unit.UnitNId + "= U." + DIColumns.Unit.UnitNId +
                 " and IUS." + DIColumns.Indicator.IndicatorNId + "=" + indicatorNid + " ORDER BY " + DIColumns.Unit.UnitName;

            return RetVal;
        }

        /// <summary>
        /// Returns query to get units for the given indicatorNids
        /// </summary>
        /// <param name="indicatorNids"></param>
        /// <returns></returns>
        public string GetUnitByIndicator(string indicatorNids)
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("Select Distinct U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitNId);

            RetVal.Append(",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal + " From " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.Unit + " U");

            RetVal.Append(" Where IUS." + DIColumns.Unit.UnitNId + "= U." + DIColumns.Unit.UnitNId);

            if (!string.IsNullOrEmpty(indicatorNids))
                RetVal.Append(" and IUS." + DIColumns.Indicator.IndicatorNId + " in(" + indicatorNids + ")");

            RetVal.Append(" ORDER BY " + DIColumns.Unit.UnitName);


            //RetVal = "Select Distinct U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitNId +
            //    ",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal + " From " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.Unit + " U"
            //     + " Where IUS." + DIColumns.Unit.UnitNId + "= U." + DIColumns.Unit.UnitNId +
            //     " and IUS." + DIColumns.Indicator.IndicatorNId + " in(" + indicatorNids + ") ORDER BY " + DIColumns.Unit.UnitName;

            return RetVal.ToString();
        }

        /// <summary>
        /// Returns query to get units for the given indicatorNids
        /// </summary>
        /// <param name="indicatorNids"></param>
        /// <returns></returns>
        public string GetUnitByIndicator(string indicatorNids, string OrderByColumn)
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("Select Distinct U." + DIColumns.Unit.UnitName + ",U." + DIColumns.Unit.UnitNId);

            RetVal.Append(",U." + DIColumns.Unit.UnitGId + ",U." + DIColumns.Unit.UnitGlobal + " From " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.Unit + " U");

            RetVal.Append(" Where IUS." + DIColumns.Unit.UnitNId + "= U." + DIColumns.Unit.UnitNId);

            if (!string.IsNullOrEmpty(indicatorNids))
                RetVal.Append(" and IUS." + DIColumns.Indicator.IndicatorNId + " in(" + indicatorNids + ")");

            if (!string.IsNullOrEmpty(OrderByColumn))
                RetVal.Append(" ORDER BY " + OrderByColumn);

            return RetVal.ToString();
        }

        #endregion

        #region "-- Duplicate Records --"

        /// <summary>
        /// Get Duplicate Units By Name and GId
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateUnits()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Unit.UnitNId + "," + DIColumns.Unit.UnitName + "," + DIColumns.Unit.UnitGId + " FROM " + this.TablesName.Unit);
            sbQuery.Append(" WHERE EXISTS (  ");
            sbQuery.Append("SELECT " + DIColumns.Unit.UnitName + "," + DIColumns.Unit.UnitGId + " FROM " + this.TablesName.Unit);

            sbQuery.Append(" GROUP BY " + DIColumns.Unit.UnitName + "," + DIColumns.Unit.UnitGId);

            sbQuery.Append(" HAVING COUNT(*) >1 )");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Duplicate Units By GIds
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateUnitByGids()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Unit.UnitNId + "," + DIColumns.Unit.UnitName + "," + DIColumns.Unit.UnitGId + " FROM " + this.TablesName.Unit + " WHERE " + DIColumns.Unit.UnitGId + " IN(");

            sbQuery.Append(" SELECT " + DIColumns.Unit.UnitGId + " FROM ");

            sbQuery.Append(this.TablesName.Unit + " GROUP BY " + DIColumns.Unit.UnitGId);

            sbQuery.Append(" HAVING COUNT(*) >1 )");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Duplicate Unit By Name
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateUnitByNames()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Unit.UnitNId + "," + DIColumns.Unit.UnitName + "," + DIColumns.Unit.UnitGId + " FROM " + this.TablesName.Unit + " WHERE  " + DIColumns.Unit.UnitName + " IN( ");

            sbQuery.Append("SELECT " + DIColumns.Unit.UnitName + " FROM ");

            sbQuery.Append(this.TablesName.Unit + " GROUP BY " + DIColumns.Unit.UnitName);

            sbQuery.Append(" HAVING COUNT(*) >1 )");

            RetVal = sbQuery.ToString();
            return RetVal;
        }
        /// <summary>
        /// Get Duplicate Records from Unit By Nid
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateUnitByNid()
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.Unit.UnitNId + "," + DIColumns.Unit.UnitName + "," + DIColumns.Unit.UnitGId + " FROM " + this.TablesName.Unit + " WHERE " + DIColumns.Unit.UnitNId + " IN(");

            SqlQuery.Append(" SELECT " + DIColumns.Unit.UnitNId + " FROM ");

            SqlQuery.Append(this.TablesName.Unit + " GROUP BY " + DIColumns.Unit.UnitNId);

            SqlQuery.Append(" HAVING COUNT(*) >1 )");

            return SqlQuery.ToString();
        }

        public string GetInValidSdmxCompliantUnitGid()
        {
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT " + DIColumns.Unit.UnitGId + "," + DIColumns.Unit.UnitName + "," + DIColumns.Unit.UnitNId);
            SqlQuery.Append(" FROM " + TablesName.Unit);
            SqlQuery.Append(" WHERE " + DIColumns.Unit.UnitGId + " NOT REGEX '^[-0-9a-zA-Z@_\\$]+$'");

            return SqlQuery.ToString();
        }
        #endregion

        #endregion

        #endregion



    }
}
