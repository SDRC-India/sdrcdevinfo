using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for ImportMapping
/// </summary>
public class ImportMapping
{
    #region "-- Private --"

    #region "-- Properties --"

    private Callback objCallBack = null;
    private string languageCode = string.Empty;
    private string dbNId = string.Empty;
    private string SelectedAgeCodeList = string.Empty;
    private string SelectedSexCodeList = string.Empty;
    private string SelectedLocationCodeList = string.Empty; 

    #endregion

    #region "-- Methods --"

    #region "-- CodeList --"

    #region "-- Indicator --"

    /// <summary>
    /// Import the Indicator Mapping data from Import Excel worksheet
    /// </summary>
    /// <param name="excel">Excel Object</param>
    /// <param name="paramString">Parameter string</param>
    private void ImportIndicatorData(DIExcel excel, StringBuilder paramString)
    {
        this.ImportCodeListData(0, excel, paramString, "Indicator");
    }

    #endregion

    #region "-- Unit --"

    /// <summary>
    /// Import the Unit Mapping data from Import Excel worksheet
    /// </summary>
    /// <param name="excel">Excel Object</param>
    /// <param name="paramString">Parameter string</param>
    private void ImportUnitData(DIExcel excel, StringBuilder paramString)
    {
        this.ImportCodeListData(1, excel, paramString, "Unit");
    }

    #endregion

    #region "-- Age --"

    /// <summary>
    /// Import the Age Mapping data from Import Excel worksheet
    /// </summary>
    /// <param name="excel">Excel Object</param>
    /// <param name="paramString">Parameter string</param>
    private void ImportAgeData(DIExcel excel, StringBuilder paramString)
    {
        this.ImportCodeListData(2, excel, paramString, "Age");
    }

    #endregion

    #region "-- Sex --"

    /// <summary>
    /// Import the Sex Mapping data from Import Excel worksheet
    /// </summary>
    /// <param name="excel">Excel Object</param>
    /// <param name="paramString">Parameter string</param>
    private void ImportSexData(DIExcel excel, StringBuilder paramString)
    {
        this.ImportCodeListData(3, excel, paramString, "Sex");
    }

    #endregion

    #region "-- Location --"

    /// <summary>
    /// Import the Location Mapping data from Import Excel worksheet
    /// </summary>
    /// <param name="excel">Excel Object</param>
    /// <param name="paramString">Parameter string</param>
    private void ImportLocationData(DIExcel excel, StringBuilder paramString)
    {
        this.ImportCodeListData(4, excel, paramString, "Location");
    }

    #endregion

    #region "-- Area --"

    /// <summary>
    /// Import the Area Mapping data from Import Excel worksheet
    /// </summary>
    /// <param name="excel">Excel Object</param>
    /// <param name="paramString">Parameter string</param>
    private void ImportAreaData(DIExcel excel, StringBuilder paramString)
    {
        this.ImportCodeListData(5, excel, paramString, "Area");
    }

    #endregion

    /// <summary>
    /// Import CodeList Mapping Data
    /// </summary>
    /// <param name="excel">Excel Object</param>
    /// <param name="paramValue">Parameter value</param>
    private bool ImportCodeListMappingData(DIExcel excel, string paramValue)
    {
        bool RetVal = false;
        //Read the IndicatorSheet
        StringBuilder ParamString = new StringBuilder();
        string ResultString = string.Empty;
        string[] paramValueArray = paramValue.Split(new string[] { "[****]" }, StringSplitOptions.None);
        if (paramValueArray[3].ToString().Trim() == string.Empty)
        {
            if (excel.GetCellValue(0, 0, 50, 0, 50).Trim() != string.Empty)
            {
                paramValueArray[3] = excel.GetCellValue(0, 0, 50, 0, 50).Trim();
            }
        }
        if (paramValueArray[4].ToString().Trim() == string.Empty)
        {
            if (excel.GetCellValue(0, 1, 50, 1, 50).Trim() != string.Empty)
            {
                paramValueArray[4] = excel.GetCellValue(0, 1, 50, 1, 50).Trim();
            }
        }
        if (paramValueArray[5].ToString().Trim() == string.Empty)
        {
            if (excel.GetCellValue(0, 0, 50, 1, 50).Trim() != string.Empty)
            {
                paramValueArray[5] = excel.GetCellValue(0, 2, 50, 2, 50).Trim();
            }
        }
        paramValue = string.Empty;
        for (int i = 0; i < paramValueArray.Length; i++)
        {
            if (paramValue == string.Empty)
            {
                paramValue = paramValueArray[i].ToString();
            }
            else
            {
                paramValue = paramValue + "[****]" + paramValueArray[i].ToString();
            }
        }

        try
        {
            ParamString.Append(paramValue);
            this.objCallBack = new Callback();
            this.ImportIndicatorData(excel, ParamString);
            this.ImportUnitData(excel, ParamString);
            if (paramValueArray[3].ToString().Trim() != string.Empty)
            {
                this.SelectedAgeCodeList = paramValueArray[3].ToString();
                this.ImportAgeData(excel, ParamString);
            }
            if (paramValueArray[4].ToString().Trim() != string.Empty)
            {
                this.SelectedSexCodeList = paramValueArray[4].ToString();
                this.ImportSexData(excel, ParamString);
            }
            
            if (paramValueArray[5].ToString().Trim() != string.Empty)
            {
                this.SelectedLocationCodeList = paramValueArray[5].ToString();
                this.ImportLocationData(excel, ParamString);
            }
            this.ImportAreaData(excel, ParamString);

            //Make the XML Files        
            ResultString = ParamString.ToString();
            if (ResultString.Contains(Constants.Delimiters.RowDelimiter))
            {
                ResultString = ResultString.Substring(0, ResultString.Length - 9);
            }
            this.objCallBack.GenerateCodelistMappingXml(ResultString);
            RetVal = true;
        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    /// <summary>
    /// Import the Data and Make XML mapping files
    /// </summary>
    /// <param name="sheetIndex">Sheet Index</param>
    /// <param name="excel">Excel Object</param>
    /// <param name="paramString">Param string</param>
    /// <param name="importCode">Import Code</param>
    private void ImportCodeListData(int sheetIndex, DIExcel excel, StringBuilder paramString, string importCode)
    {
        int StartRowIndex = 4;
        string SourceGID = string.Empty;
        string TargetGId = string.Empty;
        Dictionary<string, string> DictSource = null;
        Dictionary<string, string> DictTarget = null;
        int SheetCount = excel.GetWorksheetCount();
        int RowCount = excel.GetUsedRange(sheetIndex).RowCount;

        if (RowCount > StartRowIndex)
        {
            excel.ActivateSheet(sheetIndex);
            DictSource = GenerateDict_Source_Target(importCode, true);
            DictTarget = GenerateDict_Source_Target(importCode, false);
            for (int RowIndex = StartRowIndex; RowIndex <= RowCount; RowIndex++)
            {
                //Get the Source and Target GIDs
                SourceGID = excel.GetCellValue(sheetIndex, RowIndex, 1, RowIndex, 1);
                TargetGId = excel.GetCellValue(sheetIndex, RowIndex, 3, RowIndex, 3);
                if (!(string.IsNullOrEmpty(SourceGID) || string.IsNullOrEmpty(TargetGId)))
                {
                    //Check if the mapped GID of Excelsheet lies in the Source Files
                    if (this.IsValidGID(DictSource, DictTarget, SourceGID, TargetGId))
                    {
                        //Make the parameter string
                        paramString.Append(importCode + Constants.Delimiters.ColumnDelimiter + SourceGID + Constants.Delimiters.ColumnDelimiter + TargetGId +
Constants.Delimiters.RowDelimiter);
                    }
                }
            }
        }
    }

    #endregion

    #region "-- IUS --"

    /// <summary>
    /// Import the IUS mapping data
    /// </summary>
    /// <param name="excel">Excel Object</param>
    /// <param name="paramValue">Parameter value</param>
    private string ImportIUSMappingData(DIExcel excel, string paramValue)
    {
        string RetVal = "false";
        //Read the IndicatorSheet
        StringBuilder ParamString = new StringBuilder();
        string ResultString = string.Empty;
        string OutResult = string.Empty;
        bool Mapped = false;
        bool IsRowMapped = false;

        try
        {
            ParamString.Append(paramValue);
            this.objCallBack = new Callback();

            int StartRowIndex = 4;
            int StartColIndex = 0;
            int SheetIndex = 0;
            int RowCount = excel.GetUsedRange(0).RowCount;
            if (RowCount > StartRowIndex)
            {
                //Loop through each row and import the records 
                for (int RowIndex = StartRowIndex; RowIndex <= RowCount; RowIndex++)
                {
                    //Import only those records whose mapping has been done.
                    if (excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 3, RowIndex, StartColIndex + 3) == "YES")
                    {
                        //Make sure that all the values of dropdown are selected
                        if (!string.IsNullOrEmpty(excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 17, RowIndex, StartColIndex + 17)) && !string.IsNullOrEmpty(excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 18, RowIndex, StartColIndex + 18)) && !string.IsNullOrEmpty(excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 19, RowIndex, StartColIndex + 19)) && !string.IsNullOrEmpty(excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 20, RowIndex, StartColIndex + 20)) && !string.IsNullOrEmpty(excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 21, RowIndex, StartColIndex + 21)) && !string.IsNullOrEmpty(excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 22, RowIndex, StartColIndex + 22)) && !string.IsNullOrEmpty(excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 23, RowIndex, StartColIndex + 23)) && !string.IsNullOrEmpty(excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 24, RowIndex, StartColIndex + 24)) && !string.IsNullOrEmpty(excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 25, RowIndex, StartColIndex + 25)))
                        {
                            //Make the parameter string    
                            ParamString.Append(excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 13, RowIndex, StartColIndex + 13) + Constants.Delimiters.IndGUIDSeoarator + excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 14, RowIndex, StartColIndex + 14) + Constants.Delimiters.IndGUIDSeoarator + excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 15, RowIndex, StartColIndex + 15) + Constants.Delimiters.ColumnDelimiter + excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 17, RowIndex, StartColIndex + 17) + Constants.Delimiters.ColumnDelimiter + excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 18, RowIndex, StartColIndex + 18) + Constants.Delimiters.ColumnDelimiter + excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 19, RowIndex, StartColIndex + 19) + Constants.Delimiters.ColumnDelimiter + excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 20, RowIndex, StartColIndex + 20) + Constants.Delimiters.ColumnDelimiter + excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 21, RowIndex, StartColIndex + 21) + Constants.Delimiters.ColumnDelimiter + excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 22, RowIndex, StartColIndex + 22) + Constants.Delimiters.ColumnDelimiter + excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 23, RowIndex, StartColIndex + 23) + Constants.Delimiters.ColumnDelimiter + excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 24, RowIndex, StartColIndex + 24) +
Constants.Delimiters.ColumnDelimiter + excel.GetCellValue(SheetIndex, RowIndex, StartColIndex + 25, RowIndex, StartColIndex + 25) + 
Constants.Delimiters.RowDelimiter);
                            Mapped = true;
                        }
                        IsRowMapped = true;
                    }
                }                

                //Make the XML Files        
                ResultString = ParamString.ToString();
                if (ResultString.Contains(Constants.Delimiters.RowDelimiter))
                {
                    ResultString = ResultString.Substring(0, ResultString.Length - 9);
                }
                OutResult = objCallBack.GenerateIUSMappingXml(ResultString);
                Global.GetLanguageKeyValue("");
                string[] OutResultArray = OutResult.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None);
                if (OutResultArray[0].ToString() == "true")
                {
                    RetVal = "true";
                }
                else
                {
                    RetVal = "false";
                    if (IsRowMapped == false)
                    {
                        RetVal += "false[**]No row mapped. Please map at least one row.";
                    }
                    else if (Mapped == false)
                    {
                        RetVal += "false[**]Invalid Mapping";
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
            RetVal = "false";
        }
        return RetVal;
    }

    #endregion

    #region "-- Metadata --"

    private bool ImportMetadataMappingData(DIExcel excel, string paramValue)
    {
        bool RetVal = false;
        StringBuilder ParamString = new StringBuilder();
        string ResultString = string.Empty;
      
        try
        {
            ParamString.Append(paramValue);
            this.objCallBack = new Callback();
            int StartRowIndex = 4;
            int RowCount = excel.GetUsedRange(0).RowCount;
            string SourceGID = string.Empty;
            string TargetGId = string.Empty;
            Dictionary<string, string> DictSource = null;
            Dictionary<string, string> DictTarget = null;
            if (RowCount > StartRowIndex)
            {
                DictSource = GenerateDict_Source_Target("Metadata", true);
                DictTarget = GenerateDict_Source_Target("Metadata", false);
                for (int RowIndex = StartRowIndex; RowIndex <= RowCount; RowIndex++)
                {
                    //Get the Source and Target GIDs
                    SourceGID = excel.GetCellValue(0, RowIndex, 1, RowIndex, 1);
                    TargetGId = excel.GetCellValue(0, RowIndex, 3, RowIndex, 3);
                    if (!(string.IsNullOrEmpty(SourceGID) || string.IsNullOrEmpty(TargetGId)))
                    {
                        //Check if the mapped GID of Excelsheet lies in the Source Files
                        if (this.IsValidGID(DictSource, DictTarget, SourceGID, TargetGId))
                        {
                            //Make the parameter string
                            ParamString.Append(SourceGID + Constants.Delimiters.ColumnDelimiter + TargetGId + Constants.Delimiters.RowDelimiter);
                        }
                    }
                }
            }

            //Make the XML Files        
            ResultString = ParamString.ToString();
            if (ResultString.Contains(Constants.Delimiters.RowDelimiter))
            {
                ResultString = ResultString.Substring(0, ResultString.Length - 9);
                RetVal = true;
            }
            objCallBack.GenerateMetadataMappingXml(ResultString);
        } 
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
            RetVal = false;
        }
        return RetVal;
    }

    #endregion

    #region "-- Common --"

    /// <summary>
    /// Check is GIDs are present in the Source and Target list
    /// </summary>
    /// <param name="sourcList">Source List</param>
    /// <param name="targetList">Target List</param>
    /// <param name="sourceGID">Source GID</param>
    /// <param name="targetGID">Target GID</param>
    /// <returns></returns>
    private bool IsValidGID(Dictionary<string, string> sourcList, Dictionary<string, string> targetList, string sourceGID, string targetGID)
    {
        bool RetVal = false;
        if (sourcList.ContainsKey(sourceGID))
        {
            if (targetList.ContainsKey(targetGID))
            {
                RetVal = true;
            }
        }
        return RetVal;
    }

    /// <summary>
    /// Generate the Source and Target GID List
    /// </summary>
    /// <param name="ImportCode">Import code like Indicator | Unit | Age | Sex | Location | Area</param>
    /// <param name="IsSource">True for Source and False for Target</param>
    /// <returns></returns>
    private Dictionary<string, string> GenerateDict_Source_Target(string ImportCode, bool IsSource)
    {
        Dictionary<string, string> RetVal = new Dictionary<string, string>();
        if (IsSource)
        {
            if (ImportCode == "Indicator")
            {
                RetVal = this.objCallBack.GetSourceIndicatorCodeListMapping(this.languageCode, this.dbNId);
            }
            else if (ImportCode == "Unit")
            {
                RetVal = this.objCallBack.GetSourceUnitCodeListMapping(this.languageCode, this.dbNId);
            }
            else if (ImportCode == "Age")
            {
                RetVal = this.objCallBack.GetSourceAgeCodeListMapping(this.languageCode, this.dbNId, this.SelectedAgeCodeList);
            }
            else if (ImportCode == "Sex")
            {
                RetVal = this.objCallBack.GetSourceSexCodeListMapping(this.languageCode, this.dbNId, this.SelectedSexCodeList);
            }
            else if (ImportCode == "Location")
            {
                RetVal = this.objCallBack.GetSourceLocationCodeListMapping(this.languageCode, this.dbNId, this.SelectedLocationCodeList);
            }
            else if (ImportCode == "Area")
            {
                RetVal = this.objCallBack.GetSourceAreaCodeListMapping(this.languageCode, this.dbNId);
            }
            else if (ImportCode == "Metadata")
            {
                string AssociatedDbNId = this.objCallBack.Get_AssociatedDB_NId(dbNId).ToString();
                RetVal = this.objCallBack.Get_DictSourceMetadata(AssociatedDbNId, languageCode);
            }
        }
        else
        {
            if (ImportCode == "Indicator")
            {
                RetVal = this.objCallBack.GetTargetIndicatorCodeListMapping(this.languageCode, this.dbNId);
            }
            else if (ImportCode == "Unit")
            {
                RetVal = this.objCallBack.GetTargetUnitCodeListMapping(this.languageCode, this.dbNId);
            }
            else if (ImportCode == "Age")
            {
                RetVal = this.objCallBack.GetTargetAgeCodeListMapping(this.languageCode, this.dbNId);
            }
            else if (ImportCode == "Sex")
            {
                RetVal = this.objCallBack.GetTargetSexCodeListMapping(this.languageCode, this.dbNId);
            }
            else if (ImportCode == "Location")
            {
                RetVal = this.objCallBack.GetTargetLocationCodeListMapping(this.languageCode, this.dbNId);
            }
            else if (ImportCode == "Area")
            {
                RetVal = this.objCallBack.GetTargetAreaCodeListMapping(this.languageCode, this.dbNId);
            }
            else if (ImportCode == "Metadata")
            {
                RetVal = this.objCallBack.Get_DictTargetMetadata(dbNId, languageCode);
            }
        }
        return RetVal;
    }



    #endregion

    #endregion

    #endregion

    #region "-- Public --"

    #region "-- Properties --"

    #endregion

    #region "-- Methods --"

    /// <summary>
    /// Import mapping from Excel file. Create the corresponding XML files
    /// </summary>
    /// <param name="mappingType">Mapping type - > Codelist | IUS | Medatata</param>
    /// <param name="paramString">Param string</param>
    /// <remarks>Param string should be in the format of DBNID[****]LangauageCode[****]UserNId[****]AgeCodeListGID[****]SexCodeListGId[****]LocationCodeListGID</remarks>
    public string ImportMapppingFile(EnumHelper.MappingType mappingType, string paramValue, string languageCode, string dbNId, string fileName)
    {
        //Load Excel mapping file
        string RetVal = string.Empty;
        DIExcel ExportExcel = new DIExcel(fileName);
        this.languageCode = languageCode;
        this.dbNId = dbNId;

        switch (mappingType)
        {
            case EnumHelper.MappingType.CodeList:
                RetVal = this.ImportCodeListMappingData(ExportExcel, paramValue).ToString().ToLower();
                break;
            case EnumHelper.MappingType.IUS:
                RetVal = this.ImportIUSMappingData(ExportExcel, paramValue).ToString().ToLower();
                break;
            case EnumHelper.MappingType.Metadata:
                RetVal = this.ImportMetadataMappingData(ExportExcel, paramValue).ToString().ToLower();
                break;
            default:
                break;
        }
        return RetVal;
    }

    #endregion

    #endregion
}