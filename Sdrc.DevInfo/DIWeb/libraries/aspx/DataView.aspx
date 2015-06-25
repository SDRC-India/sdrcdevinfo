<%@ Page Language="C#" MasterPageFile="~/libraries/aspx/HomeMaster.master" 
AutoEventWireup="true" CodeFile="DataView.aspx.cs" 
Inherits="libraries_aspx_DataView" Title="Data View" EnableSessionState="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="cphHeadContent" Runat="Server">
    <link href="../../stock/themes/default/spryassets/sprycollapsiblepanel.css" rel="stylesheet" type="text/css" />
<link href="../../stock/themes/default/css/style_dataview.css" rel="stylesheet" type="text/css" />
<link href="../../stock/themes/default/css/jquery-ui.css" rel="stylesheet" type="text/css" />
<script type="text/javascript">
    var featureList = ["tab", "panelbar", "dialog"];
</script>
<script type="text/javascript" src="<%=Global.diuilib_url%>/<%=Global.diuilib_version%>/js/oatgrid/loader.js"></script>
<script src="../../stock/themes/default/spryassets/sprycollapsiblepanel.js?v=<%=Global.js_version%>" type="text/javascript"></script>
<!--<script type="text/javascript" src="../js/di.jquery-1.4.4.js"></script>-->
<script type="text/javascript" src="../js/DataView1.js?v=<%=Global.js_version%>"></script>
<script type="text/javascript" src="../js/visualizer1.js?v=<%=Global.js_version%>"></script>

<script type="text/javascript" src="../js/jsScrollbar.js?v=<%=Global.js_version%>"></script>
<script type="text/javascript" src="../js/jsScroller.js?v=<%=Global.js_version%>"></script>
<!-- for google map -->
<link  href="http://code.google.com/apis/maps/documentation/javascript/examples/standard.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
<script type="text/javascript" src="https://www.google.com/jsapi?key=<%=hgoogleapikey%>"></script>

   <%-- <script src="../SvgTopng/jquery-latest.js"></script>
    <script type="text/javascript" src="../SvgTopng/saveSvgAsPng.js"></script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphMainContent" Runat="Server">
    <%--<div id="divFiltersMask" onclick="showFiltersMask()">
        <ul id="ulFilterMask" class="example_menu">
            <li><span onclick="showHideComponent('area')">More... </span> </li>
            <li><span onclick="showHideComponent('ind')">More... </span> </li>
        </ul>
    </div>--%>

<div id="divShowLoading" class="clp_loadingajax">
	<div id="divShowLoadingText" class="clp_loadingajax_text"><!--Loading Data...--></div>
</div>

<!-- for callout -->
<div id="divCallout" class="callout border-callout callout_main_cont">
        <div id="divCalloutText" class="content"><!--Callout text--></div>
        <b class="border-notch notch"></b>
        <b class="notch"></b>
</div>

<!--[if IE]><script language="javascript" type="text/javascript" src="../js/excanvas.js"></script><![endif]-->
<script type="text/javascript">
    // create pivoting object
    var DEMO = {};
    window.cal = false;
    DEMO.pivot = { panel: 1, tab: 0, div: "pivot", needs: ["pivot"], cb: CallBack }

    // initiation function for the pivoting
    function init() {

        /* tabs */
        var tab = new OAT.Tab("content");
        for (var p in DEMO) {
            var name = DEMO[p].div;
            tab.add("tab_" + name, name);
        }

        /* panelbar_content */
        var pb = new OAT.Panelbar("panelbar", 10);
        pb.addPanel("pb_1", "pb_11");

        /* create dimmer element */
        var dimmerElm = OAT.Dom.create("div", { border: "2px solid #000", padding: "1em", position: "absolute", backgroundColor: "#fff" });
        document.body.appendChild(dimmerElm);
        OAT.Dom.hide(dimmerElm);

        for (var p in DEMO) { DEMO[p].drawn = false; }
        tab.options.goCallback = function (oldIndex, newIndex) {
            var oldName, newName;
            for (var p in DEMO) {
                var v = DEMO[p];
                if (v.tab == oldIndex) { oldName = p; }
                if (v.tab == newIndex) { newName = p; }
            }


            var obj = DEMO[newName];
            //OAT.Dimmer.show(dimmerElm);
            OAT.Dom.center(dimmerElm, 1, 1);
            var ref = function () {
                if (!window.location.href.match(/:source/)) {
                    //OAT.Dimmer.hide();
                    ResetFilters();
                }
            }
            OAT.Loader.loadFeatures(obj.needs, ref);
            //alert('after loadfeatures');
        }
        pb.go(0);
        tab.go(0);
    }

</script>   

<!-- ***************** - DATA VIEW -- DataView JS Files Ends- ***************** -->


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
<input type="hidden" id="langAreaText" value="" /> 
<input type="hidden" id="langIndicatorText" value="" />
<input type="hidden" id="langTimePeriodText" value="" /> 
<input type="hidden" id="lngGoToParent" value="" />
<input type="hidden" id="lngSelectall" value="" />
<input type="hidden" id="lngClearall" value="" /> 
<input type="hidden" id="lngMore" value="" />
<input type="hidden" id="lngSorting" value="" />
<input type="hidden" id="lngHideCol" value="" />
<input type="hidden" id="lngMRD" value="" />

<input type="hidden" id="lngLineChartHelpText" value="" />
<input type="hidden" id="langDVTotalRecords" value="" />
<input type="hidden" id="langDVSource" value="" />
<input type="hidden" id="langDVAreaID" value="" />
<input type="hidden" id="langSavingGallery" value="" />
<input type="hidden" id="langLoadingGallery" value="" />
<input type="hidden" id="langLoadingVisualization" value="" />
<input type="hidden" id="langLoadingData" value="" />

<input type="hidden" id="LineChartDisabled" value="" />
<input type="hidden" id="ColumnChartDisabled" value="" />
<input type="hidden" id="BarChartDisabled" value="" />
<input type="hidden" id="AreaChartDisabled" value="" />
<input type="hidden" id="PieChartDisabled" value="" />
<input type="hidden" id="MapChartDisabled" value="" />
<input type="hidden" id="PyramidChartDisabled" value="" />
<input type="hidden" id="TreeMapChartDisabled" value="" />
<input type="hidden" id="RadarChartDisabled" value="" />
<input type="hidden" id="ScatterChartDisabled" value="" />
<input type="hidden" id="DefaultDisabled" value="" />
<input type="hidden" id="langPleaseSelect" value="" />
<input type="hidden" id="langNDFFCS" value="" />
<input type="hidden" id="langSWWS" value="" />
<input type="hidden" id="langTopic" value="" />
<input type="hidden" id="langArea" value="" />

<!--REGION DECLARING HIDDEN INPUT FIELDS FOR LANGUAGE HANDLING OF SETTING TOOLTIP-->
<input  type="hidden" id="ToolTipimgShowHideFiltersPanel" value="" />
<input  type="hidden" id="ToolTipBold" value="" />
<input  type="hidden" id="ToolTipItalic" value="" />
<input  type="hidden" id="ToolTipUnderline" value="" />
<input  type="hidden" id="ToolTipIncreaseFontSize" value="" />
<input  type="hidden" id="ToolTipDecreaseFontSize" value="" />
<input  type="hidden" id="ToolTipFontColor" value="" />
<input  type="hidden" id="ToolTipFillColor" value="" />
<input  type="hidden" id="ToolTipBarsBorderColor" value="" />
<input  type="hidden" id="ToolTipBarBorderLineWidth" value="" />
<input  type="hidden" id="ToolTipDashType" value="" />
<input  type="hidden" id="ToolTipNoBorderIcon" value="" />
<input  type="hidden" id="ToolTipFillColor1" value="" />
<input  type="hidden" id="ToolTipChangeSeriesColor" value="" />
<input  type="hidden" id="ToolTipChangeThemeColor" value="" />
<input  type="hidden" id="ToolTipCustomTextDirection45Text" value="" />
<input  type="hidden" id="ToolTipCustomTextDirectionMinus45Text" value="" />
<input  type="hidden" id="ToolTipCustomTextDirection90Text" value="" />
<input  type="hidden" id="ToolTipCustomTextDirectionMinus90Text" value="" />
<input  type="hidden" id="ToolTipCustomTextHorizontal" value="" />
<input  type="hidden" id="ToolTipHideSettingBar" value="" />
<input  type="hidden" id="ToolTipShowSettingBar" value="" />
<input  type="hidden" id="ToolTipHideFilterpanel" value="" />
<input  type="hidden" id="ToolTipShowFilterPanel" value="" />
<input  type="hidden" id="ToolTipSortAtoZ" value="" />
<input  type="hidden" id="ToolTipSortZtoA" value="" />
<input  type="hidden" id="ToolTipExpandGeneralSetting" value="" />
<input  type="hidden" id="ToolTipExpandFormatSetting" value="" />
<input  type="hidden" id="ToolTipExpandSeriesSetting" value="" />


<!--END REGION-->

<!--END REGION-->


<div class="dv_main_contnr">
<div class="tristate_dropdown_contnr">   
    
    <!-- ***************** - DATA VIEW -- DataView Navigation Starts- ***************** -->
    <div class="dv_tab_panel_wrapr">
    
    <!-- The Tabs (UL LI)Starts Here -->
    <!--<div class="toggle_arrow" onclick="toggleVisPanel('up')"><a href="#"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="5" border="0" /></a></div>
	<div class="toggle_arrow_down" onclick="toggleVisPanel('down')" style="display:none;"><a href="#"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="5" border="0" /></a></div>-->

	<div class="fullViewContainer">
    <div id="DivHowToVideoLinks"style="float:left; display:none;" >
          <span id="SpanHowToVideoImg" onclick="PlayHowToVideoPopUp('true','visulization')" ><img id="imgHowToVideo"  src="../../stock/themes/default/images/play-icon-Dv.png" onclick="PlayHowToVideoPopUp('true','visulization');" class="Img-How-toVideo-Dv"></span>
     <span class="Icon_1" id="SpanHowToVideoText" ><a id="lang_SpanHowToVideo" href="javascript:void(0);"  onclick="PlayHowToVideoPopUp('true','visulization')" style="padding-left:10px;">How to Video</a></span>
     </div>
     <div style="float:left;">
	 <span id="saveGallerySpan" class="fullViewIcon fullViewIcon_1" onclick="openGalleryInputPop()"></span>
     <span class="Icon_1"><a id="langSaveAs" href="javascript:void(0);" onclick="openGalleryInputPop()">Save As</a></span>
	 <!--<span class="fullViewIcon fullViewIcon_2"></span>-->     
	 <span class="fullViewIcon fullViewIcon_3" onclick="toggleDownload()" id="clickDownload"></span>
     <span style="cursor: pointer;" onclick="toggleDownload()" id="clickDownloadLabel" class="Icon_1 fnt_sz_twlv">Download</span>     
	 <span id="shareSpan" class="fullViewIcon fullViewIcon_4" onclick="callChartProperty('share', '')"></span>
     <span class="Icon_1"><a id="langShare" href="javascript:void(0);"   onclick="callChartProperty('share', '')">Share</a></span>
	 <!--<span class="fullViewIcon fullViewIcon_5"></span>-->
	 <span id="toggle_arrow" rel='TTHSBar' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');" class="fullViewIcon fullViewIcon_6" onclick="toggleVisPanel('')" rel1="up"></span>
     </div>
	</div>
    
    <!-- ***************** - DATA VIEW -- Pivot,Arrange,Swap Option Starts- ***************** -->
    <!--<div class="pivotContainer">
        <span class="pivotIcon1"><input name="chkTogglePivot" id="chkTogglePivot" type="checkbox" onclick="HandlePivotClick(this);" value="" /></span>
        <span class="Icon_1"><label for="chkTogglePivot"><a id="langArrange" href="javascript:void(0);" onclick="HandlePivotClick(this);" ><%--Arrange--%></a></label></span>
        <span class="fullViewIcon pivotIcon2" onclick="PivotRowColHeaders()"></span>
        <span class="Icon_1"><a href="javascript:void(0);" id="langPivot" onclick="PivotRowColHeaders()"><%--Pivot--%></a></span>
        <span class="fullViewIcon pivotIcon3" onclick="swapRowColHeaders(this);"></span>
        <span class="Icon_1"><a href="javascript:void(0);" id="langSwap" onclick="swapRowColHeaders(this);"><%--Swap--%></a></span>
    
    </div>-->
    <!-- ***************** - DATA VIEW -- Pivot,Arrange,Swap Option Ends- ***************** -->
    
    
    	<div id="toggleDownload" class="fullViewContainer2 fullViewContainer_expnd_box" onblur="toggleDownload()">
		<!-- Start links for Chart -->
		<!--<p id="vcLangLblDlPDF"><a href="javascript:void(0);" onclick="callChartProperty('export', 'application/vnd.xls')" id="lang_Download_PDF"><!--Download as PDF</a></p>-->
		<p id="vcLangLblDlPNG"><a href="javascript:void(0);" onclick="callChartProperty('export', 'image/png')" id="lang_Download_PNG"><!--Download as PNG--></a></p>
		<p id="vcLangLblDlSVG"><a href="javascript:void(0);" onclick="callChartProperty('export', 'image/svg+xml')" id="lang_Download_SVG"><!--Download as SVG--></a></p>
		<p id="vcLangLblDlXLS"><a href="javascript:void(0);" onclick="callChartProperty('export', 'application/vnd.xls')" id="lang_Download_XLS"><!--Download as XLS--></a></p>
		
		<!-- Start links for Table -->
		<p id="tblLangLblDlXLS"><a href="javascript:void(0);" onclick="ExportOATtoExcel()" id="lang_tbl_Download_XLS"><!--Download as XLS--></a></p>

		<!-- Start links for MAP -->
		<p id="mapLangLblDlPNG"><a href="javascript:void(0);" onclick="mapImageOptions(true)" id="lang_Download_Image"><!--Download as Image--></a></p>
		<p id="mapLangLblDlXLS"><a href="javascript:void(0);" onclick="mapDownload('excel')" id="lang_map_Download_XLS"><!--Download as XLS--></a></p>
		<p id="mapLangLblDlKML"><a href="javascript:void(0);" onclick="mapDownload('kmz')" id="lang_Download_KML"><!--Download as KML--></a></p>
		<!-- End links for MAP -->
	</div>
	<ul class="blue">
	<!--<li><a id="tab-1t" class="" href="javascript:void(0);" onclick="toggleVisTab('tab-1')">File</a></li>-->
	<li><a id="tab-2t" class="current" href="javascript:void(0);" onclick="toggleVisTab('tab-2')"><span id="lang_Visualization"><!--Visualization--></span></a></li>
	<li id="setting_tab"><a id="tab-3t" class="" href="javascript:void(0);" onclick="toggleVisTab('tab-3')"><span id="lang_Settings"><!--Settings--></span></a></li>
	<!--<li><a id="tab-4t" class="" href="javascript:void(0);" onclick="toggleVisTab('tab-4')">Map</a></li>-->
    <!--<li><a id="tab-4t" class="" href="javascript:void(0);" onclick="toggleVisTab('tab-4')">Options</a></li>-->
    <!--<li><a id="tab-5t" class="" href="javascript:void(0);" onclick="toggleVisTab('tab-5')">View & Sort</a></li>-->
	</ul>
    
    <!-- The Tabs (UL LI)Ends Here -->
    
    <div class="panes">
    
    <!-- The Tab "File" Data Starts Here--> 
	<!--<div id="tab-1" class="panesInner">
    	<span id="file_section1">
        <span class="file_section1">
		 <table width="98%" border="0" align="left" cellpadding="0" cellspacing="2">
		  <tr>
			<td width="28%"><p id="vcLangLblGallName">Gallery Name</p></td>
			<td width="72%"><select name="" class="vcInputSelBox" style="width:225px;"><option>Select Gallery Name</option></select></td>
		  </tr>
		  <tr>
			<td width="28%"><p id="vcLangLblFile">File</p></td>
			<td width="72%"><input name="" class="vcInputTxtBox" style="width:222px;" type="text" /></td>
		  </tr>
		  <tr>
			<td>&nbsp;</td>
			<td><input type="button" value="Open" onclick="" class="dv_field_button"> <input type="button" value="Save" onclick="" class="dv_field_button"> <input type="button" value="Save as" onclick="" class="dv_field_button"> &nbsp;&nbsp;&nbsp;<input id="flPrintVC" type="button" value="Print" onclick="changeVisulizerChart('print', '')" class="dv_field_button"><input id="flPrintOAT" type="button" value="Print" onclick="PrintOAT()" class="dv_field_button" style="display:none;"></td>
		  </tr>
		 </table>
		</span>
        <span class="file_section1_bottom" id="vcLangLblSvPnt">Save / Print</span>
        <span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>
        </span>
    	<span id="file_section2">
        <span class="file_section1">
		  <table width="98%" border="0" align="left" cellpadding="0" cellspacing="2">
		  <tr>
			<td width="28%"><p id="vcLangLblFile2">File</p></td>
			<td width="72%"><select name="" class="vcInputSelBox" style="width:225px;"><option></option></select></td>
		  </tr>
		  <tr>
			<td>&nbsp;</td>
			<td><input type="button" value="Open" onclick="" class="dv_field_button"> <input type="button" value="Save" onclick="" class="dv_field_button"> <input type="button" value="Save as" onclick="" class="dv_field_button"></td>
		  </tr>
		 </table>
		</span>
        <span class="file_section1_bottom" id="vcLangLblDesign">Design</span>
        <span class="corner_diable"><a href="#"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></a></span>
        </span>
    	<span id="file_section3">
        <span class="file_section1">
		 <table width="98%" border="0" align="left" cellpadding="0" cellspacing="2">
		  <tr>
			<td width="60%" height="20px"><p id="vcLangLblDlPDF"><img src="../../stock/themes/default/images/dataview/download_icon.png">&nbsp <a href="javascript:void(0);" onclick="callChartProperty('export', 'application/pdf')">Download as PDF</a></p><p id="vcLangLblDlXLS" style="display:none;"><img src="../../stock/themes/default/images/dataview/download_icon.png">&nbsp <a href="javascript:void(0);" onclick="ExcelOAT()">Download as XLS</a></p></td>
			<td width="40%"><p id="vcLangLblDlShare"><img src="../../stock/themes/default/images/dataview/share.png">&nbsp <a href="javascript:void(0);" onclick="callChartProperty('share', '')">Share</a></p></td>
		  </tr>
		  <tr>
			<td width="60%" height="20px"><p id="vcLangLblDlPNG"><img src="../../stock/themes/default/images/dataview/download_icon.png">&nbsp; <a href="javascript:void(0);" onclick="callChartProperty('export', 'image/png')">Download as PNG</a></p></td>
			<td width="40%"></td>
		  </tr>
		  <tr>
			<td width="60%" height="20px"><p id="vcLangLblDlSVG"><img src="../../stock/themes/default/images/dataview/download_icon.png">&nbsp; <a href="javascript:void(0);" onclick="callChartProperty('export', 'image/svg+xml')">Download as SVG</a></p></td>
			<td width="40%"></td>
		  </tr>
		 </table>
		</span>
        <span class="file_section1_bottom" id="vcLangLblExpShare">Export / Share</span>
        <span class="corner_diable"><a href="#"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></a></span>
        </span>
    </div> -->
    <!-- The Tab "File" Data Ends Here-->  
     
    <!-- The Tab "Visualization" Data Starts Here--> 
	<div id="tab-2" class="panesInner2" style="display: block;">
    	<span id="visul_section1">
        <span id="atable" class="visul_section1"><span id="replace1" class="visual_icon-1-1" rel='table' onclick="changeVisulizerChart(this)"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span></span>
        <span class="visul_section1_bottom" onmouseover="showVisualizationHeading(this)" onmouseout="HideCallout('divCallout');" id="lang_Table"><!--Table--></span>
        <!--<span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>-->
        </span>
        
        <!-- ICON TWO Starts HERE-->
        <span id="visul_section2">
			<span id="aline" class="visul_section1"><span id="replace2" rel='line' class="visual_icon-2-2" onmouseover="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmouseout="HideCallout('divCallout');" onclick="changeVisulizerChart(this)"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span>
			<span id="span_visual_icon_tristate2" class="visual_icon_tristate" rel="line"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="10" border="0" /></span>
			<!-- -->
			<span class="tristate_dropdown_contnr">
				<div id="visual_icon_tristate_line" class="tristate_dropdown">
					<div class="tristate_icon_line0"><a href="#" rel="line" onclick="changeVisulizerChart(this)" title="" id="DVLine"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
					<div class="tristate_icon_line1"><a href="#" rel="stackline" onclick="changeVisulizerChart(this)" title="" id="DVStackLine"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
					<div class="tristate_icon_line2"><a href="#" rel="100stackline" onclick="changeVisulizerChart(this)" title="" id="DVStackLine100"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
					<div class="drop_tristate_bg"><span class="visul_section1_bottom visul_section1_t_spc" id="lang_Line"><!--Line--></span></div>
				</div>
        
			</span>
			<!-- -->
        </span>

		<span class="visul_section1_bottom" onmouseover="showVisualizationHeading(this)" onmouseout="HideCallout('divCallout');" id="lang_Line_di"><!--Line--></span>
        <!--<span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>-->
        </span>
        
        <!-- ICON TWO Ends HERE-->
        
        <!-- ICON THREE Starts HERE-->
        <span id="visul_section3">
        <span id="acolumn" class="visul_section1"><span id="replace3" class="visual_icon-3-3" rel='column' onmouseover="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmouseout="HideCallout('divCallout');" onclick="changeVisulizerChart(this)"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span>
        
        <span id="span_visual_icon_tristate3" class="visual_icon_tristate" rel="column"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="10" border="0" /></span>
		<!-- -->
		<span class="tristate_dropdown_contnr">
		<div id="visual_icon_tristate_column" class="tristate_dropdown">
        	<div class="tristate_icon_column0"><a href="#" rel="column" onclick="changeVisulizerChart(this)" title="" id="DVColumn"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
			<div class="tristate_icon_column1"><a href="#" rel="stackcolumn" onclick="changeVisulizerChart(this)" title="" id="DVStackColumn"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
            <div class="tristate_icon_column2"><a href="#" rel="100stackcolumn" onclick="changeVisulizerChart(this)" title="" id="DVStackColumn100"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
            <div class="drop_tristate_bg"><span class="visul_section1_bottom visul_section1_t_spc" id="lang_Column"><!--Column--></span></div>
        </div>
        
		</span>
		<!-- -->
        
        </span>
        <span class="visul_section1_bottom" onmouseover="showVisualizationHeading(this)" onmouseout="HideCallout('divCallout');" id="lang_Column_di"><!--Column--></span>
        <!--<span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>-->
        </span>        
        <!-- ICON THREE Ends HERE-->
        
        <!-- ICON FOUR Starts HERE-->
        <span id="visul_section4">
        <span id="abar" class="visul_section1"><span id="replace4" class="visual_icon-4-4" rel='bar' onmouseover="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmouseout="HideCallout('divCallout');" onclick="changeVisulizerChart(this)"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span>
        
        <span id="span_visual_icon_tristate4" class="visual_icon_tristate" rel="bar"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="10" border="0" /></span>
		<!-- -->
		<span class="tristate_dropdown_contnr">
		<div id="visual_icon_tristate_bar" class="tristate_dropdown">
        	<div class="tristate_icon_bar0"><a href="#" rel="bar" onclick="changeVisulizerChart(this)" title="" id="DVBar"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
			<div class="tristate_icon_bar1"><a href="#" rel="stackbar" onclick="changeVisulizerChart(this)" title="" id="DVStackBar"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
            <div class="tristate_icon_bar2"><a href="#" rel="100stackbar" onclick="changeVisulizerChart(this)" title="" id="DVStackBar100"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
            <div class="drop_tristate_bg"><span class="visul_section1_bottom visul_section1_t_spc" id="lang_Bar"><!--Bar--></span></div>
        </div>
        
		</span>
		<!-- -->
        
        </span>
        <span class="visul_section1_bottom" onmouseover="showVisualizationHeading(this)" onmouseout="HideCallout('divCallout');" id="lang_Bar_di"><!--Bar--></span>
        <!--<span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>-->
        </span>
        <!-- ICON FOUR Ends HERE-->
        
        <!-- ICON FIVE Starts HERE-->
        <span id="visul_section5">
        <span id="aarea" class="visul_section1"><span id="replace5" class="visual_icon-5-5" rel='area' onmouseover="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmouseout="HideCallout('divCallout');" onclick="changeVisulizerChart(this)"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span>
        
        <span id="span_visual_icon_tristate5" class="visual_icon_tristate" rel="area"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="10" border="0" /></span>
		<!-- -->
		<span class="tristate_dropdown_contnr">
		<div id="visual_icon_tristate_area" class="tristate_dropdown">
        	<div class="tristate_icon_area0"><a href="#" rel="area" onclick="changeVisulizerChart(this)" title="" id="DVArea"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
			<div class="tristate_icon_area1"><a href="#" rel="stackarea" onclick="changeVisulizerChart(this)" title="" id="DVStackArea"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
            <div class="tristate_icon_area2"><a href="#" rel="100stackarea" onclick="changeVisulizerChart(this)" title="" id="DVStackArea100"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
            <div class="drop_tristate_bg"><span class="visul_section1_bottom visul_section1_t_spc" id="lang_Area"><!--Area--></span></div>
        </div>
        
		</span>
		<!-- -->
        
        </span>
        <span class="visul_section1_bottom" onmouseover="showVisualizationHeading(this)" onmouseout="HideCallout('divCallout');" id="lang_Area_di"><!--Area--></span>
        <!--<span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>-->
        </span>
        <!-- ICON FIVE Ends HERE-->
        
        <span id="visul_section6">
        <span id="apie" class="visul_section1"><span id="replace6" class="visual_icon-6-6" rel='pie' onmouseover="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmouseout="HideCallout('divCallout');" onclick="changeVisulizerChart(this)"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span></span>
        <span class="visul_section1_bottom" onmouseover="showVisualizationHeading(this)" onmouseout="HideCallout('divCallout');" id="lang_Pie"><!--Pie--></span>
        <!--<span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>-->
        </span>
        
        <!--<span id="visul_section8">
        <span class="visul_section1"><span id="replace8" class="visual_icon-8-8" rel="radar" onclick="changeVisulizerChart(this)"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span></span>
        <span class="visul_section1_bottom">Radar</span>
        <span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>
        </span>-->
        
        <span id="visul_section7">
			<div id="visul_section_tristate_map"></div>
			<span id="amap" class="visul_section1">
				<span id="replace7" class="visual_icon-7-7" rel='map' onmouseover="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmouseout="HideCallout('divCallout');" onclick="changeVisulizerChart(this)"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span>
		
				<span id="span_visual_icon_tristate7" class="visual_icon_tristate" rel="map"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="10" border="0" /></span>
				<!-- -->
				<span class="tristate_dropdown_contnr">
					<div id="visual_icon_tristate_map" class="tristate_dropdown" style="height:156px;">
						<div class="tristate_icon_map0" title="" id="DVSingleTheme"><a href="javascript:void(0);" rel="map" onclick="changeVisulizerChart(this)"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
						<div class="tristate_icon_map3" title="" id="DVMultipleTheme"><a href="javascript:void(0);" rel="mapm" onclick="changeVisulizerChart(this)"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
						<div class="tristate_icon_map1" title="" id="DVGoogleMap2dTheme"><a href="javascript:void(0);" rel="map2d" onclick="changeVisulizerChart(this)"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
						<div class="tristate_icon_map2" title="" id="DVGoogleMap3dTheme"><a href="javascript:void(0);" rel="map3d" onclick="changeVisulizerChart(this)"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
						<div class="drop_tristate_bg"><span class="visul_section1_bottom visul_section1_t_spc" id="lang_Map"><!--Map--></span></div>
					</div>
			
				</span>
			<!-- -->
			</span>
			<span class="visul_section1_bottom" onmouseover="showVisualizationHeading(this)" onmouseout="HideCallout('divCallout');" id="lang_Map_di"><!--Map--></span>
			<!--<span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>-->
        </span>
        
        
        
        <span id="visul_section8">
        <span id="apyramid" class="visul_section1"><span id="replace8" class="visual_icon-8-8" rel='pyramid' onclick="changeVisulizerChart(this)" onmouseover="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmouseout="HideCallout('divCallout');"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span></span>
        <span class="visul_section1_bottom" onmouseover="showVisualizationHeading(this)" onmouseout="HideCallout('divCallout');" id="lang_Pyramid"><!--Pyramid--></span>
        <!--<span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>-->
        </span>
        
        <span id="visul_section9">
        <span id="atreemap" class="visul_section1"><span id="replace9" class="visual_icon-9-9" rel='treemap' onclick="changeVisulizerChart(this)" onmouseover="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmouseout="HideCallout('divCallout');"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span></span>
        <span class="visul_section1_bottom" onmouseover="showVisualizationHeading(this)" onmouseout="HideCallout('divCallout');" id="lang_Treemap"><!--Treemap--></span>
       <!-- <span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>-->
        </span>
        
        <span id="visul_section10">
        <span id="aradar" class="visul_section1"><span id="replace10" class="visual_icon-10-10" rel="radar" onclick="changeVisulizerChart(this)" onmouseover="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmouseout="HideCallout('divCallout');"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span></span>
        <span class="visul_section1_bottom" onmouseover="showVisualizationHeading(this)" onmouseout="HideCallout('divCallout');" id="lang_Radar"><!--Radar--></span>        
        </span>
        
        
        <span id="visul_section11" style="display: none;">
        <span class="visul_section1"><span id="replace11" class="visual_icon-11-11" rel="cloud" onclick="changeVisulizerChart(this)" onmouseover="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmouseout="HideCallout('divCallout');"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span></span>
        <span class="visul_section1_bottom" onmouseover="showVisualizationHeading(this)" onmouseout="HideCallout('divCallout');" >Cloud</span>
        </span>
        
        
        <span id="visul_section12">
		<div id="visul_section_tristate_scatter"></div>
        <span id="ascatter2d" class="visul_section1"><span id="replace12" class="visual_icon-12-12" rel='scatter2d' onclick="changeVisulizerChart(this);" onmouseover="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmouseout="HideCallout('divCallout');"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></span>
		
		<span id="span_visual_icon_tristate12" class="" rel="scatter2d"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="10" border="0" /></span>
		<!-- -->
		<span class="tristate_dropdown_contnr">
		<div id="visual_icon_tristate_scatter2d" style="height:62px;" class="tristate_dropdown">
        	<div class="tristate_icon_scatter1"><a href="javascript:void(0);" rel="scatter3d" onclick="changeVisulizerChart(this)" title="Scatter 3D" onmouseover="showVisualizationToolTip(this)" onmousemove="showVisualizationToolTip(this)" onmouseout="HideCallout('divCallout');"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="32" height="32" border="0" /></a></div>
            
            <div class="drop_tristate_bg"><span class="visul_section1_bottom visul_section1_t_spc" id="langScatter"></span></div>
        </div>
        
		</span>
		<!-- -->

		</span>
        <span class="visul_section1_bottom" onmouseover="showVisualizationHeading(this)" onmouseout="HideCallout('divCallout');" id="lang_Scatter_di"><!--Map--></span>
        <!--<span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>-->
        </span>
        
        
        
    </div>
    <!-- The Tab "Visualization" Data Ends Here--> 
    
    
    <!-- The Tab "Settings" Data Starts Here-->
	<div id="tab-3" class="panesInner">
    	<span id="settings_section1">
          <span class="settings_section1">
			 <div class="settings_section1_lft_sec">
			   <p><input id="di_vcpborder" type="checkbox" onclick="callChartProperty('border', this.checked)" /><span id="langVcShowBdr"><!--Show Border--></span></p>
			   <p><input id="di_vcpgridx" type="checkbox" onclick="callChartProperty('gridX', this.checked)" /><span id="langVcGridLnX"><!--Gridlines [X-Axis]--></span></p>
			   <p><input id="di_vcpgridy" type="checkbox" onclick="callChartProperty('gridY', this.checked)" /><span id="langVcGridLnY"><!--Gridlines [Y-Axis]--></span></p>
			   <p><input id="di_vcpdatalbl" type="checkbox" onclick="callChartProperty('dataLbl', this.checked)" /><span id="langVcDataLbl"><!--Data Labels--></span></p>
			   
			 </div>
			 <div class="settings_section1_rgt_sec">
			   <p>&nbsp;<input id="di_vcplegend" type="checkbox" onclick="callChartProperty('legend', this.checked)" />&nbsp;&nbsp;<span id="langVcLegend"> <!--Legend--></span></p>
			   <p><script>vcColorPicker('dicolor1', '#ffffff', '../../stock/themes/default/images/dataview', 'y', 'Chart', 'langVcBgChart');</script></p>
			   <p><div id="colorpick_pa_hiddn"><script>vcColorPicker('dicolor2', '#ffffff', '../../stock/themes/default/images/dataview', 'y', 'Plot Area', 'langVcBgPArea');</script></div></p>
			 </div>
		  </span>
		  <span class="settings_section1_bottom" id="lang_General" onclick="openVisPropertyPopUp('general')"><!--General--></span>
          <span class="corner" rel='TTEGSetting' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');" onclick="openVisPropertyPopUp('general')"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="10" height="10" border="0" /></span>
        </span>
        
        <span id="settings_section2">
        <span class="settings_section1">
		  <table cellpadding="0" cellspacing="4">
		   <tr>
		    <td><select id="di_vcTitlesList" class="vcInputSelBox" style="width:250px;" onchange="callChartProperty('title', this.value)"></select></td>
		   </tr>
		   <tr id="titles_fun_row1">
		    <td><input type="text" id="di_vctitleval" value="" class="vcInputTxtBox" style="width:248px;" onchange='callChartProperty("changeTitle", this.value)'/></td>
		   </tr>
		   <tr>
		    <td>
			 <table cellpadding="0" cellspacing="0">
			  <tr>
			  <td class="vctfontlist_pos"><select id="di_vctfontlist" onchange="callChartProperty('changeTfont', this.value)" class="vcInputSelBox" style="width:50px;"></select></td>
			  <td><span class="vcTitleIcon vcTitleIcon_fontbig" rel='TTIFSize' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
               onclick="callChartProperty('changeTFontOneByOne', true);javascript:di_jq(this).addClass('currentFS');"></span></td>
			  <td><span class="vcTitleIcon vcTitleIcon_fontsmall" rel='TTDFSize' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"

              onclick="callChartProperty('changeTFontOneByOne', false);javascript:di_jq(this).addClass('currentFS');"></span></td>
			  <td><span id="vc_Bold" class="vcTitleIcon vcTitleIcon_bold"
               rel='TTBold' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
                onclick="callChartProperty('changeTFontstyle', 'b');javascript:di_jq(this).addClass('currentFS');toggleActiveClass(this);"></span></td>
			  <td><span id="vc_Italic"   rel='TTItalic' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
              class="vcTitleIcon vcTitleIcon_italic" onclick="callChartProperty('changeTFontstyle', 'i');javascript:di_jq(this).addClass('currentFS');toggleActiveClass(this);"></span></td>
			  <td><span id="vc_Underline" rel='TTULine' onmouseover="showSettingToolTip(this);" onmouseout="HideCallout('divCallout');"
               class="vcTitleIcon vcTitleIcon_underline" onclick="callChartProperty('changeTFontstyle', 'u');javascript:di_jq(this).addClass('currentFS');toggleActiveClass(this);"></span></td>
			  <td><span class="vcTitleIcon vcTitleIcon_color" rel='TTFColor' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"> <script>	  vcColorPicker('dicolor5', '#ffffff', '../../stock/themes/default/images/dataview', 'n', 'Plot Area', 'langVcBgPArea');</script></span></td>
			  </tr>
			 </table>
			</td>
		   </tr>
		   <tr id="titles_fun_row2">
		    <td><p><span id="lang_XPos"><!--X-Pos.--></span> <input id="labelXPos" type="text" class="vcInputTxtBox" style="width:50px;" onchange="callChartProperty('customLabelPos')" /> &nbsp;&nbsp; <span id="lang_YPos"><!--Y-Pos.--></span> <input id="labelYPos" type="text" class="vcInputTxtBox" style="width:50px;" onchange="callChartProperty('customLabelPos')" /></p></td>
		   </tr>
		   <table width="97%" cellpadding="0" cellspacing="0">
           <tr id="labels_fun_row1" style="display:none;">
		        <td>
		            <p>Pos. <input id="di_PosTxt" type="text" class="vcInputTxtBox" onchange="callChartProperty('dataLabelPosition', this.value)" style="width:50px;"/></p></td>
                    <td><p><input id="di_showLabel" type="checkbox" onclick="callChartProperty('noLabel', this.checked)"/> <span id="lang_Show"><!--Show--></span> </p></td>
                    <td>
                    <p><table width="100%" cellpadding="0" cellspacing="0">
			  <tr>
			  <td><span id="rotation1" rel='TTCTD45Text' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
              class="vcLabelIcon vcLabelIcon_1" onclick="callChartProperty('rotation', '315');RotationActiveClass(this);"></span></td>

			  <td><span id="rotation2" rel='TTCTDM45Text' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
               class="vcLabelIcon vcLabelIcon_2" onclick="callChartProperty('rotation', '-315');RotationActiveClass(this);"></span></td>

			  <td><span id="rotation3" rel='TTCTD90Text' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
              class="vcLabelIcon vcLabelIcon_3" onclick="callChartProperty('rotation', '270');RotationActiveClass(this);"></span></td>

			  <td><span id="rotation4" rel='TTCTDM90Text' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
              class="vcLabelIcon vcLabelIcon_4" onclick="callChartProperty('rotation', '-270');RotationActiveClass(this);"></span></td>

			  <td><span id="rotation5" rel='TTCTDHorizontal' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
              class="vcLabelIcon vcLabelIcon_5" onclick="callChartProperty('rotation', '360');RotationActiveClass(this);"></span></td>
			  </tr></table></p>
		        </td>
                </tr>
                </table>
		   </tr>
		    <tr>
		    <td colspan="2"><table width="97%" cellpadding="0" cellspacing="0" >
			  <tr id="labels_fun_row2" style="display:none;">
			   <td><p><span id="lang_Decimals"><!--Decimals--> </span><input id="di_decimalTxt" type="text" class="vcInputTxtBox" style="width:50px;" onchange="callChartProperty('decimal', this.value)" /></p></td>
			   <td align="right" style="padding-top:3px;"><p>&nbsp;&nbsp;<input type="checkbox" id="di_seperatorChkbox" onclick="callChartProperty('seperator', this.checked)" /> <span id="lang_U_1000separator"><!--Use 1,000 separator--></span></p></td>
			  </tr>
			 </table></td>
		   </tr>
		  </table>
		</span>
        <span class="settings_section1_bottom" id="lang_Titles_Labels" 
><!--Titles/Labels--></span>
        <span class="corner_diable" rel='TTEFSetting' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>
        </span>
        <span id="settings_section4">
        <span class="settings_section1">
		  <table cellpadding="0" cellspacing="3">
		   <tr>
		    <td><select id="di_vcSelSeries" onchange="callChartProperty('series', this.value)" class="vcInputSelBox" style="width:162px;"></select></td>
		   </tr>
		   <tr>
		    <td>
			 <table cellpadding="0" cellspacing="0">
			  <tr>
			  <td rel='TTFColor' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');">
              <script>vcColorPicker('dicolor7', '#ffffff', '../../stock/themes/default/images/dataview', 'y', '', '');</script>
              </td>
			  <td>
              <span id="editSeries" rel='TTBBColor' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"; class="vcSriesIcon vcSriesIcon_1" >
              <script>vcColorPicker('dicolor8', '#ffffff', '', 'n', '', '');</script>
              </span>
              </td>
			  <td>
              <span id="styleSeries" rel='TTBBLWidth' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');" class="vcSriesIcon vcSriesIcon_2" onclick="openSbdrSolidDiv()">
              </span>
			  <div id="sbdrSolidDiv">
			   <div class="vcbdrSolidIcon vcbdrSolidIcon_1" onclick="callChartProperty('seriesBorderWidth', '1')"></div>
			   <div class="vcbdrSolidIcon vcbdrSolidIcon_2" onclick="callChartProperty('seriesBorderWidth', '2')"></div>
			   <div class="vcbdrSolidIcon vcbdrSolidIcon_3" onclick="callChartProperty('seriesBorderWidth', '3')"></div>
			   <div class="vcbdrSolidIcon vcbdrSolidIcon_4" onclick="callChartProperty('seriesBorderWidth', '4')"></div>
			   <div class="vcbdrSolidIcon vcbdrSolidIcon_5" onclick="callChartProperty('seriesBorderWidth', '5')"></div>
			   <div class="vcbdrSolidIcon vcbdrSolidIcon_6" onclick="callChartProperty('seriesBorderWidth', '6')"></div>
			   <div class="vcbdrSolidIcon vcbdrSolidIcon_7" onclick="callChartProperty('seriesBorderWidth', '7')"></div>
			  </div>
			  </td>
			  <td><span id="lineStyle" rel='TTDType' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"

              class="vcSriesIcon vcSriesIcon_3" onclick="openSbdrThinDiv()"></span>
			  <div id="sbdrThinDiv">
			   <div class="vcbdrThinIcon vcbdrThinIcon_1" onclick="callChartProperty('seriesBorderStyle', 'Solid')"></div>
			   <div class="vcbdrThinIcon vcbdrThinIcon_2" onclick="callChartProperty('seriesBorderStyle', 'ShortDash')"></div>
			   <div class="vcbdrThinIcon vcbdrThinIcon_3" onclick="callChartProperty('seriesBorderStyle', 'ShortDashDot')"></div>
			   <div class="vcbdrThinIcon vcbdrThinIcon_4" onclick="callChartProperty('seriesBorderStyle', 'ShortDashDotDot')"></div>
			   <div class="vcbdrThinIcon vcbdrThinIcon_5" onclick="callChartProperty('seriesBorderStyle', 'Dot')"></div>
			   <div class="vcbdrThinIcon vcbdrThinIcon_6" onclick="callChartProperty('seriesBorderStyle', 'Dash')"></div>
			  </div>
			  </td>
			  <td><span id="noBorderIcon" rel='TTNBIcon' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"

               class="vcSriesIcon vcSriesIcon_4" onclick="callChartProperty('seriesBorderWidth', '0')"></span></td>
			  </tr>
			 </table>
			</td>
		   </tr>
		   <tr>
		    <td>
			 <table cellpadding="0" cellspacing="0" class="vcColorFormat_main_contnr">
			  <tr>
			   <td><span class="vcColorFormatIcon vcColorFormatIcon_1" onclick="callChartProperty('chartStyle', '1')"></span></td>
			   <td><span class="vcColorFormatIcon vcColorFormatIcon_2" rel='TTCSColor' onclick="callChartProperty('chartStyle', '2')"></span></td>
			   <td valign="bottom"><a href="javascript:void(0);" onclick="openSFormatDiv()"><img src="../../stock/themes/default/images/dataview/sformatdown.png" id="sFormatDivClickId"></a></td>
			  </tr>
			 </table>
			 <div id="sFormatDiv" class="sFormatDiv">
			  <table cellpadding="0" cellspacing="0">
			   <tr>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_3" onclick="callChartProperty('chartStyle', '3')"></span></td>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_4" onclick="callChartProperty('chartStyle', '4')"></span></td>
			   </tr>
			   <tr>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_5" onclick="callChartProperty('chartStyle', '5')"></span></td>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_6" onclick="callChartProperty('chartStyle', '6')"></span></td>
			   </tr>
			   <tr>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_7" onclick="callChartProperty('chartStyle', '7')"></span></td>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_8" onclick="callChartProperty('chartStyle', '8')"></span></td>
			   </tr>
               <tr>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_9" onclick="callChartProperty('chartStyle', '9')"></span></td>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_10" onclick="callChartProperty('chartStyle', '10')"></span></td>
			   </tr>

			   <tr>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_11" onclick="callChartProperty('chartStyle', '11')"></span></td>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_12" onclick="callChartProperty('chartStyle', '12')"></span></td>
			   </tr>
			   <tr>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_13" onclick="callChartProperty('chartStyle', '13')"></span></td>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_14" onclick="callChartProperty('chartStyle', '14')"></span></td>
			   </tr>
			   <tr>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_15" onclick="callChartProperty('chartStyle', '15')"></span></td>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_16" onclick="callChartProperty('chartStyle', '16')"></span></td>
			   </tr>
			   <tr>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_17" onclick="callChartProperty('chartStyle', '17')"></span></td>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_18" onclick="callChartProperty('chartStyle', '18')"></span></td>
			   </tr>
			   <tr>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_19" onclick="callChartProperty('chartStyle', '19')"></span></td>
			    <td><span class="vcColorFormatIcon vcColorFormatIcon_20" onclick="callChartProperty('chartStyle', '20')"></span></td>
			   </tr>
			  </table>
			 </div>
			</td>
		   </tr>
		  </table>
		</span>
        <span class="settings_section1_bottom" id="lang_Series" 

        onclick="openVisPropertyPopUp('series')"><!--Series--></span>
        <span class="corner" rel='TTESSetting' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"><a href="javascript:void(0)" onclick="openVisPropertyPopUp('series')"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="10" height="10" border="0" /></a></span>
        </span>

        <!--Pivot, Arrage and Swap-->
        <span id="settings_section5">
        <span class="settings_section1">
		<table cellspacing="3" cellpadding="0">
		   <tbody>
			<tr id="arrangeRow">
		    	    <td>
				<span class="pivotIcon1"><input type="checkbox" value="" onclick="HandlePivotClick(this);" id="chkTogglePivot" name="chkTogglePivot"></span>
        			<span class="Icon_1"><label for="chkTogglePivot"><a onclick="HandlePivotClick(this);" href="javascript:void(0);" id="langArrange">Arrange</a></label></span>
			    </td>
		        </tr>
			<tr id="pivotRow">
			    <td>
				<span onclick="PivotRowColHeaders()" class="fullViewIcon pivotIcon2"></span>
			        <span class="Icon_1"><a onclick="PivotRowColHeaders()" id="langPivot" href="javascript:void(0);">Pivot</a></span>
			    </td>
			</tr>
			<tr id="swapRow">
			    <td>
				<span onclick="swapRowColHeaders(this);" class="fullViewIcon pivotIcon3"></span>
        <span class="Icon_1"><a onclick="swapRowColHeaders(this);" id="langSwap" href="javascript:void(0);">Swap</a></span>
			    </td>
			</tr>
		  </tbody>
		</table>
	</span>
        <span id="Span3" class="settings_section1_bottom"></span>
        <!--<span class="corner"><a onclick="openVisPropertyPopUp('series')" href="javascript:void(0)"><img width="10" height="10" border="0" src="../../stock/themes/default/images/dataview/spacer.gif"></a></span>-->
        </span>
        <!-- End pivot, Arrage and Swap-->
    </div>
    <!-- The Tab "Settings" Data Starts Here-->

	<!-- The Tab "Option" Data Starts Here-->

    <div id="tab-4" class="panesInner">
	  <span id="map_section1">
        <span class="map_section1">
		 <table cellpadding="0" cellspacing="0" width="100%">
		  <tr>
		   <td>
		    <!--<div id="diMapParentMode">
			 <span id="mapNull" rel="" class="mapModeIcon mapModeIcon_null" title="Default"><a class="mapModeIcon selMode"></a></span>
			 <span id="mapFull" rel="full" class="mapModeIcon mapModeIcon_full" title="Full Extent"></span>
			 <span id="mapZext" rel="zext" class="mapModeIcon mapModeIcon_zext" title="Zoom in frame"></span>
			 <span id="mapZin" rel="zin" class="mapModeIcon mapModeIcon_zin" title="Zoom In"></span>
			 <span id="mapZout" rel="zout" class="mapModeIcon mapModeIcon_zout" title="Zoom Out"></span>
			 <span id="mapPan" rel="pan" class="mapModeIcon mapModeIcon_pan" title="Pan"></span>
			</div>-->
		   </td>
		  </tr>
		  <tr>
		   <td><p><input type="checkbox" id="map_dtlabels" onclick="mapDBNSCalls(this.checked, 'dl')"> <span id="lang_Data_Labels"><!--Data Labels--></span></p></td>
		  </tr>
		  <tr>
		   <td><p><input type="checkbox" id="map_borders" onclick="mapDBNSCalls(this.checked, 'bdr')"> <span id="lang_Borders"><!--Borders--></span></p></td>
		  </tr>
		  <tr>
		   <td><p><input type="checkbox" id="map_nrsymbol" onclick="mapDBNSCalls(this.checked, 'ns')"> <span id="lang_North_symbol"><!--North symbol--></span></p></td>
		  </tr>
		  <tr>
		   <td><p><input type="checkbox" id="map_scale" onclick="mapDBNSCalls(this.checked, 'sc')"> <span id="lang_Scale"><!--Scale--></span></p></td>
		  </tr>
		 </table>
		</span>
        <span class="map_section1_bottom" id="vcLangLblOptions" onclick="openMapPropertyPopUp('general')"><span id="lang_General_tab4"><!--General--></span></span>
        <span class="corner" onclick="openMapPropertyPopUp('general')"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>
      </span>
	  <span id="map_section2">
        <span class="map_section2">
		 <table cellpadding="0" cellspacing="0" width="100%">
		  <tr>
		   <td colspan="7" style="padding-bottom:3px;"><select id="di_mapSelTitle" class="vcInputSelBox" style="width:213px;" onchange="getMapTitleDetails(this.value)">
		    <option value="1" id="lang_Title"><!--Title--></option><option value="2" id="lang_Data_Label"><!--Data Label 1--></option><option value="6" id="lang_Data_Label2"><!--Data Label 2--></option><option value="4" id="lang_Legend_Title"><!--Legend Title--></option><option value="5" id="lang_Legend_Body"><!--Legend Body--></option><option value="3" id="lang_Disclaimer"><!--Disclaimer--></option>
		   </select></td>
		  </tr>
		  <tr>
		   <td colspan="7" style="padding-bottom:3px;"><input type="text" id="di_mapTxtTitle" class="vcInputSelBox" style="width:213px;" onchange="setMapTitleDetails()" maxlength="250" /></td>
		  </tr>
		  <tr>
		   <td class="vctfontlist_pos"><select id="di_mapSelFont" class="vcInputSelBox" style="width:50px;" onchange="setMapTitleDetails()"><option value="6">6</option><option value="7">7</option><option value="8">8</option><option value="9">9</option><option value="10" selected>10</option><option value="11">11</option><option value="12">12</option><option value="13">13</option><option value="14">14</option><option value="15">15</option><option value="16">16</option><option value="17">17</option><option value="18">18</option><option value="19">19</option><option value="20">20</option><option value="21">21</option><option value="22">22</option><option value="23">23</option><option value="24">24</option><option value="25">25</option><option value="26">26</option><option value="27">27</option><option value="28">28</option><option value="29">29</option><option value="30">30</option><option value="31">31</option></select></td>
		   <td><span id="mapTitle_bFont" rel='TTIFSize' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
             class="vcTitleIcon vcTitleIcon_fontbig" onclick="checkMapFontST('BF')"></span></td>
		 
           <td><span id="mapTitle_sFont" rel='TTDFSize' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
           class="vcTitleIcon vcTitleIcon_fontsmall" onclick="checkMapFontST('SF')"></span></td>

		   <td><span id="mapTitle_Bold" rel='TTBold' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
           class="vcTitleIcon vcTitleIcon_bold" onclick="checkMapFontST('B')"></span></td>

		   <td><span id="mapTitle_Italic" rel='TTItalic' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
            class="vcTitleIcon vcTitleIcon_italic" onclick="checkMapFontST('I')"></span></td>

		   <td><span id="mapTitle_Underline"  rel='TTULine' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"
           class="vcTitleIcon vcTitleIcon_underline" onclick="checkMapFontST('U')"></span></td>

		   <td><span class="vcTitleIcon vcTitleIcon_color" rel='TTFColor' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');" ><script>		                                                       vcColorPicker('dicolormap1', '#000000', '../../stock/themes/default/images/dataview', 'n', '', '');</script>
		    <input type="hidden" id="h_dicolormap1" value="#000000" />
			<input type="hidden" id="h_fontbold" value="false" />
			<input type="hidden" id="h_fontitalic" value="false" />
			<input type="hidden" id="h_fontunderline" value="false" />
		   </span></td>
		  </tr>
		 </table>
		</span>
        <span class="map_section2_bottom" id="vcLangLblOptions"><span id="lang_map_Titles_Labels"><!--Titles/Labels--></span></span>
        <span class="corner_diable"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>
      </span>
	  <span id="map_section3">
        <span class="map_section3">
		 <table cellpadding="0" cellspacing="0" width="100%">
		  <tr>
		   <td width="210" valign="top" nowrap class="MapTheme_contnr">
		    <div id="shMapTheme" class="shMap_bttm">
				<div><table width="100%" cellpadding="0" cellspacing="0"><tr><td><p id="mapTxtSelTheme"><!--Select Theme--></p></td><td align="right"><div id="shMapAction2"><img src="../../stock/themes/default/images/dataview/Plus_Icon.gif" style="cursor:pointer;" onclick="openMapPropertyPopUp('theme_add')"> <span id="lngMapAddTheme" style="cursor:pointer;font-size:11px;" onclick="openMapPropertyPopUp('theme_add')"><!--Add Theme--></span></div></td></tr></table></div>
				<div><table cellpadding="0" cellspacing="0"><tr><td><select id="di_mapTheme" class="vcInputSelBox" onchange="changeMapThemeSelection(this.value)"></select></td><td class="mapThemeType"><img id="mapThemeType" rel=""></td></tr></table></div>
			</div>
			<div id="shMapSeries" class="shMap_bttm">
			 <p id="mapTxtSelSeries"><!--Select Series--></p>
			 <p><select id="di_mapSeries" class="vcInputSelBox" onchange="changeSeriesBreaks()"></select></p>
			</div>
			<div id="shMapAction">
			 <p><!--<input id="mapAddTheme" type="button" value="Add" class="dv_mapButton" onclick="openMapPropertyPopUp('theme_add')">&nbsp;--><input id="mapDelTheme" type="button" value="Delete" class="dv_mapButton" onclick="deleteMapTheme()" /> &nbsp <span id="mapShowTheme"><input type="checkbox" id="mapChkShowTh" onclick="visibilityMapTheme(this.checked)" /> <span id="mapTxtShowTh"><!--Show Theme--></span></span></p>
			</div>
		   </td>
		   <td width="304" valign="top" class="mapThTypeColorHatch_contnr">
		    <!-- Start div for theme types Color and Hatch -->
			<div id="mapThTypeColorHatch">
			   <table cellpadding="0" cellspacing="0">
				<tr>
				 <td width="100" valign="top">
				   <!-- Start for Color/Hatch quick settings-->
				   <div id="divIdColoHatchShow"> 
					<p id="lang_Break_Type"><!--Break Type--></p>
					<p><select id="di_mapBrkType" class="vcInputSelBox" onchange="changeSeriesBreaks()"><option value="EqualCount" id="lang_Equal_Count"><!--Equal Count--></option><option value="EqualSize" id="lang_Equal_Size"><!--Equal Size--></option><option value="Continuous" id="lang_Continuous"><!--Continuous--></option><option value="Discontinuous" id="lang_Discontinuous"><!--Discontinuous--></option></select></p>
					<p style="padding-top:3px;" id="lang_Break_Count"><!--Break Count--></p>
					<p style="padding-bottom:5px;"><select id="di_mapBrkCount" class="vcInputSelBox" onchange="changeSeriesBreaks()">
					   <option value="2">2</option>
					   <option value="3">3</option>
					   <option value="4">4</option>
					   <option value="5">5</option>
					   <option value="6">6</option>
					   <option value="7">7</option>
					   <option value="8">8</option>
					   <option value="9">9</option>
					   <option value="10">10</option>
					 </select></p>

					 <table id="mapSeriesFormatTable" rel='TTCTColor' onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');" cellpadding="0" cellspacing="0" class="mapSeriesFormatTable">
					  <tr>
					   <td><div id="mapSrC_0" class="map_color_palette_1" rel="0" onclick="javascript:di_jq('#mapSeriesFormatDiv').slideToggle();"></div></td>
					   <td valign="bottom"><a href="javascript:void(0);" onclick="javascript:di_jq('#mapSeriesFormatDiv').slideToggle();"><img src="../../stock/themes/default/images/dataview/sformatdown.png" id="clickMapSeries"></a></td>
					  </tr>
					 </table>
					 <div id="mapSeriesFormatDiv" class="mapSeriesFormatDiv">
						<div class="map_color_palette_container">
							<div id="mapSrC_1" rel="1" class="map_color_palette_1" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_2" rel="2" class="map_color_palette_2" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_3" rel="3" class="map_color_palette_3" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_4" rel="4" class="map_color_palette_4" onclick="mapChangeSeriesColor(this.id)"></div>
						</div>
						<div class="map_color_palette_container">
							<div id="mapSrC_5" rel="5" class="map_color_palette_5" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_6" rel="6" class="map_color_palette_6" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_7" rel="7" class="map_color_palette_7" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_8" rel="8" class="map_color_palette_8" onclick="mapChangeSeriesColor(this.id)"></div>
						</div>
						<div class="map_color_palette_container">
							<div id="mapSrC_9" rel="9" class="map_color_palette_9" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_10" rel="10" class="map_color_palette_10" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_11" rel="11" class="map_color_palette_11" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_12" rel="12" class="map_color_palette_12" onclick="mapChangeSeriesColor(this.id)"></div>
						</div>
						<div class="map_color_palette_container">
							<div id="mapSrC_13" rel="13" class="map_color_palette_13" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_14" rel="14" class="map_color_palette_14" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_15" rel="15" class="map_color_palette_15" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_16" rel="16" class="map_color_palette_16" onclick="mapChangeSeriesColor(this.id)"></div>
						</div>
						<div class="map_color_palette_container">
							<div id="mapSrC_17" rel="17" class="map_color_palette_17" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_18" rel="18" class="map_color_palette_18" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_19" rel="19" class="map_color_palette_19" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_20" rel="20" class="map_color_palette_20" onclick="mapChangeSeriesColor(this.id)"></div>
						</div>
						<div class="map_color_palette_container">
							<div id="mapSrC_21" rel="21" class="map_color_palette_21" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_22" rel="22" class="map_color_palette_22" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_23" rel="23" class="map_color_palette_23" onclick="mapChangeSeriesColor(this.id)"></div>
							<div id="mapSrC_24" rel="24" class="map_color_palette_24" onclick="mapChangeSeriesColor(this.id)"></div>
						</div>
					 </div>
				   </div>
				   <!-- End for Color/Hatch quick settings-->
				   <!-- Start for Chart quick settings-->
				   <div id="divIdChartShow" style="display:none;">
				    <table cellpadding="0" cellspacing="0">
					   <tr>
						 <td width="100" valign="top">
						   <div>
							<p class="mapChartSlider_bttm" id="lang_Width" style="padding-bottom:4px;"><!--Width--></p>
							<div id="mapChartSliderW" class="mapChartSlider"></div>
						   </div>
						   <div style="padding-top:5px;">
							<p class="mapChartSlider_bttm" id="lang_Height" style="padding-bottom:4px;"><!--Height--></p>
							<div id="mapChartSliderH" class="mapChartSlider"></div>
						   </div>
						 </td>
						</tr>
					  </table>
				   </div>
				   <!-- End for Chart quick settings-->
				 </td>
				 <td width="204">
					 <div id="scrollContainer">
					 <div id="scrollSubContainer">
					  <div id="mapThemeLegend" class="Scroller-Container" style="position:relative;">
					  </div>
					 </div>

					 <div id="map_hatchpat_list" class="map_hatchpat_list">
						<div class="map_hatch_patern_container">
							<div class="hatch_patern_Horizontal" onclick="setMapHatchPatern('Horizontal')" title="Horizontal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Vertical" onclick="setMapHatchPatern('Vertical')" title="Vertical"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_ForwardDiagonal" onclick="setMapHatchPatern('ForwardDiagonal')" title="ForwardDiagonal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_BackwardDiagonal" onclick="setMapHatchPatern('BackwardDiagonal')" title="BackwardDiagonal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Cross" onclick="setMapHatchPatern('Cross')" title="Cross"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
						</div>
						<div class="map_hatch_patern_container">
							
							<div class="hatch_patern_DiagonalCross" onclick="setMapHatchPatern('DiagonalCross')" title="DiagonalCross"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Percent05" onclick="setMapHatchPatern('Percent05')" title="Percent05"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Percent10" onclick="setMapHatchPatern('Percent10')" title="Percent10"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Percent20" onclick="setMapHatchPatern('Percent20')" title="Percent20"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Percent25" onclick="setMapHatchPatern('Percent25')" title="Percent25"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
						</div>
						<div class="map_hatch_patern_container">
							
							<div class="hatch_patern_Percent30" onclick="setMapHatchPatern('Percent30')" title="Percent30"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Percent40" onclick="setMapHatchPatern('Percent40')" title="Percent40"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Percent50" onclick="setMapHatchPatern('Percent50')" title="Percent50"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Percent60" onclick="setMapHatchPatern('Percent60')" title="Percent60"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Percent70" onclick="setMapHatchPatern('Percent70')" title="Percent70"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
						</div>
						<div class="map_hatch_patern_container">
							
							<div class="hatch_patern_Percent75" onclick="setMapHatchPatern('Percent75')" title="Percent75"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Percent80" onclick="setMapHatchPatern('Percent80')" title="Percent80"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Percent90" onclick="setMapHatchPatern('Percent90')" title="Percent90"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_LightDownwardDiagonal" onclick="setMapHatchPatern('LightDownwardDiagonal')" title="LightDownwardDiagonal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_LightUpwardDiagonal" onclick="setMapHatchPatern('LightUpwardDiagonal')" title="LightUpwardDiagonal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
						</div>
						<div class="map_hatch_patern_container">
							<div class="hatch_patern_DarkDownwardDiagonal" onclick="setMapHatchPatern('DarkDownwardDiagonal')" title="DarkDownwardDiagonal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_DarkUpwardDiagonal" onclick="setMapHatchPatern('DarkUpwardDiagonal')" title="DarkUpwardDiagonal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_WideDownwardDiagonal" onclick="setMapHatchPatern('WideDownwardDiagonal')" title="WideDownwardDiagonal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_WideUpwardDiagonal" onclick="setMapHatchPatern('WideUpwardDiagonal')" title="WideUpwardDiagonal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_LightVertical" onclick="setMapHatchPatern('LightVertical')" title="LightVertical"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
						</div>
						<div class="map_hatch_patern_container">
							<div class="hatch_patern_LightHorizontal" onclick="setMapHatchPatern('LightHorizontal')" title="LightHorizontal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_NarrowVertical" onclick="setMapHatchPatern('NarrowVertical')" title="NarrowVertical"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_NarrowHorizontal" onclick="setMapHatchPatern('NarrowHorizontal')" title="NarrowHorizontal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_DarkVertical" onclick="setMapHatchPatern('DarkVertical')" title="DarkVertical"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_DarkHorizontal" onclick="setMapHatchPatern('DarkHorizontal')" title="DarkHorizontal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
						</div>
						<div class="map_hatch_patern_container">
						
							<div class="hatch_patern_DashedDownwardDiagonal" onclick="setMapHatchPatern('DashedDownwardDiagonal')" title="DashedDownwardDiagonal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_DashedUpwardDiagonal" onclick="setMapHatchPatern('DashedUpwardDiagonal')" title="DashedUpwardDiagonal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_DashedHorizontal" onclick="setMapHatchPatern('DashedHorizontal')" title="DashedHorizontal"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_DashedVertical" onclick="setMapHatchPatern('DashedVertical')" title="DashedVertical"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_SmallConfetti" onclick="setMapHatchPatern('SmallConfetti')" title="SmallConfetti"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>        
						</div>
						<div class="map_hatch_patern_container">
							
							<div class="hatch_patern_LargeConfetti" onclick="setMapHatchPatern('LargeConfetti')" title="LargeConfetti"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_ZigZag" onclick="setMapHatchPatern('ZigZag')" title="ZigZag"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Wave" onclick="setMapHatchPatern('Wave')" title="Wave"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_DiagonalBrick" onclick="setMapHatchPatern('DiagonalBrick')" title="DiagonalBrick"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_HorizontalBrick" onclick="setMapHatchPatern('HorizontalBrick')" title="HorizontalBrick"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
						</div>
						<div class="map_hatch_patern_container">
							
							<div class="hatch_patern_Weave" onclick="setMapHatchPatern('Weave')" title="Weave"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Plaid" onclick="setMapHatchPatern('Plaid')" title="Plaid"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Divot" onclick="setMapHatchPatern('Divot')" title="Divot"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_DottedGrid" onclick="setMapHatchPatern('DottedGrid')" title="DottedGrid"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_DottedDiamond" onclick="setMapHatchPatern('DottedDiamond')" title="DottedDiamond"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
						</div>
						<div class="map_hatch_patern_container">
							<div class="hatch_patern_Shingle" onclick="setMapHatchPatern('Shingle')" title="Shingle"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Trellis" onclick="setMapHatchPatern('Trellis')" title="Trellis"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_Sphere" onclick="setMapHatchPatern('Sphere')" title="Sphere"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_SmallGrid" onclick="setMapHatchPatern('SmallGrid')" title="SmallGrid"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_SmallCheckerBoard" onclick="setMapHatchPatern('SmallCheckerBoard')" title="SmallCheckerBoard"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
						</div>
						<div class="map_hatch_patern_container">
							<div class="hatch_patern_LargeCheckerBoard" onclick="setMapHatchPatern('LargeCheckerBoard')" title="LargeCheckerBoard"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_OutlinedDiamond" onclick="setMapHatchPatern('OutlinedDiamond')" title="OutlinedDiamond"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
							<div class="hatch_patern_SolidDiamond" onclick="setMapHatchPatern('SolidDiamond')" title="SolidDiamond"><a href="javascript:void(0)"><img src="../../stock/themes/default/images/dataview/map/spacer.gif" width="17" height="15" /></a></div>
						</div>
					 </div>

					</div>
					<!-- For scrollbar ...starts-->
					<div id="Scrollbar-Container" style="visibility: visible; ">
					  <div class="Scrollbar-Up"></div>
					  <div class="Scrollbar-Down"></div>
					  <div class="Scrollbar-Track" style="background-color:#d3d4d6; ">
						<div class="Scrollbar-Handle" style="visibility: visible;"></div>
					  </div>
					</div>
					<!-- For scrollbar ...ends-->
				 </td>
				</tr>
			   </table>
		    </div>
			<!-- End div for theme types Color and Hatch -->
			<!-- Start div for theme type Dot densisty -->
			<div id="mapThTypeDotDensity" style="display:none;">
			 <table cellpadding="0" cellspacing="0">
			  <tr>
			   <td class="Marker_Style_pos"><p id="lang_Marker_Style"><!--Marker Style--></p></td>
			   <td>
			     <table cellpadding="0" cellspacing="0" class="DotDtyStyle">
					<tr>
					  <td><div id="mapDotDtyStyle_0" class="mapDotDtyStyle_Circle" rel="Circle"></div></td>
					  <td valign="bottom"><a href="javascript:void(0);" onclick="javascript:di_jq('#mapDotDtyStyleList').slideToggle();"><img src="../../stock/themes/default/images/dataview/sformatdown.png" id="clickMapDotDensity"></a></td>
					</tr>
				  </table>
				  <div id="mapDotDtyStyleList" class="mapDotDtyStyleList">
					<div id="mapDotDtyStyle_Circle" class="mapDotDtyStyle_Circle" onclick="mapChangeDotDtyStyle('Circle')"></div>
					<div id="mapDotDtyStyle_Square" class="mapDotDtyStyle_Square" onclick="mapChangeDotDtyStyle('Square')"></div>
					<div id="mapDotDtyStyle_Triangle" class="mapDotDtyStyle_Triangle" onclick="mapChangeDotDtyStyle('Triangle')"></div>
					<div id="mapDotDtyStyle_Cross" class="mapDotDtyStyle_Cross" onclick="mapChangeDotDtyStyle('Cross')"></div>
				  </div>
			   </td>
			  </tr>
			  <tr>
			   <td class="Marker_Style_pos"><p id="lang_Marker_Size"><!--Marker Size--></p></td>
			   <td class="Marker_Style_pos_top"><select id="mapDotDtySize" class="vcInputSelBox" style="width:80px;" onchange="setMapDotDensityDetails()"><option value="1">1</option><option value="2">2</option><option value="3">3</option><option value="4">4</option><option value="5">5</option><option value="6">6</option><option value="7">7</option><option value="8">8</option><option value="9">9</option><option value="10">10</option><option value="11">11</option><option value="12">12</option><option value="13">13</option><option value="14">14</option><option value="15">15</option><option value="16">16</option><option value="17">17</option><option value="18">18</option><option value="19">19</option><option value="20">20</option></select></td>
			  </tr>
			  <tr>
			   <td class="Marker_Style_pos"><p id="lang_Marker_Color"><!--Marker Color--> </p></td>
			   <td class="Marker_Style_pos_top"><div id="mapDotDtyColor"></div><input type="hidden" id="mapDotDtyColorCodeH"></td>
			  </tr>
			  <tr>
			   <td class="Marker_Style_pos"><p id="lang_Marker_Value"><!--Marker Value--></p></td>
			   <td class="Marker_Style_pos_top"><input id="mapDotDtyValue" class="vcInputTxtBox" style="width:80px;" onchange="setMapDotDensityDetails()"></td>
			  </tr>
			 </table>
			</div>
			<!-- End div for theme type Dot densisty -->
			
		   </td>
		  </tr>
		 </table>
		</span>
        <span class="map_section3_bottom" id="vcLangLblOptions" onclick="openMapPropertyPopUp('theme_edit')"><span id="lang_Theme"><!--Theme--></span></span>
        <span class="corner" onclick="openMapPropertyPopUp('theme_edit')"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></span>
      </span>
    </div>

	<!-- The Tab "Option" Data Starts Here-->
    
    <!-- The Tab "Option" Data Starts Here-->

    <!--<div id="tab-4" class="panesInner">
	  <span id="options_section1">
        <span class="options_section1">
		 <select id="di_seriesPie" class="vcInputSelBox" style="width:200px;" onchange="callChartProperty('changePieData', this.selectedIndex)"></select>
		</span>
        <span class="options_section1_bottom" id="vcLangLblOptions">Options</span>
        <span class="corner_diable"><a href="#"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></a></span>
      </span>
    </div>-->

	<!-- The Tab "Option" Data Starts Here-->

    <!-- The Tab "View/Sort" Data Starts Here-->

	<!--<div id="tab-5" class="panesInner">
	  <span id="viewsort_section1">
        <span class="viewsort_section1" id="divViewPivotRowHeaders">
		</span>
        <span class="viewsort_section1_bottom" id="vcLangLblView">View</span>
        <span class="corner_diable"><a href="#"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></a></span>
        </span>
    	<span id="viewsort_section2">
        <span class="viewsort_section1">
		  <p>Sort</p>
		</span>
        <span class="viewsort_section1_bottom" id="vcLangLblSort">Sort</span>
        <span class="corner_diable"><a href="#"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></a></span>
        </span>
    	<span id="viewsort_section3">
        <span class="viewsort_section1">
		 <p><input type="checkbox"> Full Screen</p>
		 <p><input type="checkbox"> Show/Hide Filter</p>
		</span>
        <span class="viewsort_section1_bottom" id="vcLangLblGeneral">General</span>
        <span class="corner_diable"><a href="#"><img src="../../stock/themes/default/images/dataview/spacer.gif" width="7" height="7" border="0" /></a></span>
        </span>
	</div>-->
	<!-- The Tab "View/Sort" Data Ends Here-->

</div>
</div>
<!-- ***************** - DATA VIEW -- DataView Navigation Ends- ***************** -->


<div class="dv_contnt_panel_wrapr">


<!-- ***************** - SIDE FILTERS --  Starts- ***************** -->
<div id="example5">
		
  <ul id="menu5" class="example_menu">
            
  </ul>
  <ul id="menu6" class="example_menu">
            
  </ul>
</div>
<!-- ***************** - SIDE FILTERS --  Ends- ***************** -->


<!-- ***************** - FILTER COLLAPSE --  Starts- ***************** -->
<div class="dv_filter_collapse_main"><a href="javascript:void(0);"><img id="imgShowHideFiltersPanel" rel='TTHFPanel'
 onmouseover="showSettingToolTip(this);"  onmouseout="HideCallout('divCallout');"; src="../../stock/themes/default/images/dataview/spacer.gif" alt="" border="0" onclick="toggleFiltersPanel();" class="dv_filter_expd_img"/></a>

	<!-- start map icons -->
	<div class="dataview_zoom_view" id="diMapParentMode" style="display:none;">
		<span id="mapNull" rel="" class="mapModeIcon mapModeIcon_null" title=""><a class="mapModeIcon selMode"></a></span>
		<span id="mapFull" rel="full" class="mapModeIcon mapModeIcon_full" title=""></span>
		<span id="mapZext" rel="zext" class="mapModeIcon mapModeIcon_zext" title=""></span>
		<span id="mapZin" rel="zin" class="mapModeIcon mapModeIcon_zin" title=""></span>
		<span id="mapZout" rel="zout" class="mapModeIcon mapModeIcon_zout" title=""></span>
		<span id="mapPan" rel="pan" class="mapModeIcon mapModeIcon_pan" title=""></span>
        <span id="mapNud" rel="nudge" class="mapModeIcon mapModeIcon_nud" title="" style="display:none;"></span>     
    </div>
   <!-- end map icons -->
   <!-- start 3d map opacity -->
   <div class="dataview_zoom_view" id="diMap3DOpacityBox" style="display:none;padding-left:5px;padding-top:5px;">
    <div id="diMap3DOpacity" style="height:150px;background-color:#D3D3D3;" title="Opacity"></div>
   </div>
   <!-- end 3d map opacity  -->

</div>
<!-- ***************** - FILTER COLLAPSE --  Ends- ***************** -->



<!-- ***************** - MIDDLE CHART -- AREA Starts- ***************** -->
<div id="vcContainer" class="vcContainer">


    <img id="imgGridLoader" class="imgGridLoader" src="../../stock/themes/default/images/loading_img.gif" alt="Loading..." />

	<!-- Start Visulization tab -->
	<div id="tab-vsection" class="middleContainer" style="display:block;">
     
	 <div id="visChartDiv" style="height:500px;"></div>
	 
	</div>
	<!-- Start Visulization tab-->

	<!-- Start Data Grid -->
	<div id="tab-dsection" class="middleContainer"> 
	 <div id="divGridContainer_dtview flt_lft">
		
		<div id="gridData">
		  <div id="dataTitle" class="heading2 flt_lft" style="padding-left:10px;"></div><div id="onoffPivoting"><input type="checkbox" name="pivotChk" id="pivotChk" onclick="togglePivot()" /> <span id="lang_Pivoting"><!--Pivoting--></span></div>
		  <div id="dataSubTitle"></div>
		  <div id="content"></div><br />
		  <div id="pivot" class="dv_pivot">
			 <div id="pivot_page" style="display: none;"></div>
			 <div id="pivot_content"></div>
			 <div id="pivot_chart"></div>
		  </div>
		</div>
		<div id="divCountDataRows"></div>
		<div id="divSingleSource"></div>
	 </div>
	 
	 <div id="tab_pivot333">
		<table width="100%" cellpadding="0" cellspacing="2" border="0" class="flt_lft">
		   <tr>
			  <td align="right">
				<div id="panelbar">
				  <div id="pb_1"></div>
				  <div id="pb_11"></div>
				</div>
			  </td>
			</tr>
		  </table>
	 </div>
	 </div>

<!-- Popup for Select Area Component ...starts-->
<div id="AreaOuterBox" class="popup_block">
    <h1 class="popup_heading" id="lang_Select_Area"><!--Select Area--></h1>
    
    <!-- Popup Inside Content ...starts-->
    <div class="popup_brd">
    
        <div id="area_div"></div>
                   
    </div>
    <!-- Popup Inside Content ...ends-->    

        <!-- Popup Buttons ...starts-->
        <div class="di_gui_button_pos"><input type="button" name="areaOk" id="areaOk" value="" onclick="onClickOkPopUpButton('area')" class="di_gui_button" /><input type="button" name="areaCancel" id="areaCancel" value="" onclick="hidePopupSelection()" class="di_gui_button" /></div>   
        <!-- Popup Buttons ...starts--> 

</div>
<!-- Popup for Select Area Component ...ends-->

<!-- Popup for Select Topic Component ...starts-->
<div id="IndOuterBox" class="popup_block">	
    <h1 id="langSelectTopic" class="popup_heading"></h1>
    
    <!-- Popup Inside Content ...starts-->
    <div class="popup_brd">
    
        <div id="indicator_div"></div>
    
    </div>
    <!-- Popup Inside Content ...ends--> 
    
        <!-- Popup Buttons ...starts-->
        <div class="di_gui_button_pos"><input type="button" name="indOk" id="indOk" value="" onclick="onClickOkPopUpButton('ind')" class="di_gui_button" /><input type="button" name="indCancel" id="indCancel" value="" onclick="hidePopupSelection()" class="di_gui_button" /></div>        
        <!-- Popup Buttons ...ends-->            

</div>
<!-- Popup for Select Topic Component ...ends-->	 

<!-- Popup for Select Topic Component ...starts-->
<div id="SourceMetaOuterBox" class="popup_block" style="width: 720px; height: 520px;">	
    <h1 id="langShowSourceMetaData" class="popup_heading multiline"></h1>
    
    <!-- Popup Inside Content ...starts-->
    <div class="popup_brd" >
    
        <div id="divSourceMetaData"></div>
    
    </div>
    <!-- Popup Inside Content ...ends--> 
    
        <!-- Popup Buttons ...starts-->
        <div class="di_gui_button_pos"><input type="button" name="indCancel" id="Button2" value="Close" onclick="showHideComponent()" class="di_gui_button" /></div>        
        <!-- Popup Buttons ...ends-->            

</div>
<!-- Popup for Select Topic Component ...ends-->	

<!--
	  <div id='IndOuterBox' class="popup_block" style="width:700px; height:485px;"><%--onclick="showHideComponent('close');"--%>		
		<table width="100%" cellpadding="0" cellspacing="0" border="0">
		    <tr>
			 <td id="langSelectTopic" class="popup_heading1"></td>
			</tr>			
		 <tr>
		  <td valign="top">
		   <table cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
		    <tr>
			 <td><div id="indicator_div"></div></td>
			</tr>
			<tr>
			 <td><div class="di_gui_button_pos"><input type="button" name="indOk" id="indOk" value="" onclick="onClickOkPopUpButton('ind')" class="di_gui_button" /><input type="button" name="indCancel" id="indCancel" value="" onclick="showHideComponent('close')" class="di_gui_button" /></div></td>
			</tr>
		   </table>
		  </td>
		 </tr>
		</table>
	</div>
       
    <div id='AreaOuterBox' class="popup_block" style="width:700px; height:475px;">		
		<table width="100%" cellpadding="0" cellspacing="0" border="0">
		    <tr>
			 <td class="popup_heading1">Select Area</td>
			</tr>			
		 <tr>
		  <td valign="top">
		   <table cellpadding="0" cellspacing="0" style="border:1px solid #d3d3d3;">
		    <tr>
			 <td><div id="area_div"></div></td>
			</tr>
			<tr>
			 <td><div class="di_gui_button_pos"><input type="button" name="areaOk" id="areaOk" value="OK" onclick="onClickOkPopUpButton('area')" class="di_gui_button" /><input type="button" name="areaCancel" id="areaCancel" value="Cancel" onclick="showHideComponent('close')" class="di_gui_button" /></div></td>
			</tr>
		   </table>
		  </td>
		 </tr>
		</table>
	</div> 

-->
	 <!-- end for data grid -->

	<!-- Start Source tab -->
	<div id="tab-ssection" class="middleContainer">
		<ul id="ulSources">
		</ul> 
	</div>
	<!-- Start Source tab-->

	<!-- Start Keywords tab -->
	<div id="tab-ksection" style="display: none;" class="middleContainer">
		<!--show keywords here-->
	</div>
	<!-- Start Keywords tab-->

</div>




<!-- ***************** - MIDDLE CHART -- AREA Ends- ***************** -->
</div>
<!-- Start Bottom Tabs -->
<div class="bttm_tab_main_contnr">
    
	<table width="100%" cellspacing="0" cellpadding="0">
	 <tr>
	  <td width="18%" id="bottom_tabs">&nbsp;</td>
	   <td width="35%">
		<ul class="blueBottm" style="height:80px;">
		 <li><a id="tab-v" class="current" href="javascript:void(0);" onclick="changeBottomTab('tab-v')"><!--Visualization--></a></li>
		 <li><a id="tab-d" class="" href="javascript:void(0);" onclick="changeBottomTab('tab-d')"><!--Data--></a></li>
		 <li><a id="tab-s" class="" href="javascript:void(0);" onclick="changeBottomTab('tab-s')"><!--Source--></a></li>
		 <li style="display:none;"><a id="tab-k" class=""  href="javascript:void(0);" onclick="changeBottomTab('tab-k')"><!--Keywords--></a></li>    
		</ul>
		</td>
      <td width="47%" class="blueBottm" id="timeLine_cont">
      </td>
    </tr>
	</table>
     <input type="hidden" id="timeLineChkBoxValue" />
          <input type="hidden" id="disabledTimelineChkBoxTitle" />
          <input type="hidden" id="enabledTimelineChkBoxTitle" />
          <input type="hidden" id="playAnimatorBtnTitle" />
          <input type="hidden" id="stopAnimatorBtnTitle" />
</div>
<!-- End Bottom Tabs -->
<div class="clear">&nbsp;</div>
</div>
</div>

<!-- START DEVELOPER CODE -->
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>'; // use in hosting app
        var dvMrdThreshold = '<%=Global.dvMrdThreshold%>';
        var dvHideSource = '<%=Global.dvHideSource%>';

        var di_components = "Area,Indicator,Language,DIVC";
        var di_vctype = "Column,Bar,Line,Area,Pie,Pyramid,Treemap,Map,Radar,Cloud,Scatter,Scatter3d";
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" +

"><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    
    </script>
	<!-- for multiple selection -->
	<script type="text/javascript" src="../js/jquery.multiselect.js?v=<%=Global.js_version%>"></script>
	<link href="../../stock/themes/default/css/jquery.multiselect.css" rel="stylesheet" type="text/css" />

	<script type="text/javascript">

	    onPageLoad('<%=hdsby%>', '<%=hdbnid%>', '<%=hselarea%>', '<%=hselind%>', '<%=hlngcode%>',

'<%=hlngcodedb%>', '<%=hselindo%>', '<%=hselareao%>', '<%=hdvnids%>', '<%=hLoggedInUserNId%>', '<%=hLoggedInUserName%>', '<%=hCsvFilePath%>', '<%=hIsMapEnabled%>');
	    // Start for tristate
	    (function ($) {
	        $.fn.delayedAction = function (options) {
	            var settings = $.extend(
					{},
					{
					    delayedAction: function () { },
					    cancelledAction: function () { },
					    hoverTime: 1000
					},
					options);

	            return this.each(function () {
	                var $this = $(this);
	                $this.hover(function () {
	                    $this.data('timerId',
								  setTimeout(function () {
								      $this.data('hover', false);
								      settings.delayedAction($this);
								  }, settings.hoverTime));
	                    $this.data('hover', true);
	                },
					function () {
					    if ($this.data('hover')) {
					        clearTimeout($this.data('timerId'));
					        settings.cancelledAction($this);
					    }
					    $this.data('hover', false);
					});
	            });
	        }
	    })(jQuery);

	    di_jq(document).ready(function () {
	        ShowHidePlayHowToVideoButton("visulization", document.getElementById('DivHowToVideoLinks'));
	        di_jq('.visual_icon_tristate').delayedAction(
			{
			    delayedAction: function ($element) {
			        id_tag = $element.attr("rel");
			        visual_open_tristate(id_tag);
			        di_jq('.visual_icon_tristate[rel="' + id_tag + '"]').css('visibility', 'hidden');
			        //di_jq('#visul_section_tristate_'+id_tag).show(); //
			        if (di_jq('#a' + id_tag).attr('class') == 'visul_section1 visual_icon_selected') {
			            di_jq('#visual_icon_tristate_' + id_tag).css("background-image", "url(../../stock/themes/default/images/dataview/bg_tristate_dark.gif)");
			        }
			    },
			    hoverTime: 500
			});

	        di_jq('#visual_icon_tristate_line').bind('mouseenter mouseleave', function (evt) {
	            //var currentTime = new Date();
	            //var type = di_jq(this).attr('rel');var mEnterTime;
	            if (evt.type == 'mouseenter') {
	                //mEnterTime = currentTime.getTime();
	            } else if (evt.type == 'mouseleave') {
	                //var mouseoverTime = currentTime.getTime() - mEnterTime;
	                visual_close_tristate('line');
	                di_jq('.visual_icon_tristate[rel="line"]').css('visibility', 'visible');
	            }
	        });
	        di_jq('#visual_icon_tristate_column').bind('mouseenter mouseleave', function (evt) {
	            if (evt.type == 'mouseleave') {
	                visual_close_tristate('column');
	                di_jq('.visual_icon_tristate[rel="column"]').css('visibility', 'visible');
	            }
	        });
	        di_jq('#visual_icon_tristate_bar').bind('mouseenter mouseleave', function (evt) {
	            if (evt.type == 'mouseleave') {
	                visual_close_tristate('bar');
	                di_jq('.visual_icon_tristate[rel="bar"]').css('visibility', 'visible');
	            }
	        });
	        di_jq('#visual_icon_tristate_area').bind('mouseenter mouseleave', function (evt) {
	            if (evt.type == 'mouseleave') {
	                visual_close_tristate('area');
	                di_jq('.visual_icon_tristate[rel="area"]').css('visibility', 'visible');
	            }
	        });
	        di_jq('#visual_icon_tristate_map').bind('mouseenter mouseleave', function (evt) {
	            if (evt.type == 'mouseleave') {
	                visual_close_tristate('map');
	                di_jq('.visual_icon_tristate[rel="map"]').css('visibility', 'visible');
	            }
	        });
	        di_jq('#visual_icon_tristate_scatter2d').bind('mouseenter mouseleave', function (evt) {
	            if (evt.type == 'mouseleave') {
	                visual_close_tristate('scatter2d');
	                di_jq('.visual_icon_tristate[rel="scatter2d"]').css('visibility', 'visible');
	            }
	        });
	    });
	    // end for tristate

	    //code to hide div for download etc.
	    di_jq(document).click(function (e) {
	        // call function to hide opened divs
	        hideDropDownDiv(e);
	    });
	    //

	    var mapObj;
	    di_jq(document).ready(function () {

	        // detect mouse position
	        /*di_jq(document).mousemove(function(e){
	        var X = e.pageX - di_jq(document).scrollLeft();
	        var Y = e.pageY - di_jq(document).scrollTop();
	        di_jq('#mouseX').val(X);
	        di_jq('#mouseY').val(Y);
	        });*/


	        /* start code for mouse wheel for MAP only */
	        function handle(delta) {
	            var s = delta + ": ";
	            if (di_jq('#diMapImg').css('display') == 'inline' && di_jq('#tab-vsection').css('display') != 'none') {
	                if (delta < 0) {
	                    //s += "down";
	                    mapObj.zoomOut();
	                }
	                else {
	                    //s += "up";
	                    mapObj.zoomIn();
	                }
	            }
	        }
	        function wheel(event) {
	            var delta = 0;
	            if (!event) event = window.event;
	            if (event.wheelDelta) {
	                delta = event.wheelDelta / 120;
	            } else if (event.detail) {
	                delta = -event.detail / 3;
	            }
	            if (delta)
	                handle(delta);

	            //if(di_jq('#diMapImg').css('display')=='inline' && di_jq('#tab-vsection').css('display')!='none') { 
	            if (event.preventDefault)
	                event.preventDefault();
	            event.returnValue = false;
	            //document.getElementById("pageMasterAspx").style.overflow = "hidden";
	            //}
	            //else {
	            //if(event.preventDefault==false)
	            //event.preventDefault();
	            //event.returnValue = true; 
	            //document.getElementById("pageMasterAspx").style.overflow = "auto";
	            //}
	        }
	        /* Initialization code. */
	        //if (window.addEventListener)
	        //window.addEventListener('DOMMouseScroll', wheel, false);
	        //window.onmousewheel = document.onmousewheel = wheel;
	        /* end code for mouse wheel*/

	        /* Start code for map */
	        di_jq('#diMapParentMode span').click(function () {
	            var pmode = di_jq(this).attr('rel');

	            if (pmode == 'full') {
	                mapObj.setFullExtent();
	            }
	            else {
	                di_jq('#diMapParentMode span a').remove();
	                di_jq(this).append('<a class="mapModeIcon selMode"></a>');
	                mapObj.mode = pmode;
	            }
	            if (pmode == 'zin') { di_jq('#diMapImg').css('cursor', '-moz-zoom-in'); }
	            else if (pmode == 'zout') { di_jq('#diMapImg').css('cursor', '-moz-zoom-out'); }
	            else if (pmode == 'pan') { di_jq('#diMapImg').css('cursor', '-moz-grab'); }
	            else if (pmode == 'zext') { di_jq('#diMapImg').css('cursor', 'crosshair'); }
	            else if (pmode == '') { di_jq('#diMapImg').css('cursor', 'default'); }

	            // for panning image
	            if (pmode == 'pan') {
	                // just refresh tha map image
	                mapObj.refreshMap();

	                di_jq('#diMapImg').draggable('enable');
	                mapObj.setPanZoom(mapObj);
	            }
	            else if (pmode == 'zext') {
	                di_jq('#diMapImg').draggable('disable', 1);
	                di_jq("#diMapImg").removeAttr('class');
	                di_jq('#diMapImg').css('opacity', 1);
	                di_jq("#diMapImg").selectable({ disabled: false });
	                mapObj.setRubberZoom(mapObj);
	            }
	            else {
	                di_jq("#diMapImg").removeAttr('class');
	                di_jq('#diMapImg').draggable('disable', 1);
	                di_jq("#diMapImg").removeClass('ui-draggable-disabled');
	                di_jq("#diMapImg").removeClass('ui-state-disabled');
	            }
	        });

	        // on click on image
	        di_jq('#diMapImg').live('click',
		   function (e, ui) {
		       var x = e.pageX - this.offsetLeft;
		       var y = e.pageY - this.offsetTop;

		       if (mapObj.mode == 'zin') mapObj.zoomIn(x, y);
		       if (mapObj.mode == 'zout') mapObj.zoomOut(x, y);
		       //if(mapObj.mode=='pan') mapObj.zoomIn();
		   }
		);
	        /* End code for map */

	    });
	    /* end functions for map */

	    /* Scroll Bar ...starts */
	    var scroller = null;
	    var scrollbar = null;

	    window.onload = function () {
	        scroller = new jsScroller(document.getElementById("scrollSubContainer"), 160, 300);
	        scrollbar = new jsScrollbar(document.getElementById("Scrollbar-Container"), scroller, true, scrollbarEvent);
	    }

	    function scrollbarEvent(o, type) {
	        if (type == "mousedown") {
	            if (o.className == "Scrollbar-Track") o.style.backgroundColor = "#E3E3E3";
	            //else o.style.backgroundColor = "#BBB";
	        } else {
	            if (o.className == "Scrollbar-Track") o.style.backgroundColor = "#EEE";
	            //else o.style.backgroundColor = "#CCC";
	        }
	    }
	    /* Scroll Bar ...ends */

	</script>

   
<!-- END OF DEVELOPER CODE -->

<!-- Start Visuliazer Pop-Up container for all -->
<div id="dv_container_popup" class="popup_block" style="height:440px;">
	<!-- The Top Heading Area Starts Here -->
    <div id="dv_sec_pop">
    <h1>General</h1>
    <h4>Modify your settings here</h4>
    </div>
	<!-- The Top Heading Area Starts Here -->
	<div id="dv_sec_mid">
		<div id="dv_sec_left"></div>
		<div id="dv_sec_right" class="popupContainer"></div>
		<div id="dv_sec_bottom">
			<div class="dv_ppup_bttn_sec"><input id="lang_ppup_OkEle" type="button" value="" onclick="ApplyClosePopup()" class="dv_button"/> <input id="btnApply" type="button" value="" onclick="Apply()" class="dv_button"/> <input id="lang_ppup_CancelEle" type="button" value="" onclick="closeVisPropertyPopUp()" class="dv_button"/></div>
		</div>
	</div>
</div>
<!-- End Visuliazer Pop-Up container for all -->
<input id="popupTab" type="hidden" value=""/>

<!-- Start MAP Pop-Up container for all -->
<div id="dv_container_popup_m" class="popup_block" style="height:440px;z-index:1200;">
	<!-- The Top Heading Area Starts Here -->
    <div id="dv_sec_pop_m">
    <h1 id="lang_Map_General"><!--General--></h1>
    <h4 id="lang_Modify_YSH"><!--Modify your settings here--></h4>
    </div>
	<!-- The Top Heading Area Starts Here -->
	<div id="dv_sec_mid_m">
		<div id="dv_sec_left_m"></div>
		<div id="dv_sec_right_m" class="popupContainer"></div>
		<div id="dv_sec_bottom_m"><input id="popupMapTab" type="hidden" value="" />
			<div class="dv_ppup_bttn_sec"><input type="button" id="lang_ppup_OK" value="" onclick="applyMapPopup('ok')" class="dv_button" /> <input type="button" id="lang_ppup_Apply" value="" onclick="applyMapPopup('apply')" class="dv_button" /> <input type="button" id="lang_ppup_Cancel" value="" onclick="closeMapPropertyPopUp()" class="dv_button" /></div>
		</div>
	</div>
</div>
<!-- End MAP Pop-Up container for all -->

<!-- Start MAP Pop-Up container for Symbol Patern -->
<div id="dv_container_popup_symbol_pat" class="popup_block">
	<!-- The Top Heading Area Starts Here -->
    <div id="dv_sec_pop_m">
    <h1 id="lang_Symbol"><!--Symbol--></h1>
    <h4 id="lang_Map_Modify_YSH"><!--Modify your settings here--></h4>
    </div>
	<!-- The Top Heading Area Starts Here -->
	<div id="dv_sec_mid_symbolpat">
		<div id="dv_sec_right_symbolpat" class="popupContainer" style="height:275px;">
		 <table cellpadding="0" cellspacing="0" width="100%" style="padding-top:10px;">
		  <tr>
		   <td class="normalText" id="lang_Marker_Font"><!--Marker Font--></td>
		   <td class="normalText" width="50" id="lang_Color"><!--Color--></td>
		   <td class="normalText" width="60" id="lang_Size"><!--Size--></td>
		   <td class="normalText" width="80" id="lang_Opacity"><!--Opacity--></td>
		  </tr>
		  <tr>
		   <td><select id="mapPopMarkerFont" class="vcInputSelBox" onchange="changeMartkerFontStyle(this.value)">
		    <option value="Webdings" id="lang_Webdings"><!--Webdings--></option>
			<option value="Wingdings" id="lang_Wingdings"><!--Wingdings--></option>
		   </select></td>
		   <td><div id="mapPopMarkerColor"></div></td>
		   <td><select id="mapPopMarkerSize" class="vcInputSelBox" style="width:50px;"></select></td>
		   <td><div id="mapPopMarkerOpacity"></div><input type="hidden" id="mapPopMarkerOpacityTxt" /></td>
		  </tr>
		  <tr>
		   <td colspan="4">
		    <div id="mapMarkerCharStyle"></div>
		   </td>
		  </tr>
		  <!--<tr>
		   <td height="25"><div id="mapMarkerPreview" class="normalText">Preview</div></td>
		  </tr>-->
		 </table>
		</div>
		<div id="dv_sec_bottom_symbolpat"><input id="popupMapSymbolTab" type="hidden" value=""/>
			<div class="dv_ppup_bttn_sec"><input type="button" id="lang_Map_OK" value="" onclick="applyMapSymbolPat()" class="dv_button" /> <input type="button" id="lang_Map_Cancel" value="" onclick="mapPOPCloseSymbolPaternList()" class="dv_button" /></div>
		</div>
	</div>
</div>
<!-- End MAP Pop-Up container for Symbol Patern -->

<!-- Start MAP Image Option Pop-Up container for download-->
<div id="dv_container_popup_download_mapimage" class="popup_block" style="width:350px;height:250px;">
	<!-- The Top Heading Area Starts Here -->
    <div id="dv_sec_pop_m">
    <h1 id="lang_Image_Settings"><!--Image Settings--></h1>
    <h4 id="lang_Map_ppup_Modify_YSH"><!--Modify your settings here--></h4>
    </div>
	<div id="dv_sec_mid_symbolpat">
		<div id="dv_sec_right_symbolpat" class="popupContainer" style="height:170px;">
		 <table cellpadding="0" cellspacing="0" width="100%" style="padding:10px;">
		  <tr>
		   <td class="normalText" height="25" width="120" id="lang_Show_Legend"><!--Show Legend--></td>
		   <td class="normalText"><input id="mapDwnld_legend" type="checkbox" checked /></td>
		  </tr>
		  <tr>
		   <td class="normalText" height="25" id="lang_ppup_Width"><!--Width--></td>
		   <td class="normalText"><input id="mapDwnld_width" type="text" class="vcInputTxtBox" style="width:85px;" /></td>
		  </tr>
		  <tr>
		   <td class="normalText" height="25" id="lang_ppup_Heigth"><!--Heigth--></td>
		   <td class="normalText"><input id="mapDwnld_height" type="text" class="vcInputTxtBox" style="width:85px;" /></td>
		  </tr>
		  <tr>
		   <td class="normalText" height="25" id="lang_ppup_Image_Format"><!--Image Format--></td>
		   <td class="normalText"><select id="mapDwnld_type" class="vcInputSelBox" style="width:85px;" /><option value="png" id="lang_PNG"><!--PNG--></option><option value="jpg" id="lang_JPG"><!--JPG--></option><option value="gif" id="lang_GIF"><!--GIF--></option></select></td>
		  </tr>
		 </table>
		</div>
		<div id="dv_sec_bottom_symbolpat">
			<div class="dv_ppup_bttn_sec"><input type="button" id="lang_ppup_Map_OK" value="" onclick="mapDownload('image')" class="dv_button" /> <input type="button" id="lang_ppup_Map_Cancel" value="" onclick="mapImageOptions(false)" class="dv_button" /></div>
		</div>
	</div>
</div>
<!-- End MAP Image Option Pop-Up container for download-->

<!-- Start Visuliazer Pop-Up container for all -->
<div id="loginPopup" class="popup_block" style="height:470px;width:720px;">
	<iframe id="loginIframe" height="100%" width="100%" frameborder="0"></iframe>
</div>
<!-- End Visuliazer Pop-Up container for all -->

<!-- Start callout for map area infomation -->
<!--<div id="dv_container_map_areainfo" class="popup_block calloutDiv" style="width:210px;height:60px;">
</div>-->
<div id="dv_container_map_areainfo" class="area_callout" style="display:none;">
</div>

      <div id="DivHowToVideo" class="popup_block"  >
         <div id="dvHowToVdeo" style="display: block;">
         </div>
      </div>
<!-- End callout for map area infomation -->
<!-- Variable for language handling-->
<input id="lang_ppup_Gallery" value="" type="hidden" />
<input id="lang_ppup_SavePres" value="" type="hidden" />
<input id="lang_ppup_SavePresDesc" value="" type="hidden" />
<input id="lang_ppup_Name" value="" type="hidden" />
<input id="lang_ppup_Keywords" value="" type="hidden" />
<input id="lang_ppup_Desc" value="" type="hidden" />
<!-- ***************** - DATA VIEW -- DataView Navigation Ends- ***************** -->


<!-- ***************** - DATA VIEW -- Map-POP-UP markup Starts- ***************** -->
<div style="display:none;">
	<!-- General Header -->
	<span id="lngMapPopGeneral"><!--General--></span>
	<span id="lngMapPopModifySetting"><!--Modify your settings here--></span>
	<!-- General tab -->
	<span id="lngMapPopLabel"><!--Label--></span>
	<span id="lngMapPopBorder"><!--Border--></span>
	<span id="lngMapPopLayer"><!--Layer--></span>
	<span id="lngMapPopLblOn"><!--Label On--></span>
	<span id="lngMapPopAreaid"><!--Area ID--></span>
	<span id="lngMapPopAreaNm"><!--Area Name--></span>
	<span id="lngMapPopDataValue"><!--Data Value--></span>
	<span id="lngMapPopTmPeriod"><!--Time Period--></span>
	<span id="lngMapPopMultiRow"><!--Multi-row--></span>
	<span id="lngMapPopIndented"><!--Indented--></span>
	<span id="lngMapPopSwde"><!--Show where data exists--></span>
	<span id="lngMapPopEffects"><!--Effects--></span>
	<span id="lngMapPopEffectTp"><!--Effect Type--></span>
	<span id="lngMapPopDepth"><!--Depth--></span>
	<span id="lngMapPopShadow"><!--Shadow--></span>
	<span id="lngMapPopEmbossed"><!--Embossed--></span>
	<span id="lngMapPopBlock"><!--Block--></span>
	<span id="lngMapPopGradient"><!--Gradient--></span>
	<span id="lngMapPopReflect"><!--Reflect--></span>
	<span id="lngMapPopLeaderLine"><!--Leader Line--></span>
	<span id="lngMapPopStraightLine"><!--Straight line--></span>
	<span id="lngMapPopFields"><!--Fields--></span>
	<span id="lngMapPopLblRow1"><!--Label Row 1--></span>
	<span id="lngMapPopLblRow2"><!--Label Row 2--></span>
	<!-- Border tab -->
	<span id="lngMapPopShowBorder"><!--Show Border--></span>
	<span id="lngMapPopWidth"><!--Width--></span>
	<span id="lngMapPopColor"><!--Color--></span>
	<span id="lngMapPopStyle"><!--Style--></span>
	<!-- layer tab -->
	<span id="lngMapPopTBase"><!--Base--></span>
	<span id="lngMapPopTFeature"><!--Feature--></span>
	<span id="lngMapPopIcnFirst"><!--First--></span>
	<span id="lngMapPopIcnLast"><!--Last--></span>
	<span id="lngMapPopIcnUp"><!--Up--></span>
	<span id="lngMapPopIcnDown"><!--Down--></span>
	<span id="lngMapPopDisputedBdr"><!--Disputed Boundries--></span>
	<!-- Theme Hedder -->
	<span id="lngMapPopThemes"><!--Themes--></span>
	<span id="lngMapPopTheme"><!--Theme--></span>
	<span id="lngMapPopLegend"><!--Legend--></span>
	<span id="lngMapPopChartSetting"><!--Chart Settings--></span>
	<!-- Theme tab-->
	<span id="lngMapPopThemeName"><!--Theme Name--></span>
	<span id="lngMapPopThemeType"><!--Theme Type--></span>
	<span id="lngMapPopThemeSeries"><!--Series--></span>
	<span id="lngMapPopThChatType"><!--Chart Type--></span>
	<span id="lngMapPopThChatColumn"><!--Column--></span>
	<span id="lngMapPopThChatPie"><!--Pie--></span>
	<span id="lngMapPopThChatLine"><!--Line--></span>
	<span id="lngMapPopThColor"><!--Color--></span>
	<span id="lngMapPopThHatch"><!--Hatch--></span>
	<span id="lngMapPopThDotden"><!--Dot Density--></span>
	<span id="lngMapPopThChart"><!--Chart--></span>
	<span id="lngMapPopThSymbol"><!--Symbol--></span>
	<!-- Lagend tab --> 
	<span id="lngMapPopBrkCount"><!--Break Count--></span>
	<span id="lngMapPopBrkType"><!--Break Type--></span>
	<span id="lngMapPopBrkEqCount"><!--Equal Count--></span>
	<span id="lngMapPopBrkEqSize"><!--Equal Size--></span>
	<span id="lngMapPopBrkCont"><!--Continuous--></span>
	<span id="lngMapPopBrkDiscont"><!--Discontinuous--></span>
	<span id="lngMapPopLgMin"><!--Min--></span>
	<span id="lngMapPopLgMax"><!--Max--></span>
	<span id="lngMapPopLgDecimals"><!--Decimals--></span>
	<span id="lngMapPopUploadSetting"><!--Upload Settings--></span>
	<span id="lngMapPopLgSmooth"><!--Smooth--></span>
	<span id="lngMapPopDwnSetting"><!--Download Settings--></span>
	<!-- Dot Density -->
	<span id="lngMapPopMarStyle"><!--Marker Style--></span>
	<span id="lngMapPopMarSize"><!--Marker Size--></span>
	<span id="lngMapPopMarColor"><!--Marker Color--></span>
	<span id="lngMapPopMarValue"><!--Marker Value--></span>
	<span id="lngMapPopMarOpacity"><!--Opacity--></span>
	<!-- Chart settings -->
	<span id="lngMapPopCSViewDtVal"><!--View Data Value--></span>
	<span id="lngMapPopCSDecimals"><!--Decimals--></span>
	<span id="lngMapPopCSWidth"><!--Width--></span>
	<span id="lngMapPopCSHeight"><!--Height--></span>
	<span id="lngMapPopCSTimeSeries"><!--Time Series--></span>
	<span id="lngMapPopCSMostRecent"><!--Most Recent--></span>
	<span id="lngMapSingleThemeSelectIndTxt"><!--Select Indicator--></span>
	<span id="lngMapSingleThemeSelectSrTxt"><!--Select Series--></span>
	<span id="lngMapChartLineThicknessTxt"><!--Line Thickness--></span>
</div>
<!-- ***************** - DATA VIEW -- Map-POP-UP markup Ends- ***************** -->


<!-- ***************** - DATA VIEW -- Visualizer-POP-UP markup Starts- ***************** -->
<div style="display:none;">	
	<!-- General tab -->
	<span id="lngGridlineTxt"><!--Gridlines--></span>
	<span id="lngChartAreaTxt"><!--Chart Area--></span>
	<span id="lngPlotAreaTxt"><!--Plot Area--></span>
	<span id="lngRadiusTxt"><!--Radius--></span>
	<span id="lngSelGLAxisTxt"><!--Select Gridlines Axis--></span>
	<span id="lngXAxisTxt"><!--X-Axis--></span>
	<span id="lngYAxisTxt"><!--Y-Axis--></span>
	<span id="lngMajorGLTxt"><!--Major Gridlines--></span>
	<span id="lngMinorGLTxt"><!--Minor Gridlines--></span>
	<span id="lngBgColorTxt"><!--Background Color--></span>
	<span id="lngNoFillTxt"><!--No Fill--></span>
	<span id="lngSolidFillTxt"><!--Solid Fill--></span>
	<span id="lngGradientFillTxt"><!--Gradient Fill--></span>
	<span id="lngFillColorTxt"><!--Fill Color--></span>
    <span id="lngColor1Txt"><!--Color 1--></span>
	<span id="lngColor2Txt"><!--Color 2--></span>
	<span id="lngShowLegendTxt"><!--Show Legend--></span>
	<span id="lngBdrWidthTxt"><!--Border Width--></span>
	<span id="lngBdrColorTxt"><!--Border Color--></span>
	<span id="lngBdrRadiusTxt"><!--Border Radius--></span>	
	<span id="lngFontSizeTxt"><!--Font Size--></span>
	<span id="lngPositionTxt"><!--Position--></span>
	<span id="lngFloatingTxt"><!--Floating--></span>
	<span id="lngXPosTxt"><!--X-Pos.--></span>	
	<span id="lngYPosTxt"><!--Y-Pos.--></span>
	<span id="lngFixedTxt"><!--Fixed--></span>
	<span id="lngLengendWidthTxt"><!--Legend Width--></span>
	<span id="lngAutoTxt"><!--Auto--></span>
	<span id="lngOptionTxt"><!--Option--></span>
	<span id="lngSeriesTypeTxt"><!--Series Type--></span>
	<span id="lngLineTxt"><!--Line--></span>	
	<span id="lngColumnTxt"><!--Column--></span>
	<span id="lngRadialAxisTxt"><!--Radial Axis--></span>
	<span id="lngHorizontalTxt"><!--Horizontal--></span>
	<span id="lngVerticalTxt"><!--Vertical--></span>	
	<span id="lngBothTxt"><!--Both--></span>
	<span id="lngNoneTxt"><!--None--></span>
	<span id="lngFormsTxt"><!--Forms--></span>
	<span id="lngSegmentTxt"><!--Segment--></span>
	<span id="lngCurveTxt"><!--Curve--></span>
	<span id="lngCharTypeTxt"><!--Chart Type--></span>
	<span id="lngCircularTxt"><!--Circular--></span>
	<span id="lngPolygonalTxt"><!--Polygonal--></span>
	<span id="lngLeftTxt"><!--Left--></span>
	<span id="lngRightTxt"><!--Right--></span>
	<span id="lngTopTxt"><!--Top--></span>
	<span id="lngBottomTxt"><!--Bottom--></span>	
	<span id="lngSeriesOptionTxt"><!--Series Option--></span>
	<span id="lngFillTxt"><!--Fill--></span>
	<span id="lngOverlapTxt"><!--Overlap--></span>	
    <span id="lngGapTxt"><!--Gap--></span>	
       
    <span id="lngChartTitleTxt"><!--Chart Title--></span>
	<span id="lngChartSubtitleTxt"><!--Chart Subtitle--></span>
	<span id="lngXAxisTitleTxt"><!--X Axis Title--></span>
	<span id="lngXAxisLabelTxt"><!--X Axis Label--></span>
	<span id="lngYAxisTitleTxt"><!--Y Axis Title--></span>	
	<span id="lngYAxisLabelTxt"><!--Y Axis Label--></span>
	<span id="lngDataLabelsTxt"><!--DataLabels--></span>	
    <span id="lngNoteTxt"><!-- Note --></span>  
    <span id="lngSourceTxt"><!-- Source--></span>
    <span id="lngSelectTxt"><!-- Select --></span>

    
	<span id="lngSelectStyleTxt"><!--Select Syle--></span>
	<span id="lngSolidTxt"><!--Solid--></span>
	<span id="lngShortDashTxt"><!--Short Dash--></span>
	<span id="lngShortDotTxt"><!--Y Short Dot--></span>	
	<span id="lngShortDashDotTxt"><!--Y Short Dash Dot--></span>
	<span id="lngShortDashDotDotTxt"><!--Short Dash dot dot--></span>	
    <span id="lngDotTxt"><!-- Dot --></span>  
    <span id="lngDashTxt"><!-- Dash--></span>
    <span id="lngLongDashTxt"><!-- Long Dash--></span>
    <span id="lngDashDotTxt"><!--Dash Dot--></span>
	<span id="lngLongDashDotTxt"><!--Long Dash Dot--></span>
	<span id="lngLongDashDotDotTxt"><!--Long Dash Dot Dot--></span>

    <span id="lngLeftTopTxt"><!--Left Top--></span>
	<span id="lngLeftMiddleTxt"><!--Left Middle--></span>
	<span id="lngLeftBottomTxt"><!--Left Bottom--></span>
	<span id="lngRightTopTxt"><!--Right Top--></span>	
	<span id="lngRightMiddleTxt"><!--Right Middle--></span>
	<span id="lngRightBottomTxt"><!--Right bottom--></span>	
    <span id="lngTopToBottomTxt"><!-- Top to bottom --></span>  
    <span id="lngBottomToTopTxt"><!-- Bottom to top--></span>
    <span id="lngLeftToRightTxt"><!-- Left to Right--></span>
    <span id="lngRightToLeftTxt"><!--Right to left--></span>
	<span id="langVcBgChart"><!--Background area--></span>
	<span id="langVcBgPArea"><!--Plot Background area--></span>	
    <span id="langGLTemp"><!--GridLines--></span>
    <span id="langNameMandatory"><!--Name should not be blank--></span>	
    <%--Source value to be shown on shares visualization--%>
    <input type="hidden" id="SourceValue" />
</div>
<!-- ***************** - DATA VIEW -- Visualizer-POP-UP markup Ends- ***************** -->

</asp:Content>


