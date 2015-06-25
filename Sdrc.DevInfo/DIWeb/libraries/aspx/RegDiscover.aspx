<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master" AutoEventWireup="true" CodeFile="RegDiscover.aspx.cs" 
    Inherits="libraries_aspx_RegDiscover" Title="Discover Data/Metadata" EnableSessionState="false" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" Runat="Server">

    <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>

    <script type="text/javascript" src="../js/sdmx/Discover.js?v=<%=Global.js_version%>"></script>
    
    <div id="reg_wide_sec">

        <table id="tblDiscover" style="width:100%;">
            <tr>
                <td style="width:10%">
                    <span id="spanDiscover" class="confg_frm_txt_lgin">Discover:</span>
                </td>
                <td style="width:90%">
                    <input id="radioData" value="Data" name="Discover" type="radio" checked="checked" onclick="Reset(true);"/>Data
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="radioMetadata" value="Metadata" name="Discover" type="radio" onclick="Reset(false);"/>Metadata
                </td>
            </tr>
            <tr id="rowDFDMFDSeparator" style="line-height:15px;">
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr id="rowDFDMFD" style="line-height:15px;">
                <td>
                    <span id="spanDFDMFD" class="confg_frm_txt_lgin">Data/Metadata Flows:</span>
                </td>
                <td style="vertical-align:bottom">
                    <select id="selectDFDMFD" onclick="ShowHideRowsForSelection();"></select>
                </td>
            </tr>
            <tr id="rowIUSSeparator" style="line-height:15px;display:none;">
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr id="rowIUS" style="line-height:15px;display:none;">
                <td>
                    <span id="spanIUS" class="confg_frm_txt_lgin">Indicator-Unit-Subgroup:</span>
                </td>
                <td style="vertical-align:bottom">
                    <a id="aIUSSelect" style="cursor:pointer;" onclick="OpenIUSPopup();">Select</a>
                    <span> | </span>
                    <a id="aIUSClear" style="cursor:pointer;" onclick="ClearIUSSelections();">Clear</a>
                    &nbsp;&nbsp;
                    <i id="spanSelectedIUSes" class="content" style="color:#aaaaaa;"></i>
                    <input type="hidden" id="hdnSelectedIUSes" value="" />
                </td>
            </tr>
            <tr id="rowIndicatorSeparator" style="line-height:15px;display:none;">
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr id="rowIndicator" style="line-height:15px;display:none;">
                <td>
                    <span id="spanIndicator" class="confg_frm_txt_lgin">Indicator:</span>
                </td>
                <td style="vertical-align:bottom">
                    <a id="aIndicatorSelect" style="cursor:pointer;" onclick="OpenIndicatorPopup();">Select</a>
                    <span> | </span>
                    <a id="aIndicatorClear" style="cursor:pointer;" onclick="ClearIndicatorSelections();">Clear</a>
                    &nbsp;&nbsp;
                    <i id="spanSelectedIndicators" class="content" style="color:#aaaaaa;"></i>
                    <input type="hidden" id="hdnSelectedIndicators" value="" />
                </td>
            </tr>
            <tr id="rowAreaSeparator" style="line-height:15px;display:none;">
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr id="rowArea" style="line-height:15px;display:none;">
                <td>
                    <span id="spanArea" class="confg_frm_txt_lgin">Area:</span>
                </td>
                <td style="vertical-align:bottom">
                    <a id="aAreaSelect" style="cursor:pointer;" onclick="OpenAreaPopup();">Select</a>
                    <span> | </span>
                    <a id="aAreaClear" style="cursor:pointer;" onclick="ClearAreaSelections();">Clear</a>
                    &nbsp;&nbsp;
                    <i id="spanSelectedAreas" class="content" style="color:#aaaaaa;"></i>
                    <input type="hidden" id="hdnSelectedAreas" value="" />
                </td>
            </tr>
            <tr id="rowTPSeparator" style="line-height:15px;display:none;">
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr id="rowTP" style="line-height:15px;display:none;">
                <td>
                    <span id="spanTP" class="confg_frm_txt_lgin">Time:</span>
                </td>
                <td style="vertical-align:bottom">
                    <a id="aTPSelect" style="cursor:pointer;" onclick="OpenTPPopup();">Select</a>
                    <span> | </span>
                    <a id="aTPClear" style="cursor:pointer;" onclick="ClearTPSelections();">Clear</a>
                    &nbsp;&nbsp;
                    <i id="spanSelectedTPs" class="content" style="color:#aaaaaa;"></i>
                    <input type="hidden" id="hdnSelectedTPs" value="" />
                </td>
            </tr>
            <tr id="rowSourceSeparator" style="line-height:15px;display:none;">
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr id="rowSource" style="line-height:15px;display:none;">
                <td>
                    <span id="spanSource" class="confg_frm_txt_lgin">Source:</span>
                </td>
                <td style="vertical-align:bottom">
                    <a id="aSourceSelect" style="cursor:pointer;" onclick="OpenSourcePopup();">Select</a>
                    <span> | </span>
                    <a id="aSourceClear" style="cursor:pointer;" onclick="ClearSourceSelections();">Clear</a>
                    &nbsp;&nbsp;
                    <i id="spanSelectedSources" class="content" style="color:#aaaaaa;"></i>
                    <input type="hidden" id="hdnSelectedSources" value="" />
                </td>
            </tr>
            <tr id="rowDiscoverSeparator" style="line-height:15px;display:none;">
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr id="rowDiscover" style="line-height:15px;display:none;">
                <td colspan="2">
                    <input id="btnDiscover" type="button" value="Discover" onclick="DiscoverDataMetadata();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
                </td>
            </tr>
        </table>

        <div id='IUSPopup' class="popup_block" style="width:700px; height:475px;">		
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="popup_heading1" id="lang_SelectIUS">Select Indicator-Unit-Subgroup</td>
                </tr>			
                <tr>
                    <td width="100%" valign="top">
                        <table  width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
                            <tr>
                                <td>
                                    <div id="divIUS" style="height:420px; overflow:auto; border:1px solid #d3d3d3;"></div>
                                </td>
                            </tr>
                        <tr>
                            <td>
                                <div class="di_gui_button_pos">
                                    <input type="button" id="btnIUSOK" value="OK" onclick="IUSPopupOk();" class="di_gui_button" />
                                    <input type="button" id="btnIUSCancel" value="Cancel" onclick="IUSPopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
                                </div>
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>

        <div id='IndicatorPopup' class="popup_block" style="width:700px; height:475px;">		
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="popup_heading1" id="lang_SelectIndicator">Select Indicator</td>
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
                                    <input type="button" id="btnIndicatorOK" value="OK" onclick="IndicatorPopupOk();" class="di_gui_button" />
                                    <input type="button" id="btnIndicatorCancel" value="Cancel" onclick="IndicatorPopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
                                </div>
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>

        <div id='AreaPopup' class="popup_block" style="width:700px; height:475px;">		
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="popup_heading1" id="lang_SelectArea">Select Area</td>
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
                                    <input type="button" id="btnAreaOK" value="OK" onclick="AreaPopupOk();" class="di_gui_button" />
                                    <input type="button" id="btnAreaCancel" value="Cancel" onclick="AreaPopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
                                </div>
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>

        <div id='TPPopup' class="popup_block" style="width:700px; height:475px;">		
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="popup_heading1" id="lang_SelectTime">Select Time</td>
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
                                    <input type="button" id="btnTPOK" value="OK" onclick="TPPopupOk();" class="di_gui_button" />
                                    <input type="button" id="btnTPCancel" value="Cancel" onclick="TPPopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
                                </div>
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>

        <div id='SourcePopup' class="popup_block" style="width:700px; height:475px;">		
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="popup_heading1" id="lang_SelectSource">Select Source</td>
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
                                    <input type="button" id="btnSourceOK" value="OK" onclick="SourcePopupOk();" class="di_gui_button" />
                                    <input type="button" id="btnSourceCancel" value="Cancel" onclick="SourcePopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
                                </div>
                            </td>
                        </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>

    </div>
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

    <script type="text/javascript">onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=hOriginaldbnid %>');
    </script>

    <!-- END OF DEVELOPER CODE -->
</asp:Content>


