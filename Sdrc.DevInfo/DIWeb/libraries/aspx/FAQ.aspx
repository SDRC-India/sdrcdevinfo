<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master"
    AutoEventWireup="true" CodeFile="FAQ.aspx.cs" Inherits="libraries_aspx_FAQ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="cphMainContent" runat="Server">
    <div class="content_containers">
        <!-- Left Links Section ...starts-->
        <div id="lft_sec_adm" class="lft_sec_adm_pos" style="height:100px">
            <ul>
                <li><a href="FAQ.aspx?T=A&PN=diorg/di_FAQ_General.html" id="aGeneralFAQ" runat="server">
                    <%--di Monitoring--%></a></li>
                <li><a href="FAQ.aspx?T=G&PN=diorg/di_Knowledge_base.html" id="aApplicationSpecificFAQ" runat="server">
                    <%--di Profile--%></a></li>                
            </ul>
        </div>
        <!-- Left Links Section ...ends-->
        <!-- Right Config Data Section ...starts-->
        <div id="rgt_sec_adm" class="rgt_sec_adm_pos">
            <h1>
                <span id="Header" runat="server"></span>
                <div class="adm_nav_opt pddn_bttm_nne flt_rgt">
                    <ul>
                        <li><a href="javascript:void(0);" id="AddNewFAQ" runat="server" style="display: none;">
                            Add New</a></li>
                        <li id="PipeAddEditNews" runat="server" style="display: none;">| </li>
                        <li><a href="javascript:void(0);" id="EditPage" runat="server" style="display: none;">
                            Edit This Page</a></li>
                    </ul>
                </div>
            </h1>
            <div class="clear">
            </div>
            <h4>
                <span id="Desc" runat="server"></span>
            </h4>
            <!-- Static Content Area ...starts-->
            <div class="sttc_cntnt_main" id="div_content" runat="server">
            </div>
            <!-- Static Content Area ...ends-->
            <!-- Right Config Data Section ...ends-->
            <div class="clear">
            </div>
        </div>
        <!-- Main Container ...ends-->
    </div>
    <!-- DEVELOPER CODE -->
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
        createFormHiddenInputs("frmFAQ", "POST");
        SetCommonLinksHref("frmFAQ", "POST");
        setPostedData('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', 10, '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');
        if (GetLanguageCounts() > 1) {
            z("LanguagePipeLi").style.display = "";
            ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', '<%=hlngcode%>');
        }
    </script>
    <script type="text/javascript">
        di_jq(document).ready(function () {
            SelectFAQMenuOption();
            var Url = document.URL.split("?")[1];
            document.getElementById('<%=EditPage.ClientID%>').href = "EditCmsContent.aspx?" + Url;
            document.getElementById('<%=AddNewFAQ.ClientID%>').href = "AddCMSContent.aspx?" + Url;
        });	   
    </script>
</asp:content>