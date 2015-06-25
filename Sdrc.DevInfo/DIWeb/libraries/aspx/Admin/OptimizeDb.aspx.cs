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
using System.IO;
using System.Collections.Generic;
using System.Xml;

public partial class libraries_aspx_Admin_DbEdit2 : System.Web.UI.Page
{
    protected string IsXmlGenerated = "false";
    protected string IsMapGenerated = "false";
    protected string IsSDMXGenerated = "false";
    protected string IsSDMXMLGenerated = "false";
    protected string IsMetadataGenerated = "false";
    protected string IsCacheResultGenerated = "false";
    protected string IsSDMXMLRegistered = "false";
    
    protected string IsXmlAreaGenerated = "false";
    protected string IsXmlFootnotesGenerated = "false";
    protected string IsXmlICGenerated = "false";
    protected string IsXmlICIUSGenerated = "false";
    protected string IsXmlIUSGenerated = "false";    
    protected string IsXmlMetadataGenerated = "false";
    protected string IsXmlQuickSearchGenerated = "false";
    protected string IsXmlTimePeriodsGenerated = "false";
    protected string hCustomParams = "";
    protected string IsSDMXDataPublished = "false";
    protected string IsSDMXDataPublishedCountryData = "false";
    protected string EnableSDMXPublishCheck = "false";
    protected void Page_Load(object sender, EventArgs e)
    {
        string DbId = string.Empty;
        string DefaultLanguage = string.Empty;
        string XmlDataFilesPath = string.Empty;
        Dictionary<string, string> DBConnections = new Dictionary<string, string>();
        string DataPublishUserSelectionFileNameWPath = string.Empty;
        string DBorDSDDBID = string.Empty;
        try
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
                hCustomParams = Global.customParams;

                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    DbId = Request.QueryString["id"];

                    if (Directory.Exists(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbId + "\\ds")))
                    {
                        IsXmlGenerated = "true";

                        DefaultLanguage = "\\" + Global.GetDefaultLanguageCodeDB(DbId, "");

                        XmlDataFilesPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbId + "\\ds" + DefaultLanguage);

                        if (Directory.Exists(Path.Combine(XmlDataFilesPath, Constants.FolderName.Codelists.Area)))
                        {
                            IsXmlAreaGenerated = "true";
                        }

                        if (Directory.Exists(Path.Combine(XmlDataFilesPath, Constants.FolderName.Codelists.Footnotes)))
                        {
                            IsXmlFootnotesGenerated = "true";
                        }

                        if (Directory.Exists(Path.Combine(XmlDataFilesPath, Constants.FolderName.Codelists.IC)))
                        {
                            IsXmlICGenerated = "true";
                        }

                        if (Directory.Exists(Path.Combine(XmlDataFilesPath, Constants.FolderName.Codelists.IC_IUS)))
                        {
                            IsXmlICIUSGenerated = "true";
                        }

                        if (Directory.Exists(Path.Combine(XmlDataFilesPath, Constants.FolderName.Codelists.IUS)))
                        {
                            IsXmlIUSGenerated = "true";
                        }

                        if (Directory.Exists(Path.Combine(XmlDataFilesPath, Constants.FolderName.Codelists.Metadata)))
                        {
                            IsXmlMetadataGenerated = "true";
                        }

                        if (File.Exists(Path.Combine(XmlDataFilesPath, Constants.FolderName.Codelists.Area + "\\qscodelist.xml")))
                        {
                            IsXmlQuickSearchGenerated = "true";
                        }

                        if (Directory.Exists(Path.Combine(XmlDataFilesPath, Constants.FolderName.Codelists.TP)))
                        {
                            IsXmlTimePeriodsGenerated = "true";
                        }
                    }

                    if (Directory.Exists(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbId + "\\sdmx")))
                    {
                        IsSDMXGenerated = "true";
                    }
                    if (Directory.Exists(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbId + "\\sdmx\\SDMX-ML")))
                    {
                        IsSDMXMLGenerated = "true";
                    }
                    if (Directory.Exists(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbId + "\\sdmx\\Metadata")))
                    {
                        IsMetadataGenerated = "true";
                    }
                    if (Directory.Exists(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbId + "\\sdmx\\Registrations")))
                    {
                        IsSDMXMLRegistered = "true";
                    }
                    if (Directory.Exists(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DbId + "\\maps")))
                    {
                        IsMapGenerated = "true";
                    }

                    if (Global.IsCacheResultGenerated(DbId))
                    {
                        IsCacheResultGenerated = "true";
                    }
                    IsSDMXDataPublished=Global.GetNCreateDefaultAppSettingKey(Constants.AppSettingKeys.IsSDMXDataPublished, "false");
                    IsSDMXDataPublishedCountryData = Global.GetNCreateDefaultAppSettingKey(Constants.AppSettingKeys.IsSDMXDataPublishedCountryData, "false");
                    string[] ConnectionDetails = null;
                   

                    Dictionary<string, string> DictConnections = new Dictionary<string, string>();
                    DBConnections = Global.GetAllConnections("DevInfo");
                    foreach (var item in DBConnections.Keys)
                    {
                        ConnectionDetails = Global.GetDbConnectionDetails(Convert.ToString(item));
                        if (Convert.ToString(item) != DbId && ConnectionDetails[4] == "true")
                        {
                            DBorDSDDBID = Convert.ToString(item);
                            DataPublishUserSelectionFileNameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DBorDSDDBID.ToString() + "\\sdmx" + "\\DataPublishedUserSelection.xml");
                            if (File.Exists(DataPublishUserSelectionFileNameWPath))
                            {
                                XmlDocument docConfig = new XmlDocument();
                                docConfig.Load(DataPublishUserSelectionFileNameWPath);
                                string IndNId = string.Empty;
                                ArrayList ALItemAdded = new ArrayList();
                                foreach (XmlElement element in docConfig.SelectNodes("/root/Data"))
                                {
                                    if (element.GetAttribute("selectedState") == "true")
                                    {
                                        chkPublishSDMXDataCountryData.Disabled = false;
                                        EnableSDMXPublishCheck = "true";
                                        break;
                                    }
                                    else
                                    {
                                        chkPublishSDMXDataCountryData.Disabled = true;
                                        EnableSDMXPublishCheck = "false";
                                    }
                                }
                              
                            }
                            else
                            {
                                chkPublishSDMXDataCountryData.Disabled = true;
                                EnableSDMXPublishCheck = "false";
                            }
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
}
