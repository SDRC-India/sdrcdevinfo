
function di_drawAreaSelectionList(div_id,areaoptions){if(di_safevalue(div_id)=='')return false;if(di_safevalue(areaoptions.codelistPath)=='')return false;if(di_safevalue(areaoptions.currentMode)=='')areaoptions.currentMode='list';if(di_safevalue(areaoptions.storePreMode)=='')areaoptions.storePreMode='list';if(di_safevalue(areaoptions.currentLevel)=='')areaoptions.currentLevel=1;if(di_safevalue(areaoptions.currentPage)=='')areaoptions.currentPage=1;if(di_safevalue(areaoptions.selectedAreas)=='')areaoptions.selectedAreas=[];areaoptions.areaMapObj='';var uid=di_generateUID();this.uid=uid;areaoptions.uid=uid;if(di_safevalue(areaoptions.labels.selectAll)=='')areaoptions.labels.selectAll='Select All';if(di_safevalue(areaoptions.labels.byList)=='')areaoptions.labels.byList='List A to Z';if(di_safevalue(areaoptions.labels.byTree)=='')areaoptions.labels.byTree='By Tree';if(di_safevalue(areaoptions.labels.byMap)=='')areaoptions.labels.byMap='By Map';if(di_safevalue(areaoptions.labels.search)=='')areaoptions.labels.search='Search';if(di_safevalue(areaoptions.labels.viewSel)=='')areaoptions.labels.viewSel='View Selection';if(di_safevalue(areaoptions.labels.metadata)=='')areaoptions.labels.metadata='Metadata';if(di_safevalue(areaoptions.labels.dataExist)=='')areaoptions.labels.dataExist='Show where data exist';if(di_safevalue(areaoptions.labels.clear)=='')areaoptions.labels.clear='Clear';if(di_safevalue(areaoptions.labels.close)=='')areaoptions.labels.close='Close';if(di_safevalue(areaoptions.labels.first)=='')areaoptions.labels.first='First';if(di_safevalue(areaoptions.labels.last)=='')areaoptions.labels.last='Last';if(di_safevalue(areaoptions.labels.next)=='')areaoptions.labels.next='Next';if(di_safevalue(areaoptions.labels.back)=='')areaoptions.labels.back='Back';if(di_safevalue(areaoptions.labels.page)=='')areaoptions.labels.page='Page';if(areaoptions.width.indexOf('%')==-1&&areaoptions.width<600)areaoptions.width='600';if(areaoptions.height<300)areaoptions.height='300';areaoptions.isDataExist='F';if(areaoptions.mode.toLowerCase()=='both'){var defMode=areaoptions.defaultMode.toLowerCase();if(di_safevalue(defMode)!=''&&defMode!='tree'&&defMode!='map'){defModeArr=defMode.split("_");areaoptions.currentMode=defModeArr[0];if(di_safevalue(defModeArr[1])!='')
areaoptions.currentLevel=defModeArr[1];}
else if(di_safevalue(defMode)!=''){areaoptions.currentMode=defMode;}}
else if(areaoptions.mode.toLowerCase()=='tree'){areaoptions.currentMode='tree';}
else{var listopt=areaoptions.mode.toLowerCase();listoptAr=listopt.split("_");areaoptions.currentMode='list';if(di_safevalue(listoptAr[1])!='')areaoptions.currentLevel=listoptAr[1];}
areaoptions.storePreMode=areaoptions.currentMode;di_areacominputarr[uid]=areaoptions;div_obj=document.getElementById(div_id);var html_data=this.createAreaSelUI(uid);div_obj.innerHTML=html_data;di_jq(document).ready(function(){var outerwidth=di_jq('#di_areaouterdiv_'+uid).width();var outerheight=di_jq('#di_areaouterdiv_'+uid).height();var headerheight=di_jq('#di_area_td_body_'+uid).height();var titleheight=di_jq('#di_areaouterdiv_'+uid).children('.di_gui_title').height();var footerheight=di_jq('#di_areawidget_'+uid).children('.di_gui_buttonbox').height();var viewselheight=di_jq('#di_area_viewsel_panel_'+uid).height();var subtractHeight=headerheight;if(di_safevalue(titleheight)!='')subtractHeight=subtractHeight+titleheight;if(di_safevalue(footerheight)!='')subtractHeight=subtractHeight+footerheight;if(di_safevalue(viewselheight)!='')subtractHeight=subtractHeight+viewselheight;di_jq('#di_area_td_body_'+uid).css("width",outerwidth+'px');di_jq('#diarea-tabs-1_'+uid).css('height',(di_areacominputarr[uid].height-subtractHeight)+'px');di_jq('#diarea-map-1_'+uid).css('height',(di_areacominputarr[uid].height-subtractHeight)+'px');di_jq('#di_areaviewsel_con_'+uid).css('width',(outerwidth-5)+'px');di_jq('#di_areaviewsel_con_'+uid).css('height',(di_areacominputarr[uid].height-18)+'px');di_jq('#di_areamtdt_view_'+uid).css('width',(outerwidth-15)+'px');var areaInitObj=new di_areaInitObject(uid);di_areacominputarr[uid].areaObj=areaInitObj;areaInitObj.storeJsonSelection(uid,di_areacominputarr[uid].jsonSelection);areaInitObj.di_areaInitiateMode(uid,'render');di_jq('#di_area_tree1_'+uid).checkboxTree({trueCheck:{val:'no'},onCheck:{ancestors:'check',descendants:'check',others:''},onUncheck:{descendants:'uncheck'}});if(di_areacominputarr[uid].currentMode!=''){var comp_mode=di_areacominputarr[uid].currentMode;di_areacominputarr[uid].areaObj.di_areaSetLinksVisiblity(uid,di_areacominputarr[uid].currentMode);di_jq('#di_area_links_'+uid+' span').click(function(){var modeclass=di_jq(this).attr('class');var relmode=di_jq(this).attr('rel');di_areacominputarr[uid].storePreMode=relmode;di_areacominputarr[uid].areaObj.di_areaChangeViewMode(uid,relmode);di_areacominputarr[uid].areaObj.di_areaSetLinksVisiblity(uid,relmode);searchObj.refresh();di_areacominputarr[uid].areaObj.di_areaInitiateMode(uid,'render');});di_jq('#di_arealevelbox_'+uid).bind('change',function(){var lvlval=this.value;var lvlvalar=lvlval.split("__");var level=lvlvalar[0];var total_pages=lvlvalar[1];di_areacominputarr[uid].currentLevel=level;di_areacominputarr[uid].currentPage=1;di_areacominputarr[uid].areaObj.di_areaInitiateMode(uid,'render');});di_jq('#di_area_data_exist_'+uid).bind('click',function(){if(this.checked==true){di_areacominputarr[uid].isDataExist='T';}
else{di_areacominputarr[uid].isDataExist='F';}
di_areacominputarr[uid].areaObj.di_areaInitiateMode(uid,'render');});if(comp_mode=='list'){di_jq('#di_arealevelbox_'+uid).attr('disabled',false);}
else if(comp_mode=='map'){}
else if(comp_mode=='tree'){}}
di_areacominputarr[uid].areaObj.di_areaLoadingSH(uid,false);di_jq('.di_gui_popupclose').click(function(){di_areacominputarr[uid].areaObj.di_hideAreaMetadataPOP(uid);});di_jq('#di_area_count_sel_'+uid).closest('a').click(function(){areaInitObj.di_viewSelections(uid);});di_jq('#di_vsclear_'+uid).click(function(){di_jq('#di_areaviewsel_con_'+uid).html('');di_jq('#di_area_tree1_'+uid+' input:checkbox:checked').attr('checked','');di_jq('#di_areaviewsel_list_'+uid).hide('slide',{direction:'right'});di_areacominputarr[uid].selectedAreas=[];areaInitObj.di_areaCount(uid);});di_jq('#di_vsclose_'+uid).click(function(){di_jq('#di_areaviewsel_list_'+uid).hide('slide',{direction:'right'});});di_jq('#di_areaviewsel_con_'+uid+' img').live('click',function(){var arlabel=di_jq(this).attr('rel');di_jq(this).parent().remove();arlabel=arlabel.split('[****]');areaInitObj.di_manageAreaSelection(uid,false,arlabel[1],arlabel[0]);var qslbl=arlabel[1].split('||');di_jq('#di_area_tree1_'+uid+' input[rel="'+qslbl[0]+'"]').attr('checked',false);});di_jq('#di_area_cancel_'+uid).bind('click',function(){areaInitObj.di_areaCancel(uid);});di_jq('#di_area_ok_'+uid).bind('click',function(){areaInitObj.di_areaOK(uid);});di_jq('#area_search_'+uid).live('keyup',function(){di_areacominputarr[uid].areaObj.di_getSearch(uid,this.value);});di_jq('#cros_area_search_'+uid).live('click',function(){di_jq('#area_search_'+uid).val('');di_jq('#area_search_'+uid).removeClass('di_gui_searchbox_defaulttext');di_jq('#area_search_'+uid).addClass('di_gui_searchbox_text');di_jq('#area_search_'+uid).focus();di_areacominputarr[uid].areaObj.di_getSearch(uid,'');});});var searchObj=new di_drawSearchBox('area_search_ctrl_td_'+uid,'area_search_'+uid,'100%','20px',areaoptions.labels.search,'');}
di_drawAreaSelectionList.prototype.createAreaSelUI=function(uid){var html_data='';try
{var aropts=di_areacominputarr[uid];if(aropts.width.indexOf('%')>0){html_data='<div id="di_areaouterdiv_'+uid+'" style="width:'+aropts.width+'">';}
else{html_data='<div id="di_areaouterdiv_'+uid+'" style="width:'+aropts.width+'px">';}
if(di_safevalue(aropts.metadataPath)!=''){html_data+='<div id="di_areamtdt_view_'+uid+'" class="di_gui_popup_block" style="display:none;"><div style="left:-17px;float:right;position: relative;top:-15px;"><b class="di_gui_popupclose" style="float:right;"></b></div><div class="di_gui_title" id="di_areamtdt_title_'+uid+'">'+aropts.labels.metadata+'</div><div id="di_areamtdt_'+uid+'" class="di_gui_metadata_container"></div></div>';}
if(aropts.showViewSel==true){var di_area_selected_list_div_height='';html_data+='<div id="di_areaviewsel_list_'+uid+'" style="position:absolute;z-index: 950; overflow: hidden; display:none;" role="dialog" tabindex="-1" aria-labelledby="alerthd" aria-hidden="false"><div class="di_gui_view_selections_box"><div class="di_gui_title">'+aropts.labels.viewSel+'</div><div id="di_areaviewsel_con_'+uid+'" style="height:'+aropts.height+'px;" class="di_gui_viewselections_body"></div><div style="text-align:right;padding-right:10px"><input id="di_vsclear_'+uid+'" type="button" value="'+aropts.labels.clear+'" class="di_gui_button"><input id="di_vsclose_'+uid+'" type="button" value="'+aropts.labels.close+'" class="di_gui_button"></div></div></div>';}
if(aropts.title!=''){html_data+='<div class="di_gui_title"><div style="float:left;">'+aropts.title+'</div><div style="position:relative;text-align:right;">&nbsp;';if(aropts.iconClose==true){html_data+='<span class="di_gui_closebutton" onclick=\'di_toggle_widget("di_areawidget_'+uid+'")\'><img src="'+di_images_path+'/close_icon.gif"></span>';}
html_data+='</div></div>';}
html_data+='<div id="di_areawidget_'+uid+'"><table width="100%" cellpadding="0" cellspacing="0" border="0" class="di_gui_container"><tr><td id="di_area_td_body_'+uid+'"><div class="di_gui_gradient_panel_box"><table width="98%" border="0" cellpadding="0" cellspacing="0" height="30"><tr><td width="5%" nowrap><div id="di_area_links_'+uid+'">';if(aropts.mode=='both'){html_data+='<span rel="tree" class="di_gui_link3">'+aropts.labels.byTree+'</span> | ';html_data+='<span rel="list" class="di_gui_link3">'+aropts.labels.byList+'</span>&nbsp;'+this.getAreaLevelsBox('selectmenu',aropts.currentLevel,uid)+' ';if(di_safevalue(aropts.callbacks.areaMap)!='')
html_data+='| <span rel="map" class="di_gui_link3">'+aropts.labels.byMap+'</span>';}
else if(aropts.mode=='tree'){html_data+='<span rel="tree" class="di_gui_seld_txt">'+aropts.labels.byTree+'</span>';}
else if(aropts.mode=='list'){html_data+='<span rel="list" class="di_gui_seld_txt">'+aropts.labels.byList+'</span>&nbsp;'+this.getAreaLevelsBox('selectmenu',aropts.currentLevel,uid)+'';}
else if(di_safevalue(aropts.callbacks.areaMap)!=''){html_data+='<span rel="map" class="di_gui_seld_txt">'+aropts.labels.byMap+'</span>';}
if(aropts.callbacks.whereDataExist!=''){html_data+=' | ';}
html_data+='</div></td>';html_data+='<td width="5%" nowrap class="di_gui_modeview_box" style="text-align:left;padding-left:5px;">';if(aropts.callbacks.whereDataExist!=''){html_data+='<table width="100%" cellpadding="0" cellspacing="0"><tr><td><input id="di_area_data_exist_'+uid+'" type="checkbox"></td><td class="di_gui_seld_txt" nowrap>'+aropts.labels.dataExist+'</td></tr></table>';}
html_data+='</td>';html_data+='<td width="90%" id="area_search_ctrl_td_'+uid+'" nowrap></td></tr></table></div></td></tr><tr><td>';html_data+='<div id="di_area_wheeler_'+uid+'" style="margin-left:37%;margin-top:5%;position:absolute;z-index:2000;"><div class="di_gui_wheeler" style="display:block;"><img src="'+di_images_path+'/ajax-loader.gif"></div></div>';html_data+='<div id="diarea-map-1_'+uid+'" class="di_gui_body" style="width:100%;display:none;"></div>';html_data+='<div id="diarea-tabs-1_'+uid+'" class="di_gui_body" style="width:100%;"><ul id="di_area_tree1_'+uid+'">';html_data+='</ul></div>';html_data+='<div id="di_area_viewsel_panel_'+uid+'" class="di_gui_gradient_panel_bottombox"><table width="80%" cellspacing="0" cellpadding="0" border="0" height="24"><tr>';if(aropts.showViewSel==true){html_data+='<td width="150"><a class="di_gui_link3">'+aropts.labels.viewSel+' (<span id="di_area_count_sel_'+uid+'">0</span>)</a></td>';}
html_data+='<td align="center" ><div id="di_list_paging_'+uid+'" class="di_gui_paging_box"></div></td></tr></table></div></td></tr></table>';if(aropts.showFooter==true){html_data+='<div class="di_gui_buttonbox"><input type="button" id="di_area_ok_'+uid+'" class="di_gui_button" value="'+aropts.labels.ok+'"/>&nbsp;<input type="button" id="di_area_cancel_'+uid+'" class="di_gui_button" value="'+aropts.labels.cancel+'"/></div>';}
html_data+='</div></div>';}
catch(err){}
return html_data;}
di_drawAreaSelectionList.prototype.getSelectedAreas=function(type){var retval='';var type=di_safevalue(type);try
{if(type=='json'){retval=this.getAreaJSON();}
else if(type=='val'){retval=this.getAreaValues();}
else{var area_nids=this.getAreaValues();var area_json=this.getAreaJSON();retval=area_nids+'||{~~}||'+area_json;}
return retval;}
catch(err){}}
di_drawAreaSelectionList.prototype.getAreaValues=function(){var ardata=di_areacominputarr[this.uid];var retval='';try
{var initVal=0;if(di_safevalue(ardata.selectedAreas[-1])!=''&&di_safevalue(ardata.selectedAreas[-1]).length>0){initVal=-1;}
var cnt1=0;for(var i=initVal;i<ardata.selectedAreas.length;i++)
{var lvlData=di_safevalue(ardata.selectedAreas[i]);for(var j=0;j<lvlData.length;j++){if(di_safevalue(lvlData[j])!=''){var lvlDataAr=lvlData[j].split('||');if(cnt1>0)retval+=',';retval+=lvlDataAr[0];cnt1++;}}}}
catch(err){}
return retval;}
di_drawAreaSelectionList.prototype.getAreaJSON=function(){var ardata=di_areacominputarr[this.uid];var retval='';try
{var initVal=0;if(di_safevalue(ardata.selectedAreas[-1])!=''&&di_safevalue(ardata.selectedAreas[-1]).length>0){initVal=-1;}
retval='{"area" : {';for(var i=initVal;i<ardata.selectedAreas.length;i++)
{var lvlData=di_safevalue(ardata.selectedAreas[i]);if(lvlData!=''){retval+='"'+i+'" : [';var cnt2=0;for(var j=0;j<lvlData.length;j++){if(di_safevalue(lvlData[j])!=''){var arData=lvlData[j];if(cnt2>0)retval+=',';retval+='"'+arData+'"';cnt2++;}}
retval+=']';if(i<ardata.selectedAreas.length-1)retval+=',';}}
retval+=' }}';}
catch(err){}
return retval;}
di_drawAreaSelectionList.prototype.getAreaLevelsBox=function(rtype,level,uid){var return_str='';try
{var aropts=di_areacominputarr[uid];di_jq.ajax({type:"GET",url:aropts.codelistPath+'/arealevels/levelmaster.xml',dataType:"xml",async:false,success:function(xml){if(rtype=='selectmenu'){return_str='<select name="di_arealevelbox_'+uid+'" id="di_arealevelbox_'+uid+'" class="di_gui_dropdown1" disabled>';di_jq(xml).find('al').each(function(){var level_number=di_jq(this).attr("number");var level_pages=di_jq(this).attr("pages");var level_name=di_jq(this).attr("n");var level_selected=''
if(aropts.currentLevel==level_number){level_selected='selected';}
return_str+='<option value="'+level_number+'__'+level_pages+'" '+level_selected+'>'+level_name+'</option>';});return_str+='</select>';}
else if(rtype=='count'){di_jq(xml).find('al[number="'+level+'"]').each(function(){return_str=di_jq(this).attr("pages");});}}});}
catch(err){}
return return_str;}
function di_areaInitObject(uid){this.delm="[~]";}
di_areaInitObject.prototype.storeJsonSelection=function(uid,jsonString){try
{if(jsonString!=''){var obj=di_jq.parseJSON(jsonString);if(obj!=null){var areaTxt=obj.area;if(di_safevalue(areaTxt)!=''){for(var i in areaTxt)
{var areaText=new String(areaTxt[i]).split(di_ElementDelimiter);for(var k=0;k<areaText.length;k++){var txt=di_safevalue(areaText[k]);var lvl=eval(i);this.di_manageAreaSelection(uid,true,txt,lvl);}}}}}}
catch(err){}}
di_areaInitObject.prototype.di_areaChangeViewMode=function(uid,mode){di_areacominputarr[uid].currentMode=mode;if(mode=='list'){di_jq('#di_arealevelbox_'+uid).attr('disabled',false);}
else{di_jq('#di_arealevelbox_'+uid).attr('disabled',true);this.di_getPagingUI(uid,'');}}
di_areaInitObject.prototype.di_areaSetLinksVisiblity=function(uid,mode){di_jq('#di_area_links_'+uid+' span').removeAttr('class');di_jq('#di_area_links_'+uid+' span').addClass('di_gui_link3');di_jq('#di_area_links_'+uid+' span').each(function(e){var relmode=di_jq(this).attr('rel');if(relmode==mode){di_jq(this).removeClass('di_gui_link3');di_jq(this).addClass('di_gui_seld_txt');}});}
di_areaInitObject.prototype.di_areaLoadingSH=function(uid,action){if(action==true)di_jq('#di_area_wheeler_'+uid).show();else di_jq('#di_area_wheeler_'+uid).hide();}
di_areaInitObject.prototype.di_areaInitiateMode=function(uid,method){var mHtml='';var ardata=di_areacominputarr[uid];var cMode=ardata.currentMode;var cPath=di_safevalue(ardata.codelistPath);this.di_areaLoadingSH(uid,true);if(cPath!=''){if(cMode=='tree'){mHtml=this.di_getAreaTreeData(uid);}
else if(cMode=='list'){mHtml=this.di_getAreaListData(uid);}
else if(cMode=='map'){ardata.areaMapObj=new di_AreaByMap(ardata.callbacks.areaMap);mHtml=this.di_createMAPUI(uid);}}
else{mHtml='<li>Codelist url is null.</li>';}
if(method=='render'){if(cMode=='map'){di_jq('#di_area_tree1_'+uid).html('');di_jq('#diarea-tabs-1_'+uid).hide();di_jq('#diarea-map-1_'+uid).html(mHtml);di_jq('#diarea-map-1_'+uid).show();var width=di_jq('#diarea-tabs-1_'+uid).width();var area_height=ardata.height;ardata.areaMapObj.drawAreaMap(uid,width-45,area_height-100,"DIareaMapImageView_"+uid,ardata.areaMapObj);this.di_MAPBindEvent(uid);di_jq('#di_area_data_exist_'+uid).attr('checked',false);di_jq('#di_area_data_exist_'+uid).attr('disabled',true);}
else{di_jq('#di_area_data_exist_'+uid).attr('disabled',false);di_jq('#diarea-map-1_'+uid).hide();di_jq('#diarea-map-1_'+uid).html('');di_jq('#di_area_tree1_'+uid).html(mHtml);di_jq('#diarea-tabs-1_'+uid).show();this.di_areaRowBindingEvent('di_area_tree1_'+uid,uid);}}
else if(method=='return')return mHtml;this.di_areaLoadingSH(uid,false);}
di_areaInitObject.prototype.di_areaRowBindingEvent=function(id,uid){var delm=this.delm;di_jq('#'+id+' li').bind('mouseenter mouseleave',function(evt){if(di_areacominputarr[uid].metadataPath!=''){var id=di_jq(this).attr('id');if(evt.type=='mouseenter'){di_jq(this).find('label:eq(1)').show();}else if(evt.type=='mouseleave'){di_jq(this).find('label:eq(1)').hide();}}});di_jq('#'+id+' li label').bind('mouseenter mouseleave',function(evt){var type=di_jq(this).attr('rel');if(type=='lbl'){if(evt.type=='mouseenter'){di_jq(this).addClass('di_gui_label_hover');}else if(evt.type=='mouseleave'){di_jq(this).removeClass('di_gui_label_hover');}}});di_jq('#'+id+' li label').bind('click',function(evt){var ischild=di_jq(this).parent().attr('rel');var li_id=di_jq(this).parent().attr('id');var ltype=di_jq(this).attr('rel');var isexist=di_jq(this).parent().find('ul').length;var arvals=di_jq(this).parent().attr('val').split(delm);var aid=arvals[0];var anid=arvals[1];var alvl=arvals[2];var aname=arvals[3];var lnid=arvals[4];if(ltype=='lbl'&&ischild=='T'&&isexist==0){var subui=di_areacominputarr[uid].areaObj.di_getSubnationAreas(uid,aid,alvl,anid);di_jq('#'+li_id).append(subui);di_areacominputarr[uid].areaObj.di_areaRowBindingEvent(li_id,uid);}
if(di_areacominputarr[uid].multiple==false&&ltype=='lbl'){var ardata=di_areacominputarr[uid];var outputval='';if(ardata.outputFormat=='aid')outputval=aid;else if(ardata.outputFormat=='nid')outputval=anid;else if(ardata.outputFormat=='name')outputval=aname;else if(ardata.outputFormat=='nid_name')outputval=anid+'_'+aname;else if(ardata.outputFormat=='aid_name')outputval=aid+'_'+aname;else if(ardata.outputFormat=='aid_nid_name')outputval=aid+'_'+anid+'_'+aname;di_jq('#di_area_tree1_'+uid+' li label').removeClass('di_gui_singlenode_selection');di_jq(this).addClass('di_gui_singlenode_selection');var arlabel=aname;if(di_areacominputarr[uid].showAreaId==true)arlabel=aname+" ("+aid+")";di_areacominputarr[uid].selectedAreas=[];di_areacominputarr[uid].areaObj.di_manageAreaSelection(uid,true,outputval+'||'+arlabel,1);if(ardata.callbacks.hotSel!=''){eval(ardata.callbacks.hotSel);}}
if(ltype=='md'&&lnid!=''){di_areacominputarr[uid].areaObj.di_showAreaMetadataPOP(uid,aname+' ('+aid+')',lnid);}});di_jq('#'+id+' li span').bind('click',function(evt){var ischild=di_jq(this).parent().attr('rel');var li_id=di_jq(this).parent().attr('id');var isexist=di_jq(this).parent().find('ul').length;if(ischild=='T'&&isexist==0){var arvals=di_jq(this).parent().attr('val').split(delm);var aid=arvals[0];var anid=arvals[1];var alvl=arvals[2];var aname=arvals[3];var subui=di_areacominputarr[uid].areaObj.di_getSubnationAreas(uid,aid,alvl,anid);di_jq('#'+li_id).append(subui);di_areacominputarr[uid].areaObj.di_areaRowBindingEvent(li_id,uid);}});di_jq('#'+id+' li input[type="checkbox"]').bind('click',function(evt){var arlabel='';var seltype=di_jq(this).parent().attr('rel');var arvals=di_jq(this).parent().attr('val');var checked=this.checked;if(seltype=='all'){di_jq(this).closest('ul').children('li').each(function(){var aval=di_safevalue(di_jq(this).attr('val'));if(aval!=''){aval=aval.split(delm);var childobj=di_jq(this).children('input[type="checkbox"]');if(childobj.attr('disabled')==false){var arlabel=aval[3];var outval=childobj.val();if(di_areacominputarr[uid].showAreaId==true)arlabel=aval[3]+" ("+aval[0]+")";if(checked==true){childobj.attr('checked',true);di_areacominputarr[uid].areaObj.di_manageAreaSelection(uid,true,outval+'||'+arlabel,aval[2]);}
else{childobj.attr('checked',false);di_areacominputarr[uid].areaObj.di_manageAreaSelection(uid,false,outval+'||'+arlabel,aval[2]);}}
else{childobj.attr('checked','');}}});}
else if(seltype=='QS'){var qs_val=di_jq(this).val();if(di_safevalue(qs_val)!=''){qs_val=qs_val.split(delm);var pid=qs_val[0];var qslabel=qs_val[1];var level=qs_val[2];var qs_enter='QS_'+pid+'_L'+level;if(checked==true){di_areacominputarr[uid].areaObj.di_manageAreaSelection(uid,true,qs_enter+'||'+qslabel,level);}
else{di_areacominputarr[uid].areaObj.di_manageAreaSelection(uid,false,qs_enter+'||'+qslabel,level);}}
else if(di_areacominputarr[uid].quickSelMode=='HRZ'){di_jq(this).closest('ul').children('li').each(function(){var aval=di_safevalue(di_jq(this).attr('val'));if(aval!=''){aval=aval.split(delm);var childobj=di_jq(this).children('input[type="checkbox"]');if(childobj.attr('disabled')==false){var arlabel=aval[3];var outval=childobj.val();if(di_areacominputarr[uid].showAreaId==true)arlabel=aval[3]+" ("+aval[0]+")";if(checked==true){childobj.attr('checked',true);di_areacominputarr[uid].areaObj.di_manageAreaSelection(uid,true,outval+'||'+arlabel,aval[2]);}
else{childobj.attr('checked',false);di_areacominputarr[uid].areaObj.di_manageAreaSelection(uid,false,outval+'||'+arlabel,aval[2]);}}
else{childobj.attr('checked','');}}});}}
else{arvals=arvals.split(delm);if(di_areacominputarr[uid].showAreaId==true)var arlabel=arvals[3]+" ("+arvals[0]+")";else arlabel=arvals[3];if(checked==true){di_areacominputarr[uid].areaObj.di_manageAreaSelection(uid,true,this.value+'||'+arlabel,arvals[2]);}
else{di_areacominputarr[uid].areaObj.di_manageAreaSelection(uid,false,this.value+'||'+arlabel,arvals[2]);}}});}
di_areaInitObject.prototype.di_getAreaTreeData=function(uid){var ar_ui='';var disabledCheckbox='disabled="true"';var checkedCheckbox='checked="true"';this.di_areaLoadingSH(uid,true);var delm=this.delm;var arData=di_areacominputarr[uid];var isDataExistFunc=arData.callbacks.whereDataExist;var flagFunc=this.di_areaGetFlag;var metaFunc=this.di_getAreaMetadataUI;var chkSelection=this.di_checkSelection;di_jq.ajax({type:"GET",url:arData.codelistPath+'/areacodelist.xml',dataType:"xml",async:false,timeout:5000,error:function(request,error){if(error=="timeout")
alert('The request timed out, File size is so high');else{}},success:function(xml){var search_patern='area[lvl="1"]';var return_areanids='';if(arData.isDataExist=='T'){var funcResult=eval(isDataExistFunc)('L_1');if(funcResult!='false'){return_areanids=funcResult.split(",");}}
di_jq(xml).find(search_patern).each(function(){var disabledStatus='0';var ar_name=di_jq(this).attr("name");var ar_nid=di_jq(this).attr("nID");var ar_ID=di_jq(this).attr("aID");var ar_de=di_jq(this).attr("dataexists");var ar_lvl=di_jq(this).attr("lvl");var ar_subnation=di_jq(this).attr("subNational");var layer_nid=di_jq(this).attr("lnid");var outputval=ar_nid;if(arData.isDataExist=='T'){if(return_areanids.length==0&&ar_de=='F'){disabledStatus='1';}
else if(return_areanids.length>0){if(di_jq.inArray(ar_nid,return_areanids)<0)
disabledStatus='1';}}
if(arData.outputFormat=='aid')outputval=ar_ID;else if(arData.outputFormat=='nid')outputval=ar_nid;else if(arData.outputFormat=='name')outputval=ar_name;else if(arData.outputFormat=='nid_name')outputval=ar_nid+'_'+ar_name;else if(arData.outputFormat=='aid_name')outputval=ar_ID+'_'+ar_name;else if(arData.outputFormat=='aid_nid_name')outputval=ar_ID+'_'+ar_nid+'_'+ar_name;ar_ui+='<li id="'+ar_nid+'_'+uid+'" rel="'+ar_subnation+'" val="'+ar_ID+delm+ar_nid+delm+ar_lvl+delm+di_strEscapeSpclchar(ar_name)+delm+layer_nid+'"';if(ar_subnation=='T')ar_ui+=' class="collapsed"';ar_ui+='>';if(ar_subnation=='T')
ar_ui+='<span class="ui-icon ui-icon-triangle-1-e"></span>';if(arData.showAreaId==true){var displayAreaLabel=ar_name+" ("+ar_ID+") ";}
else{var displayAreaLabel=ar_name;}
var selectedClass='';if(arData.multiple==true){ar_ui+='<input type="checkbox" value="'+outputval+'" ';if(disabledStatus=='1'){ar_ui+=disabledCheckbox;}
if(chkSelection(uid,outputval+'||'+di_trim(displayAreaLabel),ar_lvl)==true&&disabledStatus==0){ar_ui+=' '+checkedCheckbox;}
ar_ui+='>';}
else{if(chkSelection(uid,outputval+'||'+di_trim(displayAreaLabel),ar_lvl)==true)
selectedClass='di_gui_singlenode_selection';}
var labelDisabledClass='';if(disabledStatus=='1'){labelDisabledClass='di_gui_deactive_label';}
if(arData.flagIconPath!='')
ar_ui+=flagFunc(uid,ar_ID);ar_ui+='<label class="di_gui_tree_label '+labelDisabledClass+' '+selectedClass+'" rel="lbl">'+displayAreaLabel+'</label>';if(arData.metadataPath!='')
ar_ui+=metaFunc(uid);ar_ui+='</li>';});}});return ar_ui;}
di_areaInitObject.prototype.di_getSubnationAreas=function(uid,area_id,level,area_nid){var delm=this.delm;var ar_ui='';var disabledCheckbox='disabled="true"';var checkedCheckbox='checked="true"';var arData=di_areacominputarr[uid];var isDataExistFunc=arData.callbacks.whereDataExist;try
{var return_areanids='';if(arData.isDataExist=='T'){var funcResult=eval(isDataExistFunc)(area_nid);if(funcResult!='false'){return_areanids=funcResult.split(",");}}
arData.areaObj.di_areaLoadingSH(uid,true);di_jq.ajax({type:"GET",url:arData.codelistPath+'/subnationals/'+area_id+'.xml',dataType:"xml",async:false,success:function(xml){ar_ui='<ul>';var counti=0;var totalItems=di_jq(xml).find('area').length;di_jq(xml).find('area').each(function(){var disabledStatus='0';var ar_name=di_jq(this).attr("name");var ar_nid=di_jq(this).attr("nID");var ar_ID=di_jq(this).attr("aID");var ar_de=di_jq(this).attr("dataexists");var ar_lvl=di_jq(this).attr("lvl");var ar_subNational=di_jq(this).attr("subNational");var layer_nid=di_jq(this).attr("lnid");var outputval=ar_nid;var parnetID=di_jq(this).attr("parentID");var parentName=di_jq(this).attr("parentName");if(arData.isDataExist=='T'){if(return_areanids.length==0&&ar_de=='F'){disabledStatus='1';}
else if(return_areanids.length>0){if(di_jq.inArray(ar_nid,return_areanids)<0)
disabledStatus='1';}}
if(arData.outputFormat=='aid')outputval=ar_ID;else if(arData.outputFormat=='nid')outputval=ar_nid;else if(arData.outputFormat=='name')outputval=ar_name;else if(arData.outputFormat=='nid_name')outputval=ar_nid+'_'+ar_name;else if(arData.outputFormat=='aid_name')outputval=ar_ID+'_'+ar_name;else if(arData.outputFormat=='aid_nid_name')outputval=ar_ID+'_'+ar_nid+'_'+ar_name;outputval=di_strEscapeSpclchar(outputval);if(counti==0&&(arData.multiple==true)){if(arData.quickSel==true){ar_ui+=arData.areaObj.di_getQuickSelectionUI(uid,level,parnetID,parentName,totalItems);}
else if(totalItems>1){ar_ui+='<li rel="all" class="di_gui_lists_specialoption di_gui_label_indent" val=""><input type="checkbox" value="">'+arData.labels.selectAll+'</li>';}}
var parent_arrow_icon='<span class="ui-icon ui-icon-triangle-1-e"></span>';var className="collapsed";if(ar_subNational!='T'){className="leaf";parent_arrow_icon='<span></span>';}
ar_ui+='<li id="'+area_nid+'_'+ar_nid+'_'+uid+'" rel="'+ar_subNational+'" class="'+className+' di_gui_label_indent" val="'+ar_ID+delm+ar_nid+delm+ar_lvl+delm+di_strEscapeSpclchar(ar_name)+delm+layer_nid+'">'+parent_arrow_icon+'';if(arData.showAreaId==true){var displayAreaLabel=ar_name+" ("+ar_ID+") ";}
else{var displayAreaLabel=ar_name;}
if(arData.multiple==true){ar_ui+='<input type="checkbox" value="'+outputval+'" ';if(disabledStatus=='1')
ar_ui+=disabledCheckbox;if(arData.areaObj.di_checkSelection(uid,outputval+'||'+di_trim(displayAreaLabel),ar_lvl)==true&&disabledStatus==0)
ar_ui+=' '+checkedCheckbox;ar_ui+='>';}
var labelDisabledClass='';if(disabledStatus=='1'){labelDisabledClass='di_gui_deactive_label';}
if(arData.flagIconPath!='')
ar_ui+=arData.areaObj.di_areaGetFlag(uid,ar_ID);ar_ui+='<label class="di_gui_tree_label '+labelDisabledClass+'" rel="lbl">'+displayAreaLabel+'</label>';if(arData.metadataPath!='')
ar_ui+=arData.areaObj.di_getAreaMetadataUI(uid);ar_ui+='</li>';counti++;});ar_ui+='</ul>';arData.areaObj.di_areaLoadingSH(uid,false);}});}
catch(err){}
arData.areaObj.di_areaLoadingSH(uid,false);return ar_ui;}
di_areaInitObject.prototype.di_getAreaListData=function(uid){var aropts=di_areacominputarr[uid];var level='';var pagecount='';var listDt=di_jq('#di_arealevelbox_'+uid).val();if(di_safevalue(listDt)!=''){listDt=listDt.split('__');level=listDt[0];pagecount=listDt[1];}
var filename=aropts.codelistPath+'/arealevels/'+level+'_'+aropts.currentPage+'.xml';var keyword='';if(di_safevalue(di_jq('#area_search_'+uid).val())!=''&&di_jq('#area_search_'+uid).val()!=aropts.labels.search){keyword=di_jq('#area_search_'+uid).val();var keyFirstChar=keyword.charAt(0).toLowerCase();filename=aropts.codelistPath+'/areasearch/'+keyFirstChar+'.xml';this.di_getPagingUI(uid,'');}
else{this.di_getPagingUI(uid,pagecount);}
return this.di_getList(uid,filename,level,keyword);}
di_areaInitObject.prototype.di_getSearch=function(uid,keyword){var ardata=di_areacominputarr[uid];if(keyword!=''&&keyword!=ardata.labels.search){if(keyword.length>0){ardata.areaObj.di_areaChangeViewMode(uid,'list');ardata.areaObj.di_areaSetLinksVisiblity(uid,'list');ardata.areaObj.di_areaInitiateMode(uid,'render');}}
else if(keyword==''){ardata.areaObj.di_areaChangeViewMode(uid,ardata.storePreMode);ardata.areaObj.di_areaSetLinksVisiblity(uid,ardata.storePreMode);ardata.areaObj.di_areaInitiateMode(uid,'render');}}
di_areaInitObject.prototype.di_getList=function(uid,filename,level,keyword){var ar_ui='';var disabledCheckbox='disabled="true"';var checkedCheckbox='checked="true"';var aropts=di_areacominputarr[uid];var delm=this.delm;var metaFunc=this.di_getAreaMetadataUI;di_jq.ajax({type:"GET",url:filename,dataType:"xml",async:false,success:function(xml){var spatern='a';if(keyword!=''&&di_safevalue(level)!='')
spatern='a[l="'+level+'"]';var return_areanids='';if(aropts.isDataExist=='T'){var funcResult=eval(aropts.callbacks.whereDataExist)('L_'+level);if(funcResult!='false'){return_areanids=funcResult.split(",");}}
var totalItems=di_jq(xml).find(spatern).length;var icount=0;di_jq(xml).find(spatern).each(function(){var disabledStatus='0';var ar_name=di_jq(this).attr("n");var ar_nid=di_jq(this).attr("nid");var ar_ID=di_jq(this).attr("id");var ar_de=di_jq(this).attr("dataexists");var layer_nid=di_jq(this).attr("lnid");var outputval=ar_nid;if(aropts.isDataExist=='T'){if(return_areanids.length==0&&ar_de=='F'){disabledStatus='1';}
else if(return_areanids.length>0){if(di_jq.inArray(ar_nid,return_areanids)<0)
disabledStatus='1';}}
var containsFoo=ar_name.toLowerCase().indexOf(keyword.toLowerCase())>=0;if(containsFoo==true){if(aropts.outputFormat=='aid')outputval=ar_ID;else if(aropts.outputFormat=='nid')outputval=ar_nid;else if(aropts.outputFormat=='name')outputval=ar_name;else if(aropts.outputFormat=='nid_name')outputval=ar_nid+'_'+ar_name;else if(aropts.outputFormat=='aid_name')outputval=ar_ID+'_'+ar_name;else if(aropts.outputFormat=='aid_nid_name')outputval=ar_ID+'_'+ar_nid+'_'+ar_name;outputval=di_strEscapeSpclchar(outputval);if(icount==0&&(aropts.multiple==true)){if(totalItems>1){ar_ui+='<li rel="all" class="di_gui_lists_specialoption" val=""><input type="checkbox" value="">'+aropts.labels.selectAll+'</li>';}}
ar_ui+='<li id="'+ar_nid+'_'+uid+'" rel="" class="di_gui_padding1" val="'+ar_ID+delm+ar_nid+delm+level+delm+di_strEscapeSpclchar(ar_name)+delm+layer_nid+'">';var displayAreaLabel=ar_name;if(aropts.showAreaId==true){displayAreaLabel+=" ("+ar_ID+") ";}
var selectedClass='';if(aropts.multiple==true){ar_ui+='<input type="checkbox" value="'+outputval+'" ';if(disabledStatus=='1')
ar_ui+=disabledCheckbox;if(aropts.areaObj.di_checkSelection(uid,outputval+'||'+di_trim(displayAreaLabel),level)==true&&disabledStatus==0)
ar_ui+=' '+checkedCheckbox;ar_ui+='>';}
else if(aropts.areaObj.di_checkSelection(uid,outputval+'||'+di_trim(displayAreaLabel),level)==true){selectedClass='di_gui_singlenode_selection';}
var labelDisabledClass='';if(disabledStatus=='1'){labelDisabledClass='di_gui_deactive_label';}
if(aropts.flagIconPath!='')
ar_ui+=aropts.areaObj.di_areaGetFlag(uid,ar_ID);ar_ui+='<label class="di_gui_tree_label '+labelDisabledClass+' '+selectedClass+'" rel="lbl">'+displayAreaLabel+'</label>';if(aropts.metadataPath!='')
ar_ui+=metaFunc(uid);ar_ui+='</li>';}
icount++;});}});return ar_ui;}
di_areaInitObject.prototype.di_areaGetFlag=function(uid,areaID){var icon_ui="";try
{if(di_areacominputarr[uid].flagIconPath!=''&&areaID!=''){var file=di_areacominputarr[uid].flagIconPath+areaID+".png";icon_ui='<img src="'+file+'" width="15" height="15" border="0">&nbsp;';}}
catch(err){}
return icon_ui;}
di_areaInitObject.prototype.di_getAreaMetadataUI=function(uid){var icon_ui="&nbsp;";try
{if(di_areacominputarr[uid].metadataPath!=''){icon_ui=' &nbsp;<label rel="md" class="di_gui_link2" style="display:none;">'+di_areacominputarr[uid].labels.metadata+'</label>';}}
catch(err){}
return icon_ui;}
di_areaInitObject.prototype.di_showAreaMetadataPOP=function(uid,title,l_nid)
{var xml_file;var xsl_file;var transform_html='';try
{di_jq('#di_areamtdt_'+uid).html('');xml_file=di_areacominputarr[uid].metadataPath+'area/'+l_nid+'.xml';xsl_file=di_areacominputarr[uid].metadataPath+'MAP.xsl';transform_html=di_xml_xsl_transformation(xml_file,xsl_file);di_compApplyMasking();di_jq('#di_areamtdt_view_'+uid).show();di_jq('#di_areamtdt_title_'+uid).html(title);di_jq('#di_areamtdt_'+uid).html(transform_html);}
catch(err){}}
di_areaInitObject.prototype.di_hideAreaMetadataPOP=function(uid)
{di_jq('#di_areamtdt_view_'+uid).hide();di_compRemoveMasking();}
di_areaInitObject.prototype.di_getQuickSelectionUI=function(uid,ignore_lvl,parentID,parentName,totalItem){var qs_ui="";var ardata=di_areacominputarr[uid];var checkedCheckbox='checked="true"';var delm=this.delm;try
{di_jq.ajax({type:"GET",url:ardata.codelistPath+'/arealevels/levelmaster.xml',dataType:"xml",async:false,timeout:5000,success:function(xml){var totLvls=di_jq(xml).find('al').length;if(ignore_lvl<totLvls-1){if(ardata.quickSelMode=='HRZ'){qs_ui+='<li rel="QS" class="di_gui_lists_specialoption di_gui_label_indent di_gui_label_btmbdr" val="">';if(totalItem>1)
qs_ui+='<input type="checkbox" value=""/>'+ardata.labels.selectAll+'&nbsp;&nbsp;';di_jq(xml).find('al').each(function(){var lavel_name=di_jq(this).attr("n");var lavel_number=di_jq(this).attr("number");if(lavel_number>parseInt(ignore_lvl)+1){if(ardata.showAreaId==true){var vSlabel=parentName+' ('+parentID+')'+' - '+lavel_name;}
else{var vSlabel=parentName+' - '+lavel_name;}
qs_ui+='<input type="checkbox" value="'+parentID+delm+vSlabel+delm+lavel_number+'" ';var matchval='QS_'+parentID+'_L'+lavel_number+'||'+vSlabel;if(ardata.areaObj.di_checkSelection(uid,matchval,lavel_number)==true){qs_ui+=checkedCheckbox;}
qs_ui+=' rel="QS_'+parentID+'_L'+lavel_number+'"/>'+lavel_name+'&nbsp;&nbsp;';}});qs_ui+='</li>';}
else{di_jq(xml).find('al').each(function(){var lavel_name=di_jq(this).attr("n");var lavel_number=di_jq(this).attr("number");if(lavel_number>parseInt(ignore_lvl)+1){if(ardata.showAreaId==true){var vSlabel=parentName+' ('+parentID+')'+' - '+lavel_name;}
else{var vSlabel=parentName+' - '+lavel_name;}
qs_ui+='<li rel="QS" class="di_gui_lists_specialoption di_gui_label_indent" val=""><input type="checkbox" value="'+parentID+delm+vSlabel+delm+lavel_number+'" ';var matchval='QS_'+parentID+'_L'+lavel_number+'||'+vSlabel;if(ardata.areaObj.di_checkSelection(uid,matchval,lavel_number)==true){qs_ui+=checkedCheckbox;}
qs_ui+=' rel="QS_'+parentID+'_L'+lavel_number+'">'+lavel_name+'</li>';}});if(totalItem>1)
qs_ui+='<li rel="all" class="di_gui_lists_specialoption di_gui_label_indent" val=""><input type="checkbox" value="">'+ardata.labels.selectAll+'</li>';}}
else if(totalItem>1){qs_ui+='<li rel="all" class="di_gui_lists_specialoption di_gui_label_indent" val=""><input type="checkbox" value="">'+ardata.labels.selectAll+'</li>';}}});}
catch(err){}
return qs_ui;}
di_areaInitObject.prototype.di_manageAreaSelection=function(uid,status,chkval,level){this.di_manageAreaSelection_1(uid,status,chkval,level);var chkvalArr=chkval.split("||");var ardata=di_areacominputarr[uid];var oldVal=[];var exist=-1;try
{oldVal=di_safevalue(ardata.selectedAreas[level]);if(oldVal=='')oldVal=[];exist=this.di_checkDuplicate(oldVal,chkval);if(status==true)
{if(exist==-1){oldVal.push(chkval);}
ardata.selectedAreas[level]=oldVal;}
else
{if(exist>-1){oldVal.splice(exist,1);ardata.selectedAreas[level]=oldVal;di_jq('#di_area_tree1_'+uid+' input[value="'+chkvalArr[0]+'"]').attr('checked',false);}}
if(ardata.multiple==true){this.di_areaCount(uid);}}
catch(err){}}
di_areaInitObject.prototype.di_manageAreaSelection_1=function(uid,status,chkval,level){var ardata=di_areacominputarr[uid];var oldVal=di_safevalue(ardata.selectedAreas[-1]);var exist=-1;if(oldVal!=''&&oldVal.length>0){var lvlVal=di_safevalue(ardata.selectedAreas[level]);if(lvlVal=='')lvlVal=[];var checkVal=chkval.split('||');for(var i=0;i<oldVal.length;i++){var existVal=oldVal[i].split('||');if(di_safevalue(existVal[0])!=''&&existVal[0]==checkVal[0]){exist=i;break;}}
if(exist>-1){oldVal.splice(exist,1);ardata.selectedAreas[-1]=oldVal;lvlVal.push(chkval);ardata.selectedAreas[level]=lvlVal;}}}
di_areaInitObject.prototype.di_checkSelection=function(uid,value,level)
{var returnStr=false;var exist=-1;try
{var check=di_safevalue(di_areacominputarr[uid].selectedAreas[level]);if(check!=''){if(di_areacominputarr[uid].selectedAreas[level].length<1){exist=-1;}
for(var i=0;i<di_areacominputarr[uid].selectedAreas[level].length;i++){if(di_areacominputarr[uid].selectedAreas[level][i]==value){exist=i;}
else{var existVal=di_areacominputarr[uid].selectedAreas[level][i].split('||');var checkVal=value.split('||');if(di_safevalue(existVal[0])!=''&&existVal[0]==checkVal[0])exist=i;}}}
var wolvl=di_safevalue(di_areacominputarr[uid].selectedAreas[-1]);if(wolvl!=''&&wolvl.length>0){var checkVal2=value.split('||');for(var i=0;i<wolvl.length;i++){var existVal2=wolvl[i].split('||');if(di_safevalue(existVal2[0])!=''&&existVal2[0]==checkVal2[0])exist=i;}}
if(exist>-1)returnStr=true;}
catch(err){}
return returnStr;}
di_areaInitObject.prototype.di_checkDuplicate=function(myarray,value){if(myarray.length<1){return-1;}
for(var i=0;i<myarray.length;i++){if(myarray[i]==value){return i;}
else{var existVal=myarray[i].split('||');var checkVal=value.split('||');if(di_safevalue(existVal[0])!=''&&existVal[0]==checkVal[0])return i;}}
return-1;}
di_areaInitObject.prototype.di_areaCount=function(uid)
{var selected_area_count=0;var level_all_data=[];var ardata=di_areacominputarr[uid];try
{var initVal=0;if(di_safevalue(ardata.selectedAreas[-1])!=''&&di_safevalue(ardata.selectedAreas[-1]).length>0){initVal=-1;}
for(var i=initVal;i<ardata.selectedAreas.length;i++)
{level_all_data=di_safevalue(ardata.selectedAreas[i]);if(level_all_data=='')level_all_data=[];if(level_all_data.length>0)
{for(var j=0;j<level_all_data.length;j++){if(di_safevalue(level_all_data[j])!='')selected_area_count++;}}}}
catch(err){}
di_jq('#di_area_count_sel_'+uid).html(selected_area_count);}
di_areaInitObject.prototype.di_singleSelection=function(uid,areaid,area_val,multiple,level)
{try
{var area_label=di_jq('#'+areaid+' > label').text();if(multiple=='false'){di_jq('li > label').removeClass('di_gui_singlenode_selection');di_jq('#'+areaid+' > label').addClass('di_gui_singlenode_selection');di_jq('#selected_area').val(area_val+'@@'+area_label);if(di_area_hot_selection_func!=''){eval(di_area_hot_selection_func);}}
else{}}
catch(err){}}
di_areaInitObject.prototype.di_viewSelections=function(uid)
{try
{di_jq('#di_areaviewsel_con_'+uid).html('');var ardata=di_areacominputarr[uid].selectedAreas;var initVal=0;if(di_safevalue(ardata[-1])!=''&&di_safevalue(ardata[-1]).length>0){initVal=-1;}
for(var i=initVal;i<ardata.length;i++){var lvldata=di_safevalue(ardata[i]);if(lvldata!=''){for(var j=0;j<lvldata.length;j++){var data=lvldata[j].split('||');var di_render_ui='<div style="padding-top:5px;"><img src="'+di_images_path+'/icon_cross.gif" class="di_gui_cursor_pointer" rel="'+i+'[****]'+lvldata[j]+'"> &nbsp;'+data[1]+'</div>';di_jq('#di_areaviewsel_con_'+uid).append(di_render_ui);}}}
di_jq('#di_areaviewsel_list_'+uid).show("slide",{direction:"right"},500);}
catch(err){}}
di_areaInitObject.prototype.di_getPagingUI=function(uid,total){try
{ardata=di_areacominputarr[uid];if(total!=''&&total>1){var ui='<span class="di_gui_paging_text" rel="first"><img src="'+di_images_path+'/PagingFirst.gif" width="8" height="11" title="'+ardata.labels.first+'"></span>&nbsp;<span class="di_gui_paging_text" rel="back"><img src="'+di_images_path+'/PagingPrev.gif" width="6" height="11" title="'+ardata.labels.back+'"></span>&nbsp;';ui+='<select class="di_gui_dropdown1">';for(var i=1;i<=total;i++){var selected='';if(i==ardata.currentPage)selected="selected";ui+='<option value="'+i+'" '+selected+'> '+ardata.labels.page+' '+i+'</option>';}
ui+='</select>';ui+='&nbsp;<span class="di_gui_paging_text" rel="next"><img src="'+di_images_path+'/PagingNext.gif" width="6" height="11" title="'+ardata.labels.next+'"></span>&nbsp;<span class="di_gui_paging_text" rel="last"><img src="'+di_images_path+'/PagingLast.gif" width="8" height="11" title="'+ardata.labels.last+'"></span>';di_jq('#di_list_paging_'+uid).html(ui);di_jq('#di_list_paging_'+uid+' span').click(function(){var page=0;var curr_page=parseInt(di_jq('#di_list_paging_'+uid+' select').val());var action=di_jq(this).attr('rel');if(action=='first'){if(curr_page==1)return false;else page=1;}
else if(action=='back'){if(curr_page>1)page=curr_page-1;else return false;}
else if(action=='next'){if(curr_page<total)page=parseInt(curr_page)+1;else return false;}
else if(action=='last'){if(curr_page==total)return false;else page=total;}
if(page>0){ardata.currentPage=page;ardata.areaObj.di_areaInitiateMode(uid,'render');}});di_jq('#di_list_paging_'+uid+' select').change(function(){ardata.currentPage=this.value;ardata.areaObj.di_areaInitiateMode(uid,'render');});}
else{di_jq('#di_list_paging_'+uid).html('');}}
catch(err){}}
di_areaInitObject.prototype.di_areaOK=function(uid){try
{var ardata=di_areacominputarr[uid];return ardata.selectedAreas;}
catch(err){}}
di_areaInitObject.prototype.di_areaCancel=function(uid){try
{var ardata=di_areacominputarr[uid];if(ardata.multiple==false){di_jq('li > label').attr('style','');}
else{di_jq('#di_area_tree1_'+uid+' input:checkbox:checked').attr('checked',false);}
ardata.selectedAreas=[];ardata.areaObj.di_areaCount(uid);}
catch(err){}}
di_areaInitObject.prototype.di_createMAPUI=function(uid){var ardata=di_areacominputarr[uid];return'<div id="DIareaMapModeView_'+uid+'" class="di_areamap_modelist"><span class="di_areamapModeIcon di_areamap_mnull" title="Default" rel=""><a class="di_areamapModeIcon selMode"></a></span><span class="di_areamapModeIcon di_areamap_mfull" title="Full Extent" rel="full"></span><span class="di_areamapModeIcon di_areamap_mzext" title="Zoom in frame" rel="zext"></span><span  class="di_areamapModeIcon di_areamap_mzin" title="Zoom In" rel="zin"></span><span class="di_areamapModeIcon di_areamap_mzout" title="Zoom Out" rel="zout"></span><span class="di_areamapModeIcon di_areamap_mpan" title="Pan" rel="pan"></span><span class="di_areamapModeIcon di_areamap_mlabel" title="Labels" rel="label"></span></div><div id="DIareaMapImageView_'+uid+'" style="overflow:hidden;"></div>';}
di_areaInitObject.prototype.di_MAPBindEvent=function(uid){var ardata=di_areacominputarr[uid];di_jq('#DIareaMapModeView_'+uid+' span').click(function(){var map_object=di_areacominputarr[uid].areaMapObj;var pmode=di_jq(this).attr('rel');if(pmode=='full'){ardata.areaObj.di_areaLoadingSH(uid,true);map_object.setFullExtent();ardata.areaObj.di_areaLoadingSH(uid,false);}
else if(pmode=='label'){ardata.areaObj.di_areaLoadingSH(uid,true);if(di_jq(this).find('.selLabel').length==0){di_jq(this).append('<a class="di_areamapModeIcon selLabel"></a>');map_object.setAreaLabels(true);}
else{di_jq(this).find('a').remove();map_object.setAreaLabels(false);}
ardata.areaObj.di_areaLoadingSH(uid,false);}
else{di_jq('#DIareaMapModeView_'+uid+' span a').remove();di_jq(this).append('<a class="di_areamapModeIcon selMode"></a>');map_object.mode=pmode;}
if(pmode=='zin'){di_jq('#diAreaMapImage_'+uid).css('cursor','-moz-zoom-in');}
else if(pmode=='zout'){di_jq('#diAreaMapImage_'+uid).css('cursor','-moz-zoom-out');}
else if(pmode=='pan'){di_jq('#diAreaMapImage_'+uid).css('cursor','-moz-grab');}
else if(pmode=='zext'){di_jq('#diAreaMapImage_'+uid).css('cursor','crosshair');}
else if(pmode==''){di_jq('#diAreaMapImage_'+uid).css('cursor','default');}
if(pmode=='pan'){map_object.refreshMap();di_jq('#diAreaMapImage_'+uid).draggable('enable');map_object.setPanZoom();}
else if(pmode=='zext'){di_jq('#diAreaMapImage_'+uid).draggable('disable',1);di_jq("#diAreaMapImage_"+uid).removeAttr('class');di_jq('#diAreaMapImage_'+uid).css('opacity',1);di_jq("#diAreaMapImage_"+uid).selectable({disabled:false});map_object.setRubberZoom();}
else{di_jq("#diAreaMapImage_"+uid).removeAttr('class');di_jq('#diAreaMapImage_'+uid).draggable('disable',1);di_jq("#diAreaMapImage_"+uid).removeClass('ui-draggable-disabled');di_jq("#diAreaMapImage_"+uid).removeClass('ui-state-disabled');di_jq("#diAreaMapImage_"+uid).removeClass('ui-selectable');map_object.refreshMap();}});var DIAREADELAY=500,diareaclicks=0,diareatimer=null;di_jq('#diAreaMapImage_'+uid).live('click',function(e,ui){var map_object=di_areacominputarr[uid].areaMapObj;var offset=di_jq(this).offset();var x=Math.round(e.pageX-offset.left);var y=Math.round(e.pageY-offset.top);diareaclicks++;ardata.areaObj.di_areaLoadingSH(uid,true);if(diareaclicks===1){diareatimer=setTimeout(function(){if(map_object.mode=='zin')map_object.zoomIn(x,y);if(map_object.mode=='zout')map_object.zoomOut(x,y);if(map_object.mode==''){map_object.clickMapImage(x,y,'single');var areaInfo=map_object.clickedAreaInfo(x,y);var areaInfoAr=areaInfo.split('[****]');if(areaInfoAr[4]=="true")
ardata.areaObj.di_manageAreaSelection(uid,true,areaInfoAr[0]+'||'+areaInfoAr[3]+' ('+areaInfoAr[1]+')',areaInfoAr[2]);else
ardata.areaObj.di_manageAreaSelection(uid,false,areaInfoAr[0]+'||'+areaInfoAr[3]+' ('+areaInfoAr[1]+')',areaInfoAr[2]);}
diareaclicks=0;ardata.areaObj.di_areaLoadingSH(uid,false);},DIAREADELAY);}
else{clearTimeout(diareatimer);if(map_object.mode==''){map_object.clickMapImage(x,y,'double');ardata.areaObj.di_areaLoadingSH(uid,false);}
diareaclicks=0;}});}