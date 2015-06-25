using System;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Connection;
using Microsoft.VisualBasic;

namespace DevInfo.Lib.DI_LibDAL.Queries.DBUsers
{
    /// <summary>
    /// Provides sql queries to update records
    /// </summary>
    public static class Update
    {

        
        #region "-- public --"

        #region "-- Methods --"

        public static string UserSessionTime(string tableDBUserAccess,string sUserNId, string sDataPrefix, bool bLoginTime, DIServerType serverType)
        {
            StringBuilder sSql = new StringBuilder();
            string RetVal = string.Empty;
            string CurrentDate = string.Empty;

            if (serverType == DIServerType.MySql)
            {
                CurrentDate = Strings.Format((System.DateTime)System.DateTime.Now, "yyyy.MM.dd hh:mm:ss");
            }
            else
            {
                CurrentDate = Strings.Format((System.DateTime)System.DateTime.Now, "MM.dd.yyyy hh:mm:ss");
            }

            if (bLoginTime == true)
            {
                //--query for updating login time 
                sSql.Append("UPDATE " + tableDBUserAccess + " SET " + DIColumns.DBUserAccess.LastLogin + " = '" + CurrentDate + "'");
                sSql.Append(" WHERE " + DIColumns.DBUserAccess.UserNId + " = '" + sUserNId + "' AND " + DIColumns.DBUserAccess.DBPrefix + " = '" + sDataPrefix + "'");
            }
            else
            {
                //--query for updating logout time 
                sSql.Append("UPDATE " + tableDBUserAccess + " SET " + DIColumns.DBUserAccess.LastLogout + " = '" + CurrentDate + "'");
                sSql.Append(" WHERE " + DIColumns.DBUserAccess.UserNId + " = '" + sUserNId + "'AND " + DIColumns.DBUserAccess.DBPrefix + " = '" + sDataPrefix + "'");
            }

            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;

            return RetVal;

        }

        public static string DIUserSessionTime(string tableDIUser, string sUserNId, string sDataPrefix, bool bLoginTime, DIServerType serverType)
        {
            StringBuilder sSql = new StringBuilder();
            string RetVal = string.Empty;
            string CurrentDate = string.Empty;

            if (serverType == DIServerType.MySql)
            {
                CurrentDate = Strings.Format((System.DateTime)System.DateTime.Now, "yyyy.MM.dd hh:mm:ss tt");
            }
            else
            {
                CurrentDate = Strings.Format((System.DateTime)System.DateTime.Now, "MM.dd.yyyy hh:mm:ss tt");
            }

            if (bLoginTime == true)
            {
                //--query for updating login time 
                sSql.Append("UPDATE " + tableDIUser + " SET " + DIColumns.DIUser.LastLogin + " = '" + CurrentDate + "'");
                sSql.Append(" WHERE " + DIColumns.DIUser.UserNId + " = '" + sUserNId + "' AND " + DIColumns.DIUser.DBPrefix + " = '" + sDataPrefix + "'");
            }
            else
            {
                //--query for updating logout time 
                sSql.Append("UPDATE " + tableDIUser + " SET " + DIColumns.DIUser.LastLogout + " = '" + CurrentDate + "'");
                sSql.Append(" WHERE " + DIColumns.DIUser.UserNId + " = '" + sUserNId + "'AND " + DIColumns.DIUser.DBPrefix + " = '" + sDataPrefix + "'");
            }

            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;

            return RetVal;

        }
        public static string UserLogStatus(string tableDBUser, int sUserNId, bool bSessionStart, DIServerType serverType)
        {
            string RetVal = string.Empty;
            StringBuilder sSql = new StringBuilder();

            sSql.Append("UPDATE ");

            if (serverType == DIServerType.MySql)
            {
                sSql.Append(tableDBUser);
            }
            else
            {
                sSql.Append(" [" + tableDBUser + "] ");

            }

            sSql.Append(" SET " + DIColumns.DBUser.ISloggedIn + " =" + Math.Abs(Convert.ToInt16(bSessionStart)));
            sSql.Append(" WHERE " + DIColumns.DBUser.UserNid + "=" + sUserNId);

            RetVal = sSql.ToString();
            //--dispose 
            sSql = null;

            return RetVal;

        }
        public static string DIUserLogStatus(string tableDBUser, int sUserNId, bool bSessionStart, DIServerType serverType)
        {
            string RetVal = string.Empty;
            StringBuilder sSql = new StringBuilder();

            sSql.Append("UPDATE ");

            if (serverType == DIServerType.MySql)
            {
                sSql.Append(tableDBUser);
            }
            else
            {
                sSql.Append(" [" + tableDBUser + "] ");

            }

            sSql.Append(" SET " + DIColumns.DIUser.ISloggedIn + " =" + Math.Abs(Convert.ToInt16(bSessionStart)));
            sSql.Append(" WHERE " + DIColumns.DIUser.UserNId + "=" + sUserNId);

            RetVal = sSql.ToString();
            //--dispose 
            sSql = null;

            return RetVal;

        }

        /// <summary>
        /// Get query to update logout time to null
        /// </summary>
        /// <param name="tableDBUserAccess"></param>
        /// <param name="sUserNId"></param>
        /// <param name="sDataPrefix"></param>
        /// <returns></returns>
        public static string ClearUserLogoutTime(string tableDBUserAccess, string sUserNId, string sDataPrefix)
        {
            StringBuilder sSql = new StringBuilder();
            string RetVal = string.Empty;
            //--query for updating logout time 
            sSql.Append("UPDATE " + tableDBUserAccess + " SET " + DIColumns.DBUserAccess.LastLogout + " = null ");
            sSql.Append(" WHERE " + DIColumns.DBUserAccess.UserNId + " = '" + sUserNId + "' AND " + DIColumns.DBUserAccess.DBPrefix + " = '" + sDataPrefix + "'");

            RetVal = sSql.ToString();
            //--dispose 
            sSql = null;

            return RetVal;
        }
        public static string DIClearUserLogoutTime(string tableDBUserAccess, string sUserNId, string sDataPrefix)
        {
            StringBuilder sSql = new StringBuilder();
            string RetVal = string.Empty;
            //--query for updating logout time 
            sSql.Append("UPDATE " + tableDBUserAccess + " SET " + DIColumns.DIUser.LastLogout + " = null ");
            sSql.Append(" WHERE " + DIColumns.DIUser.UserNId + " = '" + sUserNId + "' AND " + DIColumns.DIUser.DBPrefix + " = '" + sDataPrefix + "'");

            RetVal = sSql.ToString();
            //--dispose 
            sSql = null;

            return RetVal;
        }
        /// <summary>
        /// Return Query to Update USer TAble
        /// </summary>
        /// <param name="tableDBUser">User for MySql and [User] for Other database</param>
        /// <param name="userName"></param>
        /// <param name="userPWD"></param>
        /// <param name="userNId"></param>
        /// <returns></returns>
        public static string DIUpdateUserName(string tableDBUser, string userName, string userNId)
        {
            StringBuilder sSql = new StringBuilder();
            string RetVal = string.Empty;

            sSql.Append("UPDATE " + tableDBUser );
            sSql.Append(" SET " + DIUser.UserName + "='" + userName + "'");
            
            sSql.Append(" WHERE " + DIUser.UserNId + "=" + userNId);
            

            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;
            return RetVal;
         }

        public static string UpdateUserName(string tableDBUser, string userName, string userPWD, string userNId)
        {
            StringBuilder sSql = new StringBuilder();
            string RetVal = string.Empty;

            sSql.Append("UPDATE " + tableDBUser);
            sSql.Append(" SET " + DBUser.UserName + "='" + userName + "'");
            sSql.Append(" ," + DBUser.UserPWD + "='" + userPWD + "'");
            sSql.Append(" WHERE " + DBUser.UserNid + "=" + userNId);


            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;
            return RetVal;
        }
        public static string DIUpdatePassword(string tableDBUser, string userPWD, string userNId)
        {
            StringBuilder sSql = new StringBuilder();
            string RetVal = string.Empty;

            sSql.Append("UPDATE " + tableDBUser);
            sSql.Append(" SET " + DIUser.UserPWD + "='" + userPWD + "'");
            
            sSql.Append(" WHERE " + DIUser.UserNId + "=" + userNId);


            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;
            return RetVal;
        }

         /// <summary>
         /// Return Sql query for Update records in User Access Table for the given UserNID,AccessTo and Data_Prefix
         /// </summary>
         /// <param name="iUserNId"></param>
         /// <param name="cAccessTo"></param>
         /// <param name="sDataPrefix"></param>
         /// <param name="TablesName"></param>
         /// <returns></returns>
         public static string EditUserAccess(int userNId, char accessTo, string dataPrefix, DITables tablesName)
         {
             string RetVal = null;
             StringBuilder sSql = new StringBuilder();

             sSql.Append("UPDATE " + tablesName.DBUserAccess);
             sSql.Append(" Set " + DIColumns.DBUserAccess.AccessTo + "='" + accessTo + "'");
             sSql.Append(" WHERE " + DIColumns.DBUserAccess.UserNId + "=" + userNId);
             sSql.Append(" AND " + DIColumns.DBUserAccess.DBPrefix + " = '" + dataPrefix + "'");

             RetVal = sSql.ToString();

             //--dispose 
             sSql = null;

             return RetVal;
         }

        public static string EditUserAccess(int userNId, string dataPrefix, int PermissionSource, int PermissionTimePeriod, int PermissionArea, int PermissionIC, int IsAdmin, string PermissionAreaDesc, string PermissionICDesc, DITables tableNames)
        {
            string RetVal = null;
            StringBuilder sSql = new StringBuilder();

            sSql.Append("UPDATE " + tableNames.DBUserAccess);
            sSql.Append(" Set " + DIColumns.DBUserAccess.PermissionSource + "=" + PermissionSource + "," + DIColumns.DBUserAccess.PermissionTimePeriod + "=" + PermissionTimePeriod + "," + DIColumns.DBUserAccess.PermissionArea + "=" + PermissionArea + "," + DIColumns.DBUserAccess.PermissionIC + "=" + PermissionIC + "," + DIColumns.DBUserAccess.PermissionAreaDescription + "='" + PermissionAreaDesc + "'," + DIColumns.DBUserAccess.PermissionICDescription + "= '" + PermissionICDesc + "'," + DIColumns.DBUserAccess.IsAdmin + "=" + IsAdmin);
            sSql.Append(" WHERE " + DIColumns.DBUserAccess.UserNId + "=" + userNId);
            sSql.Append(" AND " + DIColumns.DBUserAccess.DBPrefix + " = '" + dataPrefix + "'");

            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;

            return RetVal;
        }
        public static string DIEditUser(int userNId, string dataPrefix, int PermissionSource, int PermissionTimePeriod, int PermissionArea, int PermissionIC, int IsAdmin, string associatedic, string associatedicwithdesc, string associatedarea, string associatedareadesc, DITables tableNames)
        {
            string RetVal = null;
            StringBuilder sSql = new StringBuilder();

            sSql.Append("UPDATE " + tableNames.DIUser);
            sSql.Append(" Set "+ DIColumns.DIUser.PermissionSource + "=" + PermissionSource + "," + DIColumns.DIUser.PermissionTimePeriod + "=" + PermissionTimePeriod + "," + DIColumns.DIUser.PermissionArea + "=" + PermissionArea + "," + DIColumns.DIUser.PermissionIC + "=" + PermissionIC + "," + DIColumns.DIUser.AssocitedIC + "='" + associatedic + "'," + DIColumns.DIUser.AssocitedICWithParent + "= '" + associatedicwithdesc + "'," + DIColumns.DIUser.AssociatedArea + "= '" + associatedarea+"',"+DIColumns.DIUser.AssociatedAreaWithParent+"= '"+associatedareadesc+"',"+DIColumns.DIUser.IsAdmin+"="+IsAdmin);
            sSql.Append(" WHERE " + DIColumns.DIUser.UserNId + "=" + userNId);
            sSql.Append(" AND " + DIColumns.DIUser.DBPrefix + " = '" + dataPrefix + "'");

            RetVal = sSql.ToString();

            //--dispose 
            sSql = null;

            return RetVal;
        }
        
#endregion

        #endregion
    }
}
