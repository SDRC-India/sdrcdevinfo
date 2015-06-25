<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master"
    AutoEventWireup="true" CodeFile="HowToVideo.aspx.cs" Inherits="libraries_aspx_HowToVideo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="Server">
</asp:Content>
<asp:content id="Content2" contentplaceholderid="cphMainContent" runat="Server">
 <div class="content_containers">
     <div id="reg_content_containers">
         <h2>
             <span id="langAboutInfoFeaturess">How To Video</span>             
         </h2>
         <div class="clear">
         </div>
         <h5>
         </h5>
     </div>
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
    <script type="text/javascript">
        createFormHiddenInputs("frmHwToVideo", "POST");
        SetCommonLinksHref("frmHwToVideo", "POST");
        setPostedData('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', 10, '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');

        if (GetLanguageCounts() > 1) {
            z("LanguagePipeLi").style.display = "";
            ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', '<%=hlngcode%>');
        }
    </script>
        <%--di_jq(document).ready(function () {
            var Url = document.URL.split("?")[1];
            document.getElementById('<%=EditPage.ClientID%>').href = "EditCMSContent.aspx?" + Url;
        });	  --%> 
    </script>
</asp:content>
