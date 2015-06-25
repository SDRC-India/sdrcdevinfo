using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.UserSelection;

using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DevInfo.Lib.DI_LibBAL.Export
{
    class ExportPDF
    {

        internal static bool ExportDataView(DataView sourceDataView, string DevInfoLogoPath, string sourceDatabaseNameForHeading, System.Drawing.Font headerFont,  System.Drawing.Font tabularDataFont, string outputFileNameWPath)
        {
            bool RetVal = false;
            RetVal = ExportPDF.ExportDataView(sourceDataView, false, DevInfoLogoPath, sourceDatabaseNameForHeading, headerFont, tabularDataFont, System.Drawing.Color.Black, outputFileNameWPath);
            return RetVal;
        }

        /// <summary>
        /// It generates PDF document by displaying data from dataView passed , with DevInfo logo image on top of it.
        /// <para>It prints only those column values which are required to be diaplayed.</para>
        /// <para>For eg: Indicator_Global, Unit_Global, Subgroup_Global are required in dataTable for globalColor, but will not be displayed.</para>
        /// </summary>
        /// <param name="sourceDataView">sourceDataView</param>
        /// <param name="RTL">bool value for RTL. </param>
        /// <param name="DevInfoLogoPath">DevInfo logo image fileName with path.</param>
        /// <param name="sourceDatabaseNameForHeading">source database name to be displayed on Top of document.</param>
        /// <param name="headerFont">Column Headers Font.</param>
        /// <param name="tabularDataFont">Font for data rows</param>
        /// <param name="globalColor">global color</param>
        /// <param name="outputFileNameWPath">output filename with path.</param>
        /// <returns></returns>
        internal static bool ExportDataView(DataView sourceDataView, bool RTL, string DevInfoLogoPath, string sourceDatabaseNameForHeading, System.Drawing.Font headerFont, System.Drawing.Font tabularDataFont, System.Drawing.Color globalColor, string outputFileNameWPath)
        {
            bool RetVal = false;
            Font TableFont;
            Font TableFontWithGlobalColor;
            Font HeaderFont;
            BaseFont _BodyFont = null;
            string[] ColumnsList = null;
            int columnCount = 0;

            try
            {
                // ::->Fetching the data for PDF document
                DataTable DTReport = sourceDataView.ToTable();
                                          
                int count = DTReport.Columns.Count;

                // ::->Take a Rectangle works as a canvas for the report
                Rectangle _PdfCanvas = new iTextSharp.text.Rectangle(PageSize.A3.Width, PageSize.A3.Height);

                // ::->Create a doucument 
                Document _Document = new Document(_PdfCanvas, 55, 72, 25, 55);
                
                // ::-> Create a file Stream
                if (File.Exists(outputFileNameWPath))
                {
                    File.Delete(outputFileNameWPath);
                }
                FileStream _FileStream = new FileStream(outputFileNameWPath, FileMode.Create);

                //Create PDF writer
                PdfWriter _PDF = PdfWriter.GetInstance(_Document, _FileStream);

                //Set the Font if provided fonts are null
                if (File.Exists(Environment.GetEnvironmentVariable("Systemroot") + "\\Fonts\\ARIALUNI.TTF"))
                {
                    //// -- Getting Arial UNICODE font to aply on all UNICODE languages 
                    _BodyFont = BaseFont.CreateFont(Environment.GetEnvironmentVariable("Systemroot") + "\\Fonts\\ARIALUNI.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                }
                else
                {
                    // Create default
                   //TODO check it and validate
					_BodyFont = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, "utf-8", BaseFont.EMBEDDED);
                }

                if (tabularDataFont == null)
                {
                    TableFont = new Font(_BodyFont, 8F, Font.NORMAL, Color.BLACK);
                    TableFontWithGlobalColor = new Font(_BodyFont, 8F, Font.NORMAL, new Color(globalColor));
                }
                else
                {
                    TableFont = new Font(Font.GetFamilyIndex(tabularDataFont.Name), tabularDataFont.Size, Font.NORMAL, Color.BLACK);
                    TableFontWithGlobalColor = new Font(Font.GetFamilyIndex(tabularDataFont.Name), tabularDataFont.Size, Font.NORMAL, new Color(globalColor));
                }

                if (headerFont == null)
                {
                    HeaderFont = new Font(_BodyFont, 8F, Font.NORMAL, Color.WHITE);
                }
                else
                {
                    HeaderFont = new Font(Font.GetFamilyIndex(headerFont.Name), headerFont.Size, Font.NORMAL, Color.WHITE);
                }

                //-----::Add Meta Data

                // Get Adaptation 's ApplicationName & Version for title
               DevInfo.Lib.DI_LibBAL.UI.UserPreference.Adaptation.ApplicationLevelSetting adaptation =  new DevInfo.Lib.DI_LibBAL.UI.UserPreference.Adaptation.ApplicationLevelSetting();
               _Document.AddTitle(adaptation.ApplicationName + " " + adaptation.ApplicationVersion); 
                _Document.AddSubject("");
                _Document.AddCreator(adaptation.ApplicationName);
                _Document.AddAuthor(adaptation.ApplicationName);
                _Document.AddHeader("Expires", "0");

                //::-> Open the Document
                _Document.Open();

                //***************************************************************************
                //::-> Creating Logo
                ExportPDF.AddLogo(ref _Document, DevInfoLogoPath, sourceDatabaseNameForHeading);

                //*********************************************************************************

                //::-> Start Creating Headers
                //::-> Create Cell and Table
                iTextSharp.text.Cell _Cell;
                iTextSharp.text.Table _Table;

                //Calculate required Column Count
                ColumnsList = new string[DTReport.Columns.Count];
                columnCount = 0;
                int c = 0;
                foreach (DataColumn Dcolumn in DTReport.Columns)
                {
                    if (DIExport.CheckColumnRelevence(Dcolumn.ColumnName))  // eliminating _global columns in DataTable
                    {
                        //Save columnName in list
                        ColumnsList[c] = Dcolumn.ColumnName;
                        columnCount++;
                    }
                    else
                    {
                        //Save blank in list
                        ColumnsList[c] = string.Empty;
                    }
                    c++;
                }

                //::-> Create the table and Set the Properties
                _Table = new iTextSharp.text.Table(columnCount);
                _Table.Cellpadding = 1;
                _Table.AutoFillEmptyCells = true;
                _Table.AutoFillEmptyCells = true;
                _Table.Border = 0;


                //::-> Writing Header Info        
                foreach (string Dcolumn in ColumnsList)
                {
                    if (Dcolumn.Trim().Length > 0)
                    {
                        _Cell = new Cell(new iTextSharp.text.Phrase(DTReport.Columns[Dcolumn].Caption, HeaderFont)); //new Font(14, 10, (int)iTextSharp.text.Font.BOLD, iTextSharp.text.Color.WHITE)));
                        _Cell.BackgroundColor = new Color(145, 145, 145);
                        _Cell.Border = 0;
                        //_Cell.Colspan = 5;
                        _Cell.VerticalAlignment = Element.ALIGN_TOP;
                        if (RTL)
                        {
                            _Cell.HorizontalAlignment = Element.ALIGN_RIGHT;
                        }
                        _Table.AddCell(_Cell);
                    }
                }

                //::-> End of the Header
                //*********************************************************************************

                //Adding Data
                foreach (DataRow Dr in DTReport.Rows)
                {
                    for (int i = 0; i < ColumnsList.Length; i++)
                    {
                        if (ColumnsList[i].Trim().Length > 0)
                        {
                            iTextSharp.text.Cell _Cellv;

                            // Check if this column have equivalent _Global column exists in DatTable
                            if (DIExport.CheckGlobalColumnValue(DTReport, Dr, ColumnsList[i]))
                            {
                                _Cellv = new iTextSharp.text.Cell(new iTextSharp.text.Phrase(Dr[i].ToString(), TableFontWithGlobalColor));    //new Font(14, 08, (int)iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)));
                            }
                            else
                            {
                                _Cellv = new iTextSharp.text.Cell(new iTextSharp.text.Phrase(Dr[i].ToString(), TableFont));    //new Font(14, 08, (int)iTextSharp.text.Font.NORMAL, iTextSharp.text.Color.BLACK)));
                            }
                            //TODO column width adjust.
                            if (RTL)
                            {
                                _Cellv.HorizontalAlignment = Element.ALIGN_RIGHT;
                            }
                            else
                            {
                                _Cellv.HorizontalAlignment = Element.ALIGN_LEFT;
                            }
                            _Cellv.VerticalAlignment = Element.ALIGN_TOP;

                            _Table.AddCell(_Cellv);
                        }
                    }
                }

                      // -- Set Table Properties
                    _Table.Cellpadding = 1;
                    _Table.Cellspacing = 0;
                    _Table.BorderWidth = 1;
                    _Table.BorderColor = Color.BLUE;
                    _Table.Alignment = 1;
                    //_Table.WidthPercentage = Convert.ToSingle(100);
                    _Table.Border = 1;
                    _Table.BorderColor = Color.BLUE;

                _Document.Add(_Table);

                _Document.Close();

                RetVal = true;
            }
            catch (Exception ex)
            {
                ExceptionHandler.ExceptionFacade.ThrowException(ex);
            }

            return RetVal;
        }

        internal static bool ExportICFromDataView(DataView dataView, ICType ICElementType, string outputFileNameWPath)
        {
            bool RetVal = false;

            return RetVal;
        }

        internal static bool ExportIndicatorFromDataView(DataView dataView, string outputFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;
            RetVal = ExportPDF.ExportDataView(dataView, false, string.Empty, string.Empty, null, null, System.Drawing.Color.Black, outputFileNameWPath);
            return RetVal;
        }

        internal static bool ExportAreaFromDataView(DataView dataView, string outputFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;
            RetVal = ExportPDF.ExportDataView(dataView, false, string.Empty, string.Empty, null, null, System.Drawing.Color.Black, outputFileNameWPath);
            return RetVal;
        }

        private static void AddLogo(ref Document _document, string logoFilePath, string sourceDatabaseHeader)
        {
            Cell _cell;
            Image jpg;

            try
            {
                //Font fontHeaderDoc = new Font(headerFont, (float)goUserPreference.Language.FontSize + 2, Font.BOLD, Color.BLACK);
                //Dim fontHeaderDoc As Font = New Font(bfComic, giAppFontSize + 2, Font.BOLD, Color.BLACK)
                Table _TableLogo;
                _TableLogo = new Table(2, 1);
                _TableLogo.Cellspacing *= 1.2F;
                // -- Logo
                if (File.Exists(logoFilePath))
                {

                    jpg = Image.GetInstance(logoFilePath);        //Adaptaion Image - "logos\\Product_banner.png");
                    _cell = new Cell(jpg);
                    _cell.Border = 0;
                    _TableLogo.AddCell(_cell);
                }
                // -- Database Name
                if (string.IsNullOrEmpty(sourceDatabaseHeader) == false)
                {
                    _cell = new Cell(new Phrase(sourceDatabaseHeader, new Font(Font.GetFamilyIndex("Arial"), 9F, Font.BOLD, Color.BLACK)));
                    //_cell = New Cell(New Phrase(goUserSelection.SelectedDbName, fontHeaderDoc))
                    _cell.Border = 0;
                    _cell.HorizontalAlignment = Cell.ALIGN_RIGHT;
                    _cell.VerticalAlignment = Cell.ALIGN_CENTER;
                    _TableLogo.AddCell(_cell);
                    //_TableLogo.WidthPercentage = 100;
                    _TableLogo.Cellpadding = 0;
                    _TableLogo.Border = 0;
                    _document.Add(_TableLogo);
                    // -- Empty line
                    _document.Add(new Phrase(" ", new Font(Font.GetFamilyIndex("Arial"), 8F, Font.BOLD, Color.BLACK)));
                }
            }
            catch (Exception)
            {

            }
        }

    }
}
