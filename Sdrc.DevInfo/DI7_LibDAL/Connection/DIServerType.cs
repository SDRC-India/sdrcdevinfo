using System;
using System.Collections.Generic;
using System.Data;
namespace DevInfo.Lib.DI_LibDAL.Connection
{
    /// <summary>
    /// Retruns server type like SqlServer, MsAccess, Excel, MySql, Oracle, etc.
    /// </summary>
    public enum DIServerType
    {
        /// <summary>
        /// 0 - Microsoft SQL SErver 7.0, 2000
        /// </summary>
        SqlServer = 0,

        /// <summary>
        /// 1 - Microsoft Access
        /// </summary>
        MsAccess = 1,

        /// <summary>
        /// 2 - Oracle Multi Platform
        /// </summary>
        Oracle = 2,

        /// <summary>
        /// 3 - MySQL Open Source
        /// </summary>
        MySql = 3,

        /// <summary>
        /// 5 - Microsoft SQL Server Express Free Version
        /// </summary>
        SqlServerExpress=5,

        /// <summary>
        /// 6 - Micrsoft Excel ??
        /// </summary>
        Excel = 6,

        /// <summary>
        /// 7 - Sqlite
        /// </summary>
        Sqlite=7

    }


    /// <summary>
    /// Static class for generic datbase connection related informations
    /// </summary>
    public static class ConnectionInfo
    {

        /// <summary>
        /// 3306
        /// </summary>
        public const int MYSQL_DEFAULT_PORT = 3306;

        private static System.Collections.Generic.Dictionary<DIServerType, string> _ServerDictionary = new Dictionary<DIServerType, string>();
        /// <summary>
        /// List of server names with server types as keys
        /// </summary>
        /// <remarks>
        /// http://www.madprops.org/blog/Bind-a-ComboBox-to-a-generic-Dictionary/
        /// </remarks>
        public static System.Collections.Generic.Dictionary<DIServerType, string> ServerDictionary
        {
            get 
            {
                if (_ServerDictionary.Count == 0)
                {
                    _ServerDictionary.Add(DIServerType.SqlServer, "Microsoft SQL Server");
                    //this._ServerList.Add(DIServerType.MsAccess, "Microsoft Access");
                    _ServerDictionary.Add(DIServerType.Oracle, "Oracle");
                    _ServerDictionary.Add(DIServerType.MySql, "MySQL");
                    _ServerDictionary.Add(DIServerType.SqlServerExpress, "SQL Server Express");
                    //this._ServerList.Add(DIServerType.Excel, "Excel");
                }
                return _ServerDictionary; 
            }
        }
    }

	

	

}
