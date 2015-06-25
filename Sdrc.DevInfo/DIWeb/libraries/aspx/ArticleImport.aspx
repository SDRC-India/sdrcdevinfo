<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master"
    AutoEventWireup="true" CodeFile="ArticleImport.aspx.cs" Inherits="libraries_aspx_Article" %>

<asp:content id="Content1" contentplaceholderid="cphHeadContent" runat="Server">
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js"></script>
</asp:content>
<asp:content id="Content2" contentplaceholderid="cphMainContent" runat="Server">
    <form runat="server">
    <div class="content_containers">
        <!-- Add content Button ...starts-->
        <h2>
            <asp:label id="ImportCmsContent" runat="server">Import CMS content to database</asp:label></h2>
        <asp:button id="BtnImportContent" runat="server" text="Import Content to database"
            onclick="BtnImportContent_Click" />
        </br> </br>
        <asp:label id="LblImportSuccessMsg" runat="server" style="color: Green; display: none;">Content Imported to database successfully</asp:label>
        <asp:label id="LblImportFailureMsg" runat="server" style="color: red; display: none;">error in Importing data to database</asp:label>
    </div>
    </form>
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
        createFormHiddenInputs("frmArticleImport", "POST");
        SetCommonLinksHref("frmArticleImport", "POST");
        setPostedData('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', 10, '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');
        if (GetLanguageCounts() > 1) {
            z("LanguagePipeLi").style.display = "";
            ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', '<%=hlngcode%>');
        }
    </script>
</asp:content>
