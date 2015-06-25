<%@ Page Title="Untitled Page" Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master" 
AutoEventWireup="true" CodeFile="RegStructure.aspx.cs" Inherits="libraries_aspx_SDMX_sdmx_RegStructure"  EnableSessionState="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" Runat="Server">
<script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
<script type="text/javascript" src="../js/jquery.ui.datepicker.js?v=<%=Global.js_version%>"></script>
 <script type="text/javascript" src="../js/sdmx/Structure.js?v=<%=Global.js_version%>"></script>
    <!-- Reg Left Section ...starts-->
    <div id="reg_lft_sec">
      <div>
         <img id="imgdivDataStucture" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse" onclick="ExpandCollapseList('imgdivDataStucture','divDataStucture')"  style="margin-top: 10px;margin-right: 4px" class="flt_lft"/>
               <h3 id="spanDataPageTitle" class="flt_lft">
                    <!--Data Structure Definition-->
                </h3>
                    &nbsp;<img id="imghelpStructures" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpDataStucture')" style="cursor: pointer;
                margin-top: 10px" onmouseout="HideCallout('divCallout')";/>
       </div>
       <br />
       <div id="divDataStucture" style="display:none;margin-left:20px">
         <div>
             <img id="imgdivConceptScheme" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse" onclick="ExpandCollapseList('imgdivConceptScheme','divConceptSchemeOuter')"  style="margin-top: 10px;margin-right: 4px" class="flt_lft"/>
            <h3 id="lang_Concept_Scheme_DSD" class="flt_lft"><%--Concept Scheme--%></h3>
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
       
      
        <br />
        </div>
     

        <!--MSD Starts-->
        <div>
        <img id="imgdivMetaDataStructure" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse" onclick="ExpandCollapseList('imgdivMetaDataStructure','divMetaDataStructure')"  style="margin-top: 10px;margin-right: 4px" class="flt_lft"/>
           <h3 id="spanMetaPageTitle" class="flt_lft">
                <!--Data Structure Definition-->
            </h3>
                &nbsp;<img id="img2" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpMAScheme')" style="cursor: pointer;
                margin-top: 10px" onmouseout="HideCallout('divCallout')";/>
        </div>
        <br />
        <div id="divMetaDataStructure" style="display:none;margin-left:20px">
       <%-- <div class="dottedline"> </div>--%>
       <br />
         <span id="SelectYourMSD" class="SelectYourMSD flt_lft"><!--Select MSD--></span>
          <div class="confg_frm_inp_bx flt_rgt">
            <select id="selectMSD" class="confg_frm_inp_bx_txt_dd " onchange="BindMSDArtefacts();">
                    <option value="0" id="lang_Select_MSD"><!-- -----------------------Select MSD--------------------- --></option>
                </select>                    
            </div> 
            <div class="clear">&nbsp;</div>
            <div id="divMetaAttributes"></div>
      </div>
        <!--MSD Ends-->

        <!--Provision Metadata-->
       
    
      <%--  <div class="dottedline"></div>--%>

       
         <div>
             <img id="imgdivPAScheme" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse" onclick="ExpandCollapseList('imgdivPAScheme','divRegistrationsByPA')"  style="margin-top: 10px;margin-right: 4px" class="flt_lft"/>
            <h3 id="heading_PA" class="flt_lft">
                <!--Data Provider Sceheme-->
            </h3>
            &nbsp;<img id="imghelpPAScheme" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpPAScheme')" style="cursor: pointer;
                margin-top: 10px" onmouseout="HideCallout('divCallout')";/>
        </div>
        <br />
         <div id="divRegistrationsByPA" style="display:none"></div>
         <div class="clear"></div>    
       
           <div>
             <img id="imgdivDPScheme" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse" onclick="ExpandCollapseList('imgdivDPScheme','divDownloadsDP')"  style="margin-top: 10px;margin-right: 4px" class="flt_lft"/>
            <h3 id="heading_Data_Provider_Scheme" class="flt_lft">
                <!--Data Provider Sceheme-->
            </h3>
            &nbsp;<img id="imghelpDPScheme" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpDPScheme')" style="cursor: pointer;
                margin-top: 10px" onmouseout="HideCallout('divCallout')";/>
        </div>
        <br />
          <div id="divDownloadsDP" style="display:none"></div>
       
         <div class="clear"></div>  
           <div>
             <img id="imgdivMAScheme" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse" onclick="ExpandCollapseList('imgdivMAScheme','divDownloadsMA')"  style="margin-top: 10px;margin-right: 4px" class="flt_lft"/>
            <h3 id="heading_Maintenance_Agency_Scheme" class="flt_lft">
                <!--Codelists-->
            </h3>
            &nbsp;<img id="imghelpMAScheme" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpMAScheme')" style="cursor: pointer;
                margin-top: 10px" onmouseout="HideCallout('divCallout')";/>
        </div>
        <br />
          <div id="divDownloadsMA" style="display:none"></div>
        <br />
     
       
           
      <!--Provision Metadata-->

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

    	    
<!-- Popup for Registration Details ...starts-->
<div id="divArtefacts" class="popup_block">	
    
    <!-- Popup Inside Content ...starts-->
    <div id="reg_content_containers">
        <h2><label id="lblArtefactType"></label></h2>
        <h5></h5>
        
        <div id="reg_wide_sec_ppup">
        
                <!-- Input Fields Area ...starts-->    
                <div id="confg_Adm_box_lgin" style="background-color:#ffffdd">
                    <pre id="preArtefacts" style="overflow:scroll;height:380px;width:664px;word-wrap:break-word; white-space:pre-wrap; padding:10px;"></pre>                    
                </div>
                <!-- Input Fields Area ...ends-->  
				

        </div>

    </div> 
    <!-- Popup Inside Content ...ends-->     
    
</div>      
<!-- Popup for Registration Details ...ends-->	

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
            <!-- Popup For Help Text starts -->
            <div id="divHelpMSD" style="display:none">
              <span class="heading2" id="span_Help_Metadata_Structure_Definition"></span>
              <br />
              <i class="content" id="i_Help_Metadata_Structure_Definition"></i>
            </div>
            <div id="divHelpMFD" style="display:none">
              <span class="heading2" id="span_Help_Metadata_Flow_Definition"></span>
              <br />
              <i class="content" id="i_Help_Metadata_Flow_Definition"></i>
            </div>
           <div id="divHelpDPScheme" style="display:none">
              <span class="heading2" id="span_Help_Data_Provider_Scheme"></span>
              <br />
              <i class="content" id="i_Help_Data_Provider_Scheme"></i>
            </div>
            <div id="divHelpMAScheme" style="display:none">
              <span class="heading2" id="span_Help_Maintenance_Agency_Scheme"></span>
              <br />
              <i class="content" id="i_Help_Maintenance_Agency_Scheme"></i>
            </div>
               <div id="divHelpPAScheme" style="display:none">
           
                       <span class="heading2" id="span_Help_PA"></span>
                       <br />
                       <i class="content" id="i_Help_PA"></i>
                 
            </div>
              <div id="divHelpDataStucture" style="display:none">
              <span class="heading2" id="span_Help_Structures"></span>
              <br />
              <i class="content" id="i_Help_Structures"></i>
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
    <input type="hidden" id="hObjectType" />
    <input type="hidden" id="hPresentational" />
    <input type="hidden" id="hTarget" />
    <input type="hidden" id="hReportStructure" />
    <input type="hidden" id="hMSD" />	
    <input type="hidden" id="hMFD" />	
    <input type="hidden" id="hCS" />
    <input type="hidden" id="hProvisionAgreement" />
   <input type="hidden" id="hDataProvider" />
    <input type="hidden" id="hId" /> 
       <input  type="hidden" id="hProvider" />
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
    <script type="text/javascript"> onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=hOriginaldbnid %>');
    </script>
    <!-- END OF DEVELOPER CODE -->
</asp:Content>

