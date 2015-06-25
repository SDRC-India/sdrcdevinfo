using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.SubgroupTypes
{
    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        #region "-- Public --"

        #region "-- Methods --"
                
        /// <summary>
        /// Returns a query to create UT_Subgroup_Type_en table
        /// </summary>
        /// <param name="forOnlineDB"></param>
        /// <param name="tableName"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public static string CreateTable(bool forOnlineDB, string tableName, DIServerType serverType)
        {
            string RetVal = string.Empty;
            
            if (forOnlineDB)
            {             
                if (serverType == DIServerType.MySql)
                {
                    RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.SubgroupTypes.SubgroupTypeNId + " int(4) NOT NULL AUTO_INCREMENT ,PRIMARY KEY (" + DIColumns.SubgroupTypes.SubgroupTypeNId + ")," +
                        DIColumns.SubgroupTypes.SubgroupTypeName + " varchar(128), " +
                        DIColumns.SubgroupTypes.SubgroupTypeGID + " varchar(60), " +
                        DIColumns.SubgroupTypes.SubgroupTypeOrder + " int(4), " +
                        DIColumns.SubgroupTypes.SubgroupTypeGlobal + " TINYINT )";
                }
                else
                {
                    RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.SubgroupTypes.SubgroupTypeNId + " int Identity(1,1) primary key," +
                        DIColumns.SubgroupTypes.SubgroupTypeName + "  varchar(128),  " +
                        DIColumns.SubgroupTypes.SubgroupTypeGID + " varchar(60), " +
                        DIColumns.SubgroupTypes.SubgroupTypeOrder + " int , " +
                        DIColumns.SubgroupTypes.SubgroupTypeGlobal + " bit )";
                }
            }
            else
            {
                RetVal = "CREATE TABLE " + tableName + " (" + DIColumns.SubgroupTypes.SubgroupTypeNId + " counter primary key," +
                    DIColumns.SubgroupTypes.SubgroupTypeName + "  Text(128), " +
                    DIColumns.SubgroupTypes.SubgroupTypeGID + " Text(60), " +
                    DIColumns.SubgroupTypes.SubgroupTypeOrder + " number ," +
                    DIColumns.SubgroupTypes.SubgroupTypeGlobal + " bit )";
            }
            return RetVal;

        }

        
        /// <summary>
        /// Returns a query to insert new subgroup type into Subgroup_Type table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="subgroupTypeName"></param>
        /// <param name="subgroupTypeGID"></param>
        /// <param name="subgroupTypeOrder"></param>
        /// <param name="isGlobal"></param>
        /// <returns></returns>
        public static string InsertSubgroupType(string tableName, string subgroupTypeName,string subgroupTypeGID , int subgroupTypeOrder,bool isGlobal)
        {
            string RetVal = string.Empty;
            RetVal = "INSERT INTO " + tableName + "(" + DIColumns.SubgroupTypes.SubgroupTypeName +","+
                DIColumns.SubgroupTypes.SubgroupTypeGID +","+
                DIColumns.SubgroupTypes.SubgroupTypeOrder +","+
                DIColumns.SubgroupTypes.SubgroupTypeGlobal + ") VALUES ('" +
                DIQueries.RemoveQuotesForSqlQuery(subgroupTypeName) + "','" + DIQueries.RemoveQuotesForSqlQuery(subgroupTypeGID) + "'," + subgroupTypeOrder + "," + isGlobal + ")";
            return RetVal;
        }

        #endregion

        #endregion
    }
}
