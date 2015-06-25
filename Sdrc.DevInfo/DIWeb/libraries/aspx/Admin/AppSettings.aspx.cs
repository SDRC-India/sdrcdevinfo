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
using System.Xml;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;

public partial class libraries_aspx_Admin_AppSettings : System.Web.UI.Page
{
    protected string hlngcode = string.Empty;
    protected string hLoggedInUserNId = string.Empty;
    protected string VAribaleDeclaredInpage = string.Empty;
    #region "--Private--"

    #region "--Methods--"

    private void FillLanguageDDL()
    {
        string LngFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNodeList ObjXmlNodeList;
        int i = 1;

        try
        {
            LngFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.LanguageFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(LngFile);

            ObjXmlNodeList = XmlDoc.SelectNodes("/" + Constants.XmlFile.Language.Tags.Root + "/" + "child::node()");

            ddlLanguage.Items.Clear();
            ddlLanguage.Items.Insert(0, new ListItem("-- Select Language --", "-1"));

            foreach (XmlNode data in ObjXmlNodeList)
            {
                ddlLanguage.Items.Insert(i, new ListItem(data.Attributes[Constants.XmlFile.Language.Tags.LanguageAttributes.Name].Value, data.Attributes[Constants.XmlFile.Language.Tags.LanguageAttributes.Code].Value));
                i++;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void FillThemes()
    {
        string ThemeDirectoryPath = string.Empty;
        string[] ThemeDirectories;
        int i = 0;
        string LstItemText = string.Empty;
        string LstItemValue = string.Empty;

        try
        {
            ThemeDirectoryPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\themes");
            ThemeDirectories = Directory.GetDirectories(ThemeDirectoryPath);            
            
           
            
            ddlTheme.Items.Clear();
           // ddlTheme.Items.Insert(0, new ListItem("-- Select Theme --", "-1"));

            foreach (string ThemeName in ThemeDirectories)
            {
                LstItemText = Path.GetFileName(ThemeName);

                LstItemValue = HttpContext.Current.Request.Url.ToString().ToLower().Replace("libraries/aspx/admin/appsettings.aspx", "stock/themes/" + LstItemText + "/css/diuilibcommon.css");
                
                ddlTheme.Items.Insert(i, new ListItem(LstItemText, LstItemValue));
                i++;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void FillUnicefRegionDDL()
    {
        this.ddlUnicefRegion.Items.Clear();
        this.ddlUnicefRegion.Items.Insert(0, new ListItem("-- Select region --", "-1"));

        this.ddlUnicefRegion.Items.Insert(1, new ListItem("CEE/CIS", "CEE/CIS"));
        this.ddlUnicefRegion.Items.Insert(2, new ListItem("East Asia and Pacific", "East Asia and Pacific"));
        this.ddlUnicefRegion.Items.Insert(3, new ListItem("Eastern and Southern Africa", "Eastern and Southern Africa"));
        this.ddlUnicefRegion.Items.Insert(4, new ListItem("Industrialized Countries/Territories", "Industrialized Countries/Territories"));
        this.ddlUnicefRegion.Items.Insert(5, new ListItem("Latin America and Caribbean", "Latin America and Caribbean"));
        this.ddlUnicefRegion.Items.Insert(6, new ListItem("Middle East and North Africa", "Middle East and North Africa"));
        this.ddlUnicefRegion.Items.Insert(7, new ListItem("South Asia", "South Asia"));
        this.ddlUnicefRegion.Items.Insert(8, new ListItem("Western and Central Africa", "Western and Central Africa"));
        this.ddlUnicefRegion.Items.Insert(9, new ListItem("World", "World"));
    }

    /// <summary>
    /// Get configurations from appsettings.xml file and set into controls
    /// </summary>
    private void SetConfigurationDetails()
    {
        string AppSettingFile = string.Empty;
        XmlDocument XmlDoc;        

        try
        {
            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);

            XmlDoc = new XmlDocument();
            XmlDoc.Load(AppSettingFile);

            #region -- Application --

            txtAdaptationName.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.adaptation_name);
            
            AdaptedFor = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.adapted_for);
            AreaNId = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.area_nid);
            SubNation = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.sub_nation);

            ddlLanguage.SelectedValue = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.default_lng);
            ddlTheme.SelectedValue = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.diuilib_theme_css);
            
            if (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.show_sliders) == "true")
            {
                chkShowSliders.Checked = true;
            }
            else
            {
                chkShowSliders.Checked = false;
            }

            this.txtSliderCount.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.slider_count);


            txtMrdThreshold.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.dvMrdThreshold);

                       
            if (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.dvHideSource) == "true")
            {
                chkHideSrc.Checked = true;
            }
            else
            {
                chkHideSrc.Checked = false;
            }               
          
            if (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isQdsGeneratedForDensedQS_Areas) == "true")
            {
                chkQDSCache.Checked = true;
            }
            else
            {
                chkQDSCache.Checked = false;
            }

                        
            if (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.showdisputed) == "true")
            {
                chkDIB.Checked = true;
            }
            else
            {
                chkDIB.Checked = false;
            }
            

            if (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.adaptation_mode) == "true")
            {
                rbAdpModeRegistry.Checked = true;                
            }
            else
            {
                rbAdpModeDI7.Checked = true;
            }

            this.txtJSVersion.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.js_version);
            this.txtApplicationVersion.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.app_version);

            this.txtAdaptationYear.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.adaptation_year);

            this.ddlUnicefRegion.SelectedValue = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.unicef_region);

            if (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isDesktopVersionAvailable) == "true")
            {
                chkDesktopVerAvailable.Checked = true;
            }
            else
            {
                chkDesktopVerAvailable.Checked = false;
            }

            if (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.enableQDSGallery) == "true")
            {
                chkQDSGallery.Checked = true;
            }
            else
            {
                chkQDSGallery.Checked = false;
            }

            chkNewsMenu.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isNewsMenuVisible) == "true") ? true : false;
            chkInnovationsMenu.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isInnovationsMenuVisible) == "true") ? true : false;
            chkContactUs.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isContactUsMenuVisible) == "true") ? true : false;
            chkSupport.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isSupportMenuVisible) == "true") ? true : false;
            chkDownloads.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isDownloadsLinkVisible) == "true") ? true : false;
            chkTraining.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isTrainingLinkVisible) == "true") ? true : false;
            chkMapLibrary.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isMapLibraryLinkVisible) == "true") ? true : false;
            chkRSSFeeds.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isRSSFeedsLinkVisible) == "true") ? true : false;            
            chkDIWorldwide.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isDiWorldWideLinkVisible) == "true") ? true : false;
            chkHowToVideo.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isHowToVideoVisible) == "true") ? true : false;
            chkSitemap.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.isSitemapVisible) == "true") ? true : false;

            txtAnimationTime.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.SliderAnimationSpeed);
            chkFAQ.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.islangFAQVisible) == "true") ? true : false;
            chkKB.Checked = (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.islangKBVisible) == "true") ? true : false;            
                    

            #endregion  

            #region "-- Contact of database administrator --"

            this.txtAdmName.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.ContactDbAdmName);
            this.txtAdmInstitution.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.ContactDbAdmInstitution);
            this.txtAdmEmail.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.ContactDbAdmEmail);

            #endregion

            #region -- Web Components --

            txtComponentVersion.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.diuilib_version);
            txtDiUiLibUrl.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.diuilib_url);

            #endregion

            #region -- Share --

            txtFBAppID.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.fbAppID);
            txtFBAppSecret.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.fbAppSecret);
            txtTwAppID.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.twAppID);
            txtTwAppSecret.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.twAppSecret);
            Global.CheckAppSetting(XmlDoc, Constants.AppSettingKeys.gaUniqueID,string.Empty);
            txtGoogleAnalyticsId.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.gaUniqueID);                      

            #endregion

            #region -- Registry --

            if (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.enableDSDSelection) == "true")
            {
                chkDSDSel.Checked = true;
            }
            else
            {
                chkDSDSel.Checked = false;
            }
            txtAreaLevel.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryAreaLevel);
            txtMSDAreaId.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMSDAreaId);
            if (Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryNotifyViaEmail) == "true")
            {
                chkNotifyViaEmail.Checked = true;
            }
            else
            {
                chkNotifyViaEmail.Checked = false;
            }
            txtAgeDefaultvalue.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingAgeDefaultValue);
            txtSexDefaultvalue.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingSexDefaultValue);
            txtLocationDefaultvalue.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingLocationDefaultValue);
            txtFrequencyDefaultvalue.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingFrequencyDefaultValue);
            txtSourceDefaultvalue.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingSourceTypeDefaultValue);
            txtNatureDefaultvalue.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingNatureDefaultValue);
            txtUnitMultDefaultvalue.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.registryMappingUnitMultDefaultValue);

            #endregion

            #region -- How to Video

            dynamic dynObj = JsonConvert.DeserializeObject(Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.HowToVideoData).ToString());
            DataTable DTLanguage = Global.GetAllDBLangaugeCodes();
            StringBuilder SB = new StringBuilder();
            foreach (var data in dynObj)
            {
                foreach (DataRow row in DTLanguage.Rows)
                {
                    if (row["Language_Code"].ToString() == (data).Name)
                    {
                        if (string.IsNullOrEmpty(VAribaleDeclaredInpage))
                        {
                            VAribaleDeclaredInpage = row["Language_Code"].ToString();
                        }
                        else
                        {
                            VAribaleDeclaredInpage = VAribaleDeclaredInpage + "," + row["Language_Code"].ToString();
                        }
                        try
                        {
                            SB.Append(SetHowToVideoControls(data, row["Language_Code"].ToString()));
                        }
                        catch (Exception ex)
                        {
                            Global.CreateExceptionString(ex, null);
                        }
                    }
                }
            }
            dvHowToVideo.InnerHtml = SB.ToString();
            MetaTag_Desc_Val.Value = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.MetaTag_Desc);
            MetaTag_Kw_Val.Value = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.MetaTag_Kw);
            #endregion
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private string SetHowToVideoControls(dynamic data, string LanguageCode)
    {
        string RetVal = string.Empty;
        try
        {
            foreach (JArray item in data)
            {                
                foreach (JToken dataItem in item.Children())
                {                   
                    RetVal = "<fieldset id='" + LanguageCode + "' class='confg_frm_inp_bx_txt pos_rel' style='width:530px;margin-left:35px;margin-bottom:20px'><span class='fldset_lbl'>" + Global.GetLanguageKeyValue("Text" + LanguageCode) + "</span><p><span id='lngHowToVideo" + LanguageCode + "H' style='width:210px; display:inline-block'>" + Global.GetLanguageKeyValue("ShowHowToVideoOnHome") + "</span>";
                    if (dataItem["HomeVisible"].ToString().Replace("\"", "") == "true")
                    {
                        RetVal += "<input ID='chkHowToVideo" + LanguageCode + "H' type='checkbox' checked='checked'></input>";
                    }
                    else
                    {
                        RetVal += "<input ID='chkHowToVideo" + LanguageCode + "H' type='checkbox'></input>";
                    }
                    RetVal += "&nbsp;&nbsp;<input ID='txtHowToVideoLink" + LanguageCode + "H' type='text' class='confg_frm_inp_bx_txt' value='" + dataItem["Homelink"].ToString().Replace("\"", "") + "'></input></p><div class='clear'></div><p><span id='lngHowToVideo" + LanguageCode + " V' style='width:210px; display:inline-block'>" + Global.GetLanguageKeyValue("ShowHowToVideoOnVisualizer") + "</span>";
                    if (dataItem["VisualizerVisible"].ToString().Replace("\"", "") == "true")
                    {
                        RetVal += "<input ID='chkHowToVideo" + LanguageCode + "V' type='checkbox' checked='checked'/>";
                    }
                    else
                    {
                        RetVal += "<input ID='chkHowToVideo" + LanguageCode + "V' type='checkbox' />";
                    }
                    RetVal += "&nbsp;&nbsp;<input  ID='txtHowToVideoLink" + LanguageCode + "V' type='text' class='confg_frm_inp_bx_txt' value='" + dataItem["Visualizerlink"].ToString().Replace("\"", "") + "'></input></p><div class='clear'></div></fieldset>";
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    

    private void SetTooltipValues()
    {
        try
        {
            MRDTooltip = Global.GetLanguageKeyValue("MRDTooltip");
            HideSrcColumnTooltip = Global.GetLanguageKeyValue("HideSourceColoumnTooltip");
            DIBTooltip = Global.GetLanguageKeyValue("DIBTooltip");            
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    #endregion

    #endregion

    #region "--Public/Protected--"

    #region "--Variables--"   

    protected string AdaptedFor = string.Empty;
    protected string AreaNId = string.Empty;
    protected string SubNation = string.Empty;

    protected string MRDTooltip = string.Empty;
    protected string HideSrcColumnTooltip = string.Empty;
    protected string DIBTooltip = string.Empty;

    #endregion

    #region "--Methods/Events--"

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[Constants.SessionNames.LoggedAdminUser] == null)
        {
            Response.Redirect("AdminLogin.aspx");
        }
        else
        {
            // Read AppSettings
            Global.GetAppSetting();

            // Set page title
            Page.Title = Global.adaptation_name;

            this.FillLanguageDDL();

            this.FillThemes();

            this.FillUnicefRegionDDL();

            this.SetConfigurationDetails();

            this.SetTooltipValues();
            this.GetPostedData();
        }
    }
    private void GetPostedData()
    {
        try
        {
            // Set language code   
            if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value)))
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
            // Get Posted Data - will be passed to the Javascript            

            if (!string.IsNullOrEmpty(Request["hlngcode"])) { hlngcode = Request["hlngcode"]; }

            //if (!string.IsNullOrEmpty(Request["hLoggedInUserNId"])) { hLoggedInUserNId = Request["hLoggedInUserNId"]; }
            if (Session[Constants.SessionNames.LoggedAdminUserNId] != null && Session[Constants.SessionNames.LoggedAdminUserNId].ToString() != "")
            {
                hLoggedInUserNId = Session[Constants.SessionNames.LoggedAdminUserNId].ToString();
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }
    #endregion

    #endregion
}
