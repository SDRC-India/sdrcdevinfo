using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.Queries.Notes;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Database
{
    /// <summary>
    /// Helps in importing assistant import from Database or template
    /// </summary>
    internal class AssistantImporter
    {
        #region "-- Private --"

        #region "-- Variables --"

        private List<string> SourceDatabaseFileNamesWPath;
        IndicatorBuilder AvailableIndicators=null;
        UnitBuilder AvailableUnit=null;
        DI6SubgroupValBuilder AvailableSubgroupVal=null;

        #endregion

        #region "-- Methods --"

        private bool IsAssistantTblExists(DIConnection dbConnection)
        {
            bool RetVal = false;
            DIQueries DBQueries;
            try
            {
                DBQueries = new DIQueries(dbConnection.DIDataSetDefault(), dbConnection.DILanguageCodeDefault(dbConnection.DIDataSetDefault()));
                // Check availability of table               
                if (dbConnection.ExecuteScalarSqlQuery("SELECT count(*) FROM " + DBQueries.TablesName.Assistant + " WHERE 1=1") != null)
                {
                    RetVal = true;
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        private void ImportAssistant(DIConnection sourceDBConnection, DIQueries sourceDBQueries)
        {
            string DataPrefix = string.Empty;
            string LanguageCode = string.Empty;
            DITables TargetTableNames;
            DITables SourceTableNames;
            string SqlString = string.Empty;
            Dictionary<string, string> SkippedTopics = new Dictionary<string, string>();


            try
            {
                DataPrefix = this._TargetDBConnection.DIDataSetDefault();

                //Get all languages from target database
                foreach (DataRow Row in this._TargetDBConnection.DILanguages(DataPrefix).Rows)
                {

                    // check language exists in source database
                    LanguageCode = Row[Language.LanguageCode].ToString();
                    if (sourceDBConnection.IsValidDILanguage(DataPrefix, LanguageCode))
                    {
                        try
                        {
                            LanguageCode = "_" + LanguageCode;

                            //create source table names
                            SourceTableNames = new DITables(DataPrefix, LanguageCode);

                            //create target table names
                            TargetTableNames = new DITables(DataPrefix, LanguageCode);

                            // overwrite Assistant_EBook table from source database to Target database
                            this.ImportEBook(ref sourceDBConnection, ref sourceDBQueries, LanguageCode, SourceTableNames, TargetTableNames);

                            //import topic info  from source database
                            try
                            {

                                // check Topic exists in target database
                                foreach (DataRow SourceTopicRow in sourceDBConnection.ExecuteDataTable(AssistantQueries.GetALLTopics(SourceTableNames.AssistantTopic)).Rows)
                                {
                                    DataTable TargetTopicsTable = null;
                                    try
                                    {
                                        TargetTopicsTable = this._TargetDBConnection.ExecuteDataTable(AssistantQueries.GetALLTopics(TargetTableNames.AssistantTopic, " where Topic_Name='" + SourceTopicRow[Assistant_Topic.TopicName].ToString() + "' "));

                                        //  Check Indicator_Gid or IUS Gids exists in target database.If not exists, then skip topic.
                                        if (this.IsIndicatorGidExistsForAssistant(SourceTopicRow[Assistant_Topic.IndicatorGId].ToString()))
                                        {
                                            if (TargetTopicsTable.Rows.Count > 0) // Overwrite
                                            {
                                                SqlString = AssistantQueries.UpdateTopicIntro(TargetTableNames.AssistantTopic,
                                                    DICommon.RemoveQuotes(SourceTopicRow[Assistant_Topic.TopicIntro].ToString()),
                                                   DICommon.RemoveQuotes(SourceTopicRow[Assistant_Topic.TopicName].ToString()),
                                                   DICommon.RemoveQuotes(SourceTopicRow[Assistant_Topic.IndicatorGId].ToString()));
                                            }
                                            else                         // create new record
                                            {
                                                SqlString = AssistantQueries.InsertTopicInfo(TargetTableNames.AssistantTopic,
                                                    DICommon.RemoveQuotes(SourceTopicRow[Assistant_Topic.TopicName].ToString()),
                                                    SourceTopicRow[Assistant_Topic.IndicatorGId].ToString(),
                                                   DICommon.RemoveQuotes(SourceTopicRow[Assistant_Topic.TopicIntro].ToString()));
                                            }

                                            this._TargetDBConnection.ExecuteNonQuery(SqlString);
                                        }
                                        else
                                        {
                                            if (!SkippedTopics.ContainsKey(SourceTopicRow[Assistant_Topic.TopicName].ToString()))
                                            {
                                                SkippedTopics.Add(SourceTopicRow[Assistant_Topic.TopicName].ToString(), SourceTopicRow[Assistant_Topic.TopicName].ToString());
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        if (TargetTopicsTable != null)
                                        {
                                            TargetTopicsTable.Dispose();
                                        }

                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.ExceptionFacade.ThrowException(ex);
                            }
                            finally
                            {
                                //dispose source database  connection
                            }

                            try
                            {
                                // get All Values of Assistant from source Database
                                DataTable SourceDbTable = sourceDBConnection.ExecuteDataTable(AssistantQueries.GetAssistantWTopicInfo(SourceTableNames.Assistant, SourceTableNames.AssistantTopic));

                                // check record exists in target database
                                foreach (DataRow SrcRow in SourceDbTable.Rows)
                                {
                                    DataTable TargetDBTable = null;
                                    try
                                    {
                                        // Check Indicator_Gid or IUS Gids exists in target database.If not exists, then skip topic.
                                        if (!SkippedTopics.ContainsKey(SrcRow[Assistant_Topic.TopicName].ToString()))
                                        {
                                            SqlString = AssistantQueries.GetAssistantWTopicInfo(TargetTableNames.Assistant, TargetTableNames.AssistantTopic,
                                                " where T.Topic_Name='" + DICommon.RemoveQuotes(SrcRow[Assistant_Topic.TopicName].ToString()) + "' and A.Assistant_Type='" +
                                                DICommon.RemoveQuotes(SrcRow[Assistant.AssistantType].ToString()) + "' ");

                                            TargetDBTable = this._TargetDBConnection.ExecuteDataTable(SqlString);

                                            if (Microsoft.VisualBasic.Information.IsDBNull(SrcRow[Assistant.AssistantText].ToString()))
                                            {
                                                SrcRow[Assistant.AssistantText] = string.Empty;
                                            }

                                            if (TargetDBTable.Rows.Count > 0)  // overwrite
                                            {
                                                SqlString = AssistantQueries.UpdateBlankTopicRecord(TargetTableNames.Assistant,
                                                    DICommon.RemoveQuotes(SrcRow[Assistant.AssistantText].ToString()),
                                                    SrcRow[Assistant.AssistantType].ToString(), TargetDBTable.Rows[0][Assistant.AssistantNId].ToString());
                                            }
                                            else                    //  create new record
                                            {
                                                // get Topic Nid from Target Database
                                                SqlString = AssistantQueries.GetALLTopics(TargetTableNames.AssistantTopic,
                                                    " where Topic_Name ='" + DICommon.RemoveQuotes(SrcRow[Assistant_Topic.TopicName].ToString()) + "' ");

                                                int TopicNid = Convert.ToInt32(this._TargetDBConnection.ExecuteDataTable(SqlString).Rows[0][Assistant_Topic.TopicNId].ToString());


                                                SqlString = AssistantQueries.CreateNewAssistantInfo(TargetTableNames.Assistant,
                                                    TopicNid, SrcRow[Assistant.AssistantType].ToString(),
                                                    DICommon.RemoveQuotes(SrcRow[Assistant.AssistantText].ToString()),
                                                    SrcRow[Assistant.AssistantOrder].ToString());
                                            }

                                            this._TargetDBConnection.ExecuteNonQuery(SqlString);
                                        }
                                    }
                                    catch (Exception e1)
                                    {
                                        ExceptionHandler.ExceptionFacade.ThrowException(e1);
                                    }
                                    finally
                                    {
                                        if (TargetDBTable != null)
                                        {
                                            TargetDBTable.Dispose();
                                        }
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.ExceptionFacade.ThrowException(ex);
                            }
                            finally
                            {
                                //dispose source database connection
                            }
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.ExceptionFacade.ThrowException(ex);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

        }

        private bool IsIndicatorGidExistsForAssistant(string GIDs)
        {
            // Check Indicator_Gid or IUS Gids exists in target database.If not exists, then skip topic.
            bool RetVal = false;
            string GID_DELIMITER = " || ";           // delimiter used when saving GID with values
            string[] sarrGID = null;

            string IndicatorGid_Nid = string.Empty;
            string UnitGid_Nid = string.Empty;
            string SubgroupValGid_Nid = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(GIDs))
                {
                    RetVal = true;
                }
                else
                {
                    sarrGID = DICommon.SplitStringNIncludeEmpyValue(GIDs, GID_DELIMITER);
                    // Check GIDs exists in the target database
                    if (sarrGID.Length > 0)
                    {
                        if (sarrGID[0].Trim().Length > 0)
                        {
                            RetVal = true;
                            IndicatorGid_Nid = this.AvailableIndicators.GetIndicatorNid(sarrGID[0].ToString(), string.Empty).ToString();
                            if (IndicatorGid_Nid.Length == 0)
                            {
                                RetVal = false;
                            }
                        }

                        if (sarrGID.Length > 1)
                        {
                            RetVal = true;
                            UnitGid_Nid = this.AvailableUnit.GetUnitNid(sarrGID[1].ToString(), string.Empty).ToString();
                            if (UnitGid_Nid.Length == 0)
                            {
                                RetVal = false;
                            }

                            SubgroupValGid_Nid = this.AvailableSubgroupVal.GetSubgroupValNid(sarrGID[2].ToString(), string.Empty).ToString();
                            if (SubgroupValGid_Nid.Length == 0)
                            {
                                RetVal = false;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                RetVal = false;
            }

            return RetVal;
        }

        private void ImportEBook(ref DIConnection Connection, ref DIQueries queries, string languageCode, DITables sourceTableNames, DITables targetTableNames)
        {
            string SqlString = string.Empty;
            string TablePrefix = this._TargetDBConnection.DIDataSetDefault();

            DataTable SourceTopicTable = null;
            string TargetConnectionString = this._TargetDBConnection.GetConnection().ConnectionString;
            string SourceConnectionString = Connection.GetConnection().ConnectionString;
            string SourceDBName = Connection.ConnectionStringParameters.DbName;
            string TargetDBName = this._TargetDBConnection.ConnectionStringParameters.DbName;
            OleDbCommand InsertCommand;
            OleDbDataAdapter Adapter;
            OleDbCommandBuilder CmdBuilder;
            DataSet EbookDataset;
            DataRow Row; 
            try
            {
                this._TargetDBConnection.ExecuteNonQuery(AssistantQueries.DeleteFrmEBook(targetTableNames.AssistanteBook));

                // get record from source database
                SourceTopicTable = Connection.ExecuteDataTable(" Select * from " + sourceTableNames.AssistanteBook);

                if (SourceTopicTable.Rows.Count > 0)
                {
                                       //dispose target and source connection
                    this._TargetDBConnection.Dispose();
                    Connection.Dispose();

                    InsertCommand = new OleDbCommand();
                    InsertCommand.Connection = new OleDbConnection(TargetConnectionString);

                    Adapter = new OleDbDataAdapter("Select * from  " + sourceTableNames.AssistanteBook, TargetConnectionString);

                    CmdBuilder = new OleDbCommandBuilder(Adapter);

                    EbookDataset = new DataSet(sourceTableNames.AssistanteBook);
                    Adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                    Adapter.Fill(EbookDataset, targetTableNames.AssistanteBook);         //Fill data adapter
                    Row = EbookDataset.Tables[0].NewRow();


                    try
                    {
                        Row[Assistant_eBook.EBook] = SourceTopicTable.Rows[0][Assistant_eBook.EBook]; //ShpBuffer
                        EbookDataset.Tables[0].Rows.Add(Row);
                        Adapter.Update(EbookDataset, targetTableNames.AssistanteBook);              // Save changes to the database
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.ExceptionFacade.ThrowException(ex);
                    }


                    if (CmdBuilder != null)
                    {
                        CmdBuilder.Dispose();
                        CmdBuilder = null;
                    }

                    if (InsertCommand != null)
                    {
                        InsertCommand.Dispose();
                        InsertCommand = null;
                    }
                    if (Adapter != null)
                    {
                        Adapter.Dispose();
                        Adapter = null;
                    }

                    //reconnect the source and target database
                    this._TargetDBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess,
                         string.Empty, string.Empty, TargetDBName, string.Empty, Common.Constants.DBPassword));
                    Connection = new DIConnection(SourceConnectionString, DIServerType.MsAccess);
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (SourceTopicTable != null)
                {
                    SourceTopicTable.Dispose();
                }
            }


        }

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables / Properties --"

        private DIConnection _TargetDBConnection;
        /// <summary>
        /// Sets target database connection
        /// </summary>
        internal DIConnection TargetDBConnection
        {
            set
            {
                this._TargetDBConnection = value;
            }
        }


        private DIQueries _TargetDBQueries;
        /// <summary>
        /// Sets target database queries
        /// </summary>
        internal DIQueries TargetDBQueries
        {
            set
            {
                this._TargetDBQueries = value;
            }
        }

        #endregion

        #region"-- New/Dispose --"

        internal AssistantImporter(DIConnection targetConnection, DIQueries targetQueries, List<string> sourceDatabaseFileNamesWPath)
        {
            this._TargetDBConnection = targetConnection;
            this._TargetDBQueries = targetQueries;
            
            this.SourceDatabaseFileNamesWPath = sourceDatabaseFileNamesWPath;

            this.AvailableIndicators = new IndicatorBuilder(this._TargetDBConnection, this._TargetDBQueries);
            this.AvailableUnit= new UnitBuilder(this._TargetDBConnection, this._TargetDBQueries);
            this.AvailableSubgroupVal= new DI6SubgroupValBuilder(this._TargetDBConnection, this._TargetDBQueries);

        }

        #endregion

        #region "-- Methods --"

        internal void ImportAssistants()
        {
            DIConnection SourceDBConnection = null;
            DIQueries SourceDBQueries = null;
            string DataPrefix = string.Empty;
            string LanguageCode = string.Empty;

            //get all source database name
            foreach (string SourceFileNameWPath in this.SourceDatabaseFileNamesWPath)
            {
                //for each source database, import notes

                try
                {
                    SourceDBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess,
                                string.Empty, string.Empty, SourceFileNameWPath, string.Empty, Common.Constants.DBPassword));
                    DataPrefix = SourceDBConnection.DIDataSetDefault();
                    LanguageCode = SourceDBConnection.DILanguageCodeDefault(DataPrefix);
                    SourceDBQueries = new DIQueries(DataPrefix, LanguageCode);

                    this.ImportAssistant(SourceDBConnection, SourceDBQueries);

                }
                catch (Exception)
                {

                }
                finally
                {
                    if (SourceDBConnection != null)
                    {
                        SourceDBConnection.Dispose();
                        this._TargetDBConnection.Dispose();
                    }

                    if (SourceDBQueries != null)
                    {
                        SourceDBQueries = null;
                    }
                }

            }
            

        }

#endregion

        #endregion
    }

}
