using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class articles_ArticleMaster : System.Web.UI.MasterPage
{

    #region -- Private --

    #region -- Methods --

    private string[] GetDisclaimerText()
    {
        string[] RetVal = null;
        string DisclaimerText = string.Empty;
        try
        {
            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            DisclaimerText = CatalogService.GetDisclaimerText();

            if (!string.IsNullOrEmpty(DisclaimerText))
            {
                RetVal = Global.SplitString(DisclaimerText, "|");
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    private void SetDisclaimerValues()
    {
        string[] DisclaimerTextArr = null;

        try
        {
            DisclaimerTextArr = this.GetDisclaimerText();

            if (DisclaimerTextArr.Length > 0)
            {
                this.DevInfoCopyrt = DisclaimerTextArr[0];
                this.Disclaimer = DisclaimerTextArr[1];
                this.applicationVersion = Global.GetLanguageKeyValue("Version") + Global.application_version;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void SetMasterSiteUrl()
    {
        string GlobalMasterWebUrl = string.Empty;

        try
        {
            DIWorldwide.Catalog catalog = new DIWorldwide.Catalog();
            catalog.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            GlobalMasterWebUrl = catalog.GetGlobalMasterWebUrl();
            mastersiteurl.HRef = GlobalMasterWebUrl;

            aGoto.HRef = GlobalMasterWebUrl;
            aGlobalNews.HRef = GlobalMasterWebUrl + "/articles/news";
            aGlobalInnovations.HRef = GlobalMasterWebUrl + "/articles/Innovations";//?T=N&PN=diorg/di_innovations.html";
            aGlobalCatalog.HRef = GlobalMasterWebUrl + "/libraries/aspx/Catalog.aspx";

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }
    #endregion

    #endregion

    #region -- Public/Protected --

    #region -- Variables --

    protected string DevInfoCopyrt = string.Empty;
    protected string Disclaimer = string.Empty;
    protected string IsGlobalAdaptation = string.Empty;
    protected string CatalogText = string.Empty;
    protected string isInnovationsMenuVisible = "true";
    protected string isNewsMenuVisible = "true";
    protected string isContactUsMenuVisible = "true";
    protected string isSupportMenuVisible = "true";
    protected string sliders_list = "";
    protected string applicationVersion = string.Empty;
    protected string isDownloadsLinkVisible = "true";
    protected string isTrainingLinkVisible = "true";
    protected string isMapLibraryLinkVisible = "true";
    protected string islangFAQVisible = "false";
    protected string isHowtoVideoVisible = "false";
    protected string isRSSFeedsLinkVisible = "true";
    protected string isDiWorldWideLinkVisible = "true";
    protected string gaUniqueID = string.Empty;
    protected string isShowMapServer = "false";
    protected string isSitemapLinkVisible = "false";
    public string mapServerPath = string.Empty;
    protected string islangKBVisible = "false";

    #endregion

    #region -- Methods/Events --

    protected void Page_Load(object sender, EventArgs e)
    {
        Adaptation_News.Value = Global.GetLanguageKeyValue("News");
        string DefMenuCategory = string.Empty;
        this.SetDisclaimerValues();
        SetMasterSiteUrl();
        aGlobalNews.InnerHtml = Global.GetLanguageKeyValue("News");
        aGlobalInnovations.InnerHtml = Global.GetLanguageKeyValue("Innovations");
        aGlobalCatalog.InnerHtml = Global.GetLanguageKeyValue("Catalog");

        aGotoDesc.InnerHtml = Global.GetLanguageKeyValue("aGotoDesc");
        aGlobalNewsDesc.InnerHtml = Global.GetLanguageKeyValue("aGlobalNewsDesc");
        aGlobalInnovationsDesc.InnerHtml = Global.GetLanguageKeyValue("aGlobalInnovationsDesc");
        aGlobalCatalogDesc.InnerHtml = Global.GetLanguageKeyValue("aGlobalCatalogDesc");
        Article_News.Value = Global.GetLanguageKeyValue("News");
        if (Global.CheckIsGlobalAdaptation())
        {
            IsGlobalAdaptation = "true";
            CatalogText = Global.GetLanguageKeyValue("Catalog");
            if (Global.CheckIsDI7ORGAdaptation())
            {
                divCountryMenu.Visible = false;
            }
            else
            {
                IsGlobalAdaptation = "false";
                CatalogText = Global.GetLanguageKeyValue("Catalog");
                if (Global.standalone_registry == "false")
                {
                    divCountryMenuMain.Visible = true;
                }
                else
                {
                    divCountryMenuMain.Visible = false;
                    headerDIVMenu.Visible = false;
                }
            }
        }
        else
        {
            IsGlobalAdaptation = "false";
            CatalogText = Global.GetLanguageKeyValue("Catalog");
            if (Global.standalone_registry == "false")
            {
                divCountryMenuMain.Visible = true;
            }
            else
            {
                divCountryMenuMain.Visible = false;
                headerDIVMenu.Visible = false;
            }
        }
        isInnovationsMenuVisible = Global.isInnovationsMenuVisible;
        isShowMapServer = Global.isShowMapServer;
        isNewsMenuVisible = Global.isNewsMenuVisible;
        isContactUsMenuVisible = Global.isContactUsMenuVisible;
        isSupportMenuVisible = Global.isSupportMenuVisible;
        sliders_list = Global.sliders_list;
        isDownloadsLinkVisible = Global.isDownloadsLinkVisible;
        isTrainingLinkVisible = Global.isTrainingLinkVisible;
        isMapLibraryLinkVisible = Global.isMapLibraryLinkVisible;
        islangFAQVisible = Global.islangFAQVisible;
        isHowtoVideoVisible = Global.isHowToVideoVisible;
        isRSSFeedsLinkVisible = Global.isRSSFeedsLinkVisible;
        isDiWorldWideLinkVisible = Global.isDiWorldWideLinkVisible;
        isSitemapLinkVisible = Global.isSitemapLinkVisible;
        gaUniqueID = Global.gaUniqueID;
        islangKBVisible = Global.islangKBVisible;
    }

    #endregion

    #endregion
}
