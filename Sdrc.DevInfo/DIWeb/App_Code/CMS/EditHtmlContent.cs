using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Data;
using System.Data.SqlTypes;

/// <summary>
/// Summary description for EditHtmlContent
/// </summary>
public class EditHtmlContent
{

    public bool EditArticleByArticleId(string requestParam,out string OutCurrentPageNo,out string OutCurrentMenuCategory,out string OutCurrentTagNids)
    {
        bool RetVal = false;
        string[] Params;
        CMSHelper Helper = new CMSHelper();
        string ContentSummary = string.Empty;
        string ContentTitle = string.Empty;
        string ContentDate = string.Empty;
        string ContentImgUrl = string.Empty;
        string UserEmailID = string.Empty;
        string DatabaseLanguage = string.Empty;
        string UserName = string.Empty;
        string CurrentDbLangCode = string.Empty;
        string ArticleTags = string.Empty;
        string MenuCategory = string.Empty;
        string ContentDescription = string.Empty;
        string ContentTag = string.Empty;
        DataContent RetValContent = null;
        int ContentId = 0;
        int ContentTagNid = 0;
        int ExistingTagNid = 0;
        string ContentPdfFileUrl = string.Empty;
        CMSHelper ObjCMSHelper = null;
        SqlDateTime SqlDtTime;
        bool IsContentUpdated = false;
        try
        {
            SqlDtTime = new SqlDateTime();
            ObjCMSHelper = new CMSHelper();
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
            // split Input Params
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);

            // Initlize RetValContent Object
            RetValContent = new DataContent();
            ContentId = Convert.ToInt32(Params[0].ToString());
            ContentTitle = Params[1].ToString();
            ContentDate = Params[2].ToString();
            ContentSummary = Params[3].ToString();
            ContentSummary=ContentSummary.Replace("\n","<br/>");
            ContentDescription = Params[4].ToString().Trim().Replace("diorg/images", "../libraries/aspx/diorg/images");

            //remove changes in script tag for ckeditor
            ContentDescription = ContentDescription.Replace("<scrpt", "<script");
            ContentDescription = ContentDescription.Replace("</scrpt", "</script");

            ContentPdfFileUrl = Params[5].ToString().Replace("diorg/", "../libraries/aspx/diorg/"); ;
            MenuCategory = Params[6].ToString();
            ContentImgUrl = Params[7].ToString().Trim().Replace( "diorg/","../libraries/aspx/diorg/");
            if (Params.Length > 8)
            {
                ContentTag = Params[8].ToString().Trim();
            }
            OutCurrentMenuCategory = MenuCategory;
            OutCurrentPageNo = Params[10].ToString().Trim();

            if (Params.Length > 11)
            {
                OutCurrentTagNids = Params[11].ToString().Trim();
            }
            else
            {
                OutCurrentTagNids = string.Empty;
            }

            ExistingTagNid = Convert.ToInt32(Params[9].ToString().Trim());
            // Check if existing tagid exists
            if (ExistingTagNid > 0)
            {
                // Delete existing Tag from mapping table
                DeleteTagsMappingsByTagId(ExistingTagNid);
            }

            UserEmailID = UserName + " [" + UserEmailID + " ]";

            //// Create Countent Url From Title
            //CountentUrl = ObjCMSHelper.CreateUrlFromInputString(ContentTitle);
            // Getb current database language
            CurrentDbLangCode = Global.GetDefaultLanguageCode();
            
            //Get Tag Nid-- add new tags to database and get nid
            if (!string.IsNullOrEmpty(ContentTag))
            {
                ContentTagNid = ObjCMSHelper.CreateAndGetTagNid(ContentTag);
            }
            //Convert Date to sql date time
            SqlDtTime = ObjCMSHelper.ConvertDataTimeToSQLDateTime(ContentDate);

            // innitlize of members of class Content
            RetValContent.ContentId = ContentId;
            RetValContent.MenuCategory = MenuCategory;
            RetValContent.Date = SqlDtTime;
            RetValContent.DateAdded = new SqlDateTime(DateTime.Now);
            RetValContent.DateModified = new SqlDateTime(DateTime.Now);
            RetValContent.Description = ContentDescription;
            RetValContent.Title = ContentTitle;
            RetValContent.PDFUpload = ContentPdfFileUrl;
            RetValContent.Summary = ContentSummary;
            RetValContent.Thumbnail = ContentImgUrl;
            RetValContent.Archived = false;
            // Username email filed is combination of username and email
            RetValContent.UserNameEmail = UserName + " " + UserEmailID;
            RetValContent.LngCode = CurrentDbLangCode;
            RetValContent.ArticleTagID = ContentTagNid;

          IsContentUpdated= this.UpdateCMSContent(RetValContent);
          if (IsContentUpdated)
          {
              RetVal = true;
              // set session varibles to maintain page no
          }
        }
        catch (Exception Ex)
        {
            OutCurrentMenuCategory = string.Empty;
            OutCurrentPageNo = string.Empty;
            OutCurrentTagNids = string.Empty;
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }

        finally
        {
            RetValContent = null;
        }
        return RetVal;
    }

    // Update existing content 
    private bool UpdateCMSContent(DataContent CMSDataContent)
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
            Param1.ParameterName = "@ContentId";
            Param1.DbType = DbType.Int32;
            Param1.Value = CMSDataContent.ContentId;
            DbParams.Add(Param1);

            // create database parameters
            System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();
            Param2.ParameterName = "@MenuCategory";
            Param2.DbType = DbType.String;
            Param2.Value = CMSDataContent.MenuCategory;
            DbParams.Add(Param2);

            System.Data.Common.DbParameter Param3 = ObjDIConnection.CreateDBParameter();
            Param3.ParameterName = "@Title";
            Param3.DbType = DbType.String;
            Param3.Value = CMSDataContent.Title;
            DbParams.Add(Param3);

            System.Data.Common.DbParameter Param4 = ObjDIConnection.CreateDBParameter();
            Param4.ParameterName = "@Date";
            Param4.DbType = DbType.DateTime;
            Param4.Value = CMSDataContent.Date;
            DbParams.Add(Param4);

            System.Data.Common.DbParameter Param5 = ObjDIConnection.CreateDBParameter();
            Param5.ParameterName = "@Thumbnail";
            Param5.DbType = DbType.String;
            Param5.Value = CMSDataContent.Thumbnail;
            DbParams.Add(Param5);

            System.Data.Common.DbParameter Param6 = ObjDIConnection.CreateDBParameter();
            Param6.ParameterName = "@Summary";
            Param6.DbType = DbType.String;
            Param6.Value = CMSDataContent.Summary;
            DbParams.Add(Param6);

            System.Data.Common.DbParameter Param7 = ObjDIConnection.CreateDBParameter();
            Param7.ParameterName = "@Description";
            Param7.DbType = DbType.String;
            Param7.Value = CMSDataContent.Description;
            DbParams.Add(Param7);

            System.Data.Common.DbParameter Param8 = ObjDIConnection.CreateDBParameter();
            Param8.ParameterName = "@PDFUpload";
            Param8.DbType = DbType.String;
            Param8.Value = CMSDataContent.PDFUpload;
            DbParams.Add(Param8);

            System.Data.Common.DbParameter Param9 = ObjDIConnection.CreateDBParameter();
            Param9.ParameterName = "@DateAdded";
            Param9.DbType = DbType.DateTime;
            Param9.Value = CMSDataContent.DateAdded;
            DbParams.Add(Param9);

            System.Data.Common.DbParameter Param10 = ObjDIConnection.CreateDBParameter();
            Param10.ParameterName = "@DateModified";
            Param10.DbType = DbType.DateTime;
            Param10.Value = CMSDataContent.DateModified;
            DbParams.Add(Param10);

            System.Data.Common.DbParameter Param11 = ObjDIConnection.CreateDBParameter();
            Param11.ParameterName = "@Archived";
            Param11.DbType = DbType.Boolean;
            Param11.Value = CMSDataContent.Archived;
            DbParams.Add(Param11);

            System.Data.Common.DbParameter Param12 = ObjDIConnection.CreateDBParameter();
            Param12.ParameterName = "@ArticleTagId";
            Param12.DbType = DbType.Int32;
            Param12.Value = CMSDataContent.ArticleTagID;
            DbParams.Add(Param12);

            System.Data.Common.DbParameter Param13 = ObjDIConnection.CreateDBParameter();
            Param13.ParameterName = "@UserNameEmail";
            Param13.DbType = DbType.String;
            Param13.Value = CMSDataContent.UserNameEmail;
            DbParams.Add(Param13);


            //System.Data.Common.DbParameter Param14 = ObjDIConnection.CreateDBParameter();
            //Param14.ParameterName = "@URL";
            //Param14.DbType = DbType.String;
            //Param14.Value = CMSDataContent.URL;
            //DbParams.Add(Param14);

            System.Data.Common.DbParameter Param15 = ObjDIConnection.CreateDBParameter();
            Param15.ParameterName = "@LngCode";
            Param15.DbType = DbType.String;
            Param15.Value = CMSDataContent.LngCode;
            DbParams.Add(Param15);

            System.Data.Common.DbParameter Param16 = ObjDIConnection.CreateDBParameter();
            Param16.ParameterName = "@Fld1";
            Param16.DbType = DbType.String;
            Param16.Value = "";
            DbParams.Add(Param16);
            System.Data.Common.DbParameter Param17 = ObjDIConnection.CreateDBParameter();
            Param17.ParameterName = "@Fld2";
            Param17.DbType = DbType.String;
            Param17.Value = "";
            DbParams.Add(Param17);
            System.Data.Common.DbParameter Param18 = ObjDIConnection.CreateDBParameter();
            Param18.ParameterName = "@Fld3";
            Param18.DbType = DbType.String;
            Param18.Value = "";
            DbParams.Add(Param18);
            System.Data.Common.DbParameter Param19 = ObjDIConnection.CreateDBParameter();
            Param19.ParameterName = "@Fld4";
            Param19.DbType = DbType.String;
            Param19.Value = "";
            DbParams.Add(Param19);
            System.Data.Common.DbParameter Param20 = ObjDIConnection.CreateDBParameter();
            Param20.ParameterName = "@Fld5";
            Param20.DbType = DbType.String;
            Param20.Value = "";
            DbParams.Add(Param20);
            System.Data.Common.DbParameter Param21 = ObjDIConnection.CreateDBParameter();
            Param21.ParameterName = "@Fld6";
            Param21.DbType = DbType.String;
            Param21.Value = "";
            DbParams.Add(Param21);

            System.Data.Common.DbParameter Param22 = ObjDIConnection.CreateDBParameter();
            Param22.ParameterName = "@Fld1Text";
            Param22.DbType = DbType.String;
            Param22.Value = "";
            DbParams.Add(Param22);
            System.Data.Common.DbParameter Param23 = ObjDIConnection.CreateDBParameter();
            Param23.ParameterName = "@Fld2Text";
            Param23.DbType = DbType.String;
            Param23.Value = "";
            DbParams.Add(Param23);
            System.Data.Common.DbParameter Param24 = ObjDIConnection.CreateDBParameter();
            Param24.ParameterName = "@Fld3Text";
            Param24.DbType = DbType.String;
            Param24.Value = "";
            DbParams.Add(Param24);
            System.Data.Common.DbParameter Param25 = ObjDIConnection.CreateDBParameter();
            Param25.ParameterName = "@Fld4Text";
            Param25.DbType = DbType.String;
            Param25.Value = "";
            DbParams.Add(Param25);

            System.Data.Common.DbParameter Param26 = ObjDIConnection.CreateDBParameter();
            Param26.ParameterName = "@Fld5Text";
            Param26.DbType = DbType.String;
            Param26.Value = "";
            DbParams.Add(Param26);

            System.Data.Common.DbParameter Param27 = ObjDIConnection.CreateDBParameter();
            Param27.ParameterName = "@Fld6Text";
            Param27.DbType = DbType.String;
            Param27.Value = "";
            DbParams.Add(Param27);

            // Execute stored procedure to Update CMS Content In Database
            DtAddCmsCont = ObjDIConnection.ExecuteNonQuery("sp_UpdateCMSContentByContentId", CommandType.StoredProcedure, DbParams);
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

    public bool DeleteTagsMappingsByTagId(int Tag)
    {
        CMSHelper ObjCMSHelper = new CMSHelper();
        string ResultNid = string.Empty;
        bool RetVal = false;
        List<System.Data.Common.DbParameter> DbParams = null;
        DIConnection ObjDIConnection = null;
        int RetDelTagId = -1;
        try
        {

            // Call method to get connection object
            ObjDIConnection = ObjCMSHelper.GetConnectionObject();
            // Check if connection object is not null
            if (ObjDIConnection == null)
            {
                RetVal = false;
                return RetVal;
            }
            // Innitilze DbParams object
            DbParams = new List<System.Data.Common.DbParameter>();
            // create tag parameter
            System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
            Param1.ParameterName = "@TagId";
            Param1.DbType = DbType.Int32;
            Param1.Value = Tag;
            DbParams.Add(Param1);

            // Execute stored procedure to get tag Nid
            RetDelTagId = Convert.ToInt32(ObjDIConnection.ExecuteScalarSqlQuery("sp_DeleteTagsMappingsByTagId", CommandType.StoredProcedure, DbParams));
            if (RetDelTagId > 0)
            {
                RetVal = true;
            }
        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            DbParams = null;
            ObjDIConnection = null;
        }
        return RetVal;
    }

    public bool DeleteArticlebyContentId(int ContentId)
    {
        CMSHelper ObjCMSHelper = new CMSHelper();
        string ResultNid = string.Empty;
        bool RetVal = false;
        List<System.Data.Common.DbParameter> DbParams = null;
        DIConnection ObjDIConnection = null;
        int RetDelTagId = -1;
        try
        {

            // Call method to get connection object
            ObjDIConnection = ObjCMSHelper.GetConnectionObject();
            // Check if connection object is not null
            if (ObjDIConnection == null)
            {
                RetVal = false;
                return RetVal;
            }
            // Innitilze DbParams object
            DbParams = new List<System.Data.Common.DbParameter>();
            // create tag parameter
            System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
            Param1.ParameterName = "@ContentId";
            Param1.DbType = DbType.Int32;
            Param1.Value = ContentId;
            DbParams.Add(Param1);

            // Execute stored procedure to get tag Nid
            RetDelTagId = Convert.ToInt32(ObjDIConnection.ExecuteNonQuery("sp_DeleteArticleByContentId", CommandType.StoredProcedure, DbParams));
            if (RetDelTagId > 0)
            {
                RetVal = true;
            }
        }
        catch (Exception Ex)
        {
            RetVal = false;
            Global.CreateExceptionString(Ex, null);
        }
        finally
        {
            DbParams = null;
            ObjDIConnection = null;
        }
        return RetVal;
    }

    public bool ShowHideArticlebyContentId(int ContentId, bool IsHidden)
    {
        CMSHelper ObjCMSHelper = new CMSHelper();
        string ResultNid = string.Empty;
        bool RetVal = false;
        List<System.Data.Common.DbParameter> DbParams = null;
        DIConnection ObjDIConnection = null;
        int RetDelTagId = -1;
        try
        {

            // Call method to get connection object
            ObjDIConnection = ObjCMSHelper.GetConnectionObject();
            // Check if connection object is not null
            if (ObjDIConnection == null)
            {
                RetVal = false;
                return RetVal;
            }
            // Innitilze DbParams object
            DbParams = new List<System.Data.Common.DbParameter>();
            System.Data.Common.DbParameter Param1 = ObjDIConnection.CreateDBParameter();
            Param1.ParameterName = "@ContentId";
            Param1.DbType = DbType.Int32;
            Param1.Value = ContentId;
            DbParams.Add(Param1);

            System.Data.Common.DbParameter Param2 = ObjDIConnection.CreateDBParameter();
            Param2.ParameterName = "@IsHidden";
            Param2.DbType = DbType.Boolean;
            Param2.Value = IsHidden;
            DbParams.Add(Param2);


            // Execute stored procedure to get tag Nid
            RetDelTagId = Convert.ToInt32(ObjDIConnection.ExecuteNonQuery("sp_ShowHideArticlesByContentId", CommandType.StoredProcedure, DbParams));
            if (RetDelTagId > 0)
            {
                RetVal = true;
            }
        }
        catch (Exception Ex)
        {
            RetVal = false;
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