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
using SpreadsheetGear;
using DevInfo.Lib.DI_LibBAL.UI.Presentations.DIExcelWrapper;
using System.IO;

public partial class libraries_aspx_NormalizeData : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.Write(getOAT_string());
    }

        
    private string getOAT_string()
    {
        string CallbackResult = string.Empty;

        if (Request.Files.Count == 0) // Data is sent via HTTP POST method only. (No file uploading is done in this case)
        {
            // Retreive all form post parameters. They can vary from page to page requirements.
            string reqMethod = Request["callback"].ToString();
            string reqParams = Request[Constants.RequestHeaderParamNames.Param1];

            Callback OCallback = new Callback(this.Page);

            switch (reqMethod)
            {

                case "dvSelector":
                    CallbackResult = OCallback.GetDataView(reqParams);
                    break;
                case "dvQDS":
                    CallbackResult = OCallback.GetDataViewDataNids(reqParams);
                    break;

                default:
                    break;
            }
        }
        else // CSV or XLS file is uploaded 
        {
            if (Request.Files[0].FileName.EndsWith(".csv")) // CSV Format
            {
                CallbackResult = GenerateOAT_String(Request.Files[0].InputStream);
            }
            else if (Request.Files[0].FileName.EndsWith(".xls")) // XLS Format
            {
                #region XLS file handling
                string tempPath = Server.MapPath("../../stock");
                string xlsFilePath = tempPath + "\\temp.xls";
                string csvFilePath = tempPath + "\\temp.csv";
                if (SaveXlsFileToTempLocation(Request.Files[0].InputStream, xlsFilePath))// Save xls file
                {
                    if (convertXLSToCSV(xlsFilePath, csvFilePath)) // Convert xls to csv
                    {
                        StreamReader csvStream = new StreamReader(csvFilePath);
                        CallbackResult = GenerateOAT_String(csvStream.BaseStream);
                        csvStream.Close();
                        csvStream.Dispose();
                        // Delete files after done task once.
                        File.Delete(xlsFilePath);
                        File.Delete(csvFilePath);
                    }
                }
                #endregion
            }
        }

        return CallbackResult;

    }

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


}
