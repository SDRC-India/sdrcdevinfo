// **********************************************************************
// Program Name: DIExport.cs

// Developed By: DG7

// Creation date: 6- Feb- 2008

// Program Comments: 
// This Class contains business logic for exporting DevInfo Data from Database to other format like (DES (.xls), PDF, HTML)
// The scope of class is in UI application, Tools Export (DataAdmin), DataEntry (DataAdmin)

// **********************************************************************

// **********************Change history*********************************

// **********************************************************************
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableGraph;
using SpreadsheetGear;
using System.Resources;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.UI.Presentations;
using System.Xml.Serialization;
using System.Drawing;
using DevInfo.Lib.DI_LibBAL.UI.UserPreference;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.DataViewPage;
using DevInfo.Lib.DI_LibBAL.CommonDelegates;
using System.Runtime.InteropServices;
using DevInfo.Lib.DI_LibBAL.DA.DML;

namespace DevInfo.Lib.DI_LibBAL.Export
{


    #region "-- Delegates to display processing information --"

    /// <summary>
    /// A delegate for Process_IndicatorUnit event
    /// </summary>
    /// <param name="indicator">Indicator Name</param>
    public delegate void CurrentIndicatorUnitInfo(string indicator, string unit, int dataCount);

    /// <summary>
    /// A delegate for End_IndicatorUnit event
    /// </summary>
    public delegate void IndicatorUnitProcessed();


    # endregion

    /// <summary>
    /// DIExport class is used to export Data into various formats (Supported formats are mentioned in DIExportType Enum.) 
    /// <para>Data source must be provided in form of either DataView or Database connections with userSelections.</para>
    /// </summary>
    public class DIExport
    {

        #region "-- Private --"

        #region "-- Variables --"

        private static Dictionary<string, string> DIGlobalColumns = new Dictionary<string, string>();
        private const string SlNo = "SlNo";

        #endregion

        #region "-- Methods --"

        #region "-- General --"

        /// <summary>
        /// Removes invalid characters from a string to make it a valid fileName.
        /// </summary>
        /// <param name="fileName">FileName string to validate.</param>
        /// <returns>valid filename after special characters removed.</returns>
        public static string RemoveSpecialCharactersFromFileName(string fileName)
        {
            //TODO: move in Utility ??
            string RetVal = string.Empty;

            string InvalidChars = @"[\\\/:\*\?""<>|]";  // all possible invalid charaters within a filename.
            Regex oRegex = new Regex(InvalidChars);
            RetVal = oRegex.Replace(fileName, " ");

            return RetVal;
        }

        /// <summary>
        /// Return comma seperated string of distinct data values in Column specifed.
        /// </summary>
        private static string DataColumnValuesToString(DataTable table, string columnName)
        {
            string RetVal = string.Empty;
            string[] DistinctColumn = new string[1];
            DistinctColumn[0] = columnName;
            if (table != null && table.Columns.Contains(columnName))
            {
                DataTable Table2 = table.DefaultView.ToTable(true, DistinctColumn);
                foreach (DataRow dr in Table2.Rows)
                {
                    if (RetVal.Length == 0)
                    {
                        RetVal = dr[columnName].ToString();
                    }
                    else
                    {
                        RetVal += "," + dr[columnName].ToString();
                    }
                }
            }
            return RetVal;
        }

        /// <summary>
        /// It makes sort string to be used for sorting DataView.
        /// </summary>
        /// <param name="sortedFields">Field class object having sorted elements.</param>
        /// <returns>sort string</returns>
        private string GetSortString(Fields sortedFields)
        {
            string RetVal = string.Empty;

            try
            {
                if (!(sortedFields == null))
                {
                    foreach (Field field in sortedFields.Sort)
                    {
                        if (RetVal.Length == 0)
                        {
                            RetVal = field.FieldID + " " + field.SortType.ToString().ToUpper();
                        }
                        else
                        {
                            RetVal += ", " + field.FieldID + " " + field.SortType.ToString().ToUpper();
                        }
                    }
                }
            }
            catch (Exception)
            {
                RetVal = string.Empty;
                //throw;
            }
            return RetVal;
        }

        #endregion

        #region "-- DES / xls --"
        private void GetWorkbookFromResourceFile(string fileNameWpath)
        {
            //TODO remove this function
            try
            {
                if (File.Exists(fileNameWpath))
                {
                    File.Delete(fileNameWpath);
                }
                //Load the resource file

                ResourceManager rm = new ResourceManager("DevInfo.Lib.DI_LibBAL.Export.ExportR", this.GetType().Assembly);

                byte[] workbookBytes = (byte[])rm.GetObject("DataEntrySpreadsheet");

                FileStream fileStream = File.Create(fileNameWpath);
                fileStream.Write(workbookBytes, 0, workbookBytes.Length);
                fileStream.Close();

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        private string SetWorkbookName(string indicatorName, string unitName)
        {
            string RetVal = string.Empty;
            try
            {
                // Limit worksheet name lenght to 255 characters.
                if ((Constants.DESWorkbookNamePrefix.Length + indicatorName.Length + unitName.Length) > 248)
                {
                    indicatorName = indicatorName.Substring(0, indicatorName.Length - (Constants.DESWorkbookNamePrefix.Length + indicatorName.Length + unitName.Length - 248));
                }
                RetVal = Constants.DESWorkbookNamePrefix + "_" + indicatorName + "_" + unitName;

                // Remove special characters from fileName
                RetVal = DIExport.RemoveSpecialCharactersFromFileName(RetVal);

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        /// Adds DES worksheet in workbook passed and fills it with Data, provided in DataView, in DES format.
        /// </summary>
        private void ProcessWorkbook(bool singleWorkbook, ref IWorkbook DESWorkbook, DataView filteredDataView, int IUCounter, string xlsFileNameWPath, string languageFileNameWPath, DIConnection dBConnection, DIQueries dBQueries, string IndicatorNId, string UnitNId)
        {
            IWorksheet DESWorsheet;
            string IndicatorUnitWorkbookName = string.Empty;    // Workbook name for each Indicator + Unit
            int SheetCounter = 1;

            List<DataTable> TableLists = GetDataTableListInChunk(filteredDataView.ToTable(), 65500);

            // Checking if singleWorkbook = false then save each worksheet in seperate workbook
            if (singleWorkbook == false)
            {
                foreach (DataTable ChunkTable in TableLists)
                {
                    DataView TempView = ChunkTable.DefaultView;
                    // Update Sector , Class in DataView
                    DIExport.AddSectorClassInDataView(ref TempView, dBConnection, dBQueries, IndicatorNId, UnitNId);

                    // Set Indicator, Unit Name for progress bar event, and raise Progress_Increment event
                    this.RaiseProcessIndicatorUnitInfo(TempView[0][Indicator.IndicatorName].ToString(), TempView[0][Unit.UnitName].ToString(), filteredDataView.Count);
                    // Get a blank DES workbook from resource file, only at first time.
                    if (SheetCounter == 1)
                    {
                        IndicatorUnitWorkbookName = this.SetWorkbookName(TempView[0][Indicator.IndicatorName].ToString(), TempView[0][Unit.UnitName].ToString());

                        // Get a blank DES workbook from resource file, and save it as specified file.
                        ExportDES.GetWorkbookFromResourceFile(WorkbookType.DataEntrySpreadsheet, Path.Combine(Path.GetDirectoryName(xlsFileNameWPath), IndicatorUnitWorkbookName + ".xls"));

                        // Create Workbook instance from same file.
                        DESWorkbook = SpreadsheetGear.Factory.GetWorkbook(Path.Combine(Path.GetDirectoryName(xlsFileNameWPath), IndicatorUnitWorkbookName + ".xls"));

                        DESWorkbook.Worksheets[0].CopyBefore(DESWorkbook.Sheets[DESWorkbook.Worksheets.Count - 1]);
                        DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].Visible = SheetVisibility.Hidden;

                        DESWorsheet = DESWorkbook.Worksheets[0];
                        DESWorsheet.Name = "Data " + "1" + Convert.ToString(TableLists.Count > 1 ? "_" + SheetCounter.ToString() : String.Empty);
                    }
                    else
                    {

                        // Copy default worksheet before itself. (i.e. Default worksheet copying itself into another new worksheet)
                        // hence, newly created worksheet will always be at second last position.
                        DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].CopyBefore(DESWorkbook.Sheets[DESWorkbook.Worksheets.Count - 1]);

                        // Assign newly created workbook which is present at second last position.
                        DESWorsheet = DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 2];
                        DESWorsheet.Visible = SheetVisibility.Visible;
                        // Rename worksheet name to "Data C"  (C - SheetNumber)
                        DESWorsheet.Name = "Data " + "1" + "_" + SheetCounter.ToString();


                    }
                    // Fill worksheet with Data in DES format, passing worksheet reference to ExportDES class method
                    ExportDES.ExportDESData(DESWorsheet, TempView, languageFileNameWPath);

                    SheetCounter++;
                }

                if (!(DESWorkbook == null))
                {
                    if (DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].Name == Constants.DESWorkbookNamePrefix)
                    {
                        // Delete last worksheet which is left blank.
                        DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].Delete();

                        // Set first sheet as active sheet.
                        DESWorkbook.Worksheets[0].Select();
                    }
                }
                // Save workbook
                DESWorkbook.Save();

            }
            else
            {
                // Get a blank DES workbook from resource file, only at first time.
                if (IUCounter == 1)
                {
                    ExportDES.GetWorkbookFromResourceFile(WorkbookType.DataEntrySpreadsheet, xlsFileNameWPath);

                    DESWorkbook = SpreadsheetGear.Factory.GetWorkbook(xlsFileNameWPath);
                }

                foreach (DataTable ChunkTable in TableLists)
                {
                    DataView TempView = ChunkTable.DefaultView;
                    // Update Sector , Class in DataView
                    DIExport.AddSectorClassInDataView(ref TempView, dBConnection, dBQueries, IndicatorNId, UnitNId);

                    // Set Indicator, Unit Name for progress bar event, and raise Progress_Increment event
                    this.RaiseProcessIndicatorUnitInfo(TempView[0][Indicator.IndicatorName].ToString(), TempView[0][Unit.UnitName].ToString(), filteredDataView.Count);


                    // Copy default worksheet before itself. (i.e. Default worksheet copying itself into another new worksheet)
                    // hence, newly created worksheet will always be at second last position.
                    DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].CopyBefore(DESWorkbook.Sheets[DESWorkbook.Worksheets.Count - 1]);

                    // Assign newly created workbook which is present at second last position.
                    DESWorsheet = DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 2];

                    // Rename worksheet name to "Data C"  (C - SheetNumber)
                    DESWorsheet.Name = "Data " + IUCounter.ToString() + Convert.ToString(TableLists.Count > 1 ? "_" + SheetCounter.ToString() : String.Empty);


                    // Fill worksheet with Data in DES format, passing worksheet reference to ExportDES class method
                    ExportDES.ExportDESData(DESWorsheet, ChunkTable.DefaultView, languageFileNameWPath);

                    SheetCounter++;
                }


            }

        }


        /// <summary>
        /// Adds DES worksheet in workbook passed and fills it with Data, provided in DataView, in DES format.
        /// </summary>
        private void ProcessWorkbook(bool singleWorkbook, ref IWorkbook DESWorkbook, DataView filteredDataView, int IUCounter, string xlsFileNameWPath, string languageFileNameWPath)
        {
            IWorksheet DESWorsheet;
            string IndicatorUnitWorkbookName = string.Empty;    // Workbook name for each Indicator + Unit
            int SheetCounter = 1;

            List<DataTable> TableLists = GetDataTableListInChunk(filteredDataView.ToTable(), 65500);

            // Checking if singleWorkbook = false then save each worksheet in seperate workbook
            if (singleWorkbook == false)
            {
                foreach (DataTable ChunkTable in TableLists)
                {
                    DataView TempView = ChunkTable.DefaultView;
                    //// Update Sector , Class in DataView
                    //DIExport.AddSectorClassInDataView(ref TempView, dBConnection, dBQueries, IndicatorNId, UnitNId);

                    // Set Indicator, Unit Name for progress bar event, and raise Progress_Increment event
                    this.RaiseProcessIndicatorUnitInfo(TempView[0][Indicator.IndicatorName].ToString(), TempView[0][Unit.UnitName].ToString(), filteredDataView.Count);
                    // Get a blank DES workbook from resource file, only at first time.
                    if (SheetCounter == 1)
                    {
                        IndicatorUnitWorkbookName = this.SetWorkbookName(TempView[0][Indicator.IndicatorName].ToString(), TempView[0][Unit.UnitName].ToString());

                        // Get a blank DES workbook from resource file, and save it as specified file.
                        ExportDES.GetWorkbookFromResourceFile(WorkbookType.DataEntrySpreadsheet, Path.Combine(Path.GetDirectoryName(xlsFileNameWPath), IndicatorUnitWorkbookName + ".xls"));

                        // Create Workbook instance from same file.
                        DESWorkbook = SpreadsheetGear.Factory.GetWorkbook(Path.Combine(Path.GetDirectoryName(xlsFileNameWPath), IndicatorUnitWorkbookName + ".xls"));

                        DESWorkbook.Worksheets[0].CopyBefore(DESWorkbook.Sheets[DESWorkbook.Worksheets.Count - 1]);
                        DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].Visible = SheetVisibility.Hidden;

                        DESWorsheet = DESWorkbook.Worksheets[0];
                        DESWorsheet.Name = "Data " + "1" + Convert.ToString(TableLists.Count > 1 ? "_" + SheetCounter.ToString() : String.Empty);
                    }
                    else
                    {

                        // Copy default worksheet before itself. (i.e. Default worksheet copying itself into another new worksheet)
                        // hence, newly created worksheet will always be at second last position.
                        DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].CopyBefore(DESWorkbook.Sheets[DESWorkbook.Worksheets.Count - 1]);

                        // Assign newly created workbook which is present at second last position.
                        DESWorsheet = DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 2];
                        DESWorsheet.Visible = SheetVisibility.Visible;
                        // Rename worksheet name to "Data C"  (C - SheetNumber)
                        DESWorsheet.Name = "Data " + "1" + "_" + SheetCounter.ToString();


                    }
                    // Fill worksheet with Data in DES format, passing worksheet reference to ExportDES class method
                    ExportDES.ExportDESData(DESWorsheet, TempView, languageFileNameWPath);

                    SheetCounter++;
                }

                // Save workbook
                DESWorkbook.Save();

            }
            else
            {
                foreach (DataTable ChunkTable in TableLists)
                {
                    DataView TempView = ChunkTable.DefaultView;
                    //// Update Sector , Class in DataView
                    //DIExport.AddSectorClassInDataView(ref TempView, dBConnection, dBQueries, IndicatorNId, UnitNId);

                    // Set Indicator, Unit Name for progress bar event, and raise Progress_Increment event
                    this.RaiseProcessIndicatorUnitInfo(TempView[0][Indicator.IndicatorName].ToString(), TempView[0][Unit.UnitName].ToString(), filteredDataView.Count);
                    // Get a blank DES workbook from resource file, only at first time.
                    if (IUCounter == 1)
                    {
                        ExportDES.GetWorkbookFromResourceFile(WorkbookType.DataEntrySpreadsheet, xlsFileNameWPath);

                        DESWorkbook = SpreadsheetGear.Factory.GetWorkbook(xlsFileNameWPath);
                    }
                    // Copy default worksheet before itself. (i.e. Default worksheet copying itself into another new worksheet)
                    // hence, newly created worksheet will always be at second last position.
                    DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].CopyBefore(DESWorkbook.Sheets[DESWorkbook.Worksheets.Count - 1]);

                    // Assign newly created workbook which is present at second last position.
                    DESWorsheet = DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 2];

                    // Rename worksheet name to "Data C"  (C - SheetNumber)
                    DESWorsheet.Name = "Data " + IUCounter.ToString() + Convert.ToString(TableLists.Count > 1 ? "_" + SheetCounter.ToString() : String.Empty);


                    // Fill worksheet with Data in DES format, passing worksheet reference to ExportDES class method
                    ExportDES.ExportDESData(DESWorsheet, ChunkTable.DefaultView, languageFileNameWPath);

                    SheetCounter++;
                }


            }

        }

        /// <summary>
        /// Adds DES worksheet in workbook passed and fills it with Data, provided in DataView, in DES format.
        /// </summary>
        private void ProcessMICSDataWorkbook(ref IWorkbook DESWorkbook, DataView filteredDataView, int IUCounter, string xlsFileNameWPath, string languageFileNameWPath)
        {
            IWorksheet DESWorsheet;
            string IndicatorUnitWorkbookName = string.Empty;    // Workbook name for each Indicator + Unit
            int SheetCounter = 1;

            DataView TempView = filteredDataView;

            // Get a blank DES workbook from resource file, only at first time.
            if (IUCounter == 1)
            {
                //Load the resource file
                byte[] workbookBytes = Export.ExportR.MICS_Compiler_Template;
                try
                {
                    if (File.Exists(xlsFileNameWPath))
                    {
                        File.Delete(xlsFileNameWPath);
                    }

                    FileStream fileStream = File.Create(xlsFileNameWPath);
                    fileStream.Write(workbookBytes, 0, workbookBytes.Length);
                    fileStream.Close();
                }
                catch
                {
                }

                DESWorkbook = SpreadsheetGear.Factory.GetWorkbook(xlsFileNameWPath);
            }

            // Copy default worksheet before itself. (i.e. Default worksheet copying itself into another new worksheet)
            // hence, newly created worksheet will always be at second last position.
            DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].CopyBefore(DESWorkbook.Sheets[DESWorkbook.Worksheets.Count - 1]);

            // Assign newly created workbook which is present at second last position.
            DESWorsheet = DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 2];

            // Rename worksheet name to "Data C"  (C - SheetNumber)
            DESWorsheet.Name = "Data " + IUCounter.ToString();

            // Fill worksheet with Data in DES format, passing worksheet reference to ExportDES class method
            ExportDES.ExportMICSDESData(DESWorsheet, TempView, languageFileNameWPath);

            //- Set Hyperlinks on Indicator and Units in Index Sheet. 
            //- MICS Index sheet is first sheet in the workbook.
            ExportDES.SetWorksheetHyperlink(DESWorkbook, 0, Constants.MICSCompilerDESCells.IndexSheetListStartingRowIndex + IUCounter - 1, Constants.MICSCompilerDESCells.IndexSheetIndicatorColumnIndex, DESWorsheet.Name, "A1", TempView[0][Indicator.IndicatorName].ToString());
            ExportDES.SetWorksheetHyperlink(DESWorkbook, 0, Constants.MICSCompilerDESCells.IndexSheetListStartingRowIndex + IUCounter - 1, Constants.MICSCompilerDESCells.IndexSheetUnitColumnIndex, DESWorsheet.Name, "A1", TempView[0][Unit.UnitName].ToString());

            //- Set Hyperlinks on Sector(Title), Sheet Name
            if (TempView.Table.Columns.Contains(Constants.SectorColumnName))
            {
                ExportDES.SetWorksheetHyperlink(DESWorkbook, 0, Constants.MICSCompilerDESCells.IndexSheetListStartingRowIndex + IUCounter - 1, Constants.MICSCompilerDESCells.IndexSheetTopicColumnIndex, DESWorsheet.Name, "A1", TempView[0][Constants.SectorColumnName].ToString());
            }
            ExportDES.SetWorksheetHyperlink(DESWorkbook, 0, Constants.MICSCompilerDESCells.IndexSheetListStartingRowIndex + IUCounter - 1, Constants.MICSCompilerDESCells.IndexSheetSheetNoColumnIndex, DESWorsheet.Name, "A1", DESWorsheet.Name);

            SheetCounter++;
        }


        /// <summary>
        /// It prepares SQl query to get records each having blank DataValue.
        /// No. of records will be (ICNid x TimePeriodNId x AreaNid x SubgroupValNid).
        /// </summary>
        /// <param name="includeGUID">true, if subgroupGIS to be included.</param>
        /// <param name="userSelection">user Selections </param>
        /// <param name="dBQueries">dbQueries to get tableNames</param>
        /// <param name="IUSNIds"></param>
        /// <returns></returns>
        private static string GetMissingRecrodsDESQuery(bool includeGUID, UserSelection userSelection, DIQueries dBQueries, string IUSNIds)
        {
            string RetVal = string.Empty;

            try
            {

                StringBuilder SqlQuery = new StringBuilder();

                SqlQuery.Append(" SELECT TP." + Timeperiods.TimePeriod + ", A." + Area.AreaID + ", A." + Area.AreaName + ", \"\" AS " + Data.DataValue + ",");
                SqlQuery.Append(" S." + SubgroupVals.SubgroupVal + ",");

                if (includeGUID)
                {
                    SqlQuery.Append(" S." + SubgroupVals.SubgroupValGId + ",");
                }

                SqlQuery.Append(" IC." + IndicatorClassifications.ICName + ", \"\" AS FootNote, \"\" AS Data_Denominator,");
                SqlQuery.Append(" I." + Indicator.IndicatorName + ",I." + Indicator.IndicatorGId + " ,U." + Unit.UnitName + ",U." + Unit.UnitGId + " ");

                SqlQuery.Append(" FROM " + dBQueries.TablesName.IndicatorClassifications + " AS IC, " + dBQueries.TablesName.TimePeriod + " AS TP, " + dBQueries.TablesName.Area + " AS A," + dBQueries.TablesName.Indicator + " AS I ," + dBQueries.TablesName.Unit + " AS U, " + dBQueries.TablesName.SubgroupVals + " AS S");


                SqlQuery.Append(" WHERE IC." + IndicatorClassifications.ICType + " = 'SR' AND IC." + IndicatorClassifications.ICParent_NId + "<>-1 ");

                // Apply Filter for AreaNid, if present.
                if (userSelection.AreaNIds.Length > 0)
                {
                    SqlQuery.Append(" AND (A." + Area.AreaNId + " in (" + userSelection.AreaNIds + ")) ");
                }

                // Apply Filter for TimePeriodNid ,if present.
                if (userSelection.TimePeriodNIds.Length > 0)
                {
                    SqlQuery.Append(" AND  (TP." + Timeperiods.TimePeriodNId + " in (" + userSelection.TimePeriodNIds + ")) ");
                }

                // Apply Filter for SourceNIds ,if present.
                if (userSelection.SourceNIds.Length > 0)
                {

                    SqlQuery.Append(" AND  (IC." + IndicatorClassifications.ICNId + " in (" + userSelection.SourceNIds + ")) ");
                }

                SqlQuery.Append(" AND  EXISTS ( Select * from " + dBQueries.TablesName.IndicatorUnitSubgroup + " IUS where IUS." + Indicator_Unit_Subgroup.IndicatorNId + "=I." + Indicator.IndicatorNId + " and IUS." + Indicator_Unit_Subgroup.UnitNId + "=U." + Unit.UnitNId + " and IUS." + Indicator_Unit_Subgroup.SubgroupValNId + "= S." + SubgroupVals.SubgroupValNId + "  AND " + Indicator_Unit_Subgroup.IUSNId + " IN (" + IUSNIds + ")  ");

                SqlQuery.Append(" AND NOT EXISTS (Select * from " + dBQueries.TablesName.Data + " AS D where IUS." + Indicator_Unit_Subgroup.IUSNId + "= D." + Data.IUSNId + "   AND A." + Area.AreaNId + " = D." + Data.AreaNId + "  and D." + Data.SourceNId + "  = IC." + IndicatorClassifications.ICNId + "  and  TP." + Timeperiods.TimePeriodNId + "  = D." + Data.TimePeriodNId + "    ) )");

                SqlQuery.Append(" Order BY TP." + Timeperiods.TimePeriod + ", A." + Area.AreaID + ", S." + SubgroupVals.SubgroupVal + ", IC." + IndicatorClassifications.ICName + " ASC");



                RetVal = SqlQuery.ToString();

            }
            catch (Exception)
            {
            }
            return RetVal;

        }


        /// <summary>
        /// It prepares SQl query to get records each having blank DataValue.
        /// No. of records will be (ICNid x TimePeriodNId x AreaNid x SubgroupValNid).
        /// </summary>
        /// <param name="includeGUID">true, if subgroupGIS to be included.</param>
        /// <param name="userSelections">user Selections </param>
        /// <param name="dBQueries">dbQueries to get tableNames</param>
        /// <returns></returns>
        private static string ProcessSQLQueryForEmptyDES(bool includeGUID, UserSelection userSelection, DIQueries dBQueries)
        {
            string RetVal = string.Empty;

            try
            {

                RetVal = "SELECT TP." + Timeperiods.TimePeriod + ", A." + Area.AreaID + ", A." + Area.AreaName + ", \"\" AS " + Data.DataValue + ", S." + SubgroupVals.SubgroupVal + ", IC." + IndicatorClassifications.ICName + ", \"\" AS " + FootNotes.FootNote + ", \"\" AS " + Data.DataDenominator;
                if (includeGUID)// If GUID is required, then include GID columns.
                {
                    RetVal += ", S." + SubgroupVals.SubgroupValGId;
                }

                RetVal += " FROM " + dBQueries.TablesName.IndicatorClassifications + " AS IC, " + dBQueries.TablesName.TimePeriod + " AS TP, " + dBQueries.TablesName.Area + " AS A, " + dBQueries.TablesName.SubgroupVals + " AS S WHERE IC." + IndicatorClassifications.ICType + " = 'SR' AND IC." + IndicatorClassifications.ICParent_NId + " <>-1 ";

                // Apply Filter for AreaNid, if present.
                if (userSelection.AreaNIds.Length > 0)
                {
                    RetVal += " AND (A." + Area.AreaNId + " in (" + userSelection.AreaNIds + "))";
                }

                // Apply Filter for TimePeriodNid ,if present.
                if (userSelection.TimePeriodNIds.Length > 0)
                {
                    RetVal += " AND (TP." + Timeperiods.TimePeriodNId + " in (" + userSelection.TimePeriodNIds + "))";
                }

                // Apply Filter for SourceNIds ,if present.
                if (userSelection.SourceNIds.Length > 0)
                {
                    RetVal += " AND (IC." + IndicatorClassifications.ICNId + " in (" + userSelection.SourceNIds + "))";
                }

                // Apply Filter for SubgroupValNIds ,if present.
                if (userSelection.SubgroupValNIds.Length > 0)
                {
                    RetVal += " AND (S." + SubgroupVals.SubgroupValNId + " in (" + userSelection.SubgroupValNIds + "))";
                }

                // Set order by
                RetVal += " Order BY TP." + Timeperiods.TimePeriod + ", A." + Area.AreaID + ", S." + SubgroupVals.SubgroupVal + ", IC." + IndicatorClassifications.ICName + " ASC";

            }
            catch (Exception)
            {
            }
            return RetVal;

        }

        /// <summary>
        /// It extracts blank DES workbook from resource file and saves it as specified file name.
        /// </summary>
        /// <param name="fileNameWpath">Desired file name of workbook</param>
        private static DataTable GetDistinctIU(bool emptyDES, DIConnection dBConnection, DIQueries dBQueries, UserSelection userSelection)
        {
            DataTable RetVal = null;
            string[] IndicatorUnitColumns;
            DataTable DistinctIUS;
            DataRow IUNids;

            try
            {
                if (!emptyDES)
                {
                    // 1. Get Distinct IUS from Database.
                    DistinctIUS = dBConnection.ExecuteDataTable(dBQueries.IUS.GetAutoDistinctIUS(userSelection));

                    // 2. Get Distinct IU from IUS
                    IndicatorUnitColumns = new string[4];
                    IndicatorUnitColumns[0] = Indicator.IndicatorNId;
                    IndicatorUnitColumns[1] = Unit.UnitNId;
                    IndicatorUnitColumns[2] = Indicator.IndicatorName;
                    IndicatorUnitColumns[3] = Unit.UnitName;
                    if (userSelection.UnitNIds.Length > 0)
                    {
                        DistinctIUS.DefaultView.RowFilter = Unit.UnitNId + " IN (" + userSelection.UnitNIds + ")";
                    }
                    DistinctIUS.DefaultView.Sort = Indicator.IndicatorName + ", " + Unit.UnitName + " ASC";
                    RetVal = DistinctIUS.DefaultView.ToTable(true, IndicatorUnitColumns);
                }
                else
                {
                    // Incase of EmptyDES, all possible Indicator + Unit combinations will be made.
                    RetVal = new DataTable();
                    if (userSelection.ShowIUS)
                    {
                        // 1. Get Distinct IUS from Database.
                        DistinctIUS = dBConnection.ExecuteDataTable(dBQueries.IUS.GetIUS(FilterFieldType.NId, userSelection.IndicatorNIds, FieldSelection.Light));
                    }
                    else
                    {
                        // 1. Get Distinct IUS from Database.
                        DistinctIUS = dBConnection.ExecuteDataTable(dBQueries.IUS.GetIUSNIdByI_U_S(userSelection.IndicatorNIds, userSelection.UnitNIds, userSelection.SubgroupValNIds));
                    }



                    // 3. Get Distinct IU from IUS
                    IndicatorUnitColumns = new string[2];

                    IndicatorUnitColumns[0] = Indicator.IndicatorNId;
                    IndicatorUnitColumns[1] = Unit.UnitNId;
                    RetVal = DistinctIUS.DefaultView.ToTable(true, IndicatorUnitColumns);

                }

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }


        /// <summary>
        /// It prepares PresentationData using DIDataView class for specified Userselections.
        /// </summary>
        /// <param name="dBConnection"></param>
        /// <param name="dBQueries"></param>
        /// <param name="userSelection"></param>
        /// <param name="xmlLanguageFileNameWPath"></param>
        /// <returns></returns>
        private static DataView GetPresentationData(DIConnection dBConnection, DIQueries dBQueries, UserSelection userSelection, string xmlLanguageFileNameWPath)
        {
            DataView RetVal = null;

            //Create UserPreference required for DIDataView object.
            UserPreference UserPreference = new UserPreference(xmlLanguageFileNameWPath);

            UserPreference.UserSelection = userSelection;

            // prepare Map's dataview 
            DIDataView DIDataView = new DevInfo.Lib.DI_LibBAL.UI.DataViewPage.DIDataView(UserPreference, dBConnection, dBQueries, "", "");
            RetVal = DIDataView.GetAllDataByUserSelection();

            if (RetVal == null)
            {
                RetVal = dBConnection.ExecuteDataTable(dBQueries.Data.GetPresentationData(userSelection, dBConnection.ConnectionStringParameters.ServerType)).DefaultView;
            }

            return RetVal;
        }

        /// <summary>
        /// It prepares DataView having requred Data for DES generation. 
        /// <para>For empty DES, it prepares DataView where no DataValue presents against UserSelections.</para>
        /// </summary>
        /// <param name="includeGUID">whether GUID to be included or not.</param>
        /// <param name="userSelection">userSelection object</param>
        /// <param name="dBConnection">Source DBase DIConnection object</param>
        /// <param name="dBQueries">Source DBase DBQueries object.</param>
        /// <param name="indicatorNId">indicator NId for which DataView to be generated.</param>
        /// <param name="unitNID">unit NId for which DataView to be generated.</param>
        private static DataView GetMICSDataByIndicatorUnitArea(DIConnection dBConnection, DIQueries dBQueries, DataTable AllData, DataTable DistinctIUS, DataTable IC_IUSTable, string languageFileNameWPath, int areaNId, DataRow IndicatorUnitInfo_dataRow)
        {
            DataView RetVal = null;
            string SqlQuery = string.Empty;
            int indicatorNId = -1;
            int unitNId = -1;
            string IUSNIds = string.Empty;
            DataRow[] IUSRows = null;

            try
            {
                //-- CASE: where DATA Exists

                // Set filter for IndicatorNId, UnitNid (becoz in GetPresentationData() does not filter Data for same)
                indicatorNId = Convert.ToInt32(IndicatorUnitInfo_dataRow[Indicator.IndicatorNId]);
                unitNId = Convert.ToInt32(IndicatorUnitInfo_dataRow[Unit.UnitNId]);
                AllData.DefaultView.RowFilter = Indicator.IndicatorNId + " = " + indicatorNId + " AND " + Unit.UnitNId + "=" + Convert.ToInt32(IndicatorUnitInfo_dataRow[Unit.UnitNId]);  // by default, for specified unit_NId

                DataTable RetValTemp = AllData.DefaultView.ToTable();

                //- Add Dummy Row if no records are found for this Indicator
                if (RetValTemp.Rows.Count == 0)
                {
                    foreach (DataColumn col in RetValTemp.Columns)
                    {
                        col.AllowDBNull = true;
                    }
                    RetValTemp.AcceptChanges();
                    DataRow NewRow = RetValTemp.NewRow();
                    NewRow[Indicator.IndicatorNId] = indicatorNId;
                    NewRow[Indicator.IndicatorName] = IndicatorUnitInfo_dataRow[Indicator.IndicatorName].ToString();
                    NewRow[Indicator.IndicatorGId] = IndicatorUnitInfo_dataRow[Indicator.IndicatorGId].ToString();
                    NewRow[Unit.UnitNId] = unitNId;
                    NewRow[Unit.UnitName] = IndicatorUnitInfo_dataRow[Unit.UnitName].ToString();
                    NewRow[Unit.UnitGId] = IndicatorUnitInfo_dataRow[Unit.UnitGId].ToString();

                    IUSRows = DistinctIUS.Select(Indicator.IndicatorNId + "=" + indicatorNId + " AND " + Unit.UnitNId + "=" + unitNId);
                    if (IUSRows.Length > 0)
                    {
                        NewRow[Indicator_Unit_Subgroup.IUSNId] = IUSRows[0][Indicator_Unit_Subgroup.IUSNId];
                    }
                    RetValTemp.Rows.Add(NewRow);
                }

                RetVal = RetValTemp.DefaultView;

                // Change Data_Value column type to string.
                RetVal.Table.Columns[Data.DataValue].DataType = typeof(string);

                #region " Add Sector & Class "

                string Sector = string.Empty;
                string Class = string.Empty;

                //- Get distinct IUS 
                string DistinctIUSNids = DIConnection.GetDelimitedValuesFromDataTable(RetVal.Table, Indicator_Unit_Subgroup.IUSNId);

                // Get Sector & Class for I, U
                DataRow[] SectorRows = IC_IUSTable.Select(IndicatorClassifications.ICParent_NId + "= -1 AND " + IndicatorClassificationsIUS.IUSNId + " IN (" + DistinctIUSNids + ")");
                if (SectorRows.Length > 0)
                {
                    Sector = SectorRows[0][IndicatorClassifications.ICName].ToString();
                    string SectorNid = SectorRows[0][IndicatorClassifications.ICNId].ToString();

                    // Get ICName from Database as Class (First child for given SectorNID and IUSNIds)
                    DataRow[] ClassRows = IC_IUSTable.Select(IndicatorClassifications.ICParent_NId + "= " + SectorNid + " AND " + IndicatorClassificationsIUS.IUSNId + " IN (" + DistinctIUSNids + ")");
                    if (ClassRows.Length > 0)
                    {
                        Class = ClassRows[0][IndicatorClassifications.ICName].ToString();
                    }
                }

                // Fill DataView with sector & class
                DIExport.FillSectorClassInDataView(ref RetVal, Sector, Class);

                #endregion

                //- update IC_Name column values to MICS round
                //- Getting MICS Round value out of ICName .. e.g. IND_MICS3_2008
                string IC_Name = string.Empty;
                string[] IC_Array = null;
                foreach (DataRow dr in RetVal.Table.Rows)
                {
                    if (dr[IndicatorClassifications.ICName] != DBNull.Value)
                    {
                        IC_Name = Convert.ToString(dr[IndicatorClassifications.ICName]);
                        IC_Array = IC_Name.Split('_');
                        if (IC_Array.Length > 1)
                        {
                            dr[IndicatorClassifications.ICName] = IC_Array[1];
                        }
                    }
                }

                RetVal.Table.AcceptChanges();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (RetVal == null)
                {
                    RetVal = (new DataTable()).DefaultView;
                }
            }

            return RetVal;
        }


        #endregion

        /// <summary>
        /// It prepares DataView having requred Data for DES generation. 
        /// <para>For empty DES, it prepares DataView where no DataValue presents against UserSelections.</para>
        /// </summary>
        /// <param name="includeGUID">whether GUID to be included or not.</param>
        /// <param name="userSelection">userSelection object</param>
        /// <param name="dBConnection">Source DBase DIConnection object</param>
        /// <param name="dBQueries">Source DBase DBQueries object.</param>
        /// <param name="indicatorNId">indicator NId for which DataView to be generated.</param>
        /// <param name="unitNID">unit NId for which DataView to be generated.</param>
        private static DataView GetMissingRecords(bool includeGUID, UserSelection userSelection, DIConnection dBConnection, DIQueries dBQueries, string languageFileNameWPath, string indicatorNId, string unitNId
            )
        {
            DataView RetVal = null;
            string SqlQuery = string.Empty;


            string UserSelectiosIndicatorNids = string.Empty;
            string UserSelectiosUnitNids = string.Empty;
            string OriginalSubgroupNIds = string.Empty;
            string IUSNIds = string.Empty;
            bool UserSelectionShowIUS = false;
            DataTable AutoSubgroupVals = null;
            DataTable IUSTable;

            try
            {
                // If emptyDES = true, then get records having blank DataValue for every possoble combinations of Specified Area, Time, Source, Subgroup.


                // Preserve original SubgroupNIds into a temp
                OriginalSubgroupNIds = userSelection.SubgroupValNIds;

                //  If SubgroupVal is blank, then get all subgroups for which given IU are combined as IUS.
                if (userSelection.SubgroupValNIds.Length == 0)
                {
                    if (userSelection.ShowIUS & userSelection.IndicatorNIds.Length > 0)
                    {
                        // Get SubgroupNIDs for given IUSNId as userSelection.IndicatorNId
                        AutoSubgroupVals = dBConnection.ExecuteDataTable(dBQueries.IUS.GetIUS(FilterFieldType.NId, userSelection.IndicatorNIds, FieldSelection.Light));

                        // Set filter for given I, U
                        AutoSubgroupVals.DefaultView.RowFilter = Indicator.IndicatorNId + " = " + indicatorNId + " AND " + Unit.UnitNId + " = " + unitNId;

                        AutoSubgroupVals = AutoSubgroupVals.DefaultView.ToTable();
                    }
                    else
                    {
                        // Get SubgroupNIDs for given Indicator + Unit
                        AutoSubgroupVals = dBConnection.ExecuteDataTable(dBQueries.IUS.GetIUSNIdByI_U_S(indicatorNId, unitNId, string.Empty));
                    }

                    // Set those SubgroupNIds into userSelection
                    userSelection.SubgroupValNIds = DIExport.DataColumnValuesToString(AutoSubgroupVals, SubgroupVals.SubgroupValNId);
                }

                // get IUSNIds on the basis of IndicatorsNId,UnitsNId and subgroupValSNId

                SqlQuery = dBQueries.IUS.GetIUSByI_U_S(indicatorNId, unitNId, userSelection.SubgroupValNIds);

                IUSTable = dBConnection.ExecuteDataTable(SqlQuery);

                IUSNIds = DIConnection.GetDelimitedValuesFromDataTable(IUSTable, Indicator_Unit_Subgroup.IUSNId);

                // Build SQl query for empty DES. (DataValue column will be blank and dont include where data exists)
                SqlQuery = DIExport.GetMissingRecrodsDESQuery(includeGUID, userSelection, dBQueries, IUSNIds);

                // Get DataView 
                RetVal = dBConnection.ExecuteDataTable(SqlQuery).DefaultView;

                //////// Limit DataRows to 65,536 as Excels sheet has 65,536 rows limit.
                //////if (RetVal.Table.Rows.Count > 65500)
                //////{
                //////    for (int i = RetVal.Table.Rows.Count - 1; i > 65500; i--)
                //////    {
                //////        RetVal.Table.Rows[i].Delete();
                //////    }
                //////}
                //////RetVal.Table.AcceptChanges();

                // Set original SubgroupNIDs back into userSelection
                userSelection.SubgroupValNIds = OriginalSubgroupNIds;



                // Rename GID columns if not required.
                if (!(includeGUID))
                {
                    RetVal.Table.Columns[Indicator.IndicatorGId].ColumnName = "I_GID";
                    RetVal.Table.Columns[Unit.UnitGId].ColumnName = "U_GID";
                    if (RetVal.Table.Columns.Contains(SubgroupVals.SubgroupValGId))
                    {
                        RetVal.Table.Columns[SubgroupVals.SubgroupValGId].ColumnName = "S_GID";
                    }
                }

                // Update Sector , Class in DataView
                DIExport.AddSectorClassInDataView(ref RetVal, dBConnection, dBQueries, indicatorNId, unitNId);

                RetVal.Table.AcceptChanges();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (RetVal == null)
                {
                    RetVal = (new DataTable()).DefaultView;
                }
            }



            return RetVal;
        }


        #region "-- Helper function --"

        private static void AddSectorClassInDataView(ref DataView desDataView, DIConnection dBConnection, DIQueries dBQueries, string indicatorNId, string unitNId)
        {
            try
            {
                string[] SectorClass = new string[2];

                // Get Sector & Class for I, U
                SectorClass = DIExport.GetSectorClass(dBConnection, dBQueries, indicatorNId, unitNId);

                // Fill DataView with sector & class
                DIExport.FillSectorClassInDataView(ref desDataView, SectorClass[0], SectorClass[1]);

            }
            catch (Exception)
            {

            }

        }

        /// <summary>
        /// It gets Sector and Class (immediate Child sector) against IU specified.
        /// </summary>
        private static string[] GetSectorClass(DIConnection dBConnection, DIQueries dBQueries, string indicatorNId, string unitNId)
        {
            string[] RetVal = new string[2];    // Holds Sector and Class
            string DistinctIUSNids = string.Empty;
            string SectorNid = string.Empty;
            string SectorName = string.Empty;
            string ClassName = string.Empty;
            string[] IUSNidColumnName = new string[1];
            IUSNidColumnName[0] = Indicator_Unit_Subgroup.IUSNId;
            DataTable SectorTable = null;
            DataTable ClassTable = null;

            // Get Sector Name & class Name for IU specfied. 

            // Get IUSNids from Database in a comma seperated string.
            DataTable DistinctIUSTable = dBConnection.ExecuteDataTable(dBQueries.IUS.GetIUSNIdByI_U_S(indicatorNId, unitNId, string.Empty));

            foreach (DataRow IUSRow in DistinctIUSTable.Rows)
            {
                if (DistinctIUSNids.Length == 0)
                {
                    DistinctIUSNids = IUSRow[Indicator_Unit_Subgroup.IUSNId].ToString();
                }
                else
                {
                    DistinctIUSNids += ", " + IUSRow[Indicator_Unit_Subgroup.IUSNId].ToString();
                }
            }

            // Get ICNid AS SectorNid, IC_Name AS SectorName from Database.

            SectorTable = dBConnection.ExecuteDataTable(dBQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.ParentNId, "-1", DistinctIUSNids, ICType.Sector, FieldSelection.Light));
            if (SectorTable.Rows.Count > 0)
            {
                SectorName = SectorTable.Rows[0][IndicatorClassifications.ICName].ToString();
                SectorNid = SectorTable.Rows[0][IndicatorClassifications.ICNId].ToString();

                // Get ICName from Database as Class (First child for given SectorNID and IUSNIds)

                ClassTable = dBConnection.ExecuteDataTable(dBQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.ParentNId, SectorNid, DistinctIUSNids, ICType.Sector, FieldSelection.Light));
                if (ClassTable.Rows.Count > 0)
                {
                    ClassName = ClassTable.Rows[0][IndicatorClassifications.ICName].ToString();
                }
            }

            // Return Sector & Class
            RetVal[0] = SectorName;
            RetVal[1] = ClassName;

            return RetVal;
        }

        /// <summary>
        /// It add Columns for Sector & Class in DataView and fills first row with Sector & Class value specifed in string array.
        /// </summary>
        private static void FillSectorClassInDataView(ref DataView desDataView, string SectorName, string ClassName)
        {
            // Add Sector column in DataView
            if (!(desDataView.Table.Columns.Contains(Constants.SectorColumnName)))
            {
                desDataView.Table.Columns.Add(Constants.SectorColumnName, typeof(string));
            }

            // Add Class column in DataView
            if (!(desDataView.Table.Columns.Contains(Constants.ClassColumnName)))
            {
                desDataView.Table.Columns.Add(Constants.ClassColumnName);
            }

            if (!(string.IsNullOrEmpty(SectorName)))
            {
                if (desDataView.Table.Rows.Count > 0)
                {
                    // Add Sector Name & Class Name in first rows only.
                    // Adding Sector Class in one rows is enough for Export process.
                    DataRow[] dr = desDataView.Table.Select(Constants.SectorColumnName + " <> ''");
                    if (dr.Length > 0)
                    {
                        dr[0][Constants.SectorColumnName] = SectorName;
                        dr[0][Constants.ClassColumnName] = ClassName;
                    }
                    else
                    {
                        desDataView.Table.Rows[0][Constants.SectorColumnName] = SectorName;
                        desDataView.Table.Rows[0][Constants.ClassColumnName] = ClassName;
                    }
                }
                desDataView.Table.AcceptChanges();
            }
        }

        private static void UpdateIndicatorUnitInDataView(ref DataView FilteredDataView, DIConnection dBConnection, DIQueries dBQueries, string indicatorNId, string unitNId)
        {
            string IndicatorName = string.Empty;
            string IndicatorGId = string.Empty;
            string UnitName = string.Empty;
            string UnitGId = string.Empty;
            DataTable TempDT = null;

            //Get indicator, Unit - Name & GID
            TempDT = dBConnection.ExecuteDataTable(dBQueries.Indicators.GetIndicator(FilterFieldType.NId, indicatorNId, FieldSelection.Light));
            if (TempDT.Rows.Count > 0)
            {
                IndicatorName = TempDT.Rows[0][Indicator.IndicatorName].ToString();
                IndicatorGId = TempDT.Rows[0][Indicator.IndicatorGId].ToString();
            }

            TempDT = dBConnection.ExecuteDataTable(dBQueries.Unit.GetUnit(FilterFieldType.NId, unitNId));
            if (TempDT.Rows.Count > 0)
            {
                UnitName = TempDT.Rows[0][Unit.UnitName].ToString();
                UnitGId = TempDT.Rows[0][Unit.UnitGId].ToString();
            }

            // Fill Indicator, Unit in DataTable
            if (!(FilteredDataView.Table.Columns.Contains(Indicator.IndicatorName)))
            {
                FilteredDataView.Table.Columns.Add(Indicator.IndicatorName);
            }
            if (!(FilteredDataView.Table.Columns.Contains(Indicator.IndicatorGId)))
            {
                FilteredDataView.Table.Columns.Add(Indicator.IndicatorGId);
            }
            if (!(FilteredDataView.Table.Columns.Contains(Unit.UnitName)))
            {
                FilteredDataView.Table.Columns.Add(Unit.UnitName);
            }
            if (!(FilteredDataView.Table.Columns.Contains(Unit.UnitGId)))
            {
                FilteredDataView.Table.Columns.Add(Unit.UnitGId);
            }
            if (FilteredDataView.Table.Rows.Count > 0)
            {
                // Adding IndicatorName, GID, UnitName, GID only at first Row.
                DataRow[] dr = FilteredDataView.Table.Select(Indicator.IndicatorName + " <> ''");
                if (dr.Length > 0)
                {
                    dr[0][Indicator.IndicatorName] = IndicatorName;
                    dr[0][Indicator.IndicatorGId] = IndicatorGId;
                    dr[0][Unit.UnitName] = UnitName;
                    dr[0][Unit.UnitGId] = UnitGId;
                }
                else
                {
                    FilteredDataView.Table.Rows[0][Indicator.IndicatorName] = IndicatorName;
                    FilteredDataView.Table.Rows[0][Indicator.IndicatorGId] = IndicatorGId;
                    FilteredDataView.Table.Rows[0][Unit.UnitName] = UnitName;
                    FilteredDataView.Table.Rows[0][Unit.UnitGId] = UnitGId;
                }
            }

            FilteredDataView.Table.AcceptChanges();
        }

        /// <summary>
        /// It generates DataTable for specified ICType. DataTable will have all n-levels of IC in one row for each IUS.
        /// <para>Eg: there are 3 levels of Goals in UT_IndicatorClassification_en, then fields will be : Goal Label 1, Goal Label 2, Goal Label 3, Indicator_Name, Unit_Name, Subgroup_Name</para>
        /// </summary>
        /// <param name="iCType">IC type</param>
        /// <param name="dbConnection">source Database DIconnection object</param>
        /// <param name="dbQueries">source database DIQueries object</param>
        /// <returns></returns>
        private static DataTable GetDataViewForICExport(ICType iCType, DIConnection dbConnection, DIQueries dbQueries)
        {
            DataTable RetVal = new DataTable(); // Final DataTable contaning data for IC spreadsheet
            DataRow ICRow = null;
            DataTable TempTable = null;
            DataView DataViewIUS = null;

            DataRow[] objRow;
            DataRow NewRow;
            int MaxLevel = 0;   // Max levels present in database for specified ICType.
            string[] _Temp;
            int cc = 0;
            int TotalCols = 0;

            try
            {
                DataTable ICTable = new DataTable();
                DataColumn[] _PK = new DataColumn[1];
                {
                    _PK[0] = ICTable.Columns.Add("ID", typeof(int));
                    _PK[0].Unique = true;
                    ICTable.PrimaryKey = _PK;
                    ICTable.Columns.Add("Parent_ID", typeof(int));
                    ICTable.Columns.Add("LABEL", typeof(string));
                    ICTable.Columns.Add("ACT_LABEL", typeof(string));
                }

                // Get IC info (IC_Nid, IC_Name, IC_Parent_Nid) from Database, filtering ICType.
                TempTable = dbConnection.ExecuteDataTable(dbQueries.IndicatorClassification.GetIC(FilterFieldType.None, "", iCType, FieldSelection.Light));
                TempTable.DefaultView.Sort = IndicatorClassifications.ICParent_NId + " ASC";
                TempTable = TempTable.DefaultView.ToTable();
                foreach (DataRow Dr in TempTable.Rows)
                {
                    NewRow = ICTable.NewRow();
                    objRow = ICTable.Select("ID=" + Dr["IC_Parent_NID"].ToString());
                    NewRow["ID"] = Dr["IC_Nid"];
                    NewRow["Parent_ID"] = Dr["IC_Parent_NID"];
                    NewRow["ACT_LABEL"] = Dr["IC_Name"].ToString();
                    if (objRow.Length > 0)
                    {
                        _Temp = Strings.Split(objRow[0]["LABEL"].ToString(), "{[~-~]}", -1, CompareMethod.Text);
                        if (MaxLevel < _Temp.Length)
                            MaxLevel = _Temp.Length;
                        NewRow["LABEL"] = objRow[0]["LABEL"].ToString() + "{[~-~]}" + Dr["IC_Name"].ToString();
                    }
                    else
                    {
                        NewRow["LABEL"] = Dr["IC_Name"].ToString();
                    }
                    ICTable.Rows.Add(NewRow);
                }

                ICTable.AcceptChanges();

                // Calculating total required columns in DataTable.
                TotalCols = MaxLevel + 1 + 3;

                // -- Fill IUS

                int jj;

                object[] ArrTemp = new object[TotalCols];

                // Adding Columns for IC levels
                string sHeadVal = string.Empty;
                switch (iCType)
                {
                    case ICType.Sector:
                        sHeadVal = "SECTOR";
                        break;
                    case ICType.Goal:
                        sHeadVal = "GOAL";
                        break;
                    case ICType.CF:
                        sHeadVal = "CF";
                        break;
                    case ICType.Institution:
                        sHeadVal = "INSTITUTION";
                        break;
                    case ICType.Theme:
                        sHeadVal = "THEME";
                        break;
                    case ICType.Convention:
                        sHeadVal = "CONVENTION";
                        break;
                    case ICType.Source:
                        sHeadVal = "SOURCE";
                        break;
                }
                // -- Columns headings
                for (jj = 0; jj <= MaxLevel; jj++)
                {
                    int t = jj + 1;
                    RetVal.Columns.Add(sHeadVal + " Label " + t);
                }
                RetVal.Columns.Add("INDICATOR");
                // GetLngStringValue(oXmlLngFile, "INDICATOR")
                RetVal.Columns.Add("UNIT");
                // GetLngStringValue(oXmlLngFile, "UNIT")
                RetVal.Columns.Add("SUBGROUP");

                for (cc = 0; cc <= ICTable.Rows.Count - 1; cc++)
                {
                    // -- if the Number of Elements in the DataRow is same as MaxLevel then get the IUS combination for this level
                    _Temp = Strings.Split(ICTable.Rows[cc]["LABEL"].ToString(), "{[~-~]}", -1, CompareMethod.Text);
                    if ((_Temp.Length - 1) == MaxLevel)
                    {
                        // -- get the IUS for this DataRow
                        try
                        {

                            //string sqlString = "SELECT UT_Indicator_Unit_Subgroup.IUSNId, UT_Indicator_en.Indicator_Name, UT_Unit_en.Unit_Name, UT_Subgroup_Vals_en.Subgroup_Val " +
                            //    " FROM UT_Subgroup_Vals_en INNER JOIN (UT_Unit_en INNER JOIN ((UT_Indicator_en INNER JOIN UT_Indicator_Unit_Subgroup ON UT_Indicator_en.Indicator_NId = UT_Indicator_Unit_Subgroup.Indicator_NId) INNER JOIN UT_Indicator_Classifications_IUS ON UT_Indicator_Unit_Subgroup.IUSNId = UT_Indicator_Classifications_IUS.IUSNId) ON UT_Unit_en.Unit_NId = UT_Indicator_Unit_Subgroup.Unit_NId) ON UT_Subgroup_Vals_en.Subgroup_Val_NId = UT_Indicator_Unit_Subgroup.Subgroup_Val_NId " +
                            //    " WHERE UT_Indicator_Classifications_IUS.IC_NId = " + ICTable.Rows[cc]["ID"].ToString();
                            //DataViewIUS = dbConnection.ExecuteDataTable(sqlString).DefaultView;
                            DataViewIUS = dbConnection.ExecuteDataTable(dbQueries.IUS.GetIUSByIC(iCType, ICTable.Rows[cc]["ID"].ToString(), FieldSelection.Light)).DefaultView;
                        }
                        catch (Exception ex)
                        {
                            ExceptionHandler.ExceptionFacade.ThrowException(ex);
                        }
                        if (DataViewIUS.Count > 0)
                        {
                            foreach (DataRowView DVRow in DataViewIUS)
                            {
                                ICRow = RetVal.NewRow();

                                for (jj = 0; jj <= _Temp.Length - 1; jj++)
                                {
                                    //ArrTemp(jj) = _Temp(jj);
                                    ICRow[jj] = _Temp[jj];
                                }
                                ICRow[jj] = DVRow[Indicator.IndicatorName].ToString();
                                ICRow[jj + 1] = DVRow[Unit.UnitName].ToString();
                                ICRow[jj + 2] = DVRow[SubgroupVals.SubgroupVal].ToString();
                                //oColl.Add(ArrTemp);
                                RetVal.Rows.Add(ICRow);
                            }
                        }
                        else
                        {
                            ICRow = RetVal.NewRow();
                            for (jj = 0; jj <= _Temp.Length - 1; jj++)
                            {
                                ICRow[jj] = _Temp[jj];
                            }
                            ICRow[jj] = "";
                            ICRow[jj + 1] = "";
                            ICRow[jj + 2] = "";
                            RetVal.Rows.Add(ICRow);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;

        }

        /// <summary>
        /// Merges dataTable 'MainTable' with other three dataTables MetaDataIndicator, metaDataArea, metaDataSource, on the basis of common columnNames.
        /// </summary>
        /// <param name="mainTable">Main DataTable having columns which are also present in other three DataTables.</param>
        /// <param name="metadataIndicator">DataTable having metaDataInfo for indicators.</param>
        /// <param name="metadataArea">DataTable having metaDataInfo for Area.</param>
        /// <param name="metadataSource">DataTable having metaDataInfo for Indicators_classification.</param>
        /// <remarks>NOTE: Column (IndicatorNID, AreaID, ICNid must be unique in MainDataTable</remarks>
        private static DataTable JoinMainTableWithMDTables(DataTable mainTable, DataTable metadataIndicator, DataTable metadataArea, DataTable metadataSource)
        {
            DataTable RetVal = null;
            DataSet DSet = new DataSet();
            try
            {
                // Create copy of main Datatable
                RetVal = mainTable.Copy();
                RetVal.TableName = "MainTable";

                // Add all tables in DataSet
                DSet.Tables.Add(RetVal);
                DSet.Tables.Add(metadataIndicator);
                DSet.Tables.Add(metadataArea);
                DSet.Tables.Add(metadataSource);

                // Add relation ship between tables
                // --1. mainTable <-> metadataIndicator
                DSet.Relations.Add("MDIndicator", metadataIndicator.Columns[Indicator.IndicatorNId], RetVal.Columns[Indicator.IndicatorNId], false);

                // --2. MainTable <-> metadataArea
                DSet.Relations.Add("MDArea", metadataArea.Columns[Area.AreaNId], RetVal.Columns[Area.AreaNId], false);

                // --3. MainTable <-> metadataSource
                DSet.Relations.Add("MDSource", metadataSource.Columns[IndicatorClassifications.ICNId], RetVal.Columns[IndicatorClassifications.ICNId], false);


                // Add "MD_IND_" columns in mainTable
                foreach (DataColumn col in metadataIndicator.Columns)
                {
                    // Check columnName must start with "MD_IND_"
                    if (col.ColumnName.Contains(UserPreference.DataviewPreference.MetadataIndicator) & RetVal.Columns.Contains(col.ColumnName) == false)
                    {
                        RetVal.Columns.Add(col.ColumnName, col.DataType);
                        //RetVal.AcceptChanges();
                        // Add expression to get data.
                        //RetVal.Columns[col.ColumnName].Expression = "parent(MDIndicator)." + col.ColumnName;
                    }
                }

                // Add "MD_AREA_" columns in mainTable
                foreach (DataColumn col in metadataArea.Columns)
                {
                    // Check columnName must start with "MD_AREA_"
                    if (col.ColumnName.Contains("MD_AREA_") & RetVal.Columns.Contains(col.ColumnName) == false)
                    {
                        RetVal.Columns.Add(col.ColumnName, col.DataType);
                        // Add expression to get data.
                        //RetVal.Columns[col.ColumnName].Expression = "parent(MDArea)." + col.ColumnName;
                    }
                }

                // Add "MD_SRC_" columns in mainTable
                foreach (DataColumn col in metadataSource.Columns)
                {
                    // Check columnName must start with "MD_SRC_"
                    if (col.ColumnName.Contains(UserPreference.DataviewPreference.MetadataSource) & RetVal.Columns.Contains(col.ColumnName) == false)
                    {
                        RetVal.Columns.Add(col.ColumnName, col.DataType);
                        // Add expression to get data.
                        //RetVal.Columns[col.ColumnName].Expression = "parent(MDSource)." + col.ColumnName;
                    }
                }

                //Start Adding Data by adding expression on "MD_" columns

                DSet.AcceptChanges();

                RetVal.Columns["MD_IND_1"].Expression = "parent(MDIndicator)." + "MD_IND_1";
                RetVal.Columns["MD_AREA_1"].Expression = "parent(MDArea)." + "MD_AREA_1";
                RetVal.Columns["MD_SRC_1"].Expression = "parent(MDSource)." + "MD_SRC_1";
                DSet.AcceptChanges();
            }
            catch (Exception ex)
            {
                RetVal = null;
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            return RetVal;

        }

        #endregion

        #region "-- Raise Event mehtods --"

        /// <summary>
        /// To raise Process_IndicatorUnitInfo
        /// </summary>
        protected void RaiseProcessIndicatorUnitInfo(string indicator, string unit, int dataCount)
        {
            if (this.Process_IndicatorUnitInfo != null)
                this.Process_IndicatorUnitInfo(indicator, unit, dataCount);
        }

        /// <summary>
        /// To raise End_IndicatorUnit
        /// </summary>
        protected void RaiseEndIndicatorUnit()
        {
            if (this.End_IndicatorUnit != null)
                this.End_IndicatorUnit();
        }

        /// <summary>
        /// To raise ProgressBar_Increment
        /// </summary>
        protected void RaiseProgressBarIncrement(int value)
        {
            if (this.ProgressBar_Increment != null)
                this.ProgressBar_Increment(value);
        }

        /// <summary>
        /// To raise ProgressBar_Initialize
        /// </summary>
        protected void RaiseProgressBarInitialize(int maximumValue)
        {
            if (this.ProgressBar_Initialize != null)
                this.ProgressBar_Initialize(maximumValue);
        }

        /// <summary>
        /// To raise ProgressBar_Close
        /// </summary>
        protected void RaiseProgressBarClose()
        {
            if (this.ProgressBar_Close != null)
                this.ProgressBar_Close();
        }


        /// <summary>
        /// To raise Process_IndicatorUnitInfo for Export Databse
        /// </summary>
        internal static void RaiseExportProcessIndicatorUnitInfo(string indicator, string unit, int dataCount)
        {
            if (DIExport.Process_IndicatorUnitInfo_Event != null)
                DIExport.Process_IndicatorUnitInfo_Event(indicator, unit, dataCount);
        }

        /// <summary>
        /// To raise End_IndicatorUnit for Export Databse
        /// </summary>
        internal static void RaiseExportEndIndicatorUnit()
        {
            if (DIExport.End_IndicatorUnit_Event != null)
                DIExport.End_IndicatorUnit_Event();
        }

        /// <summary>
        /// To raise ProgressBar_Increment for Export Databse
        /// </summary>
        internal static void RaiseExportProgressBarIncrement(int value)
        {
            if (DIExport.ProgressBar_Increment_Event != null)
                DIExport.ProgressBar_Increment_Event(value);
        }

        /// <summary>
        /// To raise ProgressBar_Initialize for Export Databse
        /// </summary>
        internal static void RaiseExportProgressBarInitialize(int maximumValue)
        {
            if (DIExport.ProgressBar_Initialize_Event != null)
                DIExport.ProgressBar_Initialize_Event(maximumValue);
        }

        /// <summary>
        /// To raise ProgressBar_Close for Export Databse
        /// </summary>
        internal static void RaiseExportProgressBarClose()
        {
            if (DIExport.ProgressBar_Close_Event != null)
                DIExport.ProgressBar_Close_Event();
        }

        #endregion

        #region "-- Where Data Exists / Blank DES / DES for Missing records --"

        private DataColumn[] GetPrimaryKeyColumnForMissingDataDES(DataTable table, UserSelection userSelections)
        {
            DataColumn[] RetVal = null;

            RetVal = new DataColumn[] { table.Columns[Indicator.IndicatorName], table.Columns[Unit.UnitName], table.Columns[SubgroupVals.SubgroupVal], table.Columns[Timeperiods.TimePeriod], table.Columns[Area.AreaID], table.Columns[IndicatorClassifications.ICName] };

            return RetVal;
        }

        /// <summary>
        /// Generates blank DES 
        /// </summary>
        /// <param name="singleWorkbook"></param>
        /// <param name="dBConnection"></param>
        /// <param name="dBQueries"></param>
        /// <param name="userSelection"></param>
        /// <param name="sortedFields"></param>
        /// <param name="xlsFileNameWPath"></param>
        /// <param name="includeGUID"></param>
        /// <param name="languageFileNameWPath"></param>
        /// <returns></returns>
        private DataView GetBlankDESDataView(DataView blankDESDataView, string indicatorNId, string unitNId, bool singleWorkbook, DIConnection dBConnection, DIQueries dBQueries, UserSelection userSelection, bool includeGUID)
        {
            DataView RetVal = null;
            string SqlQuery = string.Empty;
            string OriginalSubgroupNIds = string.Empty;
            DataTable AutoSubgroupValsTable = null;
            IndicatorInfo IndicatorInfoObj;
            IndicatorBuilder IndicatorBuilderObj;
            UnitInfo UnitInfoObj;
            UnitBuilder UnitBuilderObj;
            DI6SubgroupValBuilder SGValBuilder;
            DI6SubgroupValInfo SGValINfo;
            DataTable DT;

            try
            {
                // 1. Get blank DES dataview only if it is null
                if (blankDESDataView == null)
                {
                    blankDESDataView = GetBlankDESDataViewWithoutIUS(userSelection, dBConnection, dBQueries);
                }

                // 2. get dAtaview for the given Indicator + Unit.

                try
                {
                    // 3.  Preserve original SubgroupNIds into a temp
                    OriginalSubgroupNIds = userSelection.SubgroupValNIds;

                    // 4.  If SubgroupVal is blank, then get all subgroups for which given IU are combined as IUS.
                    if (userSelection.SubgroupValNIds.Length == 0)
                    {
                        if (userSelection.ShowIUS & userSelection.IndicatorNIds.Length > 0)
                        {
                            // 4.1  Get SubgroupNIDs for given IUSNId as userSelection.IndicatorNId
                            AutoSubgroupValsTable = dBConnection.ExecuteDataTable(dBQueries.IUS.GetIUS(FilterFieldType.NId, userSelection.IndicatorNIds, FieldSelection.Light));

                            // 4.2 Set filter for given I, U
                            AutoSubgroupValsTable.DefaultView.RowFilter = Indicator.IndicatorNId + " = " + indicatorNId + " AND " + Unit.UnitNId + " = " + unitNId;

                            AutoSubgroupValsTable = AutoSubgroupValsTable.DefaultView.ToTable();
                        }
                        else
                        {
                            // 4.3 Get SubgroupNIDs for given Indicator + Unit
                            AutoSubgroupValsTable = dBConnection.ExecuteDataTable(dBQueries.IUS.GetIUSNIdByI_U_S(indicatorNId, unitNId, string.Empty));
                        }

                        // 4.4 Set those SubgroupNIds into userSelection
                        userSelection.SubgroupValNIds = DIExport.DataColumnValuesToString(AutoSubgroupValsTable, SubgroupVals.SubgroupValNId);
                    }

                    //////// get IUSNIds on the basis of IndicatorsNId,UnitsNId and subgroupValSNId

                    //////SqlQuery = dBQueries.IUS.GetIUSByI_U_S(indicatorNId, unitNId, userSelection.SubgroupValNIds);

                    //////IUSTable = dBConnection.ExecuteDataTable(SqlQuery);

                    //////IUSNIds = DIConnection.GetDelimitedValuesFromDataTable(IUSTable, Indicator_Unit_Subgroup.IUSNId);


                    //  **************************************************************************
                    // get indicator,unit and subgroup info
                    IndicatorBuilderObj = new IndicatorBuilder(dBConnection, dBQueries);
                    IndicatorInfoObj = IndicatorBuilderObj.GetIndicatorInfo(FilterFieldType.NId, indicatorNId, FieldSelection.Light);

                    UnitBuilderObj = new UnitBuilder(dBConnection, dBQueries);
                    UnitInfoObj = UnitBuilderObj.GetUnitInfo(FilterFieldType.NId, unitNId);

                    SGValBuilder = new DI6SubgroupValBuilder(dBConnection, dBQueries);

                    // 5. process and update filteredDataview  for all subgroup nids

                    if (RetVal == null)
                    {
                        RetVal = new DataView(blankDESDataView.Table.Copy());
                        RetVal.Table.Clear();
                        // add subgroup columns
                        if (includeGUID)
                        {
                            RetVal.Table.Columns.Add(SubgroupVals.SubgroupValGId);
                        }

                        RetVal.Table.Columns.Add(SubgroupVals.SubgroupVal);

                        // check and insert columns for indicator & unit 
                        if (!(RetVal.Table.Columns.Contains(Indicator.IndicatorName)))
                        {
                            RetVal.Table.Columns.Add(Indicator.IndicatorName);
                        }
                        if (!(RetVal.Table.Columns.Contains(Indicator.IndicatorGId)))
                        {
                            RetVal.Table.Columns.Add(Indicator.IndicatorGId);
                        }
                        if (!(RetVal.Table.Columns.Contains(Unit.UnitName)))
                        {
                            RetVal.Table.Columns.Add(Unit.UnitName);
                        }
                        if (!(RetVal.Table.Columns.Contains(Unit.UnitGId)))
                        {
                            RetVal.Table.Columns.Add(Unit.UnitGId);
                        }

                        // set indicator and unit values
                        RetVal.Table.Columns[Indicator.IndicatorGId].DefaultValue = IndicatorInfoObj.GID;
                        RetVal.Table.Columns[Indicator.IndicatorName].DefaultValue = IndicatorInfoObj.Name;
                        RetVal.Table.Columns[Unit.UnitGId].DefaultValue = UnitInfoObj.GID;
                        RetVal.Table.Columns[Unit.UnitName].DefaultValue = UnitInfoObj.Name;
                    }
                    else
                    {
                        RetVal.Table.Clear();
                    }

                    foreach (string SGNid in DICommon.SplitString(userSelection.SubgroupValNIds, ","))
                    {
                        //////// 5.1 if rows are morethan 65500 then dont add more rows
                        //////if (RetVal.Table.Rows.Count > 65500)
                        //////{
                        //////    break;
                        //////}
                        // 5.2 get subgroup val info
                        SGValINfo = SGValBuilder.GetSubgroupValInfo(FilterFieldType.NId, SGNid);

                        DT = blankDESDataView.Table.Copy();
                        DT.Clear();

                        // 5.3 add subgroup columns
                        if (includeGUID)
                        {
                            DT.Columns.Add(SubgroupVals.SubgroupValGId);
                            DT.Columns[SubgroupVals.SubgroupValGId].DefaultValue = SGValINfo.GID;
                        }

                        DT.Columns.Add(SubgroupVals.SubgroupVal);
                        DT.Columns[SubgroupVals.SubgroupVal].DefaultValue = SGValINfo.Name;

                        // 5.4 merge blank DES data view
                        DT.Merge(blankDESDataView.Table);

                        // 5.5 update Subgroup in BlankDESDataView 
                        RetVal.Table.Merge(DT);

                    }

                    //  **************************************************************************

                    //////// 5.6 Limit DataRows to 65,536 as Excels sheet has 65,536 rows limit.
                    //////if (RetVal.Table.Rows.Count > 65500)
                    //////{
                    //////    for (int i = RetVal.Table.Rows.Count - 1; i > 65500; i--)
                    //////    {
                    //////        RetVal.Table.Rows[i].Delete();
                    //////    }
                    //////}
                    //////RetVal.Table.AcceptChanges();

                    // 5.7 Set original SubgroupNIDs back into userSelection
                    userSelection.SubgroupValNIds = OriginalSubgroupNIds;




                    ////////// 5.9 Rename GID columns if not required.
                    ////////if (!(includeGUID))
                    ////////{
                    ////////    FilteredDataView.Table.Columns[Indicator.IndicatorGId].ColumnName = "I_GID";
                    ////////    FilteredDataView.Table.Columns[Unit.UnitGId].ColumnName = "U_GID";
                    ////////    if (FilteredDataView.Table.Columns.Contains(SubgroupVals.SubgroupValGId))
                    ////////    {
                    ////////        FilteredDataView.Table.Columns[SubgroupVals.SubgroupValGId].ColumnName = "S_GID";
                    ////////    }
                    ////////}

                    ////////// 5.10 Update Sector , Class in DataView
                    ////////DIExport.AddSectorClassInDataView(ref FilteredDataView, dBConnection, dBQueries, indicatorNId, unitNId);
                    ////////FilteredDataView.Table.AcceptChanges();

                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (RetVal == null)
                    {
                        RetVal = (new DataTable()).DefaultView;
                    }
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (RetVal == null)
                {
                    RetVal = (new DataTable()).DefaultView;
                }
            }
            return RetVal;
        }


        private DataView GetBlankDESDataViewWithoutIUS(UserSelection userSelection, DIConnection dBConnection, DIQueries dBQueries)
        {
            DataView RetVal = null;
            string SqlQuery = string.Empty;

            try
            {
                // get records having blank DataValue for every possoble combinations of Specified Area, Time, Source.
                // Build SQl query for empty DES. (DataValue, Denominator, subgroupval, subgroupval_gid columns will be blank)
                SqlQuery = this.GetQueryForEmptyDES(userSelection, dBQueries);

                // Get DataView 
                RetVal = dBConnection.ExecuteDataTable(SqlQuery).DefaultView;

                //////// Limit DataRows to 65,536 as Excels sheet has 65,536 rows limit.
                //////if (RetVal.Table.Rows.Count > 65500)
                //////{
                //////    for (int i = RetVal.Table.Rows.Count - 1; i > 65500; i--)
                //////    {
                //////        RetVal.Table.Rows[i].Delete();
                //////    }
                //////}
                //////RetVal.Table.AcceptChanges();

            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (RetVal == null)
                {
                    RetVal = (new DataTable()).DefaultView;
                }
            }

            return RetVal;
        }

        /// <summary>
        /// It prepares SQl query to get records each having blank DataValue.
        /// No. of records will be (ICNid x TimePeriodNId x AreaNid x SubgroupValNid).
        /// </summary>      
        /// <param name="userSelections">user Selections </param>
        /// <param name="dBQueries">dbQueries to get tableNames</param>
        /// <returns></returns>
        private string GetQueryForEmptyDES(UserSelection userSelection, DIQueries dBQueries)
        {
            string RetVal = string.Empty;

            try
            {

                RetVal = "SELECT ";

                // fetch timeperiod only if it available in userSelection
                if (userSelection.TimePeriodNIds.Length == 0)
                {
                    RetVal += " \"\" AS " + Timeperiods.TimePeriod + " ,";
                }
                else
                {
                    RetVal += " TP." + Timeperiods.TimePeriod + ",";
                }


                // fetch source only if it available in userSelection
                if (userSelection.SourceNIds.Length == 0)
                {
                    RetVal += " \"\" AS " + IndicatorClassifications.ICName + " ,";
                }
                else
                {
                    RetVal += "  IC." + IndicatorClassifications.ICName + ",";

                }


                RetVal += " A." + Area.AreaID + ", A." + Area.AreaName + ", \"\" AS " + Data.DataValue + ", \"\" AS " + FootNotes.FootNote + ", \"\" AS " + Data.DataDenominator;

                #region "-- From Clause--"

                // form clause
                RetVal += " FROM " + dBQueries.TablesName.Area + " AS A  ";

                // add source table, if source nids exists
                if (userSelection.SourceNIds.Length > 0)
                {
                    RetVal += ", " + dBQueries.TablesName.IndicatorClassifications + " AS IC ";
                }

                // add timeperiod table, if timeperiod nids exists
                if (userSelection.TimePeriodNIds.Length > 0)
                {
                    RetVal += ", " + dBQueries.TablesName.TimePeriod + " AS TP ";
                }
                #endregion

                #region "-- Where Clause --"

                // where clause
                RetVal += " WHERE 1=1 ";

                // Apply Filter for sourceNid, if present.
                if (userSelection.SourceNIds.Length > 0)
                {
                    RetVal += " And IC." + IndicatorClassifications.ICType + " = 'SR' AND IC." + IndicatorClassifications.ICParent_NId + " <>-1 ";
                }


                // Apply Filter for AreaNid, if present.
                if (userSelection.AreaNIds.Length > 0)
                {
                    RetVal += " AND (A." + Area.AreaNId + " in (" + userSelection.AreaNIds + "))";
                }

                // Apply Filter for TimePeriodNid ,if present.
                if (userSelection.TimePeriodNIds.Length > 0)
                {
                    RetVal += " AND (TP." + Timeperiods.TimePeriodNId + " in (" + userSelection.TimePeriodNIds + "))";
                }

                // Apply Filter for SourceNIds ,if present.
                if (userSelection.SourceNIds.Length > 0)
                {
                    RetVal += " AND (IC." + IndicatorClassifications.ICNId + " in (" + userSelection.SourceNIds + "))";
                }

                #endregion

                #region "-- Order By --"


                // Set order by
                RetVal += " Order BY ";
                if (userSelection.TimePeriodNIds.Length > 0)
                {
                    RetVal += " TP." + Timeperiods.TimePeriod + ", ";
                }

                RetVal += " A." + Area.AreaID;
                if (userSelection.SourceNIds.Length > 0)
                {
                    RetVal += ", IC." + IndicatorClassifications.ICName;

                }

                RetVal += " ASC";

                #endregion

            }
            catch (Exception)
            {
            }
            return RetVal;

        }


        /// <summary>
        /// It prepares DataView having requred Data for DES generation. 
        /// <para>For empty DES, it prepares DataView where no DataValue presents against UserSelections.</para>
        /// </summary>
        /// <param name="includeGUID">whether GUID to be included or not.</param>
        /// <param name="userSelection">userSelection object</param>
        /// <param name="dBConnection">Source DBase DIConnection object</param>
        /// <param name="dBQueries">Source DBase DBQueries object.</param>
        /// <param name="indicatorNId">indicator NId for which DataView to be generated.</param>
        /// <param name="unitNID">unit NId for which DataView to be generated.</param>
        private DataView GetDESDataViewWhereDataExits(bool includeGUID, UserSelection userSelection, DIConnection dBConnection, DIQueries dBQueries, string languageFileNameWPath, string indicatorNId, string unitNId
          )
        {
            DataView RetVal = null;
            string SqlQuery = string.Empty;


            string UserSelectiosIndicatorNids = string.Empty;
            string UserSelectiosUnitNids = string.Empty;
            string OriginalSubgroupNIds = string.Empty;
            string IUSNIds = string.Empty;
            bool UserSelectionShowIUS = false;
            DataTable AutoSubgroupVals = null;
            DataTable IUSTable;

            try
            {

                #region "-- where DATA Exists --"

                // Preserve indicatorNids & unitNids of UserSelection.
                UserSelectiosIndicatorNids = userSelection.IndicatorNIds;
                UserSelectiosUnitNids = userSelection.UnitNIds;

                // Set single specified indicatorNId, unitNid in UserSelection, to get PresentationData only for selected IU
                userSelection.IndicatorNIds = indicatorNId;
                userSelection.UnitNIds = unitNId;

                if (userSelection.ShowIUS)
                {
                    // Preserve ShowIUS property
                    UserSelectionShowIUS = userSelection.ShowIUS;
                    // Set ShowIUS = false
                    userSelection.ShowIUS = false;
                }


                // Get  Data for the given userSelection 

                RetVal = dBConnection.ExecuteDataTable(dBQueries.Data.GetDataNIDs(userSelection.IndicatorNIds, userSelection.TimePeriodNIds, userSelection.AreaNIds, userSelection.SourceNIds, userSelection.ShowIUS, FieldSelection.Heavy, false)).DefaultView;
                //DIExport.GetPresentationData(dBConnection, dBQueries, userSelection, languageFileNameWPath);


                // Restore UserSelection.ShowIUS 
                userSelection.ShowIUS = UserSelectionShowIUS;
                // Set back original IndicatorNid  unitNid back into UserSelection object.
                userSelection.IndicatorNIds = UserSelectiosIndicatorNids;
                userSelection.UnitNIds = UserSelectiosUnitNids;


                RetVal.RowFilter = Unit.UnitNId + "=" + unitNId;  // by default, for specified unit_NId
                // Set filter for UnitNid, SubgroupValNid and Source (becoz in GetPresentationData() does not filter Data for same)
                if (userSelection.SourceNIds.Length > 0)
                {
                    RetVal.RowFilter += " AND " + IndicatorClassifications.ICNId + " IN (" + userSelection.SourceNIds + ")";
                }
                if (userSelection.UnitNIds.Length > 0)
                {
                    RetVal.RowFilter += " AND " + Unit.UnitNId + " IN (" + userSelection.UnitNIds + ")";
                }
                if (userSelection.SubgroupValNIds.Length > 0)
                {
                    RetVal.RowFilter += " AND " + SubgroupVals.SubgroupValNId + " IN (" + userSelection.SubgroupValNIds + ")";
                }

                if ((userSelection.ShowIUS) && !string.IsNullOrEmpty(UserSelectiosIndicatorNids))
                {
                    //If ShowIUS = true, apply rowFilter for given indicatorNid and UnitNid (in parameter)
                    RetVal.RowFilter += " AND " + Indicator_Unit_Subgroup.IUSNId + " IN (" + UserSelectiosIndicatorNids + ")";
                    //RetVal.RowFilter += " AND " + Unit.UnitNId + " = " + unitNId;
                }


                DataTable RetValTemp = RetVal.ToTable();
                // Add Footnote in DataView, as GetPresentationData() does not include FootNote
                DIExport.AddFootNoteInDataTable(ref RetValTemp, dBConnection, dBQueries);
                RetVal = RetValTemp.DefaultView;
                // Change Data_Value column type to string.
                RetVal.Table.Columns[Data.DataValue].DataType = typeof(string);

                RetVal.Table.AcceptChanges();

                #endregion

            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (RetVal == null)
                {
                    RetVal = (new DataTable()).DefaultView;
                }
            }



            return RetVal;
        }

        #endregion


        #region "-- DataTable in Chunk--"

        private static DataTable AddSerialNoInTable(DataTable table)
        {
            DataTable RetVal = new DataTable();
            DataColumn dtCol = new DataColumn();

            dtCol.DataType = Type.GetType("System.Int32");
            dtCol.AutoIncrement = true;
            dtCol.ColumnName = DIExport.SlNo;
            dtCol.AutoIncrementSeed = 1;
            RetVal.Columns.Add(dtCol);

            RetVal.Merge(table, true);

            return RetVal;
        }

        private static List<DataTable> GetDataTableListInChunk(DataTable table, int chunkSize)
        {
            List<DataTable> RetVal = new List<DataTable>();
            DataView NewTableView = AddSerialNoInTable(table).DefaultView;
            try
            {
                for (int i = 0; i < table.Rows.Count; i += chunkSize)
                {
                    DataTable TempTable = null;

                    NewTableView.RowFilter = DIExport.SlNo + " > " + i + " AND " + DIExport.SlNo + " <= " + (i + chunkSize);
                    TempTable = NewTableView.ToTable();
                    //TempTable.Columns.Remove(DIExport.SlNo);
                    TempTable.AcceptChanges();

                    RetVal.Add(TempTable);

                }
            }
            catch (Exception ex)
            {
            }
            return RetVal;
        }

        #endregion

        #endregion

        #endregion

        #region "-- Internal --"
        /// <summary>
        /// It checks whether _Global columns (Indicator_Global, Unit_Global, SubgroupVal_Global, Area_Global, IC_Global) are existing in passed DataTable in perticular row. If pressnt then returns bool value.
        /// </summary>
        /// <param name="table">dataTable in which column to be checked</param>
        /// <param name="dr">dataRow</param>
        /// <param name="columnName">columnName</param>
        /// <returns>bool value of corresponding _Global column .</returns>
        internal static bool CheckGlobalColumnValue(DataTable table, DataRow dr, string columnName)
        {
            bool RetVal = false;
            //-- Get GlobalColumns Into Dictionary
            DIExport.DIGlobalColumns = DIExport.GetGlobalColumns();

            if (string.IsNullOrEmpty(columnName) == true)
            {
                columnName = string.Empty;
            }
            if (table != null && dr != null)
            {
                try
                {

                    foreach (string item in DIExport.DIGlobalColumns.Keys)
                    {
                        if (columnName == item)
                        {
                            if ((table.Columns.Contains(item)) && (table.Columns.Contains(DIExport.DIGlobalColumns[item])))
                            {
                                if (table.Columns[DIExport.DIGlobalColumns[item]].DataType == Type.GetType("System.Boolean"))
                                {
                                    RetVal = (bool)dr[DIExport.DIGlobalColumns[item]];
                                }
                                else
                                {
                                    RetVal = Convert.ToInt32(dr[DIExport.DIGlobalColumns[item]]) == 0 ? false : true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return RetVal;
        }

        private static Dictionary<string, string> GetGlobalColumns()
        {
            Dictionary<string, string> RetVal = new Dictionary<string, string>();

            RetVal.Add(Indicator.IndicatorName, Indicator.IndicatorGlobal);
            RetVal.Add(Indicator.IndicatorGId, Indicator.IndicatorGlobal);
            RetVal.Add(Unit.UnitName, Unit.UnitGlobal);
            RetVal.Add(Unit.UnitGId, Unit.UnitGlobal);
            RetVal.Add(SubgroupVals.SubgroupVal, SubgroupVals.SubgroupValGlobal);
            RetVal.Add(SubgroupVals.SubgroupValGId, SubgroupVals.SubgroupValGlobal);
            RetVal.Add(Area.AreaName, Area.AreaGlobal);
            RetVal.Add(Area.AreaID, Area.AreaGlobal);
            RetVal.Add(IndicatorClassifications.ICName, IndicatorClassifications.ICGlobal);
            RetVal.Add(IndicatorClassifications.ICGId, IndicatorClassifications.ICGlobal);

            return RetVal;
        }

        /// <summary>
        /// It checks whether _Global columns (Indicator_Global, Unit_Global, SubgroupVal_Global, Area_Global, IC_Global) are existing in passed DataTable in perticular row. If pressnt then returns bool value.
        /// </summary>
        /// <param name="table">dataTable in which column to be checked</param>
        /// <param name="dr">dataRow</param>
        /// <param name="columnName">columnName</param>
        /// <returns>bool value of corresponding _Global column .</returns>
        //internal static bool CheckGlobalColumnValue(DataTable table, DataRow dr, string columnName)
        //{
        //    bool RetVal = false;

        //    if (string.IsNullOrEmpty(columnName) == true)
        //    {
        //        columnName = string.Empty;
        //    }
        //    if (table != null && dr != null)
        //    {
        //        // Check if  name of Column exists in any eligible columns
        //        if (columnName == Indicator.IndicatorName || columnName == Indicator.IndicatorGId)
        //        {
        //            // Check if corresponding _Global column exists in table
        //            if (table.Columns.Contains(Indicator.IndicatorGlobal) && Information.IsDBNull(dr[Indicator.IndicatorGlobal]) == false)
        //            {
        //                RetVal = (bool)dr[Indicator.IndicatorGlobal];
        //            }
        //        }
        //        else if (columnName == Unit.UnitName || columnName == Unit.UnitGId)
        //        {
        //            // Check if corresponding _Global column exists in table
        //            if (table.Columns.Contains(Unit.UnitGlobal) && Information.IsDBNull(dr[Unit.UnitGlobal]) == false)
        //            {
        //                RetVal = (bool)dr[Unit.UnitGlobal];
        //            }
        //        }
        //        else if (columnName == SubgroupVals.SubgroupVal || columnName == SubgroupVals.SubgroupValGId)
        //        {
        //            // Check if corresponding _Global column exists in table
        //            if (table.Columns.Contains(SubgroupVals.SubgroupValGlobal) && Information.IsDBNull(dr[SubgroupVals.SubgroupValGlobal]) == false)
        //            {
        //                RetVal = (bool)dr[SubgroupVals.SubgroupValGlobal];
        //            }
        //        }
        //        else if (columnName == Area.AreaID || columnName == Area.AreaName)
        //        {
        //            // Check if corresponding _Global column exists in table
        //            if (table.Columns.Contains(Area.AreaGlobal) && Information.IsDBNull(dr[Area.AreaGlobal]) == false)
        //            {
        //                RetVal = (bool)dr[Area.AreaGlobal];
        //            }
        //        }
        //        else if (columnName == IndicatorClassifications.ICName || columnName == IndicatorClassifications.ICGId)
        //        {
        //            // Check if corresponding _Global column exists in table
        //            if (table.Columns.Contains(IndicatorClassifications.ICGlobal) && Information.IsDBNull(dr[IndicatorClassifications.ICGlobal]) == false)
        //            {
        //                RetVal = (bool)dr[IndicatorClassifications.ICGlobal];
        //            }
        //        }
        //    }

        //    return RetVal;
        //}

        /// <summary>
        /// Generally in Export process in UI or DataAdmin, few columns are not required in exported document(HTML, pdf, xls) but are presents in dataView
        ///<para>List of such columns are: indicator_Global, Unit_Global, Subgroup_Val_Global, Area_Global, IC_Global</para> 
        /// </summary>
        internal static bool CheckColumnRelevence(string columnName)
        {
            //Generally in Export process in UI, DataAdmin, few columns are not required export document but presents in dataView
            // List of such undisplayable columns are: indicator_Global, Unit_Global, Subgroup_Val_Global, Area_Global, IC_Global

            bool RetVal = true;
            if (string.IsNullOrEmpty(columnName) == false)
            {
                if (columnName == Indicator.IndicatorGlobal || columnName == Unit.UnitGlobal || columnName == SubgroupVals.SubgroupValGlobal || columnName == IndicatorClassifications.ICGlobal || columnName == Area.AreaGlobal)
                {
                    RetVal = false;
                }
            }
            return RetVal;
        }

        #endregion

        #region "-- Public --"

        #region "-- New/Dispose --"


        #endregion

        # region " -- Properties --"

        private static bool _CRINGExport = false;
        /// <summary>
        /// Get/Set CRINGExport
        /// </summary>
        public static bool CRINGExport
        {
            get { return _CRINGExport; }
            set { _CRINGExport = value; }
        }

        private static string _DataNIds = string.Empty;
        //Get Set DataNIds
        public static string DataNIds
        {
            get { return _DataNIds; }
            set { _DataNIds = value; }
        }
	
	

        # endregion

        # region "-- Methods --"

        #region "-- DES --"
        /// <summary>
        /// Generates DevInfo Data Entry Spreadsheet using DataView as data source.
        /// <para>DataView must have manadatory columns</para>
        /// <para>Mandatory columns (Indicator_Name, Unit_Name, Subgroup_Val, TimePeriod, AreaID, Area_Name, DataValue, ICSource, Footnote, DataDenominator)</para>
        /// <para>Optional columns: Indicator_GID, Unit_GID, Subgroup_Val_Gid, Sector (IC_Name), Class (IC_Name)</para>
        /// <para>To insert Sector & Class in a DataView, Developer can use DIExport.AddSectorClassInDataTable()</para>
        /// </summary>
        /// <param name="singleWorkbook">true, if single workbook is required.</param>
        /// <param name="desDataView">DataView containing Data required for DES generation. Columns required for DES are mentioned above.
        /// </param>
        /// <param name="xlsFileNameWPath">workbook file name with path.</param>
        /// <param name="languageFileNameWPath">Xml language file name with path, for language handling.</param>
        /// <returns></returns>
        public bool ExportDataEntrySpreadsheet(bool singleWorkbook, DataView desDataView, bool includeGUID, Fields sortedFields, string xlsFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;
            IWorkbook DESWorkbook = null;
            DataTable DistinctIU;
            string[] IndicatorUnitColumns = new string[2];
            string RowFilter = string.Empty;
            string SortFilter = string.Empty;
            int IUCounter = 0;
            int ProgressBarValue = 0;

            try
            {
                // 1. Check if DataView is valid to be used to generate DES.
                if ((DIExport.IsValidDESDataView(desDataView)))
                {
                    // 1) Remove GUID columns in desDataView , if GUID is not required.
                    if (!includeGUID)
                    {
                        if (desDataView.Table.Columns.Contains(Indicator.IndicatorGId))
                        {
                            desDataView.Table.Columns.Remove(Indicator.IndicatorGId);
                        }
                        if (desDataView.Table.Columns.Contains(Unit.UnitGId))
                        {
                            desDataView.Table.Columns.Remove(Unit.UnitGId);
                        }
                        if (desDataView.Table.Columns.Contains(SubgroupVals.SubgroupValGId))
                        {
                            desDataView.Table.Columns.Remove(SubgroupVals.SubgroupValGId);
                        }
                    }
                    desDataView.Table.AcceptChanges();

                    // 2. Get distinct Indicator, Unit from sourceDataView.
                    IndicatorUnitColumns[0] = Indicator.IndicatorName;
                    IndicatorUnitColumns[1] = Unit.UnitName;
                    DistinctIU = desDataView.ToTable(true, IndicatorUnitColumns);

                    // Initialize progress bar
                    this.RaiseProgressBarInitialize(DistinctIU.Rows.Count + 1);

                    // 4. Iterate through each Row having distinct Indicator + Unit.
                    foreach (DataRow dr in DistinctIU.Rows)
                    {

                        IUCounter++;
                        ProgressBarValue++;
                        this.RaiseProgressBarIncrement(ProgressBarValue);
                        this.RaiseEndIndicatorUnit();   //Indicates that previous indicator unit is processed.

                        // 4.1) Set rowfilter on sourceDataview
                        desDataView.RowFilter = Indicator.IndicatorName + " = '" + DICommon.RemoveQuotes(dr[Indicator.IndicatorName].ToString()) + "' AND " + Unit.UnitName + " = '" + DICommon.RemoveQuotes(dr[Unit.UnitName].ToString()) + "'";

                        // 4.2) Set sort order on desDataView.
                        desDataView.Sort = this.GetSortString(sortedFields);

                        // 4.3) Get DataView for selected Indicator & Unit , by applying rowfilter on sourceDataview
                        DataView FilteredDataView = desDataView.ToTable().DefaultView;
                        desDataView.RowFilter = string.Empty;
                        desDataView.Sort = string.Empty;

                        //// 4.4) Set indicator, unit Name, and DataCount in progress bar event
                        //this.RaiseProcessIndicatorUnitInfo(dr[Indicator.IndicatorName].ToString(), dr[Unit.UnitName].ToString(), FilteredDataView.Table.Rows.Count);

                        // 4.5) Process workbook. Fills it with Data fromm DataView.
                        this.ProcessWorkbook(singleWorkbook, ref DESWorkbook, FilteredDataView, IUCounter, xlsFileNameWPath, languageFileNameWPath);

                    }

                    //if (singleWorkbook)
                    //{
                    if (!(DESWorkbook == null))
                    {
                        // Delete last worksheet which is left blank.
                        DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].Delete();

                        // Set first sheet as active sheet.
                        DESWorkbook.Worksheets[0].Select();

                        // Save Workbook
                        DESWorkbook.Save();
                    }
                    //}

                    RetVal = true;

                }
                else
                {
                    // TODO: validate exception message.
                    throw new ApplicationException("Invalid dataview. Column missing.");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (!(DESWorkbook == null))
                {
                    DESWorkbook.Save();
                }
                // Close progress bar
                this.RaiseProcessIndicatorUnitInfo(string.Empty, string.Empty, 0);

                this.RaiseProgressBarClose();

            }

            return RetVal;
        }


        /// <summary>
        /// Generates DevInfo Data Entry Spreadsheet using Database as data source.
        /// </summary>
        /// <param name="singleWorkbook">true, if single workbook is required.</param>
        /// <param name="emptyDES">true, to get missing records</param>
        /// <param name="withWhereDataExists">true to get records where data exists</param>
        /// <param name="dBConnection">DbConnection object of source database.</param>
        /// <param name="dBQueries">DBQueries object of source database.</param>
        /// <param name="userSelection">UserSelection object containg NIDs of Indicator, Unit, Subgroup, Area, Source, TimePeriod</param>
        /// <param name="xlsFileNameWPath">workbook file name with path.</param>
        /// <param name="includeGUID">true, if GUID of I, U, S to be included in DES.</param>
        /// <param name="languageFileNameWPath">Xml language file name with path, for language handling.</param>
        /// <returns></returns>
        public bool ExportDataEntrySpreadsheet(bool singleWorkbook, bool emptyDES, bool withWhereDataExists, DIConnection dBConnection, DIQueries dBQueries, UserSelection userSelection, Fields sortedFields, string xlsFileNameWPath, bool includeGUID, string languageFileNameWPath)
        {
            bool RetVal = false;
            IWorkbook DESWorkbook = null;

            DataTable DistinctIU;
            String SqlQuery = string.Empty;
            DataView FilteredDataView = null;
            DataTable MissingRecordTable;
            DataTable WhereDataExistsTable;
            string RowFilter = string.Empty;
            int IUCounter = 0;
            int ProgressBarValue = 0;

            try
            {

                //1. Get Distinct IU
                DistinctIU = DIExport.GetDistinctIU(emptyDES, dBConnection, dBQueries, userSelection);

                // Initialize progress bar
                this.RaiseProgressBarInitialize(DistinctIU.Rows.Count );

                // 3. Iterate through each Row having distinct Indicator + Unit.
                foreach (DataRow dr in DistinctIU.Rows)
                {
                    //// Prepare DES DataView for given IU
                    //if (emptyDES && FilteredDataView != null)
                    //{
                    //    // If FilteredDataView is already made, Dataview will be same for every IU,
                    //    // then use existing Dataview
                    //    // Update Indicator & Unit in same DataView
                    //    DIExport.UpdateIndicatorUnitInDataView(ref FilteredDataView, dBConnection, dBQueries, dr[Indicator.IndicatorNId].ToString(), dr[Unit.UnitNId].ToString());

                    //    // Update Sector , Class in DataView
                    //    DIExport.AddSectorClassInDataView(ref FilteredDataView, dBConnection, dBQueries, dr[Indicator.IndicatorNId].ToString(), dr[Unit.UnitNId].ToString());

                    //}
                    //else

                    // get missing records with where data exists
                    if (emptyDES & withWhereDataExists)
                    {
                        //-- CASE: Missing Records + DATA Exists

                        //get records where data exists
                        WhereDataExistsTable = DIExport.GetDESDataViewForIU(includeGUID, false, userSelection, dBConnection, dBQueries, languageFileNameWPath, dr[Indicator.IndicatorNId].ToString(), dr[Unit.UnitNId].ToString()).Table;

                        //get missing records
                        MissingRecordTable = DIExport.GetMissingRecords(includeGUID, userSelection, dBConnection, dBQueries, languageFileNameWPath, dr[Indicator.IndicatorNId].ToString(), dr[Unit.UnitNId].ToString()).Table;


                        List<String> RequiredColumns = new List<string>();

                        foreach (DataColumn Column in MissingRecordTable.Columns)
                        {
                            if (Column.ColumnName != Data.DataDenominator)
                                RequiredColumns.Add(Column.ColumnName);
                        }

                        MissingRecordTable = MissingRecordTable.DefaultView.ToTable(true, RequiredColumns.ToArray());

                        // merge missing records & records where data exists

                        WhereDataExistsTable.Merge(MissingRecordTable);


                        // add missing columns
                        if (WhereDataExistsTable.Columns.Contains(Data.DataDenominator) == false)
                        {
                            WhereDataExistsTable.Columns.Add(Data.DataDenominator);
                        }
                        
                        FilteredDataView = WhereDataExistsTable.DefaultView;


                    }
                    else // get missing records or where data exists
                    {
                        //-- CASE 1: blank Records
                        //-- CASE 2: Where Data Exists 

                        FilteredDataView = DIExport.GetDESDataViewForIU(includeGUID, emptyDES, userSelection, dBConnection, dBQueries, languageFileNameWPath, dr[Indicator.IndicatorNId].ToString(), dr[Unit.UnitNId].ToString());
                    }

                     //raise Progressbar_Increment event

                    if (FilteredDataView.Count > 0)
                    {
                        // Set Indicator, Unit Name for progress bar event, and raise Progress_Increment event
                        //this.RaiseProcessIndicatorUnitInfo(FilteredDataView.Table.Rows[0][Indicator.IndicatorName].ToString(), FilteredDataView.Table.Rows[0][Unit.UnitName].ToString(), FilteredDataView.Table.Rows.Count);

                        // Set sort order on FilteredDataView.
                        FilteredDataView.Sort = this.GetSortString(sortedFields);

                        IUCounter++;
                        try
                        {
                            // Process Workbook by filling worksheet with DataView.
                            this.ProcessWorkbook(singleWorkbook, ref DESWorkbook, FilteredDataView, IUCounter, xlsFileNameWPath, languageFileNameWPath);
                        }
                        catch (Exception)
                        {
                        }
                    }

                    ProgressBarValue++;
                    this.RaiseProgressBarIncrement(ProgressBarValue); 
                }

                //if (singleWorkbook)
                //{
                if (!(DESWorkbook == null))
                {
                    // Delete last worksheet which is left blank.
                    DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].Delete();

                    // Set first sheet as active sheet.
                    DESWorkbook.Worksheets[0].Select();

                    // Save Workbook
                    DESWorkbook.Save();
                }
                //}
                RetVal = true;
            }

            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (!(DESWorkbook == null))
                {
                    DESWorkbook.Save();
                }
                // Close progress bar
                this.RaiseProcessIndicatorUnitInfo(string.Empty, string.Empty, 0);

                this.RaiseProgressBarClose();
            }
            return RetVal;
        }

        /// <summary>
        /// Exports Data Entry Spreadsheet for MICS Compiler. It exports workbook for area specified and generated Indicator-Unit data sheets.
        /// </summary>
        /// <param name="dBConnection"></param>
        /// <param name="dBQueries"></param>
        /// <param name="userSelection"></param>
        /// <param name="xlsFileNameWPath"></param>
        /// <returns></returns>
        public bool ExportMICSCompilerDES(DIConnection dBConnection, DIQueries dBQueries, int areaNId, string xlsFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;
            IWorkbook DESWorkbook = null;
            DataTable DistinctIUS = null;
            DataTable DistinctIU = null;
            DataTable AllData = null;
            DataTable IC_IUSTable = null;
            DataTable DistinctSubgroupVal = null;
            String SqlQuery = string.Empty;
            DataView FilteredDataView = null;
            int IUCounter = 0;
            UserSelection userSelection = new UserSelection();
            int ProgressBarValue = 0;

            try
            {

                // 1. Get all IUS for IC Sector with sorting on ICName, IndicatorName, UnitName, SubgroupVals
                DistinctIUS = dBConnection.ExecuteDataTable(dBQueries.IUS.GetDistinctIUSByIC(ICType.Sector, string.Empty, FieldSelection.Light, IndicatorClassifications.ICName + ", " + Indicator.IndicatorName + ", " + Unit.UnitName + ", " + SubgroupVals.SubgroupVal));

                //- Get Distinct IU out of Distinct IUS
                DistinctIU = DistinctIUS.DefaultView.ToTable(true, Indicator.IndicatorNId, Indicator.IndicatorName, Indicator.IndicatorGId, Unit.UnitNId, Unit.UnitName, Unit.UnitGId);
                //DistinctIU.DefaultView.Sort = Indicator.IndicatorName + " ASC";

                //- 2. Get all data for given Area from UT_Data
                // Set specified AreaNId in UserSelection, to get PresentationData only for selected Area
                userSelection.AreaNIds = areaNId.ToString();
                userSelection.ShowIUS = false;

                // Get Presentation Data for specified IU in userSelection using DIDataView class.
                AllData = DIExport.GetPresentationData(dBConnection, dBQueries, userSelection, languageFileNameWPath).Table;

                // Add Footnote in DataView, as GetPresentationData() does not include FootNote
                DIExport.AddFootNoteInDataTable(ref AllData, dBConnection, dBQueries);

                //- Get All IC_IUS at once
                IC_IUSTable = dBConnection.ExecuteDataTable(dBQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.None, "", string.Empty, ICType.Sector, FieldSelection.Light));

                //// Initialize progress bar
                //this.RaiseProgressBarInitialize(DistinctIU.Rows.Count + 1);

                // 3. Iterate through each Row having distinct Indicator + Unit.
                foreach (DataRowView drIndicatorUnit in DistinctIU.DefaultView)
                {
                    //// Prepare DES DataView for given IU
                    FilteredDataView = DIExport.GetMICSDataByIndicatorUnitArea(dBConnection, dBQueries, AllData, DistinctIUS, IC_IUSTable, languageFileNameWPath, areaNId, drIndicatorUnit.Row);

                    ProgressBarValue++;
                    this.RaiseProgressBarIncrement(ProgressBarValue);   //raise Progressbar_Increment event

                    // Set Indicator, Unit Name for progress bar event, and raise Progress_Increment event
                    this.RaiseProcessIndicatorUnitInfo(drIndicatorUnit[Indicator.IndicatorName].ToString(), drIndicatorUnit[Unit.UnitName].ToString(), FilteredDataView.Count);

                    if (FilteredDataView.Count > 0)
                    {
                        // Set sort order on FilteredDataView.
                        FilteredDataView.Sort = Timeperiods.TimePeriod + " ASC";

                        IUCounter++;
                        try
                        {
                            // Process Workbook by filling worksheet with DataView.
                            this.ProcessMICSDataWorkbook(ref DESWorkbook, FilteredDataView, IUCounter, xlsFileNameWPath, languageFileNameWPath);
                        }
                        catch (Exception)
                        {
                        }
                    }

                }

                #region "- INDEX Sheet -"

                //- Set AreaID and AreaName in Index Sheet. MICS Index sheet is first sheet in the workbook.
                IWorksheet MICSIndexWorsheet = DESWorkbook.Worksheets[0];

                DataTable dtArea = dBConnection.ExecuteDataTable(dBQueries.Area.GetArea(FilterFieldType.NId, areaNId.ToString(), DevInfo.Lib.DI_LibDAL.Queries.Area.Select.OrderBy.AreaNId));

                if (dtArea.Rows.Count > 0)
                {
                    MICSIndexWorsheet.Cells[Constants.MICSCompilerDESCells.IndexSheetAreaIDCell].Value = dtArea.Rows[0][Area.AreaID].ToString();
                    MICSIndexWorsheet.Cells[Constants.MICSCompilerDESCells.IndexSheetAreaNameCell].Value = dtArea.Rows[0][Area.AreaName].ToString();
                }

                //- Set Distinct Subgroups in Index Sheet. (This List is used as DropDown List Reference in subgroupVal Column in every Data Sheet)
                DistinctSubgroupVal = DistinctIUS.DefaultView.ToTable(true, SubgroupVals.SubgroupVal);
                foreach (DataRow drSG in DistinctSubgroupVal.Rows)
                {
                    MICSIndexWorsheet.Cells["G4"].Insert(InsertShiftDirection.Down);
                }
                ExportDES.PasteDataViewInSheet(MICSIndexWorsheet, DistinctSubgroupVal.DefaultView, Constants.MICSCompilerDESCells.IndexSheetSubgroupListCell);

                MICSIndexWorsheet.Protect(string.Empty);
                #endregion

                if (!(DESWorkbook == null))
                {
                    // Delete last worksheet which is left blank.
                    DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].Delete();

                    // Set first sheet as active sheet.
                    DESWorkbook.Worksheets[0].Select();

                    // Save Workbook
                    DESWorkbook.Save();
                }

                RetVal = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {

                if (!(DESWorkbook == null))
                {
                    DESWorkbook.Save();
                }

                //this.RaiseProgressBarClose();
            }
            return RetVal;
        }

        /// <summary>
        /// It prepares DataView having requred Data for DES generation. 
        /// <para>For empty DES, it prepares DataView where no DataValue presents against UserSelections.</para>
        /// </summary>
        /// <param name="includeGUID">whether GUID to be included or not.</param>
        /// <param name="userSelection">userSelection object</param>
        /// <param name="dBConnection">Source DBase DIConnection object</param>
        /// <param name="dBQueries">Source DBase DBQueries object.</param>
        /// <param name="indicatorNId">indicator NId for which DataView to be generated.</param>
        /// <param name="unitNID">unit NId for which DataView to be generated.</param>
        public static DataView GetDESDataViewForIU(bool includeGUID, bool emptyDES, UserSelection userSelection, DIConnection dBConnection, DIQueries dBQueries, string languageFileNameWPath, string indicatorNId, string unitNId
          )
        {
            DataView RetVal = null;
            string SqlQuery = string.Empty;


            string UserSelectiosIndicatorNids = string.Empty;
            string UserSelectiosUnitNids = string.Empty;
            string OriginalSubgroupNIds = string.Empty;
            string IUSNIds = string.Empty;
            bool UserSelectionShowIUS = false;
            DataTable AutoSubgroupVals = null;
            DataTable IUSTable;

            try
            {
                // If emptyDES = true, then get records having blank DataValue for every possoble combinations of Specified Area, Time, Source, Subgroup.
                if (emptyDES)
                {
                    // Preserve original SubgroupNIds into a temp
                    OriginalSubgroupNIds = userSelection.SubgroupValNIds;

                    //  If SubgroupVal is blank, then get all subgroups for which given IU are combined as IUS.
                    if (userSelection.SubgroupValNIds.Length == 0)
                    {
                        if (userSelection.ShowIUS & userSelection.IndicatorNIds.Length > 0)
                        {
                            // Get SubgroupNIDs for given IUSNId as userSelection.IndicatorNId
                            AutoSubgroupVals = dBConnection.ExecuteDataTable(dBQueries.IUS.GetIUS(FilterFieldType.NId, userSelection.IndicatorNIds, FieldSelection.Light));

                            // Set filter for given I, U
                            AutoSubgroupVals.DefaultView.RowFilter = Indicator.IndicatorNId + " = " + indicatorNId + " AND " + Unit.UnitNId + " = " + unitNId;

                            AutoSubgroupVals = AutoSubgroupVals.DefaultView.ToTable();
                        }
                        else
                        {
                            // Get SubgroupNIDs for given Indicator + Unit
                            AutoSubgroupVals = dBConnection.ExecuteDataTable(dBQueries.IUS.GetIUSNIdByI_U_S(indicatorNId, unitNId, string.Empty));
                        }

                        // Set those SubgroupNIds into userSelection
                        userSelection.SubgroupValNIds = DIExport.DataColumnValuesToString(AutoSubgroupVals, SubgroupVals.SubgroupValNId);
                    }

                    // get IUSNIds on the basis of IndicatorsNId,UnitsNId and subgroupValSNId

                    SqlQuery = dBQueries.IUS.GetIUSByI_U_S(indicatorNId, unitNId, userSelection.SubgroupValNIds);

                    IUSTable = dBConnection.ExecuteDataTable(SqlQuery);

                    IUSNIds = DIConnection.GetDelimitedValuesFromDataTable(IUSTable, Indicator_Unit_Subgroup.IUSNId);

                    // Build SQl query for empty DES. (DataValue column will be blank)
                    SqlQuery = DIExport.ProcessSQLQueryForEmptyDES(includeGUID, userSelection, dBQueries);


                    // Get DataView 
                    RetVal = dBConnection.ExecuteDataTable(SqlQuery).DefaultView;

                    // Limit DataRows to 65,536 as Excels sheet has 65,536 rows limit.
                    //if (RetVal.Table.Rows.Count > 65500)
                    //{                        
                    //    for (int i = RetVal.Table.Rows.Count - 1; i > 65500; i--)
                    //    {
                    //        RetVal.Table.Rows[i].Delete();
                    //    }
                    //}
                    //RetVal.Table.AcceptChanges();


                    // Update Indicator Unit in DataView
                    DIExport.UpdateIndicatorUnitInDataView(ref RetVal, dBConnection, dBQueries, indicatorNId, unitNId);

                    // Set original SubgroupNIDs back into userSelection
                    userSelection.SubgroupValNIds = OriginalSubgroupNIds;
                }
                else
                {
                    //-- CASE: where DATA Exists

                    // Preserve indicatorNids & unitNids of UserSelection.
                    UserSelectiosIndicatorNids = userSelection.IndicatorNIds;
                    UserSelectiosUnitNids = userSelection.UnitNIds;

                    // Set single specified indicatorNId, unitNid in UserSelection, to get PresentationData only for selected IU
                    userSelection.IndicatorNIds = indicatorNId;
                    userSelection.UnitNIds = unitNId;

                    if (userSelection.ShowIUS)
                    {
                        // Preserve ShowIUS property
                        UserSelectionShowIUS = userSelection.ShowIUS;
                        // Set ShowIUS = false
                        userSelection.ShowIUS = false;
                    }


                    // Get Presentation Data for specified IU in userSelection using DIDataView class.

                    RetVal = DIExport.GetPresentationData(dBConnection, dBQueries, userSelection, languageFileNameWPath);


                    // Restore UserSelection.ShowIUS 
                    userSelection.ShowIUS = UserSelectionShowIUS;
                    // Set back original IndicatorNid  unitNid back into UserSelection object.
                    userSelection.IndicatorNIds = UserSelectiosIndicatorNids;
                    userSelection.UnitNIds = UserSelectiosUnitNids;


                    RetVal.RowFilter = Unit.UnitNId + "=" + unitNId;  // by default, for specified unit_NId
                    // Set filter for UnitNid, SubgroupValNid and Source (becoz in GetPresentationData() does not filter Data for same)
                    if (userSelection.SourceNIds.Length > 0)
                    {
                        RetVal.RowFilter += " AND " + IndicatorClassifications.ICNId + " IN (" + userSelection.SourceNIds + ")";
                    }
                    if (userSelection.UnitNIds.Length > 0)
                    {
                        RetVal.RowFilter += " AND " + Unit.UnitNId + " IN (" + userSelection.UnitNIds + ")";
                    }
                    if (userSelection.SubgroupValNIds.Length > 0)
                    {
                        RetVal.RowFilter += " AND " + SubgroupVals.SubgroupValNId + " IN (" + userSelection.SubgroupValNIds + ")";
                    }

                    if ((userSelection.ShowIUS))
                    {
                        //If ShowIUS = true, apply rowFilter for given indicatorNid and UnitNid (in parameter)
                        RetVal.RowFilter += " AND " + Indicator_Unit_Subgroup.IUSNId + " IN (" + UserSelectiosIndicatorNids + ")";
                        //RetVal.RowFilter += " AND " + Unit.UnitNId + " = " + unitNId;
                    }




                    DataTable RetValTemp = RetVal.ToTable();
                    // Add Footnote in DataView, as GetPresentationData() does not include FootNote
                    DIExport.AddFootNoteInDataTable(ref RetValTemp, dBConnection, dBQueries);
                    RetVal = RetValTemp.DefaultView;
                    // Change Data_Value column type to string.
                    RetVal.Table.Columns[Data.DataValue].DataType = typeof(string);
                }

                // Rename GID columns if not required.
                if (!(includeGUID))
                {
                    RetVal.Table.Columns[Indicator.IndicatorGId].ColumnName = "I_GID";
                    RetVal.Table.Columns[Unit.UnitGId].ColumnName = "U_GID";
                    if (RetVal.Table.Columns.Contains(SubgroupVals.SubgroupValGId))
                    {
                        RetVal.Table.Columns[SubgroupVals.SubgroupValGId].ColumnName = "S_GID";
                    }
                }

                // Update Sector , Class in DataView
                DIExport.AddSectorClassInDataView(ref RetVal, dBConnection, dBQueries, indicatorNId, unitNId);

                RetVal.Table.AcceptChanges();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (RetVal == null)
                {
                    RetVal = (new DataTable()).DefaultView;
                }
            }



            return RetVal;
        }

        /// <summary>
        /// It validates Dataview checking existance of required columns for Data Entry spreadsheet.
        /// </summary>
        /// <param name="desDataview">DataView to check.</param>
        /// <returns>true, if a valid dataView.</returns>
        public static bool IsValidDESDataView(DataView desDataview)
        {
            bool RetVal = false;
            if (!(desDataview == null) & (desDataview.Table.Columns.Contains(Indicator.IndicatorName) & desDataview.Table.Columns.Contains(Unit.UnitName) & desDataview.Table.Columns.Contains(SubgroupVals.SubgroupVal)
                & desDataview.Table.Columns.Contains(Timeperiods.TimePeriod) & desDataview.Table.Columns.Contains(Area.AreaID) & desDataview.Table.Columns.Contains(Area.AreaName) & desDataview.Table.Columns.Contains(Data.DataValue)
                & desDataview.Table.Columns.Contains(IndicatorClassifications.ICName) & desDataview.Table.Columns.Contains(FootNotes.FootNote) & desDataview.Table.Columns.Contains(Data.DataDenominator)))
            {
                RetVal = true;
            }
            return RetVal;
        }

        #endregion

        #region "-- Database --"

        /// <summary>
        /// Exports the data from one specified source database into new Access database on the basis of UserSelections.
        /// </summary>
        /// <param name="userSelection">DAL.UserSelection object having NIDs of Indicators, Units, SubgroupVal, Area, TimePeriod, ICs, Source.</param>
        /// <param name="maintainTemplate">bool true if database template is to maintained and only dateValue will be deleted</param>
        /// <param name="sourceDBConnection">DIConnection object of source database. Source database can be of any type.</param>
        /// <param name="destinationDBNameWPath">Destination Access database name with Path.</param>
        /// <param name="tempFolderPath">temp folder path.</param>
        /// <returns>true, if data is exported succesfully.</returns>
        public static bool ExportDatabase(UserSelection userSelection, DIConnection sourceDBConnection, bool maintainTemplate, string destinationDBNameWPath, string tempFolderPath)
        {
            return DIExport.ExportDatabase(userSelection, -1, sourceDBConnection, null, maintainTemplate, destinationDBNameWPath, tempFolderPath);
        }

        public static bool ExportDatabase(UserSelection userSelection, int areaLevel, DIConnection sourceDBConnection, bool maintainTemplate, string destinationDBNameWPath, string tempFolderPath)
        {
            return DIExport.ExportDatabase(userSelection, areaLevel, sourceDBConnection, null, maintainTemplate, destinationDBNameWPath, tempFolderPath);
        }

        public static bool ExportDatabase(UserSelection userSelection, DIConnection sourceDBConnection, DIQueries sourceDBQueries, bool maintainTemplate, string destinationDBNameWPath, string tempFolderPath)
        {
            return DIExport.ExportDatabase(userSelection, -1, sourceDBConnection, sourceDBQueries, maintainTemplate, destinationDBNameWPath, tempFolderPath);
        }

        /// <summary>
        /// Exports the data from one specified source database into new Access database on the basis of UserSelections.
        /// </summary>
        /// <param name="userSelection">DAL.UserSelection object having NIDs of Indicators, Units, SubgroupVal, Area, TimePeriod, ICs, Source.</param>
        /// <param name="areaLevel"></param>
        /// <param name="maintainTemplate">bool true if database template is to maintained and only dateValue will be deleted</param>
        /// <param name="sourceDBConnection">DIConnection object of source database. Source database can be of any type.</param>
        /// <param name="destinationDBNameWPath">Destination Access database name with Path.</param>
        /// <param name="tempFolderPath">temp folder path.</param>
        /// <returns>true, if data is exported succesfully.</returns>
        public static bool ExportDatabase(UserSelection userSelection, int areaLevel, DIConnection sourceDBConnection, DIQueries sourceDBQueries, bool maintainTemplate, string destinationDBNameWPath, string tempFolderPath)
        {
            bool RetVal = false;

            if (sourceDBConnection != null)
            {
                switch (sourceDBConnection.ConnectionStringParameters.ServerType)
                {
                    case DIServerType.Excel:
                        break;
                    case DIServerType.MsAccess:
                        //-- Export database into Access (mdb).
                        Export.ExportDatabase ExportDB = new ExportDatabase(userSelection, sourceDBConnection);
                        ExportDB.AreaLevel = areaLevel;
                        RetVal = ExportDB.ExportMDB(maintainTemplate, destinationDBNameWPath, tempFolderPath);
                        break;
                    case DIServerType.Oracle:
                        break;
                    case DIServerType.SqlServer:
                    case DIServerType.MySql:
                    case DIServerType.SqlServerExpress:
                       
                        if (_CRINGExport == false)
                        {
                            Export.ExportOnlineDatabase ExportOnlineDB = new ExportOnlineDatabase(userSelection, sourceDBConnection, sourceDBQueries);
                            RetVal = ExportOnlineDB.ExportMDB(destinationDBNameWPath, tempFolderPath);
                        }
                        else
                        {
                            Export.ExportOnlineDatabaseCRING ExportOnlineDBCRING = new ExportOnlineDatabaseCRING(userSelection, sourceDBConnection, sourceDBQueries);
                            RetVal = ExportOnlineDBCRING.ExportMDB(destinationDBNameWPath, tempFolderPath, _DataNIds);
                        }
                        break;
                    default:
                        break;
                }
            }
            return RetVal;
        }

        /// <summary>
        /// Export database for Online and create new database file for Online connection
        /// </summary>
        /// <param name="userSelection"></param>
        /// <param name="areaLevel"></param>
        /// <param name="sourceDBConnection"></param>
        /// <param name="sourceDBQueries"></param>
        /// <param name="maintainTemplate"></param>
        /// <param name="destinationDBNameWPath"></param>
        /// <param name="tempFolderPath"></param>
        /// <param name="createNewDatabaseForOnline"></param>
        /// <returns></returns>
        public static bool ExportDatabase(UserSelection userSelection, int areaLevel, DIConnection sourceDBConnection, DIQueries sourceDBQueries, bool maintainTemplate, string destinationDBNameWPath, string tempFolderPath,bool createNewDatabaseForOnline)
        {
            bool RetVal = false;

            if (sourceDBConnection != null)
            {
                switch (sourceDBConnection.ConnectionStringParameters.ServerType)
                {
                    case DIServerType.Excel:
                        break;
                    case DIServerType.MsAccess:
                        //-- Export database into Access (mdb).
                        Export.ExportDatabase ExportDB = new ExportDatabase(userSelection, sourceDBConnection);
                        ExportDB.AreaLevel = areaLevel;
                        RetVal = ExportDB.ExportMDB(maintainTemplate, destinationDBNameWPath, tempFolderPath);
                        break;
                    case DIServerType.Oracle:
                        break;
                    case DIServerType.SqlServer:
                    case DIServerType.MySql:
                    case DIServerType.SqlServerExpress:                     

                      
                            Export.ExportOnlineDatabase ExportOnlineDB = new ExportOnlineDatabase(userSelection, sourceDBConnection, sourceDBQueries);
                            RetVal = ExportOnlineDB.ExportMDB(destinationDBNameWPath, tempFolderPath, createNewDatabaseForOnline);
                       
                        
                        break;
                    default:
                        break;
                }
            }
            return RetVal;
        }


        #endregion

        #region "-- PDF / HTML/ CSV / XML--"

        /// <summary>
        /// Exports Data in DataView to output format specified. Output Data format will be same as  DataView.
        /// </summary>
        /// <param name="sourceDataView">DataView having data to export</param>
        /// <param name="outputType">Output format desired.</param>
        /// <param name="outputFileNameWPath">output File Name with path.</param>
        /// <returns>True, if exported succesfully.</returns>
        public static bool ExportDataView(DataView sourceDataView, DIExportOutputType outputType, string outputFileNameWPath)
        {
            bool RetVal = false;
            switch (outputType)
            {
                case DIExportOutputType.Spreadsheet:
                    RetVal = ExportDES.ExportDataView(sourceDataView, outputFileNameWPath);
                    break;
                case DIExportOutputType.PDF:
                    RetVal = ExportPDF.ExportDataView(sourceDataView, string.Empty, string.Empty, null, null, outputFileNameWPath);
                    break;
                case DIExportOutputType.HTML:
                    RetVal = ExportHTML.ExportDataView(sourceDataView, outputFileNameWPath);
                    break;
                case DIExportOutputType.CSV:
                    RetVal = ExportCSV.ExportDataView(sourceDataView, outputFileNameWPath);
                    break;
                default:
                    break;
            }
            return RetVal;
        }

        /// <summary>
        /// It exports the dataView, passed as parameter, to various fomrat
        /// </summary>
        /// <param name="sourceDataView">source dataView containing rows and columns</param>
        /// <param name="columnHeaderFont">font for column headings</param>
        /// <param name="dataRowFont">Desired Font for data to be displayed.</param>
        /// <param name="outputType">output type like PDF, HTML, xls, csv</param>
        /// <param name="outputFileNameWPath">output filename with path</param>
        /// <returns>true, if exported succesfully</returns>
        public static bool ExportDataView(DataView sourceDataView, Font columnHeaderFont, Font dataRowFont, DIExportOutputType outputType, string outputFileNameWPath)
        {
            bool RetVal = false;
            // Add Color.Blank as default color.
            switch (outputType)
            {
                case DIExportOutputType.Spreadsheet:
                    RetVal = ExportDES.ExportDataView(sourceDataView, columnHeaderFont, dataRowFont, Color.Black, outputFileNameWPath);
                    break;
                case DIExportOutputType.PDF:
                    RetVal = ExportPDF.ExportDataView(sourceDataView, false, string.Empty, string.Empty, null, null, Color.Black, outputFileNameWPath);
                    break;
                case DIExportOutputType.HTML:
                    RetVal = ExportHTML.ExportDataView(sourceDataView, false, columnHeaderFont, dataRowFont, Color.Black, outputFileNameWPath);
                    break;
                case DIExportOutputType.CSV:
                    RetVal = ExportCSV.ExportDataView(sourceDataView, outputFileNameWPath);
                    break;

                default:
                    break;
            }
            return RetVal;
        }

        /// <summary>
        /// It exports the dataView, passed as parameter, to various fomrat
        /// </summary>
        /// <param name="sourceDataView">dataView as data source.</param>
        /// <param name="columnHeaderFont">font for column headers</param>
        /// <param name="dataRowFont">Desired Font for data to be displayed.</param>
        /// <param name="globalColor">global Color used for global columns (INdicator_global, Unit_global, subgroup_val_global)</param>
        /// <param name="outputType">type like PDF, HTML, xls, csv</param>
        /// <param name="outputFileNameWPath">Output filename with path.</param>
        /// <returns></returns>
        public static bool ExportDataView(DataView sourceDataView, Font columnHeaderFont, Font dataRowFont, Color globalColor, DIExportOutputType outputType, string outputFileNameWPath)
        {
            bool RetVal = false;
            switch (outputType)
            {
                case DIExportOutputType.Spreadsheet:
                    RetVal = ExportDES.ExportDataView(sourceDataView, columnHeaderFont, dataRowFont, globalColor, outputFileNameWPath);
                    break;
                case DIExportOutputType.PDF:
                    RetVal = ExportPDF.ExportDataView(sourceDataView, false, string.Empty, string.Empty, columnHeaderFont, dataRowFont, globalColor, outputFileNameWPath);
                    break;
                case DIExportOutputType.HTML:
                    RetVal = ExportHTML.ExportDataView(sourceDataView, false, columnHeaderFont, dataRowFont, globalColor, outputFileNameWPath);
                    break;
                case DIExportOutputType.CSV:
                    RetVal = ExportCSV.ExportDataView(sourceDataView, outputFileNameWPath);
                    break;
                default:
                    break;
            }
            return RetVal;
        }


        /// <summary>
        /// It exports the dataView, passed as parameter, to various fomrat
        /// </summary>
        /// <param name="sourceDataView">dataView as data source.</param>
        /// <param name="RTL">True, if RTL is required for Columns</param>
        /// <param name="columnHeaderFont">font for column headers</param>
        /// <param name="dataRowFont">Desired Font for data to be displayed.</param>
        /// <param name="globalColor">global Color used for global columns (INdicator_global, Unit_global, subgroup_val_global)</param>
        /// <param name="outputType">output format required (pdf, html, xls, csv)</param>
        /// <param name="outputFileNameWPath">Output filename with path.</param>
        /// <returns></returns>
        public static bool ExportDataView(DataView sourceDataView, bool RTL, Font columnHeaderFont, Font dataRowFont, Color globalColor, DIExportOutputType outputType, string outputFileNameWPath)
        {
            bool RetVal = false;
            switch (outputType)
            {
                case DIExportOutputType.Spreadsheet:
                    RetVal = ExportDES.ExportDataView(sourceDataView, columnHeaderFont, dataRowFont, globalColor, outputFileNameWPath);
                    break;
                case DIExportOutputType.PDF:
                    RetVal = ExportPDF.ExportDataView(sourceDataView, RTL, string.Empty, string.Empty, columnHeaderFont, dataRowFont, globalColor, outputFileNameWPath);
                    break;
                case DIExportOutputType.HTML:
                    RetVal = ExportHTML.ExportDataView(sourceDataView, RTL, columnHeaderFont, dataRowFont, globalColor, outputFileNameWPath);
                    break;
                case DIExportOutputType.CSV:
                    RetVal = ExportCSV.ExportDataView(sourceDataView, outputFileNameWPath);
                    break;
                default:
                    break;
            }
            return RetVal;
        }

        /// <summary>
        /// Generates customized PDF report of dataView specified, along with DevInfo Logo image(or any adaptation) & SourceDatabase title required in the document beginning.
        /// </summary>
        /// <param name="sourceDataView">source DataView.</param>
        /// <param name="globalColor">global color if _Global columns Values are true..</param>
        /// <param name="HeaderFont">Column Header's Font.(can be passed NULL)</param>
        /// <param name="RTL">bool value for RTL</param>
        /// <param name="TabularDataFont">Table row's data font.</param>
        /// <param name="DevInfoLogoPath">DevInfo log fileName with path. Logo appears on top of Document</param>
        /// <param name="sourceDatabaseName">Source Database File Name to be printed on top od PDF document.</param>
        /// <param name="outputFileNameWPath">output pdf FileName with path.</param>
        /// <returns></returns>
        public static bool ExportDataViewToPDF(DataView sourceDataView, bool RTL, string DevInfoLogoPath, string sourceDatabaseName, Font HeaderFont, Font TabularDataFont, Color globalColor, string outputFileNameWPath)
        {
            bool RetVal = false;
            try
            {
                RetVal = ExportPDF.ExportDataView(sourceDataView, RTL, DevInfoLogoPath, sourceDatabaseName, HeaderFont, TabularDataFont, globalColor, outputFileNameWPath);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        /// It exports the Indicator Classifications of specified ICType, present in DataView. <para>Exported ICs will be in format: ICLevel 1, ICLevel 2, ...ICLevel n , Indicator, Unit, Subgroup. </para>  
        /// </summary>
        /// <param name="ICElementType">ICType enum value </param>
        /// <param name="ICDataView">DataView having IC Data in format: ICLevel 1, ICLevel 2, ...ICLevel n , Indicator, Unit, Subgroup. </param>
        /// <param name="outputType">Output format type.</param>
        /// <param name="outputFileNameWPath">Output fileName With path.</param>
        /// <returns></returns>
        public static bool ExportIC(ICType ICElementType, DataView ICDataView, DIExportOutputType outputType, string outputFileNameWPath)
        {
            bool RetVal = false;

            try
            {
                switch (outputType)
                {
                    case DIExportOutputType.DES:
                    case DIExportOutputType.Spreadsheet:
                        RetVal = ExportDES.ExportICFromDataView(ICDataView, ICElementType, outputFileNameWPath);
                        break;
                    case DIExportOutputType.PDF:
                        RetVal = ExportPDF.ExportICFromDataView(ICDataView, ICElementType, outputFileNameWPath);
                        break;
                    case DIExportOutputType.HTML:
                        RetVal = ExportHTML.ExportICFromDataView(ICDataView, ICElementType, outputFileNameWPath);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        /// <summary>
        /// It exports the Indicator Classifications of specified ICType, present in Database. <para>Exported ICs will be in format: ICLevel 1, ICLevel 2, ...ICLevel n , Indicator, Unit, Subgroup. </para>  
        /// </summary>
        /// <param name="ICElementType">ICType enum value</param>
        /// <param name="dBConnection">DIConnection object of source Database,</param>
        /// <param name="dBQueries">DIQueries object of source Database.</param>
        /// <param name="exportOutputType">Output format type.</param>
        /// <param name="outputFileNameWPath">Output fileName With path.</param>
        /// <param name="languageFileNameWPath">language xml file name path.</param>
        /// <returns></returns>
        public static bool ExportIC(ICType ICElementType, DIConnection dBConnection, DIQueries dBQueries, DIExportOutputType exportOutputType, string outputFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;
            DataView ICDataView = null;

            try
            {
                // Get Distinct IndicatorClassification labels for N levels and Indicator, Unit, Subgroup in a DataView.
                ICDataView = DIExport.GetDataViewForICExport(ICElementType, dBConnection, dBQueries).DefaultView;

                // Export Indicator Entry Spreadsheet using DataView.
                RetVal = DIExport.ExportIC(ICElementType, ICDataView, exportOutputType, outputFileNameWPath);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        /// It exports all Indicators present in DataView parameter (IndicatorName, GID) into desired output format (DES, PDF, HTml, etc)
        /// </summary>
        /// <param name="indicatorDataView">DataView having Indicator_Name, Indicator_GId columns.</param>
        /// <param name="outputType">Output format type enum value.</param>
        /// <param name="outputFileNameWPath">Output fileName With path.</param>
        /// <returns>true, if export succesful.</returns>
        public static bool ExportIndicator(DataView indicatorDataView, DIExportOutputType outputType, string outputFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;

            try
            {
                switch (outputType)
                {
                    case DIExportOutputType.Spreadsheet:
                        RetVal = ExportDES.ExportDataView(indicatorDataView, outputFileNameWPath);
                        break;
                    case DIExportOutputType.DES:
                        RetVal = ExportDES.ExportIndicatorEntrySpreadsheet(indicatorDataView, outputFileNameWPath, languageFileNameWPath);
                        break;
                    case DIExportOutputType.PDF:
                        RetVal = ExportPDF.ExportIndicatorFromDataView(indicatorDataView, outputFileNameWPath, languageFileNameWPath);
                        break;
                    case DIExportOutputType.HTML:
                        RetVal = ExportHTML.ExportIndicatorFromDataView(indicatorDataView, outputFileNameWPath, languageFileNameWPath);
                        break;
                    case DIExportOutputType.CSV:
                        RetVal = ExportCSV.ExportDataView(indicatorDataView, outputFileNameWPath);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        /// <summary>
        ///  It exports all Indicators present in DataBase (IndicatorName, GID) into desired output format (DES, PDF, HTml, etc)
        /// </summary>
        /// <param name="dBConnection">DIConnection object</param>
        /// <param name="dBQueries">DIQueries object</param>
        /// <param name="exportOutputType">Output format type enum value.</param>
        /// <param name="outputFileNameWPath">Output fileName With path.</param>
        /// <param name="languageFileNameWPath">language fileName Path.</param>
        /// <returns></returns>
        public static bool ExportIndicator(DIConnection dBConnection, DIQueries dBQueries, DIExportOutputType exportOutputType, string outputFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;
            DataView IndicatorDataView = null;

            try
            {
                // Export Indicator Entry Spreadsheet using DataView.
                //RetVal = DIExport.ExportIndicator(IndicatorDataView, exportOutputType, outputFileNameWPath, languageFileNameWPath);
                RetVal = DIExport.ExportIndicator(dBConnection, dBQueries, string.Empty, false, exportOutputType, outputFileNameWPath, languageFileNameWPath);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        /// <summary>
        ///  It exports Indicators information(IndicatorName, GID) for given indicator NIds, into desired output format (DES, PDF, HTml, etc)
        /// </summary>
        /// <param name="dBConnection">DIConnection object</param>
        /// <param name="dBQueries">DIQueries object</param>
        /// <param name="indicatorNIds">comma delimited indicator NIds, for Filter</param>
        /// <param name="showIUS">true if IUS information need to be shown in output.</param>
        /// <param name="exportOutputType">Output format type enum value.</param>
        /// <param name="outputFileNameWPath">Output fileName With path.</param>
        /// <param name="languageFileNameWPath">language fileName Path.</param>
        /// <returns></returns>
        public static bool ExportIndicator(DIConnection dBConnection, DIQueries dBQueries, string NIdsFilterString, bool isIUSNIds, DIExportOutputType exportOutputType, string outputFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;
            DataTable IndicatorDT = null;

            try
            {
                // Get Distinct Indicator_Name and Indicator_Gid for given IndicatorNIds in a DataView 
                if (string.IsNullOrEmpty(NIdsFilterString))
                {
                    NIdsFilterString = string.Empty;
                }

                //-- Check if showIUS is true, then Get IUS gor given indicators,. 
                //-- else, get only indicatorsNames, IndicatorGID
                if (isIUSNIds)
                {
                    IndicatorDT = dBConnection.ExecuteDataTable(dBQueries.IUS.GetIUS(FilterFieldType.NId, NIdsFilterString, FieldSelection.Light));

                    //-- Get Only Names columns
                    IndicatorDT = IndicatorDT.DefaultView.ToTable(false, Indicator.IndicatorName, Unit.UnitName, SubgroupVals.SubgroupVal);
                }
                else
                {
                    IndicatorDT = dBConnection.ExecuteDataTable(dBQueries.Indicators.GetIndicator(FilterFieldType.NId, NIdsFilterString, FieldSelection.Light));

                    //-- Get Only Indicator Name column
                    IndicatorDT = IndicatorDT.DefaultView.ToTable(false, new string[] { Indicator.IndicatorName, Indicator.IndicatorGId });
                }

                //-- Language Handling of Columns
                if (IndicatorDT.Columns.Contains(Indicator.IndicatorName))
                {
                    IndicatorDT.Columns[Indicator.IndicatorName].Caption = DILanguage.GetLanguageString("INDICATOR");
                }
                if (IndicatorDT.Columns.Contains(Indicator.IndicatorGId))
                {
                    IndicatorDT.Columns[Indicator.IndicatorGId].Caption = DILanguage.GetLanguageString("GID");
                }
                if (IndicatorDT.Columns.Contains(Unit.UnitName))
                {
                    IndicatorDT.Columns[Unit.UnitName].Caption = DILanguage.GetLanguageString("UNIT");
                }
                if (IndicatorDT.Columns.Contains(SubgroupVals.SubgroupVal))
                {
                    IndicatorDT.Columns[SubgroupVals.SubgroupVal].Caption = DILanguage.GetLanguageString("SUBGROUP");
                }

                // Export Indicators using DataView.
                RetVal = DIExport.ExportIndicator(IndicatorDT.DefaultView, exportOutputType, outputFileNameWPath, languageFileNameWPath);
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        /// <summary>
        ///  It exports all Areas information (AreaID, AreaName, AreaLevel, AreaGId, ParentAreaId) provided in dataview into specified output format.
        /// </summary>
        /// <param name="areaDataView">dataView object having all required column.</param>
        /// <param name="outputType">output format type</param>
        /// <param name="outputFileNameWPath">output filename path.</param>
        /// <param name="languageFileNameWPath">language xml filename path. (required for xls Areas heading)</param>
        /// <returns></returns>
        public static bool ExportArea(DataView areaDataView, DIExportOutputType outputType, string outputFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;

            try
            {
                switch (outputType)
                {
                    case DIExportOutputType.Spreadsheet:
                        RetVal = ExportDES.ExportDataView(areaDataView, outputFileNameWPath);
                        break;
                    case DIExportOutputType.DES:
                        RetVal = ExportDES.ExportAreaEntrySpreadsheet(areaDataView, outputFileNameWPath, languageFileNameWPath);
                        break;
                    case DIExportOutputType.PDF:
                        RetVal = ExportPDF.ExportAreaFromDataView(areaDataView, outputFileNameWPath, languageFileNameWPath);
                        break;
                    case DIExportOutputType.HTML:
                        RetVal = ExportHTML.ExportAreaFromDataView(areaDataView, outputFileNameWPath, languageFileNameWPath);
                        break;
                    case DIExportOutputType.CSV:
                        RetVal = ExportCSV.ExportDataView(areaDataView, outputFileNameWPath);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        /// <summary>
        /// It exports all Areas information (AreaID, AreaName, AreaLevel, AreaGId, ParentAreaId) into specified output format.
        /// </summary>
        /// <param name="dBConnection">source database DIConnection object</param>
        /// <param name="dBQueries">source database DiQueries object.</param>
        /// <param name="exportOutputType">output format type(.pdf, html, xls, csv)</param>
        /// <param name="outputFileNameWPath">output filename path.</param>
        /// <param name="languageFileNameWPath">language xml filename path. (required for xls Areas heading)</param>
        /// <returns></returns>
        public static bool ExportArea(DIConnection dBConnection, DIQueries dBQueries, DIExportOutputType exportOutputType, string outputFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;

            //-- Passing empty string as AreaNIds in overloaded function.
            RetVal = DIExport.ExportArea(dBConnection, dBQueries, string.Empty, exportOutputType, outputFileNameWPath, languageFileNameWPath);

            return RetVal;
        }

        /// <summary>
        /// It Areas information (AreaID, AreaName, AreaLevel, AreaGId, ParentAreaId) for given Area NIds, into specified output format.
        /// </summary>
        /// <param name="dBConnection">source database DIConnection object</param>
        /// <param name="dBQueries">source database DiQueries object.</param>
        /// <param name="areaNIds">filter string Area NIds </param>
        /// <param name="exportOutputType">output format type(.pdf, html, xls, csv)</param>
        /// <param name="outputFileNameWPath">output filename path.</param>
        /// <param name="languageFileNameWPath">language xml filename path. (required for xls Areas heading)</param>
        /// <returns></returns>
        public static bool ExportArea(DIConnection dBConnection, DIQueries dBQueries, string areaNIds, DIExportOutputType exportOutputType, string outputFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;
            DataTable AreaDT = null;

            try
            {

                //-- SQL Query to get AreaName, AreaID, Level, ParentID
                // TODO move in DAL.
                string SqlString = "SELECT A1." + Area.AreaID + ", A1." + Area.AreaName + ", A1." + Area.AreaLevel + " , A1." + Area.AreaGId + ", A2." + Area.AreaID + " AS " + Constants.ParentAreaIDColumn + " " +
" FROM " + dBQueries.TablesName.Area + " AS A1 LEFT JOIN " + dBQueries.TablesName.Area + " AS A2 ON A1." + Area.AreaParentNId + " = A2." + Area.AreaNId;

                if (string.IsNullOrEmpty(areaNIds) == false)
                {
                    SqlString += " WHERE A1." + Area.AreaNId + " IN (" + areaNIds + ")";
                }
                SqlString += " Order by A1." + Area.AreaID + ", A1." + Area.AreaName + ", A1." + Area.AreaLevel + " ASC";

                //-- get Datatable
                AreaDT = dBConnection.ExecuteDataTable(SqlString);

                //-- If ExportOutput type is other than DES,
                //-- then include 3 columns (AreaName, AreaID, AreaLevel)
                if (exportOutputType != DIExportOutputType.DES)
                {
                    AreaDT = AreaDT.DefaultView.ToTable(false, Area.AreaName, Area.AreaID, Area.AreaLevel);
                }

                //-- Language Handling of Columns
                if (AreaDT.Columns.Contains(Area.AreaName))
                {
                    AreaDT.Columns[Area.AreaName].Caption = DILanguage.GetLanguageString("AREANAME");
                }
                if (AreaDT.Columns.Contains(Area.AreaID))
                {
                    AreaDT.Columns[Area.AreaID].Caption = DILanguage.GetLanguageString("AREAID");
                }
                if (AreaDT.Columns.Contains(Area.AreaLevel))
                {
                    AreaDT.Columns[Area.AreaLevel].Caption = DILanguage.GetLanguageString("AREA_LEVEL");
                }
                if (AreaDT.Columns.Contains(Constants.ParentAreaIDColumn))
                {
                    AreaDT.Columns[Constants.ParentAreaIDColumn].Caption = Constants.ParentAreaIDColumnCaption;
                }

                // Export Indicator Entry Spreadsheet using DataView.
                RetVal = DIExport.ExportArea(AreaDT.DefaultView, exportOutputType, outputFileNameWPath, languageFileNameWPath);

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        /// <summary>
        /// Export Timeperiods into specified output format.
        /// </summary>
        /// <param name="dBConnection">source database DIConnection object</param>
        /// <param name="dBQueries">source database DiQueries object.</param>
        /// <param name="timePeriodNIds">filter string timeperiod NIds</param>
        /// <param name="exportOutputType">output format type(.pdf, html, xls, csv)</param>
        /// <param name="outputFileNameWPath">output filename path.</param>
        /// <returns></returns>
        public static bool ExportTimePeriods(DIConnection dBConnection, DIQueries dBQueries, string timePeriodNIds, DIExportOutputType exportOutputType, string outputFileNameWPath)
        {
            bool RetVal = false;
            DataView TimePeriodView = null;
            try
            {
                // Get dataView for given timeperiod NIds
                TimePeriodView = dBConnection.ExecuteDataTable(dBQueries.Timeperiod.GetTimePeriod(timePeriodNIds)).DefaultView;

                //-- get dataView having one column "TimePeriod"
                TimePeriodView = TimePeriodView.ToTable(false, Timeperiods.TimePeriod).DefaultView;

                //-- Language Handling of Columns
                if (TimePeriodView.Table.Columns.Contains(Timeperiods.TimePeriod))
                {
                    TimePeriodView.Table.Columns[Timeperiods.TimePeriod].Caption = DILanguage.GetLanguageString("TIMEPERIOD");
                }

                //-- Exporting as general Dataview
                RetVal = DIExport.ExportDataView(TimePeriodView, exportOutputType, outputFileNameWPath);

                return RetVal;
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }


        #endregion

        #region "-- Other --"

        /// <summary>
        /// Adds Footnotes against FootNotesNID in each row in in the Dataview's DataTable passed.
        /// </summary>
        /// <param name="desDataTable">DataView in which footnotes to be added</param>
        /// <param name="dBConnection">DIconnection object</param>
        /// <param name="dBQueries">DIQueries oject</param>
        public static void AddFootNoteInDataTable(ref DataTable desDataTable, DIConnection dBConnection, DIQueries dBQueries)
        {
            string FootNoteNIds = string.Empty;
            DataTable FootNoteTable = new DataTable("FootNote");
            DataSet DSet = new DataSet();
            DataTable DESDataTable = desDataTable;

            DESDataTable.TableName = "DESDataTable";
            //DataColumn ParentColumn;
            //DataColumn ChildColumn;
            try
            {
                if (desDataTable != null && desDataTable.Rows.Count > 0)
                {
                    // Get Distinct FootNotesNIds from DataView.
                    FootNoteNIds = DIExport.DataColumnValuesToString(desDataTable, FootNotes.FootNoteNId);

                    // Get FootNotes from distinct FootnoteNIds in DataView.
                    FootNoteTable = dBConnection.ExecuteDataTable(dBQueries.Footnote.GetFootnote(FilterFieldType.NId, FootNoteNIds));

                    //// Join two DataViews (FootNote & desDataView)
                    //DSet.Tables.Add(DESDataTable);
                    //DSet.Tables.Add(FootNoteTable);
                    //ChildColumn = DESDataTable.Columns[FootNotes.FootNoteNId];
                    //ParentColumn = FootNoteTable.Columns[FootNotes.FootNoteNId];

                    //// Relations b/w two tables.
                    //DSet.Relations.Add("FootNoteMap", ParentColumn, ChildColumn);

                    if (!(DESDataTable.Columns.Contains(FootNotes.FootNote)))
                    {
                        DESDataTable.Columns.Add(FootNotes.FootNote);
                    }

                    // Start Adding Data
                    foreach (DataRow dr in DESDataTable.Rows)
                    {
                        if (dr[FootNotes.FootNoteNId].ToString() != "-1")
                        {
                            //dr[FootNotes.FootNote] = dr.GetChildRows(FootNoteRelation)[0][FootNotes.FootNote];
                            dr[FootNotes.FootNote] = FootNoteTable.Select(FootNotes.FootNoteNId + " = " + dr[FootNotes.FootNoteNId])[0][FootNotes.FootNote].ToString();
                        }

                    }

                    DESDataTable.AcceptChanges();
                }
            }
            catch (Exception)
            {
            }

        }

        /// <summary>
        /// It adds Sector & Class columns and fills values agiainst IUSNId in each rows.
        /// </summary>
        /// <param name="desDataTable">(By REF)dataTable where columns are to be added. </param>
        /// <param name="dBConnection">DIConnection object</param>
        /// <param name="dBQueries">DIQueries object</param>
        public static void AddSectorClassInDataTable(ref DataTable desDataTable, DIConnection dBConnection, DIQueries dBQueries)
        {
            string DistinctIUSNIds;

            DataTable SectorTable = new DataTable();
            DataTable ClassTable = new DataTable();
            string SectorNIds;


            try
            {
                //Get distinct IUS from DataTable
                DistinctIUSNIds = DIExport.DataColumnValuesToString(desDataTable, Indicator_Unit_Subgroup.IUSNId);

                // Get all ICs (Sector) for above IUS
                SectorTable = dBConnection.ExecuteDataTable(dBQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.ParentNId, "-1", DistinctIUSNIds, ICType.Sector, FieldSelection.Light));

                // Get Class for Sector NIDs
                if (SectorTable.Rows.Count > 0)
                {
                    //SectorName = SectorTable.Rows[0][IndicatorClassifications.ICName].ToString();
                    SectorNIds = DIExport.DataColumnValuesToString(SectorTable, IndicatorClassifications.ICNId);

                    // Get ICName from Database as Class (First child for given SectorNID and IUSNIds)

                    ClassTable = dBConnection.ExecuteDataTable(dBQueries.IndicatorClassification.GetICForIUSNId(FilterFieldType.ParentNId, SectorNIds, DistinctIUSNIds, ICType.Sector, FieldSelection.Light));
                }

                // Add required columns
                if (desDataTable.Columns.Contains(Constants.SectorColumnName) == false)
                {
                    desDataTable.Columns.Add(Constants.SectorColumnName);
                }
                if (desDataTable.Columns.Contains(Constants.ClassColumnName) == false)
                {
                    desDataTable.Columns.Add(Constants.ClassColumnName);
                }

                // Fill Sector & Class in Main DataTable
                // loop each IUS, add sector, class in it.
                foreach (DataRow dr in desDataTable.Rows)
                {
                    //  Add sector
                    DataRow[] Drows = SectorTable.Select(Indicator_Unit_Subgroup.IUSNId + " = " + dr[Indicator_Unit_Subgroup.IUSNId].ToString());
                    if (Drows.Length > 0)
                    {
                        dr[Constants.SectorColumnName] = Drows[0][IndicatorClassifications.ICName];
                    }

                    //  Add Class
                    Drows = ClassTable.Select(Indicator_Unit_Subgroup.IUSNId + " = " + dr[Indicator_Unit_Subgroup.IUSNId].ToString());
                    if (Drows.Length > 0)
                    {
                        dr[Constants.ClassColumnName] = Drows[0][IndicatorClassifications.ICName];
                    }
                }

                desDataTable.AcceptChanges();
            }
            catch (Exception)
            {

            }

        }


        # endregion

        #region "-- Where Data Exists / Blank DES / DES for Missing records --"

        /// <summary>
        /// Generates DES  for missing data ( blank DES + DES where data exists )
        /// </summary>
        /// <param name="singleWorkbook"></param>
        /// <param name="dBConnection"></param>
        /// <param name="dBQueries"></param>
        /// <param name="userSelection"></param>
        /// <param name="sortedFields"></param>
        /// <param name="xlsFileNameWPath"></param>
        /// <param name="includeGUID"></param>
        /// <param name="languageFileNameWPath"></param>
        /// <returns></returns>
        public bool ExportMissingDataDES(bool singleWorkbook, DIConnection dBConnection, DIQueries dBQueries, UserSelection userSelection, Fields sortedFields, string xlsFileNameWPath, bool includeGUID, string languageFileNameWPath)
        {
            bool RetVal = false;
            IWorkbook DESWorkbook = null;

            DataTable DistinctIU;
            String SqlQuery = string.Empty;
            DataView FilteredDataView = null;
            DataView BlankDESDataView = null;
            DataTable BlankRecordsTable = null;
            DataTable WhereDataExistsTable = null;
            DataColumn[] PrimayKeyColumns;
            string RowFilter = string.Empty;
            int IUCounter = 0;
            int ProgressBarValue = 0;
            string IndicatorNId = string.Empty;
            string UnitNId = string.Empty;

            try
            {

                //1. Get Distinct IU
                DistinctIU = DIExport.GetDistinctIU(true, dBConnection, dBQueries, userSelection);

                // Initialize progress bar
                this.RaiseProgressBarInitialize(DistinctIU.Rows.Count + 1);



                // 3. Iterate through each Row having distinct Indicator + Unit.
                foreach (DataRow dr in DistinctIU.Rows)
                {
                    IndicatorNId = dr[Indicator.IndicatorNId].ToString();
                    UnitNId = dr[Unit.UnitNId].ToString();


                    // 2.1 get records where data exists
                    WhereDataExistsTable = this.GetDESDataViewWhereDataExits(includeGUID, userSelection, dBConnection, dBQueries, languageFileNameWPath, IndicatorNId, UnitNId).Table;

                    // 2.2 get blank records for all given combination
                    // get blank DES data view
                    BlankRecordsTable = GetBlankDESDataView(BlankDESDataView, IndicatorNId, UnitNId, singleWorkbook, dBConnection, dBQueries, userSelection, includeGUID).Table;

                    // 2.3. merge WhereDataExistsTable and BlankRecordsTable

                    // delete denominator column from BlankRecordsTable 
                    if (BlankRecordsTable.Columns.Contains(Data.DataDenominator))
                    {
                        BlankRecordsTable.Columns.Remove(Data.DataDenominator);
                    }

                    // merge missing records & records where data exists
                    if (WhereDataExistsTable != null)
                    {

                        // set primary key
                        if (WhereDataExistsTable.Rows.Count > 0)
                        {
                            PrimayKeyColumns = this.GetPrimaryKeyColumnForMissingDataDES(WhereDataExistsTable, userSelection);

                            WhereDataExistsTable.PrimaryKey = PrimayKeyColumns;

                        }
                    }

                    if (BlankRecordsTable != null)
                    {
                        // set primary key
                        if (BlankRecordsTable.Rows.Count > 0)
                        {
                            PrimayKeyColumns = this.GetPrimaryKeyColumnForMissingDataDES(BlankRecordsTable, userSelection);

                            BlankRecordsTable.PrimaryKey = PrimayKeyColumns;

                        }
                    }

                    try
                    {
                        if (userSelection.TimePeriodNIds.Length == 0 || userSelection.TimePeriodNIds.Length == 0)
                        {
                            foreach (DataRow BlankDESRow in BlankRecordsTable.Rows)
                            {
                                string SelectionClause = string.Empty;
                                SelectionClause = Area.AreaID + " ='" + DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[Area.AreaID])) + "' AND "
           + Indicator.IndicatorName + " ='" + DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[Indicator.IndicatorName])) + "' AND "
           + Unit.UnitName + " ='" + DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[Unit.UnitName])) + "' AND "
           + SubgroupVals.SubgroupVal + " ='" + DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[SubgroupVals.SubgroupVal])) + "'";

                                if (userSelection.TimePeriodNIds.Length > 0)
                                {
                                    SelectionClause += " AND " + Timeperiods.TimePeriod + " ='" + DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[Timeperiods.TimePeriod])) + "'";
                                }

                                if (userSelection.SourceNIds.Length > 0)
                                {
                                    SelectionClause += " AND " + IndicatorClassifications.ICName + " ='" + DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[IndicatorClassifications.ICName])) + "'";
                                }

                                if (WhereDataExistsTable.Select(SelectionClause).Length == 0)
                                {
                                    DataRow NewRow = WhereDataExistsTable.NewRow();
                                    // add timeperiod, Area_ID,Area_name,IC_name,Indicator_name,indicator_gid, Unit_Name,unit_gid, Sugbroup_Val, Subgroup_val_gid

                                    // Timeperiod
                                    NewRow[Timeperiods.TimePeriod] = DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[Timeperiods.TimePeriod]));

                                    // source
                                    NewRow[IndicatorClassifications.ICName] = DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[IndicatorClassifications.ICName]));


                                    // Area
                                    NewRow[Area.AreaID] = DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[Area.AreaID]));
                                    NewRow[Area.AreaName] = DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[Area.AreaName]));

                                    //Indicator
                                    NewRow[Indicator.IndicatorName] = DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[Indicator.IndicatorName]));
                                    if (BlankRecordsTable.Columns.Contains(Indicator.IndicatorGId))
                                    {
                                        NewRow[Indicator.IndicatorGId] = DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[Indicator.IndicatorGId]));
                                    }

                                    //Unit
                                    NewRow[Unit.UnitName] = DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[Unit.UnitName]));
                                    if (BlankRecordsTable.Columns.Contains(Unit.UnitGId))
                                    {
                                        NewRow[Unit.UnitGId] = DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[Unit.UnitGId]));
                                    }

                                    // subgroup_val
                                    NewRow[SubgroupVals.SubgroupVal] = DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[SubgroupVals.SubgroupVal]));
                                    if (BlankRecordsTable.Columns.Contains(SubgroupVals.SubgroupValGId))
                                    {
                                        NewRow[SubgroupVals.SubgroupValGId] = DICommon.RemoveQuotes(Convert.ToString(BlankDESRow[SubgroupVals.SubgroupValGId]));
                                    }

                                    WhereDataExistsTable.Rows.Add(NewRow);
                                    WhereDataExistsTable.AcceptChanges();
                                }
                            }
                        }

                        else
                        {
                            WhereDataExistsTable.Merge(BlankRecordsTable, true, MissingSchemaAction.AddWithKey);
                        }
                    }
                    catch (Exception ex)
                    {

                    }



                    // add missing columns
                    if (WhereDataExistsTable.Columns.Contains(Data.DataDenominator) == false)
                    {
                        WhereDataExistsTable.Columns.Add(Data.DataDenominator);
                    }

                    FilteredDataView = WhereDataExistsTable.DefaultView;


                    //////// 2.4 Limit DataRows to 65,536 as Excels sheet has 65,536 rows limit.
                    //////if (FilteredDataView.Table.Rows.Count > 65500)
                    //////{                       
                    //////    for (int i = FilteredDataView.Table.Rows.Count - 1; i > 65500; i--)
                    //////    {
                    //////        FilteredDataView.Table.Rows[i].Delete();
                    //////    }
                    //////}
                    //////FilteredDataView.Table.AcceptChanges();

                    // 2.5 update filteredDataView 
                    try
                    {
                        // Rename GID columns if not required.
                        if (!(includeGUID))
                        {
                            FilteredDataView.Table.Columns[Indicator.IndicatorGId].ColumnName = "I_GID";
                            FilteredDataView.Table.Columns[Unit.UnitGId].ColumnName = "U_GID";
                            if (FilteredDataView.Table.Columns.Contains(SubgroupVals.SubgroupValGId))
                            {
                                FilteredDataView.Table.Columns[SubgroupVals.SubgroupValGId].ColumnName = "S_GID";
                            }
                        }

                        //// Update Sector , Class in DataView
                        //DIExport.AddSectorClassInDataView(ref FilteredDataView, dBConnection, dBQueries, IndicatorNId, UnitNId);

                        FilteredDataView.Table.AcceptChanges();

                    }
                    catch (Exception)
                    {
                    }

                    ProgressBarValue++;
                    this.RaiseProgressBarIncrement(ProgressBarValue);   //raise Progressbar_Increment event

                    if (FilteredDataView.Count > 0)
                    {
                        //// Set Indicator, Unit Name for progress bar event, and raise Progress_Increment event
                        //this.RaiseProcessIndicatorUnitInfo(FilteredDataView.Table.Rows[0][Indicator.IndicatorName].ToString(), FilteredDataView.Table.Rows[0][Unit.UnitName].ToString(), FilteredDataView.Table.Rows.Count);

                        // Set sort order on FilteredDataView.
                        FilteredDataView.Sort = this.GetSortString(sortedFields);

                        IUCounter++;
                        try
                        {
                            // Process Workbook by filling worksheet with DataView.
                            this.ProcessWorkbook(singleWorkbook, ref DESWorkbook, FilteredDataView, IUCounter, xlsFileNameWPath, languageFileNameWPath, dBConnection, dBQueries, IndicatorNId, UnitNId);
                        }
                        catch (Exception)
                        {
                        }
                    }

                }

                //if (singleWorkbook)
                //{
                if (!(DESWorkbook == null))
                {
                    // Delete last worksheet which is left blank.
                    DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].Delete();

                    // Set first sheet as active sheet.
                    DESWorkbook.Worksheets[0].Select();

                    // Save Workbook
                    DESWorkbook.Save();
                }
                //}
                RetVal = true;
            }

            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (!(DESWorkbook == null))
                {
                    DESWorkbook.Save();
                }
                // Close progress bar
                this.RaiseProcessIndicatorUnitInfo(string.Empty, string.Empty, 0);

                this.RaiseProgressBarClose();
            }
            return RetVal;
        }

        /// <summary>
        /// Generates DES where data exists
        /// </summary>
        /// <param name="singleWorkbook"></param>
        /// <param name="dBConnection"></param>
        /// <param name="dBQueries"></param>
        /// <param name="userSelection"></param>
        /// <param name="sortedFields"></param>
        /// <param name="xlsFileNameWPath"></param>
        /// <param name="includeGUID"></param>
        /// <param name="languageFileNameWPath"></param>
        /// <returns></returns>
        public bool ExportDESWhereDataExists(bool singleWorkbook, DIConnection dBConnection, DIQueries dBQueries, UserSelection userSelection, Fields sortedFields, string xlsFileNameWPath, bool includeGUID, string languageFileNameWPath)
        {
            bool RetVal = false;
            IWorkbook DESWorkbook = null;

            DataTable DistinctIU;
            String SqlQuery = string.Empty;
            DataView FilteredDataView = null;
            DataTable MissingRecordTable;
            DataTable WhereDataExistsTable;
            string RowFilter = string.Empty;
            int IUCounter = 0;
            int ProgressBarValue = 0;
            string IndicatorNId = string.Empty;
            string UnitNId = string.Empty;

            try
            {

                //1. Get Distinct IU
                DistinctIU = DIExport.GetDistinctIU(false, dBConnection, dBQueries, userSelection);

                // Initialize progress bar
                this.RaiseProgressBarInitialize(DistinctIU.Rows.Count + 1);

                // 3. Iterate through each Row having distinct Indicator + Unit.
                foreach (DataRow dr in DistinctIU.Rows)
                {
                    IndicatorNId = dr[Indicator.IndicatorNId].ToString();
                    UnitNId = dr[Unit.UnitNId].ToString();

                    // get records where data exists
                    FilteredDataView = this.GetDESDataViewWhereDataExits(includeGUID, userSelection, dBConnection, dBQueries, languageFileNameWPath, IndicatorNId, UnitNId);

                    // update filteredDataView 
                    try
                    {
                        // Rename GID columns if not required.
                        if (!(includeGUID))
                        {
                            FilteredDataView.Table.Columns[Indicator.IndicatorGId].ColumnName = "I_GID";
                            FilteredDataView.Table.Columns[Unit.UnitGId].ColumnName = "U_GID";
                            if (FilteredDataView.Table.Columns.Contains(SubgroupVals.SubgroupValGId))
                            {
                                FilteredDataView.Table.Columns[SubgroupVals.SubgroupValGId].ColumnName = "S_GID";
                            }
                        }

                        //// Update Sector , Class in DataView
                        //DIExport.AddSectorClassInDataView(ref FilteredDataView, dBConnection, dBQueries, IndicatorNId, UnitNId);

                        FilteredDataView.Table.AcceptChanges();

                    }
                    catch (Exception)
                    {
                    }


                    ProgressBarValue++;
                    this.RaiseProgressBarIncrement(ProgressBarValue);   //raise Progressbar_Increment event

                    if (FilteredDataView.Count > 0)
                    {
                        //// Set Indicator, Unit Name for progress bar event, and raise Progress_Increment event
                        //this.RaiseProcessIndicatorUnitInfo(FilteredDataView.Table.Rows[0][Indicator.IndicatorName].ToString(), FilteredDataView.Table.Rows[0][Unit.UnitName].ToString(), FilteredDataView.Table.Rows.Count);

                        // Set sort order on FilteredDataView.
                        FilteredDataView.Sort = this.GetSortString(sortedFields);

                        IUCounter++;
                        try
                        {
                            // Process Workbook by filling worksheet with DataView.
                            this.ProcessWorkbook(singleWorkbook, ref DESWorkbook, FilteredDataView, IUCounter, xlsFileNameWPath, languageFileNameWPath, dBConnection, dBQueries, IndicatorNId, UnitNId);
                        }
                        catch (Exception)
                        {
                        }
                    }

                }

                if (singleWorkbook)
                {
                    if (!(DESWorkbook == null))
                    {
                        // Delete last worksheet which is left blank.
                        DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].Delete();

                        // Set first sheet as active sheet.
                        DESWorkbook.Worksheets[0].Select();

                        // Save Workbook
                        DESWorkbook.Save();
                    }
                }

                RetVal = true;
            }

            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (!(DESWorkbook == null))
                {
                    DESWorkbook.Save();
                }
                // Close progress bar
                this.RaiseProcessIndicatorUnitInfo(string.Empty, string.Empty, 0);

                this.RaiseProgressBarClose();
            }
            return RetVal;
        }

        /// <summary>
        /// Generates blank DES 
        /// </summary>
        /// <param name="singleWorkbook"></param>
        /// <param name="dBConnection"></param>
        /// <param name="dBQueries"></param>
        /// <param name="userSelection"></param>
        /// <param name="sortedFields"></param>
        /// <param name="xlsFileNameWPath"></param>
        /// <param name="includeGUID"></param>
        /// <param name="languageFileNameWPath"></param>
        /// <returns></returns>
        public bool ExportBlankDES(bool singleWorkbook, DIConnection dBConnection, DIQueries dBQueries, UserSelection userSelection, Fields sortedFields, string xlsFileNameWPath, bool includeGUID, string languageFileNameWPath)
        {
            bool RetVal = false;
            IWorkbook DESWorkbook = null;

            DataTable DistinctIU;
            String SqlQuery = string.Empty;
            DataView FilteredDataView = null;
            DataView BlankDESDataView = null;
            DataTable WhereDataExistsTable;
            string RowFilter = string.Empty;
            int IUCounter = 0;
            int ProgressBarValue = 0;

            string OriginalSubgroupNIds = string.Empty;
            DataTable AutoSubgroupVals = null;

            string IndicatorNId = string.Empty;
            string UnitNId = string.Empty;

            try
            {

                //1. Get Distinct IU
                DistinctIU = DIExport.GetDistinctIU(true, dBConnection, dBQueries, userSelection);

                // Initialize progress bar
                this.RaiseProgressBarInitialize(DistinctIU.Rows.Count + 1);

                // 2. Iterate through each Row having distinct Indicator + Unit.
                foreach (DataRow dr in DistinctIU.Rows)
                {

                    try
                    {
                        IndicatorNId = dr[Indicator.IndicatorNId].ToString();
                        UnitNId = dr[Unit.UnitNId].ToString();

                        // get blank DES data view
                        FilteredDataView = GetBlankDESDataView(BlankDESDataView, IndicatorNId, UnitNId, singleWorkbook, dBConnection, dBQueries, userSelection, includeGUID);


                        // Update Indicator Unit in DataView
                        DIExport.UpdateIndicatorUnitInDataView(ref FilteredDataView, dBConnection, dBQueries, IndicatorNId, UnitNId);

                        // Set original SubgroupNIDs back into userSelection
                        userSelection.SubgroupValNIds = OriginalSubgroupNIds;

                        // Rename GID columns if not required.
                        if (!(includeGUID))
                        {
                            FilteredDataView.Table.Columns[Indicator.IndicatorGId].ColumnName = "I_GID";
                            FilteredDataView.Table.Columns[Unit.UnitGId].ColumnName = "U_GID";
                            if (FilteredDataView.Table.Columns.Contains(SubgroupVals.SubgroupValGId))
                            {
                                FilteredDataView.Table.Columns[SubgroupVals.SubgroupValGId].ColumnName = "S_GID";
                            }
                        }

                        //// Update Sector , Class in DataView
                        //DIExport.AddSectorClassInDataView(ref FilteredDataView, dBConnection, dBQueries, IndicatorNId, UnitNId);

                        FilteredDataView.Table.AcceptChanges();

                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        if (FilteredDataView == null)
                        {
                            FilteredDataView = (new DataTable()).DefaultView;
                        }
                    }

                    //   >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>

                    ProgressBarValue++;
                    this.RaiseProgressBarIncrement(ProgressBarValue);   //raise Progressbar_Increment event

                    if (FilteredDataView.Count > 0)
                    {
                        //// Set Indicator, Unit Name for progress bar event, and raise Progress_Increment event
                        //this.RaiseProcessIndicatorUnitInfo(FilteredDataView.Table.Rows[0][Indicator.IndicatorName].ToString(), FilteredDataView.Table.Rows[0][Unit.UnitName].ToString(), FilteredDataView.Table.Rows.Count);

                        // Set sort order on FilteredDataView.
                        FilteredDataView.Sort = this.GetSortString(sortedFields);

                        IUCounter++;
                        try
                        {
                            // Process Workbook by filling worksheet with DataView.
                            this.ProcessWorkbook(singleWorkbook, ref DESWorkbook, FilteredDataView, IUCounter, xlsFileNameWPath, languageFileNameWPath, dBConnection, dBQueries, IndicatorNId, UnitNId);
                        }
                        catch (Exception)
                        {
                        }
                    }

                }

                //if (singleWorkbook)
                //{
                if (!(DESWorkbook == null))
                {
                    // Delete last worksheet which is left blank.
                    DESWorkbook.Worksheets[DESWorkbook.Worksheets.Count - 1].Delete();

                    // Set first sheet as active sheet.
                    DESWorkbook.Worksheets[0].Select();

                    // Save Workbook
                    DESWorkbook.Save();
                }
                //}

                RetVal = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (!(DESWorkbook == null))
                {
                    DESWorkbook.Save();
                }
                // Close progress bar
                this.RaiseProcessIndicatorUnitInfo(string.Empty, string.Empty, 0);

                this.RaiseProgressBarClose();
            }
            return RetVal;
        }

        # endregion

        # endregion

        #region "-- Events --"

        /// <summary>
        /// Fires when processing indicator
        /// </summary>
        public event CurrentIndicatorUnitInfo Process_IndicatorUnitInfo;

        /// <summary>
        /// Fires when any specific IndicatorUnit export is over.
        /// </summary>
        public event IndicatorUnitProcessed End_IndicatorUnit;

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


        #endregion

        #region "-- Export Events --"

        /// <summary>
        /// Fires when processing indicator For Export
        /// </summary>
        public static event CurrentIndicatorUnitInfo Process_IndicatorUnitInfo_Event;

        /// <summary>
        /// Fires when any specific IndicatorUnit export is over.For Export
        /// </summary>
        public static event IndicatorUnitProcessed End_IndicatorUnit_Event;

        /// <summary>
        /// Fires when value of prgressbar is changed For Export
        /// </summary>
        public static event IncrementProgressBar ProgressBar_Increment_Event;

        /// <summary>
        /// Fires when process started to initialize progress bar For Export
        /// </summary>
        public static event InitializeProgressBar ProgressBar_Initialize_Event;

        /// <summary>
        /// Fireds when process stop For Export
        /// </summary>
        public static event CloseProgressBar ProgressBar_Close_Event;


        #endregion


        #endregion

    }
}
