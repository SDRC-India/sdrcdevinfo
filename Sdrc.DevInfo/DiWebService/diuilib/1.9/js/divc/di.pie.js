/*
* di.pie.js
* 
*/

var pieDataCollection=[];
/*Create markup for visualizer container
function to draw pie chart */
function di_draw_pie_chart(title, uid, width, height, div_id, data, datatype, x_title, y_title, allowTabs, is_save, save_url, lngcodefile, isCloseBtn, hotFuncCloseBtn, storagePath, vcType, settingFileUrl, sourceText, DCURL) {

	var subtitle='';
	var visu_height = height - 50;
	var visu_height_wsmenu = visu_height - 130;

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
		
	var html_data = di_getPieChartMarkup(uid, title, subtitle, x_title,  y_title, width, height, allowTabs, di_vc_libpath, isCloseBtn, hotFuncCloseBtn, storagePath, 'pie', vcType,DCURL);

	div_obj.innerHTML = html_data; // render the html	
	// upload data
	di_jq(document).ready(function() {

		di_createFontSizeDdl('di_vctfontlist'+uid);
		di_createFontSizeDdl('di_vclfontlist'+uid);		
		di_createFontSizeDdl('di_vcslfontlist'+uid);	
		
		// calling function to initialize color pickers
		//initColorPickers(uid, 'pie');
		
		di_readDefaultChartSetting(settingFileUrl,title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle,'pie','normal',sourceText);

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
						di_readDefaultChartSetting(settingFileUrl,title, uid, visul_div_id, filename, 'j.str', x_title, y_title, subtitle,'pie',sourceText);		

//						di_create_pie_chart(title, uid, visul_div_id, filename, 'j.str', x_title, y_title, subtitle);
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


// function to create pie chart
function di_create_pie_chart(title, uid, div_id, data, datatype, x_title, y_title, subtitle,defaultSettings,chartType,sourceText) 
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
			vc_data = di_covertToProperInput(data);
		}
	}
	else
	{
		vc_data.categoryCollection = [];
		vc_data.seriesCollection = [];
	}
	//di_createCategoryList(vc_data,'di_vcSelSeries'+uid);		
	vc_data = di_getInputDataForPieChart(vc_data);

	// Customize Settings
	defaultSettings = defaultSettings.replace("$renderDivId$",div_id);
	defaultSettings = defaultSettings.replace("$chartTitle$",title);
	defaultSettings = defaultSettings.replace("$chartSubTitle$",subtitle);
	defaultSettings = defaultSettings.replace("$category$",JSON.stringify([]));
	defaultSettings = defaultSettings.replace("$xAxisTitle$",x_title);
	defaultSettings = defaultSettings.replace("$seriesData$",JSON.stringify([vc_data[0].seriesData]));
	defaultSettings = defaultSettings.replace("$yAxisTitle$",y_title);
	defaultSettings = defaultSettings.replace("$plotOptionId$",JSON.stringify({pie:{shadow:false,
		allowPointSelect: true,
		cursor: 'pointer',
		dataLabels:
		{
			enabled: true,
			color:"#000000",
			connectorWidth:1,
			style:{font:'normal 10px arial',color:'#000000',fontWeight:'normal',textDecoration:'none',width:'150px'},
			formatter: function() {
			return this.y + ":" + this.y;}
			},
				showInLegend: true}}));

	var chartDefSetting = jQuery.parseJSON(defaultSettings);
	
	chartDefSetting.tooltip.formatter = function() {return '<b>'+this.point.name +': '+ this.y + "</b><br>" + di_replaceAll(this.series.name,"{@@}"," ");};
	chartDefSetting.legend.labelFormatter = function() {
		var tooltipStr  = di_replaceAll(this.name,"{@@}"," ");		
		return tooltipStr;
			};	
	chartDefSetting.plotOptions.pie.dataLabels.formatter = function(){
			return this.point.name + ":" + this.y ;
			}
	// If there is 1 series then legend should be off
	if(vc_data.length>0 && vc_data[0].seriesData.data.length==1)
	{
		chartDefSetting.legend.enabled = false;
	}
	var sourceStyleObj = di_getStyle("","","","","","");
	var labelStyleObj = di_getStyle("","","","","","");
	var yPosStr = document.getElementById(div_id).style.height;
	yPosStr = parseInt(yPosStr.substr(0,yPosStr.length-2));
	var sourceFunObj = di_getTextDrawFunction(sourceText,20,yPosStr,'',50,50,sourceStyleObj,labelStyleObj);
	chartDefSetting.chart.events.load=sourceFunObj;
	chart = new Highcharts.Chart(chartDefSetting);	
	chart.options.legend.layout="vertical";
	chart.options.legend.width = "";
	chart.options.tooltip.style.padding = 5;
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
				pieDataCollection[i].pieChartData = vc_data;				
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
		item.xPos = 50;
		item.yPos = 430;
		item.isYAxisSep = false;
		item.isDataLabelSep = false;
		item.YAxisSepDecimal = 0;
		item.isDataLabelSep = 0;
		item.chartBgcGS = "1";
		item.chartPBgcGS = "1";
		item.chartBorderWidth = 1;
		item.legendBorderWidth = 1;
		item.sourceXPos = 20;
		item.sourceYPos = yPosStr;
		chartCollection.push(item);
		
		var dataitem = new Object();
		dataitem.id = uid;
		dataitem.pieChartData = vc_data;		
		pieDataCollection.push(dataitem);
	}	
	

	/*
	// Set Visualizer UI
	di_setHideShowUI(uid);
	di_setPieDataLabelChkbox(uid);
	di_setLegendUI(uid);
	di_setChartAndPlotareaBgUI(uid);
	di_setDropdownlist('di_vcTitlesList'+uid,title);
	di_vcSelTitleVal(title,uid);
	var titleListCtrl = document.getElementById('di_vcTitlesList'+uid);
	di_setChartTitleUI(uid, titleListCtrl);
	var seriesddlObj = document.getElementById('di_vcpieddl'+uid);
	if(vc_data.length==1 || vc_data.length==0)
	{
		seriesddlObj.style.display ="none";
		document.getElementById('di_visulcontainer_ddl'+uid).style.height = "10px";
	}
	else
	{
		seriesddlObj.style.display ="";
		//document.getElementById('di_visulcontainer_ddl'+uid).style.height = "";
	}
	di_setDropdownlist('di_vcSelLegend'+uid,'6');*/
}

/* Show DataLabels
	Parameters: chartObj - chart object
	chartTyle - type (line, pie and bar)*/
function di_showPieDataLabels(chartObj)
{
	var chartInput = chartObj.options;
	chartInput.plotOptions.pie.dataLabels.enabled = true;
	chartObj = new Highcharts.Chart(chartInput);
}

/* Hide DataLabels
	Parameters: chartObj - chart object
	chartTyle - type (line, pie and bar)*/
function di_hidePieDataLabels(chartObj)
{
	var chartInput = chartObj.options;
	chartInput.plotOptions.pie.dataLabels.enabled = false;
	chartObj = new Highcharts.Chart(chartInput);
}

/*Set datalabels checkbox
	Parameter:	chartObj - chart object*/
function di_setPieDataLabelChkbox(uid)
{
	var chartObj = di_getChartObject(uid);
	document.getElementById('di_vcpdatalbl'+uid).checked = chartObj.options.plotOptions.pie.dataLabels.enabled;
}


/*Convert data for Pie Chart*/
function di_getInputDataForPieChart(vc_data)
{

	var category = vc_data.categoryCollection;
	var seriesCol = vc_data.seriesCollection;
	
	var pieSeriesCol=[];
	if(category.length==0)
	{
		var item =new Object();
		var pieSeries=new Object();		
		pieSeries.type = "pie";
		pieSeries.name = "";
		pieSeries.data = [];
		item.seriesData = pieSeries;
		pieSeriesCol.push(item)
	}
	else
	{
		for(var i=0;i<seriesCol.length;i++)
		{		
			var series = seriesCol[i];
			if(series!="")
			{
				var item =new Object();
				item.seriesName = series.name;
				var pieSeries=new Object();
				pieSeries.type = "pie";
				pieSeries.name = series.name;
				var dataValue = [];
				for(var j=0;j<category.length;j++)
				{
					var dataPoint = [];
					var val = "";
					for(var k=j;k<j+1;k++)
					{						
						dataPoint.push(category[k]);				
					}
					val = series.data[j];
					if (isNaN(val))
					val = null;
					dataPoint.push(val);
					dataValue.push(dataPoint);
				}		
				pieSeries.data = dataValue;
				item.seriesData = pieSeries;
				pieSeriesCol.push(item);
			}
		}
	}
	return pieSeriesCol;
}


/* Return Pie data object 
		Parameter : uid - unique id*/
function di_getPieDataObject(uid)
{
	var pieDataObj;
	if(pieDataCollection.length>0)
	{
		for(var i=0;i<pieDataCollection.length;i++)
		{
			if(pieDataCollection[i].id==uid)
			{
				pieDataObj = pieDataCollection[i].pieChartData;
				break;
			}
		}
	}
	return pieDataObj;
}

/* Set Pie data object 
		Parameter : uid - unique id*/
function di_setPieDataObject(uid,pieDataObj)
{
	if(pieDataCollection.length>0)
	{
		for(var i=0;i<pieDataCollection.length;i++)
		{
			if(pieDataCollection[i].id==uid)
			{
				pieDataCollection[i].pieChartData = pieDataObj;
				break;
			}
		}
	}
}

function di_changePieData(uid,index)
{
	var pieData = di_getPieDataObject(uid);
	var seriesArray = [];
	seriesArray.push(pieData[index].seriesData);
	var chartObj = di_getChartObject(uid);
	var chartInput = chartObj.options;	
	chartInput.series = seriesArray;
	chartObj = new Highcharts.Chart(chartInput);
}


/*Select color from Series color ddl
	Parameters:	uid - unique chart id
				color - series color*/
function di_selectPieCategoryColor(uid,color)
{
    var ddlObj = document.getElementById('di_vcSelSeries'+uid);
    var seriesIndex = ddlObj.options[ddlObj.selectedIndex].value;
    if(seriesIndex=="-1")
    {
        alert("Select series name");
        return;
    }
    di_changePieCategoryColor(uid,seriesIndex,color);
}
/*Chagne Series Color
		Parameter:	uid - unique chart id
					seriesIndex - selected series index
					color - series color*/
function di_changePieCategoryColor(uid,seriesIndex,color)
{
	var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;	
	chartInput.colors[seriesIndex] = color;
    chart = new Highcharts.Chart(chartInput);
}


/* function to create UI part of chart */
function di_getPieChartMarkup(uid, title, subtitle, x_title, y_title, width, height, allowTabs, di_vc_libpath, isCloseBtn, hotFuncCloseBtn, storagePath, chartType, vcType,DCURL)
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
	html_data +='<div id="di_visulcontainer_ddl'+uid+'" style=""><select id="di_vcpieddl'+uid+'" onchange=\'di_changePieData("'+uid+'",this.selectedIndex)\' style="display:none;"></select></div>';
	html_data +='<div id="di_visulcontainer_chart'+uid+'" style="height:'+(visu_height-20)+'px;"></div>';	

	/*html_data +='<div id="di_visulcontainer_map'+uid+'" style="height:'+visu_height+'px;display:none;"><iframe width="100%" height="100%" scrolling="no" frameborder="0" name="divcframe'+uid+'" id="divcframe'+uid+'" marginheight="0" marginwidth="0" src=""></iframe></div>';
	html_data +='<div style="height:10px;">&nbsp;</div>';
	html_data +='<div id="di_visulcontainer_chart'+uid+'" style="height:'+(visu_height-10)+'px;width:"'+width+'px;"></div>';*/

	html_data +='<div id="di_visulcontainer_grid'+uid+'" style="height:'+(visu_height-10)+'px;display:none;overflow:auto;"></div>'; // for grid data

	
	html_data +='<div></div>'; // close div of vc container and inner container

	return html_data;

}


function di_selectPieSeriesName(uid,index)
{
	if(index==0)
	{
		di_setColorDialogBox("dicolor4"+uid,"#ffffff");
		return;
	}
	var chartObj = di_getChartObject(uid);	
	color = chartObj.options.colors[index-1];	
	//di_setColorDialogBox("dicolor4"+uid,color);
}


function getPieChartSettingsObject(chartInput,chartType)
{
	var $plotOptionId$,$seriesData$;
	var newSettings = {
						colors:chartInput.colors,
						chart:{
							renderTo:"$renderDivId$",
							zoomType:"x",
							borderWidth:chartInput.chart.borderWidth,
							borderColor:chartInput.chart.borderColor,
							backgroundColor:chartInput.chart.backgroundColor,
							plotBackgroundColor:chartInput.chart.plotBackgroundColor,
							plotBorderWidth:chartInput.chart.plotBorderWidth,
							plotBorderColor:chartInput.chart.plotBorderColor,
							spacingBottom:chartInput.chart.spacingBottom,
							events:{load:chartInput.chart.events.load}
							},						
						title:{
								text:"$chartTitle$",
								style:{
									font:chartInput.title.style.font,
									color:chartInput.title.style.color,
									fontWeight:chartInput.title.style.fontWeight,
									textDecoration:chartInput.title.style.textDecoration
									}
								},
						subtitle:{
							text:"$chartSubTitle$",
							style:{
									font:chartInput.subtitle.style.font,
									color:chartInput.subtitle.style.color,
									fontWeight:chartInput.subtitle.style.fontWeight,
									textDecoration:chartInput.subtitle.style.textDecoration
									}
								},
						legend:{
								borderWidth: chartInput.legend.borderWidth,
								align: chartInput.legend.align,
								verticalAlign: chartInput.legend.verticalAlign,
								backgroundColor: chartInput.legend.backgroundColor,
								layout: chartInput.legend.layout,
								enabled: chartInput.legend.enabled,
								floating: chartInput.legend.floating,
								itemStyle:{
											font:chartInput.legend.itemStyle.font,
											color:chartInput.legend.itemStyle.color,
											fontWeight:chartInput.legend.itemStyle.fontWeight,
											textDecoration:chartInput.legend.itemStyle.textDecoration
											},
								symbolWidth:chartInput.legend.symbolWidth,
								itemWidth:chartInput.legend.itemWidth,
								labelFormatter:null,
								x:chartInput.legend.x,
								y:chartInput.legend.y
								},
						tooltip:{
								backgroundColor: chartInput.tooltip.backgroundColor,
								borderRadius: chartInput.tooltip.borderRadius,
								borderWidth: chartInput.tooltip.borderWidth,
								formatter:null,
								shared: chartInput.tooltip.shared,
								style:{
									font:chartInput.tooltip.style.font,
									color:chartInput.tooltip.style.color,
									fontWeight:chartInput.tooltip.style.fontWeight,
									textDecoration:chartInput.tooltip.style.textDecoration
									}
								},
						plotOptions:"$plotOptionId$",
						series:"$seriesData$",
						exporting:{enabled:false}
						}

return 	newSettings;
}



/*Change datalabels color
	Parameter:	chartObj - chart object
				color - font color*/
function di_changePieDataLabelColor(uid,color)
{
	var chartObj = di_getChartObject(uid);
	var chartInput = chartObj.options;
	//chartInput.plotOptions.pie.dataLabels.color = color;
	var styleObj = di_getPieDataLabelStyle(chartObj);
	styleObj.color = color;
	di_setPieDataLabelStyle(chartObj,styleObj)	
}

/*Return Datalabel's style*/
function di_getPieDataLabelStyle(chartObj)
{
	var styleObj;	
	var chartInput = chartObj.options;
	styleObj = chartInput.plotOptions.pie.dataLabels.style;
	styleObj.color = chartInput.plotOptions.pie.dataLabels.color;
	return styleObj;
}

/*Set Datalabel's style*/
function di_setPieDataLabelStyle(chartObj,styleObj)
{
	var chartInput = chartObj.options;
	chartInput.plotOptions.pie.dataLabels.style = styleObj;
	chartInput.plotOptions.pie.dataLabels.color = styleObj.color;
	chartObj = new Highcharts.Chart(chartInput);
}


function di_changePieDataLabelFontStyle(uid,styleStr,size)
{
	var chartObj = di_getChartObject(uid);	
	var styleObj = di_getPieDataLabelStyle(chartObj);
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
	di_setPieDataLabelStyle(chartObj,styleObj);
}
/* Chart Data Label rotation
		Parameter : uid - chart unique id*/
function di_changePieDataLabelRotation(uid,angle)
{
	var chartObj = di_getChartObject(uid);	
	chartObj.options.plotOptions.pie.dataLabels.rotation = angle;
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
	newFontStr = fontArray[0]+ " " + val + " " + fontArray[2];
	chartInput.plotOptions.pie.dataLabels.style.font = newFontStr;
	chartObj = new Highcharts.Chart(chartInput);
}

/*increase/decrdatalabels font size
	Parameter:	chartObj - chart object*/
function di_togglePieDataLabelFontSize(uid,isIncrease)
{
	var chartObj = di_getChartObject(uid);
	var chartInput = chartObj.options;	
	var fontStr = chartInput.plotOptions.pie.dataLabels.style.font;
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
	chartInput.plotOptions.pie.dataLabels.style.font = newFontStr;
	chartObj = new Highcharts.Chart(chartInput);
}