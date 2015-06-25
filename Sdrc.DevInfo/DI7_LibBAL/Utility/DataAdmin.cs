using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;
using DevInfo.Lib.DI_LibDAL.ExceptionHandler;
using DevInfo.Lib.DI_LibBAL.ExceptionHandler;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    public class DataAdmin
    {
        /// <summary>
        ///  Defines default file names
        /// </summary>
        public static class DefaultFileNames
        {
            public const string DA_APP_SYS_FILE = "da.xml";
            public const string DA_APP_PREF_FILE = "pref.xml";
            public const string DA_APP_DBPREF_FILE = "DatabasePref.xml";
            public const string DL_LANGUAGE_CODES = "Destination_Language.xml";
            public const string DATA_ENTRY_SPREADSHEET = "DataEntry.xls";
            public const string INDICATOR_SPREADSHEET = "Indicator.xls";
            public const string AREA_SPREADSHEET = "Area.xls";
            public const string GOAL_SPREADSHEET = "Goal.xls";
            public const string CF_SPREADSHEET = "CF.xls";
            public const string CONVENTION_SPREADSHEET = "Convention.xls";
            public const string INSTITUTION_SPREADSHEET = "Institution.xls";
            public const string THEME_SPREADSHEET = "Theme.xls";
            public const string SECTOR_SPREADSHEET = "Sector.xls";
            public const string VALIDATE_DATABASE_FILE = "Validate_Template.mdb";
            public const string SPL_DATABASE_FILE = "SPL_Template.mdb";
            public const string VALIDATE_TEMP_XLSFILE = "ExportValidate.xls";
            public const string DBC_CONVERSION_FILE = "NEW_CONVERTED_DB.mdb";
            public const string IMP_MAPPING_FILE = "ImpMapping.xml";
            public const string OG_File_Name = "DI_Gallery";
            public const string DT_TEMPLATE_FILE = "New Template.tpl";
            public const string DA_TITLEBAR_ICON = "da-16x16.ico";

        }

        #region "-- Constants --"
        /// <summary>
        /// To get DATA Admin's System File
        /// </summary>
        public const string DASystemFileName = "da.xml";                     //--Data Admin Excahnge File

        /// <summary>
        /// To Get Data Admin's Preference file
        /// </summary>
        public const string DAPrefFileName = "pref.xml";                  //--Data Admin PrefFile

        #endregion

        #region "-- Variables --"

        /// <summary>    
        /// Gets the DATA ADMIN Application Path
        /// </summary>
        public static string DAApplicationPath = string.Empty;

        /// <summary>
        /// 
        /// Gets The DATA ADMIN Language Code
        /// </summary>
        public static string DAApplicationLangauge = string.Empty;

        /// <summary>
        /// 
        /// Gets the DATA Admin Langauge Code
        /// </summary>
        public static string DAAppliationLangCode = string.Empty;


        /// <summary>
        /// Gets the application font name
        /// </summary>
        public static string FontName = string.Empty;

        /// <summary>
        /// Gets the application font style
        /// </summary>
        public static int FontStyle = 0;

        /// <summary>
        /// Gets the application font size
        /// </summary>
        public static int FontSize = 8;

        /// <summary>
        /// Gets the global color
        /// </summary>
        public static Color GlobalColor = Color.Blue;

        /// <summary>
        /// Gets true if sector is visible
        /// </summary>
        public static bool IsSectorVisible = false;
        /// <summary>
        /// Gets true if goal is visible
        /// </summary>
        public static bool IsGoalVisible = false;
        /// <summary>
        /// Gets true if CF is visible
        /// </summary>
        public static bool IsCfVisible = false;

        /// <summary>
        /// Gets true if theme is visible
        /// </summary>
        public static bool IsThemeVisible = false;
        /// <summary>
        /// Gets true if source is visible
        /// </summary>
        public static bool IsSourceVisible = false;
        /// <summary>
        /// Gets true if Institiution is visible
        /// </summary>
        public static bool IsInstitiutionVisible = false;

        /// <summary>
        /// Gets true if convention is visible
        /// </summary>
        public static bool IsConventionVisible = false;


        public static string DAGalleryFolderName = string.Empty;

        public static bool ShowLanguageBasedCaption = false;
        public static bool ApplyNativeLanguageOnButton = false;
        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Loads the DA System File
        /// </summary>
        public static void LoadSystemFile()
        {

            DI5_INIFile.DI5_IniFileReader oXMLReader;     //-- obj helps to interact with xml file

            if (System.IO.File.Exists(DataAdmin.DAApplicationPath + "\\" + DataAdmin.DASystemFileName) == false)
            {
                DataAdmin.DAApplicationPath = "";
                return;
            }

            oXMLReader = new DI5_INIFile.DI5_IniFileReader(DataAdmin.DAApplicationPath + "\\" + DataAdmin.DASystemFileName);

            //-- finding the values from xml file and save to global variables
            try
            {
                // -- Folders
                DICommon.DefaultFolder.DefaultISOFldrpath = DataAdmin.DAApplicationPath + "\\" + oXMLReader.GetIniValue("locations", "templates");

                DICommon.CountryISoCodeFilename = DICommon.DefaultFolder.DefaultISOFldrpath + "\\" + oXMLReader.GetIniValue("locations", "iso_code_filename");

                DICommon.DefaultFolder.DefaultDataFolder = oXMLReader.GetIniValue("locations", "def_data_folder");

                DICommon.DefaultFolder.DefaultSpreadSheetsFolder = oXMLReader.GetIniValue("locations", "def_spreadsheet_folder");

                DICommon.RegistryFilePath = Path.Combine(Path.Combine(DataAdmin.DAApplicationPath, oXMLReader.GetIniValue("locations", "registry")), oXMLReader.GetIniValue("locations", "registry_filename"));

                DataAdmin.DAGalleryFolderName = oXMLReader.GetIniValue("locations", "galleryfoldername");
                
                DICommon.DESImportBlankDataValueSymbol = oXMLReader.GetIniValue("des_import", "blank_datavalue_symbol");
                
                //DICommon.SDMXRegistryWebServiceUrl = Path.Combine(Path.Combine(DataAdmin.DAApplicationPath, oXMLReader.GetIniValue("locations", "sdmx_webservice")), oXMLReader.GetIniValue("locations", "sdmx_webservice"));
                

            }
            catch (Exception ex)
            {
                //     ShowErrorMessage ( ex );
            }
            finally
            {
                oXMLReader = null;
            }

        }

        /// <summary>
        /// Loads the Prefernces from the file
        /// </summary>
        public static void LoadPrefrences()
        {
            DI5_INIFile.DI5_IniFileReader oXMLReader;      //-- xmlReader object to read pref xml file
            try
            {
                string[] AvailableICs ;

                if ((File.Exists(DataAdmin.DAApplicationPath + @"\" + DataAdmin.DAPrefFileName)))
                {

                    oXMLReader = new DI5_INIFile.DI5_IniFileReader(DataAdmin.DAApplicationPath + @"\" + DataAdmin.DAPrefFileName);

                    //--  SET GLOBAL COLOR
                    DataAdmin.GlobalColor = ColorTranslator.FromHtml(oXMLReader.GetIniValue("general", "global_color"));

                    //-- Language
                    DataAdmin.DAApplicationLangauge = oXMLReader.GetIniValue("language", "app_lng");               //-- DI_English [en].xml, DI_Chinese[zh].xml

                    DataAdmin.DAAppliationLangCode = DAApplicationLangauge.Substring(DAApplicationLangauge.IndexOf("[") + 1);
                    // -- Retrieve the code from the language
                    DataAdmin.DAAppliationLangCode = DAAppliationLangCode.Replace("[", "");
                    DataAdmin.DAAppliationLangCode = DAAppliationLangCode.Replace("]", "");
                    DataAdmin.DAAppliationLangCode = DAAppliationLangCode.Replace(".xml", "");

                    //-- Application font 
                    DataAdmin.FontName = oXMLReader.GetIniValue("language", "fontname");
                    DataAdmin.FontStyle = Convert.ToInt32(oXMLReader.GetIniValue("language", "fontstyle"));
                    DataAdmin.FontSize = Convert.ToInt32(oXMLReader.GetIniValue("language", "fontsize"));

                    DataAdmin.IsSectorVisible = true;
                    DataAdmin.IsGoalVisible = true;
                    DataAdmin.IsCfVisible = true;
                    DataAdmin.IsThemeVisible = true;
                    DataAdmin.IsSourceVisible = true;
                    DataAdmin.IsInstitiutionVisible = true;
                    DataAdmin.IsConventionVisible = true;

                    AvailableICs = DICommon.SplitString(Convert.ToString(oXMLReader.GetIniValue("indicator", "treeviews")), ",");

                    if (AvailableICs[0] == "0")
                    {
                        DataAdmin.IsSectorVisible = false;
                    }
                    if (AvailableICs[1] == "0")
                    {
                        DataAdmin.IsGoalVisible = false;
                    }
                    if (AvailableICs[2] == "0")
                    {
                        DataAdmin.IsCfVisible = false;
                    }
                    if (AvailableICs[3] == "0")
                    {
                        DataAdmin.IsThemeVisible = false;
                    }
                    if (AvailableICs[4] == "0")
                    {
                        DataAdmin.IsSourceVisible = false;
                    }
                    if (AvailableICs[5] == "0")
                    {
                        DataAdmin.IsInstitiutionVisible = false;
                    }
                    if (AvailableICs[6] == "0")
                    {
                        DataAdmin.IsConventionVisible = false;
                    }


                    try
                    {
                        // Set Languge on Button for Chinese issue
                        DataAdmin.ShowLanguageBasedCaption = Convert.ToBoolean(oXMLReader.GetIniValue("general", "apply_nativelanguage_onbutton"));
                    }
                    catch (Exception)
                    {
                        // do nothing
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionFacade.ThrowException(ex);
            }
        }


        #endregion


    }

}
