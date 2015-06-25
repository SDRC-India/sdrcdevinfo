using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Resources;
using System.Text;


using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;

namespace DevInfo.Lib.DI_LibDAL.Connection
{
    /// <summary>
    /// Database Connection. Methods for DataTable, DataReader, Execute NonQueries.
    /// </summary>
    /// <example>
    /// <code >
    ///     string DataPrefix = string.Empty;
    ///     string LanguageCode = string.Empty;
    /// 
    ///     //Step 1 : Create DIConnection object
    ///     DIConnection Connection = new DIConnection(DIServerType.MsAccess, "", "","c:\\MDG Info 2006.mdb", "", "");
    ///      
    ///     //Step 2 : Get default data prefix.          
    ///     DataPrefix = Connection.DIDataSetDefault();
    ///     
    ///     //Step 3 : Get default language code.
    ///     LanguageCode = Connection.DILanguageCodeDefault(DataPrefix);
    /// 
    ///     //Step 4 : Create DIQueries object 
    ///     DIQueries queries = new DIQueries(DataPrefix, LanguageCode);
    /// 
    ///     //Step 3: Get all indicator
    ///     DataTable dt= Connection.ExecuteDataTable(queries.Indicators.SelectAll());
    ///     MessageBox.Show(dt.Rows.Count.ToString());
    /// 
    /// /// 
    ///     // To check Dataset and language code exists in database or not
    ///     bool IsExists = Connection.IsValidDIDataSets(DataPrefix);
    ///     MessageBox.Show(IsExists.ToString());
    /// 
    ///     IsExists = Connection.IsValidDILanguage(DataPrefix, LanguageCode);
    ///     MessageBox.Show(IsExists.ToString());
    /// 
    ///     //To get all DevInfo dataset
    ///     DataTable DataPrefixDataTable = Connection.DIDataSets();
    /// 
    ///     //To get all available language
    ///     DataTable LanguageDataTable = Connection.DILanguages(DataPrefix);
    /// 
    ///     //To Dispose connection object
    ///     Connection.Dispose();
    /// 
    /// </code>
    /// </example>
    /// <remarks>
    /// </remarks>
    public class DIConnection : IDisposable
    {

        #region "-- Private --"

        #region "-- Variables --"

        private string ConnectionString;
        private DbProviderFactory DBProvider;
        private DIServerType DBType;
        private DbConnection DatabaseConnection;

        #endregion

        #region "-- Methods --"

        protected virtual DbProviderFactory GetProviderInstance(DIServerType dbType)
        {
            DbProviderFactory RetVal = null;

            switch (dbType)
            {
                case DIServerType.MySql:
                    RetVal = MySql.Data.MySqlClient.MySqlClientFactory.Instance;
                    break;
                case DIServerType.SqlServer:
                case DIServerType.SqlServerExpress:
                    RetVal = System.Data.SqlClient.SqlClientFactory.Instance;
                    break;
                case DIServerType.Oracle:
                    RetVal = System.Data.OracleClient.OracleClientFactory.Instance;
                    break;
                case DIServerType.MsAccess:
                case DIServerType.Excel:
                    RetVal = System.Data.OleDb.OleDbFactory.Instance;
                    break;

                //case DIServerType.Sqlite:
                //    RetVal = System.Data.SQLite.SQLiteFactory.Instance;
                //    break;
            }
            return RetVal;

        }

        private DbCommand GetSqlStringCommand(string query)
        {
            //if (string.IsNullOrEmpty(query)) throw new ArgumentException(ExceptionKeys.Default.InvalidSqlString, "query");

            return this.CreateCommandByCommandType(CommandType.Text, query);
        }

        private DbCommand GetSqlCommand(string query, CommandType commandType, List<DbParameter> parameters)
        {
            DbCommand RetVal = null;

            // get command object
            RetVal = this.CreateCommandByCommandType(commandType, query);

            // add parameters
            if (parameters != null)
            {
                foreach (DbParameter parameter in parameters)
                {
                    if (parameter != null)
                    {
                        RetVal.Parameters.Add(parameter);
                    }
                }
            }
            return RetVal;
        }

        private DbCommand CreateCommandByCommandType(CommandType commandType, string commandText)
        {
            DbCommand RetVal = this.DBProvider.CreateCommand();
            RetVal.CommandTimeout = 0;
            RetVal.CommandType = commandType;
            RetVal.CommandText = commandText;

            return RetVal;
        }

        private void InitializeConnectionObject()
        {
            //create connnection
            try
            {
                this.DatabaseConnection = this.CreateConnection();
                this.DatabaseConnection.Open();
                DIConnection._ConnectionType = this._ConnectionStringParameters.ServerType;
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private DbConnection CreateConnection()
        {
            DbConnection RetVal = null;
            try
            {
                RetVal = this.DBProvider.CreateConnection();
                RetVal.ConnectionString = this.ConnectionString;
            }
            catch (Exception ex)
            {
                RetVal = null;
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        private object DoExecuteScalar(DbCommand command)
        {
            object RetVal = new object();
            try
            {
                RetVal = command.ExecuteScalar();

            }
            catch (Exception ex)
            {
                RetVal = null;
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        private int DoExecuteNonQuery(DbCommand command)
        {
            int RetVal = -1;
            try
            {
                RetVal = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        private IDataReader DoExecuteReader(DbCommand command, CommandBehavior cmdBehavior)
        {
            IDataReader RetVal = null;
            try
            {
                RetVal = command.ExecuteReader(cmdBehavior);
            }
            catch (Exception ex)
            {
                RetVal = null;
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }


        #endregion

        #endregion

        #region "-- Public  --"

        #region "-- Variables / Properties --"

        private DIConnectionDetails _ConnectionStringParameters;
        /// <summary>
        ///  Gets parameters used in connection string.
        /// </summary>
        public DIConnectionDetails ConnectionStringParameters
        {
            get
            {
                return this._ConnectionStringParameters;
            }
        }

        private static DIServerType _ConnectionType = DIServerType.MsAccess;
        /// <summary>
        /// Get ot Set ServerType
        /// </summary>
        public static DIServerType ConnectionType
        {
            get { return DIConnection._ConnectionType; }
            set { DIConnection._ConnectionType = value; }
        }


        #endregion

        #region "-- New/Dispose --"

        /// <summary>
        /// Returns object of DIConnection.
        /// </summary>
        /// <param name="connectionString">Connection string with all parameters</param>
        /// <param name="serverType">ServerType like : Sql,MsAccess, MySql,Oracle ,etc.</param>
        public DIConnection(string connectionString, DIServerType serverType)
        {
            this.ConnectionString = connectionString;

            // get provider
            this.DBType = serverType;
            this.DBProvider = GetProviderInstance(serverType);

            // set the server type into connection parameter object
            this.SetSeverTypeInConnectionParams();

            //create connnection
            this.InitializeConnectionObject();
        }

        /// <summary>
        /// Returns object of DIConnection
        /// </summary>
        /// <param name="connection">Object of DIConnectionString</param>
        public DIConnection(DIConnectionDetails connection)
        {
            // create connection string
            this._ConnectionStringParameters = connection;
            this.ConnectionString = connection.GetConnectionString();

            // get provider
            this.DBType = connection.ServerType;
            this.DBProvider = this.GetProviderInstance(this.DBType);

            //create connnection
            this.InitializeConnectionObject();
        }

        /// <summary>
        /// Returns object of DIConnection
        /// </summary>
        /// <param name="serverType">Server Type :SQL, Oracel, MySql, MsAccess, etc </param>
        /// <param name="serverName">Name of the server to connect to the database. Optional for MsAccess</param>
        /// <param name="portNo">Port No. Required for MySql(default portNo: 3306). </param>
        /// <param name="databaseName">Database name.Set full file path for MsAccess </param>
        /// <param name="userName">User name to access database.Optional for MsAccess</param>
        /// <param name="password">Password to access database</param>
        public DIConnection(DIServerType serverType, string serverName, string portNo, string databaseName, string userName, string password)
        {

            // Extract Port Number from the Server Name if available
            if (serverName.Contains(":"))
            {
                portNo = serverName.Substring(serverName.IndexOf(":") + 1);
                serverName = serverName.Substring(0, serverName.IndexOf(":"));
            }

            // create connection string
            this._ConnectionStringParameters = new DIConnectionDetails(serverType, serverName, portNo, databaseName, userName, password);
            this.ConnectionString = this._ConnectionStringParameters.GetConnectionString();

            // get provider
            this.DBType = this._ConnectionStringParameters.ServerType;
            this.DBProvider = this.GetProviderInstance(this.DBType);

            //create connnection
            this.InitializeConnectionObject();
        }


        #region IDisposable Members
        /// <summary>
        /// Releases all resources used by DIConnection object.
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (this.DatabaseConnection != null)
                {
                    if (this.DatabaseConnection.State != ConnectionState.Closed)
                    {
                        this.DatabaseConnection.Close();
                    }
                    this.DatabaseConnection.Dispose();
                }
            }
            catch (Exception ex)
            {

                ExceptionFacade.ThrowException(ex);
            }

        }

        #endregion

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Sets  the server type into connection parameter object which can be used in Sql queries.
        /// </summary>
        public void SetSeverTypeInConnectionParams()
        {
            this._ConnectionStringParameters = new DIConnectionDetails();
            this._ConnectionStringParameters.ServerType = this.DBType;
        }

        /// <summary>
        /// Returns instance of DBConnection
        /// </summary>
        /// <returns></returns>
        public DbConnection GetConnection()
        {
            return this.DatabaseConnection;
        }


        /// <summary>
        /// Executes the Scalar Query on the DataBase
        /// </summary>
        /// <param name="sqlQuery">string Sqlquery</param>
        /// <returns>Object</returns>
        public object ExecuteScalarSqlQuery(string sqlQuery)
        {
            object RetVal = null;
            DbCommand Command = this.GetSqlStringCommand(sqlQuery);

            DIConnection.PrepareCommand(Command, this.DatabaseConnection);
            RetVal = DoExecuteScalar(Command);


            return RetVal;
        }


        /// <summary>
        /// Executes the Scalar Query on the DataBase
        /// </summary>
        /// <param name="sqlQuery">string Sqlquery</param>
        /// <param name="commandType">Type of command, like stored procedure, sql query,etc</param>
        /// <param name="parameters">List of DBParameter.To create Dbparameter use CreateDBParameter() method </param>
        /// <returns>Object</returns>
        public object ExecuteScalarSqlQuery(string sqlQuery, CommandType commandType, List<DbParameter> parameters)
        {
            object RetVal = null;
            DbCommand Command = this.GetSqlCommand(sqlQuery, commandType, parameters);
            DIConnection.PrepareCommand(Command, this.DatabaseConnection);

            RetVal = DoExecuteScalar(Command);


            return RetVal;
        }

        /// <summary>
        /// Executes a SQL statement
        /// </summary>
        /// <param name="sqlQuery">sql query</param>
        /// <returns>Number of affected rows</returns>
        public int ExecuteNonQuery(string sqlQuery)
        {
            int RetVal = -1;
            DbCommand Command = this.GetSqlStringCommand(sqlQuery);

            //execute sql query
            DIConnection.PrepareCommand(Command, this.DatabaseConnection);
            RetVal = this.DoExecuteNonQuery(Command);

            return RetVal;
        }

        /// <summary>
        /// Executes a SQL statement
        /// </summary>
        /// <param name="sqlQuery">sql query</param>
        /// <param name="commandType">Type of command, like stored procedure, sql query,etc</param>
        /// <param name="parameters">List of DBParameter.To create Dbparameter use CreateDBParameter() method </param>
        /// <returns>Number of affected rows</returns>
        public int ExecuteNonQuery(string sqlQuery, CommandType commandType, List<DbParameter> parameters)
        {
            int RetVal = -1;
            DbCommand Command = this.GetSqlCommand(sqlQuery, commandType, parameters);

            //execute sql query
            DIConnection.PrepareCommand(Command, this.DatabaseConnection);
            RetVal = this.DoExecuteNonQuery(Command);

            return RetVal;
        }

        /// <summary> 
        /// Returns the results of sql query executed on the databases 
        /// </summary>
        /// <param name="sqlQuery"> string sqlQuery</param>
        /// <returns>Returns the IDataReader</returns>
        public IDataReader ExecuteReader(string sqlQuery)
        {

            IDataReader RetVal;
            DbCommand Command = this.GetSqlStringCommand(sqlQuery);

            DIConnection.PrepareCommand(Command, this.DatabaseConnection);
            RetVal = this.DoExecuteReader(Command, CommandBehavior.Default);

            return RetVal;
        }

        /// <summary>
        /// Creates the new instance of parameter using common DbProvider.
        /// </summary>
        /// <returns></returns>
        public DbParameter CreateDBParameter()
        {
            return this.DBProvider.CreateParameter();
        }

        /// <summary>
        /// Create instance of DBDataAdapter using common DbProvider.
        /// </summary>
        /// <returns></returns>
        public DbDataAdapter CreateDBDataAdapter()
        {
            return this.DBProvider.CreateDataAdapter();
        }

        /// <summary>
        /// Returns the instance of  current DBProviderFactory
        /// </summary>
        /// <returns></returns>
        public DbProviderFactory GetCurrentDBProvider()
        {
            return this.DBProvider;
        }


        /// <summary> 
        /// Returns the results of sql query executed on the databases 
        /// </summary>
        /// <param name="sqlQuery"> string sqlQuery</param>
        /// <param name="commandType">Type of command, like stored procedure, sql query,etc</param>
        /// <param name="parameters">List of DBParameter.To create Dbparameter use CreateDBParameter() method </param>
        /// <returns>Returns the IDataReader</returns>
        public IDataReader ExecuteReader(string sqlQuery, CommandType commandType, List<DbParameter> parameters)
        {

            IDataReader RetVal;
            DbCommand Command = this.GetSqlCommand(sqlQuery, commandType, parameters);

            DIConnection.PrepareCommand(Command, this.DatabaseConnection);
            RetVal = this.DoExecuteReader(Command, CommandBehavior.Default);

            return RetVal;
        }

        /// <summary>
        /// Return the Datatable
        /// </summary>
        /// <param name="sqlQuery">string sqlquery</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sqlQuery)
        {
            DataTable RetVal = new DataTable();
            IDataReader Reader;

            DbCommand Command = GetSqlStringCommand(sqlQuery);

            DIConnection.PrepareCommand(Command, this.DatabaseConnection);
            Reader = DoExecuteReader(Command, CommandBehavior.Default);
            if (Reader != null)
            {
                try
                {
                    RetVal.Load(Reader);
                }
                catch (System.Data.ConstraintException cx)
                {
                    Reader.Close();

                    switch (this._ConnectionStringParameters.ServerType)
                    {
                        case DIServerType.MySql:
                        case DIServerType.Sqlite:
                            break;
                        case DIServerType.MsAccess:
                        case DIServerType.Oracle:
                        case DIServerType.SqlServer:
                        case DIServerType.SqlServerExpress:
                            ExceptionFacade.ThrowException(cx);
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Reader.Close();
                    throw;
                }
            }
            else
            {
                RetVal = null;
            }

            return RetVal;
        }

        /// <summary>
        /// Return the Datatable
        /// </summary>
        /// <param name="sqlQuery">string sqlquery</param>
        /// <param name="commandType">Type of command, like stored procedure, sql query,etc</param>
        /// <param name="parameters">List of DBParameter.To create Dbparameter use CreateDBParameter() method </param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string sqlQuery, CommandType commandType, List<DbParameter> parameters)
        {
            DataTable RetVal = new DataTable();
            IDataReader Reader;

            DbCommand Command = this.GetSqlCommand(sqlQuery, commandType, parameters);

            DIConnection.PrepareCommand(Command, this.DatabaseConnection);
            Reader = DoExecuteReader(Command, CommandBehavior.Default);
            RetVal.Load(Reader);

            return RetVal;
        }

        /// <summary>
        /// Returns DevInfo datasets.
        /// </summary>
        /// <returns>DataTable</returns>
        public DataTable DIDataSets()
        {
            return this.ExecuteDataTable(DIQueries.GetAllDataset());
        }

        /// <summary>
        /// Returns default dataprefix
        /// </summary>
        /// <returns>string</returns>
        public string DIDataSetDefault()
        {
            string RetVal = string.Empty;
            try
            {
                DataTable table = this.ExecuteDataTable(DIQueries.GetAllDataset());
                if (table != null)
                {
                    if (table.Rows.Count > 0)
                    {

                        RetVal = table.Rows[0][DevInfo.Lib.DI_LibDAL.Queries.DIColumns.DBAvailableDatabases.AvlDBPrefix].ToString();
                        RetVal += "_";
                    }
                }
            }
            catch (Exception ex)
            {
                RetVal = string.Empty;
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        /// <summary>
        /// Returns default language code.
        /// </summary>
        /// <param name="dataPrefix">Dataprefix like UT_</param>
        /// <returns>string</returns>
        public string DILanguageCodeDefault(string dataPrefix)
        {
            string RetVal = string.Empty;
            try
            {
                DataTable table = this.ExecuteDataTable(DIQueries.GetDefaultLangauge(dataPrefix));
                if (table != null)
                {
                    if (table.Rows.Count > 0)
                    {

                        RetVal = table.Rows[0][DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Language.LanguageCode].ToString();
                        RetVal = "_" + RetVal;
                    }
                }
            }
            catch (Exception ex)
            {
                RetVal = string.Empty;
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;

        }

        /// <summary>
        /// Returns all available languages
        /// </summary>
        /// <param name="dataPrefix">Dataprefix like UT_</param>
        /// <returns>DataTable</returns>
        public DataTable DILanguages(string dataPrefix)
        {
            return this.ExecuteDataTable(DIQueries.GetLangauges(dataPrefix));
        }

        /// <summary>
        /// Returns language name for the given language code
        /// </summary>
        /// <param name="dataPrefix">Dataprefix like UT_</param>
        /// <param name="lngCode">like en</param>
        /// <returns>DataTable</returns>
        public string GetLanguageName(string dataPrefix, string lngCode)
        {
            string RetVal = string.Empty;

            foreach (DataRow Row in this.ExecuteDataTable(DIQueries.GetLangaugeName(dataPrefix, lngCode)).Rows)
            {
                RetVal = Convert.ToString(Row[Language.LanguageName]);
                break;
            }

            return RetVal;
        }

        /// <summary>
        /// Returns true/false.True if langauage code exists
        /// </summary>
        /// <param name="dataPrefix">Dataprefix like UT_</param>
        /// <param name="languageCode">Language code like en or _en</param>
        /// <returns>true/false</returns>
        public bool IsValidDILanguage(string dataPrefix, string languageCode)
        {
            bool RetVal = false;
            try
            {
                if (Convert.ToDouble(this.ExecuteScalarSqlQuery(DIQueries.CheckLanguageExists(dataPrefix, languageCode))) > 0)
                {
                    RetVal = true;
                }

            }
            catch (Exception ex)
            {
                RetVal = false;
            }

            return RetVal;
        }

        /// <summary>
        /// Returns true/false.True if dataprefix exists.
        /// </summary>
        /// <param name="dataPrefix">Dataprefix like UT or UT_</param>
        /// <returns>true/false</returns>
        public bool IsValidDIDataSets(string dataPrefix)
        {

            bool RetVal = false;
            try
            {
                if ((int)this.ExecuteScalarSqlQuery(DIQueries.CheckDatasetExists(dataPrefix)) > 0)
                {
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                RetVal = false;
            }
            return RetVal;
        }

        /// <summary>
        /// Inserts new database file name into Available Database table
        /// </summary>
        /// <param name="newDBfileName">Database file name without path</param>
        public void InsertNewDBFileName(string dataPrefix, string newDBfileName)
        {

            try
            {

                this.ExecuteNonQuery(DIQueries.InsertNewDBName(dataPrefix, newDBfileName));

            }
            catch (Exception ex)
            {

                throw new ApplicationException(ex.ToString());
            }

        }

        /// <summary>
        /// get the IDENTITY / AUTONUMBER value for the last row inserted into database
        /// </summary>
        /// <returns></returns>
        public int GetNewId()
        {
            return Convert.ToInt32(this.ExecuteScalarSqlQuery(DIQueries.GetNewNID()));
        }

        /// <summary>
        /// Drops the given table
        /// </summary>
        /// <param name="tableName"></param>
        public void DropTable(string tableName)
        {
            try
            {
                this.ExecuteNonQuery(DIQueries.DropTable(tableName));
            }
            catch (Exception)
            {
                // do nothing
            }
        }

        /// <summary>
        /// Drops the given column 
        /// <param name="columnName"></param>
        /// <param name="tableName"></param>
        /// </summary>
        public void DropIndividualColumnOfTable(string tableName, string columnName)
        {
            try
            {
                this.ExecuteNonQuery(DIQueries.DropIndividualColumnOfTable(tableName, columnName));
            }
            catch (Exception)
            {
                //do nothing
            }
        }

        /// <summary>
        /// Adds column into existing table
        /// <param name="columnName"></param>
        /// <param name="tableName"></param>
        /// <param name="dataType"></param>
        /// <param name="defaultValue">may be empty</param>
        /// </summary>
        public void AddColumn(string tableName, string columnName, string dataType, string defaultValue)
        {
            this.ExecuteNonQuery(DIQueries.AddColumn(tableName, columnName, dataType, defaultValue));
        }

        /// <summary>
        /// Update the default dataset.
        /// </summary>
        /// <param name="datasetPrefix">Dataset Prefix</param>
        public void SetDefaultDataSet(string datasetPrefix)
        {
            DITables DITableName = new DITables(string.Empty, string.Empty);
            this.ExecuteNonQuery("UPDATE " + DITableName.DBAvailableDatabases + " SET " + DI_LibDAL.Queries.DIColumns.DBAvailableDatabases.AvlDBDefault + " = '1' WHERE " + DI_LibDAL.Queries.DIColumns.DBAvailableDatabases.AvlDBPrefix + " = '" + datasetPrefix + "'");
        }

        #region "-- Create and Drop Index --"

        public void CreateIndex(string indexName, string indexOnTableName, string columnName)
        {
            try
            {
                this.ExecuteNonQuery(DIQueries.CreateIndex(indexName, indexOnTableName, columnName));
            }
            catch (Exception ex)
            {
                //do nothing
            }
        }

        public void DropIndex(string indexName, string indexOnTableName)
        {
            try
            {
                this.ExecuteNonQuery(DIQueries.DropIndex(indexName, indexOnTableName));
            }
            catch (Exception ex)
            {
                //do nothing
            }
        }

        #endregion

        #endregion

        #endregion

        #region "-- Static --"

        private static void PrepareCommand(DbCommand command, DbConnection connection)
        {
            try
            {
                if (command == null) throw new ArgumentNullException("command");
                if (connection == null) throw new ArgumentNullException("connection");

                command.Connection = connection;
                //command.Connection.Open();
            }

            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);

            }
        }


        /// <summary>
        /// Iterate a DataReader and get comma Delimited Values for defined column
        /// </summary>
        /// <param name="rd"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static string GetDelimitedValuesFromReader(IDataReader rd, string FieldName)
        {
            string RetVal = string.Empty;
            StringBuilder sb = new StringBuilder();
            while (rd.Read())
            {
                sb.Append(rd[FieldName].ToString() + ",");
            }
            RetVal = sb.ToString();

            // Remove trailing "," if any
            if (RetVal.Length > 0)
            {
                RetVal = RetVal.Substring(0, RetVal.Length - 1);
            }
            return RetVal;
        }
        /// <summary>
        /// Iterate a DataView and get comma Delimited Values for defined column
        /// </summary>
        /// <param name="rd"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static string GetDelimitedValuesFromDataView(DataView dv, string FieldName)
        {
            string RetVal = string.Empty;
            RetVal = GetDelimitedValuesFromDataTable(dv.ToTable(), FieldName);
            return RetVal;
        }

        /// <summary>
        /// Iterate a DataTable and get comma Delimited Values for defined column
        /// </summary>
        /// <param name="rd"></param>
        /// <param name="FieldName"></param>
        /// <returns></returns>
        public static string GetDelimitedValuesFromDataTable(DataTable dt, string FieldName)
        {
            string RetVal = string.Empty;
            List<string> DistinctList = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(dr[FieldName].ToString().Trim()))
                {
                    if (!(DistinctList.Contains(dr[FieldName].ToString())))
                    {
                        DistinctList.Add(dr[FieldName].ToString());
                    }
                }
            }

            RetVal = string.Join(",", DistinctList.ToArray());

            return RetVal;
        }

        /// <summary>
        /// Iterate a DataTable and get comma Delimited Values for defined column
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="FieldName"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string GetDelimitedValuesFromDataTable(DataTable dt, string FieldName, string separator)
        {
            string RetVal = string.Empty;
            List<string> DistinctList = new List<string>();

            foreach (DataRow dr in dt.Rows)
            {
                if (!string.IsNullOrEmpty(dr[FieldName].ToString().Trim()))
                {
                    if (!(DistinctList.Contains(dr[FieldName].ToString())))
                    {
                        DistinctList.Add(dr[FieldName].ToString());
                    }
                }
            }

            RetVal = string.Join(separator, DistinctList.ToArray());

            return RetVal;
        }


        /// <summary>
        /// Copies the given sourceTable from SourceDatabase into TargetDatabase
        /// </summary>
        public static void CreateLinkTableForAccessDB(string targetDatabase, string sourceDatabase, string targetTable, string sourceTable)
        {
            // Create another connection to read SourceDataBase.
            OleDbConnection Connection;
            OleDbCommand Command;

            string SqlString = string.Empty;
            DIConnection TargetDBConnection = null;

            try
            {
                string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data source=" + sourceDatabase + ";Jet OLEDB:Database Password=" + DIConnectionDetails.MSAccessDefaultPassword + ";Persist Security Info=False;";
                Connection = new OleDbConnection(ConnectionString);
                Command = new OleDbCommand();
                Command.Connection = Connection;
                Connection.Open();



                // 1. Drop target table if already exits
                // 1.1 Connect to target database
                try
                {
                    TargetDBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, targetDatabase, string.Empty, string.Empty);
                    TargetDBConnection.DropTable(targetTable);
                }
                catch (Exception)
                {
                }
                finally
                {
                    // 1.2 Dispose connection 
                    if (TargetDBConnection != null)
                    {
                        TargetDBConnection.Dispose();
                    }
                }

                // 2. Insert Source table into targetDatabase.targetTable
                SqlString = "INSERT INTO [MS Access;Database=" + targetDatabase + ";pwd=" + DIConnectionDetails.MSAccessDefaultPassword + ";].[" + targetTable + "]" +
         " SELECT *  FROM " + sourceTable;

                Command.CommandText = SqlString;

                Command.ExecuteNonQuery();

                Connection.Close();



            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Get 1 or 0 In case of SqlServer for Bolean value
        /// </summary>
        /// <param name="boolValue"></param>
        /// <returns></returns>
        public static string GetBoolValue(bool boolValue)
        {
            string RetVal = string.Empty;

            switch (DIConnection._ConnectionType)
            {
                case DIServerType.SqlServer:
                case DIServerType.MySql:
                case DIServerType.Sqlite:
                    RetVal = Convert.ToString(boolValue ? 1 : 0);
                    break;
                default:
                    RetVal = boolValue.ToString();
                    break;
            }

            return RetVal;
        }


        #endregion





    }
}
