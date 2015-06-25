using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport
{
    /// <summary>
    /// Constants Used For Comparison Report Sheet
    /// </summary>
    internal static class Constants
    {

        #region "-- Internal --"

        internal const int HeaderRowIndex = 0;
        internal const int HeaderColIndex = 0;
        internal const int HeaderColValueIndex = 1;
        internal const int DetailsRowIndex = 4;
        internal const int RecordsGapCount = 2;
        internal const int LanguageRowIndex = 2;
        internal const int MissingTextRowIndex = 3;

        /// <summary>
        /// Class containing Summary Report Sheet Customization Information.
        /// </summary>
        internal static class SheetsLayout // SummaryDataCustomizationInfo
        {
            internal const string SheetSerialNo = "S No.";
            internal const string ComparisonReportPrefix = "DB_ComparisonReport_";
            internal const int TotalComparisonReportCount = 16;
            internal const int MAXEXCELROWS = 50000;
            internal const int SheetNameMaxLength = 30;
            internal const int HeaderFontSize = 14;
            //-- Columns Width Settings
            internal const int HeaderNameColWidth = 80;
            internal const int AreaNameColumnWidth = 50;
            internal const string OBLIQUE_DELIMITER = "//";
            internal const string UNDERSCORE_DELIMITER = "_";

        }

        /// <summary>
        /// Constants For Database Schema
        /// </summary>
        internal static class DBSchema
        {
            internal static string DBPassword = "unitednations2000";
            internal static string TABLECOLLECTION = "Tables";
            internal static string TABLETYPE = "TABLE_TYPE";
            internal static string TABLE = "TABLE";
            internal static string TABLENAME = "TABLE_NAME";
        }

        /// <summary>
        /// Constants Used For Language String
        /// </summary>
        internal static class SheetHeader
        {
            internal const string SUMMARY = "SUMMARY";
            internal const string INDICATOR = "INDICATOR";
            internal const string UNIT = "UNIT";
            internal const string SUBGROUP = "SUBGROUP";
            internal const string IUS = "IUS";
            internal const string TIMEPERIOD = "TIMEPERIOD";
            internal const string AREA = "AREA";
            internal const string SECTOR = "SECTOR";
            internal const string CF = "CF";
            internal const string GOAL = "GOAL";
            internal const string INSTITUTION = "INSTITUTION";
            internal const string THEME = "THEME";
            internal const string CONVENTION = "CONVENTION";
            internal const string SOURCE = "SOURCE";
            internal const string DATA = "DATA";
            internal const string DATABASESUMMARYREPORT = "DATABASESUMMARYREPORT";
            internal const string REFERENCE = "REFERENCE";
            internal const string COUNT = "COUNT";
            internal const string TARGET = "TARGET";
            internal const string LANGUAGE = "LANGUAGE";
            internal const string MISSING = "MISSING";
            internal const string ADDITIONAL = "ADDITIONAL";
            internal const string AREAID = "AREAID";
            internal const string AREANAME = "AREANAME";
            internal const string DATAVALUE = "DATAVALUE";
            internal const string SOURCECOMMON = "SOURCECOMMON";
        }


        /// <summary>
        /// Constants OF Temporary Tables Created For Comparison Report.
        /// </summary>
        internal static class TempTables
        {
            internal static string TempTablePrefix = "Temp_";
            internal static string TempIUSTable = "_IUS";
            internal static string TargetDataWithGIDs = "TargetDataWithGIDs";
            internal static string ReferenceData_withGIDs = "ReferenceData_withGIDs";
        }

        /// <summary>
        /// Constants For Different Sheets
        /// </summary>
        internal static class Sheet
        {

            #region "-- Internal --"
            
            /// <summary>
            /// Class Containing Constants for Indicator Sheet
            /// </summary>
            internal static class Indicator
            {
                #region " -- Internal --"

                #region " -- Constants --"

                internal const int DetailsRowIndex = 4;
                internal const int NameColIndex = 1;

                #endregion
                #endregion
            }

            /// <summary>
            /// Unit Sheet Constants
            /// </summary>
            internal static class Unit
            {
                #region " -- Internal --"

                #region " -- Constants --"

                internal const int DetailsRowIndex = 4;
                internal const int NameColIndex = 1;

                #endregion
                #endregion
            }

            /// <summary>
            /// Subgroup Sheet Constants
            /// </summary>
            internal static class Subgroup
            {
                #region " -- Internal --"

                #region " -- Constants --"

                internal const int DetailsRowIndex = 4;
                internal const int NameColIndex = 1;

                #endregion
                #endregion
            }

            /// <summary>
            /// Timeperiod Sheet Constants
            /// </summary>
            internal static class Timeperiods
            {
                #region " -- Internal --"

                #region " -- Constants --"

                internal const int DetailsRowIndex = 4;
                internal const int NameColIndex = 1;

                #endregion
                #endregion
            }

            /// <summary>
            /// IUS Sheet Constants
            /// </summary>
            internal static class IUS
            {
                #region " -- Internal --"

                #region " -- Constants --"

                internal const int DetailsRowIndex = 4;
                internal const int NameColIndex = 1;
                internal const int LastColIndex = 3;

                #endregion

                #endregion
            }

            /// <summary>
            /// Area Sheet Constants
            /// </summary>
            internal static class Area
            {
                #region " -- Internal --"

                #region " -- Constants --"

                internal const int DetailsRowIndex = 4;
                internal const int NameColIndex = 1;

                #endregion

                #endregion
            }

            /// <summary>
            /// IC Sheet Constants
            /// </summary>
            internal static class IC
            {
                #region " -- Internal --"

                #region " -- Constants --"

                internal const int DetailsRowIndex = 4;
                internal const int NameColIndex = 1;

                #endregion

                #endregion
            }

            /// <summary>
            /// IC Sheet Constants
            /// </summary>
            internal static class Data
            {
                #region " -- Internal --"

                #region " -- Constants --"

                internal const int DetailsRowIndex = 4;
                internal const int NameColIndex = 1;
                internal const int LastColIndex = 8;

                #endregion

                #endregion
            }

            #endregion
        }



        #endregion

    }
}
