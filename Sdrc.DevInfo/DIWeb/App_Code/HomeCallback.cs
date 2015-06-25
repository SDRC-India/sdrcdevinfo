using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Globalization;
using System.Text;

public partial class Callback : System.Web.UI.Page
{
    public string GetIndWhereDataExists(string requestParam)
    {
        string RetVal = string.Empty;
        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();

        string[] Params;
        string AreaNids = string.Empty;
        string IUNids = string.Empty;
        string LngCode = string.Empty;
        int DbNid = -1;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            AreaNids = Params[0];
            IUNids = Params[1];
            DbNid = int.Parse(Params[2]);
            LngCode = Params[3];
            DIConnection ObjDIConnection = this.GetDbConnection(DbNid);

            if (ObjDIConnection != null)
            {
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
                Param1.ParameterName = "strAllAreaQSIds_Nids";
                Param1.DbType = DbType.String;
                Param1.Value = AreaNids;
                DbParams.Add(Param1);

                System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();
                Param2.ParameterName = "strIU_Nids";
                Param2.DbType = DbType.String;
                Param2.Value = IUNids;
                DbParams.Add(Param2);

                DataTable dtAreaWhereExist = ObjDIConnection.ExecuteDataTable("sp_get_ind_wheredataexists_" + Global.GetDefaultLanguageCodeDB(DbNid.ToString(), LngCode), CommandType.StoredProcedure, DbParams);

                foreach (DataRow Row in dtAreaWhereExist.Rows)
                {
                    RetVal += "," + Row[0];
                }

                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal = RetVal.Substring(1);
                }
                else
                {
                    RetVal = "";
                }
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    public string GetAreaWhereDataExists(string requestParam)
    {
        string RetVal = string.Empty;
        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();

        string[] Params;
        string AreaNids = string.Empty;
        string IUNids = string.Empty;
        string LngCode = string.Empty;
        int DbNid = -1;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            AreaNids = Params[0];
            IUNids = Params[1];
            DbNid = int.Parse(Params[2]);
            LngCode = Params[3];
            DIConnection ObjDIConnection = this.GetDbConnection(DbNid);

            if (ObjDIConnection != null)
            {
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
                Param1.ParameterName = "strAllAreaQSIds_Nids";
                Param1.DbType = DbType.String;
                Param1.Value = AreaNids;
                DbParams.Add(Param1);

                System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();
                Param2.ParameterName = "strIUS_Nids";
                Param2.DbType = DbType.String;
                Param2.Value = IUNids;
                DbParams.Add(Param2);

                DataTable dtAreaWhereExist = ObjDIConnection.ExecuteDataTable("sp_get_area_wheredataexists_" + Global.GetDefaultLanguageCodeDB(DbNid.ToString(), LngCode), CommandType.StoredProcedure, DbParams);

                foreach (DataRow Row in dtAreaWhereExist.Rows)
                {
                    RetVal += "," + Row[0];
                }

                if (!string.IsNullOrEmpty(RetVal))
                {
                    RetVal = RetVal.Substring(1);
                }
                else
                {
                    RetVal = "";
                }
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    /// <summary>
    /// Get default area names of database
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string GetSelectedArea(string requestParam)
    {
        string RetVal = string.Empty;
        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();

        string[] Params;
        string DbNid = string.Empty;
        string DefLngCodeDb = string.Empty;
        string DefArea = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            DbNid = Params[0];
            DefLngCodeDb = Params[1];

            DefArea = Global.GetDefaultArea(DbNid);

            DIConnection ObjDIConnection = this.GetDbConnection(int.Parse(DbNid));

            if (ObjDIConnection != null)
            {
                System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
                Param1.ParameterName = "AreaNids";
                Param1.DbType = DbType.String;
                Param1.Value = DefArea;
                DbParams.Add(Param1);

                DataTable dtAreaNames = ObjDIConnection.ExecuteDataTable("sp_get_AreaNames_" + DefLngCodeDb, CommandType.StoredProcedure, DbParams);

                if (dtAreaNames.Rows.Count > 0)
                {
                    foreach (DataRow Row in dtAreaNames.Rows)
                    {
                        RetVal += ", " + Row[0];
                    }

                    if (!string.IsNullOrEmpty(RetVal))
                    {
                        RetVal = RetVal.Substring(2);
                    }
                    else
                    {
                        RetVal = "false";
                    }
                }
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    /// <summary>
    /// Get default area cound of database
    /// </summary>
    /// <param name="requestParam"></param>
    /// <returns></returns>
    public string GetDefaultAreaCountOfSelDb(string requestParam)
    {
        string RetVal = string.Empty;
        string DbNid = requestParam;

        try
        {
            if (!string.IsNullOrEmpty(DbNid))
            {
                RetVal = Convert.ToString(Global.GetDefaultAreaCount(DbNid));
            }
            else
            {
                RetVal = "false";
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

        }

        return RetVal;
    }

    /// <summary>
    /// Get disclaimer text from diworldwide web service
    /// </summary>
    /// <returns></returns>
    public string GetDisclaimerText()
    {
        string RetVal = string.Empty;

        try
        {
            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            RetVal = CatalogService.GetDisclaimerText();
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return RetVal;
    }

    //Check If enrty does not exist in Global Catalog for this adaptation, then insert an entry and make Master Account as Admin
    public string EntryIntoGlobalCatalog(string requestParam)
    {
        string RetVal = string.Empty;
        string CatalogImageURL = requestParam;
        DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
        CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
        DataSet dsCatalogAdaptation = new DataSet();
        //dsCatalogAdaptation = CatalogService.CatalogExists(Global.GetAdaptationUrl());
        dsCatalogAdaptation = CatalogService.CatalogExists(Global.GetAdaptationGUID());
        if (dsCatalogAdaptation.Tables[0].Rows.Count == 0)
        {
            string AdaptationName;
            string DbAdmName;
            string DbAdmInstitution;
            string DbAdmEmail;
            string Subnational;
            string AdaptedFor;
            string AreaNId;
            string Version;
            XmlDocument XmlDoc;
            string AppSettingFile = string.Empty;
            AdaptationName = Global.adaptation_name;
            DbAdmName = Global.DbAdmName;
            DbAdmInstitution = Global.DbAdmInstitution;
            DbAdmEmail = Global.DbAdmEmail;
            AdaptedFor = Global.adapted_for;
            Subnational = Global.sub_nation;
            AreaNId = Global.area_nid;
            Version = Global.application_version;
            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(AppSettingFile);
            Global.CheckAppSetting(XmlDoc, Constants.AppSettingKeys.Country, string.Empty);
            string Country = Global.Country;
            GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            //DataSet GlobalUserDetail = GlobalUserWebService.GetMasterAccountDetail(Global.GetAdaptationUrl());
            DataSet GlobalUserDetail = GlobalUserWebService.GetMasterAccountDetail(Global.GetAdaptationGUID());
            string UserNId = GlobalUserDetail.Tables[0].Rows[0][0].ToString();
            string FirstName = GlobalUserDetail.Tables[0].Rows[0][2].ToString();
            Create_MaintenanceAgency_ForAdmin(UserNId, FirstName, Global.GetDefaultLanguageCode());
            //GlobalUserWebService.IsAdminRegistered(Global.GetAdaptationUrl(), AdaptationName, DbAdmName, DbAdmInstitution, DbAdmEmail, AdaptedFor, Country, Subnational, AreaNId, CatalogImageURL, "5");

            GlobalUserWebService.IsAdminRegistered(Global.GetAdaptationUrl(), AdaptationName, DbAdmName, DbAdmInstitution, DbAdmEmail, AdaptedFor, Country, Subnational, AreaNId, CatalogImageURL, "5", Global.GetAdaptationGUID());
            Frame_Message_And_Send_Catalog_Mail(AdaptationName, Global.GetAdaptationUrl(), "False", DbAdmName, DbAdmInstitution, DbAdmEmail, AdaptedFor, Country, Subnational, String.Format("{0:r}", DateTime.Now), String.Format("{0:r}", DateTime.Now), "New");
        }
        return RetVal;
    }

    // creats a new file from input file content
    public string EditCMSContent(string requestParam)
    {
        string RetVal = string.Empty;
        string Relativepath = string.Empty;
        string HtmlContent = string.Empty;
        string[] Params;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            Relativepath = Params[0];
            HtmlContent = Params[1];

            string FullPath = Server.MapPath(Relativepath);
            System.IO.File.WriteAllText(FullPath, HtmlContent, System.Text.Encoding.GetEncoding("iso-8859-1"));
            Relativepath = Params[0];

            RetVal = "true";
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    // Create new content page andd add link on main html page
    public string AddCMSContent(string requestParam)
    {
        string RetVal = string.Empty;
        string Relativepath = string.Empty;
        string HtmlContent = string.Empty;
        string[] Params;
        string LinkUrl = string.Empty;
        string Title = string.Empty;
        string Date;
        string Summary = string.Empty;
        string DetailedContent = string.Empty;
        string PdfSrc = string.Empty;
        string TargetFolderPath = string.Empty;
        string PageCategory = string.Empty;
        string ImageUrl = string.Empty;
        string MonthName = string.Empty;
        string FullLinkUrl = string.Empty;
        string TargetFilePath = string.Empty;
        string UserEmailID = string.Empty;
        string UserName = string.Empty;
        string RequestingPagePath = string.Empty;
        string DBConnectionStatusMessage = string.Empty;
        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        DIConnection ObjDIConnection = null;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            if (!string.IsNullOrEmpty(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUserNId].ToString()))
            {
                UserEmailID = Global.Get_User_EmailId_ByAdaptationURL(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUserNId].ToString());
            }
            // get user name from session
            if (!string.IsNullOrEmpty(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser].ToString()))
            {
                UserName = HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser].ToString();
            }

            LinkUrl = Params[0];
            Title = Params[1];
            Date = Params[2];
            Summary = Params[3];
            DetailedContent = Params[4];
            PdfSrc = Params[5];
            RequestingPagePath = Params[6];
            ImageUrl = Params[7];

            //DataTable dtAreaNames = ObjDIConnection.ExecuteNonQuery("usp_AddCMSContent", CommandType.StoredProcedure, DbParams);


            // create new html page
            if (RequestingPagePath.Contains("news"))
            {
                TargetFolderPath = Constants.FolderName.Diorg_news_content;
                //This path is hard coaded so that news content save on first page only need to handle this later
                TargetFilePath = "diorg\\di_news.html";
            }
            if (RequestingPagePath.Contains("action"))
            {
                TargetFolderPath = Constants.FolderName.Diorg_action_content;
                //This path is hard coaded so that action content save on first page only need to handle this later
                TargetFilePath = "diorg\\devinfo_in_action.html";
            }
            // Check if html file exist
            String LinkUrlFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, TargetFolderPath, LinkUrl);

            if (File.Exists(LinkUrlFullPath))
            {
                Random rnd = new Random();
                string RandomNo = rnd.Next(0, 10000).ToString();
                LinkUrl = LinkUrl.Insert(LinkUrl.Length - 5, RandomNo);
            }
            if (CreateNewInnerContentPage(Title, DetailedContent, Date, PdfSrc, TargetFolderPath, LinkUrl) == "true")
            {
                // add link to Html page
                FullLinkUrl = Path.Combine(TargetFolderPath, LinkUrl);
                string[] LinkArray = FullLinkUrl.Split(new string[] { "aspx\\" }, StringSplitOptions.None);
                if (UpdateMainContentPage(Title, Summary, Date, LinkArray[1].ToString(), ImageUrl, TargetFilePath) == "true")
                {
                    RetVal = "true";
                }
            }
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;

    }


    // Create new content page
    private string CreateNewInnerContentPage(string Title, string DetailedContent, string Date, string PdfSrc, string TargetFolderPath, string TargetFileName)
    {
        string RetVal = string.Empty;
        string CMSTemplateFullPath = string.Empty;
        string CMSTargetFileFullPath = string.Empty;
        string CMSTargetFilePath = string.Empty;
        try
        {

            CMSTemplateFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.CmsTemplateFolder, Constants.FileName.CmsNewContentPage_Template);

            CMSTargetFileFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, TargetFolderPath, TargetFileName);
            // GetHtml Template for Creating new CMS page
            string HtmlTemplate = GetHtmlTemplate(CMSTemplateFullPath);

            // Set Values in Html Template
            HtmlTemplate = HtmlTemplate.Replace("#Title", Title);
            HtmlTemplate = HtmlTemplate.Replace("#MainContent", DetailedContent);
            HtmlTemplate = HtmlTemplate.Replace("#PdfUrl", PdfSrc);
            HtmlTemplate = HtmlTemplate.Replace("#Date", Date);
            System.IO.File.WriteAllText(CMSTargetFileFullPath, HtmlTemplate, System.Text.Encoding.GetEncoding("iso-8859-1"));
            RetVal = "true";

        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    private string UpdateMainContentPage(string Title, string Summary, string Date, string LinkUrl, string ImageUrl, string TargetFilePath)
    {
        string RetVal = string.Empty;
        string CMSTemplateFullPath = string.Empty;
        //  string CMSTargetFileFullPath = string.Empty;
        string RequestingPageFullPath = string.Empty;
        string[] TableHtml = null;
        string[] InnerTableHtml = null;
        string FinalTableHtml = string.Empty;
        string InnerHTML = string.Empty;
        string HtmlLinkTemplate = string.Empty;
        try
        {
            LinkUrl = LinkUrl.Replace("\\", @"/");
            if (TargetFilePath.Contains("action"))
            {
                //HtmlLinkTemplate = Constants.FileName.CmsNewLinkContent_Template;
                HtmlLinkTemplate = "ActionLink_Template.html";
            }
            else
            {
                HtmlLinkTemplate = "NewsLink_Template.html";
            }

            CMSTemplateFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.CmsTemplateFolder, HtmlLinkTemplate);

            RequestingPageFullPath = Server.MapPath(TargetFilePath);
            // GetHtml Template for Creating new CMS page
            string LinkHtmlTemplate = GetHtmlTemplate(CMSTemplateFullPath);

            // Set Values in Html Template
            LinkHtmlTemplate = string.Format(LinkHtmlTemplate, LinkUrl, Title, Date, ImageUrl, Summary);


            string PageHtmlTemplate = GetHtmlTemplate(RequestingPageFullPath);
            PageHtmlTemplate = PageHtmlTemplate.Replace("< table", "<table").Replace("tbody >", "tbody>").Replace("tr >", "tr>").Replace("< tr", "<tr");
            TableHtml = PageHtmlTemplate.Split(new string[] { "news_space" }, StringSplitOptions.RemoveEmptyEntries);
            if (!string.IsNullOrEmpty(TableHtml[0].ToString()))
            {
                InnerTableHtml = TableHtml[0].ToString().Split(new string[] { "<tr>" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < InnerTableHtml.Length; i++)
                {
                    if (string.IsNullOrEmpty(InnerHTML))
                    {
                        InnerHTML = InnerTableHtml[i].ToString();
                    }
                    else
                    {
                        if (i == InnerTableHtml.Length - 2)
                        {
                            InnerHTML += InnerTableHtml[i] + LinkHtmlTemplate + "<tr>" + InnerTableHtml[i + 1];
                            break;
                        }
                        else
                        {
                            if (InnerTableHtml.Length == 2)
                            {
                                //InnerHTML += InnerTableHtml[i] + LinkHtmlTemplate + "<tr>" + InnerTableHtml[i + 1];
                                InnerHTML += LinkHtmlTemplate + "<tr>" + InnerTableHtml[i].ToString();
                                break;
                            }
                            else
                            {
                                InnerHTML += "<tr>" + InnerTableHtml[i].ToString();
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(InnerHTML))
                {
                    TableHtml[0] = InnerHTML;
                }

                for (int i = 0; i < TableHtml.Length; i++)
                {
                    if (string.IsNullOrEmpty(FinalTableHtml))
                    {
                        FinalTableHtml = TableHtml[i].ToString();
                    }
                    else
                    {
                        FinalTableHtml += "news_space" + TableHtml[i].ToString();
                    }
                }

            }

            System.IO.File.WriteAllText(RequestingPageFullPath, FinalTableHtml, System.Text.Encoding.GetEncoding("iso-8859-1"));
            RetVal = "true";

        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    // Creates copy of cms template file
    public bool CopyCMSTemplateFile(string TemplateFileName, string TemplateTargetLocation, string TargetFileName)
    {
        string CMSTemplatefolderPath = string.Empty;
        string CMSTemplateFileName = string.Empty;
        string SaveTemplateToPath = string.Empty;
        string CMSTemplateFullPath = string.Empty;
        string CMSTargetFileFullPath = string.Empty;

        bool RetVal = false;
        try
        {
            CMSTemplatefolderPath = Constants.FolderName.CmsTemplateFolder;
            CMSTemplateFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, CMSTemplatefolderPath, TemplateFileName);
            CMSTargetFileFullPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, TemplateTargetLocation, TargetFileName);
            if (!File.Exists(CMSTemplateFullPath))
            {
                return false;
            }
            else
            {
                File.Copy(CMSTemplateFullPath, CMSTargetFileFullPath);
                RetVal = true;
            }
        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }
    /// <summary>
    /// Read Html template for description 
    /// </summary>
    /// <param name="KmlTemplatePath">Template source location</param>
    /// <returns>return string containig template if template exist else return empty string</returns>
    private string GetHtmlTemplate(string FileFullPath)
    {
        string RetVal = string.Empty;
        try
        {

            string HtmlTemplatePath = FileFullPath;
            System.Net.WebClient client = new System.Net.WebClient();
            if (File.Exists(HtmlTemplatePath))
            {
                String htmlCode = client.DownloadString(HtmlTemplatePath);
                RetVal = htmlCode;
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    // Create new content page and add link on main html page   
    public string AddCMSContents(string requestParam)
    {
        string RetVal = string.Empty;
        bool IsArticleAdded = false;
        try
        {
            HtmlContentAdder ImportContent = new HtmlContentAdder();
            IsArticleAdded = ImportContent.CreateCmsContent(requestParam);
            RetVal = IsArticleAdded.ToString();
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;

    }
    // Create new content page and add link on main html page   
    public string EditArticle(string requestParam)
    {
        string RetVal = string.Empty;
        bool IsArticleUpdated = false;
        string OutCurrentPageNo = string.Empty;
        string MenuCategory = string.Empty;
        string OutCurrentTagNids = string.Empty;
        try
        {
            EditHtmlContent EditContent = new EditHtmlContent();

            IsArticleUpdated = EditContent.EditArticleByArticleId(requestParam, out OutCurrentPageNo, out MenuCategory, out OutCurrentTagNids);
            if (IsArticleUpdated && !string.IsNullOrEmpty(OutCurrentPageNo) || !string.IsNullOrEmpty(OutCurrentTagNids))
            {
                Session[Constants.SessionNames.CurrentArticlePageNo] = OutCurrentPageNo;
                Session[Constants.SessionNames.CurrentArticleTagIds] = OutCurrentTagNids;
                Session[Constants.SessionNames.CurrentArticleMenuCategory] = MenuCategory;
            }
            RetVal = IsArticleUpdated.ToString();
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;

    }

    // Create new content page and add link on main html page   
    public string GetArticlesByMenuCategoryAndTags(string requestParam)
    {
        string RetContentHtml = string.Empty;
        ArticlesGenerator GenerateArticle = new ArticlesGenerator();
        // HtmlGenerator GenerateHtml = new HtmlGenerator();
        string MenuCategory = string.Empty;
        int MaxArticleCount = 0;
        int RecordStartPasition = 1;
        int currentPageNumber = 1;
        String TagNids = string.Empty;
        string[] Params;
        bool IsHiddenArticleVisible = false;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            MenuCategory = Params[0];
            TagNids = Params[1];
            currentPageNumber = Convert.ToInt32(Params[2].ToString().Trim());
            IsHiddenArticleVisible = Convert.ToBoolean(Params[3]);
            if (!string.IsNullOrEmpty(Global.MaxArticlesCount))
            {
                MaxArticleCount = Convert.ToInt32(Global.MaxArticlesCount);
            }
            RecordStartPasition = (currentPageNumber - 1) * MaxArticleCount + 1;

            if (!string.IsNullOrEmpty(TagNids))// if tag is not null or empty get records by tag
            {
                RetContentHtml = GenerateArticle.GetArticleByMenuCategoryNTag(RecordStartPasition, MaxArticleCount, MenuCategory, currentPageNumber, TagNids, IsHiddenArticleVisible);
            }
            else
            {
                RetContentHtml = GenerateArticle.GetArticleByMenuCategory(RecordStartPasition, MaxArticleCount, MenuCategory, currentPageNumber, IsHiddenArticleVisible);
            }
        }
        catch (Exception Ex)
        {
            RetContentHtml = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetContentHtml;

    }



    public string GetArticleByContentIdForSharing(string CallingPageUrl)
    {
        string RetVal = string.Empty;
        try
        {
            ArticlesGenerator GenerateArticle = new ArticlesGenerator();
            DataContent ObjDataContent = new DataContent();

            // Call method to get data content based on url
            ObjDataContent = GenerateArticle.GetDataContentFromDatabaseByUrl(CallingPageUrl, false);

            if (ObjDataContent != null)
            {
                RetVal = GenerateArticle.GetHtmlByMenuCategory(ObjDataContent).Replace("\"../","\"../../../");               
            }
            RetVal = RetVal + Constants.Delimiters.ParamDelimiter + ObjDataContent.Title + Constants.Delimiters.ParamDelimiter + ObjDataContent.Date;
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    // Create new content page and add link on main html page   
    public string GetTagsListByMenuCategory(string requestParam)
    {
        string RetContentHtml = string.Empty;
        ArticlesGenerator GenerateArticle = null;
        string MenuCategory = string.Empty;
        bool IsHiddenArticleVisible = false;
        string[] Params;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            MenuCategory = Params[0];
            IsHiddenArticleVisible = Convert.ToBoolean(Params[1]);
            if (!string.IsNullOrEmpty(MenuCategory))
            {
                GenerateArticle = new ArticlesGenerator();
                RetContentHtml = GenerateArticle.GetTagsListByMenuCategory(MenuCategory, IsHiddenArticleVisible);
            }
        }
        catch (Exception Ex)
        {
            RetContentHtml = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetContentHtml;
    }

    public string DeleteArticlebyContentId(string requestParam)
    {
        string RetVal = string.Empty;
        EditHtmlContent EditArticle = null;
        int ContentId = 0;
        string[] Params;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            ContentId = Convert.ToInt32(Params[0]);
            if (ContentId > 0)
            {
                EditArticle = new EditHtmlContent();
                RetVal = EditArticle.DeleteArticlebyContentId(ContentId).ToString().ToLower();
            }
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    public string ShowHideArticlebyContentId(string requestParam)
    {
        string RetVal = string.Empty;
        EditHtmlContent EditArticle = null;
        int ContentId = 0;
        bool IsHidden = true;
        string[] Params;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            ContentId = Convert.ToInt32(Params[0]);
            IsHidden = Convert.ToBoolean(Params[1]);
            if (ContentId > 0)
            {
                EditArticle = new EditHtmlContent();
                RetVal = EditArticle.ShowHideArticlebyContentId(ContentId, IsHidden).ToString().ToLower();
            }
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    public string GetArticlesMenucategoriesListJson(string requestParam)
    {
        string RetVal = string.Empty;
        MenuCategories ObjMenuCat = null;
        bool isShowHiddenMenuCat = true;
        string PageName=  string.Empty;
            string[] Params;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            PageName = Params[0];
            ObjMenuCat = new MenuCategories();
            RetVal = ObjMenuCat.GetMenuCategoriesListJson(isShowHiddenMenuCat, PageName);
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    public string DeleteMenuCategory(string requestParam)
    {
        string RetVal = string.Empty;
        MenuCategories ObjMenuCat = null;
        string CategoryName = string.Empty;
        string[] Params;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            CategoryName = Params[0];
            ObjMenuCat = new MenuCategories();
            RetVal = ObjMenuCat.DeleteMenuCategory(CategoryName).ToString();
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }
    public string AddMenuCategory(string requestParam)
    {
        string CategoryName = string.Empty;
        string LinkText = string.Empty;
        string HeaderText = string.Empty;
        string HeaderDesc = string.Empty;
        string RetVal = string.Empty;
        string PageName = string.Empty;
        MenuCategories ObjMenuCat = null;
        string[] Params;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            CategoryName = Params[0];
            LinkText = Params[1];
            HeaderText = Params[2];
            HeaderDesc = Params[3];
            PageName = Params[4];

            ObjMenuCat = new MenuCategories();
            RetVal = ObjMenuCat.AddMenuCategory(CategoryName, LinkText, HeaderText, HeaderDesc, PageName).ToString();
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }
    public string EditMenuCategory(string requestParam)
    {
        string CategoryName = string.Empty;
        string LinkText = string.Empty;
        string HeaderText = string.Empty;
        string HeaderDesc = string.Empty;
        string RetVal = string.Empty;
        string SelcetedPageName = string.Empty;
        MenuCategories ObjMenuCat = null;
        string[] Params;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            CategoryName = Params[0];
            LinkText = Params[1];
            HeaderText = Params[2];
            HeaderDesc = Params[3];
            SelcetedPageName = Params[4];
            ObjMenuCat = new MenuCategories();
            RetVal = ObjMenuCat.EditMenuCategory(CategoryName, LinkText, HeaderText, HeaderDesc, SelcetedPageName).ToString();
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    public string MoveUpNDownMenuCat(string requestParam)
    {
        string CategoryName = string.Empty;
        bool MoveUp = false;
        bool MoveDown = false;
        string RetVal = string.Empty;
        MenuCategories ObjMenuCat = null;
        string PageName = string.Empty;
        string[] Params;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            CategoryName = Params[0];
            MoveUp = Convert.ToBoolean(Params[1]);
            MoveDown = Convert.ToBoolean(Params[2]);
            PageName = Params[3];
            ObjMenuCat = new MenuCategories();
            RetVal = ObjMenuCat.MoveUpNDownMenuCat(CategoryName, MoveUp, MoveDown, PageName).ToString();
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }
    public string InstallPatch()
    {
        string RetVal = string.Empty;
        PatchInstaller ObjInstallPatch = null;
        try
        {

            ObjInstallPatch = new PatchInstaller();
            RetVal = ObjInstallPatch.InstallPatch().ToString();
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    public string UpdateLanguageFiles()
    {
        string RetVal = string.Empty;
        PatchInstaller ObjInstallPatch = null;
        try
        {

            ObjInstallPatch = new PatchInstaller();
            RetVal = ObjInstallPatch.UpdateLanguageFiles().ToString();
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }
    public string UpdateAppSettingFile()
    {
        string RetVal = string.Empty;
        PatchInstaller ObjInstallPatch = null;
        try
        {
            ObjInstallPatch = new PatchInstaller();
            RetVal = ObjInstallPatch.UpdateAppSettingFile().ToString();
            if (ObjInstallPatch.UpdateAppSettingFile())
            {
                string LogMessage = PatchInstaller.ReadKeysForPatch("AddKeysToAppsetPassed").ToString();
                XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, PatchInstaller.ReadKeysForPatch("StatusPassed").ToString(),string.Empty);       
            }
            else
            {
                string LogMessage = string.Format(PatchInstaller.ReadKeysForPatch("AddKeysToAppsetPassed").ToString());
                XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, PatchInstaller.ReadKeysForPatch("StatusPassed").ToString(),string.Empty);       
            }
        }
        catch (Exception Ex)
        {
            string LogMessage = PatchInstaller.ReadKeysForPatch("AddKeysToAppsetFailed").ToString();
            XLSLogGenerator.WriteLogForPatchInstallation(LogMessage, PatchInstaller.ReadKeysForPatch("StatusFail").ToString(),string.Empty);       
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }


}