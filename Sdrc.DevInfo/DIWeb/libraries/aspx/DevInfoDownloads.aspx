<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true" CodeFile="DevInfoDownloads.aspx.cs" Inherits="libraries_aspx_DevInfoDownloads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
<div class="content_containers">
    
    <!-- Left Links Section ...starts-->
    <div id="lft_sec_adm" class="lft_sec_adm_pos" >
        <ul>
            <li><a href="DevInfoDownloads.aspx?T=DL7&PN=diorg/di_downloads_7.html" id="aDownload7" runat="server">DevInfo 7</a></li>
            <li><a href="DevInfoDownloads.aspx?T=DL6&PN=diorg/di_downloads.html" id="aDownload6" runat="server">DevInfo 6</a></li>
        </ul>
    </div>
    <!-- Left Links Section ...ends-->    

    <!-- Right Config Data Section ...starts-->
    <div id="rgt_sec_adm" class="rgt_sec_adm_pos">    
    <!--<div id="reg_content_containers">-->
        <h1>DevInfo Downloads</h1>
        <h4></h4>

        <!-- Main Contact Page Content Area ...starts-->
        <div class="desc_pg_main_sec" id="div_content" runat="server">
        </div>
        <!-- Main Contact Page Content Area ...ends-->            
    </div>
    <!-- Right Config Data Section ...ends-->     
    <div class="clear"></div>
</div>

<!-- DEVELOPER CODE -->
<script type="text/javascript">
    CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

    var di_components = "Language";
    var di_component_version = '<%=Global.diuilib_version%>';
    var di_theme_css = '<%=Global.diuilib_theme_css%>';
    var di_diuilib_url = '<%=Global.diuilib_url%>';
    document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
    document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
</script>
 <script type="text/javascript">
     di_jq(document).ready(function () {
         SelectDownloadsMenuOption();
     });	
    </script>
</asp:Content>

