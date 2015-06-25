<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" EnableSessionState="true"
    AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="libraries_aspx_Home" %>

<asp:content id="Content1" contentplaceholderid="cphHeadContent" runat="Server">
    <script type="text/javascript" language="javascript" src="../js/swfobject.js?v=<%=Global.js_version%>"></script>
    <!-- jQuery (required) -->
    <script type="text/javascript" src="../js/jquery.hm_slider_min.js?v=<%=Global.js_version%>"></script>
    <!-- HomeSlider -->

    <link rel="stylesheet" href="../../stock/themes/default/css/home_slider.css?v=<%=Global.js_version%>" />
    <link rel="stylesheet" href="../../stock/themes/default/css/theme-metallic.css?v=<%=Global.js_version%>" />
    <script type="text/javascript" src="../js/jquery.home_slider.js?v=<%=Global.js_version%>"></script>
   
    <script type="text/javascript" src="../js/jquery.ellipsis.js"></script>
    <!-- Older IE stylesheet, to reposition navigation arrows, added AFTER the theme stylesheet above -->
    <!--[if lte IE 7]>
	<link rel="stylesheet" href="../../stock/themes/default/css/home_slider-ie.css" type="text/css" media="screen" />
	<![endif]-->
    <!-- <script src="../js/AC_RunActiveContent.js" type="text/javascript"></script>	-->
    <script type="text/javascript">
        //var elm = document.getElementsByTagName('body');
        //if ((screen.width>=800) && (screen.height>=600))
        if ((screen.width >= 1152) && (screen.width < 1280)) {
            document.writeln('<link rel="stylesheet" type="text/css" href="../../stock/themes/default/css/v_slider/style1152.css" />');
        }
        if ((screen.width >= 1280) && (screen.width < 1360)) {
            document.writeln('<link rel="stylesheet" type="text/css" href="../../stock/themes/default/css/v_slider/style1280.css" />');
        }
        if (screen.width >= 1360) {
            document.writeln('<link rel="stylesheet" type="text/css" href="../../stock/themes/default/css/v_slider/style1366.css" />');
        }
        //else
        //{
        //document.writeln('<link rel="stylesheet" type="text/css" href="../../stock/themes/default/css/v_slider/style1024.css" />');
        //}

    </script>
    <script type="text/javascript">
        window.onload = function () {
            try {
                //if (parent) {
                //var oHead = document.getElementsByTagName("head")[0];
                //var arrStyleSheets = parent.document.getElementsByTagName("style");
                //                for (var i = 0; i < arrStyleSheets.length; i++)
                //                    oHead.appendChild(arrStyleSheets[i].cloneNode(true));
                //}
                $(".ellipsis").ellipsis(); //ADDED THIS LINE FOR ELLIPSIS FUNCTION   
                if (readCookie('hlngcode') == "ar") {
                    setTimeout("rtlPlayPauseBPos()", 2000);
                }
            }
            catch (err) {
                err.Message;
            }
        }
    </script>

</asp:content>
<asp:content id="cntMainContent" contentplaceholderid="cphMainContent" runat="Server">
    <!-- Page Markup starts from here -->
    <!--REGION FOR DECLARING HIDDEN INPUT FIELDS FOR LANGUAGE HANDLING OF AREA & INDICATOR COMPONENTS-->
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
    <input type="hidden" id="langHiddenSelectarealevel" value="" />
    <input type="hidden" id="langHiddenByTree" value="" />
    <input type="hidden" id="langHiddenListAtoZ" value="" />
    <input type="hidden" id="langHiddenAlphabetically" value="" />
    <input type="hidden" id="langHiddenByMap" value="" />
    <input type="hidden" id="langHiddenFirst" value="" />
    <input type="hidden" id="langHiddenBack" value="" />
    <input type="hidden" id="langHiddenNext" value="" />
    <input type="hidden" id="langHiddenLast" value="" />
    <input type="hidden" id="langHiddenPage" value="" />
    <input type="hidden" id="langQdsIndCaption" value="" />
    <input type="hidden" id="langQdsAreaCaption" value="" />
    <input type="hidden" id="langQdsIndDefText" value="" />
    <input type="hidden" id="langQdsAreaDefText" value="" />
    <input type="hidden" id="langQdsSerchBtnText" value="" />
    <input type="hidden" id="langQdsAreaBlockText" value="" />
    <input type="hidden" id="langIndicators" value="" />
    <input type="hidden" id="langAreas" value="" />
    <input type="hidden" id="langSources" value="" />
    <input type="hidden" id="langDataValues" value="" />
    <input type="hidden" id="langUpdatedOn" value="" />
    <input type="hidden" id="langSubGroup" value="" />
    <input type="hidden" id="langAreasColon" value="" />
    <input type="hidden" id="langSourcesColon" value="" />
    <input type="hidden" id="langResultsTimePeriod" value="" />
    <input type="hidden" id="langResultsDimensions" value="" />
    <input type="hidden" id="langResultsDataValues" value="" />
    <input type="hidden" id="langResultsCloudView" value="" />
    <input type="hidden" id="langResultsAddToCart" value="" />
    <input type="hidden" id="langResultsVisualize" value="" />
    <input type="hidden" id="langResultsShare" value="" />
    <input type="hidden" id="isQdsToBeRendered" value="<%= isQdsToBeRendered %>" />
    <input type="hidden" id="langREMOVE_FROM_DATA_CART" value="" />
    <input type="hidden" id="langLblContactDbAdm" value="" />
    <input type="hidden" id="langLblDbAdmName" value="" />
    <input type="hidden" id="langLblDbAdmInstitution" value="" />
    <input type="hidden" id="langLblDbAdmEmail" value="" />
    <input type="hidden" id="langHide" value="" />
    <input type="hidden" id="langShow" value="" />
    <input type="hidden" id="langMin" value="" />
    <input type="hidden" id="langMax" value="" />
    <input type="hidden" id="langQDSResults" value="" />
    <input type="hidden" id="langQDSTimeTaken" value="" />
    <input type="hidden" id="langQDSSeconds" value="" />
    <input type="hidden" id="langSearchingDatabase" value="" />
    <input type="hidden" id="langLoadingResults" value="" />
    <input type="hidden" id="langmoreAreas" value="" />
    <input type="hidden" id="langmoreIndicators" value="" />
    <input type="hidden" id="langmoreUDKs" value="" />
    <input type="hidden" id="langGalleryMore" value="" />
    <input type="hidden" id="langGalleryModifiedOn" value="" />
    <input type="hidden" id="langGalleryDescription" value="" />
    <input type="hidden" id="langMRDFor" value="" />
    <input type="hidden" id="langDownloadCSVFile" value="" />
    <input type="hidden" id="Lang_ToolTipShowVideo" value="" />
    <input type="hidden" id="show_cloud" value="<%= hShowCloud %>" />
    <!--END REGION-->
    <!-- Main Container ...starts-->
    <!-- Change Database and Adaptation Screen -->
    <!-- Slider and Search Boxes Section ...starts-->
    <!-- Slider Area ...starts-->
    <div class="hm_vid_pnl_contnr">
        <div id="sh_slider">
            <!-- Slider Panel ...starts-->
            <div class="panel_table_hm_sldr">

                <%--<div id="sh_slider">--%>
               <div id="slider">
                    <!-- HomeSlider #1 -->
                    <ul id="slider1" style="text-align: center;">
                    </ul>
                    <!-- END HomeSlider #1 -->
                </div>
            </div>
            <!-- Slider Panel ...ends-->
        </div>

        <div class="clear">
        </div>
     
        <!-- Expand/Collapse Functionality ...starts -->
        <div id="hm_main_exp_colp" onclick="ShowHideSlider();" style="cursor: pointer;">
            <div class="hm_main_exp_colp_lft">
            </div>
            <div class="hm_main_exp_colp_mid">
                <img src="../../stock/themes/default/images/colp_up_slider.png" alt="collapse" id="but_sh_img_colp_up" /><img
                    src="../../stock/themes/default/images/exp_down_slider.png" alt="expand" id="but_sh_img_exp_down" /><a
                        href="#" id="but_sh"><%--Hide--%></a></div>
            <div class="hm_main_exp_colp_rgt">
            </div>
        </div>
        <!-- Expand/Collapse Functionality ...ends -->
    </div>
    <!-- Slider Area ...ends-->
    <div class="clear"></div>
    <div class="content_containers">
        <!-- QDS & DBS Search Boxes Area ...starts-->
        <div id="dbs_qds_main_hm" class="roundedcorners">
            <!-- Quick Data Search Section ...starts-->
            <div id="qds_cont">
                <div class="flt_lft qds_bx">
                    <p class="heading1_qds" id="langQuickDataSearch">
                        <!--Quick data search-->
                    </p>
                    <div class="roundedcorners_thin bckgrnd_wht">
                        <div id="quick_data_search_div">
                        </div>
                    </div>
                </div>
                <span class="clear"></span>
            </div>
            <!-- Quick Data Search Section ...ends-->
            <h1 class="flt_lft or_txt" id="langOR">
            </h1>
            <!-- DBS Boxes Section ...starts-->
            <div id="pnlDSBY_pg">
                <h1 id="langBrowseF_D_B">
                    <!--Browse for data by-->
                </h1>
                <div class="clear">
                </div>
                <!-- TOPIC Tab ...starts-->
                <div class="tab_panel_pg column" onclick="doIndicatorSelection()">
                    <div class="tab_topc_sec_pg">
                        <div class="tab_topc_icn">
                        </div>
                        <div class="tab_topc_sec_contnt">
                            <h3 id="langTopic">
                                <!--Topic-->
                            </h3>
                            <p id="langSearchB_SGTI">
                                <!--Search by sector, goal, theme or indicator name-->
                            </p>
                        </div>
                    </div>
                </div>
                <!-- TOPIC Tab ...ends-->
                <h1 class="flt_lft or_txt_ta" id="langOR1">                    
                </h1>
                <!-- AREA Tab ...starts-->
                <div class="tab_panel_pg column" onclick="doAreaSelection()">
                    <div class="tab_ar_sec_pg">
                        <div class="tab_ar_icn">
                        </div>
                        <div class="tab_ar_sec_contnt">
                            <h3 id="langArea">
                                <!--Area-->
                            </h3>
                            <p id="langSearchB_CPD">
                                <!--Search by country, province or district-->
                            </p>
                        </div>
                    </div>
                </div>
                <!-- AREA Tab ...ends-->
            </div>
            <!-- DBS Boxes Section ...ends-->
            <span class="clear"></span>
            <div class="flt_rgt pddn_tp_bttm_fve adv_search_btn_pos">
                <%-- <div><embed src='http://www.youtube.com/watch?v=udg4Kvs4St8' /></div>--%>
                <div id="DivHowToVideoLinks" class="flt_lft" style="display: none;">
                    <img id="imgHowToVideo" src="../../stock/themes/default/images/play-icon-Hm.png" class="Img-How-toVideo-Hm" onclick="PlayHowToVideoPopUp('true','Home');"/>
                    
                </div>
                <div class="flt_lft">
                    <!-- Split Grdnt Buttn ...starts-->
                    <div class="bttn_splt_grd_lft">
                    </div>
                    <div class="bttn_splt_grd_mid">
                        <a href="javascript:void(0)" id="langAdvancedSearch" onclick="doAdvanceSelection()">
                            <!--Advanced Search-->
                        </a>
                    </div>
                    <div class="bttn_splt_grd_rgt">
                    </div>
                    <!-- Split Grdnt Buttn ...ends-->
                </div>
                <div class="clear"></div>
            </div>
            <div class="clear"></div>
        </div>
        <!-- Added for Popup Box func in gallery home-->
        <div id='PopupOuterBox' class="popup_block">
            <div id="popupContent">
            </div>
        </div>
        <div id="DivHowToVideo" class="popup_block">
            <div id="dvHowToVdeo" style="display: block;">
            </div>
        </div>
        <div id='PopupMoreBox' class="popup_block">
            <div id="moreContent">
            </div>
        </div>
        <span id="langkeywords" style="display: none">Keywords</span>
        <input id="lang_Download_PNG" value="" type="hidden" />
        <input id="lang_Download_SVG" value="" type="hidden" />
        <input id="lang_Download_XLS" value="" type="hidden" />
        <input id="lang_Download_Image" value="" type="hidden" />
        <input id="lang_Download_KML" value="" type="hidden" />
        <input id="lang_ppup_Next" value="" type="hidden" />
        <input id="lang_ppup_Prev" value="" type="hidden" />
        <input type="hidden" id="delete_confirmation_txt" value="" />
        <input type="hidden" id="delete_presentation_txt" value="" />
        <input type="hidden" id="delete_right_txt" value="" />
        <input type="hidden" id="NoRecordsFound" value="" />
        <input id="GalView" value="" type="hidden" />
        <input id="GalDownload" value="" type="hidden" />
        <input id="GalShare" value="" type="hidden" />
        <input id="GalDelete" value="" type="hidden" />
        <!-- End of code added for Popup Box func in gallery home-->
        <!-- DBS Boxes Section ...ends-->
        <!-- QDS & DBS Search Boxes Area ...ends-->
        <!-- Slider and Search Boxes Section ...ends-->
        <!-- Change Database and Adaptation Screen FINISH -->
        <!-- Line Divider ...starts-->
        <!--<div class="line_shdw_dvdr"></div>-->
        <!-- Line Divider ...ends-->
        <!-- Time Counter ...starts-->
        <div id="divTimeCounter" class="spanTimeCounter_pos">
            <i><span class="content flt_lft" id="spanTimeCounter"></span></i><span onclick="AddAllToCart();"
                id="spanAddAllToCartMain" class="content flt_lft"><span class="qds_icon2 flt_lft">&nbsp;</span><span
                    id="spanAddAllToCart"><%--Add all to cart--%></span></span> <span onclick="RemoveAllFromCart();"
                        id="spanRemoveAllFromCartMain" class="content flt_lft"><span class="qds_icon7 flt_lft">
                            &nbsp;</span><span id="spanRemoveAllFromCart"><%--Remove all from cart--%></span></span>
            <span id="spanMultipleDbSearchMain" class="content flt_lft" onclick="MoveOnCatalogForSearchMultipleDatabases();">
                <span class="qds_icon8 flt_lft">&nbsp;</span><span id="spanMultipleDbSearch"><%--Click here to search through multiple databases--%></span></span>
        </div>
        <div class="clear">
        </div>
        <!--<br />-->
        <!-- Time Counter ...ends-->
        <!-- QDS DATA SECTION ...starts-->
        <!-- Loader Animation ...starts-->
        <div id="glry_LoadingOuter">
            <div class="clear">
                &nbsp;</div>
            <div id="spanLoadingOuter" style="display: none;">
                <img src="../../stock/themes/default/images/loading_img.gif" height="16" width="16"
                    alt="" /><i class="content" id="spanLoading"></i></div>
        </div>
        <!-- Loader Animation ...ends-->
        <!-- Data Cart Box ...starts-->
        <div id="tblViewData" class="roundedcorners" style="display: none; z-index: 99999;">
            <h1 id="lang_Datacart">
                <!--Data cart-->
            </h1>
            <div id="tblViewDataInner" class="roundedcorners" style="display: none;">
                <div id="divViewData">
                </div>
            </div>
            <div id="butViewData" onclick="ViewAllData();">
                <div class="cmp_bviewd_lft" usemap="#Map">
                </div>
                <div class="cmp_bviewd_mid dv_button_value" id="lang_Data">
                    <!--Data-->
                </div>
                <div class="cmp_bviewd_rgt">
                </div>
            </div>
        </div>
        <!-- Data Cart Box ...ends-->
        <!-- For Thumbnail images ...starts-->
        <div id="divQDSResults">
        </div>
        <!-- For Thumbnail images ...ends-->
        <div class="clear">
            &nbsp;</div>
        <!-- CamInfo 2010 Info ...starts-->
        <div id="divDatabaseFullInfo">
        </div>
        <!-- CamInfo 2010 Info ...ends-->
        <!-- Call Out Tool Tip ...starts-->
        <div id="divCallout" class="callout border-callout">
            <div id="divCalloutText" class="content">
                <!--Callout text-->
            </div>
            <b class="border-notch notch"></b><b class="notch"></b>
        </div>
        <div id="divCallout_qds" class="callout_cata border-callout-cata">
            <ul id="divDimensionsList">
            </ul>
            <b class="border-notch notch"></b><b class="notch"></b>
        </div>
        <!-- Call Out Tool Tip ...ends-->
        <!-- Cloud Popup ...starts-->
        <div id="divCloudOuter" class="popup_block">
            <div id="divCloudHeader">
            </div>
            <div id="divCloud">
            </div>
        </div>
        <!-- Cloud Popup ...ends-->
        <!-- start for pop-up -->
        <div id="popup_db_details" class="popup_block">
            <div id="database_div">
            </div>
        </div>
        <!-- end for pop-up -->
        <!-- QDS DATA SECTION ...ends-->
        <!-- Divider Strip ...starts-->
        <br />
        <%--<div class="dvdr_strip"><img src="../../stock/themes/default/images/divider.jpg" alt="" /></div>--%>
        <!-- Divider Strip ...ends-->
        <div class="clear">
        </div>
        <!-- Main Container ...ends-->
    </div>
    <!-- ALL POPUPS DEFINED ...starts-->
    <!-- Popup for DI Video ...starts-->
    <div id="VideoPopupOuterBox" class="popup_block">
        <span id="flashcontent" style="z-index: 1650"></span>
    </div>
    <!-- Popup for DI Video ...ends-->
    <!-- Popup for Select Area Component ...starts-->
    <div id="AreaOuterBox" class="popup_block">
        <h1 class="popup_heading" id="langSelectArea">
            <!--Select Area-->
        </h1>
        <!-- Popup Inside Content ...starts-->
        <div class="popup_brd">
            <div id="area_div_popup">
            </div>
        </div>
        <!-- Popup Inside Content ...ends-->
        <!-- Popup Buttons ...starts-->
        <div class="di_gui_button_pos">
            <input type="button" name="areaOk" id="areaOk" value="" onclick="onGotoQDSResult('area')"
                class="di_gui_button" /><input type="button" name="areaCancel" id="areaCancel" value=""
                    onclick="hidePopupSelection()" class="di_gui_button" /></div>
        <!-- Popup Buttons ...starts-->
    </div>
    <!-- Popup for Select Area Component ...ends-->
    <!-- Popup for Select Topic Component ...starts-->
    <div id="IndOuterBox" class="popup_block">
        <h1 class="popup_heading" id="lang_SelectTopic">
            <!--Select Topic-->
        </h1>
        <!-- Popup Inside Content ...starts-->
        <div class="popup_brd">
            <div id="indicator_div_popup">
            </div>
        </div>
        <!-- Popup Inside Content ...ends-->
        <!-- Popup Buttons ...starts-->
        <div class="di_gui_button_pos">
            <input type="button" name="indOk" id="indOk" value="" onclick="onGotoQDSResult('ind')"
                class="di_gui_button" /><input type="button" name="indCancel" id="indCancel" value=""
                    onclick="hidePopupSelection()" class="di_gui_button" /></div>
        <!-- Popup Buttons ...ends-->
    </div>
    <!-- Popup for Select Topic Component ...ends-->
    <!-- Popup for Default Areas Selected List ...starts-->
    <div id="popup_sel_areas" class="popup_block">
        <h1 class="popup_heading" id="langDefaultAreas">
            <!--Default Areas-->
        </h1>
        <!-- Popup Inside Content ...starts-->
        <div class="popup_brd">
            <!-- <div id="popup_sel_areas_count"></div>-->
            <div id="popup_sel_areas_names">
            </div>
        </div>
        <!-- Popup Inside Content ...ends-->
        <!-- Popup Buttons ...starts-->
        <div class="di_gui_button_pos">
            <!--<input type="button" value="" class="di_gui_button" onclick="doAreaSelection();" id="langChangeAreaSelection" />-->
            <input type="button" value="" class="di_gui_button" onclick="CloseSelectedAreas();"
                id="langDF_Popp_Ok" /></div>
        <!-- Popup Buttons ...ends-->
    </div>
    <!-- Popup for Default Areas Selected List ...ends-->
    <!-- Popup for Advanced Search ...starts-->
    <div id="advSearchOuterBox" class="popup_block">
        <h1 class="popup_heading" id="lang_AdvancedSearch">
            <!--Advanced Search-->
        </h1>
        <!-- Component Area ...starts-->
        <div id="pnlDSBY">
            <!-- TOPIC Tab ...starts-->
            <div id="tab_ind_selected" class="tab_panel_selected column" onclick="showNavTab('ind')">
                <div class="tab_topc_sec">
                    <div class="tab_topc_icn">
                    </div>
                    <h3 id="lang_ppup_Topic">
                        <!--Topic-->
                    </h3>
                    <p id="lang_ppup_SearchB_SGTI">
                        <!--Search by sector, goal, theme or indicator name-->
                    </p>
                </div>
            </div>
            <!-- TOPIC Tab ...ends-->
            <!-- AREA Tab ...starts-->
            <div id="tab_area_selected" class="tab_panel column" onclick="showNavTab('ar')">
                <div class="tab_ar_sec">
                    <div class="tab_ar_icn">
                    </div>
                    <h3 id="lang_ppup_Area">
                        <!--Area-->
                    </h3>
                    <p id="lang_ppup_SearchB_CPD">
                        <!--Search by country, province or district-->
                    </p>
                </div>
            </div>
            <!-- AREA Tab ...ends-->
        </div>
        <div class="clear">
        </div>
        <!-- Indicator Tab Box ...starts-->
        <div id="tab_indicator" class="comp_box_brd_ppup" style="display: block;">
            <!-- Indicator Component ...starts-->
            <div id="indicator_div"></div>
            <!-- Indicator Component ...ends-->
        </div>
        <!-- Indicator Tab Box ...ends-->
        <div class="clear"></div>
        <!-- Area Tab Box ...starts-->
        <div id="tab_area" class="comp_box_brd_ar_ppup" style="display: none;">
            <!-- Area Component ...starts-->
            <div id="area_div"></div>
            <!-- Area Component ...ends-->
            <div class="clear"></div>
        </div>
        <!-- Area Tab Box ...ends-->
        <!-- Component Bottom Buttons Section ...starts-->
        <div class="panel_table">
            <!-- Left Txt Section ...starts-->
            <div id="lf_cbt_sec">
                <label id="lblDefalutAreaCount"></label>
                &nbsp;<span onclick="ShowSelectedAreas();" style="cursor: pointer;" id="langDefaultAreasSelected"><!--default areas selected--></span>
            </div>
            <!-- Left Txt Section ...ends-->
            <!-- Indicator/Area Component Bottom Right Buttons Section ...starts-->
            <div id="rt_cba_sec">
                <!-- Popup Buttons ...starts-->
                <div class="di_gui_button_pos">
                    <input type="button" name="" id="lang_Submit" value="" class="di_gui_button" onclick="onGotoDataViewPg()" /><input
                        type="button" name="" id="lang_Cancel" value="" class="di_gui_button" onclick="hidePopupSelection()" /></div>
                <!-- Popup Buttons ...starts-->
            </div>
            <!-- Indicator/Area Component Bottom Right Buttons Section ...ends-->
        </div>
        <!-- Component Bottom Buttons Section ...ends-->
        <!-- Component Area ...ends -->
    </div>
    <!-- Popup for Advanced Search ...ends-->
    <!-- ALL POPUPS DEFINED ...ends-->
    <!-- DEVELOPER CODE -->
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app
        var VisibleSlidersCount = '<%=Global.VisibleSlidersCount%>';
        var SliderAnimationSpeed = '<%=Global.SliderAnimationSpeed%>';
        var di_components = "Area,Indicator,Qds,Language";
        var di_vctype = "";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    </script>
    <!-- Added for Popup Box func in gallery home-->
    <script type="text/javascript" src="../js/NGallery.js?v=<%=Global.js_version%>"></script>
    <!-- End of code added for Popup Box func in gallery home-->
    <script type="text/javascript" src="../js/QDS.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/home.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript">

        DbAdmName = '<%=DbAdmName%>';
        DbAdmInstitution = '<%=DbAdmInstitution%>';
        DbAdmEmail = '<%=DbAdmEmail%>';
        onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=dbname%>', '<%=dbsummary%>', '<%=dbdesc%>', '<%=DefAreaCount%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=isGalleryEnabled%>', '<%=hdvnids%>');
    </script>
    <script type="text/javascript">
        di_jq(document).ready(function () {
            // set tool  tip for show video link
            document.getElementById("imgHowToVideo").onmouseover = function (event) {
                ShowCallout('divCallout', z('Lang_ToolTipShowVideo').value, event, 'imgHowToVideo');
            };

            document.getElementById("imgHowToVideo").onmouseout = function (event) {
                HideCallout('divCallout');
            };
            ShowHidePlayHowToVideoButton("Home", document.getElementById('DivHowToVideoLinks'));
            // Set up Sliders	
            SetSliders();

            di_jq(document).click(function (e) {
                // call function to hide opened divs
                HideDimentionsCallOutDiv(e);
            });

            equalHeight($(".column"));
        });	   	   	
    </script>
    <script type="text/javascript">
        di_jq(document).click(function (e) {
            hideDropDownDiv(e);
        });
    </script>
    <!-- END OF DEVELOPER CODE -->
</asp:content>