using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using DevInfo.Lib.DI_LibBAL.Utility;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
    /// <summary>
    /// Base Class For Summary Reports
    /// </summary>
    /// <remarks>Inherited in All sheets of Summary Reports.</remarks>
    internal abstract class SheetGenerator
    {

        #region "-- Protected --"

        #region "-- Method --"

        /// <summary>
        /// Get Temp TAble For IC
        /// </summary>
        /// <returns>DataTable</returns>
        protected DataTable GetTempTable()
        {
            DataTable RetVal = new DataTable();

            DataColumn[] IDCol = new DataColumn[1];
            IDCol[0] = RetVal.Columns.Add(Constants.ICTempTableColumns.ID, Type.GetType("System.Int32"));
            IDCol[0].Unique = true;
            RetVal.PrimaryKey = IDCol;
            RetVal.Columns.Add(Constants.ICTempTableColumns.PARENT_ID, Type.GetType("System.Int32"));
            RetVal.Columns.Add(Constants.ICTempTableColumns.LABEL, Type.GetType("System.String"));
            RetVal.Columns.Add(Constants.ICTempTableColumns.ACT_LABEL, Type.GetType("System.String"));

            return RetVal;
        }

        /// <summary>
        /// Get Name of Indicator Type like Sector, Goal,Source from the Abbreviation
        /// </summary>
        /// <param name="icName">Short Name of Classification Type like 'SC' for Sector</param>
        /// <returns>string</returns>
        protected string GetICTypeName(string icName)
        {
            string RetVal = string.Empty;
            icName = "'" + icName + "'";
            // -- Get Full name of Classification from its Short Name
            if (DIQueries.ICTypeText[ICType.Sector] == icName)
                RetVal = this.ColumnHeader[DSRColumnsHeader.SECTOR];
            else if (DIQueries.ICTypeText[ICType.Goal] == icName)
                RetVal = this.ColumnHeader[DSRColumnsHeader.GOAL];

            else if (DIQueries.ICTypeText[ICType.CF] == icName)
                RetVal = this.ColumnHeader[DSRColumnsHeader.CF];

            else if (DIQueries.ICTypeText[ICType.Theme] == icName)
                RetVal = this.ColumnHeader[DSRColumnsHeader.THEME];

            else if (DIQueries.ICTypeText[ICType.Source] == icName)
                RetVal = this.ColumnHeader[DSRColumnsHeader.SOURCE];

            else if (DIQueries.ICTypeText[ICType.Institution] == icName)
                RetVal = this.ColumnHeader[DSRColumnsHeader.INSTITUTION];

            else if (DIQueries.ICTypeText[ICType.Convention] == icName)
                RetVal = this.ColumnHeader[DSRColumnsHeader.CONVENTION];


            return RetVal;
        }

        /// <summary>
        /// Create Sheet for Supplied Name
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        /// <param name="sheetName">WorkSheet Name</param>
        /// <returns>int</returns>
        protected int CreateSheet(ref DIExcel excelFile, string sheetName)
        {
            int RetVal = 0;
            string TempName = string.Empty;

            TempName = sheetName;
            // -- IF Length of Sheet Name is greater than defined sheet length then Get SheetName for defined length. 
            if (TempName.Length > Constants.SheetsLayout.SheetNameMaxLength)
            {
                TempName = TempName.Substring(0, Constants.SheetsLayout.SheetNameMaxLength);
            }
            TempName = TempName.Replace(@"/","_");
            // -- Create Indicator Sheet.
            excelFile.CreateWorksheet(TempName);
            // -- Get Sheet Index
            RetVal = excelFile.GetSheetIndex(TempName);

            return RetVal;

        }

        /// <summary>
        /// Set S.No into DataTable
        /// </summary>
        /// <param name="table">DataTable</param>
        protected void SetSNoIntoTableColumn(ref DataTable table)
        {
            table.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrement = false;
            int Counter = 0;
            
            foreach (DataRowView row in table.DefaultView)
            {
                Counter += 1;
                row[DILanguage.GetLanguageString("SERIAL_NUMBER")] = Counter;
            }
        }

        /// <summary>
        /// Apply Font Settings
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        /// <param name="sheetNo">Excel Sheet Index</param>
        /// <param name="firstRowIndex">Start Row Index</param>
        /// <param name="lastRowIndex">End Row Index</param>
        /// <param name="firstColIndex">Start Column Index</param>
        /// <param name="lastColIndex">End Column Index</param>
        /// <param name="boldHeader">Set Heading Font Bold</param>
        protected void ApplyFontSettings(ref DIExcel excelFile, int sheetNo, int firstRowIndex, int firstColIndex, int lastRowIndex, int lastColIndex, bool boldHeader)
        {
            excelFile.GetRangeFont(sheetNo, firstRowIndex, firstColIndex, lastRowIndex, lastColIndex).Name = DatabaseSummaryReportGenerator.FontName ;
            excelFile.GetRangeFont(sheetNo, firstRowIndex, firstColIndex, lastRowIndex, lastColIndex).Size = DatabaseSummaryReportGenerator.FontSize ;
            // -- Set First Row Header Bold
            if (boldHeader == true)
            {
                excelFile.GetRangeFont(sheetNo, firstRowIndex, firstColIndex, firstRowIndex, lastColIndex).Bold = true;
                this.SetCellsBorder(ref excelFile, sheetNo, firstRowIndex, firstColIndex, lastColIndex);
            }

        }

        /// <summary>
        /// Set Line Borders of Cells
        /// </summary>
        /// <param name="excelFile">Excel File Oblect</param>
        /// <param name="sheetNo">Sheet No</param>
        /// <param name="startRow">Starting Row Index</param>
        /// <param name="startCol">Starting Column Index</param>
        /// <param name="lastCol">Last Column Index</param>
        protected void SetCellsBorder(ref DIExcel excelFile, int sheetNo, int startRow, int startCol, int lastCol)
        {
            //-- Set Border Line 
            for (int i = startCol; i <= lastCol; i++)
            {
                excelFile.SetCellBorder(sheetNo, startRow, i, SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, Color.Black, SpreadsheetGear.BordersIndex.EdgeTop);
                excelFile.SetCellBorder(sheetNo, startRow, i, SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, Color.Black, SpreadsheetGear.BordersIndex.EdgeBottom);
                excelFile.SetCellBorder(sheetNo, startRow, i, SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, Color.Black, SpreadsheetGear.BordersIndex.EdgeLeft);
                excelFile.SetCellBorder(sheetNo, startRow, i, SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, Color.Black, SpreadsheetGear.BordersIndex.EdgeRight);
                excelFile.SetCellBorder(sheetNo, startRow, i, SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, Color.Black, SpreadsheetGear.BordersIndex.InsideHorizontal);
                excelFile.SetCellBorder(sheetNo, startRow, i, SpreadsheetGear.LineStyle.Continous, SpreadsheetGear.BorderWeight.Thin, Color.Black, SpreadsheetGear.BordersIndex.InsideVertical);
            }

        }

        #endregion
        
        #endregion

        #region "-- Internal --"

        #region "-- Enum --"

        /// <summary>
        /// Enum for Columns Headers of Report Sheet
        /// </summary>
        internal enum DSRColumnsHeader
        {
         SUMMARY_REPORT,
         SUMMARY,
         DATABASENAME,
         DATABASE,
         VERSION,
         TARGET,
         REFERENCE,
         TEMPLATE_LOG,
         DATABASE_LOG,
         TEMPLATE,
         NOTES_NOTES,
         AUTHOR,
         COMMENTS_TYPE,
         FOOTNOTES,
         DATA,
         NUMBER,
         COUNT,
         SUM,
         OF,
         DATAVALUE,
         FILENAME,
         ACTION,
         DATE_TIME,
         USER,
         CREATED_ON,
         INDICATOR,
         UNIT,
         SUBGROUP,
         SUBGROUPTYPE,
         SUBGROUPDIMENSION,
         IUS,
         AREA,
         LEVEL,
         TIMEPERIOD,
         MIN,
         MAX,
         SOURCE,
         LANGUAGE,
         CLASSIFICATION_INDICATOR,
         SECTOR,
         GOAL,
         CF,
         INSTITUTION,
         THEME,
         CONVENTION,
         GLOBAL,
         INFORMATION,
         MISSINGINFORMATION,
         YES,
         NO,
         AREAID,
         AREANAME,
         LEVEL_NAME,
         LAYERNAME,
         STARTDATE,
         ENDDATE


     }

        #endregion

        #region "-- Variables and Properties --"
        
        //-- Used To List Column Headers for Summary Sheets.
        internal Dictionary<DSRColumnsHeader, string> ColumnHeader;
        internal SummarySheetType CurrentSheetType;

        #endregion
        
        #region "-- New/Dispose --"

        /// <summary>
        /// Constructor To Initilize Column Header Collection
        /// </summary>
        public SheetGenerator()
        {
            this.InitColumnHeading();
        }

        #endregion
        
        #region "-- Method --"

        /// <summary>
        /// Abstract Method for Generating Report Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        /// <remarks>Must be Used by all Derived Classes</remarks>
        internal abstract void GenerateSheet(ref DIExcel excelFile);

        /// <summary>
        /// Initilize Column Header String
        /// </summary>
        internal void InitColumnHeading()
        {
            //-- Initilize Collection If Null.
            if (this.ColumnHeader == null)
            {
                this.ColumnHeader = new Dictionary<DSRColumnsHeader, string>();

                this.ColumnHeader.Add(DSRColumnsHeader.MISSINGINFORMATION, DILanguage.GetLanguageString("MISSING") + " " + DILanguage.GetLanguageString("INFORMATION"));
                this.ColumnHeader.Add(DSRColumnsHeader.SUMMARY_REPORT, DILanguage.GetLanguageString("SUMMARY_REPORT"));
                this.ColumnHeader.Add(DSRColumnsHeader.SUMMARY, DILanguage.GetLanguageString("SUMMARY"));
                this.ColumnHeader.Add(DSRColumnsHeader.TEMPLATE_LOG, DILanguage.GetLanguageString("TEMPLATE_LOG"));
                this.ColumnHeader.Add(DSRColumnsHeader.DATABASE_LOG, DILanguage.GetLanguageString("DATABASE_LOG"));
                this.ColumnHeader.Add(DSRColumnsHeader.REFERENCE, DILanguage.GetLanguageString("REFERENCE"));
                this.ColumnHeader.Add(DSRColumnsHeader.TARGET, DILanguage.GetLanguageString("TARGET"));

                this.ColumnHeader.Add(DSRColumnsHeader.CREATED_ON, DILanguage.GetLanguageString("CREATED_ON"));
                this.ColumnHeader.Add(DSRColumnsHeader.DATABASENAME, DILanguage.GetLanguageString("DATABASENAME"));
                this.ColumnHeader.Add(DSRColumnsHeader.DATABASE, DILanguage.GetLanguageString("DATABASE"));
                this.ColumnHeader.Add(DSRColumnsHeader.VERSION, DILanguage.GetLanguageString("VERSION"));
                this.ColumnHeader.Add(DSRColumnsHeader.NUMBER, DILanguage.GetLanguageString("NUMBER"));

                this.ColumnHeader.Add(DSRColumnsHeader.FOOTNOTES, DILanguage.GetLanguageString("FOOTNOTES"));
                //-- For Comments
                this.ColumnHeader.Add(DSRColumnsHeader.NOTES_NOTES, DILanguage.GetLanguageString("NOTES_NOTES"));
                this.ColumnHeader.Add(DSRColumnsHeader.AUTHOR, DILanguage.GetLanguageString("NOTES_EXPERT"));
                this.ColumnHeader.Add(DSRColumnsHeader.COMMENTS_TYPE, DILanguage.GetLanguageString("NOTES_CLASSIFICATION"));

                this.ColumnHeader.Add(DSRColumnsHeader.ACTION, DILanguage.GetLanguageString("ACTION"));
                this.ColumnHeader.Add(DSRColumnsHeader.COUNT, DILanguage.GetLanguageString("COUNT"));
                this.ColumnHeader.Add(DSRColumnsHeader.SUM, DILanguage.GetLanguageString("SUM"));
                this.ColumnHeader.Add(DSRColumnsHeader.OF, DILanguage.GetLanguageString("OF"));
                this.ColumnHeader.Add(DSRColumnsHeader.DATAVALUE, DILanguage.GetLanguageString("DATAVALUE"));

                this.ColumnHeader.Add(DSRColumnsHeader.DATE_TIME, DILanguage.GetLanguageString("DATE_TIME"));
                this.ColumnHeader.Add(DSRColumnsHeader.STARTDATE, DILanguage.GetLanguageString("STARTDATE"));
                this.ColumnHeader.Add(DSRColumnsHeader.ENDDATE, DILanguage.GetLanguageString("ENDDATE"));
                this.ColumnHeader.Add(DSRColumnsHeader.FILENAME, DILanguage.GetLanguageString("FILENAME"));

                this.ColumnHeader.Add(DSRColumnsHeader.INDICATOR, DILanguage.GetLanguageString("INDICATOR"));
                this.ColumnHeader.Add(DSRColumnsHeader.SUBGROUP, DILanguage.GetLanguageString("SUBGROUP"));
                this.ColumnHeader.Add(DSRColumnsHeader.SUBGROUPTYPE, DILanguage.GetLanguageString("SUBGROUP_TYPE"));
                this.ColumnHeader.Add(DSRColumnsHeader.SUBGROUPDIMENSION, DILanguage.GetLanguageString("SUBGROUP_DIMENSION" ));

                this.ColumnHeader.Add(DSRColumnsHeader.UNIT, DILanguage.GetLanguageString("UNIT"));
                this.ColumnHeader.Add(DSRColumnsHeader.TIMEPERIOD, DILanguage.GetLanguageString("TIMEPERIOD"));
                this.ColumnHeader.Add(DSRColumnsHeader.IUS, DILanguage.GetLanguageString("IUS"));
                this.ColumnHeader.Add(DSRColumnsHeader.AREA, DILanguage.GetLanguageString("AREA"));
                this.ColumnHeader.Add(DSRColumnsHeader.AREAID, DILanguage.GetLanguageString("AREAID"));
                this.ColumnHeader.Add(DSRColumnsHeader.AREANAME, DILanguage.GetLanguageString("AREANAME"));
                this.ColumnHeader.Add(DSRColumnsHeader.LANGUAGE, DILanguage.GetLanguageString("LANGUAGE"));
                this.ColumnHeader.Add(DSRColumnsHeader.LAYERNAME, DILanguage.GetLanguageString("LAYERNAME"));
                this.ColumnHeader.Add(DSRColumnsHeader.LEVEL, DILanguage.GetLanguageString("LEVEL"));
                this.ColumnHeader.Add(DSRColumnsHeader.LEVEL_NAME, DILanguage.GetLanguageString("LEVEL_NAME"));
                this.ColumnHeader.Add(DSRColumnsHeader.MAX, DILanguage.GetLanguageString("MAX"));
                this.ColumnHeader.Add(DSRColumnsHeader.MIN, DILanguage.GetLanguageString("MIN"));

                this.ColumnHeader.Add(DSRColumnsHeader.NO, DILanguage.GetLanguageString("NO"));
                this.ColumnHeader.Add(DSRColumnsHeader.YES, DILanguage.GetLanguageString("YES"));

                this.ColumnHeader.Add(DSRColumnsHeader.CLASSIFICATION_INDICATOR, DILanguage.GetLanguageString("CLASSIFICATION_INDICATOR"));
                this.ColumnHeader.Add(DSRColumnsHeader.SECTOR, DILanguage.GetLanguageString("SECTOR"));
                this.ColumnHeader.Add(DSRColumnsHeader.INFORMATION, DILanguage.GetLanguageString("INFORMATION"));
                this.ColumnHeader.Add(DSRColumnsHeader.INSTITUTION, DILanguage.GetLanguageString("INSTITUTION"));
                this.ColumnHeader.Add(DSRColumnsHeader.CF, DILanguage.GetLanguageString("CF"));
                this.ColumnHeader.Add(DSRColumnsHeader.CONVENTION, DILanguage.GetLanguageString("CONVENTION"));
                this.ColumnHeader.Add(DSRColumnsHeader.GLOBAL, DILanguage.GetLanguageString("GLOBAL"));
                this.ColumnHeader.Add(DSRColumnsHeader.GOAL, DILanguage.GetLanguageString("GOAL"));
                this.ColumnHeader.Add(DSRColumnsHeader.SOURCE, DILanguage.GetLanguageString("SOURCE"));
                this.ColumnHeader.Add(DSRColumnsHeader.THEME, DILanguage.GetLanguageString("THEME"));
                this.ColumnHeader.Add(DSRColumnsHeader.USER, DILanguage.GetLanguageString("USER"));
                this.ColumnHeader.Add(DSRColumnsHeader.DATA, DILanguage.GetLanguageString("DATA"));
            }
        }

        #endregion

        #endregion

    }
}
