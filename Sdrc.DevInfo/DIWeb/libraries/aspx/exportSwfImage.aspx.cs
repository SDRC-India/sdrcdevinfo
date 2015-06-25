using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.IO;
using SpreadsheetGear;
using System.Drawing.Imaging;
using System.Xml;

public partial class libraries_aspx_exportSwfImage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Bitmap map;
        if (Request["chartImg"] != null && Request["chartType"] != null && Request["tableData"] != null && Request["source"] != null && Request["keywords"] != null && Request["title"] != null && Request["subtitle"] != null && Request["filename"] != null)
        {
            string imageBitString = Request["chartImg"].ToString();
            string imageType = Request["chartType"].ToString();
            string tTableData = Request["tableData"].ToString();
            string tSource = Request["source"].ToString();
            string tKeywords = Request["keywords"].ToString();
            string tTitle = Request["title"].ToString();
            string tSubtitle = Request["subtitle"].ToString();
            string tfilename = Request["filename"].ToString();
            if (String.IsNullOrEmpty(tfilename))
                tfilename = "radar";
            string extenstion =string.Empty;
            string contentType = string.Empty;
            MemoryStream tData = null;
            switch(imageType.ToLower())
            {
                case "image/png":
                    tData = new MemoryStream();
                    MemoryStream picStream = new MemoryStream(Convert.FromBase64String(imageBitString));
                    extenstion = ".png";
                    contentType = "image/png";
                    tData = picStream;
                    picStream.Close();
                    picStream.Dispose();                    
                    break;
                case "application/vnd.xls":
                    tData = new MemoryStream();
                    Callback callbackObj = new Callback(this.Page);
                    IWorkbook workbook = callbackObj.getWorkbookForSwf(tTitle, tSubtitle, tSource, tKeywords, tTableData, imageBitString);
                    if (workbook!=null)
                        workbook.SaveToStream(tData, FileFormat.XLS97);
                    else
                        workbook.SaveToStream(tData, FileFormat.XLS97);
                    workbook.Close();
                    extenstion = ".xls";
                    contentType = "application/vnd.xls";
                    break;
            }
            tData.Close();
            tData.Dispose();            
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = contentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + tfilename + extenstion +"");
            Response.BinaryWrite(tData.ToArray());
            Response.End();
        }
    }
    
}