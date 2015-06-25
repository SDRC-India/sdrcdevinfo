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
using System.IO;
using System.Xml;

public partial class libraries_aspx_SDMX_sdmx_Download : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request["fileId"] != null && Request["fileName"] == null && Request["fileType"] == null)
        {
            downloadFile(Request["fileId"].ToString(),null,null);
        }
        else if (Request["fileId"] != null && Request["fileName"] != null && Request["fileType"] != null)
        {
            downloadFile(Request["fileId"].ToString(), Request["fileName"].ToString(), Request["fileType"].ToString());
        }
        else if (!string.IsNullOrEmpty(Request["hDownload"]))
        {
            downloadContent(Request["hDownload"].ToString(), Request["hFileName"].ToString(), Request["hFileType"].ToString());
        }
    }

    /// <summary>
    /// Download a file
    /// </summary>
    /// <param name="filePath">File path</param>
    /// <param name="newFileName">New filename</param>
    /// <param name="fileType">file type</param>
    private void downloadFile(string filePath, string newFileName, string fileType)
    {
        try
        {
            // ::->Make  a filestream
            FileStream fs;

            // ::->Get filename
            string fileName;
            string folderPath = filePath.Substring(filePath.IndexOf("stock"));
            if (string.IsNullOrEmpty(newFileName))
            {
                fileName = filePath.Substring(filePath.LastIndexOf('/') + 1, filePath.Length - filePath.LastIndexOf('/') - 1);
            }            
            else
            {
                if (newFileName.Length > 240)
                {
                    newFileName = newFileName.Substring(0, 240);
                }
                fileName = newFileName + filePath.Substring(filePath.LastIndexOf('.'));
            }

            // ::->Open file
            if (File.Exists(Server.MapPath("~/" + folderPath)))
            {
                fs = File.Open(Server.MapPath("~/" + folderPath), FileMode.Open);

                // ::->Convert file in the bytes
                byte[] byteBuffer = new byte[fs.Length];
                fs.Read(byteBuffer, 0, (int)fs.Length);

                // ::->CLose the file
                fs.Close();

                // ::->Code for adding the Confirmation Dialog
                Response.ClearContent();
                Response.ClearHeaders();                
                if (fileType == "xml")
                {
                    Response.ContentType = "application/xml";                    
                }
                else if (fileType == "svg")
                {
                    Response.ContentType = "image/svg+xml";
                }
                else if (fileType == "png")
                {
                    Response.ContentType = "image/png";
                }
                else if (fileType == "xls")
                {
                    Response.ContentType = "application/vnd.xls";
                }
                else if (fileType == "txt")
                {
                    Response.ContentType = "text/plain";
                }
                else
                {
                    Response.ContentType = "application/octet-stream";
                }
                Response.AddHeader("Content-disposition", "attachment; filename=\"" + fileName + "\"");
                Response.BinaryWrite(byteBuffer);                
                Response.End();
                Response.SuppressContent = true;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void downloadContent(string Content, string fileName, string fileType)
    {
        XmlDocument Document;
        StreamWriter Writer;
        MemoryStream Stream;
        string EncodedContent = string.Empty;
        Stream = new MemoryStream();

        if (fileType == "xml")
        {
            Document = new XmlDocument();            
            Document.LoadXml(this.Page.Server.HtmlDecode(Content));
            Document.Save(Stream);
        }
        else if (fileType == "txt")
        {
            Writer = new StreamWriter(Stream);
            Writer.Write(Content);
            Writer.Flush();
        }        
        Stream.Seek(0, SeekOrigin.Begin);

        this.Page.Response.ClearContent();
        if (fileType == "xml")
        {
            this.Page.Response.ContentType = "application/xml";
        }
        else if (fileType == "txt")
        {
            this.Page.Response.ContentType = "text/plain";
        }

        this.Page.Response.AddHeader("Content-Length", Stream.ToArray().Length.ToString());
        this.Page.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName + "." + fileType);
        this.Page.Response.BinaryWrite(Stream.ToArray());
        this.Page.Response.End();
    }
}
