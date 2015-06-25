// JScript File

var diStructureAreaID = "Area ID";
var diStructureMRD = "isMRD";
var diStructureTimePeriod = "Time Period";
var diStructureSource = "Source";
var diStructureArea = "Area";
var diStructureTopic = "Indicator";

var isManuallySet = false;
var arrSavedFilters = [];
var ptrCurrentFilter = -1;

var isFilterApplied = false;
var minDataVal = 0;
var maxDataVal = 0;
var QdsDataNIds = '';
var OATfilePath = '';
var isMapEnable = true;
var CurrentFilter = '';

var FullScreenWidth = 0;
var FullScreenHeight = 0;

var isFullScreen = true;
var chartVisualizationDecided = 'table';

var isMRD = true;

var dataAll = [];
var dataMRD = [];

var isRefreshOnlyHeaderFilters = false;

var divSorting;
var divShowHide;
var divLineHelp;

var isSingleRowHeader = false;
var oldBlackList = [];
var isResetCallback = 0;

var Areas = [];
var Indicators = [];
var Units = [];
var Sources = [];
var IUSs = [];

var mapviewtype = 'single';

var OatFirstDraw_DisableFilters = true;
var strDimensionSubgroups = '';

function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hdvnids, hLoggedInUserNId, hLoggedInUserName, hCsvFilePath, hIsMapEnabled) {
    var hsgcount = 10;
    var breadcrumStr = '';
    QdsDataNIds = hdvnids;
    OATfilePath = hCsvFilePath;
    isMapEnable = (hIsMapEnabled === "true");

    // Set the active selection to Data Search Navigation link    
    di_jq("#aDS").attr("class", "navActive")

    // Hide background color div
    di_jq("#apDiv2").css("display", "none");

    // ************************************************
    // create Form Tag with hidden input boxes
    // ************************************************	
    createFormHiddenInputs("frm_dv", "POST");
    SetCommonLinksHref("frm_dv", "POST");

    // ************************************************1
    // set page level variables based on selections or defaults
    // ************************************************1
    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName, hdvnids);

    // ************************************************1
    // Load QDS Component
    // ************************************************1

    //	if (hdsby == "qds")
    //	{
    //	    ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div',hdbnid, hlngcodedb, '100%', '100px', 'GoToQDS', hselindo.split("[****]")[0], hselareao.split("[****]")[0], hselindo.split("[****]")[1], hselareao.split("[****]")[1],false);
    //	}
    //	else if (hdsby == "as")
    //    {
    //        ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div',hdbnid, hlngcodedb, '100%', '100px', 'GetQDSResult', hselindo.split("[****]")[0], hselareao.split("[****]")[0], hselindo.split("[****]")[1], hselareao.split("[****]")[1], true);
    //    }
    //	else
    //	{
    //	    ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div',hdbnid, hlngcodedb, '100%', '100px', 'GoToQDS', '', '', '', '',false);
    //	}

    // ************************************************1
    // Load Language Component
    // ************************************************1
    if (hlngcode == null && hlngcode == "") {
        hlngcode = readCookie('hlngcode');
    }

    if (GetLanguageCounts() > 1) {
        ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);
    }

    //    if(hdsby=="topic")
    //    {
    //	    // ************************************************1
    //	    // Load Area Component
    //	    // ************************************************1
    //	    //ShowAreaComponent( getAbsURL('stock'), 'area_div', hdbnid, hlngcodedb,'960', '300');
    //	    
    //	    // Breadcrum string for area selection        
    //        breadcrumStr = "<a class='di_gui_link' onclick='BreadCrumPost(\"frm_dv\",\"home.aspx\",\"POST\");'>Search for data by Topic</a>&nbsp;&gt;&nbsp;<a class='di_gui_link' onclick='BreadCrumPost(\"frm_dv\",\"Selection.aspx\",\"POST\");'>Select area</a>&nbsp;&gt;&nbsp;Data View";
    //    }
    //    else if(hdsby=="area")
    //    {
    //	    // ************************************************1
    //	    // Load Indicator Component
    //	    // ************************************************1
    //	    //ShowIndicatorComponent(getAbsURL('stock'), 'indicator_div', hdbnid, hlngcodedb, hselindo,'960', '300', hsgcount);
    //	    
    //	    // Breadcrum string for indicator selection
    //	    breadcrumStr = "<a class='di_gui_link' onclick='BreadCrumPost(\"frm_dv\",\"home.aspx\",\"POST\");'>Search for data by Geographic area</a>&nbsp;&gt;&nbsp;<a class='di_gui_link' onclick='BreadCrumPost(\"frm_dv\",\"Selection.aspx\",\"POST\");'>Select Indicator</a>&nbsp;&gt;&nbsp;Data View";
    //        
    //    }
    //    else if(hdsby=="qds")
    //    {   
    //	    // Breadcrum string for indicator selection
    //	    breadcrumStr = "<a class='di_gui_link' onclick='BreadCrumPost(\"frm_dv\",\"home.aspx\",\"POST\");'>Home</a>&nbsp;&gt;&nbsp;Data View";        
    //    }
    //    
    //    z('breadcrum_div').innerHTML = breadcrumStr;
    if (getQueryVariable("lang") == null) LoadLanguage("en");
    else LoadLanguage(getQueryVariable("lang"));


} // end on load function 

function GoToQDS() {
    z('hdsby').value = "qds";
    z('hselindo').value = di_qds_get_ind_search_data() + "[****]" + di_get_selected_indicators_block_details();
    z('hselareao').value = di_qds_get_area_search_data() + "[****]" + di_get_selected_areas_block_details();
    PostData("frm_dv", "QDS.aspx", "POST");
}

function AdvancedSearch() {
    try {
        z('hdsby').value = "as";
        z('hselindo').value = di_qds_get_ind_search_data() + "[****]" + "";
        z('hselareao').value = di_qds_get_area_search_data() + "[****]" + "";
        PostData("frm_dv", "QDS.aspx", "POST");
    }
    catch (err) { }
}

/* function to show component (Area or Indicator) in pop up */
function showHideComponent(comp) {
    if (comp == 'area') {
        ShowProgressBar(true);
        di_jq('#divShowLoading').hide();
        di_jq('#IndOuterBox').hide('slow');
        SetCloseButtonInPopupDiv(di_jq('#AreaOuterBox'), 'showHideComponent');
        di_jq('#AreaOuterBox').show('slow');

        GetWindowCentered(z("AreaOuterBox"), 787, 520);

        if (di_jq('#area_div').html() == '') {
            ShowAreaComponent(getAbsURL('stock'), 'area_div', z("hdbnid").value, z("hlngcodedb").value, z("hselareao").value, '767', '400', 'list_1');
            di_loading('hide');
        }
    }
    else if (comp == 'ind') {
        ShowProgressBar(true);
        di_jq('#divShowLoading').hide();
        di_jq('#AreaOuterBox').hide('slow');
        SetCloseButtonInPopupDiv(di_jq('#IndOuterBox'), 'showHideComponent');
        di_jq('#IndOuterBox').show('slow');

        GetWindowCentered(z("IndOuterBox"), 787, 520);

        //if(di_jq('#indicator_div').html()=='') 
        ShowIndicatorComponent(getAbsURL('stock'), 'indicator_div', z("hdbnid").value, z("hlngcodedb").value, z("hselindo").value, '767', '400', z("hsgcount").value, 'list', true, true);

    }
    else if (comp == 'dv') {

        ShowProgressBar(true);
        di_jq('#AreaOuterBox').hide('slow');

        SetCloseButtonInPopupDiv(di_jq('#divDataValueRange'), 'showHideComponent');

        di_jq('#divDataValueRange').show('slow');
        GetWindowCentered(z("divDataValueRange"), 710, 250);

    }
    else if (comp == 'headers') {

        ShowProgressBar(true);
        di_jq('#AreaOuterBox').hide('slow');

        SetCloseButtonInPopupDiv(di_jq('#divPivotHeaders'), 'showHideComponent');
        di_jq('#divPivotHeaders').show('slow');

        GetWindowCentered(z("divPivotHeaders"), 250, 128);

    }
    else if (comp == 'process') {
        di_jq('#AreaOuterBox').hide('slow');
        di_jq('#IndOuterBox').hide('slow');
        di_jq('#divDataValueRange').hide('slow');
    }
    else if (comp == 'source') {
        ShowProgressBar(true);
        di_jq('#divShowLoading').hide();
        SetCloseButtonInPopupDiv(di_jq('#SourceMetaOuterBox'), 'showHideComponent');
        di_jq('#SourceMetaOuterBox').show('slow');

        GetWindowCentered(z("SourceMetaOuterBox"), 720, 520);

    }
    else if (comp == 'login') {
        //ShowProgressBar(true);
        ApplyMaskingDiv();
        z("loginIframe").src = "login.aspx?popup=true&lng=" + z("hlngcode").value;
        di_jq('#AreaOuterBox').hide('slow');
        di_jq('#divDataValueRange').hide('slow');
        SetCloseButtonInPopupDiv(di_jq('#loginPopup'), 'showHideComponent');

        di_jq('#loginPopup').show('slow');
        GetWindowCentered(z("loginPopup"), 710, 450);
    }
    else if (comp == undefined) {
        ShowProgressBar(false);
        di_jq('#AreaOuterBox').hide('slow');
        di_jq('#IndOuterBox').hide('slow');
        di_jq('#SourceMetaOuterBox').hide('slow');
        di_jq('#divDataValueRange').hide('slow');
        di_jq('#divEmbeddLink').hide('slow');
        di_jq('#divPivotHeaders').hide('slow');
        di_jq('#loginPopup').hide('slow');

    }
    else {
        ShowProgressBar(false);
        di_jq('#AreaOuterBox').hide('slow');
        di_jq('#IndOuterBox').hide('slow');
        di_jq('#SourceMetaOuterBox').hide('slow');
        di_jq('#divDataValueRange').hide('slow');
        di_jq('#divEmbeddLink').hide('slow');
        di_jq('#divPivotHeaders').hide('slow');
        di_jq('#loginPopup').hide('slow');
        RemoveMaskingDiv();
    }

    isRefreshOnlyHeaderFilters = false;
}

//********************* Start for Grid

function hidePopupSelection() {
    //	di_jq('#GradOutDiv').hide();
    RemoveMaskingDiv();
    di_jq('#AreaOuterBox').hide('slow');
    di_jq('#IndOuterBox').hide('slow');
    di_loading('hide');
}

function onClickOkPopUpButton(type) {
    var delemeter = "||{~~}||";
    if (type == 'area') {
        QdsDataNIds = '';
        var areaData = AreaCompObj.getSelectedAreas().split(delemeter);
        z('hselarea').value = areaData[0];
        z('hselareao').value = areaData[1];
        //ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div', z('hdbnid').value, z('hlngcodedb').value , '100%', '100px', 'GoToQDS', '', '', '', '',false);
        ResetFilters();

    }
    else if (type == 'ind') {
        QdsDataNIds = '';
        var indData = IndCompObj.getSelectedData().split(delemeter);
        z('hselind').value = indData[0];
        z('hselindo').value = indData[1];
        //ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div',z('hdbnid').value, z('hlngcodedb').value, '100%', '100px', 'GoToQDS', '', '', '', '',false);
        ResetFilters();

    }
    else if (type == 'dv') {

        GenerateData();
    }
    else if (type == 'divEmailLink') {

        //GenerateData();
    }
    showHideComponent('process');

    delete delemeter;


}

var PivotRowDelimiter = "#";

function get_data(ResponseText) {

    var header, lastcol;
    var data = [];
    var mainlist = [];
    var sublist = [];
    var DataContent;

    DataContent = ResponseText.split(PivotRowDelimiter);

    if (OATfilePath == '') // Typical OAT string from home page
    {
        renderOATfromQDSorSelector(DataContent, header, data, mainlist, sublist, lastcol);
    }

    else // Data from file(CSV, XLS) upload
    {
        isMRD = false;
        renderOATfromCYV(DataContent, header, data, mainlist, sublist, lastcol);
    }

    ShowProgressBar(false);
}

function ShowProgressBar(TrueOrFalse) {
    if (TrueOrFalse == true) {

        z('divShowLoadingText').innerHTML = z('langLoadingData').value;
        z('imgGridLoader').style.display = 'none';
        z('divShowLoading').style.display = 'block';
        ApplyMaskingDiv();
        //ShowLoadingDiv();

        //z('imgGridLoader').style.display = 'block';        
    }
    else if (TrueOrFalse == false) {
        z('imgGridLoader').style.display = 'none';
        z('divShowLoading').style.display = 'none';
        //HideLoadingDiv();
        RemoveMaskingDiv();
    }
    else {

        z('divShowLoadingText').innerHTML = TrueOrFalse;

    }
}

// Callback to server is done here. Intelligence is be applied over here for deciding which callback has to be made on the basis of data/file posted
function CallBack() {
    ShowProgressBar(true);
    isResetCallback = 1; // variable to reset callback


    // Data is uploaded via file(CSV/XLS)
    if (OATfilePath != '') FileUpload_Callback();
    // Qds search is used to generate dataview
    else if (z('hdsby').value == "qds") {
        if (QdsDataNIds == '') {
            NonQDS_Callback();
        }
        else {
            QDS_Callback();
        }
    }
    // Advanced search flow is used to reach on DataView page
    else NonQDS_Callback();  // if (z('hdsby').value == "as")


}

function updateTitles(TitleContent) {
    z('divSingleSource').innerHTML = '';
    var tmpTitle = z('dataTitle');
    var tmpSubTitle = z('dataSubTitle');

    var tmpPairs = TitleContent.split("|");

    var strTitle = "";
    var strSubTitle = "";
    for (var i = 0; i < tmpPairs.length; i++) {
        var tmpKeyVal = tmpPairs[i].split(":");
        var tmpItemVal = tmpPairs[i].substring(tmpKeyVal[0].length + 1);
        switch (tmpKeyVal[0]) {
            case diStructureArea:
                strTitle += ", " + tmpItemVal;
                break;
            case diStructureTopic:
                strTitle += ", " + tmpItemVal;
                break;
            case "Unit":
                strTitle += "-" + tmpItemVal;
                break;
            case diStructureTimePeriod:
                strTitle += ", " + tmpItemVal;
                break;
            case "Sub Group":
                if (tmpItemVal != "Total")
                    strSubTitle += ", " + tmpItemVal;
                break;
            case diStructureSource:
                {
                    var tmpSourceLI = document.createElement('li');
                    tmpSourceLI.innerHTML = tmpItemVal;
                    z('ulSources').innerHTML = '';
                    z('ulSources').appendChild(tmpSourceLI);

                    //                    z('divSingleSource').innerHTML = 'Source: ' + tmpItemVal;
                    z('divSingleSource').innerHTML = z('langDVSource').value + tmpItemVal;


                    break;
                }
        }
    }
    if (strTitle.indexOf(",") == 0) strTitle = strTitle.substring(1);
    if (strSubTitle.indexOf(",") == 0) strSubTitle = strSubTitle.substring(1);
    tmpTitle.innerHTML = strTitle.trim();
    if (tmpSubTitle == '') {
        tmpSubTitle.style.display = 'none';
    }
    {
        tmpSubTitle.style.display = 'block';
        tmpSubTitle.innerHTML = strSubTitle.trim();
    }

    return strTitle;
}

/* function to load footnotes for any FootNoteNid*/
function getFootNote(FootNoteNid, event) {
    var Result = '';
    var FootPath = getAbsURL('stock') + 'data/' + z('hdbnid').value + '/ds/en/footnotes/';
    var di_return_first_ic_type_val = '';
    di_jq.ajax({
        type: "GET",
        url: FootPath + FootNoteNid + ".xml",
        dataType: "xml",
        async: false,
        success: function (xml) {
            try {
                var tmpFootNoteVal = xml.lastChild.text || xml.lastChild.textContent;
                if (tmpFootNoteVal != '') {
                    Result = tmpFootNoteVal.toString();
                    ShowCallout('divCallout', Result, event);
                }
                else {
                    //alert("Blank");
                }

            }
            catch (ex) {
                //ShowProgressBar(false);
                //alert("Error : " + ex.message);
            }
        },
        error: function () {
            //ShowProgressBar(false);
            //
        },
        cache: false
    });

}


function JSonToArray(strJson) {
    var Result = [];
    var startPos = strJson.indexOf("{");

    var CollonSeparatedKeyVal = strJson.substring(startPos + 1).split("|");

    for (i = 0; i < CollonSeparatedKeyVal.length; i++) {
        var arrKeyVal = CollonSeparatedKeyVal[i].split(":");
        Result.push(arrKeyVal[0]); //Pushed Nid
        Result.push(CollonSeparatedKeyVal[i].substring(arrKeyVal[0].length + 1)); //Pushed corresponding string value
    }
    return Result;
}

function getStringFromNId(arrNidsStringPair, StrNId) {
    var Result = "";
    for (var i = 0; i < arrNidsStringPair.length; i = i + 2) {
        if (arrNidsStringPair[i] == StrNId) {
            Result = arrNidsStringPair[i + 1];
            break;
        }
    }
    return Result;

}

//********************* Start for Grid
function GenerateData() {
    //ShowProgressBar(true);
    isFilterApplied = true;
    CallBack();
}


function ResetFilters() {
    //    var tmpChkMRD = z('chkIsMRD');
    //    if(QdsDataNIds != '')
    //    {   
    //        tmpChkMRD.defaultChecked = false;
    //    }
    //    else
    //    {
    //        tmpChkMRD.defaultChecked = true;
    //    }

    //ShowProgressBar(true);
    isFilterApplied = false;
    CallBack();
}


function NonQDS_Callback() {
    var area_nids = z('hselarea').value;
    var ius_nid = z('hselind').value;
    var arrIUS = ius_nid.split(',');
    var container = z('allIndicators');
    var areao_nids = z('hselareao').value;
    var iuso_nid = z('hselindo').value;


    var InputParam = area_nids; 				                            // AreaNids
    InputParam += ParamDelimiter + ius_nid; 	                                // IU_Nids
    InputParam += ParamDelimiter + z('hdbnid').value; 	// DbNid
    InputParam += ParamDelimiter + z('hlngcode').value; 	// DbNid
    InputParam += ParamDelimiter + areao_nids;
    InputParam += ParamDelimiter + iuso_nid;
    if (isFilterApplied)  //Filters applied case
    {
        CurrentFilter = prepareFilterParameters();
        InputParam += ParamDelimiter + CurrentFilter;
    }

    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: { 'callback': '3', 'param1': InputParam },
        async: false,
        success: function (data) {
            try {
                get_data(data);
                changeHeaderText();
                changeFeaturesText();
                changeHideColumnsText();
            }
            catch (ex) {
                ShowProgressBar(false);
                InValidSelection();
                //                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        cache: false
    });

    if (isManuallySet) isManuallySet = false;
}


function QDS_Callback() {
    var RequiredDataNIds = QdsDataNIds.split("[####]")[0]; //Added to resolve expanded add to cart bug
    //    var InputParam = QdsDataNIds;
    var InputParam = RequiredDataNIds;

    InputParam += ParamDelimiter + z('hdbnid').value; 	// DbNid
    InputParam += ParamDelimiter + z('hlngcode').value; 	// language code

    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: { 'callback': '5', 'param1': InputParam },
        async: false,
        success: function (data) {
            try {
                get_data(data);
                changeHeaderText();
                changeFeaturesText();
                changeHideColumnsText();
            }
            catch (ex) {
                ShowProgressBar(false);
                InValidSelection();

                //                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        cache: false
    });

    if (isManuallySet) isManuallySet = false;
}

function HtmlOAT() {
    var objOAT = OAT.getSelfInstance();
    var InputParam = z('dataTitle').innerHTML + ParamDelimiter + z('dataSubTitle').innerHTML + ParamDelimiter;
    InputParam += JSON.stringify(objOAT.headerRow) + ParamDelimiter + JSON.stringify(objOAT.filteredData) + ParamDelimiter +
                   JSON.stringify(objOAT.rowConditions) + ParamDelimiter + JSON.stringify(objOAT.colConditions) + ParamDelimiter
                   + z('hdbnid').value;

    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: { 'callback': '13', 'param1': InputParam },
        async: false,
        success: function (data) {
            try {
                var EmbeddURL = getAbsURL('stock/shared/dv') + data + ".htm";
                //TODO : Read all settings from APP Settings and remove below hard coding.
                SocialSharing("You should check this out!", "DevInfo", EmbeddURL, getAbsURL('stock/templates') + "fb_table.png", "Powered By DevInfo", "This is dummy description for testing the facebook sharing feature.");

            }
            catch (ex) {
                ShowProgressBar(false);
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        cache: false
    });
}

function PrintOAT() {
    var mywindow = window.open('', '', 'scrollbars=yes', '');

    mywindow.document.write('<html><head><title>Print Data</title>');
    mywindow.document.write('<link href="../../stock/themes/default/css/style.css" rel="stylesheet" type="text/css" />');
    mywindow.document.write('</head><body >');
    mywindow.document.write(z('pivot').innerHTML);
    mywindow.document.write('</body></html>');
    mywindow.document.close();
    mywindow.print();
}

function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}


function ShowHidePivotHeaders(objOAT, divShowHideColumns) {
    var changedHeaders = [];

    for (var i = 0; i < objOAT.rowConditions.length; i++) {
        changedHeaders.push("row" + objOAT.rowConditions[i]);
        changedHeaders.push(objOAT.headerRow[objOAT.rowConditions[i]]);
    }

    for (var i = 0; i < objOAT.colConditions.length; i++) {
        changedHeaders.push("col" + objOAT.colConditions[i]);
        changedHeaders.push(objOAT.headerRow[objOAT.colConditions[i]]);
    }

    for (var i = 0; i < objOAT.headerRow.length; i++) // Check for those headers which were neither ColHeaders not RowHeaders
    {
        if (changedHeaders.indexOf(objOAT.headerRow[i]) == -1) //Found such Header, now add them into rendering list
        {
            changedHeaders.push(i);
            changedHeaders.push(objOAT.headerRow[i]);
        }
    }
    createShowHideCols(divShowHideColumns, changedHeaders);

}

function createShowHideCols(divSources, arrSources) {
    var objOAT = OAT.getSelfInstance();
    var arrSource = arrSources;
    var container = divSources;
    var tmpUL = document.createElement('ul');
    var arrDontRender = [diStructureMRD, "Data Value"];

    container.style.overflow = 'auto';

    //NOW JUST APPEND Show/Hide Columns
    for (var j = 0; j < arrSource.length; j = j + 2) {
        var tmpLI = document.createElement('li');
        var checkbox = document.createElement('input');
        checkbox.type = "checkbox";
        checkbox.value = arrSource[j];
        checkbox.id = "chkSource" + arrSource[j];

        try // Check it off if it doesnt exist in row & col headers
        {
            arrSource[j] = parseInt(arrSource[j].replace('row', '').replace('col', ''));

            var isExistsInRowCol = objOAT.rowConditions.indexOf(arrSource[j]) +
                                   objOAT.colConditions.indexOf(arrSource[j]) //&& (arrSource[j].indexOf('row') != 0 || arrSource[j].indexOf('col') != 0);

            if (isExistsInRowCol == -2)
                checkbox.defaultChecked = false;
            else
                checkbox.defaultChecked = true;
        }
        catch (err) {
            checkbox.defaultChecked = false;
        }

        checkbox.onclick = function (event) { removeOATrowHeader(this); };

        var lbl4chk = document.createElement('label');
        lbl4chk.htmlFor = "chkSource" + arrSource[j];
        lbl4chk.id = "lbl4chk" + arrSource[j];

        var SourceText = arrSource[j + 1];
        if (SourceText.length > 20) {
            SourceText = SourceText.substring(0, 21);
        }

        if (SourceText == diStructureAreaID) checkbox.name = "chkHideShowAreaID";

        lbl4chk.appendChild(document.createTextNode(SourceText));

        tmpLI.appendChild(checkbox);
        tmpLI.appendChild(lbl4chk);

        if (!(SourceText == diStructureMRD || SourceText == "Data Value")) {
            tmpUL.appendChild(tmpLI);
        }
    }

    tmpUL.style.overflow = 'auto';
    tmpUL.style.height = '100px';

    container.appendChild(tmpUL);
}


var isPanelVisible = true;

function SlideIt() {
    if (isPanelVisible) {
        z('divSlideIt').style.backgroundImage = z('divSlideIt').style.backgroundImage.replace('hide', 'show');
        if (GetRunningBrowserName() == 'Chrome') {
            di_jq('#tdFilters').hide('slow');
        }
        else {
            di_jq('#divFilters').hide('slow');
        }
        isPanelVisible = false;
    }
    else {
        z('divSlideIt').style.backgroundImage = z('divSlideIt').style.backgroundImage.replace('show', 'hide');
        if (GetRunningBrowserName() == 'Chrome') {
            di_jq('#tdFilters').show('slow');
        }
        else {
            di_jq('#divFilters').show('slow');
        }
        isPanelVisible = true;
    }
}


function getQueryVariable(variable) {
    var query = window.location.search.substring(1);
    var vars = query.split("&");
    for (var i = 0; i < vars.length; i++) {
        var pair = vars[i].split("=");
        if (pair[0] == variable) {
            return pair[1];
        }
    }
}

function loadjscssfile(filename, filetype) {
    if (filetype == "js") { //if filename is a external JavaScript file
        var fileref = document.createElement('script')
        fileref.setAttribute("type", "text/javascript")
        fileref.setAttribute("src", filename)
    }
    else if (filetype == "css") { //if filename is an external CSS file
        var fileref = document.createElement("link");
        fileref.setAttribute("rel", "stylesheet");
        fileref.setAttribute("type", "text/css");
        fileref.setAttribute("href", filename);
    }
    if (typeof fileref != "undefined")
        document.getElementsByTagName("head")[0].appendChild(fileref)
}


function replacejscssfile(oldfilename, newfilename, filetype) {
    var targetelement = (filetype == "js") ? "script" : (filetype == "css") ? "link" : "none" //determine element type to create nodelist using
    var targetattr = (filetype == "js") ? "src" : (filetype == "css") ? "href" : "none" //determine corresponding attribute to test for
    var allsuspects = document.getElementsByTagName(targetelement)
    for (var i = allsuspects.length; i >= 0; i--) { //search backwards within nodelist for matching elements to remove
        if (allsuspects[i] && allsuspects[i].getAttribute(targetattr) != null && allsuspects[i].getAttribute(targetattr).indexOf(oldfilename) != -1) {
            var newelement = createjscssfile(newfilename, filetype)
            allsuspects[i].parentNode.replaceChild(newelement, allsuspects[i])
        }
    }
}

function checkloadjscssfile(filename, filetype) {
    if (filesadded.indexOf("[" + filename + "]") == -1) {
        loadjscssfile(filename, filetype)
        filesadded += "[" + filename + "]" //List of files added in the form "[filename1],[filename2],etc"
    }
    else
        alert("file already added!")
}



function createjscssfile(filename, filetype) {
    if (filetype == "js") { //if filename is a external JavaScript file
        var fileref = document.createElement('script')
        fileref.setAttribute("type", "text/javascript")
        fileref.setAttribute("src", filename)
    }
    else if (filetype == "css") { //if filename is an external CSS file
        var fileref = document.createElement("link")
        fileref.setAttribute("rel", "stylesheet")
        fileref.setAttribute("type", "text/css")
        fileref.setAttribute("href", filename)
    }
    return fileref
}

// "../../stock/themes/default/css/style.css"

function FileUpload_Callback() {
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: "callback=62&param1=" + OATfilePath,
        async: false,
        success: function (data) {
            try {
                get_data(data);
            }
            catch (ex) {
                ShowProgressBar(false);
                InValidSelection();
                //                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        cache: false
    });

    if (isManuallySet) isManuallySet = false;
}

function getFilterElement(FilterText) {
    var tmpHeader = document.createElement('li');
    var tmpFilter = document.createElement('a');

    tmpFilter.innerHTML = FilterText;
    tmpFilter.className = 'collapsed';

    if (FilterText == "Sorting" || FilterText == "Hide Columns" || FilterText == "Help") {
        tmpFilter.style.color = "#750000";
        tmpHeader.id = "div" + FilterText.replace(' ', '');
    }

    else if (FilterText == diStructureAreaID) {
        tmpHeader.style.display = 'none';
        tmpHeader.id = 'divFilterAreaID';
    }
    else {
        tmpHeader.id = "divFilter" + FilterText.replace(' ', '');
    }

    //tmpFilter.style.paddingLeft = '18px';
    tmpHeader.appendChild(tmpFilter);

    return tmpHeader;
}

function getFilterCondition(checkBoxValue, isChecked, CondtionText, onClickFunctionName) {
    var tmpLI = document.createElement('li');

    var tmpCondition = document.createElement('input');
    tmpCondition.type = "checkbox";
    tmpCondition.id = 'chkFilter' + checkBoxValue.replace(',', '');
    tmpCondition.defaultChecked = isChecked;
    tmpCondition.value = checkBoxValue;
    tmpCondition.onclick = function (event) { onClickFunctionName(this); };

    var lbl4chk = document.createElement('label');
    lbl4chk.htmlFor = tmpCondition.id;
    lbl4chk.name = CondtionText;
    lbl4chk.onclick = function (event) { onClickFunctionName(z(tmpCondition.id)); };
    if (CondtionText.length > 25) //Hide 
    {
        CondtionText = CondtionText.substring(0, 25) + '...';
        lbl4chk.onmouseover = function (event) {
            ShowCallout('divCallout', this.name, event);
        };
        lbl4chk.onmouseout = function (event) { HideCallout('divCallout'); };
    }
    lbl4chk.appendChild(document.createTextNode(CondtionText));


    tmpLI.appendChild(tmpCondition);
    tmpLI.appendChild(lbl4chk);
    return tmpLI;

}

function getFilterMore(typeAreaInd, FilterIndex) {
    var tmpLI = document.createElement('li');
    var tmpFilter = document.createElement('div');
    var aSelectAll = document.createElement('span');
    var spanSeparator = document.createElement('span');
    var aClearAll = document.createElement('span');
    var aAreaInd = document.createElement('span');

    tmpFilter.style.width = '100%';
    tmpFilter.style.cssFloat = 'left';

    if (FilterIndex != undefined) {

        aSelectAll.style.verticalAlign = 'bottom';
        //aSelectAll.style.marginLeft = '5px';
        aSelectAll.style.cssFloat = 'left';
        aSelectAll.style.width = '50px';
        aSelectAll.style.cursor = 'pointer';
        aSelectAll.innerHTML =SetMaxChars(z('lngSelectall').value,10);
        aSelectAll.rel = FilterIndex;
        aSelectAll.onclick = function (event) {
            toggleAllFilters(this);
        }
        if (z('lngSelectall').value.length > 10) {
            aSelectAll.onmouseover = function (event) {
                ShowCallout('divCallout', z('lngSelectall').value, this);
            }
            aSelectAll.onmouseout = function (event) {
                HideCallout('divCallout');
            }
        }
        spanSeparator.style.cssFloat = 'left';
        spanSeparator.style.width = '5px';
        spanSeparator.style.marginLeft = '5px';
        spanSeparator.innerHTML = ' | ';

        aClearAll.style.verticalAlign = 'bottom';
        aClearAll.style.marginLeft = '5px';
        aClearAll.style.cssFloat = 'left';
        aClearAll.style.width = '50px';
        aClearAll.style.cursor = 'pointer';
        aClearAll.innerHTML = SetMaxChars(z("lngClearall").value, 10);
        aClearAll.rel = FilterIndex;
        aClearAll.onclick = function (event) {
            toggleAllFilters(this);
        }
        if (z('lngClearall').value.length > 10) {
            aClearAll.onmouseover = function (event) {
                ShowCallout('divCallout', z('lngClearall').value, this);
            }
            aClearAll.onmouseout = function (event) {
                HideCallout('divCallout');
            }
        }

        tmpFilter.appendChild(aSelectAll);
        tmpFilter.appendChild(spanSeparator);
        tmpFilter.appendChild(aClearAll);

    }

    aAreaInd.style.verticalAlign = 'bottom';
    aAreaInd.style.marginLeft = '5px';
    aAreaInd.style.cssFloat = 'right';
    aAreaInd.style.width = '50px';
    aAreaInd.style.cursor = 'pointer';
    aAreaInd.innerHTML = "More...";

    if (typeAreaInd == diStructureArea) {
        aAreaInd.onclick = function (event) { showHideComponent('area'); stopEventBubbling(event); };
        tmpFilter.appendChild(aAreaInd);
    }
    else if (typeAreaInd == diStructureTopic) {
        aAreaInd.onclick = function (event) { showHideComponent('ind'); stopEventBubbling(event); };
        tmpFilter.appendChild(aAreaInd);
    }

    tmpLI.appendChild(tmpFilter);

    return tmpLI;

}


var isRoutesVisible = false;

function generateFilterPanels(CurrentOAT, divFiltersPanel) {

    while (divFiltersPanel.hasChildNodes()) {
        divFiltersPanel.removeChild(divFiltersPanel.lastChild);
    }

    var tmpFiltersHeight = 320;
    var allCollapsibles = [];
    var objOAT = CurrentOAT;

    var tmpExpandableHeight = tmpFiltersHeight - (objOAT.headerRow.length - 1) * 25; // Deduct filters height from overall height 

    for (var i = 0; i < objOAT.headerRow.length - 1; i++) {
        var tmpHeader = getFilterElement(objOAT.headerRow[i]);

        if (objOAT.headerRow[i] == diStructureMRD) break;

        var tmpUL = document.createElement('ul');
        var tmpUL_height = 0;

        // Append all distinct items of any header into its respective filter collapsible tab

        for (var j = 0; j < objOAT.conditions[i].distinctValues.length; j++) {

            var tmpVal = objOAT.conditions[i].distinctValues[j];
            if (tmpVal != null && tmpVal != '')  // Don't show blank filter criterias
            {
                var isBlackListed = false;
                if (objOAT.conditions[i].blackList.indexOf(tmpVal) != -1) isBlackListed = true;

                if (j == 0) {
                    var tmpHeaderName = '';
                    switch (objOAT.headerRow[i]) {

                        case diStructureArea:
                            {
                                tmpHeaderName = diStructureArea;
                                break;
                            }
                        case diStructureTopic:
                            {
                                tmpHeaderName = diStructureTopic;
                                break;
                            }
                        case diStructureTimePeriod:
                            {
                                tmpHeaderName = diStructureTimePeriod;
                                break;
                            }
                        default:
                            {
                                tmpHeaderName = '';
                                break;
                            }
                    }

                    var tmpMore = getFilterMore(tmpHeaderName, i);
                    tmpUL.appendChild(tmpMore);

                    if (objOAT.headerRow[i] == diStructureTimePeriod) { // MRD li creation
                        var tmpLI = document.createElement('li');

                        var tmpCondition = document.createElement('input');
                        tmpCondition.type = "checkbox";
                        tmpCondition.defaultChecked = isMRD;
                        tmpCondition.onclick = function (event) { ToggleMRD(this); };

                        var lbl4chk = document.createElement('label');
                        lbl4chk.id = "MRDlbltxt";
                        lbl4chk.htmlFor = tmpCondition.id;
                        lbl4chk.onclick = function (event) { ToggleMRD(this); };
                        var mrdNode = document.createTextNode(z("lngMRD").value);
                        //mrdNode.id = "MRDlbltxt";
                        lbl4chk.appendChild(mrdNode);

                        tmpLI.appendChild(tmpCondition);
                        tmpLI.appendChild(lbl4chk);

                        tmpUL.appendChild(tmpLI);
                        tmpUL_height += 15;
                    }

                }

                var tmpLI = getFilterCondition(i + ',' + j, !isBlackListed, tmpVal, filterApplied);

                if (j == objOAT.conditions[i].distinctValues.length - 1) // For giving little margin before last item in UL
                {
                    tmpLI.style.marginBottom = '10px';
                }
                tmpUL.appendChild(tmpLI);
                tmpUL_height += 15;
            }

        }

        tmpHeader.appendChild(tmpUL);
        divFiltersPanel.appendChild(tmpHeader);
        if (tmpUL_height >= tmpExpandableHeight) {
            tmpUL.style.overflow = 'auto';
            tmpUL.style.height = tmpExpandableHeight + 'px';
            tmpUL.style.marginBottom = '8px';
        }
    }


    syncSources(CurrentOAT);

    /// Append Sorting, Hide column & others column in filters panel


    /// Code for making each and every menu item collapsible

    setTimeout(function () {
        // Accordion
        di_jq('#menu5 > li > a.expanded + ul').slideToggle('medium');
        di_jq('#menu5 > li > a').click(function () {
            di_jq('#menu5 > li > a.expanded').not(this).toggleClass('expanded').toggleClass('collapsed').parent().find('> ul').slideToggle('medium');
            di_jq(this).toggleClass('expanded').toggleClass('collapsed').parent().find('> ul').slideToggle('medium');
        });
    }, 250);

}




function NoDataFound() {
    ShowProgressBar(false);
    z('imgGridLoader').style.display = 'none';

    var PivotContent = '<center>' + z('langPleaseSelect').value + ' <a href ="#" onclick="showHideComponent(' + "'ind')" + '">' + z('langTopic').value + '</a> and ';
    PivotContent += ' <a href ="#" onclick="showHideComponent(' + "'area')" + '"> ' + z('langArea').value + ' </a> here.</center>';
    z('pivot_content').innerHTML = PivotContent;
    z('dataTitle').innerHTML = z('langNDFFCS').value;
    z('dataSubTitle').innerHTML = '';
    z('divCountDataRows').innerHTML = '';
    z('divSingleSource').innerHTML = '';
    toggleVisBtmTab('tab-d');

}

function InValidSelection() {
    ShowProgressBar(false);
    z('imgGridLoader').style.display = 'none';
    var PivotContent = '<center>' + z('langPleaseSelect').value + ' <a href ="#" onclick="showHideComponent(' + "'ind')" + '">' + z('langTopic').value + '</a> and ';
    PivotContent += ' <a href ="#" onclick="showHideComponent(' + "'area')" + '">' + z('langArea').innerHTML + ' </a> here.</center>';
    z('pivot_content').innerHTML = PivotContent;
    z('dataTitle').innerHTML = z('langSWWS').value;
    z('dataSubTitle').innerHTML = '';
    z('divCountDataRows').innerHTML = '';
    z('divSingleSource').innerHTML = '';
    toggleVisBtmTab('tab-d');
    if (readCookie('hlngcode') == null) {
        z("hlngcodedb").value = getDefaultLanguage();
    }
    else z("hlngcodedb").value = readCookie('hlngcode');
    if (z("hlngcodedb").value == "ar") {
        isRTL = true;
    }
    z("hdbnid").value = getDefaultDBNId();

}

function getDefaultDBNId() {
    var XmlPageURL = getAbsURL('stock') + "db.xml" + "?v=" + AppVersion;
    var defDBNId = "";
    di_jq.ajax({
        type: "GET",
        url: XmlPageURL,
        dataType: "xml",
        async: false,
        timeout: 5000,
        error: function (request, error) {
        },
        success: function (xml) {
            di_jq(parseXml(xml)).find('dbinfo').each(function () {
                defDBNId = di_jq(this).attr("def");
            });
        }
    });
    return defDBNId;
}

// for VC tabs
function toggleVisTab(tabR) {
    if (tabR != '') {
        var currentClass = di_jq('#' + tabR + 't').attr('class');
        if (currentClass == 'current' && di_jq('.panes').css('display') != 'none') {
            toggleVisPanel('up');
        }
        else {
            toggleVisPanel('down');
            di_jq('.panes div').each(function () { if (di_jq(this).attr('class') == 'panesInner' || di_jq(this).attr('class') == 'panesInner2') { di_jq(this).hide(); } });
            if ((chartVisualizationDecided == 'map' || chartVisualizationDecided == 'map2d' || chartVisualizationDecided == 'map3d') && tabR == 'tab-3') {

                //di_jq('#tab-4').slideToggle();
                di_jq('#tab-4').show();
            }
            else {
                di_jq('#' + tabR).show();
            }
            di_jq('.blue li > a').each(function () { di_jq(this).removeClass('current'); });
            di_jq('#' + tabR + 't').addClass('current');
        }
    } // outer if
}

/* function to toggle download option */
function toggleDownload() {
    if (OAT.getSelfInstance() == null)
        return;
    di_jq('#toggleDownload').slideToggle();
}
// for VC panel
function toggleVisPanel(ref) {
    if (ref == '' || ref == null || ref == undefined) {
        ref = di_jq('#toggle_arrow').attr('rel1');
    }
    if (ref == 'up') {
        di_jq('.panes').hide('slow');
        di_jq('#toggle_arrow').attr('rel1', 'down');
        di_jq('#toggle_arrow').removeClass('fullViewIcon_6');
        di_jq('#toggle_arrow').addClass('fullViewIcon_7');
        di_jq('#toggle_arrow').attr('rel', 'TTSSBar');
        di_jq('#timeLineWrapper').css("display", "block");
        if (di_jq('#timeLineWrapper').css("display") == "block") {
            setTimeLineHeight();
        }
    }
    else if (ref == 'down') {
        di_jq('.panes').show('slow');
        di_jq('#toggle_arrow').attr('rel1', 'up');
        di_jq('#toggle_arrow').removeClass('fullViewIcon_7');
        di_jq('#toggle_arrow').addClass('fullViewIcon_6');
        di_jq('#toggle_arrow').attr('rel', 'TTHSBar');
        if (di_jq('#timeLineWrapper').css("display") == "block") {
            setTimeLineHeight();
        }
    }
}
// for VC Bottom panel
function toggleVisBtmTab(refId) {
    if (chartVisualizationDecided == "map" && refId == 'tab-v') di_jq('#diMapParentMode').show(); else di_jq('#diMapParentMode').hide();
    if (chartVisualizationDecided == "map3d" && refId == 'tab-v') di_jq('#diMap3DOpacityBox').show(); else di_jq('#diMap3DOpacityBox').hide();

    if (refId == 'tab-v') {
        di_jq('#tab-3t').parents("li:first").show();
        di_jq('.blueBottm li > a').each(function () { di_jq(this).removeClass('current'); });
        di_jq('#' + refId).addClass('current');
        di_jq('.middleContainer').hide();
        di_jq('#' + refId + 'section').show();
        // refresh chart
        showVisualizer(chartVisualizationDecided);
        //showVisualizer(chartVisualizationDecided);

        // hiding seeting tab for 2d/3d map
        if (chartVisualizationDecided == "map2d" || chartVisualizationDecided == "map3d") {
            di_jq('#tab-3t').parents("li:first").hide();
        }
    }
    else {
        //di_jq('#tab-3t').parents("li:first").hide();
        if (chartVisualizationDecided == "table") {
            z("settings_section1").style.display = z("settings_section2").style.display = z("settings_section4").style.display = "none";
        }
        else {
            z("settings_section1").style.display = z("settings_section2").style.display = z("settings_section4").style.display = "";
        }

        z("arrangeRow").style.display = "block";
        z("pivotRow").style.display = "block";
        z("swapRow").style.display = "block";

        di_jq('.blueBottm li > a').each(function () { di_jq(this).removeClass('current'); });
        di_jq('#' + refId).addClass('current');
        di_jq('.middleContainer').hide();
        di_jq('#' + refId + 'section').show();
    }
}

/* function to click bottom tab (to be placed in dataview1.js)*/
function changeBottomTab(refId) {
    if (di_jq('#timeLine_cont')[0].firstChild != undefined) {
        if (refId == "tab-v") {
            di_jq(di_jq('#timeLine_cont')[0].firstChild).css({ 'display': 'block' });
        }
        else {
            di_jq(di_jq('#timeLine_cont')[0].firstChild).css({ 'display': 'none' });
        }
    }
    if (chartVisualizationDecided == "map" && refId == "tab-v") di_jq('#diMapParentMode').show(); else di_jq('#diMapParentMode').hide();
    if (chartVisualizationDecided == "map" || chartVisualizationDecided == "map2d" || chartVisualizationDecided == "map3d") {
        di_jq('.blueBottm li > a').each(function () { di_jq(this).removeClass('current'); });
        di_jq('#' + refId).addClass('current');
        di_jq('.middleContainer').hide();
        di_jq('#' + refId + 'section').show();
    }
    else {
        toggleVisBtmTab(refId);
    }
}




function showVisualizationToolTip(elementID) {
    var toolTipMessage = '';

    var vistType = di_jq(elementID).attr('rel');

    var elementClass = elementID.className;

    var tmpArrClass = elementClass.split('-');

    // If disabled then only show guidance in tooltip
    if (tmpArrClass.length == 3) {

        switch (vistType) {
            case 'line':
                toolTipMessage = z('LineChartDisabled').value;
                break;
            case 'column':
                toolTipMessage = z('ColumnChartDisabled').value;
                break;
            case 'bar':
                toolTipMessage = z('BarChartDisabled').value;
                break;
            case 'area':
                toolTipMessage = z('AreaChartDisabled').value;
                break;
            case 'pie':
                toolTipMessage = z('PieChartDisabled').value;
                break;
            case 'map':
                toolTipMessage = z('MapChartDisabled').value;
                break;
            case 'pyramid':
                toolTipMessage = z('PyramidChartDisabled').value;
                break;
            case 'treemap':
                toolTipMessage = z('TreeMapChartDisabled').value;
                break;
            case 'radar':
                toolTipMessage = z('RadarChartDisabled').value;
                break;
            case 'scatter2d':
                toolTipMessage = z('ScatterChartDisabled').value;
                break;
            case 'maptip':
                toolTipMessage = z('ScatterChartDisabled').value;
                break;
            default:
                toolTipMessage = z('DefaultDisabled').value;
        }
        ShowCallout('divCallout', toolTipMessage, elementID);
    }
}

function showVisualizationHeading(elementID) {
    //    var toolTipMessage = z(elementID.id).innerHTML;
    //        ShowCallout('divCallout', toolTipMessage, elementID);
}

function showSettingToolTip(elementID) {
    var toolTipMessage = '';

    var vistType = di_jq(elementID).attr('rel');
    switch (vistType) {
        case 'TTBold':
            toolTipMessage = z('ToolTipBold').value;
            break;
        case 'TTBBColor':
            toolTipMessage = z('ToolTipBarsBorderColor').value;
            break;
        case 'TTItalic':
            toolTipMessage = z('ToolTipItalic').value;
            break;
        case 'TTULine':
            toolTipMessage = z('ToolTipUnderline').value;
            break;
        case 'TTIFSize':
            toolTipMessage = z('ToolTipIncreaseFontSize').value;
            break;
        case 'TTDFSize':
            toolTipMessage = z('ToolTipDecreaseFontSize').value;
            break;
        case 'TTFColor':
            toolTipMessage = z('ToolTipFontColor').value;
            break;
        case 'TTFillC':
            toolTipMessage = z('ToolTipFillColor').value;
            break;
        case 'TTNBIcon':
            toolTipMessage = z('ToolTipNoBorderIcon').value;
            break;
        case 'TTBBLWidth':
            toolTipMessage = z('ToolTipBarBorderLineWidth').value;
            break;
        case 'TTBold':
            toolTipMessage = z('ToolTipBold').value;
            break;
        case 'TTBBColor':
            toolTipMessage = z('ToolTipBarsBorderColor').value;
            break;
        case 'TTDType':
            toolTipMessage = z('ToolTipDashType').value;
            break;
        case 'TTFColor1':
            toolTipMessage = z('ToolTipFillColor1').value;
            break;
        case 'TTCSColor':
            toolTipMessage = z('ToolTipChangeSeriesColor').value;
            break;
        case 'TTCTColor':
            toolTipMessage = z('ToolTipChangeThemeColor').value;
            break;
        case 'TTCTD45Text':
            toolTipMessage = z('ToolTipCustomTextDirection45Text').value;
            break;
        case 'TTCTDM45Text':
            toolTipMessage = z('ToolTipCustomTextDirectionMinus45Text').value;
            break;
        case 'TTCTD90Text':
            toolTipMessage = z('ToolTipCustomTextDirection90Text').value;
            break;
        case 'TTCTDM90Text':
            toolTipMessage = z('ToolTipCustomTextDirectionMinus90Text').value;
            break;
        case 'TTCTDHorizontal':
            toolTipMessage = z('ToolTipCustomTextHorizontal').value;
            break;
        case 'TTHSBar':
            toolTipMessage = z('ToolTipHideSettingBar').value;
            break;
        case 'TTSSBar':
            toolTipMessage = z('ToolTipShowSettingBar').value;
            break;
        case 'TTHFPanel':
            toolTipMessage = z('ToolTipHideFilterpanel').value;
            break;
        case 'TTSFPanel':
            toolTipMessage = z('ToolTipShowFilterPanel').value;
            break;
        case 'TTSAtoZ':
            toolTipMessage = z('ToolTipSortAtoZ').value;
            break;
        case 'TTSZtoA':
            toolTipMessage = z('ToolTipSortZtoA').value;
            break;
        case 'TTRSS':
            toolTipMessage = z('TooltipRSS').value;
            break;
        case 'TTEGSetting':
            toolTipMessage = z('ToolTipExpandGeneralSetting').value;
            break;
        case 'TTEFSetting':
            toolTipMessage = z('ToolTipExpandFormatSetting').value;
            break;
        case 'TTESSetting':
            toolTipMessage = z('ToolTipExpandSeriesSetting').value;
            break;
        default:
            toolTipMessage = "";
    }
    ShowCallout('divCallout', toolTipMessage, elementID);
}

function changeVisulizerChart(elementID) {
    di_jq('#timeLine_cont').html("");
    var vistType;

    vistType = di_jq(elementID).attr('rel');

    // calling function to change icon
    changeVisulizerIcon(vistType);

    /* added fro single/multiple theme map view*/
    if (vistType == 'mapm') {
        vistType = 'map';
        mapviewtype = 'multiple';
    }
    else {
        mapviewtype = 'single';
    }
    /* end */


    if (vistType == null) vistType = elementID;
    else {
        var tmpClassName = elementID.className;

        if (tmpClassName.indexOf('-') != tmpClassName.lastIndexOf('-')) return;
    }

    di_jq('#tab-2 span').removeClass('visual_icon_selected');
    if (vistType != 'table') {
        di_jq('#tab-v').closest('li').show(); // for visualization bottom tab
        chartVisualizationDecided = vistType;
        toggleVisBtmTab('tab-v');

        if (OATfilePath == '') {
            if (chartVisualizationDecided.substr(0, 3) == 'map') {
                showHideAllFilterPanes(false);
                z('divFilter' + diStructureArea).style.display = '';
                z('divFilter' + diStructureTopic).style.display = '';
            }
            else showHideAllFilterPanes(true);
        }
        else if (chartVisualizationDecided.substr(0, 3) == 'map') {
            z('imgShowHideFiltersPanel').style.display = 'none';
            toggleFiltersPanel(false);
        }
        else {
            z('imgShowHideFiltersPanel').style.display = '';
            toggleFiltersPanel(true);
        }


        //        if (vistType == 'line') divLineHelp.style.display = 'block';
        //        else divLineHelp.style.display = 'none';

        //        if (vistType == 'map') toggleFiltersPanel(false);
        //        else toggleFiltersPanel(true);
    }
    else {
        di_jq('#tab-v').closest('li').hide(); // for visualization bottom tab
        chartVisualizationDecided = vistType;
        toggleVisBtmTab('tab-d');
        z('imgShowHideFiltersPanel').style.display = '';
        toggleFiltersPanel(true);
        showHideAllFilterPanes(true);
        divLineHelp.style.display = 'none';
    }

    var selectedTab = vistType;
    // for map
    if (selectedTab == 'map2d' || selectedTab == 'map3d') selectedTab = 'map';
    // for line
    if (selectedTab == 'stackline' || selectedTab == '100stackline') selectedTab = 'line';
    // for column
    if (selectedTab == 'stackcolumn' || selectedTab == '100stackcolumn') selectedTab = 'column';
    // for bar
    if (selectedTab == 'stackbar' || selectedTab == '100stackbar') selectedTab = 'bar';
    // for area
    if (selectedTab == 'stackarea' || selectedTab == '100stackarea') selectedTab = 'area';

    di_jq('#a' + selectedTab).addClass('visual_icon_selected');

    showHideDownloads();

    di_jq('#tab-2t').removeClass('current');
    toggleVisTab('tab-2');

    //    if (chartVisualizationDecided.substr(0, 3) == 'map') showFiltersMask();
    //    else hideFiltersMask();

}
/* function to change visulizer icon in the visulizer band */
function changeVisulizerIcon(vistType) {
    if (vistType != '' && vistType != null && vistType != undefined) {
        var addext = '_' + vistType;
        if (vistType == 'map' || vistType == 'mapm' || vistType == 'map2d' || vistType == 'map3d') {
            if (vistType == 'map') addext = '';
            di_jq('#replace7').attr('class', 'visual_icon-7' + addext);
            di_jq('#replace7').attr('rel', vistType);
        }
        else if (vistType == 'line' || vistType == 'stackline' || vistType == '100stackline') {
            if (vistType == 'line') addext = '';
            di_jq('#replace2').attr('class', 'visual_icon-2' + addext);
            di_jq('#replace2').attr('rel', vistType);
        }
        else if (vistType == 'column' || vistType == 'stackcolumn' || vistType == '100stackcolumn') {
            if (vistType == 'column') addext = '';
            di_jq('#replace3').attr('class', 'visual_icon-3' + addext);
            di_jq('#replace3').attr('rel', vistType);
        }
        else if (vistType == 'bar' || vistType == 'stackbar' || vistType == '100stackbar') {
            if (vistType == 'bar') addext = '';
            di_jq('#replace4').attr('class', 'visual_icon-4' + addext);
            di_jq('#replace4').attr('rel', vistType);
        }
        else if (vistType == 'area' || vistType == 'stackarea' || vistType == '100stackarea') {
            if (vistType == 'area') addext = '';
            di_jq('#replace5').attr('class', 'visual_icon-5' + addext);
            di_jq('#replace5').attr('rel', vistType);
        }
    }
}

function showHideDownloads() {

    di_jq('#vcLangLblDlPDF').hide();
    di_jq('#vcLangLblDlPNG').hide();
    di_jq('#vcLangLblDlSVG').hide();
    di_jq('#vcLangLblDlXLS').hide();
    di_jq('#tblLangLblDlXLS').hide();
    di_jq('#mapLangLblDlPNG').hide();
    di_jq('#mapLangLblDlXLS').hide();
    di_jq('#mapLangLblDlKML').hide();
    di_jq('#flPrintVC').hide();
    di_jq('#flPrintOAT').hide();

    if (chartVisualizationDecided == 'map') {
        di_jq('#mapLangLblDlPNG').show();
        di_jq('#mapLangLblDlXLS').show();
        di_jq('#mapLangLblDlKML').show();
    }
    else if (chartVisualizationDecided == 'table') {
        di_jq('#tblLangLblDlXLS').show();
        di_jq('#flPrintOAT').show();
    }
    else {
        //di_jq('#vcLangLblDlPDF').show();
        di_jq('#vcLangLblDlPNG').show();
        di_jq('#vcLangLblDlSVG').show();
        di_jq('#vcLangLblDlXLS').show();
        di_jq('#flPrintVC').show();
    }
}


/* function to create color picker */
function vcColorPicker(fieldid, value, img_path, bgimg_flag, label, labelID) {
    var stylemk = 'cursor:pointer;width:24px;height:20px;float:left;';
    if (bgimg_flag == 'y') {
        stylemk += 'background:url(' + img_path + '/icon_colorp.png) no-repeat;';
    }
    return_ui = '<table cellpadding="0" cellspacing="1" border="0"><tr><td width="30px"><div id="' + fieldid + '" style="' + stylemk + '"><div id="inp_' + fieldid + '" style="border:1px solid #d3d3d3;position:relative;top:13px;left:1px;font-size:1px;width:18px;height:4px;background-color:' + value + ';"></div></div></td>';
    if (bgimg_flag == 'y') {
        return_ui += '<td><p id="' + labelID + '"> ' + label + '</p></td>';
    }
    return_ui += '</tr></table>';
    document.write(return_ui);
}
function openSFormatDiv() {
    di_jq('#sFormatDiv').slideToggle();

    di_jq('#sbdrThinDiv').hide();
    di_jq('#sbdrSolidDiv').hide();
}
function openSbdrThinDiv() {
    di_jq('#sbdrThinDiv').slideToggle();

    di_jq('#sFormatDiv').hide();
    di_jq('#sbdrSolidDiv').hide();
}
function openSbdrSolidDiv() {
    di_jq('#sbdrSolidDiv').slideToggle();

    di_jq('#sbdrThinDiv').hide();
    di_jq('#sFormatDiv').hide();
}
function openRotaionDiv() {
    di_jq('#rotaionDiv').slideToggle();
}
/*function openVisPropertyPopUp() {
ApplyMaskingDiv();
//Set close button at right corner of popup div
SetCloseButtonInPopupDiv(di_jq('#dv_container_popup'),'closeVisPropertyPopUp');
di_jq('#dv_container_popup').show('slow');
GetWindowCentered(z('dv_container_popup'), 580, 440);
}*/
function openVisPropertyPopUp(action) {
    if (chartVisualizationDecided == "treemap")
        return;
    var htmlui = '';
    z('popupTab').value = action;
    var modiTxt = di_jq('#lngMapPopModifySetting').html();
    var nofillTxt = di_jq('#lngNoFillTxt').html();
    var solidFillTxt = di_jq('#lngSolidFillTxt').html();
    var gradientFillTxt = di_jq('#lngGradientFillTxt').html();
    var fillColorTxt = di_jq('#lngFillColorTxt').html();
    var Color2Txt = di_jq('#lngColor2Txt').html();
    // getting text from body part
    var genTxt = di_jq('#lngMapPopGeneral').html();
    var bdrTxt = di_jq('#lngMapPopBorder').html();
    var showBdrTxt = di_jq('#lngMapPopShowBorder').html();
    var bdrWidthTxt = di_jq('#lngMapPopWidth').html();
    var bdrColorTxt = di_jq('#lngMapPopColor').html();
    var bdrStyleTxt = di_jq('#lngMapPopStyle').html();
    var gridlineTxt = di_jq('#lngGridlineTxt').html();
    var chartAreaTxt = di_jq('#lngChartAreaTxt').html();
    var plotAreaTxt = di_jq('#lngPlotAreaTxt').html();
    var legendTxt = di_jq('#lngMapPopLegend').html();
    var radiusTxt = di_jq('#lngRadiusTxt').html();
    var selGLAxisTxt = di_jq('#lngSelGLAxisTxt').html();
    var xAxisTxt = di_jq('#lngXAxisTxt').html();
    var yAxisTxt = di_jq('#lngYAxisTxt').html();
    var majorGLTxt = di_jq('#lngMajorGLTxt').html();
    var minorGLTxt = di_jq('#lngMinorGLTxt').html();
    var bgColorTxt = di_jq('#lngBgColorTxt').html();

    var showLegendTxt = di_jq('#lngShowLegendTxt').html();
    var BdrWidthTxt = di_jq('#lngBdrWidthTxt').html();
    var bdrColorTxt = di_jq('#lngBdrColorTxt').html();
    var bdrRadiusTxt = di_jq('#lngBdrRadiusTxt').html();
    var fontSizeTxt = di_jq('#lngFontSizeTxt').html();
    var positionTxt = di_jq('#lngPositionTxt').html();
    var floatingTxt = di_jq('#lngFloatingTxt').html();
    var xPosTxt = di_jq('#lngXPosTxt').html();
    var yPosTxt = di_jq('#lngYPosTxt').html();
    var fixedTxt = di_jq('#lngFixedTxt').html();
    var lengendWidthTxt = di_jq('#lngLengendWidthTxt').html();
    var autoTxt = di_jq('#lngAutoTxt').html();
    var optionTxt = di_jq('#lngOptionTxt').html();
    var seriesTypeTxt = di_jq('#lngSeriesTypeTxt').html();
    var lineTxt = di_jq('#lngLineTxt').html();
    var columnTxt = di_jq('#lngColumnTxt').html();
    var radialAxisTxt = di_jq('#lngRadialAxisTxt').html();
    var horizontalTxt = di_jq('#lngHorizontalTxt').html();
    var verticalTxt = di_jq('#lngVerticalTxt').html();
    var bothTxt = di_jq('#lngBothTxt').html();
    var noneTxt = di_jq('#lngNoneTxt').html();
    var formsTxt = di_jq('#lngFormsTxt').html();
    var segmentTxt = di_jq('#lngSegmentTxt').html();
    var curveTxt = di_jq('#lngCurveTxt').html();
    var charTypeTxt = di_jq('#lngCharTypeTxt').html();
    var circularTxt = di_jq('#lngCircularTxt').html();
    var polygonalTxt = di_jq('#lngPolygonalTxt').html();
    var leftTxt = di_jq('#lngLeftTxt').html();
    var rightTxt = di_jq('#lngRightTxt').html();
    var topTxt = di_jq('#lngTopTxt').html();
    var bottomTxt = di_jq('#lngBottomTxt').html();
    var seriesOptionTxt = di_jq('#lngSeriesOptionTxt').html();

    switch (action) {
        case "general":



            /* change header */
            di_jq('#dv_sec_pop').html('<h1>' + genTxt + '</h1><h4>' + modiTxt + '</h4>');
            if (chartVisualizationDecided != "radar") {
                /* change header */


                /* change left */
                htmlui = '<ul class="left_button"><li><a id="popTabbdr" href="javascript:void(0)" class="selected" onclick=\'clickPopPanel("popTabbdr")\'>' + bdrTxt + '</a></li><li><a id="popTabgrid" href="javascript:void(0)" onclick=\'clickPopPanel("popTabgrid")\'>' + gridlineTxt + '</a></li><li><a id="popTabcarea" href="javascript:void(0)" onclick=\'clickPopPanel("popTabcarea")\'>' + chartAreaTxt + '</a></li><li><a id="popTabparea" href="javascript:void(0)" onclick=\'clickPopPanel("popTabparea")\'>' + plotAreaTxt + '</a></li><li><a id="popTablegend" href="javascript:void(0)" onclick=\'clickPopPanel("popTablegend")\'>' + legendTxt + '</a></li></ul>';
                di_jq('#dv_sec_left').html(htmlui);

                /* change right */
                htmlui = '<div class="dv_sec_right_ppup_hdn_pos"><fieldset class="fieldset"><legend id="popfieldSet">' + bdrTxt + '</legend></fieldset></div>		 <div id="popTabbdr_sec" class="panelContainer" style="display:block;"><table width="90%" cellpadding="0" cellspacing="0"><tr><td colspan="2" height="30" valign="top"><input id="popBorderChkbox" type="checkbox"> ' + showBdrTxt + '</td></tr><tr><td width="20%"><p>' + bdrWidthTxt + '</p></td><td width="10%"><input id="popBorderWidthTxt" type="text" class="vcInputTxtBox" style="width:50px;"></td><td width="70%" style="padding-left:5px;"><div class="vcpopNumCountIcon vcpopNumCountIcon_up" onclick=\'plusMinusCount("+", "popBorderWidthTxt")\'></div><div class="vcpopNumCountIcon vcpopNumCountIcon_down" onclick=\'plusMinusCount("-", "popBorderWidthTxt")\'></div></td></tr><tr><td><p>' + bdrColorTxt + '</p></td><td><table cellpadding="0" cellspacing="0"><tr><td><input id="popchartBoderColor" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor10" style="cursor:pointer;"></td></tr></table></td></tr><tr><td><p>' + radiusTxt + '</p></td><td><input id="popBorderRadiusTxt" type="text" class="vcInputTxtBox" style="width:50px;"></td><td style="padding-left:5px;"><div class="vcpopNumCountIcon vcpopNumCountIcon_up" onclick=\'plusMinusCount("+", "popBorderRadiusTxt")\'></div><div class="vcpopNumCountIcon vcpopNumCountIcon_down" onclick=\'plusMinusCount("-", "popBorderRadiusTxt")\'></div></td></tr></table></div><div id="popTabgrid_sec" class="panelContainer"><table width="60%" cellpadding="0" cellspacing="0"><tr><td height="30" colspan="3" valign="top"><p>' + selGLAxisTxt + ' &nbsp;&nbsp; <select id="popAxisDdl" class="vcInputSelBox" style="width:70px" onchange=\'callChartProperty("popAxis",this.value)\'><option value="1">' + xAxisTxt + '</option><option value="2">' + yAxisTxt + '</option></select></p></td></tr><tr><td height="25" colspan="3"><p><input id="popMajorGridChkbox" onchange=\'callChartProperty("popupGLEnabled",this.checked)\' type="checkbox"> &nbsp;' + majorGLTxt + '</p><div style="padding-left:20px;"><table><tr><td><p>' + bdrWidthTxt + '</p></td><td><input id="popGridLineWidthTxt" onchange=\'callChartProperty("popupGLWidth",this.value)\' type="text" class="vcInputTxtBox" style="width:50px;"></td><td style="padding-left:5px;"><div class="vcpopNumCountIcon vcpopNumCountIcon_up" onclick=\'plusMinusCount("+", "popGridLineWidthTxt")\'></div><div class="vcpopNumCountIcon vcpopNumCountIcon_down" onclick=\'plusMinusCount("-", "popGridLineWidthTxt")\'></div></td></tr>  <tr><td><p>' + bdrColorTxt + '</p></td><td><table cellpadding="0" cellspacing="0"><tr><td><input id="popGridLineColorTxt" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor12" style="cursor:pointer;"></td></tr></table></td></tr><tr><td><p>' + bdrStyleTxt + '</p></td><td colspan="2"><select id="popMajorLineStyle" onchange=\'callChartProperty("popupGLStyle",this.value)\' class="vcInputSelBox" style="width:76px;"></select></td></tr></table></div></td></tr><tr><td height="25" colspan="3"><br/><br/><p><input id="popXMinorLineChkbox" onchange=\'callChartProperty("popupMGLEnabled",this.checked)\' type="checkbox"> &nbsp;' + minorGLTxt + '</p><div style="padding-left:20px;"><table><tr>		   <td><p>' + bdrWidthTxt + '</p></td><td><input id="popMinorXLineWidthTxt" onchange=\'callChartProperty("popupMGLWidth",this.value)\' type="text" class="vcInputTxtBox" style="width:50px;"></td><td><div class="vcpopNumCountIcon vcpopNumCountIcon_up" onclick=\'plusMinusCount("+", "popMinorXLineWidthTxt")\'></div><div class="vcpopNumCountIcon vcpopNumCountIcon_down" onclick=\'plusMinusCount("-", "popMinorXLineWidthTxt")\'></div></td></tr><tr><td><p>' + bdrColorTxt + '</p></td><td><table cellpadding="0" cellspacing="0"><tr><td><input id="popMinorXLineColorTxt" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor11" style="cursor:pointer;"></td></tr></table></td></tr><tr><td><p>' + bdrStyleTxt + '</p></td><td colspan="2"><select id="popMinorLineStyle" onchange=\'callChartProperty("popupMGLStyle",this.value)\' class="vcInputSelBox" style="width:76px;"></select></td></tr></table></div></td></tr></table></div><div id="popTabcarea_sec" class="panelContainer"><table width="60%" cellpadding="0" cellspacing="0"><tr><td height="30" colspan="3" valign="top"><p><b>' + bgColorTxt + '</b></p></td></tr><tr><td height="25" colspan="3"><p><input id="popChartBgNoColor" name="cafillcolor" type="radio" onclick=\'chartAreabgColor("no")\' checked> &nbsp;' + nofillTxt + '</p><p><input id="popChartBgColor" name="cafillcolor" type="radio" onclick=\'chartAreabgColor("solid")\'> &nbsp;' + solidFillTxt + '</p><p><input id="popChartBgGColor" name="cafillcolor" type="radio" onclick=\'chartAreabgColor("grad")\'> &nbsp;' + gradientFillTxt + '</p><p><div style="border:1px solid #ccccc9;padding:10px;margin-left:20px;display:none;" id="cabgcolortbl"><table width="200"><tr><td width="20%" nowrap><p id="cafillcolorpt">' + fillColorTxt + '</p></td><td width="20%"><table cellpadding="0" cellspacing="0"><tr><td><input id="popChartBgColorTxt" type="text" class="vcInputTxtBox" style="width:20px;"></td><td width="60%" style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor13" style="cursor:pointer;"></td></tr></table></td></tr><tr id="cacolor2tr"><td><p>' + Color2Txt + '</p></td><td><table cellpadding="0" cellspacing="0"><tr><td><input id="popChartBgColorTxt2" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor14" style="cursor:pointer;"></td></tr></table></td></tr><tr id="castyletr"><td><p>' + bdrStyleTxt + '</p></td><td colspan="2"><select id="popChartBgStyleDdl" type="text" class="vcInputSelBox" style="width:100px;"></td></tr></table></div></p></td></tr></table></div><div id="popTabparea_sec" class="panelContainer"><table width="60%" cellpadding="0" cellspacing="0"><tr><td height="30" colspan="3" valign="top"><p><b>' + bgColorTxt + '</b></p></td></tr><tr><td height="25" colspan="3"><p><input id="popChartPBgNoColor" name="pafillcolor" type="radio" onclick=\'chartPAreabgColor("no")\' checked> &nbsp;' + nofillTxt + '</p><p><input id="popChartPBgColor" name="pafillcolor" type="radio" onclick=\'chartPAreabgColor("solid")\'> &nbsp;' + solidFillTxt + '</p><p><input id="popChartPBgGColor" name="pafillcolor" type="radio" onclick=\'chartPAreabgColor("grad")\'> &nbsp;' + gradientFillTxt + '</p><p><div style="border:1px solid #ccccc9;padding:10px;margin-left:20px;display:none;" id="pabgcolortbl"><table width="200"><tr><td width="20%" nowrap><p id="pafillcolorpt">' + fillColorTxt + '</p></td><td width="20%"><table cellpadding="0" cellspacing="0"><tr><td><input id="popChartPBgColorTxt" type="text" class="vcInputTxtBox" style="width:20px;"></td><td width="60%" style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor15" style="cursor:pointer;"></td></tr></table></td></tr><tr id="pacolor2tr"><td><p>' + Color2Txt + '</p></td><td><table cellspacing="0" cellpadding="0"><tr><td><input id="popChartPBgColorTxt2" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor16" style="cursor:pointer;"></td></tr></table></td></tr><tr id="pastyletr"><td><p>' + bdrStyleTxt + '</p></td><td colspan="2"><select id="popChartPBgColorDdl" class="vcInputSelBox" style="width:100px;"></select></td></tr></table></div></p></td></tr></table></div><div id="popTablegend_sec" class="panelContainer"><table width="90%" cellpadding="0" cellspacing="0"><tr><td width="6%"><input id="popLegendChkbox" type="checkbox"></td><td height="20" colspan="3"><p> ' + showLegendTxt + '</p></td></tr><tr><td width="6%"><input type="checkbox" id="popLegendBorderChkbox"></td>		    <td colspan="3"><p> ' + bdrTxt + '</p></td></tr><tr><td width="6%"></td><td width="30%"><p> ' + BdrWidthTxt + '</p></td><td width="15%"><input type="text" id="popLegendBorderTxt" class="vcInputTxtBox" style="width:50px;"></td><td style="padding-left:5px;"><div class="vcpopNumCountIcon vcpopNumCountIcon_up" onclick=\'plusMinusCount("+", "popLegendBorderTxt")\'></div><div class="vcpopNumCountIcon vcpopNumCountIcon_down" onclick=\'plusMinusCount("-", "popLegendBorderTxt")\'></div></td></tr><tr><td width="6%"></td><td><p> ' + bdrColorTxt + '</p></td>			<td><table cellpadding="0" cellspacing="0"><tr><td><input id="popLegendBorderColorTxt" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="cursor:pointer;padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor17"></td></tr></table></td></tr><tr><td width="6%"></td><td><p> ' + bdrRadiusTxt + '</p></td>			<td><input id="popLegendBorderRadiusTxt" type="text" class="vcInputTxtBox" style="width:50px;"></td><td style="padding-left:5px;"><div class="vcpopNumCountIcon vcpopNumCountIcon_up" onclick=\'plusMinusCount("+", "popLegendBorderRadiusTxt")\'></div><div class="vcpopNumCountIcon vcpopNumCountIcon_down" onclick=\'plusMinusCount("-", "popLegendBorderRadiusTxt")\'></div></td></tr><tr><td width="6%"><br/><input id="legendBgColorChkbox" type="checkbox"></td><td nowrap><br/><p> ' + bgColorTxt + '</p></td><td><br/><table cellpadding="0" cellspacing="0"><tr><td><input id="poplegendBgColorTxt" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor18" style="cursor:pointer;"></td></tr></table></td></tr><tr><td width="6%"></td><td><p> ' + fontSizeTxt + '</p></td>		<td colspan="2"><table cellpadding="0" cellspacing="0"><tr><td><select id="popLegendFontDdl" class="vcInputSelBox" style="width:50px;"></select> &nbsp;</td>			   <td><span class="vcTitleIcon vcTitleIcon_fontbig" onclick=\'increaseDdlValue(true)\'></span></td><td><span class="vcTitleIcon vcTitleIcon_fontsmall" onclick=\'increaseDdlValue(false)\'></span></td><td><span id="legend_bold" class="vcTitleIcon vcTitleIcon_bold" onclick=\'toggleLegendItemStyle(this,"b")\'></span></td><td><span id="legend_italic" class="vcTitleIcon vcTitleIcon_italic" onclick=\'toggleLegendItemStyle(this,"i")\'></span></td><td><span id="legend_underline" class="vcTitleIcon vcTitleIcon_underline" onclick=\'toggleLegendItemStyle(this,"u")\'></span></td><td><span id="dicolor19" class="vcTitleIcon vcTitleIcon_color"></span>';

                htmlui += '</td></tr></table></td></tr><tr><td colspan="4"><br/><p><b>' + positionTxt + '</b></p></td></tr><tr><td width="6%"></td>		    <td><p><input id="legendFloatingChkbox" name="rfloating" type="radio" checked> ' + floatingTxt + '</p></td><td colspan="2">' + xPosTxt + ' <input id="legendXTxt" type="text" class="vcInputTxtBox" style="width:30px;"> &nbsp; ' + yPosTxt + ' <input id="legendYTxt" type="text" class="vcInputTxtBox" style="width:30px;"></td>		   </tr><tr><td width="6%"></td><td><p><input id="legendFixedChkbox" name="rfloating" type="radio"> ' + fixedTxt + '</p></td><td colspan="2"><select id="popLengedPos" class="vcInputSelBox" style="width:69px;"></select></td></tr><tr><td colspan="2"><br/><p><b>' + lengendWidthTxt + '</b></p></td><td colspan="2"><br/><table><tr><td><input id="legendAutoLenChkbox" name="autoFix" type="radio" checked> ' + autoTxt + ' </td><td><input id="legendFixLenChkbox" name="autoFix" type="radio"> ' + fixedTxt + ' </td><td><input type="text" id="poplegendwdcount" class="vcInputTxtBox" style="width:50px;"></td><td style="padding-left:5px;"><div class="vcpopNumCountIcon vcpopNumCountIcon_up" onclick=\'plusMinusCount("+", "poplegendwdcount")\'></div><div class="vcpopNumCountIcon vcpopNumCountIcon_down" onclick=\'plusMinusCount("-", "poplegendwdcount")\'></div></td></tr></table></td></tr></table></div>';
                di_jq('#dv_sec_right').html(htmlui);
            }
            else {

                /* change left */
                htmlui = '<ul class="left_button"><li><a id="popTabbdr" href="javascript:void(0)" class="selected" onclick=\'clickPopPanel("popTabbdr")\'>' + bdrTxt + '</a></li><li><a id="popTabgrid" href="javascript:void(0)" onclick=\'clickPopPanel("popTabgrid")\'>' + optionTxt + '</a></li><li><a id="popTabcarea" href="javascript:void(0)" onclick=\'clickPopPanel("popTabcarea")\'>' + chartAreaTxt + '</a></li><li><a id="popTablegend" href="javascript:void(0)" onclick=\'clickPopPanel("popTablegend")\'>' + legendTxt + '</a></li></ul>';
                di_jq('#dv_sec_left').html(htmlui);


                /* change right */
                htmlui = '<div class="dv_sec_right_ppup_hdn_pos"><fieldset class="fieldset"><legend id="popfieldSet">' + bdrTxt + '</legend></fieldset></div>		 <div id="popTabbdr_sec" class="panelContainer" style="display:block;"><table width="90%" cellpadding="0" cellspacing="0"><tr><td colspan="2" height="30" valign="top"><input id="popBorderChkbox" type="checkbox"> ' + showBdrTxt + '</td></tr><tr><td width="20%"><p>' + bdrWidthTxt + '</p></td><td width="10%"><input id="popBorderWidthTxt" type="text" class="vcInputTxtBox" style="width:50px;"></td><td width="70%" style="padding-left:5px;"><div class="vcpopNumCountIcon vcpopNumCountIcon_up" onclick=\'plusMinusCount("+", "popBorderWidthTxt")\'></div><div class="vcpopNumCountIcon vcpopNumCountIcon_down" onclick=\'plusMinusCount("-", "popBorderWidthTxt")\'></div></td></tr><tr><td><p>' + bdrColorTxt + '</p></td><td><table cellpadding="0" cellspacing="0"><tr><td><input id="popchartBoderColor" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor10" style="cursor:pointer;"></td></tr></table></td></tr><tr><td><p>' + radiusTxt + '</p></td><td><input id="popBorderRadiusTxt" type="text" class="vcInputTxtBox" style="width:50px;"></td><td style="padding-left:5px;"><div class="vcpopNumCountIcon vcpopNumCountIcon_up" onclick=\'plusMinusCount("+", "popBorderRadiusTxt")\'></div><div class="vcpopNumCountIcon vcpopNumCountIcon_down" onclick=\'plusMinusCount("-", "popBorderRadiusTxt")\'></div></td></tr></table></div><div id="popTabgrid_sec" class="panelContainer"><table width="60%" cellpadding="0" cellspacing="0"><tr><td height="30" colspan="3" valign="top"><p><table cellpadding="0" cellspacing="0"><tr><td>' + seriesTypeTxt + '&nbsp;&nbsp;</td><td><input id="vcpoplineradio" type="radio" name="seriestype" value="l"/>&nbsp;' + lineTxt + '&nbsp;&nbsp; <input id="vcpopcolumnradio" type="radio" name="seriestype" value="c"/>&nbsp;' + columnTxt + '</td></tr></table>  </p></td></tr><tr><td><p><table cellpadding="0" cellspacing="0"><tr><td>' + radialAxisTxt + '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><select id="vcpopRadialAxisddl"><option value="h">' + horizontalTxt + '</option><option value="v">' + verticalTxt + '</option><option value="b">' + bothTxt + '</option><option value="n">' + noneTxt + '</option></select></td></tr></table></p></td></tr><tr><td><p><table cellpadding="0" cellspacing="0"><tr><td>' + formsTxt + ' &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td><td><select id="vcpopFormDdl"><option value="s">' + segmentTxt + '</option><option value="c">' + curveTxt + '</option></select></td></tr></table></p></td></tr><tr><td>' + charTypeTxt + ' &nbsp;&nbsp; <input id="vcpopCircularradio" type="radio" name="charttype" value="c"/>' + circularTxt + '&nbsp;<input id="vcpopPolygonalradio" type="radio" name="charttype" value="p"/>' + polygonalTxt + '</td></tr></table></div><div style="padding-left:20px;"></div><div id="popTabcarea_sec" class="panelContainer"><table width="60%" cellpadding="0" cellspacing="0"><tr><td height="30" colspan="3" valign="top"><p><b>' + bgColorTxt + '</b></p></td></tr><tr><td height="25" colspan="3"><p><input id="popChartBgNoColor" name="cafillcolor" type="radio" onclick=\'chartAreabgColor("no")\' checked> &nbsp;' + nofillTxt + '</p><p><input id="popChartBgColor" name="cafillcolor" type="radio" onclick=\'chartAreabgColor("solid")\'> &nbsp;' + solidFillTxt + '</p><p><div style="border:1px solid #ccccc9;padding:10px;margin-left:20px;display:none;" id="cabgcolortbl"><table width="200"><tr><td width="20%" nowrap><p id="cafillcolorpt">' + fillColorTxt + '</p></td><td width="20%"><table cellpadding="0" cellspacing="0"><tr><td><input id="popChartBgColorTxt" type="text" class="vcInputTxtBox" style="width:20px;"></td><td width="60%" style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor13" style="cursor:pointer;"></td></tr></table></td></tr><tr id="cacolor2tr"><td><p>' + Color2Txt + '</p></td><td><table cellpadding="0" cellspacing="0"><tr><td><input id="popChartBgColorTxt2" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor14" style="cursor:pointer;"></td></tr></table></td></tr><tr id="castyletr"><td><p>' + bdrStyleTxt + '</p></td><td colspan="2"><select id="popChartBgStyleDdl" type="text" class="vcInputSelBox" style="width:100px;"></td></tr></table></div></p></td></tr></table></div><div id="popTabparea_sec" class="panelContainer"><table width="60%" cellpadding="0" cellspacing="0"><tr><td height="30" colspan="3" valign="top"><p><b>' + bgColorTxt + '</b></p></td></tr><tr><td height="25" colspan="3"><p><input id="popChartPBgNoColor" name="pafillcolor" type="radio" onclick=\'chartPAreabgColor("no")\' checked> &nbsp;' + nofillTxt + '</p><p><input id="popChartPBgColor" name="pafillcolor" type="radio" onclick=\'chartPAreabgColor("solid")\'> &nbsp;' + solidFillTxt + '</p><p><div style="border:1px solid #ccccc9;padding:10px;margin-left:20px;display:none;" id="pabgcolortbl"><table width="200"><tr><td width="20%" nowrap><p id="pafillcolorpt">' + fillColorTxt + '</p></td><td width="20%"><table cellpadding="0" cellspacing="0"><tr><td><input id="popChartPBgColorTxt" type="text" class="vcInputTxtBox" style="width:20px;"></td><td width="60%" style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor15" style="cursor:pointer;"></td></tr></table></td></tr><tr id="pacolor2tr"><td><p>' + Color2Txt + '</p></td><td><table cellspacing="0" cellpadding="0"><tr><td><input id="popChartPBgColorTxt2" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor16" style="cursor:pointer;"></td></tr></table></td></tr><tr id="pastyletr"><td><p>' + bdrStyleTxt + '</p></td><td colspan="2"><select id="popChartPBgColorDdl" class="vcInputSelBox" style="width:100px;"></select></td></tr></table></div></p></td></tr></table></div><div id="popTablegend_sec" class="panelContainer"><table width="90%" cellpadding="0" cellspacing="0"><tr><td width="6%"><input id="popLegendChkbox" type="checkbox"></td><td height="20" colspan="3"><p> ' + showLegendTxt + '</p></td></tr><tr><td width="6%"><input type="checkbox" id="popLegendBorderChkbox"></td>		    <td colspan="3"><p> ' + bdrTxt + '</p></td></tr><tr><td width="6%"></td><td width="30%"><p> ' + BdrWidthTxt + '</p></td><td width="15%"><input type="text" id="popLegendBorderTxt" class="vcInputTxtBox" style="width:50px;"></td><td style="padding-left:5px;"><div class="vcpopNumCountIcon vcpopNumCountIcon_up" onclick=\'plusMinusCount("+", "popLegendBorderTxt")\'></div><div class="vcpopNumCountIcon vcpopNumCountIcon_down" onclick=\'plusMinusCount("-", "popLegendBorderTxt")\'></div></td></tr><tr><td width="6%"></td><td><p> ' + bdrColorTxt + '</p></td>			<td><table cellpadding="0" cellspacing="0"><tr><td><input id="popLegendBorderColorTxt" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="cursor:pointer;padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor17"></td></tr></table></td></tr><tr><td width="6%"></td><td><p> ' + bdrRadiusTxt + '</p></td>			<td><input id="popLegendBorderRadiusTxt" type="text" class="vcInputTxtBox" style="width:50px;"></td><td style="padding-left:5px;"><div class="vcpopNumCountIcon vcpopNumCountIcon_up" onclick=\'plusMinusCount("+", "popLegendBorderRadiusTxt")\'></div><div class="vcpopNumCountIcon vcpopNumCountIcon_down" onclick=\'plusMinusCount("-", "popLegendBorderRadiusTxt")\'></div></td></tr><tr><td width="6%"><br/><input id="legendBgColorChkbox" type="checkbox"></td><td nowrap><br/><p> ' + bgColorTxt + '</p></td><td><br/><table cellpadding="0" cellspacing="0"><tr><td><input id="poplegendBgColorTxt" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor18" style="cursor:pointer;"></td></tr></table></td></tr>';

                htmlui += '<tr><td colspan="4"><br/><p><b>' + positionTxt + '</b></p></td></tr><tr><td width="6%"></td><td><p>' + fixedTxt + '</p></td><td colspan="2"><select id="popLengedPos" class="vcInputSelBox" style="width:69px;"><option value="l">' + leftTxt + '</option><option value="r">' + rightTxt + '</option><option value="t">' + topTxt + '</option><option value="b">' + bottomTxt + '</option></select></td></tr></table></div>';
                di_jq('#dv_sec_right').html(htmlui);
            }
            break;
        case "title":
            break;
        case "label":
            break;
        case "series":
            var lngpopSeriesTxt = di_jq('#lngMapPopThemeSeries').html();
            var fillTxt = di_jq('#lngFillTxt').html();
            var overlapTxt = di_jq('#lngOverlapTxt').html();
            var gapTxt = di_jq('#lngGapTxt').html();


            if (chartVisualizationDecided != "radar") {
                /* change header */
                di_jq('#dv_sec_pop').html('<h1>' + lngpopSeriesTxt + '</h1><h4>' + modiTxt + '</h4>');

                /* change left */
                htmlui = '<ul class="left_button"><li><a id="popTabSrOp" href="javascript:void(0)" class="selected" onclick=\'clickPopPanel("popTabSrOp")\'>' + seriesOptionTxt + '</a></li><li><a id="popTabSrFill" href="javascript:void(0)" onclick=\'clickPopPanel("popTabSrFill")\'>' + fillTxt + '</a></li></ul>';
                di_jq('#dv_sec_left').html(htmlui);

                /* change right */
                htmlui = '<div class="dv_sec_right_ppup_hdn_pos"><fieldset class="fieldset"><legend id="popfieldSetSr">' + seriesOptionTxt + '</legend></fieldset></div>		 <div id="popTabSrOp_sec" class="panelContainer" style="display:block;"><table width="90%" cellpadding="0" cellspacing="0"><tr><td width="20%"><p>' + overlapTxt + '</p></td><td width="20%"><div id="sliderId1" style="background-color:#e2e2e2;"></div></td><td width="60%" class="vcInputTxtBox_pos"><input type="text" id="srOverlapNum" class="vcInputTxtBox" style="width:30px;" ></td></tr><tr><td width="20%"><p>' + gapTxt + '</p></td><td width="20%"><div id="sliderId2" style="background-color:#e2e2e2;"></div></td><td width="60%" class="vcInputTxtBox_pos"><input type="text" id="srGapNum" class="vcInputTxtBox" style="width:30px;"></td></tr></table></div><div id="popTabSrFill_sec" class="panelContainer"><table width="60%" cellpadding="0" cellspacing="0"><tr><td height="30" colspan="3" valign="top"><p><select id="popSeriesDdl" class="vcInputSelBox" onchange=\'PopupChangeSeries(this.value)\' style="width:100px;"></select></p></td><tr><td height="25" colspan="3"><p><input id="popSeriesNoColor" onchange=\'callChartProperty("popupSeriesNoColor",this.value)\' name="seriesfillcolor" type="radio" onclick=\'SeriesOptColor("no")\' checked> &nbsp;' + nofillTxt + '</p><p><input id="popSeriesSolidColor" name="seriesfillcolor" type="radio" onclick=\'SeriesOptColor("solid")\'> &nbsp;' + solidFillTxt + '</p><p><input id="popSeriesGColor" name="seriesfillcolor" type="radio" onclick=\'SeriesOptColor("grad")\'> &nbsp;' + gradientFillTxt + '</p><p><div style="border:1px solid #ccccc9;padding:10px;margin-left:20px;display:none;" id="srbgcolortbl"><table width="200"><tr><td width="20%" nowrap><p id="srfillcolorpt">' + fillColorTxt + '</p></td><td><table cellpadding="0" cellspacing="0"><tr><td><input id="popSeriesColorTxt" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor20" style="cursor:pointer;"></td></tr></table></td></tr><tr id="srcolor2tr"><td><p>' + Color2Txt + '</p></td><td><table cellpadding="0" cellspacing="0"><tr><td><input id="popSeriesColorTxt2" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor21" style="cursor:pointer;"></td></tr></table></td></tr><tr id="srstyletr"><td><p>' + bdrStyleTxt + '</p></td><td colspan="2"><select id="popSeriesStyleTxt" onchange=\'callChartProperty("popupSeriesGCStyle",this.value)\' type="text" class="vcInputSelBox" style="width:100px;"></td></tr></table></div></p><p><div><table cellpadding="0" cellspacing="0"><tr><td><p>' + bdrWidthTxt + '</p></td><td><input id="popSeriesWidthTxt" onchange=\'callChartProperty("popupSeriesWidth",this.value)\' type="text" class="vcInputTxtBox" style="width:50px;"></td><td style="padding-left:5px;"><div class="vcpopNumCountIcon vcpopNumCountIcon_up" onclick=\'plusMinusCount("+", "popSeriesWidthTxt")\'></div><div class="vcpopNumCountIcon vcpopNumCountIcon_down" onclick=\'plusMinusCount("-", "popSeriesWidthTxt")\'></div></td></tr><tr><td><p>' + bdrColorTxt + '</p></td><td><table cellpadding="0" cellspacing="0"><tr><td><input id="popSeriesBdrColor" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor22" style="cursor:pointer;"></td></tr></table></td></tr><tr><td><p>' + bdrStyleTxt + '</p></td><td colspan="2"><select id="popSeriesStyle"  onchange=\'callChartProperty("popupSeriesStyle",this.value)\' class="vcInputSelBox" style="width:75px;"></select></td></tr></table></div></p></td></tr></table></div>'
                di_jq('#dv_sec_right').html(htmlui);

                di_jq("#sliderId1").slider({
                    range: "max",
                    min: 0,
                    max: 29,
                    value: 0,
                    slide: function (event, ui) {
                        di_jq("#srOverlapNum").val(ui.value / 100);
                    }
                });
                di_jq("#sliderId2").slider({
                    range: "max",
                    min: 0,
                    max: 29,
                    value: 0,
                    slide: function (event, ui) {
                        di_jq("#srGapNum").val(ui.value / 100);
                    }
                });

            }
            else {
                /* change header */
                //di_jq('#dv_sec_pop').html('<h1>Series</h1><h4>Modify your settings here</h4>');

                /* change left */
                /*htmlui = '<ul class="left_button"><li><a id="popTabSrFill" href="javascript:void(0)" onclick=\'clickPopPanel("popTabSrFill")\'>Fill</a></li></ul>';
                di_jq('#dv_sec_left').html(htmlui);*/

                /* change right */
                /*htmlui = '<div style="padding:10px 0 0 20px;"><fieldset class="fieldset"><legend id="popfieldSetSr">Series Option</legend></fieldset></div>		 <div id="popTabSrFill_sec" style="display:block;" class="panelContainer"><table width="60%" cellpadding="0" cellspacing="0"><tr><td height="30" colspan="3" valign="top"><p><select id="popSeriesDdl" class="vcInputSelBox" onchange=\'PopupChangeSeries(this.value)\' style="width:100px;"></select></p></td><tr><td height="25" colspan="3"><p><input id="popSeriesNoColor" onchange=\'callChartProperty("popupSeriesNoColor",this.value)\' name="seriesfillcolor" type="radio" onclick=\'SeriesOptColor("no")\' checked> &nbsp;No Fill</p><p><input id="popSeriesSolidColor" name="seriesfillcolor" type="radio" onclick=\'SeriesOptColor("solid")\'> &nbsp;Solid Fill</p><div style="border:1px solid #ccccc9;padding:10px;margin-left:20px;display:none;" id="srbgcolortbl"><table width="200"><tr><td width="20%" nowrap><p id="srfillcolorpt">Fill Color</p></td><td><table cellpadding="0" cellspacing="0"><tr><td><input id="popSeriesColorTxt" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor20" style="cursor:pointer;"></td></tr></table></td></tr><tr id="srcolor2tr"><td><p>Color 2</p></td><td><table cellpadding="0" cellspacing="0"><tr><td><input id="popSeriesColorTxt2" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor21" style="cursor:pointer;"></td></tr></table></td></tr><tr id="srstyletr"><td><p>Style</p></td><td colspan="2"><select id="popSeriesStyleTxt" onchange=\'callChartProperty("popupSeriesGCStyle",this.value)\' type="text" class="vcInputSelBox" style="width:100px;"></td></tr></table></div></p><p><div><table cellpadding="0" cellspacing="0"><tr><td><p>Border Color</p></td><td><table cellpadding="0" cellspacing="0"><tr><td><input id="popSeriesBdrColor" type="text" class="vcInputTxtBox" style="width:20px;"></td><td style="padding-left:10px;"><img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor22" style="cursor:pointer;"></td></tr></table></td></tr></table></div></p></td></tr></table></div>'
                di_jq('#dv_sec_right').html(htmlui);*/
                return;
            }
            break
    }
    di_jq('#btnApply').show();
    ApplyMaskingDiv();
    //Set close button at right corner of popup div
    SetCloseButtonInPopupDiv(di_jq('#dv_container_popup'), 'closeVisPropertyPopUp');
    di_jq('#dv_container_popup').show('slow');
    GetWindowCentered(z('dv_container_popup'), 580, 440);
    applyPopSetting(action);

    // initialte pop color picker
    popUpInitColorPic();
}
function plusMinusCount(act, fieldid) {
    var curVal = di_jq('#' + fieldid).val();
    if (curVal == '' || curVal == null)
        curVal = 0;
    if (act == '+') {
        di_jq('#' + fieldid).val(eval(curVal) + 1);
    }
    else if (act == '-' && curVal > 0) {
        di_jq('#' + fieldid).val(curVal - 1);
    }
}
function chartAreabgColor(action) {
    if (action == 'no') {
        di_jq('#cabgcolortbl').hide('slow');
    }
    else if (action == 'solid') {
        di_jq('#cabgcolortbl').show();
        di_jq('#cacolor2tr').hide();
        di_jq('#castyletr').hide();
        di_jq('#cafillcolorpt').html(z("lngFillColorTxt").innerHTML);
    }
    else if (action == 'grad') {
        di_jq('#cabgcolortbl').show();
        di_jq('#cacolor2tr').show();
        di_jq('#castyletr').show();
        di_jq('#cafillcolorpt').html(z("lngColor1Txt").innerHTML);
    }
}
function chartPAreabgColor(action) {
    if (action == 'no') {
        di_jq('#pabgcolortbl').hide('slow');
    }
    else if (action == 'solid') {
        di_jq('#pabgcolortbl').show();
        di_jq('#pacolor2tr').hide();
        di_jq('#pastyletr').hide();
        di_jq('#pafillcolorpt').html(z("lngFillColorTxt").innerHTML);
    }
    else if (action == 'grad') {
        di_jq('#pabgcolortbl').show();
        di_jq('#pacolor2tr').show();
        di_jq('#pastyletr').show();
        di_jq('#pafillcolorpt').html(z("lngColor1Txt").innerHTML);
    }
}
function SeriesOptColor(action) {
    if (action == 'no') {
        di_jq('#srbgcolortbl').hide('slow');
    }
    else if (action == 'solid') {
        di_jq('#srbgcolortbl').show();
        di_jq('#srcolor2tr').hide();
        di_jq('#srstyletr').hide();
        di_jq('#srfillcolorpt').html(z("lngFillColorTxt").innerHTML);
    }
    else if (action == 'grad') {
        di_jq('#srbgcolortbl').show();
        di_jq('#srcolor2tr').show();
        di_jq('#srstyletr').show();
        di_jq('#srfillcolorpt').html(z("lngColor1Txt").innerHTML);
    }
}
function closeVisPropertyPopUp() {
    RemoveMaskingDiv();
    di_jq('#dv_container_popup').hide('slow');
    if (z('di_vcpborder') != undefined && z('popBorderChkbox') != undefined) {
        z('di_vcpborder').defaultChecked = z('popBorderChkbox').checked;
    }
}
function clickPopPanel(refId) {
    di_jq('.left_button li > a').each(function () { di_jq(this).removeClass('selected'); });
    di_jq('#' + refId).addClass('selected');
    di_jq('.panelContainer').hide();
    di_jq('#' + refId + '_sec').show();

    di_jq('#popfieldSet').html(di_jq('#' + refId).html());
}

function DecideVisualizationType(OatGrid) {

    //1. Indentify if Time dimension is available and multiple year value for same indicator (time series is available). If yes then show Line chart
    //2. If not Time series data then show Column chart if simple legend (one dimension) is formed
    //3. If complex legend show Table

    chartVisualizationDecided = 'table';

    var indexOfTP = OatGrid.headerRow.indexOf(diStructureTimePeriod);

    if (indexOfTP != -1) {

        switch (OatGrid.rowConditions.indexOf(indexOfTP)) {
            case (0):
                {
                    if (isMRD) chartVisualizationDecided = 'column';
                    else chartVisualizationDecided = 'line';
                    break;
                }
            case (1):
                {
                    if (!(isValidForColumnChart(OatGrid, 1) && isMRD == false) && OatGrid.colConditions.length < 2) {
                        chartVisualizationDecided = 'column';
                    }

                    break;
                }
            default:
                {
                    chartVisualizationDecided = 'table';
                    break;
                }
        }
    }


    else if (OatGrid.rowConditions.length == 1) // If only Area or Indicator is displayed in rowHeaders
    {
        // Then check if source exists in colHeaders
        //      If YES exists then check whether other exists also
        //          If NOT then draw COLUMN chart
        //          Else draw GRID
        //      Else draw COLUMN

        var indexSource = OatGrid.headerRow.indexOf(diStructureSource);

        if (
            (OatGrid.colConditions.indexOf(indexSource) != -1 && OatGrid.colConditions.length == 1)
            ||
            OatGrid.colConditions.indexOf(indexSource) == -1
             ) // ONLY Source exists or Source doesn't exists at all : then draw COLUMN
            chartVisualizationDecided = 'column';

    }

    chartVisualizationDecided = DecideVisTypes(OatGrid);
    if (OATfilePath != '' && isMapEnable) chartVisualizationDecided = 'map';
    changeVisulizerChart(chartVisualizationDecided);
    initFacts();
}



function isValidForColumnChart(OatGrid, TimePeriodIndex) {
    var isValidForColumn = false;
    for (var i = 0; i < OatGrid.conditions[TimePeriodIndex].distinctValues.length; i++) {
        var tmpTimePeriod = OatGrid.conditions[TimePeriodIndex].distinctValues[i];
        var tmpIndicatorsForTP = [];

        for (var j = 0; j < OatGrid.allData.length; j++) {
            if (OatGrid.allData[j][TimePeriodIndex] == tmpTimePeriod) // Found current time period
            {
                var tmpIndicator = OatGrid.allData[j][TimePeriodIndex - 1];
                if (tmpIndicatorsForTP.indexOf(tmpIndicator) == -1) tmpIndicatorsForTP.push(tmpIndicator);

                if (tmpIndicatorsForTP.length >= 2) {
                    isValidForColumn = true;
                    break;
                }
            }
        }
        if (isValidForColumn) break;
    }

    return isValidForColumn;
}


function renderOATfromCYV(DataContent, header, data, mainlist, sublist, lastcol) {
    if (DataContent.length == 1) {
        NoDataFound();
        return false;
    }

    isMRD = false;

    z('dataTitle').innerHTML = DataContent[1];
    var AllKeyVals = DataContent[0].split("|");
    var Title = DataContent[1];
    header = DataContent[2].split("|");
    var KeysVals = new Array();
    var Contents = new Array();
    for (var i = 3; i < DataContent.length; i++) {
        Contents[i - 3] = DataContent[i];
    }

    for (var i = 0; i < AllKeyVals.length; i++) {
        var tmpKeyVal = AllKeyVals[i].split(":");
        KeysVals.push(tmpKeyVal[0]);
        KeysVals.push(tmpKeyVal[1]);
    }

    for (var i = 0; i < Contents.length - 1; i++) {

        var tmpRow = Contents[i].split("|");

        for (var j = 0; j < tmpRow.length - 1; j++) {
            tmpRow[j] = getStringFromNId(KeysVals, tmpRow[j]);
        }


        var indexOfMRD = header.indexOf(diStructureMRD);
        if (indexOfMRD != -1) { // MRD information is provided by user
            if (tmpRow[indexOfMRD].toLowerCase() == "true") { // Is current row MRD
                dataMRD.push(tmpRow);
                isMRD = true;
            }
        }

        dataAll.push(tmpRow);
    }


    for (var i = 0; i < header.length - 2; i++) {
        if (header[i] != diStructureMRD) {
            mainlist.push(i);
        }

    }
    sublist.push(header.length - 2);

    // For no row headers of AREA, INDICATOR or TIME PERIOD simply swap the column headers to row headers

    if (mainlist.length == 0) {
        mainlist = sublist;
        sublist = [];
    }

    lastcol = header.length - 1;

    isFiltersRedrawNeeded = true;
    if (isMRD)
        var pivot = new OAT.Pivot("pivot_content", "", "pivot_page", header, dataMRD, mainlist, sublist, [], lastcol, { showChart: 0, showEmpty: 0 });
    else
        var pivot = new OAT.Pivot("pivot_content", "", "pivot_page", header, dataAll, mainlist, sublist, [], lastcol, { showChart: 0, showEmpty: 0 });

    togglePivot(z('pivotChk').checked);
    DecideVisualizationType(pivot);

}

function renderOATfromQDSorSelector(DataContent, header, data, mainlist, sublist, lastcol) {

    if (DataContent.length == 1) {
        NoDataFound();
        return false;
    }

    var AllJSONs = DataContent[0].split("}");

    for (var i = 0; i < AllJSONs.length; i++) {
        var currentJSON = AllJSONs[i];
        switch (i) {
            case 0:
                Areas = JSonToArray(currentJSON);
                break;
            case 1:
                Indicators = JSonToArray(currentJSON);
                break;
            case 2:
                Units = JSonToArray(currentJSON);
                break;
            case 3:
                Sources = JSonToArray(currentJSON);
                break;
            case 4:
                IUSs = JSonToArray(currentJSON);
                break;
            case 5:
                strDimensionSubgroups = currentJSON.substring(1);
                break;
        }
    }

    updateTitles(DataContent[1]);
    header = DataContent[2].split("|");
    header.splice(header.length - 2, 1); // Remove footer NID from headers
    header[header.indexOf("Area_ID")] = diStructureAreaID;


    var Contents = new Array();
    for (var i = 3; i < DataContent.length; i++) {
        Contents[i - 3] = DataContent[i];
    }

    dataAll = [];
    dataMRD = [];

    for (var i = 0; i < Contents.length; i++) {

        var tmpRow = Contents[i].split("|");

        for (var j = 0; j < tmpRow.length; j++) {

            switch (j) {
                case header.indexOf(diStructureArea):
                    tmpRow[j] = getStringFromNId(Areas, tmpRow[j]);
                    break;
                case header.indexOf(diStructureTopic):
                    tmpRow[j] = getStringFromNId(Indicators, tmpRow[j]);
                    break;
                case header.indexOf(diStructureSource):
                    tmpRow[j] = getStringFromNId(Sources, tmpRow[j]);
                    break;
                case tmpRow.length - 2:
                    {
                        var nowDataVal = tmpRow[j - 1];
                        if (maxDataVal < nowDataVal) maxDataVal = nowDataVal;
                        if (minDataVal > nowDataVal) minDataVal = nowDataVal;
                        if (tmpRow[j] != '-1') { // If footernote column is traversed

                            nowDataVal = "<a href='javascript:void(0);' onmouseout='javascript:HideCallout()' onmouseover='javascript:getFootNote(" + tmpRow[j] + ", event)'>" + tmpRow[j + 1] + "</a>";
                            tmpRow[j + 1] = nowDataVal;
                            j++;
                        }
                    }

            }
        }
        tmpRow.splice(tmpRow.length - 2, 1);

        if (tmpRow[header.indexOf(diStructureMRD)] == "1") dataMRD.push(tmpRow);
        dataAll.push(tmpRow);
    }

    var totc = "";
    lastcol = header.length - 1;

    var diFixedColumns = [diStructureArea, diStructureTopic, diStructureTimePeriod]; //diStructureMRD, "Area_ID",

    for (var c = 0; c < header.length; c++) {
        if (diFixedColumns.indexOf(header[c]) != -1) {
            if (header[c] == diStructureArea && Areas.length <= 2) { }
            else if (header[c] == diStructureTopic && Indicators.length <= 2) { }
            else mainlist.push(c);
        }
        else if (header[c] != diStructureMRD && header[c] != diStructureAreaID && !(dvHideSource == 'true' && header[c] == diStructureSource)) sublist.push(c);

    }
    sublist.splice(sublist.length - 1, 1);

    // For no row headers of AREA, INDICATOR or TIME PERIOD simply swap the column headers to row headers

    if (mainlist.length == 0) {
        mainlist = sublist;
        sublist = [];
    }


    var pivot;
    isFiltersRedrawNeeded = true;

    if (dataAll.length >= dvMrdThreshold) {
        isMRD = true;
        pivot = new OAT.Pivot("pivot_content", "", "pivot_page", header, dataMRD, mainlist, sublist, [], lastcol, { showChart: 0, showEmpty: 0 });
    }
    else {
        isMRD = false;

        // Start of code for swapping the first column header to row header
        var indexOfTP = mainlist.indexOf(header.indexOf(diStructureTimePeriod)); // Time period column is made only RowHeader for drawing meaningful chart

        if ((indexOfTP == 0 || indexOfTP == 1) && sublist.length == 0) {
            if (indexOfTP == 1) {
                mainlist.splice(0, 1);
                sublist.push(0);
            }
        }
        // End of code for swapping the first column header to row header

        pivot = new OAT.Pivot("pivot_content", "", "pivot_page", header, dataAll, mainlist, sublist, [], lastcol, { showChart: 0, showEmpty: 0 });
    }

    /* off pivoting bydefault */
    togglePivot(z('pivotChk').checked);
    DecideVisualizationType(pivot);

}



function removeOATrowHeader(clickedCheckbox) {
    var objOAT = OAT.getSelfInstance();
    var tmpIndex;
    try {
        if (clickedCheckbox.value.indexOf('row') == 0 || clickedCheckbox.value.indexOf('col') == 0) {
            tmpIndex = parseInt(clickedCheckbox.value.substring(3));
        }
        else {
            tmpIndex = clickedCheckbox.value;
        }
    }
    catch (err) {
        tmpIndex = clickedCheckbox.value;
    }

    if (clickedCheckbox.checked) {
        if (clickedCheckbox.value.indexOf("col") == 0) // If Col header is chosen
        {
            objOAT.colConditions.push(tmpIndex);
        }
        else if (clickedCheckbox.value.indexOf("row") == 0) {
            objOAT.rowConditions.push(tmpIndex);
        }
        else {
            objOAT.rowConditions.push(clickedCheckbox.value);
        }
        if (clickedCheckbox.name == "chkHideShowAreaID") z('divFilterAreaID').style.display = 'block';
    }

    else {
        var tmpPoint;
        if (objOAT.colConditions.indexOf(tmpIndex) != -1) // If current header exists in column
        {
            tmpPoint = objOAT.colConditions.indexOf(tmpIndex);
            objOAT.colConditions.splice(tmpPoint, 1);
        }
        else if (objOAT.rowConditions.indexOf(tmpIndex) != -1) // If current header exists in row
        {
            tmpPoint = objOAT.rowConditions.indexOf(tmpIndex);
            objOAT.rowConditions.splice(tmpPoint, 1);
        }
        else // If current header exists no where : DO NOTHING
        { }
        if (clickedCheckbox.name == "chkHideShowAreaID") z('divFilterAreaID').style.display = 'none';
    }

    isFiltersRedrawNeeded = false;
    objOAT.go();
    togglePivot(z('pivotChk').checked);
    // refresh chart
    showVisualizer(chartVisualizationDecided);

    getSortingOptions(z('optSorting'));

}


var OatAllCondtions = [];

function filterApplied(domElement) {

    var objOAT = OAT.getSelfInstance();
    var conditionDimensions = domElement.value.split(',');

    //START of code for disabling from Hide Columns panel

    var areAllFiltersChecked = true;

    for (var i = 0; i < objOAT.conditions[conditionDimensions[0]].distinctValues.length; i++) {
        if (z('chkFilter' + conditionDimensions[0] + i) != null && !z('chkFilter' + conditionDimensions[0] + i).checked) {
            areAllFiltersChecked = false;
            break;
        }
    }

    try {
        z('chkSourcerow' + conditionDimensions[0]).disabled = !areAllFiltersChecked;
    }
    catch (errRow) {

        try {
            z('chkSourcecol' + conditionDimensions[0]).disabled = !areAllFiltersChecked;
        }
        catch (errCol) {

            try {
                z('chkSource' + conditionDimensions[0]).disabled = !areAllFiltersChecked;
            }
            catch (err) {
            }

        }

    }
    //z('chkSourcerow' + conditionDimensions[0]).disabled = true;

    //END of code for disabling from Hide Columns panel

    var HeaderName = objOAT.conditions[conditionDimensions[0]].distinctValues[conditionDimensions[1]];
    if (!domElement.checked) {
        objOAT.conditions[conditionDimensions[0]].blackList.push(HeaderName);
    }
    else {
        var blackListedIndex = objOAT.conditions[conditionDimensions[0]].blackList.indexOf(HeaderName);
        objOAT.conditions[conditionDimensions[0]].blackList.splice(blackListedIndex, 1);
    }
    isFiltersRedrawNeeded = false;
    objOAT.go();
    if (objOAT.filteredData.length != 0) {
        togglePivot(z('pivotChk').checked);
        // refresh chart
    }

    // In case of map, apply filters from filters panel by making a special call with selected topics and areas.
    if (chartVisualizationDecided == 'map') {
        var SelectedTopicsAreas = getFilteredTopicsAreas(objOAT);
        requestToMapFilter(SelectedTopicsAreas[0], SelectedTopicsAreas[1]);
        return;
    }
    else if (chartVisualizationDecided == 'map2d' || chartVisualizationDecided == 'map3d') {
        var SelectedTopicsAreas = getFilteredTopicsAreas(objOAT);
        requestToMapFilter(SelectedTopicsAreas[0], SelectedTopicsAreas[1]);
    }

    showVisualizer(chartVisualizationDecided);

}

function getFilteredTopicsAreas(objectOfOAT) {

    var RetVal = [];

    RetVal[0] = '';
    RetVal[1] = '';

    var indexOfAreasColumn = objectOfOAT.headerRow.indexOf(diStructureArea);

    // Add all areas to array and then remove blacklist areas. By doing this, we get currently selected area names.
    var checkedAreaNames = objectOfOAT.conditions[indexOfAreasColumn].distinctValues.clone();
    var blackListedAreas = objectOfOAT.conditions[indexOfAreasColumn].blackList.clone();

    for (var i = 0; i < blackListedAreas.length; i++) {
        var indexOfBlackListedItem = checkedAreaNames.indexOf(blackListedAreas[i]);
        checkedAreaNames.splice(indexOfBlackListedItem, 1);
    }

    for (var i = 0; i < checkedAreaNames.length; i++) {
        RetVal[1] += ',' + getAreaNId(checkedAreaNames[i]);
    }



    var indexOfTopicsColumn = objectOfOAT.headerRow.indexOf(diStructureTopic);

    // Add all Topics to array and then remove blacklist Topics. By doing this, we get currently selected Topic names.
    var checkedTopicNames = objectOfOAT.conditions[indexOfTopicsColumn].distinctValues.clone();
    var blackListedTopics = objectOfOAT.conditions[indexOfTopicsColumn].blackList.clone();

    for (var i = 0; i < blackListedTopics.length; i++) {
        var indexOfBlackListedItem = checkedTopicNames.indexOf(blackListedTopics[i]);
        checkedTopicNames.splice(indexOfBlackListedItem, 1);
    }

    for (var i = 0; i < checkedTopicNames.length; i++) {
        RetVal[0] += ',' + getIUNId(checkedTopicNames[i]);
    }


    // Remove extra comma
    if (RetVal[0] != '') RetVal[0] = RetVal[0].substr(1);
    if (RetVal[1] != '') RetVal[1] = RetVal[1].substr(1);

    return RetVal;
}

function getAreaNId(AreaName) {

    var indexOfAreaName = Areas.indexOf(AreaName);
    indexOfAreaName--;

    return Areas[indexOfAreaName];
}

function getIUNId(IUName) {

    var indexOfTopicName = Indicators.indexOf(IUName);
    indexOfTopicName--;

    return Indicators[indexOfTopicName];
}


function showHideAllFilterPanes(isShown) {
    var objOAT = OAT.getSelfInstance();

    var tmpCssProperty = '';
    if (!isShown) tmpCssProperty = 'none';

    for (var i = 0; i < objOAT.headerRow.length; i++) {
        var tmpHeaderName = objOAT.headerRow[i];
        if (tmpHeaderName == diStructureAreaID) break;
        try {
            var tmpDivFilter = z('divFilter' + tmpHeaderName.replace(' ', ''));
            tmpDivFilter.style.display = tmpCssProperty;
        }
        catch (err) {
            var msg = err.Message;
        }
    }

    z('divSorting').style.display = tmpCssProperty;
    z('divHideColumns').style.display = tmpCssProperty;
    //z('divHelp').style.display = tmpCssProperty;
}


function sortAsc(a, b) {
    return a - b;
}

function sortDsc(a, b) {
    return b - a;
}

function ToggleMRD(chkBox) {
    isMRD = !isMRD;
    var objOAT = OAT.getSelfInstance();

    if (chkBox.checked) DrawGridFromOAT(objOAT, dataMRD, true);
    else DrawGridFromOAT(objOAT, dataAll, true);
}


function DisplayRoutes(divFilters) {

    var tmpUL = document.createElement('ul');
    var tmpLI = document.createElement('li');

    var tmpSelect = document.createElement('select');
    tmpSelect.id = 'optSorting';
    tmpSelect.style.width = '100px';
    tmpSelect.onchange = function (event) {
        SortRoute(this.value);
    }

    var divAsc = getAscDescDiv();

    getSortingOptions(tmpSelect);

    tmpLI.appendChild(tmpSelect);
    tmpLI.appendChild(divAsc);
    tmpUL.style.marginTop = '5px';
    tmpUL.style.marginBottom = '10px';
    tmpUL.appendChild(tmpLI);

    divFilters.appendChild(tmpUL);

}


function getSortingOptions(tmpSelect) {

    var objOAT = OAT.getSelfInstance();

    var tmpOrigArray = [];
    var allRoutes = getSeriesNameCol(objOAT.colStructure.items, tmpOrigArray, "");

    tmpSelect.innerHTML = '';

    LastSortingValue = 'rowHeader' + objOAT.rowConditions[0];

    for (var i = 0; i < objOAT.rowConditions.length; i++) // for row headers //rowConditions
    {
        var tmpHeaderName = objOAT.headerRow[objOAT.rowConditions[i]];
        var tmpIndexInHeader = objOAT.headerRow.indexOf(tmpHeaderName);
        var tmpOption = getSortingElement('rowHeader' + tmpIndexInHeader, 'rowHeader' + tmpIndexInHeader, tmpHeaderName);
        tmpSelect.appendChild(tmpOption);
    }

    for (var i = 0; i < objOAT.colConditions.length; i++) // for row headers //rowConditions
    {
        var tmpHeaderName = objOAT.headerRow[objOAT.colConditions[i]];
        var tmpIndexInHeader = objOAT.headerRow.indexOf(tmpHeaderName);
        var tmpOption = getSortingElement('colHeader' + tmpIndexInHeader, 'colHeader' + tmpIndexInHeader, tmpHeaderName);
        tmpSelect.appendChild(tmpOption);
    }

    if (allRoutes.length == 1) // Col headers exists
    {
        for (var i = 0; i < allRoutes.length; i++) // For col header's routes 
        {
            while (allRoutes[i].indexOf("{@@}") != -1) allRoutes[i] = allRoutes[i].replace("{@@}", " ");

            var tmpRouteLI = getSortingElement('route' + i, 'route' + i, allRoutes[i]);
            tmpSelect.appendChild(tmpRouteLI);
        }

    }
    else if (allRoutes.length == 0) // only Data value sorting
    {
        if (objOAT.rowConditions.length == 1) {
            var tmpOption = getSortingElement('route', -1, 'Data Value');
            tmpSelect.appendChild(tmpOption);
        }
    }

    return tmpSelect;
}


function getSortingElement(optID, optVal, SortingText) {

    var tmpOption = document.createElement('option');
    tmpOption.id = optID;
    tmpOption.value = optVal;
    tmpOption.innerHTML = SortingText;

    return tmpOption;

}

var isFiltersRedrawNeeded = false;
var LastSortingValue;

function getSeriesNameRoutes(collection, actualSeries, str) {
    for (var i = 0; i < collection.length; i++) {
        str += "/" + collection[i].value;
        if (collection[i].items.length > 0) {
            getSeriesNameCol(collection[i].items, actualSeries, str);
            str = str.substr(0, str.lastIndexOf("/"));
        }
        else if (!collection[i].items) {
            str = str.substr(1);
            actualSeries.push(str);
            str = str.substr(0, str.lastIndexOf("/"));
        }
    }
    return actualSeries;
}


function SortRoute(intRoute) {

    var objOAT = OAT.getSelfInstance();
    var SortedSuperSet = [];

    LastSortingValue = intRoute;

    if (intRoute.indexOf('rowHeader') != -1 || intRoute.indexOf('colHeader') != -1) {
        ProcessColSorting(objOAT, intRoute.replace('rowHeader', '').replace('colHeader', ''));
        return;
    }

    if (intRoute.indexOf('route') == 0) { // (intRoute != -1 || SortDropDown.id.indexOf('route') == 0)
        //Declare those header names on which sorting has to be done
        intRoute = intRoute.replace('route', '');
        var tmpOrigArray = [];
        var allRoutes = getSeriesNameCol(objOAT.colStructure.items, tmpOrigArray, ""); //getSeriesNameRoutes


        for (var i = 0; i < allRoutes.length; i++) {
            //if(allRoutes[i].indexOf('/') == 0) allRoutes[i] = allRoutes[i].substring(1);
            while (allRoutes[i].indexOf("{@@}") != -1) allRoutes[i] = allRoutes[i].replace("{@@}", "|");
            //if(allRoutes[i].indexOf('|') == allRoutes[i].length - 1) allRoutes[i] = allRoutes[i].substring(0,allRoutes[i].length - 1);
        }

        //Find first SUPER SET of allData which satisfies above criteria        
        //Sort(asc/desc) above SUPER SET array elements                     
        //Find (& persist order of subsets) other such sub sets in allData on which similar sorting has to be done
        //Sort each of these sub sets        
        //Combine sorted SUPER SET with above sorted sub sets in same order
        for (var i = 0; i < allRoutes.length; i++) {
            if (i == intRoute) {
                SortedSuperSet = SortedSuperSet.concat(getSortedSubSet(objOAT, getSubSetForRoute(objOAT, allRoutes[i])));
            }
            else {
                SortedSuperSet = SortedSuperSet.concat(getSubSetForRoute(objOAT, allRoutes[i]));
            }
        }

    }

    else {
        SortedSuperSet = getSortedSubSet(objOAT, objOAT.allData);
    }

    //Finally draw OAT grid from SortedSuperSet

    isSingleRowHeader = true;
    saveOldBlackList(objOAT);
    var pivot = new OAT.Pivot("pivot_content", "", "pivot_page", objOAT.headerRow, SortedSuperSet, objOAT.rowConditions, objOAT.colConditions, [], objOAT.headerRow.length - 1, { showChart: 0, showEmpty: 0, externalSort: true });

    togglePivot(z('pivotChk').checked);
    isRoutesVisible = true;


    showVisualizer(chartVisualizationDecided);


}



function saveOldBlackList(objOat) {
    oldBlackList = [];
    for (var i = 0; i < objOat.conditions.length; i++) {
        oldBlackList.push(objOat.conditions[i]);
    }
}

function getSubSetForRoute(objOAT, strRoute) {
    var tmpRouteValues = strRoute.split("|");
    var SuperSet = [];

    for (var i = 0; i < objOAT.allData.length; i++) {
        var tmpRouteContainer = [];
        for (var p = 0; p < objOAT.colConditions.length; p++) {
            if (tmpRouteValues[p] == objOAT.allData[i][objOAT.colConditions[p]]) tmpRouteContainer.push(tmpRouteValues[p]); // If current Oat cell belongs to required SUPER SET 
        }
        var isMatched = false;
        if (tmpRouteContainer.length == tmpRouteValues.length) {
            SuperSet.push(objOAT.allData[i]);
        }
    }
    return SuperSet;
}

function getSortedSubSet(objOAT, SubSet) {

    /*

    Algorithm involved :

    1. Make a clone of subset(SortedSuperSet). This clone will be used to re-index the elements after sorting
    2. Make an array of data values in this subset = SuperSetValues
    3. Sort above array of data value in asc/desc

    4. Now traverse first row in original subset. Take data value of this row.
    5. Find this data value's index in sorted array(SuperSetValues)
    6. Replace clone's(SortedSuperSet) above indexed row with first row of original SubSet
    7. Nullify that index of Sorted array(SuperSetValues). It is done for handling if more than one rows have same data value.

    8. Repeat above steps 4 to 8 for second row( & so on...) of original SubSet

    */
    var SortedSuperSet = []; //  SubSet.clone();
    var SuperSetValues = [];

    var arrDataValueIndexed = [];

    var SortedDataValues = [];

    for (var i = 0; i < SubSet.length; i++) {

        var tmpMarkUp = SubSet[i][SubSet[i].length - 1];
        var tmpDataValue = extractDataVale(tmpMarkUp);

        arrDataValueIndexed.push([i, tmpDataValue]);

        SortedDataValues.push(tmpDataValue);

        // Save data value & its index in arr1(two dimensional)
        // Save markup & its index in arr2(two dimensional)
        // Fetch only data Value col from arr1 and sort it to arr3(1 dimensional)
        // re-arrange arr1 items in accordance of arr3
        // get markup from arr2 in reference of sorted arr1
        // re-arrange original subset using indexes in arr2

        //SuperSetValues.push(tmpDataValue);
    }


    if (isSortingAsc) SortedDataValues.sort(sortAsc);
    else SortedDataValues.sort(sortDsc);

    for (var i = 0; i < SortedDataValues.length; i++) {

        for (var j = 0; j < arrDataValueIndexed.length; j++) {
            if (arrDataValueIndexed[j][1] == SortedDataValues[i]) {

                var indexInUnsortedSubset = arrDataValueIndexed[j][0];

                arrDataValueIndexed[j][1] = "";

                SortedSuperSet.push(SubSet[indexInUnsortedSubset]);
                break;
            }
        }
    }

    return SortedSuperSet;
}



function DrawGridFromOAT(objOAT, AllDataArray, isRedrawFilters) {

    isRefreshOnlyHeaderFilters = false;
    if (isRedrawFilters == true) isFiltersRedrawNeeded = true;
    else isFiltersRedrawNeeded = false;

    var pivot = new OAT.Pivot("pivot_content", "", "pivot_page", objOAT.headerRow, AllDataArray, objOAT.rowConditions, objOAT.colConditions, [], objOAT.headerRow.length - 1, { showChart: 0, showEmpty: 0 });

    togglePivot(z('pivotChk').checked);
    isRoutesVisible = true;
    DecideVisTypes(pivot);
    showVisualizer(chartVisualizationDecided);
}


function ProcessColSorting(objOAT, indexColHeader) {
    isFiltersRedrawNeeded = false;
    var cond = objOAT.conditions[indexColHeader];

    if (isSortingAsc) { cond.sort = 1; }
    else { cond.sort = -1; }
    objOAT.sort(cond);
    objOAT.go();

    isRefreshOnlyHeaderFilters = true;
    generateFilterPanels(objOAT, z('menu5'));

    togglePivot(z('pivotChk').checked);

    showVisualizer(chartVisualizationDecided);
}


var isSortingAsc = true;

function getAscDescDiv() {
    var imgAscDesc = document.createElement('img');
    imgAscDesc.id = 'imgAscDesc';
    imgAscDesc.src = '../../stock/themes/default/images/dataview/sort-ascend.png';
    imgAscDesc.style.cursor = 'pointer';
    imgAscDesc.style.verticalAlign = 'bottom';
    imgAscDesc.style.marginLeft = '10px';

    imgAscDesc.onclick = function (event) {
        toggleSortingAscDsc();
    }
    imgAscDesc.onmouseover = function (event) {
        imgAscDesc.rel = 'TTSAtoZ'

        if (imgAscDesc.src.indexOf('ascend') != null && imgAscDesc.src.indexOf('ascend') != undefined) {
            if (imgAscDesc.src.indexOf('ascend') != -1) {
                imgAscDesc.rel = 'TTSAtoZ'
            }
            else {
                imgAscDesc.src = imgAscDesc.src.replace('descend', 'ascend');
                imgAscDesc.rel = 'TTSZtoA'
            }
        }
        showSettingToolTip(imgAscDesc);
    }
    imgAscDesc.onmouseout = function (event) {
        HideCallout('divCallout');
    }
    return imgAscDesc;
}

function toggleSortingAscDsc() {
    var imgAscDesc = z('imgAscDesc');

    if (imgAscDesc.src.indexOf('ascend') != -1) {
        imgAscDesc.src = imgAscDesc.src.replace('ascend', 'descend');
        imgAscDesc.rel = 'TTSZtoA'
    }
    else {
        imgAscDesc.src = imgAscDesc.src.replace('descend', 'ascend');
        imgAscDesc.rel = 'TTSAtoZ'
    }

    isSortingAsc = !isSortingAsc;
    SortRoute(LastSortingValue);
    showSettingToolTip(imgAscDesc);
}

function createOthersFilterPanel(divOthers) {
    var tmpUL = document.createElement('ul');
    var tmpPivot = getFilterCondition("PivotToggler", false, "Pivoting", HandlePivotClick);
    var tmpSwap = getFilterCondition("OatSwapper", false, "Swap", swapRowColHeaders);

    tmpUL.appendChild(tmpPivot);
    tmpUL.appendChild(tmpSwap);
    divOthers.appendChild(tmpUL);
}

function createLineHelpPanel(divLineHelpPanel) {
    var tmpUL = document.createElement('ul');
    var tmpLI = document.createElement('li');

    tmpLI.innerHTML = 'Help text';
    tmpLI.innerHTML = z('lngLineChartHelpText').value;
    tmpUL.appendChild(tmpLI);
    divLineHelpPanel.appendChild(tmpUL);
}

function HandlePivotClick(domElement) {
    isFiltersRedrawNeeded = false;
    z('pivotChk').defaultChecked = domElement.checked;
    //togglePivot(z('chkFilterPivot_Toggler').checked);
    togglePivot(z('pivotChk').checked);
}

function toggleFiltersPanel(ShowFilters) {
    var visibilityStatus = z('example5').style.display;
    if (ShowFilters == null) {
        if (visibilityStatus == 'none') {
            showFiltersPanel();
        }
        else {
            hideFiltersPanel();
        }
    }
    else {
        if (ShowFilters) showFiltersPanel();
        else hideFiltersPanel();
    }
    //    if (z('imgShowHideFiltersPanel').src.indexOf('arrow_collapse-2.png') == -1)
    //        z('imgShowHideFiltersPanel').src = '../../stock/themes/default/images/dataview/arrow_collapse-2.png';
    //    else
    //        z('imgShowHideFiltersPanel').src = '../../stock/themes/default/images/dataview/arrow_collapse.png';
}

function showFiltersPanel() {
    z('tab-dsection').style.width = '100%';
    z('tab-vsection').style.width = '100%';
    di_jq('#example5').show('slow');
    di_jq('#bottom_tabs').show('slow');
    if (chartVisualizationDecided != 'table') di_resetChartWidth(chartVisualizationDecided, "", "di_vc_treeContainer");
    // resize map
    if (chartVisualizationDecided == 'map') {
        //setTimeout(resizeMap(false), 5000);
        resizeMap(false);
    }

    z('imgShowHideFiltersPanel').src = '../../stock/themes/default/images/dataview/arrow_collapse-2.png';
    z('imgShowHideFiltersPanel').rel = 'TTHFPanel';
}

function hideFiltersPanel() {

    var FilterPanelWidth;

    if (z('tab-dsection').style.display == 'block') FilterPanelWidth = z('tab-dsection').offsetWidth;
    else if (z('tab-vsection').style.display == 'block') FilterPanelWidth = z('tab-vsection').offsetWidth;
    FilterPanelWidth += z('example5').offsetWidth;
    di_jq('#bottom_tabs').hide('slow');
    di_jq('#example5').hide('slow',
            function () {
                z('tab-dsection').style.width = FilterPanelWidth + 'px';
                z('tab-vsection').style.width = FilterPanelWidth + 'px';

            }
        );

    if (chartVisualizationDecided != 'table') di_resetChartWidth(chartVisualizationDecided, FilterPanelWidth, "di_vc_treeContainer");
    // resize map
    if (chartVisualizationDecided == 'map') {
        //resizeMap(true);
        //setTimeout(resizeMap(true), 5000);
        resizeMap(true);
    }

    z('imgShowHideFiltersPanel').src = '../../stock/themes/default/images/dataview/arrow_collapse.png';
    z('imgShowHideFiltersPanel').rel = 'TTSFPanel';
}

function swapRowColHeaders(domElement) {
    var objOAT = OAT.getSelfInstance();

    var colConds = objOAT.colConditions.clone();

    if (colConds.length == 0) return;

    objOAT.colConditions = objOAT.rowConditions.clone();

    objOAT.rowConditions = colConds;
    isFiltersRedrawNeeded = false;
    objOAT.go();
    togglePivot(z('pivotChk').checked);
    showVisualizer(chartVisualizationDecided);
}

function PivotRowColHeaders() {
    var objOAT = OAT.getSelfInstance();

    var colConds = objOAT.colConditions.clone();
    var rowConds = objOAT.rowConditions.clone();

    if (colConds.length == 0 && rowConds.length == 0) return;

    var lastColCondition = colConds.pop();
    var lastRowCondition = rowConds[0];

    colConds.unshift(lastRowCondition);
    if (lastColCondition != null) rowConds.splice(0, 1, lastColCondition);
    else rowConds.splice(0, 1);

    objOAT.colConditions = colConds;

    objOAT.rowConditions = rowConds;

    isFiltersRedrawNeeded = false;
    objOAT.go();
    togglePivot(z('pivotChk').checked);
    showVisualizer(chartVisualizationDecided);
}

function refreshSortingOthersPanel() {
    var objOAT = OAT.getSelfInstance();

    var tmpSorting = getFilterElement("Sorting");
    var tmpShowHide = getFilterElement("Hide Columns");
    var tmpLineHelp = getFilterElement("Help");
    tmpSorting.style.marginTop = '25px';

    DisplayRoutes(tmpSorting);
    ShowHidePivotHeaders(objOAT, tmpShowHide);
    createLineHelpPanel(tmpLineHelp);

    divSorting = tmpSorting;
    divShowHide = tmpShowHide;
    divLineHelp = tmpLineHelp;
    divLineHelp.style.display = 'none';
}


// This is the event which is fired everytime when OAT is drawn
function ReDrawFiltersPanel(objOAT) {


    if (OatFirstDraw_DisableFilters && strDimensionSubgroups != '') {

        try {

            var isAnyDimensionAvailable = false;

            for (var i = 0; i < objOAT.headerRow.length - 1; i++) {
                var tmpHeaderName = objOAT.headerRow[i];
                if (!
            (tmpHeaderName == diStructureArea
            || tmpHeaderName == diStructureAreaID
            || tmpHeaderName == diStructureMRD
            || tmpHeaderName == diStructureSource
            || tmpHeaderName == diStructureTimePeriod
            || tmpHeaderName == diStructureTopic
            )
        ) {
                    objOAT.conditions[i].blackList = objOAT.conditions[i].distinctValues.clone();

                    // Remove blanks from blacklist
                    var indexOfBlanks = objOAT.conditions[i].blackList.indexOf("");
                    if (indexOfBlanks != -1) objOAT.conditions[i].blackList.splice(indexOfBlanks, 1);

                    isAnyDimensionAvailable = true;
                }
            }

            if (!isAnyDimensionAvailable) {
                OatFirstDraw_DisableFilters = false;
                ReDrawFiltersPanel(objOAT);
                return;
            }

            var arrDimensionsSubgroups = strDimensionSubgroups.split('|');

            // Traverse each that dimension where few items are going to be CHECKED ON
            for (var i = 0; i < arrDimensionsSubgroups.length - 1; i++) {

                var tmpDimensionSG = arrDimensionsSubgroups[i].split(':');
                var tmpDimension = tmpDimensionSG[0];
                var tmpSGs = tmpDimensionSG[1];
                var arrSGs = tmpSGs.split(',');
                // Index of that Dimension where few items are checked ON
                var indexOfHeader = objOAT.headerRow.indexOf(tmpDimension);

                for (var j = 0; j < arrSGs.length; j++) {

                    // Remove those blacklisted items which qualifies to be CHECKED ON
                    var indexOfBlackListItem = objOAT.conditions[indexOfHeader].blackList.indexOf(arrSGs[j]);
                    objOAT.conditions[indexOfHeader].blackList.splice(indexOfBlackListItem, 1);
                }
            }
            OatFirstDraw_DisableFilters = false;
            objOAT.go();

        }
        catch (err) {
            var msg = err.Message;
        }

    }
    else {

        createFiltersPanel(objOAT);

    }


    OatFirstDraw_DisableFilters = false;

    // Enable/Disable visualization icons

    DecideVisTypes(objOAT);

    //ShowProgressBar('Visualizing Data');

    ShowProgressBar(false);
    updateCount(objOAT);
}


// This is an even which is fired when any kind of filtering is done on OAT
function updateCount(objOAT) {

    // Enable/Disable visualization icons

    DecideVisTypes(objOAT);

    // Update count in div

    var tmpTotalRecords = 0;

    if (dataAll.length == 0) tmpTotalRecords = objOAT.allData.length;
    else tmpTotalRecords = dataAll.length;

    //    z('divCountDataRows').innerHTML = '<b>Total Records: </b>' + objOAT.filteredData.length + '/' + tmpTotalRecords;
    z('divCountDataRows').innerHTML = '<b>' + z('langDVTotalRecords').value + '</b>' + objOAT.filteredData.length + '/' + tmpTotalRecords;


    // Sync the sources tab with applied filters
    syncSources(objOAT);


    //Redraw the Sorting drop down elements. This handles the case when only one column header is left.
    //getSortingOptions(z('optSorting'));

}


function createFiltersPanel(objOAT) {

    if (isSingleRowHeader) {

        for (var i = 0; i < oldBlackList.length; i++) {
            objOAT.conditions[i].blackList = oldBlackList[i].blackList;
        }
        isSingleRowHeader = false;
        isRefreshOnlyHeaderFilters = true;
        objOAT.go();
        togglePivot(z('pivotChk').checked);
    }
    else {


        if (isFiltersRedrawNeeded) {

            generateFilterPanels(objOAT, z('menu5'));

            if (!isRefreshOnlyHeaderFilters) { // Re-create Sorting, ShowHide & Others panel only if required(very specific scenarios) 
                var menu6 = z('menu6');

                while (menu6.hasChildNodes()) {
                    menu6.removeChild(menu6.lastChild);
                }

                refreshSortingOthersPanel();

                menu6.appendChild(divSorting);
                menu6.appendChild(divShowHide);
                menu6.appendChild(divLineHelp);

                setTimeout(function () {
                    // Accordion
                    di_jq('#menu6 > li > a.expanded + ul').slideToggle('medium');
                    di_jq('#menu6 > li > a').click(function () {
                        di_jq('#menu6 > li > a.expanded').not(this).toggleClass('expanded').toggleClass('collapsed').parent().find('> ul').slideToggle('medium');
                        di_jq(this).toggleClass('expanded').toggleClass('collapsed').parent().find('> ul').slideToggle('medium');
                    });

                }, 250);

            }
            isRefreshOnlyHeaderFilters = false;
        }

        isFiltersRedrawNeeded = true;

        if (objOAT.colConditions.length == 0) z('pivotChk').defaultChecked = true;

    }
}


function syncSources(objOAT) {
    var ulSources = z('ulSources');
    ulSources.innerHTML = '';

    for (var i = 0; i < objOAT.conditions.length; i++) {

        // If multiple sources exists then bind from OAT
        if (objOAT.headerRow[i] == diStructureSource) {
            for (var j = 0; j < objOAT.conditions[i].distinctValues.length; j++) {
                var SourceText = objOAT.conditions[i].distinctValues[j];
                if (objOAT.conditions[i].blackList.indexOf(SourceText) == -1) {
                    var tmpLI = getLiSourceMeta(SourceText); //document.createElement('li'); //  
                    ulSources.appendChild(tmpLI);
                }
            }
            break;
        }

        // Otherwise get that single source from divSingleSource
        if (i == objOAT.conditions.length - 1) {
            var tmpLI = getLiSourceMeta(z('divSingleSource').innerHTML.replace('Source :', ''));
            ulSources.appendChild(tmpLI);
        }
    }
}

function getSourceNId(SourceText) {
    var result = 0;
    SourceText = SourceText.trim();
    var indexOfSource = Sources.indexOf(SourceText);
    result = Sources[indexOfSource - 1];
    return result;
}

function getLiSourceMeta(SourceFullText) {

    var result = document.createElement('li');
    var SourceNId = getSourceNId(SourceFullText);
    var tmpA = document.createElement('a');

    tmpA.innerHTML = SourceFullText;
    tmpA.href = 'javascript:void(0);';

    tmpA.onclick = function (event) {
        showSourceMetaData(SourceNId, SourceFullText);
    };

    result.appendChild(tmpA);
    return result;
}

function showSourceMetaData(SourceNId, SourceFullText) {
    var MetaDataPath = '../../stock/data/' + z('hdbnid').value + '/ds/' + z('hlngcodedb').value + '/metadata';
    var xmlPath = MetaDataPath + '/source/' + SourceNId + '.xml';
    var xsltPath = MetaDataPath + '/SRC.xsl';
    var html = di_jq.transform({ async: false, xml: xmlPath, xsl: xsltPath });
    if (html != undefined) {
        z('divSourceMetaData').innerHTML = html;
    }
    z('langShowSourceMetaData').innerHTML = SourceFullText;
    showHideComponent('source');
}

function getValidVisInfo(RecordCount, RowsCount, ColumnsCount, RowHeader1DistinctCount) {

    var ValidVisInfo = []; // for default = "10000000"

    // Table decision

    ValidVisInfo.push(true);

    // Line chart decision

    if (RecordCount <= 1000) ValidVisInfo.push(true);
    else ValidVisInfo.push(false);

    // Column, Bar, Area chart decision

    if (RowsCount * ColumnsCount <= 2500) {
        ValidVisInfo.push(true);
        ValidVisInfo.push(true);
        ValidVisInfo.push(true);
    }
    else {
        ValidVisInfo.push(false);
        ValidVisInfo.push(false);
        ValidVisInfo.push(false);
    }

    // Pie chart decision

    if (RowsCount <= 100) ValidVisInfo.push(true);
    else ValidVisInfo.push(false);

    // Map decision

    ValidVisInfo.push(isMapEnable);

    //Scatter plot decision

    //ValidVisInfo.push(false);

    // Pyramid chart decision

    if (ColumnsCount >= 2 && RowsCount <= 100) ValidVisInfo.push(true);
    else ValidVisInfo.push(false);

    // Tree map decision

    if (RowHeader1DistinctCount <= 100) ValidVisInfo.push(true);
    else ValidVisInfo.push(false);

    // Radar chart decision
    if (ColumnsCount >= 3)
        ValidVisInfo.push(true);
    else
        ValidVisInfo.push(false);

    // Cloud map decision

    ValidVisInfo.push(true);

    // Scatter 2D decision

    ValidVisInfo.push(true);

    return ValidVisInfo;

}


function DecideVisTypes(objOAT) {

    var SuggestedVisualizer = chartVisualizationDecided;

    var indexOfFirstRow = 0;

    indexOfFirstRow = parseInt(objOAT.rowConditions[0]);
    if (!isFinite(indexOfFirstRow)) indexOfFirstRow = 0;

    var tmpValidVisualizers = getValidVisInfo(objOAT.filteredData.length, objOAT.tabularData[0].length, objOAT.tabularData.length, objOAT.conditions[indexOfFirstRow].distinctValues.length - objOAT.conditions[indexOfFirstRow].blackList.length);

    // Code for enable/disable icons in visualizations ribbon

    for (var i = 0; i < tmpValidVisualizers.length; i++) {

        var tmpVisualizerStyle = z('replace' + (i + 1));
        var tmpTristateStyle = z('span_visual_icon_tristate' + (i + 1));

        if (tmpValidVisualizers[i])// Enable icon
        {
            tmpVisualizerStyle.className = 'visual_icon-' + (i + 1);
            if (tmpTristateStyle != undefined) tmpTristateStyle.style.display = 'block';
        }
        else { // Disable icon
            tmpVisualizerStyle.className = 'visual_icon-' + (i + 1) + '-' + (i + 1);
            if (tmpTristateStyle != undefined) tmpTristateStyle.style.display = 'none';
        }

    }

    // Show table if line & column chart are disabled
    if (tmpValidVisualizers[1] == false && tmpValidVisualizers[2] == false) {
        SuggestedVisualizer = 'table';
    }

    return SuggestedVisualizer;
}

/* function to hide dropdown div */
function hideDropDownDiv(e) {

    var clickId = e.target.id;
    var clickClass = e.target.className;

    // for download div
    var downLoadArr = ['clickDownload', 'clickDownloadLabel', 'toggleDownload', 'vcLangLblDlPDF', 'vcLangLblDlPNG', 'vcLangLblDlSVG', 'vcLangLblDlXLS', 'tblLangLblDlXLS', 'mapLangLblDlPNG', 'mapLangLblDlXLS', 'mapLangLblDlKML'];
  
    if (jQuery.inArray(clickId, downLoadArr) == -1) {
        di_jq('#toggleDownload').hide('slow');
    }

    // for map series
    var mapSeriesArr = ['clickMapSeries', 'mapSeriesFormatDiv', 'mapSrC_0'];
    if (jQuery.inArray(clickId, mapSeriesArr) == -1) {
        di_jq('#mapSeriesFormatDiv').hide('slow');
    }

    // for map hatch patern
    if (di_jq('#map_hatchpat_list').css('display') != 'none') {
        var mappatern = [];
        di_jq('#scrollContainer div[id*="map_hatchpat_"]').each(function () {
            mappatern.push(this.id);
        });

        if (jQuery.inArray(clickId, mappatern) == -1) {
            di_jq('#map_hatchpat_list').hide('slow');
            di_jq('#map_hatchpat_list').removeAttr('rel');
            for (var i = 0; i < mappatern.length; i++) {
                if (mappatern[i] != 'map_hatchpat_list')
                    di_jq('#' + mappatern[i]).attr('rel', 0);
            }
        }
    } //

    // for Dot deinsity
    var mapDotDensityArr = ['clickMapDotDensity', 'mapDotDtyStyleList'];
    if (jQuery.inArray(clickId, mapDotDensityArr) == -1) {
        di_jq('#mapDotDtyStyleList').hide('slow');
    }

    // for map hatch paetern on pop-up
    if (di_jq('#dv_container_popup_m').css('display') != 'none') {
        var mappatern_pop = [];
        di_jq('#mapPopIdCrHatLdTbl div[id*="mappop_hatchpat_"]').each(function () {
            mappatern_pop.push(this.id);
        });
        if (jQuery.inArray(clickId, mappatern_pop) == -1) {
            di_jq('#mapPopIdCrHatLdTbl div[id*="mappop_hatchpat_list_"]').each(function () {
                //di_jq('#'+this.id).removeAttr('rel');
                di_jq('#' + this.id).hide('slow');
            });
        }
    }

    // for visualizer color format 
    if (di_jq('#sFormatDiv').css('display') != 'none') {
        var visuCFormat = ['sFormatDivClickId'];
        if (jQuery.inArray(clickId, visuCFormat) == -1) {
            di_jq('#sFormatDiv').hide('slow');
        }
    }
    // for visualizer style series
    if (di_jq('#sbdrSolidDiv').css('display') != 'none') {
        var visuStSr = ['styleSeries'];
        if (jQuery.inArray(clickId, visuStSr) == -1) {
            di_jq('#sbdrSolidDiv').hide('slow');
        }
    }
    // for visualizer line style
    if (di_jq('#sbdrThinDiv').css('display') != 'none') {
        var visuLnSt = ['lineStyle'];
        if (jQuery.inArray(clickId, visuLnSt) == -1) {
            di_jq('#sbdrThinDiv').hide('slow');
        }
    }

    // for map layer on pop-up
    if (di_jq('#mapPopTab-2_sec_con').css('display') != 'none') {
        var mappatern_pop = [];
        di_jq('#mapPopTab-2_sec_con_tbl div[id*="ly_style_"]').each(function () {
            mappatern_pop.push(this.id);
        });
        if (jQuery.inArray(clickId, mappatern_pop) == -1) {
            di_jq('#mapPopTab-2_sec_con_tbl div[id*="mappop_layer_stylelist_"]').each(function () {
                if (clickId != this.id)
                    di_jq('#' + this.id).hide('slow');
            });
        }
    }
}
/* function to open map setting pop-ups */
function openMapPropertyPopUp(action) {
    if (chartVisualizationDecided != "map")
        return;

    var actType = 'add';
    var applySettingsOf = 'label';
    var actionAr = action.split("_");
    action = actionAr[0];
    if (actionAr[1] != undefined) actType = actionAr[1];

    var htmlui = '';
    //z('popupTab').value = action;
    switch (action) {
        case "general":
            // getting text from body part
            var genTxt = di_jq('#lngMapPopGeneral').html();
            var modiTxt = di_jq('#lngMapPopModifySetting').html();
            var labelTxt = di_jq('#lngMapPopLabel').html();
            var bdrTxt = di_jq('#lngMapPopBorder').html();
            var lyerTxt = di_jq('#lngMapPopLayer').html();
            var labelOnTxt = di_jq('#lngMapPopLblOn').html();
            var areaIdTxt = di_jq('#lngMapPopAreaid').html();
            var areaNmTxt = di_jq('#lngMapPopAreaNm').html();
            var daatValTxt = di_jq('#lngMapPopDataValue').html();
            var tmPeriodTxt = di_jq('#lngMapPopTmPeriod').html();
            var multiRowTxt = di_jq('#lngMapPopMultiRow').html();
            var indentedTxt = di_jq('#lngMapPopIndented').html();
            var swdeTxt = di_jq('#lngMapPopSwde').html();
            var effectsTxt = di_jq('#lngMapPopEffects').html();
            var effectTpTxt = di_jq('#lngMapPopEffectTp').html();
            var depthTxt = di_jq('#lngMapPopDepth').html();
            var showBdrTxt = di_jq('#lngMapPopShowBorder').html();
            var bdrWidthTxt = di_jq('#lngMapPopWidth').html();
            var bdrColorTxt = di_jq('#lngMapPopColor').html();
            var bdrStyleTxt = di_jq('#lngMapPopStyle').html();
            var lBaseTxt = di_jq('#lngMapPopTBase').html();
            var lFeatureTxt = di_jq('#lngMapPopTFeature').html();
            var lFirstTxt = di_jq('#lngMapPopIcnFirst').html();
            var lLastTxt = di_jq('#lngMapPopIcnLast').html();
            var lUpTxt = di_jq('#lngMapPopIcnUp').html();
            var lDownTxt = di_jq('#lngMapPopIcnDown').html();
            var disputedBdrTxt = di_jq('#lngMapPopDisputedBdr').html();
            var ShadowTxt = di_jq('#lngMapPopShadow').html();
            var EmbossedTxt = di_jq('#lngMapPopEmbossed').html();
            var BlockTxt = di_jq('#lngMapPopBlock').html();
            var GradientTxt = di_jq('#lngMapPopGradient').html();
            var ReflectTxt = di_jq('#lngMapPopReflect').html();
            var leaderLineTxt = di_jq('#lngMapPopLeaderLine').html(); // 'Leader Line';
            var straightLnTxt = di_jq('#lngMapPopStraightLine').html(); // 'Straight line';
            var fieldsTxt = di_jq('#lngMapPopFields').html(); // 'Fields';
            var lblRow1Txt = di_jq('#lngMapPopLblRow1').html(); //'Label Row 1';
            var lblRow2Txt = di_jq('#lngMapPopLblRow2').html(); // 'Label Row 2';

            /* change header */
            di_jq('#dv_sec_pop_m').html('<h1>' + genTxt + '</h1><h4>' + modiTxt + '</h4>');
   
            /* change left */
            htmlui = '<ul class="left_button"><li><a id="popTabLabel_m" href="javascript:void(0)" class="selected" onclick=\'clickPopPanelMap("popTabLabel_m", "label")\'>' + labelTxt + '</a></li><li><a id="popTabbdr_m" href="javascript:void(0)" onclick=\'clickPopPanelMap("popTabbdr_m", "border")\'>' + bdrTxt + '</a></li><li><a id="popTabLayer_m" href="javascript:void(0)" onclick=\'clickPopPanelMap("popTabLayer_m", "layer")\'>' + lyerTxt + '</a></li></ul>';

            di_jq('#dv_sec_left_m').html(htmlui);

            /* change right */
            htmlui = '<div class="dv_sec_right_ppup_hdn_pos"><fieldset class="fieldset"><legend id="popfieldSet_m">' + labelTxt + '</legend></fieldset></div>';

            // border
            htmlui += '<div id="popTabbdr_m_sec" class="panelContainer" style="display:none;"><table width="90%" cellpadding="0" cellspacing="0"><tr><td colspan="2" height="30" valign="top"><input id="popBorderChkbox_m" type="checkbox"> ' + showBdrTxt + '</td></tr><tr><td width="20%"><p>' + bdrWidthTxt + '</p></td><td width="80%"><table cellpadding="0" cellspacing="0"><tr><td><input id="popBorderWidthTxt_m" type="text" class="vcInputTxtBox" style="width:40px;"></td><td width="16" style="background-image:url(../../stock/themes/default/images/dataview/numcount.gif);"><div style="width:16px;height:10px;cursor:pointer;" onclick="toggleMapBorder(1)"></div><div style="width:16px;height:7px;cursor:pointer;" onclick="toggleMapBorder(0)"></div></td></tr></table></td></tr><tr><td><p>' + bdrColorTxt + '</p></td><td><input id="popchartBoderColor_m" type="text" class="vcInputTxtBox" style="width:20px;"> <img src="../../stock/themes/default/images/dataview/icon_colorp.png" id="dicolor0_m" style="cursor:pointer;"></td></tr><tr><td><p>' + bdrStyleTxt + '</p></td><td><table cellpadding="0" cellspacing="0" style="width:65px;border:1px solid #d3d3d3;padding:1px;background:#fff;"><tr><td><span id="mapBdrStl_Selected" class="mapBdrStIcon" rel="" onclick="mapChangeBdrStyle(this.id)"></span></td><td valign="bottom"><a href="javascript:void(0);" onclick=\'javascript:di_jq("#mapBdrStyleDiv").slideToggle();\'><img src="../../stock/themes/default/images/dataview/sformatdown.png"></a></td></tr></table><div id="mapBdrStyleDiv" style="z-index:1200;width:61px;height:62px;background:#fff;border:1px solid #d3d3d3;position:absolute;display:none;overflow:auto;padding:1px;"><table cellpadding="0" cellspacing="0"><tr><td><div id="mapBdrStl_Solid" class="mapBdrStIcon mapBdrStIcon_Solid" rel="Solid" onclick="mapChangeBdrStyle(this.id)" style="height:12px;"></div><div id="mapBdrStl_Dash" class="mapBdrStIcon mapBdrStIcon_Dash" rel="Dash" onclick="mapChangeBdrStyle(this.id)" style="height:12px;"></div><div id="mapBdrStl_Dot" class="mapBdrStIcon mapBdrStIcon_Dot" rel="Dot" onclick="mapChangeBdrStyle(this.id)" style="height:12px;"></div><div id="mapBdrStl_DashDot" class="mapBdrStIcon mapBdrStIcon_DashDot" rel="DashDot" onclick="mapChangeBdrStyle(this.id)" style="height:12px;"></div><div id="mapBdrStl_DashDotDot" class="mapBdrStIcon mapBdrStIcon_DashDotDot" rel="DashDotDot" onclick="mapChangeBdrStyle(this.id)" style="height:12px;"></div></td></tr></table></div></td></tr></table></div>';

            // label 
            /* commented 07 Aug 2012
            htmlui += '<div id="popTabLabel_m_sec" class="panelContainer" style="display:block;"><table width="90%" cellpadding="0" cellspacing="0"><tr><td height="30" valign="top" class="normalText"><table width="100%" cellpadding="0" cellspacing="0"><tr><td class="normalText"><input id="popLabelChkbox_m" type="checkbox"> '+labelOnTxt+'</td><td style="padding-right:10px;"><select id="di_mapPopSelFont" class="vcInputSelBox" style="width:50px;">' + getDropDownOptions(6, 31) + '</select></td><td><span id="mapPopTitle_bFont" class="vcTitleIcon vcTitleIcon_fontbig" onclick=\'checkMapFontSTPopUp("BF")\'></span></td><td><span id="mapPopTitle_sFont" class="vcTitleIcon vcTitleIcon_fontsmall" onclick=\'checkMapFontSTPopUp("SF")\'></span></td><td><span id="mapPopTitle_Bold" class="vcTitleIcon vcTitleIcon_bold" onclick=\'checkMapFontSTPopUp("B")\'></span></td><td><span id="mapPopTitle_Italic" class="vcTitleIcon vcTitleIcon_italic" onclick=\'checkMapFontSTPopUp("I")\'></span></td><td><span id="mapPopTitle_Underline" class="vcTitleIcon vcTitleIcon_underline" onclick=\'checkMapFontSTPopUp("U")\'></span></td><td><span id="mapPopTitle_Color" class="vcTitleIcon vcTitleIcon_color"><div id="inp_popdicolormap1" style="position:relative;top:15px;left:4px;font-size:1px;width:18px;height:5px;"></div></span><input type="hidden" id="poph_dicolormap1" value="#000000"><input type="hidden" id="poph_fontbold" value="false"><input type="hidden" id="poph_fontitalic" value="false"><input type="hidden" id="poph_fontunderline" value="false"></td></tr><tr><td colspan="8" height="25"><select id="di_mapLabelFontNm" class="vcInputSelBox" style="width:213px;"><option value="Arial">Arial</option><option value="Arial Black">Arial Black</option><option value="Comic Sans MS">Comic Sans MS</option><option value="Georgia">Georgia</option><option value="Impact">Impact</option><option value="Lucida Console">Lucida Console</option><option value="Lucida Sans Unicode">Lucida Sans Unicode</option><option value="Palatino Linotype, Book Antiqua">Palatino Linotype, Book Antiqua</option><option value="Tahoma">Tahoma</option><option value="Times New Roman">Times New Roman</option><option value="Trebuchet MS">Trebuchet MS</option><option value="Verdana">Verdana</option><option value="MS Sans Serif">MS Sans Serif</option><option value="MS Serif">MS Serif</option></select></td></tr></table></td></tr><tr><td><div style="width:200px;border:1px solid #d3d3d3;padding:5px;"><div><input id="popLabelAID_m" type="checkbox" value="0" onclick="showHideMapLabel(0)"> <span class="normalText">'+areaIdTxt+'</span></div><div><input id="popLabelANM_m" type="checkbox" value="1" onclick="showHideMapLabel(0)"> <span class="normalText">'+areaNmTxt+'</span></div><div><input id="popLabelDTV_m" type="checkbox" value="2" onclick="showHideMapLabel(0)"> <span class="normalText">'+daatValTxt+'</span></div><div><input id="popLabelCNSY_m" type="checkbox" value="3" onclick="showHideMapLabel(0)"> <span class="normalText">'+tmPeriodTxt+'</span></div><div style="border-bottom:1px solid #d3d3d3;margin:5px;"></div><div><input id="popLabelMROW_m" type="checkbox" onclick="showHideMapLabel(1, this.checked)"><span class="normalText">'+multiRowTxt+'</span></div><div><input id="popLabelINDT_m" type="checkbox"><span class="normalText">'+indentedTxt+'</span></div><div><input id="popLabelSWDE_m" type="checkbox"> <span class="normalText">'+swdeTxt+'</span></div></div></td></tr><tr><td height="30" valign="middle" class="normalText"><input id="popEffectsChkbox_m" type="checkbox"> '+effectsTxt+'</td></tr><tr><td><table cellpadding="0" cellspacing="0" style="width:210px;border:1px solid #d3d3d3;padding:5px;"><tr><td><p>'+effectTpTxt+'</p></td><td><p>'+depthTxt+'</p></td><td></td></tr><tr><td><p><select id="popEffectType_m" class="vcInputSelBox" style="width:100px;"><option value="Shadow">'+ShadowTxt+'</option><option value="Embossed">'+EmbossedTxt+'</option><option value="Block">'+BlockTxt+'</option><option value="Gradient">'+GradientTxt+'</option><option value="Reflect">'+ReflectTxt+'</option></select></p></td><td><p><input id="popEffectDepth_m" type="text" class="vcInputSelBox" value="1" style="width:50px;"></p></td><td><input id="popEffectColor_m" class="vcInputTxtBox" type="text" style="width: 20px;cursor:pointer;"></td></tr></table></td></tr></table></div>';*/

            // label 
            htmlui += '<div id="popTabLabel_m_sec" class="panelContainer" style="display:block;"><table width="90%" cellpadding="0" cellspacing="0"><tr><td valign="top"><div class="normalText"><input id="popLabelChkbox_m" type="checkbox">' + labelOnTxt + '</div><div class="normalText"><input id="popLabelSWDE_m" type="checkbox">' + swdeTxt + '</div><div class="normalText"><input id="popLabelMROW_m" type="checkbox" onclick="showHideMapLabel(1, this.checked)">' + multiRowTxt + '</div></td><td valign="top"><fieldset class="fieldset2"><legend>' + fieldsTxt + '</legend><div class="normalText"><input id="popLabelAID_m" type="checkbox" value="0" onclick="showHideMapLabel(0)">' + areaIdTxt + '</div><div class="normalText"><input id="popLabelANM_m" type="checkbox" value="1" onclick="showHideMapLabel(0)">' + areaNmTxt + '</div><div class="normalText"><input id="popLabelDTV_m" type="checkbox" value="2" onclick="showHideMapLabel(0)">' + daatValTxt + '</div><div class="normalText"><input id="popLabelCNSY_m" type="checkbox" value="3" onclick="showHideMapLabel(0)">' + tmPeriodTxt + '</div></fieldset></td></tr><tr><td colspan="2"><div class="mapLabelRowTab"><span id="mapPopLblRow1" onclick="mapLblRowClick(1)" class="selectedLblTab" style="cursor:pointer;">' + lblRow1Txt + '</span> <span id="mapPopLblRowSeptor">|</span> <span id="mapPopLblRow2" onclick="mapLblRowClick(2)" style="cursor:pointer;">' + lblRow2Txt + '</span></div><div id="mapPopLbkRow1_con"><table cellpadding="0" cellspacing="0"><tr><td><select id="di_mapLabelFontNm1" class="vcInputSelBox" style="width:115px;"><option value="Arial">Arial</option><option value="Arial Black">Arial Black</option><option value="Comic Sans MS">Comic Sans MS</option><option value="Georgia">Georgia</option><option value="Impact">Impact</option><option value="Lucida Console">Lucida Console</option><option value="Lucida Sans Unicode">Lucida Sans Unicode</option><option value="Palatino Linotype, Book Antiqua">Palatino Linotype, Book Antiqua</option><option value="Tahoma">Tahoma</option><option value="Times New Roman">Times New Roman</option><option value="Trebuchet MS">Trebuchet MS</option><option value="Verdana">Verdana</option><option value="MS Sans Serif">MS Sans Serif</option><option value="MS Serif">MS Serif</option></select>&nbsp;&nbsp;</td><td><select id="di_mapPopSelFont1" class="vcInputSelBox" style="width:50px;">' + getDropDownOptions(6, 31) + '</select>&nbsp;&nbsp;</td><td><span id="mapPopTitle_bFont1" class="vcTitleIcon vcTitleIcon_fontbig" onclick=\'checkMapFontSTPopUp("BF",1)\'></span></td><td><span id="mapPopTitle_sFont1" class="vcTitleIcon vcTitleIcon_fontsmall" onclick=\'checkMapFontSTPopUp("SF",1)\'></span></td><td><span id="mapPopTitle_Bold1" class="vcTitleIcon vcTitleIcon_bold" onclick=\'checkMapFontSTPopUp("B",1)\'></span></td><td><span id="mapPopTitle_Italic1" rel="TTItalic"  class="vcTitleIcon vcTitleIcon_italic" onclick=\'checkMapFontSTPopUp("I",1)\'></span></td><td><span id="mapPopTitle_Underline1" rel="TTULine" onmouseover="showSettingToolTip(this);" class="vcTitleIcon vcTitleIcon_underline" onclick=\'checkMapFontSTPopUp("U",1)\'></span></td><td><span id="mapPopTitle_Color1" class="vcTitleIcon vcTitleIcon_color"><div id="inp_popdicolormap1" style="position:relative;top:15px;left:4px;font-size:1px;width:18px;height:5px;"></div></span><input type="hidden" id="poph_dicolormap1" value="#000000"><input type="hidden" id="poph_fontbold1" value="false"><input type="hidden" id="poph_fontitalic1" value="false"><input type="hidden" id="poph_fontunderline1" value="false"></td></tr></table></div><div id="mapPopLbkRow2_con" style="display:none;"><table cellpadding="0" cellspacing="0"><tr><td><select id="di_mapLabelFontNm2" class="vcInputSelBox" style="width:115px;"><option value="Arial">Arial</option><option value="Arial Black">Arial Black</option><option value="Comic Sans MS">Comic Sans MS</option><option value="Georgia">Georgia</option><option value="Impact">Impact</option><option value="Lucida Console">Lucida Console</option><option value="Lucida Sans Unicode">Lucida Sans Unicode</option><option value="Palatino Linotype, Book Antiqua">Palatino Linotype, Book Antiqua</option><option value="Tahoma">Tahoma</option><option value="Times New Roman">Times New Roman</option><option value="Trebuchet MS">Trebuchet MS</option><option value="Verdana">Verdana</option><option value="MS Sans Serif">MS Sans Serif</option><option value="MS Serif">MS Serif</option></select>&nbsp;&nbsp;</td><td><select id="di_mapPopSelFont2" class="vcInputSelBox" style="width:50px;">' + getDropDownOptions(6, 31) + '</select>&nbsp;&nbsp;</td><td><span id="mapPopTitle_bFont2" class="vcTitleIcon vcTitleIcon_fontbig" onclick=\'checkMapFontSTPopUp("BF",2)\'></span></td><td><span id="mapPopTitle_sFont2" class="vcTitleIcon vcTitleIcon_fontsmall" onclick=\'checkMapFontSTPopUp("SF",2)\'></span></td><td><span id="mapPopTitle_Bold2" class="vcTitleIcon vcTitleIcon_bold" onclick=\'checkMapFontSTPopUp("B",2)\'></span></td><td><span id="mapPopTitle_Italic2" class="vcTitleIcon vcTitleIcon_italic" onclick=\'checkMapFontSTPopUp("I",2)\'></span></td><td><span id="mapPopTitle_Underline2" class="vcTitleIcon vcTitleIcon_underline" onclick=\'checkMapFontSTPopUp("U",2)\'></span></td><td><span id="mapPopTitle_Color2" class="vcTitleIcon vcTitleIcon_color"><div id="inp_popdicolormap2" style="position:relative;top:15px;left:4px;font-size:1px;width:18px;height:5px;"></div></span><input type="hidden" id="poph_dicolormap2" value="#000000"><input type="hidden" id="poph_fontbold2" value="false"><input type="hidden" id="poph_fontitalic2" value="false"><input type="hidden" id="poph_fontunderline2" value="false"></td></tr></table></div></td></tr><tr><td style="padding-top:10px;" colspan="2"><fieldset class="fieldset2"><legend><input id="popEffectsChkbox_m" type="checkbox"> ' + effectsTxt + '</legend><table cellpadding="0" cellspacing="0" style="padding:5px;"><tr><td class="normalText">' + effectTpTxt + '&nbsp;&nbsp;</td><td class="normalText">' + depthTxt + '&nbsp;&nbsp;</td><td></td></tr><tr><td><select id="popEffectType_m" class="vcInputSelBox" style="width:100px;"><option value="Shadow">' + ShadowTxt + '</option><option value="Embossed">' + EmbossedTxt + '</option><option value="Block">' + BlockTxt + '</option><option value="Gradient">' + GradientTxt + '</option><option value="Reflect">' + ReflectTxt + '</option></select>&nbsp;&nbsp;</td><td><input id="popEffectDepth_m" type="text" class="vcInputSelBox" value="1" style="width:50px;">&nbsp;&nbsp;</td><td><input id="popEffectColor_m" class="vcInputTxtBox" type="text" style="width: 20px;cursor:pointer;"></td></tr></table></fieldset></td></tr><tr><td style="padding-top:3px;" colspan="2"><fieldset class="fieldset2"><legend><input id="popLeaderChkbox_m" type="checkbox"> ' + leaderLineTxt + '</legend><table cellpadding="0" cellspacing="0" style="padding:5px;"><tr><td class="normalText">' + straightLnTxt + '&nbsp;&nbsp;</td><td class="normalText">' + bdrStyleTxt + '&nbsp;&nbsp;</td><td class="normalText">' + bdrWidthTxt + '&nbsp;&nbsp;</td><td></td></tr><tr><td><select id="popLeaderLnStr_m" class="vcInputSelBox" style="width:70px;"><option value="yes">Yes</option><option value="no">No</option></select>&nbsp;&nbsp;</td><td style="padding-right:8px;"><table cellpadding="0" cellspacing="0" style="width:65px;border:1px solid #d3d3d3;padding:1px;background:#fff;"><tr><td><span id="mapLeaderStylel_Selected" class="mapLeaderStIcon" rel=""></span></td><td valign="bottom"><a href="javascript:void(0);" onclick=\'javascript:di_jq("#mapLeaderStyleDiv").slideToggle();\'><img src="../../stock/themes/default/images/dataview/sformatdown.png"></a></td></tr></table><div id="mapLeaderStyleDiv" style="z-index:1200;width:61px;height:72px;background:#fff;border:1px solid #d3d3d3;position:absolute;display:none;overflow:auto;padding:1px;"><table cellpadding="0" cellspacing="0"><tr><td><div id="mapLeaderStylel_Solid" class="mapLeaderStIcon mapLeaderStIcon_Solid" rel="Solid" onclick="mapChangeLeaderStyle(this.id)" style="height:12px;"></div><div id="mapLeaderStylel_Dash" class="mapLeaderStIcon mapLeaderStIcon_Dash" rel="Dash" onclick="mapChangeLeaderStyle(this.id)" style="height:12px;"></div><div id="mapLeaderStylel_Dot" class="mapLeaderStIcon mapLeaderStIcon_Dot" rel="Dot" onclick="mapChangeLeaderStyle(this.id)" style="height:12px;"></div><div id="mapLeaderStylel_DOTSolid" class="mapLeaderStIcon mapLeaderStIcon_DOTSolid" rel="DOTSolid" onclick="mapChangeLeaderStyle(this.id)" style="height:12px;"></div><div id="mapLeaderStylel_DOTDash" class="mapLeaderStIcon mapLeaderStIcon_DOTDash" rel="DOTDash" onclick="mapChangeLeaderStyle(this.id)" style="height:12px;"></div><div id="mapLeaderStylel_DOTDot" class="mapLeaderStIcon mapLeaderStIcon_DOTDot" rel="DOTDot" onclick="mapChangeLeaderStyle(this.id)" style="height:12px;"></div></td></tr></table></div></td><td><input id="popLeaderWidth_m" type="text" class="vcInputSelBox" value="1" style="width:50px;">&nbsp;&nbsp;</td><td><input id="popLeaderColor_m" class="vcInputTxtBox" type="text" style="width: 20px;cursor:pointer;"></td></tr></table></fieldset></td></tr></table></div>';

            // layer tab
            htmlui += '<div id="popTabLayer_m_sec" class="panelContainer" style="display:none;"><table id="mapPopIdLayer" width="100%" cellpadding="0" cellspacing="0"><tr><td><ul id="mapPopIdLayer_tabs" class="blue"><li><a id="mapPopTab-1t" class="current" onclick=\'toggleLayerTab("mapPopTab-1")\' href="javascript:void(0);">' + lBaseTxt + '</a></li><li><a id="mapPopTab-2t" class="" onclick=\'toggleLayerTab("mapPopTab-2")\' href="javascript:void(0);">' + lFeatureTxt + '</a></li></ul></td></tr><tr><td><div id="mapPopTab-1_sec"><div id="mapPopTab-1_sec_con" style="height:200px;overflow:auto;margin-top:10px;background-color:#e2e2e2;border:1px solid #D3D3D3;"></div><div style="height:20px;padding-top:5px;"><span class="LayerIcon LayerIcon_OFirst" onclick=\'mapChangeLayerOrder("first")\' title="' + lFirstTxt + '"></span><span class="LayerIcon LayerIcon_OUp" onclick=\'mapChangeLayerOrder("up")\' title="' + lUpTxt + '"></span><span class="LayerIcon LayerIcon_ODown" onclick=\'mapChangeLayerOrder("down")\' title="' + lDownTxt + '"></span><span class="LayerIcon LayerIcon_OLast" onclick=\'mapChangeLayerOrder("last")\' title="' + lLastTxt + '"></span></div></div><div id="mapPopTab-2_sec" style="display:none;"><div style="padding-top:10px;"><input type="checkbox" id="flDisputedBnd" onclick="chkUnchkDisputedBdr(this.checked)"> ' + disputedBdrTxt + '</div><div id="mapPopTab-2_sec_con" style="height:180px;overflow:auto;margin-top:10px;background-color:#e2e2e2;border:1px solid #D3D3D3;z-index:99;"></div><div style="padding-top:5px;display:none;"><input type="file" id="flUploadLayer" size="50"> </div></div></td></tr></table></div>';

            // render HTML
            di_jq('#dv_sec_right_m').html(htmlui);

            // for map border color picker
            di_jq('#dicolor0_m').ColorPicker({
                onSubmit: function (hsb, hex, rgb, el) {
                    di_jq(el).ColorPickerHide();
                    di_jq('#popchartBoderColor_m').val('#' + hex);
                    di_jq('#popchartBoderColor_m').css("backgroundColor", "#" + hex);
                    di_jq('#popchartBoderColor_m').css("color", "#" + hex);
                    //function to call
                }
            });
            di_jq('#popEffectColor_m').ColorPicker({
                onSubmit: function (hsb, hex, rgb, el) {
                    di_jq(el).ColorPickerHide();
                    di_jq('#popEffectColor_m').val('#' + hex);
                    di_jq('#popEffectColor_m').css("backgroundColor", "#" + hex);
                    di_jq('#popEffectColor_m').css("color", "#" + hex);
                    //function to call
                }
            });
            di_jq('#popLeaderColor_m').ColorPicker({
                onSubmit: function (hsb, hex, rgb, el) {
                    di_jq(el).ColorPickerHide();
                    di_jq('#popLeaderColor_m').val('#' + hex);
                    di_jq('#popLeaderColor_m').css("backgroundColor", "#" + hex);
                    di_jq('#popLeaderColor_m').css("color", "#" + hex);
                    //function to call
                }
            });
            break;
        case "theme":
            applySettingsOf = actType + 'theme';
            var isChart = false;
            var themeType = di_jq('#mapThemeType').attr('class');
            if (themeType == 'Chart') isChart = true;

            // getting label text from the body
            var themesTxt = di_jq('#lngMapPopThemes').html();
            var modiTxt = di_jq('#lngMapPopModifySetting').html();
            var themeTxt = di_jq('#lngMapPopTheme').html();
            var legendTxt = di_jq('#lngMapPopLegend').html();
            var chSettingTxt = di_jq('#lngMapPopChartSetting').html();
            var themeNmTxt = di_jq('#lngMapPopThemeName').html();
            var themeTpTxt = di_jq('#lngMapPopThemeType').html();
            var themeSrTxt = di_jq('#lngMapPopThemeSeries').html();
            var thChartTpSrTxt = di_jq('#lngMapPopThChatType').html();
            var chartColumSrTxt = di_jq('#lngMapPopThChatColumn').html();
            var chartPieSrTxt = di_jq('#lngMapPopThChatPie').html();
            var chartLineSrTxt = di_jq('#lngMapPopThChatLine').html();
            var thColorTxt = di_jq('#lngMapPopThColor').html();
            var thHatchTxt = di_jq('#lngMapPopThHatch').html();
            var thDotTxt = di_jq('#lngMapPopThDotden').html();
            var thChartTxt = di_jq('#lngMapPopThChart').html();
            var thSymbolTxt = di_jq('#lngMapPopThSymbol').html();
            var brkCountTxt = di_jq('#lngMapPopBrkCount').html();
            var brkTypeTxt = di_jq('#lngMapPopBrkType').html();
            var brkEqCntTxt = di_jq('#lngMapPopBrkEqCount').html();
            var brkEqSizeTxt = di_jq('#lngMapPopBrkEqSize').html();
            var brkContiTxt = di_jq('#lngMapPopBrkCont').html();
            var brkDiscontTxt = di_jq('#lngMapPopBrkDiscont').html();
            var lgMinTxt = di_jq('#lngMapPopLgMin').html();
            var lgMaxTxt = di_jq('#lngMapPopLgMax').html();
            var lgDecTxt = di_jq('#lngMapPopLgDecimals').html();
            var uploadSeTxt = di_jq('#lngMapPopUploadSetting').html();
            var smoothTxt = di_jq('#lngMapPopLgSmooth').html();
            var downSetTxt = di_jq('#lngMapPopDwnSetting').html();
            var marketStTxt = di_jq('#lngMapPopMarStyle').html();
            var marketSzTxt = di_jq('#lngMapPopMarSize').html();
            var marketCoTxt = di_jq('#lngMapPopMarColor').html();
            var marketVlTxt = di_jq('#lngMapPopMarValue').html();
            var marketOpTxt = di_jq('#lngMapPopMarOpacity').html();
            var viewDataTxt = di_jq('#lngMapPopCSViewDtVal').html();
            var DecimalsTxt = di_jq('#lngMapPopCSDecimals').html();
            var csWidthTxt = di_jq('#lngMapPopCSWidth').html();
            var csHeightTxt = di_jq('#lngMapPopCSHeight').html();
            var csTimeSrTxt = di_jq('#lngMapPopCSTimeSeries').html();
            var csMostRecentTxt = di_jq('#lngMapPopCSMostRecent').html();
            var csTmPeriodTxt = di_jq('#lngMapPopTmPeriod').html();
            var csLineThicknessTxt = di_jq('#lngMapChartLineThicknessTxt').html();

            /* change header */
            di_jq('#dv_sec_pop_m').html('<h1>' + themesTxt + '</h1><h4>' + modiTxt + '</h4>');

            /* change left */
            htmlui = '<ul class="left_button"><li><a id="popTabTheme_m" href="javascript:void(0)" class="selected" onclick=\'clickPopPanelMap("popTabTheme_m", "' + actType + 'theme")\'>' + themeTxt + '</a></li>';
            if (actType == 'edit') {
                htmlui += '<li><a id="popTabLegend_m" href="javascript:void(0)" onclick=\'clickPopPanelMap("popTabLegend_m", "themelegend")\'>' + legendTxt + '</a></li>';
                if (isChart) {
                    htmlui += '<li><a id="popTabChart_m" href="javascript:void(0)" onclick=\'clickPopPanelMap("popTabChart_m", "chartSetting")\'>' + chSettingTxt + '</a></li>';
                }
            }
            htmlui += '</ul>';

            di_jq('#dv_sec_left_m').html(htmlui);

            /* change right */
            htmlui = '<div class="dv_sec_right_ppup_hdn_pos"><fieldset class="fieldset"><legend id="popfieldSet_m">' + themeTxt + '</legend></fieldset></div>';

            // theme tab
            //htmlui += '<div id="popTabTheme_m_sec" class="panelContainer" style="display:block;"><table width="90%" cellpadding="0" cellspacing="0"><tr><td><p>'+themeNmTxt+' <br/><input id="popThemeNmTxt_m" type="text" class="vcInputTxtBox" style="width:340px;"></p></td></tr><tr><td><p>'+themeTpTxt+' <br/><select id="popThemeType_m" class="vcInputSelBox" style="width:340px;"><option value="Color">'+thColorTxt+'</option><option value="Hatch">'+thHatchTxt+'</option><option value="DotDensity">'+thDotTxt+'</option><option value="Chart">'+thChartTxt+'</option><option value="Symbol">'+thSymbolTxt+'</option></select></p></td></tr><tr><td><p>'+themeSrTxt+' <br/><select id="popSeries_m" class="vcInputSelBox" style="width:340px;"></select></p></td></tr><tr><td><p id="shChartType">'+thChartTpSrTxt+' <br/><select id="popChartType_m" class="vcInputSelBox" style="width:150px;"><option value="Column">'+chartColumSrTxt+'</option><option value="Pie">'+chartPieSrTxt+'</option><option value="Line">'+chartLineSrTxt+'</option></select></p></td></tr></table></div>';

            htmlui += '<div id="popTabTheme_m_sec" class="panelContainer" style="display:block;"><table width="90%" cellpadding="0" cellspacing="0"><tr><td><p>' + themeTpTxt + ' <br/><select id="popThemeType_m" class="vcInputSelBox" style="width:340px;"><option value="Color">' + thColorTxt + '</option><option value="Hatch">' + thHatchTxt + '</option><option value="DotDensity">' + thDotTxt + '</option><option value="Chart">' + thChartTxt + '</option><option value="Symbol">' + thSymbolTxt + '</option></select></p></td></tr><tr><td><p>' + themeSrTxt + ' <br/><select id="popSeries_m" class="vcInputSelBox" style="width:340px;"></select></p></td></tr><tr><td><p>' + themeNmTxt + ' <br/><input id="popThemeNmTxt_m" type="text" class="vcInputTxtBox" style="width:340px;"></p></td></tr><tr><td><p id="shChartType">' + thChartTpSrTxt + ' <br/><select id="popChartType_m" class="vcInputSelBox" style="width:150px;"><option value="Column">' + chartColumSrTxt + '</option><option value="Pie">' + chartPieSrTxt + '</option><option value="Line">' + chartLineSrTxt + '</option></select></p></td></tr>';
            // code if edit theme
            if (actType == 'edit') {
                htmlui += '<tr><td><table width="350" cellpadding="0" cellspacing="0" id="di_popThemeTPCont"><tr><td><p><input type="radio" name="pop_mapThStTimePr" id="pop_mapThStTimePr1" value="TP" onclick="changeVisibilityTHTP(this.value)">' + csTmPeriodTxt + '</p></td><td><p><input type="radio" name="pop_mapThStTimePr" id="pop_mapThStTimePr2" value="MR" onclick="changeVisibilityTHTP(this.value)">' + csMostRecentTxt + '</p></td></tr><tr><td colspan="2"><p><select id="pop_mapThStTimePrMenu" class="vcInputSelBox" style="width:150px;"></select></p></td></tr></table></td></tr>';
            }
            htmlui += '</table></div>';

            // legend tab
            htmlui += '<div id="popTabLegend_m_sec" class="panelContainer" style="display:none;">';

            // legend for Color and Hatch theme types
            if (themeType == 'Color' || themeType == 'Hatch' || themeType == 'Symbol') {

                htmlui += '<table id="mapPopIdColorHatch" width="100%" cellpadding="0" cellspacing="0"><tr><td><p>' + brkCountTxt + ' <br/> <select id="pop_mapBrkCount" class="vcInputSelBox" style="width:65px;" onchange=\'setMapLegendSetting("brc", this.value)\'>' + getDropDownOptions(2, 10) + '</select></p></td><td><p>' + brkTypeTxt + ' <br/> <select id="pop_mapBrkType" class="vcInputSelBox" style="width:95px;" onchange=\'setMapLegendSetting("brt", this.value)\'><option value="EqualCount">' + brkEqCntTxt + '</option><option value="EqualSize">' + brkEqSizeTxt + '</option><option value="Continuous">' + brkContiTxt + '</option><option value="Discontinuous">' + brkDiscontTxt + '</option></select></p></td><td><p>' + lgMinTxt + ' <br/> <input id="pop_mapMin" class="vcInputSelBox" style="width:70px;" onkeyup=\'setMapLegendSetting("min", this.value)\'></p></td><td><p>' + lgMaxTxt + ' <br/> <input id="pop_mapMax" class="vcInputSelBox" style="width:70px;" onkeyup=\'setMapLegendSetting("max", this.value)\'></p></td><td><p>' + lgDecTxt + ' <br/> <select id="pop_mapDecimals" class="vcInputSelBox" style="width:50px;" onchange=\'setMapLegendSetting("dec", this.value)\'>' + getDropDownOptions(0, 15) + '</select></p></td></tr><tr><td colspan="5"><div id="mapPopIdColorHatchLegendMask" style="position:absolute;display:none;height:180px;width:414px;background: url(../../stock/themes/default/images/mask.png);text-align:center;"><img src="../../stock/themes/default/images/processing.gif" style="padding-top:15%;"></div><div id="mapPopIdColorHatchLegend" style="height:180px;border:1px solid #D3D3D3;overflow:auto;background-color:#d3d3d3;"></div></td></tr><tr><td colspan="5" align="right"><p><input type="button" value="' + smoothTxt + '" class="dv_mapButton" id="mapPopSmoothCl" onclick="getSmoothColors()"></p></td></tr><tr><td colspan="3"><form id="uplThemeSetPopFrm" name="uplThemeSetPopFrm" action="MapInputFile.aspx?UploadType=LegendInfo" method="POST" enctype="multipart/form-data">' + uploadSeTxt + ' <input type="file" id="uplThemeSetPop" name="uplThemeSetPop" size="18"><input type="hidden" name="themeId" value="' + di_jq('#di_mapTheme').val() + '"></form><input type="hidden" id="uploadedTLFileNM"></td><td colspan="2" align="right"><p><input type="button" value="' + downSetTxt + '" class="dv_mapButton" onclick="downloadMapThemeSet()" style="width:110px;"></p></td></tr></table>';

            }

            // legend for DotDensity theme type
            if (themeType == 'DotDensity') {
                htmlui += '<table id="mapPopIdDotDensity" width="90%" cellpadding="0" cellspacing="0"><tr><td width="35%"><p>' + marketStTxt + '</p></td><td><table cellspacing="0" cellpadding="0" style="width:80px;border:1px solid #d3d3d3;padding:0px;background:#fff;"><tr><td><div id="mapPopDotDtyStyle_0" class="mapDotDtyStyle_Circle" rel="Circle" style="margin:1px;"></div></td><td valign="bottom"><a href="javascript:void(0);" onclick=\'javascript:di_jq("#mapPopDotDtyStyleList").slideToggle();\'><img src="../../stock/themes/default/images/dataview/sformatdown.png" id="clickMapPopDotDensity"></a></td></tr></table> <div id="mapPopDotDtyStyleList" style="background-color:#fff;z-index:1200;width:73px; height:70px; border:solid 1px #d3d3d3;position:absolute;display:none;float:left;padding-left:5px;"><div class="mapDotDtyStyle_Circle" onclick=\'mapPopChangeDotDtyStyle("Circle")\'></div><div class="mapDotDtyStyle_Square" onclick=\'mapPopChangeDotDtyStyle("Square")\'></div><div class="mapDotDtyStyle_Triangle" onclick=\'mapPopChangeDotDtyStyle("Triangle")\'></div><div class="mapDotDtyStyle_Cross" onclick=\'mapPopChangeDotDtyStyle("Cross")\'></div></div></td></tr><tr><td><p>' + marketSzTxt + '</p></td><td><select id="mapPopDotDtySize" class="vcInputSelBox" onchange="" style="width:80px;">' + getDropDownOptions(1, 20) + '</select></td></tr><tr><td><p>' + marketCoTxt + '</p></td><td><div id="mapPopDotDtyColor" style="cursor:pointer;float:left;width:20px;height:20px;background-color:#000;"></div></td></tr><tr><td><p>' + marketVlTxt + '</p></td><td><input id="mapPopDotDtyValue" class="vcInputTxtBox" onchange="" style="width:78px;"></td></tr><tr><td><p>' + marketOpTxt + '</p></td><td><div id="mapPopDotDtyOpacity" style="background-color:#e2e2e2;width:80px;"></div><input type="hidden" id="mapPopDotDtyOpacityTxt"></td></tr></table>';
            }

            // legend for Chart theme type
            if (themeType == 'Chart') {
                htmlui += '<table id="mapPopIdChart" width="100%" cellpadding="0" cellspacing="0"><tr><td height="30" width="14%" nowrap><p>' + marketOpTxt + '</p></td><td><div id="mapPopChartOpacity" style="background-color:#e2e2e2;width:80px;"></div><input type="hidden" id="mapPopChartOpacityTxt"></td></tr><tr><td colspan="2"><div id="mapPopChartLegend" style="overflow:auto;height:200px;border:1px solid #d3d3d3;padding:5px;"></div></td></tr></table>';
            }

            htmlui += '</div>';

            // chart settings tab
            if (themeType == 'Chart') {
                htmlui += '<div id="popTabChart_m_sec" class="panelContainer" style="display:none;">';

                htmlui += '<table id="mapPopIdChartSettings" width="100%" cellpadding="0" cellspacing="0"><tr><td colspan="2"><p><input type="checkbox" id="pop_mapChartStViewDtVal"> ' + viewDataTxt + '</p></td></tr><tr><td width="35%"><p> ' + DecimalsTxt + '</p></td><td><select id="pop_mapChartStDecimals" class="vcInputSelBox" style="width:65px;">' + getDropDownOptions(0, 15) + '</select></td></tr><tr><td><p> ' + csWidthTxt + '</p></td><td><div id="pop_mapChartSliderW" style="background-color:#e2e2e2;width:80px;"></div><input type="hidden" id="pop_mapChartSliderWTxt"></td></tr><tr id="mapPopChartHghtTR"><td><p> ' + csHeightTxt + '</p></td><td><div id="pop_mapChartSliderH" style="background-color:#e2e2e2;width:80px;"></div><input type="hidden" id="pop_mapChartSliderHTxt"></td></tr><tr id="mapPopChartLineThTR" style="display:none;"><td><p> ' + csLineThicknessTxt + '</p></td><td><div id="pop_mapChartLineSliderT" style="background-color:#e2e2e2;width:80px;"></div><input type="hidden" id="pop_mapChartLineSliderTTxt"></td></tr><tr><td><p><input type="radio" name="pop_mapChartStTimePr" id="pop_mapChartStTimeSr" value="TM" onclick="changeVisibilityTMP(this.value)">' + csTimeSrTxt + '</p></td><td><p><input type="radio" name="pop_mapChartStTimePr" id="pop_mapChartStTimeMR" value="MR" onclick="changeVisibilityTMP(this.value)">' + csMostRecentTxt + '</p></td></tr><tr><td colspan="2"><div id="mapPopChartStTimeList" style="overflow:auto;height:140px;border:1px solid #d3d3d3;display:none;"></div></td></tr></table>';

                htmlui += '</div>';
            }

            //htmlui += '</td></tr></table></div>';

            // render HTML
            di_jq('#dv_sec_right_m').html(htmlui);


            //onchange of theme type
            di_jq('#popThemeType_m').bind('change', function () {
                var seriesLength = di_jq("#popSeries_m option").length;
                var first = di_jq("#popSeries_m option").eq(0);
                if (this.value == 'Chart') {
                    di_jq('#shChartType').show();
                    di_jq('#di_popThemeTPCont').hide();
                    if (seriesLength > 1) {
                        if (first.val() == '') first.remove();
                        di_jq('#popSeries_m').attr('multiple', 'multiple');
                        di_jq("#popSeries_m").multiselect({
                            selectedText: "# of # selected",
                            height: 'auto',
                            header: false
                        });
                    }
                }
                else {
                    di_jq('#shChartType').hide();
                    di_jq('#di_popThemeTPCont').show();
                    if (actType == 'add') {
                        var indx = 1;
                        z('popSeries_m').options[0] = new Option('', '');
                        di_jq('#di_mapSeries option').each(function () {
                            z('popSeries_m').options[indx] = new Option(this.text, this.value);
                            indx++;
                        });
                    }
                    if (seriesLength > 1) {
                        di_jq('#popSeries_m').removeAttr('multiple');
                        di_jq("#popSeries_m").multiselect("destroy");
                    }
                }
            });

            // bind for upload theme settings
            di_jq('#uplThemeSetPop').bind('change', function () {
                if (di_jq('#uplThemeSetPop').val() != '') {
                    doUploadThemeSettings();
                }
            });

            //onchange of series
            di_jq('#popSeries_m').bind('change', function () {
                if (this.value != '') {
                    if (actType == 'edit' || actType == 'add') {
                        // function to request for time period of selected series
                        getThemeTimePeriodOfSeries(this.value, actType);
                    }

                    //di_jq('#popThemeNmTxt_m').val('Theme - '+di_jq("#popSeries_m option[value='"+this.value+"']").text());
                }
                else {
                    di_jq('#popThemeNmTxt_m').val('');
                }
            });

            // fill series drop down menu 
            var indx = 0;
            if (actType == 'add') {
                indx = 1;
                z('popSeries_m').options[0] = new Option('', '');
            }
            di_jq('#di_mapSeries option').each(function () {
                z('popSeries_m').options[indx] = new Option(this.text, this.value);
                indx++;
            });

            // common activities
            if (isChart) di_jq('#shChartType').show();
            else di_jq('#shChartType').hide();

            break;
    }

    ApplyMaskingDiv();
    //Set close button at right corner of popup div
    SetCloseButtonInPopupDiv(di_jq('#dv_container_popup_m'), 'closeMapPropertyPopUp');
    di_jq('#dv_container_popup_m').show('slow');
    GetWindowCentered(z('dv_container_popup_m'), 580, 440);

    // apply default settings
    applyMapPopUpSetting(applySettingsOf);
}
// function to click on label row tab
function mapLblRowClick(type) {
    di_jq('#mapPopLbkRow1_con').hide();
    di_jq('#mapPopLbkRow2_con').hide();
    di_jq('#mapPopLblRow1').removeClass('selectedLblTab');
    di_jq('#mapPopLblRow2').removeClass('selectedLblTab');
    di_jq('#mapPopLbkRow' + type + '_con').show();

    di_jq('#mapPopLblRow' + type).addClass('selectedLblTab');
}
// function to toggle layer tab on map pop-up
function toggleLayerTab(tabR) {
    di_jq('#mapPopIdLayer_tabs li a').attr('class', '');
    di_jq('#' + tabR + 't').addClass('current');
    di_jq('#mapPopTab-1_sec').hide();
    di_jq('#mapPopTab-2_sec').hide();
    di_jq('#' + tabR + '_sec').show();

    if (tabR == 'mapPopTab-1') di_jq('#popupMapTab').val('baselayer');
    else if (tabR == 'mapPopTab-2') di_jq('#popupMapTab').val('featurelayer');
}
/* function to return option UI for select menu */
function getDropDownOptions(start, num) {
    var optionUi = '';
    for (var i = start; i <= num; i++) {
        optionUi += '<option value="' + i + '">' + i + '</option>';
    }
    return optionUi;
}
/* function close map pop-up*/
function closeMapPropertyPopUp() {
    RemoveMaskingDiv();
    di_jq('#dv_container_popup_m').hide('slow');
}
/* function to change left tab on map pop-up */
function clickPopPanelMap(refId, type) {
    di_jq('.left_button li > a').each(function () { di_jq(this).removeClass('selected'); });
    di_jq('#' + refId).addClass('selected');
    di_jq('.panelContainer').hide();
    di_jq('#' + refId + '_sec').show();

    di_jq('#popfieldSet_m').html(di_jq('#' + refId).html());

    applyMapPopUpSetting(type);
}

/* Start fro tritrate*/
/* function to reset menu option  */
function resetMapMenuOptions(flag) {
    if (flag == 'yes') {
        di_jq('#vcContainer').css('height', '628px').show('slow');
        di_jq('#tab-vsection').css('height', '628px').show('slow');
        di_jq('#visChartDiv').css('margin-top', '128px');
    }
    else {
        di_jq('#vcContainer').css('height', '500px');
        di_jq('#tab-vsection').css('height', '500px');
        di_jq('#visChartDiv').css('margin-top', '0px');
    }
}
/* function to open tri state of visualization */
function visual_open_tristate(type) {
    if (!di_jq.browser.mozilla) {
        if (chartVisualizationDecided == 'map3d') {
            resetMapMenuOptions('yes');
        }
        else {
            resetMapMenuOptions('no');
        }
    }
    di_jq('#visual_icon_tristate_column').hide();
    di_jq('.visual_icon_tristate[rel="column"]').css('visibility', 'visible');
    di_jq('#visual_icon_tristate_line').hide();
    di_jq('.visual_icon_tristate[rel="line"]').css('visibility', 'visible');
    di_jq('#visual_icon_tristate_bar').hide();
    di_jq('.visual_icon_tristate[rel="bar"]').css('visibility', 'visible');
    di_jq('#visual_icon_tristate_area').hide();
    di_jq('.visual_icon_tristate[rel="area"]').css('visibility', 'visible');
    di_jq('#visual_icon_tristate_map').hide();
    di_jq('.visual_icon_tristate[rel="map"]').css('visibility', 'visible');
    di_jq('#visual_icon_tristate_scatter').hide();
    di_jq('.visual_icon_tristate[rel="scatter2d"]').css('visibility', 'visible');
    di_jq('#visual_icon_tristate_' + type).slideDown();
}
/* function to open tri state of visualization */
function visual_close_tristate(type) {
    if (!di_jq.browser.mozilla) {
        if (chartVisualizationDecided == 'map3d') {
            resetMapMenuOptions('no');
        }
    }
    //var width = di_jq('#a'+type).width();
    //di_jq('#visual_icon_tristate_'+type).css('margin', '0 -3px');
    //di_jq('#visual_icon_tristate_'+type).css('width', width);
    di_jq('#visual_icon_tristate_' + type).slideUp();
}
/* End fro tritrate*/


function toggleAllFilters(FilterItem) {
    var objOAT = OAT.getSelfInstance();
    var nowToggleState = true;
    if (FilterItem.innerHTML != SetMaxChars(z('lngSelectall').value,10)) {
        nowToggleState = false;
    }

    objOAT.conditions[FilterItem.rel].blackList = [];

    try {
        for (var i = 0; i < objOAT.conditions[FilterItem.rel].distinctValues.length; i++) {
            var tmpChkFilter = z('chkFilter' + FilterItem.rel.toString() + i.toString());
            tmpChkFilter.defaultChecked = nowToggleState;
            if (!nowToggleState) {
                objOAT.conditions[FilterItem.rel].blackList.push(objOAT.conditions[FilterItem.rel].distinctValues[i]);
            }
        }
        isFiltersRedrawNeeded = false;
        objOAT.go();

        togglePivot(z('pivotChk').checked);


        if (chartVisualizationDecided == 'map') {
            var SelectedTopicsAreas = getFilteredTopicsAreas(objOAT);
            requestToMapFilter(SelectedTopicsAreas[0], SelectedTopicsAreas[1]);
            return;
        }

        // refresh chart
        showVisualizer(chartVisualizationDecided);
    }

    catch (err) {
        var errorText = err;
    }
}


function getIndicatorNIdFromIU(IU_Name) {

    var result = '';

    for (var i = 0; i < Indicators.length; i++) {
        var tmpIU = Indicators[i];
        if (tmpIU == IU_Name) {
            tmpIU = Indicators[i - 1]; // Get I_U NId format , eg. 237_9
            result = tmpIU.substr(0, tmpIU.indexOf("_")); // Fetch IndicatorNId from I_U, eg. 237
            break;
        }
    }
    return result;
}

function showFiltersMask() {
    z('divFiltersMask').style.display = 'block';
    z('divFiltersMask').style.top = z('example5').offsetTop + 'px';
    z('divFiltersMask').style.left = z('example5').offsetLeft + 'px';
    z('divFiltersMask').style.height = z('example5').offsetHeight + 'px';
    z('divFiltersMask').style.width = z('example5').offsetWidth + 'px';

}
function hideFiltersMask() {
    z('divFiltersMask').style.display = 'none';
}

// To change filter header text
function changeHeaderText() {

    try {
        var liTags = z("menu5").childNodes;
        for (var tagIndex = 0; tagIndex < liTags.length; tagIndex++) {
            var allLiTags = liTags[tagIndex].childNodes;
            switch (tagIndex.toString()) {
                case "0":
                    allLiTags[0].innerHTML = z("langAreaText").value;
                    break;
                case "1":
                    allLiTags[0].innerHTML = z("langIndicatorText").value;
                    break;
                case "2":
                    var objOat = OAT.getSelfInstance();
                    if (objOat.headerRow[2] == "Time Period") allLiTags[0].innerHTML = z("langTimePeriodText").value;
                    break;
            }
        }
        var liTags = z("menu6").childNodes;
        for (var tagIndex = 0; tagIndex < liTags.length; tagIndex++) {
            var allLiTags = liTags[tagIndex].childNodes;
            switch (tagIndex.toString()) {
                case "0":
                    allLiTags[0].innerHTML = z("lngSorting").value;
                    break;
                case "1":
                    allLiTags[0].innerHTML = z("lngHideCol").value;
                    break;
            }
        }
    }
    catch (err) { }
}

function changeFeaturesText() {
    try {
        var liTags = z("menu5").childNodes;
        for (var tagIndex = 0; tagIndex < liTags.length; tagIndex++) {
            var allLiTags = liTags[tagIndex].childNodes;
            var ulTags = allLiTags[1].childNodes;
            var spanTags = ulTags[0].childNodes[0].childNodes;
            for (var spanIndex = 0; spanIndex < spanTags.length; spanIndex++) {
                if (spanIndex != 1) {
                    switch (spanIndex) {
                        case 0:
                            spanTags[spanIndex].innerHTML = SetMaxChars(z("lngSelectall").value,10);
                            break;
                        case 2:
                            spanTags[spanIndex].innerHTML = SetMaxChars(z("lngClearall").value,10);
                            break;
                        case 3:
                            spanTags[spanIndex].innerHTML = SetMaxChars(z("lngMore").value,10) + "...";
                            break;
                    }
                }
            }
        }
        z("MRDlbltxt").innerHTML = z("lngMRD").value;
    }
    catch (err) { }
}

function changeHideColumnsText() {
    try {
        var ulTags = z("divHideColumns").childNodes[1];
        for (var tagIndex = 0; tagIndex < ulTags.childNodes.length; tagIndex++) {
            var labelNode = ulTags.childNodes[tagIndex].childNodes[1];
            switch (labelNode.innerHTML) {
                case diStructureArea:
                    labelNode.innerHTML = z("langAreaText").value; ;
                    break;
                case diStructureTopic:
                    labelNode.innerHTML = z("langIndicatorText").value; ;
                    break;
                case diStructureTimePeriod:
                    labelNode.innerHTML = z("langTimePeriodText").value; ;
                    break;
                case ulTags.childNodes.length - 1:
                    labelNode.innerHTML = z("lngMapPopAreaid").innerHTML;
                    break;
                case diStructureAreaID:
                    labelNode.innerHTML = z("langDVAreaID").value;
                    break;
                case diStructureSource:
                    labelNode.innerHTML = z("langDVSource").value;
                    break;
            }
        }
    }
    catch (err) { }
}


function SetMaxChars(InputText, Limit) {
    if (InputText.length > Limit) {
        return InputText.substr(0, (Limit-4)) + "...";
    }
    else {
        return InputText;
        }
   // return InputText;
}