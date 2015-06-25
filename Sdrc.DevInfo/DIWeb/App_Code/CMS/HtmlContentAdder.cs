using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlTypes;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Data;
using HtmlAgilityPack;

/// <summary>
/// This Class in used for Adding new content to CMSDatabase
/// </summary>
public class HtmlContentAdder
{   

    //Create Datacontent Object from Input Params
    private DataContent CreateCMSDataContent(string requestParam)
    {
        CMSHelper ObjCMSHelper = new CMSHelper();
        DataContent RetValContent = null;
        string PdfFileUrl = string.Empty;

        string ContentSummary = string.Empty;
        string ContentTitle = string.Empty;
        string ContentDate = string.Empty;
        string ContentImgUrl = string.Empty;
        string ContentPageUrl = string.Empty;
        string UserEmailID = string.Empty;
        string DatabaseLanguage = string.Empty;
        string UserName = string.Empty;
        string CurrentDbLangCode = string.Empty;
        string CountentUrl = string.Empty;
        string ArticleTags = string.Empty;
        string MenuCategory = string.Empty;
        string ContentDescription = string.Empty;
        string ContentTag = string.Empty;
        int ContentTagNid = -1;
        SqlDateTime SqlDtTime = new SqlDateTime();
        string[] Params;
        try
        {
            // split Input Params
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            // Check if session is having nid for loggedin Admin
            if (!string.IsNullOrEmpty(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUserNId].ToString()))
            {// get user email
                UserEmailID = Global.Get_User_EmailId_ByAdaptationURL(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUserNId].ToString());
            }
            // Check if session is having username for loggedin Admin
            if (!string.IsNullOrEmpty(HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser].ToString()))
            { // get user name from session
                UserName = HttpContext.Current.Session[Constants.SessionNames.LoggedAdminUser].ToString();
            }

            // Initlize RetValContent Object
            RetValContent = new DataContent();
            ContentTitle = Params[0].ToString();
            ContentDate = Params[1].ToString();
            ContentSummary = Params[2].ToString();
            ContentSummary = ContentSummary.Replace("\n", "<br/>");
            ContentDescription = Params[3].ToString().Replace("diorg/images", "../libraries/aspx/diorg/images");
            PdfFileUrl = Params[4].ToString().Replace("diorg/", "../libraries/aspx/diorg/");
            MenuCategory = Params[5].ToString();
            ContentImgUrl = Params[6].ToString().Trim().Replace("diorg/","../libraries/aspx/diorg/");
            if (Params.Length > 7)
            {
                ContentTag = Params[7].ToString().Trim();
            }
            UserEmailID = UserName + " [" + UserEmailID + " ]";

            // Create Countent Url From Title
            CountentUrl = ObjCMSHelper.CreateUrlFromInputString(ContentTitle);
            // Getb current database language
            CurrentDbLangCode = Global.GetDefaultLanguageCode();
            //Get Tag Nid
            if (!string.IsNullOrEmpty(ContentTag))
            {
                ContentTagNid = ObjCMSHelper.CreateAndGetTagNid(ContentTag.Trim());
            }
            if(string.IsNullOrEmpty(ContentDate) && MenuCategory.ToLower()!=EnumHelper.PageName.News.ToString().ToLower())
            {

                SqlDtTime=DateTime.Now.AddYears(-100);
            }
            else
            {
            //Convert Date to sql date time
            SqlDtTime = ObjCMSHelper.GetSqlDataTimeFromInptDate(ContentDate);
            }
            // innitlize of members of class Content
            RetValContent.MenuCategory = MenuCategory;
            RetValContent.Date = SqlDtTime;
            RetValContent.DateAdded = new SqlDateTime(DateTime.Now);
            RetValContent.DateModified = new SqlDateTime(DateTime.Now);
            RetValContent.Description = ContentDescription;
            RetValContent.Title = ContentTitle;
            RetValContent.PDFUpload = PdfFileUrl;
            RetValContent.Summary = ContentSummary;
            RetValContent.Thumbnail = ContentImgUrl;
            RetValContent.URL = CountentUrl;
            RetValContent.Archived = false;
            // Username email filed is combination of username and email
            RetValContent.UserNameEmail = UserName + " " + UserEmailID;
            RetValContent.LngCode = CurrentDbLangCode;
            RetValContent.ArticleTagID = ContentTagNid;
            RetValContent.IsDeleted = false;
            RetValContent.IsHidden = false;

            //Extra fields to be used in future
            RetValContent.Fld1 = string.Empty;
            RetValContent.Fld2 = string.Empty;
            RetValContent.Fld3 = string.Empty;
            RetValContent.Fld4 = string.Empty;
            RetValContent.Fld5 = string.Empty;
            RetValContent.Fld6 = string.Empty;
            RetValContent.Fld1Text = string.Empty;
            RetValContent.Fld2Text = string.Empty;
            RetValContent.Fld3Text = string.Empty;
            RetValContent.Fld4Text = string.Empty;
            RetValContent.Fld5Text = string.Empty;
            RetValContent.Fld6Text = string.Empty;
        }
        catch (Exception Ex)
        {
            RetValContent = null;
            Global.CreateExceptionString(Ex, null);
        }
        // return news object 
        return RetValContent;
    }

    // Create new content page andd add link on main html page
    private bool AddCMSContentToDB(DataContent CMSDataContent)
    {
        bool RetVal = false;
        string ConnectionMessage = string.Empty;
        List<System.Data.Common.DbParameter> DbParams = new List<System.Data.Common.DbParameter>();
        DIConnection ObjDIConnection = null;
        int DtAddCmsCont = -1;
        try
        {
            CMSHelper ObjCMSHelper = new CMSHelper();
            // call method to get connection object
            ObjDIConnection = ObjCMSHelper.GetConnectionObject(out ConnectionMessage);

            // create database parameters
            System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
            Param1.ParameterName = "@MenuCategory";
            Param1.DbType = DbType.String;
            Param1.Value = CMSDataContent.MenuCategory;
            DbParams.Add(Param1);

            System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();
            Param2.ParameterName = "@Title";
            Param2.DbType = DbType.String;
            Param2.Value = CMSDataContent.Title;
            DbParams.Add(Param2);

            System.Data.Common.DbParameter Param3 = ObjDIConnection.CreateDBParameter();
            Param3.ParameterName = "@Date";
            Param3.DbType = DbType.DateTime;
            Param3.Value = CMSDataContent.Date;
            DbParams.Add(Param3);

            System.Data.Common.DbParameter Param4 = ObjDIConnection.CreateDBParameter();
            Param4.ParameterName = "@Thumbnail";
            Param4.DbType = DbType.String;
            Param4.Value = CMSDataContent.Thumbnail;
            DbParams.Add(Param4);

            System.Data.Common.DbParameter Param5 = ObjDIConnection.CreateDBParameter();
            Param5.ParameterName = "@Summary";
            Param5.DbType = DbType.String;
            Param5.Value = CMSDataContent.Summary;
            DbParams.Add(Param5);

            System.Data.Common.DbParameter Param6 = ObjDIConnection.CreateDBParameter();
            Param6.ParameterName = "@Description";
            Param6.DbType = DbType.String;
            Param6.Value = CMSDataContent.Description;
            DbParams.Add(Param6);

            System.Data.Common.DbParameter Param7 = ObjDIConnection.CreateDBParameter();
            Param7.ParameterName = "@PDFUpload";
            Param7.DbType = DbType.String;
            Param7.Value = CMSDataContent.PDFUpload;
            DbParams.Add(Param7);

            System.Data.Common.DbParameter Param8 = ObjDIConnection.CreateDBParameter();
            Param8.ParameterName = "@DateAdded";
            Param8.DbType = DbType.DateTime;
            Param8.Value = CMSDataContent.DateAdded;
            DbParams.Add(Param8);

            System.Data.Common.DbParameter Param9 = ObjDIConnection.CreateDBParameter();
            Param9.ParameterName = "@DateModified";
            Param9.DbType = DbType.DateTime;
            Param9.Value = CMSDataContent.DateModified;
            DbParams.Add(Param9);

            System.Data.Common.DbParameter Param10 = ObjDIConnection.CreateDBParameter();
            Param10.ParameterName = "@Archived";
            Param10.DbType = DbType.Boolean;
            Param10.Value = CMSDataContent.Archived;
            DbParams.Add(Param10);

            System.Data.Common.DbParameter Param11 = ObjDIConnection.CreateDBParameter();
            Param11.ParameterName = "@ArticleTagId";
            Param11.DbType = DbType.Int32;
            Param11.Value = CMSDataContent.ArticleTagID;
            DbParams.Add(Param11);

            System.Data.Common.DbParameter Param12 = ObjDIConnection.CreateDBParameter();
            Param12.ParameterName = "@UserNameEmail";
            Param12.DbType = DbType.String;
            Param12.Value = CMSDataContent.UserNameEmail;
            DbParams.Add(Param12);


            System.Data.Common.DbParameter Param14 = ObjDIConnection.CreateDBParameter();
            Param14.ParameterName = "@URL";
            Param14.DbType = DbType.String;
            Param14.Value = CMSDataContent.URL;
            DbParams.Add(Param14);

            System.Data.Common.DbParameter Param15 = ObjDIConnection.CreateDBParameter();
            Param15.ParameterName = "@LngCode";
            Param15.DbType = DbType.String;
            Param15.Value = CMSDataContent.LngCode;
            DbParams.Add(Param15);

            System.Data.Common.DbParameter Param16 = ObjDIConnection.CreateDBParameter();
            Param16.ParameterName = "@IsDeleted";
            Param16.DbType = DbType.Boolean;
            Param16.Value =CMSDataContent.IsDeleted;
            DbParams.Add(Param16);

            System.Data.Common.DbParameter Param17 = ObjDIConnection.CreateDBParameter();
            Param17.ParameterName = "@IsHidden";
            Param17.DbType = DbType.Boolean;
            Param17.Value = CMSDataContent.IsHidden;
            DbParams.Add(Param17);

            System.Data.Common.DbParameter Param18 = ObjDIConnection.CreateDBParameter();
            Param18.ParameterName = "@Fld1";
            Param18.DbType = DbType.String;
            Param18.Value = "";
            DbParams.Add(Param18);

            System.Data.Common.DbParameter Param19 = ObjDIConnection.CreateDBParameter();
            Param19.ParameterName = "@Fld2";
            Param19.DbType = DbType.String;
            Param19.Value = "";
            DbParams.Add(Param19);

            System.Data.Common.DbParameter Param20 = ObjDIConnection.CreateDBParameter();
            Param20.ParameterName = "@Fld3";
            Param20.DbType = DbType.String;
            Param20.Value = "";
            DbParams.Add(Param20);

            System.Data.Common.DbParameter Param21 = ObjDIConnection.CreateDBParameter();
            Param21.ParameterName = "@Fld4";
            Param21.DbType = DbType.String;
            Param21.Value = "";
            DbParams.Add(Param21);

            System.Data.Common.DbParameter Param22 = ObjDIConnection.CreateDBParameter();
            Param22.ParameterName = "@Fld5";
            Param22.DbType = DbType.String;
            Param22.Value = "";
            DbParams.Add(Param22);
            System.Data.Common.DbParameter Param23 = ObjDIConnection.CreateDBParameter();
            Param23.ParameterName = "@Fld6";
            Param23.DbType = DbType.String;
            Param23.Value = "";
            DbParams.Add(Param23);

            System.Data.Common.DbParameter Param24 = ObjDIConnection.CreateDBParameter();
            Param24.ParameterName = "@Fld1Text";
            Param24.DbType = DbType.String;
            Param24.Value = "";
            DbParams.Add(Param24);
            System.Data.Common.DbParameter Param25 = ObjDIConnection.CreateDBParameter();
            Param25.ParameterName = "@Fld2Text";
            Param25.DbType = DbType.String;
            Param25.Value = "";
            DbParams.Add(Param25);
            System.Data.Common.DbParameter Param26 = ObjDIConnection.CreateDBParameter();
            Param26.ParameterName = "@Fld3Text";
            Param26.DbType = DbType.String;
            Param26.Value = "";
            DbParams.Add(Param26);
            System.Data.Common.DbParameter Param27= ObjDIConnection.CreateDBParameter();
            Param27.ParameterName = "@Fld4Text";
            Param27.DbType = DbType.String;
            Param27.Value = "";
            DbParams.Add(Param27);

            System.Data.Common.DbParameter Param28 = ObjDIConnection.CreateDBParameter();
            Param28.ParameterName = "@Fld5Text";
            Param28.DbType = DbType.String;
            Param28.Value = "";
            DbParams.Add(Param28);

            System.Data.Common.DbParameter Param29 = ObjDIConnection.CreateDBParameter();
            Param29.ParameterName = "@Fld6Text";
            Param29.DbType = DbType.String;
            Param29.Value = "";
            DbParams.Add(Param29);

            // Execute stored procedure to Insert CMS Content In Database
            DtAddCmsCont = ObjDIConnection.ExecuteNonQuery("sp_AddCMSContent", CommandType.StoredProcedure, DbParams);
            //To check if records inserted in database,check if count of no of rows effected is greater than 0
            if (DtAddCmsCont > 0)
            {
                RetVal = true;
            }
            // return false
            else
            {
                RetVal = false;
            }
        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }




    /// <summary>
    /// This method add CMS content to database
    /// </summary>
    /// <param name="requestParam">Input Parem containing inputs for cms</param>
    /// <returns>true if new added successfully , else return false</returns>
    public bool CreateCmsContent(string requestParam)
    {
        bool RetVal = false;
        bool IsCmsContentAddedToDb = false;
        DataContent ObjDataCont = new DataContent();
        string MenuCategory = string.Empty;

        try
        {
            // create news object, to be inserted in database
            ObjDataCont = this.CreateCMSDataContent(requestParam);
            if (ObjDataCont != null)
            {
                // call method to add news to database
                IsCmsContentAddedToDb = this.AddCMSContentToDB(ObjDataCont);
                // check if content added to database successfully, set return value to true
                if (IsCmsContentAddedToDb)
                {
                    RetVal = true;
                }
                else
                {
                    RetVal = false;
                }
            }
            else
            {
                RetVal = false;
            }
        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }

        return RetVal;
    }

    private string GetTextFromHtml(string InputHtmlString)
    {
        string RetVal = string.Empty;
        HtmlDocument document = null;

        document = new HtmlDocument();
        // load html page
        document.Load(InputHtmlString);
       
        return RetVal;
    }

}