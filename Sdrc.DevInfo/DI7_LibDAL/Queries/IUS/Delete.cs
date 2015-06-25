using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibDAL.Queries.IUS
{
    /// <summary>
    /// Provides sql queries to delete records
    /// </summary>
    public  class Delete
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
        /// Returns query to delete records from Indicator_Unit_Subgroup table
        /// </summary>
        /// <param name="nids"></param>
        /// <returns></returns>
        public string DeleteIUS(string nids)
        {

            string RetVal = string.Empty;
            RetVal = "DELETE FROM " + this.TablesName.IndicatorUnitSubgroup + " Where " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN (" + nids + ")";

            return RetVal;
        }

        public static string DeleteIUS(string IUStableName, string indicatorNIds, string unitNIds, string subgroupValNIds, string IUSNIds, bool ForNotInClause)
        {
            string RetVal = string.Empty;


            string InClause = string.Empty;

            RetVal = "Delete from " + IUStableName + " WHERE 1 = 1 ";

            if (ForNotInClause)
            {
                InClause = " NOT IN ";
            }
            else
            {
                InClause = " IN ";
            }

            //- Filter IndicatorNids
            if (string.IsNullOrEmpty(indicatorNIds) == false)
            {
                RetVal += " AND " + DIColumns.Indicator.IndicatorNId + InClause + " (" + indicatorNIds + " )";
            }

            //- Filter Unitnid
            if (string.IsNullOrEmpty(unitNIds) == false)
            {
                RetVal += " AND " + DIColumns.Unit.UnitNId + InClause + " (" + unitNIds + " )";
            }

            //- Filter SubgroupValNIds
            if (string.IsNullOrEmpty(subgroupValNIds) == false)
            {
                RetVal += " AND " + DIColumns.SubgroupValsSubgroup.SubgroupValNId + InClause + " (" + subgroupValNIds + " )";
            }
            //-  Filter IUSNids
            if (string.IsNullOrEmpty(IUSNIds) == false)
            {
                RetVal += " AND " + DIColumns.Indicator_Unit_Subgroup.IUSNId + InClause + " (" + IUSNIds + " )";
            }

            return RetVal;
        }
        
        #endregion

        #endregion
    }
}
