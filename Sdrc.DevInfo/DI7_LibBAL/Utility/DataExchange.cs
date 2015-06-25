using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using System.Windows.Forms;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    public class DataExchange
    {

        #region "-- Constants --"
        /// <summary>
        /// To Get Data Exchange Pref File
        /// </summary>
        public const string DXPrefFileName = "DX_Pref.xml";               //-- data exchange pref file

        /// <summary>
        /// Database perf file name
        /// </summary>
        public const string DXDatabasePref = "DatabasePreference.xml";

        #endregion

        #region "-- Variables --"

        /// <summary>
        /// Gets the pref file name with path.
        /// </summary>
        public static string DXPrefFileNameWPath = string.Empty;

        /// <summary>
        /// Gets or sets database pref filename with path
        /// </summary>
        public static string DXDatabasePrefFileNameWPath = string.Empty;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// To Load default values required to run exchange module.
        /// </summary>
        /// <param name="commandLineArguments">Use Environment.CommandLine</param>
        /// <returns>True or False. Returns False, if DevInfo Data Admin does not exist</returns>
        public static bool LoadDefaultValuesForExchange(string CLA)
        {
            bool RetVal = true;

            //get command line arguments
            string[] CommandLineArguments = DIRegistry.GetDACommandLineArgs(CLA);        //-- for command line argument
            DataAdmin.DAApplicationPath = "C:\\devinfo\\DevInfo 7.0\\DevInfo 7.0 Data Admin";

            // if path doesnt exist then get it frm registry
            if (System.IO.Directory.Exists(DataAdmin.DAApplicationPath) == false)
            {

                if (CommandLineArguments.ToString().Length > 0)
                {
                    if (CommandLineArguments[0].Length > 0)
                    {
                        DataAdmin.DAApplicationPath = CommandLineArguments[0];
                    }

                    ////if (CommandLineArguments[1].Length > 0)
                    ////{
                    ////    DataAdmin.DAApplicationLangauge = CommandLineArguments[1];
                    ////}

                    ////if (CommandLineArguments[2].Length > 0)
                    ////{
                    ////    DataAdmin.DAAppliationLangCode = CommandLineArguments[2];
                    ////}                    
                }
            }

            try
            {
                // 3. if CLA is not given then set latest version of data admin 
                if (System.IO.Directory.Exists(DataAdmin.DAApplicationPath) == false)
                {
                    DataAdmin.DAApplicationPath = @"C:\DevInfo\DevInfo 7.0\DevInfo 7.0 Data Admin";
                }

                //if (DataAdmin.DAApplicationPath.Trim().Length == 0)
                //{                   
                if (System.IO.Directory.Exists(DataAdmin.DAApplicationPath) == false)
                {
                    DataAdmin.DAApplicationPath = "C:\\devinfo\\DevInfo 6.0 Data Admin";

                    if (System.IO.Directory.Exists(DataAdmin.DAApplicationPath) == false)
                    {
                        DataAdmin.DAApplicationPath = "C:\\devinfo\\DevInfo 5.0 Data Admin";
                    }
                }
                //}
                if (Directory.Exists(DataAdmin.DAApplicationPath))
                {

                    DataAdmin.LoadSystemFile();
                    DataAdmin.LoadPrefrences();
                    DICommon.SetDefaultFoldersPath();
                    DICommon.LangaugeFileNameWithPath = DICommon.DefaultFolder.DefaultLanguageFolder + "\\" + DataAdmin.DAApplicationLangauge + DICommon.FileExtension.XML;
                }
                else
                {
                    DataAdmin.DAApplicationPath = "";
                }
            }
            catch (Exception e1)
            {
                DataAdmin.DAApplicationPath = "";
            }

            //-- check bin\temp fldr exists or not .If not, then create it.
            if (Directory.Exists(DataAdmin.DAApplicationPath + "\\bin") == false)
            {
                Directory.CreateDirectory(DataAdmin.DAApplicationPath + "\\bin");
                Directory.CreateDirectory(DICommon.DefaultFolder.DefaultExchangeTempFolder);
            }
            if (Directory.Exists(DICommon.DefaultFolder.DefaultExchangeTempFolder) == false)
            {
                Directory.CreateDirectory(DICommon.DefaultFolder.DefaultExchangeTempFolder);
            }

            // set pref file path
            DataExchange.DXPrefFileNameWPath = DICommon.Applicationpath + "\\Bin\\" + DXPrefFileName;

            DataExchange.DXDatabasePrefFileNameWPath = DICommon.Applicationpath + "\\Bin\\" + DataExchange.DXDatabasePref;

            if (string.IsNullOrEmpty(DataAdmin.DAApplicationPath))
            {
                RetVal = false;
            }
            return RetVal;
        }
        /// <summary>
        /// Load exchange's default variables and values.
        /// </summary>
        /// <param name="applicationName"> Application name</param>
        /// <param name="applicationPath">Application path</param>
        /// <param name="CLA">command line argument</param>
        /// <returns>true/false. True if loaded successfully otherwise false.</returns>
        public static bool LoadExchange(string applicationName, string applicationPath, string CLA)
        {
            bool RetVal = true;


            try
            {
                DICommon.ApplicationName = applicationName;
                DICommon.Applicationpath = applicationPath;
                DataExchange.LoadDefaultValuesForExchange(CLA);

                // create spreadsheet folder if doesnt exist
                if (!string.IsNullOrEmpty(DICommon.DefaultFolder.DefaultSpreadSheetsFolder))
                {

                    if (!Directory.Exists(DICommon.DefaultFolder.DefaultSpreadSheetsFolder))
                    {
                        Directory.CreateDirectory(DICommon.DefaultFolder.DefaultSpreadSheetsFolder);
                    }

                }
                DICommon.DefaultFolder.DefaultDXIconFolder = DICommon.Applicationpath + @"\bin\ICONS\";

                DILanguage.GetLanguageFile();
                if (!string.IsNullOrEmpty(DICommon.LangaugeFileNameWithPath))
                {
                    DILanguage.Open(DICommon.LangaugeFileNameWithPath);
                }
            }
            catch (Exception ex)
            {
                RetVal = false;
                throw new ApplicationException(ex.ToString());
            }

            return RetVal;
        }

        /// <summary>
        /// Returns the database language code
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public static string GetDBLanguageCode(DIConnection dbConnection)
        {
            string RetVal = string.Empty;
            string DataPrefix = dbConnection.DIDataSetDefault();

            if (dbConnection.IsValidDILanguage(DataPrefix, DataAdmin.DAAppliationLangCode))
            {
                RetVal = "_" + DataAdmin.DAAppliationLangCode;
            }
            else
            {
                RetVal = dbConnection.DILanguageCodeDefault(DataPrefix);
            }

            return RetVal;
        }

        /// <summary>
        /// Returns the DIQueries object
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <returns></returns>
        public static DIQueries GetDBQueries(DIConnection dbConnection)
        {
            DIQueries RetVal = null;

            string LanguageCode = string.Empty;
            string DataPrefix = dbConnection.DIDataSetDefault();

            if (dbConnection.IsValidDILanguage(DataPrefix, DataAdmin.DAAppliationLangCode))
            {
                LanguageCode = "_" + DataAdmin.DAAppliationLangCode;
            }
            else
            {
                LanguageCode = dbConnection.DILanguageCodeDefault(DataPrefix);
            }

            RetVal = new DIQueries(DataPrefix, LanguageCode);

            return RetVal;
        }

        /// <summary>
        /// Returns the DIQueries object
        /// </summary>
        /// <param name="dbConnection"></param>
        /// <param name="dataPrefix"></param>    
        /// <returns></returns>
        public static DIQueries GetDBQueries(DIConnection dbConnection, string dataPrefix)
        {
            DIQueries RetVal = null;

            string LanguageCode = string.Empty;

            //-- Set Default dataprefix if not passed
            if (string.IsNullOrEmpty(dataPrefix))
                dataPrefix = dbConnection.DIDataSetDefault();

            if (dbConnection.IsValidDILanguage(dataPrefix, DataAdmin.DAAppliationLangCode))
            {
                LanguageCode = "_" + DataAdmin.DAAppliationLangCode;
            }
            else
            {
                LanguageCode = dbConnection.DILanguageCodeDefault(dataPrefix);
            }

            RetVal = new DIQueries(dataPrefix, LanguageCode);

            return RetVal;
        }

        #endregion

    }
}
