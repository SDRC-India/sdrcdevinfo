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

public partial class libraries_aspx_Admin_ProviderDetails : System.Web.UI.Page
{
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
        }
    }
}
