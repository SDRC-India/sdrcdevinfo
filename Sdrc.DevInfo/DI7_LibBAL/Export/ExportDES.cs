using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using SpreadsheetGear;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using System.Resources;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.Import.DAImport.DES;
using DevInfo.Lib.DI_LibBAL.Utility;
using System.Reflection;
using System.Drawing;
using Microsoft.VisualBasic;

namespace DevInfo.Lib.DI_LibBAL.Export
{
    internal class ExportDES
    {

        # region "-- New / Dispose --"



        # endregion

        # region "-- Variables/Properties --"


	
        # endregion

        #region "-- Private --"
        /// <summary>
        /// It extracts blank DES workbook from resource file and saves it as specified file name.
        /// </summary>
        /// <param name="fileNameWpath">Desired file name of workbook.</param>
        internal static void GetWorkbookFromResourceFile(WorkbookType workbookType, string fileNameWpath)
        {
            byte[] workbookBytes = null;
            try
            {
                if (File.Exists(fileNameWpath))
                {
                    File.Delete(fileNameWpath);
                }
                //Load the resource file

                ResourceManager rm = new ResourceManager("DevInfo.Lib.DI_LibBAL.Export.ExportR", Assembly.GetExecutingAssembly());
                switch (workbookType)
                {
                    case WorkbookType.DataEntrySpreadsheet:
                        workbookBytes = (byte[])rm.GetObject("DataEntrySpreadsheet");
                        break;
                    case WorkbookType.IndicatorEntrySpreadsheet:
                        workbookBytes = (byte[])rm.GetObject("Indicator");
                        break;
                    case WorkbookType.AreaEntrySpreadsheet:
                       workbookBytes = (byte[])rm.GetObject("Area");
                        break;
                    case WorkbookType.General:
                        workbookBytes = (byte[])rm.GetObject("General");
                        break;
                }

                FileStream fileStream = File.Create(fileNameWpath);
                fileStream.Write(workbookBytes, 0, workbookBytes.Length);
                fileStream.Close();

            }
            catch (Exception ex)
            {
                //TODO: exception message?
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        #endregion

        # region "-- internal --"

        /// <summary>
        /// It fills the worksheet with Data in DataView(passed as parameter ) as it is.
        /// <para> Data is pasted in worksheet at cell - A1</para>
        /// </summary>
        /// <param name="worksheet">Worksheet object to fill.</param>
        /// <param name="dataView">Dataview object as DataSource.</param>
        internal static void ExportData(IWorksheet worksheet, DataView dataView, Font columnHeaderFont, Font dataRowFont, Color globalColor)
        {
            //TODO handling for Font (header / data), color
            Font TableFont ;
            Font HeaderFont;
            bool ApplyGlobalColor = false;              // whether global color is to be applied on cell or not.

            int TimePeriodColumnIndex = -1;
            int DataColumnIndex = -1;

            // -- Get dataTable
            DataTable DT = dataView.ToTable();
            string[] ColActualName = new string[DT.Columns.Count];
            string[] ColHeaderName = new string[DT.Columns.Count];

            // Cell address
            int RowOffset = 0;
            int ColOffset = 0;
            IRange Range;
            IFont RangeFont;
            try
            {
                // Set Fonts objects
                if (dataRowFont == null)
                {
                    TableFont = new Font("Arial", 9F, FontStyle.Regular);
                }
                else
                {
                    TableFont = dataRowFont;
                }
                // Column Headers font
                if (columnHeaderFont == null)
                {
                    HeaderFont = new Font("Arial", 9F, FontStyle.Regular);
                }
                else
                {
                    HeaderFont = columnHeaderFont;
                }

                // Get Actual Columns and Displayed Columns
                for (int i = 0; i <= DT.Columns.Count - 1; i++)
                {
                    if (DIExport.CheckColumnRelevence(DT.Columns[i].ToString()))
                    {
                        ColActualName[i] = DT.Columns[i].ToString();
                        ColHeaderName[i] = DT.Columns[i].Caption;
                    }
                    else
                    {
                        ColActualName[i] = string.Empty;
                        ColHeaderName[i] = string.Empty;
                    }

                    ////-- Get TimePeriod, Data Column index position.
                    //if (DT.Columns[Timeperiods.TimePeriod]., true) != -1)
                    //{
                    //    TimePeriodColumnIndex = i;
                    //}

                    //if (string.Compare(DT.Columns[i].ToString(), Data.DataValue, true) != -1)
                    //{
                    //    DataColumnIndex = i;
                    //}
                }

                // -- Header
                RowOffset = 0;  //First Row for columns
                for (int cc = 0; cc <= ColHeaderName.Length - 1; cc++)
                {   
                    if (ColHeaderName[cc].Trim().Length > 0)
                    {  
                        ColOffset = cc;
                        worksheet.Cells[RowOffset, ColOffset].Value = ColHeaderName[cc].ToString();
                    }

                }

                // Set Column's row font
                worksheet.Cells[0, 0, 0, ColOffset].Font.Size = HeaderFont.Size;
                worksheet.Cells[0, 0, 0, ColOffset].Font.Name = HeaderFont.Name;
                worksheet.Cells[0, 0, 0, ColOffset].Interior.Color = Color.Gray;
                //worksheet.Cells[0, 0, 0, ColOffset].Font.Bold = true;
                worksheet.Cells[0, 0, 0, ColOffset].Font.Color = Color.White;

                //////Change 'TimePeriod' Column format to Text
                ////worksheet.Range[0, TimePeriodColumnIndex, DT.Rows.Count + 2, TimePeriodColumnIndex].NumberFormat = "@";

                // Add table data
                RowOffset = 1;      // Start adding data from row = 2 
                foreach (DataRow dr in DT.Rows)
                {
                    for (int cc = 0; cc <= ColActualName.Length - 1; cc++)
                    {
                        if (ColActualName[cc].Trim().Length > 0)
                        {
                            ColOffset = cc;
                            Range = worksheet.Cells[RowOffset, ColOffset];

                            //-- Change Number format as TEXT for TimePerid or DataValue
                            if ((ColActualName[cc] == Timeperiods.TimePeriod) || (ColActualName[cc] == Data.DataValue))
                            {
                                //Change 'TimePeriod' Column format to Text
                                worksheet.Range[0, cc, DT.Rows.Count + 2, cc].NumberFormat = "@";

                            }

                            if (Information.IsDBNull(dr[ColActualName[cc]])) // _DT.Rows[cc][ColActualName[jj]]))
                            {
                                Range.Value = "";
                            }
                            else
                            {
                                Range.Value = dr[ColActualName[cc]].ToString();
                                if (globalColor != Color.Black)
                                {
                                    // Check if column have equivalent _Global column exists in DatTable
                                    ApplyGlobalColor = DIExport.CheckGlobalColumnValue(DT, dr, ColActualName[cc]);
                                    if (ApplyGlobalColor)
                                    {
                                        Range.Font.Color = globalColor;
                                    }
                                }
                                
                            }
                        }
                    }
                    RowOffset++;
                }

                // Set Font for whole data Range
                RangeFont = worksheet.Cells[1, 1, worksheet.UsedRange.RowCount, worksheet.UsedRange.ColumnCount].Font;
                RangeFont.Size = TableFont.Size;
                RangeFont.Name = TableFont.Name;
                //RangeFont.Italic = dataRowFont.Italic;

                worksheet.Cells.Columns.AutoFit();
            }
            catch (Exception ex)
            {

                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }           
        }

        /// <summary>
        /// It converts worksheet into a Data Entry spreadsheet, filling Indicator, Unit and other data provided in DataView.
        /// </summary>
        /// <param name="worksheet">Worksheet object to be filled with data.</param>
        /// <param name="dataView">Dataview as Datasource having all neccesory column to make DES.</param>
        /// <returns></returns>
        internal static void ExportDESData(IWorksheet worksheet, DataView IUdataView, string languageFileNameWPath)
        {
            IRange Range;
            string[] DEScolumns = new string[9];
            // logic for generating single DevInfo DataEntryspreadsheet from a DataView.
            // use blank spreadsheet from resource file.
            try
            {
                // 1. Set Indicator Name and GId at respective cells.
                if (IUdataView.Table.Select(Indicator.IndicatorName + " <> '' ").Length > 0)
                {
                    // As IndicatorName might be present in only one row (Not all rows), so extract that row.
                    worksheet.Cells[DESCellAddress.IndicatorName].Value = IUdataView.Table.Select(Indicator.IndicatorName + " <> '' ")[0][Indicator.IndicatorName].ToString();
                }

                if (IUdataView.Table.Columns.Contains(Indicator.IndicatorGId))
                {
                    if (IUdataView.Table.Select(Indicator.IndicatorGId + " <> '' ").Length > 0)
                    {
                        // As IndicatorGID might be present in only one row (Not all rows), so extract that row.
                        worksheet.Cells[DESCellAddress.IndicatorGID].Value = IUdataView.Table.Select(Indicator.IndicatorName + " <> '' ")[0][Indicator.IndicatorGId].ToString();
                    }
                }
                else
                {
                    // create column for IndicatorGID, if not present
                    IUdataView.Table.Columns.Add(Indicator.IndicatorGId);
                }

                // 2. Set unit Name and GID at respective cell
                if (IUdataView.Table.Select(Unit.UnitName + " <> '' ").Length > 0)
                {
                    // As UnitName might be present in only one row (Not all rows), so extract that row.
                    worksheet.Cells[DESCellAddress.UnitName].Value = IUdataView.Table.Select(Unit.UnitName + " <> '' ")[0][Unit.UnitName].ToString();
                }
                if (IUdataView.Table.Columns.Contains(Unit.UnitGId))
                {
                    if (IUdataView.Table.Select(Unit.UnitGId + " <> '' ").Length > 0)
                    {
                        // As UnitGID might be present in only one row (Not all rows), so extract that row.
                        worksheet.Cells[DESCellAddress.UnitGID].Value = IUdataView.Table.Select(Unit.UnitGId + " <> '' ")[0][Unit.UnitGId].ToString();
                    }
                }
                else
                {
                    // create column for UnitGID, if not present
                    IUdataView.Table.Columns.Add(Unit.UnitGId);
                }

                // 3. Set Sector and Class 
                if (IUdataView.Table.Columns.Contains(Constants.SectorColumnName))
                {
                    // As Sector value is present in only one row (Not all rows), so extract that row.
                    if (IUdataView.Table.Select(Constants.SectorColumnName + " <> '' ").Length > 0)
                    {
                        worksheet.Cells[DESCellAddress.Sector].Value = IUdataView.Table.Select(Constants.SectorColumnName + " <> '' ")[0][Constants.SectorColumnName].ToString();
                    }
                }
                if (IUdataView.Table.Columns.Contains(Constants.ClassColumnName))
                {
                    // As Class value is present in only one row (Not all rows), so extract that row.
                    if (IUdataView.Table.Select(Constants.ClassColumnName+ " <> '' ").Length > 0)
                    {
                        worksheet.Cells[DESCellAddress.Class].Value = IUdataView.Table.Select(Constants.ClassColumnName + " <> '' ")[0][Constants.ClassColumnName].ToString();
                    }
                }


                //4.  Reorder columns as required for DES. (Timeperiod, AreaID, AreaName, DataValue, SubgroupVal, Source, Footnote, Denominator)
                DEScolumns[0] = Timeperiods.TimePeriod;
                DEScolumns[1] = Area.AreaID;
                DEScolumns[2] = Area.AreaName;
                DEScolumns[3] = Data.DataValue;
                DEScolumns[4] = SubgroupVals.SubgroupVal;
                DEScolumns[5] = IndicatorClassifications.ICName;
                DEScolumns[6] = FootNotes.FootNote;
                DEScolumns[7] = Data.DataDenominator;
                DEScolumns[8] = SubgroupVals.SubgroupValGId;
                if (!(IUdataView.Table.Columns.Contains(SubgroupVals.SubgroupValGId)))
                {
                    // create column for SubgroupValGID
                    IUdataView.Table.Columns.Add(SubgroupVals.SubgroupValGId);
                }
                if (!(IUdataView.Table.Columns.Contains(FootNotes.FootNote)))
                {
                    IUdataView.Table.Columns.Add(FootNotes.FootNote);
                }

                 //Change 4th Column 'Data_Value'- format to Text
                worksheet.Range["D:D"].NumberFormat = "@";
                


                // 5. Paste all data from DataView at right cell in spreadsheet.
                Range = worksheet.Cells[DESCellAddress.DataStartingCell];
                Range.CopyFromDataTable(IUdataView.ToTable(false, DEScolumns), SpreadsheetGear.Data.SetDataFlags.NoColumnHeaders);

                // 6. Copy & Paste Subgroup GUID column from "I" to "L" column.
                IRange SourceRange = worksheet.Cells["I11:I" + worksheet.UsedRange.Rows.Count.ToString()];
                IRange DestRange = worksheet.Cells["L11:L" + worksheet.UsedRange.Rows.Count.ToString()];
                SourceRange.Copy(DestRange);
                worksheet.Cells["I11:I" + worksheet.UsedRange.Rows.Count.ToString()].Delete(DeleteShiftDirection.Up);

                // 7. Auto-Resize column width. 
                worksheet.Cells.Columns.AutoFit();
                worksheet.Cells.Columns["B9"].ColumnWidth = 14 ;    // AreaID column width. TODO set dynamically
                //worksheet.Cells.Columns["A9"].ColumnWidth = 10;     // TimePeriod Column width
                
                // Set FontSize = 8 for Rows after 9th row.
                IFont SheetFont = worksheet.Cells["A9:L" + worksheet.UsedRange.Rows.Count.ToString()].Font;
                SheetFont.Size = 8;

                // 8. Language handling of headings.
                ExportDES.SetDESLanguageHeadings(worksheet, languageFileNameWPath);
                
            }
            catch (Exception ex)
            {

                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

        }

        /// <summary>
        /// It creates worksheet i.e Data Entry spreadsheet for MICS, filling Indicator, Unit and other data provided in DataView.
        /// </summary>
        /// <param name="worksheet">Worksheet object to be filled with data.</param>
        /// <param name="dataView">Dataview as Datasource having all neccesory column to make DES.</param>
        /// <returns></returns>
        internal static void ExportMICSDESData(IWorksheet worksheet, DataView IUdataView, string languageFileNameWPath)
        {
            IRange Range;
            string[] DEScolumns = new string[7];
            // logic for generating single DevInfo DataEntryspreadsheet from a DataView.
            // use blank spreadsheet from resource file.
            try
            {
                worksheet.Unprotect(string.Empty);

                // 1. Set Indicator Name and GId at respective cells.
                if (IUdataView.Table.Select(Indicator.IndicatorName + " <> '' ").Length > 0)
                {
                    // As IndicatorName might be present in only one row (Not all rows), so extract that row.
                    worksheet.Cells[DESCellAddress.IndicatorName].Value = IUdataView.Table.Select(Indicator.IndicatorName + " <> '' ")[0][Indicator.IndicatorName].ToString();
                }

                if (IUdataView.Table.Columns.Contains(Indicator.IndicatorGId))
                {
                    if (IUdataView.Table.Select(Indicator.IndicatorGId + " <> '' ").Length > 0)
                    {
                        // As IndicatorGID might be present in only one row (Not all rows), so extract that row.
                        worksheet.Cells[DESCellAddress.IndicatorGID].Value = IUdataView.Table.Select(Indicator.IndicatorName + " <> '' ")[0][Indicator.IndicatorGId].ToString();
                    }
                }
                else
                {
                    // create column for IndicatorGID, if not present
                    IUdataView.Table.Columns.Add(Indicator.IndicatorGId);
                }

                // 2. Set unit Name and GID at respective cell
                if (IUdataView.Table.Select(Unit.UnitName + " <> '' ").Length > 0)
                {
                    // As UnitName might be present in only one row (Not all rows), so extract that row.
                    worksheet.Cells[DESCellAddress.UnitName].Value = IUdataView.Table.Select(Unit.UnitName + " <> '' ")[0][Unit.UnitName].ToString();
                }
                if (IUdataView.Table.Columns.Contains(Unit.UnitGId))
                {
                    if (IUdataView.Table.Select(Unit.UnitGId + " <> '' ").Length > 0)
                    {
                        // As UnitGID might be present in only one row (Not all rows), so extract that row.
                        worksheet.Cells[DESCellAddress.UnitGID].Value = IUdataView.Table.Select(Unit.UnitGId + " <> '' ")[0][Unit.UnitGId].ToString();
                    }
                }
                else
                {
                    // create column for UnitGID, if not present
                    IUdataView.Table.Columns.Add(Unit.UnitGId);
                }

                // 3. Set Sector and Class 
                if (IUdataView.Table.Columns.Contains(Constants.SectorColumnName))
                {
                    // As Sector value is present in only one row (Not all rows), so extract that row.
                    if (IUdataView.Table.Select(Constants.SectorColumnName + " <> '' ").Length > 0)
                    {
                        worksheet.Cells[DESCellAddress.Sector].Value = IUdataView.Table.Select(Constants.SectorColumnName + " <> '' ")[0][Constants.SectorColumnName].ToString();
                    }
                }
                if (IUdataView.Table.Columns.Contains(Constants.ClassColumnName))
                {
                    // As Class value is present in only one row (Not all rows), so extract that row.
                    if (IUdataView.Table.Select(Constants.ClassColumnName + " <> '' ").Length > 0)
                    {
                        worksheet.Cells[DESCellAddress.Class].Value = IUdataView.Table.Select(Constants.ClassColumnName + " <> '' ")[0][Constants.ClassColumnName].ToString();
                    }
                }


                //4.  Reorder columns as required for DES. (Timeperiod, AreaID, AreaName, DataValue, SubgroupVal, Source, Footnote, Denominator)
                DEScolumns[0] = Timeperiods.TimePeriod;
                DEScolumns[1] = Data.DataValue;
                DEScolumns[2] = SubgroupVals.SubgroupVal;
                DEScolumns[3] = IndicatorClassifications.ICName;
                DEScolumns[4] = FootNotes.FootNote;
                DEScolumns[5] = Data.DataDenominator;
                DEScolumns[6] = SubgroupVals.SubgroupValGId;
                if (!(IUdataView.Table.Columns.Contains(SubgroupVals.SubgroupValGId)))
                {
                    // create column for SubgroupValGID
                    IUdataView.Table.Columns.Add(SubgroupVals.SubgroupValGId);
                }
                if (!(IUdataView.Table.Columns.Contains(FootNotes.FootNote)))
                {
                    IUdataView.Table.Columns.Add(FootNotes.FootNote);
                }

                //Change 4th Column 'Data_Value'- format to Text
                worksheet.Range["B:B"].NumberFormat = "@";


                // 5. Paste all data from DataView at right cell in spreadsheet.
                Range = worksheet.Cells[DESCellAddress.DataStartingCell];
                Range.CopyFromDataTable(IUdataView.ToTable(false, DEScolumns), SpreadsheetGear.Data.SetDataFlags.NoColumnHeaders);

                // 6. Copy & Paste Subgroup GUID column from "I" to "L" column.

                IRange SourceRange = worksheet.Cells["G11:G" + worksheet.UsedRange.Rows.Count.ToString()];
                IRange DestRange = worksheet.Cells["L11:L" + worksheet.UsedRange.Rows.Count.ToString()];
                SourceRange.Copy(DestRange);
                worksheet.Cells["G11:G" + worksheet.UsedRange.Rows.Count.ToString()].Delete(DeleteShiftDirection.Up);

                worksheet.Cells.Rows.AutoFit();

                IRange UnlockRange = worksheet.Range.Columns["A:A"];
                UnlockRange.Locked = false;
                UnlockRange = worksheet.Range["B:B"];
                UnlockRange.Locked = false;
                UnlockRange = worksheet.Range["C:C"];
                UnlockRange.Locked = false;
                UnlockRange = worksheet.Range["D:D"];
                UnlockRange.Locked = false;
                UnlockRange = worksheet.Range["E:E"];
                UnlockRange.Locked = false;
                UnlockRange = worksheet.Range["F:F"];
                UnlockRange.Locked = false;
                UnlockRange = worksheet.Range["G:G"];
                UnlockRange.Locked = false;
                UnlockRange = worksheet.Range["H7"];
                UnlockRange.Locked = false;

                UnlockRange = worksheet.Range["A1:G10"];
                UnlockRange.Locked = true;
            }
            catch (Exception ex)
            {

                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (worksheet != null)
                {
                    worksheet.Protect(string.Empty);
                }
            }
        }

        internal static void PasteDataViewInSheet(IWorksheet worksheet, DataView dvSource, string cellAddress)
        {
            IRange Range = worksheet.Cells[cellAddress];
            Range.CopyFromDataTable(dvSource.ToTable(), SpreadsheetGear.Data.SetDataFlags.NoColumnHeaders);

        }

        internal static void ExportIndicatorClassification(ICType ICElementType, IWorksheet worksheet, DataView dataView)
        {
            try
            {
                IRange Range;
                IFont HeadingFont;
                string WorksheetHeading = string.Empty;
                // String Array having required columns in IndicatorEntry Spreadsheet.
                string[] ICColumns = new string[2];
                ICColumns[0] = Indicator.IndicatorName;
                ICColumns[1] = Indicator.IndicatorGId;

                // Set Heading on Cell A1
                Range = worksheet.Cells["A1"];
                switch (ICElementType)
                {
                    case ICType.Sector:
                        Range.Value = "SECTOR";
                        break;
                    case ICType.Goal:
                        Range.Value = "GOAL";
                        break;
                    case ICType.CF:
                        Range.Value = "CF";
                        break;
                    case ICType.Institution:
                        Range.Value = "INSTITUTION";
                        break;
                    case ICType.Theme:
                        Range.Value = "THEME";
                        break;
                    case ICType.Convention:
                        Range.Value = "CONVENTION";
                        break;
                    case ICType.Source:
                        Range.Value = "SOURCE";
                        break;
                }

                // Set heading Font.
                HeadingFont = Range.Font;
                HeadingFont.Bold = true;
                HeadingFont.Size = 14;

                // Wrap cells for heading
                Range = worksheet.Cells[0,0, 0, worksheet.UsedRange.ColumnCount - 1];
                Range.Merge();

                //Set Borders

                // Paste dataTable on A4 Cell along with column Name.
                Range = worksheet.Cells["A4"];
                Range.CopyFromDataTable(dataView.ToTable(), SpreadsheetGear.Data.SetDataFlags.AllText);

                // Set column width to twice the current.
                for (int i = 0; i < worksheet.UsedRange.ColumnCount; i++)
                {
                    worksheet.Cells[0, i].ColumnWidth *= 2;
                }

                // Wrap text of Data rows
                Range = worksheet.Cells[3, 0, worksheet.UsedRange.RowCount -1, worksheet.UsedRange.ColumnCount - 1];
                Range.WrapText = true;


                //Save
                worksheet.Workbook.Save();
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

        }

        internal static void ExportArea(IWorksheet worksheet, DataView dataView)
        {
            // String Array having required columns in Area Entry Spreadsheet.
            string[] AreaColumns = new string[5];
                AreaColumns[0] = Area.AreaID;
                AreaColumns[1] = Area.AreaName;
                AreaColumns[2] = Area.AreaLevel;
                AreaColumns[3] = Area.AreaGId;
                AreaColumns[4] = Constants.ParentAreaIDColumn;

            IRange Range = worksheet.Cells[Constants.CellAddress.AreaWorksheetStartAddress];
            Range.CopyFromDataTable(dataView.ToTable(false, AreaColumns), SpreadsheetGear.Data.SetDataFlags.NoColumnHeaders);

            // Auto-Resize column width. 
            worksheet.Cells.Columns.AutoFit();

            // TODO Language handling of headings.       
        }

        internal static void ExportIndicator(IWorksheet worksheet, DataView dataView)
        {
            // String Array having required columns in IndicatorEntry Spreadsheet.
            string[] IndicatorColumns = new string[2];
                IndicatorColumns[0] = Indicator.IndicatorName;
                IndicatorColumns[1] = Indicator.IndicatorGId;
            
            IRange Range = worksheet.Cells[Constants.CellAddress.IndicatorWorksheetStartAddress];
       
            Range.CopyFromDataTable(dataView.ToTable(true, IndicatorColumns), SpreadsheetGear.Data.SetDataFlags.NoColumnHeaders);

            #region "-- Changes for Language based string for CensusInfo "Data/Indicator" "
                      
            //-- Replace Indicator with Language string
            worksheet.Name =  DILanguage.GetLanguageString(Constants.IUSSheet.Indicator).Replace(@"/","_");
            worksheet.Range[0, 0].Value = Convert.ToString(worksheet.Range[0, 0].Value).Replace("Indicator", DILanguage.GetLanguageString("INDICATOR"));

            #endregion
            // Auto-Resize column width. 
            worksheet.Cells.Columns.AutoFit();

            // TODO Language handling of headings.
            //ExportDES.SetDESLanguageHeadings(worksheet, languageFileNameWPath);


        }

        # endregion


        internal static bool ExportDataView(DataView dataView, string outputFileNameWPath)
        {
            bool RetVal = false;

            try
            {
                IWorkbook Workbook = SpreadsheetGear.Factory.GetWorkbook();

                ExportDES.ExportData(Workbook.Worksheets[0], dataView, null, null, Color.Black);
                Workbook.SaveAs(outputFileNameWPath, FileFormat.XLS97);
                Workbook.Close();
                RetVal = true;
            }
            catch (Exception ex)
            {

                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }            
            return RetVal;
        }

        internal static bool ExportDataView(DataView dataView, Font columnHeaderFont, Font dataRowFont, Color globalColor, string outputFileNameWPath)
        {
            bool RetVal = false;

            try
            {
                IWorkbook Workbook = SpreadsheetGear.Factory.GetWorkbook();

                ExportDES.ExportData(Workbook.Worksheets[0], dataView, columnHeaderFont, dataRowFont, globalColor);
                Workbook.SaveAs(outputFileNameWPath, FileFormat.XLS97);
                Workbook.Close();
                RetVal = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        internal static bool ExportICFromDataView(DataView dataView, ICType ICElementType, string xlsFileNameWPath)
        {
            bool RetVal = false;
            IWorkbook ICWorkbook = null;
            try
            {
                // 1. Get blank Workbook  and save it as specified filenameWpath.
                ExportDES.GetWorkbookFromResourceFile(WorkbookType.General, xlsFileNameWPath);

                ICWorkbook = SpreadsheetGear.Factory.GetWorkbook(xlsFileNameWPath);

               // 2. Paste DataView on worksheet
                ExportDES.ExportIndicatorClassification(ICElementType, ICWorkbook.Worksheets[0], dataView);

                ICWorkbook.Save();
                ICWorkbook.Close();
                RetVal = true;

            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        internal static bool ExportIndicatorEntrySpreadsheet(DataView dataView, string xlsFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;
            IWorkbook IndicatorWorkbook = null;

            try
            {
                // 1. validate required columns (indicator_Name , Indicator_GID)
                if (dataView.Table.Columns.Contains(Indicator.IndicatorName) && dataView.Table.Columns.Contains(Indicator.IndicatorGId))
                {
                    // 2. Get blank fileName and save it as specified filenameWpath.
                    ExportDES.GetWorkbookFromResourceFile(WorkbookType.IndicatorEntrySpreadsheet, xlsFileNameWPath);

                    IndicatorWorkbook = SpreadsheetGear.Factory.GetWorkbook(xlsFileNameWPath);

                    // 3. Rename Worksheet name to "Indicator"

                    // 4. Paste DataView on worksheet
                    ExportDES.ExportIndicator(IndicatorWorkbook.Worksheets[0], dataView);

                    // TODO language handling
                    IndicatorWorkbook.Save();
                    IndicatorWorkbook.Close();
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        internal static bool ExportAreaEntrySpreadsheet(DataView dataView, string xlsFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;
            IWorkbook AreaWorkbook = null;

            try
            {
                // 1. validate required columns (Area_ID, Area_Name, Area_Gid, Area_Level, Parent_ID)
                if (dataView.Table.Columns.Contains(Area.AreaID) && dataView.Table.Columns.Contains(Area.AreaName) && dataView.Table.Columns.Contains(Area.AreaLevel) && dataView.Table.Columns.Contains(Area.AreaGId) && dataView.Table.Columns.Contains(Constants.ParentAreaIDColumn))
                {
                    // 2. Get blank fileName and save it as specified filenameWpath.
                    ExportDES.GetWorkbookFromResourceFile(WorkbookType.AreaEntrySpreadsheet, xlsFileNameWPath);

                    AreaWorkbook = SpreadsheetGear.Factory.GetWorkbook(xlsFileNameWPath);

                    // 3. Rename Worksheet name to "Area"

                    // 4. Paste DataView on worksheet
                    ExportDES.ExportArea(AreaWorkbook.Worksheets[0], dataView);

                    AreaWorkbook.Save();
                    AreaWorkbook.Close();
                    RetVal = true;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
            return RetVal;
        }

        internal static void SetDESLanguageHeadings(IWorksheet worksheet, string languageFileNameWPath)
        {
            try
            {
                if (File.Exists(languageFileNameWPath))
                {
                    // 
                    DILanguage.Open(languageFileNameWPath);
                    worksheet.Cells[DESCellAddress.DESLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.DESHeading);
                    worksheet.Cells[DESCellAddress.SectorLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.Sector);
                    worksheet.Cells[DESCellAddress.ClassLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.Class);
                    worksheet.Cells[DESCellAddress.IndicatorLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.Indicator);
                    worksheet.Cells[DESCellAddress.UnitLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.Unit);
                    worksheet.Cells[DESCellAddress.TimeLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.TimePeriod);
                    worksheet.Cells[DESCellAddress.AreaIDLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.AreaID);
                    worksheet.Cells[DESCellAddress.AreaNameLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.AreaName);
                    worksheet.Cells[DESCellAddress.DataValueLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.DataValue);
                    worksheet.Cells[DESCellAddress.SubgroupLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.Subgroup);
                    worksheet.Cells[DESCellAddress.SourceLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.Source);
                    worksheet.Cells[DESCellAddress.FootnoteLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.Footnotes);
                    worksheet.Cells[DESCellAddress.DenominatorLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.Denominator);
                    worksheet.Cells[DESCellAddress.DecimalLable].Value = DILanguage.GetLanguageString(Constants.DESLanguageKeys.Decimals);

                }
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }
        }

        internal static void SetWorksheetHyperlink(IWorkbook workbook, int sheetIndex, int rowindex, int columnindex, string targetHyperlinkWorksheetName, string targetHyperlinkCellAddress, string textToDisplay)
        {
            try
            {
                SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];

                IRange Range = worksheet.Cells[rowindex, columnindex];

                worksheet.Hyperlinks.Add(Range, null, "'" + targetHyperlinkWorksheetName + "'!" + targetHyperlinkCellAddress, "", textToDisplay);
                //E.g: worksheet.Hyperlinks.Add(Range, @"http://www.google.com", null, "", "");
            }
            catch (Exception)
            {
            }
        }
    }
}
