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
using DILibSDMX = DevInfo.Lib.DI_LibSDMX;

public partial class libraries_aspx_Admin_AdminMaster : System.Web.UI.MasterPage
{
    #region -- Private --

    #region -- Methods --


    #endregion

    #endregion

    #region -- Public/Protected --

    #region -- Methods/Events --
    public string LogfilePath = string.Empty;
    private string IsHeaderCreated = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        LogfilePath = "../../../" + (System.IO.Path.Combine(Constants.FolderName.CSVLogPath.ToString(), Constants.FileName.XLSLogFileName.ToString()) + ".xls");             
        try
        {
            if (Session[Constants.SessionNames.LoggedAdminUser] != null)
            {
                this.liUser.InnerHtml = Session[Constants.SessionNames.LoggedAdminUser].ToString();
                this.liAgencyIdVal.InnerHtml = DILibSDMX.Constants.MaintenanceAgencyScheme.Prefix + Session[Constants.SessionNames.LoggedAdminUserNId].ToString();
                this.liProviderIDVal.InnerHtml = DILibSDMX.Constants.DataProviderScheme.Prefix + Session[Constants.SessionNames.LoggedAdminUserNId].ToString();
            }

            if (ConfigurationManager.AppSettings[Constants.WebConfigKey.IsGlobalAllow].ToLower() == "true")
            {

                LiUserCaption.Visible = false;
            }
            else
            {
                //if global allow is false hide user management tab in the admin panel
                LiAdaptationUser.Visible = false;
                LiUserCaption.Visible = true;
            }


            if (Global.CheckIsGlobalAdaptation())
            {
                LiCatalogCaption.Visible = false;
                // LiAdaptationUser.Visible = true;

            }
            else
            {
                LiCatalogCaption.Visible = false;
                // LiAdaptationUser.Visible = false;

            }
            string isMasterAccount = Global.CheckIfMasterAccount(Session[Constants.SessionNames.LoggedAdminUserNId].ToString());
            if (isMasterAccount == "true")
            {
                this.liUser.Style.Add("display", "none");
                this.liUserPipeline.Style.Add("display", "none");

            }
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
        }

    protected void LogoutAdmin(object sender, EventArgs e)
    {
        Session[Constants.SessionNames.LoggedAdminUserNId] = null;
        Session[Constants.SessionNames.LoggedAdminUser] = null;
        Response.Redirect("AdminLogin.aspx");
    }


    #endregion

    #endregion
}
