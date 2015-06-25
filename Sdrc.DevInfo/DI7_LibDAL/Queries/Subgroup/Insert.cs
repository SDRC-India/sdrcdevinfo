using System;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.Subgroup
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Private --"

        #region "-- Variables --"

        private const string TableName = "subgroup";
        private const string SubgroupValTableName = "subgroup_vals";

        #endregion

        #endregion

        #region "-- public --"

        #region "-- Methods --"

        /// <summary>
        /// Returns qurey to insert SubgroupOrder column into Subgroup table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <param name="forOnlineDB"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string AddOrderColumn(string dataPrefix,string languageCode, bool forOnlineDB, DIServerType serverType)
        {
            string RetVal = string.Empty;
            RetVal = "ALTER TABLE " + new DITables(dataPrefix, languageCode).Subgroup + " ADD COLUMN  [" + DIColumns.Subgroup.SubgroupOrder + "] ";
            if (forOnlineDB)
            {
                if (serverType == DIServerType.MySql)
                {
                    RetVal += " int(4) ";
                }
                else
                {
                    RetVal += " int ";
                }
            }
            else
            {
                RetVal += " Long ";
            }

            return RetVal;
        }

        /// <summary>
        /// insert Subgroup Into Subgroup Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="subgroupName">Subgroup Name</param>
        /// <param name="subgroupGID">Subgroup Gid</param>
        /// <param name="isSubgroupGlobal">true/false .True if it is global </param>
        /// <param name="subgroupType">Type of subgroup like age ,sex ...</param>
        /// <returns></returns>
        public static string InsertSubgroup(string dataPrefix, string languageCode, string subgroupName, string subgroupGID, bool isSubgroupGlobal, SubgroupType subgroupType)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + " (" + DIColumns.Subgroup.SubgroupName + ","
           + DIColumns.Subgroup.SubgroupGId + ","
           + DIColumns.Subgroup.SubgroupGlobal + ","
           + DIColumns.Subgroup.SubgroupType + ")"
           + " values('" + DIQueries.RemoveQuotesForSqlQuery(subgroupName) + "','" + DIQueries.RemoveQuotesForSqlQuery(subgroupGID) + "'," + isSubgroupGlobal + "," + (int)subgroupType + ")";

            return RetVal;
        }


        /// <summary>
        /// insert Subgroup Into Subgroup Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="subgroupName">Subgroup Name</param>
        /// <param name="subgroupGID">Subgroup Gid</param>
        /// <param name="isSubgroupGlobal">true/false .True if it is global </param>
        /// <param name="subgroupType">Type of subgroup like age ,sex ...</param>
        /// <returns></returns>
        public static string InsertSubgroup(string dataPrefix, string languageCode, string subgroupName, string subgroupGID, bool isSubgroupGlobal, int subgroupType)
        {
            return InsertSubgroup(dataPrefix, languageCode, subgroupName, subgroupGID, isSubgroupGlobal, subgroupType, DIServerType.MsAccess);
        }

        /// <summary>
        /// insert Subgroup Into Subgroup Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="subgroupName">Subgroup Name</param>
        /// <param name="subgroupGID">Subgroup Gid</param>
        /// <param name="isSubgroupGlobal">true/false .True if it is global </param>
        /// <param name="subgroupType">Type of subgroup like age ,sex ...</param>
        /// <returns></returns>
        public static string InsertSubgroup(string dataPrefix, string languageCode, string subgroupName, string subgroupGID, bool isSubgroupGlobal, int subgroupType, DIServerType serverType)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + " (" + DIColumns.Subgroup.SubgroupName + ","
           + DIColumns.Subgroup.SubgroupGId + ","
           + DIColumns.Subgroup.SubgroupGlobal + ","
           + DIColumns.Subgroup.SubgroupType + ")"
           + " values('" + DIQueries.RemoveQuotesForSqlQuery(subgroupName) + "','" + DIQueries.RemoveQuotesForSqlQuery(subgroupGID) + "',";

            switch (serverType)
            {
                case DIServerType.SqlServer:
                case DIServerType.MySql:
                    if (isSubgroupGlobal)
                    {
                        RetVal += " 1 ";
                    }
                    else
                    {
                        RetVal += " 0 ";
                    }
                    break;

                case DIServerType.MsAccess:
                    RetVal += isSubgroupGlobal;
                    break;
                case DIServerType.Oracle:
                    break;
               
                case DIServerType.SqlServerExpress:
                    break;
                case DIServerType.Excel:
                    break;
                default:
                    break;
            }

            RetVal += "," + subgroupType + ")";

            


            return RetVal;
        }

        /// <summary>
        /// insert Subgroup Into Subgroup Table
        /// </summary>
        /// <param name="dataPrefix">DataPrefix like UT_</param>
        /// <param name="languageCode">language code like _en</param>
        /// <param name="subgroupName">Subgroup Name</param>
        /// <param name="subgroupGID">Subgroup Gid</param>
        /// <param name="isSubgroupGlobal">true/false .True if it is global </param>
        /// <param name="subgroupType">Type of subgroup like age ,sex ...</param>
        /// <param name="serverType"></param>
        /// <param name="subgroupOrder"></param> 
        /// <returns></returns>
        public static string InsertSubgroup(string dataPrefix, string languageCode, string subgroupName, string subgroupGID, bool isSubgroupGlobal, int subgroupType, DIServerType serverType,int subgroupOrder)
        {
            string RetVal = string.Empty;

            RetVal = "INSERT INTO " + dataPrefix + Insert.TableName + languageCode + " (" + DIColumns.Subgroup.SubgroupName + ","
           + DIColumns.Subgroup.SubgroupGId + ","
           + DIColumns.Subgroup.SubgroupGlobal + ","
           + DIColumns.Subgroup.SubgroupOrder + ","
           + DIColumns.Subgroup.SubgroupType + ")"
           + " values('" + DIQueries.RemoveQuotesForSqlQuery(subgroupName) + "','" + DIQueries.RemoveQuotesForSqlQuery(subgroupGID) + "'," ;

            switch (serverType)
            {
                case DIServerType.SqlServer:
                case DIServerType.MySql:
                    if (isSubgroupGlobal)
                    {
                        RetVal += " 1 ";
                    }
                    else
                    {
                        RetVal += " 0 ";
                    }
                    break;

                case DIServerType.MsAccess:
                    RetVal += isSubgroupGlobal;
                    break;
                case DIServerType.Oracle:
                    break;

                case DIServerType.SqlServerExpress:
                    break;
                case DIServerType.Excel:
                    break;
                default:
                    break;
            }

            RetVal += "," + subgroupOrder + "," + subgroupType + ")";




            return RetVal;
        }
        

        /////// <summary>
        /////// Insert subgroupVal record into subgroupVal Table
        /////// </summary>
        /////// <param name="dataPrefix">Data prefix like UT_</param>
        /////// <param name="languageCode">Language code like _en </param>
        /////// <param name="subgroupVal">Subgroup val name</param>
        /////// <param name="subgroupValGid">SubgroupVal GId </param>
        /////// <param name="isGlobal">Ture/False. True if subgroupval is global otherwise false</param>
        /////// <param name="subgroupValAge">Nid of Age subgroup</param>
        /////// <param name="subgroupValSex">subgroupVal sex</param>
        /////// <param name="location">subgroupVal Location</param>
        /////// <param name="subgothers">subgroupVal Others</param>
        /////// <returns></returns>        
        ////public static string InsertSubgroupVal(string dataPrefix, string languageCode, string subgroupVal, string subgroupValGid, bool isGlobal, int subgroupValAge, int subgroupValSex, int location, int subgothers)
        ////{
        ////    string RetVal = string.Empty;

        ////    RetVal = "INSERT INTO " + dataPrefix + Insert.SubgroupValTableName + languageCode + "(" + DIColumns.SubgroupVals.SubgroupVal + ","
        ////    + DIColumns.SubgroupVals.SubgroupValGId + ","
        ////    + DIColumns.SubgroupVals.SubgroupValGlobal + ","
        ////    + DIColumns.SubgroupVals.SubgroupValAge + ","
        ////    + DIColumns.SubgroupVals.SubgroupValSex + ","
        ////    + DIColumns.SubgroupVals.SubgroupValLocation + ","
        ////    + DIColumns.SubgroupVals.SubgroupValOthers + ")"
        ////    + " VALUES('" + subgroupVal + "','" + subgroupValGid + "',"
        ////    + isGlobal + "," + subgroupValAge + "," + subgroupValSex + ","
        ////    + location + "," + subgothers + " ) ";


        ////    return RetVal;
        ////}
        
        #endregion

        #endregion
    }
}
