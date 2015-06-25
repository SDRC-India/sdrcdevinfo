<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CYV.aspx.cs" Inherits="libraries_aspx_CYV" EnableSessionState="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <!-- DEVELOPER CODE -->
    <script type="text/javascript">
	var featureList=["tab","panelbar","dialog"];
	var testInnerHtml="";
</script>
<script type="text/javascript" src="../js/di.jquery-1.4.4.js"></script>
<script type="text/javascript" src="../js/visualizer.js"></script>
<script type="text/javascript">

function CallBack(){}

function getStringFromNId(arrNidsStringPair,StrNId)
{
    var Result = "";
    for(var i=0; i<arrNidsStringPair.length; i = i + 2){
        if(arrNidsStringPair[i] == StrNId)
        {
            Result = arrNidsStringPair[i+1];
            break;
        }
    }
    return Result;

}    

function get_data1(ResponseText) 
{    
    var form = document.createElement("form");
                    form.setAttribute("method", "post");
                    form.setAttribute("action", "DataView.aspx");
                    
    var hiddenField = document.createElement("input");              
                        hiddenField.setAttribute("name", "hCsvFilePath");
                        hiddenField.setAttribute("value", ResponseText);
                        form.appendChild(hiddenField);
                        document.body.appendChild(form);
                        
    form.submit();
	//get_data(ResponseText);
}

function get_data(ResponseText)
{

    var DataContent = ResponseText.split("#"); 
	if(DataContent.length==1) {
	    z('imgGridLoader').style.display = 'none';
	    
	    var PivotContent = '<center>Please select <a href ="#" onclick="showHideComponent(' + "'ind')" +'"> Topic </a> and ';
	    PivotContent += ' <a href ="#" onclick="showHideComponent(' + "'area')" +'"> Area </a> here.</center>';
	    z('pivot_content').innerHTML = PivotContent;
	    z('dataTitle').innerHTML = 'No data found for current selections';
	    z('dataSubTitle').innerHTML = '';
	    z('divCountDataRows').innerHTML = '';
	    z('divSingleSource').innerHTML = '';
		return false;
	}

	var data = [];
	var DataContent = ResponseText.split("#");
	z('dataTitle').innerHTML = DataContent[1];
	var AllKeyVals = DataContent[0].split("|");
	var Title = DataContent[1];
	var header = DataContent[2].split("|");
	var KeysVals = new Array();
	var Contents = new Array();
	for(var i=3; i<DataContent.length; i++) {
		Contents[i-3] = DataContent[i];		
	}
	
	for(var i=0; i<AllKeyVals.length; i++)
	{
	    var tmpKeyVal = AllKeyVals[i].split(":");
	    KeysVals.push(tmpKeyVal[0]);
	    KeysVals.push(tmpKeyVal[1]);
	}
	
	for (var i=0;i<Contents.length-1;i++) {
	
	    var tmpRow = Contents[i].split("|");
		
		for (var j=0;j<tmpRow.length-1;j++){
		    tmpRow[j] = getStringFromNId(KeysVals,tmpRow[j]);
		}
		data.push(tmpRow);
	}
	
	var ColHeaders = new Array();
	var RowHeaders = new Array();	
	for(var i = 0; i < header.length - 2; i++)
	{
	    ColHeaders.push(i);
	}
    RowHeaders.push(header.length-2);
	
	var pivot = new OAT.Pivot("pivot_content","","pivot_page",header,data,ColHeaders,RowHeaders,[],header.length-1,{showChart:0,showEmpty:0});	
	setVisualizerData('<%=_QuerySType%>');

}

</script>


<script type="text/javascript" src="http://dgps/di7poc/diuilib/1.9/js/oatgrid/loader.js"></script>

    <link href="../../stock/themes/default/css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        CookiePostfixStr = '_' + '<%=Global.CookiePostfixStr%>';    // use in hosting app
		
        var di_components = "DIVC";
	    var di_vctype = '<%=_QuerySType%>';
        var di_component_version = '<%=Global.diuilib_version%>';
        var di_theme_css = '<%=Global.diuilib_theme_css%>';    
        var di_diuilib_url = '<%=Global.diuilib_url%>';
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibincludes.js'" + "><\/script>");
        document.write("<script type='text/javascript' src='" + "<%=Global.diuilib_url%>" + "diuilibcommon.js'" + "><\/script>");
    </script>
	<script type="text/javascript" src="../js/Common.js"></script>
	<script type="text/javascript" src="../js/home.js"></script>
    
    
    <script type="text/javascript">
    
    var strAllDivs = '<div id="gridData" style="margin: 10px 20px 10px 0px; height: 230px; width: 750px; overflow: auto;"><div id="dataTitle" class="heading2"></div><div id="dataSubTitle" style="font-size: 10px; width: 100%; float: left;"></div><div id="content"></div><br /><div id="pivot" style="float: left; width: 100%; margin-top: 20px;"><div id="pivot_page" style="display: none;"></div><div id="pivot_content"></div><div id="pivot_chart"></div></div></div><div id="divCountDataRows" style="font-size: 10px;"></div><div style="font-size: 10px; margin-top: 8px;" id="divSingleSource"></div>';
    
    var strTabPivot = '<div id="tab_pivot333"><table width="100%" cellpadding="0" cellspacing="2" border="0" style="float:left;"><tr><td align="right"><div id="panelbar"><div id="pb_1"></div><div id="pb_11"></div></div></td></tr></table></div>';

// create pivoting object
var DEMO = {};
window.cal = false;
DEMO.pivot = {panel:1,tab:0,div:"pivot",needs:["pivot"],cb:CallBack}

// initiation function for the pivoting
function init(){	
   
	/* tabs */
	var tab = new OAT.Tab("content");
	for (var p in DEMO) {
		var name = DEMO[p].div;
		tab.add ("tab_" + name, name);
	}

	/* panelbar_content */
	var pb = new OAT.Panelbar("panelbar",10);
	pb.addPanel("pb_1","pb_11");
	
	/* create dimmer element */
	var dimmerElm = OAT.Dom.create("div",{border:"2px solid #000",padding:"1em",position:"absolute",backgroundColor:"#fff"});
	document.body.appendChild(dimmerElm);
	OAT.Dom.hide(dimmerElm);
	
	for (var p in DEMO) { DEMO[p].drawn = false; }
	tab.options.goCallback = function(oldIndex,newIndex) {
		var oldName, newName;
		for (var p in DEMO) {
			var v = DEMO[p];
			if (v.tab == oldIndex) { oldName = p; }
			if (v.tab == newIndex) { newName = p; }
		}
		
		
		var obj = DEMO[newName];
				//OAT.Dimmer.show(dimmerElm);
				OAT.Dom.center(dimmerElm,1,1);
				var ref = function() {  
					if (!window.location.href.match(/:source/)) {
					//OAT.Dimmer.hide(); 
					//alert('loading done');
					}				
				}				
				OAT.Loader.loadFeatures(obj.needs,ref);
				//alert('after loadfeatures');
			}
			pb.go(0);
			tab.go(0);
}

</script>
    <!-- END OF DEVELOPER CODE -->
</head>
<body style="margin-left:15px;"> 

<div id="vc_div" style="width: 800px; height: auto;"></div>



    <script type="text/javascript">


	function setVisualizerData(chartType)
	{
		var objOAT=OAT.getSelfInstance();
		var hc_chartTitle = z('dataTitle').innerHTML;
		var categoryArray = getCategory(objOAT);
		var seriesArray = getSeriesData(objOAT,categoryArray);		
		var chartData = {
		categoryCollection:categoryArray,
		seriesCollection:seriesArray
		}
		di_setVisualizerData('mac',chartData,chartType,hc_chartTitle);
		
	}	

    /* function show visulizer component */
    var clientHeight = (document.documentElement.clientHeight)-50;
	if(clientHeight > 600)
	   clientHeight = 600;
	       
	   
	
	   
	var hc_chartType = '<%=_QuerySType%>';
	switch(hc_chartType.toLowerCase())
    {
        case "column":
                di_draw_column_chart(
				  '',															// title of the chart
				  'mac',														// Unique Id for reference
				  '100%',														// Width
				  clientHeight,													// Height
				  'vc_div',														// Div id of the div in which the component HTML will be drawn.
				  '',															// Data collection in form of JSON
				  'j.obj',														// Data collection type j.obj/j.str (Json Object/Json String)
				  '',															// Label for X-Axis
				  '',															// Label for Y-Axis
				  ['Data','Settings','Series','Options','Save & Export'],		// Allowed tab names array
				  true,															// Allow to save
				  '',															// Url where data to be post for saving 			
				  '',															// URL of language code xml file
				  true,															// Is close button will show
				  'parent.hideVCSelection()',									// Hot function when click on close button
				  'stock/shared/Presentation/',                                 // Storage Path
				  '<%=_QueryType%>',														// Option to be uploaded data for [map/chart/scatter]
				  '../../stock/templates/column.txt',							// Default Chart setting
				  'normal',
				  '',
				  'OatInputFile.aspx'
				);
            break;
        case "line":
                di_draw_line_chart(
		        '',															// title of the chart
		        'mac',																	// Unique Id for reference
		        '100%',																	// Width
		        clientHeight,																	// Height
		        'vc_div',															// Div id of the div in which the component HTML will be drawn.
		        '',																	// Data collection in form of JSON
		        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
		        '',																// Label for X-Axis
		        '',																// Label for Y-Axis
		        ['Data','Settings','Series','Options','Save & Export'],		// Allowed tab names array
		        true,																	// Allow to save
		        '',																		// Url where data to be post for saving 			
		        '',
		        true,
		        'parent.hideVCSelection()',									// Hot function when click on close button
				 'stock/shared/Presentation/',                                 // Storage Path
				 '<%=_QueryType%>',														// Option to be uploaded data for [map/chart/scatter]
				 '../../stock/templates/line.txt',							// Default Chart setting
				 'normal',
				 '',
				 'OatInputFile.aspx'
	        );
            break;
        case "bar":
                di_draw_bar_chart(
		        '',															// title of the chart
		        'mac',																	// Unique Id for reference
		        '100%',																	// Width
		        clientHeight,																	// Height
		        'vc_div',															// Div id of the div in which the component HTML will be drawn.
		        '',																	// Data collection in form of JSON
		        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
		        '',																// Label for X-Axis
		        '',																// Label for Y-Axis
		        ['Data','Settings','Series','Options','Save & Export'],		// Allowed tab names array
		        true,																	// Allow to save
		        '',																		// Url where data to be post for saving 			
		        '',
		        true,
		        'parent.hideVCSelection()',									// Hot function when click on close button
				  'stock/shared/Presentation/',                                 // Storage Path
				  '<%=_QueryType%>',														// Option to be uploaded data for [map/chart/scatter]
				  '../../stock/templates/bar.txt',							// Default Chart setting
				  'normal',
				  '',
				  'OatInputFile.aspx'
	        );
            break;
        case "area":
            di_draw_area_chart(
		        '',															// title of the chart
		        'mac',																	// Unique Id for reference
		        '100%',																	// Width
		        clientHeight,																	// Height
		        'vc_div',															// Div id of the div in which the component HTML will be drawn.
		        '',																	// Data collection in form of JSON
		        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
		        '',																// Label for X-Axis
		        '',																// Label for Y-Axis
		        ['Data','Settings','Series','Options','Save & Export'],		// Allowed tab names array
		        true,																	// Allow to save
		        '',																		// Url where data to be post for saving 			
		        '',
		        true,
		        'parent.hideVCSelection()',									// Hot function when click on close button
				  'stock/shared/Presentation/',                                 // Storage Path
				  '<%=_QueryType%>',														// Option to be uploaded data for [map/chart/scatter]
				  '../../stock/templates/area.txt',							// Default Chart setting
				  'normal',
				  '',
				  'OatInputFile.aspx'
	        );
	        break;
        case "pie":
        di_draw_pie_chart(
	        '',															// title of the chart
	        'mac',																	// Unique Id for reference
	        '100%',																	// Width
	        clientHeight,																	// Height
	        'vc_div',															// Div id of the div in which the component HTML will be drawn.
	        '',																	// Data collection in form of JSON
	        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
	        '',																// Label for X-Axis
	        '',																// Label for Y-Axis
	        ['Data','Settings','Series','Options','Save & Export'],		// Allowed tab names array
	        true,																	// Allow to save
	        '',																		// Url where data to be post for saving 			
	        '',
	        true,
	        'parent.hideVCSelection()',									// Hot function when click on close button
				  'stock/shared/Presentation/',                                 // Storage Path
				  '<%=_QueryType%>',														// Option to be uploaded data for [map/chart/scatter]
				  '../../stock/templates/area.txt',							// Default Chart setting
				  '',
				  'OatInputFile.aspx'
	        );
	        
            break;
       case "donut":
            di_draw_donut_chart(
		        '',															// title of the chart
		        'do',																	// Unique Id for reference
		        '100%',																	// Width
		        '600',																	// Height
		        hc_ctrlId,															// Div id of the div in which the component HTML will be drawn.
		        chartData,																	// Data collection in form of JSON
		        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
		        xAxisTitle,																// Label for X-Axis
		        yAxisTitle,																// Label for Y-Axis
		        ['Settings','Series','Options','Save & Export'],		// Allowed tab names array
		        true,																	// Allow to save
		        '',																		// Url where data to be post for saving 			
		        '',
		        true,
		        'closeChartPopup()',
		        'shareVisualizer',                                           // Share Hot Selection Function
				'chart',
				'../../stock/templates/donut.txt',
				'source'
	        );
	        break;
	   case "stackcolumn":
            di_draw_column_chart(
		        '',															// title of the chart
		        'stcl',																	// Unique Id for reference
		        '100%',																	// Width
		        '600',																	// Height
		        hc_ctrlId,															// Div id of the div in which the component HTML will be drawn.
		        chartData,																	// Data collection in form of JSON
		        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
		        xAxisTitle,																// Label for X-Axis
		        yAxisTitle,																// Label for Y-Axis
		        ['Settings','Series','Options','Save & Export'],		// Allowed tab names array
		        true,																	// Allow to save
		        '',																		// Url where data to be post for saving 			
		        '',
		        true,
		        'closeChartPopup()',
		        'shareVisualizer',                                           // Share Hot Selection Function
				'chart',
				'../../stock/templates/column.txt',
				'stacking',
				'source'
	        );
	        break;
	   case "stackline":
            di_draw_line_chart(
		        '',															// title of the chart
		        'stli',																	// Unique Id for reference
		        '100%',																	// Width
		        '600',																	// Height
		        hc_ctrlId,															// Div id of the div in which the component HTML will be drawn.
		        chartData,																	// Data collection in form of JSON
		        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
		        xAxisTitle,																// Label for X-Axis
		        yAxisTitle,																// Label for Y-Axis
		        ['Settings','Series','Options','Save & Export'],		// Allowed tab names array
		        true,																	// Allow to save
		        '',																		// Url where data to be post for saving 			
		        '',
		        true,
		        'closeChartPopup()',
		        'shareVisualizer',                                           // Share Hot Selection Function
				'chart',
				'../../stock/templates/line.txt',
				'stacking',
				'source'
	        );
	        break;
	   case "stackbar":
            di_draw_bar_chart(
		        '',															// title of the chart
		        'stba',																	// Unique Id for reference
		        '100%',																	// Width
		        '600',																	// Height
		        hc_ctrlId,															// Div id of the div in which the component HTML will be drawn.
		        chartData,																	// Data collection in form of JSON
		        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
		        xAxisTitle,																// Label for X-Axis
		        yAxisTitle,																// Label for Y-Axis
		        ['Settings','Series','Options','Save & Export'],		// Allowed tab names array
		        true,																	// Allow to save
		        '',																		// Url where data to be post for saving 			
		        '',
		        true,
		        'closeChartPopup()',
		        'shareVisualizer',                                           // Share Hot Selection Function
				'chart',
				'../../stock/templates/bar.txt',
				'stacking',
				'source'
	        );
	        break;
	   case "stackarea":
            di_draw_area_chart(
		        '',															// title of the chart
		        'star',																	// Unique Id for reference
		        '100%',																	// Width
		        '600',																	// Height
		        hc_ctrlId,															// Div id of the div in which the component HTML will be drawn.
		        chartData,																	// Data collection in form of JSON
		        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
		        xAxisTitle,																// Label for X-Axis
		        yAxisTitle,																// Label for Y-Axis
		        ['Settings','Series','Options','Save & Export'],		// Allowed tab names array
		        true,																	// Allow to save
		        '',																		// Url where data to be post for saving 			
		        '',
		        true,
		        'closeChartPopup()',
		        'shareVisualizer',                                           // Share Hot Selection Function
				'chart',
				'../../stock/templates/area.txt',
				'stacking',
				'source'
	        );
	        break;
	   case "100stackcolumn":
            di_draw_column_chart(
		        '',															// title of the chart
		        '100stcl',																	// Unique Id for reference
		        '100%',																	// Width
		        '600',																	// Height
		        hc_ctrlId,															// Div id of the div in which the component HTML will be drawn.
		        chartData,																	// Data collection in form of JSON
		        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
		        xAxisTitle,																// Label for X-Axis
		        yAxisTitle,																// Label for Y-Axis
		        ['Settings','Series','Options','Save & Export'],		// Allowed tab names array
		        true,																	// Allow to save
		        '',																		// Url where data to be post for saving 			
		        '',
		        true,
		        'closeChartPopup()',
		        'shareVisualizer',                                           // Share Hot Selection Function
				'chart',
				'../../stock/templates/column.txt',
				'percent',
				'source'
	        );
	        break;
	   case "100stackline":
            di_draw_line_chart(
		        '',															// title of the chart
		        '100stli',																	// Unique Id for reference
		        '100%',																	// Width
		        '600',																	// Height
		        hc_ctrlId,															// Div id of the div in which the component HTML will be drawn.
		        chartData,																	// Data collection in form of JSON
		        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
		        xAxisTitle,																// Label for X-Axis
		        yAxisTitle,																// Label for Y-Axis
		        ['Settings','Series','Options','Save & Export'],		// Allowed tab names array
		        true,																	// Allow to save
		        '',																		// Url where data to be post for saving 			
		        '',
		        true,
		        'closeChartPopup()',
		        'shareVisualizer',                                           // Share Hot Selection Function
				'chart',
				'../../stock/templates/line.txt',
				'percent',
				'source'
	        );
	        break;
	   case "100stackbar":
            di_draw_bar_chart(
		        '',															// title of the chart
		        '100stba',																	// Unique Id for reference
		        '100%',																	// Width
		        '600',																	// Height
		        hc_ctrlId,															// Div id of the div in which the component HTML will be drawn.
		        chartData,																	// Data collection in form of JSON
		        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
		        xAxisTitle,																// Label for X-Axis
		        yAxisTitle,																// Label for Y-Axis
		        ['Settings','Series','Options','Save & Export'],		// Allowed tab names array
		        true,																	// Allow to save
		        '',																		// Url where data to be post for saving 			
		        '',
		        true,
		        'closeChartPopup()',
		        'shareVisualizer',                                           // Share Hot Selection Function
				'chart',
				'../../stock/templates/bar.txt',
				'percent',
				'source'
	        );
	        break;
	   case "100stackarea":
            di_draw_area_chart(
		        '',															// title of the chart
		        '100star',																	// Unique Id for reference
		        '100%',																	// Width
		        '600',																	// Height
		        hc_ctrlId,															// Div id of the div in which the component HTML will be drawn.
		        chartData,																	// Data collection in form of JSON
		        'j.obj',																// Data collection type j.obj/j.str (Json Object/Json String)
		        xAxisTitle,																// Label for X-Axis
		        yAxisTitle,																// Label for Y-Axis
		        ['Settings','Series','Options','Save & Export'],		// Allowed tab names array
		        true,																	// Allow to save
		        '',																		// Url where data to be post for saving 			
		        '',
		        true,
		        'closeChartPopup()',
		        'shareVisualizer',                                           // Share Hot Selection Function
				'chart',
				'../../stock/templates/area.txt',
				'percent',
				'source'
	        );
	        break;
    }       
	        
    function drawOATGrid(data, divid){
        if(z('divGridContainer').innerHTML.length>0)
        {
            testInnerHtml = z('divGridContainer').innerHTML;
        }
        z(divid).innerHTML = testInnerHtml;
        z('divGridContainer').innerHTML = "";
        get_data1(data);
    }
     
    </script>
   
   
 <div id="divGridContainer" style="float: left; padding-left: 20px; padding-bottom: 5px; visibility: hidden;" class="roundedcorners">
                    <div id="gridData" style="margin: 10px 20px 10px 0px; height: 90%; width: 730px;">
                        <div id="dataTitle" class="heading2">
                        </div>
                        <div id="dataSubTitle" style="font-size: 10px; width: 100%; float: left;">
                        </div>
                        <div id="content">
                        </div><br />
                        <div id="pivot" style="float: left; width: 100%; margin-top: 20px;">
                            <div id="pivot_page" style="display: none;">
                            </div>
                            <div id="pivot_content">
                            </div>
                            <div id="pivot_chart">
                            </div>
                        </div>
                        <div id="divCountDataRows" style="font-size: 10px;">
                    </div>
                    </div>
                    
                    <div style="font-size: 10px; margin-top: 8px;" id="divSingleSource">
                    </div>
                </div>
                
                
                
                
                <div id="tab_pivot333">
                         <table width="100%" cellpadding="0" cellspacing="2" border="0" style="float:left;">
                             <tr>
                                 <td align="right">
                                     <div id="panelbar">
                                         <div id="pb_1">
                                         </div>
                                         <div id="pb_11">
                                         </div>
                                     </div>
                                 </td>
                             </tr>
                         </table>
                     </div>
                

</body>
</html>
