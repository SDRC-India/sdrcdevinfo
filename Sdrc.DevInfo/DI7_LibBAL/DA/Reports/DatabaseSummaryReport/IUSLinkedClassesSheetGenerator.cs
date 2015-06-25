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
    /// Used To Generate IUS Linked To Classifications Sheet of Summary Report
    /// </summary>
    internal class IUSLinkedClassesSheetGenerator: SheetGenerator
    {

        #region "-- private --"

        #region "-- Methods --"

        /// <summary>
        /// Get IUS DataTable
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable GetIUSLinkedTable()
        {
            DataTable RetVal = this.CreateIUSLinkedTable();
            // -- Get IUS Linked Data Table
            string Query = IUSLinkedToClassificationQuery.IUSLinkedTOMultipleIC();
            DataTable Table = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(Query);

            Query = IUSLinkedToClassificationQuery.IUSLinkedTOMultipleICType();
            DataTable TempTable = DatabaseSummaryReportGenerator.DBConnection.ExecuteDataTable(Query);
            Table.Merge(TempTable);

            // -- Get DataView of Distinct Record
            DataView ICTable = Table.DefaultView.ToTable(true, Indicator.IndicatorName, Unit.UnitName, SubgroupVals.SubgroupVal).DefaultView ;
            ICTable.Sort = Indicator.IndicatorName + " Asc," + Unit.UnitName + " Asc," + SubgroupVals.SubgroupVal + " Asc";

            foreach (DataRowView RowView in ICTable)
            {
                RetVal.ImportRow(RowView.Row);
            }
            // -- Rename Columns
            this.RenameIUSLinkedTable(ref RetVal);

            return RetVal;
        }

        /// <summary>
        ///Create Linked IUS to Classification DataTable 
        /// </summary>
        /// <returns>DataTable</returns>
        private DataTable CreateIUSLinkedTable()
        {
            DataTable RetVal = new DataTable();

            RetVal.Columns.Add(DILanguage.GetLanguageString("SERIAL_NUMBER"));
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrement = true;
            RetVal.Columns[DILanguage.GetLanguageString("SERIAL_NUMBER")].AutoIncrementSeed = 1;
            RetVal.Columns.Add(Indicator.IndicatorName);
            RetVal.Columns.Add(Unit.UnitName);
            RetVal.Columns.Add(SubgroupVals.SubgroupVal);

            return RetVal;
        }

        /// <summary>
        /// Rename Column of IUS Linked Table
        /// </summary>
        /// <param name="table">Data Table OF IUSLinkedToIC</param>
        private void RenameIUSLinkedTable(ref DataTable table)
        {
            table.Columns[Indicator.IndicatorName].ColumnName = base.ColumnHeader[DSRColumnsHeader.INDICATOR];
            table.Columns[Unit.UnitName].ColumnName = base.ColumnHeader[DSRColumnsHeader.UNIT];
            table.Columns[SubgroupVals.SubgroupVal].ColumnName = base.ColumnHeader[DSRColumnsHeader.SUBGROUP];
        }

        #endregion

        #endregion

        #region "-- Internal --"

        #region "-- Methods --"

        /// <summary>
        /// Create IUS Linked To Multiple Classifications Sheet
        /// </summary>
        /// <param name="excelFile">Excel File</param>
        internal override void GenerateSheet(ref DIExcel excelFile)
        {
            DataTable Table = null;
            int sheetNo = base.CreateSheet(ref excelFile, DILanguage.GetLanguageString("IUS_LINKED_TO_CLASSIFICATIONS"));
            
            // -- sheet content 
            excelFile.SetCellValue(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex, DILanguage.GetLanguageString("IUS_LINKED_TO_CLASSIFICATIONS"));
            excelFile.GetCellFont(sheetNo, Constants.HeaderRowIndex, Constants.HeaderColIndex).Size = Constants.SheetsLayout.HeaderFontSize;

            // -- Get IUSLinked Data TAble.
            Table = this.GetIUSLinkedTable();
            excelFile.LoadDataTableIntoSheet(Constants.Sheet.IUSLinkedTOIC.IUSLinkedDetailsRowIndex, Constants.HeaderColIndex, Table, sheetNo, false);

            int LastRow = Constants.Sheet.IUSLinkedTOIC.IUSLinkedDetailsRowIndex + Table.Rows.Count;

            // -- Apply Font Settings
            this.ApplyFontSettings(ref excelFile, sheetNo, Constants.Sheet.IUSLinkedTOIC.IUSLinkedDetailsRowIndex, Constants.HeaderColIndex, LastRow, Constants.Sheet.IUSLinkedTOIC.IUSLinkedLastColIndex, true);

            // -- Set Column Width
            excelFile.SetColumnWidth(sheetNo, Constants.SheetsLayout.HeaderNameColWidth, Constants.Sheet.IUSLinkedTOIC.IUSLinkedNameColIndex, Constants.Sheet.IUSLinkedTOIC.IUSLinkedNameColIndex, LastRow, Constants.Sheet.IUSLinkedTOIC.IUSLinkedNameColIndex);
            // -- autofit Map 
            excelFile.AutoFitColumns(sheetNo, Constants.Sheet.IUSLinkedTOIC.IUSLinkedDetailsRowIndex, Constants.Sheet.IUSLinkedTOIC.IUSLinkedLastColIndex, LastRow, Constants.Sheet.IUSLinkedTOIC.IUSLinkedLastColIndex);
            // -- Wrap Text of Indicator Column
            excelFile.WrapText(sheetNo, Constants.HeaderRowIndex, Constants.Sheet.IUSLinkedTOIC.IUSLinkedNameColIndex, LastRow, Constants.Sheet.IUSLinkedTOIC.IUSLinkedNameColIndex, true);
          
  
        }

        #endregion

        #endregion

        
    }
}



namespace DevInfo.Lib.DI_LibBAL.DA.Reports.DatabaseSummaryReport
{
internal class IUSLinkedToClassificationQuery
{

    internal static string IUSLinkedTOMultipleICType()
    {
        string RetVal = string.Empty;

        RetVal = "SELECT I." + Indicator.IndicatorName +", S." + SubgroupVals.SubgroupVal +", U." + Unit.UnitName 
                + " FROM " + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications +" AS IC," 
                + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassificationsIUS +" AS ICIUS,"
                + DatabaseSummaryReportGenerator.DBQueries.TablesName .Unit + " AS U,"
                + DatabaseSummaryReportGenerator.DBQueries.TablesName.SubgroupVals + " AS S,"
                + DatabaseSummaryReportGenerator.DBQueries.TablesName.Indicator + " AS I, "
                + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorUnitSubgroup + " AS IUS,"
                + " ( SELECT IUS." + Indicator_Unit_Subgroup.IUSNId +", IC."+ IndicatorClassifications.ICType 
                +" FROM "+ DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications + " AS IC,"
                + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassificationsIUS +" AS ICIUS," 
                + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorUnitSubgroup +" AS IUS " 
                + " WHERE IC." +IndicatorClassifications.ICNId + "= ICIUS."+ IndicatorClassificationsIUS.ICNId 
                + " AND IUS."+ Indicator_Unit_Subgroup.IUSNId +"=ICIUS."+ IndicatorClassificationsIUS.IUSNId 
                + " AND IC." + IndicatorClassifications.ICType + " <> " + DIQueries.ICTypeText[ICType.Source]
                + ")  AS T WHERE IC."+ IndicatorClassifications.ICNId + "= ICIUS." + IndicatorClassificationsIUS.ICNId 
                + " AND IUS." + Indicator_Unit_Subgroup.IUSNId + "= ICIUS." +IndicatorClassificationsIUS.IUSNId 
                + " AND IC."+ IndicatorClassifications.ICType + "<>"+ DIQueries.ICTypeText[ICType.Source]
                + " AND I."+ Indicator.IndicatorNId + "=IUS."+ Indicator_Unit_Subgroup.IndicatorNId + " AND S."
                + SubgroupVals.SubgroupValNId + " = IUS."+ Indicator_Unit_Subgroup.SubgroupValNId + " AND U." + Unit.UnitNId 
                + " =IUS."+ Indicator_Unit_Subgroup.UnitNId + " AND T." + Indicator_Unit_Subgroup.IUSNId + "= IUS."
                + Indicator_Unit_Subgroup.IUSNId + " AND IC."+ IndicatorClassifications.ICType + " <>T."+ IndicatorClassifications.ICType 
                + " GROUP BY IUS."+ Indicator_Unit_Subgroup.IUSNId+", IC."+ IndicatorClassifications.ICType 
                + ", I."+ Indicator.IndicatorName + ", S."+ SubgroupVals.SubgroupVal + ",U."+ Unit.UnitName ;

        return RetVal;
    
    }

    internal static string IUSLinkedTOMultipleIC()
    {
        string RetVal = string.Empty;

        RetVal = "SELECT DISTINCT I." + Indicator.IndicatorName +", U." + Unit.UnitName + ", SGV." + SubgroupVals.SubgroupVal 
            + " FROM  " + DatabaseSummaryReportGenerator.DBQueries.TablesName.SubgroupVals + " AS SGV, " 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName.Indicator + " AS I, " 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName .Unit + " AS U, " 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorUnitSubgroup + " AS IUS, " 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassificationsIUS +" AS ICIUS," 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications +" AS IC, (SELECT  * FROM  " 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications 
            +" AS IC   WHERE NOT  EXISTS ( SELECT * FROM " 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications + " AS IC1  Where  IC." 
            + IndicatorClassifications.ICNId +"=IC1." +IndicatorClassifications.ICParent_NId + " AND IC1." 
            +IndicatorClassifications.ICParent_NId + " >0)) AS TEMP,(SELECT DISTINCT I." 
            + Indicator.IndicatorName +", U." + Unit.UnitName + ", SGV." + SubgroupVals.SubgroupVal + ", IC." 
            + IndicatorClassifications.ICName +" AS ParentIC, TEMP." +IndicatorClassifications.ICParent_NId + ", TEMP." 
            + IndicatorClassifications.ICName +" As ChildIC, IUS."+ Indicator_Unit_Subgroup.IUSNId + " FROM  " 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorUnitSubgroup + " AS IUS, " 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName.Indicator + " AS I, " 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName .Unit + " AS U, " 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName.SubgroupVals + " AS SGV, " 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassificationsIUS +" AS ICIUS," 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications + " AS IC, (SELECT  * FROM  " 
            + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications 
            + " AS IC WHERE NOT EXISTS ( SELECT * FROM " + DatabaseSummaryReportGenerator.DBQueries.TablesName.IndicatorClassifications 
            + " AS IC1  Where  IC." + IndicatorClassifications.ICNId + " = IC1." +IndicatorClassifications.ICParent_NId 
            + " AND IC1." +IndicatorClassifications.ICParent_NId + " >0)) AS TEMP WHERE  1=1 AND  IUS."
            + Indicator_Unit_Subgroup.IndicatorNId +"=I." + Indicator.IndicatorNId + " AND IUS." + Indicator_Unit_Subgroup.UnitNId 
            + " = U."+ Unit.UnitNId + " AND IUS."+ Indicator_Unit_Subgroup.SubgroupValNId +"= SGV." + SubgroupVals.SubgroupValNId 
            + " AND IUS."+ Indicator_Unit_Subgroup.IUSNId +"= ICIUS."+ IndicatorClassificationsIUS.IUSNId 
            + " AND ICIUS." + IndicatorClassifications.ICNId + " = IC." + IndicatorClassifications.ICNId + " AND TEMP." 
            + IndicatorClassifications.ICParent_NId + " =IC."+ IndicatorClassifications.ICNId 
            + " GROUP BY IUS."+ Indicator_Unit_Subgroup.IUSNId + ", I." + Indicator.IndicatorName +", U." + Unit.UnitName 
            + ", SGV." + SubgroupVals.SubgroupVal +", IC."+ IndicatorClassifications.ICType + ", IC." 
            + IndicatorClassifications.ICName + " , TEMP." + IndicatorClassifications.ICNId + ", TEMP." 
            +IndicatorClassifications.ICParent_NId + ", TEMP." + IndicatorClassifications.ICName +" HAVING (((IC."
            + IndicatorClassifications.ICType+ ")<>" 
            + DIQueries.ICTypeText[ICType.Source]+ "))) as Table1 WHERE  1=1 AND  IUS."+ Indicator_Unit_Subgroup.IndicatorNId
            + "=I." + Indicator.IndicatorNId + " AND IUS." + Indicator_Unit_Subgroup.UnitNId + "= U." + Unit.UnitNId 
            + " AND IUS." + Indicator_Unit_Subgroup.SubgroupValNId +" = SGV."+ SubgroupVals.SubgroupValNId 
            + " AND IUS."+Indicator_Unit_Subgroup.IUSNId + "= ICIUS."+ IndicatorClassificationsIUS.IUSNId + " AND ICIUS." 
            + IndicatorClassifications.ICNId + " = IC." + IndicatorClassifications.ICNId + " and Table1.ParentIC <> IC." 
            + IndicatorClassifications.ICName + " AND  IUS."+ Indicator_Unit_Subgroup.IUSNId + "= Table1."
            + Indicator_Unit_Subgroup.IUSNId + " AND TEMP." + IndicatorClassifications.ICParent_NId 
            + " =IC." + IndicatorClassifications.ICNId + " GROUP BY IUS." + Indicator_Unit_Subgroup.IUSNId 
            + ", I." + Indicator.IndicatorName + ", U." + Unit.UnitName + ", SGV." + SubgroupVals.SubgroupVal + ", IC."
            + IndicatorClassifications.ICType + ", IC." + IndicatorClassifications.ICName + ", TEMP." 
            + IndicatorClassifications.ICNId + ", TEMP." + IndicatorClassifications.ICParent_NId + ", TEMP." 
            + IndicatorClassifications.ICName + " HAVING (((IC."+ IndicatorClassifications.ICType + ")<>'SR'))";

        return RetVal;

    }

}

}





