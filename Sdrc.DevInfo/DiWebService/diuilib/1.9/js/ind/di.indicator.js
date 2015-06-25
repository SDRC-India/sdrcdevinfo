/*
    * di.indicator.js
	* Function to create indicator UI
	*
*/

/* main function for indicator component */
function di_drawIndicatorList(div_id, indopts) {
	//alert(indopts.sectors.SC)
	/* validation for given inputs */
	if(di_safevalue(div_id)=='') return false;
	if(di_safevalue(indopts.codelistPath)=='') return false;
	indopts.currentMode = 'list'; // current mode
	indopts.storePreMode = 'list'; // buffer varibale for storing previous mode
	indopts.currentSector = '';
	indopts.selectedInds = []; // store selected indicators
	indopts.selectedDims = []; // store selected simentions
	indopts.selectedDimVals = []; // store selected simention values
	indopts.selectedIus = []; // store selected iusnids
	indopts.isDataExist = 'F';
	
	var uid = di_generateUID(); // generate unique id
	this.uid = uid;

	if(di_safevalue(indopts.sectors.SC)=='') indopts.sectors.SC = 'Sector';
	if(di_safevalue(indopts.sectors.GL)=='') indopts.sectors.GL = 'Goal';
	if(di_safevalue(indopts.sectors.SR)=='') indopts.sectors.SR = 'Data Sources';
	if(di_safevalue(indopts.sectors.IT)=='') indopts.sectors.IT = 'Institution';
	if(di_safevalue(indopts.sectors.TH)=='') indopts.sectors.TH = 'Theme';
	if(di_safevalue(indopts.sectors.CN)=='') indopts.sectors.CN = 'Convention';
	if(di_safevalue(indopts.sectors.CF)=='') indopts.sectors.CF = 'Conceptual Framework';
	if(di_safevalue(indopts.labels.selectAll)=='') indopts.labels.selectAll = 'Select All';
	if(di_safevalue(indopts.labels.byList)=='') indopts.labels.byList = 'List A to Z';
	if(di_safevalue(indopts.labels.byTree)=='') indopts.labels.byTree = 'By Tree';
	if(di_safevalue(indopts.labels.search)=='') indopts.labels.search = 'Search';
	if(di_safevalue(indopts.labels.viewSel)=='') indopts.labels.viewSel = 'View Selection';
	if(di_safevalue(indopts.labels.metadata)=='') indopts.labels.metadata = 'Metadata';
	if(di_safevalue(indopts.labels.dataExist)=='') indopts.labels.dataExist = 'Show where data exist';
	if(di_safevalue(indopts.labels.clear)=='') indopts.labels.clear = 'Clear';
	if(di_safevalue(indopts.labels.close)=='') indopts.labels.close = 'Close';
	if(di_safevalue(indopts.labels.OK)=='') indopts.labels.OK = 'OK';
	if(di_safevalue(indopts.labels.Cancel)=='') indopts.labels.Cancel = 'Cancel';
	if(di_safevalue(indopts.labels.more)=='') indopts.labels.more = 'More';
	if(di_safevalue(indopts.labels.sgDim)=='') indopts.labels.sgDim = 'Subgroup Dimensions';

	// manage width and height of component
	if(indopts.width.indexOf('%')==-1 && indopts.width<600) indopts.width = '600';
	if(indopts.height<300) indopts.height = '300';

	// decide the current mode & cuurent sector to be displayed
	if(indopts.mode.toLowerCase()=='both') { 
		var defMode = indopts.defaultMode.toLowerCase();
		if(di_safevalue(defMode)!='' && defMode!='list') {
			defModeArr = defMode.split("_");
			indopts.currentMode = defModeArr[0];
			if(di_safevalue(defModeArr[1])!='')
			indopts.currentSector = defModeArr[1].toUpperCase();
		}
		else if(di_safevalue(defMode)!='') {
			indopts.currentMode = defMode;
		}
	}
	else if(indopts.mode.toLowerCase()=='list') {
		indopts.currentMode = 'list';
	}
	else {
		var listopt = indopts.mode.toLowerCase();
		listoptAr = listopt.split("_");
		indopts.currentMode = 'tree';
		if(di_safevalue(listoptAr[1])!='') indopts.currentSector = listoptAr[1].toUpperCase();
	}

	indopts.storePreMode = indopts.currentMode;
	di_indcominputarr[uid] = indopts;

	/* creating an onject for draw div id */
	div_obj = document.getElementById(div_id);
	/* Start for area component HTML */
	var html_data = this.createUI(uid);
	div_obj.innerHTML = html_data;

	// Render search component
	var searchObj = new di_drawSearchBox(
		'di_indsearch_ctrl_td_'+uid,				// Div/TD id
		'ind_search_'+uid,							// Id Prefix	
		'',											// Width
		'20px',										// Height
		indopts.labels.search,						// Default text for search component
		''											// callback function on keyup
	);

	di_jq(document).ready(function() { 

		// getting width/height details
		var outerwidth = di_jq('#di_indouterdiv_'+uid).width(); 
		var outerheight = di_jq('#di_indouterdiv_'+uid).height();
		var headerheight = di_jq('#di_ind_td_body_'+uid).height();
		var titleheight = di_jq('#di_indouterdiv_'+uid).children('.di_gui_title').height();
		var viewselheight = di_jq('#di_ind_viewsel_panel_'+uid).height();
		
		// setting width/height
		var subtractHeight = headerheight;
		if(di_safevalue(titleheight)!='') subtractHeight = subtractHeight + titleheight;
		if(di_safevalue(viewselheight)!='') subtractHeight = subtractHeight + viewselheight;
		di_jq('#di_ind_td_body_'+uid).css("width", outerwidth+'px');
		di_jq('#di_ind_whmask_'+uid).css("width", (outerwidth -3)+'px');
		di_jq('#di_ind_whmask_'+uid).css("height", (di_indcominputarr[uid].height - subtractHeight ) +'px');
		di_jq('#di_ind_tabs-1_'+uid).css('height', (di_indcominputarr[uid].height - subtractHeight ) +'px');
		di_jq('#di_indviewsel_con_'+uid).css('width', (outerwidth - 5)+'px');
		di_jq('#di_indviewsel_con_'+uid).css('height', (di_indcominputarr[uid].height - 40)+'px');
		di_jq('#di_indmtdt_view_'+uid).css('width', (outerwidth - 15)+'px');

		// init indicator object
		var indInitObj = new di_indInitObject(uid);
		di_indcominputarr[uid].indObj = indInitObj;

		// store Json data
		indInitObj.storeJsonSelection(uid, di_indcominputarr[uid].jsonSelection);

		// render default indicator view 
		indInitObj.di_indInitiateView(uid, 'render');

		// setting active/deactive links
		indInitObj.di_setLinksVisiblity(uid);

		// calling tree structure
		di_jq('#di_ind_tree1_'+uid).checkboxTree({ 
			 trueCheck:{
				 val:'no'
			 },
			 onCheck: { 
				 ancestors: 'check', 
				 descendants: 'check', 
				 others: '' 
			 }, 
			 onUncheck: { 
				 descendants: 'uncheck'
			 } 
		});

		// bindling click on links to change mode
		di_jq('#di_indlinks_'+uid+' span').click(function() { 
			//changeViewMode
			var modeclass = di_jq(this).attr('class');
			var relmode = di_jq(this).attr('rel');
			var sid = di_jq(this).attr('id');
			var opts = di_indcominputarr[uid];

			if(sid=='di_fsid_'+uid) {
				var sval = di_jq(this).attr('val');
				if(sval=='a') {
					if(di_jq('#di_sldiv_'+uid).css('display')!='none') {
						di_jq('#di_sldiv_'+uid).hide();
						//di_jq('#di_fsid_'+uid+ ' img').attr('src', di_images_path+'/down-arrow.png');
					}
					else {
						di_jq('#di_sldiv_'+uid).show();
						//di_jq('#di_fsid_'+uid+ ' img').attr('src', di_images_path+'/up_arrow.png');
					}
					return false;
				}
			} // added 23 July
			else if(sid=='di_fsidimg_'+uid){
				if(di_jq('#di_sldiv_'+uid).css('display')!='none') di_jq('#di_sldiv_'+uid).hide();
				else di_jq('#di_sldiv_'+uid).show();
				return false;
			}
			
			// change mode
			opts.indObj.di_changeMode(uid, relmode);
			// change the link visiblity
			opts.indObj.di_setLinksVisiblity(uid);
			// refresh search box
			searchObj.refresh();
			// render indicator content
			opts.indObj.di_indInitiateView(uid, 'render');
		
		});
		// binding if sectors are in menu layout
		di_jq('#di_sldiv_'+uid+ ' li').click(function() {
			var relmode = di_jq(this).attr('rel');
			var text = di_jq(this).html();
			var opts = di_indcominputarr[uid];

			//di_jq('#di_fsid_'+uid).html(text + ' <img src="'+di_images_path+'/down-arrow.png" width="10" style="cursor:pointer;"/>');
			di_jq('#di_fsid_'+uid).html(text); // commented above on 23 July
			di_jq('#di_fsid_'+uid).attr('rel', relmode);
			di_jq('#di_fsid_'+uid).attr('val', 'a');

			// change mode
			opts.indObj.di_changeMode(uid, relmode);
			// change the link visiblity
			opts.indObj.di_setLinksVisiblity(uid);
			// refresh search box
			searchObj.refresh();
			// render indicator content
			opts.indObj.di_indInitiateView(uid, 'render');
		});
		// for clicking on where data exist
		di_jq('#di_ind_data_exist_'+uid).bind('click', function() {
			var opts = di_indcominputarr[uid];
			if(this.checked==true) {
				opts.isDataExist = 'T';
			}
			else {
				opts.isDataExist = 'F';
			}
			// render indicator content
			opts.indObj.di_indInitiateView(uid, 'render');
		});
		// onkeyup functions for search box
		di_jq('#ind_search_'+uid).live('keyup', function() {
			di_indcominputarr[uid].indObj.di_getSearch(uid, this.value);
		});
		di_jq('#cros_ind_search_'+uid).live('click', function() {
			if(di_jq('#ind_search_'+uid).val()!='') {
				di_jq('#ind_search_'+uid).val('');
				di_jq('#ind_search_'+uid).removeClass('di_gui_searchbox_defaulttext');
				di_jq('#ind_search_'+uid).addClass('di_gui_searchbox_text');
				di_jq('#ind_search_'+uid).focus();
				di_indcominputarr[uid].indObj.di_getSearch(uid, '');
			}
		});
		// onclick metadata close icon
		di_jq('.di_gui_popupclose').click(function() {
			//alert(di_jq(this).closest('.di_gui_popup_block').length);
			di_indcominputarr[uid].indObj.di_hideMetadataPOP(uid);
		});
		// onclick view selections
		di_jq('#di_ind_count_sel_'+uid).closest('a').click(function() {
		 	di_indcominputarr[uid].indObj.di_viewSelections(uid);
		});
		di_jq('#di_vsindclose_'+uid).click(function() { 
			di_jq('#di_indviewsel_list_'+uid).hide('slide', {direction:'right'});
		});
		// click on view selection buttons
		di_jq('#di_vsindclear_'+uid).click(function() { 
			di_jq('#di_indviewsel_con_'+uid).html('');
			di_jq('#di_ind_tree1_'+uid+' input:checkbox:checked').attr('checked', '');
			di_jq('#di_indviewsel_list_'+uid).hide('slide', {direction:'right'});	
			
			di_indcominputarr[uid].indObj.di_manageSelection(uid, true, '', 'clear');
		});
		// click on remove icon on view selection
		di_jq('#di_indviewsel_con_'+uid+' img').live('click', function() { 
			var type = di_jq(this).attr('rel');
			var val = di_jq(this).parent().attr('rel');
			var valAr = val.split('||');
			if(type=='ius')	var iuval = di_jq(this).parent().parent().attr('rel');
			else var iuval = di_jq(this).parent().attr('rel');

			var iuvalAr = iuval.split('||');
			var iuidp = '#iu_'+iuvalAr[0]+'_'+iuvalAr[1]+'_'+uid+' ul';
			if(type=='iu') {
				di_indcominputarr[uid].indObj.di_manageSelection(uid, false, val, 'ind');
				di_jq('#iu_'+valAr[0]+'_'+valAr[1]+'_'+uid+ ' input').attr('checked', false);
				di_jq(this).parent().remove();

				// for refresh sg list if open
				if(di_jq(iuidp).length>0) {
					var sgtype = di_jq(iuidp).attr('rel');
					if(sgtype=='sg') {
						di_indcominputarr[uid].indObj.getSgData(uid, iuvalAr[0], iuvalAr[1], 'list', '');
					}
				}
			}
			else if(type=='ius') {
				var valStr = iuvalAr[0]+'||'+iuvalAr[1]+'||'+val;
				di_indcominputarr[uid].indObj.di_manageSelection(uid, false, valStr, 'ius');
				
				var parentVal = di_safevalue(di_indcominputarr[uid].selectedIus[iuvalAr[0]+ '_' +iuvalAr[1]]);
				if(parentVal!='' && parentVal.length>0) {
					di_jq(this).parent().remove();
					
					if(di_jq(iuidp).length>0) {
						var sgtype = di_jq(iuidp).attr('rel');
						if(sgtype=='sg') {
							di_jq('#sg_list_'+iuvalAr[0]+'_'+iuvalAr[1]+'_'+uid+ ' div input[value="'+valAr[0]+'"]').attr('checked', false);
							di_indcominputarr[uid].indObj.getSgData(uid, iuvalAr[0], iuvalAr[1], 'list', '');
						}
						else { // for iusg
							di_jq('#iu_'+iuvalAr[0]+'_'+iuvalAr[1]+'_'+uid+ ' li input[value="'+valAr[0]+'"]').attr('checked', false);
						}
					} // end if
				}
				else {
					di_jq(this).parent().parent().remove();
					di_jq('#iu_'+iuvalAr[0]+'_'+iuvalAr[1]+'_'+uid+ ' input').attr('checked', false);
				}
			}
		});

	});

}
/* function to create UI of the component */
di_drawIndicatorList.prototype.createUI = function(uid) {
	var ui='';
	try
	{
		var opts = di_indcominputarr[uid];
		var wd;

		if(opts.width.indexOf('%')>0) wd = opts.width;
		else wd = opts.width+'px';
	
		ui ='<div id="di_indouterdiv_'+uid+'" style="width:'+wd+'">';

		// ui for metadata
		if(opts.showMetadata==true) { 
			ui += '<div id="di_indmtdt_view_'+uid+'" class="di_gui_popup_block" style="display:none;"><div class="di_gui_popupclose_div"><b class="di_gui_popupclose" style="float:right;"></b></div><div id="di_indmtdt_title_'+uid+'" class="di_gui_title">'+opts.labels.metadata+'</div><div id="di_indmtdt_con_'+uid+'" class="di_gui_metadata_container"></div></div>';
		}
		// start view selections ui
		if(opts.showViewSel==true) {
			ui += '<div id="di_indviewsel_list_'+uid+'" style="position:absolute;z-index: 950; overflow: hidden; display:none;" role="dialog" tabindex="-1" aria-labelledby="alerthd" aria-hidden="false"><div class="di_gui_view_selections_box"><div class="di_gui_title">'+opts.labels.viewSel+'</div><div id="di_indviewsel_con_'+uid+'" class="di_gui_viewselections_body"></div><div style="text-align:right;padding-right:10px"><input id="di_vsindclear_'+uid+'" type="button" value="'+opts.labels.clear+'" class="di_gui_button"/><input id="di_vsindclose_'+uid+'" type="button" value="'+opts.labels.close+'" class="di_gui_button"/></div></div></div>';
		}
		// start title ui
		if(opts.title!='') {
		    // html for area component title
		    ui += '<div class="di_gui_title">' + opts.title + '</div>';
		}

		// Start body ui
		ui += '<div id="di_indwidget_'+uid+'"><table width="100%" cellpadding="0" cellspacing="0" border="0" class="di_gui_container"><tr><td id="di_ind_td_body_'+uid+'">';
		
		// for herder menu
		//ui += '<div class="di_gui_gradient_panel_box"><table width="98%" border="0" cellpadding="0" cellspacing="0" height="30"><tr><td width="5%" nowrap style="padding-right:5px;"><div id="di_indlinks_'+uid+'">'+ this.getMenuLinks_old(uid) +'</div></td><td width="5%" nowrap style="padding-right:10px;">';

		ui += '<div class="di_gui_gradient_panel_box"><table width="98%" border="0" cellpadding="0" cellspacing="0" height="30"><tr><td width="5%" nowrap style="padding-right:5px;">'+ this.getMenuLinks(uid) +'</td><td width="5%" nowrap style="padding-right:10px;">';
		
		if(opts.callbacks.whereDataExist!='') {
			ui += '<table width="100%" cellpadding="0" cellspacing="0"><tr><td><input id="di_ind_data_exist_'+uid+'" type="checkbox"></td><td class="di_gui_seld_txt" nowrap>'+opts.labels.dataExist+'</td></tr></table>';
		}
		ui += '</td><td width="90%" id="di_indsearch_ctrl_td_'+uid+'" nowrap></td></tr></table></div></td></tr><tr><td>';

		// area wheeler
		ui += '<div id="di_ind_whmask_'+uid+'" style="position:absolute;background-color:#000000;z-index:2000;padding-top:8px;text-align:center;opacity:.1;filter: alpha(opacity=10);"><div style="margin-top:5%;"><img src="'+di_images_path+'/ajax-loader.gif"></div></div>';
		//ui += '<div id="di_ind_wheeler_'+uid+'" style="margin-left:37%;margin-top:5%;position:absolute;z-index:2000;"><div class="di_gui_wheeler" style="display:block;"><img src="'+di_images_path+'/ajax-loader.gif"></div></div>';

		// Start for main content part
		ui += '<div id="di_ind_tabs-1_'+uid+'" class="di_gui_body"><ul id="di_ind_tree1_'+uid+'"></ul></div>';

		if(opts.showViewSel==true && opts.multiple==true) {
			ui += '<div id="di_ind_viewsel_panel_'+uid+'" class="di_gui_gradient_panel_bottombox"><table width="80%" cellspacing="0" cellpadding="0" border="0" height="24"><tr><td><a class="di_gui_link3">'+opts.labels.viewSel+' (<span id="di_ind_count_sel_'+uid+'">0</span>)</a></td></tr></table></a></div>';
		}
		
	    ui += '</td></tr></table></div></div>';

	}
	catch (err){ }
	return ui;
}
/* function to get sector links working here*/
di_drawIndicatorList.prototype.getMenuLinks = function(uid, act) {
	var ui='<div id="di_indlinks_'+uid+'">';
	var uiinner = '';
	var opts = di_indcominputarr[uid];
	var vmode = opts.mode.toLowerCase();

	if(vmode=='both' || vmode=='list')
		ui += '<span rel="list" class="di_gui_link3">'+ opts.labels.byList +'</span> | ';

	if(vmode=='both' || vmode=='tree') {
		di_jq.ajax({
			type: "GET",
			url: opts.codelistPath + "/ic/avl_ic_types.xml",
			dataType: "xml",
			async:false,
			success: function(xml) {
				var types = di_jq(xml).find('avl_ics').text();
				var typesAr = types.split(",");
//				// need to remove CF because for web application we are not givig this option 
				while (typesAr.indexOf('CF') !== -1) {
				    typesAr.splice(typesAr.indexOf('CF'), 1);
				}
				if(opts.currentSector=='') {
					opts.currentSector = typesAr[0];
				}
				//if(opts.sectorMode=='menu') {
				if(typesAr.length > 3) {
					uiinner = '<div id="di_sldiv_'+uid+'" class="di_gui_dd_container"><b class="di_gui_notch"></b><ul class="di_gui_dd">';

					var stextFirst = eval('opts.sectors.' + opts.currentSector);
					//ui += '<span id="di_fsid_'+uid+'" rel="'+opts.currentSector+'" class="di_gui_link3" val="a">'+stextFirst+' <img src="'+di_images_path+'/down-arrow.png" width="10" style="cursor:pointer;"/></span> | ';

					ui += '<span id="di_fsid_'+uid+'" rel="'+opts.currentSector+'" class="di_gui_link3" val="a">'+stextFirst+'</span> <span id="di_fsidimg_'+uid+'" style="padding-left:0px;"><img src="'+di_images_path+'/down-arrow.png" width="10" style="cursor:pointer;"/></span> | ';

					for( var i=0; i<typesAr.length; i++ ) {
						var stext = eval('opts.sectors.' + typesAr[i]);
						uiinner += '<li rel="'+typesAr[i]+'" class="di_gui_link3">'+stext+'</li>';
					}
					uiinner += '</ul></div>';
				}
				else {
					for( var i=0; i<typesAr.length; i++ ) {
						var stext = eval('opts.sectors.' + typesAr[i]);
						ui += '<span rel="'+typesAr[i]+'" class="di_gui_link3">'+ stext +'</span> | ';
					} // end for
				}
			}
		});
	}
	ui += '</div>'+uiinner;

	if(act=='render') {
		di_jq('#di_indlinks_'+uid).append(ui);
	}
	else {
		return ui;
	}
}
/* function to get sector links */
di_drawIndicatorList.prototype.getMenuLinks_old = function(uid, act) {
	var ui='';
	var opts = di_indcominputarr[uid];
	var vmode = opts.mode.toLowerCase();

	if(vmode=='both' || vmode=='list')
		ui = '<span rel="list" class="di_gui_link3">'+ opts.labels.byList +'</span> | ';

	if(vmode=='both' || vmode=='tree') {
		di_jq.ajax({
			type: "GET",
			url: opts.codelistPath + "/ic/avl_ic_types.xml",
			dataType: "xml",
			async:false,
			success: function(xml) {
				var types = di_jq(xml).find('avl_ics').text();
				var typesAr = types.split(",");
				if(opts.currentSector=='') {
					opts.currentSector = typesAr[0];
				}
				for( var i=0; i<typesAr.length; i++ ) {
					var stext = eval('opts.sectors.' + typesAr[i]);
					ui += '<span rel="'+typesAr[i]+'" class="di_gui_link3">'+ stext +'</span> | ';
				} // end for
			}
		});
	}

	if(act=='render') {
		di_jq('#di_indlinks_'+uid).append(ui);
	}
	else {
		return ui;
	}
}
/* function to handle special chars */
di_drawIndicatorList.prototype.handleSpecialChars = function(stringData,act) {
	var repdquote = '&quot;';
	if(act=='dq') { // double quotes
		stringData = stringData.replace(/\"/g,repdquote);
	}
	else if(act=='backs') { // back slash
		stringData = stringData.replace(/[/\\*]/g, "");
	}
	else { // will add to handle all special chars
		stringData = stringData;
	}
	return stringData;
}
/* function to get selected indicator */
di_drawIndicatorList.prototype.getSelectedData = function() {
	var uid =this.uid;
	var opts = di_indcominputarr[uid];
	var di_tmp_all_isunid = new Array();
	
	// preparing JSON data 
	var di_json_data = '{';
		// for iu
		di_json_data += '"iu":[';
		
		for(var i=0; i<opts.selectedInds.length; i++) { 
			var IUText_a = this.handleSpecialChars(opts.selectedInds[i],'dq');
			
			var IUText = IUText_a.split("||");
			di_json_data += '"'+IUText[0]+'~'+IUText[1]+'||'+IUText[2]+'~'+IUText[3]+'"';
			if(i < opts.selectedInds.length-1) {
				di_json_data += ",";
			}

			// start for default iusnid case
			var di_iu = IUText[0]+'_'+IUText[1];
			if(di_safevalue(opts.selectedIus[di_iu])=='' || opts.selectedIus[di_iu].length==0) { 
				di_jq.ajax({
					type: "GET", 
					url: opts.codelistPath + "/ius/ius_"+di_iu+".xml",
					dataType: "xml",
					async:false,
					success: function(xml) {
						var di_iu_brk = di_iu.split("_");
						if(di_iu_brk[0]!='') {
							di_jq(xml).find('iu[inid="'+di_iu_brk[0]+'"]').each(function(){
								var di_defisunid = di_jq(this).attr("defiusnid");
								if(di_defisunid!='' && di_defisunid!=null) {
									di_tmp_all_isunid.push(di_defisunid);
								}
								else {
									di_jq(xml).find('ius').each(function(){ 
										di_tmp_all_isunid.push( di_jq(this).attr("iusnid") );
									});
								}
							});
					    } // end if
					}
				});
			}
			// end for default iusnid case
		}
		di_json_data += '],';
  
		// for dimension
		di_json_data += '"sg_dim":{';

		for(var i=0; i<opts.selectedInds.length; i++) {
			var IUText_a = this.handleSpecialChars(opts.selectedInds[i],'dq');

			var IUText = IUText_a.split("||");
			var di_iu = IUText[0]+"_"+IUText[1];
			if(di_safevalue(opts.selectedDims[di_iu])!='') {
				
				di_json_data += '"iu_'+ di_iu +'" : [';
				
				for(var j=0; j<opts.selectedDims[di_iu].length; j++) {
					di_json_data += '"'+ opts.selectedDims[di_iu][j] +'"';
					if(j < opts.selectedDims[di_iu].length-1) {
						di_json_data += ",";
					}
				}
				di_json_data += '],';

				//if(i!=0 && i < opts.selectedInds.length-1) {
					//di_json_data += ',';
				//}
			}
		}
		// remove last comma
		var lastChar = di_json_data.charAt(di_json_data.length-1);
		if(lastChar==',') di_json_data = di_json_data.slice(0, -1);
		//di_json_data = di_json_data.substring(0, di_json_data.length - 1);

		di_json_data += '},';

		// for dimension value
		di_json_data += '"sg_dim_val":{';

		/*// not in use now
		for(var i=0; i<opts.selectedInds.length; i++) {
			var IUText = opts.selectedInds[i].split("||");
			var di_iu = IUText[0]+"_"+IUText[1];
			if(di_safevalue(opts.selectedDimVals[di_iu])!='') {
				
				di_json_data += '"iu_'+ di_iu +'" : [';
				
				for(var j=0; j<opts.selectedDimVals[di_iu].length; j++) {
					di_json_data += '"'+ opts.selectedDimVals[di_iu][j] +'"';
					if(j < opts.selectedDimVals[di_iu].length-1) {
						di_json_data += ",";
					}
				}
				di_json_data += ']';

				if(i < opts.selectedInds.length-1) {
					di_json_data += ',';
				}
			}
		}*/

		di_json_data += '},';

		// for iusnid 
		di_json_data += '"iusnid":{';
		
		for(var i=0; i<opts.selectedInds.length; i++) {
			var IUText_a = this.handleSpecialChars(opts.selectedInds[i],'dq');

			var IUText = IUText_a.split("||");
			var di_iu = IUText[0]+"_"+IUText[1];
			if(di_safevalue(opts.selectedIus[di_iu])!='') {

				di_json_data += '"iu_'+ di_iu +'" : [';
				
				for(var j=0; j<opts.selectedIus[di_iu].length; j++) {
					
					var IUSINdText = opts.selectedIus[di_iu][j].split("||");
					di_json_data += '"'+ opts.selectedIus[di_iu][j] +'"';
					if(j < opts.selectedIus[di_iu].length-1) {
						di_json_data += ",";
					}
					di_tmp_all_isunid.push(IUSINdText[0]);
				}
				di_json_data += '],';

				//if(i < opts.selectedInds.length-1) { 
					//var IUText = opts.selectedInds[i].split("||");
					//var di_iu2 = IUText[0]+"_"+IUText[1];
					//if(di_safevalue(opts.selectedIus[di_iu2])!='' && opts.selectedIus[di_iu2].length>0)
					//di_json_data += ',';
				//}
			}
		}

		// remove last comma
		var lastChar = di_json_data.charAt(di_json_data.length-1);
		if(lastChar==',') di_json_data = di_json_data.slice(0, -1);
		//di_json_data = di_json_data.substring(0, di_json_data.length - 1);

		di_json_data += '}';

	//}
	di_json_data += '}'; 

	di_json_data = this.handleSpecialChars(di_json_data,'backs');

	return di_tmp_all_isunid + '||{~~}||' + di_json_data;
}




/* internal class function to create an object for internal calling */
function di_indInitObject(uid) {
	this.uid = uid;
	this.delm = "[~]";
	this.pdelm = "||";
	this.tdelm = "~";
	//this.opts = di_indcominputarr[uid];
}
/* function initiate mode and get data accordingly */
di_indInitObject.prototype.di_indInitiateView = function(uid, method) { 
	var ui = '';
	var dt = di_indcominputarr[uid]; //this.opts;
	this.di_loading(uid, true); // loader on

	if(dt.codelistPath!='') {
		di_jq('#di_sldiv_'+uid).hide();
		if(dt.currentMode=='list') { // list view
			di_jq('#di_fsid_'+uid).attr('val', 'd');
			ui = this.di_getListView(uid);
		}
		else if(dt.currentMode=='tree') { // tree view
			di_jq('#di_fsid_'+uid).attr('val', 'a');
			ui = this.di_getTreeView(uid);
		}
	}
	else {
		ui = '<li>Codelist url is null.</li>';
	}
	
	this.di_loading(uid, false); // loader off

	if(method=='render') {
		// render the ui
		di_jq('#di_ind_tree1_'+uid).html(ui);

		// calling binding function
		if(dt.currentMode=='list') {
			this.di_IUSBindingEvent(uid, 'di_ind_tree1_'+uid);
		}
		else if(dt.currentMode=='tree') {
			this.di_ICBindingEvent(uid, 'di_ind_tree1_'+uid);
		}
	}
	else {
		return ui;
	}
}
/* function to bind an event for click label/span/checkbox etc */
di_indInitObject.prototype.di_ICBindingEvent = function(uid, id) {
	var delm = this.delm;
	var opts = di_indcominputarr[uid]; //this.opts;

	// for li label click
	di_jq('#'+id+' li label').bind('click', function(evt){
		var ic_nid = di_jq(this).parent().attr('rel');
		var isexist = di_jq(this).parent().find('ul').length;
		var li_id = di_jq(this).parent().attr('id');
		var ltype = di_jq(this).attr('rel'); // for label: ic, span: ic
		
		if(isexist==0 && ltype=='ic') { // not has child, need to add children
			opts.indObj.di_loadData(uid, ic_nid);
			//di_jq('#'+li_id).append(subui);
			//opts.indObj.di_ICBindingEvent(uid, li_id);
		}
	});
	// li click span
	di_jq('#'+id+' li span').bind('click', function(evt){
		var ic_nid = di_jq(this).parent().attr('rel');
		var isexist = di_jq(this).parent().find('ul').length;
		var li_id = di_jq(this).parent().attr('id');
		var ltype = di_jq(this).attr('rel'); // for label: ic, span: ic
		
		if(isexist==0 && ltype=='ic') { // not has child, need to add children
			opts.indObj.di_loadData(uid, ic_nid);
			//di_jq('#'+li_id).append(subui);
			//opts.indObj.di_ICBindingEvent(uid, li_id);
		}
	});
}
/* function to bind an event for click label/span/checkbox etc */
di_indInitObject.prototype.di_IUSBindingEvent = function(uid, id) {
	// for show/hide metadata
	var delm = this.pdelm;
	var dt = di_indcominputarr[uid];//this.opts;
	di_jq('#'+id+' li').bind('mouseenter mouseleave', function(evt){ 
		if(dt.metadataPath!='') {
			//var id = di_jq(this).attr('id');
			if (evt.type == 'mouseenter') {
				di_jq(this).find('label:eq(1)').show();
			} else if (evt.type == 'mouseleave') {
				di_jq(this).find('label:eq(1)').hide();
			}
		}
	});
	// for li label click
	di_jq('#'+id+' li label').bind('click', function(evt){ 
		var ic_nid = di_jq(this).parent().attr('rel');
		var isexist = di_jq(this).parent().find('ul').length;
		var li_id = di_jq(this).parent().attr('id');
		var ltype = di_jq(this).attr('rel'); // for label: ic, span: ic

		if(isexist==0 && ltype=='iu') { // not has child, need to add children
			var defsg = di_jq(this).parent().attr('val');
			// calling subgroup selection function to show list
			dt.indObj.di_drawSubgroupList(uid, ic_nid, defsg);
		}
		else if(ltype=='miu') {
			var iuname = di_jq(this).parent().find('label:eq(0)').html();
			dt.indObj.di_showMetadataPOP(uid, ic_nid.split('||')[0], iuname);
		}
		// if label clicked for single selection 
		if(ltype=='iu' && dt.multiple==false) {
			// for single selection
			// coming soon
		}
	});
	// for change label bgcolor
	di_jq('#'+id+' li label').bind('mouseenter mouseleave', function(evt){
		var type = di_jq(this).attr('rel');
		if(type=='iu') {
			if (evt.type == 'mouseenter') {
				//di_jq('#'+this.id+ ' > label:eq(0)').addClass('di_gui_label_hover');
				di_jq(this).addClass('di_gui_label_hover');
			} else if (evt.type == 'mouseleave') {
				di_jq(this).removeClass('di_gui_label_hover');
			}
		}
	});
	// li click span
	di_jq('#'+id+' li span').bind('click', function(evt){ 
		var iu_detail = di_jq(this).parent().attr('rel'); // iu detail inid,unid,iname,uname
		var isexist = di_jq(this).parent().find('ul').length;
		var li_id = di_jq(this).parent().attr('id');
		var ltype = di_jq(this).attr('rel'); // for label: ic, span: ic
		
		if(isexist==0 && ltype=='iu') { // not has child, need to add children
			var defsg = di_jq(this).parent().attr('val');
			// calling subgroup selection function to show list
			dt.indObj.di_drawSubgroupList(uid, iu_detail, defsg);
		}
	});
	// for li checkbox click
	di_jq('#'+id+' li input[type="checkbox"]').bind('click', function(evt){ 
		var liDetail = di_jq(this).parent().attr('rel');
		var defiusdet = di_jq(this).parent().attr('defius');
		var checked = this.checked;
		if(liDetail=='all') { // for select iu all
			di_jq(this).closest('ul').children('li').each(function() { 
				var iuval = di_safevalue(di_jq(this).attr('rel'));  
				var defiusdet = di_safevalue(di_jq(this).attr('defius'));
				
				if(iuval!='' && iuval!='all') {
					var defsg = di_jq(this).attr('val');
					var childobj = di_jq(this).children('input[type="checkbox"]');
					if(childobj.attr('disabled')==false) {
						if(checked==true) { 
							childobj.attr('checked', true);
							// insert iu
							dt.indObj.di_manageSelection(uid, true, iuval, 'ind'); 
							// inser ius for above iu commented 19Jun12
							//if(di_safevalue(defiusdet)!='') {
								//var iudt = iuval.split(delm);
								//dt.indObj.di_manageSelection(uid, true, iudt[0]+delm+iudt[1]+delm+defiusdet, 'ius');
							//}
							// added 19Jun12 check if sg list available for this iu
							if(di_jq(this).find('ul').length>0) {
								var iudt = iuval.split(delm);
								var sgtype = di_jq(this).find('ul').attr('rel');
								var npatern = '#sg_list_'+iudt[0]+'_'+iudt[1]+'_'+uid+' div input[type="checkbox"]';
								if(sgtype=='iusg') { // for default sg 
									npatern = '#iu_'+iudt[0]+'_'+iudt[1]+'_'+uid+' li input[type="checkbox"]';
								}
								di_jq(npatern).each(function(){
									var relv = di_jq(this).attr('rel');
									if(relv!=undefined && relv==defsg) {
										di_jq(this).attr('checked', true);
										iusval = di_jq(this).parent().attr('rel'); 
									}
								});
								dt.indObj.di_manageSelection(uid, true, iusval, 'ius');
							} 
							else if(di_safevalue(defiusdet)!='') {
								var iudt = iuval.split(delm);
								dt.indObj.di_manageSelection(uid, true, iudt[0]+delm+iudt[1]+delm+defiusdet, 'ius');
							}//19Jun12
						}
						else {
							//childobj.attr('checked', false); // commented 19Jun12
							di_jq(this).find('input[type="checkbox"]').attr('checked', false);  // added 19Jun12
							dt.indObj.di_manageSelection(uid, false, iuval, 'ind');
						}
					} // end if
					else {
						childobj.attr('checked', false);
					}
				}
			});
		}
		else { // individual selection
			if(checked==true) {
				dt.indObj.di_manageSelection(uid, true, liDetail, 'ind');

				// if already sg panel exist
				if(di_jq(this).parent().find('ul').length>0) {
					var sgtype = di_jq(this).parent().find('ul').attr('rel');
					var iudt = liDetail.split(delm);
					var defsg = di_jq(this).parent().attr('val');
					var iusval = '';
					var npatern = '#sg_list_'+iudt[0]+'_'+iudt[1]+'_'+uid+' div input[type="checkbox"]';
					
					if(sgtype=='iusg') { // for default sg 
						npatern = '#iu_'+iudt[0]+'_'+iudt[1]+'_'+uid+' li input[type="checkbox"]';
					}
					di_jq(npatern).each(function(){
						var relv = di_jq(this).attr('rel');
						if(relv==defsg) {
							di_jq(this).attr('checked', true);
							iusval = di_jq(this).parent().attr('rel'); 
						}
					});
					dt.indObj.di_manageSelection(uid, true, iusval, 'ius');
				}
				else if(di_safevalue(defiusdet)!=''){ // if sg panel does not exist
					var iudt = liDetail.split(delm);
					dt.indObj.di_manageSelection(uid, true, iudt[0]+delm+iudt[1]+delm+defiusdet, 'ius');
				}
			}
			else {
				dt.indObj.di_manageSelection(uid, false, liDetail, 'ind');
				if(di_jq(this).parent().find('ul').length>0) {
					var sgtype = di_jq(this).parent().find('ul').attr('rel');
					if(sgtype=='sg') {
						var iudt = liDetail.split(delm);
						//di_jq(this).parent().find('input[type="checkbox"]').attr('checked', false);
						dt.indObj.getSgData(uid, iudt[0], iudt[1], 'list', '');
					}
					else { // for iusg type
						di_jq(this).parent().find('li input[type="checkbox"]').attr('checked', false);
					}
				}
			}
		}
	});
}

/* function to get list view data */
di_indInitObject.prototype.di_getListView = function(uid) {
	var ui = '';
	var dt = di_indcominputarr[uid]; //this.opts;

	// for keyword
	var filename = dt.codelistPath + "/ius/_iu_.xml";
	var keyword = '';

	if(di_safevalue(di_jq('#ind_search_'+uid).val())!='' && di_jq('#ind_search_'+uid).val()!=dt.labels.search) {
		keyword = di_jq('#ind_search_'+uid).val();
		var keyFirstChar = keyword.charAt(0).toLowerCase();
		filename = dt.codelistPath + '/ius/iussearch/' + keyFirstChar +'.xml';
	}
	
	ui = this.di_getListData(uid, filename, keyword);
	return ui;
}
/* function to get search result */
di_indInitObject.prototype.di_getSearch = function(uid, keyword) {
	var dt = di_indcominputarr[uid]; //this.opts;
	if(keyword!='' && keyword!=dt.labels.search) {
		if(keyword.length > 0) {
			dt.currentMode = 'list';
			// change the link visiblity
			dt.indObj.di_setLinksVisiblity(uid);
			// render indicator content
			dt.indObj.di_indInitiateView(uid, 'render');
		}
	}
	else if(keyword=='') {
		dt.currentMode = dt.storePreMode;
		dt.indObj.di_setLinksVisiblity(uid);
		dt.indObj.di_indInitiateView(uid, 'render');
	}
}
/* function to check selection if checked where data exist */
di_indInitObject.prototype.di_checkIUSelectionWDE = function(uid, input_iu_nids, result_nids) {
	var delm = this.pdelm;
	var dt = di_indcominputarr[uid];//this.opts;
	var input_l = input_iu_nids.length;
	var output_l = result_nids.length;
	if(input_l > 0) {
		//alert(input_l + ' - ' + output_l);
		if(output_l==0) {
			// remove all selection
			dt.indObj.di_manageSelection(uid, false, '', 'clear');
		}
		else { 
			for(var i=0; i<input_l; i++) {
				var iuid = input_iu_nids[i];
				if(jQuery.inArray(iuid, result_nids) == '-1') {
					var arr = iuid.split("_");
					dt.indObj.di_manageSelection(uid, false, arr[0]+delm+arr[1], 'ind'); 
				}
			} // end for
		}
	} // end if
}
/* function to get list/search view data */
di_indInitObject.prototype.di_getListData = function(uid, filename, keyword) {
	var ui = '';
	var dt = di_indcominputarr[uid]; //this.opts;
	var delm = this.pdelm;
	var disabledCheckbox ='disabled="true"';
	var checkedCheckbox ='checked="true"';
	var isDataExistFunc = dt.callbacks.whereDataExist;
	
	di_jq.ajax({
		type: "GET",
		url: filename,
		dataType: "xml",
		async:false,
		timeout: 5000,
		error: function(request, error) { 
			if (error == "timeout") 
				alert('The request timed out, File size is so high'); 
			else { 
				//alert('error: ' + request.status ); 
			} 
		},
		success: function(xml) {
			var tempArr = new Array();
			var s_patern = 'iu';
			if(keyword!='' && keyword!=null) {
				s_patern = 'iu[t="IND"]'; // for search only in indicator
			}

			var de_type = 1; // for nuteral
			// code for where data exist
			var result_nids = '';
			if(dt.isDataExist=='T') { 
				var input_iu_nids = []; // input i_u nids for get data
				di_jq(xml).find(s_patern).each(function(){
					input_iu_nids.push( di_jq(this).attr("inid") + '_' + di_jq(this).attr("unid") );
				});
				var funcResult = eval(isDataExistFunc)(input_iu_nids); 
				if(funcResult=='falseWOArea') de_type = 2; // for woithout area (would be checked flag in xml file)
				else if(funcResult=='false') de_type = 3; // check with return nids
				else {
					result_nids = funcResult.split(",");
					de_type = 3; // check with return nids

					// code to deselect id is not in result set if already selected
					dt.indObj.di_checkIUSelectionWDE(uid, input_iu_nids, result_nids);
				}
			} // end wde
			if(dt.selectAll==true && dt.multiple==true)
				ui += '<li rel="all" class="di_gui_lists_specialoption"><input type="checkbox" value="">&nbsp;'+ dt.labels.selectAll +'</li>';

			di_jq(xml).find(s_patern).each(function(){
				var iu_output = '';
				var disabledStatus = '0';
				var defsg = '';

				var i_name = di_strEscapeSpclchar(di_jq(this).attr("in"));
				var i_nid = di_jq(this).attr("inid");
				var u_name = di_strEscapeSpclchar(di_jq(this).attr("un"));
				var u_nid = di_jq(this).attr("unid");
				var ds_name = di_strEscapeSpclchar(di_jq(this).attr("defsgn"));
				var ds_nid = di_jq(this).attr("defsgnid");
				var dataexists = di_jq(this).attr("dataexists");
				var defiusnid = di_safevalue(di_jq(this).attr("defiusnid"));
				var defiust = '';
				if(defiusnid!='') defiust = defiusnid+delm+ds_name;
				
				if(de_type==2){ // check for where data esixt in xml file
					if(dataexists=='T') disabledStatus='0';
					else disabledStatus = '1';
				}
				else if(de_type==3) { // check in return ids
					if(result_nids.length>0) {
						if(di_jq.inArray( i_nid+'_'+u_nid , result_nids) < 0) disabledStatus = '1';
						else disabledStatus = '0';
					}
					else {
						disabledStatus = '1';
					}
				} //

				var containsFoo = i_name.toLowerCase().indexOf(keyword.toLowerCase()) >= 0;
				//if(containsFoo==true) {
				if(disabledStatus=='0' && containsFoo==true) {

					var liclass = 'leaf';
					var lispan = '<span rel="iu"></span>';
					var onclickfunc = '';
						
					if(dt.sgSelection==true) {
						liclass = 'collapsed';
						lispan = '<span class="ui-icon ui-icon-triangle-1-e" rel="iu"></span>';

						if(di_safevalue(ds_nid)!='') defsg = ds_nid;
					}

					ui += '<li rel="'+i_nid+ delm +u_nid+ delm + i_name + delm + u_name +'" id="iu_'+i_nid+'_'+u_nid+'_'+uid+'" class="'+liclass+'" val="' + defsg + '" defius="'+defiust+'">' + lispan;

					// code for define output format
					if(dt.outputFormat=='nid')
						iu_output = i_nid + '~' + u_nid;
					else if(dt.outputFormat=='name')
						iu_output = i_name + '~' + u_name;
					else if(dt.outputFormat=='nid_name')
						iu_output = i_nid +'_'+ i_name + '~' + u_nid +'_'+ u_name;
					iu_output = di_strEscapeSpclchar(iu_output);

					// if multiple selection
					if(dt.multiple==true) {
						ui += '<input type="checkbox" value="'+iu_output+'" ';
						
						// code to check for disbaled
						var labelDisabledClass = '';
						//if(disabledStatus=='1') {
							//ui += disabledCheckbox;
							//labelDisabledClass = 'di_gui_deactive_label';
						//}
							
						// code to check pre selected
						if(dt.indObj.di_checkDuplicate(dt.selectedInds, i_nid+ delm +u_nid+ delm +i_name+ delm +u_name)==1 && disabledStatus==0) ui += ' ' + checkedCheckbox;
							
						ui += '>';
					}//
					
					ui += '<label class="di_gui_leafnode_label '+labelDisabledClass+'" rel="iu">' + i_name + ', ' + u_name + '</label>';

					// for metadata
					if(dt.showMetadata==true)
						ui += ' &nbsp;<label class="di_gui_link2" style="display:none;" rel="miu">' + dt.labels.metadata + '</label>';

					ui += '</li>'; // end li

				}
			});
		}
	});
	return ui;
}
/* function to get tree view data */
di_indInitObject.prototype.di_getTreeView = function(uid) {
	var ui = '';
	var dt = di_indcominputarr[uid]; //this.opts;
	var filename = dt.codelistPath + "/ic/"+ dt.currentSector.toLowerCase() +".xml";
	
	if(dt.collapsed == true) 
		filename = dt.codelistPath + "/ic/"+ dt.currentSector.toLowerCase() +"_l1.xml";
	
	di_jq.ajax({ 
		type: "GET",
		url: filename,
		dataType: "xml",
		async:false,
		success: function(xml) {
			var tempArr = new Array();
			di_jq(xml).find('c').each(function(){ 
				var name = di_strEscapeSpclchar(di_jq(this).attr("n"));
				var nid = di_jq(this).attr("nid");
				var isglobal = di_jq(this).attr("isG");
				if(jQuery.inArray(nid, tempArr)=='-1') {
					tempArr.push(nid);
					var colclass = 'expanded';
					var spclass = 'ui-icon ui-icon-triangle-1-se';
					var object = di_jq(this);
					var childlength = object.find('c').length;
					
					if(childlength==0) { 
						colclass = 'collapsed';
						spclass = 'ui-icon ui-icon-triangle-1-e';
					}
					
					ui += '<li id="ic_'+nid+'_'+uid+'" class="'+colclass+'" rel="'+nid+'"><span class="'+spclass+'" rel="ic"></span>';
					ui += '<label class="di_gui_tree_label" rel="ic">'+name+'</label>';

					// aclling recursive function 
					if(childlength>0)
					ui += dt.indObj.di_getChildIC(uid, object, tempArr); 

					ui += '</li>';
				}
			});
		}
	});

	return ui;
}
/* function to get recursive tree view data */
di_indInitObject.prototype.di_getChildIC = function(uid, object, tempArr) { 
	var ui='';
	var dt = di_indcominputarr[uid]; //this.opts;
	
	if(object.find('c').length>0)
	{ 
		// if need to collapsed node after 2 level
		if(dt.collapsed == true) ui += '<ul style="display:none;">';
		else ui += '<ul>';

		object.find('c').each(function(){ 
			var name = di_strEscapeSpclchar(di_jq(this).attr("n"));
			var nid = di_jq(this).attr("nid");
			var isglobal = di_jq(this).attr("isG");

			if(jQuery.inArray(nid, tempArr)=='-1') {
				tempArr.push(nid);
				var colclass = 'collapsed';
				var spclass = 'ui-icon ui-icon-triangle-1-e';
				var object2 = di_jq(this);
				var childlength = object2.find('c').length;
				if(childlength>0 && dt.collapsed == false) { 
					colclass = 'expanded';
					spclass = 'ui-icon ui-icon-triangle-1-se';
				}
				ui += '<li id="ic_'+nid+'_'+uid+'" class="'+colclass+' di_gui_label_indent" rel="'+nid+'"><span class="'+spclass+'" rel="ic"></span>';
				ui += '<label class="di_gui_tree_label" rel="ic">'+name+'</label>';
				// calling recusive function
				if(childlength>0)
				ui += dt.indObj.di_getChildIC(uid, object2, tempArr);

				ui += '</li>';
			}
		
		});
		ui += '</ul>';
	} // end if
	return ui;
}
/* function to load child data after clicking on parent IC */
di_indInitObject.prototype.di_loadData = function(uid, ic_nid) { 
	if(ic_nid=='') return false;
	var disabledCheckbox ='disabled="true"';
	var checkedCheckbox ='checked="true"';
	var dt = di_indcominputarr[uid]; //this.opts;
	var delm = this.pdelm;
	var isDataExistFunc = dt.callbacks.whereDataExist;
	
	di_jq.ajax({ 
		url: dt.codelistPath + '/ic/'+ dt.currentSector.toLowerCase() +'_l1_' + ic_nid + '.xml',
		type:'GET',
		dataType: "xml",
		async:false,
		error: function()
		{
			// ic does not exist, load indicator list here
			di_jq.ajax({
				type: "GET",
				url: dt.codelistPath + '/ic_ius/iu_' + ic_nid + '.xml',
				dataType: "xml",
				async:false,
				success: function(xml) {
					var ui = '<ul>';
					var s_patern = 'iu';
					var de_type = 1; // for nuteral
					// code for where data exist
					var result_nids = '';
					if(dt.isDataExist=='T') { 
						var input_iu_nids = []; // input i_u nids for get data
						di_jq(xml).find('iu').each(function(){
							input_iu_nids.push( di_jq(this).attr("inid") + '_' + di_jq(this).attr("unid") );
						});
						var funcResult = eval(isDataExistFunc)(input_iu_nids); 
						if(funcResult=='falseWOArea') de_type = 2; // for woithout area (would be checked flag in xml file)
						else if(funcResult=='false') de_type = 3; // check with return nids
						else {
							result_nids = funcResult.split(",");
							de_type = 3; // check with return nids
						}
					}
					
					if(dt.selectAll==true && dt.multiple==true)
						ui += '<li rel="all" class="di_gui_lists_specialoption di_gui_label_indent"><input type="checkbox" value="">&nbsp;'+ dt.labels.selectAll +'</li>';

					di_jq(xml).find(s_patern).each(function(){
						var iu_output = '';
						var disabledStatus = '0';
						var defsg = '';

						var i_name = di_jq(this).attr("in"); // indicator name
						var i_nid = di_jq(this).attr("inid"); // indiactor nid
						var u_name = di_jq(this).attr("un"); // unit name
						var u_nid = di_jq(this).attr("unid"); // unit nid
						var ds_name = di_jq(this).attr("defsgn"); // default subgroups name
						var ds_nid = di_jq(this).attr("defsgnid"); // default subgroups nid
						var dataexists = di_jq(this).attr("dataexists"); // data exist
						var defsgnid = di_jq(this).attr("defsgnid"); // default subgroup nid
						var defiusnid = di_safevalue(di_jq(this).attr("defiusnid")); // default ius nid
						var defiust = '';
						if(defiusnid!='') defiust = defiusnid+delm+ds_name;

						if(de_type==2){ // check for where data esixt in xml file
							if(dataexists=='T') disabledStatus='0';
							else disabledStatus = '1';
						}
						else if(de_type==3) { // check in return ids
							if(result_nids.length>0) {
								if(di_jq.inArray( i_nid+'_'+u_nid , result_nids) < 0) disabledStatus = '1';
								else disabledStatus = '0';
							}
							else {
								disabledStatus = '1';
							}
						}

						if(disabledStatus=='0') {

						var liclass = 'leaf';
						var lispan = '<span rel="iu"></span>';
						var onclickfunc = '';
						
						if(dt.sgSelection==true) {
							liclass = 'collapsed';
							lispan = '<span class="ui-icon ui-icon-triangle-1-e" rel="iu"></span>';

							if(di_safevalue(ds_nid)!='') defsg = ds_nid;
						}

						ui += '<li rel="'+i_nid+ delm +u_nid+ delm + i_name + delm + u_name +'" id="iu_'+i_nid+'_'+u_nid+'_'+uid+'" class="'+liclass+' di_gui_label_indent" val="' + defsg + '" defius="'+defiust+'">' + lispan;

						// code for define output format
						if(dt.outputFormat=='nid')
							iu_output = i_nid + '~' + u_nid;
						else if(dt.outputFormat=='name')
							iu_output = i_name + '~' + u_name;
						else if(dt.outputFormat=='nid_name')
							iu_output = i_nid +'_'+ i_name + '~' + u_nid +'_'+ u_name;
						iu_output = di_strEscapeSpclchar(iu_output);

						// if multiple selection
						if(dt.multiple==true) {
							ui += '<input type="checkbox" value="'+iu_output+'" ';
							
							// code to check for disbaled
							var labelDisabledClass = '';
							if(disabledStatus=='1') {
								ui += disabledCheckbox;
								labelDisabledClass = 'di_gui_deactive_label';
							}
							
							// code to check pre selected
							if(dt.indObj.di_checkDuplicate(dt.selectedInds, i_nid+ delm +u_nid+ delm +i_name+ delm +u_name)==1 && disabledStatus==0) ui += ' ' + checkedCheckbox;
							
							ui += '>';
						}

						ui += '<label class="di_gui_leafnode_label '+labelDisabledClass+'" rel="iu">' + i_name + ', ' + u_name + '</label>';

						// for metadata
						if(dt.showMetadata==true)
							ui += ' &nbsp;<label class="di_gui_link2" style="display:none;" rel="miu">' + dt.labels.metadata + '</label>';

						ui += '</li>'; // end li

						} // end if for disabled

					});

					ui += '</ul>';

					di_jq("#ic_"+ic_nid+'_'+uid).append(ui);
					// binding event for ius
					dt.indObj.di_IUSBindingEvent(uid, 'ic_'+ic_nid+'_'+uid);
				}
			});
		},
		success: function(xml)
		{
			//ic file exists, load ic child
			var object = di_jq(xml);
			var tempArr = new Array();
			var ui = dt.indObj.di_getChildIC(uid, object, tempArr);
			di_jq('#ic_'+ic_nid+'_'+uid).append(ui);
			dt.indObj.di_ICBindingEvent(uid, 'ic_'+ic_nid+'_'+uid);
		} // end success
	
	});
}
/* function to store json input data for persisiting */
di_indInitObject.prototype.storeJsonSelection = function(uid, jsonString) {
	try
	{
		var delm = this.pdelm;
		var delm2 = this.tdelm;
		var dt = di_indcominputarr[uid]; //this.opts;
		dt.selectedInds = [];
		dt.selectedDims = [];
		dt.selectedDimVals = [];
		dt.selectedIus = [];
		
		if(jsonString!='') {
			var obj = di_jq.parseJSON(jsonString);
			var indObj = obj.iu; // indicator object
			var dimObj = obj.sg_dim; // subgroup dimention object
			var dimValObj = obj.sg_dim_val; // subgroup dimention value object
			var iusObj = obj.iusnid; // ius object
			
			// manage iu selcetion
			if(di_safevalue(indObj)!='' && indObj.length>0) {
				for(var i=0; i<indObj.length; i++) { 
					if(di_safevalue(indObj[i])!='') {
						var IUText = indObj[i].split(delm);
						var IUNIdsBr = IUText[0].split(delm2); // i,u nids
						var IUTextBr = IUText[1].split(delm2); // i,u text

						var mtext = IUNIdsBr[0]+ delm +IUNIdsBr[1]+ delm +IUTextBr[0]+ delm +IUTextBr[1];
						//dt.selectedInds.push(mtext);
						this.di_manageSelection(uid, true, mtext, 'ind');
					}
				}
			}
			// manage sg dimention selcetion
			if(di_safevalue(dimObj)!='') {
				for(var i in dimObj) {
					var indexv = i.replace("iu_", "");
					dt.selectedDims[indexv] = dimObj[i];
				}
			} 

			// manage sg dimention values selcetion
			if(di_safevalue(dimValObj)!='') {
				for(var i in dimValObj) {
					var indexval = i.replace("iu_", ""); 
					dt.selectedDimVals[indexval] = dimValObj[i]; 
				} 
			}

			// manage ius selcetion
			if(di_safevalue(iusObj)!='') { 
				for(var i in iusObj) {
					var indexiusnid = i.replace("iu_", "");
					dt.selectedIus[indexiusnid] = iusObj[i];

					/*var temp_store_val = [];
					var temp_store_text = [];
					for(var a=0; a<iusObj[i].length; a++) {
						var valueTxt = iusnidobj[i][a].split("||");
						temp_store_val.push(valueTxt[0]);
						temp_store_text.push(iusnidobj[i][a]);
					}
					di_store_selected_iusnid[indexiusnid] = temp_store_val;
					di_store_selected_iusnid_txt[indexiusnid] = temp_store_text;*/
				}
			}
		} 
	}
	catch (err){alert('Json format is not valid.');}
}
/* function to manage selection of iu/dim/dimv/ius */
di_indInitObject.prototype.di_manageSelection = function(uid, status, chkval, type) {
	try
	{
		var delm = this.pdelm;
		var dt = di_indcominputarr[uid]; //this.opts;
		var chkvalAr = chkval.split(delm);
		switch(type) {
			case "clear":
				dt.selectedInds = [];
				dt.selectedDims = [];
				dt.selectedDimVals = [];
				dt.selectedIus = [];
				di_jq('#di_ind_count_sel_'+uid).html(0);
			break;
			case "ind": // manage indicator selection
				if(status==true) { // insert
					var di_idx = this.di_checkDuplicate(dt.selectedInds, chkval);
					if(di_idx==-1) dt.selectedInds.push(chkval);
				}
				else { // delete
					var idx;
					for(var i=0; i<= dt.selectedInds.length; i++) {
						if(dt.selectedInds[i]==chkval) {
							dt.selectedInds.splice(i, 1);
							break;
						}
						else if(di_safevalue(chkvalAr[2])==''){
							if(dt.selectedInds[i].indexOf(chkvalAr[0]+ delm +chkvalAr[1] + delm)>-1) {
								dt.selectedInds.splice(i, 1);
								break;
							}
						}
					}
					// delete all data for removed iu
					dt.selectedDims[chkvalAr[0]+ '_' +chkvalAr[1]] = [];
					dt.selectedDimVals[chkvalAr[0]+ '_' +chkvalAr[1]] = [];
					dt.selectedIus[chkvalAr[0]+ '_' +chkvalAr[1]] = [];
				}
				di_jq('#di_ind_count_sel_'+uid).html(dt.selectedInds.length);
			break;
			case "dims": // manage dimension selection
				var dimAr = di_safevalue(dt.selectedDims[chkvalAr[0]+'_'+chkvalAr[1]]);
				if(dimAr=='') dimAr = [];

				if(status==true) { // insert
					var di_idx = this.di_checkDuplicate(dimAr, chkvalAr[2]);
					if(di_idx==-1) {
						dimAr.push(chkvalAr[2]);
						dt.selectedDims[chkvalAr[0]+'_'+chkvalAr[1]] = dimAr;
					}
				}
				else { // remove
					if(dimAr!='') {
						var idx;
						for(var i=0; i<dimAr.length; i++) {
							if(dimAr[i]==chkvalAr[2]) {
								dimAr.splice(i, 1);
								if(dimAr.length>0) {
									dt.selectedDims[chkvalAr[0]+'_'+chkvalAr[1]] = dimAr;
								}
								break;
							}
						} // end for
					} // end if
				}
			break;
			case "dimv": // manage dimension values selection
				// not in use
			break;
			case "ius": // manage ius selection
				if(dt.multiple==true) {
					var iusAr = di_safevalue(dt.selectedIus[chkvalAr[0]+ '_' +chkvalAr[1]]);
					if(iusAr=='') iusAr = [];
					if(status==true) { // insert
						var di_idx = this.di_checkDuplicate(iusAr, chkvalAr[2]+ delm +chkvalAr[3]);
						if(di_idx==-1) iusAr.push(chkvalAr[2]+ delm +chkvalAr[3]);
						dt.selectedIus[chkvalAr[0]+ '_' +chkvalAr[1]] = iusAr;
					}
					else { // remove
						if(iusAr!='') {
							var idx;
							for(var i=0; i<iusAr.length; i++) {
								if(iusAr[i]==chkvalAr[2]+ delm +chkvalAr[3]) {
									iusAr.splice(i, 1);
									if(iusAr.length>0) {
										dt.selectedIus[chkvalAr[0]+ '_' +chkvalAr[1]] = iusAr;
									}
									else{
										// delete iu from selectedInds
										this.di_manageSelection(uid, false, chkvalAr[0]+ delm +chkvalAr[1] + delm, 'ind');
									}
									break;
								}
							} // end for
						} // end if
					} // end else
				} // end multipleif 
				else { 
					var indDetail = di_jq('#iu_'+chkvalAr[0]+'_'+chkvalAr[1]+'_'+uid).attr('rel');
					indDetail = indDetail.split('||');
					dt.selectedInds = [];
					dt.selectedInds.push(chkvalAr[0]+ '||' +chkvalAr[1]+'||'+indDetail[2]+'||'+indDetail[3]);
					iusAr = [];
					iusAr.push(chkvalAr[2]+ delm +chkvalAr[3]);
					dt.selectedIus[chkvalAr[0]+ '_' +chkvalAr[1]] = iusAr;
				}
			break;
		} // end switch
		
	}
	catch (err){}
}
/* function to view selection list */
di_indInitObject.prototype.di_viewSelections = function(uid) {
	try
    {  
		var dt = di_indcominputarr[uid]; //this.opts;
	    di_jq('#di_indviewsel_con_'+uid).html('');
		var inds = dt.selectedInds;
		for(var i=0; i<inds.length; i++) {
			var iudata = di_safevalue(inds[i]);
			var iudataAr = iudata.split(this.pdelm); // pipe delemeter
			if(iudataAr.length>0) {
				var i_nid = iudataAr[0];
				var u_nid = iudataAr[1];
				var i_name = iudataAr[2];
				var u_name = iudataAr[3];
				var di_ui = '<div style="padding-top:5px;" rel="'+i_nid+ this.pdelm +u_nid+ this.pdelm + i_name + this.pdelm + u_name+'"><img src="'+di_images_path+'/icon_cross.gif" class="di_gui_cursor_pointer" rel="iu"> &nbsp;'+ i_name +', '+u_name;
				// Start for listing subgroups
				if(di_safevalue(dt.selectedIus[i_nid + '_' + u_nid])!='') { 
					var iusAr = dt.selectedIus[i_nid + '_' + u_nid];

					for(var j=0; j<iusAr.length; j++) {
						var iusnidVTx = iusAr[j].split(this.pdelm);
						var iusnid = iusnidVTx[0];
						var sgtxt = iusnidVTx[1];

						di_ui += '<div style="margin-left:20px;padding:2px;" rel="'+iusAr[j]+'"><img src="'+di_images_path+'/icon_cross.gif" class="di_gui_cursor_pointer" rel="ius"> &nbsp;'+ sgtxt + '</div>';
					} // end for
				} // end if
					
				di_ui += '</div>';

				di_jq('#di_indviewsel_con_'+uid).append(di_ui);
			}
		}
		di_jq('#di_indviewsel_list_'+uid).show("slide", { direction: "right" }, 500);
	}
    catch(err){}
}
/* function to manage selection of iu/dim/dimv/ius */
di_indInitObject.prototype.di_checkDuplicate = function(myarray, val) {
	if(myarray.length<1) {
		return -1;
	}
	for(var i=0; i<myarray.length; i++) {
		if(myarray[i] == val)
			return 1;
	}
	return -1;
}
/* function to show metadata for iu */
di_indInitObject.prototype.di_showMetadataPOP = function(uid, i_nid, i_name) {
	var xml_file;
	var xsl_file;
	var opts = di_indcominputarr[uid]; // this.opts
	try
	{	di_jq('#di_indmtdt_con_'+uid).html('');
	    xml_file = opts.codelistPath + '/metadata/indicator/'+i_nid+'.xml'; 
	    xsl_file = opts.codelistPath + '/metadata/IND.xsl';
	
	    var transform_html = di_xml_xsl_transformation(xml_file, xsl_file);
		
	    //di_compApplyMasking();
		di_jq('#di_indmtdt_view_'+uid).show();	   
		di_jq('#di_indmtdt_title_'+uid).html(i_name);
	    di_jq('#di_indmtdt_con_'+uid).html(transform_html);

		di_jq('#di_indmtdt_con_'+uid).find('img').each(function() {
			var src = $(this).attr("src"); // get the src
			var newSrc = opts.codelistPath + '/metadata/indicator/' + src; // get the path 
			$(this).attr("src", newSrc);
		});
		
	}
	catch(err){}
}
/* function to hide metadata popup */
di_indInitObject.prototype.di_hideMetadataPOP = function(uid)
{
    di_jq('#di_indmtdt_view_'+uid).hide();	  
	di_compRemoveMasking();
}
/* function to show/hide loder */
di_indInitObject.prototype.di_loading = function(uid, action) {
	//if(action==true) di_jq('#di_ind_wheeler_'+uid).show(); else di_jq('#di_ind_wheeler_'+uid).hide();
	if(action==true) di_jq('#di_ind_whmask_'+uid).show(); else di_jq('#di_ind_whmask_'+uid).hide();
}
/* function to change view mode */
di_indInitObject.prototype.di_changeMode = function(uid, mode) { 
	var opts = di_indcominputarr[uid];
	if(mode=='list') {
		opts.currentMode = mode;
		opts.storePreMode = mode;
	}
	else {
		opts.currentSector = mode;
		opts.currentMode = 'tree';
		opts.storePreMode = 'tree';
	}
	//alert(this.opts.currentMode + ' - ' + this.opts.currentSector);
}
/* function to change view mode */
di_indInitObject.prototype.di_setLinksVisiblity = function(uid) {
	var mode = '';
	var opts = di_indcominputarr[uid];

	if(opts.currentMode=='list') mode = 'list';
	else mode = opts.currentSector;
	di_jq('#di_indlinks_'+uid+ ' span').removeAttr('class');
	di_jq('#di_indlinks_'+uid+ ' span').addClass('di_gui_link3');
	
	di_jq('#di_indlinks_'+uid+ ' span').each(function(e) {
		var relmode = di_jq(this).attr('rel');
		if(relmode==mode) {
			di_jq(this).removeClass('di_gui_link3');
			di_jq(this).addClass('di_gui_seld_txt');
		}
	});
}
/**********************************************/
// End Indicator related functions
/**********************************************/



/**********************************************/
// Start subgroup related functions
/**********************************************/

/* function to init subgroup selection list */
di_indInitObject.prototype.di_drawSubgroupList = function(uid, iu_details, defsgnid) {
	var opts = di_indcominputarr[uid];
	var delm = this.pdelm;
	var showsg = false;
	var ui = '';
	var xmlObj = '';

	if(opts.sgSelection==true && iu_details!='') {
		var dar = iu_details.split(this.pdelm);
		var inid = dar[0];	// indicator nid
		var unid = dar[1];	// unit nid
		var inm = dar[2];	// indicator name
		var unm = dar[3];	// unit name
		var isIuChecked = di_jq("#iu_"+inid+'_'+unid+'_'+uid+' > input:checkbox').is(':checked');

		//var sgopts = {suid:uid, width:'', height:'', count:opts.sgLimit};
		//var sgObj = new di_drawSubgroupList('iu_'+inid+'_'+unid+'_'+uid, sgopts);

		ui = '<ul rel="iusg">';
		di_jq.ajax({
			type: "GET",
			url: opts.codelistPath + '/ius/ius_' + inid + '_' + unid + '.xml',
			dataType: "xml",
			async:false,
			success: function(xml) {
				xmlObj = xml;
				var sg_num = di_jq(xml).find('ius').length;
				// if default limit is less/equal
				if(sg_num <= opts.sgLimit) { 
					ui = '<ul rel="iusg">';
					var count=0;
					var totalIusnids = di_jq(xml).find('ius').length;

					if(opts.multiple==true && opts.selectAll==true && totalIusnids>1)
					ui += '<li class="di_gui_lists_specialoption di_gui_label_indent" rel="all"><input type="checkbox" value="">'+ opts.labels.selectAll +'</li>';

					di_jq(xml).find('ius').each(function(){
						if(count <= opts.sgLimit) {
							var sg_name = di_strEscapeSpclchar(di_jq(this).attr("n"));
							var sg_nid = di_jq(this).attr("nid");
							var iusnid = di_jq(this).attr("iusnid");
							var dataexists = di_jq(this).attr("dataexists");

							ui += '<li class="leaf di_gui_label_indent" rel="'+inid+delm+unid+delm+iusnid+delm+sg_name+'">';
							var is_checked = '';

							var presel = di_safevalue(opts.selectedIus[inid+'_'+unid]);

							if(presel!='' && opts.indObj.di_checkDuplicate(presel, iusnid+ delm +sg_name)==1) {
								is_checked =' checked="checked" ';
							}
							else if(presel.length==0 && defsgnid==sg_nid && isIuChecked==true) { ;
								is_checked =' checked="checked" ';
								opts.indObj.di_manageSelection(uid, true, inid+ delm +unid+ delm +iusnid+ delm +sg_name, 'ius');
							} 
							
							if(opts.multiple==true) {
								ui += '<input type="checkbox" value="'+iusnid+'" '+is_checked+' rel="'+sg_nid+'">&nbsp;';
							}

							ui += '<label class="di_gui_leafnode_label">' + sg_name + '</label>';

							ui += '</li>';

							count++;

						} // end count if
					});
					ui += '</ul>';
				}// end sg_num end
				else {
					showsg = true;
					ui = '<ul rel="sg"><li style="margin-left:-30px;">' + opts.indObj.getSgUI(uid, iu_details, defsgnid, xml) + '</li></ul>';
				}
			} // end success
		});
		
		di_jq('#iu_'+inid+'_'+unid+'_'+uid).append(ui);

		if(showsg==true) {
			// binding event for menu
			this.di_SgBindingEvent(uid, 'sgm_'+inid+'_'+unid+'_'+uid, 'menu');
			
			var defmode = 'list';
			var checkdim = di_safevalue(opts.selectedDims[inid+'_'+unid]);
			if(checkdim!='' && opts.selectedDims[inid+'_'+unid].length>0) defmode = 'dim';

			this.getSgData(uid, inid, unid, defmode, xmlObj);
		}
		else { 
			// binding event for the default iusg
			this.di_SgBindingEvent(uid, 'iu_'+inid+'_'+unid+'_'+uid, 'iusg');
		}
	
	} // end if
}
/* function to init subgroup selection ui */
di_indInitObject.prototype.getSgUI = function(uid, iu_details, defsgnid, xml) {
	var opts = di_indcominputarr[uid];
	var delm = this.pdelm;
	var dar = iu_details.split(delm);
	var panheight = opts.height-140;
	var ui = '<div style="width:100%;"><table width="100%" cellpadding="0" cellspacing="0"><tr><td>';

	// subgroup dimensions
	//ui += '<table cellspacing="0" cellpadding="0" id="sgm_'+dar[0]+'_'+dar[1]+'_'+uid+'"><tr><td style="padding-left:5px;" height="25"><div class="di_gui_link3" rel="sglist">'+opts.labels.byList+'</div></td>'; // commented on 14 Sept 12
	ui += '<table cellspacing="0" cellpadding="0" id="sgm_'+dar[0]+'_'+dar[1]+'_'+uid+'"><tr><td><table cellpadding="0" cellspacing="0"><tr><td style="padding-left:5px;" height="25"><div class="di_gui_link3" rel="sglist">'+opts.labels.byList+'</div></td>';

	var totalSGD = di_jq(xml).find('st').length;
	if(totalSGD > 0) {
		var colCount = 0 ; 
		di_jq(xml).find('st').each(function(){
			var sg_dim_name = di_jq(this).attr("n");
			var sg_dim_nid = di_jq(this).attr("nid");
			var objectXml = di_jq(this);
			var totalSGDVals = objectXml.find('stv').length;
			if(totalSGDVals > 0) {
				var is_checked = '';
				if(di_safevalue(opts.selectedDims[dar[0]+'_'+dar[1]])!='' && opts.indObj.di_checkDuplicate(opts.selectedDims[dar[0]+'_'+dar[1]], sg_dim_nid)==1) {
					is_checked = 'checked="checked" ';
				}

				ui += '<td style="padding:0 2px 1px 5px;width:10px;" align="center">|</td><td class="di_gui_seld_txt"><input type="checkbox" value="'+sg_dim_nid+'" '+is_checked+'>&nbsp;'+sg_dim_name+'</td>';

				if(colCount==4) {
					//ui += '</tr><tr>'; // commmented on 14 Sept 12
					ui += '</tr></table></td></tr><tr><td><table cellpadding="0" cellspacing="0"><tr>';
					colCount=0;
				}

				colCount++;
			} // end total
		});
	} // end total

	//ui += '</tr></table></td></tr><tr><td><div id="sg_list_'+dar[0]+'_'+dar[1]+'_'+uid+'" class="di_gui_smallbox_panel" style="width:98%;height:'+panheight+'px;"></div></td></tr></table>'; //commented on 14 Sept 12

	ui += '</table></td></tr></table></td></tr><tr><td><div id="sg_list_'+dar[0]+'_'+dar[1]+'_'+uid+'" class="di_gui_smallbox_panel" style="width:98%;height:'+panheight+'px;"></div></td></tr></table>';

	ui += '</div>';

	return ui;
}
/* function to init subgroup selection list */
di_indInitObject.prototype.getSgData = function(uid, inid, unid, action, xml) {
	if(di_safevalue(action)=='') action = 'list';
	var opts = di_indcominputarr[uid];
	var delm = this.pdelm;
	var ui = '';
	var iu_nids = inid + '_' + unid;
	var xmlObj = '';
	var isIuChecked = di_jq("#iu_"+inid+'_'+unid+'_'+uid+' > input:checkbox').is(':checked');
	
	// read xml file if xml data is null
	if(di_safevalue(xml)=='') {
		di_jq.ajax({
			type: 'GET',
			url: opts.codelistPath + '/ius/ius_'+ iu_nids +'.xml',
			dataType:"xml",
			async:false,
			success: function(xml){
				xmlObj = xml;
			}
		});
	}
	else {
		xmlObj = xml;
	}
	// empty the container
	di_jq('#sg_list_'+iu_nids+'_'+uid).empty();
	// for list view
	if(action=='list') {
		di_jq('#sgm_'+iu_nids+'_'+uid+' div').removeClass('di_gui_link3');
		di_jq('#sgm_'+iu_nids+'_'+uid+' div').addClass('di_gui_seld_txt');

		if(di_safevalue(opts.selectedDims[inid+'_'+unid])!='') opts.selectedDims[inid+'_'+unid] = [];

		di_jq('#sgm_'+iu_nids+'_'+uid+' input:checkbox:checked').each( function(e) {
			di_jq(this).attr('checked', false);
		});

		var defiusnid = di_jq(xmlObj).find('iu[inid="'+inid+'"]').attr("defiusnid");
		var totalSV = di_jq(xmlObj).find('ius').length;
		if(totalSV > 0) {

			if(opts.multiple==true && opts.selectAll==true)
			ui += '<div rel="all"><input type="checkbox" value=""><label class="di_gui_lists_specialoption"> '+ opts.labels.selectAll +'</label></div>'; 
			
			di_jq(xmlObj).find('ius').each(function(){
				var sv_name = di_jq(this).attr("n");
				var sv_nid = di_jq(this).attr("nid");
				var sg_is_global = di_jq(this).attr('sisG');
				var iusnid = di_jq(this).attr("iusnid");
				var dataexists = di_jq(this).attr("dataexists");

				var is_checked = '';
				if(di_safevalue(opts.selectedIus[inid+'_'+unid])!='' && opts.indObj.di_checkDuplicate(opts.selectedIus[inid+'_'+unid], iusnid+delm+sv_name)==1) {
					is_checked = 'checked="checked"';
				}
				else if(iusnid==defiusnid && isIuChecked==true){
					is_checked = 'checked="checked"';
					opts.indObj.di_manageSelection(uid, true, inid+ delm +unid+ delm +iusnid+ delm +sv_name, 'ius');
				}

				ui += '<div rel="'+inid+delm+unid+delm+iusnid+delm+sv_name+'">';
				if(opts.multiple==true)
				ui += '<input rel="'+sv_nid+'" type="checkbox" value="'+ iusnid +'" '+is_checked+'>';
				
				ui += '<label class="di_gui_tree_label">'+ sv_name +'</label></div>';
			});
		} // end if
	}
	else if(action=='dim') { // for dimension view
		di_jq('#sgm_'+iu_nids+'_'+uid+' div').removeClass('di_gui_seld_txt');
		di_jq('#sgm_'+iu_nids+'_'+uid+' div').addClass('di_gui_link3');

		var selected_dim = new Array();
		var di_sg_total_arr = new Array();
		var xml_obj = new Array();
		
		di_jq('#sgm_'+iu_nids+'_'+uid+' input:checkbox:checked').each( function(e) {
			selected_dim.push(this.value);
		});
		if(selected_dim.length==0) {
			// display list mode
			opts.indObj.getSgData(uid, inid, unid, 'list', xmlObj);
		}
		for(var i=0; i<selected_dim.length; i++) {
			di_jq(xmlObj).find('st[nid='+selected_dim[i]+']').each(function(){
				var innerobj = di_jq(this);
				di_jq(innerobj).find('stv').each(function() { 
					di_sg_total_arr.push(di_jq(this).attr('n'));
				});
			});
		} // end for

		// start for getting combinations
		var selectionDimCount = selected_dim.length;
		if(di_jq(xmlObj).find('ius').length>0) {
			if(opts.multiple==true && opts.selectAll==true)
			ui += '<div rel="all"><input type="checkbox" value=""><label class="di_gui_lists_specialoption"> '+ opts.labels.selectAll +'</label></div>';
			
			di_jq(xmlObj).find('ius').each(function(){
				var sg_name = di_jq(this).attr('n');
				var sgname = sg_name;
				var sv_nid = di_jq(this).attr("nid");
				var iusnid = di_jq(this).attr('iusnid');
				var sg_is_global = di_jq(this).attr('sisG');
				var sg_data_exist = di_jq(this).attr('dataexists');
				var count=0;

				// logic
				// sector dimension values will be replaced by null and if returned sg name is null then this will show
				// newly added if whole word exist then this will also display
				for(var j=0; j<di_sg_total_arr.length; j++) {
					sg_name = sg_name.replace(/^\s+|\s+$/g,"");
					var serch = jQuery.inArray(sg_name, di_sg_total_arr);
					var di_replaced = sg_name.search(di_sg_total_arr[j]) >= 0;
					
					if(di_replaced) {
						sg_name = sg_name.replace( di_sg_total_arr[j], " " );
						count = count+1;
					}
					if(sg_name == " " || serch > 0){
						if(count>=selectionDimCount) {
							var is_checked = '';
							if(di_safevalue(opts.selectedIus[inid+'_'+unid])!='' && opts.indObj.di_checkDuplicate(opts.selectedIus[inid+'_'+unid], iusnid+delm+sgname)==1) {
								is_checked = 'checked="checked"'
							}
							else {
								//if(count>=selectionDimCount) is_checked = 'checked="checked"';
							}

							ui += '<div rel="'+inid+delm+unid+delm+iusnid+delm+sgname+'">';
							
							if(opts.multiple==true)
							ui += '<input rel="'+sv_nid+'" type="checkbox" value="'+iusnid+'" '+is_checked+'>';
							
							ui += '<label class="di_gui_tree_label"> '+ sgname +'</label></div>';
									
							count = 0;
						}
					}

				} // end for
			});
		}

	} // end dimendion mode

	di_jq('#sg_list_'+iu_nids+'_'+uid).append(ui);

	// binding event
	this.di_SgBindingEvent(uid, 'sg_list_'+iu_nids+'_'+uid, 'sglist');
}
/* function to bind an event for click label/span/checkbox etc */
di_indInitObject.prototype.di_SgBindingEvent = function(uid, id, type) {
	var delm = this.pdelm;
	var opts = di_indcominputarr[uid];

	if(type=='menu') {
		// click on menu
		var idAr = id.split('_');
		// click on list div
		di_jq('#'+id+' div').bind('click', function() {
			var type = di_jq(this).attr('rel');
			if(type=='sglist') {
				opts.indObj.getSgData(uid, idAr[1], idAr[2], 'list', '');
			}
		});
		// click on dimension checkbox
		di_jq('#'+id+' input[type="checkbox"]').bind('click', function() {
			var val = this.value;
			var checked = this.checked;
			var iudims = idAr[1] + '||' + idAr[2] + '||' + val;
			if(checked==true) {
				opts.indObj.di_manageSelection(uid, true, iudims, 'dims');
			}
			else {
				opts.indObj.di_manageSelection(uid, false, iudims, 'dims');
			}
			
			opts.indObj.getSgData(uid, idAr[1], idAr[2], 'dim', '');
		});
	} // end menu event
	else if(type=='sglist' || type=='iusg') { // for sg/iusg list
		var patern = '';
		if(type=='sglist') patern = 'div';
		else patern = 'li';

		// for change label bgcolor
		di_jq('#'+id+' '+patern+' label').bind('mouseenter mouseleave', function(evt){
			var type = di_jq(this).parent().attr('rel');
			if(type!='all') {
				if (evt.type == 'mouseenter') {
					//di_jq('#'+this.id+ ' > label:eq(0)').addClass('di_gui_label_hover');
					di_jq(this).addClass('di_gui_label_hover');
				} else if (evt.type == 'mouseleave') {
					di_jq(this).removeClass('di_gui_label_hover');
				}
			}
		});
		// for li label click
		di_jq('#'+id+' '+patern+' label').bind('click', function(evt){
			var iu_nids = di_jq(this).parent().attr('rel');
			var iu_Ar = iu_nids.split(delm);
			var iuid = 'iu_'+iu_Ar[0]+'_'+iu_Ar[1]+'_'+uid;
			var iuvals = di_jq('#'+iuid).attr('rel');
			if(iu_nids!='all') {
				if(di_jq(this).parent().find('input[type="checkbox"]').is(':checked')==true) {
					di_jq(this).parent().find('input[type="checkbox"]').attr('checked', false);
					opts.indObj.di_manageSelection(uid, false, iu_nids, 'ius');
				}
				else{
					di_jq(this).parent().find('input[type="checkbox"]').attr('checked', true);
					if(di_safevalue(opts.selectedIus[iu_Ar[0]+'_'+iu_Ar[1]])=='') {
						opts.indObj.di_manageSelection(uid, true, iuvals, 'ind');
						di_jq('#'+iuid+' input[value="'+iu_Ar[0]+'~'+iu_Ar[1]+'"]').attr('checked', true);
					}
					opts.indObj.di_manageSelection(uid, true, iu_nids, 'ius');
				}
			}
			else {
				//select all
			}

			if(di_safevalue(opts.selectedIus[iu_Ar[0]+'_'+iu_Ar[1]])=='') {
				di_jq('#'+iuid+' input[value="'+iu_Ar[0]+'~'+iu_Ar[1]+'"]').attr('checked', false);
			}
		});
		// for li checkbox click
		di_jq('#'+id+' '+patern+' input[type="checkbox"]').bind('click', function(evt){
			var iu_nids = di_jq(this).parent().attr('rel');
			var checked = this.checked;
			var iuid = '';
			var i_nid = '';
			var u_nid = '';
			var iuvals = '';
			if(iu_nids!='all') {
				var iu_Ar = iu_nids.split(delm);
				i_nid = iu_Ar[0];
				u_nid = iu_Ar[1];
				iuid = 'iu_'+i_nid+'_'+u_nid+'_'+uid;
				iuvals = di_jq('#'+iuid).attr('rel');
				
				if(checked==true) {
					if(di_safevalue(opts.selectedIus[i_nid+'_'+u_nid])=='') {
						opts.indObj.di_manageSelection(uid, true, iuvals, 'ind');
						di_jq('#'+iuid+' input[value="'+i_nid+'~'+u_nid+'"]').attr('checked', true);
					}
					opts.indObj.di_manageSelection(uid, true, iu_nids, 'ius');
				}
				else{
					opts.indObj.di_manageSelection(uid, false, iu_nids, 'ius');
				}
			}
			else { // select all
				var count = 0;
				di_jq('#'+id+' '+patern+' input[type="checkbox"]').each(function(evt){ 
					var iunids = di_jq(this).parent().attr('rel');
					var iuAr = iunids.split(delm);
					i_nid = iuAr[0];
					u_nid = iuAr[1];
					iuid = 'iu_'+i_nid+'_'+u_nid+'_'+uid;
					iuvals = di_jq('#'+iuid).attr('rel');
					if(iunids!='all') {
						di_jq(this).attr('checked', checked);
						if(checked==true && count==0) {
							if(di_safevalue(opts.selectedIus[i_nid+'_'+u_nid])=='') {
								opts.indObj.di_manageSelection(uid, true, iuvals, 'ind');
								di_jq('#'+iuid+' input[value="'+i_nid+'~'+u_nid+'"]').attr('checked', true);
							}
						}
						opts.indObj.di_manageSelection(uid, checked, iunids, 'ius');
					
					count++;
					}
				});
			} // end
			// check if the selection of ius is empty then iu will get deselected
			if(di_safevalue(opts.selectedIus[i_nid+'_'+u_nid])=='') {
				di_jq('#'+iuid+' input[value="'+i_nid+'~'+u_nid+'"]').attr('checked', false);
			}
			// check if all sg selected
			opts.indObj.di_SgBindingSelectAll('#'+id+' '+patern);
		});

		// check if all sg selected
	    opts.indObj.di_SgBindingSelectAll('#'+id+' '+patern);

	} // end sg event
}

/* function to check if all sg select then check select all checkbox automatccaly */
di_indInitObject.prototype.di_SgBindingSelectAll = function(idpatr) { 
	var totchk = di_jq(idpatr+' input[type="checkbox"]').length;
	if(totchk>1) {
		var totvchk = di_jq(idpatr+' input[value!=""]').length;
		var totchked = di_jq(idpatr+' input[value!=""]:checked').length;
		if(totvchk == totchked) {
			//select all will be checked
			di_jq(idpatr+' input[value=""]').attr('checked', true);
		}
		else {
			//select all will be unchecked
			di_jq(idpatr+' input[value=""]').attr('checked', false);
		}
	}
}

/**********************************************/
// End Subgroup related functions
/**********************************************/



