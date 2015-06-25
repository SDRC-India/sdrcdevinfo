using System;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using SpreadsheetGear;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using System.Windows.Forms;
using System.Drawing;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Export
{
    /// <summary>
    /// Helps in exporting IUS Sheet 
    /// </summary>
    public class IUSSheetExporter
    {

        #region "-- private --"

        #region "-- Variables --"

        private DIConnection DBConnection;
        private DIQueries DBQueries;
        private List<string> SubgroupDimensionsList = new List<string>();
        #endregion

        #region "-- Methods --"

        private DataTable GetIUSTable()
        {
            DataTable RetVal;
            DataTable IUSTable = null;
            DataTable SGValDimensionTable = null;
            
            SGValDimensionTable = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupVals.GetSubgroupsValsWithDimensionNDimValues());

            // Get IUS Table
            IUSTable = this.DBConnection.ExecuteDataTable(this.DBQueries.IUS.GetIUS(FilterFieldType.None, string.Empty, FieldSelection.Light));
            
            // Sort Indicator,Unit,SubgroupVal
            IUSTable.DefaultView.Sort = Indicator.IndicatorName + " ASC," + Unit.UnitName + " ASC," + SubgroupVals.SubgroupVal + " Asc";
            IUSTable = IUSTable.DefaultView.ToTable();

            // Add Subgroup Dimension Column Into IUS Table
            this.AddSubgroupDimensionColumns(IUSTable, SGValDimensionTable);
            
            // Update Subgroup Dimension Values Into IUS Subgroup Columns
            this.UpdateSubgroupIntoIUSTable(IUSTable, SGValDimensionTable);

            // Merger S.No Column Into IUS TAble
            RetVal = this.MergeSnoColumns(IUSTable).Copy();

            // Remove Extra Columns from IUSTable
            this.RemoveExtraColumns(RetVal);

            // Set Column Caption
            this.SetColumnsCaption(RetVal);

            return RetVal;
        }

        private void UpdateSubgroupIntoIUSTable(DataTable iusTable, DataTable sgValDimensionTable)
        {
            for (int i = 0; i < iusTable.Rows.Count; i++)
            {
                // Set INdicatorGId-UnitGId-SubgroupValGId in GUID COlumn
                iusTable.Rows[i][Constants.IUSSheet.GUIDColumn] = Convert.ToString(iusTable.Rows[i][Indicator.IndicatorGId]) + Constants.IUSSheet.GIdSeprator + Convert.ToString(iusTable.Rows[i][Unit.UnitGId]) + Constants.IUSSheet.GIdSeprator + Convert.ToString(iusTable.Rows[i][SubgroupVals.SubgroupValGId]);
                // Set Subgroup Dimension Value for each Subgroup Dimension
                foreach (string SGType in this.SubgroupDimensionsList)
                {
                    DataRow[] IUSRows = sgValDimensionTable.Select(SubgroupVals.SubgroupValNId + "=" + Convert.ToString(iusTable.Rows[i][SubgroupVals.SubgroupValNId]) + " AND " + SubgroupTypes.SubgroupTypeName + "='" + DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(SGType) + "'");
                    // Set Subgroup if SGDimension Value exists for SubgroupDimension
                    if (IUSRows.Length > 0)
                    {
                        iusTable.Rows[i][SGType] = Convert.ToString(IUSRows[0][Subgroup.SubgroupName]);
                    }

                    iusTable.AcceptChanges();
                }
            }
        }

        private void SetColumnsCaption(DataTable iusTable)
        {
            // Set Column Captions
            iusTable.Columns[Indicator.IndicatorName].ColumnName = Constants.IUSSheet.Indicator;
            iusTable.Columns[Unit.UnitName].ColumnName = Constants.IUSSheet.Unit;
            iusTable.Columns[SubgroupVals.SubgroupVal].ColumnName = Constants.IUSSheet.Subgroup;

            iusTable.Columns[Constants.IUSSheet.GUIDColumn].SetOrdinal(1);
            iusTable.AcceptChanges();
        }

        private void RemoveExtraColumns(DataTable iusTable)
        {
            int ColIndex = 0;
            // Get IUSTable with Fields required            
            while (ColIndex < iusTable.Columns.Count)
            {
                // Delete Invalid Columns 
                if (iusTable.Columns[ColIndex].ColumnName == Constants.IUSSheet.SNo || iusTable.Columns[ColIndex].ColumnName == Constants.IUSSheet.GUIDColumn || iusTable.Columns[ColIndex].ColumnName == Indicator.IndicatorName || iusTable.Columns[ColIndex].ColumnName == Unit.UnitName || iusTable.Columns[ColIndex].ColumnName == SubgroupVals.SubgroupVal || this.SubgroupDimensionsList.Contains(iusTable.Columns[ColIndex].ColumnName))
                {
                }
                else
                {
                    // Remove Column
                    iusTable.Columns.RemoveAt(ColIndex);
                    ColIndex--;
                }
                iusTable.AcceptChanges();
                ColIndex++;
            }
        }

        private void AddSubgroupDimensionColumns(DataTable IUSTable,  DataTable sgValDimensionTable)
        {
           
            DataTable SGDimensionTable;
            // Get Sungroup Dimensions 
            SGDimensionTable = this.DBConnection.ExecuteDataTable(this.DBQueries.SubgroupTypes.GetSubgroupTypes(FilterFieldType.None, string.Empty));
                       
            // Add GUID Column
            IUSTable.Columns.Add(Constants.IUSSheet.GUIDColumn);

            this.SubgroupDimensionsList.Clear();
            // Get Subgroup Dimension Columns
            foreach (DataRow Row in SGDimensionTable.Rows)
            {
                this.SubgroupDimensionsList.Add(Convert.ToString(Row[SubgroupTypes.SubgroupTypeName]));
                IUSTable.Columns.Add(Convert.ToString(Row[SubgroupTypes.SubgroupTypeName]));
            }
            IUSTable.AcceptChanges();          

           
        }

        private DataTable MergeSnoColumns(DataTable iusTable)
        {
            DataTable RetVal = new DataTable();
            DataColumn SnoColumn = new DataColumn(Constants.IUSSheet.SNo, Type.GetType("System.Int32"));
            SnoColumn.AutoIncrement = true;
            SnoColumn.AutoIncrementSeed = 1;
            SnoColumn.AutoIncrementStep = 1;
            RetVal.Columns.Add(SnoColumn);


            RetVal.Merge(iusTable);

            return RetVal;
        }
        
        private void FormatCell(DIExcel excelObj, int sheetIndex)
        {
            try
            {
                // Set Title Font
                excelObj.GetRangeFont(sheetIndex, Constants.IUSSheet.TitleRowIndex, Constants.IUSSheet.FirstColumnIndex, Constants.IUSSheet.TitleRowIndex, Constants.IUSSheet.FirstColumnIndex).Bold = true;
                excelObj.GetRangeFont(sheetIndex, Constants.IUSSheet.TitleRowIndex, Constants.IUSSheet.FirstColumnIndex, Constants.IUSSheet.TitleRowIndex, Constants.IUSSheet.FirstColumnIndex).Size = 12;

                // Set Name Font
                excelObj.GetRangeFont(sheetIndex, Constants.IUSSheet.NameRowIndex, Constants.IUSSheet.FirstColumnIndex, Constants.IUSSheet.NameRowIndex, Constants.IUSSheet.FirstColumnIndex).Size = 10;
                excelObj.GetRangeFont(sheetIndex, Constants.IUSSheet.NameRowIndex, Constants.IUSSheet.FirstColumnIndex, Constants.IUSSheet.NameRowIndex, Constants.IUSSheet.FirstColumnIndex).Bold = true;

                excelObj.GetRange(sheetIndex, Constants.IUSSheet.TableStartRowIndex, Constants.IUSSheet.FirstColumnIndex, excelObj.GetUsedRange(sheetIndex).RowCount - 1, excelObj.GetUsedRange(sheetIndex).ColumnCount - 1);

                // Set Table Header backgroud Color LightGray
                excelObj.SetRangeColor(sheetIndex, Constants.IUSSheet.TableStartRowIndex, Constants.IUSSheet.FirstColumnIndex, Constants.IUSSheet.TableStartRowIndex, excelObj.GetUsedRange(sheetIndex).ColumnCount - 1, Color.Black, Color.LightGray);
                // Set GUID Column backgroud Color LightGray
                excelObj.SetRangeColor(sheetIndex, Constants.IUSSheet.TableStartRowIndex, Constants.IUSSheet.GuidColIndex, excelObj.GetUsedRange(sheetIndex).RowCount - 1, Constants.IUSSheet.GuidColIndex, Color.Black, Color.LightGray);

                // Set Subgroup Dimension Column backgroud Color LightGray
                excelObj.SetRangeColor(sheetIndex, Constants.IUSSheet.DimensionHeaderRowIndex, Constants.IUSSheet.DimensionStartColIndex, excelObj.GetUsedRange(sheetIndex).RowCount - 1, excelObj.GetUsedRange(sheetIndex).ColumnCount - 1, Color.Black, Color.LightGray);
                // Set Bottom Border color for Subgroup Dimensions
                excelObj.SetRangeBorder(sheetIndex, Constants.IUSSheet.DimensionHeaderRowIndex, Constants.IUSSheet.DimensionStartColIndex,Constants.IUSSheet.DimensionHeaderRowIndex,excelObj.GetUsedRange(sheetIndex).ColumnCount - 1, LineStyle.Continuous, BorderWeight.Thin,Color.Black,BordersIndex.EdgeBottom);

                // Set Font Size
                excelObj.GetRangeFont(sheetIndex, Constants.IUSSheet.DimensionHeaderRowIndex, Constants.IUSSheet.FirstColumnIndex, excelObj.GetUsedRange(sheetIndex).RowCount - 1, excelObj.GetUsedRange(sheetIndex).ColumnCount - 1).Size = 8;
                
                // Set Font Bold
                excelObj.GetRangeFont(sheetIndex, Constants.IUSSheet.DimensionHeaderRowIndex, Constants.IUSSheet.FirstColumnIndex, Constants.IUSSheet.TableStartRowIndex, excelObj.GetUsedRange(sheetIndex).ColumnCount - 1).Bold = true;

                // Set Font Name
                excelObj.GetRangeFont(sheetIndex, Constants.IUSSheet.TitleRowIndex, Constants.IUSSheet.FirstColumnIndex, excelObj.GetUsedRange(sheetIndex).RowCount - 1, excelObj.GetUsedRange(sheetIndex).ColumnCount - 1).Name = Constants.IUSSheet.SheetFontName;
                // Center Align
                excelObj.SetHorizontalAlignment(sheetIndex, Constants.IUSSheet.DimensionHeaderRowIndex, Constants.IUSSheet.DimensionStartColIndex, HAlign.Center);

                // Set S.No Column Width
                excelObj.SetColumnWidth(sheetIndex, Constants.IUSSheet.SnoColumnWidth, Constants.IUSSheet.TableStartRowIndex, Constants.IUSSheet.FirstColumnIndex, excelObj.GetUsedRange(sheetIndex).RowCount - 1, Constants.IUSSheet.FirstColumnIndex);
                // Set GUID Column Width
                excelObj.SetColumnWidth(sheetIndex, Constants.IUSSheet.GUIDColumnWidth, Constants.IUSSheet.TableStartRowIndex, Constants.IUSSheet.GuidColIndex, excelObj.GetUsedRange(sheetIndex).RowCount - 1, Constants.IUSSheet.GuidColIndex);
                // Set Indicator Column Width
                excelObj.SetColumnWidth(sheetIndex, Constants.IUSSheet.IndicatorColumnWidth, Constants.IUSSheet.TableStartRowIndex, Constants.IUSSheet.IndicatorColIndex, excelObj.GetUsedRange(sheetIndex).RowCount - 1, Constants.IUSSheet.IndicatorColIndex);
                // Set Unit Column Width
                excelObj.SetColumnWidth(sheetIndex, Constants.IUSSheet.UnitColumnWidth, Constants.IUSSheet.TableStartRowIndex, Constants.IUSSheet.UnitColIndex, excelObj.GetUsedRange(sheetIndex).RowCount - 1, Constants.IUSSheet.UnitColIndex);
                // Set Subgroup Dimensions Column Width
                excelObj.SetColumnWidth(sheetIndex, Constants.IUSSheet.SubgroupColumnWidth, Constants.IUSSheet.TableStartRowIndex, Constants.IUSSheet.SgValColIndex, excelObj.GetUsedRange(sheetIndex).RowCount - 1, Constants.IUSSheet.SgValColIndex);

                // Set Subgroup Dimensions Column Width
                excelObj.SetColumnWidth(sheetIndex, Constants.IUSSheet.DimensionsColumnWidth, Constants.IUSSheet.TableStartRowIndex, Constants.IUSSheet.DimensionStartColIndex, excelObj.GetUsedRange(sheetIndex).RowCount - 1, excelObj.GetUsedRange(sheetIndex).ColumnCount - 1);

            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }

        private void FreezePane(DIExcel excelObj, int sheetIndex)
        {
            SpreadsheetGear.IWorksheet Worksheet = excelObj.GetWorksheet(sheetIndex);
            // Split after column "B" (ScrollColumn is zero based). 
            Worksheet.WindowInfo.SplitColumns = 2; 
         
            // Split after row 5 (ScrollRow is zero based). 
            Worksheet.WindowInfo.SplitRows = 5; 
         
            // Freeze the panes. 
            Worksheet.WindowInfo.FreezePanes = true;                
          
        }       
        
        #endregion

        #endregion

        #region "-- public --"

        #region "-- Variables --"




        #endregion

        #region "-- new/dispose --"
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        public IUSSheetExporter(DIConnection dbConnection, DIQueries dbQueries)
        {
            this.DBConnection = dbConnection;
            this.DBQueries = dbQueries;
        }

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Export IUS into IUS Spreadsheet
        /// </summary>
        /// <param name="xlsOutputFileName"></param>
        /// <param name="languageFileNameWPath"></param>
        /// <returns></returns>
        public bool ExportIUS(string xlsOutputFileName, string languageFileNameWPath)
        {
            bool RetVal = false;
            int SheetIndex = 0;
            DataTable IUSSheetTable = null;
            IWorksheet WorkSheet = null;

            DIExcel ExcelObj = new DIExcel();
            try
            {
                // Get IUS Table
                IUSSheetTable = this.GetIUSTable();
                // Rename First Sheet
                ExcelObj.RenameWorkSheet(0, Constants.IUSSheet.SheetName);
                WorkSheet = ExcelObj.GetWorkSheet(Constants.IUSSheet.SheetName);
                SheetIndex = ExcelObj.GetSheetIndex(Constants.IUSSheet.SheetName);

                // Set Sheet Title and Header Values
                ExcelObj.SetCellValue(SheetIndex, Constants.IUSSheet.TitleRowIndex, Constants.IUSSheet.FirstColumnIndex, Constants.IUSSheet.SheetTitle);
                ExcelObj.SetCellValue(SheetIndex, Constants.IUSSheet.NameRowIndex, Constants.IUSSheet.FirstColumnIndex, Constants.IUSSheet.SheetName);

                // Load DataTable Into Worksheet
                ExcelObj.LoadDataTableIntoSheet(Constants.IUSSheet.TableStartRowIndex, Constants.IUSSheet.FirstColumnIndex, IUSSheetTable, SheetIndex, false);

                // Set Subgroup Dimension Column Header
                ExcelObj.SetCellValue(SheetIndex, Constants.IUSSheet.DimensionHeaderRowIndex, Constants.IUSSheet.DimensionStartColIndex, Constants.IUSSheet.SubgroupDimensionsColumn);
                // Merger Subgroup Dimension Column Text
                ExcelObj.MergeCells(SheetIndex, Constants.IUSSheet.DimensionHeaderRowIndex, Constants.IUSSheet.DimensionStartColIndex, Constants.IUSSheet.DimensionHeaderRowIndex, ExcelObj.GetUsedRange(SheetIndex).ColumnCount - 1);

                // Format Sheet Contents
                this.FormatCell(ExcelObj, SheetIndex);
                
                // Freeze Pane
                this.FreezePane(ExcelObj, SheetIndex);

                // Save Excel File
                ExcelObj.SaveAs(xlsOutputFileName);
               
                RetVal = true;
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
            finally
            {
                if (ExcelObj != null)
                {
                    ExcelObj.Close();
                }
            }
            return RetVal;
        }

        #endregion


        #endregion

    }
}
