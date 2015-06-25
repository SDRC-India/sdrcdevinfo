<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master"
    AutoEventWireup="true" CodeFile="RegWebServiceDemo.aspx.cs" Inherits="libraries_aspx_RegWebServiceDemo" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" runat="Server">
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/jquery.ui.datepicker.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/sdmx/RegistryWebServiceDemo.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript">

        di_jq(document).ready(function () {
            di_jq("#txtStartDate").datepicker({ dateFormat: 'dd-mm-yy' });
            di_jq("#txtEndDate").datepicker({ dateFormat: 'dd-mm-yy' });

        });
 
    </script>
    <!-- Reg Left Section ...starts-->
    <div id="reg_lft_sec" style="width: 100%">
        <!-- Input Fields Area ...starts-->
        <div id="confg_Adm_box_lgin">
            <p class="confg_frm_txt_lgin_ppup" id="lang_WebServiceFunctions">
                <%--Web Service Functions : --%></p>
            <p class="confg_frm_inp_bx_big" style="float: left;">
                <select id="selWebServiceFunction" class="confg_frm_inp_bx_txt_dd" onchange="GetIndicativeIdAgencyIdVersionValues();">
                    <option value="0" id="lang_Select_a_function">
                        <%----------------------Select a function--------------------%></option>
                    <option value="GetDataflow" id="optGetDataflow">GetDataflow</option>
                    <option value="GetMetadataflow" id="optGetMetadataflow">GetMetadataflow</option>
                    <option value="GetDataStructure" id="optGetDataStructure">GetDataStructure</option>
                    <option value="GetMetadataStructure" id="optGetMetadataStructure">GetMetadataStructure</option>
                    <option value="GetCategoryScheme" id="optGetCategoryScheme">GetCategoryScheme</option>
                    <option value="GetCategorisation" id="optGetCategorisation">GetCategorisation</option>
                    <option value="GetConceptScheme" id="optGetConceptScheme">GetConceptScheme</option>
                    <option value="GetCodelist" id="optGetCodelist">GetCodelist</option>
                    <option value="GetOrganisationScheme" id="optGetOrganisationScheme">GetOrganisationScheme</option>
                    <option value="GetProvisionAgreement" id="optGetProvisionAgreement">GetProvisionAgreement</option>
                    <option value="GetConstraint" id="optGetConstraint">GetConstraint</option>
                    <option value="GetStructures" id="optGetStructures">GetStructures</option>
                    <option value="QuerySubscription" id="optQuerySubscription">QuerySubscription</option>
                    <option value="QueryRegistration" id="optQueryRegistration">QueryRegistration</option>
                    <option value="SubmitSubscription" id="optSubmitSubscription">SubmitSubscription</option>
                    <option value="SubmitRegistration" id="optSubmitRegistration">SubmitRegistration</option>                   
                </select>
            </p>
            <span id="spanSampleValues" class="content" style="display: inline;">
                <%-- %>Sample Values --%></span>
            <div class="clear">
            </div>
            <div id="divIdAgencyIdVersionEntry" style="display: none">
                <p class="confg_frm_txt_lgin_ppup" id="lang_Id">
                    <%--Id : --%></p>
                <p class="confg_frm_inp_bx_big" style="float: left;">
                    <input type="text" id="txtId" class="confg_frm_inp_bx_txt" /></p>
                <i id="iId" class="content" style="display: inline;"></i>
                <div class="clear">
                </div>
                <p class="confg_frm_txt_lgin_ppup" id="lang_AgencyId">
                    <%--Agency Id : --%></p>
                <p class="confg_frm_inp_bx_big" style="float: left;">
                    <input type="text" id="txtAgencyId" class="confg_frm_inp_bx_txt" /></p>
                <i id="iAgencyId" class="content" style="display: inline;"></i>
                <div class="clear">
                </div>
                <p class="confg_frm_txt_lgin_ppup" id="lang_Version">
                    <%--Version : --%></p>
                <p class="confg_frm_inp_bx_big" style="float: left;">
                    <input type="text" id="txtVersion" class="confg_frm_inp_bx_txt" /></p>
                <i id="iVersion" class="content" style="display: inline;"></i>
                <div class="clear">
                </div>
            </div>
            <div class="clear">
            </div>
            <div id="divSubmitSubscription" style="display: none">
                <div>
                    <p class="confg_frm_txt_lgin_ppup flt_lft">
                        <span id="lang_Notification_Mail">
                            <%--Notification Mail--%></span>
                    </p>
                </div>
                <p class="confg_frm_inp_bx_big">
                    <input type="text" id="txtNotificationMail" class="confg_frm_inp_bx_txt" />
                    <input type="checkbox" id="chkIsSOAP" /><span id="lang_SOAP_Protocol"><%--SOAP Protocol--%></span>
                </p>
                <div class="clear">
                </div>
                <p class="confg_frm_txt_lgin_ppup">
                    <span id="lang_Notification_HTTP">
                        <%--Notification HTTP--%></span>
                </p>
                <p class="confg_frm_inp_bx">
                    <input type="text" id="txtNotificationHTTP" class="confg_frm_inp_bx_txt" /></p>
                <div class="clear">
                </div>
                <p class="confg_frm_txt_lgin_ppup reg_subs_vald_prd" id="lang_Validity_Period">
                    <%--Validity Period--%></p>
                <p class="confg_frm_inp_bx">
                </p>
                <div class="clear">
                </div>
                <p class="confg_frm_txt_lgin_ppup" id="lang_Start_Date">
                    <%--Start Date--%></p>
                <p class="confg_frm_inp_bx">
                    <input id="txtStartDate" type="text" class="confg_frm_inp_bx_txt_sml" /></p>
                <div class="clear">
                </div>
                <p class="confg_frm_txt_lgin_ppup" id="lang_End_Date">
                    <%--End Date--%></p>
                <p class="confg_frm_inp_bx">
                    <input id="txtEndDate" type="text" class="confg_frm_inp_bx_txt_sml" /></p>
                <div class="clear">
                </div>
                <br />
                <p class="confg_frm_txt_lgin_ppup">
                    <span id="lang_preference">
                        <!--Language Preference-->
                    </span>
                </p>
                <p class="confg_frm_inp_bx">
                    <select id="selLang" class="confg_frm_inp_bx_txt_dd" onchange="">
                        <%--<option value="0" id="sel_langPref_option"><!--Language Preference--></option>--%>
                    </select>
                </p>
                <div class="clear">
                </div>
                <p class="confg_frm_txt_lgin_ppup">
                    <span id="lang_Event_Selector">
                        <%--Event Selector--%></span>
                </p>
                <p class="confg_frm_inp_bx">
                    <select id="selEventSelector" class="confg_frm_inp_bx_txt_dd" onchange="BindRegCategoryOrDataFlows();">
                        <option value="0" id="lang_Data_Registration">
                            <%--Data Registration--%></option>
                        <option value="1" id="lang_Metadata_Registration">
                            <%--Metadata Registration--%></option>
                        <option value="2" id="lang_Structural_Metadata_Registration">
                            <%--Structural Metadata Registration--%></option>
                    </select>
                </p>
                <div class="reg_subs_ppup_inp_spc">
                </div>
                <div id="divCategory">
                    <p class="confg_frm_txt_lgin_ppup" id="lang_Category">
                        <%--Category--%></p>
                    <div class="confg_frm_inp_bx_big_ppup">
                        <div id="divRegistrationCategory">
                            <div id="divICTypes" class="di_gui_gradient_panel_box_ppup">
                            </div>
                            <div id="divCatScheme">
                            </div>
                        </div>
                    </div>
                </div>
                <div id="divMetadataFlows" style="display: none">
                    <p class="confg_frm_txt_lgin_ppup" id="lang_Metadata_Flows">
                        <%--Metadata Flows--%></p>
                    <p class="confg_frm_inp_bx">
                        <select id="selMFD" class="confg_frm_inp_bx_txt_dd">
                            <option value="0" id="lang_Select_MFD">
                                <%--Select MFD--%></option>
                        </select>
                    </p>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="clear">
            </div>
            <div id="divSubmitRegistration" style="display: none">
                <%--  <p class="confg_frm_txt_lgin_ppup""><span   id="spanWebServiceURL" >Web Service URL:</span>
                        </p>
                        <p class="confg_frm_inp_bx"><input type="text" id="txtWebServiceURL" class="confg_frm_inp_bx_txt" /></p> 
                        <div class="clear"></div> 
                    
                        <p class="confg_frm_txt_lgin_ppup" ><span  id="spanIsREST">REST Protocol</span>
                        </p>  
                        <p class="confg_frm_inp_bx_chkbx_lgn"><input type="checkbox" id="chkIsREST" onclick="EnableDisableAssociatedTextBox('txtWADLURL');"/></p>     
                        <div class="clear"></div>    
                    
                        <p class="confg_frm_txt_lgin_ppup"><span  id="spanWADLURL">WADL URL:</span>
                        </p>
                        <p class="confg_frm_inp_bx"><input type="text" id="txtWADLURL" disabled="disabled" class="confg_frm_inp_bx_txt" /></p> 
                        <div class="clear"></div>  
                    
                        <p class="confg_frm_txt_lgin_ppup" ><span  id="spanIsSOAP">SOAP Protocol</span>
                        </p>
                        <p class="confg_frm_inp_bx_chkbx_lgn"><input type="checkbox" id="chkIsSOAPRegistration" onclick="EnableDisableAssociatedTextBox('txtWSDLURL');"/></p> 
                        <div class="clear"></div>    
                    
                        <p class="confg_frm_txt_lgin_ppup" ><span  id="spanWSDLURL">WSDL URL:</span>
                        </p>
                        <p class="confg_frm_inp_bx"><input type="text" id="txtWSDLURL" disabled="disabled" class="confg_frm_inp_bx_txt" /></p> 
                        <div class="clear"></div>  --%>
                <p class="confg_frm_txt_lgin_ppup" id="P1">
                    <span id="lang_IsMetadata">
                        <%--Is Metadata:--%></span></p>
                <p class="confg_frm_inp_bx_chkbx_lgn">
                    <input type="checkbox" id="chkIsMetadata" onclick="BindDFDAndMFDList();" /></p>
                <div class="clear">
                </div>
                <p class="confg_frm_txt_lgin_ppup" id="lang_Select_DM_Flows">
                    <%--Select Data/Metadata Flows:--%></p>
                <p class="confg_frm_inp_bx">
                    <select id="selDFDMFD" class="confg_frm_inp_bx_txt_dd" onchange="ClearFileUrl();">
                        <option value="0" id="lang_Select_DFD_MFD">
                            <%-- -----------------------Select DFD/MFD--------------------- --%></option>
                    </select>
                </p>
                <div class="clear">
                </div>
                <%-- <h3 id="spanSimpleDataSource" class="flt_lft">Simple Data/Metadata Repository Details</h3>    
	                    <div class="clear"></div>    --%>
                <p class="confg_frm_txt_lgin_ppup" id="spanFileURL">
                    <%--SDMX-ML File/Metadata Report URL: --%></p>
                <p class="confg_frm_inp_bx">
                    <input type="text" id="txtFileURL" class="confg_frm_inp_bx_txt" /></p>
                <div class="clear">
                </div>
            </div>
        </div>
        <!-- Input Fields Area ...ends-->
        <!-- Configuration Button ...starts-->
        <div class="adm_upd_bttn_lgin_ppup" style="margin-left: 0px; float: left">
            <input type="button" id="btnGenerateApiCall" value="" onclick="GenerateAPICall();"
                class="di_gui_button" />
        </div>
        <!-- Configuration Button ...ends-->
        <div class="clear">
        </div>
        <div class="roundedcorners" style="width: 100%;">
            <p class="confg_frm_txt_lgin_ppup" id="lang_APIUrl" style="margin-top: 10px">
                <%--API URL : --%></p>
            <p class="confg_frm_inp_bx" style="padding-left: 0px">
                <a id="aAPIURLValue" class="confg_frm_txt_lgin" style="cursor: pointer; white-space: pre"
                    target="_blank"></a>
            </p>
            <div class="clear">
            </div>
            <p class="confg_frm_txt_lgin_ppup" id="lang_Name_of_the_Method">
                <%--Name of the Method : --%></p>
            <p class="confg_frm_inp_bx" style="padding-left: 0px">
                <span id="spanMethodValue" class="confg_frm_txt_lgin" style="padding-top: 0px"></span>
            </p>
            <div class="clear">
            </div>
        </div>
        <br />
        <div style="border-bottom: 2px dotted #c7c7c7; width: 100%">
            &nbsp;</div>
        <br />
        <div style="width: 48%; margin-right: 20px; float: left;">
            <span id="spanRequest" class="heading2">
                <%--Request--%></span>
        </div>
        <div style="width: 48%; float: left;">
            <span id="spanResponse" class="heading2">
                <%--Response--%></span>
        </div>
        <br />
        <div style="width: 48%; margin-right: 20px; float: left;">
            <a id="aDownloadRequest" style="cursor: pointer; font-size: 14px;" onclick="DownloadContent('textRequest');">
                <%--Download--%></a> <span style="font-size: 14px;">| </span><a id="aCopyRequest"
                    style="cursor: pointer; font-size: 14px;" onclick="CopyContent('textRequest');">
                    <%--Copy--%></a> <span style="font-size: 14px;">| </span><a id="aViewRequest" style="cursor: pointer;
                        font-size: 14px;" target="_blank">
                        <%--View--%></a>
        </div>
        <div style="width: 48%; float: left;">
            <a id="aDownloadResponse" style="cursor: pointer; font-size: 14px;" onclick="DownloadContent('textResponse');">
                <%--Download--%></a> <span style="font-size: 14px;">| </span><a id="aCopyResponse"
                    style="cursor: pointer; font-size: 14px;" onclick="CopyContent('textResponse');">
                    <%--Copy--%></a> <span style="font-size: 14px;">| </span><a id="aViewResponse" style="cursor: pointer;
                        font-size: 14px;" target="_blank">
                        <%--View--%></a>
        </div>
        <br />
        <div id="divRequest" class="roundedcorners" style="overflow: auto; width: 48%; margin-right: 20px;
            float: left; height: 200px; background-color: #ffffdd">
            <pre id="textRequest" class="content" style="word-wrap: break-word; white-space: pre-wrap;
                padding: 10px;">
                    </pre>
        </div>
        <div id="divResponse" class="roundedcorners" style="overflow: auto; float: left;
            width: 48%; height: 200px; background-color: #ffffdd;">
            <pre id="textResponse" class="content" style="word-wrap: break-word; white-space: pre-wrap;
                padding: 10px;">
                    </pre>
        </div>
    </div>
    <!-- Reg Left Section ...ends-->
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
    <!-- END OF DEVELOPER CODE -->
</asp:Content>
