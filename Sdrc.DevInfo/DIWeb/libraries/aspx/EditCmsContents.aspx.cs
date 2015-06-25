using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Text;

public partial class libraries_aspx_EditCmsContents : System.Web.UI.Page
{
    // public string IsShowPdf = string.Empty;
    public string SelectedPageName = string.Empty;
    public string SelectedMenuCat = string.Empty;
    public int TagID = 0;
    public DataContent RetDataContent;
    public string DetaildContent = string.Empty;
    public string EditDatetime = string.Empty;
    public string ThumbNailImageSrc = string.Empty;
    public string PdfSrc = string.Empty;
    public string CMSContent = string.Empty;
    public string RequestingPagePath = string.Empty;
    protected string hdsby = string.Empty;
    protected string hdbnid = string.Empty;
    protected string hselarea = string.Empty;
    protected string hselind = string.Empty;
    protected string hlngcode = string.Empty;
    protected string hlngcodedb = string.Empty;
    protected string hselindo = string.Empty;
    protected string hselareao = string.Empty;
    protected string hLoggedInUserNId = string.Empty;
    protected string hLoggedInUserName = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        bool IsHiddenArticlesVisible = true;
        if (Session[Constants.SessionNames.LoggedAdminUser] == null)
        {
            IsHiddenArticlesVisible = false;
            Response.Redirect("Login.aspx");
        }
        else
        {
            if (Request.QueryString["PN"] != null)
            {

                // get url of requested page from query string
                string CallingPageUrl = Request.QueryString["PN"].ToString();
                // get html content from database bay url and, set datacontent to page for editing
                this.SetEditableHtmlContent(CallingPageUrl, IsHiddenArticlesVisible);
            }
            if (Request.QueryString["CP"] != null)
            {
                HdnCurrentArticlePageNo.Value = Request.QueryString["CP"].ToString(); ;
                //  Session[Constants.SessionNames.CurrentArticlePageNo] = Request.QueryString["CP"].ToString();
            }
            if (Request.QueryString["AT"] != null)
            {
                HdnCurrentArticleTagIds.Value = Request.QueryString["AT"].ToString();
                // Session[Constants.SessionNames.CurrentArticleTagIds] = Request.QueryString["AT"].ToString();
            }


            GetPostedData();
        }
    }

    private void GetPostedData()
    {
        try
        {

            if (!string.IsNullOrEmpty(Request["hdsby"])) { hdsby = Request["hdsby"]; }
            else
            {
                hdsby = "News";
            }

            //Set database NId - check in the posetd data
            if (!string.IsNullOrEmpty(Request["hdbnid"]))
            {
                // -- check in the posetd data first
                hdbnid = Request["hdbnid"];
            }
            else if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)].Value)))
            {
                // then check in the cookie
                hdbnid = Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId)].Value;
            }
            else
            {
                // get default db nid ;
                hdbnid = Global.GetDefaultDbNId();
            }
            Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.DBNId), hdbnid, Page);

            // Set Selected Areas - check in the posetd data
            if (!string.IsNullOrEmpty(Request["hselarea"]))
            {
                hselarea = Global.formatQuoteString(Request["hselarea"]);
                Session["hselarea"] = Request["hselarea"];
            }
            else
            {
                hselarea = Session["hselarea"] != null ? Session["hselarea"].ToString() : null;
            }

            // Set Selected Indicators - check in the posetd data
            if (!string.IsNullOrEmpty(Request["hselind"]))
            {
                hselind = Global.formatQuoteString(Request["hselind"]);
                Session["hselind"] = Request["hselind"];
            }
            else
                hselind = Session["hselind"] != null ? Session["hselind"].ToString() : null;

            // Set selected Indicator JSON object
            if (!string.IsNullOrEmpty(Request["hselindo"]))
            {
                hselindo = Global.formatQuoteString(Request["hselindo"]);
                Session["hselindo"] = Global.formatQuoteString(Request["hselindo"]);
            }
            else
                hselindo = Session["hselindo"] != null ? Session["hselindo"].ToString() : null;

            // Set selected area JSON object
            if (!string.IsNullOrEmpty(Request["hselareao"]))
            {
                hselareao = Global.formatQuoteString(Request["hselareao"]);
                Session["hselareao"] = Global.formatQuoteString(Request["hselareao"]);
            }
            else
                hselareao = Session["hselareao"] != null ? Session["hselareao"].ToString() : null;

            // Set language code   
            if (!string.IsNullOrEmpty(Request["hlngcode"]))
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


            //Set Database language code

            // get default database lng code
            hlngcodedb = Global.GetDefaultLanguageCodeDB(hdbnid.ToString(), hlngcode);
            Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.DBLanguageCode), hlngcodedb, Page);
            if (!string.IsNullOrEmpty(Request["hLoggedInUserNId"]))
            {
                hLoggedInUserNId = Request["hLoggedInUserNId"];
                Session["hLoggedInUserNId"] = Request["hLoggedInUserNId"];
            }
            else
                hLoggedInUserNId = Session["hLoggedInUserNId"] != null ? Session["hLoggedInUserNId"].ToString() : null;
            if (!string.IsNullOrEmpty(Request["hLoggedInUserName"]))
            {
                hLoggedInUserName = Request["hLoggedInUserName"];
                Session["hLoggedInUserName"] = Request["hLoggedInUserName"];
            }
            else
                hLoggedInUserName = Session["hLoggedInUserName"] != null ? Session["hLoggedInUserName"].ToString() : null;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }


    private void SetEditableHtmlContent(string CallingPageUrl, bool IsHiddenArticlesVisible)
    {
        ArticlesGenerator GenerateArticle = null;
        RetDataContent = new DataContent();
        List<string> RetTags = new List<string>();
        string Summary = string.Empty;
        try
        {
            GenerateArticle = new ArticlesGenerator();
            RetDataContent = GenerateArticle.GetDataContentFromDatabaseByUrl(CallingPageUrl, IsHiddenArticlesVisible);
            if (RetDataContent != null)
            {
                Summary = RetDataContent.Summary.Replace("<br/>", "\n");
                SelectedMenuCat = RetDataContent.MenuCategory;
                HdnContentId.Value = RetDataContent.ContentId.ToString();
                txtSummary.Value = Summary.ToString();
                txtTitle.Value = RetDataContent.Title.ToString();
                DetaildContent = RetDataContent.Description.ToString();
                DetaildContent = DetaildContent.Replace("{", "{{").Replace("}", "}}");
                DetaildContent = DetaildContent.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
                DetaildContent = DetaildContent.Replace("../libraries/aspx/diorg/", "diorg/");

                ////changes in script tag for ckeditor
                DetaildContent = DetaildContent.Replace("<script", "<scrpt");
                DetaildContent = DetaildContent.Replace("</script", "</scrpt");
                DetaildContent = DetaildContent.Replace(@"\", "\\");
                DetaildContent = ConvertToAscii(DetaildContent);
                // DetaildContent = Regex.Replace(DetaildContent, @"[^\u0000-\u007F]", string.Empty);
                if (RetDataContent.Date.Value.Year != 1900)
                {
                    EditDatetime = RetDataContent.Date.ToString();
                }

                ThumbNailImageSrc = RetDataContent.Thumbnail.ToString();
                ThumbNailImageSrc = ThumbNailImageSrc.Replace("../libraries/aspx/diorg/", "diorg/");

                PdfSrc = RetDataContent.PDFUpload.ToString();
                PdfSrc = PdfSrc.Replace("../libraries/aspx/diorg/", "diorg/");
                txtTags.Value = GenerateArticle.GetTagsFromDatabaseByTagId(Convert.ToInt32(RetDataContent.ArticleTagID.ToString()));
                TagID = RetDataContent.ArticleTagID;

                // set selected page name
                SelectedPageName = GenerateArticle.GetPageNameFromDbByMenuCategory(RetDataContent.MenuCategory);
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
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