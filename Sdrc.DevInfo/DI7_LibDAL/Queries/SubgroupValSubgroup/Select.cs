using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.SubgroupValSubgroup
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
        /// Get records from Subgroup_Vals_Subgroup table based on SubgroupValNId and or SubgroupNId
        /// </summary>
        /// <param name="SubgroupValNId">comma delimited SubgroupValNId which may be null or empty</param>
        /// <param name="SubgroupNId">comma delimited SubgroupNId which may be null or empty</param>
        /// <returns></returns>
        public string GetSubgroupValsSubgroup(string SubgroupValNId, string SubgroupNId)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT " + DIColumns.SubgroupValsSubgroup.SubgroupValSubgroupNId + "," + DIColumns.SubgroupValsSubgroup.SubgroupValNId + "," + DIColumns.SubgroupValsSubgroup.SubgroupNId;
            RetVal += " FROM " + this.TablesName.SubgroupValsSubgroup;
            RetVal += " WHERE 1 = 1 ";
            if (!string.IsNullOrEmpty(SubgroupValNId))
            {
                RetVal += " AND " + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " IN (" + SubgroupValNId + ")";
            }
            if (!string.IsNullOrEmpty(SubgroupNId))
            {
                RetVal += " AND " + DIColumns.SubgroupValsSubgroup.SubgroupNId + " IN (" + SubgroupNId + ")";
            }
            return RetVal;
        }

        public string GetSubgroupValsSubgroupWithType(string SubgroupValNId, string SubgroupNId)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT SGVS." + DIColumns.SubgroupValsSubgroup.SubgroupValSubgroupNId + ",SGVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + ",SGVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + ",SG." + DIColumns.Subgroup.SubgroupType + ",SG." + DIColumns.Subgroup.SubgroupName + ",SGV." + DIColumns.SubgroupVals.SubgroupVal;
            RetVal += " FROM " + this.TablesName.SubgroupValsSubgroup + " AS SGVS ," + this.TablesName.Subgroup + " AS SG," + this.TablesName.SubgroupVals + " AS SGV";
            RetVal += " WHERE SGVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + " = SG." + DIColumns.Subgroup.SubgroupNId;
            RetVal += " AND SGVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " = SGV." + DIColumns.SubgroupVals.SubgroupValNId;
            if (!string.IsNullOrEmpty(SubgroupValNId))
            {
                RetVal += " AND SGVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " IN (" + SubgroupValNId + ")";
            }
            if (!string.IsNullOrEmpty(SubgroupNId))
            {
                RetVal += " AND SGVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + " IN (" + SubgroupNId + ")";
            }
            return RetVal;
        }

        /// <summary>
        /// Where  Subgroup value exists in Subgroup_Val_Subgroup table but not in Subgroup_Vals table
        /// </summary>
        /// <returns></returns>
        public string GetUnmatchedSubgroupValSubgroupBySubgroupValNid()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            SbQuery.Append("SELECT DISTINCT(" + DIColumns.SubgroupValsSubgroup.SubgroupValNId + ") FROM " + this.TablesName.SubgroupValsSubgroup + " AS SVS");

            SbQuery.Append(" WHERE NOT EXISTS  (SELECT * FROM " + this.TablesName.SubgroupVals);

            SbQuery.Append(" SV WHERE SV." + DIColumns.SubgroupVals.SubgroupValNId + " = SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + ")");

            RetVal = SbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Unmatched SubgroupValSubgroup Based On SubgroupVALNID
        /// </summary>
        /// <returns></returns>
        public string GetUnmatchedSubgroupValSubgroupBySubgroupNid()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            SbQuery.Append("SELECT DISTINCT(" + DIColumns.SubgroupValsSubgroup.SubgroupNId + ")  FROM " + this.TablesName.SubgroupValsSubgroup + "  AS SVS");

            SbQuery.Append(" WHERE NOT EXISTS(SELECT * FROM " + this.TablesName.Subgroup + " AS S");

            SbQuery.Append(" WHERE S." + DIColumns.Subgroup.SubgroupNId + " = SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + ")");

            RetVal = SbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Duplicate SubgroupValSubgroup
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubgroupValSubgroup()
        {
            string RetVal = string.Empty;
            StringBuilder SbQuery = new StringBuilder();

            SbQuery.Append("SELECT SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValSubgroupNId + ",SGV." + DIColumns.SubgroupVals.SubgroupValNId + ", SGV." + DIColumns.SubgroupVals.SubgroupVal);

            SbQuery.Append(", S." + DIColumns.Subgroup.SubgroupNId + ", S." + DIColumns.Subgroup.SubgroupName);
            SbQuery.Append(" FROM " + this.TablesName.SubgroupValsSubgroup + "  AS SVS," + this.TablesName.SubgroupVals + " SGV," + this.TablesName.Subgroup + " S");

            SbQuery.Append(" WHERE ");
            SbQuery.Append("SGV." + DIColumns.SubgroupVals.SubgroupValNId + "  = SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId);
            SbQuery.Append(" AND S." + DIColumns.Subgroup.SubgroupNId + "  = SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId);
            SbQuery.Append(" AND SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " IN ( ");

            SbQuery.Append("SELECT SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId);
            SbQuery.Append(" FROM " + this.TablesName.SubgroupValsSubgroup + "  AS SVS");

            SbQuery.Append(" GROUP BY  SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + ",  SVS.");

            SbQuery.Append(DIColumns.SubgroupValsSubgroup.SubgroupNId + " HAVING COUNT(*)>1  )");

            RetVal = SbQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Get records from Subgroup_Vals_Subgroup table based on SubgroupValNId and or SubgroupNId
        /// </summary>
        /// <param name="SubgroupValNId">comma delimited SubgroupValNId which may be null or empty</param>
        /// <param name="SubgroupNId">comma delimited SubgroupNId which may be null or empty</param>
        /// <returns></returns>
        public string GetSubgroupValsSubgroup(string SubgroupValNId, string SubgroupNId, bool getDimensionInfo)
        {
            string RetVal = string.Empty;
            RetVal = "SELECT SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValSubgroupNId + ",SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + ",SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId;

            if (getDimensionInfo)
            {
                RetVal += ",IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupNids;
            }

            RetVal += " FROM " + this.TablesName.SubgroupValsSubgroup + " SVS";

            if (getDimensionInfo)
            {
                RetVal += "," + this.TablesName.IndicatorUnitSubgroup + " IUS";
            }

            if (getDimensionInfo)
            {
                RetVal += " WHERE IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " = SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId;
            }
            else
            {
                RetVal += " WHERE 1 = 1 ";
            }

            if (!string.IsNullOrEmpty(SubgroupValNId))
            {
                RetVal += " AND " + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " IN (" + SubgroupValNId + ")";
            }
            if (!string.IsNullOrEmpty(SubgroupNId))
            {
                RetVal += " AND " + DIColumns.SubgroupValsSubgroup.SubgroupNId + " IN (" + SubgroupNId + ")";
            }
            return RetVal;
        }

        /// <summary>
        /// Get SubgroupVal and with multiple Subgroups
        /// </summary>
        /// <returns></returns>
        public string GetSubgroupValsWithSubgroups()
        {
            string RetVal = string.Empty;
            StringBuilder Sb = new StringBuilder();

            Sb.Append("SELECT  SGV." + DIColumns.SubgroupVals.SubgroupVal + ",S." + DIColumns.Subgroup.SubgroupName + ",S." + DIColumns.Subgroup.SubgroupNId + ", ST." + DIColumns.SubgroupTypes.SubgroupTypeName + ", SVS." + DIColumns.SubgroupVals.SubgroupValNId + " , SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + ", ST." + DIColumns.SubgroupTypes.SubgroupTypeNId + ", SGV." + DIColumns.SubgroupVals.SubgroupValGId + ", S." + DIColumns.Subgroup.SubgroupGId + " , ST." + DIColumns.SubgroupTypes.SubgroupTypeGID);

            Sb.Append(" FROM " + this.TablesName.SubgroupValsSubgroup + " AS SVS, " + this.TablesName.SubgroupVals + " AS SGV, " + this.TablesName.Subgroup + " AS S," + this.TablesName.SubgroupType + " AS ST ");
            Sb.Append(" WHERE SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + "= SGV." + DIColumns.SubgroupVals.SubgroupValNId + " AND SVS." + DIColumns.SubgroupValsSubgroup.SubgroupNId + " = S." + DIColumns.Subgroup.SubgroupNId + " AND  S." + DIColumns.Subgroup.SubgroupType + "= ST." + DIColumns.SubgroupTypes.SubgroupTypeNId);


            RetVal = Sb.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get SubgroupVal for Multiple Subgroup
        /// </summary>
        /// <param name="subgroupNIdsForAge"></param>
        /// <returns></returns>
        public string GetSubgroupValNidsBySubgroup(string subgroupNIdsForAge)
        {
            string RetVal = string.Empty;
            StringBuilder Sb = new StringBuilder();

            Sb.Append("SELECT SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + ",Count( SVS." + DIColumns.Subgroup.SubgroupNId + ") As ValueCount");
            Sb.Append(" FROM " + this.TablesName.SubgroupValsSubgroup + " AS SVS");
            
            if (!string.IsNullOrEmpty(subgroupNIdsForAge))
            {
                Sb.Append(" WHERE SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + " IN(" + subgroupNIdsForAge + ")");
            }

            Sb.Append(" GROUP BY SVS." + DIColumns.SubgroupValsSubgroup.SubgroupValNId + "  Having(Count( SVS." + DIColumns.Subgroup.SubgroupNId + ")>1)");

            RetVal = Sb.ToString();
            return RetVal;
        }

        /// <summary>
        /// Get Duplicate Records of SubgroupValSubgroup By SubgroupValSubgroupNId
        /// </summary>
        /// <returns></returns>
        public string GetDuplicateSubgroupValSubgroupByNid()
        {
            StringBuilder SqlQuery = new StringBuilder();
            try
            {
                SqlQuery.Append("SELECT SVSG." + DIColumns.SubgroupValsSubgroup.SubgroupValSubgroupNId + ",SVSG." + DIColumns.SubgroupValsSubgroup.SubgroupNId + ",SVSG." + DIColumns.SubgroupValsSubgroup.SubgroupValNId);

                SqlQuery.Append(" FROM " + this.TablesName.SubgroupValsSubgroup + " SVSG");

                SqlQuery.Append(" WHERE EXISTS ( ");

                SqlQuery.Append("SELECT SVSG1." + DIColumns.SubgroupValsSubgroup.SubgroupValSubgroupNId);

                SqlQuery.Append(" FROM " + this.TablesName.SubgroupValsSubgroup + " SVSG1");

                SqlQuery.Append(" GROUP BY SVSG1." + DIColumns.SubgroupValsSubgroup.SubgroupValSubgroupNId + " HAVING COUNT(*)>1 )");
            }
            catch (Exception)
            {
                SqlQuery.Length = 0;
            }

            return SqlQuery.ToString();

        }
        #endregion

        #endregion

    }
}
