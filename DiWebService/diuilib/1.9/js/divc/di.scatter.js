/*
* di.scatter.js
* 
*/

// function to create scatter chart
function di_create_scatter_chart(title, uid, div_id, data, datatype, x_title, y_title, subtitle,defaultSettings,chartType,chartCategory,sourceText) 
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
	// Customize Settings
	
	var scatterData = di_makeScatterPlotData(vc_data,0,1);	
	//var plotingOption = {scatter:{marker:{radius:5,states:{hover:{enabled:true,lineColor:'rgb(100,100,100)'}}},states:{hover:{marker:{enabled: false}}}}};
	var plotingOption = {scatter:{marker:{radius:5,states:{hover:{enabled:true,lineColor:'rgb(100,100,100)'}}}}};
	// Customize Settings
	defaultSettings = defaultSettings.replace("$renderDivId$",div_id);
	defaultSettings = defaultSettings.replace("$chartTitle$",title);
	defaultSettings = defaultSettings.replace("$chartSubTitle$",subtitle);
	//defaultSettings = defaultSettings.replace("$category$",JSON.stringify(vc_data.categoryCollection));
	defaultSettings = defaultSettings.replace("$xAxisTitle$",x_title);
	defaultSettings = defaultSettings.replace("$seriesData$",JSON.stringify(scatterData));
	defaultSettings = defaultSettings.replace("$yAxisTitle$",y_title);
	defaultSettings = defaultSettings.replace("$plotOptionId$",JSON.stringify(plotingOption));
	var chartDefSetting = jQuery.parseJSON(defaultSettings);
	chartDefSetting.tooltip.formatter = function() {return '<b>'+this.x +': '+ this.y + "</b><br>" + di_replaceAll(this.series.name,"{@@}"," ");};
	chartDefSetting.legend.labelFormatter = function() {return di_replaceAll(this.name,"{@@}"," ");};
	chartDefSetting.symbols=['circle','circle','circle','circle','circle','circle'];
	var totalDataLabels = 0;
	totalDataLabels = vc_data.categoryCollection.length;
	var totalLegendItems = vc_data.seriesCollection.length;
	chartDefSetting = di_labelRotationSettings(totalDataLabels,totalLegendItems,chartDefSetting,chartType);	
	var sourceStyleObj = di_getStyle("","","","","","");
	var labelStyleObj = di_getStyle("","","","","","");
	var yPosStr = document.getElementById(div_id).style.height;
	yPosStr = parseInt(yPosStr.substr(0,yPosStr.length-2)) - 10;
	var sourceFunObj = di_getTextDrawFunction(sourceText,20,yPosStr,'',50,50,sourceStyleObj,labelStyleObj);
	chartDefSetting.chart.events.load = sourceFunObj;
	chart = new Highcharts.Chart(chartDefSetting);	
	chart.options.legend.layout="vertical";
	chart.options.legend.width = "";
	chart.options.tooltip.style.padding = 5;
	chart.options.yAxis.labels.formatter = function(){return this.value;}
	chart = new Highcharts.Chart(chart.options);	
	// Check for dublicate object
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
		item.isCommaSeperator = false;
		chartCollection.push(item);	
	}			
}

/* Show DataLabels
	Parameters: chartObj - chart object
	chartTyle - type (line, scatter and bar)*/
function di_showScatterDataLabels(chartObj)
{
	var chartInput = chartObj.options;
	chartInput.plotOptions.scatter.dataLabels.enabled = true;
	chartObj = new Highcharts.Chart(chartInput);
}

/* Hide DataLabels
	Parameters: chartObj - chart object
	chartTyle - type (line, scatter and bar)*/
function di_hideScatterDataLabels(chartObj)
{
	var chartInput = chartObj.options;
	chartInput.plotOptions.scatter.dataLabels.enabled = false;
	chartObj = new Highcharts.Chart(chartInput);
}

/*Set datalabels checkbox
	Parameter:	chartObj - chart object*/
function di_setColumnDataLabelChkbox(uid)
{
	var chartObj = di_getChartObject(uid);
	document.getElementById('di_vcpdatalbl'+uid).checked = chartObj.options.plotOptions.scatter.dataLabels.enabled;
}


/*Change datalabels color
	Parameter:	chartObj - chart object
				color - font color*/
function di_changeScatterDataLabelColor(uid,color)
{
	var chartObj = di_getChartObject(uid);
	var chartInput = chartObj.options;	
	//chartInput.plotOptions.scatter.dataLabels.color = color;
	var styleObj = di_getScatterDataLabelStyle(chartObj);
	styleObj.color = color;
	di_setColumnDataLabelStyle(chartObj,styleObj)	
}

/*Return Datalabel's style*/
function di_getScatterDataLabelStyle(chartObj)
{
	var styleObj;	
	var chartInput = chartObj.options;
	styleObj = chartInput.plotOptions.scatter.dataLabels.style;
	styleObj.color = chartInput.plotOptions.scatter.dataLabels.color;
	return styleObj;
}

/*Set Datalabel's style*/
function di_setColumnDataLabelStyle(chartObj,styleObj)
{
	var chartInput = chartObj.options;
	chartInput.plotOptions.scatter.dataLabels.style = styleObj;
	chartInput.plotOptions.scatter.dataLabels.color = styleObj.color;
	chartObj = new Highcharts.Chart(chartInput);
}


function di_changeScatterDataLabelFontStyle(uid,styleStr,size)
{
	var chartObj = di_getChartObject(uid);	
	var styleObj = di_getScatterDataLabelStyle(chartObj);
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
	di_setColumnDataLabelStyle(chartObj,styleObj);
}
/* Chart Data Label rotation
		Parameter : uid - chart unique id*/
function di_changeScatterDataLabelRotation(uid,angle)
{
	var chartObj = di_getChartObject(uid);	
	chartObj.options.plotOptions.scatter.dataLabels.rotation = angle;
	chartObj = new Highcharts.Chart(chartObj.options);
}

/*datalabels font size
	Parameter:	chartObj - chart object*/
function di_setScatterDataLabelFontSize(uid,val)
{
	var chartObj = di_getChartObject(uid);
	var chartInput = chartObj.options;	
	var fontStr = chartInput.plotOptions.scatter.dataLabels.style.font;
	var fontArray = fontStr.split(' ');
	var newFontStr = "";
	newFontStr = fontArray[0]+ " " + val + "px " + fontArray[2];
	chartInput.plotOptions.scatter.dataLabels.style.font = newFontStr;
	chartObj = new Highcharts.Chart(chartInput);
}

/*increase/decrdatalabels font size
	Parameter:	chartObj - chart object*/
function di_toggleScatterDataLabelFontSize(uid,isIncrease)
{
	var chartObj = di_getChartObject(uid);
	var chartInput = chartObj.options;	
	var fontStr = chartInput.plotOptions.scatter.dataLabels.style.font;
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
	chartInput.plotOptions.scatter.dataLabels.style.font = newFontStr;
	chartObj = new Highcharts.Chart(chartInput);
}


function di_makeScatterPlotData(vc_data,xIndex,yIndex)
{
	var scatterPlotData=[];
	if(vc_data.seriesCollection.length>=2)
	{
		var xPoint = vc_data.seriesCollection[xIndex];
		var yPoint = vc_data.seriesCollection[yIndex];
		for(var k=0;k<vc_data.categoryCollection.length;k++)
		{
			var scatterSeriesObj = new Object();
			scatterSeriesObj.name = vc_data.categoryCollection[k];
			//scatterSeriesObj.color = di_get_random_color('randam');
			var scatterSeriesData=[];					
			var scaterItem = [];
			scaterItem.push(xPoint.data[k]);
			scaterItem.push(yPoint.data[k]);
			//scaterItem.marker = {radius:5,fillColor:di_get_random_color('randam')};
			scatterSeriesData.push(scaterItem);
			scatterSeriesObj.data = scatterSeriesData;
			scatterPlotData.push(scatterSeriesObj);
		}
	}
	return scatterPlotData;
}

/* Return scatter data
		Parameter : uid - chart unique id*/
function di_getScatterDataCollection(uid)
{
	var data;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				data = chartCollection[i].data;
				break;
			}
		}
	}
	return data;
}

function di_changeScaterPlotData(uid)
{
	var di_x = document.getElementById('di_vc_xPoint'+uid);
	var di_y = document.getElementById('di_vc_yPoint'+uid);
	var di_z = document.getElementById('di_vc_size'+uid);
	if(di_x.options[di_x.selectedIndex].value!="-1" && di_y.options[di_y.selectedIndex].value != "-1" && di_z.options[di_z.selectedIndex].value != "-1")
	{
		if(di_x.options[di_x.selectedIndex].value != di_y.options[di_y.selectedIndex].value && di_y.options[di_y.selectedIndex].value!=di_z.options[di_z.selectedIndex].value && di_z.options[di_z.selectedIndex].value != di_x.options[di_x.selectedIndex].value)
		{
			var chartObj = di_getChartObject(uid);
			var scaterRowData = di_getScatterDataCollection(uid);
			di_updateScatterData(chartObj,scaterRowData,di_x.options[di_x.selectedIndex].value,di_y.options[di_y.selectedIndex].value,di_z.options[di_z.selectedIndex].value);
		}
	}
}

function di_updateScatterData(chartObj,rowData,di_xPoint,di_yPoint,di_zPoint)
{
	var xPoint = vc_data.seriesCollection[di_xPoint];
	var yPoint = vc_data.seriesCollection[di_yPoint];
	var zPoint = vc_data.seriesCollection[di_zPoint];
	for(var i=0;i<chartObj.series[0].data.length;i++)
	{
		chartObj.series[0].data[i].update({x:xPoint[i],y:yPoint[i],marker:{radius:2,fillColor:di_get_random_color('randam')}});
	}
}

