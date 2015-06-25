<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PatchInstaller.aspx.vb"
    Inherits="di_Worldwide.PatchInstaller" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../Stock/css/Style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js"></script>
    <script type="text/javascript" src="../js/PatchInstaller.js"></script>
</head>
<body>
    <form id="FrmPatchInstaller" runat="server">
    <!-- Header Area ...starts-->
    <div id="header">
        <div class="header">
            <!-- Logo Section ...starts-->
            <div class="main_logo">
                <img src="../../Stock/Images/logo.png" alt="" width="230" height="71" /></div>
            <!-- Logo Section ...ends-->
            <!-- Top Links ...starts-->
            <div class="headerlink1_adm">
                <ul>
                    <li><a id="aLogFile" href="javascript:void(0);" runat="server">Log File</a></li>
                </ul>
            </div>
            <!-- Top Links ...ends-->
        </div>
    </div>
    <!-- Header Area ...ends-->
    <!-- Main Container ...starts-->
    <div class="content_containers">
        <div class="clear navls_top_pg">
            &nbsp;</div>
        <!-- Left Links Section ...starts-->
        <div id="lft_sec_adm">
            <ul>
                <li><a href="javascript:void(0);" id="LangLeftLnkInstPatch" runat="server" class="adm_lft_nav_seld">
                    <!--Install Patch-->
                </a></li>
            </ul>
        </div>
        <!-- Left Links Section ...ends-->
        <!-- Right Config Data Section ...starts-->
        <div id="rgt_sec_adm">
            <div id="MainContent">
                <h1 id="lang_db_PatchMainHeading" runat="server">
                    <!-- Patch Settings-->
                </h1>
                <h4 id="lang_db_Patch_subHead" runat="server">
                    <!--install patch from here-->
                </h4>
                <h2 id="langInstallingPatchHead" runat="server">
                    <!-- installing patch-->
                </h2>
                <div class="confg_Adm_box">
                    <div class="adm_db_flt_lft" style="display: block">
                        <img id="imgInstPatchTickGray" alt='' src="~/Stock/Images/tickmark_grey.png" style="display: block;"
                            runat="server" />
                        <img id="imgInstPatchProcessing" alt='' src="~/Stock/Images/processing.gif" style="display: none;"
                            runat="server" />
                        <img id="imgInstPatchTick" alt='' src="~/Stock/Images/tickmark.png" style="display: none;"
                            runat="server" />
                        <img id="imgInstPatchError" alt='' src="~/Stock/Images/error.png" style="display: none;"
                            runat="server" />
                    </div>
                    <div class="adm_o_db_txt" id="langInstallingPatch" runat="server">
                        <!-- Installing Patch-->
                    </div>
                </div>
                <!-- Configuration Update Button ...starts -->
                <div class="adm_upd_bttn">
                    <input type="button" id="BtnInstall_Patch" value="" 
                        onclick="InstallPatch();" runat="server" class="di_gui_button lng_bttn_sz" />
                </div>
                <!-- Configuration Update Button ...ends-->
            </div>
            <div class="clear">
            </div>
        </div>
        <!-- Main Container ...ends-->
    </div>
    <input type="hidden" id="LangPatchInstSuccess" value="" runat="server" />
    <input type="hidden" id="LangPatchInstFailed" value="" runat="server" />
    <input type="hidden" id="LangPatchLngSuccess" value="" runat="server" />
    <input type="hidden" id="LangPatchLngError" value="" runat="server" />
    <input type="hidden" id="LangPatchSetSuccess" value="Patch installed successfully."
        runat="server" />
    <input type="hidden" id="LangPatchSetError" value="Error in installing patch" runat="server" />
    <div id="divLoading" class="divLoading">
    </div>
    <div id="MaskedDiv" class="MaskedDiv">
    </div>
    </form>
    <script type="text/javascript">
        function InstallPatch() {
            di_jq("#imgInstPatchError").css("display", "none");
            di_jq("#imgInstPatchTickGray").css("display", "none");
            di_jq("#imgInstPatchTick").css("display", "none");
            di_jq("#imgInstPatchProcessing").css("display", "block");
            ApplyMaskingDiv();
            di_jq.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "PatchInstaller.aspx/InstallPatch",
                data: "{}",
                async: true,
                success: function (data) {

                    if (data == "true") {

                        di_jq("#imgInstPatchTick").css("display", "block");
                        alert(z("LangPatchSetSuccess").value);
                        RemoveMaskingDiv();
                    }
                    else {
                        di_jq("#imgInstPatchProcessing").css("display", "none");
                        di_jq("#imgInstPatchError").css("display", "block");
                        di_jq("#imgInstPatchTick").css("display", "none");
                        alert(di_jq("#LangPatchSetError").value);
                        RemoveMaskingDiv();
                    }

                },
                error: function (result) {
                    di_jq("#imgInstPatchProcessing").css("display", "none");
                    di_jq("#imgInstPatchError").css("display", "block");
                    di_jq("#imgInstPatchTick").css("display", "none");
                    alert(di_jq("#LangPatchSetError").value);
                    RemoveMaskingDiv();
                }
            });
        }
    </script>
</body>
</html>
