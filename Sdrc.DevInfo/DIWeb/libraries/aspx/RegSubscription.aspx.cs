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
using System.Text;
using System.Xml;
using SDMXObjectModel;
using SDMXObjectModel.Registry;

public partial class libraries_aspx_SDMX_sdmx_Subscription : System.Web.UI.Page
{
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
    protected string hOriginaldbnid = string.Empty;
    protected string hIsUploadedDSD = string.Empty;


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Read AppSettings
            Global.GetAppSetting();

            // Set page title
            Page.Title = Global.adaptation_name + " - Registry - " + Constants.Pages.Registry.Subscription;

            // Read http header or cookie values
            GetPostedData();
            bool IsKeyFound = false;
            string ProtectedValue = string.Empty;
            if (ConfigurationManager.AppSettings["AppProtected"] != null)
            {
                IsKeyFound = true;
                ProtectedValue = ConfigurationManager.AppSettings["AppProtected"];
            }
            if (IsKeyFound && ProtectedValue == "true" && Session["hLoggedInUserNId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                ((libraries_aspx_RegistryMaster)this.Master).Populate_Select_DSD_DropDown(hdbnid);
                ((libraries_aspx_RegistryMaster)this.Master).ShowHide_Select_DSD_DropDown(Global.enableDSDSelection);

            }
            
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void GetPostedData()
    {
        // Get Posted Data - will be passed to the Javascript
        if (!string.IsNullOrEmpty(Request["hdsby"]))
        {
            hdsby = Request["hdsby"];
            Session["hdsby"] = Request["hdsby"];
        }
        else
            hdsby = Session["hdsby"] != null ? Session["hdsby"].ToString() : null;
        if (!string.IsNullOrEmpty(Request["hdbnid"]))
        {
            hdbnid = Request["hdbnid"];
            Session["hdbnid"] = Request["hdbnid"];
            //if (hdbnid != Global.GetDefaultDSDNId() && Global.GetDefaultDSDNId() != string.Empty)
            //{
            //    hdbnid = Global.GetDefaultDSDNId();
            //    Session["hdbnid"] = Convert.ToString(hdbnid);
            //}
            //else
            //{
            //    Session["hdbnid"] = Request["hdbnid"];
            //}
        }
        else
            hdbnid = Session["hdbnid"] != null ? Session["hdbnid"].ToString() : null;// Global.GetDefaultDbNId();
        if (!string.IsNullOrEmpty(Request["hselarea"]))
        {
            hselarea = Request["hselarea"];
            Session["hselarea"] = Request["hselarea"];
        }
        else
            hselarea = Session["hselarea"] != null ? Session["hselarea"].ToString() : null;
        if (!string.IsNullOrEmpty(Request["hselind"]))
        {
            hselind = Request["hselind"];
            Session["hselind"] = Request["hselind"];
        }
        else
            hselind = Session["hselind"] != null ? Session["hselind"].ToString() : null;
        if (!string.IsNullOrEmpty(Request["hlngcode"]))
        {
            hlngcode = Request["hlngcode"];
            Session["hlngcode"] = Request["hlngcode"];
        }
        else
            hlngcode = Session["hlngcode"] != null ? Session["hlngcode"].ToString() : null;//Global.GetDefaultLanguageCode();
        if (!string.IsNullOrEmpty(Request["hlngcodedb"]))
        {
            hlngcodedb = Request["hlngcodedb"];
            Session["hlngcodedb"] = Request["hlngcodedb"];
        }
        else
            hlngcodedb = Session["hlngcodedb"] != null ? Session["hlngcodedb"].ToString() : null;// Global.GetDefaultLanguageCode();
        if (!string.IsNullOrEmpty(Request["hselindo"]))
        {
            hselindo = Global.formatQuoteString(Request["hselindo"]);
            Session["hselindo"] = Global.formatQuoteString(Request["hselindo"]);
        }
        else
            hselindo = Session["hselindo"] != null ? Session["hselindo"].ToString() : null;
        if (!string.IsNullOrEmpty(Request["hselareao"]))
        {
            hselareao = Global.formatQuoteString(Request["hselareao"]);
            Session["hselareao"] = Global.formatQuoteString(Request["hselareao"]);
        }
        else
            hselareao = Session["hselareao"] != null ? Session["hselareao"].ToString() : null;
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

        if (Request["hOriginaldbnid"] != null && !string.IsNullOrEmpty(Request["hOriginaldbnid"]))
        {
            hOriginaldbnid = Request["hOriginaldbnid"];
        }
        else
        {
            hOriginaldbnid = Global.GetDefaultDbNId();
        }

        if (Global.GetDefaultDSDNId() != string.Empty && hdbnid==Global.GetDefaultDSDNId())
        {
            hIsUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(Global.GetDefaultDSDNId())).ToString();
        }
        else
        {
            hIsUploadedDSD = Global.IsDSDUploadedFromAdmin(Convert.ToInt32(hdbnid)).ToString();
        }
    }


  

}
