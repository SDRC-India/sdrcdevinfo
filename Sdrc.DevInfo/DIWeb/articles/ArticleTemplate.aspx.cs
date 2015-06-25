using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Text;
using System.Web.UI.HtmlControls;

public partial class articles_ArticleTemplate : System.Web.UI.Page
{
    protected string SelectedPageName = string.Empty;
    public string ArticleTags = string.Empty;
    public bool IsHiddenArticlesVisible = false;
    public string MenuCategory = string.Empty;
    protected string hdsby = string.Empty;
    protected string hdbnid = string.Empty;
    protected string hselarea = string.Empty;
    protected string hselind = string.Empty;
    protected string hlngcode = string.Empty;
    protected string hdvnids = string.Empty;
    protected string hlngcodedb = string.Empty;
    protected string hselindo = string.Empty;
    protected string hselareao = string.Empty;
    protected string hLoggedInUserNId = string.Empty;
    protected string hLoggedInUserName = string.Empty;
    protected string dbname = string.Empty;
    protected string dbsummary = string.Empty;
    protected string dbdesc = string.Empty;
    protected int DefAreaCount = 0;
    protected string ShowSliders = string.Empty;
    protected string isGalleryEnabled = "false";
    protected string isQdsToBeRendered = "false";
    protected string DbAdmName = string.Empty;
    protected string DbAdmInstitution = string.Empty;
    protected string DbAdmEmail = string.Empty;
    protected string hShowCloud = "false";
    protected string gaUniqueID = string.Empty;
    protected string ATitle = string.Empty;
    protected string SharingImageUrl = string.Empty;
    protected string SharingTitle = string.Empty;
    protected string SharingSummary = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        string CallingPageUrl = string.Empty;

        string UrlString = string.Empty;
        //  string UrlNumericSubstring = string.Empty;
        //  bool IsSubStrNumeric = false;
        // int NumericVal = 0;
        ArticlesGenerator GenerateArticle = null;
        if (!Page.IsPostBack)
        {

            // check if admin is loggedin then show hidden articles
            if (Session[Constants.SessionNames.LoggedAdminUser] != null)
            {
                IsHiddenArticlesVisible = true;
            }
            UrlString = Request.Url.OriginalString.ToString().ToLower();
            if (UrlString.Contains("articles/"))
            {
                if (Session[Constants.SessionNames.CurrentArticleMenuCategory] != null && Session[Constants.SessionNames.CurrentArticlePageNo] != null || Session[Constants.SessionNames.CurrentArticleTagIds] != null)
                {
                    string ArticleTagNids = string.Empty;
                    string ArticleCurrentPageNo = string.Empty;
                    string ArticleMenucategory = string.Empty;
                    if (Session[Constants.SessionNames.CurrentArticleMenuCategory] != null)
                    {
                        ArticleMenucategory = Session[Constants.SessionNames.CurrentArticleMenuCategory].ToString();
                    }
                    if (Session[Constants.SessionNames.CurrentArticlePageNo] != null)
                    {
                        ArticleCurrentPageNo = Session[Constants.SessionNames.CurrentArticlePageNo].ToString();
                    }
                    if (Session[Constants.SessionNames.CurrentArticleTagIds] != null)
                    {
                        ArticleTagNids = Session[Constants.SessionNames.CurrentArticleTagIds].ToString();
                        if (ArticleTagNids.Length > 2)
                        {
                            ArticleTagNids = ArticleTagNids.Replace('~', ',');
                        }
                    }
                    if (Session[Constants.SessionNames.CurrentArticleTagIds] != null)
                    {
                        ArticleTagNids = Session[Constants.SessionNames.CurrentArticleTagIds].ToString();
                        if (ArticleTagNids.Length > 2)
                        {
                            ArticleTagNids = ArticleTagNids.Replace('~', ',');
                        }
                    }
                    GetArticlesByMenuCategoryAndTags(ArticleMenucategory, ArticleCurrentPageNo, ArticleTagNids, IsHiddenArticlesVisible);
                    MenuCategory = ArticleMenucategory;
                    GenerateArticle = new ArticlesGenerator();
                    SelectedPageName = GenerateArticle.GetPageNameFromDbByMenuCategory(MenuCategory);
                    Session[Constants.SessionNames.CurrentArticlePageNo] = null;
                    Session[Constants.SessionNames.CurrentArticleTagIds] = null;
                    Session[Constants.SessionNames.CurrentArticleMenuCategory] = null;
                }
                else
                {
                    CallingPageUrl = Request.QueryString["category"].ToString().ToLower();
                    this.ShowArticles(CallingPageUrl, IsHiddenArticlesVisible);
                }
                ShowHideAddButton();
                // call method to Get posted data
                GetPostedData();
            }
            SetHideAndEditButtonVisiblity(false);
        }
    }

    private void GetPostedData()
    {
        try
        {
            string LanguageCode = string.Empty;
            DataTable DTLanguage = null;
            // Get Posted Data - will be passed to the Javascript

            // Set Data Search by
            if (!string.IsNullOrEmpty(Request["hdsby"]))
            {
                hdsby = Request["hdsby"];
                Session["hdsby"] = Request["hdsby"];
            }
            else
                hdsby = Session["hdsby"] != null ? Session["hdsby"].ToString() : null;
            //Set database NId - check in the posetd data
            if (!string.IsNullOrEmpty(Request["hdbnid"]))
            {
                // -- check in the posetd data first
                hdbnid = Global.GetDefaultDbNId(); //Request["hdbnid"];
            }
            else if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)].Value)))
            {
                // then check in the cookie               
                hdbnid = Global.GetDefaultDbNId();
            }
            else
            {
                // get default db nid ;
                hdbnid = Global.GetDefaultDbNId();
            }

            if (!Global.IsDbIdExists(hdbnid))
            {
                // get default db nid ;
                hdbnid = Global.GetDefaultDbNId();
            }

            Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId), hdbnid, Page);

            // Set Selected Areas - check in the posetd data
            if (!string.IsNullOrEmpty(Request["hselarea"]))
            {
                hselarea = Request["hselarea"];
                Session["hselarea"] = Request["hselarea"];
            }
            else
            {
                hselarea = Session["hselarea"] != null ? Session["hselarea"].ToString() : null;
            }

            // Set Selected Indicators - check in the posetd data
            if (!string.IsNullOrEmpty(Request["hselind"]))
            {
                hselind = Request["hselind"];
                Session["hselind"] = Request["hselind"];
            }
            else
                hselind = Session["hselind"] != null ? Session["hselind"].ToString() : null;

            // Set selected Indicator JSON object
            if (!string.IsNullOrEmpty(Request["hselindo"]))
            {
                hselindo = formatQuoteString(Request["hselindo"]);
                Session["hselindo"] = formatQuoteString(Request["hselindo"]);
            }
            else
                hselindo = Session["hselindo"] != null ? Session["hselindo"].ToString() : null;

            // Set selected area JSON object
            if (!string.IsNullOrEmpty(Request["hselareao"]))
            {
                hselareao = formatQuoteString(Request["hselareao"]);
                Session["hselareao"] = formatQuoteString(Request["hselareao"]);
            }
            else
                hselareao = Session["hselareao"] != null ? Session["hselareao"].ToString() : null;

            // Set selected Cache NIds object
            if (!string.IsNullOrEmpty(Request["hdvnids"]))
            {
                hdvnids = formatQuoteString(Request["hdvnids"]);
                Session["hdvnids"] = formatQuoteString(Request["hdvnids"]);
            }
            else
                hdvnids = Session["hdvnids"] != null ? Session["hdvnids"].ToString() : null;
            //
            // Set language code  
            if (Request.QueryString["lngCode"] != null)
            {
                DTLanguage = Global.GetAllDBLangaugeCodes();
                if (DTLanguage != null)
                {
                    foreach (DataRow dr in DTLanguage.Rows)
                    {
                        if (dr["Language_Code"].ToString() == Request.QueryString["lngCode"].ToString())
                        {
                            LanguageCode = Request.QueryString["lngCode"].ToString();
                            break;
                        }
                    }
                }
            }
            else if (Request.QueryString["jsonAreasTopics"] != null)
            {
                try
                {
                    string[] QSArray = Request.QueryString["jsonAreasTopics"].Split(new string[] { "," }, StringSplitOptions.None);
                    if (QSArray.Length >= 6)
                    {
                        string QALngCode = QSArray[5].Split(new string[] { ":" }, StringSplitOptions.None)[1].Replace("\"", "").Replace("}", "");
                        DTLanguage = Global.GetAllDBLangaugeCodes();
                        if (DTLanguage != null)
                        {
                            foreach (DataRow dr in DTLanguage.Rows)
                            {
                                if (dr["Language_Code"].ToString() == QALngCode)
                                {
                                    LanguageCode = QALngCode;
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }
            if (!string.IsNullOrEmpty(LanguageCode))
            {
                hlngcode = LanguageCode;
            }
            else if (!string.IsNullOrEmpty(Request["hlngcode"]))
            {
                // -- check in the posetd data first
                hlngcode = Request["hlngcode"];
            }
            else if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value)))
            {
                // then check in the cookie
                hlngcode = Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value;
            }
            else
            {
                // get default lng code
                hlngcode = Global.GetDefaultLanguageCode();
            }
            Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode), hlngcode, Page);
            hlngcodedb = Global.GetDefaultLanguageCodeDB(hdbnid.ToString(), hlngcode);

            Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.DBLanguageCode), hlngcodedb, Page);
            if (!string.IsNullOrEmpty(Request["hLoggedInUserNId"]))
            {
                hLoggedInUserNId = Request["hLoggedInUserNId"];
                Session["hLoggedInUserNId"] = Request["hLoggedInUserNId"];
            }
            else if (Session[Constants.SessionNames.LoggedAdminUserNId] != null && Session[Constants.SessionNames.LoggedAdminUserNId].ToString() != "")
            {
                hLoggedInUserNId = Session[Constants.SessionNames.LoggedAdminUserNId].ToString();
            }
            else
                hLoggedInUserNId = Session["hLoggedInUserNId"] != null ? Session["hLoggedInUserNId"].ToString() : null;

            if (!string.IsNullOrEmpty(Request["hLoggedInUserName"]))
            {
                hLoggedInUserName = Request["hLoggedInUserName"];
                Session["hLoggedInUserName"] = Request["hLoggedInUserName"];
            }
            else if (Session[Constants.SessionNames.LoggedAdminUser] != null && Session[Constants.SessionNames.LoggedAdminUser].ToString() != "")
            {
                hLoggedInUserName = Session[Constants.SessionNames.LoggedAdminUser].ToString();
            }
            else
                hLoggedInUserName = Session["hLoggedInUserName"] != null ? Session["hLoggedInUserName"].ToString() : null;


            // Get show sliders status in variable
            if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.ShowSliders)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.ShowSliders)].Value)))
            {
                // then check in the cookie
                ShowSliders = Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.ShowSliders)].Value;
            }
            else
            {
                // get show sliders value from appsettings.xml
                ShowSliders = Global.GetShowSlidersValue();
                Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.ShowSliders), ShowSliders, Page);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private string formatQuoteString(string SingleQuotedString)
    {
        string result = string.Empty;

        result = SingleQuotedString.Replace(@"'", @"\'");

        while (result.IndexOf(@"\\'") != -1)
        {
            result = result.Replace(@"\\'", @"\'");
        }

        return result;
    }

    // show button for adding article, if admin is logged in, else hide it
    private void ShowHideAddButton()
    {
        if (Session[Constants.SessionNames.LoggedAdminUser] != null)
        {
            if (!string.IsNullOrEmpty(Session[Constants.SessionNames.LoggedAdminUser].ToString()))
            {
                AddNewNews.Style.Add("display", "block");
                DIv_Adm_Options.Style.Add("display", "block");
            }
            else
            {
                DIv_Adm_Options.Style.Add("display", "none");
            }

        }
    }

    // show button for editing article, if admin is logged in, else hide it
    private void SetHideAndEditButtonVisiblity(bool Status)
    {
        if (Status)
        {
            if (Session[Constants.SessionNames.LoggedAdminUser] != null)
            {
                DIv_Adm_Options.Style.Add("display", "block");
                EditPage.Style.Add("display", "block");
                //  PipeDeleteNews.Style.Add("display", "block");
                //  PipeAddNews.Style.Add("display", "block");
                DeleteNews.Style.Add("display", "block");
            }
            else
            {
                DIv_Adm_Options.Style.Add("display", "none");
            }
        }
    }

    /// <summary>
    /// get articles from database,
    /// bind it to html
    /// </summary>
    /// <param name="CallingPageUrl">paramater for maintaining article type</param>
    private void ShowArticles(string CallingPageUrl, bool IsHiddenArticlesVisible)
    {
        ArticlesGenerator GenerateArticle = new ArticlesGenerator();
        List<MenuCategories> ListMenuCat = null;
        MenuCategories ObjMenuCat = null;
        string RetContentHtml = string.Empty;
        int MaxArticleCount = 0;
        int CurrentPageNo = 1;
        string RetMenuCategory = string.Empty;
        bool IsGetArticleByUrl = false;

        //   string AdaptaionPagingName = Constants.CMS.AdaptaionPagingName.ToLower();
        int ArticleStartPosition = 0;
        try
        {
            ListMenuCat = new List<MenuCategories>();
            ObjMenuCat = new MenuCategories();
            ListMenuCat = ObjMenuCat.GetMenuCategoriesList(true);
            foreach (MenuCategories Category in ListMenuCat)
            {
                if (CallingPageUrl.ToLower() == Category.MenuCategory.ToLower())
                {
                    IsGetArticleByUrl = true;
                    MenuCategory = Category.MenuCategory.ToLower();
                    if (!string.IsNullOrEmpty(Global.MaxArticlesCount))
                    {
                        MaxArticleCount = Convert.ToInt32(Global.MaxArticlesCount);
                    }

                    ArticleStartPosition = (CurrentPageNo - 1) * MaxArticleCount + 1;

                    // Get Html For News 
                    RetContentHtml = GenerateArticle.GetArticleByMenuCategory(ArticleStartPosition, MaxArticleCount, MenuCategory, CurrentPageNo, IsHiddenArticlesVisible);
                    break;
                }
            }
            if (!IsGetArticleByUrl)
            {
                this.ShowArticleByUrl(CallingPageUrl, true);
            }
            // Check if Return Html is not empty
            else if (!string.IsNullOrEmpty(RetContentHtml))
            {
                //set Html to content page
                divContent.InnerHtml = RetContentHtml;
            }
            SelectedPageName = GenerateArticle.GetPageNameFromDbByMenuCategory(MenuCategory);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }


    private void ShowArticleByUrl(string CallingPageUrl, bool IsHiddenArticlesVisible)
    {
        try
        {
            ArticlesGenerator GenerateArticle = new ArticlesGenerator();
            string RetContentHtml = string.Empty;
            
            DataContent ObjDataContent = new DataContent();

            // Call method to get data content based on url
            ObjDataContent = GenerateArticle.GetDataContentFromDatabaseByUrl(CallingPageUrl, IsHiddenArticlesVisible);

            if (ObjDataContent != null)
            {
                RetContentHtml = GenerateArticle.GetHtmlByMenuCategory(ObjDataContent);
                // Call method to get Html of inner content page 
                try
                {
                    // set meta tag values
                    if (!string.IsNullOrEmpty(ObjDataContent.Summary))
                    {
                        Page.MetaDescription = ObjDataContent.Summary;  //Summary
                    }
                    if (!string.IsNullOrEmpty(ObjDataContent.Title))
                    {
                        Page.MetaDescription = CreateKyWordsFromTitle(ObjDataContent.Title);
                        ATitle = ObjDataContent.Title;
                    }
                    SpanMenuCategoryHeading.InnerText = ObjDataContent.Title;
                    SpanMenuCategoryHeading.Style.Add("Font-Size", "18px");
                    SpanHeaderDescription.InnerHtml = ObjDataContent.Date.ToString();
                    SpanHeaderDescription.Style.Add("Font-Style", "italic");

                    if (!string.IsNullOrEmpty(ObjDataContent.PDFUpload))
                    {
                        ArticlePdf.HRef = ObjDataContent.PDFUpload;
                    }
                    else
                    {
                        ArticlePdf.HRef = "javascript:void(0);";
                    }
                    // Set selected page name
                    SelectedPageName = GenerateArticle.GetPageNameFromDbByMenuCategory(ObjDataContent.MenuCategory);
                    SharingTitle = ConvertToAscii(ObjDataContent.Title.Replace("'", "\\'").Replace(@"""", @"\"""));
                    SharingSummary = ConvertToAscii(ObjDataContent.Summary.Replace("'", "\\'").Replace(@"""", @"\"""));
                    SharingImageUrl = ObjDataContent.Thumbnail;

                }
                catch (Exception Ex)
                {
                    Global.CreateExceptionString(Ex, null);
                }


                MenuCategory = ObjDataContent.MenuCategory.ToLower();
            }

            // Check if Return Html is not empty
            if (!string.IsNullOrEmpty(RetContentHtml))
            {
                //set Html to content page
                divContent.InnerHtml = RetContentHtml;
                SetDeleteLinkUrl(ObjDataContent.ContentId);
                SetHideAndEditButtonVisiblity(true);
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
    }

    private void AddMetaPropForFaceBook(string ThumbNailURL)
    {
        // Set meta tag title
        HtmlMeta TagTitle = new HtmlMeta();
        TagTitle.Attributes.Add("property", "og:title");
        TagTitle.Content = "Dev Info"; // don't HtmlEncode() string. HtmlMeta already escapes characters.
        Page.Header.Controls.Add(TagTitle);

        // Set meta tag image URL
        HtmlMeta TagImg = new HtmlMeta();
        TagImg.Attributes.Add("property", "og:image");
        string ImgBrowserUrl = Global.SplitString(HttpContext.Current.Request.Url.AbsoluteUri, "/articles/")[0] + ThumbNailURL.Replace("../", "/");
        TagImg.Content = ImgBrowserUrl; // don't HtmlEncode() string. HtmlMeta already escapes characters.
        Page.Header.Controls.Add(TagImg);

        // Set meta tag Description
        HtmlMeta TagDesc = new HtmlMeta();
        TagDesc.Attributes.Add("property", "og:description");
        TagDesc.Content = "Powered By DevInfo"; // don't HtmlEncode() string. HtmlMeta already escapes characters.
        Page.Header.Controls.Add(TagDesc);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ArticleTitle"></param>
    /// <returns></returns>
    private string CreateKyWordsFromTitle(string ArticleTitle)
    {
        ArticleTitle = ArticleTitle.ToLower().Replace("&quot;", " ").Replace(" the ", " ").Replace(" to ", " ").Replace(" a ", " ").Replace(" an ", " ").Replace(" is", " ").Replace(" with ", " ")
         .Replace(" was ", " ").Replace(" were ", " ").Replace(" will ", " ").Replace(" shall ", " ").Replace(" at ", " ").Replace(" it ", " ").Replace(" on ", " ").Replace(" for ", " ");
        string Keyword = string.Empty;
        String[] ArrTitle = ArticleTitle.Split(' ');
        for (int Icount = 0; Icount < ArrTitle.Length; Icount++)
        {
            string Item = ArrTitle[Icount].ToLower();
            if (!string.IsNullOrEmpty(Item))
            {
                Keyword = Keyword + Item + ",";
            }
        }
        Keyword = Keyword.Remove(Keyword.Length - 1);
        return Keyword;
    }

    private void SetDeleteLinkUrl(int ContentId)
    {

        DeleteNews.Attributes.Add("href", "javascript:DeleteArticlebyContentId('" + ContentId.ToString().ToLower() + "','" + 1 + "');");
    }

    public string GetArticlesByMenuCategoryAndTags(string ArticleMenuCategory, string CurrentPageNo, string TagNids, bool IsHiddenArticlesVisible)
    {
        string RetContentHtml = string.Empty;
        ArticlesGenerator GenerateArticle = new ArticlesGenerator();
        int MaxArticleCount = 0;
        int RecordStartPasition = 1;
        int currentPageNumber = 1;
        try
        {
            currentPageNumber = Convert.ToInt32(CurrentPageNo);
            if (!string.IsNullOrEmpty(Global.MaxArticlesCount))
            {
                MaxArticleCount = Convert.ToInt32(Global.MaxArticlesCount);
            }
            RecordStartPasition = (currentPageNumber - 1) * MaxArticleCount + 1;

            if (!string.IsNullOrEmpty(TagNids))// if tag is not null or empty get records by tag
            {
                ArticleTags = TagNids;
                RetContentHtml = GenerateArticle.GetArticleByMenuCategoryNTag(RecordStartPasition, MaxArticleCount, ArticleMenuCategory, currentPageNumber, TagNids, IsHiddenArticlesVisible);
            }
            else
            {
                RetContentHtml = GenerateArticle.GetArticleByMenuCategory(RecordStartPasition, MaxArticleCount, ArticleMenuCategory, currentPageNumber, IsHiddenArticlesVisible);
            }
            // Check if Return Html is not empty
            if (!string.IsNullOrEmpty(RetContentHtml))
            {
                //set Html to content page
                divContent.InnerHtml = RetContentHtml;
            }

            MenuCategory = ArticleMenuCategory;
            // SetHeaderText(ArticleMenuCategory);
        }
        catch (Exception Ex)
        {
            RetContentHtml = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetContentHtml;

    }

    private string ConvertToAscii(string InString)
    {
        StringBuilder newStringBuilder = new StringBuilder();
        newStringBuilder.Append(InString.Normalize(NormalizationForm.FormKD)
                                        .Where(x => x < 128)
                                        .ToArray());
        return newStringBuilder.ToString();
    }
}
