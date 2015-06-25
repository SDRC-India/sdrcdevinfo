using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Drawing;
using System.Diagnostics;
using SpreadsheetGear;
using System.Drawing.Imaging;
using System.Net;
using Svg;
//using Microsoft.Office.Interop.Excel; 


public partial class Callback : System.Web.UI.Page
{
    #region "--Methods--"

    #region "--Private--"

    #region "--Common--"

    private string Gallery_GetSearchResultsInnerHTML(DataTable DtQDSResults, string SearchLanguage, int DBNId, bool HandleAsDIUAOrDIUFlag, bool IsASFlag, string GalleryItemType)
    {
        string RetVal;

        if (HandleAsDIUAOrDIUFlag)
        {
            RetVal = this.Gallery_GetDIUAInnerHTML(DtQDSResults, SearchLanguage, DBNId, IsASFlag, GalleryItemType);
        }
        else
        {
            RetVal = this.Gallery_GetDIUInnerHTML(DtQDSResults, SearchLanguage, DBNId, IsASFlag, GalleryItemType);
        }

        return RetVal;
    }

    private string Gallery_GetDIUAInnerHTML(DataTable DtQDSResults, string SearchLanguage, int DBNId, bool IsASFlag, string GalleryItemType)
    {
        string RetVal;
        string Indicator, ICName, Unit, Area, DefaultSG, MRDTP, MRD, SGNIds, TPNIds, SourceNIds, DVNIds, DVSeries;
        int IndicatorNId, UnitNId, AreaNId, SGCount, TPCount, SourceCount, DVCount;
        DataTable DtPresentations;

        RetVal = string.Empty;

        foreach (DataRow DrQDSResults in DtQDSResults.Rows)
        {
            IndicatorNId = Convert.ToInt32(DrQDSResults["IndicatorNId"].ToString());
            UnitNId = Convert.ToInt32(DrQDSResults["UnitNId"].ToString());
            AreaNId = Convert.ToInt32(DrQDSResults["AreaNId"].ToString());
            Indicator = DrQDSResults["Indicator"].ToString();
            ICName = DrQDSResults["ICName"].ToString();
            Unit = DrQDSResults["Unit"].ToString();
            Area = DrQDSResults["Area"].ToString();
            DefaultSG = DrQDSResults["DefaultSG"].ToString();
            MRDTP = DrQDSResults["MRDTP"].ToString();
            MRD = DrQDSResults["MRD"].ToString();
            SGCount = Convert.ToInt32(DrQDSResults["SGCount"].ToString());
            SourceCount = Convert.ToInt32(DrQDSResults["SourceCount"].ToString());
            TPCount = Convert.ToInt32(DrQDSResults["TPCount"].ToString());
            DVCount = Convert.ToInt32(DrQDSResults["DVCount"].ToString());
            SGNIds = DrQDSResults["SGNIds"].ToString();
            SourceNIds = DrQDSResults["SourceNIds"].ToString();
            TPNIds = DrQDSResults["TPNIds"].ToString();
            DVNIds = DrQDSResults["DVNIds"].ToString();
            DVSeries = DrQDSResults["DVSeries"].ToString();

            DtPresentations = this.GetGalleryThumbnailsTable(Indicator, Area, SearchLanguage, DBNId, GalleryItemType);

            if (DtPresentations != null && DtPresentations.Rows.Count > 0)
            {
                RetVal += this.Gallery_GetDIUASingleDivHTML(IsASFlag, SearchLanguage, DBNId, IndicatorNId, UnitNId, AreaNId, Indicator, ICName, Unit, Area, DefaultSG, MRDTP, MRD, SGCount, SourceCount, TPCount, DVCount, SGNIds, SourceNIds, TPNIds, DVNIds, DVSeries, DtPresentations);
            }
        }

        return RetVal;
    }

    private string Gallery_GetDIUInnerHTML(DataTable DtQDSResults, string SearchLanguage, int DBNId, bool IsASFlag, string GalleryItemType)
    {
        string RetVal;
        string Indicator, Unit, ICName, AreaNIds, SGNIds, SourceNIds, TPNIds, DVNIds;
        int IndicatorNId, UnitNId, AreaCount, SGCount, SourceCount, TPCount, DVCount;
        DataTable DtPresentations;

        RetVal = string.Empty;

        foreach (DataRow DrQDSResults in DtQDSResults.Rows)
        {
            IndicatorNId = Convert.ToInt32(DrQDSResults["IndicatorNId"].ToString());
            UnitNId = Convert.ToInt32(DrQDSResults["UnitNId"].ToString());
            Indicator = DrQDSResults["Indicator"].ToString();
            ICName = DrQDSResults["ICName"].ToString();
            Unit = DrQDSResults["Unit"].ToString();
            AreaCount = Convert.ToInt32(DrQDSResults["AreaCount"].ToString());
            SGCount = Convert.ToInt32(DrQDSResults["SGCount"].ToString());
            SourceCount = Convert.ToInt32(DrQDSResults["SourceCount"].ToString());
            TPCount = Convert.ToInt32(DrQDSResults["TPCount"].ToString());
            DVCount = Convert.ToInt32(DrQDSResults["DVCount"].ToString());
            AreaNIds = DrQDSResults["AreaNIds"].ToString();
            SGNIds = DrQDSResults["SGNIds"].ToString();
            SourceNIds = DrQDSResults["SourceNIds"].ToString();
            TPNIds = DrQDSResults["TPNIds"].ToString();
            DVNIds = DrQDSResults["DVNIds"].ToString();

            DtPresentations = this.GetGalleryThumbnailsTable(Indicator, string.Empty, SearchLanguage, DBNId, GalleryItemType);

            if (DtPresentations != null && DtPresentations.Rows.Count > 0)
            {
                RetVal += this.Gallery_GetDIUSingleDivHTML(IsASFlag, SearchLanguage, DBNId, IndicatorNId, UnitNId, Indicator, ICName, Unit, AreaCount, SGCount, SourceCount, TPCount, DVCount, AreaNIds, SGNIds, SourceNIds, TPNIds, DVNIds, DtPresentations);
            }
        }

        return RetVal;
    }

    private string Gallery_GetDIUASingleDivHTML(bool IsASFlag, string SearchLanguage, int DBNId, int IndicatorNId, int UnitNId, int AreaNId, string Indicator, string ICName, string Unit, string Area, string DefaultSG, string MRDTP, string MRD, int SGCount, int SourceCount, int TPCount, int DVCount, string SGNIds, string SourceNIds, string TPNIds, string DVNIds, string DVSeries, DataTable DtPresentations)
    {
        string RetVal;
        string IdUniquePart;

        IdUniquePart = DBNId.ToString() + "_" + IndicatorNId.ToString() + "_" + UnitNId.ToString() + "_" + AreaNId.ToString();

        RetVal = string.Empty;
        RetVal += "<div id=\"DIUA_" + IdUniquePart + "\">";
        RetVal += "<span style=\"line-height:20px;text-decoration:underline;\">";
        RetVal += "<span class=\"heading2\" id=\"Area_" + IdUniquePart + "\">" + Area + "</span>";
        RetVal += " - <span class=\"heading2\" id=\"Indicator_" + IdUniquePart + "\">" + Indicator + "</span>";
        RetVal += "</span>";
        RetVal += ", <span class=\"content\" id=\"Unit_" + IdUniquePart + "\">" + Unit + "</span>";
        RetVal += " | <a href=\"javascript:ShareResult('" + IdUniquePart + "');\"><u>share</u></a>";

        RetVal += this.Gallery_GetIndicatorDescriptionDiv(IndicatorNId, UnitNId, AreaNId, SearchLanguage, DBNId);
        RetVal += this.Gallery_GetICNameDiv(ICName);
        
        RetVal += "<div id=\"Gallery_" + IdUniquePart + "\" separatorLength=\"100\"></div>";
        RetVal += this.Gallery_GetGalleryThumbnailsInnerHtml(DtPresentations, 100);
        RetVal += "</br>";
        RetVal += "</div>";

        return RetVal;
    }

    private string Gallery_GetDIUSingleDivHTML(bool IsASFlag, string SearchLanguage, int DBNId, int IndicatorNId, int UnitNId, string Indicator, string ICName, string Unit, int AreaCount, int SGCount, int SourceCount, int TPCount, int DVCount, string AreaNIds, string SGNIds, string SourceNIds, string TPNIds, string DVNIds, DataTable DtPresentations)
    {
        string RetVal;
        string IdUniquePart;

        IdUniquePart = DBNId.ToString() + "_" + IndicatorNId.ToString() + "_" + UnitNId.ToString() + "_0";

        RetVal = string.Empty;
        RetVal += "<div id=\"DIUA_" + IdUniquePart + "\">";
        RetVal += "<span style=\"line-height:20px;text-decoration:underline;\">";
        RetVal += "<span class=\"heading2\" id=\"Indicator_" + IdUniquePart + "\">" + Indicator + "</span>";
        RetVal += "</span>";
        RetVal += ", <span class=\"content\" id=\"Unit_" + IdUniquePart + "\">" + Unit + " </span>";
        RetVal += " | <a href=\"javascript:ShareResult('" + IdUniquePart + "');\"><u>share</u></a>";

        RetVal += this.Gallery_GetIndicatorDescriptionDiv(IndicatorNId, UnitNId, 0, SearchLanguage, DBNId);
        RetVal += this.Gallery_GetICNameDiv(ICName);

        RetVal += "<div id=\"Gallery_" + IdUniquePart + "\" separatorLength=\"100\"></div>";
        RetVal += this.Gallery_GetGalleryThumbnailsInnerHtml(DtPresentations, 100);
        RetVal += "</br>";
        RetVal += "</div>";

        return RetVal;
    }

    private string Gallery_GetIndicatorDescriptionDiv(int IndicatorNId, int UnitNId, int AreaNId, string SearchLanguage, int DBNId)
    {
        string RetVal;
        string Description;

        Description = this.Gallery_GetIndicatorDescription(IndicatorNId, SearchLanguage, DBNId);

        RetVal = "<div><i id=\"IDesc_" + IndicatorNId.ToString() + "_" + UnitNId.ToString() + "_" + AreaNId.ToString() + "\"";

        if (Description.Length > 100)
        {
            RetVal += "onmouseover=\"ShowCallout('divCallout', '" + Description + "', event);\"";
            RetVal += " onmouseout=\"HideCallout('divCallout');\"";

            Description = Description.Substring(0, 100) + "...";
        }

        RetVal += "style=\"line-height:20px; color:#999999\" class=\"content\">";
        RetVal += Description;
        RetVal += "</i></div>";

        return RetVal;
    }

    private string Gallery_GetIndicatorDescription(int IndicatorNId, string SearchLanguage, int DBNId)
    {
        string RetVal;
        XmlDocument MetadataDocument;

        RetVal = string.Empty;
        MetadataDocument = null;

        try
        {
            MetadataDocument = new XmlDocument();
            MetadataDocument.Load(Server.MapPath(@"~\stock\data\" + DBNId.ToString() + @"\ds\" + SearchLanguage + @"\metadata\indicator\" + IndicatorNId.ToString() + ".xml"));
            foreach (XmlNode Category in MetadataDocument.GetElementsByTagName("Category"))
            {
                if (Category.Attributes["name"].Value == "Definition")
                {
                    RetVal = Category.ChildNodes[0].InnerText;
                    RetVal = RetVal.Replace("{{~}}", "");
                    break;
                }
            }
            RetVal = RetVal.Trim();
        }
        catch (Exception ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    private string Gallery_GetICNameDiv(string ICName)
    {
        string RetVal;
        RetVal = string.Empty;
        if (!string.IsNullOrEmpty(ICName))
        {
            RetVal = "<div>";
            RetVal += "<span ";
            ICName = "Searched by: " + ICName.Trim();
            if (ICName.Length > 100)
            {
                RetVal += "onmouseover=\"ShowCallout('divCallout', '" + ICName + "', event);\"";
                RetVal += " onmouseout=\"HideCallout('divCallout');\"";

                ICName = ICName.Substring(0, 100) + "...";
            }
            RetVal += "style=\"line-height:20px; color:#999999\" class=\"content\">";
            RetVal += ICName;
            RetVal += "</span>";
            RetVal += "</div>";
        }
        return RetVal;
    }

    #endregion "--Common--"

    #region "--GetGalleryThumbnails--"

    private string Gallery_GetGalleryThumbnailsInnerHtml(DataTable DtPresentations, int GallerySeparatorLength)
    {
        string RetVal;
        string PresentationNId, PresentationName, PresentationDesc, PrsentationText;

        RetVal = string.Empty;

        RetVal = "<span style=\"color:#A0A0A0\">" + string.Empty.PadLeft(GallerySeparatorLength - 1, '-') + "</span>";
        RetVal += "<table><tr>";

        foreach (DataRow DrPresentations in DtPresentations.Rows)
        {
            PresentationNId = DrPresentations["pres_nid"].ToString();
            PresentationName = DrPresentations["pres_name"].ToString();
            PresentationDesc = DrPresentations["desc"].ToString();

            PrsentationText = "<div>Title: " + PresentationName + "<br/><br/>Description: " + PresentationDesc + "</div>";

            RetVal += "<td onmouseover=\"ShowCallout('divCallout', '" + PrsentationText + "', event);\" onmouseout=\"HideCallout('divCallout');\">";
            RetVal += "<img src='..\\..\\stock\\Gallery\\" + PresentationNId + "\\" + PresentationNId + "_t.jpg' width='150px' height='150px'/>";
            RetVal += "</td>";
            RetVal += "<td>&nbsp;&nbsp;</td>";
        }

        RetVal += "</tr></table>";

        return RetVal;
    }

    #endregion "--GetGalleryThumbnails--"

    #region "--SaveGalleryItem--"

    private string InsertIntoKeywordsTable(string Indicators, string Areas, string UDK)
    {
        string RetVal;
        string Query;
        DataTable DtKeyword;
        DIConnection DIConnection;

        RetVal = string.Empty;
        Query = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            
            foreach (string Indicator in Indicators.Split(new string[] { "{@@}" }, StringSplitOptions.None))
            {
                if (!String.IsNullOrEmpty(Indicator))
                {
                    string IndiStr = DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(Indicator);
                    Query = "SELECT 1 FROM keywords WHERE keyword = '" + IndiStr + "' AND keyword_type = 'I'";
                    DtKeyword = DIConnection.ExecuteDataTable(Query);
                    if (DtKeyword.Rows.Count < 1)
                    {
                        Query = "  INSERT INTO keywords (keyword, keyword_type)";
                        Query += "  VALUES('" + IndiStr + "','I')";
                        DIConnection.ExecuteDataTable(Query);
                    }
                    Query = " SELECT keyword_nid FROM keywords WHERE keyword = '" + IndiStr + "' AND keyword_type = 'I'";
                    DtKeyword = DIConnection.ExecuteDataTable(Query);
                    if (DtKeyword != null && DtKeyword.Rows.Count > 0)
                    {
                        string nid = DtKeyword.Rows[0]["keyword_nid"].ToString();
                        if(RetVal.IndexOf(nid)==-1)
                            RetVal += "," + nid;
                    }
                }
            }

            foreach (string Area in Areas.Split(new string[] { "{@@}" }, StringSplitOptions.None))
            {
                if (!String.IsNullOrEmpty(Area))
                {
                    string AreaString = Area;
                    AreaString = DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(AreaString);
                    Query = "SELECT 1 FROM keywords WHERE keyword = '" + AreaString + "' AND keyword_type = 'A'";
                    DtKeyword = DIConnection.ExecuteDataTable(Query);
                    if (DtKeyword.Rows.Count < 1)
                    {
                        Query = "  INSERT INTO keywords (keyword, keyword_type)";
                        Query += "  VALUES('" + AreaString + "','A')";
                        DIConnection.ExecuteDataTable(Query);
                    }
                    Query = " SELECT keyword_nid FROM keywords WHERE keyword = '" + AreaString + "' AND keyword_type = 'A'";
                    DtKeyword = DIConnection.ExecuteDataTable(Query);                    

                    if (DtKeyword != null && DtKeyword.Rows.Count > 0)
                    {
                        string nid = DtKeyword.Rows[0]["keyword_nid"].ToString();
                        if (RetVal.IndexOf(nid) == -1)
                            RetVal += "," + nid;
                    }
                }
            }
            foreach (string UDKStr in UDK.Split(new string[] { "{@@}" }, StringSplitOptions.None))
            {
                if (!String.IsNullOrEmpty(UDKStr))
                {
                    Query = "SELECT 1 FROM keywords WHERE keyword = '" + UDKStr + "' AND keyword_type = 'UDK'";                    
                    DtKeyword = DIConnection.ExecuteDataTable(Query);
                    if (DtKeyword.Rows.Count < 1)
                    {
                        Query = "  INSERT INTO keywords (keyword, keyword_type)";
                        Query += "  VALUES('" + UDKStr + "','UDK')";
                        DIConnection.ExecuteDataTable(Query);
                    }
                    Query = " SELECT keyword_nid FROM keywords WHERE keyword = '" + UDKStr + "' AND keyword_type = 'UDK'";
                    DtKeyword = DIConnection.ExecuteDataTable(Query);                    
                    if (DtKeyword != null && DtKeyword.Rows.Count > 0)
                    {
                        string nid = DtKeyword.Rows[0]["keyword_nid"].ToString();
                        if (RetVal.IndexOf(nid) == -1)
                            RetVal += "," + nid;
                    }
                }
            }
            RetVal += ",";
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private string InsertIntoPresentationsTable(string CreatedTime, string ModifiedTime, string CategoryNId, string Name, string Type, string ChartType, string Desc, string UDK, string DBNId, string LngCode, string UserNId, string KeyWordNIds)
    {
        string RetVal;
        string Query;
        DataTable DtPresentation;
        DIConnection DIConnection;
        System.Data.Common.DbParameter DbParam;
        List<System.Data.Common.DbParameter> DbParams;

        RetVal = string.Empty;
        Query = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            DbParams = new List<System.Data.Common.DbParameter>();
            if (string.IsNullOrEmpty(UserNId))
                UserNId = "-1";
            Query = "INSERT INTO presentations (created_time, modified_time, pres_name, type, chart_type, description, udk, dbnid, lng_code, user_nid, " +
                    "keyword_nids,cat_nid) " +
                    "VALUES(#" + CreatedTime + "#,#" + ModifiedTime + "#,'" + DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(Name) + "','" + Type + "','" + ChartType + "','" + Desc.Replace("'", "''") + "','" + DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(UDK) + "','" + DBNId + "','" + LngCode + "'," + UserNId + ",'" + KeyWordNIds + "'," + CategoryNId + ")";            

            DtPresentation = DIConnection.ExecuteDataTable(Query);
            Query = "SELECT MAX(pres_nid) AS pres_nid FROM presentations;";
            DtPresentation = DIConnection.ExecuteDataTable(Query);
            if (DtPresentation != null && DtPresentation.Rows.Count > 0)
            {
                RetVal = DtPresentation.Rows[0]["pres_nid"].ToString();
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private void Save_Settings_File(string FileNameWPath, string Content)
    {
        XmlDocument XmlDocument;
        XmlDeclaration XmlDeclaration;
        XmlElement PresentationsElement, PresentationElement, ChartdataElement;

        XmlDocument = new XmlDocument();
        try
        {
            XmlDeclaration = XmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlDocument.AppendChild(XmlDeclaration);

            PresentationsElement = XmlDocument.CreateElement("presentations");
            XmlDocument.AppendChild(PresentationsElement);

            PresentationElement = XmlDocument.CreateElement("presentation");
            PresentationElement.SetAttribute("id", Guid.NewGuid().ToString());

            ChartdataElement = XmlDocument.CreateElement("chartdata");
            ChartdataElement.InnerText = Content;

            PresentationElement.AppendChild(ChartdataElement);
            PresentationsElement.AppendChild(PresentationElement);

            XmlDocument.Save(FileNameWPath);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
    }

    private void Create_File(string SourcePath, string DestinationPath)
    {
        File.Copy(SourcePath, DestinationPath);
    }
    /// <summary>
    /// To create new image with new dimensions.
    /// </summary>
    /// <param name="smallImgPath">Source Img</param>
    /// <param name="bigImgPath">Destination Img</param>
    /// <param name="width">New Width</param>
    /// <param name="height">New Height</param>
    private void SaveThumbnail(string smallImgPath, string bigImgPath, int width, int height)
    {
        Encoder myEncoder;
        EncoderParameter myEncoderParameter;
        EncoderParameters myEncoderParameters=new EncoderParameters();
        ImageCodecInfo myImageCodecInfo;
        // Get an ImageCodecInfo object that represents the JPEG codec.
        myImageCodecInfo = GetEncoderInfo("image/png");
        System.Drawing.Image.GetThumbnailImageAbort myCallback = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);
        Bitmap myBitmap = new Bitmap(smallImgPath);
        // for the Quality parameter category.
        myEncoder = Encoder.Quality;
        // Save the bitmap as a JPEG file with quality level 50.
        myEncoderParameter = new EncoderParameter(myEncoder, 100L);
        myEncoderParameters.Param[0] = myEncoderParameter;
        int imageHeight = (int)((width * myBitmap.Height) / myBitmap.Width);
        int imageWidth = (int)((height * myBitmap.Width) / myBitmap.Height);
        System.Drawing.Image myThumbnail = myBitmap.GetThumbnailImage(imageWidth, height, myCallback, IntPtr.Zero);

        myThumbnail.Save(bigImgPath, myImageCodecInfo, myEncoderParameters);
        myBitmap.Dispose();
        myThumbnail.Dispose();
    }
    private static ImageCodecInfo GetEncoderInfo(String mimeType)
    {
        int j;
        ImageCodecInfo[] encoders;
        encoders = ImageCodecInfo.GetImageEncoders();
        for (j = 0; j < encoders.Length; ++j)
        {
            if (encoders[j].MimeType == mimeType)
                return encoders[j];
        }
        return null;
    }
    #endregion "--SaveGalleryItem--"

    /*#region Resizing Image
    private static byte[] ResizeImageFile(byte[] imageFile, int targetSize)
    {                
        Image original = Image.FromStream(new MemoryStream(imageFile));
        int targetH, targetW;
        if (original.Height > original.Width)
        {
            targetH = targetSize;
            targetW = (int)(original.Width * ((float)targetSize / (float)original.Height));
        }
        else
        {
            targetW = targetSize;
            targetH = (int)(original.Height * ((float)targetSize / (float)original.Width));
        }
        Image imgPhoto = Image.FromStream(new MemoryStream(imageFile));
        // Create a new blank canvas.  The resized image will be drawn on this canvas.
        Bitmap bmPhoto = new Bitmap(targetW, targetH, PixelFormat.Format24bppRgb);
        bmPhoto.SetResolution(72, 72);
        Graphics grPhoto = Graphics.FromImage(bmPhoto);
        grPhoto.SmoothingMode = SmoothingMode.AntiAlias;
        grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
        grPhoto.PixelOffsetMode = PixelOffsetMode.HighQuality;
        grPhoto.DrawImage(imgPhoto, new Rectangle(0, 0, targetW, targetH), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel);
        // Save out to memory and then to a file.  We dispose of all objects to make sure the files don't stay locked.
        MemoryStream mm = new MemoryStream();        
        bmPhoto.Save(mm, System.Drawing.Imaging.ImageFormat.Jpeg);
        original.Dispose();
        imgPhoto.Dispose();
        bmPhoto.Dispose();
        grPhoto.Dispose();
        return mm.GetBuffer();
    }
    #endregion */    
    #region Get DataTable containing Indicator names according given indicatorNids
    /// <summary>
    /// Get DataTable containing Indicator names
    /// </summary>
    /// <param name="indicatorsString">IndicatorNids String seperated by {@@}</param>
    /// <param name="DBNId">Database Nid</param>
    /// <param name="langCode">Language Code</param>
    /// <returns>DataTable</returns>    
    private DataTable getIndicatorNameDataTable(string indicatorsString, int DBNId, string langCode)
    {
        DataTable indiTable = null;
        if (!string.IsNullOrEmpty(indicatorsString) && DBNId != null)
        {
            string delemeter = "{@@}";
            string query = "select indicator_name from ut_indicator_" + langCode + " where indicator_nid in(";
            string parameters = string.Empty;
            string[] indicatorsList = indicatorsString.Split(new string[] { delemeter }, StringSplitOptions.None);
            foreach (string indi in indicatorsList)
            {
                if (!string.IsNullOrEmpty(indi))
                {
                    parameters += ",'" + indi + "'";
                }
            }
            if (parameters.Length > 0)
            {
                parameters = parameters.Substring(1);
            }
            query += parameters + ")";
            DIConnection DIConnection = Global.GetDbConnection(DBNId);
            indiTable = DIConnection.ExecuteDataTable(query);
        }
        return indiTable;
    }
    #endregion
    #region Get Indicator name string seperated by {@@} from given datatable
    /// <summary>
    /// Get Indicator name string seperated by {@@}
    /// </summary>
    /// <param name="dt">DataTable</param>
    /// <returns>Indicators name seperated by {@@}</returns>
    private string getIndicatorNames(DataTable dt)
    {
        string indList = string.Empty;
        if (dt != null)
        {
            string delemeter = "{@@}";
            foreach (DataRow dr in dt.Rows)
            {
                indList += delemeter + dr[0].ToString();
            }
        }
        if (indList.Length > 0)
        {
            indList = indList.Substring(4);
        }
        return indList;
    }
    #endregion
    #region Delete presentation record
    /// <summary>
    /// Delete presentation
    /// </summary>
    /// <param name="presId">Presentation Id</param>
    private void deletePresentationRecord(string presId)
    {
        string Query = string.Empty;
        DIConnection DIConnection = null;
        DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);
        Query = "Delete from presentations where pres_nid =" + presId + ";";
        DIConnection.ExecuteDataTable(Query);
    }
    #endregion
    #endregion "--Private--"



    #region "--Public--"

    public string Gallery_GetQDSResults(string requestParam)
    {
        string RetVal;
        string[] Params;
        string SearchIndicators, SearchICs, SearchAreas, SearchLanguage, GalleryItemType;
        int DBNId;
        bool HandleAsDIUAOrDIUFlag;
        DataTable DtQDSResults;

        RetVal = string.Empty;
        HandleAsDIUAOrDIUFlag = true;
        GetSearchResults_StartTime = DateTime.Now;        
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            SearchIndicators = this.SortString(Params[0].Trim(), ",");
            SearchICs = this.SortString(Params[1].Trim(), ",");
            SearchAreas = this.SortString(Params[2].Trim(), ",");
            SearchLanguage = Params[3].Trim();
            DBNId = Convert.ToInt32(Params[4].Trim());
            GalleryItemType = Params[5].Trim();

            if (string.IsNullOrEmpty(SearchAreas))
            {
                SearchAreas = Global.GetDefaultArea(DBNId.ToString());
                HandleAsDIUAOrDIUFlag = false;
            }

            DtQDSResults = this.GetQDSResultsTable(DBNId, SearchIndicators, SearchICs, SearchAreas, SearchLanguage, HandleAsDIUAOrDIUFlag);
            RetVal = this.Gallery_GetSearchResultsInnerHTML(DtQDSResults, SearchLanguage, DBNId, HandleAsDIUAOrDIUFlag, false, GalleryItemType);

            if (!string.IsNullOrEmpty(RetVal))
            {
                RetVal = "<div style=\"margin-left:3px;\">" + RetVal + "</div>";
            }

            RetVal += Constants.Delimiters.ParamDelimiter + DtQDSResults.Rows.Count.ToString() + " results in " +
                      ((TimeSpan)DateTime.Now.Subtract(GetSearchResults_StartTime)).TotalSeconds.ToString("0.00") + " seconds";
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    public string Gallery_GetASResults(string requestParam)
    {
        string RetVal;
        string[] Params;
        string SearchIndicatorICs, SearchAreas, SearchLanguage, GalleryItemType;
        int DBNId, NumResults;
        double ElapsedTime;
        bool HandleAsDIUAOrDIUFlag;
        DataTable DtASResults;

        RetVal = string.Empty;
        HandleAsDIUAOrDIUFlag = true;
        GetSearchResults_StartTime = DateTime.Now;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            SearchIndicatorICs = this.CustomiseStringForIndexingAndQuery(Params[0].Trim());
            SearchAreas = this.CustomiseStringForIndexingAndQuery(Params[1].Trim());
            SearchLanguage = Params[2].Trim();
            DBNId = Convert.ToInt32(Params[3].Trim());
            NumResults = Convert.ToInt32(Params[4].Trim());
            ElapsedTime = Convert.ToDouble(Params[5].Trim());
            GalleryItemType = Params[6].Trim();

            if (string.IsNullOrEmpty(SearchAreas))
            {
                SearchAreas = Global.GetDefaultArea(DBNId.ToString());
                HandleAsDIUAOrDIUFlag = false;
            }

            DtASResults = this.GetASResultsTableFromIndexingDatabase(DBNId, SearchIndicatorICs, SearchAreas, SearchLanguage, HandleAsDIUAOrDIUFlag);

            if (DtASResults.Rows.Count == 0)
            {
                DtASResults = this.GetASResultsTableFromDIDatabase(DBNId, SearchIndicatorICs, SearchAreas, SearchLanguage, HandleAsDIUAOrDIUFlag);
                this.InsertIntoIndexingDatabase(DBNId, SearchIndicatorICs, SearchAreas, SearchLanguage, DtASResults, HandleAsDIUAOrDIUFlag);
            }

            RetVal = this.Gallery_GetSearchResultsInnerHTML(DtASResults, SearchLanguage, DBNId, HandleAsDIUAOrDIUFlag, true, GalleryItemType);

            if (!string.IsNullOrEmpty(RetVal))
            {
                RetVal = "<fieldset><legend style=\"padding:10px;\" class=\"heading2\">Database: " +
                         Global.GetDbDetails(DBNId.ToString(), SearchLanguage)[0] + "</legend>" + RetVal + "</fieldset>";
            }

            RetVal += Constants.Delimiters.ParamDelimiter + (DtASResults.Rows.Count + NumResults).ToString() + " results in " +
                      (((TimeSpan)DateTime.Now.Subtract(GetSearchResults_StartTime)).TotalSeconds + ElapsedTime).ToString("0.00") + " seconds";
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
            
        }

        return RetVal;
    }

    public string Gallery_ShareSearchResult(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DivHtml, TemplateHtml;
        string hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao;

        RetVal = string.Empty;

        try
        {
            Params = requestParam.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None);
            DivHtml = Params[0].Trim();
            hdsby = Params[1].Trim();
            hdbnid = Params[2].Trim();
            hselarea = Params[3].Trim();
            hselind = Params[4].Trim();
            hlngcode = Params[5].Trim();
            hlngcodedb = Params[6].Trim();
            hselindo = Params[7].Trim();
            hselareao = Params[8].Trim();
            RetVal = Guid.NewGuid().ToString();

            Global.GetAppSetting();

            TemplateHtml = File.ReadAllText(Server.MapPath("../../stock/shared/Gallery/Gallery_Template.htm"));
            TemplateHtml = TemplateHtml.Replace("QDS_DIV", DivHtml);
            TemplateHtml = TemplateHtml.Replace("QDS_TITLE", Global.adaptation_name + " - Search");
            TemplateHtml = TemplateHtml.Replace("DI_DIUILIB_URL", Global.diuilib_url);
            TemplateHtml = TemplateHtml.Replace("DI_COMPONENT_VERSION", Global.diuilib_version);
            TemplateHtml = TemplateHtml.Replace("DI_THEME_CSS", Global.diuilib_theme_css);

            TemplateHtml = TemplateHtml.Replace("src=\"..\\..\\", "src=\"..\\..\\..\\");
            TemplateHtml = TemplateHtml.Replace("..%5C..%5C", "..%5C..%5C..%5C");

            TemplateHtml = TemplateHtml.Replace("<u>share</u>", "");
            TemplateHtml = TemplateHtml.Replace(" | ", "&nbsp;&nbsp;");

            TemplateHtml = TemplateHtml.Replace("onPageLoad(QDS_hdsby,QDS_hdbnid,QDS_hselarea,QDS_hselind,QDS_hlngcode,QDS_hlngcodedb,QDS_hselindo,QDS_hselareao);",
                                                "onPageLoad('" + hdsby + "','" + hdbnid + "','" + hselarea + "','" + hselind + "','" + hlngcode +
                                                "','" + hlngcodedb + "','" + hselindo + "','" + hselareao + "');");

            File.WriteAllText(Server.MapPath("../../stock/shared/Gallery/") + RetVal + ".htm", TemplateHtml);
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }
    #region User is admin or not
    /// <summary>
    /// Is user admin
    /// </summary>
    /// <param name="usernid">user nid</param>
    /// <returns>true if user is admin else false</returns>    
    public bool isUserAdmin(string usernid)
    {
        bool isAdmin = false;
        string Query = string.Empty;
        DataTable dtUsers=null;
        DIConnection DIConnection=null;
        diworldwide_userinfo.UserLoginInformation Service;

        if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
        {
            Service = new diworldwide_userinfo.UserLoginInformation();
            Service.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            isAdmin = Service.IsUserAdmin(usernid, Global.GetAdaptationGUID());
        }
        else
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);
            Query = "Select user_is_admin from Users where nid =" + usernid + ";";
            dtUsers = DIConnection.ExecuteDataTable(Query);
            if (dtUsers.Rows.Count > 0)
            {
                if (dtUsers.Rows[0][0].ToString().ToLower() == "true")
                {
                    isAdmin = true;
                }
            }
        }
        
        return isAdmin;
    }

    

    #endregion
    public string deletePresentation(string userNid,string presId)
    {
        string isDeleted = "false";
        try
        {
            string folderStructure = "stock\\Gallery\\";
            if (userNid != "-1")
            {
                #region Making folder structure
                if (isUserAdmin(userNid))
                    folderStructure += "public\\admin\\" + presId + "\\";
                else
                    folderStructure += "private\\" + userNid + "\\" + presId + "\\";
                #endregion                
                if (Directory.Exists(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure)))
                {
                    deletePresentationRecord(presId);
                    Directory.Delete(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure), true);
                    isDeleted = "true";
                }                
            }            
        }
        catch (Exception ex)
        {
            isDeleted = ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        return isDeleted;
    }
    
    public string SaveGalleryItem(string requestParam)
    {
        string RetVal;
        string[] Params;
        string DBNId, LngCode, imgInText = string.Empty, UserNId, CategoryNId, Indicators,IndicatorNids, Areas, UDK, Name, Desc, Type, ChartType, SettingData = string.Empty, folderStructure = "stock\\Gallery\\", title = string.Empty, subtitle = string.Empty, source = string.Empty, keywords = string.Empty, tableData = string.Empty;
        string KeyWordNIds, SelAreaInQds,dbLngCode=string.Empty;

        RetVal = string.Empty;

        try
        {
            Params = requestParam.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None);
            DBNId = Params[0].Trim();
            LngCode = Params[1].Trim();
            UserNId = Params[2].Trim();
            CategoryNId = Params[3].Trim();
            IndicatorNids = Params[4].Trim();
            Indicators = getIndicatorNames(getIndicatorNameDataTable(IndicatorNids, Int32.Parse(DBNId), LngCode));
            Areas = Params[5].Trim();
            UDK = Params[6].Trim();
            Name = Params[7].Trim();
            Desc = Params[8].Trim();
            Type = Params[9].Trim();
            ChartType = Params[10].Trim();
            SelAreaInQds = Params[11].Trim();
            #region Making folder structure
            if (isUserAdmin(UserNId))
                folderStructure += "public\\admin\\";
            else
                folderStructure += "private\\" + UserNId + "\\";
            #endregion
            #region In case of graph and table
            if (Type.ToLower() == "g" || Type.ToLower() == "t")
            {
                imgInText = Params[12].Trim();
                SettingData = Params[13].Trim();
                title = Params[14].Trim();
                subtitle = Params[15].Trim();
                source = Params[16].Trim();
                keywords = Params[17].Trim();
                tableData = Params[18].Trim();
            }
            #endregion
            // Reading XML to read block  {@@}
            dbLngCode = Global.GetDefaultLanguageCodeDB(DBNId.ToString(), LngCode);
            int IntResult;



            string Query = string.Empty;            
            DataTable DtPresentation;
            DIConnection DIConnection = null;             
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            Query = "SELECT * from presentations WHERE pres_name ='" + DevInfo.Lib.DI_LibBAL.Utility.DICommon.RemoveQuotes(Name) + "' AND user_nid =" + UserNId;
            DtPresentation = DIConnection.ExecuteDataTable(Query);
            if (DtPresentation.Rows.Count > 0)
            {
                RetVal = "-999";
            }
            else
            {
                if (!Int32.TryParse(SelAreaInQds, out IntResult))
                {
                    XmlDocument BlockDoc = new XmlDocument();
                    string BlockCodeListPath = HttpContext.Current.Request.PhysicalApplicationPath + "stock\\data\\" + DBNId + "\\ds\\" + LngCode;
                    if (Directory.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "stock\\data\\" + DBNId + "\\ds\\" + LngCode))
                    {
                        BlockCodeListPath += "\\area\\qscodelist.xml";
                    }
                    else
                    {
                        BlockCodeListPath = HttpContext.Current.Request.PhysicalApplicationPath + "stock\\data\\" + DBNId + "\\ds\\" + dbLngCode + "\\area\\qscodelist.xml";
                    }
                    BlockDoc.Load(BlockCodeListPath);
                    XmlNodeList aTags = BlockDoc.GetElementsByTagName("a");
                    foreach (XmlNode aTag in aTags)
                    {
                        if (aTag.Attributes["nid"].Value == SelAreaInQds)
                        {
                            Areas += "{@@}" + aTag.Attributes["n"].Value;
                            break;
                        }
                    }
                }
                else
                {
                    List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
                    DIConnection ObjDIConnection = this.GetDbConnection(int.Parse(DBNId));
                    System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
                    Param1.ParameterName = "AreaNids";
                    Param1.DbType = DbType.String;
                    Param1.Value = SelAreaInQds;
                    DbParams.Add(Param1);

                    DataTable dtAreaNames = ObjDIConnection.ExecuteDataTable("sp_get_AreaNames_" + dbLngCode, CommandType.StoredProcedure, DbParams);
                    if (dtAreaNames != null && dtAreaNames.Rows.Count > 0)
                    {
                        Areas += "{@@}" + dtAreaNames.Rows[0][0].ToString();
                    }
                }
                // Insert records into database
                KeyWordNIds = this.InsertIntoKeywordsTable(Indicators, Areas, UDK);
                //RetVal = this.InsertIntoPresentationsTable(DateTime.Now.ToShortDateString(), DateTime.Now.ToShortDateString(), CategoryNId, Name, Type,
                //                                            ChartType, Desc, UDK, DBNId, LngCode, UserNId, KeyWordNIds);
                RetVal = this.InsertIntoPresentationsTable(DateTime.Now.ToString(), DateTime.Now.ToString(), CategoryNId, Name, Type,
                                                            ChartType, Desc, UDK, DBNId, LngCode, UserNId, KeyWordNIds);
                // Created Folder if not exists
                this.Create_Directory_If_Not_Exists(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal));
                #region Save details for radar
                if (ChartType == "radar" || ChartType == "scatter3d")
                {
                    MemoryStream picStream = new MemoryStream(Convert.FromBase64String(imgInText));
                    System.Drawing.Image chartImage = System.Drawing.Image.FromStream(picStream);
                    Bitmap bmp = new Bitmap(chartImage, 269, 150);
                    bmp.Save(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + "_t.png"), System.Drawing.Imaging.ImageFormat.Png);
                    bmp.Dispose();
                    bmp = new Bitmap(chartImage, 584, 326);
                    bmp.Save(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".png"), System.Drawing.Imaging.ImageFormat.Png);
                    bmp.Dispose();
                    chartImage.Dispose();
                    this.Create_File(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\swfvisualizer.html"), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".html"));
                    this.Save_Settings_File(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".xml"), SettingData);
                    Callback callbackObj = new Callback(this.Page);
                    IWorkbook workbook = callbackObj.getWorkbookForSwf(title, subtitle, source, keywords, tableData, imgInText);
                    workbook.SaveAs(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".xls"), SpreadsheetGear.FileFormat.XLS97);
                    workbook.Close();
                }
                #endregion
                #region Save details for Map
                else if (ChartType == "map")
                {
                    SaveKMLMap(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal), RetVal);
                    SavePngMap(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal), RetVal + "_t", false, false, 150, 269);
                    SavePngMap(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal), RetVal, true, true, 326, 584);
                    SaveExcelMap(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal), RetVal);
                    //SaveMap(string FolderNameWPath, string FileName)
                }
                #endregion
                #region Save details for table
                else if (ChartType == "table")
                {
                    this.Create_File(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\table.png"), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".png"));
                    this.Create_File(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\table_t.png"), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + "_t.png"));
                    Callback callbackObj = new Callback(this.Page);
                    IWorkbook workbook = callbackObj.getWorkbookXLS(title, subtitle, source, keywords, tableData, imgInText);
                    workbook.SaveAs(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".xls"), FileFormat.XLS97);
                    workbook.Close();
                    SettingData = SettingData.Replace("[******]", "[****]");
                    WriteHTMLtoFile(SettingData, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".html"));

                }
                #endregion
                #region Save details for treemap
                else if (ChartType == "treemap" || ChartType == "cloud")
                {
                    string thumbImage = string.Empty;
                    string bigImage = string.Empty;
                    if (ChartType == "treemap")
                    {
                        thumbImage = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\treemap_t.png");
                        bigImage = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\treemap.png");
                    }
                    else
                    {
                        thumbImage = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\cloud_t.png");
                        bigImage = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\cloud.png");
                    }
                    this.Create_File(bigImage, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".png"));
                    this.Create_File(thumbImage, Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + "_t.png"));
                    Callback callbackObj = new Callback(this.Page);
                    IWorkbook workbook = callbackObj.getWorkbookXLS(title, subtitle, source, keywords, tableData, imgInText);
                    workbook.SaveAs(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".xls"), SpreadsheetGear.FileFormat.XLS97);
                    workbook.Close();
            
                    this.Save_Settings_File(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".xml"), SettingData);
                    File.WriteAllText(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".html"), imgInText);
                }
                #endregion
                #region Save details for other visualizer callforsvg_rasterizer
                else
                {
                    Process dosProcess = new Process();
                    string svg_folder = "libraries\\svg_rasterizer\\";
                    string workingDir = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, svg_folder);
                    string filePath = Path.Combine(workingDir, RetVal + Constants.XLSDownloadType.SvgExtention);
                    FileStream fileStream = File.Create(filePath);
                    fileStream.Close();
                    using (StreamWriter sw = new StreamWriter(filePath))
                    {
                        sw.Write(imgInText);
                    }
                    string filePth = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @filePath);

                    var sampleDoc = SvgDocument.Open(filePth, new Dictionary<string, string> 
                        {
                            {"entity1", "fill:red" },
                            {"entity2", "fill:yellow" }
                        });
                    string destination = @workingDir + "\\" + RetVal + ".png";
                    sampleDoc.Draw().Save(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, destination));



                    //string command = "/C java -jar " + Constants.XLSDownloadType.BatikRasterizerJarFile + " " + Constants.XLSDownloadType.ImageType + " " + RetVal + Constants.XLSDownloadType.SvgExtention;
                    //ProcessStartInfo startInfo = new ProcessStartInfo("cmd.exe");
                    //startInfo.UseShellExecute = false;
                    //startInfo.WorkingDirectory = workingDir;
                    //startInfo.Arguments = command;
                    //startInfo.CreateNoWindow = false;
                    //dosProcess.StartInfo = startInfo;
                    //dosProcess.Start();

                    //dosProcess.WaitForExit();
                    dosProcess.Close();
                    File.Move(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, svg_folder) + RetVal + ".png", Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".png"));
                    File.Move(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, svg_folder) + RetVal + ".svg", Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".svg"));
                    SaveThumbnail(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".png"), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + "_t.png"), 150, 150);
                    File.Copy(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".png"), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + "t.png"), true);

                    File.Delete(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".png"));
                    SaveThumbnail(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + "t.png"), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".png"), 698, 326);

                    this.Save_Settings_File(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".xml"), SettingData);
                    this.Create_File(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, "stock\\templates\\visualizer.html"), Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".html"));
                    Callback callbackObj = new Callback(this.Page);
                    IWorkbook workbook = callbackObj.getWorkbookXLS(title, subtitle, source, keywords, tableData, imgInText);
                    workbook.SaveAs(Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, folderStructure + RetVal + "\\" + RetVal + ".xls"), SpreadsheetGear.FileFormat.XLS97);
                    workbook.Close();
                }
                #endregion                 
            }
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }
    
    public bool ThumbnailCallback()
    {
        return false;
    }
    
    #endregion "--Public--"    

    #endregion "--Methods--"


    #region "--R & D work--"
    //static Excel.Workbook MyBook = null;
    //static Excel.Application MyApp = null;
    //static Excel.Worksheet MySheet = null;

    //public string excelToImage(string DB_PATH)
    //{

    //    //MyApp = new Excel.Application();
    //    //MyApp.Visible = false;
    //    //MyBook = MyApp.Workbooks.Open(DB_PATH);
    //    //MySheet = (Excel.Worksheet)MyBook.Sheets[1]; // Explicit cast is not required here
        
        
    //    //lastRow = MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row; 
    //}
    #endregion "--R & D work--"
}
