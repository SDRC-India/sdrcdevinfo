using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibDAL.Queries.DBUsers
{
    /// <summary>
    /// Provides sql queries to get records
    /// </summary>
    public class Select
    {
        #region "-- Private --"

        #region "-- Table Name --"
       
        DITables TablesName;

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
        
        public Select(DITables tablesName)
        {
            this.TablesName = tablesName;
        }

        #endregion


        #region "-- Methods --"

       

        public string UserAuthenticate(string sUserName, string sPassword, string sDataPrefix, DIServerType serverType)
        {

            string RetVal = string.Empty;
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT UD." + DIColumns.DBUserAccess.AccessTo + ", U." + DIColumns.DBUser.UserNid + ", U." + DIColumns.DBUser.ISAdmin );
           
            if (serverType == DIServerType.MySql)
            {
                SqlQuery.Append(" FROM " + TablesName.DBUser + " U,");
            }
            else
            {
                SqlQuery.Append(" FROM [" + TablesName.DBUser + "] U,");
            }

            SqlQuery.Append ( TablesName.DBUserAccess + " UD ");

            SqlQuery.Append(" WHERE UD." + DIColumns.DBUserAccess.UserNId + " = U." + DIColumns.DBUser.UserNid + " AND UD." + DIColumns.DBUserAccess.DBPrefix
                + " = '" + sDataPrefix + "'" + " AND U." + DIColumns.DBUser.UserName + "='" + sUserName + "' AND U." + DIColumns.DBUser.UserPWD + " = '" + sPassword + "'");

            RetVal = SqlQuery.ToString();
           
            return RetVal;

        }
        public string DIUserAuthenticate(string sUserName, string sPassword, string sDataPrefix, DIServerType serverType)
        {

            string RetVal = string.Empty;
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT " + DIColumns.DIUser.UserNId + "," + DIColumns.DIUser.IsAdmin + "," + DIColumns.DIUser.PermissionSource + "," + DIColumns.DIUser.PermissionTimePeriod +"," + DIColumns.DIUser.PermissionArea + "," + DIColumns.DIUser.PermissionIC );

            if (serverType == DIServerType.MySql)
            {
                SqlQuery.Append(" FROM " + TablesName.DIUser);
            }
            else
            {
                SqlQuery.Append(" FROM [" + TablesName.DIUser + "]");
            }

            

            SqlQuery.Append(" WHERE " + DIColumns.DIUser.DBPrefix
                + " = '" + sDataPrefix + "'" + " AND " + DIColumns.DIUser.UserName + "='" + sUserName + "' AND " + DIColumns.DIUser.UserPWD + " = '" + sPassword + "'");

            RetVal = SqlQuery.ToString();
            return RetVal;

        }

       
        public string UserMgmtAuthenticate(string sUserName, string sPassword,DIServerType serverType)
        {
            string RetVal = string.Empty;
            StringBuilder sSql = new StringBuilder();

            sSql.Append("SELECT U." + DIColumns.DBUser.UserNid + ", U." + DIColumns.DBUser.ISloggedIn);

            if (serverType == DIServerType.MySql)
            {
                sSql.Append(" FROM " + TablesName.DBUser + " U ");
            }
            else
            {
                sSql.Append(" FROM [" + TablesName.DBUser + "] U ");
            }

            //sSql.Append(" WHERE U." + DIColumns.DBUser.UserName + " = '" + sUserName + "' AND U." + DIColumns.DBUser.UserPWD + " = '" + sPassword + "' AND " + DIColumns.DBUser.ISAdmin + " <> 0");

            sSql.Append(" WHERE U." + DIColumns.DBUser.UserName + " = '" + sUserName + "' AND U." + DIColumns.DBUser.UserPWD + " = '" + sPassword + "' ");
            RetVal = sSql.ToString();
            //--dispose 
            sSql = null;

            return RetVal;
        }

        public string DIUserMgmtAuthenticate(string sUserName, string sPassword, DIServerType serverType)
        {
            string RetVal = string.Empty;
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT " + DIColumns.DIUser.UserNId + "," + DIColumns.DIUser.IsAdmin + "," + DIColumns.DIUser.PermissionSource + "," + DIColumns.DIUser.PermissionTimePeriod + "," + DIColumns.DIUser.PermissionArea + "," + DIColumns.DIUser.PermissionIC + "," + DIColumns.DIUser.ISloggedIn);

            if (serverType == DIServerType.MySql)
            {
                SqlQuery.Append(" FROM " + TablesName.DIUser );
            }
            else
            {
                SqlQuery.Append(" FROM [" + TablesName.DIUser + "]");
            }
            SqlQuery.Append(" WHERE " + DIColumns.DIUser.UserName + " = '" + sUserName + "' AND " + DIColumns.DIUser.UserPWD + " = '" + sPassword + "'");
            RetVal = SqlQuery.ToString();
           

            return RetVal;
        }


        /// <summary>
        /// Returns Query for  Admin Authentication 
        /// </summary>
        /// <param name="sUserName"></param>
        /// <param name="sPassword"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public string UserAdminMgmtAuthenticate(string userName, string password, DIServerType serverType)
        {
            string RetVal = string.Empty;
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT U." + DIColumns.DBUser.UserNid + ", U." + DIColumns.DBUser.ISloggedIn);

            if (serverType == DIServerType.MySql)
            {
                SqlQuery.Append(" FROM " + TablesName.DBUser + " U ");
            }
            else
            {
                SqlQuery.Append(" FROM [" + TablesName.DBUser + "] U ");
            }

            SqlQuery.Append(" WHERE U." + DIColumns.DBUser.UserName + " = '" + userName + "' AND U." + DIColumns.DBUser.UserPWD + " = '" + password + "' and "+DIColumns.DBUser.ISAdmin+" <> 0 " );
            RetVal = SqlQuery.ToString();
            return RetVal;
        }

        public string DIUserAdminMgmtAuthenticate(string userName, string password, DIServerType serverType)
        {
            string RetVal = string.Empty;
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT " + DIColumns.DIUser.UserNId + "," + DIColumns.DIUser.IsAdmin + "," + DIColumns.DIUser.ISloggedIn);

            if (serverType == DIServerType.MySql)
            {
                SqlQuery.Append(" FROM " + TablesName.DIUser );
            }
            else
            {
                SqlQuery.Append(" FROM [" + TablesName.DIUser + "]");
            }

            SqlQuery.Append(" WHERE " + DIColumns.DIUser.UserName + " = '" + userName + "' AND " + DIColumns.DIUser.UserPWD + " = '" + password + "' and " + DIColumns.DIUser.IsAdmin + " <> 0 ");
            RetVal = SqlQuery.ToString();
            return RetVal;
        }

        /// <summary>
        /// Return Query to Check User exist or Not
        /// </summary>
        /// <param name="filterFieldType"></param>
        /// <param name="filterText"></param>
        /// <param name="fieldSelection"></param>
        /// <param name="editMode"></param>
        /// <param name="userNId"></param>
        /// <param name="serverType"></param>
        /// <returns></returns>
        public string CheckUserExistence(FilterFieldType filterFieldType, string filterText, FieldSelection fieldSelection, bool editMode, int userNId, DIServerType serverType) 
        {
            StringBuilder sbQuery = new StringBuilder();
            string RetVal = string.Empty;

            sbQuery.Append("SELECT " + DBUser.UserNid + "," + DBUser.UserName + "," + DBUser.UserPWD + "," + DBUser.ISAdmin + "," + DBUser.ISloggedIn);

            //- Get User table Name for Servertype
            if (serverType == DIServerType.MySql)
                sbQuery.Append(" FROM " + TablesName.DBUser + " ");
            else
                sbQuery.Append(" FROM [" + TablesName.DBUser + "] ");

            //   WHERE Clause
            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
                sbQuery.Append(" WHERE ");

            if (filterText.Length > 0)
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DBUser.UserNid  + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DBUser.UserName + " = ('" + filterText + "')");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(filterText);
                        break;
                   case FilterFieldType.NIdNotIn:
                       sbQuery.Append(DBUser.UserNid + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DBUser.UserName + " <> ('" + filterText + "')");
                        break;
                    default:
                        break;
                }

                if (editMode)
                    sbQuery.Append(" AND User_Nid <> " + userNId);
            }

            RetVal = sbQuery.ToString();

            //--dispose 
            sbQuery = null;
            return RetVal;
        }
        public string CheckDIUserExistence(FilterFieldType filterFieldType, string filterText, FieldSelection fieldSelection, bool editMode, int userNId, DIServerType serverType)
        {
            StringBuilder sbQuery = new StringBuilder();
            string RetVal = string.Empty;

            sbQuery.Append("SELECT " + DIUser.UserNId+ "," + DIUser.UserName + "," + DIUser.UserPWD + "," + DIUser.IsAdmin + "," + DIUser.ISloggedIn);

            //- Get User table Name for Servertype
            if (serverType == DIServerType.MySql)
                sbQuery.Append(" FROM " + TablesName.DIUser + " ");
            else
                sbQuery.Append(" FROM [" + TablesName.DIUser + "] ");

            //   WHERE Clause
            if (filterFieldType != FilterFieldType.None && filterText.Length > 0)
                sbQuery.Append(" WHERE ");

            if (filterText.Length > 0)
            {
                switch (filterFieldType)
                {
                    case FilterFieldType.None:
                        break;
                    case FilterFieldType.NId:
                        sbQuery.Append(DIUser.UserNId + " IN (" + filterText + ")");
                        break;
                    case FilterFieldType.ParentNId:
                        break;
                    case FilterFieldType.Name:
                        sbQuery.Append(DIUser.UserName + " = ('" + filterText + "')");
                        break;
                    case FilterFieldType.Search:
                        sbQuery.Append(filterText);
                        break;
                    case FilterFieldType.NIdNotIn:
                        sbQuery.Append(DIUser.UserNId + " NOT IN (" + filterText + ")");
                        break;
                    case FilterFieldType.NameNotIn:
                        sbQuery.Append(DIUser.UserName + " <> ('" + filterText + "')");
                        break;
                    default:
                        break;
                }

                if (editMode)
                    sbQuery.Append(" AND User_NId <> " + userNId);
            }

            RetVal = sbQuery.ToString();

            //--dispose 
            sbQuery = null;
            return RetVal;
        }
        /// <summary>
        ///  Return query for Users on based on DATAPREFIX and common users between useraccess and user table
        /// </summary>
        /// <param name="dataPrefix"></param>
        /// <returns></returns>
        public string GetUsers(string dataPrefix)
        {
            string RetVal = null;
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT DISTINCT U." + DIColumns.DBUser.UserNid + ", U." + DIColumns.DBUser.UserName + ", U." + DIColumns.DBUser.UserPWD + ", U." + DIColumns.DBUser.ISAdmin + ", U." + DIColumns.DBUser.ISloggedIn + ", ");
            SqlQuery.Append(" UA." + DIColumns.DBUserAccess.UserAccessNId + ", UA." + DIColumns.DBUserAccess.AccessTo + ", UA." + DIColumns.DBUserAccess.LastLogin + ", UA." + DIColumns.DBUserAccess.LastLogout);
            SqlQuery.Append(" FROM [" + this.TablesName.DBUser + "] U INNER JOIN " + this.TablesName.DBUserAccess + " UA ON U." + DIColumns.DBUser.UserNid + " = UA." + DIColumns.DBUserAccess.UserNId);
            SqlQuery.Append(" WHERE (UA." + DIColumns.DBUserAccess.DBPrefix + "='" + dataPrefix + "') OR (U." + DIColumns.DBUser.ISAdmin + " <> 0)");

            RetVal = SqlQuery.ToString();

            return RetVal;
        }
        
        public string GetDIUsers(string dataPrefix)
        {
            string RetVal = null;
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT DISTINCT " + DIColumns.DIUser.UserNId + "," + DIColumns.DIUser.UserName + "," + DIColumns.DIUser.UserPWD + ", ");
            SqlQuery.AppendLine(DIColumns.DIUser.PermissionSource +" = ");
            SqlQuery.AppendLine("CASE " + DIColumns.DIUser.PermissionSource);
            SqlQuery.AppendLine("WHEN 'True' THEN 'Full Access'");
            SqlQuery.AppendLine("WHEN 'False' THEN 'Limited Access'");
            SqlQuery.AppendLine("ELSE 'Limited Access'");
            SqlQuery.AppendLine("END" + ",");
            SqlQuery.AppendLine(DIColumns.DIUser.PermissionTimePeriod + " = ");
            SqlQuery.AppendLine("CASE " + DIColumns.DIUser.PermissionTimePeriod);
            SqlQuery.AppendLine("WHEN 'True' THEN 'Full Access'");
            SqlQuery.AppendLine("WHEN 'False' THEN 'Limited Access'");
            SqlQuery.AppendLine("ELSE 'Limited Access'");
            SqlQuery.AppendLine("END" + ",");
            SqlQuery.AppendLine(DIColumns.DIUser.PermissionArea + " = ");
            SqlQuery.AppendLine("CASE " + DIColumns.DIUser.PermissionArea);
            SqlQuery.AppendLine("WHEN 'True' THEN 'Full Access'");
            SqlQuery.AppendLine("WHEN 'False' THEN 'Limited Access'");
            SqlQuery.AppendLine("ELSE 'Limited Access'");
            SqlQuery.AppendLine("END" + ",");
            SqlQuery.AppendLine(DIColumns.DIUser.PermissionIC + " = ");
            SqlQuery.AppendLine("CASE " + DIColumns.DIUser.PermissionIC);
            SqlQuery.AppendLine("WHEN 'True' THEN 'Full Access'");
            SqlQuery.AppendLine("WHEN 'False' THEN 'Limited Access'");
            SqlQuery.AppendLine("ELSE 'Limited Access'");
            SqlQuery.AppendLine("END" + ",");
            SqlQuery.Append(DIColumns.DIUser.IsAdmin + "," + DIColumns.DIUser.LastLogin + "," + DIColumns.DIUser.LastLogout);
            SqlQuery.Append(" FROM [" + this.TablesName.DIUser + "]");
            SqlQuery.Append(" WHERE (" + DIColumns.DIUser.DBPrefix + "='" + dataPrefix + "')AND (" + DIColumns.DBUser.UserName + " <> 'Admin')");

            RetVal = SqlQuery.ToString();
            return RetVal;
        }
        public string CountUsers(string dataprefix)
        {
            string RetVal = null;
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT COUNT(*)");
            SqlQuery.Append(" FROM [" + this.TablesName.DIUser + "]");
            SqlQuery.Append(" WHERE (" + DIColumns.DIUser.DBPrefix + "='" + dataprefix + "')");

            RetVal = SqlQuery.ToString();

            return RetVal;
        }
        /// <summary>
        /// Returns the Query to find List of Associated ICNID 
        /// </summary>
        /// <param name="usernid"></param>
        /// <returns></returns>
        public string GetDIUserAssociatedICList(string usernid)
        {
            string RetVal = null;
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT DISTINCT " + DIColumns.DIUser.AssocitedIC);
            SqlQuery.Append(" FROM [" + this.TablesName.DIUser + "] ");
            SqlQuery.Append(" WHERE (" + DIColumns.DIUser.UserNId + "='" + usernid + "')");

            RetVal = SqlQuery.ToString();


            return RetVal;
        }
        /// <summary>
        /// Returns the Query to find List of Associated ICNID With Parent NID
        /// </summary>
        /// <param name="usernid"></param>
        /// <returns></returns>
        public string GetDIUserAssociatedICListWithParent(string usernid)
        {
            string RetVal = null;
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT DISTINCT " + DIColumns.DIUser.AssocitedICWithParent);
            SqlQuery.Append(" FROM [" + this.TablesName.DIUser + "] ");
            SqlQuery.Append(" WHERE (" + DIColumns.DIUser.UserNId + "='" + usernid + "')");

            RetVal = SqlQuery.ToString();

    
            return RetVal;
        }

        /// <summary>
        /// Returns the Query to find List of Associated AreaNID 
        /// </summary>
        /// <param name="usernid"></param>
        /// <returns></returns>
        public string GetDIUserAssociatedAreaList(string usernid)
        {
            string RetVal = null;
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT DISTINCT " + DIColumns.DIUser.AssociatedArea);
            SqlQuery.Append(" FROM [" + this.TablesName.DIUser + "] ");
            SqlQuery.Append(" WHERE (" + DIColumns.DIUser.UserNId + "='" + usernid + "')");

            RetVal = SqlQuery.ToString();

            return RetVal;
        }

        /// <summary>
        /// Returns the Query to find List of Associated AreaNID with Parent NID 
        /// </summary>
        /// <param name="usernid"></param>
        /// <returns></returns>
        public string GetDIUserAssociatedAreaListWithParent(string usernid)
        {
            string RetVal = null;
            StringBuilder SqlQuery = new StringBuilder();

            SqlQuery.Append("SELECT DISTINCT " + DIColumns.DIUser.AssociatedAreaWithParent);
            SqlQuery.Append(" FROM [" + this.TablesName.DIUser + "] ");
            SqlQuery.Append(" WHERE (" + DIColumns.DIUser.UserNId + "='" + usernid + "')");

            RetVal = SqlQuery.ToString();

            return RetVal;
        }
        public string CheckDBVersion(String version, string tableNames)
        {
            string RetVal = null;
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT COUNT(" + DIColumns.DBVersion.VersionNId + ")");
            SqlQuery.Append(" FROM [" + tableNames + "]");
            SqlQuery.Append(" WHERE " + DIColumns.DBVersion.VersionNumber + " IN (" + version + ")");
            RetVal = SqlQuery.ToString();
        
            return RetVal;

        }
        public string CheckExistence(string tableNames)
        {
            string RetVal = null;
            StringBuilder SqlQuery = new StringBuilder();
            SqlQuery.Append("SELECT COUNT(*) FROM  sys.objects ");
            SqlQuery.Append(" WHERE (object_id=OBJECT_ID(N'[dbo].["+tableNames+"]'))");
            RetVal = SqlQuery.ToString();
            SqlQuery = null;
            return RetVal;
           
        }
        #endregion

        #endregion

    }
}
