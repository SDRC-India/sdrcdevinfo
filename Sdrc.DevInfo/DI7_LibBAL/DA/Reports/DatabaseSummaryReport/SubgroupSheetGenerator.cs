using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Used To Create Subgroup Sheet
    /// </summary>
    internal class SubgroupSheetGenerator   :   SheetGenerator 
    {

        #region "-- private --"
       
        #region "-- Methods --"

        /// <summary>
        /// Careate Subgroup Table
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable CreateSubgroupValTable()
        {
            DataTable RetVal = new DataTable();
            // -- Add Required Columns
            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER").ToString()].AutoIncrementSeed = 1;
            RetVal.Columns.Add(SubgroupVals.SubgroupVal);
            RetVal.Columns.Add(SubgroupVals.SubgroupValGlobal);

            return RetVal;
        }

        /// <summary>
        /// Get Subgroup Table
        /// </summary>
        /// <returns></returns>
        private DataTable GetSubGroupTable()
        {
            DataTable RetVal = this.CreateSubgroupValTable();
            // -- Fill Subgroup TAble 
            DataView Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(DatabaseSummaryReportGenerator.DBQueries.SubgroupVals.GetSubgroupVals(FilterFieldType.None, string.Empty)).DefaultView;
            Table.Sort = SubgroupVals.SubgroupVal + " Asc";

            foreach (DataRowView row in Table)
            {
                string GlobalVal = string.Empty;
                DataRow Temp = RetVal.NewRow();

                Temp.BeginEdit();
                // Set Global VAlue to Yes or No
                if (!string.IsNullOrEmpty(row[SubgroupVals.SubgroupValGlobal].ToString()))
                {
                    // -- IF Global value is True Change it TO Yes else No. 
                    if (row[SubgroupVals.SubgroupValGlobal].ToString().ToUpper() == "TRUE")
                    { GlobalVal = "Yes"; }
                    else
                    { GlobalVal = "No"; }
                }
                else
                { GlobalVal = "No"; }

                Temp[SubgroupVals.SubgroupVal] = row[SubgroupVals.SubgroupVal];
                Temp[SubgroupVals.SubgroupValGlobal] = GlobalVal;
                Temp.EndEdit();
                // -- Add Row Into Table
                RetVal.Rows.Add(Temp);

            }
            //-- Rename Table
            this.RenameSubGroupTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        /// Rename SubGroupTAble
        /// </summary>
        /// <param name="table"></param>
        private void RenameSubGroupTable(ref DataTable table)
        {
            table.Columns[SubgroupVals.SubgroupVal].ColumnName = base.ColumnHeader[DSRColumnsHeader.SUBGROUP];
            table.Columns[SubgroupVals.SubgroupValGlobal].ColumnName = base.ColumnHeader[DSRColumnsHeader.GLOBAL];
        }

        #endregion

        #endregion

        #region "-- Internal--"

        #region "-- Methods --"

        /// <summary>
        /// Create SubGroup Sheet
        /// </summary>
        /// <param name="excelFile"></param>
        internal override void GenerateSheet(ref DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper.DIExcel excelFile)
        {
            int SheetNo = this.CreateSheet(ref excelFile, this.ColumnHeader[DSRColumnsHeader.SUBGROUP]);
            DataTable Table = null;

            // -- sheet content 
            excelFile.SetCellValue(SheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, ColumnHeader[DSRColumnsHeader.SUBGROUP]);
            excelFile.GetCellFont(SheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            excelFile.SetCellValue(SheetNo, Constants.Sheet.Subgroup.SubGroupDetailsRowIndex, Constants.Sheet.SummaryReport.SubgroupColValueIndex, ColumnHeader[DSRColumnsHeader.SUBGROUP]);
            excelFile.SetCellValue(SheetNo, Constants.Sheet.Subgroup.SubGroupDetailsRowIndex, Constants.Sheet.SummaryReport.SubgroupColValueIndex, ColumnHeader[DSRColumnsHeader.GLOBAL]);

            Table = this.GetSubGroupTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.Subgroup.SubGroupDetailsRowIndex, Constants.HeaderColIndex, Table, SheetNo, false);

            int LastRow = Constants.Sheet.Subgroup.SubGroupDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, SheetNo, Constants.Sheet.Subgroup.SubGroupDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.Subgroup.SubGroupLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(SheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.Subgroup.SubGroupDetailsRowIndex, Constants.Sheet.Subgroup.SubGroupNameColIndex, LastRow, Constants.Sheet.Subgroup.SubGroupNameColIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(SheetNo, Constants.Sheet.Subgroup.SubGroupDetailsRowIndex, Constants.Sheet.Subgroup.SubGroupNameColIndex, LastRow, Constants.Sheet.Subgroup.SubGroupNameColIndex, true);
           
  
        }

        #endregion

        #endregion

       
    }
}
