using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace DevInfo.Lib.DI_LibDAL.Connection
{

    /// <summary>
    /// Allows to set parameters for connection string and bulids connection string. 
    /// </summary>
    [Serializable]
    public class DIConnectionDetails
    {

        #region "-- Private --"

        #region "-- Variables --"

        internal const string MSAccessDefaultPassword = "unitednations2000";

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables --"

        private DIServerType _ServerType = DIServerType.MsAccess;
        /// <summary>
        /// Gets Server Type :SQL, Oracel, MySql, MsAccess, etc. 
        /// </summary>
        /// <remarks>Setter to support xml serialization and is not intended to be used directly from your code</remarks>
        public DIServerType ServerType
        {
            get
            {
                return this._ServerType;
            }

            set
            {
                this._ServerType = value;
            }
        }

        private string _ServerName = string.Empty;
        /// <summary>
        /// Gets name of the server to connect to the database
        /// </summary>
        /// <remarks>Setter to support xml serialization and is not intended to be used directly from your code</remarks>
        public string ServerName
        {
            get
            {
                return this._ServerName;
            }
            set
            {
                this._ServerName = value;
            }
        }

        private string _DbName=string.Empty;
        /// <summary>
        /// Gets database name
        /// </summary>
        /// <remarks>Setter to support xml serialization and is not intended to be used directly from your code</remarks>
        public string DbName
        {
            get
            {
                return this._DbName;
            }
            set
            {
                this._DbName = value;
            }
        }

        private string _UserName = string.Empty;
        /// <summary>
        /// Gets username to access database
        /// </summary>
        /// <remarks>Setter to support xml serialization and is not intended to be used directly from your code</remarks>
        public string UserName
        {
            get
            {
                return this._UserName;
            }
            set
            {
                this._UserName = value;
            }
        }

        private string _Password = string.Empty;
        /// <summary>
        /// Gets password.
        /// </summary>
        /// <remarks>Setter to support xml serialization and is not intended to be used directly from your code</remarks>
        public string Password
        {
            get
            {
                return this._Password;
            }

            set
            {
                this._Password = value;
            }
        }

        private string _PortNo = string.Empty;
        /// <summary>
        /// Gets PortNo. Required only for MySql databases.
        /// </summary>
        /// <remarks>Setter to support xml serialization and is not intended to be used directly from your code</remarks>
        public string PortNo
        {
            get
            {
                return this._PortNo;
            }
            set
            {
                this._PortNo = value;
            }
        }


        #endregion

        #region "-- New / Dispose --"

        /// <summary>
        /// Returns Object of ConnectionString which helps to get connection string.
        /// </summary>
        /// <param name="serverType">Server Type :SQL, Oracel, MySql, MsAccess, etc </param>
        /// <param name="serverName">Name of the server to connect to the database. Optional for MsAccess</param>
        /// <param name="portNo">Port No. Required for MySql(default portNo: 3306). </param>
        /// <param name="databaseName">Database name.Set full file path for MsAccess </param>
        /// <param name="userName">User name to access database.Optional for MsAccess</param>
        /// <param name="password">Password to access database</param>
        public DIConnectionDetails(DIServerType serverType, string serverName, string portNo, string databaseName, string userName, string password)
        {
            this._ServerType = serverType;
            this._ServerName = serverName;
            this._PortNo = portNo;
            this._DbName = databaseName;
            this._UserName = userName;
            this._Password = password;
        }
        /// <summary>
        /// Public default parameterless constructor to support xml serialization and is not intended to be used directly from your code
        /// </summary>
        public DIConnectionDetails()
        {

        }


        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Returns ADO.Net connection string syntax used for creating DBConnection
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            string RetVal = string.Empty;
            string DatabaseName =DIQueries.RemoveQuotes(this._DbName);
            string ServerName = DIQueries.RemoveQuotes(this._ServerName);

            switch (this.ServerType)
            {
                case DIServerType.SqlServer:


                    RetVal = "Server='" + ServerName + "';Database='" + DatabaseName + "';Uid='" + this._UserName + "';Pwd='" + this._Password + "';";
                    break;

                case DIServerType.MsAccess:
                    //sets default password if password is missing
                    if (string.IsNullOrEmpty(this._Password))
                    {
                        this._Password = DIConnectionDetails.MSAccessDefaultPassword;
                    }

                    RetVal = "Provider = Microsoft.Jet.OLEDB.4.0;Data source ='" + DatabaseName + "';Jet OLEDB:Database Password=" + this._Password + ";Persist Security Info=False;";

                    break;

                case DIServerType.Excel:
                    //TODO
                    //"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\MySpreadsheet.XLS;Extended Properties=""Excel 8.0;HDR=Yes;IMEX=2"""
                    //Accessing Excel Spreadsheet in C#    
                    //http://www.c-sharpcorner.com/UploadFile/bourisaw/AccessExcelDb08292005061358AM/AccessExcelDb.aspx
                    break;

                case DIServerType.MySql:

                    RetVal = "Server='" + ServerName + "';Port='" + this._PortNo + "';Database='" + DatabaseName + "';Uid='" + this._UserName + "';Pwd='" + this._Password + "';";
                    break;


                case DIServerType.Oracle:
                    

                    //RetVal = "Provider=msdaora;Data Source=" + this.ServerName + ";User Id=" + this.UserName + ";Password=" + this.Password + ";";
                    RetVal = "Data Source=" + ServerName + ";User Id=" + this._UserName + ";Password=" + this._Password + ";";
                    //"Data Source=orcl;User Id=system;Password=system;"
                    break;

                case DIServerType.SqlServerExpress:

                    RetVal = "Server=" + ServerName + ";AttachDbFilename='" + DatabaseName + "';Trusted_Connection=Yes;";
                    break;

                case DIServerType.Sqlite:
                    RetVal= "Data Source =" + DatabaseName + "; Version = 3; New = false;";
                    break;

                default:
                    break;
            }
            return RetVal;
        }
        /// <summary>
        /// Determines whether the specified Object instances are considered equal. 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object obj)
        {
            //Guidelines for Overloading Equals() and Operator
            //http://msdn2.microsoft.com/en-us/library/ms173147(VS.80).aspx

            bool RetVal = false;

            // If parameter is null return false.
            if (obj == null)
            {
                RetVal = false;
            }
            else
            {
                // cast to DIConnectionDetails.
                DIConnectionDetails p = obj as DIConnectionDetails;
                RetVal = this.Equals(p);
            }

            return RetVal;
        }

        /// <summary>
        /// Determines whether the specified DIConnectionDetails instances are considered equal. 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        /// <remarks>
        /// It is also recommended that in addition to implementing Equals(object), 
        /// any class also implement Equals(type) for their own type, to enhance performance
        /// </remarks>
        public bool Equals(DIConnectionDetails p)
        {
            bool RetVal = false;

            // If parameter is null return false:
            if ((System.Object)p == null)
            {
                RetVal = false;
            }
            else
            {
                // Return true if all field values match:
                if (this._ServerType == p.ServerType && string.Compare(this._ServerName, p.ServerName, true) == 0 && string.Compare(this._DbName, p.DbName, true) == 0 && string.Compare(this._UserName, p.UserName, true) == 0 && string.Compare(this._Password, p.Password, true) == 0 && string.Compare(this._PortNo, p.PortNo, true) == 0)
                {
                    RetVal = true;
                }
                else
                {
                    RetVal = false;
                }
            }

            return RetVal;
        }

        #region " --Export Import --"
        /// <summary>
        /// Get Connection details from physical file and deserilize it into DIConnectionDetails
        /// </summary>
        /// <param name="ConnectionFile"></param>
        /// <returns></returns>
        public static DIConnectionDetails GetConnectionDetails(string ConnectionFile)
        {

            DIConnectionDetails RetVal = null;
            Stream oStream = null;

            IFormatter formatter = new BinaryFormatter();
            try
            {
                if (System.IO.File.Exists(ConnectionFile))
                {
                    oStream = new FileStream(ConnectionFile, System.IO.FileMode.Open);
                    RetVal = (DIConnectionDetails)formatter.Deserialize(oStream);
                    oStream.Close();
                }
            }
            catch (Exception ex)
            {
                if (oStream != null)
                {
                    oStream.Dispose();
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Serialize and Save Connection Details to physical file
        /// </summary>
        /// <param name="ConnectionFile"></param>
        public void SaveConnectionDetails(string ConnectionFile)
        {
            Stream oStream = null;
            IFormatter formatter = new BinaryFormatter();
            try
            {
                oStream = new FileStream(ConnectionFile, System.IO.FileMode.Create);
                formatter.Serialize(oStream, this);
                oStream.Close();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #endregion

        #endregion

    }
}
