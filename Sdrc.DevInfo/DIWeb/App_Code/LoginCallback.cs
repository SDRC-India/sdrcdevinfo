using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DevInfo.Lib.DI_LibDAL.Connection;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using iTextSharp.text;
using DevInfo.Lib.DI_LibSDMX;
using System.Text;
using System.Security.Cryptography;
using SDMXObjectModel.Message;
using SDMXObjectModel.Registry;
using SDMXObjectModel.Common;
using SDMXObjectModel;
using DI7.Lib.BAL.HTMLGenerator;

public partial class Callback : System.Web.UI.Page
{
    #region "--Variables--"

    diworldwide_userinfo.UserLoginInformation GlobalUserWebService = new diworldwide_userinfo.UserLoginInformation();

    string AdaptationUrl = string.Empty;

    #endregion "--Variables--"

    #region "--Methods--"

    #region "--Private--"

    #region "--Common--"

    private bool Is_Existing_User(string EmailId)
    {

        bool RetVal;
        DIConnection DIConnection;
        string Query;
        DataTable DtUser;

        RetVal = false;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);
            Query = "SELECT * FROM Users WHERE User_Email_Id = '" + EmailId + "';";
            DtUser = DIConnection.ExecuteDataTable(Query);

            if (DtUser != null && DtUser.Rows.Count > 0)
            {
                RetVal = true;
            }
            else
            {
                RetVal = false;
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private void Save_In_User_Table(string EmailId, string Password, string FirstName, string LastName, string Country, bool IsDataProvider, bool IsAdmin, bool SendDevInfoUpdates)
    {
        string InsertQuery;
        DIConnection DIConnection;

        InsertQuery = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);

            InsertQuery = "INSERT INTO Users (User_Email_Id, User_Password, User_First_Name, User_Last_Name, User_Country, User_Is_Provider, User_Is_Admin, User_Send_Updates) VALUES('" + EmailId + "','" + Password + "','" + FirstName + "','" + LastName + "','" + Country + "','" + IsDataProvider.ToString() + "','" + IsAdmin.ToString() + "','" + SendDevInfoUpdates.ToString() + "');";

            DIConnection.ExecuteDataTable(InsertQuery);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    private string Save_In_TokenInformation(string UserNid, bool IsRegistration, bool isDataview)
    {
        string Query;
        DIConnection DIConnection;

        Query = string.Empty;
        DIConnection = null;
        string TokeyKey = string.Empty;
        string TimeStamp = string.Empty;
        DataTable TokeInfoTable = null;
        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);

            Query = "Select * from TokenInformation where User_Nid = " + UserNid + " and IsRegistration = " + IsRegistration + " and IsDataviewFlow=" + isDataview + ";";
            TokeInfoTable = DIConnection.ExecuteDataTable(Query);
            if (TokeInfoTable != null && TokeInfoTable.Rows.Count > 0)
            {
                Query = "delete from TokenInformation where User_Nid = " + UserNid + " and IsRegistration = " + IsRegistration + " and IsDataviewFlow=" + isDataview;
                DIConnection.ExecuteDataTable(Query);
            }
            if (TokeInfoTable != null)
            {
                TokeyKey = Guid.NewGuid().ToString();
                TimeStamp = DateTime.Now.ToString();
                Query = "INSERT INTO TokenInformation (User_Nid, CreatedTime, TokenKey, IsRegistration, IsDataviewFlow) VALUES(" + UserNid + ",'" + TimeStamp + "','" + TokeyKey + "'," + IsRegistration + "," + isDataview + ");";
                DIConnection.ExecuteDataTable(Query);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
        return TokeyKey;
    }

    private void Update_In_User_Table(string UserNId, string EmailId, string Password, string FirstName, string LastName, string Country, bool IsDataProvider, bool IsAdmin, bool SendDevInfoUpdates)
    {
        string UpdateQuery;
        DIConnection DIConnection;

        UpdateQuery = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            if (!string.IsNullOrEmpty(Password))
            {
                UpdateQuery = "UPDATE Users SET User_Email_Id = '" + EmailId + "',User_Password='" + Password + "', User_First_Name = '" + FirstName + "', User_Last_Name = '" + LastName + "', User_Country = '" + Country + "', User_Is_Provider = '" + IsDataProvider.ToString() + "', User_Is_Admin = '" + IsAdmin.ToString() + "', User_Send_Updates = '" + SendDevInfoUpdates.ToString() + "' WHERE NId = " + UserNId + ";";
            }
            else
            {
                UpdateQuery = "UPDATE Users SET User_Email_Id = '" + EmailId + "',User_First_Name = '" + FirstName + "', User_Last_Name = '" + LastName + "', User_Country = '" + Country + "', User_Is_Provider = '" + IsDataProvider.ToString() + "', User_Is_Admin = '" + IsAdmin.ToString() + "', User_Send_Updates = '" + SendDevInfoUpdates.ToString() + "' WHERE NId = " + UserNId + ";";
            }
            DIConnection.ExecuteDataTable(UpdateQuery);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    private void Delete_In_User_Table(int UserNId)
    {
        string DeleteQuery;
        DIConnection DIConnection;

        DeleteQuery = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);

            DeleteQuery = "DELETE FROM Users WHERE NId = " + UserNId.ToString() + ";";

            DIConnection.ExecuteDataTable(DeleteQuery);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    private DataTable Get_User(string EmailId, string Password, bool isAdmin)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        string Query;

        RetVal = null;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);

            Query = "SELECT * FROM Users WHERE User_Email_Id = '" + EmailId + "' AND User_Password = '" + Password + "'";

            if (isAdmin)
            {
                Query += " And User_Is_Admin = 'True'";
            }

            Query += ";";


            RetVal = DIConnection.ExecuteDataTable(Query);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private DataTable Get_User(string EmailId)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        string Query;

        RetVal = null;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);
            Query = "SELECT * FROM Users WHERE User_Email_Id = '" + EmailId + "';";
            RetVal = DIConnection.ExecuteDataTable(Query);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private DataTable Get_User(int UserNId)
    {
        DataTable RetVal;
        DIConnection DIConnection;
        string Query;

        RetVal = null;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);
            Query = "SELECT * FROM Users WHERE NId = " + UserNId.ToString() + ";";
            RetVal = DIConnection.ExecuteDataTable(Query);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private void Create_MaintenanceAgency_ForAdmin(string userNId, string name, string language)
    {
        string MAId, FolderPath, FileNameWPath;

        MAId = DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + userNId;
        FolderPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users);
        FileNameWPath = Path.Combine(FolderPath, DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.FileName);

        if (File.Exists(FileNameWPath))
        {
            SDMXUtility.Register_User(SDMXSchemaType.Two_One, FileNameWPath, UserTypes.Agency, MAId, name, language, string.Empty);
        }
        else
        {
            SDMXUtility.Register_User(SDMXSchemaType.Two_One, string.Empty, UserTypes.Agency, MAId, name, language, FolderPath);
        }
    }

    private void Frame_Message_And_Send_Mail(string FirstName, string EmailId, string UserNId, bool IsAdmin, bool IsDataProvider, string Language)
    {
        string MessageContent = string.Empty;
        string Subject = string.Empty;
        string Body = string.Empty;
        string TamplatePath = string.Empty;
        TamplatePath = Path.Combine(this.Page.Request.PhysicalApplicationPath, Constants.FolderName.EmailTemplates);
        if (IsAdmin == true)
        {
            TamplatePath += Language + "\\" + Constants.FileName.AdminRegistration;
            MessageContent = GetEmailTamplate(TamplatePath);
            Subject = MessageContent.Split("\r\n".ToCharArray())[0].ToString();
            Subject = Subject.Replace("[^^^^]", "");
            Subject = Subject.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
            Body = MessageContent.Replace(MessageContent.Split("\r\n".ToCharArray())[0], "");
            Body = Body.Replace("[****]USER_NAME[****]", FirstName);
            Body = Body.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
            Body = Body.Replace("[****]ADAPTATION_URL[****]", this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("libraries")));
            Body = Body.Replace("[****]USER_EMAILID[****]", EmailId);
            Body = Body.Replace("[****]DATA_PROVIDER_ID[****]", DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId);
            Body = Body.Replace("[****]AGENCY_ID[****]", DevInfo.Lib.DI_LibSDMX.Constants.MaintenanceAgencyScheme.Prefix + UserNId);
            Body = Body.Replace("[****]EMAILID_DB_ADMIN[****]", Global.DbAdmEmail);
        }
        else if (IsDataProvider == true)
        {
            TamplatePath += Language + "\\" + Constants.FileName.DataProviderRegistration;
            MessageContent = GetEmailTamplate(TamplatePath);
            Subject = MessageContent.Split("\r\n".ToCharArray())[0].ToString();
            Subject = Subject.Replace("[^^^^]", "");
            Subject = Subject.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
            Body = MessageContent.Replace(MessageContent.Split("\r\n".ToCharArray())[0], "");
            Body = Body.Replace("[****]USER_NAME[****]", FirstName);
            Body = Body.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
            Body = Body.Replace("[****]ADAPTATION_URL[****]", this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("libraries")));
            Body = Body.Replace("[****]USER_EMAILID[****]", EmailId);
            Body = Body.Replace("[****]DATA_PROVIDER_ID[****]", DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId);
            Body = Body.Replace("[****]EMAILID_DB_ADMIN[****]", Global.DbAdmEmail);
        }
        else
        {
            TamplatePath += Language + "\\" + Constants.FileName.DataConsumerRegistration;
            MessageContent = GetEmailTamplate(TamplatePath);
            Subject = MessageContent.Split("\r\n".ToCharArray())[0].ToString();
            Subject = Subject.Replace("[^^^^]", "");
            Subject = Subject.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
            Body = MessageContent.Replace(MessageContent.Split("\r\n".ToCharArray())[0], "");
            Body = Body.Replace("[****]USER_NAME[****]", FirstName);
            Body = Body.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
            Body = Body.Replace("[****]ADAPTATION_URL[****]", this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("libraries")));
            Body = Body.Replace("[****]USER_EMAILID[****]", EmailId);
            Body = Body.Replace("[****]DATA_CONSUMER_ID[****]", DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId);
            Body = Body.Replace("[****]EMAILID_DB_ADMIN[****]", Global.DbAdmEmail);
        }
        this.Send_Email(Global.adaptation_name + " - WebMaster", "no-reply@dataforall.org", EmailId, Subject, Body, true, FirstName);        
        //this.Send_Email(Global.adaptation_name + " - WebMaster", ConfigurationManager.AppSettings["NotificationSenderEmailId"].ToString(), EmailId, Subject, Body, true, FirstName);
    }

    #region "Sending token message"
    private void Frame_TokenMessage_And_Send_Mail(string FirstName, string EmailId, bool IsAdmin, string TokenKey, bool ActivationFlow, string Language, bool isDataView, string UserNId = "", bool IsDataProvider = false)
    {
        string MessageContent = string.Empty;
        string Subject = string.Empty;
        string Body = string.Empty;
        string TamplatePath = string.Empty;
        string IPAdress = string.Empty;
        string ACTIVATION_URL = string.Empty;
        string FORGOT_URL = string.Empty;
        TamplatePath = Path.Combine(this.Page.Request.PhysicalApplicationPath, Constants.FolderName.EmailTemplates);
        IPAdress = this.Page.Request.UserHostAddress;
        ACTIVATION_URL = this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.LastIndexOf("/") + 1) + "accountactivated.aspx?emailid=" + EmailId + "&key=" + TokenKey + "&flow=" + ActivationFlow + "&popup=" + isDataView;
        FORGOT_URL = this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.LastIndexOf("/") + 1) + "accountconfirmation.aspx?emailid=" + EmailId + "&key=" + TokenKey + "&flow=" + ActivationFlow + "&popup=" + isDataView;
        //ACTIVATION_URL = this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.LastIndexOf("/") + 1) + "accountconfirmation.aspx?emailid=" + EmailId + "&key=" + TokenKey + "&flow=" + ActivationFlow;
        if (!IsAdmin)
        {
            if (ActivationFlow)
            {
                TamplatePath += Language + "\\" + Constants.FileName.Activation;
                MessageContent = GetEmailTamplate(TamplatePath);
                Subject = MessageContent.Split("\r\n".ToCharArray())[0].ToString();
                Subject = Subject.Replace("[^^^^]", "");
                Subject = Subject.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
                Body = MessageContent.Replace(MessageContent.Split("\r\n".ToCharArray())[0], "");
                Body = Body.Replace("[****]USER_NAME[****]", FirstName);
                Body = Body.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
                Body = Body.Replace("[****]ADAPTATION_URL[****]", this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("libraries")));
                Body = Body.Replace("[****]USER_EMAILID[****]", EmailId);
                Body = Body.Replace("[****]DATA_CONSUMER_ID[****]", DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId);
                Body = Body.Replace("[****]EMAILID_DB_ADMIN[****]", Global.DbAdmEmail);
                Body = Body.Replace("[****]ACTIVATION_URL[****]", ACTIVATION_URL);
                this.Send_Email(Global.adaptation_name + " - WebMaster", "no-reply@dataforall.org", EmailId, Subject, Body, true, FirstName);
            }
            else
            {
                TamplatePath += Language + "\\" + Constants.FileName.ForgotPassword;
                MessageContent = GetEmailTamplate(TamplatePath);
                Subject = MessageContent.Split("\r\n".ToCharArray())[0].ToString();
                Subject = Subject.Replace("[^^^^]", "");
                Subject = Subject.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
                Body = MessageContent.Replace(MessageContent.Split("\r\n".ToCharArray())[0], "");
                Body = Body.Replace("[****]USER_NAME[****]", FirstName);
                Body = Body.Replace("[****]USER_EMAIL_ID[****]", EmailId);
                Body = Body.Replace(" [****]IP_ADDRESS[****]", IPAdress);
                Body = Body.Replace("[****]FORGOT_URL[****]", FORGOT_URL);
                Body = Body.Replace("[****]EMAILID_DB_ADMIN[****]", Global.DbAdmEmail);
                Body = Body.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
                this.Send_Email(Global.adaptation_name + " - WebMaster", "no-reply@dataforall.org", EmailId, Subject, Body, true, FirstName);
            }
        }
    }
    #endregion

    private void Frame_UpdateMessage_And_Send_Mail(string FirstName, string EmailId, string Language)
    {
        string MessageContent = string.Empty;
        string Subject = string.Empty;
        string Body = string.Empty;
        string TamplatePath = string.Empty;
        //TamplatePath = Path.Combine(this.Page.Request.PhysicalApplicationPath, Constants.FolderName.Template);   
        TamplatePath = Path.Combine(this.Page.Request.PhysicalApplicationPath, Constants.FolderName.EmailTemplates);
        TamplatePath += Language + "\\" + Constants.FileName.UpdatePassword;
        MessageContent = GetEmailTamplate(TamplatePath);
        Subject = MessageContent.Split("\r\n".ToCharArray())[0].ToString();
        Subject = Subject.Replace("[^^^^]", "");
        Subject = Subject.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
        Body = MessageContent.Replace(MessageContent.Split("\r\n".ToCharArray())[0], "");
        Body = Body.Replace("[****]USER_NAME[****]", FirstName);
        Body = Body.Replace("[****]EMAILID_DB_ADMIN[****]", Global.DbAdmEmail);
        Body = Body.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
        this.Send_Email(Global.adaptation_name + " - WebMaster", "no-reply@dataforall.org", EmailId, Subject, Body, true, FirstName);
    }

    private void Frame_Message_And_Send_Catalog_Mail(string AdaptationName, string AdaptationURL, string Visible, string DBName, string DBInsitute, string DBEmail, string AdaptedFor, string Country, string Subnational, string DateCreated, string DateModified, string Type)
    {
        if (Type == "New")
        {
            try
            {
                string MessageContent = string.Empty;
                string Subject = string.Empty;
                string Body = string.Empty;
                string TamplatePath = string.Empty;
                TamplatePath = Path.Combine(this.Page.Request.PhysicalApplicationPath, Constants.FolderName.EmailTemplates);
                TamplatePath += "en" + "\\" + Constants.FileName.CatalogNewEntry;
                MessageContent = GetEmailTamplate(TamplatePath);
                Subject = MessageContent.Split("\r\n".ToCharArray())[0].ToString();
                Subject = Subject.Replace("[^^^^]", "");
                Subject = Subject.Replace("[****]ADAPTATION_NAME[****]", AdaptationName);
                Body = MessageContent.Replace(MessageContent.Split("\r\n".ToCharArray())[0], "");
                Body = Body.Replace("[****]ADAPTATION_NAME[****]", AdaptationName);
                Body = Body.Replace("[****]ADAPTATION_URL[****]", AdaptationURL);
                Body = Body.Replace("[****]ADAPTATION_VISIBLE[****]", "False");
                Body = Body.Replace("[****]DB_NAME[****]", DBName);
                Body = Body.Replace("[****]DB_INSTITUTION[****]", DBInsitute);
                Body = Body.Replace("[****]DB_EMAIL[****]", DBEmail);
                Body = Body.Replace("[****]ADAPTED_FOR[****]", AdaptedFor);
                Body = Body.Replace("[****]COUNTRY[****]", Country);
                Body = Body.Replace("[****]SUBNATIONAL[****]", Subnational);
                Body = Body.Replace("[****]DATE_CREATED[****]", DateCreated);
                Body = Body.Replace("[****]DATE_MODIFIED[****]", DateModified);
                this.Send_Email(Global.adaptation_name + " - WebMaster", "support@dataforall.org", "support@dataforall.org", Subject, Body, true);
            }
            catch (Exception ex)
            {
                Global.CreateExceptionString(ex, null);
            }
        }
        else
        {
            try
            {
                string MessageContent = string.Empty;
                string Subject = string.Empty;
                string Body = string.Empty;
                string TamplatePath = string.Empty;
                TamplatePath = Path.Combine(this.Page.Request.PhysicalApplicationPath, Constants.FolderName.EmailTemplates);
                TamplatePath += "en" + "\\" + Constants.FileName.CatalogUpdatedEntry;
                MessageContent = GetEmailTamplate(TamplatePath);
                Subject = MessageContent.Split("\r\n".ToCharArray())[0].ToString();
                Subject = Subject.Replace("[^^^^]", "");
                Subject = Subject.Replace("[****]ADAPTATION_NAME[****]", AdaptationName);
                Subject = Subject.Replace("[****]ADAPTATION_VISIBILITY[****]", Visible);
                Body = MessageContent.Replace(MessageContent.Split("\r\n".ToCharArray())[0], "");
                Body = Body.Replace("[****]ADAPTATION_NAME[****]", AdaptationName);
                Body = Body.Replace("[****]ADAPTATION_URL[****]", AdaptationURL);
                Body = Body.Replace("[****]ADAPTATION_VISIBLE[****]", Visible);
                Body = Body.Replace("[****]DB_NAME[****]", DBName);
                Body = Body.Replace("[****]DB_INSTITUTION[****]", DBInsitute);
                Body = Body.Replace("[****]DB_EMAIL[****]", DBEmail);
                Body = Body.Replace("[****]ADAPTED_FOR[****]", AdaptedFor);
                Body = Body.Replace("[****]COUNTRY[****]", Country);
                Body = Body.Replace("[****]SUBNATIONAL[****]", Subnational);
                Body = Body.Replace("[****]DATE_CREATED[****]", DateCreated);
                Body = Body.Replace("[****]DATE_MODIFIED[****]", DateModified);
                this.Send_Email(Global.adaptation_name + " - WebMaster", "support@dataforall.org", "support@dataforall.org", Subject, Body, true);
            }

            catch (Exception ex)
            {
                Global.CreateExceptionString(ex, null);
            }
        }
    }
    private DataTable Replace_AreaNIds_With_Names(DataTable DtUsers)
    {
        DataTable RetVal;
        DataRow RetValRow;
        string AreaNIds, AreaNames;
        Dictionary<string, string> DictArea;

        RetVal = null;
        RetValRow = null;
        AreaNIds = ",";
        AreaNames = string.Empty;
        DictArea = new Dictionary<string, string>();

        foreach (DataRow DrUsers in DtUsers.Rows)
        {
            if (!AreaNIds.Contains("," + DrUsers["User Country"].ToString() + ","))
            {
                AreaNIds += DrUsers["User Country"].ToString() + ",";
            }
        }

        if (AreaNIds.Length > 1)
        {
            AreaNIds = AreaNIds.Substring(1, AreaNIds.Length - 2);

            GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            AreaNames = GlobalUserWebService.GetAreasName(AreaNIds);


            for (int i = 0; i < AreaNIds.Split(',').Length; i++)
            {
                DictArea.Add(AreaNIds.Split(',')[i], AreaNames.Split(',')[i]);
            }

            RetVal = DtUsers.Clone();
            RetVal.Columns["User Country"].DataType = typeof(System.String);

            foreach (DataRow DrUsers in DtUsers.Rows)
            {
                RetValRow = RetVal.NewRow();
                RetValRow.ItemArray = DrUsers.ItemArray;
                RetValRow["User Country"] = DictArea[DrUsers["User Country"].ToString()].ToString();
                RetVal.Rows.Add(RetValRow);
            }
        }

        return RetVal;
    }

    private void Frame_Message_And_Send_Mail_DPRight(string AdminUser, string AdminUserId, string UserName, string EmailId, string Language, string UserNId)
    {
        string MessageContent = string.Empty;
        string Subject = string.Empty;
        string Body = string.Empty;
        string TamplatePath = string.Empty;
        DIConnection DIConnection;
        string Country = string.Empty;

        AdaptationUsers.AdaptationUsers UserLoginService = new AdaptationUsers.AdaptationUsers();
        UserLoginService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.AdaptationUserWebService;
        if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
        {
            DataSet dsUsers = new DataSet();
            dsUsers = UserLoginService.GetAreaForUser(Convert.ToInt32(UserNId));
            Country = dsUsers.Tables[0].Rows[0][0].ToString();
        }
        else
        {
            DataTable dtCountryNId = null;
            string AreaNId = string.Empty;
            string Query;
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
            Query = "SELECT User_Country FROM Users WHERE nid = " + UserNId + ";";
            dtCountryNId = DIConnection.ExecuteDataTable(Query);
            if (dtCountryNId != null && dtCountryNId.Rows.Count > 0)
            {
                AreaNId = dtCountryNId.Rows[0]["User_Country"].ToString().Trim();
            }
            DataSet dsUsers = new DataSet();
            dsUsers = UserLoginService.GetAreaFromAreaNId(Convert.ToInt32(AreaNId));
            Country = dsUsers.Tables[0].Rows[0][0].ToString();

        }
        TamplatePath = Path.Combine(this.Page.Request.PhysicalApplicationPath, Constants.FolderName.EmailTemplates);
        TamplatePath += Language + "\\" + Constants.FileName.RequestDataProviderRights;
        MessageContent = GetEmailTamplate(TamplatePath);
        Subject = MessageContent.Split("\r\n".ToCharArray())[0].ToString();
        Subject = Subject.Replace("[^^^^]", "");
        Subject = Subject.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
        Body = MessageContent.Replace(MessageContent.Split("\r\n".ToCharArray())[0], "");
        Body = Body.Replace("[****]USER_EMAILID[****]", EmailId);
        Body = Body.Replace("[****]USER_NAME[****]", UserName);
        Body = Body.Replace("[****]COUNTRY[****]", Country);
        Body = Body.Replace("[****]ADAPTATION_NAME[****]", Global.adaptation_name);
        Body = Body.Replace("[****]ADAPTATION_URL[****]", this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("libraries")));
        this.Send_Email("WebMaster - " + Global.adaptation_name, "no-reply@dataforall.org", AdminUserId, Subject, Body, true, AdminUser);
    }

    private string Get_AdminNId()
    {
        string RetVal;
        diworldwide_userinfo.UserLoginInformation Service;
        DIConnection DIConnection;
        DataTable DtAdmin;

        RetVal = string.Empty;
        Service = null;
        DIConnection = null;
        DtAdmin = null;

        try
        {
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                Service = new diworldwide_userinfo.UserLoginInformation();
                Service.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
                RetVal = Service.GetAdminNid(Global.GetAdaptationGUID());
            }
            else
            {
                DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
                DtAdmin = DIConnection.ExecuteDataTable("SELECT NId FROM Users WHERE User_Is_Admin = 'True'");

                if (DtAdmin != null && DtAdmin.Rows.Count > 0)
                {
                    RetVal = DtAdmin.Rows[0]["NId"].ToString().Trim();
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    private void Frame_Message_And_Send_Forget_Password_Mail(string Name, string EmailId, string Password)
    {
        string MessageContent;

        MessageContent = "Dear " + Name + ",\n\nThis mail is in response to a forgot password request raised at " + Global.adaptation_name +
                                 "(" + this.Page.Request.Url.AbsoluteUri.Substring(0, this.Page.Request.Url.AbsoluteUri.IndexOf("libraries")) + "). Please find your new credentials below:";
        MessageContent += "\n\nUser Id: " + EmailId + "\n\nPassword: " + Password;

        MessageContent += "\n\nThank You.";
        MessageContent += "\n\nWith Best Regards,";
        MessageContent += "\n\nAdmin";
        MessageContent += "\n\n" + Global.adaptation_name;

        this.Send_Email(ConfigurationManager.AppSettings["NotificationSender"].ToString(), ConfigurationManager.AppSettings["NotificationSenderEmailId"].ToString(), EmailId, "Forgot Password Response", MessageContent);
    }

    #endregion "--Common--"

    #region "--UpdateUserDetails--"

    private void Update_DataProvider_Artefact(string UserNId, string name, string language)
    {
        string UserId, FilenameWPath, OutputFolder;
        UserTypes UserType;

        if (Global.Is_Already_Existing_Provider(UserNId) == true)
        {
            OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users);
            FilenameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);
            UserType = UserTypes.Provider;
            UserId = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId;

            SDMXUtility.Register_User(SDMXSchemaType.Two_One, FilenameWPath, UserType, UserId, name, language, string.Empty);
        }
    }

    private void Convert_Provider_To_Consumer(string UserNId, string name, string language)
    {
        DataTable DtRegisteredDatabases;
        DIConnection DIConnection;
        string UserId, FilenameWPath, OutputFolder, Query;
        string[] PAFileNames;
        string[] RegistrationFileNames, SubscriptionFileNames;
        string[] ConstraintFileNames;
        UserTypes UserType;
        RegistryInterfaceType RegistryInterfaceSubscription;

        DIConnection = null;

        try
        {
            if (Global.Is_Already_Existing_Provider(UserNId) == true)
            {
                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users);

                #region "--Unregister Provider--"

                FilenameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);
                UserType = UserTypes.Provider;
                UserId = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId;

                SDMXUtility.UnRegister_User(SDMXSchemaType.Two_One, FilenameWPath, UserType, UserId);

                #endregion "--Unregister Provider--"

                #region "--Register Consumer--"

                FilenameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.FileName);
                UserType = UserTypes.Consumer;
                UserId = DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId;

                if (File.Exists(FilenameWPath))
                {
                    SDMXUtility.Register_User(SDMXSchemaType.Two_One, FilenameWPath, UserType, UserId, name, language, string.Empty);
                }
                else
                {
                    SDMXUtility.Register_User(SDMXSchemaType.Two_One, string.Empty, UserType, UserId, name, language, OutputFolder);
                }

                #endregion "--Register Consumer--"

                DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                                  string.Empty, string.Empty);
                Query = "SELECT DISTINCT DBNId FROM Artefacts WHERE DBNId <> -1;";
                DtRegisteredDatabases = DIConnection.ExecuteDataTable(Query);

                foreach (DataRow DrRegisteredDatabases in DtRegisteredDatabases.Rows)
                {
                    #region "--Delete PA--"

                    OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Provisioning Metadata\\PAs");
                    PAFileNames = Directory.GetFiles(OutputFolder);
                    FilenameWPath = Path.Combine(OutputFolder, DevInfo.Lib.DI_LibSDMX.Constants.PA.Prefix + UserNId + DevInfo.Lib.DI_LibSDMX.Constants.Underscore);

                    foreach (string FileName in PAFileNames)
                    {
                        if (FileName.Contains(FilenameWPath))
                        {
                            Query = "DELETE FROM Artefacts WHERE FileLocation = '" + FileName + "';";
                            DIConnection.ExecuteDataTable(Query);
                            File.Delete(FileName);
                        }
                    }

                    #endregion "--Delete PA--"

                    #region "--Delete Registration Folder--"

                    OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Registrations\\" + UserNId);
                    RegistrationFileNames = Directory.GetFiles(OutputFolder);

                    foreach (string FileName in RegistrationFileNames)
                    {
                        Query = "DELETE FROM Artefacts WHERE FileLocation = '" + FileName + "';";
                        DIConnection.ExecuteDataTable(Query);
                        File.Delete(FileName);
                    }

                    Global.DeleteDirectory(OutputFolder);

                    #endregion "--Delete Registration Folder--"

                    #region "--Delete Constraints Folder--"

                    OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Constraints\\" + UserNId);
                    ConstraintFileNames = Directory.GetFiles(OutputFolder);

                    foreach (string FileName in ConstraintFileNames)
                    {
                        Query = "DELETE FROM Artefacts WHERE FileLocation = '" + FileName + "';";
                        DIConnection.ExecuteDataTable(Query);
                        File.Delete(FileName);
                    }

                    Global.DeleteDirectory(OutputFolder);

                    #endregion "--Delete Constraints Folder--"

                    #region "--Change Subscriptions--"

                    OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Subscriptions\\" + UserNId);
                    SubscriptionFileNames = Directory.GetFiles(OutputFolder);

                    foreach (string FileName in SubscriptionFileNames)
                    {
                        RegistryInterfaceSubscription = (RegistryInterfaceType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(RegistryInterfaceType), FileName);

                        ((SubmitSubscriptionsRequestType)RegistryInterfaceSubscription.Item).SubscriptionRequest[0].Subscription.Organisation = new OrganisationReferenceType();
                        ((SubmitSubscriptionsRequestType)RegistryInterfaceSubscription.Item).SubscriptionRequest[0].Subscription.Organisation.Items = new List<object>();
                        ((SubmitSubscriptionsRequestType)RegistryInterfaceSubscription.Item).SubscriptionRequest[0].Subscription.Organisation.Items.Add(new DataConsumerRefType(DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId, DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.AgencyId, DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Id, DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Version));

                        Serializer.SerializeToFile(typeof(RegistryInterfaceType), RegistryInterfaceSubscription, FileName);
                    }

                    #endregion "--Change Subscriptions--"
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    private void Convert_Consumer_To_Provider(string UserNId, string name, string language)
    {
        DataTable DtRegisteredDatabases;
        DIConnection DIConnection;
        string UserId, FilenameWPath, OutputFolder, Query;
        string[] SubscriptionFileNames;
        UserTypes UserType;
        RegistryInterfaceType RegistryInterfaceSubscription;

        DIConnection = null;

        try
        {
            if (this.Is_Already_Existing_Consumer(UserNId) == true)
            {
                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users);

                #region "--Unregister Consumer--"

                FilenameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.FileName);
                UserType = UserTypes.Consumer;
                UserId = DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId;

                SDMXUtility.UnRegister_User(SDMXSchemaType.Two_One, FilenameWPath, UserType, UserId);

                #endregion "--Unregister Consumer--"

                #region "--Register Provider--"

                FilenameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);
                UserType = UserTypes.Provider;
                UserId = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId;

                if (File.Exists(FilenameWPath))
                {
                    SDMXUtility.Register_User(SDMXSchemaType.Two_One, FilenameWPath, UserType, UserId, name, language, string.Empty);
                }
                else
                {
                    SDMXUtility.Register_User(SDMXSchemaType.Two_One, string.Empty, UserType, UserId, name, language, OutputFolder);
                }

                #endregion "--Register Provider--"

                #region "--Change Subscriptions--"

                DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                                      string.Empty, string.Empty);
                Query = "SELECT DISTINCT DBNId FROM Artefacts WHERE DBNId <> -1;";
                DtRegisteredDatabases = DIConnection.ExecuteDataTable(Query);

                foreach (DataRow DrRegisteredDatabases in DtRegisteredDatabases.Rows)
                {
                    OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Subscriptions\\" + UserNId);
                    SubscriptionFileNames = Directory.GetFiles(OutputFolder);

                    foreach (string FileName in SubscriptionFileNames)
                    {
                        RegistryInterfaceSubscription = (RegistryInterfaceType)SDMXObjectModel.Deserializer.LoadFromFile(typeof(RegistryInterfaceType), FileName);

                        ((SubmitSubscriptionsRequestType)RegistryInterfaceSubscription.Item).SubscriptionRequest[0].Subscription.Organisation = new OrganisationReferenceType();
                        ((SubmitSubscriptionsRequestType)RegistryInterfaceSubscription.Item).SubscriptionRequest[0].Subscription.Organisation.Items = new List<object>();
                        ((SubmitSubscriptionsRequestType)RegistryInterfaceSubscription.Item).SubscriptionRequest[0].Subscription.Organisation.Items.Add(new DataProviderRefType(DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.AgencyId, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Id, DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Version));

                        Serializer.SerializeToFile(typeof(RegistryInterfaceType), RegistryInterfaceSubscription, FileName);
                    }
                }

                #endregion "--Change Subscriptions--"

                Global.Create_Other_Artefacts_And_Update_Folder_Structures_For_Provider_Per_Database(UserNId);
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    private void Update_DataConsumer_Artefact(string UserNId, string name, string language)
    {
        string UserId, FilenameWPath, OutputFolder;
        UserTypes UserType;

        if (this.Is_Already_Existing_Consumer(UserNId) == true)
        {
            OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users);
            FilenameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.FileName);
            UserType = UserTypes.Consumer;
            UserId = DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId;

            SDMXUtility.Register_User(SDMXSchemaType.Two_One, FilenameWPath, UserType, UserId, name, language, string.Empty);
        }
    }

    #endregion "--UpdateUserDetails--"

    #region "--Delete User--"

    private void Delete_Provider_And_Artefacts(string UserNId)
    {
        DataTable DtRegisteredDatabases;
        DIConnection DIConnection;
        string UserId, FilenameWPath, OutputFolder, Query;
        string[] PAFileNames;
        string[] RegistrationFileNames, SubscriptionFileNames;
        string[] ConstraintFileNames;
        UserTypes UserType;

        DIConnection = null;

        try
        {
            if (Global.Is_Already_Existing_Provider(UserNId) == true)
            {
                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users);

                #region "--Unregister Provider--"

                FilenameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.FileName);
                UserType = UserTypes.Provider;
                UserId = DevInfo.Lib.DI_LibSDMX.Constants.DataProviderScheme.Prefix + UserNId;

                SDMXUtility.UnRegister_User(SDMXSchemaType.Two_One, FilenameWPath, UserType, UserId);

                #endregion "--Unregister Provider--"

                DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                                  string.Empty, string.Empty);
                Query = "SELECT DISTINCT DBNId FROM Artefacts WHERE DBNId <> -1;";
                DtRegisteredDatabases = DIConnection.ExecuteDataTable(Query);

                foreach (DataRow DrRegisteredDatabases in DtRegisteredDatabases.Rows)
                {
                    #region "--Delete PA--"

                    OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Provisioning Metadata\\PAs");
                    PAFileNames = Directory.GetFiles(OutputFolder);
                    FilenameWPath = Path.Combine(OutputFolder, DevInfo.Lib.DI_LibSDMX.Constants.PA.Prefix + UserNId + DevInfo.Lib.DI_LibSDMX.Constants.Underscore);

                    foreach (string FileName in PAFileNames)
                    {
                        if (FileName.Contains(FilenameWPath))
                        {
                            Query = "DELETE FROM Artefacts WHERE FileLocation = '" + FileName + "';";
                            DIConnection.ExecuteDataTable(Query);
                            File.Delete(FileName);
                        }
                    }

                    #endregion "--Delete PA--"

                    #region "--Delete Registration Folder--"

                    OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Registrations\\" + UserNId);
                    RegistrationFileNames = Directory.GetFiles(OutputFolder);

                    foreach (string FileName in RegistrationFileNames)
                    {
                        Query = "DELETE FROM Artefacts WHERE FileLocation = '" + FileName + "';";
                        DIConnection.ExecuteDataTable(Query);
                        File.Delete(FileName);
                    }

                    Global.DeleteDirectory(OutputFolder);

                    #endregion "--Delete Registration Folder--"

                    #region "--Delete Constraints Folder--"

                    OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Constraints\\" + UserNId);
                    ConstraintFileNames = Directory.GetFiles(OutputFolder);

                    foreach (string FileName in ConstraintFileNames)
                    {
                        Query = "DELETE FROM Artefacts WHERE FileLocation = '" + FileName + "';";
                        DIConnection.ExecuteDataTable(Query);
                        File.Delete(FileName);
                    }

                    Global.DeleteDirectory(OutputFolder);

                    #endregion "--Delete Constraints Folder--"

                    #region "--Delete Subscriptions--"

                    OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Subscriptions\\" + UserNId);
                    SubscriptionFileNames = Directory.GetFiles(OutputFolder);

                    foreach (string FileName in SubscriptionFileNames)
                    {
                        Query = "DELETE FROM Artefacts WHERE FileLocation = '" + FileName + "';";
                        DIConnection.ExecuteDataTable(Query);
                        File.Delete(FileName);
                    }

                    Global.DeleteDirectory(OutputFolder);

                    #endregion "--Delete Subscriptions--"
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    private void Delete_Consumer_And_Artefacts(string UserNId)
    {
        DataTable DtRegisteredDatabases;
        DIConnection DIConnection;
        string UserId, FilenameWPath, OutputFolder, Query;
        string[] SubscriptionFileNames;
        UserTypes UserType;

        DIConnection = null;

        try
        {
            if (this.Is_Already_Existing_Consumer(UserNId) == true)
            {
                OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users);

                #region "--Unregister Consumer--"

                FilenameWPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Users + DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.FileName);
                UserType = UserTypes.Consumer;
                UserId = DevInfo.Lib.DI_LibSDMX.Constants.DataConsumerScheme.Prefix + UserNId;

                SDMXUtility.UnRegister_User(SDMXSchemaType.Two_One, FilenameWPath, UserType, UserId);

                #endregion "--Unregister Consumer--"

                #region "--Delete Subscriptions--"

                DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                                      string.Empty, string.Empty);
                Query = "SELECT DISTINCT DBNId FROM Artefacts WHERE DBNId <> -1;";
                DtRegisteredDatabases = DIConnection.ExecuteDataTable(Query);

                foreach (DataRow DrRegisteredDatabases in DtRegisteredDatabases.Rows)
                {
                    OutputFolder = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.Data + DrRegisteredDatabases["DBNId"].ToString() + "\\sdmx\\Subscriptions\\" + UserNId);
                    SubscriptionFileNames = Directory.GetFiles(OutputFolder);

                    foreach (string FileName in SubscriptionFileNames)
                    {
                        Query = "DELETE FROM Artefacts WHERE FileLocation = '" + FileName + "';";
                        DIConnection.ExecuteDataTable(Query);
                        File.Delete(FileName);
                    }

                    Global.DeleteDirectory(OutputFolder);
                }

                #endregion "--Delete Subscriptions--"
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
    }

    #endregion "--Delete User--"

    #region "Insert into catalog"
    private bool InsertIntoCatalog()
    {
        string WebURL = Global.GetAdaptationUrl();
        string AdaptationName = Global.adaptation_name;
        string AdaptedFor = Global.adapted_for;
        string SubNation = Global.sub_nation;
        string DbAdmName = Global.DbAdmName;
        string DbAdmInstitution = Global.DbAdmInstitution;
        string DbAdmEmail = Global.DbAdmEmail;
        string UnicefRegion = Global.UnicefRegion;
        string AdaptationYear = Global.AdaptationYear;
        //Check for newly added App Setting Key -Country
        XmlDocument XmlDoc;
        string AppSettingFile = string.Empty;
        AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
        XmlDoc = new XmlDocument();
        XmlDoc.Load(AppSettingFile);
        Global.CheckAppSetting(XmlDoc, Constants.AppSettingKeys.Country, string.Empty);
        string Country = Global.Country;
        bool RetVal = false;
        string typeOfEmail = string.Empty;
        string DateCreated = string.Empty;
        string Visible = string.Empty;
        try
        {
            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            DataSet dsCatalogAdaptation = new DataSet();
            dsCatalogAdaptation = CatalogService.CatalogExists(Global.GetAdaptationGUID());
            if (dsCatalogAdaptation.Tables[0].Rows.Count == 0)
            {
                typeOfEmail = "New";
                DateCreated = String.Format("{0:r}", DateTime.Now);
            }
            else
            {
                typeOfEmail = "Updated";
                DateCreated = String.Format("{0:r}", Convert.ToDateTime(dsCatalogAdaptation.Tables[0].Rows[0][1]));
                Visible = dsCatalogAdaptation.Tables[0].Rows[0][2].ToString();
            }
            if (CatalogService.SetCatalog(AdaptationName, "", "", false, true, WebURL, 0, 0, 0, 0, "", "", DateTime.Now.ToString(), 0, SubNation, "", DbAdmName, DbAdmInstitution, DbAdmEmail, UnicefRegion, AdaptationYear, "", "", AdaptedFor, Country, DateTime.Now.ToString(), Global.GetAdaptationGUID()))
            {
                RetVal = true;
                Frame_Message_And_Send_Catalog_Mail(AdaptationName, WebURL, Visible, DbAdmName, DbAdmInstitution, DbAdmEmail, AdaptedFor, Country, SubNation, DateCreated, String.Format("{0:r}", DateTime.Now), typeOfEmail);
            }
        }
        catch (Exception ex)
        {
            RetVal = false;
            Global.CreateExceptionString(ex, null);

        }
        return RetVal;
    }
    #endregion

    private string IsAccountActive(string UserNid)
    {
        DataTable TokenInfoDT = null;
        DIConnection DIConnection;
        string Query;
        string RetVal = string.Empty;
        DIConnection = null;
        DateTime TimeStamp;
        double ExpireTime = 1;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);
            Query = "SELECT * FROM TokenInformation WHERE User_Nid = " + UserNid + " and isDataviewFlow=false;";
            TokenInfoDT = DIConnection.ExecuteDataTable(Query);
            if (TokenInfoDT != null && TokenInfoDT.Rows.Count > 0)
            {
                RetVal = false + Constants.Delimiters.ParamDelimiter + "0";
                string TS = TokenInfoDT.Rows[0]["createdtime"].ToString();
                TimeStamp = Convert.ToDateTime(TS);
                TimeSpan TimeSpan = DateTime.Now.Subtract(TimeStamp);
                if (TimeSpan.TotalHours > ExpireTime)
                {
                    RetVal = false + Constants.Delimiters.ParamDelimiter + "2";
                }
            }
            else
            {
                RetVal = true + Constants.Delimiters.ParamDelimiter + "1";
                Query = "SELECT * FROM TokenInformation WHERE User_Nid = " + UserNid + " and isDataviewFlow=true;";
                TokenInfoDT = DIConnection.ExecuteDataTable(Query);
                if (TokenInfoDT != null && TokenInfoDT.Rows.Count == 0)
                {
                    RetVal = true + Constants.Delimiters.ParamDelimiter + "1";
                }
                else
                {
                    string TS = TokenInfoDT.Rows[0]["createdtime"].ToString();
                    TimeStamp = Convert.ToDateTime(TS);
                    TimeSpan TimeSpan = DateTime.Now.Subtract(TimeStamp);
                    if (TimeSpan.TotalHours <= ExpireTime)
                    {
                        RetVal = true + Constants.Delimiters.ParamDelimiter + "1";
                    }
                    else
                    {
                        RetVal = false + Constants.Delimiters.ParamDelimiter + "2";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private string GetDeactivatedAccountInfo(string UserNid)
    {
        DataTable TokenInfoDT = null;
        DIConnection DIConnection;
        string Query;
        string RetVal = string.Empty;
        DIConnection = null;

        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);
            Query = "SELECT * FROM TokenInformation WHERE User_Nid = " + UserNid + ";";
            TokenInfoDT = DIConnection.ExecuteDataTable(Query);
            if (TokenInfoDT != null && TokenInfoDT.Rows.Count == 1)
            {
                RetVal = "Your account has not been activated yet.";
            }
            else if (TokenInfoDT != null && TokenInfoDT.Rows.Count == 2)
            {
                RetVal = "You have changed your password but it's not activated.";
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    private string GetEmailTamplate(string url)
    {
        string AllText = string.Empty;
        try
        {
            AllText = File.ReadAllText(url);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return AllText;
    }

    private void Call_Send_Mail_DPRight(string FirstName, string EmailId, string Language, string UserNId)
    {
        string AdminNId = this.Get_AdminNId();
        string[] AdminDetails;
        AdminDetails = Global.SplitString(this.GetUserDetails(AdminNId), Constants.Delimiters.ParamDelimiter);
        Frame_Message_And_Send_Mail_DPRight(AdminDetails[2], AdminDetails[0], FirstName, EmailId, Language, UserNId);
    }

    #endregion "--Private--"

    #region "--Public--"

    public string RegisterUser(string requestParam)
    {
        string RetVal;
        string[] Params, AdminDetails;
        string EmailId, FirstName, Country, Password, Language, UserNId, AdminNId;
        bool IsDataProvider = false, IsAdmin, DataProviderRights, SendDevInfoUpdates, isDataview;

        ///Variables for creatin XLSLogfile 
        string XLSFileMsg = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            EmailId = Params[0].ToString().Trim();
            FirstName = Params[1].ToString().Trim();
            Country = Params[2].ToString().Trim();
            Password = Global.OneWayEncryption(Params[3].ToString().Trim());
            DataProviderRights = bool.Parse(Params[4].ToString().Trim());
            IsAdmin = bool.Parse(Params[5].ToString().Trim());
            SendDevInfoUpdates = bool.Parse(Params[6].ToString().Trim());
            Language = Params[7].ToString().Trim();
            isDataview = Convert.ToBoolean(Params[8].ToString().Trim());
            UserNId = string.Empty;
            IsDataProvider = DataProviderRights;
            if (IsAdmin == true)
            {
                IsDataProvider = true;
            }
            // Global Registration
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
                if (!GlobalUserWebService.Is_Existing_User(EmailId))
                {
                    //AdaptationUrl = Global.GetAdaptationUrl();

                    GlobalUserWebService.Save_In_User_Table(EmailId, Password, FirstName, string.Empty, Country, IsDataProvider, IsAdmin, SendDevInfoUpdates, Global.GetAdaptationGUID());
                    UserNId = GlobalUserWebService.GetUserNid(EmailId, Password, IsAdmin);
                    if (!string.IsNullOrEmpty(UserNId))
                    {
                        if (IsAdmin == true)
                        {
                            #region "call method to write log in csv file"
                            XLSFileMsg = string.Format(Constants.CSVLogMessage.NewAdminAccountcreated, FirstName, EmailId);
                            WriteLogInXLSFile(Constants.AdminModules.UserManagement.ToString(), XLSFileMsg);
                            #endregion

                            this.Create_MaintenanceAgency_ForAdmin(UserNId, FirstName, Language);
                            this.Frame_Message_And_Send_Mail(FirstName, EmailId, UserNId, IsAdmin, IsDataProvider, Language);
                            Session[Constants.SessionNames.LoggedAdminUserNId] = null;
                        }
                        else
                        {
                            string TokenKey = GlobalUserWebService.Save_In_TokenInformation(UserNId, true, isDataview);
                            this.Frame_TokenMessage_And_Send_Mail(FirstName, EmailId, IsAdmin, TokenKey, true, Language, isDataview, UserNId, IsDataProvider);
                        }
                        //this.Frame_Message_And_Send_Mail(FirstName, EmailId, UserNId, IsAdmin, IsDataProvider);

                        RetVal = "true" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("IDRegistered") + Constants.Delimiters.ParamDelimiter + UserNId + "|" + IsDataProvider.ToString();
                    }
                    else
                    {
                        RetVal = "false" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("UserNIDBlank");
                    }
                    if (DataProviderRights)
                    {
                        Call_Send_Mail_DPRight(FirstName, EmailId, Language, UserNId);
                    }
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("RegisteredForDifferentUser");
                }
            }
            else // Local Registration
            {
                if (!this.Is_Existing_User(EmailId))
                {
                    this.Save_In_User_Table(EmailId, Password, FirstName, string.Empty, Country, IsDataProvider, IsAdmin, SendDevInfoUpdates);
                    DataTable DtUser = this.Get_User(EmailId, Password, IsAdmin);

                    if (DtUser != null && DtUser.Rows.Count > 0)
                    {
                        UserNId = DtUser.Rows[0]["NId"].ToString();
                    }

                    if (IsAdmin == true)
                    {
                        this.Create_MaintenanceAgency_ForAdmin(UserNId, FirstName, Language);
                        this.Frame_Message_And_Send_Mail(FirstName, EmailId, UserNId, IsAdmin, IsDataProvider, Language);

                    }
                    else
                    {
                        string TokenKey = Save_In_TokenInformation(UserNId, true, isDataview);
                        this.Frame_TokenMessage_And_Send_Mail(FirstName, EmailId, IsAdmin, TokenKey, true, Language, isDataview, UserNId, IsDataProvider);
                    }

                    //this.Frame_Message_And_Send_Mail(FirstName, EmailId, UserNId, IsAdmin, IsDataProvider);

                    RetVal = "true" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("IDRegistered") + Constants.Delimiters.ParamDelimiter + UserNId + "|" + IsDataProvider.ToString();
                    if (DataProviderRights)
                    {
                        Call_Send_Mail_DPRight(FirstName, EmailId, Language, UserNId);
                    }
                }

                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("RegisteredForDifferentUser");
                }
            }
            //if (DataProviderRights)
            //{
            //    AdminNId = this.Get_AdminNId();
            //    AdminDetails = Global.SplitString(this.GetUserDetails(AdminNId), Constants.Delimiters.ParamDelimiter);
            //    Frame_Message_And_Send_Mail_DPRight(AdminDetails[2], AdminDetails[0], FirstName, EmailId,Language,UserNId);
            //}
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string LoginUser(string requestParam, bool isAdmin)
    {
        string RetVal;
        string[] Params;
        string EmailId, Password;
        DataTable DtUser;
        string UserNid = string.Empty;
        string TokenKey = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            EmailId = Params[0].ToString().Trim();
            Password = Global.OneWayEncryption(Params[1].ToString().Trim());

            // Global Login Flow
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
                UserNid = GlobalUserWebService.Get_UserNid(EmailId, Global.GetAdaptationGUID());
                //UserNid = "";
                if (!string.IsNullOrEmpty(UserNid))
                {
                    string loginInfo = GlobalUserWebService.IsAccountActive(UserNid);
                    bool Islogin = Convert.ToBoolean(loginInfo.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None)[0]);
                    string LoginMsgNumber = loginInfo.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None)[1];
                    if (Islogin)
                    {
                        RetVal = GlobalUserWebService.LoginUser(EmailId, Password, isAdmin, Global.GetAdaptationGUID());

                        if (this.isUserAdmin(UserNid))
                        {
                            if (RetVal.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None).Length >= 3)
                            {
                                Session[Constants.SessionNames.LoggedAdminUser] = RetVal.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None)[3];
                                Session[Constants.SessionNames.LoggedAdminUserNId] = RetVal.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None)[2].Split('|')[0];
                            }
                        }
                    }
                    else
                    {
                        RetVal = "false" + Constants.Delimiters.ParamDelimiter + LoginMsgNumber;
                    }
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + "3";
                }
            }
            else //"Local login Flow"
            {
                DtUser = this.Get_User(EmailId, Password, isAdmin);

                if (DtUser != null && DtUser.Rows.Count > 0)
                {
                    UserNid = DtUser.Rows[0]["NId"].ToString();
                    string loginInfo = this.IsAccountActive(UserNid);
                    bool Islogin = Convert.ToBoolean(loginInfo.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None)[0]);
                    string LoginMsgNumber = loginInfo.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None)[1];
                    if (Islogin)
                    {
                        RetVal = "true" + Constants.Delimiters.ParamDelimiter + LoginMsgNumber + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["NId"].ToString() + "|" + DtUser.Rows[0]["User_Is_Provider"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_First_Name"].ToString();

                        if (this.isUserAdmin(UserNid))
                        {
                            Session[Constants.SessionNames.LoggedAdminUser] = DtUser.Rows[0]["User_First_Name"].ToString();
                            Session[Constants.SessionNames.LoggedAdminUserNId] = DtUser.Rows[0]["NId"].ToString();
                        }
                    }
                    else
                    {
                        RetVal = "false" + Constants.Delimiters.ParamDelimiter + LoginMsgNumber;
                    }
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + "3";
                }
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + "4" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);
        }
        finally
        {
        }

        return RetVal;
    }

    public string GetUserDetails(string requestParam)
    {
        string RetVal;
        string[] Params, UserDetails;
        DataTable DtUser;
        string UserNId, UserEmailId, UserPwd, UserFirstName, UserLastName, AreaNid, IsProvider, IsAdmin, SendDevInfoUpdates;
        try
        {
            RetVal = string.Empty;
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            UserNId = Params[0].ToString().Trim();
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
                RetVal = GlobalUserWebService.GetUserDetails(UserNId, Global.GetAdaptationGUID());
                if (!string.IsNullOrEmpty(RetVal))
                {
                    UserDetails = RetVal.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None);
                    UserEmailId = UserDetails[0];
                    UserPwd = UserDetails[1];
                    UserFirstName = UserDetails[2];
                    UserLastName = UserDetails[3];
                    AreaNid = UserDetails[4];
                    IsProvider = UserDetails[5];
                    IsAdmin = UserDetails[6];
                    SendDevInfoUpdates = UserDetails[7];
                    RetVal = UserEmailId + Constants.Delimiters.ParamDelimiter + UserPwd + Constants.Delimiters.ParamDelimiter +
                        UserFirstName + Constants.Delimiters.ParamDelimiter + UserLastName + Constants.Delimiters.ParamDelimiter + AreaNid + Constants.Delimiters.ParamDelimiter + IsProvider + Constants.Delimiters.ParamDelimiter + IsAdmin + Constants.Delimiters.ParamDelimiter + SendDevInfoUpdates;
                }
            }
            else
            {
                DtUser = this.Get_User(Convert.ToInt32(UserNId));

                if (DtUser != null && DtUser.Rows.Count > 0)
                {
                    RetVal = DtUser.Rows[0]["User_Email_id"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_Password"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_First_Name"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_Last_Name"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_Country"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_Is_Provider"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_Is_Admin"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_Send_Updates"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            RetVal = ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string UpdateUserDetails(string requestParam)
    {
        string RetVal;
        string[] Params, AdminDetails;
        string UserNId, EmailId, FirstName, Country, Language, AdminNId, Password = string.Empty;
        bool IsAdmin = false;
        bool IsDataProvider = false;
        bool DataProviderRightsRequested = false;
        bool SendDevInfoUpdates = false;
        string UserEmailId = string.Empty;
        bool isUserExists = false;
        DataTable DtUser;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            UserNId = Params[0].ToString().Trim().Split('|')[0];
            EmailId = Params[1].ToString().Trim();
            FirstName = Params[2].ToString().Trim();
            Country = Params[3].ToString().Trim();
            if (Params[4].ToString().Trim() != "null")
                Password = Global.OneWayEncryption(Params[4].ToString().Trim());
            DataProviderRightsRequested = bool.Parse(Params[5].ToString().Trim());
            SendDevInfoUpdates = bool.Parse(Params[6].ToString().Trim());
            Language = Params[7].ToString().Trim();
            GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {

                IsAdmin = GlobalUserWebService.IsUserAdmin(UserNId, Global.GetAdaptationGUID());
                IsDataProvider = GlobalUserWebService.IsUserDataProvider(UserNId, Global.GetAdaptationGUID());
                UserEmailId = GlobalUserWebService.GetUserEmailId(UserNId);
                isUserExists = GlobalUserWebService.Is_Existing_User(EmailId);
            }
            else
            {
                DtUser = this.Get_User(Convert.ToInt32(UserNId));
                IsAdmin = Convert.ToBoolean(DtUser.Rows[0]["User_Is_Admin"].ToString());
                IsDataProvider = Convert.ToBoolean(DtUser.Rows[0]["User_Is_Provider"].ToString());
                UserEmailId = DtUser.Rows[0]["User_Email_id"].ToString();
                isUserExists = this.Is_Existing_User(EmailId);
            }

            if ((UserEmailId != EmailId) && isUserExists)
            {
                RetVal = "false" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("RegisteredForDifferentUser");
            }
            else
            {
                if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
                {
                    GlobalUserWebService.Update_In_User_Table(UserNId, EmailId, Password, FirstName, string.Empty, Country, DataProviderRightsRequested, IsAdmin, SendDevInfoUpdates, Global.GetAdaptationGUID());
                }
                else
                {
                    this.Update_In_User_Table(UserNId, EmailId, Password, FirstName, string.Empty, Country, DataProviderRightsRequested, IsAdmin, SendDevInfoUpdates);
                }

                if (IsDataProvider == false)
                {
                    this.Update_DataConsumer_Artefact(UserNId, FirstName, Language);
                }
                else if (IsDataProvider == true)
                {
                    this.Update_DataProvider_Artefact(UserNId, FirstName, Language);
                }

                //this.Frame_Message_And_Send_Mail(FirstName, EmailId, UserNId, IsAdmin, IsDataProvider,Language);

                if (DataProviderRightsRequested)
                {
                    AdminNId = this.Get_AdminNId();
                    AdminDetails = Global.SplitString(this.GetUserDetails(AdminNId), Constants.Delimiters.ParamDelimiter);

                    this.Frame_Message_And_Send_Mail_DPRight(AdminDetails[2], AdminDetails[0], FirstName, EmailId, Language, UserNId);
                }

                RetVal = "true";
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string ForgotPassword(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string Name, EmailId, Password, EncryptedPassword, Language;
        bool IsDataview;
        DataTable DtUser;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            EmailId = Params[0].ToString().Trim();
            IsDataview = Convert.ToBoolean(Params[1].ToString().Trim());
            Language = (Params[2].ToString().Trim());
            Password = Guid.NewGuid().ToString().Substring(0, 8);
            EncryptedPassword = Global.OneWayEncryption(Password);
            GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            // Global Flow
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                if (GlobalUserWebService.Is_Existing_User(EmailId))
                {
                    //GlobalUserWebService.Update_Password(EmailId, EncryptedPassword);
                    Name = GlobalUserWebService.Get_UserName(EmailId, Global.GetAdaptationGUID());
                    string UserNid = GlobalUserWebService.Get_UserNid(EmailId, Global.GetAdaptationGUID());
                    bool IsAdmin = GlobalUserWebService.IsUserAdmin(UserNid, Global.GetAdaptationGUID());
                    //this.Frame_Message_And_Send_Forget_Password_Mail(Name, EmailId, Password);
                    string TokenKey = GlobalUserWebService.Save_In_TokenInformation(UserNid, false, IsDataview);
                    this.Frame_TokenMessage_And_Send_Mail(Name, EmailId, IsAdmin, TokenKey, false, Language, IsDataview);
                    RetVal = "true" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("MailSent");
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("IDNotRegistered");
                }
            }
            else // Local Flow
            {
                DtUser = this.Get_User(EmailId);

                if (DtUser != null && DtUser.Rows.Count > 0)
                {
                    //Query = "UPDATE Users SET User_Password = '" + EncryptedPassword  + "' WHERE User_Email_Id = '" + EmailId + "';";
                    //DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"), string.Empty, string.Empty);
                    // DIConnection.ExecuteDataTable(Query);

                    Name = DtUser.Rows[0]["User_First_Name"].ToString();
                    //this.Frame_Message_And_Send_Forget_Password_Mail(Name, EmailId, Password);
                    string TokenKey = Save_In_TokenInformation(DtUser.Rows[0]["nid"].ToString(), false, IsDataview);
                    bool IsAdmin = this.isUserAdmin(DtUser.Rows[0]["nid"].ToString());
                    this.Frame_TokenMessage_And_Send_Mail(Name, EmailId, IsAdmin, TokenKey, false, Language, IsDataview);
                    RetVal = "true" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("MailSent");
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("IDNotRegistered");
                }
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetAllUsersGridHTML()
    {
        string RetVal = string.Empty;

        DataTable DtUsers;
        DIConnection DIConnection = null;
        HTMLTableGenerator TableGenerator;

        try
        {
            //AdaptationUrl = Global.GetAdaptationUrl();
            GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                DtUsers = GlobalUserWebService.GetAllUsersGridHTML(Global.GetAdaptationGUID());
            }
            else
            {
                DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);
                string Query = "SELECT NId, User_Email_Id as Email, User_First_Name as [Name], User_Country as [User Country], User_Is_Admin as [Admin User], User_Is_Admin as [Provider User], User_Send_Updates AS [Send Updates] FROM Users Where User_Is_Admin = 'False'";

                DtUsers = DIConnection.ExecuteDataTable(Query);
            }

            DtUsers = this.Replace_AreaNIds_With_Names(DtUsers);

            TableGenerator = new HTMLTableGenerator();
            TableGenerator.RowDisplayType = HTMLTableGenerator.DisplayType.RadioButtonType;
            RetVal = TableGenerator.GetTableHmtl(DtUsers, "NId", "usr", true);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    public string AdminLoginUser(string requestParam)
    {
        string RetVal = string.Empty;
        string[] LoggedUserArr;

        RetVal = LoginUser(requestParam, true);

        LoggedUserArr = Global.SplitString(RetVal, Constants.Delimiters.ParamDelimiter);

        if (LoggedUserArr[0] == "true")
        {
            Session[Constants.SessionNames.LoggedAdminUser] = LoggedUserArr[3];
            Session[Constants.SessionNames.LoggedAdminUserNId] = LoggedUserArr[2].Split('|')[0];
        }

        return RetVal;
    }

    public string DeleteUser(string requestParam)
    {
        string RetVal;
        string UserNId, Query;
        bool IsDataProvider, IsAdmin;
        RetVal = string.Empty;
        UserNId = string.Empty;
        Query = string.Empty;
        IsDataProvider = false;
        IsAdmin = false;
        DataTable DtTable;


        try
        {
            UserNId = requestParam;
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                //In global case user deletion not possible. (Not exposed via UI)
                //if (GlobalUserWebService.IsValidUser(UserNId))
                //{
                //    IsDataProvider = GlobalUserWebService.IsUserDataProvider(UserNId);
                //    IsAdmin = GlobalUserWebService.IsUserAdmin(UserNId);

                //    if (IsAdmin != true)
                //    {
                //        if (IsDataProvider == true)
                //        {
                //            this.Delete_Provider_And_Artefacts(UserNId);
                //        }
                //        else
                //        {
                //            this.Delete_Consumer_And_Artefacts(UserNId);
                //        }

                //        this.Delete_In_User_Table(Convert.ToInt32(UserNId));
                //    }
                //    else
                //    {
                //        RetVal = "false" + Constants.Delimiters.ParamDelimiter + "Admin user can't be deleted!";
                //    }
                //}
                //else
                //{
                //    RetVal = "false" + Constants.Delimiters.ParamDelimiter + "Invalid UserNId!";
                //}
            }
            else
            {
                DtTable = this.Get_User(Convert.ToInt32(UserNId));

                if (DtTable != null && DtTable.Rows.Count > 0)
                {
                    IsDataProvider = Convert.ToBoolean(DtTable.Rows[0]["User_Is_Provider"].ToString());
                    IsAdmin = Convert.ToBoolean(DtTable.Rows[0]["User_Is_Admin"].ToString());

                    if (IsAdmin != true)
                    {
                        if (IsDataProvider == true)
                        {
                            this.Delete_Provider_And_Artefacts(UserNId);
                        }
                        else
                        {
                            this.Delete_Consumer_And_Artefacts(UserNId);
                        }

                        this.Delete_In_User_Table(Convert.ToInt32(UserNId));
                    }
                    else
                    {
                        RetVal = "false" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("AdminUserNotDeleted");
                    }
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("InvalidUserNId");
                }
            }

            RetVal = "true";
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string GetM49Countries()
    {
        string AreaData = string.Empty;
        try
        {
            DIWorldwide.Catalog catalog = new DIWorldwide.Catalog();
            catalog.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            /*string Url = Request.Url.AbsoluteUri;
            int index = Url.IndexOf("libraries");
            string BaseUrl = Url.Substring(0, index - 1);
            string AdaptationData = catalog.GetJsonAdaptations(this.Page.Server.UrlDecode(BaseUrl.ToLower()));*/
            AreaData = catalog.GetM49Countries();
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        return AreaData;
    }

    public string IsAdminRegistered()
    {
        string RetVal = "False";
        string BaseUrl;
        string AdaptationName;
        string DbAdmName;
        string DbAdmInstitution;
        string DbAdmEmail;
        string AdaptedFor;
        string Subnational;
        DataTable DtTable;
        string EmailId = string.Empty;
        string UserNId = string.Empty;
        string FirstName = string.Empty;
        string Language = string.Empty;
        string typeOfEmail = string.Empty;
        DIConnection DIConnection = null;
        XmlDocument XmlDoc;

        string AppSettingFile = string.Empty;
        AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings[Constants.WebConfigKey.AppSettingFile]);
        XmlDoc = new XmlDocument();
        XmlDoc.Load(AppSettingFile);
        Global.CheckAppSetting(XmlDoc, Constants.AppSettingKeys.Country, string.Empty);
        string Country = Global.Country;
        if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
        {
            BaseUrl = Global.GetAdaptationUrl();
            AdaptationName = Global.adaptation_name;
            DbAdmName = Global.DbAdmName;
            DbAdmInstitution = Global.DbAdmInstitution;
            DbAdmEmail = Global.DbAdmEmail;
            AdaptedFor = Global.adapted_for;
            Subnational = Global.sub_nation;
            DIWorldwide.Catalog CatalogService = new DIWorldwide.Catalog();
            CatalogService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.CatalogWebService;
            GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            DataSet dsCatalogAdaptation = new DataSet();
            dsCatalogAdaptation = CatalogService.CatalogExists(Global.GetAdaptationGUID());
            string DateCreated = string.Empty;
            string AreaNId = Global.area_nid;
            string Visible = string.Empty;
            if (dsCatalogAdaptation.Tables[0].Rows.Count == 0)
            {
                typeOfEmail = "New";
                DateCreated = String.Format("{0:r}", DateTime.Now);
            }
            else
            {
                typeOfEmail = "Updated";
                DateCreated = String.Format("{0:r}", Convert.ToDateTime(dsCatalogAdaptation.Tables[0].Rows[0][1]));
                Visible = dsCatalogAdaptation.Tables[0].Rows[0][2].ToString();
            }
            RetVal = GlobalUserWebService.IsAdminRegistered(BaseUrl, AdaptationName, DbAdmName, DbAdmInstitution, DbAdmEmail, AdaptedFor, Country, Subnational, AreaNId, "", "", Global.GetAdaptationGUID());
            if (typeOfEmail == "New")
            {
                Frame_Message_And_Send_Catalog_Mail(AdaptationName, BaseUrl, Visible, DbAdmName, DbAdmInstitution, DbAdmEmail, AdaptedFor, Country, Subnational, DateCreated, String.Format("{0:r}", DateTime.Now), typeOfEmail);
            }
            if (RetVal == "TrueMA")
            {
                DataSet GlobalUserDetail = GlobalUserWebService.GetMasterAccountDetail(Global.GetAdaptationGUID());
                UserNId = GlobalUserDetail.Tables[0].Rows[0][0].ToString();
                EmailId = GlobalUserDetail.Tables[0].Rows[0][1].ToString();
                FirstName = GlobalUserDetail.Tables[0].Rows[0][2].ToString();
                Language = "en";
                if (typeOfEmail == "New")
                {
                    Frame_Message_And_Send_Mail(FirstName, EmailId, UserNId.ToString(), true, true, Language);
                }
                RetVal = "True";
            }

        }
        else
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                         string.Empty, string.Empty);
            string Query = "SELECT * FROM Users WHERE USER_IS_ADMIN='True'";

            DtTable = DIConnection.ExecuteDataTable(Query);
            if (DtTable != null && DtTable.Rows.Count > 0)
            {
                RetVal = "True";
            }
        }
        return RetVal;
    }

    public string GetAllUsersHTMLForAdmin()
    {
        string RetVal;
        int Counter;
        diworldwide_userinfo.UserLoginInformation Service;
        DIConnection DIConnection = null;
        DataTable DtUsers = null;

        RetVal = string.Empty;
        Counter = 0;

        try
        {
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                Service = new diworldwide_userinfo.UserLoginInformation();
                Service.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
                DtUsers = Service.GetAllUsersGridHTML(Global.GetAdaptationGUID());
            }
            else
            {
                DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                          string.Empty, string.Empty);

                DtUsers = DIConnection.ExecuteDataTable("SELECT NId, User_Email_Id as Email, User_First_Name as [First Name], User_Country as [User Country], User_Is_Admin as [Admin User], User_Is_Provider as [Provider User] FROM Users Where User_Is_Admin = 'False'");
            }

            DtUsers = this.Replace_AreaNIds_With_Names(DtUsers);

            RetVal = "<table id=\"tblUsers\" style=\"width:100%;float:left\" border=\"0\" cellSpacing=\"0\" cellPadding=\"0\" class=\"roundedcorners\">";
            RetVal += "<tbody>";

            RetVal += "<tr class=\"HeaderRowStyle\">";

            RetVal += "<td class=\"HeaderColumnStyle\"><span id=\"spanEmail\"></span></td>";
            RetVal += "<td class=\"HeaderColumnStyle\"><span id=\"spanName\"></span></td>";
            RetVal += "<td class=\"HeaderColumnStyle\"><span id=\"spanCountry\"></span></td>";
            RetVal += "<td class=\"HeaderColumnStyle\"><span id=\"spanProvider\"></span></td>";

            RetVal += "</tr>";

            foreach (DataRow DrUsers in DtUsers.Rows)
            {
                Counter++;

                if (Counter % 2 == 0)
                {
                    RetVal += "<tr class=\"SelectedDataRowStyle\">";
                }
                else
                {
                    RetVal += "<tr class=\"DataRowStyle\">";
                }

                RetVal += "<td class=\"DataColumnStyle\">" + DrUsers["Email"].ToString() + "</td>";
                RetVal += "<td class=\"DataColumnStyle\">" + DrUsers["First Name"].ToString() + "</td>";
                RetVal += "<td class=\"DataColumnStyle\">" + DrUsers["User Country"].ToString() + "</td>";

                if (DrUsers["Provider User"].ToString().ToLower() == "true")
                {
                    RetVal += "<td class=\"CheckBoxDataColumnStyle\"><input type=\"checkbox\" id=\"chkUser_" + DrUsers["NId"].ToString() + "\" value=\"" + DrUsers["NId"].ToString() + "\" checked=\"checked\"/></td>";
                }
                else
                {
                    RetVal += "<td class=\"CheckBoxDataColumnStyle\"><input type=\"checkbox\" id=\"chkUser_" + DrUsers["NId"].ToString() + "\" value=\"" + DrUsers["NId"].ToString() + "\"/></td>";
                }

                RetVal += "</tr>";
            }

            RetVal += "</tbody>";
            RetVal += "</table>";
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
        finally
        {
        }

        return RetVal;
    }

    public string UpdateUsers(string requestParam)
    {
        string RetVal;
        string[] Params, UserDetailsParams;
        string UserNId, Language;
        string UserDetails, EmailId, FirstName, LastName, Country, Password;
        bool IsDataProvider, IsAdmin, SendDevInfoUpdates, IsDataProviderNew;

        RetVal = string.Empty;
        UserNId = string.Empty;
        Language = string.Empty;
        UserDetails = string.Empty;
        EmailId = string.Empty;
        FirstName = string.Empty;
        LastName = string.Empty;
        Country = string.Empty;
        Password = string.Empty;
        IsDataProvider = false;
        IsAdmin = false;
        SendDevInfoUpdates = false;
        IsDataProviderNew = false;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            Language = Params[0];
            GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            for (int i = 1; i < Params.Length; i++)
            {
                if (!string.IsNullOrEmpty(Params[i]))
                {
                    UserNId = Params[i].Split('|')[0].ToString();
                    IsDataProviderNew = Convert.ToBoolean(Params[i].Split('|')[1].ToString());

                    UserDetails = this.GetUserDetails(UserNId);
                    UserDetailsParams = Global.SplitString(UserDetails, Constants.Delimiters.ParamDelimiter);
                    EmailId = UserDetailsParams[0].ToString().Trim();
                    Password = UserDetailsParams[1].ToString().Trim();
                    FirstName = UserDetailsParams[2].ToString().Trim();
                    LastName = UserDetailsParams[3].ToString().Trim();
                    Country = UserDetailsParams[4].ToString().Trim();
                    IsDataProvider = bool.Parse(UserDetailsParams[5].ToString().Trim());
                    IsAdmin = bool.Parse(UserDetailsParams[6].ToString().Trim());
                    SendDevInfoUpdates = bool.Parse(UserDetailsParams[7].ToString().Trim());

                    if (IsDataProvider != IsDataProviderNew)
                    {
                        if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
                        {
                            GlobalUserWebService.Update_In_User_Table(UserNId, EmailId, Password, FirstName, string.Empty, Country, IsDataProviderNew, IsAdmin, SendDevInfoUpdates, Global.GetAdaptationGUID());
                        }
                        else
                        {
                            this.Update_In_User_Table(UserNId, EmailId, Password, FirstName, string.Empty, Country, IsDataProviderNew, IsAdmin, SendDevInfoUpdates);
                        }

                        if (IsDataProvider == false && IsDataProviderNew == false)
                        {
                            this.Update_DataConsumer_Artefact(UserNId, FirstName + " " + LastName, Language);
                        }
                        else if (IsDataProvider == false && IsDataProviderNew == true)
                        {
                            this.Convert_Consumer_To_Provider(UserNId, FirstName + " " + LastName, Language);
                        }
                        else if (IsDataProvider == true && IsDataProviderNew == false)
                        {
                            this.Convert_Provider_To_Consumer(UserNId, FirstName + " " + LastName, Language);
                        }
                        else if (IsDataProvider == true && IsDataProviderNew == true)
                        {
                            this.Update_DataProvider_Artefact(UserNId, FirstName + " " + LastName, Language);
                        }

                        this.Frame_Message_And_Send_Mail(FirstName, EmailId, UserNId, IsAdmin, IsDataProviderNew, Language);
                    }
                }
            }

            RetVal = "true";
        }
        catch (Exception ex)
        {
            RetVal = "false";
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string AdminLogout()
    {
        string RetVal = "False";
        if (Session[Constants.SessionNames.LoggedAdminUserNId] != null)
        {
            Session[Constants.SessionNames.LoggedAdminUserNId] = null;
            Session[Constants.SessionNames.LoggedAdminUser] = null;
            RetVal = "True";
        }
        return RetVal;
    }

    public string RequestAdminForDataProviderRights(string requestParam)
    {
        string[] Params;
        string RetVal;
        string AdminNId, UserNId, Language;
        string[] AdminDetails, UserDetails;

        RetVal = string.Empty;
        AdminNId = string.Empty;
        UserNId = string.Empty;
        AdminDetails = null;
        UserDetails = null;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            AdminNId = this.Get_AdminNId();
            UserNId = Params[0];
            Language = Params[1];
            AdminDetails = Global.SplitString(this.GetUserDetails(AdminNId), Constants.Delimiters.ParamDelimiter);
            UserDetails = Global.SplitString(this.GetUserDetails(UserNId), Constants.Delimiters.ParamDelimiter);

            this.Frame_Message_And_Send_Mail_DPRight(AdminDetails[2], AdminDetails[0], UserDetails[2], UserDetails[0], Language, UserNId);

            RetVal = "true";
        }
        catch (Exception ex)
        {
            RetVal = "false";
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string ActiveAccount(string requestParam)
    {
        DataTable TokenInfoDT = null;
        DIConnection DIConnection;
        string Query;
        string RetVal = "false";
        DIConnection = null;
        string TokenKey = string.Empty;
        string EmailId = string.Empty;
        bool isRegistration;
        DataTable UserDT = null;
        string UserNid = string.Empty;
        string Name = string.Empty;
        DateTime TimeStamp;
        double ExpireTime = 24;
        try
        {
            string[] ParamArray = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            EmailId = ParamArray[0].Trim();
            TokenKey = ParamArray[1].Trim();
            isRegistration = Convert.ToBoolean(ParamArray[2].Trim());
            GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            // Global Login Flow
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                RetVal = GlobalUserWebService.ActivateAccount(HttpUtility.UrlDecode(EmailId), TokenKey, isRegistration, Global.GetAdaptationGUID());
            }
            else
            {
                UserDT = this.Get_User(EmailId);
                if (UserDT != null && UserDT.Rows.Count > 0)
                {
                    UserNid = UserDT.Rows[0]["nid"].ToString();
                    Name = UserDT.Rows[0]["user_first_name"].ToString();
                    DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                                  string.Empty, string.Empty);
                    Query = "SELECT * FROM TokenInformation WHERE TokenKey = '" + TokenKey + "' and IsRegistration=" + isRegistration + ";";
                    TokenInfoDT = DIConnection.ExecuteDataTable(Query);
                    if (TokenInfoDT != null && TokenInfoDT.Rows.Count > 0)
                    {
                        string TS = TokenInfoDT.Rows[0]["createdtime"].ToString();
                        TimeStamp = Convert.ToDateTime(TS);
                        TimeSpan TimeSpan = DateTime.Now.Subtract(TimeStamp);
                        if (TimeSpan.TotalHours <= ExpireTime)
                        {
                            RetVal = "false" + Constants.Delimiters.ParamDelimiter + "0" + Constants.Delimiters.ParamDelimiter + UserNid;// +Constants.Delimiters.ParamDelimiter + Name;
                            if (isRegistration)
                            {
                                Query = "DELETE FROM TokenInformation WHERE User_Nid = " + UserNid + "and IsRegistration=" + isRegistration + ";";
                                DIConnection.ExecuteDataTable(Query);
                            }
                        }
                        else
                        {
                            RetVal = "false" + Constants.Delimiters.ParamDelimiter + "1";
                        }
                    }
                    else
                    {
                        RetVal = "true" + Constants.Delimiters.ParamDelimiter + "2";
                    }
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + "3";
                }
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }

        return RetVal;
    }

    public string ChangePassword(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string UserNid, Password, EncryptedPassword, Query, IsRegistration, OldPwd = string.Empty, EmailId, UserName, UserDetailsStr, Language;
        DIConnection DIConnection;
        DataTable DtUser, DtTokenInfo;
        string[] UserDetails = null;

        ///Variables for creating XLSLogfile 
        string XLSFileMsg = string.Empty;
        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            UserNid = Params[0].ToString().Trim();
            Password = Params[1].ToString().Trim();
            IsRegistration = Params[2].ToString().Trim();
            if (Params[3].ToString().Trim() != "null")
                OldPwd = Params[3].ToString().Trim();
            Language = Params[4].ToString().Trim();
            EncryptedPassword = Global.OneWayEncryption(Password);
            DIConnection = null;
            GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            // Global Flow
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                if (!string.IsNullOrEmpty(OldPwd))
                {
                    if (GlobalUserWebService.IsValidUser(UserNid))
                    {
                        if (GlobalUserWebService.IsMatchOldPassword(UserNid, Global.OneWayEncryption(OldPwd), Global.GetAdaptationGUID()) == false)
                        {
                            RetVal = "false" + Constants.Delimiters.ParamDelimiter + "0";
                            return RetVal;
                        }
                    }
                }
                RetVal = GlobalUserWebService.ChangePassword(UserNid, EncryptedPassword, Convert.ToBoolean(IsRegistration), Global.GetAdaptationGUID());
                UserDetailsStr = GlobalUserWebService.GetUserDetails(UserNid, Global.GetAdaptationGUID());
                UserDetails = UserDetailsStr.Split(new string[] { Constants.Delimiters.ParamDelimiter }, StringSplitOptions.None);
                EmailId = UserDetails[0];
                UserName = UserDetails[2];
                Frame_UpdateMessage_And_Send_Mail(UserName, EmailId, Language);

                #region "Call method to write log in csv file"
                XLSFileMsg = string.Format(Constants.CSVLogMessage.ChangePassword, UserName);
                WriteLogInXLSFile(Constants.AdminModules.UserManagement.ToString(), XLSFileMsg);
                #endregion
            }
            else // Local Flow
            {
                if (!string.IsNullOrEmpty(OldPwd))
                {
                    DtUser = this.Get_User(Int16.Parse(UserNid));
                    if (DtUser != null && DtUser.Rows.Count > 0)
                    {
                        if (!Global.OneWayEncryption(OldPwd).Equals(DtUser.Rows[0]["User_Password"].ToString()))
                        {
                            RetVal = "false" + Constants.Delimiters.ParamDelimiter + "0";
                            return RetVal;
                        }
                    }
                }
                DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                              string.Empty, string.Empty);
                DtUser = this.Get_User(Convert.ToInt32(UserNid));
                if (DtUser != null && DtUser.Rows.Count > 0)
                {
                    Query = "Update Users set user_password = '" + EncryptedPassword + "' where nid = " + UserNid + ";";
                    DIConnection.ExecuteDataTable(Query);
                    if (!string.IsNullOrEmpty(IsRegistration))
                    {
                        Query = "SELECT * FROM TokenInformation WHERE User_Nid = " + UserNid + "and IsRegistration=" + IsRegistration + ";";
                        DtTokenInfo = DIConnection.ExecuteDataTable(Query);
                        if (DtTokenInfo != null && DtTokenInfo.Rows.Count > 0)
                        {
                            Query = "DELETE FROM TokenInformation WHERE User_Nid = " + UserNid + "and IsRegistration=" + IsRegistration + ";";
                            DIConnection.ExecuteDataTable(Query);
                        }
                    }
                    RetVal = "true" + Constants.Delimiters.ParamDelimiter + "1" + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["nid"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_First_Name"].ToString() + Constants.Delimiters.ParamDelimiter + DtUser.Rows[0]["User_Is_Admin"].ToString();
                    Frame_UpdateMessage_And_Send_Mail(DtUser.Rows[0]["User_First_Name"].ToString(), DtUser.Rows[0]["User_Email_Id"].ToString(), Language);
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + "2";
                }
            }
            if (RetVal == "true")
            {

            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + "3";
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string RegenerateActivationLink(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string Name, EmailId, Password, EncryptedPassword, Language;
        bool IsDataview;
        DataTable DtUser;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            EmailId = Params[0].ToString().Trim();
            IsDataview = Convert.ToBoolean(Params[1].ToString().Trim());
            Language = (Params[2].ToString().Trim());
            Password = Guid.NewGuid().ToString().Substring(0, 8);
            EncryptedPassword = Global.OneWayEncryption(Password);
            GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            // Global Flow
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                if (GlobalUserWebService.Is_Existing_User(EmailId))
                {
                    Name = GlobalUserWebService.Get_UserName(EmailId, Global.GetAdaptationGUID());
                    string UserNid = GlobalUserWebService.Get_UserNid(EmailId, Global.GetAdaptationGUID());
                    bool IsAdmin = GlobalUserWebService.IsUserAdmin(UserNid, Global.GetAdaptationGUID());
                    string TokenKey = GlobalUserWebService.Save_In_TokenInformation(UserNid, true, IsDataview);
                    this.Frame_TokenMessage_And_Send_Mail(Name, EmailId, IsAdmin, TokenKey, true, Language, IsDataview);
                    RetVal = "true" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("MailSent");
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("IDNotRegistered");
                }
            }
            else // Local Flow
            {
                DtUser = this.Get_User(EmailId);

                if (DtUser != null && DtUser.Rows.Count > 0)
                {

                    Name = DtUser.Rows[0]["User_First_Name"].ToString();
                    string TokenKey = Save_In_TokenInformation(DtUser.Rows[0]["nid"].ToString(), true, IsDataview);
                    bool IsAdmin = this.isUserAdmin(DtUser.Rows[0]["nid"].ToString());
                    this.Frame_TokenMessage_And_Send_Mail(Name, EmailId, IsAdmin, TokenKey, true, Language, IsDataview);
                    RetVal = "true" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("MailSent");
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("IDNotRegistered");
                }
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    public string RegenrateForgotPasswordLink(string requestParam)
    {
        string RetVal = string.Empty;
        string[] Params;
        string Name, EmailId, Password, EncryptedPassword, Language;
        bool IsDataview;
        DataTable DtUser;

        try
        {
            Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
            EmailId = Params[0].ToString().Trim();
            IsDataview = Convert.ToBoolean(Params[1].ToString().Trim());
            Language = (Params[2].ToString().Trim());
            Password = Guid.NewGuid().ToString().Substring(0, 8);
            EncryptedPassword = Global.OneWayEncryption(Password);
            GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
            // Global Flow
            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {
                if (GlobalUserWebService.Is_Existing_User(EmailId))
                {
                    Name = GlobalUserWebService.Get_UserName(EmailId, Global.GetAdaptationGUID());
                    string UserNid = GlobalUserWebService.Get_UserNid(EmailId, Global.GetAdaptationGUID());
                    bool IsAdmin = GlobalUserWebService.IsUserAdmin(UserNid, Global.GetAdaptationGUID());
                    string TokenKey = GlobalUserWebService.Save_In_TokenInformation(UserNid, false, IsDataview);
                    this.Frame_TokenMessage_And_Send_Mail(Name, EmailId, IsAdmin, TokenKey, false, Language, IsDataview);
                    RetVal = "true" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("MailSent");
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("IDNotRegistered");
                }
            }
            else // Local Flow
            {
                DtUser = this.Get_User(EmailId);

                if (DtUser != null && DtUser.Rows.Count > 0)
                {

                    Name = DtUser.Rows[0]["User_First_Name"].ToString();
                    string TokenKey = Save_In_TokenInformation(DtUser.Rows[0]["nid"].ToString(), false, IsDataview);
                    bool IsAdmin = this.isUserAdmin(DtUser.Rows[0]["nid"].ToString());
                    this.Frame_TokenMessage_And_Send_Mail(Name, EmailId, IsAdmin, TokenKey, false, Language, IsDataview);
                    RetVal = "true" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("MailSent");
                }
                else
                {
                    RetVal = "false" + Constants.Delimiters.ParamDelimiter + Global.GetLanguageKeyValue("IDNotRegistered");
                }
            }
        }
        catch (Exception ex)
        {
            RetVal = "false" + Constants.Delimiters.ParamDelimiter + ex.Message;
            Global.CreateExceptionString(ex, null);

        }
        finally
        {
        }

        return RetVal;
    }

    private string DeleteExisting_TokenInformation(string UserNid, bool IsRegistration, bool isDataview)
    {
        string Query;
        DIConnection DIConnection;

        Query = string.Empty;
        DIConnection = null;
        string TokeyKey = string.Empty;
        string TimeStamp = string.Empty;
        DataTable TokeInfoTable = null;
        try
        {
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);

            Query = "Select * from TokenInformation where User_Nid = " + UserNid + " and IsRegistration = " + IsRegistration + " and IsDataviewFlow=" + isDataview + ";";
            TokeInfoTable = DIConnection.ExecuteDataTable(Query);
            if (TokeInfoTable != null && TokeInfoTable.Rows.Count > 0)
            {
                Query = "delete from TokenInformation where User_Nid = " + UserNid + " and IsRegistration = " + IsRegistration + " and IsDataviewFlow=" + isDataview;
                DIConnection.ExecuteDataTable(Query);
            }

        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);

            throw ex;
        }
        finally
        {
            if (DIConnection != null)
            {
                DIConnection.Dispose();
            }
        }
        return TokeyKey;
    }

    private bool CheckForMasterAdmin(string UserName, string Password)
    {
        DataSet RetVal;
        GlobalUserWebService.Url = ConfigurationManager.AppSettings[Constants.WebConfigKey.DiWorldWide4] + Constants.WSQueryStrings.UserLoginService;
        RetVal = GlobalUserWebService.GetMasterAccountEmailPassword();
        if (RetVal.Tables[0].Rows.Count > 0)
        {
            if ((RetVal.Tables[0].Rows[0][0].ToString().ToLower() == UserName.ToLower()) && (RetVal.Tables[0].Rows[0][1].ToString() == Password))
            {
                return true;
            }
            else
                return false;
        }
        else
            return false;

    }
    #endregion "--Public--"

    #endregion "--Methods--"




}
