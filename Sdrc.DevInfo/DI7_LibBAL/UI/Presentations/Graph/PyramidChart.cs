using System;
using System.Collections.Generic;
using System.Text;

using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableGraph;
using DevInfo.Lib.DI_LibBAL.Utility;

using SpreadsheetGear;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Graph
{
    internal class PyramidChart
    {
        #region "-- Public --"

        #region "-- New / Dispose --"

        public PyramidChart()
        { 
        }

        #endregion

        #region "-- Variable / Properties --"

        //private string _DataSourceDimensions = string.Empty;
        ///// <summary>
        ///// Gets or sets the DataSourceDimensions
        ///// </summary>
        ///// <remarks>Contains the comma delimited start and end row and column index. This will help to get the range for excel chart generation.</remarks>
        //public string DataSourceDimensions
        //{
        //    get 
        //    {
        //        return this._DataSourceDimensions; 
        //    }
        //    set 
        //    {
        //        this._DataSourceDimensions = value; 
        //    }
        //}
	

        #endregion

        #region "-- Method --"

        internal Dictionary<string, string> GetSeries(Fields fields, string xlsFileNameWPath, ChartMode chartMode, int columnCount)
        {
            Dictionary<string, string> RetVal = new Dictionary<string, string>();
            try
            {
                Dictionary<string, IRange> PyramidSeries = new Dictionary<string, IRange>();

                if (chartMode == ChartMode.Insert || chartMode == ChartMode.Edit)
                {
                    this.TableSheetIndex = 1;
                }

                this.ExcelFile = new DIExcel(xlsFileNameWPath, System.Threading.Thread.CurrentThread.CurrentCulture);

                //-- Insert column and its value.
                this.InserDataValueColumn(fields, columnCount);

                //-- Generate the series for pyramid chart
                PyramidSeries = this.GetPyramidSeries(fields, chartMode);

                //-- Save the file
                this.ExcelFile.Save();

                //-- Insert / Edit the chart
                switch (chartMode)
                {
                    case ChartMode.Insert:
                        this.InsertChart(PyramidSeries, xlsFileNameWPath);
                        break;

                    case ChartMode.Edit:
                        this.CreatePyramidChartAndSetDataRange(xlsFileNameWPath, PyramidSeries);
                        break;

                    case ChartMode.ExcelInsert:
                    case ChartMode.ExcelEdit:
                        RetVal = this.ConvertRangeIntoString(PyramidSeries);
                        break;
                    default:
                        break;
                }

                this.ExcelFile.Close();
            }
            catch (Exception)
            {
            }
            return RetVal;
        }  

        #endregion

        #endregion

        #region "-- Private --"

        #region "-- Constant --"        

        private const int AreaColumnIndex = 0;

        private const int DimensionIndex = 1;

        /// <summary>
        /// Spreadhseet gear object
        /// </summary>
        private SpreadsheetGear.Windows.Forms.WorkbookView moSpreadsheet = new SpreadsheetGear.Windows.Forms.WorkbookView();

        #endregion

        #region "-- Variable --"

        private DIExcel ExcelFile;

        private int InsertColumnIndex = -1;

        private int TableSheetIndex = 0;

        #endregion

        #region "-- Method --"

        /// <summary>
        /// Insert the pyramid chart.
        /// </summary>
        /// <param name="pyramidSeries"></param>
        /// <param name="filename"></param>
        private void InsertChart(Dictionary<string, IRange> pyramidSeries, string filename)
        {
            this.LoadWorkbook(filename);
            SpreadsheetGear.Charts.IChart objChart;

            SpreadsheetGear.Shapes.IShapes shapes = moSpreadsheet.ActiveWorksheet.Shapes;
            if (shapes.Count > 0)
            {
                shapes[0].Delete();
            }
            objChart = shapes.AddChart(10, 10, 550, 400).Chart;
            objChart.ChartType = SpreadsheetGear.Charts.ChartType.BarClustered;

            int index = 0;
            foreach (KeyValuePair<string, IRange> Series in pyramidSeries)
            {
                if (index == 0)
                {
                   objChart.SetSourceData(Series.Value, SpreadsheetGear.Charts.RowCol.Columns);
                   objChart.SeriesCollection[0].Name = Series.Key;
                   objChart.SeriesCollection[0].Values = Series.Value;
                }
                else if (index == 1)
                {
                    objChart.SeriesCollection[0].XValues = Series.Value;
                }
                else
                {
                    SpreadsheetGear.Charts.ISeries ChartSeries = objChart.SeriesCollection.Add();
                    ChartSeries.Name = Series.Key;
                    ChartSeries.Values = Series.Value;
                }
                index += 1;
            }

            objChart.ChartGroups[0].GapWidth = 0.0;
            objChart.ChartGroups[0].Overlap = 100.0;
            objChart.Axes[SpreadsheetGear.Charts.AxisType.Category].MajorTickMark = SpreadsheetGear.Charts.TickMark.None;
            objChart.Axes[SpreadsheetGear.Charts.AxisType.Category].TickLabelPosition = SpreadsheetGear.Charts.TickLabelPosition.Low;
            objChart.PlotArea.Width = 380;
            objChart.Legend.IncludeInLayout = false;
            objChart.Legend.Position = SpreadsheetGear.Charts.LegendPosition.Right;
            objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].TickLabels.NumberFormat = "#" + DICommon.NumberGroupSeparator + "##0" + DICommon.NumberDecimalSeparator + "00;#" + DICommon.NumberGroupSeparator + "##0" + DICommon.NumberDecimalSeparator + "00"; 

            if(objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MaximumScale > -(objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MinimumScale))
            {
                objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MinimumScale = -(objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MaximumScale);
            }
            else if (objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MaximumScale < -(objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MinimumScale))
            {
                objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MaximumScale = -(objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MinimumScale);
            }
            


            this.moSpreadsheet.ActiveWorkbook.Save();
            this.moSpreadsheet.ActiveWorkbook.Close();
            this.moSpreadsheet.ReleaseLock();
        }

        /// <summary>
        /// Create chart and redfine the data source.
        /// </summary>
        /// <param name="prestnationFilenameWPath"></param>
        private void CreatePyramidChartAndSetDataRange(string prestnationFilenameWPath, Dictionary<string, IRange> pyramidSeries)
        {
            try
            {
                int index = 0;
                int OldSeriesCollectionCount = 0;
                SpreadsheetGear.Charts.ISeries ChartSeries = null;

                this.LoadWorkbook(prestnationFilenameWPath);                
                SpreadsheetGear.Charts.IChart objChart;

                try
                {
                    //-- Rename Graph worksheet
                    this.moSpreadsheet.ActiveWorkbook.Sheets["Graph"].Name = DILanguage.GetLanguageString("Graph");
                }
                catch (Exception)
                {
                }
                this.moSpreadsheet.ActiveWorkbook.Sheets[DILanguage.GetLanguageString("Graph")].Select();

                SpreadsheetGear.IWorksheet worksheet = (SpreadsheetGear.IWorksheet)moSpreadsheet.ActiveWorkbook.Sheets[DILanguage.GetLanguageString("Table")];
                SpreadsheetGear.Shapes.IShapes shapes = moSpreadsheet.ActiveWorksheet.Shapes;

                objChart = shapes[0].Chart;
                OldSeriesCollectionCount = objChart.SeriesCollection.Count;
                objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MinimumScaleIsAuto = true;
                objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MaximumScaleIsAuto = true;

                foreach (KeyValuePair<string, IRange> Series in pyramidSeries)
                {
                    if (index == 0)
                    {
                        ChartSeries = objChart.SeriesCollection[index];
                        ChartSeries.Name = Series.Key;
                        ChartSeries.XValues = Series.Value;
                    }
                    else if (index == 1)
                    {
                        ChartSeries.Name = Series.Key;
                        ChartSeries.Values = Series.Value;
                    }
                    else
                    {
                        ChartSeries = objChart.SeriesCollection[index - 1];
                        ChartSeries.Name = Series.Key;
                        ChartSeries.Values = Series.Value;
                    }
                    index += 1;
                }

                //this.moSpreadsheet.ActiveWorkbook.Save();
                //shapes = moSpreadsheet.ActiveWorksheet.Shapes;
                //objChart = shapes[0].Chart;

                //if (objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MaximumScale > -(objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MinimumScale))
                //{
                //    objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MinimumScale = -(objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MaximumScale);
                //}
                //else if (objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MaximumScale < -(objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MinimumScale))
                //{
                //    objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MaximumScale = -(objChart.Axes[SpreadsheetGear.Charts.AxisType.Value].MinimumScale);
                //}

                this.moSpreadsheet.ActiveWorkbook.Save();
                this.moSpreadsheet.ActiveWorkbook.Close();
                this.moSpreadsheet.ReleaseLock();
            }
            catch (Exception)
            {
            }
        }

        private void LoadWorkbook(string fileNameWPath)
        {
            this.moSpreadsheet.GetLock();
            if (this.moSpreadsheet.ActiveWorkbook != null)
            {
                this.moSpreadsheet.ActiveWorkbook.Close();
            }
            this.moSpreadsheet.ActiveWorkbook = SpreadsheetGear.Factory.GetWorkbook(fileNameWPath, System.Globalization.CultureInfo.CurrentCulture);
            this.moSpreadsheet.ActiveWorkbook.DisplayDrawingObjects = SpreadsheetGear.DisplayDrawingObjects.DisplayShapes;
        }

        /// <summary>
        /// Generate the pyramid series
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, IRange> GetPyramidSeries(Fields fields, ChartMode chartMode)
        {
            Dictionary<string, IRange> RetVal = new Dictionary<string, IRange>();
            try
            {
                int Index=0;
                int StartRowIndex = 1;
                int FormulaRange = -1; 
                IRange Range = null;
                string ColumnHeader = string.Empty;
                string TempAreaName = string.Empty;
                string AreaName = string.Empty;
                int RowCount = this.ExcelFile.GetUsedRange(TableSheetIndex).Rows.Count;
                int ColumnCount = this.ExcelFile.GetUsedRange(TableSheetIndex).Columns.Count;
                int NewColumnIndex = this.InsertColumnIndex;

                if (fields.Rows.Count > 1)
                {
                    if (this.ExcelFile.GetUsedRange(TableSheetIndex).Columns.Count > 2)
                    {
                        FormulaRange = fields.Rows.Count;
                    }
                    else
                    {
                        FormulaRange = this.InsertColumnIndex - 2;
                    }

                    AreaName = this.ExcelFile.GetCellValue(TableSheetIndex, 1, AreaColumnIndex, 1, AreaColumnIndex);

                    for (int ColumnIndex = FormulaRange; ColumnIndex < this.InsertColumnIndex; ColumnIndex += 2)
                    {
                        for (int RowIndex = 2; RowIndex < RowCount; RowIndex++)
                        {
                            TempAreaName = this.ExcelFile.GetCellValue(TableSheetIndex, RowIndex, AreaColumnIndex, RowIndex, AreaColumnIndex);
                            if (AreaName.ToLower() != TempAreaName.ToLower())
                            {
                                ColumnHeader = this.ExcelFile.GetCellValue(TableSheetIndex, 0, ColumnIndex, 0, ColumnIndex).Replace("\n", "-");
                                if (Index == 0)
                                {
                                    //this._DataSourceDimensions = StartRowIndex.ToString() + "," + DimensionIndex.ToString() + "," + (RowIndex - 1) + "," + (this.InsertColumnIndex - 2);
                                    Range = this.ExcelFile.GetSelectedRange(TableSheetIndex, StartRowIndex, DimensionIndex, RowIndex - 1, FormulaRange);
                                    RetVal.Add(AreaName + " - " + ColumnHeader, Range);
                                }
                                else
                                {
                                    Range = this.ExcelFile.GetSelectedRange(TableSheetIndex, RowIndex, ColumnIndex, RowIndex, ColumnIndex);
                                    RetVal.Add(AreaName + " - " + ColumnHeader, Range);
                                }

                                ColumnHeader = this.ExcelFile.GetCellValue(TableSheetIndex, 0, NewColumnIndex, 0, NewColumnIndex).Replace("\n", "-");
                                Range = this.ExcelFile.GetSelectedRange(TableSheetIndex, RowIndex, NewColumnIndex, RowIndex, NewColumnIndex);
                                RetVal.Add(AreaName + " - " + ColumnHeader, Range);

                                AreaName = TempAreaName;
                                StartRowIndex = RowIndex;                                
                            }
                        }
                        Index += 1;
                        StartRowIndex = 2;
                        NewColumnIndex += 1;
                    }
                }
                else
                {
                    for (int ColumnIndex = 1; ColumnIndex < this.InsertColumnIndex; ColumnIndex += 2)
                    {
                        if (Index == 0)
                        {
                            if (chartMode == ChartMode.Insert)
                            {
                                Range = this.ExcelFile.GetSelectedRange(TableSheetIndex, StartRowIndex, 1, RowCount - 1, 1);
                                ColumnHeader = this.ExcelFile.GetCellValue(TableSheetIndex, 0, 1, 0, 1).Replace("\n", "-");
                                RetVal.Add(ColumnHeader, Range);

                                Range = this.ExcelFile.GetSelectedRange(TableSheetIndex, StartRowIndex, 0, RowCount - 1, 0);
                                ColumnHeader = "1";
                                RetVal.Add(ColumnHeader, Range);
                            }
                            else if (chartMode == ChartMode.ExcelInsert)
                            {
                                Range = this.ExcelFile.GetSelectedRange(TableSheetIndex, StartRowIndex, 0, RowCount - 1, 1);
                                ColumnHeader = this.ExcelFile.GetCellValue(TableSheetIndex, 0, 1, 0, 1).Replace("\n", "-");
                                RetVal.Add(ColumnHeader, Range);
                            }
                            else if (chartMode == ChartMode.Edit || chartMode == ChartMode.ExcelEdit)
                            {
                                Range = this.ExcelFile.GetSelectedRange(TableSheetIndex, StartRowIndex, 0, RowCount - 1, fields.Columns.Count - 1);
                                ColumnHeader = ""; 
                                RetVal.Add(ColumnHeader, Range);

                                Range = this.ExcelFile.GetSelectedRange(TableSheetIndex, StartRowIndex, 1, RowCount - 1, 1);
                                ColumnHeader = this.ExcelFile.GetCellValue(TableSheetIndex, 0, 1, 0, 1).Replace("\n", "-");
                                RetVal.Add(ColumnHeader, Range);
                            }
                        }
                        else
                        {
                            Range = this.ExcelFile.GetSelectedRange(TableSheetIndex, StartRowIndex, ColumnIndex, RowCount - 1, ColumnIndex);
                            ColumnHeader = this.ExcelFile.GetCellValue(TableSheetIndex, 0, ColumnIndex, 0, ColumnIndex).Replace("\n", "-");
                            if (!string.IsNullOrEmpty(ColumnHeader.Trim()))
                            {
                                RetVal.Add(ColumnHeader, Range);
                            }
                        }

                        Range = this.ExcelFile.GetSelectedRange(TableSheetIndex, StartRowIndex, NewColumnIndex, RowCount - 1, NewColumnIndex);
                        ColumnHeader = this.ExcelFile.GetCellValue(TableSheetIndex, 0, NewColumnIndex, 0, NewColumnIndex).Replace("\n", "-");
                        if (!string.IsNullOrEmpty(ColumnHeader.Trim()))
                        {
                            RetVal.Add(ColumnHeader, Range);
                        }

                        Index += 1;
                        NewColumnIndex += 1;
                    }
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Inser the data value column and its data value
        /// </summary>
        /// <param name="fields"></param>
        private void InserDataValueColumn(Fields fields, int columnCount)
        {            
            int RowCount = -1;
            int ColumnCount = -1;
            string Range = string.Empty;
            int FormulaRange = -1;
            int NewColumnIndex = -1;

            //-- Get the index of the column
            InsertColumnIndex = columnCount + fields.Rows.Count;
            NewColumnIndex = this.InsertColumnIndex;

            if (this.ExcelFile.GetUsedRange(TableSheetIndex).Columns.Count > 2)
            {
                FormulaRange = fields.Rows.Count + 1;
            }
            else
            {
                FormulaRange = this.InsertColumnIndex - 1;
            }

            //-- Used row count
            RowCount = this.ExcelFile.GetUsedRange(TableSheetIndex).Rows.Count;
            ColumnCount = this.ExcelFile.GetUsedRange(TableSheetIndex).Columns.Count;


            for (int ColumnIndex = FormulaRange; ColumnIndex < ColumnCount; ColumnIndex += 2)
            {
                //-- Copy the datavalues through formula
                for (int Index = 0; Index < RowCount; Index++)
                {
                    string ColumnRange = this.ExcelFile.GetRange(TableSheetIndex, Index, ColumnIndex, Index, ColumnIndex);
                    if (Index == 0)
                    {
                        this.ExcelFile.ExecuteFormula(TableSheetIndex, Index, NewColumnIndex, Index, NewColumnIndex, "=" + ColumnRange);
                    }
                    else
                    {
                        this.ExcelFile.ExecuteFormula(TableSheetIndex, Index, NewColumnIndex, Index, NewColumnIndex, "=-" + ColumnRange);
                    }
                    this.ExcelFile.SetForegroundColor(TableSheetIndex, NewColumnIndex, System.Drawing.Color.White);  
                }
                NewColumnIndex += 1;
            }
        }

        /// <summary>
        /// Get the index new inserted column.
        /// </summary>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        private int GetInsertedColumnIndex(int rowCount)
        {
            int RetVal = 0;
            try
            {
                int ColumnCount = 0;

                //-- Get the index of column which is to be inserted.
                ColumnCount = this.ExcelFile.GetUsedRange(TableSheetIndex).Columns.Count;
                //if (ColumnCount - rowCount >= 2)
                //{
                //    //RetVal = rowCount + 1;
                    RetVal = ColumnCount;
                //}
                //else
                //{
                //    RetVal = rowCount;
                //}
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Convert the IRange into the string for excel mode
        /// </summary>
        /// <param name="pyramidSeries"></param>
        /// <returns></returns>
        private Dictionary<string, string> ConvertRangeIntoString(Dictionary<string, IRange> pyramidSeries)
        {
            Dictionary<string, string> RetVal = new Dictionary<string, string>();
            try
            {
                foreach (KeyValuePair<string, IRange> Series in pyramidSeries)
                {
                    RetVal.Add(Series.Key, Series.Value.GetAddress(false, false, ReferenceStyle.A1, false, null));
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        #endregion

        #endregion
    }
}
