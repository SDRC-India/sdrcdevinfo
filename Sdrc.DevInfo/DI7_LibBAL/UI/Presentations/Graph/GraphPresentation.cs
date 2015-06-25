using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Xml;
using System.Drawing.Imaging;
using System.Xml.Serialization;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;

using DevInfo.Lib.DI_LibBAL.UI.DataViewPage;
using DevInfo.Lib.DI_LibBAL.UI.Presentations;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common.TableGraph;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Table;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Common;
using DevInfo.Lib.DI_LibBAL.Utility;

using SpreadsheetGear;
using SpreadsheetGear.Charts;

namespace DevInfo.Lib.DI_LibBAL.UI.Presentations.Graph
{

    /// <summary>
    /// Represents the main class for Graph Wizard. 
    /// Generate the image for the graph .that graph will be used for both desktop and web application
    /// </summary>
    /// <remarks>
    /// For desktop an instance of this class shall be held in a variable throughout the lifecycle of graph wizard 
    /// For Web application instance of this class shall be held in session variable. 
    /// </remarks>	
    public class GraphPresentation : ICloneable, IDisposable,ILanguage
    {

        #region " -- ENUM --"
 
        /// <summary>
        /// Enum for comparing the object.
        /// </summary>
        public enum ObjectComparison
        {
            /// <summary>
            /// Compare only the format section
            /// </summary>
            Format = 0,
            /// <summary>
            /// Compare the Graph presenation obect.
            /// </summary>
            All = 1
        }

        #endregion

        # region " -- Private -- "

        #region " -- Variables -- "
        /// <summary>
        /// GraphDataSource : Created using TablePresenation class
        /// </summary>
        [XmlIgnore()]
        public DataTable GraphDataSource;

        /// <summary>
        /// Hold the UserPref Object for private Usuage
        /// </summary>
        private UserPreference.UserPreference UserPreference;

        /// <summary>
        /// Spreadhseet gear object
        /// </summary>
        private SpreadsheetGear.Windows.Forms.WorkbookView moSpreadsheet = new SpreadsheetGear.Windows.Forms.WorkbookView();

        private PyramidChart PyramidChart = new PyramidChart();

        #endregion

        #region -- Method --
        /// <summary>
        /// Use UTF8 encoding to deserailize the text
        /// </summary>
        /// <param name="SerializeText"></param>
        /// <returns></returns>
        private static byte[] StringToUTF8ByteArray(string SerializeText)
        {
            byte[] RetVal;
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();
                RetVal = encoding.GetBytes(SerializeText);
            }
            catch (Exception)
            {

                throw;
            }
            return RetVal;
        }

        /// <summary>
        /// We would be using UTF-8 encoding for the creating the XML stream for the custom object as it supports a wide range of Unicode character values and surrogates. 
        /// For this purpose, we will make use of the UTF8Encoding class provided by the .Net framework
        /// </summary>
        /// <param name="characters"></param>
        /// <returns>String</returns>
        private string UTF8ByteArrayToString(byte[] characters)
        {
            string RetVal = string.Empty;
            try
            {
                UTF8Encoding Encoding = new UTF8Encoding();
                RetVal = Encoding.GetString(characters).Trim();
            }
            catch (Exception ex)
            {
                throw;
            }
            return RetVal;
        }


        /// <summary>
        /// Gerenate the Image name for the graph image
        /// </summary>
        /// <returns>String ::-> Image name </returns>
        private string GenerateImageName()
        {
            string RetVal = string.Empty;
            //RetVal = Constants.IMAGENAMEPREFIX + DateTime.Now.ToString(Constants.IMAGEDATETIMESTAMP) + Constants.IMAGEEXTENTION;
            return RetVal;
        }

        private void InsertChart(string prestnationFilenameWPath)
        {
            //-- Load the worksheet
            this.LoadWorkbook(prestnationFilenameWPath);

            try
            {
                this.moSpreadsheet.GetLock();
                try
                {
                    //-- Rename Graph worksheet
                    this.moSpreadsheet.ActiveWorkbook.Sheets["Graph"].Name = DILanguage.GetLanguageString("Graph");
                }
                catch (Exception)
                {
                }
                this.moSpreadsheet.ActiveWorkbook.Sheets[DILanguage.GetLanguageString("Graph")].Select();
                SpreadsheetGear.Charts.IChart objChart;

                SpreadsheetGear.IWorksheet worksheet = (SpreadsheetGear.IWorksheet)moSpreadsheet.ActiveWorkbook.Sheets[DILanguage.GetLanguageString("Table")];
                SpreadsheetGear.Shapes.IShapes shapes = moSpreadsheet.ActiveWorksheet.Shapes;
                if (shapes.Count > 0)
                {
                    shapes[0].Delete();
                }

                objChart = shapes.AddChart(10, 10, 500, 400).Chart;

                //SpreadsheetGear.IRange Source1 = worksheet.Range[this.moSpreadsheet.ActiveWorkbook.Worksheets[DILanguage.GetLanguageString("Table")].UsedRange.Address];
                SpreadsheetGear.IRange ChartSource = this.DefineChartRange(worksheet);

                objChart.ChartType = this._SpreadsheetGearChartType;
                objChart.SetSourceData(ChartSource, SpreadsheetGear.Charts.RowCol.Columns);
               

                if (this._SpreadsheetGearChartType != SpreadsheetGear.Charts.ChartType.Pie)
                {                    
                    DataRow Row = this._TablePresentation.TableXLS.Rows[0];
                    for (int i = 0; i < objChart.SeriesCollection.Count; i++)
                    {
                        objChart.SeriesCollection[i].Name = Row[i + 1].ToString();
                        objChart.SeriesCollection[i].XValues = this.moSpreadsheet.ActiveWorkbook.Worksheets[1].Range[1, 0, ChartSource.Range.RowCount, 0];
                        objChart.SeriesCollection[i].Values = this.moSpreadsheet.ActiveWorkbook.Worksheets[1].Range[1, i + 1, ChartSource.Range.RowCount, i + 1];
                    }
                }

                this.ApplyTemplate();
                this.moSpreadsheet.ActiveWorkbook.Save();
            }
            catch (Exception)
            {
            }
            finally
            {
                this.moSpreadsheet.ReleaseLock();
            }
        }

        /// <summary>
        /// Define the chart range from the table worksheet
        /// </summary>
        /// <param name="worksheet"></param>
        /// <returns></returns>
        private SpreadsheetGear.IRange DefineChartRange(SpreadsheetGear.IWorksheet worksheet)
        {
            SpreadsheetGear.IRange Retval = null;
            try
            {
                //int ColumnCount = 1;
                //if (worksheet.UsedRange.ColumnCount - 2 <= 0)
                //{
                //    ColumnCount = 1;
                //}
                //else
                //{
                //    ColumnCount = worksheet.UsedRange.ColumnCount - 2;
                //}
                Retval = worksheet.Range[0 + 1, 0, this._TablePresentation.DataTableRowCount - 1, this._TablePresentation.DataTableColumnCount - 1];// this.moSpreadsheet.ActiveWorkbook.Worksheets[DILanguage.GetLanguageString("Table")].UsedRange.Address];
            }
            catch (Exception)
            {

            }
            return Retval;
        }

        /// <summary>
        /// Load the spreadsheetgear workbook
        /// </summary>
        /// <param name="prestnationFilenameWPath"></param>
        private void LoadWorkbook(string prestnationFilenameWPath)
        {
            try
            {
                this.moSpreadsheet.GetLock();
                if (this.moSpreadsheet.ActiveWorkbook != null)
                {
                    this.moSpreadsheet.ActiveWorkbook.Close();
                }
                this.moSpreadsheet.ActiveWorkbook = SpreadsheetGear.Factory.GetWorkbook(prestnationFilenameWPath, System.Globalization.CultureInfo.CurrentCulture);
                this.moSpreadsheet.ActiveWorkbook.DisplayDrawingObjects = SpreadsheetGear.DisplayDrawingObjects.DisplayShapes;
                
            }         
            catch (Exception)
            {
            }
            finally
            {
                this.moSpreadsheet.ReleaseLock();
            }
        }

        /// <summary>
        /// Get the chart image
        /// </summary>
        /// <returns></returns>
        private Image GenerateImage()
        {
            Image Retval = null;
            try
            {             
                this.moSpreadsheet.GetLock();

                //-- Set the aspect ratio of the image
                this.SetAspectRatio();

                SpreadsheetGear.Shapes.IShapes ExcelShapes = this.moSpreadsheet.ActiveWorksheet.Shapes;
                SpreadsheetGear.Drawing.Image SpreadsheetImage = new SpreadsheetGear.Drawing.Image(ExcelShapes[0]);

                Bitmap ExcelBitmap = SpreadsheetImage.GetBitmap(); 
                Retval = (Image)ExcelBitmap;
            }
            catch (Exception)
            {
            }
            finally
            {
                this.moSpreadsheet.ReleaseLock();
            }
            return Retval;
        }

        /// <summary>
        /// Get the spreadsheet color
        /// </summary>
        /// <param name="systemColor"></param>
        /// <returns></returns>
        private SpreadsheetGear.Drawing.Color GetSpreadsheetColor(Color systemColor)
        {
            SpreadsheetGear.Drawing.Color Retval = new SpreadsheetGear.Drawing.Color();
            try
            {
                Retval = Color.FromArgb(systemColor.R, systemColor.G, systemColor.B);
                //Retval = systemColor;
                //Retval.A = systemColor.A;
                //Retval.R = systemColor.R;
                //Retval.G = systemColor.G;
                //Retval.B = systemColor.B;
            }
            catch (Exception)
            {
            }
            return Retval;
        }     

        /// <summary>
        /// Apply the template file
        /// </summary>
        private void ApplyTemplate()
        {
            try
            {
                this.moSpreadsheet.ActiveWorkbook.Sheets[DILanguage.GetLanguageString("Graph")].Select();
                SpreadsheetGear.Shapes.IShapes shapes = moSpreadsheet.ActiveWorksheet.Shapes;

                #region  " -- Chart Area -- "

                //-- Background color
                if (this._GraphAppearence.ChartArea.FontTemplate.ShowBackColor)
                {
                    shapes[0].Chart.ChartArea.Format.Fill.Solid();
                    shapes[0].Chart.ChartArea.Format.Fill.BackColor.SchemeColor = 0;
                    shapes[0].Chart.ChartArea.Format.Fill.BackColor.ThemeColor = SpreadsheetGear.Themes.ColorSchemeIndex.None;
                    shapes[0].Chart.ChartArea.Format.Fill.BackColor.RGB = this._GraphAppearence.ChartArea.FontTemplate.BackColor;//this.GetSpreadsheetColor(this._GraphAppearence.ChartArea.FontTemplate.BackColor);
                    shapes[0].Chart.ChartArea.Format.Fill.Solid();
                }

                //-- Border color
                if (this._GraphAppearence.ChartArea.FontTemplate.BorderStyle != FontSetting.CellBorderStyle.None)
                {
                    shapes[0].Chart.ChartArea.Format.Line.BackColor.RGB = ColorTranslator.FromHtml(this._GraphAppearence.ChartArea.FontTemplate.BorderColor); // this.GetSpreadsheetColor(ColorTranslator.FromHtml());
                }

                #endregion

                #region  " -- Plot Area -- "

                ////-- Background color
                //if (this._GraphAppearence.PlotArea.FontTemplate.ShowBackColor)
                //{
                //    shapes[0].Chart.PlotArea.Format.Fill.Solid();
                //    shapes[0].Chart.PlotArea.Format.Fill.BackColor.RGB = this.GetSpreadsheetColor(this._GraphAppearence.PlotArea.FontTemplate.BackColor);
                //}

                ////-- Border color
                //if (this._GraphAppearence.PlotArea.FontTemplate.BorderStyle != FontSetting.CellBorderStyle.None)
                //{
                //    shapes[0].Chart.PlotArea.Format.Line.BackColor.RGB = this.GetSpreadsheetColor(ColorTranslator.FromHtml(this._GraphAppearence.PlotArea.FontTemplate.BorderColor));
                //}

                #endregion

                #region  " -- Legend -- "

                shapes[0].Chart.HasLegend = this._GraphAppearence.Legends.Show;


                if (this._GraphAppearence.Legends.Show)
                {
                    //-- Background color
                    if (this._GraphAppearence.Legends.FontTemplate.ShowBackColor)
                    {
                        shapes[0].Chart.Legend.Format.Fill.BackColor.RGB = this.GetSpreadsheetColor(this._GraphAppearence.Legends.FontTemplate.BackColor);
                    }

                    //-- Position
                    switch (this._GraphAppearence.Legends.ElementPosition)
                    {
                        case LegendPosition.Bottom:
                            shapes[0].Chart.Legend.Position = LegendPosition.Bottom;
                            break;
                        case LegendPosition.Corner:
                            break;
                        case LegendPosition.Custom:
                            break;
                        case LegendPosition.Left:
                            shapes[0].Chart.Legend.Position = LegendPosition.Left;
                            break;
                        case LegendPosition.Right:
                            shapes[0].Chart.Legend.Position = LegendPosition.Right;
                            break;
                        case LegendPosition.Top:
                            shapes[0].Chart.Legend.Position = LegendPosition.Top;
                            break;
                        default:
                            break;
                    }      

                    shapes[0].Chart.Legend.Font.Name = this._GraphAppearence.Legends.FontTemplate.FontName;
                    shapes[0].Chart.Legend.Font.Size = this._GraphAppearence.Legends.FontTemplate.FontSize;
                    shapes[0].Chart.Legend.Font.Color = this.GetSpreadsheetColor(this._GraphAppearence.Legends.FontTemplate.ForeColor);

                    //-- Font style
                    switch (this._GraphAppearence.Legends.FontTemplate.FontStyle)
                    {
                        case FontStyle.Bold:
                            shapes[0].Chart.Legend.Font.Bold = true;
                            shapes[0].Chart.Legend.Font.Italic = false;
                            shapes[0].Chart.Legend.Font.Underline = UnderlineStyle.None;
                            break;
                        case FontStyle.Italic:
                            shapes[0].Chart.Legend.Font.Bold = false;
                            shapes[0].Chart.Legend.Font.Italic = true;
                            shapes[0].Chart.Legend.Font.Underline = UnderlineStyle.None;
                            break;
                        case FontStyle.Regular:
                            shapes[0].Chart.Legend.Font.Bold = false;
                            shapes[0].Chart.Legend.Font.Italic = false;
                            shapes[0].Chart.Legend.Font.Underline = UnderlineStyle.None;
                            break;
                        case FontStyle.Strikeout:
                            break;
                        case FontStyle.Underline:
                            shapes[0].Chart.Legend.Font.Bold = false;
                            shapes[0].Chart.Legend.Font.Italic = false;
                            shapes[0].Chart.Legend.Font.Underline = UnderlineStyle.Single;
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                #region  " -- Y Axis -- "

                if (this._SpreadsheetGearChartType != SpreadsheetGear.Charts.ChartType.Pie)
                {

                    if (!this._GraphAppearence.YAxis.Show)
                    {
                        shapes[0].Chart.Axes[AxisType.Value].TickLabelPosition = TickLabelPosition.None;
                    }
                    shapes[0].Chart.Axes[AxisType.Value].HasMajorGridlines = false;

                    switch (this._GraphAppearence.YAxis.Orientation)
                    {
                        case TextOrientation.Custom:
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Orientation = this._GraphAppearence.YAxis.OrientationAngle;
                            break;
                        case TextOrientation.Horizontal:
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Orientation = SpreadsheetGear.Charts.TickLabelOrientation.Horizontal;
                            break;
                        case TextOrientation.VerticalLeftFacing:
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Orientation = SpreadsheetGear.Charts.TickLabelOrientation.Downward;
                            break;
                        case TextOrientation.VerticalRightFacing:
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Orientation = SpreadsheetGear.Charts.TickLabelOrientation.Upward;
                            break;
                        default:
                            break;
                    }


                    shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Color = this.GetSpreadsheetColor(this._GraphAppearence.YAxis.FontTemplate.ForeColor);
                    shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Name = this._GraphAppearence.YAxis.FontTemplate.FontName;
                    shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Size = this._GraphAppearence.YAxis.FontTemplate.FontSize;

                    //-- Font style
                    switch (this._GraphAppearence.YAxis.FontTemplate.FontStyle)
                    {
                        case FontStyle.Bold:
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Bold = true;
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Italic = false;
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Underline = UnderlineStyle.None;
                            break;
                        case FontStyle.Italic:
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Bold = false;
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Italic = true;
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Underline = UnderlineStyle.None;
                            break;
                        case FontStyle.Regular:
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Bold = false;
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Italic = false;
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Underline = UnderlineStyle.None;
                            break;
                        case FontStyle.Strikeout:
                            break;
                        case FontStyle.Underline:
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Bold = false;
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Italic = false;
                            shapes[0].Chart.Axes[AxisType.Value].TickLabels.Font.Underline = UnderlineStyle.Single;
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                #region  " -- X Axis -- "

                if (this._SpreadsheetGearChartType != SpreadsheetGear.Charts.ChartType.Pie)
                {
                    if (!this._GraphAppearence.XAxis.Show)
                    {
                        shapes[0].Chart.Axes[AxisType.Category].TickLabelPosition = TickLabelPosition.None;
                    }
                    shapes[0].Chart.Axes[AxisType.Category].HasMajorGridlines = false;

                    switch (this._GraphAppearence.XAxis.Orientation)
                    {
                        case TextOrientation.Custom:
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Orientation = this._GraphAppearence.XAxis.OrientationAngle;
                            break;
                        case TextOrientation.Horizontal:
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Orientation = SpreadsheetGear.Charts.TickLabelOrientation.Horizontal;
                            break;
                        case TextOrientation.VerticalLeftFacing:
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Orientation = SpreadsheetGear.Charts.TickLabelOrientation.Downward;
                            break;
                        case TextOrientation.VerticalRightFacing:
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Orientation = SpreadsheetGear.Charts.TickLabelOrientation.Upward;
                            break;
                        default:
                            break;
                    }


                    shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Color = this.GetSpreadsheetColor(this._GraphAppearence.XAxis.FontTemplate.ForeColor);
                    shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Name = this._GraphAppearence.XAxis.FontTemplate.FontName;
                    shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Size = this._GraphAppearence.XAxis.FontTemplate.FontSize;

                    //-- Font style
                    switch (this._GraphAppearence.XAxis.FontTemplate.FontStyle)
                    {
                        case FontStyle.Bold:
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Bold = true;
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Italic = false;
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Underline = UnderlineStyle.None;
                            break;
                        case FontStyle.Italic:
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Bold = false;
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Italic = true;
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Underline = UnderlineStyle.None;
                            break;
                        case FontStyle.Regular:
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Bold = false;
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Italic = false;
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Underline = UnderlineStyle.None;
                            break;
                        case FontStyle.Strikeout:
                            break;
                        case FontStyle.Underline:
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Bold = false;
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Italic = false;
                            shapes[0].Chart.Axes[AxisType.Category].TickLabels.Font.Underline = UnderlineStyle.Single;
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                #region  " -- Grid -- "

                if (this._SpreadsheetGearChartType != SpreadsheetGear.Charts.ChartType.Pie)
                {
                    if (this._GraphAppearence.Grid.Show)
                    {
                        shapes[0].Chart.Axes[AxisType.Value].HasMajorGridlines = true;
                        shapes[0].Chart.Axes[AxisType.Value].MajorGridlines.Format.Line.ForeColor.RGB = this.GetSpreadsheetColor(ColorTranslator.FromHtml(this._GraphAppearence.Grid.LineColor));
                        shapes[0].Chart.Axes[AxisType.Value].MajorGridlines.Format.Line.Weight = this._GraphAppearence.Grid.LineWidth * .20;
                    }
                }

                #endregion

                #region  " -- Title -- "
                if (this._GraphAppearence.TitleSetting.Show == true)
                {
                    shapes[0].Chart.HasTitle = true;
                    shapes[0].Chart.ChartTitle.Text = this._TablePresentation.Title + " " + "\n" + " " + this._TablePresentation.Subtitle;
                    shapes[0].Chart.ChartTitle.Orientation = SpreadsheetGear.Charts.TickLabelOrientation.Horizontal;
                    shapes[0].Chart.ChartTitle.IncludeInLayout = true;

                    //-- Background color
                    if (this._GraphAppearence.TitleSetting.FontTemplate.ShowBackColor)
                    {
                    }

                    shapes[0].Chart.ChartTitle.Font.Name = this._GraphAppearence.TitleSetting.FontTemplate.FontName;
                    shapes[0].Chart.ChartTitle.Font.Size = this._GraphAppearence.TitleSetting.FontTemplate.FontSize;
                    shapes[0].Chart.ChartTitle.Font.Color = this.GetSpreadsheetColor(this._GraphAppearence.TitleSetting.FontTemplate.ForeColor);

                    //-- Font style
                    switch (this._GraphAppearence.TitleSetting.FontTemplate.FontStyle)
                    {
                        case FontStyle.Bold:
                            shapes[0].Chart.ChartTitle.Font.Bold = true;
                            shapes[0].Chart.ChartTitle.Font.Italic = false;
                            shapes[0].Chart.ChartTitle.Font.Underline = UnderlineStyle.None;
                            break;
                        case FontStyle.Italic:
                            shapes[0].Chart.ChartTitle.Font.Bold = false;
                            shapes[0].Chart.ChartTitle.Font.Italic = true;
                            shapes[0].Chart.ChartTitle.Font.Underline = UnderlineStyle.None;
                            break;
                        case FontStyle.Regular:
                            shapes[0].Chart.ChartTitle.Font.Bold = false;
                            shapes[0].Chart.ChartTitle.Font.Italic = false;
                            shapes[0].Chart.ChartTitle.Font.Underline = UnderlineStyle.None;
                            break;
                        case FontStyle.Strikeout:
                            break;
                        case FontStyle.Underline:
                            shapes[0].Chart.ChartTitle.Font.Bold = false;
                            shapes[0].Chart.ChartTitle.Font.Italic = false;
                            shapes[0].Chart.ChartTitle.Font.Underline = UnderlineStyle.Single;
                            break;
                        default:
                            break;
                    }   
                }               

                #endregion

            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Set the aspect ratio of the image
        /// </summary>
        /// <param name="ExcelBitmap"></param>
        private void SetAspectRatio()
        {
            //-- Set the aspect ratio.
            Single ImageAspectRatio = 1;

            SpreadsheetGear.Charts.IChart objChart = null;
            SpreadsheetGear.Shapes.IShapes shapes = moSpreadsheet.ActiveWorksheet.Shapes;
            objChart = shapes[0].Chart;

            //ImageAspectRatio = Convert.ToSingle(shapes[0].Width / shapes[0].Height);
            //if (ImageAspectRatio > 1)
            //{
            //    this._Width = Convert.ToInt32(Convert.ToSingle(this._Width) / ImageAspectRatio);
            //}
            //else
            //{
            //    this._Height = Convert.ToInt32(Convert.ToSingle(this._Height) * ImageAspectRatio);
            //}

            shapes[0].Width = (this._Width * 74) / 100;
            shapes[0].Height = (this._Height * 75) / 100;
        }

        #endregion

        #endregion

        #region " -- Public / Friend -- "

        #region " -- Constants -- "

        /// <summary>
        /// GraphExcelTemplate.xls
        /// </summary>
        public const string EXCEL_FILE_TEMPLATE = "GraphExcelTemplate.xls";

        /// <summary>
        /// GraphNonExcelTemplate.xls
        /// </summary>
        public const string NON_EXCEL_FILE_TEMPLATE = "GraphNonExcelTemplate.xls";

        /// <summary>
        /// Chart.crtx
        /// </summary>
        public const string GRAPH_TEMPLATE = "Chart.crtx";

        /// <summary>
        /// DataTimeStamp in the format ::->MMM-dd-yyyy hh_mm_ss_fff tt
        /// </summary>            
        public const string IMAGEDATETIMESTAMP = "MMM-dd-yyyy_hh_mm_ss_fff_tt";

        /// <summary>
        /// PNG
        /// </summary>
        public const string IMAGEEXTENTION = ".png";

        /// <summary>
        /// Constant for Language string of datavalue used to set caption of field object.
        /// </summary>
        public const string DATAVALUE = "Data Value";

        #endregion

        #region "-- Constructor --"

        /// <summary>
        /// Initilize the object for Graph
        /// </summary>
        /// <param name="presentationData">DataView</param>
        /// <param name="DIConnection">DIConnection used in IC Classification of step 3 & its implementation in step 6 and also for footnotes text and comments text in step 6</param>
        /// <param name="DIQueries">DIQueries</param>
        /// <param name="UserSelection">UserSelection2 (New UserSelection)</param>
        /// <remarks>
        /// This class using Table presenation class for generating datasource
        /// </remarks>
        public GraphPresentation(DIDataView dIDataView, DIConnection dIConnection, DIQueries dIQueries, UserPreference.UserPreference userPreference)
        {

            // - Create object of TablePresentation
            this._TablePresentation = new TablePresentation(dIDataView, dIConnection, dIQueries, userPreference, string.Empty);

            // - Set the Presenatation Type
            this._TablePresentation.PresentationType = Presentation.PresentationType.Graph;

            // - Initilize the graph appearence oject
            this._GraphAppearence = new StyleTemplate(Presentation.PresentationType.Graph, userPreference.General.ShowExcel);

            // Set the user pref for internal use
            this.UserPreference = userPreference;

            this._SpreadsheetGearChartType = GetSpreadsheetChartTypeFromXlChartType(this.UserPreference.Chart.ChartType);

            // Initilize the all the language strings
            this.ApplyLanguageSettings();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dIDataView"></param>
        /// <param name="dIConnection"></param>
        /// <param name="dIQueries"></param>
        /// <param name="userPreference"></param>
        /// <param name="outputFolderPath"></param>
        /// <param name="outputType"></param>
        public GraphPresentation(DIDataView dIDataView, DIConnection dIConnection, DIQueries dIQueries, UserPreference.UserPreference userPreference, string outputFolderPath, PresentationOutputType outputType)
        {

            // - Create object of TablePresentation
            this._TablePresentation = new TablePresentation(dIDataView, dIConnection, dIQueries, userPreference, string.Empty);

            // - Set the Presenatation Type
            this._TablePresentation.PresentationType = Presentation.PresentationType.Graph;

            this._TablePresentation.ShowExcel = userPreference.General.ShowExcel;

            // - Initilize the graph appearence oject
            this._GraphAppearence = new StyleTemplate(Presentation.PresentationType.Graph, userPreference.General.ShowExcel);

            if (!userPreference.General.ShowExcel)
            {
                if (outputType == PresentationOutputType.ExcellSheet)
                {
                    this._TablePresentation.PresentationOutputType = PresentationOutputType.ExcellSheet;
                }
                else
                {
                    this._TablePresentation.PresentationOutputType = PresentationOutputType.MHT;
                }

                // Set the user pref.
                this.UserPreference = userPreference;

                this._SpreadsheetGearChartType = GetSpreadsheetChartTypeFromXlChartType(this.UserPreference.Chart.ChartType);

                // Initilize the all the language strings
                this.ApplyLanguageSettings();
            }
        }

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public GraphPresentation()
        {
            // Do Nothing -- only for serialization
        }


        #endregion

        #region " -- Properties -- "

        /// <summary>
        /// Hold the object of TablePresenation class
        /// </summary>
        private TablePresentation _TablePresentation;
        /// <summary>
        /// Gets the TablePresentation class object.
        /// </summary>
        public TablePresentation TablePresentation
        {
            get { return _TablePresentation; }
            set
            {
                _TablePresentation = value;
            }
        }

        private string _PresentationPath = string.Empty;
        /// <summary>
        /// Gets the presentation file name.
        /// </summary>
        public string PresentationPath
        {
            get
            {
                return this._PresentationPath;
            }
        }

        private StyleTemplate _GraphAppearence;
        /// <summary>
        /// Get or sets the graph appearence
        /// </summary>
        public StyleTemplate GraphAppearence
        {
            get
            {
                return this._GraphAppearence;
            }
            set
            {
                this._GraphAppearence = value;
            }
        }

        private int _Height = 300;
        /// <summary>
        /// Image Height
        /// </summary>
        public int Height
        {
            get { return _Height; }
            set { _Height = value; }
        }

        private int _Width = 400;
        /// <summary>
        /// Image Width
        /// </summary>
        public int Width
        {
            get { return _Width; }
            set { _Width = value; }
        }

        private string _BgColor = "#F0F0EF";
        /// <summary>
        /// BorderColor 
        /// </summary>
        public string BgColor
        {
            get { return _BgColor; }
            set { _BgColor = value; }
        }

        private int _BorderCornerRadius = 0;
        /// <summary>
        /// BorderCornerRadius 
        /// </summary>
        public int BorderCornerRadius
        {
            get { return _BorderCornerRadius; }
            set { _BorderCornerRadius = value; }
        }

        private SpreadsheetGear.Charts.ChartType _SpreadsheetGearChartType = SpreadsheetGear.Charts.ChartType.BarClustered;
        /// <summary>
        /// Gets or sets the spreadsheet gear chart type
        /// </summary>
        public SpreadsheetGear.Charts.ChartType SpreadsheetGearChartType
        {
            get
            {
                return this._SpreadsheetGearChartType;
            }
            set
            {
                this._SpreadsheetGearChartType = value;
            }
        }

        private ObjectComparison _GraphObjectComparison = ObjectComparison.All;
        /// <summary>
        /// Gets or sets the object comparion for equal method
        /// </summary>
        [XmlIgnore()]
        public ObjectComparison GraphObjectComparison
        {
            get
            {
                return this._GraphObjectComparison;
            }
            set
            {
                this._GraphObjectComparison = value;
            }
        }

        private string _GraphPresentationFileNameWPath = string.Empty;
        /// <summary>
        /// Gets the graph presentation file name with path
        /// </summary>
        public string GraphPresentationFileNameWPath
        {
            get 
            {
                return this._GraphPresentationFileNameWPath; 
            }
        }
	

        #endregion

        #region " -- Method -- "

        public Dictionary<string, string> GetPyramidSeries(string presentationFileNameWPath, ChartMode chartMode)
        {
            Dictionary<string, string> RetVal = new Dictionary<string, string>();
            try
            {
                //-- Set the show excel to true so thatcolumn will not merged into the column for pyramid chart.
                bool ShowExcel = this._TablePresentation.ShowExcel;
                this._TablePresentation.ShowExcel = true;

                //-- Generate the table, data, source and keyword worksheet
                this._TablePresentation.GeneratePresentation(Path.GetDirectoryName(presentationFileNameWPath), Path.GetFileName(presentationFileNameWPath));
                RetVal = this.PyramidChart.GetSeries(this.TablePresentation.Fields, presentationFileNameWPath, chartMode, this._TablePresentation.ColumnArrangementTable.Rows.Count);

                //-- Reset the showExcel status
                this._TablePresentation.ShowExcel = ShowExcel;
                this._GraphPresentationFileNameWPath = presentationFileNameWPath;

                if (!this._TablePresentation.ShowExcel)
                {
                    this.ApplyTemplate(presentationFileNameWPath);
                }
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Generate graph presentation.
        /// </summary>
        /// <param name="PresentationOutputFolder"></param>
        /// <param name="presentationFileName"></param>
        /// <returns></returns>
        public string GenerateGraph(string PresentationOutputFolder, string presentationFileName)
        {
            string Retval = string.Empty;
            try
            {
                Retval = this._TablePresentation.GeneratePresentation(PresentationOutputFolder, presentationFileName);
                Retval = Path.Combine(PresentationOutputFolder, Retval);
                this.InsertChart(Retval);
                this._GraphPresentationFileNameWPath = Retval;
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;
        }

        /// <summary>
        /// Method Used to Generate Graph presentation
        /// </summary>
        /// <param name="PresentationOutputFolder"></param>
        /// <returns> </returns>
        public string GenerateGraph(string PresentationOutputFolder)
        {
            string Retval = string.Empty;
            try
            {
                Retval = this.GenerateGraph(PresentationOutputFolder, string.Empty);
            }
            catch (Exception)
            {
                Retval = string.Empty;
            }
            return Retval;            
        }

        /// <summary>
        ///  Method Used to Generate the Graph Image
        /// </summary>
        /// <returns>Image object</returns>
        public Image GenerateGraphImage(string PresentationOutputFolder, string presentationFileName)
        {
            Image RetVal = null;
            try
            {            
                presentationFileName = this.GenerateGraph(PresentationOutputFolder, presentationFileName);                
                RetVal = this.GenerateImage();
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        ///  Method Used to Generate the Graph Image
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <returns></returns>
        public Image GenerateGraphImage(string fileNameWPath)
        {
            Image RetVal = null;
            try
            {
                this.LoadWorkbook(fileNameWPath);
                RetVal = this.GenerateImage();                
            }
            catch (Exception)
            {
            }
            return RetVal;
        }

        /// <summary>
        /// Generate graph image file
        /// </summary>
        /// <param name="PresentationOutputFolder"></param>
        /// <returns></returns>
        public bool GenerateGraphImageFile(string PresentationOutputFolder, string presentationFileName, string imageFileNameWPath)
        {
            bool Retval = false;
            try
            {
                Image GraphImage;
                presentationFileName = this.GenerateGraph(PresentationOutputFolder, presentationFileName);
                GraphImage = this.GenerateImage();
                GraphImage.Save(imageFileNameWPath);
                Retval = true;
            }
            catch (Exception)
            {
                
            }
            return Retval;    
        }

        /// <summary>
        /// Generate graph image file
        /// </summary>
        /// <param name="excelFileNameWPath"></param>
        /// <returns></returns>
        public bool GenerateGraphImageFromExcel(string excelFileNameWPath, string imageFileNameWPath)
        {
            bool Retval = false;
            try
            {
                Image GraphImage;
                this.LoadWorkbook(excelFileNameWPath);
                GraphImage = this.GenerateImage();
                GraphImage.Save(imageFileNameWPath);
                Retval = true;
            }
            catch (Exception)
            {

            }
            return Retval;
        }

        /// <summary>
        /// Create chart and redfine the data source.
        /// </summary>
        /// <param name="prestnationFilenameWPath"></param>
        public void CreateChartAndSetDataRange(string prestnationFilenameWPath)
        {
            try
            {
                string PresentationFileName = prestnationFilenameWPath;
                //-- Generate the presentation
                prestnationFilenameWPath = this._TablePresentation.GeneratePresentation(Path.GetDirectoryName(prestnationFilenameWPath), Path.GetFileName(prestnationFilenameWPath));
                this._GraphPresentationFileNameWPath = PresentationFileName;
                prestnationFilenameWPath = PresentationFileName;

                this.LoadWorkbook(prestnationFilenameWPath);
                this.moSpreadsheet.GetLock();
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
                SpreadsheetGear.IRange ChartSource = this.DefineChartRange(worksheet);
                objChart.SetSourceData(ChartSource, SpreadsheetGear.Charts.RowCol.Columns);

                if (this._SpreadsheetGearChartType != SpreadsheetGear.Charts.ChartType.Pie)
                {
                    DataRow Row = this._TablePresentation.TableXLS.Rows[0];
                    for (int i = 0; i < objChart.SeriesCollection.Count; i++)
                    {
                        objChart.SeriesCollection[i].Name = Row[i + 1].ToString();
                    }
                }

                this.moSpreadsheet.ActiveWorkbook.Save();
                this.moSpreadsheet.ActiveWorkbook.Close();
                this.moSpreadsheet.ReleaseLock();      
            }
            catch (Exception)
            {
            }
        } 

        /// <summary>
        /// Create the chart in the spreadsheet workbook.
        /// </summary>
        /// <param name="prestnationFilenameWPath"></param>
        public void SetDataRange(string prestnationFilenameWPath)
        {
            try
            {
                this.LoadWorkbook(prestnationFilenameWPath);

                this.moSpreadsheet.GetLock();
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

                objChart = (SpreadsheetGear.Charts.IChart)shapes[0];
                SpreadsheetGear.IRange ChartSource = this.DefineChartRange(worksheet);
                objChart.SetSourceData(ChartSource, SpreadsheetGear.Charts.RowCol.Columns);
                if (this._SpreadsheetGearChartType != SpreadsheetGear.Charts.ChartType.Pie)
                {
                    DataRow Row = this._TablePresentation.TableXLS.Rows[0];
                    for (int i = 0; i < objChart.SeriesCollection.Count; i++)
                    {
                        objChart.SeriesCollection[i].Name = Row[i + 1].ToString();
                    }
                }
                this.moSpreadsheet.ActiveWorkbook.Save();
                this.moSpreadsheet.ActiveWorkbook.Close();
                this.moSpreadsheet.ReleaseLock();
            }
            catch (Exception)
            {
            }
        }

        public void ApplyTemplate(string presentationFileNameWPath)
        {
            this.LoadWorkbook(presentationFileNameWPath);
            this.moSpreadsheet.GetLock();
            this.ApplyTemplate();
            this.moSpreadsheet.ActiveWorkbook.Save();
            this.moSpreadsheet.ReleaseLock();
        }

        /// <summary>
        /// Update the title of chart.
        /// </summary>
        public void UpdateTitle(string prestnationFilenameWPath)
        {
            this.LoadWorkbook(prestnationFilenameWPath);
            this.moSpreadsheet.GetLock();

            this.moSpreadsheet.ActiveWorkbook.Sheets[DILanguage.GetLanguageString("Graph")].Select();
            SpreadsheetGear.Shapes.IShapes shapes = moSpreadsheet.ActiveWorksheet.Shapes;
            shapes[0].Chart.HasTitle = true;
            shapes[0].Chart.ChartTitle.Text = this._TablePresentation.Title + " " + "\n" + " " + this._TablePresentation.Subtitle;
            this.moSpreadsheet.ActiveWorkbook.Save();

            this.moSpreadsheet.ReleaseLock();
        }

        /// <summary>
        /// Get XlChartType against spreadsheet ChartType
        /// </summary>
        /// <param name="InfragisticsChartType"></param>
        /// <returns></returns>
        public static int GetXlChartTypeFromSpreadsheetChartType(SpreadsheetGear.Charts.ChartType SpreadsheetChartType)
        {
            int RetVal = 51;
            switch (SpreadsheetChartType)
            {
                case SpreadsheetGear.Charts.ChartType.Area:
                    break;
                case SpreadsheetGear.Charts.ChartType.Area3D:
                    break;
                case SpreadsheetGear.Charts.ChartType.AreaStacked:
                    break;
                case SpreadsheetGear.Charts.ChartType.AreaStacked100:
                    break;
                case SpreadsheetGear.Charts.ChartType.AreaStacked1003D:
                    break;
                case SpreadsheetGear.Charts.ChartType.AreaStacked3D:
                    break;
                case SpreadsheetGear.Charts.ChartType.BarClustered:
                    RetVal = 57;
                    break;
                case SpreadsheetGear.Charts.ChartType.BarClustered3D:
                    break;
                case SpreadsheetGear.Charts.ChartType.BarOfPie:
                    break;
                case SpreadsheetGear.Charts.ChartType.BarStacked:
                    //-- We are using BarClustered for creating the pyramid chard. As we already creating Bar chart, so we use different Bar chart type for pyrmaid chart.
                    RetVal = 58;
                    break;
                case SpreadsheetGear.Charts.ChartType.BarStacked100:
                    break;
                case SpreadsheetGear.Charts.ChartType.BarStacked1003D:
                    break;
                case SpreadsheetGear.Charts.ChartType.BarStacked3D:
                    break;
                case SpreadsheetGear.Charts.ChartType.Bubble:
                    break;
                case SpreadsheetGear.Charts.ChartType.Bubble3DEffect:
                    break;
                case SpreadsheetGear.Charts.ChartType.Column3D:
                    break;
                case SpreadsheetGear.Charts.ChartType.ColumnClustered:
                    RetVal = 51;
                    break;
                case SpreadsheetGear.Charts.ChartType.ColumnClustered3D:
                    break;
                case SpreadsheetGear.Charts.ChartType.ColumnStacked:
                    break;
                case SpreadsheetGear.Charts.ChartType.ColumnStacked100:
                    break;
                case SpreadsheetGear.Charts.ChartType.ColumnStacked1003D:
                    break;
                case SpreadsheetGear.Charts.ChartType.ColumnStacked3D:
                    break;
                case SpreadsheetGear.Charts.ChartType.Combination:
                    break;
                case SpreadsheetGear.Charts.ChartType.ConeBarClustered:
                    break;
                case SpreadsheetGear.Charts.ChartType.ConeBarStacked:
                    break;
                case SpreadsheetGear.Charts.ChartType.ConeBarStacked100:
                    break;
                case SpreadsheetGear.Charts.ChartType.ConeCol:
                    break;
                case SpreadsheetGear.Charts.ChartType.ConeColClustered:
                    break;
                case SpreadsheetGear.Charts.ChartType.ConeColStacked:
                    break;
                case SpreadsheetGear.Charts.ChartType.ConeColStacked100:
                    break;
                case SpreadsheetGear.Charts.ChartType.CylinderBarClustered:
                    break;
                case SpreadsheetGear.Charts.ChartType.CylinderBarStacked:
                    break;
                case SpreadsheetGear.Charts.ChartType.CylinderBarStacked100:
                    break;
                case SpreadsheetGear.Charts.ChartType.CylinderCol:
                    break;
                case SpreadsheetGear.Charts.ChartType.CylinderColClustered:
                    break;
                case SpreadsheetGear.Charts.ChartType.CylinderColStacked:
                    break;
                case SpreadsheetGear.Charts.ChartType.CylinderColStacked100:
                    break;
                case SpreadsheetGear.Charts.ChartType.Doughnut:
                    break;
                case SpreadsheetGear.Charts.ChartType.DoughnutExploded:
                    break;
                case SpreadsheetGear.Charts.ChartType.Line:
                    RetVal = 4;
                    break;
                case SpreadsheetGear.Charts.ChartType.Line3D:
                    break;
                case SpreadsheetGear.Charts.ChartType.LineMarkers:
                    break;
                case SpreadsheetGear.Charts.ChartType.LineMarkersStacked:
                    break;
                case SpreadsheetGear.Charts.ChartType.LineMarkersStacked100:
                    break;
                case SpreadsheetGear.Charts.ChartType.LineStacked:
                    break;
                case SpreadsheetGear.Charts.ChartType.LineStacked100:
                    break;
                case SpreadsheetGear.Charts.ChartType.Pie:
                    RetVal = 5;
                    break;
                case SpreadsheetGear.Charts.ChartType.Pie3D:
                    break;
                case SpreadsheetGear.Charts.ChartType.PieExploded:
                    break;
                case SpreadsheetGear.Charts.ChartType.PieExploded3D:
                    break;
                case SpreadsheetGear.Charts.ChartType.PieOfPie:
                    break;
                case SpreadsheetGear.Charts.ChartType.PyramidBarClustered:
                    break;
                case SpreadsheetGear.Charts.ChartType.PyramidBarStacked:
                    break;
                case SpreadsheetGear.Charts.ChartType.PyramidBarStacked100:
                    break;
                case SpreadsheetGear.Charts.ChartType.PyramidCol:
                    break;
                case SpreadsheetGear.Charts.ChartType.PyramidColClustered:
                    break;
                case SpreadsheetGear.Charts.ChartType.PyramidColStacked:
                    break;
                case SpreadsheetGear.Charts.ChartType.PyramidColStacked100:
                    break;
                case SpreadsheetGear.Charts.ChartType.Radar:
                    break;
                case SpreadsheetGear.Charts.ChartType.RadarFilled:
                    break;
                case SpreadsheetGear.Charts.ChartType.RadarMarkers:
                    break;
                case SpreadsheetGear.Charts.ChartType.StockHLC:
                    break;
                case SpreadsheetGear.Charts.ChartType.StockOHLC:
                    break;
                case SpreadsheetGear.Charts.ChartType.StockVHLC:
                    break;
                case SpreadsheetGear.Charts.ChartType.StockVOHLC:
                    break;
                case SpreadsheetGear.Charts.ChartType.Surface:
                    break;
                case SpreadsheetGear.Charts.ChartType.SurfaceTopView:
                    break;
                case SpreadsheetGear.Charts.ChartType.SurfaceTopViewWireframe:
                    break;
                case SpreadsheetGear.Charts.ChartType.SurfaceWireframe:
                    break;
                case SpreadsheetGear.Charts.ChartType.XYScatter:
                    break;
                case SpreadsheetGear.Charts.ChartType.XYScatterLines:
                    break;
                case SpreadsheetGear.Charts.ChartType.XYScatterLinesNoMarkers:
                    break;
                case SpreadsheetGear.Charts.ChartType.XYScatterSmooth:
                    break;
                case SpreadsheetGear.Charts.ChartType.XYScatterSmoothNoMarkers:
                    break;
                default:
                    break;
            }
            return RetVal;
        }

        /// <summary>
        /// Get Spreadshhet ChartType against XlChartType
        /// </summary>
        /// <param name="ExcelChartType"></param>
        /// <returns></returns>
        public static SpreadsheetGear.Charts.ChartType GetSpreadsheetChartTypeFromXlChartType(int ExcelChartType)
        {
            SpreadsheetGear.Charts.ChartType RetVal = SpreadsheetGear.Charts.ChartType.ColumnClustered;
            switch (ExcelChartType)
            {
                case 51:
                    RetVal = SpreadsheetGear.Charts.ChartType.ColumnClustered;
                    break;
                case 57:
                    RetVal = SpreadsheetGear.Charts.ChartType.BarClustered;
                    break;
                case 4:
                    RetVal = SpreadsheetGear.Charts.ChartType.Line;
                    break;
                case 5:
                    RetVal = SpreadsheetGear.Charts.ChartType.Pie;
                    break;
                case 58:
                    //-- We are using BarClustered for creating the pyramid chard. As we already creating Bar chart, so we use different Bar chart type for pyrmaid chart.
                    RetVal = SpreadsheetGear.Charts.ChartType.BarStacked;
                    break;
            }
            return RetVal;
        }

        /// <summary>
        /// Get the Collection for the Locations
        /// </summary>
        /// <returns>Dictionary colection</returns>
        ///<remark>
        /// For Binding DataTextFied will br "key"
        /// DataValueFied will br "value"
        /// </remark>
        public Dictionary<String, String> GetLocations()
        {
            Dictionary<String, String> RetVal = new Dictionary<string, string>();
            RetVal.Add(LanguageSettings.TOP, ((int)LegendPosition.Top).ToString());
            RetVal.Add(LanguageSettings.BOTTOM, ((int)LegendPosition.Bottom).ToString());
            RetVal.Add(LanguageSettings.RIGHT, ((int)LegendPosition.Right).ToString());
            RetVal.Add(LanguageSettings.LEFT, ((int)LegendPosition.Left).ToString());
            return RetVal;
        }

        /// <summary>
        /// Get the Collection for the Font Size
        /// </summary>
        /// <returns>Dictionary colection</returns>
        ///<remark>
        /// For Binding DataTextFied will br "key"
        /// DataValueFied will br "value"
        /// </remark>
        public Dictionary<String, String> GetFontSizeRange()
        {
            Dictionary<String, String> RetVal = new Dictionary<string, string>();
            for (int i = 1; i < 45; i++)
            {
                RetVal.Add(i.ToString(), i.ToString());
            }
            return RetVal;
        }

        /// <summary>
        /// Get the Collection for the BorderWidthRange
        /// </summary>
        /// <returns>Dictionary colection</returns>
        ///<remark>
        /// For Binding DataTextFied will br "key"
        /// DataValueFied will br "value"
        /// </remark>
        public Dictionary<String, String> GetBorderWidthRange()
        {
            Dictionary<String, String> RetVal = new Dictionary<string, string>();
            for (int i = 0; i < 10; i++)
            {
                RetVal.Add(i.ToString(), i.ToString());
            }
            return RetVal;
        }

        /// <summary>
        /// Update the excel chart type.
        /// </summary>
        /// <param name="chartType"></param>
        public void UpdateChart(string filenameWpath, SpreadsheetGear.Charts.ChartType chartType)
        {
            try
            {
                this.LoadWorkbook(filenameWpath);
                this.moSpreadsheet.GetLock();
                SpreadsheetGear.Charts.IChart objChart;

                SpreadsheetGear.Shapes.IShapes shapes = moSpreadsheet.ActiveWorksheet.Shapes;
                objChart = (SpreadsheetGear.Charts.IChart)shapes[0].Chart;
                objChart.ChartType = chartType;
                this.moSpreadsheet.ActiveWorkbook.Save();
                this.moSpreadsheet.ActiveWorkbook.Close();
            }
            catch (Exception)
            {
            }
            finally
            {
                this.moSpreadsheet.ReleaseLock();
            }
        }

        #endregion

        #endregion

        #region"--Inner Class --"

        /// <summary>
        /// Contains the Constants for languageindependent string 
        /// </summary>
        private class LanguageSettings
        {

            /// <summary>
            /// #TOP
            /// </summary>
            public static string TOP = "#TOP";

            /// <summary>
            /// #BOTTOM
            /// </summary>
            public static string BOTTOM = "#BOTTOM";
            /// <summary>
            /// #RIGHT
            /// </summary>
            public static string RIGHT = "#RIGHT";
            /// <summary>
            /// #LEFT
            /// </summary>
            public static string LEFT = "#LEFT";
            /// <summary>
            /// #CENTER
            /// </summary>
            public static string CENTER = "#CENTER";
            /// <summary>
            /// #FAR
            /// </summary>
            public static string FAR = "#FAR";
            /// <summary>
            /// #NEAR
            /// </summary>
            public static string NEAR = "#NEAR";
            /// <summary>
            /// #SOLID
            /// </summary>
            public static string SOLID = "#SOLID";
            /// <summary>
            /// #DASH
            /// </summary>
            public static string DASH = "#DASH";
            /// <summary>
            /// #DOT
            /// </summary>
            public static string DOT = "#DOT";
            /// <summary>
            /// #DASHDOT
            /// </summary>
            public static string DASHDOT = "#DASHDOT";
            /// <summary>
            /// #DASHDOTDOT
            /// </summary>
            public static string DASHDOTDOT = "#DASHDOTDOT";
            /// <summary>
            /// #VerticalLeftFacing
            /// </summary>
            public static string VERTICALLEFTFACING = "#VerticalLeftFacing";
            /// <summary>
            /// #VerticalRightFacing
            /// </summary>
            public static string VERTICALRIGHTFACING = "#VerticalRightFacing";
            /// <summary>
            /// #Horizontal
            /// </summary>
            public static string HORIZONTAL = "#Horizontal";
            /// <summary>
            /// #Custom
            /// </summary>
            public static string CUSTOM = "#Custom";

            #region Chart Related Strings

            /// <summary>
            /// #ColumnChart
            /// </summary>
            public static string ColumnChart = "#ColumnChart";
            /// <summary>
            /// #ColumnChart
            /// </summary>
            public static string BarChart = "#ColumnChart";
            /// <summary>
            /// #AreaChart
            /// </summary>
            public static string AreaChart = "#AreaChart";
            /// <summary>
            /// #LineChart
            /// </summary>
            public static string LineChart = "#LineChart";
            /// <summary>
            /// #PieChart
            /// </summary>
            public static string PieChart = "#PieChart";
            /// <summary>
            /// #BubbleChart
            /// </summary>
            public static string BubbleChart = "#BubbleChart";
            /// <summary>
            /// #ColumnChart3D
            /// </summary>
            public static string ColumnChart3D = "#ColumnChart3D";
            /// <summary>
            /// #BarChart3D
            /// </summary>
            public static string BarChart3D = "#BarChart3D";
            /// <summary>
            /// #AreaChart3D
            /// </summary>
            public static string AreaChart3D = "#AreaChart3D";
            /// <summary>
            /// #LineChart3D
            /// </summary>
            public static string LineChart3D = "#LineChart3D";
            /// <summary>
            /// #PieChart3D
            /// </summary>
            public static string PieChart3D = "#PieChart3D";
            /// <summary>
            /// #StackColumnChart
            /// </summary>
            public static string StackColumnChart = "#StackColumnChart";
            /// <summary>
            /// #StackBarChart
            /// </summary>
            public static string StackBarChart = "#StackBarChart";
            /// <summary>
            /// #Stack3DColumnChart
            /// </summary>
            public static string Stack3DColumnChart = "#Stack3DColumnChart";
            /// <summary>
            /// #Stack3DBarChart
            /// </summary>
            public static string Stack3DBarChart = "#Stack3DBarChart";
            /// <summary>
            /// #ColumnLineChart
            /// </summary>
            public static string ColumnLineChart = "#ColumnLineChart";
            /// <summary>
            /// #StackAreaChart
            /// </summary>
            public static string StackAreaChart = "#StackAreaChart";
            /// <summary>
            /// #StackAreaChart
            /// </summary>
            public static string StackLineChart = "#StackLineChart";

            #endregion
        }

        #endregion

        #region " -- Save / Load -- "

        /// <summary>
        /// Save the GraphPresentation in form of XML file.
        /// </summary>
        /// <param name="fileNameWPath">File path of Serialized xml</param>        
        public void Save(string fileNameWPath)
        {
            try
            {
                XmlSerializer GraphSerialize = new XmlSerializer(typeof(GraphPresentation));
                StreamWriter GraphWriter = new StreamWriter(fileNameWPath);
                GraphSerialize.Serialize(GraphWriter, this);
                GraphWriter.Close();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Load the deserialize XML file
        /// </summary>
        /// <param name="fileNameWPath">File path of Serialized xml</param>
        /// <returns>Object of Table Presentation</returns>
        /// <remarks> This overloaded method does not update NIds from GIds. 
        /// If you want to update NIds from GIds, use Load method with three parameter.
        /// Call IntializeTablePresentstion after loading the report </remarks>
        public static GraphPresentation Load(string fileNameWPath)
        {
            GraphPresentation RetVal;
            try
            {
                XmlSerializer GraphSerialize = new XmlSerializer(typeof(GraphPresentation));
                TextReader GraphReader = new StreamReader(fileNameWPath);
                RetVal = (GraphPresentation)GraphSerialize.Deserialize(GraphReader);
                GraphReader.Close();
            }
            catch (Exception ex)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Get the serialized text using MemoryStream.
        /// </summary>
        /// <param name="updateGId">True, Conversion of NId to GId</param>
        /// <remarks>http://www.dotnetjohn.com/PrintFriend.aspx?articleid=173</remarks>
        /// <returns>serialized string</returns>
        public string GetSerializedText()
        {
            string RetVal = string.Empty;
            try
            {
                XmlSerializer GraphSerialize = new XmlSerializer(typeof(GraphPresentation));
                MemoryStream MemoryStream = new MemoryStream();
                //TODO UTF8 reason
                XmlTextWriter xmlTextWriter = new XmlTextWriter(MemoryStream, Encoding.UTF8);
                GraphSerialize.Serialize(xmlTextWriter, this);
                MemoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                RetVal = UTF8ByteArrayToString(MemoryStream.ToArray());
                MemoryStream.Close();
            }
            catch (Exception ex)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Load the deserialize XML text
        /// </summary>
        /// <param name="SerializeText"></param>
        /// <returns></returns>
        public static GraphPresentation LoadFromSerializeText(string SerializeText)
        {
            GraphPresentation RetVal;
            try
            {
                XmlSerializer GraphSerialize = new XmlSerializer(typeof(GraphPresentation));
                MemoryStream MemoryStream = new MemoryStream(StringToUTF8ByteArray(SerializeText));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(MemoryStream, Encoding.UTF8);
                RetVal = (GraphPresentation)GraphSerialize.Deserialize(MemoryStream);

                MemoryStream.Dispose();
            }
            catch (Exception ex)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Load the deserialize XML text
        /// </summary>
        /// <param name="SerializeText"></param>
        /// <returns></returns>
        public static GraphPresentation LoadFromSerializeText(string SerializeText, string languageFilePath, string maskFolderPath, string outputFolderPath)
        {
            GraphPresentation RetVal;
            Boolean IsInitilized = false;
            DIDataView objDIDataView;
            DIConnection objDIConnection;
            DIQueries objDIQueries;

            UserPreference.UserPreference objUserPreference;
            try
            {
                XmlSerializer GraphSerialize = new XmlSerializer(typeof(GraphPresentation));
                MemoryStream MemoryStream = new MemoryStream(StringToUTF8ByteArray(SerializeText));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(MemoryStream, Encoding.UTF8);
                RetVal = (GraphPresentation)GraphSerialize.Deserialize(MemoryStream);

                // Set the Essential Properties
                if (RetVal.TablePresentation.UserPreference.General.ShowExcel)
                {
                    RetVal.TablePresentation.PresentationOutputType = PresentationOutputType.ExcellSheet;
                }
                else
                {
                    RetVal.TablePresentation.PresentationOutputType = PresentationOutputType.MHT;
                }
                if (RetVal != null)
                {
                    objUserPreference = RetVal.TablePresentation.UserPreference;
                    //TODO: Remove hardcoding
                    maskFolderPath = Path.Combine(objUserPreference.General.AdaptationPath, @"Bin\Templates\Metadata\Mask");
                    objDIConnection = new DIConnection(objUserPreference.Database.SelectedConnectionDetail);

                    objDIQueries = new DIQueries(objUserPreference.Database.SelectedDatasetPrefix, objUserPreference.Database.DatabaseLanguage);

                    //-- TODO remove hardcoded

                    objDIDataView = new DevInfo.Lib.DI_LibBAL.UI.DataViewPage.DIDataView(objUserPreference, objDIConnection, objDIQueries, maskFolderPath, string.Empty);
                    objDIDataView.GetAllDataByUserSelection();

                    IsInitilized = RetVal.IntializeGraphPresentation(objDIConnection, objDIQueries, string.Empty, objDIDataView);
                    if (!IsInitilized)
                    {
                        RetVal = null;
                    }
                }
                MemoryStream.Dispose();
            }
            catch (Exception ex)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Load the deserialize XML file and update the NIDs on the basis of GIDs
        /// </summary>
        /// <param name="fileNameWPath"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <returns></returns>
        public static GraphPresentation Load(string fileNameWPath, DIConnection dbConnection, DIQueries dbQueries)
        {
            GraphPresentation RetVal;
            try
            {
                XmlSerializer GraphSerialize = new XmlSerializer(typeof(GraphPresentation));
                TextReader GraphReader = new StreamReader(fileNameWPath);
                RetVal = (GraphPresentation)GraphSerialize.Deserialize(GraphReader);
                RetVal.TablePresentation.UserPreference.UserSelection.UpdateNIdsFromGIds(dbConnection, dbQueries);
                GraphReader.Close();
            }
            catch (Exception ex)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Load the deserialize XML string and update the NIDs on the basis of GIDs
        /// </summary>
        /// <param name="SerializeText"></param>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <returns></returns>
        public static GraphPresentation LoadFromSerializeText(string SerializeText, DIConnection dbConnection, DIQueries dbQueries)
        {
            GraphPresentation RetVal;
            try
            {
                XmlSerializer GraphSerialize = new XmlSerializer(typeof(GraphPresentation));
                MemoryStream MemoryStream = new MemoryStream(StringToUTF8ByteArray(SerializeText));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(MemoryStream, Encoding.UTF8);
                RetVal = (GraphPresentation)GraphSerialize.Deserialize(MemoryStream);
                RetVal.TablePresentation.UserPreference.UserSelection.UpdateNIdsFromGIds(dbConnection, dbQueries);

                MemoryStream.Dispose();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }

        /// <summary>
        /// Update the Table presentation object and grapg datasource
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <param name="languageFilePath">Obsolete not in use. May set string.empty for time being</param>
        /// <param name="dbDataView"></param>
        /// <returns></returns>
        /// <remarks>This method is called after calling the load method, only if we have presnetation data</remarks>
        public bool IntializeGraphPresentation(DIConnection dbConnection, DIQueries dbQueries, string languageFilePath, DIDataView dbDataView)
        {
            bool Retval = false;
            try
            {
                Retval = this._TablePresentation.IntializeTablePresentation(dbConnection, dbQueries, string.Empty, dbDataView);
            }
            catch (Exception)
            {
                Retval = false;
            }
            return Retval;
        }

        /// <summary>
        /// Update the Table presentation object and grapg datasource
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dbQueries"></param>
        /// <param name="languageFilePath">Obsolete not in use. May set string.empty for time being</param>
        /// <param name="maskFilePath"></param>
        /// <returns></returns>
        /// <remarks>This method is called after calling the load method.</remarks>
        public bool IntializeGraphPresentation(DIConnection dbConnection, DIQueries dbQueries, string languageFilePath, string maskFilePath)
        {
            bool Retval = false;
            try
            {
                Retval = this._TablePresentation.IntializeTablePresentation(dbConnection, dbQueries, string.Empty, maskFilePath);
            }
            catch (Exception)
            {
                Retval = false;
            }
            return Retval;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            GraphPresentation RetVal;
            try
            {
                XmlSerializer XmlSerializer = new XmlSerializer(typeof(GraphPresentation));
                MemoryStream MemoryStream = new MemoryStream();
                XmlSerializer.Serialize(MemoryStream, this);
                MemoryStream.Position = 0;
                RetVal = (GraphPresentation)XmlSerializer.Deserialize(MemoryStream);
                MemoryStream.Close();
                MemoryStream.Dispose();
            }
            catch (Exception)
            {
                RetVal = null;
            }
            return RetVal;
        }



        #endregion

        #region " -- Object Comparision -- "

        /// <summary>
        /// GetHashCode is necessary to impement with equals method.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return 0;
        }

        /// <summary>
        /// Compare the TablePresentation object with the temp object.
        /// </summary>
        /// <param name="tempObj"></param>
        /// <returns></returns>
        public override bool Equals(System.Object tempObj)
        {
            try
            {
                GraphPresentation TempGraphPresentation = (GraphPresentation)tempObj;

                if (this._GraphObjectComparison == ObjectComparison.All)
                {
                    if (!this.TablePresentation.Equals(TempGraphPresentation.TablePresentation))
                    {
                        return false;
                    }

                    if (this._BorderCornerRadius != TempGraphPresentation.BorderCornerRadius)
                    {
                        return false;
                    }

                    if (this._SpreadsheetGearChartType != TempGraphPresentation._SpreadsheetGearChartType)
                    {
                        return false;
                    }

                    if (this._Height != TempGraphPresentation.Height)
                    {
                        return false;
                    }

                    if (this._PresentationPath != TempGraphPresentation.PresentationPath)
                    {
                        return false;
                    }

                    if (this._Width != TempGraphPresentation.Width)
                    {
                        return false;
                    }
                }

                //-- Title Settings 
                if (this._GraphAppearence.TitleSetting.FontTemplate.FontName != TempGraphPresentation.GraphAppearence.TitleSetting.FontTemplate.FontName)
                {
                    return false;
                }

                if (this.TablePresentation.Title != TempGraphPresentation.TablePresentation.Title)
                {
                    return false;
                }

                if (this.TablePresentation.Subtitle != TempGraphPresentation.TablePresentation.Subtitle)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.FontTemplate.FontStyle != TempGraphPresentation.GraphAppearence.TitleSetting.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.FontTemplate.FontSize != TempGraphPresentation.GraphAppearence.TitleSetting.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.FontTemplate.ForeColor != TempGraphPresentation.GraphAppearence.TitleSetting.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.FontTemplate.ShowBackColor != TempGraphPresentation.GraphAppearence.TitleSetting.FontTemplate.ShowBackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.FontTemplate.BackColor != TempGraphPresentation.GraphAppearence.TitleSetting.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.FontTemplate.BorderStyle != TempGraphPresentation.GraphAppearence.TitleSetting.FontTemplate.BorderStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.FontTemplate.BorderColor != TempGraphPresentation.GraphAppearence.TitleSetting.FontTemplate.BorderColor)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.BgColor != TempGraphPresentation._GraphAppearence.TitleSetting.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.ElementAlignment != TempGraphPresentation._GraphAppearence.TitleSetting.ElementAlignment)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.ElementPosition != TempGraphPresentation._GraphAppearence.TitleSetting.ElementPosition)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.Extent != TempGraphPresentation._GraphAppearence.TitleSetting.Extent)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.Flip != TempGraphPresentation._GraphAppearence.TitleSetting.Flip)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.LineColor != TempGraphPresentation._GraphAppearence.TitleSetting.LineColor)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.LineStyle != TempGraphPresentation._GraphAppearence.TitleSetting.LineStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.LineWidth != TempGraphPresentation._GraphAppearence.TitleSetting.LineWidth)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.Orientation != TempGraphPresentation._GraphAppearence.TitleSetting.Orientation)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.OrientationAngle != TempGraphPresentation._GraphAppearence.TitleSetting.OrientationAngle)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.Show != TempGraphPresentation._GraphAppearence.TitleSetting.Show)
                {
                    return false;
                }

                if (this._GraphAppearence.TitleSetting.ShowLabel != TempGraphPresentation._GraphAppearence.TitleSetting.ShowLabel)
                {
                    return false;
                }

                //-- Grid Settings 
                if (this._GraphAppearence.Grid.FontTemplate.FontName != TempGraphPresentation.GraphAppearence.Grid.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.FontTemplate.FontStyle != TempGraphPresentation.GraphAppearence.Grid.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.FontTemplate.FontSize != TempGraphPresentation.GraphAppearence.Grid.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.FontTemplate.BackColor != TempGraphPresentation.GraphAppearence.Grid.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.FontTemplate.ForeColor != TempGraphPresentation.GraphAppearence.Grid.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.BgColor != TempGraphPresentation._GraphAppearence.Grid.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.BgColor != TempGraphPresentation._GraphAppearence.Grid.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.BgColor != TempGraphPresentation._GraphAppearence.Grid.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.ElementAlignment != TempGraphPresentation._GraphAppearence.Grid.ElementAlignment)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.ElementPosition != TempGraphPresentation._GraphAppearence.Grid.ElementPosition)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.Extent != TempGraphPresentation._GraphAppearence.Grid.Extent)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.Flip != TempGraphPresentation._GraphAppearence.Grid.Flip)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.LineColor != TempGraphPresentation._GraphAppearence.Grid.LineColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.LineStyle != TempGraphPresentation._GraphAppearence.Grid.LineStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.LineWidth != TempGraphPresentation._GraphAppearence.Grid.LineWidth)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.Orientation != TempGraphPresentation._GraphAppearence.Grid.Orientation)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.OrientationAngle != TempGraphPresentation._GraphAppearence.Grid.OrientationAngle)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.Show != TempGraphPresentation._GraphAppearence.Grid.Show)
                {
                    return false;
                }

                if (this._GraphAppearence.Grid.ShowLabel != TempGraphPresentation._GraphAppearence.Grid.ShowLabel)
                {
                    return false;
                }

                //-- Footnotes Settings 
                if (this._GraphAppearence.Footnotes.FontTemplate.FontName != TempGraphPresentation.GraphAppearence.Footnotes.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.FontTemplate.FontStyle != TempGraphPresentation.GraphAppearence.Footnotes.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.FontTemplate.FontSize != TempGraphPresentation.GraphAppearence.Footnotes.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.FontTemplate.BackColor != TempGraphPresentation.GraphAppearence.Footnotes.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.FontTemplate.ForeColor != TempGraphPresentation.GraphAppearence.Footnotes.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.BgColor != TempGraphPresentation._GraphAppearence.Footnotes.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.ElementAlignment != TempGraphPresentation._GraphAppearence.Footnotes.ElementAlignment)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.ElementPosition != TempGraphPresentation._GraphAppearence.Footnotes.ElementPosition)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.Extent != TempGraphPresentation._GraphAppearence.Footnotes.Extent)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.Flip != TempGraphPresentation._GraphAppearence.Footnotes.Flip)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.LineColor != TempGraphPresentation._GraphAppearence.Footnotes.LineColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.LineStyle != TempGraphPresentation._GraphAppearence.Footnotes.LineStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.LineWidth != TempGraphPresentation._GraphAppearence.Footnotes.LineWidth)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.Orientation != TempGraphPresentation._GraphAppearence.Footnotes.Orientation)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.OrientationAngle != TempGraphPresentation._GraphAppearence.Footnotes.OrientationAngle)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.Show != TempGraphPresentation._GraphAppearence.Footnotes.Show)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.FontTemplate.Inline != TempGraphPresentation._GraphAppearence.Footnotes.FontTemplate.Inline)
                {
                    return false;
                }

                if (this._GraphAppearence.Footnotes.ShowLabel != TempGraphPresentation._GraphAppearence.Footnotes.ShowLabel)
                {
                    return false;
                }


                //-- Border Settings 
                if (this._GraphAppearence.Border.FontTemplate.FontName != TempGraphPresentation.GraphAppearence.Border.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.FontTemplate.FontStyle != TempGraphPresentation.GraphAppearence.Border.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.FontTemplate.FontSize != TempGraphPresentation.GraphAppearence.Border.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.FontTemplate.BackColor != TempGraphPresentation.GraphAppearence.Border.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.FontTemplate.ForeColor != TempGraphPresentation.GraphAppearence.Border.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.BgColor != TempGraphPresentation._GraphAppearence.Border.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.BgColor != TempGraphPresentation._GraphAppearence.Border.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.BgColor != TempGraphPresentation._GraphAppearence.Border.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.ElementAlignment != TempGraphPresentation._GraphAppearence.Border.ElementAlignment)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.ElementPosition != TempGraphPresentation._GraphAppearence.Border.ElementPosition)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.Extent != TempGraphPresentation._GraphAppearence.Border.Extent)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.Flip != TempGraphPresentation._GraphAppearence.Border.Flip)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.LineColor != TempGraphPresentation._GraphAppearence.Border.LineColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.LineStyle != TempGraphPresentation._GraphAppearence.Border.LineStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.LineWidth != TempGraphPresentation._GraphAppearence.Border.LineWidth)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.Orientation != TempGraphPresentation._GraphAppearence.Border.Orientation)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.OrientationAngle != TempGraphPresentation._GraphAppearence.Border.OrientationAngle)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.Show != TempGraphPresentation._GraphAppearence.Border.Show)
                {
                    return false;
                }

                if (this._GraphAppearence.Border.ShowLabel != TempGraphPresentation._GraphAppearence.Border.ShowLabel)
                {
                    return false;
                }

                //-- X axis Settings 
                if (this._GraphAppearence.XAxis.FontTemplate.FontName != TempGraphPresentation.GraphAppearence.XAxis.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.FontTemplate.FontStyle != TempGraphPresentation.GraphAppearence.XAxis.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.FontTemplate.FontSize != TempGraphPresentation.GraphAppearence.XAxis.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.FontTemplate.BackColor != TempGraphPresentation.GraphAppearence.XAxis.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.FontTemplate.ForeColor != TempGraphPresentation.GraphAppearence.XAxis.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.BgColor != TempGraphPresentation._GraphAppearence.XAxis.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.BgColor != TempGraphPresentation._GraphAppearence.XAxis.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.BgColor != TempGraphPresentation._GraphAppearence.XAxis.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.ElementAlignment != TempGraphPresentation._GraphAppearence.XAxis.ElementAlignment)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.ElementPosition != TempGraphPresentation._GraphAppearence.XAxis.ElementPosition)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.Extent != TempGraphPresentation._GraphAppearence.XAxis.Extent)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.Flip != TempGraphPresentation._GraphAppearence.XAxis.Flip)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.LineColor != TempGraphPresentation._GraphAppearence.XAxis.LineColor)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.LineStyle != TempGraphPresentation._GraphAppearence.XAxis.LineStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.LineWidth != TempGraphPresentation._GraphAppearence.XAxis.LineWidth)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.Orientation != TempGraphPresentation._GraphAppearence.XAxis.Orientation)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.OrientationAngle != TempGraphPresentation._GraphAppearence.XAxis.OrientationAngle)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.Show != TempGraphPresentation._GraphAppearence.XAxis.Show)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxis.ShowLabel != TempGraphPresentation._GraphAppearence.XAxis.ShowLabel)
                {
                    return false;
                }

                //-- Y axis Settings 
                if (this._GraphAppearence.YAxis.FontTemplate.FontName != TempGraphPresentation.GraphAppearence.YAxis.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.FontTemplate.FontStyle != TempGraphPresentation.GraphAppearence.YAxis.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.FontTemplate.FontSize != TempGraphPresentation.GraphAppearence.YAxis.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.FontTemplate.BackColor != TempGraphPresentation.GraphAppearence.YAxis.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.FontTemplate.ForeColor != TempGraphPresentation.GraphAppearence.YAxis.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.BgColor != TempGraphPresentation._GraphAppearence.YAxis.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.BgColor != TempGraphPresentation._GraphAppearence.YAxis.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.BgColor != TempGraphPresentation._GraphAppearence.YAxis.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.ElementAlignment != TempGraphPresentation._GraphAppearence.YAxis.ElementAlignment)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.ElementPosition != TempGraphPresentation._GraphAppearence.YAxis.ElementPosition)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.Extent != TempGraphPresentation._GraphAppearence.YAxis.Extent)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.Flip != TempGraphPresentation._GraphAppearence.YAxis.Flip)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.LineColor != TempGraphPresentation._GraphAppearence.YAxis.LineColor)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.LineStyle != TempGraphPresentation._GraphAppearence.YAxis.LineStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.LineWidth != TempGraphPresentation._GraphAppearence.YAxis.LineWidth)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.Orientation != TempGraphPresentation._GraphAppearence.YAxis.Orientation)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.OrientationAngle != TempGraphPresentation._GraphAppearence.YAxis.OrientationAngle)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.Show != TempGraphPresentation._GraphAppearence.YAxis.Show)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxis.ShowLabel != TempGraphPresentation._GraphAppearence.YAxis.ShowLabel)
                {
                    return false;
                }

                //-- Legends Settings 
                if (this._GraphAppearence.Legends.FontTemplate.FontName != TempGraphPresentation.GraphAppearence.Legends.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.FontTemplate.FontStyle != TempGraphPresentation.GraphAppearence.Legends.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.FontTemplate.FontSize != TempGraphPresentation.GraphAppearence.Legends.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.FontTemplate.BackColor != TempGraphPresentation.GraphAppearence.Legends.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.FontTemplate.ForeColor != TempGraphPresentation.GraphAppearence.Legends.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.BgColor != TempGraphPresentation._GraphAppearence.Legends.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.ElementAlignment != TempGraphPresentation._GraphAppearence.Legends.ElementAlignment)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.ElementPosition != TempGraphPresentation._GraphAppearence.Legends.ElementPosition)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.Extent != TempGraphPresentation._GraphAppearence.Legends.Extent)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.Flip != TempGraphPresentation._GraphAppearence.Legends.Flip)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.LineColor != TempGraphPresentation._GraphAppearence.Legends.LineColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.LineStyle != TempGraphPresentation._GraphAppearence.Legends.LineStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.LineWidth != TempGraphPresentation._GraphAppearence.Legends.LineWidth)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.Orientation != TempGraphPresentation._GraphAppearence.Legends.Orientation)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.OrientationAngle != TempGraphPresentation._GraphAppearence.Legends.OrientationAngle)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.Show != TempGraphPresentation._GraphAppearence.Legends.Show)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.ShowLabel != TempGraphPresentation._GraphAppearence.Legends.ShowLabel)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.FontTemplate.Show != TempGraphPresentation._GraphAppearence.Legends.FontTemplate.Show)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.FontTemplate.BorderColor != TempGraphPresentation._GraphAppearence.Legends.FontTemplate.BorderColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.FontTemplate.ShowBackColor != TempGraphPresentation._GraphAppearence.Legends.FontTemplate.ShowBackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.Legends.FontTemplate.BorderStyle != TempGraphPresentation._GraphAppearence.Legends.FontTemplate.BorderStyle)
                {
                    return false;
                }

                //-- plot area Settings 
                if (this._GraphAppearence.PlotArea.FontTemplate.ShowBackColor != TempGraphPresentation.GraphAppearence.PlotArea.FontTemplate.ShowBackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.PlotArea.FontTemplate.BackColor != TempGraphPresentation.GraphAppearence.PlotArea.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.PlotArea.FontTemplate.BorderStyle != TempGraphPresentation.GraphAppearence.PlotArea.FontTemplate.BorderStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.PlotArea.FontTemplate.BorderColor != TempGraphPresentation.GraphAppearence.PlotArea.FontTemplate.BorderColor)
                {
                    return false;
                }

                //-- chart area Settings 
                if (this._GraphAppearence.ChartArea.FontTemplate.ShowBackColor != TempGraphPresentation.GraphAppearence.ChartArea.FontTemplate.ShowBackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.ChartArea.FontTemplate.BackColor != TempGraphPresentation.GraphAppearence.ChartArea.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.ChartArea.FontTemplate.BorderStyle != TempGraphPresentation.GraphAppearence.ChartArea.FontTemplate.BorderStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.ChartArea.FontTemplate.BorderColor != TempGraphPresentation.GraphAppearence.ChartArea.FontTemplate.BorderColor)
                {
                    return false;
                }

               
                //-- XAxisSeriesLabel Settings 
                if (this._GraphAppearence.XAxisSeriesLabel.FontTemplate.FontName != TempGraphPresentation.GraphAppearence.XAxisSeriesLabel.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.FontTemplate.FontStyle != TempGraphPresentation.GraphAppearence.XAxisSeriesLabel.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.FontTemplate.FontSize != TempGraphPresentation.GraphAppearence.XAxisSeriesLabel.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.FontTemplate.BackColor != TempGraphPresentation.GraphAppearence.XAxisSeriesLabel.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.FontTemplate.ForeColor != TempGraphPresentation.GraphAppearence.XAxisSeriesLabel.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.BgColor != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.BgColor != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.BgColor != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.ElementAlignment != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.ElementAlignment)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.ElementPosition != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.ElementPosition)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.Extent != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.Extent)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.Flip != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.Flip)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.LineColor != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.LineColor)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.LineStyle != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.LineStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.LineWidth != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.LineWidth)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.Orientation != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.Orientation)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.OrientationAngle != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.OrientationAngle)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.Show != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.Show)
                {
                    return false;
                }

                if (this._GraphAppearence.XAxisSeriesLabel.ShowLabel != TempGraphPresentation._GraphAppearence.XAxisSeriesLabel.ShowLabel)
                {
                    return false;
                }

                //-- YAxisSeriesLabel Settings 
                if (this._GraphAppearence.YAxisSeriesLabel.FontTemplate.FontName != TempGraphPresentation.GraphAppearence.YAxisSeriesLabel.FontTemplate.FontName)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.FontTemplate.FontStyle != TempGraphPresentation.GraphAppearence.YAxisSeriesLabel.FontTemplate.FontStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.FontTemplate.FontSize != TempGraphPresentation.GraphAppearence.YAxisSeriesLabel.FontTemplate.FontSize)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.FontTemplate.BackColor != TempGraphPresentation.GraphAppearence.YAxisSeriesLabel.FontTemplate.BackColor)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.FontTemplate.ForeColor != TempGraphPresentation.GraphAppearence.YAxisSeriesLabel.FontTemplate.ForeColor)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.BgColor != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.BgColor != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.BgColor != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.BgColor)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.ElementAlignment != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.ElementAlignment)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.ElementPosition != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.ElementPosition)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.Extent != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.Extent)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.Flip != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.Flip)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.LineColor != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.LineColor)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.LineStyle != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.LineStyle)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.LineWidth != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.LineWidth)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.Orientation != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.Orientation)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.OrientationAngle != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.OrientationAngle)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.Show != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.Show)
                {
                    return false;
                }

                if (this._GraphAppearence.YAxisSeriesLabel.ShowLabel != TempGraphPresentation._GraphAppearence.YAxisSeriesLabel.ShowLabel)
                {
                    return false;
                }
            }

            catch (Exception ex)
            {
                return false;
            }
            return true;

        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                this.GraphDataSource.Dispose();
                this._TablePresentation.Dispose();

            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region ILanguage Members

        public void ApplyLanguageSettings()
        {
            try
            {

                // TOP
                LanguageSettings.TOP = DILanguage.GetLanguageString("TOP");

                // BOTTOM 
                LanguageSettings.BOTTOM = DILanguage.GetLanguageString("BOTTOM");

                // RIGHT 
                LanguageSettings.RIGHT = DILanguage.GetLanguageString("RIGHT");

                // LEFT 
                LanguageSettings.LEFT = DILanguage.GetLanguageString("LEFT");

                // CENTER 
                LanguageSettings.CENTER = DILanguage.GetLanguageString("CENTER");

                // FAR 
                LanguageSettings.FAR = DILanguage.GetLanguageString("FAR");

                // NEAR 
                LanguageSettings.NEAR = DILanguage.GetLanguageString("NEAR");

                // SOLID 
                LanguageSettings.SOLID = DILanguage.GetLanguageString("SOLID");

                // DASH 
                LanguageSettings.DASH = DILanguage.GetLanguageString("DASH");

                // DASHDOT 
                LanguageSettings.DASHDOT = DILanguage.GetLanguageString("DASHDOT");

                // DASHDOTDOT 
                LanguageSettings.DASHDOTDOT = DILanguage.GetLanguageString("LanguageSettingsDASHDOTDOT");

                // VERTICALLEFTFACING 
                LanguageSettings.VERTICALLEFTFACING = DILanguage.GetLanguageString("VERTICALLEFTFACING");

                // VERTICALRIGHTFACING 
                LanguageSettings.VERTICALRIGHTFACING = DILanguage.GetLanguageString("VERTICALRIGHTFACING");

                // HORIZONTAL 
                LanguageSettings.HORIZONTAL = DILanguage.GetLanguageString("HORIZONTAL");

                // CUSTOM 
                LanguageSettings.CUSTOM = DILanguage.GetLanguageString("CUSTOM");

                // ColumnChart 
                LanguageSettings.ColumnChart = DILanguage.GetLanguageString("ColumnChart");

                // BarChart 
                LanguageSettings.BarChart = DILanguage.GetLanguageString("BarChart");

                // AreaChart 
                LanguageSettings.AreaChart = DILanguage.GetLanguageString("AreaChart");

                // LineChart 
                LanguageSettings.LineChart = DILanguage.GetLanguageString("LineChart");

                // PieChart 
                LanguageSettings.PieChart = DILanguage.GetLanguageString("PieChart");

                // BubbleChart 
                LanguageSettings.BubbleChart = DILanguage.GetLanguageString("BubbleChart");

                // ColumnChart3D 
                LanguageSettings.ColumnChart3D = DILanguage.GetLanguageString("ColumnChart3D");

                // BarChart3D 
                LanguageSettings.BarChart3D = DILanguage.GetLanguageString("BarChart3D");

                // AreaChart3D 
                LanguageSettings.AreaChart3D = DILanguage.GetLanguageString("AreaChart3D");

                // LineChart3D 
                LanguageSettings.LineChart3D = DILanguage.GetLanguageString("LineChart3D");

                // PieChart3D 
                LanguageSettings.PieChart3D = DILanguage.GetLanguageString("PieChart3D");

                // StackColumnChart 
                LanguageSettings.StackColumnChart = DILanguage.GetLanguageString("StackColumnChart");

                // StackBarChart 
                LanguageSettings.StackBarChart = DILanguage.GetLanguageString("StackBarChart");

                // Stack3DColumnChart 
                LanguageSettings.Stack3DColumnChart = DILanguage.GetLanguageString("Stack3DColumnChart");

                // Stack3DBarChart 
                LanguageSettings.Stack3DBarChart = DILanguage.GetLanguageString("Stack3DBarChart");

                // ColumnLineChart 
                LanguageSettings.ColumnLineChart = DILanguage.GetLanguageString("ColumnLineChart");

                // StackAreaChart 
                LanguageSettings.StackAreaChart = DILanguage.GetLanguageString("StackAreaChart");

                // StackLineChart 
                LanguageSettings.StackLineChart = DILanguage.GetLanguageString("StackLineChart");

            }

            catch (Exception)
            {
            }
        }

        #endregion

    }
}
