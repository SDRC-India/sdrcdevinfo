var labelType, useGradients, nativeTextSupport, animate;

(function() {
  var ua = navigator.userAgent,
      iStuff = ua.match(/iPhone/i) || ua.match(/iPad/i),
      typeOfCanvas = typeof HTMLCanvasElement,
      nativeCanvasSupport = (typeOfCanvas == 'object' || typeOfCanvas == 'function'),
      textSupport = nativeCanvasSupport 
        && (typeof document.createElement('canvas').getContext('2d').fillText == 'function');
  //I'm setting this based on the fact that ExCanvas provides text support for IE
  //and that as of today iPhone/iPad current text support is lame
  labelType = (!nativeCanvasSupport || (textSupport && !iStuff))? 'Native' : 'HTML';
  nativeTextSupport = labelType == 'Native';
  useGradients = nativeCanvasSupport;
  animate = !(iStuff || !nativeCanvasSupport);
})();

var Log = {
  elem: false,
  write: function(text){
    if (!this.elem) 
      this.elem = document.getElementById('log');
    this.elem.innerHTML = text;
    this.elem.style.left = (500 - this.elem.offsetWidth / 2) + 'px';
  }
};


function di_draw_treemap_chart(title, uid, width, height, div_id, treemapdata, datatype, x_title, y_title, allowTabs, is_save, save_url, lngcodefile, isCloseBtn, hotFuncCloseBtn, shareHotSelection, vcType, settingFileUrl, chartCategory, sourceText, DCURL) {

	var subtitle='';
	var visu_height = height - 50;
	var visu_height_wsmenu = visu_height - 130;

	var div_obj = document.getElementById(div_id);
	var di_vc_libpath = 'http://dgps/di7poc/diuilib/1.1/';
//	var di_vc_libpath = 'http://www.devinfolive.info/di7beta_1/diwebservices/diuilib/1.1/';

	if(hotFuncCloseBtn==null || hotFuncCloseBtn=='' || lngcodefile==undefined) { 
		hotFuncCloseBtn = 'di_vcclose("'+uid+'")';
	}

	if(lngcodefile=='' || lngcodefile==null || lngcodefile==undefined) {
		lngcodefile = di_vc_libpath + 'library/store/lng.xml';
	}
	//var di_vcLangArray = setLangVars( lngcodefile );

	var visul_div_id = 'di_visulcontainer_chart'+uid;
		
	var html_data = di_getVisualizerMarkupForTreeMap(uid, title, subtitle, x_title,  y_title, width, height, allowTabs, di_vc_libpath, isCloseBtn, hotFuncCloseBtn, shareHotSelection, 'treemap', vcType, DCURL);

	div_obj.innerHTML = html_data; // render the html	
	// upload data
	di_jq(document).ready(function() {

		// block for mousehover
		di_vcChangeIconStale();
		// end

		di_createFontSizeDdl('di_vctfontlist'+uid);
		di_createFontSizeDdl('di_vclfontlist'+uid);
		di_createFontSizeDdl('di_vcslfontlist'+uid);	

		// calling function to initialize color pickers
		initColorPickers(uid, 'column');
		
				
		if(vcType=='chart')
		di_create_treemap_chart(uid, "di_visulcontainer_chart"+uid, treemapdata, 'treemap');
		
		
		if(jQuery.inArray('Data', allowTabs) > -1) { // styart if data tab is avalable

		/*********** Start for upload own data************/
		di_jq('#di_vcfrm1'+uid).ajaxForm({
			beforeSubmit: function() {
				//jq('#results').html('Submitting...');
				di_jq('#di_vcprogress_bar'+uid).show();
				di_jq('#di_vcproc1'+uid).removeClass('di_gui_proc_deactive');
				di_jq('#di_vcproc1'+uid).addClass('di_gui_proc_active');
			},
			success: function(filename) { 
				
				/* calling parent application function for drawing grid an dchart
				   input: string data, divid
				*/
				drawOATGrid(filename, 'di_visulcontainer_grid'+uid);

				di_jq('#di_vcupbttn'+uid).attr('disabled', false); // enable upload button
				di_jq('#di_vcprogress_bar'+uid).hide(); // hide process text
				di_jq('#di_vcupfile'+uid).val(''); // empty file input field
				// end
			}
		});

		if(jQuery.inArray('Data', allowTabs) > -1) {
		di_vc_changeTab('gtabm_0'+uid, 'Data', "'"+allowTabs+"'", uid, visu_height);
		}
		
		} // end for data tab
		/*********** End for upload own data************/

		/*********** Start for save to gallery from ************/
		di_jq('#divcsvglfrm_'+uid).ajaxForm({ 
			beforeSubmit: function() {
				//jq('#results').html('Submitting...');
			},
			success: function(imgcontent) {
				//alert(filename);
				// calling function to store file.
				if(imgcontent=='false') {
					alert('Error:');
					return false;
				}
				else {
					var UDK = di_jq('#divc_ptag'+uid).val();
					var Name = di_jq('#divc_pname'+uid).val();
					var Desc = di_jq('#divc_pdesc'+uid).val();
					var Type = 'G';
					var ChartType = 'COL';
					
					var chartObj = di_getChartObject(uid); // chart object
					var chartInput = chartObj.options;
					var settingData  = YAHOO.lang.JSON.stringify(chartInput);  // JSON string
					
					// callback function
					SaveGalleryItem(UDK, Name, Desc, Type, ChartType, imgcontent, settingData);
				}
			}
		});
		/*********** End for save to gallery from ************/

	});		
}


/* function to create UI part of chart */
function di_getVisualizerMarkupForTreeMap(uid, title, subtitle, x_title, y_title, width, height, allowTabs, di_vc_libpath, isCloseBtn, hotFuncCloseBtn, storagePath, chartType, vcType, DCURL)
{
	// DCURL is for callback url for data tab
	var html_data;
	var visu_height = height - 50;
	if(vcType=='' || vcType==null || vcType==undefined)
		vcType = 'map';

	// preparing HTML
	if(width.indexOf('%')>0) {
		 html_data ='<div id="di_vccontainer'+uid+'" style="width:' + width +';height:'+height+'px;">';
	}
	else {
		 html_data ='<div id="di_vccontainer'+uid+'" style="width:' + width +'px;height:'+height+'px;">';
	}

	// start for main menu
	html_data +='<div id="di_vcmenu'+uid+'"><table cellpadding="0" cellspacing="0"><tr><td nowrap><ul id="di_vcglobalnav'+uid+'" class="di_gui_vctabcontainer">';
	if(allowTabs!='' && allowTabs.length > 0) {
		for(var i=0; i<allowTabs.length; i++) {
			var tabref = allowTabs[i].replace(" & ", "");
			html_data +='<li class="di_gui_vctabcontainerli"><a id="gtabm_'+i+uid+'" class="di_gui_vctabcontainerlia" href="javascript:void(0)" onclick=\'di_vc_changeTab(this.id, "'+tabref+'", "'+allowTabs+'", "'+uid+'", "'+(visu_height-10)+'")\'>'+allowTabs[i]+'</a></li>';
		}
	}
	html_data +='</ul></td></tr></table></div>';

	// start inner container
	html_data +='<div id="di_vcinnercontainer'+uid+'" class="di_gui_vccontainer" style="height:'+visu_height+'px;"><input type="hidden" name="di_vcmapxmlfile" id="di_vcmapxmlfile'+uid+'" value="">';
	if(isCloseBtn==true) {
		html_data +='<div class="di_vcbtn_close"><img src="'+di_vc_libpath+'images/di_close_bttn.png" title="Close Window" alt="Close"  border="0" onclick=\''+hotFuncCloseBtn+';\' /></div>';
	}
	
	// start for sub menu
	// for data sub menu
	if(jQuery.inArray('Data', allowTabs) > -1) {
		html_data +='<div id="di_vcsubmenu_data'+uid+'" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box2" style="width:772px"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><form name="di_vcfrm1'+uid+'" id="di_vcfrm1'+uid+'" method="POST" action="'+DCURL+'?type='+vcType+'" enctype="multipart/form-data"><table width="90%" cellpadding="0" cellspacing="0"><tr><td valign="top"><p>Upload data XLS file: <input type="file" name="di_vcupfile" id="di_vcupfile'+uid+'" size="28">&nbsp;<input type="hidden" name="di_vctype" id="di_vctype'+uid+'" value="'+vcType+'">&nbsp; <input type="submit" name="di_vcupbttn" id="di_vcupbttn'+uid+'" value="Upload" class="di_vcclickbttn"></p><div id="di_vcprogress_bar'+uid+'" style="display:none;"><div id="di_vcproc1'+uid+'" class="di_gui_proc_deactive">* Uploading data...</div><div id="di_vcproc2'+uid+'" class="di_gui_proc_deactive">* Validating data...</div><div id="di_vcproc3'+uid+'" class="di_gui_proc_deactive">* Preparing data for Map...</div><div id="di_vcproc4'+uid+'" style="height:5px;" class="di_gui_proc_active"></div></div></td><td valign="top" nowrap><ul><li><a class="di_vcclicklink" href="'+di_vc_libpath+'library/store/SampleDataEntrySheet.xls">Download data entry sheet</a></li><li><a class="di_vcclicklink" href="'+di_vc_libpath+'library/store/AreaSheet.xls">Download Area Excel file</a></li></ul><div><a href="javascript:void(0)" onclick=\'di_toggleContainer("'+uid+'","c");setVisualizerData("'+chartType+'")\'>Chart</a> | <a href="javascript:void(0)" onclick=\'di_toggleContainer("'+uid+'","g")\'>Grid</a></div></td></tr></table></form></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Upload Data</td></tr></table></div></div></div>';
	}

	// for settings
	html_data +='<div id="di_vcsubmenu_settings'+uid+'" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><p><input type="checkbox" id="di_vcpborder'+uid+'" onclick=\'di_toggleChartBorder("'+uid+'", this.checked)\'>Border</p><p><input type="checkbox" id="di_vcpgridl'+uid+'" onclick=\'di_toggleGridLine("'+uid+'", this.checked)\'>Gridlines</p><p><input type="checkbox" id="di_vcpdatalbl'+uid+'" onclick=\'di_toggleDatalabels("'+uid+'", this.checked,"'+chartType+'")\'>Data Labels</p><p><input type="checkbox" onclick="">Data Tabs</p></td><td valign="top" style="padding:0px 5px 0px 5px;"><p><input type="checkbox" onclick=\'di_toggleLegendVisibility("'+uid+'",this.checked)\' id="di_vcplegend'+uid+'" onclick="">&nbsp;&nbsp;Legend</p><div><table cellspacing="0"><tr><td>'+di_coloPicker1(uid, 'dicolor1', '#ffffff', di_vc_libpath, 'y')+'</td><td><p>Chart</p></td></tr></table></div><div><table cellspacing="0"><tr><td>'+di_coloPicker1(uid, 'dicolor2', '#ffffff', di_vc_libpath, 'y')+'</td><td><p>Plot Area</p></td></tr></table></div></td></tr><tr><td colspan="2" height="20" class="di_vcsec_box_bttmtxt">General</td></tr></table></div><div class="di_vcsec_box"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><table width=100%" cellpadding="0" cellspacing="0"><tr><td>'+di_vcGetTitles(title, subtitle, x_title, y_title,'Source','Label', uid)+'</td></tr><tr><td height="30"><input type="text" id="di_vctitleval'+uid+'" value="" class="di_vcdrop_box" disabled="true" onchange=\'di_changeTitleText("'+uid+'", this.value)\'/></td></tr><tr><td height="30"><table cellpadding="0" cellspacing="0"><tr><td><select id="di_vctfontlist'+uid+'" onchange=\'di_applyTitltFontSize("'+uid+'",this.value)\' class="di_vcsmalldrop_box"></select>&nbsp</td><td><span class="di_vc_icon di_vc_icon_fontbig" onclick=\'di_applyTitleFontSize("'+uid+'",true)\'></span></td><td><span class="di_vc_icon di_vc_icon_fontsmall" onclick=\'di_applyTitleFontSize("'+uid+'",false)\'></span></td><td><span class="di_vc_icon di_vc_icon_bold" onclick=\'di_applyTitleFontStyle("'+uid+'", "b")\'></span></td><td><span class="di_vc_icon di_vc_icon_italic" onclick=\'di_applyTitleFontStyle("'+uid+'", "i")\'></span></td><td><span class="di_vc_icon di_vc_icon_underline" onclick=\'di_applyTitleFontStyle("'+uid+'", "u")\'></span></td><td><span class="di_vc_icon di_vc_icon_color">'+di_coloPicker1(uid, 'dicolor5', '#FF0500', di_vc_libpath, 'n')+'</span></td></tr></table></td></tr></table></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Titles</td></tr></table></div><div class="di_vcsec_box" style="width:316px"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><table width=100%" cellpadding="0" cellspacing="0"><tr><td><select id="di_vcSelLabel'+uid+'" class="di_vcdrop_box" onchange=\'di_setAxisDatalabelsUI("'+uid+'", this,"'+chartType+'")\'><option value="-1">Select Axis</option><option value="x">X-Axis</option><option value="y">Y-Axis</option><option value="dl">Data Labels</option></select></td></tr><tr><td height="30"><table cellpadding="0" cellspacing="0"><tr><td><select id="di_vclfontlist'+uid+'" onchange=\'di_applyAxisDataLabelFontSize("'+uid+'",this.value,"'+chartType+'")\' class="di_vcsmalldrop_box"></select>&nbsp</td><td><span class="di_vc_icon di_vc_icon_fontbig" onclick=\'di_applyAxisLabelFontSize("'+uid+'",true,"'+chartType+'")\'></span></td><td><span class="di_vc_icon di_vc_icon_fontsmall" onclick=\'di_applyAxisLabelFontSize("'+uid+'",false,"'+chartType+'")\'></span></td><td><span class="di_vc_icon di_vc_icon_bold"   onclick=\'di_applyAxisLabelFontStyle("'+uid+'", "b","'+chartType+'")\'></span></td><td><span class="di_vc_icon di_vc_icon_italic" onclick=\'di_applyAxisLabelFontStyle("'+uid+'", "i","'+chartType+'")\'></span></td><td><span class="di_vc_icon di_vc_icon_underline" onclick=\'di_applyAxisLabelFontStyle("'+uid+'", "u","'+chartType+'")\'></span></td><td><span class="di_vc_icon di_vc_icon_color">'+di_coloPicker1(uid, 'dicolor6', '#FF0500', di_vc_libpath, 'n')+'</span></td></tr></table></td></tr><tr><td height="30"><table cellspacing="0" cellpadding="0"><tr><td height="28"><span class="di_vc_lblicon di_vc_lblicon_1" onclick=\'di_applyAxisDataLabelRotation("'+uid+'","315","'+chartType+'")\'></span></td><td><span class="di_vc_lblicon di_vc_lblicon_2" onclick=\'di_applyAxisDataLabelRotation("'+uid+'","-315","'+chartType+'")\'></span></td><td><span class="di_vc_lblicon di_vc_lblicon_3" onclick=\'di_applyAxisDataLabelRotation("'+uid+'","270","'+chartType+'")\'></span></td><td><span class="di_vc_lblicon di_vc_lblicon_4" onclick=\'di_applyAxisDataLabelRotation("'+uid+'","-270","'+chartType+'")\'></span></td><td><span class="di_vc_lblicon di_vc_lblicon_5" onclick=\'di_applyAxisDataLabelRotation("'+uid+'","360","'+chartType+'")\'></span></td></tr></table></td></tr></table></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Data Labels</td></tr></table></div></div></div>';
	
	// for series
	html_data +='<div id="di_vcsubmenu_series'+uid+'" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><table width=100%" cellpadding="0" cellspacing="0"><tr><td valign="top"><select id="di_vcSelLegend'+uid+'" onchange=\'di_changeLegendPosition("'+uid+'", this.value)\' class="di_vcdrop_box70"><option value="1">Left top</option><option value="2">Left middle</option><option value="3">Left bottom</option><option value="4">Top</option><option value="5">Bottom</option><option value="6">Right top</option><option value="7">Right middle</option><option value="8">Right bottom</option></select></td><td><p><input type="checkbox" id="di_vclborderchk'+uid+'" onclick=\'di_toggleLegendBorder("'+uid+'", this.checked)\'/>&nbsp;&nbsp;Border </p><p><table cellspacing="0"><tr><td>'+di_coloPicker1(uid, 'dicolor3', '#FFFFFF', di_vc_libpath, 'y')+'</td><td><p>Background Color</p></td></tr></table></p></td></tr><tr><td height="30" colspan="2"><table cellpadding="0" cellspacing="0"><tr><td><select id="di_vcslfontlist'+uid+'" onchange=\'di_applyLegendFontSize("'+uid+'",this.value)\' class="di_vcsmalldrop_box"></select>&nbsp</td><td><span class="di_vc_icon di_vc_icon_fontbig" onclick=\'di_applyLegendItemSize("'+uid+'",true)\'></span></td><td><span class="di_vc_icon di_vc_icon_fontsmall" onclick=\'di_applyLegendItemSize("'+uid+'",false)\'></span></td><td><span class="di_vc_icon di_vc_icon_bold"   onclick=\'di_applyLegendFontStyle("'+uid+'", "b")\'></span></td><td><span class="di_vc_icon di_vc_icon_italic" onclick=\'di_applyLegendFontStyle("'+uid+'", "i")\'></span></td><td><span class="di_vc_icon di_vc_icon_underline" onclick=\'di_applyLegendFontStyle("'+uid+'", "u")\'></span></span></td><td><span class="di_vc_icon di_vc_icon_color">'+di_coloPicker1(uid, 'dicolor7', '#000000', di_vc_libpath, 'n')+'</span></td></tr></table></td></tr></table></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Legend</td></tr></table></div><div class="di_vcsec_box2"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><table cellpadding="0" cellspacing="0"><tr><td><span class="di_vc_sformaticon di_vc_sformaticon_1" onclick=\'di_setChartStyle("'+uid+'","1")\'></span></td><td><span class="di_vc_sformaticon di_vc_sformaticon_2" onclick=\'di_setChartStyle("'+uid+'","2")\'></span></td><td><span class="di_vc_sformaticon di_vc_sformaticon_3" onclick=\'di_setChartStyle("'+uid+'","3")\'></span></td></tr></table></td><td valign="top" style="padding:0px 5px 0px 5px;"><p><select id="di_vcSelSeries'+uid+'" class="di_vcdrop_box" onchange=\'di_selectSeriesName("'+uid+'", this.selectedIndex)\'></select></p><p><table cellpadding="0" cellspacing="0"><tr><td height="30">'+di_coloPicker1(uid, 'dicolor4', '#FFFFFF', di_vc_libpath, 'y')+'</td><td>&nbsp;</td><td><span class="di_vc_sricon di_vc_sricon_1">'+di_coloPicker1(uid, 'dicolor8', '#FFFFFF', di_vc_libpath, 'n')+'</span></td><td><span class="di_vc_sricon di_vc_sricon_2" onclick=\'onchangeSeriesBorderWidth("'+uid+'",2,"'+chartType+'")\'></span></td><td><span class="di_vc_sricon di_vc_sricon_3" onclick=\'onchangeSeriesStyle("'+uid+'","dot","'+chartType+'")\'></span></td><td><span class="di_vc_sricon di_vc_sricon_4" onclick=\'onchangeSeriesBorderWidth("'+uid+'",0,"'+chartType+'")\'></span></td></tr></table></p></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt" colspan="2">Format</td></tr></table></div></div></div>';

	// for options
	html_data +='<div id="di_vcsubmenu_options'+uid+'" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box"><span>Theme : </span><select onchange=\'di_changeTreeMapTheme("'+uid+'",this.value)\'><option value="randam">Randam</option><option value="blue">Blue</option><option value="red">Red</option></select></div></div></div>';

	// for export
	html_data +='<form id="divcsvglfrm_'+uid+'" method="POST" style="display:none;"><input type="hidden" name="filename" id="filename_'+uid+'"><input type="hidden" name="type" id="type_'+uid+'"><input type="hidden" name="width" id="width_'+uid+'"><input type="hidden" name="svg" id="svg_'+uid+'"></form>';

	html_data +='<div id="di_vcsubmenu_saveexport'+uid+'" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box3" style="width:180px"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><ul class="img_pointr"><li><img src="'+di_vc_libpath+'images/save_gallery.png" width="16px" height="16px" style="padding-right:10px;" id="idicolor1'+uid+'"/><a href="javascript:void(0)" onclick="saveToGalPopUp()" class="di_vcclicklink">Save to gallery</a><div id="divcsvGalpop" class="di_vctogglediv" style="display:none;"><div>Name<br/><input type="text" id="divc_pname'+uid+'" class="di_vcdrop_box"></div><div>Description<br/><textarea id="divc_pdesc'+uid+'" cols="30" rows="4"></textarea></div><br/><div>Tag<br/><input type="text" id="divc_ptag'+uid+'" class="di_vcdrop_box"></div><br/><br/><div style="text-align:right;"><input type="button" value="OK" onclick=\'saveToGallary("'+uid+'", "'+di_vc_libpath+'library/PHP/exporting-server/")\' class="di_vcclickbttn">&nbsp;<input type="button" value="Cancel" onclick="saveToGalPopUp()" class="di_vcclickbttn"></div></div></li><li><img src="'+di_vc_libpath+'images/share_compn.png" width="16px" height="16px" style="padding-right:10px;" id="idicolor1'+uid+'"/><a href="javascript:void(0)" onclick=\'di_shareVisualizer("'+uid+'","'+storagePath+'")\' class="di_vcclicklink">Share</a></li><li><img src="'+di_vc_libpath+'images/print_compn.png" width="16px" height="16px" style="padding-right:10px;" id="idicolor1'+uid+'"/><a href="javascript:void(0)" onclick=\'di_printChart("'+uid+'")\' class="di_vcclicklink">Print</a></li></ul></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Save</td></tr></table></div><div class="di_vcsec_box3" style="width:589px;"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><ul><li style="padding:2px;"><a href="javascript:void(0)" onclick=\'di_exportImage("'+uid+'","image/png","'+chartType+'")\' class="di_vcclicklink">Download PNG Image</a></li><li style="padding:2px;"><a href="javascript:void(0)" onclick=\'di_exportImage("'+uid+'","image/jpeg","'+chartType+'")\' class="di_vcclicklink">Download JPEG Image</a></li><li style="padding:2px;"><a href="javascript:void(0)" onclick=\'di_exportImage("'+uid+'","image/png","'+chartType+'")\' class="di_vcclicklink">Download PDF Document</a></li><li style="padding:2px;"><a href="javascript:void(0)" onclick=\'di_exportImage("'+uid+'","image/svg+xml","'+chartType+'")\' class="di_vcclicklink" class="di_vcclicklink">Download SVG Vector Image</a></li></ul></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Export</td></tr></table></div></div></div>';

	// end for sub menu

	html_data +='<div id="di_visulcontainer_map'+uid+'" style="height:'+visu_height+'px;display:none;"><iframe width="100%" height="100%" scrolling="no" frameborder="0" name="divcframe'+uid+'" id="divcframe'+uid+'" marginheight="0" marginwidth="0" src=""></iframe></div>';
	html_data +='<div style="height:10px;">&nbsp;</div>';
	html_data +='<div style="height:70px;"><table width="100%" heigh="100%" cellpadding="0" cellspacing="0"><tr><td align="left" height="30" width="10%"><img id="di_visulcontainer_back'+uid+'" src="'+di_vc_libpath+'images/di_close_bttn.png" title="Go to parent"/></td><td height="30"><span id="di_vc_title'+uid+'" style="font:normal 11px arial;color:#A0A0A0;fontWeight:normal;textDecoration:none">'+title+'</span></td></tr><tr><td height="30" colspan="2"><span id="di_vc_subtitle'+uid+'" style="font:normal 9px arial;color:#A0A0A0;fontWeight:normal;textDecoration:none">'+subtitle+'</span></td></tr></table></div>';
	html_data +='<div id="di_visulcontainer_chart'+uid+'" style="height:'+(visu_height-100)+'px;width:"'+width+'px;"></div>';

	html_data +='<div id="di_visulcontainer_grid'+uid+'" style="height:'+(visu_height-10)+'px;display:none;overflow:auto;"></div>'; // for grid data
	//html_data +='<div id="di_visulcontainer_map'+uid+'" style="display:none;"></div>';
	html_data +='<div style="height:20px;text-align:left;"><span id="di_vc_source'+uid+'" style="font:normal 9px arial;color:#A0A0A0;fontWeight:normal;textDecoration:none">source</span></div>';
	
	html_data +='<div></div>'; // close div of vc container and inner container

	return html_data;
}


/*Create treemap*/
function di_create_treemap_chart(title,subtitle,sourceText,uid, div_id, treemapdata, chartType) {
	//var lvl_detct = 0;
  //init TreeMap
  var tm = new $jit.TM.Squarified({
    //where to inject the visualization
    injectInto: div_id,
    //parent box title heights
    titleHeight: 15,
    //enable animations
    animate: animate,
	// show level
	levelsToShow:1,
    //box offsets
    offset: 1,	
    //Attach left and right click events
    Events: {
      enable: true,
      onClick: function(node) {		 
        if(node) {
			tm.enter(node);
			/*lvl_detct++;
			if(lvl_detct > 0){
				document.getElementById("di_vc_GoRootbtn").style.display = "";
			}*/
		}
      },
      onRightClick: function() {
        tm.out();
		/*lvl_detct--;
		if(lvl_detct == 0){
			document.getElementById("di_vc_GoRootbtn").style.display = "none";
		}*/
      }
    },
    duration: 1000,
    //Enable tips
    Tips: {
      enable: true,
      //add positioning offsets
      offsetX: 20,
      offsetY: 20,
      //implement the onShow method to
      //add content to the tooltip when a node
      //is hovered
      onShow: function(tip, node, isLeaf, domElement) {
        var html = "<div class=\"tip-title\">" + node.name 
          + "</div><div class=\"tip-text\">";
        var data = node.data;
        if(data.playcount) {
          html += "data value: " + data.playcount;
        }
        if(data.image) {
          html += "<img src=\""+ data.image +"\" class=\"album\" />";
        }
        tip.innerHTML =  html; 
      }  
    },
    //Add the name of the node in the correponding label
    //This method is called once, on label creation.
    onCreateLabel: function(domElement, node){
        domElement.innerHTML = node.name;
        var style = domElement.style;
        style.display = '';
        style.border = '1px solid transparent';
        domElement.onmouseover = function() {
          style.border = '1px solid #9FD4FF';
        };
        domElement.onmouseout = function() {
          style.border = '1px solid transparent';
        };
    }
	// Add canvas label styling
    /*Label: {		
      type: 'HTML', // "Native" or "HTML"
      color: 'red',
    }*/
  });
  tm.loadJSON(treemapdata);
  tm.refresh();

  //end
  //add events to radio buttons
  /*var sq = $jit.id('r-sq'),
      st = $jit.id('r-st'),
      sd = $jit.id('r-sd');
  var util = $jit.util;
  util.addEvent(sq, 'change', function() {
    if(!sq.checked) return;
    util.extend(tm, new $jit.Layouts.TM.Squarified);
    tm.refresh();
  });
  util.addEvent(st, 'change', function() {
    if(!st.checked) return;
    util.extend(tm, new $jit.Layouts.TM.Strip);
    tm.layout.orientation = "v";
    tm.refresh();
  });
  util.addEvent(sd, 'change', function() {
    if(!sd.checked) return;
    util.extend(tm, new $jit.Layouts.TM.SliceAndDice);
    tm.layout.orientation = "v";
    tm.refresh();
  });*/
  //add event to the back button
  var back = $jit.id('di_vc_GoRootbtn');
  $jit.util.addEvent(back, 'click', function() {
    tm.out();
	/*lvl_detct--;
	if(lvl_detct == 0){
		document.getElementById("di_vc_GoRootbtn").style.display = "none";
	}*/
  });

  // Check for dublicate object
	var isUidExist=false;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				isUidExist = true;
				chartCollection[i].chartInstance = tm;
				//chartCollection[i].theme = "randam";
				break;
			}
		}		
	}
	if(!isUidExist)
	{
		var item = new Object();
		item.id = uid;
		item.chartInstance = tm;
		item.theme = "randam";
		item.title = {text:title,style:{font:'normal 15px arial',fontSize:'15px',fontWeight:'normal',color:'#000000',textDecoration:'none'}};
		item.subtitle={text:subtitle,style:{font:'normal 12px arial',fontSize:'12px',fontWeight:'normal',color:'#000000',textDecoration:'none'}};
		item.source={text:sourceText,style:{font:'normal 9px arial',fontSize:'12px',fontWeight:'normal',color:'#000000',textDecoration:'none'}};
		chartCollection.push(item);
	}	
}


function di_changeTreeMapTheme(uid,divId,di_tree_themeValue)
{
	var chartObj = di_getChartObject(uid);
	var jsonData = chartObj.op.viz.json;
	di_setThemeCode(uid,di_tree_themeValue);
	jsonData = di_fillTreemapThemeColor(jsonData,di_tree_themeValue);
	document.getElementById(divId).innerHTML = "";
	di_create_treemap_chart("","","",uid, divId, jsonData, "treemap");
}


function di_fillTreemapThemeColor(treeStructure,di_tree_themeValue)
{
	var di_themeColor;	
	treeStructure.data.$color = di_get_random_color(di_tree_themeValue);
    var childNodes = treeStructure.children;
    if(childNodes.length>0)
    {
        for(var i=0;i<childNodes.length;i++)
        {
            di_fillTreemapThemeColor(childNodes[i],di_tree_themeValue);
        }
    }
    return treeStructure;    
}

/*Increase/Decrease title font size
	Parameters: subtitleobj - title object
				isIncrease - true if increase otherwise false
	*/

function di_increaseTextFontSizeForTree(titleobj,isIncrease)
{
	
	var fontStr = titleobj.style.fontSize;
	var val;
	if(isIncrease)
	{
		val = parseInt(fontStr.substr(0,fontStr.length-2)) + 1 ;		
	}
	else
	{
		val = parseInt(fontStr.substr(0,fontStr.length-2)) - 1 ;
	}
	if(parseInt(val)>=9 && parseInt(val)<=31)
	{
		titleobj.style.fontSize = val + "px";		
	}	
}

/*Change Chart Title Style
		Parameter : treeTitleObj - title object
					styleStr - font style (bold, italic or etc)
					size - font size*/
function di_changeTextStyleForTree(treeTitleObj,styleStr,size)
{	
	switch(styleStr)
	{
		case "b": // Bold
			if(treeTitleObj.style.fontWeight=="bold")
			{
				treeTitleObj.style.fontWeight = "normal";
			}
			else
			{
				treeTitleObj.style.fontWeight = "bold";
			}			
			break;
		case "i": // Italic
			var fontString = treeTitleObj.style.font.split(";");			
			if(fontString[0].indexOf('italic')>-1)
			{
				treeTitleObj.style.font = fontString[0].replace("italic","normal");
			}
			else
			{
				if(fontString[0].indexOf('bold')>-1)
				{
					treeTitleObj.style.font = fontString[0].replace("bold","bold italic");
				}
				else
				{
					treeTitleObj.style.font = "italic " + fontString[0];
				}
			}
			break;
		case "u": // Underline
			if(treeTitleObj.style.textDecoration == "underline")
			{
				treeTitleObj.style.textDecoration = "none";
			}
			else
			{
				treeTitleObj.style.textDecoration = "underline";
			}
			break;
	}		
}


function redrawTreeMap(uid,divId,treeData,title)
{	
	document.getElementById(divId).innerHTML = "";
	var themeCode = di_getThemeCode(uid);
	treeData = di_fillTreemapThemeColor(treeData,themeCode);
	di_create_treemap_chart(title,"","",uid,divId, treeData, "treemap");	
}

/* Return Chart Theme code
		Parameter : uid - chart unique id*/
function di_getThemeCode(uid)
{
	var theme;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				theme = chartCollection[i].theme;
				break;
			}
		}
	}
	return theme;
}

/* Set Chart Theme code
		Parameter : uid - chart unique id*/
function di_setThemeCode(uid,theme)
{
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				chartCollection[i].theme = theme;
				break;
			}
		}
	}	
}

/* Return Treemap title obj
		Parameter : uid - chart unique id
					type - 0(title), 1(subtitle), 2(source)*/

function di_getTreeTitleObj(uid,type)
{
	var title;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				if(type==0)
				{
					title = chartCollection[i].title;
				}
				else if(type==1)
				{
					title = chartCollection[i].subtitle;
				}
				else if(type==2)
				{
					title = chartCollection[i].source;
				}
				break;
			}
		}
	}
	return title;
}

/* Set  Treemap title obj
		Parameter : uid - chart unique id
		title - titlt object
		type - 0(title), 1(subtitle), 2(source)*/
function di_setTreeTitleObj(uid,title,type)
{
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{				
				if(type==0)
				{
					chartCollection[i].title = title;
				}
				else if(type==1)
				{
					chartCollection[i].subtitle = title;
				}
				else if(type==2)
				{
					chartCollection[i].source = title;
				}
				break;
			}
		}
	}	
}