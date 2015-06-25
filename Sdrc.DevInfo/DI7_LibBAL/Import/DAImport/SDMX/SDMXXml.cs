using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Text;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Import.DAImport;
using DAImportCommon = DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Log;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;



namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.SDMX
{
    /// <summary>
    /// Helps in importing records from SDMX XML File
    /// </summary>
    public class SDMXXml : DIData
    {
        #region "-- Private --"

        #region "-- Methods --"

        private void ImportFrmXmlFile(string XMLFileNameWPath)
        {
            try
            {
                //Step 1: Check file is in valid format or not
                if (this.IsValidFileFormat(XMLFileNameWPath))
                {
                    //Step 2: Import Data from xml file into  TempDataTable (target database/template)           
                    this.ImportDataIntoTempDataTable(XMLFileNameWPath);
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private bool IsValidFileFormat(string fileNameWPath)
        {
            return true;
        }

        private void ImportDataIntoTempDataTable(string fileNameWPath)
        {
            string TargetConnectionString = string.Empty;
            string TargetConnectionServerName = string.Empty;
            string TargetConnectionDBName = string.Empty;
            string TargetConnectionUser = string.Empty;
            string TargetConnectionPassword = string.Empty;
            string TargetConnectionPort = string.Empty;
            DIServerType TargetConnectionServerType;
            System.Data.Common.DbDataAdapter Adapter = null;
            System.Data.Common.DbCommandBuilder CmdBuilder = null;
            DataSet TargetFileDataset = null;
            DataSet SourceDataSet = null;
            DIServerType ServerType;
            System.Data.Common.DbCommand cmd;
            DIConnection Connection;

            try
            {
                SourceDataSet = this.GetDataSetFrmXmlFile(fileNameWPath);
                if (SourceDataSet != null)
                {
                    TargetConnectionString = this.DBConnection.GetConnection().ConnectionString;
                    TargetConnectionServerName = this.DBConnection.ConnectionStringParameters.ServerName;
                    TargetConnectionServerType = this.DBConnection.ConnectionStringParameters.ServerType;
                    TargetConnectionDBName = this.DBConnection.ConnectionStringParameters.DbName;
                    TargetConnectionUser = this.DBConnection.ConnectionStringParameters.UserName;
                    TargetConnectionPassword = this.DBConnection.ConnectionStringParameters.Password;
                    TargetConnectionPort = this.DBConnection.ConnectionStringParameters.PortNo;
                    ServerType = this.DBConnection.ConnectionStringParameters.ServerType;


                    //dispose connection
                    this.DBConnection.Dispose();

                    try
                    {
                        //                        Connection = new DIConnection(TargetConnectionString, ServerType);
                        Connection = new DIConnection(TargetConnectionServerType, TargetConnectionServerName, TargetConnectionPort, TargetConnectionDBName, TargetConnectionUser, TargetConnectionPassword);
                        Adapter = Connection.CreateDBDataAdapter();
                        cmd = Connection.GetCurrentDBProvider().CreateCommand();
                        cmd.CommandText = "Select * from " + DAImportCommon.Constants.TempDataTableName;
                        cmd.Connection = Connection.GetConnection();
                        Adapter.SelectCommand = cmd;

                        CmdBuilder = Connection.GetCurrentDBProvider().CreateCommandBuilder();
                        CmdBuilder.DataAdapter = Adapter;

                        TargetFileDataset = new System.Data.DataSet();

                        Adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        try
                        {
                            Adapter.Fill(TargetFileDataset, DAImportCommon.Constants.TempDataTableName);
                        }
                        catch (Exception ex)
                        {
                            throw new ApplicationException("ImportDataIntoTempDataTable - Adapter.Fill - " + TargetConnectionServerType + " - " + TargetConnectionServerName + " - " + TargetConnectionPort + " - " + TargetConnectionDBName + " - " + TargetConnectionUser + " - " + TargetConnectionPassword + ex.ToString());
                        }

                        TargetFileDataset.Tables[DAImport.Common.Constants.TempDataTableName].Columns[Data.DataNId].AutoIncrement = true;

                        if (TargetFileDataset.Tables[DAImport.Common.Constants.TempDataTableName].Rows.Count > 0)
                        {
                            TargetFileDataset.Tables[DAImport.Common.Constants.TempDataTableName].DefaultView.Sort = TargetFileDataset.Tables[DAImport.Common.Constants.TempDataTableName].Columns[Data.DataNId].ColumnName + " DESC";

                            TargetFileDataset.Tables[DAImport.Common.Constants.TempDataTableName].Columns[Data.DataNId].AutoIncrementSeed = Convert.ToInt32(TargetFileDataset.Tables[DAImport.Common.Constants.TempDataTableName].DefaultView[0][Data.DataNId]) + 1;
                        }

                        TargetFileDataset.Tables[DAImport.Common.Constants.TempDataTableName].PrimaryKey = new DataColumn[] { TargetFileDataset.Tables[DAImport.Common.Constants.TempDataTableName].Columns[Data.DataNId] };
                        TargetFileDataset.Tables[DAImport.Common.Constants.TempDataTableName].AcceptChanges();

                        //Get tempdatatable
                        this.FillTempDataTable(SourceDataSet, TargetFileDataset.Tables[DAImport.Common.Constants.TempDataTableName]);

                        //update TempDataTable into target database
                        Adapter.Update(TargetFileDataset, DAImport.Common.Constants.TempDataTableName);
                        System.Threading.Thread.Sleep(1000);
                        Connection.Dispose();

                    }
                    catch (Exception ex)
                    {
                        ExceptionFacade.ThrowException(ex);
                    }
                    finally
                    {
                        //dispose all used objects  
                        if (Adapter != null)
                        {
                            Adapter.Dispose();
                        }

                        if (CmdBuilder != null)
                        {
                            CmdBuilder.Dispose();
                        }

                        if (TargetFileDataset != null)
                        {
                            TargetFileDataset.Dispose();
                        }

                        //reconnect to  target database
                        //this.ConnectToDatabase();
                        //this.DBConnection = new DIConnection(TargetConnectionString, ServerType);
                        this.DBConnection = new DIConnection(TargetConnectionServerType, TargetConnectionServerName, TargetConnectionPort, TargetConnectionDBName, TargetConnectionUser, TargetConnectionPassword);

                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (SourceDataSet != null)
                {
                    SourceDataSet.Dispose();
                }
            }

        }

        private DataSet GetDataSetFrmXmlFile(string fileNameWPath)
        {
            DataSet RetVal = new DataSet();
            try
            {
                RetVal.ReadXml(fileNameWPath);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
                RetVal = null;
            }
            return RetVal;
        }

        private void FillTempDataTable(DataSet SourceXmlDataset, DataTable TempDataTable)
        {
            DataTable TempSDMXDataTable;

            try
            {
                TempSDMXDataTable = SourceXmlDataset.Tables[SDMXTableNames.SeriesTable];

                // Create a relation between Code and annotations
                foreach (DataRow CodeRow in TempSDMXDataTable.Rows)
                {
                    foreach (DataRow DescriptionRow in CodeRow.GetChildRows(SourceXmlDataset.Relations[this.GetRelationIndex(SourceXmlDataset)]))
                    {
                        DataRow Drow = TempDataTable.NewRow();
                        Drow[Indicator.IndicatorGId] = CodeRow[SDMXColumnNames.IndicatorColumn];
                        Drow[Unit.UnitGId] = CodeRow[SDMXColumnNames.UnitColumn];
                        Drow[SubgroupVals.SubgroupValGId] = CodeRow[SDMXColumnNames.SubgroupValColumn];
                        Drow[Area.AreaID] = CodeRow[SDMXColumnNames.AreaColumn];
                        Drow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataValue] = DescriptionRow[SDMXColumnNames.OBSValueColumn].ToString();
                        Drow[Timeperiods.TimePeriod] = DescriptionRow[SDMXColumnNames.TimeperiodColumn].ToString();
                        Drow[DAImport.Common.Constants.SourceColumnName] = DescriptionRow[SDMXColumnNames.SourceColumn].ToString();
                        try
                        {
                            if (DescriptionRow[SDMXColumnNames.DenominatorColumn].ToString().Trim().Length > 0)
                            {
                                Drow[DevInfo.Lib.DI_LibDAL.Queries.DIColumns.Data.DataDenominator] = DescriptionRow[SDMXColumnNames.DenominatorColumn];
                            }

                        }
                        catch (Exception ex)
                        {
                        }
                        Drow[FootNotes.FootNote] = DescriptionRow[SDMXColumnNames.FootntoeColumn].ToString();
                        // Inserting Value for not null columns in dataTable.
                        Drow[Indicator.IndicatorName] = string.Empty;
                        Drow[Unit.UnitName] = string.Empty;
                        Drow[Indicator.IndicatorNId] = 0;
                        Drow[Unit.UnitNId] = 0;
                        Drow[SubgroupVals.SubgroupValNId] = 0;
                        Drow[IndicatorClassifications.ICGlobal] = false;
                        Drow[DAImport.Common.Constants.RecommendedSourceColumnName] = false;
                        Drow[DAImport.Common.Constants.NewIUSColumnName] = 0;
                        Drow[DAImport.Common.Constants.Old_Data_Nid] = 0;
                        Drow[DAImport.Common.Constants.Old_Source_Nid] = 0;

                        TempDataTable.Rows.Add(Drow);
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private String GetRelationIndex(DataSet SourceXmlDataset)
        {
            //can be changed in future
            String RetVal = string.Empty;
            RetVal = "Series_DI_Obs";
            return RetVal;
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region"-- New/Dispose --"

        internal SDMXXml(List<string> SDMXXmlFiles, string targetFileNameWPath, string tempFolderPath, string htmlLogOutputPath)
        {
            this.SetBasicPropertiesAndProcessValues(SDMXXmlFiles, targetFileNameWPath, tempFolderPath, htmlLogOutputPath);

        }

        internal SDMXXml(List<string> SDMXXmlFiles, string tempFolderPath, DIConnection dbConnection, DIQueries dbQueries)
        {
            this._DBConnection = dbConnection;

            this.SetBasicPropertiesAndProcessValuesForOnline(SDMXXmlFiles, tempFolderPath, dbQueries.DataPrefix, dbQueries.LanguageCode);
        }

        internal SDMXXml(List<string> SDMXXmlFiles, string tempFolderPath, DIConnection dbConnection, string datasetPrefix, string languageCode)
        {
            this._DBConnection = dbConnection;
            this.SetBasicPropertiesAndProcessValuesForOnline(SDMXXmlFiles, tempFolderPath, datasetPrefix, languageCode);
        }

        #endregion

        #endregion

        #region "-- Protected --"

        #region "-- Methods --"

        protected override void ProcessTempDataTable()
        {
            //step1: Process and import data from excel file into target database/template.
            foreach (string XMLFileNameWPath in this.TempFiles)
            {
                this.ImportFrmXmlFile(XMLFileNameWPath);
            }

            // Step 2: create log for null records
            this.SkippedRecordsTable = this.DBConnection.ExecuteDataTable(this.DAQuery.GetSkippedRecords());

            // Step 3: Remove all rows from Temp_Data table where Data Value is null or blank, or I, U, S are blank.
            this.DBConnection.ExecuteNonQuery(this.DAQuery.RemoveNullRecordsFromTempDataTableForSDMX());

        }

        protected override void AddNotesAssistants()
        {
            // Do Nothing : Notes and assistants does nt exist in SDMX xml file
        }

        protected override bool UploadDatabase()
        {
            bool RetVal = false;
            //--
            return RetVal;
        }
        #endregion

        #endregion

        protected override void SetElementLogInfoTable()
        {
            string ColumnName = string.Empty;
            // 1. Set Indicator LogInfo Table 
            //Adding desired 2 columns for Indicator
            this.IndicatorLogInfoTable = new DataTable();
            this.IndicatorLogInfoTable.Columns.Add(Indicator.IndicatorGId);
            this.IndicatorLogInfoTable.Columns.Add(DAImportCommon.Constants.Log.MapIndicatorColumnName);
            // Add rows from unmatched element Table.
            this.FillElementLogInfoTable(this.RequiredDIElements[RequiredDIElementsType.Indicator].UnmatchedElementsTable, ref this.IndicatorLogInfoTable, Indicator.IndicatorGId);

            // 2. Set Unit LogInfo Table.
            // Adding desired 2 columns for  Unit
            this.UnitLogInfoTable = new DataTable();
            this.UnitLogInfoTable.Columns.Add(Unit.UnitGId);
            this.UnitLogInfoTable.Columns.Add(DAImportCommon.Constants.Log.MapUnitColumnName);
            // Add rows from unmatched element Table.
            this.FillElementLogInfoTable(this.RequiredDIElements[RequiredDIElementsType.Unit].UnmatchedElementsTable, ref this.UnitLogInfoTable, Unit.UnitGId);

            // 3. Set Subgroup LogInfo Table.
            // Adding desired 2 columns  for  Subgroup
            this.SubgroupLogInfoTable = new DataTable();
            this.SubgroupLogInfoTable.Columns.Add(SubgroupVals.SubgroupValGId);
            this.SubgroupLogInfoTable.Columns.Add(DAImportCommon.Constants.Log.MapSubgroupValColumnName);
            // Add rows from unmatched element Table.
            this.FillElementLogInfoTable(this.RequiredDIElements[RequiredDIElementsType.Subgroup].UnmatchedElementsTable, ref this.SubgroupLogInfoTable, SubgroupVals.SubgroupValGId);

            // 4. Set Area LogInfo Table.
            // Adding desired 2 columns  for  Area
            this.AreaLogInfoTable = new DataTable();
            //this.AreaLogInfoTable.Columns.Add(DAImportCommon.Constants.Log.UnmatchedAreaColumnName);
            this.AreaLogInfoTable.Columns.Add(Area.AreaID);
            this.AreaLogInfoTable.Columns.Add(DAImportCommon.Constants.Log.MapAreaColumnName);
           
            // Add rows from unmatched element Table.
            this.FillElementLogInfoTable(this.RequiredDIElements[RequiredDIElementsType.Area].UnmatchedElementsTable, ref this.AreaLogInfoTable, Area.AreaID);
        }

        private void FillElementLogInfoTable(DataTable UnmatchedSourceTable, ref DataTable ElementInfoTable, string ColumnName)
        {
            foreach (DataRow UnmatchedRow in UnmatchedSourceTable.Rows)
            {
                DataRow Drow = ElementInfoTable.NewRow();
                Drow[0] = UnmatchedRow[ColumnName].ToString();     // 1st column is for Unmatched element
                Drow[1] = string.Empty;                            // 2nd column is kept blank                              
                ElementInfoTable.Rows.Add(Drow);
            }
        }

        protected override void UpdateALLElementLogInfoTable()
        {
            // Update Log Info Table with mapped for each element (Indicator, Unit, Subgroup,Area)

            // 1.Update rows for Mapped Indicator in IndicatorInfoTable.
            foreach (MappedElementInfo MappedElement in this.RequiredDIElements[RequiredDIElementsType.Indicator].MappedElements.Values)
            {
                this.UpdateElementLogTable(ref this.IndicatorLogInfoTable, MappedElement, Indicator.IndicatorGId);
            }

            // 2.Update rows for Mapped Unit in UnitInfoTable.
            foreach (MappedElementInfo MappedElement in this.RequiredDIElements[RequiredDIElementsType.Unit].MappedElements.Values)
            {
                this.UpdateElementLogTable(ref this.UnitLogInfoTable, MappedElement, Unit.UnitGId);
            }

            // 3.Update rows for Mapped SubgroupVal in InfoTable.
            foreach (MappedElementInfo MappedElement in this.RequiredDIElements[RequiredDIElementsType.Subgroup].MappedElements.Values)
            {
                this.UpdateElementLogTable(ref this.SubgroupLogInfoTable, MappedElement, SubgroupVals.SubgroupValGId);
            }

            // 4.Update rows for Mapped Area in AreaInfoTable.
            foreach (MappedElementInfo MappedElement in this.RequiredDIElements[RequiredDIElementsType.Area].MappedElements.Values)
            {
                this.UpdateElementLogTable(ref this.AreaLogInfoTable, MappedElement, Area.AreaID);
            }
        }

        /// <summary>
        /// It updates the mapping of unmatched element with Mapped element in ElementLogTable used in HTML log.
        /// </summary>
        private void UpdateElementLogTable(ref DataTable elementLogTable, MappedElementInfo mappedElement, string columnName)
        {
            int RowPos = 0;
            foreach (DataRow Drow in elementLogTable.Rows)
            {
                if (Drow[0].ToString() == mappedElement.UnMatchedRow[columnName].ToString())
                {
                    elementLogTable.Rows[RowPos][1] = mappedElement.MatchedRow[columnName];
                    break;
                }
                RowPos++;
            }
        }





      
    }
    
}
