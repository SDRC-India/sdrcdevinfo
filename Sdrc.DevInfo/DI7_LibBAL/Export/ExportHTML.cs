using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.UserSelection;
using Microsoft.VisualBasic;
using System.Drawing;

namespace DevInfo.Lib.DI_LibBAL.Export
{
    class ExportHTML
    {
        #region "-- New / Dipose --"

        //public ExportHTML(DIConnection dBConnection, DIQueries dBQueries, ExportUserSelections userSelection, DIExportOutputType exportOutputType, string outputFolder, bool includeGUID, bool singleWorkbook)
        //{
        //    //
        //    this._DBConnection = dBConnection;
        //    this._DBQueries = dBQueries;
        //    this._UserSelection = userSelection;
        //    this._ExportFileType = exportOutputType;
        //    this._OutputFolder = outputFolder;

        //}

        #endregion
        
        internal static bool ExportDataView(DataView sourceDataView, string outputFileNameWPath)
        {
             bool RetVal = false;
             RetVal = ExportHTML.ExportDataView(sourceDataView, false, null, null, Color.Black, outputFileNameWPath);
            return RetVal;
        }

        /// <summary>
        /// It exports data provided in dataView to HTML format.
        /// </summary>
        internal static bool ExportDataView(DataView dataView, bool RTL, Font columnheaderFont, Font dataRowFont, Color globalColor, string outputFileNameWPath)
        {
            bool RetVal = false;
            string HeaderFont =string.Empty;
            string BodyFont= string.Empty;
            string BodyFontWithGlobalColor = string.Empty;      // Font used to display text in global color.
            bool ApplyGlobalColor = false;              // whether global color is to be applied on cell or not.
            string sStrVal = string.Empty;
            string sDir = "ltr";


            {
                try
                {   
                    // -- Get DataView
                    DataTable _DT = dataView.ToTable();                    

                    // Create Fonts for headers 
                    if (columnheaderFont == null)
                    {  
                        HeaderFont = "<font style=\"FONT-FAMILY:Arial;FONT-SIZE:8pt;\">";   // Default is kept Arail , 8pt
                    }
                    else
                    {
                        HeaderFont = "<font style=\"FONT-FAMILY:" + columnheaderFont.Name + ";FONT-SIZE:" + columnheaderFont.Size.ToString() + "pt;\">";
                    }

                    // Create Fonts for Data rows
                    if (dataRowFont == null)
                    {   
                        BodyFont = "<font style=\"FONT-FAMILY:Arial;FONT-SIZE:8pt;\">";     // Default is kept Arail , 8pt
                        BodyFontWithGlobalColor = "<font style=\"FONT-FAMILY:Arial;FONT-SIZE:8pt;Color:" + globalColor.Name + ";\">";
                    }
                    else
                    {
                        BodyFont = "<font style=\"FTON-FAMILY:" + dataRowFont.Name + ";FONT-SIZE:" + dataRowFont.Size.ToString() + "pt;\">";
                        BodyFontWithGlobalColor = "<font style=\"FONT-FAMILY:" + dataRowFont.Name + ";FONT-SIZE:" + dataRowFont.Size.ToString() + "pt;Color:" + globalColor.Name + ";\">";
                    }

                    System.IO.StreamWriter output = new System.IO.StreamWriter(outputFileNameWPath, false, System.Text.UTF8Encoding.UTF8);
                    string delim;

                    // -- HTML Part
                    if (RTL == true)
                    {
                        sDir = "rtl";  
                    }
                    output.Write("<HTML dir='" + sDir + "'><HEAD><TITLE> Data </TITLE>");   //-- TODO: Title to be set right. (Functional Requirement.)
                    output.WriteLine();
                    output.Write("<meta http-equiv=\"Content-Type\" content=\"text/html;charset=UTF-8;\">");
                    output.Write("</HEAD><BODY>");
                    output.WriteLine();


                    // --TODO get the table styles to get the Column widths 


                    // -- Get all Visible Columns
                    //int iCols;
                    string[] ColActualName = new string[_DT.Columns.Count];
                    string[] ColHeaderName = new string[_DT.Columns.Count];

                        for (int i = 0; i <= _DT.Columns.Count - 1; i++)
                        {
                            if (DIExport.CheckColumnRelevence(_DT.Columns[i].ToString()))
                            {
                                ColActualName[i] = _DT.Columns[i].ToString();
                                ColHeaderName[i] = _DT.Columns[i].Caption;
                            }
                            else
                            {
                                ColActualName[i] = string.Empty;
                                ColHeaderName[i] = string.Empty;
                            }
                        }


                    // -- Parent Table start here
                    output.Write("<table width=90% align=center>");
                    output.WriteLine();

                    // -- Header
                    output.Write("<TR bgcolor='#cccccc' style='font-color:#ffffff'>");
                    output.WriteLine();
                    for (int cc = 0; cc <= ColActualName.Length - 1; cc++)
                    {
                        if (ColHeaderName[cc].Trim().Length > 0)
                        {
                            output.Write("<TD>" + HeaderFont);
                            output.Write(ColHeaderName[cc]);
                            output.Write("</TD>");
                            output.WriteLine();
                        }
                    }
                    output.WriteLine();
                    output.Write("</TR>");
                    output.WriteLine();


                    // -- Body
                    {
                        //for (int cc = 0; cc <= _DT.Rows.Count - 1; cc++)
                        foreach (DataRow dr in _DT.Rows)
                        {
                            output.Write("<TR>");
                            output.WriteLine();


                            foreach (string col in ColActualName) //; jj <= ColActualName.Length - 1; jj++)
                            {
                                if (col.Trim().Length == 0)
                                {
                                    // Do nothing
                                }
                                else
                                {
                                    output.Write("<TD>");
                                    output.WriteLine();
                                    if (Information.IsDBNull(dr[col])) // _DT.Rows[cc][ColActualName[jj]]))
                                    {
                                        sStrVal = "";
                                    }
                                    else
                                    {
                                        sStrVal = dr[col].ToString();
                                        if (globalColor != Color.Black)
                                        {
                                            // Check if column have equivalent _Global column exists in DatTable
                                            ApplyGlobalColor = DIExport.CheckGlobalColumnValue(_DT, dr, col);
                                        }
                                    }


                                    if (ApplyGlobalColor)
                                    {
                                        output.Write(BodyFontWithGlobalColor + sStrVal + "</font>");
                                    }
                                    else
                                    {
                                        output.Write(BodyFont + sStrVal + "</font>");
                                    }


                                    output.Write("</TD>");
                                    output.WriteLine();
                                }
                            }
                            output.Write("</TR>");
                            output.WriteLine();
                        }
                    }

                    output.WriteLine();
                    output.Write("</table>");
                    output.WriteLine();

                    // -- End of HTML Part
                    output.Write("</BODY></HTML>");
                    output.Close();

                    RetVal = true;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.ExceptionFacade.ThrowException(ex);
                }
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
            RetVal = ExportHTML.ExportDataView(dataView, false, null, null, Color.Black, outputFileNameWPath);
            return RetVal;
        }

        internal static bool ExportAreaFromDataView(DataView dataView, string outputFileNameWPath, string languageFileNameWPath)
        {
            bool RetVal = false;
            RetVal = ExportHTML.ExportDataView(dataView, false, null, null, Color.Black, outputFileNameWPath);
            return RetVal;
        }

    }
}
