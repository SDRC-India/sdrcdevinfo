using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Xml;
using SpreadsheetGear;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using System.IO;
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibBAL;
using DevInfo.Lib.DI_LibSDMX;
using SDMXObjectModel.Message;
using SDMXObjectModel;
using SDMXObjectModel.Data.StructureSpecific;
using SDMXObjectModel.Structure;
using SDMXObjectModel.Common;

public partial class libraries_aspx_OatInputFile : System.Web.UI.Page
{
    #region Loading the page
    string key = string.Empty;
    /// <summary>
    /// Loading page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string FilePath = string.Empty;
        string tempPath = Server.MapPath("../../stock/tempCYV");
        string Records = string.Empty;
        string RetVal = string.Empty;
        StreamReader Reader;
        FilePath = tempPath + "\\" + Environment.TickCount.ToString();
        #region CSV file Uploading
        if (Request.Files[0].FileName.ToLower().EndsWith(".csv")) // CSV Format
        {
            FilePath += ".csv";
            Request.Files[0].SaveAs(FilePath);
            Records = getRecordsFromCSV(Request.Files[0].InputStream);
            RetVal = FilePath + "{[***]}" + Records;
        }
        #endregion
        #region DES File Uploading
        else if (Request.Files[0].FileName.ToLower().EndsWith(".xls") || Request.Files[0].FileName.ToLower().EndsWith(".xlsx")) // XLS Format
        {
            string lngCode = string.Empty;
            int dbNid;
            string isShowMapServer = string.Empty;
            if (Request.Form["lng_Code"] != null && Request.Form["db_Nid"] != null && Request.Form["ShowMapServer"] != null)
            {
                lngCode = Request.Form["lng_Code"].ToString();
                dbNid = Int32.Parse(Request.Form["db_Nid"].ToString());
                isShowMapServer = Request.Form["ShowMapServer"].ToString();
                FilePath += ".xls";
                Request.Files[0].SaveAs(FilePath);
                RetVal = UnMatchedListFromDES(FilePath, dbNid, lngCode, isShowMapServer);
            }
        }
        #endregion
        #region SDMX-ML File Uploading
        else if (Request.Files[0].FileName.ToLower().EndsWith(".xml"))
        {
            string lngCode = string.Empty;
            string dbNid = string.Empty;
            string isShowMapServer = string.Empty;
            if (Request.Form["lng_Code"] != null && Request.Form["db_Nid"] != null && Request.Form["ShowMapServer"] != null)
            {
                lngCode = Request.Form["lng_Code"].ToString();
                dbNid = Request.Form["db_Nid"].ToString();//Int32.Parse(Request.Form["db_Nid"].ToString());
                isShowMapServer = Request.Form["ShowMapServer"].ToString();
                string SDMXFile = FilePath + ".xml";
                string DESFile = FilePath + ".xls";
                SDMXObjectModel.Message.StructureType DSD;
                StructureSpecificTimeSeriesDataType Data;
                try
                {
                    string DSDFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath + "stock\\data\\" + dbNid, "sdmx\\Complete.xml");
                    //sdmxFileDoc = new XmlDocument();
                    //sdmxFileDoc.Load(Request.Files[0].InputStream);
                    Request.Files[0].SaveAs(SDMXFile);
                    Reader = new StreamReader(Request.Files[0].InputStream);
                    //  Load SDMX-ML file                   
                    Data = (StructureSpecificTimeSeriesDataType)Deserializer.LoadFromText(typeof(StructureSpecificTimeSeriesDataType), Reader.ReadToEnd());

                    // Load Complete DSD
                    DSD = (SDMXObjectModel.Message.StructureType)Deserializer.LoadFromFile(typeof(SDMXObjectModel.Message.StructureType), DSDFile);

                    if (SDMXUtility.Validate_SDMXMLFile(SDMXSchemaType.Two_One, SDMXFile))
                    {
                        File.Delete(SDMXFile);
                        string csvFilepath = SDMXFile.Substring(0, SDMXFile.Length - 4);
                        string CSVFilePath = processSDMXFileWithDSD(Data, DSD, lngCode, csvFilepath);
                        StreamReader reader = new StreamReader(CSVFilePath);
                        Records = getRecordsFromCSV(reader.BaseStream);
                        RetVal = CSVFilePath + "{[***]}" + Records;
                    }
                    else
                    {
                        RetVal = "invalid";
                    }
                    if (File.Exists(SDMXFile))
                        File.Delete(SDMXFile);
                }
                catch (Exception ex)
                {
                    RetVal = "invalid";
                    Global.CreateExceptionString(ex, null);
                }
            }
        }
        #endregion
        Response.Write(RetVal);
    }
    #endregion
    #region Private Methods
    #region Process DES(Validate DES & get unmatached list with DESfile path)
    /// <summary>
    /// Process DES(Validate DES & get unmatached list with DESfile path)
    /// </summary>
    /// <param name="DESFile">DES Physical path</param>
    /// <param name="dbNid">Database Nid</param>
    /// <param name="lngCode">Language code</param>
    /// <returns>unmatached list with DESfile path seperated by {[***]}</returns>
    private string UnMatchedListFromDES(string DESFile, int dbNid, string lngCode, string isShowMapServer)
    {
        string result = string.Empty;
        string Records = string.Empty;
        if (DevInfo.Lib.DI_LibBAL.Utility.DICommon.ISValidDIDataEntrySphreadsheet(DESFile, true))
        {
            Records = getAreaListFromXLS(DESFile);
            DataTable dt = GetDataTable(dbNid, Records, lngCode, isShowMapServer);
            Records = filterRecords(dt, Records);
            Callback obj = new Callback(this.Page);
            Records = obj.uniqueListItem(Records);
            result = DESFile + "{[***]}" + Records;
        }
        else
        {
            File.Delete(DESFile);
            result = "invalid";
        }
        return result;
    }
    #endregion

    #region  Generating OAT String
    /// <summary>
    /// Generating OAT String
    /// </summary>
    /// <param name="fileInputStream"></param>
    /// <returns></returns>
    private string GenerateOAT_String(Stream fileInputStream)
    {
        StringBuilder finalOatInput = new StringBuilder();

        StreamReader CsvFile = new StreamReader(fileInputStream);
        DataTable dtFromCsv = new DataTable("CSV");
        bool IsFirstRow = true;
        List<string> wordList = new List<string>();
        List<string> NIdValuePair = new List<string>();

        #region Convert CSV to DataTable

        while (CsvFile.Peek() != -1)
        {
            string line = CsvFile.ReadLine();
            if (line.IndexOf('"') > -1)
            {
                wordList = getDoubleQuoteString(line);
                foreach (string word in wordList)
                {
                    string temp = word.Replace(",", "{}");
                    line = line.Replace('"' + word + '"', temp);
                }
            }

            string[] nowCsvLineVals = line.Split(',');
            for (int i = 0; i < nowCsvLineVals.Length; i++)
            {
                string tempString = nowCsvLineVals[i].Replace("{}", ",");
                nowCsvLineVals[i] = tempString;
            }
            if (!IsFirstRow) //Get new row for Datatable if its not the first line of CSV
            {
                dtFromCsv.Rows.Add(nowCsvLineVals);
            }
            else //If its CSV's first line then use that for creating ColumnNames of data table
            {
                foreach (string ColumnName in nowCsvLineVals)
                {
                    dtFromCsv.Columns.Add(ColumnName);
                }
                IsFirstRow = false;
            }
        }

        #endregion

        #region Getting those Column Indexes which do not qualify for Numeric DataValues
        List<int> NonNumericColumns = new List<int>();
        for (int i = 0; i < dtFromCsv.Columns.Count; i++)
        {
            foreach (DataRow dr in dtFromCsv.Rows)
            {
                float tmpVal = 0;
                if (!float.TryParse(dr[i].ToString(), out tmpVal))
                {
                    NonNumericColumns.Add(i);
                    break;
                }
                NIdValuePair.Add(dtFromCsv.Columns[i].ColumnName);
                break;
            }
        }
        #endregion

        #region Prepare Headers & KeyValue pairs for all columns


        string Headers = string.Empty;
        string Titles = string.Empty;

        for (int i = 0; i < NonNumericColumns.Count; i++)
        {
            DataTable dtDistinctVals = dtFromCsv.DefaultView.ToTable(true, dtFromCsv.Columns[NonNumericColumns[i]].ColumnName);
            if (dtDistinctVals.Rows.Count == 1) // If Column has same data then send it to Titles
            {
                Titles += dtDistinctVals.Rows[0][0].ToString() + " - "; //dtFromCsv.Columns[NonNumericColumns[i]].ColumnName + " - ";
            }
            else // Make NId:Value pair for them and append them in headers
            {
                Headers += dtFromCsv.Columns[NonNumericColumns[i]].ColumnName + "|";
                foreach (DataRow dr in dtDistinctVals.Rows)
                {
                    NIdValuePair.Add(dr[dtFromCsv.Columns[NonNumericColumns[i]].ColumnName].ToString());
                }
            }
        }

        Headers += " - |Data Value";
        if (Titles.EndsWith(" - ")) Titles = Titles.Substring(0, Titles.Length - 3);

        #endregion

        foreach (string ValueOfKey in NIdValuePair)
        {
            finalOatInput.Append(NIdValuePair.IndexOf(ValueOfKey) + ":" + ValueOfKey + "|");
        }
        finalOatInput.Append("#");
        finalOatInput.Append(Titles + "#");

        finalOatInput.Append(Headers + "#");

        #region OAT data rows are prepared in string format


        foreach (DataRow dr in dtFromCsv.Rows)
        {
            string tmpRow = string.Empty;
            for (int i = 0; i < dtFromCsv.Columns.Count; i++)// If Non-Numeric distinct columns then append their NIds
            {
                if (Headers.Contains(dtFromCsv.Columns[i].ColumnName + "|")
                    && NonNumericColumns.Contains(i))
                {
                    tmpRow += NIdValuePair.IndexOf(dr[i].ToString()).ToString() + "|";
                }
            }
            for (int i = 0; i < dtFromCsv.Columns.Count; i++)
            {
                if (!Headers.Contains(dtFromCsv.Columns[i].ColumnName + "|")
                    && !NonNumericColumns.Contains(i)) // If Non-Numeric distinct columns then append their NIds
                {
                    finalOatInput.Append(tmpRow
                        + NIdValuePair.IndexOf(dtFromCsv.Columns[i].ColumnName).ToString()
                        + "|" + dr[i].ToString() + "#");
                }
            }
        }

        #endregion

        return finalOatInput.ToString();
    }
    #endregion

    #region double quutes words list
    /// <summary>
    /// Return double quutes words list
    /// </summary>
    /// <param name="str">String</param>
    /// <returns>list of double quotes string</returns>
    private List<string> getDoubleQuoteString(string str)
    {
        string[] wordsArray = str.Split('"');
        List<string> correctWordList = new List<string>();
        int count = -1;
        foreach (string word in wordsArray)
        {
            count++;
            if (count % 2 != 0)
            {
                correctWordList.Add(word);
            }
        }
        return correctWordList;
    }
    #endregion

    #region Geting records from csv file
    /// <summary>
    /// Geting records from csv file
    /// </summary>
    /// <param name="fileInputStream">CSV file stream</param>
    /// <returns></returns>
    private string getRecordsFromCSV(Stream fileInputStream)
    {
        string records = string.Empty;
        bool flagValidData = true;
        StreamReader CsvFileDT = new StreamReader(fileInputStream, Encoding.UTF7);
        int count = -1;
        List<string> wordList = new List<string>();
        #region Convert CSV to DataTable

        DataTable dtResult = new DataTable("CSV");
        char separator = ',';
        string lineDT = CsvFileDT.ReadLine();
        if (lineDT.IndexOf('"') > -1)
        {
            wordList = getDoubleQuoteString(lineDT);
            foreach (string word in wordList)
            {
                string temp = word.Replace(",", "{}");
                lineDT = lineDT.Replace('"' + word + '"', temp);
            }
        }
        string[] nowCsvLineVals = { };
        nowCsvLineVals = lineDT.Split(separator);
        for (int i = 0; i < nowCsvLineVals.Length; i++)
        {
            string tempString = nowCsvLineVals[i].Replace("{}", ",");
            nowCsvLineVals[i] = tempString;
        }

        int tmpSameColExistsCounter = 0;
        foreach (string ColumnName in nowCsvLineVals)
        {
            string ColName = ColumnName;
            if (dtResult.Columns.Contains(ColumnName))
            {
                ColName += tmpSameColExistsCounter.ToString();
                tmpSameColExistsCounter++;
            }
            dtResult.Columns.Add(ColName);
        }

        while (CsvFileDT.Peek() != -1)
        {
            string lineCheck = CsvFileDT.ReadLine();
            if (lineCheck.IndexOf('"') > -1)
            {
                wordList = getDoubleQuoteString(lineCheck);
                if (wordList.Count > dtResult.Columns.Count)
                {
                    flagValidData = false;
                    break;
                }

            }
        }
        if (flagValidData)
        {
            StreamReader CsvFile = new StreamReader(fileInputStream, Encoding.UTF7);
            fileInputStream.Seek(0, SeekOrigin.Begin);
            while (CsvFile.Peek() != -1)
            {
                count++;
                if (count <= 7)
                {
                    string line = CsvFile.ReadLine();
                    if (line.IndexOf('"') > -1)
                    {
                        wordList = getDoubleQuoteString(line);
                        foreach (string word in wordList)
                        {
                            string temp = word.Replace(Constants.Delimiters.Comma, "{}");
                            line = line.Replace('"' + word + '"', temp);
                        }
                    }
                    line = line.Replace(",", ((char)9).ToString());
                    line = line.Replace("{}", Constants.Delimiters.Comma);
                    records += line;
                    records += "\r\n";
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            records = "Invalid Record";
        }


        #endregion
        return records;
    }
    #endregion

    #region Save xls file to temp location in server
    /// <summary>
    /// Save xls file to temp location in server
    /// </summary>
    /// <param name="fileInputStream">Stream Object</param>
    /// <param name="xlsFilePath">xls file's path</param>
    /// <returns>true if successful else false</returns>
    private bool SaveXlsFileToTempLocation(Stream fileInputStream, string xlsFilePath)
    {
        bool isSaved = true;
        try
        {

            int length = 256;
            int bytesRead = 0;
            Byte[] buffer = new Byte[length];

            // write the required bytes
            using (FileStream fs = new FileStream(xlsFilePath, FileMode.Create))
            {
                do
                {
                    bytesRead = fileInputStream.Read(buffer, 0, length);
                    fs.Write(buffer, 0, bytesRead);
                }
                while (bytesRead == length);
            }

            fileInputStream.Dispose();
        }
        catch (Exception ex)
        {
            isSaved = false;
            Global.CreateExceptionString(ex, null);
        }
        return isSaved;
    }
    #endregion

    #region convert xls format to csv format for OAT component
    /// <summary>
    /// convert xls format to csv format for OAT component
    /// </summary>
    /// <param name="xlsFilePath">xls file's path</param>
    /// <param name="csvFilePath">csv file's path</param>
    ///  <returns>true if successful else false</returns>
    private bool convertXLSToCSV(string xlsFilePath, string csvFilePath)
    {
        bool isConverted = true;
        try
        {
            IWorkbook workBook = SpreadsheetGear.Factory.GetWorkbook(xlsFilePath);
            IWorksheet workSheet = workBook.Worksheets[0];
            string indicator = string.Empty;
            if (workSheet.Cells["B5"].Value != null)
            {
                indicator = workSheet.Cells["B5"].Value.ToString();
            }
            string unit = string.Empty;
            if (workSheet.Cells["B7"].Value != null)
            {
                unit = workSheet.Cells["B7"].Value.ToString();
            }
            if (workSheet.Cells["D9"].Value != null)
            {
                workSheet.Cells["D9"].Value = "Values";
            }
            // Delete rows
            workSheet.Cells.Range["1:8"].Delete();
            workSheet.Cells.Range["2:2"].Delete();

            // Delete area id column
            workSheet.Cells.Range["B:B"].UnMerge();
            workSheet.Cells.Range["B:B"].Delete();

            // Delete footnotes & Denominator columns
            workSheet.Cells.Range["F:X"].UnMerge();
            workSheet.Cells.Range["F:X"].Delete();

            // Add Column in worksheet
            IRange Range = workSheet.Range[0, 0, workSheet.Cells.Rows.Count - 1, 0];
            Range.Insert(InsertShiftDirection.Right);
            workSheet.Cells["A1"].Value = "Indicator";
            string source = workSheet.Cells["F2"].Value.ToString();
            bool isSingleSource = true;
            //indicator = indicator.Insert(indicator.Length, ", ");
            string newValue = indicator + ", " + unit;
            for (int rowNo = 2; rowNo <= workSheet.UsedRange.Rows.Count; rowNo++)
            {
                workSheet.Cells["A" + rowNo].NumberFormat = "@";
                workSheet.Cells["A" + rowNo].Value = newValue;
                if (rowNo != 2 && source != workSheet.Cells["F" + rowNo].Value.ToString() && isSingleSource == true)
                {
                    isSingleSource = false;
                }
                workSheet.Cells["B" + rowNo].NumberFormat = "@";
                workSheet.Cells["B" + rowNo].Value = '"' + workSheet.Cells["B" + rowNo].Text + '"';
            }

            // Delete source column if more than 1 source
            if (!isSingleSource)
            {
                workSheet.Cells.Range["F:F"].UnMerge();
                workSheet.Cells.Range["F:F"].Delete();
            }
            workBook.SaveAs(csvFilePath, FileFormat.CSV);
        }
        catch (Exception ex)
        {
            isConverted = false;
            Global.CreateExceptionString(ex, null);
        }
        return isConverted;
    }
    #endregion

    #region Get DataTable
    /// <summary>
    /// Get DataTable
    /// </summary>
    /// <param name="DBId">Database Id</param>
    /// <param name="arealist">Area Id/Name List</param>  
    /// <param name="lngCode">Language code</param>
    /// <returns>Matched records</returns>
    private DataTable GetDataTable(int DBId, string arealist, string lngCode, string isShowMapServer)
    {
        DataTable dtMatchList = new DataTable();
        StringBuilder sbResult = new StringBuilder();
        DIConnection _DBCon = null;

        if (isShowMapServer == "true")
        {
            _DBCon = new DIConnection(Global.GetMapServerConnectionDetails());
            lngCode = Global.GetMapServerLangCode(lngCode);
        }
        else
        {
            _DBCon = Global.GetDbConnection(DBId);
        }

        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();

        string AreaList = string.Empty;
        #region Prepare parameters & stored procedure execution.

        try
        {

            if (!String.IsNullOrEmpty(arealist))
            {
                AreaList = SqlParameterFormatString(arealist, "{}");
            }
            if (_DBCon != null)
            {
                System.Data.Common.DbParameter Param1 = _DBCon.CreateDBParameter();
                Param1.ParameterName = "type";
                Param1.DbType = DbType.String;
                Param1.Value = key;
                DbParams.Add(Param1);

                System.Data.Common.DbParameter Param2 = _DBCon.CreateDBParameter();
                Param2.ParameterName = "arealist";
                Param2.DbType = DbType.String;
                Param2.Value = AreaList;
                DbParams.Add(Param2);

                //Get all data with MRD, Sources & Time period filter. If needed then DataValue filters would be applied over this set of data.
                dtMatchList = _DBCon.ExecuteDataTable("sp_get_matcharealist_" + lngCode, CommandType.StoredProcedure, DbParams);

                return dtMatchList;

            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return dtMatchList;

        }

        finally
        {
            if (_DBCon != null)
            {
                _DBCon.Dispose();
            }

        }
        #endregion
        return dtMatchList;
    }
    #endregion

    #region Filter records with matched records
    /// <summary>
    /// Filter records with matched records
    /// </summary>
    /// <param name="dt">Matched Datatable</param>
    /// <param name="list">List</param>
    /// <returns>Filterd records</returns>
    private string filterRecords(DataTable dt, string list)
    {
        string result = string.Empty;
        if (dt.Rows.Count > 0)
        {
            string[] allList = list.Split(new string[] { "{}" }, StringSplitOptions.None);
            foreach (string str in allList)
            {
                if (!String.IsNullOrEmpty(str))
                {
                    bool isFound = false;
                    foreach (DataRow row in dt.Rows)
                    {
                        if (str == row[0].ToString())
                        {
                            isFound = true;
                            break;
                        }
                    }
                    if (!isFound)
                    {
                        result += str + "{}";
                    }
                }
            }
            if (!String.IsNullOrEmpty(result))
            {
                result = result.Substring(0, result.Length - 2);
            }
        }
        else
        {
            result = list;
        }
        return result;
    }
    #endregion

    #region Prepair parameter string for procedure
    /// <summary>
    /// Prepair parameter string for procedure
    /// </summary>
    /// <param name="unformatedString">Unformated string</param>
    /// <param name="delemeter">delemeter in unformated string</param>
    /// <returns>Formated string</returns>
    private string SqlParameterFormatString(string unformatedString, string delemeter)
    {
        string formatedString = string.Empty;
        string[] strList = null;
        strList = unformatedString.Split(new String[] { delemeter }, StringSplitOptions.None);
        foreach (string str in strList)
        {
            if (!String.IsNullOrEmpty(str))
            {
                formatedString += Constants.Delimiters.SingleQuote + str.Replace("'", "''") + Constants.Delimiters.SingleQuote + Constants.Delimiters.Comma;
            }
        }
        if (!String.IsNullOrEmpty(formatedString))
        {
            formatedString = formatedString.Substring(0, formatedString.Length - 1);
        }
        return formatedString;
    }
    #endregion

    #region Getting list from excel sheet
    /// <summary>
    /// Getting list from excel sheet
    /// </summary>
    /// <param name="fileStream">File stream</param>
    /// <returns></returns>
    private string getAreaListFromXLS(string FilePath)
    {
        string records = string.Empty;
        bool isAreaIdCol = false;
        bool isAreaNameCol = false;
        int AreaIdIndex = 0, AreaNameIndex = 0;
        string colName = string.Empty;
        IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook(FilePath);
        IWorksheet workSheet = workbook.Worksheets[0];
        // Remove Top 8 rows
        workSheet.Cells.Range["1:8"].Delete();
        int NoOfColumn = 8;
        IRange headerRange = workSheet.Cells[0, 0, 0, NoOfColumn - 1];
        for (int i = 0; i < 8; i++)
        {
            colName = headerRange.Columns[0, i].Value.ToString().ToLower();
            if (colName == "area id")
            {
                AreaIdIndex = i;
                isAreaIdCol = true;
            }
            if (colName == "area name")
            {
                AreaNameIndex = i;
                isAreaNameCol = true;
            }
        }
        workSheet.Cells.Range["1:2"].Delete();
        if (!isAreaIdCol && isAreaNameCol)
        {
            key = "aname";
            records = getMatchingList(workSheet, AreaNameIndex);
        }
        else
        {
            key = "aid";
            records = getMatchingList(workSheet, AreaIdIndex);
        }
        workbook.Close();
        //if(File.Exists(FilePath))
        //File.Delete(FilePath);
        return records;
    }
    #endregion

    #region Get list from worksheet
    /// <summary>
    /// Get list from worksheet
    /// </summary>
    /// <param name="workSheet">Worksheet object</param>
    /// <param name="colIndex">column index</param>
    /// <returns>list</returns>
    private string getMatchingList(IWorksheet workSheet, int colIndex)
    {
        string result = string.Empty;
        string Seperator = "{}";
        int NoOfRows = workSheet.UsedRange.Rows.Count;
        for (int i = 0; i < NoOfRows; i++)
        {
            result += Seperator + workSheet.Cells[i, colIndex].Value.ToString();
        }
        result = result.Substring(2);
        return result;
    }
    #endregion

    #region Process SDMX-ML file with Complete DSD
    /// <summary>
    /// Process SDMX-ML file with complete DSD to make csv file
    /// </summary>
    /// <param name="sdmxSeries">Series nodes in SDMX-ML </param>
    /// <param name="DSD">DSD Object</param>
    /// <param name="lngCode">language code</param>
    /// <param name="csvFilepath">CSV file path</param>
    /// <returns></returns>
    private string processSDMXFileWithDSD(StructureSpecificTimeSeriesDataType sdmxSeries, SDMXObjectModel.Message.StructureType DSD, string lngCode, string csvFilepath)
    {
        string result = string.Empty;
        string codeListCode = string.Empty;
        string attributeID = string.Empty;
        string value = string.Empty;
        string obsValue = string.Empty;
        string timePeriod = string.Empty;
        List<string> dataForCSV = new List<string>();
        string dataRow = string.Empty;
        string seperator = "{@@}";
        SeriesType series = null;
        int i = 0;
        // Make heading row        
        series = (SeriesType)sdmxSeries.DataSet[0].Items[i];
        for (int j = 0; j < series.AnyAttr.Count; j++)
        {
            XmlAttribute seriesAttr = series.AnyAttr[j];
            dataRow += seperator + seriesAttr.Name;
        }
        /*for (int c = 0; c < series.Obs[0].AnyAttr.Count; c++)
        {
            dataRow += seperator + series.Obs[0].AnyAttr[c].Name;
        }*/
        dataRow += seperator + "OBS_VALUE" + seperator + "Time_Period";
        dataRow = dataRow.Substring(4);
        dataForCSV.Add(dataRow);


        for (i = 0; i < sdmxSeries.DataSet[0].Items.Count; i++)
        {
            dataRow = string.Empty;
            string tempDataRow = string.Empty;
            series = (SeriesType)sdmxSeries.DataSet[0].Items[i];
            for (int j = 0; j < series.AnyAttr.Count; j++)
            {
                XmlAttribute seriesAttr = series.AnyAttr[j];
                attributeID = seriesAttr.Value;
                switch (seriesAttr.Name)
                {
                    case Constants.MyData.INDICATOR:
                        codeListCode = "CL_INDICATOR";
                        break;
                    case Constants.MyData.UNIT:
                        codeListCode = "CL_UNIT";
                        break;
                    case Constants.MyData.LOCATION:
                        codeListCode = "CL_LOCATION";
                        break;
                    case Constants.MyData.SEX:
                        codeListCode = "CL_SEX";
                        break;
                    case Constants.MyData.AGE:
                        codeListCode = "CL_AGE";
                        break;
                    case Constants.MyData.OTHER:
                        codeListCode = "CL_OTHER";
                        break;
                    case Constants.MyData.AREA:
                        codeListCode = "CL_AREA";
                        break;
                    case Constants.MyData.SOURCE:
                        codeListCode = "CL_SOURCE";
                        break;
                }
                value = getCorrespondingValueByIdAndType(DSD, codeListCode, attributeID, lngCode);
                dataRow += seperator + value;
            }
            dataRow = dataRow.Substring(4);
            //tempDataRow = dataRow.Substring(4);
            if (series.Obs.Count > 0) // Reading Obs nodes
            {
                for (int k = 0; k < series.Obs.Count; k++)
                {
                    tempDataRow = dataRow;
                    /*for (int c = 0; c < series.Obs[k].AnyAttr.Count; c++)
                    {
                        tempDataRow += seperator + series.Obs[k].AnyAttr[c].Value;
                    }*/
                    tempDataRow += seperator + series.Obs[k].OBS_VALUE + seperator + series.Obs[k].TIME_PERIOD;
                    dataForCSV.Add(tempDataRow);
                }
            }
        }
        result = createCSV_By_SdmxMl(dataForCSV, csvFilepath);
        return result;
    }
    #endregion

    #region getting value by id from complete DSD
    private string getCorrespondingValueByIdAndType(SDMXObjectModel.Message.StructureType DSD, string codeListCode, string ID, string lngCode)
    {
        string result = ID;
        try
        {
            for (int i = 0; i < DSD.Structures.Codelists.Count; i++)
            {
                SDMXObjectModel.Structure.CodelistType codelistNode = DSD.Structures.Codelists[i];
                if (codelistNode.id == codeListCode)
                {
                    for (int j = 0; j < codelistNode.Items.Count; j++)
                    {
                        CodeType structureCodeNode = (CodeType)codelistNode.Items[j];
                        if (structureCodeNode.id == ID)
                        {
                            for (int k = 0; k < structureCodeNode.Name.Count; k++)
                            {
                                TextType commanNameNode = (TextType)structureCodeNode.Name[k];
                                if (commanNameNode.lang.ToLower() == lngCode.ToLower())
                                {
                                    result = commanNameNode.Value;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return result;
    }
    #endregion
    private string createCSV_By_SdmxMl(List<string> sdmxmlData, string filename)
    {
        string result = string.Empty;
        IWorkbook workbook = SpreadsheetGear.Factory.GetWorkbook();
        IWorksheet workSheet = workbook.Worksheets[0];
        for (int row = 0; row < sdmxmlData.Count; row++)
        {
            string[] columns = sdmxmlData[row].Split(new string[] { "{@@}" }, StringSplitOptions.None);
            for (int column = 0; column < columns.Length; column++)
            {
                workSheet.Cells[row, column].Value = columns[column];
            }
        }
        result = filename + ".csv";
        workbook.SaveAs(result, FileFormat.CSV);
        workbook.Close();
        return result;
    }
    #endregion
}
