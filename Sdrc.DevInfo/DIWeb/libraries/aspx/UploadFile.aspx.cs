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

public partial class libraries_aspx_UploadFile : System.Web.UI.Page
{

    #region "--Private--"

    #region "--Methods--"

    private void handleUploadedFileValidation()
    {
        string tempPath = Server.MapPath("../../stock/tempSDMXFiles");
        string FileName = string.Empty;

        if (Request.Files[0].InputStream.Length > 0)
        {
            if (!(Directory.Exists(tempPath)))
            {
                Directory.CreateDirectory(tempPath);
            }
            if (!string.IsNullOrEmpty(Request.Files.AllKeys[0]) && (Request.Files.AllKeys[0] == "UplInputSdmxMlFile"))
            {
                FileName = "SDMX_" + Guid.NewGuid().ToString() + ".xml";
            }
            else if (!string.IsNullOrEmpty(Request.Files.AllKeys[0]) && (Request.Files.AllKeys[0] == "UplInputDSDFile"))
            {
                FileName = "DSD_" + Guid.NewGuid().ToString() + ".xml";
            }
            else if (!string.IsNullOrEmpty(Request.Files.AllKeys[0]) && (Request.Files.AllKeys[0] == "UplInputMetadataReportFile"))
            {
                FileName = "Metadata_" + Guid.NewGuid().ToString() + ".xml";
            }

            string FilePath = tempPath + "\\" + FileName;

            if (Request.Files[0].FileName.EndsWith(".xml"))
            {
                Request.Files[0].SaveAs(FilePath);
            }

            Response.Write(FilePath);
        }
        else
        {
            Response.Write("No File");
        }

    }

    private void handleUploadedFileComparison()
    {
        string tempPath = Server.MapPath("../../stock/tempSDMXFiles");
        string FileName1 = string.Empty;
        string FileName2 = string.Empty;
        string FilePath1 = string.Empty;
        string FilePath2 = string.Empty;

        if (!(Directory.Exists(tempPath)))
        {
            Directory.CreateDirectory(tempPath);
        }

        if (!string.IsNullOrEmpty(Request.Files.AllKeys[0]) && (Request.Files.AllKeys[0] == "UplDSDAgainstDevInfoDSD"))
        {
            if (Request.Files[0].InputStream.Length > 0)
            {
                FileName1 = "DSDAgainstDevInfoDSD_" + Environment.TickCount.ToString() + ".xml";
                FilePath1 = tempPath + "\\" + FileName1;
                if (Request.Files[0].FileName.EndsWith(".xml"))
                {
                    Request.Files[0].SaveAs(FilePath1);
                }
                Response.Write(FilePath1);
            }
            else
            {
                Response.Write("No File");
            }
        }
        else
        {
            if ((Request.Files[0].InputStream.Length > 0) && (Request.Files[1].InputStream.Length > 0))
            {
                FileName1 = "DSD1_" + Environment.TickCount.ToString() + ".xml";
                FileName2 = "DSD2_" + Environment.TickCount.ToString() + ".xml";
                FilePath1 = tempPath + "\\" + FileName1;
                FilePath2 = tempPath + "\\" + FileName2;

                if (Request.Files[0].FileName.EndsWith(".xml"))
                {
                    Request.Files[0].SaveAs(FilePath1);
                }
                if (Request.Files[1].FileName.EndsWith(".xml"))
                {
                    Request.Files[1].SaveAs(FilePath2);
                }
                Response.Write(FilePath1 + Constants.Delimiters.Comma + FilePath2);
            }
            else
            {
                Response.Write("No File");
            }
        }



    }

    private void handleUploadedDSDFromAdmin()
    {
        string tempPath = Server.MapPath("../../stock/tempSDMXFiles");
        string FileName = string.Empty;

        if (Request.Files[0].InputStream.Length > 0)
        {
            if (!(Directory.Exists(tempPath)))
            {
                Directory.CreateDirectory(tempPath);
            }
            if (!string.IsNullOrEmpty(Request.Files.AllKeys[0]) && ((Request.Files.AllKeys[0] == "UplInputDSDFileFromAdmin") || (Request.Files.AllKeys[0] == "UpdateUplDSDFileFromAdmin")))
            {
                FileName = "AdminDSD_" + Guid.NewGuid().ToString() + ".xml";
            }

            string FilePath = tempPath + "\\" + FileName;

            if (Request.Files[0].FileName.EndsWith(".xml"))
            {
                Request.Files[0].SaveAs(FilePath);
            }

            Response.Write(FilePath);
        }
        else
        {
            Response.Write("No File");
        }

    }
    private void handleUploadedExcel()
    {
        string tempPath = Server.MapPath("../../stock/tempMappingFiles");
        string FileName = string.Empty;
        string FileExtension = string.Empty;
         string FilePath=string.Empty;
        if (Request.Files[0].InputStream.Length > 0)
        {
            if (!(Directory.Exists(tempPath)))
            {
                Directory.CreateDirectory(tempPath);
            }
            if (!string.IsNullOrEmpty(Request.Files.AllKeys[0]) && ((Request.Files.AllKeys[0] == "ImportMappingExcel") || (Request.Files.AllKeys[0] == "ImportMappingExcel")))
            {
                FileName = Request.Files[0].FileName;
                FileName = Path.GetFileName(FileName);
                FileExtension = Path.GetExtension(Request.Files[0].FileName).Substring(1);
                // Allow further processing if input file is xls file
                if (FileExtension.ToLower() == "xls")
                {
                    FilePath = tempPath + "\\" + FileName;
                    Request.Files[0].SaveAs(FilePath);
                }
                else
                { Response.Write("Invalid File"); }
            }


            Response.Write(FilePath);
        }
        else
        {
            Response.Write("No File");
        }

    }

    

    /// <summary>
    /// Upload adaptaion logo image
    /// </summary>
    private void HandleUploadedLogoImageFile()
    {
        string AdaptationLogoImagesPath = string.Empty;
        string FileName = string.Empty;
        string SaveFilePath = string.Empty;
        string FilePathUrl = string.Empty;

        try
        {
            AdaptationLogoImagesPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.AdaptationLogoImages);

            if (Request.Files[0].InputStream.Length > 0)
            {
                if (!(Directory.Exists(AdaptationLogoImagesPath)))
                {
                    Directory.CreateDirectory(AdaptationLogoImagesPath);
                }
                if (!string.IsNullOrEmpty(Request.Files.AllKeys[0]) && ((Request.Files.AllKeys[0] == "flCatalogImageName") || (Request.Files.AllKeys[0] == "flCatalogImageName")))
                {
                    FileName = "Logo_" + Guid.NewGuid().ToString() + ".png";
                }

                SaveFilePath = Path.Combine(AdaptationLogoImagesPath, FileName);

                Request.Files[0].SaveAs(SaveFilePath);

                FilePathUrl = Global.GetAdaptationUrl() + "/" + Constants.FolderName.AdaptationLogoImages.Replace("\\", "/") + FileName;

                Response.Write(FilePathUrl);
            }
            else
            {
                Response.Write("No File");
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }
    }   

    #endregion

    #endregion

    #region "--Public/Protected--"

    #region "--Methods/Events--"

    protected void Page_Load(object sender, EventArgs e)
    {
        
        bool IsKeyFound = false;
            string ProtectedValue = string.Empty;
            if (ConfigurationManager.AppSettings["AppProtected"] != null)
            {
                IsKeyFound = true;
                ProtectedValue = ConfigurationManager.AppSettings["AppProtected"];
            }
            if (IsKeyFound && ProtectedValue == "true" && Session["hLoggedInUserNId"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                if (Request.Files.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Request.Files.AllKeys[0]) && ((Request.Files.AllKeys[0] == "UplInputSdmxMlFile") || (Request.Files.AllKeys[0] == "UplInputDSDFile") || (Request.Files.AllKeys[0] == "UplInputMetadataReportFile")))
                    {
                        handleUploadedFileValidation();
                    }
                    else if (!string.IsNullOrEmpty(Request.Files.AllKeys[0]) && ((Request.Files.AllKeys[0] == "UplInputDSDFileFromAdmin") || (Request.Files.AllKeys[0] == "UpdateUplDSDFileFromAdmin")))
                    {
                        handleUploadedDSDFromAdmin();
                    }
                    else if (!string.IsNullOrEmpty(Request.Files.AllKeys[0]) && ((Request.Files.AllKeys[0] == "flCatalogImageName") || (Request.Files.AllKeys[0] == "flCatalogImageName")))
                    {
                        if (Request.Files[0].ContentLength > 0)
                        {
                            this.HandleUploadedLogoImageFile();
                        }
                        else
                        {
                            handleUploadedFileComparison();
                        }
                    }
                    else if (!string.IsNullOrEmpty(Request.Files.AllKeys[0]) && ((Request.Files.AllKeys[0] == "ImportMappingExcel") || (Request.Files.AllKeys[0] == "ImportMappingExcel")))
                    {
                        handleUploadedExcel();
                    }
                    else
                    {
                        handleUploadedFileComparison();
                    }

                } // Save uploaded file & process it further
            }
    }

    #endregion

    #endregion
        
}
