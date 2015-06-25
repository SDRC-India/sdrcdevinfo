<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PatchInstaller.aspx.cs" Inherits="libraries_aspx_PatchInstaller" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../stock/themes/default/css/diuilibcommon.css" rel="stylesheet" type="text/css" />
    <link href="../../stock/themes/default/css/style.css" rel="stylesheet" type="text/css" />
    <!--<link href="../../stock/themes/default/css/style_rtl.css" rel="stylesheet" type="text/css" />  -->
    <!--[if IE]>
        <link rel="stylesheet" type="text/css" href="../../stock/themes/default/css/style-ie.css" />
<![endif]-->
    <link href="../../stock/themes/default/images/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <script type="text/javascript" src="../js/common.js"></script>
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/admin.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/browsr_selector.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/PatchInstaller.js"></script>
</head>
<body>
    <form id="FrmPatchInstaller" runat="server">
    <!-- Header Area ...starts-->
    <div id="header">
        <div class="header">
            <!-- Logo Section ...starts-->
            <div class="main_logo">
                <a href="home.aspx" id="aHeader">
                    <img src="../../stock/themes/default/images/cust/logo.png" alt="" width="230" height="71" /></a></div>
            <!-- Logo Section ...ends-->
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
            <h1 id="lang_db_PatchMainHeading" runat="server">
                <!-- Patch Settings-->
            </h1>
            <h4 id="lang_db_Patch_subHead" runat="server">
                <!--install patch from here-->
            </h4>          
                <a href="javascript:void(0);" id="aLogFile" runat="server" class="PatchInsl_loglnk"> </a>
          
            <div class="confg_Adm_box">
                <div class="adm_db_flt_lft" style="display: block">
                    <img id="imgInstPatchTickGray" alt='' src="../../stock/themes/default/images/tickmark_grey.png"
                        style="display: block;" runat="server" />
                    <img id="imgInstPatchProcessing" alt='' src="../../stock/themes/default/images/processing.gif"
                        style="display: none;" runat="server" />
                    <img id="imgInstPatchTick" alt='' src="../../stock/themes/default/images/tickmark.png"
                        style="display: none;" runat="server" />
                    <img id="imgInstPatchError" alt='' src="../../stock/themes/default/images/error.png"
                        style="display: none;" runat="server" />
                </div>
                <div class="adm_o_db_txt" id="langInstallingPatch" runat="server">
                    <!-- Installing Patch-->
                </div>
                <div class="adm_db_flt_lft">
                    <img id="imgUpdLngTickGray" alt='' src="../../stock/themes/default/images/tickmark_grey.png"
                        style="display: block;" runat="server" />
                    <img id="imgUpdLngProcessing" alt='' src="../../stock/themes/default/images/processing.gif"
                        style="display: none;" runat="server" />
                    <img id="imgUpdLngTick" alt='' src="../../stock/themes/default/images/tickmark.png"
                        style="display: none;" runat="server" />
                    <img id="imgUpdLngError" alt='' src="../../stock/themes/default/images/error.png"
                        style="display: none;" runat="server" />
                </div>
                <div class="adm_o_db_txt" id="langUpdLanguage_Patch" runat="server">
                    <!-- Updating Language-->
                </div>
                <div class="adm_db_flt_lft">
                    <img id="imgUpdSetTickGray" alt='' src="../../stock/themes/default/images/tickmark_grey.png"
                        style="display: block;" runat="server" />
                    <img id="imgUpdSetProcessing" alt='' src="../../stock/themes/default/images/processing.gif"
                        style="display: none;" runat="server" />
                    <img id="imgUpdSetTick" alt='' src="../../stock/themes/default/images/tickmark.png"
                        style="display: none;" runat="server" />
                    <img id="imgUpdSetError" alt='' src="../../stock/themes/default/images/error.png"
                        style="display: none;" runat="server" />
                </div>
                <div class="adm_o_db_txt" id="langUpdatingAppSet_Patch" runat="server">
                    Updating Settings1
                </div>
                <div class="adm_db_flt_lft" style="display: none;">
                    <img id="imgSDMXTickGray" alt='' src="../../stock/themes/default/images/tickmark_grey.png"
                        style="display: block;" runat="server" />
                    <img id="imgSDMXProcessing" alt='' src="../../stock/themes/default/images/processing.gif"
                        style="display: none;" runat="server" />
                    <img id="imgSDMXTick" alt='' src="../../stock/themes/default/images/tickmark.png"
                        style="display: none;" runat="server" />
                    <img id="imgSDMXError" alt='' src="../../stock/themes/default/images/error.png" style="display: none;"
                        runat="server" />
                </div>
            </div>
            <!-- Configuration Update Button ...starts -->
            <div class="adm_upd_bttn pddn_tp_fftn">
                <input type="button" id="langInstallBtn_Patch" value="" onclick="InstallPatch(LogFilePath);"
                    runat="server" class="di_gui_button lng_bttn_sz" />
            </div>
            <!-- Configuration Update Button ...ends-->
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
            
            var LogFilePath = '<%=LogFilePath %>';
    </script>
</body>
</html>
