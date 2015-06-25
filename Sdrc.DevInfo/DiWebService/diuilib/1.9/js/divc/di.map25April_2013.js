
function di_Map()
{var btype='';var ua=window.navigator.userAgent;var msie=ua.indexOf("MSIE ")
if(msie>0)
btype='ie'+parseInt(ua.substring(msie+5,ua.indexOf(".",msie)));else
btype='';this.bType=btype;this.delemeter="[****]";this.mapWidth=600;this.mapHeight=400;this.seriesData='';this.areaIds='';this.divId='';this.dbnid='';this.dblngid='';this.callback='Callback.aspx?bType='+btype;this.callbackNo=200;this.imageUrl='';this.mode='';this.startQuard='';this.endQuard='';this.clientObj='';this.isDataLabel=false;this.areaInfoArray=[];this.mapObjectName="mapObj";this.tootTipDivId="dv_container_map_areainfo";this.nudging=true;this.maptype='multiple';}
di_Map.prototype.initMap=function(width,height,dbnid,dblngid){this.mapWidth=width;this.mapHeight=height;this.dbnid=dbnid;this.dblngid=dblngid;this.callBack({'callback':250,'param1':this.mapWidth,'param2':this.mapHeight,'param3':this.dbnid,'param4':this.dblngid});}
di_Map.prototype.drawMapImage=function(width,height,seriesData,areaIds,divId,dbnid,dblngid,maptype){this.mapWidth=width;this.mapHeight=height;this.seriesData=seriesData;this.areaIds=areaIds;this.divId=divId;this.dbnid=dbnid;this.dblngid=dblngid;this.maptype=maptype;this.responseTxt=this.callBack({'callback':this.callbackNo,'param1':this.mapWidth,'param2':this.mapHeight,'param3':this.dbnid,'param4':this.dblngid,'param5':this.maptype});this.drawMap(this.responseTxt);}
di_Map.prototype.drawMap=function(imgUrl){try
{var imgStyle='';var img=document.createElement('img');img.id="diMapImg";if(this.mode=='zin'){imgStyle='-moz-zoom-in';}
else if(this.mode=='zout'){imgStyle='-moz-zoom-out';}
else if(this.mode=='pan'){imgStyle='-moz-grab';}
else if(this.mode=='zext'){imgStyle='crosshair';}
img.style.cursor=imgStyle;img.src=imgUrl; di_jq('#'+this.divId).html(img); if(this.mode=='pan'&&this.clientObj!=''){this.setPanZoom(this.clientObj);}
if(this.mode=='zext'&&this.clientObj!=''){this.setRubberZoom(this.clientObj);}
if(this.isDataLabel){var tooltipId=this.tootTipDivId;var getlayerInfo=this.GetLayerTaggingInformation();if(getlayerInfo!='false'){di_jq('#diMapImg').attr("usemap","#map");di_jq('#'+this.divId).append(this.createMapInfoHTML(getlayerInfo));di_jq('area').each(function(e){di_jq(this).easyTooltip({useElement:tooltipId,yOffset:70,xOffset:35});});}
else{this.nudging=false;}}
if(this.bType=='ie8')this.deleteFile(imgUrl);}
catch(err){}}
di_Map.prototype.callBack=function(postdata){var response='';di_jq.ajax({type:"POST",url:this.callback,data:postdata,async:false,success:function(res){try
{response=res;}
catch(ex){alert("Error : "+ex.message);}},error:function(){alert("Error occured");},cache:false});return response;}
di_Map.prototype.mapNav=function(){if(this.mode=='zin'){this.zoomIn();}
else if(this.mode=='zout'){this.zoomOut();}
else if(this.mode=='full'){this.setFullExtent();}
else if(this.mode=='zrabur'){}
else if(this.mode=='pan'){}}
di_Map.prototype.zoomIn=function(x,y){this.responseTxt=this.callBack({'callback':201,'param1':x,'param2':y});this.drawMap(this.responseTxt);}
di_Map.prototype.zoomOut=function(x,y){this.responseTxt=this.callBack({'callback':202,'param1':x,'param2':y});this.drawMap(this.responseTxt);}
di_Map.prototype.setFullExtent=function(){this.responseTxt=this.callBack({'callback':205});this.drawMap(this.responseTxt);}
di_Map.prototype.setDataLabels=function(val){this.isDataLabel=val;this.responseTxt=this.callBack({'callback':206,'param1':val});this.drawMap(this.responseTxt);}
di_Map.prototype.setBorder=function(bdrWait){this.responseTxt=this.callBack({'callback':207,'param1':bdrWait});this.drawMap(this.responseTxt);}
di_Map.prototype.setNorthSymbol=function(val){this.responseTxt=this.callBack({'callback':208,'param1':val});this.drawMap(this.responseTxt);}
di_Map.prototype.setScale=function(val){this.responseTxt=this.callBack({'callback':209,'param1':val});this.drawMap(this.responseTxt);}
di_Map.prototype.setPan=function(x1,y1,x2,y2){this.responseTxt=this.callBack({'callback':204,'param1':x1,'param2':y1,'param3':x2,'param4':y2});this.drawMap(this.responseTxt);}
di_Map.prototype.setRubber=function(x1,y1,x2,y2){this.responseTxt=this.callBack({'callback':203,'param1':x1,'param2':y1,'param3':x2,'param4':y2});this.drawMap(this.responseTxt);}
di_Map.prototype.setPanZoom=function(obj){var startX='';var startY='';var endX='';var endY='';this.clientObj=obj;di_jq("#diMapImg").draggable({start:function(event,ui){var currentPos=ui.helper.position();startX=parseInt(currentPos.left);startY=parseInt(currentPos.top);},stop:function(event,ui){var currentPos=ui.helper.position();endX=parseInt(currentPos.left);endY=parseInt(currentPos.top);obj.setPan(startX,startY,endX,endY);}});}
di_Map.prototype.setRubberZoom=function(obj){var startX='';var startY='';var endX='';var endY='';this.clientObj=obj;di_jq("#diMapImg").selectable({start:function(event,ui){var offset=di_jq(this).offset();startX=Math.round(event.pageX-offset.left);startY=Math.round(event.pageY-offset.top);},stop:function(event,ui){var offset=di_jq(this).offset();endX=Math.round(event.pageX-offset.left);endY=Math.round(event.pageY-offset.top);if(mapObj.mode!='zext'){if(mapObj.mode=='zin')mapObj.zoomIn(endX,endY);if(mapObj.mode=='zout')mapObj.zoomOut(endX,endY);}
else{obj.setRubber(startX,startY,endX,endY);}}});}
di_Map.prototype.getThemeInfo=function(themeID){var themeInfo=this.callBack({'callback':211,'param1':themeID});return themeInfo;}
di_Map.prototype.getSeriesInfo=function(){var seriesInfo=this.callBack({'callback':213});return seriesInfo;}
di_Map.prototype.setLegendColor=function(themeId,index,color){this.responseTxt=this.callBack({'callback':212,'param1':themeId,'param2':index,'param3':color});this.drawMap(this.responseTxt);}
di_Map.prototype.setSeriesNBreaks=function(themeId,seriesID,breakType,breakCount){this.responseTxt=this.callBack({'callback':214,'param1':themeId,'param2':seriesID,'param3':breakType,'param4':breakCount});this.drawMap(this.responseTxt);}
di_Map.prototype.setMapSize=function(width,height){this.responseTxt=this.callBack({'callback':210,'param1':width,'param2':height});this.drawMap(this.responseTxt);}
di_Map.prototype.getMapTitleInfo=function(){var titleInfo=this.callBack({'callback':215});return titleInfo;}
di_Map.prototype.getMapDisclaimerInfo=function(){var disclaimerInfo=this.callBack({'callback':216});return disclaimerInfo;}
di_Map.prototype.getMapDataLblInfo=function(){var dataLblInfo=this.callBack({'callback':219});return dataLblInfo;}
di_Map.prototype.setMapTitleInfo=function(setStr){this.callBack({'callback':217,'param1':setStr});}
di_Map.prototype.setMapDisclaimerInfo=function(setStr){this.callBack({'callback':218,'param1':setStr});}
di_Map.prototype.setMapDataLblInfo=function(setStr,redraw){if(redraw){this.responseTxt=this.callBack({'callback':220,'param1':setStr,'param2':true});this.drawMap(this.responseTxt);}
else{this.callBack({'callback':220,'param1':setStr,'param2':false});}}
di_Map.prototype.downloadMap=function(type,keywords){this.callBack({'callback':221,'param1':type,'param2':keywords});}
di_Map.prototype.refreshMap=function(){this.drawMap(this.responseTxt);}
di_Map.prototype.setSeriesColor=function(themeId,startColor,endColor){this.responseTxt=this.callBack({'callback':222,'param1':themeId,'param2':startColor,'param3':endColor});this.drawMap(this.responseTxt);}
di_Map.prototype.getBorderStyle=function(){var borderStyle=this.callBack({'callback':224});return borderStyle;}
di_Map.prototype.setBorderStyle=function(showBdr,width,color,style){this.responseTxt=this.callBack({'callback':223,'param1':showBdr,'param2':width,'param3':color,'param4':style});this.drawMap(this.responseTxt);}
di_Map.prototype.getLabelStyle=function(){var labelStyle=this.callBack({'callback':226});return labelStyle;}
di_Map.prototype.setLabelStyle=function(labelString,styleString){var labelStringAr=labelString.split('[****]');if(labelStringAr[0]=="true")this.isDataLabel=true;else this.isDataLabel=false;this.responseTxt=this.callBack({'callback':225,'param1':labelString,'param2':styleString});this.drawMap(this.responseTxt);}
di_Map.prototype.getMapImagePath=function(){var imagePath=this.callBack({'callback':228});return imagePath;}
di_Map.prototype.addMapTheme=function(inputString){this.responseTxt=this.callBack({'callback':229,'param1':inputString});if(this.responseTxt=='exist'){alert('Theme already exists.');return false;}
else{this.drawMap(this.responseTxt);}}
di_Map.prototype.ediMapTheme=function(themeId,inputString,ismrd,selectedTP){this.responseTxt=this.callBack({'callback':232,'param1':themeId,'param2':inputString,'param3':ismrd,'param4':selectedTP});if(this.responseTxt=='exist'){alert('Theme already exists.');return false;}
else{this.drawMap(this.responseTxt);}}
di_Map.prototype.UpdateThemeLegends=function(themeId,inputString,timeP,persistLegends){this.responseTxt=this.callBack({'callback':1034,'param1':themeId,'param2':inputString,'param3':timeP,'param4':persistLegends});if(this.responseTxt=='exist'){alert('Theme already exists.');return false;}else{this.drawMap(this.responseTxt);}}
di_Map.prototype.deleteMapTheme=function(themeId){this.responseTxt=this.callBack({'callback':231,'param1':themeId});this.drawMap(this.responseTxt);}
di_Map.prototype.visibleMapTheme=function(themeId,visible){this.responseTxt=this.callBack({'callback':230,'param1':themeId,'param2':visible});this.drawMap(this.responseTxt);}
di_Map.prototype.setDotDensityDetails=function(themeId,inputStr){this.responseTxt=this.callBack({'callback':234,'param1':themeId,'param2':inputStr});this.drawMap(this.responseTxt);}
di_Map.prototype.setHatchPatern=function(themeId,legendIndex,paternType){this.responseTxt=this.callBack({'callback':237,'param1':themeId,'param2':legendIndex,'param3':paternType});this.drawMap(this.responseTxt);}
di_Map.prototype.GetLegendBySettingChange=function(themeId,inputStr){this.responseTxt=this.callBack({'callback':236,'param1':themeId,'param2':inputStr});this.drawMap(this.responseTxt);}
di_Map.prototype.GetLegendRanges=function(themeId,legendIndex,rangeTo){var responseTxt=this.callBack({'callback':235,'param1':themeId,'param2':legendIndex,'param3':rangeTo});return responseTxt;}
di_Map.prototype.setThemeLegend=function(themeId,inputStr){this.responseTxt=this.callBack({'callback':238,'param1':themeId,'param2':inputStr});this.drawMap(this.responseTxt);}
di_Map.prototype.getSmoothColors=function(themeId,firstColor,endColor){var responseTxt=this.callBack({'callback':239,'param1':themeId,'param2':firstColor,'param3':endColor});return responseTxt;}
di_Map.prototype.setChartSettings=function(themeId,type,inputStr){this.responseTxt=this.callBack({'callback':240,'param1':themeId,'param2':type,'param3':inputStr});this.drawMap(this.responseTxt);}
di_Map.prototype.setChartLegendSettings=function(themeId,settingStr,legendStr,tmpList){this.responseTxt=this.callBack({'callback':241,'param1':themeId,'param2':settingStr,'param3':legendStr,'param3':tmpList});this.drawMap(this.responseTxt);}
di_Map.prototype.setImageLegendTitle=function(type,inputStr){responseTxt=this.callBack({'callback':246,'param1':type,'param2':inputStr});return responseTxt;}
di_Map.prototype.getImageLegendTitle=function(type){responseTxt=this.callBack({'callback':247,'param1':type});return responseTxt;}
di_Map.prototype.GetLayerTaggingInformation=function(){responseTxt=this.callBack({'callback':243});return responseTxt;}
di_Map.prototype.GetAreaInformation=function(areaId){if(this.areaInfoArray[areaId]!=''&&this.areaInfoArray[areaId]!=undefined){return this.areaInfoArray[areaId];}
else{responseTxt=this.callBack({'callback':244,'param1':areaId});this.areaInfoArray[areaId]=responseTxt;return responseTxt;}}
di_Map.prototype.showMapAreaInfo=function(areaId){var areaInfo=this.GetAreaInformation(areaId);if(areaInfo!='false'){di_jq("#"+this.tootTipDivId).html(this.getAreaInfoHTML(areaInfo));}
else{var areaid=di_jq('#'+areaId).attr('id');var areaname=di_jq('#'+areaId).attr('title');var string=areaname+' ('+areaid+')'+this.delemeter+''+this.delemeter+''+this.delemeter;di_jq("#"+this.tootTipDivId).html(this.getAreaInfoHTML(string));}}
di_Map.prototype.getAreaInfoHTML=function(areaInfo){var areaInfoAr=areaInfo.split(mapDataDelemeter);var areaTitle,areaTime,areaSeries,areaSource;(areaInfoAr[0]!='')?areaTitle=areaInfoAr[0]:areaTitle='NA';(areaInfoAr[1]!='')?areaTime=areaInfoAr[1]:areaTime='NA';(areaInfoAr[2]!='')?areaSeries=areaInfoAr[2]:areaSeries='NA';(areaInfoAr[3]!='')?areaSource=areaInfoAr[3]:areaSource='NA';var html='Error';if(areaInfoAr[0]!='false'){html='<div class="area_callout_title">'+areaTitle+'</div>';html+='<div class="area_callout_text">'+areaTime+'<br />'+areaSeries+' <br/>'+areaSource+'</div>';html+='<b class="area_notch"></b>';}
return html;}
di_Map.prototype.setLabelNudging=function(areaId,inputStr){this.responseTxt=this.callBack({'callback':248,'param1':areaId,'param2':inputStr});this.drawMap(this.responseTxt);}
di_Map.prototype.createMapInfoHTML=function(jsonStr){var mapInfo='<map name="map">';if(jsonStr!=''){var jObj=di_jq.parseJSON(jsonStr);var areaObj=jObj.area;if(areaObj.length>0){for(var i=0;i<areaObj.length;i++){var areaString=areaObj[i].split(this.delemeter);var areaId=areaString[0];var areaName=areaString[1];var areaCord=areaString[2];if(areaId!=''){mapInfo+='<area shape="rect" coords="'+areaCord+'" id="'+areaId+'" title="'+areaName+'" onmouseover=\''+this.mapObjectName+'.showMapAreaInfo("'+areaId+'")\' onclick=\''+this.mapObjectName+'.mapLabelNudging("'+areaId+'")\' style="cursor:pointer" />';}}}}
mapInfo+='</map>';return mapInfo;}
di_Map.prototype.mapLabelNudging=function(areaId){di_jq("#nudging_label").remove();var delemeter=this.delemeter;if(this.mode=='nudge'&&this.nudging==true){var setNudging=this.mapObjectName+'.setLabelNudging';var areaName=di_jq('#'+areaId).attr('title');var imgposset=di_jq('#diMapImg').position();var coords=di_jq('#'+areaId).attr('coords').toString();var coordsAr=coords.split(',');var left1=coordsAr[0];var left2=coordsAr[2];var top1=coordsAr[1];var top2=coordsAr[3];var width=Math.round(left2-left1);var height=Math.round(top2-top1);di_jq("#"+this.divId).append("<div id='nudging_label'>"+areaName+"</div>");di_jq("#nudging_label").css("position","absolute").css("left",Math.round(Number(left1)+Number(imgposset.left))+"px").css("top",Math.round(Number(top1)+Number(imgposset.top))+"px").css("display","none").css("border","1px solid #000000").css("width",width+"px").css("background-color","white").css("opacity","0.3").css("height",height+"px").css("cursor","move").fadeIn("fast");di_jq('#nudging_label').draggable({start:function(event,ui){var currentPos=ui.helper.position();startX=parseInt(currentPos.left);startY=parseInt(currentPos.top);var areaName=di_jq('#'+areaId).attr('title');di_jq('#nudging_label').html(areaName);},stop:function(event,ui){var currentPos=ui.helper.position();endX=parseInt(currentPos.left);endY=parseInt(currentPos.top);var inputStr=Math.round(endX-Number(imgposset.left))+delemeter+Math.round(endY-Number(imgposset.top))+delemeter+width+delemeter+height;eval(setNudging+'("'+areaId+'", "'+inputStr+'")');}});}}
di_Map.prototype.getKMZ=function(){responseTxt=this.callBack({'callback':249});return responseTxt;}
di_Map.prototype.getBaseLayerInfo=function(){var responseTxt=this.callBack({'callback':252});return responseTxt;}
di_Map.prototype.getFeatureLayerInfo=function(){var responseTxt=this.callBack({'callback':251});return responseTxt;}
di_Map.prototype.setFeatureLayerInfo=function(disputed,inputStr){this.responseTxt=this.callBack({'callback':253,'param1':disputed,'param2':inputStr});this.drawMap(this.responseTxt);}
di_Map.prototype.setBaseLayerInfo=function(inputStr,ids){this.responseTxt=this.callBack({'callback':254,'param1':inputStr,'param2':ids});this.drawMap(this.responseTxt);}
di_Map.prototype.setUploadedLegendSettings=function(themeId,filename){this.responseTxt=this.callBack({'callback':268,'param1':themeId,'param2':filename});this.drawMap(this.responseTxt);}
di_Map.prototype.deleteFile=function(url){this.callBack({'callback':273,'param1':url});}
di_Map.prototype.getDisputedBoundry=function(){this.callBack({'callback':256});}
di_Map.prototype.refreshAreaInfoArray=function(){this.areaInfoArray=[];}
di_Map.prototype.getChartTimePeriods=function(themeId){var responseTxt=this.callBack({'callback':285,'param1':themeId});return responseTxt;}
di_Map.prototype.getSeriesThemeName=function(themeId){var responseTxt=this.callBack({'callback':286,'param1':themeId});return responseTxt;}