<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true" CodeFile="DevInfoWorldwide.aspx.cs" Inherits="libraries_aspx_DevInfoWorldwide" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
<div class="content_containers">
    
    <!-- Left Links Section ...starts-->
    <div id="lft_sec_adm" class="lft_sec_adm_pos">
        <ul>
            <li><a href="DevInfoWorldwide.aspx?T=W1&PN=diorg/worldwide.html" id="Worldwide1"
                runat="server"><span id="Header1" runat="server"></span></a></li>
            <li><a href="DevInfoWorldwide.aspx?T=W2&PN=diorg/di_adaptations.html" id="Worldwide2"
                runat="server"><span id="Header2" runat="server"></span></a></li>
            <li><a href="DevInfoWorldwide.aspx?T=W3&PN=diorg/di_adaptations_country.html" id="Worldwide3"
                runat="server"><span id="Header3" runat="server"></span></a></li>
            <li><a href="DevInfoWorldwide.aspx?T=W4&PN=diorg/di_adaptations_other_bodies.html" id="Worldwide4"
                runat="server"><span id="Header4" runat="server"></span></a></li>
        </ul>
    </div>
    <!-- Left Links Section ...ends-->
    <!-- Right Config Data Section ...starts-->
    <div id="rgt_sec_adm" class="rgt_sec_adm_pos">
        <!--<div id="reg_content_containers">-->
        <h1>
            <span id="HeaderDesc" runat="server"></span>
        </h1>
        <h4>
        </h4>
        <!-- Main Contact Page Content Area ...starts-->
        <div class="desc_pg_main_sec sttc_cntnt_main" id="div_content" runat="server">
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
    createFormHiddenInputs("frmWorldwide", "POST");
    SetCommonLinksHref("frmWorldwide", "POST");
    setPostedData('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', 10, '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');
    if (GetLanguageCounts() > 1) {
        z("LanguagePipeLi").style.display = "";
        ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', '<%=hlngcode%>');
    }
    </script>
 <script type="text/javascript">
     di_jq(document).ready(function () {
         SelectWorldwideMenuOption();
     });	
    </script>
</asp:Content>

