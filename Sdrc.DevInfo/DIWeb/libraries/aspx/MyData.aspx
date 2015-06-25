<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true"
    CodeFile="MyData.aspx.cs" Inherits="libraries_aspx_MyData" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="Server">
    <style type="text/css">
        /*    
#csvDataTblId td {
	padding:2px;
}
#csvDataUNMtbl td {
	padding:2px;
}
.inputField { 
	font-size:11px;
	border:1px solid #d3d3d3;
}*/
        .menuItem
        {
            cursor: pointer;
        }
        .menuItem:hover
        {
            cursor: pointer;
            color: red;
        }
    </style>
</asp:Content>
<asp:content id="Content2" contentplaceholderid="cphMainContent" runat="Server">
    <!-- Popup for Select Area Component ...starts-->
    <input type="hidden" id="MapServerToolTip" value="" />
    <div id='AreaOuterBox' class="popup_block" style="width: 702px; height: 400px;">
        <h1 class="popup_heading" id="langSelectArea">
            <!--Select Area-->
        </h1>
        <!-- Popup Inside Content ...starts-->
        <div class="popup_brd">
            <div id="area_div_popup">
            </div>
        </div>
        <!-- Popup Inside Content ...ends-->
    </div>
    <!-- for callout -->
    <div id="divCallout" class="callout border-callout callout_main_cont">
        <div id="divCalloutText" class="content">
            <!--Callout text-->
        </div>
        <b class="border-notch notch"></b><b class="notch"></b>
    </div>
    <!-- Popup for Select Area Component ...ends-->
    <!-- Popup for Use map serverpopup component Component ...starts-->
    <div id='MapServerPopup' class="popup_block" style="width: 500px; height: 210px;">
        <h1 class="popup_heading" id="langUseMapServer">
            <div id="MapSHeading"></div></h1>
        <!-- Popup Inside Content ...starts-->
        <div class="popup_brd" style="padding-left: 10px;">
            <br />
            <p id="useMapServerDescription1">
                <div id="MapSDesc1"></div></p>
            <br />
            <p id="useMapServerDescription3">
                <div id="MapSDesc2"></div></p>
            <br />
            <br />
        </div>
        <!-- Popup Inside Content ...ends-->
        <div id="mapserver_popup" class="di_gui_button_pos">
            <input type="button" name="YesBtn" id="useMapServerBtn" value="Yes" class="di_gui_button"
                onclick="checkUseMapServerChkBox()" />
            <input type="button" name="NoBtn" id="DontUseMapServerBtn" value="No" class="di_gui_button"
                onclick="hideMapServerPopup()" />
        </div>
    </div>
    <!-- Popup for MyData Note ...starts-->
    <div id='myDataNote' class="popup_block" style="width: 720px; height: 400px;">
        <h1 class="popup_wid_subhdn pddn_nne" id="lang_Note">
            <!--Note-->
        </h1>
        <h2 class="popup_subhdin" id="lang_CSI_CSVf">
            <!--Create, Save and Import in CSV format-->
        </h2>
        <!-- Popup Inside Content ...starts-->
        <!--<div class="popup_brd">-->
        <div class="no_brdr_spc">
            <p>
                <img src="../../stock/themes/default/images/spacer.gif" alt="" width="68" height="68"
                    class="info_i_icn flt_lft" /><span id="lang_IOUD_string"><!--In order to upload data from an Excel file into DevInfo, it must first be saved as a CSV file. CSV (Comma Separated Value) format simplies the data entry process for large volumes of data, which are typically disaggregated in numerous ways as well as by numerous geographical areas.--></span></p>
            <p id="lang_FEDE_string">
                <!--First, enter your data in an Excel spreadsheet using the following format. Note that you can have as many Indicators | Units | Subgroups as you wish.-->
            </p>
            <p>
                <img src="../../stock/themes/default/images/mydata_tablr_data.png" alt="" width="707"
                    height="161" /></p>
            <p>
                <span id="lang_TClickon">
                    <!--Then, click on-->
                </span>&nbsp;<b id="lang_SaveAs"><!--Save As--></b>&nbsp;<span id="lanf_ASCSV_string"><!--and select "Comma Separated Values (.csv)." You are now able to upload your data in the My Data Section--></span></p>
            <!--</div>-->
        </div>
        <!-- Popup Inside Content ...ends-->
    </div>
    <!-- Popup for MyData Note ...ends-->
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <div id="cyvContainer" class="flt_lft">
                    <form name="MyTMPform" id="MyTMPform" method="POST" action="OatInputFile.aspx" enctype="multipart/form-data">
                    <div id="popup_reg_vis" style="height: auto">
                        <!-- Popup Inside Content ...starts-->
                        <div id="reg_content_containers">
                            <h2 id="spanPageTitle">
                                <!--Create your own visualization-->
                            </h2>
                            <h5 id="pPageDescription">
                                <!--Create your own visualization-->
                            </h5>
                            <!-- Upload/Visualization/Share section ...starts-->
                            <div class="upld_vis_shr_main">
                                <div class="md_upld_main flt_lft">
                                    <div class="md_upload_sec">
                                        <img src="../../stock/themes/default/images/spacer.gif" alt="" width="72" height="56" /></div>
                                    <p class="md_blu_hdn" id="lang_Upload">
                                        <!--Upload-->
                                    </p>
                                    <p id="lang_the_data_from_file">
                                        <!--the data from file-->
                                    </p>
                                </div>
                                <div class="md_uvs_arrw flt_lft">
                                </div>
                                <div class="md_upld_main md_viz_spc flt_lft">
                                    <div class="md_viz_sec">
                                        <img src="../../stock/themes/default/images/spacer.gif" alt="" width="62" height="64" /></div>
                                    <p id="lang_Create_your_own">
                                        <!--Create your own-->
                                    </p>
                                    <p class="md_blu_hdn" id="lang_Visualization">
                                        <!--Visualization-->
                                    </p>
                                </div>
                                <div class="md_uvs_arrw flt_lft">
                                </div>
                                <div class="md_upld_main md_shr_spc flt_lft">
                                    <div class="md_shr_sec">
                                        <img src="../../stock/themes/default/images/spacer.gif" alt="" width="76" height="53" /></div>
                                    <p class="md_blu_hdn" id="lang_Share">
                                        <!--Share-->
                                    </p>
                                    <p id="lang_it_with_the_community">
                                        <!--it with the community-->
                                    </p>
                                </div>
                                <div class="clear">
                                </div>
                            </div>
                            <!-- Upload/Visualization/Share section ...ends-->
                            <!--<div id="reg_lft_sec" style="height:330px;">-->
                            <div id="reg_lft_sec">
                                <div id="mydata_step1" class="flt_lft">
                                    <h3 id="lang_Upload_DFF">
                                        <!--1 Upload data from file-->
                                    </h3>
                                    <h4>
                                        <span id="step1Description">
                                            <!--Upload data from the following file formats: CSV, DevInfo Data Entry.-->
                                        </span><a href="javascript:void(0)" onclick="myDataNote()">
                                            <img src="../../stock/themes/default/images/spacer.gif" alt="" width="20" height="20"
                                                class="ques_icn" /></a></h4>
                                    <!-- Buttons/Input Field ...starts -->
                                    <div id="uploadFileData" class="clear">
                                        <input type="file" id="cyvFile" name="cyvInputFile" class="confg_frm_inp_bx_txt_reg"
                                            size="35" /><input type="hidden" name="csvFilePath" id="csvFilePath" />
                                    </div>
                                    <!-- Buttons/Input Field ...starts -->
                                    <!-- Copy paste text area-->
                                    <h3 id="lang_CopyP_TData_BB">
                                        <!--OR Copy and paste tabular data into the box below-->
                                    </h3>
                                    <h4 id="mydata_info_btn" class="flt_lft">
                                    </h4>
                                    <div id="clearFreeTxt" onclick="clearField()">
                                        <!--Clear-->
                                    </div>
                                    <div class="clear flt_lft">
                                        <textarea id="freeTxtData" name="freeTxtData" rows="1" cols="40" tabindex="0"></textarea></div>
                                </div>
                                <!-- End step 1 -->
                                <div class="clear">
                                </div>
                                <!-- Start step 2 -->
                                <div id="mydata_step2" style="display: none;">
                                    <h3 id="lang_Identify_AID_Aname">
                                        <!--2 Identify Area IDs or Area Names (Optional)-->
                                    </h3>
                                    <h4 id="step2Description">
                                        <!--To create a map from your data, the data must be linked to the Area IDs contained in the DevInfo Digital Map Library. To link your data, identify the column which contains Area IDs. If your data set does not contain DevInfo Area IDs, the identify which column has geographic Area Names to lookup up Area IDs automatically.-->
                                    </h4>
                                    <!-- Start Table grid -->
                                    <div class="flt_lft">
                                        <div id="csvDataTblId">
                                        </div>
                                        <div id="isRowTimePeriodDiv">
                                            <input type="checkbox" name="isRowTimePeriod" id="isRowTimePeriod" onchange="disableAllTimePeriodDdl(this)" />
                                            <span id="lang_DDataColumn_HCT">
                                                <!--Does data column headers contain timeperiod?-->
                                            </span>
                                        </div>
                                        <!-- UI for use map server maps -->
                                        <%-- <div id="isShowMapServerDiv" style="display:none;"><input type="checkbox" name="isShowMapServer" id="isShowMapServer" value="yes" disabled="true"/> <span id="lang_ShowMapServerDiv"><!--Use map server maps--></span></div>--%>
                                    </div>
                                </div>
                                <!-- End step 2 -->
                                <div class="clear">
                                </div>
                                <!-- Start step 3 -->
                                <div id="mydata_step3" style="display: none;">
                                    <h3 id="lang_Matching_Area_Names">
                                        <!--3 Matching Area Names-->
                                    </h3>
                                    <h4 id="step3Description">
                                        <!--Some Area IDs or Area Names did not match with the DevInfo Digital Map Library. Select the best match for each one below.-->
                                    </h4>
                                    <h4 id="step3DescriptionMatch" style="display: none;">
                                        <!--All Area matched.-->
                                    </h4>
                                    <!-- Start unmatched grid -->
                                    <div class="flt_lft">
                                        <div id="csvDataUNMtbl">
                                        </div>
                                        <div id="isShowMapServerDiv" style="display: none;">
                                            <input type="checkbox" name="isShowMapServer" onchange="useMapServerCheckChanged(this)"
                                                id="isShowMapServer" value="yes" disabled="true" />
                                            <span id="lang_ShowMapServerDiv">
                                                <!--Use map server maps-->
                                            </span>&nbsp;&nbsp;<img id="imgmaptip" rel='maptip' src="../../stock/themes/default/images/help-small.png" onmouseover="showMapServerToolTip(this)"  onmousemove="showMapServerToolTip(this)" onmouseout="HideCallout('divCallout');" />
                                        </div>
                                    </div>
                                </div>
                                <!-- End step 3 -->
                                <div id="loaderDiv" style="display: none;">
                                    <h3 id="H1">
                                        <!--3 Loading Data From Map Server-->
                                    </h3>
                                    <h4 id="H2">
                                        <!--Please wait while data is being loaded from map server.-->
                                    </h4>
                                    <!-- Start unmatched grid -->
                                    <div class="flt_lft">
                                        <img src="../../stock/themes/default/images/ajax-loader.gif" id="loader" />
                                    </div>
                                </div>
                            </div>
                            <div class="clear">
                            </div>
                            <!-- Popup Buttons ...starts-->
                            <div class="mydata_bttns">
                                <div class="flt_lft">
                                    <input type="button" name="BackBttn" id="BackBttn" value="" class="di_gui_button_disabled"
                                        disabled="disabled" onclick="doBack()" /></div>
                                <div class="flt_rgt">
                                    <input type="button" name="NextBttn" id="NextBttn" value="" class="di_gui_button_disabled"
                                        disabled="disabled" onclick="doNext()" /></div>
                                <input type="hidden" name="mappingStr" id="mappingStr" value="" />
                                <input type="hidden" name="arIDColIndex" id="arIDColIndex" value="-1" />
                                <input type="hidden" name="arNameColIndex" id="arNameColIndex" value="-1" />
                                <input type="hidden" name="cyvType" id="cyvType" value="" />
                                <input type="hidden" name="tPeriosCol" id="tPeriosCol" value="" />
                                <input type="hidden" name="lng_Code" id="lng_Code" value="" />
                                <input type="hidden" name="db_Nid" id="db_Nid" value="" />
                                <input type="hidden" name="ShowMapServer" id="ShowMapServer" value="false" />                
                            </div>
                            <!-- Popup Buttons ...ends-->
                        </div>
                        <!-- Popup Inside Content ...ends-->
                    </div>
                    </form>
                </div>
            </td>
        </tr>
    </table>
    <%--REGION FOR DECLARING HIDDEN INPUT FIELDS FOR LANGUAGE HANDLING OF AREA & INDICATOR COMPONENTS--%>
    <input type="hidden" id="langHiddenSelectAll" value="" />
    <input type="hidden" id="langHiddenSelectarealevel" value="" />
    <input type="hidden" id="langHiddenByTree" value="" />
    <input type="hidden" id="langHiddenListAtoZ" value="" />
    <input type="hidden" id="langHiddenByMap" value="" />
    <input type="hidden" id="langHiddenSearch" value="" />
    <input type="hidden" id="langHiddenViewselections" value="" />
    <input type="hidden" id="langHiddenMetadata" value="" />
    <input type="hidden" id="langHiddenShowwheredataexist" value="" />
    <input type="hidden" id="langHiddenClear" value="" />
    <input type="hidden" id="langHiddenClose" value="" />
    <input type="hidden" id="langHiddenFirst" value="" />
    <input type="hidden" id="langHiddenBack" value="" />
    <input type="hidden" id="langHiddenNext" value="" />
    <input type="hidden" id="langHiddenLast" value="" />
    <input type="hidden" id="langHiddenPage" value="" />
    <input type="hidden" id="langHiddenDataview" value="" />
    <input type="hidden" id="Select_MyData" value="" />
    <input type="hidden" id="Mydata_LangAreaId" value="" />
    <input type="hidden" id="Mydata_LangAreaName" value="" />
    <input type="hidden" id="Mydata_LangTimePeriod" value="" />
    <%--END REGION--%>
    <!-- DEVELOPER CODE -->
    <script type="text/javascript" language="javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

        var di_components = "Area,Language";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    </script>
    <script type="text/javascript" language="javascript" src="../js/di.jquery.form.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" language="javascript" src="../js/MyData.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" language="javascript">        onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=dbname%>', '<%=dbsummary%>', '<%=dbdesc%>', '<%=DefAreaCount%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');



    </script>
    <!-- END OF DEVELOPER CODE -->
</asp:content>
