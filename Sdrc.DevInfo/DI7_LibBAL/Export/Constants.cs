using System;
using System.Collections.Generic;
using System.Text;

namespace DevInfo.Lib.DI_LibBAL.Export
{
    public static class Constants
    {

        #region "-- Public --"
          // Sector & class column names to set in DataView passed for DES.
        public const string SectorColumnName = "Sector";
        public const string SectorNid = "SectorNid";
        public const string ClassColumnName = "Class";
        public const string ParentAreaIDColumnCaption = "Parent AreaID";
        public const string ParentAreaIDColumn = "ParentAreaId";
      
        #endregion

        #region "-- Internal --"
        internal const string DESWorkbookNamePrefix = "Data";

        internal static class DESLanguageKeys
        {
            internal const string DESHeading = "DEVINFO_DATA_ENTRY_SPREADSHEET";
            internal const string Indicator = "INDICATOR";
            internal const string Unit = "UNIT";
            internal const string Sector = "SECTOR";
            internal const string Class = "CLASS";
            internal const string TimePeriod = "TIME";
            internal const string AreaID = "AREAID";
            internal const string AreaName = "AREANAME";
            internal const string DataValue = "DATAVALUE";
            internal const string Subgroup = "SUBGROUP";
            internal const string Source = "SOURCE";
            internal const string Footnotes = "FOOTNOTES";
            internal const string Denominator = "DENOMINATOR";
            internal const string Decimals = "DECIMALPLACES";

        }

        internal static class CellAddress
        {
            //Cell address for Indicator Entry Spreadsheet, Area Entry Spreadsheet.
            internal const string IndicatorWorksheetStartAddress = "A6";
            internal const string AreaWorksheetStartAddress = "A6";
           
        }

        internal static class MICSCompilerDESCells
        {
            internal const string IndexSheetAreaIDCell = "B2";
            internal const string IndexSheetAreaNameCell = "B3";
            internal const string IndexSheetSubgroupListCell = "G3";
            internal const string IndexSheetTimePeriodListCell = "H3";

            internal const int IndexSheetListStartingRowIndex = 5;
            internal const int IndexSheetTopicColumnIndex = 0;
            internal const int IndexSheetSheetNoColumnIndex = 1;
            internal const int IndexSheetIndicatorColumnIndex = 2;
            internal const int IndexSheetUnitColumnIndex = 3;

        }

        /// <summary>
        /// Constants for Export worksheet "Indicator and Dimension - GUIDs"
        /// </summary>
        internal class IUSSheet
        {            
            internal const string SheetTitle = "CensusInfo Registry";
            internal const string SheetName = "Indicator and Dimension - GUIDs";
            internal const string SheetFontName = "Arial";
            internal const string SNo = "S.No.";
            internal const string GUIDColumn = "GUID";
            internal const string Indicator = "Indicator";
            internal const string Unit = "Unit";
            internal const string Subgroup = "Subgroup";
            internal const string SubgroupDimensionsColumn = "Subgroup Dimensions";
            internal const string GIdSeprator = "-";

            internal const int FirstColumnIndex = 0;
            internal const int TitleRowIndex = 0;
            internal const int NameRowIndex = 1;
            internal const int TableStartRowIndex = 4;
            internal const int IndicatorColIndex = 2;
            internal const int UnitColIndex = 3;
            internal const int SgValColIndex = 4;
            internal const int DimensionStartColIndex = 5;
            internal const int GuidColIndex = 1;
            internal const int DimensionHeaderRowIndex = 3;

            internal const double SnoColumnWidth = 5;
            internal const double GUIDColumnWidth = 35;
            internal const double IndicatorColumnWidth = 13;
            internal const double UnitColumnWidth = 9;
            internal const double SubgroupColumnWidth = 17;
            internal const double DimensionsColumnWidth = 9;

        }

        #endregion

    }
}
