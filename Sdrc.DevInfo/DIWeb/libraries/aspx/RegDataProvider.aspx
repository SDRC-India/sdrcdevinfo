<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master" AutoEventWireup="true" Async="true" CodeFile="RegDataProvider.aspx.cs" Inherits="libraries_aspx_RegDataProvider" Title="Data Provider" EnableSessionState="true" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" Runat="Server">
<script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>
<script type="text/javascript" src="../js/sdmx/DataProvider.js?v=<%=Global.js_version%>"></script>
    
<div id="reg_wide_sec">

    <table style="width:100%;">
        <tr id="segmentDatabase">
            <td style="width:10%">
                <span id="spanDatabase" class="confg_frm_txt_lgin"><%--Database:--%></span>
            </td>
            <td style="width:90%">
                <a id="aDatabaseSelect" style="cursor:pointer;" onclick="OpenDatabasePopup();"><%--Select--%></a>
                <span> | </span>
                <a id="aDatabaseClear" style="cursor:pointer;" onclick="ClearDatabaseSelections();"><%--Clear--%></a>
                &nbsp;&nbsp;
                <i id="spanSelectedDatabase" class="content" style="color:#aaaaaa;"></i>
                <input type="hidden" id="hdnSelectedDatabase" value="" />
            </td>
        </tr>
        <tr style="line-height:0px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr id="segmentOR">
            <td colspan="2" align="center">
                <span id="spanOR" class="confg_frm_txt_lgin" style="font-weight:bold;"><%--OR--%></span>
            </td>
        </tr>
        <tr style="line-height:0px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr id="segmentDSD">
            <td style="width:10%">
                <span id="spanDSD" class="confg_frm_txt_lgin"><%--DSD[Id(Version, AgencyId)]:--%></span>
            </td>
            <td style="width:90%">
                <a id="aDSDSelect" style="cursor:pointer;" onclick="OpenDSDPopup();"><%--Select--%></a>
                <span> | </span>
                <a id="aDSDClear" style="cursor:pointer;" onclick="ClearDSDSelections();"><%--Clear--%></a>
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
                <span id="spanAction" class="confg_frm_txt_lgin"><%--Action:--%></span>
            </td>
            <td>
                <select id="selectAction" class="di_gui_dropdown1" onchange="SelectActionChange();">
                    <option value="0" selected="selected" id="SelectOne"><%----Select One----%></option>
                    <option value="1" id="GenerateSDMXML"><%--Generate SDMX-ML--%></option>
              <%--      <option value="2" id="RegisterSDMXML">Register SDMX-ML</option>--%>
                    <option value="3" id="GenerateMetadata"><%--Generate Metadata--%></option>
                  <%--  <option value="4" id="RegisterMetadata">--Register Metadata--</option>--%>
                </select>
                &nbsp;&nbsp;
                <span id="spanGenerateSDMXMLDescription" class="content" style="color:#aaaaaa; display:none;"><%--Generate SDMX-ML files from databases or in case of uploaded DSDs from associated databases.--%></span>
                <span id="spanRegisterSDMXMLDescription" class="content" style="color:#aaaaaa; display:none;"><%--Register already generated SDMX-ML files.--%></span>
                <span id="spanGenerateMetadataDescription" class="content" style="color:#aaaaaa; display:none;"><%--Generate Metadata Reports from databases or in case of uploaded DSDs from associated databases.--%></span>
                <span id="spanRegisterMetadataDescription" class="content" style="color:#aaaaaa; display:none;"><%--Register already generated Metadata Reports.--%></span>
            </td>
        </tr>
    </table>

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
                <input type="hidden" id="hdnSelectedIndicators" value="" />
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
                <a id="aAreaSelect" style="cursor:pointer;" onclick="OpenAreaPopup();"><%--Select--%></a>
                <span> | </span>
                <a id="aAreaClear" style="cursor:pointer;" onclick="ClearAreaSelections();"><%--Clear--%></a>
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
                <span id="spanTP" class="confg_frm_txt_lgin"><%--Time:--%></span>
            </td>
            <td>
                <a id="aTPSelect" style="cursor:pointer;" onclick="OpenTPPopup();"><%--Select--%></a>
                <span> | </span>
                <a id="aTPClear" style="cursor:pointer;" onclick="ClearTPSelections();"><%--Clear--%></a>
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
                <span id="spanSource" class="confg_frm_txt_lgin"><%--Source:--%></span>
            </td>
            <td>
                <a id="aSourceSelect" style="cursor:pointer;" onclick="OpenSourcePopup();"><%--Select--%></a>
                <span> | </span>
                <a id="aSourceClear" style="cursor:pointer;" onclick="ClearSourceSelections();"><%--Clear--%></a>
                &nbsp;&nbsp;
                <i id="spanSelectedSources" class="content" style="color:#aaaaaa;"></i>
                <input type="hidden" id="hdnSelectedSources" value="" />
            </td>
        </tr>
        <tr style="line-height:15px;">
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr id="rowGenerateSDMXML">
            <td colspan="2">
                <input id="btnGenerateSDMXML" type="button" onclick="GenerateSDMXML();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
        </tr>
        <tr id="rowRegisterSDMXML">
            <td colspan="2">
                <input id="btnRegisterSDMXML" type="button" onclick="RegisterSDMXML();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
        </tr>
        <tr id="rowGenerateMetadata">
            <td colspan="2">
                <input id="btnGenerateMetadata" type="button" onclick="GenerateMetadata();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
        </tr>
        <tr id="rowRegisterMetadata">
            <td colspan="2">
                <input id="btnRegisterMetadata" type="button" onclick="RegisterMetadata();" class="di_gui_button lng_bttn_sz" style="margin-left:0px;"/>
            </td>
        </tr>
    </table>

    <div id='DatabasePopup' class="popup_block" style="width:700px; height:475px;">		
        <table width="100%" cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td class="popup_heading1" id="lang_SelectDatabase"><%--Select Database--%></td>
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
                                <input type="button" id="btnDatabaseOK"  onclick="DatabasePopupOk();" class="di_gui_button" />
                                <input type="button" id="btnDatabaseCancel" onclick="DatabasePopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
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
                <td class="popup_heading1" id="lang_SelectDSD"><%--Select DSD--%></td>
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
                                <input type="button" id="btnDSDOK" onclick="DSDPopupOk();" class="di_gui_button" />
                                <input type="button" id="btnDSDCancel" onclick="DSDPopupCancel(); " class="di_gui_button" style="margin-right:5px"/>
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
    <input type="hidden" id="hSelectAll" />
    <input type="hidden" id="hDatabase" />
    <input type="hidden" id="hPublisher" />
    <input type="hidden" id="hId" />
    <input type="hidden" id="hAgencyId" />
    <input type="hidden" id="hVersion" />
    <input type="hidden" id="hAssosciatedDatabase" />
    <input type="hidden" id="hdnMappingNotFound" />
    <input type="hidden" id="hdnNoDataFound" />
    <input type="hidden" id="hdnDSDORDB" />
    <input type="hidden" id="hdnDBAreaId" />
    
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

<script type="text/javascript">    onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=hOriginaldbnid %>');
</script>

<!-- END OF DEVELOPER CODE -->
</asp:Content>


