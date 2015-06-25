<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" EnableSessionState="true"
    CodeFile="NGallery.aspx.cs" Inherits="libraries_aspx_NGallery" Title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" runat="Server">

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
    
            <!-- DBS & QDS Search Boxes Area ...starts-->
            <div id="dbs_qds_main">

                <!-- Database Search Section ...starts-->
                <div id="dbs_cont" style="display:none;">
                <p class="heading1_dbs">Database</p>
                <div class="roundedcorners"><div id="db_details_div"></div></div>
                <div class="clear"></div>
                </div>
                <!-- Database Search Section...ends-->
         
                <!-- Quick Data Search Section ...starts-->
                <div id="qds_cont">
                <p id="dbinuse" class="heading1_qds"><span id="langQuickDataSearch"><!--Quick data search--></span></p>
                <div class="roundedcorners"><div id="quick_data_search_div"></div></div>
                <div class="clear"></div>
                <!--<p><a href="javascript:ShowDbComponent();" class="adv_search" id="langAdvancedSearch">Advanced Search</a></p>-->
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
                <li><a id="aGalleryAll" href="javascript:SearchGallery('A');"><!--All--></a></li>
                <li class="glry_nav_pipe">|</li>
                <li><a id="aGalleryTables" href="javascript:SearchGallery('T');"><!--Tables--></a></li>
                <li class="glry_nav_pipe">|</li>
                <li><a id="aGalleryGraphs" href="javascript:SearchGallery('G');"><!--Graphs--></a></li>
                <li class="glry_nav_pipe">|</li> 
                <li><a id="aGalleryMaps" href="javascript:SearchGallery('M');"><!--Maps--></a></li>
                <li class="glry_nav_pipe">|</li>
                <li><a href="javascript:void(0);"><input type="checkbox" onclick="showTitle(this.checked)" checked="checked"/><span id="showTitleChk"><!--Show Title--></span></a></li>
                </ul>
            </div>      
            <!-- Gallery Nav List ...ends-->             
            <div class="clear"></div>
            <!-- For Thumbnail images ...starts-->            
            <div id="divQDSResults"></div>
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

    <!-- END OF DEVELOPER CODE -->
</asp:Content>
