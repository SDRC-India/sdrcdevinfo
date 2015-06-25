using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Diagnostics;
using System.Drawing.Imaging;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using System.Text;
using SpreadsheetGear;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using System.Xml;
using System.Collections.Generic;

public partial class libraries_aspx_ExportVisualization : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        #region variables
        string tType = string.Empty;
        string tFileName =string.Empty;
        string tSvg = string.Empty, tTitle = string.Empty, tSubtitle = string.Empty, tSource = string.Empty, tTableData = string.Empty, tKeywords = string.Empty;
        #endregion
        
        #region getting information from posted form
        if (Request.Form[Constants.XLSDownloadType.TypeText] != null)
        {
            tType = Request.Form[Constants.XLSDownloadType.TypeText].ToString();
        }
        if (Request.Form[Constants.XLSDownloadType.FilenameText] != null)
        {
            tFileName = Request.Form[Constants.XLSDownloadType.FilenameText].ToString();
        }
        if (string.IsNullOrEmpty(tFileName))
        {
            tFileName = Constants.XLSDownloadType.DefaultFilename;
        }
        if (!string.IsNullOrEmpty(Request.Form[Constants.XLSDownloadType.SvgText].ToString())) // Generate svg file on temp,location
        {
            tSvg = Request.Form[Constants.XLSDownloadType.SvgText].ToString();
        }
        if (Request.Form["title"] != null)
        {
            tTitle = Request.Form["title"].ToString();
        }
        if (Request.Form["subtitle"] != null)
        {
            tSubtitle = Request.Form["subtitle"].ToString();
        }
        if (Request.Form["source"] != null)
        {
            tSource = Request.Form["source"].ToString();
        }
        if (Request.Form["tabledata"] != null)
        {
            tTableData = Request.Form["tabledata"].ToString();
        }
        if (Request.Form["keywords"] != null)
        {
            tKeywords = Request.Form["keywords"].ToString();
        }
        #endregion
        string tExt = Constants.XLSDownloadType.XlsExtention;
        MemoryStream tData = new MemoryStream();
        Callback callbackObj = new Callback(this.Page);
        IWorkbook workbook = callbackObj.getWorkbookXLS(tTitle, tSubtitle, tSource, tKeywords, tTableData, tSvg);
        workbook.SaveToStream(tData, FileFormat.Excel8);
        tData.Close();
        tData.Dispose();
        workbook.Close();
        #region Download at client side
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = tType;
        Response.AppendHeader("Content-Disposition", "attachment; filename=\"" + tFileName + tExt + "\"");
        Response.BinaryWrite(tData.ToArray());
        Response.End();
        #endregion        
    }
}