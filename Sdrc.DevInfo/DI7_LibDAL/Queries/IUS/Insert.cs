using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.IUS
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "indicator_unit_subgroup";

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Insert record into UT_Indicator_Unit_Subgroup Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        ///<param name="indicatorNid">Indicator Nid</param>
        ///<param name="unitNid">Unit Nid</param>
        ///<param name="subgroupValNid">Subgroup Val Nid</param>
        ///<param name="maximumValue">Maximum Value</param>
        ///<param name="minimumValue">Minimum Value</param>
        public static string InsertIUS(string dataPrefix, int indicatorNid, int unitNid, int subgroupValNid, long maximumValue, long minimumValue)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + "(" + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ","
            + DIColumns.Indicator_Unit_Subgroup.UnitNId + "," + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ","
            + DIColumns.Indicator_Unit_Subgroup.MaxValue + "," + DIColumns.Indicator_Unit_Subgroup.MinValue
            + ") "
            + " VALUES(" + indicatorNid + "," + unitNid + "," + subgroupValNid + "," + maximumValue + "," + minimumValue + ")";

            return RetVal;
        }

        /// <summary>
        /// Insert record into UT_Indicator_Unit_Subgroup Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        ///<param name="indicatorNid">Indicator Nid</param>
        ///<param name="unitNid">Unit Nid</param>
        ///<param name="subgroupValNid">Subgroup Val Nid</param>
        ///<param name="maximumValue">Maximum Value</param>
        ///<param name="minimumValue">Minimum Value</param>
        /// <param name="isDefaultSubgroup">true/false</param>
        /// <returns></returns>
        public static string InsertIUS(string dataPrefix, int indicatorNid, int unitNid, int subgroupValNid, long maximumValue, long minimumValue, bool isDefaultSubgroup)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + "(" + DIColumns.Indicator_Unit_Subgroup.IndicatorNId + ","
            + DIColumns.Indicator_Unit_Subgroup.UnitNId + "," + DIColumns.Indicator_Unit_Subgroup.SubgroupValNId + ","
            + DIColumns.Indicator_Unit_Subgroup.MaxValue + "," + DIColumns.Indicator_Unit_Subgroup.MinValue + "," 
            + DIColumns.Indicator_Unit_Subgroup.IsDefaultSubgroup
            + ") "
            + " VALUES(" + indicatorNid + "," + unitNid + "," + subgroupValNid + "," + maximumValue + "," + minimumValue + "," + isDefaultSubgroup + ")";

            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert Data_Exist column into UT_Indicator_Unit_Subgroup table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDataExistColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.IndicatorUnitSubgroup + " ADD COLUMN  " + DIColumns.Indicator_Unit_Subgroup.DataExist + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " TinyInt(1) ";
                }
                else
                {
                    RetVal += " Bit ";
                }
            }
            else
            {
                RetVal += " Bit ";
            }

            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert Subgroup_Nids column into UT_Indicator_Unit_Subgroup table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertSubgroupNidsColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.IndicatorUnitSubgroup + " ADD COLUMN  " + DIColumns.Indicator_Unit_Subgroup.SubgroupNids + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(50) ";
                }
                else
                {
                    RetVal += " varchar(50) ";
                }
            }
            else
            {
                RetVal += " text(50) ";
            }

            return RetVal;
        }

        /// <summary>
        /// Returns qurey to insert Subgroup_Type_Nids column into UT_Indicator_Unit_Subgroup table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertSubgroupTypeNidsColumn(DITables tablesName, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + tablesName.IndicatorUnitSubgroup + " ADD COLUMN  " + DIColumns.Indicator_Unit_Subgroup.SubgroupTypeNids + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(50) ";
                }
                else
                {
                    RetVal += " varchar(50) ";
                }
            }
            else
            {
                RetVal += " text(50) ";
            }

            return RetVal;
        }

        #region "-- DI7 Changes --"

       

        /// <summary>
        ///  add column IsDefaultSubgroup
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7IsDefaultSubgroupColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).IndicatorUnitSubgroup + " ADD COLUMN  " + DIColumns.Indicator_Unit_Subgroup.IsDefaultSubgroup + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " TinyInt(1) ";
                }
                else
                {
                    RetVal += " Bit ";
                }
            }
            else
            {
                RetVal += " Bit ";
            }
            return RetVal;
        }
       
        /// <summary>
        /// add column AvlMinDataValue
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7AvlMinDataValueColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).IndicatorUnitSubgroup + " ADD COLUMN  " + DIColumns.Indicator_Unit_Subgroup.AvlMinDataValue + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " Double(18,5) ";
                }
                else
                {
                    RetVal += " Decimal(18,5) ";
                }
            }
            else
            {
                RetVal += " Double ";
            }
            return RetVal;
        }
       
        /// <summary>
        /// add column AvlMaxDataValue
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7AvlMaxDataValueColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).IndicatorUnitSubgroup + " ADD COLUMN  " + DIColumns.Indicator_Unit_Subgroup.AvlMaxDataValue + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " Double(18,5) ";
                }
                else
                {
                    RetVal += " Decimal(18,5) ";
                }
            }
            else
            {
                RetVal += " Double ";
            }
            return RetVal;
        }
        
        /// <summary>
        /// Add column AvlMinTimePeriod
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7AvlMinTimePeriodColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).IndicatorUnitSubgroup + " ADD COLUMN  " + DIColumns.Indicator_Unit_Subgroup.AvlMinTimePeriod + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(60) ";
                }
                else
                {
                    RetVal += " varchar(60) ";
                }
            }
            else
            {
                RetVal += " text(60) ";
            }
            return RetVal;
        }
        
        /// <summary>
        /// add column AvlMaxTimePeriod
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string InsertDI7AvlMaxTimePeriodColumn(string dataPrefix, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, String.Empty).IndicatorUnitSubgroup + " ADD COLUMN  " + DIColumns.Indicator_Unit_Subgroup.AvlMaxTimePeriod + " ";

            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " varchar(60) ";
                }
                else
                {
                    RetVal += " varchar(60) ";
                }
            }
            else
            {
                RetVal += " text(60) ";
            }
            return RetVal;
        }


        #endregion

        #endregion

        #endregion

    }
}
