using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Drawing;
using System.Diagnostics;

public partial class Callback : System.Web.UI.Page
{
    #region Public functions
    /// <summary>
    /// This funtion is used to save chartinput into xml file & retrun a url
    /// </summary>
    /// <param name="input">ChartInput as string</param>
    /// <returns>Url</returns>
    public string sharedPresentation(string input, string storagePath, string html, string visType)
    {
        string retVal = string.Empty;
        /*if (visType == "radar")
        {
            retVal = saveImage(storagePath, input);
        }
        else
        {
            retVal = writeXml(input, storagePath, data);
        }*/
        retVal = writeXml(input, storagePath, html, visType);
        CreateThumbnail(retVal, html, visType);
        return retVal;
    }
    /// <summary>
    /// Return chart data to client
    /// </summary>
    /// <param name="key">Key</param>
    /// <returns>chart data</returns>
    public string getChartData(string key, string storagePath)
    {
        string retVal = "";
        retVal = readVisualizerXml(key, storagePath);
        return retVal;
    }
    #endregion


    #region Private functions
    private string saveImage(string storagePath,string rawImage)
    {                
        string fileName = string.Empty;        
        if (!String.IsNullOrEmpty(rawImage))
        {                        
            Guid oGuid;
            oGuid = Guid.NewGuid();
            string uniqueId = oGuid.ToString();            
            fileName = uniqueId + Constants.XLSDownloadType.PngExtention;
            string physicalPath = Server.MapPath(storagePath + fileName);
            MemoryStream picStream = new MemoryStream(Convert.FromBase64String(rawImage));            
            Bitmap bmp = new Bitmap(picStream);
            bmp.Save(physicalPath, System.Drawing.Imaging.ImageFormat.Png);
            bmp.Dispose();
            picStream.Close();           
        }
        return fileName;
    }
    /// <summary>
    /// Write chart input into xml file
    /// </summary>
    /// <returns>Key</returns>
    private string writeXml(string input, string storagePath, string html, string visType)
    {
        string retVal=string.Empty;
        string fileUrl = storagePath;
        Guid oGuid;
        oGuid = Guid.NewGuid();
        string uniqueId = oGuid.ToString();
        string fileName = uniqueId + ".xml";
        string physicalPath = string.Empty;
        if (visType.ToLower().Equals("treemap"))
        {
            string HtmlfileName = uniqueId + ".html";
            physicalPath = Server.MapPath(fileUrl + HtmlfileName);
            if (!File.Exists(physicalPath))
            {
                createTreemapHtmlFile(physicalPath, html);                        
            }
        }
        physicalPath = Server.MapPath(fileUrl + fileName);
        if (!File.Exists(physicalPath))
        {
            if (createVisualizerXmlFile(physicalPath, input))
            {
                retVal = uniqueId;
            }            
        }
        return retVal;
    }

    private void CreateThumbnail(string uniqueId, string svgInput, string visType)
    {
        string workingDir=string.Empty, filePath=string.Empty, command=string.Empty, svg_folder = "libraries\\svg_rasterizer\\", storagePath = "stock\\shared\\vc\\";
        FileStream fileStream = null;
        Process dosProcess = null;
        if (visType.ToLower().Equals("treemap"))
        {
            File.Copy(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\fb_treemap.png"), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, storagePath + "treemap\\" + uniqueId + ".png"));
            return;
        }
        else if (visType.ToLower().Equals("radar"))
        {
            File.Copy(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\fb_radar.jpg"), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, storagePath + uniqueId + ".jpg"));
            return;
        }
        dosProcess = new Process();        
        workingDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, svg_folder);
        filePath = Path.Combine(workingDir, uniqueId + Constants.XLSDownloadType.SvgExtention);
        fileStream = File.Create(filePath);
        fileStream.Close();
        using (StreamWriter sw = new StreamWriter(filePath))
        {
            sw.Write(svgInput);
        }
        command = "/C java -jar " + Constants.XLSDownloadType.BatikRasterizerJarFile + " " + Constants.XLSDownloadType.ImageType + " " + uniqueId + Constants.XLSDownloadType.SvgExtention;
        ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
        startInfo.UseShellExecute = false;
        startInfo.WorkingDirectory = workingDir;
        startInfo.Arguments = command;
        startInfo.CreateNoWindow = true;
        dosProcess.StartInfo = startInfo;
        dosProcess.Start();
        dosProcess.WaitForExit();
        dosProcess.Close();
        File.Move(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, svg_folder) + uniqueId + ".png", Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, storagePath + uniqueId + "_big.png"));
        SaveThumbnail(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, storagePath + uniqueId + "_big.png"), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, storagePath + uniqueId + ".png"), 200, 200);        
        if(File.Exists(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, storagePath +  uniqueId + "_big.png")))
        {
            File.Delete(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, storagePath + uniqueId + "_big.png"));
        }
    }

    /// <summary>
    /// Create Visualizer.xml file
    /// </summary>
    /// <param name="filePath">physical path of file</param>
    /// <param name="key">unique key</param>
    private bool createVisualizerXmlFile(string filePath, string input)
    {
        bool result = true; 
        try
        {            
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration;
            XmlElement presentationsElement, presentationElement, chartdataElement, pagetitleElement;
            // 1. write declaration tag into xml file
            xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDoc.AppendChild(xmlDeclaration);
            // 2. write root element
            presentationsElement = xmlDoc.CreateElement("presentations");
            xmlDoc.AppendChild(presentationsElement);
            // 4. write child element of root element
            presentationElement = xmlDoc.CreateElement("presentation");            
            // 5. write child node of child element 
            chartdataElement = xmlDoc.CreateElement("chartdata");
            // 6. set text into node
            chartdataElement.InnerText = input;
            // Add attribute            
            chartdataElement.SetAttribute("title", Global.adaptation_name);
            presentationElement.AppendChild(chartdataElement);         
            // 7. write child node of chile element.
            //pagetitleElement = xmlDoc.CreateElement("pagetitle");
            //pagetitleElement.InnerText = Global.adaptation_name + " : share";
            //presentationElement.AppendChild(pagetitleElement);
            presentationsElement.AppendChild(presentationElement);
            // 7. save file
            xmlDoc.Save(filePath);            
        }
        catch (Exception ex)
        {
            result = false;
        }
        return result;
    }
    private bool createTreemapHtmlFile(string filePath, string input)
    {
        bool result = true;
        try
        {
            //open file with saved number            
            StreamWriter sw = File.CreateText(filePath);
            sw.Write(input);
            sw.Close();
            sw.Dispose();            
        }
        catch (Exception ex)
        {
            result = false;
        }
        return result;
    }
    private bool modifyVisualizerXml(string filePath, string key, string input)
    {
        bool result = true;
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            XmlElement presentationElement, chartdataElement;
            XmlNode presentationsElement;
            // 1. get root element            
            presentationsElement = xmlDoc.GetElementsByTagName("presentations")[0];
            // 2. write child element of root element
            presentationElement = xmlDoc.CreateElement("presentation");
            // set attribute
            presentationElement.SetAttribute("id", key);
            // 3. write child node of child element 
            chartdataElement = xmlDoc.CreateElement("chartdata");
            // 4. set text into node
            chartdataElement.InnerText = input;
            presentationElement.AppendChild(chartdataElement);
            presentationsElement.AppendChild(presentationElement);
            // 5. save file
            xmlDoc.Save(filePath);
        }
        catch (Exception ex)
        {
            result = false;
        }
        return result;
    }   
    /// <summary>
    /// Read visualizer xml file to get chart data
    /// </summary>
    /// <param name="key">key</param>
    /// <returns>chart data</returns>
    private string readVisualizerXml(string key, string storagePath)
    {
        string result = string.Empty;
        try
        {
           XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList presentationElements;            
            string fileUrl = storagePath;
            string fileName = key + ".xml";
            string physicalPath = Server.MapPath(fileUrl + fileName);
            if (File.Exists(physicalPath))
            {
                xmlDoc.Load(physicalPath);
                presentationElements = xmlDoc.GetElementsByTagName("chartdata");
                foreach (XmlNode node in presentationElements)
                {
                    result = node.InnerText + "[****]";
                    if (node.Attributes.Count > 0)
                    {
                        result += node.Attributes[0].Value;
                        if (node.Attributes[1] != null)
                        {
                            if (node.Attributes[1].Value.Trim() != string.Empty)
                            {
                                result += "[****]" + node.Attributes[1].Value;    
                            }                            
                        }
                    }
                    else
                    {
                        result += Global.adaptation_name;
                    }
                }
            }
        }
        catch (Exception ex)
        {
        }
        return result;
    }
    #endregion
}
