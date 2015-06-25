using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using iTextSharp.text;

public partial class Callback : System.Web.UI.Page
{
    #region "--Methods--"

    #region "--Private--"

    #region "--Common--"

    private string NGallery_GetGalleryResultsXML(string IndsText, string AreasText, string keywords, string SearchLanguage, int DBNId, string GalleryItemType, List<string> SearchWords, ref int NumResults, string basePath)
    {
        string RetVal;
        string Indicator, Area;
        string PresNId, PresType, PresTitle, PresDesc, PresModifiedDate, KeywordNIds, SearchedKeyWords, UserNid, ChartType;
        DataTable DtPresentations, DtTemp;
        List<string> UsedPresNIds;
        string[] IndiTextList, AreaTextList, KeywordTextList;

        RetVal = string.Empty;
        DtPresentations = new DataTable();
        UsedPresNIds = new List<string>();
        if (keywords.ToString().ToLower() == "null")
        {
            if (string.IsNullOrEmpty(AreasText) && string.IsNullOrEmpty(IndsText))
            {
                DtTemp = this.GetGalleryThumbnailsTable(string.Empty, string.Empty, SearchLanguage, DBNId, GalleryItemType);

                DtPresentations.Merge(DtTemp);
            }
            if (string.IsNullOrEmpty(AreasText) && !string.IsNullOrEmpty(IndsText))
            {
                IndiTextList = IndsText.Split(new String[] { "||" }, StringSplitOptions.None);
                foreach (string indiStr in IndiTextList)
                {
                    Indicator = DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(indiStr);
                    Area = string.Empty;
                    DtTemp = this.GetGalleryThumbnailsTable(Indicator, Area, SearchLanguage, DBNId, GalleryItemType);
                    DtPresentations.Merge(DtTemp);
                }
            }

            if (string.IsNullOrEmpty(IndsText) && !string.IsNullOrEmpty(AreasText))
            {
                AreaTextList = AreasText.Split(new String[] { "||" }, StringSplitOptions.None);
                foreach (string areaStr in AreaTextList)
                {
                    Area = DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(areaStr);
                    Indicator = string.Empty;
                    DtTemp = this.GetGalleryThumbnailsTable(Indicator, Area, SearchLanguage, DBNId, GalleryItemType);
                    DtPresentations.Merge(DtTemp);
                }
            }

            if (!string.IsNullOrEmpty(IndsText) && !string.IsNullOrEmpty(AreasText))
            {
                IndiTextList = IndsText.Split(new String[] { "||" }, StringSplitOptions.None);
                AreaTextList = AreasText.Split(new String[] { "||" }, StringSplitOptions.None);
                foreach (string indiStr in IndiTextList)
                {
                    //Indicator = indiStr;
                    Indicator = DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(indiStr);
                    foreach (string areaStr in AreaTextList)
                    {
                        Area = DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(areaStr);
                        //Area = areaStr;
                        DtTemp = this.GetGalleryThumbnailsTable(Indicator, Area, SearchLanguage, DBNId, GalleryItemType);
                        DtPresentations.Merge(DtTemp);
                    }
                }
            }
        }
        else
        {
            KeywordTextList = keywords.Split(new String[] { "||" }, StringSplitOptions.None);
            foreach (string keyword in KeywordTextList)
            {
                if (!string.IsNullOrEmpty(keyword))
                {
                    DtTemp = this.GetGalleryTable(keyword, SearchLanguage, DBNId, GalleryItemType);
                    if (DtTemp != null)
                        DtPresentations.Merge(DtTemp);
                }
            }
        }
        foreach (DataRow DrPresentations in DtPresentations.Rows)
        {
            if (UsedPresNIds.Contains(DrPresentations["pres_nid"].ToString()))
            {
                continue;
            }
            else
            {
                UsedPresNIds.Add(DrPresentations["pres_nid"].ToString());
            }

            PresNId = DrPresentations["pres_nid"].ToString();
            PresType = DrPresentations["type"].ToString();
            PresTitle = DrPresentations["pres_name"].ToString();
            PresDesc = DrPresentations["description"].ToString();
            PresDesc = this.Page.Server.HtmlEncode(PresDesc);
            PresModifiedDate = DrPresentations["modified_time"].ToString();
            KeywordNIds = DrPresentations["keyword_nids"].ToString();
            UserNid = DrPresentations["user_nid"].ToString();
            ChartType = DrPresentations["chart_type"].ToString();

            if (KeywordNIds.Length > 2)
            {
                KeywordNIds = KeywordNIds.Substring(1, KeywordNIds.Length - 2);
            }
            //SearchedKeyWords = Global.GetSearchedKeywords(KeywordNIds, SearchWords, Server.MapPath("~//stock//Database.mdb"));
            SearchedKeyWords = "";

            string folderPath = basePath.Substring(basePath.IndexOf("stock"));

            if (Directory.Exists(Server.MapPath("~/" + folderPath + PresNId)) && Directory.GetFiles(Server.MapPath("~/" + folderPath + PresNId)).Length > 0)
            {
                NumResults++;
                RetVal += "<g p=\"" + PresType + "\" s=\"" + PresNId + "/" + PresNId + "_t.png\" sbig=\"" + PresNId + "/" + PresNId + ".png\"";
                RetVal += " shtml=\"" + PresNId + "/" + PresNId + ".html?id=" + PresNId + "&unid=" + UserNid + "\" t=\"" + PresTitle + "\" i=\"" + PresNId + "\" desc=\"" + PresDesc + "\""; RetVal += " md=\"" + PresModifiedDate + "\" k=\"" + SearchWords + "\" kid=\"" + KeywordNIds + "\" vtype= \"" + ChartType + "\" fpath=\"" + basePath + "\" pnid=\"" + PresNId + "\" />";

            }
        }
        return RetVal;
    }
    private DataTable GetGalleryTable(string keyword, string Language, int DBNId, string Type)
    {
        DataTable DTKeyword, RetVal = null;
        DIConnection DIConnection;
        string GetKeywordsQuery, GetPresentationsQuery;
        DataTable DtKeywords;

        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);

            RetVal = new DataTable();
            if (!string.IsNullOrEmpty(keyword))
            {
                GetKeywordsQuery = "SELECT * FROM Keywords WHERE keyword LIKE '%" + keyword.Trim() + "%' AND keyword_type = 'UDK'";
                DtKeywords = DIConnection.ExecuteDataTable(GetKeywordsQuery);
                if (DtKeywords.Rows.Count > 0)
                {
                    foreach (DataRow KeywordRow in DtKeywords.Rows)
                    {
                        GetPresentationsQuery = "SELECT * FROM Presentations WHERE dbnid = '" + DBNId.ToString() + "' AND lng_code = '" + Language + "'";
                        GetPresentationsQuery += " AND keyword_nids LIKE '%," + KeywordRow["keyword_nid"].ToString() + ",%'";
                        if (Type != "A")
                        {
                            GetPresentationsQuery += " AND type = '" + Type + "';";
                        }
                        DTKeyword = DIConnection.ExecuteDataTable(GetPresentationsQuery);
                        RetVal.Merge(DTKeyword);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private string GetSearchedKeywords(string KeywordNIds, List<string> SearchWords)
    {
        string RetVal;
        string GetKeywordsQuery, KeyWord;
        DataTable DtKeywords;
        DIConnection DIConnection;

        RetVal = string.Empty;
        GetKeywordsQuery = "SELECT * FROM keywords WHERE keyword_nid IN (" + KeywordNIds + ") And keyword_type='UDK'";
        DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                       string.Empty, string.Empty);
        try
        {
            DtKeywords = DIConnection.ExecuteDataTable(GetKeywordsQuery);
            if (DtKeywords.Rows.Count > 0)
            {
                foreach (DataRow DrKeyWord in DtKeywords.Rows)
                {
                    KeyWord = DrKeyWord["keyword"].ToString();
                    RetVal += KeyWord + "||";
                    /*if (SearchWords.Contains(KeyWord))
                    {
                        RetVal += KeyWord + "||";
                    }*/
                }
            }
            else
            {
                GetKeywordsQuery = "SELECT * FROM keywords WHERE keyword_nid IN (" + KeywordNIds + ")";
                DtKeywords = DIConnection.ExecuteDataTable(GetKeywordsQuery);
                if (DtKeywords.Rows.Count > 0)
                {
                    foreach (DataRow DrKeyWord in DtKeywords.Rows)
                    {
                        KeyWord = DrKeyWord["keyword"].ToString();
                        RetVal += KeyWord + "||";
                    }
                }
            }

            if (RetVal.Length > 0)
            {
                RetVal = RetVal.Substring(0, RetVal.Length - 2);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private string NGallery_GetMoreKeywordXML(string KeywordNIds, string LangCode)
    {
        string RetVal;
        string Areas, Indicators, UDKs;
        string GetKeywordsQuery;
        DataTable DtKeywords;
        DIConnection DIConnection;

        RetVal = "<results>";
        Areas = string.Empty;
        Indicators = string.Empty;
        UDKs = string.Empty;
        GetKeywordsQuery = "SELECT * FROM keywords WHERE keyword_nid IN (" + KeywordNIds + ")";
        DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                       string.Empty, string.Empty);
        try
        {
            DtKeywords = DIConnection.ExecuteDataTable(GetKeywordsQuery);

            foreach (DataRow DrKeyWord in DtKeywords.Rows)
            {
                switch (DrKeyWord["keyword_type"].ToString())
                {
                    case "A":
                        Areas += DrKeyWord["keyword"].ToString() + ", ";
                        break;
                    case "I":
                        Indicators += DrKeyWord["keyword"].ToString() + ", ";
                        break;
                    case "UDK":
                        UDKs += DrKeyWord["keyword"].ToString() + ", ";
                        break;
                    default:
                        break;
                }
            }

            if (Areas.Length > 0)
            {
                Areas = Areas.Substring(0, Areas.Length - 2);
            }

            if (Indicators.Length > 0)
            {
                Indicators = Indicators.Substring(0, Indicators.Length - 2);
            }

            if (UDKs.Length > 0)
            {
                UDKs = UDKs.Substring(0, UDKs.Length - 2);
            }

            XmlDocument LangSpecificWords;
            LangSpecificWords = null;
            string langAreas = "Areas";
            string langIndicators = "Indicators";
            string langUDKs = "Keywords";

            try
            {
                LangSpecificWords = new XmlDocument();
                LangSpecificWords.Load(Server.MapPath(@"~\stock\language\" + LangCode.ToString() + @"\Gallery.xml"));
                foreach (XmlNode Keyword in LangSpecificWords.GetElementsByTagName("lng"))
                {
                    if (Keyword.Attributes["id"].Value.ToUpper() == "LANGMOREAREAS")
                    {
                        langAreas = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToUpper() == "LANGMOREINDICATORS")
                    {
                        langIndicators = Keyword.Attributes["val"].Value;
                    }
                    if (Keyword.Attributes["id"].Value.ToUpper() == "LANGMOREUDKS")
                    {
                        langUDKs = Keyword.Attributes["val"].Value;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.CreateExceptionString(ex, null);
            }
            //RetVal += "<ar txt=\"Area\" val=\"" + Areas + "\"/>";
            //RetVal += "<ind txt=\"Indicator\" val=\"" + Indicators + "\"/>";
            //RetVal += "<udk txt=\"Keywords\" val=\"" + UDKs + "\"/>";
            RetVal += "<ar txt=\"" + langAreas + "\" val=\"" + Areas + "\"/>";
            RetVal += "<ind txt=\"" + langIndicators + "\" val=\"" + Indicators + "\"/>";
            RetVal += "<udk txt=\"" + langUDKs + "\" val=\"" + UDKs + "\"/>";
            RetVal += "</results>";
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private string NGallery_GetSearchResultsXML(DataTable DtQDSResults, string SearchLanguage, int DBNId, string GalleryItemType, List<string> SearchWords, ref int NumResults, string basePath)
    {
        string RetVal;
        string Indicator, Area;
        string PresNId, PresType, PresTitle, PresDesc, PresModifiedDate, KeywordNIds, SearchedKeyWords, UserNid, ChartType;
        DataTable DtPresentations, DtTemp;
        List<string> UsedPresNIds;

        RetVal = string.Empty;
        DtPresentations = new DataTable();
        UsedPresNIds = new List<string>();

        foreach (DataRow DrQDSResults in DtQDSResults.Rows)
        {
            Indicator = DrQDSResults["Indicator"].ToString();

            if (DtQDSResults.Columns.Contains("Area"))
            {
                Area = DrQDSResults["Area"].ToString();
            }
            else
            {
                Area = string.Empty;
            }

            DtTemp = this.GetGalleryThumbnailsTable(Indicator, Area, SearchLanguage, DBNId, GalleryItemType);

            if (DtPresentations == null)
            {
                DtPresentations = DtTemp.Clone();
                DtPresentations.Merge(DtTemp);
            }
            else
            {
                DtPresentations.Merge(DtTemp);
            }
        }

        foreach (DataRow DrPresentations in DtPresentations.Rows)
        {
            if (UsedPresNIds.Contains(DrPresentations["pres_nid"].ToString()))
            {
                continue;
            }
            else
            {
                UsedPresNIds.Add(DrPresentations["pres_nid"].ToString());
            }

            PresNId = DrPresentations["pres_nid"].ToString();
            PresType = DrPresentations["type"].ToString();
            PresTitle = DrPresentations["pres_name"].ToString();
            PresDesc = DrPresentations["description"].ToString();
            PresModifiedDate = DrPresentations["modified_time"].ToString();
            KeywordNIds = DrPresentations["keyword_nids"].ToString();
            UserNid = DrPresentations["user_nid"].ToString();
            ChartType = DrPresentations["chart_type"].ToString();

            if (KeywordNIds.Length > 0)
            {
                KeywordNIds = KeywordNIds.Substring(1, KeywordNIds.Length - 2);
            }

            //SearchedKeyWords = this.GetSearchedKeywords(KeywordNIds, SearchWords);
            string folderPath = basePath.Substring(basePath.IndexOf("stock"));
            if (Directory.Exists(Server.MapPath("~/" + folderPath + PresNId)))
            {
                NumResults++;
                RetVal += "<g p=\"" + PresType + "\" s=\"" + PresNId + "/" + PresNId + "_t.png\" sbig=\"" + PresNId + "/" + PresNId + ".png\"";
                RetVal += " shtml=\"" + PresNId + "/" + PresNId + ".html?id=" + PresNId + "&unid=" + UserNid + "\" t=\"" + PresTitle + "\" i=\"" + PresNId + "\" desc=\"" + PresDesc + "\""; RetVal += " md=\"" + PresModifiedDate + "\" k=\"" + SearchWords + "\" kid=\"" + KeywordNIds + "\" vtype= \"" + ChartType + "\" fpath=\"" + basePath + "\" pnid=\"" + PresNId + "\" />";
            }
        }

        return RetVal;
    }
    #endregion "--Common--"

    #endregion "--Private--"

    #region "--Public--"

    public string NGallery_GetQDSResults(string requestParam)
    {
        string RetVal;
        string[] Params;
        string SearchIndicators, SearchICs, SearchAreas, SearchLanguage, GalleryItemType, userNid, keywords = string.Empty, galleryFolder = string.Empty;
        List<string> SearchWords;
        int DBNId, NumResults;
        bool HandleAsDIUAOrDIUFlag;
        DataTable DtQDSResults;
        DateTime StartTime;

        RetVal = string.Empty;
        NumResults = 0;
        HandleAsDIUAOrDIUFlag = true;
        StartTime = DateTime.Now;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            SearchIndicators = this.SortString(Params[0].Trim(), ",");
            SearchICs = this.SortString(Params[1].Trim(), ",");
            SearchAreas = this.SortString(Params[2].Trim(), ",");
            SearchLanguage = Params[3].Trim();
            DBNId = Convert.ToInt32(Params[4].Trim());
            GalleryItemType = Params[5].Trim();
            SearchWords = new List<string>(Params[6].Trim().Split(new string[] { "||" }, StringSplitOptions.None));
            userNid = Params[7].Trim();
            if (!string.IsNullOrEmpty(Params[8]))
                keywords = this.SortString(Params[8].Trim(), ",");
            if (string.IsNullOrEmpty(SearchAreas))
            {
                //SearchAreas = Global.GetDefaultArea(DBNId.ToString());
                HandleAsDIUAOrDIUFlag = false;
            }
            if (isUserAdmin(userNid) || userNid == "-1")
            {
                galleryFolder = "stock/gallery/public/admin/";
                //DtQDSResults = this.GetQDSResultsTable(DBNId, SearchIndicators, SearchICs, SearchAreas, SearchLanguage, HandleAsDIUAOrDIUFlag);
                RetVal = "<results><s><base_path path=\"" + this.Page.Request.Url.AbsoluteUri.Replace("libraries/aspx/CallBack.aspx", galleryFolder) + "\"/>" + this.NGallery_GetGalleryResultsXML(SearchIndicators, SearchAreas, keywords, SearchLanguage, DBNId, GalleryItemType, SearchWords, ref NumResults, this.Page.Request.Url.AbsoluteUri.Replace("libraries/aspx/CallBack.aspx", galleryFolder)) + "</s></results>";
            }
            if (!isUserAdmin(userNid) && userNid != "-1")
            {
                galleryFolder = "stock/gallery/private/" + userNid + "/";
                //DtQDSResults = this.GetQDSResultsTable(DBNId, SearchIndicators, SearchICs, SearchAreas, SearchLanguage, HandleAsDIUAOrDIUFlag);
                string privateResult = this.NGallery_GetGalleryResultsXML(SearchIndicators, SearchAreas, keywords, SearchLanguage, DBNId, GalleryItemType, SearchWords, ref NumResults, this.Page.Request.Url.AbsoluteUri.Replace("libraries/aspx/CallBack.aspx", galleryFolder));
                galleryFolder = "stock/gallery/public/admin/";
                string publicResult = this.NGallery_GetGalleryResultsXML(SearchIndicators, SearchAreas, keywords, SearchLanguage, DBNId, GalleryItemType, SearchWords, ref NumResults, this.Page.Request.Url.AbsoluteUri.Replace("libraries/aspx/CallBack.aspx", galleryFolder));
                galleryFolder = "stock/gallery/private/" + userNid + "/";
                //RetVal = "<results><s><base_path path=\"" + this.Page.Request.Url.AbsoluteUri.Replace("libraries/aspx/CallBack.aspx", galleryFolder) + "\"/>" + privateResult + "</s></results>";
                RetVal = "<results><s><base_path path=\"" + this.Page.Request.Url.AbsoluteUri.Replace("libraries/aspx/CallBack.aspx", galleryFolder) + "\"/>" + privateResult + publicResult + "</s></results>";

            }
            RetVal += Constants.Delimiters.ParamDelimiter + NumResults.ToString() + Constants.Delimiters.ParamDelimiter +
                     ((TimeSpan)DateTime.Now.Subtract(StartTime)).TotalSeconds.ToString("0.00");
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string NGallery_GetASResults(string requestParam)
    {
        string RetVal;
        string[] Params;
        string SearchIndicatorICs, SearchAreas, SearchLanguage, GalleryItemType, DBNIds;
        List<string> SearchWords;
        int DBNId, NumResults;
        bool HandleAsDIUAOrDIUFlag;
        DataTable DtASResults;
        DateTime StartTime;

        RetVal = "<results><s><base_path path=\"" + this.Page.Request.Url.AbsoluteUri.Replace("libraries/aspx/CallBack.aspx", "stock/gallery/") + "\"/></s>";
        NumResults = 0;
        HandleAsDIUAOrDIUFlag = true;
        DtASResults = new DataTable();
        StartTime = DateTime.Now;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            SearchIndicatorICs = this.CustomiseStringForIndexingAndQuery(Params[0].Trim());
            SearchAreas = this.CustomiseStringForIndexingAndQuery(Params[1].Trim());
            SearchLanguage = Params[2].Trim();
            DBNIds = Params[3].Trim();
            GalleryItemType = Params[4].Trim();
            SearchWords = new List<string>(Params[5].Trim().Split(new string[] { "||" }, StringSplitOptions.None));

            HandleAsDIUAOrDIUFlag = !string.IsNullOrEmpty(SearchAreas);

            for (int i = 0; i < DBNIds.Split(',').Length; i++)
            {
                DBNId = Convert.ToInt32(DBNIds.Split(',')[i]);

                if (HandleAsDIUAOrDIUFlag == false)
                {
                    SearchAreas = Global.GetDefaultArea(DBNId.ToString());
                }

                DtASResults = this.GetASResultsTableFromIndexingDatabase(DBNId, SearchIndicatorICs, SearchAreas, SearchLanguage, HandleAsDIUAOrDIUFlag);

                if (DtASResults.Rows.Count == 0)
                {
                    DtASResults = this.GetASResultsTableFromDIDatabase(DBNId, SearchIndicatorICs, SearchAreas, SearchLanguage, HandleAsDIUAOrDIUFlag);
                    this.InsertIntoIndexingDatabase(DBNId, SearchIndicatorICs, SearchAreas, SearchLanguage, DtASResults, HandleAsDIUAOrDIUFlag);
                }

                RetVal += this.NGallery_GetSearchResultsXML(DtASResults, SearchLanguage, DBNId, GalleryItemType, SearchWords, ref NumResults, "");

            }
            RetVal += "</results>" + Constants.Delimiters.ParamDelimiter + NumResults.ToString() + " results in " +
                      ((TimeSpan)DateTime.Now.Subtract(StartTime)).TotalSeconds.ToString("0.00") + " seconds";
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string NGallery_GetMoreKeywords(string requestParam)
    {
        string RetVal;
        string[] Params;
        string KeywordNIds, LangCode;

        RetVal = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            KeywordNIds = Params[0].Trim();
            LangCode = Params[1].Trim();
            RetVal = this.NGallery_GetMoreKeywordXML(KeywordNIds, LangCode);
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }
    public string GetSearchedKeywords(string requestParam)
    {
        string RetVal;
        string[] Params;
        RetVal = string.Empty;
        string KeywordNIds;
        List<string> SearchWords;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            KeywordNIds = Params[0].Trim();
            if (Params.Length > 1)
            {
                SearchWords = new List<string>(Params[1].Trim().Split(new string[] { "||" }, StringSplitOptions.None));
            }
            else
            {
                SearchWords = new List<string>();
            }

            string SearchedKeyWords = Global.GetSearchedKeywords(KeywordNIds, SearchWords, Server.MapPath("~//stock//Database.mdb"));
            RetVal = SearchedKeyWords;
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;

    }


    #endregion "--Public--"

    #endregion "--Methods--"
}
