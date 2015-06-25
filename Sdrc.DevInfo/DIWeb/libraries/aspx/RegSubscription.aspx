<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master" AutoEventWireup="true" CodeFile="RegSubscription.aspx.cs" Inherits="libraries_aspx_SDMX_sdmx_Subscription" Title="Untitled Page" EnableSessionState="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" Runat="Server">

     <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
     <script type="text/javascript" src="../js/jquery.ui.datepicker.js?v=<%=Global.js_version%>"></script>
     <script type="text/javascript" src="../js/sdmx/Subscription.js?v=<%=Global.js_version%>"></script>

<script type="text/javascript">

// create pivoting object
var DEMO = {};
window.cal = false;
DEMO.pivot = {panel:1,tab:0,div:"pivot",needs:["pivot"],cb:CallBack}

// initiation function for the pivoting
function init(){	
   
	/* tabs */
	var tab = new OAT.Tab("content");
	for (var p in DEMO) {
		var name = DEMO[p].div;
		tab.add ("tab_" + name, name);
	}

	/* panelbar_content */
	var pb = new OAT.Panelbar("panelbar",10);
	pb.addPanel("pb_1","pb_11");
	
	/* create dimmer element */
	var dimmerElm = OAT.Dom.create("div",{border:"2px solid #000",padding:"1em",position:"absolute",backgroundColor:"#fff"});
	document.body.appendChild(dimmerElm);
	OAT.Dom.hide(dimmerElm);
	
	for (var p in DEMO) { DEMO[p].drawn = false; }
	tab.options.goCallback = function(oldIndex,newIndex) {
		var oldName, newName;
		for (var p in DEMO) {
			var v = DEMO[p];
			if (v.tab == oldIndex) { oldName = p; }
			if (v.tab == newIndex) { newName = p; }
		}
		
		
		var obj = DEMO[newName];
				//OAT.Dimmer.show(dimmerElm);
				OAT.Dom.center(dimmerElm,1,1);
				var ref = function() {  
					if (!window.location.href.match(/:source/)) {
					
					BindSubscriptions();
					}				
				}				
				OAT.Loader.loadFeatures(obj.needs,ref);
				//alert('after loadfeatures');
			}
			pb.go(0);
			tab.go(0);
}

</script>

<script type="text/javascript">
 
      di_jq(document).ready(function() {
        di_jq("#txtStartDate").datepicker({ dateFormat: 'dd-mm-yy' });
        di_jq("#txtEndDate").datepicker({ dateFormat: 'dd-mm-yy' });

      });
 
</script>

	    <!-- Reg Wide Section ...starts-->
	    <div id="reg_wide_sec">
        <p><input type="button" value="" id="btnAddNew" class="submit_button"  onclick="OpenSubscriptionDetailsPopup('A','');" />
        &nbsp;<img id="imghelpAddSubscription" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpAddSubscription');"
                    style="cursor: pointer; margin-top: 10px; vertical-align:bottom;" onmouseout="HideCallout('divCallout')";/>
        </p>

        <div>
            <h3 id="lang_List_of_Subscriptions" class="flt_lft"><!--List of Subscriptions:--></h3>
    	    &nbsp;<img id="imghelpSubscription" src="../../stock/themes/default/images/help.gif" alt="Help" onclick="ToggleCallout('divCallout', event, 'divHelpSubscription');"
                    style="cursor: pointer; margin-top: 10px" onmouseout="HideCallout('divCallout')";/>
        </div>    
        <!-- OAT Grid ...starts-->
        <div id="divGridContainer" class="roundedcorners">
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
            <div id="divCountDataRows"></div>
            <div id="divSingleSource"></div>
        </div>   
        <!-- OAT Grid ...ends-->                                 	    
    	    
	    </div>
	    <!-- Reg Wide Section ...ends-->    	    
   
   
	    
<!-- Popup for Subscription Details ...starts-->
<div id="divSubscriptionDetails" class="popup_block">
 
    <!-- Popup Inside Content ...starts-->
    <div id="reg_content_containers">
<fieldset>    
        <h2 id="lang_Subscription_Details"><!--Subscription Details--></h2>
        <h5></h5>
        
        <div id="reg_wide_sec_ppup">
        
                <!-- Input Fields Area ...starts-->    
                <div id="confg_Adm_box_lgin">
                    <div>
                     <p class="confg_frm_txt_lgin_ppup flt_lft"><span  id="lang_Notification_Mail" ><!--Notification Mail--></span>
                     <img id="imghelpNotificationMail" src="../../stock/themes/default/images/help.gif" alt="Help" 
                     onclick="ToggleCallout('divCallout', event, 'divHelpNotificationMail');" style="vertical-align:top; margin-top:-5px;cursor:pointer;" 
                     onmouseout="HideCallout('divCallout')";/>
                    </p>
                    </div>
                   
                    <p class="confg_frm_inp_bx_big"><input type="text" id="txtNotificationMail" class="confg_frm_inp_bx_txt" />
                    <input type="checkbox" id="chkIsSOAP" /><span id="lang_SOAP_Protocol"><!--SOAP Protocol--></span>
                    <img id="imghelpSOAPProtocol" src="../../stock/themes/default/images/help.gif" alt="Help" 
                     onclick="ToggleCallout('divCallout', event, 'divHelpSOAPProtocolSubscription');" style="vertical-align:top; margin-top:-5px;cursor:pointer;" 
                     onmouseout="HideCallout('divCallout')";/>
                    </p> 
                    <div class="clear"></div> 
                    
                    <p class="confg_frm_txt_lgin_ppup" ><span id="lang_Notification_HTTP"><!--Notification HTTP--></span>
                    <img id="imghelpNotificationHTTP" src="../../stock/themes/default/images/help.gif" alt="Help" 
                     onclick="ToggleCallout('divCallout', event, 'divHelpNotificationHTTP');" style="vertical-align:top; margin-top:-5px;cursor:pointer;" 
                     onmouseout="HideCallout('divCallout')";/>
                    </p>
                    <p class="confg_frm_inp_bx"><input type="text" id="txtNotificationHTTP" class="confg_frm_inp_bx_txt" /></p> 
                    <div class="clear"></div>  
                    
                    <p class="confg_frm_txt_lgin_ppup reg_subs_vald_prd" id="lang_Validity_Period"><!--Validity Period--></p>
                    <p class="confg_frm_inp_bx"></p> 
                    <div class="clear"></div>    
                    
                    <p class="confg_frm_txt_lgin_ppup" id="lang_Start_Date"><!--Start Date--></p>
                    <p class="confg_frm_inp_bx"><input id="txtStartDate" type="text" class="confg_frm_inp_bx_txt_sml" /></p> 
                    <div class="clear"></div>     
                    
                    <p class="confg_frm_txt_lgin_ppup" id="lang_End_Date"><!--End Date--></p>
                    <p class="confg_frm_inp_bx"><input id="txtEndDate" type="text" class="confg_frm_inp_bx_txt_sml" /></p> 
                    <div class="clear"></div>   
                    
                    <br />
                    <p class="confg_frm_txt_lgin_ppup" ><span id="lang_preference"><!--Language Preference--></span>
                    <img id="imgHelpLangPref" src="../../stock/themes/default/images/help.gif" alt="Help" 
                     onclick="ToggleCallout('divCallout', event, 'divHelpLangPref');" style="vertical-align:top; margin-top:-5px;cursor:pointer;" 
                     onmouseout="HideCallout('divCallout')";/>
                    </p>
                    <p class="confg_frm_inp_bx">
                        <select id="selLang" class="confg_frm_inp_bx_txt_dd" onchange="">
                            <%--<option value="0" id="sel_langPref_option"><!--Language Preference--></option>--%>
                        </select>                    
                    </p>                    
                    
                    <div class="clear"></div>
                    <p class="confg_frm_txt_lgin_ppup" ><span id="lang_Event_Selector"><!--Event Selector--></span>
                    <img id="imghelpEventSelector" src="../../stock/themes/default/images/help.gif" alt="Help" 
                     onclick="ToggleCallout('divCallout', event, 'divHelpEventSelector');" style="vertical-align:top; margin-top:-5px;cursor:pointer;" 
                     onmouseout="HideCallout('divCallout')";/>
                    </p>

                    <p class="confg_frm_inp_bx">
                        <select id="selEventSelector" class="confg_frm_inp_bx_txt_dd" onchange="BindRegCategoryOrDataFlows();">
                            <option value="0" id="lang_Data_Registration"><!--Data Registration--></option>
                            <option value="1" id="lang_Metadata_Registration"><!--Metadata Registration--></option>
                            <option value="2" id="lang_Structural_Metadata_Registration"><!--Structural Metadata Registration--></option>
                        </select>                    
                    </p> 
                    <div class="reg_subs_ppup_inp_spc"></div> 	
                    <div id ="divCategory">
                         <p class="confg_frm_txt_lgin_ppup" id="lang_Category"><!--Category--></p>
                         <div class="confg_frm_inp_bx_big_ppup">
                            <div id="divRegistrationCategory">
                                <div id="divICTypes" class="di_gui_gradient_panel_box_ppup"></div>
                                <div id="divCatScheme"></div>
                            </div>                    
                         </div> 
                    </div>
                     <div id ="divMetadataFlows" style="display:none">
                        <p class="confg_frm_txt_lgin_ppup" id="lang_Metadata_Flows"><!--Metadata Flows--></p>
                        <p class="confg_frm_inp_bx">
                            <select id="selMFD" class="confg_frm_inp_bx_txt_dd">
                                <option value="0" id="lang_Select_MFD"><!-- -----------------------Select MFD--------------------- --></option>
                            </select>                    
                        </p> 
                     </div>
                   
                    <div class="clear"></div>                                                                                                       
                </div>     
                <!-- Input Fields Area ...ends-->  
				
				<!-- Configuration Button ...starts-->
				<div class="adm_upd_bttn_lgin_ppup">
				    <span id="rowAddSubscription" style="display:none"><input type="button" id="btnAddSubscription" value="" onclick="AddSubscription();" class="di_gui_button"/></span>
                    <span id="rowUpdateSubscription" style="display:none"><input type="button" id="btnUpdateSubscription" value="" onclick="UpdateSubscription();" class="di_gui_button"/></span>	        
                    <span id="rowDeleteSubscription" style="display:none"><input type="button" id="btnDeleteSubscription" value="" onclick="DeleteSubscription();" class="di_gui_button lng_bttn_sz" /></span>	
                    <span id="rowViewSubscription" style="display:none"></span>                    
				</div>
				<!-- Configuration Button ...ends--> 				

                
        </div>
</fieldset>      

    </div> 
    <!-- Popup Inside Content ...ends-->     
    
</div>      
<!-- Popup for Subscription Details ...ends-->	  
 <input type="hidden" id="hdnSubscriptionId" value=""/>
 <input type="hidden" id="hSubscription" value=""/>  
 <input type="hidden" id="hEmail" value=""/>  
 <input type="hidden" id="hWebServiceAddress" value=""/>  
 <input type="hidden" id="hStartDate" value=""/>  
 <input type="hidden" id="hEndDate" value=""/>  
 <input type="hidden" id="hEventType" value=""/>  
 <input type="hidden" id="hAction" value=""/>
 <input type="hidden" id="hLangPref" value=""/>
 <input type="hidden" id="hView" value=""/> 
 <input type="hidden" id="hEdit" value=""/> 
 <input type="hidden" id="hDelete" value=""/>
 <input type="hidden" id="hMessage" value=""/>  
 <input type="hidden" id="hErrorOccurred" value=""/>
 <input type="hidden" id="hMsgRegistryURN" value=""/>
 <input type="hidden" id="hMsgNotificationMailHTTP" value=""/>
 <input type="hidden" id="hMsgSubscriberAssignedId" value=""/>
 <input type="hidden" id="hMsgStartDate" value=""/>
 <input type="hidden" id="hMsgEndDate" value=""/>   
 <input type="hidden" id="hMsgInvalidEmail" value=""/>
 <input type="hidden" id="hMsgCategory" value=""/>   
 <input type="hidden" id="hMsgMFD" value=""/>
 <input type="hidden" id="hMsgSubcriptionDetailsNotRetrieved" value=""/>   
 <input type="hidden" id="hMsgDeleteSubscription" value=""/>
 <!-- Popup For Help starts -->
    <div id="divCallout" class="callout border-callout" style="z-index:2400">
        <div id="divCalloutText" class="content">
            <!-- Popup For Help Text starts -->
            <div id="divHelpAddSubscription" style="display:none">
              <span class="heading2" id="span_Help_Add_Subscription"></span>
              <br />
              <i class="content" id="i_Help_Add_Subscription"></i>
            </div>
            <div id="divHelpSubscription" style="display:none">
              <span class="heading2" id="span_Help_Subscription"></span>
              <br />
              <i class="content" id="i_Help_Subscription"></i>
            </div>
            <div id="divHelpNotificationMail" style="display:none">
              <span class="heading2" id="span_Help_Notification_Mail"></span>
              <br />
              <i class="content" id="i_Help_Notification_Mail"></i>
            </div>
            <div id="divHelpNotificationHTTP" style="display:none">
              <span class="heading2" id="span_Help_Notification_HTTP"></span>
              <br />
              <i class="content" id="i_Help_Notification_HTTP"></i>
            </div>
            <div id="divHelpSOAPProtocolSubscription" style="display:none">
              <span class="heading2" id="span_Help_SOAP_Protocol_Subscription"></span>
              <br />
              <i class="content" id="i_Help_SOAP_Protocol_Subscription"></i>
            </div>
            <div id="divHelpEventSelector" style="display:none">
              <span class="heading2" id="span_Help_Event_Selector"></span>
              <br />
              <i class="content" id="i_Help_Event_Selector"></i>
            </div>
              <div id="divHelpLangPref" style="display:none">
              <span class="heading2" id="span_Help_Lang_Pref"></span>
              <br />
              <i class="content" id="i_Help_Lang_Pref"></i>
            </div>
            <!-- Popup For Help Text ends -->
        </div>
        <b class="border-notch notch"></b><b class="notch"></b>
    </div>
<!-- Popup For Help ...ends -->	
   
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

