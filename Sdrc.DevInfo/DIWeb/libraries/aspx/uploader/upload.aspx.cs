using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class libraries_aspx_uploader_upload : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string CKEditorFuncNum = Request["CKEditorFuncNum"].ToString();
        string file = Path.GetFileName(Request.Files[0].FileName);
        string url = "diorg/images/news_images/" + file;
        Request.Files[0].SaveAs(Server.MapPath("../diorg/images/news_images/" + Path.GetFileName(Request.Files[0].FileName)));
        Response.Write("<script>window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ", \"" + url + "\");</script>");
    }
}