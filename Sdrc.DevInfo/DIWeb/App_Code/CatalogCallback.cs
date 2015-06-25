using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Data;

/// <summary>
/// Summary description for CatalogCallback
/// </summary>
/// 
public partial class Callback : System.Web.UI.Page
{
    #region "-- Public --"

    #region "-- Methods --"

    public string GetJsonM49CountriesFile()
    {
        string RetVal = string.Empty;
        try
        {                       
            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            RetVal = CatalogService.GetJsonM49CountriesFile();
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string AdminSaveAdaptation(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string AdaptationName = string.Empty;
        string Description = string.Empty;
        string Version = string.Empty;
        bool IsDesktop;
        bool IsWeb;
        string WebURL = string.Empty;
        int AreaCount;
        int IUSCount;
        int TimePeriodsCount;
        int DataValuesCount;
        string StartYear = string.Empty;
        string EndYear = string.Empty;
        string LastModifiedOn = string.Empty;
        int AreaNId;
        string SubNation = string.Empty;
        string CatalogImage = string.Empty;
        string DbAdmName = string.Empty;
        string DbAdmInstitution = string.Empty;
        string DbAdmEmail = string.Empty;
        string UnicefRegion = string.Empty;
        string AdaptationYear = string.Empty;
        string DbLanguages = string.Empty;
        string LangCode_CSVFiles = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            AdaptationName = Params[0];
            Description = Params[1];
            Version = Params[2];
            IsDesktop = Convert.ToBoolean(Params[3]);
            IsWeb = Convert.ToBoolean(Params[4]);
            WebURL = Params[5];
            AreaCount = Convert.ToInt32(Params[6]);
            IUSCount = Convert.ToInt32(Params[7]);
            TimePeriodsCount = Convert.ToInt32(Params[8]);
            DataValuesCount = Convert.ToInt32(Params[9]);
            StartYear = Params[10];
            EndYear = Params[11];
            LastModifiedOn = Params[12];
            AreaNId = Convert.ToInt32(Params[13]);
            SubNation = Params[14];
            CatalogImage = Params[15];
            DbAdmName = Params[16];
            DbAdmInstitution = Params[17];
            DbAdmEmail = Params[18];
            UnicefRegion = Params[19];
            AdaptationYear = Params[20];
            DbLanguages = Params[21];
            if(Params.Length > 22)
            LangCode_CSVFiles = Params[22];
            string AdaptedFor = Global.adapted_for;
            XmlDocument XmlDoc;
            string AppSettingFile = string.Empty;
            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(AppSettingFile);
            Global.CheckAppSetting(XmlDoc, Constants.AppSettingKeys.Country, string.Empty);
            string Country = Global.Country;
            string typeOfEmail = string.Empty;
            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            DataSet dsCatalogAdaptation = new DataSet();
            dsCatalogAdaptation = CatalogService.CatalogExists(Global.GetAdaptationGUID());
            string DateCreated = string.Empty;
            string Visible = string.Empty;
            if (dsCatalogAdaptation.Tables[0].Rows.Count == 0)
            {
                typeOfEmail = "New";
                DateCreated = String.Format("{0:r}", DateTime.Now);
            }
            else
            {
                typeOfEmail = "Updated";
                DateCreated = String.Format("{0:r}", Convert.ToDateTime(dsCatalogAdaptation.Tables[0].Rows[0][1]));
                Visible = dsCatalogAdaptation.Tables[0].Rows[0][2].ToString();

            }
            if (CatalogService.SetCatalog(AdaptationName, Description, Version, IsDesktop, IsWeb, WebURL, AreaCount, IUSCount, TimePeriodsCount, DataValuesCount, StartYear, EndYear, LastModifiedOn, AreaNId, SubNation, CatalogImage, DbAdmName, DbAdmInstitution, DbAdmEmail, UnicefRegion, AdaptationYear, DbLanguages, LangCode_CSVFiles, AdaptedFor, Country, DateTime.Now.ToString(), Global.GetAdaptationGUID()))
            {
                RetVal = "true";
                Frame_Message_And_Send_Catalog_Mail(AdaptationName, WebURL, Visible, DbAdmName, DbAdmInstitution, DbAdmEmail, AdaptedFor, Country, SubNation, DateCreated, String.Format("{0:r}", DateTime.Now), typeOfEmail);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetJsonAdaptationFile()
    {
        string RetVal = string.Empty;
        //string WebURL = string.Empty;

        try
        {            
            //WebURL = Global.GetAdaptationUrl();

            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            RetVal = CatalogService.GetJsonAdaptations(Global.GetAdaptationGUID());
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetCatalogCacheResults(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string SearchAreas = string.Empty;
        string SearchIndicators = string.Empty;
        string LangCodeDb = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            SearchAreas = Params[0];
            SearchIndicators = Params[1];            
            LangCodeDb = Params[2];

            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            //RetVal = CatalogService.GetCatalogCacheResults(SearchAreas, SearchIndicators);

            RetVal = CatalogService.GetCatalogMatchedResults(SearchAreas, SearchIndicators, LangCodeDb);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string AdminDeleteCatalog(string requestParam)
    {
        string RetVal = string.Empty;
        int AdaptationNId = -1;

        try
        {
            AdaptationNId = Convert.ToInt32(requestParam);

            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            if(CatalogService.DeleteCatalog(AdaptationNId))
            {
                RetVal = "true";
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string AdminUpdateAdaptation(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string AdaptationName = string.Empty;
        string Description = string.Empty;
        string Version = string.Empty;
        bool IsDesktop;
        bool IsWeb;
        string WebURL = string.Empty;
        int AreaCount;
        int IUSCount;
        int TimePeriodsCount;
        int DataValuesCount;
        string StartYear = string.Empty;
        string EndYear = string.Empty;
        string LastModifiedOn = string.Empty;
        int AreaNId;
        string SubNation = string.Empty;
        string CatalogImage = string.Empty;
        int AdaptationNId = -1;
        string DbAdmName = string.Empty;
        string DbAdmInstitution = string.Empty;
        string DbAdmEmail = string.Empty;
        string UnicefRegion = string.Empty;
        string AdaptationYear = string.Empty;
        string DbLanguages = string.Empty;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            AdaptationName = Params[0];
            Description = Params[1];
            Version = Params[2];
            IsDesktop = Convert.ToBoolean(Params[3]);
            IsWeb = Convert.ToBoolean(Params[4]);
            WebURL = Params[5];
            AreaCount = Convert.ToInt32(Params[6]);
            IUSCount = Convert.ToInt32(Params[7]);
            TimePeriodsCount = Convert.ToInt32(Params[8]);
            DataValuesCount = Convert.ToInt32(Params[9]);
            StartYear = Params[10];
            EndYear = Params[11];
            LastModifiedOn = Params[12];
            AreaNId = Convert.ToInt32(Params[13]);
            SubNation = Params[14];
            CatalogImage = Params[15];
            AdaptationNId = Convert.ToInt32(Params[16]);
            DbAdmName = Params[17];
            DbAdmInstitution = Params[18];
            DbAdmEmail = Params[19];
            UnicefRegion = Params[20];
            AdaptationYear = Params[21];
            DbLanguages = Params[22];

            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            if (CatalogService.UpdateCatalog(AdaptationNId, AdaptationName, Description, Version, IsDesktop, IsWeb, WebURL, AreaCount, IUSCount, TimePeriodsCount, DataValuesCount, StartYear, EndYear, LastModifiedOn, AreaNId, SubNation, CatalogImage, DbAdmName, DbAdmInstitution, DbAdmEmail, UnicefRegion, AdaptationYear, DbLanguages))
            {
                RetVal = "true";
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string AdminGetAdaptationVersions()
    {
        string RetVal = string.Empty;        

        try
        {
            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            RetVal = CatalogService.GetAdaptationVersions();
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string CheckIsDI7ORGAdaptation()
    {
        string RetVal = string.Empty;

        try
        {
            if (Global.CheckIsDI7ORGAdaptation())
            {
                RetVal = "false";
            }
            else
            {
                DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
                CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
                RetVal = CatalogService.GetGlobalMasterWebUrl() + "/libraries/aspx/Catalog.aspx";
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string CheckGlobalAdaptation()
    {
        string RetVal = string.Empty;

        try
        {
            if (Global.CheckIsGlobalAdaptation())
            {
                RetVal = "false";
            }
            else
            {
                DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
                CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
                RetVal = CatalogService.GetGlobalMasterWebUrl() + "/libraries/aspx/Catalog.aspx";
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    public string GetGlobalAdaptationURL()
    {
        string GlobalMasterWebUrl = string.Empty;
        string RetVal = string.Empty;

        try
        {
            DIWorldwide.Catalog catalog = new DIWorldwide.Catalog();
            catalog.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            GlobalMasterWebUrl = catalog.GetGlobalMasterWebUrl();
            RetVal = GlobalMasterWebUrl + "/libraries/aspx/";
        }

        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }

        return RetVal;
    }

    #endregion

    #endregion
}