<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master" AutoEventWireup="true"
    CodeFile="RegStructuralMD.aspx.cs" Inherits="libraries_aspx_SDMX_sdmx_StructuralMetadata"
    Title="Untitled Page" EnableSessionState="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" runat="Server">
    <script type="text/javascript" src="../js/sdmx/StructuralMD.js?v=<%=Global.js_version%>"></script>
    <!-- Reg Left Section ...starts-->
    <div id="reg_lft_sec">
        <div>
           <img id="imgdivDSD" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse" onclick="ExpandCollapseList('imgdivDSD','divDSD')"  style="margin-top: 10px;margin-right: 4px" class="flt_lft"/>
           <h3 id="lang_Data_Structure_Definition" class="flt_lft">
                <!--Data Structure Definition-->
            </h3>
            &nbsp;<img id="imghelpDSD" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpDSD')"
                style="cursor: pointer; margin-top: 10px" onmouseout="HideCallout('divCallout')";/>
        </div>
        <br />
         <div id="divDSD" style="display:none">
            <div id="divDimensions" runat="server">
            </div>
            <div id="divAttributes" runat="server">
            </div>
            <div id="divMeasure" runat="server">
            </div>
         </div>
       
        <div>
            <img id="imgdivDFD" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse" onclick="ExpandCollapseList('imgdivDFD','divDFDOuter')"  style="margin-top: 10px;margin-right: 4px" class="flt_lft"/>
            <h3 id="lang_Data_Flow_Definition" class="flt_lft">
               <%-- Data Flow Definition--%></h3>
            &nbsp;<img id="imghelpDFD" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpDFD')" style="cursor: pointer;
                margin-top: 10px" onmouseout="HideCallout('divCallout')";/>
        </div>
        <br />
         <div id="divDFDOuter" style="display:none">
            <div id="divDFD" runat="server">
            </div>
        </div>
       
        <div>
             <img id="imgdivConceptScheme" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse" onclick="ExpandCollapseList('imgdivConceptScheme','divConceptSchemeOuter')"  style="margin-top: 10px;margin-right: 4px" class="flt_lft"/>
            <h3 id="lang_Concept_Scheme" class="flt_lft"><%--Concept Scheme--%></h3>
            &nbsp;<img id="imghelpConceptScheme" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpCS')"
                style="cursor: pointer; margin-top: 10px" onmouseout="HideCallout('divCallout')";/>
        </div>
        <br />
        <div id="divConceptSchemeOuter" style="display:none">
            <div id="divConceptScheme" runat="server">
            </div>
        </div>
        <div>
             <img id="imgdivCodelists" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse" onclick="ExpandCollapseList('imgdivCodelists','divCodelistsOuter')"  style="margin-top: 10px;margin-right: 4px" class="flt_lft"/>
            <h3 id="lang_Codelists" class="flt_lft">
                <!--Codelists-->
            </h3>
            &nbsp;<img id="imghelpCodelist" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpCL')" style="cursor: pointer;
                margin-top: 10px" onmouseout="HideCallout('divCallout')";/>
        </div>
        <br />
        <div id="divCodelistsOuter" style="display:none">
            <div id="divCodelists" runat="server">
            </div>
        </div>
    </div>
    <!-- Reg Left Section ...ends-->
    <!-- Reg Right Section ...starts-->
    <div id="reg_rgt_sec">
        <!-- Download Buttons ...starts-->
        <div class="reg_dwnld_bttn_main">
            <p>
                <a id="lnkDownloadAll" href="#">
                    <!--Download All-->
                </a>
            </p>
        </div>
        <div class="reg_dwnld_bttn_main">
            <p>
                <a id="lnkDownloadRpt" href="#">
                    <!--Download Report-->
                </a>
            </p>
        </div>
        <!-- Download Buttons ...ends-->
    </div>
    <!-- Reg Right Section ...ends-->
    <!-- Popup Registration StructuralMD(view) ...starts -->
    <div id="divCodelistXml" class="popup_block">
        <div id="divCodelistXmlData">
        </div>
    </div>
    <!-- Popup Registration StructuralMD(view) ...ends -->
    <!-- Popup For Help starts -->
    <div id="divCallout" class="callout border-callout">
        <div id="divCalloutText" class="content">
            <!-- Popup For Help Text starts -->
            <div id="divHelpDSD" style="display:none">
              <span class="heading2" id="span_Help_Data_Structure_Definition"></span>
              <br />
              <i class="content" id="i_Help_Data_Structure_Definition"></i>
            </div>
            <div id="divHelpDFD" style="display:none">
              <span class="heading2" id="span_Help_Data_Flow_Definition"></span>
              <br />
              <i class="content" id="i_Help_Data_Flow_Definition"></i>
            </div>
            <div id="divHelpCS" style="display:none">
              <span class="heading2" id="span_Help_Concept_Scheme"></span>
              <br />
              <i class="content" id="i_Help_Concept_Scheme"></i>
            </div>
            <div id="divHelpCL" style="display:none">
              <span class="heading2" id="span_Help_Codelist"></span>
              <br />
              <i class="content" id="i_Help_Codelist"></i>
            </div>
            <!-- Popup For Help Text ends -->
        </div>
        <b class="border-notch notch"></b><b class="notch"></b>
    </div>
    <!-- Popup For Help ...ends -->
    <input type="hidden" id="hView" />
    <input type="hidden" id="hDownload" />
    <input type="hidden" id="hAttachmentLevel" />
    <input type="hidden" id="hMandatory" />
    <input type="hidden" id="hYes" />
    <input type="hidden" id="hNo" />
    <input type="hidden" id="hErrorOccurred" />
    <input type="hidden" id="hFileNotFound" />

    <!-- START DEVELOPER CODE -->
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

        var di_components = "Area,Indicator,Language";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
        var hIsUploadedDSD = '<%= hIsUploadedDSD %>'; 
    </script>
    <script type="text/javascript">onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=hOriginaldbnid %>');
    </script>
    <!-- END OF DEVELOPER CODE -->
</asp:Content>
