<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master" AutoEventWireup="true" CodeFile="RegPublishData.aspx.cs" Inherits="libraries_aspx_SDMX_sdmx_PublishData" Title="Publish Data" EnableSessionState="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" Runat="Server">
    <link href="../../stock/themes/default/css/style_dataview.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>

    <script type="text/javascript" src="../js/sdmx/PublishData.js?v=<%=Global.js_version%>"></script>
    
    <script type="text/javascript">

        // create pivoting object
        var DEMO = {};
        window.cal = false;
        DEMO.pivot = { panel: 1, tab: 0, div: "pivot", needs: ["pivot"], cb: CallBack }

        // initiation function for the pivoting
        function init() {

            /* tabs */
            var tab = new OAT.Tab("content");
            for (var p in DEMO) {
                var name = DEMO[p].div;
                tab.add("tab_" + name, name);
            }

            /* panelbar_content */
            var pb = new OAT.Panelbar("panelbar", 10);
            pb.addPanel("pb_1", "pb_11");

            /* create dimmer element */
            var dimmerElm = OAT.Dom.create("div", { border: "2px solid #000", padding: "1em", position: "absolute", backgroundColor: "#fff" });
            document.body.appendChild(dimmerElm);
            OAT.Dom.hide(dimmerElm);

            for (var p in DEMO) { DEMO[p].drawn = false; }
            tab.options.goCallback = function (oldIndex, newIndex) {
                var oldName, newName;
                for (var p in DEMO) {
                    var v = DEMO[p];
                    if (v.tab == oldIndex) { oldName = p; }
                    if (v.tab == newIndex) { newName = p; }
                }


                var obj = DEMO[newName];
                //OAT.Dimmer.show(dimmerElm);
                OAT.Dom.center(dimmerElm, 1, 1);
                var ref = function () {
                    if (!window.location.href.match(/:source/)) {

                        GetRegistrationsSummary(document.getElementById('hdbnid').value, document.getElementById('hLoggedInUserNId').value.split('|')[0], document.getElementById('hlngcodedb').value);
                    }
                }
                OAT.Loader.loadFeatures(obj.needs, ref);
                //alert('after loadfeatures');
            }
            pb.go(0);
            tab.go(0);
        }

    </script>

	    <!-- Reg Wide Section ...starts-->
	    <div id="reg_wide_sec">
 <%--           <table style="width:100%;">
         <tr>
            <td>
                <span id="spanAction" class="confg_frm_txt_lgin"></span>
                 <input type="hidden" id="hdnSelectedDatabase" value="" />
                   <input type="hidden" id="hdnSelectedDSD" value="" />
            </td>
            <td>--%>
                <p id="spanAction" class="confg_frm_txt_lgin" style="width:100px"><%--Action:--%></p>
                <input type="hidden" id="hdnSelectedDatabase" value="" />
                <input type="hidden" id="hdnSelectedDSD" value="" />
                <p class="flt_lft "> <%-- confg_frm_inp_bx_big--%>
                    <select id="selectAction" class="confg_frm_inp_bx_txt_dd" onchange="SelectActionChange();">
                        <option value="0" selected="selected" id="SelectOne"><%----Select One----%></option>
                        <option value="1" id="GenerateSDMXML"><%--Generate SDMX-ML--%></option>
                  <%--      <option value="2" id="RegisterSDMXML">Register SDMX-ML</option>--%>
                        <option value="3" id="GenerateMetadata"><%--Generate Metadata--%></option>
                      <%--  <option value="4" id="RegisterMetadata">--Register Metadata--</option>--%>
                    </select>
                    <span class="dsp_blck">
                        <span id="spanGenerateSDMXMLDescription" class="content" style="color:#aaaaaa; display:none;"><%--Generate SDMX-ML files from databases or in case of uploaded DSDs from associated databases.--%></span>
                        <span id="spanRegisterSDMXMLDescription" class="content" style="color:#aaaaaa; display:none;"><%--Register already generated SDMX-ML files.--%></span>
                        <span id="spanGenerateMetadataDescription" class="content" style="color:#aaaaaa; display:none;"><%--Generate Metadata Reports from databases or in case of uploaded DSDs from associated databases.--%></span>
                        <span id="spanRegisterMetadataDescription" class="content" style="color:#aaaaaa; display:none;"><%--Register already generated Metadata Reports.--%></span>
                    </span>
                </p>
                <div class="clear"></div>
<%--            </td>

        </tr>

        </table>--%>
         
            <%--
          <table id="tblAction" style="width:100%; display:none;">
            <tr style="line-height:15px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
        <td>
         <div id="divIndicator" style="display:none;height:200px;overflow:auto;"></div>
       <input type="hidden" id="hdnSelectedIndicators" value="" />
        </td>
        </tr>
            
        <tr id="rowGenerateSDMXML">
            <td colspan="2">
                <input id="btnGenerateSDMXML" type="button" onclick="GenerateSDMXML();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
                 <input id="btnSave" type="button" onclick="SaveSelection();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
        </tr>
       
        <tr id="rowGenerateMetadata">
            <td colspan="2">
                <input id="btnGenerateMetadata" type="button" onclick="GenerateMetadata();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
        </tr>
      
          </table>--%>
          <div id="divIndicatorGrid" style="display:none;overflow:auto;" class="mrgn_tp_tn"></div>
        <table id="tblAction" style="width:100%; display:none;">
         <tr id="rowIndicatorSelection">
            <td style="width:10%">
                <span id="spanIndicator" class="confg_frm_txt_lgin"><%--Indicator:--%></span>
            </td>
            <td style="width:90%">
                <a id="aIndicatorSelect" style="cursor:pointer;" onclick="OpenIndicatorPopup();"><%--Select--%></a>
                <span> | </span>
                <a id="aIndicatorClear" style="cursor:pointer;" onclick="ClearIndicatorSelections();"><%--Clear--%></a>
                &nbsp;&nbsp;
                <i id="spanSelectedIndicators" class="content" style="color:#aaaaaa;"></i>
            <%--    <input type="hidden" id="hdnSelectedIndicators" value="" />--%>
            </td>
        </tr>
        <tr style="line-height:0px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr id="rowAreaSelection">
            <td>
                <span id="spanArea" class="confg_frm_txt_lgin"><%--Area:--%></span>
            </td>
            <td>
                <a id="aAreaSelect" style="cursor:pointer;" onclick="OpenAreaPopup(this);"><%--Select--%></a>
                <span> | </span>
                <a id="aAreaClear" style="cursor:pointer;" onclick="ClearAreaSelections();"><%--Clear--%></a>
                &nbsp;&nbsp;
                <i id="spanSelectedAreas" class="content" style="color:#aaaaaa;"></i>
               <%-- <input type="hidden" id="hdnSelectedAreas" value="" />--%>
            </td>
        </tr>
        <tr style="line-height:0px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr id="rowTPSelection">
            <td>
                <span id="spanTP" class="confg_frm_txt_lgin"><%--Time:--%></span>
            </td>
            <td>
                <a id="aTPSelect" style="cursor:pointer;" onclick="OpenTPPopup(this);"><%--Select--%></a>
                <span> | </span>
                <a id="aTPClear" style="cursor:pointer;" onclick="ClearTPSelections();"><%--Clear--%></a>
                &nbsp;&nbsp;
                <i id="spanSelectedTPs" class="content" style="color:#aaaaaa;"></i>
               <%-- <input type="hidden" id="hdnSelectedTPs" value="" />--%>
            </td>
        </tr>
        <tr style="line-height:0px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr id="rowSourceSelection">
            <td>
                <span id="spanSource" class="confg_frm_txt_lgin"><%--Source:--%></span>
            </td>
            <td>
                <a id="aSourceSelect" style="cursor:pointer;" onclick="OpenSourcePopup(this);"><%--Select--%></a>
                <span> | </span>
                <a id="aSourceClear" style="cursor:pointer;" onclick="ClearSourceSelections();"><%--Clear--%></a>
                &nbsp;&nbsp;
                <i id="spanSelectedSources" class="content" style="color:#aaaaaa;"></i>
               <%-- <input type="hidden" id="hdnSelectedSources" value="" />--%>
            </td>
        </tr>
        <tr style="line-height:15px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr id="rowGenerateSDMXML">
            <td>
                <input id="btnGenerateSDMXML" type="button" onclick="GenerateSDMXML();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
                
            </td>
            <td>
             <input id="btnSave" type="button" onclick="SaveSelection();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;display:none"/>
            </td>
              <td>
             <a id="lnkErrorLog"  href="javascript:void(0);" onclick="OpenErrorLog();" style="display:none"><i>Error Log</i></a>
            </td>
        </tr>
     <%--   <tr id="rowRegisterSDMXML">
            <td colspan="2">
                <input id="btnRegisterSDMXML" type="button" onclick="RegisterSDMXML();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
        </tr>--%>
        <tr id="rowGenerateMetadata">
            <td colspan="2">
                <input id="btnGenerateMetadata" type="button" onclick="GenerateMetadata();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
              <td>
             <input id="btnSaveMetadata" type="button" onclick="SaveMetadataSelection();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;display:none"/>
            </td>
        </tr>
     <%--   <tr id="rowRegisterMetadata">
            <td colspan="2">
                <input id="btnRegisterMetadata" type="button" onclick="RegisterMetadata();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
        </tr>--%>
    </table>
    
            <p class="pddn_tp_bttm_thrty"><input type="button" id="btnAddNewRegistration" value="" onclick="OpenRegistrationDetailsPopup('A', '');" class="di_gui_button" />
        &nbsp;<img id="imghelpAddRegistration" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpAddRegistration');"
                    style="cursor: pointer; margin-top: 10px; vertical-align:bottom;" onmouseout="HideCallout('divCallout')";/>
        </p>

            <div>
                <h3 id="lang_List_of_Registrations_And_Constraints" class="flt_lft"><%--List of Registrations And Constraints:--%></h3>
    	    &nbsp;<img id="imghelpRegistrationAndConstraint" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpRegistrationAndConstraint');"
                        style="cursor: pointer; margin-top: 10px" onmouseout="HideCallout('divCallout')";/>
            </div>      
        
              <div id="divResultsCount" class="content" style="float:right"></div>
            <div id="divGridContainer" class="roundedcorners pddn_fv pddn_lft_nne">
            
            <!-- OAT Grid ...starts-->
            <div id="gridData">
                <div id="dataTitle" class="heading2"></div>
                <div id="dataSubTitle"></div>
                <div id="content"></div>
                <!--<br />-->
                <div id="pivot" class="pivot_sec">
                    <div id="pivot_page"></div>
                    <div id="pivot_content"></div>
                    <div id="pivot_chart"></div>
                </div>
            </div>
      <%--      <div id="divResultsCount" class="content" style="padding:5px 0 10px"></div>--%>
      <div style="" class="txt_algn_cntr">
            <a id="aPrevious" onclick="GetPreviousPageRegistrations();" style="cursor:pointer;"><%--Previous--%></a>&nbsp;|&nbsp;
            <a id="aNext" onclick="GetNextPageRegistrations();" style="cursor:pointer;"><%--Next--%></a>
            </div>
            <div id="divCountDataRows"></div>
            <div id="divSingleSource"></div>
        </div>   
              <!-- OAT Grid ...ends-->    
                <input type="hidden" id="hdnSelectedIndicators" value="" />
            <div id='IndicatorPopup' class="popup_block" style="width:700px; height:475px;">		
           <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td class="popup_heading1" id="lang_SelectIndicator"><%--Select Indicator--%></td>
            </tr>			
            <tr>
                <td width="100%" valign="top">
                    <table  width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
                        <tr>
                            <td>
                                  <div id="divIndicator" style="height:420px; overflow:auto; border:1px solid #d3d3d3;"></div>
                            </td>
                        </tr>
                    <tr>
                        <td>
                            <div class="di_gui_button_pos">
                                <input type="button" id="btnIndicatorOK" onclick="IndicatorPopupOk();" class="di_gui_button" />
                                <input type="button" id="btnIndicatorCancel" onclick="IndicatorPopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
                            </div>
                        </td>
                    </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>                             	    
    	     <input type="hidden" id="hdnSelectedAreas" value="" />
            <div id='AreaPopup' class="popup_block" style="width:700px; height:475px;">		
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td class="popup_heading1" id="lang_SelectArea"><%--Select Area--%></td>
            </tr>			
            <tr>
                <td width="100%" valign="top">
                    <table  width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
                        <tr>
                            <td>
                                <div id="divArea" style="height:420px; overflow:auto; border:1px solid #d3d3d3;"></div>
                            </td>
                        </tr>
                    <tr>
                        <td>
                            <div class="di_gui_button_pos">
                                <input type="button" id="btnAreaOK" onclick="AreaPopupOk();" class="di_gui_button" />
                                <input type="button" id="btnAreaCancel" onclick="AreaPopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
                            </div>
                        </td>
                    </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
             <input type="hidden" id="hdnSelectedTPs" value="" />
            <div id='TPPopup' class="popup_block" style="width:700px; height:475px;">		
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td class="popup_heading1" id="lang_SelectTime"><%--Select Time--%></td>
            </tr>			
            <tr>
                <td width="100%" valign="top">
                    <table  width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
                        <tr>
                            <td>
                                <div id="divTP" style="height:420px; overflow:auto; border:1px solid #d3d3d3;"></div>
                            </td>
                        </tr>
                    <tr>
                        <td>
                            <div class="di_gui_button_pos">
                                <input type="button" id="btnTPOK" onclick="TPPopupOk();" class="di_gui_button" />
                                <input type="button" id="btnTPCancel" onclick="TPPopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
                            </div>
                        </td>
                    </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
              <input type="hidden" id="hdnSelectedSources" value="" />
            <div id='SourcePopup' class="popup_block" style="width:700px; height:475px;">		
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td class="popup_heading1" id="lang_SelectSource"><%--Select Source--%></td>
            </tr>			
            <tr>
                <td width="100%" valign="top">
                    <table  width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
                        <tr>
                            <td>
                                <div id="divSource" style="height:420px; overflow:auto; border:1px solid #d3d3d3;"></div>
                            </td>
                        </tr>
                    <tr>
                        <td>
                            <div class="di_gui_button_pos">
                                <input type="button" id="btnSourceOK" onclick="SourcePopupOk();" class="di_gui_button" />
                                <input type="button" id="btnSourceCancel" onclick="SourcePopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
                            </div>
                        </td>
                    </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>




	    </div>
	    <!-- Reg Wide Section ...ends-->    
	    
         <div id='ErrorPopup' class="popup_block" style="width:500px; height:275px;">		
             <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td class="popup_heading1" id="Td1">Error Log</td>
            </tr>			
            <tr>
                <td width="100%" valign="top">
                    <table  width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
                        <tr>
                            <td>
                                <div id="divErrorLog" style="height:238px; overflow:auto; border:1px solid #d3d3d3;"></div>
                            </td>
                        </tr>
                   
                    </table>
                </td>
            </tr>
        </table>
    </div>












	    
<!-- Popup for Registration Details ...starts-->
<div id="divRegistrationDetails" class="popup_block" style="height:400px">	
    
    <!-- Popup Inside Content ...starts-->
    <div id="reg_content_containers">
        <h2 id="lang_Registration_Details"><!--Registration Details--></h2>
        <h5></h5>
        
        <div id="reg_wide_sec_ppup">
                <h3 id="spanQueryableDataSource" class="flt_lft"><!--Queryable Data Repository Details--></h3> 
	                &nbsp;<img id="imghelpQueryableDataRepository" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpQueryableDataRepository');"
                        style="cursor: pointer; margin-top: 10px" onmouseout="HideCallout('divCallout')";/> 
                <div class="clear"></div>
	       
	        
                <!-- Input Fields Area ...starts-->    
                <div id="confg_Adm_box_lgin">
                     
                    <p class="confg_frm_txt_lgin""><span   id="spanWebServiceURL" ><!--Web Service URL:--></span>
                    <img id="imghelpWebServiceURL" src="../../stock/themes/default/images/help.gif" alt="Help" 
                     onclick="ToggleCallout('divCallout', event, 'divHelpWebServiceURL');" style="vertical-align:top; margin-top:-5px;cursor:pointer;" 
                     onmouseout="HideCallout('divCallout')";/>
                    </p>
                    <p class="confg_frm_inp_bx"><input type="text" id="txtWebServiceURL" class="confg_frm_inp_bx_txt" /></p> 
                    <div class="clear"></div> 
                    
                    <p class="confg_frm_txt_lgin" ><span  id="spanIsREST"><!--REST Protocol--></span>
                    <img id="imghelpRestProtocol" src="../../stock/themes/default/images/help.gif" alt="Help" 
                     onclick="ToggleCallout('divCallout', event, 'divHelpRestProtocol');" style="vertical-align:top; margin-top:-5px;cursor:pointer;" 
                     onmouseout="HideCallout('divCallout')";/>
                    </p>  
                    <p class="confg_frm_inp_bx_chkbx_lgn"><input type="checkbox" id="chkIsREST" onclick="EnableDisableAssociatedTextBox('txtWADLURL');"/></p>     
                    <div class="clear"></div>    
                    
                    <p class="confg_frm_txt_lgin"><span  id="spanWADLURL"><!--WADL URL:--></span>
                    <img id="imghelpWADLURL" src="../../stock/themes/default/images/help.gif" alt="Help" 
                     onclick="ToggleCallout('divCallout', event, 'divHelpWADLURL');" style="vertical-align:top; margin-top:-5px;cursor:pointer;" 
                     onmouseout="HideCallout('divCallout')";/>
                    </p>
                    <p class="confg_frm_inp_bx"><input type="text" id="txtWADLURL" disabled="disabled" class="confg_frm_inp_bx_txt" /></p> 
                    <div class="clear"></div>  
                    
                    <p class="confg_frm_txt_lgin" ><span  id="spanIsSOAP"><!--SOAP Protocol--></span>
                    <img id="imghelpSOAPProtocol" src="../../stock/themes/default/images/help.gif" alt="Help" 
                     onclick="ToggleCallout('divCallout', event, 'divHelpSOAPProtocolRegistration');" style="vertical-align:top; margin-top:-5px;cursor:pointer;" 
                     onmouseout="HideCallout('divCallout')";/>
                    </p>
                    <p class="confg_frm_inp_bx_chkbx_lgn"><input type="checkbox" id="chkIsSOAP" onclick="EnableDisableAssociatedTextBox('txtWSDLURL');"/></p> 
                    <div class="clear"></div>    
                    
                    <p class="confg_frm_txt_lgin" ><span  id="spanWSDLURL"><!--WSDL URL:--></span>
                    <img id="imghelpWSDLURL" src="../../stock/themes/default/images/help.gif" alt="Help" 
                     onclick="ToggleCallout('divCallout', event, 'divHelpWSDLURL');" style="vertical-align:top; margin-top:-5px;cursor:pointer;" 
                     onmouseout="HideCallout('divCallout')";/>
                    </p>
                    <p class="confg_frm_inp_bx"><input type="text" id="txtWSDLURL" disabled="disabled" class="confg_frm_inp_bx_txt" /></p> 
                    <div class="clear"></div>  

                    <p class="confg_frm_txt_lgin" id="P1"><span id="lang_IsMetadata"><!--Is Metadata:--></span></p>
                    <p class="confg_frm_inp_bx_chkbx_lgn"><input type="checkbox" id="chkIsMetadata" onclick="BindDFDAndMFDList();"/></p> 
                    <div class="clear"></div>  
                    
                    <p class="confg_frm_txt_lgin" id="lang_Select_DM_Flows"><!--Select Data/Metadata Flows:--></p>
                    <p class="confg_frm_inp_bx">
                        <select id="selDFDMFD" class="confg_frm_inp_bx_txt_dd">
                            <option value="0" id="lang_Select_DFD_MFD"><!-- -----------------------Select DFD/MFD--------------------- --></option>
                        </select>                    
                    </p>  
                    <div class="clear"></div>    
                    
	            <h3 id="spanSimpleDataSource" class="flt_lft"><!--Simple Data/Metadata Repository Details--></h3>    
	                &nbsp;<img id="imghelpSimpleDataMetadataRepository" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpSimpleDataMetadataRepository');"
                            style="cursor: pointer; margin-top: 10px" onmouseout="HideCallout('divCallout')";/>   
	                 <div class="clear"></div>    
                    <p class="confg_frm_txt_lgin" id="spanFileURL"><!--SDMX-ML File/Metadata Report URL: --></p>
                    <p class="confg_frm_inp_bx"><input type="text" id="txtFileURL" class="confg_frm_inp_bx_txt" /></p> 
                    <div class="clear"></div> 	                                                                                
                    
                </div>     
                <!-- Input Fields Area ...ends-->  
				
				<!-- Configuration Button ...starts-->
				<div class="adm_upd_bttn_lgin">
	    			<span id="rowAddRegistration" style="display:none;"><input type="button" id="btnAddRegistration" value="" onclick="AddRegistrationAndSendNotification();" class="di_gui_button"/></span>
                    <span id="rowUpdateRegistration" style="display:none;"><input type="button" id="btnUpdateRegistration" value="" onclick="UpdateRegistrationAndSendNotification();" class="di_gui_button"/></span>	
                    <span id="rowDeleteRegistration" style="display:none;"><input type="button" id="btnDeleteRegistration" value="" onclick="DeleteRegistration();" class="di_gui_button" /></span>
                    <span id="rowViewRegistration" style="display:none;"><input type="button" id="btnSendNotification" value="" onclick="SendNotification();" class="di_gui_button" /></span>                    	
				</div>
				<!-- Configuration Button ...ends--> 				
                
        </div>
        
    </div> 
    <!-- Popup Inside Content ...ends-->     
    
</div>      
<!-- Popup for Registration Details ...ends-->	 

<!-- Popup For Help starts -->
    <div id="divCallout" class="callout border-callout" style="z-index:2400">
        <div id="divCalloutText" class="content">
            <!-- Popup For Help Text starts -->
            <div id="divHelpAddRegistration" style="display:none">
              <span class="heading2" id="span_Help_Add_Registration"></span>
              <br />
              <i class="content" id="i_Help_Add_Registration"></i>
            </div>
            <div id="divHelpRegistrationAndConstraint" style="display:none">
              <div>
                  <span class="heading2"  id="span_Help_Registration"></span>
                  <br />
                  <i class="content" id="i_Help_Registration"></i>
             </div>
             <div>
                  <span class="heading2" id="span_Help_Constraint"></span>
                  <br />
                  <i class="content" id="i_Help_Constraint"></i>
             </div>
            </div>
            <div id="divHelpWebServiceURL" style="display:none">
              <span class="heading2" id="span_Help_WebService_URL"></span>
              <br />
              <i class="content" id="i_Help_WebService_URL"></i>
            </div>
            <div id="divHelpRestProtocol" style="display:none">
              <span class="heading2" id="span_Help_Rest_Protocol"></span>
              <br />
              <i class="content" id="i_Help_Rest_Protocol"></i>
            </div>
            <div id="divHelpWADLURL" style="display:none">
              <span class="heading2" id="span_Help_WADL_URL"></span>
              <br />
              <i class="content" id="i_Help_WADL_URL"></i>
            </div>
            <div id="divHelpSOAPProtocolRegistration" style="display:none">
              <span class="heading2" id="span_Help_SOAP_Protocol_Registration"></span>
              <br />
              <i class="content" id="i_Help_SOAP_Protocol_Registration"></i>
            </div>
            <div id="divHelpWSDLURL" style="display:none">
              <span class="heading2" id="span_Help_WSDL_URL"></span>
              <br />
              <i class="content" id="i_Help_WSDL_URL"></i>
            </div>
            <div id="divHelpQueryableDataRepository" style="display:none">
              <span class="heading2" id="span_Help_Queryable_Data_Repository"></span>
              <br />
              <i class="content" id="i_Help_Queryable_Data_Repository"></i>
            </div>
            <div id="divHelpSimpleDataMetadataRepository" style="display:none">
              <span class="heading2" id="span_Help_Simple_Data_Metadata_Repository"></span>
              <br />
              <i class="content" id="i_Help_Simple_Data_Metadata_Repository"></i>
            </div>
            <!-- Popup For Help Text ends -->
        </div>
        <b class="border-notch notch"></b><b class="notch"></b>
    </div>
<!-- Popup For Help ...ends -->	
         <input type="hidden" id="hSelectAll" />
        <input type="hidden" id="hdnRegistrationId" value=""/>
        <input type="hidden" id="hId" value=""/>
           <input type="hidden" id="hIndicator" value=""/>
              <input type="hidden" id="hLanguage" value=""/>
        <input type="hidden" id="hQueryableData" value=""/>
        <input type="hidden" id="hWADL" value=""/>
        <input type="hidden" id="hWSDL" value=""/>
        <input type="hidden" id="hSimpleData" value=""/>
        <input type="hidden" id="hDataMetadata" value=""/>
        <input type="hidden" id="hDFDMFD" value=""/>
        <input type="hidden" id="hAction" value=""/>
        <input type="hidden" id="hConstraints" value=""/>
        <input type="hidden" id="hView" value=""/>
        <input type="hidden" id="hEdit" value=""/>
        <input type="hidden" id="hDelete" value=""/>
        <input type="hidden" id="hDownload" value=""/>
        <input type="hidden" id="hData" value=""/>
        <input type="hidden" id="hMetadata" value=""/>
        <input type="hidden" id="hStartIndex" value="0"/>
        <input type="hidden" id="hNumberPagingRows" value="0"/>
        <input type="hidden" id="hTotalRows" value="0"/>
        <input type="hidden" id="hNoRegistrationsFound" value=""/>
        <input type="hidden" id="hErrorOccurred" value=""/>
        <input type="hidden" id="hDeleteRegistrationConfirmation" value=""/>
        <input type="hidden" id="hWADLRequired" value=""/>
        <input type="hidden" id="hWSDLRequired" value=""/>
        <input type="hidden" id="hSelectDataMetadataFlow" value=""/>
        <input type="hidden" id="hProvideWSUrlOrSDMXMLFileUrl" value=""/>
        <input type="hidden" id="hRegistrationWithId" value=""/>
        <input type="hidden" id="hDeletedSuccessfully" value=""/>
        <input type="hidden" id="hdnDSDORDB" />
          <input type="hidden" id="hdnLangTotalRows" value=""/>
            <input type="hidden" id="hdnMappingNotFound" />
    <input type="hidden" id="hdnNoDataFound" />

    <input type="hidden" id="hdnDBAreaId" />
    <input type="hidden" id="hdnSelectionSaved" />
   <input type="hidden" id="hdnSelectionError" />
   <input type="hidden" id="hdnErrorOccured" />
   <input type="hidden" id="hdnSelectOneIndicator" />
   
                     
<!-- For OAT Grid Purpose ...starts-->        
        <div id="tab_pivot333">
             <table width="100%" cellpadding="0" cellspacing="2" border="0" style="float:left;">
                 <tr>
                     <td align="right">
                         <div id="panelbar">
                             <div id="pb_1">
                             </div>
                             <div id="pb_11">
                             </div>
                         </div>
                     </td>
                 </tr>
             </table>
        </div>   
<!-- For OAT Grid Purpose ...ends-->            

    
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

    <script type="text/javascript">        onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=hOriginaldbnid %>');
    </script>

    <!-- END OF DEVELOPER CODE -->
</asp:Content>


