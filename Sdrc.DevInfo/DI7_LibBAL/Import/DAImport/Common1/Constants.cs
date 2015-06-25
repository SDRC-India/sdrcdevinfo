using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Import.DAImport.Common
{
    /// <summary>
    /// This is Static Class used to store Hardcoded string. 
    /// </summary>
    internal static class Constants
    {
        internal const string PleaseWaitString = "Please wait...";
        internal const string DBPassword = "unitednations2000";
        internal const string TempSheetTableName = "TempSheetTable";
        internal const string TempDataTableName = "TempDataTable";
        internal const string TempCSVTableName = "TempCSVTable";

        internal const string TempNotesDataTableName = "lnk_Notes_Data";
        internal const string TempNotesProfileTableName = "lnk_Notes_profile";

        internal const string DuplicateTempTable = "DuplicateTemp";

        internal const string TempBlankIUSTable = "TempBlankIUS";
        internal const string TempUnmatchedIUSTable = "TempUnmatchedIUS";

        internal const string CSVDelimiter = "\",\"";   //  separator used: double quotes with comma (",")
        internal const string COMMADelimiter = ",";

        //internal const char CSV_IUSGId_Delimiter = '-';
        internal const char CSV_IUSGId_Delimiter = '|';
        internal const int FIXED_CSV_COLUMNS = 5;
        internal const string CSV_BLANK_DATA_VALUE_SYMBOL = "-";

        //For source 
        internal const int PublisherMaxLength = 100; 

        #region " -- Added Temp columns --"

        internal const string TempColumnName = "TempCol";

        // Column name for IUS NId in Temp_Data Table. It represnts NID for unmatched I,U,S records
        internal const string NewIUSColumnName = "New_IUSNid";

        // Column name for Source in Temp_Data Table.
        internal const string SourceColumnName = "Source";

        // Column name for recommended source in Temp_Data Table.
        internal const string RecommendedSourceColumnName = "RecommendedSource";

        //ColumnName FootNote in TempDatTAble
        internal const string FootNoteColumnName = "Footnote";

        internal const string DecimalColumnName = "Decimals";

        internal const string Old_Data_Nid = "Old_Data_Nid";

        internal const string Old_Source_Nid = "OLD_IC_NId";
        internal const string SkippedIUSColumnName = "SkippedIUS";
        internal const string SkippedIUSReason = "Reason";
        internal const string invalidindicator = "Invalid Indicator GID";
        internal const string invalidunit = "Invalid Unit GID";
        internal const string invalidsubgroupval = "Invalid SubGroup Val GID";
        # endregion

        /// <summary>
        /// This class have columnName used in XmlMappedLog file.
        /// ColumnName for each Unmatched and Mapped elements. 
        /// </summary>
        internal static class Log
        {
            internal const string MapIndicatorColumnName = "Map_Indicator_Name";
            internal const string UnmatchedIndicatorColumnName = "Indicator_Name";

            internal const string MapUnitColumnName = "Map_Unit_Name";
            internal const string UnmatchedUnitColumnName = "Unit_Name";

            internal const string MapSubgroupValColumnName = "Map_Subgroup_Val";
            internal const string UnmatchedSubgroupValColumnName = "Subgroup_Val";

            internal const string MapAreaColumnName = "Map_Area_Name";
            internal const string UnmatchedAreaColumnName = "Area_Name";

            internal const string DuplicateIndicatorColumnName= "Indicator_Name";
            internal const string DuplicateUnitColumnName="Unit_Name";
            internal const string DuplicateSubgroupValColumnName="Subgroup_Val";
            internal const string DuplicateTimeperiodColumnName="TimePeriod";
            internal const string DuplicateAreaIDColumnName="Area_ID";
            internal const string DuplicateSourceColumnName="Source";

            internal const string SkippedIndicatorColumnName = "Indicator_name";
            internal const string SkippedUnitColumnName = "Unit_Name";
            internal const string SkippedSubgroupValColumnName = "Subgroup_Val";
            internal const string SkippedTimePeriodColumnName = "TimePeriod";
            internal const string SkippedSourceFileColumnName = "SourceFile";
            internal const string SkippedIndicatorGid = "";
            internal const string SkippedUnitGid = "";
            internal const string SkippedSubGroupValGid = "";
            internal const string CSVDataRowNoColumnName = "Row_No";


            // columns for Skipped files
            internal const string SkippedSheetName = "SheetName";
            internal const string SkippedFileReason = "SkippedReason";
                  
        }
    }
}
