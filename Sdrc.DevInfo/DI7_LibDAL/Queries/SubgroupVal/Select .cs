using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.SubgroupVal
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
        /// Get all records from SubgroupVals table 
        /// </summary>      
        /// <remarks>This methods is used in Conversion process, so dont change it </remarks>
        /// <returns></returns>
        public string GetSubgroupVals()
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT * ");
            sbQuery.Append(" FROM " + this.TablesName.SubgroupVals);

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get  NId, Name, GID, Global from SubgroupVals table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId, Name, Search, NIdNotIn, NameNotIn</param>
        /// <param name="filterText">For FilterFieldType "Search" FilterText should be SubgroupVals.SubgroupVal LIKE '%Rural%' or SubgroupVals.SubgroupVal LIKE '%Total 0-14 yr%'</param>
        /// <returns></returns>
        public string GetSubgroupVals(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.SubgroupVals.SubgroupValNId + "," + DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValGId + "," + DIColumns.SubgroupVals.SubgroupValGlobal + "," + DIColumns.SubgroupVals.SubgroupValOrder);

            sbQuery.Append(" FROM " + this.TablesName.SubgroupVals);

            if (filterFieldType != FilterFieldType.None)
                sbQuery.Append(" WHERE ");

            if (filterText.Length > 0)
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DIColumns.SubgroupVals.SubgroupValNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.ID:
                        break;
                    case FilterFieldType.GId:
                        sbQuery.Append(DIColumns.SubgroupVals.SubgroupValGId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DIColumns.SubgroupVals.SubgroupVal + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(filterText);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(DIColumns.SubgroupVals.SubgroupValNId + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DIColumns.SubgroupVals.SubgroupVal + " NOT IN (" + filterText + ")");
                        break;
                    default:
                        break;
                }
            }

            sbQuery.Append(" Order by " + DIColumns.SubgroupVals.SubgroupValOrder + " ");
            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get  NId, Name, GID, Global from SubgroupVals by IndicatorNids and UnitNids
        /// </summary>
        /// <param name="IndicatorNids">comma seperated indicatorNids</param>
        /// <param name="UnitNids">comma seperated unitNids</param>
        /// <returns></returns>
        public string GetSubgroupValsByUnitNIndicatorNids(string IndicatorNids, string UnitNids)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT DISTINCT " + DIColumns.SubgroupVals.SubgroupValNId + "," + DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValGId + "," + DIColumns.SubgroupVals.SubgroupValGlobal + "," + DIColumns.SubgroupVals.SubgroupValOrder + " ");

            sbQuery.Append("FROM " + this.TablesName.SubgroupVals + " SG1 ");

            sbQuery.Append("WHERE EXISTS( ");

            sbQuery.Append("SELECT * ");

            sbQuery.Append("FROM " + this.TablesName.SubgroupVals + " SG2, " + this.TablesName.IndicatorUnitSubgroup + " IUS ");

            sbQuery.Append("WHERE ");

            sbQuery.Append("SG2." + DIColumns.SubgroupVals.SubgroupValNId + "=" + "IUS." + DIColumns.SubgroupVals.SubgroupValNId + " AND ");

            if (!string.IsNullOrEmpty(IndicatorNids))
            {
                sbQuery.Append(DIColumns.Indicator.IndicatorNId + " IN (" + IndicatorNids + ") AND ");
            }

            if (!string.IsNullOrEmpty(UnitNids))
            {
                sbQuery.Append(DIColumns.Unit.UnitNId + " IN (" + UnitNids + ") AND ");
            }

            sbQuery.Append(" SG1." + DIColumns.SubgroupVals.SubgroupValNId + "=" + "SG2." + DIColumns.SubgroupVals.SubgroupValNId + " ");

            sbQuery.Append(" ) ");

            sbQuery.Append("Order by " + DIColumns.SubgroupVals.SubgroupValOrder + " ");
            RetVal = sbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get  distinct Subgroup_Val_NId from SubgroupVals table for a given  subgroup type
        /// </summary>
        /// <param name="subgroupTypeNIds">Comma separated Subgroup_Type_NIds</param>
        /// <returns></returns>
        public string GetSubgroupValsNIdsByType(string subgroupTypeNIds)
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            SbQuery.Append("SELECT DISTINCT SVS." + DIColumns.SubgroupVals.SubgroupValNId);

            SbQuery.Append(" FROM " + this.TablesName.SubgroupValsSubgroup + " AS SVS WHERE EXISTS (");
            SbQuery.Append(" SELECT * FROM " + this.TablesName.Subgroup + " S WHERE SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + "=S." + DIColumns.Subgroup.SubgroupNId + " AND S." + DIColumns.Subgroup.SubgroupType + " IN(" + subgroupTypeNIds + ") )");


            RetVal = SbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        ///  Returns sql query to get max subgroup val order
        /// </summary>
        /// <returns></returns>
        public string GetMaxSubgroupValOrder()
        {
            string RetVal = string.Empty;

            RetVal = "Select max(" + DIColumns.SubgroupVals.SubgroupValOrder + ") FROM " + this.TablesName.SubgroupVals;

            return RetVal;
        }


        #region "-- Exchange --"

        /// <summary>
        /// Returns query to get subgroup vals for the given unit and indicator
        /// </summary>
        /// <param name="unitNId"></param>
        /// <param name="indicatorNid"></param>
        /// <returns></returns>
        public string GetSubgroupVal(int unitNId, int indicatorNid)
        {
            string RetVal = string.Empty;

            RetVal = "Select Distinct SG." + DIColumns.SubgroupVals.SubgroupValNId + ",SG." + DIColumns.SubgroupVals.SubgroupVal + ",SG." + DIColumns.SubgroupVals.SubgroupValGId + ",SG." + DIColumns.SubgroupVals.SubgroupValGlobal
                + " From " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.SubgroupVals + " SG"
                + " where IUS." + DIColumns.SubgroupVals.SubgroupValNId + " = SG." + DIColumns.SubgroupVals.SubgroupValNId
                + " and IUS." + DIColumns.Unit.UnitNId + "=" + unitNId
                + " and IUS." + DIColumns.Indicator.IndicatorNId + "=" + indicatorNid + " ORDER BY " + DIColumns.SubgroupVals.SubgroupVal;

            return RetVal;
        }

        /// <summary>
        /// Returns query to get subgroup vals for the given unitNids and indicatorNids
        /// </summary>
        /// <param name="unitNIds"></param>
        /// <param name="indicatorNids"></param>
        /// <returns></returns>
        public string GetSubgroupVal(string unitNIds, string indicatorNids)
        {
            StringBuilder RetVal = new StringBuilder();

            RetVal.Append("Select Distinct SG." + DIColumns.SubgroupVals.SubgroupValNId + ",SG." + DIColumns.SubgroupVals.SubgroupVal + ",SG." + DIColumns.SubgroupVals.SubgroupValGId + ",SG." + DIColumns.SubgroupVals.SubgroupValGlobal);

            RetVal.Append(" From " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.SubgroupVals + " SG");

            RetVal.Append(" where IUS." + DIColumns.SubgroupVals.SubgroupValNId + " = SG." + DIColumns.SubgroupVals.SubgroupValNId);

            if (!string.IsNullOrEmpty(unitNIds))
                RetVal.Append(" and IUS." + DIColumns.Unit.UnitNId + " in(" + unitNIds + ")");

            if (!string.IsNullOrEmpty(indicatorNids))
                RetVal.Append(" and IUS." + DIColumns.Indicator.IndicatorNId + " in(" + indicatorNids + ")");

            RetVal.Append(" ORDER BY " + DIColumns.SubgroupVals.SubgroupVal);


            //RetVal = "Select Distinct SG." + DIColumns.SubgroupVals.SubgroupValNId + ",SG." + DIColumns.SubgroupVals.SubgroupVal + ",SG." + DIColumns.SubgroupVals.SubgroupValGId + ",SG." + DIColumns.SubgroupVals.SubgroupValGlobal
            //    + " From " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.SubgroupVals + " SG"
            //    + " where IUS." + DIColumns.SubgroupVals.SubgroupValNId + " = SG." + DIColumns.SubgroupVals.SubgroupValNId
            //    + " and IUS." + DIColumns.Unit.UnitNId + "=" + unitNIds
            //    + " and IUS." + DIColumns.Indicator.IndicatorNId + "=" + indicatorNids + " ORDER BY " + DIColumns.SubgroupVals.SubgroupVal;

            return RetVal.ToString();
        }

        /// <summary>
        /// Retruns subgroup val nid where combination of subgroup nids is exactly same as given in the subgroup list
        /// </summary>
        /// <param name="subgroupNIds"></param>
        /// <returns></returns>
        public string GetSubgroupValNIdBySubgroups(List<string> subgroupNIds)
        {
            string RetVal = "Select " + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " FRom " + this.TablesName.SubgroupValsSubgroup + " Where  " + DIColumns.SubgroupValsSubgroup.SubgroupNId + " IN(" + string.Join(",", subgroupNIds.ToArray()) + " ) group by " + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " Having Count(*) = " + subgroupNIds.Count;

            return RetVal;
        }

        /// <summary>
        /// Returns query to get SUbgroupvals with Subgroup and subgroupType
        /// </summary>
        /// <param name="subgroupValNid"></param>
        /// <returns></returns>
        public string GetSubgroupsValsWithDimensionNDimValues()
        {
            string RetVal = string.Empty;

            RetVal = "Select  SGV." + DIColumns.SubgroupVals.SubgroupValNId + " ,SGV." + DIColumns.SubgroupVals.SubgroupVal + ",S." + DIColumns.Subgroup.SubgroupNId + ", S." + DIColumns.Subgroup.SubgroupType + ", S." + DIColumns.Subgroup.SubgroupName + ", SGT." + DIColumns.SubgroupTypes.SubgroupTypeName + ", S." + DIColumns.Subgroup.SubgroupOrder
                + "  FROM " + this.TablesName.Subgroup +
                " S, " + this.TablesName.SubgroupValsSubgroup + " SVS, " + this.TablesName.SubgroupVals + " SGV," + this.TablesName.SubgroupType + " SGT "
                + " WHERE S." + DIColumns.Subgroup.SubgroupNId + " = SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + " AND SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId
                + " AND SGT." + DIColumns.SubgroupTypes.SubgroupTypeNId + "=S." + DIColumns.Subgroup.SubgroupType;

            return RetVal;
        }

        #endregion

        #region "-- Duplicate Records --"

        /// <summary>
        /// Get Duplicate SubgroupVals By Gid and SubgroupVal
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubgroupVal()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();
            SbQuery.Append("SELECT " + DIColumns.SubgroupVals.SubgroupValNId + "," + DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValGId);
            SbQuery.Append(" FROM " + this.TablesName.SubgroupVals);
            SbQuery.Append(" WHERE EXISTS (");

            SbQuery.Append("SELECT " + DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValGId);
            SbQuery.Append(" FROM " + this.TablesName.SubgroupVals + " GROUP BY ");
            SbQuery.Append(DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValGId + " HAVING COUNT(*) >1 )");

            RetVal = SbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Duplicate SubgroupVal
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubgroupValByName()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            SbQuery.Append("SELECT " + DIColumns.SubgroupVals.SubgroupValNId + "," + DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValGId);
            SbQuery.Append(" FROM " + this.TablesName.SubgroupVals + " WHERE " + DIColumns.SubgroupVals.SubgroupVal + " IN(");

            SbQuery.Append("SELECT " + DIColumns.SubgroupVals.SubgroupVal);

            SbQuery.Append(" FROM " + this.TablesName.SubgroupVals + " GROUP BY ");

            SbQuery.Append(DIColumns.SubgroupVals.SubgroupVal + " HAVING COUNT(*) >1 )");

            RetVal = SbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Duplicate SubgroupValGid
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubgroupValByGId()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            SbQuery.Append("SELECT " + DIColumns.SubgroupVals.SubgroupValNId + "," + DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValGId);

            SbQuery.Append(" FROM " + this.TablesName.SubgroupVals + " WHERE " + DIColumns.SubgroupVals.SubgroupValGId + " IN(");

            SbQuery.Append("SELECT " + DIColumns.SubgroupVals.SubgroupValGId);

            SbQuery.Append(" FROM " + this.TablesName.SubgroupVals + " GROUP BY ");

            SbQuery.Append(DIColumns.SubgroupVals.SubgroupValGId + " HAVING COUNT(*) >1 )");

            RetVal = SbQuery.ToString();
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

            sbQuery.Append("SELECT S." + DIColumns.SubgroupVals.SubgroupValNId + " AS " + DIColumns.SubgroupVals.SubgroupValNId + ",S." + DIColumns.SubgroupVals.SubgroupVal + " AS " + DIColumns.SubgroupVals.SubgroupVal + ",S." + DIColumns.SubgroupVals.SubgroupValGId + " AS " + DIColumns.SubgroupVals.SubgroupValGId);
            sbQuery.Append(",S1." + DIColumns.SubgroupVals.SubgroupValNId + ",S1." + DIColumns.SubgroupVals.SubgroupVal + ",S1." + DIColumns.SubgroupVals.SubgroupValGId + " FROM " + this.TablesName.SubgroupVals + " S," + table.SubgroupVals + " S1 ");

            sbQuery.Append(" WHERE ");
            sbQuery.Append(" S." + DIColumns.SubgroupVals.SubgroupValNId + "= S1." + DIColumns.SubgroupVals.SubgroupValNId);
            sbQuery.Append(" AND S." + DIColumns.SubgroupVals.SubgroupValGId + "<> S1." + DIColumns.SubgroupVals.SubgroupValGId);

            RetVal = sbQuery.ToString();
            return RetVal;
        }
        /// <summary>
        /// Get Duplicate Records of SubgroupVals By Nids
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubGroupValByNid()
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.SubgroupVals.SubgroupValNId + "," + DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValGId);

            SqlQuery.Append(" FROM " + this.TablesName.SubgroupVals + " WHERE " + DIColumns.SubgroupVals.SubgroupValNId + " IN(");

            SqlQuery.Append("SELECT " + DIColumns.SubgroupVals.SubgroupValNId);

            SqlQuery.Append(" FROM " + this.TablesName.SubgroupVals + " GROUP BY ");

            SqlQuery.Append(DIColumns.SubgroupVals.SubgroupValNId + " HAVING COUNT(*) >1 )");

            return SqlQuery.ToString();
        }

        #endregion

        public string GetInValidSdmxCompliantSubGroupValGid()
        {
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT " + DIColumns.SubgroupVals.SubgroupValGId + "," + DIColumns.SubgroupVals.SubgroupValOrder + "," + DIColumns.SubgroupVals.SubgroupValNId);
            SqlQuery.Append(" FROM " + TablesName.SubgroupVals);
            

            return SqlQuery.ToString();
        }
        #endregion

        #endregion

    }
}
