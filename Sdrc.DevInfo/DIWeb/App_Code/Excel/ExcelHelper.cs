using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using SpreadsheetGear;
using System.Drawing;

internal class ExcelHelper
{

    /// <summary>
    /// Open excel file 
    /// </summary>
    /// <param name="excelFileName">Excel file name with path</param>
    /// <returns>IWorkBook</returns>
    internal static IWorkbook OpenExcelFile(string excelFileName)
    {
        IWorkbook RetVal = null;
        try
        {
            // Open a workbook file.
            RetVal = SpreadsheetGear.Factory.GetWorkbook(excelFileName);

        }
        catch (Exception ex)
        {
            throw ex;
        }
        return RetVal;
    }

    /// <summary>
    /// Returns the number of used rows
    /// </summary>
    /// <param name="workbook">Instance of workbook </param>
    /// <param name="sheetIndex">Sheet index starts from zero</param>
    /// <returns></returns>
    internal static int GetUsedRowsCount(IWorkbook workbook, int sheetIndex)
    {
        int RetVal = 0;
        try
        {
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            RetVal = worksheet.UsedRange.RowCount;
        }
        catch (Exception ex)
        {
             throw ex;
            RetVal = 0;
        }
        return RetVal;
    }

    /// <summary>
    /// Returns the number of used rows
    /// </summary>
    /// <param name="workbook">Instance of workbook </param>
    /// <param name="sheetIndex">Sheet index starts from zero</param>
    /// <returns></returns>
    internal static IRange GetUsedRange(IWorkbook workbook, int sheetIndex)
    {
        IRange RetVal = null;
        try
        {
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            RetVal = worksheet.UsedRange;
        }
        catch (Exception ex)
        {
             throw ex;
        }
        return RetVal;
    }


    /// <summary>
    /// To insert new worksheet.
    /// </summary>
    /// <param name="workbook">Iworkbook Instance.</param>
    /// <param name="sheetName">Sheet name </param>
    internal static void InsertWorkSheet(SpreadsheetGear.IWorkbook workbook, string sheetName)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetName];

            if (worksheet == null)
            {

                workbook.Worksheets.Add();
                worksheet = workbook.Worksheets[workbook.Worksheets.Count - 1];
            }

            // Set the worksheet name.
            worksheet.Name = sheetName;
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Append new worksheet at end
    /// </summary>
    /// <param name="workbook">Iworkbook Instance.</param>
    /// <param name="sheetName">Sheet name </param>
    internal static void AppendWorkSheet(SpreadsheetGear.IWorkbook workbook, string sheetName)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetName];
            worksheet.CopyAfter(worksheet);
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Delete the worksheet
    /// </summary>
    /// <param name="workbook">Iworkbook Instance.</param>
    /// <param name="sheetIndex">Sheet Index</param>
    internal static void DeleteWorkSheet(SpreadsheetGear.IWorkbook workbook, int sheetIndex)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            worksheet.Delete();
        }
        catch (Exception)
        {
        }
    }

    internal static void SetReadOnlyRange(SpreadsheetGear.IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            IRange Range = worksheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            //Range.Validation.Delete();
            Range.Validation.Add(ValidationType.Custom, ValidationAlertStyle.Stop, ValidationOperator.Equal, "''", "");
            Range.Validation.ErrorTitle = "CRING Online";
            Range.Validation.ErrorMessage = "Read Only Cell";
            Range.Validation.IgnoreBlank = false;
        }
        catch (Exception)
        {
        }
    }

    internal static void MakeDropDownList(SpreadsheetGear.IWorkbook workbook, int sheetIndex, int row, int col, string cellAddress)   
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            IRange Range = worksheet.Cells[row, col];
            Range.Validation.Add(ValidationType.List, ValidationAlertStyle.Information, ValidationOperator.Default, "=" + cellAddress, "");           
        }
        catch (Exception)
        {
        }
    }

    internal static void SetValueByDropDownSelection(SpreadsheetGear.IWorkbook workbook, int sheetIndex, int rowIndex, int colIndex, string dropDownAddress, string sourceAddress)
    {   
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            IRange Range = worksheet.Cells[rowIndex, colIndex];
            Range.Validation.Add(ValidationType.Custom, ValidationAlertStyle.Information, ValidationOperator.Default, "=VLOOKUP(" + dropDownAddress + "," + sourceAddress + ",2)", "");
        }
        catch (Exception)
        {
        }
    }

    internal static void RemoveValidation(SpreadsheetGear.IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            IRange Range = worksheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            Range.Validation.Delete();
        }
        catch (Exception)
        {
        }
    }

    internal static void SetValidationList(SpreadsheetGear.IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, string list)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            if (endRowIndex == -1)
            {
                endRowIndex = worksheet.Cells.RowCount - 1;
            }
            IRange Range = worksheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            Range.Validation.Add(ValidationType.List, ValidationAlertStyle.Stop, ValidationOperator.Between, list, "");
            Range.Validation.ErrorTitle = "";
            Range.Validation.ErrorMessage = "";
            Range.Validation.IgnoreBlank = true;
        }
        catch (Exception)
        {
        }
    }

    #region "-- Working with Data Table --"

    /// <summary>
    /// To load Data Table into worksheet.
    /// </summary>
    /// <param name="workbook">IWorkBook instance</param>
    /// <param name="worksheetDataTable">Data table</param>
    /// <param name="sheetName">Sheet name in which data has to be inserted. </param>
    internal static void LoadDataTableIntoSheet(SpreadsheetGear.IWorkbook workbook, DataTable worksheetDataTable, string sheetName)
    {
        // Acquire a workbook set lock.
        //this.workbook.GetLock();
        try
        {
            ExcelHelper.LoadDataTableIntoSheet(workbook, "$A$1", worksheetDataTable, sheetName, false, true);
            //// Get a workbook reference.
            ////SpreadsheetGear.IWorkbook workbook = this.workbook.ActiveWorkbook;
            //// Get a worksheet reference.
            //SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetName];


            //if (worksheet == null)
            //{

            //    workbook.Worksheets.Add();
            //    worksheet = workbook.Worksheets[workbook.Worksheets.Count - 1];
            //}
            ////clear all values
            //worksheet.Cells.Clear();

            //// Set the worksheet name.
            //worksheet.Name = sheetName;

            //// Get the top left cell for the DataTable.
            //SpreadsheetGear.IRange cell = worksheet.Cells["$A$1"];

            //// set the format to text
            //worksheet.Cells[0, 0, worksheetDataTable.Rows.Count + 7, 19].NumberFormat = "@";



            //// Copy the DataTable to the worksheet range.
            //cell.CopyFromDataTable(worksheetDataTable, SpreadsheetGear.Data.SetDataFlags.None);


            //// Auto size all worksheet columns which contain data.
            //worksheet.UsedRange.Columns.AutoFit();
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
        finally
        {
            // Release the workbook set lock.
            //this.workbook.ReleaseLock();
        }
    }

    /// <summary>
    /// To load Data Table into worksheet.
    /// </summary>
    /// <param name="workbook">IWorkBook instance</param>
    /// <param name="startPosition">Starting position of datatable like $A$10</param>
    /// <param name="worksheetDataTable">Data table</param>
    /// <param name="sheetName">Sheet name in which data has to be inserted. </param>
    /// <param name="SupressColumnHeader">Set to true, to hide column header and false to display column header</param>
    /// <param name="FirstClearAllCells">set to true, if you want to clear all cells before pasting datatable into worksheet </param>
    internal static void LoadDataTableIntoSheet(SpreadsheetGear.IWorkbook workbook, string startPosition, DataTable worksheetDataTable, string sheetName, bool SupressColumnHeader, bool firstClearAllCells)
    {
        // Acquire a workbook set lock.
        //this.workbook.GetLock();
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetName];


            if (worksheet == null)
            {
                workbook.Worksheets.Add();
                worksheet = workbook.Worksheets[workbook.Worksheets.Count - 1];
            }
            // Set the worksheet name.
            worksheet.Name = sheetName;


            //clear all values
            if (firstClearAllCells)
            {
                worksheet.Cells.Clear();
            }


            // Get the cell range for the DataTable.
            SpreadsheetGear.IRange cell = worksheet.Cells[startPosition];//["$A$1"];


            // set the format to text
            worksheet.Cells[0, 0, worksheetDataTable.Rows.Count + 7, 19].NumberFormat = "@";


            // Copy the DataTable to the worksheet range.
            cell.CopyFromDataTable(worksheetDataTable, SpreadsheetGear.Data.SetDataFlags.None);


            // Auto size all worksheet columns which contain data.
            worksheet.UsedRange.Columns.AutoFit();

            // delete column headers row
            if (SupressColumnHeader)
            {
                IRange deleteRow = worksheet.Cells[startPosition];
                deleteRow.EntireRow.Delete(DeleteShiftDirection.Up);
            }

        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
        finally
        {
            // Release the workbook set lock.
            //this.workbook.ReleaseLock();
        }
    }


    /// <summary>
    /// To load Data Table into worksheet.
    /// </summary>
    /// <param name="workbook">IWorkBook instance</param>
    /// <param name="rowIndex">zero based row index</param>
    /// <param name="colIndex">zero based column index</param>
    /// <param name="worksheetDataTable">Data table</param>
    /// <param name="sheetIndex">Zero based sheet index</param>
    /// <param name="SupressColumnHeader">Set to true, to hide column header and false to display column header</param>
    internal static void LoadDataTableIntoSheet(SpreadsheetGear.IWorkbook workbook, int rowIndex, int colIndex, DataTable worksheetDataTable, int sheetIndex, bool SupressColumnHeader)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];


            if (worksheet == null)
            {
                workbook.Worksheets.Add();
                worksheet = workbook.Worksheets[workbook.Worksheets.Count - 1];
            }


            // Get the cell range for the DataTable.
            SpreadsheetGear.IRange cell = worksheet.Cells[rowIndex, colIndex];//["$A$1"];

            // Copy the DataTable to the worksheet range.
            cell.CopyFromDataTable(worksheetDataTable, SpreadsheetGear.Data.SetDataFlags.None);


            // delete column headers row
            if (SupressColumnHeader)
            {
                IRange deleteRow = worksheet.Cells[rowIndex, colIndex];
                deleteRow.EntireRow.Delete(DeleteShiftDirection.Up);
            }

        }
        catch (Exception ex)
        {
             throw ex;
        }
        finally
        {
            // Release the workbook set lock.
            //this.workbook.ReleaseLock();
        }
    }



    /// <summary>
    /// Returns Data Table. Converts given worksheet into data table.  
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="excelFileName">Excel file name with path.</param>
    /// <param name="sheetName">Worksheet name.</param>
    /// <returns> DataTable</returns>
    internal static DataTable GetDataTableFromSheet(IWorkbook workbook, string excelFileName, string sheetName)
    {
        DataTable RetVal = new DataTable(sheetName);
        DataSet ExcelDataSet = new DataSet(System.IO.Path.GetFileNameWithoutExtension(excelFileName));
        SpreadsheetGear.IRange Cell;
        SpreadsheetGear.IWorksheet Worksheet;

        try
        {
            // Create a workbook from an Excel file.
            workbook = SpreadsheetGear.Factory.GetWorkbook(excelFileName);

            // Get a worksheet reference.
            Worksheet = workbook.Worksheets[sheetName];
            if (Worksheet != null)
            {

                // Get the top left cell for the DataTable.
                //                Cell = Worksheet.Cells["$A$1"];
                Cell = Worksheet.Cells[Worksheet.UsedRange.Address];
                RetVal = Cell.GetDataTable(SpreadsheetGear.Data.GetDataFlags.FormattedText);

            }
            else
            {
                RetVal = new DataTable(sheetName);
            }

            //ExcelDataSet = workbook.GetDataSet(SpreadsheetGear.Data.GetDataFlags.FormattedText);

            //if (ExcelDataSet.Tables.Contains(sheetName))
            //{
            //    RetVal = ExcelDataSet.Tables[sheetName];
            //}
        }
        catch (Exception ex)
        {
            RetVal = new DataTable(sheetName);
            //                 throw ex;
        }
        return RetVal;
    }

    #endregion

    #region "-- Working with Array & range --"


    /// <summary>
    /// To set array values into worksheet.
    /// </summary>
    /// <param name="workbook">Iworkbook Instance.</param>
    /// <param name="sheetName">Sheet name in which data has to be inserted</param>
    /// <param name="startRowIndex">Start row index starts from zero</param>
    /// <param name="startColIndex">Start column index starts from zero</param>
    /// <param name="endRowIndex">End row index starts from zero</param>
    /// <param name="endColIndex">End column index starts from zero</param>
    /// <param name="valueArray"></param>
    internal static void SetArrayValuesIntoSheet(SpreadsheetGear.IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, object[,] valueArray)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];

            if (worksheet == null)
            {

                workbook.Worksheets.Add();
                worksheet = workbook.Worksheets[workbook.Worksheets.Count - 1];
            }

            // Set the worksheet name.
            //worksheet.Name = sheetName;

            // Get the cell range for the DataTable.				
            SpreadsheetGear.IRange Range = worksheet.Cells[startRowIndex, startColIndex, endRowIndex - 1, endColIndex - 1];

            //set array value
            Range.Value = valueArray;


            // Auto fit the range
            Range.Columns.AutoFit();


        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
        finally
        {
            // Release the workbook set lock.
            //this.workbook.ReleaseLock();
        }
    }

    #endregion

    #region "-- Cell Value --"

    /// <summary>
    /// Returns cell value as string.
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    /// <param name="cellAddress">address of cell like A4 .</param>
    /// <returns> string</returns>
    internal static string GetCellValue(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        string RetVal = string.Empty;
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            if (worksheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex].Value == null)
            {
                RetVal = string.Empty;
            }
            else
            {
                RetVal = worksheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex].Value.ToString();
            }
        }
        catch (Exception ex)
        {
            //                 throw ex;
            RetVal = string.Empty;
        }
        return RetVal;
    }


    /// <summary>
    /// Returns cell value as string.
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    /// <param name="cellAddress">address of cell like A4 .</param>
    /// <returns> string</returns>
    internal static string GetCellValue(IWorkbook workbook, int sheetIndex, string cellAddress)
    {
        string RetVal = string.Empty;
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];

            if (worksheet.Cells[cellAddress].Value == null)
            {
                RetVal = string.Empty;
            }
            else
            {
                RetVal = worksheet.Cells[cellAddress].Value.ToString();
            }



        }
        catch (Exception ex)
        {
             throw ex;
            RetVal = string.Empty;
        }
        return RetVal;
    }


    /// <summary>
    /// To set value of a cell.
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    /// <param name="cellAddress">Address of cell like A4 .</param>
    /// <param name="value">Value</param>
    /// <returns> string</returns>
    internal static void SetCellValue(IWorkbook workbook, int sheetIndex, string cellAddress, object value)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];

            worksheet.Cells[cellAddress].Value = value;

        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    /// <summary>
    /// To set value of a cell.
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <param name="value">Value</param>
    /// <returns> string</returns>
    internal static void SetCellValue(IWorkbook workbook, int sheetIndex, int rowIndex, int colIndex, object value)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];

            worksheet.Cells[rowIndex, colIndex].Value = value;

        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    #endregion

    #region "-- Comments --"

    /// <summary>
    /// To insert comment in active sheet
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="cellAddress">Address of cell</param>
    /// <param name="comment">Comment string</param>
    /// <param name="clearOldComments">True/False. True to clear old comments</param>
    internal static void AddComment(IWorkbook workbook, string cellAddress, string comment, bool clearOldComments)
    {
        SpreadsheetGear.IRange Cell;
        SpreadsheetGear.IComment CellComment;
        try
        {
            // Get a reference to a cell.
            Cell = workbook.ActiveWorksheet.Cells[cellAddress];
            // If the cell has no comment, add one.
            if (Cell.Comment != null & clearOldComments)
            {
                Cell.ClearComments();
            }

            CellComment = Cell.AddComment(comment);

            // Turn off the default bold font.
            CellComment.Shape.TextFrame.Characters.Font.Bold = false;
            //CellComment.Visible = true;
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    /// <summary>
    /// To insert cell comment 
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="sheetIndex">Worksheet index starts from zero</param>
    /// <param name="rowIndex">Row Index starts from zero</param>
    /// <param name="colIndex">Column Index starts from zero</param>
    /// <param name="comment">Comment string</param>
    /// <param name="clearOldComments">True/False. True to clear old comments</param>
    internal static void AddComment(IWorkbook workbook, int sheetIndex, int rowIndex, int colIndex, string comment, bool clearOldComments)
    {
        SpreadsheetGear.IRange Cell;
        SpreadsheetGear.IComment CellComment;
        try
        {
            // Get a reference to a cell.
            Cell = workbook.Worksheets[sheetIndex].Cells[rowIndex, colIndex];

            // If the cell has no comment, add one.
            if (Cell.Comment != null & clearOldComments)
            {
                Cell.ClearComments();
            }

            CellComment = Cell.AddComment(comment);

        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    #endregion


    /// <summary>
    /// Returns cell height .
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="rowIndex">Row index starts from zero</param>
    ///<param name="colIndex">Column index starts from zero</param>
    /// <returns> double</returns>
    internal static double GetCellHeight(IWorkbook workbook, int sheetIndex, int colIndex, int rowIndex)
    {
        double RetVal = 0;
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];

            RetVal = worksheet.Cells[rowIndex, colIndex].RowHeight;
        }
        catch (Exception ex)
        {
            RetVal = 0;
        }
        return RetVal;
    }

    /// <summary>
    /// Set the range font and size
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="sheetIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="startColIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColIndex"></param>
    /// <param name="fontname"></param>
    /// <param name="fontsize"></param>
    internal static void SetFont(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, string fontname, int fontsize)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            IRange Range = worksheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            Range.Font.Name = fontname;
            Range.Font.Size = fontsize;
        }
        catch (Exception)
        {
        }
    }


    /// <summary>
    /// Activate the Cell by applying Select() method of spreadsheet gear.
    /// </summary>
    /// <param name="workbook">Active workbook</param>
    /// <param name="sheetIndex">sheet Index</param>
    /// <param name="colIndex">Column index</param>
    /// <param name="rowIndex">Row index</param>
    /// <returns></returns>
    internal static void SelectCell(IWorkbook workbook, int sheetIndex, int colIndex, int rowIndex)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            worksheet.Cells[rowIndex, colIndex].Select();
        }
        catch (Exception ex)
        {
        }
    }

    internal static void ActivateCell(IWorkbook workbook, int sheetIndex, int colIndex, int rowIndex)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            worksheet.Cells[rowIndex, colIndex].Activate();
        }
        catch (Exception ex)
        {
        }
    }

    /// <summary>
    /// Returns reference of ICharacters which can be used to set the format of characters of a cell.
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="sheetIndex">Sheet index</param>
    /// <param name="colIndex">Zero based column index </param>
    /// <param name="rowIndex">Zero based row index</param>
    /// <param name="start">Zero based starting character</param>
    /// <param name="length">The number of characters</param>
    /// <returns></returns>
    internal static ICharacters GetCellCharacters(IWorkbook workbook, int sheetIndex, int colIndex, int rowIndex, int start, int length)
    {
        ICharacters RetVal = null;
        try
        {
            RetVal = workbook.Worksheets[sheetIndex].Cells[rowIndex, colIndex].GetCharacters(start, length);
        }
        catch (Exception)
        {
            RetVal = null;

        }

        return RetVal;
    }

    /// <summary>
    /// Returns cell address after removing sheet name from it.
    /// </summary>
    /// <param name="cellAddress"></param>
    /// <param name="sheetName"></param>
    /// <returns></returns>
    internal static string RemoveSheetNameFrmCellAddress(string cellAddress, string sheetName)
    {
        string RetVal = string.Empty;

        // remove sheet name from cell address 
        if (cellAddress.StartsWith(sheetName))
        {
            RetVal = cellAddress.Substring(sheetName.Length + 2).ToString();
        }

        return RetVal;
    }

    /// <summary>
    /// Returns sheet name
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="sheetIndex">sheetIndex</param>
    /// <returns>Returns sheet name</returns>
    internal static string GetSheetName(IWorkbook workbook, int sheetIndex)
    {
        string RetVal = string.Empty;

        try
        {
            RetVal = workbook.Worksheets[sheetIndex].Name;
        }
        catch (Exception ex)
        {
             throw ex;
            RetVal = string.Empty;
        }
        return RetVal;
    }

    /// <summary>
    /// To delete a row at particular position.
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="position">row position</param>
    internal static void DeleteRowAt(IWorkbook workbook, int sheetIndex, int position)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            worksheet.Cells.Range[position + ":" + position].Delete();
        }
        catch (Exception ex)
        {
             throw ex;
        }
    }
    /// <summary>
    /// To delete a Column at particular position.
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="position">row position</param>
    internal static void DeleteColumnAt(IWorkbook workbook, int sheetIndex, string position)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            worksheet.Cells.Range[position + ":" + position].UnMerge();
            worksheet.Cells.Range[position + ":" + position].Delete();

        }
        catch (Exception ex)
        {
             throw ex;
        }
    }

    /// <summary>
    /// To insert a row at particular position.
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="position">row position</param>
    internal static void InsertRowAt(IWorkbook workbook, int sheetIndex, int position)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            worksheet.Cells.Range[position + ":" + position].Insert();

        }
        catch (Exception ex)
        {
             throw ex;
        }
    }




    /// <summary>
    /// Move Column from a Position to other
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    /// <param name="DestinationPosition">Destination Position</param>
    /// <param name="SourcePosition">Source Position</param>
    internal static void MoveColumnTo(IWorkbook workbook, int sheetIndex, string SourcePosition, string DestinationPosition)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            IRange Rg = worksheet.Cells.Columns[DestinationPosition + ":" + DestinationPosition];
            worksheet.Cells.Columns[SourcePosition + ":" + SourcePosition].Copy(Rg);
        }
        catch (Exception ex)
        {
             throw ex;
        }
    }

    /// <summary>
    /// Move Column from a Position to other
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    /// <param name="DestinationPosition">Destination Position</param>
    /// <param name="SourcePosition">Source Position</param>
    internal static void MoveRowTo(IWorkbook workbook, int sheetIndex, string SourcePosition, string DestinationPosition)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            IRange Rg = worksheet.Cells.Rows[DestinationPosition + ":" + DestinationPosition];
            worksheet.Cells.Rows[SourcePosition + ":" + SourcePosition].Copy(Rg);            
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Get Column Header
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    /// <param name="RowPosition">RowPosition Position</param>
    /// <param name="ColumnPosition">ColumnPosition Position</param>
    internal static string GetColumnHeader(IWorkbook workbook, int sheetIndex, int RowPosition, int ColumnPosition)
    {
        string RetVal = String.Empty;
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            string FirstSubstring = worksheet.Cells[RowPosition, ColumnPosition].Address.Substring(1, 1);
            string SecondSubstring = worksheet.Cells[RowPosition, ColumnPosition].Address.Substring(2, 1);
            if (SecondSubstring != "$")
            {
                RetVal = FirstSubstring + SecondSubstring;
            }
            else
            {
                RetVal = FirstSubstring;
            }

            return RetVal;
        }
        catch (Exception ex)
        {
             throw ex;
            return string.Empty;
        }
    }

    internal static void SetCellFormatType(IWorkbook iWorkbook, int sheetIndex, int rowIndex, int colIndex, NumberFormatType formatType)
    {
        string formatString = string.Empty;
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = iWorkbook.Worksheets[sheetIndex];
            worksheet.Range.Cells[rowIndex, colIndex].Select();
            switch (formatType)
            {
                case NumberFormatType.Currency:
                    worksheet.Range.Cells[rowIndex, colIndex].NumberFormat = "$#,##0.00";
                    break;
                case NumberFormatType.Date:
                    worksheet.Range.Cells[rowIndex, colIndex].NumberFormat = "m/d/yyyy"; ;
                    break;
                case NumberFormatType.DateTime:
                case NumberFormatType.Time:
                    worksheet.Range.Cells[rowIndex, colIndex].NumberFormat = "[$-F400]h:mm:ss AM/PM";
                    break;
                case NumberFormatType.Fraction:
                    worksheet.Range.Cells[rowIndex, colIndex].NumberFormat = "# ?/?";
                    break;
                case NumberFormatType.General:
                    worksheet.Range.Cells[rowIndex, colIndex].NumberFormat = "General";
                    break;
                case NumberFormatType.None:
                    // worksheet.Range.Columns[columnAddress].NumberFormat;
                    break;
                case NumberFormatType.Number:
                    worksheet.Range.Cells[rowIndex, colIndex].NumberFormat = "0.00";
                    break;
                case NumberFormatType.Percent:
                    worksheet.Range.Cells[rowIndex, colIndex].NumberFormat = "0.00%";
                    break;
                case NumberFormatType.Scientific:
                    worksheet.Range.Cells[rowIndex, colIndex].NumberFormat = "0.00E+00";
                    break;
                case NumberFormatType.Text:
                    worksheet.Range.Cells[rowIndex, colIndex].NumberFormat = "@";
                    break;
                default:
                    break;
            }

        }
        catch (Exception ex)
        {
             throw ex;
        }
    }


    internal static void SetRangeFormatType(IWorkbook iWorkbook, int sheetIndex, int startRowIndex, int startColumnIndex, int endRowIndex, int endColIndex, NumberFormatType formatType)
    {
        string formatString = string.Empty;
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = iWorkbook.Worksheets[sheetIndex];
            //worksheet.Range.Cells[rowIndex, colIndex].Select();
            worksheet.Range.Cells[startRowIndex, startColumnIndex, endRowIndex, endColIndex].Select();

            switch (formatType)
            {
                case NumberFormatType.Currency:
                    worksheet.Range.Cells[startRowIndex, startColumnIndex, endRowIndex, endColIndex].NumberFormat = "$#,##0.00";
                    break;
                case NumberFormatType.Date:
                    worksheet.Range.Cells[startRowIndex, startColumnIndex, endRowIndex, endColIndex].NumberFormat = "m/d/yyyy"; ;
                    break;
                case NumberFormatType.DateTime:
                case NumberFormatType.Time:
                    worksheet.Range.Cells[startRowIndex, startColumnIndex, endRowIndex, endColIndex].NumberFormat = "[$-F400]h:mm:ss AM/PM";
                    break;
                case NumberFormatType.Fraction:
                    worksheet.Range.Cells[startRowIndex, startColumnIndex, endRowIndex, endColIndex].NumberFormat = "# ?/?";
                    break;
                case NumberFormatType.General:
                    worksheet.Range.Cells[startRowIndex, startColumnIndex, endRowIndex, endColIndex].NumberFormat = "General";
                    break;
                case NumberFormatType.None:
                    // worksheet.Range.Columns[columnAddress].NumberFormat;
                    break;
                case NumberFormatType.Number:
                    //worksheet.Range.Cells[rowIndex, colIndex].NumberFormat = "0.00";
                    worksheet.Range.Cells[startRowIndex, startColumnIndex, endRowIndex, endColIndex].NumberFormat = "0.#######";
                    break;
                case NumberFormatType.Percent:
                    worksheet.Range.Cells[startRowIndex, startColumnIndex, endRowIndex, endColIndex].NumberFormat = "0.00%";
                    break;
                case NumberFormatType.Scientific:
                    worksheet.Range.Cells[startRowIndex, startColumnIndex, endRowIndex, endColIndex].NumberFormat = "0.00E+00";
                    break;
                case NumberFormatType.Text:
                    worksheet.Range.Cells[startRowIndex, startColumnIndex, endRowIndex, endColIndex].NumberFormat = "@";
                    break;
                default:
                    break;
            }

        }
        catch (Exception ex)
        {
             throw ex;
        }
    }

    /// <summary>
    /// Changes the column format type (eg: text, general, Date, number)
    /// </summary>
    /// <param name="iWorkbook"></param>
    /// <param name="sheetIndex"></param>
    /// <param name="columnAddress">column address. foreg: A:A , B:B</param>
    /// <param name="formatType">format type.</param>
    internal static void SetColumnFormatType(IWorkbook iWorkbook, int sheetIndex, string columnAddress, NumberFormatType formatType)
    {
        string formatString = string.Empty;

        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = iWorkbook.Worksheets[sheetIndex];
            worksheet.Range.Columns[columnAddress].Select();

            switch (formatType)
            {
                case NumberFormatType.Currency:
                    worksheet.Range.Columns[columnAddress].NumberFormat = "$#,##0.00";
                    break;
                case NumberFormatType.Date:
                    worksheet.Range.Columns[columnAddress].NumberFormat = "m/d/yyyy"; ;
                    break;
                case NumberFormatType.DateTime:
                case NumberFormatType.Time:
                    worksheet.Range.Columns[columnAddress].NumberFormat = "[$-F400]h:mm:ss AM/PM";
                    break;
                case NumberFormatType.Fraction:
                    worksheet.Range.Columns[columnAddress].NumberFormat = "# ?/?";
                    break;
                case NumberFormatType.General:
                    worksheet.Range.Columns[columnAddress].NumberFormat = "General";
                    break;
                case NumberFormatType.None:
                    // worksheet.Range.Columns[columnAddress].NumberFormat;
                    break;
                case NumberFormatType.Number:
                    worksheet.Range.Columns[columnAddress].NumberFormat = "0.00";
                    break;
                case NumberFormatType.Percent:
                    worksheet.Range.Columns[columnAddress].NumberFormat = "0.00%";
                    break;
                case NumberFormatType.Scientific:
                    worksheet.Range.Columns[columnAddress].NumberFormat = "0.00E+00";
                    break;
                case NumberFormatType.Text:
                    worksheet.Range.Columns[columnAddress].NumberFormat = "@";
                    break;
                default:
                    break;
            }


        }
        catch (Exception ex)
        {
             throw ex;
        }
    }

    internal static void SetFormatNumbericValue(IWorkbook iWorkbook, int sheetIndex, string columnAddress, bool isDecimalValue, int decimalPlaces, bool isGroupingRequired)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = iWorkbook.Worksheets[sheetIndex];
            worksheet.Range.Columns[columnAddress].Select();
            string DataValueFormat = GetNumberFormat(isDecimalValue, decimalPlaces, isGroupingRequired, Convert.ToDouble(worksheet.Range.Columns[columnAddress].Value));
            if (isDecimalValue)
            {
                worksheet.Range.Columns[columnAddress].NumberFormat = DataValueFormat; // "#,##0.0000";
            }
            else
            {
                worksheet.Range.Columns[columnAddress].NumberFormat = DataValueFormat;
            }
        }
        catch (Exception ex)
        {
             throw ex;
        }
    }

    internal static void SetFormatNumbericValue(IWorkbook iWorkbook, int sheetIndex, string columnAddress, string formattedValue)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = iWorkbook.Worksheets[sheetIndex];
            worksheet.Range.Columns[columnAddress].Select();
            worksheet.Range.Columns[columnAddress].NumberFormat = "@";
            worksheet.Range.Columns[columnAddress].Value = formattedValue; // "#,##0.0000";
        }
        catch (Exception ex)
        {
             throw ex;
        }
    }

    /// <summary>
    /// Get the number format according to the decimal places
    /// </summary>
    /// <param name="decimalPlaces"></param>
    /// <returns></returns>
    private static string GetNumberFormat(bool isDecimalValue, int decimalPlaces, bool isGroupingRequired, double value)
    {
        string Retval = string.Empty;
        try
        {
            if (isGroupingRequired)
            {
                Retval = "#" + ExcellUtility.NumberGroupSeparator + "##0";
            }
            else
            {
                Retval = "###0";
            }

            if (isDecimalValue)
            {
                if (decimalPlaces > 0)
                {
                    Retval += ExcellUtility.NumberDecimalSeparator;
                }
                for (int i = 0; i < decimalPlaces; i++)
                {
                    Retval += "0";
                }
            }
            else
            {
                if (value.ToString().Contains(ExcellUtility.NumberDecimalSeparator))
                {
                    int DecimalPlaces = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalDigits;
                    Retval += ExcellUtility.NumberDecimalSeparator;
                    for (int i = 0; i < DecimalPlaces; i++)
                    {
                        Retval += "#";
                    }
                }
            }
        }
        catch (Exception)
        {
        }
        return Retval;
    }

    /// <summary>
    /// To Merge Cells
    /// </summary>
    /// <param name="workbook">IWorkbook instance. </param>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="rowIndex1">Row Index1 starts from zero</param>
    ///<param name="colIndex1">Column Index1 starts from zero</param>
    ///<param name="rowIndex2">Row Index2 starts from zero</param>
    ///<param name="colIndex2">Column Index2 starts from zero</param>
    internal static void UnMergeCells(IWorkbook workbook, int sheetIndex, int rowIndex1, int colIndex1, int rowIndex2, int colIndex2)
    {
        string RetVal = string.Empty;
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            worksheet.Cells[rowIndex1, colIndex1, rowIndex2, colIndex2].MergeCells = false;

        }
        catch (Exception ex)
        {
             throw ex;
        }

    }

    internal static void MergeCells(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            worksheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex].Merge();
        }
        catch (Exception)
        {
        }
    }


    #region "-- Font --"

    /// <summary>
    /// Returns font of a cell
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="worksheetIndex">Work sheet index</param>
    ///<param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    internal static IFont GetCellFont(IWorkbook workbook, int worksheetIndex, int rowIndex, int colIndex)
    {
        IFont RetVal;
        SpreadsheetGear.IRange Range;
        try
        {
            // Get a range and set font
            Range = workbook.Worksheets[worksheetIndex].Cells[rowIndex, colIndex];
            RetVal = Range.Font;
        }
        catch (Exception ex)
        {
            //                 throw ex;
            RetVal = null;
        }
        return RetVal;
    }

    /// <summary>
    /// Returns font of a range. 
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="worksheetIndex">worksheet index</param>
    /// <param name="startColIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColInex"></param>
    internal static IFont GetRangeFont(IWorkbook workbook, int worksheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        IFont RetVal;
        SpreadsheetGear.IRange Range;
        try
        {
            // get range and range font
            Range = workbook.Worksheets[worksheetIndex].Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            RetVal = Range.Font;
        }
        catch (Exception ex)
        {
            //                 throw ex;
            RetVal = null;
        }
        return RetVal;
    }


    #endregion

    #region "-- Working with Columns --"

    /// <summary>
    /// To set column width to auto fit.
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="sheetIndex">Sheet index starts from zero.</param>
    /// <param name="columnIndex">column index starts from zero</param>
    internal static void AutoFitColumn(IWorkbook workbook, int sheetIndex, int columnIndex)
    {
        try
        {

            workbook.Worksheets[sheetIndex].Cells[columnIndex.ToString() + ":" + columnIndex.ToString()].Columns.AutoFit();

        }
        catch (Exception ex)
        {
            //                 throw ex;                
        }
    }

    /// <summary>
    /// To set columns width to auto fit.
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="sheetIndex">Sheet index starts from zero.</param>
    ///<param name="startRowIndex">Start row index starts from zero</param>
    ///<param name="startColIndex">Start column index starts from zero</param>
    ///<param name="endRowIndex">End row index starts from zero</param>
    ///<param name="endColIndex">End column index starts from zero</param>
    internal static void AutoFitColumns(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            //workbook.Worksheets[sheetIndex].Cells[startRowIndex,startColIndex,endRowIndex,endColIndex].Columns.AutoFit();
            workbook.Worksheets[sheetIndex].Range[startRowIndex, startColIndex, endRowIndex, endColIndex].EntireColumn.AutoFit();
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    #endregion

    #region "-- TextAlignment --"

    /// <summary>
    /// To set vertical alignment 
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="sheetIndex">Worksheet index starts from zero</param>
    /// <param name="startColIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColInex"></param>
    /// <param name="valign"></param>
    internal static void SetVerticalAlignment(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColInex, VAlign valign)
    {
        SpreadsheetGear.IRange Range;
        try
        {
            // Get a reference to a cell.
            Range = workbook.Worksheets[sheetIndex].Cells[startRowIndex, startColIndex, endRowIndex, endColInex];
            Range.VerticalAlignment = valign;
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    /// <summary>
    /// To set vertical alignment 
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="sheetIndex">Worksheet index starts from zero.</param>
    /// <param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    internal static void SetVerticalAlignment(IWorkbook workbook, int sheetIndex, int rowIndex, int colIndex, VAlign valign)
    {
        SpreadsheetGear.IRange Range;
        try
        {
            // Get a reference to a cell.
            Range = workbook.Worksheets[sheetIndex].Cells[rowIndex, colIndex];
            Range.VerticalAlignment = valign;
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    /// <summary>
    /// To set horizontal alignment 
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="sheetIndex">Worksheet index starts from zero</param>
    /// <param name="startColIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColInex"></param>
    /// <param name="halign"></param>
    internal static void SetHorizontalAlignment(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColInex, HAlign halign)
    {
        SpreadsheetGear.IRange Range;
        try
        {
            // Get a reference to a cell.
            Range = workbook.Worksheets[sheetIndex].Cells[startRowIndex, startColIndex, endRowIndex, endColInex];
            Range.HorizontalAlignment = halign;
        }
        catch (Exception ex)
        {
            //       throw ex;
        }
    }

    /// <summary>
    /// To set horizontal alignment 
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="sheetIndex">Worksheet index starts from zero.</param>
    /// <param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <param name="halign"></param>
    internal static void SetHorizontalAlignment(IWorkbook workbook, int sheetIndex, int rowIndex, int colIndex, HAlign halign)
    {
        SpreadsheetGear.IRange Range;
        try
        {
            // Get a reference to a cell.
            Range = workbook.Worksheets[sheetIndex].Cells[rowIndex, colIndex];
            Range.HorizontalAlignment = halign;
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    #endregion

    #region "-- Color --"

    /// <summary>
    /// To set background color of cell
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="cellAddress">Address of cell</param>
    /// <param name="cellColor">color</param>
    internal static void SetCellColor(IWorkbook workbook, string cellAddress, System.Drawing.Color cellColor)
    {
        SpreadsheetGear.IRange Cell;
        try
        {
            // remove sheet name from cell address 
            cellAddress = ExcelHelper.RemoveSheetNameFrmCellAddress(cellAddress, workbook.ActiveWorksheet.Name);

            // Get a reference to a cell.
            Cell = workbook.ActiveWorksheet.Cells[cellAddress];

            Cell.Interior.Color = cellColor;

            ExcelHelper.SetAllBorders(Cell.Range);

        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    /// <summary>
    /// To set background  and foreground color of a cell range in an active worksheet
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="startColIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColInex"></param>
    ///<param name="backColor">Background color </param>
    ///<param name="foreColor">foreground color</param>
    internal static void SetRangeColor(IWorkbook workbook, int startRowIndex, int startColIndex, int endRowIndex, int endColInex, System.Drawing.Color foreColor, System.Drawing.Color backColor)
    {
        SpreadsheetGear.IRange Range;
        try
        {

            // Get a reference to a cell.
            Range = workbook.ActiveWorksheet.Cells[startRowIndex, startColIndex, endRowIndex, endColInex];

            ExcelHelper.SetColor(Range, foreColor, backColor);
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    /// <summary>
    /// To set background color of a cell range in an active worksheet
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="startColIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColInex"></param>
    ///<param name="backColor">Background color </param>
    ///<param name="foreColor">foreground color</param>
    internal static void SetBackgroundColor(IWorkbook workbook, int startRowIndex, int startColIndex, int endRowIndex, int endColInex, System.Drawing.Color backColor)
    {
        SpreadsheetGear.IRange Range;
        try
        {

            // Get a reference to a cell.
            Range = workbook.ActiveWorksheet.Cells[startRowIndex, startColIndex, endRowIndex, endColInex];

            ExcelHelper.SetBackgroundColor(Range, backColor);
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }


    /// <summary>
    /// Edit the Current Color Palette of workbook
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="ColorPelatte">array containing new colors</param>
    internal static void SetWorkbookColorPalette(IWorkbook workbook, Color[] ColorPelatte)
    {

        try
        {
            for (int count = 0; count < ColorPelatte.Length; count++)
            {
                workbook.Colors[count] = ColorPelatte[count];
            }
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }


    /// <summary>
    /// To set background  and foreground color of a range. 
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="worksheetIndex">worksheet index</param>
    /// <param name="startColIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColInex"></param>
    ///<param name="backColor">Background color </param>
    ///<param name="foreColor">foreground color</param>
    internal static void SetRangeColor(IWorkbook workbook, int worksheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColInex, System.Drawing.Color foreColor, System.Drawing.Color backColor)
    {
        SpreadsheetGear.IRange Range;
        try
        {

            // Get a reference to a cell.
            Range = workbook.Worksheets[worksheetIndex].Cells[startRowIndex, startColIndex, endRowIndex, endColInex];
            ExcelHelper.SetColor(Range, foreColor, backColor);
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    /// <summary>
    /// To set background  and foreground color of a cell in an active worksheet
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    ///<param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    ///<param name="backColor">Background color </param>
    ///<param name="foreColor">foreground color</param>
    internal static void SetCellColor(IWorkbook workbook, int rowIndex, int colIndex, System.Drawing.Color foreColor, System.Drawing.Color backColor)
    {
        SpreadsheetGear.IRange Range;
        try
        {
            // Get a reference to a cell.
            Range = workbook.ActiveWorksheet.Cells[rowIndex, colIndex];
            ExcelHelper.SetColor(Range, foreColor, backColor);
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    /// <summary>
    /// To set background  and foreground color of a cell
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="worksheetIndex">Work sheet index</param>
    ///<param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    ///<param name="backColor">Background color </param>
    ///<param name="foreColor">foreground color</param>
    internal static void SetCellColor(IWorkbook workbook, int worksheetIndex, int rowIndex, int colIndex, System.Drawing.Color foreColor, System.Drawing.Color backColor)
    {
        SpreadsheetGear.IRange Range;
        try
        {

            // Get a reference to a cell.
            Range = workbook.Worksheets[worksheetIndex].Cells[rowIndex, colIndex];
            ExcelHelper.SetColor(Range, foreColor, backColor);
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    /// <summary>
    /// To set foreground color of a cell
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="worksheetIndex">Work sheet index</param>
    ///<param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    ///<param name="foreColor">foreground color</param>
    internal static void SetCellForegroundColor(IWorkbook workbook, int worksheetIndex, int rowIndex, int colIndex, System.Drawing.Color foreColor)
    {
        SpreadsheetGear.IRange Range;
        try
        {

            // Get a reference to a cell.
            Range = workbook.Worksheets[worksheetIndex].Cells[rowIndex, colIndex];
            Range.Font.Color = foreColor;
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    /// <summary>
    /// To set color of cell 
    /// </summary>
    ///<param name="range"></param>
    ///<param name="backColor"></param>
    ///<param name="foreColor"></param>
    internal static void SetColor(IRange range, System.Drawing.Color foreColor, System.Drawing.Color backColor)
    {
        try
        {
            range.Interior.Color = backColor;
            range.Font.Color = foreColor;
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    /// <summary>
    /// To set color of cell 
    /// </summary>
    ///<param name="range"></param>
    ///<param name="backColor"></param>
    ///<param name="foreColor"></param>
    internal static void SetBackgroundColor(IRange range, System.Drawing.Color backColor)
    {
        try
        {
            range.Interior.Color = backColor;
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    #endregion

    #region "-- Border --"

    /// <summary>
    /// To set all border.
    /// </summary>
    /// <param name="range">Instance of IRange</param>
    internal static void SetAllBorders(IRange range)
    {
        try
        {

            // Get a reference to all the borders of the range.
            SpreadsheetGear.IBorders borders = range.Borders;

            // Set the border Linestyle, Weight, and Color.
            borders.LineStyle = SpreadsheetGear.LineStyle.Continuous;
            //borders.Weight = SpreadsheetGear.BorderWeight;
            borders.Color = System.Drawing.Color.LightGray;
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    /// <summary>
    /// To set border of a range.
    /// </summary>
    /// <param name="workbook"> Instance of workbook</param>
    /// <param name="worksheetIndex">worksheet index </param>
    /// <param name="startRowIndex">starting row index</param>
    /// <param name="startColIndex">starting column index</param>
    /// <param name="endRowIndex">last row index</param>
    /// <param name="endColIndex">last column index</param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    internal static void SetRangeBorders(IWorkbook workbook, int worksheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor)
    {
        try
        {
            //get range
            IRange Range = workbook.Worksheets[worksheetIndex].Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];

            // set border's properties
            ExcelHelper.SetBorders(Range, lineStyle, borderWeight, borderColor);
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    /// <summary>
    /// To set range border of a active worksheet.
    /// </summary>
    /// <param name="workbook"> Instance of workbook</param>
    /// <param name="startRowIndex">starting row index</param>
    /// <param name="startColIndex">starting column index</param>
    /// <param name="endRowIndex">last row index</param>
    /// <param name="endColIndex">last column index</param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    internal static void SetRangeBorders(IWorkbook workbook, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor)
    {
        try
        {
            //get range
            IRange Range = workbook.ActiveWorksheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];

            // set border's properties
            ExcelHelper.SetBorders(Range, lineStyle, borderWeight, borderColor);
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    /// <summary>
    /// To set cell's border .
    /// </summary>
    /// <param name="workbook">Instance of workbook</param>
    ///<param name="rowIndex"></param>        
    /// <param name="colIndex"></param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    internal static void SetCellBorder(IWorkbook workbook, int rowIndex, int colIndex, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor)
    {
        SpreadsheetGear.IRange Cell;
        try
        {
            // Get a reference to a cell.
            Cell = workbook.ActiveWorksheet.Cells[rowIndex, colIndex];

            // set border's properties
            ExcelHelper.SetBorders(Cell.Range, lineStyle, borderWeight, borderColor);
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    /// <summary>
    /// To set cell's border .
    /// </summary>
    /// <param name="workbook">Instance of workbook</param>
    /// <param name="worksheetIndex">worksheet index</param>
    ///<param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    internal static void SetCellBorder(IWorkbook workbook, int worksheetIndex, int rowIndex, int colIndex, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor)
    {
        SpreadsheetGear.IRange Cell;
        try
        {
            // Get a reference to a cell.
            Cell = workbook.Worksheets[worksheetIndex].Cells[rowIndex, colIndex];

            // set border's properties
            ExcelHelper.SetBorders(Cell.Range, lineStyle, borderWeight, borderColor);
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    internal static void SetRangeBorder(IWorkbook workbook, int worksheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        SpreadsheetGear.IRange Cell;
        int rowIndex = 0;
        int colIndex = 0;
        try
        {
            //// Get a reference to a cell.
            //Cell = workbook.Worksheets[worksheetIndex].Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];

            //// set border's properties
            //ExcelHelper.SetBorders(Cell.Range, LineStyle.Continuous, BorderWeight.Medium, Color.Black);
            for (rowIndex = startRowIndex; rowIndex <= endRowIndex; rowIndex++)
            {
                for (colIndex = startColIndex; colIndex <= endColIndex; colIndex++)
                {
                    Cell = workbook.Worksheets[worksheetIndex].Cells[rowIndex, colIndex];
                    ExcelHelper.SetBorders(Cell.Range, LineStyle.Continuous, BorderWeight.Thin, Color.Black);
                }
            }

        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    /// <summary>
    /// To set border.
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="cellAddress">Address of cell</param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    internal static void SetBorders(IRange range, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor)
    {
        SpreadsheetGear.IBorder TopBorder;
        SpreadsheetGear.IBorder BottomBorder;
        SpreadsheetGear.IBorder LeftBorder;
        SpreadsheetGear.IBorder RightBorder;

        try
        {
            // Get a reference to all the borders of the range.

            //top border
            TopBorder = range.Borders[BordersIndex.EdgeTop];
            // Set the  top border Linestyle, Weight, and Color.
            ExcelHelper.SetBorderStyle(ref TopBorder, lineStyle, borderWeight, borderColor);


            //bottom border
            BottomBorder = range.Borders[BordersIndex.EdgeBottom];
            // Set the  bottom border Linestyle, Weight, and Color.
            ExcelHelper.SetBorderStyle(ref BottomBorder, lineStyle, borderWeight, borderColor);


            //left border
            LeftBorder = range.Borders[BordersIndex.EdgeLeft];
            // Set the  LeftBorder Linestyle, Weight, and Color.
            ExcelHelper.SetBorderStyle(ref LeftBorder, lineStyle, borderWeight, borderColor);

            //Right border
            RightBorder = range.Borders[BordersIndex.EdgeRight];
            // Set the  right border Linestyle, Weight, and Color.
            ExcelHelper.SetBorderStyle(ref RightBorder, lineStyle, borderWeight, borderColor);

        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    /// <summary>
    /// To set border style
    /// </summary>
    /// <param name="border"></param>
    /// <param name="lineStyle"></param>
    /// <param name="borderWeight"></param>
    /// <param name="borderColor"></param>
    internal static void SetBorderStyle(ref SpreadsheetGear.IBorder border, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor)
    {
        border.LineStyle = lineStyle;
        border.Weight = borderWeight;
        border.Color = borderColor;
    }

    /// <summary>
    /// To set cell's border .
    /// </summary>
    /// <param name="workbook">Instance of workbook</param>
    /// <param name="worksheetIndex">worksheet index</param>
    ///<param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    /// <param name="borderIndex">Index of border like top, left ,..etc</param>
    internal static void SetCellBorder(IWorkbook workbook, int worksheetIndex, int rowIndex, int colIndex, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor, BordersIndex borderIndex)
    {
        SpreadsheetGear.IRange Cell;
        SpreadsheetGear.IBorder CellBorder;
        try
        {
            // Get a reference to a cell.
            Cell = workbook.Worksheets[worksheetIndex].Cells[rowIndex, colIndex];

            // Get specified border
            CellBorder = Cell.Borders[borderIndex];

            // set border's properties
            ExcelHelper.SetBorderStyle(ref CellBorder, lineStyle, borderWeight, borderColor);
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    /// <summary>
    /// To set border of a range.
    /// </summary>
    /// <param name="workbook"> Instance of workbook</param>
    /// <param name="worksheetIndex">worksheet index </param>
    /// <param name="startRowIndex">starting row index</param>
    /// <param name="startColIndex">starting column index</param>
    /// <param name="endRowIndex">last row index</param>
    /// <param name="endColIndex">last column index</param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    /// <param name="borderIndex">Index like top , left, botton, etc</param>
    internal static void SetRangeBorders(IWorkbook workbook, int worksheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor, BordersIndex borderIndex)
    {
        SpreadsheetGear.IBorder RangeBorder;
        try
        {
            //get range
            IRange Range = workbook.Worksheets[worksheetIndex].Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];

            RangeBorder = Range.Borders[borderIndex];
            // set border's properties
            ExcelHelper.SetBorderStyle(ref RangeBorder, lineStyle, borderWeight, borderColor);
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    #endregion

    #region "-- Working With Worksheet --"


    /// <summary>
    /// To activate worksheet 
    /// </summary>
    /// <param name="workbook"></param>
    internal static void ActivateWorksheet(IWorkbook workbook, int sheetIndex)
    {

        try
        {
            if (workbook.Worksheets[sheetIndex] != null)
            {
                workbook.WindowInfo.ActiveWorksheet = workbook.Worksheets[sheetIndex];
            }
        }
        catch (Exception)
        {

        }
    }


    /// <summary>
    /// Pastes the image in the selected column.
    /// </summary>
    /// <param name="workbook">Active workbook</param>
    /// <param name="sheetIndex">Active sheet index</param>
    /// <param name="pictureData">Picture in byte[] form</param>
    /// <param name="columnPosition">Column index</param>
    /// <param name="rowPosition">Row index</param>
    internal static void PasteImage(IWorkbook workbook, int sheetIndex, byte[] pictureData, int columnPosition, int rowPosition)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet Worksheet = workbook.Worksheets[sheetIndex];

            if (Worksheet != null)
            {
                SpreadsheetGear.IWorksheetWindowInfo windowInfo = Worksheet.WindowInfo;
                double left = windowInfo.ColumnToPoints(columnPosition);
                double top = windowInfo.RowToPoints(rowPosition);
                Worksheet.Shapes.AddPicture(pictureData, left, top, 10, 10);
            }
        }
        catch (Exception ex)
        {
            // throw ex;
        }

    }

    internal static void PasteImage(IWorkbook workbook, int sheetIndex, string filePath, int columnPosition, int rowPosition, double width, double height)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet Worksheet = workbook.Worksheets[sheetIndex];

            if (Worksheet != null)
            {
                SpreadsheetGear.IWorksheetWindowInfo windowInfo = Worksheet.WindowInfo;
                double left = windowInfo.ColumnToPoints(columnPosition);
                double top = windowInfo.RowToPoints(rowPosition);
                Worksheet.Shapes.AddPicture(filePath, left, top, width, height);
            }
        }
        catch (Exception ex)
        {
            // throw ex;
        }

    }



    /// <summary>
    /// returns the column width of provided range
    /// </summary>
    /// <param name="workbook">Active workbook</param>
    /// <param name="sheetIndex">Active sheet</param>
    /// <param name="startRowIndex">start row of range</param>
    /// <param name="startColumnIndex">start column of range</param>
    /// <param name="endRowIndex">end row of range</param>
    /// <param name="endColumnIndex">end column of range</param>
    /// <returns>Column width</returns>
    internal static double GetColumnWidth(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex)
    {
        double ColumnWidth = 0.0;
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet Worksheet = workbook.Worksheets[sheetIndex];

            if (Worksheet != null)
            {
                ColumnWidth = Worksheet.Cells[startRowIndex, startColumnIndex, endRowIndex, endColumnIndex].ColumnWidth;
            }
        }
        catch (Exception ex)
        {
            ColumnWidth = 0.0;
        }
        return ColumnWidth;
    }


    /// <summary>
    /// Sets the column width of the provided range
    /// </summary>
    /// <param name="workbook">Active workbook</param>
    /// <param name="sheetIndex">Active sheet</param>
    /// <param name="width">width of column</param>
    /// <param name="startRowIndex">start row of range</param>
    /// <param name="startColumnIndex">start column of range</param>
    /// <param name="endRowIndex">end row of range</param>
    /// <param name="endColumnIndex">end column of range</param>
    internal static void SetColumnWidth(IWorkbook workbook, int sheetIndex, double width, int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet Worksheet = workbook.Worksheets[sheetIndex];

            if (Worksheet != null)
            {
                SpreadsheetGear.IRange Rng = Worksheet.Range[startRowIndex, startColumnIndex, endRowIndex, endColumnIndex];
                Rng.ColumnWidth = width;
            }
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    internal static void SetRowHeight(IWorkbook workbook, int sheetIndex, double height, int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet Worksheet = workbook.Worksheets[sheetIndex];

            if (Worksheet != null)
            {
                SpreadsheetGear.IRange Rng = Worksheet.Range[startRowIndex, startColumnIndex, endRowIndex, endColumnIndex];
                Rng.RowHeight = height;
            }
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }


    /// <summary>
    /// To show and hide worksheet's grid line .
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="sheetIndex"></param>
    /// <param name="display"></param>
    internal static void ShowWorkSheetGridLine(IWorkbook workbook, int sheetIndex, bool display)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet Worksheet = workbook.Worksheets[sheetIndex];

            if (Worksheet != null)
            {
                Worksheet.WindowInfo.DisplayGridlines = display;
            }
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    /// <summary>
    /// Renames the active worksheet
    /// </summary>
    /// <param name="workbook">Active workbook</param>
    /// <param name="oldSheetName">old worksheet name</param>
    /// <param name="newSheetName">new worksheet name</param>
    internal static void RenameWorkSheet(IWorkbook workbook, string oldSheetName, string newSheetName)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet Worksheet = workbook.Worksheets[oldSheetName];

            if (Worksheet != null)
            {
                Worksheet.Name = newSheetName;
                workbook.Save();
            }
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="workbook"></param>
    ///// <param name="sheetIndex"></param>
    ///// <param name="rowIndex"></param>
    ///// <param name="colIndex"></param>
    ///// <param name="ColumnWidth"></param>
    //internal static void SetColumnWidth(IWorkbook workbook, int sheetIndex,int startRowIndex,int startColIndex,int endRowIndex,int endColIndex,double ColumnWidth)
    //{
    //    try
    //    {																									
    //        // Get a worksheet reference.
    //        SpreadsheetGear.IWorksheet Worksheet = workbook.Worksheets[sheetIndex];

    //        if (Worksheet != null)
    //        {
    //            Worksheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex].Width = ColumnWidth;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        //                 throw ex;
    //    }

    //}




    internal static void RenameWorkSheet(IWorkbook workbook, int sheetIndex, string sheetName)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet Worksheet = workbook.Worksheets[sheetIndex];
            if (Worksheet != null)
            {
                Worksheet.Name = sheetName;
                //workbook.Save();					
            }
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    /// <summary>
    /// To create worksheet.
    /// </summary>
    /// <param name="workbook">IWorkBook instance</param>
    /// <param name="sheetName">Sheet name in which data has to be inserted. </param>
    internal static void CreateWorksheet(SpreadsheetGear.IWorkbook workbook, string sheetName)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetName];

            // if worksheet not found then create it.
            if (worksheet == null)
            {

                workbook.Worksheets.Add();
                worksheet = workbook.Worksheets[workbook.Worksheets.Count - 1];
            }

            // Set the worksheet name.
            worksheet.Name = sheetName;

        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="sheetName"></param>
    internal static void WrapText(SpreadsheetGear.IWorkbook workbook, int sheetIndex, int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex, bool wrapText)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            if (worksheet != null)
            {
                worksheet.Cells[startRowIndex, startColumnIndex, endRowIndex, endColumnIndex].WrapText = wrapText;
            }
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="sheetName"></param>
    internal static void PasteRows(SpreadsheetGear.IWorkbook workbook, int sheetIndex, int startRowIndex, int pasteRowIndex, int endRowIndex)
    {
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            if (worksheet != null)
            {
                IRange Range = worksheet.Range[startRowIndex, 0, endRowIndex, worksheet.Cells.ColumnCount - 1].Range;
                worksheet.Cells.Copy(Range);
                ExcelHelper.ActivateCell(workbook, sheetIndex, 0, pasteRowIndex);
                //worksheet.WindowInfo.
                SpreadsheetGear.Windows.Forms.WorkbookView w1 = null;
                w1.ActiveWorkbook = workbook;
                w1.ActiveWorksheet = worksheet;
                w1.Paste();
            }
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
    }


    #endregion

    #region "-- Save File --"

    /// <summary>
    /// To save opened workbook
    /// </summary>
    /// <param name="workbook"></param>
    internal static void Save(IWorkbook workbook)
    {
        try
        {
            workbook.Save();
        }
        catch (Exception ex)
        {

            //                 throw ex;
        }

    }

    /// <summary>
    /// To save Workbook 
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="fileName">Excel filename</param>
    internal static void SaveAs(IWorkbook workbook, string fileName)
    {
        try
        {
            workbook.SaveAs(fileName, FileFormat.XLS97);
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }

    }

    /// <summary>
    /// To protect contents of worksheet
    /// </summary>
    /// <param name="workbook">IWorkbook</param>
    /// <param name="sheetIndex">The zero based index of worksheet</param>
    internal static void ProtectSheetWithoutPassword(IWorkbook workbook, string sheetIndex)
    {
        try
        {
            workbook.Worksheets[sheetIndex].ProtectContents = true;
        }
        catch (Exception ex)
        {

            //                 throw ex;
        }
    }

    #endregion

    internal static void SetWorksheetHyperlink(IWorkbook workbook, int sheetIndex, string worksheetName, int rowindex, int columnindex)
    {
        try
        {
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];

            IRange Range = worksheet.Cells[rowindex, columnindex];

            worksheet.Hyperlinks.Add(Range, null, "'" + worksheetName + "'!A1", worksheetName, worksheetName);
        }
        catch (Exception)
        {
        }
    }

    internal static void SetWorksheetHyperlink(IWorkbook workbook, int sheetIndex, string worksheetName, string cellAddress, int rowindex, int columnindex, string text)
    {
        try
        {
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];

            IRange Range = worksheet.Cells[rowindex, columnindex];

            worksheet.Hyperlinks.Add(Range, null, "'" + worksheetName + "'!" + cellAddress, "", "");
            //worksheet.Hyperlinks.Add(Range, @"http://www.google.com", null, "", "");
        }
        catch (Exception)
        {
        }
    }

    internal static void SetCellHyperlink(IWorkbook workbook, int sheetIndex, int rowindex, int columnindex, string link, string text)
    {
        try
        {
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];

            IRange Range = worksheet.Cells[rowindex, columnindex];

            worksheet.Hyperlinks.Add(Range, link, null, "", text);
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// To get Sheet Index
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="sheetName">SheetName</param>
    /// <returns>Returns -1 if not found</returns>
    internal static int GetSheetIndex(IWorkbook workbook, string sheetName)
    {
        int RetVal = -1;

        try
        {
            RetVal = workbook.Worksheets[sheetName].Index;
        }
        catch (Exception ex)
        {
            //                 throw ex;
        }
        return RetVal;
    }

    /// <summary>
    /// Hide the workseet from the workbook
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="worksheetName"></param>
    internal static void HideWorksheet(IWorkbook workbook, string worksheetName)
    {
        try
        {
            workbook.Worksheets[worksheetName].Visible = SpreadsheetGear.SheetVisibility.Hidden;
        }
        catch (Exception)
        {
        }
    }

    internal static string GetRange(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        string Retval = string.Empty;
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            Retval = worksheet.Range.Columns[startRowIndex, startColIndex, endRowIndex, endColIndex].GetAddress(false, false, ReferenceStyle.A1, false, null);
        }
        catch (Exception)
        {
        }
        return Retval;
    }

    internal static int GetWorksheetRowCount(IWorkbook workbook, int sheetIndex)
    {
        int Retval = 0;
        try
        {
            // Get a worksheet reference.
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];
            Retval = worksheet.Range.Rows.RowCount;
        }
        catch (Exception)
        {
        }
        return Retval;
    }

    internal static void FreezePanes(IWorkbook workbook, int sheetIndex, int scrollColumn, int scrollRows, int splitColumns, int splitRows)
    {
        // Get a worksheet reference.
        SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];

        worksheet.WindowInfo.ScrollColumn = scrollColumn;
        worksheet.WindowInfo.SplitColumns = splitColumns;

        worksheet.WindowInfo.ScrollRow = scrollRows;
        worksheet.WindowInfo.SplitRows = splitRows;

        worksheet.WindowInfo.FreezePanes = true;
    }

    internal static void SetGrouping(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        workbook.ActiveWorksheet.Outline.SummaryColumn = SummaryColumn.Left;
        workbook.ActiveWorksheet.Outline.SummaryRow = SummaryRow.Above;
        workbook.ActiveWorksheet.Range.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex].EntireRow.Group();
        workbook.ActiveWorksheet.Outline.ShowLevels(1, 0);
    }

    internal static void SetRangeBold(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        workbook.ActiveWorksheet.Range.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex].Font.Bold = true;
    }

    internal static void SetRangeItalics(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        workbook.ActiveWorksheet.Range.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex].Font.Italic = true;
    }

    internal static void SetRangeUnderline(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        workbook.ActiveWorksheet.Range.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex].Font.Underline = UnderlineStyle.Single;        
    }

    internal static void HideRangeCells(IWorkbook workbook, int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[sheetIndex];       
        worksheet.Range.Columns[startRowIndex, startColIndex, endRowIndex, endColIndex].Columns.Hidden = true;        
    }
}

