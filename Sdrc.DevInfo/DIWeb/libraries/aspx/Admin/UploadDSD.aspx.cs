using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class libraries_aspx_Admin_UploadDSD : System.Web.UI.Page
{
    # region "Private"

    # endregion

    # region "Public/Protected"

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

    # endregion

}