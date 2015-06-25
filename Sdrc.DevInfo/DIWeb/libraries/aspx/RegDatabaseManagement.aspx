<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master"
    AutoEventWireup="true" CodeFile="RegDatabaseManagement.aspx.cs" Inherits="libraries_aspx_RegDatabaseManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphRegistryMainContent" runat="Server">
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/sdmx/DatabaseManagement.js?v=<%=Global.js_version%>"></script>
    <!-- Reg Left Section ...starts-->
    <div id="reg_lft_sec" style="width: 100%">
        <h3 id="lang_List_Of_Databases">
            <%--List Of Databases :--%></h3>
        <div class="confg_Adm_box" id="divListofDatabases">
        </div>
        <br />
        <div>
            <input type="button" value="" id="btnGenerateSDMXArtefacts" class="submit_button"
                onclick="GenerateSDMXArtefacts();" />
        </div>
        <div style="height: 100px">
        </div>
    </div>
    <!-- Reg Left Section ...ends-->
    <input type="hidden" id="hConnectionName" />
    <input type="hidden" id="hDatabaseType" />
    <input type="hidden" id="hCreatedOn" />
    <input type="hidden" id="hPublisher" />
    <input type="hidden" id="hSNo" />
    <input type="hidden" id="hId" />
    <input type="hidden" id="hAgencyId" />
    <input type="hidden" id="hVersion" />
    <input type="hidden" id="hAssosciatedDatabase" />
    <input type="hidden" id="hDetails" />
    <input type="hidden" id="hAction" />
    <input type="hidden" id="hAssosciatedDSD" />
    <input type="hidden" id="hView" />
    <input type="hidden" id="hDownload" />
    <input type="hidden" id="hEdit" />
    <input type="hidden" id="hDelete" />
    <input type="hidden" id="hArtifactsGeneratedSuccessfully" />
    <input type="hidden" id="hUploadedSuccessfully" />
    <input type="hidden" id="hErrorOccured" />
    <input type="hidden" id="hErrorWhileUploading" />
    <input type="hidden" id="hUpdatedSuccessfully" />
    <input type="hidden" id="hErrorWhileUpdating" />
     <input type="hidden" id="hDeletedSuccessfully" />
    <input type="hidden" id="hErrorWhileDeleting" />
      <input type="hidden" id="hArtifactsNotFound" />
    <input type="hidden" id="hSameVersionUploaded" />
     <input type="hidden" id="hSelectAssociatedDatabase" />
    
    <!-- START DEVELOPER CODE -->
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

        var di_components = "Language";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
        var hIsUploadedDSD = '<%= hIsUploadedDSD %>'; 
    </script>
    <script type="text/javascript">
        onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=hOriginaldbnid %>');
    </script>
    <script type="text/javascript" src="../js/di.jquery.form.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript">
        di_jq(document).ready(function () {
            BindDatabasesInTheTable();
        });
    </script>
    <!-- END OF DEVELOPER CODE -->
</asp:Content>
