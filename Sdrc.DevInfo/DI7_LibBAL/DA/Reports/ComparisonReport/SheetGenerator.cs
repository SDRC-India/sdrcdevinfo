using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Drawing;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL.Utility;
using DevInfo.Lib.DI_LibDAL.Queries;
using DevInfo.Lib.DI_LibDAL.Queries.DIColumns;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;

namespace DevInfo.Lib.DI_LibBAL.DA.Reports.ComparisonReport
{
    /// <summary>
    /// Class to generate sheets for Comparison Report
    /// </summary>
    internal class SheetGenerator
    {

        #region "-- private/ Protected --"

        #region "-- Variables --"

        private int SheetCounter = 0;
        private int NameColIndex = 0;
        private int LastColIndex = 0;
        private string LanguageName = string.Empty;

        #endregion

        #region "-- Methods --"

        #region "-- Common Methods --"

        /// <summary>
        /// Set Heading Name of Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        /// <param name="sheetNo">Sheet Index</param>
        private void SetSheetHeading(ref DIExcel excelFile, int sheetNo, string sheetName)
        {
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, sheetName);
            excelFile.GetRangeFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, Constants.MissingTextRowIndex, Constants.HeaderColIndex).Bold = true;

        }

        /// <summary>
        /// Set Language and Its Value
        /// </summary>
        /// <param name="excelFile"></param>
        /// <param name="sheetNo"></param>
        private void SetSheetLanguageValue(ref DIExcel excelFile, int sheetNo)
        {
            excelFile.SetCellValue(sheetNo, Constants.LanguageRowIndex, Constants.HeaderColIndex, DILanguage.GetLanguageString(Constants.SheetHeader.LANGUAGE));

            excelFile.SetCellValue(sheetNo, Constants.LanguageRowIndex, Constants.HeaderColValueIndex, this.LanguageName);

        }

        /// <summary>
        /// Set Missing Text and Fonts
        /// </summary>
        /// <param name="excelFile"></param>
        /// <param name="sheetNo"></param>
        private void SetMissingText(ref DIExcel excelFile, int sheetNo)
        {
            excelFile.SetCellValue(sheetNo, Constants.MissingTextRowIndex, Constants.HeaderColIndex, DILanguage.GetLanguageString(Constants.SheetHeader.MISSING) + " : " + DBNameForMissingRecords);
            // -- Set Headers Font Bold.
            excelFile.GetRangeFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, Constants.MissingTextRowIndex, Constants.HeaderColIndex).Bold = true;
        }

        private string GetValidSheetName(string sheetName, int count)
        {
            string RetVal = string.Empty;

            RetVal = sheetName;
            // -- IF Length of Sheet Name is greater than defined sheet length then Get SheetName for defined length. 
            if (RetVal.Length > Constants.SheetsLayout.SheetNameMaxLength)
            {
                RetVal = RetVal.Substring(0, Constants.SheetsLayout.SheetNameMaxLength - 1);
            }
            if (count > 0)
                RetVal = RetVal + count.ToString();

            return RetVal;
        }

        /// <summary>
        /// Create Sheets 
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        /// <param name="sheetName">WorkSheet Name</param>
        /// <returns>int</returns>
        private int CreateSheet(ref DIExcel excelFile, string sheetName, int count)
        {
            int RetVal = 0;
            string TempName = string.Empty;

            TempName = this.GetValidSheetName(sheetName, count);
            //-- Replace "/" in Sheet Name with "_"
            TempName = TempName.Replace(@"/", "_");
            // -- Create Indicator Sheet.
            excelFile.CreateWorksheet(TempName);
            // -- Get Sheet Index
            RetVal = excelFile.GetSheetIndex(TempName);

            return RetVal;

        }

        /// <summary>
        /// Set Font Style in Reports
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        private void ApplyFontSetting(ref DIExcel excelFile, int sheetNo, int missingRecordsCount)
        {
            int CurrentRowIndex = 0;

            // -- Set Font Of Missing Records
            excelFile.GetRangeFont(sheetNo, Constants.Sheet.Indicator.DetailsRowIndex, Constants.HeaderColIndex, Constants.Sheet.Indicator.DetailsRowIndex, this.LastColIndex).Bold = true;
            CurrentRowIndex = Constants.Sheet.Indicator.DetailsRowIndex + Constants.RecordsGapCount + missingRecordsCount;
            // -- Set Font Of Addtional Records
            excelFile.GetRangeFont(sheetNo, CurrentRowIndex - 1, Constants.HeaderColIndex, CurrentRowIndex + 1, this.LastColIndex).Bold = true;
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.DetailsRowIndex, Constants.Sheet.Indicator.NameColIndex, CurrentRowIndex, this.NameColIndex);
            // -- Wrap Text of Name Column
            excelFile.WrapText(sheetNo, Constants.DetailsRowIndex, this.NameColIndex, CurrentRowIndex, this.NameColIndex, true);

        }

        /// <summary>
        /// Generate Multiple Sheets for having records more than 50,000
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        /// <param name="sheetName">Sheet Name </param>
        /// <param name="missingRecords">Missing Records DataTable</param>
        /// <param name="additionalRecords">Additional Records DataTable</param>
        private void GenerateSheets(ref DIExcel excelFile, string sheetName, DataTable missingRecordsTable, DataTable additionalRecordsTable)
        {
            int CurrentRowIndex = 0;
            int MissingRecordCount = 0;
            int AdditionalRecordCount = 0;
            int SheetNo = 0;
            int FirstRowNo = 0;
            DataTable MissingRecords = null;
            DataTable AdditionalRecords = null;
            DataTable TempTable = null;

            MissingRecords = missingRecordsTable;
            AdditionalRecords = additionalRecordsTable;

            MissingRecordCount = (MissingRecords != null) ? MissingRecords.Rows.Count : 0;
            AdditionalRecordCount = (AdditionalRecords != null) ? AdditionalRecords.Rows.Count : 0;

            if ((MissingRecordCount + AdditionalRecordCount) > 0)
            {
                SheetNo = this.CreateSheet(ref excelFile, sheetName, this.SheetCounter);

                // -- Set Sheet Headers
                this.SetSheetHeading(ref  excelFile, SheetNo, sheetName);
                this.SetSheetLanguageValue(ref  excelFile, SheetNo);
                this.SetMissingText(ref  excelFile, SheetNo);
                this.SheetCounter += 1;
                try
                {
                    // -- Write Missing Records If Its Not Null
                    if (MissingRecords != null)
                    {
                        if (MissingRecords.Rows.Count <= Constants.SheetsLayout.MAXEXCELROWS)
                        {
                            excelFile.LoadDataTableIntoSheet(Constants.Sheet.Indicator.DetailsRowIndex, Constants.HeaderColIndex, MissingRecords, SheetNo, false);
                            MissingRecordCount = MissingRecords.Rows.Count;
                            MissingRecords = null;
                        }
                        else
                        {
                            FirstRowNo = Convert.ToInt32(MissingRecords.Rows[0][DILanguage.GetLanguageString("SERIAL_NUMBER")]);

                            // -- Get Records Less than Maximum Excel sheet Rows
                            DataRow[] Rows = MissingRecords.Select("[" + DILanguage.GetLanguageString("SERIAL_NUMBER") + "]" + " < " + (FirstRowNo + Constants.SheetsLayout.MAXEXCELROWS));
                            TempTable = MissingRecords.Clone();
                            foreach (DataRow Row in Rows)
                            {
                                TempTable.ImportRow(Row);
                            }

                            excelFile.LoadDataTableIntoSheet(Constants.Sheet.Indicator.DetailsRowIndex, Constants.HeaderColIndex, TempTable, SheetNo, false);
                            MissingRecordCount = Constants.SheetsLayout.MAXEXCELROWS;

                            for (int i = 0; i < MissingRecordCount; i++)
                            {
                                MissingRecords.Rows[i].Delete();
                            }
                            MissingRecords.AcceptChanges();
                        }
                    }

                    CurrentRowIndex = Constants.Sheet.Indicator.DetailsRowIndex + Constants.RecordsGapCount + MissingRecordCount;
                    excelFile.SetCellValue(SheetNo, CurrentRowIndex, Constants.HeaderColIndex, DILanguage.GetLanguageString(Constants.SheetHeader.ADDITIONAL) + " : " + DBNameForAdditionalRecords);
                    CurrentRowIndex += 1;
                    // -- Write Additional Records If Its Not Null
                    if (AdditionalRecords != null)
                    {
                        if ((MissingRecordCount + AdditionalRecords.Rows.Count) <= Constants.SheetsLayout.MAXEXCELROWS)
                        {
                           
                            // -- Load Additional Records Into Sheet
                            excelFile.LoadDataTableIntoSheet(CurrentRowIndex, Constants.HeaderColIndex, AdditionalRecords, SheetNo, false);
                            AdditionalRecordCount = 0;
                            //MissingRecordCount = 0;
                            AdditionalRecords = null;
                        }
                        else
                        {
                            int Counter = 0;
                            FirstRowNo = Convert.ToInt32(AdditionalRecords.Rows[0][DILanguage.GetLanguageString("SERIAL_NUMBER")]);
                            // -- Get Records Less than Maximum Excel sheet Rows
                            DataRow[] Rows = AdditionalRecords.Select("[" + DILanguage.GetLanguageString("SERIAL_NUMBER") + "]" + " < " + ((FirstRowNo + Constants.SheetsLayout.MAXEXCELROWS) - MissingRecordCount));

                            TempTable = AdditionalRecords.Clone();
                            foreach (DataRow Row in Rows)
                            {
                                TempTable.ImportRow(Row);
                                Counter += 1;
                            }
                            // -- Load Additional Records Into Sheet
                            excelFile.LoadDataTableIntoSheet(CurrentRowIndex, Constants.HeaderColIndex, TempTable, SheetNo, false);

                            // -- Get Extra Records
                            for (int i = 0; i < Counter; i++)
                            {
                                AdditionalRecords.Rows[i].Delete();
                            }
                            AdditionalRecords.AcceptChanges();
                            AdditionalRecordCount = AdditionalRecords.Rows.Count;
                           
                        }
                    }
                    this.ApplyFontSetting(ref excelFile, SheetNo, MissingRecordCount);

                    MissingRecordCount = (MissingRecords != null) ? MissingRecordCount : 0;
                    AdditionalRecordCount = (AdditionalRecords != null) ? AdditionalRecordCount : 0;

                    if ((MissingRecordCount + AdditionalRecordCount) > 0)
                    {
                        this.GenerateSheets(ref excelFile, sheetName, MissingRecords, AdditionalRecords);
                    }

                    
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region "-- Public / Friend --"

        #region"-- Variables/Properties --"

        internal string DBNameForMissingRecords= string.Empty;
        internal string DBNameForAdditionalRecords = string.Empty;

        #endregion

        #region "-- Methods --"

        /// <summary>
        /// Generate Sheet for Comparison Report
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        /// <param name="dbConnection">Database Connection </param>
        /// <param name="dbQueries">DIQueries</param>
        /// <param name="sheetType">Sheet Type for Comparison Reports</param>
        internal void GenerateSheet(ref DIExcel excelFile, DIConnection dbConnection, DIQueries dbQueries, SheetType sheetType)
        {
            int SheetNo = 0;
            int CurrentRowIndex = 0;
            DataTable MissingRecords = null;
            DataTable AdditionalRecords = null;
            SheetSource SheetGenerator = null;

            // -- Get Sheet Class Instance
            SheetGenerator = SheetSourceFactory.CreateInstance(sheetType, dbConnection, dbQueries);

            this.NameColIndex = SheetGenerator.NameColumnIndex;
            this.LastColIndex = SheetGenerator.LastColumnIndex;
            this.LanguageName = SheetGenerator.GetLanguageName();

            // -- Get Missing Records
            MissingRecords = SheetGenerator.GetMissingRecordsTable();
            //-- Get Additional Records
            AdditionalRecords = SheetGenerator.GetAdditionalRecordsTable();

            // if records is morethan 50000 then create multiple sheets
            if (MissingRecords.Rows.Count > Constants.SheetsLayout.MAXEXCELROWS || (MissingRecords.Rows.Count + AdditionalRecords.Rows.Count) > Constants.SheetsLayout.MAXEXCELROWS)
            {
                // -- Create Multiple Sheet
                this.GenerateSheets(ref excelFile, SheetGenerator.SheetName, MissingRecords, AdditionalRecords);
            }
            else
            {
                // -- Create Worksheet
                SheetNo = this.CreateSheet(ref excelFile, SheetGenerator.SheetName, 0);

                // -- Set Initial Sheet Value
                this.SetSheetHeading(ref  excelFile, SheetNo, SheetGenerator.SheetName);
                this.SetSheetLanguageValue(ref  excelFile, SheetNo);
                this.SetMissingText(ref  excelFile, SheetNo);

                // -- Load Missing Records Into Sheet
                excelFile.LoadDataTableIntoSheet(Constants.Sheet.Indicator.DetailsRowIndex, Constants.HeaderColIndex, MissingRecords, SheetNo, false);
                CurrentRowIndex = Constants.Sheet.Indicator.DetailsRowIndex + Constants.RecordsGapCount + MissingRecords.Rows.Count;
                excelFile.SetCellValue(SheetNo, CurrentRowIndex, Constants.HeaderColIndex, DILanguage.GetLanguageString(Constants.SheetHeader.ADDITIONAL) + " : " + DBNameForAdditionalRecords);
                CurrentRowIndex += 1;
                // -- Load Additional Records Into Sheet
                excelFile.LoadDataTableIntoSheet(CurrentRowIndex, Constants.HeaderColIndex, AdditionalRecords, SheetNo, false);
                // -- Apply Font Settings
                this.ApplyFontSetting(ref excelFile, SheetNo, MissingRecords.Rows.Count);
            }
        }

        #endregion

        #endregion

    }
}
