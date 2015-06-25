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

public partial class libraries_aspx_Selection : System.Web.UI.Page
{
    protected string hdsby = string.Empty;
    protected string hdbnid = string.Empty;
    protected string hselarea = string.Empty;
    protected string hselind = string.Empty;
    protected string hlngcode = string.Empty;
    protected string hlngcodedb = string.Empty;
    protected string hselindo = string.Empty;
    protected string hselareao = string.Empty;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Read http header or cookie values
            GetPostedData();

            // Read AppSettings
            Global.GetAppSetting();
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
        }
        else
            hdbnid = Session["hdbnid"] != null ? Session["hdbnid"].ToString() : null;
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
            hlngcode = Session["hlngcode"] != null ? Session["hlngcode"].ToString() : null;
        if (!string.IsNullOrEmpty(Request["hlngcodedb"]))
        {
            hlngcodedb = Request["hlngcodedb"];
            Session["hlngcodedb"] = Request["hlngcodedb"];
        }
        else
            hlngcodedb = Session["hlngcodedb"] != null ? Session["hlngcodedb"].ToString() : null;
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
    }
}
