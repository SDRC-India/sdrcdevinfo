<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master"
    AutoEventWireup="true" CodeFile="Innovations.aspx.cs" Inherits="libraries_aspx_Innovations" %>

<asp:content id="Content1" contentplaceholderid="cphHeadContent" runat="Server">
</asp:content>
<asp:content id="Content2" contentplaceholderid="cphMainContent" runat="Server">
    <!-- Main Container ...starts-->
    <div class="content_containers">
        <!-- Left Links Section ...starts-->
        <div id="lft_sec_adm" class="lft_sec_adm_pos">
            <ul>
                <li><a href="innovations.aspx?T=M&PN=diorg/di_monitoring.html" id="aMonitoring" runat="server">
                    <%--di Monitoring--%></a></li>
                <li><a href="innovations.aspx?T=P&PN=diorg/di_profile.html" id="aProfile" runat="server">
                    <%--di Profile--%></a></li>
                <li><a href="http://www.digw.org/" id="aGameworks" runat="server" target="_blank">
                    <%--di Gameworks--%></a></li>
                <li><a href="innovations.aspx?T=D&PN=diorg/di_dashboards.html" id="aDashboard" runat="server">
                    <%--di Dashboard--%></a></li>
            </ul>
        </div>
        <!-- Left Links Section ...ends-->
        <!-- Right Config Data Section ...starts-->
        <div id="rgt_sec_adm" class="rgt_sec_adm_pos">
            <h1>
                <span id="InnoMainHeading">Innovations</span>
                <div class="adm_nav_opt pddn_bttm_nne flt_rgt">
                    <ul>
                        <li><a href="javascript:void(0);" id="AddNewInnovation" runat="server" style="display: none;">
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
                <span id="InnoSubHeading">di Monitoring, di Profile, di Gameworks, di Dashboard</span></h4>
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
        createFormHiddenInputs("frmInnovations", "POST");
        SetCommonLinksHref("frmInnovations", "POST");
        setPostedData('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', 10, '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');
    </script>
    <script type="text/javascript">
        di_jq(document).ready(function () {
            SelectInnovationMenuOption();
            var Url = document.URL.split("?")[1];
            document.getElementById('<%=EditPage.ClientID%>').href = "EditCmsContent.aspx?" + Url;
            document.getElementById('<%=AddNewInnovation.ClientID%>').href = "AddCMSContent.aspx?" + Url;
        });	   
    </script>
</asp:content>
