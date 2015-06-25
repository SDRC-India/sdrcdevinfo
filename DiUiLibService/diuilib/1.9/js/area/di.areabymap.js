
function di_AreaByMap(callbackUrl)
{if(callbackUrl=='')return false;this.delemeter="[****]";this.width=600;this.height=300;this.divId='';this.callback=callbackUrl;this.mode='';this.responseTxt='';this.clientObj='';this.uid='';}
di_AreaByMap.prototype.callBack=function(postdata){var response='';di_jq.ajax({type:"POST",url:this.callback,data:postdata,async:false,success:function(res){try
{response=res;}
catch(ex){alert("Error : "+ex.message);}},error:function(){alert("Error occured");},cache:false});return response;}
di_AreaByMap.prototype.drawAreaMap=function(uid,width,height,divId,clientObj){this.uid=uid;this.width=width;this.height=height;this.divId=divId;this.clientObj=clientObj;this.responseTxt=this.callBack({'callback':1,'param1':this.width,'param2':this.height});this.drawMap(this.responseTxt);}
di_AreaByMap.prototype.drawMap=function(imgUrl){try
{var imgStyle='';var img=document.createElement('img');img.id="diAreaMapImage_"+this.uid;if(this.mode=='zin'){imgStyle='-moz-zoom-in';}
else if(this.mode=='zout'){imgStyle='-moz-zoom-out';}
else if(this.mode=='pan'){imgStyle='-moz-grab';}
else if(this.mode=='zext'){imgStyle='crosshair';}
img.style.cursor=imgStyle;img.src=imgUrl;di_jq('#'+this.divId).html(img);if(this.mode=='pan'&&this.clientObj!=''){this.setPanZoom();}
if(this.mode=='zext'&&this.clientObj!=''){this.setRubberZoom();}}
catch(err){}}
di_AreaByMap.prototype.zoomIn=function(x,y){this.responseTxt=this.callBack({'callback':2,'param1':x,'param2':y});this.drawMap(this.responseTxt);}
di_AreaByMap.prototype.zoomOut=function(x,y){this.responseTxt=this.callBack({'callback':3,'param1':x,'param2':y});this.drawMap(this.responseTxt);}
di_AreaByMap.prototype.setFullExtent=function(){this.responseTxt=this.callBack({'callback':6});this.drawMap(this.responseTxt);}
di_AreaByMap.prototype.setAreaLabels=function(val){this.responseTxt=this.callBack({'callback':9,'param1':val});this.drawMap(this.responseTxt);}
di_AreaByMap.prototype.setPan=function(x1,y1,x2,y2){this.responseTxt=this.callBack({'callback':5,'param1':x1,'param2':y1,'param3':x2,'param4':y2});this.drawMap(this.responseTxt);}
di_AreaByMap.prototype.setRubber=function(x1,y1,x2,y2){this.responseTxt=this.callBack({'callback':4,'param1':x1,'param2':y1,'param3':x2,'param4':y2});this.drawMap(this.responseTxt);}
di_AreaByMap.prototype.setPanZoom=function(){var startX='';var startY='';var endX='';var endY='';var obj=this.clientObj;var uid=this.uid;var ardata=di_areacominputarr[uid];di_jq("#diAreaMapImage_"+uid).draggable({start:function(event,ui){var currentPos=ui.helper.position();startX=parseInt(currentPos.left);startY=parseInt(currentPos.top);},stop:function(event,ui){var currentPos=ui.helper.position();endX=parseInt(currentPos.left);endY=parseInt(currentPos.top);ardata.areaObj.di_areaLoadingSH(uid,true);obj.setPan(startX,startY,endX,endY);ardata.areaObj.di_areaLoadingSH(uid,false);}});}
di_AreaByMap.prototype.setRubberZoom=function(){var startX='';var startY='';var endX='';var endY='';var obj=this.clientObj;var uid=this.uid;var ardata=di_areacominputarr[uid];di_jq("#diAreaMapImage_"+uid).selectable({start:function(event,ui){var offset=di_jq(this).offset();startX=Math.round(event.pageX-offset.left);startY=Math.round(event.pageY-offset.top);},stop:function(event,ui){var offset=di_jq(this).offset();endX=Math.round(event.pageX-offset.left);endY=Math.round(event.pageY-offset.top);if(obj.mode!='zext'){if(obj.mode=='zin')obj.zoomIn(endX,endY);if(obj.mode=='zout')obj.zoomOut(endX,endY);}
else{ardata.areaObj.di_areaLoadingSH(uid,true);obj.setRubber(startX,startY,endX,endY);ardata.areaObj.di_areaLoadingSH(uid,false);}}});}
di_AreaByMap.prototype.resetMapSize=function(width,height){this.width=width;this.height=height;this.responseTxt=this.callBack({'callback':10,'param1':width,'param2':height});this.drawMap(this.responseTxt);}
di_AreaByMap.prototype.refreshMap=function(){this.drawMap(this.responseTxt);}
di_AreaByMap.prototype.clickMapImage=function(x,y,action){this.responseTxt=this.callBack({'callback':7,'param1':action,'param2':x+this.delemeter+y});this.drawMap(this.responseTxt);}
di_AreaByMap.prototype.clickedAreaInfo=function(x,y){var responseTxt=this.callBack({'callback':8,'param1':x+this.delemeter+y});return responseTxt;}