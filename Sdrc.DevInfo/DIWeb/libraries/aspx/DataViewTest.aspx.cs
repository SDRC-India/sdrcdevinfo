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

public partial class libraries_aspx_DataView : System.Web.UI.Page
{
    protected string hdsby = string.Empty;
    protected string hdbnid = string.Empty;
    protected string hselarea = string.Empty;
    protected string hselind = string.Empty;
    protected string hlngcode = string.Empty;
    protected string hlngcodedb = string.Empty;
    protected string hselindo = string.Empty;
    protected string hselareao = string.Empty;
    protected string hdvnids = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Set pate title
            Page.Title = "CAMInfo";

            // Read http header or cookie values
            GetPostedData();

            // Read AppSettings
            Global.GetAppSetting();
        }
        catch (Exception)
        {
        }
    }


    private void GetPostedData()
    {
        // Get Posted Data - will be passed to the Javascript
        if (!string.IsNullOrEmpty(Request["hdsby"])) { hdsby = Request["hdsby"]; }
        if (!string.IsNullOrEmpty(Request["hdbnid"])) { hdbnid = Request["hdbnid"]; }
        if (!string.IsNullOrEmpty(Request["hselarea"])) { hselarea = Request["hselarea"]; }
        if (!string.IsNullOrEmpty(Request["hselind"])) { hselind = Request["hselind"]; }
        if (!string.IsNullOrEmpty(Request["hlngcode"])) { hlngcode = Request["hlngcode"]; }
        if (!string.IsNullOrEmpty(Request["hlngcodedb"])) { hlngcodedb = Request["hlngcodedb"]; }
        if (!string.IsNullOrEmpty(Request["hselindo"])) { hselindo = Request["hselindo"]; }
        if (!string.IsNullOrEmpty(Request["hselareao"])) { hselareao = Request["hselareao"]; }
        if (!string.IsNullOrEmpty(Request["hdvnids"])) { hdvnids = Request["hdvnids"]; }
    }
}
