using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using DevInfo.Lib.DI_LibBAL.UI.UserPreference;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    public static class SubApplicationInfo
    {
        #region " -- Private -- "

        #region " -- Variables -- "

        
        private static string AdaptationFileNameWPath = string.Empty;
        private static string PrefFilePath = string.Empty;        
        private static UserPreference UserPreferenceObj = null;

        private const string SPATIAL_FOLDER = @"Map\SpatialMaps";
        private const string TEMP_FOLDER = "TEMP";
        private const string FLASH_IMAGES_FOLDER = @"Flash\Images";
        private const string MASK_FOLDER = @"Metadata\Mask";
        private const string FORMAT_FOLDER = "Format";
        private const string CUSTOMFEATURELAYER_FOLDER = @"Map\Custom Feature Layers";

        #endregion

        #region " -- Methods -- "


        #endregion

        #endregion

        #region " -- Public -- "

        #region " -- Variables / Properties -- "

        public static Adaptation AdaptationObj = null;

        private static bool _IsDILiteSetup = false;
        /// <summary>
        /// Set true, if DIlite setup
        /// </summary>
        public static bool IsDILiteSetup
        {
            set 
            {
                _IsDILiteSetup = value; 
            }
        }

        private static string _LiteAdaptationPath;
        /// <summary>
        /// Set the lite adaptation path
        /// </summary>
        public static string LiteAdaptationPath
        {
            set 
            {
                _LiteAdaptationPath = value; 
            }
        }


        private static string _AdaptationFolderPath = string.Empty;

        public static string AdaptationFolderPath
        {
            get
            {
                return _AdaptationFolderPath;
            }
            set { _AdaptationFolderPath = value; }
        }


        #endregion

        #region " -- Path Affected By Lite Setup

        /// <summary>
        /// Get the Gallery database file path.
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetGalleryDatabaseFileNameWPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }

                if (!_IsDILiteSetup)
                {     
                    if (!string.IsNullOrEmpty(appPath))
                    {
                        Retval = Path.Combine(Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Stock), AdaptationObj.DirectoryLocation.SystemFolder), DICommon.Others.GalleryDatabase);
                    }
                }
                else
                {
                    Retval = Path.Combine(Path.Combine(Path.Combine(_LiteAdaptationPath, AdaptationObj.DirectoryLocation.Stock), AdaptationObj.DirectoryLocation.SystemFolder), DICommon.Others.GalleryDatabase);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the spatial map folder path.
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetMapSpatialFolderPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }

                if (!_IsDILiteSetup)
                {
                    if (!string.IsNullOrEmpty(appPath))
                    {
                        Retval = Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Stock), SPATIAL_FOLDER);
                    }
                }
                else
                {
                    Retval = Path.Combine(Path.Combine(_LiteAdaptationPath, AdaptationObj.DirectoryLocation.Stock), SPATIAL_FOLDER);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the presentation folder path.
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetPresentationFolderPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }

                if (!_IsDILiteSetup)
                {
                    if (!string.IsNullOrEmpty(appPath))
                    {
                        Retval = Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Gallery);
                    }
                }
                else
                {
                    Retval = Path.Combine(_LiteAdaptationPath, AdaptationObj.DirectoryLocation.Gallery);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the temp folder path.
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetTempFolderPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }

                if (!_IsDILiteSetup)
                {
                    if (!string.IsNullOrEmpty(appPath))
                    {
                        Retval = Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Stock), TEMP_FOLDER);
                    }
                }
                else
                {
                    Retval = Path.Combine(Path.Combine(_LiteAdaptationPath, AdaptationObj.DirectoryLocation.Stock), TEMP_FOLDER);
                }

            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the pref file name with path
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetPrefFilePath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }

                if (!_IsDILiteSetup)
                {
                    if (!string.IsNullOrEmpty(appPath))
                    {
                        Retval = PrefFilePath;
                    }
                }
                else
                {
                    Retval = Path.Combine(Path.Combine(Path.Combine(_LiteAdaptationPath, AdaptationObj.DirectoryLocation.Stock), AdaptationObj.DirectoryLocation.SystemFolder), UserPreference.USER_PREF_FILE_NAME);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the stock folder path
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetStockFolderPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }

                if (!_IsDILiteSetup)
                {
                    if (!string.IsNullOrEmpty(appPath))
                    {
                        Retval = Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Stock);
                    }
                }
                else
                {
                    Retval = Path.Combine(_LiteAdaptationPath, AdaptationObj.DirectoryLocation.Stock);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the format folder path
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetFormatFolderPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }

                if (!_IsDILiteSetup)
                {
                    if (!string.IsNullOrEmpty(appPath))
                    {
                        Retval = Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Stock), FORMAT_FOLDER);
                    }
                }
                else
                {
                    Retval = Path.Combine(Path.Combine(_LiteAdaptationPath, AdaptationObj.DirectoryLocation.Stock), FORMAT_FOLDER);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the custom feature layer folder path
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetCustomFeatureFolderPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }

                if (!_IsDILiteSetup)
                {
                    if (!string.IsNullOrEmpty(appPath))
                    {
                        Retval = Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Stock), CUSTOMFEATURELAYER_FOLDER);
                    }
                }
                else
                {
                    Retval = Path.Combine(Path.Combine(_LiteAdaptationPath, AdaptationObj.DirectoryLocation.Stock), CUSTOMFEATURELAYER_FOLDER);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the report folder path
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetReportFolderPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }

                if (!_IsDILiteSetup)
                {
                    if (!string.IsNullOrEmpty(appPath))
                    {
                        Retval = Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Reports);
                    }
                }
                else
                {
                    Retval = Path.Combine(_LiteAdaptationPath, AdaptationObj.DirectoryLocation.Reports);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }


        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Sets the DIAdaptations static property and sets the Adaptations folder path.
        /// </summary>
        /// <param name="appPath"></param>
        public static void GetAdaptationFolderPath(string appPath)
        {
            try
            {
                DIAdaptations DIAdaptationObj;

                if (!_IsDILiteSetup)
                {
                    //-- Get the DIAdapatation.xml file path.
                    AdaptationFileNameWPath = Path.Combine(appPath, DIAdaptations.DI_ADAPTATIONS_FILENAME);
                }

                if (File.Exists(AdaptationFileNameWPath))
                {
                    DIAdaptationObj = DIAdaptations.Load(AdaptationFileNameWPath);
                    if (!String.IsNullOrEmpty(DIAdaptationObj.LastUsedAdaptation.AdaptationFolder))
                    {
                        AdaptationFolderPath = DIAdaptationObj.LastUsedAdaptation.AdaptationFolder;

                        //-- check for the existence of adaptation folder path.
                        if (Directory.Exists(AdaptationFolderPath))
                        {
                            //-- Create the instance of Adaptation.xml
                            if (File.Exists(Path.Combine(AdaptationFolderPath, Adaptation.ADAPTATION_FILE_NAME)))
                            {
                                AdaptationObj = Adaptation.Load(Path.Combine(AdaptationFolderPath, Adaptation.ADAPTATION_FILE_NAME));
                            }
                            else
                            {
                                AdaptationObj = new Adaptation(); 
                            }
                        }
                    }
                }
                else if (_IsDILiteSetup)
                {
                    if (!string.IsNullOrEmpty(_AdaptationFolderPath))
                    {
                        if (File.Exists(Path.Combine(_AdaptationFolderPath, Adaptation.ADAPTATION_FILE_NAME)))
                        {
                            AdaptationObj = Adaptation.Load(Path.Combine(AdaptationFolderPath, Adaptation.ADAPTATION_FILE_NAME)); 
                        }
                        else
                        {
                            AdaptationObj = new Adaptation();
                        }
                    }
                }

                if (AdaptationObj != null && Directory.Exists(Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Stock), AdaptationObj.DirectoryLocation.SystemFolder)))
                {
                    if (!File.Exists(Path.Combine(Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Stock), AdaptationObj.DirectoryLocation.SystemFolder), UserPreference.USER_PREF_FILE_NAME)))
                    {
                        UserPreferenceObj = new UserPreference(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Language));

                        if (Directory.Exists(Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Stock), AdaptationObj.DirectoryLocation.SystemFolder)))
                        {
                            //-- If the system file directory exists.
                            PrefFilePath = Path.Combine(Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Stock), AdaptationObj.DirectoryLocation.SystemFolder), UserPreference.USER_PREF_FILE_NAME);
                        }
                        else
                        {
                            PrefFilePath = string.Empty;
                        }
                    }
                    else
                    {
                        UserPreferenceObj = UserPreference.Load(Path.Combine(Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Stock), AdaptationObj.DirectoryLocation.SystemFolder), UserPreference.USER_PREF_FILE_NAME), Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Language));
                        PrefFilePath = Path.Combine(Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Stock), AdaptationObj.DirectoryLocation.SystemFolder), UserPreference.USER_PREF_FILE_NAME);
                    }
                }



            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Get the language file path.
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetLanguageFilePath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }
                if (!string.IsNullOrEmpty(appPath))
                {
                    Retval = Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Language), UserPreferenceObj.Language.InterfaceLanguage + ".xml");
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the adaptation folder path.
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetAdaptationPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }
                Retval = AdaptationFolderPath;
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the fiash image folder path
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetFlashImagesPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }
                if (!string.IsNullOrEmpty(appPath))
                {
                    Retval = Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Adaptation), FLASH_IMAGES_FOLDER);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the data folder path
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetDataFolderPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }
                if (!string.IsNullOrEmpty(appPath))
                {
                    Retval = Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Data);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }


        /// <summary>
        /// Get the stock folder path
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetMaskFolderPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }
                if (!string.IsNullOrEmpty(appPath))
                {
                    Retval = Path.Combine(Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Template), MASK_FOLDER);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the common image folder path
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetCommonImageFolderPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }
                if (!string.IsNullOrEmpty(appPath))
                {
                    Retval = Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Common);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the adaptation graphics folder path
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetAdaptationImageFolderPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }
                if (!string.IsNullOrEmpty(appPath))
                {
                    Retval = Path.Combine(AdaptationFolderPath, AdaptationObj.DirectoryLocation.Adaptation);
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }

        /// <summary>
        /// Get the DIAdaptation xml folder path
        /// </summary>
        /// <param name="appPath"></param>
        /// <returns></returns>
        public static string GetDIAdaptationFolderPath(string appPath)
        {
            string Retval = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(appPath) && string.IsNullOrEmpty(AdaptationFolderPath))
                {
                    GetAdaptationFolderPath(appPath);
                }
                if (!string.IsNullOrEmpty(appPath))
                {
                    Retval = AdaptationFileNameWPath;
                }
            }
            catch (Exception)
            {
            }
            return Retval;
        }
  
        #endregion

        #endregion

    }
}
