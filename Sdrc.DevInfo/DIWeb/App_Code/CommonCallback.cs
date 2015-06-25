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
using System.IO;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.ComponentModel;

public partial class Callback : System.Web.UI.Page
{
    #region "--Variables--"

    private DIConnection _DBCon;

    #endregion "--Variables--"

    #region "--Constructors--"

    public Callback(Page OPage)
    {
        this.Page = OPage;
        Global.GetAppSetting();
    }

    public Callback()
    {
        //do Nothing
    }

    #endregion "--Constructors--"

    #region "--Methods--"

    private DIConnection GetDbConnection(int dbNId)
    {
        bool usingMapServer = false;//Convert.ToBoolean(isMapServer.Trim());
        if (Session["IsMapServer"] != null && Convert.ToBoolean(Session["IsMapServer"]))
        {
            usingMapServer = Convert.ToBoolean(Session["IsMapServer"]);
        }
        if (!usingMapServer)
        {
            _DBCon = Global.GetDbConnection(dbNId);
        }
        else
        {
            _DBCon = new DIConnection(Global.GetMapServerConnectionDetails());
        }
        return _DBCon;
    }

    private void Create_Directory_If_Not_Exists(string RetVal)
    {
        if (!Directory.Exists(RetVal))
        {
            Directory.CreateDirectory(RetVal);
        }
    }

    private void Create_File_If_Not_Exists(string fileNameWithPath, byte[] fileContent)
    {
        FileStream Stream = null;
        BinaryWriter Writer = null;

        try
        {
            if (!File.Exists(fileNameWithPath))
            {
                Stream = new FileStream(fileNameWithPath, FileMode.Create);
                Writer = new BinaryWriter(Stream);
                Writer.Write(fileContent);
                Writer.Close();
            }
        }
        catch (Exception ex)
        {
            if (Writer != null)
            {
                Writer.Close();
            }
            Global.CreateExceptionString(ex, null);
            throw ex;
        }
    }

    private void Send_Email(string SenderName, string SenderEmailId, string ReceiverEmailId, string Subject, string Content, bool IsHTMLBody = false, string ReceiverName = "", string RegisterationOrSubscriptionId = "", string RegisterationOrSubscription = "")
    {
        MailMessage MailMessage;
        SmtpClient SmtpClient;

        System.Net.Mail.MailAddress ToAddress = new MailAddress(ReceiverEmailId, ReceiverName);
        try
        {
            MailMessage = new MailMessage();
            SmtpClient = new SmtpClient();

            MailMessage.From = new MailAddress(SenderEmailId, SenderName);
            MailMessage.To.Add(ToAddress);
            MailMessage.IsBodyHtml = IsHTMLBody;
            MailMessage.Priority = MailPriority.Normal;
            MailMessage.Subject = Subject;
            MailMessage.Body = Content;

            SmtpClient.ServicePoint.MaxIdleTime = 2;
            SmtpClient.SendCompleted += new SendCompletedEventHandler((sender, e) => SendCompletedCallback(sender, e, RegisterationOrSubscriptionId, RegisterationOrSubscription));
            SmtpClient.SendAsync(MailMessage, null);
            //  MailMessage.Dispose();
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        finally
        {

        }
    }

    static bool mailSent = false;
    private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e, string RegisterationOrSubscriptionId, string RegisterationOrSubscription)
    {
        // Get the unique identifier for this asynchronous operation.
        String token = (string)e.UserState;

        if (e.Cancelled)
        {
            Console.WriteLine("[{0}] Send canceled.", token);
            XLSLogGenerator.WriteCSVLogForMailStatus(RegisterationOrSubscription, RegisterationOrSubscriptionId, "Send canceled", token);
        }
        if (e.Error != null)
        {
            Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            XLSLogGenerator.WriteCSVLogForMailStatus(RegisterationOrSubscription, RegisterationOrSubscriptionId, "Error Detail " + e.Error.ToString(), token);
        }
        else
        {
            Console.WriteLine("Message sent.");
            XLSLogGenerator.WriteCSVLogForMailStatus(RegisterationOrSubscription, RegisterationOrSubscriptionId, "Message sent", token);
        }
        mailSent = true;

        //CSVLogGenerator.WriteCSVLogForTesting("EMail Status", RegisterationOrSubscriptionId, token, Convert.ToString(mailSent));

    }
    public bool SessionReset()
    {
        try
        {
            Session["hLoggedInUserNId"] = null;
            Session["hLoggedInUserName"] = null;
            Session[Constants.SessionNames.LoggedAdminUserNId] = null;
            Session[Constants.SessionNames.LoggedAdminUser] = null;
            return true;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
            return false;
        }
    }
    #endregion "--Methods--"

    public string SaveHeaderDetails(string requestParam)
    {
        string RetVal = "false";
        string[] Params;
        string DbNid = string.Empty;
        bool IsDIDatabase = false;
        string UserNId = string.Empty;
        string MAInputParam =  string.Empty;
        try
        {

            HeaderDetails ObjHedDet = new HeaderDetails();
            if (ObjHedDet.SaveHeaderDetails(requestParam))
            {
                Params = Global.SplitString(requestParam, Constants.Delimiters.ParamDelimiter);
                DbNid = Params[0];
                UserNId = Params[21];

                IsDIDatabase = Convert.ToBoolean(Params[2]);
                if (IsDIDatabase)
                {
                    MAInputParam = DbNid + Constants.Delimiters.ParamDelimiter + UserNId;
                    RetVal = AdminUpdateArtifactsWithHeaderForDatabase(MAInputParam).ToString();

                }
                else
                {
                    MAInputParam = DbNid + Constants.Delimiters.ParamDelimiter + UserNId;
                    RetVal = AdminUpdateArtifactsWithHeaderForUploadedDSD(MAInputParam).ToString();
                }
               
            }
        }
        catch (Exception Ex)
        {
            RetVal = "false";
        }
        return RetVal.ToLower();
    }

    public string GetHeaderDetail(string requestParam)
    {
        string RetVal = string.Empty;
        System.Web.Script.Serialization.JavaScriptSerializer jSearializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        try
        {
            HeaderDetails ObjHedDet = new HeaderDetails();
            RetVal = ObjHedDet.GetHeaderDetail(requestParam);
        }
        catch (Exception Ex)
        {
            RetVal = string.Empty;
            Global.CreateExceptionString(Ex, null);
        }
        return RetVal;
    }
}

