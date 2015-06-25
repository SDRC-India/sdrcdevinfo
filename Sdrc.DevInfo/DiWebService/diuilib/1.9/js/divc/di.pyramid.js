// function to create pyramid chart
function di_create_pyramid_chart(title,uid,div_id,data,datatype,x_title,y_title,subtitle,defaultSettings,chartType,chartCategory,sourceText)
{
	var vc_data=new Object();
	if(data!="")
	{
		if(datatype=="j.str")
		{
			vc_data=jQuery.parseJSON(data);
			vc_data=di_covertToProperInput(vc_data);
		}
		else
		{
			vc_data=di_covertToProperInput(data);
		}
	}
	else
	{
		vc_data.categoryCollection=[];
		vc_data.seriesCollection=[];
	}
	var pyaramidData=di_makeDataForPyramid(vc_data);	
	defaultSettings=defaultSettings.replace("$renderDivId$",div_id);
	defaultSettings=defaultSettings.replace("$chartTitle$",title);
	defaultSettings=defaultSettings.replace("$chartSubTitle$",subtitle);
	defaultSettings=defaultSettings.replace("$category1$",JSON.stringify(pyaramidData.categoryCollection));
	defaultSettings=defaultSettings.replace("$category2$",JSON.stringify(pyaramidData.categoryCollection));
	defaultSettings=defaultSettings.replace("$xAxisTitle1$",x_title);
	defaultSettings=defaultSettings.replace("$xAxisTitle2$",x_title);
	defaultSettings=defaultSettings.replace("$seriesData$",JSON.stringify(pyaramidData.seriesCollection));
	defaultSettings=defaultSettings.replace("$yAxisTitle$",y_title);
	var chartDefSetting=jQuery.parseJSON(defaultSettings);
	chartDefSetting.plotOptions.series.dataLabels.formatter = function () { return Math.abs(this.y); };
//	chartDefSetting.xAxis[0].labels.formatter = function () { return Math.abs(this.value); };
//	chartDefSetting.xAxis[1].labels.formatter = function () { return Math.abs(this.value); };
	chartDefSetting.yAxis.labels.formatter = function () {
	    return Math.abs(this.value);	    
	};
	chartDefSetting.tooltip.formatter=function(){return'<b>'+this.x+': '+Math.abs(this.y)+"</b><br>"+di_replaceAll(this.series.name,"{@@}"," ");};
	chartDefSetting.legend.labelFormatter=function(){return di_replaceAll(this.name,"{@@}"," ");};
	var totalDataLabels=0;
	totalDataLabels=pyaramidData.categoryCollection.length;
	var totalLegendItems=pyaramidData.seriesCollection.length;
	chartDefSetting=di_labelRotationSettings(totalDataLabels,totalLegendItems,chartDefSetting,chartType);
	var sourceStyleObj=di_getStyle("","","","","","");
	var labelStyleObj=di_getStyle("","","","","","");
	var yPosStr=document.getElementById(div_id).style.height;
	yPosStr=parseInt(yPosStr.substr(0,yPosStr.length-2));
	var sourceFunObj=di_getTextDrawFunction(sourceText,20,yPosStr,'',50,50,sourceStyleObj,labelStyleObj);	 
	chartDefSetting.chart.events.load = sourceFunObj;
	var allVal=getAllValues(pyaramidData.seriesCollection);
	var maxVal=allVal.max();
	chartDefSetting.yAxis.min = 0-(maxVal +200);
	chartDefSetting.yAxis.max = maxVal+200;
	//chartDefSetting.tooltip.style.padding=5;
	var chart=new Highcharts.Chart(chartDefSetting);
	chart.options.legend.layout="vertical";		
	chart.options.tooltip.style.padding=5;
	chart.options.legend.width=null;
	//chart.options.yAxis.labels.formatter = function(){return this.value;}
	//alert(chart.options.tooltip.style.padding);	
	chart=new Highcharts.Chart(chart.options);
	//alert(chart.options.tooltip.style.padding);	
	var isUidExist=false;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				isUidExist=true;
				chartCollection[i].chartInstance=chart;
				break;
			}
		}
	}
	if(!isUidExist)
	{
		var item=new Object();
		item.id=uid;
		item.chartInstance=chart;
		item.source=sourceText;
		item.label="";
		item.sourceStyle=sourceStyleObj;
		item.labelStyle=labelStyleObj;
		item.xPos=50;
		item.yPos=430;
		item.isYAxisSep=false;
		item.isDataLabelSep=false;
		item.YAxisSepDecimal=0;
		item.isDataLabelSep=0;
		item.chartBgcGS="1";
		item.chartPBgcGS="1";
		item.chartBorderWidth=1;
		item.legendBorderWidth=1;
		item.sourceXPos=20;
		item.sourceYPos=yPosStr;
		item.isCommaSeperator = false;
		chartCollection.push(item);
	}
}
function di_makeDataForPyramid(vc_data)
{
	var data=new Object();
	if(vc_data.seriesCollection.length>1)
	{
		data.categoryCollection=vc_data.categoryCollection;
		data.seriesCollection=[];
		var seriesObject=new Object();
		seriesObject.name=vc_data.seriesCollection[0].name;
		seriesObject.data=[];
		for(var i=0;i<vc_data.seriesCollection[0].data.length;i++)
		{
			var dataValue=vc_data.seriesCollection[0].data[i];
			var value=null;
			if(dataValue!=null&&!isNaN(dataValue))
			value=0-dataValue;
			seriesObject.data.push(value);
		}
	seriesObject.dataLabels={enabled:true,align:'left',x:null,y:null,color:'#000000',style:{font:'normal 10px arial',color:'#000000',fontWeight:'normal',textDecoration:'none'}};
	data.seriesCollection.push(seriesObject);
	vc_data.seriesCollection[1].dataLabels={enabled:true,align:'right',x:15,y:null,color:'#000000',style:{font:'normal 10px arial',color:'#000000',fontWeight:'normal',textDecoration:'none'}};
	data.seriesCollection.push(vc_data.seriesCollection[1]);
	}
	return data
}


/* Show DataLabels
	Parameters: chartObj - chart object	*/
function di_showPyramidDataLabels(chartObj)
{
	var chartInput = chartObj.options;
	chartInput.series[0].dataLabels.enabled = true;
	chartInput.series[1].dataLabels.enabled = true;
	chartObj = new Highcharts.Chart(chartInput);
}

/* Hide DataLabels
	Parameters: chartObj - chart object*/
function di_hidePyramidDataLabels(chartObj)
{
	var chartInput = chartObj.options;
	chartInput.series[0].dataLabels.enabled = false;
	chartInput.series[1].dataLabels.enabled = false;
	chartObj = new Highcharts.Chart(chartInput);
}

/*Set datalabels checkbox
	Parameter:	chartObj - chart object*/
function di_setPyramidDataLabelChkbox(uid)
{
	var chartObj = di_getChartObject(uid);
	document.getElementById('di_vcpdatalbl'+uid).checked = chartObj.options.plotOptions.bar.dataLabels.enabled;
}


/*Change datalabels color
	Parameter:	chartObj - chart object
				color - font color*/
function di_changePyramidDataLabelColor(uid,color)
{
	var chartObj = di_getChartObject(uid);
	var chartInput = chartObj.options;	
	//chartInput.plotOptions.column.dataLabels.color = color;
	var styleObj = di_getPyramidDataLabelStyle(chartObj);
	styleObj.color = color;
	di_setPyramidDataLabelStyle(chartObj,styleObj)	
}

/*Return Datalabel's style*/
function di_getPyramidDataLabelStyle(chartObj)
{
	var styleObj;	
	var chartInput = chartObj.options;
	styleObj = chartInput.series[0].dataLabels.style;
	styleObj.color = chartInput.series[0].dataLabels.color;
	return styleObj;
}

/*Set Datalabel's style*/
function di_setPyramidDataLabelStyle(chartObj,styleObj)
{
	var chartInput = chartObj.options;
	chartInput.series[0].dataLabels.style = chartInput.series[1].dataLabels.style = styleObj;
	chartInput.series[0].dataLabels.color = chartInput.series[1].dataLabels.color = styleObj.color;
	chartObj = new Highcharts.Chart(chartInput);
}


function di_changePyramidDataLabelFontStyle(uid,styleStr,size)
{
	var chartObj = di_getChartObject(uid);	
	var styleObj = di_getPyramidDataLabelStyle(chartObj);
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
	di_setPyramidDataLabelStyle(chartObj,styleObj);
}
/* Chart Data Label rotation
		Parameter : uid - chart unique id*/
function di_changePyramidDataLabelRotation(uid,angle)
{
	var chartObj = di_getChartObject(uid);	
	chartObj.options.series[0].dataLabels.rotation = angle;
	chartObj.options.series[1].dataLabels.rotation = angle;
	chartObj = new Highcharts.Chart(chartObj.options);
}

/*datalabels font size
	Parameter:	chartObj - chart object*/
function di_setPyramidDataLabelFontSize(uid,val)
{
	var chartObj = di_getChartObject(uid);
	var chartInput = chartObj.options;	
	var fontStr = chartInput.series[0].dataLabels.style.font;
	var fontArray = fontStr.split(' ');
	var newFontStr = "";
	newFontStr = fontArray[0]+ " " + val + " " + fontArray[2];
	chartInput.series[0].dataLabels.style.font = chartInput.series[1].dataLabels.style.font = newFontStr;
	chartObj = new Highcharts.Chart(chartInput);
}

/*increase/decrdatalabels font size
	Parameter:	chartObj - chart object*/
function di_togglePyramidDataLabelFontSize(uid,isIncrease)
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
	chartInput.series[0].dataLabels.style.font  = chartInput.series[1].dataLabels.style.font = newFontStr;
	chartObj = new Highcharts.Chart(chartInput);
}