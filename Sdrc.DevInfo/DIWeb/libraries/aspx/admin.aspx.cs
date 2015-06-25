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
using System.IO;
using System.Xml;
using System.Collections.Generic;

public partial class libraries_aspx_admin_admin : System.Web.UI.Page
{
    #region "--Private--"

    #region "--Methods--"

    private void GetPostedData()
    {
        try
        {
            // Get Posted Data - will be passed to the Javascript        
            if (!string.IsNullOrEmpty(Request["hlngcode"])) 
            { 
                hlngcode = Request["hlngcode"];
                Session["hlngcode"] = Request["hlngcode"];
            }
            else
                hlngcode = Session["hlngcode"] != null ? Session["hlngcode"].ToString() : null;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    /// <summary>
    /// Get configurations from appsettings.xml file and set into controls
    /// </summary>
    private void SetConfigurationDetails()
    {
        string AppSettingFile = string.Empty;
        XmlDocument XmlDoc;

        try
        {
            AppSettingFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings["AppSettingFile"]);

            XmlDoc = new XmlDocument();
            XmlDoc.Load(AppSettingFile);

            txtAdaptationName.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.adaptation_name);
            txtVersion.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.diuilib_version);
            txtDiUiLibUrl.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.diuilib_url);
            txtDiUiLibThemCss.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.diuilib_theme_css);
            txtFBAppID.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.fbAppID);
            txtFBAppSecret.Text = Global.GetNodeValue(XmlDoc, Constants.AppSettingKeys.fbAppSecret);
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void FillCategories()
    {
        string DBFile = string.Empty;
        XmlDocument XmlDoc;
        XmlNodeList ObjXmlNodeList;
        int i = 1;
        string SelectedCategory = string.Empty;

        try
        {
            DBFile = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, ConfigurationManager.AppSettings["DBConnectionsFile"]);
            XmlDoc = new XmlDocument();
            XmlDoc.Load(DBFile);

            ObjXmlNodeList = XmlDoc.SelectNodes("/dbinfo/" + "child::node()");

            cmbCategory.Items.Clear();
            cmbCategory.Items.Insert(0, new ListItem("-- Select Category --","-1"));

            foreach (XmlNode data in ObjXmlNodeList)
            {
                cmbCategory.Items.Insert(i, new ListItem(data.Attributes["name"].Value));
                i++;
            }

            SelectedCategory = Global.GetSelectedCategoryName(Global.GetDefaultDbNId());

            cmbCategory.SelectedValue = SelectedCategory;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void FillDatabaseType()
    {
        try
        {
            cmbDatabaseType.Items.Clear();
            cmbDatabaseType.Items.Insert(0, new ListItem("-- Select Setver Type --", "-1"));
            cmbDatabaseType.Items.Insert(1, new ListItem("SqlServer", "0"));
            cmbDatabaseType.Items.Insert(2, new ListItem("MsAccess", "1"));
            cmbDatabaseType.Items.Insert(3, new ListItem("MySql", "3"));
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void FillAvilableConnections()
    {
        Dictionary<string, string> ConnDetails = new Dictionary<string, string>();
        int i = 1;        

        try
        {
            cmbAvilableConn.Items.Clear();
            cmbAvilableConn.Items.Insert(0, new ListItem("-- Select Connection --","-1"));         

            ConnDetails = Global.GetAllConnections(cmbCategory.SelectedValue);

            foreach (KeyValuePair<string, string> Data in ConnDetails)
            {
                cmbAvilableConn.Items.Insert(i, new ListItem(Data.Value, Data.Key));
                i++;
            }
                        
            //Select db in list            
            cmbAvilableConn.SelectedValue = hdefdbnid;
                        
            this.SetDbConnectionDetailsValues(hdefdbnid);

            //Checked default database checkbox
            chkDefault.Checked = true;
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }

    private void SetDbConnectionDetailsValues(string dbNId)
    {
        string[] DBDetails;
        string[] DBConnectionDetails;

        try
        {
            //Get connection details of database
            DBDetails = Global.GetDbConnectionDetails(dbNId);

            //Set db values on controls
            txtConnName.Text = DBDetails[0];

            DBConnectionDetails = Global.SplitString(DBDetails[1], "||");
            cmbDatabaseType.SelectedValue = DBConnectionDetails[0];
            txtServerName.Text = DBConnectionDetails[1];
            txtDatabaseName.Text = DBConnectionDetails[2];
            txtUserName.Text = DBConnectionDetails[3];

            if (DBConnectionDetails.Length > 4)
            {
                txtPassword.Text = DBConnectionDetails[4];
            }

            txtDefaultArea.Text = DBDetails[2];
            txtDesc.Text = DBDetails[3];
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

    protected string hlngcode = string.Empty;
    protected string hdefdbnid = string.Empty;    

    #endregion

    #region "--Methods/Events--"

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {            
            //Get default dbNId from db.xml file
            hdefdbnid = Global.GetDefaultDbNId();

            // Read AppSettings
            Global.GetAppSetting();

            // Set page title
            Page.Title = Global.adaptation_name;

            // Read http header or cookie values
            this.GetPostedData();

            // Get configurations from appsettings.xml file and set into controls
            this.SetConfigurationDetails();

            // Set database connections values
            this.FillCategories();
            this.FillDatabaseType();
            this.FillAvilableConnections();
        }
        catch (Exception ex)
        {
            Global.CreateExceptionString(ex, null);
        }
    }
    
    //protected void btnTestConnection_Click(object sender, EventArgs e)
    //{

    //}
        
    protected void txtUserName_TextChanged(object sender, EventArgs e)
    {

    }
    
    //protected void btnRegisterDatabase_Click(object sender, EventArgs e)
    //{

    //}
    
    //protected void btnUnregister_Click(object sender, EventArgs e)
    //{

    //}

    #endregion

    #endregion
}
