using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.IO;
using HtmlAgilityPack;
using System.Data;
using System.Text;

/// <summary>
/// contains method for retreving articles and related information from datbase
/// </summary>
public class ArticlesGenerator
{
    #region "--Public--"

    /// <summary>
    /// Create html page containing articles based on menu category
    /// 1. get content from database for articles by menu category
    /// 2. get total record count of articles by menucatogry for pagination
    /// 3. call method to add pagination, as well define clicks of page links
    /// </summary>
    /// <param name="RecordStartPosition"> position from where to selcect first record</param>
    /// <param name="MaxArticleCount">no of articles to be retreved</param>
    /// <param name="MenuCategory">category of article</param>
    /// <returns>if record exists in database returns html containing articles, else returns empty string </returns>
    public string GetArticleByMenuCategory(int RecordStartPosition, int MaxArticleCount, string MenuCategory, int currentPageNumber, bool IsHiddenArticlesVisible)
    {
        string RetVal = string.Empty;
        List<DataContent> ListDataContent = new List<DataContent>();
        int TotalArticleCount = 0;
        try
        {
            // Call method to get data content based on url
            ListDataContent = this.GetDataContentFromDatabase_ByMenuCategory(RecordStartPosition, MaxArticleCount, MenuCategory, IsHiddenArticlesVisible);

            // Call Method to Get total article conunt by MenuCategory
            TotalArticleCount = this.GetTotalArticleCountByMenucategory(MenuCategory, IsHiddenArticlesVisible);

            //check if data content is null then return null
            if (ListDataContent == null)
            {
                return RetVal;
            }
            //If Return ObjDataContent is not null execute further code
            else
            {
                // Call method to Get Html By menu category
                RetVal = this.GetFrontPageHtmlByMenuCategoryNTag(ListDataContent, MenuCategory, MaxArticleCount, TotalArticleCount, currentPageNumber);
            }

        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);

        }
        return RetVal;
    }
    /// <summary>
    /// Create html page containing articles based on menu category
    /// 1. get content from database for articles by menu category
    /// 2. get total record count of articles by menucatogry for pagination
    /// 3. call method to add pagination, as well define clicks of page links
    /// </summary>
    /// <param name="RecordStartPosition"> position from where to selcect first record</param>
    /// <param name="MaxArticleCount">no of articles to be retreved</param>
    /// <param name="MenuCategory">category of article</param>
    /// <returns>if record exists in database returns html containing articles, else returns empty string </returns>
    public string GetArticleByMenuCategoryNTag(int RecordStartPosition, int MaxArticleCount, string MenuCategory, int currentPageNumber, string TagIds, bool IsHiddenArticlesVisible)
    {
        string RetVal = string.Empty;
        List<DataContent> ListDataContent = new List<DataContent>();
        int TotalArticleCount = 0;
        try
        {
            // Call method to get data content based on url
            ListDataContent = this.GetDataFromDbByMenuCategoryNTagIds(RecordStartPosition, MaxArticleCount, MenuCategory, TagIds, IsHiddenArticlesVisible);

            // Call Method to Get total article conunt by MenuCategory
            TotalArticleCount = this.GetTotalArticleCountByMenucategoryNTagIds(MenuCategory, TagIds, IsHiddenArticlesVisible);

            //check if data content is null then return null
            if (ListDataContent == null)
            {
                return RetVal;
            }
            //If Return ObjDataContent is not null execute further code
            else
            {
                // Call method to Get Html By menu category
                RetVal = this.GetFrontPageHtmlByMenuCategoryNTag(ListDataContent, MenuCategory, MaxArticleCount, TotalArticleCount, currentPageNumber, TagIds);
            }
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }
    /// <summary>
    /// Get CMS contenty data from database by URL
    /// </summary>
    /// <param name="Url">URL of CMS Content</param>
    /// <returns>returns Object Containing data for CMS Content if exist, else returns null</returns>
    public DataContent GetDataContentFromDatabaseByUrl(string Url, bool IsHiddenArticlesVisible)
    {
        DataContent RetVal = null;
        DIConnection ObjDIConnection = null;
        CMSHelper Helper = new CMSHelper();
        List<System.Data.Common.DbParameter> DbParams = null;
        DataTable DtRetData = null;
        try
        {   //Get Connection Object
            ObjDIConnection = Helper.GetConnectionObject();
            // Checke if Connection  object is null then return null and stop further flow to execute
            if (ObjDIConnection == null)
            { return RetVal; }
            // If Connection object is not null then excute further code 
            else
            {
                // Initlize DbParameter
                DbParams = new List<System.Data.Common.DbParameter>();
                // create tag parameter
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
                Param1.ParameterName = "@Url";
                Param1.DbType = DbType.String;
                Param1.Value = Url;
                DbParams.Add(Param1);

                // create tag parameter
                System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();
                Param2.ParameterName = "@IsHiddenArticlesVisible";
                Param2.DbType = DbType.Boolean;
                Param2.Value = IsHiddenArticlesVisible;
                DbParams.Add(Param2);

                // Execute stored procedure to get CMS Data From Database
                DtRetData = ObjDIConnection.ExecuteDataTable("sp_GetDataByUrl", CommandType.StoredProcedure, DbParams);
                // Check if return datatable is not null and having atleast 1 record
                if (DtRetData != null && DtRetData.Rows.Count > 0)
                {
                    // Initlize class DataContent
                    RetVal = new DataContent();
                    // innitlize values to class variables
                    RetVal.ContentId = (int)DtRetData.Rows[0]["ContentId"];
                    RetVal.MenuCategory = DtRetData.Rows[0]["MenuCategory"].ToString();//DtRetData.Rows[0]["MenuCategory"]!=null? DtRetData.Rows[0]["MenuCategory"].ToString() : string.Empty;
                    RetVal.Title = DtRetData.Rows[0]["Title"].ToString();
                    RetVal.Thumbnail = DtRetData.Rows[0]["Thumbnail"].ToString();
                    RetVal.Date = Convert.ToDateTime(DtRetData.Rows[0]["Date"].ToString());
                    RetVal.DateAdded = Convert.ToDateTime(DtRetData.Rows[0]["DateAdded"]);
                    RetVal.DateModified = Convert.ToDateTime(DtRetData.Rows[0]["DateModified"]);
                    RetVal.Description = DtRetData.Rows[0]["Description"].ToString();
                    RetVal.PDFUpload = DtRetData.Rows[0]["PDFUpload"].ToString();
                    RetVal.Summary = DtRetData.Rows[0]["Summary"].ToString();
                    RetVal.URL = DtRetData.Rows[0]["URL"].ToString();
                    RetVal.Archived = (bool)DtRetData.Rows[0]["Archived"];
                    RetVal.UserNameEmail = DtRetData.Rows[0]["UserNameEmail"].ToString();
                    RetVal.LngCode = DtRetData.Rows[0]["LngCode"].ToString();
                    RetVal.ArticleTagID =string.IsNullOrEmpty(DtRetData.Rows[0]["ArticleTagId"].ToString())?-1: (int)DtRetData.Rows[0]["ArticleTagId"];
                    RetVal.IsHidden = (bool)DtRetData.Rows[0]["IsHidden"];
                }
            }
        }
        catch (Exception Ex)
        {
            RetVal = null;
            Global.CreateExceptionString(Ex, null);
        }

        finally
        {
            ObjDIConnection = null;
        }
        return RetVal;
    }
    /// <summary>
    /// Gets tags from database based on TagId, using stored procedure
    /// </summary>
    /// <param name="TagId">NId of tag for which tags are to be retrieved</param>
    /// <returns>Retruns list containg tags if exise, elase returns null</returns>
    public string GetTagsFromDatabaseByTagId(int TagId)
    {
        string RetVal = string.Empty;
        List<string> RetTags = null;
        DIConnection ObjDIConnection = null;
        CMSHelper Helper = new CMSHelper();
        List<System.Data.Common.DbParameter> DbParams = null;
        DataTable DtRetData = null;
        string TagValue = string.Empty;
        try
        {   //Get Connection Object
            ObjDIConnection = Helper.GetConnectionObject();
            // Checke if Connection  object is null then return null and stop further flow to execute
            if (ObjDIConnection == null)
            { return RetVal; }
            // If Connection object is not null then excute further code 
            else
            {
                // Initlize DbParameter
                DbParams = new List<System.Data.Common.DbParameter>();
                // create tag parameter
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
                Param1.ParameterName = "@TagId";
                Param1.DbType = DbType.String;
                Param1.Value = TagId;
                DbParams.Add(Param1);

                // Execute stored procedure to get Tags From Database
                DtRetData = ObjDIConnection.ExecuteDataTable("sp_GetTagsById", CommandType.StoredProcedure, DbParams);
                // Check if return datatable is not null and having atleast 1 record
                if (DtRetData != null && DtRetData.Rows.Count > 0)
                {
                    RetTags = new List<string>();
                    // Itterate through loop
                    for (int Icount = 0; Icount < DtRetData.Rows.Count; Icount++)
                    {// Get Tag from datatable
                        TagValue = DtRetData.Rows[Icount]["ArticleTag"].ToString();
                        // assign tag value to list RetVal
                        RetTags.Add(TagValue);
                    }
                }

                if (RetTags != null)
                {
                    foreach (string Tag in RetTags)
                    {
                        RetVal = Tag + "," + RetVal;
                    }
                    RetVal = RetVal.Substring(0, RetVal.Length - 1);
                }
            }
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            ObjDIConnection = null;
        }
        return RetVal;
    }
    /// <summary>
    /// Gets Tags from database by menucategory
    /// </summary>
    /// <param name="MenuCategory">category of article</param>
    /// <returns>html string containing tags in the form of html list</returns>
    public string GetTagsListByMenuCategory(string MenuCategory, bool IsHiddenArticlesVisible)
    {
        string ResultNid = string.Empty;
        string RetVal = string.Empty;
        Dictionary<int, string> lstTags = null;
        StringBuilder sb = new StringBuilder();
        try
        {

            //Load the Menu Tags if Main content page for News , Action or Facts are shown
            if (!string.IsNullOrEmpty(MenuCategory))
            {
                //Get Tags menu for Facts
                lstTags = this.GetAllTagsFromDBByMenuCategory(MenuCategory, IsHiddenArticlesVisible);
                foreach (var item in lstTags)
                {
                    sb.Append("<input type='checkbox'  id='Chk" + item.Key + "' value='" + item.Key + "'  onclick=\"return GetArticlesByTags('" + item.Key + "');\"/><span class='news_tag_opt'>" + item.Value + "</span><br />");
                }
                if (!string.IsNullOrEmpty(sb.ToString()))
                {
                    RetVal = sb.ToString();
                    //DVTags.InnerHtml = "<span style='font-weight:bold'>Tags</span>";
                    //DVTagsMenu.InnerHtml = sb.ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            RetVal = null;
            Global.CreateExceptionString(Ex, null);
        }

        return RetVal;
    }
    /// <summary>
    /// Get pagename from database for articles by menu category
    /// </summary>
    /// <param name="MenuCategory">Input Menu Category</param>
    /// <returns>Returns pagename if pagename exists in database for input menucategory</returns>
    public string GetPageNameFromDbByMenuCategory(string MenuCategory)
    {
        string RetVal = string.Empty;
        DIConnection ObjDIConnection = null;
        CMSHelper Helper = null;
        string RetPageName = string.Empty;
        try
        {
            Helper = new CMSHelper();
            List<System.Data.Common.DbParameter> DbParams = null;
            try
            {
                //Get Connection Object
                ObjDIConnection = Helper.GetConnectionObject();
                // Checke if Connection  object is null then return null and stop further flow to execute
                if (ObjDIConnection == null)
                { return RetVal; }
                // If Connection object is not null then excute further code 
                else
                {
                    // Initlize DbParameter
                    DbParams = new List<System.Data.Common.DbParameter>();
                    // create MenuCategory parameter
                    System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
                    Param1.ParameterName = "@MenuCategory";
                    Param1.DbType = DbType.String;
                    Param1.Value = MenuCategory;
                    DbParams.Add(Param1);
                    // Execute stored procedure to get Page Name From Database
                    RetPageName = ObjDIConnection.ExecuteScalarSqlQuery("sp_GetPageNameByMenuCategory", CommandType.StoredProcedure, DbParams).ToString();
                    // Check if return datatable is not null and having atleast 1 record
                    if (!string.IsNullOrEmpty(RetPageName))
                    {
                        RetVal = RetPageName;
                    }
                }
            }
            catch (Exception Ex)
            {
                RetVal = string.Empty;
                Global.CreateExceptionString(Ex, null);
            }

            finally
            {
                ObjDIConnection = null;
            }
            return RetVal;
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    #endregion

    #region "--Private--"

    #region "--Variables--"

    internal static string ArticleContentPage = Constants.CMS.ArticleContentPage_Template;
    internal static string ArticleMainPage = Constants.CMS.DevinfoArticle_Template;
    internal static string ArticleLinkPage = Constants.CMS.ArticleLink_Template;

    #endregion

    #region "--Methods--"

    #region "--Common Methods used for retreving data based on MenuCategory And Tag--"

    /// <summary>
    /// Select all the article by menucategory from database
    /// </summary>
    /// <param name="MenuCategory">menucategory</param>
    /// <returns>dictionary containing  tagid as key and tag as value</returns>
    private Dictionary<int, string> GetAllTagsFromDBByMenuCategory(string MenuCategory, bool IsHiddenArticlesVisible)
    {
        CMSHelper ObjCMSHelper = null;
        string ResultNid = string.Empty;
        Dictionary<int, string> RetVal = null;
        List<System.Data.Common.DbParameter> DbParams = null;
        DIConnection ObjDIConnection = null;
        DataTable DtTags = null;
        int TagNid = 0;
        string Tag = string.Empty;
        try
        {
            ObjCMSHelper = new CMSHelper();
            // Call method to get connection object
            ObjDIConnection = ObjCMSHelper.GetConnectionObject();
            // Check if connection object is not null
            if (ObjDIConnection == null)
            {
                RetVal = null;
                return RetVal;
            }
            DtTags = new DataTable();
            RetVal = new Dictionary<int, string>();
            // Innitilze DbParams object
            DbParams = new List<System.Data.Common.DbParameter>();
            // create string MenuCategory parameter
            System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
            Param1.ParameterName = "@MenuCategory";
            Param1.DbType = DbType.String;
            Param1.Value = MenuCategory;
            DbParams.Add(Param1);

            // create Bool IsHidden parameter
            System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();
            Param2.ParameterName = "@IsHiddenArticlesVisible";
            Param2.DbType = DbType.Boolean;
            Param2.Value = IsHiddenArticlesVisible;
            DbParams.Add(Param2);

            // Execute stored procedure to get all tag by menu category
            DtTags = ObjDIConnection.ExecuteDataTable("sp_GetAllTagsByMenuCategory", CommandType.StoredProcedure, DbParams);
            if (DtTags.Rows.Count > 0)
            {
                for (int Icount = 0; Icount < DtTags.Rows.Count; Icount++)
                {
                    TagNid = (int)DtTags.Rows[Icount]["Nid"];
                    Tag = DtTags.Rows[Icount]["ArticleTag"].ToString();
                    RetVal.Add(TagNid, Tag);
                }
            }
        }
        catch (Exception Ex)
        {
            RetVal = null;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            DbParams = null;
            ObjDIConnection = null;
        }
        return RetVal;
    }
    /// <summary>
    /// Get Html Content Based on DataContent menu category
    /// </summary>
    /// <param name="Data">Is a Object contining records for creating CMS Template</param>
    /// <returns>String containing Html template in which input datacontent is binded</returns>
    public string GetHtmlByMenuCategory(DataContent Data)
    {
        string RetVal = string.Empty;
        try
        {
            RetVal = BindDataToTemplateByMenuCategory(Data.MenuCategory, Data);
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);

        }
        return RetVal;
    }
    /// <summary>
    /// Bind Imput datacontent to html template, according to menu category
    /// </summary>
    /// <param name="Menu">menu category of cms content</param>
    /// <param name="Data">object containing fileds for creating cms template</param>
    /// <param name="TemplateFullPath">Path of Html Template</param>
    /// <returns>Returns html template Binded with input cms content</returns>
    private string BindDataToTemplateByMenuCategory(string Menu, DataContent Data)
    {
        string RetVal = string.Empty;
        CMSHelper CmsHelp = null;
        string TemplateHtml = string.Empty;
        String TemplateFullPath = string.Empty;
        try
        {
            CmsHelp = new CMSHelper();
            TemplateFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ArticleContentPage);
            // Get template of inner Content page based on MenuCategory
            TemplateHtml = CmsHelp.GetHtmlFromPath(TemplateFullPath);

            // Set Values in Html Template
           // TemplateHtml = TemplateHtml.Replace("#Title", Data.Title);
            TemplateHtml = TemplateHtml.Replace("#MainContent", Data.Description);
            //if (!string.IsNullOrEmpty(Data.PDFUpload))
            //{
            //    TemplateHtml = TemplateHtml.Replace("#PdfUrl", Data.PDFUpload + " " + " \"target=_blank");
            //}
            //else
            //{
            //    TemplateHtml = TemplateHtml.Replace("#PdfUrl", "javascript:void(0)");
            //}
            //TemplateHtml = TemplateHtml.Replace("#Date", Data.Date.ToString());
            TemplateHtml = TemplateHtml.Replace(" #Summary", Data.Summary);

            RetVal = TemplateHtml;

        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);

        }
        return RetVal;

    }
    /// <summary>
    /// Creates HTML Template Based On URL
    /// </summary>
    /// <param name="Url">Url oF Html Template</param>
    /// <param name="MenuCategory">Out Parameter, Keeps track of MenuCategory, of return html content</param>
    /// <returns>Returns Html containd template if exist, else returns empty string</returns>
    public string CreateCMSHtmlByUrl(string Url, out string MenuCategory, out int ContentId, out string ArticleSummary, out string ArticleTitle, bool IsHiddenArticlesVisible)
    {
        string RetVal = string.Empty;
        DataContent ObjDataContent = new DataContent();
        MenuCategory = string.Empty;
        ArticleTitle = string.Empty;
        ArticleSummary = string.Empty;
        ContentId = -1;
        try
        {
            // Call method to get data content based on url
            ObjDataContent = this.GetDataContentFromDatabaseByUrl(Url, IsHiddenArticlesVisible);
            //check if data content is null then return null
            if (ObjDataContent == null)
            {
                return RetVal;
            }
            else //If Return ObjDataContent is not null execute further code
            {
                // Assign out parameter MenuCategory, to keep track MenuCategory of return html content 
                MenuCategory = ObjDataContent.MenuCategory;
                ContentId = ObjDataContent.ContentId;
                ArticleSummary = ObjDataContent.Summary;
                ArticleTitle = ObjDataContent.Title;
                // Call method to Get Html By menu category
                RetVal = this.GetHtmlByMenuCategory(ObjDataContent);
            }

        }
        catch (Exception Ex)
        {
            MenuCategory = string.Empty;
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);

        }
        return RetVal;
    }

    /// <summary>
    /// call methods to create html for main page containg articles links
    /// </summary>
    /// <param name="ListData">list containg records of articles</param>
    /// <param name="MenuCategory">Category of articles</param>
    /// <param name="MaxArticleCount">Maximum no of articles to show in main page</param>
    /// <param name="TotalArticleCount">Total articles present in database</param>
    /// <param name="currentPageNumber">Current Page no</param>
    /// <returns>html containg articles links if record exists, else return empty string</returns>
    private string GetFrontPageHtmlByMenuCategoryNTag(List<DataContent> ListData, string MenuCategory, int MaxArticleCount, int TotalArticleCount, int currentPageNumber, string TagIds = "")
    {
        string RetVal = string.Empty;
        try
        {
            RetVal = CreateArticleLinksForMainPage(MenuCategory, ListData, MaxArticleCount, TotalArticleCount, currentPageNumber, TagIds);
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);

        }
        return RetVal;
    }

    private string CreateArticleLinksForMainPage(string MenuCategory, List<DataContent> ListDataContent, int MaxArticleCount, int TotalArticleCount, int currentPageNumber, string TagIds)
    {
        string RetVal = string.Empty;
        CMSHelper CmsHelp = null;
        string TemplateHtml = string.Empty;
        String MainPageTemplateContent = string.Empty;
        String LinkTemplateContent = string.Empty;
        HtmlDocument HtmDocument = null;
        String MainPageTemplatePath = string.Empty;
        String LinkTemplatePath = string.Empty;
        string ShowHideLinkStyle = "none";
        string ShowUnHideLinkStyle = "none";
        string DeleteMethod = string.Empty;
        string HideUnHideMethod = string.Empty;
        bool IsHidden = false;
        string ShowEditOptionsStyle = "none";
        string ThumbnailDisplayStatus = string.Empty;
        string DescriptionDisplayStatus = string.Empty;
        string LinkUrl = string.Empty;
        string DateDisplayStatus = string.Empty;
        string EditLinkUrl = string.Empty;
        string AdminOptionsStyling=string.Empty;
        try
        {

            MainPageTemplatePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ArticleMainPage);
            LinkTemplatePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ArticleLinkPage);
            if (HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser] != null)
            {
                ShowEditOptionsStyle = "block";
            }
            HtmDocument = new HtmlDocument();
            HtmDocument.Load(MainPageTemplatePath);
            CmsHelp = new CMSHelper();
            // Get Content of Main Page by mainpage template path
            MainPageTemplateContent = CmsHelp.GetHtmlFromPath(MainPageTemplatePath);

            // itterate through loop
            foreach (DataContent ObjDataCont in ListDataContent)
            {
                if (HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser] != null && ObjDataCont.IsHidden)
                {
                    IsHidden = false;
                    ShowUnHideLinkStyle = "inline";
                    ShowHideLinkStyle = "none";
                }
                else if (HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser] != null && !ObjDataCont.IsHidden)
                {
                    IsHidden = true;
                    ShowHideLinkStyle = "inline";
                    ShowUnHideLinkStyle = "none";
                }

                HideUnHideMethod = "javascript:ShowHideArticlebyContentId('" + ObjDataCont.ContentId.ToString().ToLower() + "','" + IsHidden.ToString() + "','" + currentPageNumber.ToString() + "')";
                DeleteMethod = "javascript:DeleteArticlebyContentId('" + ObjDataCont.ContentId.ToString().ToLower() + "','" + currentPageNumber.ToString() + "')";
                // Get Content of Link Page by linkpage template path
                LinkTemplateContent = CmsHelp.GetHtmlFromPath(LinkTemplatePath);
                // Set Values in Html 
                EditLinkUrl = ObjDataCont.URL;
                if (string.IsNullOrEmpty(ObjDataCont.Description))
                {
                    LinkUrl = "javascript:void(0);";
                   // DescriptionDisplayStatus = "none";
                }
                else
                {
                    LinkUrl = ObjDataCont.URL;
                  //  DescriptionDisplayStatus = "block";
                }
                if (!string.IsNullOrEmpty(ObjDataCont.Thumbnail))
                {
                    AdminOptionsStyling = ShowEditOptionsStyle + "; padding-top: 42px;";
                }
                else
                {
                    AdminOptionsStyling = ShowEditOptionsStyle + "; padding-top: 0px;";
                }
                ////if (DateTime.Now.Year-ObjDataCont.Date.Value.Year>100)
                //if(ObjDataCont.Date.Value.Year.ToString()=="1900")
                //{
                //    DateDisplayStatus = "none";
                //}
                //else
                //{
                //    DateDisplayStatus = "block";
                //}
                if (!string.IsNullOrEmpty(TagIds))
                {
                    TagIds = TagIds.Replace(',', '~');
                    LinkTemplateContent = string.Format(LinkTemplateContent, LinkUrl, ObjDataCont.Title, ObjDataCont.Date, ObjDataCont.Thumbnail, ObjDataCont.Summary, AdminOptionsStyling, ShowUnHideLinkStyle, ShowHideLinkStyle, HideUnHideMethod, DeleteMethod, currentPageNumber, "&AT=" + TagIds, EditLinkUrl);
                }
                else
                {
                    LinkTemplateContent = string.Format(LinkTemplateContent, LinkUrl, ObjDataCont.Title, ObjDataCont.Date, ObjDataCont.Thumbnail, ObjDataCont.Summary, AdminOptionsStyling, ShowUnHideLinkStyle, ShowHideLinkStyle, HideUnHideMethod, DeleteMethod, currentPageNumber, string.Empty, EditLinkUrl);
                }
                
                HtmDocument = this.AddHtmlLinkContentToMainPage(HtmDocument, LinkTemplateContent);

            }

            // Add pager
            if (TotalArticleCount > 1)
            {
                HtmDocument = this.AddPaginationToMainPage(HtmDocument, LinkTemplateContent, MaxArticleCount, TotalArticleCount, MenuCategory, currentPageNumber);
            }

            RetVal = HtmDocument.DocumentNode.InnerHtml;
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);

        }
        return RetVal;

    }

    private HtmlDocument AddHtmlLinkContentToMainPage(HtmlDocument Document, string LinkContent)
    {
        HtmlDocument RetVal = null;
        try
        {
            // Targets a specific node(since all the page contains parent table tag with width 700)
            HtmlNode content_wrapper = Document.DocumentNode.SelectNodes("//table[@id='MainTableForNewsList']").FirstOrDefault();

            HtmlNode tr = Document.CreateElement("tr");
            content_wrapper.ChildNodes.Append(tr);
            HtmlNode td = Document.CreateElement("td");
            td.InnerHtml = LinkContent;
            tr.ChildNodes.Append(td);

            RetVal = Document;
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
        // return html document
        return RetVal;
    }

    /// <summary>
    /// Addd pagination to main Page
    /// </summary>
    /// <param name="Document">Html document in which, pagination is to add</param>
    /// <param name="LinkContent">Link </param>
    /// <param name="MaxArticleCount">Maximum no of articles to show in main page</param>
    /// <param name="TotalArticleCount">Total articles present in database</param>
    /// <param name="MenuCategory">Category of articles</param>
    /// <param name="currentPageNumber">Current Page no</param>
    /// <param name="totalPagingMenuLength">Optional parameter for PagingMenu by default its 5</param>
    /// <returns></returns>
    private HtmlDocument AddPaginationToMainPage(HtmlDocument Document, string LinkContent, int MaxArticleCount, int TotalArticleCount, string MenuCategory, int currentPageNumber, int totalPagingMenuLength = 10)
    {        
        HtmlDocument RetVal = null;
        int PageCount = 0;
        int Icount = 1;
        int StartIndex = 1;
        int LastIndex = -1;
        int HalfPageSize = totalPagingMenuLength / 2;
        try
        {
            // Targets a specific node(since all the page contains parent table tag with width 700)
            HtmlNode content_wrapper = Document.DocumentNode.SelectNodes("//div[@id='PagerTr']").FirstOrDefault();
            if (TotalArticleCount % MaxArticleCount == 0)
            {
                PageCount = TotalArticleCount / MaxArticleCount;
            }
            else
            {
                PageCount = TotalArticleCount / MaxArticleCount + 1;
            }

            LastIndex = PageCount;
            //Get Current Menu Numbers
            if (currentPageNumber <= HalfPageSize)
            {
                if (PageCount > totalPagingMenuLength)
                {
                    LastIndex = totalPagingMenuLength;
                }
                else
                {
                    LastIndex = PageCount;
                }
            }
            else
            {
                if (currentPageNumber > (PageCount - HalfPageSize))
                {
                    if (PageCount > totalPagingMenuLength)
                    {
                        StartIndex = PageCount - totalPagingMenuLength;
                    }
                }
                else
                {
                    if (PageCount > totalPagingMenuLength)
                    {
                        StartIndex = currentPageNumber - HalfPageSize;
                        LastIndex = currentPageNumber + HalfPageSize;
                    }
                }
            }
            //Make the Start and Move Previous buttons
            //Move First
            HtmlNode InputTag;
            HtmlNode AnchorTag;
            HtmlNode divTag;
            divTag = Document.CreateElement("div");
            divTag.SetAttributeValue("class", "pagging_div");
            divTag.SetAttributeValue("id", "divPaging");
            CreateNextPreviousButtons(Document, content_wrapper, out InputTag, "<<", currentPageNumber, PageCount, MenuCategory);
            //Move Previous
            CreateNextPreviousButtons(Document, content_wrapper, out InputTag, "<", currentPageNumber, PageCount, MenuCategory);
            for (Icount = StartIndex; Icount <= LastIndex; Icount++)
            {
                AnchorTag = Document.CreateElement("a");

                AnchorTag.SetAttributeValue("href", "javascript:ShowArticleByPage('" + Icount + "');");
                AnchorTag.SetAttributeValue("pageno", Icount.ToString());
                if (currentPageNumber == Icount)
                {
                    AnchorTag.SetAttributeValue("class", "pagination_lnk page_selected");
                    AnchorTag.InnerHtml = "<b>" + Icount.ToString() + "</b>";
                }
                else
                {
                    AnchorTag.SetAttributeValue("class", "pagination_lnk");
                    AnchorTag.InnerHtml = Icount.ToString();
                }

                divTag.ChildNodes.Append(AnchorTag);
            }

            content_wrapper.ChildNodes.Append(divTag);
            //Move Next
            CreateNextPreviousButtons(Document, content_wrapper, out InputTag, ">", currentPageNumber, PageCount, MenuCategory);
            //Move Last
            CreateNextPreviousButtons(Document, content_wrapper, out InputTag, ">>", currentPageNumber, PageCount, MenuCategory);

            RetVal = Document;
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
        // return html document
        return RetVal;
    }
    /// <summary>
    /// Creates Links for Previous and Next in pager
    /// </summary>
    /// <param name="Document">Html document in which, Previous and Next is to add</param>
    /// <param name="content_wrapper">Html document node in which Previous and Next is to add</param>
    /// <param name="InputTag">Html document node containg tag for Previous and Next</param>
    /// <param name="AnchorTag"></param>
    /// <param name="symbol"></param>
    /// <param name="currentPageNumber"></param>
    /// <param name="pageSize"></param>
    /// <param name="MenuCategory"></param>
    private static void CreateNextPreviousButtons(HtmlDocument Document, HtmlNode content_wrapper, out HtmlNode InputTag, string symbol, int currentPageNumber, int pageSize, string MenuCategory)
    {
        InputTag = Document.CreateElement("input");
       // InputTag.SetAttributeValue("type", "button");
        string PageNameRequesting = string.Empty;
        if (symbol == "<<")
        {
            InputTag.SetAttributeValue("class", "dbl_lft_arrw pagging_marg_rt_fr");
            InputTag.SetAttributeValue("onclick", "ShowArticleByPage('" + 1 + "');");
            if (currentPageNumber == 1)
            {
                InputTag.SetAttributeValue("disabled", "disabled");
            }
        }
        else if (symbol == "<")
        {
            InputTag.SetAttributeValue("class", "sngl_lft_arrw pagging_marg_rt_tw");
            if (currentPageNumber == 1)
            {
                InputTag.SetAttributeValue("disabled", "disabled");
            }
            else
            {
                InputTag.SetAttributeValue("onclick", "ShowArticleByPage('" + (currentPageNumber - 1) + "');");
            }
        }
        else if (symbol == ">")
        {
            InputTag.SetAttributeValue("class", "sngl_rgt_arrw pagging_marg_lt_tw");
            if (currentPageNumber == pageSize)
            {
                InputTag.SetAttributeValue("disabled", "disabled");
            }
            else
            {
                InputTag.SetAttributeValue("onclick", "ShowArticleByPage('" + (currentPageNumber + 1) + "');");
            }
        }
        else if (symbol == ">>")
        {
            InputTag.SetAttributeValue("class", "dbl_rgt_arrw pagging_marg_lt_fr");
            InputTag.SetAttributeValue("onclick", "ShowArticleByPage('" + pageSize + "');");
            if (currentPageNumber == pageSize)
            {
                InputTag.SetAttributeValue("disabled", "disabled");
            }
        }
        content_wrapper.ChildNodes.Append(InputTag);
    }

    #endregion

    #region "--Methods for retreving data based on MenuCategory Only--"
    /// <summary>
    /// Execute stored procedure to get data from database By MenuCategory
    /// </summary>
    /// <param name="RecordStartPosition">Strarting position from where to retreve record</param>
    /// <param name="NoOfRecords">No of records to retreve from database</param>
    /// <param name="MenuCategory">Category of articles</param>
    /// <returns>List containing articles records based on MenuCategory if exists, else return null</returns>
    private List<DataContent> GetDataContentFromDatabase_ByMenuCategory(int RecordStartPosition, int NoOfRecords, string MenuCategory, bool IsHiddenArticlesVisible)
    {
        List<DataContent> RetVal = null;
        DataContent ObjDataContent = new DataContent();
        DIConnection ObjDIConnection = null;
        CMSHelper Helper = new CMSHelper();
        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        DataTable DtRetData = null;
        try
        {   //Get Connection Object
            ObjDIConnection = Helper.GetConnectionObject();
            // Checke if Connection  object is null then return null and stop further flow to execute
            if (ObjDIConnection == null)
            { return RetVal; }
            // If Connection object is not null then excute further code 
            else
            {
                // create database parameters
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param1.ParameterName = "@RecordStartPosition";
                Param1.DbType = DbType.Int32;
                Param1.Value = RecordStartPosition;
                DbParams.Add(Param1);

                System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter(); // create NoOfRecords parameter
                Param2.ParameterName = "@NoOfRecords";
                Param2.DbType = DbType.Int32;
                Param2.Value = NoOfRecords;
                DbParams.Add(Param2);

                System.Data.Common.DbParameter Param3 = ObjDIConnection.CreateDBParameter(); // create MenuCategory parameter
                Param3.ParameterName = "@MenuCategory";
                Param3.DbType = DbType.String;
                Param3.Value = MenuCategory;
                DbParams.Add(Param3);

                System.Data.Common.DbParameter Param4 = ObjDIConnection.CreateDBParameter(); // create MenuCategory parameter
                Param4.ParameterName = "@IsHiddenArticlesVisible";
                Param4.DbType = DbType.Boolean;
                Param4.Value = IsHiddenArticlesVisible;
                DbParams.Add(Param4);


                // Execute stored procedure to get CMS Data From Database
                DtRetData = ObjDIConnection.ExecuteDataTable("sp_GetArticlesByMenuCategory", CommandType.StoredProcedure, DbParams);
                // Check if return datatable is not null and having atleast 1 record
                if (DtRetData != null && DtRetData.Rows.Count > 0)
                {
                    RetVal = new List<DataContent>();
                    for (int ICount = 0; ICount < DtRetData.Rows.Count; ICount++)
                    {
                        // Initlize class DataContent
                        ObjDataContent = new DataContent();
                        // innitlize values to class variables
                        ObjDataContent.ContentId = (int)DtRetData.Rows[ICount]["ContentId"];
                        ObjDataContent.MenuCategory = DtRetData.Rows[ICount]["MenuCategory"].ToString();//DtRetData.Rows[0]["MenuCategory"]!=null? DtRetData.Rows[0]["MenuCategory"].ToString() : string.Empty;
                        ObjDataContent.Title = DtRetData.Rows[ICount]["Title"].ToString();
                        ObjDataContent.Thumbnail = DtRetData.Rows[ICount]["Thumbnail"].ToString();
                        ObjDataContent.Date = Convert.ToDateTime(DtRetData.Rows[ICount]["Date"].ToString());
                        ObjDataContent.DateAdded = Convert.ToDateTime(DtRetData.Rows[ICount]["DateAdded"]);
                        ObjDataContent.DateModified = Convert.ToDateTime(DtRetData.Rows[ICount]["DateModified"]);
                        ObjDataContent.Description = DtRetData.Rows[ICount]["Description"].ToString();
                        ObjDataContent.PDFUpload = DtRetData.Rows[ICount]["PDFUpload"].ToString();
                        ObjDataContent.Summary = DtRetData.Rows[ICount]["Summary"].ToString();
                        ObjDataContent.URL = DtRetData.Rows[ICount]["URL"].ToString();
                        ObjDataContent.Archived = (bool)DtRetData.Rows[ICount]["Archived"];
                        ObjDataContent.UserNameEmail = DtRetData.Rows[ICount]["UserNameEmail"].ToString();
                        ObjDataContent.LngCode = DtRetData.Rows[ICount]["LngCode"].ToString();
                        ObjDataContent.ArticleTagID = string.IsNullOrEmpty(DtRetData.Rows[ICount]["ArticleTagId"].ToString())? -1 : (int)DtRetData.Rows[ICount]["ArticleTagId"];
                        ObjDataContent.IsHidden = (bool)DtRetData.Rows[ICount]["IsHidden"];
                        RetVal.Add(ObjDataContent);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            RetVal = null;
            Global.CreateExceptionString(Ex, null);
        }

        finally
        {
            ObjDIConnection = null;
        }
        return RetVal;
    }
    /// <summary>
    /// Gets count maximum no of articles by MenuCategory
    /// </summary>
    /// <param name="MenuCategory">Input MenuCategory</param>
    /// <returns>TotalArticleCount</returns>
    private int GetTotalArticleCountByMenucategory(string MenuCategory, bool IsHiddenArticlesVisible)
    {
        CMSHelper ObjCMSHelper = new CMSHelper();
        string ResultNid = string.Empty;
        int RetVal = 0;
        string DBConnectionStatusMessage = string.Empty;
        List<System.Data.Common.DbParameter> DbParams = null;
        DIConnection ObjDIConnection = null;
        int TotalArticleCount = -1;
        try
        {

            // Call method to get connection object
            ObjDIConnection = ObjCMSHelper.GetConnectionObject(out DBConnectionStatusMessage);
            // Check if connection object is not null
            if (ObjDIConnection == null)
            {
                RetVal = -1;
                return RetVal;
            }
            // Innitilze DbParams object
            DbParams = new List<System.Data.Common.DbParameter>();
            // create string MenuCategory parameter
            System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
            Param1.ParameterName = "@MenuCategory";
            Param1.DbType = DbType.String;
            Param1.Value = MenuCategory;
            DbParams.Add(Param1);

            System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();
            Param2.ParameterName = "@IsHiddenArticlesVisible";
            Param2.DbType = DbType.Boolean;
            Param2.Value = IsHiddenArticlesVisible;
            DbParams.Add(Param2);

            // Execute stored procedure to get tag Nid
            TotalArticleCount = Convert.ToInt32(ObjDIConnection.ExecuteScalarSqlQuery("sp_GetArticlesCountByMenuCategory", CommandType.StoredProcedure, DbParams));
            if (TotalArticleCount > 0)
            {
                RetVal = TotalArticleCount;
            }
        }
        catch (Exception Ex)
        {
            RetVal = -1;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            DbParams = null;
            ObjDIConnection = null;
        }
        return RetVal;
    }
    #endregion

    #region "--Methods for retreving data based on MenuCategory And Tag--"

    /// <summary>
    /// Execute stored procedure to get data from database ByMenu Category and Tags
    /// </summary>
    /// <param name="RecordStartPosition"></param>
    /// <param name="NoOfRecords">No of records to retreve from database</param>
    /// <param name="MenuCategory">Category of articles</param>
    /// <param name="TagIds">Ids of tags seprated with ',' </param>
    /// <returns>List containing articles records based on MenuCategoryand Tags if exists, else return null</returns>
    private List<DataContent> GetDataFromDbByMenuCategoryNTagIds(int RecordStartPosition, int NoOfRecords, string MenuCategory, string TagIds, bool IsHiddenArticlesVisible)
    {
        List<DataContent> RetVal = null;
        DataContent ObjDataContent = new DataContent();
        DIConnection ObjDIConnection = null;
        CMSHelper Helper = new CMSHelper();
        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        DataTable DtRetData = null;
        try
        {   //Get Connection Object
            ObjDIConnection = Helper.GetConnectionObject();
            // Checke if Connection  object is null then return null and stop further flow to execute
            if (ObjDIConnection == null)
            { return RetVal; }
            // If Connection object is not null then excute further code 
            else
            {
                // create database parameters
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();// create RecordStartPosition parameter
                Param1.ParameterName = "@RecordStartPosition";
                Param1.DbType = DbType.Int32;
                Param1.Value = RecordStartPosition;
                DbParams.Add(Param1);

                System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter(); // create NoOfRecords parameter
                Param2.ParameterName = "@NoOfRecords";
                Param2.DbType = DbType.Int32;
                Param2.Value = NoOfRecords;
                DbParams.Add(Param2);

                System.Data.Common.DbParameter Param3 = ObjDIConnection.CreateDBParameter(); // create MenuCategory parameter
                Param3.ParameterName = "@MenuCategory";
                Param3.DbType = DbType.String;
                Param3.Value = MenuCategory;
                DbParams.Add(Param3);

                System.Data.Common.DbParameter Param4 = ObjDIConnection.CreateDBParameter(); // create MenuCategory parameter
                Param4.ParameterName = "@TagIds";
                Param4.DbType = DbType.String;
                Param4.Value = TagIds;
                DbParams.Add(Param4);

                System.Data.Common.DbParameter Param5 = ObjDIConnection.CreateDBParameter(); // create MenuCategory parameter
                Param5.ParameterName = "@IsHiddenArticlesVisible";
                Param5.DbType = DbType.Boolean;
                Param5.Value = IsHiddenArticlesVisible;
                DbParams.Add(Param5);


                // Execute stored procedure to get CMS Data From Database
                DtRetData = ObjDIConnection.ExecuteDataTable("sp_GetArticlesByMenuCategoryNTags", CommandType.StoredProcedure, DbParams);
                // Check if return datatable is not null and having atleast 1 record
                if (DtRetData != null && DtRetData.Rows.Count > 0)
                {
                    RetVal = new List<DataContent>();
                    for (int ICount = 0; ICount < DtRetData.Rows.Count; ICount++)
                    {
                        // Initlize class DataContent
                        ObjDataContent = new DataContent();
                        // innitlize values to class variables
                        ObjDataContent.ContentId = (int)DtRetData.Rows[ICount]["ContentId"];
                        ObjDataContent.MenuCategory = DtRetData.Rows[ICount]["MenuCategory"].ToString();//DtRetData.Rows[0]["MenuCategory"]!=null? DtRetData.Rows[0]["MenuCategory"].ToString() : string.Empty;
                        ObjDataContent.Title = DtRetData.Rows[ICount]["Title"].ToString();
                        ObjDataContent.Thumbnail = DtRetData.Rows[ICount]["Thumbnail"].ToString();
                        ObjDataContent.Date = Convert.ToDateTime(DtRetData.Rows[ICount]["Date"].ToString());
                        ObjDataContent.DateAdded = Convert.ToDateTime(DtRetData.Rows[ICount]["DateAdded"]);
                        ObjDataContent.DateModified = Convert.ToDateTime(DtRetData.Rows[ICount]["DateModified"]);
                        ObjDataContent.Description = DtRetData.Rows[ICount]["Description"].ToString();
                        ObjDataContent.PDFUpload = DtRetData.Rows[ICount]["PDFUpload"].ToString();
                        ObjDataContent.Summary = DtRetData.Rows[ICount]["Summary"].ToString();
                        ObjDataContent.URL = DtRetData.Rows[ICount]["URL"].ToString();
                        ObjDataContent.Archived = (bool)DtRetData.Rows[ICount]["Archived"];
                        ObjDataContent.UserNameEmail = DtRetData.Rows[ICount]["UserNameEmail"].ToString();
                        ObjDataContent.LngCode = DtRetData.Rows[ICount]["LngCode"].ToString();
                        ObjDataContent.ArticleTagID = (int)DtRetData.Rows[ICount]["ArticleTagId"];
                        ObjDataContent.IsHidden = (bool)DtRetData.Rows[ICount]["IsHidden"];
                        RetVal.Add(ObjDataContent);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            RetVal = null;
            Global.CreateExceptionString(Ex, null);
        }

        finally
        {
            ObjDIConnection = null;
        }
        return RetVal;
    }
    /// <summary>
    /// Gets count maximum no of articles by MenuCategory and TagId
    /// </summary>
    /// <param name="MenuCategory">Input MenuCategory</param>
    /// <returns>TotalArticleCount</returns>
    private int GetTotalArticleCountByMenucategoryNTagIds(string MenuCategory, string TagIds, bool IsHiddenArticlesVisible)
    {
        CMSHelper ObjCMSHelper = new CMSHelper();
        string ResultNid = string.Empty;
        int RetVal = 0;
        List<System.Data.Common.DbParameter> DbParams = null;
        DIConnection ObjDIConnection = null;
        int TotalArticleCount = -1;
        try
        {

            // Call method to get connection object
            ObjDIConnection = ObjCMSHelper.GetConnectionObject();
            // Check if connection object is not null
            if (ObjDIConnection == null)
            {
                RetVal = -1;
                return RetVal;
            }
            // Innitilze DbParams object
            DbParams = new List<System.Data.Common.DbParameter>();
            // create string MenuCategory parameter
            System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
            Param1.ParameterName = "@MenuCategory";
            Param1.DbType = DbType.String;
            Param1.Value = MenuCategory;
            DbParams.Add(Param1);

            System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();
            Param2.ParameterName = "@TagIds";
            Param2.DbType = DbType.String;
            Param2.Value = TagIds;
            DbParams.Add(Param2);

            System.Data.Common.DbParameter Param3 = ObjDIConnection.CreateDBParameter();
            Param3.ParameterName = "@IsHiddenArticlesVisible";
            Param3.DbType = DbType.Boolean;
            Param3.Value = IsHiddenArticlesVisible;
            DbParams.Add(Param3);

            // Execute stored procedure to get tag Nid
            TotalArticleCount = Convert.ToInt32(ObjDIConnection.ExecuteScalarSqlQuery("sp_GetArticlesCountByMenuCategoryNTag", CommandType.StoredProcedure, DbParams));
            if (TotalArticleCount > 0)
            {
                RetVal = TotalArticleCount;
            }
        }
        catch (Exception Ex)
        {
            RetVal = -1;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            DbParams = null;
            ObjDIConnection = null;
        }
        return RetVal;
    }
    #endregion
    #endregion

    #endregion
}