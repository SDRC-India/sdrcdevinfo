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

public partial class libraries_aspx_CYV : System.Web.UI.Page
{
    public string _QueryType = "";
    public string _QuerySType = "Column";

    protected void Page_Load(object sender, EventArgs e)
    {
        string CurrentType = Request.QueryString["type"];
        if (CurrentType == "map" || CurrentType == "scatter")
        {
            _QueryType = CurrentType;
        }
        else
        {
            _QueryType = "chart";
            _QuerySType = CurrentType;
        }
    }
}
