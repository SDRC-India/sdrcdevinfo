/*
DevInfo 7
map.js to create map component
*/

/* class name for map component */
/*function di_Map( width, height, seriesData, areaIds, divId, dbnid, dblngid)
{
	this.delemeter = "[****]";
	this.mapWidth = width;
	this.mapHeight = height;
	this.seriesData = seriesData;
	this.areaIds = areaIds;
	this.divId = divId;
	this.dbnid = dbnid;
	this.dblngid = dblngid;
	
	// hardcoded variables
	this.callback = 'Callback.aspx';
	this.callbackNo = 200;
	this.imageUrl = '';
	this.mode = '';
	this.startQuard = ''; // x, y quardinates underscore( _ ) separated
	this.endQuard = ''; // x, y quardinates underscore( _ ) separated
	this.clientObj = '';
	this.isDataLabel = false;
	this.areaInfoArray = [];
	this.mapObjectName = "mapObj";
	this.tootTipDivId = "dv_container_map_areainfo";
	this.nudging = true;

	this.responseTxt = this.callBack( {'callback':this.callbackNo, 'param1':this.mapWidth, 'param2':this.mapHeight, 'param3':this.dbnid, 'param4':this.dblngid} );
	
	this.drawMap(this.responseTxt);

}*/
function di_Map()
{
	var btype = '';
	var ua = window.navigator.userAgent;
	var msie = ua.indexOf ( "MSIE " )
    if ( msie > 0 )      // If Internet Explorer, return version number
        btype = 'ie'+parseInt (ua.substring (msie+5, ua.indexOf (".", msie )));
    else                 // If another browser, return 0
       btype = '';

	this.bType = btype; // browser type empty for all prowser except IE
	this.delemeter = "[****]";
	this.mapWidth = 600;
	this.mapHeight = 400;
	this.seriesData = '';
	this.areaIds = '';
	this.divId = '';
	this.dbnid = '';
	this.dblngid = '';
	// hardcoded variables
	this.callback = 'Callback.aspx?bType='+btype;
	this.callbackNo = 200;
	this.imageUrl = '';
	this.mode = '';
	this.startQuard = ''; // x, y quardinates underscore( _ ) separated
	this.endQuard = ''; // x, y quardinates underscore( _ ) separated
	this.clientObj = '';
	this.isDataLabel = false;
	this.areaInfoArray = [];
	this.mapObjectName = "mapObj";
	this.tootTipDivId = "dv_container_map_areainfo";
	this.nudging = true;
	this.maptype = 'multiple';
}
/* prototype function to init map only
input: post data (width, height, dbnid and dblngnid)
output: null
*/
di_Map.prototype.initMap = function( width, height, dbnid, dblngid ) {
	
	this.mapWidth = width;
	this.mapHeight = height;
	this.dbnid = dbnid;
	this.dblngid = dblngid;

	this.callBack( {'callback':250, 'param1':this.mapWidth, 'param2':this.mapHeight, 'param3':this.dbnid, 'param4':this.dblngid} );
}

/* prototype function to draw map image
input: post data
output: draw map image
*/
di_Map.prototype.drawMapImage = function( width, height, seriesData, areaIds, divId, dbnid, dblngid, maptype ) {
	
	this.mapWidth = width;
	this.mapHeight = height;
	this.seriesData = seriesData;
	this.areaIds = areaIds;
	this.divId = divId;
	this.dbnid = dbnid;
	this.dblngid = dblngid;
	this.maptype = maptype;

	this.responseTxt = this.callBack( {'callback':this.callbackNo, 'param1':this.mapWidth, 'param2':this.mapHeight, 'param3':this.dbnid, 'param4':this.dblngid, 'param5':this.maptype} );
	
	this.drawMap(this.responseTxt);
}

/* function to draw map
input: image url
*/
di_Map.prototype.drawMap = function(imgUrl) { 

	try
	{
		var imgStyle = '';
		var img = document.createElement('img');
		img.id = "diMapImg";
		if(this.mode=='zin') {
			imgStyle = '-moz-zoom-in';
		}
		else if(this.mode=='zout') {
			imgStyle = '-moz-zoom-out';
		}
		else if(this.mode=='pan') {
			imgStyle = '-moz-grab';
		}
		else if(this.mode=='zext') {
			imgStyle = 'crosshair';
		}
		img.style.cursor = imgStyle;
		//img.src = 'data:image/png;base64,' + imgUrl;
		img.src = imgUrl;
		di_jq('#'+this.divId).html(img);

		if(this.mode=='pan' && this.clientObj!='') {
			//di_jq('#diMapImg').draggable( 'enable' );
			this.setPanZoom(this.clientObj);
		}
		if(this.mode=='zext' && this.clientObj!='') {
			//di_jq('#diMapImg').draggable( 'enable' );
			this.setRubberZoom(this.clientObj);
		}

		if(this.isDataLabel) {
			var tooltipId = this.tootTipDivId;
			var getlayerInfo = this.GetLayerTaggingInformation();
			
			if(getlayerInfo!='false') {
				di_jq('#diMapImg').attr("usemap", "#map");
				di_jq('#'+this.divId).append( this.createMapInfoHTML(getlayerInfo) );

				// hard code in the component. (it should be in hosting apps)
				di_jq('area').each(function(e) { 
					if(this.id!='' && this.id!=undefined) {
					di_jq(this).easyTooltip({
						useElement: tooltipId,
						yOffset: 70,
						xOffset: 35
						//tooltipId:tooltipId
					});
					}
				});
				//
			}
			else {
				this.nudging = false;
			}
		}

		if(this.bType=='ie8') this.deleteFile(imgUrl);
	}
	catch (err){}
}

/* prototype function to send callback 
input: post data
output: response text
*/
di_Map.prototype.callBack = function (postdata) {
	/// wait 3 seconds
	/*setTimeout(function() {
		loadingVisualization(true);

		/// wait 3 seconds
		//setTimeout(function() {
			//loadingVisualization(false);
		//}, 500);
	}, 3000);*/

	//var handle = setTimeout(myFunction, 5000);
	//loadingVisualization(true);

	//delayTime(5000);

	var response = '';
	//var cc = this.callback;
	//setTimeout(function() {	
	di_jq.ajax({
          type: "POST",
          url: this.callback,
          data: postdata,
          async:false,
		  beforeSend:function(){ 
			//loadingVisualization(true);
		  },
          success: function(res){
              try
              {
                  response = res;
			  }
              catch(ex){
                  alert("Error : " + ex.message);
              }
			  //loadingVisualization(false);
          },
          error:function(){
              alert("Error occured");
          },
          cache: false
    });
	//}, 500); alert(response);
	return response;
}
function delayTime(time) {
  var d1 = new Date();
  var d2 = new Date();
  while (d2.valueOf() < d1.valueOf() + time) {
    d2 = new Date();
  }
}

/* prototype common function for zoom in/out, pan, fullextent, rabur zoom on map
note: depends on this.mode. (not in use)
input: inharit variables
output: re-draw map
*/
di_Map.prototype.mapNav = function() {
	if(this.mode=='zin') {
		this.zoomIn();
	}
	else if(this.mode=='zout') {
		this.zoomOut();
	}
	else if(this.mode=='full') {
		this.setFullExtent();
	}
	else if(this.mode=='zrabur') {
	}
	else if(this.mode=='pan') {
	}
}

/* prototype function to zoom in map
input: ''
output: re-draw map
*/
di_Map.prototype.zoomIn = function(x, y) {
	this.responseTxt = this.callBack( {'callback':201, 'param1':x, 'param2':y} );
	this.drawMap(this.responseTxt);
}
/* prototype function to zoom out map
input: ''
output: re-draw map
*/
di_Map.prototype.zoomOut = function(x, y) {
	this.responseTxt = this.callBack( {'callback':202, 'param1':x, 'param2':y} );
	this.drawMap(this.responseTxt);
}
/* prototype function to ser full extent
input: ''
output: re-draw map
*/
di_Map.prototype.setFullExtent = function() {
	this.responseTxt = this.callBack( {'callback':205} );
	this.drawMap(this.responseTxt);
}
/* prototype function to set data labels
input: ''
output: re-draw map
*/
di_Map.prototype.setDataLabels = function(val) {
	this.isDataLabel = val;
	this.responseTxt = this.callBack( {'callback':206, 'param1':val} );
	this.drawMap(this.responseTxt);
}
/* prototype function to set border
input: ''
output: re-draw map
*/
di_Map.prototype.setBorder = function(bdrWait) {
	this.responseTxt = this.callBack( {'callback':207, 'param1':bdrWait} );
	this.drawMap(this.responseTxt);
}
/* prototype function to set north symbol
input: ''
output: re-draw map
*/
di_Map.prototype.setNorthSymbol = function(val) {
	this.responseTxt = this.callBack( {'callback':208, 'param1':val} );
	this.drawMap(this.responseTxt);
}
/* prototype function to set scale
input: ''
output: re-draw map
*/
di_Map.prototype.setScale = function(val) {
	this.responseTxt = this.callBack( {'callback':209, 'param1':val} );
	this.drawMap(this.responseTxt);
}
/* prototype function to set pan zoom
input: ''
output: re-draw map
*/
di_Map.prototype.setPan = function(x1, y1, x2, y2) {
	this.responseTxt = this.callBack( {'callback':204, 'param1':x1, 'param2':y1, 'param3':x2, 'param4':y2} );
	this.drawMap(this.responseTxt);
}
/* prototype function to set rubber zoom
input: ''
output: re-draw map
*/
di_Map.prototype.setRubber = function(x1, y1, x2, y2) {
	this.responseTxt = this.callBack( {'callback':203, 'param1':x1, 'param2':y1, 'param3':x2, 'param4':y2} );
	this.drawMap(this.responseTxt);
}
/* prototype function to set panning
input: ''
output: re-draw map
*/
di_Map.prototype.setPanZoom = function(obj) {
	var startX = '';
	var startY = '';
	var endX = '';
	var endY = '';
	this.clientObj = obj;
	di_jq("#diMapImg").draggable({ 
		// Find original position of dragged image.
		start: function(event, ui) {
			//var offset = di_jq(this).offset();
			//var x = e.pageX - this.offsetLeft;
			//var y = e.pageY - this.offsetTop;
			//startX = event.clientX - offset.left;
			//startY = event.clientY - offset.top;
			//di_jq('body').prepend('Start - ' +x+'-'+y+'<br>');
			var currentPos = ui.helper.position();
			startX = parseInt(currentPos.left);
			startY = parseInt(currentPos.top);
		},
		// Find position where image is dropped.
		stop: function(event, ui) {
			//var offset = di_jq(this).offset();
			//endX = event.clientX - offset.left;
			//endY = event.clientY - offset.top;
			//di_jq('body').prepend('Stop - ' + x+'-'+y+'<br>');
			var currentPos = ui.helper.position();
			endX = parseInt(currentPos.left);
			endY = parseInt(currentPos.top);

			//alert(startX + ' - ' + startY + " <BR> " + endX +' - '+ endY);
			obj.setPan(startX, startY, endX, endY);
		}
	});
}
/* prototype function to set rubber zoom
input: ''
output: re-draw map
*/
di_Map.prototype.setRubberZoom = function(obj) {
	var startX = '';
	var startY = '';
	var endX = '';
	var endY = '';
	this.clientObj = obj;
	di_jq("#diMapImg").selectable({ 
		// Find original position of dragged image.
		start: function(event, ui) {
			var offset = di_jq(this).offset();
			//var x = e.pageX - this.offsetLeft;
			//var y = e.pageY - this.offsetTop;
			startX = Math.round(event.pageX - offset.left);
			startY = Math.round(event.pageY - offset.top);
		},
		// Find position where image is dropped.
		stop: function(event, ui) {
			var offset = di_jq(this).offset();
			endX = Math.round(event.pageX - offset.left);
			endY = Math.round(event.pageY - offset.top);
			
			if(mapObj.mode!='zext') {
				if(mapObj.mode=='zin') mapObj.zoomIn(endX, endY);
				if(mapObj.mode=='zout') mapObj.zoomOut(endX, endY);
			}
			else {
				obj.setRubber(startX, startY, endX, endY);
			}
		}
	});
}
/* prototype function to get theme info
input:
*/
di_Map.prototype.getThemeInfo = function(themeID) {
	var themeInfo = this.callBack( {'callback':211, 'param1':themeID});
	return themeInfo;
}
/* prototype function to get series info
input:
*/
di_Map.prototype.getSeriesInfo = function() {
	var seriesInfo = this.callBack( {'callback':213} );
	return seriesInfo;
}
/* prototype function to set legend color
input: ''
output: re-draw map
*/
di_Map.prototype.setLegendColor = function(themeId, index, color) {
	this.responseTxt = this.callBack( {'callback':212, 'param1':themeId, 'param2':index, 'param3':color} );
	this.drawMap(this.responseTxt);
}
/* prototype function to refresh map if change series/break type/break count
input: ''
output: re-draw map
*/
di_Map.prototype.setSeriesNBreaks = function(themeId, seriesID, breakType, breakCount) {
	this.responseTxt = this.callBack( {'callback':214, 'param1':themeId, 'param2':seriesID, 'param3':breakType, 'param4':breakCount} );
	this.drawMap(this.responseTxt);
}
/* prototype function to resize map
input: ''
output: re-draw map
*/
di_Map.prototype.setMapSize = function(width, height) {
	this.responseTxt = this.callBack( {'callback':210, 'param1':width, 'param2':height} );
	this.drawMap(this.responseTxt);
}
/* prototype function to get map title info
input: ''
output: string
*/
di_Map.prototype.getMapTitleInfo = function() {
	var titleInfo = this.callBack( {'callback':215} );
	return titleInfo;
}
/* prototype function to get map disclaimer info
input: ''
output: string
*/
di_Map.prototype.getMapDisclaimerInfo = function() {
	var disclaimerInfo = this.callBack( {'callback':216} );
	return disclaimerInfo;
}
/* prototype function to get map data labels info
input: ''
output: string
*/
di_Map.prototype.getMapDataLblInfo = function(label) {
	var dataLblInfo = this.callBack( {'callback':219, 'param1':label} );
	return dataLblInfo;
}
/* prototype function to set map title info
input: ''
output: string
*/
di_Map.prototype.setMapTitleInfo = function(setStr) {
	this.callBack( {'callback':217, 'param1':setStr} );
}
/* prototype function to set map disclaimer info
input: ''
output: string
*/
di_Map.prototype.setMapDisclaimerInfo = function(setStr) {
	this.callBack( {'callback':218, 'param1':setStr} );
}
/* prototype function to set map data labels info
input: ''
output: string
*/
di_Map.prototype.setMapDataLblInfo = function(setStr, redraw, label) {
	if(redraw) {
		this.responseTxt = this.callBack( {'callback':220, 'param1':setStr, 'param2':true, 'param3':label} );
		this.drawMap(this.responseTxt);
	}
	else{
		this.callBack( {'callback':220, 'param1':setStr, 'param2':false, 'param3':label} );
	}
}
/* prototype function to export map
input: type
output: download
*/
di_Map.prototype.downloadMap = function(type, keywords) {
	this.callBack( {'callback':221, 'param1':type, 'param2':keywords} );
}

/* prototype function to just refresh the map image
input: type
output: download
*/
di_Map.prototype.refreshMap = function() {
	this.drawMap(this.responseTxt);
}
/* prototype function to set series color
input: colorCode
output: redraw chart
*/
di_Map.prototype.setSeriesColor = function(themeId, startColor, endColor) {
	this.responseTxt = this.callBack( {'callback':222, 'param1':themeId, 'param2':startColor, 'param3':endColor} );
	this.drawMap(this.responseTxt);
}
/* prototype function to get border style
input: colorCode
output: redraw chart
*/
di_Map.prototype.getBorderStyle = function() {
	var borderStyle = this.callBack( {'callback':224} );
	return borderStyle;
}
/* prototype function to set border style
input: colorCode
output: redraw chart
*/
di_Map.prototype.setBorderStyle = function(showBdr, width, color, style) {
	this.responseTxt = this.callBack( {'callback':223, 'param1':showBdr, 'param2':width, 'param3':color, 'param4':style} );
	this.drawMap(this.responseTxt);
}
/* prototype function to get label style
input: colorCode
output: redraw chart
*/
di_Map.prototype.getLabelStyle = function() {
	var labelStyle = this.callBack( {'callback':226} );
	return labelStyle;
}
/* prototype function to set label style
input: colorCode
output: redraw chart
*/
di_Map.prototype.setLabelStyle = function(labelString, isShowLabel) {
	// for is data label on
	if(isShowLabel=="yes") this.isDataLabel = true; else this.isDataLabel = false;

	this.responseTxt = this.callBack( {'callback':225, 'param1':labelString} );
	this.drawMap(this.responseTxt);
}

/* prototype function to get image path
input: colorCode
output: return path
*/
di_Map.prototype.getMapImagePath = function() {
	var imagePath = this.callBack( {'callback':228} );
	return imagePath;
}

/* prototype function to add new theme
input: string
output: redraw chart
*/
di_Map.prototype.addMapTheme = function(inputString) {
	this.responseTxt = this.callBack( {'callback':229, 'param1':inputString} );
	if(this.responseTxt=='exist') {
		alert('Theme already exists.');
		return false;
	}
	else {
		this.drawMap(this.responseTxt);
	}
}

/* prototype function to edit theme
input: string
output: redraw chart
*/
di_Map.prototype.ediMapTheme = function(themeId, inputString, ismrd, selectedTP) {
	this.responseTxt = this.callBack( {'callback':232, 'param1':themeId, 'param2':inputString, 'param3':ismrd, 'param4':selectedTP} );
	if(this.responseTxt=='exist') {
		alert('Theme already exists.');
		return false;
	}
	else {
		this.drawMap(this.responseTxt);
	}
}

di_Map.prototype.UpdateThemeLegends=function(themeId,inputString,timeP,persistLegends){this.responseTxt=this.callBack({'callback':1034,'param1':themeId,'param2':inputString,'param3':timeP,'param4':persistLegends});if(this.responseTxt=='exist'){alert('Theme already exists.');return false;}else{this.drawMap(this.responseTxt);}}

/* prototype function to delete map theme
input: string
output: redraw chart
*/
di_Map.prototype.deleteMapTheme = function(themeId) {
	this.responseTxt = this.callBack( {'callback':231, 'param1':themeId} );
	this.drawMap(this.responseTxt);
}

/* prototype function to change visibility of map theme
input: string
output: redraw chart
*/
di_Map.prototype.visibleMapTheme = function(themeId, visible) {
	this.responseTxt = this.callBack( {'callback':230, 'param1':themeId, 'param2':visible} );
	this.drawMap(this.responseTxt);
}

/* prototype function to change details of dot density of map theme
input: string
output: redraw chart
*/
di_Map.prototype.setDotDensityDetails = function(themeId, inputStr) {
	this.responseTxt = this.callBack( {'callback':234, 'param1':themeId, 'param2':inputStr} );
	this.drawMap(this.responseTxt);
}

/* prototype function to set Hatch patern of map theme
input: string
output: redraw chart
*/
di_Map.prototype.setHatchPatern = function(themeId, legendIndex, paternType) {
	this.responseTxt = this.callBack( {'callback':237, 'param1':themeId, 'param2':legendIndex, 'param3':paternType} );
	this.drawMap(this.responseTxt);
}

/* prototype function to set legend settings
input: string
output: redraw chart
*/
di_Map.prototype.GetLegendBySettingChange = function(themeId, inputStr) {
	this.responseTxt = this.callBack( {'callback':236, 'param1':themeId, 'param2':inputStr} );
	this.drawMap(this.responseTxt);
}

/* prototype function to get calculated range after changing range to of legend
input: string
output: return
*/
di_Map.prototype.GetLegendRanges = function(themeId, legendIndex, rangeTo) {
	var responseTxt = this.callBack( {'callback':235, 'param1':themeId, 'param2':legendIndex, 'param3': rangeTo} );
	return responseTxt;
}

/* prototype function to set theme legend
input: string
output: redraw chart
*/
di_Map.prototype.setThemeLegend = function(themeId, inputStr) {
	this.responseTxt = this.callBack( {'callback':238, 'param1':themeId, 'param2':inputStr} );
	this.drawMap(this.responseTxt);
}

/* prototype function to set theme legendget smooth colors
input: string
output: return
*/
di_Map.prototype.getSmoothColors = function(themeId, firstColor, endColor) {
	var responseTxt = this.callBack( {'callback':239, 'param1':themeId, 'param2':firstColor, 'param3':endColor} );
	return responseTxt;
}

/* prototype function to set chart theme settings
input: string
output: redraw chart
*/
di_Map.prototype.setChartSettings = function(themeId, type, inputStr) {
	this.responseTxt = this.callBack( {'callback':240, 'param1':themeId, 'param2':type, 'param3':inputStr} );
	this.drawMap(this.responseTxt);
}

/* prototype function to set chart theme legend's settings
input: string
output: redraw chart
*/
di_Map.prototype.setChartLegendSettings = function(themeId, settingStr, legendStr, tmpList) {
	this.responseTxt = this.callBack( {'callback':241, 'param1':themeId, 'param2':settingStr, 'param3':legendStr, 'param4':tmpList} );
	this.drawMap(this.responseTxt);
}

/* prototype function to set map image legend title/body
input: string
output: return
*/
di_Map.prototype.setImageLegendTitle = function(type, inputStr) {
	responseTxt = this.callBack( {'callback': 246, 'param1':type, 'param2':inputStr} );
	return responseTxt;
}

/* prototype function to get map image legend title/body
input: string
output: return
*/
di_Map.prototype.getImageLegendTitle = function(type) {
	responseTxt = this.callBack( {'callback': 247, 'param1':type} );
	return responseTxt;
}

/* prototype function to get map layer tagging information
input: string
output: return
*/
di_Map.prototype.GetLayerTaggingInformation = function() {
	responseTxt = this.callBack( {'callback': 243} );
	return responseTxt;
}

/* prototype function to get map area information information
input: string
output: return
*/
di_Map.prototype.GetAreaInformation = function(areaId) {
	if(this.areaInfoArray[areaId]!='' && this.areaInfoArray[areaId]!=undefined) {
		return this.areaInfoArray[areaId];
	}
	else {
		responseTxt = this.callBack( {'callback': 244, 'param1':areaId} );
		this.areaInfoArray[areaId] = responseTxt;
		return responseTxt;
	}
}

/* prototype function to mouse hover on map area tooltip
input: string
output: return
*/
di_Map.prototype.showMapAreaInfo = function(areaId) {
	
	var areaInfo = this.GetAreaInformation(areaId);
	if(areaInfo!='false') {
		di_jq("#"+this.tootTipDivId).html(this.getAreaInfoHTML(areaInfo));
	}
	else {
		var areaid = di_jq('#'+areaId).attr('id');
		var areaname = di_jq('#'+areaId).attr('title');
		var string = areaname + ' ('+areaid+')' + this.delemeter + '' + this.delemeter + '' + this.delemeter;
		di_jq("#"+this.tootTipDivId).html(this.getAreaInfoHTML(string));
	}
}

/* prototype function to mouse hover on map area tooltip
input: string
output: return
*/
di_Map.prototype.getAreaInfoHTML = function(areaInfo) {
	
	var areaInfoAr = areaInfo.split(mapDataDelemeter);
	var areaTitle, areaTime, areaSeries, areaSource;
	(areaInfoAr[0]!='') ? areaTitle = areaInfoAr[0] : areaTitle = 'NA';
	(areaInfoAr[1]!='') ? areaTime = areaInfoAr[1] : areaTime = 'NA';
	(areaInfoAr[2]!='') ? areaSeries = areaInfoAr[2] : areaSeries = 'NA';
	(areaInfoAr[3]!='') ? areaSource = areaInfoAr[3] : areaSource = 'NA';
	var html = 'Error';
	if(areaInfoAr[0]!='false') {
		html = '<div class="area_callout_title">'+ areaTitle +'</div>';
		html += '<div class="area_callout_text">'+ areaTime +'<br />'+ areaSeries +' <br/>'+ areaSource +'</div>';
		html += '<b class="area_notch"></b>';
	}
	return html;
}

/* prototype function to set label nudging on map iamge
input: string
output: redraw chart
*/
di_Map.prototype.setLabelNudging = function(areaId, inputStr) {
	this.responseTxt = this.callBack( {'callback':248, 'param1':areaId, 'param2':inputStr} );
	this.drawMap(this.responseTxt);
}

/* prototype function to create mapInfo HTML from JSON
input: 
output: response text
*/
di_Map.prototype.createMapInfoHTML = function(jsonStr){ 
	var mapInfo = '<map name="map">';
	if(jsonStr!='') {
		var jObj = di_jq.parseJSON(jsonStr);
		var areaObj = jObj.area;
		if(areaObj.length>0) {
			for(var i=0; i<areaObj.length; i++) { 
				var areaString = areaObj[i].split(this.delemeter);
				var areaId = areaString[0]; // area id
				var areaName = areaString[1]; // area name
				var areaCord = areaString[2]; // area quardinates

				if(areaId!='') {
					mapInfo += '<area shape="rect" coords="'+areaCord+'" id="'+areaId+'" title="'+areaName+'" onmouseover=\''+this.mapObjectName+'.showMapAreaInfo("'+areaId+'")\' onclick=\''+this.mapObjectName+'.mapLabelNudging("'+areaId+'")\' style="cursor:pointer" />';
				}

			} // end for
		} // end if
	} // end if 

	mapInfo += '</map>';

	return mapInfo;
}

/* prototype function to map label Nudging
input: 
output: response text
*/
di_Map.prototype.mapLabelNudging = function(areaId){
	// if mode is nudging
	di_jq("#nudging_label").remove(); 
	var delemeter = this.delemeter;
	if(this.mode=='nudge' && this.nudging==true) {
		var setNudging = this.mapObjectName + '.setLabelNudging';
		var areaName = di_jq('#'+areaId).attr('title');
		//var subnavioffset = di_jq('#'+areaId).position();
		var imgposset = di_jq('#diMapImg').position();
		var coords = di_jq('#'+areaId).attr('coords').toString();
		var coordsAr = coords.split(',');
		var left1 = coordsAr[0];
		var left2 = coordsAr[2];
		var top1 = coordsAr[1];
		var top2 = coordsAr[3];
		var width = Math.round(left2 - left1);
		var height = Math.round(top2 - top1);
		di_jq("#"+this.divId).append("<div id='nudging_label'>"+areaName+"</div>");		
		di_jq("#nudging_label")
			.css("position","absolute")
			//.css("top",(e.pageY - 5)+ "px")
			//.css("left",e.pageX + "px")
			.css("left",Math.round(Number(left1) + Number(imgposset.left))+ "px")
			.css("top",Math.round(Number(top1) + Number(imgposset.top)) + "px")
			.css("display","none")
			.css("border","1px solid #000000")
			.css("width",width+"px")
			.css("background-color","white")
			.css("opacity","0.3")
			.css("height",height+"px")
			.css("cursor","move")
			.fadeIn("fast");

		di_jq('#nudging_label').draggable({
		// Find original position of dragged image.
			start: function(event, ui) {
				var currentPos = ui.helper.position();
				startX = parseInt(currentPos.left);
				startY = parseInt(currentPos.top);

				var areaName = di_jq('#'+areaId).attr('title');
				di_jq('#nudging_label').html(areaName);
			},
			// Find position where image is dropped.
			stop: function(event, ui) { 
				var currentPos = ui.helper.position();
				endX = parseInt(currentPos.left);
				endY = parseInt(currentPos.top);
				
				var inputStr = Math.round(endX - Number(imgposset.left)) + delemeter + Math.round( endY - Number(imgposset.top) ) + delemeter + width + delemeter + height; 
				eval(setNudging+'("'+areaId+'", "'+ inputStr +'")');
			}
		});
	}
}

/* prototype function to get KMZ
input: string
output: return
*/
di_Map.prototype.getKMZ = function() {
	responseTxt = this.callBack( {'callback': 249} );
	return responseTxt;
}

/* prototype function to get Base layer info
input: none
output: return responce json string
*/
di_Map.prototype.getBaseLayerInfo = function() {
	var responseTxt = this.callBack( {'callback':252} );
	return responseTxt;
}

/* prototype function to get Feature layer info
input: none
output: return responce json string
*/
di_Map.prototype.getFeatureLayerInfo = function() {
	var responseTxt = this.callBack( {'callback':251} );
	return responseTxt;
}

/* prototype function to set Feature layer info
input: disputed, input string
output: draw map image
*/
di_Map.prototype.setFeatureLayerInfo = function(disputed, inputStr) {
	this.responseTxt = this.callBack( {'callback':253, 'param1':disputed, 'param2':inputStr} );
	this.drawMap(this.responseTxt);
}

/* prototype function to set Base layer info
input: input string
output: draw map image
*/
di_Map.prototype.setBaseLayerInfo = function(inputStr, ids) {
	this.responseTxt = this.callBack( {'callback':254, 'param1':inputStr, 'param2':ids} );
	this.drawMap(this.responseTxt);
}

/* prototype function to set uploaded theme legend setting
input: input string
output: draw map image
*/
di_Map.prototype.setUploadedLegendSettings = function(themeId, filename) {
	this.responseTxt = this.callBack( {'callback':268, 'param1':themeId, 'param2':filename} );
	this.drawMap(this.responseTxt);
}

/* prototype function to delete any file
input: string file URL
output: null
*/
di_Map.prototype.deleteFile = function(url) {
	this.callBack( {'callback': 273, 'param1':url} );
}

/* prototype function to set disputed boundries
input: null
output: true/false
*/
di_Map.prototype.getDisputedBoundry = function() {
	this.callBack( {'callback': 256} );
}

/* prototype function to set area information array
input: null
output: null (only reset the info variable)
*/
di_Map.prototype.refreshAreaInfoArray = function() {
	this.areaInfoArray = [];
}

/* prototype function to get time periods for chart settings
input: null
output: time period comma separted with checked-t, unchecked-f
*/
di_Map.prototype.getChartTimePeriods = function(themeId) {
	var responseTxt = this.callBack( {'callback':285, 'param1':themeId} );
	return responseTxt;
}

/* prototype function to get theme name of the changes series
input: themeId
output: theme name
*/
di_Map.prototype.getSeriesThemeName = function(themeId) {
	var responseTxt = this.callBack( {'callback':286, 'param1':themeId} );
	return responseTxt;
}

/* prototype function to get time period of selected series
input: theme id, selected series id + subgroup name
output: return responce time periods comma separated
*/
di_Map.prototype.getTimePeriodOfSeries = function(themeId, seriesId) {
	var responseTxt = this.callBack( {'callback':290, 'param1':themeId, 'param2':seriesId} );
	return responseTxt;
}

/* prototype function to get opacity for chart legand
input: param1:theme id, param2: ChartLegend
output: return opacity alpha val
*/
di_Map.prototype.getChartLegendAphaVal = function(themeId, opacityFor) {
	var responseTxt = this.callBack( {'callback':291, 'param1':themeId, 'param2':opacityFor} );
	return responseTxt;
}

/* prototype function to set filter on Map
input: param1:indicator nids, param2: area nids (comma separated)
output: return image of the map
*/
di_Map.prototype.setMapFilterRequest = function(inids, anids, render) {
	this.responseTxt = this.callBack( {'callback':292, 'param1':inids, 'param2':anids} );
	if(render=='yes') {
		this.drawMap(this.responseTxt);
	}
}

/* prototype function to get theme count
input: none
output: return theme count
*/
di_Map.prototype.getMapThemeCount = function() {
	var responseTxt = this.callBack( {'callback':293} );
	return responseTxt;
}






 
/*function Drawmap(uid)
{
	var mapObject=new Map(500,600,200);
	var isUidExist=false;
	if(chartCollection.length>0)
	{
		for(var i=0;i<chartCollection.length;i++)
		{
			if(chartCollection[i].id==uid)
			{
				isUidExist = true;
				chartCollection[i].chartInstance = mapObject;				
				break;
			}
		}		
	}
	if(!isUidExist)
	{
		var item = new Object();
		item.id = uid;
		item.chartInstance = mapObject;
		chartCollection.push(item);		
	}
	mapObject.initializeMap(mapObject.initializeCallback);
}
*/
