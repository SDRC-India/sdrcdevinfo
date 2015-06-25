/*
* di.scatter3d.js
* 
*/

var scatterDivId = "scatterDiv";
// function to create scatter chart
function di_create_scatter3d_chart(title, uid, div_id, data, datatype, x_title, y_title, subtitle,defaultSettings,chartType,chartCategory,sourceText) 
{
	var vc_data;
	document.getElementById(div_id).innerHTML = "<div id='" + uid+scatterDivId + "'></div>";
	if(data !="")
	{
		if(datatype=="j.str")		
			vc_data = jQuery.parseJSON(data);
		else
			vc_data = data;
	}
	else
	{
		vc_data.categoryCollection = [];
		vc_data.seriesCollection = [];
	}
	// Embed radar into html
	var swfFilePath = di_version_url + "swf/Di7BubbleChart.swf";	
	var scatter3dSettings = null;
	if(defaultSettings!=null)
	{
		defaultSettings = defaultSettings.replace("$chartTitle$",title);
		defaultSettings = defaultSettings.replace("$chartSubTitle$",subtitle);
		defaultSettings = defaultSettings.replace("$chartSource$",sourceText);
		scatter3dSettings = jQuery.parseJSON(defaultSettings);
		scatter3dSettings.data = vc_data;
		scatter3dSettings.swfURL = swfFilePath;
	}
	else
	{
		scatter3dSettings = di_getScatter3dSettings(uid);
		scatter3dSettings.data = vc_data;
		di_setScatter3dSettings(uid,scatter3dSettings);
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
		item.settings = scatter3dSettings;
		chartCollection.push(item);			
	}
	di_LoadSWF(swfFilePath,uid,scatterDivId, 480, 895);	
}

/*This function is called from swf after loading completed swf into html.We get swf id here*/
function ScatterPlotLoaded(swfId,uid)
{	
	var scatter3dObject = swfobject.getObjectById(swfId);	
	var settingsObject = di_getScatter3dSettings(uid);	
	// Check for dublicate object
	var isUidExist=false;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				isUidExist = true;
				chartCollection[i].chartInstance = scatter3dObject;				
				break;
			}
		}		
	}
	if(!isUidExist)
	{
		var item = new Object();
		item.id = uid;
		item.chartInstance = scatter3dObject;
		item.settings = settingsObject;
		chartCollection.push(item);			
	}
	di_applyScatterDefaultSettings(scatter3dObject,settingsObject);
}

/*Apply scatter's default settings*/
function di_applyScatterDefaultSettings(scatter3dObject,scatter3dSettings)
{
	scatter3dObject.dataToBubbleChart(scatter3dSettings.data);
	scatter3dObject.SetNoteLabelFontSize(scatter3dSettings.note.style.fontSize);
	scatter3dObject.SetNoteLabelColor(scatter3dSettings.note.style.color);
	scatter3dObject.SetNoteLabelWeight(scatter3dSettings.note.style.fontWeight);
	scatter3dObject.SetNoteLabelDecoration(scatter3dSettings.note.style.textDecoration);
	scatter3dObject.SetNoteLabelStyle(scatter3dSettings.note.style.italic);
	scatter3dObject.SetNoteLabelX(scatter3dSettings.note.position.x);
	scatter3dObject.SetNoteLabelY(scatter3dSettings.note.position.y);

	scatter3dObject.SetSourceLabel(scatter3dSettings.source.text);
	scatter3dObject.SetSourceLabelFontSize(scatter3dSettings.source.style.fontSize);
	scatter3dObject.SetSourceLabelColor(scatter3dSettings.source.style.color);
	scatter3dObject.SetSourceLabelWeight(scatter3dSettings.source.style.fontWeight);
	scatter3dObject.SetSourceLabelDecoration(scatter3dSettings.source.style.textDecoration);
	scatter3dObject.SetSourceLabelStyle(scatter3dSettings.source.style.italic);
    scatter3dObject.SetNoteLabelX(scatter3dSettings.source.position.x);
	scatter3dObject.SetNoteLabelY(scatter3dSettings.source.position.y);

	scatter3dObject.SetTitle(scatter3dSettings.title.text);
	scatter3dObject.SetTitleFontSize(scatter3dSettings.title.style.fontSize);
	scatter3dObject.SetTitleColor(scatter3dSettings.title.style.color);
	scatter3dObject.SetTitleWeight(scatter3dSettings.title.style.fontWeight);
	scatter3dObject.SetTitleDecoration(scatter3dSettings.title.style.textDecoration);
	scatter3dObject.SetTitleStyle(scatter3dSettings.title.style.italic);

	scatter3dObject.SetSubTitle(scatter3dSettings.subtitle.text);
	scatter3dObject.SetSubTitleFontSize(scatter3dSettings.subtitle.style.fontSize);
	scatter3dObject.SetSubTitleColor(scatter3dSettings.subtitle.style.color);
	scatter3dObject.SetSubTitleWeight(scatter3dSettings.subtitle.style.fontWeight);
	scatter3dObject.SetSubTitleDecoration(scatter3dSettings.subtitle.style.textDecoration);
	scatter3dObject.SetSubTitleStyle(scatter3dSettings.subtitle.style.italic);	
}

/*Set data to scatter 3d
	Parameters uid:- unique id
				dara: - scatter's data*/
function di_setDataToScatter3d(uid,data)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.dataToBubbleChart(data);	
}

/*Get scatter 3d settings
	Parameters uid: - unique id*/
function di_getScatter3dCurrentSettings(uid)
{
	var settingsObj = di_getScatter3dSettings(uid);
	return JSON.stringify(settingsObj);
}

/*Export Scatter3d chart
	Parameters uid: - Unique Id*/
function di_scatter3dImageBitString(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	var resultImage = scatter3dObject.getImageBitString();	
	return resultImage;
}

/* Return Scatter3d settings 
		Parameter : uid - chart unique id*/
function di_getScatter3dSettings(uid)
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

/* store Scatter3d settings 
		Parameter : uid - chart unique id
					settingsObj - radar's settings*/
function di_setScatter3dSettings(uid,settingsObj)
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

/*Scatter3d Source Postion
	Parameters: uid :-Unique id
				return postion(var pos={x:'',y:''})*/
function di_GetScatter3dSourceLabelPosition(uid)
{
	var pos = {x:0,y:0};
	var scatter3dObject = di_getChartObject(uid);
	pos.x =scatter3dObject.GetSourceLabelX();
	pos.y =scatter3dObject.GetSourceLabelY();
	return pos;
}

/*Scatter3d Note Postion
	Parameters: uid :-Unique id
				Get postion(var pos={x:'',y:''})*/
function di_GetScatter3dCustomLabelPosition(uid)
{
	var pos = {x:0,y:0};
	var scatter3dObject = di_getChartObject(uid);
	pos.x = scatter3dObject.GetNoteLabelX();
	pos.y = scatter3dObject.GetNoteLabelY();
	return pos;
}

/*Scatter3d Title Text
	Parameters: uid :- Unique id
				return title text*/
function di_GetScatter3dTitle(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetTitle();
}

/*Scatter3d Title FontSize
	Parameters: uid :- Unique id
				return number*/
function di_GetScatter3dTitleFontsize(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetTitleFontSize();	
}
/*Scatter3d Title Weight
	Parameters: uid :- Unique id
				return b(italic)/n(normal)*/
function di_GetScatter3dTitleWeight(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetTitleWeight();
}

/*Scatter3d Title Font Style
	Parameters: uid :- Unique id
				return i(italic)/n(normal)*/
function di_GetScatter3dTitleStyle(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetTitleStyle();
}

/*Scatter3d Title Decoration
	Parameters: uid :- Unique id
				return u(underline)/n(normal)*/
function di_GetScatter3dTitleDecoration(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetTitleDecoration();	
}

/*Scaater3d Title color
	Parameters: uid :- Unique id
				return color*/
function di_GetScatter3dTitleColor(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetTitleColor();
}

/*Scatter3d Sub Title Text
	Parameters: uid :- Unique id
				return text*/
function di_GetScatter3dSubTitle(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetSubTitle();
}
/*Scatter3d SubTitle Font size
	Parameters: uid :- Unique id
				fontSize :- number*/
function di_GetScatter3dSubTitleFontsize(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetSubTitleFontSize();	
}

/*Scatter3d SubTitle Weight
	Parameters: uid :- Unique id
				return i(italic)/n(normal)*/
function di_GetScatter3dSubTitleWeight(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetSubTitleWeight();
}

/*Scatter3d SubTitle Font Style
	Parameters: uid :- Unique id
				return i(italic)/n(normal)*/
function di_GetScatter3dSubTitleStyle(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetSubTitleStyle();
}

/*Scatter3d SubTitle Decoration
	Parameters: uid :- Unique id
				return u(underline)/n(normal)*/
function di_GetScatter3dSubTitleDecoration(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetSubTitleDecoration();	
}

/*Scatter3d SubTitle Color
	Parameters: uid :- Unique id
				return color*/
function di_GetScatter3dSubTitleColor(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetSubTitleColor();
}

/*Scatter3d Source Text
	Parameters: uid :- Unique id
				return text*/
function di_GetScatter3dSourceText(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetSourceLabel();
}

/*Scatter3d Source Font size
	Parameters: uid :- Unique id
				return number*/
function di_GetScatter3dSourceLabelFontsize(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetSourceLabelFontSize();	
}

/*Scatter3d Source Label Weight
	Parameters: uid :- Unique id
				return i(italic)/n(normal)*/
function di_GetScatter3dSourceLabelWeight(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetSourceLabelWeight();
}

/*Scatter3d SourceLabel Font Style
	Parameters: uid :- Unique id
				return i(italic)/n(normal)*/
function di_GetScatter3dSourceLabelStyle(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetSourceLabelStyle();
}

/*Scatter3d Source Decoration
	Parameters: uid :- Unique id
				return u(underline)/n(normal)*/
function di_GetScatter3dSourceLabelDecoration(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetSourceLabelDecoration();	
}

/*Scatter3d Source Color
	Parameters: uid :- Unique id
				return color*/
function di_GetScatter3dSourceLabelColor(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetSourceLabelColor();
}

/*Scatter3d Note
	Parameters: uid :-Unique id
				return custom text*/
function di_GetScatter3dCustomText(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetNoteLabel();
}
/*Scatter3d Custom Label Font size
	Parameters: uid :- Unique id
				return number*/
function di_GetScatter3dCustomLabelFontsize(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetNoteLabelFontSize();	
}
/*Scatter3d Custom Label Weight
	Parameters: uid :- Unique id
				return  i(italic)/n(normal)*/
function di_GetScatter3dCustomLabelWeight(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetNoteLabelWeight();
}

/*Scatter3d Custom Label Font style
	Parameters: uid :- Unique id
				return i(italic)/n(normal)*/
function di_GetSCatter3dCustomLabelStyle(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetNoteLabelStyle();
}

/*Scatter3d Custom Label Decoration
	Parameters: uid :- Unique id
				return  u(underline)/n(normal)*/
function di_GetScatter3dCustomLabelDecoration(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetNoteLabelDecoration();	
}

/*Scatter3d Custom Label Color
	Parameters: uid :- Unique id
				return color*/
function di_GetScatter3dCustomLabelColor(uid)
{
	var scatter3dObject = di_getChartObject(uid);
	return scatter3dObject.GetNoteLabelColor();
}

/*Scatter3d Title Text
	Parameters: uid :- Unique id
				title :- text*/
function di_Scatter3dTitle(uid,title)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetTitle(title);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.title.text= title;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Sub Title Text
	Parameters: uid :- Unique id
				subtitle :- text*/
function di_Scatter3dSubTitle(uid,subtitle)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSubTitle(subtitle);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.subtitle.text= subtitle;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Source Text
	Parameters: uid :- Unique id
				text :- text*/
function di_Scatter3dSourceText(uid,text)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSourceLabel(text);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.source.text= text;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Custom Text
	Parameters: uid :- Unique id
				text :- text*/
function di_Scatter3dCustomText(uid,text)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetNoteLabel(text);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.note.text= text;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Title FontSize
	Parameters: uid :- Unique id
				fontSize :- number*/
function di_Scatter3dTitleFontsize(uid,fontSize)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetTitleFontSize(fontSize);	
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.title.style.fontSize= fontSize;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d SubTitle Font size
	Parameters: uid :- Unique id
				fontSize :- number*/
function di_Scatter3dSubTitleFontsize(uid,fontSize)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSubTitleFontSize(fontSize);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.subtitle.style.fontSize= fontSize;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Source Label Decoration
	Parameters: uid :- Unique id
				fontSize :- number*/
function di_Scatter3dSourceLabelFontsize(uid,fontSize)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSourceLabelFontSize(fontSize);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.source.style.fontSize= fontSize;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Custom Label Font size
	Parameters: uid :- Unique id
				fontSize :- number*/
function di_Scatter3dCustomLabelFontsize(uid,fontSize)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetNoteLabelFontSize(fontSize);	
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.note.style.fontSize= fontSize;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Radar Title Weight
	Parameters: uid :- Unique id
				weight :- i(italic)/n(normal)*/
function di_Scatter3dTitleWeight(uid,weight)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetTitleWeight(weight);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.title.style.fontWeight= weight;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Title Font Style
	Parameters: uid :- Unique id
				style :- i(italic)/n(normal)*/
function di_Scatter3dTitleStyle(uid,style)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetTitleStyle(style);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.title.style.italic= style;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Title Decoration
	Parameters: uid :- Unique id
				decoration :- u(underline)/n(normal)*/
function di_Scatter3dTitleDecoration(uid,decoration)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetTitleDecoration(decoration);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.title.style.textDecoration= decoration;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d SubTitle Weight
	Parameters: uid :- Unique id
				weight :- i(italic)/n(normal)*/
function di_Scatter3dSubTitleWeight(uid,weight)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSubTitleWeight(weight);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.subtitle.style.fontWeight= weight;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d SubTitle Font Style
	Parameters: uid :- Unique id
				style :- i(italic)/n(normal)*/
function di_Scatter3dSubTitleStyle(uid,style)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSubTitleStyle(style);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.subtitle.style.italic= style;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d SubTitle Decoration
	Parameters: uid :- Unique id
				decoration :- u(underline)/n(normal)*/
function di_Scatter3dSubTitleDecoration(uid,decoration)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSubTitleDecoration(decoration);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.subtitle.style.textDecoration= decoration;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Source Label Weight
	Parameters: uid :- Unique id
				weight :- i(italic)/n(normal)*/
function di_Scatter3dSourceLabelWeight(uid,weight)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSourceLabelWeight(weight);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.source.style.fontWeight= weight;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d SourceLabel Font Style
	Parameters: uid :- Unique id
				style :- i(italic)/n(normal)*/
function di_Scatter3dSourceLabelStyle(uid,style)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSourceLabelStyle(style);
	var scatter3dSettingsObject = di_getRadarSettings(uid);
	scatter3dSettingsObject.source.style.italic= style;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Source Decoration
	Parameters: uid :- Unique id
				decoration :- u(underline)/n(normal)*/
function di_Scatter3dSourceLabelDecoration(uid,decoration)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSourceLabelDecoration(decoration);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.source.style.textDecoration= decoration;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Custom Label Weight
	Parameters: uid :- Unique id
				weight :- i(italic)/n(normal)*/
function di_Scatter3dCustomLabelWeight(uid,weight)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetNoteLabelWeight(weight);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.note.style.fontWeight= weight;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Custom Label Font style
	Parameters: uid :- Unique id
				style :- i(italic)/n(normal)*/
function di_Scatter3dCustomLabelStyle(uid,style)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetNoteLabelStyle(style);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.note.style.italic= style;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Custom Label Decoration
	Parameters: uid :- Unique id
				decoration :- u(underline)/n(normal)*/
function di_Scatter3dCustomLabelDecoration(uid,decoration)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetNoteLabelDecoration(decoration);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.note.style.textDecoration= decoration;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Title color
	Parameters: uid :- Unique id
				color :- color*/
function di_Scatter3dTitleColor(uid,color)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetTitleColor(color);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.title.style.color= color;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d SubTitle Color
	Parameters: uid :- Unique id
				color :- color*/
function di_Scatter3dSubTitleColor(uid,color)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSubTitleColor(color);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.subtitle.style.color= color;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Source Color
	Parameters: uid :- Unique id
				color :- color*/
function di_Scatter3dSourceLabelColor(uid,color)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSourceLabelColor(color);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.source.style.color= color;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Custom Label Color
	Parameters: uid :- Unique id
				color :- color*/
function di_Scatter3dCustomLabelColor(uid,color)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetNoteLabelColor(color);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.note.style.color= color;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Source Postion
	Parameters: uid :-Unique id
				position :- postion(var pos={x:'',y:''})*/
function di_Scatter3dSourceLabelPosition(uid,position)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetSourceLabelX(position.x);
	scatter3dObject.SetSourceLabelY(position.y);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.source.position.x= position.x;
	scatter3dSettingsObject.source.position.y= position.y;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}

/*Scatter3d Note Postion
	Parameters: uid :-Unique id
				position :- postion(var pos={x:'',y:''})*/
function di_Scatter3dCustomLabelPosition(uid,position)
{
	var scatter3dObject = di_getChartObject(uid);
	scatter3dObject.SetNoteLabelX(position.x);
	scatter3dObject.SetNoteLabelY(position.y);
	var scatter3dSettingsObject = di_getScatter3dSettings(uid);
	scatter3dSettingsObject.note.position.x= position.x;
	scatter3dSettingsObject.note.position.y= position.y;
	di_setScatter3dSettings(uid,scatter3dSettingsObject);
}