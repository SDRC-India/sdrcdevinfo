<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true" CodeFile="DevInfoMapLibrary.aspx.cs" Inherits="libraries_aspx_DevInfoMapLibrary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
<div class="content_containers">
    <div id="reg_content_containers">
        <h2>DevInfo Map Library</h2>
        <h5></h5>

        <!-- Main Contact Page Content Area ...starts-->
        <div class="desc_pg_main_sec" id="div_content" runat="server">
        </div>
        <!-- Main Contact Page Content Area ...ends-->            
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
</asp:Content>

