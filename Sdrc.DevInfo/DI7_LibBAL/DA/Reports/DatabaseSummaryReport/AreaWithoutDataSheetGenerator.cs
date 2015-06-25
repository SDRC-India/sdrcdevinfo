using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Create Area Without Data Sheet of Summary Report
    /// </summary>
    internal class AreaWithoutDataSheetGenerator:SheetGenerator
    {

        
        #region "-- private --"

        #region "-- Methods --"
        
        /// <summary>
        /// Get DataTable for IUS Missing DataValue 
        /// </summary>
        /// <returns></returns>
        private DataTable GetAreasWithoutDataTable()
        {
            DataTable RetVal = this.CreateAreaWithoutDataValueTable();

            // -- Get Surces witout DataValues Table
            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Area.GetAreasWithoutData()).DefaultView;
            // -- Sort by AreaID
            Table.Sort = Area.AreaID + " Asc";

            foreach (DataRowView RowView in Table)
            {
                RetVal.ImportRow(RowView.Row);
            }

            this.RenameAreaWithoutDataValueTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        /// Create Area TAble for Missing DataValue
        /// </summary>
        /// <returns></returns>
        private DataTable CreateAreaWithoutDataValueTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrementSeed = 1;
            RetVal.Columns.Add(Area.AreaID);
            RetVal.Columns.Add(Area.AreaName);
            RetVal.Columns.Add(Area_Level.AreaLevel);

            return RetVal;
        }

        /// <summary>
        /// Set Columns Name as Language based 
        /// </summary>
        /// <param name="table"></param>
        private void RenameAreaWithoutDataValueTable(ref DataTable table)
        {
            table.Columns[Area.AreaID].ColumnName = this.ColumnHeader[DSRColumnsHeader.AREAID];
            table.Columns[Area.AreaName].ColumnName = this.ColumnHeader[DSRColumnsHeader.AREANAME];
            table.Columns[Area_Level.AreaLevel].ColumnName = this.ColumnHeader[DSRColumnsHeader.LEVEL];

        }

        #endregion

        #endregion

        #region "-- Internal --"
        
        /// <summary>
        /// Create Sources without DataValue Sheet
        /// </summary>
        /// <param name="excelFile"></param>
        internal override void GenerateSheet(ref DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper.DIExcel excelFile)
        {
            int sheetNo = this.CreateSheet(ref excelFile, DILanguage.GetLanguageString("AREAS_WITHOUT_DATA"));
            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, DILanguage.GetLanguageString("AREAS_WITHOUT_DATA"));
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            // -- Get Area Data TAble.
            Table = this.GetAreasWithoutDataTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.Area.AreaDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.Area.AreaDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.Area.AreaDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.Area.AreaWithoutDataLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.OthersColumnWidth, Constants.Sheet.Area.AreaDetailsRowIndex, Constants.Sheet.Area.AreaIDColIndex, LastRow, Constants.Sheet.Area.AreaIDColIndex);
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.Area.AreaDetailsRowIndex, Constants.Sheet.Area.AreaNameColIndex, LastRow, Constants.Sheet.Area.AreaNameColIndex);

            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.HeaderRowIndex, Constants.Sheet.SummaryReport.AreaColValueIndex, LastRow, Constants.Sheet.SummaryReport.AreaColValueIndex, true);
           
              
        }

        #region "-- Methods --"

        #endregion

        #endregion

        
    }
}
