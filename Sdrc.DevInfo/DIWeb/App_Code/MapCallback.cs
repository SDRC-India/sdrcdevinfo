using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevInfo.Lib.DI_LibMap;
using System.IO;
using System.Collections.Generic;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Drawing;
using System.Globalization;
using Microsoft.VisualBasic;
using System.Xml.Serialization;
using System.Xml;
using System.Collections;
using System.Drawing.Drawing2D;
using System.Data.Common;
using System.Drawing.Imaging;
using System.Text;
using System.Linq;
using System.Web.Script.Serialization;
using DevInfo.Lib.DI_LibBAL.Controls.TimeperiodBAL;
using Newtonsoft.Json;


/// <summary>
/// Summary description for MapCallback
/// </summary>
public partial class Callback : System.Web.UI.Page
{

    #region "--Private--"

    #region "--Variable--"
    /// <summary>
    /// Default start date "1/1/1800" for chronical representivity of map file in en-US culture format (MM/DD/YYYY) 
    /// </summary>
    private const string DEFAULT_START_DATE = "1/1/1800";

    /// <summary>
    /// Default end date "12/31/3000" for chronical representivity of map file in en-US culture format (MM/DD/YYYY) 
    /// </summary>
    private const string DEFAULT_END_DATE = "12/31/3000"; //12/31/2200 'thai era exceeds 2200 years so extended to 3000

    private string TempPath = HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV;

    private StringBuilder serverResponseTimeBuilder = new StringBuilder();

    public string BrowserType = string.Empty;

    #endregion

    #region "--Methods--"

    #region "--Serialize XML--"

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
            //Global.WriteErrorsInLog("From SerializeObject Object : " + serializeObj.GetType().ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);
            Global.CreateExceptionString(ex, "From SerializeObject Object : " + serializeObj.GetType().ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);

        }
        finally
        {
            _IO.Flush();
            _IO.Close();
        }
    }

    private Object DeSerializeObject(string p_FileNameWPath, Type type)
    {
        Object RetVal = null;
        FileStream _IO = null;
        try
        {
            string DirPath = string.Empty;
            DirPath = Path.GetDirectoryName(p_FileNameWPath);

            if (!Directory.Exists(DirPath))
                Directory.CreateDirectory(DirPath);

            _IO = new FileStream(p_FileNameWPath, FileMode.Open);
            XmlSerializer _SRZFrmt = new XmlSerializer(type);
            RetVal = _SRZFrmt.Deserialize(_IO);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From DeSerializeObject Object : " + type.ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);
            Global.CreateExceptionString(ex, "From DeSerializeObject Object : " + type.ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);

        }
        finally
        {
            _IO.Flush();
            _IO.Close();
        }
        return RetVal;
    }

    #endregion

    #region "-- Map --"

    private bool SetTimeLogIntoFile(string stringText)
    {
        bool RetVal = false;
        StreamWriter writer = null;
        try
        {
            writer = new StreamWriter(Path.Combine(this.TempPath, Constants.FileName.MapServerResponseTime), true);
            writer.WriteLine(stringText);
            RetVal = true;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
            }
        }

        return RetVal;
    }

    private string GetXmlOfSerializeObject(string TempFileWPath, Object objToSerialize)
    {
        string RetVal = string.Empty;
        try
        {
            this.SerializeObject(TempFileWPath, objToSerialize);
            RetVal = XmlFileReader.ReadXML(TempFileWPath, string.Empty);

            if (File.Exists(TempFileWPath))
            {
                try
                {
                    File.Delete(TempFileWPath);
                }
                catch (Exception ex)
                {
                    Global.CreateExceptionString(ex, null);
                }
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetXmlOfSerializeObject Object : " + objToSerialize.GetType().ToString() + "->" + " File:" + TempFileWPath + " Error: " + ex.Message);
            Global.CreateExceptionString(ex, "From GetXmlOfSerializeObject Object : " + objToSerialize.GetType().ToString() + "->" + " File:" + TempFileWPath + " Error: " + ex.Message);

            throw;
        }
        return RetVal;
    }

    private Object GetSerializeObjectFromXml(string p_FileNameWPath, Type type)
    {
        Object RetVal = null;
        try
        {
            RetVal = this.DeSerializeObject(p_FileNameWPath, type);

            if (File.Exists(p_FileNameWPath))
            {
                try
                {
                    File.Delete(p_FileNameWPath);
                }
                catch (Exception ex)
                {
                    Global.CreateExceptionString(ex, null);
                }
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetSerializeObjectFromXml Object : " + type.ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);
            Global.CreateExceptionString(ex, "From GetSerializeObjectFromXml Object : " + type.ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);

        }
        return RetVal;
    }

    private string InitializeBaseMap(float width, float Height, string dbNId, string langCode, bool isSingleThemeMap, bool useMapServer)
    {
        if (Session["IsMapServer"] != null && Convert.ToBoolean(Session["IsMapServer"]))
        {
            bool usingMapServer = Convert.ToBoolean(Session["IsMapServer"]);
            if (usingMapServer)
            {
                langCode = Global.GetMapServerLangCode(langCode);
            }
        }
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        string TemplateStyleFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Template);
        string LanguageFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Language);
        string MapFolder = string.Empty;
        string DisputedBoundriesFolder = string.Empty;
        Map diMap = null;
        DataTable dtSessionData = null;
        DataTable SeriesDataTable = null;
        SeriesData seriesData = null;
        SourceData sourceData = new SourceData();
        string seriesID = string.Empty;
        string seriesName = string.Empty;
        string FilterText = string.Empty;
        bool isUpdateMap = true;

        string themeID = string.Empty;
        List<string> IUSName = null;
        bool isMrdColumnAdded = false;

        try
        {
            //First step To load map from session/ NewMap/ From preserved file
            this.SetTimeLogIntoFile("Get map Object-> " + DateTime.Now.ToLongTimeString());
            diMap = this.GetSessionMapObject(ref isUpdateMap, ref isMrdColumnAdded);
            diMap.IsMRDColumnAddedInTable = isMrdColumnAdded;
            diMap.UseMapServer = useMapServer;
            //diMap.CanvasColor = Color.Red;
           // diMap.MissingColor = Color.Black;
          
            if (diMap.Layers.Count > 0)
            {
                Layer layer = diMap.Layers[0];
                // diMap.Layers. = ly;
                // ly = Color.Blue;
            }
            if (isUpdateMap)
            {
                //2. Set datatable and areaNids
                dtSessionData = diMap.MapData;

                //get seriesData from session table 
                this.SetTimeLogIntoFile("Get series Info-> " + DateTime.Now.ToLongTimeString());
                seriesData = this.GetSeriesInfo(diMap.MapDataColumns, dtSessionData, diMap.isMyData, dbNId, langCode);
                seriesID = seriesData[0].SeriesID;
                seriesName = seriesData[0].SeriesName;

                if (!diMap.isMyData)
                {
                    this.SetTimeLogIntoFile("Start Get Source Info-> " + DateTime.Now.ToLongTimeString());
                    sourceData = this.GetSourceInfo(diMap.MapDataColumns, dtSessionData, diMap.isMyData, dbNId, langCode);
                    this.SetTimeLogIntoFile("End Get Source Info-> " + DateTime.Now.ToLongTimeString());
                }

                if (dtSessionData.Columns.Contains(diMap.MapDataColumns.isMRD) && diMap.IsMRDColumnAddedInTable == false)
                {
                    FilterText = diMap.MapDataColumns.SeriesNid + " in(" + seriesID + ") and " + diMap.MapDataColumns.isMRD + "='true' ";
                }
                else
                {
                    FilterText = diMap.MapDataColumns.SeriesNid + " in(" + seriesID + ") ";
                }

                //TODO Get Path from WebService

                ////////MapFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.Maps);
                ////////DisputedBoundriesFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.DisputedBoundries);

                bool usingMapServer = false;//Convert.ToBoolean(isMapServer.Trim());
                if (Session["IsMapServer"] != null)
                {
                    usingMapServer = Convert.ToBoolean(Session["IsMapServer"]);
                }
                if (!usingMapServer)
                {
                    MapFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.Maps);
                    DisputedBoundriesFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.DisputedBoundries);
                }
                else
                {
                    // string mapServerFileDirectory = "D:\\MapServer\\stock\\data\\1\\maps";
                    string mapServerFileDirectory = Global.GetMapServerDirectory();
                    MapFolder = mapServerFileDirectory;
                    DisputedBoundriesFolder = Path.Combine(mapServerFileDirectory + "\\DIB\\");
                }


                diMap.Width = width;
                diMap.Height = Height;
                diMap.SpatialMapFolder = MapFolder;
                diMap.DIBFolderPath = DisputedBoundriesFolder;
                diMap.FirstColor = ColorTranslator.FromHtml("#b6effd");
                diMap.FourthColor = ColorTranslator.FromHtml("#1c84a0");
                diMap.SeriesData = seriesData;
                diMap.SourceData = sourceData;
                diMap.Title = seriesName;
                diMap.LabelEffectSettings = new LabelEffectSetting();

                diMap.MultiRowLabel = false;

                diMap.AngledNudgeLdrLine = true;
                diMap.ShowPointStartingLdrLine = true;
                diMap.ShowLabelWithNewPattern = true;
                //adding 3 level items in the list
                if (diMap.ShowLabelWithNewPattern && diMap.LevelFontSettingList.Count == 0)
                {
                    this.SetLevelWiseLabelsFont(diMap.LevelFontSettingList, diMap.TemplateStyle.LabelFontSetting.FontTemplate.Font(), diMap.TemplateStyle.LabelFontSetting.FontTemplate.ForeColor);
                }

                this.SetTimeLogIntoFile("Template Style Loading.. -> " + DateTime.Now.ToLongTimeString());
                diMap.TemplateStyle = StyleTemplate.LoadStyleTemplate(Path.Combine(TemplateStyleFolderPath, Constants.Map.MapLanguageFileName));

                diMap.ScaleUnitText = "KM";

                //TODO language based strings        
                this.SetTimeLogIntoFile("GetNSet language string -> " + DateTime.Now.ToLongTimeString());
                diMap.LanguageStrings = this.GetMapLanguageStrings(LanguageFolderPath, langCode);
                if (diMap.LanguageStrings == null)
                {
                    diMap.LanguageStrings = new LanguageStrings();
                }

                this.SetTimeLogIntoFile("Set Map layers-> " + DateTime.Now.ToLongTimeString());
                this.SetMapLayers(diMap, dbNId, langCode);
                dtSessionData = diMap.MapData;

                diMap.SetFullExtent();

                //get series data by applying default filter
                this.SetTimeLogIntoFile("Get series Data-> " + DateTime.Now.ToLongTimeString());
                SeriesDataTable = this.GetSeriesData(dtSessionData, FilterText);

                diMap.Themes.Clear();
                this.SetTimeLogIntoFile("Create Theme-> " + DateTime.Now.ToLongTimeString());
                diMap.CreateTheme(SeriesDataTable.DefaultView, ThemeType.Color, 4, BreakType.EqualCount, ChartType.Column, seriesID, -1);
                if (diMap.Themes.Count > 0)
                {
                    IUSName = new List<string>(seriesName.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                    themeID = diMap.Themes[diMap.Themes.Count - 1].ID;
                    //diMap.Themes[themeID].Name = diMap.LanguageStrings.Theme + " - " + seriesName;
                    //diMap.Themes[themeID].LegendTitle = seriesName;
                    diMap.Themes[themeID].Name = IUSName[IUSName.Count - 1].Trim();
                    diMap.Themes[themeID].LegendTitle = diMap.Themes[themeID].Name;
                    diMap.Themes[themeID].DotColor = ColorTranslator.FromHtml("#000000");
                    diMap.Themes[themeID].isMRD = true;

                    //Bug no: 4850 [see all list of time period [ but timeperiod was not visible because is mrd check was there
                    //diMap.Themes[diMap.Themes[diMap.Themes.Count - 1].ID].TimePeriods = this.GetCSVInList(diMap.MapData, diMap.MapDataColumns.TimePeriod, true, FilterText);
                    diMap.Themes[themeID].TimePeriods = this.GetCSVInList(diMap.MapData, diMap.MapDataColumns.TimePeriod, true, diMap.MapDataColumns.SeriesNid + " in(" + seriesID + ") ");

                    diMap.setThemeRange(themeID, SeriesDataTable.DefaultView.Table);
                }

                //as per requirement TimePeriod or Most recent data append after map title 
                this.AppendTimePeriodinMapTitle(diMap, dtSessionData, FilterText);

                //add map into  session variable
                this.SetTimeLogIntoFile("Set session variable-> " + DateTime.Now.ToLongTimeString());
                Session["DIMap"] = diMap;
                Session["DataViewNonQDS"] = diMap.MapData;

                //serialize map in last
                MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
                //this.SerializeObject(MapFileWPath, diMap);
                this.SetTimeLogIntoFile("Start Saving Map into file named-> " + Path.GetFileNameWithoutExtension(MapFileWPath) + " Time:" + DateTime.Now.ToLongTimeString());
                diMap.Serialize(MapFileWPath);
            }

            this.SetTimeLogIntoFile("Get Map Image Starts-> " + DateTime.Now.ToLongTimeString());

            RetVal = SetBorderStyle("true", "1", "#000000", "Solid");
           // RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));
            this.SetTimeLogIntoFile("Get Map Image Ends-> " + DateTime.Now.ToLongTimeString());
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From InitializeBaseMap hdbnid : " + dbNId + "->" + ex.Message);
            Global.CreateExceptionString(ex, "From InitializeBaseMap hdbnid : " + dbNId + "->" + ex.Message);

            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }
        finally
        {
        }

        return RetVal;
    }

    private bool ReCreateDefaultTheme(Map diMap)
    {
        bool RetVal = false;
        string MapFileWPath = string.Empty;
        string TemplateStyleFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Template);
        string LanguageFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Language);
        string MapFolder = string.Empty;
        string DisputedBoundriesFolder = string.Empty;
        //Map diMap = null;
        DataTable dtSessionData = null;
        DataTable SeriesDataTable = null;
        SeriesData seriesData = null;
        SourceData sourceData = new SourceData();
        string seriesID = string.Empty;
        string seriesName = string.Empty;
        string FilterText = string.Empty;
        bool isUpdateMap = true;

        string themeID = string.Empty;
        List<string> IUSName = null;
        List<string> VisibleIUSNids = null;
        List<string> VisibleSourceNids = null;

        try
        {

            //First step To load map from session/ NewMap/ From preserved file
            if (isUpdateMap)
            {
                //2. Set datatable and areaNids
                dtSessionData = diMap.MapData;
                seriesData = diMap.SeriesData;
                sourceData = diMap.SourceData;

                ////Pending see after bug: remove entry from Source data  where iusnid not seleted
                //VisibleSourceNids = new List<string>(diMap.VisibleIndicatorNids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                //for (int i = 0; i < sourceData.Count; i++)
                //{
                //    if (!VisibleIUSNids.Contains(sourceData[i].SourceNId))
                //    {
                //        sourceData.Remove(Convert.ToInt32(sourceData[i].SourceNId));
                //        i--;
                //    }
                //}
                //diMap.SourceData = sourceData;

                //Remove series from which is not selected
                seriesID = seriesData[0].SeriesID;
                seriesName = seriesData[0].SeriesName;

                if (dtSessionData.Columns.Contains(diMap.MapDataColumns.isMRD))
                {
                    FilterText = diMap.MapDataColumns.SeriesNid + " in(" + seriesID + ") and " + diMap.MapDataColumns.isMRD + "='true' ";
                }
                else
                {
                    FilterText = diMap.MapDataColumns.SeriesNid + " in(" + seriesID + ") ";
                }

                diMap.Title = seriesName;

                diMap.SetFullExtent();

                SeriesDataTable = this.GetSeriesData(dtSessionData, FilterText);

                diMap.Themes.Clear();
                diMap.CreateTheme(SeriesDataTable.DefaultView, ThemeType.Color, 4, BreakType.EqualCount, ChartType.Column, seriesID, -1);
                if (diMap.Themes.Count > 0)
                {
                    IUSName = new List<string>(seriesName.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                    themeID = diMap.Themes[diMap.Themes.Count - 1].ID;
                    //diMap.Themes[themeID].Name = diMap.LanguageStrings.Theme + " - " + seriesName;
                    //diMap.Themes[themeID].LegendTitle = seriesName;
                    diMap.Themes[themeID].Name = IUSName[IUSName.Count - 1].Trim();
                    diMap.Themes[themeID].LegendTitle = diMap.Themes[themeID].Name;
                    diMap.Themes[themeID].DotColor = ColorTranslator.FromHtml("#000000");
                    diMap.Themes[themeID].isMRD = true;

                    //Bug no: 4850 [see all list of time period [ but timeperiod was not visible because is mrd check was there
                    //diMap.Themes[diMap.Themes[diMap.Themes.Count - 1].ID].TimePeriods = this.GetCSVInList(diMap.MapData, diMap.MapDataColumns.TimePeriod, true, FilterText);
                    diMap.Themes[themeID].TimePeriods = this.GetCSVInList(diMap.MapData, diMap.MapDataColumns.TimePeriod, true, diMap.MapDataColumns.SeriesNid + " in(" + seriesID + ") ");

                    diMap.setThemeRange(themeID, SeriesDataTable.DefaultView.Table);
                }

                //as per requirement TimePeriod or Most recent data append after map title 
                this.AppendTimePeriodinMapTitle(diMap, dtSessionData, FilterText);
            }

            RetVal = true;
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ReCreateDefaultTheme : ->" + ex.Message);
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    private void SetLevelWiseLabelsFont(List<FontSetting> LevelFontSettingList, Font LevelFont, Color ForeColor)
    {
        FontSetting fSetting = null;
        if (LevelFontSettingList.Count == 0)
        {
            fSetting = new FontSetting(LevelFont, ForeColor, Color.Transparent);
            fSetting.FontSize = 12;
            //fSetting.ForeColor = Color.Green;
            LevelFontSettingList.Add(fSetting);

            fSetting = new FontSetting(LevelFont, ForeColor, Color.Transparent);
            fSetting.FontSize = 11;
            //fSetting.ForeColor = Color.Red;
            LevelFontSettingList.Add(fSetting);
        }
    }

    private void AppendTimePeriodinMapTitle(Map diMap, DataTable dtSessionData, string filterText)
    {
        DataRow[] rows = null;
        string TextToAppend = string.Empty;
        try
        {
            dtSessionData.DefaultView.RowFilter = filterText;
            dtSessionData = dtSessionData.DefaultView.ToTable();

            rows = dtSessionData.DefaultView.ToTable(true, diMap.MapDataColumns.TimePeriod).Select();
            if (rows != null && rows.Length > 0)
            {
                TextToAppend = Convert.ToString(rows[0][diMap.MapDataColumns.TimePeriod]);

                if (rows.Length == 1 && !string.IsNullOrEmpty(TextToAppend))
                {
                    diMap.Title = diMap.Title + " (" + Convert.ToString(rows[0][diMap.MapDataColumns.TimePeriod]) + ")";
                }
                else
                {
                    diMap.Title = diMap.Title + " (" + diMap.LanguageStrings.MostRecentData + ")";
                }
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From AppendTimePeriodinMapTitle : " + "->" + ex.Message);
            Global.CreateExceptionString(ex, null);

        }

    }

    private List<string> GetCSVInList(DataTable dataTable, string columnName, bool isSortByGivenColumn, string filterText)
    {
        List<string> RetVal = new List<string>();

        try
        {
            dataTable.DefaultView.RowFilter = filterText;
            dataTable = dataTable.DefaultView.ToTable();

            //TODO append only distinct elements
            if (dataTable.Columns.Contains(columnName))
            {
                if (isSortByGivenColumn)
                    dataTable.DefaultView.Sort = columnName;

                foreach (DataRow dvRow in dataTable.DefaultView.ToTable(true, columnName).Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dvRow[columnName])))
                        RetVal.Add(Convert.ToString(dvRow[columnName]));
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw;
        }
        return RetVal;
    }

    private void AppendTimePeriodinMapTitle(Map diMap, DataTable dtSessionData)
    {
        DataRow[] rows = null;
        string TextToAppend = string.Empty;

        try
        {
            rows = dtSessionData.DefaultView.ToTable(true, diMap.MapDataColumns.TimePeriod).Select();
            if (rows != null && rows.Length > 0)
            {
                TextToAppend = Convert.ToString(rows[0][diMap.MapDataColumns.TimePeriod]);

                if (rows.Length == 1 && !string.IsNullOrEmpty(TextToAppend))
                {
                    diMap.Title = diMap.Title + " (" + Convert.ToString(rows[0][diMap.MapDataColumns.TimePeriod]) + ")";
                }
                else
                {
                    diMap.Title = diMap.Title + " (" + diMap.LanguageStrings.MostRecentData + ")";
                }
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From AppendTimePeriodinMapTitle : " + "->" + ex.Message);
            Global.CreateExceptionString(ex, null);

        }

    }

    private List<string> GetCSVInList(DataTable dataTable, string columnName, bool isSortByGivenColumn)
    {
        List<string> RetVal = new List<string>();

        try
        {
            //TODO append only distinct elements
            if (dataTable.Columns.Contains(columnName))
            {
                if (isSortByGivenColumn)
                    dataTable.DefaultView.Sort = columnName;

                foreach (DataRow dvRow in dataTable.DefaultView.ToTable(true, columnName).Rows)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(dvRow[columnName])))
                        RetVal.Add(Convert.ToString(dvRow[columnName]));
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw;
        }
        return RetVal;
    }

    private MemoryStream DrawMap(Map diMap)
    {
        MemoryStream RetVal = new MemoryStream();

        Bitmap bmp = new Bitmap(Convert.ToInt32(diMap.Width), Convert.ToInt32(diMap.Height));
        diMap.DrawMap(string.Empty, Graphics.FromImage(bmp));
        bmp.Save(RetVal, ImageFormat.Png);

        return RetVal;
    }

    private Map GetSessionMapObject()
    {
        Map RetVal = null;
        string MapFileWPath = string.Empty;
        bool isMrdColumnAdded = false;

        if (HttpContext.Current.Request.Cookies["SessionID"] == null)
        {
            Global.SaveCookie("SessionID", HttpContext.Current.Session.SessionID, this.Page);
        }

        if (Session["DataViewNonQDS"] == null)
        {
            MapFileWPath = Path.Combine(TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
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

        //TODO ensure that dtSessionData contains AreaNId column, Decimal Numeric data column
        RetVal.MapData = this.ResetDTColumns(RetVal.MapDataColumns, RetVal.MapData, RetVal.isMyData, ref isMrdColumnAdded);

        return RetVal;
    }

    /// <summary>
    /// for innitialize map case
    /// </summary>
    /// <param name="isUpdateMap"></param>
    /// <returns></returns>
    private Map GetSessionMapObject(ref bool isUpdateMap, ref bool isMrdColumnAdded)
    {
        Map RetVal = null;
        string MapFileWPath = string.Empty;
        SessionDetails sdetails = null;
        isMrdColumnAdded = false;

        if (HttpContext.Current.Request.Cookies["SessionID"] == null)
        {
            Global.SaveCookie("SessionID", HttpContext.Current.Session.SessionID, this.Page);
        }

        //if (Session["DataViewNonQDS"] == null)
        //{
        //    MapFileWPath = Path.Combine(TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
        //    //Get session id into cookies
        //    if (File.Exists(MapFileWPath))
        //    {
        //        RetVal = Map.Load(MapFileWPath);
        //        Session["DataViewNonQDS"] = RetVal.MapData;
        //        Session["CurrentSelectedAreaNids"] = RetVal.SelectedAreaIDNids;
        //        Session["IsMyData"] = RetVal.isMyData;
        //        Session["DIMap"] = RetVal;
        //    }
        //}
        //else
        //{

        if (Session["DIMap"] == null)
        {
            RetVal = new Map();

            //RetVal.MapData = (DataTable)Session["DataViewNonQDS"];
            //RetVal.SelectedAreaIDNids = (string)Session["CurrentSelectedAreaNids"];
            //RetVal.isMyData = Convert.ToBoolean(Session["IsMyData"]);

            //Handling of creating map visualization after session time out
            try
            {
                //to do get from xml
                sdetails = SessionDetails.Load(TempPath + HttpContext.Current.Request.Cookies["SessionID"].Value + "_sessionDetails" + ".xml");
            }
            catch (Exception ex)
            {
                Global.CreateExceptionString(ex, null);

            }

            if (sdetails == null)
            {
                RetVal.MapData = (DataTable)Session["DataViewNonQDS"];
                RetVal.SelectedAreaIDNids = (string)Session["CurrentSelectedAreaNids"];
                RetVal.isMyData = Convert.ToBoolean(Session["IsMyData"]);
            }
            else
            {
                RetVal.MapData = sdetails.DataViewNonQDS;
                RetVal.SelectedAreaIDNids = sdetails.CurrentSelectedAreaNids;
                RetVal.isMyData = sdetails.IsMyData;
            }

            Session["DIMap"] = RetVal;
        }
        else
        {
            Map mapFromFile = null;
            MapFileWPath = Path.Combine(TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            //Get session id into cookies
            if (File.Exists(MapFileWPath))
            {
                mapFromFile = Map.Load(MapFileWPath);
            }

            isUpdateMap = false;
            RetVal = (Map)Session["DIMap"];

            if (mapFromFile != null && mapFromFile.isMyData != RetVal.isMyData)
            {
                Session["DataViewNonQDS"] = RetVal.MapData;
                Session["CurrentSelectedAreaNids"] = RetVal.SelectedAreaIDNids;
                Session["IsMyData"] = RetVal.isMyData;
                Session["DIMap"] = RetVal;
                isUpdateMap = true;
            }
        }

        //}

        //add area IDs into session datatable
        if (RetVal.isMyData)
        {
            RetVal.SelectedAreaIDNids = this.GetCSVByCaption(RetVal.MapData, Area.AreaID);
            Session["CurrentSelectedAreaNids"] = RetVal.SelectedAreaIDNids;
        }

        //TODO ensure that dtSessionData contains AreaNId column, Decimal Numeric data column
        RetVal.MapData = this.ResetDTColumns(RetVal.MapDataColumns, RetVal.MapData, RetVal.isMyData, ref isMrdColumnAdded);

        return RetVal;
    }

    private DataTable GetSeriesData(DataTable dtSessionData, string seriesFilterText)
    {
        DataTable RetVal = null;

        dtSessionData.DefaultView.RowFilter = seriesFilterText;
        RetVal = dtSessionData.DefaultView.ToTable();

        dtSessionData.DefaultView.RowFilter = string.Empty;

        return RetVal;
    }

    private SeriesData GetSeriesInfo(DI_MapDataColumns mapDataColumns, DataTable SeriesDataTable, bool isMyData, string dbNid, string LanguageCode)
    {
        SeriesData RetVal = null;
        Hashtable Indicators = new Hashtable();
        DataTable dtDistinctIUSs = null;
        DataTable dtDistinctIndicatorNIds = null;
        Hashtable IUSs = null;
        SeriesInfo seriesInfo;
        List<string> distinctIUSNIds = new List<string>();
        List<string> columnsToSkipFilter = new List<string>();
        List<string> distinctColumns = new List<string>();
        string SeriesName = string.Empty;

        try
        {
            if (SeriesDataTable.Rows.Count > 0)
            {
                seriesInfo = new SeriesInfo();
                RetVal = new SeriesData();
                if (!isMyData)
                {
                    //get theme name IndicatorUnitSubgroup from Database

                    _DBCon = Global.GetDbConnection(int.Parse(dbNid));
                    dtDistinctIndicatorNIds = SeriesDataTable.DefaultView.ToTable(true, mapDataColumns.SeriesNid);
                    dtDistinctIUSs = SeriesDataTable.DefaultView.ToTable(true, mapDataColumns.SeriesNid);
                    this.SetTimeLogIntoFile("Start getting IUSs Nmaes from Procedure sp_getIUSNames_-> " + DateTime.Now.ToLongTimeString());
                    IUSs = getIUSNames(dtDistinctIUSs, LanguageCode);
                    this.SetTimeLogIntoFile("Ends getting IUSs Nmaes from Procedure sp_getIUSNames_-> " + DateTime.Now.ToLongTimeString());
                    foreach (object key in IUSs.Keys)
                    {
                        seriesInfo = new SeriesInfo();
                        seriesInfo.SeriesID = key.ToString();
                        seriesInfo.SeriesName = IUSs[key].ToString();
                        RetVal.Add(seriesInfo);
                    }
                }
                else
                {
                    columnsToSkipFilter.Add(mapDataColumns.AreaID);
                    columnsToSkipFilter.Add(mapDataColumns.AreaName);
                    columnsToSkipFilter.Add(mapDataColumns.DataValue);
                    columnsToSkipFilter.Add(mapDataColumns.NumericData);
                    columnsToSkipFilter.Add(mapDataColumns.TimePeriod);
                    columnsToSkipFilter.Add(mapDataColumns.isMRD);

                    //set row filter
                    foreach (DataColumn column in SeriesDataTable.Columns)
                    {
                        if (!columnsToSkipFilter.Contains(column.ColumnName))
                        {
                            distinctColumns.Add(column.ColumnName);
                        }
                    }

                    dtDistinctIndicatorNIds = SeriesDataTable.DefaultView.ToTable(true, distinctColumns.ToArray());
                    foreach (DataRow row in dtDistinctIndicatorNIds.Rows)
                    {
                        seriesInfo = new SeriesInfo();
                        SeriesName = string.Empty;
                        seriesInfo.SeriesID = Convert.ToString(row[mapDataColumns.SeriesNid]);
                        foreach (string columnName in distinctColumns)
                        {
                            if (columnName != mapDataColumns.SeriesNid)
                            {
                                SeriesName += Convert.ToString(row[columnName]) + ", ";
                            }
                        }

                        seriesInfo.SeriesName = SeriesName.TrimEnd(", ".ToCharArray());
                        RetVal.Add(seriesInfo);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw;
        }

        return RetVal;
    }

    private SourceData GetSourceInfo(DI_MapDataColumns mapDataColumns, DataTable sessionData, bool isMyData, string dbNid, string LanguageCode)
    {
        SourceData RetVal = null;
        DataTable SourceTable = null;
        SourceInfo sourceInfo;
        try
        {
            if (sessionData.Rows.Count > 0)
            {
                sourceInfo = new SourceInfo();
                RetVal = new SourceData();

                //get theme name IndicatorUnitSubgroup from Database
                _DBCon = Global.GetDbConnection(int.Parse(dbNid));
                //Step 2 Get IUS and Source information
                //Step 2.1. Get Source Information from Database
                List<DbParameter> DbParams = null;
                DbParameter sourceParam1 = _DBCon.CreateDBParameter();
                sourceParam1.ParameterName = "SourceNIds";
                sourceParam1.DbType = DbType.String;
                sourceParam1.Value = this.AddQuotesInCommaSeperated(this.GetCSV(sessionData, mapDataColumns.SourceNid), false);

                DbParams = new List<DbParameter>();
                DbParams.Add(sourceParam1);
                SourceTable = _DBCon.ExecuteDataTable("sp_get_sources_from_nids_" + LanguageCode, CommandType.StoredProcedure, DbParams).DefaultView.ToTable(true, mapDataColumns.SourceNid, mapDataColumns.SourceName);

                foreach (DataRow row in SourceTable.Rows)
                {
                    sourceInfo = new SourceInfo();
                    sourceInfo.SourceNId = row[mapDataColumns.SourceNid].ToString();
                    sourceInfo.SourceName = row[mapDataColumns.SourceName].ToString();
                    RetVal.Add(sourceInfo);
                }

            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw;
        }

        return RetVal;
    }

    private DataTable ResetDTColumns(DI_MapDataColumns mapDataColumns, DataTable dtSessionData, bool isMyData, ref bool isMrdColumnAdded)
    {
        DataTable RetVal = null;
        string dataValue = string.Empty;
        string IUSNId = string.Empty;
        bool isNumericColumnAdded = false;
        DataTable distinctSessionTable = null;
        isMrdColumnAdded = false;

        try
        {
            //1. if my data case then transform myData format to MapData 
            if (isMyData && !dtSessionData.Columns.Contains(mapDataColumns.SeriesNid))
            {
                dtSessionData = this.TransformTableMyDataCase(mapDataColumns, dtSessionData, ref isMrdColumnAdded);
            }

            //2. set table name
            dtSessionData.TableName = "dvTable";

            //3. Add data_value column as NumericData with datatype decimal. This will handle texual values
            isNumericColumnAdded = this.CheckNCreateColumn(dtSessionData, mapDataColumns.NumericData, typeof(decimal));

            if (isNumericColumnAdded)
            {
                //2. add new column as numeric data
                for (int row = 0; row < dtSessionData.Rows.Count; row++)
                {
                    dataValue = Convert.ToString(dtSessionData.Rows[row][mapDataColumns.DataValue]);
                    if (!Global.isNumeric(dataValue, System.Globalization.NumberStyles.Any))
                    {
                        //set numeric values for distinct textual data values (Yes, No, Don't Know)
                        distinctSessionTable = dtSessionData.DefaultView.ToTable(true, mapDataColumns.DataValue);
                        distinctSessionTable.Columns.Add(mapDataColumns.NumericData, typeof(decimal));
                        for (int i = 0; i < distinctSessionTable.Rows.Count; i++)
                        {
                            distinctSessionTable.Rows[i][mapDataColumns.NumericData] = i + 1;
                        }

                        //remove then add again
                        dtSessionData.Columns.Remove(mapDataColumns.NumericData);
                        dtSessionData = this.AddRecordFromTableWithoutLoop(dtSessionData, distinctSessionTable, mapDataColumns.DataValue, mapDataColumns.DataValue + "," + mapDataColumns.NumericData);
                        break;
                    }
                    else
                    {
                        dtSessionData.Rows[row][mapDataColumns.NumericData] = dtSessionData.Rows[row][mapDataColumns.DataValue];
                    }
                }
            }

            RetVal = dtSessionData;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw;
        }
        return RetVal;
    }

    private void CheckNCreateIsMRDColumn(DI_MapDataColumns mapDataColumns, DataTable dtSessionData)
    {
        bool isNewColumnCreated = false;
        try
        {
            isNewColumnCreated = this.CheckNCreateColumn(dtSessionData, mapDataColumns.isMRD, typeof(bool));

            if (isNewColumnCreated)
            {
                //to do get MRD

            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
    }

    /// <summary>
    /// Reduce my data to map Data structure
    /// </summary>
    /// <param name="mapDataColumns"></param>
    /// <param name="dtSessionData"></param>
    /// <returns></returns>
    private DataTable TransformTableMyDataCase(DI_MapDataColumns mapDataColumns, DataTable dtSessionData, ref bool isMrdColumnAdded)
    {
        DataTable RetVal = new DataTable();

        string numericColumnIndexes = string.Empty;
        List<string> numericColumnList = new List<string>();

        string areaColumnIndexes = string.Empty;
        List<string> areaColumnList = new List<string>();

        string isMRDColumnIndexes = string.Empty;
        List<string> isMRDColumnList = new List<string>();

        string timePeriodColumnIndexes = string.Empty;
        List<string> timePeriodColumnList = new List<string>();

        int counter = 1;
        DataRow DRRow = null;
        Dictionary<string, string> columnHistory = new Dictionary<string, string>();
        List<string> seriesCombinations = new List<string>();
        string SeriesText = string.Empty;
        isMrdColumnAdded = false;

        try
        {

            //1) getColumnCaption of Area and Numeric column and set columns index
            foreach (DataColumn column in dtSessionData.Columns)
            {
                if (!string.IsNullOrEmpty(column.Caption))
                {
                    //area columns
                    if (column.Caption == mapDataColumns.AreaID || column.Caption == mapDataColumns.AreaName)
                    {
                        if (column.Caption == mapDataColumns.AreaID && areaColumnIndexes != "")
                        {
                            areaColumnIndexes = dtSessionData.Columns.IndexOf(column) + "," + areaColumnIndexes;
                        }
                        else
                        {
                            areaColumnIndexes += dtSessionData.Columns.IndexOf(column) + ",";
                        }
                        continue;
                    }

                    ////numeric columns
                    //if (column.DataType == typeof(decimal))
                    //{
                    //    numericColumnIndexes += dtSessionData.Columns.IndexOf(column) + ",";
                    //}

                    //numeric columns
                    if (column.Caption.StartsWith("[**Data_Value**]"))
                    {
                        numericColumnIndexes += dtSessionData.Columns.IndexOf(column) + ",";
                    }

                    ////ToDo Manage it by gaurav in DES case Column caption get Time instead of TimePeriod
                    //if (column.Caption == "Time")
                    //{
                    //    column.Caption = mapDataColumns.TimePeriod;
                    //    column.ColumnName = mapDataColumns.TimePeriod;
                    //}

                    //timeperiod columns
                    if (column.Caption == mapDataColumns.TimePeriod)
                    {
                        timePeriodColumnIndexes += dtSessionData.Columns.IndexOf(column) + ",";
                    }
                    if (column.Caption == mapDataColumns.isMRD)
                    {
                        isMRDColumnIndexes += dtSessionData.Columns.IndexOf(column) + ",";
                    }
                }
            }
            areaColumnIndexes = areaColumnIndexes.Trim(',');
            numericColumnIndexes = numericColumnIndexes.Trim(',');
            timePeriodColumnIndexes = timePeriodColumnIndexes.Trim(',');
            isMRDColumnIndexes = isMRDColumnIndexes.Trim(',');

            //2. get column name by column Index 
            //2.1 for numeric columns
            foreach (string numericColumnIndex in numericColumnIndexes.Split(','))
            {
                if (!string.IsNullOrEmpty(numericColumnIndex))
                {
                    numericColumnList.Add(dtSessionData.Columns[Convert.ToInt32(numericColumnIndex)].ColumnName);
                }
            }
            //2.2. for area columns
            foreach (string areaColumnIndex in areaColumnIndexes.Split(','))
            {
                if (!string.IsNullOrEmpty(areaColumnIndex))
                {
                    areaColumnList.Add(dtSessionData.Columns[Convert.ToInt32(areaColumnIndex)].ColumnName);
                }
            }
            //2.3. for timeperiod columns
            foreach (string timeperiodColumnIndex in timePeriodColumnIndexes.Split(','))
            {
                if (!string.IsNullOrEmpty(timeperiodColumnIndex))
                {
                    timePeriodColumnList.Add(dtSessionData.Columns[Convert.ToInt32(timeperiodColumnIndex)].ColumnName);
                }
            }

            //2.4. for timeperiod columns
            foreach (string MRDColumnIndex in isMRDColumnIndexes.Split(','))
            {
                if (!string.IsNullOrEmpty(MRDColumnIndex))
                {
                    isMRDColumnList.Add(dtSessionData.Columns[Convert.ToInt32(MRDColumnIndex)].ColumnName);
                }
            }

            ////Remove extra quotes from session data table
            //dtSessionData = this.RemoveQuotefromTableContent(Timeperiods.TimePeriod, dtSessionData);

            //3. create columns
            //3.1 for area column
            RetVal.Columns.Add(mapDataColumns.AreaID, typeof(string));
            if (areaColumnIndexes.Split(',').Length > 1)
            {
                RetVal.Columns.Add(mapDataColumns.AreaName, typeof(string));
            }

            //3.2 for timeperiod Column
            RetVal.Columns.Add(mapDataColumns.TimePeriod, typeof(string));

            //3.3 for series ID for distinct combination od data
            RetVal.Columns.Add(mapDataColumns.SeriesNid, typeof(int));
            foreach (DataColumn column in dtSessionData.Columns)
            {
                if (!string.IsNullOrEmpty(column.ColumnName) && !numericColumnList.Contains(column.ColumnName) && column.Caption != mapDataColumns.AreaID && column.Caption != mapDataColumns.AreaName && column.Caption != mapDataColumns.TimePeriod && column.Caption.ToLower() != "ismrd")
                {
                    RetVal.Columns.Add("s" + counter, typeof(string));
                    RetVal.Columns["s" + counter].Caption = "[**SERIES**]s" + counter;
                    columnHistory.Add(column.ColumnName, "s" + counter);
                    counter++;
                }
            }

            //3.4 For Numeric combinations like India,IMR,2010,Male for male and itz kind of subgroup_Val
            RetVal.Columns.Add("s" + counter, typeof(string));
            RetVal.Columns["s" + counter].Caption = "[**SERIES**]s" + counter;

            //3.5 for other combination type column
            RetVal.Columns.Add(mapDataColumns.isMRD, typeof(bool));

            //3.6 for other combination type column
            RetVal.Columns.Add(mapDataColumns.DataValue, typeof(string));

            //4. add rows for discinct combination
            foreach (DataRow row in dtSessionData.Rows)
            {
                //4.1 add all numeric combinations
                foreach (string columnName in numericColumnList)
                {
                    SeriesText = string.Empty;
                    if (string.IsNullOrEmpty(row[areaColumnList[0]].ToString().Trim()) || string.IsNullOrEmpty(row[columnName].ToString().Trim()))
                    {
                        //areaID is blank then wont cosider unmapped area //check for data value also if blank then not add column
                        continue;
                    }

                    DRRow = RetVal.NewRow();

                    //4.1.1 set area values by index
                    DRRow[mapDataColumns.AreaID] = row[areaColumnList[0]].ToString().Trim();

                    if (isMRDColumnList.Count > 0)
                    {
                        DRRow[mapDataColumns.isMRD] = row[isMRDColumnList[0]];
                    }
                    else
                    {
                        if (isMrdColumnAdded == false)
                            isMrdColumnAdded = true;

                        DRRow[mapDataColumns.isMRD] = false;
                    }

                    if (areaColumnList.Count > 1)
                    {
                        DRRow[mapDataColumns.AreaName] = row[areaColumnList[1]].ToString().Trim();
                    }

                    //4.1.2 set timeperiod values by index
                    if (timePeriodColumnList.Count > 0)
                    {
                        DRRow[mapDataColumns.TimePeriod] = row[timePeriodColumnList[0]];
                    }
                    else
                    {
                        DRRow[mapDataColumns.TimePeriod] = string.Empty;
                    }

                    //4.1.3 set value for other combination type column
                    foreach (KeyValuePair<string, string> columnDic in columnHistory)
                    {
                        DRRow[columnDic.Value] = row[columnDic.Key];
                        SeriesText += row[columnDic.Key] + ",";
                    }

                    // subgroup_Val 
                    DRRow["s" + counter] = columnName;
                    DRRow[mapDataColumns.DataValue] = row[columnName];

                    SeriesText += columnName;
                    if (!seriesCombinations.Contains(SeriesText))
                    {
                        seriesCombinations.Add(SeriesText);
                    }
                    DRRow[mapDataColumns.SeriesNid] = seriesCombinations.IndexOf(SeriesText) + 1;

                    RetVal.Rows.Add(DRRow);

                    RetVal.AcceptChanges();
                }
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw;
        }
        finally
        {
            if (RetVal == null)
            { RetVal = dtSessionData; }
        }
        return RetVal;
    }

    public DataTable RemoveQuotefromTableContent(string columnName, DataTable tbl)
    {
        DataTable RetVal = null;

        try
        {
            if (tbl.Columns.Contains(columnName))
            {
                foreach (DataRow row in tbl.Rows)
                {
                    if (row[columnName] != null)
                        row[columnName] = row[columnName].ToString().Trim('\'');
                }
            }
            RetVal = tbl;
        }
        catch (Exception ex)
        {
            RetVal = tbl;
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    private List<string> SetMapLayers(Map diMap, string dbNId, string langCode)
    {
        List<string> RetVal = new List<string>();
        DIConnection DBConnection = null;
        //DIQueries DBQueries = null;

        //DIConnection MapServerConnection = null;
        //DIQueries MapServerQueries = null;

        string sSql = string.Empty;

        DateTime dtStartDate;
        DateTime dtEndDate;
        DateTime dtTempStartDate = DateTime.Parse(DEFAULT_START_DATE).Date;
        DateTime dtTempEndDate = DateTime.Parse(DEFAULT_START_DATE).Date;
        CultureInfo ociEnUS = new CultureInfo("en-US", false);
        string LayerName = string.Empty;
        string areaID = string.Empty;
        string areaName = string.Empty;
        string AreaInClause = string.Empty;
        List<DbParameter> DbParams = new List<DbParameter>();
        DataTable dtMapData = null;
        string selectedAreaIDNids = string.Empty;
        string sTimePeriods = string.Empty;

        //consider only in mydata case only
        bool isAreaSearchFromName = false;
        List<string> TimePeriods = null;

        DataView dvAreaMapLayerInfo = null;
        string ProcName = string.Empty;
        string RelationShipColumnName = string.Empty;
        List<string> AreaSearch_IDs_Nids_Names = null;

        DbParameter areaParam1;
        DbParameter areaParam2;
        Layer _Layer = null;
        string mapFolder = string.Empty;

        try
        {
            dtMapData = diMap.MapData;
            selectedAreaIDNids = diMap.SelectedAreaIDNids;
            mapFolder = diMap.SpatialMapFolder;
            sTimePeriods = this.GetCSV(dtMapData, diMap.MapDataColumns.TimePeriod);

            if (string.IsNullOrEmpty(dbNId))
            {
                dbNId = Global.GetDefaultDbNId();
            }

            ////////this.SetTimeLogIntoFile("  Starts Creating Connection-> " + DateTime.Now.ToLongTimeString());
            ////////DBConnection = Global.GetDbConnection(Convert.ToInt32(dbNId));
            ////////this.SetTimeLogIntoFile("  Ends Creating Connection-> " + DateTime.Now.ToLongTimeString());
            //////////TODO Remove usage of DAL queries and use stored procedure instead
            ////////DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), langCode);

            if (diMap.UseMapServer)
            {
                DBConnection = new DIConnection(Global.GetMapServerConnectionDetails());
                langCode = Global.GetMapServerLangCode(langCode);
            }
            else
            {
                this.SetTimeLogIntoFile("  Starts Creating Connection-> " + DateTime.Now.ToLongTimeString());
                DBConnection = Global.GetDbConnection(Convert.ToInt32(dbNId));
                this.SetTimeLogIntoFile("  Ends Creating Connection-> " + DateTime.Now.ToLongTimeString());
            }

            //-- Get the date range for which shapefiles are to be considered
            //-- handle different Reginal Settings - Thai, Arabic etc
            dtStartDate = DateTime.Parse(DEFAULT_START_DATE, ociEnUS).Date;
            dtEndDate = DateTime.Parse(DEFAULT_END_DATE, ociEnUS).Date;

            //TODO Skip time based logic in case of MyData            
            TimePeriods = new List<string>(sTimePeriods.Split(new []{","},StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < TimePeriods.Count; i++)
            {
             
                dtStartDate = DateTime.Parse(DEFAULT_START_DATE, ociEnUS).Date;
                dtEndDate = DateTime.Parse(DEFAULT_END_DATE, ociEnUS).Date;
                this.SetStartDateEndDate(TimePeriods[i], ref dtTempStartDate, ref dtTempEndDate);
                if (i == 0) //For first time set dtStartDate dtEndDate
                {
                    dtStartDate = dtTempStartDate;
                    dtEndDate = dtTempEndDate;
                }
                else
                {
                    if (System.DateTime.Compare(dtTempStartDate, dtStartDate) < 0)
                        dtStartDate = dtTempStartDate;

                    if (System.DateTime.Compare(dtTempEndDate, dtEndDate) > 0)
                        dtEndDate = dtTempEndDate;
                }
            }

            //get area from session dataview table
            AreaSearch_IDs_Nids_Names = new List<string>();
            foreach (string areaids in selectedAreaIDNids.Split(','))
            {
                if (areaids.StartsWith("QS_"))
                {
                    //AreaSearch_IDs_Nids_Names.Add(areaids.ToString().Substring(areaids.ToString().IndexOf("QS_") + 3));
                    AreaSearch_IDs_Nids_Names.Add(areaids.ToString());
                }
                else
                {
                    AreaSearch_IDs_Nids_Names.Add(areaids.ToString());
                }
            }

            if (!diMap.isMyData)
            {
                //search data by area NIDs, IDS both in QDS, Browse data case                
                DbParams = new List<DbParameter>();
                ProcName = "sp_get_areaMapLayer_" + langCode;
                areaParam1 = DBConnection.CreateDBParameter();
                areaParam1.ParameterName = "strAllAreaQSIds_Nids";
                areaParam1.DbType = DbType.String;
                areaParam1.Value = this.AddQuotesInCommaSeperated(AreaSearch_IDs_Nids_Names, false);

                DbParams = new List<DbParameter>();
                DbParams.Add(areaParam1);
            }
            else
            {

                //TODO handle My data case to get area table information layes search by AreaIDs or name
                //TODO get filter text by ID and Name both 
                //TO DO search data by area Name OR ID for MYData Case
                if (isAreaSearchFromName)
                {
                    //search data by area Name                    
                    DbParams = new List<DbParameter>();
                    ProcName = "sp_GetAreaMapLayer_" + langCode;

                    areaParam1 = DBConnection.CreateDBParameter();
                    areaParam1.ParameterName = "FilterFieldType";
                    areaParam1.DbType = DbType.String;
                    areaParam1.Value = diMap.MapDataColumns.AreaName;
                    DbParams.Add(areaParam1);

                    areaParam2 = DBConnection.CreateDBParameter();
                    areaParam2.ParameterName = "FilterText";
                    areaParam2.DbType = DbType.String;
                    areaParam2.Value = this.AddQuotesInCommaSeperated(AreaSearch_IDs_Nids_Names, false);
                    DbParams.Add(areaParam2);

                }
                else
                {
                    //search data by area id
                    DbParams = new List<DbParameter>();
                    ProcName = "sp_GetAreaMapLayer_" + langCode;

                    areaParam1 = DBConnection.CreateDBParameter();
                    areaParam1.ParameterName = "FilterFieldType";
                    areaParam1.DbType = DbType.String;
                    areaParam1.Value = diMap.MapDataColumns.AreaID;
                    DbParams.Add(areaParam1);

                    areaParam2 = DBConnection.CreateDBParameter();
                    areaParam2.ParameterName = "FilterText";
                    areaParam2.DbType = DbType.String;
                    areaParam2.Value = this.AddQuotesInCommaSeperated(AreaSearch_IDs_Nids_Names, false);
                    DbParams.Add(areaParam2);
                }
            }

            ////////////if (diMap.UseMapServer)
            ////////////{
            ////////////    MapServerConnection = new DIConnection(Global.GetMapServerConnectionDetails());
            ////////////    //MapServerQueries = this.GetMapServerQueries(DBConnection.DIDataSetDefault(), langCode, MapServerConnection);

            ////////////    this.SetTimeLogIntoFile("  Starts getting records from Map Server database-> " + DateTime.Now.ToLongTimeString());
            ////////////    dvAreaMapLayerInfo = MapServerConnection.ExecuteDataTable(ProcName, CommandType.StoredProcedure, DbParams).DefaultView;
            ////////////    this.SetTimeLogIntoFile("  Ends getting records from Map Server database-> " + DateTime.Now.ToLongTimeString());
            ////////////}
            ////////////else
            ////////////{
            ////////////    this.SetTimeLogIntoFile("  Starts getting records from database-> " + DateTime.Now.ToLongTimeString());
            ////////////    dvAreaMapLayerInfo = DBConnection.ExecuteDataTable(ProcName, CommandType.StoredProcedure, DbParams).DefaultView;
            ////////////    this.SetTimeLogIntoFile("  Ends getting records from database-> " + DateTime.Now.ToLongTimeString());
            ////////////}


            this.SetTimeLogIntoFile("  Starts getting records from Map Server database-> " + DateTime.Now.ToLongTimeString());
            dvAreaMapLayerInfo = DBConnection.ExecuteDataTable(ProcName, CommandType.StoredProcedure, DbParams).DefaultView;
            this.SetTimeLogIntoFile("  Ends getting records from Map Server database-> " + DateTime.Now.ToLongTimeString());


            if (dtMapData.Columns.Contains(diMap.MapDataColumns.AreaID))
            {
                RelationShipColumnName = diMap.MapDataColumns.AreaID;
            }
            else if (dtMapData.Columns.Contains(diMap.MapDataColumns.AreaName))
            {
                RelationShipColumnName = diMap.MapDataColumns.AreaName;
            }
            else
            {
                RelationShipColumnName = diMap.MapDataColumns.AreaNId;
            }

            try
            {
                //Add AreaID and AreaName in SessionData table
                ////TO DO handling of blank data value this.RemoveBlankValueFromColumn(ref dtMapData, diMap.MapDataColumns.DataValue);
                diMap.MapData = this.AddRecordFromTableWithoutLoop(dtMapData, dvAreaMapLayerInfo.Table, RelationShipColumnName, diMap.MapDataColumns.AreaNId + "," + diMap.MapDataColumns.AreaID + "," + diMap.MapDataColumns.AreaName);
            }
            catch (Exception ex)
            {
                //check and delete for unmatched area ID in parent and child table
                if (ex.Message == "This constraint cannot be enabled as not all values have corresponding parent values.")
                {
                    dtMapData = this.RemoveUnmatchedRecordInTargetTable(dtMapData, dvAreaMapLayerInfo.Table, RelationShipColumnName);
                    diMap.MapData = this.AddRecordFromTableWithoutLoop(dtMapData, dvAreaMapLayerInfo.Table, RelationShipColumnName, diMap.MapDataColumns.AreaNId + "," + diMap.MapDataColumns.AreaID + "," + diMap.MapDataColumns.AreaName);
                }
                Global.CreateExceptionString(ex, null);

            }

            dvAreaMapLayerInfo.Sort = Area.AreaID + "," + Area_Map_Layer.EndDate;

            //get distinct layers by max end date
            dvAreaMapLayerInfo = this.GetDistinctLayersByMaxEndDate(dvAreaMapLayerInfo.Table, diMap.MapData).DefaultView;

            this.SetTimeLogIntoFile("  Starts adding layers-> " + DateTime.Now.ToLongTimeString());
            //-- Loop each layer associated with this AreaNid, & add that layer in collection
            foreach (DataRow drAreaMap in dvAreaMapLayerInfo.Table.Rows)
            {
                try
                {
                    //Check if same Layer ID already exists in Layers Collection, if yes then skip.. 
                    if (diMap.Layers[drAreaMap[Area_Map_Metadata.LayerName].ToString()] == null)  //"Layer_Name"
                    {
                        //TODO Skip time based logic in case of MyData
                        //TODO Remove refernce of Microsoft.VisualBasic.Information.IsDBNull
                        //*** BugFix 01 Feb 2006 Problem with different Reginal Settings - Thai, Arabic etc
                        dtTempStartDate = (System.DateTime)(Information.IsDBNull(drAreaMap[Area_Map_Layer.StartDate]) ? DateTime.Parse(DEFAULT_START_DATE, ociEnUS) : DateTime.Parse(((System.DateTime)drAreaMap[Area_Map_Layer.StartDate]).Month + "/" + ((System.DateTime)drAreaMap[Area_Map_Layer.StartDate]).Day + "/" + ((System.DateTime)drAreaMap[Area_Map_Layer.StartDate]).Year, ociEnUS));
                        dtTempEndDate = (System.DateTime)(Information.IsDBNull(drAreaMap[Area_Map_Layer.EndDate]) ? DateTime.Parse(DEFAULT_END_DATE, ociEnUS) : DateTime.Parse(((System.DateTime)drAreaMap[Area_Map_Layer.EndDate]).Month + "/" + ((System.DateTime)drAreaMap[Area_Map_Layer.EndDate]).Day + "/" + ((System.DateTime)drAreaMap[Area_Map_Layer.EndDate]).Year, ociEnUS));

                        //Get only those map files whose start date and end date are between Selected TimePeriods / Presentation data time periods
                        if (System.DateTime.Compare(dtTempEndDate, dtStartDate) < 0 | System.DateTime.Compare(dtTempStartDate, dtEndDate) > 0)
                        {
                            //--- Do nothing
                            Console.Write("");
                        }
                        else
                        {
                            //Add Layer to Layers collection
                            _Layer = diMap.Layers.AddSpatialLayer(mapFolder, drAreaMap[Area_Map_Metadata.LayerName].ToString());

                            if ((_Layer != null)) //if error while adding layer
                            {
                                //Set Layer properties
                                //_Layer.ID = drAreaMap[Area.AreaID].ToString();
                                _Layer.LayerName = drAreaMap[Area_Map_Metadata.LayerName].ToString();
                                _Layer.Area_Level = int.Parse(drAreaMap[Area.AreaLevel].ToString());
                                _Layer.LayerType = (ShapeType)(int)drAreaMap[Area_Map_Layer.LayerType];
                                _Layer.StartDate = dtTempStartDate;
                                _Layer.EndDate = dtTempEndDate;
                                _Layer.AreaNames.Add(drAreaMap[diMap.MapDataColumns.AreaID].ToString(), drAreaMap[diMap.MapDataColumns.AreaName].ToString());
                                //_Layer.LabelMultirow = true;
                                _Layer.LabelEffectSettings = new LabelEffectSetting();

                                //Set property that point layer exists
                                if (_Layer.LayerType == ShapeType.Point)
                                {
                                    diMap.PointShapeIncluded = true;
                                }

                                //Set visibility off for feature layers by default
                                switch (_Layer.LayerType)
                                {
                                    case ShapeType.PointFeature:
                                    case ShapeType.PolygonFeature:
                                    case ShapeType.PolyLineFeature:
                                        _Layer.Visible = false;
                                        break;
                                }


                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Global.CreateExceptionString(ex, null);


                }
            }
            this.SetTimeLogIntoFile("  Ends adding layers-> " + DateTime.Now.ToLongTimeString());
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw;
        }

        return RetVal;
    }

    /// <summary>
    /// Get distinct layers by maximum end date like if 2 layers exists for UP then it will pick only one which has max end date
    /// some area name displayed twice at map.(Peter Leth)
    /// </summary>
    /// <param name="dvAreaMapLayerInfo"></param>
    /// <returns></returns>
    private DataTable GetDistinctLayersByMaxEndDate(DataTable dvAreaMapLayerInfo, DataTable mapData)
    {
        DataTable RetVal = null;
        DataTable TempTable = null;
        DataRow[] tempRows = null;

        List<DateTime> EndDatetimeList = new List<DateTime>();

        DateTime MaxDatetime = new DateTime();
        string LayerName = string.Empty;
        DataRow[] tempRowsByArea = null;
        int TotalLength = 0;
        Dictionary<DateTime, string> LayerList = new Dictionary<DateTime, string>();
        DataTable DistinctTimePeriodAndArea;

        DateTime dtStartDate = new DateTime();
        DateTime dtEndDate = new DateTime();
        DateTime dtTempStartDate = DateTime.Parse(DEFAULT_START_DATE).Date;
        DateTime dtTempEndDate = DateTime.Parse(DEFAULT_START_DATE).Date;
        DataRow[] TempDatetimeRows;

        Dictionary<string, DateTime> LayerWithDate = new Dictionary<string, DateTime>();
        List<DateTime> LayerWithDateList = new List<DateTime>();
        List<string> selectedTimePeriod = new List<string>();

        try
        {

            selectedTimePeriod = DevInfo.Lib.DI_LibBAL.Utility.DICommon.GetCommaSeperatedListOfGivenColumn(mapData, Timeperiods.TimePeriod, false, string.Empty);


            DistinctTimePeriodAndArea = this.GetDistinctTimePeriodsAndArea(mapData);

            TempTable = dvAreaMapLayerInfo.DefaultView.ToTable(true, Area_Map_Metadata.LayerName, Area.AreaID, Area_Map_Layer.EndDate);

            foreach (DataRow AreaRow in TempTable.DefaultView.ToTable(true, Area.AreaID).Rows)
            {
                tempRows = null;
                tempRows = dvAreaMapLayerInfo.Select(Area.AreaID + "='" + AreaRow[Area.AreaID] + "'");
                DistinctTimePeriodAndArea.DefaultView.RowFilter = Area.AreaID + "='" + AreaRow[Area.AreaID] + "'";

                //if more than one row exist then get layer of max end date otherwise delete that record
                if (tempRows.Length > 0)
                {
                    EndDatetimeList.Clear();
                    LayerList.Clear();
                    LayerWithDate.Clear();
                    LayerWithDateList.Clear();

                    //1. get max end date                    
                    foreach (DataRow row in tempRows)
                    {
                        TempDatetimeRows = DistinctTimePeriodAndArea.DefaultView.ToTable().Select();
                        LayerName = Convert.ToString(row[Area_Map_Metadata.LayerName]);

                        dtStartDate = Convert.ToDateTime(row[Area_Map_Layer.StartDate]);
                        dtEndDate = Convert.ToDateTime(row[Area_Map_Layer.EndDate]);

                        if (!EndDatetimeList.Contains(dtEndDate))
                            EndDatetimeList.Add(dtEndDate);

                        if (!LayerList.ContainsKey(dtEndDate))
                        {
                            DataRow[] layerRow = dvAreaMapLayerInfo.Select(Area.AreaID + "='" + AreaRow[Area.AreaID] + "' and " + Area_Map_Layer.EndDate + "='" + dtEndDate + "'");
                            if (layerRow.Length > 0)
                            {
                                LayerName = Convert.ToString(layerRow[0][Area_Map_Metadata.LayerName]);
                            }

                            LayerList.Add(dtEndDate, LayerName);
                        }
                    }

                    EndDatetimeList.Sort();
                    selectedTimePeriod.Sort();

                    //get layer by timeperiod in multiple layer case
                    if (selectedTimePeriod.Count > 0)
                    {
                        TimePeriodFacade.SetStartDateEndDate(selectedTimePeriod[selectedTimePeriod.Count - 1], ref dtTempStartDate, ref dtTempEndDate);
                        for (int i = 0; i < EndDatetimeList.Count; i++)
                        {
                            if (dtTempEndDate.CompareTo(EndDatetimeList[i]) <= 0)
                            {
                                MaxDatetime = EndDatetimeList[i];
                                break;
                            }
                        }
                    }

                    if (!LayerList.ContainsKey(MaxDatetime))
                        MaxDatetime = EndDatetimeList[EndDatetimeList.Count - 1];

                    LayerName = LayerList[MaxDatetime];

                    //Correct the Layername
                    LayerName = LayerName.Replace("'", "''");
                    //2. filter administrative area (ignore feature layers) and max timeperiod from table 
                    tempRowsByArea = dvAreaMapLayerInfo.Select(Area.AreaID + "='" + AreaRow[Area.AreaID] + "' and " + Area_Map_Metadata.LayerName + "<>'" + LayerName + "' and " + Area_Map_Layer.LayerType + " IN (" + (int)Enum.Parse(typeof(ShapeType), ShapeType.Point.ToString()) + "," + (int)Enum.Parse(typeof(ShapeType), ShapeType.PolyLine.ToString()) + "," + (int)Enum.Parse(typeof(ShapeType), ShapeType.Polygon.ToString()) + ")");
                    TotalLength = tempRowsByArea.Length;

                    for (int i = 0; i < TotalLength; i++)
                    {
                        tempRowsByArea[i].Delete();
                    }

                    dvAreaMapLayerInfo.AcceptChanges();

                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            RetVal = dvAreaMapLayerInfo;
        }

        //RetVal = dvAreaMapLayerInfo;

        return RetVal;
    }

    /// <summary>
    /// Get distinct timeperiods and area from presentation data
    /// </summary>
    /// <returns></returns>
    public DataTable GetDistinctTimePeriodsAndArea(DataTable dvAreaMapLayerInfo)
    {
        DataTable RetVal = null;

        string[] _DistinctColumn = new string[2];
        _DistinctColumn[0] = Timeperiods.TimePeriod;
        _DistinctColumn[1] = Area.AreaID;

        RetVal = dvAreaMapLayerInfo.DefaultView.ToTable(true, _DistinctColumn);

        return RetVal;

        //return m_QueryBase.Time_GetTimePeriods();
    }

    private void RemoveBlankValueFromColumn(ref DataTable dtMapData, string ColumnName)
    {
        if (dtMapData.Columns.Contains(ColumnName))
        {
            for (int i = 0; i < dtMapData.Rows.Count; i++)
            {
                if (string.IsNullOrEmpty(Convert.ToString(dtMapData.Rows[i][ColumnName])))
                {
                    dtMapData.Rows.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    private DataTable RemoveUnmatchedRecordInTargetTable(DataTable TargetTable, DataTable SourceTable, string ColumnName)
    {
        DataTable RetVal = null;
        List<string> targetTableAreanIDs = null;
        List<string> sourceTableAreanIDs = null;
        DataRow[] rows = null;
        int TotalRowsToDelete = 0;
        try
        {
            //get distinct areaID list from TargetTable
            targetTableAreanIDs = this.GetCSVInList(TargetTable, ColumnName, false);

            //get distinct areaID list from SourceTable
            sourceTableAreanIDs = this.GetCSVInList(SourceTable, ColumnName, false);

            for (int i = 0; i < targetTableAreanIDs.Count; i++)
            {
                if (!sourceTableAreanIDs.Contains(targetTableAreanIDs[i]))
                {
                    //remove record from target table
                    rows = TargetTable.Select(ColumnName + "='" + targetTableAreanIDs[i] + "'");
                    TotalRowsToDelete = rows.Length;
                    for (int counter = 0; counter < TotalRowsToDelete; counter++)
                    {
                        TargetTable.Rows.Remove(rows[counter]);
                    }
                }
            }
            TargetTable.AcceptChanges();
            RetVal = TargetTable;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
        return RetVal;
    }

    private static DataSet GetInvariantDataSet()
    {
        // Set locale of dataset to invariant culture to handle cases for different regional settings
        DataSet RetVal = new DataSet();
        RetVal.Locale = new System.Globalization.CultureInfo("", false);
        return RetVal;
    }

    private DataTable AddRecordFromTableWithoutLoop(DataTable TargetTable, DataTable SourceTable, string RelationshipColumnsName, string ColumnNamesToCopyData)
    {
        DataTable RetVal = null;
        DataSet _Base = new DataSet();

        try
        {
            DataTable dtSourceTableCopy = SourceTable.DefaultView.ToTable(true, ColumnNamesToCopyData.Split(',')).Copy();
            dtSourceTableCopy.TableName = "dtSourceTableCopy";
            _Base.Tables.Add(dtSourceTableCopy);


            DataTable dtTargetTableCopy = TargetTable.Copy();
            dtTargetTableCopy.TableName = "dtTargetTableCopy";
            _Base.Tables.Add(dtTargetTableCopy);

            _Base.Relations.Add("RelationshipName", _Base.Tables["dtSourceTableCopy"].Columns[RelationshipColumnsName], _Base.Tables["dtTargetTableCopy"].Columns[RelationshipColumnsName], true);

            foreach (string columnName in ColumnNamesToCopyData.Split(','))
            {
                if (!dtTargetTableCopy.Columns.Contains(columnName))
                {
                    //add data into CHILD table if column not exists
                    if (columnName.ToLower().EndsWith("nid"))
                    {
                        dtTargetTableCopy.Columns.Add(columnName, typeof(int));
                    }
                    else
                    {
                        dtTargetTableCopy.Columns.Add(columnName, SourceTable.Columns[columnName].DataType);
                    }
                    dtTargetTableCopy.Columns[columnName].Expression = "parent(RelationshipName)." + columnName;
                }
            }

            //remove relationship
            dtTargetTableCopy.ParentRelations.Clear();
            dtTargetTableCopy.ChildRelations.Clear();
            dtTargetTableCopy.Constraints.Clear();
            dtTargetTableCopy.AcceptChanges();

            dtSourceTableCopy.ParentRelations.Clear();
            dtSourceTableCopy.ChildRelations.Clear();
            dtSourceTableCopy.Constraints.Clear();

            dtTargetTableCopy.AcceptChanges();

            _Base.Relations.Clear();
            _Base.AcceptChanges();

            //copy data
            RetVal = dtTargetTableCopy.DefaultView.ToTable();
            RetVal.TableName = TargetTable.TableName;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw;
        }
        finally
        {
            _Base.Dispose();
        }

        return RetVal;
    }

    private bool CheckNCreateColumn(DataTable tableData, string ColumnName, Type ColumnType)
    {
        bool RetVal = false;
        if (!tableData.Columns.Contains(ColumnName))
        {
            tableData.Columns.Add(ColumnName, ColumnType);
            RetVal = true;
        }
        return RetVal;
    }

    private void SetStartDateEndDate(string timePeriod, ref System.DateTime startDate, ref System.DateTime endDate)
    {
        string[] sDate;
        //-- Handle Problem with different Reginal Settings - Thai, Arabic etc
        System.Globalization.CultureInfo ociEnUS = new System.Globalization.CultureInfo("en-US", false);
        try
        {
            switch (timePeriod.Length)
            {
                case 4:
                    //yyyy
                    startDate = DateTime.Parse("1/1/" + timePeriod, ociEnUS);
                    endDate = DateTime.Parse("12/31/" + timePeriod, ociEnUS);
                    break;
                case 7:
                    //yyyy.mm
                    sDate = timePeriod.Split('.');
                    startDate = DateTime.Parse(sDate[1] + "/1/" + sDate[0], ociEnUS);
                    endDate = DateTime.Parse(sDate[1] + "/" + DateTime.DaysInMonth(int.Parse(sDate[0]), int.Parse(sDate[1])) + "/" + sDate[0], ociEnUS);
                    break;
                case 9:
                    //yyyy-yyyy
                    if (timePeriod.IndexOf(".") == -1)
                    {
                        sDate = timePeriod.Split('-');
                        startDate = DateTime.Parse("1/1/" + sDate[0], ociEnUS);
                        endDate = DateTime.Parse("12/31/" + sDate[1], ociEnUS);
                    }
                    else
                    {
                        sDate = timePeriod.Split('.');
                        startDate = DateTime.Parse("1/1/" + sDate[0], ociEnUS);
                        endDate = DateTime.Parse("12/31/" + sDate[1], ociEnUS);
                    }

                    break;
                case 10:
                    //yyyy.mm.dd
                    sDate = timePeriod.Split('.');
                    startDate = DateTime.Parse(sDate[1] + "/" + sDate[2] + "/" + sDate[0], ociEnUS);
                    endDate = DateTime.Parse(sDate[1] + "/" + sDate[2] + "/" + sDate[0], ociEnUS);
                    break;
                case 15:
                    //yyyy.mm-yyyy.mm
                    string[] sTempDate;
                    sTempDate = timePeriod.Split('-');
                    sDate = sTempDate[0].Split('.');
                    startDate = DateTime.Parse(sDate[1] + "/1/" + sDate[0], ociEnUS);
                    sDate = sTempDate[1].Split('.');
                    endDate = DateTime.Parse(sDate[1] + "/" + DateTime.DaysInMonth(int.Parse(sDate[0]), int.Parse(sDate[1])) + "/" + sDate[0], ociEnUS);
                    break;
                case 21:
                    //yyyy.mm.dd-yyyy.mm.dd
                    string[] sTempDate2;
                    sTempDate2 = timePeriod.Split('-');
                    sDate = sTempDate2[0].Split('.');
                    startDate = DateTime.Parse(sDate[1] + "/" + sDate[2] + "/" + sDate[0], ociEnUS);
                    sDate = sTempDate2[1].Split('.');
                    endDate = DateTime.Parse(sDate[1] + "/" + sDate[2] + "/" + sDate[0], ociEnUS);
                    break;
            }
        }
        catch (Exception ex)
        {
            //startDate = DateTime.Parse(DEFAULT_START_DATE, ociEnUS);
            //endDate = DateTime.Parse(DEFAULT_END_DATE, ociEnUS);
            Global.CreateExceptionString(ex, null);

        }
    }

    private string GetCSV(DataTable dataTable, string columnName)
    {
        string RetVal = string.Empty;

        try
        {
            //TODO append only distinct elements
            if (dataTable.Columns.Contains(columnName))
            {
                foreach (DataRow dvRow in dataTable.DefaultView.ToTable(true, columnName).Rows)
                {
                    RetVal += "" + Convert.ToString(dvRow[columnName]) + ",";
                }
                RetVal = RetVal.Trim(',');
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw;
        }
        return RetVal;
    }

    private string GetCSVByCaption(DataTable dataTable, string columnCaption)
    {
        string RetVal = string.Empty;
        string ColumnName = string.Empty;

        try
        {
            foreach (DataColumn column in dataTable.Columns)
            {
                if (column.Caption == columnCaption)
                {
                    ColumnName = column.ColumnName;
                    break;
                }
            }

            //TODO append only distinct elements
            if (!string.IsNullOrEmpty(ColumnName))
            {
                foreach (DataRow dvRow in dataTable.DefaultView.ToTable(true, ColumnName).Rows)
                {
                    RetVal += "" + Convert.ToString(dvRow[ColumnName]) + ",";
                }
                RetVal = RetVal.Trim(',');
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw;
        }
        return RetVal;
    }

    private string AddQuotesInCommaSeperated(string IDs)
    {
        string RetVal = string.Empty;

        foreach (string id in IDs.Split(','))
        {
            if (string.IsNullOrEmpty(id))
            {
                continue;
            }

            RetVal += "'" + id + "',";
        }

        return RetVal.Trim(',');
    }

    private string AddQuotesInCommaSeperated(List<string> IDs, bool isAddQuote)
    {
        string RetVal = string.Empty;

        foreach (string id in IDs)
        {
            if (string.IsNullOrEmpty(id))
            {
                continue;
            }
            if (isAddQuote)
            {
                RetVal += "'" + id + "',";
            }
            else
            {
                RetVal += "" + id + ",";
            }
        }

        return RetVal.Trim(',');
    }

    private string AddQuotesInCommaSeperated(string IDs, bool isAddQuote)
    {
        string RetVal = string.Empty;

        foreach (string id in IDs.Split(','))
        {
            if (string.IsNullOrEmpty(id))
            {
                continue;
            }
            if (isAddQuote)
            {
                RetVal += "'" + id + "',";
            }
            else
            {
                RetVal += "" + id + ",";
            }
        }

        return RetVal.Trim(',');
    }

    private FontStyle GetFontStyle(bool isBold, bool isItalic, bool isUnderlined)
    {
        FontStyle RetVal = FontStyle.Regular;

        //to do set font style
        if (isBold)
        {
            RetVal = FontStyle.Bold;
        }

        if (isItalic)
        {
            if (RetVal == FontStyle.Bold)
            {
                RetVal = RetVal | FontStyle.Italic;
            }
            else
            {
                RetVal = FontStyle.Italic;
            }

        }

        if (isUnderlined)
        {
            if (RetVal == FontStyle.Bold || RetVal == FontStyle.Italic || (RetVal == (FontStyle.Italic | FontStyle.Bold)))
            {
                RetVal = RetVal | FontStyle.Underline;
            }
            else
            {
                RetVal = FontStyle.Underline;
            }
        }
        return RetVal;
    }

    private string GetBase64Image(string filename)
    {

        FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
        byte[] filebytes = new byte[fs.Length];
        fs.Read(filebytes, 0, Convert.ToInt32(fs.Length));
        return "data:image/png;base64," +
            Convert.ToBase64String(filebytes, Base64FormattingOptions.None);
    }

    private string GetBase64MapImageString(MemoryStream mapStream)
    {
        string RetVal = string.Empty;
        string FilenameWPath = string.Empty;
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;

        try
        {
            FilenameWPath = Path.Combine(this.TempPath, System.DateTime.Now.Ticks.ToString() + ".png");

            if (!string.IsNullOrEmpty(this.BrowserType) && this.BrowserType == "ie8")
            {
                this.SaveMemoryStreamIntoFile(mapStream, FilenameWPath);
                RetVal = Path.Combine(AbsoluteTempFile.Substring(0, AbsoluteTempFile.LastIndexOf("libraries")), FilenameWPath.Substring(FilenameWPath.LastIndexOf("stock"))).Replace("\\", "/");
            }
            else
            {
                RetVal = "data:image/png;base64," + Convert.ToBase64String(mapStream.ToArray(), Base64FormattingOptions.None);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            if (mapStream != null)
            {
                mapStream.Dispose();
            }
        }

        return RetVal;
    }

    /// <summary>
    /// Save files from memory stream
    /// </summary>
    /// <param name="ms"></param>
    /// <param name="FileName"></param>
    public void SaveMemoryStreamIntoFile(MemoryStream ms, string FileName)
    {
        FileStream outStream = null;
        try
        {
            outStream = File.OpenWrite(FileName);
            ms.WriteTo(outStream);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From xml : ->" + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            if (outStream != null)
            {
                outStream.Flush();
                outStream.Close();
            }
        }
    }

    /// <summary>
    /// Save Content in file
    /// </summary>
    /// <param name="Content"></param>
    /// <param name="FileNameWPath"></param>
    public void SaveContentIntoFile(string Content, string FileNameWPath)
    {
        StreamWriter writer = null;

        try
        {
            writer = new StreamWriter(Path.Combine(TempPath, "serverResponseTime.txt"), true);
            writer.WriteLine(Content);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From xml : ->" + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            if (writer != null)
            {
                writer.Flush();
                writer.Close();
            }
        }
    }

    private LanguageStrings GetMapLanguageStrings(string LanguageFolderPath, string LangCode)
    {
        LanguageStrings RetVal = new LanguageStrings();
        string MapSettingFile = string.Empty;
        string xmlNodePath = string.Empty;
        XmlFileReader objxmlFilereader = null;

        try
        {
            MapSettingFile = Path.Combine(LanguageFolderPath, LangCode) + "\\" + Constants.Map.MapLanguageFileName;
            objxmlFilereader = new XmlFileReader(MapSettingFile);

            if (objxmlFilereader != null)
            {
                RetVal.DisclaimerText = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, "langMapDisclaimerText"), Constants.Map.XMLValueAtributeName);
                RetVal.MissingValue = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, "langMissingValue"), Constants.Map.XMLValueAtributeName);
                RetVal.Map = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, "langMap"), Constants.Map.XMLValueAtributeName);
                RetVal.Data = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, "langData"), Constants.Map.XMLValueAtributeName);
                RetVal.Source = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, "langSource"), Constants.Map.XMLValueAtributeName);
                RetVal.MostRecentData = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, "langMostRecentData"), Constants.Map.XMLValueAtributeName);
                RetVal.Theme = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, "langTheme"), Constants.Map.XMLValueAtributeName);
            }

            if (string.IsNullOrEmpty(RetVal.MostRecentData))
                RetVal.MostRecentData = new LanguageStrings().MostRecentData;

            if (string.IsNullOrEmpty(RetVal.Theme))
                RetVal.Theme = new LanguageStrings().Theme;
        }
        catch (Exception ex)
        {
            RetVal = null;
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    /// <summary>
    /// Delete all temp files which is more than 3 days. 
    /// </summary>
    private void DeleteAllFilesFromTemp()
    {
        try
        {
            (from f in new DirectoryInfo(TempPath).GetFiles()
             where f.CreationTime < DateTime.Now.Subtract(TimeSpan.FromDays(2))
             select f
            ).ToList()
            .ForEach(f => f.Delete());
        }
        catch (Exception ex)
        {

        }
    }

    #endregion

    #endregion

    #endregion

    #region "--Public--"

    #region "--Methods--"

    #region "Tagging information"

    /// <summary>
    /// Get in JSON format {"area":["ASIKAZ[****]Kazakhstan[****]239.2347,57.97687,300.2017,71.22687","ASIJPN[****]Japan[****]704.2527,125.7532,737.7405,139.0032"]}
    /// </summary>
    /// <returns></returns>
    public string GetTaggingInformation()
    {
        string RetVal = string.Empty;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".xml");
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        string MapFolder = string.Empty;
        Map diMap = null;
        StringBuilder ObjStringBuilder = new StringBuilder();

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //---------------------DEMO            
            //{
            //  "area":[
            //       "ASIKAZ[****]Kazakhstan[****]239.2347,57.97687,300.2017,71.22687",
            //       "ASIJPN[****]Japan[****]704.2527,125.7532,737.7405,139.0032",
            //       "ASIIRQ[****]Iraq[****]108.1244,154.5031,130.607,167.7531",
            //       "ASIIND[****]India[****]333.9663,216.0874,361.3448,229.3374"
            //     ]
            //}

            ObjStringBuilder.Append("{");
            ObjStringBuilder.AppendLine(" \"area\":[");

            if (diMap.ImageMap != null)
            {
                for (int i = 0; i < diMap.ImageMap.Count; i++)
                {
                    //"ASIKAZ[****]Kazakhstan[****]239.2347,57.97687,300.2017,71.22687"
                    ObjStringBuilder.AppendLine("\"" + diMap.ImageMap[i].TagID + Constants.Delimiters.ParamDelimiter + diMap.ImageMap[i].TagDisplayText + Constants.Delimiters.ParamDelimiter + diMap.ImageMap[i].Coordinates + "\"");

                    if (i != diMap.ImageMap.Count - 1)
                    {
                        ObjStringBuilder.Append(",");
                    }
                }
            }

            ObjStringBuilder.AppendLine("]");
            ObjStringBuilder.AppendLine("}");

            //var jss = new JavaScriptSerializer();
            //dynamic JsonData = jss.Deserialize<dynamic>(ObjStringBuilder.ToString());
            //JsonData["area"][0]
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetTaggingInformation ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        RetVal = ObjStringBuilder.ToString();

        return RetVal;
    }

    /// <summary>
    /// AreaID, Areaname, SelectedSeriesName/ IUS, DisplayInfo/data value, timeperiod, [TODO source] on the basis on area ID
    /// </summary>
    /// <param name="areaID"></param>
    /// <param name="selectedThemeID">if hatch and color theme is not exist then consider selected theme ID</param>
    /// <returns></returns>
    public string GetAreaInfo(string areaID)
    {
        string RetVal = string.Empty;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".xml");
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        string MapFolder = string.Empty;
        Map diMap = null;
        Theme theme = null;
        string SelectedSeriesName = string.Empty;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            theme = diMap.Themes.GetActiveTheme();
            if (theme == null)
            {
                foreach (Theme thm in diMap.Themes)
                {
                    if (thm.Type != ThemeType.Hatch && thm.Type != ThemeType.Color)
                    {
                        theme = thm;
                        break;
                    }
                }
            }

            if (theme.Type != ThemeType.Chart && theme.SelectedSeriesID.Split(',').Length == 1)
            {
                SelectedSeriesName = diMap.SeriesData[theme.SelectedSeriesID].SeriesName;
            }
            else
            {
                //TODo 
                SelectedSeriesName = string.Empty;
            }

            //Format
            //Area Name (Area ID) [****]
            //Data Value (Time Period)[****]
            //Series Caption[****]
            //Source
            AreaInfo areaInfo = new AreaInfo();
            if (theme.AreaIndexes[areaID] != null)
            {
                areaInfo = (AreaInfo)theme.AreaIndexes[areaID];
                RetVal += areaInfo.AreaName + " (" + areaID + ") " + Constants.Delimiters.ParamDelimiter;
                if (theme.Type == ThemeType.Hatch || theme.Type == ThemeType.Color)
                {
                    RetVal += areaInfo.DisplayInfo + Constants.Delimiters.ParamDelimiter;
                    RetVal += SelectedSeriesName + Constants.Delimiters.ParamDelimiter;
                    RetVal += areaInfo.Source;
                }
                else
                {
                    RetVal += "" + Constants.Delimiters.ParamDelimiter;
                    RetVal += "" + Constants.Delimiters.ParamDelimiter;
                    RetVal += "";
                }
            }
            else if (diMap.Layers[areaID] != null)
            {
                Hashtable AreaNames = diMap.Layers[areaID].AreaNames;
                if (AreaNames.ContainsKey(areaID))
                {
                    RetVal += (string)AreaNames[areaID] + " (" + areaID + ") " + Constants.Delimiters.ParamDelimiter;

                    RetVal += "N/A" + Constants.Delimiters.ParamDelimiter;
                    RetVal += SelectedSeriesName + Constants.Delimiters.ParamDelimiter;
                    RetVal += "";
                }
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetAreaInfo ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #region "Theme Info"

    public string ResetAllTheme(string SelectedIUNids, string SelectedAreaNids)
    {
        string RetVal = string.Empty;
        bool isMyData = false;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        DataTable dtSessionData = null;
        DataTable SeriesDataTable = null;
        SeriesData seriesData = null;
        string seriesID = string.Empty;
        string seriesName = string.Empty;
        StringBuilder FilterText = new StringBuilder();
        string ThemeName = string.Empty;
        Theme OldTheme;
        List<string> seriesIDsList = new List<string>();
        Theme theme = null;
        int countTheme = 0;
        List<string> themeNames = new List<string>();
        Dictionary<int, string> themeNamesDisc = new Dictionary<int, string>();
        string ThemeNumber = string.Empty;

        string ThemeID, SeriesIDs = string.Empty;
        ThemeType themeType;
        BreakType breakType;
        int breakCount = 0;
        ChartType chartType;
        List<string> AreaNIdList = new List<string>();
        List<string> IndicatorNidList = new List<string>();
        List<string> areaNameSelectedRecord = new List<string>();
        List<string> areaIDSelectedRecord = new List<string>();
        bool isLayerVisible = false;
        bool isSetSelectedSeriesID = false;
        List<string> VisibleSeriesList = null;
        string SelectedIndicatorNids = string.Empty;
        List<string> removedList = new List<string>();
        List<string> themeSelectedSeries = null;
        int i = 0;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            dtSessionData = diMap.MapData;
            diMap.VisibleAreaIDNids = SelectedAreaNids;

            if (!string.IsNullOrEmpty(SelectedIUNids))
            {
                SelectedIndicatorNids = string.Join(",", this.GetCSVInList(dtSessionData, diMap.MapDataColumns.SeriesNid, false, "IUNID in(" + this.AddQuotesInCommaSeperated(SelectedIUNids) + ")"));
                diMap.VisibleIndicatorNids = SelectedIndicatorNids;

                VisibleSeriesList = new List<string>(SelectedIndicatorNids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

                if (dtSessionData != null)
                {
                    //remove entry from series data  where iusnid not seleted
                    for (i = 0; i < diMap.SeriesData.Count; i++)
                    {
                        if (!VisibleSeriesList.Contains(diMap.SeriesData[i].SeriesID))
                        {
                            diMap.HiddenSeriesData.Add(diMap.SeriesData[i]);
                            removedList.Add(diMap.SeriesData[i].SeriesID);
                            diMap.SeriesData.Remove(diMap.SeriesData.ItemIndex(diMap.SeriesData[i].SeriesID));
                            i--;
                        }
                    }

                    //add entry if visible series
                    //for (int i = 0; i < VisibleSeriesList.Count; i++)
                    for (i = 0; i < diMap.HiddenSeriesData.Count; i++)
                    {
                        //if (diMap.HiddenSeriesData[VisibleSeriesList[i]] != null && !removedList.Contains(VisibleSeriesList[i]))
                        if (VisibleSeriesList.Contains(diMap.HiddenSeriesData[i].SeriesID))
                        {
                            diMap.SeriesData.Add(diMap.HiddenSeriesData[i]);
                            diMap.HiddenSeriesData.Remove(i);
                            i--;
                        }
                    }

                    for (int themeIndex = 0; themeIndex < diMap.Themes.Count; themeIndex++)
                    {
                        isSetSelectedSeriesID = false;
                        FilterText.Clear();

                        theme = diMap.Themes[themeIndex];
                        ThemeID = theme.ID;
                        ThemeName = theme.Name;
                        themeSelectedSeries = new List<string>(theme.SelectedSeriesID.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

                        foreach (string serID in themeSelectedSeries)
                        {
                            if (VisibleSeriesList.Contains(serID))
                            {
                                //if (theme.Type == ThemeType.Chart && themeSelectedSeries[themeSelectedSeries.Count - 1] != serID)
                                //{
                                //    continue;
                                //}

                                isSetSelectedSeriesID = true;

                            }
                            else
                            {
                                isSetSelectedSeriesID = false;
                                break;
                            }
                        }

                        if (isSetSelectedSeriesID == false)
                        {
                            diMap.Themes.Remove(themeIndex);
                            themeIndex--;
                            continue;
                        }

                        if (isSetSelectedSeriesID == false && VisibleSeriesList.Count > 0)
                        {
                            //change series ID
                            SeriesIDs = VisibleSeriesList[0];
                        }

                        SeriesIDs = theme.SelectedSeriesID;


                        breakType = theme.BreakType;
                        breakCount = theme.BreakCount;
                        themeType = theme.Type;
                        chartType = theme.ChartType;

                        isMyData = diMap.isMyData;
                        seriesData = diMap.SeriesData;
                        OldTheme = theme;


                        if (!dtSessionData.Columns.Contains(diMap.MapDataColumns.isMRD))
                        {
                            FilterText.Append(diMap.MapDataColumns.SeriesNid + " in(" + SeriesIDs + ") ");
                        }
                        else
                        {
                            FilterText.Append(diMap.MapDataColumns.SeriesNid + " in(" + SeriesIDs + ") ");
                            if (theme.DisplayChartMRD)
                            {
                                FilterText.Append(" and " + diMap.MapDataColumns.isMRD + "='true' ");
                            }
                        }

                        //check if Most recent is true then 
                        if (!OldTheme.isMRD && !string.IsNullOrEmpty(OldTheme.SelectedTimePeriod) && dtSessionData.Columns.Contains(diMap.MapDataColumns.TimePeriod))
                        {
                            FilterText.Append(" and " + diMap.MapDataColumns.TimePeriod + "='" + OldTheme.SelectedTimePeriod + "' ");
                        }
                        else
                        {
                            //do nothing for  MRD is true
                        }

                        //set areaNids selection
                        if (!string.IsNullOrEmpty(SelectedAreaNids))
                        {
                            AreaNIdList.AddRange(SelectedAreaNids.Split(','));

                            if (!string.IsNullOrEmpty(FilterText.ToString().Trim()))
                                FilterText.Append(" and ");

                            FilterText.Append(diMap.MapDataColumns.AreaNId + " in(" + SelectedAreaNids + ") ");
                        }

                        //set IndicatorNids selection
                        if (!string.IsNullOrEmpty(SelectedIndicatorNids))
                        {
                            IndicatorNidList.AddRange(SelectedIndicatorNids.Split(','));

                            if (!string.IsNullOrEmpty(FilterText.ToString().Trim()))
                                FilterText.Append(" and ");

                            FilterText.Append(diMap.MapDataColumns.SeriesNid + " in(" + SelectedIndicatorNids + ") ");
                        }

                        if (theme != null)
                        {
                            //get series data by applying default filter
                            SeriesDataTable = this.GetSeriesData(dtSessionData, FilterText.ToString());

                            //Reset theme name while changing series id
                            seriesIDsList.AddRange(SeriesIDs.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                            if (seriesIDsList.Count == 1 && seriesIDsList[0] != theme.SelectedSeriesID)
                            {
                                ThemeName = diMap.LanguageStrings.Theme + " - " + seriesData[seriesIDsList[0]].SeriesName;
                                foreach (Theme thme in diMap.Themes)
                                {
                                    //if (thme.Name.ToLower().StartsWith(ThemeName.ToLower()))
                                    if (thme.Name.StartsWith(ThemeName))
                                    {
                                        themeNames.Add(thme.Name);
                                        if (thme.Name != ThemeName)
                                        {
                                            try
                                            {
                                                ThemeNumber = thme.Name.Substring(thme.Name.LastIndexOf("(") + 1);
                                                themeNamesDisc.Add(Convert.ToInt32(ThemeNumber.Substring(0, ThemeNumber.LastIndexOf(")"))), thme.Name);
                                            }
                                            catch (Exception ex)
                                            {
                                                Global.CreateExceptionString(ex, null);
                                            }
                                        }
                                        countTheme++;
                                    }
                                }

                                if (countTheme > 0)
                                {
                                    try
                                    {
                                        //set theme names with correct numbers start from
                                        for (i = 1; i <= diMap.Themes.Count; i++)
                                        {
                                            if (!themeNamesDisc.ContainsKey(i))
                                            {
                                                countTheme = i;
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Global.CreateExceptionString(ex, null);

                                    }

                                    ThemeName += " (" + countTheme + ") ";
                                }

                                //as per requirement TimePeriod or Most recent data append after map title 
                                diMap.Title = seriesData[seriesIDsList[0]].SeriesName;

                                this.AppendTimePeriodinMapTitle(diMap, dtSessionData, FilterText.ToString());
                            }

                            diMap.UpdateTheme(SeriesDataTable.DefaultView, themeType, breakCount, breakType, chartType, SeriesIDs, OldTheme);
                            diMap.Themes[ThemeID].SelectedSeriesID = SeriesIDs;
                            diMap.Themes[ThemeID].Name = ThemeName;
                            diMap.Themes[ThemeID].LegendTitle = ThemeName;

                            if (!string.IsNullOrEmpty(diMap.Themes[ThemeID].SelectedTimePeriod) && diMap.Themes[ThemeID].isMRD == false)
                            {
                                //reset map title
                                diMap.Title = diMap.SeriesData[SeriesIDs].SeriesName + " (" + diMap.Themes[ThemeID].SelectedTimePeriod + ")";
                            }
                        }
                    }

                    if (diMap.Themes.Count == 0)
                    {
                        //create default theme
                        this.ReCreateDefaultTheme(diMap);
                    }
                }
            }
            else
            {
                //set all as series data as hidden and clear Series Data
                foreach (SeriesInfo serInfo in diMap.SeriesData)
                {
                    diMap.HiddenSeriesData.Add(serInfo);
                }
                diMap.SeriesData.Clear();

                diMap.Themes.Clear();
            }

            if (!string.IsNullOrEmpty(SelectedAreaNids))
            {
                areaNameSelectedRecord = this.GetCSVInList(diMap.MapData, diMap.MapDataColumns.AreaName, true, diMap.MapDataColumns.AreaNId + " in(" + SelectedAreaNids + ")");
                areaIDSelectedRecord = this.GetCSVInList(diMap.MapData, diMap.MapDataColumns.AreaID, true, diMap.MapDataColumns.AreaNId + " in(" + SelectedAreaNids + ")");
            }

            //set layer visiblity true/false
            foreach (Layer layer in diMap.Layers)
            {
                isLayerVisible = false;
                if (layer.ID.ToLower().StartsWith("ind"))
                {
                }

                for (i = 0; i < areaIDSelectedRecord.Count; i++)
                {
                    if (layer.Records[areaIDSelectedRecord[i]] != null || layer.LayerName == areaNameSelectedRecord[i])
                    {
                        isLayerVisible = true;
                        break;
                    }
                }
                //foreach (string areaid in areaIDSelectedRecord)
                //{
                //    if (layer.Records[areaid] != null)
                //    {
                //        isLayerVisible = true;
                //        break;
                //    }
                //}

                layer.Visible = isLayerVisible;
            }

            //Map image URl is pic from getimage function already
            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMap"] = diMap;
            Session["DataViewNonQDS"] = diMap.MapData;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From EditThemeInfo ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            GC.Collect();
        }

        return RetVal;
    }

    public string EditThemeInfo(string ThemeID, string SeriesIDs, string breakType, string breakCount, string themeType, string chartType)
    {
        string RetVal = string.Empty;
        bool isMyData = false;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        DataTable dtSessionData = null;
        DataTable SeriesDataTable = null;
        SeriesData seriesData = null;
        string seriesID = string.Empty;
        string seriesName = string.Empty;
        string FilterText = string.Empty;
        string ThemeName = string.Empty;
        Theme OldTheme;
        List<string> seriesIDsList = new List<string>();
        Theme theme = null;
        int countTheme = 0;
        List<string> themeNames = new List<string>();
        Dictionary<int, string> themeNamesDisc = new Dictionary<int, string>();
        string ThemeNumber = string.Empty;
        DataTable timeperiodTable = null;
        bool isSeriesChange = true;
        List<string> IUSName = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            dtSessionData = diMap.MapData;

            if (dtSessionData != null)
            {
                theme = diMap.Themes[ThemeID];
                ThemeName = theme.Name;

                isMyData = diMap.isMyData;
                seriesData = diMap.SeriesData;
                OldTheme = diMap.Themes[ThemeID];

                if (dtSessionData.Columns.Contains(diMap.MapDataColumns.isMRD))
                {
                    if ((theme.Type != ThemeType.Chart || (theme.Type == ThemeType.Chart && theme.DisplayChartMRD == true)) && (diMap.IsMRDColumnAddedInTable == false && string.IsNullOrEmpty(theme.SelectedTimePeriod)))
                    {
                        FilterText = diMap.MapDataColumns.SeriesNid + " in(" + SeriesIDs + ") and " + diMap.MapDataColumns.isMRD + "='true' ";
                    }
                    else
                    {
                        FilterText = diMap.MapDataColumns.SeriesNid + " in(" + SeriesIDs + ") ";
                    }
                }
                else
                {
                    FilterText = diMap.MapDataColumns.SeriesNid + " in(" + SeriesIDs + ") ";
                }

                //check if Most recent is true then 
                if (!OldTheme.isMRD && !string.IsNullOrEmpty(OldTheme.SelectedTimePeriod) && dtSessionData.Columns.Contains(diMap.MapDataColumns.TimePeriod))
                {
                    FilterText += " and " + diMap.MapDataColumns.TimePeriod + "='" + OldTheme.SelectedTimePeriod + "' ";
                }
                else
                {
                    //do nothing for  MRD is true
                }

                if (string.IsNullOrEmpty(themeType))
                {
                    themeType = OldTheme.Type.ToString();
                    //themeType = ThemeType.Color.ToString();
                }

                if (string.IsNullOrEmpty(chartType))
                {
                    chartType = OldTheme.ChartType.ToString();
                    //chartType = ChartType.Column.ToString();
                }

                if (theme != null)
                {
                    //get series data by applying default filter
                    SeriesDataTable = this.GetSeriesData(dtSessionData, FilterText);

                    //Reset theme name while changing series id
                    seriesIDsList.AddRange(SeriesIDs.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                    if (seriesIDsList.Count == 1 && seriesIDsList[0] != theme.SelectedSeriesID)
                    {
                        IUSName = new List<string>(seriesData[seriesIDsList[0]].SeriesName.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));

                        //ThemeName = diMap.LanguageStrings.Theme + " - " + seriesData[seriesIDsList[0]].SeriesName;
                        ThemeName = IUSName[IUSName.Count - 1].Trim();

                        //if theme exists then append theme type
                        if (diMap.Themes.Exists(ThemeName) && diMap.Themes.GetThemeByName(ThemeName).ID != theme.ID)
                        {
                            if (theme.Type == ThemeType.DotDensity)
                            {
                                ThemeName = "Dot Density - " + ThemeName;
                            }
                            else
                            {
                                ThemeName = theme.Type.ToString() + " - " + ThemeName;
                            }
                        }

                        //check again if exists theme name woth theme type then
                        if (diMap.Themes.Exists(ThemeName) && diMap.Themes.GetThemeByName(ThemeName).ID != theme.ID)
                        {
                            foreach (Theme thme in diMap.Themes)
                            {
                                //if (thme.Name.ToLower().StartsWith(ThemeName.ToLower()))
                                if (thme.Name.StartsWith(ThemeName))
                                {
                                    themeNames.Add(thme.Name);
                                    if (thme.Name != ThemeName)
                                    {
                                        try
                                        {
                                            ThemeNumber = thme.Name.Substring(thme.Name.LastIndexOf("(") + 1);
                                            themeNamesDisc.Add(Convert.ToInt32(ThemeNumber.Substring(0, ThemeNumber.LastIndexOf(")"))), thme.Name);
                                        }
                                        catch (Exception ex)
                                        {
                                            Global.CreateExceptionString(ex, null);
                                        }
                                    }
                                    countTheme++;
                                }
                            }
                        }

                        if (countTheme > 0)
                        {
                            try
                            {
                                //set theme names with correct numbers start from
                                for (int i = 1; i <= diMap.Themes.Count; i++)
                                {
                                    if (!themeNamesDisc.ContainsKey(i))
                                    {
                                        countTheme = i;
                                        break;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Global.CreateExceptionString(ex, null);

                            }

                            ThemeName += " (" + countTheme + ") ";
                        }

                        //as per requirement TimePeriod or Most recent data append after map title 
                        diMap.Title = seriesData[seriesIDsList[0]].SeriesName;

                        this.AppendTimePeriodinMapTitle(diMap, dtSessionData, FilterText);
                    }

                    if (theme.SelectedSeriesID != SeriesIDs)
                    {
                        //Preserve legend only when seriesNot change
                        isSeriesChange = false;
                    }

                    diMap.UpdateTheme(SeriesDataTable.DefaultView, (ThemeType)Enum.Parse(typeof(ThemeType), themeType), Convert.ToInt32(breakCount), (BreakType)Enum.Parse(typeof(BreakType), breakType), (ChartType)Enum.Parse(typeof(ChartType), chartType), SeriesIDs, OldTheme, isSeriesChange);
                    diMap.Themes[ThemeID].SelectedSeriesID = SeriesIDs;
                    diMap.Themes[ThemeID].Name = ThemeName;
                    diMap.Themes[ThemeID].LegendTitle = ThemeName;
                    diMap.Themes[ThemeID].TimePeriods = this.GetCSVInList(diMap.MapData, diMap.MapDataColumns.TimePeriod, true, diMap.MapDataColumns.SeriesNid + " in(" + SeriesIDs + ") ");

                    if (!string.IsNullOrEmpty(diMap.Themes[ThemeID].SelectedTimePeriod) && diMap.Themes[ThemeID].isMRD == false)
                    {
                        //reset map title
                        diMap.Title = diMap.SeriesData[SeriesIDs].SeriesName + " (" + diMap.Themes[ThemeID].SelectedTimePeriod + ")";
                    }
                }

                diMap.Themes[ThemeID].SetRangeCount(SeriesDataTable.DefaultView, diMap.SourceData, diMap.MapDataColumns);

                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

                //add map into  session variable
                Session["DIMap"] = diMap;

                //serialize map in last
                MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
                this.SerializeObject(MapFileWPath, diMap);
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From EditThemeInfo ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetThemeInfo(string ThemeID)
    {
        string RetVal = string.Empty;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".xml");
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        string MapFolder = string.Empty;
        Map diMap = null;
        Themes themes = null;
        bool visible = false;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            if (!string.IsNullOrEmpty(ThemeID))
            {
                themes = new Themes();
                visible = diMap.Themes[ThemeID].Visible;

                Theme theme = (Theme)diMap.Themes[ThemeID].Clone();
                theme.AreaIndexes = new System.Collections.Hashtable();
                themes.Add(theme);
                theme.Visible = visible;
            }
            else
            {
                themes = (Themes)diMap.Themes.Clone();
                foreach (Theme theme in themes)
                {
                    theme.Visible = diMap.Themes[theme.ID].Visible;
                    theme.AreaIndexes = new System.Collections.Hashtable();
                }
            }

            //get theme info xml for all themes
            RetVal = this.GetXmlOfSerializeObject(TempFileWPath, themes);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetThemeInfo ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetTimeperiodOfSeries(string ThemeID, string SelectedSeriesID)
    {
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".xml");
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        string MapFolder = string.Empty;
        Map diMap = null;
        List<string> TimePeriodList = null;
        StringBuilder RetVal = new StringBuilder();
        List<string> seriesNameList = new List<string>();

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            if (!string.IsNullOrEmpty(ThemeID) && diMap.Themes[ThemeID].TimePeriods != null)
            {
                //RetVal = String.Join(",", diMap.Themes[ThemeID].TimePeriods);
                TimePeriodList = this.GetCSVInList(diMap.MapData, diMap.MapDataColumns.TimePeriod, true, diMap.MapDataColumns.SeriesNid + " in(" + SelectedSeriesID + ") ");
                seriesNameList = new List<string>(diMap.SeriesData[SelectedSeriesID].SeriesName.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                //RetVal = String.Join(",", TimePeriodList);

                //JSON format
                //{"tps":["2000","2009","2010"],"sg":"subgroup name"}

                RetVal.Append("{");
                RetVal.Append("\"tps\":");
                RetVal.Append("[");
                for (int i = 0; i < TimePeriodList.Count; i++)
                {
                    RetVal.Append("\"" + TimePeriodList[i] + "\"");
                    if (i != TimePeriodList.Count - 1)
                        RetVal.Append(",");
                }
                RetVal.Append("]");
                RetVal.Append(",");
                RetVal.Append("\"sg\":");
                RetVal.Append("\"" + seriesNameList[seriesNameList.Count - 1].Trim() + "\"");
                RetVal.Append("}");
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetTimeperiodOfSeries ->" + ex.Message);
            RetVal.Append("false" + Constants.Delimiters.ParamDelimiter + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ThemeID"></param>
    /// <param name="OpacityFor">ChartLegend to get opacity for chart legend</param>
    /// <returns></returns>
    public string GetChartLegendOpacity(string ThemeID, string OpacityFor)
    {
        string RetVal = string.Empty;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".xml");
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        string MapFolder = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            if (!string.IsNullOrEmpty(ThemeID) && OpacityFor == "ChartLegend")
            {
                RetVal = Color.FromArgb(Convert.ToInt32(diMap.Themes[ThemeID].SeriesFillStyle[0])).A.ToString();
            }

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetTimeperiodOfSeries ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetThemeCount()
    {
        string RetVal = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            RetVal = diMap.Themes.Count.ToString();
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetTimeperiodOfSeries ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    public string AddTheme(string param1)
    {
        string RetVal = string.Empty;
        string[] param;

        string MapFileWPath = string.Empty;
        string TemplateStyleFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Template);
        string MapFolder = string.Empty;
        Map diMap = null;
        DataTable dtSessionData = null;
        DataTable SeriesDataTable = null;
        SeriesData seriesData = null;
        string FilterText = string.Empty;

        bool isMyData = false;

        string themeName = string.Empty;
        string themeType = string.Empty;
        string seriesIDs = string.Empty;
        string chartType = string.Empty;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            dtSessionData = diMap.MapData;

            param = Global.SplitString(param1, Constants.Delimiters.ParamDelimiter);

            themeName = param[0];
            themeType = param[1];
            seriesIDs = param[2];
            chartType = param[3];

            if (this.IsThemeExists(string.Empty, themeName, seriesIDs, themeType, chartType))
            {
                RetVal = "exist";
                return RetVal;
            }

            if (dtSessionData != null)
            {
                isMyData = diMap.isMyData;
                seriesData = diMap.SeriesData;

                if (!dtSessionData.Columns.Contains(diMap.MapDataColumns.isMRD))
                {
                    FilterText = diMap.MapDataColumns.SeriesNid + " in(" + seriesIDs + ") ";
                }
                else
                {
                    FilterText = diMap.MapDataColumns.SeriesNid + " in(" + seriesIDs + ") ";
                    if (themeType != ThemeType.Chart.ToString())
                    {
                        FilterText += " and " + diMap.MapDataColumns.isMRD + "='true' ";
                    }
                }

                //get series data by applying default filter
                SeriesDataTable = this.GetSeriesData(dtSessionData, FilterText);

                if (string.IsNullOrEmpty(themeType))
                {
                    themeType = ThemeType.Color.ToString();
                }

                if (string.IsNullOrEmpty(chartType))
                {
                    chartType = ChartType.Column.ToString();
                }

                //TODO set theme name and ID
                Theme theme = diMap.CreateTheme(SeriesDataTable.DefaultView, (ThemeType)Enum.Parse(typeof(ThemeType), themeType), 4, BreakType.EqualCount, (ChartType)Enum.Parse(typeof(ChartType), chartType), seriesIDs, -1);
                theme.Name = themeName;
                theme.LegendTitle = themeName;

                if (themeType == ThemeType.Chart.ToString() && chartType == ChartType.Column.ToString())
                {
                    //theme.ShowChartAxis = false;
                }

                //if (themeType == ThemeType.Chart.ToString() && chartType == ChartType.Pie.ToString())
                //{
                //    theme.PieAutoSize = false;
                //}

                theme.TimePeriods = this.GetCSVInList(diMap.MapData, diMap.MapDataColumns.TimePeriod, true);

                //add map into  session variable
                Session["DIMap"] = diMap;

                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

                //serialize map in last
                MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
                this.SerializeObject(MapFileWPath, diMap);
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From AddTheme ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="themeID">themeID</param>
    /// <param name="param2">param2 : theme name[****]theme type[****]series[****]chart type (if theme type is Chart else empty)</param>
    /// <returns></returns>
    public string UpdateTheme(string ThemeID, string param2, string isMRD, string Selectedtimeperiod)
    {
        string RetVal = string.Empty;
        string[] param;

        string MapFileWPath = string.Empty;
        string TemplateStyleFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Template);
        string MapFolder = string.Empty;
        Map diMap = null;
        DataTable dtSessionData = null;
        DataTable SeriesDataTable = null;
        SeriesData seriesData = null;
        string FilterText = string.Empty;

        bool isMyData = false;

        string themeName = string.Empty;
        string themeType = string.Empty;
        string seriesIDs = string.Empty;
        string chartType = string.Empty;
        Theme OldTheme = null;

        try
        {


            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            dtSessionData = diMap.MapData;

            param = Global.SplitString(param2, Constants.Delimiters.ParamDelimiter);

            themeName = param[0];
            themeType = param[1];
            seriesIDs = param[2];
            chartType = param[3];

            if (this.IsThemeExists(ThemeID, themeName, seriesIDs, themeType, chartType))
            {
                RetVal = "exist";
                return RetVal;
            }

            if (dtSessionData != null)
            {
                isMyData = diMap.isMyData;
                seriesData = diMap.SeriesData;
                OldTheme = diMap.Themes[ThemeID];

                if (!dtSessionData.Columns.Contains(diMap.MapDataColumns.isMRD) || isMyData)
                {
                    FilterText = diMap.MapDataColumns.SeriesNid + " in(" + seriesIDs + ") ";
                }
                else
                {
                    FilterText = diMap.MapDataColumns.SeriesNid + " in(" + seriesIDs + ") ";
                    if (diMap.Themes[ThemeID].DisplayChartMRD || isMRD == "true")
                    {
                        FilterText += " and " + diMap.MapDataColumns.isMRD + "='true' ";
                    }
                }

                //check if Most recent is true then 
                if (!string.IsNullOrEmpty(isMRD))
                {
                    if (isMRD == "false" && !string.IsNullOrEmpty(Selectedtimeperiod) && dtSessionData.Columns.Contains(diMap.MapDataColumns.TimePeriod))
                    {
                        FilterText += " and " + diMap.MapDataColumns.TimePeriod + "='" + Selectedtimeperiod + "' ";
                    }
                    else
                    {
                        //do nothing for  MRD is true
                    }
                }

                if (string.IsNullOrEmpty(themeType))
                {
                    themeType = OldTheme.Type.ToString();
                }

                if (string.IsNullOrEmpty(chartType))
                {
                    chartType = OldTheme.ChartType.ToString();
                }

                if (diMap.Themes[ThemeID] != null)
                {
                    //get series data by applying default filter
                    SeriesDataTable = this.GetSeriesData(dtSessionData, FilterText);
                    if (isMRD == "false" && string.IsNullOrEmpty(Selectedtimeperiod.Trim()))
                    {
                        for (int i = 0; i < OldTheme.ChartTimePeriods.Count; i++)
                        {
                            string tp = OldTheme.ChartTimePeriods.Keys[i];
                            OldTheme.ChartTimePeriods[tp] = true;
                        }
                    }
                    diMap.UpdateTheme(SeriesDataTable.DefaultView, (ThemeType)Enum.Parse(typeof(ThemeType), themeType), diMap.Themes[ThemeID].BreakCount, diMap.Themes[ThemeID].BreakType, (ChartType)Enum.Parse(typeof(ChartType), chartType), seriesIDs, OldTheme);
                    diMap.Themes[ThemeID].SelectedSeriesID = seriesIDs;

                    //reset theme properties
                    diMap.Themes[ThemeID].Name = themeName;
                    diMap.Themes[ThemeID].LegendTitle = themeName;

                    //reset most recent and selected timeperiod
                    if (!string.IsNullOrEmpty(isMRD))
                        diMap.Themes[ThemeID].isMRD = Convert.ToBoolean(isMRD);
                    if (!string.IsNullOrEmpty(Selectedtimeperiod) && diMap.Themes[ThemeID].isMRD == false)
                    {
                        diMap.Themes[ThemeID].SelectedTimePeriod = Selectedtimeperiod;

                        //reset map title
                        diMap.Title = diMap.SeriesData[seriesIDs].SeriesName + " (" + Selectedtimeperiod + ")";
                    }
                    else
                    {
                        if (seriesIDs.Split(',').Length == 1)
                        {
                            diMap.Title = diMap.SeriesData[seriesIDs].SeriesName;
                            this.AppendTimePeriodinMapTitle(diMap, SeriesDataTable, FilterText);
                        }
                    }
                }

                //add map into  session variable
                Session["DIMap"] = diMap;

                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

                //serialize map in last
                MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
                this.SerializeObject(MapFileWPath, diMap);
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From UpdateTheme ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="themeID">themeID</param>
    /// <param name="param2">param2 : theme name[****]theme type[****]series[****]chart type (if theme type is Chart else empty)</param>
    /// <returns></returns>
    public string UpdateThemeLegends(string ThemeID, string param2, string timeP, string persistLegends)
    {
        string RetVal = string.Empty;
        string[] param;
        string MapFileWPath = string.Empty;
        string TemplateStyleFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Template);
        string MapFolder = string.Empty;
        Map diMap = null;
        DataTable dtSessionData = null;
        DataTable SeriesDataTable = null;
        SeriesData seriesData = null;
        string FilterText = string.Empty;
        bool isMyData = false;
        string themeName = string.Empty;
        string themeType = string.Empty;
        string seriesIDs = string.Empty;
        string chartType = string.Empty;
        Theme OldTheme = null;
        try
        {
            diMap = this.GetSessionMapObject();
            dtSessionData = diMap.MapData;
            param = Global.SplitString(param2, Constants.Delimiters.ParamDelimiter);
            themeName = param[0];
            themeType = param[1];
            seriesIDs = param[2];
            chartType = param[3];
            if (this.IsThemeExists(ThemeID, themeName, seriesIDs, themeType, chartType))
            {
                RetVal = "exist";
                return RetVal;
            }
            if (dtSessionData != null)
            {
                isMyData = diMap.isMyData;
                seriesData = diMap.SeriesData;
                OldTheme = diMap.Themes[ThemeID];
                FilterText = diMap.MapDataColumns.SeriesNid + " in(" + seriesIDs + ") ";
                bool persistLegendsParsed = false;
                if (bool.TryParse(persistLegends, out persistLegendsParsed))
                {
                    if (persistLegendsParsed)
                    {
                        if (!string.IsNullOrEmpty(timeP))
                        {
                            FilterText += " and " + diMap.MapDataColumns.TimePeriod + "='" + timeP + "' ";
                            if (!string.IsNullOrEmpty(timeP))
                            {
                                OldTheme.SelectedTimePeriod = timeP;
                                diMap.Title = diMap.SeriesData[seriesIDs].SeriesName + " (" + timeP + ")";
                            }
                        }
                    }
                }
                if (string.IsNullOrEmpty(themeType))
                {
                    themeType = OldTheme.Type.ToString();
                }
                if (string.IsNullOrEmpty(chartType))
                {
                    chartType = OldTheme.ChartType.ToString();
                }

                if (diMap.Themes[ThemeID] != null)
                {
                    SeriesDataTable = this.GetSeriesData(dtSessionData, FilterText);
                    if (!persistLegendsParsed)
                    {
                        diMap.UpdateThemeLegend(OldTheme, SeriesDataTable.DefaultView, false);
                    }
                    else
                    {
                        diMap.UpdateRenderingInfo(SeriesDataTable.DefaultView, OldTheme, diMap.SourceData, diMap.MapDataColumns, timeP);
                    }
                    diMap.Themes[ThemeID].SelectedSeriesID = seriesIDs;
                    //reset theme properties
                    diMap.Themes[ThemeID].Name = themeName;
                    diMap.Themes[ThemeID].LegendTitle = themeName;
                    diMap.Themes[ThemeID].isMRD = false;
                }
                //add map into  session variable
                Session["DIMap"] = diMap;
                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));
                //serialize map in last
                MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
                this.SerializeObject(MapFileWPath, diMap);
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From UpdateTheme ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }
        return RetVal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ThemeID"></param>
    /// <param name="isMRD"></param>
    /// <param name="Selectedtimeperiod"></param>
    /// <returns></returns>
    public string SetSelectedTimePeriodInTheme(string ThemeID, string isMRD, string Selectedtimeperiod)
    {
        string RetVal = string.Empty;

        string MapFileWPath = string.Empty;
        string TemplateStyleFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Template);
        string MapFolder = string.Empty;
        Map diMap = null;
        DataTable dtSessionData = null;
        DataTable SeriesDataTable = null;
        SeriesData seriesData = null;
        string FilterText = string.Empty;

        bool isMyData = false;

        string themeName = string.Empty;
        string seriesIDs = string.Empty;
        Theme OldTheme = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            dtSessionData = diMap.MapData;

            if (dtSessionData != null)
            {
                isMyData = diMap.isMyData;
                seriesData = diMap.SeriesData;

                OldTheme = diMap.Themes[ThemeID];

                //set selected timeperiod for all theme except chart theme
                if (OldTheme.Type != ThemeType.Chart)
                {
                    seriesIDs = OldTheme.SelectedSeriesID;
                    themeName = OldTheme.Name;

                    if (!dtSessionData.Columns.Contains(diMap.MapDataColumns.isMRD))
                    {
                        FilterText = diMap.MapDataColumns.SeriesNid + " in(" + seriesIDs + ") ";
                    }
                    else
                    {
                        FilterText = diMap.MapDataColumns.SeriesNid + " in(" + seriesIDs + ") ";
                        if (diMap.Themes[ThemeID].DisplayChartMRD)
                        {
                            FilterText += " and " + diMap.MapDataColumns.isMRD + "='true' ";
                        }
                    }

                    //check if Most recent is true then 
                    if (!string.IsNullOrEmpty(isMRD))
                    {
                        if (isMRD == "false" && !string.IsNullOrEmpty(Selectedtimeperiod) && dtSessionData.Columns.Contains(diMap.MapDataColumns.TimePeriod))
                        {
                            FilterText += " and " + diMap.MapDataColumns.TimePeriod + "='" + Selectedtimeperiod + "' ";
                        }
                        else
                        {
                            //do nothing for  MRD is true
                        }
                    }

                    if (diMap.Themes[ThemeID] != null)
                    {
                        //get series data by applying default filter
                        SeriesDataTable = this.GetSeriesData(dtSessionData, FilterText);

                        diMap.UpdateTheme(SeriesDataTable.DefaultView, OldTheme.Type, diMap.Themes[ThemeID].BreakCount, diMap.Themes[ThemeID].BreakType, OldTheme.ChartType, seriesIDs, OldTheme);
                        diMap.Themes[ThemeID].SelectedSeriesID = seriesIDs;

                        //reset theme properties
                        diMap.Themes[ThemeID].Name = themeName;
                        diMap.Themes[ThemeID].LegendTitle = themeName;

                        //reset most recent and selected timeperiod
                        if (!string.IsNullOrEmpty(isMRD))
                            diMap.Themes[ThemeID].isMRD = Convert.ToBoolean(isMRD);
                        if (!string.IsNullOrEmpty(Selectedtimeperiod) && diMap.Themes[ThemeID].isMRD == false)
                            diMap.Themes[ThemeID].SelectedTimePeriod = Selectedtimeperiod;
                    }

                    //add map into  session variable
                    Session["DIMap"] = diMap;
                }

                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

                //serialize map in last
                MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
                this.SerializeObject(MapFileWPath, diMap);
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From UpdateTheme ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string SetThemeVisiblity(string themeID, string visible)
    {
        string RetVal = string.Empty;
        Map diMap = null;
        string MapFileWPath = string.Empty;

        try
        {

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            diMap.Themes[themeID].Visible = Convert.ToBoolean(visible);

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetThemeVisiblity ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string SetDisputedBoundriesVisiblity(string isVisible)
    {
        string RetVal = string.Empty;
        Map diMap = null;
        string MapFileWPath = string.Empty;

        try
        {

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            diMap.ShowDIBLayers = Convert.ToBoolean(isVisible);

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetDisputedBoundriesVisiblity ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string DeleteTheme(string themeID)
    {
        string RetVal = string.Empty;
        Map diMap = null;
        string MapFileWPath = string.Empty;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            diMap.Themes.Remove(diMap.Themes.ItemIndex(themeID));

            //set visiblity in if only one theme exists
            if (diMap.Themes.Count == 1)
            {
                diMap.Themes[0].Visible = true;
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From DeleteTheme ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// update dot density information like dot style size color value
    /// </summary>
    /// <param name="themeID">param1 : themeId</param>
    /// <param name="param2">param2 : style[****]size[****]color[****]value</param>
    /// <returns></returns>
    public string UpdateDotDensityTheme(string themeID, string param2)
    {
        string RetVal = string.Empty;
        Map diMap = null;
        string MapFileWPath = string.Empty;
        string[] paramValues = null;
        MarkerStyle DotStyle;
        float DotSize = 0;
        Color DotColor;
        double DotValue = 0;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            paramValues = Global.SplitString(param2, Constants.Delimiters.ParamDelimiter);

            DotStyle = (MarkerStyle)(Enum.Parse(typeof(MarkerStyle), paramValues[0]));
            DotSize = float.Parse(paramValues[1]);
            DotColor = ColorTranslator.FromHtml(paramValues[2]);
            DotValue = double.Parse(paramValues[3]);

            diMap.Themes[themeID].DotStyle = DotStyle;
            diMap.Themes[themeID].DotSize = DotSize;
            diMap.Themes[themeID].DotColor = DotColor;
            diMap.Themes[themeID].DotValue = DotValue;

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From UpdateDotDensityTheme ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public bool IsThemeExists(string themeID, string themeName, string seriesIDs, string themeType, string chartType)
    {
        bool RetVal = false;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //check new theme case
            foreach (Theme theme in diMap.Themes)
            {

                if (string.IsNullOrEmpty(themeID))
                {
                    // NEW cASE
                    //1 check same name
                    if (theme.Name.ToLower() == themeName.ToLower())
                    {
                        RetVal = true;
                    }

                    //2. check themetype and series ID Combination also and for chart check chart type
                    if (!RetVal && theme.Type.ToString() == themeType && seriesIDs == theme.SelectedSeriesID)
                    {
                        if (theme.Type != ThemeType.Chart)
                        {
                            RetVal = true;
                        }
                        if (theme.Type == ThemeType.Chart && theme.ChartType.ToString() == chartType)
                        {
                            RetVal = true;
                        }
                    }
                }
                else
                {
                    // EDIT cASE
                    //1 check same name
                    if (theme.Name.ToLower() == themeName.ToLower() && theme.ID != themeID)
                    {
                        RetVal = true;
                    }

                    //2. check themetype and series ID Combination also and for chart check chart type
                    if (!RetVal && theme.Type.ToString() == themeType && seriesIDs == theme.SelectedSeriesID && theme.ID != themeID)
                    {
                        if (theme.Type != ThemeType.Chart)
                        {
                            RetVal = true;
                        }
                        if (theme.Type == ThemeType.Chart && theme.ChartType.ToString() == chartType)
                        {
                            RetVal = true;
                        }
                    }
                }



                if (RetVal)
                {
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From IsThemeExists ->" + ex.Message);
            Global.CreateExceptionString(ex, null);

            RetVal = false;
        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// Get Theme name by theme id
    /// </summary>
    /// <param name="themeID"></param>
    /// <returns></returns>
    public string GetSeriesThemeName(string themeID)
    {
        string RetVal = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            if (diMap.Themes[themeID] != null)
            {
                RetVal = diMap.Themes[themeID].Name;
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetSeriesThemeName ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #region "Chart"

    /// <summary>
    /// Update chart width, height, Color fill style
    /// </summary>
    /// <param name="themeID">theme ID</param>
    /// <param name="param2">to update width,height,color</param>
    /// <param name="param3">values to update</param>
    /// <param name="param4">chart time period</param>
    /// <returns></returns>
    public string UpdateChartSettings(string themeID, string param2, string param3)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        Theme theme = null;

        try
        {

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            theme = diMap.Themes[themeID];
            switch (param2)
            {
                case "width":
                    if (theme.ChartType != ChartType.Column)
                    {
                        //theme.ChartSize = int.Parse(param3);
                        theme.PieAutoSizeFactor = (float.Parse(param3) / 2);
                    }
                    else
                        theme.ChartWidth = int.Parse(param3);
                    break;
                case "height":
                    theme.ChartSize = int.Parse(param3);
                    break;
                default:
                    break;
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));
            //diMap.DrawMap(this.TempPath + "ss.png", null);

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From UpdateChartSettings ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #region "TimePeriods"

    /// <summary>
    /// to exclude timeperiods from theme
    /// </summary>
    /// <param name="themeID"></param>
    /// <param name="param2"></param>
    /// <returns></returns>
    public string SetChartTimePeriodsVisiblity(string themeID, string param2)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        List<string> tps = new List<string>();
        SortedList<string, bool> chartTimePeriod = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            tps.AddRange(param2.Split(','));

            if (diMap.Themes.ItemIndex(themeID) > -1)
            {
                chartTimePeriod = diMap.Themes[themeID].ChartTimePeriods;

                for (int i = 0; i < chartTimePeriod.Count; i++)
                {
                    if (tps.Contains(chartTimePeriod.Keys[i]))
                        chartTimePeriod[chartTimePeriod.Keys[i]] = false;
                    else
                        chartTimePeriod[chartTimePeriod.Keys[i]] = true;
                }
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));
            //diMap.DrawMap(this.TempPath + "ss.png", null);

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetChartTimePeriodsVisiblity ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// Get time period
    /// </summary>
    /// <param name="themeID"></param>
    /// <returns></returns>
    public string GetChartTimePeriods(string themeID)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        string timeperiods = string.Empty;
        Theme theme = null;

        SortedList<string, bool> chartTimePeriod = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            if (diMap.Themes.ItemIndex(themeID) > -1)
            {
                theme = diMap.Themes[themeID];

                //for chart theme hide columns
                if (theme.Type == ThemeType.Chart)
                {
                    chartTimePeriod = theme.ChartTimePeriods;
                    for (int i = 0; i < chartTimePeriod.Count; i++)
                    {
                        RetVal += chartTimePeriod.Keys[i];
                        if (!string.IsNullOrEmpty(RetVal.Trim()))
                        {
                            if (chartTimePeriod[chartTimePeriod.Keys[i]])
                            {
                                RetVal += "_t,";
                            }
                            else
                            {
                                RetVal += "_f,";
                            }
                        }
                    }
                }

                //timeperiods = this.GetCSV(diMap.MapData, Timeperiods.TimePeriod);
                //foreach (string timeperiod in timeperiods.Split(','))
                //{
                //    RetVal += timeperiod;

                //    if (!diMap.ExcludeTimePeriods.Contains(timeperiod))
                //    {

                //    }
                //    else
                //    {

                //    }
                //}

                //time period list
                RetVal = RetVal.Trim(',');
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetChartTimePeriods ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #region "--Layer--"

    /// <summary>
    /// Get in feature layer info
    /// </summary>
    /// <returns></returns>
    public string GetFeatureLayerInfo()
    {
        string RetVal = string.Empty;
        Map diMap = null;
        StringBuilder ObjStringBuilder = new StringBuilder();
        Layer LYR = null;
        string colorCode = string.Empty;
        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //---------------------DEMO            
            //{ 
            //    "disputed":[true],
            //    "feature":["id[****]visible[****]type[****]color[****]style[****]opacity/size/thickness",
            //        "id[****]visible[****]type[****]color[****]style[****]opacity/size/thickness"
            //       ]
            //}

            ObjStringBuilder.Append("{ ");
            ObjStringBuilder.Append("\"showdisputed\":[" + Global.showdisputed + "],");
            ObjStringBuilder.Append("\"disputed\":[" + diMap.ShowDIBLayers.ToString().ToLower() + "],");
            ObjStringBuilder.Append("\"feature\":[");

            //"id[****]visible[****]type[****]color[****]style[****]opacity/size/thickness"
            for (int i = 0; i < diMap.Layers.Count; i++)
            {
                LYR = diMap.Layers[i];
                switch (LYR.LayerType)
                {
                    case ShapeType.PointCustom:
                    case ShapeType.PointFeature:
                    case ShapeType.PolyLineCustom:
                    case ShapeType.PolyLineFeature:
                    case ShapeType.PolygonBuffer:
                    case ShapeType.PolygonCustom:
                    case ShapeType.PolygonFeature:
                        //id[****]visible[****]type[****]color
                        if (LYR.LayerType == ShapeType.PointCustom) LYR.LayerType = ShapeType.PointFeature;
                        if (LYR.LayerType == ShapeType.PolyLineCustom) LYR.LayerType = ShapeType.PolyLineFeature;
                        if (LYR.LayerType == ShapeType.PolygonCustom) LYR.LayerType = ShapeType.PolygonFeature;

                        ObjStringBuilder.Append("\"" + LYR.ID + Constants.Delimiters.ParamDelimiter + LYR.Visible.ToString().ToLower() + Constants.Delimiters.ParamDelimiter + LYR.LayerType + Constants.Delimiters.ParamDelimiter + Global.GetHexColorCode(LYR.FillColor));

                        //[****]style[****]opacity/size/thickness
                        if (LYR.LayerType == ShapeType.PointFeature || LYR.LayerType == ShapeType.PointCustom)
                            ObjStringBuilder.Append(Constants.Delimiters.ParamDelimiter + LYR.MarkerStyle + Constants.Delimiters.ParamDelimiter + LYR.MarkerSize);

                        if (LYR.LayerType == ShapeType.PolyLineFeature || LYR.LayerType == ShapeType.PolyLineCustom)
                            ObjStringBuilder.Append(Constants.Delimiters.ParamDelimiter + LYR.BorderStyle + Constants.Delimiters.ParamDelimiter + LYR.BorderSize);

                        if (LYR.LayerType == ShapeType.PolygonCustom || LYR.LayerType == ShapeType.PolygonFeature || LYR.LayerType == ShapeType.PolygonBuffer)
                            ObjStringBuilder.Append(Constants.Delimiters.ParamDelimiter + LYR.FillStyle + Constants.Delimiters.ParamDelimiter + LYR.FillColor.A);

                        ObjStringBuilder.Append(Constants.Delimiters.ParamDelimiter + LYR.IsDIB.ToString().ToLower() + "\"");
                        ObjStringBuilder.Append(",");
                        break;
                }
            }

            if (ObjStringBuilder.ToString().EndsWith(","))
                ObjStringBuilder.Remove(ObjStringBuilder.Length - 1, 1);

            ObjStringBuilder.Append("]");
            ObjStringBuilder.Append("}");
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetFeatureLayerInfo ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        RetVal = ObjStringBuilder.ToString();

        return RetVal;
    }

    /// <summary>
    /// Get in feature layer info
    /// </summary>
    /// <returns></returns>
    public string GetBaseLayerInfo()
    {
        string RetVal = string.Empty;
        Map diMap = null;
        StringBuilder ObjStringBuilder = new StringBuilder();
        Layer LYR = null;
        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //---------------------DEMO            
            //{ 
            //    "base":["id[****]visible[****]type",
            //     "id[****]visible[****]type"
            //    ]
            //}

            ObjStringBuilder.Append("{ ");
            ObjStringBuilder.Append(" \"base\":[");

            //"id[****]visible[****]type"
            for (int i = diMap.Layers.Count - 1; i >= 0; i--)
            {
                LYR = diMap.Layers[i];
                switch (LYR.LayerType)
                {
                    case ShapeType.Point:
                    case ShapeType.Polygon:
                    case ShapeType.PolyLine:
                        ObjStringBuilder.Append("\"" + LYR.ID + Constants.Delimiters.ParamDelimiter + LYR.Visible.ToString().ToLower() + Constants.Delimiters.ParamDelimiter + LYR.LayerType + "\"");
                        ObjStringBuilder.Append(",");
                        break;
                }
            }

            if (ObjStringBuilder.ToString().EndsWith(","))
                ObjStringBuilder.Remove(ObjStringBuilder.Length - 1, 1);

            ObjStringBuilder.Append("]");
            ObjStringBuilder.Append("}");
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetBaseLayerInfo ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        RetVal = ObjStringBuilder.ToString();

        return RetVal;
    }

    /// <summary>
    /// Set Disputed Boundry Visible only if showdisputed is set true in appSettings.xml
    /// </summary>
    /// <returns>true/false Error message</returns>
    public string SetDisputedBoundryVisible(string disputedBoundryVisible)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            if (Global.showdisputed == "true")
            {
                //step To load map from session/ NewMap/ From preserved file
                diMap = this.GetSessionMapObject();

                diMap.ShowDIBLayers = Convert.ToBoolean(disputedBoundryVisible);

                //add map into  session variable
                Session["DIMap"] = diMap;

                //RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

                //serialize map in last
                MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
                this.SerializeObject(MapFileWPath, diMap);
            }
            RetVal = "true";
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetFeatureLayerInfo->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }


        return RetVal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string ShowDisputedBoundryDefault()
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            if (Global.showdisputed == "true")
            {
                //step To load map from session/ NewMap/ From preserved file
                diMap = this.GetSessionMapObject();

                diMap.ShowDIBLayers = true;
                diMap.ShowDIBLayers = false;

                //add map into  session variable
                Session["DIMap"] = diMap;

                //serialize map in last
                MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
                this.SerializeObject(MapFileWPath, diMap);
            }
            RetVal = "true";
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ShowDisputedBoundryDefault->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }


        return RetVal;
    }

    /// <summary>
    /// Set in feature layer info
    /// </summary>
    /// <returns></returns>
    public string SetFeatureLayerInfo(string disputedBoundryVisible, string param2)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        Layer LYR = null;
        string[] ParamValues = null;
        string[] LayerInformation = null;
        string LayerID = string.Empty;

        bool LayerVisible = false;
        Color FillColor;
        MarkerStyle markerStyle;
        int MarkerSize = 0;
        DashStyle borderStyle;
        float BorderSize = 0;
        FillStyle fillStyle;
        bool ShowDICLayersOld = false;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            ParamValues = Global.SplitString(param2, Constants.Delimiters.ParamDelimiter);

            ShowDICLayersOld = diMap.ShowDIBLayers;
            diMap.ShowDIBLayers = Convert.ToBoolean(disputedBoundryVisible);

            foreach (string layerInfo in ParamValues)
            {
                //"id[****]visible[****]color[****]style[****]opacity/size/thickness"
                LayerInformation = Global.SplitString(layerInfo, Constants.Delimiters.ValuesDelimiter);

                LayerID = LayerInformation[0];
                LayerVisible = Convert.ToBoolean(LayerInformation[1]);
                FillColor = ColorTranslator.FromHtml(LayerInformation[2]);

                LYR = diMap.Layers[LayerID];

                if (diMap.ShowDIBLayers == true && LYR.IsDIB)
                    LYR.Visible = LayerVisible;
                else if (!LYR.IsDIB)
                    LYR.Visible = LayerVisible;

                LYR.FillColor = FillColor;

                if (LYR.LayerType == ShapeType.PointFeature || LYR.LayerType == ShapeType.PointCustom)
                {
                    markerStyle = (MarkerStyle)Enum.Parse(typeof(MarkerStyle), LayerInformation[3]);
                    MarkerSize = Convert.ToInt32(LayerInformation[4]);

                    LYR.MarkerStyle = markerStyle;
                    LYR.MarkerSize = MarkerSize;
                }

                if (LYR.LayerType == ShapeType.PolyLineFeature || LYR.LayerType == ShapeType.PolyLineCustom)
                {
                    borderStyle = (DashStyle)Enum.Parse(typeof(DashStyle), LayerInformation[3]);
                    BorderSize = float.Parse(LayerInformation[4]);

                    LYR.BorderStyle = borderStyle;
                    LYR.BorderSize = BorderSize;
                }

                if (LYR.LayerType == ShapeType.PolygonCustom || LYR.LayerType == ShapeType.PolygonFeature || LYR.LayerType == ShapeType.PolygonBuffer)
                {
                    fillStyle = (FillStyle)Enum.Parse(typeof(FillStyle), LayerInformation[3]);

                    LYR.FillStyle = fillStyle;
                    //Opacity is included in LayerInformation[2] param valur for fill color
                    //LYR.FillColor.A = Convert.ToInt32(LayerInformation[4]);
                }
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetFeatureLayerInfo->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }


        return RetVal;
    }

    /// <summary>
    /// Set be layer info
    /// </summary>
    /// <returns></returns>
    public string SetBaseLayerInfo(string param, string upDownTopBottomLayerIDs)
    {
        string RetVal = string.Empty;
        string MapFolder = string.Empty;
        Map diMap = null;
        Layer LYR = null;
        string[] LayerInformation = null;
        string[] ParamValues = null;
        string MapFileWPath = string.Empty;
        List<string> UpDownTopBottomLayerIDsList = null;
        Layers layersClone = null;
        int MoveFrom = 0;
        int MoveTo = 0;

        string LayerID = string.Empty;
        bool LayerVisible = false;
        int Z;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            ParamValues = Global.SplitString(param, Constants.Delimiters.ParamDelimiter);

            layersClone = (Layers)diMap.Layers.Clone();

            UpDownTopBottomLayerIDsList = Global.SplitStringInList(upDownTopBottomLayerIDs, Constants.Delimiters.ParamDelimiter);

            //"id[****]visible[****]type"
            for (int i = 0; i < ParamValues.Length; i++)
            {
                LayerInformation = Global.SplitString(ParamValues[i], Constants.Delimiters.ValuesDelimiter);
                LayerID = LayerInformation[0];
                LayerVisible = Convert.ToBoolean(LayerInformation[1]);

                LYR = diMap.Layers[LayerID];
                LYR.Visible = LayerVisible;

                //set move up down top bottom of layer
                if (UpDownTopBottomLayerIDsList.Contains(LYR.ID))
                {
                    MoveFrom = layersClone.LayerIndex(LYR.ID);
                    MoveTo = i;

                    //Old and Solved Problem with move up and down
                    //if (MoveFrom != MoveTo)
                    //    diMap.Layers.SwapLayer(MoveFrom, MoveTo);

                    //TODO get move to layer index
                    for (int x = 0; x < ParamValues.Length; x++)
                    {
                        if (ParamValues[x].StartsWith(LayerInformation[0] + Constants.Delimiters.ValuesDelimiter))
                        {
                            MoveTo = ParamValues.Length - x - 1;
                            break;
                        }
                    }

                    if (MoveFrom > MoveTo)
                    {
                        for (Z = MoveFrom; Z > MoveTo; Z--)
                        {
                            diMap.Layers.MoveUp(Z);
                        }
                    }
                    else if (MoveFrom != MoveTo)
                    {
                        for (Z = MoveFrom; Z < MoveTo; Z++)
                        {
                            diMap.Layers.MoveDown(Z);
                        }
                    }
                }
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetBaseLayerInfo :->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #region "-- Legend --"

    /// <summary>
    /// 
    /// </summary>
    /// <param name="themeID"></param>
    /// <param name="param2">//breakcount, breaktype, max, min, decimals</param>
    /// <param name="legendIndex"></param>
    /// <param name="editedLegendRangeTo"></param>
    /// <returns></returns>
    public string GetUpdatedLegendRanges(string themeID, int legendIndex, string editedLegendRangeTo)
    {
        string RetVal = string.Empty;
        Map diMap = null;
        Theme theme = null;
        string MapFileWPath = string.Empty;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            theme = diMap.Themes[themeID];

            theme.UpdateLegendRanges(legendIndex, decimal.Parse(editedLegendRangeTo), diMap.MapDataColumns);
            theme.Legends[legendIndex].RangeTo = decimal.Parse(editedLegendRangeTo);

            for (int i = legendIndex; i < theme.Legends.Count - 1; i++)
            {
                Legend legend = theme.Legends[i];
                legend.Caption = legend.RangeFrom + " - " + legend.RangeTo;

                //RetVal += legend.Caption + Constants.Delimiters.ValuesDelimiter;
                RetVal += legend.RangeFrom + Constants.Delimiters.ValuesDelimiter;
                RetVal += legend.RangeTo + Constants.Delimiters.ValuesDelimiter;
                RetVal += legend.ShapeCount + Constants.Delimiters.ValuesDelimiter;
                RetVal += legend.Caption;

                if (i < theme.Legends.Count - 2)
                {
                    RetVal += Constants.Delimiters.ParamDelimiter;
                }
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetUpdatedLegendRanges ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="themeID"></param>
    /// <param name="param2">//breakcount, breaktype, max, min, decimals</param>
    /// <returns></returns>
    public string GetLegendRangesByThemeChange(string themeID, string param2)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        Theme theme = null;
        //breakcount, breaktype, max, min, decimals
        string maxValue = string.Empty;
        string minValue = string.Empty;
        string FilterText = string.Empty;

        int BreakCount = 0;
        BreakType breakType;
        decimal Maximum = 0;
        decimal Minimum = 0;
        int Decimals = 0;
        DataTable dtSessionData = null;
        DataTable SeriesDataTable = null;

        string[] param = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            dtSessionData = diMap.MapData;

            param = Global.SplitString(param2, Constants.Delimiters.ParamDelimiter);
            BreakCount = Convert.ToInt32(param[0]);
            breakType = (BreakType)Enum.Parse(typeof(BreakType), param[1]);
            Maximum = decimal.Parse(param[2]);
            Minimum = decimal.Parse(param[3]);
            Decimals = Convert.ToInt32(param[4]);

            theme = (Theme)diMap.Themes[themeID];
            theme.BreakCount = BreakCount;
            theme.BreakType = breakType;
            theme.Maximum = Maximum;
            theme.Minimum = Minimum;
            theme.Decimals = Decimals;

            //to solve bug 4270
            theme.Legends.Clear();

            if (!dtSessionData.Columns.Contains(diMap.MapDataColumns.isMRD))
            {
                FilterText = diMap.MapDataColumns.SeriesNid + " in(" + theme.SelectedSeriesID + ") ";
            }
            else
            {
                FilterText = diMap.MapDataColumns.SeriesNid + " in(" + theme.SelectedSeriesID + ") ";
                if (theme.Type != ThemeType.Chart)
                {
                    FilterText += " and " + diMap.MapDataColumns.isMRD + "='true' ";
                }
            }

            //get series data by applying default filter
            SeriesDataTable = this.GetSeriesData(dtSessionData, FilterText);

            diMap.setThemeRange(themeID, SeriesDataTable.DefaultView.Table);

            theme.Smooth(ColorTranslator.FromHtml(theme.StartColor), ColorTranslator.FromHtml(theme.EndColor));

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetLegendRangesByThemeChange ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string Smooth(string ThemeID, string firstColor, string endColor)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            Theme theme = (Theme)diMap.Themes[ThemeID].Clone();

            theme.StartColor = firstColor;
            theme.EndColor = endColor;
            if (theme.Type != ThemeType.Chart && theme.Type != ThemeType.DotDensity)
            {
                theme.Smooth(ColorTranslator.FromHtml(firstColor), ColorTranslator.FromHtml(endColor));
            }

            for (int i = 0; i < theme.Legends.Count - 1; i++)
            {
                RetVal += Global.GetHexColorCode(theme.Legends[i].Color) + ",";
            }

            RetVal = RetVal.Trim(',');

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From Smooth->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="themeID"></param>
    /// <param name="param2">
    /// legendIndex~~Caption~~Title~~RangeFrom~~RangeTo~~ShapeCount~~Color
    /// legendIndex~~Caption~~Title~~RangeFrom~~RangeTo~~ShapeCount~~Color~~FillStyle
    /// legendIndex~~Caption~~Title~~RangeFrom~~RangeTo~~ShapeCount~~Color~~MarkerChar~~MarkerSize~~MarkerFont name
    /// </param>
    /// <returns></returns>
    public string UpdateLegendRange(string themeID, string param2)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        Legends legends = null;
        int legendIndex = 0;
        string[] paramLegends = null;
        string[] paramLegend = null;
        string LegendTitle = string.Empty;
        decimal RangeFrom = 0;
        decimal RangeTo = 0;
        string LegendCaption = string.Empty;
        int ShapeCount = 0;
        Color LegendColor;
        FillStyle fillStyle;
        char MarkerChar;
        int MarkerSize = 0;
        Font MarkerFont = null;


        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            legends = diMap.Themes[themeID].Legends;

            paramLegends = Global.SplitString(param2, Constants.Delimiters.ParamDelimiter);

            //legendIndex~~Caption~~Title~~RangeFrom~~RangeTo~~ShapeCount~~Color
            foreach (string legend in paramLegends)
            {
                paramLegend = Global.SplitString(legend, Constants.Delimiters.ValuesDelimiter);

                legendIndex = Convert.ToInt32(paramLegend[0]);
                if (legends.Count - 1 > legendIndex)
                {
                    LegendTitle = paramLegend[2];
                    RangeFrom = decimal.Parse(paramLegend[3]);
                    RangeTo = decimal.Parse(paramLegend[4]);

                    legends[legendIndex].Title = LegendTitle;
                    legends[legendIndex].RangeFrom = RangeFrom;
                    legends[legendIndex].RangeTo = RangeTo;
                }

                LegendCaption = paramLegend[1];
                ShapeCount = int.Parse(paramLegend[5]);
                LegendColor = ColorTranslator.FromHtml(paramLegend[6]);

                legends[legendIndex].Caption = LegendCaption;
                legends[legendIndex].ShapeCount = ShapeCount;
                legends[legendIndex].Color = LegendColor;

                //if (diMap.Themes[themeID].Type != ThemeType.Symbol)
                //{
                //    legends[legendIndex].Color = LegendColor;
                //}

                if (diMap.Themes[themeID].Type == ThemeType.Hatch)
                {
                    fillStyle = (FillStyle)Enum.Parse(typeof(FillStyle), paramLegend[7]);

                    legends[legendIndex].FillStyle = fillStyle;
                }

                if (diMap.Themes[themeID].Type == ThemeType.Symbol)
                {
                    MarkerChar = Convert.ToChar(Convert.ToInt32(paramLegend[7].Trim()));
                    MarkerSize = Convert.ToInt32(paramLegend[8].Trim());
                    MarkerFont = new Font(paramLegend[9], MarkerSize);

                    legends[legendIndex].MarkerChar = MarkerChar;
                    legends[legendIndex].MarkerSize = MarkerSize;
                    legends[legendIndex].MarkerFont = MarkerFont;
                }

                //Update break counts
                diMap.Themes[themeID].UpdateLegendBreakCount(diMap.MapDataColumns);
            }


            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From UpdateLegendRange->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string ResetLegendColor(string ThemeID, int legendIndex, string NewColor)
    {
        string RetVal = string.Empty;
        Map diMap = null;
        string MapFileWPath = string.Empty;
        Legends ObjLegends = null;
        string LegendColor = string.Empty;
        string LegendTitle = string.Empty;
        string[] seriesFillStyle = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //GET Legend color + legend title
            if (diMap.Themes != null && diMap.Themes.Count > 0 && diMap.Themes[ThemeID] != null)
            {
                ObjLegends = (Legends)((Theme)diMap.Themes[ThemeID]).Legends;
                if (diMap.Themes[ThemeID].Type == ThemeType.Chart)
                {
                    seriesFillStyle = new string[diMap.Themes[ThemeID].SeriesFillStyle.Length];
                    for (int i = 0; i < diMap.Themes[ThemeID].SeriesFillStyle.Length; i++)
                    {
                        seriesFillStyle[i] = diMap.Themes[ThemeID].SeriesFillStyle[i];
                    }

                    seriesFillStyle[legendIndex] = Convert.ToString(ColorTranslator.FromHtml(NewColor).ToArgb());

                    diMap.Themes[ThemeID].SeriesFillStyle = seriesFillStyle;
                }
                else if (diMap.Themes[ThemeID].Type == ThemeType.DotDensity)
                {
                    diMap.Themes[ThemeID].DotColor = ColorTranslator.FromHtml(NewColor);
                }
                else
                {
                    ObjLegends[legendIndex].Color = ColorTranslator.FromHtml(NewColor);
                }

                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));
            }
            else
            {
                RetVal = "false";
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ResetLegendColor->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// update width height and color (by edit settings)
    /// </summary>
    /// <param name="ThemeID"></param>
    /// <param name="param1">width, height, viewdatavalue, decimal, timeseries ~ most recent</param>
    /// <param name="param3">index~~color[****]index~~color[****]index~~color</param>
    /// <returns></returns>
    public string UpdateChartLegendSettings(string ThemeID, string param1, string param3, string param2)
    {
        string RetVal = string.Empty;
        Map diMap = null;
        string MapFileWPath = string.Empty;
        string LegendColor = string.Empty;
        string LegendTitle = string.Empty;
        string[] paramChartSettings = null;

        string[] paramChartInfo = null;
        string[] seriesFillStyle = null;
        int LegendIndex = -1;

        int ChartWidth = 0;
        int ChartSize = 0;
        bool DisplayChartData = false;
        int Decimals = 0;
        bool DisplayChartMRD = false;

        List<string> tps = new List<string>();
        SortedList<string, bool> chartTimePeriod = null;
        string timeperiods = string.Empty;
        Theme theme = null;

        string LegendColorARGB = string.Empty;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //1) set chart property [[param1 contains [width, height, viewdatavalue, decimal, timeseries ~ most recent]]]
            paramChartSettings = Global.SplitString(param1, Constants.Delimiters.ParamDelimiter);

            ChartWidth = int.Parse(paramChartSettings[0]);
            ChartSize = int.Parse(paramChartSettings[1]);
            DisplayChartData = Convert.ToBoolean(paramChartSettings[2]);
            Decimals = int.Parse(paramChartSettings[3]);
            theme = diMap.Themes[ThemeID];

            //theme.ChartWidth = ChartWidth;
            //theme.ChartSize = ChartSize;
            if (theme.ChartType == ChartType.Line)
            {
                theme.ChartLineThickness = int.Parse(paramChartSettings[1]);
                theme.PieAutoSizeFactor = (float.Parse(paramChartSettings[0]) / 2);
            }
            else if (theme.ChartType == ChartType.Pie)
                theme.PieAutoSizeFactor = (float.Parse(paramChartSettings[0]) / 2);
            else
            {
                theme.ChartWidth = ChartWidth;
                theme.ChartSize = ChartSize;
            }

            theme.DisplayChartData = DisplayChartData;
            theme.Decimals = Decimals;

            //Most recent MRD or time series
            DisplayChartMRD = Convert.ToBoolean(paramChartSettings[4]);
            theme.DisplayChartMRD = DisplayChartMRD;

            paramChartSettings = null;
            //2) set legend information
            if (!string.IsNullOrEmpty(param3))
            {
                // param2 contains [index,color value]
                paramChartSettings = Global.SplitString(param3, Constants.Delimiters.ParamDelimiter);
                seriesFillStyle = new string[diMap.Themes[ThemeID].SeriesFillStyle.Length];

                foreach (string chartSettings in paramChartSettings)
                {
                    paramChartInfo = Global.SplitString(chartSettings, Constants.Delimiters.ValuesDelimiter);
                    LegendIndex = Convert.ToInt32(paramChartInfo[0]);
                    LegendColorARGB = Convert.ToString(ColorTranslator.FromHtml(paramChartInfo[1]).ToArgb());
                    seriesFillStyle[LegendIndex] = LegendColorARGB;
                }
                diMap.Themes[ThemeID].SeriesFillStyle = seriesFillStyle;
            }

            //3) setting chart settings //add into exclude time period list            
            if (!string.IsNullOrEmpty(param2))
            {
                tps.AddRange(param2.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }

            //for chart theme hide columns and is based on param2
            if (theme.Type == ThemeType.Chart)
            {
                chartTimePeriod = theme.ChartTimePeriods;
                for (int i = 0; i < chartTimePeriod.Count; i++)
                {
                    if (tps.Contains(chartTimePeriod.Keys[i]))
                        chartTimePeriod[chartTimePeriod.Keys[i]] = false;
                    else
                        chartTimePeriod[chartTimePeriod.Keys[i]] = true;
                }
            }

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From UpdateChartLegendSettings->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string ChangeHatchPatern(string ThemeID, string legendIndex, string patternType)
    {
        string RetVal = string.Empty;
        Map diMap = null;
        string MapFileWPath = string.Empty;
        Legends ObjLegends = null;
        string LegendColor = string.Empty;
        string LegendTitle = string.Empty;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //GET Legend color + legend title
            if (diMap.Themes != null && diMap.Themes.Count > 0 && diMap.Themes[ThemeID] != null)
            {
                ObjLegends = (Legends)((Theme)diMap.Themes[ThemeID]).Legends;

                ObjLegends[int.Parse(legendIndex)].FillStyle = (FillStyle)Enum.Parse(typeof(FillStyle), patternType);

                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));
            }
            else
            {
                RetVal = "false";
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ChangeHatchPattern->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetSeriesInfo()
    {
        string RetVal = string.Empty;
        Map diMap = null;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".xml");
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        string LegendColor = string.Empty;
        string LegendTitle = string.Empty;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            RetVal = this.GetXmlOfSerializeObject(TempFileWPath, diMap.SeriesData);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetSeriesInfo ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }
        return RetVal;
    }

    public string SetLegendSeriesColor(string ThemeID, string firstColor, string endColor)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            Theme theme = diMap.Themes[ThemeID];

            theme.StartColor = firstColor;
            theme.EndColor = endColor;
            if (theme.Type != ThemeType.Chart && theme.Type != ThemeType.DotDensity)
            {
                theme.Smooth(ColorTranslator.FromHtml(firstColor), ColorTranslator.FromHtml(endColor));
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetLegendSeriesColor->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// <LegendInfo><Legends></Legends><\LegendInfo>
    /// </summary>
    /// <param name="ThemeID"></param>
    /// <param name="legendInfo"></param>
    /// <returns></returns>
    public string UploadLegends(string ThemeID, string legendInfoFileNameWPath)
    {
        string RetVal = string.Empty;
        Map diMap = null;
        string MapFileWPath = string.Empty;
        DataTable dtSessionData = null;
        DataTable SeriesDataTable = null;
        SeriesData seriesData = null;
        string FilterText = string.Empty;
        bool isMyData = false;

        string themeName = string.Empty;
        string themeType = string.Empty;
        string chartType = string.Empty;
        Theme OldTheme = null;
        Theme NewTheme = null;
        string breakType = string.Empty;
        string breakCount = string.Empty;
        LegendInfo LegendInfo;
        Legend Newlegend = null;
        Legend SavedLegend = null;
        int LegendCount = 0;
        Decimal min = 0, max = 0;
        Legends legends = null;
        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();
            LegendInfo = (LegendInfo)this.GetSerializeObjectFromXml(legendInfoFileNameWPath, typeof(LegendInfo));

            dtSessionData = diMap.MapData;

            if (dtSessionData != null && LegendInfo != null)
            {
                isMyData = diMap.isMyData;
                seriesData = diMap.SeriesData;
                OldTheme = diMap.Themes[ThemeID];

                if (!dtSessionData.Columns.Contains(diMap.MapDataColumns.isMRD))
                {
                    FilterText = diMap.MapDataColumns.SeriesNid + " in(" + OldTheme.SelectedSeriesID + ") ";
                }
                else
                {
                    FilterText = diMap.MapDataColumns.SeriesNid + " in(" + OldTheme.SelectedSeriesID + ") ";
                    if (diMap.Themes[ThemeID].DisplayChartMRD)
                    {
                        FilterText += " and " + diMap.MapDataColumns.isMRD + "='true' ";
                    }
                }

                themeType = OldTheme.Type.ToString();

                //to get from Uploaded legend Info

                chartType = LegendInfo.LegendChartType.ToString();
                breakType = LegendInfo.LegendBreakType.ToString();
                breakCount = LegendInfo.LegendBreakCount.ToString();

                if (diMap.Themes[ThemeID] != null)
                {
                    //get series data by applying default filter
                    SeriesDataTable = this.GetSeriesData(dtSessionData, FilterText);

                    NewTheme = diMap.UpdateTheme(SeriesDataTable.DefaultView, (ThemeType)Enum.Parse(typeof(ThemeType), themeType), Convert.ToInt32(breakCount), (BreakType)Enum.Parse(typeof(BreakType), breakType), (ChartType)Enum.Parse(typeof(ChartType), chartType), OldTheme.SelectedSeriesID, OldTheme);
                    NewTheme.SelectedSeriesID = OldTheme.SelectedSeriesID;


                    if (NewTheme.Legends != null && NewTheme.Legends.Count > 0)
                    {
                        if (NewTheme.Legends.Count != LegendInfo.Legends.Count)
                        {
                            LegendCount = LegendInfo.Legends.Count - 1;
                        }
                        else
                        {
                            LegendCount = NewTheme.Legends.Count;
                        }
                        for (int i = 0; i < LegendCount; i++)
                        {
                            Newlegend = NewTheme.Legends[i];
                            SavedLegend = LegendInfo.Legends[i];

                            Newlegend.Caption = SavedLegend.Caption;
                            Newlegend.Color = SavedLegend.Color;
                            Newlegend.Title = SavedLegend.Title;
                            Newlegend.RangeFrom = SavedLegend.RangeFrom;
                            Newlegend.RangeTo = SavedLegend.RangeTo;
                            min = Math.Min(min, Newlegend.RangeFrom);
                            max = Math.Max(max, Newlegend.RangeTo);

                            Newlegend.ShapeCount = SavedLegend.ShapeCount;

                            Newlegend.LabelField = SavedLegend.LabelField;
                            Newlegend.LabelIndented = SavedLegend.LabelIndented;
                            Newlegend.LabelMultiRow = SavedLegend.LabelMultiRow;
                            Newlegend.LabelVisible = SavedLegend.LabelVisible;
                            Newlegend.MarkerChar = SavedLegend.MarkerChar;
                            Newlegend.MarkerFont = SavedLegend.MarkerFont;
                            Newlegend.MarkerSize = SavedLegend.MarkerSize;
                            Newlegend.MarkerType = SavedLegend.MarkerType;
                            Newlegend.SymbolImage = SavedLegend.SymbolImage;
                            Newlegend.Visible = SavedLegend.Visible;
                            Newlegend.XmlColorType = SavedLegend.XmlColorType;
                            Newlegend.XmlMarkerFont = SavedLegend.XmlMarkerFont;
                        }
                    }
                }

                ////reset and Update theme Range
                NewTheme.Minimum = min;
                NewTheme.Maximum = max;
                NewTheme.SetRangeCount(SeriesDataTable.DefaultView, diMap.SourceData, diMap.MapDataColumns);

                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

                //add map into  session variable
                Session["DIMap"] = diMap;

                //serialize map in last
                MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
                this.SerializeObject(MapFileWPath, diMap);

                this.deleteFiles(legendInfoFileNameWPath);
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From UploadLegends ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    private void deleteFiles(string fileNames)
    {
        if (!string.IsNullOrEmpty(fileNames))
        {
            foreach (string fileName in fileNames.Split(','))
            {
                try
                {
                    if (File.Exists(fileName))
                        File.Delete(fileName);
                }
                catch (Exception ex)
                {
                    Global.CreateExceptionString(ex, null);

                }
            }
        }
    }

    #endregion

    #region "-- Zoom Panning Full extent--"

    public string ZoomIn()
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //apply zoom
            diMap.zoom(1.2f);

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ZoomIn ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string ZoomIn(string pZx, string pZy)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //apply zoom
            diMap.zoom(1.2f, float.Parse(pZx), float.Parse(pZy));

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ZoomIn ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string ZoomOut()
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //apply zoom
            diMap.zoom(0.75f, 0, 0);

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ZoomOut ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string ZoomOut(string pZx, string pZy)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            if (pZx == null)
            {
                pZx = "0";
                pZy = "0";
            }
            //apply zoom
            diMap.zoom(0.75f, float.Parse(pZx), float.Parse(pZy));

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ZoomOut ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string ZoomToRectangle(string mouseDownX, string mouseDownY, string x, string y)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //apply zoom
            diMap.zoom(Convert.ToInt32(mouseDownX), Convert.ToInt32(mouseDownY), Convert.ToInt32(x), Convert.ToInt32(y));

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From ZoomToRectangle ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }
        return RetVal;
    }

    public string Pan(string pX1, string pY1, string pX2, string pY2)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //apply panning
            diMap.Pan(int.Parse(pX1), int.Parse(pY1), int.Parse(pX2), int.Parse(pY2));

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From Pan ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string FullExtent()
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            diMap.SetFullExtent();

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From FullExtent ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #region "-- Label Font Border North symbol--"

    public string Label(string dataLabelVisible)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        CustomLabel customLabel = null;
        try
        {
            bool isDataLabelVisible = Convert.ToBoolean(dataLabelVisible);

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //Set data labels to true false
            if (diMap.Layers != null && diMap.Layers.Count > 0)
            {
                foreach (Layer layer in diMap.Layers)
                {
                    layer.LabelVisible = isDataLabelVisible;

                    foreach (DictionaryEntry Key in layer.ModifiedLabels)
                    {
                        customLabel = ((CustomLabel)Key.Value);
                        customLabel.Visible = isDataLabelVisible;
                        if (string.IsNullOrEmpty(customLabel.LabelField))
                        {
                            customLabel.LabelField = "1";
                        }
                    }

                    //special comment label field [0 show AreaId] [1 show AreaName default] [2 show DisplayInfo] [3 show Unitname] [4 show subgroup] [5 show TimePeriod] 
                    if (string.IsNullOrEmpty(layer.LabelField))
                    {
                        layer.LabelField = "1";
                    }
                }

                //add map into  session variable
                Session["DIMap"] = diMap;

                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

                //serialize map in last
                MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
                this.SerializeObject(MapFileWPath, diMap);
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From Label ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string NorthSymbol(string northSymbolVisible)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            diMap.NorthSymbol = Convert.ToBoolean(northSymbolVisible);

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From NorthSymbol ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string Scale(string scaleVisible)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        string visibleTps = string.Empty;

        try
        {

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            diMap.Scale = Convert.ToBoolean(scaleVisible);

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From Scale ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string Border(string borderWidth)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //Set Border with if true set to 1 else 0 
            foreach (Layer layer in diMap.Layers)
            {
                layer.BorderSize = float.Parse(borderWidth);
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From Border ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string SetBorderStyle(string isShowBorder, string borderWidth, string borderColor, string borderStyle)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            foreach (Layer layer in diMap.Layers)
            {
                if (isShowBorder == "false")
                {
                    borderWidth = "0.00";
                }

                layer.BorderSize = float.Parse(borderWidth);
                layer.BorderColor = ColorTranslator.FromHtml(borderColor);
                layer.BorderStyle = (DashStyle)Enum.Parse(typeof(DashStyle), borderStyle);
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetBorderStyle->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetBorderStyle()
    {
        string RetVal = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            if (diMap.Layers.Count > 0)
            {
                Layer layer = diMap.Layers[0];
                RetVal = ((layer.BorderSize > 0) ? "true" : "false") + Constants.Delimiters.ParamDelimiter +
                         layer.BorderSize + Constants.Delimiters.ParamDelimiter +
                         Global.GetHexColorCode(layer.BorderColor) + Constants.Delimiters.ParamDelimiter +
                         layer.BorderStyle;

            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetBorderStyle ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="areaID">areaID</param>
    /// <param name="param2">dropped x, y, width, height </param>
    /// <returns></returns>
    public string Nudging(string areaID, string param2)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        string[] paramValues;
        RectangleF placeHolder = new RectangleF();
        string LayerId = string.Empty;
        string AreaId = string.Empty;
        string caption = string.Empty;
        Layer layer = null;
        CustomLabel CustLabel = null;
        Shape shape = null;
        string Coordinates = string.Empty;
        float NewX = 0, NewY = 0, Width = 0, Height = 0;
        PointF StartPoint, StartPointMapCoordinate;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            paramValues = Global.SplitString(param2, Constants.Delimiters.ParamDelimiter);
            NewX = float.Parse(paramValues[0]);
            NewY = float.Parse(paramValues[1]);
            Width = float.Parse(paramValues[2]);
            Height = float.Parse(paramValues[3]);

            RectangleF CurExt = diMap.CurrentExtent;

            foreach (Layer LYR in diMap.Layers)
            {
                // Render layer only if it lies within current map extent
                Hashtable ht = LYR.GetRecords(LYR.LayerPath + "\\" + LYR.ID);
                IDictionaryEnumerator dicEnumerator = ht.GetEnumerator();
                while (dicEnumerator.MoveNext())
                {
                    if (((Shape)dicEnumerator.Value).AreaId == areaID)
                    {
                        shape = (Shape)dicEnumerator.Value;
                        caption = diMap.GetLabel(LYR, ref shape);
                        layer = LYR;
                        break;
                    }
                }
            }

            StartPoint = new PointF(NewX, NewY);
            placeHolder.Width = Width;
            placeHolder.Height = Height;
            placeHolder.X = StartPoint.X + (placeHolder.Width / 2);
            placeHolder.Y = StartPoint.Y + (placeHolder.Height / 2);

            StartPointMapCoordinate = diMap.PointToClient(placeHolder.X, placeHolder.Y);
            placeHolder.X = StartPointMapCoordinate.X;
            placeHolder.Y = StartPointMapCoordinate.Y;

            if (layer.ModifiedLabels.ContainsKey(areaID))
            {
                CustLabel = ((CustomLabel)layer.ModifiedLabels[areaID]);
                CustLabel.PlaceHolder = placeHolder;
                CustLabel.DrawPoint = StartPointMapCoordinate;
            }
            else
            {
                CustLabel = new CustomLabel();
                CustLabel.AreaId = areaID;

                CustLabel.Caption = caption;
                CustLabel.LabelField = layer.LabelField;
                CustLabel.MultiRow = layer.LabelMultirow;
                CustLabel.Indent = layer.LabelIndented;
                CustLabel.PlaceHolder = placeHolder;
                CustLabel.DrawPoint = StartPointMapCoordinate;
                CustLabel.LabelFont = layer.LabelFont;
                CustLabel.ForeColor = layer.LabelColor;
                CustLabel.LeaderVisible = diMap.LeaderVisible;
                CustLabel.LeaderWidth = diMap.LeaderWidth;
                CustLabel.LeaderStyle = diMap.LeaderStyle;
                CustLabel.LeaderColor = diMap.LeaderColor;
                layer.ModifiedLabels.Add(areaID, CustLabel);
            }

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From Nudging->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string SetDataLabelsJSON(string param)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        CustomLabel customLabel = null;
        bool dataLabelVisible = false;
        string labelFields = string.Empty;
        bool labelMultirow = false;
        bool showLabelWhereDataExists = false;
        bool showLabelEffects = false;

        bool showLeader = false;
        bool showStraightLine = false;
        LabelEffect effectType;
        int effectDepth = 0;
        Color effectColor;

        DashStyle leaderStyle = DashStyle.Solid;
        int leaderWidth = 0;
        string leaderWidtStr = string.Empty;
        Color leaderColor = new Color();
        string lineStyleStr = string.Empty;

        var jss = new JavaScriptSerializer();

        int fontSize = 0;
        FontStyle fontStyle;
        Color foreColor;
        string fontName = string.Empty;

        try
        {
            dynamic JsonData = jss.Deserialize<dynamic>(param);


            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            dataLabelVisible = Convert.ToBoolean(JsonData["showLabel"]);
            showLabelWhereDataExists = Convert.ToBoolean(JsonData["swde"]);
            labelMultirow = Convert.ToBoolean(JsonData["multiRow"]);
            labelFields = String.Join(",", JsonData["area"]).ToString().Trim(',');

            showLabelEffects = Convert.ToBoolean(JsonData["showEffect"]);
            effectType = (LabelEffect)Enum.Parse(typeof(LabelEffect), JsonData["effectType"]);
            effectDepth = Convert.ToInt32(JsonData["effectDepth"]);
            effectColor = ColorTranslator.FromHtml(JsonData["effectColor"]);
            showLeader = Convert.ToBoolean(JsonData["showLeader"]);
            showStraightLine = JsonData["straightLine"] == "yes" ? true : false;
            lineStyleStr = JsonData["lineStyle"];
            fontName = Convert.ToString(JsonData["label1"][0]);
            leaderWidtStr = Convert.ToString(JsonData["lineWidth"]);

            diMap.ShowPointStartingLdrLine = ((lineStyleStr.StartsWith("DOT") == true) ? true : false);

            diMap.ShowLabelWhereDataExists = showLabelWhereDataExists;
            diMap.ShowLabelEffects = showLabelEffects;
            diMap.LabelEffectSettings.Effect = effectType;
            diMap.LabelEffectSettings.Depth = effectDepth;
            diMap.LabelEffectSettings.EffectColor = effectColor;
            diMap.MultiRowLabel = labelMultirow;
            diMap.AngledNudgeLdrLine = showStraightLine;
            diMap.LeaderVisible = showLeader;

            //level1
            fontSize = Convert.ToInt32(JsonData["label1"][1]);
            fontStyle = this.GetFontStyle(Convert.ToBoolean(JsonData["label1"][2]), Convert.ToBoolean(JsonData["label1"][3]), Convert.ToBoolean(JsonData["label1"][4]));
            foreColor = ColorTranslator.FromHtml(JsonData["label1"][5]);
            diMap.LevelFontSettingList[0].FontSize = fontSize;
            diMap.LevelFontSettingList[0].FontStyle = fontStyle;
            diMap.LevelFontSettingList[0].ForeColor = foreColor;
            diMap.LevelFontSettingList[0].FontName = fontName;

            //level2
            if (labelMultirow == true)
            {
                fontSize = Convert.ToInt32(JsonData["label2"][1]);
                fontStyle = this.GetFontStyle(Convert.ToBoolean(JsonData["label2"][2]), Convert.ToBoolean(JsonData["label2"][3]), Convert.ToBoolean(JsonData["label2"][4]));
                foreColor = ColorTranslator.FromHtml(JsonData["label2"][5]);
                diMap.LevelFontSettingList[1].FontSize = fontSize;
                diMap.LevelFontSettingList[1].FontStyle = fontStyle;
                diMap.LevelFontSettingList[1].ForeColor = foreColor;
                diMap.LevelFontSettingList[1].FontName = Convert.ToString(JsonData["label2"][0]);
            }

            leaderWidth = Convert.ToInt32(Convert.ToDecimal(leaderWidtStr).ToString("##"));
            leaderStyle = (DashStyle)Enum.Parse(typeof(DashStyle), (lineStyleStr.StartsWith("DOT") ? lineStyleStr.Substring(3) : lineStyleStr));

            leaderColor = ColorTranslator.FromHtml(JsonData["lineColor"]);
            diMap.LeaderWidth = leaderWidth;
            diMap.LeaderStyle = leaderStyle;
            diMap.LeaderColor = leaderColor;

            //Set data labels to true false
            if (diMap.Layers != null && diMap.Layers.Count > 0)
            {
                foreach (Layer layer in diMap.Layers)
                {
                    //special comment label field [0 show AreaId] [1 show AreaName default] [2 show DisplayInfo] [3 show Unitname] [4 show subgroup] [5 show TimePeriod] 

                    if (string.IsNullOrEmpty(fontName))
                    {
                        fontName = layer.LabelFont.Name;
                    }

                    //labelColor = ColorTranslator.FromHtml(JsonData["label1"][5]);
                    //LabelFont = new Font(fontName, float.Parse(JsonData["label1"][1]), this.GetFontStyle(Convert.ToBoolean(JsonData["label1"][2]), Convert.ToBoolean(JsonData["label1"][3]), Convert.ToBoolean(JsonData["label1"][4])));
                    //layer.LabelColor = labelColor;
                    //layer.LabelFont = LabelFont;

                    layer.LabelVisible = dataLabelVisible;
                    layer.LabelField = labelFields;
                    layer.LabelMultirow = labelMultirow;
                    layer.ShowLabelEffects = diMap.ShowLabelEffects;
                    layer.LabelEffectSettings = diMap.LabelEffectSettings;

                    foreach (DictionaryEntry Key in layer.ModifiedLabels)
                    {
                        customLabel = ((CustomLabel)Key.Value);
                        customLabel.Visible = Convert.ToBoolean(dataLabelVisible);
                        customLabel.LabelField = labelFields;
                        customLabel.MultiRow = Convert.ToBoolean(labelMultirow);

                        customLabel.LeaderVisible = diMap.LeaderVisible;
                        customLabel.LeaderStyle = diMap.LeaderStyle;
                        customLabel.LeaderWidth = diMap.LeaderWidth;
                        customLabel.LeaderColor = diMap.LeaderColor;

                        customLabel.ForeColor = layer.LabelColor;
                        customLabel.LabelFont = layer.LabelFont;
                    }
                }

                //add map into  session variable
                Session["DIMap"] = diMap;

                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));
            }
            else
            {
                RetVal = "false";
            }

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetDataLabels->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetDataLabelsJSON()
    {
        //string RetVal = string.Empty;
        StringBuilder RetVal = new StringBuilder();
        Map diMap = null;


        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            ////JSON
            //{
            // showLabel:true/false,
            // swde:true/false,
            // multiRow:true/false,
            // area:[1,2],
            // label1:["Arial","15","true","true","true","#808080","yes"],
            // label2:["Arial","15","true","true","true","#808080","no"],
            // showEffect:true/false,
            // effectType:block/shadow,
            // effectDepth:1,
            // effectColor:#808080,
            // showLeader:true/false,
            // straightLine:yes/no,
            // lineStyle:solid/dash/dotted/dotSolid/,
            // lineWidth:12,
            // lineColor:#808080
            //}

            if (diMap != null && diMap.Layers.Count > 0)
            {
                Layer layer = diMap.Layers[0];
                RetVal.Append("{");
                RetVal.Append("\"" + "showLabel" + "\":" + "\"" + layer.LabelVisible.ToString().ToLower() + "\"" + ",");
                RetVal.Append("\"" + "swde" + "\":" + "\"" + diMap.ShowLabelWhereDataExists.ToString().ToLower() + "\"" + ",");
                RetVal.Append("\"" + "multiRow" + "\":" + "\"" + diMap.MultiRowLabel.ToString().ToLower() + "\"" + ",");

                RetVal.Append("\"" + "area" + "\":");
                RetVal.Append("[");
                RetVal.Append(layer.LabelField.Trim(','));
                RetVal.Append("]" + ",");


                //"Arial","15","true","true","true","#808080","yes"
                RetVal.Append("\"" + "label1" + "\":");
                RetVal.Append("[");
                RetVal.Append("\"" + diMap.LevelFontSettingList[0].FontName + "\"" + ",");
                RetVal.Append("\"" + diMap.LevelFontSettingList[0].FontSize + "\"" + ",");
                RetVal.Append("\"" + diMap.LevelFontSettingList[0].Font().Bold.ToString().ToLower() + "\"" + ",");
                RetVal.Append("\"" + diMap.LevelFontSettingList[0].Font().Italic.ToString().ToLower() + "\"" + ",");
                RetVal.Append("\"" + diMap.LevelFontSettingList[0].Font().Underline.ToString().ToLower() + "\"" + ",");
                RetVal.Append("\"" + Global.GetHexColorCode(diMap.LevelFontSettingList[0].ForeColor) + "\"" + ",");
                RetVal.Append("\"" + ((diMap.LabelCaseList[0] == LabelCase.UpperCase) ? "Yes" : "no") + "\"");
                RetVal.Append("]" + ",");

                RetVal.Append("\"" + "label2" + "\":");
                RetVal.Append("[");
                RetVal.Append("\"" + diMap.LevelFontSettingList[1].FontName + "\"" + ",");
                RetVal.Append("\"" + diMap.LevelFontSettingList[1].FontSize + "\"" + ",");
                RetVal.Append("\"" + diMap.LevelFontSettingList[1].Font().Bold.ToString().ToLower() + "\"" + ",");
                RetVal.Append("\"" + diMap.LevelFontSettingList[1].Font().Italic.ToString().ToLower() + "\"" + ",");
                RetVal.Append("\"" + diMap.LevelFontSettingList[1].Font().Underline.ToString().ToLower() + "\"" + ",");
                RetVal.Append("\"" + Global.GetHexColorCode(diMap.LevelFontSettingList[1].ForeColor) + "\"" + ",");
                RetVal.Append("\"" + ((diMap.LabelCaseList[1] == LabelCase.UpperCase) ? "Yes" : "no") + "\"");
                RetVal.Append("]" + ",");

                RetVal.Append("\"" + "showEffect" + "\":" + "\"" + diMap.ShowLabelEffects.ToString().ToLower() + "\"" + ",");
                RetVal.Append("\"" + "effectType" + "\":" + "\"" + diMap.LabelEffectSettings.Effect.ToString() + "\"" + ",");
                RetVal.Append("\"" + "effectDepth" + "\":" + "\"" + diMap.LabelEffectSettings.Depth + "\"" + ",");
                RetVal.Append("\"" + "effectColor" + "\":" + "\"" + Global.GetHexColorCode(diMap.LabelEffectSettings.EffectColor) + "\"" + ",");

                RetVal.Append("\"" + "showLeader" + "\":" + "\"" + diMap.LeaderVisible.ToString().ToLower() + "\"" + ",");
                RetVal.Append("\"" + "straightLine" + "\":" + "\"" + ((diMap.AngledNudgeLdrLine == true) ? "Yes" : "no") + "\"" + ",");
                RetVal.Append("\"" + "lineStyle" + "\":" + "\"" + ((diMap.ShowPointStartingLdrLine == true) ? "DOT" : "") + (diMap.LeaderStyle) + "\"" + ",");
                RetVal.Append("\"" + "lineWidth" + "\":" + "\"" + diMap.LeaderWidth + "\"" + ",");
                RetVal.Append("\"" + "lineColor" + "\":" + "\"" + Global.GetHexColorCode(diMap.LeaderColor) + "\"");

                RetVal.Append("}");
            }
            else
            {
                RetVal.Append("false");
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetDataLabelsJSON ->" + ex.Message);
            RetVal.Append("false" + Constants.Delimiters.ParamDelimiter + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal.ToString();
    }

    public string SetDataLabels(string param, string param2)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        string[] paramValues;
        CustomLabel customLabel = null;
        bool dataLabelVisible = false;
        string labelFields = string.Empty;
        bool labelMultirow = false;
        bool labelIndented = false;
        bool showLabelWhereDataExists = false;
        bool showLabelEffects = false;
        LabelEffect effectType;
        int effectDepth = 0;
        Color effectColor;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            paramValues = Global.SplitString(param, Constants.Delimiters.ParamDelimiter);

            dataLabelVisible = Convert.ToBoolean(paramValues[0]);
            labelFields = paramValues[1].Trim(',');
            labelMultirow = Convert.ToBoolean(paramValues[2]);
            labelIndented = Convert.ToBoolean(paramValues[3]);
            showLabelWhereDataExists = Convert.ToBoolean(paramValues[4]);
            showLabelEffects = Convert.ToBoolean(paramValues[5]);
            effectType = (LabelEffect)Enum.Parse(typeof(LabelEffect), paramValues[6]);
            effectDepth = Convert.ToInt32(paramValues[7]);
            effectColor = ColorTranslator.FromHtml(paramValues[8]);

            diMap.ShowLabelWhereDataExists = showLabelWhereDataExists;
            diMap.ShowLabelEffects = showLabelEffects;
            diMap.LabelEffectSettings.Effect = effectType;
            diMap.LabelEffectSettings.Depth = effectDepth;
            diMap.LabelEffectSettings.EffectColor = effectColor;
            diMap.MultiRowLabel = labelMultirow;

            //Set data labels to true false
            if (diMap.Layers != null && diMap.Layers.Count > 0)
            {
                foreach (Layer layer in diMap.Layers)
                {
                    //special comment label field [0 show AreaId] [1 show AreaName default] [2 show DisplayInfo] [3 show Unitname] [4 show subgroup] [5 show TimePeriod] 
                    layer.LabelVisible = dataLabelVisible;
                    layer.LabelField = labelFields;
                    layer.LabelMultirow = labelMultirow;
                    layer.LabelIndented = labelIndented;
                    layer.ShowLabelEffects = diMap.ShowLabelEffects;
                    layer.LabelEffectSettings = diMap.LabelEffectSettings;

                    foreach (DictionaryEntry Key in layer.ModifiedLabels)
                    {
                        customLabel = ((CustomLabel)Key.Value);
                        customLabel.Visible = Convert.ToBoolean(dataLabelVisible);
                        customLabel.LabelField = labelFields;
                        customLabel.MultiRow = Convert.ToBoolean(labelMultirow);
                        customLabel.Indent = Convert.ToBoolean(labelIndented);
                    }
                }

                //add map into  session variable
                Session["DIMap"] = diMap;

                if (!string.IsNullOrEmpty(param2))
                {
                    this.SetDataLabelInfo(param2, false);
                }

                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));
            }
            else
            {
                RetVal = "false";
            }

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetDataLabels->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetDataLabels()
    {
        string RetVal = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            if (diMap != null && diMap.Layers.Count > 0)
            {
                Layer layer = diMap.Layers[0];
                RetVal = layer.LabelVisible.ToString().ToLower() + Constants.Delimiters.ParamDelimiter
                        + layer.LabelField + Constants.Delimiters.ParamDelimiter
                        + layer.LabelMultirow.ToString().ToLower() + Constants.Delimiters.ParamDelimiter
                        + layer.LabelIndented.ToString().ToLower() + Constants.Delimiters.ParamDelimiter
                        + diMap.ShowLabelWhereDataExists.ToString().ToLower() + Constants.Delimiters.ParamDelimiter
                        + diMap.ShowLabelEffects.ToString().ToLower() + Constants.Delimiters.ParamDelimiter
                        + diMap.LabelEffectSettings.Effect + Constants.Delimiters.ParamDelimiter
                        + diMap.LabelEffectSettings.Depth + Constants.Delimiters.ParamDelimiter
                        + Global.GetHexColorCode(diMap.LabelEffectSettings.EffectColor);

                //Data label information
                RetVal += this.GetDataLabelInfo();

            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetDataLabels ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetDataLabelInfo()
    {
        string RetVal = string.Empty;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".png");
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        string MapFolder = string.Empty;
        Map diMap = null;

        try
        {

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            RetVal = Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.LabelFontSetting.FontTemplate.FontSize + Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.LabelFontSetting.FontTemplate.Font().Bold.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.LabelFontSetting.FontTemplate.Font().Italic.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.LabelFontSetting.FontTemplate.Font().Underline.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                Global.GetHexColorCode(diMap.TemplateStyle.LabelFontSetting.FontTemplate.ForeColor) + Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.LabelFontSetting.FontTemplate.FontName;

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetDataLabelInfo->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// Label level information for level 1 label or level 2 label
    /// </summary>
    /// <param name="LabelLevel">1 or 2 in Integer</param>
    /// <returns></returns>
    public string GetDataLabelInfo(int LabelLevel)
    {
        string RetVal = string.Empty;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".png");
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        string MapFolder = string.Empty;
        Map diMap = null;

        FontSetting fontSetting = null;

        try
        {

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            if (LabelLevel == 1)
                fontSetting = diMap.LevelFontSettingList[0];
            else
                fontSetting = diMap.LevelFontSettingList[1];

            RetVal = Constants.Delimiters.ParamDelimiter +
                fontSetting.FontSize + Constants.Delimiters.ParamDelimiter +
                fontSetting.Font().Bold.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                fontSetting.Font().Italic.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                fontSetting.Font().Underline.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                Global.GetHexColorCode(fontSetting.ForeColor) + Constants.Delimiters.ParamDelimiter +
                fontSetting.FontName;

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetDataLabelInfo->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetLegendInfo(string titleOrBody)
    {
        string RetVal = string.Empty;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".png");
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        string MapFolder = string.Empty;
        Map diMap = null;

        try
        {

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            Theme theme = diMap.Themes[0];
            if (titleOrBody == "title")
            {

                RetVal = Constants.Delimiters.ParamDelimiter +
                    diMap.TemplateStyle.Legends.FontTemplate.FontSize + Constants.Delimiters.ParamDelimiter +
                    diMap.TemplateStyle.Legends.FontTemplate.Font().Bold.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                    diMap.TemplateStyle.Legends.FontTemplate.Font().Italic.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                    diMap.TemplateStyle.Legends.FontTemplate.Font().Underline.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                    Global.GetHexColorCode(diMap.TemplateStyle.Legends.FontTemplate.ForeColor);
            }
            else if (titleOrBody == "body")
            {
                RetVal = Constants.Delimiters.ParamDelimiter +
                       diMap.TemplateStyle.LegendBody.FontTemplate.FontSize + Constants.Delimiters.ParamDelimiter +
                       diMap.TemplateStyle.LegendBody.FontTemplate.Font().Bold.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                       diMap.TemplateStyle.LegendBody.FontTemplate.Font().Italic.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                       diMap.TemplateStyle.LegendBody.FontTemplate.Font().Underline.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                       Global.GetHexColorCode(diMap.TemplateStyle.LegendBody.FontTemplate.ForeColor);
            }
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetLegendInfo->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string SetLegendInfo(string titleOrBody, string paramValues)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        string[] param;
        int FontSize = 0;
        FontStyle fontStyle;
        Color ForeColor;
        Color LegendColor;
        Font LegendFont;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            param = Global.SplitString(paramValues, Constants.Delimiters.ParamDelimiter);

            if (titleOrBody == "title")
            {
                FontSize = Convert.ToInt32(param[1]);
                fontStyle = this.GetFontStyle(Convert.ToBoolean(param[2]), Convert.ToBoolean(param[3]), Convert.ToBoolean(param[4]));
                ForeColor = ColorTranslator.FromHtml(param[5]);

                diMap.TemplateStyle.Legends.FontTemplate.FontSize = FontSize;
                diMap.TemplateStyle.Legends.FontTemplate.FontStyle = fontStyle;
                diMap.TemplateStyle.Legends.FontTemplate.ForeColor = ForeColor;

                foreach (Theme theme in diMap.Themes)
                {
                    LegendColor = ColorTranslator.FromHtml(param[5]);
                    LegendFont = new Font(theme.LegendFont.Name, float.Parse(param[1]), this.GetFontStyle(Convert.ToBoolean(param[2]), Convert.ToBoolean(param[3]), Convert.ToBoolean(param[4])));

                    theme.LegendColor = LegendColor;
                    theme.LegendFont = LegendFont;
                }
            }
            else if (titleOrBody == "body")
            {
                FontSize = Convert.ToInt32(param[1]);
                fontStyle = this.GetFontStyle(Convert.ToBoolean(param[2]), Convert.ToBoolean(param[3]), Convert.ToBoolean(param[4]));
                ForeColor = ColorTranslator.FromHtml(param[5]);

                diMap.TemplateStyle.LegendBody.FontTemplate.FontSize = FontSize;
                diMap.TemplateStyle.LegendBody.FontTemplate.FontStyle = fontStyle;
                diMap.TemplateStyle.LegendBody.FontTemplate.ForeColor = ForeColor;

                foreach (Theme theme in diMap.Themes)
                {
                    LegendColor = ColorTranslator.FromHtml(param[5]);
                    LegendFont = new Font(theme.LegendFont.Name, float.Parse(param[1]), this.GetFontStyle(Convert.ToBoolean(param[2]), Convert.ToBoolean(param[3]), Convert.ToBoolean(param[4])));

                    theme.LegendBodyColor = LegendColor;
                    theme.LegendBodyFont = LegendFont;
                }
            }

            RetVal = "true";

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetLegendInfo->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;

    }

    public string SetDataLabelInfo(string MapDisclaimerInfo, bool isGetMapImageURL)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        string[] param;
        CustomLabel customLabel = null;

        int fontSize = 0;
        FontStyle fontStyle;
        Color foreColor;
        Color labelColor;
        Font LabelFont;
        string fontName = string.Empty;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            param = Global.SplitString(MapDisclaimerInfo, Constants.Delimiters.ParamDelimiter);

            fontSize = Convert.ToInt32(param[1]);
            fontStyle = this.GetFontStyle(Convert.ToBoolean(param[2]), Convert.ToBoolean(param[3]), Convert.ToBoolean(param[4]));
            foreColor = ColorTranslator.FromHtml(param[5]);

            diMap.TemplateStyle.LabelFontSetting.FontTemplate.FontSize = fontSize;
            diMap.TemplateStyle.LabelFontSetting.FontTemplate.FontStyle = fontStyle;
            diMap.TemplateStyle.LabelFontSetting.FontTemplate.ForeColor = foreColor;

            if (param.Length >= 7 && !string.IsNullOrEmpty(param[6]))
            {
                fontName = param[6];
                diMap.TemplateStyle.LabelFontSetting.FontTemplate.FontName = param[6];
            }

            foreach (Layer layer in diMap.Layers)
            {
                if (string.IsNullOrEmpty(fontName))
                {
                    fontName = layer.LabelFont.Name;
                }

                labelColor = ColorTranslator.FromHtml(param[5]);
                LabelFont = new Font(fontName, float.Parse(param[1]), this.GetFontStyle(Convert.ToBoolean(param[2]), Convert.ToBoolean(param[3]), Convert.ToBoolean(param[4])));

                layer.LabelColor = labelColor;
                layer.LabelFont = LabelFont;

                foreach (DictionaryEntry Key in layer.ModifiedLabels)
                {
                    customLabel = ((CustomLabel)Key.Value);
                    customLabel.ForeColor = layer.LabelColor;
                    customLabel.LabelFont = layer.LabelFont;
                }
            }

            if (isGetMapImageURL)
            {
                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));
            }
            else
            {
                RetVal = "true";
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetDataLabelInfo->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string SetDataLabelInfo(string MapDisclaimerInfo, bool isGetMapImageURL, int LabelLevel)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        string[] param;
        CustomLabel customLabel = null;

        int fontSize = 0;
        FontStyle fontStyle;
        Color foreColor;
        Color labelColor;
        Font LabelFont;
        string fontName = string.Empty;

        FontSetting fontSetting = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            param = Global.SplitString(MapDisclaimerInfo, Constants.Delimiters.ParamDelimiter);

            fontSize = Convert.ToInt32(param[1]);
            fontStyle = this.GetFontStyle(Convert.ToBoolean(param[2]), Convert.ToBoolean(param[3]), Convert.ToBoolean(param[4]));
            foreColor = ColorTranslator.FromHtml(param[5]);

            if (LabelLevel == 1)
                fontSetting = diMap.LevelFontSettingList[0];
            else
                fontSetting = diMap.LevelFontSettingList[1];

            fontSetting.FontSize = fontSize;
            fontSetting.FontStyle = fontStyle;
            fontSetting.ForeColor = foreColor;

            if (param.Length >= 7 && !string.IsNullOrEmpty(param[6]))
            {
                fontName = param[6];
                fontSetting.FontName = param[6];
            }

            foreach (Layer layer in diMap.Layers)
            {
                if (string.IsNullOrEmpty(fontName))
                {
                    fontName = layer.LabelFont.Name;
                }

                labelColor = ColorTranslator.FromHtml(param[5]);
                LabelFont = new Font(fontName, float.Parse(param[1]), this.GetFontStyle(Convert.ToBoolean(param[2]), Convert.ToBoolean(param[3]), Convert.ToBoolean(param[4])));

                layer.LabelColor = labelColor;
                layer.LabelFont = LabelFont;

                foreach (DictionaryEntry Key in layer.ModifiedLabels)
                {
                    customLabel = ((CustomLabel)Key.Value);
                    customLabel.ForeColor = layer.LabelColor;
                    customLabel.LabelFont = layer.LabelFont;
                }
            }

            if (LabelLevel == 1)
                diMap.LevelFontSettingList[0] = fontSetting;
            else
                diMap.LevelFontSettingList[1] = fontSetting;

            diMap.TemplateStyle.LabelFontSetting.FontTemplate = fontSetting;

            if (isGetMapImageURL)
            {
                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));
            }
            else
            {
                RetVal = "true";
            }

            //add map into  session variable
            Session["DIMap"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetDataLabelInfo->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #region "-- Template Information Title/Disclaimer--"

    public string GetTitleInfo()
    {
        string RetVal = string.Empty;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".png");
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        string MapFolder = string.Empty;
        Map diMap = null;

        try
        {

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            RetVal = diMap.Title + Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.TitleSetting.FontTemplate.FontSize + Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.TitleSetting.FontTemplate.Font().Bold.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.TitleSetting.FontTemplate.Font().Italic.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.TitleSetting.FontTemplate.Font().Underline.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                Global.GetHexColorCode(diMap.TemplateStyle.TitleSetting.FontTemplate.ForeColor);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetTitleInfo->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetDisclaimerInfo()
    {
        string RetVal = string.Empty;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".png");
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        string MapFolder = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            RetVal = diMap.LanguageStrings.DisclaimerText + Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.DisclaimerFont.FontTemplate.FontSize + Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.DisclaimerFont.FontTemplate.Font().Bold.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.DisclaimerFont.FontTemplate.Font().Italic.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                diMap.TemplateStyle.DisclaimerFont.FontTemplate.Font().Underline.ToString().ToLower() + Constants.Delimiters.ParamDelimiter +
                Global.GetHexColorCode(diMap.TemplateStyle.DisclaimerFont.FontTemplate.ForeColor);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetDisclaimerInfo->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string SetTitleInfo(string MapInfo)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        string[] param;
        string MapTitle = string.Empty;

        int fontSize = 0;
        FontStyle fontStyle;
        Color foreColor;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            param = Global.SplitString(MapInfo, Constants.Delimiters.ParamDelimiter);
            MapTitle = param[0];
            fontSize = Convert.ToInt32(param[1]);
            fontStyle = this.GetFontStyle(Convert.ToBoolean(param[2]), Convert.ToBoolean(param[3]), Convert.ToBoolean(param[4]));
            foreColor = ColorTranslator.FromHtml(param[5]);

            diMap.Title = MapTitle;
            diMap.TemplateStyle.TitleSetting.FontTemplate.FontSize = fontSize;
            diMap.TemplateStyle.TitleSetting.FontTemplate.FontStyle = fontStyle;
            diMap.TemplateStyle.TitleSetting.FontTemplate.ForeColor = foreColor;

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = "true";

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetTitleInfo->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string SetDisclaimerInfo(string MapDisclaimerInfo)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        string[] param;
        string DisclaimerText = string.Empty;
        int fontSize = 0;
        FontStyle fontStyle;
        Color foreColor;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            param = Global.SplitString(MapDisclaimerInfo, Constants.Delimiters.ParamDelimiter);
            DisclaimerText = param[0];
            fontSize = Convert.ToInt32(param[1]);
            fontStyle = this.GetFontStyle(Convert.ToBoolean(param[2]), Convert.ToBoolean(param[3]), Convert.ToBoolean(param[4]));
            foreColor = ColorTranslator.FromHtml(param[5]);

            diMap.LanguageStrings.DisclaimerText = DisclaimerText;
            diMap.TemplateStyle.DisclaimerFont.FontTemplate.FontSize = fontSize;
            diMap.TemplateStyle.DisclaimerFont.FontTemplate.FontStyle = fontStyle;
            diMap.TemplateStyle.DisclaimerFont.FontTemplate.ForeColor = foreColor;

            //add map into  session variable
            Session["DIMap"] = diMap;

            RetVal = "true";

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SetDisclaimerInfo->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #region "-- Map Image URL Refresh --"

    public string RefreshImage()
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From RefreshImage ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    /// <summary>
    /// Return http map image URL
    /// </summary>
    /// <returns>http URL of PNG map image </returns>
    public string GetMapImageURL()
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;
        string ShareMapFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.ShareMap, System.Guid.NewGuid() + ".png");
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        Theme CurrentTheme = null;
        string MissingLegendTitle = string.Empty;
        MemoryStream tStream = new MemoryStream();
        float ActualHeight = 0;
        float Actualwidth = 0;
        bool includeLegend = true;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString());

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            ActualHeight = diMap.Height;
            Actualwidth = diMap.Width;

            CurrentTheme = diMap.Themes.GetActiveTheme();

            MissingLegendTitle = CurrentTheme.Legends[CurrentTheme.Legends.Count - 1].Title;
            CurrentTheme.Legends[CurrentTheme.Legends.Count - 1].Title = string.Empty;

            //diMap.DrawMap(TempFileWPath, null);
            if (!Directory.Exists(Path.GetDirectoryName(ShareMapFileWPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(ShareMapFileWPath));
            }

            //diMap.GetCompositeMapImage(Path.GetDirectoryName(ShareMapFileWPath), Path.GetFileNameWithoutExtension(ShareMapFileWPath), "png", Path.GetDirectoryName(ShareMapFileWPath), false, string.Empty, true, true);
            diMap.GetCompositeMapImage(tStream, "png", Path.GetDirectoryName(TempFileWPath), false, string.Empty, includeLegend, true, Actualwidth, ActualHeight);

            this.SaveMemoryStreamIntoFile(tStream, ShareMapFileWPath);
            RetVal = Path.Combine(AbsoluteTempFile.Substring(0, AbsoluteTempFile.LastIndexOf("libraries")), ShareMapFileWPath.Substring(ShareMapFileWPath.LastIndexOf("stock"))).Replace("\\", "/");

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetMapImageURL ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            if (diMap != null)
            {
                diMap.Height = ActualHeight;
                diMap.Width = Actualwidth;
            }

            if (tStream != null)
            {
                tStream.Flush();
                tStream.Close();
            }

            if (CurrentTheme != null || CurrentTheme.Legends != null)
            {
                //reset legend setting after export
                CurrentTheme.Legends[CurrentTheme.Legends.Count - 1].Title = MissingLegendTitle;
            }
        }

        return RetVal;
    }

    public string GetKMZ()//string Height, string Width)
    {
        string RetVal = string.Empty;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString());
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        string MapFolder = string.Empty;
        Map diMap = null;
        string Keyword = string.Empty;
        string MissingLegendTitle = string.Empty;
        MemoryStream tStream = new MemoryStream();
        bool includeLegend = false;
        //float OrigionalHeight = 0, OrigionalWidth = 0;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //OrigionalHeight = diMap.Height;
            //OrigionalWidth = diMap.Width;
            //if (!string.IsNullOrEmpty(Height) && Convert.ToInt32(Height) > 0)
            //    diMap.Height = float.Parse(Height);
            //if (!string.IsNullOrEmpty(Width) && Convert.ToInt32(Width) > 0)
            //    diMap.Width = float.Parse(Width);

            diMap.Save(tStream, TempFileWPath + ".kmz", GoogleMapFileType.KMZ, includeLegend, Path.GetDirectoryName(TempFileWPath));
            this.SaveMemoryStreamIntoFile(tStream, TempFileWPath + ".kmz");

            //IN 2d case append http://
            if (!AbsoluteTempFile.ToLower().StartsWith("http://"))
            {
                AbsoluteTempFile = "http://" + AbsoluteTempFile;
            }

            RetVal = Path.Combine(AbsoluteTempFile.Substring(0, AbsoluteTempFile.LastIndexOf("libraries")), TempFileWPath.Substring(TempFileWPath.LastIndexOf("stock"))).Replace("\\", "/") + ".kmz";
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetKMZ ->" + ex.Message);
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            //diMap.Height = OrigionalHeight;
            //diMap.Width = OrigionalWidth;
        }
        return RetVal;
    }

    public string DeleteFile(string fileURL)
    {
        string RetVal = string.Empty;
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString());
        string PhyAppTempFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "Stock");
        string FileNameWithPath = string.Empty;

        FileNameWithPath = PhyAppTempFile + fileURL.Substring(TempFileWPath.LastIndexOf("stock")).Replace("/", "\\");
        this.deleteFiles(FileNameWithPath);
        return RetVal;
    }

    #endregion

    #region "-- Resize --"

    public string Resize(string width, string height)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            //apply Resize on map
            diMap.Width = (float)Convert.ToDecimal(width);
            diMap.Height = (float)Convert.ToDecimal(height);

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + ".xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From Resize ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #region "-- Save Map --"

    /// <summary>
    /// save KML at given location 
    /// </summary>
    /// <param name="FolderNameWPath"></param>
    /// <param name="FileName"></param>
    /// <returns></returns>
    public string SaveKMLMap(string FolderNameWPath, string FileName)
    {
        string RetVal = string.Empty;
        MemoryStream tStream = new MemoryStream();
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".xml");
        string FileWPathToSave = Path.Combine(FolderNameWPath, FileName + ".kmz");
        Map diMap = null;
        Themes themes = null;
        string MissingLegendTitle = string.Empty;
        bool includeLegend = false;

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

            this.SaveMemoryStreamIntoFile(tStream, FileWPathToSave);
            RetVal = "true";
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SaveKMLMap : ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            if (tStream != null)
            {
                tStream.Flush();
                tStream.Close();
            }
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
        return RetVal;
    }

    /// <summary>
    /// save png at given location 
    /// </summary>
    /// <param name="FolderNameWPath"></param>
    /// <param name="FileName"></param>
    /// <param name="includeLegend"></param>
    /// <param name="includeTitle"></param>
    /// <param name="height"></param>
    /// <param name="width"></param>
    /// <returns></returns>
    public string SavePngMap(string FolderNameWPath, string FileName, bool includeLegend, bool includeTitle, float height, float width)
    {
        string RetVal = string.Empty;
        MemoryStream tStream = new MemoryStream();
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".xml");
        string FileWPathToSave = Path.Combine(FolderNameWPath, FileName + ".png");
        Map diMap = null;
        Themes themes = null;
        string MissingLegendTitle = string.Empty;
        float ActualHeight = 0;
        float Actualwidth = 0;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            ActualHeight = diMap.Height;
            Actualwidth = diMap.Width;

            diMap.Height = height;
            diMap.Width = width;

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

            if (FileName.EndsWith("_t"))
            {
                diMap.GetMapImage(tStream, "png", false);
            }
            else
            {
                diMap.GetCompositeMapImage(tStream, "png", Path.GetDirectoryName(TempFileWPath), false, string.Empty, includeLegend, includeTitle, width, height);
            }

            this.SaveMemoryStreamIntoFile(tStream, FileWPathToSave);
            RetVal = "true";
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SavePngMap ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            if (diMap != null)
            {
                diMap.Height = ActualHeight;
                diMap.Width = Actualwidth;
            }

            if (tStream != null)
            {
                tStream.Flush();
                tStream.Close();
            }
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
        return RetVal;
    }

    /// <summary>
    /// save excel at given location
    /// </summary>
    /// <param name="FolderNameWPath"></param>
    /// <param name="FileName"></param>
    /// <param name="dbNid"></param>
    /// <param name="langCode"></param>
    /// <returns></returns>
    public string SaveExcelMap(string FolderNameWPath, string FileName)
    {
        string RetVal = string.Empty;
        MemoryStream tStream = new MemoryStream();
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV);
        string FileWPathToSave = Path.Combine(FolderNameWPath, FileName + ".xls");
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

            diMap.GeneratePresentation(tStream, TempFileWPath, "emf");
            this.SaveMemoryStreamIntoFile(tStream, FileWPathToSave);
            RetVal = "true";
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SaveExcelMap : ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            if (tStream != null)
            {
                tStream.Flush();
                tStream.Close();
            }

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

        return RetVal;
    }

    /// <summary>
    /// serialize map at given location
    /// </summary>
    /// <param name="FolderNameWPath"></param>
    /// <param name="FileName"></param>
    /// <returns></returns>
    public string SaveMap(string FolderNameWPath, string FileName)
    {
        string RetVal = string.Empty;
        string FileWPathToSave = Path.Combine(FolderNameWPath, FileName + ".xml");
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSessionMapObject();

            this.SerializeObject(FileWPathToSave, diMap);

            RetVal = "true";
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From SaveMap : ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    public string GetSavedMapImage(string FolderNameWPath, string FileName, bool includeLegend, bool includeTitle, float height, float width)
    {
        string RetVal = string.Empty;
        MemoryStream tStream = new MemoryStream();
        string TempFileWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV, DateTime.Now.Ticks.ToString() + ".xml");
        string FileWPathToSave = Path.Combine(FolderNameWPath, FileName);
        string AbsoluteTempFile = HttpContext.Current.Request.Url.AbsoluteUri;
        Map diMap = null;
        Themes themes = null;
        string MissingLegendTitle = string.Empty;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = Map.Load(FileWPathToSave + ".xml");
            height = diMap.Height;
            width = diMap.Width;

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

            diMap.GetCompositeMapImage(tStream, ".png", Path.GetDirectoryName(TempFileWPath), false, string.Empty, includeLegend, includeTitle, width, height);

            this.SaveMemoryStreamIntoFile(tStream, FileWPathToSave + ".png");
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From GetSavedMapImage : ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
            if (tStream != null)
            {
                tStream.Flush();
                tStream.Close();
            }
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
        return RetVal;
    }

    #endregion

    #region "-- Initiaize map --"

    //TODO get information for Dataview mode Browse by .., QDS, MyData
    /// <summary>
    /// 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="dbNId"></param>
    /// <param name="langCode"></param>
    /// <param name="mapTheme">single/multiple</param>
    /// <returns></returns>
    public string InitiaizeMap(string width, string height, string dbNId, string langCode, string mapTheme)
    {
        string RetVal = string.Empty;

        RetVal = this.InitiaizeMap(width, height, dbNId, langCode, mapTheme, false.ToString());

        return RetVal;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="dbNId"></param>
    /// <param name="langCode"></param>
    /// <param name="mapTheme">single/multiple</param>
    /// <param name="useMapServer">true/false</param>
    /// <returns></returns>
    public string InitiaizeMap(string width, string height, string dbNId, string langCode, string mapTheme, string useMapServer)
    {
        string RetVal = string.Empty;
        float MapWidth, MapHeight;
        bool isSingleThemeMap = false;
        //bool UseMapServer = false;

        try
        {
            bool UseMapServer = Convert.ToBoolean(useMapServer);
            MapWidth = (float)Convert.ToDecimal(width);
            MapHeight = (float)Convert.ToDecimal(height);

            if (string.IsNullOrEmpty(dbNId))
            {
                dbNId = Global.GetDefaultDbNId();
            }
            if (string.IsNullOrEmpty(langCode))
            {
                langCode = Global.GetDefaultLanguageCode();
            }

            //TODO test [intentionaly session dimap is set to null] 
            if (Session["DataViewNonQDS"] == null)
            {
                Session["DIMap"] = null;
            }

            if (mapTheme != null && mapTheme == "single")
            {
                isSingleThemeMap = true;
                Session["DIMap"] = null;
            }

            this.SetTimeLogIntoFile("---------------------" + "Date: " + DateTime.Now.Date.ToLongDateString() + "    Time: " + DateTime.Now.ToLongTimeString() + "--------------------");

            this.SetTimeLogIntoFile("Starts Deleting files");
            var t = System.Threading.Tasks.Task.Factory.StartNew(() => DeleteAllFilesFromTemp());

            this.SetTimeLogIntoFile("Starts InitiaizeMap");
            RetVal = this.InitializeBaseMap(MapWidth, MapHeight, dbNId, langCode, isSingleThemeMap, UseMapServer);

        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From InitiaizeMap hdbnid : " + dbNId + "->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    //Call very first time if map not created but want to see 2d map
    public string InitiaizeGoogleMap(string width, string height, string dbNId, string langCode)
    {
        string RetVal = string.Empty;
        float MapWidth, MapHeight;
        string LangFileNameWPath = string.Empty;
        bool UseMapServer = false;

        try
        {
            MapWidth = (float)Convert.ToDecimal(width);
            MapHeight = (float)Convert.ToDecimal(height);
            LangFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.BaseLanguageFile]);

            if (string.IsNullOrEmpty(dbNId))
            {
                dbNId = Global.GetDefaultDbNId();
            }
            if (string.IsNullOrEmpty(langCode))
            {
                langCode = Global.GetDefaultLanguageCode();
            }

            //TODO test [intentionaly session dimap is set to null] 
            if (Session["DataViewNonQDS"] == null)
            {
                Session["DIMap"] = null;
            }

            this.InitializeBaseMap(MapWidth, MapHeight, dbNId, langCode, false, UseMapServer);
        }
        catch (Exception ex)
        {
            //Global.WriteErrorsInLog("From InitiaizeGoogleMap hdbnid : " + dbNId + "->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #endregion

    #endregion

}
