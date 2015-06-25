using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Collections;
using Microsoft.Win32;

namespace DevInfo.Lib.DI_LibBAL.Utility
{

    //#region -- Name / Value Pair Class --
    ///// <summary>
    ///// Name / Value Pair Class. Generally used with ComboBox control to store the Item Data information
    ///// </summary>
    //[Obsolete]
    //public class NameValuePair
    //{
    //    #region -- Variables --
    //    private string mText = "";          //-- store value
    //    private object mItemData;		  //-- item data object
    //    #endregion

    //    /// <summary>
    //    /// Constructor - Send Name and Value (Eg. Name for Text in ComboBox, ItemData for ComboBox)
    //    /// </summary>
    //    public NameValuePair(string NewText, object NewItemData)
    //    {
    //        mText = NewText;
    //        mItemData = NewItemData;
    //    }

    //    /// <summary>
    //    /// Returns the text
    //    /// </summary>
    //    public override string ToString()
    //    {
    //        return mText;
    //    }

    //    /// <summary>
    //    /// Set or Get the Text (Name)
    //    /// </summary>
    //    public string Text
    //    {
    //        get
    //        {
    //            return mText;
    //        }
    //        set
    //        {
    //            mText = Text;
    //        }
    //    }
    //    /// <summary>
    //    /// Set or Get ItemData (Value)
    //    /// </summary>
    //    public object ItemData
    //    {
    //        get
    //        {
    //            return mItemData;
    //        }
    //        set
    //        {
    //            mItemData = ItemData;
    //        }
    //    }
    //}
    //#endregion

    #region -- Registry --
    /// <summary>
    /// Registry Class for handling all Registry Functions
    /// </summary>
    public class RegistryInfo
    {

        ///// <summary>
        ///// GetInstallLocation - find the "Install Location" string from registry where 
        ///// compareKey is the part of any child of parent key's "Uninstall String"
        ///// </summary>
        //public static string GetInstallLocation(string sParentKey, string sCompareKey)
        //{
        //    RegistryKey oRegKey;
        //    oRegKey = Registry.LocalMachine.OpenSubKey(sParentKey);

        //    if (oRegKey != null)
        //    {
        //        foreach (string sSubKey in oRegKey.GetSubKeyNames())
        //        {
        //            if (sSubKey.StartsWith("{") == false)
        //            {
        //                if (sSubKey != "DevInfo 5.0 Data Admin")
        //                {
        //                    RegistryKey oSubKey = Registry.LocalMachine.OpenSubKey(sParentKey + "\\" + sSubKey);
        //                    if (oSubKey.GetValue("UninstallString") != null)
        //                    {

        //                        string sUninstallString = (string)oSubKey.GetValue("UninstallString");
        //                        if (oSubKey.GetValue("Install Location") != null)
        //                        {
        //                            string sInstallLocation = (string)oSubKey.GetValue("Install Location");
        //                            sUninstallString = sUninstallString.Replace("/P", "*");
        //                            sUninstallString = (string)(sUninstallString.Split('*'))[0];
        //                            sUninstallString = sUninstallString.Replace(" ", "");
        //                            if (sUninstallString.ToLower() == sCompareKey.ToLower())
        //                            {
        //                                return sInstallLocation;
        //                            }
        //                        }
        //                    }
        //                    oSubKey.Close();
        //                }
        //            }
        //        }
        //        oRegKey.Close();
        //    }
        //    return "";
        //}


        ///// <summary>
        ///// GetInstallLocationofParentKey - find the "Install Location" string of Parent key from registry 
        ///// </summary>
        //public static string GetInstallLocationofParentKey(string sParentKey)
        //{
        //    RegistryKey oRegKey;
        //    string sInstallLocation = "";
        //    oRegKey = Registry.LocalMachine.OpenSubKey(sParentKey);
        //    if (oRegKey != null)
        //    {
        //        if (oRegKey.GetValue("Install Location") != null)
        //            sInstallLocation = (string)oRegKey.GetValue("Install Location");
        //        oRegKey.Close();
        //    }
        //    return (sInstallLocation);
        //}

        /// <summary>
        /// It enables/ disables Autorun feature of specified application's Key and Path.
        /// </summary>
        /// <param name="enable">true to enable. False to disable</param>
        /// <param name="applicationKey">application registry keyName</param>
        /// <param name="applicationPath">application file name with Path</param>
        /// <returns>true, if successfully done.</returns>
        public static bool EnableAutoRunInRegistry(string applicationKey, string applicationPath, bool enable)
        {
            bool RetVal = false;

            //-- Key to add reference for AutoRun
            //HKEY_Local_Machine\SOFTWARE\Microsoft\Windows\CurrentVersion\Run
            try
            {
                RegistryKey masterKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (masterKey == null)
                {
                    //Console.WriteLine("Null Masterkey!");
                }
                else
                {
                    try
                    {

                        //-- Enable Key in registry
                        if (enable)
                        {
                            if (string.IsNullOrEmpty(applicationKey) == false && string.IsNullOrEmpty(applicationPath) == false)
                            {
                                masterKey.SetValue(applicationKey, applicationPath, RegistryValueKind.String);
                                RetVal = true;
                            }
                        }
                        else
                        {
                            //-- Remove Key from Registry
                            masterKey.DeleteValue(applicationKey, false);
                            RetVal = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        masterKey.Close();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return RetVal;
        }
    }
    #endregion

    //#region -- DevInfo Registry --
    ///// <summary>
    ///// DevInfo Class for handling all Functions for DevInfo specific features
    ///// </summary>
    public class DIRegistry
    {

    //    /// <summary>
    //    /// GetDevInfoUserInterfacePath - find the DevInfo User Inteface adaptation path from registry.
    //    /// </summary>
    //    public static String GetDevInfoUserInterfacePath()
    //    {
    //        //Path : /a C:\DevInfo\DevInfo 5.0\
    //        string sUninstallLocation = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
    //        return (" /a " + RegistryInfo.GetInstallLocation(sUninstallLocation, "c:\\devinfo\\uninstall\\di5uninstall.exe"));
    //    }

        /// <summary>
        /// GetDevInfoDataAdminPath - find the DevInfo DataAdmin "Install Location"  from registry.
        /// </summary>
        public static String GetDevInfoDataAdminPath()
        {
            string sUninstallLocation = "Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\DevInfo 5.0 Data Admin";
            //return (RegistryInfo.GetInstallLocationofParentKey(sUninstallLocation));
            //TODO remove hardcoding
            return @"C:\DevInfo\DevInfo 7.0\DevInfo 7.0 Data Admin";
        }


        /// <summary>
        /// GetDACommandLineArgs - find the DevInfo DataAdmin path, language name and language code.
        /// </summary>
        public static string[] GetDACommandLineArgs(string sCmdLnArg)
        {
            string[] sCmdArgs = new string[3];
            string sDAAppPath, sAppLng = "";
            string sAppLngCode = "";
            char cSplitChar = '*';
            sCmdArgs[0] = "";
            sCmdArgs[1] = "";
            sCmdArgs[2] = "";
            if (sCmdLnArg.Trim() != "")
            {
                try
                {
                    sCmdLnArg = sCmdLnArg.Replace("/a ", cSplitChar.ToString());
                    //-- get the arguments 
                    sCmdLnArg = sCmdLnArg.Split(cSplitChar)[1];

                    if ((sCmdLnArg.IndexOf("/l ") != -1) || (sCmdLnArg.IndexOf("/L ") != -1))
                    {
                        //"C:\-- Projects --\DevInfo 5.0 - VS 2005\DI_DataAdmin\bin /L  DI_English [en]"
                        sCmdLnArg = sCmdLnArg.Replace("/l ", cSplitChar.ToString());
                        sCmdLnArg = sCmdLnArg.Replace("/L ", cSplitChar.ToString());
                        sDAAppPath = sCmdLnArg.Split(cSplitChar)[0];
                        sAppLng = sCmdLnArg.Split(cSplitChar)[1];
                        sAppLng = sAppLng.Replace("\"", "");
                    }
                    else
                    {
                        sDAAppPath = sCmdLnArg;
                    }

                    //-- Remove the slash from the end of the path if present
                    if (sDAAppPath.EndsWith("\\") || sDAAppPath.EndsWith("/") || sDAAppPath.EndsWith(" "))
                    {
                        sDAAppPath = sDAAppPath.Substring(0, sDAAppPath.Length - 1);
                    }

                    //-- get language file name :"DI_English [en]"
                    if (sAppLng.Length > 2)
                    {
                        if (sAppLng.StartsWith(" "))
                            sAppLng = sAppLng.Substring(1);
                        if (sAppLng.EndsWith(" "))
                            sAppLng = sAppLng.Substring(0, sAppLng.Length - 1);
                        sAppLngCode = sAppLng.Substring(sAppLng.IndexOf("["));
                        sAppLngCode = sAppLngCode.Replace("[", "");
                        sAppLngCode = sAppLngCode.Replace("]", "");
                        sAppLngCode = sAppLngCode.Replace(" ", "");
                    }
                    sCmdArgs[0] = sDAAppPath;
                    sCmdArgs[1] = sAppLng;
                    sCmdArgs[2] = sAppLngCode;
                }
                catch (Exception ex)
                {
                    sCmdArgs[0] = "";
                    sCmdArgs[1] = "";
                    sCmdArgs[2] = "";
                }

            }
            return sCmdArgs;
        }

    }
    //#endregion
}
