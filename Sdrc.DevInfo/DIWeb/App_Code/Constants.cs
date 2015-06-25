using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;



/// <summary>
/// Summary description for Constants
/// </summary>
public static class Constants
{
    public static string EncryptionKey = "<\"}#$7#%";

    public const string LanguageKeyValueTag = "root/Row";

    public static class RequestHeaderParamNames
    {
        public const string CallBack = "callback";
        public const string Param1 = "param1";
        public const string Param2 = "param2";
        public const string Param3 = "param3";
        public const string Param4 = "param4";
        public const string Param5 = "param5";
        public const string Param6 = "param6";
        public const string Param7 = "param7";
    }

    public static class Delimiters
    {
        public const string ParamDelimiter = "[****]";
        public const string PivotColumnDelimiter = "|";
        public const string PivotRowDelimiter = "#";

        public const string ValuesDelimiter = "~~";
        public const string SingleQuote = "'";
        public const string Comma = ",";

        public const string RowDelimiter = "[**[R]**]";
        public const string ColumnDelimiter = "[**[C]**]";
        public const string StartDelimiter = "[[**start**]]";
        public const string EndDelimiter = "[[**end**]]";

        public const string GalleryParamDelimeterMain = "[^^^^]";
        public const string GalleryParamDelimeterSub = "[::::]";
        public const string IndGUIDSeoarator = "@__@";
        public const string Underscore = "_";
    }

    public static class AppSettingKeys
    {
        //Application
        public const string adaptation_name = "adaptation_name";
        public const string adapted_for = "adapted_for";
        public const string area_nid = "area_nid";
        public const string sub_nation = "sub_nation";
        public const string default_lng = "default_lng";
        public const string show_db_selection = "show_db_selection";
        public const string show_sliders = "show_sliders";
        public const string slider_count = "visible_sliders_count";
        public const string enableDSDSelection = "enableDSDSelection";
        public const string isQdsGeneratedForDensedQS_Areas = "isQdsGeneratedForDensedQS_Areas";
        public const string showdisputed = "showdisputed";
        public const string adaptation_mode = "standalone_registry";
        public const string js_version = "js_version";
        public const string registryAreaLevel = "registryAreaLevel";
        public const string registryMSDAreaId = "registryMSDAreaId";
        public const string registryNotifyViaEmail = "registryNotifyViaEmail";
        public const string VisibleSlidersCount = "visible_sliders_count";
        public const string adaptation_year = "adaptation_year";
        public const string unicef_region = "unicef_region";
        public const string isDesktopVersionAvailable = "is_desktop_version_available";
        public const string app_version = "app_version";

        //Database Administrator
        public const string ContactDbAdmName = "contact_db_adm_name";
        public const string ContactDbAdmInstitution = "contact_db_adm_institution";
        public const string ContactDbAdmEmail = "contact_db_adm_email";

        //Web Components
        public const string diuilib_version = "diuilib_version";
        public const string diuilib_url = "diuilib_url";
        public const string diuilib_theme_css = "diuilib_theme_css";

        //Share
        public const string fbAppID = "fbAppID";
        public const string fbAppSecret = "fbAppSecret";
        public const string twAppID = "twAppID";
        public const string twAppSecret = "twAppSecret";
        public const string dvMrdThreshold = "dvMrdThreshold";
        public const string dvHideSource = "dvHideSource";
        public const string enableQDSGallery = "enableQDSGallery";
        public const string googleapikey = "googleapikey";
        public const string GUID = "GUID";

        public const string standalone_registry = "standalone_registry";
        public const string registryMappingAgeDefaultValue = "registryMappingAgeDefaultValue";
        public const string registryMappingSexDefaultValue = "registryMappingSexDefaultValue";
        public const string registryMappingLocationDefaultValue = "registryMappingLocationDefaultValue";
        public const string registryMappingFrequencyDefaultValue = "registryMappingFrequencyDefaultValue";
        public const string registryMappingSourceTypeDefaultValue = "registryMappingSourceTypeDefaultValue";
        public const string registryMappingNatureDefaultValue = "registryMappingNatureDefaultValue";
        public const string registryMappingUnitMultDefaultValue = "registryMappingUnitMultDefaultValue";
        public const string registryPagingRows = "registryPagingRows";

        public const string customParams = "customParams";
        public const string GalleryPageSize = "GalleryPageSize";

        public const string isInnovationsMenuVisible = "isInnovationsMenuVisible";
        public const string isNewsMenuVisible = "isNewsMenuVisible";
        public const string isContactUsMenuVisible = "isContactUsMenuVisible";
        public const string isSupportMenuVisible = "isSupportMenuVisible";

        public const string show_cloud = "show_cloud";
        public const string DownloadCSV = "DownloadCSV";
        public const string AdapUserPageSize = "AdapUserPageSize";
        public const string sliders_list = "sliders_list";
        public const string isWorkingOnIP = "isWorkingOnIP";

        public const string isDownloadsLinkVisible = "isDownloadsLinkVisible";
        public const string isTrainingLinkVisible = "isTrainingLinkVisible";
        public const string isMapLibraryLinkVisible = "isMapLibraryLinkVisible";
        public const string islangFAQVisible = "islangFAQVisible";
        public const string HowToVideoData = "HowToVideoData";
        public const string islangKBVisible = "islangKBVisible";


        public const string isRSSFeedsLinkVisible = "isRSSFeedsLinkVisible";
        public const string isDiWorldWideLinkVisible = "isDiWorldWideLinkVisible";
        public const string isSitemapVisible = "isSitemapVisible";
        public const string isHowToVideoVisible = "isHowToVideoVisible";
        public const string SliderAnimationSpeed = "SliderAnimationSpeed";
        public const string gaUniqueID = "gaUniqueID";
        public const string Country = "Country";

        public const string CategoryScheme = "CategoryScheme";
        public const string isShowMapServer = "isShowMapServer";
        public const string DropNCreateSitemapURLTable = "DropNCreateSitemapURLTable";
        public const string ShowWebmasterAccount = "ShowWebmasterAccount";

        public const string CmsDbCreationFile = "CmsDbCreationFile";
        public const string CmsTableDefinition = "CmsTableDefinition";
        public const string CmsDataBasePath = "CmsDataBasePath";
        public const string MaxArticlesCount = "MaxArticlesCount";

        public const string MetaTag_Desc = "MetaTag_Desc";
        public const string MetaTag_Kw = "MetaTag_Kw";
        public const string IsSDMXHeaderCreated = "IsSDMXHeaderCreated";
        public const string IsSDMXDataPublished = "IsSDMXDataPublished";
        public const string IsSDMXDataPublishedCountryData = "IsSDMXDataPublishedCountryData";
    }

    public static class FolderName
    {
        public const string MasterKeyVals = "stock\\Language\\MasterKeyVals\\";
        public const string PageKeysMappings = "stock\\Language\\PageKeysMappings\\";
        public const string Language = "stock\\Language\\";
        public const string Template = "stock\\templates\\";
        public const string Data = "stock\\data\\";
        public const string Adaptation = "stock\\Adaptation\\";
        public const string AdaptationLogoImages = "stock\\AdaptationLogoImages\\";
        public const string AdaptationSliderHTML = "stock\\Adaptation\\slider_html\\";
        public const string EmailTemplates = "stock\\templates\\EmailTemplates\\";
        public const string LanguageXMLFiles = "stock\\LanguageXML\\";
        public const string TemporaryXLS = "stock\\LanguageXML\\TempXLSFile";
        public const string LanguageTemplate = "stock\\templates\\LanguageXMLTemplate";
        public const string LanguageBackUp = "stock\\Language\\LanguageBackUp";
        public const string TempMappingExcelFiles = "stock\\tempMappingFiles";
        /// <summary>
        /// stock\\map\\DIB\\
        /// </summary>
        public const string DisputedBoundries = "stock\\map\\DIB\\";
        public const string Users = "stock\\Users\\";
        public const string DataFiles = "ds\\";
        public const string Maps = "maps\\";
        public const string TempCYV = "stock\\tempCYV\\";
        public const string ShareMap = "stock\\shared\\map\\";
        public const string ErrorLogs = "stock\\ErrorLogs\\";
        public const string CSVLogPath = "stock\\CSVLogs";

        public const string CmsTemplateFolder = "libraries\\aspx\\diorg\\Template\\";
        public const string Diorg_news_content = "libraries\\aspx\\diorg\\news_content\\";
        public const string Diorg_facts_content = "libraries\\aspx\\diorg\\facts\\";
        public const string Diorg_action_content = "libraries\\aspx\\diorg\\devinfo_in_action\\";
        public const string DiorgContent = "libraries\\aspx\\diorg\\";

        public static class SDMX
        {
            public const string sdmx = "sdmx\\";
            public const string Categories = "sdmx\\Categories\\";
            public const string Categorisations = "sdmx\\Categorisations\\";
            public const string Codelists = "sdmx\\Codelists\\";
            public const string Concepts = "sdmx\\Concepts\\";
            public const string Metadata = "sdmx\\Metadata\\";
            public const string AreaMetadata = "sdmx\\Metadata\\Area\\";
            public const string IndicatorMetadata = "sdmx\\Metadata\\Indicator\\";
            public const string SourceMetadata = "sdmx\\Metadata\\Source\\";
            public const string MSD = "sdmx\\MSD\\";
            public const string ProvisioningMetadata = "sdmx\\Provisioning Metadata\\";
            public const string Constraints = "sdmx\\Constraints\\";
            public const string PAs = "sdmx\\Provisioning Metadata\\PAs\\";
            public const string Registrations = "sdmx\\Registrations\\";
            public const string Subscriptions = "sdmx\\Subscriptions\\";
            public const string SDMX_ML = "sdmx\\SDMX-ML\\";
            public const string Mapping = "sdmx\\Mapping\\";
            public const string RegistryServicePath = "libraries/ws/RegistryService.asmx";
            public const string DataServicePath = "libraries/ws/DataServices.asmx";
        }



        public static class Codelists
        {
            public const string IUS = "ius";
            public const string IC = "ic";
            public const string IC_IUS = "ic_ius";
            public const string Area = "area";
            public const string TP = "tp";
            public const string Metadata = "metadata";
            public const string Footnotes = "footnotes";
        }
    }

    public static class FileName
    {
        public const string CatalogAdaptationsTemplate = "stock\\shared\\adaptations\\CatalogPage.htm";
        public const string MapServerResponseTime = "serverResponseTime.txt";
        public const string ForgotPassword = "ForgotPassword.txt";//"forgotpassword.txt";
        public const string UpdatePassword = "ChangePasswordUserSite.txt";//"updatepassword.txt";
        public const string UpdatePasswordAdmin = "ChangePasswordAdminPanel.txt";//"updatepassword.txt";
        public const string Registration = "registration.txt";
        public const string Activation = "UserRegistration.txt";//"activationlink.txt";
        public const string AdminRegistration = "AdminRegistration.txt";//"adminregistration.txt";
        public const string DataProviderRegistration = "DataProviderRightConfirmation.txt";//"dataproviderregistration.txt";
        public const string DataConsumerRegistration = "DataConsumerRightConfirmation.txt";//"dataconsumerregistration.txt";
        public const string RequestDataProviderRights = "EmailToWebMasterForNewRegistration.txt";
        public const string SubscriptionSDMX = "Subscription_SDMX.txt";
        public const string RegistrationData_MetaDataSDMX = "Register_Data_MetaData.txt";
        public const string RegistrationData_DeletedSDMX = "Registration_Deleted.txt";
        public const string CatalogNewEntry = "CatalogNewEntry.txt";
        public const string CatalogUpdatedEntry = "CatalogUpdatedEntry.txt";
        public const string XLSLogFileName = "Log_DI7_Web_AdminPanel_LogFile";
        public const string LanguageXMLTemplate = "Template_Language.xml";
        public const string CmsNewContentPage_Template = "NewContentPage_Template.html";
        public const string CmsNewLinkContent_Template = "NewLink_Template.html";
        public const string SDMXLogFileName = "Log_DI7_Web_SDMXRegistry_LogFile";
        public const string SitemapHtmlPage = "stock\\sitemap.htm";
        public const string HeaderFileName = "Header.xml";


    }

    public static class PatchConstaints
    {
        public const string PatchLanguageXML = "stock\\Patch\\PatchLanguageXML.xml";
        public const string PatchAppSetting = "stock\\Patch\\PatchAppSetting.xml";
        public const string PatchInstLogFileName = "DI7_PatchUpdate_LogFile";
        public const string PatchInstalMessages = "stock\\Patch\\PatchInstalMessages.xml";
        public const string PatchFolderPath = "stock\\Patch\\";
    }

    public static class WebConfigKey
    {
        public const string LanguageFile = "LanguageFile";
        public const string AppSettingFile = "AppSettingFile";
        public const string BaseLanguageFile = "BaseLanguageFile";
        public const string DBConnectionsFile = "DBConnectionsFile";
        public const string DestinationLanguageFile = "DestinationLanguageFile";
        public const string TableDefinitionsFile = "TableDefinitionsFile";
        public const string DbScriptFile = "DbScriptFile";
        public const string DbScriptsExecutionFile = "DbScriptsExecutionFile";
        public const string DbScriptsCreationFile = "DbScriptsCreationFile";
        public const string DbTableIndexes = "DbTableIndexes";
        public const string IsGlobalAllow = "IsGlobalAllow";
        public const string AppVersionFile = "AppVersionFile";
        public const string DiWorldWide4 = "DiWorldWide4";
        public const string CmsDbCreationFile = "CmsDbCreationFile";
        public const string CmsTableDefinition = "CmsTableDefinition";
        public const string CmsDataBasePath = "CmsDataBasePath";
        public const string DropNCreateSitemapURLTable = "DropNCreateSitemapURLTable";
        public const string MaxArticlesCount = "MaxArticlesCount";
    }

    public static class XmlFile
    {
        public static class AppSettings
        {
            public static class Tags
            {
                public const string Root = "appsettings";
                public const string Item = "item";

                public static class ItemAttributes
                {
                    public const string Name = "n";
                    public const string Value = "v";
                }
            }
        }

        public static class Db
        {
            public static class Tags
            {
                public const string Root = "dbinfo";
                public const string Category = "category";
                public const string Database = "db";

                public static class RootAttributes
                {
                    public const string Default = "def";
                    public const string DefaultDSD = "defDSD";
                }

                public static class CategoryAttributes
                {
                    public const string Name = "name";
                }

                public static class DatabaseAttributes
                {
                    public const string Id = "id";
                    public const string Name = "n";
                    public const string SDMXDb = "sdmxdb";
                    public const string DSDId = "dsdid";
                    public const string DSDVersion = "dsdversion";
                    public const string DSDAgencyId = "dsdagencyid";
                    public const string AssosciatedDb = "assosciateddb";
                    public const string Count = "count";
                    public const string DefaultLanguage = "deflng";
                    public const string DefaultIndicator = "defind";
                    public const string DefaultIndicatorJSON = "defindjson";
                    public const string DefaultArea = "defarea";
                    public const string DefaultAreaJSON = "defareajson";
                    public const string DefaultAreaCount = "defac";
                    public const string DatabaseConnection = "dbConn";
                    public const string AvailableLanguage = "avllng";
                    public const string LastModified = "lastmod";
                    public const string DescriptionEnglish = "desc_en";
                    public const string DescriptionFrench = "desc_fr";
                    public const string LanguageCodeCSVFiles = "langcode_csvfiles";
                    public const string DefaultDSD = "defaultDSD";
                    public const string IsSDMXHeaderCreated = "IsSDMXHeaderCreated";
                }
            }
        }

        public static class Language
        {
            public static class Tags
            {
                public const string Root = "lnginfo";
                public const string Language = "lng";

                public static class RootAttributes
                {
                    public const string Default = "def";
                }

                public static class LanguageAttributes
                {
                    public const string Code = "code";
                    public const string Name = "n";
                    public const string RTLDirection = "rtl";
                }
            }
        }

        public static class MasterLanguage
        {
            public static class Tags
            {
                public const string Root = "root";
                public const string Row = "Row";

                public static class RowAttributes
                {
                    public const string Key = "key";
                    public const string Value = "value";
                }
            }
        }

        public static class DestinationLanguage
        {
            public static class Tags
            {
                public const string Root = "DevInfo7";
                public const string Destination = "Destination";
                public const string LanguageName = "LANGUAGE_NAME";
                public const string LanguageCode = "LANGUAGE_CODE";
                public const string PageDirection = "PAGEDIRECTION";
                public const string FileVersion = "FILE_VERSION";
            }
        }
    }

    public static class SDMXValidationMessages
    {
        public const string ValidXML = "Valid XML";
        public const string ValidSDMXFile = "Valid SDMX File";
        public const string ValidDimensions = "Valid Dimensions";
        public const string ValidCodesForDimensions = "Valid Codes For Dimensions";
        public const string ValidMandatoryDimensions = "Valid Mandatory Dimensions";
        public const string ValidAllDimensions = "Valid All Dimensions";
        public const string ValidAttributes = "Valid Attributes";
        public const string ValidPrimaryMeasure = "Valid Primary Measure";
        public const string InvalidWebService = "Invalid Web Service Url/GetStructureSpecificTimeSeriesData method does not exists.";
        public const string ValidMFDSelection = "Valid MFD Selection";
        public const string ValidReferredMSD = "Valid Referred MSD";
        public const string ValidMetadataReport = "Valid Metadata Report";
        public const string ValidMetadataTarget = "Valid Metadata Target";
        public const string ValidMetadataTargetObjectReference = "Valid Metadata Target Object Reference";
        public const string ValidReportStructure = "Valid Report Structure";
        public const string ValidXMLMessage = "It is s valid XML file.";
        public const string ValidSDMXFileMessage = "It is a valid SDMX File compliant to SDMX Schemas.";
        public const string ValidDimensionsMessage = "The Dimensions specified in the the SDMX-ML File are valid according to the Master DSD.";
        public const string ValidCodesForDimensionsMessage = "The Codes for the Dimensions specified in the the SDMX-ML File are valid according to their codelists.";
        public const string ValidMandatoryDimensionsMessage = "The File contains all the mandatory dimensions according to the Master DSD.";
        public const string ValidAllDimensionsMessage = "All Dimensions specified in the DSD file are valid according to the Master DSD.";
        public const string ValidAttributesMessage = "All Attributes specified in the DSD file are valid according to the Master DSD.";
        public const string ValidPrimaryMeasureMessage = "Primary Measure specified in the DSD file is valid according to the Master DSD.";
        public const string ValidReferredMSDMessage = "The Metadata Structure Definition(MSD) referred in the Metadata Report File exists.";
        public const string ValidMetadataReportMessage = "The Report specified in the Metadata Report File is valid.";
        public const string ValidMetadataTargetMessage = "The Target specified in the Metadata Report File is valid.";
        public const string ValidMetadataTargetObjectReferenceMessage = "The Target Object Reference specified in the Metadata Report File is valid.";
        public const string ValidReportStructureMessage = "The Report Structure i.e. the Reported Attributes specified in the Metadata Report File is valid.";
          
    }

    public static class CookieName
    {
        public const string DBNId = "hdbnid";
        public const string LanguageCode = "hlngcode";
        public const string DBLanguageCode = "hlngcodedb";
        public const string ShowSliders = "ShowSliders";
        public const string DSDNID = "hdsdnid";
        public const string UserNId = "usrnid";
    }

    public static class Pages
    {
        public static class Registry
        {
            public const string StructuralMetadata = "Structures";
            public const string StructuralMetadataMetadata = "Structural Metadata-Metadata";
            public const string Subscription = "Subscription";
            public const string PublishData = "Publish Data";
            public const string ProvisioningMetadata = "Provisioning Metadata";
            public const string Validate = "Validate";
            public const string Compare = "Compare";
            public const string DataQuery = "Data Query";
            public const string WebServiceDemo = "Web Service Demonstration";
            public const string Discover = "Discover Data/Metadata";
            public const string Mapping = "Mapping";
            public const string MaintenanceAgency = "Maintenance Agency";
            public const string Upload = "Upload";
            public const string DatabaseManagement = "Database Management";
            public const string DataProvider = "Data Provider";
            public const string DiscoverRegistrations = "Data";
        }
    }

    public static class WSQueryStrings
    {
        public const string p = "p";
        public const string ResponseFormat = "ResponseFormat";
        public const string SDMXFormat = "SDMXFormat";
        public const string Language = "Language";
        public const string Title = "Title";
        public const string FootNote = "Fnote";
        public const string GroupByIndicator = "GbyI";
        public const string UserLoginService = "UserLoginInformation.asmx";
        public const string CatalogWebService = "Catalog.asmx";
        public const string UtilityWebService = "Utility.asmx";
        public const string AdaptationUserWebService = "AdaptationUsers.asmx";
        public static class ResponseFormatTypes
        {
            public const string SDMX = "SDMX";
            public const string JSON = "JSON";
            public const string XML = "XML";
            public const string TABLE = "TABLE";
        }

        public static class SDMXFormatTypes
        {
            public const string Generic = "Generic";
            public const string GenericTS = "GenericTS";
            public const string StructureSpecific = "StructureSpecific";
            public const string StructureSpecificTS = "StructureSpecificTS";
        }

        public static class SDMXContentTypes
        {
            public const string Generic = "application/vnd.sdmx.genericdata+xml;version=2.1";
            public const string GenericTS = "application/vnd.sdmx.generictimeseriesdata+xml;version=2.1";
            public const string StructureSpecific = "application/vnd.sdmx.structurespecificdata+xml;version=2.1";
            public const string StructureSpecificTS = "application/vnd.sdmx.structurespecifictimeseriesdata+xml;version=2.1";
        }
    }

    public static class XLSDownloadType
    {
        public const string SvgRasterizerFolder = "libraries\\svg_rasterizer";
        public const string BatikRasterizerJarFile = "batik-rasterizer.jar";
        public const string TempFolderLocation = "../../stock/tempcyv";
        public const string TypeText = "type";
        public const string FilenameText = "filename";
        public const string DefaultFilename = "chart";
        public const string SvgText = "svg";
        public const string DownloadType = "application/vnd.xls";
        public const string SwfImage = "image/png";
        public const string ImageType = "-m image/png";
        public const string XlsExtention = ".xls";
        public const string SvgExtention = ".svg";
        public const string PngExtention = ".png";
        public const string ExcelTemplateFilePath = "stock\\templates\\HighChartExcelDownload.xls";
    }

    public static class Map
    {
        /// <summary>
        /// map.xml
        /// </summary>
        public static string MapLanguageFileName = "map.xml";
        /// <summary>
        /// XXXX
        /// </summary>
        public static string LangReplaceXPathString = "XXXX";
        /// <summary>
        /// /Language/lng[@id='XXXX']
        /// </summary>
        public static string MapLangXPath = "/Language/lng[@id='" + LangReplaceXPathString + "']";
        /// <summary>
        /// val
        /// </summary>
        public static string XMLValueAtributeName = "val";
    }

    public static class SessionNames
    {
        public const string LoggedAdminUser = "LoggedAdminUser";
        public const string LoggedAdminUserNId = "LoggedAdminUserNId";
        public const string CurrentArticlePageNo = "CurrentArticlePageNo";
        public const string CurrentArticleTagIds = "CurrentArticleTagIds";
        public const string CurrentArticleMenuCategory = "CurrentArticleMenuCategory";
    }

    public static class DBVersion
    {
        public const string DI7_0_0_0 = "7.0.0.0";
        public const string VersionsChangedDatesDI7_0_0_0 = "9 Feb 2012";
        public const string VersionCommentsDI7_0_0_0 = "DI 7 related chnages made, Metadata_Category table modified & new columns added in Data,Timeperiod,Area and IndicatorClassification table";
    }

    /// <summary>
    /// Constants used for SDMX artefacts' generation if SDMXUtility.SDMXFormat is set to UNSD.
    /// </summary>
    public static class UNSD
    {
        /// <summary>
        /// CodeList Constants.
        /// </summary>
        public static class CodeList
        {
            public static class Age
            {
                public const string Id = "CL_AGE_GROUP_COUNTRY_DATA";
                public const string AgencyId = "UNSD";
                public const string Version = "0.1";
                public const string Name = "Age group code list";
                public const string Description = null;
                public const string FileName = "Age.xml";
                public const string Gid = "AGE";
                public const string Default_Value = "NA";
            }

            public static class Frequency
            {
                public const string Id = "CL_FREQ_MDG";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Units code list";
                public const string Description = null;
                public const string FileName = "Frequency.xml";
                public const string Default_Value = "A";
            }

            public static class Location
            {
                public const string Id = "CL_LOCATION_MDG";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Location code list";
                public const string Description = null;
                public const string FileName = "Location.xml";
                public const string Gid = "LOCATION";
                public const string Default_Value = "T";
            }

            public static class Nature
            {
                public const string Id = "CL_NATURE_MDG";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Nature code list";
                public const string Description = null;
                public const string FileName = "Nature.xml";
                public const string Default_Value = "NA";
            }

            public static class Area
            {
                public const string Id = "CL_REF_AREA_MDG";
                public const string AgencyId = "IAEG";
                public const string Version = "2.0";
                public const string Name = "Reference area code list";
                public const string Description = null;
                public const string FileName = "Area.xml";
                public const string MetadataFolderName = "Area";
            }

            public static class Indicator
            {
                public const string Id = "CL_SERIES_COUNTRY_DATA";
                public const string AgencyId = "UNSD";
                public const string Version = "0.2";
                public const string Name = "Series code list";
                public const string Description = null;
                public const string FileName = "Indicator.xml";
                public const string MetadataFolderName = "Indicator";
            }

            public static class Sex
            {
                public const string Id = "CL_SEX_MDG";
                public const string AgencyId = "IAEG";
                public const string Version = "2.0";
                public const string Name = "Sex code list";
                public const string Description = null;
                public const string FileName = "Sex.xml";
                public const string Gid = "SEX";
                public const string Default_Value = "NA";
            }

            public static class SourceType
            {
                public const string Id = "CL_SOURCE_TYPE_MDG";
                public const string AgencyId = "IAEG";
                public const string Version = "2.0";
                public const string Name = "Source Type Code List";
                public const string Description = null;
                public const string FileName = "SourceType.xml";
                public const string Default_Value = "NA";
            }

            public static class Unit
            {
                public const string Id = "CL_UNIT_COUNTRY_DATA";
                public const string AgencyId = "UNSD";
                public const string Version = "0.3";
                public const string Name = "Units code list";
                public const string Description = null;
                public const string FileName = "Unit.xml";
            }

            public static class UnitMult
            {
                public const string Id = "CL_UNIT_MULT_SDMX";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Units multipliers code list";
                public const string Description = null;
                public const string FileName = "UnitMult.xml";
                public const string Default_Value = "0";
            }

            public static class ICType
            {
                public const string Id = "CL_ICTYPE";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "IndicatorClassificationType";
                public const string Description = "List of IndicatorClassificationTypes";
                public const string FileName = "ICType.xml";
            }

            public static class IndicatorClassification
            {
                public const string Id = "CL_INDICATORCLASSIFICATION";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "Indicator Classification";
                public const string Description = "List of Indicator Classifications";
                public const string FileName = "IndicatorClassification.xml";
                public const string MetadataFolderName = "Source";
            }

            public static class Subgroup
            {
                public const string Id = "CL_SUBGROUP";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "Subgroup";
                public const string Description = "List of Subgroups";
                public const string FileName = "Subgroup.xml";
            }

            public static class SubgroupType
            {
                public const string Id = "CL_SUBGROUPTYPES";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "SubgroupType";
                public const string Description = "List of SubgroupTypes";
                public const string FileName = "SubgroupType.xml";
            }

            public static class SubgroupVal
            {
                public const string Id = "CL_SUBGROUPVALUES";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "Subgroup Value";
                public const string Description = "List of Subgroup Values";
                public const string FileName = "SubgroupVal.xml";
            }

            public static class IC_IUS
            {
                public const string Id = "HCL_IC_IUS";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "IC-IUS";
                public const string Description = "IC - IUS Mapping";
                public const string HierarchyId = "VH_IC_IUS";
                public const string FileName = "IC-IUS.xml";

                public static class Alias
                {
                    public const string IndicatorClassification = "IndicatorClassification";
                    public const string IUS = "IUS";
                }
            }

            public static class ICT_IC
            {
                public const string Id = "HCL_ICT_IC";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "IndicatorClassificationType-IndicatorClassification";
                public const string Description = "IndicatorClassificationTypes - IndicatorClassification Mapping";
                public const string ICTAlias = "IndicatorClassificationType";
                public const string ICAlias = "IndicatorClassification";
                public const string HierarchyId = "VH_ICT_IC";
                public const string FileName = "ICT-IC.xml";

                public static class Alias
                {
                    public const string ICType = "ICType";
                    public const string IndicatorClassification = "IndicatorClassification";
                }
            }

            public static class IUS
            {
                public const string Id = "HCL_IUS";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "IUS";
                public const string Description = "Indicator - Unit - SubgroupValues Mapping";
                public const string HierarchyId = "VH_IUS";
                public const string FileName = "IUS.xml";

                public static class Alias
                {
                    public const string Indicator = "Indicator";
                    public const string Unit = "Unit";
                    public const string SubgroupVal = "SubgroupVal";
                }
            }

            public static class SGT_SG
            {
                public const string Id = "HCL_SGT_SG";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "SubgroupType-Subgroup";
                public const string Description = "SubgroupTypes - Subgroup Mapping";
                public const string HierarchyId = "VH_SGT_SG";
                public const string FileName = "SGT-SG.xml";

                public static class Alias
                {
                    public const string SubgroupType = "SubgroupType";
                    public const string Subgroup = "Subgroup";
                }
            }

            public static class SGV_SG
            {
                public const string Id = "HCL_SGV_SG";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "SubgroupValue-Subgroup";
                public const string Description = "SubgroupValues - Subgroup Mapping";
                public const string HierarchyId = "VH_SGV_SG";
                public const string FileName = "SGV-SG.xml";

                public static class Alias
                {
                    public const string SubgroupVal = "SubgroupVal";
                    public const string Subgroup = "Subgroup";
                }
            }
        }

        /// <summary>
        /// KeyFamily Constants.
        /// </summary>
        public static class KeyFamily
        {
            public const string Id = "CountryData";
            public const string AgencyId = "UNSD";
            public const string Version = "1.1";
            public const string Name = "SDMX-CountryData";
            public const string Description = null;
        }

        /// <summary>
        /// Concept Constants.
        /// </summary>
        public static class Concept
        {
            public static class MSDConcepts
            {
                public const string Id = "MDG_CONCEPTS";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Contact point in international agency";
                public const string Description = "Provide information of person (or generic e-mail addresses of contact points) to be contacted by users for additional information.";
                public const string FileName = "MDG_CONCEPTS.xml";
            }
            public static class Age
            {
                public const string Id = "AGE_GROUP";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Age group";
                public const string Description = "Age - or age range - of the individuals the observation refers to.";
            }

            public static class Footnotes
            {
                public const string Id = "FOOTNOTES";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Footnotes";
                public const string Description = "Additional information on specific aspects of each observation, such as how the observation was computed/estimated or details that could affect the comparability of this data point with others in a time series.";
            }

            public static class Frequency
            {
                public const string Id = "FREQ";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Frequency";
                public const string Description = "Indicates rate of recurrence at which observations occur (e.g. monthly, yearly, biannually, etc.).";
            }

            public static class Location
            {
                public const string Id = "LOCATION";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Location";
                public const string Description = "Refers to a disaggregation within the Reference Area the data alludes; normally National (total), Urban or Rural - although additional disaggregations are possible within an area (e.g. subUrban).";
            }

            public static class Nature
            {
                public const string Id = "NATURE";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Nature of data points";
                public const string Description = "Information on the production and dissemination of the data (e.g.: if the figure has been produced and disseminated by the country, estimated by international agencies, etc.)";
            }

            public static class Area
            {
                public const string Id = "REF_AREA";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Reference Area";
                public const string Description = "Reference Area: Specific areas (e.g. Country, Regional Grouping, etc)  the observed values refer to. Reference areas can be determined according to different criteria (e.g.: geographical, economic, etc.).";
            }

            public static class Indicator
            {
                public const string Id = "SERIES";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Series";
                public const string Description = "Additional information on specific aspects of each observation, such as how the observation was computed/estimated or details that could affect the comparability of this data point with others in a time series.";
            }

            public static class Sex
            {
                public const string Id = "SEX";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Sex";
                public const string Description = "Gender condition: male or female. This dimension applies only if data can be dissaggregated by sex. ";
            }

            public static class SourceDetail
            {
                public const string Id = "SOURCE_DETAIL";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Source details";
                public const string Description = "Information on the name and specific details of each observation's data source. Both the sources for the numerator and denominator could be specified here.";
            }

            public static class SourceType
            {
                public const string Id = "SOURCE_TYPE";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Source type";
                public const string Description = "Type of data source: survey, administrative records, census or other. Details about the source, if available, could be provided in the free-text attribute SOURCE_DETAIL.";
            }

            public static class TimeDetail
            {
                public const string Id = "TIME_DETAIL";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Time period details";
                public const string Description = "When TIME_PERIOD refers to a date range, this attribute is used to provide METADATA on the actual range the observation refers to (e.g. for period '2001-2003' TIME_PERIOD would be 2002 but the actual dates --2001-2003-- would be expressed here).";
            }

            public static class TimePeriod
            {
                public const string Id = "TIME_PERIOD";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Time period";
                public const string Description = "Reference date - or date range - the observed value refers to (usually different from the dates of data production or dissemination). For MDG data exchange it is usually expressed as a four-digit year (e.g.: 1995) so, if the observation refers to a period of time (range), details about such period should be specified in the attribute TIME_DETAIL";
            }

            public static class Unit
            {
                public const string Id = "UNIT";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Units of measurement";
                public const string Description = "Dimension by which the series are described (e.g.: percentage, USD, etc.)";
            }

            public static class UnitMult
            {
                public const string Id = "UNIT_MULT";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Unit multiplier";
                public const string Description = "Exponent in base 10 that multiplied by the observation numeric value gives the result expressed in the unit of measure.";
            }

            public static class ObsVal
            {
                public const string Id = "OBS_VALUE";
                public const string AgencyId = "IAEG";
                public const string Version = "1.0";
                public const string Name = "Observation Value";
                public const string Description = "Observation value";
            }

            public static class Subgroup
            {
                public const string Id = "SUBGROUP";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "SubgroupVal";
                public const string Description = "SubgroupVal description";
            }

            public static class Source
            {
                public const string Id = "SOURCE";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "Source";
                public const string Description = "Source description";
            }

            public static class Denominator
            {
                public const string Id = "DENOMINATOR";
                public const string AgencyId = "";
                public const string Version = "6.0";
                public const string Name = "Denominator";
                public const string Description = "Denominator description";
            }
        }
    }

    public static class MyData
    {
        public const string INDICATOR = "INDICATOR";
        public const string UNIT = "UNIT";
        public const string LOCATION = "LOCATION";
        public const string SEX = "SEX";
        public const string AGE = "AGE";
        public const string OTHER = "OTHER";
        public const string AREA = "AREA";
        public const string SOURCE = "SOURCE";
    }

    public enum Language
    {
        English,
        Spanish,
        French,
        Russian,
        Arabic,
        Chinese,
        Nepali,
    };

    public enum CreateXmlFor
    {
        SrcXmlFile,
        DestXMLFile
    };

    public static class AdminModules
    {
        public const string AppSettings = "App Settings";
        public const string DatabaseSettings = "Database Settings";
        public const string LanguageSettings = "Language Settings";
        public const string UserManagement = "User Management";
    }
    public static class CSVLogMessage
    {
        public const string UpdateConnection = "Connection Name:{0} \r\n Server/Host Name:{1} \r\n Database Name:{2} \r\n User Name:{3} \r\n Description:{4}.";
        public const string OptimizeDatabsae = "Database optimization performed for:{0}.";
        public const string GeneratedPageXMLs = "Page XMLs Generated.";
        public const string AppSettingSelectiveUpdate = "{0} values has been updated from {1} to {2}.";
        public const string LanguageDeleted = "Language {0} - Deleted.";
        public const string EditLanguage = "Language file values updated from:{0} - to: {1}.";
        public const string ImportLanguage = "Import performed from:{0} - to: {1}.";
        public const string ExportLanguage = "Export performed from:{0} - to: {1}.";
        public const string NewAdminAccountcreated = "New Admin user account has created with User Name: {0} and Email Id: {1}.";
        public const string UserSetAsAdmin = "Admin user has changed. New admin user name: {0}.";
        public const string ChangePassword = "Password changed for User Name: {0}.";
        public const string DefaultIndicatorUpdated = "Default Indicators Updated.";
        public const string DefaultAreaUpdated = "Default Areas Updated.";
        public const string CmsDatabaseCreated = "CMS database created for Adaptation {0}. Created CMS databse Name is: {1}";
    }

    public static class XLSImportMessage
    {
        public const string ImportSelectFIle = "Language file not selected.";
        public const string ErrorinReadXLSFile = "Error occured in reading XLS file";
        public const string ImportFileExtension = "Invalid file! Only MS Excel(XLS) files are supported";
        public const string LanguageNotExist = "Invalid file";
        public const string ImportFalied = "Error in importing XLS file";
        public const string ImportSuccess = " Language files imported successfully";
        public const string XMLTempNotExist = "Language template does not exist";
        public const string ErrorCreateXMLTemplate = "Error occured in creating xml template";
    }

    public static class CMS
    {
        public const string ArticleContentPage_Template = "stock\\templates\\CMSTemplate\\ContentPage_Template.html";
        public const string ArticleLink_Template = "stock\\templates\\CMSTemplate\\Link_Template.html";
        public const string DevinfoArticle_Template = "stock\\templates\\CMSTemplate\\devinfo-article.html";
        public const string AdaptaionPagingName = "-page";
    }
}
