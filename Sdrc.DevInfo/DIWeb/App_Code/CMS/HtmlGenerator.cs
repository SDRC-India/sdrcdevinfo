using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.IO;
using HtmlAgilityPack;


/// <summary>
/// Generates Html templates For CMS
/// </summary>
//public class HtmlGenerator
//{
//    // Variables for Containing path of comtent pages
//    internal static string NewsContentPage = Constants.CMS.NewsContentPage_Template;
//    internal static string ActionContentPage = Constants.CMS.ActionContentPage_Template;
//    internal static string GoalContentPage = Constants.CMS.FactsContentPage_Template;

//    internal static string NewsMainPage = Constants.CMS.AdaptaionNews_Template;
//    internal static string ActionMainPage = Constants.CMS.AdaptaionAction_Template;
//    internal static string GoalMainPage = Constants.CMS.AdaptaionFacts_Template;

//    internal static string NewsLinkPage = Constants.CMS.NewsLink_Template;
//    internal static string ActionLinkPage = Constants.CMS.ActionLink_Template;
//    internal static string GoalLinkPage = Constants.CMS.FactsLink_Template;


//    /// <summary>
//    /// Get CMS contenty data from database by URL
//    /// </summary>
//    /// <param name="Url">URL of CMS Content</param>
//    /// <returns>returns Object Containing data for CMS Content if exist, else returns null</returns>
//    public DataContent GetDataContentFromDatabaseByUrl(string Url)
//    {
//        DataContent RetVal = null;
//        DIConnection ObjDIConnection = null;
//        CMSHelper Helper = new CMSHelper();
//        List<System.Data.Common.DbParameter> DbParams = null;
//        DataTable DtRetData = null;
//        try
//        {   //Get Connection Object
//            ObjDIConnection = Helper.GetConnectionObject();
//            // Checke if Connection  object is null then return null and stop further flow to execute
//            if (ObjDIConnection == null)
//            { return RetVal; }
//            // If Connection object is not null then excute further code 
//            else
//            {
//                // Initlize DbParameter
//                DbParams = new List<System.Data.Common.DbParameter>();
//                // create tag parameter
//                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
//                Param1.ParameterName = "@Url";
//                Param1.DbType = DbType.String;
//                Param1.Value = Url;
//                // add Tag Parm to DbParameter
//                DbParams.Add(Param1);

//                // Execute stored procedure to get CMS Data From Database
//                DtRetData = ObjDIConnection.ExecuteDataTable("sp_GetDataByUrl", CommandType.StoredProcedure, DbParams);
//                // Check if return datatable is not null and having atleast 1 record
//                if (DtRetData != null && DtRetData.Rows.Count > 0)
//                {
//                    // Initlize class DataContent
//                    RetVal = new DataContent();
//                    // innitlize values to class variables
//                    RetVal.ContentId = (int)DtRetData.Rows[0]["ContentId"];
//                    RetVal.MenuCategory = DtRetData.Rows[0]["MenuCategory"].ToString();//DtRetData.Rows[0]["MenuCategory"]!=null? DtRetData.Rows[0]["MenuCategory"].ToString() : string.Empty;
//                    RetVal.Title = DtRetData.Rows[0]["Title"].ToString();
//                    RetVal.Thumbnail = DtRetData.Rows[0]["Thumbnail"].ToString();
//                    RetVal.Date = Convert.ToDateTime(DtRetData.Rows[0]["Date"].ToString());
//                    RetVal.DateAdded = Convert.ToDateTime(DtRetData.Rows[0]["DateAdded"]);
//                    RetVal.DateModified = Convert.ToDateTime(DtRetData.Rows[0]["DateModified"]);
//                    RetVal.Description = DtRetData.Rows[0]["Description"].ToString();
//                    RetVal.PDFUpload = DtRetData.Rows[0]["PDFUpload"].ToString();
//                    RetVal.Summary = DtRetData.Rows[0]["Summary"].ToString();
//                    RetVal.URL = DtRetData.Rows[0]["URL"].ToString();
//                    RetVal.Archived = (bool)DtRetData.Rows[0]["Archived"];
//                    RetVal.UserNameEmail = DtRetData.Rows[0]["UserNameEmail"].ToString();
//                    RetVal.LngCode = DtRetData.Rows[0]["LngCode"].ToString();
//                    RetVal.ArticleTagID = (int)DtRetData.Rows[0]["ArticleTagId"];
//                }
//            }
//        }
//        catch (Exception Ex)
//        {
//            RetVal = null;
//            Global.CreateExceptionString(Ex, null);
//        }

//        finally
//        {
//            ObjDIConnection = null;
//        }
//        return RetVal;
//    }

//    /// <summary>
//    /// Gets tags from database based on TagId, using stored procedure
//    /// </summary>
//    /// <param name="TagId">NId of tag for which tags are to be retrieved</param>
//    /// <returns>Retruns list containg tags if exise, elase returns null</returns>
//    public string GetTagsFromDatabaseByTagId(int TagId)
//    {
//        string RetVal = string.Empty;
//        List<string> RetTags = null;
//        DIConnection ObjDIConnection = null;
//        CMSHelper Helper = new CMSHelper();
//        List<System.Data.Common.DbParameter> DbParams = null;
//        DataTable DtRetData = null;
//        string TagValue = string.Empty;
//        try
//        {   //Get Connection Object
//            ObjDIConnection = Helper.GetConnectionObject();
//            // Checke if Connection  object is null then return null and stop further flow to execute
//            if (ObjDIConnection == null)
//            { return RetVal; }
//            // If Connection object is not null then excute further code 
//            else
//            {
//                // Initlize DbParameter
//                DbParams = new List<System.Data.Common.DbParameter>();
//                // create tag parameter
//                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
//                Param1.ParameterName = "@TagId";
//                Param1.DbType = DbType.String;
//                Param1.Value = TagId;
//                DbParams.Add(Param1);

//                // Execute stored procedure to get Tags From Database
//                DtRetData = ObjDIConnection.ExecuteDataTable("sp_GetTagsById", CommandType.StoredProcedure, DbParams);
//                // Check if return datatable is not null and having atleast 1 record
//                if (DtRetData != null && DtRetData.Rows.Count > 0)
//                {
//                    RetTags = new List<string>();
//                    // Itterate through loop
//                    for (int Icount = 0; Icount < DtRetData.Rows.Count; Icount++)
//                    {// Get Tag from datatable
//                        TagValue = DtRetData.Rows[Icount]["ArticleTag"].ToString();
//                        // assign tag value to list RetVal
//                        RetTags.Add(TagValue);
//                    }
//                }

//                if (RetTags != null)
//                {
//                    foreach (string Tag in RetTags)
//                    {
//                        RetVal = Tag + "," + RetVal;
//                    }
//                    RetVal = RetVal.Substring(0, RetVal.Length - 1);
//                }
//            }
//        }
//        catch (Exception Ex)
//        {
//            RetVal = string.Empty;
//            Global.CreateExceptionString(Ex, null);
//        }
//        finally
//        {
//            ObjDIConnection = null;
//        }
//        return RetVal;
//    }

//    /// <summary>
//    /// Get Html Content Based on DataContent menu category
//    /// </summary>
//    /// <param name="Data">Is a Object contining records for creating CMS Template</param>
//    /// <returns>String containing Html template in which input datacontent is binded</returns>
//    private string GetHtmlByMenuCategory(DataContent Data)
//    {
//        string RetVal = string.Empty;
//        String TemplateFullPath = string.Empty;
//        try
//        {
//            // EnumHelper.MenuCategory OCallbackType = (EnumHelper.MenuCategory)Menu;
//            switch (Data.MenuCategory.ToLower())
//            {
//                case "news":
//                    TemplateFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, NewsContentPage);
//                    break;
//                case "action":
//                    TemplateFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ActionContentPage);
//                    break;
//                case "facts":
//                    TemplateFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, GoalContentPage);
//                    break;
//            }
//            // Call method to bind input Data to html template, according to menu category 
//            RetVal = BindDataToTemplateByMenuCategory(Data.MenuCategory, Data, TemplateFullPath);

//        }
//        catch (Exception Ex)
//        {
//            RetVal = string.Empty;
//            Global.CreateExceptionString(Ex, null);

//        }
//        return RetVal;
//    }

//    /// <summary>
//    /// Bind Imput datacontent to html template, according to menu category
//    /// </summary>
//    /// <param name="Menu">menu category of cms content</param>
//    /// <param name="Data">object containing fileds for creating cms template</param>
//    /// <param name="TemplateFullPath">Path of Html Template</param>
//    /// <returns>Returns html template Binded with input cms content</returns>
//    private string BindDataToTemplateByMenuCategory(string Menu, DataContent Data, string TemplateFullPath)
//    {
//        string RetVal = string.Empty;
//        CMSHelper CmsHelp = null;
//        string TemplateHtml = string.Empty;
//        try
//        {
//            // check if template file existes in database,
//            // if file not exist return empty string 
//            // and break futher execution of code
//            if (!File.Exists(TemplateFullPath))
//            {
//                return RetVal;
//            }
//            else
//            {
//                CmsHelp = new CMSHelper();
//                // Get template of inner Content page based on MenuCategory
//                TemplateHtml = CmsHelp.GetHtmlFromPath(TemplateFullPath);
//                // check if page is not of Facts type
//                // since facts doesnot contains fields PdfUrl and Date
//                // replace #fields of template with actual value 
//                if (Menu != EnumHelper.MenuCategory.Facts.ToString())
//                {
//                    // Set Values in Html Template
//                    TemplateHtml = TemplateHtml.Replace("#Title", Data.Title);
//                    TemplateHtml = TemplateHtml.Replace("#MainContent", Data.Description);
//                    TemplateHtml = TemplateHtml.Replace("#PdfUrl", Data.PDFUpload);
//                    TemplateHtml = TemplateHtml.Replace("#Date", Data.Date.ToString());
//                    RetVal = TemplateHtml;
//                }
//                // check if page is of Facts type
//                // facts contains an extra field summary
//                // replace #fields of template with actual value 
//                else if (Menu == EnumHelper.MenuCategory.Facts.ToString())
//                {
//                    // Set Values in Html Template
//                    TemplateHtml = TemplateHtml.Replace("#Title", Data.Title);
//                    TemplateHtml = TemplateHtml.Replace("#MainContent", Data.Description);
//                    TemplateHtml = TemplateHtml.Replace("#Summary", Data.Summary);
//                    RetVal = TemplateHtml;
//                }
//            }
//        }
//        catch (Exception Ex)
//        {
//            RetVal = string.Empty;
//            Global.CreateExceptionString(Ex, null);

//        }
//        return RetVal;

//    }

//    /// <summary>
//    /// Creates HTML Template Based On URL
//    /// </summary>
//    /// <param name="Url">Url oF Html Template</param>
//    /// <param name="MenuCategory">Out Parameter, Keeps track of MenuCategory, of return html content</param>
//    /// <returns>Returns Html containd template if exist, else returns empty string</returns>
//    public string CreateCMSHtmlByUrl(string Url, out string MenuCategory)
//    {
//        string RetVal = string.Empty;
//        DataContent ObjDataContent = new DataContent();
//        MenuCategory = string.Empty;
//        try
//        {
//            // Call method to get data content based on url
//            ObjDataContent = this.GetDataContentFromDatabaseByUrl(Url);
//            //check if data content is null then return null
//            if (ObjDataContent == null)
//            {
//                return RetVal;
//            }
//            else //If Return ObjDataContent is not null execute further code
//            {
//                // Assign out parameter MenuCategory, to keep track MenuCategory of return html content 
//                MenuCategory = ObjDataContent.MenuCategory;
//                // Call method to Get Html By menu category
//                RetVal = this.GetHtmlByMenuCategory(ObjDataContent);
//            }

//        }
//        catch (Exception Ex)
//        {
//            MenuCategory = string.Empty;
//            RetVal = string.Empty;
//            Global.CreateExceptionString(Ex, null);

//        }
//        return RetVal;
//    }

//    /// <summary>
//    /// Create html page containing articles based on menu category
//    /// 1. get content from database for articles by menu category
//    /// 2. get total record count of articles by menucatogry for pagination
//    /// 3. call method to add pagination, as well define clicks of page links
//    /// </summary>
//    /// <param name="RecordStartPosition"> position from where to selcect first record</param>
//    /// <param name="MaxArticleCount">no of articles to be retreved</param>
//    /// <param name="MenuCategory">category of article</param>
//    /// <returns>if record exists in database returns html containing articles, else returns empty string </returns>
//    public string GetArticleByMenuCategory(int RecordStartPosition, int MaxArticleCount, string MenuCategory, int currentPageNumber)
//    {
//        string RetVal = string.Empty;
//        List<DataContent> ListDataContent = new List<DataContent>();
//        int TotalArticleCount = 0;
//        try
//        {
//            // Call method to get data content based on url
//            ListDataContent = this.GetDataContentFromDatabase_ByMenuCategory(RecordStartPosition, MaxArticleCount, MenuCategory);

//            // Call Method to Get total article conunt by MenuCategory
//            TotalArticleCount = this.GetTotalArticleCount_ByMenucategory(MenuCategory);

//            //check if data content is null then return null
//            if (ListDataContent == null)
//            {
//                return RetVal;
//            }
//            //If Return ObjDataContent is not null execute further code
//            else
//            {
//                // Call method to Get Html By menu category
//                RetVal = this.GetFrontPageHtmlByMenuCategoryNTag(ListDataContent, MenuCategory, MaxArticleCount, TotalArticleCount, currentPageNumber);
//            }

//        }
//        catch (Exception Ex)
//        {
//            RetVal = string.Empty;
//            Global.CreateExceptionString(Ex, null);

//        }
//        return RetVal;
//    }


//    private List<DataContent> GetDataContentFromDatabase_ByMenuCategory(int RecordStartPosition, int NoOfRecords, string MenuCategory)
//    {
//        List<DataContent> RetVal = null;
//        DataContent ObjDataContent = new DataContent();
//        DIConnection ObjDIConnection = null;
//        CMSHelper Helper = new CMSHelper();
//        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
//        DataTable DtRetData = null;
//        try
//        {   //Get Connection Object
//            ObjDIConnection = Helper.GetConnectionObject();
//            // Checke if Connection  object is null then return null and stop further flow to execute
//            if (ObjDIConnection == null)
//            { return RetVal; }
//            // If Connection object is not null then excute further code 
//            else
//            {
//                // create database parameters
//                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
//                Param1.ParameterName = "@RecordStartPosition";
//                Param1.DbType = DbType.Int32;
//                Param1.Value = RecordStartPosition;
//                DbParams.Add(Param1);

//                System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter(); // create NoOfRecords parameter
//                Param2.ParameterName = "@NoOfRecords";
//                Param2.DbType = DbType.Int32;
//                Param2.Value = NoOfRecords;
//                DbParams.Add(Param2);

//                System.Data.Common.DbParameter Param3 = ObjDIConnection.CreateDBParameter(); // create MenuCategory parameter
//                Param3.ParameterName = "@MenuCategory";
//                Param3.DbType = DbType.String;
//                Param3.Value = MenuCategory;
//                DbParams.Add(Param3);

//                // Execute stored procedure to get CMS Data From Database
//                DtRetData = ObjDIConnection.ExecuteDataTable("sp_GetCMSRecordsByMenuCategory", CommandType.StoredProcedure, DbParams);
//                // Check if return datatable is not null and having atleast 1 record
//                if (DtRetData != null && DtRetData.Rows.Count > 0)
//                {
//                    RetVal = new List<DataContent>();
//                    for (int ICount = 0; ICount < DtRetData.Rows.Count; ICount++)
//                    {
//                        // Initlize class DataContent
//                        ObjDataContent = new DataContent();
//                        // innitlize values to class variables
//                        ObjDataContent.MenuCategory = DtRetData.Rows[ICount]["MenuCategory"].ToString();//DtRetData.Rows[0]["MenuCategory"]!=null? DtRetData.Rows[0]["MenuCategory"].ToString() : string.Empty;
//                        ObjDataContent.Title = DtRetData.Rows[ICount]["Title"].ToString();
//                        ObjDataContent.Thumbnail = DtRetData.Rows[ICount]["Thumbnail"].ToString();
//                        ObjDataContent.Date = Convert.ToDateTime(DtRetData.Rows[ICount]["Date"].ToString());
//                        ObjDataContent.DateAdded = Convert.ToDateTime(DtRetData.Rows[ICount]["DateAdded"]);
//                        ObjDataContent.DateModified = Convert.ToDateTime(DtRetData.Rows[ICount]["DateModified"]);
//                        ObjDataContent.Description = DtRetData.Rows[ICount]["Description"].ToString();
//                        ObjDataContent.PDFUpload = DtRetData.Rows[ICount]["PDFUpload"].ToString();
//                        ObjDataContent.Summary = DtRetData.Rows[ICount]["Summary"].ToString();
//                        ObjDataContent.URL = DtRetData.Rows[ICount]["URL"].ToString();
//                        ObjDataContent.Archived = (bool)DtRetData.Rows[ICount]["Archived"];
//                        ObjDataContent.UserNameEmail = DtRetData.Rows[ICount]["UserNameEmail"].ToString();
//                        ObjDataContent.LngCode = DtRetData.Rows[ICount]["LngCode"].ToString();
//                        ObjDataContent.ArticleTagID = (int)DtRetData.Rows[ICount]["ArticleTagId"];
//                        RetVal.Add(ObjDataContent);
//                    }
//                }
//            }
//        }
//        catch (Exception Ex)
//        {
//            RetVal = null;
//            Global.CreateExceptionString(Ex, null);
//        }

//        finally
//        {
//            ObjDIConnection = null;
//        }
//        return RetVal;
//    }

//    private string GetFrontPageHtmlByMenuCategoryNTag(List<DataContent> ListData, string MenuCategory, int MaxArticleCount, int TotalArticleCount, int currentPageNumber)
//    {
//        string RetVal = string.Empty;
//        String MainPageTemplatePath = string.Empty;
//        String LinkTemplatePath = string.Empty;
//        try
//        {
//            switch (MenuCategory.ToLower())
//            {
//                case "news":
//                    MainPageTemplatePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, NewsMainPage);
//                    LinkTemplatePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, NewsLinkPage);
//                    break;
//                case "action":
//                    MainPageTemplatePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ActionMainPage);
//                    LinkTemplatePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ActionLinkPage);
//                    break;
//                case "facts":
//                    MainPageTemplatePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, GoalMainPage);
//                    LinkTemplatePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, GoalLinkPage);
//                    break;
//            }
//            // Call method to bind input Data to html template, according to menu category 
//            RetVal = CreateArticleLinksForMainPage(MenuCategory, ListData, MainPageTemplatePath, LinkTemplatePath, MaxArticleCount, TotalArticleCount, currentPageNumber);

//        }
//        catch (Exception Ex)
//        {
//            RetVal = string.Empty;
//            Global.CreateExceptionString(Ex, null);

//        }
//        return RetVal;
//    }

//    private string CreateArticleLinksForMainPage(string MenuCategory, List<DataContent> ListDataContent, string MainPageTemplatePath, string LinkTemplatePath, int MaxArticleCount, int TotalArticleCount, int currentPageNumber)
//    {
//        string RetVal = string.Empty;
//        CMSHelper CmsHelp = null;
//        string TemplateHtml = string.Empty;
//        String MainPageTemplateContent = string.Empty;
//        String LinkTemplateContent = string.Empty;
//        HtmlDocument HtmDocument = null;
//        string ShowEditLinkStyle = "none";
//        try
//        {
//            if (!File.Exists(MainPageTemplatePath) && !File.Exists(LinkTemplatePath))
//            {
//                return RetVal;
//            }
//            else
//            {
//                if (HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser] != null)
//                {
//                    ShowEditLinkStyle = "block";
//                }

//                HtmDocument = new HtmlDocument();
//                HtmDocument.Load(MainPageTemplatePath);
//                CmsHelp = new CMSHelper();
//                // Get Content of Main Page by mainpage template path
//                MainPageTemplateContent = CmsHelp.GetHtmlFromPath(MainPageTemplatePath);

//                // itterate through loop
//                foreach (DataContent ObjDataCont in ListDataContent)
//                {
//                    // Get Content of Link Page by linkpage template path
//                    LinkTemplateContent = CmsHelp.GetHtmlFromPath(LinkTemplatePath);
//                    // Set Values in Html 
//                    if (MenuCategory != EnumHelper.MenuCategory.Facts.ToString())
//                    {
//                        LinkTemplateContent = string.Format(LinkTemplateContent, ObjDataCont.URL, ObjDataCont.Title, ObjDataCont.Date, ObjDataCont.Thumbnail, ObjDataCont.Summary, ShowEditLinkStyle);
//                    }
//                    else if (MenuCategory == EnumHelper.MenuCategory.Facts.ToString())
//                    {
//                        LinkTemplateContent = string.Format(LinkTemplateContent, ObjDataCont.URL, ObjDataCont.Title, ObjDataCont.Date, ObjDataCont.Thumbnail, ObjDataCont.Summary, ShowEditLinkStyle);
//                    }
//                    HtmDocument = this.AddHtmlLinkContentToMainPage(HtmDocument, LinkTemplateContent);

//                }

//                // Add pager
//                if (TotalArticleCount > 1)
//                {
//                    HtmDocument = this.AddPaginationToMainPage(HtmDocument, LinkTemplateContent, MaxArticleCount, TotalArticleCount, MenuCategory, currentPageNumber);
//                }

//                RetVal = HtmDocument.DocumentNode.InnerHtml;
//            }
//        }
//        catch (Exception Ex)
//        {
//            RetVal = string.Empty;
//            Global.CreateExceptionString(Ex, null);

//        }
//        return RetVal;

//    }


//    private HtmlDocument AddHtmlLinkContentToMainPage(HtmlDocument Document, string LinkContent)
//    {
//        HtmlDocument RetVal = null;
//        try
//        {
//            // Targets a specific node(since all the page contains parent table tag with width 700)
//            HtmlNode content_wrapper = Document.DocumentNode.SelectNodes("//table[@id='MainTableForNewsList']").FirstOrDefault();

//            HtmlNode tr = Document.CreateElement("tr");
//            content_wrapper.ChildNodes.Append(tr);
//            HtmlNode td = Document.CreateElement("td");
//            td.InnerHtml = LinkContent;
//            tr.ChildNodes.Append(td);

//            RetVal = Document;
//        }
//        catch (Exception Ex)
//        {
//            Global.CreateExceptionString(Ex, null);
//        }
//        // return html document
//        return RetVal;
//    }

//    private HtmlDocument AddPaginationToMainPage(HtmlDocument Document, string LinkContent, int MaxArticleCount, int TotalArticleCount, string MenuCategory, int currentPageNumber, int totalPagingMenuLength = 5)
//    {
//        string NewsPage = Constants.CMS.AdaptaionNewsPageName.ToLower();
//        string ActionPage = Constants.CMS.AdaptaionActionPageName.ToLower();
//        string FactsPage = Constants.CMS.AdaptaionActionPageName.ToLower();
//        string AdaptaionPagingName = Constants.CMS.AdaptaionPagingName.ToLower();
//        HtmlDocument RetVal = null;
//        int PageCount = 0;
//        int Icount = 1;
//        int StartIndex = 1;
//        int LastIndex = -1;
//        int HalfPageSize = totalPagingMenuLength / 2;
//        try
//        {
//            // Targets a specific node(since all the page contains parent table tag with width 700)
//            HtmlNode content_wrapper = Document.DocumentNode.SelectNodes("//tr[@id='PagerTr']").FirstOrDefault();

//            if (TotalArticleCount % MaxArticleCount == 0)
//            {
//                PageCount = TotalArticleCount / MaxArticleCount;
//            }
//            else
//            {
//                PageCount = TotalArticleCount / MaxArticleCount + 1;
//            }

//            LastIndex = PageCount;
//            //Get Current Menu Numbers
//            if (currentPageNumber <= HalfPageSize)
//            {
//                if (PageCount > totalPagingMenuLength)
//                {
//                    LastIndex = totalPagingMenuLength;
//                }
//                else
//                {
//                    LastIndex = PageCount;
//                }
//            }
//            else
//            {
//                if (currentPageNumber > (PageCount - HalfPageSize))
//                {
//                    if (PageCount > totalPagingMenuLength)
//                    {
//                        StartIndex = PageCount - totalPagingMenuLength;
//                    }
//                }
//                else
//                {
//                    if (PageCount > totalPagingMenuLength)
//                    {
//                        StartIndex = currentPageNumber - HalfPageSize;
//                        LastIndex = currentPageNumber + HalfPageSize;
//                    }
//                }
//            }
//            //Make the Start and Move Previous buttons
//            //Move First
//            HtmlNode TdTag;
//            HtmlNode AnchorTag;
//            CreateNextPreviousButtons(Document, content_wrapper, out TdTag, out AnchorTag, "<<", currentPageNumber, PageCount, MenuCategory);
//            //Move Previous
//            CreateNextPreviousButtons(Document, content_wrapper, out TdTag, out AnchorTag, "<", currentPageNumber, PageCount, MenuCategory);
//            for (Icount = StartIndex; Icount <= LastIndex; Icount++)
//            {
//                TdTag = Document.CreateElement("td");
//                TdTag.SetAttributeValue("id", "PagerTd" + Icount.ToString());
//                TdTag.SetAttributeValue("class", "wdh_pcnt_eght txt_algn_cntr");
//                AnchorTag = Document.CreateElement("a");
//                AnchorTag.SetAttributeValue("id", "PagerAnc");
//                switch (MenuCategory.ToLower())
//                {
//                    case "news":
//                        AnchorTag.SetAttributeValue("href", NewsPage + AdaptaionPagingName + Icount);
//                        break;
//                    case "action":
//                        AnchorTag.SetAttributeValue("href", ActionPage + AdaptaionPagingName + Icount);
//                        break;
//                    case "facts":
//                        AnchorTag.SetAttributeValue("href", FactsPage + AdaptaionPagingName + Icount);
//                        break;
//                }
//                AnchorTag.SetAttributeValue("pageno", Icount.ToString());
//                if (currentPageNumber == Icount)
//                {
//                    AnchorTag.InnerHtml = "<b class='fnt_sz_sntn'>" + Icount.ToString() + "</b>";
//                }
//                else
//                {
//                    AnchorTag.InnerHtml = Icount.ToString();
//                }

//                TdTag.ChildNodes.Append(AnchorTag);
//                content_wrapper.ChildNodes.Append(TdTag);
//            }
//            //Move Next
//            CreateNextPreviousButtons(Document, content_wrapper, out TdTag, out AnchorTag, ">", currentPageNumber, PageCount, MenuCategory);
//            //Move Last
//            CreateNextPreviousButtons(Document, content_wrapper, out TdTag, out AnchorTag, ">>", currentPageNumber, PageCount, MenuCategory);
//            RetVal = Document;
//        }
//        catch (Exception Ex)
//        {
//            Global.CreateExceptionString(Ex, null);
//        }
//        // return html document
//        return RetVal;
//    }

//    private static void CreateNextPreviousButtons(HtmlDocument Document, HtmlNode content_wrapper, out HtmlNode TdTag, out HtmlNode AnchorTag, string symbol, int currentPageNumber, int pageSize, string MenuCategory)
//    {
//        string NewsPage = Constants.CMS.AdaptaionNewsPageName.ToLower();
//        string ActionPage = Constants.CMS.AdaptaionActionPageName.ToLower();
//        string FactsPage = Constants.CMS.AdaptaionActionPageName.ToLower();
//        string AdaptaionPagingName = Constants.CMS.AdaptaionPagingName.ToLower();
//        TdTag = Document.CreateElement("td");
//        TdTag.SetAttributeValue("class", "wdh_pcnt_three txt_algn_cntr");
//        AnchorTag = Document.CreateElement("a");
//        string PageNameRequesting = string.Empty;
//        switch (MenuCategory.ToLower())
//        {
//            case "news":
//                PageNameRequesting = NewsPage + AdaptaionPagingName;
//                break;
//            case "action":
//                PageNameRequesting = ActionPage + AdaptaionPagingName;
//                break;
//            case "facts":
//                PageNameRequesting = FactsPage + AdaptaionPagingName;
//                break;
//        }
//        if (symbol == "<<")
//        {
//            AnchorTag.InnerHtml = "<img src='../stock/themes/default/images/first.png' alt='Next' />";
//            AnchorTag.SetAttributeValue("href", PageNameRequesting + "1");
//            if (currentPageNumber == 1)
//            {
//                AnchorTag.SetAttributeValue("disabled", "disabled");
//            }
//        }
//        else if (symbol == "<")
//        {
//            AnchorTag.InnerHtml = "<img src='../stock/themes/default/images/prev.png' alt='Next' />";
//            if (currentPageNumber == 1)
//            {
//                AnchorTag.SetAttributeValue("href", PageNameRequesting + "1");
//                AnchorTag.SetAttributeValue("disabled", "disabled");
//            }
//            else
//            {
//                AnchorTag.SetAttributeValue("href", PageNameRequesting + (currentPageNumber - 1));
//            }
//        }
//        else if (symbol == ">")
//        {
//            AnchorTag.InnerHtml = "<img src='../stock/themes/default/images/next.png' alt='Next' />";
//            if (currentPageNumber == pageSize)
//            {
//                AnchorTag.SetAttributeValue("href", PageNameRequesting + pageSize);
//                AnchorTag.SetAttributeValue("disabled", "disabled");
//            }
//            else
//            {
//                AnchorTag.SetAttributeValue("href", PageNameRequesting + (currentPageNumber + 1));
//            }
//        }
//        else if (symbol == ">>")
//        {
//            AnchorTag.InnerHtml = "<img src='../stock/themes/default/images/last.png' alt='Next' />";
//            AnchorTag.SetAttributeValue("href", PageNameRequesting + pageSize);
//            if (currentPageNumber == pageSize)
//            {
//                AnchorTag.SetAttributeValue("disabled", "disabled");
//            }
//        }
//        TdTag.ChildNodes.Append(AnchorTag);
//        content_wrapper.ChildNodes.Append(TdTag);
//    }


//    /// <summary>
//    /// Gets count maximum no of articles by MenuCategory
//    /// </summary>
//    /// <param name="MenuCategory">Input MenuCategory</param>
//    /// <returns>TotalArticleCount</returns>
//    private int GetTotalArticleCount_ByMenucategory(string MenuCategory)
//    {
//        CMSHelper ObjCMSHelper = new CMSHelper();
//        string ResultNid = string.Empty;
//        int RetVal = 0;
//        string DBConnectionStatusMessage = string.Empty;
//        List<System.Data.Common.DbParameter> DbParams = null;
//        DIConnection ObjDIConnection = null;
//        int TotalArticleCount = -1;
//        try
//        {

//            // Call method to get connection object
//            ObjDIConnection = ObjCMSHelper.GetConnectionObject(out DBConnectionStatusMessage);
//            // Check if connection object is not null
//            if (ObjDIConnection == null)
//            {
//                RetVal = -1;
//                return RetVal;
//            }
//            // Innitilze DbParams object
//            DbParams = new List<System.Data.Common.DbParameter>();
//            // create string MenuCategory parameter
//            System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
//            Param1.ParameterName = "@MenuCategory";
//            Param1.DbType = DbType.String;
//            Param1.Value = MenuCategory;
//            DbParams.Add(Param1);

//            // Execute stored procedure to get tag Nid
//            TotalArticleCount = Convert.ToInt32(ObjDIConnection.ExecuteScalarSqlQuery("sp_GetArticlesCountByMenuCategory", CommandType.StoredProcedure, DbParams));
//            if (TotalArticleCount > 0)
//            {
//                RetVal = TotalArticleCount;
//            }
//        }
//        catch (Exception Ex)
//        {
//            RetVal = -1;
//            Global.CreateExceptionString(Ex, null);
//        }
//        finally
//        {
//            DbParams = null;
//            ObjDIConnection = null;
//        }
//        return RetVal;
//    }




//    /// <summary>
//    /// Create html page containing articles based on menu category and article tags
//    /// 1. get content from database for articles by menu category and article tags
//    /// 2. get total record count of articles by menucatogry and article tags for pagination
//    /// 3. call method to add pagination, as well define clicks of page links
//    /// </summary>
//    /// <param name="RecordStartPosition"> position from where to selcect first record</param>
//    /// <param name="MaxArticleCount">no of articles to be retreved</param>
//    /// <param name="MenuCategory">category of article</param>
//    /// <returns>if record exists in database returns html containing articles, else returns empty string </returns>
//    public string GetArticleByMenuCategory(int RecordStartPosition, int MaxArticleCount, string MenuCategory, int currentPageNumber, string ArticleTags)
//    {
//        string RetVal = string.Empty;
//        List<DataContent> ListDataContent = new List<DataContent>();
//        int TotalArticleCount = 0;
//        try
//        {
//            // Call method to get data content based on url
//            ListDataContent = this.GetDataContentFromDatabase_ByMenuCategory(RecordStartPosition, MaxArticleCount, MenuCategory);

//            // Call Method to Get total article conunt by MenuCategory
//            TotalArticleCount = this.GetTotalArticleCount_ByMenucategory(MenuCategory);

//            //check if data content is null then return null
//            if (ListDataContent == null)
//            {
//                return RetVal;
//            }
//            //If Return ObjDataContent is not null execute further code
//            else
//            {
//                // Call method to Get Html By menu category
//                RetVal = this.GetFrontPageHtmlByMenuCategoryNTag(ListDataContent, MenuCategory, MaxArticleCount, TotalArticleCount, currentPageNumber);
//            }

//        }
//        catch (Exception Ex)
//        {
//            RetVal = string.Empty;
//            Global.CreateExceptionString(Ex, null);

//        }
//        return RetVal;
//    }

//    //------------------------------------------------------------------------------------------------------------------------------------------------------
//    //------------------------------------------------------------------------------------------------------------------------------------------------------

//    /// <summary>
//    /// Get Articles from database based on ArticleTags and MenuCategory, as object
//    /// And bind this object to html Template Based 
//    /// </summary>
//    /// <param name="ArticleTags">tags ofr articles seperated with  ','</param>
//    /// <param name="MenuCategory">specify the MenuCategory of article</param>
//    /// <returns>Returns Html containd template if exist, else returns empty string</returns>
//    public string GetArticlesByArticleTagNMenuCategory(string ArticleTags, string MenuCategory)
//    {
//        string RetVal = string.Empty;
//        DataContent ObjDataContent = new DataContent();
//        MenuCategory = string.Empty;
//        try
//        {
//            // Call method to get data content based on url
//            ObjDataContent = this.GetDataFromDbByAricleTagsNMenuCategory(ArticleTags, MenuCategory);
//            //check if data content is null then return null
//            if (ObjDataContent == null)
//            {
//                return null;
//            }
//            else //If Return ObjDataContent is not null execute further code
//            {
//                // Assign out parameter MenuCategory, to keep track MenuCategory of return html content 
//                MenuCategory = ObjDataContent.MenuCategory;
//                // Call method to Get Html By menu category
//                RetVal = this.GetHtmlByMenuCategory(ObjDataContent);
//            }

//        }
//        catch (Exception Ex)
//        {
//            MenuCategory = string.Empty;
//            RetVal = string.Empty;
//            Global.CreateExceptionString(Ex, null);

//        }
//        return RetVal;
//    }

//    /// <summary>
//    /// Selcect article from database based on menu category and article tags
//    /// </summary>
//    /// <param name="ArticleTags">ArticleTags consist of tags seperated with ','</param>
//    /// <param name="MenuCategory">MenuCategory of requested article</param>
//    /// <returns>returns Object Containing data Content for article if exist, else returns null</returns>
//    public DataContent GetDataFromDbByAricleTagsNMenuCategory(string ArticleTags, string MenuCategory)
//    {
//        DataContent RetVal = null;
//        DIConnection ObjDIConnection = null;
//        CMSHelper Helper = new CMSHelper();
//        List<System.Data.Common.DbParameter> DbParams = null;
//        DataTable DtRetData = null;
//        try
//        {   //Get Connection Object
//            ObjDIConnection = Helper.GetConnectionObject();
//            // Checke if Connection  object is null then return null and stop further flow to execute
//            if (ObjDIConnection == null)
//            { return RetVal; }
//            // If Connection object is not null then excute further code 
//            else
//            {
//                // Initlize DbParameter
//                DbParams = new List<System.Data.Common.DbParameter>();
//                // create tag parameter
//                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
//                Param1.ParameterName = "@ArticleTags";
//                Param1.DbType = DbType.String;
//                Param1.Value = ArticleTags;
//                // add Tag Parm to DbParameter
//                DbParams.Add(Param1);

//                System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();
//                Param2.ParameterName = "@MenuCategory";
//                Param2.DbType = DbType.String;
//                Param2.Value = MenuCategory;
//                // add Tag Parm to DbParameter
//                DbParams.Add(Param2);

//                // Execute stored procedure to get CMS Data From Database
//                DtRetData = ObjDIConnection.ExecuteDataTable("sp_GetCMSRecordsByMenuCategory", CommandType.StoredProcedure, DbParams);
//                // Check if return datatable is not null and having atleast 1 record
//                if (DtRetData != null && DtRetData.Rows.Count > 0)
//                {
//                    // Initlize class DataContent
//                    RetVal = new DataContent();
//                    // innitlize values to class variables
//                    RetVal.ContentId = (int)DtRetData.Rows[0]["ContentId"];
//                    RetVal.MenuCategory = DtRetData.Rows[0]["MenuCategory"].ToString();
//                    RetVal.Title = DtRetData.Rows[0]["Title"].ToString();
//                    RetVal.Thumbnail = DtRetData.Rows[0]["Thumbnail"].ToString();
//                    RetVal.Date = Convert.ToDateTime(DtRetData.Rows[0]["Date"].ToString());
//                    RetVal.DateAdded = Convert.ToDateTime(DtRetData.Rows[0]["DateAdded"]);
//                    RetVal.DateModified = Convert.ToDateTime(DtRetData.Rows[0]["DateModified"]);
//                    RetVal.Description = DtRetData.Rows[0]["Description"].ToString();
//                    RetVal.PDFUpload = DtRetData.Rows[0]["PDFUpload"].ToString();
//                    RetVal.Summary = DtRetData.Rows[0]["Summary"].ToString();
//                    RetVal.URL = DtRetData.Rows[0]["URL"].ToString();
//                    RetVal.Archived = (bool)DtRetData.Rows[0]["Archived"];
//                    RetVal.UserNameEmail = DtRetData.Rows[0]["UserNameEmail"].ToString();
//                    RetVal.LngCode = DtRetData.Rows[0]["LngCode"].ToString();
//                    RetVal.ArticleTagID = (int)DtRetData.Rows[0]["ArticleTagId"];
//                }
//            }
//        }
//        catch (Exception Ex)
//        {
//            RetVal = null;
//            Global.CreateExceptionString(Ex, null);
//        }

//        finally
//        {
//            ObjDIConnection = null;
//        }
//        return RetVal;
//    }



//    /// <summary>
//    /// Gets count of maximum no of articles by MenuCategory and Tag
//    /// </summary>
//    /// <param name="MenuCategory">Input MenuCategory</param>
//    /// <returns>TotalArticleCount</returns>
//    private int GetTotalArticleCountByMenucategoryNTag(string MenuCategory)
//    {
//        CMSHelper ObjCMSHelper = new CMSHelper();
//        string ResultNid = string.Empty;
//        int RetVal = 0;
//        string DBConnectionStatusMessage = string.Empty;
//        List<System.Data.Common.DbParameter> DbParams = null;
//        DIConnection ObjDIConnection = null;
//        int TotalArticleCount = -1;
//        try
//        {

//            // Call method to get connection object
//            ObjDIConnection = ObjCMSHelper.GetConnectionObject(out DBConnectionStatusMessage);
//            // Check if connection object is not null
//            if (ObjDIConnection == null)
//            {
//                RetVal = -1;
//                return RetVal;
//            }
//            // Innitilze DbParams object
//            DbParams = new List<System.Data.Common.DbParameter>();
//            // create string MenuCategory parameter
//            System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
//            Param1.ParameterName = "@MenuCategory";
//            Param1.DbType = DbType.String;
//            Param1.Value = MenuCategory;
//            DbParams.Add(Param1);

//            // Execute stored procedure to get tag Nid
//            TotalArticleCount = Convert.ToInt32(ObjDIConnection.ExecuteScalarSqlQuery("sp_GetArticlesCountByMenuCategoryNTag", CommandType.StoredProcedure, DbParams));
//            if (TotalArticleCount > 0)
//            {
//                RetVal = TotalArticleCount;
//            }
//        }
//        catch (Exception Ex)
//        {
//            RetVal = -1;
//            Global.CreateExceptionString(Ex, null);
//        }
//        finally
//        {
//            DbParams = null;
//            ObjDIConnection = null;
//        }
//        return RetVal;
//    }



//    private List<DataContent> GetDataFromDbByMenuCategoryNTagIds(int RecordStartPosition, int NoOfRecords, string MenuCategory, string TagIds)
//    {
//        List<DataContent> RetVal = null;
//        DataContent ObjDataContent = new DataContent();
//        DIConnection ObjDIConnection = null;
//        CMSHelper Helper = new CMSHelper();
//        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
//        DataTable DtRetData = null;
//        try
//        {   //Get Connection Object
//            ObjDIConnection = Helper.GetConnectionObject();
//            // Checke if Connection  object is null then return null and stop further flow to execute
//            if (ObjDIConnection == null)
//            { return RetVal; }
//            // If Connection object is not null then excute further code 
//            else
//            {
//                // create database parameters
//                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
//                Param1.ParameterName = "@RecordStartPosition";
//                Param1.DbType = DbType.Int32;
//                Param1.Value = RecordStartPosition;
//                DbParams.Add(Param1);

//                System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter(); // create NoOfRecords parameter
//                Param2.ParameterName = "@NoOfRecords";
//                Param2.DbType = DbType.Int32;
//                Param2.Value = NoOfRecords;
//                DbParams.Add(Param2);

//                System.Data.Common.DbParameter Param3 = ObjDIConnection.CreateDBParameter(); // create MenuCategory parameter
//                Param3.ParameterName = "@MenuCategory";
//                Param3.DbType = DbType.String;
//                Param3.Value = MenuCategory;
//                DbParams.Add(Param3);


//                System.Data.Common.DbParameter Param4 = ObjDIConnection.CreateDBParameter(); // create MenuCategory parameter
//                Param4.ParameterName = "@TagIds";
//                Param4.DbType = DbType.String;
//                Param4.Value = MenuCategory;
//                DbParams.Add(Param4);

//                // Execute stored procedure to get CMS Data From Database
//                DtRetData = ObjDIConnection.ExecuteDataTable("sp_GetCMSRecordsByMenuCategory", CommandType.StoredProcedure, DbParams);
//                // Check if return datatable is not null and having atleast 1 record
//                if (DtRetData != null && DtRetData.Rows.Count > 0)
//                {
//                    RetVal = new List<DataContent>();
//                    for (int ICount = 0; ICount < DtRetData.Rows.Count; ICount++)
//                    {
//                        // Initlize class DataContent
//                        ObjDataContent = new DataContent();
//                        // innitlize values to class variables
//                        ObjDataContent.MenuCategory = DtRetData.Rows[ICount]["MenuCategory"].ToString();//DtRetData.Rows[0]["MenuCategory"]!=null? DtRetData.Rows[0]["MenuCategory"].ToString() : string.Empty;
//                        ObjDataContent.Title = DtRetData.Rows[ICount]["Title"].ToString();
//                        ObjDataContent.Thumbnail = DtRetData.Rows[ICount]["Thumbnail"].ToString();
//                        ObjDataContent.Date = Convert.ToDateTime(DtRetData.Rows[ICount]["Date"].ToString());
//                        ObjDataContent.DateAdded = Convert.ToDateTime(DtRetData.Rows[ICount]["DateAdded"]);
//                        ObjDataContent.DateModified = Convert.ToDateTime(DtRetData.Rows[ICount]["DateModified"]);
//                        ObjDataContent.Description = DtRetData.Rows[ICount]["Description"].ToString();
//                        ObjDataContent.PDFUpload = DtRetData.Rows[ICount]["PDFUpload"].ToString();
//                        ObjDataContent.Summary = DtRetData.Rows[ICount]["Summary"].ToString();
//                        ObjDataContent.URL = DtRetData.Rows[ICount]["URL"].ToString();
//                        ObjDataContent.Archived = (bool)DtRetData.Rows[ICount]["Archived"];
//                        ObjDataContent.UserNameEmail = DtRetData.Rows[ICount]["UserNameEmail"].ToString();
//                        ObjDataContent.LngCode = DtRetData.Rows[ICount]["LngCode"].ToString();
//                        ObjDataContent.ArticleTagID = (int)DtRetData.Rows[ICount]["ArticleTagId"];
//                        RetVal.Add(ObjDataContent);
//                    }
//                }
//            }
//        }
//        catch (Exception Ex)
//        {
//            RetVal = null;
//            Global.CreateExceptionString(Ex, null);
//        }

//        finally
//        {
//            ObjDIConnection = null;
//        }
//        return RetVal;
//    }



//    /// <summary>
//    /// Select all the article by menucategory from database
//    /// </summary>
//    /// <param name="MenuCategory">menucategory</param>
//    /// <returns>dictionary containing  tagid as key and tag as value</returns>
//    public Dictionary< int,string > GetAllTagsFromDBByMenuCategory(string MenuCategory)
//    {
//        CMSHelper ObjCMSHelper = null;
//        string ResultNid = string.Empty;
//        Dictionary<int, string> RetVal = null;
//        List<System.Data.Common.DbParameter> DbParams = null;
//        DIConnection ObjDIConnection = null;
//        DataTable DtTags = null;
//        int TagNid = 0;
//        string Tag = string.Empty;
//        try
//        {
//            ObjCMSHelper = new CMSHelper();
//            // Call method to get connection object
//            ObjDIConnection = ObjCMSHelper.GetConnectionObject();
//            // Check if connection object is not null
//            if (ObjDIConnection == null)
//            {
//                RetVal = null;
//                return RetVal;
//            }
//            DtTags = new DataTable();
//            RetVal = new Dictionary<int, string>();
//            // Innitilze DbParams object
//            DbParams = new List<System.Data.Common.DbParameter>();
//            // create string MenuCategory parameter
//            System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
//            Param1.ParameterName = "@MenuCategory";
//            Param1.DbType = DbType.String;
//            Param1.Value = MenuCategory;
//            DbParams.Add(Param1);

//            // Execute stored procedure to get all tag by menu category
//            DtTags = ObjDIConnection.ExecuteDataTable("sp_GetAllTagsByMenuCategory", CommandType.StoredProcedure, DbParams);
//            if (DtTags.Rows.Count > 0)
//            {
//                for (int Icount = 0; Icount < DtTags.Rows.Count; Icount++)
//                {
//                    TagNid = (int)DtTags.Rows[Icount]["Nid"];
//                    Tag = DtTags.Rows[Icount]["ArticleTag"].ToString();
//                    RetVal.Add(TagNid,Tag);
//                }
//            }
//        }
//        catch (Exception Ex)
//        {
//            RetVal = null;
//            Global.CreateExceptionString(Ex, null);
//        }
//        finally
//        {
//            DbParams = null;
//            ObjDIConnection = null;
//        }
//        return RetVal;
//    }
//}