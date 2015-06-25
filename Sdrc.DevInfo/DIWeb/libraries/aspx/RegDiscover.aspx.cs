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

public partial class libraries_aspx_RegDiscover : System.Web.UI.Page
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
            Page.Title = Global.adaptation_name + " - Registry - " + Constants.Pages.Registry.Discover;

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
        catch (Exception)
        {
        }
    }

    private void GetPostedData()
    {
        // Get Posted Data - will be passed to the Javascript
        if (!string.IsNullOrEmpty(Request["hdsby"])) { hdsby = Request["hdsby"]; }
        if (!string.IsNullOrEmpty(Request["hdbnid"])) {

            hdbnid = Request["hdbnid"];
            //if (hdbnid != Global.GetDefaultDSDNId() && Global.GetDefaultDSDNId() != string.Empty)
            //{
            //    hdbnid = Global.GetDefaultDSDNId();
            //}
            //else
            //{
            //    hdbnid = Request["hdbnid"];
            //}
        }
        if (!string.IsNullOrEmpty(Request["hselarea"])) { hselarea = Request["hselarea"]; }
        if (!string.IsNullOrEmpty(Request["hselind"])) { hselind = Request["hselind"]; }
        if (!string.IsNullOrEmpty(Request["hlngcode"])) { hlngcode = Request["hlngcode"]; }
        if (!string.IsNullOrEmpty(Request["hlngcodedb"])) { hlngcodedb = Request["hlngcodedb"]; }
        if (!string.IsNullOrEmpty(Request["hselindo"])) { hselindo = Request["hselindo"]; }
        if (!string.IsNullOrEmpty(Request["hselareao"])) { hselareao = Request["hselareao"]; }
        if (!string.IsNullOrEmpty(Request["hLoggedInUserNId"])) { hLoggedInUserNId = Request["hLoggedInUserNId"]; }
        if (!string.IsNullOrEmpty(Request["hLoggedInUserName"])) { hLoggedInUserName = Request["hLoggedInUserName"]; }

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
