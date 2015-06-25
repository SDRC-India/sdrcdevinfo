using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using System.Net;
using System.Net.Mail;
/// <summary>
/// Summary description for EmailShare
/// </summary>
public class EmailShare
{
    private string sub = string.Empty;
    private string bod = string.Empty;

	public EmailShare(string SubjectTemplate, string BodyTemplate)
	{
        sub = SubjectTemplate;
        bod = BodyTemplate;        
	}

    public bool SendTemplateEmail(string ToByComma, string AppName, string LinkShare)
    {
        try
        {
            MailMessage mail = new MailMessage(); //"DevInfo Support Group <support@devinfo.info>", "vschauhan@devinfo.info", "", "");

            mail.From = new MailAddress(ConfigurationManager.AppSettings["NotificationSenderEmailId"].ToString(), ConfigurationManager.AppSettings["NotificationSender"].ToString());

            mail.Subject = sub.Replace("$APP_NAME", AppName).Trim();
            mail.Body = bod.Replace("$APP_NAME", AppName).Replace("$SHARE_LINK", LinkShare).Trim();

            string[] allTO = ToByComma.Split(',');
           
            if (allTO.Length > 0) mail.To.Clear();
            foreach (string tmpTo in allTO)
            {
                mail.To.Add(tmpTo.Trim());
            }

            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient(); //("174.122.242.132", 2525);
            smtp.Send(mail);
            return true;
        }
        catch (Exception ex)
        {
            return false;
            Global.CreateExceptionString(ex, null);

        }
    }
}
