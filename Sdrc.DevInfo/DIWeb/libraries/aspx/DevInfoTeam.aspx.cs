using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class libraries_aspx_DevInfoTeam : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Read AppSettings
        Global.GetAppSetting();

        // Set page title
        Page.Title = Global.adaptation_name;
    }
}