using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

/// <summary>
/// Summary description for ExportMapping
/// </summary>
public class ExportMapping
{
    #region "-- Private --"

    #region "-- Variables / Properties --"

    private string SourceName = string.Empty;
    private string TargetName = string.Empty;
    private string IUSDropDownRowCounts = string.Empty;

    #endregion

    #region "-- Methods --"

    #region "-- Code List --"

    #region "-- Indicator --"

    /// <summary>
    /// Fill/Set the Indicator Sheet
    /// </summary>
    /// <param name="excel">Excell object</param>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <param name="sheetIndex">Sheet Index</param>
    private void FillIndicatorMappingSheet(Callback objCallback, DIExcel excel, string languageCode, string dbNId, int sheetIndex)
    {

        #region " -- Declaration of Variables and Initialization of Objects -- "

        Dictionary<string, string> DictSourceIndicator = null;
        Dictionary<string, string> DictTargetIndicator = null;
        Dictionary<string, string> DictMappedIndicators = null;
        Dictionary<string, string> Header = null;
        int HiddenRowCount = 4;
        int ReferenceColumnCount = 20;
        int RowCount = 4;
        int ColumnCount = 0;

        //Get the Source and Target Indicators with their mappings
        DictSourceIndicator = objCallback.GetSourceIndicatorCodeListMapping(languageCode, dbNId);
        DictTargetIndicator = objCallback.GetTargetIndicatorCodeListMapping(languageCode, dbNId);
        try { DictMappedIndicators = objCallback.GetIndicatorMappedCodeList(dbNId); }
        catch (Exception) { }


        #endregion

        #region "-- Set Excelfile Values --"

        this.SetExcelFileValues(excel, languageCode, dbNId, sheetIndex, DictSourceIndicator, DictTargetIndicator, DictMappedIndicators, ref Header, ref HiddenRowCount, ReferenceColumnCount, ref RowCount, ColumnCount);

        #endregion

    }

    #endregion

    #region "-- Unit --"

    /// <summary>
    /// Fill/Set the Unit Sheet
    /// </summary>
    /// <param name="excel">Excell object</param>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <param name="sheetIndex">Sheet Index</param>
    private void FillUnitMappingSheet(Callback objCallback, DIExcel excel, string languageCode, string dbNId, int sheetIndex)
    {

        #region " -- Declaration of Variables and Initialization of Objects -- "

        //Set the sheetName        
        Dictionary<string, string> DictSourceUnit = null;
        Dictionary<string, string> DictTargetUnit = null;
        Dictionary<string, string> DictMappedUnit = null;
        Dictionary<string, string> Header = null;
        int HiddenRowCount = 4;
        int ReferenceColumnCount = 20;
        int RowCount = 4;
        int ColumnCount = 0;

        //Get the Source and Target Indicators with their mappings
        DictSourceUnit = objCallback.GetSourceUnitCodeListMapping(languageCode, dbNId);
        DictTargetUnit = objCallback.GetTargetUnitCodeListMapping(languageCode, dbNId);
        try { DictMappedUnit = objCallback.GetUnitMappedCodeList(dbNId); }
        catch (Exception) { }


        #endregion

        #region "-- Set Excelfile Values --"

        this.SetExcelFileValues(excel, languageCode, dbNId, sheetIndex, DictSourceUnit, DictTargetUnit, DictMappedUnit, ref Header, ref HiddenRowCount, ReferenceColumnCount, ref RowCount, ColumnCount);

        #endregion

    }

    #endregion

    #region "-- Age --"

    /// <summary>
    /// Fill/Set the Age Sheet
    /// </summary>
    /// <param name="excel">Excell object</param>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <param name="sheetIndex">Sheet Index</param>
    private void FillAgeMappingSheet(Callback objCallback, DIExcel excel, string languageCode, string dbNId, int sheetIndex, string selectedAgeCodeList)
    {

        #region " -- Declaration of Variables and Initialization of Objects -- "

        //Set the sheetName        
        Dictionary<string, string> DictSourceAge = null;
        Dictionary<string, string> DictTargetAge = null;
        Dictionary<string, string> DictMappedAge = null;
        Dictionary<string, string> Header = null;
        int HiddenRowCount = 4;
        int ReferenceColumnCount = 20;
        int RowCount = 4;
        int ColumnCount = 0;

        //Get the Source and Target Indicators with their mappings
        try { DictSourceAge = objCallback.GetSourceAgeCodeListMapping(languageCode, dbNId, selectedAgeCodeList); }
        catch (Exception) { }
        DictTargetAge = objCallback.GetTargetAgeCodeListMapping(languageCode, dbNId);
        try { DictMappedAge = objCallback.GetAgeMappedCodeList(dbNId); }
        catch (Exception) { }


        #endregion

        #region "-- Set Excelfile Values --"
        if (DictSourceAge != null)
        {
            this.SetExcelFileValues(excel, languageCode, dbNId, sheetIndex, DictSourceAge, DictTargetAge, DictMappedAge, ref Header, ref HiddenRowCount, ReferenceColumnCount, ref RowCount, ColumnCount);
        }

        #endregion

    }

    #endregion

    #region "-- Sex --"

    /// <summary>
    /// Fill/Set the Sex Sheet
    /// </summary>
    /// <param name="excel">Excell object</param>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <param name="sheetIndex">Sheet Index</param>
    private void FillSexMappingSheet(Callback objCallback, DIExcel excel, string languageCode, string dbNId, int sheetIndex, string selectedSexCodelist)
    {

        #region " -- Declaration of Variables and Initialization of Objects -- "

        //Set the sheetName        
        Dictionary<string, string> DictSourceSex = null;
        Dictionary<string, string> DictTargetSex = null;
        Dictionary<string, string> DictMappedSex = null;
        Dictionary<string, string> Header = null;
        int HiddenRowCount = 4;
        int ReferenceColumnCount = 20;
        int RowCount = 4;
        int ColumnCount = 0;

        //Get the Source and Target Indicators with their mappings
        try { DictSourceSex = objCallback.GetSourceSexCodeListMapping(languageCode, dbNId, selectedSexCodelist); }
        catch (Exception) { }
        DictTargetSex = objCallback.GetTargetSexCodeListMapping(languageCode, dbNId);
        try { DictMappedSex = objCallback.GetSexMappedCodeList(dbNId); }
        catch (Exception) { }


        #endregion

        #region "-- Set Excelfile Values --"
        if (DictSourceSex != null)
        {
            this.SetExcelFileValues(excel, languageCode, dbNId, sheetIndex, DictSourceSex, DictTargetSex, DictMappedSex, ref Header, ref HiddenRowCount, ReferenceColumnCount, ref RowCount, ColumnCount);
        }


        #endregion

    }

    #endregion

    #region "-- Location --"

    /// <summary>
    /// Fill/Set the Location Sheet
    /// </summary>
    /// <param name="excel">Excell object</param>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <param name="sheetIndex">Sheet Index</param>
    private void FillLocationMappingSheet(Callback objCallback, DIExcel excel, string languageCode, string dbNId, int sheetIndex, string selectedLocationCodelist)
    {

        #region " -- Declaration of Variables and Initialization of Objects -- "

        //Set the sheetName        
        Dictionary<string, string> DictSourceLocation = null;
        Dictionary<string, string> DictTargetLocation = null;
        Dictionary<string, string> DictMappedLocation = null;
        Dictionary<string, string> Header = null;
        int HiddenRowCount = 4;
        int ReferenceColumnCount = 20;
        int RowCount = 4;
        int ColumnCount = 0;

        //Get the Source and Target Indicators with their mappings
        try { DictSourceLocation = objCallback.GetSourceLocationCodeListMapping(languageCode, dbNId, selectedLocationCodelist); }
        catch (Exception) { }
        DictTargetLocation = objCallback.GetTargetLocationCodeListMapping(languageCode, dbNId);
        try { DictMappedLocation = objCallback.GetLocationMappedCodeList(dbNId); }
        catch (Exception) { }


        #endregion

        #region "-- Set Excelfile Values --"
        if (DictSourceLocation != null)
        {
            this.SetExcelFileValues(excel, languageCode, dbNId, sheetIndex, DictSourceLocation, DictTargetLocation, DictMappedLocation, ref Header, ref HiddenRowCount, ReferenceColumnCount, ref RowCount, ColumnCount);
        }

        #endregion

    }

    #endregion

    #region "-- Area --"

    /// <summary>
    /// Fill/Set the Area Sheet
    /// </summary>
    /// <param name="excel">Excell object</param>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <param name="sheetIndex">Sheet Index</param>
    private void FillAreaMappingSheet(Callback objCallback, DIExcel excel, string languageCode, string dbNId, int sheetIndex)
    {

        #region " -- Declaration of Variables and Initialization of Objects -- "

        //Set the sheetName        
        Dictionary<string, string> DictSourceArea = null;
        Dictionary<string, string> DictTargetArea = null;
        Dictionary<string, string> DictMappedArea = null;
        Dictionary<string, string> Header = null;
        int HiddenRowCount = 4;
        int ReferenceColumnCount = 20;
        int RowCount = 4;
        int ColumnCount = 0;

        //Get the Source and Target Indicators with their mappings
        DictSourceArea = objCallback.GetSourceAreaCodeListMapping(languageCode, dbNId);
        DictTargetArea = objCallback.GetTargetAreaCodeListMapping(languageCode, dbNId);
        try { DictMappedArea = objCallback.GetAreaMappedCodeList(dbNId); }
        catch (Exception) { }

        #endregion

        #region "-- Set Excelfile Values --"

        this.SetExcelFileValues(excel, languageCode, dbNId, sheetIndex, DictSourceArea, DictTargetArea, DictMappedArea, ref Header, ref HiddenRowCount, ReferenceColumnCount, ref RowCount, ColumnCount);

        #endregion

    }

    #endregion

    /// <summary>
    /// Export Codelist Mapping
    /// </summary>
    /// <param name="excel">Excell object</param>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNid</param>
    /// <param name="sheetIndex">Sheet Index</param>
    private void ExportCodelistMapping(DIExcel excel, string languageCode, string dbNId, int sheetIndex, string selectedAgeCodelist, string selectedSexCodelist, string selectedLocationCodelist)
    {

        Callback ObjCallback = new Callback();
        //Set the sheetName
        excel.RenameWorkSheet(sheetIndex, Global.GetLanguageKeyValue("Indicator").ToString());

        //Fill the Age, Sex and LocationCode List Values
        excel.SetCellValue(sheetIndex, 0, 50, selectedAgeCodelist);
        excel.SetCellValue(sheetIndex, 1, 50, selectedSexCodelist);
        excel.SetCellValue(sheetIndex, 2, 50, selectedLocationCodelist);

        //Hide these columns
        excel.HideRangeCells(sheetIndex, 0, 50, 3, 50);

        //Fill Indicator Sheet
        this.FillIndicatorMappingSheet(ObjCallback, excel, languageCode, dbNId, sheetIndex);

        //Insert Unit sheet
        excel.InsertWorkSheet(Global.GetLanguageKeyValue("Unit").ToString());

        //Fill Unit Sheet
        this.FillUnitMappingSheet(ObjCallback, excel, languageCode, dbNId, sheetIndex + 1);

        //Insert Age Sheet
        excel.InsertWorkSheet(Global.GetLanguageKeyValue("Age").ToString());

        //Fill Age Sheet
        this.FillAgeMappingSheet(ObjCallback, excel, languageCode, dbNId, sheetIndex + 2, selectedAgeCodelist);

        //Insert Sex Sheet
        excel.InsertWorkSheet(Global.GetLanguageKeyValue("Sex").ToString());

        //Fill Sex Sheet
        this.FillSexMappingSheet(ObjCallback, excel, languageCode, dbNId, sheetIndex + 3, selectedSexCodelist);

        //Insert Location Sheet
        excel.InsertWorkSheet(Global.GetLanguageKeyValue("Location").ToString());

        //Fill Location Sheet
        this.FillLocationMappingSheet(ObjCallback, excel, languageCode, dbNId, sheetIndex + 4, selectedLocationCodelist);

        //Insert Area Sheet
        excel.InsertWorkSheet(Global.GetLanguageKeyValue("Area").ToString());

        //Fill Area Sheet
        this.FillAreaMappingSheet(ObjCallback, excel, languageCode, dbNId, sheetIndex + 5);

        //activate the first sheet
        excel.ActivateSheet(sheetIndex);

    }

    #endregion

    #region "-- IUS --"

    /// <summary>
    /// Make Reference Columns for IUS
    /// </summary>
    /// <param name="excel">Excel Object</param>
    /// <param name="sheetIndex">Excell sheet index</param>
    /// <param name="ReferenceColumnCount">Starting column index where reference columns will start</param>
    /// <param name="HiddenRowCount">Starting row index where reference rows will start</param>
    /// <param name="DictTargetUnit">Unit list</param>
    /// <param name="DictTargetAge">Age list</param>
    /// <param name="DictTargetSex">Sex list</param>
    /// <param name="DictTargetLocation">Location list</param>
    /// <param name="DictTargetFrequency">Frequency list</param>
    /// <param name="DictTargetSourceType">Source type list</param>
    /// <param name="DictTargetNature">Nature list</param>
    /// <param name="DictTargetUnitMult">Unitmut list</param>
    private void MakeReferenceColumnsforIUS(DIExcel excel, int sheetIndex, int ReferenceColumnCount, int HiddenRowCount, Dictionary<string, string> DictTargetIndicator, Dictionary<string, string> DictTargetUnit, Dictionary<string, string> DictTargetAge, Dictionary<string, string> DictTargetSex, Dictionary<string, string> DictTargetLocation, Dictionary<string, string> DictTargetFrequency, Dictionary<string, string> DictTargetSourceType, Dictionary<string, string> DictTargetNature, Dictionary<string, string> DictTargetUnitMult)
    {
        //Set the MappedInformation Values and Hide it
        excel.SetCellValue(sheetIndex, HiddenRowCount, 29, "YES");
        excel.SetCellValue(sheetIndex, HiddenRowCount + 1, 29, "NO");
        this.IUSDropDownRowCounts = "2";

        //Make Reference Column for Indicator
        HiddenRowCount = 4;
        foreach (KeyValuePair<string, string> TargetIndicator in DictTargetIndicator)
        {
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount, TargetIndicator.Value);
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 1, TargetIndicator.Key);
            HiddenRowCount++;
        }
        this.IUSDropDownRowCounts += "~" + HiddenRowCount.ToString();
        //Hide the Reference Columns
        excel.HideRangeCells(sheetIndex, 4, ReferenceColumnCount, HiddenRowCount, ReferenceColumnCount + 1);


        //Make Reference Column for Unit
        HiddenRowCount = 4;
        foreach (KeyValuePair<string, string> TargetUnit in DictTargetUnit)
        {
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount+2, TargetUnit.Value);
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 3, TargetUnit.Key);
            HiddenRowCount++;
        }
        this.IUSDropDownRowCounts += "~" + HiddenRowCount.ToString();
        //Hide the Reference Columns
        excel.HideRangeCells(sheetIndex, 4, ReferenceColumnCount + 2, HiddenRowCount, ReferenceColumnCount + 3);


        //Make Reference Column for Age
        HiddenRowCount = 4;
        foreach (KeyValuePair<string, string> TargetAge in DictTargetAge)
        {
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 4, TargetAge.Value);
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 5, TargetAge.Key);
            HiddenRowCount++;
        }
        this.IUSDropDownRowCounts += "~" + HiddenRowCount.ToString();
        //Hide the Reference Columns
        excel.HideRangeCells(sheetIndex, 4, ReferenceColumnCount +4, HiddenRowCount, ReferenceColumnCount + 5);


        //Make Reference Column for Sex
        HiddenRowCount = 4;
        foreach (KeyValuePair<string, string> TargetSex in DictTargetSex)
        {
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 6, TargetSex.Value);
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 7, TargetSex.Key);
            HiddenRowCount++;
        }
        this.IUSDropDownRowCounts += "~" + HiddenRowCount.ToString();
        //Hide the Reference Columns
        excel.HideRangeCells(sheetIndex, 4, ReferenceColumnCount + 6, HiddenRowCount, ReferenceColumnCount + 7);


        //Make Reference Column for Location
        HiddenRowCount = 4;
        foreach (KeyValuePair<string, string> TargetLocation in DictTargetLocation)
        {
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 8, TargetLocation.Value);
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 9, TargetLocation.Key);
            HiddenRowCount++;
        }
        this.IUSDropDownRowCounts += "~" + HiddenRowCount.ToString();
        //Hide the Reference Columns
        excel.HideRangeCells(sheetIndex, 4, ReferenceColumnCount + 8, HiddenRowCount, ReferenceColumnCount + 9);


        //Make Reference Column for Frequency
        HiddenRowCount = 4;
        foreach (KeyValuePair<string, string> TargetFrequency in DictTargetFrequency)
        {
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 10, TargetFrequency.Value);
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 11, TargetFrequency.Key);
            HiddenRowCount++;
        }
        this.IUSDropDownRowCounts += "~" + HiddenRowCount.ToString();
        //Hide the Reference Columns
        excel.HideRangeCells(sheetIndex, 4, ReferenceColumnCount + 10, HiddenRowCount, ReferenceColumnCount + 11);


        //Make Reference Column for SourceType
        HiddenRowCount = 4;
        foreach (KeyValuePair<string, string> TargetSourceType in DictTargetSourceType)
        {
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 12, TargetSourceType.Value);
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 13, TargetSourceType.Key);
            HiddenRowCount++;
        }
        this.IUSDropDownRowCounts += "~" + HiddenRowCount.ToString();
        //Hide the Reference Columns
        excel.HideRangeCells(sheetIndex, 4, ReferenceColumnCount + 12, HiddenRowCount, ReferenceColumnCount + 13);


        //Make Reference Column for Nature
        HiddenRowCount = 4;
        foreach (KeyValuePair<string, string> TargetNature in DictTargetNature)
        {
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 14, TargetNature.Value);
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 15, TargetNature.Key);
            HiddenRowCount++;
        }
        this.IUSDropDownRowCounts += "~" + HiddenRowCount.ToString();
        //Hide the Reference Columns
        excel.HideRangeCells(sheetIndex, 4, ReferenceColumnCount + 14, HiddenRowCount, ReferenceColumnCount + 15);


        //Make Reference Column for UnitMult
        HiddenRowCount = 4;
        foreach (KeyValuePair<string, string> TargetUnitMult in DictTargetUnitMult)
        {
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 16, TargetUnitMult.Value);
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 17, TargetUnitMult.Key);
            HiddenRowCount++;
        }
        this.IUSDropDownRowCounts += "~" + HiddenRowCount.ToString();
        //Hide the Reference Columns
        excel.HideRangeCells(sheetIndex, 4, ReferenceColumnCount + 16, HiddenRowCount, ReferenceColumnCount + 17);
    }

    /// <summary>
    /// Make Drop down list in IUS mapping case
    /// </summary>
    /// <param name="excel">Excel object</param>
    /// <param name="sheetIndex">Excel sheet index</param>
    /// <param name="referenceColumnIndex">Reference column start index</param>
    /// <param name="columnIndex">Column count index</param>
    /// <param name="rowIndex">Rowindex</param>
    private void MakeDropDownListsForIUSMapping(DIExcel excel, int sheetIndex, int referenceColumnIndex, int columnIndex, int rowIndex)
    {
        referenceColumnIndex = 30;
        columnIndex = 3;
        string[] IUSDropDownRowCountsArray = this.IUSDropDownRowCounts.Split(new string[] { "~" }, StringSplitOptions.None);
        //Make Mapped(Yes/No) dropdown list
        excel.MakeDropDownList(sheetIndex, rowIndex, columnIndex, "$" + excel.GetSheetColumnNameByIndex(29) + "$5:$" + excel.GetSheetColumnNameByIndex(29) + "$6");

        //Make Indicator Dropdown list
        columnIndex++;
        excel.MakeDropDownList(sheetIndex, rowIndex, columnIndex, "$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$5:$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$" + (IUSDropDownRowCountsArray[1].ToString()));
        excel.SetCellValue(sheetIndex, rowIndex, columnIndex + 13, "=IF(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "," + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "5:" + excel.GetSheetColumnNameByIndex(referenceColumnIndex + 1) + (Convert.ToInt32(IUSDropDownRowCountsArray[1].ToString()) + 1) + ",2,FALSE))");

        //Make Unit Dropdown list
        referenceColumnIndex = referenceColumnIndex + 2;
        columnIndex++;
        excel.MakeDropDownList(sheetIndex, rowIndex, columnIndex, "$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$5:$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$" + (IUSDropDownRowCountsArray[2].ToString()));
        excel.SetCellValue(sheetIndex, rowIndex, columnIndex + 13, "=IF(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "," + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "5:" + excel.GetSheetColumnNameByIndex(referenceColumnIndex + 1) + (Convert.ToInt32(IUSDropDownRowCountsArray[2].ToString()) + 1) + ",2,FALSE))");

        //Make Age drop down
        referenceColumnIndex = referenceColumnIndex + 2;
        columnIndex++;
        excel.MakeDropDownList(sheetIndex, rowIndex, columnIndex, "$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$5:$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$" + (IUSDropDownRowCountsArray[3].ToString()));
        excel.SetCellValue(sheetIndex, rowIndex, columnIndex + 13, "=IF(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "," + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "5:" + excel.GetSheetColumnNameByIndex(referenceColumnIndex + 1) + (Convert.ToInt32(IUSDropDownRowCountsArray[3].ToString()) + 1) + ",2,FALSE))");

        //Make Sex drop down
        referenceColumnIndex = referenceColumnIndex + 2;
        columnIndex++;
        excel.MakeDropDownList(sheetIndex, rowIndex, columnIndex, "$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$5:$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$" + (IUSDropDownRowCountsArray[4].ToString()));
        excel.SetCellValue(sheetIndex, rowIndex, columnIndex + 13, "=IF(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "," + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "5:" + excel.GetSheetColumnNameByIndex(referenceColumnIndex + 1) + (Convert.ToInt32(IUSDropDownRowCountsArray[4].ToString()) + 1) + ",2,FALSE))");

        //Make Location drop down
        referenceColumnIndex = referenceColumnIndex + 2;
        columnIndex++;
        excel.MakeDropDownList(sheetIndex, rowIndex, columnIndex, "$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$5:$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$" + (IUSDropDownRowCountsArray[5].ToString()));
        excel.SetCellValue(sheetIndex, rowIndex, columnIndex + 13, "=IF(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "," + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "5:" + excel.GetSheetColumnNameByIndex(referenceColumnIndex + 1) + (Convert.ToInt32(IUSDropDownRowCountsArray[5].ToString()) + 1) + ",2,FALSE))");

        //Make frequency drop down
        referenceColumnIndex = referenceColumnIndex + 2;
        columnIndex++;
        excel.MakeDropDownList(sheetIndex, rowIndex, columnIndex, "$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$5:$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$" + (IUSDropDownRowCountsArray[6].ToString()));
        excel.SetCellValue(sheetIndex, rowIndex, columnIndex + 13, "=IF(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "," + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "5:" + excel.GetSheetColumnNameByIndex(referenceColumnIndex + 1) + (Convert.ToInt32(IUSDropDownRowCountsArray[6].ToString()) + 1) + ",2,FALSE))");

        //Make Source type drop down
        referenceColumnIndex = referenceColumnIndex + 2;
        columnIndex++;
        excel.MakeDropDownList(sheetIndex, rowIndex, columnIndex, "$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$5:$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$" + (IUSDropDownRowCountsArray[7].ToString()));
        excel.SetCellValue(sheetIndex, rowIndex, columnIndex + 13, "=IF(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "," + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "5:" + excel.GetSheetColumnNameByIndex(referenceColumnIndex + 1) + (Convert.ToInt32(IUSDropDownRowCountsArray[7].ToString()) + 1) + ",2,FALSE))");

        //Make Nature drop down
        referenceColumnIndex = referenceColumnIndex + 2;
        columnIndex++;
        excel.MakeDropDownList(sheetIndex, rowIndex, columnIndex, "$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$5:$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$" + (IUSDropDownRowCountsArray[8].ToString()));
        excel.SetCellValue(sheetIndex, rowIndex, columnIndex + 13, "=IF(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "," + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "5:" + excel.GetSheetColumnNameByIndex(referenceColumnIndex + 1) + (Convert.ToInt32(IUSDropDownRowCountsArray[8].ToString()) + 1) + ",2,FALSE))");

        //Make UnitMult drop down
        referenceColumnIndex = referenceColumnIndex + 2;
        columnIndex++;
        excel.MakeDropDownList(sheetIndex, rowIndex, columnIndex, "$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$5:$" + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "$" + (IUSDropDownRowCountsArray[9].ToString()));
        excel.SetCellValue(sheetIndex, rowIndex, columnIndex + 13, "=IF(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(columnIndex) + (rowIndex + 1) + "," + excel.GetSheetColumnNameByIndex(referenceColumnIndex) + "5:" + excel.GetSheetColumnNameByIndex(referenceColumnIndex + 1) + (Convert.ToInt32(IUSDropDownRowCountsArray[9].ToString()) + 1) + ",2,FALSE))");
    }

    /// <summary>
    /// Set the Mapped values of Dropdown
    /// </summary>
    /// <param name="excel">Excel object</param>
    /// <param name="sheetIndex">Excel sheet index</param>
    /// <param name="ColumnCount">Starting column index</param>
    /// <param name="RowCount">Working row index</param>
    /// <param name="DictMappingIUS">Mapped IUS list</param>
    /// <param name="DictMappingUnit">Mapped unit list</param>
    /// <param name="DictTargetUnit">Target unit list</param>
    /// <param name="DictMappingAge">Mapped age list</param>
    /// <param name="DictTargetAge">Target age list</param>
    /// <param name="DictTargetSex">Target sex list</param>
    /// <param name="DictMappingSex">Mapped sex list</param>
    /// <param name="DictMappingLocation">Mapped location list</param>
    /// <param name="DictTargetLocation">Target location list</param>
    /// <param name="DictTargetFrequency">Target frequency list</param>
    /// <param name="DictTargetSourceType">Target source type list</param>
    /// <param name="DictTargetNature">Target nature list</param>
    /// <param name="DictTargetUnitMult">Target unitmult list</param>
    /// <param name="IUSGId">IUS GID</param>
    /// <param name="UnitGId">Unit GID</param>
    /// <param name="AgeGId">Age GID</param>
    /// <param name="SexGId">Sex GId</param>
    /// <param name="LocationGId">Location GID</param>
    private void SetMappedDropDownValues(DIExcel excel, int sheetIndex, int ColumnCount, int RowCount, Dictionary<string, string> DictMappingIUS, Dictionary<string, string> DictMappingIndicator, Dictionary<string, string> DictTargetIndicator, Dictionary<string, string> DictMappingUnit, Dictionary<string, string> DictTargetUnit, Dictionary<string, string> DictMappingAge, Dictionary<string, string> DictTargetAge, Dictionary<string, string> DictTargetSex, Dictionary<string, string> DictMappingSex, Dictionary<string, string> DictMappingLocation, Dictionary<string, string> DictTargetLocation, Dictionary<string, string> DictTargetFrequency, Dictionary<string, string> DictTargetSourceType, Dictionary<string, string> DictTargetNature, Dictionary<string, string> DictTargetUnitMult, string IUSGId, string IndicatorGID, string UnitGId, string AgeGId, string SexGId, string LocationGId)
    {
        //Set the Mapping to YES/NO depending on mapped values
        if (DictMappingIUS.ContainsKey(IUSGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, 3, "YES");
        }

        ColumnCount = 4;
        //Set the Mapped Unit
        if (DictMappingIUS.ContainsKey(IUSGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetIndicator[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[0]]);
        }
        else if (DictMappingIndicator.ContainsKey(IndicatorGID))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetIndicator[DictMappingIndicator[IndicatorGID]]);
        }

        ColumnCount++;
        //Set the Mapped Unit
        if (DictMappingIUS.ContainsKey(IUSGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetUnit[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[1]]);
        }
        else if (DictMappingUnit.ContainsKey(UnitGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetUnit[DictMappingUnit[UnitGId]]);
        }

        //Set the Mapped Age
        ColumnCount++;
        if (DictMappingIUS.ContainsKey(IUSGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetAge[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[2]]);
        }
        else if (DictMappingAge.ContainsKey(AgeGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetAge[DictMappingAge[AgeGId]]);
        }
        else if (DictTargetAge.ContainsKey(Global.registryMappingAgeDefaultValue))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetAge[Global.registryMappingAgeDefaultValue]);
        }

        //Set the Mapped Sex
        ColumnCount++;
        if (DictMappingIUS.ContainsKey(IUSGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetSex[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[3]]);
        }
        else if (DictMappingSex.ContainsKey(SexGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetSex[DictMappingSex[SexGId]]);
        }
        else if (DictTargetSex.ContainsKey(Global.registryMappingSexDefaultValue))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetSex[Global.registryMappingSexDefaultValue]);
        }

        //Set the Mapped Location
        ColumnCount++;
        if (DictMappingIUS.ContainsKey(IUSGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetLocation[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[4]]);
        }
        else if (DictMappingLocation.ContainsKey(LocationGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetLocation[DictMappingLocation[LocationGId]]);
        }
        else if (DictTargetLocation.ContainsKey(Global.registryMappingLocationDefaultValue))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetLocation[Global.registryMappingLocationDefaultValue]);
        }


        //Set the Mapped Frequency
        ColumnCount++;
        if (DictMappingIUS.ContainsKey(IUSGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetFrequency[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[5]]);
        }
        else if (DictTargetFrequency.ContainsKey(Global.registryMappingFrequencyDefaultValue))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetFrequency[Global.registryMappingFrequencyDefaultValue]);
        }

        //Set the Mapped Source
        ColumnCount++;
        if (DictMappingIUS.ContainsKey(IUSGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetSourceType[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[6]]);
        }
        else if (DictTargetSourceType.ContainsKey(Global.registryMappingSourceTypeDefaultValue))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetSourceType[Global.registryMappingSourceTypeDefaultValue]);
        }


        //Set the Mapped Nature
        ColumnCount++;
        if (DictMappingIUS.ContainsKey(IUSGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetNature[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[7]]);
        }
        else if (DictTargetNature.ContainsKey(Global.registryMappingNatureDefaultValue))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetNature[Global.registryMappingNatureDefaultValue]);
        }

        //Set the Mapped UnitMult
        ColumnCount++;
        if (DictMappingIUS.ContainsKey(IUSGId))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetUnitMult[DictMappingIUS[IUSGId].Split(new string[] { "@__@" }, StringSplitOptions.None)[8]]);
        }
        else if (DictTargetUnitMult.ContainsKey(Global.registryMappingUnitMultDefaultValue))
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, DictTargetUnitMult[Global.registryMappingUnitMultDefaultValue]);
        }
    }

    /// <summary>
    /// Export the Mapping file of IUS
    /// </summary>
    private void ExportIUSMapping(DIExcel excel, int sheetIndex, string dbNId, string languageCode, string ageCodelist, string sexCodelist, string locationCodelist)
    {
        //Set the sheetName
        excel.RenameWorkSheet(sheetIndex, Global.GetLanguageKeyValue("IUS").ToString());

        #region "-- Declare and Initialize the Variables and properties --"

        SDMXObjectModel.Message.StructureType SourceCodelistStructure;
        SDMXApi_2_0.Message.StructureType TargetCodelistStructure;
        SDMXObjectModel.Message.StructureType MappingCodelistStructure;

        string Indicator = string.Empty;
        string Unit = string.Empty;
        string SubgroupVal = string.Empty;
        string IndicatorGId = string.Empty;
        string UnitGId = string.Empty;
        string SubgroupValGId = string.Empty;
        string AgeGId = string.Empty;
        string SexGId = string.Empty;
        string LocationGId = string.Empty;
        Dictionary<string, string> Header = null;
        string IndicatorCodelistId, UnitCodelistId, AreaCodelistId, AgeCodelistId, SexCodelistId, LocationCodelistId, NatureCodelistId, FreqCodelistId, SourceTypeCodelistId, UnitMultCodelistId;

        int HiddenRowCount = 4;
        int ReferenceColumnCount = 30;
        int RowCount = 4;
        int ColumnCount = 0;

        Global.GetAppSetting();

        Dictionary<string, string> DictTargetIndicator, DictSourceIUS, DictTargetUnit, DictTargetAge, DictTargetSex, DictTargetLocation, DictTargetFrequency, DictTargetSourceType, DictTargetNature, DictTargetUnitMult, DictMappingIUS, DictMappingIndicator, DictMappingUnit, DictMappingAge, DictMappingSex, DictMappingLocation;
        Callback objCallBack = new Callback();

        #endregion

        #region "-- Set the Object lists --"

        //Set the Codelist structures
        objCallBack.Get_Codelist_Source_Target_Mapping_Structure(dbNId, out SourceCodelistStructure, out TargetCodelistStructure, out MappingCodelistStructure);

        IndicatorCodelistId = string.Empty;
        UnitCodelistId = string.Empty;
        AreaCodelistId = string.Empty;
        AgeCodelistId = string.Empty;
        SexCodelistId = string.Empty;
        LocationCodelistId = string.Empty;
        NatureCodelistId = string.Empty;
        FreqCodelistId = string.Empty;
        SourceTypeCodelistId = string.Empty;
        UnitMultCodelistId = string.Empty;
        foreach (SDMXApi_2_0.Structure.DimensionType Dimensions in TargetCodelistStructure.KeyFamilies[0].Components.Dimension)
        {
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Indicator.Id)
            {
                IndicatorCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Unit.Id)
            {
                UnitCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Area.Id)
            {
                AreaCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Age.Id)
            {
                AgeCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Sex.Id)
            {
                SexCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Location.Id)
            {
                LocationCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.Frequency.Id)
            {
                FreqCodelistId = Dimensions.codelist;
            }
            if (Dimensions.conceptRef == Constants.UNSD.Concept.SourceType.Id)
            {
                SourceTypeCodelistId = Dimensions.codelist;
            }
        }
        foreach (SDMXApi_2_0.Structure.AttributeType Attributes in TargetCodelistStructure.KeyFamilies[0].Components.Attribute)
        {
            if (Attributes.conceptRef == Constants.UNSD.Concept.Nature.Id)
            {
                NatureCodelistId = Attributes.codelist;
            }
            if (Attributes.conceptRef == Constants.UNSD.Concept.UnitMult.Id)
            {
                UnitMultCodelistId = Attributes.codelist;
            }
        }

        //Set the Dictionary objects with related values. Here Source means DevInfo and Target means UNSD. Mapping is a mapping between these two.
        DictSourceIUS = objCallBack.Get_DictSourceIUS(languageCode, SourceCodelistStructure, MappingCodelistStructure);

        DictTargetIndicator = objCallBack.Get_DictTargetCodelist(languageCode, IndicatorCodelistId, TargetCodelistStructure);
        DictTargetUnit = objCallBack.Get_DictTargetCodelist(languageCode, UnitCodelistId, TargetCodelistStructure);
        DictTargetAge = objCallBack.Get_DictTargetCodelist(languageCode, AgeCodelistId, TargetCodelistStructure);
        DictTargetSex = objCallBack.Get_DictTargetCodelist(languageCode, SexCodelistId, TargetCodelistStructure);
        DictTargetLocation = objCallBack.Get_DictTargetCodelist(languageCode, LocationCodelistId, TargetCodelistStructure);
        DictTargetFrequency = objCallBack.Get_DictTargetCodelist(languageCode, FreqCodelistId, TargetCodelistStructure);
        DictTargetSourceType = objCallBack.Get_DictTargetCodelist(languageCode, SourceTypeCodelistId, TargetCodelistStructure);
        DictTargetNature = objCallBack.Get_DictTargetCodelist(languageCode, NatureCodelistId, TargetCodelistStructure);
        DictTargetUnitMult = objCallBack.Get_DictTargetCodelist(languageCode, UnitMultCodelistId, TargetCodelistStructure);

        DictMappingIUS = objCallBack.Get_DictMappingIUS(dbNId);
        DictMappingIndicator = objCallBack.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Indicator.id, MappingCodelistStructure);
        DictMappingUnit = objCallBack.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Unit.id, MappingCodelistStructure);
        DictMappingAge = objCallBack.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Age.id, MappingCodelistStructure);
        DictMappingSex = objCallBack.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Sex.id, MappingCodelistStructure);
        DictMappingLocation = objCallBack.Get_DictMappingCodelist(DevInfo.Lib.DI_LibSDMX.Constants.StructureSet.CodelistMap.Location.id, MappingCodelistStructure);

        #endregion

        #region "-- Set the Source/Target values and its header --"

        //Set Source and Target Values        
        excel.SetCellValue(sheetIndex, 0, 0, Global.GetLanguageKeyValue("Source").ToString() + ": " + this.SourceName);
        excel.MergeCells(sheetIndex, 0, 0, 0, 1);
        excel.SetRangeBold(sheetIndex, 0, 0, 0, 0);
        excel.SetCellValue(sheetIndex, 1, 0, Global.GetLanguageKeyValue("Target").ToString() + ": " + this.TargetName);
        excel.MergeCells(sheetIndex, 1, 0, 1, 1);
        excel.SetRangeBold(sheetIndex, 1, 0, 1, 0);

        //SetHeader
        Header = new Dictionary<string, string>();
        Header.Add("0", Global.GetLanguageKeyValue("Indicator").ToString());
        Header.Add("1", Global.GetLanguageKeyValue("Unit").ToString());
        Header.Add("2", Global.GetLanguageKeyValue("Subgroup").ToString());
        Header.Add("3", Global.GetLanguageKeyValue("Mapped").ToString());
        Header.Add("4", Global.GetLanguageKeyValue("Indicator").ToString());
        Header.Add("5", Global.GetLanguageKeyValue("Unit").ToString());
        Header.Add("6", Global.GetLanguageKeyValue("Age").ToString());
        Header.Add("7", Global.GetLanguageKeyValue("Sex").ToString());
        Header.Add("8", Global.GetLanguageKeyValue("Location").ToString());
        Header.Add("9", Global.GetLanguageKeyValue("Frequency").ToString());
        Header.Add("10", Global.GetLanguageKeyValue("Source_Type").ToString());
        Header.Add("11", Global.GetLanguageKeyValue("Nature").ToString());
        Header.Add("12", Global.GetLanguageKeyValue("Unit_Multiplier").ToString());
        this.SetExcelHeader(excel, sheetIndex, 3, Header);

        #endregion

        #region "-- Make Reference columns --"

        //Build the Reference Columns of each dropdown
        //Make the destination Indicator List and hide them. It will be used in the excell file as a drop down options
        this.MakeReferenceColumnsforIUS(excel, sheetIndex, ReferenceColumnCount, HiddenRowCount, DictTargetIndicator, DictTargetUnit, DictTargetAge, DictTargetSex, DictTargetLocation, DictTargetFrequency, DictTargetSourceType, DictTargetNature, DictTargetUnitMult);

        #endregion

        #region "-- Loop in each IUS and Set column Values. Set dropdowns. Map the Values. --"

        foreach (string IUSGId in DictSourceIUS.Keys)
        {
            IndicatorGId = IUSGId.Split(new string[] { "@__@" }, StringSplitOptions.None)[0];
            UnitGId = IUSGId.Split(new string[] { "@__@" }, StringSplitOptions.None)[1];
            SubgroupValGId = IUSGId.Split(new string[] { "@__@" }, StringSplitOptions.None)[2];
            //Set the values of Age, Sex and Location
            objCallBack.Get_SubgroupBreakup(SubgroupValGId, ref AgeGId, ref SexGId, ref LocationGId, SourceCodelistStructure, ageCodelist, sexCodelist, locationCodelist);
            Indicator = DictSourceIUS[IUSGId].ToString().Split(new string[] { "@__@" }, StringSplitOptions.None)[0];
            Unit = DictSourceIUS[IUSGId].ToString().Split(new string[] { "@__@" }, StringSplitOptions.None)[1];
            SubgroupVal = DictSourceIUS[IUSGId].ToString().Split(new string[] { "@__@" }, StringSplitOptions.None)[2];


            //Set the Values of Indicator Unit and Subgroup
            ColumnCount = 0;
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, Indicator);
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 1, Unit);
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 2, SubgroupVal);

            excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 13, IndicatorGId);
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 14, UnitGId);
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 15, SubgroupValGId);
            //Set Mapped Indicator GID
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 16, DictMappingIndicator[IndicatorGId].ToString());

            //Make the Drop downs for Indicator | UNIT | Age | Sex | Location | Frequency | SourceType | Nature | UnitMult. The order is same as written here in comment
            this.MakeDropDownListsForIUSMapping(excel, sheetIndex, ReferenceColumnCount, ColumnCount, RowCount);

            //Set the mapped dropdown values
            this.SetMappedDropDownValues(excel, sheetIndex, ColumnCount, RowCount, DictMappingIUS, DictMappingIndicator, DictTargetIndicator, DictMappingUnit, DictTargetUnit, DictMappingAge, DictTargetAge, DictTargetSex, DictMappingSex, DictMappingLocation, DictTargetLocation, DictTargetFrequency, DictTargetSourceType, DictTargetNature, DictTargetUnitMult, IUSGId, IndicatorGId, UnitGId, AgeGId, SexGId, LocationGId);

            RowCount++;
        }

        #endregion

        #region "-- Set column width and hide the not showing columns

        //Set the Column width
        excel.SetColumnWidth(sheetIndex, 30.00, 4, 0, RowCount, 2);
        excel.SetColumnWidth(sheetIndex, 20.00, 4, 3, RowCount, 11);

        ////Hide the columns       
        ////Hide the Indicator GID | Unit GID and SubgroupValGID
        //for (int i = 1; i <= 5; i = i++)
        //{
        //    excel.HideRangeCells(sheetIndex, 4, i, 1000, i);
        //}

        //Hide the Mapped Indicator GID Column
        excel.HideRangeCells(sheetIndex, 4, 13, 1000, 25);
        excel.HideRangeCells(sheetIndex, 4, 29, 1000, 29);

        ////Hide the GID of DropdownListValues
        //for (int i = 9; i <= 23; i = i + 2)
        //{
        //    excel.HideRangeCells(sheetIndex, 4, i, 1000, i);
        //} 

        #endregion

    }

    #endregion

    #region "-- Meta Data --"

    /// <summary>
    /// Export the Mapping file of Metadata
    /// </summary>
    private void ExportMetadataMapping(DIExcel excel, int sheetIndex, string dbNId, string languageCode)
    {
        //Set the sheetName
        excel.RenameWorkSheet(sheetIndex, Global.GetLanguageKeyValue("Metadata").ToString());
        string MappedKey = string.Empty;
        string MappedValue = string.Empty;

        #region " -- Set Header and Source Target Values --"

        //Set Source and Target Values        
        excel.SetCellValue(sheetIndex, 0, 0, Global.GetLanguageKeyValue("Source").ToString() + ": " + this.SourceName);
        excel.MergeCells(sheetIndex, 0, 0, 0, 1);
        excel.SetRangeBold(sheetIndex, 0, 0, 0, 0);
        excel.SetCellValue(sheetIndex, 1, 0, Global.GetLanguageKeyValue("Target").ToString() + ": " + this.TargetName);
        excel.MergeCells(sheetIndex, 1, 0, 1, 1);
        excel.SetRangeBold(sheetIndex, 1, 0, 1, 0);


        //SetHeader
        Dictionary<string, string> Header = new Dictionary<string, string>();
        Header.Add("0", Global.GetLanguageKeyValue("DevInfo_Metadata").ToString());
        Header.Add("1", Global.GetLanguageKeyValue("Category_GIds").ToString());
        Header.Add("2", Global.GetLanguageKeyValue("UNSD_Metadata").ToString());
        Header.Add("3", Global.GetLanguageKeyValue("Concept_Ids").ToString());
        this.SetExcelHeader(excel, sheetIndex, 3, Header);

        #endregion

        #region "-- Declare and Initialize the Variables and properties --"

        string AssociatedDbNId = string.Empty;
        //string DictSourceMetadataValue, DictTargetMetadataValue, DictMappingMetadataValue, SourceMetadataGId, TargetMetadataGId;
        Dictionary<string, string> DictSourceMetadata, DictTargetMetadata, DictMappingMetadata;
        Callback objCalBack = new Callback();

        int HiddenRowCount = 4;
        int ReferenceColumnCount = 30;
        int RowCount = 4;
        int ColumnCount = 0;

        #endregion

        #region "-- Set the object lists --"

        AssociatedDbNId = objCalBack.Get_AssociatedDB_NId(dbNId).ToString();
        DictSourceMetadata = objCalBack.Get_DictSourceMetadata(AssociatedDbNId, languageCode);
        DictTargetMetadata = objCalBack.Get_DictTargetMetadata(dbNId, languageCode);
        DictMappingMetadata = objCalBack.Get_DictMappingMetadata(dbNId);

        #endregion

        #region "-- Make reference columns --"

        foreach (KeyValuePair<string, string> TargetCategory in DictTargetMetadata)
        {
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount, TargetCategory.Value);
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 1, TargetCategory.Key);
            HiddenRowCount++;
        }
        //Hide the Reference Columns
        excel.HideRangeCells(sheetIndex, 4, ReferenceColumnCount, HiddenRowCount, ReferenceColumnCount + 1);

        #endregion

        #region "-- Fill MetaData information in Excel sheet --"

        //Loop on each source Matadata list and fill the excell sheet
        foreach (KeyValuePair<string, string> Category in DictSourceMetadata)
        {
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount, Category.Value);
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 1, Category.Key);
            excel.MakeDropDownList(sheetIndex, RowCount, ColumnCount + 2, "$" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + "$5:$" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + "$" + (HiddenRowCount));
            excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 3, "=IF(" + excel.GetSheetColumnNameByIndex(ColumnCount + 2) + (RowCount + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(ColumnCount + 2) + (RowCount + 1) + "," + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + "5:" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount + 1) + (HiddenRowCount + 1) + ",2,FALSE))");

            //If mapped metadata are there, show them mapped
            if (DictMappingMetadata.ContainsKey(Category.Key))
            {
                MappedKey = DictMappingMetadata[Category.Key];
                MappedValue = DictTargetMetadata[MappedKey];
                excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 2, MappedValue);
            }
            RowCount++;
        }

        #endregion

        #region "-- Set the column width --"

        excel.SetColumnWidth(sheetIndex, 30.00, 4, 0, RowCount, 3);

        #endregion
    }

    #endregion

    #region "-- Common --"

    /// <summary>
    /// Set Excell Header by parameters
    /// </summary>
    /// <param name="excel">Excel Object</param>
    /// <param name="sheetIndex">Excel Sheet Index</param>
    /// <param name="rowIndex">Row Index</param>
    /// <param name="headerStrings">Format is ColumnIndex, HeaderValue</param>
    private void SetExcelHeader(DIExcel excel, int sheetIndex, int rowIndex, Dictionary<string, string> headerStrings)
    {
        foreach (KeyValuePair<string, string> HeaderValue in headerStrings)
        {
            excel.SetCellValue(sheetIndex, rowIndex, Convert.ToInt32(HeaderValue.Key), HeaderValue.Value);
            excel.SetRangeBold(sheetIndex, rowIndex, Convert.ToInt32(HeaderValue.Key), rowIndex, Convert.ToInt32(HeaderValue.Key));
            excel.SetVerticalAlignment(sheetIndex, rowIndex, Convert.ToInt32(HeaderValue.Key), SpreadsheetGear.VAlign.Center);
            excel.SetHorizontalAlignment(sheetIndex, rowIndex, Convert.ToInt32(HeaderValue.Key), SpreadsheetGear.HAlign.Center);
        }
        excel.SetRowHeight(sheetIndex, 25.00, rowIndex, 0, rowIndex, 0);
    }

    /// <summary>
    /// Set Values in Excel File
    /// </summary>
    /// <param name="excel">Excel Object</param>
    /// <param name="languageCode">Language Code</param>
    /// <param name="dbNId">DBNId</param>
    /// <param name="sheetIndex">Excel Sheet Index</param>
    /// <param name="DictSource">Source List</param>
    /// <param name="DictTarget">Target List</param>
    /// <param name="DictMapped">Mapping List</param>
    /// <param name="Header">Header</param>                                   
    /// <param name="HiddenRowCount">Ref List Row Count</param>
    /// <param name="ReferenceColumnCount">Ref List Column Count</param>
    /// <param name="RowCount">Row Count</param>
    /// <param name="ColumnCount">Column Count</param>
    private void SetExcelFileValues(DIExcel excel, string languageCode, string dbNId, int sheetIndex, Dictionary<string, string> DictSource, Dictionary<string, string> DictTarget, Dictionary<string, string> DictMapped, ref Dictionary<string, string> Header, ref int HiddenRowCount, int ReferenceColumnCount, ref int RowCount, int ColumnCount)
    {
        #region " -- Set Header and Source Target Values --"

        string MappedKey = string.Empty;
        string MappedValue = string.Empty;

        //Set Source and Target Values        
        excel.SetCellValue(sheetIndex, 0, 0, Global.GetLanguageKeyValue("Source").ToString() + ": " + this.SourceName);
        excel.MergeCells(sheetIndex, 0, 0, 0, 1);
        excel.SetRangeBold(sheetIndex, 0, 0, 0, 0);
        excel.SetCellValue(sheetIndex, 1, 0, Global.GetLanguageKeyValue("Target").ToString() + ": " + this.TargetName);
        excel.MergeCells(sheetIndex, 1, 0, 1, 1);
        excel.SetRangeBold(sheetIndex, 1, 0, 1, 0);


        //SetHeader
        Header = new Dictionary<string, string>();
        Header.Add("0", Global.GetLanguageKeyValue("SourceName").ToString());
        Header.Add("1", Global.GetLanguageKeyValue("SourceGID").ToString());
        Header.Add("2", Global.GetLanguageKeyValue("TargetName").ToString());
        Header.Add("3", Global.GetLanguageKeyValue("TargetGID").ToString());
        this.SetExcelHeader(excel, sheetIndex, 3, Header);

        #endregion

        #region "-- Set Reference Columns -- "

        //Make the destination Indicator List and hide them. It will be used in the excell file as a drop down options        
        foreach (KeyValuePair<string, string> TargetIndicator in DictTarget)
        {
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount, TargetIndicator.Value);
            excel.SetCellValue(sheetIndex, HiddenRowCount, ReferenceColumnCount + 1, TargetIndicator.Key);
            HiddenRowCount++;
        }
        //Hide the Reference Columns
        excel.HideRangeCells(sheetIndex, 4, ReferenceColumnCount, HiddenRowCount, ReferenceColumnCount + 1);

        #endregion

        #region " -- Set Mapped Cells -- "

        //Set the Already mapped Values
        if (DictMapped != null)
        {
            foreach (KeyValuePair<string, string> SourceIndicator in DictSource)
            {
                if (DictMapped.ContainsKey(SourceIndicator.Key))
                {
                    excel.SetCellValue(sheetIndex, RowCount, ColumnCount, SourceIndicator.Value);
                    excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 1, SourceIndicator.Key);
                    excel.MakeDropDownList(sheetIndex, RowCount, ColumnCount + 2, "$" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + "$5:$" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + "$" + (HiddenRowCount));
                    excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 3, "=IF(" + excel.GetSheetColumnNameByIndex(ColumnCount + 2) + (RowCount + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(ColumnCount + 2) + (RowCount + 1) + "," + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + "5:" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount + 1) + (HiddenRowCount + 1) + ",2,FALSE))");

                    //Get Mapped Key
                    MappedKey = DictMapped[SourceIndicator.Key];
                    MappedValue = DictTarget[MappedKey];
                    //excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 2, "=(" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + (RowCount + 1) + ")");
                    excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 2, MappedValue);
                    RowCount++;
                }
            }
        }


        #endregion

        #region " -- Set Source and Target Column Values -- "

        //Set all values except mapped ones in the Excel sheet. Set the drop down for Target Indicator and its value in next column which will get fill up on drop down selection        
        foreach (KeyValuePair<string, string> SourceIndicator in DictSource)
        {
            if (DictMapped == null)
            {
                excel.SetCellValue(sheetIndex, RowCount, ColumnCount, SourceIndicator.Value);
                excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 1, SourceIndicator.Key);
                excel.MakeDropDownList(sheetIndex, RowCount, ColumnCount + 2, "$" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + "$5:$" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + "$" + (HiddenRowCount));
                excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 3, "=IF(" + excel.GetSheetColumnNameByIndex(ColumnCount + 2) + (RowCount + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(ColumnCount + 2) + (RowCount + 1) + "," + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + "5:" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount + 1) + (HiddenRowCount + 1) + ",2,FALSE))");
                RowCount++;
            }
            else if (!DictMapped.ContainsKey(SourceIndicator.Key))
            {
                excel.SetCellValue(sheetIndex, RowCount, ColumnCount, SourceIndicator.Value);
                excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 1, SourceIndicator.Key);
                excel.MakeDropDownList(sheetIndex, RowCount, ColumnCount + 2, "$" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + "$5:$" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + "$" + (HiddenRowCount));
                excel.SetCellValue(sheetIndex, RowCount, ColumnCount + 3, "=IF(" + excel.GetSheetColumnNameByIndex(ColumnCount + 2) + (RowCount + 1) + "=\"\",\"\", + VLOOKUP(" + excel.GetSheetColumnNameByIndex(ColumnCount + 2) + (RowCount + 1) + "," + excel.GetSheetColumnNameByIndex(ReferenceColumnCount) + "5:" + excel.GetSheetColumnNameByIndex(ReferenceColumnCount + 1) + (HiddenRowCount + 1) + ",2,FALSE))");
                RowCount++;
            }
        }

        #endregion

        #region " -- Set Read only and width of Columns -- "

        //Set the Columns Source Indicator, Source Indicator Code and Target Indicator Code as Read Only
        excel.SetReadOnlyRange(sheetIndex, 4, ColumnCount, RowCount, ColumnCount + 1);
        excel.SetReadOnlyRange(sheetIndex, 4, ColumnCount + 3, RowCount, ColumnCount + 3);

        //Set the Column width to 40 for all columns
        excel.SetColumnWidth(sheetIndex, 40.00, 4, ColumnCount, RowCount, ColumnCount + 3);

        #endregion
    }

    #endregion

    #endregion

    #endregion

    #region "-- Public --"

    #region "-- Variables / Properties --"

    #endregion

    #region "-- Methods --"

    /// <summary>
    /// Used to Export the mapping of Codelist, IUS and Area
    /// </summary>
    /// <param name="mappingType">Mapping Type</param>
    public string ExportFiles(EnumHelper.MappingType mappingType, string languageCode, string dbNId, string sourceName, string targetName, string selectedAgeCodelist, string selectedSexCodelist, string selectedLocationCodelist)
    {
        //Initialize the Excell object and activate its first sheet
        string RetVal = string.Empty;
        string FilePath = string.Empty;
        DIExcel ExportExcel = new DIExcel();
        int SheetIndex = 0;
        ExportExcel.ActivateSheet(SheetIndex);
        this.SourceName = sourceName;
        this.TargetName = targetName;
        string ExcelFileName = string.Empty;
        switch (mappingType)
        {
            case EnumHelper.MappingType.CodeList:
                this.ExportCodelistMapping(ExportExcel, languageCode, dbNId, SheetIndex, selectedAgeCodelist, selectedSexCodelist, selectedLocationCodelist);
                break;
            case EnumHelper.MappingType.IUS:
                this.ExportIUSMapping(ExportExcel, SheetIndex, dbNId, languageCode, selectedAgeCodelist, selectedSexCodelist, selectedLocationCodelist);
                break;
            case EnumHelper.MappingType.Metadata:
                this.ExportMetadataMapping(ExportExcel, SheetIndex, dbNId, languageCode);
                break;
            default:
                break;
        }

        ExcelFileName = mappingType.ToString() + "_Mapping_" + DateTime.Now.Ticks.ToString() + ".xls";

        FilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.TempMappingExcelFiles, ExcelFileName);
        if (!Directory.Exists(HttpContext.Current.Server.MapPath("stock/tempMappingFiles")))
        {
            Directory.CreateDirectory(HttpContext.Current.Server.MapPath("stock/tempMappingFiles"));
        }
        ExportExcel.SaveAs(FilePath);
        ExportExcel.Close();
        RetVal = Path.Combine(Constants.FolderName.TempMappingExcelFiles.Replace("stock\\", ""), ExcelFileName).Replace("\\", "/");
        return RetVal;
    }

    #endregion

    #endregion
}
