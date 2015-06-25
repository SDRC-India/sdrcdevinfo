using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Import.DAImport;
using DAImportCommon = DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using System.IO;
using DevInfo.Lib.DI_LibDAL.UserSelection;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Database
{
    /// <summary>
    /// Helps in importing records from database or template
    /// </summary>
    public class Database : DIData
    {
        #region "-- private --"

        #region "-- variables --"

        private UserSelection UserSelections = null;
        private DIConnection SourceDBConnection = null;
        private DIQueries SourceDBQueries = null;
        private DIConnection TargetDBConnection = null;
        private DIQueries TargetDBQueries = null;

        #endregion

        #endregion


        #region "-- Internal --"

        #region "-- New/Dispose --"

        /// <summary>
        /// DIData Constructor specific to source as 'Database'.
        /// </summary>
        /// <param name="DBSourceFiles">List of Source Database fileName with Path.</param>
        /// <param name="targetFileNameWPath">Target Database.</param>
        /// <param name="tempFolderPath">TempFolder where all processing will be done.</param>
        /// <param name="htmlOutputPath">Outout path for Html log file.</param>
        internal Database(List<string> DBSourceFiles, string targetFileNameWPath, string tempFolderPath, string htmlOutputPath)
        {
            this.SetBasicPropertiesAndProcessValues(DBSourceFiles, targetFileNameWPath, tempFolderPath, htmlOutputPath);

        }

        internal Database(UserSelection userSelections, DIConnection sourceDBConnection, DIQueries sourceDBQueries, DIConnection targetDBConnection, DIQueries targetDBQueries)
        {

            this.UserSelections = userSelections;
            this.SourceDBConnection = sourceDBConnection;
            this.SourceDBQueries = sourceDBQueries;
            this.TargetDBConnection = targetDBConnection;
            this.TargetDBQueries = targetDBQueries;
        }

        #endregion

        #endregion

        #region " -- Protected -- "

        #region "-- Methods --"

        /// <summary>
        /// It process SourceDatabase by inserting its all UT_Data into TargetDatabase's TempDataTable.
        /// </summary>
        protected override void ProcessTempDataTable()
        {
            int MaxValueForProgressBar = 0;
            int ProgressBarValue = 0;
            int FileIndex = 1;
            string SqlString = string.Empty;
            string LangaugeCode = this.DBQueries.LanguageCode;
            DITables TablesName;

            // Create another connection to read SourceDataBase.
            DIConnection SrcDBConnection;
            OleDbConnection Connection;
            OleDbCommand Command;

            try
            {

                //Set progress bar and process info
                MaxValueForProgressBar = this.TempFiles.Count + 3;
                this.RaiseProgressBarInitializeEvent(MaxValueForProgressBar);


                // Close DB connection so that Worksheet can be import by other object.
                this.DBConnection.Dispose();

                //Increment progress bar by 1 unit.
                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                //Set progress bar and process info
                this.HideCommonProcessInfo();
                //this.RaiseProcessSourceFileNameEvent(Path.GetFileName(this._TargetFileNameWPath));

                foreach (string DBSourceFile in this.TempFiles)
                {
                    //set process info
                    this.RaiseProcessSourceFileNameEvent(Path.GetFileName(DBSourceFile));
                    //this.RaiseProcessSheetnameEvent(Path.GetFileName(DBSourceFile));
                    this.RaiseProcessFileNoEvent(FileIndex, this.TempFiles.Count);

                    // Create instance of DITables 
                    // check language code exists in source database or not
                    SrcDBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, DBSourceFile, string.Empty, string.Empty);
                    if (!SrcDBConnection.IsValidDILanguage(SrcDBConnection.DIDataSetDefault(), LangaugeCode))
                    {
                        // if not then get the default language code form source database
                        LangaugeCode = SrcDBConnection.DILanguageCodeDefault(SrcDBConnection.DIDataSetDefault());
                    }

                    // initialize instance of DITables
                    TablesName = new DITables(SrcDBConnection.DIDataSetDefault(), LangaugeCode);

                    // dispose source Database DIConnection object
                    SrcDBConnection.Dispose();





                    string ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data source=" + DBSourceFile + ";Jet OLEDB:Database Password=" + DAImportCommon.Constants.DBPassword + ";Persist Security Info=False;";
                    Connection = new OleDbConnection(ConnectionString);
                    Command = new OleDbCommand();
                    Command.Connection = Connection;
                    Connection.Open();



                    // 1. Insert Source Database into targetDatabase.TempDataTable
                    SqlString = "INSERT INTO [MS Access;Database=" + this.TempTargetFile + ";pwd=" + DAImportCommon.Constants.DBPassword + ";].[" + DAImportCommon.Constants.TempDataTableName + "](" + DAImportCommon.Constants.Old_Data_Nid + ", " + Data.DataValue + ", " + Area.AreaID + ", " + Area.AreaName + ", " + FootNotes.FootNote + ", " + Timeperiods.TimePeriod + ", " + DAImportCommon.Constants.SourceColumnName + ", " + Indicator.IndicatorName + ", " + Indicator.IndicatorGId + ", " + Unit.UnitName + ", " + Unit.UnitGId + ", " + SubgroupVals.SubgroupVal + ", " + SubgroupVals.SubgroupValGId + ", " + DAImportCommon.Constants.Log.SkippedSourceFileColumnName + "," + IndicatorClassifications.ICGlobal + ", " + IndicatorClassificationsIUS.RecommendedSource + "," + Data.ICIUSOrder + " ) " +
             " SELECT D." + Data.DataNId + ", D." + Data.DataValue + ", A." + Area.AreaID + ", A." + Area.AreaName + ", F." + FootNotes.FootNote + ", T." + Timeperiods.TimePeriod + ", IC." + IndicatorClassifications.ICName + ", I." + Indicator.IndicatorName + ", I." + Indicator.IndicatorGId + ", U." + Unit.UnitName + ", U." + Unit.UnitGId + ", SG." + SubgroupVals.SubgroupVal + ", SG." + SubgroupVals.SubgroupValGId + ", '" + DICommon.RemoveQuotes(Path.GetFileName(DBSourceFile)) + "', " + IndicatorClassifications.ICGlobal + ", ICIUS." + IndicatorClassificationsIUS.RecommendedSource + ",D." + Data.ICIUSOrder
             + " FROM " + TablesName.IndicatorClassificationsIUS + " AS ICIUS INNER JOIN (" +

TablesName.TimePeriod + " AS T INNER JOIN (" + TablesName.Indicator + " AS I INNER JOIN (" + TablesName.Unit + " AS U INNER JOIN (" + TablesName.SubgroupVals + " AS SG INNER JOIN (" + TablesName.IndicatorUnitSubgroup + " AS IUS INNER JOIN (" + TablesName.IndicatorClassifications + " AS IC INNER JOIN (" + TablesName.FootNote + " AS F INNER JOIN (" + TablesName.Area + " AS A INNER JOIN " + TablesName.Data + " AS D " + " ON A." + Area.AreaNId + "= D." + Area.AreaNId + ") ON F." + FootNotes.FootNoteNId + "= D." + FootNotes.FootNoteNId + ") ON IC." + IndicatorClassifications.ICNId + " = D." + Data.SourceNId + ") ON IUS." + Indicator_Unit_Subgroup.IUSNId + " = D." + Data.IUSNId + ") ON SG." + SubgroupVals.SubgroupValNId + "= IUS." + Indicator_Unit_Subgroup.SubgroupValNId + ") ON U." + Unit.UnitNId + " = IUS." + Indicator_Unit_Subgroup.UnitNId + ") ON I." + Indicator.IndicatorNId + " = IUS." + Indicator_Unit_Subgroup.IndicatorNId + ") ON T." + Timeperiods.TimePeriodNId + " = D." + Timeperiods.TimePeriodNId + ""
+ ") ON (D.Source_NId = ICIUS.IC_NId) AND (D.IUSNId = ICIUS.IUSNId)"
;


                    Command.CommandText = SqlString;

                    // --Increment progress bar by 1 unit.
                    ProgressBarValue += 1;
                    this.RaiseProgressBarIncrementEvent(ProgressBarValue);

                    Command.ExecuteNonQuery();

                    //  update IC_IUS_Label in Temp data table
                    SqlString = " UPDATE  [MS Access;Database=" + this.TempTargetFile + ";pwd=" + DAImportCommon.Constants.DBPassword + ";].[" + DAImportCommon.Constants.TempDataTableName + "] As TD INNER JOIN " + TablesName.RecommendedSources + " AS R ON TD." + DAImportCommon.Constants.Old_Data_Nid + " = R." + RecommendedSources.DataNId + " SET TD." + RecommendedSources.ICIUSLabel + "=R." + RecommendedSources.ICIUSLabel + " where TD." + DAImportCommon.Constants.Log.SkippedSourceFileColumnName + " ='" + DICommon.RemoveQuotes(Path.GetFileName(DBSourceFile)) + "'";

                    Command.CommandText = SqlString;
                    Command.ExecuteNonQuery();


                    Connection.Close();

                    // 2. Connect again old DBconnection.
                    this.ConnectToDatabase();

                    // 3. Update Database filename in TempData table
                    //this.DBConnection.ExecuteNonQuery(this.DAQuery.UpdateSourceFileNameInTempSheetTable(Path.GetFileName(DBSourceFile)));
                    FileIndex += 1;
                }

                // --Increment progress bar by 1 unit.
                ProgressBarValue += 1;
                this.RaiseProgressBarIncrementEvent(ProgressBarValue);


                // Step 2: Remove all rows from Temp_Data table where Data Value is null or blank, or I, U, S are blank.
                this.DBConnection.ExecuteNonQuery(DAQuery.RemoveNullRecordsFromTempDataTable());

                //set process info :hide source file and file no
                this.RaiseHideSheetNameEvent();
                this.RaiseHideSheetOrFileNoEvent();




            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        /// <summary>
        /// It adds matched Notes and Assistance from Source Database into Target Database.
        /// </summary>
        protected override void AddNotesAssistants()
        {
            // 1. Add Notes
            NotesImporter NotesImport = new NotesImporter(this._DBConnection, this._DBQueries, this.TempFolderPath);
            NotesImport.ImportNotes();

            // 2. Add Assistants
            this.ConnectToDatabase();
            AssistantImporter AssistantImport = new AssistantImporter(this._DBConnection, this._DBQueries, this.SourceFileNamesWPath);
            AssistantImport.ImportAssistants();

        }

        protected override bool UploadDatabase()
        {
            bool RetVal = false;

            DataTable Table = null;
            IndicatorBuilder IndicatorBuilderObj = null;
            IUSBuilder IUSBuilderObj = null;
            AreaBuilder AreaBuilderObj = null;
            SourceBuilder SourceBuilderObj = null;
            TimeperiodBuilder TimeperiodBuilderObj = null;
            FootnoteBuilder FootnoteBuilderObj = null;
            FootnoteBuilder SourceDBFootnoteBuilderObj = null;

            Dictionary<string, int> AreaList = new Dictionary<string, int>();
            Dictionary<string, int> SourceList = new Dictionary<string, int>();
            Dictionary<string, int> TimeperiodList = new Dictionary<string, int>();
            Dictionary<string, int> IUSList = new Dictionary<string, int>();

            int AreaNId = 0;
            int SourceNId = 0;
            int TimeperiodNId = 0;
            int IUSNId = 0;
            string IndicatorGID = string.Empty;
            string UnitGID = string.Empty;
            string SubgroupGID = string.Empty;
            string IndicatorName = string.Empty;
            string UnitName = string.Empty;
            string SubgroupName = string.Empty;
            string IUSString = string.Empty;
            string Datavalue = string.Empty;
            int FootNotesNId = -1;
            string DataDenominator = string.Empty;
            int SheetCount = 0;
            int Counter = 0;
            int CurrentSheetNo = 0;

            try
            {

                //-- Copy File INto Temp Location
                Table = SourceDBConnection.ExecuteDataTable(this.SourceDBQueries.Data.GetDataNIDs(UserSelections.IndicatorNIds, UserSelections.TimePeriodNIds, UserSelections.AreaNIds, UserSelections.SourceNIds, UserSelections.ShowIUS, FieldSelection.Heavy, false));

                DevInfo.Lib.DI_LibBAL.DA.DML.DIDatabase DIDatabaseObj = new DevInfo.Lib.DI_LibBAL.DA.DML.DIDatabase(this.TargetDBConnection, this.TargetDBQueries);

                //-- Initialize Progress
                this.RaiseInitilizeProcessEvent(Path.GetFileName(this.SourceDBConnection.ConnectionStringParameters.DbName), SheetCount, 100);

                if (Table.Rows.Count > 0)
                {
                    IndicatorBuilderObj = new IndicatorBuilder(this.TargetDBConnection, this.TargetDBQueries);
                    IUSBuilderObj = new IUSBuilder(this.TargetDBConnection, this.TargetDBQueries);
                    AreaBuilderObj = new AreaBuilder(this.TargetDBConnection, this.TargetDBQueries);
                    SourceBuilderObj = new SourceBuilder(this.TargetDBConnection, this.TargetDBQueries);
                    TimeperiodBuilderObj = new TimeperiodBuilder(this.TargetDBConnection, this.TargetDBQueries);
                    FootnoteBuilderObj = new FootnoteBuilder(this.TargetDBConnection, this.TargetDBQueries);
                    //-- Get Footnotes from Source Database
                    SourceDBFootnoteBuilderObj = new FootnoteBuilder(SourceDBConnection, SourceDBQueries);
                }

                foreach (DataRow Row in Table.Rows)
                {
                    //-- GET AREANID
                    if (AreaList.ContainsKey(Convert.ToString(Row[Area.AreaID])))
                    {
                        AreaNId = AreaList[Convert.ToString(Row[Area.AreaID])];
                    }
                    else
                    {
                        AreaNId = AreaBuilderObj.GetAreaNidByAreaID(Convert.ToString(Row[Area.AreaID]));
                        AreaList.Add(Convert.ToString(Row[Area.AreaID]), AreaNId);
                    }

                    if (AreaNId > 0)
                    {
                        //-- GET SourceNID
                        if (SourceList.ContainsKey(Convert.ToString(Row[IndicatorClassifications.ICName])))
                        {
                            SourceNId = SourceList[Convert.ToString(Row[IndicatorClassifications.ICName])];
                        }
                        else
                        {
                            SourceNId = SourceBuilderObj.CheckNCreateSource(Convert.ToString(Row[IndicatorClassifications.ICName]));
                            SourceList.Add(Convert.ToString(Row[IndicatorClassifications.ICName]), SourceNId);
                        }

                        //-- GET TIMEPERIOD NID
                        if (TimeperiodList.ContainsKey(Convert.ToString(Row[Timeperiods.TimePeriod])))
                        {
                            TimeperiodNId = TimeperiodList[Convert.ToString(Row[Timeperiods.TimePeriod])];
                        }
                        else
                        {
                            TimeperiodNId = TimeperiodBuilderObj.CheckNCreateTimeperiod(Convert.ToString(Row[Timeperiods.TimePeriod]));
                            TimeperiodList.Add(Convert.ToString(Row[Timeperiods.TimePeriod]), TimeperiodNId);
                        }


                        this.RaiseSetProcessEvent(Convert.ToString(Row[Indicator.IndicatorName]), Convert.ToString(Row[Unit.UnitName]), CurrentSheetNo);


                        //-- GET IUSNID
                        IndicatorGID = Convert.ToString(Row[Indicator.IndicatorGId]);
                        UnitGID = Convert.ToString(Row[Unit.UnitGId]);
                        SubgroupGID = Convert.ToString(Row[SubgroupVals.SubgroupValGId]);
                        IndicatorName = Convert.ToString(Row[Indicator.IndicatorName]);
                        UnitName = Convert.ToString(Row[Unit.UnitName]);
                        SubgroupName = Convert.ToString(Row[SubgroupVals.SubgroupVal]);

                        IUSString = IndicatorGID + "~" + UnitGID + "~" + SubgroupGID + "~" + IndicatorName + "~" + UnitName + "~" + SubgroupName;

                        if (IUSList.ContainsKey(IUSString))
                        {
                            IUSNId = IUSList[IUSString];
                        }
                        else
                        {
                            IUSNId = IUSBuilderObj.GetIUSNid(IndicatorGID, UnitGID, SubgroupGID, IndicatorName, UnitName, SubgroupName);
                            IUSList.Add(IUSString, IUSNId);
                        }

                        if (SourceNId > 0 && TimeperiodNId > 0 && IUSNId > 0)
                        {

                            Datavalue = Convert.ToString(Row[Data.DataValue]);


                            FootNotesNId = FootnoteBuilderObj.CheckNCreateFoonote(SourceDBFootnoteBuilderObj.GetNameByNid(Convert.ToInt32(Row[FootNotes.FootNoteNId])));
                            DataDenominator = Convert.ToString(Row[Data.DataDenominator]);
                            DataDenominator = string.IsNullOrEmpty(DataDenominator) ? "0" : DataDenominator;

                            if (!string.IsNullOrEmpty(Datavalue))
                            {
                                DIDatabaseObj.CheckNCreateData(AreaNId, IUSNId, SourceNId, TimeperiodNId, Datavalue, FootNotesNId, Convert.ToInt32(DataDenominator));
                            }
                        }
                    }

                    Counter++;
                    this.RaiseProgressBarIncrement(Convert.ToInt32((Counter * 100) / Table.Rows.Count));

                }
                DIDatabaseObj.UpdateIndicatorUnitSubgroupNIDsInData();
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        # endregion

        # endregion


    }
}
