using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class libraries_aspx_MapInputFile : System.Web.UI.Page
{
    string key = string.Empty;
    /// <summary>
    /// Loading page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string FilePath = string.Empty;
        string tempPath = Server.MapPath("../../stock/tempCYV");
        string ThemeId = string.Empty;
        string RetVal = string.Empty;
        FilePath = tempPath + "\\" + Environment.TickCount.ToString();
        #region LegendInfo serialized object is Uploading
        if (Request.QueryString["UploadType"] == "LegendInfo") // Legend information save Format
        {
            if (Request.Form["themeId"] != null)
            {
                ThemeId = Request.Form["themeId"].ToString();
            }

            if (!string.IsNullOrEmpty(ThemeId))
            {
                FilePath += ".xml";
                //TODO check for valid xml
                Request.Files[0].SaveAs(FilePath);
                //Callback cback = new Callback(this.Page);
                //cback.UploadLegends(ThemeId, FilePath);
            }
            
            RetVal = FilePath;
        }
        #endregion       
        Response.Write(RetVal);
    }


}