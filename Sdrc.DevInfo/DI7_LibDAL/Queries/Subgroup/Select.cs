using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Subgroup
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
        /// Get  NId, Name, GID, Global from SubgroupVals table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId, Name, Search, NIdNotIn, NameNotIn</param>
        /// <param name="filterText">For FilterFieldType "Search" FilterText should be SubgroupVals.SubgroupVal LIKE '%Rural%' or SubgroupVals.SubgroupVal LIKE '%Total 0-14 yr%'</param>
        /// <returns></returns>
        public string GetSubgroupVals(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.SubgroupVals.SubgroupValNId + "," + DIColumns.SubgroupVals.SubgroupVal + "," + DIColumns.SubgroupVals.SubgroupValGId + "," + DIColumns.SubgroupVals.SubgroupValGlobal);

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
            RetVal = sbQuery.ToString();
            return RetVal;
        }

       
        
        /// <summary>
        /// Get  NId, Name, GID, Global, Type from Subgroup table for a given  Filter Criteria
        /// </summary>
        /// <param name="filterFieldType">Applicable for NId, Name, Type, Search, NIdNotIn, NameNotIn</param>
        /// <param name="filterText">For FilterFieldType "Search" FilterText should be Subgroup.SubgroupName LIKE '%Rural%' or SubgroupVals.SubgroupVal LIKE '%Total%'</param>
        /// <returns></returns>
        public string GetSubgroup(FilterFieldType filterFieldType, string filterText)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIColumns.Subgroup.SubgroupNId + "," + DIColumns.Subgroup.SubgroupName + "," + DIColumns.Subgroup.SubgroupGId + "," + DIColumns.Subgroup.SubgroupGlobal + "," + DIColumns.Subgroup.SubgroupType + "," + DIColumns.Subgroup.SubgroupOrder);

            sbQuery.Append(" FROM " + this.TablesName.Subgroup);

            if (!string.IsNullOrEmpty(filterText) && filterFieldType != FilterFieldType.None)
                sbQuery.Append(" WHERE ");

            if (!string.IsNullOrEmpty(filterText))
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DIColumns.Subgroup.SubgroupNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.ID:
                        break;
                    case FilterFieldType.GId:
                        sbQuery.Append(DIColumns.Subgroup.SubgroupGId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DIColumns.Subgroup.SubgroupName + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Type:
                        sbQuery.Append(DIColumns.Subgroup.SubgroupType + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(filterText);
                        break;
                    case FilterFieldType.Global:
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(DIColumns.Subgroup.SubgroupNId + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DIColumns.Subgroup.SubgroupName + " NOT IN (" + filterText + ")");
                        break;
                    default:
                        break;
                }
            }
            RetVal = sbQuery.ToString();
            return RetVal;
        }


        /// <summary>
        /// Get IndGId_SubgroupGIds based on IndicatorNId_SubgroupNIds
        /// </summary>
        /// <param name="IndicatorNId_SubgroupNIds">comma delimited IndicatorNId_SubgroupNIds</param>
        /// <param name="DIServerType">Database type</param>
        /// <returns></returns>
        internal string GetIndGId_SubgroupGIds(string IndicatorNId_SubgroupNIds, DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId, ",", Delimiter.TEXT_SEPARATOR, DIServerType));

            sbQuery.Append(" FROM " + this.TablesName.Indicator + " AS I," + this.TablesName.SubgroupVals + " AS SGV");

            sbQuery.Append(" WHERE (" + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorNId + ",SGV." + DIColumns.SubgroupVals.SubgroupValNId, ",", Delimiter.NUMERIC_SEPARATOR, DIServerType) + ") IN (" + IndicatorNId_SubgroupNIds + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        internal string GetIndicatorNId_SubgroupNIds(string IndGId_SubgroupGIds, DIServerType DIServerType)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();

            sbQuery.Append("SELECT " + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorNId + ",SGV." + DIColumns.SubgroupVals.SubgroupValNId, ",", Delimiter.NUMERIC_SEPARATOR, DIServerType));

            sbQuery.Append(" FROM " + this.TablesName.Indicator + " AS I," + this.TablesName.SubgroupVals + " AS SGV");

            sbQuery.Append(" WHERE (" + DIQueries.SQL_GetConcatenatedValues("I." + DIColumns.Indicator.IndicatorGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId, ",", Delimiter.TEXT_SEPARATOR, DIServerType) + ") IN (" + IndGId_SubgroupGIds + ")");

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get the Subgroup Vals on the basis of IUS
        /// </summary>
        /// <param name="iusNId"></param>
        /// <returns></returns>
        public string GetSubgroups(string iusNId)
        {
            string RetVal = string.Empty;
            StringBuilder sbQuery = new StringBuilder();
            sbQuery.Append("SELECT DISTINCT IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + ",SGV." + DIColumns.SubgroupVals.SubgroupValNId + ",SGV." + DIColumns.SubgroupVals.SubgroupVal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGlobal + ",SGV." + DIColumns.SubgroupVals.SubgroupValGId + ",SGV." + DIColumns.SubgroupVals.SubgroupValOrder);
            sbQuery.Append(" FROM " + TablesName.IndicatorUnitSubgroup + " IUS," + TablesName.SubgroupVals + " SGV");
            sbQuery.Append(" WHERE SGV." + DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);

            if (string.IsNullOrEmpty(iusNId))
            {
                iusNId = "-1";
            }
            sbQuery.Append(" AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + iusNId + ")");
            sbQuery.Append(" ORDER BY SGV." + DIColumns.SubgroupVals.SubgroupVal);
            RetVal = sbQuery.ToString();
            return RetVal;
        }


       /// <summary>
       /// Returns sql query to get subgroup information with subgrouptype and subgroup order
       /// </summary>
       /// <param name="subgroupNids">Comma separated subgroup nids which may be blank </param>
       /// <returns></returns>
        public string GetSubgroupInfoWithTypeNOrder(string subgroupNids)
        {
            string RetVal = string.Empty;

            RetVal = "SELECT SG." + DIColumns.Subgroup.SubgroupNId + ", SG." + DIColumns.Subgroup.SubgroupName + ", SG." + DIColumns.Subgroup.SubgroupGId + ", SG." + DIColumns.Subgroup.SubgroupGlobal + ", SGType." + DIColumns.SubgroupTypes.SubgroupTypeOrder + ", SGType." + DIColumns.SubgroupTypes.SubgroupTypeName + ", SGType." + DIColumns.SubgroupTypes.SubgroupTypeNId + ", SGType." + DIColumns.SubgroupTypes.SubgroupTypeGID + ",SG."+DIColumns.Subgroup.SubgroupOrder 
                + " FROM " + this.TablesName.SubgroupType + " AS SGType," + this.TablesName.Subgroup + " AS SG WHERE SGType." + DIColumns.SubgroupTypes.SubgroupTypeNId + "= SG." + DIColumns.Subgroup.SubgroupType + " ";

            if (!string.IsNullOrEmpty(subgroupNids))
            {
                RetVal += " AND SG." + DIColumns.Subgroup.SubgroupNId + " IN(" + subgroupNids + ") ";
            }

            RetVal  =RetVal+ " ORDER BY SGType." + DIColumns.SubgroupTypes.SubgroupTypeOrder +", SG."+ DIColumns.Subgroup.SubgroupName ;

            return RetVal;
        }


        /// <summary>
        /// Returns query to get SubgroupNid and SubgroupType on basis of SubgroupValNid
        /// </summary>
        /// <param name="subgroupValNid"></param>
        /// <returns></returns>
        public string GetSubgroupNIdNType(int subgroupValNid)
        {
            string RetVal = string.Empty;

            RetVal = "Select  S." + DIColumns.Subgroup.SubgroupType + " ,S." + DIColumns.Subgroup.SubgroupNId + "  from  " + this.TablesName.Subgroup +
                " S INNER JOIN " + this.TablesName.SubgroupValsSubgroup + " SVS   ON S." + DIColumns.Subgroup.SubgroupNId + "  = SVS."
                + DIColumns.SubgroupValsSubgroup.SubgroupNId + "  where SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " = " + subgroupValNid.ToString();

            return RetVal;
        }

        /// <summary>
        /// Retruns query to count records where same subgroup type is already associated with the subgroupVals
        /// </summary>
        /// <param name="subgroupNId"></param>
        /// <param name="newType">new subgroup type</param>
        /// <returns></returns>
        public string IsSGTypeAssociatedWOtherSubgroups(int subgroupNId, int newType )
        {
            string RetVal = string.Empty;

           RetVal=" SELECT COUNT(*) FROM "+  this.TablesName.SubgroupValsSubgroup  +" AS SVS, "+
               this.TablesName.Subgroup +" AS S  WHERE "+ 
               " EXISTS  ( Select * from "+  this.TablesName.SubgroupValsSubgroup  +" As SVS1 where SVS1."+
               DIColumns.SubgroupValsSubgroup.SubgroupNId + "="+  subgroupNId +"  AND SVS."+ 
               DIColumns.SubgroupValsSubgroup.SubgroupValNId +"=SVS1."+ DIColumns.SubgroupValsSubgroup.SubgroupValNId +") " +
               " AND s."+ DIColumns.Subgroup.SubgroupNId +"=SVS."+ DIColumns.SubgroupValsSubgroup.SubgroupNId +
               " AND s." + DIColumns.Subgroup.SubgroupNId  + "<>" + subgroupNId + " and s." + DIColumns.Subgroup.SubgroupType + "=" + newType;

         
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

            sbQuery.Append("SELECT S." + DIColumns.Subgroup.SubgroupNId + " AS " + DIColumns.Subgroup.SubgroupNId + ",S." + DIColumns.Subgroup.SubgroupName + " AS " + DIColumns.Subgroup.SubgroupName + ",S." + DIColumns.Subgroup.SubgroupGId + " AS " + DIColumns.Subgroup.SubgroupGId );
            sbQuery.Append(", S1." + DIColumns.Subgroup.SubgroupNId + ",S1." + DIColumns.Subgroup.SubgroupName + ",S1." + DIColumns.Subgroup.SubgroupGId + " FROM " + this.TablesName.Subgroup + " S," + table.Subgroup + " S1 ");

            sbQuery.Append(" WHERE ");
            sbQuery.Append(" S." + DIColumns.Subgroup.SubgroupNId + "= S1." + DIColumns.Subgroup.SubgroupNId);
            sbQuery.Append(" AND S." + DIColumns.Subgroup.SubgroupGId + "<> S1." + DIColumns.Subgroup.SubgroupGId );

            RetVal = sbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get the subgroupNIds on the Indicator, unit and IUSNIDs
        /// </summary>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <param name="iusNIds"></param>
        /// <returns></returns>
        public string GetSubgroupForIndicatorUnit(int indicatorNId, int unitNId, string iusNIds)
        {
            string RetVal = string.Empty; 
            try
            {
                RetVal = " SELECT SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + ",SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId;
                RetVal += ",IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId;

                RetVal += " FROM " + this.TablesName.IndicatorUnitSubgroup + " IUS," + this.TablesName.SubgroupValsSubgroup + " SVS ";
                RetVal += " WHERE SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId;

                RetVal += " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + " = " + indicatorNId + " AND IUS." + DIColumns.Indicator_Unit_Subgroup.UnitNId + " = " + unitNId;
                RetVal += " AND IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + iusNIds + ")";
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        ///  Returns sql query to get max subgroup order
        /// </summary>
        /// <returns></returns>
        public string GetMaxSubgroupOrder(int subgroupTypeNid)
        {
            string RetVal = string.Empty;

            RetVal = "Select max(" + DIColumns.Subgroup.SubgroupOrder + ") FROM " + this.TablesName.Subgroup + " WHERE " + DIColumns.Subgroup.SubgroupType +"=" + subgroupTypeNid;
 
            return RetVal;
        }
        /// <summary>
        /// Get Duplicate Records of SubGroup By Nid
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubGroupByNid()
        {
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT " + DIColumns.Subgroup.SubgroupNId + "," + DIColumns.Subgroup.SubgroupName + "," + DIColumns.Subgroup.SubgroupGId + "," + DIColumns.Subgroup.SubgroupType);

            SqlQuery.Append(" FROM " + this.TablesName.Subgroup);

            SqlQuery.Append(" WHERE " + DIColumns.Subgroup.SubgroupNId + " IN( ");

            SqlQuery.Append("SELECT " + DIColumns.Subgroup.SubgroupNId);

            SqlQuery.Append(" FROM " + this.TablesName.Subgroup + " GROUP BY ");

            SqlQuery.Append(DIColumns.Subgroup.SubgroupNId + " HAVING COUNT(*) >1 )");

            return SqlQuery.ToString();
        }
        public string GetInValidSdmxCompliantSubGroupGid()
        {
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT " + DIColumns.Subgroup.SubgroupGId + "," + DIColumns.Subgroup.SubgroupName + "," + DIColumns.Subgroup.SubgroupNId);
            SqlQuery.Append(" FROM " + TablesName.Subgroup);
            

            return SqlQuery.ToString();
        }

        #region "-- Duplicate Records --"

        /// <summary>
        /// Get Duplicate Subgroups By Name,Gids and Type
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubgroups()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            SbQuery.Append("SELECT " + DIColumns.Subgroup.SubgroupNId +","+ DIColumns.Subgroup.SubgroupName + "," + DIColumns.Subgroup.SubgroupGId + "," + DIColumns.Subgroup.SubgroupType );
            SbQuery.Append(" FROM " + this.TablesName.Subgroup);
            SbQuery.Append(" WHERE EXISTS (");

            SbQuery.Append("SELECT " + DIColumns.Subgroup.SubgroupName + "," + DIColumns.Subgroup.SubgroupGId + "," + DIColumns.Subgroup.SubgroupType );
            SbQuery.Append(" FROM " + this.TablesName.Subgroup  + " GROUP BY ");
            
            SbQuery.Append(DIColumns.Subgroup.SubgroupName + "," + DIColumns.Subgroup.SubgroupGId + "," + DIColumns.Subgroup.SubgroupType );
            
            SbQuery.Append(" HAVING COUNT(*) >1 )");

            RetVal = SbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Duplicate SubgroupName
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubgroupName()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();
                    

            SbQuery.Append("SELECT " + DIColumns.Subgroup.SubgroupNId + "," + DIColumns.Subgroup.SubgroupName + "," + DIColumns.Subgroup.SubgroupGId + "," + DIColumns.Subgroup.SubgroupType);

            SbQuery.Append(" FROM " + this.TablesName.Subgroup + " WHERE " + DIColumns.Subgroup.SubgroupName + " IN( ");

            SbQuery.Append("SELECT " + DIColumns.Subgroup.SubgroupName);

            SbQuery.Append(" FROM " + this.TablesName.Subgroup + " GROUP BY ");

            SbQuery.Append(DIColumns.Subgroup.SubgroupName + " HAVING COUNT(*) >1 )");

            RetVal = SbQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Duplicate SubgroupGid
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubgroupGid()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            SbQuery.Append("SELECT " + DIColumns.Subgroup.SubgroupNId + "," + DIColumns.Subgroup.SubgroupName + "," + DIColumns.Subgroup.SubgroupGId + "," + DIColumns.Subgroup.SubgroupType);

            SbQuery.Append(" FROM " + this.TablesName.Subgroup);

            SbQuery.Append(" WHERE " + DIColumns.Subgroup.SubgroupGId + " IN( ");
            
            SbQuery.Append("SELECT " + DIColumns.Subgroup.SubgroupGId );
            
            SbQuery.Append(" FROM " + this.TablesName.Subgroup + " GROUP BY ");
            
            SbQuery.Append(DIColumns.Subgroup.SubgroupGId + " HAVING COUNT(*) >1 )");

            RetVal = SbQuery.ToString();

            return RetVal;
        }


        #endregion

        #endregion

        #endregion

    }
}
