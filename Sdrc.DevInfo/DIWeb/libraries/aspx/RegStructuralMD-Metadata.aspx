<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master" 
AutoEventWireup="true" CodeFile="RegStructuralMD-Metadata.aspx.cs" Inherits="libraries_aspx_RegStructuralMD_Metadata" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" runat="Server">

    <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/sdmx/StructuralMD-Metadata.js?v=<%=Global.js_version%>"></script>
    
	    <!-- Reg Left Section ...starts-->
	    <div id="reg_lft_sec">
            <div class="confg_frm_inp_bx">
                <select id="selectMSD" class="confg_frm_inp_bx_txt_dd" onchange="BindMSDArtefacts();">
                    <option value="0" id="lang_Select_MSD"><!-- -----------------------Select MSD--------------------- --></option>
                </select>                    
            </div> 
            <div class="clear">&nbsp;</div>
            <div id="divAttributes"></div>
	    </div>
	    <!-- Reg Left Section ...ends-->
			
		<!-- Reg Right Section ...starts-->
		<div id="reg_rgt_sec" style="display:none">
            <!-- Download Buttons ...starts-->	

            <div class="reg_dwnld_bttn_main">
            <p><a id="lnkDownloadAll" href="#"><!--Download All--></a></p>
            </div>	

            <!-- Download Buttons ...ends-->
		</div>
		<!-- Reg Right Section ...ends-->	 
<!-- Popup For Help starts -->                
    <div id="divCallout" class="callout border-callout">
        <div id="divCalloutText" class="content">
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
            <div id="divHelpCS" style="display:none">
              <span class="heading2" id="span_Help_Concept_Scheme"></span>
              <br />
              <i class="content" id="i_Help_Concept_Scheme"></i>
            </div>
            <!-- Popup For Help Text ends -->
        </div>
        <b class="border-notch notch"></b><b class="notch"></b>
    </div>
<!-- Popup For Help ...ends --> 
    <input type="hidden" id="hView" />
    <input type="hidden" id="hDownload" />
    <input type="hidden" id="hObjectType" />
    <input type="hidden" id="hPresentational" />
    <input type="hidden" id="hYes" />
    <input type="hidden" id="hNo" />
    <input type="hidden" id="hTarget" />
    <input type="hidden" id="hReportStructure" />
    <input type="hidden" id="hMSD" />	
    <input type="hidden" id="hMFD" />	
    <input type="hidden" id="hCS" />
    <input type="hidden" id="hErrorOccurred" />			
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

