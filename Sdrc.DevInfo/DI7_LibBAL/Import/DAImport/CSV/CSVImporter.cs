using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Data;
using System.Data.OleDb;
using System.Text;

using SpreadsheetGear;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DAImportCommon = DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Log;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.Common;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Globalization;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.CSV
{
    public class CSVImporter : DIData
    {

        #region "private"

        #region "Variables"


        List<string> IUSGIdFields = null;   //- List of IUS GIds field names that are imported from CSV.
        private int ProgressBarValue = 0;
        private DataTable NewAreaImported = null;

        #endregion

        #region "Methods"

        private void ImportCsvFile(string filename)
        {
            DataTable CSVData = null;
            string IUSGID = string.Empty;
            string TempCSVFileName = string.Empty;
            string InvalidCSVReason = string.Empty;

            if (CSVImporter.ValidateCSVFile(filename, out InvalidCSVReason))
            {
                //-Copy CSV File to temp location with arbitary name
                try
                {
                    TempCSVFileName = Path.Combine(this._TempFolderPath, DateTime.Now.Ticks.ToString() + ".csv");
                    File.Copy(filename, TempCSVFileName, true);
                }
                catch
                {
                }

                try
                {
                    //- CSV File format is shown below. Each Field is delimited by comma (,) character
                    //-(Refer CensusInfo CSV Document Guidelines)
                    //- First 5 Columns in CSV are fix for AreaId, AreaName, timePeriod, Source, Footnotes.
                    // Rest are IUS Columns which represents either delimited GIDs or delimited Names :- in form of IndicatorGID-UnitGId-SubgroupGId
                    //-Rest Column 6th to Column N represents dataValue for IUS
                    // AreaId , AreaName , TimePeriod , Source, Footnotes , IUS_1_GID_Or_Name , IUS_2_GID_or_Name , ..... IUS_n_GID_or_Name
                    FileInfo file = new FileInfo(TempCSVFileName);

                    //using (OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + file.DirectoryName + "';Extended Properties='text;HDR=Yes;FMT=Delimited(,)';"))
                    //{
                    //    using (OleDbCommand cmd = new OleDbCommand(string.Format("SELECT * FROM [{0}]", file.Name), con))
                    //    {
                    //        con.Open();

                    CSVData = CSVImporter.GetCSVDataTable(TempCSVFileName, true);

                    //- Get IUS Gid fields present in first row of CSV file. 
                    this.GetIUSGIdFields(TempCSVFileName);

                    this.ProgressBarValue++;
                    this.RaiseProgressBarIncrementEvent(this.ProgressBarValue);

                    // Using a DataTable to process the data
                    //using (OleDbDataAdapter adp = new OleDbDataAdapter(cmd))
                    //{
                    //    ////- Get IUS Gid fields present in first row of CSV file. 
                    //    //this.GetIUSGIdFields(TempCSVFileName);

                    //    CSVData = new DataTable();

                    //    adp.MissingMappingAction = MissingMappingAction.Passthrough;
                    //    adp.Fill(CSVData);

                    //- Get IUS Gid fields present in first row of CSV file. 
                    ////this.GetIUSGIdFields(CSVData);


                    this.ProgressBarValue++;
                    this.RaiseProgressBarIncrementEvent(this.ProgressBarValue);

                    //- Validate Data. Check all columns exists. No. of columns should >= 6
                    if (this.ValidateData(CSVData))
                    {
                        //- CSV DataTable will always have columns FieldNames :- 
                        //- AreaID, AreaName, TimePeriod, Source, Footnotes & IUS GIDs
                        //- Create a Temp Table in Target Database with exact Column Structure as specified in CSV File.
                        this.CreateTempCSVTable(CSVData);

                        //- Now, Dump all records from DataTable into Database's Temp CSV Table
                        this.InsertCSVDataIntoTempTable(CSVData, filename);

                        this.ProgressBarValue++;
                        this.RaiseProgressBarIncrementEvent(this.ProgressBarValue);
                    }
                    //        }
                    //    }
                    //}


                    this.ProgressBarValue++;
                    this.RaiseProgressBarIncrementEvent(this.ProgressBarValue);

                    // Get skipped Data records , Area Records, Sources 
                    // Log Row number along with CSV source File Name.
                    this.GetSkippedDataValuesForLog(CSVData, this.IUSGIdFields);
                    this.AreaSkippedTable.Merge(this.GetSkippedRecordsDataTableForLog(Area.AreaID));
                    this.SourceSkippedTable.Merge(this.GetSkippedRecordsDataTableForLog(Common.Constants.Log.DuplicateSourceColumnName));

                    try
                    {
                        // Step 4: Save all invalid timeperiods into InvalidTimeperiodTable
                        this.InvalidTimeperiodTable.Merge(this.DBConnection.ExecuteDataTable(this._DBQueries.Timeperiod.GetInvalidTimeperiods(Constants.TempCSVTableName)));

                        // Step 6: Delete all records where  timeperiod is null
                        this.DBConnection.ExecuteNonQuery(this._DBQueries.Delete.Timeperiod.DeleteInvalidTimeperiod(Constants.TempCSVTableName));
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(ex.ToString());
                    }

                    //- Process CSV Data.
                    // As all IUS GIDs/Names are in form of IndicatorGID-UnitGId-SubgroupGId
                    // Insert dataValues under IUS_GID Columns into another TempDataTable's Data_Value column
                    // along with AreaID, AreaName, TimePeriod, Source, Footnotes, IndicatorGID, UnitGID, SubgroupGID.
                    // NOTE: TempDataTable refers to standard temp table used while importing DES.
                    if (CSVData.Rows.Count >= 1)
                    {
                        //- Process Data and insert Data from TempCSVTable into "TempDataTable"
                        this.ProcessCSVData(CSVData);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                }

                //- Drop Temp CSV Table
                this.DropTable(Constants.TempCSVTableName);
            }
            else
            {
                this.UpdateSkippedFileLog(filename, "", "Invalid CSV file format.");
            }
        }

        private void GetIUSGIdFields(DataTable csvTable)
        {
            this.IUSGIdFields = new List<string>();

            try
            {
                //- now pick IUS_GIDs starting from 5th columns.

                // Read first line containing column headers
                //strLine = sr.ReadLine();
                //csvFields = strLine.Split(new string[] { Constants.CSVDelimiter }, StringSplitOptions.RemoveEmptyEntries);

                //- CSV field must have atleast 6 columns (5 fixed columns + 1 IUS)
                if (csvTable.Columns.Count >= Constants.FIXED_CSV_COLUMNS + 1)
                {
                    //- rename IUS_GIds columns Name.
                    for (int i = Constants.FIXED_CSV_COLUMNS; i < csvTable.Columns.Count; i++)
                    {
                        this.IUSGIdFields.Add(csvTable.Columns[i].ColumnName.Trim().Trim('\"'));
                    }
                }

                if (this.IUSGIdFields.Count > 0)
                {
                    string[] I_U_S_GId = this.IUSGIdFields[0].Split(Constants.CSV_IUSGId_Delimiter);

                    //- Raise event for Information of Indicator, Unit, Subgroup
                    this.RaiseIUSInfoEvent(Convert.ToString(I_U_S_GId[0]).Trim(), Convert.ToString(I_U_S_GId[1]).Trim(), Convert.ToString(I_U_S_GId[2]).Trim());
                }


            }
            catch
            {
            }
            finally
            {
            }
        }

        private void GetIUSGIdFields(string filename)
        {
            StreamReader sr = null;

            this.IUSGIdFields = new List<string>();

            try
            {
                DataTable Table = CSVImporter.GetCSVDataTable(filename, true);
                //- now pick IUS_GIDs starting from 5th columns.
                sr = new StreamReader(filename);

                string strLine = "";
                string[] csvFields = null;
                string StrCSV = "";
                List<string> CSVColumns = new List<string>();

                while (!sr.EndOfStream)
                {
                    // Read first line containing column headers
                    strLine = sr.ReadLine();
                    csvFields = strLine.Split(new string[] { Constants.COMMADelimiter }, StringSplitOptions.RemoveEmptyEntries);
                    break;
                }
                int DelimeterPosition = 0;
                int CommDelimeterPosition = 0;
                int StartIndex = 0;
                string ColumnName = string.Empty;

                if (Table.Columns.Count >= Constants.FIXED_CSV_COLUMNS + 1)
                {

                    //- rename IUS_GIds columns Name.
                    for (int i = Constants.FIXED_CSV_COLUMNS; i < Table.Columns.Count; i++)
                    {
                        ColumnName = "";
                        string FindString = Table.Columns[i].ColumnName.TrimEnd('#').Replace(Path.GetFileNameWithoutExtension(filename) + "#csv.", "");

                        StartIndex = strLine.IndexOf(FindString.Substring(0, FindString.Length - 2), StartIndex + 1);

                        if (Table.Columns.Count > i + 1)
                        {
                            DelimeterPosition = strLine.IndexOf(Table.Columns[i + 1].ColumnName.TrimEnd('#'), StartIndex + Table.Columns[i].ColumnName.TrimEnd('#').Length - 1) - 1;

                            if (DelimeterPosition < 0)
                            {
                                FindString = Table.Columns[i + 1].ColumnName.TrimEnd('#').Replace(Path.GetFileNameWithoutExtension(filename) + "#csv.", "");
                                string FindStringPrev = Table.Columns[i].ColumnName.TrimEnd('#').Replace(Path.GetFileNameWithoutExtension(filename) + "#csv.", "");

                                DelimeterPosition = strLine.IndexOf(FindString.Substring(0, FindString.Length - 2), StartIndex + FindStringPrev.Length - 2) - 1;
                            }
                            if (DelimeterPosition >= 0)
                            {
                                ColumnName = strLine.Substring(StartIndex, DelimeterPosition - StartIndex);
                            }
                        }
                        else
                        {
                            ColumnName = strLine.Substring(StartIndex, strLine.Length - StartIndex);
                        }

                        this.IUSGIdFields.Add(ColumnName.Trim(',').Trim('\"').Trim());
                    }
                }

                if (this.IUSGIdFields.Count > 0)
                {
                    string[] I_U_S_GId = this.IUSGIdFields[0].Split(Constants.CSV_IUSGId_Delimiter);

                    //- Raise event for Information of Indicator, Unit, Subgroup
                    this.RaiseIUSInfoEvent(I_U_S_GId[0].Trim(), I_U_S_GId[1].Trim(), I_U_S_GId[2].Trim());
                }

            }
            catch
            {
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }
        }

        private int GetTotalIUSCount(List<string> csvFilePaths)
        {
            int RetVal = 0;
            StreamReader sr = null;

            if (csvFilePaths != null)
            {
                foreach (string filePath in csvFilePaths)
                {
                    try
                    {
                        //- now pick IUS_GIDs starting from 6th columns.
                        sr = new StreamReader(filePath);

                        string strLine = "";
                        string[] csvFields = null;

                        while (!sr.EndOfStream)
                        {
                            strLine = sr.ReadLine();
                            csvFields = strLine.Split(new string[] { Constants.CSVDelimiter }, StringSplitOptions.RemoveEmptyEntries);

                            //- CSV field must have atleast 6 columns (5 fixed columns + 1 IUS)
                            if (csvFields.Length >= Constants.FIXED_CSV_COLUMNS)
                            {
                                RetVal += csvFields.Length - Constants.FIXED_CSV_COLUMNS;
                            }

                            break;
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        if (sr != null)
                        {
                            sr.Close();
                        }
                    }
                }
            }

            return RetVal;
        }

        private bool ValidateData(DataTable CSVData)
        {
            bool RetVal = false;

            //- Validation Rules
            //- CSV Data should have minimim 6 Columns (5 Fixed + 1 IUS)
            //- CSV Data should have more than 1 row.
            if (CSVData != null)
            {
                if (CSVData.Columns.Count > Constants.FIXED_CSV_COLUMNS)
                {
                    if (CSVData.Rows.Count >= 1)
                    {
                        RetVal = true;

                        //- Rename DataTable Columns as per the ColumnNames in devInfo Database.
                        CSVData.Columns[0].ColumnName = Area.AreaID;
                        CSVData.Columns[1].ColumnName = Area.AreaName;
                        CSVData.Columns[2].ColumnName = Timeperiods.TimePeriod;
                        CSVData.Columns[3].ColumnName = Constants.SourceColumnName;
                        CSVData.Columns[4].ColumnName = FootNotes.FootNote;
                    }
                }
            }

            return RetVal;
        }

        private void CreateTempCSVTable(DataTable CSVData)
        {
            int IUSColumns = -1;
            string sSQL = "Select D." + Data.DataNId + " AS [" + Constants.Log.CSVDataRowNoColumnName + "], '' AS " + Area.AreaID + ", '' AS " + Area.AreaName + ", '' AS " + Timeperiods.TimePeriod + ", '' AS " + Constants.SourceColumnName + ", '' AS " + FootNotes.FootNote;

            //- Get No. of IUS GUID Columns in CSV DataTable.
            //- IUS GID column starts from 5th -> N fields.(Refer CensusInfo CSV Document Guidelines)
            IUSColumns = CSVData.Columns.Count;

            //- Dynamic IUSGID Columns starts form 6th column
            //- For each IUS GUID Column.
            for (int IUSColumnIndex = 0; IUSColumnIndex < IUSColumns - Constants.FIXED_CSV_COLUMNS; IUSColumnIndex++)
            {
                //- Pick IUS_GID from first Row under Column 'n'
                sSQL += ", D." + Data.DataValue + " AS [IUS_" + (IUSColumnIndex + 1) + "]";
            }

            sSQL += ", '' AS " + Constants.Log.SkippedSourceFileColumnName;

            sSQL += " INTO " + Constants.TempCSVTableName + " FROM " + this._DBQueries.TablesName.Data + " AS D WHERE 1=0";
            this._DBConnection.ExecuteNonQuery(sSQL);
        }

        private void InsertCSVDataIntoTempTable(DataTable CSVData, string csvFilePath)
        {
            DataRow NewRow = null;

            //- Adding "SourceFile" column in CSVData table
            DataTable CSVDataCopy = CSVData.Copy();
            CSVDataCopy.Columns.Add(Constants.Log.SkippedSourceFileColumnName);
            CSVDataCopy.Columns[Constants.Log.SkippedSourceFileColumnName].Expression = "'" + Path.GetFileName(csvFilePath) + "'";

            //- Insert dataTable into Target Database's "TempCSVData" using Adapter.Fill
            System.Data.Common.DbCommandBuilder CmdBuilder = this._DBConnection.GetCurrentDBProvider().CreateCommandBuilder();

            System.Data.Common.DbDataAdapter Adpt = this._DBConnection.GetCurrentDBProvider().CreateDataAdapter();
            CmdBuilder.DataAdapter = Adpt;

            System.Data.Common.DbCommand cmd = this._DBConnection.GetConnection().CreateCommand();
            cmd.Connection = this._DBConnection.GetConnection();

            //- Get Blank DataTable structure from "TempCSVData"
            string Sql = "SELECT " + Area.AreaID + ", " + Area.AreaName + ", " + Timeperiods.TimePeriod + ", " + Constants.SourceColumnName + ", " + FootNotes.FootNote;

            for (int IUSColumnIndex = 0; IUSColumnIndex < CSVData.Columns.Count - Constants.FIXED_CSV_COLUMNS; IUSColumnIndex++)
            {
                //- Pick IUS_GID from first Row under Column 'n'
                Sql += ", IUS_" + (IUSColumnIndex + 1);
            }
            Sql += ", " + Constants.Log.SkippedSourceFileColumnName;
            Sql += " FROM  " + Constants.TempCSVTableName + " WHERE 1=0";

            DataTable DataTableToFill = this._DBConnection.ExecuteDataTable(Sql);


            cmd.CommandText = Sql;

            Adpt.SelectCommand = cmd;
            Adpt.InsertCommand = CmdBuilder.GetInsertCommand();
            Adpt.FillLoadOption = LoadOption.OverwriteChanges;
            Adpt.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            Adpt.FillSchema(DataTableToFill, SchemaType.Source);

            Adpt.AcceptChangesDuringUpdate = true;

            //- Setting Source DataTable Name same as Target DataTable to Fill.
            CSVDataCopy.TableName = DataTableToFill.TableName;

            Predicate<object> BlankSysbolPresent = this.ISDataBlankSymbolPresent;
            //- Adding records from CSVData into DataTable linked to Adapter
            foreach (DataRow Row in CSVDataCopy.Rows)
            {
                NewRow = DataTableToFill.NewRow();
                NewRow.ItemArray = Row.ItemArray;

                //------------- C1 Start -----------------------"
                //-- Replace DES ImportBlankDataValueSymbol with "000000"
                try
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Array.Find(NewRow.ItemArray, BlankSysbolPresent))))
                    {
                        NewRow.BeginEdit();
                        for (int i = 0; i < NewRow.ItemArray.Length; i++)
                        {
                            if (Convert.ToString(NewRow.ItemArray[i]) == DICommon.DESImportBlankDataValueSymbol)
                            {
                                NewRow[i] = "000000";
                            }
                        }
                        NewRow.EndEdit();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionFacade.ThrowException(ex);
                }
                //----------------- C1 End ------------------------

                DataTableToFill.Rows.Add(NewRow);
            }

            //- Update SourceDataTable into Target Database Table.
            Adpt.Update(DataTableToFill);
        }

        private bool ISDataBlankSymbolPresent(object symbol)
        {
            bool RetVal = false;

            if (Convert.ToString(symbol) == DICommon.DESImportBlankDataValueSymbol)
            {
                RetVal = true;
            }

            return RetVal;
        }

        private void ProcessCSVData(DataTable CSVData)
        {
            string sSQL = string.Empty;
            string[] I_U_S_GId = null;
            string IUS_GID = null;
            string IUS_GID_ColumnName = string.Empty;

            // As all IUS GIDs are in form of IndicatorGID-UnitGId-SubgroupGId
            // Insert dataValues under IUS_GID Columns into another TempDataTable's Data_Value column

            //- Dynamic IUSGID Columns starts form 6th column
            //- For each IUS_GID Columns in CSVTable.
            for (int IUSColumnIndex = 0; IUSColumnIndex < CSVData.Columns.Count - Constants.FIXED_CSV_COLUMNS; IUSColumnIndex++)
            {
                if (IUSColumnIndex < this.IUSGIdFields.Count)
                {
                    //- progress bar increment Event for current IUS
                    this.ProgressBarValue += 1;
                    this.RaiseProgressBarIncrementEvent(this.ProgressBarValue);

                    //- Split IUS_GID into IndicatorGID, Unit_Gid, SubgroupValGId
                    // as all IUS GIDs are in form of IndicatorGID_UnitGId_SubgroupGId
                    IUS_GID = this.IUSGIdFields[IUSColumnIndex];
                    I_U_S_GId = IUS_GID.Split(Constants.CSV_IUSGId_Delimiter);

                    if (I_U_S_GId.Length == 3)
                    {
                        //-- Trim space in GIDs
                        I_U_S_GId[0] = Convert.ToString(I_U_S_GId[0]).Trim();
                        I_U_S_GId[1] = Convert.ToString(I_U_S_GId[1]).Trim();
                        I_U_S_GId[2] = Convert.ToString(I_U_S_GId[2]).Trim();
                        //-- SET GID within 60 character
                        string IndicatorGID = Convert.ToString(I_U_S_GId[0].Substring(0, I_U_S_GId[0].Length > 60 ? 59 : I_U_S_GId[0].Length)).Trim();
                        string UnitGID = Convert.ToString(I_U_S_GId[1].Substring(0, I_U_S_GId[1].Length > 60 ? 59 : I_U_S_GId[1].Length)).Trim();
                        //-- SET GID within 50 character
                        string SubgroupValGID = Convert.ToString(I_U_S_GId[2].Substring(0, I_U_S_GId[2].Length > 50 ? 49 : I_U_S_GId[2].Length)).Trim();

                        //- Raise event for Information of Indicator, Unit, Subgroup
                        this.RaiseIUSInfoEvent(Convert.ToString(IndicatorGID).Trim(), Convert.ToString(UnitGID).Trim(), Convert.ToString(SubgroupValGID).Trim());

                        IUS_GID_ColumnName = "IUS_" + (IUSColumnIndex + 1);

                        //- Insert all dataValue under nth IUS_GID Column
                        // along with AreaID, AreaName, TimePeriod, SOurce, IndicatorGID, UnitGID, SubgroupGID.
                        ////sSQL = "INSERT INTO " + Constants.TempDataTableName + "(" + Area.AreaID + "," + Area.AreaName + "," + Timeperiods.TimePeriod + "," + Constants.SourceColumnName + "," + FootNotes.FootNote + "," + Indicator.IndicatorGId + "," + Unit.UnitGId + "," + SubgroupVals.SubgroupValGId + "," + Data.DataValue + "," + Constants.Log.SkippedSourceFileColumnName + ")" +
                        ////    " Select T." + Area.AreaID + ", T." + Area.AreaName + ", T." + Timeperiods.TimePeriod + ",T." + Constants.SourceColumnName + ", T." + FootNotes.FootNote + ", '" + DIQueries.RemoveQuotesForSqlQuery(I_U_S_GId[0]) + "','" + DIQueries.RemoveQuotesForSqlQuery(I_U_S_GId[1]) + "','" + DIQueries.RemoveQuotesForSqlQuery(I_U_S_GId[2]) + "', T." + IUS_GID_ColumnName + ", T." + Constants.Log.SkippedSourceFileColumnName +
                        ////    " FROM " + Constants.TempCSVTableName + " AS T ";


                        sSQL = "INSERT INTO " + Constants.TempDataTableName + "(" + Area.AreaID + "," + Area.AreaName + "," + Timeperiods.TimePeriod + "," + Constants.SourceColumnName + "," + FootNotes.FootNote + "," + Indicator.IndicatorName + "," + Indicator.IndicatorGId + "," + Unit.UnitName + "," + Unit.UnitGId + "," + SubgroupVals.SubgroupVal + "," + SubgroupVals.SubgroupValGId + "," + Data.DataValue + "," + Constants.Log.SkippedSourceFileColumnName + ")" +
                           " Select T." + Area.AreaID + ", T." + Area.AreaName + ", T." + Timeperiods.TimePeriod + ",T." + Constants.SourceColumnName + ", T." + FootNotes.FootNote + ", '" + DIQueries.RemoveQuotesForSqlQuery(I_U_S_GId[0]) + "', '" + DIQueries.RemoveQuotesForSqlQuery(IndicatorGID) + "','" + DIQueries.RemoveQuotesForSqlQuery(I_U_S_GId[1]) + "','" + DIQueries.RemoveQuotesForSqlQuery(UnitGID) + "','" + DIQueries.RemoveQuotesForSqlQuery(I_U_S_GId[2]) + "','" + DIQueries.RemoveQuotesForSqlQuery(SubgroupValGID) + "', T." + IUS_GID_ColumnName + ", T." + Constants.Log.SkippedSourceFileColumnName +
                           " FROM " + Constants.TempCSVTableName + " AS T ";

                        this._DBConnection.ExecuteNonQuery(sSQL);
                    }
                }
            }
        }

        private void DropTable(string tableName)
        {
            try
            {
                this._DBConnection.ExecuteNonQuery("DROP TABLE " + tableName);
            }
            catch
            {
            }
        }

        private void ImportAreas()
        {
            string sSQL = string.Empty;
            System.Data.Common.DbDataAdapter Adpt = null;
            DevInfo.Lib.DI_LibBAL.DA.DML.DIDatabase Database = new DevInfo.Lib.DI_LibBAL.DA.DML.DIDatabase(this._DBConnection, this._DBQueries);
            DevInfo.Lib.DI_LibBAL.DA.DML.AreaInfo areaInfo = null;

            try
            {
                //- Create New Areas log
                sSQL = " Select DISTINCT T." + Area.AreaID + ", T." + Area.AreaName + ", T." + Constants.Log.SkippedSourceFileColumnName +
                        " FROM " + Constants.TempDataTableName + " AS T  LEFT JOIN " + this._DBQueries.TablesName.Area + " AS A " +
                        " ON T." + Area.AreaID + " = A." + Area.AreaID + " where A." + Area.AreaNId + " IS null AND (T." + Area.AreaID + " Is Not Null)";

                this.NewAreaImported = this._DBConnection.ExecuteDataTable(sSQL);


                //- Insert Distinct unmatched Areas from TempDataTable into UT_Area.
                foreach (DataRow drArea in this.NewAreaImported.Rows)
                {
                    areaInfo = new DevInfo.Lib.DI_LibBAL.DA.DML.AreaInfo();
                    areaInfo.Name = Convert.ToString(drArea[Area.AreaName]);
                    areaInfo.ID = Convert.ToString(drArea[Area.AreaID]);
                    areaInfo.GID = Convert.ToString(drArea[Area.AreaID]);
                    areaInfo.ParentNid = -1;
                    areaInfo.Parent = new DevInfo.Lib.DI_LibBAL.DA.DML.AreaInfo();

                    Database.DIArea.CheckNCreateArea(areaInfo);
                }
                //foreach (DataRow Row in this._DBConnection.DILanguages(this._DBQueries.DataPrefix).Rows)
                //{
                //    // Get Object of CPDQueries for Current Language
                //    DIQueries LangBasedDBQueries = new DIQueries(this._DBQueries.DataPrefix, Row[Language.LanguageCode].ToString());

                //    sSQL = "Insert into " + LangBasedDBQueries.TablesName.Area + " (" + Area.AreaParentNId + ", " + Area.AreaID + ", " + Area.AreaName + ", " + Area.AreaGId + "," + Area.AreaLevel + ")" +
                //        " Select DISTINCT -1, T." + Area.AreaID + ", T." + Area.AreaName + ", T." + Area.AreaID + ", 1 " +
                //        " FROM " + Constants.TempDataTableName + " AS T  LEFT JOIN " + LangBasedDBQueries.TablesName.Area + " AS A " +
                //        " ON T." + Area.AreaID + " = A." + Area.AreaID + " where A." + Area.AreaNId + " IS null AND (T." + Area.AreaID + " Is Not Null)";

                //    this._DBConnection.ExecuteNonQuery(sSQL);

                //}
            }
            catch
            {
            }
            finally
            {

            }
        }

        private void UpdateIndicatorNamesByGIds()
        {
            string sSQL = string.Empty;

            //- Update IndicatorName for matched GIDs in TempDataTable.
            sSQL = "UPDATE " + Constants.TempDataTableName + " AS T INNER JOIN " + this._DBQueries.TablesName.Indicator + " AS I ON T." + Indicator.IndicatorGId + " = I." + Indicator.IndicatorGId +
                " SET T." + Indicator.IndicatorName + " = I." + Indicator.IndicatorName +
                " WHERE T." + Indicator.IndicatorGId + " IS NOT Null";

            this._DBConnection.ExecuteNonQuery(sSQL);


            //- Update IndicatorGID to IndicatorName with in tempTable for unmatched GID records.
            sSQL = "UPDATE " + Constants.TempDataTableName + " AS T " +
                " SET T." + Indicator.IndicatorName + " = T." + Indicator.IndicatorGId +
                " WHERE T." + Indicator.IndicatorName + " IS Null OR T." + Indicator.IndicatorName + " = ''";

            this._DBConnection.ExecuteNonQuery(sSQL);
        }

        private void UpdateUnitNamesByGIds()
        {
            string sSQL = string.Empty;

            //- Update UnitName for matched GIDs in TempDataTable.
            sSQL = "UPDATE " + Constants.TempDataTableName + " AS T INNER JOIN " + this._DBQueries.TablesName.Unit + " AS I ON T." + Unit.UnitGId + " = I." + Unit.UnitGId +
                " SET T." + Unit.UnitName + " = I." + Unit.UnitName +
                " WHERE T." + Unit.UnitGId + " IS NOT Null";

            this._DBConnection.ExecuteNonQuery(sSQL);

            //- Update UnitGID to UnitName with in tempTable for unmatched GID records.
            sSQL = "UPDATE " + Constants.TempDataTableName + " AS T " +
                " SET T." + Unit.UnitName + " = T." + Unit.UnitGId +
                " WHERE T." + Unit.UnitName + " IS Null OR T." + Unit.UnitName + " =''";

            this._DBConnection.ExecuteNonQuery(sSQL);

        }

        private void UpdateSubgroupValsByGIds()
        {
            string sSQL = string.Empty;

            //- Update SubgroupVals for matched GIDs in TempDataTable.
            sSQL = "UPDATE " + Constants.TempDataTableName + " AS T INNER JOIN " + this._DBQueries.TablesName.SubgroupVals + " AS S ON T." + SubgroupVals.SubgroupValGId + " = S." + SubgroupVals.SubgroupValGId +
                " SET T." + SubgroupVals.SubgroupVal + " = S." + SubgroupVals.SubgroupVal +
                " WHERE T." + SubgroupVals.SubgroupValGId + " IS NOT Null";

            this._DBConnection.ExecuteNonQuery(sSQL);

            //- Update subgroupValGID to SubgroupValName with in tempTable for unmatched records.
            sSQL = "UPDATE " + Constants.TempDataTableName + " AS T " +
                " SET T." + SubgroupVals.SubgroupVal + " = T." + SubgroupVals.SubgroupValGId +
                " WHERE T." + SubgroupVals.SubgroupVal + " IS Null OR T." + SubgroupVals.SubgroupVal + " =''";

            this._DBConnection.ExecuteNonQuery(sSQL);
        }

        private void InsertIndicatorUnitSubgroups()
        {
            string SQL = string.Empty;

            //- Create DML Database object.
            DevInfo.Lib.DI_LibBAL.DA.DML.DIDatabase Database = new DevInfo.Lib.DI_LibBAL.DA.DML.DIDatabase(this._DBConnection, this._DBQueries);
            DevInfo.Lib.DI_LibBAL.DA.DML.IUSInfo newIUS = null;
            DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupInfo SubgroupDimensionValue = null;
            DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupTypeInfo oSubgroupType = null;
            DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupValBuilder SubgroupValBuilder = new DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupValBuilder(this._DBConnection, this._DBQueries);
            DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupTypeBuilder SubgroupTypeBuilder = new DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupTypeBuilder(this._DBConnection, this._DBQueries);
            DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupBuilder SubgroupBuilder = new DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupBuilder(this._DBConnection, this._DBQueries);

            try
            {
                //- Get all IndicatorGID, UnitGId, SubgroupVal_Gid form TempTable & thier Names + NIds from associated tables.
                SQL = "SELECT DISTINCT T." + Indicator.IndicatorName + ", T." + Unit.UnitName + ", T." + SubgroupVals.SubgroupVal + ", T." + Indicator.IndicatorGId + ", T." + Unit.UnitGId + ", T." + SubgroupVals.SubgroupValGId + ", I." + Indicator.IndicatorNId + ", U." + Unit.UnitNId + ", S." + SubgroupVals.SubgroupValNId + ", I." + Indicator.IndicatorName + ", U." + Unit.UnitName + ", S." + SubgroupVals.SubgroupVal +
                    " FROM ((" + Constants.TempDataTableName + " AS T LEFT JOIN " + this._DBQueries.TablesName.Indicator + " AS I ON T." + Indicator.IndicatorGId + " = I." + Indicator.IndicatorGId + ") LEFT JOIN " +
                        this._DBQueries.TablesName.Unit + " AS U ON T." + Unit.UnitGId + " = U." + Unit.UnitGId + ") LEFT JOIN " + this._DBQueries.TablesName.SubgroupVals + " AS S ON T." + SubgroupVals.SubgroupValGId + " = S." + SubgroupVals.SubgroupValGId;

                DataTable UnmatchedI_U_S_GIds = this._DBConnection.ExecuteDataTable(SQL);

                if (UnmatchedI_U_S_GIds != null && UnmatchedI_U_S_GIds.Rows.Count > 0)
                {
                    //- For each unmatched I_U_S, 

                    foreach (DataRow drIUS in UnmatchedI_U_S_GIds.Rows)
                    {
                        // create IUS combination in database    
                        newIUS = new DevInfo.Lib.DI_LibBAL.DA.DML.IUSInfo();

                        //- If IndicatorNId not found for indicatorGID in template, then create GID by IndicatorName
                        // Logic for creating GID by Name is fixed. Removing space character by Dots and converting into upper case.
                        // FOR E.g.: GID for "Population Size" will be "POPULATION.SIZE"
                        // GID for "Total Female" will be "TOTAL.FEMALE"
                        if (drIUS[Indicator.IndicatorNId] == DBNull.Value || Convert.ToInt32(drIUS[Indicator.IndicatorNId]) <= 0)
                        {
                            newIUS.IndicatorInfo.Name = Convert.ToString(drIUS["T." + Indicator.IndicatorName]);
                            newIUS.IndicatorInfo.GID = Convert.ToString(drIUS[Indicator.IndicatorGId]);
                            newIUS.IndicatorInfo.GID = this.CreateGIdByName(newIUS.IndicatorInfo.GID);
                        }
                        else
                        {
                            newIUS.IndicatorInfo.Name = Convert.ToString(drIUS["I." + Indicator.IndicatorName]);
                            newIUS.IndicatorInfo.GID = Convert.ToString(drIUS[Indicator.IndicatorGId]);
                        }

                        //- If UnitNId not found for UnitGID in template, then create GID by UnitName
                        if (drIUS[Unit.UnitNId] == DBNull.Value || Convert.ToInt32(drIUS[Unit.UnitNId]) <= 0)
                        {
                            newIUS.UnitInfo.Name = Convert.ToString(drIUS["T." + Unit.UnitName]);
                            newIUS.UnitInfo.GID = Convert.ToString(drIUS[Unit.UnitGId]);
                            newIUS.UnitInfo.GID = this.CreateGIdByName(newIUS.UnitInfo.GID);
                        }
                        else
                        {
                            newIUS.UnitInfo.Name = Convert.ToString(drIUS["U." + Unit.UnitName]);
                            newIUS.UnitInfo.GID = Convert.ToString(drIUS[Unit.UnitGId]);
                        }

                        //- If SubgroupValNId not found for Subgroup_Val_GID in template, then create GID by Subgroup_Val
                        if (drIUS[SubgroupVals.SubgroupValNId] == DBNull.Value || Convert.ToInt32(drIUS[SubgroupVals.SubgroupValNId]) <= 0)
                        {
                            newIUS.SubgroupValInfo.Name = Convert.ToString(drIUS["T." + SubgroupVals.SubgroupVal]);
                            newIUS.SubgroupValInfo.GID = Convert.ToString(drIUS[SubgroupVals.SubgroupValGId]);
                            newIUS.SubgroupValInfo.GID = this.CreateGIdByName(newIUS.SubgroupValInfo.GID);
                            //-- Check if 
                            newIUS.SubgroupValInfo.Nid = SubgroupValBuilder.GetSubgroupValNid(newIUS.SubgroupValInfo.GID, newIUS.SubgroupValInfo.Name);
                            //- Add Subgroup Dimensions Value against SubgroupVal
                            SubgroupDimensionValue = new DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupInfo();
                            if (newIUS.SubgroupValInfo.Nid <= 0)
                            {
                                //- Assign Subgroup Dimension (SubgroupType) as "OTHERS"
                                oSubgroupType = new DevInfo.Lib.DI_LibBAL.DA.DML.DI6SubgroupTypeInfo();
                                oSubgroupType.Name = SubgroupType.Others.ToString();
                                oSubgroupType.GID = SubgroupType.Others.ToString().ToUpper();
                                oSubgroupType.Nid = SubgroupTypeBuilder.CheckNCreateSubgroupType(oSubgroupType);

                                //- keep Subgroup Dimensions Value same as Subgroup_Val
                                SubgroupDimensionValue.DISubgroupType = oSubgroupType;
                                SubgroupDimensionValue.Name = newIUS.SubgroupValInfo.Name;
                                SubgroupDimensionValue.Type = oSubgroupType.Nid;
                                SubgroupDimensionValue.Nid = SubgroupBuilder.CheckNCreateSubgroup(SubgroupDimensionValue);

                                //- Check and insert SubgroupVal
                                newIUS.SubgroupValInfo.Nid = SubgroupValBuilder.CheckNCreateSubgroupVal(newIUS.SubgroupValInfo);

                                //- Add SubgroupVal - Subgroup RelationShip
                                SubgroupValBuilder.InsertSubgroupValRelations(newIUS.SubgroupValInfo.Nid, SubgroupDimensionValue.Nid);
                            }
                        }
                        else
                        {
                            newIUS.SubgroupValInfo.Name = Convert.ToString(drIUS["S." + SubgroupVals.SubgroupVal]);
                            newIUS.SubgroupValInfo.GID = Convert.ToString(drIUS[SubgroupVals.SubgroupValGId]);
                            newIUS.SubgroupValInfo.Nid = Convert.ToInt32(drIUS[SubgroupVals.SubgroupValNId]);
                        }

                        newIUS.Nid = Database.DIIUS.CheckNCreateIUS(newIUS);
                    }

                    //- NOW update IndiatorGID, UnitGId, SubgroupGID in TempDataTable for matched Names

                    //- Update IndicatorGID in TempDataTable for matching Names between UT_Indicator and TempDataTable
                    SQL = "UPDATE " + Constants.TempDataTableName + " AS T INNER JOIN " + this._DBQueries.TablesName.Indicator + " AS I ON T." + Indicator.IndicatorName + " = I." + Indicator.IndicatorName +
                        " SET T." + Indicator.IndicatorGId + " = I." + Indicator.IndicatorGId +
                        " WHERE T." + Indicator.IndicatorName + " IS NOT Null AND T." + Indicator.IndicatorName + " <> ''";

                    this._DBConnection.ExecuteNonQuery(SQL);

                    //- Update UnitGID in TempDataTable for matching Names between UT_Unit and TempDataTable
                    SQL = "UPDATE " + Constants.TempDataTableName + " AS T INNER JOIN " + this._DBQueries.TablesName.Unit + " AS U ON T." + Unit.UnitName + " = U." + Unit.UnitName +
                        " SET T." + Unit.UnitGId + " = U." + Unit.UnitGId +
                        " WHERE T." + Unit.UnitName + " IS NOT Null AND T." + Unit.UnitName + " <> ''";

                    this._DBConnection.ExecuteNonQuery(SQL);

                    //- Update SubgroupValGID in TempDataTable for matching Names between UT_Subgroup_Vals and TempDataTable
                    SQL = "UPDATE " + Constants.TempDataTableName + " AS T INNER JOIN " + this._DBQueries.TablesName.SubgroupVals + " AS S ON T." + SubgroupVals.SubgroupVal + " = S." + SubgroupVals.SubgroupVal +
                        " SET T." + SubgroupVals.SubgroupValGId + " = S." + SubgroupVals.SubgroupValGId +
                        " WHERE T." + SubgroupVals.SubgroupVal + " IS NOT Null AND T." + SubgroupVals.SubgroupVal + " <> ''";

                    this._DBConnection.ExecuteNonQuery(SQL);

                }
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        /// <summary>
        /// It Replaces space chars (' ') by dot character (.) in the string
        //  then converts result to UPPER CASE.
        /// </summary>
        /// <param name="elementName"></param>
        /// <returns></returns>
        private string CreateGIdByName(string elementName)
        {
            //- Logic for creating Name by GID is :
            // Replace space char (' ') by dot character (.) in the string
            // then convert string to UPPER CASE.

            // FOR E.g.: "Population Size" becomes "POPULATION.SIZE"
            string RetVal = string.Empty;

            if (!string.IsNullOrEmpty(elementName))
            {
                RetVal = elementName.ToUpper().Replace(' ', '.');
            }

            return RetVal;
        }

        private DataTable GetSkippedRecordsDataTableForLog(string columnName)
        {
            DataTable RetVal = null;

            try
            {
                RetVal = this.DBConnection.ExecuteDataTable("Select " + Constants.Log.SkippedSourceFileColumnName + ", " + Constants.Log.CSVDataRowNoColumnName + " FROM " + Common.Constants.TempCSVTableName + " WHERE " + columnName + " Is Null OR " + columnName + " = ''");
            }
            catch (Exception ex)
            {
                RetVal = null;
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        private void GetSkippedDataValuesForLog(DataTable CSVData, List<string> IUSGIdFields)
        {
            DataTable TempSkippedRecords = null;
            string IUSColumnName = string.Empty;
            string IUSGID = string.Empty;
            string sql = string.Empty;

            try
            {
                if (CSVData != null && CSVData.Columns.Count > 0)
                {
                    //- For each IUS columns in CSV DataTable
                    // get log for blank DataValues that are going to be skipped.
                    for (int IUSColumnIndex = 0; IUSColumnIndex < CSVData.Columns.Count - Constants.FIXED_CSV_COLUMNS; IUSColumnIndex++)
                    {
                        if (IUSColumnIndex < this.IUSGIdFields.Count)
                        {
                            IUSColumnName = "IUS_" + (IUSColumnIndex + 1);
                            IUSGID = IUSGIdFields[IUSColumnIndex];

                            //TempSkippedRecords = CSVData.DefaultView.ToTable(true, Constants.Log.SkippedSourceFileColumnName, Constants.Log.CSVDataRowNoColumnName);

                            sql = "SELECT " + Constants.Log.SkippedSourceFileColumnName + ", (" + Constants.Log.CSVDataRowNoColumnName + " + 1) AS " + Constants.Log.CSVDataRowNoColumnName + ", '" + DIQueries.RemoveQuotesForSqlQuery(IUSGID) + "' AS [IUS] FROM " + Constants.TempCSVTableName + " WHERE " + IUSColumnName + " Is Null OR " + IUSColumnName + " = '' OR TRIM(" + IUSColumnName + ") = '" + Constants.CSV_BLANK_DATA_VALUE_SYMBOL + "'";
                            TempSkippedRecords = this._DBConnection.ExecuteDataTable(sql);


                            //- Merge Skipped records into Main SkippeddataValue for Log.
                            this.DataSkippedTable.Merge(TempSkippedRecords);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private void RaiseIUSInfoEvent(string indicatorGId, string unitGId, string subgroupValGID)
        {
            try
            {
                if (string.IsNullOrEmpty(indicatorGId) == false)
                {
                    DataTable DTIUS = this._DBConnection.ExecuteDataTable(this._DBQueries.IUS.GetIUSNIdsByGID(indicatorGId, unitGId, subgroupValGID));

                    if (DTIUS.Rows.Count > 0)
                    {
                        this.RaiseProcessIndicatorNameEvent(DTIUS.Rows[0][Indicator.IndicatorName].ToString());
                        this.RaiseProcessUnitNameEvent(DTIUS.Rows[0][Unit.UnitName].ToString());
                    }
                    else
                    {
                        this.RaiseProcessIndicatorNameEvent(indicatorGId);
                        this.RaiseProcessUnitNameEvent(unitGId);
                    }
                }
            }
            catch
            { }

        }

        #endregion

        #endregion

        #region "Internal"

        #region "New/Dispose"

        /// <summary>
        /// Initialize the CSV importer Class object.
        /// </summary>
        /// <param name="csvFileNamePath">CSV file Name with Path.</param>
        /// <param name="targetFileNameWPath">Target file refers to Access database file path.</param>
        /// <param name="tempFolderPath">Temp folder path</param>
        /// <param name="htmlLogOutputPath">html output log path.</param>
        public CSVImporter(List<string> csvFilesNamePath, string targetFileNameWPath, string tempFolderPath, string htmlLogOutputPath)
        {
            this.TempTargetFile = Path.Combine(tempFolderPath, Path.GetFileName(targetFileNameWPath));
            try
            {
                File.Copy(targetFileNameWPath, this.TempTargetFile, true);
            }
            catch
            { }

            this.SetBasicPropertiesAndProcessValues(csvFilesNamePath, targetFileNameWPath, tempFolderPath, htmlLogOutputPath);

        }


        #endregion

        #region "Properties"

        #endregion

        #endregion

        #region "Protected"

        #region "Methods"

        protected override void ProcessTempDataTable()
        {
            //- Count total No. of IUS_Gids present in all CSV files.
            int TotalIUS = this.GetTotalIUSCount(this.TempFiles);
            this.ProgressBarValue = 0;

            //- Initialize progressbar Event
            int MaxProgressBarValue = TotalIUS + 4 * (this.TempFiles.Count);
            this.RaiseProgressBarInitializeEvent(MaxProgressBarValue);

            //- Clear Log tables
            this.AreaSkippedTable = new DataTable();
            this.SourceSkippedTable = new DataTable();
            this.InvalidTimeperiodTable = new DataTable();
            this.InvalidSourceTable = new DataTable();
            this.DataSkippedTable = new DataTable();

            foreach (string csvFilepath in this.TempFiles)
            {
                //Raise event  to display processing information
                this.ProgressBarValue += 1;
                //- raise Event for current CSV File
                this.RaiseProgressBarIncrementEvent(this.ProgressBarValue);
                this.RaiseProcessSourceFileNameEvent(Path.GetFileName(csvFilepath));

                this.ImportCsvFile(csvFilepath);
            }

            //- update IndicatorNames, UnitName, subgroupVal for matched GIds in TempDataTable
            this.UpdateIndicatorNamesByGIds();
            this.UpdateUnitNamesByGIds();
            this.UpdateSubgroupValsByGIds();

            //- Insert Indictor, Unit, SubgroupVals for unmatched GIDs
            //- Consider GIDs as names while inserting into the database.
            this.InsertIndicatorUnitSubgroups();

            //- Insert new Areas in UT_Area table
            //- As requirement in CSV import, all new Areas need to be added in Database at level 1 with parent -1
            this.ImportAreas();

            this.ProgressBarValue++;
            this.RaiseProgressBarIncrementEvent(this.ProgressBarValue);

            this.RaiseProgressBarIncrementEvent(MaxProgressBarValue);

            // Remove all rows from Temp_Data table where Data Value is null or (blank or '-') OR AreaID Is Null OR TimePeriod Is Null OR SourceColumnName Is Null
            this._DBConnection.ExecuteNonQuery("Delete from " + Constants.TempDataTableName + " where (" + Data.DataValue + " Is Null) OR (" + Data.DataValue + " = '') OR (TRIM(" + Data.DataValue + ") = '" + Constants.CSV_BLANK_DATA_VALUE_SYMBOL + "' ) OR (" + Area.AreaID + " Is Null) OR (" + Timeperiods.TimePeriod + " Is Null)" + " OR (" + Constants.SourceColumnName + " Is Null  )" + " OR (" + Constants.SourceColumnName + " ='' )");
        }


        protected override void AddNotesAssistants()
        {
            //
        }

        protected override void CreateHTMLLog()
        {
            // 1. Close DB connection
            this._DBConnection.Dispose();
            GC.Collect();

            // 3. Re-Create TargetDbConnection
            this._DBConnection = new DIConnection(new DIConnectionDetails(DIServerType.MsAccess, string.Empty,
            string.Empty, this.TempTargetFile, string.Empty, DAImportCommon.Constants.DBPassword));

            // 4. Set required elements for Log
            int TotalTimePeriod = (int)this.DBConnection.ExecuteScalarSqlQuery(this.DAQuery.GetDistinctTimePeriodCount());
            int TotalSources = (int)this.DBConnection.ExecuteScalarSqlQuery(this.DAQuery.GetDistinctSourcesCount());
            int TotalData = (int)this.DBConnection.ExecuteDataTable(this.DAQuery.GetDataCount()).Rows[0][0];

            this.HtmlLog = new HTMLLog(this.ImportFileType, TotalTimePeriod, TotalSources, TotalData, this.IndicatorLogInfoTable, this.UnitLogInfoTable, this.SubgroupLogInfoTable, this.NewAreaImported, this.DuplicateRecordsTable, this.SkippedRecordsTable, this.SkippedFiles);

            this.HtmlLog.StartTime = this.StartTime.ToString();
            this.HtmlLog.InvalidTimeperiodsTable = this.InvalidTimeperiodTable;
            this.HtmlLog.InvalidSourceTable = this.InvalidSourceTable;
            this.HtmlLog.SkippedAreaTable = this.AreaSkippedTable;
            this.HtmlLog.SkippedDataTable = this.DataSkippedTable;
            this.HtmlLog.SkippedSourceTable = this.SourceSkippedTable;
            this.HtmlLog.SkippedSubgroupTable = this.SubgroupSkippedTable;
            this.HtmlLog.IUSLogInfoTable = this.IUSLogInfoTable;

            // 5. Create Log
            this.HtmlLog.TargetFilePath = this.TargetFileNameForLogFile;
            this._HtmlLogFilePath = this.HtmlLog.CreateCSVLog(this._TargetFileNameWPath, this.SourceFileNamesWPath, this._HtmlLogOutPutPath);

        }

        protected override bool UploadDatabase()
        {
            bool RetVal = false;
            //--
            return RetVal;
        }

        /// <summary>
        /// CSV file must have atleast 6 column fields , delimieted by comma ','
        // AreaID, AreaName, TimePeriod, Source, Footnote, IUSGId 1.. IUSGID 2.. IUSGID n
        /// </summary>
        /// <param name="csvfilename"></param>
        /// <returns></returns>
        public static bool ValidateCSVFileOld(string csvfilename, out string invalidReason)
        {
            StreamReader sr = null;
            bool RetVal = false;
            bool CanProceed = true;
            invalidReason = string.Empty;
            int FieldCount = 0;
            int LineNo = 0;

            try
            {
                //read csv file.
                sr = new StreamReader(csvfilename);

                string strLine = "";
                string[] csvFields = null;

                while (!sr.EndOfStream)
                {
                    strLine = sr.ReadLine();
                    LineNo += 1;

                    //- Check Double Quotes must presents in all CSV column Fields.
                    if (strLine.EndsWith("\"") && strLine.StartsWith("\""))
                    {
                        csvFields = strLine.Trim('"').Split(new string[] { Constants.CSVDelimiter }, StringSplitOptions.None); // Constants.CSVDelimiter

                        //- Get total number of fields.
                        FieldCount = csvFields.Length;

                        //- CSV field must have atleast 6 columns. 
                        // AreaID, AreaName, TimePeriod, Source, Footnote, IUSGId 1.. IUSGID 2.. IUSGID n
                        if (csvFields.Length >= Constants.FIXED_CSV_COLUMNS + 1)
                        {
                            for (int i = 0; i < csvFields.Length; i++)
                            {
                                if (string.IsNullOrEmpty(csvFields[i].Trim()))
                                {
                                    RetVal = false;
                                    CanProceed = false;
                                    break;
                                }
                            }

                            if (CanProceed)
                            {
                                //- now pick IUS_GIDs/Names starting from 6th columns.
                                //- Validate IUS_GIds columns.
                                for (int i = Constants.FIXED_CSV_COLUMNS; i < csvFields.Length; i++)
                                {
                                    string[] I_U_S_GId = csvFields[i].Split(Constants.CSV_IUSGId_Delimiter);

                                    if (I_U_S_GId.Length == 3)
                                    {
                                        RetVal = true;
                                    }
                                    else
                                    {
                                        RetVal = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        invalidReason = "Invalid CSV file format. Double quotes missing in fields.";
                    }

                    break;
                }

                if (RetVal)
                {
                    //- Check Double Quotes must presents in all CSV Fields or not.
                    while (!sr.EndOfStream && RetVal)
                    {
                        strLine = sr.ReadLine();
                        LineNo += 1;

                        csvFields = strLine.Trim('"').Split(new string[] { Constants.CSVDelimiter }, StringSplitOptions.None); // Constants.CSVDelimiter


                        if (strLine.StartsWith("\"") || (csvFields.Length >= Constants.FIXED_CSV_COLUMNS - 1))
                        {
                            //- line is valid & fieldCount also matched, do nothing
                        }
                        else
                        {
                            // double quotes missing in line.
                            invalidReason = "Invalid CSV file format. Double quotes missing in fields at line number: " + LineNo;
                            RetVal = false;
                            break;
                        }
                    }
                }

                if (sr != null)
                {
                    sr.Close();
                }
            }
            catch
            {
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }

            return RetVal;
        }


        ////public static bool ValidateCSVFile(string csvfilename, out string invalidReason)
        ////{
        ////    DataTable CSVTable = null;
        ////    bool RetVal = false;
        ////    bool CanProceed = true;
        ////    invalidReason = string.Empty;
        ////    int FieldCount = 0;
        ////    int LineNo = 0;

        ////    try
        ////    {
        ////        //read csv file.
        ////        CSVTable = CSVImporter.GetCSVDataTable(csvfilename, true);

        ////        //- Get total number of fields.
        ////        FieldCount = CSVTable.Columns.Count;

        ////        //- CSV field must have atleast 6 columns. 
        ////        // AreaID, AreaName, TimePeriod, Source, Footnote, IUSGId 1.. IUSGID 2.. IUSGID n
        ////        if (CSVTable.Columns.Count >= Constants.FIXED_CSV_COLUMNS + 1)
        ////        {
        ////            for (int i = 0; i < CSVTable.Columns.Count; i++)
        ////            {
        ////                if (string.IsNullOrEmpty(CSVTable.Columns[i].ColumnName.Trim()))
        ////                {
        ////                    RetVal = false;
        ////                    CanProceed = false;
        ////                    break;
        ////                }
        ////            }

        ////            if (CanProceed)
        ////            {
        ////                //- now pick IUS_GIDs/Names starting from 6th columns.
        ////                //- Validate IUS_GIds columns.
        ////                for (int i = Constants.FIXED_CSV_COLUMNS; i < CSVTable.Columns.Count; i++)
        ////                {
        ////                    string[] I_U_S_GId = CSVTable.Columns[i].ColumnName.Split(Constants.CSV_IUSGId_Delimiter);

        ////                    if (I_U_S_GId.Length == 3)
        ////                    {
        ////                        RetVal = true;
        ////                    }
        ////                    else
        ////                    {
        ////                        RetVal = false;
        ////                    }
        ////                }
        ////            }
        ////        }
        ////        else
        ////        {
        ////            invalidReason = "Invalid CSV file format.";
        ////        }

        ////    }
        ////    catch
        ////    {
        ////    }
        ////    finally
        ////    {
        ////    }

        ////    return RetVal;
        ////}


        public static bool ValidateCSVFile(string filename, out string invalidReason)
        {
            StreamReader sr = null;
            DataTable CSVTable = null;
            bool RetVal = false;
            bool CanProceed = true;

            invalidReason = string.Empty;

            try
            {


                CSVTable = CSVImporter.GetCSVDataTable(filename, true);
                //- now pick IUS_GIDs starting from 5th columns.

                sr = new StreamReader(filename);

                string strLine = "";
                string[] csvFields = null;

                List<string> CSVColumns = new List<string>();

                while (!sr.EndOfStream)
                {
                    // Read first line containing column headers
                    strLine = sr.ReadLine();
                    csvFields = strLine.Split(new string[] { Constants.COMMADelimiter }, StringSplitOptions.RemoveEmptyEntries);
                    break;
                }

                CSVColumns = GetCSVRecord(csvFields);

                int DelimeterPosition = 0;
                int StartIndex = 0;
                string ColumnName = string.Empty;

                if (CSVTable.Columns.Count >= Constants.FIXED_CSV_COLUMNS + 1)
                {
                    for (int i = 0; i < CSVTable.Columns.Count; i++)
                    {
                        if (string.IsNullOrEmpty(CSVTable.Columns[i].ColumnName.Trim()))
                        {
                            RetVal = false;
                            CanProceed = false;
                            break;
                        }
                    }

                    if (CanProceed)
                    {
                        //- rename IUS_GIds columns Name.
                        for (int i = Constants.FIXED_CSV_COLUMNS; i < CSVTable.Columns.Count; i++)
                        {
                            //-- Column name of 64 character if duplicate can prefixed with <filename.columnname>
                            string FindString = CSVTable.Columns[i].ColumnName.TrimEnd('#').Replace(Path.GetFileNameWithoutExtension(filename) + "#csv.", "");

                            StartIndex = strLine.IndexOf(FindString.Substring(0, FindString.Length - 2), StartIndex + 1);

                            if (CSVTable.Columns.Count > i + 1)
                            {
                                DelimeterPosition = strLine.IndexOf(CSVTable.Columns[i + 1].ColumnName.TrimEnd('#'), StartIndex + CSVTable.Columns[i].ColumnName.TrimEnd('#').Length - 1) - 1;

                                if (DelimeterPosition < 0)
                                {
                                    FindString = CSVTable.Columns[i + 1].ColumnName.TrimEnd('#').Replace(Path.GetFileNameWithoutExtension(filename) + "#csv.", "");
                                    string FindStringPrev = CSVTable.Columns[i].ColumnName.TrimEnd('#').Replace(Path.GetFileNameWithoutExtension(filename) + "#csv.", "");

                                    DelimeterPosition = strLine.IndexOf(FindString.Substring(0, FindString.Length - 2), StartIndex + FindStringPrev.Length - 2) - 1;
                                }

                                ColumnName = strLine.Substring(StartIndex, DelimeterPosition - StartIndex);
                            }
                            else
                            {
                                ColumnName = strLine.Substring(StartIndex, strLine.Length - StartIndex);
                            }
                            string[] I_U_S_GId = ColumnName.Split(Constants.CSV_IUSGId_Delimiter);

                            if (I_U_S_GId.Length == 3)
                            {
                                RetVal = true;
                            }
                            else
                            {
                                RetVal = false;
                            }

                        }
                    }

                }
                else
                {
                    invalidReason = "Invalid CSV file format.";
                }
            }
            catch
            {
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }

            return RetVal;
        }


        //public static DataTable GetCSVDataTable(string csvFileNameWPath, bool IsFirstRowHeader)
        //{
        //    string Header = "No";
        //    string SqlQuery = string.Empty;
        //    DataTable RetVal = null;
        //    string PathOnly = string.Empty;
        //    string CSVFileName = string.Empty;

        //    try
        //    {

        //        PathOnly = Path.GetDirectoryName(csvFileNameWPath);
        //        CSVFileName = Path.GetFileName(csvFileNameWPath);

        //        SqlQuery = @"SELECT * FROM [" + CSVFileName + "]";

        //        if (IsFirstRowHeader)
        //        {
        //            Header = "Yes";
        //        }

        //        try
        //        {
        //            //-- Connect to csv file
        //            //'using (OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + file.DirectoryName + "';Extended Properties='text;HDR=Yes;FMT=Delimited(,)';"))

        //            using (OleDbConnection CSVDBConnection = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + PathOnly + ";Extended Properties='Text;HDR=" + Header + ";FMT=Delimited(,)'"))
        //            {
        //                using (OleDbCommand Command = new OleDbCommand(SqlQuery, CSVDBConnection))
        //                {
        //                    using (OleDbDataAdapter Adapter = new OleDbDataAdapter(Command))
        //                    {
        //                        RetVal = new DataTable();
        //                        DataTable TempTable = new DataTable();
        //                        RetVal.Locale = CultureInfo.CurrentCulture;

        //                        Adapter.MissingMappingAction = MissingMappingAction.Passthrough;
        //                        Adapter.FillSchema(RetVal, SchemaType.Mapped);

        //                        for (int i = 5; i < RetVal.Columns.Count; i++)
        //                        {
        //                            RetVal.Columns[i].DataType = Type.GetType("System.String");
        //                        }
        //                        RetVal.AcceptChanges();

        //                        Adapter.Fill(RetVal);


        //                    }
        //                }
        //            }
        //        }
        //        catch
        //        {
        //            //-- reconnect to csv file by other OLEDB provider
        //            using (OleDbConnection CSVDBConnection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + PathOnly +
        //            ";Extended Properties=\"Text;HDR=" + Header + "\""))
        //            {
        //                using (OleDbCommand Command = new OleDbCommand(SqlQuery, CSVDBConnection))
        //                {
        //                    using (OleDbDataAdapter Adapter = new OleDbDataAdapter(Command))
        //                    {
        //                        RetVal = new DataTable();
        //                        RetVal.Locale = CultureInfo.CurrentCulture;
        //                        Adapter.MissingMappingAction = MissingMappingAction.Passthrough;
        //                        Adapter.Fill(RetVal);
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    finally
        //    {
        //    }

        //    return RetVal;

        //}


        public static DataTable GetCSVDataTable(string fileName, bool isRowOneHeader)
        {
            List<string> Columns = new List<string>();

            DataTable csvDataTable = new DataTable();

            //no try/catch - add these in yourselfs or let exception happen
            String[] csvData = File.ReadAllLines(fileName);

            //if no data in file ‘manually’ throw an exception
            if (csvData.Length == 0)
            {
                throw new Exception("CSV File Appears to be Empty");
            }

            String[] headings = csvData[0].Split(',');
            int index = 0; //will be zero or one depending on isRowOneHeader

            if (isRowOneHeader) //if first record lists headers
            {
                index = 1; //so we won’t take headings as data

                //for each heading
                Columns = GetCSVRecord(headings);

                int ColIndex = 0;
                while (true)
                {
                    if (ColIndex >= Columns.Count)
                    {
                        break;
                    }
                    if (Columns[ColIndex] == "")
                    {
                        Columns.RemoveAt(ColIndex);
                        ColIndex--;
                    }
                    ColIndex++;
                }

                foreach (string Col in Columns)
                {
                    csvDataTable.Columns.Add(Col.Trim('\"').Trim(), typeof(string));
                }
            }
            else //if no headers just go for col1, col2 etc.
            {
                Columns = GetCSVRecord(headings);
                for (int i = 0; i < Columns.Count; i++)
                {
                    //create arbitary column names
                    csvDataTable.Columns.Add("col" + (i + 1).ToString(), typeof(string));
                }
            }

            //populate the DataTable
            for (int i = index; i < csvData.Length; i++)
            {
                Columns = GetCSVRecord(csvData[i].Split(','));
                //create new rows
                DataRow row = csvDataTable.NewRow();
                if (Columns.Count > csvDataTable.Columns.Count)
                {

                }

                for (int j = 0; j < Columns.Count; j++)
                {
                    if (j < csvDataTable.Columns.Count)
                    {
                        //fill them
                        row[j] = Columns[j];
                    }
                }

                //add rows to over DataTable
                csvDataTable.Rows.Add(row);
            }
            csvDataTable.AcceptChanges();
            //return the CSV DataTable
            return csvDataTable;

        }

        private static List<string> GetCSVRecord(String[] csArray)
        {

            List<string> RetVal = new List<string>();
            bool StartFound = false;

            try
            {

                for (int i = 0; i < csArray.Length; i++)
                {
                    RetVal.Add(csArray[i]);
                }

                int ColCounter = RetVal.Count;
                int ColIndex = 0;

                while (ColIndex < ColCounter)
                {
                    if (ColIndex > 0)
                    {
                        if (RetVal[ColIndex].Contains("\""))
                        {
                            if (RetVal[ColIndex].StartsWith("\"") && !RetVal[ColIndex].EndsWith("\""))
                            {
                                StartFound = true;
                            }
                            else if (StartFound && RetVal[ColIndex].EndsWith("\""))
                            {
                                StartFound = false;
                                RetVal[ColIndex - 1] = RetVal[ColIndex - 1].Trim("\"".ToCharArray()) + "," + RetVal[ColIndex].Trim("\"".ToCharArray());
                                RetVal.RemoveAt(ColIndex);
                                ColCounter--;
                                ColIndex--;
                            }
                            else if (StartFound)
                            {
                                RetVal[ColIndex - 1] = RetVal[ColIndex - 1].Trim("\"".ToCharArray()) + "," + RetVal[ColIndex].Trim("\"".ToCharArray());
                                RetVal.RemoveAt(ColIndex);
                                ColCounter--;
                                ColIndex--;
                            }

                        }
                        else if (StartFound)
                        {
                            RetVal[ColIndex - 1] = RetVal[ColIndex - 1].Trim("\"".ToCharArray()) + "," + RetVal[ColIndex].Trim("\"".ToCharArray());
                            RetVal.RemoveAt(ColIndex);
                            ColCounter--;
                            ColIndex--;
                        }
                    }
                    ColIndex++;
                }

                for (int i = 0; i < RetVal.Count; i++)
                {
                    RetVal[i] = RetVal[i].Trim('\"');
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return RetVal;
        }


        #endregion

        #endregion




    }
}
