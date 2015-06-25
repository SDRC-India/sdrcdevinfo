using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Data;
using System.IO;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.DA.DML;
using DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL;
using DevInfo.Lib.DI_LibBAL.UI.DataViewPage;
using DevInfo.Lib.DI_LibBAL.UI.UserPreference;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.Converter.Database;

namespace DevInfo.Lib.DI_LibBAL.Export
{
    internal class ExportOnlineDatabase
    {
        #region "Inner Classes"

        #region "Private"

        #endregion

        #region "Variables"

        //-- Varibles to store NIDs from userSelections.
        private UserSelection UserSelection = null;
        string IndicatorNIDs = string.Empty;
        string UnitNIDs = string.Empty;
        string SubgroupValNIDs = string.Empty;
        string IUSNIDs = string.Empty;
        string AreaNIDs = string.Empty;
        string TimePeriodNIDs = string.Empty;
        string SourceNIDs = string.Empty;
        private bool ApplyMRD = false;
        private const string TEMP_TABLE_Data = "TEMP_Data";

        //-- Souce Database DIConnection object
        private DIConnection SourceDBConnection = null;
        private DIQueries SourceDBQueries = null;
        private DIConnection DestDBConnection = null;
        private DIQueries DestDBQueries = null;
        private DITables DBTableNames = null;
        private DITables SourceTableNames = null;
        private System.Data.DataTable LanguageTable = null;

        private BaseLookInSource LookInSource = null;


        #endregion

        #region "Methods"

        private BaseLookInSource GetLookInImporterObject(ElementImportType elementType)
        {
            // It returns BaseLookInSource which is used to export main database master Contents 
            // like Indicators, Unit, Subgroups, IUS, ICs, timePeriods.

            BaseLookInSource retVal = DevInfo.Lib.DI_LibBAL.Controls.LookInWindowBAL.ListViewFactory.CreateInstance(elementType);
            retVal.ImportFileType = DataSourceType.Template;
            retVal.IncludeArea = true;
            retVal.IncludeMap = true;
            retVal.ShowIUS = true;
            retVal.ShowSector = true;
            retVal.SourceDBConnection = this.SourceDBConnection;
            retVal.SourceDBQueries = this.SourceDBQueries;
            retVal.TargetDBConnection = this.DestDBConnection;
            retVal.TargetDBQueries = this.DestDBQueries;
            retVal.SetColumnsInfo();
            retVal.SourceTable = retVal.GetDataTable("");

            return retVal;
        }

        private string GetConnectionStringForSQLSyntax(DIConnection dbConnection)
        {
            string ConnStringClause = string.Empty;

            if (dbConnection != null)
            {
                switch (dbConnection.ConnectionStringParameters.ServerType)
                {
                    case DIServerType.SqlServer:
                    case DIServerType.SqlServerExpress:
                        // InBuild ConnectionString used in SQL Query should be in format:-
                        // [odbc;driver={SQL Server};server=MYSERVER;database=MYDB;uid=myuser;pwd=mypass].[TableName]" +
                        ConnStringClause = "[odbc;driver={SQL Server};" +
                            "server=" + dbConnection.ConnectionStringParameters.ServerName + ";" +
                            "database=" + dbConnection.ConnectionStringParameters.DbName + ";" +
                            "uid=" + dbConnection.ConnectionStringParameters.UserName + ";" +
                            "pwd=" + dbConnection.ConnectionStringParameters.Password + ";]";
                        break;

                    case DIServerType.MySql:
                        // for MySQL:-
                        //DRIVER={MySQL ODBC 3.51 Driver};SERVER=server;PORT=3306;DATABASE=myDb;UID=uid;PWD=pass;OPTION=3;
                        ConnStringClause = "[odbc;DRIVER={MySQL ODBC 3.51 Driver};" +
                            "SERVER=" + dbConnection.ConnectionStringParameters.ServerName + ";" +
                            "port=" + dbConnection.ConnectionStringParameters.PortNo + ";" +
                            "DATABASE=" + dbConnection.ConnectionStringParameters.DbName + ";" +
                            "UID=" + dbConnection.ConnectionStringParameters.UserName + ";" +
                            "PASSWORD=" + dbConnection.ConnectionStringParameters.Password + ";OPTION=3;]";

                        break;
                }
            }

            return ConnStringClause;
        }

        #region " Indicator, Unit, Subgroup, IUS, IC, Area, TimePeriod"

        private void ProcessIndicators()
        {
            try
            {
                if (string.IsNullOrEmpty(this.IndicatorNIDs) == false)
                {
                    //- Get IUSNId for given Indicators.
                    string NewIUSNIds = Common.GetCommaSeperatedString(this.SourceDBConnection.ExecuteDataTable(this.SourceDBQueries.IUS.GetIUSByI_U_S(this.IndicatorNIDs, this.UnitNIDs, this.SubgroupValNIDs)), Indicator_Unit_Subgroup.IUSNId);

                    //-- Merge NewIUS with given IUS.
                    this.IUSNIDs = Common.MergeNIds(this.IUSNIDs, NewIUSNIds);
                }

                if (string.IsNullOrEmpty(this.IUSNIDs) == false)
                {
                    IUSBuilder SourceIUSBuilder = new IUSBuilder(this.SourceDBConnection, this.SourceDBQueries);
                    IUSBuilder DestinationIUSBuilder = new IUSBuilder(this.DestDBConnection, this.DestDBQueries);
                    IUSInfo SourceIUSInfo = null;

                    // import each ius 
                    foreach (string SourceIUSNid in DICommon.SplitString(this.IUSNIDs, ","))
                    {
                        // 1. get source ius info 
                        SourceIUSInfo = SourceIUSBuilder.GetIUSInfo(FilterFieldType.NId, SourceIUSNid, FieldSelection.Light);

                        if (SourceIUSInfo != null && SourceIUSInfo.Nid > 0)
                        {
                            // 2. import ius into destination db
                            DestinationIUSBuilder.ImportIUS(SourceIUSInfo, this.SourceDBQueries, this.SourceDBConnection);

                        }
                    }

                }
            }
            catch
            {
            }
        }

        private void ProcessIndicatorClassifications()
        {
            try
            {
                if (string.IsNullOrEmpty(this.IUSNIDs) == false)
                {
                    //- Get ICNIds related to given IUSNIds
                    DataTable DTICs = this.SourceDBConnection.ExecuteDataTable(this.SourceDBQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.None, string.Empty, this.IUSNIDs, FieldSelection.Light));

                    //- Sector
                    this.ProcessIC(ICType.Sector, DTICs);

                    //- Goals
                    this.ProcessIC(ICType.Goal, DTICs);

                    //- CF
                    this.ProcessIC(ICType.CF, DTICs);

                    //- Themes
                    this.ProcessIC(ICType.Theme, DTICs);

                    //- Institutions
                    this.ProcessIC(ICType.Institution, DTICs);

                    //- Convention
                    this.ProcessIC(ICType.Convention, DTICs);

                    //- Sources
                    this.ProcessIC(ICType.Source, DTICs);
                }
            }
            catch
            {
                throw;
            }
        }

        private void ProcessIC(ICType icType, DataTable ICTable)
        {
            ElementImportType ICElementType = ElementImportType.Sector;
            string ICNIds = string.Empty;

            switch (icType)
            {
                case ICType.Goal:
                    ICElementType = ElementImportType.Goal;
                    ICTable.DefaultView.RowFilter = IndicatorClassifications.ICType + " IN (" + DIQueries.ICTypeText[icType] + ")";
                    break;
                case ICType.Sector:
                    ICElementType = ElementImportType.Sector;
                    ICTable.DefaultView.RowFilter = IndicatorClassifications.ICType + " IN (" + DIQueries.ICTypeText[icType] + ")";
                    break;
                case ICType.CF:
                    ICTable.DefaultView.RowFilter = IndicatorClassifications.ICType + " IN (" + DIQueries.ICTypeText[icType] + ")";
                    ICElementType = ElementImportType.Framework;
                    break;
                case ICType.Theme:
                    ICElementType = ElementImportType.Theme;
                    ICTable.DefaultView.RowFilter = IndicatorClassifications.ICType + " IN (" + DIQueries.ICTypeText[icType] + ")";
                    break;
                case ICType.Source:
                    ICElementType = ElementImportType.Source;
                    ICTable.DefaultView.RowFilter = IndicatorClassifications.ICType + " IN (" + DIQueries.ICTypeText[icType] + ") AND " + IndicatorClassifications.ICParent_NId + " NOT IN (-1)";
                    break;
                case ICType.Institution:
                    ICElementType = ElementImportType.Institution;
                    ICTable.DefaultView.RowFilter = IndicatorClassifications.ICType + " IN (" + DIQueries.ICTypeText[icType] + ")";
                    break;
                case ICType.Convention:
                    ICElementType = ElementImportType.Convention;
                    ICTable.DefaultView.RowFilter = IndicatorClassifications.ICType + " IN (" + DIQueries.ICTypeText[icType] + ")";
                    break;
            }

            // Get  ICNId on the basis of ICType
            ICNIds = DIConnection.GetDelimitedValuesFromDataView(ICTable.DefaultView, IndicatorClassifications.ICNId);

            if (string.IsNullOrEmpty(ICNIds) == false)
            {
                //- Initialize LookInImport object (used to export ICs into another database)
                this.LookInSource = this.GetLookInImporterObject(ICElementType);

                //- Export ICs associated with IUSNId.
                List<string> IC_List = new List<string>(ICNIds.Split(','));
                this.LookInSource.ImportValues(IC_List, false);
            }
        }

        private void ProcessAreas()
        {
            if (string.IsNullOrEmpty(this.AreaNIDs) == false)
            {
                //- Initialize LookInSource library object for ElementImportType - Area
                this.LookInSource = this.GetLookInImporterObject(ElementImportType.Area);

                //- Export Areas to target Databases
                List<string> Area_List = new List<string>(this.AreaNIDs.Split(','));
                this.LookInSource.ImportValues(Area_List, false);
            }
        }

        private void ProcessTimePeriods()
        {
            if (string.IsNullOrEmpty(this.TimePeriodNIDs) == false)
            {
                //- Initialize LookInSource library object for ElementImportType - TimePeriods
                this.LookInSource = this.GetLookInImporterObject(ElementImportType.Timeperiod);

                //- Export timePeriod to target Databases.
                List<string> TimePeriod_List = new List<string>(this.TimePeriodNIDs.Split(','));
                this.LookInSource.ImportValues(TimePeriod_List, false);
            }
        }

        #endregion

        #region " UT_Data Table"

        private bool ExportData()
        {
            bool RetVal = false;

            try
            {
                UserPreference TempUserPref = new UserPreference();
                TempUserPref.UserSelection = this.UserSelection;

                //- Insert SourceDatabase UT_Data table into Temp table in Target Database
                this.InsertDataTableIntoTemp();
                DIExport.RaiseExportProgressBarIncrement(45);

                //- Delete from UT_data by DataView Filters if present in UserSelection
                this.DeleteFromDataByDataFilters();
                DIExport.RaiseExportProgressBarIncrement(50);

                //- Update New NIds of Indicator, Unit, Subgroup in temp Data Table
                this.UpdateNewI_U_S_NIds();
                DIExport.RaiseExportProgressBarIncrement(55);

                //- Update NewNId of IUS in temp Data Table
                this.UpdateNewIUSNIds();
                DIExport.RaiseExportProgressBarIncrement(60);

                //- Update NewTimePeriodNId in temp Data Table
                this.UpdateNewTimePeriodNId();
                DIExport.RaiseExportProgressBarIncrement(65);

                //- Update NewSourceNId in temp Data Table
                this.UpdateNewSourceNIds();
                DIExport.RaiseExportProgressBarIncrement(70);

                //- Insert Records from Temp Data into Main UT_Data
                this.InsertDataFromTempIntoMain();
                DIExport.RaiseExportProgressBarIncrement(75);

                this.InsertSourceNId_IUSNIdRelationship();
                DIExport.RaiseExportProgressBarIncrement(80);
            }
            catch
            {
                throw;
            }

            return RetVal;
        }

        private void InsertDataTableIntoTemp()
        {
            string Sql = string.Empty;
            string ConnStringClause = string.Empty;

            try
            {
                //Sql = "SELECT D.*, I." + Indicator.IndicatorGId + ", U." + Unit.UnitGId + ", S." + SubgroupVals.SubgroupValGId + ", IC." + IndicatorClassifications.ICName + ", A." + Area.AreaID + ", T." + Timeperiods.TimePeriod +
                //" INTO [" + TEMP_TABLE_Data + "] FROM ";

                //- Insert required data from UT_Data into Source Database's Temp table "Temp_Data"
                Sql = "SELECT D.*, I." + Indicator.IndicatorGId + ", U." + Unit.UnitGId + ", S." + SubgroupVals.SubgroupValGId + ", IC." + IndicatorClassifications.ICName + ", A." + Area.AreaID + ", T." + Timeperiods.TimePeriod +
     " FROM " + this.SourceTableNames.Data + " AS D, " + this.SourceTableNames.Indicator + " AS I, " + this.SourceTableNames.Unit + " AS U, " +
                 this.SourceTableNames.SubgroupVals + " AS S, " + this.SourceTableNames.IndicatorClassifications + " AS IC, " + this.SourceTableNames.Area + " AS A, " +
                 this.SourceTableNames.TimePeriod + " AS T ";

                Sql += "  WHERE D." + Data.IndicatorNId + "= I." + Indicator.IndicatorNId + " AND U." + Unit.UnitNId + " = D." + Data.UnitNId +
                    " AND S." + SubgroupVals.SubgroupValNId + " = D." + Data.SubgroupValNId + " AND IC." + IndicatorClassifications.ICNId + " = D." + Data.SourceNId +
                    " AND D." + Data.AreaNId + " = A." + Area.AreaNId + " AND T." + Timeperiods.TimePeriodNId + " = D." + Data.TimePeriodNId +
                    " AND D." + Data.IUSNId + " IN (" + this.IUSNIDs + ")";

                if (string.IsNullOrEmpty(this.AreaNIDs) == false)
                {
                    Sql += " AND D." + Data.AreaNId + " IN ( " + this.AreaNIDs + ")";
                }

                if (string.IsNullOrEmpty(this.TimePeriodNIDs) == false)
                {
                    Sql += " AND D." + Data.TimePeriodNId + " IN ( " + this.TimePeriodNIDs + ")";
                }

                if (string.IsNullOrEmpty(this.SourceNIDs) == false)
                {
                    Sql += " AND D." + Data.SourceNId + " IN ( " + this.SourceNIDs + ")";
                }

                //- Get all records from "Temp_Data" in a DataTable
                DataTable DtSourceData = this.SourceDBConnection.ExecuteDataTable(Sql);

                if (this.UserSelection.DataViewFilters != null)
                {
                    try
                    {
                        //- Delete from TEMP_Data for DataNIds filters
                        if (string.IsNullOrEmpty(this.UserSelection.DataViewFilters.DeletedDataNIds) == false)
                        {
                            DataRow[] RowsToDelete = DtSourceData.Select(Data.DataNId + " IN (" + this.UserSelection.DataViewFilters.DeletedDataNIds + ")");
                            foreach (DataRow dr in RowsToDelete)
                            {
                                dr.Delete();
                            }
                            DtSourceData.AcceptChanges();
                        }
                    }
                    catch
                    { }
                }

                //- Insert dataTable into Target Database's "Temp_Data" using Adapter.Fill
                System.Data.Common.DbCommandBuilder CmdBuilder = this.DestDBConnection.GetCurrentDBProvider().CreateCommandBuilder();

                System.Data.Common.DbDataAdapter Adpt = this.DestDBConnection.GetCurrentDBProvider().CreateDataAdapter();
                CmdBuilder.DataAdapter = Adpt;

                System.Data.Common.DbCommand cmd = this.DestDBConnection.GetConnection().CreateCommand();
                cmd.Connection = this.DestDBConnection.GetConnection();

                //- Create a Temp Table "Temp_Data" in Target Database 
                // having same structure as Source's "Temp_Data"
                Sql = "SELECT D.*, I." + Indicator.IndicatorGId + ", U." + Unit.UnitGId + ", S." + SubgroupVals.SubgroupValGId + ", IC." + IndicatorClassifications.ICName + ", A." + Area.AreaID + ", T." + Timeperiods.TimePeriod +
                " INTO " + TEMP_TABLE_Data + " FROM " + this.DBTableNames.Data + " AS D, " + this.DBTableNames.Indicator + " AS I, " + this.DBTableNames.Unit + " AS U, " +
                this.DBTableNames.SubgroupVals + " AS S, " + this.DBTableNames.IndicatorClassifications + " AS IC, " + this.DBTableNames.Area + " AS A, " +
                this.DBTableNames.TimePeriod + " AS T ";

                Sql += " WHERE D." + Data.IndicatorNId + "= I." + Indicator.IndicatorNId + " AND U." + Unit.UnitNId + " = D." + Data.UnitNId +
                    " AND S." + SubgroupVals.SubgroupValNId + " = D." + Data.SubgroupValNId + " AND IC." + IndicatorClassifications.ICNId + " = D." + Data.SourceNId +
                    " AND D." + Data.AreaNId + " = A." + Area.AreaNId + " AND T." + Timeperiods.TimePeriodNId + " = D." + Data.TimePeriodNId;

                Sql += " AND 1=0";

                this.DestDBConnection.ExecuteNonQuery(Sql);

                //- Get DataTable structure from "Temp_Data"
                Sql = "SELECT * FROM  " + TEMP_TABLE_Data;
                DataTable DataTableToFill = this.DestDBConnection.ExecuteDataTable(Sql);

                cmd.CommandText = Sql;

                Adpt.SelectCommand = cmd;
                Adpt.InsertCommand = CmdBuilder.GetInsertCommand();
                Adpt.FillLoadOption = LoadOption.OverwriteChanges;
                Adpt.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                Adpt.FillSchema(DataTableToFill, SchemaType.Source);

                Adpt.AcceptChangesDuringUpdate = true;

                //- Setting Source DataTable Name same as Target DataTable to Fill.
                DtSourceData.TableName = DataTableToFill.TableName;

                foreach (DataRow Row in DtSourceData.Rows)
                {
                    Row.SetAdded();
                }

                //- Update SourceDataTable into Target Database Table.
                Adpt.Update(DtSourceData);
            }
            catch (Exception ex)
            {
            }
            finally
            {
            }
        }

        private void UpdateNewI_U_S_NIds()
        {
            string Sql = string.Empty;

            //- Update IndicatorNid in Temp table for matched Indicator_GID
            Sql = "Update " + TEMP_TABLE_Data + " AS D, " + this.DBTableNames.Indicator + " AS I " +
            " SET D." + Data.IndicatorNId + " = I." + Indicator.IndicatorNId +
            " WHERE D." + Indicator.IndicatorGId + " = I." + Indicator.IndicatorGId;

            this.DestDBConnection.ExecuteNonQuery(Sql);

            //- Update UnitNId in Temp table for matched UnitGId
            Sql = "Update " + TEMP_TABLE_Data + " AS D, " + this.DBTableNames.Unit + " AS U " +
            " SET D." + Data.UnitNId + " = U." + Unit.UnitNId +
            " WHERE D." + Unit.UnitGId + " = U." + Unit.UnitGId;

            this.DestDBConnection.ExecuteNonQuery(Sql);

            //- Update SubgroupValNId in Temp table for matched SubgroupVal_GID
            Sql = "Update " + TEMP_TABLE_Data + " AS D, " + this.DBTableNames.SubgroupVals + " AS S " +
            " SET D." + Data.SubgroupValNId + " = S." + SubgroupVals.SubgroupValNId +
            " WHERE D." + SubgroupVals.SubgroupValGId + " = S." + SubgroupVals.SubgroupValGId;

            this.DestDBConnection.ExecuteNonQuery(Sql);


            //- Update AreaNId in Temp table for matched Area Id
            Sql = "Update " + TEMP_TABLE_Data + " AS D, " + this.DBTableNames.Area + " AS A " +
            " SET D." + Data.AreaNId + " = A." + Area.AreaNId +
            " WHERE D." + Area.AreaID + " = A." + Area.AreaID;

            this.DestDBConnection.ExecuteNonQuery(Sql);
        }

        private void UpdateNewIUSNIds()
        {
            string Sql = string.Empty;

            //- Update IUSNId in Temp table for matched IndicatorNid, UnitNid, SubgroupsNId
            Sql = "Update " + TEMP_TABLE_Data + " AS D, " + this.DBTableNames.IndicatorUnitSubgroup + " AS IUS " +
            " SET D." + Data.IUSNId + " = IUS." + Indicator_Unit_Subgroup.IUSNId +
            " WHERE D." + Data.IndicatorNId + " = IUS." + Indicator_Unit_Subgroup.IndicatorNId +
            " AND D." + Data.UnitNId + " = IUS." + Indicator_Unit_Subgroup.UnitNId +
            " AND D." + Data.SubgroupValNId + " = IUS." + Indicator_Unit_Subgroup.SubgroupValNId;

            this.DestDBConnection.ExecuteNonQuery(Sql);
        }

        private void UpdateNewTimePeriodNId()
        {
            string Sql = string.Empty;

            //- Update timeperiodNId in Temp table for matched timeperiod
            Sql = "Update " + TEMP_TABLE_Data + " AS D, " + this.DBTableNames.TimePeriod + " AS T " +
            " SET D." + Data.TimePeriodNId + " = T." + Timeperiods.TimePeriodNId +
            " WHERE D." + Timeperiods.TimePeriod + " = T." + Timeperiods.TimePeriod;

            this.DestDBConnection.ExecuteNonQuery(Sql);
        }

        private void UpdateNewSourceNIds()
        {
            string Sql = string.Empty;

            //- Update SourceNId in Temp table for matched ICGId
            Sql = "Update " + TEMP_TABLE_Data + " AS D, " + this.DBTableNames.IndicatorClassifications + " AS IC " +
            " SET D." + Data.SourceNId + " = IC." + IndicatorClassifications.ICNId +
            " WHERE D." + IndicatorClassifications.ICName + " = IC." + IndicatorClassifications.ICName;

            this.DestDBConnection.ExecuteNonQuery(Sql);
        }

        private void InsertDataFromTempIntoMain()
        {
            string Sql = string.Empty;
            StringBuilder Sb = new StringBuilder();


            ////- Insert data from TempData Table into UT_Data
            ////Sql = " Insert Into " + this.DBTableNames.Data + " SELECT D." + Data.DataNId + ", D." + Data.IUSNId + ", D." + Data.TimePeriodNId + ", D." + Data.AreaNId + ", D." + Data.DataValue + ", D." + Data.StartDate + ", D." + Data.EndDate +
            ////    ", D." + Data.DataDenominator + ", D." + Data.FootNoteNId + ", D." + Data.SourceNId + ", D." + Data.IndicatorNId + ", D." + Data.UnitNId + ", D." + Data.SubgroupValNId +
            ////" FROM " + TEMP_TABLE_Data + " AS D";


            //- Insert data from TempData Table into UT_Data
            Sb.Append("Insert Into " + this.DBTableNames.Data);

            Sb.Append(" SELECT D." + Data.IUSNId + ", D." + Data.TimePeriodNId + ", D." + Data.AreaNId + ", D.");

            Sb.Append(Data.DataValue + ", D." + Data.StartDate + ", D." + Data.EndDate);

            Sb.Append(", D." + Data.DataDenominator + ", D." + Data.FootNoteNId + ", D." + Data.SourceNId + ", D." + Data.IndicatorNId + ", D.");
            Sb.Append(Data.UnitNId + ", D." + Data.SubgroupValNId);

            Sb.Append(" FROM " + TEMP_TABLE_Data + " AS D");

            Sb.Append(" WHERE NOT EXISTS ( SELECT DT." + Data.IUSNId + ", DT." + Data.TimePeriodNId + ", DT." + Data.AreaNId + ", DT.");

            Sb.Append(Data.DataValue + ", DT." + Data.StartDate + ", DT." + Data.EndDate);

             Sb.Append(", D." + Data.DataDenominator + ", DT." + Data.FootNoteNId + ", DT." + Data.SourceNId + ", DT." + Data.IndicatorNId + ", DT." + Data.UnitNId + ", DT." + Data.SubgroupValNId);

            Sb.Append(" FROM  " + this.DBTableNames.Data + " AS DT WHERE ");

            Sb.Append(" D." + Data.IUSNId + "= DT." + Data.IUSNId);

            Sb.Append(" AND D." + Data.TimePeriodNId + "= DT." + Data.TimePeriodNId + " AND ");

            Sb.Append(" D." + Data.AreaNId + "= DT." + Data.AreaNId + " AND ");
            Sb.Append(" D." + Data.SourceNId + "=  DT." + Data.SourceNId +")");

            Sql = Sb.ToString();

            this.DestDBConnection.ExecuteNonQuery(Sql);
        }

        private void InsertSourceNId_IUSNIdRelationship()
        {
            string ParentNIds = string.Empty;
            int ICNId = -1;
            int IUSNId = -1;

            try
            {
                //- Get Distinct SourceNId & IUS from UT_Data
                DataTable IC_IUS = this.DestDBConnection.ExecuteDataTable(this.DestDBQueries.Data.GetUnMatchedDataByIUSNIDWICIUS());

                //- For each SourceNId
                //- get all Source ParentNid and add thier relationship with IUSNid
                foreach (DataRow drIC_IUS in IC_IUS.Rows)
                {
                    ICNId = Convert.ToInt32(drIC_IUS[Data.SourceNId]);
                    IUSNId = Convert.ToInt32(drIC_IUS[Data.IUSNId]);

                    //- Get IC Parents for this ICNId
                    ParentNIds = this.GetICParentNIds(ICNId.ToString());

                    if (string.IsNullOrEmpty(ParentNIds) == false)
                    {
                        foreach (string parentNId in new List<string>(ParentNIds.Split(',')))
                        {
                            int PNid = int.Parse(parentNId);
                            if (PNid > 0)
                            {
                                //- Add relationShip
                                this.DestDBConnection.ExecuteNonQuery(DI_LibDAL.Queries.IndicatorClassification.Insert.InsertICAndIUSRelation(this.DestDBQueries.DataPrefix, PNid, IUSNId, false));
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        private string GetICParentNIds(string icNIds)
        {
            string RetVal = string.Empty;
            string Query = string.Empty;
            string ParentNIds = string.Empty;

            if (!(string.IsNullOrEmpty(icNIds)))
            {
                RetVal = icNIds;

                //-- SQl query to get parent NIds
                Query = this.DestDBQueries.IndicatorClassification.GetIC(FilterFieldType.NId, icNIds, FieldSelection.Light);

                //- get ParentNids in a string.
                ParentNIds = Common.GetCommaSeperatedString(this.DestDBConnection.ExecuteDataTable(Query), IndicatorClassifications.ICParent_NId);

                if (string.IsNullOrEmpty(ParentNIds) == false)
                {
                    ParentNIds = this.GetICParentNIds(ParentNIds);

                    RetVal = Common.MergeNIds(RetVal, ParentNIds);
                }
            }

            return RetVal;
        }

        #region "DataView Filters"

        private void DeleteFromDataByDataFilters()
        {
            string Query = string.Empty;
            DataViewFilters DataViewFilters = this.UserSelection.DataViewFilters;
            try
            {
                if (DataViewFilters != null)
                {
                    //- Delete from TEMP_Data for following filters

                    // MRD Filter
                    if (DataViewFilters.MostRecentData)
                    {
                        this.DeleteFromDataTableByMRD();
                    }

                    //- IndicatorNId filter
                    if (string.IsNullOrEmpty(DataViewFilters.DeletedSourceNIds) == false)
                    {
                        //- If ShowSourceByIUS is true, 
                        // then NIds are in format:- IUSNId_SourceNId
                        if (DataViewFilters.ShowSourceByIUS)
                        {
                            string[] IUSNId_SourceNId = Common.GetDelimtedNIdsArray(DataViewFilters.DeletedSourceNIds, '_');

                            Query = DIQueries.DeleteRecords(TEMP_TABLE_Data, Data.IUSNId, IUSNId_SourceNId[0], false);
                            Query += " AND " + Data.SourceNId + " IN (" + IUSNId_SourceNId[1] + ")";
                        }
                        else
                        {
                            // else, NIds are in format:- SourceNId
                            Query = DIQueries.DeleteRecords(TEMP_TABLE_Data, Data.SourceNId, DataViewFilters.DeletedSourceNIds, false);
                        }

                        this.DestDBConnection.ExecuteNonQuery(Query);
                    }

                    //- Unit filter
                    if (string.IsNullOrEmpty(DataViewFilters.DeletedUnitNIds) == false)
                    {
                        //- If ShowUnitByIndicator is true, 
                        // then NIds are in format:- IndNId_UnitNId
                        if (DataViewFilters.ShowUnitByIndicator)
                        {
                            string[] IndNId_UnitNId = Common.GetDelimtedNIdsArray(DataViewFilters.DeletedUnitNIds, '_');

                            Query = DIQueries.DeleteRecords(TEMP_TABLE_Data, Data.IndicatorNId, IndNId_UnitNId[0], false);
                            Query += " AND " + Data.UnitNId + " IN (" + IndNId_UnitNId[1] + ")";
                        }
                        else
                        {
                            // else, NIds are in format:- UnitNId
                            Query = DIQueries.DeleteRecords(TEMP_TABLE_Data, Data.UnitNId, DataViewFilters.DeletedUnitNIds, false);
                        }

                        this.DestDBConnection.ExecuteNonQuery(Query);
                    }

                    //- Subgroup Filter
                    if (string.IsNullOrEmpty(DataViewFilters.DeletedSubgroupNIds) == false)
                    {
                        //- If ShowSubgroupByIndicator is true, 
                        // then NIds are in format:- IndNId_SubgroupNId
                        if (DataViewFilters.ShowSubgroupByIndicator)
                        {
                            string[] IndNId_SubgroupNId = Common.GetDelimtedNIdsArray(DataViewFilters.DeletedSubgroupNIds, '_');

                            Query = DIQueries.DeleteRecords(TEMP_TABLE_Data, Data.IndicatorNId, IndNId_SubgroupNId[0], false);
                            Query += " AND " + Data.SubgroupValNId + " IN (" + IndNId_SubgroupNId[1] + ")";
                        }
                        else
                        {
                            // else, NIds are in format:- SubgroupNIds
                            Query = DIQueries.DeleteRecords(TEMP_TABLE_Data, Data.SubgroupValNId, DataViewFilters.DeletedSubgroupNIds, false);
                        }

                        this.DestDBConnection.ExecuteNonQuery(Query);
                    }

                    //- IUS-DataValue Filter
                    if (DataViewFilters.IndicatorDataValueFilters != null && DataViewFilters.IndicatorDataValueFilters.Count > 0)
                    {
                        string IndNId = DataViewFilters.IndicatorDataValueFilters.SQL_GetIndicatorDataValueFilters(this.DestDBConnection.ConnectionStringParameters.ServerType);

                        if (string.IsNullOrEmpty(IndNId) == false)
                        {
                            Query = "DELETE FROM " + TEMP_TABLE_Data + " AS D2 WHERE NOT EXISTS " +
                                "(Select * from " + TEMP_TABLE_Data + " AS D WHERE D." + Data.DataNId + "=D2." + Data.DataNId + " " + IndNId + ")";

                            this.DestDBConnection.ExecuteNonQuery(Query);
                        }
                    }

                    // DataView Range Filter
                    if (DataViewFilters.DataValueFilter != null && DataViewFilters.DataValueFilter.OpertorType != OpertorType.None)
                    {
                        //- Set Delete Query
                        Query = DIQueries.DeleteRecords(TEMP_TABLE_Data, string.Empty, string.Empty, false);

                        Query = Query + Common.GetDataValueRangeFilderString(DataViewFilters.DataValueFilter);

                        this.DestDBConnection.ExecuteNonQuery(Query);
                    }
                }
            }
            catch
            {
            }
        }

        private void DeleteFromDataTableByMRD()
        {
            //- Delete Records filtering Most Recent Data.
            try
            {
                //- Get Records left in Temp_data
                string sSql = "Select * FROM " + TEMP_TABLE_Data;
                DataTable DTData = this.DestDBConnection.ExecuteDataTable(sSql);

                string sAreaNId = string.Empty;
                string sIUSNId = string.Empty;
                StringBuilder sbMRDDataNIDs = new StringBuilder();

                // Most Recent Data
                //sort dataview in decending order of timeperiod so that latest record can be obtained
                DTData.DefaultView.Sort = Indicator_Unit_Subgroup.IUSNId + "," + Area.AreaNId + "," + Timeperiods.TimePeriod + " DESC";
                foreach (DataRow DRowParentTable in DTData.DefaultView.ToTable().Rows)
                {
                    // Get the record for latest timeperiod.
                    if (sAreaNId != DRowParentTable[Area.AreaNId].ToString() || sIUSNId != DRowParentTable[Indicator_Unit_Subgroup.IUSNId].ToString())
                    {
                        sAreaNId = DRowParentTable[Area.AreaNId].ToString();
                        sIUSNId = DRowParentTable[Indicator_Unit_Subgroup.IUSNId].ToString();
                        sbMRDDataNIDs.Append("," + DRowParentTable[Data.DataNId].ToString());
                    }
                }
                if (sbMRDDataNIDs.Length > 0)
                {
                    string sMRDDataNIDs = sbMRDDataNIDs.ToString().Substring(1);

                    //- Delete Records from Data Where DataNIDs NOT IN (MRD_DataNIds)
                    this.DestDBConnection.ExecuteNonQuery(DIQueries.DeleteRecords(TEMP_TABLE_Data, Data.DataNId, sMRDDataNIDs, true));
                }
            }
            catch
            {
            }
        }

        #endregion
        #endregion


        #region " Other Tables (Footnotes, Notes, XSLT)"

        private void ProcessOtherTables()
        {
            //- Footnotes
            this.InsertFootNotes();

            //- Notes
            this.InsertNotesAndRelatedTables();
        }

        private void InsertFootNotes()
        {
            try
            {
                // - Get ConnectionString to be used in inner SQL Query .
                // for e.g: "SELECT * INTO [TEMP_Data] FROM [odbc;driver={SQL Server};Server=dgps;Database=DI6Databases;Uid=sa;Pwd=;].[SN_Data]" +
                string ConnStringClause = this.GetConnectionStringForSQLSyntax(this.SourceDBConnection);

                //-- Get Distinct FootNotes in UT_data table
                DataTable DTFootNotes = this.DestDBConnection.ExecuteDataTable("Select DISTINCT " + Data.FootNoteNId + " FROM " + this.DestDBQueries.TablesName.Data);

                string FootNotesNIds = Common.GetCommaSeperatedString(DTFootNotes, FootNotes.FootNoteNId);

                if (string.IsNullOrEmpty(FootNotesNIds) == false)
                {
                    this.DestDBConnection.ExecuteNonQuery("DELETE FROM " + this.DBTableNames.FootNote);

                    string Sql = "INSERT INTO " + this.DBTableNames.FootNote + " SELECT * FROM " + ConnStringClause + ".[" + this.SourceTableNames.FootNote + "] AS FT " +
                        " WHERE FT." + FootNotes.FootNoteNId + " IN (" + FootNotesNIds + ")";

                    this.DestDBConnection.ExecuteNonQuery(Sql);
                }
            }
            catch
            {
            }
        }

        private void InsertNotesAndRelatedTables()
        {
            try
            {
                // - Get ConnectionString to be used in inner SQL Query.
                string ConnStringClause = this.GetConnectionStringForSQLSyntax(this.SourceDBConnection);

                //- Get Distinct DataNIds present in UT_Data table
                DataTable DTDistinctData = this.DestDBConnection.ExecuteDataTable("SELECT DISTINCT " + Data.DataNId + "  FROM " + this.DBTableNames.Data);
                string DistinctNIds = Common.GetCommaSeperatedString(DTDistinctData, Data.DataNId);

                if (string.IsNullOrEmpty(DistinctNIds) == false)
                {
                    //-- Notes_Data
                    //- Insert from SourceDatabase's Notes_data into target Database filtering selected DataNIds.
                    string Sql = "INSERT INTO " + this.DBTableNames.NotesData + " SELECT * FROM " + ConnStringClause + ".[" + this.SourceTableNames.NotesData + "] AS ND " +
                        " WHERE ND." + Notes_Data.DataNId + " IN (" + DistinctNIds + ")";
                    this.DestDBConnection.ExecuteNonQuery(Sql);

                    //- Get Distinct NotesNIds from resultant UT_Notes_Data table.
                    DTDistinctData = this.DestDBConnection.ExecuteDataTable("SELECT DISTINCT " + Notes_Data.NotesNId + "  FROM " + this.DBTableNames.NotesData);
                    DistinctNIds = Common.GetCommaSeperatedString(DTDistinctData, Notes_Data.NotesNId);

                    //-- Notes
                    if (string.IsNullOrEmpty(DistinctNIds) == false)
                    {
                        //- Insert SourceDatabase's Notes into target Database filtering selected NotesNIds.
                        Sql = "INSERT INTO " + this.DBTableNames.Notes + " SELECT * FROM " + ConnStringClause + ".[" + this.SourceTableNames.Notes + "] AS N " +
                            " WHERE N." + Notes.NotesNId + " IN (" + DistinctNIds + ")";
                        this.DestDBConnection.ExecuteNonQuery(Sql);


                        //-- Profiles
                        //- Get Distinct ProfileNIds from resultant UT_Notes table.
                        DTDistinctData = this.DestDBConnection.ExecuteDataTable("SELECT DISTINCT " + Notes.ProfileNId + "  FROM " + this.DBTableNames.Notes);
                        DistinctNIds = Common.GetCommaSeperatedString(DTDistinctData, Notes.ProfileNId);

                        if (string.IsNullOrEmpty(DistinctNIds) == false)
                        {
                            //- Insert SourceDatabase's NotesProfiles into target Database filtering selected ProfileNIds.
                            Sql = "INSERT INTO " + this.DBTableNames.NotesProfile + " SELECT * FROM " + ConnStringClause + ".[" + this.SourceTableNames.NotesProfile + "] AS NP " +
                                " WHERE NP." + Notes_Profile.ProfileNId + " IN (" + DistinctNIds + ")";
                            this.DestDBConnection.ExecuteNonQuery(Sql);
                        }

                        //-- Notes Classifications
                        //- Get Distinct ClassificationNIds from resultant UT_Notes table.
                        DTDistinctData = this.DestDBConnection.ExecuteDataTable("SELECT DISTINCT " + Notes.ClassificationNId + "  FROM " + this.DBTableNames.Notes);
                        DistinctNIds = Common.GetCommaSeperatedString(DTDistinctData, Notes.ClassificationNId);

                        if (string.IsNullOrEmpty(DistinctNIds) == false)
                        {
                            //- Insert SourceDatabase's Notes_Classification into target Database filtering selected ClassificationNIds.
                            Sql = "INSERT INTO " + this.DBTableNames.NotesClassification + " SELECT * FROM " + ConnStringClause + ".[" + this.SourceTableNames.Notes + "] AS NC " +
                                " WHERE NC." + Notes_Classification.ClassificationNId + " IN (" + DistinctNIds + ")";
                            this.DestDBConnection.ExecuteNonQuery(Sql);
                        }
                    }
                }
            }
            catch
            {
            }
        }


        #endregion

        private void DeleteTable(string tableName, DIConnection dbConnection)
        {
            try
            {
                dbConnection.ExecuteNonQuery(" Drop Table " + tableName);
            }
            catch
            {
            }
        }

        #endregion

        #endregion

        #region "Public"

        #region "New/Dispose"

        internal ExportOnlineDatabase(UserSelection userSelection, DIConnection sourceDBConnection, DIQueries sourceDBQueries)
        {
            if (sourceDBConnection != null && userSelection != null)
            {
                this.UserSelection = userSelection;

                //--  Assign NIDs in userSelection.
                if (userSelection.ShowIUS)
                {
                    this.IUSNIDs = userSelection.IndicatorNIds;
                }
                else
                {
                    this.IndicatorNIDs = userSelection.IndicatorNIds;
                }

                this.AreaNIDs = userSelection.AreaNIds;
                this.TimePeriodNIDs = userSelection.TimePeriodNIds;
                this.SourceNIDs = userSelection.SourceNIds;
                this.ApplyMRD = userSelection.DataViewFilters.MostRecentData;

                this.SourceDBConnection = sourceDBConnection;
                this.SourceDBQueries = sourceDBQueries;         //new DIQueries(this.SourceDBConnection.DIDataSetDefault(), this.SourceDBConnection.DILanguageCodeDefault(this.SourceDBConnection.DIDataSetDefault()));
            }
        }

        #endregion

        #region "Properties"

        #endregion

        #region "Methods"

        internal bool ExportMDB(string destinationDBNameWPath, string tempFolderPath, bool createNewDatabase)
        {
            bool RetVal = false;
            int ProgressCount = 0;
            try
            {
                if (createNewDatabase)
                {
                    this.ExportMDB(destinationDBNameWPath, tempFolderPath);
                }
                else
                {
                    DIExport.RaiseExportProgressBarInitialize(100);

                    //- Establish Connection with Destination Database in temp folder
                    this.DestDBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, destinationDBNameWPath, string.Empty, string.Empty);
                    this.DestDBQueries = new DIQueries(DestDBConnection.DIDataSetDefault(), DestDBConnection.DILanguageCodeDefault(DestDBConnection.DIDataSetDefault()));
                    this.DBTableNames = new DITables(DestDBConnection.DIDataSetDefault(), DestDBConnection.DILanguageCodeDefault(DestDBConnection.DIDataSetDefault()));
                    this.SourceTableNames = this.SourceDBQueries.TablesName;

                    // if database/tempalte already in DI6 format then convert it into latest format
                    DBConverterDecorator DBConverter = new DBConverterDecorator(this.DestDBConnection, this.DestDBQueries);
                    DBConverter.DoConversion(false);

                    //-- Get LanguageTable
                    this.LanguageTable = this.DestDBConnection.DILanguages(this.DestDBConnection.DIDataSetDefault());

                    DIExport.RaiseExportProgressBarIncrement(1);

                    //- Export Indicator, Unit, Subgroupval, IUS
                    this.ProcessIndicators();
                    DIExport.RaiseExportProgressBarIncrement(10);

                    // Export linked IC information
                    this.ProcessIndicatorClassifications();
                    DIExport.RaiseExportProgressBarIncrement(20);

                    //- Export Area
                    this.ProcessAreas();
                    DIExport.RaiseExportProgressBarIncrement(30);

                    //- Export TimePeriod
                    this.ProcessTimePeriods();
                    DIExport.RaiseExportProgressBarIncrement(40);

                    //- Export Data
                    this.ExportData();
                    DIExport.RaiseExportProgressBarIncrement(80);

                    this.ProcessOtherTables();
                    DIExport.RaiseExportProgressBarIncrement(90);

                    DIExport.RaiseExportProgressBarIncrement(ProgressCount++);
                    this.DeleteTable(TEMP_TABLE_Data, this.DestDBConnection);

                    DIExport.RaiseExportProgressBarIncrement(100);
                }
            }
            catch
            {
                throw;
            }

            return RetVal;
        }

        internal bool ExportMDB(string destinationDBNameWPath, string tempFolderPath)
        {
            bool RetVal = false;
            string TempDatabase = string.Empty;
            int ProgressCount = 0;

            try
            {
                DIExport.RaiseExportProgressBarInitialize(100);

                //- Create a blank DevInfo database As Temp database.
                TempDatabase = Path.Combine(tempFolderPath, DateTime.Now.Ticks.ToString());
                DIDatabase.CreateDevInfoDBFile(TempDatabase);

                //- Establish Connection with Destination Database in temp folder
                this.DestDBConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, TempDatabase, string.Empty, string.Empty);
                this.DestDBQueries = new DIQueries(DestDBConnection.DIDataSetDefault(), DestDBConnection.DILanguageCodeDefault(DestDBConnection.DIDataSetDefault()));
                this.DBTableNames = new DITables(DestDBConnection.DIDataSetDefault(), DestDBConnection.DILanguageCodeDefault(DestDBConnection.DIDataSetDefault()));
                this.SourceTableNames = this.SourceDBQueries.TablesName;

                // if database/tempalte already in DI6 format then convert it into latest format
                DBConverterDecorator DBConverter = new DBConverterDecorator(this.DestDBConnection, this.DestDBQueries);
                DBConverter.DoConversion(false);

                //-- Get LanguageTable
                this.LanguageTable = this.DestDBConnection.DILanguages(this.DestDBConnection.DIDataSetDefault());

                DIExport.RaiseExportProgressBarIncrement(1);

                //- Export Indicator, Unit, Subgroupval, IUS
                this.ProcessIndicators();
                DIExport.RaiseExportProgressBarIncrement(10);

                // Export linked IC information
                this.ProcessIndicatorClassifications();
                DIExport.RaiseExportProgressBarIncrement(20);

                //- Export Area
                this.ProcessAreas();
                DIExport.RaiseExportProgressBarIncrement(30);

                //- Export TimePeriod
                this.ProcessTimePeriods();
                DIExport.RaiseExportProgressBarIncrement(40);

                //- Export Data
                this.ExportData();
                DIExport.RaiseExportProgressBarIncrement(80);

                this.ProcessOtherTables();
                DIExport.RaiseExportProgressBarIncrement(90);

                DIExport.RaiseExportProgressBarIncrement(ProgressCount++);
                this.DeleteTable(TEMP_TABLE_Data, this.DestDBConnection);

                DIExport.RaiseExportProgressBarIncrement(ProgressCount++);
                File.Copy(TempDatabase, destinationDBNameWPath, true);
                DIExport.RaiseExportProgressBarIncrement(100);
            }
            catch
            {
                throw;
            }

            return RetVal;
        }

        #endregion

        #endregion

    }
}


