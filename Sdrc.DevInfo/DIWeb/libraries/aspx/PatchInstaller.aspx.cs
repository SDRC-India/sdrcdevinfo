using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class libraries_aspx_PatchInstaller : System.Web.UI.Page
{
    public string LogFilePath = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {      
        //Set the Controls UI Text and Delete the existsing log file
        SetControlsText();
        DeleteEistingLogFile();
        LogFilePath = "../../" + (System.IO.Path.Combine(Constants.FolderName.CSVLogPath.ToString(), Constants.PatchConstaints.PatchInstLogFileName.ToString()).Replace("\\", "/") + ".xls");
        if (Request.QueryString.Count > 0)
        {
            if (Request.QueryString["Src"] != null && Request.QueryString["Src"].ToString() == "1")
            {
                langInstallBtn_Patch.Visible = false;
                ClientScript.RegisterStartupScript(Page.GetType(), "OnLoad", "InstallPatch('" + LogFilePath + "');", true);
                System.IO.File.WriteAllText(Server.MapPath("wa.txt"), "text");
                langInstallBtn_Patch.Visible = false;
            }
            if (Request.QueryString["Src"] != null && Request.QueryString["Src"].ToString() == "2")
            {
                langInstallBtn_Patch.Visible = false;
            }
        }
    }
    /// <summary>
    /// Set Control UI Text
    /// </summary>
    private void SetControlsText()
    {
        if (!string.IsNullOrEmpty(PatchInstaller.ReadKeysForPatch("LangLeftLnkInstPatch")))
        { LangLeftLnkInstPatch.InnerText = PatchInstaller.ReadKeysForPatch("LangLeftLnkInstPatch"); }

        if (!string.IsNullOrEmpty(PatchInstaller.ReadKeysForPatch("lang_db_PatchMainHeading")))
        { lang_db_PatchMainHeading.InnerText = PatchInstaller.ReadKeysForPatch("lang_db_PatchMainHeading"); }

        if (!string.IsNullOrEmpty(PatchInstaller.ReadKeysForPatch("lang_db_Patch_subHead")))
        { lang_db_Patch_subHead.InnerText = PatchInstaller.ReadKeysForPatch("lang_db_Patch_subHead"); }

        if (!string.IsNullOrEmpty(PatchInstaller.ReadKeysForPatch("langInstallingPatch")))
        { langInstallingPatch.InnerText = PatchInstaller.ReadKeysForPatch("langInstallingPatch"); }

        if (!string.IsNullOrEmpty(PatchInstaller.ReadKeysForPatch("langUpdLanguage_Patch")))
        { langUpdLanguage_Patch.InnerText = PatchInstaller.ReadKeysForPatch("langUpdLanguage_Patch"); }

        if (!string.IsNullOrEmpty(PatchInstaller.ReadKeysForPatch("langInstallBtn_Patch")))
        { langInstallBtn_Patch.Value = PatchInstaller.ReadKeysForPatch("langInstallBtn_Patch"); }

        if (!string.IsNullOrEmpty(PatchInstaller.ReadKeysForPatch("LangPatchInstSuccess")))
        { LangPatchInstSuccess.Value = PatchInstaller.ReadKeysForPatch("LangPatchInstSuccess"); }

        if (!string.IsNullOrEmpty(PatchInstaller.ReadKeysForPatch("LangPatchInstFailed")))
        { LangPatchInstFailed.Value = PatchInstaller.ReadKeysForPatch("LangPatchInstFailed"); }

        if (!string.IsNullOrEmpty(PatchInstaller.ReadKeysForPatch("langUpdatingAppSet_Patch")))
        { langUpdatingAppSet_Patch.InnerText = PatchInstaller.ReadKeysForPatch("langUpdatingAppSet_Patch"); }

        if (!string.IsNullOrEmpty(Global.GetLanguageKeyValue("Lang_aLogFile")))
        { aLogFile.InnerText = Global.GetLanguageKeyValue("Lang_aLogFile"); }
    }

    /// <summary>
    /// Delete Existing Log file
    /// </summary>
    private void DeleteEistingLogFile()
    {
        string directryPath = string.Empty;
        string FileName = string.Empty;
        string FilePath = string.Empty;
        //Get Directort Path
        directryPath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, Constants.FolderName.CSVLogPath);
        // Get file Name
        FileName = Constants.PatchConstaints.PatchInstLogFileName;
        // Get a FilePath
        FilePath = Path.Combine(HttpContext.Current.Request.PhysicalApplicationPath, directryPath, FileName) + ".xls";

        // if file is already existing then delete existion file
        if (File.Exists(FilePath))
        {
            File.Delete(FilePath);
        }
        aLogFile.HRef = "javascript:alert('"+ PatchInstaller.ReadKeysForPatch("LogFileMsg")+"');";
    }
}