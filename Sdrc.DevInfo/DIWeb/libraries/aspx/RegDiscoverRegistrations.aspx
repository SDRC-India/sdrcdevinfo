<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master" AutoEventWireup="true" CodeFile="RegDiscoverRegistrations.aspx.cs" Inherits="libraries_aspx_RegDiscoverRegistrations" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" Runat="Server">

     <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
     <script type="text/javascript" src="../js/jquery.ui.datepicker.js?v=<%=Global.js_version%>"></script>
     <script type="text/javascript" src="../js/sdmx/DiscoverRegistrations.js?v=<%=Global.js_version%>"></script>

	<!-- Reg Wide Section ...starts-->
	<div id="reg_wide_sec">
    <div>
         <p class="confg_frm_txt_lgin_ppup" ><span id="lang_View_Registrations_By"><%--View Registrations by--%> : </span>
        </p>
        <p class="confg_frm_inp_bx">
            <select id="selViewRegistrationsBy" class="confg_frm_inp_bx_txt_dd" onchange="BindRegistrations();">
                <option value="0" id="lang_Data_Flow_Definition"><%--Data Flow Definition--%></option>
                <option value="1" id="lang_Metadata_Flow_Definition"><%--Metadata Flow Definition--%></option>
                <option value="2" id="lang_Provision_Agreement"><%--Provision Agreement--%></option>
                <option value="3" id="lang_Data_Provider"><%--Data Provider--%></option>
            </select>                    
        </p> 
    </div>
    <div class="clear"></div>  
    <div id="divRegistrationsByDFD" style="display:none"></div>
    <div class="clear"></div> 
    <div id="divRegistrationsByMFD" style="display:none"></div>
    <div class="clear"></div> 
    <div id="divRegistrationsByPA" style="display:none"></div>
    <div class="clear"></div>    
    <div id="divRegistrationsByDP" style="display:none"></div>         
	</div>
	<!-- Reg Wide Section ...ends-->    	    
   <input type="hidden" id="hView" />
   <input type="hidden" id="hDownload" /> 
   <input type="hidden" id="hId" /> 
   <input type="hidden" id="hQueryableData" />
   <input type="hidden" id="hWADL" />
   <input type="hidden" id="hWSDL" />
   <input type="hidden" id="hSimpleData" />
   <input type="hidden" id="hDataMetadata" />
   <input type="hidden" id="hConstraints" />
   <input type="hidden" id="hProvisionAgreement" />
   <input type="hidden" id="hDataProvider" />
   <input type="hidden" id="hDFDMFD" />
   <input type="hidden" id="hDFDMFDShortForm" />
   <input type="hidden" id="hProvider" />
   <input type="hidden" id="hNoRegistrationFound" />
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


                //*** Set divFooterRow Properties ****
//                DivFR.style.width = (parseInt(width) - 16) + 'px';
//                DivFR.style.position = 'relative';
//                DivFR.style.top = -headerHeight + 'px';
//                DivFR.style.verticalAlign = 'top';
//                DivFR.style.paddingtop = '2px';


//                if (isFooter) {
//                    var tblfr = tbl.cloneNode(true);
//                    tblfr.removeChild(tblfr.getElementsByTagName('tbody')[0]);
//                    var tblBody = document.createElement('tbody');
//                    tblfr.style.width = '100%';
//                    tblfr.cellSpacing = "0";
//                    //*****In the case of Footer Row *******
//                    tblBody.appendChild(tbl.rows[tbl.rows.length - 1]);
//                    tblfr.appendChild(tblBody);
//                    DivFR.appendChild(tblfr);
//                }
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


