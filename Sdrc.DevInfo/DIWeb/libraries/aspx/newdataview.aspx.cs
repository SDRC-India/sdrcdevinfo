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



public partial class libraries_aspx_newdataview : System.Web.UI.Page
{
    protected string hdsby = string.Empty;
    protected string hdbnid = string.Empty;
    protected string hselarea = string.Empty;
    protected string hselind = string.Empty;
    protected string hlngcode = string.Empty;
    protected string hlngcodedb = string.Empty;
    protected string hselindo = string.Empty;
    protected string hselareao = string.Empty;
    protected string hdvnids = string.Empty;
    protected string hLoggedInUserNId = string.Empty;
    protected string hLoggedInUserName = string.Empty;
    protected string hCsvFilePath = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            // Read AppSettings
            Global.GetAppSetting();

            // Set page title
            Page.Title = Global.adaptation_name + " - Data";

           
            //if (Request.Files.Count > 0) handleUploadedFile(); // Save uploaded file & process it further
            //else 
            GetPostedData(); // Read http header or cookie values
            
            
        }
        catch (Exception)
        {
        }
    }


    private void GetPostedData()
    {
        // Get Posted Data - will be passed to the Javascript
        if (!string.IsNullOrEmpty(Request["hdsby"])) { hdsby = Request["hdsby"]; }
        if (!string.IsNullOrEmpty(Request["hdbnid"])) { hdbnid = Request["hdbnid"]; }
        if (!string.IsNullOrEmpty(Request["hselarea"])) { hselarea = Request["hselarea"]; }
        if (!string.IsNullOrEmpty(Request["hselind"])) { hselind = Request["hselind"]; }
        if (!string.IsNullOrEmpty(Request["hlngcode"])) { hlngcode = Request["hlngcode"]; }
        if (!string.IsNullOrEmpty(Request["hlngcodedb"])) { hlngcodedb = Request["hlngcodedb"]; }
        if (!string.IsNullOrEmpty(Request["hselindo"])) { hselindo = Request["hselindo"]; }
        if (!string.IsNullOrEmpty(Request["hselareao"])) { hselareao = Request["hselareao"]; }
        if (!string.IsNullOrEmpty(Request["hdvnids"])) { hdvnids = Request["hdvnids"]; }
        if (!string.IsNullOrEmpty(Request["hLoggedInUserNId"])) { hLoggedInUserNId = Request["hLoggedInUserNId"]; }
        if (!string.IsNullOrEmpty(Request["hLoggedInUserName"])) { hLoggedInUserName = Request["hLoggedInUserName"]; }

        handleUploadedFile();
        
        
    }

    private void saveXGP()
    {
        string tempPath = Server.MapPath("../../stock/tempCYV");

        if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);
        string FilePath = tempPath + "\\" + Environment.TickCount.ToString();
        FilePath += ".xgp";
        File.WriteAllText(FilePath, Request.Form["freeTxtData"]);
        hCsvFilePath = FilePath.Replace("\\", "\\\\");        
    }

    private void handleUploadedFile()
    {
        string tempPath = Server.MapPath("../../stock/tempCYV");

        if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);

        string FilePath = tempPath + "\\" + Environment.TickCount.ToString();

        switch (Request["cyvType"])
        {
            case "Spreadsheet":
                {
                    FilePath += ".xls";
                    Request.Files[0].SaveAs(FilePath);
                    hCsvFilePath = FilePath.Replace("\\", "\\\\");  
                    break;
                }
            case "CSV":
                {
                    hCsvFilePath = Request["csvFilePath"].Replace("\\", "\\\\"); 
                    break;
                }
            case "DataEntry":
                {
                    FilePath += ".xgp";
                    File.WriteAllText(FilePath, Request.Form["freeTxtData"]);
                    hCsvFilePath = FilePath.Replace("\\", "\\\\");     
                    break;
                }
        }


        //if (!string.IsNullOrEmpty(Request["csvFilePath"])) { hCsvFilePath = Request["csvFilePath"].Replace("\\", "\\\\"); }
        //if (!string.IsNullOrEmpty(Request["freeTxtData"])) { saveXGP(); }
        

        //if (Request.Files[0].FileName.EndsWith(".csv")) // CSV Format
        //{
        //    FilePath += ".csv";
        //    Request.Files[0].SaveAs(FilePath);
        //}
        //else if (Request.Files[0].FileName.EndsWith(".xls")) // XLS Format
        //{
        //    FilePath += ".xls";
        //    Request.Files[0].SaveAs(FilePath);
        //}
        //else
        //{
        //    FilePath += ".xgp";
        //    File.WriteAllText(FilePath, Request.Form["freeTxtData"]);
        //}       

              
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
        }
        return isConverted;
    }

    /// <summary>
    /// Converts indicator string from QDS to new string which is compatible with Indicator component
    /// </summary>
    /// <param name="QdsFormattedIU"></param>
    /// <returns></returns>
    private string getIndicatorStringForComponent(string QdsFormattedIU)
    {
        string result = string.Empty;

        

        return result;
    }

}
