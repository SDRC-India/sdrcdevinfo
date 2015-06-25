using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class libraries_aspx_Admin_DbEdit1 : System.Web.UI.Page
{
    protected string hlngcode = string.Empty;
    protected string hLoggedInUserNId = string.Empty;
    protected string IsAdd = string.Empty;

    protected string msgOldPwd = string.Empty;
    protected string msgNewPwd = string.Empty;
    protected string msgCNewPwd = string.Empty;
    protected string msgPwdNotMatch = string.Empty;
    protected string ChangeDBPassword = string.Empty;
    #region "--Private--"

    #region "--Methods--"

    private void FillDatabaseType()
    {
        try
        {
            ddlDatabaseType.Items.Clear();
            ddlDatabaseType.Items.Insert(0, new ListItem("-- Select Server Type --", "-1"));
            ddlDatabaseType.Items.Insert(1, new ListItem("Sql Server", "0"));            
            //ddlDatabaseType.Items.Insert(2, new ListItem("My Sql", "3"));
            //ddlDatabaseType.Items.Insert(3, new ListItem("Firebird", "8"));
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    #endregion

    #endregion

    #region "--Public/Protected--"

    #region "--Variables--"

    #endregion

    #region "--Methods/Events--"

    protected void Page_Load(object sender, EventArgs e)
    {
        string DbId = string.Empty;
        string[] DbDetails;

        try
        {
            if (Session[Constants.SessionNames.LoggedAdminUser] == null)
            {
                Response.Redirect("AdminLogin.aspx");
            }
            else
            {
                // Read AppSettings
                Global.GetAppSetting();

                // Set page title
                Page.Title = Global.adaptation_name;
                this.GetPostedData();
                this.FillDatabaseType();

                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    DbId = Request.QueryString["id"].ToString();
                    DbDetails = Global.GetDbNConnectionDetails(DbId, "en");
                                        
                    this.txtConnName.Text = DbDetails[1];
                    this.ddlDatabaseType.SelectedValue = DbDetails[2];
                    this.txtServerName.Text = DbDetails[3];
                    this.txtDatabaseName.Text = DbDetails[4];
                    this.txtUserName.Text = DbDetails[5];
                    this.txtPassword.Attributes.Add("value", DbDetails[6]);
                    this.txtDesc.Text = DbDetails[7];

                    if (DbId == Global.GetDefaultDbNId())
                    {
                        this.chkDefault.Checked = true;
                    }

                    this.IsAdd = "false";
                    //this.chkRegCatalog.Checked = true;
                }
                else
                {                    
                    this.txtConnName.Text = "";
                    this.ddlDatabaseType.SelectedIndex = 0;
                    this.txtServerName.Text = "";
                    this.txtDatabaseName.Text = "";
                    this.txtUserName.Text = "";
                    this.txtPassword.Text = "";
                    this.txtDesc.Text = "";
                    this.chkDefault.Checked = false;
                    this.IsAdd = "true";
                }
                msgOldPwd = Global.GetLanguageKeyValue("Enter_Old_Password");
                msgNewPwd = Global.GetLanguageKeyValue("Enter_Password");
                msgCNewPwd = Global.GetLanguageKeyValue("Confirm_Password_Msg");
                msgPwdNotMatch = Global.GetLanguageKeyValue("ReEnter_Password");
                ChangeDBPassword = Global.GetLanguageKeyValue("ChangeDBPassword");
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }
    private void GetPostedData()
    {
        try
        {
            // Set language code   
            if (Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)] != null && (!string.IsNullOrEmpty(Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value)))
            {
                // then check in the cookie
                hlngcode = Request.Cookies[Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode)].Value;
            }
            else
            {
                // get default lng code
                hlngcode = Global.GetDefaultLanguageCode();
            }
            Global.SaveCookie(Global.GetCookieNameByAdapatation(Constants.CookieName.LanguageCode), hlngcode, Page);
            // Get Posted Data - will be passed to the Javascript            

            if (!string.IsNullOrEmpty(Request["hlngcode"])) { hlngcode = Request["hlngcode"]; }

           // if (!string.IsNullOrEmpty(Request["hLoggedInUserNId"])) { hLoggedInUserNId = Request["hLoggedInUserNId"]; }
            if (Session[Constants.SessionNames.LoggedAdminUserNId] != null && Session[Constants.SessionNames.LoggedAdminUserNId].ToString() != "")
            {
                hLoggedInUserNId = Session[Constants.SessionNames.LoggedAdminUserNId].ToString();
            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }
    #endregion

    #endregion
}
