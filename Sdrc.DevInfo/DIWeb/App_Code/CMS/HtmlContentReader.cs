using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HtmlAgilityPack;
using System.IO;
using System.Data.SqlTypes;
using System.Globalization;
using System.Text.RegularExpressions;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Data;

/// <summary>
/// Reads Html Content and returns data as list
/// </summary>
public class HtmlContentReader
{

    #region "Privare Variables"
    /// <summary>
    /// Sets news html filename prefix. Default is "di_news_"
    /// </summary>
    internal string NewsHtmlFilenamePrefix = "di_news";

    /// <summary>
    /// Sets facts html filename prefix. Default is "di_facts_goal"
    /// </summary>
    internal string FactsHtmlFilenamePrefix = "di_facts";

    /// <summary>
    /// Sets devinfoaction html filename prefix. Default is "devinfo_in_action"
    /// </summary>
    internal string ActionHtmlFilenamePrefix = "devinfo_in_action";
    #endregion



    // Read News content
    public List<DataContent> ReadNewsHtmlContent()
    {
        List<DataContent> ListDC = new List<DataContent>();

        string ContentFileFullPath = string.Empty;
        string HtmlPageFullPath = string.Empty;
        string HtmlPageName = string.Empty;
        HtmlDocument document = null;

        SqlDateTime SqlDtTime;
        CMSHelper ObjCMSHelper = null;
        try
        {
            // itterate through loop for all news pages
            for (int iCount = 1; iCount < 11; iCount++)
            {
                if (iCount == 1)
                {
                    //get name of news html page
                    HtmlPageName = NewsHtmlFilenamePrefix  + ".html";
                }
                else
                {
                    //get name of news html page
                    HtmlPageName = NewsHtmlFilenamePrefix +"_" + iCount + ".html";
                }

                document = new HtmlDocument();
                // create html page full path 
                HtmlPageFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.DiorgContent, HtmlPageName);
                if (!File.Exists(HtmlPageFullPath))
                {
                    continue;
                }
                // load html page
                document.Load(HtmlPageFullPath);
                // Targets a specific node(since all the page contains parent table tag with width 700)
                HtmlNode content_wrapper = document.DocumentNode.SelectNodes("//table[@width='700']").FirstOrDefault();
                // get html news collection
                HtmlNodeCollection search_results = content_wrapper.SelectNodes("//td[@class='news_space']");
                //itterate through loop
                foreach (HtmlNode result in search_results)
                {
                    string PdfFileUrl = string.Empty;
                    string RetPageContent = string.Empty;
                    string ContentSummary = string.Empty;
                    string ContentTitle = string.Empty;
                    string ContentDate = string.Empty;
                    string ImgUrl = string.Empty;
                    string ContentPageUrl = string.Empty;
                    string UserEmailID = string.Empty;
                    string DatabaseLanguage = string.Empty;
                    string UserName = string.Empty;
                    string CurrentDbLangCode = string.Empty;
                    string ContentUrl = string.Empty;
                    DataContent Content = new DataContent();
                    // get title of news

                    if (result.SelectNodes(".//td//span[@class='header']//a") != null && !string.IsNullOrEmpty(result.SelectNodes(".//td//span[@class='header']//a").FirstOrDefault().InnerText.Trim()))
                    {
                        ContentTitle = result.SelectNodes(".//td//span[@class='header']//a").FirstOrDefault().InnerText.Trim();
                    }
                    else if (result.SelectNodes(".//p//span[@class='header']") != null)
                    {
                        ContentTitle = result.SelectNodes(".//p//span[@class='header']").FirstOrDefault().InnerText.Trim();
                    }
                    else if (result.SelectNodes(".//span//strong//a") != null)
                    {
                        ContentTitle = result.SelectNodes(".//span//strong//a").FirstOrDefault().InnerText.Trim();
                    }
                    else if (result.SelectNodes(".//p//strong//a") != null)
                    {
                        ContentTitle = result.SelectNodes(".//p//strong//a").FirstOrDefault().InnerText.Trim();
                    }
                    else if (result.SelectNodes(".//td[@valign='top']//strong//a") != null)
                    {
                        ContentTitle = result.SelectNodes(".//td[@valign='top']//strong//a").FirstOrDefault().InnerText.Trim();
                    }
                    else if (result.SelectNodes(".//td[@valign='top']//p//strong//a") != null)
                    {
                        ContentTitle = result.SelectNodes(".//td[@valign='top']//p//strong//a").FirstOrDefault().InnerText.Trim();
                    }
                    else if (result.SelectNodes(".//td[@valign='top']//p//font//a") != null)
                    {
                        ContentTitle = result.SelectNodes(".//td[@valign='top']//p//font//a").FirstOrDefault().InnerText.Trim();
                    }
                    else if (result.SelectNodes(".//td[@valign='top']//p//a//strong") != null)
                    {
                        ContentTitle = result.SelectNodes(".//td[@valign='top']//p//a//strong").FirstOrDefault().InnerText.Trim();
                    }
                    // get summary of news
                    if (result.SelectNodes(".//td[@width='71%']//tr//td") != null)
                    {
                        ContentSummary = result.SelectNodes(".//td[@width='71%']//tr//td").ElementAt(1).InnerText.Trim().ToString();
                    }
                    // get summary of news
                    else if (result.SelectNodes(".//td[@height='68%']//tr//td") != null)
                    {
                        ContentSummary = result.SelectNodes(".//td[@height='68%']//tr//td").ElementAt(1).InnerText.Trim().ToString();
                    }
                    if (ContentSummary.Contains("more") && ContentSummary.Contains("&nbsp;") || string.IsNullOrEmpty(ContentSummary))
                    {
                        if (result.SelectNodes(".//td[@height='68%']//tr//td") != null)
                        {
                            ContentSummary = result.SelectNodes(".//td[@height='68%']//tr/td").FirstOrDefault().InnerText.Trim();
                        }

                        else if (result.SelectNodes(".//td[@valign='top']//p//strong") != null)
                        {
                            ContentSummary = result.SelectNodes(".//td[@valign='top']//p//strong").ElementAt(1).InnerText.Trim().ToString();
                        }
                    }
                    // get date created of news
                    if (result.SelectSingleNode(".//em") != null)
                    {
                        ContentDate = result.SelectSingleNode(".//em").InnerText.Replace("(", "").Replace(")", "").Trim();
                    }
                    else if (result.SelectSingleNode(".//span[@class='adaptation_italic'] ") != null)
                    {
                        ContentDate = result.SelectSingleNode(".//span[@class='adaptation_italic']").InnerText.Replace("(", "").Replace(")", "").Trim();

                    }
                    ContentDate = ContentDate.Replace("&nbsp;", "");
                    // get Thumbnail url of news
                    if (result.SelectNodes(".//img[@class='reflect ropacity30 ']") != null)
                    {
                        ImgUrl = result.SelectNodes(".//img[@class='reflect ropacity30 ']").FirstOrDefault().GetAttributeValue("src", "").ToString();
                    }

                    else if (result.SelectNodes(".//img[@class='reflect ropacity30']") != null)
                    {
                        ImgUrl = result.SelectNodes(".//img[@class='reflect ropacity30']").FirstOrDefault().GetAttributeValue("src", "").ToString();
                    }

                    else if (result.SelectNodes(".//td//a//img[@width='140']") != null)
                    {
                        ImgUrl = result.SelectNodes(".//td//a//img[@width='140']").FirstOrDefault().GetAttributeValue("src", "").ToString();
                    }
                    if (ImgUrl.Contains("diorg/"))
                    {
                        ImgUrl = ImgUrl.Replace("diorg/", "../libraries/aspx/diorg/");
                    }
                    // get content page of url
                    if (result.SelectNodes(".//td[@width='29%']//a") != null)
                    {
                        ContentPageUrl = result.SelectNodes(".//td[@width='29%']//a").FirstOrDefault().GetAttributeValue("href", "").ToString();
                    }
                    else if (result.SelectNodes(".//td[@width='29%']//tr/td//a") != null)
                    {
                        ContentPageUrl = result.SelectNodes(".//td[@width='29%']//tr/td//a").FirstOrDefault().GetAttributeValue("href", "").ToString();
                    }
                    else if (result.SelectNodes(".//td[@width='28%']//a") != null)
                    {
                        ContentPageUrl = result.SelectNodes(".//td[@width='28%']//a").FirstOrDefault().GetAttributeValue("href", "").ToString();
                    }
                    else if (result.SelectNodes(".//td[@width='28%']//tr//td//a") != null)
                    {
                        ContentPageUrl = result.SelectNodes(".//td[@width='28%']//tr//td//a").FirstOrDefault().GetAttributeValue("href", "").ToString();
                    }
                    else if (result.SelectNodes(".//td[@valign='top']//p//strong//a") != null)
                    {
                        ContentPageUrl = result.SelectNodes(".//td[@valign='top']//p//strong//a").FirstOrDefault().GetAttributeValue("href", "").ToString();
                    }
                    //get content of inner html page
                    if (ContentPageUrl.Contains("diorg/"))
                    {
                        // create full path of inner html page
                        ContentFileFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.DiorgContent,
                            Global.SplitString(ContentPageUrl, "diorg/")[1].ToString().Replace(@"/", "\\"));
                        // check if file exist
                        if (File.Exists(ContentFileFullPath))
                        {
                            // call method to get content of html page
                            //get pdfurl as out parameter
                            RetPageContent = GetInnerContentNPdfUrl(ContentFileFullPath, out PdfFileUrl);
                        }
                    }

                    // Initlize object of class cmshelper 
                    ObjCMSHelper = new CMSHelper();

                    // remove space and ;nbsp from date
                    ContentDate = ObjCMSHelper.RemoveExtraCharsFromDate(ContentDate);
                    // get created date of news
                    SqlDtTime = new SqlDateTime(ObjCMSHelper.GetSqlDataTimeFromInptDate(ContentDate));

                    // get user email id from session
                    if (!string.IsNullOrEmpty(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUserNId].ToString()))
                    {
                        UserEmailID = Global.Get_User_EmailId_ByAdaptationURL(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUserNId].ToString());
                    }
                    // get user name from session
                    if (!string.IsNullOrEmpty(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser].ToString()))
                    {
                        UserName = HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser].ToString();
                    }

                    // Create url from title
                    ContentUrl = ObjCMSHelper.CreateUrlFromInputString(ContentTitle);

                    //Get language of current database 
                    CurrentDbLangCode = Global.GetDefaultLanguageCode();

                    // innitlize of members of class Content
                    Content.MenuCategory = "News";
                    Content.Date = SqlDtTime;
                    Content.DateAdded = new SqlDateTime(DateTime.Now);
                    Content.DateModified = new SqlDateTime(DateTime.Now);
                    Content.Description = RetPageContent;
                    Content.Title = ContentTitle;
                    Content.PDFUpload = PdfFileUrl;
                    Content.Summary = ContentSummary;
                    Content.Thumbnail = ImgUrl;
                    Content.URL = ContentUrl;
                    Content.Archived = false;
                    Content.UserNameEmail = UserName + " " + UserEmailID;
                    Content.LngCode = CurrentDbLangCode;
                    Content.ArticleTagID = -1;
                    // all class Content to list
                    ListDC.Add(Content);
                }
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
        // return list containing all html pages
        return ListDC;
    }

    // Read Action page html Content
    public List<DataContent> ReadActionHtmlContent()
    {
        List<DataContent> ListDC = new List<DataContent>();

        SqlDateTime SqlDtTime = new SqlDateTime();
        CMSHelper ObjCMSHelper = null;
        try
        {
            string PdfFileUrl = string.Empty;
            string RetPageContent = string.Empty;
            string ContentFileFullPath = string.Empty;
            string HtmlPageFullPath = string.Empty;
            string HtmlPageName = string.Empty;
            HtmlDocument document = null;
            string ContentSummary = string.Empty;
            string ContentTitle = string.Empty;
            string ContentDate = string.Empty;
            string ImgUrl = string.Empty;
            string ContentPageUrl = string.Empty;
            string UserEmailID = string.Empty;
            string DatabaseLanguage = string.Empty;
            string UserName = string.Empty;
            string CurrentDbLangCode = string.Empty;
            string CountentUrl = string.Empty;
            string ArticleTags = string.Empty;
            //get name of news html page
            HtmlPageName = this.ActionHtmlFilenamePrefix + ".html";
            document = new HtmlDocument();
            // create html page full path 
            HtmlPageFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.DiorgContent, HtmlPageName);
            if (!File.Exists(HtmlPageFullPath))
            {
                return ListDC;
            }
            // load html page
            document.Load(HtmlPageFullPath);
            // Targets a specific node(since all the page contains parent table tag with width 700)
            HtmlNode content_wrapper = document.DocumentNode.SelectNodes("//table[@width='700']").FirstOrDefault();
            // get html news collection
            HtmlNodeCollection search_results = content_wrapper.SelectNodes("//td[@class='news_space']");
            //itterate through loop
            foreach (HtmlNode result in search_results)
            {
                DataContent Content = new DataContent();

                // get title of news
                if (result.SelectNodes(".//p//span[@class='header']") != null && !string.IsNullOrEmpty(result.SelectNodes(".//p//span[@class='header']").FirstOrDefault().InnerText))
                {
                    ContentTitle = result.SelectNodes(".//p//span[@class='header']").FirstOrDefault().InnerText.Trim();
                }
                else if (result.SelectNodes(".//span//strong//a") != null && !string.IsNullOrEmpty(result.SelectNodes(".//span//strong//a").FirstOrDefault().InnerText))
                {
                    ContentTitle = result.SelectNodes(".//span//strong//a").FirstOrDefault().InnerText.Trim();
                }
                else if (result.SelectNodes(".//p//a//strong") != null && !string.IsNullOrEmpty(result.SelectNodes(".//p//a//strong").FirstOrDefault().InnerText.Trim()))
                {
                    ContentTitle = result.SelectNodes(".//p//a//strong").FirstOrDefault().InnerText.Trim();
                }
                else if (result.SelectNodes(".//p//strong//a") != null && !string.IsNullOrEmpty(result.SelectNodes(".//p//strong//a").FirstOrDefault().InnerText.Trim()))
                {
                    ContentTitle = result.SelectNodes(".//p//strong//a").FirstOrDefault().InnerText.Trim();
                }
                else if (result.SelectNodes(".//p//a") != null && !string.IsNullOrEmpty(result.SelectNodes(".//p//a").FirstOrDefault().InnerText))
                {
                    ContentTitle = result.SelectNodes(".//p//a").FirstOrDefault().InnerText.Trim();
                }
                else if (result.SelectNodes(".//p//strong") != null && !string.IsNullOrEmpty(result.SelectNodes(".//p//strong").FirstOrDefault().InnerText))
                {
                    ContentTitle = result.SelectNodes(".//p//strong").FirstOrDefault().InnerText.Trim();
                }

                // get summary of news
                if (result.SelectNodes(".//td[@width='71%']//tr/td") != null)
                {
                    ContentSummary = result.SelectNodes(".//td[@width='71%']//tr/td").ElementAt(1).InnerText.Trim().ToString();
                }
                // get date created of news
                if (result.SelectSingleNode(".//span[@class='adaptation_italic']") != null)
                {
                    ContentDate = result.SelectSingleNode(".//span[@class='adaptation_italic']").InnerText.Replace("(", "").Replace(")", "").Trim();
                }
                // get Thumbnail url of news
                if (result.SelectNodes(".//img[@class='reflect ropacity30']") != null)
                {
                    ImgUrl = result.SelectNodes(".//img[@class='reflect ropacity30']").FirstOrDefault().GetAttributeValue("src", "").ToString();
                }
                else if (result.SelectNodes(".//td//a//img[@width='140']") != null)
                {
                    ImgUrl = result.SelectNodes(".//td//a//img[@width='140']").FirstOrDefault().GetAttributeValue("src", "").ToString();
                }
                else if (result.SelectNodes(".//img[@class='reflect ropacity30 ']") != null)
                {
                    ImgUrl = result.SelectNodes(".//img[@class='reflect ropacity30 ']").FirstOrDefault().GetAttributeValue("src", "").ToString();
                }
                if (ImgUrl.Contains("diorg/"))
                {
                    ImgUrl = ImgUrl.Replace("diorg/", "../libraries/aspx/diorg/");
                }
                // get content page of url
                if (result.SelectNodes(".//td[@width='29%']//a") != null)
                {
                    ContentPageUrl = result.SelectNodes(".//td[@width='29%']//a").FirstOrDefault().GetAttributeValue("href", "").ToString();
                }
                else if (result.SelectNodes(".//td[@width='29%']//tr/td//a") != null)
                {
                    ContentPageUrl = result.SelectNodes(".//td[@width='29%']//tr/td//a").FirstOrDefault().GetAttributeValue("href", "").ToString();
                }
                else if (result.SelectNodes(".//td[@width='28%']//a") != null)
                {
                    ContentPageUrl = result.SelectNodes(".//td[@width='28%']//a").FirstOrDefault().GetAttributeValue("href", "").ToString();
                }
                //get content of inner html page
                if (ContentPageUrl.Contains("diorg/"))
                {
                    // create full path of inner html page
                    ContentFileFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.DiorgContent,
                        Global.SplitString(ContentPageUrl, "diorg/")[1].ToString().Replace(@"/", "\\"));
                    // check if file exist
                    if (File.Exists(ContentFileFullPath))
                    {
                        // call method to get content of html page
                        //get pdfurl as out parameter
                        RetPageContent = GetInnerContentNPdfUrl(ContentFileFullPath, out PdfFileUrl);
                    }
                }
                // Initlize object of class cmshelper 
                ObjCMSHelper = new CMSHelper();
                // remove space and ;nbsp from date
                ContentDate = ObjCMSHelper.RemoveExtraCharsFromDate(ContentDate);
                // get created date of news
                SqlDtTime = new SqlDateTime(ObjCMSHelper.GetSqlDataTimeFromInptDate(ContentDate));
                // get user email id from session
                if (!string.IsNullOrEmpty(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUserNId].ToString()))
                {
                    UserEmailID = Global.Get_User_EmailId_ByAdaptationURL(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUserNId].ToString());
                }
                // get user name from session
                if (!string.IsNullOrEmpty(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser].ToString()))
                {
                    UserName = HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser].ToString();
                }

                // Create url from title
                CountentUrl = ObjCMSHelper.CreateUrlFromInputString(ContentTitle);

                //Get language of current database 
                CurrentDbLangCode = Global.GetDefaultLanguageCode();

                // innitlize of members of class Content
                Content.MenuCategory = "Action";
                Content.Date = SqlDtTime;
                Content.DateAdded = new SqlDateTime(DateTime.Now);
                Content.DateModified = new SqlDateTime(DateTime.Now);
                Content.Description = RetPageContent;
                Content.Title = ContentTitle;
                Content.PDFUpload = PdfFileUrl;
                Content.Summary = ContentSummary;
                Content.Thumbnail = ImgUrl;
                Content.URL = CountentUrl;
                Content.Archived = false;
                Content.UserNameEmail = UserName + " " + UserEmailID;
                Content.LngCode = CurrentDbLangCode;
                Content.ArticleTagID = -1;
                Content.IsDeleted = false;
                Content.IsDeleted = false;
                // all class Content to list
                ListDC.Add(Content);
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
        // return list containing all html pages
        return ListDC;
    }


    // Read facts page html content
    public List<DataContent> ReadFactsHtmlContent()
    {
        List<DataContent> ListDC = new List<DataContent>();
        string PdfFileUrl = string.Empty;
        string RetPageContent = string.Empty;
        string NewsContentFullPath = string.Empty;
        string HtmlPageFullPath = string.Empty;
        string HtmlPageName = string.Empty;
        HtmlDocument document = null;
        string NewsSummary = string.Empty;
        string NewsTitle = string.Empty;
        string NewsContentDate = string.Empty;
        string ImgUrl = string.Empty;
        string ContentPageUrl = string.Empty;
        string UserEmailID = string.Empty;
        string DatabaseLanguage = string.Empty;
        string UserName = string.Empty;
        string CurrentDbLangCode = string.Empty;
        string NewsUrl = string.Empty;
        string ArticleTags = string.Empty;
        SqlDateTime SqlDtTime;
        // Initlize object of class cmshelper 
        CMSHelper ObjCMSHelper = null;
        try
        {
            //get name of news html page
            HtmlPageName = this.FactsHtmlFilenamePrefix + ".html";
            document = new HtmlDocument();
            // create html page full path 
            HtmlPageFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.DiorgContent, HtmlPageName);
            if (!File.Exists(HtmlPageFullPath))
            {
                return ListDC;
            }
            // load html page
            document.Load(HtmlPageFullPath);
            // Targets a specific node(since all the page contains parent table tag with width 700)
            HtmlNode content_wrapper = document.DocumentNode.SelectNodes("//table[@width='700']//tr//td[@height='450']").FirstOrDefault();
            // get html news collection
            if (content_wrapper.SelectNodes("//table[@width='700']//tr//td") != null)
            {
                HtmlNodeCollection SplitString = content_wrapper.SelectNodes("//div[@class='goal']");
                foreach (HtmlNode HtmlNodResult in SplitString)
                {
                    //HtmlNodResult.se
                    DataContent Content = new DataContent();

                    // Initlize object of class cmshelper 
                    ObjCMSHelper = new CMSHelper();
                    // remove space and ;nbsp from date
                    NewsContentDate = ObjCMSHelper.RemoveExtraCharsFromDate(NewsContentDate);
                    // get created date of news
                    SqlDtTime = new SqlDateTime(ObjCMSHelper.GetSqlDataTimeFromInptDate(NewsContentDate));

                    // get user email id from session
                    if (!string.IsNullOrEmpty(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUserNId].ToString()))
                    {
                        UserEmailID = Global.Get_User_EmailId_ByAdaptationURL(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUserNId].ToString());
                    }
                    // get user name from session
                    if (!string.IsNullOrEmpty(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser].ToString()))
                    {
                        UserName = HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser].ToString();
                    }

                    // Create url from title
                    NewsUrl = ObjCMSHelper.CreateUrlFromInputString(NewsTitle);

                    //Get language of current database 
                    CurrentDbLangCode = Global.GetDefaultLanguageCode();

                    // innitlize of members of class Content
                    Content.Date = SqlDtTime;
                    Content.DateAdded = new SqlDateTime(DateTime.Now);
                    Content.DateModified = new SqlDateTime(DateTime.Now);
                    Content.Description = RetPageContent;
                    Content.Title = NewsTitle;
                    Content.PDFUpload = PdfFileUrl;
                    Content.Summary = NewsSummary;
                    Content.Thumbnail = ImgUrl;
                    Content.URL = NewsUrl;
                    Content.Archived = false;
                    Content.UserNameEmail = UserName + " " + UserEmailID;
                    Content.LngCode = CurrentDbLangCode;
                    Content.ArticleTagID = -1;
                    // all class Content to list
                    ListDC.Add(Content);
                }

            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
        // return list containing all html pages
        return ListDC;
    }

    // Get News Content page html
    private string GetInnerContentNPdfUrl(string ContentPageUrl, out string PdfFileUrl)
    {
        string RetVal = string.Empty;
        PdfFileUrl = string.Empty;
        HtmlDocument HtmlDoc = new HtmlDocument();
        HtmlDoc.Load(ContentPageUrl);
        // Targets a specific node(since all the page contains parent table tag with width 700)
        HtmlNode content_wrapper = HtmlDoc.DocumentNode.SelectNodes("//table[@width='700']").FirstOrDefault();
        //  HtmlNode BodyContent = HtmlDoc.DocumentNode.SelectNodes("//div[@id='divPrintReadyArea']").FirstOrDefault();
        if (content_wrapper.SelectNodes(".//a[@target='_blank']") != null)
        {
            PdfFileUrl = content_wrapper.SelectNodes(".//a[@target='_blank']").FirstOrDefault().GetAttributeValue("href", "").ToString();
            if (PdfFileUrl.Contains("diorg/"))
            {
                PdfFileUrl = PdfFileUrl.Replace("diorg/", "../libraries/aspx/diorg/");
            }
        }
        if (content_wrapper.SelectNodes(".//tr//td[@background='diorg/images/middle_bgcenter.gif']") != null && !string.IsNullOrEmpty(content_wrapper.SelectNodes(".//tr//td[@background='diorg/images/middle_bgcenter.gif']").FirstOrDefault().InnerHtml.ToString()))
        {
            RetVal = content_wrapper.SelectNodes(".//tr//td[@background='diorg/images/middle_bgcenter.gif']").FirstOrDefault().InnerHtml.ToString().Trim();
        }
        else if (content_wrapper.SelectNodes(".//tr//td[@background='diorg/images/middle_bgcenter.gif']//table") != null && !string.IsNullOrEmpty(content_wrapper.SelectNodes(".//tr//td[@background='diorg/images/middle_bgcenter.gif']//table").FirstOrDefault().InnerHtml.ToString()))
        {
            RetVal = content_wrapper.SelectNodes(".//tr//td[@background='diorg/images/middle_bgcenter.gif']//table").FirstOrDefault().InnerHtml.ToString().Trim();
        }
        else if (content_wrapper.SelectNodes(".//tr//td[@background='../images/middle_bgcenter.gif']//table") != null)
        {
            RetVal = content_wrapper.SelectNodes(".//tr//td[@background='../images/middle_bgcenter.gif']//table").FirstOrDefault().InnerHtml.ToString().Trim();
        }
        if (RetVal.Contains("diorg/"))
        {
            RetVal = RetVal.Replace("diorg/", "../libraries/aspx/diorg/");
        }
        // replace ' with '' 
        RetVal = RetVal.Replace("'", "").Replace(@"\", "\\");
        return RetVal;
    }

    // Get facts Content html
    private string GetInnerContentForFacts(string ContentPageUrl)
    {
        string RetVal = string.Empty;
        HtmlDocument HtmlDoc = new HtmlDocument();
        HtmlDoc.Load(ContentPageUrl);
        HtmlNode BodyContentForPdf = HtmlDoc.DocumentNode.SelectNodes("//tr[@class='bodytext']").FirstOrDefault();

        if (HtmlDoc.DocumentNode.SelectNodes("//div[@id='divPrintReadyArea']//tr//td") != null)
        {
            RetVal = HtmlDoc.DocumentNode.SelectNodes("//div[@id='divPrintReadyArea']//tr//td").FirstOrDefault().InnerText.ToString().Trim();
        }
        return HtmlDoc.DocumentNode.OuterHtml.ToString();
    }




    /// <summary>
    /// This methods check in database if tag exist, then returns tag Nid
    /// else add tag to database and return Nid of Newly created tag 
    /// </summary>
    /// <param name="Tag">Input Tag name</param>
    /// <returns>Tag NId</returns>
    private int CreateAndGetTagNid(string Tag)
    {
        int RetVal = 0;
        string DBConnectionStatusMessage = string.Empty;
        List<System.Data.Common.DbParameter> DbParams = null;
        DIConnection ObjDIConnection = null;
        DataTable DtTagNid = null;
        CMSHelper ObjCMSHelper = null;
        try
        {

            // Call metho0d to get connection object
            ObjDIConnection = ObjCMSHelper.GetConnectionObject(out DBConnectionStatusMessage);
            // Check if connection object is not null
            if (ObjDIConnection == null)
            {
                RetVal = 0;
                return RetVal;
            }
            // Innitilze DbParams object
            DbParams = new List<System.Data.Common.DbParameter>();
            // create tag parameter
            System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
            Param1.ParameterName = "@Tag";
            Param1.DbType = DbType.String;
            Param1.Value = Tag;
            DbParams.Add(Param1);

            // Execute stored procedure to get tag Nid
            DtTagNid = ObjDIConnection.ExecuteDataTable("sp_CreateAndGetTagNid", CommandType.StoredProcedure, DbParams);
            // check if value is not existing in data table, then return 0
            if (DtTagNid.Rows.Count > 0)
            {
                RetVal = 0;
                return RetVal;
            }
            // Get Nid 
            else
            {
                RetVal = Convert.ToInt32(DtTagNid.Rows[0][0].ToString());
            }

        }
        catch (Exception Ex)
        {
            RetVal = 0;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

}