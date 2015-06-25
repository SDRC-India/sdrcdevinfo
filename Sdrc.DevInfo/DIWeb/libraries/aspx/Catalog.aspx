<%@ Page Title="" Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" AutoEventWireup="true" CodeFile="Catalog.aspx.cs" Inherits="libraries_aspx_Catalog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
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

<input type="hidden" id="langResultsTimePeriod" value="" />
<input type="hidden" id="langResultsDimensions" value="" />
<input type="hidden" id="langResultsDataValues" value="" />
<input type="hidden" id="langResultsCloudView" value="" />
<input type="hidden" id="langResultsAddToCart" value="" />
<input type="hidden" id="langResultsVisualize" value="" />
<input type="hidden" id="langResultsShare" value="" />

<input type="hidden" id="langNumberOf" value="" />
<input type="hidden" id="langGeographicalAreas" value="" />
<input type="hidden" id="langVariables" value="" />
<input type="hidden" id="langTimePeriod" value="" />
<input type="hidden" id="langData" value="" />
<input type="hidden" id="langLastUpdated" value="" />
<input type="hidden" id="langKeywords" value="" />
<input type="hidden" id="langPreviewData" value="" />
<input type="hidden" id="langHideData" value="" />
<input type="hidden" id="langGotoData" value="" /> 

<input type="hidden" id="langDBLanguage" value="" /> 
<input type="hidden" id="langCountryName" value="" />
<input type="hidden" id="langRegionName" value="" />
<input type="hidden" id="langAdaptationYear" value="" /> 
<input type="hidden" id="LangDbAdminContact" value="" /> 
<input type="hidden" id="LangEmail" value="" /> 

<input type="hidden" id="langMin" value="" />
<input type="hidden" id="langMax" value="" />
<input type="hidden" id="langQDSResults" value="" />
<input type="hidden" id="langQDSTimeTaken" value="" />
<input type="hidden" id="langQDSSeconds" value="" />
<input type="hidden" id="NoRecordsFound" value="" />
<input type="hidden" id="langMRDFor" value="" />
<input type="hidden" id="langSearchingDatabase" value="" />
<input type="hidden" id="langLoadingResults" value="" />
<input type="hidden" id="show_cloud" value="<%= hShowCloud %>" />
<input type="hidden" id="langDownloadCSVFile" value="" />
<input type="hidden" id="hGotoGlobalCatalog" value="" />


 <div class="content_containers">
 
     <div id="dbs_qds_main">
     
         <!-- Quick Data Search Section ...starts-->
         <div id="qds_cont" style="width: 100%;">
            <p class="heading1_qds" id="langQuickDataSearch">
                 <!--Quick data search-->
            </p>
            <div class="roundedcorners_thin" style="margin-top:10px;">
                 <div id="quick_data_search_div">
                 </div>                 
            </div>    
            <br />         
            <!--<span id="spnFilteredText" class="qds_box_blw_txt"><!--Search through multiple databases that you have filtered below</span>-->   
            <p id="lang_box_blw_txt_one" class="qds_box_blw_txt"><!--The Catalog section allows users to scroll through the various DevInfo adaptations either by location or by DevInfo version.--></p>   
            <p id="lang_box_blw_txt_two" class="qds_box_blw_txt"><!--Basic information for each adaptation such as the number of geographical areas, time period, and contact information will be provided (for the latter, click on the “i” next to the adaptation name).--></p> 
            <p id="lang_box_blw_txt_thr" class="qds_box_blw_txt"><!--If an adaptation is available online, users will be able to access them by clicking directly on the adaptation name.--></p>                               
             <div class="clear">
             </div>
          <%--   <p>
                 <a href="javascript:void(0)" class="adv_search" id="langAdvancedSearch" onclick="doAdvanceSelection()">
                     <!--Advanced Search-->
                 </a>
             </p>--%>
         </div>
         <!-- Quick Data Search Section ...ends-->
         
         <div class="clear adv_src_dvdr_spc">
         </div>         
     </div>
     <i><span class="content" id="spanTimeCounter" style="float: left; display: none;"></span></i>
     <%--<div class="spanTimeCounter_pos">
         <i><span class="content" id="spanTimeCounter" style="float: left;"></span></i><span
             id="spanAddAllToCart" onclick="AddAllToCart();" class="content qds_icon2" style="display: none;
             float: left; margin-left: 50px; padding-left: 25px; width: 100px;">Add all to cart</span>
         <span id="spanRemoveAllFromCart" onclick="RemoveAllFromCart();" class="content qds_icon7"
             style="display: none; float: left; margin-left: 10px; padding-left: 25px; width: 150px;">
             Remove all from cart</span>
     </div>
     <br />--%>          
     
     <div id="glry_LoadingOuter" style="display: none;">
         <div id="spanLoadingOuter" style="display: none;">
             <img src="../../stock/themes/default/images/loading_img.gif" height="16" width="16"
                 alt="" /><i class="content" id="spanLoading"></i></div>
     </div>


     <div id="reg_content_containers">
         <h2 id="spanPageTitle"><!--Catalog--></h2>
         <h5>
             <span id="pPageDescription"><!--View global and country catalog--></span>
             <span style="float: right;">
                <span id="langCount"><%--Count: --%></span>
                <span id="spanAdaptationsCount">0</span>
            </span>
         </h5>
         
     </div>
    
    <div id="divCatalogHTML">
        <%=CatalogHtml%>
    </div>

     <div id="divCloudOuter" class="popup_block">
         <div id="divCloudHeader">
         </div>
         <div id="divCloud">
         </div>
     </div>

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

</div>            

    <!-- DEVELOPER CODE -->

    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app

        var di_components = "Search,Qds,DIVC,Language";
        var di_vctype = "Column,Line";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    </script>

    <script type="text/javascript" src="../js/QDS.js?v=<%=Global.js_version%>"></script>
    <script type="text/javascript" src="../js/Catalog.js?v=<%=Global.js_version%>"></script>

    <script type="text/javascript">
        onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>', '<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>');

        di_jq(document).ready(function () {
            SetCatalogPageClass(); 
            di_jq(document).click(function (e) {
                // call function to hide opened divs
                HideAdapCallOutDiv(e);
            });

            ShowQDSComponentAndCatalogs();            
        });

    </script>

    <!-- END OF DEVELOPER CODE -->

</asp:Content>

