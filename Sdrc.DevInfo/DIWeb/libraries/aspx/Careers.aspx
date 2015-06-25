<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true" CodeFile="Careers.aspx.cs" Inherits="libraries_aspx_Careers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">

<div class="content_containers">
    <div id="reg_content_containers">
        <h2>Careers</h2>
        <h5></h5>

        <!-- Main Page Content Area ...starts-->
        <div class="desc_pg_main_sec">
            <!-- Iframe for Content of page ...starts-->
            <iframe id="frame_auto" src="Careers.htm" scrolling="no" frameborder="0"></iframe>
            <!-- Iframe for Content of page ...ends-->
        </div>
        <!-- Main Page Content Area ...ends-->            
    </div>
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

<!-- For Iframe Auto Height ...starts-->
<script type="text/javascript">
    di_jq('frame_auto').contents().find('body').css({ "min-height": "100", "overflow": "hidden" });
    setInterval("di_jq('iframe').height(di_jq('iframe').contents().find('body').height() + 20)", 1);
</script>
<!-- For Iframe Auto Height ...ends-->
</asp:Content>

