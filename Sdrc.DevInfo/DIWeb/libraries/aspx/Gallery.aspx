<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" EnableSessionState="true"
    CodeFile="Gallery.aspx.cs" Inherits="libraries_aspx_Gallery" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">

<style type="text/css">
    .clp_loadingajax {
    background: -moz-linear-gradient(center top , #474E56 0%, #1D232B 100%) repeat scroll 0 0 transparent;
    border-color: black;
    border-radius: 8px 8px 8px 8px;
    color: white !important;
    display: block;
    height: 40px;
    left: 45%;
    opacity: 0.85;
    padding-top: 12px;
    position: absolute;
    text-decoration: none;
    text-shadow: 0 1px 0 rgba(0, 0, 0, 0.75);
    top: 50%;
    width: 250px;
    z-index: 5000001 !important;
}
.Pagination {
	padding:3px;
	margin:3px;
	text-align: right;
}

.a {
	padding: 2px 5px 2px 5px;
	margin-right: 2px;
	border: 1px solid #ddd;
	
	text-decoration: none; 
	color: #aaa;
}
.a:hover, a:active {
	padding: 2px 5px 2px 5px;
	margin-right: 2px;
	border: 1px solid #a0a0a0;
}
.paginationCurrent {
	padding: 2px 5px 2px 5px;
	margin-right: 2px;
	border: 1px solid #e0e0e0;
	font-weight: bold;
	background-color: #f0f0f0;
	color: #aaa;
}


</style>
<!-- start for pop-up -->
<div id='PopupOuterBox' class="popup_block">
  <div id="popupContent"></div>
</div>
<div id='PopupMoreBox' class="popup_block">
  <div id="moreContent"></div>
</div>
<!-- end for pop-up -->

<!-- Main Container ...starts-->
<div class="content_containers">

<input type="hidden" id="langQdsIndCaption" value="" />
<input type="hidden" id="langQdsAreaCaption" value="" />
<input type="hidden" id="langQdsIndDefText" value="" />
<input type="hidden" id="langQdsAreaDefText" value="" />
<input type="hidden" id="langQdsSerchBtnText" value="" />
<input type="hidden" id="langQdsAreaBlockText" value="" />
<input type="hidden" id="delete_confirmation_txt" value="" />
<input type="hidden" id="delete_presentation_txt" value="" />
<input type="hidden" id="delete_right_txt" value="" />

<input type="hidden" id="langGalleryResults" value="" />
<input type="hidden" id="langGalleryTimeTaken" value="" />
<input type="hidden" id="langGallerySeconds" value="" />
<input type="hidden" id="langmoreAreas" value="" />
<input type="hidden" id="langmoreIndicators" value="" />
<input type="hidden" id="langmoreUDKs" value="" />
<input type="hidden" id="langGalleryMore" value="" />
<input type="hidden" id="langGalleryModifiedOn" value="" />
<input type="hidden" id="langGalleryDescription" value="" />
<input type="hidden" id="hPageSize" value="<%= hPageSize %>" />

	    
    <div id="reg_content_containers">
        <!--
        <h2>di Gallery</h2>
        <h5></h5>
        
        
        <div class="desc_pg_main_sec">
            <p>di Gallery allows you to explore the realm of possibilities within DevInfo by accessing many types of presentation objects: tables, graphs, maps, profiles, customized galleries, online books, videos, publications, and infographics.</p>
        </div>-->

            <!-- DBS & QDS Search Boxes Area ...starts-->
            <div id="dbs_qds_main">

                <!-- Database Search Section ...starts-->
                <!--<div id="dbs_cont" style="display:none;">
                <p class="heading1_dbs">Database</p>
                <div class="roundedcorners"><div id="db_details_div"></div></div>
                <div class="clear"></div>
                </div>-->
                <!-- Database Search Section...ends-->
         
                <!-- Quick Data Search Section ...starts-->
                <div id="qds_cont" class="flt_lft" style="width:45%;">
                <p id="dbinuse" class="heading1_qds"><span id="langQuickDataSearch"><!--Quick data search--></span></p>
                <div class="roundedcorners"><div id="quick_data_search_div"></div></div>
                <div class="clear"></div>
                <!--<p><a href="javascript:ShowDbComponent();" class="adv_search" id="langAdvancedSearch">Advanced Search</a></p>-->
                </div>
                <div class="heading1_qds flt_lft qds_keywrd_spc" id="langOR"><!-- Or--></div>
                <div id="galler_keywords" class="flt_lft">
                <p class="heading1_qds"><span id="langkeywords">Keywords</span></p>
                <div class="roundedcorners">
                    <div id="keywordArea">
                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
	                        <tbody>
		                        <tr>
			                        <td style="padding-right:1px;">
				                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
					                        <tbody>
						                        <tr>
							                        <td height="66px" class="di_qds_box_mid_strp brdr_bttm_n">
								                        <table width="100%" cellspacing="0" cellpadding="0" border="0">
									                        <tbody>
										                        <tr>
											                        <td style="width:100%;padding-right:3px;"><input type="text" onfocus="text_keyword_focus()" onblur="text_keyword_blur();" onkeyup="text_kewordPressout(event)" style="width:100%; border:none; font-weight:normal;  margin-left:2px; padding-left:2px; display:inline;" class="qds_textfield_default" id="keyword_txt"></td>
                                                                    <td>
<img id="img_keyword_cross" style="cursor: pointer; border: 0pt none; width: 16px; height: 16px; display:none;" onclick="ClearKeyword();" src="../../stock/themes/default/images/cross.gif">
</td></tr>
									                        </tbody>
								                        </table>
							                        </td>
						                        </tr>
					                        </tbody>
				                        </table>
			                        </td>
		                        <td width="42" valign="middle" class="di_qds_box_side_strp"> <a><img style="cursor:pointer; border:0;" onclick="JavaScript:return GetGalleryResultByKeywords();" src="../../stock/themes/default/images/search_button.gif"></a><div class="di_qds_search_txt"><a onclick="JavaScript:return GetGalleryResultByKeywords();" id="lngSearch"><!--Search--></a></div></td> </tr> </tbody>
                    </table></div></div>
                <div class="clear"></div>
                </div>
                <!-- Quick Data Search Section ...ends--> 
                
                    <div class="clear"></div>

            </div>
            <!-- DBS & QDS Search Boxes Area ...ends-->   
            
           <!-- Time Counter ...starts-->                       
            <div class="spanTimeCounter_pos">
            <i><span class="content" id="spanTimeCounter"></span></i>
            </div>
            <!-- Time Counter ...ends--> 
            
            <!-- Loader Animation ...starts-->
            <div id="glry_LoadingOuter">
                <div id="spanLoadingOuter" style="display:none;"><img src="../../stock/themes/default/images/loading_img.gif" height="16" width="16" /><i class="content" id="spanLoading"></i></div>
            </div>
            <!-- Loader Animation ...ends-->              
             
            
            <!-- Gallery Nav List ...starts-->
            <div id="tblQDSResults" style="display:none">
                <ul class="glry_nav_list">
                <li><a id="aGalleryAll" onclick="javascript:ApplyFilterFormat(this)" href="javascript:SearchGallery('A');"><!--All--></a></li>
                <li class="glry_nav_pipe">|</li>
                <li><a id="aGalleryTables" onclick="javascript:ApplyFilterFormat(this)"  href="javascript:SearchGallery('T');"><!--Tables--></a></li>
                <li class="glry_nav_pipe">|</li>
                <li><a id="aGalleryGraphs" onclick="javascript:ApplyFilterFormat(this)"  href="javascript:SearchGallery('G');"><!--Graphs--></a></li>
                <li class="glry_nav_pipe">|</li>
                <li><a id="aGalleryMaps" onclick="javascript:ApplyFilterFormat(this)"  href="javascript:SearchGallery('M');"><!--Maps--></a></li>
                <li class="glry_nav_pipe">|</li>
                <li><a href="javascript:void(0);"><input id="showtitlechkbox" type="checkbox" onclick="showTitle(this.checked)" checked="checked"/><label id="showTitleChk" for="showtitlechkbox" style="cursor:pointer;"><!--Show Title--></label></a></li>
                </ul>
                  <div class="Pagination" >
                <input id="btnFirstPage" onclick="FirstPage()" class="dbl_lft_arrw" value="" />
                <input  id="btnDecPage" onclick="DecrementPageNumber()" class="sngl_lft_arrw" value="" />
                <div id="divPaging" style="display:inline"></div>
                <input  id="btnIncPage"  onclick="IncrementPageNumber()" class="sngl_rgt_arrw" value="" />
                <input  id="btnLastPage"  onclick="LastPage()" class="dbl_rgt_arrw" value="" />
                </div>
            </div>
            <!-- Gallery Nav List ...ends-->
            <div class="clear"></div>
            <!-- For Thumbnail images ...starts-->
            <div id="divQDSResults" style="display:block;"></div>
            <!-- For Thumbnail images ...ends-->            
            <div class="clear"></div>
	    
        
        <!--</div>-->
          
           

    
            <!-- Call Out Tool Tip ...starts-->
            <div id="divCallout" class="callout border-callout">
                <div id="divCalloutText" class="content"><!--Callout text--></div>
                <b class="border-notch notch"></b><b class="notch"></b>
            </div>  
            <!-- Call Out Tool Tip ...ends-->    
            
            <!-- start for pop-up -->    
            
            <div id="popup_db_details" class="popup_block">
                <div id="database_div"></div>
            </div>
            
            <!-- end for pop-up -->    
            <input id="lang_Download_PNG" value="" type="hidden" />
            <input id="lang_Download_SVG" value="" type="hidden" />
            <input id="lang_Download_XLS" value="" type="hidden" />
            <input id="lang_Download_Image" value="" type="hidden" />
            <input id="lang_Download_KML" value="" type="hidden" />            
            <input id="lang_ppup_Next" value="" type="hidden" />
            <input id="lang_ppup_Prev" value="" type="hidden" />
            <input id="GalView" value="" type="hidden" />
            <input id="GalDownload" value="" type="hidden" />
            <input id="GalShare" value="" type="hidden" />
            <input id="GalDelete" value="" type="hidden" />

        <div id="divShowLoading" class="clp_loadingajax" style="display: none;">
        <div id="divShowLoadingText" class="clp_loadingajax_text">Loading Visualization...</div>
        </div>

    </div>
<!-- Main Container ...ends--> 
</div>
    <!-- DEVELOPER CODE -->

    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app
        
        var di_components = "Qds,Language,Database";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';        
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
       
    </script>

    <script type="text/javascript" src="../js/NGallery.js?v=<%=Global.js_version%>"></script>

    <script type="text/javascript">onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');</script>
    <script type="text/javascript">
        di_jq(document).click(function (e) {
            hideDropDownDiv(e);
        });
        </script>
    <!-- END OF DEVELOPER CODE -->

	   
</asp:Content>
