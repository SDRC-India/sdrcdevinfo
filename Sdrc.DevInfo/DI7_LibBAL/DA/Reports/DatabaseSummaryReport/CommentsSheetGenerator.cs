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
    /// Used To Generate Comments Sheet of Summary Report
    /// </summary>
    internal class CommentsSheetGenerator:SheetGenerator
    {

        #region "-- private --"
       
        #region "-- Methods --"
        
        /// <summary>
        /// Get Comments DataValues DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetCommentsTables()
        {
            // -- Get Table Structure
            DataTable RetVal = this.CreateCommentsTable();

            // -- Get Duplicate Values Data Table
            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.IUS.GetAllIUSForComments()).DefaultView;//DatabaseSummaryReportGenerator.DBQueries.Footnote.GetFootnote(FilterFieldType.None,""));

            // -- Sort By Indicator, Unit, Subgroup
            Table.Sort = Indicator.IndicatorName + " ASC," + Unit.UnitName + " ASC," + SubgroupVals.SubgroupVal + " ASC";
            
            int Counter = 0;
            foreach (DataRowView RowView in Table)
            {
                // -- Check If Records Exceeds from Max rows of Excel Sheet
                if (Counter > RangeCheckReport.RangeCheckCustomizationInfo.MAX_EXCEL_ROWS)
                    break;
                Counter += 1;
                // -- Import Row TO Comments DataTable
                RetVal.ImportRow(RowView.Row);
                // -- Get Comments
                DataTable TempTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.Notes.GetNotes(RowView[Data.DataNId].ToString(), string.Empty, string.Empty, string.Empty, CheckedStatus.All, FieldSelection.Heavy));
                for (int i = 0; i < TempTable.Rows.Count; i++)
                {
                    if (i > 0)       // -- For more than one comments add New row with Comments
                    {
                        DataRow Row = RetVal.NewRow();
                        Row[Notes_Profile.ProfileName] = TempTable.Rows[i][Notes_Profile.ProfileName];
                        Row[Notes_Classification.ClassificationName] = TempTable.Rows[i][Notes_Classification.ClassificationName];
                        Row[Notes.Note] = TempTable.Rows[i][Notes.Note];
                        RetVal.Rows.Add(Row);
                    }
                    else         // -- For First Comments add Comments in same row of last imported data.
                    {
                        RetVal.Rows[RetVal.Rows.Count - 1][Notes_Profile.ProfileName] = TempTable.Rows[i][Notes_Profile.ProfileName];
                        RetVal.Rows[RetVal.Rows.Count - 1][Notes_Classification.ClassificationName] = TempTable.Rows[i][Notes_Classification.ClassificationName];
                        RetVal.Rows[RetVal.Rows.Count - 1][Notes.Note] = TempTable.Rows[i][Notes.Note];
                    }
                }
                // -- Insert A Blank Record After A Group of Comments
                DataRow BlankRow = RetVal.NewRow();
                RetVal.Rows.Add(BlankRow);
            }

            // -- Rename Columns
            this.RenameCommentsTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        ///Create Table for Comments
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable CreateCommentsTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns.Add(Indicator.IndicatorName);
            RetVal.Columns.Add(Unit.UnitName, Type.GetType("System.String"));
            RetVal.Columns.Add(SubgroupVals.SubgroupVal);
            RetVal.Columns.Add(Area.AreaName);
            RetVal.Columns.Add(Timeperiods.TimePeriod);
            RetVal.Columns.Add(IndicatorClassifications.ICName);
            RetVal.Columns.Add(Data.DataValue);
            // -- For Comments
            RetVal.Columns.Add(Notes_Profile.ProfileName);
            RetVal.Columns.Add(Notes_Classification.ClassificationName);
            RetVal.Columns.Add(Notes.Note);
            return RetVal;
        }

        /// <summary>
        /// Rename Comments Table For Language Based Columns NAme
        /// </summary>
        /// <param name="table">DataTable For Comments</param>
        private void RenameCommentsTable(ref DataTable table)
        {
            table.Columns[Indicator.IndicatorName].ColumnName = this.ColumnHeader[DSRColumnsHeader.INDICATOR];
            table.Columns[Unit.UnitName].ColumnName = this.ColumnHeader[DSRColumnsHeader.UNIT];
            table.Columns[SubgroupVals.SubgroupVal].ColumnName = this.ColumnHeader[DSRColumnsHeader.SUBGROUP];
            table.Columns[Timeperiods.TimePeriod].ColumnName = this.ColumnHeader[DSRColumnsHeader.TIMEPERIOD];
            table.Columns[IndicatorClassifications.ICName].ColumnName = this.ColumnHeader[DSRColumnsHeader.SOURCE];
            table.Columns[Area.AreaName].ColumnName = this.ColumnHeader[DSRColumnsHeader.AREA];
            table.Columns[Data.DataValue].ColumnName = this.ColumnHeader[DSRColumnsHeader.DATA];
            // -- Column for Comments
            table.Columns[Notes_Profile.ProfileName].ColumnName = this.ColumnHeader[DSRColumnsHeader.AUTHOR];
            table.Columns[Notes_Classification.ClassificationName].ColumnName = this.ColumnHeader[DSRColumnsHeader.COMMENTS_TYPE];
            table.Columns[Notes.Note].ColumnName = this.ColumnHeader[DSRColumnsHeader.NOTES_NOTES];

        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        ///  Create Comments Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            DataTable Table = null;
            int sheetNo = this.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.NOTES_NOTES]);
            
            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, this.ColumnHeader[DSRColumnsHeader.NOTES_NOTES]);
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            // -- Get Comments Data TAble.
            Table = this.GetCommentsTables();
            int Counter = 0;
            foreach (DataRow row in Table.Rows)
            {
                //-- Exit IF current Record is For Comments.
                if (!string.IsNullOrEmpty(row[this.ColumnHeader[DSRColumnsHeader.INDICATOR]].ToString()))
                {
                    Counter += 1;
                    row[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()] = Counter;
                }
            }
            Table.AcceptChanges();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.Comments.CommentsDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.Comments.CommentsDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.Comments.CommentsDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.Comments.CommentsLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.Comments.CommentsDetailsRowIndex, Constants.Sheet.Comments.CommentsNameColIndex, LastRow, Constants.Sheet.Comments.CommentsNameColIndex);
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.Comments.CommentsDetailsRowIndex, Constants.Sheet.Comments.CommentsLastColIndex, LastRow, Constants.Sheet.Comments.CommentsLastColIndex);

            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.HeaderRowIndex, Constants.Sheet.Comments.CommentsLastColIndex, LastRow, Constants.Sheet.Comments.CommentsLastColIndex, true);
           
           

        }
       
        #endregion

        #endregion

        
    }
}
