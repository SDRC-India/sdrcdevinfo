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

public partial class libraries_aspx_SaveXLS : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string input = Request["settings"];
        string filename = Request["filename"];
        Response.Clear();
        Response.AddHeader("Content-disposition", "attachment; filename=" + filename);
        input = input.Replace("\"$category$\"", "$category$");
        input = input.Replace("\"$plotOptionId$\"", "$plotOptionId$");
        input = input.Replace("\"$seriesData$\"", "$seriesData$");
        this.Page.Response.Write(input);
        Response.End();        
    }   
}
