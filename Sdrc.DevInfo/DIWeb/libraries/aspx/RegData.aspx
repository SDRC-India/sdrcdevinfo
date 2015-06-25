<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master"
    AutoEventWireup="true" CodeFile="RegData.aspx.cs" Inherits="libraries_aspx_RegData" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" runat="Server">
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/jquery.ui.datepicker.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/sdmx/Data.js?v=<%=Global.js_version%>"></script>
    <!-- Reg Wide Section ...starts-->
    <div id="reg_wide_sec">
        <div class="clear">
        </div>
        <div id="divRegistrationsByDFD" style="display:none">
        </div>
     
    </div>
    <!-- Reg Wide Section ...ends-->
    <input type="hidden" id="hView" />
    <input type="hidden" id="hDownload" />
    <input type="hidden" id="hId" />
    <input type="hidden" id="hQueryableData" />
    <input type="hidden" id="hWADL" />
    <input type="hidden" id="hWSDL" />
    <input type="hidden" id="hSimpleData" />
    <input type="hidden" id="hData" />
    <input type="hidden" id="hMetadata" />
    <input type="hidden" id="hConstraints" />
    <input type="hidden" id="hProvisionAgreement" />
    <input type="hidden" id="hDataProvider" />
    <input type="hidden" id="hDFDMFD" />
    <input type="hidden" id="hDFDMFDShortForm" />
    <input type="hidden" id="hProvider" />
    <input type="hidden" id="hNoRegistrationFound" />
    <input type="hidden" id="hDataRegistration" />
    <input type="hidden" id="hMetadataRegistration" />
     <input type="hidden" id="hMetadataRegistrationArea" />
      <input type="hidden" id="hMetadataRegistrationIndicator" />
       <input type="hidden" id="hMetadataRegistrationSource" />
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

        // BindRegistrations();
   
    </script>
    <script language="javascript" type="text/javascript">
        // BindRegistrations();
    </script>
    <script language="javascript" type="text/javascript">
        function MakeStaticHeader(gridId, divHeaderRowId, divMainContentId, divFooterRowId, height, width, headerHeight, isFooter) {
            var tbl = document.getElementById(gridId);
            if (tbl) {
                var DivHR = document.getElementById(divHeaderRowId);
                var DivMC = document.getElementById(divMainContentId);
                var DivFR = document.getElementById(divFooterRowId);


                //*** Set divheaderRow Properties ****
                DivHR.style.height = headerHeight + 'px';
                DivHR.style.width = (parseInt(width) - 16) + 'px';
                DivHR.style.position = 'relative';
                DivHR.style.top = '0px';
                DivHR.style.zIndex = '10';
                DivHR.style.verticalAlign = 'top';


                //*** Set divMainContent Properties ****
                DivMC.style.width = width + 'px';
                DivMC.style.height = height + 'px';
                DivMC.style.position = 'relative';
                DivMC.style.top = -headerHeight + 'px';
                DivMC.style.zIndex = '1';

                //****Copy Header in divHeaderRow****
                DivHR.appendChild(tbl.cloneNode(true));
            }
        }

        function OnScrollDiv(Scrollablediv, divHeaderRowId, divFooterRowId) {
            document.getElementById(divHeaderRowId).scrollLeft = Scrollablediv.scrollLeft;
            document.getElementById(divFooterRowId).scrollLeft = Scrollablediv.scrollLeft;
        }
    </script>
    <!-- END OF DEVELOPER CODE -->
</asp:Content>
