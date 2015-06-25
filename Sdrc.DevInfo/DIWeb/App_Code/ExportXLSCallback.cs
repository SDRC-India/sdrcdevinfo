using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.IO;
using SpreadsheetGear;
using System.Drawing.Imaging;
using Svg;
/// <summary>
/// Summary description for ExportXLSCallback
/// </summary>
public partial class Callback : System.Web.UI.Page
{    
    /// <summary>
    /// Return workbook object containing data
    /// </summary>
    /// <param name="tTitle">Title</param>
    /// <param name="tSubtitle">Subtile</param>
    /// <param name="tSource">Source Text</param>
    /// <param name="tKeywords">Keywords</param>
    /// <param name="tTableData"OAT data</param>
    /// <param name="imgData">Image data</param>
    /// <returns>Workbook</returns>
    #region Get workbook for all visualize except swf radar
    public IWorkbook getWorkbookXLS(string tTitle, string tSubtitle, string tSource, string tKeywords, string tTableData, string tSvg)
    {
        IWorkbook workbook = null;
        try
        {
            string workingDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.XLSDownloadType.SvgRasterizerFolder);
            string baticPath = Constants.XLSDownloadType.BatikRasterizerJarFile;
            string templateFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.XLSDownloadType.ExcelTemplateFilePath);
            if (File.Exists(templateFilePath))
            {
                Process dosProcess = new Process();
                dosProcess.EnableRaisingEvents = false;
                if (!Page.IsPostBack)
                {                
                    ExportExcel exportExcel = new ExportExcel();                                                                            
                    workbook = SpreadsheetGear.Factory.GetWorkbook(templateFilePath);
                    #region Table tab
                    IWorksheet tableWorksheet = workbook.Worksheets[1];
                    tableWorksheet.Name = "Table";
                    tableWorksheet = exportExcel.getTableWorksheet(tableWorksheet, tTableData, tTitle);
                    #endregion
                    #region Source tab
                    IWorksheet sourceWorksheet = workbook.Worksheets[2];
                    sourceWorksheet.Name = "Source";
                    sourceWorksheet.Cells["A1"].Value = "Source";
                    string[] sourceArray = null;
                    if (!string.IsNullOrEmpty(tSource))
                    {
                        sourceArray = tSource.Split(new string[] { "{}" }, StringSplitOptions.None);
                        int i = 2;
                        foreach (string sourceString in sourceArray)
                        {
                            sourceWorksheet.Cells["A" + i].Value = sourceString;
                            i++;
                        }
                    }
                    #endregion
                    #region Keywords tab
                    IWorksheet keywordWorksheet = workbook.Worksheets[3];
                    keywordWorksheet.Name = "Keywords";
                    keywordWorksheet.Cells["A1"].Value = "Keywords";
                    string[] keywordArray = null;
                    if (!string.IsNullOrEmpty(tKeywords))
                    {
                        keywordArray = tKeywords.Split(new string[] { "{}" }, StringSplitOptions.None);
                        int i = 2;
                        foreach (string keywordString in keywordArray)
                        {
                            keywordWorksheet.Cells["A" + i].Value = keywordString;
                            i++;
                        }
                    }
                    #endregion
                    #region If Svg ImageFormat
                    if (!string.IsNullOrEmpty(tSvg))
                    {
                        string tempFilename = DateTime.Now.ToString();
                        tempFilename = exportExcel.removeInvalidCharacters(tempFilename);
                        string filePath = Path.Combine(workingDir, tempFilename + Constants.XLSDownloadType.SvgExtention);
                        FileStream fileStream = File.Create(filePath);
                        fileStream.Close();
                        using (StreamWriter sw = new StreamWriter(filePath))
                        {
                            sw.Write(tSvg);
                        }
                        # region Image convertion svg to png using Batick
                        /* Convert svg to png using http://xmlgraphics.apache.org/batik/tools/rasterizer.html */

                        string filePth = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @filePath);

                        var sampleDoc = SvgDocument.Open(filePth, new Dictionary<string, string> 
                        {
                            {"entity1", "fill:red" },
                            {"entity2", "fill:yellow" }
                        });
                        string destination = @workingDir + "\\" + tempFilename + ".png";
                        sampleDoc.Draw().Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destination));

                        int imageWidth = 470;
                        //string command = "/C java -jar " + baticPath + " " + Constants.XLSDownloadType.ImageType + " " + tempFilename + Constants.XLSDownloadType.SvgExtention;
                        //ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
                        //startInfo.UseShellExecute = false;
                        //startInfo.WorkingDirectory = workingDir;
                        //startInfo.Arguments = command;
                        //startInfo.CreateNoWindow = true;
                        //dosProcess.StartInfo = startInfo;
                        //dosProcess.Start();
                        //dosProcess.WaitForExit();
                        //dosProcess.Close();
                        #endregion
                        #region Graph tab
                        string chartImageName = Path.Combine(workingDir, tempFilename);
                        if (File.Exists(chartImageName + Constants.XLSDownloadType.PngExtention))
                        {
                            MemoryStream picStream = new MemoryStream();
                            System.Drawing.Image chartImageObj = System.Drawing.Image.FromFile(chartImageName + Constants.XLSDownloadType.PngExtention);
                            chartImageObj.Save(picStream, ImageFormat.Png);
                            chartImageObj.Dispose();
                            // Graph tab
                            IWorksheet visualizerWorksheet = workbook.Worksheets[0];
                            visualizerWorksheet.Name = "Graph";
                            visualizerWorksheet.Cells["A1"].Value = tTitle;
                            visualizerWorksheet.Cells["A2"].Value = tSubtitle;
                            visualizerWorksheet.Cells["A3"].Value = DateTime.Now.ToString();
                            string baseURL = this.Page.Request.Url.OriginalString.ToString();
                            baseURL = baseURL.Substring(0, baseURL.IndexOf("libraries") - 1);
                            visualizerWorksheet.Cells["A4"].Value = baseURL;
                            visualizerWorksheet.Cells["A1"].Columns.AutoFit();
                            System.Drawing.Image chartImage = System.Drawing.Image.FromStream(picStream);
                            int imageHeight = (int)((imageWidth * chartImage.Height) / chartImage.Width);
                            visualizerWorksheet.Shapes.AddPicture(picStream.ToArray(), 0, 100, (double)imageWidth, (double)imageHeight);
                            picStream.Close();
                            picStream.Dispose();
                            visualizerWorksheet.MoveBefore(workbook.Sheets[0]);
                            //Delete image files.                                    
                            if (File.Exists(chartImageName + Constants.XLSDownloadType.PngExtention))
                            {
                                File.Delete(chartImageName + Constants.XLSDownloadType.PngExtention);
                            }
                            if (File.Exists(chartImageName + Constants.XLSDownloadType.SvgExtention))
                            {
                                File.Delete(chartImageName + Constants.XLSDownloadType.SvgExtention);
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        workbook.Sheets[0].Delete();
                    }
                    # endregion
                    workbook.Sheets[0].Select();                                
                }                                
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return workbook;
    }
    #endregion
    /// <summary>
    /// Return workbook object containing data
    /// </summary>
    /// <param name="tTitle">Title</param>
    /// <param name="tSubtitle">Subtile</param>
    /// <param name="tSource">Source Text</param>
    /// <param name="tKeywords">Keywords</param>
    /// <param name="tTableData"OAT data</param>
    /// <param name="imgData">Image data</param>
    /// <returns>Workbook</returns>
    #region Get workbook for swf radar only
    public IWorkbook getWorkbookForSwf(string tTitle, string tSubtitle, string tSource, string tKeywords, string tTableData, string imgData)
    {
        IWorkbook workbook = null;
        MemoryStream picStream = new MemoryStream(Convert.FromBase64String(imgData));        
        int imageWidth = 470;
        string templateFilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath,Constants.XLSDownloadType.ExcelTemplateFilePath);
        ExportExcel exportExcel = new ExportExcel();
        if (File.Exists(templateFilePath))
        {
            workbook = SpreadsheetGear.Factory.GetWorkbook(templateFilePath);
            #region Table tab
            IWorksheet tableWorksheet = workbook.Worksheets[1];
            tableWorksheet.Name = "Table";
            tableWorksheet = exportExcel.getTableWorksheet(tableWorksheet, tTableData, tTitle);
            #endregion
            #region Source tab
            IWorksheet sourceWorksheet = workbook.Worksheets[2];
            sourceWorksheet.Name = "Source";
            sourceWorksheet.Cells["A1"].Value = "Source";
            string[] sourceArray = null;
            if (!string.IsNullOrEmpty(tSource))
            {
                sourceArray = tSource.Split(new string[] { "{}" }, StringSplitOptions.None);
                int i = 2;
                foreach (string sourceString in sourceArray)
                {
                    sourceWorksheet.Cells["A" + i].Value = sourceString;
                    i++;
                }
            }
            #endregion
            #region Keywords tab
            IWorksheet keywordWorksheet = workbook.Worksheets[3];
            keywordWorksheet.Name = "Keywords";
            keywordWorksheet.Cells["A1"].Value = "Keywords";
            string[] keywordArray = null;
            if (!string.IsNullOrEmpty(tKeywords))
            {
                keywordArray = tKeywords.Split(new string[] { "{}" }, StringSplitOptions.None);
                int i = 2;
                foreach (string keywordString in keywordArray)
                {
                    keywordWorksheet.Cells["A" + i].Value = keywordString;
                    i++;
                }
            }
            #endregion
            #region Graph tab
            IWorksheet visualizerWorksheet = workbook.Worksheets[0];
            visualizerWorksheet.Name = "Graph";
            visualizerWorksheet.Cells["A1"].Value = tTitle;
            visualizerWorksheet.Cells["A2"].Value = tSubtitle;
            visualizerWorksheet.Cells["A3"].Value = DateTime.Now.ToString();
            string baseURL = this.Page.Request.Url.OriginalString.ToString();
            baseURL = baseURL.Substring(0, baseURL.IndexOf("libraries") - 1);
            visualizerWorksheet.Cells["A4"].Value = baseURL;
            visualizerWorksheet.Cells["A1"].Columns.AutoFit();
            // Insert Image
            System.Drawing.Image chartImage = System.Drawing.Image.FromStream(picStream);
            int imageHeight = (int)((imageWidth * chartImage.Height) / chartImage.Width);
            visualizerWorksheet.Shapes.AddPicture(picStream.ToArray(), 0, 100, (double)imageWidth, (double)imageHeight);
            visualizerWorksheet.MoveBefore(workbook.Sheets[0]);
            #endregion
            workbook.Sheets[0].Select();            
            picStream.Close();
            picStream.Dispose();
        }
        return workbook;
    }
    #endregion
}