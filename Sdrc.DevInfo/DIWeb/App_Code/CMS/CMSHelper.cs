using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Text.RegularExpressions;
using System.Data;

/// <summary>
/// This Helper class contains common methods for Creating And Editing CMS content
/// </summary>
public class CMSHelper
{
    public CMSHelper()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// Method to test the get connection object
    /// </summary>
    /// <returns>Connection Object if exist / Null if Failed to connect</returns>
    public DIConnection GetConnectionObject()
    {
        DIConnection ObjDIConnection = null;
        string[] DBConnArr;
        string PortNo = string.Empty;
        string UserName = string.Empty;
        string DBConn = string.Empty;
        string Password = string.Empty;
        string DatabaseName = string.Empty; // Name of Adaptation Database
        string CMSDatabaseName = string.Empty; // Name of CMS Database 
        string DbNId = string.Empty;
        DIConnection RetVal = null;
        try
        {
            // Get Database NID of Default Database
            DbNId = Global.GetDefaultDbNId();
            // If Database NID is null or empty break further execution of code 
            if (string.IsNullOrEmpty(DbNId))
            {
                RetVal = null;
                return RetVal;
            }
            //Get connection details of database
            DBConnArr = Global.GetDbNConnectionDetails(DbNId, string.Empty);
            // If Database returned database details are less than 6 break further execution of code 
            if (DBConnArr.Length < 6)
            {
                RetVal = null;
                return RetVal;
            }
            // get port no 
            PortNo = DBConnArr[3].ToString();
            // Get User Name
            UserName = DBConnArr[5].ToString();
            //Get Decrypted password
            Password = DBConnArr[6].ToString();
            //Get Adaptation Database name
            DatabaseName = DBConnArr[4].ToString();
            //Name of CMS Database
            DatabaseName = DatabaseName + "_CMS";

            try
            {
                ObjDIConnection = new DIConnection(((DIServerType)Convert.ToInt32(DBConnArr[2])), PortNo, string.Empty, DatabaseName, UserName, Password);
                RetVal = ObjDIConnection;
            }
            catch (Exception Ex)
            {
                Global.CreateExceptionString(Ex, null);
                RetVal = ObjDIConnection;
                return RetVal;
            }

        }
        catch (Exception Ex)
        {
            RetVal = null;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    /// Method to test the get connection object
    /// </summary>
    /// <param name="Message">Returns meassge of connection falure of success status</param>
    /// <returns>Connection Object if exist / Null if Failed to connect</returns>
    public DIConnection GetConnectionObject(out string Message)
    {
        DIConnection ObjDIConnection = null;
        string[] DBConnArr;
        string PortNo = string.Empty;
        string UserName = string.Empty;
        string DBConn = string.Empty;
        string Password = string.Empty;
        string DatabaseName = string.Empty; // Name of Adaptation Database
        string CMSDatabaseName = string.Empty; // Name of CMS Database 
        string DbNId = string.Empty;
        DIConnection RetVal = null;
        try
        {
            // Get Database NID of Default Database
            DbNId = Global.GetDefaultDbNId();
            // If Database NID is null or empty break further execution of code 
            if (string.IsNullOrEmpty(DbNId))
            {
                Message = "Unable to get DbNid of current Database";
                RetVal = null;
                return RetVal;
            }
            //Get connection details of database
            DBConnArr = Global.GetDbNConnectionDetails(DbNId, string.Empty);
            // If Database returned database details are less than 6 break further execution of code 
            if (DBConnArr.Length < 6)
            {
                Message = "Unable to get connection details of current database";
                RetVal = null;
                return RetVal;
            }
            // get port no 
            PortNo = DBConnArr[3].ToString();
            // Get User Name
            UserName = DBConnArr[5].ToString();
            //Get Decrypted password
            Password = DBConnArr[6].ToString();
            //Get Adaptation Database name
            DatabaseName = DBConnArr[4].ToString();
            //Name of CMS Database
            DatabaseName = DatabaseName + "_CMS";

            try
            {
                ObjDIConnection = new DIConnection(((DIServerType)Convert.ToInt32(DBConnArr[2])), PortNo, string.Empty, DatabaseName, UserName, Password);
                Message = string.Empty;
                RetVal = ObjDIConnection;
            }
            catch (Exception Ex)
            {
                Global.CreateExceptionString(Ex, null);
                Message = "Error in creating connetion with CMS database: " + DatabaseName;
                RetVal = ObjDIConnection;
                return RetVal;
            }

        }
        catch (Exception Ex)
        {
            RetVal = null;
            Message = "Exception occured in creating connection with database";
            Global.CreateExceptionString(Ex, null);
        }
        Message = string.Empty;
        return RetVal;
    }

    // Create Url From Input String
    public string CreateUrlFromInputString(string InputString)
    {
        string Retval = string.Empty;
        string[] ArrTitle;
        string RandomNoString = string.Empty;
        InputString = this.RemoveSpecialCharacters(InputString);
        Random RandNo = new Random();
        ArrTitle = InputString.Split(' ');
        int Icount = 0;
        //if (ArrTitle.Length > 9)
        //{
        //    while (Icount < 9)
        //    {
        //        if (!string.IsNullOrEmpty(ArrTitle[Icount].ToString().Trim()))
        //        {
        //            if (!string.IsNullOrEmpty(ArrTitle[Icount]))
        //            {
        //                Retval = Retval + ArrTitle[Icount] + "-";
        //            }
        //            Icount++;    
        //        }                
        //    }
        //}
        //else
        //{
        while (Icount < ArrTitle.Length)
        {
            if (!string.IsNullOrEmpty(ArrTitle[Icount]))
            {
                Retval = Retval + ArrTitle[Icount] + "-";
            }
            Icount++;
        }
        // }
        // get random no between 1 to 100
        RandomNoString = RandNo.Next(1, 100).ToString();
        Retval = Retval + RandomNoString;
        
        return Retval;
    }

    // remove special chars from input string
    public string RemoveSpecialCharacters(string InputString)
    {
        Regex r = new Regex("(?:[^a-z0-9- ]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
        return r.Replace(InputString, String.Empty);
    }

    //parse input string time to datetime
    public DateTime GetSqlDataTimeFromInptDate(string InputDate)
    {
        DateTime RetVal = new DateTime();
        DateTime DtTime = new DateTime();
        // check if date in html page is not null or empty
        if (!string.IsNullOrEmpty(InputDate))
        {// remaove &nbsp; from news and replace ' 'e with '-' so that date can be parsed to datetime
            try
            {
                DateTime.TryParse(InputDate, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DtTime);
                if (DtTime.ToString().Contains(DateTime.Now.Date.ToString()))
                {
                    DtTime = DateTime.Now;
                }
                RetVal = DtTime;
            }
            catch (Exception Ex)
            {
                DtTime = DateTime.Now;
                Global.WriteErrorsInLogFolder("Error in try parsing date" + InputDate);
            }

        }
        // if date is empty then use current date
        else
        {
            DtTime = DateTime.Now;
        }
        RetVal = DtTime;
        return RetVal;
    }


    //parse input string time to datetime
    public string RemoveExtraCharsFromDate(string InputDate)
    {
        InputDate = InputDate.Replace(" ", "-").Replace("&nbsp;", "");
        InputDate = InputDate.Replace("---", "-");
        InputDate = InputDate.Replace("--", "-");
        return InputDate;
    }


    //convert date time to sql date time
    public DateTime ConvertDataTimeToSQLDateTime(string InputDate)
    {
        //  DateTime RetVal = new DateTime();
        DateTime RetVal = new DateTime();
        // check if date in html page is not null or empty
        if (!string.IsNullOrEmpty(InputDate))
        {// remaove &nbsp; from news and replace ' 'e with '-' so that date can be parsed to datetime
            try
            {
                DateTime.TryParse(InputDate, out RetVal);
            }
            catch (Exception Ex)
            {
                RetVal = DateTime.Now;
                Global.WriteErrorsInLogFolder("Error in try parsing date" + InputDate);
            }

        }
        // if date is empty then use current date
        else
        {
            RetVal = DateTime.Now;
        }
        return RetVal;
    }

    /// <summary>
    /// Read Html template for description 
    /// </summary>
    /// <param name="KmlTemplatePath">Template source location</param>
    /// <returns>return string containig template if template exist else return empty string</returns>
    public string GetHtmlFromPath(string FileFullPath)
    {
        string RetVal = string.Empty;
        try
        {

            string HtmlTemplatePath = FileFullPath;
            System.Net.WebClient client = new System.Net.WebClient();
            if (System.IO.File.Exists(HtmlTemplatePath))
            {
                String htmlCode = client.DownloadString(HtmlTemplatePath);
                RetVal = htmlCode;
            }
        }
        catch (Exception Ex)
        {
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }

    /// <summary>
    /// This methods check in database if tag exist, then returns tag Nid
    /// else add tag to database and return Nid of Newly created tag 
    /// </summary>
    /// <param name="Tag">Input Tag name</param>
    /// <returns>Tag NId</returns>
    public int CreateAndGetTagNid(string Tag)
    {
        CMSHelper ObjCMSHelper = new CMSHelper();
        string ResultNid = string.Empty;
        int RetVal = 0;
        string DBConnectionStatusMessage = string.Empty;
        List<System.Data.Common.DbParameter> DbParams = null;
        DIConnection ObjDIConnection = null;
        int MaxTagId = -1;
        try
        {

            // Call method to get connection object
            ObjDIConnection = ObjCMSHelper.GetConnectionObject(out DBConnectionStatusMessage);
            // Check if connection object is not null
            if (ObjDIConnection == null)
            {
                RetVal = -1;
                return RetVal;
            }
            // Innitilze DbParams object
            DbParams = new List<System.Data.Common.DbParameter>();
            // create tag parameter
            System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
            Param1.ParameterName = "@Tags";
            Param1.DbType = DbType.String;
            Param1.Value = Tag;
            DbParams.Add(Param1);

            // Execute stored procedure to get tag Nid
            MaxTagId = Convert.ToInt32(ObjDIConnection.ExecuteScalarSqlQuery("sp_CreateAndGetTagNid", CommandType.StoredProcedure, DbParams));
            if (MaxTagId > 0)
            {
                RetVal = MaxTagId;
            }
        }
        catch (Exception Ex)
        {
            RetVal = -1;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            DbParams = null;
            ObjDIConnection = null;
        }
        return RetVal;
    }

}