<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/RegistryMaster.master"
    AutoEventWireup="true" CodeFile="RegMapping.aspx.cs" Inherits="libraries_aspx_RegMapping" %>

<asp:Content ID="Content2" ContentPlaceHolderID="cphRegistryMainContent" runat="Server">
    <%--  <script type="text/javascript" src="../js/di.jquery-1.4.4.js?v=<%=Global.js_version%>"></script>--%>
    <%--    <script type="text/javascript" src="../js/jquery-1.6.2.js?v=<%=Global.js_version%>"></script>--%>
    <script type="text/javascript" src="../js/jquery-1.8.2.min.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript">
        var d = jQuery.noConflict();



    </script>
    <script type="text/javascript" src="../js/sdmx/Mapping.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/chosen.jquery.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/customSelect.jquery.js?v=<%=Global.js_version%>"></script>
    <div id="DivMappingOptions" class="MappingOptions">
        <div id="DivMappingTypeSelection">
            <ul class="maping_type_nav" id="UlMappingTypeSelectionOptions">
                <li id="liMappingTypeCodeList"><a href="javascript:void(0);" id="aCodelist" onclick="ShowHideMappings('divCodelistMapping');"
                    style="width: 170px;" class="adm_lft_nav_seld">
                    <!--Codelist-->
                </a><span id="SpnCodeListPipe">|</span></li>
                <li id="liMappingTypeIUS"><a href="javascript:void(0);" id="aIUS" onclick="ShowHideMappings('divIUSMappingOuter');"
                    style="width: 170px;" class="adm_lft_nav_seld">
                    <!--IUS-->
                </a><span id="SpnIUSPipe">|</span></li>
                <li id="liMappingTypeMetaData"><a href="javascript:void(0);" id="aMetadata" onclick="ShowHideMappings('divMetadataMappingOuter');"
                    style="width: 170px;" class="adm_lft_nav">
                    <!--Metadata-->
                </a></li>
            </ul>
        </div>
        <div id="DivImportExportOptions" class="flt_rgt">
            <ul class="import_export_nav" id="ImportExportOptions">
                <li id="liImport">
                    <img id="imgExportExcel" class="cur_pointr" onclick="javascript:ExportMappingExcel(0);"
                        border="0" src="../../stock/themes/default/images/export_excel.png" alt="Export Excel">
                    <a id="lnkExportMapExcel" href="javascript:ExportMappingExcel(0)">
                        <!--Export-->
                    </a></li>
                <li id="liExport">
                    <!-- Export Import Buttons ...starts-->
                    <form id="frmImportMapExcel" name="frmImportMapExcel" runat="server" enctype="multipart/form-data"
                    method="post">
                    <asp:FileUpload ID="FileUpdXLS" runat="server" Style="display: none;" />
                    <img id="imgImportMapExcel" class="cur_pointr" onclick="javascript:OpenUploadExcellPopup(0);"
                        border="0" alt="Import Excell" src="../../stock/themes/default/images/import_excel.png">
                    <a id="lnkImportMapExcel" href="javascript:OpenUploadExcellPopup(0)">
                        <!--Import-->
                    </a>
                    </form>
                    <!-- Export Import Buttons ...ends-->
                </li>
            </ul>
        </div>
    </div>
    <!-- Reg Mapping Section ...starts-->
    <div id="divMapping_adm" class="mappingSection">
        <div id="divCodelistMapping" style="display: none">
            <div id="divCodelistMappingHeading">
                <div class="mappingHeading" id="divCodelistsHeadinh">
                    <h3 id="lang_Mapping_Of_DevInfo_And_UNSD_Codelists">
                        <%--Mapping of DevInfo against UNSD Codelists--%>
                    </h3>
                </div>
                <div id="divCodelistHelpImg" style="float: left; position: relative; top: 1px;">
                    <img onmouseout="HideCallout('divCallout')" style="cursor: pointer; margin-top: 10px"
                        onclick="ToggleCallout('divCallout', event, 'divHelpCodelistMapping')" alt="Help"
                        src="../../stock/themes/default/images/help.gif" id="img1"></div>
            </div>
            <br />
            <div style="width: 100%; float: left;">
                <img id="imgdivIndicatorMapping" src="../../stock/themes/default/images/expand.png"
                    alt="Expand and Collapse" onclick="BindIndicatorCodelist('true');" style="margin-right: 4px;
                    cursor: pointer;" class="flt_lft" />
                <h3 id="lang_IndicatorMapping">
                    <%--Indicator Mapping :--%></h3>
                </br>
            </div>
            <br />
            <div id="divIndicatorMapping" class="roundedcorners" style="width: 100%; float: left;
                padding-top: 31px; background-color: #ffffdd; display: none;">
            </div>
            <div class="clear">
            </div>
            <br />
            <div>
                <%-- <img id="imgdivUnitMapping" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse"
                    onclick="ExpandCollapseList('imgdivUnitMapping','divUnitMapping');" style="margin-right: 4px;
                    cursor: pointer;" class="flt_lft" />--%>
                <img id="imgdivUnitMapping" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse"
                    onclick="BindUnitCodelist('true');" style="margin-right: 4px; cursor: pointer;"
                    class="flt_lft" />
                <h3 id="lang_UnitMapping">
                    <%--Unit Mapping :--%></h3>
            </div>
            <br />
            <div id="divUnitMapping" class="roundedcorners" style="width: 100%; float: left;
                background-color: #ffffdd; display: none">
            </div>
            <div class="clear">
            </div>
            <br />
            <div>
                <img id="imgdivAgeMapping" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse"
                    onclick="ExpandCollapseList('imgdivAgeMapping','divAgeMapping');" style="margin-right: 4px;
                    cursor: pointer;" class="flt_lft" />
                <h3 id="lang_AgeMapping">
                    <%--Age Mapping :--%>
                </h3>
                <div class="flt_lft">
                    <span id="spanAgeSelectedCodelist" class="confg_frm_txt_lgin">
                        <%--AgeCodelist[Id(Version, AgencyId)]:--%></span>
                </div>
                <div class="flt_lft codelistSelect">
                    <a id="aAgeCodelistSelect" style="cursor: pointer;" onclick="OpenCodelistPopup(this);">
                        <%--Select--%></a> <span>| </span><a id="aAgeCodelistClear" style="cursor: pointer;"
                            onclick="ClearCodelistSelections(this);">
                            <%--Clear--%></a> &nbsp;&nbsp; <i id="spanAgeCodelist" class="content" style="color: #aaaaaa;">
                            </i>
                    <input type="hidden" id="hdnSelectedAgeCodelist" value="" />
                </div>
            </div>
            <br />
            <div id="divAgeMapping" class="roundedcorners" style="width: 100%; float: left; background-color: #ffffdd;
                display: none">
            </div>
            <div class="clear">
            </div>
            <br />
            <div>
                <img id="imgdivSexMapping" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse"
                    onclick="ExpandCollapseList('imgdivSexMapping','divSexMapping');" style="margin-right: 4px;
                    cursor: pointer;" class="flt_lft" />
                <h3 id="lang_SexMapping">
                    <%--Sex Mapping :--%></h3>
                <div class="flt_lft">
                    <span id="spanSelectedSexCodelist" class="confg_frm_txt_lgin">
                        <%--SexCodelist[Id(Version, AgencyId)]:--%></span>
                </div>
                <div class="flt_lft codelistSelect">
                    <a id="aSexCodelistSelect" style="cursor: pointer;" onclick="OpenCodelistPopup(this);">
                        <%--Select--%></a> <span>| </span><a id="aSexCodelistClear" style="cursor: pointer;"
                            onclick="ClearCodelistSelections(this);">
                            <%--Clear--%></a> &nbsp;&nbsp; <i id="spanSexCodelist" class="content" style="color: #aaaaaa;">
                            </i>
                    <input type="hidden" id="hdnSelectedSexCodelist" value="" />
                </div>
            </div>
            <br />
            <div id="divSexMapping" class="roundedcorners" style="width: 100%; float: left; background-color: #ffffdd;
                display: none">
            </div>
            <div class="clear">
            </div>
            <br />
            <div>
                <img id="imgdivLocationMapping" src="../../stock/themes/default/images/expand.png"
                    alt="Expand and Collapse" onclick="ExpandCollapseList('imgdivLocationMapping','divLocationMapping');"
                    style="margin-right: 4px; cursor: pointer;" class="flt_lft" />
                <h3 id="lang_LocationMapping">
                    <%--Location Mapping :--%></h3>
                <div class="flt_lft">
                    <span id="spanSelectedLocationCodelist" class="confg_frm_txt_lgin">
                        <%--LocationCodelist[Id(Version, AgencyId)]:--%></span>
                </div>
                <div class="flt_lft codelistSelect">
                    <a id="aLocationCodelistSelect" style="cursor: pointer;" onclick="OpenCodelistPopup(this);">
                        <%--Select--%></a> <span>| </span><a id="aLocationCodelistClear" style="cursor: pointer;"
                            onclick="ClearCodelistSelections(this);">
                            <%--Clear--%></a> &nbsp;&nbsp; <i id="spanLocationCodelist" class="content" style="color: #aaaaaa;">
                            </i>
                    <input type="hidden" id="hdnSelectedLocationCodelist" value="" />
                </div>
            </div>
            <div class="clear">
            </div>
            <br />
            <div id="divLocationMapping" class="roundedcorners" style="width: 100%; float: left;
                background-color: #ffffdd; display: none">
            </div>
            <%--  ----------------------------------AREA MAPPING-----------------------------------%>
            <div class="clear">
            </div>
            <br />
            <div>
                <%--  <img id="imgdivAreaMapping" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse"
                    onclick="ExpandCollapseList('imgdivAreaMapping','divAreaMapping');" style="margin-right: 4px;
                    cursor: pointer;" class="flt_lft" />--%>
                <img id="imgdivAreaMapping" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse"
                    onclick="BindAreaCodelist('true');" style="margin-right: 4px; cursor: pointer;"
                    class="flt_lft" />
                <h3 id="lang_AreaMapping">
                    <%--Area Mapping :--%></h3>
            </div>
            <div class="ddlAreaLevel" style="display:none">
            <select id="selectAreaLevel" onchange="BindAreaCodelist('true')"></select>
            </div>
            <br />
            <div id="divAreaMapping" class="roundedcorners" style="width: 100%; float: left;
                background-color: #ffffdd; display: none">
            </div>
            <div class="clear">
            </div>
            <br />
            <%-- -------------------------save-------------------------%>
            <div class="adm_upd_bttn_lgin_ppup" style="margin-left: 0px; float: left">
                <input type="button" id="btnGenerateCodelistsMappingFile" value="" onclick="GenerateCodelistsMappingFile();"
                    class="di_gui_button" />
            </div>
        </div>
        <div id="divIUSMappingOuter" style="display: none;">
            <div style="padding-bottom: 6%;" id="divIUSMappingHeading">
                <div style="float: left;" id="divIUSMapp">
                    <h3 style="padding: 8px 0px 1px; color: rgb(64, 65, 65); font-size: 21px; width: 100%;"
                        id="lang_Mapping_Of_DevInfo_And_UNSD_IUS">
                        <%--Mapping of DevInfo I-U-S against UNSD Codelists--%>
                    </h3>
                </div>
                <div id="divIUSHelpImg" style="float: left; position: relative; top: 1px;">
                    <img onmouseout="HideCallout('divCallout')" style="cursor: pointer; margin-top: 10px"
                        onclick="ToggleCallout('divCallout', event, 'divHelpIUSMapping')" alt="Help"
                        src="../../stock/themes/default/images/help.gif" id="img2"></div>
            </div>
            <div>
                <h3 id="lang_IUS_Mapping">
                    <%--IUS Mapping :--%></h3>
            </div>
            <br />
            <div id="divIUSMapping" class="roundedcorners" style="width: 100%; float: left; background-color: #ffffdd;">
            </div>
            <div class="clear">
            </div>
            <div class="adm_upd_bttn_lgin_ppup" style="margin-left: 0px; float: left">
                <input type="button" id="btnGenerateIUSMappingFile" value="" onclick="GenerateIUSMappingFile();"
                    class="di_gui_button" />
            </div>
        </div>
        <div id="divMetadataMappingOuter" style="display: none">
            <div style="padding-bottom: 6%;" id="divMetadataMappingHeading">
                <div style="float: left;" id="divMetadataHeadinh">
                    <h3 style="padding: 8px 0px 1px; color: rgb(64, 65, 65); font-size: 21px; width: 100%;"
                        id="lang_Mapping_Of_DevInfo_And_UNSD_Metadata">
                        <%--Mapping of DevInfo against UNSD Metadata Concepts--%>
                    </h3>
                </div>
                <div id="divMetadataHelpImg" style="float: left; position: relative; top: 1px;">
                    <img onmouseout="HideCallout('divCallout')" style="cursor: pointer; margin-top: 10px"
                        onclick="ToggleCallout('divCallout', event, 'divHelpMetadataMapping')" alt="Help"
                        src="../../stock/themes/default/images/help.gif" id="img3"></div>
            </div>
            <br />
            <div>
                <%--<img id="imgdivMetadataMapping" src="../../stock/themes/default/images/expand.png" alt="Expand and Collapse" onclick="ExpandCollapseList('imgdivMetadataMapping','divMetadataMapping')"  style="margin-right: 4px; cursor:pointer;" class="flt_lft"/>--%>
                <h3 id="lang_Metadata_Mapping">
                    <%--Metadata Mapping :--%></h3>
            </div>
            <br />
            <div id="divMetadataMapping" class="roundedcorners" style="width: 100%; float: left;
                background-color: #ffffdd;">
            </div>
            <div class="clear">
            </div>
            <div class="adm_upd_bttn_lgin_ppup" style="margin-left: 0px; float: left">
                <input type="button" id="btnGenerateMetadataMappingFile" value="" onclick="GenerateMetadataMappingFile();"
                    class="di_gui_button" />
            </div>
        </div>
        <div id='CodelistPopup' class="popup_block" style="width: 700px; height: 475px;">
            <table width="100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td class="popup_heading1" id="lang_SelectCodelist">
                        <%--Select Codelist--%>
                    </td>
                </tr>
                <tr>
                    <td width="100%" valign="top">
                        <table width="100%" cellpadding="0" cellspacing="0" style="border: 1px solid #d3d3d3;">
                            <tr>
                                <td>
                                    <div id="divCodelist" style="height: 420px; overflow: auto; border: 1px solid #d3d3d3;">
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div class="di_gui_button_pos">
                                        <input type="button" id="btnCodelistOK" onclick="CodelistPopupOk();" class="di_gui_button" />
                                        <input type="button" id="btnCodelistCancel" onclick="CodelistPopupCancel(); " class="di_gui_button"
                                            style="margin-right: 5px" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divCallout" class="callout border-callout">
        <div id="divCalloutText" class="content">
            <div id="divHelpCodelistMapping" style="display: none">
                <%--                <span class="heading2" id="span_HelpCodelist_hdn"></span>
                <br />--%>
                <i class="content" id="lang_Mapping_Of_DevInfo_And_UNSD_Codelists_Desc"></i>
            </div>
            <div id="divHelpIUSMapping" style="display: none">
                <%--                <span class="heading2" id="span_HelpIUS_hdn"></span>
                <br />--%>
                <i class="content" id="lang_Mapping_Of_DevInfo_And_UNSD_IUS_Desc"></i>
            </div>
            <div id="divHelpMetadataMapping" style="display: none">
                <%--                <span class="heading2" id="span_HelpMetadata_hdn"></span>
                <br />--%>
                <i class="content" id="lang_Mapping_Of_DevInfo_And_UNSD_Metadata_Desc">
                    <%--Mapping of DevInfo Metadata Concept Schemes' concepts against UNSD Metadata Concept Schemes' concepts. This mapping is used to generate Metadata Reports from DevInfo database that are compliant against UNSD's MSD.--%></i>
            </div>
        </div>
        <b class="border-notch notch"></b><b class="notch"></b>
    </div>
    <div id="divImportExcel" class="popup_block" style="height: 175px; width: 47%;">
        <div class="confg_Adm_box">
            <div id="divUploadExcel">
                <form id="frmUploadExcel" name="frmUploadExcel" action="UploadFile.aspx" enctype="multipart/form-data"
                method="post">
                <h3 id="lang_Upload_Excel_file">
                    Upload
                    <%--Upload Excell File--%></h3>
                <br />
                <p class="confg_frm_txt">
                    <input id="ImportMappingExcel" name="ImportMappingExcel" type="file" class="confg_frm_inp_bx_txt_reg di_gui_button"
                        size="75" style="width: 500px" /><br />
                    <br />
                    <br />
                    <input type="submit" name="btnUploadExcelForImport" id="btnUploadExcelForImport"
                        value="" class="di_gui_button" />
                </p>
                </form>
            </div>
        </div>
    </div>
    <!-- Popup For Help starts -->
    <input type="hidden" id="hShowAll" />
    <input type="hidden" id="hMapped" />
    <input type="hidden" id="hUnMapped" />
    <input type="hidden" id="hUnSaved" />
    <input type="hidden" id="hDevInfoIndicator" />
    <input type="hidden" id="hIndicatorGIds" />
    <input type="hidden" id="hUNSDIndicator" />
    <input type="hidden" id="hIndicatorIds" />
    <input type="hidden" id="hSearchIndicator" />
    <input type="hidden" id="hFilterIndicatorLists" />
    <input type="hidden" id="hDevInfoUnit" />
    <input type="hidden" id="hUnitGIds" />
    <input type="hidden" id="hUNSDUnit" />
    <input type="hidden" id="hUnitIds" />
    <input type="hidden" id="hSearchUnit" />
    <input type="hidden" id="hFilterUnitLists" />
    <input type="hidden" id="hDevInfoAge" />
    <input type="hidden" id="hAgeGIds" />
    <input type="hidden" id="hUNSDAge" />
    <input type="hidden" id="hAgeIds" />
    <input type="hidden" id="hSearchAge" />
    <input type="hidden" id="hFilterAgeLists" />
    <input type="hidden" id="hDevInfoSex" />
    <input type="hidden" id="hSexGIds" />
    <input type="hidden" id="hUNSDSex" />
    <input type="hidden" id="hSexIds" />
    <input type="hidden" id="hSearchSex" />
    <input type="hidden" id="hFilterSexLists" />
    <input type="hidden" id="hDevInfoLocation" />
    <input type="hidden" id="hLocationGIds" />
    <input type="hidden" id="hUNSDLocation" />
    <input type="hidden" id="hLocationIds" />
    <input type="hidden" id="hSearchLocation" />
    <input type="hidden" id="hFilterLocationLists" />
    <input type="hidden" id="hSearchIUS" />
    <input type="hidden" id="hIndicator" />
    <input type="hidden" id="hUnit" />
    <input type="hidden" id="hSubgroup" />
    <input type="hidden" id="hMap" />
    <input type="hidden" id="hAge" />
    <input type="hidden" id="hSex" />
    <input type="hidden" id="hLocation" />
    <input type="hidden" id="hFrequency" />
    <input type="hidden" id="hSourceType" />
    <input type="hidden" id="hNature" />
    <input type="hidden" id="hUnitMultiplier" />
    <input type="hidden" id="hDevInfoMetadata" />
    <input type="hidden" id="hCategoryGIds" />
    <input type="hidden" id="hUNSDMetadata" />
    <input type="hidden" id="hConceptIds" />
    <input type="hidden" id="hSearchMetadata" />
    <input type="hidden" id="hFilterMetadataLists" />
    <input type="hidden" id="hSelect" />
    <input type="hidden" id="hCodelistId" />
    <input type="hidden" id="hCodelistAgencyId" />
    <input type="hidden" id="hCodelistVersion" />
    <input type="hidden" id="hCodelistName" />
    <input type="hidden" id="hSelectedCodelist" />
    <input type="hidden" id="hOrginialSelectedAgeCodelist" />
    <input type="hidden" id="hOrginialSelectedSexCodelist" />
    <input type="hidden" id="hOrginialSelectedLocCodelist" />
    <input type="hidden" id="hdnDivIndicatorInnerHtml" />
    <input type="hidden" id="hdnDivUnitInnerHtml" />
    <input type="hidden" id="hdnDivAgeInnerHtml" />
    <input type="hidden" id="hdnDivSexInnerHtml" />
    <input type="hidden" id="hdnDivLocationInnerHtml" />
    <input type="hidden" id="hDevInfoArea" />
    <input type="hidden" id="hAreaGIds" />
    <input type="hidden" id="hUNSDArea" />
    <input type="hidden" id="hAreaIds" />
    <input type="hidden" id="hSearchArea" />
    <input type="hidden" id="Lang_InvalidFileExtension" />
    <input type="hidden" id="Lang_SelectExeclFileToUpload" />
    <input type="hidden" id="Lang_ImportSuccess" />
    <input type="hidden" id="Lang_ImportFaliure" />
    <input type="hidden" id="hSelectOneOption" />
    <input type="hidden" id="hMappingFileGenerateStatus" />
    <input type="hidden" id="hMapOneConcept" />
    <input type="hidden" id="hAlreadyMappedConcept" />
    <input type="hidden" id="hSelectOneMappedRow" />
    <input type="hidden" id="hPreviousMappingsRemoved" />
    <input type="hidden" id="hMapOneCode" />
    <input type="hidden" id="hSelectAllDropdowns" />
    
    <!-- START DEVELOPER CODE -->
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

        var di_components = "Language,Search";
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
    <script type="text/javascript" src="../js/di.jquery.form.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript">
        di_jq(document).ready(function () {
            di_jq('#frmUploadExcel').ajaxForm({
                success: function (response) {
                    if (response == "No File") {
                        //alert("Please browse a XLS file to upload.");
                        alert(z('Lang_SelectExeclFileToUpload').innerHTML);
                    }
                    else if (response == "Invalid File") {
                        //alert("Invalid file! Only MS Excel(XLS) files are supported.");
                        alert(z('Lang_InvalidFileExtension').innerHTML);
                    }
                    else {
                        ImportMappingExcel(MappingType, response);
                    }
                }

            });

        });
    </script>
</asp:Content>
