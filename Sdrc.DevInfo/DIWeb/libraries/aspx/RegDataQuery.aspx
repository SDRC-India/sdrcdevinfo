<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true"
    CodeFile="RegDataQuery.aspx.cs" Inherits="libraries_aspx_RegDataQuery" Title="Query Data"%>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">
    <!-- START INCLUDE JAVASCRIPTS -->
    
    <script type="text/javascript" src="../js/di.jquery-1.4.4.js"></script>
    <script type="text/javascript" src="../js/sdmx/DataQuery.js"></script>
    
    <!-- END INCLUDE JAVASCRIPTS -->
    
    
    <!-- START REGION FOR HIDDEN INPUT FIELDS FOR LANGUAGE HANDLING OF AREA & INDICATOR COMPONENTS -->

    <input type="hidden" id="langHiddenBoth" value="" />
    <input type="hidden" id="langHiddenTree" value="" />
    <input type="hidden" id="langHiddenAtoZ" value="" />
    <input type="hidden" id="langHiddenSearch" value="" />
    <input type="hidden" id="langHiddenViewselections" value="" />
    <input type="hidden" id="langHiddenSector" value="" />
    <input type="hidden" id="langHiddenGoal" value="" />
    <input type="hidden" id="langHiddenSource" value="" />
    <input type="hidden" id="langHiddenInstitution" value="" />
    <input type="hidden" id="langHiddenTheme" value="" />
    <input type="hidden" id="langHiddenConvention" value="" />
    <input type="hidden" id="langHiddenConceptualFramework" value="" />
    <input type="hidden" id="langHiddenselectsubgroup" value="" />
    <input type="hidden" id="langHiddenSelectAll" value="" />
    <input type="hidden" id="langHiddenShowwheredataexist" value="" />
    <input type="hidden" id="langHiddenOK" value="" />
    <input type="hidden" id="langHiddenClear" value="" />
    <input type="hidden" id="langHiddenClose" value="" />
    <input type="hidden" id="langHiddenSubgroupDimensions" value="" />
    <input type="hidden" id="langHiddenMore" value="" />
    <input type="hidden" id="langHiddenMetadata" value="" />
    <input type="hidden" id="langHiddenAlphabetically" value="" />

    <input type="hidden" id="langHiddenSelectarealevel" value="" />
    <input type="hidden" id="langHiddenByTree" value="" />
    <input type="hidden" id="langHiddenListAtoZ" value="" />
    <input type="hidden" id="langHiddenByMap" value="" />
    <input type="hidden" id="langHiddenFirst" value="" />
    <input type="hidden" id="langHiddenBack" value="" />
    <input type="hidden" id="langHiddenNext" value="" />
    <input type="hidden" id="langHiddenLast" value="" />
    <input type="hidden" id="langHiddenPage" value="" /> 

     <!-- END REGION FOR HIDDEN INPUT FIELDS FOR LANGUAGE HANDLING OF AREA & INDICATOR COMPONENTS -->

    <!-- START MARKUP -->
    
    <div class="content_containers">

        <div id="reg_content_containers">

            <h2 id="spanPageTitle"><!--Query Data--></h2>
		    <h5 id="pPageDescription"><!--Make queries and retrieve data in various formats.--></h5>

            <div id="reg_wide_sec">
        
                <!-- Input Fields Area ...starts -->
                <div class="confg_Adm_box"> 

                    <p class="confg_frm_txt_lgin" id="spanLanguage"><!--Language:--></p>
                    <p class="confg_frm_inp_bx_wide"><select id="selectLanguage" class="confg_frm_inp_bx_txt_dd"></select></p>
                    <div class="clear"></div>
                
                    <p class="confg_frm_txt_lgin" id="spanIndicator"><!--Indicator:--></p>
                    <p class="confg_frm_inp_bx_txtonly">
                            <a id="aIndicator" class="cur_pointr" onclick="showHideComponent('ind');"><!--Select Indicator--></a>
                            <i id="spanSelectedIndicators" class="content spanSelected_itlc_txt"></i>                    
                    </p>
                    <div class="clear"></div>

                    <p class="confg_frm_txt_lgin" id="spanArea"><!--Area:--></p>
                    <p class="confg_frm_inp_bx_txtonly">
                            <a id="aArea" class="cur_pointr" onclick="showHideComponent('area');"><!--Select Area--></a>
                            <i id="spanSelectedAreas" class="content spanSelected_itlc_txt"></i>                    
                    </p>
                    <div class="clear"></div>

                    <p class="confg_frm_txt_lgin" id="selectMRD"><!--Most Recent Data:--></p>
                    <p class="confg_frm_inp_bx_wide"><input id="chkMRD" type="checkbox" onclick="EnableDisableTimeRangeDropDowns();" /></p>
                    <div class="clear"></div>

                    <div id="selectOr" class="selectOr"> <!--OR--> </div>

                    <p class="confg_frm_txt_lgin" id="spanTimeRange"><!--Time:--></p>
                    <p class="confg_frm_inp_bx_txtonly"><input type="hidden" id="hdnTimeRange" value="" class="confg_frm_inp_bx_txt" />
                            <a id="aTimeRange" class="cur_pointr" onclick="showHideComponent('tp');"><!--Select Time--></a>
                            <i id="spanSelectedTPs" class="content spanSelected_itlc_txt"></i>                    
                    </p>
                    <div class="clear"></div>

                    <p class="confg_frm_txt_lgin" id="spanSource"><!--Source:--></p>
                    <p class="confg_frm_inp_bx_txtonly"><input type="hidden" id="hdnSource" value="" class="confg_frm_inp_bx_txt" />
                            <a id="aSource" class="cur_pointr" onclick="showHideComponent('source');"><!--Select Source--></a>
                            <i id="spanSelectedSources" class="content spanSelected_itlc_txt"></i>                    
                    </p>
                    <div class="clear"></div>

                    <p class="confg_frm_txt_lgin" id="spanRequestFormat"><!--Request Format:--></p>
                    <p class="confg_frm_inp_bx_wide">                
                            <select id="selectRequestFormat" class="confg_frm_inp_bx_txt_dd" onchange="ArrangeResponseOptions(this.value);">
                                <option value="0" selected="selected" id="lang_SOAP"><!--SOAP--></option>
                                <option value="1" id="lang_REST"><!--REST--></option>
                                <option value="2" id="lang_HTTP"><!--HTTP--></option>
                            </select>    
                    </p>
                    <div class="clear"></div>

                    <p class="confg_frm_txt_lgin" id="spanResponseFormat"><!--Response Format:--></p>
                    <p class="confg_frm_inp_bx_wide">                
                            <select id="selectResponseFormat" class="confg_frm_inp_bx_txt_smlst flt_lft" onclick="ShowHideTableSDMX();" onchange="ShowHideTableOptions(this.value)">
                                <option value="0" selected="selected" id="lang_SDMX"><!--SDMX--></option>
                                <option value="1" id="lang_JSON"><!--JSON--></option>
                                <option value="2" id="lang_XML"><!--XML--></option>                               
                            </select>
                            <select id="selectSDMXFormat" class="confg_frm_inp_bx_txt_sml">
                                <option value="0" id="lang_Generic"><!--Generic--></option>
                                <option value="1" id="lang_Generic_Time_Series"><!--Generic Time Series--></option>
                                <option value="2" id="lang_Structure_Specific"><!--Structure Specific--></option>
                                <option value="3" selected="selected" id="lang_Structure_Specific_TS"><!--Structure Specific Time Series--></option>
                            </select>
                    </p>
                    <div class="clear"></div>

                    <p class="confg_frm_txt_lgin"></p>
                    <div class="confg_frm_inp_bx_wide"> 
                        <div id="tableFormatOptions" class="confg_frm_inp_bx_txt_big" style="width:427px;display:none;">
                            <div class="confg_frm_txt_lgin" style="display:none;"><input type="checkbox" id="chkboxGbyi"/><label for="chkboxGbyi">Group By Indicator</label></div>
                            <div class="clear"></div>

                            <div class="confg_frm_txt_lgin"><input type="checkbox" id="chkboxTitle" onchange="EnableDiableTextbox(this,'txtboxTitle')"/><label for="chkboxTitle">Title</label></div>
                            <div class="flt_lft"><input id="txtboxTitle" type="text" disabled="ture" class="confg_frm_inp_bx_txt_sml" /></div>
                            <div class="clear"></div>
                            
                            <div class="confg_frm_txt_lgin"><input type="checkbox" id="chkboxFootnote" onchange="EnableDiableTextbox(this,'txtboxFootnote')"/><label for="chkboxFootnote">Footnote</label></div>
                            <div class="flt_lft"><input id="txtboxFootnote" type="text" disabled="ture" class="confg_frm_inp_bx_txt_sml" /></div>
                            <div class="clear"></div>
                        
                    </div>
                    <div class="clear"></div>

<%--                    <p id="tableFormatOptions" class="confg_frm_inp_bx_txt_smlst" style="height:60px;width:300px;display:none;">
                        <input type="checkbox" id="chkboxGbyi"/><label for="chkboxGbyi">Group By Indicator</label><br />
                        <input type="checkbox" id="chkboxTitle" onchange="EnableDiableTextbox(this,'txtboxTitle')"/><label for="chkboxTitle">Title</label><input id="txtboxTitle" type="text" disabled="ture"/><br />
                        <input type="checkbox" id="chkboxFootnote" onchange="EnableDiableTextbox(this,'txtboxFootnote')"/><label for="chkboxFootnote">Footnote</label><input id="txtboxFootnote" type="text" disabled="ture"/>
                    </p>
                    <div class="clear"></div>--%>
                    <span id="lng_Table" style="display:none;"><!--Table--></span>
                </div>
                <!-- Input Fields Area ...ends -->
                 <div class="clear"></div>
    <!-- Button ...starts-->
    <div class="adm_upd_bttn"><input id="btnGetQueryAndResponse" type="button" value="" onclick="GetRequests();" class="di_gui_button lng_bttn_sz"/></div>
    <!-- Button ...ends-->  
                
<div class="clear"></div>

                <!-- Input Fields Area ...starts -->
                <div class="confg_Adm_box roundedcorners round_adm_box" id="tblSoapAPI" style="display:none"> 

                    <p class="confg_frm_txt_lgin" id="spanAPIURL"><!--API URL:--></p>
                    <p class="confg_frm_inp_bx_wide"><a id="aAPIURLValue" class="confg_frm_txt_lgin" target="_blank"></a></p>
                    <div class="clear"></div>

                    <div id="rowSoapMethod">
                        <p class="confg_frm_txt_lgin" id="spanSoapMethod"><!--SOAP Method:--></p>
                        <p class="confg_frm_inp_bx_wide confg_frm_txt_lgin" id="spanSoapMethodValue"></p>
                    </div>
                    <div class="clear"></div>

                </div>
                <!-- Input Fields Area ...ends -->

                <!-- Request Response Section ...starts-->
                <div id="tblRequestResponse" class="req_res_sec dtted_dvdr" style="display:none">
                    <!-- Request Section ...starts-->
                    <div class="req_lft_sec">
                        <h4 id="spanRequest"><!--Request--></h4>
                        <p><input type="hidden" id="hdnRequestContentType" value="" /><a id="aDownloadRequest" onclick="DownloadContent('textRequest');" class="cur_pointr"><!--Download--></a> | <a id="aCopyRequest" onclick="CopyContent('textRequest');" class="cur_pointr"><!--Copy--></a></p>
                        <div id="divRequest" class="req_res_box roundedcorners">
                            <pre id="textRequest" class="content req_res_pre_txt"></pre>
                        </div>
                    </div>
                    <!-- Request Section ...ends-->

                    <!-- Response Section ...starts-->
                    <div class="res_lft_sec">
                        <h4 id="spanResponse"><!--Response--></h4>
                        <p><input type="hidden" id="hdnResponseContentType" value="" /><a id="aDownloadResponse" class="cur_pointr" onclick="DownloadContent('textResponse');"><!--Download--></a> | <a id="aCopyResponse" class="cur_pointr" onclick="CopyContent('textResponse');"><!--Copy--></a></p>
                        <div id="divResponse" class="req_res_box roundedcorners">
                            <pre id="textResponse" class="content req_res_pre_txt">
                            </pre>
                        </div>
                    </div>
                    <!-- Response Section ...ends-->
                </div>
                <!-- Request Response Section ...ends-->

            </div>

        </div>

    </div>
    
<!-- ALL POPUPS DEFINED ...starts-->
                <%--<div id='AreaOuterBox' class="popup_block" style="width:700px; height:475px;">		
                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td class="popup_heading1" id="lang_SelectArea"><!--Select Area--></td>
                        </tr>			
                        <tr>
                            <td valign="top">
                                <table cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
                                    <tr>
                                        <td>
                                            <div id="area_div"></div>
                                        </td>
                                    </tr>
                                <tr>
                                    <td>
                                        <div class="di_gui_button_pos">
                                            <input type="button" name="areaOk" id="areaOk" value="" onclick="onClickOkPopUpButton('area')" class="di_gui_button" />
                                            <input type="button" name="areaCancel" id="areaCancel" value="" onclick="showHideComponent('close')" class="di_gui_button" style="margin-right:5px"/>
                                        </div>
                                    </td>
                                </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div> --%>

<!-- Popup for Select Area Component ...starts-->
<div id="AreaOuterBox" class="popup_block">
    <h1 class="popup_heading" id="lang_SelectArea"><!--Select Area--></h1>
    
    <!-- Popup Inside Content ...starts-->
    <div class="popup_brd">
    
        <div id="area_div"></div>
                   
    </div>
    <!-- Popup Inside Content ...ends-->    

        <!-- Popup Buttons ...starts-->
        <div class="di_gui_button_pos"><input type="button" name="areaOk" id="areaOk" value="" onclick="onClickOkPopUpButton('area')" class="di_gui_button" /><input type="button" name="areaCancel" id="areaCancel" value="" onclick="showHideComponent('close')" class="di_gui_button" /></div>   
        <!-- Popup Buttons ...starts--> 

</div>
<!-- Popup for Select Area Component ...ends-->

<%--                <div id='IndOuterBox' class="popup_block" style="width:700px; height:485px;">
                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td id="langSelectTopic" class="popup_heading1"><span id="lang_SelectIndicator"><!--Select Indicator--></span></td>
                        </tr>			
                        <tr>
                            <td valign="top">
                                <table cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
                                    <tr>
                                        <td>
                                            <div id="indicator_div"></div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="di_gui_button_pos">
                                                <input type="button" name="indOk" id="indOk" value="" onclick="onClickOkPopUpButton('ind')" class="di_gui_button" />
                                                <input type="button" name="indCancel" id="indCancel" value="" onclick="showHideComponent('close')" class="di_gui_button" style="margin-right:5px"/>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>--%>

<!-- Popup for Select Topic Component ...starts-->
<div id="IndOuterBox" class="popup_block">	
    <h1 class="popup_heading" id="lang_SelectIndicator"><!--Select Indicator--></h1>
    
    <!-- Popup Inside Content ...starts-->
    <div class="popup_brd">
    
        <div id="indicator_div"></div>
    
    </div>
    <!-- Popup Inside Content ...ends--> 
    
        <!-- Popup Buttons ...starts-->
        <div class="di_gui_button_pos"><input type="button" name="indOk" id="indOk" value="" onclick="onClickOkPopUpButton('ind')" class="di_gui_button" /><input type="button" name="indCancel" id="indCancel" value="" onclick="showHideComponent('close')" class="di_gui_button" /></div>        
        <!-- Popup Buttons ...ends-->            

</div>
<!-- Popup for Select Topic Component ...ends-->


<%--                <div id='TimeRangeOuterBox' class="popup_block" style="width:700px; height:475px;">		
                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td class="popup_heading1" id="lang_SelectTime"><!--Select Time--></td>
                        </tr>			
                        <tr>
                            <td width="100%" valign="top">
                                <table  width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
                                    <tr>
                                        <td>
                                            <div id="divTimeRange" style="height:420px; overflow:auto; border:1px solid #d3d3d3;"></div>
                                        </td>
                                    </tr>
                                <tr>
                                    <td>
                                        <div class="di_gui_button_pos">
                                            <input type="button" id="tpOK" value="" onclick="onClickOkPopUpButton('tp')" class="di_gui_button" />
                                            <input type="button" id="tpCancel" value="" onclick="showHideComponent('close'); " class="di_gui_button" style="margin-right:5px"/>
                                        </div>
                                    </td>
                                </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div> --%>

<!-- Popup for Select Time Component ...starts-->
<div id="TimeRangeOuterBox" class="popup_block">	
    <h1 class="popup_heading" id="lang_SelectTime"><!--Select Time--></h1>
    
    <!-- Popup Inside Content ...starts-->
    <div class="popup_brd">
    
        <div id="divTimeRange"></div>
    
    </div>
    <!-- Popup Inside Content ...ends--> 
    
        <!-- Popup Buttons ...starts-->
        <div class="di_gui_button_pos"><input type="button" id="tpOK" value="" onclick="onClickOkPopUpButton('tp')" class="di_gui_button" /><input type="button" id="tpCancel" value="" onclick="showHideComponent('close');" class="di_gui_button" /></div>        
        <!-- Popup Buttons ...ends-->            

</div>
<!-- Popup for Select Time Component ...ends-->

<!-- Popup for Select Source Component ...starts-->
<div id="SourceOuterBox" class="popup_block">	
    <h1 class="popup_heading" id="lang_SelectSource"><!--Select Source--></h1>
    
    <!-- Popup Inside Content ...starts-->
    <div class="popup_brd">
    
        <div id="divSource"></div>
    
    </div>
    <!-- Popup Inside Content ...ends--> 
    
        <!-- Popup Buttons ...starts-->
        <div class="di_gui_button_pos"><input type="button" id="sourceOK" value="" onclick="onClickOkPopUpButton('source')" class="di_gui_button" /><input type="button" id="sourceCancel" value="" onclick="showHideComponent('close');" class="di_gui_button" /></div>        
        <!-- Popup Buttons ...ends-->            

</div>
<!-- Popup for Select Source Component ...ends-->


<!-- ALL POPUPS DEFINED ...ends-->    
    <!-- END MARKUP -->

    <!-- START DEVELOPER CODE -->
    
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

        var di_components = "Area,Indicator,Language";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    </script>

    <script type="text/javascript">
     //   di_jq(document).ready(function () {
            onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');
       // });
    </script>
    
    <!-- END DEVELOPER CODE -->
    
</asp:Content>
