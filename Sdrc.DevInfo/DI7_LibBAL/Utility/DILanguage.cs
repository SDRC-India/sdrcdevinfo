using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace DevInfo.Lib.DI_LibBAL.Utility
{
    /// <summary>
    /// Use this class to get language strings.
    /// </summary>
    public class DILanguage
    {
        #region "-- Private --"

        #region "-- Variables --"

        private static XmlDocument XMLDoc;

        #endregion

        #endregion

        #region "-- Public --"

        #region "-- Variables --"

        public static bool IsRTL = false;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Validates for valid language file name convention and valid xml content of language file.
        /// For validation of naming convention of language file name in isolation, use DevInfo.Lib.DI_LibBAL.NamingConvention.DIFileNameChecker.IsValidLanguageFileName
        /// </summary>
        /// <returns></returns>
        public static bool IsValidLanguageFile(string languageFilePath)
        {
            bool RetVal = false;

            //-- Check for valid argument and file existance
            if (!string.IsNullOrEmpty(languageFilePath) && File.Exists(languageFilePath))
            {
                //-- Check for valid language file name convention
                if (DevInfo.Lib.DI_LibBAL.NamingConvention.DIFileNameChecker.IsValidLanguageFileName(Path.GetFileName(languageFilePath)))
                {
                    try
                    {
                        //-- Try loading xml file
                        XmlDocument xmlDocument = new XmlDocument();
                        xmlDocument.Load(languageFilePath);

                        //-- Try to fetch a value against LANGUAGE_NAME name key
                        XmlElement xmlElement = xmlDocument.GetElementById("LANGUAGE_NAME");

                        //-- If some valid value is returned, then its a valid language xml content
                        //-- This check may be made more exhaustive if desired.
                        if (xmlElement != null)
                        {
                            if (!string.IsNullOrEmpty(xmlElement.Attributes["value"].Value))
                            {
                                RetVal = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        RetVal = false;
                    }
                }
            }
            return RetVal;
        }

        /// <summary>
        /// To open language file
        /// </summary>
        /// <param name="filename"></param>
        public static void Open(string filename)
        {
            XMLDoc = new XmlDocument();
            XMLDoc.Load(filename);

            //set IsRTL
            if (DILanguage.GetLanguageString("PAGEDIRECTION").ToString().ToUpper() == "RTL")
            {
                DILanguage.IsRTL = true;
            }

        }

        /// <summary>
        ///  Returns value from langauge file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetLanguageString(string key)
        {
             string RetVal = string.Empty;
            XmlElement Element;

            try
            {

                key = key.ToUpper();
                Element = XMLDoc.GetElementById(key);
                if (Element != null)
                {
                    RetVal = Element.Attributes["value"].Value;
                }
            }
            catch (Exception)
            {
                RetVal = key;
            }

            return RetVal;
        }

        /// <summary>
        /// Gets The Current  Language File 
        /// </summary>
        /// <returns></returns>
        public static XmlDocument GetLanguageFile()
        {
            //internal XmlDocument GetLanguageFile(Optional ByVal sLngCode As String = "") 
            XmlDocument RetVal = new XmlDocument();

            try
            {

                //-- Load Language File          
                RetVal.Load(DICommon.LangaugeFileNameWithPath);
            }

            catch (Exception ex)
            {
                //throw;
            }
            return RetVal;
        }

        /// <summary>
        /// Obsolete function. Use DILanguage.Open and DILanguage.GetLanguageString method instead
        /// </summary>
        /// <param name="XMLDoc"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [Obsolete]
        public static string GetLngStringValue(XmlDocument XMLDoc, string key)
        {
            //TODO phase out its usage (DIUI, DA, DX) and rename it to GetLanguageString (overloaded)
            string RetVal = string.Empty;
            XmlElement Element;
            ;
            try
            {

                key = key.ToUpper();
                Element = XMLDoc.GetElementById(key);
                if (Element != null)
                {
                    RetVal = Element.Attributes["value"].Value;
                }
            }
            catch (Exception)
            {
                RetVal =  key;
            }
            return RetVal;
        }

        /// <summary>
        /// Get language code (en) form language file name (DI_English [en])
        /// </summary>
        /// <param name="languageFileName">Language file name without extension DI_English [en]</param>
        /// <returns></returns>
        public static string GetLanguageCode(string languageFileName)  
        {
            string RetVal = string.Empty;

            //-- Sample language file name -> DI_English [en].xml
            if (!string.IsNullOrEmpty(languageFileName))
            {
                int iStartIndex;
                int iEndIndex;
                iStartIndex = languageFileName.IndexOf("[") + 1;
                iEndIndex = languageFileName.IndexOf("]");

                RetVal = languageFileName.Substring(iStartIndex, iEndIndex - iStartIndex);
            }
            return RetVal;

        }


        /// <summary>
        ///  Returns value from langauge file with replaced special character
        /// </summary>
        /// <param name="key"></param>
        /// <param name="replaceSpecialChar">true to replace the special character like "&", "&amp; </param>
        /// <returns></returns>
        public static string GetLanguageString(string key,bool replaceSpecialChar)
        {
            string RetVal = string.Empty;
            XmlElement Element;

            try
            {
                key = key.ToUpper();
                Element = XMLDoc.GetElementById(key);
                if (Element != null)
                {
                    RetVal = Element.Attributes["value"].Value;

                    //-- Replace special character for Xml/Html based string
                    if (replaceSpecialChar)
                    {
                        RetVal = DICommon.RemoveXMLSpecialCharacter(RetVal);
                    }
                }

            }
            catch (Exception)
            {
                RetVal = key;
            }

            return RetVal;
        }

        #endregion

        #endregion
    }
}
