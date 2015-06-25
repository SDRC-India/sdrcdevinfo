using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.IUS
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public class Update
    {
        #region "-- Private --"

        #region "-- Variables --"

        private DITables TablesName;

        #endregion

        #endregion

        #region "-- public --"

        #region "-- New / Dispose --"

        public Update(DITables tablesName)
        {
            this.TablesName = tablesName;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns query to update max and min value in IUS table
        /// </summary>
        /// <param name="IUSNid"></param>
        /// <param name="maxVal"></param>
        /// <param name="minVal"></param>
        /// <returns></returns>
        public string UpdateMaxMinValues(int IUSNid, string maxVal, string minVal)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + this.TablesName.IndicatorUnitSubgroup + " Set ";


            if (!string.IsNullOrEmpty(maxVal))
            {
                RetVal += " Max_Value =" + maxVal;
            }
            else
            {
                RetVal += " Max_Value =null";
            }

            if (!string.IsNullOrEmpty(minVal))
            {
                RetVal += "  ,Min_Value= " + minVal;
            }
            else
            {
                RetVal += "  ,Min_Value= null";
            }

            RetVal += " where IUSNID=" + IUSNid;



            return RetVal;
        }


        /// <summary>
        /// Returns query to update complete IUS information in IUS table
        /// </summary>
        /// <param name="IUSNid"></param>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <param name="subgroupValNId"></param>
        /// <param name="maxVal"></param>
        /// <param name="minVal"></param>
        /// <returns></returns>
        public string UpdateIUS(int IUSNid, int indicatorNId, int unitNId, int subgroupValNId, string maxVal, string minVal)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + this.TablesName.IndicatorUnitSubgroup + " Set ";

            RetVal += DIColumns.Indicator.IndicatorNId + "  =" + indicatorNId + " ,";
            RetVal += DIColumns.Unit.UnitNId + "  =" + unitNId + " ,";
            RetVal += DIColumns.SubgroupVals.SubgroupValNId + "  =" + subgroupValNId + " ,";

            if (!string.IsNullOrEmpty(maxVal))
            {
                RetVal += DIColumns.Indicator_Unit_Subgroup.MaxValue + "=" + maxVal;
            }
            else
            {
                RetVal += DIColumns.Indicator_Unit_Subgroup.MaxValue + "=0";
            }

            if (!string.IsNullOrEmpty(minVal))
            {
                RetVal += "  ," + DIColumns.Indicator_Unit_Subgroup.MinValue + "= " + minVal;
            }
            else
            {
                RetVal += "  ," + DIColumns.Indicator_Unit_Subgroup.MinValue + "= 0";
            }

            RetVal += " WHERE " + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=" + IUSNid;



            return RetVal;
        }

/// <summary>
        /// Returns query to update complete IUS information in IUS table
        /// </summary>
        /// <param name="IUSNid"></param>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <param name="subgroupValNId"></param>
        /// <param name="maxVal"></param>
        /// <param name="minVal"></param>
        /// <returns></returns>
        public string UpdateIUSWithDefaultSubgroup(int IUSNid, int indicatorNId, int unitNId, int subgroupValNId, string maxVal, string minVal, bool isDefaultSubgroup)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + this.TablesName.IndicatorUnitSubgroup + " Set ";

            RetVal += DIColumns.Indicator.IndicatorNId + "  =" + indicatorNId + " ,";
            RetVal += DIColumns.Unit.UnitNId + "  =" + unitNId + " ,";
            RetVal += DIColumns.SubgroupVals.SubgroupValNId + "  =" + subgroupValNId + " ,";

            if (!string.IsNullOrEmpty(maxVal))
            {
                RetVal += DIColumns.Indicator_Unit_Subgroup.MaxValue + "=" + maxVal;
            }
            else
            {
                RetVal += DIColumns.Indicator_Unit_Subgroup.MaxValue + "=0";
            }

            if (!string.IsNullOrEmpty(minVal))
            {
                RetVal += "  ," + DIColumns.Indicator_Unit_Subgroup.MinValue + "= " + minVal;
            }
            else
            {
                RetVal += "  ," + DIColumns.Indicator_Unit_Subgroup.MinValue + "= 0";
            }

            RetVal += "  ," + DIColumns.Indicator_Unit_Subgroup.IsDefaultSubgroup + "= " + isDefaultSubgroup;

            RetVal += " WHERE " + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=" + IUSNid;

            return RetVal;
        }

        /// <summary>
        /// Returns query to update complete IUS information in IUS table 
        /// </summary>
        /// <param name="IUSNid"></param>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <param name="subgroupValNId"></param>
        /// <param name="maxVal"></param>
        /// <param name="minVal"></param>
        /// <param name="dataExist"></param>
        /// <returns></returns>
        public string UpdateIUS(int IUSNid, int indicatorNId, int unitNId, int subgroupValNId, string maxVal, string minVal, bool dataExist)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + this.TablesName.IndicatorUnitSubgroup + " Set ";

            RetVal += DIColumns.Indicator.IndicatorNId + "  =" + indicatorNId + " ,";
            RetVal += DIColumns.Unit.UnitNId + "  =" + unitNId + " ,";
            RetVal += DIColumns.SubgroupVals.SubgroupValNId + "  =" + subgroupValNId + " ,";

            if (!string.IsNullOrEmpty(maxVal))
            {
                RetVal += DIColumns.Indicator_Unit_Subgroup.MaxValue + "=" + maxVal;
            }
            else
            {
                RetVal += DIColumns.Indicator_Unit_Subgroup.MaxValue + "=0";
            }

            if (!string.IsNullOrEmpty(minVal))
            {
                RetVal += "  ," + DIColumns.Indicator_Unit_Subgroup.MinValue + "= " + minVal;
            }
            else
            {
                RetVal += "  ," + DIColumns.Indicator_Unit_Subgroup.MinValue + "= 0";
            }

            //-- Set 1 if Data_exist TRUE else set 0
            RetVal += " ," + DIColumns.Indicator_Unit_Subgroup.DataExist + "=" + Convert.ToString(dataExist ? 1 : 0);

            RetVal += " WHERE " + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=" + IUSNid;



            return RetVal;
        }

        /// <summary>
        /// Returns query to update unit NId on the basis of indicatro NId
        /// </summary>
        /// <param name="indicatorNId"></param>
        /// <param name="unitNId"></param>
        /// <returns></returns>
        public string UpdateUnitNId(int indicatorNId, int unitNId)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + this.TablesName.IndicatorUnitSubgroup + " Set ";

            RetVal += DIColumns.Unit.UnitNId + "  =" + unitNId;

            RetVal += " where " + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + "  =" + indicatorNId;



            return RetVal;
        }

        /// <summary>
        ///  Returns query to update DataExist
        /// </summary>
        /// <param name="IUSNIds"></param>
        /// <param name="dataExist"></param>
        /// <returns></returns>
        public string UpdateIUSDataExist(string IUSNIds, bool dataExist)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + this.TablesName.IndicatorUnitSubgroup + " SET " + DIColumns.Indicator_Unit_Subgroup.DataExist + "=" + Convert.ToString(dataExist ? 1 : 0);

            if (!string.IsNullOrEmpty(IUSNIds))
            {
                RetVal += " WHERE " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN( " + IUSNIds + ")";
            }

            return RetVal;
        }

        #region "-- DataExists  --"

        /// <summary>
        /// Returns sql query to update dataexists value to false for all records.
        /// </summary>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public string UpdateDataExistsToFalse(DIServerType serverType)
        {
            string RetVal = string.Empty;

            switch (serverType)
            {
                case DIServerType.SqlServerExpress:
                case DIServerType.MySql:
                case DIServerType.SqlServer:
                case DIServerType.MsAccess:
                    RetVal = "UPDATE " + this.TablesName.IndicatorUnitSubgroup + " AS IUS SET IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist + "=0";
                    break;
                case DIServerType.Oracle:
                    break;
                case DIServerType.Excel:
                    break;
                default:
                    break;
            }

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to update dataexists values to true only if data exists 
        /// </summary>
        /// <returns></returns>
        public string UpdateDataExistsValues()
        {
            string RetVal = string.Empty;

            RetVal = " UPDATE " + this.TablesName.IndicatorUnitSubgroup + " AS IUS SET IUS." + DIColumns.Indicator_Unit_Subgroup.DataExist + "=True where EXists ( select * from " + this.TablesName.Data + " D where IUS." + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=D." + DIColumns.Data.IUSNId + ") ";

            return RetVal;
        }

        /// <summary>
        /// Returns sql query to update subgroup nids for the given IUSNID
        /// </summary>
        /// <param name="IUSNid"></param>
        /// <param name="subgroupNIds"></param>
        /// <returns></returns>
        public string UpdateSubgroupNids(string IUSNid, string subgroupNIds)
        {
            string RetVal = string.Empty;

            RetVal = "Update " + this.TablesName.IndicatorUnitSubgroup + " SET " + DIColumns.Indicator_Unit_Subgroup.SubgroupNids + "='" + subgroupNIds + "' WHERE " + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=" + IUSNid;

            return RetVal;
        }

        #endregion


        /// <summary>
        /// Update AvlMinDataValue
        /// </summary>
        /// <returns></returns>
        public string UpdateMinMaxDataAndTimeperiodValue(int IUSNId, string minDataValue, string maxDataValue, string minTimeperiod, string maxTimepriod)
        {
            string RetVal = string.Empty;
            StringBuilder Sb = new StringBuilder();

            Sb.Append("UPDATE " + this.TablesName.IndicatorUnitSubgroup);

            Sb.Append(" SET " + DIColumns.Indicator_Unit_Subgroup.AvlMinDataValue + " =" + minDataValue);
            Sb.Append("," + DIColumns.Indicator_Unit_Subgroup.AvlMaxDataValue + " =" + maxDataValue);
            Sb.Append("," + DIColumns.Indicator_Unit_Subgroup.AvlMinTimePeriod + " ='" + minTimeperiod + "'");
            Sb.Append("," + DIColumns.Indicator_Unit_Subgroup.AvlMaxTimePeriod + " ='" + maxTimepriod + "'");

            Sb.Append(" WHERE " + DIColumns.Indicator_Unit_Subgroup.IUSNId + "=" + IUSNId);


            RetVal = Sb.ToString();
            return RetVal;
        }

        /// <summary>
        /// Update ISDefautSubgroup
        /// </summary>
        /// <param name="IUSNIds"></param>
        /// <param name="isDefaultSubgroup"></param>
        /// <returns></returns>
        public string UpdateIUSISDefaultSubgroup(string IUSNIds, bool isDefaultSubgroup)
        {
            string RetVal = string.Empty;

            RetVal = "UPDATE " + this.TablesName.IndicatorUnitSubgroup + " SET " + DIColumns.Indicator_Unit_Subgroup.IsDefaultSubgroup + "=" + Convert.ToString(isDefaultSubgroup ? 1 : 0);

            if (!string.IsNullOrEmpty(IUSNIds))
            {
                RetVal += " WHERE " + DIColumns.Indicator_Unit_Subgroup.IUSNId + " IN( " + IUSNIds + ")";
            }

            return RetVal;
        }

        /// <summary>
        /// Set ISDefaultSubgroup=True where SubgroupVal=Total
        /// </summary>
        /// <param name="isDefaultSubgroup">pass True</param>
        /// <param name="TotalSubgroupVal">pass 'Total'</param>
        /// <returns></returns>
        public string UpdateISDefaultSubgroup(bool isDefaultSubgroup, string TotalSubgroupVal)
        {
            string RetVal = string.Empty;
            StringBuilder Sb = new StringBuilder();

            Sb.Append("UPDATE " + this.TablesName.IndicatorUnitSubgroup + " SET ");

            Sb.Append(DIColumns.Indicator_Unit_Subgroup.IsDefaultSubgroup + " = " + Convert.ToString(isDefaultSubgroup ? 1 : 0));

            Sb.Append(" WHERE " + DIColumns.SubgroupVals.SubgroupValNId);

            Sb.Append(" IN ( SELECT IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + " FROM ");

            Sb.Append(this.TablesName.SubgroupVals + " AS S INNER JOIN " + this.TablesName.IndicatorUnitSubgroup + " AS IUS ON S.");

            Sb.Append(DIColumns.SubgroupVals.SubgroupValNId + " = IUS." + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId);

            Sb.Append(" WHERE S." + DIColumns.SubgroupVals.SubgroupVal + "=" + TotalSubgroupVal + ")");

            RetVal = Sb.ToString();

            return RetVal;
        }

        #endregion

        #endregion

    }
}
