using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Constants Used In Summary Reports
    /// </summary>
    internal static class Constants
    {
        
        #region " -- Internal --"

            #region " -- Constants --"

            internal const int HeaderRowIndex = 0;
            internal const int HeaderColIndex = 0;
        
            #endregion

    #endregion

        #region "-- Inner Classes --"

        #region "-- Inner Classes : Constants for sheets--"

        /// <summary>
        /// Class Containing Constants for Sheets of Summary Report 
        /// </summary>
        internal static class Sheet
        {

        /// <summary>
        /// Class Containing Constants for SummaryReport Sheet In Summary Report
        /// </summary>
        internal static class SummaryReport
        {
            #region " -- Internal --"

            #region " -- Constants --"

            internal const int SummaryReportSheetIndex = 0;
            internal const int ICLanguageDefaultIndex = 26;

            internal const int ICHeaderCellIndex = 0;
            internal const int ICHeaderColValueIndex = 1;
            internal const int ICHeaderLastColIndex = 9;

            internal const int LanguageColIndex = 0;
            internal const int LanguageColCountValueIndex = 1;
            internal const int LanguageNameColValueIndex = 2;

            internal const int ICTypeColIndex = 0;
            internal const int ICTypeColCountValueIndex = 1;

            internal const int DataBaseNameRowIndex = 3;
            internal const int DataBaseNameColIndex = 0;
            internal const int DataBaseValueColIndex = 1;

            internal const int Created_ONRowIndex = 4;
            internal const int Created_ONColIndex = 0;
            internal const int Created_ONValueColIndex = 1;

            internal const int TemplateRowIndex = 5;
            internal const int TemplateColIndex = 0;
            internal const int TemplateValueColIndex = 1;

            internal const int CountRowIndex = 6;
            internal const int CountColIndex = 0;
            internal const int CountColValueIndex = 1;

            internal const int IndicatorRowIndex = 9;
            internal const int IndicatorColIndex = 0;
            internal const int IndicatorColValueIndex = 1;

            internal const int UnitRowIndex = 10;
            internal const int UnitColIndex = 0;
            internal const int UnitColValueIndex = 1;

            internal const int SubgroupRowIndex = 11;
            internal const int SubgroupTypeRowIndex = 12;
            internal const int SubgroupDimensionRowIndex = 13;
            internal const int SubgroupColIndex = 0;
            internal const int SubgroupColValueIndex = 1;

            internal const int IUSRowIndex = 14;
            internal const int IUSColIndex = 0;
            internal const int IUSColValueIndex = 1;

            internal const int AreaRowIndex = 16;
            internal const int AreaColIndex = 0;
            internal const int AreaColValueIndex = 1;
            internal const int AreaLevelRowValueIndex = 17;
            internal const int AreaLevelCountDataRowIndex = 18;
            internal const int AreaLevelSumDataRowIndex = 19;
            internal const int AreaLevelColIndex = 2;
            internal const int AreaLevelColValueIndex = 3;

            internal const int TimeRowIndex = 21;
            internal const int TimeColIndex = 0;
            internal const int TimeColValueIndex = 1;

            internal const int MaxTimeRowIndex = 22;
            internal const int TimeMinMaxColIndex = 2;
            internal const int TimeMinMaxColValueIndex = 3;

            internal const int SourceRowIndex = 24;
            internal const int SourceColIndex = 0;
            internal const int SourceColValueIndex = 1;

            #endregion

            #endregion

        }

        /// <summary>
        /// Class Containing Constants for Indicator Sheet
        /// </summary>
        internal static class Indicator
        {
            #region " -- Internal --"

            #region " -- Constants --"

            internal const int DetailsRowIndex = 3;
            internal const int NameColIndex = 1;

            #endregion
            #endregion
        }

        /// <summary>
        /// Class Containing Constants for Unit Sheet
        /// </summary>
        internal static class Unit
        {
            #region " -- Internal --"

            #region " -- Constants --"

            internal const int DetailsRowIndex = 3;
            internal const int LastColIndex = 2;

            #endregion
            #endregion
        }

        /// <summary>
        /// Class Containing Constants for Subgroup Sheet
        /// </summary>
        internal static class Subgroup
        {
            #region " -- Internal --"

            #region " -- Constants --"


            internal const int SubGroupNameColIndex = 1;
            internal const int SubGroupDetailsRowIndex = 3;
            internal const int SubGroupLastColIndex = 2;
            #endregion
            #endregion
        }

        /// <summary>
        /// Class Containing Constants for IUS Sheet
        /// </summary>
        internal static class IUS
        {
            #region " -- Internal --"

            #region " -- Constants --"


            internal const int IUSNameColIndex = 1;
            internal const int IUSDetailsRowIndex = 3;
            internal const int IUSCountColINdex = 4;
            internal const int IUSLastColIndex = 5;

            #endregion
            #endregion
        }

        /// <summary>
        /// Class Containing Constants for Timeperiod Sheet
        /// </summary>
        internal static class Timeperiod
        {
            #region " -- Internal --"

            #region " -- Constants --"


            internal const int TimePeriodDetailsRowIndex = 3;
            internal const int TimePeriodLastColIndex = 1;
            internal const int TimePeriodColValueIndex = 1;

            #endregion
            #endregion
        }
        
        /// <summary>
        ///Class Containing Constants for Area Sheet 
        /// </summary>
        internal static class Area
        {
            #region " -- Internal --"

            #region " -- Constants --"


            internal const int AreaIDColIndex = 1;
            internal const int AreaNameColIndex = 2;
            internal const int AreaLevelNameColIndex = 4;
            internal const int AreaDetailsRowIndex = 3;
            internal const int AreaStartDateColIndex = 7;
            internal const int AreaLastColIndex = 10;
            internal const int AreaWithoutDataLastColIndex = 3;

            #endregion
            #endregion
        }

        /// <summary>
        /// Class Containing Constants for IC Sheet
        /// </summary>
        internal static class IC
        {
            #region " -- Internal --"

            #region " -- Constants --"


            internal const int ICHeaderRowIndex = 0;
            internal const int ICHeaderColIndex = 0;
            internal const int ICSetorIndicatorNameColIndex = 3;
            internal const int ICIndicatorNameColIndex = 2;
            internal const int ICLevelColIndex = 1;
            internal const int ICDetailsRowIndex = 3;
            internal const int ICDetailsRowCount = 10;
            internal const int ICLastColIndex = 5;

            #endregion
            #endregion
        }

        /// <summary>
        /// Class Containing Constants for IC(Missing IUS) Sheet
        /// </summary>
        internal static class ICMissingIUS
        {
            #region " -- Internal --"

            #region " -- Constants --"

            //#region "-- Classification Not Used For IUS --"

            internal const int UnmatchedIUSICColIndex = 1;
            internal const int UnmatchedIUSICDetailsRowIndex = 3;
            internal const int UnmatchedIUSICLastColIndex = 1;

            #endregion
            #endregion
        }

        /// <summary>
        /// Class Containing Constants for IUS Linked To IC Sheet
        /// </summary>
        internal static class IUSLinkedTOIC
        {
            #region " -- Internal --"

            #region " -- Constants --"

            internal const int IUSLinkedNameColIndex = 1;
            internal const int IUSLinkedDetailsRowIndex = 3;
            internal const int IUSLinkedLastColIndex = 3;

            #endregion
            #endregion
        }

        /// <summary>
        /// Class Containing Constants for DuplicateData Sheet
        /// </summary>
        internal static class DuplicateData
        {
            #region " -- Internal --"

            #region " -- Constants --"

            internal const int DuplicateDataDetailsRowIndex = 3;
            internal const int DuplicateDataNameColIndex = 3;
            internal const int DuplicateDataOthersColIndex = 4;
            internal const int DuplicateDataLastColIndex = 8;

            #endregion
            #endregion

        }

        /// <summary>
        /// Class Containing Constants for Footnote Sheet
        /// </summary>
        internal static class Footnotes
        {
            #region " -- Internal --"

            #region " -- Constants --"


            internal const int FootNotesHeaderRowIndex = 0;
            internal const int FootNotesHeaderColIndex = 0;
            internal const int FootNotesNameColIndex = 1;
            internal const int FootNotesOthersColIndex = 2;
            internal const int FootNotesDetailsRowIndex = 3;
            internal const int FootNotesLastColIndex = 8;

            #endregion
            #endregion
        }

        /// <summary>
        /// Class Containing Constants for Comments Sheet
        /// </summary>
        internal static class Comments
        {
            #region " -- Internal --"

            #region " -- Constants --"


            internal const int CommentsNameColIndex = 1;
            internal const int CommentsValueColIndex = 8;
            internal const int CommentsOthersColIndex = 2;
            internal const int CommentsDetailsRowIndex = 3;
            internal const int CommentsLastColIndex = 10;

            #endregion
            #endregion
        }

        /// <summary>
        /// Class Containing Constants for Database Log Sheet
        /// </summary>
        internal static class DBLog
        {
            #region " -- Internal --"

            #region " -- Constants --"


            internal const int DataBaseLogNameColIndex = 1;
            internal const int DataBaseLogDetailsRowIndex = 3;
            internal const int DataBaseLogLastColIndex = 4;

            #endregion
            #endregion
        }

        /// <summary>
        /// Class Containing Constants for Template Log Sheet
        /// </summary>
        internal static class TemplateLog
        {
            #region " -- Internal --"

            #region " -- Constants --"


            internal const int TemplateLogNameColIndex = 1;
            internal const int TemplateLogDetailsRowIndex = 3;
            internal const int TemplateLogLastColIndex = 4;

            #endregion

            #endregion

        }

        /// <summary>
        /// Class Containing Constants for DBVersion Sheet
        /// </summary>
        internal static class DBVersion
        {
            #region " -- Internal --"

            #region " -- Constants --"

            internal const int DetailsRowIndex = 3;
            internal const int LastColIndex = 3;
            internal const int ColValueIndex = 1;

            #endregion

            #endregion
        }
        

        }

        #endregion

        /// <summary>
        /// Class containing Summary Report Sheet Customization Information.
        /// </summary>
        internal static class   SheetsLayout // SummaryDataCustomizationInfo
        {
            internal const int TotalSummaryReportCount = 26;
            internal const string SheetSerialNo = "S No.";
            internal const int SNoColumnWidth = 5;
            internal const int SheetNameMaxLength = 31;
            internal const int HeaderFontSize = 14;
            //-- Columns Width Settings
            internal const int HeaderNameColWidth = 80;
            internal const int TimePeriodColWidth = 30;
            internal const int OthersColumnWidth = 20;
            internal const int ICLavelColWidth = 20;
            internal const int ICNameColWidth = 40;
            internal const int AreaNameColumnWidth = 50;

            internal const string OBLIQUE_DELIMITER = "//";
            internal const string UNDERSCORE_DELIMITER = "_";
        }
       
        /// <summary>
        /// Constants OF Different Sheet Names not presents in Language File
        /// </summary>
        internal static class SheetsNames
        {
            // -- List of Constant for Summary Report WorkSheet
            internal const string IUSLinkedTOClasses = "IUS Linked TO Classifications";
            internal const string IUSNotLinkedTOClasses = "IUS Missing Classification";
            internal const string UnmatchedIUSClassifications = "Classification Missing IUS";
            internal const string IUSWithoutData = "IUS Without Data";
            internal const string AreasWithoutData = "Areas Without Data";
            internal const string SourcesWithoutData = "Sources Without Data";
            internal const string TimeperiodsWithoutData = "TimePeriods Without Data";
            internal const string DuplicateData = "Duplicate Datavalues";
            internal const string Dimension = "Dimensions";
        }

        /// <summary>
        /// Constants Used For IC DataTable Column Name
        /// </summary>
        internal static class ICTempTableColumns
        {
            //-- Constants Defined For Temporary DataTable
            internal const string ID = "ID";
            internal const string PARENT_ID = "Parent_ID";
            internal const string LABEL = "LABEL";
            internal const string ACT_LABEL = "ACT_LABEL";

            internal const string PARENTID_VALUE = "-99";
            internal const string ICLEVEL_SEPERATOR = "{[~-~]}";
        }
        #endregion
    }
}
