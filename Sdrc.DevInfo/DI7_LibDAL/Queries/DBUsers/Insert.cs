using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;

namespace DevInfo.Lib.DI_LibDAL.Queries.DBUsers
{

   

    /// <summary>
    /// Provides sql queries to insert records
    /// </summary>
    public static class Insert
    {
        
        #region "-- public --"
         
        #region "-- Variables --"

       
        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Return query for Add User for Online database.
        /// </summary>
        /// <param name="userTable">Table should be User for MySql and [User] for Others</param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public static string AddUser(string userTable,string userName,string password ,int isAdmin) 
        {

            string RetVal=string.Empty;
            StringBuilder sSql = new StringBuilder();

            sSql.Append("INSERT INTO " + userTable + " (" + DIColumns.DBUser.UserName + "," + DIColumns.DBUser.UserPWD +"," + DIColumns.DBUser.ISAdmin + ")" );
            sSql.Append(" VALUES('" + userName + "','" + password  + "'," + isAdmin + ")");

            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;

            return RetVal;
        }



        /// <summary>
        /// Returns query to add record in UserAccess Table
        /// </summary>
        /// <param name="userNId"></param>
        /// <param name="accessTo"></param>
        /// <param name="dataPrefix"></param>
        /// <param name="tableNames"></param>
        /// <returns></returns>
        public static string AddUserAccess(int userNId, char accessTo, string dataPrefix, DITables tableNames)
        {
            string RetVal = null;
            StringBuilder sSql = new StringBuilder();

            sSql.Append("INSERT INTO " + tableNames.DBUserAccess + "(" + DIColumns.DBUserAccess.UserNId + ", " + DIColumns.DBUserAccess.AccessTo + ", " + DIColumns.DBUserAccess.DBPrefix + ")");
            sSql.Append(" VALUES(" + userNId + ",'" + accessTo + "','" + dataPrefix + "')");

            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;
            return RetVal;
        }
        public static string AddUserAccess(int userNId, string dataPrefix,int PermissionSource,int PermissionTimePeriod,int PermissionArea,int PermissionIC,int IsAdmin,string PermissionAreaDesc,string PermissionICDesc, DITables tableNames)
        {
            string RetVal = null;
            StringBuilder sSql = new StringBuilder();

            sSql.Append("INSERT INTO " + tableNames.DBUserAccess + "(" + DIColumns.DBUserAccess.UserNId + ", " + DIColumns.DBUserAccess.DBPrefix + ","+ DIColumns.DBUserAccess.PermissionSource+","+DIColumns.DBUserAccess.PermissionTimePeriod+","+DIColumns.DBUserAccess.PermissionArea+","+DIColumns.DBUserAccess.PermissionIC+","+DIColumns.DBUserAccess.IsAdmin+","+DIColumns.DBUserAccess.PermissionAreaDescription+","+DIColumns.DBUserAccess.PermissionICDescription+")");
            //sSql.Append(" VALUES(" + userNId + "," + dataPrefix +","+PermissionSource+","+PermissionTimePeriod+","+PermissionArea+","+PermissionIC+","+IsAdmin+","+PermissionAreaDesc+","+PermissionICDesc+ ")");
            sSql.Append(" VALUES(" + userNId + ",'" + dataPrefix + "'," + PermissionSource + ","
          + PermissionTimePeriod + "," + PermissionArea + "," + PermissionIC + ","
          + IsAdmin + ",'" + PermissionAreaDesc + "','" + PermissionICDesc + "')");
            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;
            return RetVal;
        }
        public static string AddDIUser(string userName, string password, string dataPrefix, int permissionsource, int permissiontimeperiod, int permissionarea, int permissionic, string associatedic, string associatedicwithparent, string associatedarea, string associatedareawithparent, int isAdmin, DITables tablenames)
        {
            string RetVal = null;
            StringBuilder sSql = new StringBuilder();
            sSql.Append("INSERT INTO " + tablenames.DIUser + "(" + DIColumns.DIUser.DBPrefix + "," + DIColumns.DIUser.UserName + "," + DIColumns.DIUser.UserPWD + "," + DIColumns.DIUser.PermissionSource + "," + DIColumns.DIUser.PermissionTimePeriod + "," + DIColumns.DIUser.PermissionArea + "," + DIColumns.DIUser.PermissionIC + "," + DIColumns.DIUser.AssocitedIC + "," + DIColumns.DIUser.AssocitedICWithParent + "," + DIColumns.DIUser.AssociatedArea + "," + DIColumns.DIUser.AssociatedAreaWithParent + "," + DIColumns.DIUser.IsAdmin + ")");
            sSql.Append(" VALUES('" + dataPrefix + "','" + DIQueries.RemoveQuotesForSqlQuery( userName) + "','" + DIQueries.RemoveQuotesForSqlQuery(password) + "'," + permissionsource + "," + permissiontimeperiod + "," + permissionarea + "," + permissionic + ",'" + associatedic + "','" + associatedicwithparent + "','" + associatedarea + "','" + associatedareawithparent + "'," + isAdmin + ")");
            RetVal = sSql.ToString();
            sSql = null;
            return RetVal;
        }

        /// <summary>
        /// Returns query to create DI_User table into sql server
        /// </summary>
        /// <returns></returns>
        public static string CreateTable()
        {
            string RetVal = null;
            StringBuilder SqlQuery = new StringBuilder();
            DITables Tables = new DITables(string.Empty, string.Empty);

            SqlQuery.AppendLine("CREATE TABLE " + Tables.DIUser +" (");
            SqlQuery.AppendLine(DIColumns.DIUser.UserNId + " Decimal(18,0) IDENTITY(1,1) NOT NULL ,");
            SqlQuery.AppendLine(DIColumns.DIUser.DBPrefix + " Char(9) ,");
            SqlQuery.AppendLine(DIColumns.DIUser.UserName + " Char(50) ,");
            SqlQuery.AppendLine(DIColumns.DIUser.UserPWD + " Char(50) ,");
            SqlQuery.AppendLine(DIColumns.DIUser.PermissionSource + " bit ,");
            SqlQuery.AppendLine(DIColumns.DIUser.PermissionTimePeriod + " bit ,");
            SqlQuery.AppendLine(DIColumns.DIUser.PermissionArea + " bit ,");
            SqlQuery.AppendLine(DIColumns.DIUser.PermissionIC + " bit ,");
            SqlQuery.AppendLine(DIColumns.DIUser.AssocitedIC + " varchar(MAX) ,");
            SqlQuery.AppendLine(DIColumns.DIUser.AssocitedICWithParent + " varchar(MAX) ,");
            SqlQuery.AppendLine(DIColumns.DIUser.AssociatedArea + " varchar(MAX),");
            SqlQuery.AppendLine(DIColumns.DIUser.AssociatedAreaWithParent + " varchar(MAX) ,");
            SqlQuery.AppendLine(DIColumns.DIUser.IsAdmin + " bit ,");
            SqlQuery.AppendLine(DIColumns.DIUser.ISloggedIn + " bit ,");
            SqlQuery.AppendLine(DIColumns.DIUser.LastLogin + " datetime ,");
            SqlQuery.AppendLine(DIColumns.DIUser.LastLogout + " datetime )");

            RetVal = SqlQuery.ToString();
            
            return RetVal;
        }

#endregion

        #endregion
    }
}