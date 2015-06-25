/*
    * diuilibcommon_.js
	* Common variables/function to draw component UI
	*
*/
/* Defining commom global variables */
var di_param_delimiter = "[****]";
var di_id_delimiter = "[~]";
var di_search_row_delemeter = "[|~~|]";
var di_ElementDelimiter = ",";
var di_images_path = di_diuilib_url + di_component_version + '/images';


/* including common css and given css files */
include_jsfile( di_diuilib_url + di_component_version + '/css/area/di.ar.ui.custom.css', 'css');
include_jsfile( di_diuilib_url + di_component_version + '/css/area/di.area.tree.min.css', 'css');
if(di_theme_css=='') {
	include_jsfile( di_diuilib_url + di_component_version + '/css/diuilibcommon.css', 'css');	
}
else {
	include_jsfile( di_theme_css, 'css');
}

/* **********************************
Start code for Area Component 
********************************** */

/* global var to store area options */
var di_areacominputarr = [];

/* **********************************
End code for Area Component 
********************************** */


/* **********************************
Start code for Indicator Component
********************************** */

/* global var to store indicator options */
var di_indcominputarr = [];

/* **********************************
End code for Indicator Component
********************************** */
/* **********************************
Start code for IE7 Fix
********************************** */
var Browser = {
    Version: function () {
        var version = 999; // we assume a sane browser
        if (navigator.appVersion.indexOf("MSIE") != -1)
        // bah, IE again, lets downgrade version number
            version = parseFloat(navigator.appVersion.split("MSIE")[1]);
        return version;
    }
}
if (Browser.Version() < 9) {
    // if client is using IE6 or lower, run this code
    include_jsfile(di_diuilib_url  + '1.1/js/di.json2.js', 'js');
}
/* **********************************
End code for IE7 Fix
********************************** */

/* **********************************
Start code for Quick Data Search Component
********************************** */

include_jsfile(di_diuilib_url + di_component_version + '/css/qds/di.qds.css', 'css');

/* Draw quick data search widget */
function di_draw_quick_data_search(draw_div_id, indicator_title, area_title, width, height, indicator_codelist_foler_path, area_codelist_folder_path, indicator_display_type, indicator_value_type, area_display_type, area_value_type, callback_on_search_selection, selection_mode, ind_defalult_text, area_defalut_text, search_img, ind_search_text, area_search_text, ind_block_details, area_block_details, advance_search, suggectionList, search_btn_text, area_isblock_text, callback_on_clear_all_click)
{ 	
    try
    {
	    di_draw_qds_ui(draw_div_id, indicator_title, area_title, width, height, indicator_codelist_foler_path, area_codelist_folder_path, indicator_display_type, indicator_value_type, area_display_type, area_value_type, callback_on_search_selection, selection_mode, ind_defalult_text, area_defalut_text, search_img, ind_search_text, area_search_text, ind_block_details, area_block_details, advance_search, suggectionList, search_btn_text, area_isblock_text, callback_on_clear_all_click);
    }	
	catch(err){}
}


/* get all selected indicators values */
function di_get_selected_indicators_values()
{
	var RetVal = '';
	
	try
	{	
		RetVal = di_qds_get_selected_indicators();
	}
	catch(err){}

	return RetVal;
}

/* get all selected areas values */
function di_get_selected_areas_values()
{
	var RetVal = '';

	try
	{		
		RetVal = di_qds_get_selected_areas();
	}
	catch(err){}
	
	return RetVal;
}

// Get inner html of indicator item blocks
function di_get_selected_indicators_block_details()
{
    var RetVal = '';

	try
	{	
		RetVal = di_get_selected_ind_block_det();
	}
	catch(err){}
	
	return RetVal;  
}

// Get inner html of area item blocks
function di_get_selected_areas_block_details()
{
    var RetVal = '';

	try
	{	
		RetVal = di_get_selected_area_block_det();
	}
	catch(err){}
	
	return RetVal;    
}

/* get entered/selected indicator data */
function di_qds_get_ind_search_data()
{
	var RetVal = '';

	try
	{		
		RetVal = di_qds_get_ind_search_text();
	}
	catch(err){}
	
	return RetVal;
}

/* get entered/selected area data */
function di_qds_get_area_search_data()
{
	var RetVal = '';

	try
	{		
		RetVal = di_qds_get_area_search_text();
	}
	catch(err){}
	
	return RetVal;
}

/* Get the text of all indicator blocks */
function di_qds_get_all_ind_block_data()
{
	var RetVal = '';

	try
	{		
		RetVal = di_qds_get_all_ind_block_text();
	}
	catch(err){}
	
	return RetVal;
}

/* Get the text of all area blocks */
function di_qds_get_all_area_block_data()
{
	var RetVal = '';

	try
	{		
		RetVal = di_qds_get_all_area_block_text();
	}
	catch(err){}
	
	return RetVal;
}


/* **********************************
End code for Quick Data Search Component
********************************** */


/* **********************************
Start code for Database Selection Component
********************************** */

// Global variable for database component
var di_db_slash = "/";
var di_db_searchText='';
var di_db_selectedDbId='';
var di_db_selectBtnText='';
var di_db_inuseBtnText='';
var di_db_indicatorsText='';
var di_db_areasText='';
var di_db_sourcesText='';
var di_db_datavaluesText='';
var di_db_lastmodText='';
var di_db_functionName='';
var di_db_dataCollection=new Array();
var di_db_controlID='';
var di_db_component_ok_function_name='';
var di_db_component_close_function_name='';

/* Set database component html in div within parent application
    Parameters: 
                database_control_id - Div ID in Parent application
                database_title - Component Title
                database_control_width - Div Width
                database_control_height - Div Height
                database_filePath - database file url(predefined structure xml file)
                database_caption - Component Caption
                database_searchbox_text - Text within searchbox
                database_select_btn - text for select button
                database_inuse_btn - text for use button
                database_dbId - database Id
                db_indicatorsText - Indicators Text
                db_areasText - Areas Text
                db_sourcesText - Sources Text
                db_dataValuesText - Data Values Text    
                database_functionName - function name which will be called while clicking on select button
*/
function di_db_drawDbSelectionControl(db_ctrl_id,db_title,db_width,db_height,db_filePath,db_caption,db_searchboxText,db_select_btn,db_inuse_btn,db_dbId,db_indText,db_areasText,db_sourcesText,db_dvText,db_lastmodText,db_hotFun,db_lng,db_isMultipleDb,db_lbl_ok,db_lbl_cancel,db_component_ok_function_name,db_component_close_function_name)
{
	var di_db_dbFileObject=null;
    var di_db_html='';
    this.di_db_selectedDbId = db_dbId;
    this.di_db_selectBtnText=db_select_btn;
    this.di_db_inuseBtnText=db_inuse_btn;
    this.di_db_indicatorsText=db_indText;
    this.di_db_areasText=db_areasText;
    this.di_db_sourcesText=db_sourcesText;
    this.di_db_datavaluesText=db_dvText;    
    this.di_db_lastmodText=db_lastmodText;    
    this.di_db_functionName = db_hotFun;
	this.di_db_controlID = db_ctrl_id;
	this.di_db_isMultipleDb = db_isMultipleDb;
    this.di_db_component_ok_function_name = db_component_ok_function_name;
	this.di_db_component_close_function_name = db_component_close_function_name;
	
	// set div attributes
	di_db_setDivControlAttributes(db_ctrl_id,db_width);
	// load datafile
    di_db_dbFileObject = di_db_LoadXMLs(db_filePath);
	// get & set html markup title & search part only(container)
    di_db_html = di_db_ControlContainer(db_title,db_caption,db_searchboxText,db_height,db_width,db_isMultipleDb,db_lbl_ok,db_lbl_cancel);
    document.getElementById(db_ctrl_id).innerHTML = di_db_html;
    // Make Array collect using parsing xml object
    di_db_makeDataCollection(db_lng,di_db_dbFileObject);
	di_db_html = di_db_GetContainerItem();
    document.getElementById('dbDetailDiv').innerHTML=di_db_html;
	/*applyCss();
	di_jq("#dbDetailDiv").jcarousel({
        scroll: 1,
		itemFallbackDimension: 300,
		initCallback: mycarousel_initCallback,
        // This tells jCarousel NOT to autobuild prev/next buttons
        buttonNextHTML: null,
        buttonPrevHTML: null
    });*/
}

function di_db_setDivControlAttributes(di_db_ctrlID,db_width)
{
	//di_jq('#'+di_db_ctrlID).hide();
	document.getElementById(di_db_ctrlID).style.border="1px solid #cecece";
	di_jq('#'+di_db_ctrlID).css("z-index",600);
	di_jq('#'+di_db_ctrlID).css("position","absolute");
	di_jq('#'+di_db_ctrlID).css("background-color","#ffffff") ;
	//di_jq('#'+di_db_ctrlID).css("top","200px") ;
	//di_jq('#'+di_db_ctrlID).css("overflow-x","auto") ;
	di_jq('#'+di_db_ctrlID).css("overflow","hidden") ;
	//di_jq('#'+di_db_ctrlID).css("-webkit-border-radius","8px");
	//di_jq('#'+di_db_ctrlID).css("border-radius","8px");
	di_jq('#'+di_db_ctrlID).css("width",db_width+"px");
	//di_jq('#'+this.di_db_controlID).css("left","-10px") ;
	//di_jq('#'+this.di_db_controlID).css("top","-10px") ;       
}

function di_db_showDatabaseControl(evt)
{
	evt = evt || window.event;                                   
     // Get x and y coordinates to show callout at mouse position    
	var xPos = 0; var yPos = 0;
	if (evt.pageX || evt.pageY){
		xPos = evt.pageX;
		yPos = evt.pageY;
	}
	else if (evt.clientX || evt.clientY){
		xPos = evt.clientX;
		yPos = evt.clientY;
	}
	//di_jq('#'+this.di_db_controlID).css("left",xPos+"px") ;
	//di_jq('#'+this.di_db_controlID).css("top",yPos+ "px") ;       
	//di_jq('#'+this.di_db_controlID).slideDown();
}
function di_db_getSelectedDB()
{
    return this.di_db_selectedDbId;
}

function di_db_getAllSelDbNIds()
{
     return di_db_sel_db_nids.join(',');
}

function di_db_getAllSelDbNames()
{
     return di_db_sel_db_names.join(',');
}

/* **********************************
End code for Database Selection Component
********************************** */




/* **********************************
Start code for Language Dropdown Component
********************************** */

// Global variable for Language Dropdown Component
var di_lc_selectedLanguage;
var di_lc_hotSelectionFunction;
var di_lc_collection=[];
function di_lc_drawLanguageSelectionComponent(di_lc_divCtrlId,di_lc_lgnFileURL,di_lc_defLng,di_lc_HotFun)
{
	var di_lc_lngFileObj;
	var di_lc_html;
	this.di_lc_hotSelectionFunction = di_lc_HotFun;
	di_lc_html = "<select id='di_lc_"+di_lc_divCtrlId+"' class='di_gui_dropdown1' onchange='di_lc_onChangeLanguage(this.value)'></select>";
	di_jq("#"+ di_lc_divCtrlId).html(di_lc_html);
	di_lc_lngFileObj = di_lc_LoadXMLs(di_lc_lgnFileURL);
	di_lc_parseLngFileAndGetMarkup(di_lc_lngFileObj,di_lc_divCtrlId);
	di_jq("#di_lc_"+di_lc_divCtrlId).val(di_lc_defLng);
}

function di_lc_getSelectedLanguage()
{
	return this.di_lc_selectedLanguage;
}
function di_lc_isLanguageRTL(di_lc_lngCode)
{
	var di_lc_isRTL = false;
	for(var i=0;i<di_lc_collection.length;i++)
	{
		var object = di_lc_collection[i];
		if(object.code.toLowerCase()==di_lc_lngCode.toLowerCase() && object.rtl.toLowerCase()=="t")
		{
			di_lc_isRTL = true;
			break;
		}
	}
	return di_lc_isRTL;
}


/* **********************************
End code for Language Dropdown Component
********************************** */


/* **********************************
Start code for common use
********************************** */

/* function to check where value is null, or undefined*/
function di_safevalue(val)
{
    if(val==undefined || val=='' || val==null)
        return '';
    else
        return val;    
}
/* function to generate unique id */
function di_generateUID() {
   return (((1+Math.random())*0x10000)|0).toString(16).substring(1);
}
/* function to toggle widget */
function di_toggle_widget(div_id) 
{
    di_jq('#'+div_id).slideToggle();	    
}
/* function to escape special chars*/
function di_strEscapeSpclchar(str) {
	//return str.replace("'", "\\'");
	return str;
}

//*Function to trim the space in the  string
function di_trim(s) {
   var temp = s;
   return temp.replace(/^\s+/,'').replace(/\s+$/,'');
}

/* include js files */
function include_jsfile(filename, filetype) {
	if (filetype=="js"){ //if filename is a external JavaScript file
		var script = document.createElement('script');
		script.src = filename;
		script.type = 'text/javascript';
		document.getElementsByTagName('head')[0].appendChild(script);
	}
	else if (filetype=="css"){ //if filename is an external CSS file
		var fileref=document.createElement("link");
		fileref.setAttribute("rel", "stylesheet");
		fileref.setAttribute("type", "text/css");
		fileref.setAttribute("href", filename);
		document.getElementsByTagName("head")[0].appendChild(fileref);
	}
	
}

// function to show/hide loading
function di_loading(val) {
	if(val=='show') {
		di_jq('#block_div').show();
		di_jq('#block_div_loading').show();
	}
	else{
		di_jq('#block_div_loading').hide();
		di_jq('#block_div').hide();
	}
}

function di_handle_undefined(val)
{
    if(val==undefined)
        return '';
    else
        return val;    
}

//function to clear search text box
function di_clear_searchbox(searchboxid) {
	di_jq('#'+searchboxid).val('');
	di_jq('#'+searchboxid).addClass('di_gui_searchbox_text');
	di_jq('#'+searchboxid).focus();
}

function di_clickclear(thisfield, defaulttext) 
{ 
    try
    {
	    if (di_jq('#'+thisfield).val() == defaulttext) {
		    di_jq('#'+thisfield).val("");
		    di_jq('#'+thisfield).removeClass('di_gui_searchbox_defaulttext');
		    di_jq('#'+thisfield).addClass('di_gui_searchbox_text');
	    }
	}	
    catch(err){}
}
function di_clickrecall(thisfield, defaulttext) 
{ 
    try
    {
	    if (di_jq('#'+thisfield).val() == "") {
		    di_jq('#'+thisfield).val(defaulttext);
		    di_jq('#'+thisfield).removeClass('di_gui_searchbox_text');
		    di_jq('#'+thisfield).addClass('di_gui_searchbox_defaulttext');
	    }
	}	
    catch(err){}
}
/* Transform xml and xslt into html */
function di_xml_xsl_transformation(xmlFile, xsltFile)
{
    var html = '';
    
    try
    {    
        html = di_jq.transform({async:false, xml: xmlFile, xsl: xsltFile});
    }
    catch(err){}
    
    return html;
}

/* Replace all occurance of a stirng */
function di_ReplaceAll(txt, replace, with_this) {
    return txt.replace(new RegExp(replace, 'g'), with_this);
}

function di_compApplyMasking(zIndex)
{   
    var MaskedDiv;
    //di_jq('#block_div').show(); 
    try
    {
        MaskedDiv = document.getElementById('block_div');
        
        if(MaskedDiv == null)
        {
            MaskedDiv = document.createElement('block_div');            
            MaskedDiv.id = 'block_div';
            document.body.appendChild(MaskedDiv);
            
        }
        
        // include a temporary iframe to make the ddl invisible/un-accessible
        var iFrameHtml = "<iframe style='position: absolute; display: block; background-color: #ffffff; " +
                            "z-index: -1; width: 100%; height: 100%; top: 0; left: 0; filter: mask();' >" + "</iframe>";                         
                            
        // Masking the whole screen    
        if(di_compIsIEBrowser()){
            MaskedDiv.innerHTML += iFrameHtml;
        }            
        MaskedDiv.style.display = '';
        MaskedDiv.style.visibility = 'visible';
        MaskedDiv.style.top = '0px';
        MaskedDiv.style.left = '0px';
        MaskedDiv.style.width = document.documentElement.clientWidth + 'px';         
        MaskedDiv.style.height = document.documentElement.scrollHeight + 'px';
        
        if(zIndex){
            MaskedDiv.style.zIndex = zIndex;
        }

    }
    catch(err){}
}
function di_compIsIEBrowser()
{
    var RetVal = true;
    
    try
    {    
        if (/MSIE (\d+\.\d+);/.test(navigator.userAgent))
        { 
             //test for MSIE x.x;
             var ieversion=new Number(RegExp.$1) // capture x.x portion and store as a number
             if (ieversion>=8){
              // IE8 or above
              RetVal = true;
             }
             else if (ieversion>=7){
              // IE7.x
              RetVal = true;
             }
             else if (ieversion>=6){
              // IE6.x
              RetVal = true;
             }
             else if (ieversion>=5){
               //IE5.x
               RetVal = true;
             }
        }
        else
        {
          RetVal = false;
        }
    }
    catch(err){}
    
    return RetVal;
}
function di_compRemoveMasking(zIndex)
{    
    try
    {
        var MaskedDiv = document.getElementById('block_div'); 
        
        // Remove the masking from the screen    
        MaskedDiv.style.display = 'none';
        MaskedDiv.innerHTML = "";
        
        if(zIndex){
            MaskedDiv.style.zIndex = zIndex;
        }
    }
    catch(err){}
}
/* **********************************
End code for common use
********************************** */