using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using VB=Microsoft.VisualBasic;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.NamingConvention
{
    /// <summary>
    /// Provides methods to check various DevInfo filenames like langauge file name (" DI_English [ex].xml"),etc.
    /// </summary>
    public static class DIFileNameChecker
    {
        #region "-- Public --"

        #region "-- Variables --"

        /// <summary>
        /// Returns the prefix for language file name 
        /// </summary>
        public const string LanguageFilePrefix = "DI_";

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Validates for correct langugae file name convention. DI_English [en].xml
        /// </summary>
        /// <param name="languageFileName">Language file name to be validated</param>
        /// <returns>bool value based on valid language file name</returns>
        public static bool IsValidLanguageFileName(string languageFileName)
        {
            //-- Valid file name sample -> DI_English [en].xml
            bool RetVal = false;

            //-- languageFileName not blank.
            if (!string.IsNullOrEmpty(languageFileName))
            {

                //////-- Validation for prefix 'DI_' 
                ////if (languageFileName.Substring(0, 3).ToUpper() == "DI_".ToUpper())
                ////{
                    string FileNameWithoutDIPrefix = languageFileName; //.Substring(3);
                    string[] arrFileNameParts = FileNameWithoutDIPrefix.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    //-- Validation for ' ' separater between DI_English and  [en]
                    if (arrFileNameParts.Length >= 2)
                    {
                        //-- Validation for '[*]' language code square braces
                        if (arrFileNameParts[1].Substring(0, 1) == "[" && arrFileNameParts[1].IndexOf("]") > -1)
                        {
                            RetVal = true;
                        }
                    }
                ////}
            }
            return RetVal;
        }


        /// <summary>
        /// Returns language code and language name.
        /// </summary>
        /// <param name="fileNameWithoutPath">Language filename without path</param>
        /// <returns></returns>
        public static string[] GetLanguageNameNCode(string fileNameWithoutPath)
        {
            string[] RetVal = new string[2];
            string[] FileNamePartsArray = null;
            
            try
            {
                if (!string.IsNullOrEmpty(fileNameWithoutPath))
                {
                    //////if (VB.Strings.Left(fileNameWithoutPath, 3).ToUpper() == DIFileNameChecker.LanguageFilePrefix.ToUpper())
                    //////{
                    //////    //--condition 1 : 'DI_' matched. 
                        
                        fileNameWithoutPath.Replace(DIFileNameChecker.LanguageFilePrefix, "");
                        FileNamePartsArray = DICommon.SplitString(fileNameWithoutPath, " ");

                        if (FileNamePartsArray.Length >= 2)
                        {
                            //--condition 2 : ' ' found (seperator). 
                            if (VB.Strings.Left(FileNamePartsArray[1], 1) == "[" && FileNamePartsArray[1].IndexOf("]") > -1)
                            {
                                //--condition 3 : '[-]' found (Language code) 
                                RetVal[0]= FileNamePartsArray[1].Substring(1, FileNamePartsArray[1].IndexOf("]") - 1);
                                RetVal[1] = FileNamePartsArray[0];
                            }
                        }

                    //////}
                }
            }
            catch (Exception)
            {
                RetVal = null;
            }

            return RetVal;
        }
        
        #endregion

#endregion

    }
}