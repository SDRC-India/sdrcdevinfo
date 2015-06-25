/*
* di.radar.js
* 
*/
var vc_data=new Object();
var radarDivId = "radarDiv";
// function to create radar chart
function di_create_radar_chart(title, uid, div_id, data, datatype, x_title, y_title, subtitle,defaultSettings,chartType,chartCategory,sourceText) 
{
	document.getElementById(div_id).innerHTML = "<div id='"+uid+radarDivId+"'></div>";
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
	// Embed radar into html
	var swfFilePath = di_version_url + "swf/RadarChart.swf";	
	var radarSettingsObject = null;
	if(defaultSettings!=null)
	{
		defaultSettings = defaultSettings.replace("$chartTitle$",title);
		defaultSettings = defaultSettings.replace("$chartSubTitle$",subtitle);
		defaultSettings = defaultSettings.replace("$chartSource$",sourceText);
		radarSettingsObject = jQuery.parseJSON(defaultSettings);
		radarSettingsObject.data = vc_data;
		radarSettingsObject.swfURL = swfFilePath;
	}
	else
	{
		radarSettingsObject = di_getRadarSettings(uid);
		radarSettingsObject.data = vc_data;
		di_setRadarSettings(uid,radarSettingsObject);
	}
	
	var isUidExist=false;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				isUidExist = true;
				break;
			}
		}		
	}
	if(!isUidExist)
	{
		var item = new Object();
		item.id = uid;		
		item.settings = radarSettingsObject;
		chartCollection.push(item);			
	}
	di_LoadSWF(swfFilePath,uid,radarDivId, 498, 895);	
}


/*This function is called from swf after loading completed swf into html.We get swf id here*/
function radarLoadingComplete(swfId,uid)
{
	//alert(swfId+"\n"+uid);	
	var radarObject = swfobject.getObjectById(swfId);		
	var settingsObject = di_getRadarSettings(uid);
	di_applyDefaultSettings(radarObject,settingsObject);	
	//radarObject.render();
// Check for dublicate object
	var isUidExist=false;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				isUidExist = true;
				chartCollection[i].chartInstance = radarObject;				
				break;
			}
		}		
	}
	if(!isUidExist)
	{
		var item = new Object();
		item.id = uid;
		item.chartInstance = radarObject;
		item.settings = settingsObject;
		chartCollection.push(item);			
	}
}



function di_applyDefaultSettings(radarObject,radarSettingsObject)
{
	radarObject.dataToRadar(radarSettingsObject.data);		
	radarObject.SetChartBorderVisible(radarSettingsObject.chart.borderVisiblity);
	radarObject.SetChartBorderWidth(radarSettingsObject.chart.borderWidth);
	radarObject.SetChartBorderRadius(radarSettingsObject.chart.borderRadius);
	radarObject.SetChartBorderColor(radarSettingsObject.chart.borderColor);
	radarObject.SetBackgroundColor(radarSettingsObject.chart.backgroundColor);
	radarObject.SetGridVisibility(radarSettingsObject.chart.gridlines);
	radarObject.SetNoteLabel(radarSettingsObject.note.text);

	radarObject.SetNoteLabelFontSize(radarSettingsObject.note.style.fontSize);
	radarObject.SetNoteLabelColor(radarSettingsObject.note.style.color);
	radarObject.SetNoteLabelWeight(radarSettingsObject.note.style.fontWeight);
	radarObject.SetNoteLabelDecoration(radarSettingsObject.note.style.textDecoration);
	radarObject.SetNoteLabelStyle(radarSettingsObject.note.style.italic);
	radarObject.SetNoteLabelX(radarSettingsObject.note.position.x);
	radarObject.SetNoteLabelY(radarSettingsObject.note.position.y);

	radarObject.SetSourceLabel(radarSettingsObject.source.text);
	radarObject.SetSourceLabelFontSize(radarSettingsObject.source.style.fontSize);
	radarObject.SetSourceLabelColor(radarSettingsObject.source.style.color);
	radarObject.SetSourceLabelWeight(radarSettingsObject.source.style.fontWeight);
	radarObject.SetSourceLabelDecoration(radarSettingsObject.source.style.textDecoration);
	radarObject.SetSourceLabelStyle(radarSettingsObject.source.style.italic);
	radarObject.SetNoteLabelX(radarSettingsObject.source.position.x);
	radarObject.SetNoteLabelY(radarSettingsObject.source.position.y);

	radarObject.SetTitle(radarSettingsObject.title.text);
	radarObject.SetTitleFontSize(radarSettingsObject.title.style.fontSize);
	radarObject.SetTitleColor(radarSettingsObject.title.style.color);
	radarObject.SetTitleWeight(radarSettingsObject.title.style.fontWeight);
	radarObject.SetTitleDecoration(radarSettingsObject.title.style.textDecoration);
	radarObject.SetTitleStyle(radarSettingsObject.title.style.italic);
	radarObject.SetSubTitle(radarSettingsObject.subtitle.text);
	radarObject.SetSubTitleFontSize(radarSettingsObject.subtitle.style.fontSize);
	radarObject.SetSubTitleColor(radarSettingsObject.subtitle.style.color);
	radarObject.SetSubTitleWeight(radarSettingsObject.subtitle.style.fontWeight);
	radarObject.SetSubTitleDecoration(radarSettingsObject.subtitle.style.textDecoration);
	radarObject.SetSubTitleStyle(radarSettingsObject.subtitle.style.italic);
	radarObject.SetLegendVisible(radarSettingsObject.legend.enabled);
	radarObject.SetLegendBorderWidth(radarSettingsObject.legend.borderWidth);
	radarObject.SetLegendBorderColor(radarSettingsObject.legend.borderColor);
	radarObject.SetLegendBorderRadius(radarSettingsObject.legend.borderRadius);
	radarObject.SetLegendBackgroundcolor(radarSettingsObject.legend.backgroundColor);
	radarObject.SetLegendBorderVisible(radarSettingsObject.legend.borderVisibility);
	radarObject.SetLegendDirection(radarSettingsObject.legend.position);		
	radarObject.SetSeriesType(radarSettingsObject.option.seriesType);
	radarObject.SetRadialAxis(radarSettingsObject.option.radialAxis);
	radarObject.SetForm(radarSettingsObject.option.form);
	radarObject.SetChartType(radarSettingsObject.option.chartType);
	radarObject.SetThemeColor(radarSettingsObject.colors);
	radarObject.render();
}

function di_getRadarCurrentSettings(uid)
{
	var settingsObj = di_getRadarSettings(uid);
	return JSON.stringify(settingsObj);
}

/* Return Radar settings 
		Parameter : uid - chart unique id*/
function di_getRadarSettings(uid)
{
	var settingsObj;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				settingsObj = chartCollection[i].settings;
				break;
			}
		}
	}
	return settingsObj;
}

/* store Radar settings 
		Parameter : uid - chart unique id
					settingsObj - radar's settings*/
function di_setRadarSettings(uid,settingsObj)
{
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				chartCollection[i].settings = settingsObj;
				break;
			}
		}
	}
}

function di_setDataToRadar(uid,data)
{
	var radarObject = di_getChartObject(uid);
	radarObject.dataToRadar(data);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.data = data;
	di_setRadarSettings(uid,radarSettingsObject);
}

/*Render Radar
	Parameter:	uid :- Unique Id*/
function di_renderRadar(uid)
{
	var radarObject = di_getChartObject(uid);
	radarObject.render();
}
/*Radar Series Type
	Parameters: uid :- Unique Id
				seriesType:	l :-line
							c :-column*/
function di_RadarSeriesType(uid,seriesType)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSeriesType(seriesType);	
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.option.seriesType = seriesType;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Series Type
	Parameters: uid :- Unique Id
				return seriesType:	l :-line
							c :-column*/
function di_GetRadarSeriesType(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSeriesType();
}
/*Radial Axis Type
	Parameters: uid :- Unique Id
				type:   h :- horizontal
						c :- column
						b :- both
						n :- none*/
function di_RadialAxis(uid,type)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetRadialAxis(type);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.option.radialAxis = type;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radial Axis Type
	Parameters: uid :- Unique Id
				return type:   h :- horizontal
						c :- column
						b :- both
						n :- none*/
function di_GetRadialAxis(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetRadialAxis();	
}
/*Radar Wave layout
	Parameters:	uid :- Unique Id
				type:	s :-segment
						c :-curve*/
function di_RadarWaveLayout(uid,type)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetForm(type);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.option.form = type;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Wave layout
	Parameters:	uid :- Unique Id
				return type:	s :-segment
						c :-curve*/
function di_GetRadarWaveLayout(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetForm();	
}
/*Radar Fill/Empty Wave
	Parameters: uid :- Unique Id
				isFill:	true/false*/
function di_isFillRadarWave(uid,isFill)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetFill(isFill);
}
/*Radar Legend Layout
	Parameters:	uid :- Unique Id
				layout: h/v*/
function di_RadarLegendLayout(uid,layout)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetLegendDirection(layout);	
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.legend.position = layout;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Legend Layout
	Parameters:	uid :- Unique Id
				return h/v*/
function di_GetRadarLegendLayout(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetLegendDirection();	
}
/*Radar Grid Visibility
	Parameters: uid :-Unique id
				isVisible :- true/false*/
function di_RadarGridVisibility(uid,isVisible)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetGridVisibility(isVisible);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.chart.gridlines = isVisible;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Grid Visibility
	Parameters: uid :-Unique id
				isVisible :- true/false*/
function di_GetRadarGridVisibility(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetGridVisibility();
}
/*Radar GridBackgorund color
	Parameters: uid :-Unique id
				color :- color code*/
function di_RadarBackgroundColor(uid,color)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetBackgroundColor(color);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.chart.backgroundColor = color;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar GridBackgorund color
	Parameters: uid :-Unique id
				return  color code*/
function di_GetRadarBackgroundColor(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetBackgroundColor();
}
/*Radar Type
	Parameters: uid :-Unique id
				type :- p(polygonal)
						c:(circular)*/
function di_RadarType(uid,type)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetChartType(type);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.option.chartType = type;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Type
	Parameters: uid :-Unique id
				return type :- p(polygonal)
						c:(circular)*/
function di_GetRadarType(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetChartType();
}


/*Radar Note
	Parameters: uid :-Unique id
				text :- custom text*/
function di_RadarCustomLabel(uid,text)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetNoteLabel(text);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.note.text= text;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Note
	Parameters: uid :-Unique id
				return custom text*/
function di_GetRadarCustomText(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetNoteLabel();
}
/*Radar Note Postion
	Parameters: uid :-Unique id
				position :- postion(var pos={x:'',y:''})*/
function di_RadarCustomLabelPosition(uid,position)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetNoteLabelX(position.x);
	radarObject.SetNoteLabelY(position.y);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.note.position.x= position.x;
	radarSettingsObject.note.position.y= position.y;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Note Postion
	Parameters: uid :-Unique id
				Get postion(var pos={x:'',y:''})*/
function di_GetRadarCustomLabelPosition(uid)
{
	var pos = {x:0,y:0};
	var radarObject = di_getChartObject(uid);
	pos.x = radarObject.GetNoteLabelX();
	pos.y = radarObject.GetNoteLabelY();
	return pos;
}
/*Radar Source
	Parameters: uid :-Unique id
				text :- custom text*/
function di_RadarSourceLabel(uid,text)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSourceLabel(text);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.source.text= text;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Source Postion
	Parameters: uid :-Unique id
				position :- postion(var pos={x:'',y:''})*/
function di_RadarSourceLabelPosition(uid,position)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSourceLabelX(position.x);
	radarObject.SetSourceLabelY(position.y);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.source.position.x= position.x;
	radarSettingsObject.source.position.y= position.y;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Source Postion
	Parameters: uid :-Unique id
				return postion(var pos={x:'',y:''})*/
function di_GetRadarSourceLabelPosition(uid)
{
	var pos = {x:0,y:0};
	var radarObject = di_getChartObject(uid);
	pos.x =radarObject.GetSourceLabelX();
	pos.y =radarObject.GetSourceLabelY();
	return pos;
}
/*Radar Border Visibility
	Parameters: uid :-Unique id
				isVisible :- true/false*/
function di_RadarBorderVisible(uid,isVisible)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetChartBorderVisible(isVisible);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.chart.borderVisiblity= isVisible;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Border Visibility
	Parameters: uid :-Unique id
				return true/false*/
function di_GetRadarBorderVisible(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetChartBorderVisible();
}
/*Radar Chart BorderWidth
	Parameters: uid :-Unique id
				borderWidth :- Border Width*/
function di_RadarChartBorderWidth(uid,borderWidth)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetChartBorderWidth(borderWidth);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.chart.borderWidth= borderWidth;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Chart BorderWidth
	Parameters: uid :-Unique id
				return  Border Width*/
function di_GetRadarChartBorderWidth(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetChartBorderWidth();	
}
/*Radar Grid Chart border color
	Parameters: uid :-Unique id
				color :- color code*/
function di_RadarChartBorderColor(uid,color)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetChartBorderColor(color);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.chart.borderColor= color;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Grid Chart border color
	Parameters: uid :-Unique id
				return color code*/
function di_GetRadarChartBorderColor(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetChartBorderColor();
}
/*Radar Grid Chart border radius
	Parameters: uid :-Unique id
				radius :- border radius*/
function di_RadarChartBorderRadius(uid,radius)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetChartBorderRadius(radius);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.chart.borderRadius= radius;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Grid Chart border radius
	Parameters: uid :-Unique id
				return border radius*/
function di_GetRadarChartBorderRadius(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetChartBorderRadius();
}
/*Radar Legend Bg Color
	Parameters: uid :- Unique id
				color :- color*/
function di_RadarLegendBgColor(uid,color)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetLegendBackgroundcolor(color);	
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.legend.backgroundColor= color;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Legend Bg Color
	Parameters: uid :- Unique id
				return color*/
function di_GetRadarLegendBgColor(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetLegendBackgroundcolor();	
}
/*Radar Legend Border Visibility
	Parameters: uid :- Unique id
				isVisible :- true/false*/
function di_RadarLegendBorderVisible(uid,isVisible)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetLegendBorderVisible(isVisible);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.legend.borderVisibility= isVisible;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Legend Border Visibility
	Parameters: uid :- Unique id
				return  true/false*/
function di_GetRadarLegendBorderVisible(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetLegendBorderVisible();
}
/*Radar Legend Border Width
	Parameters: uid :- Unique id
				width :- width*/
function di_RadarLegendBorderWidth(uid,width)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetLegendBorderWidth(width);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.legend.borderWidth= width;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Legend Border Width
	Parameters: uid :- Unique id
				return width*/
function di_GetRadarLegendBorderWidth(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetLegendBorderWidth();	
}
/*Radar Legend Border Color
	Parameters: uid :- Unique id
				color :- color*/
function di_RadarLegendBorderColor(uid,color)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetLegendBorderColor(color);	
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.legend.borderColor= color;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Legend Border Color
	Parameters: uid :- Unique id
				return color*/
function di_GetRadarLegendBorderColor(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetLegendBorderColor();	
}
/*Radar Legend Border Radius
	Parameters: uid :- Unique id
				radius :- radius*/
function di_RadarLegendBorderRadius(uid,radius)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetLegendBorderRadius(radius);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.legend.borderRadius= radius;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Legend Border Radius
	Parameters: uid :- Unique id
				return radius*/
function di_GetRadarLegendBorderRadius(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetLegendBorderRadius();	
}
/*Radar Title Text
	Parameters: uid :- Unique id
				title :- text*/
function di_RadarTitle(uid,title)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetTitle(title);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.title.text= title;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Title Text
	Parameters: uid :- Unique id
				return title text*/
function di_GetRadarTitle(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetTitle();
}
/*Radar Title color
	Parameters: uid :- Unique id
				color :- color*/
function di_RadarTitleColor(uid,color)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetTitleColor(color);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.title.style.color= color;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Title color
	Parameters: uid :- Unique id
				return color*/
function di_GetRadarTitleColor(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetTitleColor();
}
/*Radar Title Decoration
	Parameters: uid :- Unique id
				decoration :- u(underline)/n(normal)*/
function di_RadarTitleDecoration(uid,decoration)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetTitleDecoration(decoration);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.title.style.textDecoration= decoration;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Title Decoration
	Parameters: uid :- Unique id
				return u(underline)/n(normal)*/
function di_GetRadarTitleDecoration(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetTitleDecoration();	
}
/*Radar Title FontSize
	Parameters: uid :- Unique id
				fontSize :- number*/
function di_RadarTitleFontsize(uid,fontSize)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetTitleFontSize(fontSize);	
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.title.style.fontSize= fontSize;
	di_setRadarSettings(uid,radarSettingsObject);
}

/*Radar Title FontSize
	Parameters: uid :- Unique id
				return number*/
function di_GetRadarTitleFontsize(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetTitleFontSize();	
}

/*Radar Title Font Style
	Parameters: uid :- Unique id
				style :- i(italic)/n(normal)*/
function di_RadarTitleStyle(uid,style)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetTitleStyle(style);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.title.style.italic= style;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Title Font Style
	Parameters: uid :- Unique id
				return i(italic)/n(normal)*/
function di_GetRadarTitleStyle(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetTitleStyle();
}
/*Radar Title Weight
	Parameters: uid :- Unique id
				weight :- i(italic)/n(normal)*/
function di_RadarTitleWeight(uid,weight)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetTitleWeight(weight);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.title.style.fontWeight= weight;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Title Weight
	Parameters: uid :- Unique id
				return b(italic)/n(normal)*/
function di_GetRadarTitleWeight(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetTitleWeight();
}
/*Radar Sub Title Text
	Parameters: uid :- Unique id
				subtitle :- text*/
function di_RadarSubTitle(uid,subtitle)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSubTitle(subtitle);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.subtitle.text= subtitle;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Sub Title Text
	Parameters: uid :- Unique id
				return text*/
function di_GetRadarSubTitle(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSubTitle();
}
/*Radar SubTitle Color
	Parameters: uid :- Unique id
				color :- color*/
function di_RadarSubTitleColor(uid,color)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSubTitleColor(color);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.subtitle.style.color= color;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar SubTitle Color
	Parameters: uid :- Unique id
				return color*/
function di_GetRadarSubTitleColor(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSubTitleColor();
}
/*Radar SubTitle Decoration
	Parameters: uid :- Unique id
				decoration :- u(underline)/n(normal)*/
function di_RadarSubTitleDecoration(uid,decoration)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSubTitleDecoration(decoration);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.subtitle.style.textDecoration= decoration;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar SubTitle Decoration
	Parameters: uid :- Unique id
				return u(underline)/n(normal)*/
function di_GetRadarSubTitleDecoration(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSubTitleDecoration();	
}
/*Radar SubTitle Font size
	Parameters: uid :- Unique id
				fontSize :- number*/
function di_RadarSubTitleFontsize(uid,fontSize)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSubTitleFontSize(fontSize);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.subtitle.style.fontSize= fontSize;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar SubTitle Font size
	Parameters: uid :- Unique id
				fontSize :- number*/
function di_GetRadarSubTitleFontsize(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSubTitleFontSize();	
}
/*Radar SubTitle Font Style
	Parameters: uid :- Unique id
				style :- i(italic)/n(normal)*/
function di_RadarSubTitleStyle(uid,style)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSubTitleStyle(style);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.subtitle.style.italic= style;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar SubTitle Font Style
	Parameters: uid :- Unique id
				return i(italic)/n(normal)*/
function di_GetRadarSubTitleStyle(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSubTitleStyle();
}
/*Radar SubTitle Weight
	Parameters: uid :- Unique id
				weight :- i(italic)/n(normal)*/
function di_RadarSubTitleWeight(uid,weight)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSubTitleWeight(weight);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.subtitle.style.fontWeight= weight;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar SubTitle Weight
	Parameters: uid :- Unique id
				return i(italic)/n(normal)*/
function di_GetRadarSubTitleWeight(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSubTitleWeight();
}

/*Radar Custom Text
	Parameters: uid :- Unique id
				text :- text*/
function di_RadarCustomText(uid,text)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetNoteLabel(text);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.note.text= text;
	di_setRadarSettings(uid,radarSettingsObject);
}

/*Radar Custom Label Color
	Parameters: uid :- Unique id
				color :- color*/
function di_RadarCustomLabelColor(uid,color)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetNoteLabelColor(color);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.note.style.color= color;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Custom Label Color
	Parameters: uid :- Unique id
				return color*/
function di_GetRadarCustomLabelColor(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetNoteLabelColor();
}
/*Radar Custom Label Decoration
	Parameters: uid :- Unique id
				decoration :- u(underline)/n(normal)*/
function di_RadarCustomLabelDecoration(uid,decoration)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetNoteLabelDecoration(decoration);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.note.style.textDecoration= decoration;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Custom Label Decoration
	Parameters: uid :- Unique id
				return  u(underline)/n(normal)*/
function di_GetRadarCustomLabelDecoration(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetNoteLabelDecoration();	
}
/*Radar Custom Label Font size
	Parameters: uid :- Unique id
				fontSize :- number*/
function di_RadarCustomLabelFontsize(uid,fontSize)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetNoteLabelFontSize(fontSize);	
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.note.style.fontSize= fontSize;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Custom Label Font size
	Parameters: uid :- Unique id
				return number*/
function di_GetRadarCustomLabelFontsize(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetNoteLabelFontSize();	
}
/*Radar Custom Label Font style
	Parameters: uid :- Unique id
				style :- i(italic)/n(normal)*/
function di_RadarCustomLabelStyle(uid,style)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetNoteLabelStyle(style);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.note.style.italic= style;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Custom Label Font style
	Parameters: uid :- Unique id
				return i(italic)/n(normal)*/
function di_GetRadarCustomLabelStyle(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetNoteLabelStyle();
}
/*Radar Custom Label Weight
	Parameters: uid :- Unique id
				weight :- i(italic)/n(normal)*/
function di_RadarCustomLabelWeight(uid,weight)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetNoteLabelWeight(weight);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.note.style.fontWeight= weight;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Custom Label Weight
	Parameters: uid :- Unique id
				return  i(italic)/n(normal)*/
function di_GetRadarCustomLabelWeight(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetNoteLabelWeight();
}
/*Radar Source Text
	Parameters: uid :- Unique id
				text :- text*/
function di_RadarSourceText(uid,text)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSourceLabel(text);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.source.text= text;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Source Text
	Parameters: uid :- Unique id
				return text*/
function di_GetRadarSourceText(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSourceLabel();
}
/*Radar Source Color
	Parameters: uid :- Unique id
				color :- color*/
function di_RadarSourceLabelColor(uid,color)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSourceLabelColor(color);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.source.style.color= color;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Source Color
	Parameters: uid :- Unique id
				return color*/
function di_GetRadarSourceLabelColor(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSourceLabelColor();
}
/*Radar Source Decoration
	Parameters: uid :- Unique id
				decoration :- u(underline)/n(normal)*/
function di_RadarSourceLabelDecoration(uid,decoration)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSourceLabelDecoration(decoration);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.source.style.textDecoration= decoration;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Source Decoration
	Parameters: uid :- Unique id
				return u(underline)/n(normal)*/
function di_GetRadarSourceLabelDecoration(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSourceLabelDecoration();	
}
/*Radar Source Label Decoration
	Parameters: uid :- Unique id
				fontSize :- number*/
function di_RadarSourceLabelFontsize(uid,fontSize)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSourceLabelFontSize(fontSize);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.source.style.fontSize= fontSize;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Source Font size
	Parameters: uid :- Unique id
				return number*/
function di_GetRadarSourceLabelFontsize(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSourceLabelFontSize();	
}
/*Radar SourceLabel Font Style
	Parameters: uid :- Unique id
				style :- i(italic)/n(normal)*/
function di_RadarSourceLabelStyle(uid,style)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSourceLabelStyle(style);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.source.style.italic= style;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar SourceLabel Font Style
	Parameters: uid :- Unique id
				return i(italic)/n(normal)*/
function di_GetRadarSourceLabelStyle(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSourceLabelStyle();
}
/*Radar Source Label Weight
	Parameters: uid :- Unique id
				weight :- i(italic)/n(normal)*/
function di_RadarSourceLabelWeight(uid,weight)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSourceLabelWeight(weight);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.source.style.fontWeight= weight;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Source Label Weight
	Parameters: uid :- Unique id
				return i(italic)/n(normal)*/
function di_GetRadarSourceLabelWeight(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSourceLabelWeight();
}
/*Radar Source Label Weight
	Parameters: uid :- Unique id
				isVisible :- true/false*/
function di_RadarBoundaryVisiblity(uid,isVisible)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetBoundaryVisible(isVisible);	
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.chart.borderVisiblity= isVisible;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Radar Source Label Weight
	Parameters: uid :- Unique id
				Thickness :- number*/
function di_RadarBoundaryThickness(uid,thickness)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetBoundaryThickness(thickness);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.chart.borderWidth= thickness;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Toggle Legend*/
function di_RadarLegendVisible(uid,isVisible)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetLegendVisible(isVisible);
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.legend.enabled= isVisible;
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Toggle Legend*/
function di_GetRadarLegendVisible(uid)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetLegendVisible();	
}
/*Change the radar series color
	Parameters uid: - Unique Id
				seriesCol :- series collection ---sample---[{name:"series name",color:"code"},{}]*/
function di_ChangeRadarSeriesColor(uid,seriesCol)
{
	var radarObject = di_getChartObject(uid);
	radarObject.SetSeriesColor(seriesCol);
}
/*Change the radar series color
	Parameters uid: - Unique Id
				series name: name of the series
				return series color*/
function di_GetRadarSeriesColor(uid,seriesName)
{
	var radarObject = di_getChartObject(uid);
	return radarObject.GetSeriesColor(seriesName);
}

/*Export radar chart
	Parameters uid: - Unique Id*/
function di_RadarImageBitString(uid)
{
	var radarObject = di_getChartObject(uid);
	var resultImage = radarObject.getImageBitString();	
	return resultImage;
}

/*Set Chart Style
            Parameter: uid -  unique chart id
						styleId - style id*/
function di_setRadarTheme(uid,styleId)
{
	var chartObj = di_getChartObject(uid);    
	chartObj.SetBackgroundColor("0xffffff");
    chartObj.SetLegendBackgroundcolor("0xffffff");
	var themeColorArray;
    if(styleId=="1")
    {				
        themeColorArray = returnColorArray("#888888");
    }
	else if(styleId=="2")
    {
        themeColorArray = returnColorArray("#39608e");
    }
	else if(styleId=="3")
    {
        themeColorArray = returnColorArray("#748c41");
    }
	else if(styleId=="4")
    {
        themeColorArray = returnColorArray("#903a38");
    }
	else if(styleId=="5")
    {
        themeColorArray = returnColorArray("#6600CC");
    }
	else if(styleId=="6")
    {
        themeColorArray = returnColorArray("#006B8F");	
    }
	else if(styleId=="7")
    {
        themeColorArray = returnColorArray("#CC7A00");		
    }
	else if(styleId=="8")
    {
        themeColorArray =  returnColorArray("#f4dd0f");
    }
	else if(styleId=="9")
    {
        themeColorArray = returnColorArray("#FF1919");	
    }
	else if(styleId=="10")
    {
        themeColorArray = returnColorArray("#006600");
    }
    else if (styleId == "11") {
        themeColorArray = returnColorArray("#888888");
    }
    else if (styleId == "12") {
        themeColorArray = returnColorArray("#39608e");
    }
    else if (styleId == "13") {
        themeColorArray = returnColorArray("#748c41");
    }
    else if (styleId == "14") {
        themeColorArray = returnColorArray("#903a38");
    }
    else if (styleId == "15") {
        themeColorArray = returnColorArray("#6600CC");
    }
    else if (styleId == "16") {
        themeColorArray = returnColorArray("#006B8F");
    }
    else if (styleId == "17") {
        themeColorArray = returnColorArray("#CC7A00");
    }
    else if (styleId == "18") {
        themeColorArray = returnColorArray("#f4dd0f");
    }
    else if (styleId == "19") {
        themeColorArray = returnColorArray("#FF1919");
    }
    else if (styleId == "20") {
        themeColorArray = returnColorArray("#006600");
    }
	if(themeColorArray!=null)
	{
		var colorThemeArray = makeRadarThemeColorArray(themeColorArray);		
		chartObj.SetThemeColor(colorThemeArray);		
	}
	var radarSettingsObject = di_getRadarSettings(uid);
	radarSettingsObject.colors = colorThemeArray;
	radarSettingsObject.chart.backgroundColor = "0xffffff";
	radarSettingsObject.legend.backgroundColor = "0xffffff";
	di_setRadarSettings(uid,radarSettingsObject);
}
/*Making proper theme color array for radar*/
function makeRadarThemeColorArray(colorArray)
{
	var radarThemeArray=[];
	if(colorArray!=null)
	{
		for(var i=0;i<colorArray.length;i++)
		{			
			var color = colorArray[i].replace("#","0x");
			radarThemeArray.push(color);
		}
	}
	return radarThemeArray;
}