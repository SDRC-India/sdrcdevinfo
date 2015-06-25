using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using SpreadsheetGear;

/// <summary>
/// Summary description for XLSLogGenerator
/// </summary>
public static class XLSLogGenerator
{


    internal static string FileNamePrefix = Constants.FileName.XLSLogFileName;
   

    /// <summary>
    /// Get file name
    /// </summary>
    /// <returns></returns>
    private static string GetFileName()
    {
        string RetVal;

        RetVal = XLSLogGenerator.FileNamePrefix;

        return RetVal;
    }

    /// <summary>
    /// Writes Logs into a XLS file under the specified path
    /// </summary>
    /// <param name="UserName"> admin name</param>
    /// <param name="Module">module in which change is done</param>
    /// <param name="Details"> Detail of changes</param>
    /// <param name="UserEmailID"> email id of user</param>
    ///  /// <param name="ClientIpAddress"> Ip Address of user</param>
    public static void WriteLogInXLSFile(string UserName, string Module, string Details, string UserEmailID,string ClientIpAddress)
    {
        string FileName = string.Empty;
        string FilePath = string.Empty;
        string CurrentDateTime = string.Empty;
        string FileFullName = string.Empty;
        string directryPath = string.Empty;
        try
        {
            CurrentDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            //Get Directort Path
            directryPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.CSVLogPath);
            // Get file Name
            FileName = XLSLogGenerator.GetFileName();

            // Get a FilePath
            FilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, directryPath, FileName) + ".xls";

            // create directory if not exists
            if (!Directory.Exists(directryPath))
            {
                Directory.CreateDirectory(directryPath);
            }
            // If file not exist create file with header
            if (!File.Exists(FilePath))
            {
                IWorksheet ExcelSheet = (IWorksheet)Factory.GetWorkbook().Sheets[0];
                // Create Header of XLS file  
                CreateXLSFileHeader(ref ExcelSheet, FileName);
                //ref IWorksheet ExcelSheet, string FilePath, string UserName,string Date, string Module, string Details, string UserEmailId
                WriteXLSLogs(ref ExcelSheet, FilePath, UserName, CurrentDateTime, Module, Details, UserEmailID, ClientIpAddress);
            }
            // If file exist directally write on file
            else
            {
                IWorksheet ExcelSheet1 = (IWorksheet)Factory.GetWorkbook(FilePath).Sheets[0];
                WriteXLSLogs(ref ExcelSheet1, FilePath, UserName, CurrentDateTime, Module, Details, UserEmailID, ClientIpAddress);
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
    }

    /// <summary>
    /// Create Header of XLS File
    /// </summary>
    /// <param name="FilePath"></param>
    private static void CreateHeader(string FilePath)
    {
        StringBuilder StrBld = new StringBuilder();
        try
        {
            //If file exists then create a file and insert the header text 
            if (!File.Exists(FilePath))
            {
                StrBld.Append("User");
                StrBld.Append(",");
                StrBld.Append("Module");
                StrBld.Append(",");
                StrBld.Append("Date");
                StrBld.Append(",");
                StrBld.Append("Details");
                StrBld.Append(",");
                StrBld.Append("UserEmailID");
                File.WriteAllText(FilePath, StrBld.ToString());
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
    }


    /// <summary>
    /// Create Header of XLS File
    /// </summary>
    /// <param name="FilePath"></param>
    private static void CreateSDMXCSVHeader(string FilePath)
    {
        StringBuilder StrBld = new StringBuilder();
        try
        {
            //If file exists then create a file and insert the header text 
            if (!File.Exists(FilePath))
            {
                StrBld.Append("Artifact Name");
                StrBld.Append(",");
                StrBld.Append("Artifact Id");
                StrBld.Append(",");
                StrBld.Append("Date");
                StrBld.Append(",");
                StrBld.Append("Status");
                StrBld.Append(",");
                StrBld.Append("Details");
                File.WriteAllText(FilePath, StrBld.ToString());
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
    }

    /// <summary>
    /// Write log into XLS file
    /// </summary>
    /// <param name="FilePath"></param>
    /// <param name="CurrentDateTime"></param>
    /// <param name="UserName"></param>
    /// <param name="Module"></param>
    /// <param name="Details"></param>
    private static void WriteToCsvFile(string FilePath, string CurrentDateTime, string UserName, string Module, string Details, string UserEmailId)
    {
        StringBuilder StrBld = new StringBuilder();
        try
        {
            StrBld.Append("\r\n");
            // Implement special handling for values that contain comma or quote
            // Enclose in quotes and double up any double quotes
            if (UserName.IndexOfAny(new char[] { '"', ',' }) != -1)
                StrBld.AppendFormat("\"{0}\"", UserName.Replace("\"", "\"\""));
            else
                StrBld.Append(UserName);
            StrBld.Append(",");
            StrBld.Append(Module);
            StrBld.Append(",");
            StrBld.Append(CurrentDateTime);
            StrBld.Append(",");
            if (Details.IndexOfAny(new char[] { '"', ',' }) != -1)
                StrBld.AppendFormat("\"{0}\"", Details.Replace("\"", "\"\""));
            else
                StrBld.Append(Details);
            StrBld.Append(",");
            StrBld.Append(UserEmailId);

            File.AppendAllText(FilePath, StrBld.ToString());
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
    }

    public static void WriteXLSLogs(ref IWorksheet ExcelSheet, string FilePath, string UserName, string Date, string Module, string Details, string UserEmailId, string ClientIpAddress)
    {
        try
        {

            int RowIndex =Convert.ToInt32(ExcelSheet.UsedRange.RowCount);
            ExcelSheet.Cells[RowIndex, 0].Value = UserName; //  "First sentence\r\n" + "Second sentence";
            ExcelSheet.Cells[RowIndex, 1].Value = Module;
            ExcelSheet.Cells[RowIndex, 2].Value = Date;
            ExcelSheet.Cells[RowIndex, 3].Value = Details;
            ExcelSheet.Cells[RowIndex, 4].Value = UserEmailId;
            ExcelSheet.Cells[RowIndex, 5].Value = ClientIpAddress;
            ExcelSheet.Cells[RowIndex, 0].Borders.Color = System.Drawing.Color.Black;
            ExcelSheet.Cells[RowIndex, 1].Borders.Color = System.Drawing.Color.Black;
            ExcelSheet.Cells[RowIndex, 2].Borders.Color = System.Drawing.Color.Black;
            ExcelSheet.Cells[RowIndex, 3].Borders.Color = System.Drawing.Color.Black;
            ExcelSheet.Cells[RowIndex, 4].Borders.Color = System.Drawing.Color.Black;
            ExcelSheet.Cells[RowIndex, 5].Borders.Color = System.Drawing.Color.Black;
            ExcelSheet.Name = GetFileName();
            ExcelSheet.SaveAs(FilePath, SpreadsheetGear.FileFormat.Excel8);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }


    private static void CreateXLSFileHeader(ref IWorksheet excelSheet, string FileName)
    {
        try
        {
            int RowIndex = 0;
            int ColumnIndex = 0;
            List<string> ListHeaderName = new List<string>();
            ListHeaderName.Add("Name");
            ListHeaderName.Add("Module");
            ListHeaderName.Add("Date");
            ListHeaderName.Add("Details");
            ListHeaderName.Add("UserEmailID");
            ListHeaderName.Add("IpAddress");
            foreach (string StrHeaderName in ListHeaderName)
            {
                excelSheet.Cells[RowIndex, ColumnIndex].Value = StrHeaderName;
                excelSheet.Cells[RowIndex, ColumnIndex].Borders.Color = System.Drawing.Color.Black; // Sets The Border Color For Header
                excelSheet.Cells[RowIndex, ColumnIndex].Interior.Color = System.Drawing.Color.Gray; // Sets The Background Color For Header
                excelSheet.Cells[RowIndex, ColumnIndex].Font.Bold = true; // Sets Header Font To Bold
                excelSheet.Cells[RowIndex, ColumnIndex].Font.Color = System.Drawing.Color.White; // Sets Header Font Color To White
                excelSheet.Cells[RowIndex, ColumnIndex].Font.Size = 12;
                excelSheet.Cells[RowIndex, ColumnIndex].VerticalAlignment = VAlign.Top;

                SpreadsheetGear.IRange Range = excelSheet.Range[RowIndex, ColumnIndex, 1, ColumnIndex + 1];
                if (StrHeaderName == "Details")
                {
                    Range.ColumnWidth = 80.00;
                }
                else
                {
                    Range.ColumnWidth = 23.00;
                }
               ColumnIndex++;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }


    public static void WriteCSVLogForMailStatus(string ArtifactName ,string ArtifactId, string Details, string MailStatus)
    {
        string FileName = string.Empty;
        string FilePath = string.Empty;
        string CurrentDateTime = string.Empty;
        string FileFullName = string.Empty;
        string directryPath = string.Empty;
        try
        {
            CurrentDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss tt");

            //Get Directort Path
            directryPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.CSVLogPath);
            // Get file Name
            FileName =Constants.FileName.SDMXLogFileName;

            // Get a FilePath
            FilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, directryPath, FileName) + ".csv";

            // create directory if not exists
            if (!Directory.Exists(directryPath))
            {
                Directory.CreateDirectory(directryPath);
            }
            // Create Header of CSV file  
            XLSLogGenerator.CreateSDMXCSVHeader(FilePath);
            StringBuilder StrBulider = new StringBuilder();
            XLSLogGenerator.WriteToCsvFile(FilePath, CurrentDateTime, ArtifactName, ArtifactId, Details, MailStatus);
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
    }

    // main calling method for writing log of patch installation
    public static void WriteLogForPatchInstallation(string Message, string Status,string ExceptionMsg)
    {

        string FileName = string.Empty;
        string FilePath = string.Empty;
        string CurrentDateTime = string.Empty;
        string FileFullName = string.Empty;
        string directryPath = string.Empty;
        try
        {
            CurrentDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            //Get Directort Path
            directryPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.CSVLogPath);
            // Get file Name
            FileName = Constants.PatchConstaints.PatchInstLogFileName;

            // Get a FilePath
            FilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, directryPath, FileName) + ".xls";

            // create directory if not exists
            if (!Directory.Exists(directryPath))
            {
                Directory.CreateDirectory(directryPath);
            }
            // If file not exist create file with header
            if (!File.Exists(FilePath))
            {
                IWorksheet ExcelSheet = (IWorksheet)Factory.GetWorkbook().Sheets[0];
                // Create Header of XLS file  
                CreatePatchInstFileHeader(ref ExcelSheet, FileName);
                //ref IWorksheet ExcelSheet, string FilePath, string UserName,string Date, string Module, string Details, string UserEmailId
                WriteLogsForInstallation(ref ExcelSheet, FilePath, CurrentDateTime, Message, Status, ExceptionMsg);
            }
            // If file exist directally write on file
            else
            {
                IWorksheet ExcelSheet1 = (IWorksheet)Factory.GetWorkbook(FilePath).Sheets[0];
                WriteLogsForInstallation(ref ExcelSheet1, FilePath, CurrentDateTime, Message, Status, ExceptionMsg);
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
    }

    private static void CreatePatchInstFileHeader(ref IWorksheet excelSheet, string FileName)
    {
        try
        {
            int RowIndex = 0;
            int ColumnIndex = 0;
            List<string> ListHeaderName = new List<string>();
           // ListHeaderName.Add("Name");
            ListHeaderName.Add("Message");
            ListHeaderName.Add("Date");
            ListHeaderName.Add("Status");
            ListHeaderName.Add("ExceptionMsg");
            foreach (string StrHeaderName in ListHeaderName)
            {
                excelSheet.Cells[RowIndex, ColumnIndex].Value = StrHeaderName;
                excelSheet.Cells[RowIndex, ColumnIndex].Borders.Color = System.Drawing.Color.Black; // Sets The Border Color For Header
                excelSheet.Cells[RowIndex, ColumnIndex].Interior.Color = System.Drawing.Color.Gray; // Sets The Background Color For Header
                excelSheet.Cells[RowIndex, ColumnIndex].Font.Bold = true; // Sets Header Font To Bold
                excelSheet.Cells[RowIndex, ColumnIndex].Font.Color = System.Drawing.Color.White; // Sets Header Font Color To White
                excelSheet.Cells[RowIndex, ColumnIndex].Font.Size = 12;
                excelSheet.Cells[RowIndex, ColumnIndex].VerticalAlignment = VAlign.Top;

                SpreadsheetGear.IRange Range = excelSheet.Range[RowIndex, ColumnIndex, 1, ColumnIndex + 1];
                if (StrHeaderName == "ExceptionMsg")
                {
                    Range.ColumnWidth = 80.00;
                }
                else
                {
                    Range.ColumnWidth = 23.00;
                }
                ColumnIndex++;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private static void WriteLogsForInstallation(ref IWorksheet ExcelSheet, string FilePath, string Date, string Message, string Status, string ExceptionMsg)
    {
        try
        {

            int RowIndex = Convert.ToInt32(ExcelSheet.UsedRange.RowCount);
            ExcelSheet.Cells[RowIndex, 0].Value = Message;
            ExcelSheet.Cells[RowIndex, 1].Value = Date;
            ExcelSheet.Cells[RowIndex, 2].Value = Status;
            ExcelSheet.Cells[RowIndex, 3].Value = ExceptionMsg;
            //ExcelSheet.Cells[RowIndex, 4].Value = UserEmailId;
            ExcelSheet.Cells[RowIndex, 0].Borders.Color = System.Drawing.Color.Black;
            ExcelSheet.Cells[RowIndex, 1].Borders.Color = System.Drawing.Color.Black;
            ExcelSheet.Cells[RowIndex, 2].Borders.Color = System.Drawing.Color.Black;
            ExcelSheet.Cells[RowIndex, 3].Borders.Color = System.Drawing.Color.Black;
          //  ExcelSheet.Cells[RowIndex, 4].Borders.Color = System.Drawing.Color.Black;
            ExcelSheet.Name = Constants.PatchConstaints.PatchInstLogFileName;
            ExcelSheet.SaveAs(FilePath, SpreadsheetGear.FileFormat.Excel8);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

}