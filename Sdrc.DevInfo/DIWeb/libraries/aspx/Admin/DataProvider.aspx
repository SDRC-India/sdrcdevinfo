<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/Admin/AdminMaster.master" AutoEventWireup="true" CodeFile="DataProvider.aspx.cs" Inherits="libraries_aspx_Admin_DataProvider" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
<script type="text/javascript" src="../../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
<script type="text/javascript" src="../../js/DataProvider.js?v=<%=Global.js_version%>"></script>

<div id="reg_lft_sec1">
    <h1>Generation and Registration of SDMX-ML and Metadata Reports</h1> 
    <h4>Generate and register SDMX-ML and Metadata Reports from databases or in case of uploaded DSDs from associated databases.</h4> 

    <table style="width:100%;">
        <tr>
            <td style="width:10%">
                <span id="spanDatabase" class="confg_frm_txt_lgin">Database:</span>
            </td>
            <td style="width:90%">
                <a id="aDatabaseSelect" style="cursor:pointer;" onclick="OpenDatabasePopup();">Select</a>
                <span> | </span>
                <a id="aDatabaseClear" style="cursor:pointer;" onclick="ClearDatabaseSelections();">Clear</a>
                &nbsp;&nbsp;
                <i id="spanSelectedDatabase" class="content" style="color:#aaaaaa;"></i>
                <input type="hidden" id="hdnSelectedDatabase" value="" />
            </td>
        </tr>
        <tr style="line-height:0px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <span id="spanOR" class="confg_frm_txt_lgin" style="font-weight:bold;">OR</span>
            </td>
        </tr>
        <tr style="line-height:0px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <span id="spanDSD" class="confg_frm_txt_lgin">DSD[Id(Version, AgencyId)]:</span>
            </td>
            <td>
                <a id="aDSDSelect" style="cursor:pointer;" onclick="OpenDSDPopup();">Select</a>
                <span> | </span>
                <a id="aDSDClear" style="cursor:pointer;" onclick="ClearDSDSelections();">Clear</a>
                &nbsp;&nbsp;
                <i id="spanSelectedDSD" class="content" style="color:#aaaaaa;"></i>
                <input type="hidden" id="hdnSelectedDSD" value="" />
            </td>
        </tr>
        <tr style="line-height:0px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>
                <span id="spanAction" class="confg_frm_txt_lgin">Action:</span>
            </td>
            <td>
                <select id="selectAction" class="di_gui_dropdown1" onchange="SelectActionChange();">
                    <option value="0" selected="selected" id="SelectOne">--Select One--</option>
                    <option value="1" id="GenerateSDMXML">Generate SDMX-ML</option>
                    <option value="2" id="RegisterSDMXML">Register SDMX-ML</option>
                    <option value="3" id="GenerateMetadata">Generate Metadata</option>
                    <option value="4" id="RegisterMetadata">Register Metadata</option>
                </select>
                &nbsp;&nbsp;
                <span id="spanGenerateSDMXMLDescription" class="content" style="color:#aaaaaa; display:none;">Generate SDMX-ML files from databases or in case of uploaded DSDs from associated databases.</span>
                <span id="spanRegisterSDMXMLDescription" class="content" style="color:#aaaaaa; display:none;">Register already generated SDMX-ML files.</span>
                <span id="spanGenerateMetadataDescription" class="content" style="color:#aaaaaa; display:none;">Generate Metadata Reports from databases or in case of uploaded DSDs from associated databases.</span>
                <span id="spanRegisterMetadataDescription" class="content" style="color:#aaaaaa; display:none;">Register already generated Metadata Reports.</span>
            </td>
        </tr>
    </table>

    <table id="tblGenerateSDMXML" style="width:100%; display:none;">
        <tr id="rowIndicatorSelection">
            <td style="width:10%">
                <span id="spanIndicator" class="confg_frm_txt_lgin">Indicator:</span>
            </td>
            <td style="width:90%">
                <a id="aIndicatorSelect" style="cursor:pointer;" onclick="OpenIndicatorPopup();">Select</a>
                <span> | </span>
                <a id="aIndicatorClear" style="cursor:pointer;" onclick="ClearIndicatorSelections();">Clear</a>
                &nbsp;&nbsp;
                <i id="spanSelectedIndicators" class="content" style="color:#aaaaaa;"></i>
                <input type="hidden" id="hdnSelectedIndicators" value="" />
            </td>
        </tr>
        <tr style="line-height:0px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr id="rowAreaSelection">
            <td>
                <span id="spanArea" class="confg_frm_txt_lgin">Area:</span>
            </td>
            <td>
                <a id="aAreaSelect" style="cursor:pointer;" onclick="OpenAreaPopup();">Select</a>
                <span> | </span>
                <a id="aAreaClear" style="cursor:pointer;" onclick="ClearAreaSelections();">Clear</a>
                &nbsp;&nbsp;
                <i id="spanSelectedAreas" class="content" style="color:#aaaaaa;"></i>
                <input type="hidden" id="hdnSelectedAreas" value="" />
            </td>
        </tr>
        <tr style="line-height:0px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr id="rowTPSelection">
            <td>
                <span id="spanTP" class="confg_frm_txt_lgin">Time:</span>
            </td>
            <td>
                <a id="aTPSelect" style="cursor:pointer;" onclick="OpenTPPopup();">Select</a>
                <span> | </span>
                <a id="aTPClear" style="cursor:pointer;" onclick="ClearTPSelections();">Clear</a>
                &nbsp;&nbsp;
                <i id="spanSelectedTPs" class="content" style="color:#aaaaaa;"></i>
                <input type="hidden" id="hdnSelectedTPs" value="" />
            </td>
        </tr>
        <tr style="line-height:0px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr id="rowSourceSelection">
            <td>
                <span id="spanSource" class="confg_frm_txt_lgin">Source:</span>
            </td>
            <td>
                <a id="aSourceSelect" style="cursor:pointer;" onclick="OpenSourcePopup();">Select</a>
                <span> | </span>
                <a id="aSourceClear" style="cursor:pointer;" onclick="ClearSourceSelections();">Clear</a>
                &nbsp;&nbsp;
                <i id="spanSelectedSources" class="content" style="color:#aaaaaa;"></i>
                <input type="hidden" id="hdnSelectedSources" value="" />
            </td>
        </tr>
        <tr style="line-height:15px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <input id="btnGenerateSDMXML" type="button" value="Generate SDMX-ML" onclick="GenerateSDMXML();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
        </tr>
    </table>

    <table id="tblRegisterSDMXML" style="width:100%; display:none;">
        <tr style="line-height:15px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <input id="btnRegisterSDMXML" type="button" value="Register SDMX-ML" onclick="RegisterSDMXML();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
        </tr>
    </table>

    <table id="tblGenerateMetadata" style="width:100%; display:none;">
        <tr style="line-height:15px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <input id="btnGenerateMetadata" type="button" value="Generate Metadata" onclick="GenerateMetadata();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
        </tr>
    </table>

    <table id="tblRegisterMetadata" style="width:100%; display:none;">
        <tr style="line-height:15px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <input id="btnRegisterMetadata" type="button" value="Register Metadata" onclick="RegisterMetadata();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
        </tr>
    </table>

    <div id='DatabasePopup' class="popup_block" style="width:700px; height:475px;">		
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td class="popup_heading1" id="lang_SelectDatabase">Select Database</td>
            </tr>			
            <tr>
                <td width="100%" valign="top">
                    <table  width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
                        <tr>
                            <td>
                                <div id="divDatabase" style="height:420px; overflow:auto; border:1px solid #d3d3d3;"></div>
                            </td>
                        </tr>
                    <tr>
                        <td>
                            <div class="di_gui_button_pos">
                                <input type="button" id="btnDatabaseOK" value="OK" onclick="DatabasePopupOk();" class="di_gui_button" />
                                <input type="button" id="btnDatabaseCancel" value="Cancel" onclick="DatabasePopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
                            </div>
                        </td>
                    </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>

    <div id='DSDPopup' class="popup_block" style="width:700px; height:475px;">		
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td class="popup_heading1" id="lang_SelectDSD">Select DSD</td>
            </tr>			
            <tr>
                <td width="100%" valign="top">
                    <table  width="100%" cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
                        <tr>
                            <td>
                                <div id="divDSD" style="height:420px; overflow:auto; border:1px solid #d3d3d3;"></div>
                            </td>
                        </tr>
                    <tr>
                        <td>
                            <div class="di_gui_button_pos">
                                <input type="button" id="btnDSDOK" value="OK" onclick="DSDPopupOk();" class="di_gui_button" />
                                <input type="button" id="btnDSDCancel" value="Cancel" onclick="DSDPopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
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

<script type="text/javascript">
    onPageLoad();
</script>

</asp:Content>
