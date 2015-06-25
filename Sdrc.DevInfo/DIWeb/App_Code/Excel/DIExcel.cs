using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Drawing;
using SpreadsheetGear;
using System.Globalization;


/// <summary>
/// To work with Excel file
/// </summary>
public class DIExcel
{
    #region "-- Private --"


    #region "-- Variables --"

    private string ExcelFileName = string.Empty;
    private SpreadsheetGear.IWorkbook Workbook = null;

    #endregion


    #endregion

    #region "-- Public --"

    #region "-- Variables / Properties --"

    private int _ActiveSheetIndex;
    /// <summary>
    /// Sets the zero base index to activate the sheet.
    /// </summary>
    public int ActiveSheetIndex
    {
        set
        {
            this._ActiveSheetIndex = value;
            ExcelHelper.ActivateWorksheet(this.Workbook, this._ActiveSheetIndex);
        }
    }


    private int _AvailableWorksheetsCount;
    /// <summary>
    /// Returns count of available worksheets.
    /// </summary>
    public int AvailableWorksheetsCount
    {
        get
        {
            try
            {
                if (this.Workbook != null)
                {
                    this._AvailableWorksheetsCount = this.Workbook.Worksheets.Count;
                }
                else
                {
                    this._AvailableWorksheetsCount = 0;
                }
            }
            catch (Exception)
            {
                this._AvailableWorksheetsCount = 0;
            }

            return this._AvailableWorksheetsCount;
        }
    }

    /// <summary>
    /// Activates the specified sheet
    /// </summary>
    /// <param name="sheetIndex">Index of sheet to be Activated.</param>
    public void ActivateSheet(int sheetIndex)
    {
        try
        {
            this.Workbook.WindowInfo.ActiveSheet = this.Workbook.Worksheets[sheetIndex];
        }
        catch (Exception)
        {


        }
    }

    #endregion

    #region "-- New/Dispose --"

    /// <summary>
    /// To create object of DIExcel	.
    /// </summary>
    /// <param name="excelFileName">Excel file name with path</param>
    public DIExcel(string excelFileName)
    {


        this.ExcelFileName = excelFileName;

        this.Workbook = ExcelHelper.OpenExcelFile(this.ExcelFileName);
    }

    /// <summary>
    /// To create object of DIExcel. Use "SaveAs(..)" method to save excel file.
    /// </summary>
    public DIExcel()
    {
        //Get workbook set from workbook factory.
        IWorkbookSet WorkbookSet = Factory.GetWorkbookSet(CultureInfo.CurrentCulture);

        // Create a new empty workbook in the workbook set. 
        this.Workbook = WorkbookSet.Workbooks.Add();


    }



    /// <summary>
    /// To create object of DIExcel
    /// </summary>
    /// <param name="excelFileName">Excel file name with path</param>
    /// <param name="culture">Specify the Cultrue info</param>
    public DIExcel(string excelFileName, CultureInfo culture)
    {
        this.ExcelFileName = excelFileName;

        //Get workbook from workbook factory.
        this.Workbook = SpreadsheetGear.Factory.GetWorkbook(this.ExcelFileName, culture);
    }

    /// <summary>
    /// To create object of DIExcel. Use "SaveAs(..)" method to save excel file.
    /// </summary>
    /// <param name="culture">Specify the Cultrue info</param>
    public DIExcel(CultureInfo culture)
    {
        //Get workbook set from workbook factory.
        IWorkbookSet WorkbookSet = Factory.GetWorkbookSet(culture);

        // Create a new empty workbook in the workbook set. 
        this.Workbook = WorkbookSet.Workbooks.Add();
    }

    #endregion

    #region "-- Methods --"

    /// <summary>
    /// Returns the number of used rows
    /// </summary>
    /// <param name="sheetIndex">Sheet index starts from zero</param>
    /// <returns></returns>
    public int GetUsedRowsCount(int sheetIndex)
    {
        return ExcelHelper.GetUsedRowsCount(this.Workbook, sheetIndex);
    }

    public IRange GetUsedRange(int sheetIndex)
    {
        return ExcelHelper.GetUsedRange(this.Workbook, sheetIndex);
    }

    /// <summary>
    /// To save all changes into stream
    /// </summary>
    public void SaveToStream(System.IO.Stream stream)
    {
        this.Workbook.SaveToStream(stream, FileFormat.XLS97);
    }


    /// <summary>
    /// To insert new worksheet.
    /// </summary>
    /// <param name="sheetName">Sheet name </param>
    public void InsertWorkSheet(string sheetName)
    {
        try
        {
            ExcelHelper.InsertWorkSheet(this.Workbook, sheetName);
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Append new worksheet at end.
    /// </summary>
    /// <param name="sheetName">Sheet name </param>
    public void AppendWorkSheet(string sheetName)
    {
        try
        {
            ExcelHelper.AppendWorkSheet(this.Workbook, sheetName);
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Delete the worksheet
    /// </summary>
    /// <param name="sheetIndex">SheetIndex </param>
    public void DeleteWorkSheet(int sheetIndex)
    {
        try
        {
            ExcelHelper.DeleteWorkSheet(this.Workbook, sheetIndex);
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Returns sheet name
    /// </summary>
    /// <param name="sheetIndex">sheetIndex</param>
    /// <returns>Returns sheet name</returns>
    public string GetSheetName(int sheetIndex)
    {
        return ExcelHelper.GetSheetName(this.Workbook, sheetIndex);
    }

    /// <summary>
    /// To delete a row at particular position.
    /// </summary>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="position">row position</param>
    public void DeleteRowAt(int sheetIndex, int position)
    {
        try
        {
            ExcelHelper.DeleteRowAt(this.Workbook, sheetIndex, position);
        }
        catch (Exception ex)
        {
           throw ex;
        }
    }

    /// <summary>
    /// To delete a Column at particular position.
    /// </summary>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="position">row position</param>
    public void DeleteColumnAt(int sheetIndex, string position)
    {
        try
        {
            ExcelHelper.DeleteColumnAt(this.Workbook, sheetIndex, position);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// To insert a row at particular position.
    /// </summary>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="position">row position</param>
    public void InsertRowAt(int sheetIndex, int position)
    {
        try
        {
            ExcelHelper.InsertRowAt(this.Workbook, sheetIndex, position);
        }
        catch (Exception ex)
        {
           throw ex;
        }
    }

    /// <summary>
    /// move Column at particular position.
    /// </summary>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="position">row position</param>
    public void MoveColumnTo(int sheetIndex, string SourcePosition, string DestinationPosition)
    {
        try
        {
            ExcelHelper.MoveColumnTo(this.Workbook, sheetIndex, SourcePosition, DestinationPosition);
        }
        catch (Exception ex)
        {
           throw ex;
        }
    }

    /// <summary>
    /// move Row at particular position.
    /// </summary>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="position">row position</param>
    public void MoveRowTo(int sheetIndex, string SourcePosition, string DestinationPosition)
    {
        try
        {
            ExcelHelper.MoveRowTo(this.Workbook, sheetIndex, SourcePosition, DestinationPosition);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Get Column Header 
    /// </summary>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="position">row position</param>
    public string GetColumnHeader(int sheetIndex, int RowPosition, int ColumnPosition)
    {
        string RetVal = string.Empty;
        try
        {
            RetVal = ExcelHelper.GetColumnHeader(this.Workbook, sheetIndex, RowPosition, ColumnPosition);
            return RetVal;
        }
        catch (Exception ex)
        {
           throw ex;
            return RetVal;
        }
    }

    public void SetCellFormatType(int sheetIndex, int rowIndex, int colIndex, NumberFormatType formatType)
    {
        try
        {
            ExcelHelper.SetCellFormatType(this.Workbook, sheetIndex, rowIndex, colIndex, formatType);
        }
        catch (Exception ex)
        {
           throw ex;
        }
    }

    public void SetRangeFormatType(int sheetIndex, int startRowIndex, int startColumnIndex, int endRowIndex, int endColIndex, NumberFormatType formatType)
    {
        try
        {
            ExcelHelper.SetRangeFormatType(this.Workbook, sheetIndex, startRowIndex, startColumnIndex, endRowIndex, endColIndex, formatType);
        }
        catch (Exception ex)
        {
           throw ex;
        }
    }

    /// <summary>
    /// Changes the column format type (eg: text, general, Date, number) for specified column address.
    /// </summary>
    /// <param name="sheetIndex">sheet index number</param>
    /// <param name="columnAddress">column address. foreg: A:A , B:B</param>
    /// <param name="formatType">format type.</param>
    public void SetColumnFormatType(string columnAddress, int sheetIndex, NumberFormatType formatType)
    {
        try
        {
            ExcelHelper.SetColumnFormatType(this.Workbook, sheetIndex, columnAddress, formatType);
        }
        catch (Exception ex)
        {
           throw ex;
        }
    }

    public void SetFormatNumbericValue(int sheetIndex, string columnAddress, bool isDecimal, int decimalPlaces, bool isGroupingRequired)
    {
        try
        {
            ExcelHelper.SetFormatNumbericValue(this.Workbook, sheetIndex, columnAddress, isDecimal, decimalPlaces, isGroupingRequired);
        }
        catch (Exception)
        {
        }
    }


    #region "-- Working with Datatable --"

    /// <summary>
    /// Returns Data Table. Converts given worksheet into data table.  
    /// </summary>
    /// <param name="sheetName">Worksheet name</param>
    /// <returns> DataTable</returns>            
    public DataTable GetDataTableFromSheet(string sheetName)
    {
        return ExcelHelper.GetDataTableFromSheet(this.Workbook, this.ExcelFileName, sheetName);
    }

    /// <summary>
    /// To load Data Table into worksheet.
    /// </summary>
    /// <param name="worksheetDataTable">Data table</param>
    /// <param name="sheetName">Sheet name in which data has to be inserted. </param>
    public void LoadDataTableIntoSheet(DataTable worksheetDataTable, string sheetName)
    {
        ExcelHelper.LoadDataTableIntoSheet(this.Workbook, worksheetDataTable, sheetName);
    }

    /// <summary>
    /// To load Data Table into worksheet.
    /// </summary>
    /// <param name="startPosition">Starting position of datatable like $A$10</param>
    /// <param name="worksheetDataTable">Data table</param>
    /// <param name="sheetName">Sheet name in which data has to be inserted. </param>
    /// <param name="SupressColumnHeader">Set true to hide column header and false to display column header</param>
    /// <param name="firstClearAllCells">set to true, if you want to clear all cells before pasting datatable into worksheet </param>
    public void LoadDataTableIntoSheet(string startPosition, DataTable worksheetDataTable, string sheetName, bool SupressColumnHeader, bool firstClearAllCells)
    {
        try
        {
            ExcelHelper.LoadDataTableIntoSheet(this.Workbook, startPosition, worksheetDataTable, sheetName, SupressColumnHeader, firstClearAllCells);
        }
        catch (Exception ex)
        {
            //ExceptionFacade.ThrowException(ex);
        }
    }

    /// <summary>
    /// To load Data Table into worksheet.
    /// </summary>
    /// <param name="rowIndex">zero based row index</param>
    /// <param name="colIndex">zero based column index</param>
    /// <param name="worksheetDataTable">Data table</param>
    /// <param name="sheetIndex">Zero based sheet index</param>
    /// <param name="SupressColumnHeader">Set to true, to hide column header and false to display column header</param>
    public void LoadDataTableIntoSheet(int rowIndex, int colIndex, DataTable worksheetDataTable, int sheetIndex, bool SupressColumnHeader)
    {
        try
        {
            ExcelHelper.LoadDataTableIntoSheet(this.Workbook, rowIndex, colIndex, worksheetDataTable, sheetIndex, SupressColumnHeader);
        }
        catch (Exception ex)
        {
           throw ex;
        }
    }


    #endregion

    /// <summary>
    /// To set array values into worksheet.
    /// </summary>
    /// <param name="sheetName">Sheet name in which data has to be inserted</param>
    /// <param name="startRowIndex">Start row index starts from zero</param>
    /// <param name="startColIndex">Start column index starts from zero</param>
    /// <param name="endRowIndex">End row index starts from zero</param>
    /// <param name="endColIndex">End column index starts from zero</param>
    /// <param name="valueArray"></param>
    public void SetArrayValuesIntoSheet(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, object[,] valueArray)
    {
        try
        {

            ExcelHelper.SetArrayValuesIntoSheet(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex, valueArray);
        }
        catch (Exception ex)
        {
            //ExceptionFacade.ThrowException(ex);
        }
    }

    public void MergeCells(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            ExcelHelper.MergeCells(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
        }
        catch (Exception Ex)
        {
            //throw;
        }
    }

    /// <summary>
    /// Set the grouping.
    /// </summary>
    /// <param name="sheetIndex"></param>
    /// <param name="rowCount"></param>
    /// <param name="columnLevel"></param>
    public void SetGrouping(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            ExcelHelper.SetGrouping(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
        }
        catch (Exception)
        {
        }
    }

    public void SetRangeBold(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            ExcelHelper.SetRangeBold(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
        }
        catch (Exception)
        {
        }
    }

    public void SetRangeItalics(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            ExcelHelper.SetRangeItalics(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
        }
        catch (Exception)
        {
        }
    }

    public void SetRangeUnderline(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            ExcelHelper.SetRangeUnderline(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
        }
        catch (Exception)
        {
        }
    }

    #region "-- Working With Worksheet --"


    /// <summary>
    /// Pastes the image in the selected column.
    /// </summary>
    /// <param name="sheetIndex">Active sheet index</param>
    /// <param name="pictureData">Picture in byte[] form</param>
    /// <param name="columnPosition">Column index</param>
    /// <param name="rowPosition">Row index</param>
    public void PasteImage(int sheetIndex, byte[] pictureData, int columnPosition, int rowPosition)
    {
        try
        {
            ExcelHelper.PasteImage(this.Workbook, sheetIndex, pictureData, columnPosition, rowPosition);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    public void PasteImage(int sheetIndex, string filePath, int columnPosition, int rowPosition, double width, double height)
    {
        try
        {
            ExcelHelper.PasteImage(this.Workbook, sheetIndex, filePath, columnPosition, rowPosition, width, height);
        }
        catch (Exception)
        {
            //               throw ex;
        }
    }

    /// <summary>
    /// Returns cell height .
    /// </summary>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="rowIndex">Row index starts from zero</param>
    ///<param name="colIndex">Column index starts from zero</param>
    /// <returns> double</returns>
    public double GetCellHeight(int sheetIndex, int colIndex, int rowIndex)
    {
        double RetVal = 0;
        try
        {
            RetVal = ExcelHelper.GetCellHeight(this.Workbook, sheetIndex, colIndex, rowIndex);
        }
        catch (Exception ex)
        {
            RetVal = 0;
        }
        return RetVal;
    }

    public void SetFont(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, string fontname, int fontsize)
    {
        try
        {
            ExcelHelper.SetFont(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex, fontname, fontsize);
        }
        catch (Exception)
        {
        }
    }


    public void SelectCell(int sheetIndex, int colIndex, int rowIndex)
    {
        try
        {
            ExcelHelper.SelectCell(this.Workbook, sheetIndex, colIndex, rowIndex);
        }
        catch (Exception ex)
        {
        }
    }

    public void ActivateCell(int sheetIndex, int colIndex, int rowIndex)
    {
        try
        {
            ExcelHelper.ActivateCell(this.Workbook, sheetIndex, colIndex, rowIndex);
        }
        catch (Exception ex)
        {
        }
    }


    /// <summary>
    /// returns the column width of provided range
    /// </summary>		
    /// <param name="sheetIndex">Active sheet</param>
    /// <param name="startRowIndex">start row of range</param>
    /// <param name="startColumnIndex">start column of range</param>
    /// <param name="endRowIndex">end row of range</param>
    /// <param name="endColumnIndex">end column of range</param>
    /// <returns>Column width</returns>
    public double GetColumnWidth(int sheetIndex, int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex)
    {
        double ColumnWidth = 0.0;
        try
        {
            ColumnWidth = ExcelHelper.GetColumnWidth(this.Workbook, sheetIndex, startRowIndex, startColumnIndex, endRowIndex, endColumnIndex);
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
    /// <param name="sheetIndex">Active sheet</param>
    /// <param name="width">width of column</param>
    /// <param name="startRowIndex">start row of range</param>
    /// <param name="startColumnIndex">start column of range</param>
    /// <param name="endRowIndex">end row of range</param>
    /// <param name="endColumnIndex">end column of range</param>
    public void SetColumnWidth(int sheetIndex, double width, int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex)
    {
        try
        {
            ExcelHelper.SetColumnWidth(this.Workbook, sheetIndex, width, startRowIndex, startColumnIndex, endRowIndex, endColumnIndex);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }

    }

    public void SetRowHeight(int sheetIndex, double height, int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex)
    {
        try
        {
            ExcelHelper.SetRowHeight(this.Workbook, sheetIndex, height, startRowIndex, startColumnIndex, endRowIndex, endColumnIndex);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }



    /// <summary>
    /// To show and hide worksheet's grid line .
    /// </summary>
    /// <param name="sheetIndex"></param>
    /// <param name="display"></param>
    public void ShowWorkSheetGridLine(int sheetIndex, bool display)
    {
        try
        {
            ExcelHelper.ShowWorkSheetGridLine(this.Workbook, sheetIndex, display);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sheetIndex"></param>
    /// <param name="display"></param>
    public void WrapText(int sheetIndex, int startRowIndex, int startColumnIndex, int endRowIndex, int endColumnIndex, bool wrapText)
    {
        try
        {
            ExcelHelper.WrapText(this.Workbook, sheetIndex, startRowIndex, startColumnIndex, endRowIndex, endColumnIndex, wrapText);
        }
        catch (Exception ex)
        {
            // throw ex;
        }

    }

    public void PasteRows(int sheetIndex, int startRowIndex, int pasteRowIndex, int endRowIndex)
    {
        ExcelHelper.PasteRows(Workbook, sheetIndex, startRowIndex, pasteRowIndex, endRowIndex);
    }


    /// <summary>
    /// Renames the active worksheet
    /// </summary>		
    /// <param name="oldSheetName">old worksheet name</param>
    /// <param name="newSheetName">new worksheet name</param>
    public void RenameWorkSheet(string oldSheetName, string newSheetName)
    {
        try
        {
            ExcelHelper.RenameWorkSheet(this.Workbook, oldSheetName, newSheetName);
        }
        catch (Exception ex)
        {
            //ExceptionFacade.ThrowException(ex);
        }

    }

    /// <summary>
    /// To rename worksheet name
    /// </summary>
    /// <param name="sheetIndex">Sheet index starts from zero.</param>
    /// <param name="sheetName">new sheet name</param>
    public void RenameWorkSheet(int sheetIndex, string sheetName)
    {
        try
        {
            ExcelHelper.RenameWorkSheet(this.Workbook, sheetIndex, sheetName);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }

    }

    /// <summary>
    /// To create worksheet.
    /// </summary>
    /// <param name="sheetName">Sheet name in which data has to be inserted. </param>
    public void CreateWorksheet(string sheetName)
    {
        try
        {
            ExcelHelper.CreateWorksheet(this.Workbook, sheetName);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    #endregion

    #region "-- Working with Columns --"

    /// <summary>
    /// To Convert column type into text
    /// </summary>
    /// <param name="sheetIndex">Zero base sheet index</param>
    /// <param name="colIndex">Zero based column index</param>
    public IWorksheet GetWorksheet(int sheetIndex)
    {
        IWorksheet RetVal = null;
        try
        {
            RetVal = this.Workbook.Worksheets[sheetIndex];

        }
        catch (Exception ex)
        {
           throw ex;
        }

        return RetVal;
    }

    /// <summary>
    /// Get the worksheet count
    /// </summary>
    public int GetWorksheetCount()
    {
        int RetVal = -1;
        try
        {
            RetVal = this.Workbook.Worksheets.Count;

        }
        catch (Exception ex)
        {
           throw ex;
        }
        return RetVal;
    }


    /// <summary>
    /// To set column width to auto fit.
    /// </summary>
    /// <param name="sheetIndex">Sheet index starts from zero.</param>
    /// <param name="columnIndex">column index starts from zero</param>
    public void AutoFitColumn(int sheetIndex, int columnIndex)
    {
        try
        {
            ExcelHelper.AutoFitColumn(this.Workbook, sheetIndex, columnIndex);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    /// <summary>
    /// To set columns width to auto fit.
    /// </summary>
    /// <param name="sheetIndex">Sheet index starts from zero.</param>
    ///<param name="startRowIndex">Start row index starts from zero</param>
    ///<param name="startColIndex">Start column index starts from zero</param>
    ///<param name="endRowIndex">End row index starts from zero</param>
    ///<param name="endColIndex">End column index starts from zero</param>
    public void AutoFitColumns(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            ExcelHelper.AutoFitColumns(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    #endregion

    #region "-- Cell Value --"

    /// <summary>
    /// Returns cell value as string
    /// </summary>
    /// <param name="sheetIndex">sheet index starts from zero</param>
    /// <param name="cellAddress">address of cell</param>
    /// <returns>string</returns>
    public string GetCellValue(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        return ExcelHelper.GetCellValue(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
    }

    /// <summary>
    /// Returns cell value as string
    /// </summary>
    /// <param name="sheetIndex">sheet index starts from zero</param>
    /// <param name="cellAddress">address of cell</param>
    /// <returns>string</returns>
    public string GetCellValue(int sheetIndex, string cellAddress)
    {
        return ExcelHelper.GetCellValue(this.Workbook, sheetIndex, cellAddress);
    }


    /// <summary>
    /// To set value of a cell.
    /// </summary>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    /// <param name="cellAddress">Address of cell like A4 .</param>
    /// <param name="value">Value</param>
    /// <returns> string</returns>
    public void SetCellValue(int sheetIndex, string cellAddress, object value)
    {
        ExcelHelper.SetCellValue(this.Workbook, sheetIndex, cellAddress, value);
    }

    /// <summary>
    /// To set value of a cell.
    /// </summary>
    /// <param name="sheetIndex">SheetIndex starts from zero.</param>
    ///<param name="rowIndex">Row index starts from zero.</param>
    /// <param name="colIndex">Column index starts from zero.</param>
    /// <param name="value">Value</param>
    /// <returns> string</returns>
    public void SetCellValue(int sheetIndex, int rowIndex, int colIndex, object value)
    {
        try
        {
            ExcelHelper.SetCellValue(this.Workbook, sheetIndex, rowIndex, colIndex, value);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    #endregion

    #region "-- Comments --"

    /// <summary>
    /// To insert comment.
    /// </summary>
    /// <param name="workbook">Instance of IWorkbook</param>
    /// <param name="cellAddress">Address of cell</param>
    /// <param name="comment">Comment string</param>
    /// <param name="clearOldComments">True/False. True to clear old comments</param>
    public void AddComment(string cellAddress, string comment, bool clearOldComments)
    {
        ExcelHelper.AddComment(this.Workbook, cellAddress, comment, clearOldComments);
    }

    /// <summary>
    /// To insert cell comment 
    /// </summary>
    /// <param name="sheetIndex">Worksheet index starts from zero</param>
    /// <param name="rowIndex">Row Index starts from zero</param>
    /// <param name="colIndex">Column Index starts from zero</param>
    /// <param name="comment">Comment string</param>
    /// <param name="clearOldComments">True/False. True to clear old comments</param>
    public void AddComment(int sheetIndex, int rowIndex, int colIndex, string comment, bool clearOldComments)
    {
        try
        {
            ExcelHelper.AddComment(this.Workbook, sheetIndex, rowIndex, colIndex, comment, clearOldComments);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    #endregion


    #region "-- TextAlignment --"

    /// <summary>
    /// To set vertical alignment 
    /// </summary>
    /// <param name="sheetIndex">Worksheet index starts from zero</param>
    /// <param name="startColIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColIndex"></param>
    /// <param name="valign"></param>
    public void SetVerticalAlignment(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, VAlign valign)
    {
        try
        {
            ExcelHelper.SetVerticalAlignment(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex, valign);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    /// <summary>
    /// To set vertical alignment 
    /// </summary>
    /// <param name="sheetIndex">Worksheet index starts from zero.</param>
    /// <param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    public void SetVerticalAlignment(int sheetIndex, int rowIndex, int colIndex, VAlign valign)
    {
        try
        {
            ExcelHelper.SetVerticalAlignment(this.Workbook, sheetIndex, rowIndex, colIndex, valign);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    /// <summary>
    /// To set horizontal alignment 
    /// </summary>
    /// <param name="sheetIndex">Worksheet index starts from zero</param>
    /// <param name="startColIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColIndex"></param>
    /// <param name="halign"></param>
    public void SetHorizontalAlignment(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, HAlign halign)
    {
        try
        {
            ExcelHelper.SetHorizontalAlignment(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex, halign);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    /// <summary>
    /// To set horizontal alignment 
    /// </summary>
    /// <param name="sheetIndex">Worksheet index starts from zero.</param>
    /// <param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <param name="halign"></param>
    public void SetHorizontalAlignment(int sheetIndex, int rowIndex, int colIndex, HAlign halign)
    {
        try
        {
            ExcelHelper.SetHorizontalAlignment(this.Workbook, sheetIndex, rowIndex, colIndex, halign);
        }
        catch (Exception ex)
        {
            //             throw ex;
        }
    }

    #endregion

    #region "-- Font --"

    /// <summary>
    /// Returns font of a cell
    /// </summary>
    /// <param name="worksheetIndex">Work sheet index</param>
    /// <param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    public IFont GetCellFont(int worksheetIndex, int rowIndex, int colIndex)
    {
        IFont RetVal;
        try
        {
            RetVal = ExcelHelper.GetCellFont(this.Workbook, worksheetIndex, rowIndex, colIndex);
        }
        catch (Exception ex)
        {
            //               throw ex;
            RetVal = null;
        }
        return RetVal;
    }

    /// <summary>
    /// Returns font of a range. 
    /// </summary>
    /// <param name="worksheetIndex">worksheet index</param>
    /// <param name="startColIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColIndex"></param>
    public IFont GetRangeFont(int worksheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        IFont RetVal;
        try
        {
            RetVal = ExcelHelper.GetRangeFont(this.Workbook, worksheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
        }
        catch (Exception ex)
        {
            //               throw ex;
            RetVal = null;
        }
        return RetVal;
    }

    /// <summary>
    /// Returns reference of ICharacters which can be used to set the format of characters of a cell.
    /// </summary>
    /// <param name="sheetIndex">Sheet index</param>
    /// <param name="colIndex">Zero based column index </param>
    /// <param name="rowIndex">Zero based row index</param>
    /// <param name="start">Zero based starting character</param>
    /// <param name="length">The number of characters</param>
    /// <returns></returns>
    public ICharacters GetCellCharacters(int sheetIndex, int colIndex, int rowIndex, int start, int length)
    {
        return ExcelHelper.GetCellCharacters(this.Workbook, sheetIndex, colIndex, rowIndex, start, length);
    }

    #endregion

    #region "-- Color --"

    /// <summary>
    /// To set background color of cell
    /// </summary>
    /// <param name="cellAddress">Address of cell</param>
    /// <param name="cellColor">color</param>

    public void SetCellColor(string cellAddress, System.Drawing.Color cellColor)
    {
        ExcelHelper.SetCellColor(this.Workbook, cellAddress, cellColor);
    }


    /// <summary>
    /// To set background  and foreground color of a cell range in an active worksheet
    /// </summary>
    /// <param name="startColIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColInex"></param>
    ///<param name="backColor">Background color </param>
    ///<param name="foreColor">foreground color</param>
    public void SetRangeColor(int startRowIndex, int startColIndex, int endRowIndex, int endColInex, System.Drawing.Color foreColor, System.Drawing.Color backColor)
    {
        try
        {
            ExcelHelper.SetRangeColor(this.Workbook, startRowIndex, startColIndex, endRowIndex, endColInex, foreColor, backColor);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    public void SetCellForegroundColor(int worksheetIndex, int rowIndex, int colIndex, System.Drawing.Color foreColor)
    {
        try
        {
            ExcelHelper.SetCellForegroundColor(Workbook, worksheetIndex, rowIndex, colIndex, foreColor);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    /// <summary>
    /// Edit the Current Color Palette of workbook
    /// </summary>
    /// <param name="ColorPelatte">array containing new colors</param>
    public void SetWorkbookColorPalette(Color[] ColorPelatte)
    {
        try
        {
            ExcelHelper.SetWorkbookColorPalette(this.Workbook, ColorPelatte);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }


    /// <summary>
    /// To set background  and foreground color of a range. 
    /// </summary>
    /// <param name="worksheetIndex">worksheet index</param>
    /// <param name="startColIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColInex"></param>
    ///<param name="backColor">Background color </param>
    ///<param name="foreColor">foreground color</param>
    public void SetRangeColor(int worksheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColInex, System.Drawing.Color foreColor, System.Drawing.Color backColor)
    {
        try
        {
            ExcelHelper.SetRangeColor(this.Workbook, worksheetIndex, startRowIndex, startColIndex, endRowIndex, endColInex, foreColor, backColor);
        }
        catch (Exception ex)
        {
            //               throw ex;
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
    public void SetBackgroundColor(int worksheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColInex, System.Drawing.Color backColor)
    {
        SpreadsheetGear.IRange Range;
        try
        {
            ExcelHelper.SetBackgroundColor(this.Workbook, startRowIndex, startColIndex, endRowIndex, endColInex, backColor);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }


    /// <summary>
    /// To set background  and foreground color of a cell in an active worksheet
    /// </summary>
    ///<param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    ///<param name="backColor">Background color </param>
    ///<param name="foreColor">foreground color</param>
    public void SetCellColor(int rowIndex, int colIndex, System.Drawing.Color foreColor, System.Drawing.Color backColor)
    {
        try
        {
            ExcelHelper.SetCellColor(this.Workbook, rowIndex, colIndex, foreColor, backColor);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    /// <summary>
    /// To set background  and foreground color of a cell
    /// </summary>
    /// <param name="worksheetIndex">Work sheet index</param>
    ///<param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// ///<param name="foreColor">foreground color</param>
    ///<param name="backColor">Background color </param>
    public void SetCellColor(int worksheetIndex, int rowIndex, int colIndex, System.Drawing.Color foreColor, System.Drawing.Color backColor)
    {
        try
        {
            ExcelHelper.SetCellColor(this.Workbook, worksheetIndex, rowIndex, colIndex, foreColor, backColor);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }
    }

    #endregion

    #region "-- Borders -- "

    /// <summary>
    /// To set cell's border .
    /// </summary>
    /// <param name="worksheetIndex">worksheet index</param>
    ///<param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    /// <param name="borderIndex">Index of border like top, left ,..etc</param>
    public void SetCellBorder(int worksheetIndex, int rowIndex, int colIndex, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor, BordersIndex borderIndex)
    {
        try
        {
            ExcelHelper.SetCellBorder(this.Workbook, worksheetIndex, rowIndex, colIndex, lineStyle, borderWeight, borderColor, borderIndex);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }

    }

    /// <summary>
    /// To set border of a range.
    /// </summary>
    /// <param name="worksheetIndex">worksheet index </param>
    /// <param name="startRowIndex">starting row index</param>
    /// <param name="startColIndex">starting column index</param>
    /// <param name="endRowIndex">last row index</param>
    /// <param name="endColIndex">last column index</param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    /// <param name="borderIndex">Index like top , left, botton, etc</param>
    public void SetRangeBorder(int worksheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor, BordersIndex borderIndex)
    {
        try
        {
            ExcelHelper.SetRangeBorders(this.Workbook, worksheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex, lineStyle, borderWeight, borderColor, borderIndex);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }

    }

    /// <summary>
    /// To set border of a range.
    /// </summary>
    /// <param name="worksheetIndex">worksheet index </param>
    /// <param name="startRowIndex">starting row index</param>
    /// <param name="startColIndex">starting column index</param>
    /// <param name="endRowIndex">last row index</param>
    /// <param name="endColIndex">last column index</param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    public void SetRangeBorders(int worksheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor)
    {
        try
        {
            ExcelHelper.SetRangeBorders(this.Workbook, worksheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex, lineStyle, borderWeight, borderColor);

        }
        catch (Exception ex)
        {
            //               throw ex;
        }

    }

    /// <summary>
    /// To set range border of a active worksheet.
    /// </summary>
    /// <param name="startRowIndex">starting row index</param>
    /// <param name="startColIndex">starting column index</param>
    /// <param name="endRowIndex">last row index</param>
    /// <param name="endColIndex">last column index</param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    public void SetRangeBorders(int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor)
    {
        try
        {
            ExcelHelper.SetRangeBorders(this.Workbook, startRowIndex, startColIndex, endRowIndex, endColIndex, lineStyle, borderWeight, borderColor);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }

    }

    /// <summary>
    /// To set cell's border .
    /// </summary>
    ///<param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    public void SetCellBorder(int rowIndex, int colIndex, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor)
    {
        try
        {
            ExcelHelper.SetCellBorder(this.Workbook, rowIndex, colIndex, lineStyle, borderWeight, borderColor);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }

    }

    /// <summary>
    /// To set cell's border .
    /// </summary>
    /// <param name="worksheetIndex">worksheet index</param>
    ///<param name="rowIndex"></param>
    /// <param name="colIndex"></param>
    /// <param name="lineStyle">line style for border</param>
    /// <param name="borderWeight">weight like thick ,thin ,etc.</param>
    /// <param name="borderColor">color for border</param>
    public void SetCellBorder(int worksheetIndex, int rowIndex, int colIndex, LineStyle lineStyle, BorderWeight borderWeight, System.Drawing.Color borderColor)
    {
        try
        {
            ExcelHelper.SetCellBorder(this.Workbook, worksheetIndex, rowIndex, colIndex, lineStyle, borderWeight, borderColor);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }

    }

    /// <summary>
    /// To set range border .
    /// </summary>
    /// <param name="worksheetIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="startColIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColIndex"></param>
    public void SetRangeBorder(int worksheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            ExcelHelper.SetRangeBorder(this.Workbook, worksheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
        }
        catch (Exception ex)
        {
            //               throw ex;
        }

    }

    #endregion

    public void SetWorksheetHyperlink(int sheetIndex, string worksheetName, int rowindex, int columnindex)
    {
        try
        {
            ExcelHelper.SetWorksheetHyperlink(this.Workbook, sheetIndex, worksheetName, rowindex, columnindex);
        }
        catch (Exception)
        {
        }
    }

    public void SetWorksheetHyperlink(int sheetIndex, string worksheetName, string cellAddress, int rowindex, int columnindex, string text)
    {
        try
        {
            ExcelHelper.SetWorksheetHyperlink(this.Workbook, sheetIndex, worksheetName, cellAddress, rowindex, columnindex, text);
        }
        catch (Exception)
        {
        }
    }

    public void SetCellHyperlink(int sheetIndex, int rowindex, int columnindex, string link, string text)
    {
        try
        {
            ExcelHelper.SetCellHyperlink(this.Workbook, sheetIndex, rowindex, columnindex, link, text);
        }
        catch (Exception)
        {
        }
    }

    public void SetReadOnlyRange(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            ExcelHelper.SetReadOnlyRange(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
        }
        catch (Exception)
        {
        }
    }


    public void MakeDropDownList(int sheetIndex, int RowIndex, int ColIndex, string cellAddress)
    {
        try
        {
            ExcelHelper.MakeDropDownList(this.Workbook, sheetIndex, RowIndex, ColIndex, cellAddress);
        }
        catch (Exception)
        {
        }
    }

    public void SetValueByDropDownSelection(int sheetIndex, int rowIndex, int colIndex, string dropDownAddress, string sourceAddress)
    {
        try
        {
            ExcelHelper.SetValueByDropDownSelection(this.Workbook, sheetIndex, rowIndex, colIndex, dropDownAddress, sourceAddress);
        }
        catch (Exception)
        {
        }
    }

    

    /// <summary>
    /// Remove the range validation
    /// </summary>
    /// <param name="sheetIndex"></param>
    /// <param name="startRowIndex"></param>
    /// <param name="startColIndex"></param>
    /// <param name="endRowIndex"></param>
    /// <param name="endColIndex"></param>
    public void RemoveValidation(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        try
        {
            ExcelHelper.RemoveValidation(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// Set the validation list for the status column
    /// </summary>
    /// <param name="sheetIndex">Sheet Index</param>
    /// <param name="startRowIndex">Start Row Index</param>
    /// <param name="startColIndex">Start Column Index</param>
    /// <param name="endRowIndex">End Row Index</param>
    /// <param name="endColIndex">End Column Index</param>
    /// <param name="list">Comma seprated status list</param>
    public void SetValidationList(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex, string list)
    {
        try
        {
            ExcelHelper.SetValidationList(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex, list);
        }
        catch (Exception)
        {
        }
    }

    /// <summary>
    /// To get Sheet Index
    /// </summary>
    /// <param name="sheetName">SheetName</param>
    /// <returns>Returns -1 if not found</returns>
    public int GetSheetIndex(string sheetName)
    {
        return ExcelHelper.GetSheetIndex(this.Workbook, sheetName);
    }

    /// <summary>
    /// To save all changes
    /// </summary>
    public void Save()
    {

        ExcelHelper.Save(this.Workbook);
    }

    /// <summary>
    /// To save Workbook. 
    /// </summary>
    /// <param name="fileName">Excel filename</param>
    public void SaveAs(string fileName)
    {
        try
        {
            ExcelHelper.SaveAs(this.Workbook, fileName);
            this.ExcelFileName = fileName;
        }
        catch (Exception ex)
        {
            //ExceptionFacade.ThrowException(ex);
        }
    }

    /// <summary>
    /// Hide the workseet from the workbook
    /// </summary>
    /// <param name="worksheetName"></param>
    public void HideRangeCells(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        ExcelHelper.HideRangeCells(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
    }

    /// <summary>
    /// Hide the workseet from the workbook
    /// </summary>
    /// <param name="worksheetName"></param>
    public void HideWorksheet(string worksheetName)
    {
        ExcelHelper.HideWorksheet(this.Workbook, worksheetName);
    }

    public string GetRange(int sheetIndex, int startRowIndex, int startColIndex, int endRowIndex, int endColIndex)
    {
        string Retval = string.Empty;
        Retval = ExcelHelper.GetRange(this.Workbook, sheetIndex, startRowIndex, startColIndex, endRowIndex, endColIndex);
        return Retval;
    }

    public int GetWorksheetRowCount(int sheetIndex)
    {
        int RetVal = 0;
        RetVal = ExcelHelper.GetWorksheetRowCount(this.Workbook, sheetIndex);
        return RetVal;
    }

    public void SetFormatNumbericValue(int sheetIndex, string columnAddress, string formattedValue)
    {
        try
        {
            ExcelHelper.SetFormatNumbericValue(this.Workbook, sheetIndex, columnAddress, formattedValue);
        }
        catch (Exception ex)
        {
           throw ex;
        }
    }

    /// <summary>
    /// Freeze the panes
    /// </summary>
    /// <param name="sheetIndex"></param>
    /// <param name="scrollColumn"></param>
    /// <param name="scrollRows"></param>
    public void FreezePanes(int sheetIndex, int scrollColumn, int scrollRows, int splitColumns, int splitRows)
    {
        ExcelHelper.FreezePanes(this.Workbook, sheetIndex, scrollColumn, scrollRows, splitColumns, splitRows);
    }


    /// <summary>
    /// To close opened excel file.
    /// </summary>
    public void Close()
    {
        this.Workbook.Close();
    }

    #endregion

    #endregion

    #region "--Static --"

    #region "-- Methods --"

    public static string Evaluate(string formulaText)
    {
        string RetVal = string.Empty;

        try
        {
            // Create a new empty workbook and get the first sheet.
            SpreadsheetGear.IWorkbook workbook =
                SpreadsheetGear.Factory.GetWorkbook();
            SpreadsheetGear.IWorksheet worksheet = workbook.Worksheets[0];

            // Evaluate the input formula.
            //object result = worksheet.EvaluateValue(formulaText);
            worksheet.Cells[1, 1].Formula = formulaText;
            RetVal = worksheet.Cells[1, 1].Value.ToString();

            if (RetVal == "-2146826281")
            {
                RetVal = "0";
            }
            if (RetVal == "Div0")
            {
                RetVal = "0";
            }
        }
        catch (Exception)
        {
            RetVal = "0";
        }

        return RetVal;
    }

    public string GetSheetColumnNameByIndex(int Index)
    {
        string RetVal = string.Empty;
        switch (Index)
        {
            case 0:
                RetVal = "A";
                break;
            case 1:
                RetVal = "B";
                break;
            case 2:
                RetVal = "C";
                break;
            case 3:
                RetVal = "D";
                break;
            case 4:
                RetVal = "E";
                break;
            case 5:
                RetVal = "F";
                break;
            case 6:
                RetVal = "G";
                break;
            case 7:
                RetVal = "H";
                break;
            case 8:
                RetVal = "I";
                break;
            case 9:
                RetVal = "J";
                break;
            case 10:
                RetVal = "K";
                break;
            case 11:
                RetVal = "L";
                break;
            case 12:
                RetVal = "M";
                break;
            case 13:
                RetVal = "N";
                break;
            case 14:
                RetVal = "O";
                break;
            case 15:
                RetVal = "P";
                break;
            case 16:
                RetVal = "Q";
                break;
            case 17:
                RetVal = "R";
                break;
            case 18:
                RetVal = "S";
                break;
            case 19:
                RetVal = "T";
                break;
            case 20:
                RetVal = "U";
                break;
            case 21:
                RetVal = "V";
                break;
            case 22:
                RetVal = "W";
                break;
            case 23:
                RetVal = "X";
                break;
            case 24:
                RetVal = "Y";
                break;
            case 25:
                RetVal = "Z";               
                break;
            case 26:
                RetVal = "AA";
                break;
            case 27:
                RetVal = "AB";
                break;
            case 28:
                RetVal = "AC";
                break;
            case 29:
                RetVal = "AD";
                break;
            case 30:
                RetVal = "AE";
                break;
            case 31:
                RetVal = "AF";
                break;
            case 32:
                RetVal = "AG";
                break;
            case 33:
                RetVal = "AH";
                break;
            case 34:
                RetVal = "AI";
                break;
            case 35:
                RetVal = "AJ";
                break;
            case 36:
                RetVal = "AK";
                break;
            case 37:
                RetVal = "AL";
                break;
            case 38:
                RetVal = "AM";
                break;
            case 39:
                RetVal = "AN";
                break;
            case 40:
                RetVal = "AO";
                break;
            case 41:
                RetVal = "AP";
                break;
            case 42:
                RetVal = "AQ";
                break;
            case 43:
                RetVal = "AR";
                break;
            case 44:
                RetVal = "AS";
                break;
            case 45:
                RetVal = "AT";              
                break;
            case 46:
                RetVal = "AU";
                break;
            case 47:
                RetVal = "AV";
                break;
            case 48:
                RetVal = "AW";
                break;
            case 49:
                RetVal = "AX";
                break;
            case 50:
                RetVal = "AY";
                break;
            case 51:
                RetVal = "AZ";
                break;
            default:
                break;
        }
        return RetVal;
    }

    #endregion

    #endregion
}

