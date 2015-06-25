using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class libraries_aspx_Admin_User : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[Constants.SessionNames.LoggedAdminUser] == null)
        {
            Response.Redirect("AdminLogin.aspx");
        }
        else
        {
            // Set page title
            Page.Title = Global.adaptation_name;
            this.BindUsersGrid();
        }
    }

    private void BindUsersGrid()
    {
        Callback objCallback;

        try
        {
            objCallback = new Callback(Page);
            this.divUsersList.InnerHtml = objCallback.GetAllUsersGridHTML();
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }
}