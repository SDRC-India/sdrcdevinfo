using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DevInfo.Lib.DI_LibMap;
using System.IO;
using System.Drawing;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using System.Drawing.Imaging;
using Microsoft.VisualBasic;
using System.Globalization;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Data.Common;
using System.Xml.Serialization;
using System.Web.UI;
using System.Collections;

/// <summary>
/// Summary description for MapSelectorCallback
/// </summary>
public partial class MapSelector : System.Web.UI.Page
{
    #region "--Private--"

    #region "--Variable--"
    /// <summary>
    /// Default start date "1/1/1800" for chronical representivity of map file in en-US culture format (MM/DD/YYYY) 
    /// </summary>
    public const string DEFAULT_START_DATE = "1/1/1800";

    /// <summary>
    /// Default end date "12/31/3000" for chronical representivity of map file in en-US culture format (MM/DD/YYYY) 
    /// </summary>
    public const string DEFAULT_END_DATE = "12/31/3000"; //12/31/2200 'thai era exceeds 2200 years so extended to 3000

    string TempPath = HttpContext.Current.Request.PhysicalApplicationPath + Constants.FolderName.TempCYV;

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
            Global.CreateExceptionString(ex, null);
            ////Global.WriteErrorsInLog("From SerializeObject Object : " + serializeObj.GetType().ToString() + "->" + " File:" + p_FileNameWPath + " Error: " + ex.Message);
        }
        finally
        {
            _IO.Flush();
            _IO.Close();
        }
    }

    #endregion

    #region "-- Map --"

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
                catch (Exception ex) { }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            ////Global.WriteErrorsInLog("From GetXmlOfSerializeObject Object : " + objToSerialize.GetType().ToString() + "->" + " File:" + TempFileWPath + " Error: " + ex.Message);
            throw;
        }
        return RetVal;
    }

    private string InitializeSelectionMap(float width, float Height, string dbNId, string langCode, int AreaLevel)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        string TemplateStyleFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Template);
        string LanguageFolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Language);
        string MapFolder = string.Empty;
        string DisputedBoundriesFolder = string.Empty;
        Map diMap = null;
        string seriesID = string.Empty;
        string seriesName = string.Empty;
        string FilterText = string.Empty;
        int AreaParentNid = -1;
        try
        {
            //First step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSelectionMapObject();

            MapFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + dbNId + "\\" + Constants.FolderName.Maps);
            DisputedBoundriesFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.DisputedBoundries);

            diMap.Width = width;
            diMap.Height = Height;
            diMap.SpatialMapFolder = MapFolder;
            diMap.DIBFolderPath = DisputedBoundriesFolder;
            diMap.FirstColor = ColorTranslator.FromHtml("#b6effd");
            diMap.FourthColor = ColorTranslator.FromHtml("#1c84a0");
            diMap.LabelEffectSettings = new LabelEffectSetting();
            diMap.TemplateStyle = StyleTemplate.LoadStyleTemplate(Path.Combine(TemplateStyleFolderPath, Constants.Map.MapLanguageFileName));
            diMap.ScaleUnitText = "KM";

            //TODO language based strings           
            diMap.LanguageStrings = this.GetMapLanguageStrings(LanguageFolderPath, langCode);
            if (diMap.LanguageStrings == null)
            {
                diMap.LanguageStrings = new LanguageStrings();
            }

            this.SetByMapLayers(diMap, dbNId, langCode, AreaLevel, AreaParentNid);

            diMap.SetFullExtent();

            diMap.Themes.Clear();

            //add map into  session variable
            Session["DIMapSelection"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + "Selection.xml");
            this.SerializeObject(MapFileWPath, diMap);

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            ////Global.WriteErrorsInLog("From InitializeSelectionMap hdbnid : " + dbNId + "->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }
        finally
        {
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

    private Map GetSelectionMapObject()
    {
        Map RetVal = null;
        string MapFileWPath = string.Empty;

        if (HttpContext.Current.Request.Cookies["SessionID"] == null)
        {
            Global.SaveCookie("SessionID", HttpContext.Current.Session.SessionID, this.Page);
        }

        MapFileWPath = Path.Combine(TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + "Selection.xml");

        if (Session["DIMapSelection"] == null && File.Exists(MapFileWPath))
        {
            RetVal = Map.Load(MapFileWPath);
            Session["DIMapSelection"] = RetVal;
        }
        else
        {
            if (Session["DIMapSelection"] == null)
            {
                RetVal = new Map();
                Session["DIMapSelection"] = RetVal;
            }
            else
            {
                RetVal = (Map)Session["DIMapSelection"];
            }
        }

        return RetVal;
    }

    private List<string> SetByMapLayers(Map diMap, string dbNId, string langCode, int AreaLevel, int AreaParentNid)
    {
        List<string> RetVal = new List<string>();
        DIConnection DBConnection = null;
        DIQueries DBQueries = null;
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
        string selectedAreaIDNids = string.Empty;
        string sTimePeriods = string.Empty;

        //consider only in mydata case only
        List<string> TimePeriods = null;

        DataView dvAreaMapLayerInfo = null;
        string ProcName = string.Empty;
        string RelationShipColumnName = string.Empty;

        DbParameter areaParam1;
        DbParameter areaParam2;
        Layer _Layer = null;

        string mapFolder = string.Empty;

        try
        {
            mapFolder = diMap.SpatialMapFolder;

            if (string.IsNullOrEmpty(dbNId))
            {
                dbNId = Global.GetDefaultDbNId();
            }

            DBConnection = Global.GetDbConnection(Convert.ToInt32(dbNId));
            //TODO Remove usage of DAL queries and use stored procedure instead
            DBQueries = new DIQueries(DBConnection.DIDataSetDefault(), langCode);

            //-- Get the date range for which shapefiles are to be considered
            //-- handle different Reginal Settings - Thai, Arabic etc
            dtStartDate = DateTime.Parse(DEFAULT_START_DATE, ociEnUS).Date;
            dtEndDate = DateTime.Parse(DEFAULT_END_DATE, ociEnUS).Date;

            //TODO Skip time based logic in case of MyData            
            TimePeriods = new List<string>(sTimePeriods.Split(','));
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

            //TO DO by areaLevel
            if (!diMap.isMyData)
            {
                //search data by area NIDs, IDS both in QDS, Browse data case                
                DbParams = new List<DbParameter>();
                ProcName = "sp_get_areaMapLayer_By_Level_" + langCode;
                areaParam1 = DBConnection.CreateDBParameter();
                areaParam1.ParameterName = "AreaLevel";
                areaParam1.DbType = DbType.Int32;
                areaParam1.Value = AreaLevel;

                DbParams = new List<DbParameter>();
                DbParams.Add(areaParam1);

                areaParam2 = DBConnection.CreateDBParameter();
                areaParam2.ParameterName = "AreaParentNId";
                areaParam2.DbType = DbType.Int32;
                areaParam2.Value = AreaParentNid;
                DbParams.Add(areaParam2);

            }
            dvAreaMapLayerInfo = DBConnection.ExecuteDataTable(ProcName, CommandType.StoredProcedure, DbParams).DefaultView;

            //if layers found then only add layers
            if (dvAreaMapLayerInfo.Count > 0)
            {
                //set into areaMap table
                if (dvAreaMapLayerInfo != null)
                {
                    diMap.MapData = dvAreaMapLayerInfo.ToTable(true, Area.AreaNId, Area.AreaID, Area.AreaParentNId, Area.AreaName, Area_Level.AreaLevel, Area.AreaBlock, Area_Map_Metadata.LayerName);
                }

                diMap.Layers.Clear();
                //foreach (Layer layer in diMap.Layers)
                //{
                //    layer.Visible = false;
                //}

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

    private static DataSet GetInvariantDataSet()
    {
        // Set locale of dataset to invariant culture to handle cases for different regional settings
        DataSet RetVal = new DataSet();
        RetVal.Locale = new System.Globalization.CultureInfo("", false);
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
            Global.CreateExceptionString(ex, null);
            //startDate = DateTime.Parse(DEFAULT_START_DATE, ociEnUS);
            //endDate = DateTime.Parse(DEFAULT_END_DATE, ociEnUS);
        }
    }

    private string GetBase64MapImageString(MemoryStream mapStream)
    {
        string RetVal = string.Empty;

        try
        {
            RetVal = "data:image/png;base64," + Convert.ToBase64String(mapStream.ToArray(), Base64FormattingOptions.None);
        }
        catch (Exception)
        {

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
            Global.CreateExceptionString(ex, null);
            ////Global.WriteErrorsInLog("From xml : ->" + ex.Message);
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

            RetVal.DisclaimerText = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, "langMapDisclaimerText"), Constants.Map.XMLValueAtributeName);
            RetVal.MissingValue = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, "langMissingValue"), Constants.Map.XMLValueAtributeName);
            RetVal.Map = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, "langMap"), Constants.Map.XMLValueAtributeName);
            RetVal.Data = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, "langData"), Constants.Map.XMLValueAtributeName);
            RetVal.Source = objxmlFilereader.GetXMLNodeAttributeValue(Constants.Map.MapLangXPath.Replace(Constants.Map.LangReplaceXPathString, "langSource"), Constants.Map.XMLValueAtributeName);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = null;
        }

        return RetVal;
    }

    #endregion

    #endregion

    #endregion

    #region "--Public--"

    #region "--Constructors--"

    public MapSelector(Page OPage)
    {
        this.Page = OPage;
    }

    #endregion

    #region "--Methods--"

    #region "-- Zoom Panning Full extent--"

    public string ZoomInByMap()
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSelectionMapObject();

            //apply zoom
            diMap.zoom(1.2f);

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMapSelection"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + "Selection.xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }
        finally
        {
        }

        return RetVal;
    }

    public string ZoomOutByMap()
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSelectionMapObject();

            //apply zoom
            diMap.zoom(0.75f, 0, 0);

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMapSelection"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + "Selection.xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }
        finally
        {
        }

        return RetVal;
    }

    public string ZoomToRectangleByMap(string mouseDownX, string mouseDownY, string x, string y)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSelectionMapObject();

            //apply zoom
            diMap.zoom(Convert.ToInt32(mouseDownX), Convert.ToInt32(mouseDownY), Convert.ToInt32(x), Convert.ToInt32(y));

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMapSelection"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + "Selection.xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }
        finally
        {
        }
        return RetVal;
    }

    public string PanByMap(string pX1, string pY1, string pX2, string pY2)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSelectionMapObject();

            //apply panning
            diMap.Pan(int.Parse(pX1), int.Parse(pY1), int.Parse(pX2), int.Parse(pY2));

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMapSelection"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + "Selection.xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }
        finally
        {
        }

        return RetVal;
    }

    public string FullExtentByMap()
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSelectionMapObject();

            diMap.SetFullExtent();

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMapSelection"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + "Selection.xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }
        finally
        {
        }

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
            diMap = this.GetSelectionMapObject();

            //apply Resize on map
            diMap.Width = (float)Convert.ToDecimal(width);
            diMap.Height = (float)Convert.ToDecimal(height);

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMapSelection"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + "Selection.xml");
            this.SerializeObject(MapFileWPath, diMap);

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #region "-- Label --"

    public string LabelByMap(string dataLabelVisible)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        try
        {
            bool isDataLabelVisible = Convert.ToBoolean(dataLabelVisible);

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSelectionMapObject();

            //Set data labels to true false
            if (diMap.Layers != null && diMap.Layers.Count > 0)
            {
                foreach (Layer layer in diMap.Layers)
                {
                    layer.LabelVisible = isDataLabelVisible;

                    //special comment label field [0 show AreaId] [1 show AreaName default] [2 show DisplayInfo] [3 show Unitname] [4 show subgroup] [5 show TimePeriod] 
                    if (string.IsNullOrEmpty(layer.LabelField))
                    {
                        layer.LabelField = "1";
                    }
                }

                RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

                //add map into  session variable
                Session["DIMapSelection"] = diMap;

                //serialize map in last
                MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + "Selection.xml");
                this.SerializeObject(MapFileWPath, diMap);
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #region "-- Highlight --"

    /// <summary>
    /// 
    /// </summary>
    /// <param name="param1">singleClick/doubleClick</param>
    /// <param name="param2">X,Y</param>
    /// <param name="dbNId"></param>
    /// <param name="langCode"></param>
    /// <returns></returns>
    public string SetMapSelection(string param1, string param2, string dbNId, string langCode)
    {
        string RetVal = string.Empty;
        MemoryStream tStream = new MemoryStream();
        string MapFileWPath = string.Empty;
        Map diMap = null;
        string MissingLegendTitle = string.Empty;
        string[] paramValues;
        int x = 0, y = 0;

        string[] areaInfo;
        DataRow[] areaRows = null;

        int AreaParentNid = -1;
        int AreaLevel = 1;

        try
        {
            paramValues = Global.SplitString(param2, Constants.Delimiters.ParamDelimiter);
            x = Convert.ToInt32(paramValues[0]);
            y = Convert.ToInt32(paramValues[1]);

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSelectionMapObject();

            if (param1 == "single")
            {
                diMap.SetSelection(x, y);
            }
            else if (param1 == "double")
            {
                if (string.IsNullOrEmpty(dbNId))
                {
                    dbNId = Global.GetDefaultDbNId();
                }
                if (string.IsNullOrEmpty(langCode))
                {
                    langCode = Global.GetDefaultLanguageCode();
                }

                //TODO get new map by area
                areaInfo = diMap.GetArea(x, y);
                areaRows = diMap.MapData.Select(Area_Map_Metadata.LayerName + "='" + areaInfo[0] + "'");

                //get add more layers
                if (areaRows.Length > 0)
                {
                    AreaParentNid = Convert.ToInt32(areaRows[0][Area.AreaNId]);
                    AreaLevel = Convert.ToInt32(areaRows[0][Area.AreaLevel]);
                    this.SetByMapLayers(diMap, dbNId, langCode, AreaLevel + 1, AreaParentNid);
                    diMap.ZoomFullExtent();
                }
            }

            RetVal = this.GetBase64MapImageString(this.DrawMap(diMap));

            //add map into  session variable
            Session["DIMapSelection"] = diMap;

            //serialize map in last
            MapFileWPath = Path.Combine(this.TempPath, HttpContext.Current.Request.Cookies["SessionID"].Value + "Selection.xml");
            this.SerializeObject(MapFileWPath, diMap);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            ////Global.WriteErrorsInLog("From SetMapSelection : ->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }
        finally
        {
        }
        return RetVal;
    }

    public string GetAreaSelection(string param1)
    {
        string RetVal = string.Empty;
        string MapFileWPath = string.Empty;
        Map diMap = null;

        string[] paramValues;
        int x = 0, y = 0;
        string[] areaInfo;
        DataRow[] areaRows = null;
        string isSelected = "false";
        try
        {
            paramValues = Global.SplitString(param1, Constants.Delimiters.ParamDelimiter);
            x = Convert.ToInt32(paramValues[0]);
            y = Convert.ToInt32(paramValues[1]);

            //step To load map from session/ NewMap/ From preserved file
            diMap = this.GetSelectionMapObject();

            //0: layerID 1: AreaID 2:AreaName
            areaInfo = diMap.GetArea(x, y);
            areaRows = diMap.MapData.Select(Area_Map_Metadata.LayerName + "='" + areaInfo[0] + "'");

            if (areaRows.Length > 0)
            {
                foreach (Layer Lyr in diMap.Layers)
                {
                    if (Lyr.SelectedArea.Count > 0)
                    {
                        if (Lyr.SelectedArea.Contains(areaRows[0][Area.AreaID].ToString()))
                        {
                            isSelected = "true";
                            break;
                        }
                    }
                }

                RetVal = areaRows[0][Area.AreaNId] + Constants.Delimiters.ParamDelimiter;
                RetVal += areaRows[0][Area.AreaID] + Constants.Delimiters.ParamDelimiter;
                RetVal += areaRows[0][Area.AreaLevel] + Constants.Delimiters.ParamDelimiter;
                RetVal += areaRows[0][Area.AreaName] + Constants.Delimiters.ParamDelimiter;
                RetVal += isSelected;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
        }
        finally
        {
        }

        return RetVal;
    }

    #endregion

    #region "-- Initiaize map --"

    //TODO get information for Dataview mode Browse by .., QDS, MyData
    public string InitializeByMap(string width, string height, string areaLevel, string dbNId, string langCode)
    {
        string RetVal = string.Empty;
        float MapWidth, MapHeight;
        int AreaLevel = 1;
        try
        {
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
            if (!string.IsNullOrEmpty(areaLevel))
            {
                AreaLevel = Convert.ToInt32(areaLevel);
            }

            RetVal = this.InitializeSelectionMap(MapWidth, MapHeight, dbNId, langCode, AreaLevel);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            ////Global.WriteErrorsInLog("From InitializeByMap hdbnid : " + dbNId + "->" + ex.Message);
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
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