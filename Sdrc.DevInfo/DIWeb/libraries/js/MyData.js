var freeTxtArData = [];
var unmatchedArData = [];
var mappedArData = [];
var currentStep = 1;

/* function to load on page load */
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, dbname, dbsummary, dbdesc, dbareacount, hLoggedInUserNId, hLoggedInUserName) {
    if (z("MyTMPform") == null) {
        z('MySubmitFormID').setAttribute("action", "DataView.aspx");
        z('MySubmitFormID').setAttribute("id", "MyTMPform");
    }
    var hsgcount = 10;
    // Set the active selection to My Data Search Navigation link
    di_jq("#aMydata").attr("class", "navActive");

    LoadLanguage(hlngcode, "HomeMaster");

    createFormHiddenInputs("frmCYV", "POST");
    SetCommonLinksHref("frmCYV", "POST");

    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    // ************************************************
    // Load Language Component
    // ************************************************
    if (GetLanguageCounts() > 1) {
        ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);
    }

    di_jq('#lng_Code').val(hlngcodedb);
    di_jq('#db_Nid').val(hdbnid);
    if (di_jq('#freeTxtData').val() != null && di_jq('#freeTxtData').val() != "") {
        doDisabledButton(['Next', 'Back'], [1, 0]);
    }
    else {
        /* action on change freetext/cyvfile*/
        di_jq('#freeTxtData').bind('keyup paste', function (e) {
            di_jq('#cyvFile').val('');
            if (di_jq('#freeTxtData').val() != '') {
                di_jq('#clearFreeTxt').show();
                doDisabledButton(['Next', 'Back'], [1, 0]);
            }
            else {
                di_jq('#clearFreeTxt').hide();
                doDisabledButton(['Next', 'Back'], [0, 0]);
            }
            if (!e.keyCode) {
                doDisabledButton(['Next', 'Back'], [1, 0]);
            }

        });
        di_jq('#cyvFile').bind('change', function () {
            di_jq('#clearFreeTxt').click();
            //di_jq('#freeTxtData').val('');
            if (di_jq('#cyvFile').val() != '')
                doDisabledButton(['Next', 'Back'], [1, 0]);
            else
                doDisabledButton(['Next', 'Back'], [0, 0]);
        });
    }
    if (z("isRowTimePeriod").checked) {
        z("isRowTimePeriod").click(z("isRowTimePeriod"));
    }
}

// JScript File
// This will parse a delimited string into an array of
// arrays. The default delimiter is the comma, but this
// can be overriden in the second argument.
function CSVToArray(strData, strDelimiter) {
    if (strData == '' || strData == null) return false;
    // Check to see if the delimiter is defined. If not,
    // then default to comma.
    strDelimiter = (strDelimiter || ",");

    // Create a regular expression to parse the CSV values.
    var objPattern = new RegExp(
                (
    // Delimiters.
                        "(\\" + strDelimiter + "|\\r?\\n|\\r|^)" +

    // Quoted fields.
                        "(?:\"([^\"]*(?:\"\"[^\"]*)*)\"|" +

    // Standard fields.
                        "([^\"\\" + strDelimiter + "\\r\\n]*))"
                ),
                "gi"
                );


    // Create an array to hold our data. Give the array
    // a default empty first row.
    var arrData = [[]];

    // Create an array to hold our individual pattern
    // matching groups.
    var arrMatches = null;


    // Keep looping over the regular expression matches
    // until we can no longer find a match.
    while (arrMatches = objPattern.exec(strData)) {

        // Get the delimiter that was found.
        var strMatchedDelimiter = arrMatches[1];

        // Check to see if the given delimiter has a length
        // (is not the start of string) and if it matches
        // field delimiter. If id does not, then we know
        // that this delimiter is a row delimiter.
        if (
                        strMatchedDelimiter.length &&
                        (strMatchedDelimiter != strDelimiter)
                        ) {

            // Since we have reached a new row of data,
            // add an empty row to our data array.
            arrData.push([]);

        }


        // Now that we have our delimiter out of the way,
        // let's check to see which kind of value we
        // captured (quoted or unquoted).
        if (arrMatches[2]) {

            // We found a quoted value. When we capture
            // this value, unescape any double quotes.
            var strMatchedValue = arrMatches[2].replace(
                                new RegExp("\"\"", "g"),
                                "\""
                                );

        } else {

            // We found a non-quoted value.
            var strMatchedValue = arrMatches[3];

        }


        // Now that we have our value string, let's add
        // it to the data array.
        //if(strMatchedValue!='' && strMatchedValue!=null) // commented on 4th May by dg26
        arrData[arrData.length - 1].push(strMatchedValue);
    }

    // Return the parsed data.
    return (arrData);
}
/* function to remove empty data in array */
function cleanArray(actual) {
    var newArray = new Array();
    for (var i = 0; i < actual.length; i++) {
        if (actual[i] && actual[i] != '') {
            newArray.push(actual[i]);
        }
    }
    return newArray;

}
var timeperiodColIndex = -1;
var areaColIndex = -1;
var headerColNameList = [];
/* function to draw table grid for data */
function drawTblFrmSourceData(csvData) {
    //di_jq('#cyvContainer').css('height', '700px');
    getNumricColIndexList(csvData);
    var arrData = cleanArray(CSVToArray(csvData, "	"));
    freeTxtArData = arrData;
    var uiHtml = '<table width="650" cellpadding="0" cellspacing="1" style="border:1px solid #d3d3d3;">';
    var totalRows = arrData.length;
    var maxRow = 6;
    var colCount = 0;

    var bgCol = '#ffffff';
    if (arrData.length < length)
        length = arrData.length;

    for (var i = 0; i < totalRows; i++) {

        var styeTR = '';
        if (i % 2 == 0) {
            bgCol = '#E7E7E7';
        }
        else {
            bgCol = '#ffffff';
        }
        colCount = arrData[i].length;
        if (colCount > 1) {
            // for identify the area id/name column
            if (i == 0) {
                uiHtml += '<tr id="cyvMapRow" style="background-color:#d3d3d3;">';
                uiHtml += '<td></td>';
                for (var j = 0; j < colCount; j++) {
                    uiHtml += '<td><select name="selArMap' + j + '" id="selArMap' + j + '" class="inputField" rel="' + j + '" onchange="timePeriodValidation()"><option value="">-'+z("Select_MyData").value+'-</option><option value="aid">' + z("Mydata_LangAreaId").value + '</option><option value="aname">' + z("Mydata_LangAreaName").value + '</option><option value="tperiod">' + z("Mydata_LangTimePeriod").value + '</option></select></td>';
                }
                uiHtml += '</tr>';
            } // end

            if (i < maxRow) {

                if (i == 0) {
                    bgCol = "#d3d3d3";
                    styeTR = 'font-weight:bold;';
                }

                uiHtml += '<tr bgcolor="' + bgCol + '" style="' + styeTR + '">';
                //uiHtml += '<td>' + '</td>';
                if (i > 0)
                    uiHtml += '<td>' + i + '</td>';
                else
                    uiHtml += '<td></td>';

                for (var j = 0; j < colCount; j++) {
                    if (arrData[i][j].toLowerCase() == "area")
                        areaColIndex = j;
                    else if (arrData[i][j].toLowerCase() == "time_period")
                        timeperiodColIndex = j;
                    uiHtml += '<td>' + arrData[i][j] + '</td>';
                }

                uiHtml += '</tr>';
            }
            else if (i == maxRow) {

                uiHtml += '<tr bgcolor="' + bgCol + '">';

                uiHtml += '<td></td>';
                for (var j = 0; j < colCount; j++) {
                    uiHtml += '<td>...</td>';
                }

                uiHtml += '</tr>';
            }
            if (i == totalRows - 1 && totalRows > maxRow) {
                (bgCol == '#E7E7E7') ? bgCol = '#ffffff' : bgCol = '#E7E7E7';
                uiHtml += '<tr bgcolor="' + bgCol + '">';

                uiHtml += '<td></td>';
                for (var j = 0; j < colCount; j++) {
                    if (arrData[i][j].toLowerCase() == "area")
                        areaColIndex = j;
                    else if (arrData[i][j].toLowerCase() == "time_period")
                        timeperiodColIndex = j;
                    uiHtml += '<td>' + arrData[i][j] + '</td>';
                }

                uiHtml += '</tr>';
            }

        }

    } // end for

    uiHtml += '</table>';

    di_jq('#csvDataTblId').html(uiHtml);
    try {
        di_jq('#selArMap' + areaColIndex).val('aname');
        di_jq('#selArMap' + timeperiodColIndex).val('tperiod');
        timePeriodValidation();
    }
    catch (err) { }
}

function getNumricColIndexList(csvData) {
    var arrData = cleanArray(CSVToArray(csvData, "	"));
    var totalRows = arrData.length;
    var colCount = 0;

    if (arrData.length < length)
        length = arrData.length;

    for (var i = 0; i < totalRows; i++) {
        colCount = arrData[i].length;
        if (colCount > 0) {
            for (var j = 0; j < colCount; j++) {
                if (i == 0)
                    headerColNameList.push(-1);
                else {
                    if (arrData[i][j] != "") {
                        if (isNumber(arrData[i][j]) && headerColNameList[j] == -1) {
                            headerColNameList[j] = j;
                        }
                    }
                }
            }
        }

    }

} // end for   

/* function */
function timePeriodValidation() {
    var show = true;
    var isshowmapsrver = false;
    var tPeriodVal = '';
    di_jq('#cyvMapRow select').each(function () {
        var selVal = di_jq(this).val();
        if (selVal == 'tperiod') {
            show = false;
            tPeriodVal = di_jq(this).attr('rel');
        }
        else if (selVal == 'aid' || selVal == 'aname') {
            isshowmapsrver = true;
        }
    });

    if (isshowmapsrver) {
        showMapServer('true');
    }
    else {
        showMapServer('false');
    }
    if (show) {
        di_jq('#isRowTimePeriodDiv').show();
        di_jq('#tPeriosCol').val('');
    }
    else {
        di_jq('#isRowTimePeriod').attr('checked', false);
        di_jq('#isRowTimePeriodDiv').hide();
        di_jq('#tPeriosCol').val(tPeriodVal);
    }
}

/* function to get unmatched area list */
function getUnmatchedArea() {

    var unmatchedDataStr = '';
    var unmatchedArray = [];
    mappedArData = [];
    unmatchedArData = [];
    var delemeter = '{}';
    var lngCode = di_jq('#hlngcodedb').val();
    var dbnid = di_jq('#hdbnid').val();
    var val = '';
    var colNum = '';
    var colNumAName = -1;
    var colNumAID = -1;

    var colcnt = 0;
    di_jq('#cyvMapRow select').each(function () {
        var selVal = di_jq(this).val();
        if (selVal == 'aid') {
            val = 'aid';
            colNum = colcnt;
            colNumAID = colcnt;
        }
        else if (val == '' && selVal == 'aname') {
            val = 'aname';
            colNum = colcnt;
            colNumAName = colcnt;
        }
        else if (selVal == 'aname') {
            colNumAName = colcnt;
        }

        colcnt++;
    });

    // set area id and name column index
    if (val == 'aid') {
        di_jq('#arIDColIndex').val(colNum);
        di_jq('#arNameColIndex').val(colNumAName);
    }
    else if (val == 'aname') {
        di_jq('#arNameColIndex').val(colNum);
        di_jq('#arIDColIndex').val(colNumAID);
    }
    else {
        di_jq('#arIDColIndex').val(colNumAID);
        di_jq('#arNameColIndex').val(colNumAName);
    }

    var selOption = di_jq('#cyvType').val();
    var service_url = 'Callback.aspx';
    var isShowMapServer = di_jq('#isShowMapServer').is(':checked');
    if (selOption == 'DataEntry') {
        // input data to be sent on service
        for (var i = 1; i < freeTxtArData.length; i++) {
            //unmatchedArray.push(freeTxtArData[i][colNum]);
            //unmatchedRowNo.push(i);
            if (i == 1) {
                unmatchedDataStr += freeTxtArData[i][colNum];
            }
            else {
                unmatchedDataStr += delemeter + freeTxtArData[i][colNum];
            }

        } // end for
        var postdata = { 'callback': 73, 'param1': selOption, 'param2': val, 'param3': unmatchedDataStr, 'param4': lngCode, 'param5': dbnid, 'param7': isShowMapServer };
    }
    else if (selOption == 'CSV') {
        var filepath = di_jq('#csvFilePath').val();
        var postdata = { 'callback': 73, 'param1': selOption, 'param2': val, 'param3': colNum, 'param4': lngCode, 'param5': dbnid, 'param6': filepath, 'param7': isShowMapServer };
    }
    var myUnmatchedList = "";
    if (val != '') {
        di_jq.ajax({
            type: "POST",
            url: service_url,
            async: false,
            data: postdata,
            success: function (responce) { //responce = 'India, Bharat{}Bhutan';
                if (responce != '') {
                    myUnmatchedList = responce;
                    var resArray = responce.split(delemeter);
                    for (var j = 0; j < resArray.length; j++) {
                        unmatchedArray.push(resArray[j]);
                        unmatchedArData.push(resArray[j]);
                        mappedArData.push(resArray[j]);
                    }
                }
            }
        });    // end callback 

        // create HTML for unmatched areas
        setUnmatchAreasHtml(unmatchedArray);
    } // end if
    else {
        di_jq('#csvDataUNMtbl').html('');
    }
    return myUnmatchedList;
} // <a href="javascript:void(0)" onclick="doAreaSelection('+i+')">Map Area</a>
/* function to create HTML for unmatched areas */
if (window.addEventListener != null) {
    window.addEventListener('click', BodyClick, false);
}
else {
    window.attachEvent('click', BodyClick, false);
}
var downArrowCol = [];
var optionDivCol = [];
function setUnmatchAreasHtml(unmatchedArray) {
    // create HTML for unmatched areas
    if (unmatchedArray.length > 0 && unmatchedArray != "") {
        var unmatchedUI = ' <form id="formTag"><table width="100%" cellpadding="0" cellspacing="1" style="border:1px solid #d3d3d3;">';
        var increaseHT = 710;
        var bgCol = '#E7E7E7';
        unmatchedUI += '<tr bgcolor="#d3d3d3"><td width="200" style="font-weight:bold;" height="25">My Data</td><td style="font-weight:bold;" width="200" height="25">Database</td></tr>';
        for (var i = 0; i < unmatchedArray.length; i++) {
       
            if (i % 2 == 0) {
                bgCol = '#ffffff';
            }
            else {
                bgCol = '#E7E7E7';
            }
            unmatchedUI += '<tr bgcolor="' + bgCol + '">';
            unmatchedUI += '<td width="50%" height="25">' + unmatchedArray[i] + ' <input type="hidden" id="mapAreaHide' + i + '" name="mapAreaHide' + i + '"> </td><td width="50%"><div id="mapAreaDiv' + i + '"><div style="width:250px;height:auto;"><table width="100%" height="100%" border="0" cellspacing="0" cellpadding="0"><tr><td id="selectedMenu' + i + '" width="90%" class="mydata_menu">Select</td><td id="downArrow"' + i + ' width="10%" class="mydata_menu_arrow" onclick="ToogleOptions(\'optionDiv' + i + '\')"></td></tr></table><div id="optionDiv' + i + '" style="width:245px;height:100px;border:1px solid #000000;display:none;z-index:3;position:absolute;background-color:#ffffff;"><div style="width:100%;height:65px;overflow-y:auto; "><table  border="0" style="width:100%" cellspacing="0" cellpadding="0"></table></div><div style="width:100%"><table  border="0" style="width:100%" cellspacing="0" cellpadding="0"><tr><td style="line-height:8px">------------------------------------------------------------</td></tr><tr><td class="menuItem" onclick="selectOption(this,\'selectedMenu' + i + '\',\'optionDiv' + i + '\', ' + i + ')"><span>More...</span></td></tr></table></div></div></div></td></div></tr>';
            increaseHT = eval(increaseHT + 30);
            if (optionDivCol.indexOf('optionDiv' + i) == -1) {
                downArrowCol.push('downArrow' + i);
                optionDivCol.push('optionDiv' + i);
            }
        }
        unmatchedUI += '</table></form>';

        di_jq('#step3Description').show();
        di_jq('#step3DescriptionMatch').hide();
        di_jq('#csvDataUNMtbl').html(unmatchedUI);

    } // end if for unmatched area
    else {
        di_jq('#step3Description').hide();
        di_jq('#step3DescriptionMatch').show();
        di_jq('#csvDataUNMtbl').html('');
    }
}

/* function to prepare mapping string */
/*function createMappingStr(val, rowNo, colNo) {
if(val!='' && val!=null) {
var addVal = rowNo+'{}'+colNo+'{}';
var mappingStr = di_jq('#mappingStr').val();
if(mappingStr!='') {
mappingStr += '{||}';
}
var searchStr = mappingStr.search(addVal);
if(searchStr>-1) {
mappingStr = mappingStr.replace("Microsoft", addVal + val)
mappingStr += addVal + val
}
		

di_jq('#mappingStr').val(mappingStr);
}
}*/
var AreaCompObj;
/* function to show area component */
function doAreaSelection(id) { //alert('mapAreaDiv' +id);
    ApplyMaskingDiv();

    //Set close button at right corner of popup div
    SetCloseButtonInPopupDiv(di_jq('#AreaOuterBox'), 'hideAreaSelection');
    di_jq('#AreaOuterBox').show('slow');
    GetWindowCentered(z('AreaOuterBox'), 700, 490);

    var JsonData = z('mapAreaHide' + id).value;

    ////////var codeListPath = getAbsURL('stock') + 'data/' + z("hdbnid").value + '/ds/' + z("hlngcodedb").value + "/";
    var useMapServer = z('isShowMapServer').checked;
    var absPath = getAbsURL('stock');
    var codeListPath = '';
    if (useMapServer) {      
        codeListPath = getMapServerPath();
    }
    else {
        codeListPath = absPath + 'data/' + z("hdbnid").value + '/ds/' + z("hlngcodedb").value + "/";
    }


    var opts = {
        title: '', 											// Title caption  
        iconClose: false, 									// Show close icon on right of the title box
        width: '700', 										// Width
        height: '320', 										// Height
        codelistPath: codeListPath + 'area', 				// Area Codelist Path
        metadataPath: codeListPath + 'metadata', 			// Metadata folder path
        flagIconPath: '', 									// Country Icon URL
        multiple: false, 										// Multiselect - true/false
        mode: 'tree', 										// Display mode - both, tree, list
        defaultMode: 'tree', 							// default display mode tree/list_level
        outputFormat: 'aid_name', 								// aid, nid, name, nid_name, aid_name, aid_nid_name
        showAreaId: true, 									// Show Area ID after area name. true/false
        quickSel: false, 										// Provide options for quick selection true/false
        quickSelMode: 'HRZ', 								// Quick Selection Arrangement Mode - HRZ|VERT
        showFooter: false, 									// Show footer with OK|Cancel buttons,
        showViewSel: false, 									// Show view selection panel
        callbacks: {
            hotSel: 'AreaHotSelectionFunc(' + id + ')', 	// hot selection function if multiple false
            whereDataExist: 'get_area_where_data_exists', // function where data exist
            areaMap: ''										// Callback URl for Area Map. (keep null for hiding)
        }, 											// callback functions
        jsonSelection: JsonData, 							// JSON format to persist area selection default
        labels: {												// Static Labels
            selectAll: z('langHiddenSelectAll').value,
            byList: z('langHiddenListAtoZ').value,
            byTree: z('langHiddenByTree').value,
            byMap: z('langHiddenByMap').value,
            search: z('langHiddenSearch').value,
            viewSel: z('langHiddenViewselections').value,
            metadata: z('langHiddenMetadata').value,
            dataExist: z('langHiddenShowwheredataexist').value,
            clear: z('langHiddenClear').value,
            close: z('langHiddenClose').value,
            first: z('langHiddenFirst').value,
            last: z('langHiddenLast').value,
            next: z('langHiddenNext').value,
            back: z('langHiddenBack').value,
            page: z('langHiddenPage').value,
            ok: 'OK',
            cancel: 'Cancel'
        }
    };

    AreaCompObj = new di_drawAreaSelectionList('area_div_popup', opts);

}
/* function to hide area pop-up*/
function hideAreaSelection() {
    RemoveMaskingDiv();
    di_jq('#AreaOuterBox').hide('slow');
    di_jq('#area_div_popup').html('');
    di_loading('hide');
}

/* function to hide mapserver pop-up*/
function hideMapServerPopup() {
    RemoveMaskingDiv();
    di_jq('#MapServerPopup').hide('slow');
    di_loading('hide');
}
function checkUseMapServerChkBox() {
    if (IsDESFile) {
        di_jq('#ShowMapServer').attr("value", "true") ;
        currentStep = 1;
        doNext();
    }   
    di_jq('#isShowMapServer').trigger('click');
    hideMapServerPopup();
}

/* function for area hot selection */
function AreaHotSelectionFunc(id) {
    var mappingStr = '';
    var selectedArea = AreaCompObj.getSelectedAreas();
    var selectedArea = selectedArea.split("||{~~}||");
    if (selectedArea[0] != '') {
        var area_data = selectedArea[0].split("_");
        var areaID = area_data[0];
        var areaName = area_data[1];

        var div = document.getElementById(optionDivCol[id]);
        var findString = areaName + " (" + areaID + ")";
        if (div.innerHTML.indexOf(findString) == -1) {
            var table = div.childNodes[0].childNodes[0];
            var row = table.insertRow(0);
            var rowHTML = "<td class='menuItem' onclick=\"selectOption(this,'selectedMenu" + id + "','optionDiv" + id + "', " + id + ")\"><span>" + areaName + " (" + areaID + ")" + "</span></td>";
            di_jq(row).html(rowHTML);
            di_jq(row.cells[0]).trigger('click');
            //row.cells[0].addEventListener('click', selectOption(row.cells[0], "selectedMenu" + id, "optionDiv" + id, id), false);
            if (table.innerHTML.indexOf("No Match") > -1) {
                var lastRowIndex = table.rows.length;
                table.deleteRow(lastRowIndex - 1);
            }
        }
        else {
            selectByValue(div, findString);
        }
        mappedArData[id] = unmatchedArData[id] + '{}' + areaID;

        di_jq('#mapAreaHide' + id).val('');

    }
    createMappingStr();

    hideAreaSelection();
    closeAlldiv();
}
/* function to create mapping string */
function createMappingStr() {
    var mappingStr = "";
    for (var i = 0; i < mappedArData.length; i++) {
        if (i == 0) {
            mappingStr = mappedArData[i];
        }
        else {
            mappingStr += '{||}' + mappedArData[i];
        }
    }
    di_jq('#mappingStr').val(mappingStr);
}
/* function to remove mapped area */
function removeAreaMapped(id, val) {
    var mappingAreaStr = mappedArData[id];
    if (mappedArData[id] != undefined) {
        var areaName = mappingAreaStr.split("{}")[0];
        //var areaId = mappingAreaStr.split("{}")[1];
        var lastIndex = val.lastIndexOf(")");
        var tempStr = val.substr(0, lastIndex);
        var firstIndex = tempStr.lastIndexOf("(");
        var areaId = tempStr.substr(firstIndex + 1);
        mappedArData[id] = unmatchedArData[id] + "{}" + areaId;
        //di_jq('#mapAreaDiv'+id).html('<a href="javascript:void(0)" onclick="doAreaSelection('+id+')">Map Area</a>');

        createMappingStr();
    }
}

/* Function for CYV file uploading  */

/*function submitCYVfile()
{
if(z('cyvFile').value!='') {
var form = document.createElement("form");
						
form.setAttribute("method", "post");
form.setAttribute("action", "DataView.aspx");
form.setAttribute("name", "MyTMPform");
form.setAttribute("id", "MyTMPformID");
form.setAttribute("enctype", "multipart/form-data");
		
form.appendChild(z('cyvFile'));
document.body.appendChild(form);
							
form.submit();
}
else if(z('freeTxtData').value!='') { 
var form = document.createElement("form");
form.setAttribute("method", "post");
form.setAttribute("action", "DataView.aspx");
form.setAttribute("name", "MyTMPform");
form.setAttribute("id", "MyTMPformID");
form.setAttribute("enctype", "multipart/form-data");
		
form.appendChild(z('freeTxtData'));
document.body.appendChild(form);
							
form.submit();
}
}*/
/* function to finally submit to view data */
function submitCYVfile() {//MySubmitFormID
    try {
        di_jq('#cyvFile').val("");
        document.getElementById('MyTMPform').setAttribute("action", "DataView.aspx");
        //document.getElementById('MyTMPform').setAttribute("id", "MySubmitFormID");
        document.getElementById('MyTMPform').submit();
    }

    catch (err) {
        var ErrorMessage = err.Message;
    }
}

/*#() */
function uploadSpreadSheetSDMX() {

    var val = di_jq('#cyvType:checked').val();
    if (val == 'Spreadsheet' || val == 'SDMX-ML') {
        document.getElementById('MyTMPform').setAttribute("action", "DataView.aspx");
        //document.getElementById('MyTMPform').setAttribute("id", "MySubmitFormID");
        document.getElementById('MyTMPform').submit();
    }

}


//****************************************************************
/* function to get file extention of the file */
function getFileExtention(filename) {
    return filename.split('.').pop();
}
/* function to click on  next button */
var IsDESFile = false;
function doNext() {
    var c = di_jq('#MyTMPform')[0].children;
    if (currentStep == 1) { // step 1
        if (di_jq('#freeTxtData').val() != '') {
            IsDESFile = false;
            drawTblFrmSourceData(di_jq('#freeTxtData').val());
            di_jq('#cyvType').val('DataEntry');
            if (z("isRowTimePeriod").checked) {
                z("isRowTimePeriod").click(z("isRowTimePeriod"));
            }
            setDataStep(2);
        }
        else if (di_jq('#cyvFile').val() != '') {

            var fileType = getFileExtention(di_jq('#cyvFile').val()).toLowerCase(); // get file type

            di_jq('#MyTMPform').ajaxForm({
                success: function (responce) {
                    if (responce == "invalid") {
                        alert("Invalid Format");
                        return;
                    }
                    var responceAr = responce.split('{[***]}');
                    if (responceAr[1] == "Invalid Record") {
                        alert("Invalid Format");
                        return;
                    }

                    di_jq('#csvFilePath').val(responceAr[0]);
                    z("isRowTimePeriod").defaultChecked = false;
                    //di_jq('#onlineDataEnrty').hide();
                    //di_jq('#freeTxtData').val(responceAr[1]);			        
                    // case for csv
                    if (fileType == 'csv') {
                        drawTblFrmSourceData(responceAr[1]);
                        di_jq('#cyvType').val('CSV');
                        setDataStep(2);
                    }
                    else if (fileType == 'xls' || fileType == 'xlsx') { // spread sheet case
                        //drawTblFrmSourceData(responceAr[1]);
                        IsDESFile = true;
                        di_jq('#cyvType').val('Spreadsheet');
                        var resArray = responceAr[1].split("{}");
                        var unmatchedArray = [];
                        unmatchedArData = [];
                        mappedArData = [];
                        for (var j = 0; j < resArray.length; j++) {
                            if (resArray[j].length != 0) {
                                unmatchedArray.push(resArray[j]);
                                unmatchedArData.push(resArray[j]);
                                mappedArData.push(resArray[j]);
                            }
                        }
                        if (unmatchedArray.length != 0) {
                            showMapServer('true');
                            setUnmatchAreasHtml(unmatchedArray);
                            getMatchingAreas(unmatchedArray, 'obj');
                        }
                        else {
                            //setUnmatchAreasHtml(unmatchedArray);
                            // getMatchingAreas(unmatchedArray, 'obj');
                            submitCYVfile();
                        }

                        //getMappingDetails(unmatchedArray, 'obj');
                        //setDataStep(3);
                    }
                    else if (fileType == 'xml') { // sdmx-ml case
                        di_jq('#cyvType').val('CSV');
                        drawTblFrmSourceData(responceAr[1]);
                        setDataStep(2);
                    }
                }
            });
            di_jq('#MyTMPform').submit();
        }
    }
    else if (currentStep == 2) {
        di_jq('#mappingStr').val('');
        var unmatchedlist = getUnmatchedArea();
        if (unmatchedlist != "") {
            getMatchingAreas(unmatchedlist, 'str');
        }
        else {
            submitCYVfile();
        }
    }
    else if (currentStep == 3) {
        storeMappingDetails();
    }

}

var useMapserverChkbxChecked = false;
/*function to click on usemap server check changed*/
function useMapServerCheckChanged(checkBoxObj) {
    if (checkBoxObj.checked) {
        di_jq('#ShowMapServer').attr("value", "true");
        useMapserverChkbxChecked = true;
    }
    else {
        di_jq('#ShowMapServer').attr("value", "false");
    }
    if (!IsDESFile) {
        currentStep = 2;
        doNext();
    }
}

/* function to click on  back button */
function doBack() {
    if (currentStep > 1) {
        var cyvtype = di_jq('#cyvType').val();
        var backCt = 1;
        if (cyvtype == 'Spreadsheet' || cyvtype == 'SDMX-ML') backCt = 2;
        useMapserverChkbxChecked = false;
        di_jq('#NextBttn').val("Next");
        setDataStep(currentStep - backCt);
    }
}
/* function to disabled/enable next/back buttons */
function doDisabledButton(typeArray, actArray) {

    for (var i = 0; i < typeArray.length; i++) {
        var act = actArray[i];
        var bttn = typeArray[i];
        if (act == 1) {
            di_jq('#' + bttn + 'Bttn').attr('disabled', false);
            di_jq('#' + bttn + 'Bttn').attr('class', 'di_gui_button');
        }
        else {
            di_jq('#' + bttn + 'Bttn').attr('disabled', true);
            di_jq('#' + bttn + 'Bttn').attr('class', 'di_gui_button_disabled');
        }
    } // end for
}
/* function to show/hide steps */
function setDataStep(step) {
    di_jq('#mydata_step1').hide();
    di_jq('#mydata_step2').hide();
    di_jq('#mydata_step3').hide();

    di_jq('#mydata_step' + step).show('slow');
    currentStep = step;

    if (step == 1) {
        doDisabledButton(['Next', 'Back'], [1, 0]);
        di_jq('#NextBttn').val(z("NextBttn").value);
        di_jq('#tPeriosCol').val('');
    }
    else if (step == 2) {
        doDisabledButton(['Next', 'Back'], [1, 1]);
        di_jq('#NextBttn').val(z("NextBttn").value);
    }
    else if (step == 3) {
       
            if (!di_jq('#isShowMapServer')[0].checked) {
                if (!useMapserverChkbxChecked && isShowMapServer == 'true') {
                    showMapserverPopup();
                }
            
        }
        doDisabledButton(['Next', 'Back'], [1, 1]);
        di_jq('#NextBttn').val(z("langHiddenDataview").value);
    }
}


function showMapserverPopup() {
    ApplyMaskingDiv();
    //Set close button at right corner of popup div
    SetCloseButtonInPopupDiv(di_jq('#MapServerPopup'), 'hideMapServerPopup');
    di_jq('#MapServerPopup').show('slow');
    GetWindowCentered(z('MapServerPopup'), 500, 200);
}

/* function to clear text area field */
function clearField() {
    di_jq('#freeTxtData').val('');
    di_jq('#clearFreeTxt').hide();
    doDisabledButton(['Next', 'Back'], [0, 0]);
    di_jq('#freeTxtData').focus();
}
/* function for storing mapping details */
function storeMappingDetails() {
    var areaIdSelection = z("arIDColIndex").value;
    var areaNameSelection = z("arNameColIndex").value;
    var mappingDetials = "";
    var mappingStr = z("mappingStr").value;
    var mappingAreaList = mappingStr.split("{||}");
    var type = "aid";
    for (var i = 0; i < mappingAreaList.length; i++) {
        var mappingAreaInfo = mappingAreaList[i];
        if (mappingAreaInfo.indexOf("{}") > -1) {
            mappingDetials += "{||}" + mappingAreaInfo;
        }
    }
    if (areaIdSelection == -1 && areaNameSelection != -1) {
        type = "aname";
    }
    var lngCode = z("lng_Code").value;
    var dbNid = z("db_Nid").value;
    var isShowMapServer = di_jq('#isShowMapServer').is(':checked');
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: { 'callback': 266, 'param1': mappingDetials, 'param2': type, 'param7': isShowMapServer },
        async: true,
        success: function (result) {
            try {
                if (isShowMapServer) {
                    di_jq('#ShowMapServer').attr("value", "true");
                }

                submitCYVfile();
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            
        },
        cache: false
    });
}

/*Gets path of map server*/
function getMapServerPath() {
    var path;
    var isShowMapServer = di_jq('#isShowMapServer').is(':checked');
    var langCode = z("hlngcodedb").value;
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: { 'callback': 1028, 'param1': langCode, 'param7': isShowMapServer },
        async: false,
        success: function (data) {
            try {
                path = data;
            }
            catch (ex) {
                alert("Error :" + ex.message);
            }
        },
        error: function () {
            
        },
        cache: false
    });
    return path;
}
/* function for retriving mapping details */
function getMappingDetails(unmatchedListArray, listType) {
    var unmatchedList = "";
    var areaIdSelection = z("arIDColIndex").value;
    var areaNameSelection = z("arNameColIndex").value;
    //if (areaIdSelection == -1 && areaNameSelection == -1)
    //return;
    var type = "aid";
    if (areaIdSelection == -1 && areaNameSelection != -1) {
        type = "aname";
    }
    var lngCode = z("lng_Code").value;
    var dbNid = z("db_Nid").value;
    if (listType == "obj") {
        for (var i = 0; i < unmatchedListArray.length; i++) {
            unmatchedList += "{}" + unmatchedListArray[i];
        }
        if (unmatchedList.length > 0)
            unmatchedList = unmatchedList.substr(2);
    }
    else {
        unmatchedList = unmatchedListArray;
    }
    var isShowMapServer = di_jq('#isShowMapServer').is(':checked');
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: { 'callback': 267, 'param1': unmatchedList, 'param2': type, 'param3': dbNid, 'param4': lngCode, 'param7': isShowMapServer },
        async: true,
        success: function (data) {
            try {
                var selectedArea = data.split("{@@}");
                if (selectedArea.length > 0) {
                    for (var id = 0; id < selectedArea.length; id++) {
                        if (selectedArea[id] != "") {
                            var mapping_data = selectedArea[id].split("{}");
                            var key = mapping_data[0];
                            var areaID = mapping_data[1];
                            var areaName = mapping_data[2];
                            if (areaID.length >= 1) {
                                var div = document.getElementById(optionDivCol[id]);
                                var table = div.childNodes[0].childNodes[0];
                                var findString = areaName + " (" + areaID + ")";
                                for (var rowIndex = 0; rowIndex < table.rows.length; rowIndex++) {
                                    if (table.rows[rowIndex].innerHTML.indexOf(findString) != -1) {
                                        table.deleteRow(rowIndex);
                                        break;
                                    }
                                }
                                // if (table.innerHTML.indexOf(findString) == -1) {
                                var row = table.insertRow(0);
                                var rowHTML = "<td class='menuItem' onclick=\"selectOption(this,'selectedMenu" + id + "','optionDiv" + id + "', " + id + ")\"><span>" + areaName + " (" + areaID + ")" + "</span></td>";
                                di_jq(row).html(rowHTML);
                                di_jq(row.cells[0]).trigger('click');
                                //row.cells[0].addEventListener('click', selectOption(row.cells[0], "selectedMenu" + id, "optionDiv" + id, id), false);
                                if (table.innerHTML.indexOf("No Match") > -1) {
                                    var lastRowIndex = table.rows.length;
                                    table.deleteRow(lastRowIndex - 1);
                                }
                                // }
                            }
                            mappedArData[id] = unmatchedArData[id] + '{}' + areaID;
                        }
                        //di_jq('#mapAreaHide' + id).val('');
                    }

                }
                createMappingStr();
            }
            catch (ex) {
                alert("Error :" + ex.message);
            }
            setDataStep(3);
            hideLoader(di_jq('#reg_lft_sec'));
        },
        error: function () {
            
        },
        cache: false
    });
    closeAlldiv();
}


function showLoader(target) {
    di_jq('#mydata_step1').hide();
    di_jq('#mydata_step2').hide();
    di_jq('#mydata_step3').hide();
    di_jq('#loaderDiv').show();
}
// When ajaxStop is fired, rmeove 'loading' from body class
function hideLoader(target) {
    di_jq('#mydata_step3').show('slow');
    di_jq('#loaderDiv').hide();
}


function selectByValue(ddlObject, value) {
    var table = ddlObject.childNodes[0].childNodes[0];
    var id = ddlObject.id.replace("optionDiv", "").trim();
    var rows = table.getElementsByTagName("tr");
    for (var rowIndex = 0; rowIndex < rows.length; rowIndex++) {
        if (rows[rowIndex].innerHTML.indexOf(value) != -1) {
            table.deleteRow(rowIndex);
            break;
        }
    }
    var row = table.insertRow(0);
    var rowHTML = "<td class='menuItem' onclick=\"selectOption(this,'selectedMenu" + id + "','optionDiv" + id + "', " + id + ")\"><span>" + value + "</span></td>";
    di_jq(row).html(rowHTML);
    di_jq(row.cells[0]).trigger('click');
    if (table.innerHTML.indexOf("No Match") > -1) {
        var lastRowIndex = table.rows.length;
        table.deleteRow(lastRowIndex - 1);
    }
}

function getDivIndex(divId) {
    var index = -1;
    for (var i = 0; i < optionDivCol.length; i++) {
        if (optionDivCol[i] == divId) {
            index = i;
            break;
        }
    }
    return index;
}

function ToogleOptions(divId) {
    var obj = document.getElementById(divId);
    if (obj.style.display == 'none') {
        closeAlldiv();
        obj.style.display = '';
    }
    else
        obj.style.display = 'none';
}
function selectOption(menu, ctrlId, divId, countid) {
    var selectedMenuItem = menu.childNodes[0].innerHTML;
    //if(selectedMenuItem==undefined) selectedMenuItem = 'No Match';
    //document.getElementById(ctrlId).innerHTML = selectedMenuItem;
    di_jq('#' + ctrlId).html(selectedMenuItem);
    ToogleOptions(divId);
    if (selectedMenuItem == "No Match")
        return;
    var id = getDivIndex(divId);
    removeAreaMapped(id, selectedMenuItem);
    if (selectedMenuItem == "More...") {
        //var itemNumber = ctrlId.substr(ctrlId.length - 1);
        doAreaSelection(countid);
    }
}
function BodyClick(e) {
    if (downArrowCol.indexOf(e.target.id) > -1 || e.target.id == "downArrow")
        return;
    if (e.target.id != "formTag")
        closeAlldiv();
}

function closeAlldiv() {
    for (var i = 0; i < optionDivCol.length; i++) {
        var optionObj = document.getElementById(optionDivCol[i]);
        if (optionObj)
            optionObj.style.display = "none";
    }
}
function disableAllTimePeriodDdl(checkboxObject) {
    var allDisable = false;
    if (checkboxObject.checked)
        allDisable = true;
    else
        allDisable = false;
    var divId = z("csvDataTblId");
    var table = divId.childNodes[0];
    var totalCols = table.rows[0].cells.length;
    var numberColsIndex = "";
    for (var i = 0; i < headerColNameList.length; i++) {
        if (headerColNameList[i] > -1)
            z("selArMap" + headerColNameList[i]).disabled = allDisable;
        numberColsIndex += "," + headerColNameList[i];
    }
    if (allDisable)
        di_jq('#tPeriosCol').val(numberColsIndex);
    else
        di_jq('#tPeriosCol').val('');
}
function isNumber(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

/* function for retriving matching areas*/
function getMatchingAreas(unmatchedListArray, listType) {
    var unmatchedList = "";
    var areaIdSelection = z("arIDColIndex").value;
    var areaNameSelection = z("arNameColIndex").value;
    //if (areaIdSelection == -1 && areaNameSelection == -1)
    //return;
    var type = "aid";
    if (areaIdSelection == -1 && areaNameSelection != -1) {
        type = "aname";
    }
    var lngCode = z("lng_Code").value;
    var dbNid = z("db_Nid").value;
    if (listType == "obj") {
        for (var i = 0; i < unmatchedListArray.length; i++) {
            unmatchedList += "{}" + unmatchedListArray[i];
        }
        if (unmatchedList.length > 0)
            unmatchedList = unmatchedList.substr(2);
    }
    else {
        unmatchedList = unmatchedListArray;
    }
    var isShowMapServer = di_jq('#isShowMapServer').is(':checked');

    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: { 'callback': 269, 'param1': unmatchedList, 'param2': type, 'param3': dbNid, 'param4': lngCode, 'param7': isShowMapServer },
        async: true,
        beforeSend: function () {
            showLoader(di_jq('#reg_lft_sec'));
        },
        success: function (data) {
            try {
                var selectedArea = data.split("{@@}");
                if (selectedArea.length > 0) {
                    for (var id = 0; id < selectedArea.length; id++) {
                        if (selectedArea[id] != "") {
                            var div = document.getElementById(optionDivCol[id]);
                            var table = div.childNodes[0].childNodes[0];
                            var mapping_data = selectedArea[id].split("{@@@@}");
                            var unmatchedArea = mapping_data[0];
                            var SuggestionsArea = mapping_data[1];
                            if (SuggestionsArea == "no") {
                                var row = table.insertRow(0);
                                //commented to Fix IE issue and added below two lines
                                //                                        row.innerHTML = "<td class='menuItem' onclick=\"selectOption(this,'selectedMenu" + id + "','optionDiv" + id + "')\"><span>No Match</span></td>";
                                var rowHTML_No = "<td class='menuItem' onclick=\"selectOption(this,'selectedMenu" + id + "','optionDiv" + id + "', " + id + ")\"><span>No Match</span></td>";
                                di_jq(row).html(rowHTML_No);

                                di_jq(row.cells[0]).trigger('click');

                                //row.cells[0].addEventListener('click', selectOption(row.cells[0], "selectedMenu" + id, "optionDiv" + id, id), false);
                            }
                            else {
                                var AidNameCol = SuggestionsArea.split("||"); // Area Id & AreaName Collection
                                for (var i = 0; i < AidNameCol.length; i++) {
                                    var areaID = AidNameCol[i].split("{}")[0];
                                    var areaName = AidNameCol[i].split("{}")[1];

                                    var findString = areaName + " (" + areaID + ")";
                                    if (table.innerHTML.indexOf(findString) == -1) {
                                        var row = table.insertRow(0);
                                        //commented to fix IE issue and added below two lines
                                        //                                                row.innerHTML = "<td class='menuItem' onclick=\"selectOption(this,'selectedMenu" + id + "','optionDiv" + id + "')\"><span>" + areaName + " (" + areaID + ")" + "</span></td>";
                                        var rowHTML = "<td class='menuItem' onclick=\"selectOption(this,'selectedMenu" + id + "','optionDiv" + id + "', " + id + ")\"><span>" + areaName + " (" + areaID + ")" + "</span></td>";
                                        di_jq(row).html(rowHTML);

                                        //di_jq(row.cells[0]).trigger('click');

                                        //row.cells[0].addEventListener('click', selectOption(row.cells[0], "selectedMenu" + id, "optionDiv" + id, id), false);
                                    }
                                }
                            }
                        }
                    }

                }
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
            getMappingDetails(unmatchedList, 'str');
        },
        error: function () {
            
        },
        cache: false
    });
    //closeAlldiv();
}

/* function to go on MyData Note Popup */
function myDataNote() {
    try {

        ApplyMaskingDiv();

        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#myDataNote'), 'hidePopupSelection');

        di_jq('#myDataNote').show('slow');
        GetWindowCentered(z('myDataNote'), 720, 400);

    }
    catch (err) { }
}

/* function to hide pop */
function hidePopupSelection() {
    try {
        //	di_jq('#GradOutDiv').hide();
        RemoveMaskingDiv();
        di_jq('#myDataNote').hide('slow');
        //di_loading('hide');
    }
    catch (err) { }
}

/* function to show hide map server checkbox */
function showMapServer(action) {
    if (action == 'true' && isShowMapServer == 'true') {
        di_jq('#isShowMapServerDiv').show();
        di_jq('#isShowMapServer').attr('disabled', false);
    }
    else {
        di_jq('#isShowMapServerDiv').hide();
        di_jq('#isShowMapServer').attr('disabled', true);
    }
}