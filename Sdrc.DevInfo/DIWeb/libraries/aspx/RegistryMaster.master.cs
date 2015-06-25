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
using DevInfo.Lib.DI_LibDAL.Connection;
using DevInfo.Lib.DI_LibSDMX;

public partial class libraries_aspx_RegistryMaster : System.Web.UI.MasterPage
{
    #region "--Variables--"

    #region "--Private--"

    #endregion "--Private--"

    #region "--Public--"

    #endregion "--Public--"

    #endregion "--Variables--"

    #region "--Constructors--"

    #region "--Private--"

    #endregion "--Private--"

    #region "--Public--"

    #endregion "--Public--"

    #endregion "--Constructors--"

    #region "--Methods--"

    #region "--Private--"

    #endregion "--Private--" 

    #region "--Public--"

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }

    public void Populate_Select_DSD_DropDown(string HDBNId)
    {
        string Query;
        string DBNId, Id, AgencyId, Version, Name;
        bool SelectedFlag;
        DataTable DtTable;
        DIConnection DIConnection;

        Query = string.Empty;
        DBNId = string.Empty;
        Id = string.Empty;
        AgencyId = string.Empty;
        Version = string.Empty;
        Name = string.Empty;
        SelectedFlag = false;
        DtTable = null;
        DIConnection = null;

        try
        {        
            DIConnection = new DIConnection(DIServerType.MsAccess, string.Empty, string.Empty, Server.MapPath("~//stock//Database.mdb"),
                           string.Empty, string.Empty);
            Query = "SELECT DISTINCT DBNId, Id, AgencyId, Version FROM Artefacts WHERE DBNId <> -1 AND Type = " + Convert.ToInt32(ArtefactTypes.DSD).ToString() + ";";

            DtTable = DIConnection.ExecuteDataTable(Query);

            foreach (DataRow DrTable in DtTable.Rows)
            {
                DBNId = DrTable["DBNId"].ToString();
                Id = DrTable["Id"].ToString();
                AgencyId = DrTable["AgencyId"].ToString();
                Version = DrTable["Version"].ToString();
                Name = Global.GetDbConnectionDetails(DBNId)[0];

                this.selectDSDInUse.Items.Add(new ListItem(Id + "[" + AgencyId + ", " + Version + "] - " + Name, DBNId));
                this.selectDSDInUse.Items[this.selectDSDInUse.Items.Count - 1].Attributes.Add("IsUploadedDSD", Global.IsDSDUploadedFromAdmin(Convert.ToInt32(DBNId)).ToString());

                if (DBNId == HDBNId && Global.GetDefaultDSDNId() != string.Empty)
                {
                    this.selectDSDInUse.Items[this.selectDSDInUse.Items.Count - 1].Selected = true;

                    SelectedFlag = true;
                }
                else if (DBNId == HDBNId &&  Global.GetDbXmlAttributeValue(DBNId, "sdmxdb") == "true")
                {
                    this.selectDSDInUse.Items[this.selectDSDInUse.Items.Count - 1].Selected = true;

                    SelectedFlag = true;
                }
                //else
                //{
                //    //if( Global.GetDefaultDSDNId() != string.Empty)
                //    //    this.selectDSDInUse.Items.FindByValue(HDBNId).Selected = true;
                //    if (HDBNId == Global.GetDefaultDSDNId())
                //    {
                //        this.selectDSDInUse.Items[this.selectDSDInUse.Items.Count - 1].Selected = true;

                //        SelectedFlag = true;
                //    }
                //}

                //if (DBNId == Global.GetDefaultDSDNId())
                //{
                //    this.selectDSDInUse.Items[this.selectDSDInUse.Items.Count - 1].Selected = true;

                //    SelectedFlag = true;
                //}
              
               // if (DBNId == HDBNId && AgencyId != "UNSD")
                if (DBNId == HDBNId && Id.Contains("DevInfo") && Global.GetDbXmlAttributeValue(DBNId, "sdmxdb") == "false")// && Global.GetDefaultDSDNId() == string.Empty
                {
                    this.selectDSDInUse.Items[this.selectDSDInUse.Items.Count - 1].Attributes.Add("class", "Database");
                }
            }

            if (SelectedFlag == false)
            {
                if (this.selectDSDInUse.Items.Count > 0)
                {
                    this.selectDSDInUse.Items[0].Selected = true;
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
    }

    public void ShowHide_Select_DSD_DropDown(string enableDSDSelection)
    {
        if (enableDSDSelection == "true")
        {
            this.spanRegistry.Visible = false;
            this.spanDSDInUse.Visible = true;
            this.selectDSDInUse.Visible = true;
            this.iDSDInUse.Visible = true;
        }
        else
        {
            this.spanRegistry.Visible = true;
            this.spanDSDInUse.Visible = false;
            this.selectDSDInUse.Visible = false;
            this.iDSDInUse.Visible = false;
        }
    }

    #endregion "--Public--"

    #endregion "--Methods--"
}
