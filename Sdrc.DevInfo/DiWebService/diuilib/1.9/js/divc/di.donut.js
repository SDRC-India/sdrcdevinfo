/*
* di.donut.js
* 
*/

/*Create markup for visualizer container
function to draw donut chart */
function di_draw_donut_chart(title, uid, width, height, div_id, data, datatype, x_title, y_title, allowTabs, is_save, save_url, lngcodefile, isCloseBtn, hotFuncCloseBtn, storagePath, vcType, settingFileUrl, sourceText, DCURL) {

	var subtitle='';
	var visu_height = height - 50;

	var div_obj = document.getElementById(div_id);
//	var di_vc_libpath = 'http://dgps/di7poc/diuilib/1.1/';
	var di_vc_libpath = 'http://www.devinfolive.info/di7beta_1/diwebservices/diuilib/1.1/';

	if(hotFuncCloseBtn==null || hotFuncCloseBtn=='' || lngcodefile==undefined) { 
		hotFuncCloseBtn = 'di_vcclose("'+uid+'")';
	}

	if(lngcodefile=='' || lngcodefile==null || lngcodefile==undefined) {
		lngcodefile = di_vc_libpath + 'library/store/lng.xml';
	}
	//var di_vcLangArray = setLangVars( lngcodefile );

	var visul_div_id = 'di_visulcontainer_chart'+uid;
		
	var html_data = di_getDonutChartMarkup(uid, title, subtitle, x_title,  y_title, width, height, allowTabs, di_vc_libpath, isCloseBtn, hotFuncCloseBtn, storagePath, 'donut', vcType,DCURL);

	div_obj.innerHTML = html_data; // render the html	
	// upload data
	di_jq(document).ready(function() {

		di_createFontSizeDdl('di_vctfontlist'+uid);
		di_createFontSizeDdl('di_vclfontlist'+uid);
		di_createFontSizeDdl('di_vcslfontlist'+uid);	
		
		// calling function to initialize color pickers
		//initColorPickers(uid, 'donut');
		
		di_readDefaultChartSetting(settingFileUrl,title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle,'donut','normal',sourceText);		

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

				/*if(filename=='false') {
					alert("File upload error!");
					di_jq('#di_vcprogress_bar'+uid).hide();
					di_jq('#di_vcupbttn'+uid).attr('disabled', false);
					return false;
				}
				else if(filename=='ValidatingError') {
					di_jq('#di_vcproc1'+uid).removeClass('di_gui_proc_active');
					di_jq('#di_vcproc1'+uid).addClass('di_gui_proc_deactive');

					di_jq('#di_vcproc2'+uid).removeClass('di_gui_proc_deactive');
					di_jq('#di_vcproc2'+uid).addClass('di_gui_proc_active');

					alert("Validating error!");
					di_jq('#di_vcprogress_bar'+uid).hide();
					di_jq('#di_vcupbttn'+uid).attr('disabled', false);
					return false;
				}
				else if(filename=='ParsingError') {
					di_jq('#di_vcproc1'+uid).removeClass('di_gui_proc_active');
					di_jq('#di_vcproc1'+uid).addClass('di_gui_proc_deactive');

					di_jq('#di_vcproc2'+uid).removeClass('di_gui_proc_active');
					di_jq('#di_vcproc2'+uid).addClass('di_gui_proc_deactive');

					di_jq('#di_vcproc3'+uid).removeClass('di_gui_proc_deactive');
					di_jq('#di_vcproc3'+uid).addClass('di_gui_proc_active');

					alert("Parsing error!");
					di_jq('#di_vcprogress_bar'+uid).hide();
					di_jq('#di_vcupbttn'+uid).attr('disabled', false);
					return false;
				}
				else {
					di_jq('#di_vcupbttn'+uid).attr('disabled', true); // disabled upload button
					di_jq('#di_vcproc4'+uid).html('');

					if(vcType == 'map') {
						di_jq('#di_visulcontainer_chart'+uid).hide();
						di_jq('#di_visulcontainer_map'+uid).show();
						di_vcshowmap(di_vc_libpath+'library/', filename, visu_height, uid);
					}
					else if(vcType == 'chart') {
						di_jq('#di_visulcontainer_map'+uid).hide();
						di_jq('#di_visulcontainer_chart'+uid).show();
						di_readDefaultChartSetting(settingFileUrl,title, uid, visul_div_id, filename, 'j.str', x_title, y_title, subtitle,'donut',sourceText);		

//						di_create_donut_chart(title, uid, visul_div_id, filename, 'j.str', x_title, y_title, subtitle);
					}

					di_jq('#di_vcupbttn'+uid).attr('disabled', false); // disabled upload button
					di_jq('#di_vcprogress_bar'+uid).hide();
					di_jq('#di_vcupfile'+uid).val('');
				}*/
			}
		});
		
		} // end for data tab
		/*********** End for upload own data************/
		if(jQuery.inArray('Data', allowTabs) > -1) {
		di_vc_changeTab('gtabm_0'+uid, 'Data', "'"+allowTabs+"'", uid, visu_height);
		}
	});		
}


// function to create donut chart
function di_create_donut_chart(title, uid, div_id, data, datatype, x_title, y_title, subtitle,defaultSettings,chartType,sourceText) 
{
	
	var vc_data=new Object();	
	if(data !="")
	{
		if(datatype=="j.str")
		{
			vc_data = jQuery.parseJSON(data);
			vc_data = di_covertToProperInput(vc_data);
		}
		else
		{
			vc_data = data;
		}
	}
	else
	{
		vc_data.categoryCollection = [];
		vc_data.seriesCollection = [];
	}
	di_createCategoryList(vc_data,'di_vcSelSeries'+uid);	
//	di_createSeriesList(vc_data,'di_vcdonutddl'+uid,true);
	vc_data = di_getInputDataForDonutChart(vc_data);

	// Customize Settings
	defaultSettings = defaultSettings.replace("$renderDivId$",div_id);
	defaultSettings = defaultSettings.replace("$chartTitle$",title);
	defaultSettings = defaultSettings.replace("$chartSubTitle$",subtitle);
	defaultSettings = defaultSettings.replace("$category$",YAHOO.lang.JSON.stringify([]));
	defaultSettings = defaultSettings.replace("$xAxisTitle$",x_title);
	defaultSettings = defaultSettings.replace("$seriesData$",YAHOO.lang.JSON.stringify(vc_data));
	defaultSettings = defaultSettings.replace("$yAxisTitle$",y_title);
	defaultSettings = defaultSettings.replace("$plotOptionId$",YAHOO.lang.JSON.stringify({pie:{shadow:false}}));

	var chartDefSetting = jQuery.parseJSON(defaultSettings);

	chartDefSetting.tooltip.formatter = function() {return '<b>'+this.point.name +': '+ this.y + "</b><br>" + di_replaceAll(this.series.name,"{@@}",", ");};
	chartDefSetting.legend.labelFormatter = function() {return di_replaceAll(this.name,"{@@}",", ");};
	// If there is 1 series then legend should be off
	if(vc_data[0].data.length==1)
	{
		chartDefSetting.legend.enabled = false;
	}
	var sourceStyleObj = di_getStyle("","","","","","");
	var labelStyleObj = di_getStyle("","","","","","");
	var yPosStr = document.getElementById(div_id).style.height;
	yPosStr = parseInt(yPosStr.substr(0,yPosStr.length-2)) - 10;
	var sourceFunObj = di_getTextDrawFunction(sourceText,20,yPosStr,'',50,50,sourceStyleObj,labelStyleObj);
	chart = new Highcharts.Chart(chartDefSetting,sourceFunObj);	
	chart.options.legend.layout="vertical";
	chart = new Highcharts.Chart(chart.options);
	// Check for dublicate chart object
	var isUidExist=false;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				isUidExist = true;
				chartCollection[i].chartInstance = chart;				
				break;
			}
		}		
	}
	if(!isUidExist)
	{
		var item = new Object();
		item.id = uid;
		item.chartInstance = chart;
		item.source = sourceText;
		item.label = "";
		item.sourceStyle = sourceStyleObj;
		item.labelStyle = labelStyleObj;
		item.chartBorderWidth = 1;
		item.legendBorderWidth = 1;
		chartCollection.push(item);		
	}	
	

	
	// Set Visualizer UI
	di_setHideShowUI(uid);
	di_setDonutDataLabelChkbox(uid);
	di_setLegendUI(uid);
	di_setChartAndPlotareaBgUI(uid);
	di_setDropdownlist('di_vcTitlesList'+uid,title);
	di_vcSelTitleVal(title,uid);
	var titleListCtrl = document.getElementById('di_vcTitlesList'+uid);
	di_setChartTitleUI(uid, titleListCtrl);
	di_setDropdownlist('di_vcSelLegend'+uid,'6');
}

/* Show DataLabels
	Parameters: chartObj - chart object
	chartTyle - type (line, donut and bar)*/
function di_showDonutDataLabels(chartObj)
{
	var chartInput = chartObj.options;
	chartInput.series[0].dataLabels.enabled = true;
	chartObj = new Highcharts.Chart(chartInput);
}

/* Hide DataLabels
	Parameters: chartObj - chart object
	chartTyle - type (line, donut and bar)*/
function di_hideDonutDataLabels(chartObj)
{
	var chartInput = chartObj.options;
	chartInput.series[0].dataLabels.enabled = false;
	chartObj = new Highcharts.Chart(chartInput);
}

/*Set datalabels checkbox
	Parameter:	chartObj - chart object*/
function di_setDonutDataLabelChkbox(uid)
{
	var chartObj = di_getChartObject(uid);
	document.getElementById('di_vcpdatalbl'+uid).checked = chartObj.options.series[0].dataLabels.enabled;
}


/*Convert data for Donut Chart*/
function di_getInputDataForDonutChart(vc_data)
{
	var colorArray = ["#4572A7", "#AA4643", "#89A54E", "#80699B", "#3D96AE","#DB843D", "#92A8CD", "#A47D7C", "#B5CA92"];
	var category = vc_data.categoryCollection;
	var seriesCol = vc_data.seriesCollection;
	var donutSeriesCol=[];
	var innersize = 0;
	if(category.length>0)
	{
		for(var i=0;i<seriesCol.length;i++)
		{		
			var series = seriesCol[i];
			if(series!="")
			{
				var donutSeries=new Object();
				donutSeries.type = "pie";
				donutSeries.name = series.name;
				innersize += 15;
				donutSeries.innerSize = innersize + "%";
				var dataValue = [];
				for(var j=0;j<category.length;j++)
				{
					var dataPoint = new Object();
					for(var k=j;k<j+1;k++)
					{
						dataPoint.name = category[k];
					}
					dataPoint.y = series.data[j];
					dataPoint.color = colorArray[j];
					dataValue.push(dataPoint);
				}
				donutSeries.data = dataValue;
				if(i==0)
				{
					donutSeries.dataLabels = {enabled:true,color:"#000000",connectorWidth:1,style:{font:'normal 10px arial',color:'#000000',fontWeight:'normal',textDecoration:'none'}};
					donutSeries.showInLegend = true;
				}
				else
				{
					donutSeries.dataLabels = {enabled:false};
					donutSeries.showInLegend = false;
				}
				donutSeriesCol.push(donutSeries);
			}
		}
	}
	return donutSeriesCol;
}


/*Select color from Series color ddl
	Parameters:	uid - unique chart id
				color - series color*/
function di_selectDonutCategoryColor(uid,color)
{
    var ddlObj = document.getElementById('di_vcSelSeries'+uid);
    var seriesIndex = ddlObj.options[ddlObj.selectedIndex].value;
    if(seriesIndex=="-1")
    {
        alert("Select series name");
        return;
    }
    di_changeDonutCategoryColor(uid,seriesIndex,color);
}
/*Chagne Series Color
		Parameter:	uid - unique chart id
					seriesIndex - selected series index
					color - series color*/
function di_changeDonutCategoryColor(uid,seriesIndex,color)
{
	var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;	
	for(var i=0;i<chartInput.series.length;i++)
	{
		var dataPoints = chartInput.series[i].data;
		dataPoints[seriesIndex].color = color;
	}
    chart = new Highcharts.Chart(chartInput);
}


/* function to create UI part of chart */
function di_getDonutChartMarkup(uid, title, subtitle, x_title, y_title, width, height, allowTabs, di_vc_libpath, isCloseBtn, hotFuncCloseBtn, storagePath, chartType, vcType,DCURL)
{
	
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
		html_data +='<div id="di_vcsubmenu_data'+uid+'" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box2" style="width:772px"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><form name="di_vcfrm1'+uid+'" id="di_vcfrm1'+uid+'" method="POST" action="'+DCURL+'?type='+vcType+'" enctype="multipart/form-data"><table width="90%" cellpadding="0" cellspacing="0"><tr><td valign="top"><p>Upload data XLS file: <input type="file" name="di_vcupfile" id="di_vcupfile'+uid+'" size="28">&nbsp;<input type="hidden" name="di_vctype" id="di_vctype'+uid+'" value="'+vcType+'">&nbsp; <input type="submit" name="di_vcupbttn" id="di_vcupbttn'+uid+'" value="Upload" class="di_vcclickbttn"></p><div id="di_vcprogress_bar'+uid+'" style="display:none;"><div id="di_vcproc1'+uid+'" class="di_gui_proc_deactive">* Uploading data...</div><div id="di_vcproc2'+uid+'" class="di_gui_proc_deactive">* Validating data...</div><div id="di_vcproc3'+uid+'" class="di_gui_proc_deactive">* Preparing data for Map...</div><div id="di_vcproc4'+uid+'" style="height:5px;" class="di_gui_proc_active"></div></div></td><td valign="top" nowrap><ul><li><a class="di_vcclicklink" href="'+di_vc_libpath+'library/store/SampleDataEntrySheet.xls">Download data entry sheet</a></li><li><a class="di_vcclicklink" href="'+di_vc_libpath+'library/store/AreaSheet.xls">Download Area Excel file</a></li></ul><br/><div><a href="javascript:void(0)" onclick=\'di_toggleContainer("'+uid+'","c");setVisualizerData("'+chartType+'")\'>Chart</a> | <a href="javascript:void(0)" onclick=\'di_toggleContainer("'+uid+'","g")\'>Grid</a></div></td></tr></table></form></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Upload Data</td></tr></table></div></div></div>';
	}

	// for settings
	html_data +='<div id="di_vcsubmenu_settings'+uid+'" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><p><input type="checkbox" id="di_vcpborder'+uid+'" onclick=\'di_toggleChartBorder("'+uid+'", this.checked)\'>Border</p><p><input type="checkbox" id="di_vcpdatalbl'+uid+'" onclick=\'di_toggleDatalabels("'+uid+'", this.checked,"'+chartType+'")\'>Data Labels</p><p><input type="checkbox" onclick="">Data Tabs</p></td><td valign="top" style="padding:0px 5px 0px 5px;"><p><input type="checkbox" onclick=\'di_toggleLegendVisibility("'+uid+'",this.checked)\' id="di_vcplegend'+uid+'" onclick="">&nbsp;&nbsp;Legend</p><div><table cellspacing="0"><tr><td>'+di_coloPicker1(uid, 'dicolor1', '#ffffff', di_vc_libpath, 'y')+'</td><td><p>Chart</p></td></tr></table></div><div><table cellspacing="0"><tr><td>'+di_coloPicker1(uid, 'dicolor2', '#ffffff', di_vc_libpath, 'y')+'</td><td><p>Plot Area</p></td></tr></table></div></td></tr><tr><td colspan="2" height="20" class="di_vcsec_box_bttmtxt">General</td></tr></table></div><div class="di_vcsec_box"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><table width=100%" cellpadding="0" cellspacing="0"><tr><td>'+di_vcGetTitles(title, subtitle, x_title, y_title,'Source','Label', uid)+'</td></tr><tr><td height="30"><input type="text" id="di_vctitleval'+uid+'" value="" class="di_vcdrop_box" disabled="true" onchange=\'di_changeTitleText("'+uid+'", this.value)\'/></td></tr><tr><td height="30"><table cellpadding="0" cellspacing="0"><tr><td><select id="di_vctfontlist'+uid+'" onchange=\'di_applyTitltFontSize("'+uid+'",this.value)\' class="di_vcsmalldrop_box"></select>&nbsp</td><td><span class="di_vc_icon di_vc_icon_fontbig" onclick=\'di_applyTitleFontSize("'+uid+'",true)\'></span></td><td><span class="di_vc_icon di_vc_icon_fontsmall" onclick=\'di_applyTitleFontSize("'+uid+'",false)\'></span></td><td><span class="di_vc_icon di_vc_icon_bold" onclick=\'di_applyTitleFontStyle("'+uid+'", "b")\'></span></td><td><span class="di_vc_icon di_vc_icon_italic" onclick=\'di_applyTitleFontStyle("'+uid+'", "i")\'></span></td><td><span class="di_vc_icon di_vc_icon_underline" onclick=\'di_applyTitleFontStyle("'+uid+'", "u")\'></span></td><td><span class="di_vc_icon di_vc_icon_color">'+di_coloPicker1(uid, 'dicolor5', '#FF0500', di_vc_libpath, 'n')+'</span></td></tr></table></td></tr></table></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Titles</td></tr></table></div><div class="di_vcsec_box" style="width:316px"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><table width=100%" cellpadding="0" cellspacing="0"><tr><td><select id="di_vcSelLabel'+uid+'" class="di_vcdrop_box" onchange=\'di_setAxisDatalabelsUI("'+uid+'", this,"'+chartType+'")\'><option value="-1">Select</option><option value="dl">Data Labels</option></select></td></tr><tr><td height="30"><table cellpadding="0" cellspacing="0"><tr><td><select id="di_vclfontlist'+uid+'" onchange=\'di_applyAxisDataLabelFontSize("'+uid+'",this.value,"'+chartType+'")\' class="di_vcsmalldrop_box"></select>&nbsp</td><td><span class="di_vc_icon di_vc_icon_fontbig" onclick=\'di_applyAxisLabelFontSize("'+uid+'",true,"'+chartType+'")\'></span></td><td><span class="di_vc_icon di_vc_icon_fontsmall" onclick=\'di_applyAxisLabelFontSize("'+uid+'",false,"'+chartType+'")\'></span></td><td><span class="di_vc_icon di_vc_icon_bold"   onclick=\'di_applyAxisLabelFontStyle("'+uid+'", "b","'+chartType+'")\'></span></td><td><span class="di_vc_icon di_vc_icon_italic" onclick=\'di_applyAxisLabelFontStyle("'+uid+'", "i","'+chartType+'")\'></span></td><td><span class="di_vc_icon di_vc_icon_underline" onclick=\'di_applyAxisLabelFontStyle("'+uid+'", "u","'+chartType+'")\'></span></td><td><span class="di_vc_icon di_vc_icon_color">'+di_coloPicker1(uid, 'dicolor6', '#FF0500', di_vc_libpath, 'n')+'</span></td></tr></table></td></tr><tr><td height="30"><table cellspacing="0" cellpadding="0"><tr><td height="28"><span class="di_vc_lblicon di_vc_lblicon_1" onclick=\'di_applyAxisDataLabelRotation("'+uid+'","315","'+chartType+'")\'></span></td><td><span class="di_vc_lblicon di_vc_lblicon_2" onclick=\'di_applyAxisDataLabelRotation("'+uid+'","-315","'+chartType+'")\'></span></td><td><span class="di_vc_lblicon di_vc_lblicon_3" onclick=\'di_applyAxisDataLabelRotation("'+uid+'","270","'+chartType+'")\'></span></td><td><span class="di_vc_lblicon di_vc_lblicon_4" onclick=\'di_applyAxisDataLabelRotation("'+uid+'","-270","'+chartType+'")\'></span></td><td><span class="di_vc_lblicon di_vc_lblicon_5" onclick=\'di_applyAxisDataLabelRotation("'+uid+'","360","'+chartType+'")\'></span></td></tr></table></td></tr></table></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Data Labels</td></tr></table></div></div></div>';
	
	// for series
	html_data +='<div id="di_vcsubmenu_series'+uid+'" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><table width=100%" cellpadding="0" cellspacing="0"><tr><td valign="top"><select id="di_vcSelLegend'+uid+'" onchange=\'di_changeLegendPosition("'+uid+'", this.value)\' class="di_vcdrop_box70"><option value="1">Left top</option><option value="2">Left middle</option><option value="3">Left bottom</option><option value="4">Top</option><option value="5">Bottom</option><option value="6">Right top</option><option value="7">Right middle</option><option value="8">Right bottom</option></select></td><td><p><input type="checkbox" id="di_vclborderchk'+uid+'" onclick=\'di_toggleLegendBorder("'+uid+'", this.checked)\'/>&nbsp;&nbsp;Border </p><p><table cellspacing="0"><tr><td>'+di_coloPicker1(uid, 'dicolor3', '#FFFFFF', di_vc_libpath, 'y')+'</td><td><p>Background Color</p></td></tr></table></p></td></tr><tr><td height="30" colspan="2"><table cellpadding="0" cellspacing="0"><tr><td><select id="di_vcslfontlist'+uid+'" onchange=\'di_applyLegendFontSize("'+uid+'",this.value)\' class="di_vcsmalldrop_box"></select>&nbsp</td><td><span class="di_vc_icon di_vc_icon_fontbig" onclick=\'di_applyLegendItemSize("'+uid+'",true)\'></span></td><td><span class="di_vc_icon di_vc_icon_fontsmall" onclick=\'di_applyLegendItemSize("'+uid+'",false)\'></span></td><td><span class="di_vc_icon di_vc_icon_bold"   onclick=\'di_applyLegendFontStyle("'+uid+'", "b")\'></span></td><td><span class="di_vc_icon di_vc_icon_italic" onclick=\'di_applyLegendFontStyle("'+uid+'", "i")\'></span></td><td><span class="di_vc_icon di_vc_icon_underline" onclick=\'di_applyLegendFontStyle("'+uid+'", "u")\'></span></span></td><td><span class="di_vc_icon di_vc_icon_color">'+di_coloPicker1(uid, 'dicolor7', '#000000', di_vc_libpath, 'n')+'</span></td></tr></table></td></tr></table></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Legend</td></tr></table></div><div class="di_vcsec_box2"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><table cellpadding="0" cellspacing="0"><tr><td><span class="di_vc_sformaticon di_vc_sformaticon_1" onclick=\'di_setChartStyle("'+uid+'","1")\'></span></td><td><span class="di_vc_sformaticon di_vc_sformaticon_2" onclick=\'di_setChartStyle("'+uid+'","2")\'></span></td><td><span class="di_vc_sformaticon di_vc_sformaticon_3" onclick=\'di_setChartStyle("'+uid+'","3")\'></span></td></tr></table></td><td valign="top" style="padding:0px 5px 0px 5px;"><p><select id="di_vcSelSeries'+uid+'" class="di_vcdrop_box" onchange=\'di_selectSeriesName("'+uid+'", this.selectedIndex)\'></select></p><p><table cellpadding="0" cellspacing="0"><tr><td height="30">'+di_coloPicker1(uid, 'dicolor4', '#FFFFFF', di_vc_libpath, 'y')+'</td><td>&nbsp;</td><td><span class="di_vc_sricon di_vc_sricon_1">'+di_coloPicker1(uid, 'dicolor8', '#FFFFFF', di_vc_libpath, 'n')+'</span></td><td><span class="di_vc_sricon di_vc_sricon_2" onclick=\'onchangeSeriesBorderWidth("'+uid+'",2,"'+chartType+'")\'></span></td><td><span class="di_vc_sricon di_vc_sricon_3" onclick=\'onchangeSeriesStyle("'+uid+'","dot","'+chartType+'")\'></span></td><td><span class="di_vc_sricon di_vc_sricon_4" onclick=\'onchangeSeriesBorderWidth("'+uid+'",0,"'+chartType+'")\'></span></td></tr></table></p></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt" colspan="2">Format</td></tr></table></div></div></div>';

	// for options
	html_data +='<div id="di_vcsubmenu_options'+uid+'" class="di_gui_navsubmenu" style="display:none;"></div>';

	// for export
	html_data +='<form id="divcsvglfrm_'+uid+'" method="POST" style="display:none;"><input type="hidden" name="filename" id="filename_'+uid+'"><input type="hidden" name="type" id="type_'+uid+'"><input type="hidden" name="width" id="width_'+uid+'"><input type="hidden" name="svg" id="svg_'+uid+'"></form>';

	html_data +='<div id="di_vcsubmenu_saveexport'+uid+'" class="di_gui_navsubmenu" style="display:none;"><div id="main_box"><div class="di_vcsec_box3" style="width:180px"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><ul class="img_pointr"><li><img src="'+di_vc_libpath+'images/save_gallery.png" width="16px" height="16px" style="padding-right:10px;" id="idicolor1'+uid+'"/><a href="javascript:void(0)" onclick="saveToGalPopUp()" class="di_vcclicklink">Save to gallery</a><div id="divcsvGalpop" class="di_vctogglediv" style="display:none;"><div>Name<br/><input type="text" id="divc_pname'+uid+'" class="di_vcdrop_box"></div><div>Description<br/><textarea id="divc_pdesc'+uid+'" cols="30" rows="4"></textarea></div><br/><div>Tag<br/><input type="text" id="divc_ptag'+uid+'" class="di_vcdrop_box"></div><br/><br/><div style="text-align:right;"><input type="button" value="OK" onclick=\'saveToGallary("'+uid+'", "'+di_vc_libpath+'library/PHP/exporting-server/")\' class="di_vcclickbttn">&nbsp;<input type="button" value="Cancel" onclick="saveToGalPopUp()" class="di_vcclickbttn"></div></div></li><li><img src="'+di_vc_libpath+'images/share_compn.png" width="16px" height="16px" style="padding-right:10px;" id="idicolor1'+uid+'"/><a href="javascript:void(0)" onclick=\'di_shareVisualizer("'+uid+'","'+storagePath+'")\' class="di_vcclicklink">Share</a></li><li><img src="'+di_vc_libpath+'images/print_compn.png" width="16px" height="16px" style="padding-right:10px;" id="idicolor1'+uid+'"/><a href="javascript:void(0)" onclick=\'di_printChart("'+uid+'")\' class="di_vcclicklink">Print</a></li></ul></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Save</td></tr></table></div><div class="di_vcsec_box3" style="width:589px;"><table width="100%" height="100%" cellpadding="0" cellspacing="0"><tr><td valign="top" height="99" style="padding:0px 5px 0px 5px;"><ul><li style="padding:2px;"><a href="javascript:void(0)" onclick=\'di_exportImage("'+uid+'","image/png","'+chartType+'")\' class="di_vcclicklink">Download PNG Image</a></li><li style="padding:2px;"><a href="javascript:void(0)" onclick=\'di_exportImage("'+uid+'","image/jpeg","'+chartType+'")\' class="di_vcclicklink">Download JPEG Image</a></li><li style="padding:2px;"><a href="javascript:void(0)" onclick=\'di_exportImage("'+uid+'","image/png","'+chartType+'")\' class="di_vcclicklink">Download PDF Document</a></li><li style="padding:2px;"><a href="javascript:void(0)" onclick=\'di_exportImage("'+uid+'","image/svg+xml","'+chartType+'")\' class="di_vcclicklink" class="di_vcclicklink">Download SVG Vector Image</a></li></ul></td></tr><tr><td height="20" class="di_vcsec_box_bttmtxt">Export</td></tr></table></div></div></div>';

	// end for sub menu

	html_data +='<div id="di_visulcontainer_map'+uid+'" style="height:'+visu_height+'px;display:none;"><iframe width="100%" height="100%" scrolling="no" frameborder="0" name="divcframe'+uid+'" id="divcframe'+uid+'" marginheight="0" marginwidth="0" src=""></iframe></div>';
	//html_data +='<div id="di_visulcontainer_ddl'+uid+'" style=""><select id="di_vcpieddl'+uid+'" onchange=\'di_changePieData("'+uid+'",this.selectedIndex)\'></select></div>';
	html_data +='<div id="di_visulcontainer_chart'+uid+'" style="height:'+(visu_height-10)+'px;"></div>';	

	/*html_data +='<div id="di_visulcontainer_map'+uid+'" style="height:'+visu_height+'px;display:none;"><iframe width="100%" height="100%" scrolling="no" frameborder="0" name="divcframe'+uid+'" id="divcframe'+uid+'" marginheight="0" marginwidth="0" src=""></iframe></div>';
	html_data +='<div style="height:10px;">&nbsp;</div>';
	html_data +='<div id="di_visulcontainer_chart'+uid+'" style="height:'+(visu_height-10)+'px;width:"'+width+'px;"></div>';*/

	html_data +='<div id="di_visulcontainer_grid'+uid+'" style="height:'+(visu_height-10)+'px;display:none;overflow:auto;"></div>'; // for grid data

	
	html_data +='<div></div>'; // close div of vc container and inner container

	return html_data;
}

/*Set Chart Style
            Parameter: uid -  unique chart id
						styleId - style id*/
function di_setDonutChartStyle(uid,styleId)
{
    var defaultStyle=['#4572A7','#AA4643','#89A54E','#80699B','#3D96AE','#DB843D','#92A8CD','#A47D7C','#B5CA92'];
    var style1Color=['#626262','#B3B3B3','#828282','#B3A8A8','#D9D5D5','#EEE9E9','#F8F1F1'];
    var style2Color=['#4F80BC','#BF504D','#9BBB59','#8BD77B','#A0E991','#BDEEB3','#D7FAD0'];
    var style3Color=['#7F9B48','#9BBB59','#C6D6AC','#99CC00','#99CC80','#99FF00','#00FF50','#00FF80','#99CC50'];
    var style4Color=['#FF9900','#FF6600','#FF9900','#FF6699','#FF9999','#FFCC99','#FFCCCC'];
	var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;	
    if(styleId=="1")
    {
		for(var i=0;i<chartInput.series.length;i++)
		{
			var dataPoints = chartInput.series[i].data;
			for(var j=0;j<dataPoints.length;j++)
			{
				dataPoints[j].color = style1Color[j];
			}			
		}        
		chartInput.chart.backgroundColor = "#ffffff";
        chartInput.legend.backgroundColor = "#ffffff";
    }
	if(styleId=="2")
    {
        for(var i=0;i<chartInput.series.length;i++)
		{
			var dataPoints = chartInput.series[i].data;
			for(var j=0;j<dataPoints.length;j++)
			{
				dataPoints[j].color = style2Color[j];
			}			
		}        
		chartInput.chart.backgroundColor = "#ffffff";
        chartInput.legend.backgroundColor = "#ffffff";
    }
	if(styleId=="3")
    {
        for(var i=0;i<chartInput.series.length;i++)
		{
			var dataPoints = chartInput.series[i].data;
			for(var j=0;j<dataPoints.length;j++)
			{
				dataPoints[j].color = style3Color[j];
			}			
		}        
		chartInput.chart.backgroundColor = "#ffffff";
        chartInput.legend.backgroundColor = "#ffffff";
    }
    chart = new Highcharts.Chart(chartInput);
}


function di_selectDonutSeriesName(uid,index)
{
	if(index==0)
	{
		di_setColorDialogBox("dicolor4"+uid,"#ffffff");
		return;
	}
	var chartObj = di_getChartObject(uid);
	var color = di_getSeriesColor(chartObj,index-1);
	if(color==null || color.toLowerCase()=="undefined")
	{
		color = chartObj.options.colors[index-1];
	}
	di_setColorDialogBox("dicolor4"+uid,color);
}




/*Change datalabels color
	Parameter:	chartObj - chart object
				color - font color*/
function di_changeDonutDataLabelColor(uid,color)
{
	var chartObj = di_getChartObject(uid);
	var chartInput = chartObj.options;	
	//chartInput.plotOptions.pie.dataLabels.color = color;
	var styleObj = di_getDonutDataLabelStyle(chartObj);
	styleObj.color = color;
	di_setDonutDataLabelStyle(chartObj,styleObj)	
}

/*Return Datalabel's style*/
function di_getDonutDataLabelStyle(chartObj)
{
	var styleObj;	
	var chartInput = chartObj.options;
	styleObj = chartInput.series[0].dataLabels.style;
	return styleObj;
}

/*Set Datalabel's style*/
function di_setDonutDataLabelStyle(chartObj,styleObj)
{
	var chartInput = chartObj.options;
	chartInput.series[0].dataLabels.style = styleObj;
	chartObj = new Highcharts.Chart(chartInput);
}


function di_changeDonutDataLabelFontStyle(uid,styleStr,size)
{
	var chartObj = di_getChartObject(uid);	
	var styleObj = di_getDonutDataLabelStyle(chartObj);
	if(styleStr=="underline" || styleStr=="none")
	{
		if(styleObj.textDecoration == "underline")
		{
			styleObj.textDecoration = "none";
		}
		else
		{
			styleObj.textDecoration = "underline";
		}
	}	
	else
	{
		if(styleStr == "bold")
		{
			if(styleObj.fontWeight == "bold")
			{
				styleObj.fontWeight = "normal";
			}
			else
			{
				styleObj.fontWeight = styleStr;
			}
		}
		else
		{
			if(styleObj.font.indexOf('italic')>-1)
			{
				styleStr = "normal";
			}
			styleObj.font = styleStr + " " + size + " Arial" ;
		}
	}
	di_setDonutDataLabelStyle(chartObj,styleObj);
}
/* Chart Data Label rotation
		Parameter : uid - chart unique id*/
function di_changeDonutDataLabelRotation(uid,angle)
{
	var chartObj = di_getChartObject(uid);	
	chartObj.options.series[0].dataLabels.rotation = angle;
	chartObj = new Highcharts.Chart(chartObj.options);
}

/*datalabels font size
	Parameter:	chartObj - chart object*/
function di_setPieDataLabelFontSize(uid,val)
{
	var chartObj = di_getChartObject(uid);
	var chartInput = chartObj.options;	
	var fontStr = chartInput.plotOptions.pie.dataLabels.style.font;
	var fontArray = fontStr.split(' ');
	var newFontStr = "";
	newFontStr = fontArray[0]+ " " + val + "px " + fontArray[2];
	chartInput.series[0].dataLabels.style.font = newFontStr;
	chartObj = new Highcharts.Chart(chartInput);
}

/*increase/decrdatalabels font size
	Parameter:	chartObj - chart object*/
function di_togglePieDataLabelFontSize(uid,isIncrease)
{
	var chartObj = di_getChartObject(uid);
	var chartInput = chartObj.options;
	var fontStr = chartInput.series[0].dataLabels.style.font;
	var fontArray = fontStr.split(' ');
	var newFontStr = "";
	var newFont = fontArray[1].substr(0,fontArray[1].length-2);
	if(isIncrease)
	{
		newFont = parseInt(newFont) + 1;
	}
	else
	{
		newFont = parseInt(newFont) - 1;
	}
	newFontStr = fontArray[0]+ " " + newFont + "px " + fontArray[2];
	chartInput.series[0].dataLabels.style.font = newFontStr;
	chartObj = new Highcharts.Chart(chartInput);
}