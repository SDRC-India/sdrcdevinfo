using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using System.Data;
using DevInfo.Lib.DI_LibDAL;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using Microsoft.VisualBasic;
using SpreadsheetGear;
using System.Xml;
using System.Collections.Specialized;
using System.Collections;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.Map;
using DevInfo.Lib.DI_LibBAL.DA.DML;
namespace DevInfo.Lib.DI_LibBAL.UI.Calculates
{
    /// <summary>
    /// Calculate class contain methods related to Spreadsheet implementation in calculate module
    /// In all calculate twice excel sheet are displayed.
    /// Aim to this class is to use spreadsheetgear instead of excel in calculates
    /// </summary>
    public class Calculates
    {
        #region " -- Private -- "


        #region " -- Variable -- "

        DIExcel DIExcel;
        LanguageStrings LangStrings = new LanguageStrings();
        DESheetInfo DataEntrySheetInfo = new DESheetInfo();
        int miCtrFillTo = 0;
        String[] msClassification = new string[7];
        // Format(Date.Now, "yyyy-MMM-dd H-mm-ss")        
        private String TimeStamp = DateTime.Now.ToString("yyyy-MMM-dd H-mm-ss");
        private String[,] msDataArray = new string[1, 7]; // (RecCount -1, 7) ' Time AreaId AreaName DataValue Subgroup Source FootNote Denominator

        //Excel.Worksheet 
        SpreadsheetGear.IWorksheet SummarySheet;
        SpreadsheetGear.IWorksheet LogSheet;

        #endregion

        #region " -- Enum -- "

        /// <summary>
        /// Diff Calculate  wizard Types
        /// </summary>
        public enum WizardType
        {
            /// <summary>
            /// Calulate Wizard : Percent
            /// </summary>
            Percent = 1,
            /// <summary>
            /// Calulate Wizard : Hundred minus
            /// </summary>
            HundredMinus = 2,
            /// <summary>
            /// Calulate Wizard : Composite Index
            /// </summary>
            CompositeIndex = 3,
            /// <summary>
            /// Calulate Wizard :SubTotal
            /// </summary>
            SubTotal = 4,
            /// <summary>
            /// Calulate Wizard : Transform Unit
            /// </summary>
            TransformUnit = 5,
            /// <summary>
            /// Calulate Wizard : User defindedFormula
            /// </summary>
            UserDefinedFormula = 6
        }

        #endregion

        #region"--Inner Class --"
        /// <summary>
        /// Class holding Different Language String
        /// </summary>
        private class LanguageStrings
        {

            public string AREAID = "AREAID";
            public string AREANAME = "AREANAME";
            public string INDICATOR = "INDICATOR";
            public string SUBGROUP = "SUBGROUP";
            public string UNIT = "UNIT";
            public string TIME = "TIME";
            public string SOURCE = "SOURCE";
            public string DATAVALUE = "DATAVALUE";
            public string DECIMALPLACES = "DECIMALPLACES";
            public string TOTAL = "TOTAL";
            public string PERCENT = "PERCENT";
            public string INDEX = "INDEX";
            public string NUMERATOR = "NUMERATOR";
            public string DENOMINATOR = "DENOMINATOR";
            public string FOOTNOTES = "FOOTNOTES";
            public string sMODULE = "MODULE";
            public string DATE_TIME = "DATE_TIME";
            public string LOG_FILE_NAME = "LOG_FILE_NAME";
            public string TARGET = "TARGET";
            public string ELEMENT = "ELEMENT";
            //*** will vary based on wizard type 
            public string VAR_WIZARD_NAME = "VAR_WIZARD_NAME";
            //*** will vary based on wizard type 
            public string VAR_LOG_NAME = "VAR_LOG_NAME";
            public string SCALE_MIN = "SCALE_MIN";
            public string SCALE_MAX = "SCALE_MAX";
            public string PERCENT_WEIGHT = "PERCENT_WEIGHT";
            public string HIGHISGOOD = "HIGHISGOOD";
            public string CONVERSION_FACTOR = "CONVERSION_FACTOR";
            public string DEVINFO_DATA_ENTRY_SPREADSHEET = "DEVINFO_DATA_ENTRY_SPREADSHEET";
            public string SUMMARY = "SUMMARY";
            public string LOG = "DATABASE_LOG";
            public string DATA = "DATA";
            public string IC_CLASS = "CLASS";
            public string STEP_ = "STEP";
            public string OF_ = "OF";
        }

        /// <summary>
        /// cLass hold Data Entry Spreadsheet related information
        /// </summary>
        public class DESheetInfo
        {
            public string SectorName = string.Empty;
            public string SectorGUID = string.Empty;
            //*** 0=False, 1=True 
            public string SectorGlobal = string.Empty;
            public string ClassName = string.Empty;
            public string ClassGUID = string.Empty;
            //*** 0=False, 1=True 
            public string ClassGlobal = string.Empty;
            public string Indicator = string.Empty;
            public string IndicatorGUID = string.Empty;
            public string Unit = string.Empty;
            public string UnitGUID = string.Empty;
            //*** Except for 100 Minus Only one array item 
            public string[] Subgroup;
            public string[] SubgroupGUID;
            public int Decimals = 0;
        }


        #endregion

        #region " -- Methods -- "

        /// <summary>
        /// Get Language File
        /// </summary>
        /// <param name="LangFilePath"></param>
        /// <returns></returns>
        private XmlDocument GetLanguageFile(string LangFilePath)
        {
            XmlDocument Retval = new XmlDocument();
            try
            {
                Retval.Load(LangFilePath);
            }
            catch (Exception ex)
            {
            }
            return Retval;
        }

        private void ApplyLanguageSettings()
        {
            // Get xml Doc           
            // XmlDocument LanguageFile = GetLanguageFile(LangFilePath);
            DILanguage.Open(LangFilePath);

            //'*** Classification Strings ie Sector - Goal - ...
            msClassification[0] = DILanguage.GetLanguageString("SECTOR");
            msClassification[1] = DILanguage.GetLanguageString("GOAL");
            msClassification[2] = DILanguage.GetLanguageString("CF");
            msClassification[3] = DILanguage.GetLanguageString("THEME");
            msClassification[4] = DILanguage.GetLanguageString("SOURCE");
            msClassification[5] = DILanguage.GetLanguageString("INSTITUTION");
            msClassification[6] = DILanguage.GetLanguageString("CONVENTION");

            LangStrings.AREAID = DILanguage.GetLanguageString("AREAID");
            LangStrings.AREANAME = DILanguage.GetLanguageString("AREANAME");
            LangStrings.CONVERSION_FACTOR = DILanguage.GetLanguageString("CONVERSION_FACTOR");
            LangStrings.DATA = DILanguage.GetLanguageString("DATA");
            LangStrings.DATAVALUE = DILanguage.GetLanguageString("DATAVALUE");
            LangStrings.DATE_TIME = DILanguage.GetLanguageString("DATE_TIME");
            LangStrings.DECIMALPLACES = DILanguage.GetLanguageString("DECIMALPLACES");
            LangStrings.DEVINFO_DATA_ENTRY_SPREADSHEET = DILanguage.GetLanguageString("DEVINFO_DATA_ENTRY_SPREADSHEET");
            LangStrings.DENOMINATOR = DILanguage.GetLanguageString("DENOMINATOR");
            LangStrings.ELEMENT = DILanguage.GetLanguageString("ELEMENT");
            LangStrings.FOOTNOTES = DILanguage.GetLanguageString("FOOTNOTES");
            LangStrings.HIGHISGOOD = DILanguage.GetLanguageString("HIGHISGOOD");
            LangStrings.IC_CLASS = DILanguage.GetLanguageString("CLASS");
            LangStrings.INDEX = DILanguage.GetLanguageString("INDEX");
            LangStrings.INDICATOR = DILanguage.GetLanguageString("INDICATOR");
            LangStrings.LOG = DILanguage.GetLanguageString("DATABASE_LOG");
            if (LangStrings.LOG.Length > 31)
            {
                LangStrings.LOG = LangStrings.LOG.Substring(0, 31);
            }
            LangStrings.LOG_FILE_NAME = DILanguage.GetLanguageString("LOG_FILE_NAME");
            LangStrings.OF_ = DILanguage.GetLanguageString("OF");
            LangStrings.NUMERATOR = DILanguage.GetLanguageString("NUMERATOR");
            LangStrings.PERCENT = DILanguage.GetLanguageString("PERCENT");
            LangStrings.PERCENT_WEIGHT = DILanguage.GetLanguageString(LangStrings.PERCENT_WEIGHT);
            LangStrings.SCALE_MAX = DILanguage.GetLanguageString("SCALE_MAX");
            LangStrings.SCALE_MIN = DILanguage.GetLanguageString("SCALE_MIN");
            LangStrings.sMODULE = DILanguage.GetLanguageString("MODULE");
            LangStrings.SOURCE = DILanguage.GetLanguageString("SOURCECOMMON");
            LangStrings.STEP_ = DILanguage.GetLanguageString("STEP");

            LangStrings.SUBGROUP = DILanguage.GetLanguageString("SUBGROUP");
            LangStrings.SUMMARY = DILanguage.GetLanguageString("SUMMARY");
            if (LangStrings.SUMMARY.Length > 31)
            {
                LangStrings.SUMMARY = LangStrings.SUMMARY.Substring(0, 31);
            }
            LangStrings.TARGET = DILanguage.GetLanguageString("TARGET");
            LangStrings.TIME = DILanguage.GetLanguageString("TIME");
            LangStrings.TOTAL = DILanguage.GetLanguageString("TOTAL");
            LangStrings.UNIT = DILanguage.GetLanguageString("UNIT");

            switch (ApplicationWizardType)
            {
                case WizardType.Percent:
                    LangStrings.VAR_WIZARD_NAME = DILanguage.GetLanguageString("PERCENT");
                    LangStrings.VAR_LOG_NAME = DILanguage.GetLanguageString("PERCENT_LOG");
                    break;
                case WizardType.HundredMinus:
                    LangStrings.VAR_WIZARD_NAME = DILanguage.GetLanguageString("HUNDREDMINUS");
                    LangStrings.VAR_LOG_NAME = DILanguage.GetLanguageString("HUNDRED_MINUS_LOG");
                    break;
                case WizardType.CompositeIndex:
                    LangStrings.VAR_WIZARD_NAME = DILanguage.GetLanguageString("COMPOSITEINDEX");
                    LangStrings.VAR_LOG_NAME = DILanguage.GetLanguageString("COMPOSITE_INDEX_LOG");
                    //this.DESheetInformation.Indicator = cbo3_3_Indicator.
                    break;
                case WizardType.SubTotal:
                    LangStrings.VAR_WIZARD_NAME = DILanguage.GetLanguageString("SUBTOTAL");
                    LangStrings.VAR_LOG_NAME = DILanguage.GetLanguageString("SUBTOTAL_LOG");
                    break;
                case WizardType.TransformUnit:
                    LangStrings.VAR_WIZARD_NAME = DILanguage.GetLanguageString("TRANSFORMUNIT");
                    LangStrings.VAR_LOG_NAME = DILanguage.GetLanguageString("TRANSFORM_UNIT_LOG");
                    break;
                case WizardType.UserDefinedFormula:
                    LangStrings.VAR_WIZARD_NAME = DILanguage.GetLanguageString("USERDEFINEDFORMULA");
                    LangStrings.VAR_LOG_NAME = DILanguage.GetLanguageString("USER_DEFINED_FORMULA_LOG");
                    break;
                default:
                    break;
            }
            //LangStrings.AREAID=
        }

        /// <summary>
        /// Raise event to notify progress for setting progress bar
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private void RaiseEventSetProgress(int value)
        {
            if (this.SetProgress != null)
            {
                this.SetProgress(value);
            }
        }

        /// <summary>
        /// Raise event for hiding progress bar
        /// </summary>
        private void RaiseEventHideProgressBar()
        {
            if (this.HideProgBar != null)
            {
                this.HideProgBar();
            }
        }

        /// <summary>
        /// Raise event to notify progress for setting progress bar
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private void RaiseEventSetCursor(bool setToDefault)
        {
            if (this.SetCursorType != null)
            {
                this.SetCursorType(setToDefault);
            }
        }

        /// <summary>
        /// Raise event to notify that step4 has been completed
        /// </summary>
        private void RaiseEventStep4Completed()
        {
            if (this.Step4Completed != null)
            {
                this.Step4Completed();
            }
        }

        /// <summary>
        /// Raise event to notify that step5 has been completed
        /// </summary>
        private void RaiseEventStep5Completed(string parentPanel)
        {
            if (this.FillDESheetDataCompleted != null)
            {
                this.FillDESheetDataCompleted(parentPanel);
            }
        }

        /// <summary>
        /// Raise event to notify that step5 has been completed
        /// </summary>
        private void RaiseEventCalculateStepCompleted(string parentPanel)
        {
            if (this.CalculteStepCompleted != null)
            {
                this.CalculteStepCompleted(parentPanel);
            }
        }


        ///// <summary>
        ///// Raise event to notify that step2_3 has been completed
        ///// </summary>
        //private void RaiseEventStep2_3Completed()
        //{
        //    if (this.Step2_3Completed != null)
        //    {
        //        this.Step2_3Completed();
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        private void RaiseEventStep3_4Completed()
        {
            if (this.Step3_4Completed != null)
            {
                this.Step3_4Completed();
            }
        }

        /// <summary>
        /// Raise event to notify to set step3 indicator and unit text 
        /// </summary>
        /// <param name="indicator"></param>
        /// <param name="unit"></param>
        private void RaiseEventSetStep3IndicatorAndUnit(string indicator, string unit)
        {
            if (this.SetStep3IndicatorAndUnit != null)
            {
                this.SetStep3IndicatorAndUnit(indicator, unit);
            }
        }

        /// <summary>
        /// Autofill destination range with the source range formula
        /// </summary>
        /// <param name="sourceRange"></param>
        /// <param name="DestinationRange"></param>
        private void AutoFillRange(IRange sourceRange, IRange DestinationRange)
        {
            DestinationRange.Formula = sourceRange.Formula;
        }

        /// <summary>
        /// Get SectorNId
        /// </summary>
        /// <param name="DBConnection"></param>
        /// <param name="DBQueries"></param>
        /// <param name="bNewSector"></param>
        /// <returns></returns>
        private int GetSectorNId(DIConnection DBConnection, DIQueries DBQueries, out bool bNewSector, out Boolean bSectorGlobal)
        {
            int RetVal = 0;
            string sQry = string.Empty;
            Boolean SectorGlobal = false;
            Boolean NewSector = false;
            try
            {
                sQry = DBQueries.Calculates.GetICNIdByGId("'" + this.DESheetInformation.SectorGUID + "'");
                //-- Executing Query 
                RetVal = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));

                if (RetVal == 0)
                {
                    NewSector = true;
                    if (string.IsNullOrEmpty(this.DESheetInformation.SectorGlobal))
                    {
                    }
                    else
                    {
                        SectorGlobal = Convert.ToBoolean(this.DESheetInformation.SectorGlobal);
                    }

                    sQry = DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.InsertIC(DBQueries.DataPrefix, DBQueries.LanguageCode, this.DESheetInformation.SectorName.Replace("'", "''"), this.DESheetInformation.IndicatorGUID,
                    SectorGlobal, -1, "", ICType.Sector);

                    //-- Using DAL for query execution and getting identity value 
                    DBConnection.ExecuteNonQuery(sQry);
                    RetVal = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                }
            }
            catch (Exception ex)
            {
                RetVal = 0;
            }
            bSectorGlobal = SectorGlobal;
            bNewSector = NewSector;
            return RetVal;
        }

        private int GetClassNId(DIConnection DBConnection, DIQueries DBQueries, out bool bNewClass, String classGUID, Boolean SectorGlobal, int iParentNId, String className)
        {
            int RetVal = 0;
            string sQry = string.Empty;
            Boolean NewClass = false;
            try
            {
                //-- Getting Query from DAL           
                sQry = DBQueries.Calculates.GetICNIdByGId("'" + classGUID + "'");

                //-- Executing Query 
                RetVal = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));

                if (RetVal == 0)
                {
                    NewClass = true;

                    // -- Getting insert Query 
                    sQry = DevInfo.Lib.DI_LibDAL.Queries.IndicatorClassification.Insert.InsertIC(DBQueries.DataPrefix, DBQueries.LanguageCode, className.Replace("'", "''"), classGUID, SectorGlobal, iParentNId, "", ICType.Sector);

                    //-- Using DAL for query execution and getting identity value 
                    DBConnection.ExecuteNonQuery(sQry);
                    RetVal = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                }
            }
            catch (Exception ex)
            {
                RetVal = 0;
            }
            bNewClass = NewClass;
            return RetVal;
        }

        #region " -- Helper Methods for insert New Records-- "

        /// <summary>
        /// Check is a valid date
        /// </summary>
        /// <param name="sDate"></param>
        /// <returns></returns>
        private bool CheckDate(string sDate)
        {
            string[] sDatePart;
            //*** Valid Formats 
            //yyyy 
            //yyyy.mm 
            //yyyy.mm.dd 
            //yyyy-yyyy 
            //yyyy.mm-yyyy.mm 
            //yyyy.mm.dd-yyyy.mm.dd 

            switch (sDate.Length)
            {
                case 4:
                    //*** YYYY 
                    if (!Information.IsNumeric(sDate))
                        return false;
                    break;
                case 7:
                    //*** YYYY.MM 
                    if (sDate.IndexOf(".") != 4)
                        return false;

                    if (sDate.IndexOf(".") != sDate.LastIndexOf("."))
                        return false;

                    sDatePart = sDate.Split('.');
                    if (Information.IsNumeric(sDatePart[0].ToString()) && Information.IsNumeric(sDatePart[1].ToString()))
                    {
                        if (Convert.ToInt32(sDatePart[1]) < 1 | Convert.ToInt32(sDatePart[1]) > 12)
                            return false;
                    }
                    else
                    {
                        return false;
                    }

                    break;
                case 9:
                    //*** YYYY-YYYY 
                    if (sDate.IndexOf("-") != 4)
                        return false;

                    //Bugfix 2007-10-25 improper check for time period format YYYY-YYYY 
                    //If sDate.IndexOf("-") <> sDate.LastIndexOf(".") Then Return False 
                    sDatePart = sDate.Split('-');
                    if (Information.IsNumeric(sDatePart[0].ToString()) & Information.IsNumeric(sDatePart[1].ToString()))
                    {
                        if (Convert.ToInt32(sDatePart[0]) > Convert.ToInt32(sDatePart[1]))
                            return false;
                    }
                    else
                    {
                        return false;
                    }

                    break;
                case 10:
                    //*** YYYY.MM.DD 
                    if (sDate.IndexOf(".") != 4)
                        return false;

                    if (sDate.LastIndexOf(".") != 7)
                        return false;

                    sDatePart = sDate.Split('.');
                    if (Information.IsNumeric(sDatePart[0].ToString()) && Information.IsNumeric(sDatePart[1].ToString()) & Information.IsNumeric(sDatePart[2].ToString()))
                    {
                        if (Convert.ToInt32(sDatePart[1]) < 1 || Convert.ToInt32(sDatePart[1]) > 12)
                        {
                            return false;
                        }
                        if (Convert.ToInt32(sDatePart[2]) < 1)
                        {
                            return false;
                        }
                        switch (Convert.ToInt32(sDatePart[2]))
                        {
                            case 1:
                            case 3:
                            case 5:
                            case 7:
                            case 8:
                            case 10:
                            case 12:
                                if (Convert.ToInt32(sDatePart[2]) > 31)
                                    return false;

                                break;
                            case 4:
                            case 6:
                            case 9:
                            case 11:
                                if (Convert.ToInt32(sDatePart[2]) > 30)
                                    return false;

                                break;
                            case 2:
                                if (System.DateTime.IsLeapYear(Convert.ToInt32(sDatePart[0])))
                                {
                                    if (Convert.ToInt32(sDatePart[2]) > 29)
                                        return false;
                                }
                                else
                                {
                                    if (Convert.ToInt32(sDatePart[2]) > 28)
                                        return false;
                                }

                                break;
                        }
                    }
                    else
                    {
                        return false;
                    }

                    break;
                case 15:
                    //*** yyyy.mm-yyyy.mm 
                    if (sDate.IndexOf('-') != 7)
                        return false;


                    string[] TempDate;
                    TempDate = sDate.Split('-');

                    if (TempDate[0].IndexOf('.') != 4)
                        return false;

                    if (TempDate[0].IndexOf(".") != TempDate[0].LastIndexOf("."))
                        return false;

                    sDatePart = TempDate[0].Split('.');
                    if (Information.IsNumeric(sDatePart[0].ToString()) & Information.IsNumeric(sDatePart[1].ToString()))
                    {
                        if (Convert.ToInt32(sDatePart[1]) < 1 || Convert.ToInt32(sDatePart[1]) > 12)
                            return false;
                    }
                    else
                    {
                        return false;
                    }


                    if (TempDate[1].ToString().IndexOf(".") != 4)
                        return false;

                    if (TempDate[1].ToString().IndexOf(".") != TempDate[1].ToString().LastIndexOf("."))
                        return false;

                    sDatePart = TempDate[1].ToString().Split('.');
                    if (Information.IsNumeric(sDatePart[0].ToString()) && Information.IsNumeric(sDatePart[1].ToString()))
                    {
                        if (Convert.ToInt32(sDatePart[1]) < 1 || Convert.ToInt32(sDatePart[1]) > 12)
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }

                    break;

                case 21:
                    //*** yyyy.mm.dd-yyyy.mm.dd 

                    if (sDate.IndexOf("-") != 10)
                        return false;


                    //string[] TempDate;
                    TempDate = sDate.Split('-');

                    if (TempDate[0].ToString().IndexOf('.') != 4)
                        return false;

                    if (TempDate[0].ToString().LastIndexOf(".") != 7)
                        return false;

                    sDatePart = TempDate[0].Split('.');
                    if (Information.IsNumeric(sDatePart[0].ToString()) && Information.IsNumeric(sDatePart[1].ToString()) & Information.IsNumeric(sDatePart[2].ToString()))
                    {
                        if (Convert.ToInt32(sDatePart[1]) < 1 || Convert.ToInt32(sDatePart[1]) > 12)
                        {
                            return false;
                        }
                        if (Convert.ToInt32(sDatePart[2]) < 1)
                        {
                            return false;
                        }
                        switch (Convert.ToInt32(sDatePart[2]))
                        {
                            case 1:
                            case 3:
                            case 5:
                            case 7:
                            case 8:
                            case 10:
                            case 12:
                                if (Convert.ToInt32(sDatePart[2]) > 31)
                                {
                                    return false;
                                }
                                break;
                            case 4:
                            case 6:
                            case 9:
                            case 11:
                                if (Convert.ToInt32(sDatePart[2]) > 30)
                                    return false;
                                break;
                            case 2:
                                if (System.DateTime.IsLeapYear(Convert.ToInt32(sDatePart[0])))
                                {
                                    if (Convert.ToInt32(sDatePart[2]) > 29)
                                    {
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (Convert.ToInt32(sDatePart[2]) > 28)
                                    {
                                        return false;
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        return false;
                    }


                    if (TempDate[1].ToString().IndexOf(".") != 4)
                    {
                        return false;
                    }
                    if (TempDate[1].ToString().LastIndexOf(".") != 7)
                    {
                        return false;
                    }
                    sDatePart = TempDate[1].Split('.');
                    if (Information.IsNumeric(sDatePart[0].ToString()) && Information.IsNumeric(sDatePart[1].ToString()) && Information.IsNumeric(sDatePart[2].ToString()))
                    {
                        if (Convert.ToInt32(sDatePart[1]) < 1 || Convert.ToInt32(sDatePart[1]) > 12)
                        {
                            return false;
                        }
                        if (Convert.ToInt32(sDatePart[2]) < 1)
                        {
                            return false;
                        }
                        switch (Convert.ToInt32(sDatePart[2]))
                        {
                            case 1:
                            case 3:
                            case 5:
                            case 7:
                            case 8:
                            case 10:
                            case 12:
                                if (Convert.ToInt32(sDatePart[2]) > 31)
                                {
                                    return false;
                                }
                                break;
                            case 4:
                            case 6:
                            case 9:
                            case 11:
                                if (Convert.ToInt32(sDatePart[2]) > 30)
                                {
                                    return false;
                                }
                                break;
                            case 2:
                                if (System.DateTime.IsLeapYear(Convert.ToInt32(sDatePart[0])))
                                {
                                    if (Convert.ToInt32(sDatePart[2]) > 29)
                                        return false;
                                }
                                else
                                {
                                    if (Convert.ToInt32(sDatePart[2]) > 28)
                                        return false;
                                }

                                break;
                        }
                    }
                    else
                    {
                        return false;
                    }

                    break;
                default:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Set Date
        /// </summary>
        /// <param name="sTimePeriod"></param>
        /// <param name="dtStartDate"></param>
        /// <param name="dtEndDate"></param>
        private void SetDate(string sTimePeriod, ref System.DateTime dtStartDate, ref System.DateTime dtEndDate)
        {
            string RetVal = string.Empty;
            string[] sDate;
            //*** BugFix 12 Apr 2006 Problem with dd-MMM-yyyy format 
            //*** BugFix 21 Feb 2006 Problem with different Reginal Settings - Thai, Arabic etc 
            System.Globalization.CultureInfo ociEnUS = new System.Globalization.CultureInfo("en-US", false);
            try
            {
                switch (sTimePeriod.Length)
                {
                    case 4:
                        //YYYY 
                        dtStartDate = DateTime.Parse("1/1/" + sTimePeriod, ociEnUS);
                        dtEndDate = DateTime.Parse("12/31/" + sTimePeriod, ociEnUS);
                        break;
                    case 7:
                        //yyyy.MM 
                        sDate = sTimePeriod.Split('.');
                        dtStartDate = DateTime.Parse(sDate[1] + "/1/" + sDate[0], ociEnUS);
                        dtEndDate = DateTime.Parse(sDate[1] + "/" + DateTime.DaysInMonth(Convert.ToInt32(sDate[0]), Convert.ToInt32(sDate[1])) + "/" + Convert.ToInt32(sDate[0]), ociEnUS);
                        break;
                    case 9:
                        //YYYY-YYYY 
                        if (sTimePeriod.IndexOf('.') == -1)
                        {
                            sDate = sTimePeriod.Split('-');
                            dtStartDate = DateTime.Parse("1/1/" + Convert.ToInt32(sDate[0]), ociEnUS);
                            dtEndDate = DateTime.Parse("12/31/" + Convert.ToInt32(sDate[1]), ociEnUS);
                        }
                        else
                        {
                            sDate = sTimePeriod.Split('.');
                            dtStartDate = DateTime.Parse("1/1/" + sDate[0], ociEnUS);
                            dtEndDate = DateTime.Parse("12/31/" + sDate[1], ociEnUS);
                        }

                        break;
                    case 10:
                        //YYYY.MM.DD 
                        sDate = sTimePeriod.Split('.');
                        dtStartDate = DateTime.Parse(sDate[1] + "/" + sDate[2] + "/" + sDate[0], ociEnUS);
                        dtEndDate = DateTime.Parse(sDate[1] + "/" + sDate[2] + "/" + sDate[0], ociEnUS);
                        break;
                    case 15:
                        //yyyy.mm-yyyy.mm 
                        string[] sTempDate;
                        sTempDate = sTimePeriod.Split('-');
                        sDate = sTempDate[0].Split('.');
                        dtStartDate = DateTime.Parse(sDate[1] + "/1/" + sDate[0], ociEnUS);
                        sDate = sTempDate[1].Split('.');
                        dtEndDate = DateTime.Parse(sDate[1] + "/" + DateTime.DaysInMonth(Convert.ToInt32(sDate[0]), Convert.ToInt32(sDate[1])) + "/" + Convert.ToInt32(sDate[0]), ociEnUS);
                        break;
                    case 21:
                        //yyyy.mm.dd-yyyy.mm.dd 

                        sTempDate = sTimePeriod.Split('-');
                        sDate = sTempDate[0].Split('.');
                        dtStartDate = DateTime.Parse(sDate[1] + "/" + sDate[2] + "/" + sDate[0], ociEnUS);
                        sDate = sTempDate[1].Split('.');
                        dtEndDate = DateTime.Parse(sDate[1] + "/" + sDate[2] + "/" + sDate[0], ociEnUS);
                        break;
                }
            }
            catch (Exception ex)
            {
                dtStartDate = DateTime.Parse(Map.DEFAULT_START_DATE, ociEnUS);
                dtEndDate = DateTime.Parse(Map.DEFAULT_END_DATE, ociEnUS);
            }

        }

        #endregion

        #endregion
        #endregion

        #region " -- Public -- "

        #region " -- Properties -- "

        private DataTable _Step1SelectionDataTable = null;
        /// <summary>
        /// Gets or sets DataTable containing Step1 Selections
        /// </summary>
        public DataTable Step1SelectionDataTable
        {
            get
            {
                return this._Step1SelectionDataTable;
            }
            set
            {
                this._Step1SelectionDataTable = value;
            }
        }

        private DataTable _Step2SelectionDataTable = null;
        /// <summary>
        /// Gets or sets DataTable containing Step2 Selections
        /// </summary>
        public DataTable Step2SelectionDataTable
        {
            get
            {
                return this._Step2SelectionDataTable;
            }
            set
            {
                this._Step2SelectionDataTable = value;
            }
        }

        private DataView _PresentationData = null;
        /// <summary>
        /// Gets or sets presentation Data
        /// </summary>
        public DataView PresentationData
        {
            get
            {
                return this._PresentationData;
            }
            set
            {
                //Bug Fixed: Remove RowFilter before assigning new value to _PresentaionData DataView
                if (this._PresentationData !=null)
                {
                    this._PresentationData.RowFilter = "";
                }
                this._PresentationData = value;
            }
        }

        private string _LogFilePath = string.Empty;
        /// <summary>
        /// Gets or sets file path  of log template file
        /// </summary>
        public string LogFilePath
        {
            get
            {
                return this._LogFilePath;
            }
            set
            {
                this._LogFilePath = value;
            }
        }

        private string _TempLogFilePath = string.Empty;
        /// <summary>
        /// Gets or sets the temp location where log file will be temporarily copied 
        /// </summary>
        public string TempLogFilePath
        {
            get
            {
                return this._TempLogFilePath;
            }
            set
            {
                this._TempLogFilePath = value;
            }
        }

        private string _DESFilePath = string.Empty;
        /// <summary>
        /// Gets or sets Data Entry spreadsheet File path
        /// </summary>
        public string DESFilePath
        {
            get
            {
                return this._DESFilePath;
            }
            set
            {
                this._DESFilePath = value;
            }
        }

        private string _TempDESFilePath;
        /// <summary>
        /// Gets or sets temp Dataentry spreadsheet File path
        /// </summary>
        public string TempDESFilePath
        {
            get
            {
                return this._TempDESFilePath;
            }
            set
            {
                this._TempDESFilePath = value;
            }
        }


        private int _TitleFontSize = 10;
        /// <summary>
        /// Gets or Sets title font size. Default is 10
        /// </summary>
        public int TitleFontSize
        {
            get
            {
                return this._TitleFontSize;
            }
            set
            {
                this._TitleFontSize = value;
            }
        }

        private Single _FontSize = 8.0F;
        /// <summary>
        /// Gets or Sets font size.default is 8.0
        /// </summary>
        public Single FontSize
        {
            get
            {
                return this._FontSize;
            }
            set
            {
                this._FontSize = value;
            }
        }

        private string _FontName = "Arial";
        /// <summary>
        /// Gets or sets font name. Default is Arial
        /// </summary>
        public string FontName
        {
            get
            {
                return this._FontName;
            }
            set
            {
                this._FontName = value;
            }
        }

        private Decimal _DecimalUpDownControlValue = 0;
        /// <summary>
        /// Gets or sets UpDownControlValues for decimal point value
        /// </summary>
        public Decimal DecimalUpDownControlValue
        {
            get
            {
                return this._DecimalUpDownControlValue;
            }
            set
            {
                this._DecimalUpDownControlValue = value;
            }
        }

        private string _LanguageFilePath = string.Empty;
        /// <summary>
        /// Gets and sets Language File Path
        /// </summary>
        public string LangFilePath
        {
            get
            {
                return this._LanguageFilePath;
            }
            set
            {
                this._LanguageFilePath = value;
            }
        }

        private WizardType _ApplicationWizardType = WizardType.Percent;
        /// <summary>
        /// Gets or sets wizard type.Default is percent
        /// </summary>
        public WizardType ApplicationWizardType
        {
            get
            {
                return this._ApplicationWizardType;
            }
            set
            {
                this._ApplicationWizardType = value;
            }
        }

        private DESheetInfo _DESheetInformation = new DESheetInfo();
        /// <summary>
        /// Gets or sets DESheetInformation
        /// </summary>
        public DESheetInfo DESheetInformation
        {
            get
            {
                return this._DESheetInformation;
            }
            set
            {
                this._DESheetInformation = value;
            }
        }


        private string[] _DESSubgroupText;
        /// <summary>
        /// Gets or sets subgroup text array for DataEntrySpreedSheet
        /// </summary>
        /// <remarks>this the text typed for formula unit in step3 unit text box</remarks>
        public string[] DESSubgroupText
        {
            get
            {
                return _DESSubgroupText;
            }
            set
            {
                _DESSubgroupText = value;
            }
        }


        private string _DESIndicatorGUId;
        /// <summary>
        /// Gets or sets Indicator GUID for DES
        /// </summary>
        public string DESIndicatorGUId
        {
            get
            {
                return _DESIndicatorGUId;
            }
            set
            {
                _DESIndicatorGUId = value;
            }
        }


        private string _DESUnitGUId;
        /// <summary>
        /// Gets or sets unit GUID for DES
        /// </summary>
        public string DESUnitGUId
        {
            get
            {
                return _DESUnitGUId;
            }
            set
            {
                _DESUnitGUId = value;
            }
        }

        private string[] _DESSubgroupGUId;
        /// <summary>
        /// Gets or sets Subgroup GUID for DES
        /// </summary>
        public string[] DESSubgroupGUId
        {
            get
            {
                return _DESSubgroupGUId;
            }
            set
            {
                _DESSubgroupGUId = value;
            }
        }

        private DIConnection _DBConnection = null;
        /// <summary>
        /// Gets or set Dbconnection
        /// </summary>
        public DIConnection DBConnection
        {
            get { return _DBConnection; }
            set { _DBConnection = value; }
        }

        private DIQueries _DBQueries;
        /// <summary>
        /// Gets or set DIqueries for claculate class
        /// </summary>
        public DIQueries DBQueries
        {
            get
            {
                return this._DBQueries;
            }
            set
            {
                this._DBQueries = value;
            }
        }

        private int _SelectedClassificationIndex;
        /// <summary>
        /// Gets or sets step 3 selected IC index
        /// </summary>
        public int SelectedClassificationIndex
        {
            get
            {
                return _SelectedClassificationIndex;
            }
            set
            {
                _SelectedClassificationIndex = value;
            }
        }

        private string _DESDecimal;
        /// <summary>
        /// Gets or sets data entry spreadsheet Decimal value
        /// </summary>
        public string DESDecimal
        {
            get
            {
                return _DESDecimal;
            }
            set
            {
                _DESDecimal = value;
            }
        }

        private ArrayList _SelectedNids;
        /// <summary>
        /// Gets or sets Selected NIds
        /// </summary>
        public ArrayList SelectedNids
        {
            get
            {
                return _SelectedNids;
            }
            set
            {
                _SelectedNids = value;
            }
        }

        private int _DESLastDataRowIndex;

        /// <summary>
        /// Gets or sets index of last row of data entry sheet to update  miCtrFillTo in client Application
        /// </summary>
        public int DESLastDataRowIndex
        {
            get
            {
                return _DESLastDataRowIndex;
            }
            set
            {
                _DESLastDataRowIndex = value;
            }
        }


        #endregion

        #region " -- Methods -- "
        /// <summary>
        /// Perform Fill Percent Summary calculation
        /// </summary>
        /// <param name="subgroupText"></param>
        public void FillPercentSummaryLog(string subgroupText)
        {
            DataView dvMain = null;
            // dvMain.RowFilter = string.Empty;
            try
            {
                //Apply Language Setting
                ApplyLanguageSettings();

                //Raise event for wait cursor            
                this.RaiseEventSetCursor(false);

                int i;
                int j;
                int k = 0;
                string sFilter = string.Empty;
                int CtrFillFrom = 0;
                int CtrFillTo = 0;
                int iSelCount = Step1SelectionDataTable.Rows.Count;
                int iRecordCount = 0;
                string TempStringForAddress = string.Empty;
                String[,] msArray = new String[1, 9];   // Log sheet array
                // Indicator and  Subgroup val NIds will be used for SSel_NId
                //string[] sSel_NId;
                string Sel_Indicator_NId = string.Empty;
                string Sel_Subgroup_NId = string.Empty;

                ArrayList sSel_Indicator_NId = new ArrayList();
                ArrayList sSel_Subgroup_Val_NId = new ArrayList();

                //Variable for fill data value and formula
                string sChageValue = string.Empty;
                string sAddress = string.Empty;
                string sNum = string.Empty;
                string sDen = string.Empty;
                string sMinRange = string.Empty;
                string sMaxRange = string.Empty;
                SpreadsheetGear.IRange oRngStart;
                SpreadsheetGear.IRange oRngTotal;
                int iDataCol = 0;
                bool SupressArea = false;
                //Dim NumTotal, DenTotal As Single
                double NumTotal;
                double DenTotal;
                int FirstNumIndex;

                // Raise Event for initialize Progress Bar 
                RaiseEventSetProgress(1);

                // Copy log file to temp location
                if (File.Exists(LogFilePath))
                {
                    File.Copy(LogFilePath, TempLogFilePath, true);
                    System.IO.File.SetAttributes(TempLogFilePath, FileAttributes.Normal);
                }
                // Getting workbook  using BAL DIExcel
                this.DIExcel = new DIExcel(TempLogFilePath, System.Globalization.CultureInfo.CurrentCulture);

                // Getting worksheets 1.Summery sheet 2. LogSheet
                SummarySheet = DIExcel.GetWorksheet(0);
                LogSheet = DIExcel.GetWorksheet(1);

                // Setting Lang based Name of worksheets
                SummarySheet.Name = LangStrings.SUMMARY;
                LogSheet.Name = LangStrings.LOG;
                RaiseEventSetProgress(10);

                // Filling General info of summery sheet
                SummarySheet.Cells[0, 0].Value = LangStrings.VAR_WIZARD_NAME;
                SummarySheet.Cells[2, 0].Value = LangStrings.sMODULE;
                SummarySheet.Cells[2, 0].Value = LangStrings.VAR_WIZARD_NAME;
                SummarySheet.Cells[3, 0].Value = LangStrings.DATE_TIME;
                SummarySheet.Cells[3, 1].Value = TimeStamp;
                SummarySheet.Cells[4, 0].Value = LangStrings.LOG_FILE_NAME;

                SummarySheet.Cells[4, 1].Value = "Log_Percent (" + TimeStamp + ").xls";
                SummarySheet.Cells[6, 0].Value = LangStrings.DECIMALPLACES;
                SummarySheet.Cells[6, 1].Value = this.DecimalUpDownControlValue;
                SummarySheet.Cells[7, 2].Value = LangStrings.NUMERATOR;
                SummarySheet.Cells[7, 2 + Step1SelectionDataTable.Rows.Count + 1].Value = LangStrings.DENOMINATOR;
                SummarySheet.Cells[7, 2 + Step1SelectionDataTable.Rows.Count + 1 + Step2SelectionDataTable.Rows.Count + 1].Value = LangStrings.TARGET;
                SummarySheet.Cells[10, 0].Value = LangStrings.AREAID;
                SummarySheet.Cells[10, 1].Value = LangStrings.AREANAME;

                // Raise Event to notify progress                
                RaiseEventSetProgress(25);

                //*** Get Distinct Selected IndicatorNId & Subgroup_Val_NId From Step1 and Step2  and fill in summary sheet
                //Loop for all Indicator row selected in step1                             
                for (i = 0; i <= Step1SelectionDataTable.Rows.Count - 1; i++)
                {
                    // Set selected indicator and subgroup val name in excel c10 and c11  and so on
                    //If second indicator and sg are selected at step 1 its values goes in d10 and d11
                    SummarySheet.Cells[9, 2 + i].Value = Step1SelectionDataTable.Rows[i][Indicator.IndicatorName].ToString(); // indicator text                     
                    SummarySheet.Cells[10, 2 + i].Value = Step1SelectionDataTable.Rows[i][SubgroupVals.SubgroupVal].ToString(); // subgroupVal text 
                    sSel_Indicator_NId.Add(Step1SelectionDataTable.Rows[i][Indicator.IndicatorNId].ToString());
                    sSel_Subgroup_Val_NId.Add(Step1SelectionDataTable.Rows[i][SubgroupVals.SubgroupValNId].ToString());
                }

                // Set total caption
                SummarySheet.Cells[10, 2 + i].Value = LangStrings.TOTAL;

                //--------------------------------------------------------------------
                //Working with step2 selection (Selected Denominator).Entering them in excel sheet
                for (i = 0; i <= Step2SelectionDataTable.Rows.Count - 1; i++)
                {

                    SummarySheet.Cells[9, 2 + Step1SelectionDataTable.Rows.Count + 1 + i].Value = Step2SelectionDataTable.Rows[i][Indicator.IndicatorName].ToString(); // indicator text 
                    SummarySheet.Cells[10, 2 + Step1SelectionDataTable.Rows.Count + 1 + i].Value = Step2SelectionDataTable.Rows[i][SubgroupVals.SubgroupVal].ToString();

                    // update distinct indicator NId 
                    if (sSel_Indicator_NId.Contains(Step2SelectionDataTable.Rows[i][Indicator.IndicatorNId].ToString()) == false)
                    {
                        sSel_Indicator_NId.Add(Step2SelectionDataTable.Rows[i][Indicator.IndicatorNId].ToString());
                    }

                    // update distinct indicator NId and subgroup NIDs
                    if (sSel_Subgroup_Val_NId.Contains(Step2SelectionDataTable.Rows[i][SubgroupVals.SubgroupValNId].ToString()) == false)
                    {
                        sSel_Subgroup_Val_NId.Add(Step2SelectionDataTable.Rows[i][SubgroupVals.SubgroupValNId].ToString());
                    }
                }
                SummarySheet.Cells[10, 2 + Step1SelectionDataTable.Rows.Count + 1 + i].Value = LangStrings.TOTAL;
                i += 1;
                SummarySheet.Cells[9, 2 + Step1SelectionDataTable.Rows.Count + 1 + i].Value = DataEntrySheetInfo.Indicator;
                SummarySheet.Cells[10, 2 + Step1SelectionDataTable.Rows.Count + 1 + i].Value = DESheetInformation.Unit;// DESUnitText;

                //*** Get the data view with all filters applied 
                dvMain = this.PresentationData;

                //*** Filter records for selected Indicator / Subgroup in calculates step1                 
                if (dvMain.RowFilter == "")
                {
                    dvMain.RowFilter = Indicator.IndicatorNId + " IN (" + string.Join(",", (string[])sSel_Indicator_NId.ToArray(typeof(string))) + ") AND " + SubgroupVals.SubgroupValNId + " IN (" + string.Join(",", (string[])sSel_Subgroup_Val_NId.ToArray(typeof(string))) + ")";
                }
                else
                {
                    dvMain.RowFilter += " AND " + Indicator.IndicatorNId + " IN (" + String.Join(",", (string[])sSel_Indicator_NId.ToArray(typeof(string))) + ") AND " + SubgroupVals.SubgroupValNId + " IN (" + string.Join(",", (string[])sSel_Subgroup_Val_NId.ToArray(typeof(string))) + ")";
                }
                sFilter = dvMain.RowFilter;

                //Fill Unique Area                 

                dvMain.Sort = Area.AreaID + " ASC, " + Timeperiods.TimePeriod + " DESC";
                CtrFillFrom = 12;//13; 
                for (i = 0; i <= dvMain.Count - 1; i++)
                {
                    if (dvMain[i][Area.AreaID].ToString() != SummarySheet.Cells[iRecordCount + CtrFillFrom - 1, 0].Value + "")
                    {
                        SummarySheet.Cells[iRecordCount + CtrFillFrom, 0].Value = dvMain[i][Area.AreaID].ToString();
                        SummarySheet.Cells[iRecordCount + CtrFillFrom, 1].Value = dvMain[i][Area.AreaName].ToString();
                        iRecordCount += 1;
                    }
                }
                // Getting Last Fill Column.
                CtrFillTo = CtrFillFrom + iRecordCount - 1;

                //Raise Event to notify Progress
                RaiseEventSetProgress(35);

                //*** Set Data Array for Log Sheet simultaneously                             
                msDataArray = new String[iRecordCount, 6];
                msArray = new String[(iRecordCount * (Step1SelectionDataTable.Rows.Count + Step2SelectionDataTable.Rows.Count + 2)) + 2, 9];
                msArray[0, 0] = LangStrings.AREAID;
                msArray[0, 1] = LangStrings.AREANAME;
                msArray[0, 2] = LangStrings.ELEMENT;
                msArray[0, 3] = LangStrings.INDICATOR;
                msArray[0, 4] = LangStrings.SUBGROUP;
                msArray[0, 5] = LangStrings.TIME;
                msArray[0, 6] = LangStrings.DATAVALUE;
                msArray[0, 7] = LangStrings.UNIT;
                msArray[0, 8] = LangStrings.SOURCE;
                k += 2;

                //Fill Data Values and Formula for each records 
                for (i = CtrFillFrom; i <= CtrFillTo; i++)
                {

                    //*** Fill Datavalue for Numerator Indicator against each AreaID 
                    NumTotal = 0;
                    DenTotal = 0;
                    FirstNumIndex = 0;
                    for (j = 0; j <= Step1SelectionDataTable.Rows.Count - 1; j++)
                    {

                        iDataCol = 3 + j;
                        dvMain.RowFilter = sFilter + " AND " + Area.AreaID + " = '" + Utility.DICommon.EscapeWildcardChar(Utility.DICommon.RemoveQuotes(SummarySheet.Cells[i, 0].Value.ToString())) + "'";

                        // Getting Indicator and subgroup Val NId                         
                        Sel_Indicator_NId = Step1SelectionDataTable.Rows[j][Indicator.IndicatorNId].ToString();
                        Sel_Subgroup_NId = Step1SelectionDataTable.Rows[j][SubgroupVals.SubgroupValNId].ToString();

                        dvMain.RowFilter += " AND " + Indicator.IndicatorNId + " =" + Sel_Indicator_NId + " AND " + SubgroupVals.SubgroupValNId + " =" + Sel_Subgroup_NId;

                        if (dvMain.Count > 0)
                        {
                            if (FirstNumIndex == 0)
                            {
                                FirstNumIndex = k;
                            }
                            try
                            {
                                SummarySheet.Cells[i, iDataCol - 1].Value = dvMain[0][Data.DataValue].ToString();
                                NumTotal += Convert.ToDouble(dvMain[0][Data.DataValue].ToString());
                            }
                            catch (Exception ex)
                            {
                            }

                            if (SupressArea == false)
                            {
                                msArray[k, 0] = dvMain[0][Area.AreaID].ToString();
                                msArray[k, 1] = dvMain[0][Area.AreaName].ToString();
                                msDataArray[i - 12, 0] = dvMain[0][Timeperiods.TimePeriod].ToString();
                                msDataArray[i - 12, 1] = dvMain[0][Area.AreaID].ToString();
                                msDataArray[i - 12, 2] = dvMain[0][Area.AreaName].ToString();
                                msDataArray[i - 12, 3] = dvMain[0][Data.DataValue].ToString();

                                //msDataArray[i - 13, 4] = cbo1_3_Subgroup.Text;
                                msDataArray[i - 12, 4] = subgroupText;

                                msDataArray[i - 12, 5] = dvMain[0][IndicatorClassifications.ICName].ToString();
                                SupressArea = true;
                            }
                            msArray[k, 2] = LangStrings.NUMERATOR;
                            msArray[k, 3] = dvMain[0][Indicator.IndicatorName].ToString();
                            msArray[k, 4] = dvMain[0][SubgroupVals.SubgroupVal].ToString();
                            msArray[k, 5] = dvMain[0][Timeperiods.TimePeriod].ToString();
                            msArray[k, 6] = dvMain[0][Data.DataValue].ToString();
                            msArray[k, 7] = dvMain[0][Unit.UnitName].ToString();
                            msArray[k, 8] = dvMain[0][IndicatorClassifications.ICName].ToString();
                            k += 1;
                        }
                    }

                    //*** Fill Datavalue for Denominator Indicator against each AreaID 
                    for (j = 0; j <= Step2SelectionDataTable.Rows.Count - 1; j++)
                    {
                        iDataCol = 3 + Step1SelectionDataTable.Rows.Count + 1 + j;
                        dvMain.RowFilter = sFilter + " AND " + Area.AreaID + " = '" + Utility.DICommon.EscapeWildcardChar(Utility.DICommon.RemoveQuotes(SummarySheet.Cells[i, 0].Value.ToString())) + "'";

                        //sSel_NId = lv1_2_Selected.Items(j).Tag.ToString.Split("_");
                        Sel_Indicator_NId = Step2SelectionDataTable.Rows[j][Indicator.IndicatorNId].ToString();
                        Sel_Subgroup_NId = Step2SelectionDataTable.Rows[j][SubgroupVals.SubgroupValNId].ToString();

                        dvMain.RowFilter += " AND " + Indicator.IndicatorNId + " =" + Sel_Indicator_NId + " AND " + SubgroupVals.SubgroupValNId + " =" + Sel_Subgroup_NId;

                        if (dvMain.Count > 0)
                        {
                            SummarySheet.Cells[i, iDataCol - 1].Value = dvMain[0][Data.DataValue].ToString();

                            try
                            {
                                DenTotal += Convert.ToDouble(dvMain[0][Data.DataValue].ToString());
                            }
                            catch (Exception ex)
                            {
                            }

                            if (SupressArea == false)
                            {
                                msArray[k, 0] = dvMain[0][Area.AreaID].ToString();
                                msArray[k, 1] = dvMain[0][Area.AreaName].ToString();
                                msDataArray[i - 12, 0] = dvMain[0][Timeperiods.TimePeriod].ToString();
                                msDataArray[i - 12, 1] = dvMain[0][Area.AreaID].ToString();
                                msDataArray[i - 12, 2] = dvMain[0][Area.AreaName].ToString();

                                //msDataArray[i - 13, 4] = cbo1_3_Subgroup.Text;
                                msDataArray[i - 12, 3] = subgroupText;

                                msDataArray[i - 12, 4] = dvMain[0][IndicatorClassifications.ICName].ToString();
                                SupressArea = true;
                            }
                            msArray[k, 2] = LangStrings.DENOMINATOR;
                            //*** Bugfix Feb 06 Hardcoded Numerator instead of Denominator 
                            msArray[k, 3] = dvMain[0][Indicator.IndicatorName].ToString();
                            msArray[k, 4] = dvMain[0][SubgroupVals.SubgroupVal].ToString();
                            msArray[k, 5] = dvMain[0][Timeperiods.TimePeriod].ToString();
                            msArray[k, 6] = dvMain[0][Data.DataValue].ToString();
                            msArray[k, 7] = dvMain[0][Unit.UnitName].ToString();
                            msArray[k, 8] = dvMain[0][IndicatorClassifications.ICName].ToString();
                            k += 1;
                        }
                    }

                    if (FirstNumIndex > 0)
                    {
                        msArray[k, 2] = LangStrings.PERCENT;
                        msArray[k, 3] = msArray[FirstNumIndex, 3];
                        msArray[k, 4] = msArray[FirstNumIndex, 4];
                        msArray[k, 5] = msArray[FirstNumIndex, 5];
                        if (DenTotal != 0)
                            msArray[k, 6] = Convert.ToString(Math.Round((100 * NumTotal / DenTotal), (int)DecimalUpDownControlValue));
                        msArray[k, 7] = LangStrings.PERCENT;
                        msArray[k, 8] = msArray[FirstNumIndex, 8];
                    }

                    k += 2;
                    SupressArea = false;
                }

                //*** Set the formula for Numerator Total 
                sChageValue = "";
                for (j = 0; j <= Step1SelectionDataTable.Rows.Count - 1; j++)
                {
                    iDataCol = 3 + j;
                    if (Strings.Trim(sChageValue) == "")
                    {
                        sChageValue = SummarySheet.Cells[CtrFillFrom, iDataCol - 1].Address.Replace("$", "");
                    }
                    else
                    {
                        TempStringForAddress = SummarySheet.Cells[CtrFillFrom, iDataCol - 1].Address.Replace("$", "");
                        sChageValue = sChageValue + " + " + TempStringForAddress;
                    }
                }
                sChageValue = "IF(" + sChageValue + "=\"\",\"\"," + sChageValue + ")";
                sAddress = SummarySheet.Cells[CtrFillFrom, iDataCol].Address;
                SummarySheet.Range[sAddress].Formula = "= " + sChageValue;

                TempStringForAddress = SummarySheet.Cells[CtrFillFrom, iDataCol].Address.Replace("$", "");
                oRngStart = SummarySheet.Range[TempStringForAddress];
                string TempStringForAddress2 = SummarySheet.Cells[CtrFillTo, iDataCol].Address.Replace("$", "");
                if (TempStringForAddress != TempStringForAddress2)
                {
                    sMaxRange = TempStringForAddress + ":" + TempStringForAddress2;
                    oRngTotal = SummarySheet.Range[sMaxRange];
                    AutoFillRange(oRngStart, oRngTotal);
                }

                //*** Set the formula for Denominator Total 
                sChageValue = "";
                for (j = 0; j <= Step2SelectionDataTable.Rows.Count - 1; j++)
                {
                    iDataCol = 3 + Step1SelectionDataTable.Rows.Count + 1 + j;
                    if (Strings.Trim(sChageValue) == "")
                    {
                        sChageValue = SummarySheet.Cells[CtrFillFrom, iDataCol - 1].Address.Replace("$", "");
                    }
                    else
                    {
                        TempStringForAddress = SummarySheet.Cells[CtrFillFrom, iDataCol - 1].Address.Replace("$", "");
                        sChageValue = sChageValue + " + " + TempStringForAddress;
                    }
                }
                sChageValue = "IF(" + sChageValue + "=\"\",\"\"," + sChageValue + ")";
                sAddress = SummarySheet.Cells[CtrFillFrom, iDataCol].Address;
                SummarySheet.Range[sAddress].Formula = "= " + sChageValue;

                TempStringForAddress = SummarySheet.Cells[CtrFillFrom, iDataCol].Address.Replace("$", "");
                TempStringForAddress2 = SummarySheet.Cells[CtrFillTo, iDataCol].Address.Replace("$", "");
                oRngStart = SummarySheet.Range[TempStringForAddress];
                if (TempStringForAddress != TempStringForAddress2)
                {
                    sMaxRange = TempStringForAddress + ":" + TempStringForAddress2;
                    oRngTotal = SummarySheet.Range[sMaxRange];

                    //oRngStart.AutoFill(Destination = oRngTotal);
                    AutoFillRange(oRngStart, oRngTotal);
                }

                //*********************** Set the formula for Percent **********************************
                sChageValue = "";
                iDataCol = 3 + Step1SelectionDataTable.Rows.Count + 1 + Step2SelectionDataTable.Rows.Count + 1;

                sNum = SummarySheet.Cells[CtrFillFrom, 2 + Step1SelectionDataTable.Rows.Count].Address.Replace("$", "");
                sDen = SummarySheet.Cells[CtrFillFrom, 2 + Step1SelectionDataTable.Rows.Count + 1 + Step2SelectionDataTable.Rows.Count].Address.Replace("$", "");

                sChageValue = "IF(OR(" + sNum + "=\"\" ," + sDen + "=\"\"),\"\",ROUND((" + sNum + " * 100 / " + sDen + ")," + SummarySheet.Cells[6, 1].Address + "))";

                sAddress = SummarySheet.Cells[CtrFillFrom, iDataCol - 1].Address;
                // Set Formula 
                SummarySheet.Range[sAddress].Formula = "= " + sChageValue;

                //Apply formula to range
                TempStringForAddress = SummarySheet.Cells[CtrFillFrom, iDataCol - 1].Address.Replace("$", "");
                TempStringForAddress2 = SummarySheet.Cells[CtrFillTo, iDataCol - 1].Address.Replace("$", "");
                oRngStart = SummarySheet.Range[TempStringForAddress];

                if (TempStringForAddress != TempStringForAddress2)
                {
                    sMaxRange = TempStringForAddress + ":" + TempStringForAddress2;
                    oRngTotal = SummarySheet.Range[sMaxRange];
                    this.AutoFillRange(oRngStart, oRngTotal);
                }

                //*** Setting the Font                                 
                try
                {
                    SummarySheet.Range[0, 0, CtrFillTo, iDataCol - 1].Font.Name = FontName;
                    SummarySheet.Range[0, 0, CtrFillTo, iDataCol - 1].Font.Size = (float)FontSize;
                    SummarySheet.Range[7, 0, CtrFillTo, iDataCol - 1].Columns.AutoFit();
                    SummarySheet.Range[CtrFillFrom, 2, CtrFillTo, iDataCol - 1].HorizontalAlignment = HAlign.Right; //xlRight 
                    SummarySheet.Cells[0, 0].Font.Bold = true;
                    SummarySheet.Cells[0, 0].Font.Italic = true;
                    SummarySheet.Cells[0, 0].Font.Size = TitleFontSize;
                    SummarySheet.Range[7, 0, 7, iDataCol - 1].Font.Bold = true;
                }
                catch (Exception ex)
                {
                }

                //*********************************** S H O W L O G ************************************** 
                LogSheet.Cells[0, 0].Value = LangStrings.VAR_LOG_NAME;
                LogSheet.Cells[2, 0].Value = LangStrings.sMODULE;
                LogSheet.Cells[2, 1].Value = LangStrings.VAR_WIZARD_NAME;
                LogSheet.Cells[3, 0].Value = LangStrings.DATE_TIME;
                LogSheet.Cells[3, 1].Value = TimeStamp;
                LogSheet.Cells[4, 0].Value = LangStrings.LOG_FILE_NAME;
                LogSheet.Cells[4, 1].Value = "Log_Percent (" + TimeStamp + ").xls";

                CtrFillFrom = 8;// 9 in Excel; 
                //*** Set Data 

                LogSheet.Range[CtrFillFrom, 0, CtrFillFrom + msArray.GetLength(0) - 1, 8].Value = msArray;
                //*** Autofit                 
                LogSheet.Range[CtrFillFrom - 2, 0, CtrFillFrom + msArray.GetLength(0) - 1, 8].Columns.AutoFit();
                LogSheet.Range[CtrFillFrom, 5, CtrFillFrom + msArray.GetLength(0) - 1, 5].HorizontalAlignment = HAlign.Left; //-4131;                
                //xlLeft 'Time column 

                //*** Set Font 
                LogSheet.Range[0, 0, CtrFillFrom + msArray.GetLength(0) - 1, 8].Font.Name = FontName;
                LogSheet.Range[0, 0, CtrFillFrom + msArray.GetLength(0) - 1, 8].Font.Size = (float)FontSize;
                LogSheet.Cells[0, 0].Font.Bold = true;
                LogSheet.Cells[0, 0].Font.Italic = true;
                LogSheet.Cells[0, 0].Font.Size = TitleFontSize;
                //****************************************************************************************** 

                // Activate forst sheet and save workbook
                DIExcel.GetWorksheet(0).Cells[0, 0].Activate();
                DIExcel.Save();

                this.RaiseEventSetProgress(100);

                //*** hide progress bar                             
                this.RaiseEventHideProgressBar();

                // Dispose worksheets
                SummarySheet = null;
                LogSheet = null;
                dvMain.RowFilter = string.Empty;
            }

            catch (Exception ex)
            {
            }
            finally
            {
                this.RaiseEventSetCursor(true);
            }
            // Raise event to show workbook Show workbook inside panel
            this.RaiseEventCalculateStepCompleted("pnl1_4");
        }

        /// <summary>
        /// Perform 100Minus Summary calculation
        /// </summary>
        public void Fill100MinusSummaryLog()
        {
            try
            {
                // Apply Language settings
                ApplyLanguageSettings();
                RaiseEventSetCursor(false);

                //Excel.Worksheet                 
                int i = 0;
                int iAreaCount = 0;
                int iCtrFillFrom;
                int iCtrFillTo;
                int iDataCol;
                string TempStringForEndCellAddress = string.Empty;
                string sAddress = string.Empty;
                DataView dvMain;
                System.Collections.Specialized.StringCollection oSubgroup = new System.Collections.Specialized.StringCollection();

                //*** Initialize Progress Bar 
                this.RaiseEventSetProgress(1);

                // Get calculate file to temp location
                if (File.Exists(this.LogFilePath))
                {
                    File.Copy(this.LogFilePath, this.TempLogFilePath, true);
                    System.IO.File.SetAttributes(TempLogFilePath, FileAttributes.Normal);
                }
                //  Opening and getting workbook                        
                this.DIExcel = new DIExcel(this.TempLogFilePath);
                // Getting worksheet
                SummarySheet = DIExcel.GetWorksheet(0);
                LogSheet = DIExcel.GetWorksheet(1);
                SummarySheet.Name = LangStrings.SUMMARY;
                LogSheet.Name = LangStrings.LOG;

                //*************************** SHOW INDEX ******************************************                 
                RaiseEventSetProgress(10);

                //*** Get the data view with all filters applied 
                dvMain = this.PresentationData;
                //dvMain.RowFilter = string.Empty;
                //if (string.IsNullOrEmpty(dvMain.RowFilter))
                //{
                dvMain.RowFilter = Indicator.IndicatorNId + " = " + Step1SelectionDataTable.Rows[0][Indicator.IndicatorNId].ToString();
                //}
                //else
                //{                 
                //    dvMain.RowFilter += Indicator.IndicatorNId + " = " + Step1SelectionDataTable.Rows[0][Indicator.IndicatorNId].ToString();
                //}

                dvMain.Sort = Area.AreaID + " ASC";
                //DataRowView drv;                 

                //msDataArray = new string[dvMain.Count - 1, 5];
                msDataArray = new string[dvMain.Count, 6];
                //  string[,] msSummaryArray = new string[dvMain.Count-1 + 3, 8];
                int summaryArrayLengh = 1;
                if (dvMain.Count>0)
                {
                    summaryArrayLengh = dvMain.Count + 3;
                }
                string[,] msSummaryArray = new string[summaryArrayLengh, 8];

                msSummaryArray[0, 0] = LangStrings.AREAID;
                msSummaryArray[0, 1] = LangStrings.AREANAME;
                msSummaryArray[0, 2] = Step1SelectionDataTable.Rows[0][0].ToString();// lv2_1_Selected.Items(0).SubItems(0).Text; 
                msSummaryArray[0, 3] = LangStrings.VAR_WIZARD_NAME;
                msSummaryArray[0, 4] = LangStrings.SUBGROUP;
                msSummaryArray[0, 5] = LangStrings.TIME;
                msSummaryArray[0, 6] = LangStrings.UNIT;
                msSummaryArray[0, 7] = LangStrings.SOURCE;

                string PrevAreaId = "";
                i += 2;
                foreach (DataRowView drv in dvMain)
                {
                    //*** Fill Datavalue for Numerator Indicator against each AreaID 
                    if (PrevAreaId != drv[Area.AreaID].ToString())
                    {
                        iAreaCount += 1;
                        PrevAreaId = drv[Area.AreaID].ToString();
                        msSummaryArray[i, 0] = drv[Area.AreaID].ToString();
                        msSummaryArray[i, 1] = drv[Area.AreaName].ToString();
                    }
                    msSummaryArray[i, 2] = drv[Data.DataValue].ToString();
                    msSummaryArray[i, 3] = System.Convert.ToString(100 - System.Convert.ToDouble(drv["Data_Value"].ToString()));
                    msSummaryArray[i, 4] = drv[SubgroupVals.SubgroupVal].ToString();

                    if (oSubgroup.Contains(drv[SubgroupVals.SubgroupValNId].ToString()) == false)
                    {
                        oSubgroup.Add(drv[SubgroupVals.SubgroupValNId].ToString());
                    }
                    msSummaryArray[i, 5] = drv[Timeperiods.TimePeriod].ToString();
                    msSummaryArray[i, 6] = drv[Unit.UnitName].ToString();
                    msSummaryArray[i, 7] = drv[IndicatorClassifications.ICName].ToString();
                    msDataArray[i - 2, 0] = drv[Timeperiods.TimePeriod].ToString();
                    msDataArray[i - 2, 1] = drv[Area.AreaID].ToString();
                    msDataArray[i - 2, 2] = drv[Area.AreaName].ToString();
                    msDataArray[i - 2, 4] = drv[SubgroupVals.SubgroupVal].ToString();
                    msDataArray[i - 2, 5] = drv[IndicatorClassifications.ICName].ToString();
                    i += 1;
                }

                string[,] msLogArray = new string[dvMain.Count - 1 + iAreaCount + 3, 9];
                //*** Data Array for Log Sheet 
                msLogArray[0, 0] = LangStrings.AREAID;
                msLogArray[0, 1] = LangStrings.AREANAME;
                msLogArray[0, 2] = LangStrings.INDICATOR;
                msLogArray[0, 3] = LangStrings.SUBGROUP;
                msLogArray[0, 4] = LangStrings.TIME;
                msLogArray[0, 5] = LangStrings.DATAVALUE;
                msLogArray[0, 6] = LangStrings.UNIT;
                msLogArray[0, 7] = LangStrings.SOURCE;
                msLogArray[0, 8] = LangStrings.VAR_WIZARD_NAME;

                PrevAreaId = "";
                i = 1;
                dvMain.GetEnumerator().Reset();
                foreach (DataRowView dvRow in dvMain)
                {
                    if (PrevAreaId != dvRow[Area.AreaID].ToString())
                    {
                        i += 1;
                        PrevAreaId = dvRow[Area.AreaID].ToString();
                        msLogArray[i, 0] = dvRow[Area.AreaID].ToString();
                        msLogArray[i, 1] = dvRow[Area.AreaName].ToString();
                    }
                    msLogArray[i, 2] = dvRow[Indicator.IndicatorName].ToString();
                    msLogArray[i, 3] = dvRow[SubgroupVals.SubgroupVal].ToString();
                    msLogArray[i, 4] = dvRow[Timeperiods.TimePeriod].ToString();
                    msLogArray[i, 5] = dvRow[Data.DataValue].ToString();
                    msLogArray[i, 6] = dvRow[Unit.UnitName].ToString();
                    msLogArray[i, 7] = dvRow[IndicatorClassifications.ICName].ToString();
                    msLogArray[i, 8] = Convert.ToString(100 - Convert.ToDouble(dvRow[Data.DataValue]));
                    i += 1;
                }

                //*** Special Handling for Subgroup in 100 Minus                                 
                this.DESheetInformation.Subgroup = new string[oSubgroup.Count];
                this.DESheetInformation.SubgroupGUID = new string[oSubgroup.Count];

                for (i = 0; i <= oSubgroup.Count - 1; i++)
                {
                    string sSql = string.Empty;
                    // sSql = this.DBQueries.Subgroup.GetSubgroupVals(Lib.DI_LibDAL.Queries.FilterFieldType.NId, oSubgroup[i]);
                    sSql = this.DBQueries.SubgroupVals.GetSubgroupVals(Lib.DI_LibDAL.Queries.FilterFieldType.NId, oSubgroup[i]);
                    dvMain = this.DBConnection.ExecuteDataTable(sSql).DefaultView;
                    this.DESheetInformation.Subgroup[i] = dvMain[0][SubgroupVals.SubgroupVal].ToString();
                    if (dvMain.Count > 0)
                    {
                        this.DESheetInformation.SubgroupGUID[i] = dvMain[0][SubgroupVals.SubgroupValGId].ToString();
                    }
                    else
                    {
                        this.DESheetInformation.SubgroupGUID[i] = Guid.NewGuid().ToString();
                    }
                }
                SummarySheet.Cells[0, 0].Value = LangStrings.VAR_WIZARD_NAME;
                SummarySheet.Cells[2, 0].Value = LangStrings.sMODULE;
                SummarySheet.Cells[2, 1].Value = LangStrings.VAR_WIZARD_NAME;
                SummarySheet.Cells[3, 0].Value = LangStrings.DATE_TIME;
                SummarySheet.Cells[3, 1].Value = TimeStamp;
                SummarySheet.Cells[4, 0].Value = LangStrings.LOG_FILE_NAME;
                SummarySheet.Cells[4, 1].Value = "Log_100 Minus (" + TimeStamp + ").xls";
                SummarySheet.Cells[6, 0].Value = LangStrings.DECIMALPLACES;
                SummarySheet.Cells[6, 1].Value = this.DecimalUpDownControlValue;

                this.RaiseEventSetProgress(25);

                //************************ Set Data in summary sheet********************                 
                SummarySheet.Range[8, 0, 8 + msSummaryArray.GetLength(0) - 1, 7].Value = msSummaryArray;

                this.RaiseEventSetProgress(35);

                //Fill Data Values and Formula for each records 
                string sChangeValue;
                string sMinRange;
                string sMaxRange;
                IRange oRngStart;
                IRange oRngTotal;

                //*** Set the formula for 100 Minus                 
                iCtrFillFrom = 10;
                iCtrFillTo = iCtrFillFrom + msSummaryArray.GetLength(0) - 2 - 2;
                if (iCtrFillTo<iCtrFillFrom)
                {
                    iCtrFillTo = iCtrFillFrom;
                }
                // -2 for header row and a blank row 
                iDataCol = 4;
                sChangeValue = "";
                TempStringForEndCellAddress = SummarySheet.Cells[iCtrFillFrom, 2].Address.Replace("$", "");
                sChangeValue = "ROUND(100 - " + TempStringForEndCellAddress + "," + SummarySheet.Cells[6, 1].Address + ")";
                sAddress = SummarySheet.Cells[iCtrFillFrom, iDataCol - 1].Address;

                // Set Formula
                SummarySheet.Range[sAddress].Formula = "= " + sChangeValue;

                // Apply formula for all rows
                sAddress = SummarySheet.Cells[iCtrFillFrom, iDataCol - 1].Address.Replace("$", "");
                oRngStart = SummarySheet.Range[sAddress];
                TempStringForEndCellAddress = SummarySheet.Cells[iCtrFillTo, iDataCol - 1].Address.Replace("$", "");
                if (sAddress != TempStringForEndCellAddress)
                {
                    sAddress = SummarySheet.Cells[iCtrFillFrom, iDataCol - 1].Address.Replace("$", "");
                    TempStringForEndCellAddress = SummarySheet.Cells[iCtrFillTo, iDataCol - 1].Address.Replace("$", "");
                    sMaxRange = sAddress + ":" + TempStringForEndCellAddress;
                    oRngTotal = SummarySheet.Range[sMaxRange];
                    AutoFillRange(oRngStart, oRngTotal);
                }

                iDataCol = 8;
                //*** Setting the Font 
                SummarySheet.Range[0, 0, iCtrFillTo, iDataCol - 1].Font.Name = this.FontName;
                SummarySheet.Range[0, 0, iCtrFillTo, iDataCol - 1].Font.Size = (float)this.FontSize;
                
                try
                {
                    SummarySheet.Range[8, 0, iCtrFillTo, iDataCol - 1].Columns.AutoFit();
                    SummarySheet.Range[iCtrFillFrom, 4, iCtrFillTo, 4].HorizontalAlignment = HAlign.Left; // -4131; 
                }
                catch (Exception ex)
                {                    
                }                
                //xlLeft 'Time column 

                SummarySheet.Cells[0, 0].Font.Bold = true;
                SummarySheet.Cells[0, 0].Font.Italic = true;
                SummarySheet.Cells[0, 0].Font.Size = this.TitleFontSize;

                //Raise event to notify Progress                 
                this.RaiseEventSetProgress(45);

                //*********************************** S H O W L O G **************************************                 
                LogSheet.Cells[0, 0].Value = LangStrings.VAR_LOG_NAME;
                LogSheet.Cells[2, 0].Value = LangStrings.sMODULE;
                LogSheet.Cells[2, 1].Value = LangStrings.VAR_WIZARD_NAME;
                LogSheet.Cells[3, 0].Value = LangStrings.DATE_TIME;
                LogSheet.Cells[3, 1].Value = TimeStamp;
                LogSheet.Cells[4, 0].Value = LangStrings.LOG_FILE_NAME;
                LogSheet.Cells[4, 1].Value = "Log_100 Minus (" + TimeStamp + ").xls";
                iCtrFillFrom = 8;
                //*** Set Data      
                LogSheet.Range[iCtrFillFrom, 0, iCtrFillFrom + msLogArray.GetLength(0) - 1, 8].Value = msLogArray;
                //NOte1 : Index  change completed
                //*** Autofit 
                LogSheet.Range[iCtrFillFrom, 0, iCtrFillFrom + msLogArray.GetLength(0) - 1, 8].Columns.AutoFit();

                LogSheet.Range[iCtrFillFrom, 4, iCtrFillFrom + msLogArray.GetLength(0) - 1, 4].HorizontalAlignment = HAlign.Left;// -4131; 
                //xlLeft 'Time column 

                //*** Set Font 
                LogSheet.Range[0, 0, iCtrFillFrom + msLogArray.GetLength(0) - 1, 8].Font.Name = this.FontName;
                LogSheet.Range[0, 0, iCtrFillFrom + msLogArray.GetLength(0) - 1, 8].Font.Size = (float)this.FontSize;

                LogSheet.Cells[0, 0].Font.Bold = true;
                LogSheet.Cells[0, 0].Font.Italic = true;
                LogSheet.Cells[0, 0].Font.Size = this.TitleFontSize;

                //****************************************************************************************** 
                // Save Workbook
                DIExcel.GetWorksheet(0).Cells[0, 0].Activate();
                DIExcel.Save();
                this.RaiseEventSetProgress(100);

                //Raise event for hiding progress bar                
                this.RaiseEventHideProgressBar();
                // Dispose
                SummarySheet = null;
                LogSheet = null;
                dvMain.RowFilter = string.Empty;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                RaiseEventSetCursor(true);
            }

            // RaiseEvent step completed
            this.RaiseEventCalculateStepCompleted("pnl2_3");
        }

        /// <summary>
        /// Perform Composite Index Calculations
        /// </summary>
        /// <param name="compositeIndexStep2DataTable"></param>
        /// <param name="scaleMinDecimalValue">nud3_3_Min.Value</param>
        /// <param name="scaleMaxDecimalValue">nud3_3_Max.Value;</param>
        /// <param name="showSubgroup"></param>
        /// <param name="step3SubgroupText"></param>
        public void FillCompositeIndexSummaryLog(DataTable compositeIndexStep2DataTable, decimal scaleMinDecimalValue, decimal scaleMaxDecimalValue, bool showSubgroup, String step3SubgroupText)
        {
            // Info required Step1DataTable,scaleMinDecimalValue,scaleMaxDecimalValue
            // ScaleMinValue,
            //CompositeIndexStep2DataTable  having columns Indicator ,Percent weight, HighIsGood
            // showSubgroup
            //cbo3_3_Subgroup.Text as step3SubgroupText

            int i = 0;
            int j = 0;
            int k = 0;
            string sFilter = string.Empty;
            int iCtrFillFrom = 0;
            int iCtrFillTo = 0;
            int iSelCount = Step1SelectionDataTable.Rows.Count;
            int iRecordCount = 0;
            string[] sSel_NId = new string[2];
            string sSel_Indicator_NId = "";
            string sSel_Subgroup_Val_NId = "";
            string TempFromAddressString = string.Empty;
            string TempToAddressString = string.Empty;
            string[,] msArray = new string[1, 8];

            try
            {
                //Set cursor to wait cursor            
                this.RaiseEventSetCursor(false);

                // Apply Language Settings
                ApplyLanguageSettings();

                //*** Set Progress bar
                this.RaiseEventSetProgress(1);

                // Get temp log file
                if (File.Exists(this.LogFilePath))
                {
                    File.Copy(this.LogFilePath, this.TempLogFilePath, true);
                    System.IO.File.SetAttributes(TempLogFilePath, FileAttributes.Normal);
                }
               
                //open file and get worksheets
                this.DIExcel = new DIExcel(TempLogFilePath);
                SummarySheet = DIExcel.GetWorksheet(0);
                LogSheet = DIExcel.GetWorksheet(1);
                SummarySheet.Name = LangStrings.SUMMARY;
                LogSheet.Name = LangStrings.LOG;

                //*************************** SHOW INDEX ****************************************** 
                // Raise Event to notify progess
                this.RaiseEventSetProgress(10);

                //*** enter lang based info in summery sheet
                SummarySheet.Cells[0, 0].Value = LangStrings.VAR_WIZARD_NAME;
                SummarySheet.Cells[2, 0].Value = LangStrings.sMODULE;
                SummarySheet.Cells[2, 1].Value = LangStrings.VAR_WIZARD_NAME;
                SummarySheet.Cells[3, 0].Value = LangStrings.DATE_TIME;
                SummarySheet.Cells[3, 1].Value = TimeStamp;
                SummarySheet.Cells[4, 0].Value = LangStrings.LOG_FILE_NAME;
                SummarySheet.Cells[4, 1].Value = "Log_Composite Index (" + TimeStamp + ").xls";
                SummarySheet.Cells[6, 3].Value = LangStrings.DECIMALPLACES;
                SummarySheet.Cells[6, 4].Value = this.DecimalUpDownControlValue;
                SummarySheet.Cells[6, 0].Value = LangStrings.SCALE_MIN;
                SummarySheet.Cells[6, 1].Value = scaleMinDecimalValue;
                SummarySheet.Cells[7, 0].Value = LangStrings.SCALE_MAX;
                SummarySheet.Cells[7, 1].Value = scaleMaxDecimalValue;
                SummarySheet.Cells[9, 0].Value = LangStrings.PERCENT_WEIGHT;
                SummarySheet.Cells[10, 0].Value = LangStrings.HIGHISGOOD;

                for (i = 2; i <= iSelCount * 2; i += 2)
                {
                    SummarySheet.Cells[9, i - 1].Value = compositeIndexStep2DataTable.Rows[i / 2 - 1][1].ToString();// dg3_2_Weight[i / 2 - 1, 1).ToString(); 
                    SummarySheet.Cells[10, i - 1].Value = compositeIndexStep2DataTable.Rows[i / 2 - 1][2].ToString();
                    SummarySheet.Cells[10, i - 1].HorizontalAlignment = HAlign.Right;       //xlRight 
                    SummarySheet.Cells[13, i - 1].Value = compositeIndexStep2DataTable.Rows[i / 2 - 1][0].ToString();
                    SummarySheet.Cells[13, i].Value = LangStrings.INDEX;
                }

                SummarySheet.Cells[13, 0].Value = Area.AreaName;
                SummarySheet.Cells[13, i - 1].Value = "Score";
                SummarySheet.Cells[13, i].Value = "Composite Index";
                SummarySheet.Cells[13, i + 1].Value = Area.AreaID;

                this.RaiseEventSetProgress(25);

                //*** Get Selected Indicator/I_S NId 
                for (i = 0; i <= iSelCount - 1; i++)
                {
                    sSel_NId[0] = Step1SelectionDataTable.Rows[i][Indicator.IndicatorNId].ToString();
                    sSel_NId[1] = Step1SelectionDataTable.Rows[i][SubgroupVals.SubgroupValNId].ToString();

                    if (i == 0)
                    {
                        if (showSubgroup)
                        {
                            sSel_Indicator_NId = sSel_NId[0];
                            sSel_Subgroup_Val_NId = sSel_NId[1];
                        }
                        else
                        {
                            sSel_Indicator_NId = sSel_NId[0];
                        }
                    }
                    else
                    {
                        if (showSubgroup)
                        {
                            sSel_Indicator_NId = sSel_Indicator_NId + "," + sSel_NId[0];
                            sSel_Subgroup_Val_NId = sSel_Subgroup_Val_NId + "," + sSel_NId[1];
                        }
                        else
                        {
                            sSel_Indicator_NId = sSel_Indicator_NId + "," + sSel_NId[0];
                        }
                    }
                }

                //*** Get the data view with all filters applied                 
                DataView dvMain = this.PresentationData;

                //*** Filter records for selected Indicator / Subgroup in calculates step1 
                if (showSubgroup)
                {
                    sFilter = Indicator.IndicatorNId + " IN (" + sSel_Indicator_NId + ") AND " + SubgroupVals.SubgroupValNId + " IN (" + sSel_Subgroup_Val_NId + ")";
                }
                else
                {
                    sFilter = Indicator.IndicatorNId + " IN (" + sSel_Indicator_NId + ")";
                }

                if (dvMain.RowFilter == "")
                {
                    dvMain.RowFilter = sFilter;
                }
                else
                {
                    dvMain.RowFilter += " AND " + sFilter;
                }
                sFilter = dvMain.RowFilter;

                //Fill Unique Area 
                dvMain.Sort = " Area_ID ASC";
                iCtrFillFrom = 16;
                for (i = 0; i <= dvMain.Count - 1; i++)
                {
                    if (dvMain[i][Area.AreaID].ToString() != SummarySheet.Cells[iRecordCount + iCtrFillFrom - 1, iSelCount * 2 + 3].Value + "")
                    {
                        SummarySheet.Cells[iRecordCount + iCtrFillFrom, 0].Value = dvMain[i][Area.AreaName];
                        SummarySheet.Cells[iRecordCount + iCtrFillFrom, iSelCount * 2 + 3].Value = dvMain[i][Area.AreaID];
                        iRecordCount += 1;
                    }
                }
                iCtrFillTo = iCtrFillFrom + iRecordCount - 1;

                //Update progress
                this.RaiseEventSetProgress(35);

                //*** Set Data Array for Log Sheet simultaneously                
                msDataArray = new string[iRecordCount, 6];
                msArray = new string[(iRecordCount * iSelCount) + iRecordCount + 3, 8];

                msArray[0, 0] = LangStrings.AREAID;
                msArray[0, 1] = LangStrings.AREANAME;
                msArray[0, 2] = LangStrings.INDICATOR;
                msArray[0, 3] = LangStrings.SUBGROUP;
                msArray[0, 4] = LangStrings.TIME;
                msArray[0, 5] = LangStrings.DATAVALUE;
                msArray[0, 6] = LangStrings.UNIT;
                msArray[0, 7] = LangStrings.SOURCE;
                k += 2;

                //Fill Data Values and Formula for each records 
                string sStaticValue = string.Empty;
                string sScoreStaticValue = "";
                string sHiddenDivision = "";
                string sChageValue = string.Empty;
                string sAddess = string.Empty;
                string sMinRange = string.Empty;
                string sMaxRange = string.Empty;
                IRange oRngStart;
                IRange oRngTotal;
                int iDataCol = 0;
                bool bSupressArea = false;

                //*** varibales for setting Avg Data Value for missing Data 
                int[] arrDataCount = new int[iSelCount];
                //float[] arrDataSum = new float[iSelCount];
                double[] arrDataSum = new double[iSelCount];
                string[] arrCellId = new string[iSelCount];

                for (i = iCtrFillFrom; i <= iCtrFillTo; i++)
                {

                    //*** Fill Datavalue for all selected indecator against each AreaID 
                    for (j = 0; j <= iSelCount - 1; j++)
                    {
                        iDataCol = (j + 1) * 2;
                        dvMain.RowFilter = sFilter + " AND " + Area.AreaID + " = '" + Utility.DICommon.EscapeWildcardChar(Utility.DICommon.RemoveQuotes(SummarySheet.Cells[i, iSelCount * 2 + 3].Value.ToString())) + "'";
                        //sSel_NId = lv3_1_Selected.Items(j).Tag.ToString.Split("_");
                        sSel_NId[0] = Step1SelectionDataTable.Rows[j][Indicator.IndicatorNId].ToString();
                        sSel_NId[1] = Step1SelectionDataTable.Rows[j][SubgroupVals.SubgroupValNId].ToString();
                        if (showSubgroup)
                        {
                            dvMain.RowFilter += " AND " + Indicator.IndicatorNId + " =" + sSel_NId[0] + " AND " + SubgroupVals.SubgroupValNId + " =" + sSel_NId[1];
                        }
                        else
                        {
                            dvMain.RowFilter += " AND " + Indicator.IndicatorNId + " =" + sSel_NId[0];
                        }

                        if (dvMain.Count == 0)
                        {
                            SummarySheet.Cells[i, iDataCol - 1].Value = 0;
                            SummarySheet.Cells[i, iDataCol - 1].NumberFormat = "0.00";
                            SummarySheet.Cells[i, iDataCol - 1].Font.Bold = true;
                            SummarySheet.Cells[i, iDataCol - 1].Font.ColorIndex = 15;
                            arrCellId[j] += i + "," +Convert.ToInt32(iDataCol-1) + ";";
                        }
                        else
                        {
                            SummarySheet.Cells[i, iDataCol - 1].Value = dvMain[0][Data.DataValue].ToString();
                            if (bSupressArea == false)
                            {
                                msArray[k, 0] = dvMain[0][Area.AreaID].ToString();
                                msArray[k, 1] = dvMain[0][Area.AreaName].ToString();

                                msDataArray[i - iCtrFillFrom, 0] = dvMain[0][Timeperiods.TimePeriod].ToString();
                                msDataArray[i - iCtrFillFrom, 1] = dvMain[0][Area.AreaID].ToString();
                                msDataArray[i - iCtrFillFrom, 2] = dvMain[0][Area.AreaName].ToString();
                                msDataArray[i - iCtrFillFrom, 4] = step3SubgroupText;

                                //dvMain[0]["Subgroup_Val") 
                                msDataArray[i - iCtrFillFrom, 5] = dvMain[0][IndicatorClassifications.ICName].ToString();
                                bSupressArea = true;
                            }
                            msArray[k, 2] = dvMain[0][Indicator.IndicatorName].ToString();
                            msArray[k, 3] = dvMain[0][SubgroupVals.SubgroupVal].ToString();
                            msArray[k, 4] = dvMain[0][Timeperiods.TimePeriod].ToString();
                            msArray[k, 5] = dvMain[0][Data.DataValue].ToString();
                            msArray[k, 6] = dvMain[0][Unit.UnitName].ToString();
                            msArray[k, 7] = dvMain[0][IndicatorClassifications.ICName].ToString();
                            arrDataCount[j] += 1;
                            arrDataSum[j] += Convert.ToDouble(dvMain[0][Data.DataValue]);
                            k += 1;
                        }
                        if (i == iCtrFillFrom)
                        {
                            //*** Build and Set Formula For Index Columns 
                            sStaticValue = SummarySheet.Cells[10, iDataCol - 1].Address;
                            //*** Extract the Alphabet part from cell address. Accomodate condition that cell address may be like A1 or AA1 
                            TempFromAddressString = SummarySheet.Cells[0, iDataCol - 1].Address.Replace("$", "");
                            sChageValue = TempFromAddressString;// Strings.Replace(SummarySheet.Cells[1, iDataCol].Address, "$", "");
                            sChageValue = (Strings.Len(sChageValue) == 3 ? Strings.Mid(sChageValue, 1, 2) : Strings.Mid(sChageValue, 1, 1));
                            sChageValue = sChageValue + Convert.ToString(iCtrFillFrom + 1);


                            sMinRange = "Min(" + SummarySheet.Cells[iCtrFillFrom, iDataCol - 1].Address + ":" + SummarySheet.Cells[iCtrFillTo, iDataCol - 1].Address + ")";
                            sMaxRange = "Max(" + SummarySheet.Cells[iCtrFillFrom, iDataCol - 1].Address + ":" + SummarySheet.Cells[iCtrFillTo, iDataCol - 1].Address + ")";
                            sAddess = SummarySheet.Cells[iCtrFillFrom, iDataCol].Address;
                            SummarySheet.Range[sAddess].Formula = "= IF((" + sMaxRange + " - " + sMinRange + ") = 0" + "," + 1 + "," + "IF(" + sStaticValue + "," + "(" + sChageValue + " - " + sMinRange + ")" + " / " + "(" + sMaxRange + " - " + sMinRange + ")," + " " + "(" + sChageValue + " - " + sMaxRange + ")" + " / " + "(" + sMinRange + " - " + sMaxRange + ")))";

                            TempFromAddressString = SummarySheet.Cells[iCtrFillFrom, iDataCol].Address.Replace("$", "");
                            oRngStart = SummarySheet.Range[TempFromAddressString];

                            TempToAddressString = SummarySheet.Cells[iCtrFillTo, iDataCol].Address.Replace("$", "");
                            if (TempFromAddressString != TempToAddressString)
                            {
                                sMaxRange = TempFromAddressString + ":" + TempToAddressString;
                                oRngTotal = SummarySheet.Range[sMaxRange];
                                this.AutoFillRange(oRngStart, oRngTotal);
                            }

                            //*** Build Formula for Score Column 
                            if (Strings.Trim(sScoreStaticValue) == "")
                            {
                                //TempFromAddressString = SummarySheet.Cells[iCtrFillFrom, iDataCol + 1].Address.Replace("$", "");
                                sScoreStaticValue = TempFromAddressString + "*" + SummarySheet.Cells[9, iDataCol - 1].Address;
                                sHiddenDivision = SummarySheet.Cells[9, iDataCol - 1].Address;
                            }
                            else
                            {
                                sScoreStaticValue = sScoreStaticValue + " + " + TempFromAddressString + "*" + SummarySheet.Cells[9, iDataCol - 1].Address;
                                sHiddenDivision = sHiddenDivision + " + " + SummarySheet.Cells[9, iDataCol - 1].Address;
                            }
                        }
                    }

                    //*** Set Formaula for all Index Colunns against each AreaID 
                    if (i == iCtrFillFrom)
                    {
                        //*** Set Formula for Score Column 
                        sAddess = SummarySheet.Cells[iCtrFillFrom, iDataCol + 1].Address;
                        SummarySheet.Range[sAddess].Formula = "= (" + sScoreStaticValue + ") / " + "(" + sHiddenDivision + ")";
                        TempToAddressString = SummarySheet.Cells[iCtrFillTo, iDataCol + 1].Address.Replace("$", "");
                        TempFromAddressString = SummarySheet.Cells[iCtrFillFrom, iDataCol + 1].Address.Replace("$", "");
                        oRngStart = SummarySheet.Range[TempFromAddressString];
                        if (TempFromAddressString != TempToAddressString)
                        {
                            sMaxRange = TempFromAddressString + ":" + TempToAddressString;
                            oRngTotal = SummarySheet.Range[sMaxRange];
                            //oRngStart.AutoFill(Destination = oRngTotal);
                            this.AutoFillRange(oRngStart, oRngTotal);
                        }

                        //*** Set Formula for Composite Index 
                        string sHiddenColumn;

                        sHiddenColumn = (Strings.Len(Strings.Mid(sMaxRange, 1, Strings.InStr(1, sMaxRange, ":", CompareMethod.Text) - 1)) == 4 ? Strings.Mid(sMaxRange, 1, 2) : Strings.Mid(sMaxRange, 1, 1));
                        sMinRange = "Min(" + SummarySheet.Cells[iCtrFillFrom, iDataCol + 1].Address + ":" + SummarySheet.Cells[iCtrFillTo, iDataCol + 1].Address + ")";
                        sMaxRange = "Max(" + SummarySheet.Cells[iCtrFillFrom, iDataCol + 1].Address + ":" + SummarySheet.Cells[iCtrFillTo, iDataCol + 1].Address + ")";

                        sStaticValue = "IF((" + sMaxRange + " - " + sMinRange + ") = 0" + ", " + "$B$8" + "," + "$B$7" + " + " + "( (" + sHiddenColumn + 17 + " - " + sMinRange + ") / (" + sMaxRange + " - " + sMinRange + ") ) * (" + "$B$8" + " - " + "$B$7" + "))";
                        //*** Set formula for Decimal Precision based on Cells(7, 5) value. 
                        //*** Rounding should be cell address based so that if user makes changes inside excel it should reflect 
                        sStaticValue = "ROUND(" + sStaticValue + "," + SummarySheet.Cells[6, 4].Address + ")";
                        sAddess = SummarySheet.Cells[iCtrFillFrom, iDataCol + 2].Address;
                        SummarySheet.Range[sAddess].Formula = "= " + sStaticValue;
                        SummarySheet.Cells[iCtrFillFrom, iDataCol + 2].NumberFormat = "General";
                        TempFromAddressString = SummarySheet.Cells[iCtrFillFrom, iDataCol + 2].Address.Replace("$", "");
                        TempToAddressString = SummarySheet.Cells[iCtrFillTo, iDataCol + 2].Address.Replace("$", "");
                        oRngStart = SummarySheet.Range[TempFromAddressString];
                        if (TempFromAddressString != TempToAddressString)
                        {
                            sMaxRange = TempFromAddressString + ":" + TempToAddressString;
                            oRngTotal = SummarySheet.Range[sMaxRange];
                            this.AutoFillRange(oRngStart, oRngTotal);
                        }
                    }
                    k += 1;
                    bSupressArea = false;
                }

                //*** Set Avg data value for Missing Data 
                string[] sCellId;
                string[] sRowCol;
                float MissingData=0.0f;
                for (i = 0; i <= iSelCount - 1; i++)
                {
                    if ((arrCellId[i] != null))
                    {
                        // Get Missing Data : DataSum/ DataCount
                        try
                        {
                            MissingData = (float)arrDataSum[i] / arrDataCount[i];
                        }
                        catch (Exception)
                        {
                            
                        }
                        
                        sCellId = arrCellId[i].ToString().Split(';');
                        for (j = 0; j <= sCellId.Length - 1; j++)
                        {
                            if (sCellId[j].ToString().Trim() != "")
                            {
                                sRowCol = sCellId[j].Split(',');
                                SummarySheet.Cells[Convert.ToInt32(sRowCol[0]), Convert.ToInt32(sRowCol[1])].Value = MissingData;
                            }
                        }
                    }
                }

                //*** Setting the Font 
                SummarySheet.Range[0, 0, iCtrFillTo, iDataCol + 3].Font.Name = this.FontName;// goUserPreference.Language.FontName; 
                SummarySheet.Range[0, 0, iCtrFillTo, iDataCol + 3].Font.Size = (float)this.FontSize; //goUserPreference.Language.FontSize;

                SummarySheet.Range[6, 0, iCtrFillTo, iDataCol + 3].Columns.AutoFit();
                SummarySheet.Range[13, 1, 14, iSelCount * 2 + 3].HorizontalAlignment = HAlign.Right;// -4152;
                //xlRight 

                SummarySheet.Cells[0, 0].Font.Bold = true;
                SummarySheet.Cells[0, 0].Font.Italic = true;
                SummarySheet.Cells[0, 0].Font.Size = this.TitleFontSize;

                //*** Progress 
                //moStatusBar.progressBar.Value = 45;
                this.RaiseEventSetProgress(45);


                //*********************************** S H O W L O G ************************************** 

                LogSheet.Cells[0, 0].Value = LangStrings.VAR_LOG_NAME;
                LogSheet.Cells[2, 0].Value = LangStrings.sMODULE;
                LogSheet.Cells[2, 1].Value = LangStrings.VAR_WIZARD_NAME;
                LogSheet.Cells[3, 0].Value = LangStrings.DATE_TIME;
                LogSheet.Cells[3, 1].Value = TimeStamp;
                LogSheet.Cells[4, 0].Value = LangStrings.LOG_FILE_NAME;
                LogSheet.Cells[4, 1].Value = "Log_Composite Index (" + TimeStamp + ").xls";

                iCtrFillFrom = 8; //9;
                //*** Set Data 
                LogSheet.Range[iCtrFillFrom, 0, iCtrFillFrom + msArray.GetLength(0) - 1, 7].Value = msArray;
                //*** Autofit 
                LogSheet.Range[iCtrFillFrom - 2, 0, iCtrFillFrom + msArray.GetLength(0) - 1, 7].Columns.AutoFit();
                //*** Set Font 
                LogSheet.Range[0, 0, iCtrFillFrom + msArray.GetLength(0) - 2, 7].Font.Name = this.FontName;  //goUserPreference.Language.FontName;
                LogSheet.Range[1, 1, iCtrFillFrom + msArray.GetLength(0) - 2, 8].Font.Size = (float)this.FontSize; //goUserPreference.Language.FontSize;


                LogSheet.Cells[0, 0].Font.Bold = true;
                LogSheet.Cells[0, 0].Font.Italic = true;
                LogSheet.Cells[0, 0].Font.Size = this.TitleFontSize;

                //*** hide progress bar 
                this.RaiseEventSetProgress(100);

                DIExcel.Save();
                this.RaiseEventHideProgressBar();
                this.RaiseEventStep3_4Completed();

                SummarySheet = null;
                LogSheet = null;
                dvMain.RowFilter = string.Empty;

            }
            catch (Exception ex)
            {
            }
            finally
            {
                this.RaiseEventSetCursor(true);
            }
        }

        /// <summary>
        /// Perform SubTotal Calculations
        /// </summary>
        /// <param name="userPreference"></param>
        /// <param name="lastAreaLevel">Last area Level ,It is milastAreaLevel in UI application </param>
        /// <param name="Step2SelectionDataTable">Data Table having Step 2 selections of area level</param>
        public void FillSubtotalSummaryLog(UserPreference.UserPreference userPreference, Int32 lastAreaLevel, DataTable Step2SelectionDataTable)
        {
            try
            {
                //Required Information: UserPreferance
                //milastAreaLevel from set level method
                //Step 2 selections of area level

                this.RaiseEventSetCursor(false);
                IWorksheet SummarySheet;
                IWorksheet LogSheet;
                int i = 0;
                int j = 0;
                int k = 0;
                int iAreaCount = 0;
                int iCtrFillFrom = 0;
                int iCtrFillTo = 0;
                int iDataCol = 0;
                string sOrgRowFilter = "";
                string Str = string.Empty;
                DataView dvSubgroup;

                //*** Initialize Progress Bar 
                this.RaiseEventSetProgress(1);

                // Copy Log to temp location
                if (File.Exists(this.LogFilePath))
                {
                    File.Copy(this.LogFilePath, this.TempLogFilePath, true);
                    System.IO.File.SetAttributes(TempLogFilePath, FileAttributes.Normal);
                }               

                ApplyLanguageSettings();

                //  Opening and getting workbook                        
                this.DIExcel = new DIExcel(TempLogFilePath);
                SummarySheet = this.DIExcel.GetWorksheet(0);
                LogSheet = this.DIExcel.GetWorksheet(1);
                SummarySheet.Name = LangStrings.SUMMARY;
                LogSheet.Name = LangStrings.LOG;


                //*************************** SHOW INDEX ****************************************** 
                this.RaiseEventSetProgress(10);

                // Get presentation data
                DataView dvMain = this.PresentationData;

                //Get subgroup details using user selection
                Str = this.DBQueries.Calculates.Indicator_Subgroup_Selected(true, userPreference.UserSelection, userPreference.Database.SelectedConnectionDetail.ServerType);
                dvSubgroup = this.DBConnection.ExecuteDataTable(Str).DefaultView;

                ////Get area details using user selection
                Str = this.DBQueries.Area.GetArea(Lib.DI_LibDAL.Queries.FilterFieldType.NId, userPreference.UserSelection.AreaNIds);
                DataView dvArea = this.DBConnection.ExecuteDataTable(Str).DefaultView;

                //Getting Log array. //*** Data Array for Log Sheet 'dvArea.Count for blank rows 
                string[,] msLogArray = new string[dvMain.Count + dvArea.Count + 2, 8];

                //Use Filter Indicator NId form indicator selected in step 1
                dvSubgroup.RowFilter = Indicator.IndicatorNId + " = " + Convert.ToInt32(this.Step1SelectionDataTable.Rows[0][Indicator.IndicatorNId]);//  lv4_1_Selected.Items(0).Tag; 
                StringCollection oSubgroup = new StringCollection();
                StringCollection oSubgroupVal = new StringCollection();
                StringCollection oSubgroupGId = new StringCollection();

                for (k = 0; k <= dvSubgroup.Count - 1; k++)
                {
                    // add subgroup and subgroup gid into subgroup collection
                    if (oSubgroup.Count > 0)
                    {
                        //oSubgroup.Add((string)dvSubgroup[k]["Subgroup_Val_NId"]) give error unable to convert int to string
                        if (oSubgroup.Contains(Convert.ToInt32(dvSubgroup[k][SubgroupVals.SubgroupValNId]).ToString()) == false)
                        {
                            //oSubgroup.Add((string)dvSubgroup[k]["Subgroup_Val_NId"]);
                            oSubgroup.Add(Convert.ToInt32(dvSubgroup[k][SubgroupVals.SubgroupValNId]).ToString());
                            oSubgroupVal.Add((string)dvSubgroup[k][SubgroupVals.SubgroupVal]);
                        }
                    }
                    else
                    {
                        oSubgroup.Add(Convert.ToInt32(dvSubgroup[k][SubgroupVals.SubgroupValNId]).ToString());
                        oSubgroupVal.Add((string)dvSubgroup[k][SubgroupVals.SubgroupVal]);
                    }
                }

                string PrevAreaId = "";
                int iAreaParentNId = 0;

                ArrayList ArrAreaNId = new ArrayList();
                Hashtable AreaCol = new Hashtable();
                string sSource = string.Empty;
                string sTimePeriod = string.Empty;

                //* Data Filters 
                if (dvMain.RowFilter == "")
                {
                    sOrgRowFilter = "";
                }
                else
                {
                    sOrgRowFilter = dvMain.RowFilter + " AND ";
                }

                // Add selected area indicator and last area level in filter. So if area level is 3
                dvMain.RowFilter = sOrgRowFilter + Indicator.IndicatorNId + " = " + Convert.ToInt32(this.Step1SelectionDataTable.Rows[0][Indicator.IndicatorNId]) + " AND Area_Level=" + lastAreaLevel;
                dvMain.Sort = Timeperiods.TimePeriod + " DESC"; // GetMostRecent Data 

                foreach (DataRowView drv in dvMain)
                {
                    //*** Fill Datavalue for Numerator Indicator against each AreaID 
                    dvArea.RowFilter = Area.AreaNId + "=" + Convert.ToInt32(drv[Area.AreaNId]);
                    iAreaParentNId = Convert.ToInt32(dvArea[0][Area.AreaParentNId]);
                    dvArea.RowFilter = Area.AreaNId + "=" + iAreaParentNId;
                    if (AreaCol.ContainsKey(iAreaParentNId) == false)
                    {
                        ArrAreaNId.Add(iAreaParentNId);
                        AreaCol.Add(iAreaParentNId, 0);
                        AreaCol[iAreaParentNId] = new Hashtable();
                        for (k = 0; k <= oSubgroup.Count - 1; k++)
                        {
                            ((Hashtable)AreaCol[iAreaParentNId]).Add((string)oSubgroup[k], 0);
                        }
                    }
                     ((Hashtable)AreaCol[iAreaParentNId])[(int)drv[SubgroupVals.SubgroupValNId]] = Convert.ToDouble(((Hashtable)AreaCol[iAreaParentNId])[(int)drv["Subgroup_Val_NId"]]) + Convert.ToDouble(drv["Data_Value"]);

                    //((Hashtable)AreaCol[iAreaParentNId])[(string)drv["Subgroup_Val_NId"]] = (int)((Hashtable)AreaCol[iAreaParentNId](string)drv["Subgroup_Val_NId"]]  + (int)drv["Data_Value"]; 
                    sSource = drv[IndicatorClassifications.ICName].ToString();
                    sTimePeriod = drv[Timeperiods.TimePeriod].ToString();
                }

                //*** BugFix 10 Aug 2006 Skipping of areas without child nodes at last level 
                dvMain.RowFilter = sOrgRowFilter + Indicator.IndicatorNId + " = " + Convert.ToInt32(this.Step1SelectionDataTable.Rows[0][Indicator.IndicatorNId]) + " AND Area_Level=" + Convert.ToString(lastAreaLevel - 1);
                dvMain.Sort = Timeperiods.TimePeriod + " DESC";
                //*** GetMostRecent Data 
                foreach (DataRowView drv in dvMain)
                {
                    //*** Fill Datavalue for Numerator Indicator against each AreaID 
                    if (AreaCol.Count > 0)
                    {
                        if (AreaCol.ContainsKey(drv[Area.AreaNId]) == false)
                        {
                            ArrAreaNId.Add(drv[Area.AreaNId]);
                            AreaCol.Add(drv[Area.AreaNId], 0);
                            AreaCol[drv[Area.AreaNId]] = new Hashtable();
                            for (k = 0; k <= oSubgroup.Count - 1; k++)
                            {
                                ((Hashtable)AreaCol[drv[Area.AreaNId]]).Add((string)oSubgroup[k], 0);
                            }
                        }

                    }
                    else
                    {
                        ArrAreaNId.Add(drv[Area.AreaNId]);
                        AreaCol.Add(drv[Area.AreaNId], 0);
                        AreaCol[drv[Area.AreaNId]] = new Hashtable();
                        for (k = 0; k <= oSubgroup.Count - 1; k++)
                        {
                            ((Hashtable)AreaCol[drv[Area.AreaNId]]).Add((string)oSubgroup[k], 0);
                        }

                    }

                    ((Hashtable)AreaCol[drv[Area.AreaNId]])[(int)drv[SubgroupVals.SubgroupValNId]] = Convert.ToDouble(drv["Data_Value"]); 
                }


                ArrayList ArrAreaNIdOrg = new ArrayList();
                ArrAreaNIdOrg = ArrAreaNId;
                Hashtable AreaColOrg = new Hashtable();
                AreaColOrg = AreaCol;

                //----------------------- Retrive the index count before declaring array -------------- 
                i = 2;
                object ColItem;
                int iDataArrCtr = 0;
                for (j = lastAreaLevel - 1; j >= 1; j += -1)
                {
                    //?? 1 may not be the top level 
                    Hashtable AreaColTemp = new Hashtable();
                    ArrayList ArrAreaNIdTemp = new ArrayList();
                    for (int l = 0; l <= ArrAreaNId.Count - 1; l++)
                    {
                        ColItem = AreaCol[ArrAreaNId[l]];
                        dvArea = null;
                        string sSql = string.Empty;
                        sSql = this.DBQueries.Area.GetArea(Lib.DI_LibDAL.Queries.FilterFieldType.NId, ((int)ArrAreaNId[l]).ToString());
                        dvArea = this.DBConnection.ExecuteDataTable(sSql).DefaultView;

                        iAreaParentNId = Convert.ToInt32(dvArea[0][Area.AreaParentNId]);
                        if (AreaColTemp.Count == 0 || AreaColTemp.ContainsKey(iAreaParentNId) == false)
                        {
                            ArrAreaNIdTemp.Add(iAreaParentNId);
                            AreaColTemp.Add(iAreaParentNId, 0);
                            AreaColTemp[iAreaParentNId] = new Hashtable();
                        }
                        if (Step2SelectionDataTable.Rows[j - 1]["Checked"].ToString().Trim().Length > 0 && Convert.ToBoolean(Step2SelectionDataTable.Rows[j - 1]["Checked"]) == true)
                        {
                            for (k = 0; k <= oSubgroup.Count - 1; k++)
                            {
                                iDataArrCtr += 1;
                            }
                            i += 1;
                        }
                    }

                    dvMain.RowFilter = sOrgRowFilter + Indicator.IndicatorNId + " = " + Convert.ToInt32(this.Step1SelectionDataTable.Rows[0][Indicator.IndicatorNId]) + " AND Area_Level=" + Convert.ToString(j - 1);
                    dvMain.Sort = Timeperiods.TimePeriod + " DESC";
                    //*** GetMostRecent Data 
                    foreach (DataRowView drv in dvMain)
                    {
                        //*** Fill Datavalue for Numerator Indicator against each AreaID 
                        if (AreaColTemp.Count == 0 || AreaColTemp.ContainsKey(drv[Area.AreaNId]) == false)
                        {
                            ArrAreaNIdTemp.Add(Convert.ToInt32(drv[Area.AreaNId]));
                            AreaColTemp.Add(Convert.ToInt32(drv[Area.AreaNId]), 0);
                            AreaColTemp[drv[Area.AreaNId]] = new Hashtable();
                            for (k = 0; k <= oSubgroup.Count - 1; k++)
                            {
                                ((Hashtable)AreaColTemp[drv[Area.AreaNId]]).Add((string)oSubgroup[k], 0);
                            }
                        }
                    }
                    ArrAreaNId = ArrAreaNIdTemp;
                    AreaCol = AreaColTemp;
                }

                ArrAreaNId = ArrAreaNIdOrg;
                AreaCol = AreaColOrg;
                //----------------------------------------------------------------------------------- 

                //--------------------- Declare Array ----------------------------------------------- 
                string[,] msSummaryArray = new string[i - 1 + 3, 1 + oSubgroup.Count + 1];
                string[,] msTempDataArray = new string[iDataArrCtr, 6];

                msSummaryArray[0, 0] = LangStrings.AREAID;
                msSummaryArray[0, 1] = LangStrings.AREANAME;
                for (k = 0; k <= oSubgroup.Count - 1; k++)
                {
                    dvSubgroup.RowFilter = SubgroupVals.SubgroupValNId + " =" + Convert.ToInt32(oSubgroup[k]);
                    msSummaryArray[0, 2 + k] = dvSubgroup[0][SubgroupVals.SubgroupVal].ToString();
                }

                i = 2;
                iDataArrCtr = 0;
                for (j = lastAreaLevel - 1; j >= 1; j += -1)
                {
                    //??? 1 may not be the top level 
                    Hashtable AreaColTemp = new Hashtable();
                    ArrayList ArrAreaNIdTemp = new ArrayList();

                    for (int l = 0; l <= ArrAreaNId.Count - 1; l++)
                    {
                        ColItem = AreaCol[ArrAreaNId[l]];
                        dvArea = null;

                        string sSql = string.Empty;
                        sSql = this.DBQueries.Area.GetArea(Lib.DI_LibDAL.Queries.FilterFieldType.NId, ArrAreaNId[l].ToString());
                        dvArea = this.DBConnection.ExecuteDataTable(sSql).DefaultView;

                        //if (lv4_2_Level.Items(j - 1).Checked)
                        if (Step2SelectionDataTable.Rows[j - 1]["Checked"].ToString().Trim().Length > 0 && Convert.ToBoolean(Step2SelectionDataTable.Rows[j - 1]["Checked"]) == true)
                        {
                            msSummaryArray[i, 0] = dvArea[0][Area.AreaID].ToString();
                            msSummaryArray[i, 1] = dvArea[0][Area.AreaName].ToString();
                        }
                        // Get Parent area Nid
                        iAreaParentNId = Convert.ToInt32(dvArea[0][Area.AreaParentNId]);
                        if (AreaColTemp.Count == 0 || AreaColTemp.ContainsKey(iAreaParentNId) == false)
                        {
                            ArrAreaNIdTemp.Add(iAreaParentNId);
                            AreaColTemp.Add(iAreaParentNId, 0);
                            AreaColTemp[iAreaParentNId] = new Hashtable();
                            for (k = 0; k <= oSubgroup.Count - 1; k++)
                            {
                                ((Hashtable)AreaColTemp[iAreaParentNId]).Add(Convert.ToInt32(oSubgroup[k]), 0);
                            }
                        }

                        //if (lv4_2_Level.Items(j - 1).Checked)
                        if (Step2SelectionDataTable.Rows[j - 1]["Checked"].ToString().Trim().Length > 0 && Convert.ToBoolean(Step2SelectionDataTable.Rows[j - 1]["Checked"]) == true)
                        {
                            for (k = 0; k <= oSubgroup.Count - 1; k++)
                            {
                                msSummaryArray[i, k + 2] = Convert.ToString(((Hashtable)AreaCol[ArrAreaNId[l]])[(Convert.ToInt32(oSubgroup[k]))]);
                                msTempDataArray[iDataArrCtr, 0] = sTimePeriod;
                                msTempDataArray[iDataArrCtr, 1] = dvArea[0][Area.AreaID].ToString();
                                msTempDataArray[iDataArrCtr, 2] = dvArea[0][Area.AreaName].ToString();
                                msTempDataArray[iDataArrCtr, 3] = msSummaryArray[i, k + 2].ToString();
                                msTempDataArray[iDataArrCtr, 4] = (string)oSubgroupVal[k];
                                msTempDataArray[iDataArrCtr, 5] = sSource;
                                iDataArrCtr += 1;

                                //*** Bugfix 19 Apr 2007 Error for very high datavalue due to usage of CINT 
                                //CType(AreaColTemp.Item(iAreaParentNId), Hashtable).Item(CStr(oSubgroup.Item(k))) = 
                                //(CType(AreaColTemp.Item(iAreaParentNId), Hashtable).Item(CStr(oSubgroup.Item(k)))) +
                                //(CType(AreaCol.Item(ArrAreaNId(l)), Hashtable).Item(CStr(oSubgroup.Item(k))))
                                ((Hashtable)AreaColTemp[iAreaParentNId])[Convert.ToInt32(oSubgroup[k])] =
                                     Convert.ToDouble(((Hashtable)AreaColTemp[iAreaParentNId])[Convert.ToInt32(oSubgroup[k])]) + Convert.ToDouble(((Hashtable)AreaCol[ArrAreaNId[l]])[Convert.ToInt32(oSubgroup[k])]);
                            }
                            i += 1;
                        }
                    }

                    //*** BugFix 10 Aug 2006 Skipping of areas without child nodes at last level 
                    //dvMain.RowFilter = sOrgRowFilter + "Indicator_NId = " + lv4_1_Selected.Items(0).Tag + " AND Area_Level=" + j - 1;
                    dvMain.RowFilter = sOrgRowFilter + Indicator.IndicatorNId + " = "
                        + Convert.ToInt32(this.Step1SelectionDataTable.Rows[0][Indicator.IndicatorNId]) + " AND " + Area_Level.AreaLevel + " =" + Convert.ToString(j - 1);
                    dvMain.Sort = Timeperiods.TimePeriod + " DESC";
                    //*** GetMostRecent Data 
                    foreach (DataRowView DataviewRow in dvMain)
                    {
                        //*** Fill Datavalue for Numerator Indicator against each AreaID 
                        if (AreaColTemp.ContainsKey(DataviewRow[Area.AreaNId]) == false)
                        {
                            ArrAreaNIdTemp.Add(DataviewRow[Area.AreaNId].ToString());
                            AreaColTemp.Add(DataviewRow[Area.AreaNId], 0);
                            AreaColTemp[Convert.ToInt32(DataviewRow[Area.AreaNId])] = new Hashtable();
                            for (k = 0; k <= oSubgroup.Count - 1; k++)
                            {
                                ((Hashtable)AreaColTemp[DataviewRow[Area.AreaNId]]).Add((string)oSubgroup[k], 0);
                            }
                        }
                        //*** Overwrite the subtotal achieved through cumulation, if data for an area exists. 
                        ((Hashtable)AreaColTemp[DataviewRow[Area.AreaNId]])[Convert.ToInt32(DataviewRow[SubgroupVals.SubgroupValNId])] = Convert.ToDouble(DataviewRow[Data.DataValue]);
                    }
                    ArrAreaNId = ArrAreaNIdTemp;
                    AreaCol = AreaColTemp;
                }

                //Finish button error - SubTotal                 
                msDataArray = new string[iDataArrCtr, 6];
                for (i = 0; i <= iDataArrCtr - 1; i++)
                {
                    for (j = 0; j <= 5; j++)
                    {
                        msDataArray[i, j] = msTempDataArray[i, j].ToString();
                    }
                }

                msLogArray[0, 0] = LangStrings.AREAID;
                msLogArray[0, 1] = LangStrings.AREANAME;
                msLogArray[0, 2] = LangStrings.INDICATOR;
                msLogArray[0, 3] = LangStrings.SUBGROUP;
                msLogArray[0, 4] = LangStrings.TIME;
                msLogArray[0, 5] = LangStrings.DATAVALUE;
                msLogArray[0, 6] = LangStrings.UNIT;
                msLogArray[0, 7] = LangStrings.SOURCE;

                PrevAreaId = "";
                i = 1;

                dvMain.RowFilter = sOrgRowFilter + Indicator.IndicatorNId + " = " + Convert.ToInt32(this.Step1SelectionDataTable.Rows[0][Indicator.IndicatorNId]);// lv4_1_Selected.Items(0).Tag; 
                dvMain.GetEnumerator().Reset();
                dvMain.Sort = " Area_ID";

                foreach (DataRowView drv in dvMain)
                {
                    if (PrevAreaId != drv[Area.AreaID].ToString())
                    {
                        i += 1;
                        PrevAreaId = drv[Area.AreaID].ToString();
                        msLogArray[i, 0] = drv[Area.AreaID].ToString();
                        msLogArray[i, 1] = drv[Area.AreaName].ToString();
                    }
                    msLogArray[i, 2] = drv[Indicator.IndicatorName].ToString();
                    msLogArray[i, 3] = drv[SubgroupVals.SubgroupVal].ToString();
                    msLogArray[i, 4] = drv[Timeperiods.TimePeriod].ToString();
                    msLogArray[i, 5] = drv[Data.DataValue].ToString();
                    msLogArray[i, 6] = drv[Unit.UnitName].ToString();
                    msLogArray[i, 7] = drv[IndicatorClassifications.ICName].ToString();
                    i += 1;
                }

                //*** Special Handling for Subgroup in SubTotal            
                this.DESheetInformation.Subgroup = new string[oSubgroup.Count];
                this.DESheetInformation.SubgroupGUID = new string[oSubgroup.Count];
                for (i = 0; i <= oSubgroup.Count - 1; i++)
                {
                    string sSql = string.Empty;
                    sSql = this.DBQueries.Subgroup.GetSubgroupVals(Lib.DI_LibDAL.Queries.FilterFieldType.NId, oSubgroup[i].ToString());
                    dvMain = this.DBConnection.ExecuteDataTable(sSql).DefaultView;

                    this.DESheetInformation.Subgroup[i] = dvMain[0][SubgroupVals.SubgroupVal].ToString();
                    if (dvMain.Count > 0)
                    {
                        this.DESheetInformation.SubgroupGUID[i] = dvMain[0][SubgroupVals.SubgroupValGId].ToString();
                    }
                    else
                    {
                        this.DESheetInformation.SubgroupGUID[i] = Guid.NewGuid().ToString();
                    }
                }

                SummarySheet.Cells[0, 0].Value = LangStrings.VAR_WIZARD_NAME;
                SummarySheet.Cells[2, 0].Value = LangStrings.sMODULE;
                SummarySheet.Cells[2, 1].Value = LangStrings.VAR_WIZARD_NAME;

                SummarySheet.Cells[3, 0].Value = LangStrings.DATE_TIME;
                SummarySheet.Cells[3, 1].Value = TimeStamp;

                SummarySheet.Cells[4, 0].Value = LangStrings.LOG_FILE_NAME;
                SummarySheet.Cells[4, 1].Value = "Log_Subtotal (" + TimeStamp + ").xls";


                SummarySheet.Cells[8, 2].Value = this.DESheetInformation.Indicator;
                this.RaiseEventSetProgress(25);

                //*** Set Data 
                iDataCol = 2 + oSubgroup.Count;
                iCtrFillFrom = 9; //10;
                iCtrFillTo = iCtrFillFrom + msSummaryArray.GetLength(0) - 1;

                SummarySheet.Range[iCtrFillFrom, 0, iCtrFillTo, iDataCol - 1].Value = msSummaryArray;
                this.RaiseEventSetProgress(35);

                //Fill Data Values and Formula for each records 
                //*** Setting the Font 
                SummarySheet.Range[0, 0, iCtrFillTo, iDataCol - 1].Font.Name = this.FontName;
                SummarySheet.Range[0, 0, iCtrFillTo, iDataCol - 1].Font.Size = (float)this.FontSize;
                SummarySheet.Range[iCtrFillFrom, 0, iCtrFillTo, iDataCol - 1].Columns.AutoFit();
                //SummarySheet.Range[iCtrFillFrom, 0, iCtrFillTo, iDataCol - 1].Rows.AutoFit();


                SummarySheet.Cells[0, 0].Font.Bold = true;
                SummarySheet.Cells[0, 0].Font.Italic = true;
                SummarySheet.Cells[0, 0].Font.Size = this.TitleFontSize;
                SummarySheet.Cells[8, 2].Font.Bold = true;

                //*** Progress             
                this.RaiseEventSetProgress(45);

                //*********************************** S H O W L O G ************************************** 

                LogSheet.Cells[0, 0].Value = LangStrings.VAR_LOG_NAME;
                LogSheet.Cells[2, 0].Value = LangStrings.sMODULE;
                LogSheet.Cells[2, 1].Value = LangStrings.VAR_WIZARD_NAME;
                LogSheet.Cells[3, 0].Value = LangStrings.DATE_TIME;
                LogSheet.Cells[3, 1].Value = this.TimeStamp;
                LogSheet.Cells[4, 0].Value = LangStrings.LOG_FILE_NAME;
                LogSheet.Cells[4, 1].Value = "Log_Subtotol (" + this.TimeStamp + ").xls";

                iCtrFillFrom = 8;// 9;
                //*** Set Data 
                LogSheet.Range[iCtrFillFrom, 0, iCtrFillFrom + msLogArray.GetLength(0) - 1, 7].Value = msLogArray;
                //*** Autofit 
                LogSheet.Range[iCtrFillFrom, 0, iCtrFillFrom + msLogArray.GetLength(0) - 1, 7].Columns.AutoFit();
                LogSheet.Range[iCtrFillFrom, 4, iCtrFillFrom + msLogArray.GetLength(0) - 1, 4].HorizontalAlignment = HAlign.Left;// -4131; 
                //xlLeft 'Time column 

                //*** Set Font 
                LogSheet.Range[0, 0, iCtrFillFrom + msLogArray.GetLength(0) - 1, 7].Font.Name = this.FontName;
                LogSheet.Range[0, 0, iCtrFillFrom + msLogArray.GetLength(0) - 1, 7].Font.Size = (float)this.FontSize;

                LogSheet.Cells[0, 0].Font.Bold = true;
                LogSheet.Cells[0, 0].Font.Italic = true;
                LogSheet.Cells[0, 0].Font.Size = this.TitleFontSize;


                //****************************************************************************************** 

                this.RaiseEventSetProgress(100);
                this.DIExcel.GetWorksheet(0).Cells[0, 0].Activate();
                this.DIExcel.Save();

                //*** hide progress bar 
                this.RaiseEventHideProgressBar();
                SummarySheet = null;
                LogSheet = null;
                dvMain.RowFilter = string.Empty;

                //goExcel.SetParent(pnl4_3.Handle.ToInt32);                 
                this.RaiseEventCalculateStepCompleted("pnl4_3");
            }

            catch (Exception ex)
            {
            }
            finally
            {
                this.RaiseEventSetCursor(true);
            }

        }

        /// <summary>
        /// Perform Transform Unit Calculations
        /// </summary>
        /// <param name="step2UnitText">Name Of the Unit in step 2</param>
        /// <param name="step2ConversionTable"></param>
        public void FillTransformUnitSummaryLog(String step2UnitText, DataTable step2ConversionTable)
        {
            try
            {
                this.RaiseEventSetCursor(false);

                IWorksheet SummarySheet;
                IWorksheet LogSheet;
                int i = 0;
                int iAreaCount = 0;
                int iCtrFillFrom = 0;
                int iCtrFillTo = 0;
                int iDataCol = 0;
                StringCollection oSubgroup = new StringCollection();
                string sTempFillFromString = string.Empty;
                string sTempFillToString = string.Empty;

                // Initialize Progress Bar 
                this.RaiseEventSetProgress(1);


                // Copy Log file to temp location
                if (File.Exists(this.LogFilePath))
                {
                    File.Copy(this.LogFilePath, this.TempLogFilePath, true);
                    System.IO.File.SetAttributes(TempLogFilePath, FileAttributes.Normal);
                }                

                ApplyLanguageSettings();
                //  Opening and getting workbook                        
                this.DIExcel = new DIExcel(TempLogFilePath);
                SummarySheet = DIExcel.GetWorksheet(0);
                LogSheet = DIExcel.GetWorksheet(1);
                SummarySheet.Name = LangStrings.SUMMARY;
                LogSheet.Name = LangStrings.LOG;


                //*************************** SHOW INDEX ****************************************** 
                this.RaiseEventSetProgress(10);

                //data view with all filters applied                       
                DataView dvMain = this.PresentationData;

                // Apply Data Filters using selected Ind NId
                if (dvMain.RowFilter == "")
                {
                    dvMain.RowFilter = Indicator.IndicatorNId + " = " + this.Step1SelectionDataTable.Rows[0][Indicator.IndicatorNId];
                }
                else
                {
                    dvMain.RowFilter += " AND " + Indicator.IndicatorNId + " = " + this.Step1SelectionDataTable.Rows[0][Indicator.IndicatorNId]; ;
                }

                dvMain.Sort = Area.AreaID + " ASC, " + Unit.UnitName + " Asc";

                StringDictionary ColUnit = new StringDictionary();
                //foreach (DataRowView Drv in (DataView)dg5_2_Conversion.DataSource)
                foreach (DataRowView Drv in step2ConversionTable.DefaultView)
                {
                    ColUnit.Add(Drv[Unit.UnitNId].ToString(), Drv["Factor"].ToString());
                }

                msDataArray = new string[dvMain.Count, 6];
                string[,] msSummaryArray = new string[dvMain.Count + 2, 9];
                msSummaryArray[0, 0] = LangStrings.AREAID;
                msSummaryArray[0, 1] = LangStrings.AREANAME;
                msSummaryArray[0, 2] = LangStrings.UNIT;
                msSummaryArray[0, 3] = LangStrings.DATAVALUE;
                msSummaryArray[0, 4] = LangStrings.CONVERSION_FACTOR;
                msSummaryArray[0, 5] = step2UnitText;// txt5_2_Unit.Text; 
                msSummaryArray[0, 6] = LangStrings.SUBGROUP;
                msSummaryArray[0, 7] = LangStrings.TIME;
                msSummaryArray[0, 8] = LangStrings.SOURCE;

                string PrevAreaId = "";
                i += 2;
                foreach (DataRowView Drv in dvMain)
                {
                    //*** Fill Datavalue for Numerator Indicator against each AreaID 
                    if (PrevAreaId != Drv[Area.AreaID].ToString())
                    {
                        iAreaCount += 1;
                        PrevAreaId = Drv[Area.AreaID].ToString();
                    }
                    msSummaryArray[i, 0] = Drv[Area.AreaID].ToString();
                    msSummaryArray[i, 1] = Drv[Area.AreaName].ToString();
                    msSummaryArray[i, 2] = Drv[Unit.UnitName].ToString();
                    msSummaryArray[i, 3] = Drv[Data.DataValue].ToString();
                    msSummaryArray[i, 4] = ColUnit[Drv[Unit.UnitNId].ToString()].ToString();
                    msSummaryArray[i, 6] = Drv[SubgroupVals.SubgroupVal].ToString();

                    if (oSubgroup.Contains(Drv[SubgroupVals.SubgroupValNId].ToString()) == false)
                    {
                        oSubgroup.Add(Drv[SubgroupVals.SubgroupValNId].ToString());
                    }

                    msSummaryArray[i, 7] = Drv[Timeperiods.TimePeriod].ToString();
                    msSummaryArray[i, 8] = Drv[IndicatorClassifications.ICName].ToString();

                    msDataArray[i - 2, 0] = Drv[Timeperiods.TimePeriod].ToString();
                    msDataArray[i - 2, 1] = Drv[Area.AreaID].ToString();
                    msDataArray[i - 2, 2] = Drv[Area.AreaName].ToString();
                    msDataArray[i - 2, 4] = Drv[SubgroupVals.SubgroupVal].ToString();
                    msDataArray[i - 2, 5] = Drv[IndicatorClassifications.ICName].ToString();

                    i += 1;
                }
                ColUnit = null;

                string[,] msLogArray = new string[dvMain.Count + iAreaCount + 2, 8];
                //*** Data Array for Log Sheet 
                msLogArray[0, 0] = LangStrings.AREAID;
                msLogArray[0, 1] = LangStrings.AREANAME;
                msLogArray[0, 2] = LangStrings.INDICATOR;
                msLogArray[0, 3] = LangStrings.SUBGROUP;
                msLogArray[0, 4] = LangStrings.TIME;
                msLogArray[0, 5] = LangStrings.DATAVALUE;
                msLogArray[0, 6] = LangStrings.UNIT;
                msLogArray[0, 7] = LangStrings.SOURCE;

                PrevAreaId = "";
                i = 1;
                dvMain.GetEnumerator().Reset();
                foreach (DataRowView Drv in dvMain)
                {
                    if (PrevAreaId != Drv[Area.AreaID].ToString())
                    {
                        i += 1;
                        PrevAreaId = Drv[Area.AreaID].ToString();
                        msLogArray[i, 0] = Drv[Area.AreaID].ToString();
                        msLogArray[i, 1] = Drv[Area.AreaName].ToString();
                    }
                    msLogArray[i, 2] = Drv[Indicator.IndicatorName].ToString();
                    msLogArray[i, 3] = Drv[SubgroupVals.SubgroupVal].ToString();
                    msLogArray[i, 4] = Drv[Timeperiods.TimePeriod].ToString();
                    msLogArray[i, 5] = Drv[Data.DataValue].ToString();
                    msLogArray[i, 6] = Drv[Unit.UnitName].ToString();
                    msLogArray[i, 7] = Drv[IndicatorClassifications.ICName].ToString();
                    i += 1;
                }

                //SubgroupNId  in Step 5 - Transform Unit 
                //*** Special Handling for Subgroup in Transform Unit                           
                this.DESheetInformation.Subgroup = new string[oSubgroup.Count];
                this.DESheetInformation.SubgroupGUID = new string[oSubgroup.Count];

                for (i = 0; i <= oSubgroup.Count - 1; i++)
                {
                    //-- Using DAL library for getting dataView 
                    string sSql;
                    sSql = this.DBQueries.Subgroup.GetSubgroupVals(Lib.DI_LibDAL.Queries.FilterFieldType.NId, oSubgroup[i].ToString());
                    dvMain = this.DBConnection.ExecuteDataTable(sSql).DefaultView;

                    this.DESheetInformation.Subgroup[i] = dvMain[0][SubgroupVals.SubgroupVal].ToString();
                    if (dvMain.Count > 0)
                    {
                        this.DESheetInformation.SubgroupGUID[i] = dvMain[0][SubgroupVals.SubgroupValGId].ToString();
                    }
                    else
                    {
                        this.DESheetInformation.SubgroupGUID[i] = Guid.NewGuid().ToString();
                    }
                }

                SummarySheet.Cells[0, 0].Value = LangStrings.VAR_WIZARD_NAME;
                SummarySheet.Cells[2, 0].Value = LangStrings.sMODULE;
                SummarySheet.Cells[2, 1].Value = LangStrings.VAR_WIZARD_NAME;
                SummarySheet.Cells[3, 0].Value = LangStrings.DATE_TIME;
                SummarySheet.Cells[3, 1].Value = this.TimeStamp;
                SummarySheet.Cells[4, 0].Value = LangStrings.LOG_FILE_NAME;
                SummarySheet.Cells[4, 1].Value = "Log_Transform Unit (" + this.TimeStamp + ").xls";
                SummarySheet.Cells[6, 0].Value = LangStrings.DECIMALPLACES;
                SummarySheet.Cells[6, 1].Value = this.DecimalUpDownControlValue;// nud5_3_Decimals.Value;                     
                SummarySheet.Cells[7, 2].Value = Step1SelectionDataTable.Rows[0][0].ToString();// lv5_1_Selected.Items(0).SubItems(0).Text;

                this.RaiseEventSetProgress(25);
                iCtrFillTo = 8 + msSummaryArray.GetLength(0) - 1;
                iDataCol = 9;

                //*** Set Data 
                SummarySheet.Range[8, 0, iCtrFillTo, iDataCol - 1].Value = msSummaryArray;

                //*** Set Alignment 
                SummarySheet.Range[8, 0, iCtrFillTo, iDataCol - 1].Columns.AutoFit();
                SummarySheet.Range[7, 0, iCtrFillTo, iDataCol - 1].HorizontalAlignment = HAlign.Left;// -4131; 
                //xlLeft 
                SummarySheet.Range[8, 3, iCtrFillTo, 5].HorizontalAlignment = HAlign.Right;     //xlRight    
                this.RaiseEventSetProgress(35);

                //*** Set Formula for Transform Unit 
                string sChageValue;
                string sAddress;
                string sMinRange;
                string sMaxRange;
                IRange oRngStart;
                IRange oRngTotal;
                string TempString = string.Empty;
                iCtrFillFrom = 10;
                iDataCol = 6;
                sChageValue = "";
                sTempFillFromString = SummarySheet.Cells[iCtrFillFrom, 3].Address.Replace("$", "");
                TempString = SummarySheet.Cells[iCtrFillFrom, 4].Address.Replace("$", "");
                sChageValue = "ROUND(" + sTempFillFromString + "*" + TempString + "," + SummarySheet.Cells[6, 1].Address + ")";
                sAddress = SummarySheet.Cells[iCtrFillFrom, iDataCol - 1].Address;
                SummarySheet.Range[sAddress].Formula = "= " + sChageValue;
                sTempFillFromString = SummarySheet.Cells[iCtrFillFrom, iDataCol - 1].Address.Replace("$", "");
                oRngStart = SummarySheet.Range[sTempFillFromString];
                sTempFillToString = SummarySheet.Cells[iCtrFillTo, iDataCol - 1].Address.Replace("$", "");
                if (sTempFillFromString != sTempFillToString)
                {
                    sMaxRange = sTempFillFromString + ":" + sTempFillToString;
                    oRngTotal = SummarySheet.Range[sMaxRange];
                    this.AutoFillRange(oRngStart, oRngTotal);
                }

                //*** Set Font 
                iDataCol = 9;
                SummarySheet.Range[0, 0, iCtrFillTo, iDataCol - 1].Font.Name = this.FontName;
                SummarySheet.Range[0, 0, iCtrFillTo, iDataCol - 1].Font.Size = (float)this.FontSize;

                SummarySheet.Cells[0, 0].Font.Bold = true;
                SummarySheet.Cells[0, 0].Font.Italic = true;
                SummarySheet.Cells[0, 0].Font.Size = this.TitleFontSize;
                SummarySheet.Cells[7, 2].Font.Bold = true;

                //*** Progress                 
                this.RaiseEventSetProgress(45);

                //*********************************** S H O W L O G ************************************** 

                LogSheet.Cells[0, 0].Value = LangStrings.VAR_LOG_NAME;
                LogSheet.Cells[2, 0].Value = LangStrings.sMODULE;
                LogSheet.Cells[2, 1].Value = LangStrings.VAR_WIZARD_NAME;
                LogSheet.Cells[3, 0].Value = LangStrings.DATE_TIME;
                LogSheet.Cells[3, 1].Value = this.TimeStamp;
                LogSheet.Cells[4, 0].Value = LangStrings.LOG_FILE_NAME;
                LogSheet.Cells[4, 1].Value = "Log_Transform Unit (" + this.TimeStamp + ").xls";

                iCtrFillFrom = 8;
                iCtrFillTo = iCtrFillFrom + msLogArray.GetLength(0) - 1;
                iDataCol = 8;

                //*** Set Data 
                LogSheet.Range[iCtrFillFrom, 0, iCtrFillTo, iDataCol - 1].Value = msLogArray;

                //*** Set Alignment 
                LogSheet.Range[iCtrFillFrom, 0, iCtrFillTo, iDataCol - 1].Columns.AutoFit();
                LogSheet.Range[iCtrFillFrom, 4, iCtrFillTo, 4].HorizontalAlignment = HAlign.Left;
                //xlLeft 'Time column 

                //*** Set Font 
                LogSheet.Range[0, 0, iCtrFillTo, iDataCol - 1].Font.Name = this.FontName;
                LogSheet.Range[0, 0, iCtrFillTo, iDataCol - 1].Font.Size = (float)this.FontSize;

                LogSheet.Cells[0, 0].Font.Bold = true;
                LogSheet.Cells[0, 0].Font.Italic = true;
                LogSheet.Cells[0, 0].Font.Size = this.TitleFontSize;
                //******************************************************************************************                 
                this.RaiseEventSetProgress(100);

                // ExcelWorkBook.Sheets(1).Cells[1, 1].Activate(); 
                //*** Bring Focus to top of the page 
                DIExcel.GetWorksheet(0).Cells[0, 0].Activate();
                DIExcel.Save();
                //*** hide progress bar 
                this.RaiseEventHideProgressBar();
                SummarySheet = null;
                LogSheet = null;

                this.RaiseEventCalculateStepCompleted("pnl5_4");                
            }

            catch (Exception ex)
            {
            }
            finally
            {
                this.RaiseEventSetCursor(true);
            }
        }

        /// <summary>
        /// Perform Calculations for User defind formula
        /// </summary>
        /// <param name="FullFormulaText">txt6_1FullFormula text in UI application</param>
        /// <param name="formulaText">txt6_1Formula text</param>
        /// <param name="showSubgroup">mbShowsubgrop</param>
        /// <param name="step2SubgroupText">cbo6_2_Subgroup.Text</param>
        /// <param name="selNId2">arraylist miSelNId2 'Used for storing NID of Sel. Indicator </param>
        /// <param name="sIdentifierChar">msIdentifierChar</param>
        public void FillUserDefinedFormulaSummaryLog(string FullFormulaText, string formulaText, bool showSubgroup, string step2SubgroupText, ArrayList selNId2, StringCollection sIdentifierChar)
        {
            System.Threading.Thread thisThread = System.Threading.Thread.CurrentThread;
            System.Globalization.CultureInfo originalCulture = thisThread.CurrentCulture;

            try
            {
                thisThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                this.RaiseEventSetCursor(false);
                IWorksheet SummarySheet;
                IWorksheet LogSheet;
                int i = 0;
                int j = 0;
                int k = 0;
                string sFilter = string.Empty;
                int iCtrFillFrom = 0;
                int iCtrFillTo = 0;
                int iRecordCount = 0;
                string[] sSel_NId;
                string sSel_Indicator_NId = "";
                string sSel_Subgroup_Val_NId = "";
                DataView dvMain;
                string[,] msArray = new string[1, 8];  // For holding data

                //Variables for set formula
                string sChageValue = string.Empty;
                string sAddress = string.Empty;
                string sMinRange = string.Empty;
                string sMaxRange = string.Empty;
                IRange oRngStart;
                IRange oRngTotal;
                string TempFillFromString = string.Empty;
                string TempFillToString = string.Empty;

                this.RaiseEventSetProgress(1);
                //*** Initialize Progress Bar 

                // Copy Log File tempalate to temp location
                if (File.Exists(this.LogFilePath))
                {
                    File.Copy(this.LogFilePath, this.TempLogFilePath, true);
                    System.IO.File.SetAttributes(TempLogFilePath, FileAttributes.Normal);
                }
                
                // Apply language settings
                ApplyLanguageSettings();

                //  Opening and getting workbook                         
                this.DIExcel = new DIExcel(TempLogFilePath);

                SummarySheet = DIExcel.GetWorksheet(0);
                LogSheet = DIExcel.GetWorksheet(1);

                // Setting name for worksheets
                SummarySheet.Name = LangStrings.SUMMARY;
                LogSheet.Name = LangStrings.LOG;

                //*************************** SHOW INDEX ******************************************                 
                this.RaiseEventSetProgress(10);

                // Getting presentation data in dvMain Dataview
                dvMain = this.PresentationData;

                //*** Data Array for Log Sheet 
                // Filling Fixed columns of summery sheet with lang based strings
                SummarySheet.Cells[0, 0].Value = LangStrings.VAR_WIZARD_NAME;
                SummarySheet.Cells[2, 0].Value = LangStrings.sMODULE;
                SummarySheet.Cells[2, 1].Value = LangStrings.VAR_WIZARD_NAME;
                SummarySheet.Cells[3, 0].Value = LangStrings.DATE_TIME;
                SummarySheet.Cells[3, 1].Value = this.TimeStamp;
                SummarySheet.Cells[4, 0].Value = LangStrings.LOG_FILE_NAME;
                SummarySheet.Cells[4, 1].Value = "Log_User Defined Formula (" + this.TimeStamp + ").xls";
                SummarySheet.Cells[6, 0].Value = LangStrings.DECIMALPLACES;
                SummarySheet.Cells[6, 1].Value = this.DecimalUpDownControlValue; //nud6_2_Decimals.Value; 
                //"Decimal Value"                                    
                SummarySheet.Cells[9, 0].Value = LangStrings.AREAID;
                SummarySheet.Cells[9, 1].Value = LangStrings.AREANAME;
                SummarySheet.Cells[9, 2 + this.SelectedNids.Count].Value = FullFormulaText;


                this.RaiseEventSetProgress(25);

                //*** Get Selected I_S NId 
                // SelectedNid is an arraylist passed by clinet application
                for (i = 0; i <= this.SelectedNids.Count - 1; i++)
                {
                    // Split selected Nid to get indicator ad subgroup val NId
                    sSel_NId = this.SelectedNids[i].ToString().Split('_');
                    if (i == 0)
                    {
                        sSel_Indicator_NId = sSel_NId[0];
                        sSel_Subgroup_Val_NId = sSel_NId[1];
                    }
                    // Get comma delimited list of selected NIds
                    else
                    {
                        sSel_Indicator_NId = sSel_Indicator_NId + "," + sSel_NId[0];
                        sSel_Subgroup_Val_NId = sSel_Subgroup_Val_NId + "," + sSel_NId[1];
                    }

                    SummarySheet.Cells[8, 2 + i].Value = this.Step1SelectionDataTable.Rows[Convert.ToInt32(selNId2[i])][0].ToString(); // lv6_1_Available.Items(selNId2[i]).SubItems(0).Text; 
                    SummarySheet.Cells[9, 2 + i].Value = this.Step1SelectionDataTable.Rows[Convert.ToInt32(selNId2[i])][1].ToString();// lv6_1_Available.Items(selNId2[i]).SubItems(1).Text; 
                }

                //*** Filter records for selected Indicator / Subgroup in calculates step1 . using mbshowSubgroup
                if (showSubgroup)
                {
                    sFilter = Indicator.IndicatorNId + " IN (" + sSel_Indicator_NId + ") AND " + SubgroupVals.SubgroupValNId + " IN (" + sSel_Subgroup_Val_NId + ")";
                }
                else
                {
                    sFilter = Indicator.IndicatorNId + " IN (" + sSel_Indicator_NId + ")";
                }

                //*** Bugfix 21 Dec 2006 Data Filters not applied 
                if (dvMain.RowFilter == "")
                {
                }
                else
                {
                    sFilter = dvMain.RowFilter + " AND " + sFilter;
                }


                //Fill Unique Area 
                dvMain.Sort = Area.AreaID + " ASC";
                iCtrFillFrom = 11;// 12; 
                for (i = 0; i <= dvMain.Count - 1; i++)
                {
                    //if (SummarySheet.Cells[iRecordCount + iCtrFillFrom , 0].Value ==null || dvMain[i]["Area_ID"].ToString() != SummarySheet.Cells[iRecordCount + iCtrFillFrom-1 , 0].Value.ToString() + "")
                    if (SummarySheet.Cells[iRecordCount + iCtrFillFrom - 1, 0].Value == null || dvMain[i]["Area_ID"].ToString() != SummarySheet.Cells[iRecordCount + iCtrFillFrom - 1, 0].Value.ToString() + "")
                    {
                        SummarySheet.Cells[iRecordCount + iCtrFillFrom, 0].Value = dvMain[i][Area.AreaID].ToString();
                        SummarySheet.Cells[iRecordCount + iCtrFillFrom, 1].Value = dvMain[i][Area.AreaName].ToString();
                        iRecordCount += 1;
                    }
                }
                iCtrFillTo = iCtrFillFrom + iRecordCount - 1;

                this.RaiseEventSetProgress(35);

                //*** Set Data Array for Log Sheet simultaneously                  
                msDataArray = new string[iRecordCount, 6];
                msArray = new string[(iRecordCount * this.SelectedNids.Count) + iRecordCount + 3, 8];
                msArray[0, 0] = LangStrings.AREAID;
                msArray[0, 1] = LangStrings.AREANAME;
                msArray[0, 2] = LangStrings.INDICATOR;
                msArray[0, 3] = LangStrings.SUBGROUP;
                msArray[0, 4] = LangStrings.TIME;
                msArray[0, 5] = LangStrings.DATAVALUE;
                msArray[0, 6] = LangStrings.UNIT;
                msArray[0, 7] = LangStrings.SOURCE;
                k += 2;

                //*** Fill Data Values and Formula for each Area 
                int iDataCol;
                bool bSupressArea = false;
                for (i = iCtrFillFrom; i <= iCtrFillTo; i++)
                {
                    for (j = 0; j <= this.SelectedNids.Count - 1; j++)
                    {
                        iDataCol = 3 + j;
                        sSel_NId = this.SelectedNids[j].ToString().Split('_');
                        dvMain.RowFilter = sFilter + " AND " + Area.AreaID + " = '" + Utility.DICommon.EscapeWildcardChar(Utility.DICommon.RemoveQuotes(SummarySheet.Cells[i, 0].Value.ToString())) + "'";
                        dvMain.RowFilter += " AND " + Indicator.IndicatorNId + "=" + sSel_NId[0] + " AND Subgroup_Val_NId =" + sSel_NId[1];
                        if (dvMain.Count > 0)
                        {

                            SummarySheet.Cells[i, iDataCol - 1].Value = dvMain[0][Data.DataValue].ToString();
                            //SummarySheet.Cells[i, iDataCol - 1].Value =dvMain[0]["Data_Value"].ToString().Replace(",","."); 
                            if (bSupressArea == false)
                            {
                                msArray[k, 0] = dvMain[0][Area.AreaID].ToString();
                                msArray[k, 1] = dvMain[0][Area.AreaName].ToString();

                                msDataArray[i - iCtrFillFrom, 0] = dvMain[0][Timeperiods.TimePeriod].ToString();
                                msDataArray[i - iCtrFillFrom, 1] = dvMain[0][Area.AreaID].ToString();
                                msDataArray[i - iCtrFillFrom, 2] = dvMain[0][Area.AreaName].ToString();
                                msDataArray[i - iCtrFillFrom, 4] = step2SubgroupText;
                                msDataArray[i - iCtrFillFrom, 5] = dvMain[0][IndicatorClassifications.ICName].ToString();
                                bSupressArea = true;
                            }
                            msArray[k, 2] = dvMain[0][Indicator.IndicatorName].ToString();
                            msArray[k, 3] = dvMain[0][SubgroupVals.SubgroupVal].ToString();
                            msArray[k, 4] = dvMain[0][Timeperiods.TimePeriod].ToString();
                            msArray[k, 5] = dvMain[0][Data.DataValue].ToString();
                            msArray[k, 6] = dvMain[0][Unit.UnitName].ToString();
                            msArray[k, 7] = dvMain[0][IndicatorClassifications.ICName].ToString();
                            k += 1;
                        }
                    }
                    k += 1;
                    bSupressArea = false;
                }


                //*** Set Formula               

                //*** Set the formula for User Defined Formula 

                // Total no of data columns
                iDataCol = 3 + this.SelectedNids.Count;
                sChageValue = formulaText; //txt6_1_Formula.Text; 

                for (i = 0; i <= sIdentifierChar.Count - 1; i++)
                {
                    sChageValue = sChageValue.Replace(sIdentifierChar[i], "_" + sIdentifierChar[i] + "_");
                }
                for (i = 0; i <= this.SelectedNids.Count - 1; i++)
                {
                    TempFillFromString = SummarySheet.Cells[iCtrFillFrom, 2 + i].Address.Replace("$", "");
                    //                    sChageValue = sChageValue.Replace("_" +  lv6_1_Available.Items(selNId2[i]).SubItems(2).Text + "_", TempFillFromString); 
                    sChageValue = sChageValue.Replace("_" + this.Step1SelectionDataTable.Rows[Convert.ToInt32(selNId2[i])][2].ToString() + "_", TempFillFromString);
                }

                sChageValue = "IF(TYPE(" + sChageValue + ")=16,\"\",ROUND(" + sChageValue + "," + SummarySheet.Cells[6, 1].Address + "))";
                sAddress = SummarySheet.Cells[iCtrFillFrom, iDataCol - 1].Address;
                SummarySheet.Range[sAddress].Formula = "= " + sChageValue;

                TempFillFromString = SummarySheet.Cells[iCtrFillFrom, iDataCol - 1].Address.Replace("$", "");
                oRngStart = SummarySheet.Range[TempFillFromString];

                TempFillToString = SummarySheet.Cells[iCtrFillTo, iDataCol - 1].Address.Replace("$", "");
                if (TempFillFromString != TempFillToString)
                {
                    sMaxRange = TempFillFromString + ":" + TempFillToString;
                    oRngTotal = SummarySheet.Range[sMaxRange];
                    this.AutoFillRange(oRngStart, oRngTotal);
                }

                //*** Setting the Font 
                SummarySheet.Range[0, 0, iCtrFillTo, iDataCol + 3].Font.Name = this.FontName;
                SummarySheet.Range[0, 0, iCtrFillTo, iDataCol + 3].Font.Size = (float)this.FontSize;
                SummarySheet.Range[6, 0, iCtrFillTo, iDataCol + 3].Columns.AutoFit();

                SummarySheet.Cells[0, 0].Font.Bold = true;
                SummarySheet.Cells[0, 0].Font.Italic = true;
                SummarySheet.Cells[0, 0].Font.Size = this.TitleFontSize;

                //*** Progress 
                this.RaiseEventSetProgress(45);

                //*********************************** S H O W L O G **************************************              
                LogSheet.Cells[0, 0].Value = LangStrings.VAR_LOG_NAME;
                LogSheet.Cells[2, 0].Value = LangStrings.sMODULE;
                LogSheet.Cells[2, 1].Value = LangStrings.VAR_WIZARD_NAME;
                LogSheet.Cells[3, 0].Value = LangStrings.DATE_TIME;
                LogSheet.Cells[3, 1].Value = this.TimeStamp;
                LogSheet.Cells[4, 0].Value = LangStrings.LOG_FILE_NAME;
                LogSheet.Cells[4, 1].Value = "Log_User Defined Formula (" + this.TimeStamp + ").xls";

                iCtrFillFrom = 8;//9; 
                //*** Set Data 
                LogSheet.Range[iCtrFillFrom, 0, iCtrFillFrom + msArray.GetLength(0) - 1, 7].Value = msArray;
                //*** Autofit 
                LogSheet.Range[iCtrFillFrom - 2, 0, iCtrFillFrom + msArray.GetLength(0) - 1, 7].Columns.AutoFit();
                //*** Set Font 
                LogSheet.Range[0, 0, iCtrFillFrom + msArray.GetLength(0) - 1, 7].Font.Name = this.FontName;
                LogSheet.Range[0, 0, iCtrFillFrom + msArray.GetLength(0) - 1, 7].Font.Size = (float)this.FontSize;

                LogSheet.Cells[0, 0].Font.Bold = true;
                LogSheet.Cells[0, 0].Font.Italic = true;
                LogSheet.Cells[0, 0].Font.Size = this.TitleFontSize;

                //****************************************************************************************** 
                this.RaiseEventSetProgress(100);
                this.DIExcel.GetWorksheet(0).Cells[0, 0].Activate();
                DIExcel.SaveAs(TempLogFilePath);

                //*** hide progress bar                 
                this.RaiseEventHideProgressBar();
                SummarySheet = null;
                LogSheet = null;
                dvMain.RowFilter = string.Empty;

            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.RaiseEventSetCursor(true);
            }
            thisThread.CurrentCulture = originalCulture;
            this.RaiseEventCalculateStepCompleted("pnl6_3");
        }

        /// <summary>
        /// Get Data From Summary sheet for creation of DES
        /// </summary>
        public void GetDESheetDataFromSummarySheet(System.Globalization.CultureInfo currentCulture)
        {
            //System.Threading.Thread thisThread = System.Threading.Thread.CurrentThread;
            //System.Globalization.CultureInfo originalCulture = thisThread.CurrentCulture;

            try
            {
                // set current thread culture to english
                //thisThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

                int i = 0;
                int j = 0;
                int iDataCol = 0;

                //this.DIExcel = new DIExcel(TempLogFilePath);
                this.DIExcel = new DIExcel(TempLogFilePath, currentCulture);
                IWorksheet SummarySheet = DIExcel.GetWorksheet(0);
                //Excel.Worksheet 
                switch (ApplicationWizardType)
                {
                    case WizardType.Percent:
                        //*** Decimal 
                        if (Information.IsNumeric(SummarySheet.Cells[6, 1].Value))
                        {
                            this.DESheetInformation.Decimals = Convert.ToInt32(SummarySheet.Cells[6, 1].Value);
                            this.DecimalUpDownControlValue = this.DESheetInformation.Decimals;
                        }
                        iDataCol = 3 + Step1SelectionDataTable.Rows.Count + Step2SelectionDataTable.Rows.Count;

                        //*** Indicator & Unit                         
                        SetDESheetIUSGuid(SummarySheet.Cells[9, iDataCol + 1].Text, SummarySheet.Cells[10, iDataCol + 1].Text, "");

                        // raise to set step3 indicator and unit text
                        RaiseEventSetStep3IndicatorAndUnit(DESheetInformation.Indicator, this.DESheetInformation.Unit);
                        //*** Data Values 
                        for (i = 0; i <= msDataArray.GetLength(0) - 1; i++)
                        {
                            //msDataArray(i, 3) = SummarySheet.Cells[i + 12, iDataCol].value;
                            msDataArray[i, 3] = SummarySheet.Cells[i + 12, iDataCol + 1].Text;
                        }

                        break;
                    case WizardType.HundredMinus:
                        //*** Decimal 
                        if (Information.IsNumeric(SummarySheet.Cells[6, 1].Value))
                        {
                            this.DESheetInformation.Decimals = Convert.ToInt32(SummarySheet.Cells[6, 1].Value);
                            this.DecimalUpDownControlValue = (decimal)this.DESheetInformation.Decimals;
                        }
                        //*** Data Values 
                        iDataCol = 3;
                        for (i = 0; i <= msDataArray.GetLength(0) - 1; i++)
                        {
                            msDataArray[i, 3] = SummarySheet.Cells[i + 10, iDataCol].Value.ToString();
                        }
                        break;
                    case WizardType.CompositeIndex:
                        //*** Decimal 
                        if (Information.IsNumeric(SummarySheet.Cells[6, 4].Value))
                        {
                            this.DESheetInformation.Decimals = Convert.ToInt32(SummarySheet.Cells[6, 4].Value);
                            this.DecimalUpDownControlValue = (decimal)this.DESheetInformation.Decimals;
                        }

                        //*** Data Values                        
                        iDataCol = 2 + (this.Step1SelectionDataTable.Rows.Count * 2) ; //*** 1 for Score Col                       

                        //*** Handling Excel Sorting for Composite Index 
                        string[] ArrAreaId = new string[msDataArray.GetLength(0)];
                        for (i = 0; i <= msDataArray.GetLength(0) - 1; i++)
                        {
                            ArrAreaId[i] = msDataArray[i, 1];
                        }

                        string[,] sTempDataArray = new string[msDataArray.GetLength(0), msDataArray.GetLength(1)];
                        int ArrIndex;
                        for (i = 0; i <= msDataArray.GetLength(0) - 1; i++)
                        {
                            ArrIndex = Array.IndexOf(ArrAreaId, SummarySheet.Cells[i + 16, iDataCol+1].Value);
                            for (j = 0; j <= msDataArray.GetLength(1) - 1; j++)
                            {
                                if (j == 3)
                                {
                                    sTempDataArray[i, 3] = SummarySheet.Cells[i + 16, iDataCol].Value.ToString();
                                }
                                else
                                {
                                    sTempDataArray[i, j] = msDataArray[ArrIndex, j];
                                }
                            }
                        }
                        msDataArray = null;
                        msDataArray = sTempDataArray;
                        break;

                    case WizardType.SubTotal:
                        // No decimal Combo 
                        //Data Values 
                        //Subtotal Step 5 
                        iDataCol = 3;
                        for (i = 0; i <= (msDataArray.GetLength(0) / this.DESheetInformation.SubgroupGUID.Length) - 1; i++)
                        {
                            for (j = 0; j <= this.DESheetInformation.SubgroupGUID.Length - 1; j++)
                            {
                                msDataArray[i + j, 3] = SummarySheet.Cells[i + 11, (iDataCol + j) - 1].Value.ToString();
                            }
                        }
                        break;


                    case WizardType.TransformUnit:
                        ////*** Decimal 
                        if (Information.IsNumeric(SummarySheet.Cells[6, 1].Value))
                        {
                            this.DESheetInformation.Decimals = Convert.ToInt32(SummarySheet.Cells[6, 1].Value);
                            this.DecimalUpDownControlValue = (decimal)this.DESheetInformation.Decimals;
                        }

                        //*** Data Values 
                        iDataCol = 5;
                        for (i = 0; i <= msDataArray.GetLength(0) - 1; i++)
                        {
                            msDataArray[i, 3] = SummarySheet.Cells[i + 10, iDataCol].Value.ToString();
                        }

                        break;
                    case WizardType.UserDefinedFormula:
                        //*** Decimal 
                        if (Information.IsNumeric(SummarySheet.Cells[6, 1].Value))
                        {
                            this.DESheetInformation.Decimals = Convert.ToInt32(SummarySheet.Cells[6, 1].Value);
                            this.DecimalUpDownControlValue = (decimal)this.DESheetInformation.Decimals;
                        }

                        //*** Data Values                         
                        iDataCol = 2 + this.SelectedNids.Count;
                        for (i = 0; i <= msDataArray.GetLength(0) - 1; i++)
                        {
                            msDataArray[i, 3] = SummarySheet.Cells[i + 11, iDataCol].Value.ToString();


                        }
                        break;
                }

                SummarySheet = null;

            }
            catch (Exception ex)
            {
            }
            // Reset Original culture
            //thisThread.CurrentCulture = originalCulture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sIndicator"></param>
        /// <param name="sUnit"></param>
        /// <param name="sSubgroup"></param>
        public void SetDESheetIUSGuid(string sIndicator, string sUnit, string sSubgroup)
        {
            DataView IndicatorDataView;
            string sSql = string.Empty;

            //*** Set Indicator GUID 
            if (sIndicator != "")
            {
                //moDESheetInfo.Indicator = sIndicator;
                DataEntrySheetInfo.Indicator = sIndicator;

                //-- Using DAL library for getting dataView                 
                sSql = this.DBQueries.Indicators.GetIndicator(Lib.DI_LibDAL.Queries.FilterFieldType.Name, "'" + DataEntrySheetInfo.Indicator + "'", Lib.DI_LibDAL.Queries.FieldSelection.Light);
                IndicatorDataView = this.DBConnection.ExecuteDataTable(sSql).DefaultView;

                if ((IndicatorDataView != null) && IndicatorDataView.Count > 0)
                {
                    DESIndicatorGUId = IndicatorDataView[0][Indicator.IndicatorGId].ToString();
                }
                else
                {
                    DESIndicatorGUId = Guid.NewGuid().ToString();
                }
            }

            //*** Set Unit GUID 
            if (sUnit != "")
            {
                DESheetInformation.Unit = sUnit;

                //-- Using DAL library for getting dataView 
                sSql = this.DBQueries.Unit.GetUnit(Lib.DI_LibDAL.Queries.FilterFieldType.Name, "'" + DESheetInformation.Unit + "'");
                IndicatorDataView = this.DBConnection.ExecuteDataTable(sSql).DefaultView;

                if ((IndicatorDataView != null) && IndicatorDataView.Count > 0)
                {
                    DESUnitGUId = IndicatorDataView[0][Unit.UnitGId].ToString();
                }
                else
                {
                    DESUnitGUId = Guid.NewGuid().ToString();
                }
            }

            //*** Set Subgroup GUID '*** Special Handling (Multiple Subgroup) for 100 Minus Subgroup done in Fill100MinusSummaryLog (Also SubTotal and Transform unit) 
            if (sSubgroup != "")
            {
                DESSubgroupText[0] = sSubgroup;

                //-- Using DAL library for getting dataView                 
                sSql = this.DBQueries.Subgroup.GetSubgroupVals(Lib.DI_LibDAL.Queries.FilterFieldType.Name, "'" + DESSubgroupText[0].ToString() + "'");
                IndicatorDataView = this.DBConnection.ExecuteDataTable(sSql).DefaultView;

                if ((IndicatorDataView != null) && IndicatorDataView.Count > 0)
                {
                    //oDESheetInfo.SubgroupGUID(0) = oDv.Item(0)("Subgroup_Val_GId");
                    DESSubgroupGUId[0] = IndicatorDataView[0][SubgroupVals.SubgroupValGId].ToString();
                }
                else
                {
                    DESSubgroupGUId[0] = Guid.NewGuid().ToString();
                }
            }

            IndicatorDataView = null;
        }

        /// <summary>
        ///  Fill Data into Data entry spreadsheet
        /// </summary>
        /// <param name="parentPanelName">Name Of the parent Panel of client application in which DES will be shown </param>
        /// <remarks> This panel name will be used in raising event at end to display this DES generated</remarks>
        public void FillDESheetData(string parentPanelName)
        {
            IWorksheet DataSheet;
            int i;
            int iCtrFillTo = 11;
            ///Copy Data entry template file to temp location
            if (File.Exists(DESFilePath))
            {
                File.Copy(DESFilePath, TempDESFilePath, true);
                System.IO.File.SetAttributes(TempDESFilePath, FileAttributes.Normal);
            }

            // Open excel workbook
            DIExcel = new DIExcel(TempDESFilePath);

            //Get Data Entry sheet            
            DataSheet = DIExcel.GetWorksheet(0);
            DataSheet.Name = LangStrings.DATA;

            // if msDataArray has some Value then Reset ictrFillTo value
            //int iCtrFillTo = 11 + msDataArray.GetLength(0) - 1;
            if (msDataArray.GetLength(0) > 0)
            {
                iCtrFillTo = 11 + msDataArray.GetLength(0) - 1;
            }
            //*** Set Data 

            DataSheet.Cells[0, 0].Value = LangStrings.DEVINFO_DATA_ENTRY_SPREADSHEET;
            DataSheet.Cells[2, 0].Value = msClassification[SelectedClassificationIndex].ToString();
            DataSheet.Cells[3, 0].Value = LangStrings.IC_CLASS;
            DataSheet.Cells[4, 0].Value = LangStrings.INDICATOR;
            DataSheet.Cells[6, 0].Value = LangStrings.UNIT;
            DataSheet.Cells[6, 6].Value = LangStrings.DECIMALPLACES;
            DataSheet.Cells[8, 0].Value = LangStrings.TIME;
            DataSheet.Cells[8, 1].Value = LangStrings.AREAID;
            DataSheet.Cells[8, 2].Value = LangStrings.AREANAME;
            DataSheet.Cells[8, 3].Value = LangStrings.DATAVALUE;
            DataSheet.Cells[8, 4].Value = LangStrings.SUBGROUP;
            DataSheet.Cells[8, 5].Value = LangStrings.SOURCE;
            DataSheet.Cells[8, 6].Value = LangStrings.FOOTNOTES;
            DataSheet.Cells[8, 7].Value = LangStrings.DENOMINATOR;
            DataSheet.Cells[8, 11].Value = "";

            DataSheet.Cells[2, 1].Value = DESheetInformation.SectorName;
            DataSheet.Cells[2, 11].Value = DESheetInformation.SectorGUID;
            DataSheet.Cells[3, 1].Value = DESheetInformation.ClassName;
            DataSheet.Cells[3, 11].Value = DESheetInformation.ClassGUID;

            DataSheet.Cells[4, 1].Value = DESheetInformation.Indicator;// DataEntrySheetInfo.Indicator;
            DataSheet.Cells[4, 11].Value = DESheetInformation.IndicatorGUID;// DESIndicatorGUId;

            DataSheet.Cells[6, 1].Value = DESheetInformation.Unit;
            DataSheet.Cells[6, 11].Value = DESheetInformation.IndicatorGUID;// DESUnitGUId;                
            DataSheet.Cells[6, 7].Value = DESheetInformation.Decimals;

            //*** Logic for showing all records irrespective of blank data value 
            //DataSheet.Range[DataSheet.Cells[11, 1], DataSheet.Cells[iCtrFillTo, 6]].Value = msDataArray;
            //If Data Array has Values then show it in Datasheet
            if (msDataArray.GetLength(0) > 0)
            {
                DataSheet.Range[10, 0, iCtrFillTo - 1, 5].Value = msDataArray;
            }
            //DataSheet.Range[10, 0, iCtrFillTo - 1, 5].Value = msDataArray;
            miCtrFillTo = iCtrFillTo;

            //*** Special Handling in case of 100 Minus, SubTotal and TransformUnit (multiple subgroup) 
            if ((ApplicationWizardType == WizardType.HundredMinus | ApplicationWizardType == WizardType.SubTotal | ApplicationWizardType == WizardType.TransformUnit) && DESheetInformation.Subgroup.Length > 1)
            {
                string sSubgroup;
                for (i = 10; i <= miCtrFillTo - 1; i++)
                {
                    //sSubgroup = DataSheet.Cells[i, 5].value;
                    sSubgroup = DataSheet.Cells[i, 4].Text;
                    if ((sSubgroup != null))
                        // DataSheet.Cells[i, 12] = moDESheetInfo.SubgroupGUID(Array.IndexOf(moDESheetInfo.Subgroup, sSubgroup));

                        DataSheet.Cells[i, 11].Value = DESheetInformation.SubgroupGUID[Array.IndexOf(DESheetInformation.Subgroup, sSubgroup)].ToString();
                    //DataSheet.Cells[i, 11].Value = DESSubgroupGUId[Array.IndexOf(DESSubgroupText, sSubgroup)];
                }
            }
            else
            {
                if ( DESheetInformation.Subgroup.Length > 1)
                {
                     DataSheet.Range[10, 11, miCtrFillTo - 1, 11].Value = DESheetInformation.SubgroupGUID[0];// moDESheetInfo.SubgroupGUID(0);
                }               
            }
            DataSheet.Range[10, 0, miCtrFillTo - 1, 5].Font.Name = FontName;
            DataSheet.Range[10, 0, miCtrFillTo - 1, 5].Font.Size = FontSize;
            try
            {
                DataSheet.Range[2, 0, miCtrFillTo - 1, 5].Columns.AutoFit();
            }
            catch (Exception ex)
            {

                throw ex;
            }


            DIExcel.Save();
            this.DESLastDataRowIndex = miCtrFillTo;
            RaiseEventStep5Completed(parentPanelName);


        }

        /// <summary>
        /// Insert Newly added records from DES to database
        /// </summary>
        /// <param name="sDestDBPath">Destination Database Path</param>
        /// <param name="MsAccessPwd">Password</param>
        /// <param name="sLng_Suffix">Lang suffix</param>
        public void InsertNewRecords(String tempDESheetPath, string sDestDBPath, String MsAccessPwd)
        {
            DataView oDV = null;
            DataRowView oDVRow;
            string sDB_Prefix;
            string sQry = "";
            DIConnection DBConnection;
            int i;
            int j;
            int iIndicatorNId = 0;
            int iUnitNId = 0;

            int[] iSubgroupNId = new int[this.DESheetInformation.Subgroup.Length];
            int[] iIUSNId = new int[this.DESheetInformation.Subgroup.Length];
            int iSectorNID = 0;
            //int[] iClassNId = new int[Strings.Split(this.DESheetInformation.ClassGUID, "{}").Length];
            int[] iClassNId = new int[this.DESheetInformation.ClassGUID.Split("{}".ToCharArray()).Length];
            int iParentNId;
            //Used for Class and Source 
            bool bNewIUS = false;
            Boolean bNewSector = false;
            bool bNewClass = false;
            //*** True if any of I / U / S is new 
            bool SectorGlobal = false;


            // -- Create New DiConnection with db as sDestDBPath 
            DBConnection = new DIConnection(DIServerType.MsAccess, "", "", sDestDBPath, "", MsAccessPwd);

            //--- Using DAL for getting Database default Prefix   
            sDB_Prefix = DBConnection.DIDataSetDefault();
            String sLng_Suffix = DBConnection.DILanguageCodeDefault(sDB_Prefix);
            DIQueries DBQueries = new DIQueries(sDB_Prefix, DBConnection.DILanguageCodeDefault(sDB_Prefix));

            // Get SectorNId
            iSectorNID = GetSectorNId(DBConnection, DBQueries, out bNewSector, out bNewSector);

            //*** If Selected Class has not been inserted by export routine then insert new Class 
            string[] sClassGuid;
            string[] sClassName;
            string[] sClassGlobal;
            iParentNId = iSectorNID;
            //sClassGuid = Strings.Split(this.DESheetInformation.ClassGUID,"{}");     
            sClassGuid = this.DESheetInformation.ClassGUID.Split("{}".ToCharArray());
            sClassName = this.DESheetInformation.ClassName.Split("{}".ToCharArray());
            sClassGlobal = this.DESheetInformation.ClassGlobal.Split("{}".ToCharArray());

            //*** BugFix 29 Aug 2006 Do not try to insert class into database if indicator is directly associated with sector 
            if (this.DESheetInformation.ClassGUID != "" & this.DESheetInformation.ClassName != "")
            {
                for (i = 0; i <= sClassGuid.Length - 1; i++)
                {
                    iClassNId[i] = GetClassNId(DBConnection, DBQueries, out bNewClass, sClassGuid[i].ToString(), SectorGlobal, iParentNId, sClassName[i].ToString());
                    iParentNId = Convert.ToInt32(iClassNId[i]);
                }
            }


            //*** Indicator_NId 
            sQry = DBQueries.Indicators.GetIndicator(FilterFieldType.GId, "'" + this.DESheetInformation.IndicatorGUID + "'", FieldSelection.NId);
            iIndicatorNId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));

            if (iIndicatorNId == 0)
            {
                bNewIUS = true;
                //-- Getting insert query using DAL 
                sQry = DI_LibDAL.Queries.Indicator.Insert.InsertIndicator(sDB_Prefix, sLng_Suffix, this.DESheetInformation.Indicator.Replace("'", "''"), this.DESheetInformation.IndicatorGUID, "", false);

                //-- Using DAL for query execution and getting identity value 
                DBConnection.ExecuteNonQuery(sQry);
                iIndicatorNId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
            }

            //*** Unit_NId 
            //-- Using DAL for getting and executing query for getting unit NId 
            sQry = DBQueries.Calculates.GetUnitNIdByGId(this.DESheetInformation.UnitGUID);
            iUnitNId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));

            if (iUnitNId == 0)
            {
                bNewIUS = true;
                //-- Getting insert query using DAL 
                sQry = DI_LibDAL.Queries.Unit.Insert.InsertUnit(sDB_Prefix, sLng_Suffix, this.DESheetInformation.Unit.Replace("'", "''"), this.DESheetInformation.UnitGUID, false);

                //-- Using DAL for query execution and getting identity value 
                DBConnection.ExecuteNonQuery(sQry);
                iUnitNId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
            }

            //*** Subgroup_Val_NId 
            for (i = 0; i <= iSubgroupNId.Length - 1; i++)
            {

                sQry = DBQueries.Calculates.GetSubgroupValNIdByGId(this.DESheetInformation.SubgroupGUID[i]);
                iSubgroupNId[i] = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));

                if (iSubgroupNId[i] == 0)
                {
                    bNewIUS = true;
                    //-- Getting insert query using DAL             
                    sQry = DBQueries.Calculates.InsertSubgroupVal(this.DESheetInformation.Subgroup[0].Replace("'", "''"), this.DESheetInformation.SubgroupGUID[0], false);

                    //-- Using DAL for query execution and getting identity value 
                    DBConnection.ExecuteNonQuery(sQry);
                    iSubgroupNId[i] = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                }
            }

            //*** IUSNId 
            if (bNewIUS)
            {
                //*** Insert New IUS 
                for (i = 0; i <= iSubgroupNId.Length - 1; i++)
                {
                    //-- Get query from DAL 
                    sQry = DBQueries.Calculates.InsertNIdsInIUSTable(iIndicatorNId, iUnitNId, iSubgroupNId[i]);

                    //-- Using DAL for query execution and getting identity value 
                    DBConnection.ExecuteNonQuery(sQry);
                    iIUSNId[i] = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));

                    //*** Create Relationship with IndicatorClassification             
                    string sICGuid;
                    sClassGuid = this.DESheetInformation.ClassGUID.Split("{}".ToCharArray());
                    sICGuid = "'" + this.DESheetInformation.SectorGUID + "'";
                    for (j = 0; j <= sClassGuid.Length - 1; j++)
                    {
                        sICGuid += ",'" + sClassGuid[j].ToString() + "'";
                    }
                    sQry = DBQueries.Calculates.GetICNIdByGId(sICGuid);

                    //-- Get Dataview using DAL           
                    oDV = DBConnection.ExecuteDataTable(sQry).DefaultView;

                    foreach (DataRowView oDVRow1 in oDV)
                    {
                        //-- Using DAL for inserting IC IUS relation 
                        sQry = DBQueries.Calculates.InsertIC_IUSRelation(Convert.ToInt32(oDVRow1["IC_NId"]), iIUSNId[i]);
                        DBConnection.ExecuteNonQuery(sQry);
                    }
                }
            }

            else if (bNewSector || bNewClass)
            {
                for (i = 0; i <= iSubgroupNId.Length - 1; i++)
                {
                    sQry = DBQueries.IUS.GetIUSByI_U_S(iIndicatorNId.ToString(), iUnitNId.ToString(), iSubgroupNId[i].ToString());
                    iIUSNId[i] = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));

                    if (bNewSector)
                    {
                        //-- Using DAL for inserting IC IUS relation 
                        sQry = DBQueries.Calculates.InsertIC_IUSRelation(iSectorNID, iIUSNId[i]);
                        DBConnection.ExecuteNonQuery(sQry);
                    }

                    if (bNewClass)
                    {
                        for (j = 0; j <= iClassNId.Length - 1; j++)
                        {

                            //-- Using DAL for inserting IC IUS relation 
                            sQry = DBQueries.Calculates.InsertIC_IUSRelation(iClassNId[j], iIUSNId[i]);
                            try
                            {
                                DBConnection.ExecuteNonQuery(sQry);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }
            else
            {
                int Index = 0;
                foreach (int SubgroupNId in iSubgroupNId)
                {
                    //-- Using DAL Query for getting IUSNId 
                    sQry = DBQueries.Calculates.GetIUSNIdByIUS(iIndicatorNId, iUnitNId, SubgroupNId);
                    iIUSNId[Index] = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));
                    Index += 1;
                }
            }

            string sTimePeriod = string.Empty;
            int iTimePeriod_NId = 0;
            Hashtable oTimePeriod = new Hashtable();
            string sAreaId = string.Empty;
            int iArea_NId = 0;
            Hashtable oAreaId = new Hashtable();
            string sDataValue = string.Empty;
            System.DateTime dtStartDate = System.DateTime.Now;
            System.DateTime dtEndDate = System.DateTime.Now;
            string sSubgroup = string.Empty;
            string sSource = string.Empty;
            int iSource_NId = 0;
            Hashtable oSource = new Hashtable();
            string sFootNote = "";
            int iFootNote_NId = -1;
            Hashtable oFootNote = new Hashtable();
            string sData_Denominator = "";
            IWorksheet DataSheet;
            DIDatabase DIdatabase = null;

            this.DIExcel = new DIExcel(tempDESheetPath);
            // this.DIExcel = new DIExcel(this.TempLogFilePath);
            DataSheet = DIExcel.GetWorksheet(0);

            {
                //For each record in excel sheet '*** Get TimePeriod_NId, Area_NId, Data Value, Subgroup, SourceNId 
                for (i = 10; i <= miCtrFillTo; i++)
                {
                    try
                    {
                        if (DataSheet.Cells[i, 0].Value.ToString() == "" || DataSheet.Cells[i, 1].Value.ToString() == "" || DataSheet.Cells[i, 3].Value.ToString() == "" || DataSheet.Cells[i, 4].Value.ToString() == "" || DataSheet.Cells[i, 5].Value.ToString() == "")
                        {
                        }
                        //*** If TimePeriod, AreaID, DataValue,Subgroup, Source is blank leave this record 
                        else
                        {
                            //*** TimePeriod 
                            sTimePeriod = DataSheet.Cells[i, 0].Value.ToString();
                            iTimePeriod_NId = -1;
                            if (CheckDate(sTimePeriod))
                            {
                                //*** Check for Valid TimeFormat allowed in DE 
                                if (oTimePeriod.ContainsKey(sTimePeriod))
                                {
                                    iTimePeriod_NId = Convert.ToInt32(oTimePeriod[sTimePeriod]);
                                }
                                else
                                {
                                    sQry = DBQueries.Calculates.GetTimeperiodNIdByTimePeriod(sTimePeriod);
                                    iTimePeriod_NId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));

                                    if (iTimePeriod_NId == 0)
                                    {
                                        // Using DAL for getting and executing Query for inserting timeperiod 
                                        sQry = DI_LibDAL.Queries.Timeperiod.Insert.InsertTimeperiod(sDB_Prefix, sTimePeriod);
                                        DBConnection.ExecuteNonQuery(sQry);
                                        iTimePeriod_NId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                                    }
                                    oTimePeriod.Add(sTimePeriod, iTimePeriod_NId);
                                }
                                SetDate(sTimePeriod, ref dtStartDate, ref dtEndDate);
                            }
                            else
                            {
                                iTimePeriod_NId = -1;
                            }

                            //*** Area 
                            sAreaId = DataSheet.Cells[i, 1].Value.ToString();
                            iArea_NId = -1;
                            if (oAreaId.ContainsKey(sAreaId))
                            {
                                iArea_NId = Convert.ToInt32(oAreaId[sAreaId]);
                            }
                            else
                            {
                                //-- Using DAL for getting AreaNId Using AreaID 
                                sQry = DBQueries.Calculates.GetAreaNIdByAreaID(sAreaId);
                                iArea_NId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));

                                if (iArea_NId == 0)
                                {
                                    oAreaId.Add(sAreaId, -1);
                                }
                                //*** Don't add New areas to database 
                                else
                                {
                                    oAreaId.Add(sAreaId, iArea_NId);
                                }
                            }

                            //*** Data value 
                            sDataValue = DataSheet.Cells[i, 3].Value.ToString();

                            //*** Subgroup 
                            sSubgroup = DataSheet.Cells[i, 4].Value.ToString();


                            //*** Source 
                            sSource = DI_LibBAL.Utility.DICommon.RemoveQuotes(DataSheet.Cells[i, 5].Value.ToString());
                            iSource_NId = -1;
                            if (oSource.ContainsKey(sSource))
                            {
                                iSource_NId = Convert.ToInt32(oSource[sSource]);
                            }
                            else
                            {
                                sQry = DBQueries.IndicatorClassification.GetIC(FilterFieldType.Name, " '" + sSource + "'", ICType.Source, FieldSelection.NId);
                                iSource_NId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));

                                if (iSource_NId == 0)
                                {
                                    string[] sSourceArr;
                                    sSourceArr = sSource.Split('_');
                                    //Publisher_ISOCode_Year: Parent= Publisher; Child= Abbr_Year 
                                    if (sSourceArr.Length >= 2)
                                    {
                                        //*** Insert Parent 
                                        sQry = DBQueries.IndicatorClassification.GetIC(FilterFieldType.Name, " '" + sSourceArr[0] + "'", ICType.Source, FieldSelection.NId);
                                        iSource_NId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));


                                        if (iSource_NId == 0)
                                        {
                                            sQry = DI_LibDAL.Queries.IndicatorClassification.Insert.InsertIC(sDB_Prefix, sLng_Suffix, sSourceArr[0], Guid.NewGuid().ToString(), false, -1, "", ICType.Source);
                                            DBConnection.ExecuteNonQuery(sQry);
                                            iSource_NId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                                        }
                                        //*** Create relationship 
                                        for (j = 0; j <= iIUSNId.Length - 1; j++)
                                        {
                                            sQry = DBQueries.Calculates.InsertIC_IUSRelation(iSource_NId, iIUSNId[j]);
                                            //'sQry = "INSERT INTO " & sDB_Prefix & "Indicator_Classifications_IUS" & " (IC_NId,IUSNId) VALUES (" & iSource_NId & "," & iIUSNId(j) & ")" 
                                            try
                                            {
                                                DBConnection.ExecuteNonQuery(sQry);
                                            }

                                            catch (Exception ex)
                                            {
                                            }
                                        }

                                        //*** Insert Source 
                                        // Using DAL for inserting Source 
                                        sQry = DI_LibDAL.Queries.IndicatorClassification.Insert.InsertIC(sDB_Prefix, sLng_Suffix, sSource.Replace("'", "''"), Guid.NewGuid().ToString(), false, iSource_NId, "", ICType.Source);
                                        DBConnection.ExecuteNonQuery(sQry);
                                        iSource_NId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));

                                        //*** Create relationship 
                                        for (j = 0; j <= iIUSNId.Length - 1; j++)
                                        {
                                            sQry = DBQueries.Calculates.InsertIC_IUSRelation(iSource_NId, iIUSNId[j]);

                                            try
                                            {
                                                DBConnection.ExecuteNonQuery(sQry);
                                            }
                                            //' oDestDB.ExecuteNonQuery(sQry) 
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //iSource_NId = -1 
                                        if (sSource.Trim() != "")
                                        {
                                            //*** Insert Parent as "Global" 
                                            sQry = DBQueries.IndicatorClassification.GetIC(FilterFieldType.Name, "Global", ICType.Source, FieldSelection.NId);
                                            iSource_NId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));

                                            if (iSource_NId == 0)
                                            {
                                                sQry = DI_LibDAL.Queries.IndicatorClassification.Insert.InsertIC(sDB_Prefix, sLng_Suffix, "Global", Guid.NewGuid().ToString(), false, -1, "", ICType.Source);
                                                DBConnection.ExecuteNonQuery(sQry);
                                                iSource_NId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                                            }

                                            //*** Create relationship 
                                            for (j = 0; j <= iIUSNId.Length - 1; j++)
                                            {
                                                sQry = DBQueries.Calculates.InsertIC_IUSRelation(iSource_NId, iIUSNId[j]);
                                                try
                                                {
                                                    DBConnection.ExecuteNonQuery(sQry);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }

                                            //*** Insert Source 
                                            sQry = DI_LibDAL.Queries.IndicatorClassification.Insert.InsertIC(sDB_Prefix, sLng_Suffix, "Global_" + sSource.Replace("'", "''"), Guid.NewGuid().ToString(), false, iSource_NId, "", ICType.Source);
                                            DBConnection.ExecuteNonQuery(sQry);
                                            iSource_NId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));

                                            //*** Create relationship 
                                            for (j = 0; j <= iIUSNId.Length - 1; j++)
                                            {

                                                sQry = DBQueries.Calculates.InsertIC_IUSRelation(iSource_NId, iIUSNId[j]);

                                                try
                                                {
                                                    DBConnection.ExecuteNonQuery(sQry);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //*** If New IUS was created then Create Relationship with Source 
                                    if (bNewIUS)
                                    {
                                        // using DAL query for getting parentNId 
                                        sQry = DBQueries.Calculates.GetICParentNIdByICNId(iSource_NId, "SR");
                                        iParentNId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));

                                        if (iParentNId != 0)
                                        {
                                            for (j = 0; j <= iIUSNId.Length - 1; j++)
                                            {
                                                sQry = DBQueries.Calculates.InsertIC_IUSRelation(iParentNId, iIUSNId[j]);
                                                try
                                                {
                                                    DBConnection.ExecuteNonQuery(sQry);
                                                }

                                                catch (Exception ex)
                                                {
                                                    //*** database maintains unique composite key for IC_NId and IUSNId - this will prevent duplicate entry if any 
                                                }
                                            }
                                        }

                                        for (j = 0; j <= iIUSNId.Length - 1; j++)
                                        {
                                            sQry = DBQueries.Calculates.InsertIC_IUSRelation(iSource_NId, iIUSNId[j]);
                                            try
                                            {
                                                DBConnection.ExecuteNonQuery(sQry);
                                            }

                                            catch (Exception ex)
                                            {
                                                Console.Write(ex.Message);
                                            }
                                        }
                                    }
                                }
                                oSource.Add(sSource, iSource_NId);
                            }

                            try
                            {
                                sFootNote = DataSheet.Cells[i, 6].Value.ToString();
                            }
                            catch (Exception ex)
                            {
                                sFootNote = "";
                            }

                            iFootNote_NId = -1;
                            if (sFootNote != "")
                            {
                                if (oFootNote.ContainsKey(sFootNote))
                                {
                                    iFootNote_NId = Convert.ToInt32(oFootNote[sFootNote]);
                                }
                                else
                                {
                                    sQry = DBQueries.Calculates.GetFootNoteNIdByFootNote(Utility.DICommon.EscapeWildcardChar(Utility.DICommon.RemoveQuotes(sFootNote)));
                                    iFootNote_NId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery(sQry));
                                    if (iFootNote_NId == 0)
                                    {
                                        sQry = DI_LibDAL.Queries.Footnote.Insert.InsertFootnote(sDB_Prefix, sLng_Suffix, sFootNote, Guid.NewGuid().ToString());
                                        DBConnection.ExecuteNonQuery(sQry);
                                        iFootNote_NId = Convert.ToInt32(DBConnection.ExecuteScalarSqlQuery("SELECT @@IDENTITY"));
                                    }
                                    oFootNote.Add(sFootNote, iFootNote_NId);
                                }
                            }

                            try
                            {
                                if (Utility.DICommon.IsNumeric(DataSheet.Cells[i, 7].Value.ToString(), System.Threading.Thread.CurrentThread.CurrentCulture))
                                {
                                    sData_Denominator = DataSheet.Cells[i, 7].Value.ToString() + ",";
                                }
                                else
                                {
                                    sData_Denominator = "";
                                }
                            }
                            catch (Exception ex)
                            {
                                sData_Denominator = "";
                            }

                            if (iIUSNId[0] == -1 | iTimePeriod_NId == -1 | iArea_NId == -1 | iSource_NId == -1 | sDataValue == "")
                            {
                            }
                            else
                            {
                                if (this.ApplicationWizardType == WizardType.HundredMinus)
                                {
                                    sQry = DBQueries.Calculates.InsertDataForCalculate(iIUSNId[Array.IndexOf(this.DESheetInformation.Subgroup, sSubgroup)], iTimePeriod_NId, iArea_NId, sDataValue, "#" + dtStartDate.ToString("MM/dd/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "#", "#" + dtEndDate.ToString("MM/dd/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "#", "", iFootNote_NId, iSource_NId);

                                    DBConnection.ExecuteNonQuery(sQry);

                                }

                                else
                                {
                                    sQry = DBQueries.Calculates.InsertDataForCalculate(iIUSNId[Array.IndexOf(this.DESheetInformation.Subgroup, sSubgroup)], iTimePeriod_NId, iArea_NId, sDataValue, "#" + dtStartDate.ToString("MM/dd/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "#", "#" + dtEndDate.ToString("MM/dd/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "#", sData_Denominator, iFootNote_NId, iSource_NId);

                                    DBConnection.ExecuteNonQuery(sQry);                                    
                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                    }
                }


                // Update Indicator Unit subgroupNId
                DIdatabase = new DIDatabase(DBConnection, DBQueries);
                DIdatabase.UpdateIndicatorUnitSubgroupNIDsInData();
                DBConnection.Dispose();
            }
            //1.Time - 2.AreaId - 3.AreaName - 4.DataValue - 5.Subgroup - 6.Source - 7.Footnote - 8.Denominator - 12.SubgroupNId     
            //DataSheet = null; 


            if ((oDV != null))
            {
                oDV.Dispose();
                oDV = null;
            }
        }

        #endregion

        #endregion

        #region " -- Delegates -- "
        /// <summary>
        /// A delegate to raise UserSelectionChanged event
        /// </summary>
        public delegate void SetProgressBar(int value);

        public delegate void HideProgressBar();
        public delegate void SetCursor(bool IsDefault);
        public delegate void ShowStep4ExcelFile();
        public delegate void ShowStep5ExcelFile(string ParentPanelName);
        public delegate void ShowStep2_3ExcelFile();
        public delegate void ShowStep3_4ExcelFile();
        public delegate void SetStep3IndicatorAndUnitText(String indicator, String unit);
        public delegate void ShowExcelAfterCalculteStep(string ParentPanelName);

        #endregion

        #region " --Events -- "

        public event SetProgressBar SetProgress;

        public event HideProgressBar HideProgBar;

        public event SetCursor SetCursorType;

        public event ShowStep4ExcelFile Step4Completed;
        public event ShowStep5ExcelFile FillDESheetDataCompleted;
        public event ShowStep2_3ExcelFile Step2_3Completed;
        public event ShowStep3_4ExcelFile Step3_4Completed;
        public event SetStep3IndicatorAndUnitText SetStep3IndicatorAndUnit;
        public event ShowExcelAfterCalculteStep CalculteStepCompleted;
        #endregion


    }
}
