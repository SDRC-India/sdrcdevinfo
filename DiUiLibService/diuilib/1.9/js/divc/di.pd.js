/*Create category dropdownlist 
	Parameters:		data - chart data collection
					ddlId - dropdownlist id*/
function di_createCategoryList(data,ddlId)
{
	var ddlIdObj = document.getElementById(ddlId);
	di_clearDdlItems(ddlId);
	var catergoryArray = data.categoryCollection;
	ddlIdObj.options[0]=new Option("Select Series","-1");
	if(catergoryArray.length>0)
	{
		for(var i=0;i<catergoryArray.length;i++)
		{
			var itemName = catergoryArray[i];
			ddlIdObj.options[i+1]=new Option(itemName,i);
		}
	}
}


/*Create series dropdownlist 
	Parameters:		data - chart data collection
					ddlId - dropdownlist id*/
function di_createSeriesList(data,ddlId,isInner)
{
	var ddlIdObj = document.getElementById(ddlId);
	di_clearDdlItems(ddlId);
	var seriesArray = data.seriesCollection;
	if(isInner)
	{		
		for(var i=0;i<seriesArray.length;i++)
		{
			var series = seriesArray[i];
			var itemName = series.name;
			ddlIdObj.options[i]=new Option(itemName,i);
		}
	}
	else
	{
		ddlIdObj.options[0]=new Option("Select Series","-1");
		for(var i=0;i<seriesArray.length;i++)
		{
			var series = seriesArray[i];
			var itemName = series.name;
			ddlIdObj.options[i+1]=new Option(itemName,i);
		}
	}
	
}


/* convert string series data collection into integer collection 
	Parameter: data - collection*/

function di_covertToProperInput(data)
{
	var seriesArray = data.seriesCollection;
	for(var i=0;i<seriesArray.length;i++)
	{
		var series = seriesArray[i];
		var yData = series.data;
		for(var j=0;j<yData.length;j++)
		{
			yData[j] = parseFloat(yData[j]);
		}
	}	
	return data;
}



/* Return Style Object
	Parameters:	hc_fontFamily - fontFamily
				hc_fontSize - font size
				hc_fontColor - font color
				hc_fontStyle - font style*/
function di_getStyle(hc_fontFamily,hc_fontSize,hc_fontColor,hc_fontStyle)
{
    var style;    
    style={	
            color:hc_fontColor,
			font:hc_fontStyle + " " + hc_fontSize + " " + hc_fontFamily,
			fontWeight:'normal'
            };
    return style;
}

/*Return default legend object*/
function di_getLegendObject()
{
    var chartLegend = {
                        align: "center",
                        backgroundColor: "#ffffff",
                        borderColor: "#909090",
                        borderRadius: 5,
                        borderWidth: 1,
                        enabled: true,
                        floating: false,                        
                        itemWidth: null,
                        layout: "horizontal",                        
                        lineHeight: 8,
                        margin: 15,
                        reversed: false,
                        shadow: false,
                        itemStyle: {},
                        symbolPadding: 5,
                        symbolWidth: 10,
                        verticalAlign: "bottom",
                        width: null,
                        x:0,
                        y:0
                        };
    return chartLegend;
}


/*Return tooltip object*/
function di_getToolTipObject()
{
    var tooltip={
                    backgroundColor: "rgba(255, 255, 255, .85)",                    
                    borderRadius: 5,
                    borderWidth: 2,
                    formatter:null,
                    shadow: false,
                    shared: false,
                    snap: 10/25,
					style:{
						padding:"5px"
					}
                    }
    return tooltip;
}



/* Get rendered chart object
	Parameter: chartObj - chart object
	Return: rendered chart object */
function di_getRenderedChartObject(chartObj)
{
	var chartInput = chartObj.options;
	return chartInput.chart;
}

/* Set chart backGround-Color
	Parameter:  chartObj - chart object
				rederdedChart - rendered chart object*/
function di_setRenderedChartObject(chartObj,rederdedChart)
{
	var chartInput = chartObj.options;
	chartInput.chart = rederdedChart;
	chartObj = new Highcharts.Chart(chartInput);
}


/* Get chart backGround-Color
	Parameter: chartObj - chart object
	Return: background-color value */
function di_getChartBgColor(chartObj)
{
	var chartInput = chartObj.options;
	var color = chartInput.chart.backgroundColor;
	return color;
}

/* Set chart backGround-Color
	Parameter:  chartObj - chart object
				background-color*/
function di_setChartBgColor(chartObj,color)
{
	var chartInput = chartObj.options;
	chartInput.chart.backgroundColor = color;
	chartObj = new Highcharts.Chart(chartInput);
}

/* Get chart's ploting area backGround-Color
	Parameter: chartObj - chart object
	Return: background-color value */
function di_getChartPlotAreaColor(chartObj)
{
	var chartInput = chartObj.options;
	var color = chartInput.chart.plotBackgroundColor;
	return color;
}

/* Set chart's ploting area backGround-Color
	Parameter:  chartObj - chart object
				background-color*/
function di_setChartPlotAreaColor(chartObj,color)
{
	var chartInput = chartObj.options;
	chartInput.chart.plotBackgroundColor = color;
	chartObj = new Highcharts.Chart(chartInput);
}

/* Get chart's border
	Parameter: chartObj - chart object
	Return: border width */
function di_getChartBorder(chartObj)
{
	var chartInput = chartObj.options;
	var borderWidth = chartInput.chart.borderWidth;
	return borderWidth;
}

/* Set chart's border
	Parameter:  chartObj - chart object
				border width */
function di_setChartBorder(chartObj,borderWidth)
{
	var chartInput = chartObj.options;
	chartInput.chart.borderWidth = borderWidth;
	chartObj = new Highcharts.Chart(chartInput);
}


/* Get Legend's border
	Parameter: chartObj - chart object
	Return: border width */
function di_getLegendBorder(chartObj)
{
	var chartInput = chartObj.options;
	var borderWidth = chartInput.legend.borderWidth;
	return borderWidth;
}

/* Set Legend's border
	Parameter:  chartObj - chart object
				border width */
function di_setLegendBorder(chartObj,borderWidth)
{
	var chartInput = chartObj.options;
	chartInput.legend.borderWidth = borderWidth;
	chartObj = new Highcharts.Chart(chartInput);
}


/* Set chart title style
	Parameter:	cahrtObj - chart object
				styleObj - style object*/
function di_setChartTitleStyle(chartObj,styleObj)
{
	var chartInput = chartObj.options;
	chartInput.title.style = styleObj;
	chartObj = new Highcharts.Chart(chartInput);
}

/* Get chart title style
	Parameter:	cahrtObj - chart object
				styleObj - style object*/
function di_getChartTitleStyle(chartObj)
{
	var chartInput = chartObj.options;
	var styleObj = chartInput.title.style;
	return styleObj;
}


/* Set chart title style
	Parameter:	cahrtObj - chart object
				styleObj - style object*/
function di_setChartSubTitleStyle(chartObj,styleObj)
{
	var chartInput = chartObj.options;
	chartInput.subtitle.style = styleObj;
	chartObj = new Highcharts.Chart(chartInput);
}

/* Get chart title style
	Parameter:	chartObj - chart object*/
function di_getChartSubTitleStyle(chartObj)
{
	var chartInput = chartObj.options;
	var styleObj = chartInput.subtitle.style;
	return styleObj;
}


/*Get Chart Legend style 
	Parameter:	chartObj - chart object
	return:		style - legend style object*/
function di_getLegendStyle(chartObj)
{
	var chartInput = chartObj.options;
	return chartInput.legend.style;
}
/*Set Chart Legend style
	Parameter:	chartObj - chart object
				styleObj - legend style object*/
function di_setLegendStyle(chartObj,styleObj)
{
	var chartInput = chartObj.options;
	chartInput.legend.style = styleObj;
	chartObj = new Highcharts.Chart(chartInput);
}


/*Get Chart xAxis 
	Parameter:	chartObj - chart object
	return:		legend - legend object*/
function di_getChartLegendObject(chartObj)
{
	var chartInput = chartObj.options;
	return chartInput.legend;
}

/*Set Chart xAxis 
	Parameter:	chartObj - chart object
				legendObj - legend object*/
function di_setChartLegendObject(chartObj,legendObj)
{
	var chartInput = chartObj.options;
	chartInput.legend = legendObj;
	chartObj = new Highcharts.Chart(chartInput);
}

/*Set Series Color
	Parameters:	chartObj - chart object
				seriesIndex - series index
				color - series color*/
function di_setSeriesColor(chartObj,seriesIndex,color)
{
    var chartInput = chartObj.options;
    chartInput.series[seriesIndex].color = color;
    chartObj = new Highcharts.Chart(chartInput);
}

/*Set Series Color
	Parameters:	chartObj - chart object
				seriesIndex - series index
				colorObj - series color*/
function di_getSeriesColor(chartObj,seriesIndex)
{
    var chartInput = chartObj.options;
    var colorObj = chartInput.series[seriesIndex].color;
    return colorObj;
}

/*Share custumize presentation with other.
This function store chart information at server and return a url to user*/
function di_shareVisualizer(uid, storagePath)
{
	var chartObj = di_getChartObject(uid);
    var chartInput = chartObj.options;
    var url;
    var InputParam  = JSON.stringify(chartInput);
    var htmlResp = di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: {'callback':14,'param1':InputParam,'param2':storagePath},
            async:false,
            success: function(data){
                try
                {
                    url = di_getBaseURL() + "stock/shared/sharedPresentation.html?id="+data;
					SocialSharing("You should check this out!","DevInfo", url);
                }
                catch(ex){
                    alert("Error : " + ex.message);
                }
            },
            error:function(){
                alert("Error occured");
            },
            cache: false
    });
}
/*Get data from server on pageload for sharedlink*/
function di_getChartSettingsFromServer()
{
	var storagePath = "stock/shared/Presentation/";
    var qs = window.location.search.substring(1); //QueryString
    var InputParam = qs.split("=")[1]; // key    
    var chartInput;
    var htmlResp = jQuery.ajax({
            type: "POST",
            url: "../../libraries/aspx/Callback.aspx",
            data: {'callback':15,'param1':InputParam,'param2':storagePath},
            async:false,
            success: function(data){                              
                try
                {                  
                      chartInput = jQuery.parseJSON(data);
                      var chart = new Highcharts.Chart(chartInput);                                                          
                }
                catch(ex){
                    alert("Error : " + ex.message);
                }
            },
            error:function(){
                alert("Error occured");
            },
            cache: false
    });
}
/*Return base url*/
function di_getBaseURL()
{
    var url = window.location.href;
    var index=0;
    var count=-1;
    var target=3;
    for(var i=0;i<url.length;i++)
    {
        var ch = url.substr(i,1);
        if(ch=="/")
        {
            count++;
        }
        if(count==target)
        {
            index=i;
            break;   
        }
    } 
    url = url.substr(0,index+1);
    return url;
}

/* Change Chart Background-color 
`		Parameter : uid - chart unique id*/
function di_changeChartBgColor(uid,color)
{
	var chartObj = di_getChartObject(uid);
	di_setChartBgColor(chartObj,color);
}

/* Change Chart Background-color 
		Parameter : uid - chart unique id*/
function di_changePlotBgColor(uid,color)
{
	var chartObj = di_getChartObject(uid);
	di_setChartPlotAreaColor(chartObj,color);
}

/* Change Chart Border 
		Parameter : uid - chart unique id
					isShow - ture if show border otherwise false*/
function di_toggleChartBorder(uid,isShow)
{
	var chartObj = di_getChartObject(uid);
	if(isShow)
	{
		di_setChartBorder(chartObj,1);
	}
	else
	{
		di_setChartBorder(chartObj,0);
	}
}

/* Change Chart Border 
		Parameter : uid - chart unique id
					isShow - ture if show border otherwise false*/
function di_toggleLegendVisibility(uid,isShow)
{
	var chartObj = di_getChartObject(uid);
	var legendObj;
	legendObj = di_getChartLegendObject(chartObj);
	legendObj.enabled = isShow;	
	di_setChartLegendObject(chartObj,legendObj);	
}

/* Toggle GridLines
		Parameter : uid - chart unique id
					isShow - ture if show border otherwise false*/
function di_toggleGridLine(uid,isShow)
{
	var chartObj = di_getChartObject(uid);
	if(isShow)
	{
		di_showGridLine(chartObj);
	}
	else
	{
		di_hideGridLine(chartObj);
	}
}

/* Hide/Show Datalabels
		Parameter : uid - chart unique id
					isShow - ture if show border otherwise false*/
function di_toggleDatalabels(uid,isShow, type)
{
	var chartObj = di_getChartObject(uid);
	if(isShow)
	{
		switch(type)
		{
			case "pie":
				di_showPieDataLabels(chartObj);
				break;
			case "donut":
				di_showDonutDataLabels(chartObj);
				break;
		}
	}
	else
	{
		switch(type)
		{
			case "pie":
				di_hidePieDataLabels(chartObj);
				break;
			case "donut":
				di_hideDonutDataLabels(chartObj);
				break;
		}
	}
}
/*Change Chart Title Style
		Parameter : uid - chart unique id
					styleStr - font style (bold, italic or etc)
					size - font size*/
function di_changeTitleStyle(uid,styleStr,size)
{
	var chartObj = di_getChartObject(uid);
	var styleObj = di_getChartTitleStyle(chartObj);
	switch(styleStr)
	{
		case "b": // Bold
			if(styleObj.fontWeight=="bold")
			{
				styleObj.fontWeight = "normal";
			}
			else
			{
				styleObj.fontWeight = "bold";
			}			
			break;
		case "i": // Italic
			if(styleObj.font.indexOf('italic')>-1)
			{
				styleObj.font = "normal "+ size +" Arial";
			}
			else
			{
				styleObj.font = "italic "+ size +" Arial";
			}
			break;
		case "u": // Underline
			if(styleObj.textDecoration == "underline")
			{
				styleObj.textDecoration = "none";
			}
			else
			{
				styleObj.textDecoration = "underline";
			}
			break;
	}	
	di_setChartTitleStyle(chartObj,styleObj);
}

/*Change Chart Sub Title Style
		Parameter : uid - chart unique id
					styleStr - font style (bold, italic or etc)
					size - font size*/
function di_changeSubTitleStyle(uid,styleStr,size)
{
	var chartObj = di_getChartObject(uid);
	var styleObj = di_getChartSubTitleStyle(chartObj);
	switch(styleStr)
	{
		case "b": // Bold
			if(styleObj.fontWeight=="bold")
			{
				styleObj.fontWeight = "normal";
			}
			else
			{
				styleObj.fontWeight = "bold";
			}			
			break;
		case "i": // Italic
			if(styleObj.font.indexOf('italic')>-1)
			{
				styleObj.font = "normal "+ size +" Arial";
			}
			else
			{
				styleObj.font = "italic "+ size +" Arial";
			}
			break;
		case "u": // Underline
			if(styleObj.textDecoration == "underline")
			{
				styleObj.textDecoration = "none";
			}
			else
			{
				styleObj.textDecoration = "underline";
			}
			break;
	}	
	di_setChartSubTitleStyle(chartObj,styleObj);
}


/*Change Chart Title Font Size
	Parameter:	uid - unique chart id
				val - font size*/
function di_changeTitleFontSize(uid,val)
{
	var chartObj = di_getChartObject(uid);
	var styleObj = di_getChartTitleStyle(chartObj);
	var fontStr = styleObj.font.split(" ");
	styleObj.font = fontStr[0] +" "+ val + " Arial";
	di_setChartTitleStyle(chartObj,styleObj);
}

/*Change Title Text of Chart/Subtitle/X-axis/Y-axis
	Parameter:	uid - unique chart id
				val - font size*/
function di_changeTitleText(uid, txtVal)
{ 
	var titleType = di_jq('#di_vcTitlesList'+uid).val();
	if(titleType!=-1) {
		if(titleType=='Column Chart') titleType=1;
		if(titleType=='Sub title') titleType=2;		
		var chartObj = di_getChartObject(uid);
		var titleObj = di_getTitleObj(chartObj, titleType);
		titleObj.text = txtVal;
		di_setTitleObj(chartObj, titleObj, titleType);
	}
}
/* Get title object
	Parameter:	cahrtObj - chart object
				titleType - title type*/
function di_getTitleObj(chartObj, titleType)
{
	var chartInput = chartObj.options;
	var titleObj;
	if(titleType==1) { // chart title
		titleObj = chartInput.title;
	}
	else if(titleType==2) { // sub title
		titleObj = chartInput.subtitle;
	}	
	return titleObj;
}


/* Set title object
	Parameter:	cahrtObj - chart object
				styleObj - style object
				titleType - title type*/
function di_setTitleObj(chartObj, titleObj, titleType)
{
	var chartOutput = chartObj.options;
	if(titleType==1) { // chart title
		chartOutput.title = titleObj;
	}
	else if(titleType==2) { // sub title
		chartOutput.subtitle = titleObj;
	}	
	chartObj = new Highcharts.Chart(chartOutput);
}

/*Change Chart SubTitle Font Size
	Parameter:	uid - unique chart id
				val - font size*/
function di_changeSubTitleFontSize(uid,val)
{
	var chartObj = di_getChartObject(uid);
	var styleObj = di_getChartSubTitleStyle(chartObj);
	var fontStr = styleObj.font.split(" ");
	styleObj.font = fontStr[0] +" "+ val + " Arial";
	di_setChartSubTitleStyle(chartObj,styleObj);
}

/* Chart title font color
		Parameter : uid - chart unique id
					color - font color*/
function di_changeTitleFontColor(uid,color)
{
	var chartObj = di_getChartObject(uid);
	var styleObj = di_getChartTitleStyle(chartObj);
	styleObj.color = color;
	di_setChartTitleStyle(chartObj,styleObj);
}

/* Chart sub title font color
		Parameter : uid - chart unique id
					color - font color*/
function di_changeSubTitleFontColor(uid,color)
{
	var chartObj = di_getChartObject(uid);
	var styleObj = di_getChartSubTitleStyle(chartObj);
	styleObj.color = color;
	di_setChartSubTitleStyle(chartObj,styleObj);
}

/* export chart image 
	Parameters:	uid - unique chart id
				imgType - image type*/
function di_exportImage(uid,imgType)
{
	var chartObj = di_getChartObject(uid);
	chartObj.exportChart({type: imgType});
}

/*Apply Title Font Style
	Parameters:	uid - unique chart id
				styleStr - font style(bold,italic etc*/
function di_applyTitleFontStyle(uid,styleStr)
{
	var index = document.getElementById('di_vcTitlesList'+uid).selectedIndex;
	if(index==0)
		return;
	var size = document.getElementById('di_vctfontlist'+uid).options[document.getElementById('di_vctfontlist'+uid).selectedIndex].value;
	switch(index-1)
	{
		case 0:
			di_changeTitleStyle(uid,styleStr,size);
			break;
		case 1:
			di_changeSubTitleStyle(uid,styleStr,size);
			break;		
	}
}
/*Apply Title Font Color
	Parameters:	uid - unique chart id
				color - font color*/
function di_applyTitleFontColor(uid,color)
{
	var index = document.getElementById('di_vcTitlesList'+uid).selectedIndex;
	if(index==0)
		return;
	switch(index-1)
	{
		case 0:
			di_changeTitleFontColor(uid,color); // chart title
			break;
		case 1:
			di_changeSubTitleFontColor(uid,color); // sub title font color
			break;		
	}
}

/*Apply Title Font Color
	Parameters:	uid - unique chart id
				val - font size*/
function di_applyTitltFontSize(uid,val)
{
	var index = document.getElementById('di_vcTitlesList'+uid).selectedIndex;
	if(index==0)
		return;
	switch(index-1)
	{
		case 0:
			di_changeTitleFontSize(uid,val); // chart title
			break;
		case 1:
			di_changeSubTitleFontSize(uid,val); // sub title font color
			break;		
	}
}



/*Toggle Legend border
	Parameters:	uid - unique chart id
				isShow - ture if show legend border otherwise false*/
function di_toggleLegendBorder(uid,isShow)
{
	var chartObj = di_getChartObject(uid);
	if(isShow)
	{
		di_setLegendBorder(chartObj,1); // show
	}
	else
	{
		di_setLegendBorder(chartObj,0); // hide
	}
}


/*Apply legend font size
	Parameters:	uid - unique chart id
				val - font size*/
function di_applyLegendFontSize(uid,val)
{
	var chartObj = di_getChartObject(uid);
	var legendObj = di_getChartLegendObject(chartObj);
	legendObj.itemStyle.fontSize = val;
	var fontStr = legendObj.itemStyle.font.split(" ");
	legendObj.itemStyle.font = fontStr[0] +" "+ val + " Arial";
	di_setChartLegendObject(chartObj,legendObj);
}

/*Apply legend font color
	Parameters:	uid - unique chart id
				color - font color*/
function di_applyLegendFontColor(uid,color)
{
	var chartObj = di_getChartObject(uid);
	var legendObj = di_getChartLegendObject(chartObj);
	legendObj.itemStyle.color = color;
	di_setChartLegendObject(chartObj,legendObj);
}
/*Apply legend font style
	Parameters:	uid - unique chart id
				style - font style*/
function di_applyLegendFontStyle(uid,styleStr)
{
	var chartObj = di_getChartObject(uid);
	var legendObj = di_getChartLegendObject(chartObj);	
	var size = document.getElementById('di_vcslfontlist'+uid).options[document.getElementById('di_vcslfontlist'+uid).selectedIndex].value;

	if(styleStr=="b")
	{
		if(legendObj.itemStyle.fontWeight=="bold")
		{
			legendObj.itemStyle.fontWeight = "normal";
		}
		else
		{
			legendObj.itemStyle.fontWeight = "bold";
		}		
	}
	else if(styleStr=="i")
	{
		if(legendObj.itemStyle.font.indexOf('italic')>-1)
		{
			legendObj.itemStyle.font = "normal "+ size + " Arial";
		}
		else
		{
			legendObj.itemStyle.font = "italic "+ size + " Arial";
		}
	}
	else if(styleStr=="u")
	{
		if(legendObj.itemStyle.textDecoration == "underline")
		{
			legendObj.itemStyle.textDecoration = "none";
		}
		else
		{
			legendObj.itemStyle.textDecoration = "underline";
		}		
	}	
	di_setChartLegendObject(chartObj,legendObj);
}


/*Apply legend background color
	Parameters:	uid - unique chart id
				color - Background color*/
function di_applyLegendBgColor(uid,color)
{
	var chartObj = di_getChartObject(uid);
	var legendObj = di_getChartLegendObject(chartObj);
	legendObj.backgroundColor = color
	di_setChartLegendObject(chartObj,legendObj);
}

/*Print chart object
	Parameters:	uid - unique chart id*/
function di_printChart(uid)
{
	var chartObj = di_getChartObject(uid);
	chartObj.print();
}


/* Increase/ Decrease Title Font size one by one
		Parameter : uid - chart unique id
					isIncrease  - true if increase otherwise false*/
function di_applyTitleFontSize(uid,isIncrease)
{
	var index = document.getElementById('di_vcTitlesList'+uid).selectedIndex;
	if(index==0)
	{
		return;
	}
	switch(index-1)
	{
		case 0:
			di_increaseTitleFontSize(uid,isIncrease);
			break;
		case 1:
			di_increaseSubTitleFontSize(uid,isIncrease);
			break;		
	}
}

/*Increase/ Decrease Chart Title Font Size one by one
		Parameter : uid - chart unique id
					isIncrease  - true if increase otherwise false*/
function di_increaseTitleFontSize(uid,isIncrease)
{
	var chartObj = di_getChartObject(uid);
	var styleObj = di_getChartTitleStyle(chartObj);
	var fontStr = styleObj.font.split(" ");	
	var val;
	if(isIncrease)
	{
		val = parseInt(fontStr[1].substr(0,fontStr[1].length-2)) + 1 ;		
	}
	else
	{
		val = parseInt(fontStr[1].substr(0,fontStr[1].length-2)) - 1 ;
	}
	if(parseInt(val)>=9 && parseInt(val)<=31)
	{
		styleObj.font = fontStr[0] +" "+ val + "px Arial";
		di_setChartTitleStyle(chartObj,styleObj);
		di_setDropdownlist('di_vctfontlist'+uid,val+"px");
	}
}

/*Increase/ Decrease Chart SubTitle Font Size one by one
		Parameter : uid - chart unique id
					isIncrease  - true if increase otherwise false*/
		
function di_increaseSubTitleFontSize(uid,isIncrease)
{
	var chartObj = di_getChartObject(uid);
	var styleObj = di_getChartSubTitleStyle(chartObj);
	var fontStr = styleObj.font.split(" ");
	var val;
	if(isIncrease)
	{
		val = parseInt(fontStr[1].substr(0,fontStr[1].length-2)) + 1 ;		
	}
	else
	{
		val = parseInt(fontStr[1].substr(0,fontStr[1].length-2)) - 1 ;
	}
	if(parseInt(val)>=9 && parseInt(val)<=31)
	{
		styleObj.font = fontStr[0] +" "+ val + "px Arial";
		di_setChartSubTitleStyle(chartObj,styleObj);
		di_setDropdownlist('di_vctfontlist'+uid,val+"px");
	}
}



/* Increase/ Decrease Legend item font size on by one
		Parameter : uid - chart unique id
					isIncrease  - true if increase otherwise false*/
function di_applyLegendItemSize(uid,isIncrease)
{
	var chartObj = di_getChartObject(uid);
	var legendObj = di_getChartLegendObject(chartObj);
	var fontStr = legendObj.itemStyle.font.split(" ");
	var val;
	if(isIncrease)
	{
		val = parseInt(fontStr[1].substr(0,fontStr[1].length-2)) + 1 ;		
	}
	else
	{
		val = parseInt(fontStr[1].substr(0,fontStr[1].length-2)) - 1 ;
	}	
	if(parseInt(val)>=9 && parseInt(val)<=31)
	{
		legendObj.itemStyle.font = fontStr[0] +" "+ val + "px Arial";
		di_setChartLegendObject(chartObj,legendObj);
		di_setDropdownlist('di_vcslfontlist'+uid,val+"px");
	}
}


/* Return Chart object 
		Parameter : uid - chart unique id*/
function di_getChartObject(uid)
{
	var chartObj;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				chartObj = chartCollection[i].chartInstance;
				break;
			}
		}
	}
	return chartObj;
}

/*Create markup for fontsize dropdown list*/
function di_createFontSizeDdl(ddlId)
{
    var ddlObject=document.getElementById(ddlId);
    di_clearDdlItems(ddlId);
    for(var i=9;i<32;i++)
    {
        ddlObject.options[i-9]=new Option(i,i+"px");
    }
}

/*Clear all items from dropdown*/
function di_clearDdlItems(ddlId)
{
    var ddlObj = document.getElementById(ddlId);
    if(ddlObj.options.length>0)
    {
        for(var i=0;i<ddlObj.options.length;i++)
        {            
            ddlObj.options[i] = null;
        }
    }
}


/* Chart title & subtitle font color
		Parameter : uid - chart unique id*/
function di_changeLegendPosition(uid,position)
{
	var chartObj = di_getChartObject(uid);
	var legendObj = di_getChartLegendObject(chartObj);
	legendObj.x = 0;
	legendObj.y = 0;
	switch(position.toLowerCase())
	{
		case "left":
			legendObj.align = "left";
			legendObj.verticalAlign ="middle";
			legendObj.layout="vertical";
			break;
		case "right":
			legendObj.align = "right";
			legendObj.verticalAlign ="middle";
			legendObj.layout="vertical";
			break;
		case "top":
			legendObj.align = "center";
			legendObj.verticalAlign ="top";
			legendObj.layout="horizontal";
			break;
		case "bottom":
			legendObj.align = "center";
			legendObj.verticalAlign ="bottom";
			legendObj.layout="horizontal";
		case "default":
			chartlegend.align = "right";
			legendObj.verticalAlign ="top";
			legendObj.layout="vertical";
			break;
	}	
	di_setChartLegendObject(chartObj,legendObj);
}

/*************************************Set Visualizer UI************************************************/
 /*Set UI value related to Title div
	Parameter: uid - unique chart id
				ddlObj - dropdownlist*/
function di_setChartTitleUI(uid,ddlObj)
{
	if(ddlObj.selectedIndex==0)
	{
		return;
	}
	var chartObj = di_getChartObject(uid);
	var styleObj;
	switch(ddlObj.selectedIndex-1)
	{
		case 0: // Chart title
			styleObj = di_getChartTitleStyle(chartObj);
			break;
		case 1: // Chart subtitle
			styleObj = di_getChartSubTitleStyle(chartObj);
			break;		
	}
	var fontsize = styleObj.font.split(" ")[1];	
	di_setDropdownlist('di_vctfontlist'+uid,fontsize);
	di_setColorDialogBox('dicolor5'+uid,styleObj.color);
	
}


/* Set color in color dialog box
	Parameter:	control id - dialog box's input id
				color - color*/
function di_setColorDialogBox(ctrlId,color)
{
	var ctrlObj = document.getElementById(ctrlId);
	ctrlObj.style.color = color;
	ctrlObj.style.backgroundColor = color;
}
/*Set UI values related to Background Div
	Parameter: uid - unique chart id*/
function di_setChartAndPlotareaBgUI(uid)
{
	var chartObj = di_getChartObject(uid);
	var renderChartObj = di_getRenderedChartObject(chartObj);
	di_setColorDialogBox('dicolor1'+uid,renderChartObj.backgroundColor);
	di_setColorDialogBox('dicolor2'+uid,renderChartObj.plotBackgroundColor);
}

/*Set UI values related to Hide/Show div
	Parameter: uid - unique chart id*/

function di_setHideShowUI(uid)
{
	var chartObj = di_getChartObject(uid);
	di_setChartBorderChkbox(uid,chartObj);
//	di_setGridlineChkbox(uid,chartObj);	
	di_setLegendChkbox(uid,chartObj);
	di_setLegendBorderChkbox(uid,chartObj);
}


/*Set border checkbox
	Parameter:	chartObj - chart object*/
function di_setChartBorderChkbox(uid,chartObj)
{
	var border = di_getChartBorder(chartObj);
	if(border==0)
	{
		document.getElementById('di_vcpborder'+uid).checked = false;
	}
	else
	{
		document.getElementById('di_vcpborder'+uid).checked = true;
	}
}
/*Set Legend checkbox
	Parameter:	chartObj - chart object*/
function di_setLegendChkbox(uid,chartObj)
{
	var legendObj = di_getChartLegendObject(chartObj);
	if(legendObj.enabled)
	{
		document.getElementById('di_vcplegend'+uid).checked = true;
	}
	else
	{
		document.getElementById('di_vcplegend'+uid).checked = false;
	}
}

/*Set Legend Border checkbox
	Parameter:	chartObj - chart object*/
function di_setLegendBorderChkbox(uid,chartObj)
{
	var legendObj = di_getChartLegendObject(chartObj);
	if(legendObj.borderWidth==0)
	{
		document.getElementById('di_vclborderchk'+uid).checked = false;
	}
	else
	{
		document.getElementById('di_vclborderchk'+uid).checked = true;
	}
}



 /*Set UI value related to Legend div
	Parameter: uid - unique chart id
				ddlObj - dropdownlist*/
function di_setLegendUI(uid)
{
	var chartObj = di_getChartObject(uid);
	var legendObj = di_getChartLegendObject(chartObj);
	var fontsize = legendObj.itemStyle.font.split(" ")[1];
	di_setDropdownlist('di_vcslfontlist'+uid,fontsize);
	di_setColorDialogBox('dicolor7'+uid,legendObj.itemStyle.color);	
}



 /*Select item into dropdownlist using item value*/
 function di_setDropdownlist(ddlId,value)
 {
    var ddlObj = document.getElementById(ddlId);    
    for(i=0;i<ddlObj.options.length;i++)
    {
        if(ddlObj.options[i].value == value)
        {
            ddlObj.options[i].selected = true;
        }
        else
        {
            ddlObj.options[i].selected = false;
        }
    }
    return;
 }

/* function to reset height of the chart */
function di_resetChartUIHeight(uid, newHeight)
{
	var chartObj = di_getChartObject(uid);
	var renderChartObj = di_getRenderedChartObject(chartObj);
	renderChartObj.height = newHeight;
	di_setRenderedChartObject(chartObj, renderChartObj);
}




/* function to close visulization component if not passed */
function di_vcclose(uid) { 
	di_jq('#di_vccontainer'+uid).hide('slow');
}

/* function to create color picker */
function di_coloPicker(uid, fieldid, value, onchangefunc, width, height) {
	var return_ui;
	var onchangeui;
	if(onchangefunc!='') {
		onchangeui = "onchange=" + onchangefunc+"('"+uid+"',this.value)";
	}
	//return_ui = '<input readonly id="'+fieldid+'" class="color {valueElement:\''+fieldid+'\',styleElement:\''+fieldid+'\',slider:true,hash:true}" value="'+value+'" style="width:25px;height:25px;border:1px solid #aaa;" '+onchangeui+'>';
	return_ui = '<input readonly id="'+fieldid+uid+'" value="'+value+'" style="width:'+width+'px;height:'+height+'px;border:1px solid #D1D1D1;" '+onchangeui+'>';
	return return_ui;
}

// function to click on tab
function di_vc_changeTab(id, tabref, allowTabs, uid, vcHeight) {
	var allowTabsBrk = allowTabs.split(",");
	if(allowTabsBrk.length > 0 ) {
		for(var i=0; i<allowTabsBrk.length; i++) { 
			var tabref2 = allowTabsBrk[i].replace(" & ", "");
			if(tabref2!=tabref)
			di_jq('#di_vcsubmenu_'+tabref2.toLowerCase()+uid).hide();
		}
	}
	var existCl = di_jq('#'+id).attr('class');
	//var vcHeight = di_jq('#di_visulcontainer_chartcl').height();
	var vcMenuHeight = 130;
	if(existCl.indexOf("di_gui_activeTab") > -1) {
		di_jq('#di_vcsubmenu_'+tabref.toLowerCase()+uid).hide();
		di_jq('#'+id).removeClass('di_gui_activeTab');
		di_jq('#di_visulcontainer_chart'+uid).css('height', vcHeight);
		di_resetChartUIHeight(uid, vcHeight);
	}
	else {
		di_jq('#di_vcglobalnav'+uid+' a').removeClass('di_gui_activeTab');
		di_jq('#'+id).addClass('di_gui_activeTab');
		di_jq('#di_vcsubmenu_'+tabref.toLowerCase()+uid).show();
		di_jq('#di_visulcontainer_chart'+uid).css('height', vcHeight-vcMenuHeight);
		di_resetChartUIHeight(uid, vcHeight-vcMenuHeight);
	}
	//di_jq('#di_vcsubmenu_'+tabref.toLowerCase()+uid).slideToggle();
}

/* function to get flash object */
function di_vcgetMyApp(appName) {
    if (navigator.appName.indexOf ("Microsoft") !=-1) {
       return window[appName];
    } else {
      return document[appName];
    }
}

/* function to show map object */
function di_vcshowmap(vc_libpath, xml_file_name, height, uid) {

	if(xml_file_name!='' && xml_file_name!=null) {
		
		var data_file_path = vc_libpath + "store/mapdata/data/"+ xml_file_name + ".xml"; 

		var swffilepath = vc_libpath + 'store/mapdata/mapxml/MapPresentation.swf';
		var swfid='di_visulcontainer_map'+uid;
		var params = {mappath: vc_libpath + 'store/mapdata/mapxml/', datapath: data_file_path, allowFullScreen: 'true', wmode: 'transparent' };
		var fullscren = {allowFullScreen: 'true', wmode: 'transparent' };
		swfobject.embedSWF(swffilepath, swfid, '100%', height-130, '8.0.0', {}, params, fullscren);

		//di_vcgetMyApp("di_visulcontainer").areaChangeEvent(data_file_path);
	}
}
/* function to get vc title select menu */
function di_vcGetTitles(title, subtitle, xtitle, ytitle, uid) {
	var ret_ui;
	ret_ui = '<p><select id="di_vcTitlesList'+uid+'" onchange=\'di_vcSelTitleVal(this.value, "'+uid+'"); di_setChartTitleUI("'+uid+'", this);\' class="di_vcdrop_box"><option value="-1">Select Title</option><option value="'+title+'">Chart Title</option><option value="'+subtitle+'">Sub Title</option></select></p>';

	return ret_ui;
}
/* function to set title label in text field */
function di_vcSelTitleVal(val, uid) {
	if(val!=-1) {
		di_jq('#di_vctitleval'+uid).attr('disabled', false);
		di_jq('#di_vctitleval'+uid).val(val);
	}
	else {
		di_jq('#di_vctitleval'+uid).val('');
		di_jq('#di_vctitleval'+uid).attr('disabled', true);
	}
}

/* function to parse language code file */
function setLangVars( codefile ) {

	var divcLngArry = new Array();

	try
	{ 
		di_jq.ajax({
			type: "GET",
			url: codefile,
			dataType: "xml",
			async:false,
			success: function(xml) { 
				di_jq(xml).find('lng').each(function(){ 
					
					var key = di_jq(this).attr("key");
					var val = di_jq(this).attr("val"); 

					divcLngArry[key] = val;
					
				});
			}
		});	
	}
	catch (err) {}
	
	return divcLngArry;
}



function di_readDefaultChartSetting(fileUrl,title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle,chartType)
{
	di_jq.get(fileUrl,function(defaultSettings){
		switch(chartType)
		{
			case "column":
				di_create_column_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle,defaultSettings,chartType);
				break;
			case "line":
				di_create_line_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle,defaultSettings,chartType);
				break;
			case "bar":
				di_create_bar_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle,defaultSettings,chartType);
				break;
			case "stack":
				di_create_stack_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle,defaultSettings,chartType);
				break;
			case "area":
				di_create_area_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle,defaultSettings,chartType);
				break;
			case "pie":
				di_create_pie_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle,defaultSettings,chartType);
				break;
			case "donut":
				di_create_donut_chart(title, uid, visul_div_id, data, datatype, x_title, y_title, subtitle,defaultSettings,chartType);
				break;
		}
	// create chart
	
	});
}


