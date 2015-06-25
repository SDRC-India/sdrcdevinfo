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
using System.Collections.Generic;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Text;
using System.Globalization;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Xml.Xsl;
using System.Linq;

public partial class libraries_aspx_Callback : System.Web.UI.Page
{
    #region "--Private--"

    #region "--Variables--"

    private enum CallbackType
    {
        None = 0,
        GetIndWhereDataExists = 1,
        GetAreaWhereDataExists = 2,
        GetDataView = 3,
        GetQDSResults = 4,
        GetDataViewDataNids = 5,
        GetGalleryThumbnails = 6,
        GetAreas = 7,
        GetSGs = 8,
        GetSources = 9,
        GetTPs = 10,
        GetASDatabases = 11,
        GetASResults = 12,
        isHTMLfilePrepared = 13,
        sharedPresentionUrl = 14,
        chartInputData = 15,
        ShareByEmail = 16,
        ShareSearchResult = 17,
        AdminUpdateConfiguration = 18,
        GetCloudContent = 19,
        GetTransformedCodelist = 20,
        GetDPArtefact = 21,
        AdminGetAvlConnections = 22,
        AdminGetConnectionDetailsData = 23,
        AdminUpdateDbConnection = 24,
        GetViewDataChildDivHtml = 25,
        GetSelectedDatabasesDetails = 26,
        GetSelectedArea = 27,
        AddSubscription = 28,
        GetDefaultAreaCountOfSelDb = 29,
        Gallery_GetQDSResults = 30,
        Gallery_GetASResults = 31,
        Gallery_ShareSearchResult = 32,
        GetSubscriptionDetails = 33,
        SaveGalleryItem = 34,
        AdminTestConnection = 35,
        AdminDeleteConnection = 36,
        AdminRegisterDatabase = 37,
        AdminGetAllDbConnections = 38,
        AdminXMLGeneration = 39,
        AdminSaveDefaultArea = 40,
        NGallery_GetQDSResults = 41,
        NGallery_GetASResults = 42,
        NGallery_GetMoreKeywords = 43,
        GetICTypes = 44,
        GetCategoryScheme = 45,
        AdminAddNewLanguage = 46,
        AdminSDMXArtefactsGeneration = 47,
        RegisterUser = 48,
        LoginUser = 49,
        AdminGetAllLanguageList = 50,
        AdminDeleteLanguage = 51,
        GenerateAllPagesXML = 52,
        AddRegistration = 53,
        GetRegistrationsSummary = 54,
        GetRegistrationDetails = 55,
        UpdateRegistration = 56,
        DeleteRegistration = 57,
        SendNotifications = 58,
        GetLoggedInUserSubscriptions = 59,
        UpdateSubscription = 60,
        DeleteSubscription = 61,
        GetOatStringFromFileUpload = 62,
        GetEmailIdOfLoggedInUser = 63,
        BindDPSchemeAndDFD = 64,
        BindDataProviders = 65,
        GetLangDBcode = 66,
        GetUserDetails = 67,
        UpdateUserDetails = 68,
        GetDataViewDataNidsMulipleDbs = 69,
        ForgotPassword = 70,
        ValidateSDMXML = 71,
        ValidateDSD = 72,
        GetUnmatchlist = 73,
        AdminMapFilesGeneration = 74,
        AdminDBScriptGeneration = 75,
        GetDataQueryRequestSOAP = 76,
        GetDataQueryResponseSDMXML = 77,
        CompareUserDSDAgainstDevInfoDSD = 78,
        CompareCodelists = 79,
        CompareUserDSD1AgainstUserDSD2 = 80,
        GetTimePeriods = 81,
        GetDataQueryResponseJSON = 82,
        GetDataQueryResponseXML = 83,
        GenerateComparisonReport = 84,
        AdminSDMXMLGeneration = 86,
        AdminSDMXMLRegistration = 87,
        GetDataQueryRequestHTTP = 88,
        GetDataQueryRequestREST = 89,
        GenerateCacheResults = 92,
        AdminRegisterDatabaseForUploadedDSD = 93,
        AdminMetadataGeneration = 94,
        GetMSDList = 95,
        GetMSDAttributes = 96,
        GetMFDList = 97,
        GetDFDAndMFDList = 98,
        AdminGetAllTheUploadedDSDsTable = 99,
        AdminUpdateDatabaseForUploadedDSD = 100,
        CheckIfUploadedDSD = 101,
        AdminGetAllTheUploadedDSDsList = 102,
        AdminGetAllTheUploadedMSDsTable = 103,
        AdminMetadataRegistration = 104,
        AdminCheckIfDSDAlreadyUploaded = 105,
        AdminGetAllTheDatabasesTable = 106,
        AdminGetAllTheDatabaseList = 107,
        DataProviderGetTPDivInnerHTML = 108,
        DataProviderGetAreaDivInnerHTML = 109,
        DataProviderGetIndicatorDivInnerHTML = 110,
        DataProviderGetDatabaseDivInnerHTML = 111,
        DataProviderGetDSDDivInnerHTML = 112,
        DataProviderGenerateSDMXML = 113,
        DataProviderRegisterSDMXML = 114,
        DataProviderGenerateMetadata = 115,
        DataProviderRegisterMetadata = 116,
        ValidateMetadataReport = 117,
        GetDataForGenerateApiCall = 119,
        GetDataQuerySources = 124,
        DataProviderGetSourceDivInnerHTML = 125,
        BindHelpText = 126,
        BindCodelistMappingLists = 127,
        GenerateCodelistMappingXml = 128,
        CheckIfAdminLoggedIn = 129,
        GetQdsResultsForAdaptation = 131,
        GetAllUsersHTMLForAdmin = 132,
        UpdateUsers = 133,
        GetIndicativeIdAgencyIdVersionValues = 134,
        RequestAdminForDataProviderRights = 135,
        ViewContent = 136,
        GenerateMetadataMappingXml = 137,
        BindMetadataMappingLists = 138,
        BindIUSMappingList = 139,
        GenerateIUSMappingXml = 140,
        BindRegistrations = 141,
        GetDefaultDb = 142,
        InitiaizeMap = 200,
        ZoomIn = 201,
        ZoomOut = 202,
        ZoomToRectangle = 203,
        Pan = 204,
        FullExtent = 205,
        Label = 206,
        Border = 207,
        NorthSymbol = 208,
        Scale = 209,
        Resize = 210,
        GetThemeInfo = 211,
        ResetLegendColor = 212,
        GetSeriesInfo = 213,
        EditThemeInfo = 214,
        GetTitleInfo = 215,
        GetDisclaimerInfo = 216,
        SetTitleInfo = 217,
        SetDisclaimerInfo = 218,
        GetDataLabelInfo = 219,
        SetDataLabelInfo = 220,
        SetLegendSeriesColor = 222,
        SetBorderStyle = 223,
        GetBorderStyle = 224,
        SetDataLabels = 225,
        GetDataLabels = 226,
        RefreshImage = 227,
        GetMapImageURL = 228,
        AddTheme = 229,
        SetThemeVisiblity = 230,
        DeleteTheme = 231,
        UpdateTheme = 232,
        UpdateDotDensityTheme = 234,
        GetUpdatedLegendRanges = 235,
        GetLegendRangesByThemeChange = 236,
        ChangeHatchPatern = 237,
        UpdateLegendRange = 238,
        Smooth = 239,
        UpdateChartSettings = 240,
        UpdateChartLegendSettings = 241,
        SetDisputedBoundriesVisiblity = 242,
        GetLayerTaggingInformation = 243,
        GetAreaInformation = 244,
        SetLegendInfo = 246,
        GetLegendInfo = 247,
        Nudging = 248,
        GetKMZ = 249,
        InitiaizeGoogleMap = 250,
        GetFeatureLayerInfo = 251,
        GetBaseLayerInfo = 252,
        SetFeatureLayerInfo = 253,
        SetBaseLayerInfo = 254,
        SetDisputedBoundryVisible = 255,
        ShowDisputedBoundryDefault = 256,
        DeleteFile = 273,
        AdminSaveDefaultIndcator = 260,
        IsUserAdmin = 261,
        AdminLoginUser = 262,
        GetAllUsersGridHTML = 263,
        DeleteUser = 264,
        DeletePresentation = 265,
        StoreMappingInfo = 266,
        GettingMappingInfo = 267,
        UploadLegends = 268,
        GettingMatchingAreas = 269,
        GetJsonM49CountriesFile = 270,
        AdminSaveAdaptation = 271,
        GetJsonAdaptations = 275,
        GetCatalogCacheResults = 276,
        IsAdminRegister = 277,
        AdminGetLanguageGridHTML = 278,
        AdminSaveLanguageChanges = 279,
        AdminLogout = 280,
        AdminDeleteCatalog = 281,
        AdminUpdateAdaptation = 282,
        GetDisclaimerText = 283,
        SetChartTimePeriodsVisiblity = 284,
        GetChartTimePeriods = 285,
        GetSeriesThemeName = 286,
        AdminGetAdaptationVersions = 287,
        AccountActivation = 288,
        ChangePassword = 289,
        GetTimeperiodOfSeries = 290,
        GetChartLegendOpacity = 291,
        ResetAllTheme = 292,
        GetThemeCount = 293,
        GetCodelistsPopupInnerHTML = 294,

        GenerateCSVFile = 295,
        BindIndicatorCodelist = 296,
        BindUnitCodelist = 297,
        BindAreaCodelist = 298,
        BindAgeCodelist = 299,
        BindSexCodelist = 300,
        BindLocationCodelist = 301,
        GetLangFromDB = 302,
        //Added To check Gallery existence
        GalleryExistence = 1000,
        SessionReset = 1002,
        GetSearchedKeywords = 1003,
        GetAllAdaptations = 1004,
        GetAllUsers = 1005,
        UpdatePassword = 1006,
        RegenrateActivationLink = 1007,
        RegenrateForgotPasswordLink = 1008,
        CheckGlobalAdaptation = 1009,
        GetGlobalAdaptationURL = 1010,
        GetCurentAdaptationsUser = 1011,
        SetUserAsAdmin = 1012,
        CheckIfMasterAccount = 1013,
        CreateAnEntryIntoGlobalCatalog = 1014,
        ImportLanguageXML = 1015,
        GenerateLogsForOptimiseDB = 1016,
        GetDataDescription = 1017,
        EditCMSContent = 1018,
        AddCMSContent = 1019,
        CheckIsDI7ORGAdaptation = 1020,
        GenerateSiteMap = 1021,
        AddCMSContents = 1022,
        EditArticle = 1023,
        GetArticlesByMenuCategoryAndTags = 1024,
        GetTagsListByMenuCategory = 1025,
        DeleteArticlebyContentId = 1026,
        ShowHideArticlebyContentId = 1027,
        GetMapServerURL = 1028,
        GetArticlesMenuCatListJson = 1029,
        DeleteMenuCategory = 1030,
        AddMenuCategory = 1031,
        EditMenuCategory = 1032,
        MoveUpNDownMenuCat = 1033,
        UpdateThemeLegends = 1034,
        InstallPatch=1035,
        UpdateLanguageFiles=1036,
        UpdateAppSettingFile=1037,
        GetArticleByContentId=1039,
        ChangeDatabasePassword = 1040,
        BindMASchemeAndDFD=1041,
        BindProvisionAgreements=1042,
        ExportMappingExcel = 1043,
        BindDataAndMetadataRegistration = 1044,
        GetHeaderDetail = 1045,
        SaveHeaderDetails = 1046,
        ImportMappingExcel=1047,
        SaveSDMXDataSelection=1048,
        SaveSDMXMetadataSelection = 1049,
        BindAreaLevels=1050

    }

    #endregion "--Variables--"

    #region "--Methods--"

    private void HandleCallBack()
    {
        string RetVal;
        string RequestParam;
        string RequestParam2;
        string RequestParam3;
        string RequestParam4;
        string RequestParam5;
        string RequestParam6;
        string RequestParam7;
        CallbackType OCallbackType;
        Callback OCallback;

        RetVal = string.Empty;
        RequestParam = string.Empty;
        RequestParam2 = string.Empty;
        RequestParam3 = string.Empty;
        RequestParam4 = string.Empty;
        RequestParam5 = string.Empty;
        RequestParam6 = string.Empty;
        RequestParam7 = string.Empty;
        OCallback = null;
        OCallbackType = CallbackType.None;

        try
        {
            if (Request.Params.Count > 0)
            {
                OCallbackType = (CallbackType)int.Parse(Request[Constants.RequestHeaderParamNames.CallBack].ToString());
                RequestParam = Request[Constants.RequestHeaderParamNames.Param1];
                RequestParam2 = Request[Constants.RequestHeaderParamNames.Param2];
                RequestParam3 = Request[Constants.RequestHeaderParamNames.Param3];
                RequestParam4 = Request[Constants.RequestHeaderParamNames.Param4];
                RequestParam5 = Request[Constants.RequestHeaderParamNames.Param5];

                RequestParam6 = Request[Constants.RequestHeaderParamNames.Param6];
                RequestParam7 = Request[Constants.RequestHeaderParamNames.Param7];
                OCallback = new Callback(this.Page);

                //get browser type to handle internet explorer 8 issue of ajax 32 bit size [reurn Image url in this case]
                if (!string.IsNullOrEmpty(Request.QueryString["bType"]))
                {
                    OCallback.BrowserType = Convert.ToString(Request.QueryString["bType"]).Trim();
                }

                switch (OCallbackType)
                {
                    case CallbackType.GetQdsResultsForAdaptation:
                        RetVal = OCallback.GetQdsResultsForAdaptation(RequestParam);
                        break;
                    case CallbackType.GenerateCacheResults:
                        RetVal = OCallback.GenerateCacheResults(RequestParam);
                        break;
                    case CallbackType.GetDataViewDataNidsMulipleDbs:
                        RetVal = OCallback.GetDataViewDataNidsMulipleDbs(RequestParam);
                        break;
                    case CallbackType.GetLangDBcode:
                        RetVal = OCallback.GetLangDBcode(RequestParam);
                        break;
                    case CallbackType.GetOatStringFromFileUpload:
                        RetVal = OCallback.getOATfromFile(RequestParam);
                        break;
                    case CallbackType.GetIndWhereDataExists:
                        RetVal = OCallback.GetIndWhereDataExists(RequestParam);
                        break;
                    case CallbackType.GetAreaWhereDataExists:
                        RetVal = OCallback.GetAreaWhereDataExists(RequestParam);
                        break;
                    case CallbackType.GetDataView:
                        RetVal = OCallback.GetDataView(RequestParam);
                        break;
                    case CallbackType.GetQDSResults:
                        if (Session["IsMapServer"] != null)
                        {
                            Session.Remove("IsMapServer");
                        }
                        RetVal = OCallback.GetQDSResults(RequestParam);
                        break;
                    case CallbackType.GetDataViewDataNids:
                        RetVal = OCallback.GetDataViewDataNids(RequestParam);
                        break;
                    case CallbackType.GetGalleryThumbnails:
                        RetVal = OCallback.GetGalleryThumbnails(RequestParam);
                        break;
                    case CallbackType.GetAreas:
                        RetVal = OCallback.GetAreas(RequestParam);
                        break;
                    case CallbackType.GetSGs:
                        RetVal = OCallback.GetSGs(RequestParam);
                        break;
                    case CallbackType.GetSources:
                        RetVal = OCallback.GetSources(RequestParam);
                        break;
                    case CallbackType.GetTPs:
                        RetVal = OCallback.GetTPs(RequestParam);
                        break;
                    case CallbackType.GetASDatabases:
                        RetVal = OCallback.GetASDatabases(RequestParam);
                        break;
                    case CallbackType.GetASResults:
                        RetVal = OCallback.GetASResults(RequestParam);
                        break;
                    case CallbackType.isHTMLfilePrepared:
                        RetVal = OCallback.WriteHTMLtoFile(RequestParam, "");
                        break;
                    case CallbackType.sharedPresentionUrl:
                        RetVal = OCallback.sharedPresentation(RequestParam, RequestParam2, RequestParam3, RequestParam4);
                        break;
                    case CallbackType.chartInputData:
                        RetVal = OCallback.getChartData(RequestParam, RequestParam2);
                        break;
                    case CallbackType.ShareByEmail:
                        RetVal = OCallback.ShareByEmail(RequestParam);
                        break;
                    case CallbackType.ShareSearchResult:
                        RetVal = OCallback.ShareSearchResult(RequestParam);
                        break;
                    case CallbackType.AdminUpdateConfiguration:
                        RetVal = OCallback.AdminUpdateConfiguration(RequestParam);
                        break;
                    case CallbackType.GetCloudContent:
                        RetVal = OCallback.GetCloudContent(RequestParam);
                        break;
                    case CallbackType.GetTransformedCodelist:
                        RetVal = OCallback.GetTransformedCodelist(RequestParam);
                        break;
                    case CallbackType.GetDPArtefact:
                        RetVal = OCallback.GetDPArtefact(RequestParam);
                        break;
                    case CallbackType.AdminGetAvlConnections:
                        RetVal = OCallback.AdminGetAvlConnections(RequestParam);
                        break;
                    case CallbackType.AdminGetConnectionDetailsData:
                        RetVal = OCallback.AdminGetConnectionDetailsData(RequestParam);
                        break;
                    case CallbackType.AdminUpdateDbConnection:
                        RetVal = OCallback.AdminUpdateDbConnection(RequestParam);
                        break;
                    case CallbackType.GetViewDataChildDivHtml:
                        RetVal = OCallback.GetViewDataChildDivHtml(RequestParam);
                        break;
                    case CallbackType.GetSelectedDatabasesDetails:
                        RetVal = OCallback.GetSelectedDatabasesDetails(RequestParam);
                        break;
                    case CallbackType.GetSelectedArea:
                        RetVal = OCallback.GetSelectedArea(RequestParam);
                        break;
                    case CallbackType.AddSubscription:
                        RetVal = OCallback.AddSubscription(RequestParam);
                        break;
                    case CallbackType.GetDefaultAreaCountOfSelDb:
                        RetVal = OCallback.GetDefaultAreaCountOfSelDb(RequestParam);
                        break;
                    case CallbackType.Gallery_GetQDSResults:
                        RetVal = OCallback.Gallery_GetQDSResults(RequestParam);
                        break;
                    case CallbackType.Gallery_GetASResults:
                        RetVal = OCallback.Gallery_GetASResults(RequestParam);
                        break;
                    case CallbackType.Gallery_ShareSearchResult:
                        RetVal = OCallback.Gallery_ShareSearchResult(RequestParam);
                        break;
                    case CallbackType.GetSubscriptionDetails:
                        RetVal = OCallback.GetSubscriptionDetails(RequestParam);
                        break;
                    case CallbackType.SaveGalleryItem:
                        RetVal = OCallback.SaveGalleryItem(RequestParam);
                        break;
                    case CallbackType.AdminTestConnection:
                        RetVal = OCallback.AdminTestConnection(RequestParam);
                        break;
                    case CallbackType.AdminDeleteConnection:
                        RetVal = OCallback.AdminDeleteConnection(RequestParam);
                        break;
                    case CallbackType.AdminRegisterDatabase:
                        RetVal = OCallback.AdminRegisterDatabase(RequestParam);
                        break;
                    case CallbackType.AdminGetAllDbConnections:
                        RetVal = OCallback.AdminGetAllDbConnections(RequestParam);
                        break;
                    case CallbackType.AdminXMLGeneration:
                        RetVal = OCallback.AdminXMLGeneration(RequestParam);
                        break;
                    case CallbackType.AdminSaveDefaultArea:
                        RetVal = OCallback.AdminSaveDefaultArea(RequestParam);
                        break;
                    case CallbackType.NGallery_GetQDSResults:
                        RetVal = OCallback.NGallery_GetQDSResults(RequestParam);
                        break;
                    case CallbackType.NGallery_GetASResults:
                        RetVal = OCallback.NGallery_GetASResults(RequestParam);
                        break;
                    case CallbackType.NGallery_GetMoreKeywords:
                        RetVal = OCallback.NGallery_GetMoreKeywords(RequestParam);
                        break;
                    case CallbackType.GetICTypes:
                        RetVal = OCallback.GetICTypes(RequestParam);
                        break;
                    case CallbackType.GetCategoryScheme:
                        RetVal = OCallback.GetCategoryScheme(RequestParam);
                        break;
                    case CallbackType.AdminAddNewLanguage:
                        RetVal = OCallback.AdminAddNewLanguage(RequestParam);
                        break;
                    case CallbackType.AdminSDMXArtefactsGeneration:
                        RetVal = OCallback.AdminSDMXArtefactsGeneration(RequestParam);
                        break;
                    case CallbackType.RegisterUser:
                        RetVal = OCallback.RegisterUser(RequestParam);
                        break;
                    case CallbackType.LoginUser:
                        RetVal = OCallback.LoginUser(RequestParam, false);
                        break;
                    case CallbackType.AdminGetAllLanguageList:
                        RetVal = OCallback.AdminGetAllLanguageList();
                        break;
                    case CallbackType.AdminDeleteLanguage:
                        RetVal = OCallback.AdminDeleteLanguage(RequestParam);
                        break;
                    case CallbackType.GenerateAllPagesXML:
                        RetVal = OCallback.GenerateAllPagesXML();
                        break;
                    case CallbackType.AddRegistration:
                        RetVal = OCallback.AddRegistration(RequestParam);
                        break;
                    case CallbackType.GetRegistrationsSummary:
                        RetVal = OCallback.GetRegistrationsSummary(RequestParam);
                        break;
                    case CallbackType.GetRegistrationDetails:
                        RetVal = OCallback.GetRegistrationDetails(RequestParam);
                        break;
                    case CallbackType.UpdateRegistration:
                        RetVal = OCallback.UpdateRegistration(RequestParam);
                        break;
                    case CallbackType.DeleteRegistration:
                        RetVal = OCallback.DeleteRegistration(RequestParam);
                        break;
                    case CallbackType.SendNotifications:
                        RetVal = OCallback.SendNotifications(RequestParam);
                        break;
                    case CallbackType.GetLoggedInUserSubscriptions:
                        RetVal = OCallback.GetLoggedInUserSubscriptions(RequestParam);
                        break;
                    case CallbackType.UpdateSubscription:
                        RetVal = OCallback.UpdateSubscription(RequestParam);
                        break;
                    case CallbackType.DeleteSubscription:
                        RetVal = OCallback.DeleteSubscription(RequestParam);
                        break;
                    case CallbackType.GetEmailIdOfLoggedInUser:
                        RetVal = OCallback.GetEmailIdOfLoggedInUser(RequestParam);
                        break;
                    case CallbackType.BindDPSchemeAndDFD:
                        RetVal = OCallback.BindDPSchemeAndDFD(RequestParam);
                        break;
                    case CallbackType.BindDataProviders:
                        RetVal = OCallback.BindDataProviders(RequestParam);
                        break;
                    case CallbackType.GetUserDetails:
                        RetVal = OCallback.GetUserDetails(RequestParam);
                        break;
                    case CallbackType.UpdateUserDetails:
                        RetVal = OCallback.UpdateUserDetails(RequestParam);
                        break;
                    case CallbackType.ForgotPassword:
                        RetVal = OCallback.ForgotPassword(RequestParam);
                        break;
                    case CallbackType.ValidateSDMXML:
                        RetVal = OCallback.ValidateSDMXML(RequestParam);
                        break;
                    case CallbackType.ValidateDSD:
                        RetVal = OCallback.ValidateDSD(RequestParam);
                        break;
                    case CallbackType.GetUnmatchlist:
                        this.SetIsMapServerValueInSession(RequestParam7);
                        this.CheckNCreateIsMapServerColumnInMDBDatabase();
                        RetVal = OCallback.GetUnmatchList(RequestParam, RequestParam2, RequestParam3, RequestParam4, RequestParam5, RequestParam6);
                        break;
                    case CallbackType.AdminMapFilesGeneration:
                        RetVal = OCallback.AdminMapFilesGeneration(RequestParam);
                        break;
                    case CallbackType.AdminDBScriptGeneration:
                        RetVal = OCallback.AdminDBScriptGeneration(RequestParam);
                        break;
                    case CallbackType.GetDataQueryRequestSOAP:
                        RetVal = OCallback.GetDataQueryRequestSOAP(RequestParam);
                        break;
                    case CallbackType.GetDataQueryResponseSDMXML:
                        RetVal = OCallback.GetDataQueryResponseSDMXML(RequestParam);
                        break;
                    case CallbackType.CompareUserDSDAgainstDevInfoDSD:
                        RetVal = OCallback.CompareUserDSDAgainstDevInfoDSD(RequestParam);
                        break;
                    case CallbackType.CompareCodelists:
                        RetVal = OCallback.CompareCodelists(RequestParam);
                        break;
                    case CallbackType.CompareUserDSD1AgainstUserDSD2:
                        RetVal = OCallback.CompareUserDSD1AgainstUserDSD2(RequestParam);
                        break;
                    case CallbackType.InitiaizeMap:
                        bool usingMapServer = false;//Convert.ToBoolean(isMapServer.Trim());
                        if (Session["IsMapServer"] != null)
                        {
                            usingMapServer = Convert.ToBoolean(Session["IsMapServer"]);
                        }
                        RetVal = OCallback.InitiaizeMap(RequestParam, RequestParam2, RequestParam3, RequestParam4, RequestParam5, usingMapServer.ToString());
                        break;
                    case CallbackType.ZoomIn:
                        RetVal = OCallback.ZoomIn();
                        break;
                    case CallbackType.ZoomOut:
                        RetVal = OCallback.ZoomOut();
                        break;
                    case CallbackType.ZoomToRectangle:
                        RetVal = OCallback.ZoomToRectangle(RequestParam, RequestParam2, RequestParam3, RequestParam4);
                        break;
                    case CallbackType.Pan:
                        RetVal = OCallback.Pan(RequestParam, RequestParam2, RequestParam3, RequestParam4);
                        break;
                    case CallbackType.FullExtent:
                        RetVal = OCallback.FullExtent();
                        break;
                    case CallbackType.Label:
                        RetVal = OCallback.Label(RequestParam);
                        break;
                    case CallbackType.Border:
                        RetVal = OCallback.Border(RequestParam);
                        break;
                    case CallbackType.NorthSymbol:
                        RetVal = OCallback.NorthSymbol(RequestParam);
                        break;
                    case CallbackType.Scale:
                        RetVal = OCallback.Scale(RequestParam);
                        break;
                    case CallbackType.Resize:
                        RetVal = OCallback.Resize(RequestParam, RequestParam2);
                        break;
                    case CallbackType.GetThemeInfo:
                        RetVal = OCallback.GetThemeInfo(RequestParam);
                        break;
                    case CallbackType.ResetLegendColor:
                        RetVal = OCallback.ResetLegendColor(RequestParam, Convert.ToInt32(RequestParam2), RequestParam3);
                        break;
                    case CallbackType.GetSeriesInfo:
                        RetVal = OCallback.GetSeriesInfo();
                        break;
                    case CallbackType.EditThemeInfo:
                        RetVal = OCallback.EditThemeInfo(RequestParam, RequestParam2, RequestParam3, RequestParam4, RequestParam5, RequestParam6);
                        break;
                    case CallbackType.GetTitleInfo:
                        RetVal = OCallback.GetTitleInfo();
                        break;
                    case CallbackType.GetDisclaimerInfo:
                        RetVal = OCallback.GetDisclaimerInfo();
                        break;
                    case CallbackType.GetDataLabelInfo:
                        RetVal = OCallback.GetDataLabelInfo(Convert.ToInt32(RequestParam));
                        break;
                    case CallbackType.SetTitleInfo:
                        RetVal = OCallback.SetTitleInfo(RequestParam);
                        break;
                    case CallbackType.SetDisclaimerInfo:
                        RetVal = OCallback.SetDisclaimerInfo(RequestParam);
                        break;
                    case CallbackType.SetDataLabelInfo:
                        RetVal = OCallback.SetDataLabelInfo(RequestParam, Convert.ToBoolean(RequestParam2), Convert.ToInt32(RequestParam3));
                        break;
                    case CallbackType.SetLegendSeriesColor:
                        RetVal = OCallback.SetLegendSeriesColor(RequestParam, RequestParam2, RequestParam3);
                        break;
                    case CallbackType.SetBorderStyle:
                        RetVal = OCallback.SetBorderStyle(RequestParam, RequestParam2, RequestParam3, RequestParam4);
                        break;
                    case CallbackType.GetBorderStyle:
                        RetVal = OCallback.GetBorderStyle();
                        break;
                    case CallbackType.SetDataLabels:
                        RetVal = OCallback.SetDataLabelsJSON(RequestParam);
                        break;
                    case CallbackType.GetDataLabels:
                        RetVal = OCallback.GetDataLabelsJSON();
                        break;
                    case CallbackType.RefreshImage:
                        RetVal = OCallback.RefreshImage();
                        break;
                    case CallbackType.GetMapImageURL:
                        RetVal = OCallback.GetMapImageURL();
                        break;
                    case CallbackType.AddTheme:
                        RetVal = OCallback.AddTheme(RequestParam);
                        break;
                    case CallbackType.SetThemeVisiblity:
                        RetVal = OCallback.SetThemeVisiblity(RequestParam, RequestParam2);
                        break;
                    case CallbackType.DeleteTheme:
                        RetVal = OCallback.DeleteTheme(RequestParam);
                        break;
                    case CallbackType.UpdateTheme:
                        RetVal = OCallback.UpdateTheme(RequestParam, RequestParam2, RequestParam3, RequestParam4);
                        break;
                    case CallbackType.UpdateThemeLegends:
                        RetVal = OCallback.UpdateThemeLegends(RequestParam, RequestParam2, RequestParam3, RequestParam4);
                        break;
                    case CallbackType.GetLegendRangesByThemeChange:
                        RetVal = OCallback.GetLegendRangesByThemeChange(RequestParam, RequestParam2);
                        break;
                    case CallbackType.UpdateDotDensityTheme:
                        RetVal = OCallback.UpdateDotDensityTheme(RequestParam, RequestParam2);
                        break;
                    case CallbackType.GetUpdatedLegendRanges:
                        RetVal = OCallback.GetUpdatedLegendRanges(RequestParam, Convert.ToInt32(RequestParam2), RequestParam3);
                        break;
                    case CallbackType.ChangeHatchPatern:
                        RetVal = OCallback.ChangeHatchPatern(RequestParam, RequestParam2, RequestParam3);
                        break;
                    case CallbackType.UpdateLegendRange:
                        RetVal = OCallback.UpdateLegendRange(RequestParam, RequestParam2);
                        break;
                    case CallbackType.Smooth:
                        RetVal = OCallback.Smooth(RequestParam, RequestParam2, RequestParam3);
                        break;
                    case CallbackType.UpdateChartSettings:
                        RetVal = OCallback.UpdateChartSettings(RequestParam, RequestParam2, RequestParam3);
                        break;
                    case CallbackType.UpdateChartLegendSettings:
                        RetVal = OCallback.UpdateChartLegendSettings(RequestParam, RequestParam2, RequestParam3, RequestParam4);
                        break;
                    case CallbackType.SetDisputedBoundriesVisiblity:
                        RetVal = OCallback.SetDisputedBoundriesVisiblity(RequestParam);
                        break;
                    case CallbackType.GetLayerTaggingInformation:
                        RetVal = OCallback.GetTaggingInformation();
                        break;
                    case CallbackType.GetAreaInformation:
                        RetVal = OCallback.GetAreaInfo(RequestParam);
                        break;
                    case CallbackType.SetLegendInfo:
                        RetVal = OCallback.SetLegendInfo(RequestParam, RequestParam2);
                        break;
                    case CallbackType.GetLegendInfo:
                        RetVal = OCallback.GetLegendInfo(RequestParam);
                        break;
                    case CallbackType.Nudging:
                        RetVal = OCallback.Nudging(RequestParam, RequestParam2);
                        break;
                    case CallbackType.GetKMZ:
                        RetVal = OCallback.GetKMZ();
                        break;
                    case CallbackType.InitiaizeGoogleMap:
                        RetVal = OCallback.InitiaizeGoogleMap(RequestParam, RequestParam2, RequestParam3, RequestParam4);
                        break;
                    case CallbackType.GetFeatureLayerInfo:
                        RetVal = OCallback.GetFeatureLayerInfo();
                        break;
                    case CallbackType.GetBaseLayerInfo:
                        RetVal = OCallback.GetBaseLayerInfo();
                        break;
                    case CallbackType.SetFeatureLayerInfo:
                        RetVal = OCallback.SetFeatureLayerInfo(RequestParam, RequestParam2);
                        break;
                    case CallbackType.SetBaseLayerInfo:
                        RetVal = OCallback.SetBaseLayerInfo(RequestParam, RequestParam2);
                        break;
                    case CallbackType.UploadLegends:
                        RetVal = OCallback.UploadLegends(RequestParam, RequestParam2);
                        break;
                    case CallbackType.SetDisputedBoundryVisible:
                        RetVal = OCallback.SetDisputedBoundryVisible(RequestParam);
                        break;
                    case CallbackType.ShowDisputedBoundryDefault:
                        RetVal = OCallback.ShowDisputedBoundryDefault();
                        break;
                    case CallbackType.GetTimeperiodOfSeries:
                        RetVal = OCallback.GetTimeperiodOfSeries(RequestParam, RequestParam2);
                        break;
                    case CallbackType.GetChartLegendOpacity:
                        RetVal = OCallback.GetChartLegendOpacity(RequestParam, RequestParam2);
                        break;
                    case CallbackType.ResetAllTheme:
                        RetVal = OCallback.ResetAllTheme(RequestParam, RequestParam2);
                        break;
                    case CallbackType.GetThemeCount:
                        RetVal = OCallback.GetThemeCount();
                        break;
                    case CallbackType.DeleteFile:
                        RetVal = OCallback.DeleteFile(RequestParam);
                        break;
                    case CallbackType.GetTimePeriods:
                        RetVal = OCallback.GetTimePeriods(RequestParam);
                        break;
                    case CallbackType.GetDataQueryResponseJSON:
                        RetVal = OCallback.GetDataQueryResponseJSON(RequestParam);
                        break;
                    case CallbackType.GetDataQueryResponseXML:
                        RetVal = OCallback.GetDataQueryResponseXML(RequestParam);
                        break;
                    case CallbackType.GenerateComparisonReport:
                        RetVal = OCallback.GenerateComparisonReport(RequestParam);
                        break;
                    case CallbackType.AdminSDMXMLGeneration:
                        RetVal = OCallback.AdminSDMXMLGeneration(RequestParam);
                        break;
                    case CallbackType.AdminSDMXMLRegistration:
                        RetVal = OCallback.AdminSDMXMLRegistration(RequestParam);
                        break;
                    case CallbackType.GetDataQueryRequestHTTP:
                        RetVal = OCallback.GetDataQueryRequestHTTP(RequestParam);
                        break;
                    case CallbackType.GetDataQueryRequestREST:
                        RetVal = OCallback.GetDataQueryRequestREST(RequestParam);
                        break;
                    case CallbackType.AdminRegisterDatabaseForUploadedDSD:
                        RetVal = OCallback.AdminRegisterDatabaseForUploadedDSD(RequestParam);
                        break;
                    case CallbackType.AdminMetadataGeneration:
                        RetVal = OCallback.AdminMetadataGeneration(RequestParam);
                        break;
                    case CallbackType.GetMSDList:
                        RetVal = OCallback.GetMSDList(RequestParam);
                        break;
                    case CallbackType.GetMSDAttributes:
                        RetVal = OCallback.GetMSDAttributes(RequestParam);
                        break;
                    case CallbackType.AdminSaveDefaultIndcator:
                        RetVal = OCallback.AdminSaveDefaultIndcator(RequestParam);
                        break;
                    case CallbackType.GetMFDList:
                        RetVal = OCallback.GetMFDList(RequestParam);
                        break;
                    case CallbackType.GetDFDAndMFDList:
                        RetVal = OCallback.GetDFDAndMFDList(RequestParam);
                        break;
                    case CallbackType.AdminGetAllTheUploadedDSDsTable:
                        RetVal = OCallback.AdminGetAllTheUploadedDSDsTable(RequestParam);
                        break;
                    case CallbackType.AdminUpdateDatabaseForUploadedDSD:
                        RetVal = OCallback.AdminUpdateDatabaseForUploadedDSD(RequestParam);
                        break;
                    case CallbackType.IsUserAdmin:
                        if (OCallback.isUserAdmin(RequestParam))
                            RetVal = "true";
                        else
                            RetVal = "false";
                        break;
                    case CallbackType.AdminLoginUser:
                        RetVal = OCallback.AdminLoginUser(RequestParam);
                        break;
                    case CallbackType.GetAllUsersGridHTML:
                        RetVal = OCallback.GetAllUsersGridHTML();
                        break;
                    case CallbackType.DeleteUser:
                        RetVal = OCallback.DeleteUser(RequestParam);
                        break;
                    case CallbackType.DeletePresentation:
                        RetVal = OCallback.deletePresentation(RequestParam, RequestParam2);
                        break;
                    case CallbackType.CheckIfUploadedDSD:
                        RetVal = OCallback.CheckIfUploadedDSD(RequestParam);
                        break;
                    case CallbackType.StoreMappingInfo:
                        RetVal = OCallback.storeMappingDetails(RequestParam, RequestParam2);
                        break;
                    case CallbackType.GettingMappingInfo:
                        RetVal = OCallback.getMappingDetails(RequestParam, RequestParam2, RequestParam3, RequestParam4);
                        break;
                    case CallbackType.AdminGetAllTheUploadedDSDsList:
                        RetVal = OCallback.AdminGetAllTheUploadedDSDsList();
                        break;
                    case CallbackType.AdminGetAllTheUploadedMSDsTable:
                        RetVal = OCallback.AdminGetAllTheUploadedMSDsTable();
                        break;
                    case CallbackType.AdminMetadataRegistration:
                        RetVal = OCallback.AdminMetadataRegistration(RequestParam);
                        break;
                    case CallbackType.AdminCheckIfDSDAlreadyUploaded:
                        RetVal = OCallback.AdminCheckIfDSDAlreadyUploaded(RequestParam);
                        break;
                    case CallbackType.AdminGetAllTheDatabasesTable:
                        RetVal = OCallback.AdminGetAllTheDatabasesTable(RequestParam);
                        break;
                    case CallbackType.AdminGetAllTheDatabaseList:
                        RetVal = OCallback.AdminGetAllTheDatabaseList();
                        break;
                    case CallbackType.DataProviderGetTPDivInnerHTML:
                        RetVal = OCallback.DataProviderGetTPDivInnerHTML(RequestParam);
                        break;
                    case CallbackType.DataProviderGetAreaDivInnerHTML:
                        RetVal = OCallback.DataProviderGetAreaDivInnerHTML(RequestParam);
                        break;
                    case CallbackType.DataProviderGetIndicatorDivInnerHTML:
                        RetVal = OCallback.DataProviderGetIndicatorDivInnerHTML(RequestParam);
                        break;
                    case CallbackType.DataProviderGetDatabaseDivInnerHTML:
                        RetVal = OCallback.DataProviderGetDatabaseDivInnerHTML(RequestParam);
                        break;
                    case CallbackType.DataProviderGetDSDDivInnerHTML:
                        RetVal = OCallback.DataProviderGetDSDDivInnerHTML(RequestParam);
                        break;
                    case CallbackType.DataProviderGenerateSDMXML:
                        RetVal = OCallback.DataProviderGenerateSDMXML(RequestParam);
                        break;
                    case CallbackType.DataProviderRegisterSDMXML:
                        RetVal = OCallback.DataProviderRegisterSDMXML(RequestParam);
                        break;
                    case CallbackType.DataProviderGenerateMetadata:
                        RetVal = OCallback.DataProviderGenerateMetadata(RequestParam);
                        break;
                    case CallbackType.DataProviderRegisterMetadata:
                        RetVal = OCallback.DataProviderRegisterMetadata(RequestParam);
                        break;
                    case CallbackType.ValidateMetadataReport:
                        RetVal = OCallback.ValidateMetadataReport(RequestParam);
                        break;
                    case CallbackType.GetDataForGenerateApiCall:
                        RetVal = OCallback.GetDataForGenerateApiCall(RequestParam);
                        break;
                    case CallbackType.GettingMatchingAreas:
                        this.SetIsMapServerValueInSession(RequestParam7);
                        RetVal = OCallback.getMatchingArea(RequestParam, RequestParam2, RequestParam3, RequestParam4);
                        break;
                    case CallbackType.GetDataQuerySources:
                        RetVal = OCallback.GetDataQuerySources(RequestParam);
                        break;
                    case CallbackType.DataProviderGetSourceDivInnerHTML:
                        RetVal = OCallback.DataProviderGetSourceDivInnerHTML(RequestParam);
                        break;
                    case CallbackType.GetJsonM49CountriesFile:
                        RetVal = OCallback.GetJsonM49CountriesFile();
                        break;
                    case CallbackType.AdminSaveAdaptation:
                        RetVal = OCallback.AdminSaveAdaptation(RequestParam);
                        break;
                    case CallbackType.GetJsonAdaptations:
                        RetVal = OCallback.GetJsonAdaptationFile();
                        break;
                    case CallbackType.BindHelpText:
                        RetVal = OCallback.BindHelpText(RequestParam);
                        break;
                    case CallbackType.GetCatalogCacheResults:
                        RetVal = OCallback.GetCatalogCacheResults(RequestParam);
                        break;
                    case CallbackType.BindCodelistMappingLists:
                        RetVal = OCallback.BindCodelistMappingLists(RequestParam);
                        break;
                    case CallbackType.GenerateCodelistMappingXml:
                        RetVal = OCallback.GenerateCodelistMappingXml(RequestParam);
                        break;
                    case CallbackType.CheckIfAdminLoggedIn:
                        RetVal = OCallback.CheckIfAdminLoggedIn(RequestParam);
                        break;
                    case CallbackType.IsAdminRegister:
                        RetVal = OCallback.IsAdminRegistered();
                        break;
                    case CallbackType.GetAllUsersHTMLForAdmin:
                        RetVal = OCallback.GetAllUsersHTMLForAdmin();
                        break;
                    case CallbackType.UpdateUsers:
                        RetVal = OCallback.UpdateUsers(RequestParam);
                        break;
                    case CallbackType.AdminGetLanguageGridHTML:
                        RetVal = OCallback.AdminGetLanguageGridHTML(RequestParam);
                        break;
                    case CallbackType.AdminSaveLanguageChanges:
                        RetVal = OCallback.AdminSaveLanguageChanges(RequestParam);
                        break;
                    case CallbackType.AdminLogout:
                        RetVal = OCallback.AdminLogout();
                        break;
                    case CallbackType.GetIndicativeIdAgencyIdVersionValues:
                        RetVal = OCallback.GetIndicativeIdAgencyIdVersionValues(RequestParam);
                        break;
                    case CallbackType.AdminDeleteCatalog:
                        RetVal = OCallback.AdminDeleteCatalog(RequestParam);
                        break;
                    case CallbackType.AdminUpdateAdaptation:
                        RetVal = OCallback.AdminUpdateAdaptation(RequestParam);
                        break;
                    case CallbackType.GetDisclaimerText:
                        RetVal = OCallback.GetDisclaimerText();
                        break;
                    case CallbackType.SetChartTimePeriodsVisiblity:
                        RetVal = OCallback.SetChartTimePeriodsVisiblity(RequestParam, RequestParam2);
                        break;
                    case CallbackType.GetChartTimePeriods:
                        RetVal = OCallback.GetChartTimePeriods(RequestParam);
                        break;
                    case CallbackType.GetSeriesThemeName:
                        RetVal = OCallback.GetSeriesThemeName(RequestParam);
                        break;
                    case CallbackType.RequestAdminForDataProviderRights:
                        RetVal = OCallback.RequestAdminForDataProviderRights(RequestParam);
                        break;
                    case CallbackType.AdminGetAdaptationVersions:
                        RetVal = OCallback.AdminGetAdaptationVersions();
                        break;
                    //case CallbackType.ViewContent:
                    //    RetVal = OCallback.ViewContent(RequestParam);
                    //    break;
                    case CallbackType.AccountActivation:
                        RetVal = OCallback.ActiveAccount(RequestParam);
                        break;
                    case CallbackType.ChangePassword:
                        RetVal = OCallback.ChangePassword(RequestParam);
                        break;
                    case CallbackType.GenerateMetadataMappingXml:
                        RetVal = OCallback.GenerateMetadataMappingXml(RequestParam);
                        break;
                    case CallbackType.BindMetadataMappingLists:
                        RetVal = OCallback.BindMetadataMappingLists(RequestParam);
                        break;
                    case CallbackType.BindIUSMappingList:
                        RetVal = OCallback.BindIUSMappingList(RequestParam);
                        break;
                    case CallbackType.BindIndicatorCodelist:
                        RetVal = OCallback.BindIndicatorCodelist(RequestParam);
                        break;
                    case CallbackType.BindUnitCodelist:
                        RetVal = OCallback.BindUnitCodelist(RequestParam);
                        break;
                    case CallbackType.BindAreaCodelist:
                        RetVal = OCallback.BindAreaCodelist(RequestParam);
                        break;
                    //case CallbackType.BindAgeCodelist:
                    //    RetVal = OCallback.BindAgeCodelist(RequestParam);
                    //    break;
                    //case CallbackType.BindSexCodelist:
                    //    RetVal = OCallback.BindSexCodelist(RequestParam);
                    //    break;
                    //case CallbackType.BindLocationCodelist:
                    //    RetVal = OCallback.BindLocationCodelist(RequestParam);
                    //    break;
                    case CallbackType.GetLangFromDB:
                        RetVal = Global.GetLangFromDB();
                        break;
                    //Added to check gallery existence
                    case CallbackType.GalleryExistence:
                        RetVal = OCallback.GalleryExistence(RequestParam).ToString();
                        break;
                    case CallbackType.GenerateIUSMappingXml:
                        RetVal = OCallback.GenerateIUSMappingXml(RequestParam);
                        break;
                    case CallbackType.BindRegistrations:
                        RetVal = OCallback.BindRegistrations(RequestParam);
                        break;
                    case CallbackType.GetDefaultDb:
                        RetVal = OCallback.GetDefaultDb();
                        break;
                    case CallbackType.SessionReset:
                        RetVal = OCallback.SessionReset().ToString();
                        break;
                    case CallbackType.GetSearchedKeywords:
                        RetVal = OCallback.GetSearchedKeywords(RequestParam).ToString();
                        break;
                    case CallbackType.GetAllAdaptations:
                        RetVal = OCallback.GetAllAdaptations();
                        break;
                    case CallbackType.GetAllUsers:
                        RetVal = OCallback.GetAllUsers(RequestParam);
                        break;
                    case CallbackType.UpdatePassword:
                        RetVal = OCallback.UpdatePassword(RequestParam);
                        break;
                    case CallbackType.GetCodelistsPopupInnerHTML:
                        RetVal = OCallback.GetCodelistsPopupInnerHTML(RequestParam);
                        break;
                    case CallbackType.GenerateCSVFile:
                        RetVal = OCallback.GenerateCsvFile();
                        break;
                    case CallbackType.RegenrateActivationLink:
                        RetVal = OCallback.RegenerateActivationLink(RequestParam);
                        break;
                    case CallbackType.RegenrateForgotPasswordLink:
                        RetVal = OCallback.RegenrateForgotPasswordLink(RequestParam);
                        break;
                    case CallbackType.CheckGlobalAdaptation:
                        RetVal = OCallback.CheckGlobalAdaptation();
                        break;
                    case CallbackType.CheckIsDI7ORGAdaptation:
                        RetVal = OCallback.CheckIsDI7ORGAdaptation();
                        break;
                    case CallbackType.GetGlobalAdaptationURL:
                        RetVal = OCallback.GetGlobalAdaptationURL();
                        break;
                    case CallbackType.GetCurentAdaptationsUser:
                        RetVal = OCallback.GetCurentAdaptationsUser(RequestParam);
                        break;
                    case CallbackType.SetUserAsAdmin:
                        RetVal = OCallback.SetUserAsAdmin(RequestParam);
                        break;
                    case CallbackType.CheckIfMasterAccount:
                        RetVal = Global.CheckIfMasterAccount(RequestParam);
                        break;
                    case CallbackType.CreateAnEntryIntoGlobalCatalog:
                        RetVal = OCallback.EntryIntoGlobalCatalog(RequestParam);
                        break;
                    case CallbackType.ImportLanguageXML:
                        RetVal = OCallback.ExportLanguageXML(RequestParam);
                        break;
                    case CallbackType.GenerateLogsForOptimiseDB:
                        RetVal = OCallback.GenerateLogsForOptimiseDB(RequestParam);
                        break;
                    case CallbackType.GetDataDescription:
                        RetVal = OCallback.GetDataDescription(RequestParam);
                        break;
                    case CallbackType.EditCMSContent:
                        RetVal = OCallback.EditCMSContent(RequestParam);
                        break;
                    case CallbackType.AddCMSContent:
                        RetVal = OCallback.AddCMSContent(RequestParam);
                        break;
                    case CallbackType.AddCMSContents:
                        RetVal = OCallback.AddCMSContents(RequestParam);
                        break;
                    case CallbackType.GenerateSiteMap:
                        RetVal = OCallback.GenerateSiteMap(RequestParam);
                        break;
                    case CallbackType.GetMapServerURL:
                        RetVal = Global.GetMapServerURL(RequestParam);
                        break;
                    case CallbackType.EditArticle:
                        RetVal = OCallback.EditArticle(RequestParam);
                        break;
                    case CallbackType.GetArticlesByMenuCategoryAndTags:
                        RetVal = OCallback.GetArticlesByMenuCategoryAndTags(RequestParam);
                        break;
                    case CallbackType.GetTagsListByMenuCategory:
                        RetVal = OCallback.GetTagsListByMenuCategory(RequestParam);
                        break;
                    case CallbackType.DeleteArticlebyContentId:
                        RetVal = OCallback.DeleteArticlebyContentId(RequestParam);
                        break;
                    case CallbackType.ShowHideArticlebyContentId:
                        RetVal = OCallback.ShowHideArticlebyContentId(RequestParam);
                        break;
                    case CallbackType.GetArticlesMenuCatListJson:
                        RetVal = OCallback.GetArticlesMenucategoriesListJson(RequestParam);
                        break;
                    case CallbackType.DeleteMenuCategory:
                        RetVal = OCallback.DeleteMenuCategory(RequestParam);
                        break;
                    case CallbackType.AddMenuCategory:
                        RetVal = OCallback.AddMenuCategory(RequestParam);
                        break;
                    case CallbackType.EditMenuCategory:
                        RetVal = OCallback.EditMenuCategory(RequestParam);
                        break;
                    case CallbackType.MoveUpNDownMenuCat:
                        RetVal = OCallback.MoveUpNDownMenuCat(RequestParam);
                        break;
                    case CallbackType.InstallPatch:
                        RetVal = OCallback.InstallPatch();
                        break;
                    case CallbackType.UpdateLanguageFiles:
                        RetVal = OCallback.UpdateLanguageFiles();
                        break;
                    case CallbackType.UpdateAppSettingFile:
                        RetVal = OCallback.UpdateAppSettingFile();
                        break;
                    case CallbackType.GetArticleByContentId:
                        RetVal = OCallback.GetArticleByContentIdForSharing(RequestParam);
                        break;
                    case CallbackType.ChangeDatabasePassword:
                        RetVal = OCallback.ChangeDatabasePassword(RequestParam);
                        break;     
                    case CallbackType.BindMASchemeAndDFD:
                        RetVal = OCallback.BindMASchemeAndDFD(RequestParam);
                        break;  
                    case CallbackType.BindProvisionAgreements:
                        RetVal = OCallback.BindProvisionAgreements(RequestParam);
                        break;
                    case CallbackType.BindDataAndMetadataRegistration:
                        RetVal = OCallback.BindDataAndMetadataRegistration(RequestParam);
                        break;
                    case CallbackType.ExportMappingExcel:
                        RetVal = OCallback.ExportMappingExcel(RequestParam);
                        break;
                    case CallbackType.GetHeaderDetail:
                        RetVal = OCallback.GetHeaderDetail(RequestParam);
                        break;
                    case CallbackType.SaveHeaderDetails:
                        RetVal = OCallback.SaveHeaderDetails(RequestParam);
                        break;
                    case CallbackType.ImportMappingExcel:
                        RetVal = OCallback.ImportMappingExcel(RequestParam);
                        break;
                    case CallbackType.SaveSDMXDataSelection:
                        RetVal = OCallback.SaveSDMXDataSelection(RequestParam);
                        break;
                    case CallbackType.SaveSDMXMetadataSelection:
                        RetVal = OCallback.SaveSDMXMetadataSelection(RequestParam);
                        break;
                    case CallbackType.BindAreaLevels:
                        RetVal = OCallback.BindAreaLevels(RequestParam);
                        break;
                    default:
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            Response.Write(RetVal);
        }
    }


    /* this method Checks whether the user has selected the Use Map server Checkbox
    * and sets its value in session*/
    private void SetIsMapServerValueInSession(string isMapServer)
    {
        if (!string.IsNullOrEmpty(isMapServer.Trim()))
        {
            bool usingMapServer = Convert.ToBoolean(isMapServer.Trim());
            if (Session["IsMapServer"] != null)
            {
                Session["IsMapServer"] = usingMapServer;
            }
            else
            {
                Session.Add("IsMapServer", usingMapServer);
            }
        }

    }

    /// <summary>
    /// Check and create the IsMapServer column in Databases.mdb database
    /// </summary>
    private void CheckNCreateIsMapServerColumnInMDBDatabase()
    {
        DIConnection dIConnection = null;
        try
        {
            dIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("../../stock/Database.mdb"),
                             string.Empty, string.Empty);
            try
            {
                DataTable table = dIConnection.GetConnection().GetSchema("Columns");
                var v = from r in table.AsEnumerable() where (r["TABLE_NAME"].ToString() == "MappingInformation" && r["COLUMN_NAME"].ToString() == "mapserver_used") select r;
                if (v.Count() == 0)
                {
                    dIConnection.AddColumn("mappinginformation", "mapserver_used", "Text", "False");
                }
            }
            catch
            {
            }
        }
        catch (Exception ex)
        {
            Global.WriteErrorsInLogFolder("error in Registering new database");
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
            if (dIConnection != null)
            {
                dIConnection.Dispose();
                dIConnection = null;
            }
        }
    }

    #endregion "--Methods--"

    #endregion "--Private--"

    #region "--Public--"

    #region "--Methods--"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            this.HandleCallBack();
        }
        catch (Exception)
        {
            
        }
        
    }

    #endregion "--Methods--"

    #endregion "--Public--"
}
