using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using System.IO;
using System.Threading;
using DAImportCommon = DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Log;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibBAL.CommonDelegates;



namespace DevInfo.Lib.DI_LibBAL.Import.DAImport
{

    # region " -- Delegate progress Bar -- "

    /// <summary>
    /// A delegate for ProgressBar_Increment event.
    /// </summary>
    public delegate void IncrementProgressBar(int value);

    /// <summary>
    /// A delegate for ProgressBar_Initialize event.
    /// </summary>
    /// <param name="maximumValue"></param>
    public delegate void InitializeProgressBar(int maximumValue);

    /// <summary>
    /// A delegate for PrograssBar_Close event
    /// </summary>
    public delegate void CloseProgressBar();

    # endregion

    #region "-- Delegates to display processing information --"

    /// <summary>
    /// A delegate for Process_Time event.
    /// </summary>
    /// <param name="timeLeft"></param>
    /// <param name="totalTimeRequired"></param>
    public delegate void ProcessingTimeDelegate(string timeLeft, string totalTimeRequired);

    /// <summary>
    /// A delegate for Process_SourceFileName event
    /// </summary>
    /// <param name="filename"></param>
    public delegate void CurrentSourceFileNameDelegate(string filename);

    /// <summary>
    /// A delegate for Process_Sheetname event
    /// </summary>
    /// <param name="sheetname"></param>
    public delegate void CurrentSheetNameDelegate(string sheetname);

    /// <summary>
    /// A delegate for Process_FileNo event
    /// </summary>
    /// <param name="current"></param>
    /// <param name="total"></param>
    public delegate void CurrentFileNoDelegate(int current, int total);


    /// <summary>
    /// A delegate for Process_FileOrSheetNo event
    /// </summary>
    /// <param name="current"></param>
    /// <param name="total"></param>
    public delegate void CurrentFileOrSheetNoDelegate(int current, int total);

    /// <summary>
    /// A delegate for Process_IndicatorName event
    /// </summary>
    /// <param name="name"></param>
    public delegate void CurrentIndicatorNameDelegate(string name);

    /// <summary>
    /// A delegate for Process_UnitName event
    /// </summary>
    /// <param name="name"></param>
    public delegate void CurrentUnitNameDelegate(string name);

    /// <summary>
    /// A delegate for Process_CurrentInfo event
    /// </summary>
    /// <param name="text"></param>
    public delegate void CurrentProcessingInfoDelegate(string text);

    /// <summary>
    /// A delegate for Process_HideSheetName event
    /// </summary>
    public delegate void HideSheetNameDelegate();

    /// <summary>
    /// A delegate for Process_HideSheetOrFileNo event
    /// </summary>
    public delegate void HideSheetOrFileNoDelegate();

    /// <summary>
    /// A delegate for Process_HideIndicatorName event
    /// </summary>
    public delegate void HideIndicatorNameDelegate();

    /// <summary>
    /// A delegate for Process_HideUnitName event
    /// </summary>
    public delegate void HideUnitNameDelegate();

    /// <summary>
    /// A delegate for Process_HideFileNo event
    /// </summary>
    public delegate void HideFileNoDelegate();

    /// <summary>
    /// A delegate for IUS Missed/Skipped to Insert into database
    /// </summary>
    public delegate void MissingIUSDetailsDelegate(string indicatorName, string unitName, string subgroupVal, string indicatorGID, string unitGID, string subgroupGId);

    #endregion


    /// <summary>
    /// This Abstract class used to import DataValue from Source Database (DES Spreadsheets , Excel Database) into another Database/Template. It Imports DataValue for those records where Indicator / Unit / Subgroup / Area is common between Source & Target Databases. 
    /// </summary>
    public abstract class DIData : IDisposable
    {

        #region "-- Private --"

        #region "-- Variables --"

        //OutPut Folder Path where Html log will be created.
        protected string _HtmlLogOutPutPath;

        // Start time of import process
        protected DateTime StartTime;

        #endregion

        #region "-- Methods --"



        #region "-- Raise Event mehtods --"

        /// <summary>
        /// To raise ProgressBar Increment Event
        /// </summary>
        protected void RaiseProgressBarIncrement(int value)
        {
            // -- Set ProgressBar Value If it is initialized.
            if (this.ProgressBar_Increment != null)
                this.ProgressBar_Increment(value);
        }


        protected void RaiseInitilizeProcessEvent(string currentProcess, int totalSheetCount, int maximumValue)
        {
            if (this.InitializeProcessEvent != null)
            {
                this.InitializeProcessEvent(currentProcess, totalSheetCount, maximumValue);
            }
        }

        protected void RaiseSetProcessEvent(string indicatorName, string unitName, int currentSheetNo)
        {
            if (this.SetProcessNameEvent != null)
            {
                this.SetProcessNameEvent(indicatorName, unitName, currentSheetNo);
            }
        }

        #endregion

        #region "-- Html Log Methods --"

        private string GetVersionDetails(DIConnection dbConnection)
        {
            string RetVal = string.Empty;

            try
            {
                if (dbConnection != null)
                {
                    RetVal = Convert.ToString(dbConnection.ExecuteScalarSqlQuery(DIQueries.GetDatabaseVersion()));
                }
            }
            catch (Exception)
            {
            }

            return RetVal;
        }

        /// <summary>
        /// Prepares all required elements for Html log creation , and Creates Html log file.
        /// </summary>
        protected virtual void CreateHTMLLog()
        {
            // 1. Update All Element Log tables with mapped values.\
            this.UpdateALLElementLogInfoTable();

            // 2. Close DB connection
            this._DBConnection.Dispose();

            // 3. Re-Create TargetDbConnection
            this._DBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess, string.Empty,
            string.Empty, this.TempTargetFile, string.Empty, DAImportCommon.Constants.DBPassword));

            // 4. Set required elements for Log
            int TotalTimePeriod = (int)this.DBConnection.ExecuteScalarSqlQuery(this.DAQuery.GetDistinctTimePeriodCount());
            int TotalSources = (int)this.DBConnection.ExecuteScalarSqlQuery(this.DAQuery.GetDistinctSourcesCount());
            int TotalData = (int)this.DBConnection.ExecuteDataTable(this.DAQuery.GetDataCount()).Rows[0][0];

            this.HtmlLog = new HTMLLog(this.ImportFileType, TotalTimePeriod, TotalSources, TotalData, this.IndicatorLogInfoTable, this.UnitLogInfoTable, this.SubgroupLogInfoTable, this.AreaLogInfoTable, this.DuplicateRecordsTable, this.SkippedRecordsTable, this.SkippedFiles);
            //this.HtmlLog.ImportType = this.ImportFileType;
            this.HtmlLog.StartTime = this.StartTime.ToString();
            this.HtmlLog.InvalidTimeperiodsTable = this.InvalidTimeperiodTable;
            this.HtmlLog.InvalidSourceTable = this.InvalidSourceTable;
            this.HtmlLog.SkippedAreaTable = this.AreaSkippedTable;
            this.HtmlLog.SkippedDataTable = this.DataSkippedTable;
            this.HtmlLog.SkippedSourceTable = this.SourceSkippedTable;
            this.HtmlLog.SkippedSubgroupTable = this.SubgroupSkippedTable;
            this.HtmlLog.IUSLogInfoTable = this.IUSLogInfoTable;

            if (this.DBConnection != null)
            {
                this.HtmlLog.TargetFileVersionNumber = this.GetVersionDetails(this.DBConnection);
            }

            // 5. Create Log
            this.HtmlLog.TargetFilePath = this.TargetFileNameForLogFile;
            this._HtmlLogFilePath = this.HtmlLog.CreateLog(this._OutputFileNameWPath, this.SourceFileNamesWPath, this._HtmlLogOutPutPath);
        }


        /// <summary>
        /// Updates Mapped elements values in all LogInfoTables.
        /// </summary>
        protected virtual void UpdateALLElementLogInfoTable()
        {
            // Update Log Info Table with mapped for each element (Indicator, Unit, Subgroup,Area)

            // 1.Update rows for Mapped Indicator in IndicatorInfoTable.
            foreach (MappedElementInfo MappedElement in this.RequiredDIElements[RequiredDIElementsType.Indicator].MappedElements.Values)
            {
                this.UpdateElementLogTable(ref this.IndicatorLogInfoTable, MappedElement, Indicator.IndicatorName);
            }

            // 2.Update rows for Mapped Unit in UnitInfoTable.
            foreach (MappedElementInfo MappedElement in this.RequiredDIElements[RequiredDIElementsType.Unit].MappedElements.Values)
            {
                this.UpdateElementLogTable(ref this.UnitLogInfoTable, MappedElement, Unit.UnitName);
            }

            // 3.Update rows for Mapped SubgroupVal in InfoTable.
            foreach (MappedElementInfo MappedElement in this.RequiredDIElements[RequiredDIElementsType.Subgroup].MappedElements.Values)
            {
                this.UpdateElementLogTable(ref this.SubgroupLogInfoTable, MappedElement, SubgroupVals.SubgroupVal);
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

        /// <summary>
        /// Sets Elemnet Log info Table for Indicator / Unit/ Subgroup/ Area
        /// Element Log info table will be used up during Html log creation.
        /// </summary>
        protected virtual void SetElementLogInfoTable()
        {
            string ColumnName = string.Empty;
            // 1. Set Indicator LogInfo Table 
            //Adding desired 2 columns for Indicator
            this.IndicatorLogInfoTable = new DataTable();
            this.IndicatorLogInfoTable.Columns.Add(DAImportCommon.Constants.Log.UnmatchedIndicatorColumnName);
            this.IndicatorLogInfoTable.Columns.Add(DAImportCommon.Constants.Log.MapIndicatorColumnName);
            // Add rows from unmatched element Table.
            this.FillElementLogInfoTable(this.RequiredDIElements[RequiredDIElementsType.Indicator].UnmatchedElementsTable, ref this.IndicatorLogInfoTable, Indicator.IndicatorName);

            // 2. Set Unit LogInfo Table.
            // Adding desired 2 columns for  Unit
            this.UnitLogInfoTable = new DataTable();
            this.UnitLogInfoTable.Columns.Add(DAImportCommon.Constants.Log.UnmatchedUnitColumnName);
            this.UnitLogInfoTable.Columns.Add(DAImportCommon.Constants.Log.MapUnitColumnName);
            // Add rows from unmatched element Table.
            this.FillElementLogInfoTable(this.RequiredDIElements[RequiredDIElementsType.Unit].UnmatchedElementsTable, ref this.UnitLogInfoTable, Unit.UnitName);

            // 3. Set Subgroup LogInfo Table.
            // Adding desired 2 columns  for  Subgroup
            this.SubgroupLogInfoTable = new DataTable();
            this.SubgroupLogInfoTable.Columns.Add(DAImportCommon.Constants.Log.UnmatchedSubgroupValColumnName);
            this.SubgroupLogInfoTable.Columns.Add(DAImportCommon.Constants.Log.MapSubgroupValColumnName);
            // Add rows from unmatched element Table.
            this.FillElementLogInfoTable(this.RequiredDIElements[RequiredDIElementsType.Subgroup].UnmatchedElementsTable, ref this.SubgroupLogInfoTable, SubgroupVals.SubgroupVal);

            // 4. Set Area LogInfo Table.
            // Adding desired 2 columns  for  Area
            this.AreaLogInfoTable = new DataTable();
            this.AreaLogInfoTable.Columns.Add(DAImportCommon.Constants.Log.UnmatchedAreaColumnName);
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


        #endregion


        #region "-- Update Nids of matched elements --"

        /// <summary>
        /// Updates Indicator_Nid, Unit_Nid, SubGroup_Nid, Area_Nid,  Map_IUSNids of Temp_Data table where GIDs or Names Matched with existing GIDs. 
        /// </summary>
        private void UpdateNidsInTempDataTable()
        {
            try
            {
                // ****** Update NID For Matched GIDs of I, U, S
                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateNIDForIndicatorGID());
                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateNIDForUnitGID());
                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateNIDForSubgroupGID());

                // ******* Update Nids where I, U, S, Area - Names matches:

                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateNIDForIndicatorName());   // Update Nid where Indicator Name matches.
                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateNIDForUnitName());     // Update Nid where unit Name matches.
                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateNIDForSubgroupValName());     // Update Nid where Subgroup Name matches.
                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateNIDForAreaNId());     // Update Nid where Area Name matches.

                // **************************
                //  Update TempData.IUSNid where Indicator_Nid, Unit_Nid, SubGroupVal_Nid matched in "Indicator_Unit_Subgroup" Table
                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateIUSNidofMatchedRecords());

                //****Unmatched IUS NId update
                //Insert rows into TempIUSBlank table where I/ U/ S  are blank.
                this.DBConnection.ExecuteNonQuery(this.DAQuery.InsertBlankI_U_SIntoTempIUSBlankTable());

                // Insert insert rows from TempDataTable into tempUnmatchedIUSTable where Indicator or unit or subgroup are blank.
                // Columns in new TempTable inserted are: (-)Data_NId AS UnmatchedIUSNid, Indicator, Unit, Subgroup
                if (this.ImportFileType == DIImportFileType.SDMXXml)
                {
                    // In SDMX, matching is done on GID basis, NOT on Names
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.InsertBlankI_U_SIntoTempUnmatchedIUSTable(true));

                    // Update unmatched IUSNid = - DataNid , where any of IndicatorNid, unitNid, SubgroupNid is NULL.  
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateUnmatchedIUSNidFromTempUnmatchedIUSTable(true));

                }
                else
                {
                    // In Spreadsheet, matching is done on Names, NOT on GID as GID might not be prersent
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.InsertBlankI_U_SIntoTempUnmatchedIUSTable(false));


                    // Update unmatched IUSNid = - DataNid , where any of IndicatorNid, unitNid, SubgroupNid is NULL.  
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateUnmatchedIUSNidFromTempUnmatchedIUSTable(false));
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        # endregion

        #region "-- Post Mapping Operation --

        private void InsertNewTimePeriods()
        {
            try
            {
                // Insert New TimePeriods from TempSheet table into TimePeriod Table
                this.DBConnection.ExecuteNonQuery(DAQuery.InsertNewTimePeriods());

                // 2. Update TimePeriod Nid in Temp_Data. 
                this.DBConnection.ExecuteNonQuery(DAQuery.UpdateTimePeriodNid());

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }

        private void InsertNewSources()
        {
            String SqlQuery = string.Empty;
            DataTable NewSourcesTable = new DataTable();
            DataTable SourceTempTable;
            DataTable TempTable;
            string SourceNID = string.Empty;
            string IUSNID = string.Empty;
            IndicatorClassificationBuilder ICBuilder;




            //DIDatabase Database = new DIDatabase();
            SourceBuilder DISource = new SourceBuilder(this.DBConnection, this.DBQueries);
            try
            {

                // 1. Update special quotes in sources 

                // as replace function does not work with ADO.net so first get the special foonote with quotes and then update source text one by one.
                if (this.DBConnection.ConnectionStringParameters.ServerType == DIServerType.MsAccess)
                {
                    SourceTempTable = this.DBConnection.ExecuteDataTable(this.DAQuery.GetSourcesWSpecialQuotesFrmTempDataTbl());

                    //update source text
                    foreach (DataRow Row in SourceTempTable.Rows)
                    {
                        this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateSpecialQuotesInSourceText(Convert.ToString(Row[DevInfo.Lib.DI_LibBAL.Import.DAImport.Common.Constants.SourceColumnName]), Convert.ToString(Row[Data.DataNId])));
                    }
                }

                //// 2. Get Distinct Sources from Temp_Data which are not in UT_Indicator_Classification
                //NewSourcesTable = this.DBConnection.ExecuteDataTable(this.DAQuery.GetDistinctSourcesFromTempData());

                // 1. Get Distinct Sources from Temp_Data 
                NewSourcesTable = this.DBConnection.ExecuteDataTable(this.DAQuery.GetAllSourceFrmTempDataTable()).DefaultView.ToTable(true, DAImportCommon.Constants.SourceColumnName);

                // 2. Insert Sources into UT_Indicator_Classification.
                foreach (DataRow SourceRow in NewSourcesTable.Rows)
                {

                    DISource.CheckNCreateSource(DICommon.RemoveQuotes(SourceRow[DAImportCommon.Constants.SourceColumnName].ToString()));
                }

                // 3. Update Source_Nid again in Temp_Data. 
                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateSourceNidInTempData());

                // 4. Update IC_Global in target database
                this.UpdateICGlobal();

                //  Insert IC_Nid & IUSNid in "Indicator_Classification_IUS" Table 
                //this.DBConnection.ExecuteNonQuery(this.DAQuery.InsertRecordInIndicator_Classification_IUS());

                // 5. insert IC_NID and IUSNID relationship in Indicator_Classification_IUS" table
                TempTable = this.DBConnection.ExecuteDataTable(this.DAQuery.GetNewSourcesNIUSNID());

                if (TempTable != null)
                {
                    ICBuilder = new IndicatorClassificationBuilder(this.DBConnection, this.DBQueries);

                    foreach (DataRow Row in TempTable.Rows)
                    {
                        SourceNID = Convert.ToString(Row[Data.SourceNId]);
                        IUSNID = Convert.ToString(Row[DevInfo.Lib.DI_LibBAL.Import.DAImport.Common.Constants.NewIUSColumnName]);

                        if (!string.IsNullOrEmpty(SourceNID) && !string.IsNullOrEmpty(IUSNID))
                        {
                            ICBuilder.AddNUpdateICIUSRelationUptoRootlevel(Convert.ToInt32(SourceNID), Convert.ToInt32(IUSNID), false);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void UpdateICGlobal()
        {
            string LanguageCode = string.Empty;
            DITables Tablenames = null;
            try
            {
                foreach (DataRow LanguageRow in this._DBConnection.DILanguages(this.DatasetPrefix).Rows)
                {
                    LanguageCode = "_" + LanguageRow[Language.LanguageCode].ToString();
                    Tablenames = new DITables(this.DatasetPrefix, LanguageCode);
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateICGlobal(Tablenames.IndicatorClassifications));
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void InsertNewFootNotes()
        {
            String SqlQuery = string.Empty;
            DataTable NewFootnotes;
            DataTable TempFootnotesTable;
            FootnoteBuilder DIFootNote;
            FootnoteInfo DIFootNoteInfo;
            int NewNid;
            Dictionary<string, int> InsertedFootnotes = new Dictionary<string, int>();
            string FootnoteText = string.Empty;
            try
            {
                // 1. Update special quotes in FootNotes 

                // as replace function does not work with ADO.net so first get the special foonote with quotes and then update footnote text one by one.
                if (this.DBConnection.ConnectionStringParameters.ServerType == DIServerType.MsAccess)
                {
                    TempFootnotesTable = this.DBConnection.ExecuteDataTable(this.DAQuery.GetFootnoteTextWSpecialQuotesFrmTempDataTbl());

                    //update footnote text
                    foreach (DataRow Row in TempFootnotesTable.Rows)
                    {
                        this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateSpecialInFootnoteText(Convert.ToString(Row[FootNotes.FootNote]), Convert.ToString(Row[Data.DataNId])));
                    }
                }
                //this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateFootNoteNid());

                // 2. Get new FootNotes from Temp_Data table which are not in UT_FootNotes Table.
                //NewFootnotes = this.DBConnection.ExecuteDataTable(this.DAQuery.GetNewFootNotes());
                NewFootnotes = this.DBConnection.ExecuteDataTable(this.DAQuery.GetFootNotesFrmTempDataTable()).DefaultView.ToTable(true, DAImportCommon.Constants.FootNoteColumnName);

                // 3. Insert FootNotes from TempData Table into UT_FootNote
                DIFootNote = new FootnoteBuilder(this.DBConnection, this.DBQueries);
                foreach (DataRow SourceRow in NewFootnotes.Rows)
                {
                    FootnoteText = Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(Convert.ToString(SourceRow[DAImportCommon.Constants.FootNoteColumnName]));

                    if (!InsertedFootnotes.ContainsKey(FootnoteText) && !string.IsNullOrEmpty(FootnoteText))
                    {
                        DIFootNoteInfo = new FootnoteInfo();
                        DIFootNoteInfo.Name = FootnoteText;
                        NewNid = DIFootNote.CheckNCreateFoonote(DIFootNoteInfo);

                        if (NewNid > 0)
                        {
                            InsertedFootnotes.Add(FootnoteText, NewNid);

                            // 4. Update FootNotes Nid in Temp_Data. 
                            this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateFootNoteNid(NewNid, FootnoteText));
                        }
                    }
                }

                //////// 4. Update FootNotes Nid in Temp_Data. 
                //////this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateFootNoteNid());

                // 5. Update FootNote Nid = -1 where FootNote is blank.
                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateNidForEmptyFootNote());
            }
            catch (Exception ex)
            {

                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        private void InsertRecommendedSources()
        {
            String SqlQuery = string.Empty;
            string ICIUSLabel = string.Empty;
            string IUSNId = string.Empty;
            string TimeperiodNId = string.Empty;
            string SourceNId = string.Empty;
            string AreaNId = string.Empty;
            string ICIUSOrder = string.Empty;

            int DataNId = -1;
            DataTable TempTable;

            RecommendedSourcesBuilder RecommendedSrcBuilder;


            try
            {
                RecommendedSrcBuilder = new RecommendedSourcesBuilder(this.DBConnection, this.DBQueries);

                // 1. Get IC_IUS_Label from Temp_Data table " Select * from Temp_data table where IC_IUS_Label<> null or IC_IUS_Label<> ''"
                TempTable = this.DBConnection.ExecuteDataTable("Select * from " + DAImportCommon.Constants.TempDataTableName + "  where " + RecommendedSources.ICIUSLabel + "<> null or " + RecommendedSources.ICIUSLabel + "<> '' or " + Data.ICIUSOrder + " <>null");

                // 2. Insert recommendedsources(IC_IUS_Label) from TempData Table into UT_REcommendedSources_en
                RecommendedSrcBuilder = new RecommendedSourcesBuilder(this.DBConnection, this.DBQueries);
                foreach (DataRow Row in TempTable.Rows)
                {
                    ICIUSLabel = Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(Convert.ToString(Row[RecommendedSources.ICIUSLabel]));
                    IUSNId = Convert.ToString(Row[DevInfo.Lib.DI_LibBAL.Import.DAImport.Common.Constants.NewIUSColumnName]);
                    TimeperiodNId = Convert.ToString(Row[Data.TimePeriodNId]);
                    AreaNId = Convert.ToString(Row[Data.AreaNId]);
                    SourceNId = Convert.ToString(Row[Data.SourceNId]);
                    ICIUSOrder = Convert.ToString(Row[Data.ICIUSOrder]);



                    // get new dataNid by IUSAST
                    DataNId = Convert.ToInt32(this.DBConnection.ExecuteScalarSqlQuery(this.DBQueries.Data.GetDataNIdByIUSTimePeriodAreaSource(IUSNId, TimeperiodNId, AreaNId, SourceNId)));
                    if (DataNId > 0)
                    {
                        // insert or update recommendedSource
                        RecommendedSrcBuilder.CheckNInsertRecommendedSource(DataNId, ICIUSOrder, ICIUSLabel);
                    }

                }

            }
            catch (Exception ex)
            {

                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        #endregion

        /// <summary>
        /// Copies all Source Files (Spreadsheets or Databases) into Specified temp folder
        /// </summary>
        private void CopyFilesIntoTempFldr()
        {
            string TempFileNameWPath = string.Empty;


            this.TempFiles = new List<string>();

            foreach (string SourceDESFileNameWPath in this._SourceFileNamesWPath)
            {
                TempFileNameWPath = this.TempFolderPath + "\\" + Path.GetFileName(SourceDESFileNameWPath);
                try
                {
                    if (File.Exists(TempFileNameWPath))
                    {
                        File.Delete(TempFileNameWPath);
                    }
                    File.Copy(SourceDESFileNameWPath, TempFileNameWPath);
                    this.TempFiles.Add(TempFileNameWPath);

                    if (Path.GetExtension(TempFileNameWPath) == DICommon.FileExtension.Database || Path.GetExtension(TempFileNameWPath) == DICommon.FileExtension.Template)   //tpl/mdb
                    {
                        if (this.ImportFileType == DIImportFileType.Database || this.ImportFileType == DIImportFileType.Template)
                        {
                            // handling for DI7 data value columns(textual and numeric data value columns)
                            DIDataValueHelper.MergeTextualandNumericDataValueColumn(TempFileNameWPath);

                        }
                    }
                }
                catch (Exception ex)
                {
                    ExceptionFacade.ThrowException(ex);
                }
            }

        }


        /// <summary>
        /// It initializes List (DiRequiredelements)  for each 5 reguired element (I, U, S, IUS, A)
        /// </summary>
        private void ProcessRequiredDIElements()
        {
            this._RequiredDIElements = new Dictionary<RequiredDIElementsType, UnmatchedElementsInfo>();
            this._RequiredDIElements.Add(RequiredDIElementsType.Indicator, new UnmatchedElementsInfo());
            this._RequiredDIElements.Add(RequiredDIElementsType.Area, new UnmatchedElementsInfo());
            this._RequiredDIElements.Add(RequiredDIElementsType.Unit, new UnmatchedElementsInfo());
            this._RequiredDIElements.Add(RequiredDIElementsType.Subgroup, new UnmatchedElementsInfo());
            this._RequiredDIElements.Add(RequiredDIElementsType.IUS, new UnmatchedElementsInfo());
        }

        /// <summary>
        /// Gets and return DataTable having availale Indicators in Target Database.
        /// </summary>
        private DataTable GetAvailableIndicator()
        {
            DataTable RetVal = null;
            try
            {
                string SqlQuery = string.Empty;
                SqlQuery = _DBQueries.Indicators.GetIndicator(FilterFieldType.None, string.Empty, FieldSelection.Light);
                RetVal = this._DBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
                RetVal = null;
            }

            return RetVal;
        }

        /// <summary>
        /// Gets and return DataTable having availale Areas(Name, ID, Level) from Target Database.
        /// </summary>
        private DataTable GetAvailableArea()
        {
            DataTable RetVal = null;
            try
            {
                string SqlQuery = string.Empty;
                SqlQuery = _DBQueries.Area.GetArea(FilterFieldType.None, string.Empty, DevInfo.Lib.DI_LibDAL.Queries.Area.Select.OrderBy.AreaId);
                RetVal = this._DBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
                RetVal = null;
            }

            return RetVal;
        }

        /// <summary>
        /// Gets and return DataTable having availale Unit(Name, GID) from Target Database.
        /// </summary>
        private DataTable GetAvailableUnit()
        {
            DataTable RetVal = null;
            try
            {
                string SqlQuery = string.Empty;
                SqlQuery = _DBQueries.Unit.GetUnit(FilterFieldType.None, "");
                RetVal = this._DBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
                RetVal = null;
            }

            return RetVal;
        }

        /// <summary>
        /// Gets and return DataTable having availale Subgroup(Name, GDI) from Target Database.
        /// </summary>
        private DataTable GetAvailableSubgroup()
        {
            DataTable RetVal = null;
            try
            {
                string SqlQuery = string.Empty;
                SqlQuery = _DBQueries.Subgroup.GetSubgroupVals(FilterFieldType.None, string.Empty);
                RetVal = this._DBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
                RetVal = null;
            }

            return RetVal;
        }

        /// <summary>
        /// Gets and return DataTable having availale IUS(IUSNId, IndNid, UnitNid, Subg Nid) from Target Database.
        /// </summary>
        private DataTable GetAvailableIUS()
        {
            DataTable RetVal = null;
            string SqlQuery = string.Empty;
            try
            {
                SqlQuery = this._DBQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.Light);
                RetVal = this._DBConnection.ExecuteDataTable(SqlQuery);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
                RetVal = null;
            }

            return RetVal;
        }

        #region "-- Methods for Mapping external mapped Element xml file--"

        private void ProcessAutoIndicatorMappingInfo(DataTable xmlMappedIndicatorTable)
        {
            string UnmatchedGID = string.Empty;
            string UnmatchedName = string.Empty;
            string MappedGID = string.Empty;
            DataRow[] UnmatchedRows;
            DataRow[] AvailableRows;
            string MappedElementKey = string.Empty;
            DataRow UnmatchedRow;

            foreach (DataRow XmlMappedRow in xmlMappedIndicatorTable.Rows)
            {
                try
                {
                    //Step1: get unmatched gid OR name
                    if (XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_IND_GID] != null &
                        !string.IsNullOrEmpty(XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_IND_GID].ToString()))
                    {
                        UnmatchedGID = XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_IND_GID].ToString();
                        //Step2: check the existence of unmatched gid in unmatchedIndicator table
                        UnmatchedRows = this._RequiredDIElements[RequiredDIElementsType.Indicator].UnmatchedElementsTable.Select(Indicator.IndicatorGId + "='" + DICommon.RemoveQuotes(UnmatchedGID) + "'");
                    }
                    else
                    {
                        //Do only for spreadsheets
                        //Step2: check the existence of unmatched Name in unmatchedIndicator table
                        if (this.ImportFileType == DIImportFileType.DataEntrySpreasheet)
                        {
                            UnmatchedName = XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_IND].ToString();

                            UnmatchedRows = this._RequiredDIElements[RequiredDIElementsType.Indicator].UnmatchedElementsTable.Select(Indicator.IndicatorName + "='" + DICommon.RemoveQuotes(UnmatchedName) + "'");
                        }
                        else
                        {
                            //Add 0 rows in UnmatchedRows
                            UnmatchedRows = this._RequiredDIElements[RequiredDIElementsType.Indicator].UnmatchedElementsTable.Select("1=0");
                        }
                    }
                    if (UnmatchedRows.Length > 0)
                    {
                        //Step3: Get mapping information from xml mapped table
                        MappedGID = XmlMappedRow[Indicator.IndicatorGId].ToString();

                        //Step4 : Check mapping information in available table
                        AvailableRows = this._RequiredDIElements[RequiredDIElementsType.Indicator].AvailableElementsTable.Select(Indicator.IndicatorGId + "='" + DICommon.RemoveQuotes(MappedGID) + "'");
                        if (AvailableRows.Length > 0)
                        {
                            UnmatchedRow = this._RequiredDIElements[RequiredDIElementsType.Indicator].UnmatchedElementsTable.NewRow();
                            UnmatchedRow.ItemArray = UnmatchedRows[0].ItemArray;

                            //Step 5: save it into 
                            // MappedElements  of requiredDIElements
                            if (this.ImportFileType == DIImportFileType.DataEntrySpreasheet)
                            {
                                MappedElementKey = UnmatchedName;
                            }
                            else
                            {
                                MappedElementKey = UnmatchedGID;
                            }

                            this._RequiredDIElements[RequiredDIElementsType.Indicator].MappedElements.Add(MappedElementKey, new MappedElementInfo(UnmatchedRow, AvailableRows[0]));

                            //Step 6 : Remove inserted unmatched row from unmatched elements table
                            this._RequiredDIElements[RequiredDIElementsType.Indicator].UnmatchedElementsTable.Rows.Remove(UnmatchedRows[0]);
                            this._RequiredDIElements[RequiredDIElementsType.Indicator].UnmatchedElementsTable.AcceptChanges();
                        }
                    }

                }
                catch (Exception ex)
                {
                    ExceptionFacade.ThrowException(ex);
                }
            }
        }


        private void ProcessAutoUnitMappingInfo(DataTable xmlMappedUnitTable)
        {
            string UnmatchedGID = string.Empty;
            string UnmatchedName = string.Empty;
            string MappedGID = string.Empty;
            DataRow[] UnmatchedRows;
            DataRow[] AvailableRows;
            string MappedElementKey = string.Empty;
            DataRow UnmatchedRow = null;

            foreach (DataRow XmlMappedRow in xmlMappedUnitTable.Rows)
            {
                try
                {
                    //Step1: get unmatched gid OR name
                    if (XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_UNIT_GID] != null &
                        !string.IsNullOrEmpty(XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_UNIT_GID].ToString()))
                    {
                        UnmatchedGID = XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_UNIT_GID].ToString();
                        //Step2: check the existence of unmatched gid in unmatchedUnit table
                        UnmatchedRows = this._RequiredDIElements[RequiredDIElementsType.Unit].UnmatchedElementsTable.Select(Unit.UnitGId + "='" + DICommon.RemoveQuotes(UnmatchedGID) + "'");
                    }
                    else
                    {
                        //Do only for spreadsheets
                        //Step2: check the existence of unmatched Name in unmatchedUnit table
                        if (this.ImportFileType == DIImportFileType.DataEntrySpreasheet)
                        {
                            UnmatchedName = XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_UNIT].ToString();

                            UnmatchedRows = this._RequiredDIElements[RequiredDIElementsType.Unit].UnmatchedElementsTable.Select(Unit.UnitName + "='" + DICommon.RemoveQuotes(UnmatchedName) + "'");
                        }
                        else
                        {
                            //Add 0 rows in UnmatchedRows
                            UnmatchedRows = this._RequiredDIElements[RequiredDIElementsType.Unit].UnmatchedElementsTable.Select("1=0");
                        }
                    }
                    if (UnmatchedRows.Length > 0)
                    {
                        //Step3: Get mapping information from xml mapped table
                        MappedGID = XmlMappedRow[Unit.UnitGId].ToString();

                        //Step4 : Check mapping information in available table
                        AvailableRows = this._RequiredDIElements[RequiredDIElementsType.Unit].AvailableElementsTable.Select(Unit.UnitGId + "='" + DICommon.RemoveQuotes(MappedGID) + "'");
                        if (AvailableRows.Length > 0)
                        {
                            UnmatchedRow = this._RequiredDIElements[RequiredDIElementsType.Unit].UnmatchedElementsTable.NewRow();
                            UnmatchedRow.ItemArray = UnmatchedRows[0].ItemArray;

                            //Step 5: save it into 
                            // MappedElements  of requiredDIElements
                            if (this.ImportFileType == DIImportFileType.DataEntrySpreasheet)
                            {
                                MappedElementKey = UnmatchedName;
                            }
                            else
                            {
                                MappedElementKey = UnmatchedGID;
                            }
                            this._RequiredDIElements[RequiredDIElementsType.Unit].MappedElements.Add(MappedElementKey, new MappedElementInfo(UnmatchedRow, AvailableRows[0]));

                            //Step 6 : Remove inserted unmatched row from unmatched elements table
                            this._RequiredDIElements[RequiredDIElementsType.Unit].UnmatchedElementsTable.Rows.Remove(UnmatchedRows[0]);
                            this._RequiredDIElements[RequiredDIElementsType.Unit].UnmatchedElementsTable.AcceptChanges();
                        }
                    }

                }
                catch (Exception ex)
                {
                    ExceptionFacade.ThrowException(ex);
                }
            }
        }

        private void ProcessAutoSubgroupMappingInfo(DataTable xmlMappedSubgroupTable)
        {
            string UnmatchedGID = string.Empty;
            string UnmatchedName = string.Empty;
            string MappedGID = string.Empty;
            DataRow[] UnmatchedRows;
            DataRow[] AvailableRows;
            string MappedElementKey = string.Empty;
            DataRow UnmatchedRow;

            foreach (DataRow XmlMappedRow in xmlMappedSubgroupTable.Rows)
            {
                try
                {
                    //Step1: get unmatched gid OR name
                    if (XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_SUBGROUP_GID] != null &
                        !string.IsNullOrEmpty(XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_SUBGROUP_GID].ToString()))
                    {
                        UnmatchedGID = XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_SUBGROUP_GID].ToString();
                        //Step2: check the existence of unmatched gid in unmatchedSubgroup table
                        UnmatchedRows = this._RequiredDIElements[RequiredDIElementsType.Subgroup].UnmatchedElementsTable.Select(SubgroupVals.SubgroupValGId + "='" + DICommon.RemoveQuotes(UnmatchedGID) + "'");
                    }
                    else
                    {
                        //Do only for spreadsheets
                        //Step2: check the existence of unmatched Name in unmatchedSubgroup table
                        if (this.ImportFileType == DIImportFileType.DataEntrySpreasheet)
                        {
                            UnmatchedName = XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_SUBGROUP].ToString();

                            UnmatchedRows = this._RequiredDIElements[RequiredDIElementsType.Subgroup].UnmatchedElementsTable.Select(SubgroupVals.SubgroupVal + "='" + DICommon.RemoveQuotes(UnmatchedName) + "'");
                        }
                        else
                        {
                            //Add 0 rows in UnmatchedRows
                            UnmatchedRows = this._RequiredDIElements[RequiredDIElementsType.Subgroup].UnmatchedElementsTable.Select("1=0");
                        }
                    }
                    if (UnmatchedRows.Length > 0)
                    {
                        //Step3: Get mapping information from xml mapped table
                        MappedGID = XmlMappedRow[Subgroup.SubgroupGId].ToString();

                        //Step4 : Check mapping information in available table
                        AvailableRows = this._RequiredDIElements[RequiredDIElementsType.Subgroup].AvailableElementsTable.Select(SubgroupVals.SubgroupValGId + "='" + DICommon.RemoveQuotes(MappedGID) + "'");
                        if (AvailableRows.Length > 0)
                        {
                            UnmatchedRow = this._RequiredDIElements[RequiredDIElementsType.Subgroup].UnmatchedElementsTable.NewRow();
                            UnmatchedRow.ItemArray = UnmatchedRows[0].ItemArray;

                            //Step 5: save it into 
                            // MappedElements  of requiredDIElements
                            if (this.ImportFileType == DIImportFileType.DataEntrySpreasheet)
                            {
                                MappedElementKey = UnmatchedName;
                            }
                            else
                            {
                                MappedElementKey = UnmatchedGID;
                            }
                            this._RequiredDIElements[RequiredDIElementsType.Subgroup].MappedElements.Add(MappedElementKey, new MappedElementInfo(UnmatchedRow, AvailableRows[0]));

                            //Step 6 : Remove inserted unmatched row from unmatched elements table
                            this._RequiredDIElements[RequiredDIElementsType.Subgroup].UnmatchedElementsTable.Rows.Remove(UnmatchedRows[0]);
                            this._RequiredDIElements[RequiredDIElementsType.Subgroup].UnmatchedElementsTable.AcceptChanges();
                        }
                    }

                }

                catch (Exception ex)
                {
                    ExceptionFacade.ThrowException(ex);
                }
            }
        }

        private void ProcessAutoAreaMappingInfo(DataTable xmlMappedAreaTable)
        {
            string UnmatchedAreaID = string.Empty;
            string UnmatchedName = string.Empty;
            string MappedAreaID = string.Empty;
            DataRow[] UnmatchedRows;
            DataRow[] AvailableRows;
            string MappedElementKey = string.Empty;
            DataRow UnmatchedRow;

            foreach (DataRow XmlMappedRow in xmlMappedAreaTable.Rows)
            {
                try
                {
                    //Step1: get unmatched AreaID
                    if (XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_AERA_ID] != null &
                        !string.IsNullOrEmpty(XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_AERA_ID].ToString()))
                    {
                        UnmatchedAreaID = XmlMappedRow[MappedInfoGenerator.Constants.COL_UNMATCHED_AERA_ID].ToString();
                        //Step2: check the existence of unmatched AreaID in unmatchedArea table
                        UnmatchedRows = this._RequiredDIElements[RequiredDIElementsType.Area].UnmatchedElementsTable.Select(Area.AreaID + "='" + DICommon.RemoveQuotes(UnmatchedAreaID) + "'");
                    }
                    else
                    {
                        //Add 0 rows in UnmatchedRows
                        UnmatchedRows = this._RequiredDIElements[RequiredDIElementsType.Area].UnmatchedElementsTable.Select("1=0");
                    }
                    if (UnmatchedRows.Length > 0)
                    {
                        //Step3: Get mapping information from xml mapped table
                        MappedAreaID = XmlMappedRow[Area.AreaID].ToString();

                        //Step4 : Check mapping information in available table
                        AvailableRows = this._RequiredDIElements[RequiredDIElementsType.Area].AvailableElementsTable.Select(Area.AreaID + "='" + DICommon.RemoveQuotes(MappedAreaID) + "'");
                        if (AvailableRows.Length > 0)
                        {
                            UnmatchedRow = this._RequiredDIElements[RequiredDIElementsType.Area].UnmatchedElementsTable.NewRow();
                            UnmatchedRow.ItemArray = UnmatchedRows[0].ItemArray;

                            //Step 5: save it into 
                            // MappedElements  of requiredDIElements
                            MappedElementKey = UnmatchedAreaID;

                            this._RequiredDIElements[RequiredDIElementsType.Area].MappedElements.Add(MappedElementKey, new MappedElementInfo(UnmatchedRow, AvailableRows[0]));

                            //Step 6 : Remove inserted unmatched row from unmatched elements table
                            this._RequiredDIElements[RequiredDIElementsType.Area].UnmatchedElementsTable.Rows.Remove(UnmatchedRows[0]);
                            this._RequiredDIElements[RequiredDIElementsType.Area].UnmatchedElementsTable.AcceptChanges();
                        }
                    }

                }

                catch (Exception ex)
                {
                    ExceptionFacade.ThrowException(ex);
                }
            }
        }


        #endregion

        /// <summary>
        /// Deletes Duplicate records from TempDataTable.
        /// </summary>
        private void DeleteDuplicateRecords()
        {
            // Insert Distinct Data_Nid of Dupicate Records into NIDTemp Table
            this.DBConnection.ExecuteNonQuery(this.DAQuery.InsertDuplicateRecordNidInTemp());

            // Insert Distinct rows of Duplicate records into DuplicateTemp Table, after matching Data_nid in NIDTemp
            this.DBConnection.ExecuteNonQuery(this.DAQuery.InsertDuplicateRecordInTemp());

            // Update New_IUSNid = -1 where Duplicate Records were found. 
            this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateIUSNidOfDuplicateRecord());

            // Get duplicate records from TempdataTable wherer New_IUSNID=-1
            this.DuplicateRecordsTable = this.DBConnection.ExecuteDataTable("Select DISTINCT sourceFile, Indicator_Name, Unit_Name, Subgroup_Val, TimePeriod, Area_ID, Source, Indicator_GId, Unit_GId, Subgroup_Val_GId from TempDataTable WHERE NEW_IUSNID=-1  Order by Indicator_Name, Unit_Name, Subgroup_Val, TimePeriod, Area_ID, Source");

            // Remove Duplicate Record from TempDataTable. i.e where New_IUSnid  = -1
            this.DBConnection.ExecuteNonQuery(this.DAQuery.DeleteDuplicateRecordsFromTempDataTable());

            // Insert Data back from DuplicateTemp Table into TempDataTable
            this.DBConnection.ExecuteNonQuery(this.DAQuery.InsertFromDuplicateTempIntoTempData());

            // Drop Temp tables used .
            this.DBConnection.ExecuteNonQuery(this.DAQuery.DropTempNIDTableOfDuplcate());
            this.DBConnection.ExecuteNonQuery(this.DAQuery.DropTempTableOfDuplcate());

        }

        /// <summary>
        /// And Inserts new TimePeriod, FootNotes, Sources in corresponding Tables.
        /// Then updates newly created NIDs in Temp_Data table.
        /// </summary>
        private void InsertTimePeriodSourceFootNotes()
        {
            try
            {
                // Insert New TimePeriod from Temp_Data into UT_TimePeriod, and update NID for same record.
                this.RaiseProcessCurrentInfoEvent("Processing Timeperiod");
                this.InsertNewTimePeriods();

                // Insert New FootNotes from Temp_Data into UT_FootNotes and update NID for same records.
                this.RaiseProcessCurrentInfoEvent("Processing footnotes");
                this.InsertNewFootNotes();

                // Insert New Sources from Temp_Data into UT_Indicator_Classifications and update NID for same records.
                this.RaiseProcessCurrentInfoEvent("Processing sources");
                this.InsertNewSources();

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void UpdateIndicatorUnitSubgroupNidsInData()
        {
            string Query = string.Empty;

            try
            {
                Query = DevInfo.Lib.DI_LibDAL.Queries.Data.Update.UpdateIndicatorUnitSubgroupVAlNids(this._DBQueries.TablesName);
                this._DBConnection.ExecuteNonQuery(Query);
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void DropTempTables()
        {
            try
            {
                // Delete Temp Data table
                this.DBConnection.ExecuteNonQuery(this.DAQuery.DropTempDataTable());
            }
            catch (Exception)
            {
                // Do nothing
            }

            try
            {
                this.DBConnection.ExecuteNonQuery(this.DAQuery.DropTempUnmatchedIUSTable());

                this.DBConnection.ExecuteNonQuery(this.DAQuery.DropTempIUSBlankTable());
            }
            catch (Exception)
            {
            }
        }


        #endregion

        #endregion

        #region "-- Protected --"

        #region "-- Variables --"

        protected string DatasetPrefix = string.Empty;      // Prefix for Target Database tables.
        protected string LanguageCode = string.Empty;      // Langusge code 
        protected List<string> TempFiles;                 // List of TempFile as SourcFiles in Temp folder
        protected bool IsForOnlineDatabase = false;

        #region "-- Log DataTables --"


        protected DataTable IndicatorLogInfoTable = null;
        protected DataTable UnitLogInfoTable = null;
        protected DataTable SubgroupLogInfoTable = null;
        protected DataTable AreaLogInfoTable = null;
        protected DataTable SkippedRecordsTable = null;
        protected DataTable DuplicateRecordsTable = null;
        protected DataTable InvalidTimeperiodTable = null;
        protected DataTable InvalidSourceTable = null;
        protected DataTable DataSkippedTable = null;
        protected DataTable AreaSkippedTable = null;
        protected DataTable SourceSkippedTable = null;
        protected DataTable SubgroupSkippedTable = null;
        protected DataTable IUSLogInfoTable = null;     //- to be used in CSV Import
        protected static DataTable SkippedIUSTable = null;
        // Stores list of skipped files in case of invalid spreadsheet or database.
        protected DataTable SkippedFiles;

        #endregion

        #endregion

        #region "-- Methods --"

        #region "-- Raise Event mehtods --"

        /// <summary>
        /// To raise Process_Time event
        /// </summary>
        protected void RaiseProcessTimeEvent(string timeLeft, string totalTimeRequired)
        {
            if (this.Process_Time != null)
                this.Process_Time(timeLeft, totalTimeRequired);
        }

        /// <summary>
        /// To raise Process_CurrentInfo event
        /// </summary>
        protected void HideCommonProcessInfo()
        {
            if (this.Process_CurrentInfo != null)
                this.Process_CurrentInfo(string.Empty);
        }

        /// <summary>
        /// To hide sheet name information 
        /// </summary>
        protected void RaiseHideSheetNameEvent()
        {
            if (this.Process_HideSheetName != null)
                this.Process_HideSheetName();
        }

        /// <summary>
        /// To hide sheet or file no information
        /// </summary>
        protected void RaiseHideSheetOrFileNoEvent()
        {
            if (this.Process_HideSheetOrFileNo != null)
                this.Process_HideSheetOrFileNo();
        }

        /// <summary>
        /// To raise Process_HideIndicatorName event which will hide the indicator name form processing information
        /// </summary>
        protected void RaiseHideIndicatorNameEvent()
        {
            if (this.Process_HideIndicatorName != null)
                this.Process_HideIndicatorName();
        }

        /// <summary>
        /// To raise Process_HideUnitName event which will hide the unit name form processing information
        /// </summary>
        protected void RaiseHideUnitNameEvent()
        {
            if (this.Process_HideUnitName != null)
                this.Process_HideUnitName();
        }

        /// <summary>
        /// To raise Process_HideFileNo event which will hide the FileNo form processing information
        /// </summary>
        protected void RaiseHideFileNoEvent()
        {
            if (this.Process_HideFileNo != null)
                this.Process_HideFileNo();
        }

        /// <summary>
        /// To raise Process_FileNo  event
        /// </summary>
        /// <param name="current"></param>
        /// <param name="total"></param>
        protected void RaiseProcessFileNoEvent(int current, int total)
        {
            if (this.Process_FileNo != null)
                this.Process_FileNo(current, total);
        }

        /// <summary>
        /// To raise Process_FileOrSheetNo  event
        /// </summary>
        /// <param name="current"></param>
        /// <param name="total"></param>
        protected void RaiseProcessFileOrSheetNoEvent(int current, int total)
        {
            if (this.Process_FileOrSheetNo != null)
                this.Process_FileOrSheetNo(current, total);
        }

        /// <summary>
        /// To raise Process_IndicatorName event
        /// </summary>
        /// <param name="name"></param>
        protected void RaiseProcessIndicatorNameEvent(string name)
        {
            if (this.Process_IndicatorName != null)
                this.Process_IndicatorName(name);
        }

        /// <summary>
        /// To raise Process_UnitName event
        /// </summary>
        /// <param name="name"></param>
        protected void RaiseProcessUnitNameEvent(string name)
        {
            if (this.Process_UnitName != null)
                this.Process_UnitName(name);
        }

        /// <summary>
        /// To raise Process_Sheetname event
        /// </summary>
        /// <param name="sheetname"></param>
        protected void RaiseProcessSheetnameEvent(string sheetname)
        {
            if (this.Process_Sheetname != null)
                this.Process_Sheetname(sheetname);
        }

        /// <summary>
        /// To raise Process_SourceFileName event
        /// </summary>
        /// <param name="fileName"></param>
        protected void RaiseProcessSourceFileNameEvent(string fileName)
        {
            if (this.Process_SourceFileName != null)
                this.Process_SourceFileName(fileName);
        }

        /// <summary>
        /// To raise Process_CurrentInfo event
        /// </summary>
        /// <param name="text"></param>
        protected void RaiseProcessCurrentInfoEvent(string text)
        {
            if (this.Process_CurrentInfo != null)
                this.Process_CurrentInfo(text);
        }

        /// <summary>
        /// To raise ProgressBar_Increment event 
        /// </summary>
        /// <param name="value"></param>
        protected void RaiseProgressBarIncrementEvent(int value)
        {
            if (this.ProgressBar_Increment != null)
                this.ProgressBar_Increment(value);
        }

        /// <summary>
        /// To raise ProgressBar_Initialize event
        /// </summary>
        /// <param name="maxValue"></param>
        protected void RaiseProgressBarInitializeEvent(int maxValue)
        {
            if (this.ProgressBar_Initialize != null)
                this.ProgressBar_Initialize(maxValue);
        }

        /// <summary>
        /// To raise ProgressBar_Close event
        /// </summary>
        protected void RaiseProgressBarCloseEvent()
        {
            if (this.ProgressBar_Close != null)
                this.ProgressBar_Close();
        }

        /// <summary>
        /// To raise Skipped IUSDetails  event
        /// </summary>
        protected void RaiseSkippedIUSEvent(string indicatorName, string unitName,string subgroupVal,string indicatorGID,string unitGID,string subgroupValGId)
        {
            if (this.MissingIUSDetailsEvent != null)
            {
                this.MissingIUSDetailsEvent(indicatorName, unitName, subgroupVal, indicatorGID, unitGID, subgroupValGId);
            }
        }

        #endregion
        /// <summary>
        /// Copy File to Temp Folder by New DateTimeStamp File NAme
        /// </summary>
        /// <param name="sourceFileNameWPath"></param>
        /// <returns></returns>
        protected string CopyExcelFileTOTempFolder(string sourceFileNameWPath)
        {
            string RetVal = string.Empty;

            RetVal = Path.Combine(this._TempFolderPath, DateTime.Now.Ticks.ToString() + Path.GetExtension(sourceFileNameWPath));

            try
            {
                //-- Delete Existing File
                if (File.Exists(RetVal))
                {
                    File.Delete(RetVal);
                }

                File.Copy(sourceFileNameWPath, RetVal, true);

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        /// <summary>
        /// Connects to Target Database. Creates DBonnection + DbQueries
        /// </summary>
        protected void ConnectToDatabase()
        {
            string LanguageCode = string.Empty;

            //-- Merge New Data values Columns into Older Data_Value Column
            DIDataValueHelper.MergeTextualandNumericDataValueColumn(this.TempTargetFile);

            this._DBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess, string.Empty,
            string.Empty, this.TempTargetFile, string.Empty, DAImportCommon.Constants.DBPassword));
            this.DatasetPrefix = this._DBConnection.DIDataSetDefault();

            this.SetQueriesObjects();
        }

        protected void SetQueriesObjects()
        {
            if (this._DBConnection != null)
            {
                // check client application langauge code is  given or not. If given then check it is available or not & if available then use it otherwise work on default language code
                if (!string.IsNullOrEmpty(this._ClientApplicationLanguageCode))
                {
                    if (this._DBConnection.IsValidDILanguage(this.DatasetPrefix, this._ClientApplicationLanguageCode))
                    {
                        this.LanguageCode = this._ClientApplicationLanguageCode;
                    }
                }

                if (string.IsNullOrEmpty(LanguageCode))
                {
                    this.LanguageCode = this._DBConnection.DILanguageCodeDefault(this.DatasetPrefix);

                }



                this._DBQueries = new DIQueries(this.DatasetPrefix, this.LanguageCode);
                this.DAQuery = new ImportQueries(this.DBQueries);
                // Setting Update queries object on the basis of Database Server Type. (UPDATE queries having JOINS are different for ACCESS and SQL Server)
                this.DAQuery.SetUpdateQueryObject(this._DBConnection.ConnectionStringParameters.ServerType);
            }
        }

        /// <summary>
        /// Set basic database properties for online connection
        /// </summary>
        /// <param name="dataset"></param>
        /// <param name="languageCode"></param>
        protected void SetDatabasePropertiesForOnlineConnection(string dataset, string languageCode)
        {
            this.DatasetPrefix = dataset;
            this.LanguageCode = languageCode;
            this._DBQueries = new DIQueries(this.DatasetPrefix, this.LanguageCode);
            this.DAQuery = new ImportQueries(this.DBQueries);
            // Setting Update queries object on the basis of Database Server Type. (UPDATE queries having JOINS are different for ACCESS and SQL Server)
            this.DAQuery.SetUpdateQueryObject(this._DBConnection.ConnectionStringParameters.ServerType);

        }

        /// <summary>
        /// Sets Basic properties for DIData object like, SourceFiles List, TargetFilenameWithPath, TempFolder
        /// </summary>
        /// <param name="sourceFilesWPath">List of SourceFiles</param>
        /// <param name="targetFileNameWPath">Target Database file with path</param>
        /// <param name="tempFolderPath">Temp path where processing will be done</param>
        /// <param name="htmlLogOutputPath"></param>
        protected void SetBasicPropertiesAndProcessValues(List<string> sourceFilesWPath, string targetFileNameWPath, string tempFolderPath, string htmlLogOutputPath)
        {
            this.IsForOnlineDatabase = false;

            //Step1: set source files and target file
            this._SourceFileNamesWPath = sourceFilesWPath;
            this._TargetFileNameWPath = targetFileNameWPath;
            //step2: set temp folder path & TempTargetfile
            this._TempFolderPath = tempFolderPath;
            this.TempTargetFile = Path.Combine(tempFolderPath, Path.GetFileName(targetFileNameWPath));

            this._HtmlLogOutPutPath = htmlLogOutputPath;

            //Step3: make connection to target file
            this.ConnectToDatabase();

            //Step4: process required elements list which can be used for mapping of unmatched elements
            this.ProcessRequiredDIElements();

        }

        /// <summary>
        /// Sets Basic properties for DIData object like, SourceFiles List, etc for online database
        /// </summary>
        /// <param name="sourceFilesWPath">List of SourceFiles</param>
        /// <param name="tempFolderPath">Temp path where processing will be done</param>
        /// <param name="dataset">Dataset Prefix like UT_</param>
        /// <param name="languageCode">Language Code like _en</param>
        protected void SetBasicPropertiesAndProcessValuesForOnline(List<string> sourceFilesWPath, string tempFolderPath, string dataset, string languageCode)
        {
            this.IsForOnlineDatabase = true;

            //Step1: set source files and target file(database file)
            this._SourceFileNamesWPath = sourceFilesWPath;
            this._TargetFileNameWPath = this._DBConnection.GetConnection().Database;
            this._TempFolderPath = tempFolderPath;

            //Step2 : set basic database properties
            this.SetDatabasePropertiesForOnlineConnection(dataset, languageCode);

            //Step3: process required elements list which can be used for mapping of unmatched elements
            this.ProcessRequiredDIElements();

        }


        /// <summary>
        /// Sets required element information. like: UnMatchedElement and Available elemets
        /// </summary>
        protected void SetRequiredElementsInformation()
        {
            //Set unmtached elements table
            this._RequiredDIElements[RequiredDIElementsType.Indicator].UnmatchedElementsTable = this.GetUnmatchedIndicator();
            this._RequiredDIElements[RequiredDIElementsType.Unit].UnmatchedElementsTable = this.GetUnmatchedUnit();
            this._RequiredDIElements[RequiredDIElementsType.Subgroup].UnmatchedElementsTable = this.GetUnmatchedSubgroup();
            this._RequiredDIElements[RequiredDIElementsType.IUS].UnmatchedElementsTable = this.GetUnmatchedIUS();
            this._RequiredDIElements[RequiredDIElementsType.Area].UnmatchedElementsTable = this.GetUnmatchedArea();

            //set available elements table
            this._RequiredDIElements[RequiredDIElementsType.Indicator].AvailableElementsTable = this.GetAvailableIndicator();
            this._RequiredDIElements[RequiredDIElementsType.Unit].AvailableElementsTable = this.GetAvailableUnit();
            this._RequiredDIElements[RequiredDIElementsType.Subgroup].AvailableElementsTable = this.GetAvailableSubgroup();
            this._RequiredDIElements[RequiredDIElementsType.IUS].AvailableElementsTable = this.GetAvailableIUS();
            this._RequiredDIElements[RequiredDIElementsType.Area].AvailableElementsTable = this.GetAvailableArea();
        }


        /// <summary>
        /// Gets and Returns DataTable having Unmatched Indicator from TempDataTable where Indicator didn't matched with existing Indicator in Target DB.
        /// </summary>
        protected DataTable GetUnmatchedIndicator()
        {
            DataTable RetVal = null;
            try
            {
                RetVal = this.DBConnection.ExecuteDataTable(this.DAQuery.GetUnmatchedIndicator());
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        ///  Gets Unmatched Area from TempDataTable where AreaID didn't matched with existing Areas in Target DB.
        /// </summary>
        /// <returns>DataTable having Unmatched AreaName, AreaID</returns>
        protected DataTable GetUnmatchedArea()
        {
            DataTable RetVal = null;
            try
            {
                RetVal = this.DBConnection.ExecuteDataTable(this.DAQuery.GetUnmatchedArea());
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
                RetVal = null;
            }

            return RetVal;
        }

        /// <summary>
        /// Gets and returns DataTable having Unmatched Unit from TempDataTable where Unit didn't matched with existing Unit in Target DB.
        /// </summary>
        protected DataTable GetUnmatchedUnit()
        {
            DataTable RetVal = null;
            try
            {
                RetVal = this.DBConnection.ExecuteDataTable(this.DAQuery.GetUnmatchedUnit());
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Gets and returns DataTable having Unmatched Subgroup from TempDataTable where Subgroup didn't matched with the one existing in Target DB.
        /// </summary>
        /// <returns></returns>
        protected DataTable GetUnmatchedSubgroup()
        {
            DataTable RetVal = null;
            try
            {
                RetVal = this.DBConnection.ExecuteDataTable(this.DAQuery.GetUnmatchedSubgroup());
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
                RetVal = null;
            }

            return RetVal;
        }

        /// <summary>
        /// Get unmatched IUS records when Indicator_Nid, Unit_Nid, SubgroupVal_Nid did NOT matched.
        /// </summary>
        /// <returns>DataTable for unmatched IUS records</returns>
        protected DataTable GetUnmatchedIUS()
        {
            DataTable RetVal = null;
            try
            {
                RetVal = this.DBConnection.ExecuteDataTable(this.DAQuery.GetUnmatchedIUS());
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Deletes (Clears ) sourceFiles from Temp folder
        /// </summary>
        protected void DeleteTempFiles()
        {
            foreach (string TempFile in this.TempFiles)
            {
                try
                {
                    System.ComponentModel.BackgroundWorker bk;


                    File.Delete(TempFile);
                }
                catch (Exception ex)
                {
                    ExceptionFacade.ThrowException(ex);
                }
            }
        }

        /// <summary>
        /// It updates SkippedFiles (spreadsheet or database ) in log. Reason for skipping must be mentioned, like invalid sheet, Missing Indicator, RTE
        /// </summary>
        protected void UpdateSkippedFileLog(string fileNameWPath, string sheetName, string reasonForSkipping)
        {
            try
            {
                if (this.SkippedFiles == null)
                {
                    // Set columns in SkippedFiles dataTable
                    this.SkippedFiles = new DataTable();
                    this.SkippedFiles.Columns.Add(DAImportCommon.Constants.Log.SkippedSourceFileColumnName, typeof(string));
                    this.SkippedFiles.Columns.Add(DAImportCommon.Constants.Log.SkippedSheetName, typeof(string));
                    this.SkippedFiles.Columns.Add(DAImportCommon.Constants.Log.SkippedFileReason, typeof(string));
                }
                string[] dr = new string[3];
                dr[0] = Path.GetFileName(fileNameWPath);                      //Spreadsheet FileName
                dr[1] = sheetName; ;                // SheetName
                dr[2] = reasonForSkipping;          // Reason
                this.SkippedFiles.Rows.Add(dr);
            }
            catch (Exception)
            {
            }
        }

        #region "-- Abstract --"

        /// <summary>
        /// Abstract Methods. Refer to overriden methods in Derived Classes.
        /// </summary>
        protected abstract void ProcessTempDataTable();

        /// <summary>
        /// It Adds Notes and Assiatants from Source Database into Target Database. (This methods works only in case of Database Source)
        /// </summary>
        protected abstract void AddNotesAssistants();

        protected abstract bool UploadDatabase();

        #endregion

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables --"

        private string _OutputFileNameWPath;
        /// <summary>
        /// Gets or sets OutputFileNameWPath 
        /// </summary>
        public string OutputFileNameWPath
        {
            get
            {
                return this._OutputFileNameWPath;
            }
            set
            {
                this._OutputFileNameWPath = value;
            }
        }


        /// <summary>
        /// Target Database or Template fileName With Path in Temp folder.
        /// </summary>
        public string TempTargetFile = string.Empty;

        protected DIConnection _DBConnection;
        /// <summary>
        ///  Gets or Sets The DIConnection for target Database
        /// </summary>
        public DIConnection DBConnection
        {
            get
            {
                return this._DBConnection;
            }
            set
            {
                this._DBConnection = value;
            }
        }

        protected DIQueries _DBQueries;
        /// <summary>
        /// Gets or Sets DbQueries for Target Databse
        /// </summary>
        public DIQueries DBQueries
        {
            get
            {
                return this._DBQueries;
            }
            set
            {
                this._DBQueries = value;
            }
        }

        protected List<string> _SourceFileNamesWPath;
        /// <summary>
        /// Gets or sets source filenames with path
        /// </summary>
        public List<string> SourceFileNamesWPath
        {
            get
            {
                return this._SourceFileNamesWPath;
            }
            set
            {
                this._SourceFileNamesWPath = value;
            }
        }

        private string _TargetFileNameForLogFile = string.Empty;
        /// <summary>
        /// Gets or Sets the target filename with path for logfile. if this path is not availble then target file name will be used in log file.
        /// </summary>
        public string TargetFileNameForLogFile
        {
            get
            {
                if (string.IsNullOrEmpty(this._TargetFileNameForLogFile))
                {
                    this._TargetFileNameForLogFile = this._TargetFileNameWPath;
                }
                return _TargetFileNameForLogFile;
            }
            set
            {
                this._TargetFileNameForLogFile = value;
            }
        }


        protected string _TargetFileNameWPath;
        /// <summary>
        /// Gets or sets target filename with path.
        /// </summary>
        public string TargetFileNameWPath
        {
            get
            {
                return this._TargetFileNameWPath;
            }
            set
            {
                this._TargetFileNameWPath = value;
            }
        }

        protected string _TempFolderPath;
        /// <summary>
        /// Gets or sets temp folder path
        /// </summary>
        public string TempFolderPath
        {
            get
            {
                return this._TempFolderPath;
            }
            set
            {
                this._TempFolderPath = value;
            }
        }

        protected string _HtmlLogFilePath;
        /// <summary>
        /// Gets the file name with Path of Html log created after process finish.
        /// </summary>
        public string HtmlLogFilePath
        {
            get
            {
                return _HtmlLogFilePath;
            }
        }

        protected Dictionary<RequiredDIElementsType, UnmatchedElementsInfo> _RequiredDIElements;
        /// <summary>
        /// Gets or Sets mapping information of required elements like indicator, unit,..etc.
        /// </summary>
        public Dictionary<RequiredDIElementsType, UnmatchedElementsInfo> RequiredDIElements
        {
            get
            {
                return this._RequiredDIElements;
            }
            set
            {
                this._RequiredDIElements = value;
            }
        }

        protected string _ClientApplicationLanguageCode = string.Empty;
        /// <summary>
        /// Sets Client application's language code. Default is empty. Set this variable when you want to import data against a particular language. For example, you want to import data from French DES file into Template which conatins FR and English tables & english language is set to default. 
        /// </summary>
        public string ClientApplicationLanguageCode
        {
            set
            {
                this._ClientApplicationLanguageCode = value;
                this.SetQueriesObjects();
            }
        }


        #endregion

        #region "-- Events --"

        /// <summary>
        /// Fires to display processing time
        /// </summary>
        public event ProcessingTimeDelegate Process_Time;

        /// <summary>
        /// Fires when processing indicator
        /// </summary>
        public event CurrentIndicatorNameDelegate Process_IndicatorName;

        /// <summary>
        /// Fires when processing unit
        /// </summary>
        public event CurrentUnitNameDelegate Process_UnitName;

        /// <summary>
        /// Fires when processing source file
        /// </summary>
        public event CurrentFileNoDelegate Process_FileNo;

        /// <summary>
        /// Fires when processing worksheet
        /// </summary>
        public event CurrentFileOrSheetNoDelegate Process_FileOrSheetNo;

        /// <summary>
        /// Fires when processing worksheet
        /// </summary>
        public event CurrentSheetNameDelegate Process_Sheetname;

        /// <summary>
        /// Fires when processing source file
        /// </summary>
        public event CurrentSourceFileNameDelegate Process_SourceFileName;

        /// <summary>
        /// Fires when processing some logic
        /// </summary>
        public event CurrentProcessingInfoDelegate Process_CurrentInfo;

        /// <summary>
        /// Fires after processing worksheet.
        /// </summary>
        public event HideSheetOrFileNoDelegate Process_HideSheetOrFileNo;

        /// <summary>
        /// Fires after processing of worksheet.
        /// </summary>
        public event HideSheetNameDelegate Process_HideSheetName;

        /// <summary>
        /// Fires after processing of Indicator(only for DES).
        /// </summary>
        public event HideIndicatorNameDelegate Process_HideIndicatorName;

        /// <summary>
        /// Fires after processing of Unit(only for DES).
        /// </summary>
        public event HideUnitNameDelegate Process_HideUnitName;

        /// <summary>
        /// Fires after processing of source file.
        /// </summary>
        public event HideFileNoDelegate Process_HideFileNo;


        /// <summary>
        /// Fires when value of prgressbar is changed.
        /// </summary>
        public event IncrementProgressBar ProgressBar_Increment;

        /// <summary>
        /// Fires when process started to initialize progress bar.
        /// </summary>
        public event InitializeProgressBar ProgressBar_Initialize;

        /// <summary>
        /// Fireds when process stop.
        /// </summary>
        public event CloseProgressBar ProgressBar_Close;

        /// <summary>
        /// Initialize Background Process
        /// </summary>
        public event InitializeWorkerProcess InitializeProcessEvent;
        /// <summary>
        /// Set Process Text
        /// </summary>
        public event SetWorkerProcessName SetProcessNameEvent;

        /// <summary>
        /// Set Skipped IUS Event
        /// </summary>
        public event MissingIUSDetailsDelegate MissingIUSDetailsEvent;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Call this method whenever importing mapping information from external Xml file to import mapping information and update mapped elements.
        /// </summary>
        /// <param name="mappedIndicator"></param>
        /// <param name="mappedUnit"></param>
        /// <param name="mappedSubgroupVal"></param>
        /// <param name="mappedArea"></param>
        public void ProcessAutoMappingInfo(DataTable mappedIndicator, DataTable mappedUnit
            , DataTable mappedSubgroupVal, DataTable mappedArea)
        {
            //Step 1: procees mapped elements for Indicator
            this.ProcessAutoIndicatorMappingInfo(mappedIndicator);

            //Step 2: procees mapped elements for Unit
            this.ProcessAutoUnitMappingInfo(mappedUnit);

            //Step 3: procees mapped elements for SubgroupVal
            this.ProcessAutoSubgroupMappingInfo(mappedSubgroupVal);

            //Step 4: procees mapped elements for Area
            this.ProcessAutoAreaMappingInfo(mappedArea);
        }

        /// <summary>
        /// Starts the Import process first Time.
        /// 1.It interates through all source Files and insert its data into Target Dtabase Temp Table.
        /// 2.Removes blank records. 
        /// 3. Updates auto - matched I, U, S, Area Nid in Temp Table.
        /// 4. Sets unMatched element DataTables.
        /// </summary>
        /// <returns></returns>
        public bool StartImportProcess()
        {
            bool RetVal = false;
            int MaxValueForProgressBar = 0;
            int ProgressBarValue = 0;
            try
            {
                //Set progress bar
                MaxValueForProgressBar = 3;
                this.RaiseProcessCurrentInfoEvent(DAImportCommon.Constants.PleaseWaitString);
                this.RaiseProgressBarInitializeEvent(MaxValueForProgressBar);

                // Set Start time of process. 
                this.StartTime = DateTime.Now;

                // Delete Temp Tables (if any present already in SQLServer Database )
                this.DropTempTables();

                // Step 1 :Create Temp_Data table in Target database , ANd Alter IUSNid column type to Double
                this.DBConnection.ExecuteNonQuery(this.DAQuery.CreateTempDataTable());
                this.DBConnection.ExecuteNonQuery(this.DAQuery.AlterIUSNidColumnTypeToDouble());
                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                // Step 2: Copy all  files into temp location. and raise progress bar increment event
                this.CopyFilesIntoTempFldr();
                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                // Step 3: Process temp data table : Processing depends on concrete classes(database/ dataenetryspreadsheet).
                // and raise progress bar increment event
                this.ProcessTempDataTable();
                this.RaiseProcessCurrentInfoEvent(DAImportCommon.Constants.PleaseWaitString);

                // Step 4: Update Nids where GIDs and Names for Indicator/ Unit/ Subroup / Area, matches.and raise progress bar increment event
                MaxValueForProgressBar = 5;
                ProgressBarValue = 0;
                this.RaiseProgressBarInitializeEvent(MaxValueForProgressBar);

                this.UpdateNidsInTempDataTable();
                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                // Step 5: Update IUSNid in Temp_Data for unmatched records and raise progress bar increment event
                this.UpdateIUSNidOfUnmatchedRecords();
                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                // Step 6: Set Required Unmatched & Available elements.
                this.SetRequiredElementsInformation();

                // Step 7: Set Log Info table used for HTML log creation
                this.SetElementLogInfoTable();

                // Step 8: Set Duplicate record table and raise progress bar increment event

                //  this.DuplicateRecordsTable = this.DBConnection.ExecuteDataTable(this.DAQuery.GetDuplicateRecordsInTempData());

                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                RetVal = true;
            }
            catch (Exception ex)
            {
                RetVal = false;
                ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                //raise close progress bar event
                this.RaiseProgressBarCloseEvent();
            }

            return RetVal;
        }


        /// <summary>
        /// Imports DataValue from TempTable into UT_Data Table. for those records whose GID and Names were either matched or Mapped during the whole process.
        /// </summary>
        /// <returns>true if Imports DataValue completes succesfully</returns>
        public bool ImportDataValue()
        {
            bool RetVal = false;
            int MaxValueForProgressBar = 0;
            int ProgressBarValue = 0;

            try
            {
                // -Set progress bar
                MaxValueForProgressBar = 7;
                this.RaiseProgressBarInitializeEvent(MaxValueForProgressBar);
                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                // Remove Duplicate records
                this.RaiseProcessCurrentInfoEvent(LanguageStrings.RemovingDuplicateRecords);
                this.DeleteDuplicateRecords();

                // Inserts New TimePeriods, FootNotes, Sources . And Updates corresponding Nids
                this.InsertTimePeriodSourceFootNotes();

                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                if (this.IsForOnlineDatabase == false)
                {
                    this.DBConnection.Dispose();
                    GC.Collect();   //- To free database Connection in garbage memory
                    this.ConnectToDatabase();
                }

                this.RaiseProcessCurrentInfoEvent(LanguageStrings.PleaseWait);

                // Set skipped records Log table before deleting unmatched records left.
                this.SkippedRecordsTable = this.DBConnection.ExecuteDataTable(this.DAQuery.GetSkippedRecords());

                //- Get Matched IUS in LogIUStable
                //- NOTE: Matched IUS is required in CSV Import Log Creation.
                this.IUSLogInfoTable = this._DBConnection.ExecuteDataTable(this.DAQuery.GetMatchedIUS());

                // Deletes those records from Temp table where New_IUSNid & Area_Nid is NULL. i.e. those records which were NOT mapped by user.
                this.RaiseProcessCurrentInfoEvent(LanguageStrings.DeletingUnmappedRecords);
                this.DBConnection.ExecuteNonQuery(this.DAQuery.DeleteUnMappedRecordsLeft());
                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                // Update Data Value for existing I, U, S, 
                this.RaiseProcessCurrentInfoEvent(LanguageStrings.UpdateValuesForExistingIUS);

                if (this._DBConnection.ConnectionStringParameters != null && this._DBConnection.ConnectionStringParameters.ServerType == DIServerType.MsAccess)
                {
                    // create temp column in Data & TempDataTable
                    this.DBConnection.ExecuteNonQuery(DAQuery.CreateTempColInDataTable());
                    this.DBConnection.ExecuteNonQuery(DAQuery.CreateTempColInTempDataTable());

                    // update temp col 
                    this.DBConnection.ExecuteNonQuery(DAQuery.UpdateTempColInTempdataTable());
                    this.DBConnection.ExecuteNonQuery(DAQuery.UpdateTempColInDataTable());

                    // update data value where tempcolumns are same
                    this.DBConnection.ExecuteNonQuery(DAQuery.UpdateDataValueforAccess());


                    ProgressBarValue += 1;
                    this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                    // Insert new Data Values
                    this.RaiseProcessCurrentInfoEvent(LanguageStrings.InsertingNewDataValue);
                    this.DBConnection.ExecuteNonQuery(DAQuery.InsertDataValueForMsAccess());

                    // drop temp column
                    this.DBConnection.ExecuteNonQuery(DIQueries.DropIndividualColumnOfTable(this.DBQueries.TablesName.Data, DevInfo.Lib.DI_LibBAL.Import.DAImport.Common.Constants.TempColumnName));
                    this.DBConnection.ExecuteNonQuery(DIQueries.DropIndividualColumnOfTable(DevInfo.Lib.DI_LibBAL.Import.DAImport.Common.Constants.TempDataTableName, DevInfo.Lib.DI_LibBAL.Import.DAImport.Common.Constants.TempColumnName));

                }
                else
                {
                    this.DBConnection.ExecuteNonQuery(DAQuery.UpdateDataValue());
                    ProgressBarValue += 1;
                    this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                    // Insert new Data Values
                    this.RaiseProcessCurrentInfoEvent(LanguageStrings.InsertingNewDataValue);
                    this.DBConnection.ExecuteNonQuery(DAQuery.InsertDataValue());
                }

                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                //// Replace BlankDataValueSymbol With Blank datavalue
                //this.DBConnection.ExecuteNonQuery(DAQuery.UpdateBlankDataValueSymbolWithBlank(DICommon.DESImportBlankDataValueSymbol));

                //insert recommended sources
                this.RaiseProcessCurrentInfoEvent("Processing recommended sources");
                this.InsertRecommendedSources();

                // Add Notes & Assistant in Target DataBase
                this.RaiseProcessCurrentInfoEvent(LanguageStrings.ProcessingNotesAndAssistant);
                this.AddNotesAssistants();
                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                if (this.IsForOnlineDatabase == false)
                {
                    // Create HTML log
                    this.RaiseProcessCurrentInfoEvent(LanguageStrings.CreatingLog);

                    this.CreateHTMLLog();
                }

                this.RaiseProcessCurrentInfoEvent(LanguageStrings.PleaseWait);

                // Delete Source files from Temp location
                this.DeleteTempFiles();

                //Drop Temp Tables Used (TempDataTable, TempUnmatchedIUSTable, TempDuplicateTable, etc..)
                this.DropTempTables();

                //-- Update indicatorNID, UNitNId, SubgroupValNId in UT_Data
                this.UpdateIndicatorUnitSubgroupNidsInData();

                // delete data rows where datavalue is "000000" (this value can exist only if missingdata symbol is set in da.xml file)
                this.DBConnection.ExecuteNonQuery("Delete FROM " + this.DBQueries.TablesName.Data + " WHERE " + Data.DataValue + "='000000'");

                // Increment progressBar value. & raise event
                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                // Dispose Database connection
                this.DBConnection.Dispose();
                this.DBQueries = null;

                // Sleep Thread for 1000 milliSecond
                Thread.Sleep(1000);

                RetVal = true;
            }
            catch (Exception ex)
            {
                RetVal = false;
                ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                //raise close progress bar event
                this.RaiseProgressBarCloseEvent();

                GC.Collect();
            }
            return RetVal;
        }


        #region "-- Mapping /Undo --"

        /// <summary>
        /// It updates required elements for IUS (Unmatched IUS).
        /// </summary>
        public void UpdateRequiredElementsInformation()
        {
            this._RequiredDIElements[RequiredDIElementsType.IUS].UnmatchedElementsTable = this.GetUnmatchedIUS();
        }

        /// <summary>
        /// Update TempData.IUSNid where Indicator_Nid, Unit_Nid, SubGroupVal_Nid presents in "Indicator_Unit_Subgroup" Table
        /// </summary>
        public void UpdateIUSNidofMatchedRecords()
        {
            try
            {
                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateIUSNidofMatchedRecords());
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// Updates IUSNIDs of umatched elements in TempDataTable. (Unmatched IUSNid = IndicatorUnmatchedNID & UnitUnmatcheNId & SubgroupUnmatchedNID) OR (Unmatched IUSNid = - DataNid)
        /// </summary>
        public void UpdateIUSNidOfUnmatchedRecords()
        {
            try
            {
                // Update unmatched IUSNid = - DataNid , where any of IndicatorNid, unitNid, SubgroupNid is NULL.  
                {
                    if (this.ImportFileType == DIImportFileType.SDMXXml)
                    {
                        // in SDMX, matching of I U S are done on GIDs, NOT on names
                        this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateUnmatchedIUSNidFromTempUnmatchedIUSTable(true));
                    }
                    else
                    {
                        // in spreadsheets cases, matching is done on Names (becos, GID may or may not be present)
                        this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateUnmatchedIUSNidFromTempUnmatchedIUSTable(false));
                    }

                }

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }

        }

        /// <summary>
        /// It updates NIDs for all mapped elements (I, U, S, IUS, Area)  in TempDataTable.
        /// </summary>
        public void UpdateNidsOfMappedElements()
        {
            this.UpdateMappedElements(RequiredDIElementsType.Indicator);
            this.UpdateMappedElements(RequiredDIElementsType.Unit);
            this.UpdateMappedElements(RequiredDIElementsType.Subgroup);

            // Update Map_IUSNid where I,U,S are matched
            this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateIUSNidofMatchedRecords());
            this.UpdateMappedElements(RequiredDIElementsType.IUS);
            this.UpdateMappedElements(RequiredDIElementsType.Area);

            //////// Update Mapped Indicators
            //////this.UpdateMappedIndicators();

            //////// Update Mapped Unit
            //////this.UpdateMappedUnits();

            //////// Update Mapped Subgroup
            //////this.UpdateMappedSubgroup();

            ////////// Update Map_IUSNid where I,U,S are matched
            ////////this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateIUSNidofMatchedRecords());

            ////////// Update IUSNid where I,U,S are left unmatched or Blank.
            //////////this.UpdateIUSNidOfUnmatchedRecords();

            //////// Update Mapped IUS
            //////this.UpdateMappedIUS();

            //////// Update Mapped Area
            //////this.UpdateMappedAreas();
        }

        #region "-- todo:Delete --"
        /// <summary>
        /// It updates NIDs for Mapped IUS of source and target Database.
        /// </summary>
        public void UpdateMappedIUS()
        {
            Dictionary<string, MappedElementInfo> MappedElements = this._RequiredDIElements[RequiredDIElementsType.IUS].MappedElements;
            if (MappedElements != null)
            {
                foreach (MappedElementInfo ElementInfo in MappedElements.Values)
                {
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateMappedElementNids(DAImportCommon.Constants.NewIUSColumnName, (int)(ElementInfo.MatchedRow[Indicator_Unit_Subgroup.IUSNId]), Indicator_Unit_Subgroup.IUSNId, (double)ElementInfo.UnMatchedRow[Indicator_Unit_Subgroup.IUSNId]));
                }
            }
        }

        /// <summary>
        /// It updates NIDs for Mapped Areas of source and target Database.
        /// </summary>
        public void UpdateMappedAreas()
        {
            Dictionary<string, MappedElementInfo> MappedElements = this._RequiredDIElements[RequiredDIElementsType.Area].MappedElements;
            foreach (MappedElementInfo ElementInfo in MappedElements.Values)
            {
                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateMappedElementNids(Area.AreaNId, (int)(ElementInfo.MatchedRow[Area.AreaNId]), Area.AreaID, DICommon.RemoveQuotes(ElementInfo.UnMatchedRow[Area.AreaID].ToString())));
            }
        }

        /// <summary>
        /// It updates NIDs for Mapped Subgroup of source and target Database.
        /// </summary>
        public void UpdateMappedSubgroup()
        {
            Dictionary<string, MappedElementInfo> MappedElements = this._RequiredDIElements[RequiredDIElementsType.Subgroup].MappedElements;

            if (MappedElements != null)
            {
                foreach (MappedElementInfo ElementInfo in MappedElements.Values)
                {
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateMappedElementNids(SubgroupVals.SubgroupValNId, (int)(ElementInfo.MatchedRow[SubgroupVals.SubgroupValNId]), SubgroupVals.SubgroupVal, DICommon.RemoveQuotes(ElementInfo.UnMatchedRow[SubgroupVals.SubgroupVal].ToString())));
                }
            }
        }

        /// <summary>
        /// It updates NIDs for Mapped Units of source and target Database.
        /// </summary>
        public void UpdateMappedUnits()
        {
            Dictionary<string, MappedElementInfo> MappedElements = this._RequiredDIElements[RequiredDIElementsType.Unit].MappedElements;
            if (MappedElements != null)
            {
                foreach (MappedElementInfo ElementInfo in MappedElements.Values)
                {
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateMappedElementNids(Unit.UnitNId, (int)(ElementInfo.MatchedRow[Unit.UnitNId]), Unit.UnitName, DICommon.RemoveQuotes(ElementInfo.UnMatchedRow[Unit.UnitName].ToString())));
                }
            }
        }

        /// <summary>
        /// It updates NIDs for Mapped Units of source and target Database.
        /// <param name="updateColumnName"></param> 
        /// </summary>
        public void UpdateMappedUnitsByGid()
        {
            Dictionary<string, MappedElementInfo> MappedElements = this._RequiredDIElements[RequiredDIElementsType.Unit].MappedElements;
            foreach (MappedElementInfo ElementInfo in MappedElements.Values)
            {
                this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateMappedElementNids(Unit.UnitNId, (int)(ElementInfo.MatchedRow[Unit.UnitNId]), Unit.UnitGId, DICommon.RemoveQuotes(ElementInfo.UnMatchedRow[Unit.UnitGId].ToString())));
            }
        }


        /// <summary>
        /// It updates NIDs for Mapped Indicators of source and target Database.
        /// </summary>
        public void UpdateMappedIndicators()
        {
            Dictionary<string, MappedElementInfo> MappedElements = this._RequiredDIElements[RequiredDIElementsType.Indicator].MappedElements;

            if (MappedElements != null)
            {
                foreach (MappedElementInfo ElementInfo in MappedElements.Values)
                {
                    this.DBConnection.ExecuteNonQuery
                        (this.DAQuery.UpdateMappedElementNids
                        (Indicator.IndicatorNId, (int)(ElementInfo.MatchedRow[Indicator.IndicatorNId]), Indicator.IndicatorName, DICommon.RemoveQuotes(ElementInfo.UnMatchedRow[Indicator.IndicatorName].ToString())));
                }
            }
        }

        #endregion


        #region MyRegion


        public void UpdateMappedElements(RequiredDIElementsType elementType)
        {
            Dictionary<string, MappedElementInfo> MappedElements;
            string columnToUpdate = string.Empty;
            string NidColumnName = string.Empty;
            string FilerColumnName = string.Empty;

            switch (elementType)
            {
                case RequiredDIElementsType.Indicator:
                    columnToUpdate = Indicator.IndicatorNId;
                    if (this.ImportFileType == DIImportFileType.SDMXXml)
                    {
                        FilerColumnName = Indicator.IndicatorGId;
                    }
                    else
                    {
                        FilerColumnName = Indicator.IndicatorName;
                    }
                    break;

                case RequiredDIElementsType.Unit:

                    columnToUpdate = Unit.UnitNId;
                    if (this.ImportFileType == DIImportFileType.SDMXXml)
                    {
                        FilerColumnName = Unit.UnitGId;
                    }
                    else
                    {
                        FilerColumnName = Unit.UnitName;
                    }
                    break;

                case RequiredDIElementsType.Subgroup:

                    columnToUpdate = SubgroupVals.SubgroupValNId;

                    if (this.ImportFileType == DIImportFileType.SDMXXml)
                    {
                        FilerColumnName = SubgroupVals.SubgroupValGId;
                    }
                    else
                    {
                        FilerColumnName = SubgroupVals.SubgroupVal;
                    }
                    break;

                case RequiredDIElementsType.IUS:
                    columnToUpdate = DAImportCommon.Constants.NewIUSColumnName;
                    break;

                case RequiredDIElementsType.Area:
                    columnToUpdate = Area.AreaNId;
                    FilerColumnName = Area.AreaID;

                    break;
                default:
                    break;
            }

            // commn values
            MappedElements = this.RequiredDIElements[elementType].MappedElements;

            if (elementType == RequiredDIElementsType.IUS)
            {
                NidColumnName = Indicator_Unit_Subgroup.IUSNId;
                FilerColumnName = NidColumnName;
                this.UpdateMappedElements(MappedElements, columnToUpdate, NidColumnName, FilerColumnName, true);
            }
            else
            {
                NidColumnName = columnToUpdate;
                this.UpdateMappedElements(MappedElements, columnToUpdate, NidColumnName, FilerColumnName);
            }
        }

        private void UpdateMappedElements(Dictionary<string, MappedElementInfo> MappedElements, string columnToUpdate, string NidColumnName, string filterColumnName, bool isExpressionInDouble)
        {
            string SqlQuery = string.Empty;

            foreach (MappedElementInfo ElementInfo in MappedElements.Values)
            {
                if (isExpressionInDouble)
                {
                    SqlQuery = this.DAQuery.UpdateMappedElementNids(columnToUpdate, (int)(ElementInfo.MatchedRow[NidColumnName]), filterColumnName, (double)ElementInfo.UnMatchedRow[filterColumnName]);
                }
                else
                {
                    SqlQuery = this.DAQuery.UpdateMappedElementNids(columnToUpdate, (int)(ElementInfo.MatchedRow[NidColumnName]), filterColumnName, ElementInfo.UnMatchedRow[filterColumnName].ToString());
                }
                this.DBConnection.ExecuteNonQuery(SqlQuery);
            }
        }

        private void UpdateMappedElements(Dictionary<string, MappedElementInfo> MappedElements, string columnToUpdate, string NidColumnName, string filterColumnName)
        {
            this.UpdateMappedElements(MappedElements, columnToUpdate, NidColumnName, filterColumnName, false);
        }

        #endregion


        /// <summary>
        /// It clears the NIDs updated in TempDataTable for Mapped elements.
        /// </summary>
        /// <param name="elementType"></param>
        public void UndoMappedElementNIDUpdate(RequiredDIElementsType elementType)
        {
            string ColumnToUpdate = string.Empty;          // It represents Column to be Updated
            string ExpressionColumnName = string.Empty;   // Represnts Column that will be checked in 'where' cluase. 
            string values = "";                       // It contains values for elements that were mapped. e.g: Indicator Names, unit Names

            // Set Column to be Updated, & ExpressionColumnName to be used in 'where' clause.
            if (this.ImportFileType == DIImportFileType.DataEntrySpreasheet)
            {
                switch (elementType)
                {
                    case RequiredDIElementsType.Indicator:
                        ColumnToUpdate = Indicator.IndicatorNId;
                        ExpressionColumnName = Indicator.IndicatorName;
                        break;

                    case RequiredDIElementsType.Unit:
                        ColumnToUpdate = Unit.UnitNId;
                        ExpressionColumnName = Unit.UnitName;
                        break;

                    case RequiredDIElementsType.Subgroup:
                        ColumnToUpdate = SubgroupVals.SubgroupValNId;
                        ExpressionColumnName = SubgroupVals.SubgroupVal;
                        break;

                    case RequiredDIElementsType.IUS:
                        ColumnToUpdate = DAImportCommon.Constants.NewIUSColumnName;
                        ExpressionColumnName = Indicator_Unit_Subgroup.IUSNId;
                        values = string.Empty;
                        break;

                    case RequiredDIElementsType.Area:
                        ColumnToUpdate = Area.AreaNId;
                        ExpressionColumnName = Area.AreaID;
                        break;

                    default:
                        break;
                }

            }
            else
            {
                switch (elementType)
                {
                    case RequiredDIElementsType.Indicator:
                        ColumnToUpdate = Indicator.IndicatorNId;
                        ExpressionColumnName = Indicator.IndicatorGId;
                        break;

                    case RequiredDIElementsType.Unit:
                        ColumnToUpdate = Unit.UnitNId;
                        ExpressionColumnName = Unit.UnitGId;
                        break;

                    case RequiredDIElementsType.Subgroup:
                        ColumnToUpdate = SubgroupVals.SubgroupValNId;
                        ExpressionColumnName = SubgroupVals.SubgroupValGId;
                        break;

                    case RequiredDIElementsType.IUS:
                        ColumnToUpdate = DAImportCommon.Constants.NewIUSColumnName;
                        ExpressionColumnName = Indicator_Unit_Subgroup.IUSNId;
                        values = string.Empty;
                        break;

                    case RequiredDIElementsType.Area:
                        ColumnToUpdate = Area.AreaNId;
                        ExpressionColumnName = Area.AreaID;
                        break;

                    default:
                        break;
                }

            }

            // Set Element values to be checked
            foreach (string value in this.RequiredDIElements[elementType].MappedElements.Keys)
            {
                if (values.Length == 0)
                {
                    if ((elementType == RequiredDIElementsType.IUS))
                    {
                        values = value;                      // For int type values
                    }
                    else
                    {
                        values += "'" + DICommon.RemoveQuotes(value) + "'";           // For string type values (remove single quote if any)
                    }
                }
                else
                {
                    if ((elementType != RequiredDIElementsType.IUS))
                    {
                        values += ", '" + DICommon.RemoveQuotes(value) + "'";     // For string type values (remove single quote if any)
                    }
                    else
                    {
                        values += ", " + value;           // For int type values
                    }
                }
            }

            // Update Table
            if (values != string.Empty)
            {
                if (elementType == RequiredDIElementsType.IUS)
                {
                    // Update Nids incase of IUS 
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.SetMappedElementNIDToNULL(ColumnToUpdate, ExpressionColumnName, values, false));
                }
                else
                {
                    // Update Nids incase of I, U, S, Area
                    this.DBConnection.ExecuteNonQuery(this.DAQuery.SetMappedElementNIDToNULL(ColumnToUpdate, ExpressionColumnName, values, true));
                }
            }
        }

        #endregion

        /// <summary>
        /// Updates specified DataBase name as parameter, in DB_Available Table.
        /// </summary>
        /// <param name="availableDbName"></param>
        public void UpdateAvialableDBInSavedFile(string availableDbWName)
        {
            DIConnectionDetails ConnectionDetails;
            DIConnection AvailableDBConnection;
            string DatasetPrefix;
            string LanguageCode;
            DIQueries AvailableDBQuery;
            ImportQueries Query;

            //' --Create connection with saved file
            ConnectionDetails = new DIConnectionDetails(DIServerType.MsAccess, string.Empty, string.Empty, availableDbWName, string.Empty, DAImportCommon.Constants.DBPassword);

            AvailableDBConnection = new DIConnection(ConnectionDetails);
            DatasetPrefix = AvailableDBConnection.DIDataSetDefault();
            LanguageCode = AvailableDBConnection.DILanguageCodeDefault(DatasetPrefix);
            AvailableDBQuery = new DIQueries(DatasetPrefix, LanguageCode);
            Query = new ImportQueries(AvailableDBQuery);

            //' - Update Avialble DB fileName in DB_Available_Database table.
            AvailableDBConnection.ExecuteNonQuery(Query.UpdateDB_Available_DatabaseTable(Path.GetFileNameWithoutExtension(availableDbWName)));

            //' -- Dispose Connection
            AvailableDBConnection.Dispose();
            AvailableDBQuery = null;
            Query = null;
        }


        #region "-- IDisposable Members --"

        /// <summary>
        /// It disposes all resources used during process & Target Db connection.
        /// </summary>
        public void Dispose()
        {
            if (this.DBConnection != null)
            {
                this.DBConnection.Dispose();
            }
        }


        #endregion

        #region "-- Upload Database --"
        /// <summary>
        /// Upload DES and Database to Other Online Database
        /// </summary>
        /// <returns></returns>
        public bool UploadData()
        {
            bool RetVal = false;

            //-- Upload database
            RetVal = this.UploadDatabase();

            return RetVal;
        }

        //private void UploadDatabaseData(UserSelection userSelections, DIConnection sourceDBConnection, DIQueries sourceDBQueries, DIConnection targetDBConnection, DIQueries targetDBQueries)
        //{
        //    DataTable Table = null;
        //    IndicatorBuilder IndicatorBuilderObj = null;
        //    IUSBuilder IUSBuilderObj = null;
        //    AreaBuilder AreaBuilderObj = null;
        //    SourceBuilder SourceBuilderObj = null;
        //    TimeperiodBuilder TimeperiodBuilderObj = null;
        //    FootnoteBuilder FootnoteBuilderObj = null;
        //    FootnoteBuilder SourceDBFootnoteBuilderObj = null;

        //    Dictionary<string, int> AreaList = new Dictionary<string, int>();
        //    Dictionary<string, int> SourceList = new Dictionary<string, int>();
        //    Dictionary<string, int> TimeperiodList = new Dictionary<string, int>();
        //    Dictionary<string, int> IUSList = new Dictionary<string, int>();

        //    int AreaNId = 0;
        //    int SourceNId = 0;
        //    int TimeperiodNId = 0;
        //    int IUSNId = 0;
        //    string IndicatorGID = string.Empty;
        //    string UnitGID = string.Empty;
        //    string SubgroupGID = string.Empty;
        //    string IndicatorName = string.Empty;
        //    string UnitName = string.Empty;
        //    string SubgroupName = string.Empty;
        //    string IUSString = string.Empty;
        //    string Datavalue = string.Empty;
        //    int FootNotesNId = -1;
        //    string DataDenominator = string.Empty;
        //    int SheetCount = 0;
        //    int Counter = 0;
        //    int CurrentSheetNo = 0;

        //    try
        //    {

        //        //-- Copy File INto Temp Location
        //        Table = sourceDBConnection.ExecuteDataTable(sourceDBQueries.Data.GetDataNIDs(userSelections.IndicatorNIds, userSelections.TimePeriodNIds, userSelections.AreaNIds, userSelections.SourceNIds, userSelections.ShowIUS, FieldSelection.Heavy, false));

        //        DevInfo.Lib.DI_LibBAL.DA.DML.DIDatabase DIDatabaseObj = new DevInfo.Lib.DI_LibBAL.DA.DML.DIDatabase(targetDBConnection, targetDBQueries);

        //        //-- Initialize Progress
        //        this.RaiseInitilizeProcessEvent(Path.GetFileName(sourceDBConnection.ConnectionStringParameters.DbName), SheetCount, 100);

        //        if (Table.Rows.Count > 0)
        //        {
        //            IndicatorBuilderObj = new IndicatorBuilder(targetDBConnection, targetDBQueries);
        //            IUSBuilderObj = new IUSBuilder(targetDBConnection, targetDBQueries);
        //            AreaBuilderObj = new AreaBuilder(targetDBConnection, targetDBQueries);
        //            SourceBuilderObj = new SourceBuilder(targetDBConnection, targetDBQueries);
        //            TimeperiodBuilderObj = new TimeperiodBuilder(targetDBConnection, targetDBQueries);
        //            FootnoteBuilderObj = new FootnoteBuilder(targetDBConnection, targetDBQueries);
        //            //-- Get Footnotes from Source Database
        //            SourceDBFootnoteBuilderObj = new FootnoteBuilder(sourceDBConnection, sourceDBQueries);
        //        }

        //        foreach (DataRow Row in Table.Rows)
        //        {
        //            //-- GET AREANID
        //            if (AreaList.ContainsKey(Convert.ToString(Row[Area.AreaID])))
        //            {
        //                AreaNId = AreaList[Convert.ToString(Row[Area.AreaID])];
        //            }
        //            else
        //            {
        //                AreaNId = AreaBuilderObj.GetAreaNidByAreaID(Convert.ToString(Row[Area.AreaID]));
        //                AreaList.Add(Convert.ToString(Row[Area.AreaID]), AreaNId);
        //            }

        //            if (AreaNId > 0)
        //            {
        //                //-- GET SourceNID
        //                if (SourceList.ContainsKey(Convert.ToString(Row[IndicatorClassifications.ICName])))
        //                {
        //                    SourceNId = SourceList[Convert.ToString(Row[IndicatorClassifications.ICName])];
        //                }
        //                else
        //                {
        //                    SourceNId = SourceBuilderObj.CheckNCreateSource(Convert.ToString(Row[IndicatorClassifications.ICName]));
        //                    SourceList.Add(Convert.ToString(Row[IndicatorClassifications.ICName]), SourceNId);
        //                }

        //                //-- GET TIMEPERIOD NID
        //                if (TimeperiodList.ContainsKey(Convert.ToString(Row[Timeperiods.TimePeriod])))
        //                {
        //                    TimeperiodNId = TimeperiodList[Convert.ToString(Row[Timeperiods.TimePeriod])];
        //                }
        //                else
        //                {
        //                    TimeperiodNId = TimeperiodBuilderObj.CheckNCreateTimeperiod(Convert.ToString(Row[Timeperiods.TimePeriod]));
        //                    TimeperiodList.Add(Convert.ToString(Row[Timeperiods.TimePeriod]), TimeperiodNId);
        //                }


        //                this.RaiseSetProcessEvent(Convert.ToString(Row[Indicator.IndicatorName]), Convert.ToString(Row[Unit.UnitName]), CurrentSheetNo);


        //                //-- GET IUSNID
        //                IndicatorGID = Convert.ToString(Row[Indicator.IndicatorGId]);
        //                UnitGID = Convert.ToString(Row[Unit.UnitGId]);
        //                SubgroupGID = Convert.ToString(Row[SubgroupVals.SubgroupValGId]);
        //                IndicatorName = Convert.ToString(Row[Indicator.IndicatorName]);
        //                UnitName = Convert.ToString(Row[Unit.UnitName]);
        //                SubgroupName = Convert.ToString(Row[SubgroupVals.SubgroupVal]);

        //                IUSString = IndicatorGID + "~" + UnitGID + "~" + SubgroupGID + "~" + IndicatorName + "~" + UnitName + "~" + SubgroupName;

        //                if (IUSList.ContainsKey(IUSString))
        //                {
        //                    IUSNId = IUSList[IUSString];
        //                }
        //                else
        //                {
        //                    IUSNId = IUSBuilderObj.GetIUSNid(IndicatorGID, UnitGID, SubgroupGID, IndicatorName, UnitName, SubgroupName);
        //                    IUSList.Add(IUSString, IUSNId);
        //                }

        //                if (SourceNId > 0 && TimeperiodNId > 0 && IUSNId > 0)
        //                {

        //                    Datavalue = Convert.ToString(Row[Data.DataValue]);


        //                    FootNotesNId = FootnoteBuilderObj.CheckNCreateFoonote(SourceDBFootnoteBuilderObj.GetNameByNid(Convert.ToInt32(Row[FootNotes.FootNoteNId])));
        //                    DataDenominator = Convert.ToString(Row[Data.DataDenominator]);
        //                    DataDenominator = string.IsNullOrEmpty(DataDenominator) ? "0" : DataDenominator;

        //                    if (!string.IsNullOrEmpty(Datavalue))
        //                    {
        //                        DIDatabaseObj.CheckNCreateData(AreaNId, IUSNId, SourceNId, TimeperiodNId, Datavalue, FootNotesNId, Convert.ToInt32(DataDenominator));
        //                    }
        //                }
        //            }

        //            Counter++;                 
        //            this.RaiseProgressBarIncrement(Convert.ToInt32((Counter * 100) / Table.Rows.Count));

        //        }
        //        DIDatabaseObj.UpdateIndicatorUnitSubgroupNIDsInData(); 
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionFacade.ThrowException(ex);
        //        //MessageBoxControl.ShowErrorMessage(ex);
        //    }

        //}

        #endregion

        #endregion

        #endregion

        #region "-- Internal --"

        #region"-- Variables/Properties --"

        internal DIImportFileType ImportFileType;
        internal ImportQueries DAQuery;

        /// <summary>
        /// HTMLLog class object used to create html log.
        /// </summary>
        internal HTMLLog HtmlLog = new HTMLLog();



        #endregion

        #endregion


    }


}
