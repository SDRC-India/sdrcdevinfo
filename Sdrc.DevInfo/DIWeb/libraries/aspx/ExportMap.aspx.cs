using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using Svg;
using System.Drawing.Imaging;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Text;
using SpreadsheetGear;
using System.Xml;
using System.Collections.Generic;
using DevInfo.Lib.DI_LibMap;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Connection;
using Ionic.Zip;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Xml.Serialization;

public partial class libraries_aspx_ExportMap : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (Request.Form["ExportType"] != null && Request.Form["filename"] != null)
            {
                string tSvg = string.Empty;
                string tType = Request.Form["ExportType"].ToString();
                string tFileName = Request.Form["filename"].ToString();
                string TempPath = HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV;

                tFileName = Global.GetValidFileName(tFileName);

                MemoryStream tStream = new MemoryStream();
                string tTmp = new Random().Next().ToString();

                string tExt = string.Empty;
                string tTypeString = string.Empty;
                string dbNid = string.Empty;
                string langCode = string.Empty;
                float height = 0, width = 0;
                bool includeLegend = false;
                string ThemeId = string.Empty;

                switch (tType)
                {
                    case "image":
                        if (Request.Form["extension"] != null)
                        {
                            tExt = Request.Form["extension"].ToString();
                        }
                        else
                        {
                            tExt = "png";
                        }

                        switch (tExt)
                        {
                            case "png":
                                tTypeString = "-m image/png";
                                break;
                            case "gif":
                                tTypeString = "-m image/gif";
                                break;
                            case "jpg":
                                tTypeString = "-m image/jpeg";
                                break;
                            case "bmp":
                                tTypeString = "-m image/x-ms-bmp";
                                break;
                            default:
                                tTypeString = "-m image/png";
                                tExt = "png";
                                break;
                        }

                        break;
                    case "kmz":
                        tTypeString = "application/vnd.google-earth.kmz kmz";
                        tExt = "kmz";
                        break;
                    case "excel":
                        tTypeString = "application/vnd.xls";
                        tExt = "xls";
                        break;
                    case "legendinfo":
                        tTypeString = "text/xml";
                        tExt = "xml";
                        break;
                }

                if (tTypeString != "")
                {
                    string tWidth = string.Empty;
                    Svg.SvgDocument tSvgObj = null;
                    tSvgObj = new SvgDocument();

                    switch (tExt)
                    {
                        case "png":
                        case "jpg":
                        case "bmp":
                        case "gif":
                            if (Request.Form["height"] != null)
                            {
                                height = float.Parse(Request.Form["height"]);
                            }

                            if (Request.Form["width"] != null)
                            {
                                width = float.Parse(Request.Form["width"].ToString());
                            }

                            if (Request.Form["includelegend"] != null)
                            {
                                includeLegend = Convert.ToBoolean(Request.Form["includelegend"].ToString());
                            }
                            else
                            { includeLegend = true; }
                            if (Request.Form["themeId"] != null)
                            {
                                tStream = this.ExportZippedPngMaps(tExt, includeLegend, height, width);
                                if (tFileName.Contains("("))
                                {
                                    tFileName = tFileName.Remove(tFileName.LastIndexOf("("));                                  
                                }
                                tExt = "zip";
                            }
                            else
                            {
                               this.ExportPngMap(tStream, tExt, includeLegend, height, width);
                            }
                            break;
                        case "kmz":
                            if (Request.Form["includelegend"] != null)
                            {
                                includeLegend = Convert.ToBoolean(Request.Form["includelegend"].ToString());
                            }
                            else
                            { includeLegend = true; }

                            this.ExportKMZMap(tStream, includeLegend);
                            break;
                        case "xls":
                            if (Request.Form["dbnid"] != null)
                            {
                                dbNid = Request.Form["dbnid"].ToString();
                            }
                            else
                            {
                                dbNid = Global.GetDefaultDbNId();
                            }
                            if (Request.Form["langCode"] != null)
                            {
                                langCode = Request.Form["langCode"].ToString();
                            }
                            else
                            {
                                langCode = Global.GetDefaultLanguageCode();
                            }

                            MemoryStream excelStream = new MemoryStream();
                            if (Request.Form["themeId"] != null)
                            {
                                if (tFileName.Contains("("))
                                {
                                    tFileName = tFileName.Remove(tFileName.LastIndexOf("("));
                                }
                            }
                            this.ExportExcelMap(excelStream);
                            tStream = Global.GetZipFileStream(excelStream, tFileName + "." + tExt + "");
                            //tStream = Global.GetZipFileStream(excelStream, tFileName + "." + tExt + "", TempPath);
                            tExt = "zip";
                            excelStream.Dispose();
                            break;
                        case "xml":
                            if (tType == "legendinfo")
                            {
                                if (Request.Form["themeId"] != null)
                                {
                                    ThemeId = Request.Form["themeId"].ToString();
                                }

                                if (!string.IsNullOrEmpty(ThemeId))
                                {
                                    FileStream fs = new FileStream(this.DownloadLegends(ThemeId, tFileName), FileMode.Open);
                                    fs.CopyTo(tStream);
                                }
                            }
                            break;
                    }

                    //tFileName = Global.GetValidFileNameURLSupport(tFileName, "_");

                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = tTypeString;
                    Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + tFileName + "." + tExt + "\"");
                    Response.BinaryWrite(tStream.ToArray());
                    Response.End();

                }
            }
        }
    }

    /// <summary>
    /// Export kmz file
    /// </summary>
    /// <param name="tStream"></param>
    /// <param name="includeLegend"></param>
    public void ExportKMZMap(MemoryStream tStream, bool includeLegend)
    {
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString());
        Map diMap = null;
        Themes themes = null;
        string MissingLegendTitle = string.Empty;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //default setting for legend missing data set and reset after export
            themes = (Themes)diMap.Themes.Clone();
            foreach (Theme theme in diMap.Themes)
            {
                if (theme.Legends != null && theme.Legends.Count > 0)
                {
                    MissingLegendTitle = theme.Legends[theme.Legends.Count - 1].Title;
                    theme.Legends[theme.Legends.Count - 1].Title = string.Empty;
                }
            }

            diMap.Save(tStream, TempFileWPath + ".kmz", GoogleMapFileType.KMZ, includeLegend, Path.GetDirectoryName(TempFileWPath));
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ExportKMZMap : ->" + ex.Message);
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            if (themes != null)
            {
                //reset legend setting after export
                foreach (Theme theme in themes)
                {
                    if (theme.Legends != null && theme.Legends.Count > 0)
                    {
                        MissingLegendTitle = theme.Legends[theme.Legends.Count - 1].Title;
                        theme.Legends[theme.Legends.Count - 1].Title = MissingLegendTitle;
                    }
                }

            }
        }

    }

    /// <summary>
    /// Export png
    /// </summary>
    /// <param name="tStream"></param>
    /// <param name="imageExt"></param>
    /// <param name="includeLegend"></param>
    /// <param name="height"></param>
    /// <param name="width"></param>
    public void ExportPngMap(MemoryStream tStream, string imageExt, bool includeLegend, float height, float width)
    {
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString());
        Map diMap = null;
        Themes themes = null;
        string MissingLegendTitle = string.Empty;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            if (height == 0)
            {
                height = diMap.Height;
            }

            if (width == 0)
            {
                width = diMap.Width;
            }

            if (includeLegend)
            {
                //default setting for legend missing data set and reset after export
                themes = (Themes)diMap.Themes.Clone();
                foreach (Theme theme in diMap.Themes)
                {
                    if (theme.Legends != null && theme.Legends.Count > 0)
                    {
                        MissingLegendTitle = theme.Legends[theme.Legends.Count - 1].Title;
                        theme.Legends[theme.Legends.Count - 1].Title = string.Empty;
                    }
                }
            }
            diMap.GetCompositeMapImage(tStream, imageExt, Path.GetDirectoryName(TempFileWPath), false, string.Empty, includeLegend, true, width, height);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ExportPngMap : ->" + ex.Message);
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            if (themes != null && includeLegend)
            {
                //reset legend setting after export
                foreach (Theme theme in themes)
                {
                    if (theme.Legends != null && theme.Legends.Count > 0)
                    {
                        MissingLegendTitle = theme.Legends[theme.Legends.Count - 1].Title;
                        theme.Legends[theme.Legends.Count - 1].Title = MissingLegendTitle;
                    }
                }

            }
        }

    }


    public MemoryStream ExportZippedPngMaps(string imageExt, bool includeLegend, float height, float width)
    {
        MemoryStream retVal = null;
        string temFileName = DateTime.Now.Ticks.ToString();
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, temFileName);
        Map diMap = null;
        Themes themes = null;
        string MissingLegendTitle = string.Empty;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            if (height == 0)
            {
                height = diMap.Height;
            }

            if (width == 0)
            {
                width = diMap.Width;
            }

            if (includeLegend)
            {
                //default setting for legend missing data set and reset after export
                themes = (Themes)diMap.Themes.Clone();
                foreach (Theme theme in diMap.Themes)
                {
                    if (theme.Legends != null && theme.Legends.Count > 0)
                    {
                        MissingLegendTitle = theme.Legends[theme.Legends.Count - 1].Title;
                        theme.Legends[theme.Legends.Count - 1].Title = string.Empty;
                    }
                }
            }           
                string themeId = Request.Form["themeId"].ToString();
                string inputStr = Request.Form["inputStr"].ToString();
                string[] param = Global.SplitString(inputStr, Constants.Delimiters.ParamDelimiter);
                Dictionary<string, MemoryStream> dicStreamFileName = diMap.GetCompositeMapImageForTp(imageExt, Path.GetDirectoryName(TempFileWPath), false, string.Empty, includeLegend, true, width, height, themeId, param);
                retVal = Global.GetZipFileStream(dicStreamFileName, imageExt);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ExportPngMap : ->" + ex.Message);
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            if (themes != null && includeLegend)
            {
                //reset legend setting after export
                foreach (Theme theme in themes)
                {
                    if (theme.Legends != null && theme.Legends.Count > 0)
                    {
                        MissingLegendTitle = theme.Legends[theme.Legends.Count - 1].Title;
                        theme.Legends[theme.Legends.Count - 1].Title = MissingLegendTitle;
                    }
                }

            }
        }
        return retVal;
    }


    /// <summary>
    /// Export excel map
    /// </summary>
    /// <param name="tStream"></param>
    /// <param name="dbNid"></param>
    /// <param name="langCode"></param>
    public void ExportExcelMap(MemoryStream tStream)
    {
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV);
        Map diMap = null;
        string MissingLegendTitle = string.Empty;
        Themes themes = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //default setting for legend missing data set and reset after export
            themes = (Themes)diMap.Themes.Clone();
            foreach (Theme theme in diMap.Themes)
            {
                if (theme.Legends != null && theme.Legends.Count > 0)
                {
                    MissingLegendTitle = theme.Legends[theme.Legends.Count - 1].Title;
                    theme.Legends[theme.Legends.Count - 1].Title = string.Empty;
                }
            }
            if (Request.Form["themeId"] != null)
            {
                string themeId = Request.Form["themeId"].ToString();
                string inputStr = Request.Form["inputStr"].ToString();
                string[] param = Global.SplitString(inputStr, Constants.Delimiters.ParamDelimiter);
                diMap.GeneratePresentationWithTp(tStream, HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, "emf", themeId, param);
            }
            else
            {
                diMap.GeneratePresentation(tStream, HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, "emf");
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ExportExcelMap : ->" + ex.Message);
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            if (themes != null)
            {
                //reset legend setting after export
                foreach (Theme theme in themes)
                {
                    if (theme.Legends != null && theme.Legends.Count > 0)
                    {
                        MissingLegendTitle = theme.Legends[theme.Legends.Count - 1].Title;
                        theme.Legends[theme.Legends.Count - 1].Title = MissingLegendTitle;
                    }
                }

            }
        }

    }

    /// <summary>
    /// <LegendInfo><Legends></Legends><\LegendInfo>
    /// </summary>
    /// <param name="ThemeID"></param>
    /// <returns>File Name WithPath</returns>
    public string DownloadLegends(string ThemeID, string fileName)
    {
        string RetVal = string.Empty;
        Map diMap = null;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".xml");
        string LegendColor = string.Empty;
        string LegendTitle = string.Empty;
        Theme theme;
        LegendInfo LegendInfo = new LegendInfo();

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            //<LegendInfo><Legends></Legends><\LegendInfo>
            diMap = this.GetSessionMapObject();
            theme = diMap.Themes[ThemeID];
            LegendInfo.LegendBreakCount = theme.BreakCount;
            LegendInfo.LegendBreakType = theme.BreakType;
            LegendInfo.LegendChartType = theme.ChartType;

            if (theme.Legends != null && theme.Legends.Count > 0)
            {
                LegendInfo.Legends = theme.Legends;
            }

            this.SerializeObject(TempFileWPath, LegendInfo);

            RetVal = TempFileWPath;
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    private Map GetSessionMapObject()
    {
        Map RetVal = null;
        string MapFileWPath = string.Empty;

        if (Session["DataViewNonQDS"] == null)
        {
            MapFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            //Get session id into cookies
            if (File.Exists(MapFileWPath))
            {
                RetVal = Map.Load(MapFileWPath);

                Session["DataViewNonQDS"] = RetVal.MapData;
                Session["CurrentSelectedAreaNids"] = RetVal.SelectedAreaIDNids;
                Session["IsMyData"] = RetVal.isMyData;
                Session["DIMap"] = RetVal;
            }
        }
        else
        {
            if (Session["DIMap"] == null)
            {
                RetVal = new Map();
                RetVal.MapData = (DataTable)Session["DataViewNonQDS"];
                RetVal.SelectedAreaIDNids = (string)Session["CurrentSelectedAreaNids"];
                RetVal.isMyData = Convert.ToBoolean(Session["IsMyData"]);

                Session["DIMap"] = RetVal;
            }
            else
            {
                RetVal = (Map)Session["DIMap"];
            }
        }

        return RetVal;
    }

    private void SerializeObject(string p_FileNameWPath, Object serializeObj)
    {
        FileStream _IO = null;
        try
        {
            string DirPath = string.Empty;
            DirPath = Path.GetDirectoryName(p_FileNameWPath);

            if (!Directory.Exists(DirPath))
                Directory.CreateDirectory(DirPath);

            _IO = new FileStream(p_FileNameWPath, FileMode.Create);
            XmlSerializer _SRZFrmt = new XmlSerializer(serializeObj.GetType());
            _SRZFrmt.Serialize(_IO, serializeObj);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            //Global.WriteErrorsInLog("From SerializeObject Object : " + serializeObj.GetType().ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);
        }
        finally
        {
            _IO.Flush();
            _IO.Close();
        }
    }

}
