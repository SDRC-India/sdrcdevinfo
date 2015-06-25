var StopGalleryLoadingProcess = false;
var GalleryEnableFlag;
var SelectedDimension = '';
var isCatalogResults = false;

var UniqueId_CloudData = [];
var AllIdUniquePart = [];

var LiDimensionsArr = [];

var counterCurrentJsonResult = 0;
var tmpBunchJsonResultsData = [];
var tmpDivForResults, tmpSpanLoadingOuterID, tmpSpanLoadingID, tmpIsExploded, GalleryXML_Data;

function onPageLoadOfQDS(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName, enableQDSGallery, JsonResultData) {
    
    var hsgcount = 10;
    // Set the active selection to Data Search Navigation link    
    di_jq("#aDS").attr("class", "navActive");

    // Hide background color div
    di_jq("#apDiv2").css("display", "none");

    createFormHiddenInputs("frmHome", "POST"); //CHANGE : frmQDS

    if (hdsby != "share") {
        SetCommonLinksHref("frmHome", "POST"); //CHANGE : frmQDS
    }

    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    if (hdsby != "share") {
        InitializeDatabaseComponent(getAbsURL('stock'), 'database_div', hdbnid, hlngcodedb, 550, 460, true, "OkDbComponent", "CloseDbComponent");
        ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);

        if ((hselindo.search("[****]") != -1) && (hselareao.search("[****]") != -1)) {
            ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div', hdbnid, hlngcodedb, '100%', '100px', 'GetQDSResult', hselindo.split("[****]")[0], hselareao.split("[****]")[0], hselindo.split("[****]")[1], hselareao.split("[****]")[1], false);
        }
        else if ((hselindo.search("[****]") == -1) && (hselareao.search("[****]") != -1)) {
            ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div', hdbnid, hlngcodedb, '100%', '100px', 'GetQDSResult', '', hselareao.split("[****]")[0], '', hselareao.split("[****]")[1], false);
        }
        else if ((hselindo.search("[****]") != -1) && (hselareao.search("[****]") == -1)) {
            ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div', hdbnid, hlngcodedb, '100%', '100px', 'GetQDSResult', hselindo.split("[****]")[0], '', hselindo.split("[****]")[1], '', false);
        }
        else if ((hselindo.search("[****]") == -1) && (hselareao.search("[****]") == -1)) {
            ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div', hdbnid, hlngcodedb, '100%', '100px', 'GetQDSResult', '', '', '', '', false);
        }

        DrawDatabaseDetailsComponent(hdbnid, 'db_details_div');
    }

    GalleryEnableFlag = enableQDSGallery;

    if (hdsby == "qds") {        
        GetQDSResult();
    }
    else if (hdsby == "as") {
        ShowDbComponent();
    }
    else if (hdsby == "share") {
        var GalleryXML = z('GalleryXML').value;
        RenderResult(JsonResultData, 'divForQdsResult', false, "", GalleryXML);
    }
}


function GetASResult() {
    var IndicatorIC, Areas, LngCode, ASDatabaseIds;
    var IndicatorText, IndicatorBlockText, AreaText, AreaBlockText;
    var spanLoading, spanLoadingOuter, divQDSResults, spanTimeCounter;

    IndicatorText = di_qds_get_ind_search_data();
    IndicatorBlockText = di_qds_get_all_ind_block_data();
    AreaText = di_qds_get_area_search_data();
    AreaBlockText = di_qds_get_all_area_block_data();

    spanLoading = z('spanLoading');
    spanLoadingOuter = z('spanLoadingOuter');
    spanTimeCounter = z('spanTimeCounter');
    divQDSResults = z('divQDSResults');

    IndicatorIC = GetTextAndBlockTextCombinedValue(IndicatorText, IndicatorBlockText);
    Areas = GetTextAndBlockTextCombinedValue(AreaText, AreaBlockText);
    LngCode = z('hlngcodedb').value;
    ASDatabaseIds = di_db_getAllSelDbNIds();

    if (((IndicatorIC != null && IndicatorIC != "") || (Areas != null && Areas != "")) &&
        (ASDatabaseIds != null && ASDatabaseIds != "")) {
        StopGalleryLoadingProcess = true;
        divQDSResults.innerHTML = "";
        spanTimeCounter.innerHTML = "";

        GetASResultForDBNumber(IndicatorIC, Areas, LngCode, ASDatabaseIds, 0, 0, 0);
    }
    else {
        divQDSResults.innerHTML = "";
        spanTimeCounter.innerHTML = "";
        spanLoadingOuter.style.display = "none";
        spanLoading.innerHTML = "";
    }
}

function getAreaLevel(AreaName, AreaNId, DbNId, LangCode) {

    var xmlFilePath = AreaName.substr(0, 1).toLowerCase() + ".xml";
    var XmlPageURL = getAbsURL('stock') + "data/" + DbNId + "/ds/" + LangCode + "/area/areasearch/" + xmlFilePath;

    var result = "";

    di_jq.ajax({
        type: "GET",
        url: XmlPageURL,
        dataType: "xml",
        async: false,
        timeout: 5000,
        error: function (request, error) {
        },
        success: function (xml) {
            $(xml).find('a').each(function () {
                var tmpAreaNId = $(this).attr('nid');
                if (tmpAreaNId == AreaNId) {
                    result = $(this).attr('l'); ;
                    return;
                }
            });

        }
    });

    return result;

}

function getIUSNId(IndicatorNId, UnitNId, DbNId, LangCode) {

    var xmlFileName = "ius_" + IndicatorNId + "_" + UnitNId + ".xml";
    var XmlPageURL = getAbsURL('stock') + "data/" + DbNId + "/ds/" + LangCode + "/ius/" + xmlFileName;

    var result = [];
    var allIUS = [];


    di_jq.ajax({
        type: "GET",
        url: XmlPageURL,
        dataType: "xml",
        async: false,
        timeout: 5000,
        error: function (request, error) {
        },
        success: function (xml) {

            $(xml).find('iu').each(function () {
                var ius = $(this).attr('defiusnid');
                if (ius != undefined && ius != null) {
                    result.push(ius);
                    return;
                }
            });

            $(xml).find('ius').each(function () {
                var iusnid = $(this).attr('iusnid');
                var n = $(this).attr('n');
                allIUS.push(iusnid + "||" + n);
            });

        }
    });

    result.push(allIUS);

    return result;

}

function RenderResults(jsonResultsData, divForResults, spanLoadingOuterID, spanLoadingID, isExploded, DbResultsLoadingTime, CatalogAdaptationURL, Gallery_XML_Data) {
    z('hdsby').value = "qds";
    if (!isExploded) reGroupBlockAreas(jsonResultsData);

    if (DbResultsLoadingTime != null) RenderTimeTaken(jsonResultsData[""].length, DbResultsLoadingTime);

    counterCurrentJsonResult = 0;

    tmpDivForResults = divForResults;
    tmpSpanLoadingOuterID = spanLoadingOuterID;
    tmpSpanLoadingID = spanLoadingID;
    tmpIsExploded = isExploded;
    GalleryXML_Data = Gallery_XML_Data;
    tmpBunchJsonResultsData = jsonResultsData;
    RenderBunchResults(CatalogAdaptationURL);
}

function reGroupBlockAreas(jsonResultsData) {

    var jsonResultsToDelete = [];

    // Traverse JSON to find only numeric area nids
    for (var i = 0; i < jsonResultsData[""].length; i++) {

        var tmpAreaNId = jsonResultsData[""][i].AreaNId;

        if (jsonResultsData[""][i].IsAreaNumeric == 'True') {
            // Find corresponding block or qs for numeric area nid
            for (var j = 0; j < jsonResultsData[""].length; j++) {
                if (
                    (jsonResultsData[""][j].BlockAreaParentNId == tmpAreaNId) &&
                    (jsonResultsData[""][j].IndicatorNId == jsonResultsData[""][i].IndicatorNId) &&
                    (jsonResultsData[""][j].UnitNId == jsonResultsData[""][i].UnitNId)
                   ) {
                    if (jsonResultsData[""][i].jsonBlock == null) {
                        jsonResultsData[""][i].jsonBlock = jsonResultsData[""][j];
                    }
                    else {
                        if (jsonResultsData[""][i].jsonBlock.AreaNId.indexOf("QS_") == 0) {
                            jsonResultsData[""][i].jsonBlock = null;
                            jsonResultsData[""][i].jsonBlock = jsonResultsData[""][j];
                        }
                        
                    }
                    // Delete merged Block/QS areas from main json
                    jsonResultsToDelete.push(jsonResultsData[""][j]);
                    break;
                }
            }
        }
        else {
        }
    }

    for (var i = 0; i < jsonResultsToDelete.length; i++) {
        var indexOfRemovableItem = jsonResultsData[""].indexOf(jsonResultsToDelete[i]);
        jsonResultsData[""].splice(indexOfRemovableItem, 1);
    }
    
}

function RenderBunchResults(CatalogAdaptationUrl) {

    var spanLoading = z(tmpSpanLoadingID);
    var spanLoadingOuter = z(tmpSpanLoadingOuterID);

    var BunchSize = 15; //Bunch Size changed to 15, orig value : 5
    var tempjsonSize = tmpBunchJsonResultsData[""].length;
    //Added for Simul Gallery Home Page
    var GalleryXML_Singl = "";
    var g = 0;
    var arrNID_GalleryXMLMain = GalleryXML_Data.split("[^^^^]");
    arrNID_GalleryXMLMain.splice(0, 1);
    //Till here
    if (isCatalogResults && tmpBunchJsonResultsData[""].length > 100) tempjsonSize = 100;

    try {

        var tmpJSON = [];

        for (var i = counterCurrentJsonResult; i < counterCurrentJsonResult + BunchSize; i++) {
            if (i < tempjsonSize) tmpJSON.push(tmpBunchJsonResultsData[""][i]);
        }


        counterCurrentJsonResult += BunchSize;

        for (var i = 0; i < tmpJSON.length; i++) {
            //Added for Simul Gallery Home Page
            
            for (g = 0; g < arrNID_GalleryXMLMain.length; g++) {
                var arrNID_Gallery_XMLSingl = arrNID_GalleryXMLMain[g];
                var NID_Singl = arrNID_Gallery_XMLSingl.split("[::::]")[0];
                if (tmpJSON[i].NId == NID_Singl) {
                    if (arrNID_Gallery_XMLSingl.split("[::::]")[1] != "") {
                        GalleryXML_Singl = arrNID_Gallery_XMLSingl.split("[::::]")[1];
                        break;
                    }
                    else
                        GalleryXML_Singl = "";
                }
                else
                    GalleryXML_Singl = "";
            }
            //Till here
            // QS results handling
            if (tmpJSON[i].AreaNId.indexOf('QS_') == 0)
                RenderQsResult(tmpJSON[i], tmpDivForResults, tmpIsExploded, CatalogAdaptationUrl, GalleryXML_Singl);//Added last parameter for Simul Gallery Home Page
            else
                RenderResult(tmpJSON[i], tmpDivForResults, tmpIsExploded, CatalogAdaptationUrl, GalleryXML_Singl);//Added last parameter for Simul Gallery Home Page
        }

        if (counterCurrentJsonResult < tempjsonSize) {
            //           spanLoading.innerHTML = 'Loading results...';
            spanLoading.innerHTML = z('langLoadingResults').value;
            setTimeout('RenderBunchResults("' + CatalogAdaptationUrl + '")', BunchSize * 100); //Timeout value changed to 100, orig value : 300

            if ((counterCurrentJsonResult + BunchSize) > tempjsonSize && tmpIsExploded == false) /*spanLoading.innerHTML = 'Loading charts...'*/;

        }
        else {
            if (tmpIsExploded == false) {
                for (var i = 0; i < AllIdUniquePart.length; i++) {
                    RenderChart(AllIdUniquePart[i]);
                }
            }

            QdsResultsLoadingFinished(tmpDivForResults);
            di_jq('.sparkline').sparkline('html', { type: 'bar' });
        }
    }
    catch (ex) {
        var pointer = i + 1;
        spanLoadingOuter.style.display = "none";
        spanLoading.innerHTML = "";
    }
}

function QdsResultsLoadingFinished(divQdsResults) {
  var strCartCookie = readCookie('QdsCart');
    var arrCartNow = [];

    if (strCartCookie != null && strCartCookie != '') arrCartNow = strCartCookie.split(',');
    z('spanLoadingOuter').style.display = "none";
    z('spanLoading').innerHTML = "";
    if (!isCatalogResults && IsShowElements ==true) {
        z('spanAddAllToCartMain').style.display = 'block';
        if (arrCartNow.length != 0) {
            z('spanRemoveAllFromCartMain').style.display = 'block';
        }
        z('spanMultipleDbSearchMain').style.display = 'block';
    }
    var strCartCookie = readCookie('QdsCart');
    var arrCartNow = [];

    if (strCartCookie != null && strCartCookie != '') arrCartNow = strCartCookie.split(',');

    for (var i = 0; i < arrCartNow.length; i++) {
        AddtoDataView(arrCartNow[i], z('hdbnid').value, false); 	//isBlock Issue : Replaced multiple if else constructs to single statement. and removed parameter isBlock
    }

//    StopGalleryLoadingProcess = false;
//    if (isSearchResultsGalleryEnabled) GetGalleryThumbnails(0, 1);

    try{
        z('divDatabaseFullInfo').style.paddingTop = '40px';
    }
    catch(err){
    var errMsg = err;
    errMsg += '';
    }
}


//Modify to handle minus value
function CommaFormatted(amount) {
    var minusPrefix = "";
    if (amount < 0) {
        minusPrefix = "-";
    }
    var delimiter = ","; // replace comma if desired
    var a = amount.split('.', 2)
    var d = a[1];
    var i = parseInt(a[0]);
    if (isNaN(i)) { return ''; }
    var minus = '';
    if (i < 0) { minus = '-'; }
    i = Math.abs(i);
    var n = new String(i);
    var a = [];
    while (n.length > 3) {
        var nn = n.substr(n.length - 3);
        a.unshift(nn);
        n = n.substr(0, n.length - 3);
    }
    if (n.length > 0) { a.unshift(n); }
    n = a.join(delimiter);

    if (d == null || d.length < 1) amount = n;
    else amount = n + '.' + d;

    amount = minus + amount;
    if (minusPrefix == "-") {
        if (amount > 0) {
            amount = minusPrefix + amount;
        }
    }
    return amount;
}

function getAreaNidForDiv(ResultAreaNId) {

}

function sortDvSeries(inputDvSeries, isTimeSeries) {

    var tmpDvSeries = inputDvSeries;

    var strSeries = '';
    var strDVs = '';

    var arrSeries = tmpDvSeries.split(':')[0].split('|');
    var arrDVs = tmpDvSeries.split(':')[1].split('|');


    if (isTimeSeries.toLowerCase() == 'true') {
        var sortedSeries = arrSeries.clone();
        sortedSeries.sort();

        for (var i = 0; i < sortedSeries.length; i++) {

            var indexOfSeries = arrSeries.indexOf(sortedSeries[i]);

            strSeries += '|' + arrSeries[indexOfSeries];
            strDVs += ',' + arrDVs[i];

            arrSeries[indexOfSeries] = null;
        }

    }
    else {
        var sortedDVs = arrDVs.clone();
        sortedDVs.sort(sortAsc);
        for (var i = 0; i < sortedDVs.length; i++) {

            var indexOfDV = arrDVs.indexOf(sortedDVs[i]);

            strSeries += '|' + arrSeries[indexOfDV];
            strDVs += ',' + sortedDVs[i];

            arrDVs[indexOfDV] = null;
        }
    }



    strSeries = strSeries.substr(1);
    strDVs = strDVs.substr(1);

    tmpDvSeries = strSeries + ':' + strDVs;

    return tmpDvSeries;

}


function get_DvSeries_Min_Max(inputDvSeries) {

    var RetVal = '';

    var MinValue = '';
    var MaxValue = '';

    var strSeries = '';
    var strDVs = '';

    var arrDVs = inputDvSeries.split(':')[1].split('|');

    var sortedSeries = arrDVs.clone();
    sortedSeries.sort(sortAsc);

    MinValue = sortedSeries[0];
    MaxValue = sortedSeries[sortedSeries.length - 1];

    RetVal = z('langMin').value + ': ' + MinValue + '      ' + z('langMax').value + ': ' + MaxValue;

    return RetVal;

}


function sortAsc(a, b) {
    return a - b;
}

function sortDsc(a, b) {
    return b - a;
}


function RenderChart(IdUniquePart) {


    var InputParam;
    var divDIUA, divGallery, btnGallery, divVisualizer, divJsonData, btnVisualizer, spanIndicator, spanArea;
    var divDVCloudId, DVCloudContent;
    var Indicator, Area;
    var spanLoading, spanLoadingOuter, divQDSResults;
    var chartType, chartData;
    var anchorViewData;

    var jsonResult;

    spanLoading = z('spanLoading');
    spanLoadingOuter = z('spanLoadingOuter');
    divQDSResults = z('divQDSResults');

    divDIUA = z("DIUA_" + IdUniquePart);
    divGallery = z("Gallery_" + IdUniquePart);
    btnGallery = z("btnGallery_" + IdUniquePart);
    divVisualizer = z("Visualizer_" + IdUniquePart);

    divJsonData = z("JsonData_" + IdUniquePart);
    jsonResult = di_jq.parseJSON(divJsonData.innerHTML);

    var tmpDefaultSG = jsonResult.DefaultSG.split('[@@@@]')[1];
    var tmpDvSeries = sortDvSeries(jsonResult.DVSeries, jsonResult.IsAreaNumeric);
    //chartData = divVisualizer.innerHTML;
    chartData = Get_Visualizer_Creation_Data(tmpDefaultSG, tmpDvSeries.split(':')[0], tmpDvSeries.split(':')[1]);

    if (di_jq.parseJSON(chartData).categoryCollection.length == 1) return;

    chartType = 'column'; // divVisualizer.getAttribute("type");
    //divVisualizer.removeAttribute("type");


    divVisualizer.innerHTML = '';

    //  CHANGES
    if (divDIUA.innerHTML.indexOf('ShowCloud') != -1) {

        var arrayChartData = di_jq.parseJSON(chartData);
        var stringCloudData = '';
        for (var a = 0; a < arrayChartData.categoryCollection.length; a++) {
            var strBasicItem = '';
            strBasicItem += 'text: "' + arrayChartData.categoryCollection[a] + '", ';
            strBasicItem += 'weight: ' + arrayChartData.seriesCollection[0].data[a] + ', ';
            strBasicItem += 'title: "' + arrayChartData.seriesCollection[0].data[a] + '"';

            stringCloudData += "{" + strBasicItem + "}";
            if (a != arrayChartData.categoryCollection.length - 1) stringCloudData += ",";
        }

        stringCloudData = "[" + stringCloudData + "]";
        UniqueId_CloudData.push([IdUniquePart, stringCloudData]);
    }

    // CHANGES END

    if (chartData != null && chartData != '') {

        divVisualizer.style.display = "block";
        var strStockPath = '../../stock/templates/';

        if (z('hdsby').value == "share") {
            strStockPath = '../../templates/';
        }

        di_initChart(strStockPath + chartType + '_qds.txt', '', IdUniquePart, divVisualizer.id, chartData,
                                             'j.str', '', '', '', chartType, 'normal', '');
    }


}

function Get_Visualizer_Creation_Data(DefaultSG, CategorySeriesCSV, DVSeriesCSV) {
    var RetVal = '';

    RetVal = "{";

    RetVal += "\"categoryCollection\":[";

    var CategorySeries = CategorySeriesCSV.split('|');
    var DVSeries = DVSeriesCSV.split(',');

    for (var Category = 0; Category < CategorySeries.length; Category++) {
        RetVal += "\"" + CategorySeries[Category] + "\",";
    }
    if (CategorySeries.length > 0) {
        RetVal = RetVal.substring(0, RetVal.length - 1);
    }
    RetVal += "],";

    RetVal += "\"seriesCollection\":[{\"name\":\"" + DefaultSG + "\",";

    RetVal += "\"data\":[";
    for (var DataValue = 0; DataValue < DVSeries.length; DataValue++) {
        RetVal += DVSeries[DataValue] + ",";
    }
    if (DVSeries.length > 0) {
        RetVal = RetVal.substring(0, RetVal.length - 1);
    }
    RetVal += "]";

    RetVal += "}]";

    RetVal += "}";

    return RetVal;
}

function RenderTimeTaken(TotalRecords, TimeTakenInSecs) {
    var spanTimeCounter = z('spanTimeCounter');
    if (TotalRecords == 0) {
        spanTimeCounter.innerHTML = z('NoRecordsFound').value; //"No Records Found";
//        spanTimeCounter.style.visibility = "visible";
    }
    else {
        spanTimeCounter.innerHTML = z('langQDSResults').value + " : " + TotalRecords;
//        spanTimeCounter.innerHTML = z('langQDSResults').value + " : " + TotalRecords + ", " + z('langQDSTimeTaken').value + " : " + TimeTakenInSecs + " " + z('langQDSSeconds').value;
//        spanTimeCounter.style.display = "none";
//        var spanAddToCart = z('spanAddAllToCartMain');
//        spanAddToCart.style.marginLeft = "0px";
    }

}

function GetGalleryThumbnails(DBNum, GalleryNum) {
    var divQDSResults, divDIUA;
    var DBId, IndicatorNId, UnitNId, AreaNId;
    var spanLoading, spanLoadingOuter;
    spanLoading = z('spanLoading');
    spanLoadingOuter = z('spanLoadingOuter');
    divQDSResults = z('divQDSResults');

    if (StopGalleryLoadingProcess == false) {
        if (divQDSResults.childNodes[DBNum] != null) {
            divDIUA = divQDSResults.childNodes[DBNum]; //.childNodes[GalleryNum];

            if (divDIUA != null) {
                DBId = divDIUA.id.split('_')[1];
                IndicatorNId = divDIUA.id.split('_')[2];
                UnitNId = divDIUA.id.split('_')[3];

                AreaNId = divDIUA.id.split('_')[4];

                spanLoadingOuter.style.display = "inline";
                spanLoading.innerHTML = "Searching the gallery...";                

                GetGalleryThumbnailsForDIUA(DBId, IndicatorNId, UnitNId, AreaNId, DBNum, GalleryNum);
            }
            else {
                spanLoadingOuter.style.display = "none";
                spanLoading.innerHTML = "";
            }
        }
        else {
            spanLoadingOuter.style.display = "none";
            spanLoading.innerHTML = "";
        }
    }
}

function ViewData(DBId, IdUniquePart) {
    PostToDataView(DBId, z("DIUA_" + IdUniquePart).childNodes[0].value);
}

function ViewAllData() {
    var divViewData, divViewDataChildDBNId, divViewDataChildDVNIds;
    var DBNIds, AllDVNIds, DBNId, DVNIds;

    divViewData = z('divViewData');
    DBNIds = ",";
    AllDVNIds = "";
    DBNId = "";
    DVNIds = "";

    for (var j = 1; j < divViewData.childNodes.length; j++) {
        divViewDataChildDBNId = divViewData.childNodes[j].childNodes[0];
        DBNId = divViewDataChildDBNId.getAttribute("DBNId");

        if (DBNIds.search("," + DBNId + ",") == -1) {
            DBNIds = DBNIds + DBNId + ",";
            DVNIds = "";

            for (var i = 1; i < divViewData.childNodes.length; i++) {
                divViewDataChildDVNIds = divViewData.childNodes[i].childNodes[0];
                if (DBNId == divViewDataChildDVNIds.getAttribute("DBNId")) {
                    DVNIds = DVNIds + divViewDataChildDVNIds.getAttribute("DVNIds") + ",";
                }
            }

            if (DVNIds.length > 0) {
                DVNIds = DVNIds.substr(0, DVNIds.length - 1);
                AllDVNIds = AllDVNIds + DVNIds + ";";
            }
        }
    }

    if (DBNIds.length > 0) {
        DBNIds = DBNIds.substr(1, DBNIds.length - 2);
    }

    if (AllDVNIds.length > 0) {
        AllDVNIds = AllDVNIds.substr(0, AllDVNIds.length - 1);
        PostAllToDataView(DBNIds, AllDVNIds);
    }
    else {
        alert('Please select atleast one search result for viewing data!');
    }
}

function GetViewDataChildDiv(IdUniquePart, Area, Indicator, Unit, DBNId, DVNIds, concat)  {
     //isBlock Issue : Removed parameter isBlock
    var RetVal, Content;

    RetVal = '';
    Content = '';

    if (Area != '') {
        if (concat) Content += Area + " areas - ";    //isBlock Issue : Changed the logic, instead of isBlock and concat only concat will be used
        else Content += Area + " - ";

    }
    if (Indicator != '') {
        Content += Indicator + ", ";
    }
    if (Unit != '') {
        Content += Unit;
    }

    RetVal = "<div id=\"divViewData_" + IdUniquePart + "\" DBNId = \"" + DBNId + "\" DVNIds=\"" + DVNIds + "\"><table width=\"100%\"><tr>";

    RetVal += "<td width=\"90%\" style=\"font-size:11px;\" title=\"" + Content + "\">";

    if (Content.length > 55) {
        RetVal += Content.substr(0, 55) + ".. ";
    }
    else {
        RetVal += Content + " ";
    }

    RetVal += "</td>";

    // isBlock Issue : Do not pass isBlock in call of RemoveFromDataview

    RetVal += "<td width=\"10%\" title=\"" + z('langREMOVE_FROM_DATA_CART').value + "\" valign=\"top\" onmouseover=\"ShowHideHoverImage(this, 'true');\" onmouseout=\"ShowHideHoverImage(this, 'false');\" onclick=\"RemoveFromDataView('" + IdUniquePart + "',false);\"  style=\"cursor:pointer;\">";
    RetVal += "<img src=\"../../stock/themes/default/images/remove.png\" />";
    RetVal += "</td>";

    RetVal += "</tr></table></div>";

    return RetVal;

}

function AddAllToCart() {

    di_jq('span').each(function (index) {
        var tmpSpanID = this.id;
        if (tmpSpanID.indexOf('ATDV_') == 0 && z(tmpSpanID).style.display != 'none') {
            var UniqueID = z(tmpSpanID).id.replace('ATDV_', '');
            var jsonData = z('JsonData_' + UniqueID).innerHTML;
            jsonData = di_jq.parseJSON(jsonData);

            z('spanRemoveAllFromCartMain').style.display = 'block';
            //if (this.parentNode.innerHTML.indexOf('<table') != 0)
            if (this.isExploded == "false") {
                AddtoDataView(UniqueID, z('hdbnid').value, true);    // isBlock Issue : Removed parameter isBlock
            }
        }
    });

}

function RemoveAllFromCart() {
    di_jq('div').each(function (index) {
        var tmpSpanID = this.id;
        if (tmpSpanID.indexOf('divViewData_') == 0) {
            var UniqueID = z(tmpSpanID).id.replace('divViewData_', '');
            RemoveFromDataView(UniqueID, true);             // isBlock Issue : Removed parameter isBlock
        }
    });
    z('spanRemoveAllFromCartMain').style.display = 'none';

}
function AddtoDataView(IdUniquePart, DBNId, isAllAdded){
    // isBlock Issue : Removed parameter isBlock
    if (z('divViewData_' + IdUniquePart) != null) return;
    z('spanRemoveAllFromCartMain').style.display = 'block';

    var InputParam;
    var tblViewData, tblViewDataInner, divViewData, divViewDataChild, anchorViewData, Language;

    var divJsonData = z('JsonData_' + IdUniquePart);

    var hselindo = z('hselindo');
    var hselareao = z('hselareao');

    var hselind = z('hselind');
    var hselarea = z('hselarea');

    var Language = z('hlngcode').value;

    tblViewData = z('tblViewData');
    tblViewDataInner = z('tblViewDataInner');
    divViewData = z('divViewData');
    anchorViewData = z('ATDV_' + IdUniquePart);

    var jsonInd = di_jq.parseJSON(hselindo.value);
    var jsonArea = di_jq.parseJSON(hselareao.value);
    var jsonData = null;
    //Added to fix epanded add to cart bug
    if (divJsonData == null) {
        var jsonCartData = di_jq.parseJSON(z('hdvnids').value.split("[####]")[1]); //.replace(/\ \"/g, "\ \\\"").replace(/\"\ /g, "\\\"\ "));
        for (var objCardSingleData in jsonCartData.cart) {
            if(objCardSingleData==IdUniquePart)
            jsonData = jsonCartData.cart[objCardSingleData][0];
        }
    }
    else {
        jsonData = di_jq.parseJSON(divJsonData.innerHTML);
    }
    var jsonStructure = JSONStructure(IdUniquePart, jsonData);   // isBlock Issue : Added this function call     
    var oldDVNIDs = z('hdvnids').value;
    var jsonCart = null;
    var jsonCartStr = "";

    var jsonDataWithoutIndDesc = jsonData;

    if (oldDVNIDs.indexOf("[####]") != -1) {
        var oldDVNIDsTemp = oldDVNIDs.split("[####]")[0];
        jsonCart = di_jq.parseJSON(oldDVNIDs.split("[####]")[1]); //.replace(/\ \"/g, "\ \\\"").replace(/\"\ /g, "\\\"\ "));
        if (jsonCart.cart[IdUniquePart] == null) {
            jsonCart.cart[IdUniquePart] = [];

            jsonDataWithoutIndDesc.IndicatorDescription = "";

            jsonCart.cart[IdUniquePart].push(jsonDataWithoutIndDesc);

        }
        jsonCartStr += JSON.stringify(jsonCart);
        z('hdvnids').value = oldDVNIDsTemp + "[####]" + jsonCartStr;
    }
    else {
        jsonCart = di_jq.parseJSON('{"cart":{}}');
        if (jsonCart.cart[IdUniquePart] == null) {
            jsonCart.cart[IdUniquePart] = [];

            jsonDataWithoutIndDesc.IndicatorDescription = "";

            jsonCart.cart[IdUniquePart].push(jsonDataWithoutIndDesc);

        }
        jsonCartStr += JSON.stringify(jsonCart);
        z('hdvnids').value = oldDVNIDs + "[####]" + jsonCartStr;
    }

    //

    var Area, Indicator, Unit, DVNIds;

    Area = jsonData.Area;
    Indicator = jsonData.Indicator;
    Unit = jsonData.Unit;
    DVNIds = jsonStructure.NId + "-" + jsonStructure.DefaultSG.split('[@@@@]')[0];   // isBlock Issue : Replaced if-else loops with single statement. 

    if (jsonInd == null) {
        jsonInd = di_jq.parseJSON('{"iu":[],"sg_dim":{},"sg_dim_val":{},"iusnid":{}}');
    }

    if (jsonArea == null) {
        jsonArea = di_jq.parseJSON('{"area":{}}');
    }

    var AreaLevel = "-1"; //getAreaLevel(jsonData.Area, jsonData.AreaNId, DBNId, Language);
    var IUSNId = getIUSNId(jsonData.IndicatorNId, jsonData.UnitNId, DBNId, Language);

    var tmpIU = jsonData.IndicatorNId + "~" + jsonData.UnitNId;
    var tmpIndName_UnitName = jsonData.Indicator + "~" + jsonData.Unit;

    if (jsonArea.area[AreaLevel] == null) {
        jsonArea.area[AreaLevel] = [];
    }

    var jsonCurrentlyAddedToCart;
    jsonCurrentlyAddedToCart = jsonStructure;         // isBlock Issue : Replaced if-else loops with single statement. 

    var jsonSubAreas = getAreaNIds_NameInArray(jsonCurrentlyAddedToCart);
    var jsonVirtualAreasForCart = jsonArea.area[AreaLevel];

    for (var i = 0; i < jsonSubAreas.length; i++) {
        var tmpNowAreaNId_NameCombination = jsonSubAreas[i];
        if (jsonVirtualAreasForCart.indexOf(tmpNowAreaNId_NameCombination) == -1) {
            jsonVirtualAreasForCart.push(tmpNowAreaNId_NameCombination);
        }
    }


    jsonInd.iu.push(tmpIU + "||" + tmpIndName_UnitName);
    jsonInd.iusnid["iu_" + jsonData.IndicatorNId + "_" + jsonData.UnitNId] = IUSNId[1];

    hselindo.value = JSON.stringify(jsonInd);
    hselareao.value = JSON.stringify(jsonArea);

    hselind.value += "," + IUSNId[0];
    hselarea.value += "," + jsonStructure.AreaNId;    // isBlock Issue : Replaced if-else loops with single statement. 

    if (hselind.value.indexOf(",") == 0) hselind.value = hselind.value.substr(1);
    if (hselarea.value.indexOf(",") == 0) hselarea.value = hselarea.value.substr(1);

    if (divViewDataChild == null) {

        tblViewData.style.display = "";
        tblViewDataInner.style.display = "";
        divViewDataChild = document.createElement('div');
        //Code to check for appending the word -Areas-
        var concat = false;
        concat = AppendAreasOrNot(IdUniquePart, jsonData);     // isBlock Issue : Replaced if-else loops with single statement.
        //End of Code to check for appending the word -Areas-
        divViewDataChild.innerHTML = GetViewDataChildDiv(IdUniquePart, Area, Indicator, Unit, DBNId, DVNIds, concat);   // isBlock Issue : Do not pass isBlock as a parameter
        divViewData.appendChild(divViewDataChild);

        if (isAllAdded) divViewData.scrollTop = divViewData.scrollHeight;
        else {
            di_jq("#divViewData_" + IdUniquePart).slideDown
					(
					    500,
					    function () {
					        divViewData.scrollTop = divViewData.scrollHeight;
					    }
					);
        }

        //#region Cart storing into cookies

		var strCartCookie = readCookie('QdsCart');
		var arrCartNow = [];

		if (strCartCookie != null && strCartCookie != '') arrCartNow = strCartCookie.split(',');

		if(arrCartNow.indexOf(IdUniquePart) == -1) arrCartNow.push(IdUniquePart);
		createCookie('QdsCart', arrCartNow);

		//#endregion
		if (divJsonData != null)
        anchorViewData.style.display = "none";
    }
}

function RemoveFromDataView(IdUniquePart, isAllRemoved) {
    //isBlock Issue : Removed parameter
    var tblViewData, tblViewDataInner, divViewDataChild, anchorViewData, DBNId, Language;
   
    var Language = z('hlngcode').value;

    try {
        var divJsonData = z('JsonData_' + IdUniquePart);

        DBNId = z('hdbnid').value;

        tblViewData = z('tblViewData');
        tblViewDataInner = z('tblViewDataInner');
        divViewDataChild = z('divViewData_' + IdUniquePart);
        anchorViewData = z('ATDV_' + IdUniquePart);

        var hselindo = z('hselindo');
        var hselareao = z('hselareao');

        var hselind = z('hselind');
        var hselarea = z('hselarea');
        //Added to fix the bug
        var hdvnids = z('hdvnids').value;
        var oldDVNIds = hdvnids.split("[####]")[0];
        var jsonCartData = di_jq.parseJSON(hdvnids.split("[####]")[1]);
        var jsonCartFinal = null;
        jsonCartFinal = di_jq.parseJSON('{"cart":{}}');
        for (var objCardSingleData in jsonCartData.cart) {
            if (objCardSingleData == IdUniquePart)
                continue;
            if (jsonCartFinal.cart[objCardSingleData] == null) {
                jsonCartFinal.cart[objCardSingleData] = [];
                jsonCartFinal.cart[objCardSingleData].push(jsonCartData.cart[objCardSingleData][0]);
            }
            }
        var finalDVNIds = oldDVNIds + "[####]" + JSON.stringify(jsonCartFinal);
        z('hdvnids').value = finalDVNIds;
        //
        var jsonInd = di_jq.parseJSON(hselindo.value);
        var jsonArea = di_jq.parseJSON(hselareao.value);

        var jsonData = di_jq.parseJSON(divJsonData.innerHTML);
        var jsonStructure = JSONStructure(IdUniquePart, jsonData);  //isBlock Issue : added to make a function call
        if (jsonInd == null) {
            jsonInd = di_jq.parseJSON('{"iu":[],"sg_dim":{},"sg_dim_val":{},"iusnid":{}}');
        }

        var tmpIU = jsonData.IndicatorNId + "~" + jsonData.UnitNId;
        var tmpIndName_UnitName = jsonData.Indicator + "~" + jsonData.Unit;

        var AreaLevel = "-1";  //getAreaLevel(jsonData.Area, jsonData.AreaNId, DBNId, Language);
        var IUSNId = getIUSNId(jsonData.IndicatorNId, jsonData.UnitNId, DBNId, Language);

        var jsonCurrentlyRemovedFromCart;
        jsonCurrentlyRemovedFromCart = jsonStructure;           //isBlock Issue : Replaced multiple if else constructs to single statement

        var jsonSubAreas = getAreaNIds_NameInArray(jsonCurrentlyRemovedFromCart);

        //jsonArea.area[AreaLevel].push(jsonData.AreaNId + "||" + jsonData.Area);

        var jsonVirtualAreasForCart = jsonArea.area[AreaLevel];

        for (var i = 0; i < jsonSubAreas.length; i++) {
            var tmpNowAreaNId_NameCombination = jsonSubAreas[i];
            var indexOfAreaNId_Name = jsonVirtualAreasForCart.indexOf(tmpNowAreaNId_NameCombination);

            if (indexOfAreaNId_Name != -1) {
                jsonVirtualAreasForCart.splice(indexOfAreaNId_Name, 1);
            }
        }

        var indexOfIU = jsonInd.iu.indexOf(tmpIU + "||" + tmpIndName_UnitName);
        //var indexOfArea = jsonArea.area[AreaLevel].indexOf(jsonData.AreaNId + "||" + jsonData.Area);

        if (indexOfIU != -1) {
            jsonInd.iu.splice(indexOfIU, 1);
        }

//        if (indexOfArea != -1) {
//            jsonArea.area[AreaLevel].splice(indexOfIU, 1);
//        }

        jsonInd.iusnid["iu_" + jsonData.IndicatorNId + "_" + jsonData.UnitNId] = null;

        var arrInd = hselind.value.split(",");
        var arrAreas = hselarea.value.split(",");

        arrInd.splice(arrInd.indexOf(IUSNId[0]), 1);
        arrAreas.splice(arrAreas.indexOf(jsonData.AreaNId), 1);

        hselind.value = "";
        hselarea.value = "";

        for (var i = 0; i < arrInd.length; i++) hselind.value += "," + arrInd[i];
        for (var i = 0; i < arrAreas.length; i++) hselarea.value += "," + arrAreas[i];

        if (hselind.value.indexOf(",") == 0) hselind.value = hselind.value.substr(1);
        if (hselarea.value.indexOf(",") == 0) hselarea.value = hselarea.value.substr(1);

        hselindo.value = JSON.stringify(jsonInd);
        hselareao.value = JSON.stringify(jsonArea);
    }

    catch (err) {
        var ErrMessageNow = err.Message;
    }

    try {

        if (isAllRemoved != null || isAllRemoved) {
            if (divViewDataChild.parentNode.parentNode.childNodes.length == 2) {
                tblViewData.style.display = "none";
                tblViewDataInner.style.display = "none";
            }
            divViewDataChild.parentNode.parentNode.removeChild(divViewDataChild.parentNode);
        }
        else {
            di_jq("#divViewData_" + IdUniquePart).slideUp
            (
                500,
                function () {
                    if (divViewDataChild.parentNode.parentNode.childNodes.length == 2) {
                        tblViewData.style.display = "none";
                        tblViewDataInner.style.display = "none";
                    }

                    divViewDataChild.parentNode.parentNode.removeChild(divViewDataChild.parentNode);
                }
            );
        }

            //#region Removing from cart's cookie

            var strCartCookie = readCookie('QdsCart');
            var arrCartNow = [];

            if (strCartCookie != null && strCartCookie != '') arrCartNow = strCartCookie.split(',');

            if (arrCartNow.indexOf(IdUniquePart) != -1) arrCartNow.splice(arrCartNow.indexOf(IdUniquePart), 1);
            createCookie('QdsCart', arrCartNow);
            if (arrCartNow.length == 0) {
                z('spanRemoveAllFromCartMain').style.display = 'none';
            }
            //#endregion

    }
    catch (err) {
        var errMsg = err.Message;
    }

    if (anchorViewData != null) {
        anchorViewData.style.display = "";
    }
}


// Below function works on block areas to get association between the area nids & area names of its children
function getAreaNIds_NameInArray(inputJson) {

    var RetVal = [];
    var AreaNId_Name;
    if (inputJson.AreaNIds == "") {
        AreaNId_Name = inputJson.AreaNId + "||" + inputJson.Area;
        RetVal.push(AreaNId_Name);
    }
    else {

        var dvSeries = inputJson.DVSeries;
        var PipeSeparatedAreaNames = dvSeries.split(":")[0];
        var CommaSeparatedAreaNIds = inputJson.AreaNIds;

        var arrAreaNIds = CommaSeparatedAreaNIds.split(",");
        var arrAreaNames = PipeSeparatedAreaNames.split("|");

        for (var i = 0; i < arrAreaNIds.length; i++) {
            AreaNId_Name = arrAreaNIds[i] + "||" + arrAreaNames[i];
            RetVal.push(AreaNId_Name);
        }
    }
    return RetVal;
}


function IsAlreadyInDataCart(IdUniquePart) {
    var RetVal;
    var divViewDataChild;

    divViewDataChild = z('divViewData_' + IdUniquePart);
    if (divViewDataChild != null) {
        RetVal = true;
    }
    else {
        RetVal = false;
    }

    return RetVal;
}

function ShowHideHoverImage(element, ShowHoverImage) {
    var tdTarget;

    tdTarget = element || event.srcElement || event.target;

    if (tdTarget != null && tdTarget.childNodes.length > 0) {
        if (ShowHoverImage == "true") {
            tdTarget.childNodes[0].src = "../../stock/themes/default/images/remove_h.png";
        }
        else {
            tdTarget.childNodes[0].src = "../../stock/themes/default/images/remove.png";
        }
    }
}

function GetAreas(DBId, AreaNIds, evt, source) {
    var InputParam, NewCallBackPageName;

    ShowCallout('divCallout', '', evt);

    if (source.getAttribute("datavalue") != null && source.getAttribute("datavalue") != "") {
        z('divCalloutText').innerHTML = source.getAttribute("datavalue");
    }
    else {
        InputParam = AreaNIds;
        InputParam += ParamDelimiter + z('hlngcodedb').value;
        InputParam += ParamDelimiter + DBId;

        NewCallBackPageName = GetCallbackPageLocation();

        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: NewCallBackPageName,
                data: { 'callback': '7', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        z('divCalloutText').innerHTML = data;
                        source.setAttribute("datavalue", data);
                    }
                    catch (ex) {
//                        alert("message:" + ex.message);
                    }
                },
                error: function () {
//                    
                },
                cache: false
            });
        }
        catch (ex) {
        }
    }
}

function GetSGs(DBId, SGNIds, evt, source) {
    var InputParam, NewCallBackPageName;

    ShowCallout('divCallout', '', evt);

    if (source.getAttribute("datavalue") != null && source.getAttribute("datavalue") != "") {
        z('divCalloutText').innerHTML = source.getAttribute("datavalue");
    }
    else {
        InputParam = SGNIds;
        InputParam += ParamDelimiter + z('hlngcodedb').value;
        InputParam += ParamDelimiter + DBId;

        NewCallBackPageName = GetCallbackPageLocation();

        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: NewCallBackPageName,
                data: { 'callback': '8', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        z('divCalloutText').innerHTML = data;
                        source.setAttribute("datavalue", data);
                    }
                    catch (ex) {
//                        alert("message:" + ex.message);
                    }
                },
                error: function () {
//                    
                },
                cache: false
            });
        }
        catch (ex) {
        }
    }
}

function GetSources(DBId, SourceNIds, evt, source) {
    var InputParam, NewCallBackPageName;

    ShowCallout('divCallout', '', evt);

    if (source.getAttribute("datavalue") != null && source.getAttribute("datavalue") != "") {
        z('divCalloutText').innerHTML = source.getAttribute("datavalue");
    }
    else {
        InputParam = SourceNIds;
        InputParam += ParamDelimiter + z('hlngcodedb').value;
        InputParam += ParamDelimiter + DBId;

        NewCallBackPageName = GetCallbackPageLocation();

        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: NewCallBackPageName,
                data: { 'callback': '9', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        z('divCalloutText').innerHTML = data;
                        source.setAttribute("datavalue", data);
                    }
                    catch (ex) {
//                        alert("message:" + ex.message);
                    }
                },
                error: function () {
//                    
                },
                cache: false
            });
        }
        catch (ex) {
        }
    }
}

function GetTPs(DBId, TPNIds, evt, source) {
    var InputParam, NewCallBackPageName;

    ShowCallout('divCallout', '', evt);

    if (source.getAttribute("datavalue") != null && source.getAttribute("datavalue") != "") {
        z('divCalloutText').innerHTML = source.getAttribute("datavalue");
    }
    else {
        InputParam = TPNIds;
        InputParam += ParamDelimiter + DBId;

        NewCallBackPageName = GetCallbackPageLocation();

        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: NewCallBackPageName,
                data: { 'callback': '10', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        z('divCalloutText').innerHTML = data;
                        source.setAttribute("datavalue", data);
                    }
                    catch (ex) {
//                        alert("message:" + ex.message);
                    }
                },
                error: function () {
//                    
                },
                cache: false
            });
        }
        catch (ex) {
        }
    }
}

function ShowCloud(IdUniquePart) {

    var InputParam, NewCallBackPageName;
    var UniqueParts, DVCloudContent;
    var data = '';

    var divVisualizer = z("VisualizerCloud_" + IdUniquePart);
    var jsonData = di_jq.parseJSON(z('JsonData_' + IdUniquePart).innerHTML);
    var chartData = divVisualizer.innerHTML;


    var arrayChartData = di_jq.parseJSON(chartData);
    var stringCloudData = '';
    for (var a = 0; a < arrayChartData.categoryCollection.length; a++) {
        var strBasicItem = '';
        strBasicItem += 'text: "' + arrayChartData.categoryCollection[a] + '", ';
        strBasicItem += 'weight: ' + arrayChartData.seriesCollection[0].data[a] + ', ';
        strBasicItem += 'title: "' + arrayChartData.seriesCollection[0].data[a] + '"';

        stringCloudData += "{" + strBasicItem + "}";
        if (a != arrayChartData.categoryCollection.length - 1) stringCloudData += ",";
    }

    data = "[" + stringCloudData + "]";

    z('divCloud').innerHTML = "";
    z('divCloudHeader').innerHTML = jsonData.Area + ' - ' + jsonData.Indicator + ', ' + jsonData.Unit;

    UniqueParts = IdUniquePart.split('_');

    if (data != "") {

        InputParam = UniqueParts[1];
        InputParam += ParamDelimiter + UniqueParts[2];
        InputParam += ParamDelimiter + UniqueParts[3];
        InputParam += ParamDelimiter + z('hlngcodedb').value;
        InputParam += ParamDelimiter + UniqueParts[0];

        NewCallBackPageName = GetCallbackPageLocation();

        ApplyMaskingDiv();
        ShowLoadingDiv();

        DVCloudContent = eval("(" + data + ")");
        di_jq("#divCloud").jQCloud(DVCloudContent);

        if (z('hdsby').value == 'share') {
            //Set close button at right corner of popup div
            SetCloseButtonInPopupDiv(di_jq('#divCloudOuter'), 'HideCloud', '../../themes/default/images/close.png');
        }
        else {
            //Set close button at right corner of popup div
            SetCloseButtonInPopupDiv(di_jq('#divCloudOuter'), 'HideCloud', '../../stock/themes/default/images/close.png');
        }

        di_jq('#divCloudOuter').show('slow');
        GetWindowCentered(z('divCloudOuter'), 700, 370);
        HideLoadingDiv();
    }

}

function HideCloud() {
    RemoveMaskingDiv();
    di_jq('#divCloudOuter').hide('slow');
}

function ShareResult(IdUniquePart,AdaptationUrl) {
    // isBlock Issue : Removed parameter isBlock
    var InputParam;
    var hselindo = z('hselindo');
    var hselareao = z('hselareao');

    var hselind = z('hselind');
    var hselarea = z('hselarea');

    var Area, Indicator, Unit, DVNIds;
    var jsonData = di_jq.parseJSON(z('JsonData_' + IdUniquePart).innerHTML);
    var jsonStructure = JSONStructure(IdUniquePart, jsonData);      // isBlock Issue : Added to make function call
    var jsonInd = di_jq.parseJSON(hselindo.value);
    var jsonArea = di_jq.parseJSON(hselareao.value);
    Area = jsonData.Area;
    Indicator = jsonData.Indicator;
    Unit = jsonData.Unit;
    DVNIds = jsonStructure.NId + "-" + jsonStructure.DefaultSG.split('[@@@@]')[0]; //isBlock Issue : Replaced multiple lines with single statement

    if (jsonInd == null) {
        jsonInd = di_jq.parseJSON('{"iu":[],"sg_dim":{},"sg_dim_val":{},"iusnid":{}}');
    }

    if (jsonArea == null) {
        jsonArea = di_jq.parseJSON('{"area":{}}');
    }

    var AreaLevel = "-1"; //getAreaLevel(jsonData.Area, jsonData.AreaNId, z('hdbnid').value, z('hlngcode').value);
    var IUSNId = getIUSNId(jsonData.IndicatorNId, jsonData.UnitNId, z('hdbnid').value, z('hlngcode').value);

    var tmpIU = jsonData.IndicatorNId + "~" + jsonData.UnitNId;
    var tmpIndName_UnitName = jsonData.Indicator + "~" + jsonData.Unit;

//    if (jsonArea.area[AreaLevel] == null) {
//        jsonArea.area[AreaLevel] = [];
    //    }
    //    jsonArea.area[AreaLevel].push(jsonData.AreaNId + "||" + jsonData.Area);

    jsonArea.area[AreaLevel] = [];
    var jsonCurrentlyAddedToCart;
    jsonCurrentlyAddedToCart = jsonStructure;       //isBlock Issue : Replaced multiple lines with single statement

    var jsonSubAreas = getAreaNIds_NameInArray(jsonCurrentlyAddedToCart);
    var jsonVirtualAreasForCart = jsonArea.area[AreaLevel];

    for (var i = 0; i < jsonSubAreas.length; i++) {
        var tmpNowAreaNId_NameCombination = jsonSubAreas[i];
        if (jsonVirtualAreasForCart.indexOf(tmpNowAreaNId_NameCombination) == -1) {
            jsonVirtualAreasForCart.push(tmpNowAreaNId_NameCombination);
        }
    }

    jsonInd.iu.push(tmpIU + "||" + tmpIndName_UnitName);
    jsonInd.iusnid["iu_" + jsonData.IndicatorNId + "_" + jsonData.UnitNId] = IUSNId[1];

    hselindo.value = JSON.stringify(jsonInd);
    hselareao.value = JSON.stringify(jsonArea);

    hselind.value += "," + IUSNId[0];
    hselarea.value += "," + jsonStructure.AreaNId;      //isBlock Issue : Replaced multiple lines with single statement

    if (hselind.value.indexOf(",") == 0) hselind.value = hselind.value.substr(1);
    if (hselarea.value.indexOf(",") == 0) hselarea.value = hselarea.value.substr(1);
    //InputParam = outerHTML(z("DIUA_" + IdUniquePart));
    InputParam = z('JsonData_' + IdUniquePart).innerHTML;
    InputParam += ParamDelimiter + "share";
    InputParam += ParamDelimiter + z('hdbnid').value;
    InputParam += ParamDelimiter + z('hselarea').value;
    InputParam += ParamDelimiter + z('hselind').value;
    InputParam += ParamDelimiter + z('hlngcode').value;
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    InputParam += ParamDelimiter + z('hselindo').value;
    InputParam += ParamDelimiter + z('hselareao').value;
    InputParam += ParamDelimiter + Area;
    InputParam += ParamDelimiter + Indicator;
    if(AdaptationUrl != "")
    InputParam += ParamDelimiter + AdaptationUrl;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '17', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    var RelativePath = "";
                    if (AdaptationUrl == "") {
                        RelativePath = getAbsURL('stock/shared/qds');
                    }
                    else {
                        RelativePath = AdaptationUrl + "/stock/shared/qds/";
                    }
                    //                    SocialSharing("You should check this out!", "DevInfo", RelativePath + data + ".htm");
                    SocialSharing(z('langShareYSCTO').value, z('langShareDevinfo').value, RelativePath + data + ".htm");

                }
                catch (ex) {
                    //                    alert("message:" + ex.message);
                }
            },
            error: function () {
                //                
            },
            cache: false
        });
    }
    catch (ex) {
    }
}


function GetGalleryThumbnailsForDIUA(DBId, IndicatorNId, UnitNId, AreaNId, DBNum, GalleryNum) {
    var InputParam;
    var divDIUA, divGallery, btnGallery, divVisualizer, spanIndicator, spanArea, divJsonData;
    var divDVCloudId, DVCloudContent;
    var Indicator, Area;
    var spanLoading, spanLoadingOuter, divQDSResults;
    var chartType, chartData;
    var anchorViewData;
    var IdUniquePart;
    var FsPath;

    var jsonData;

    if (StopGalleryLoadingProcess == false) {
        spanLoading = z('spanLoading');
        spanLoadingOuter = z('spanLoadingOuter');
        divQDSResults = z('divQDSResults');

        IdUniquePart = DBId + "_" + IndicatorNId + "_" + UnitNId + "_" + AreaNId;

        divDIUA = z("DIUA_" + IdUniquePart);
        divGallery = z("Gallery_" + IdUniquePart);
        btnGallery = z("btnGallery_" + IdUniquePart);
        divVisualizer = z("Visualizer_" + IdUniquePart);
        divJsonData = z("JsonData_" + IdUniquePart);
        divGallery.innerHTML = "";

        if (divJsonData != null) jsonData = di_jq.parseJSON(divJsonData.innerHTML);

        //spanIndicator = z("Indicator_" + IdUniquePart);
        Indicator = jsonData.Indicator;
        Area = jsonData.Area;

//        if (AreaNId.indexOf('QS') == 0 || AreaNId == '0') { //
//            Area = '';
//        }
//        else {
//            spanArea = z("Area_" + IdUniquePart);
//            Area = spanArea.innerHTML;
//        }

        anchorViewData = z("ATDV_" + IdUniquePart);

        if (IsAlreadyInDataCart(IdUniquePart) == true) {
            anchorViewData.style.display = "none";
        }

        InputParam = Indicator;
        //InputParam += ParamDelimiter;
        InputParam += ParamDelimiter + Area;        
        InputParam += ParamDelimiter + z('hlngcodedb').value;
        InputParam += ParamDelimiter + DBId; 
        var userLoginId = "";
        if (z('hLoggedInUserNId').value != "") {
            userLoginId = z('hLoggedInUserNId').value.split("|")[0];
        }
        if (userLoginId == "")
            userLoginId = "-1";

        InputParam += ParamDelimiter + userLoginId;
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '6', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (StopGalleryLoadingProcess == false) {

                            if (data != null && data != '') {
                                //var arrData = data.split("[****]");
                                //FsPath = arrData[0];                                
                                //commented to implement pop up in home page gallery
//                                    var jsonData = di_jq.parseJSON(data);
//                                    var galleryItemHTML = RenderGalleryThumbnail(jsonData);

//                                    if (galleryItemHTML != '') {
//                                        divGallery.innerHTML = galleryItemHTML;
                                        callbackGalleryInitQDS(data, divGallery.id); //Added to implement Gallery pop up in home page--2
                                        btnGallery.style.display = "block";
                                        divGallery.style.display = "none";


//                                    }
                                   

                                //}
                            }

                            GalleryNum = GalleryNum + 1;
                            if (GalleryNum < divQDSResults.childNodes[DBNum].length) { // CHANGE : .childNodes.
                                GetGalleryThumbnails(DBNum, GalleryNum);
                            }
                            else {
                                di_qds_resize_ind_blocks();
                                di_qds_resize_area_blocks();

                                DBNum = DBNum + 1;
                                if (DBNum < divQDSResults.childNodes.length) {
                                    GetGalleryThumbnails(DBNum, 1);
                                }
                                else {
                                    spanLoadingOuter.style.display = "none";
                                    spanLoading.innerHTML = "";
                                }
                            }
                        }
                    }
                    catch (ex) {
//                        alert("message:" + ex.message);
                    }
                },
                error: function () {
//                    
                },
                cache: false
            });
        }
        catch (ex) {
        }
    }
}

function RenderGalleryThumbnail(jsonDataForSingleResultRow) {

    var RetVal = '';
    var count = 0;
    if (jsonDataForSingleResultRow != null && jsonDataForSingleResultRow != "") {

        RetVal += "<table cellpadding='5'>";

        for (var i = 0; i < jsonDataForSingleResultRow.PresentationTable.length; i++) { //
            var jsonRow = jsonDataForSingleResultRow.PresentationTable[i];
            var PresentationNId = jsonRow.pres_nid;
            var PresentationName = jsonRow.pres_name;
            var PresentationDesc = jsonRow.description;
            var PresentationPath = jsonRow.path;
            var PrsentationText
            if (count % 5 == 0) {
                RetVal += "<tr>";
            }
            count++;
                var PrsentationText = "<div>Title: " + PresentationName + "<br/><br/>Description: " + PresentationDesc + "</div>";

                RetVal += "<td onmouseover=\"ShowCallout('divCallout', '" + PrsentationText + "', event);\" onmouseout=\"HideCallout('divCallout');\">";                
                RetVal += "<img src='" + PresentationPath + PresentationNId + "/" + PresentationNId + "_t.png' style='border:1px solid #ECECEC;' width='150px' height='150px'/>";
                RetVal += "</td>";
                RetVal += "<td>&nbsp;&nbsp;</td>";
                if (count % 5 == 0) {
                    RetVal += "</tr>";
                    count = 0;
                }
        }
            if (jsonDataForSingleResultRow.PresentationTable.length < 5) {
                RetVal += "</tr>";
        }
        RetVal += "</table>";
    }  

    return RetVal;
}


function GetASResultForDBNumber(IndicatorIC, Areas, LngCode, ASDatabaseIds, ASDBNumber, NumResults, ElapsedTime) {
    var InputParam;
    var DBId, DBName;
    var spanLoading, spanLoadingOuter, divQDSResults, spanTimeCounter;

    spanLoading = z('spanLoading');
    spanLoadingOuter = z('spanLoadingOuter');
    divQDSResults = z('divQDSResults');
    spanTimeCounter = z('spanTimeCounter');

    DBId = ASDatabaseIds.split(",")[ASDBNumber];
    DBName = di_db_getAllSelDbNames().split(",")[ASDBNumber];

    if ((IndicatorIC != null && IndicatorIC != "") || (Areas != null && Areas != "")) {
        spanLoadingOuter.style.display = "inline";
        spanLoading.innerHTML = "Searching " + DBName + " database...";

        InputParam = IndicatorIC;
        InputParam += ParamDelimiter + Areas;
        InputParam += ParamDelimiter + LngCode;
        InputParam += ParamDelimiter + DBId;
        InputParam += ParamDelimiter + NumResults;
        InputParam += ParamDelimiter + ElapsedTime;

        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '12', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        divQDSResults.innerHTML = divQDSResults.innerHTML + data.split(ParamDelimiter)[0];
                        spanTimeCounter.innerHTML = data.split(ParamDelimiter)[1];
                        StopGalleryLoadingProcess = false;

                        di_qds_resize_ind_blocks();
                        di_qds_resize_area_blocks();

                        NumResults = data.split(ParamDelimiter)[1].split(' ')[0];
                        ElapsedTime = data.split(ParamDelimiter)[1].split(' ')[data.split(ParamDelimiter)[1].split(' ').length - 2];

                        ASDBNumber = ASDBNumber + 1;
                        if (ASDBNumber < ASDatabaseIds.split(",").length) {
                            GetASResultForDBNumber(IndicatorIC, Areas, LngCode, ASDatabaseIds, ASDBNumber, NumResults, ElapsedTime);
                        }
                        else {
                            di_jq('.sparkline').sparkline('html', { type: 'bar' });

                            GetGalleryThumbnails(0, 1);
                        }
                    }
                    catch (ex) {
//                        alert("message:" + ex.message);
                    }
                },
                error: function () {
//                    
                },
                cache: false
            });
        }
        catch (ex) {
        }
    }
    else {
        divQDSResults.innerHTML = "";
        spanTimeCounter.innerHTML = "";
        spanLoadingOuter.style.display = "none";
        spanLoading.innerHTML = "";
    }
}

function PostToDataView(IdUniquePart, DBNId) {
    //isBlock Issue : Removed Parameter isBlock
    createCookie('QdsCart', "") //added to resolve expanded addto cart issue
    var PostPageName = "dataview.aspx";
    var divJsonData = z('JsonData_' + IdUniquePart);
    var jsonData = di_jq.parseJSON(divJsonData.innerHTML);
    var jsonStructure = JSONStructure(IdUniquePart, jsonData);   //isBlock Issue : Added to make function call
    var DVNIds;
    z('hselind').value = "";
    z('hselarea').value = "";
    DVNIds = jsonStructure.NId;             //isBlock Issue : Replaced multiple if else loops with single statement

    if (z('hdsby').value == 'share') {
        if (jsonData.jsonBlock != null) DVNIds = jsonData.jsonBlock.NId;

        //        z('hselindo').value = "" + "[****]" + "";//Commented to Resolve Bug in Sharing
        //        z('hselareao').value = "" + "[****]" + "";//Commented to Resolve Bug in Sharing
        PostPageName = "../../../libraries/aspx/dataview.aspx";
    }
    if (IdUniquePart != undefined) {


        var hselindo = z('hselindo');
        var hselareao = z('hselareao');

        var hselind = z('hselind');
        var hselarea = z('hselarea');

        var Language = z('hlngcode').value;

      

        var jsonInd = di_jq.parseJSON(hselindo.value);
        var jsonArea = di_jq.parseJSON(hselareao.value);


        jsonInd = di_jq.parseJSON('{"iu":[],"sg_dim":{},"sg_dim_val":{},"iusnid":{}}');
        jsonArea = di_jq.parseJSON('{"area":{}}');

        var AreaLevel = "-1";// getAreaLevel(jsonData.Area, jsonData.AreaNId, DBNId, Language);
        var IUSNId = getIUSNId(jsonData.IndicatorNId, jsonData.UnitNId, DBNId, Language);

        var tmpIU = jsonData.IndicatorNId + "~" + jsonData.UnitNId;
        var tmpIndName_UnitName = jsonData.Indicator + "~" + jsonData.Unit;
        jsonArea.area[AreaLevel] = [];
        var jsonCurrentlyAddedToCart;
        jsonCurrentlyAddedToCart = jsonStructure;   //isBlock Issue : Replaced multiple if else loops with single statement


        var jsonSubAreas = getAreaNIds_NameInArray(jsonCurrentlyAddedToCart);
        var jsonVirtualAreasForCart = jsonArea.area[AreaLevel];

        for (var i = 0; i < jsonSubAreas.length; i++) {
            var tmpNowAreaNId_NameCombination = jsonSubAreas[i];
            if (jsonVirtualAreasForCart.indexOf(tmpNowAreaNId_NameCombination) == -1) {
                jsonVirtualAreasForCart.push(tmpNowAreaNId_NameCombination);
            }
        }
        jsonInd.iu.push(tmpIU + "||" + tmpIndName_UnitName);
        jsonInd.iusnid["iu_" + jsonData.IndicatorNId + "_" + jsonData.UnitNId] = IUSNId[1];

        hselindo.value = JSON.stringify(jsonInd);
        hselareao.value = JSON.stringify(jsonArea);

    

        hselind.value = IUSNId[0];
        hselarea.value += "," + jsonStructure.AreaNId;     //isBlock Issue : Replaced multiple if else loops with single statement
    }

    if (z('hdsby').value != 'share') z('hdsby').value = 'qds';
    z('hdbnid').value = DBNId;
    z('hdvnids').value = DVNIds + ":" + SelectedDimension;
    PostData("frmHome", PostPageName, "POST");
}

function PostToDataViewFromVisualize(IdUniquePart, DBNId) {
    z('hselind').value = "";
    z('hselarea').value = "";
    //isBlock Issue : Removed Parameter isBlock
    createCookie('QdsCart', "") //added to resolve expanded addto cart issue
    var PostPageName = "dataview.aspx";
    var divJsonData = z('JsonData_' + IdUniquePart);
    var jsonData = di_jq.parseJSON(divJsonData.innerHTML);
    var jsonStructure = JSONStructure(IdUniquePart, jsonData);   //isBlock Issue : Added to make function call
    var DVNIds;
    DVNIds = jsonStructure.NId + "-" + jsonStructure.DefaultSG.split('[@@@@]')[0];           //isBlock Issue : Replaced multiple if else loops with single statement
    if (z('hdsby').value == 'share') {
//        if (jsonData.jsonBlock != null) DVNIds = jsonData.jsonBlock.NId;

//        z('hselindo').value = "" + "[****]" + "";//Commented to Resolve Bug in Sharing
        //        z('hselareao').value = "" + "[****]" + "";//Commented to Resolve Bug in Sharing
        PostPageName = "../../../libraries/aspx/dataview.aspx";
    }
    if (IdUniquePart != undefined) {


        var hselindo = z('hselindo');
        var hselareao = z('hselareao');

        var hselind = z('hselind');
        var hselarea = z('hselarea');

        var Language = z('hlngcode').value;

        var jsonInd = di_jq.parseJSON(hselindo.value);
        var jsonArea = di_jq.parseJSON(hselareao.value);


        jsonInd = di_jq.parseJSON('{"iu":[],"sg_dim":{},"sg_dim_val":{},"iusnid":{}}');
        jsonArea = di_jq.parseJSON('{"area":{}}');

        var AreaLevel = "-1"; //getAreaLevel(jsonData.Area, jsonData.AreaNId, DBNId, Language);
        var IUSNId = getIUSNId(jsonData.IndicatorNId, jsonData.UnitNId, DBNId, Language);

        var tmpIU = jsonData.IndicatorNId + "~" + jsonData.UnitNId;
        var tmpIndName_UnitName = jsonData.Indicator + "~" + jsonData.Unit;

//        if (jsonArea.area[AreaLevel] == null) {
//            jsonArea.area[AreaLevel] = [];
        //        }
        //        jsonArea.area[AreaLevel].push(jsonData.AreaNId + "||" + jsonData.Area);
        jsonArea.area[AreaLevel] = [];

        var jsonCurrentlyAddedToCart;
        jsonCurrentlyAddedToCart = jsonStructure;   //isBlock Issue : Replaced multiple if else loops with single statement

        var jsonSubAreas = getAreaNIds_NameInArray(jsonCurrentlyAddedToCart);
        var jsonVirtualAreasForCart = jsonArea.area[AreaLevel];

        for (var i = 0; i < jsonSubAreas.length; i++) {
            var tmpNowAreaNId_NameCombination = jsonSubAreas[i];
            if (jsonVirtualAreasForCart.indexOf(tmpNowAreaNId_NameCombination) == -1) {
                jsonVirtualAreasForCart.push(tmpNowAreaNId_NameCombination);
            }
        }

        jsonInd.iu.push(tmpIU + "||" + tmpIndName_UnitName);
        jsonInd.iusnid["iu_" + jsonData.IndicatorNId + "_" + jsonData.UnitNId] = IUSNId[1];

        hselindo.value = JSON.stringify(jsonInd);
        hselareao.value = JSON.stringify(jsonArea);

        hselind.value = IUSNId[0];
        hselarea.value += "," + jsonStructure.AreaNId;     //isBlock Issue : Replaced multiple if else loops with single statement
    }

    if (z('hdsby').value != 'share') z('hdsby').value = 'qds';
    z('hdbnid').value = DBNId;
    z('hdvnids').value = DVNIds + ":";
    if (IsShowElements == true || IsShowElements == undefined || IsShowElements == null) {
        PostData("frmHome", PostPageName, "POST");
    }
    else {
        PostDataForQDS("frmHome", PostPageName, "POST");
    }
}

//&& pgName != 'RegStructuralMD.aspx'
/* function to post data */
function PostDataForQDS(formID, pgName, PostingMethod) {
    if (z('hOriginaldbnid') != null && pgName != 'RegData.aspx' && pgName != 'RegStructure.aspx' && pgName != 'RegStructuralMD-Metadata.aspx' &&
    pgName != 'RegSubscription.aspx' && pgName != 'RegPublishData.aspx' && pgName != 'RegProviders.aspx' &&
    pgName != 'RegValidation.aspx' && pgName != 'RegComparison.aspx' && pgName != 'RegWebServiceDemo.aspx' &&
    pgName != 'RegDiscover.aspx' && pgName != 'Login.aspx' && pgName != 'RegMapping.aspx' &&
    pgName != 'RegMaintenanceAgency.aspx' && pgName != 'RegDatabaseManagement.aspx' && pgName != 'RegUpload.aspx' && pgName != 'RegDataProvider.aspx' && pgName != 'RegDiscoverRegistrations.aspx' && pgName != 'RegDataQuery.aspx' && pgName != 'RegTools.aspx') {
        z('hdbnid').value = z('hOriginaldbnid').value;
    }

    z(formID).action = pgName;
    z(formID).method = PostingMethod;
    z(formID).setAttribute("target", "_blank");
    z(formID).submit();
}

//Data cart to dataview
function PostAllToDataView(DBNId, DVNIds) {
    var PostPageName = "dataview.aspx";

    if (z('hdsby').value == 'share') {
        if (jsonData.jsonBlock != null) DVNIds = jsonData.jsonBlock.NId;
        
        //        z('hselindo').value = "" + "[****]" + "";//Commented to Resolve Bug in Sharing
        //        z('hselareao').value = "" + "[****]" + "";//Commented to Resolve Bug in Sharing
        PostPageName = "../../../libraries/aspx/dataview.aspx";
    }

    if (z('hdsby').value != 'share') z('hdsby').value = 'qds';
    z('hdbnid').value = DBNId;
    var hdvnids = z('hdvnids').value;
    var oldDVNIDs = hdvnids.split("[####]")[1];

    //    z('hdvnids').value = DVNIds + ":" + SelectedDimension;
    z('hdvnids').value = DVNIds + ":" + "[####]" + oldDVNIDs;
    var finaldvnids = z('hdvnids').value;
    //Added to resolve expanded add to cart bug
    
    PostData("frmHome", PostPageName, "POST");
}


function GetTextAndBlockTextCombinedValue(Text, BlockText) {
    var RetVal;

    RetVal = "";
    if (Text != null && Text != '') {
        Text = ReplaceAll(Text, ",", "||");
        RetVal = Text;

        if (BlockText != null && BlockText != '') {
            RetVal = RetVal + "||" + BlockText;
        }
    }
    else {
        if (BlockText != null && BlockText != '') {
            RetVal = BlockText;
        }
    }

    return RetVal;
}

function Get_Indicators_from_components_IndicatorIC(IndicatorIC) {
    var Indicators;

    Indicators = '';
    for (var i = 0; i < IndicatorIC.length; i++) {
        var arrInd_IC = IndicatorIC[i].split('_');
        if (arrInd_IC[1] == undefined) {
            Indicators = Indicators + arrInd_IC[0] + ","; 
        }
        else if (arrInd_IC[1] == 1) {
            Indicators = Indicators + arrInd_IC[0] + ","; // If I_U is provided by component then fetch I out of it
        }        
    }

    if (Indicators.length > 0) {
        Indicators = Indicators.substr(0, Indicators.length - 1);
    }

    return Indicators;
}

function Get_IC_from_components_IndicatorIC(IndicatorIC) {
    var ICs;

    ICs = '';
    for (var i = 0; i < IndicatorIC.length; i++) {
        if (IndicatorIC[i].split('_')[1] == 0) {
            ICs = ICs + IndicatorIC[i].split('_')[0] + ",";
        }
    }

    if (ICs.length > 0) {
        ICs = ICs.substr(0, ICs.length - 1);
    }

    return ICs;
}

function GetCallbackPageLocation() {
    var RetVal;

    if (z('hdsby').value == 'share') {
        RetVal = "../../../libraries/aspx/" + CallBackPageName;
    }
    else {
        RetVal = CallBackPageName;
    }

    return RetVal;
}

function ToggleExpandCollapse(evt) {
    var eventRaiser;
    var eventTarget, eventBuffer;
    var GalleryTester = new RegExp("btnGallery_");
    var VisualizerTester = new RegExp("btnVisualizer_");
    evt = evt || window.event;
    if (evt.srcElement != null) {
        eventRaiser = evt.srcElement;
    }
    else if (evt.target != null) {
        eventRaiser = evt.target;
    }

    if (GalleryTester.test(eventRaiser.id) == true) {
        eventTarget = '#Gallery_' + eventRaiser.id.replace("btnGallery_", "");
        eventBuffer = '#GalleryBuffer_' + eventRaiser.id.replace("btnGallery_", "");
    }
    else if (VisualizerTester.test(eventRaiser.id) == true) {
        eventTarget = '#Visualizer_' + eventRaiser.id.replace("btnVisualizer_", "");
        eventBuffer = '#VisualizerBuffer_' + eventRaiser.id.replace("btnVisualizer_", "");
    }

    if (eventRaiser.getAttribute('class') == 'expand') {
        eventRaiser.setAttribute('class', "collapse");
    }
    else if (eventRaiser.getAttribute('class') == 'collapse') {
        eventRaiser.setAttribute('class', "expand");
    }

    di_jq(eventTarget).slideToggle();
    di_jq(eventBuffer).slideToggle();
}

/**
*
* jquery.sparkline.js
*
* v1.6
* (c) Splunk, Inc 
* Contact: Gareth Watts (gareth@splunk.com)
* http://omnipotent.net/jquery.sparkline/
*
* Generates inline sparkline charts from data supplied either to the method
* or inline in HTML
* 
* Compatible with Internet Explorer 6.0+ and modern browsers equipped with the canvas tag
* (Firefox 2.0+, Safari, Opera, etc)
*
* License: New BSD License
* 
* Copyright (c) 2010, Splunk Inc.
* All rights reserved.
* 
* Redistribution and use in source and binary forms, with or without modification, 
* are permitted provided that the following conditions are met:
* 
*     * Redistributions of source code must retain the above copyright notice, 
*       this list of conditions and the following disclaimer.
*     * Redistributions in binary form must reproduce the above copyright notice, 
*       this list of conditions and the following disclaimer in the documentation 
*       and/or other materials provided with the distribution.
*     * Neither the name of Splunk Inc nor the names of its contributors may 
*       be used to endorse or promote products derived from this software without 
*       specific prior written permission.
* 
* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
* EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
* OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
* SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, 
* SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT 
* OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
* HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, 
* OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS 
* SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
* 
*
* Usage: 
*  di_jq(selector).sparkline(values, options)
*
* If values is undefined or set to 'html' then the data values are read from the specified tag:
*   <p>Sparkline: <span class="sparkline">1,4,6,6,8,5,3,5</span></p>
*   di_jq('.sparkline').sparkline();
* There must be no spaces in the enclosed data set
*
* Otherwise values must be an array of numbers or null values
*    <p>Sparkline: <span id="sparkline1">This text replaced if the browser is compatible</span></p>
*    di_jq('#sparkline1').sparkline([1,4,6,6,8,5,3,5])
*    di_jq('#sparkline2').sparkline([1,4,6,null,null,5,3,5])
*
* Values can also be specified in an HTML comment, or as a values attribute:
*    <p>Sparkline: <span class="sparkline"><!--1,4,6,6,8,5,3,5 --></span></p>
*    <p>Sparkline: <span class="sparkline" values="1,4,6,6,8,5,3,5"></span></p>
*    di_jq('.sparkline').sparkline();
*
* For line charts, x values can also be specified:
*   <p>Sparkline: <span class="sparkline">1:1,2.7:4,3.4:6,5:6,6:8,8.7:5,9:3,10:5</span></p>
*    di_jq('#sparkline1').sparkline([ [1,1], [2.7,4], [3.4,6], [5,6], [6,8], [8.7,5], [9,3], [10,5] ])
*
* By default, options should be passed in as teh second argument to the sparkline function:
*   di_jq('.sparkline').sparkline([1,2,3,4], {type: 'bar'})
*
* Options can also be set by passing them on the tag itself.  This feature is disabled by default though
* as there's a slight performance overhead:
*   di_jq('.sparkline').sparkline([1,2,3,4], {enableTagOptions: true})
*   <p>Sparkline: <span class="sparkline" sparkType="bar" sparkBarColor="red">loading</span></p>
* Prefix all options supplied as tag attribute with "spark" (configurable by setting tagOptionPrefix)
*
* Supported options:
*   lineColor - Color of the line used for the chart
*   fillColor - Color used to fill in the chart - Set to '' or false for a transparent chart
*   width - Width of the chart - Defaults to 3 times the number of values in pixels
*   height - Height of the chart - Defaults to the height of the containing element
*   chartRangeMin - Specify the minimum value to use for the Y range of the chart - Defaults to the minimum value supplied
*   chartRangeMax - Specify the maximum value to use for the Y range of the chart - Defaults to the maximum value supplied
*   chartRangeClip - Clip out of range values to the max/min specified by chartRangeMin and chartRangeMax
*   chartRangeMinX - Specify the minimum value to use for the X range of the chart - Defaults to the minimum value supplied
*   chartRangeMaxX - Specify the maximum value to use for the X range of the chart - Defaults to the maximum value supplied
*   composite - If true then don't erase any existing chart attached to the tag, but draw
*           another chart over the top - Note that width and height are ignored if an
*           existing chart is detected.
*   tagValuesAttribute - Name of tag attribute to check for data values - Defaults to 'values'
*   enableTagOptions - Whether to check tags for sparkline options 
*   tagOptionPrefix - Prefix used for options supplied as tag attributes - Defaults to 'spark'
*
* There are 7 types of sparkline, selected by supplying a "type" option of 'line' (default),
* 'bar', 'tristate', 'bullet', 'discrete', 'pie' or 'box'
*    line - Line chart.  Options:
*       spotColor - Set to '' to not end each line in a circular spot
*       minSpotColor - If set, color of spot at minimum value
*       maxSpotColor - If set, color of spot at maximum value
*       spotRadius - Radius in pixels
*       lineWidth - Width of line in pixels
*       normalRangeMin 
*       normalRangeMax - If set draws a filled horizontal bar between these two values marking the "normal"
*                      or expected range of values
*       normalRangeColor - Color to use for the above bar
*       drawNormalOnTop - Draw the normal range above the chart fill color if true
*       defaultPixelsPerValue - Defaults to 3 pixels of width for each value in the chart
*
*   bar - Bar chart.  Options:
*       barColor - Color of bars for postive values
*       negBarColor - Color of bars for negative values
*       zeroColor - Color of bars with zero values
*       nullColor - Color of bars with null values - Defaults to omitting the bar entirely
*       barWidth - Width of bars in pixels
*       colorMap - Optional mappnig of values to colors to override the *BarColor values above
*                  can be an Array of values to control the color of individual bars
*       barSpacing - Gap between bars in pixels
*       zeroAxis - Centers the y-axis around zero if true
*
*   tristate - Charts values of win (>0), lose (<0) or draw (=0)
*       posBarColor - Color of win values
*       negBarColor - Color of lose values
*       zeroBarColor - Color of draw values
*       barWidth - Width of bars in pixels
*       barSpacing - Gap between bars in pixels
*       colorMap - Optional mappnig of values to colors to override the *BarColor values above
*                  can be an Array of values to control the color of individual bars
*
*   discrete - Options:
*       lineHeight - Height of each line in pixels - Defaults to 30% of the graph height
*       thesholdValue - Values less than this value will be drawn using thresholdColor instead of lineColor
*       thresholdColor
*
*   bullet - Values for bullet graphs msut be in the order: target, performance, range1, range2, range3, ...
*       options:
*       targetColor - The color of the vertical target marker
*       targetWidth - The width of the target marker in pixels
*       performanceColor - The color of the performance measure horizontal bar
*       rangeColors - Colors to use for each qualitative range background color
*
*   pie - Pie chart. Options:
*       sliceColors - An array of colors to use for pie slices
*       offset - Angle in degrees to offset the first slice - Try -90 or +90
*
*   box - Box plot. Options:
*       raw - Set to true to supply pre-computed plot points as values
*             values should be: low_outlier, low_whisker, q1, median, q3, high_whisker, high_outlier
*             When set to false you can supply any number of values and the box plot will
*             be computed for you.  Default is false.
*       showOutliers - Set to true (default) to display outliers as circles
*       outlierIRQ - Interquartile range used to determine outliers.  Default 1.5
*       boxLineColor - Outline color of the box
*       boxFillColor - Fill color for the box
*       whiskerColor - Line color used for whiskers
*       outlierLineColor - Outline color of outlier circles
*       outlierFillColor - Fill color of the outlier circles
*       spotRadius - Radius of outlier circles
*       medianColor - Line color of the median line
*       target - Draw a target cross hair at the supplied value (default undefined)
*      
*   
*       
*   Examples:
*   di_jq('#sparkline1').sparkline(myvalues, { lineColor: '#f00', fillColor: false });
*   di_jq('.barsparks').sparkline('html', { type:'bar', height:'40px', barWidth:5 });
*   di_jq('#tristate').sparkline([1,1,-1,1,0,0,-1], { type:'tristate' }):
*   di_jq('#discrete').sparkline([1,3,4,5,5,3,4,5], { type:'discrete' });
*   di_jq('#bullet').sparkline([10,12,12,9,7], { type:'bullet' });
*   di_jq('#pie').sparkline([1,1,2], { type:'pie' });
*/


(function (di_jq) {

    /*
    * Default configuration settings
    */
    var defaults = {
        // Settings common to most/all chart types
        common: {
            type: 'line',
            lineColor: '#00f',
            fillColor: '#cdf',
            defaultPixelsPerValue: 3,
            width: 'auto',
            height: 'auto',
            composite: false,
            tagValuesAttribute: 'values',
            tagOptionsPrefix: 'spark',
            enableTagOptions: false
        },
        // Defaults for line charts
        line: {
            spotColor: '#f80',
            spotRadius: 1.5,
            minSpotColor: '#f80',
            maxSpotColor: '#f80',
            lineWidth: 1,
            normalRangeMin: undefined,
            normalRangeMax: undefined,
            normalRangeColor: '#ccc',
            drawNormalOnTop: false,
            chartRangeMin: undefined,
            chartRangeMax: undefined,
            chartRangeMinX: undefined,
            chartRangeMaxX: undefined
        },
        // Defaults for bar charts
        bar: {
            barColor: '#aaaaaa',
            negBarColor: '#f44',
            zeroColor: undefined,
            nullColor: undefined,
            zeroAxis: undefined,
            barWidth: 5,
            barSpacing: 2,
            chartRangeMax: undefined,
            chartRangeMin: 0,
            chartRangeClip: false,
            colorMap: undefined
        },
        // Defaults for tristate charts
        tristate: {
            barWidth: 4,
            barSpacing: 1,
            posBarColor: '#6f6',
            negBarColor: '#f44',
            zeroBarColor: '#999',
            colorMap: {}
        },
        // Defaults for discrete charts
        discrete: {
            lineHeight: 'auto',
            thresholdColor: undefined,
            thresholdValue: 0,
            chartRangeMax: undefined,
            chartRangeMin: undefined,
            chartRangeClip: false
        },
        // Defaults for bullet charts
        bullet: {
            targetColor: 'red',
            targetWidth: 3, // width of the target bar in pixels
            performanceColor: 'blue',
            rangeColors: ['#D3DAFE', '#A8B6FF', '#7F94FF'],
            base: undefined // set this to a number to change the base start number
        },
        // Defaults for pie charts
        pie: {
            sliceColors: ['#f00', '#0f0', '#00f']
        },
        // Defaults for box plots
        box: {
            raw: false,
            boxLineColor: 'black',
            boxFillColor: '#cdf',
            whiskerColor: 'black',
            outlierLineColor: '#333',
            outlierFillColor: 'white',
            medianColor: 'red',
            showOutliers: true,
            outlierIQR: 1.5,
            spotRadius: 1.5,
            target: undefined,
            targetColor: '#4a2',
            chartRangeMax: undefined,
            chartRangeMin: undefined
        }
    };

    // Provide a cross-browser interface to a few simple drawing primitives
    var VCanvas_base, VCanvas_canvas, VCanvas_vml;
    di_jq.fn.simpledraw = function (width, height, use_existing) {
        if (use_existing && this[0].VCanvas) {
            return this[0].VCanvas;
        }
        if (width === undefined) {
            width = di_jq(this).innerWidth();
        }
        if (height === undefined) {
            height = di_jq(this).innerHeight();
        }
        if (di_jq.browser.hasCanvas) {
            return new VCanvas_canvas(width, height, this);
        } else if (di_jq.browser.msie) {
            return new VCanvas_vml(width, height, this);
        } else {
            return false;
        }
    };

    var pending = [];


    di_jq.fn.sparkline = function (uservalues, userOptions) {
        return this.each(function () {
            var options = new di_jq.fn.sparkline.options(this, userOptions);
            var render = function () {
                var values, width, height;
                if (uservalues === 'html' || uservalues === undefined) {
                    var vals = this.getAttribute(options.get('tagValuesAttribute'));
                    if (vals === undefined || vals === null) {
                        vals = di_jq(this).html();
                    }
                    values = vals.replace(/(^\s*<!--)|(-->\s*di_jq)|\s+/g, '').split(',');
                } else {
                    values = uservalues;
                }

                width = options.get('width') == 'auto' ? values.length * options.get('defaultPixelsPerValue') : options.get('width');
                if (options.get('height') == 'auto') {
                    if (!options.get('composite') || !this.VCanvas) {
                        // must be a better way to get the line height
                        var tmp = document.createElement('span');
                        tmp.innerHTML = 'a';
                        di_jq(this).html(tmp);
                        height = di_jq(tmp).innerHeight();
                        di_jq(tmp).remove();
                    }
                } else {
                    height = options.get('height');
                }

                di_jq.fn.sparkline[options.get('type')].call(this, values, options, width, height);
            };
            // jQuery 1.3.0 completely changed the meaning of :hidden :-/
            if ((di_jq(this).html() && di_jq(this).is(':hidden')) || (di_jq.fn.jquery < "1.3.0" && di_jq(this).parents().is(':hidden')) || !di_jq(this).parents('body').length) {
                pending.push([this, render]);
            } else {
                render.call(this);
            }
        });
    };

    di_jq.fn.sparkline.defaults = defaults;


    di_jq.sparkline_display_visible = function () {
        for (var i = pending.length - 1; i >= 0; i--) {
            var el = pending[i][0];
            if (di_jq(el).is(':visible') && !di_jq(el).parents().is(':hidden')) {
                pending[i][1].call(el);
                pending.splice(i, 1);
            }
        }
    };


    /**
    * User option handler
    */
    var UNSET_OPTION = {};
    var normalizeValue = function (val) {
        switch (val) {
            case 'undefined':
                val = undefined;
                break;
            case 'null':
                val = null;
                break;
            case 'true':
                val = true;
                break;
            case 'false':
                val = false;
                break;
            default:
                var nf = parseFloat(val);
                if (val == nf) {
                    val = nf;
                }
        }
        return val;
    };
    di_jq.fn.sparkline.options = function (tag, userOptions) {
        var extendedOptions;
        this.userOptions = userOptions = userOptions || {};
        this.tag = tag;
        this.tagValCache = {};
        var defaults = di_jq.fn.sparkline.defaults;
        var base = defaults.common;
        this.tagOptionsPrefix = userOptions.enableTagOptions && (userOptions.tagOptionsPrefix || base.tagOptionsPrefix);

        var tagOptionType = this.getTagSetting('type');
        if (tagOptionType === UNSET_OPTION) {
            extendedOptions = defaults[userOptions.type || base.type];
        } else {
            extendedOptions = defaults[tagOptionType];
        }
        this.mergedOptions = di_jq.extend({}, base, extendedOptions, userOptions);
    };


    di_jq.fn.sparkline.options.prototype.getTagSetting = function (key) {
        var val, i, prefix = this.tagOptionsPrefix;
        if (prefix === false || prefix === undefined) {
            return UNSET_OPTION;
        }
        if (this.tagValCache.hasOwnProperty(key)) {
            val = this.tagValCache.key;
        } else {
            val = this.tag.getAttribute(prefix + key);
            if (val === undefined || val === null) {
                val = UNSET_OPTION;
            } else if (val.substr(0, 1) == '[') {
                val = val.substr(1, val.length - 2).split(',');
                for (i = val.length; i--; ) {
                    val[i] = normalizeValue(val[i].replace(/(^\s*)|(\s*di_jq)/g, ''));
                }
            } else if (val.substr(0, 1) == '{') {
                var pairs = val.substr(1, val.length - 2).split(',');
                val = {};
                for (i = pairs.length; i--; ) {
                    var keyval = pairs[i].split(':', 2);
                    val[keyval[0].replace(/(^\s*)|(\s*di_jq)/g, '')] = normalizeValue(keyval[1].replace(/(^\s*)|(\s*di_jq)/g, ''));
                }
            } else {
                val = normalizeValue(val);
            }
            this.tagValCache.key = val;
        }
        return val;
    };

    di_jq.fn.sparkline.options.prototype.get = function (key) {
        var tagOption = this.getTagSetting(key);
        if (tagOption !== UNSET_OPTION) {
            return tagOption;
        }
        return this.mergedOptions[key];
    };


    /**
    * Line charts
    */
    di_jq.fn.sparkline.line = function (values, options, width, height) {
        var xvalues = [], yvalues = [], yminmax = [];
        for (var i = 0; i < values.length; i++) {
            var val = values[i];
            var isstr = typeof (values[i]) == 'string';
            var isarray = typeof (values[i]) == 'object' && values[i] instanceof Array;
            var sp = isstr && values[i].split(':');
            if (isstr && sp.length == 2) { // x:y
                xvalues.push(Number(sp[0]));
                yvalues.push(Number(sp[1]));
                yminmax.push(Number(sp[1]));
            } else if (isarray) {
                xvalues.push(val[0]);
                yvalues.push(val[1]);
                yminmax.push(val[1]);
            } else {
                xvalues.push(i);
                if (values[i] === null || values[i] == 'null') {
                    yvalues.push(null);
                } else {
                    yvalues.push(Number(val));
                    yminmax.push(Number(val));
                }
            }
        }
        if (options.get('xvalues')) {
            xvalues = options.get('xvalues');
        }

        var maxy = Math.max.apply(Math, yminmax);
        var maxyval = maxy;
        var miny = Math.min.apply(Math, yminmax);
        var minyval = miny;

        var maxx = Math.max.apply(Math, xvalues);
        var minx = Math.min.apply(Math, xvalues);

        var normalRangeMin = options.get('normalRangeMin');
        var normalRangeMax = options.get('normalRangeMax');

        if (normalRangeMin !== undefined) {
            if (normalRangeMin < miny) {
                miny = normalRangeMin;
            }
            if (normalRangeMax > maxy) {
                maxy = normalRangeMax;
            }
        }
        if (options.get('chartRangeMin') !== undefined && (options.get('chartRangeClip') || options.get('chartRangeMin') < miny)) {
            miny = options.get('chartRangeMin');
        }
        if (options.get('chartRangeMax') !== undefined && (options.get('chartRangeClip') || options.get('chartRangeMax') > maxy)) {
            maxy = options.get('chartRangeMax');
        }
        if (options.get('chartRangeMinX') !== undefined && (options.get('chartRangeClipX') || options.get('chartRangeMinX') < minx)) {
            minx = options.get('chartRangeMinX');
        }
        if (options.get('chartRangeMaxX') !== undefined && (options.get('chartRangeClipX') || options.get('chartRangeMaxX') > maxx)) {
            maxx = options.get('chartRangeMaxX');
        }
        var rangex = maxx - minx === 0 ? 1 : maxx - minx;
        var rangey = maxy - miny === 0 ? 1 : maxy - miny;
        var vl = yvalues.length - 1;

        if (vl < 1) {
            this.innerHTML = '';
            return;
        }

        var target = di_jq(this).simpledraw(width, height, options.get('composite'));
        if (target) {
            var canvas_width = target.pixel_width;
            var canvas_height = target.pixel_height;
            var canvas_top = 0;
            var canvas_left = 0;

            var spotRadius = options.get('spotRadius');
            if (spotRadius && (canvas_width < (spotRadius * 4) || canvas_height < (spotRadius * 4))) {
                spotRadius = 0;
            }
            if (spotRadius) {
                // adjust the canvas size as required so that spots will fit
                if (options.get('minSpotColor') || (options.get('spotColor') && yvalues[vl] == miny)) {
                    canvas_height -= Math.ceil(spotRadius);
                }
                if (options.get('maxSpotColor') || (options.get('spotColor') && yvalues[vl] == maxy)) {
                    canvas_height -= Math.ceil(spotRadius);
                    canvas_top += Math.ceil(spotRadius);
                }
                if (options.get('minSpotColor') || options.get('maxSpotColor') && (yvalues[0] == miny || yvalues[0] == maxy)) {
                    canvas_left += Math.ceil(spotRadius);
                    canvas_width -= Math.ceil(spotRadius);
                }
                if (options.get('spotColor') || (options.get('minSpotColor') || options.get('maxSpotColor') && (yvalues[vl] == miny || yvalues[vl] == maxy))) {
                    canvas_width -= Math.ceil(spotRadius);
                }
            }


            canvas_height--;

            var drawNormalRange = function () {
                if (normalRangeMin !== undefined) {
                    var ytop = canvas_top + Math.round(canvas_height - (canvas_height * ((normalRangeMax - miny) / rangey)));
                    var height = Math.round((canvas_height * (normalRangeMax - normalRangeMin)) / rangey);
                    target.drawRect(canvas_left, ytop, canvas_width, height, undefined, options.get('normalRangeColor'));
                }
            };

            if (!options.get('drawNormalOnTop')) {
                drawNormalRange();
            }

            var path = [];
            var paths = [path];
            var x, y, vlen = yvalues.length;
            for (i = 0; i < vlen; i++) {
                x = xvalues[i];
                y = yvalues[i];
                if (y === null) {
                    if (i) {
                        if (yvalues[i - 1] !== null) {
                            path = [];
                            paths.push(path);
                        }
                    }
                } else {
                    if (y < miny) {
                        y = miny;
                    }
                    if (y > maxy) {
                        y = maxy;
                    }
                    if (!path.length) {
                        // previous value was null
                        path.push([canvas_left + Math.round((x - minx) * (canvas_width / rangex)), canvas_top + canvas_height]);
                    }
                    path.push([canvas_left + Math.round((x - minx) * (canvas_width / rangex)), canvas_top + Math.round(canvas_height - (canvas_height * ((y - miny) / rangey)))]);
                }
            }
            var lineshapes = [];
            var fillshapes = [];
            var plen = paths.length;
            for (i = 0; i < plen; i++) {
                path = paths[i];
                if (!path.length) {
                    continue; // last value was null
                }
                if (options.get('fillColor')) {
                    path.push([path[path.length - 1][0], canvas_top + canvas_height - 1]);
                    fillshapes.push(path.slice(0));
                    path.pop();
                }
                // if there's only a single point in this path, then we want to display it as a vertical line
                // which means we keep path[0]  as is
                if (path.length > 2) {
                    // else we want the first value 
                    path[0] = [path[0][0], path[1][1]];
                }
                lineshapes.push(path);
            }

            // draw the fill first, then optionally the normal range, then the line on top of that
            plen = fillshapes.length;
            for (i = 0; i < plen; i++) {
                target.drawShape(fillshapes[i], undefined, options.get('fillColor'));
            }

            if (options.get('drawNormalOnTop')) {
                drawNormalRange();
            }

            plen = lineshapes.length;
            for (i = 0; i < plen; i++) {
                target.drawShape(lineshapes[i], options.get('lineColor'), undefined, options.get('lineWidth'));
            }

            if (spotRadius && options.get('spotColor')) {
                target.drawCircle(canvas_left + Math.round(xvalues[xvalues.length - 1] * (canvas_width / rangex)), canvas_top + Math.round(canvas_height - (canvas_height * ((yvalues[vl] - miny) / rangey))), spotRadius, undefined, options.get('spotColor'));
            }
            if (maxy != minyval) {
                if (spotRadius && options.get('minSpotColor')) {
                    x = xvalues[di_jq.inArray(minyval, yvalues)];
                    target.drawCircle(canvas_left + Math.round((x - minx) * (canvas_width / rangex)), canvas_top + Math.round(canvas_height - (canvas_height * ((minyval - miny) / rangey))), spotRadius, undefined, options.get('minSpotColor'));
                }
                if (spotRadius && options.get('maxSpotColor')) {
                    x = xvalues[di_jq.inArray(maxyval, yvalues)];
                    target.drawCircle(canvas_left + Math.round((x - minx) * (canvas_width / rangex)), canvas_top + Math.round(canvas_height - (canvas_height * ((maxyval - miny) / rangey))), spotRadius, undefined, options.get('maxSpotColor'));
                }
            }

        } else {
            // Remove the tag contents if sparklines aren't supported
            this.innerHTML = '';
        }
    };


    /** 
    * Bar charts
    */
    di_jq.fn.sparkline.bar = function (values, options, width, height) {
        width = (values.length * options.get('barWidth')) + ((values.length - 1) * options.get('barSpacing'));
        var num_values = [];
        for (var i = 0, vlen = values.length; i < vlen; i++) {
            if (values[i] == 'null' || values[i] === null) {
                values[i] = null;
            } else {
                values[i] = Number(values[i]);
                num_values.push(Number(values[i]));
            }
        }
        var max = Math.max.apply(Math, num_values),
            min = Math.min.apply(Math, num_values);
        if (options.get('chartRangeMin') !== undefined && (options.get('chartRangeClip') || options.get('chartRangeMin') < min)) {
            min = options.get('chartRangeMin');
        }
        if (options.get('chartRangeMax') !== undefined && (options.get('chartRangeClip') || options.get('chartRangeMax') > max)) {
            max = options.get('chartRangeMax');
        }
        var zeroAxis = options.get('zeroAxis');
        if (zeroAxis === undefined) {
            zeroAxis = min < 0;
        }
        var range = max - min === 0 ? 1 : max - min;

        var colorMapByIndex, colorMapByValue;
        if (di_jq.isArray(options.get('colorMap'))) {
            colorMapByIndex = options.get('colorMap');
            colorMapByValue = null;
        } else {
            colorMapByIndex = null;
            colorMapByValue = options.get('colorMap');
        }

        var target = di_jq(this).simpledraw(width, height, options.get('composite'));
        if (target) {
            var color,
                canvas_height = target.pixel_height,
                yzero = min < 0 && zeroAxis ? canvas_height - Math.round(canvas_height * (Math.abs(min) / range)) - 1 : canvas_height - 1;

            for (i = values.length; i--; ) {
                var x = i * (options.get('barWidth') + options.get('barSpacing')),
                    y,
                    val = values[i];
                if (val === null) {
                    if (options.get('nullColor')) {
                        color = options.get('nullColor');
                        val = (zeroAxis && min < 0) ? 0 : min;
                        height = 1;
                        y = (zeroAxis && min < 0) ? yzero : canvas_height - height;
                    } else {
                        continue;
                    }
                } else {
                    if (val < min) {
                        val = min;
                    }
                    if (val > max) {
                        val = max;
                    }
                    color = (val < 0) ? options.get('negBarColor') : options.get('barColor');
                    if (zeroAxis && min < 0) {
                        height = Math.round(canvas_height * ((Math.abs(val) / range))) + 1;
                        y = (val < 0) ? yzero : yzero - height;
                    } else {
                        height = Math.round(canvas_height * ((val - min) / range)) + 1;
                        y = canvas_height - height;
                    }
                    if (val === 0 && options.get('zeroColor') !== undefined) {
                        color = options.get('zeroColor');
                    }
                    if (colorMapByValue && colorMapByValue[val]) {
                        color = colorMapByValue[val];
                    } else if (colorMapByIndex && colorMapByIndex.length > i) {
                        color = colorMapByIndex[i];
                    }
                    if (color === null) {
                        continue;
                    }
                }
                target.drawRect(x, y, options.get('barWidth') - 1, height - 1, color, color);
            }
        } else {
            // Remove the tag contents if sparklines aren't supported
            this.innerHTML = '';
        }
    };


    /**
    * Tristate charts
    */
    di_jq.fn.sparkline.tristate = function (values, options, width, height) {
        values = di_jq.map(values, Number);
        width = (values.length * options.get('barWidth')) + ((values.length - 1) * options.get('barSpacing'));

        var colorMapByIndex, colorMapByValue;
        if (di_jq.isArray(options.get('colorMap'))) {
            colorMapByIndex = options.get('colorMap');
            colorMapByValue = null;
        } else {
            colorMapByIndex = null;
            colorMapByValue = options.get('colorMap');
        }

        var target = di_jq(this).simpledraw(width, height, options.get('composite'));
        if (target) {
            var canvas_height = target.pixel_height,
                half_height = Math.round(canvas_height / 2);

            for (var i = values.length; i--; ) {
                var x = i * (options.get('barWidth') + options.get('barSpacing')),
                    y, color;
                if (values[i] < 0) {
                    y = half_height;
                    height = half_height - 1;
                    color = options.get('negBarColor');
                } else if (values[i] > 0) {
                    y = 0;
                    height = half_height - 1;
                    color = options.get('posBarColor');
                } else {
                    y = half_height - 1;
                    height = 2;
                    color = options.get('zeroBarColor');
                }
                if (colorMapByValue && colorMapByValue[values[i]]) {
                    color = colorMapByValue[values[i]];
                } else if (colorMapByIndex && colorMapByIndex.length > i) {
                    color = colorMapByIndex[i];
                }
                if (color === null) {
                    continue;
                }
                target.drawRect(x, y, options.get('barWidth') - 1, height - 1, color, color);
            }
        } else {
            // Remove the tag contents if sparklines aren't supported
            this.innerHTML = '';
        }
    };


    /** 
    * Discrete charts
    */
    di_jq.fn.sparkline.discrete = function (values, options, width, height) {
        values = di_jq.map(values, Number);
        width = options.get('width') == 'auto' ? values.length * 2 : width;
        var interval = Math.floor(width / values.length);

        var target = di_jq(this).simpledraw(width, height, options.get('composite'));
        if (target) {
            var canvas_height = target.pixel_height,
                line_height = options.get('lineHeight') == 'auto' ? Math.round(canvas_height * 0.3) : options.get('lineHeight'),
                pheight = canvas_height - line_height,
                min = Math.min.apply(Math, values),
                max = Math.max.apply(Math, values);
            if (options.get('chartRangeMin') !== undefined && (options.get('chartRangeClip') || options.get('chartRangeMin') < min)) {
                min = options.get('chartRangeMin');
            }
            if (options.get('chartRangeMax') !== undefined && (options.get('chartRangeClip') || options.get('chartRangeMax') > max)) {
                max = options.get('chartRangeMax');
            }
            var range = max - min;

            for (var i = values.length; i--; ) {
                var val = values[i];
                if (val < min) {
                    val = min;
                }
                if (val > max) {
                    val = max;
                }
                var x = (i * interval),
                    ytop = Math.round(pheight - pheight * ((val - min) / range));
                target.drawLine(x, ytop, x, ytop + line_height, (options.get('thresholdColor') && val < options.get('thresholdValue')) ? options.get('thresholdColor') : options.get('lineColor'));
            }
        } else {
            // Remove the tag contents if sparklines aren't supported
            this.innerHTML = '';
        }

    };


    /**
    * Bullet charts
    */
    di_jq.fn.sparkline.bullet = function (values, options, width, height) {
        values = di_jq.map(values, Number);
        // target, performance, range1, range2, range3

        width = options.get('width') == 'auto' ? '4.0em' : width;

        var target = di_jq(this).simpledraw(width, height, options.get('composite'));
        if (target && values.length > 1) {
            var canvas_width = target.pixel_width - Math.ceil(options.get('targetWidth') / 2),
                canvas_height = target.pixel_height,
                min = Math.min.apply(Math, values),
                max = Math.max.apply(Math, values);

            if (options.get('base') === undefined) {
                min = min < 0 ? min : 0;
            } else {
                min = options.get('base');
            }
            var range = max - min;

            // draw range values
            for (var i = 2, vlen = values.length; i < vlen; i++) {
                var rangeval = values[i],
                    rangewidth = Math.round(canvas_width * ((rangeval - min) / range));
                target.drawRect(0, 0, rangewidth - 1, canvas_height - 1, options.get('rangeColors')[i - 2], options.get('rangeColors')[i - 2]);
            }

            // draw the performance bar
            var perfval = values[1],
                perfwidth = Math.round(canvas_width * ((perfval - min) / range));
            target.drawRect(0, Math.round(canvas_height * 0.3), perfwidth - 1, Math.round(canvas_height * 0.4) - 1, options.get('performanceColor'), options.get('performanceColor'));

            // draw the target line
            var targetval = values[0],
                x = Math.round(canvas_width * ((targetval - min) / range) - (options.get('targetWidth') / 2)),
                targettop = Math.round(canvas_height * 0.10),
                targetheight = canvas_height - (targettop * 2);
            target.drawRect(x, targettop, options.get('targetWidth') - 1, targetheight - 1, options.get('targetColor'), options.get('targetColor'));
        } else {
            // Remove the tag contents if sparklines aren't supported
            this.innerHTML = '';
        }
    };


    /**
    * Pie charts
    */
    di_jq.fn.sparkline.pie = function (values, options, width, height) {
        values = di_jq.map(values, Number);
        width = options.get('width') == 'auto' ? height : width;

        var target = di_jq(this).simpledraw(width, height, options.get('composite'));
        if (target && values.length > 1) {
            var canvas_width = target.pixel_width,
                canvas_height = target.pixel_height,
                radius = Math.floor(Math.min(canvas_width, canvas_height) / 2),
                total = 0,
                next = 0,
                circle = 2 * Math.PI;

            for (var i = values.length; i--; ) {
                total += values[i];
            }

            if (options.get('offset')) {
                next += (2 * Math.PI) * (options.get('offset') / 360);
            }
            var vlen = values.length;
            for (i = 0; i < vlen; i++) {
                var start = next;
                var end = next;
                if (total > 0) {  // avoid divide by zero
                    end = next + (circle * (values[i] / total));
                }
                target.drawPieSlice(radius, radius, radius, start, end, undefined, options.get('sliceColors')[i % options.get('sliceColors').length]);
                next = end;
            }
        }
    };


    /**
    * Box plots
    */
    var quartile = function (values, q) {
        if (q == 2) {
            var vl2 = Math.floor(values.length / 2);
            return values.length % 2 ? values[vl2] : (values[vl2] + values[vl2 + 1]) / 2;
        } else {
            var vl4 = Math.floor(values.length / 4);
            return values.length % 2 ? (values[vl4 * q] + values[vl4 * q + 1]) / 2 : values[vl4 * q];
        }
    };

    di_jq.fn.sparkline.box = function (values, options, width, height) {
        values = di_jq.map(values, Number);
        width = options.get('width') == 'auto' ? '4.0em' : width;

        var minvalue = options.get('chartRangeMin') === undefined ? Math.min.apply(Math, values) : options.get('chartRangeMin'),
            maxvalue = options.get('chartRangeMax') === undefined ? Math.max.apply(Math, values) : options.get('chartRangeMax'),
            target = di_jq(this).simpledraw(width, height, options.get('composite')),
            vlen = values.length,
            lwhisker, loutlier, q1, q2, q3, rwhisker, routlier;

        if (target && values.length > 1) {
            var canvas_width = target.pixel_width,
                canvas_height = target.pixel_height;
            if (options.get('raw')) {
                if (options.get('showOutliers') && values.length > 5) {
                    loutlier = values[0]; lwhisker = values[1]; q1 = values[2]; q2 = values[3]; q3 = values[4]; rwhisker = values[5]; routlier = values[6];
                } else {
                    lwhisker = values[0]; q1 = values[1]; q2 = values[2]; q3 = values[3]; rwhisker = values[4];
                }
            } else {
                values.sort(function (a, b) { return a - b; });
                q1 = quartile(values, 1);
                q2 = quartile(values, 2);
                q3 = quartile(values, 3);
                var iqr = q3 - q1;
                if (options.get('showOutliers')) {
                    lwhisker = undefined; rwhisker = undefined;
                    for (var i = 0; i < vlen; i++) {
                        if (lwhisker === undefined && values[i] > q1 - (iqr * options.get('outlierIQR'))) {
                            lwhisker = values[i];
                        }
                        if (values[i] < q3 + (iqr * options.get('outlierIQR'))) {
                            rwhisker = values[i];
                        }
                    }
                    loutlier = values[0];
                    routlier = values[vlen - 1];
                } else {
                    lwhisker = values[0];
                    rwhisker = values[vlen - 1];
                }
            }

            var unitsize = canvas_width / (maxvalue - minvalue + 1),
                canvas_left = 0;
            if (options.get('showOutliers')) {
                canvas_left = Math.ceil(options.get('spotRadius'));
                canvas_width -= 2 * Math.ceil(options.get('spotRadius'));
                unitsize = canvas_width / (maxvalue - minvalue + 1);
                if (loutlier < lwhisker) {
                    target.drawCircle((loutlier - minvalue) * unitsize + canvas_left, canvas_height / 2, options.get('spotRadius'), options.get('outlierLineColor'), options.get('outlierFillColor'));
                }
                if (routlier > rwhisker) {
                    target.drawCircle((routlier - minvalue) * unitsize + canvas_left, canvas_height / 2, options.get('spotRadius'), options.get('outlierLineColor'), options.get('outlierFillColor'));
                }
            }

            // box
            target.drawRect(
                Math.round((q1 - minvalue) * unitsize + canvas_left),
                Math.round(canvas_height * 0.1),
                Math.round((q3 - q1) * unitsize),
                Math.round(canvas_height * 0.8),
                options.get('boxLineColor'),
                options.get('boxFillColor'));
            // left whisker
            target.drawLine(
                Math.round((lwhisker - minvalue) * unitsize + canvas_left),
                Math.round(canvas_height / 2),
                Math.round((q1 - minvalue) * unitsize + canvas_left),
                Math.round(canvas_height / 2),
                options.get('lineColor'));
            target.drawLine(
                Math.round((lwhisker - minvalue) * unitsize + canvas_left),
                Math.round(canvas_height / 4),
                Math.round((lwhisker - minvalue) * unitsize + canvas_left),
                Math.round(canvas_height - canvas_height / 4),
                options.get('whiskerColor'));
            // right whisker
            target.drawLine(Math.round((rwhisker - minvalue) * unitsize + canvas_left),
                Math.round(canvas_height / 2),
                Math.round((q3 - minvalue) * unitsize + canvas_left),
                Math.round(canvas_height / 2),
                options.get('lineColor'));
            target.drawLine(
                Math.round((rwhisker - minvalue) * unitsize + canvas_left),
                Math.round(canvas_height / 4),
                Math.round((rwhisker - minvalue) * unitsize + canvas_left),
                Math.round(canvas_height - canvas_height / 4),
                options.get('whiskerColor'));
            // median line
            target.drawLine(
                Math.round((q2 - minvalue) * unitsize + canvas_left),
                Math.round(canvas_height * 0.1),
                Math.round((q2 - minvalue) * unitsize + canvas_left),
                Math.round(canvas_height * 0.9),
                options.get('medianColor'));
            if (options.get('target')) {
                var size = Math.ceil(options.get('spotRadius'));
                target.drawLine(
                    Math.round((options.get('target') - minvalue) * unitsize + canvas_left),
                    Math.round((canvas_height / 2) - size),
                    Math.round((options.get('target') - minvalue) * unitsize + canvas_left),
                    Math.round((canvas_height / 2) + size),
                    options.get('targetColor'));
                target.drawLine(
                    Math.round((options.get('target') - minvalue) * unitsize + canvas_left - size),
                    Math.round(canvas_height / 2),
                    Math.round((options.get('target') - minvalue) * unitsize + canvas_left + size),
                    Math.round(canvas_height / 2),
                    options.get('targetColor'));
            }
        } else {
            // Remove the tag contents if sparklines aren't supported
            this.innerHTML = '';
        }
    };


    // Setup a very simple "virtual canvas" to make drawing the few shapes we need easier
    // This is accessible as di_jq(foo).simpledraw()

    if (di_jq.browser.msie && !document.namespaces.v) {
        document.namespaces.add('v', 'urn:schemas-microsoft-com:vml', '#default#VML');
    }

    if (di_jq.browser.hasCanvas === undefined) {
        var t = document.createElement('canvas');
        di_jq.browser.hasCanvas = t.getContext !== undefined;
    }

    VCanvas_base = function (width, height, target) {
    };

    VCanvas_base.prototype = {
        init: function (width, height, target) {
            this.width = width;
            this.height = height;
            this.target = target;
            if (target[0]) {
                target = target[0];
            }
            target.VCanvas = this;
        },

        drawShape: function (path, lineColor, fillColor, lineWidth) {
            alert('drawShape not implemented');
        },

        drawLine: function (x1, y1, x2, y2, lineColor, lineWidth) {
            return this.drawShape([[x1, y1], [x2, y2]], lineColor, lineWidth);
        },

        drawCircle: function (x, y, radius, lineColor, fillColor) {
            alert('drawCircle not implemented');
        },

        drawPieSlice: function (x, y, radius, startAngle, endAngle, lineColor, fillColor) {
            alert('drawPieSlice not implemented');
        },

        drawRect: function (x, y, width, height, lineColor, fillColor) {
            alert('drawRect not implemented');
        },

        getElement: function () {
            return this.canvas;
        },

        _insert: function (el, target) {
            di_jq(target).html(el);
        }
    };

    VCanvas_canvas = function (width, height, target) {
        return this.init(width, height, target);
    };

    VCanvas_canvas.prototype = di_jq.extend(new VCanvas_base(), {
        _super: VCanvas_base.prototype,

        init: function (width, height, target) {
            this._super.init(width, height, target);
            this.canvas = document.createElement('canvas');
            if (target[0]) {
                target = target[0];
            }
            target.VCanvas = this;
            di_jq(this.canvas).css({ display: 'inline-block', width: width, height: height, verticalAlign: 'center' });
            this._insert(this.canvas, target);
            this.pixel_height = di_jq(this.canvas).height();
            this.pixel_width = di_jq(this.canvas).width();
            this.canvas.width = this.pixel_width;
            this.canvas.height = this.pixel_height;
            di_jq(this.canvas).css({ width: this.pixel_width, height: this.pixel_height });
        },

        _getContext: function (lineColor, fillColor, lineWidth) {
            var context = this.canvas.getContext('2d');
            if (lineColor !== undefined) {
                context.strokeStyle = lineColor;
            }
            context.lineWidth = lineWidth === undefined ? 1 : lineWidth;
            if (fillColor !== undefined) {
                context.fillStyle = fillColor;
            }
            return context;
        },

        drawShape: function (path, lineColor, fillColor, lineWidth) {
            var context = this._getContext(lineColor, fillColor, lineWidth);
            context.beginPath();
            context.moveTo(path[0][0] + 0.5, path[0][1] + 0.5);
            for (var i = 1, plen = path.length; i < plen; i++) {
                context.lineTo(path[i][0] + 0.5, path[i][1] + 0.5); // the 0.5 offset gives us crisp pixel-width lines
            }
            if (lineColor !== undefined) {
                context.stroke();
            }
            if (fillColor !== undefined) {
                context.fill();
            }
        },

        drawCircle: function (x, y, radius, lineColor, fillColor) {
            var context = this._getContext(lineColor, fillColor);
            context.beginPath();
            context.arc(x, y, radius, 0, 2 * Math.PI, false);
            if (lineColor !== undefined) {
                context.stroke();
            }
            if (fillColor !== undefined) {
                context.fill();
            }
        },

        drawPieSlice: function (x, y, radius, startAngle, endAngle, lineColor, fillColor) {
            var context = this._getContext(lineColor, fillColor);
            context.beginPath();
            context.moveTo(x, y);
            context.arc(x, y, radius, startAngle, endAngle, false);
            context.lineTo(x, y);
            context.closePath();
            if (lineColor !== undefined) {
                context.stroke();
            }
            if (fillColor) {
                context.fill();
            }
        },

        drawRect: function (x, y, width, height, lineColor, fillColor) {
            return this.drawShape([[x, y], [x + width, y], [x + width, y + height], [x, y + height], [x, y]], lineColor, fillColor);
        }

    });

    VCanvas_vml = function (width, height, target) {
        return this.init(width, height, target);
    };

    VCanvas_vml.prototype = di_jq.extend(new VCanvas_base(), {
        _super: VCanvas_base.prototype,

        init: function (width, height, target) {
            this._super.init(width, height, target);
            if (target[0]) {
                target = target[0];
            }
            target.VCanvas = this;
            this.canvas = document.createElement('span');
            di_jq(this.canvas).css({ display: 'inline-block', position: 'relative', overflow: 'hidden', width: width, height: height, margin: '0px', padding: '0px', verticalAlign: 'top' });
            this._insert(this.canvas, target);
            this.pixel_height = di_jq(this.canvas).height();
            this.pixel_width = di_jq(this.canvas).width();
            this.canvas.width = this.pixel_width;
            this.canvas.height = this.pixel_height;
            var groupel = '<v:group coordorigin="0 0" coordsize="' + this.pixel_width + ' ' + this.pixel_height + '"' +
                    ' style="position:absolute;top:0;left:0;width:' + this.pixel_width + 'px;height=' + this.pixel_height + 'px;"></v:group>';
            this.canvas.insertAdjacentHTML('beforeEnd', groupel);
            this.group = di_jq(this.canvas).children()[0];
        },

        drawShape: function (path, lineColor, fillColor, lineWidth) {
            var vpath = [];
            for (var i = 0, plen = path.length; i < plen; i++) {
                vpath[i] = '' + (path[i][0]) + ',' + (path[i][1]);
            }
            var initial = vpath.splice(0, 1);
            lineWidth = lineWidth === undefined ? 1 : lineWidth;
            var stroke = lineColor === undefined ? ' stroked="false" ' : ' strokeWeight="' + lineWidth + '" strokeColor="' + lineColor + '" ';
            var fill = fillColor === undefined ? ' filled="false"' : ' fillColor="' + fillColor + '" filled="true" ';
            var closed = vpath[0] == vpath[vpath.length - 1] ? 'x ' : '';
            var vel = '<v:shape coordorigin="0 0" coordsize="' + this.pixel_width + ' ' + this.pixel_height + '" ' +
                 stroke +
                 fill +
                ' style="position:absolute;left:0px;top:0px;height:' + this.pixel_height + 'px;width:' + this.pixel_width + 'px;padding:0px;margin:0px;" ' +
                ' path="m ' + initial + ' l ' + vpath.join(', ') + ' ' + closed + 'e">' +
                ' </v:shape>';
            this.group.insertAdjacentHTML('beforeEnd', vel);
        },

        drawCircle: function (x, y, radius, lineColor, fillColor) {
            x -= radius + 1;
            y -= radius + 1;
            var stroke = lineColor === undefined ? ' stroked="false" ' : ' strokeWeight="1" strokeColor="' + lineColor + '" ';
            var fill = fillColor === undefined ? ' filled="false"' : ' fillColor="' + fillColor + '" filled="true" ';
            var vel = '<v:oval ' +
                stroke +
                fill +
                ' style="position:absolute;top:' + y + 'px; left:' + x + 'px; width:' + (radius * 2) + 'px; height:' + (radius * 2) + 'px"></v:oval>';
            this.group.insertAdjacentHTML('beforeEnd', vel);

        },

        drawPieSlice: function (x, y, radius, startAngle, endAngle, lineColor, fillColor) {
            if (startAngle == endAngle) {
                return;  // VML seems to have problem when start angle equals end angle.
            }
            if ((endAngle - startAngle) == (2 * Math.PI)) {
                startAngle = 0.0;  // VML seems to have a problem when drawing a full circle that doesn't start 0
                endAngle = (2 * Math.PI);
            }

            var startx = x + Math.round(Math.cos(startAngle) * radius);
            var starty = y + Math.round(Math.sin(startAngle) * radius);
            var endx = x + Math.round(Math.cos(endAngle) * radius);
            var endy = y + Math.round(Math.sin(endAngle) * radius);

            // Prevent very small slices from being mistaken as a whole pie
            if (startx == endx && starty == endy && (endAngle - startAngle) < Math.PI) {
                return;
            }

            var vpath = [x - radius, y - radius, x + radius, y + radius, startx, starty, endx, endy];
            var stroke = lineColor === undefined ? ' stroked="false" ' : ' strokeWeight="1" strokeColor="' + lineColor + '" ';
            var fill = fillColor === undefined ? ' filled="false"' : ' fillColor="' + fillColor + '" filled="true" ';
            var vel = '<v:shape coordorigin="0 0" coordsize="' + this.pixel_width + ' ' + this.pixel_height + '" ' +
                 stroke +
                 fill +
                ' style="position:absolute;left:0px;top:0px;height:' + this.pixel_height + 'px;width:' + this.pixel_width + 'px;padding:0px;margin:0px;" ' +
                ' path="m ' + x + ',' + y + ' wa ' + vpath.join(', ') + ' x e">' +
                ' </v:shape>';
            this.group.insertAdjacentHTML('beforeEnd', vel);
        },

        drawRect: function (x, y, width, height, lineColor, fillColor) {
            return this.drawShape([[x, y], [x, y + height], [x + width, y + height], [x + width, y], [x, y]], lineColor, fillColor);
        }
    });

})(jQuery);


/*!
* jQCloud Plugin for jQuery
*
* Version 0.2.4
*
* Copyright 2011, Luca Ongaro
* Licensed under the MIT license.
*
* Date: Sun Aug 14 00:09:07 +0200 2011
*/

(function (di_jq) {
    di_jq.fn.jQCloud = function (word_array, options) {
        // Reference to the container element
        var di_jqthis = this;
        // Reference to the ID of the container element
        var container_id = di_jqthis.attr('id');

        var divCloud = z(container_id);
        // Default options value
        var default_options = {
            width: di_jqthis.width(),
            height: di_jqthis.height(),
            center: {
                x: di_jqthis.width() / 2,
                y: di_jqthis.height() / 2
            },
            delayedMode: word_array.length > 50,
            randomClasses: 0
        };

        // Maintain backward compatibility with old API (pre 0.2.0), where the second argument of jQCloud was a callback function
        if (typeof options === 'function') {
            options = { callback: options }
        }

        options = di_jq.extend(default_options, options || {});

        // Add the "jqcloud" class to the container for easy CSS styling
        di_jqthis.addClass("jqcloud");

        var drawWordCloud = function () {
            // Helper function to test if an element overlaps others
            var hitTest = function (elem, other_elems) {
                // Pairwise overlap detection
                var overlapping = function (a, b) {
                    if (Math.abs(2.0 * a.offsetLeft + a.offsetWidth - 2.0 * b.offsetLeft - b.offsetWidth) < a.offsetWidth + b.offsetWidth) {
                        if (Math.abs(2.0 * a.offsetTop + a.offsetHeight - 2.0 * b.offsetTop - b.offsetHeight) < a.offsetHeight + b.offsetHeight) {
                            return true;
                        }
                    }
                    return false;
                };
                var i = 0;
                // Check elements for overlap one by one, stop and return false as soon as an overlap is found
                for (i = 0; i < other_elems.length; i++) {
                    if (overlapping(elem, other_elems[i])) {
                        return true;
                    }
                }
                return false;
            };

            // Make sure every weight is a number before sorting
            for (i = 0; i < word_array.length; i++) {
                word_array[i].weight = parseFloat(word_array[i].weight, 10);
            }

            // Sort word_array from the word with the highest weight to the one with the lowest
            word_array.sort(function (a, b) { if (a.weight < b.weight) { return 1; } else if (a.weight > b.weight) { return -1; } else { return 0; } });

            var step = 2.0;
            var already_placed_words = [];
            var aspect_ratio = options.width / options.height;

            // Function to draw a word, by moving it in spiral until it finds a suitable empty place. This will be iterated on each word.
            var drawOneWord = function (index, word) {
                // Define the ID attribute of the span that will wrap the word, and the associated jQuery selector string
                var word_id = container_id + "_word_" + index;
                var word_selector = "#" + word_id;

                // If the option randomClasses is a number, and higher than 0, assign this word randomly to a class
                // of the kind 'r1', 'r2', 'rN' with N = randomClasses
                // If option randomClasses is an array, assign this word randomly to one of the classes in the array
                var random_class = (typeof options.randomClasses === "number" && options.randomClasses > 0)
          ? " r" + Math.ceil(Math.random() * options.randomClasses)
          : ((di_jq.isArray(options.randomClasses) && options.randomClasses.length > 0)
            ? " " + options.randomClasses[Math.floor(Math.random() * options.randomClasses.length)]
            : "");

                var angle = 6.28 * Math.random();
                var radius = 0.0;

                // Linearly map the original weight to a discrete scale from 1 to 10
                var weight = Math.round((word.weight - word_array[word_array.length - 1].weight) / (word_array[0].weight - word_array[word_array.length - 1].weight) * 9.0) + 1;

                var inner_html = word.url !== undefined ? "<a href='" + encodeURI(word.url).replace(/'/g, "%27") + "'>" + word.text + "</a>" : word.text;
                di_jqthis.append("<span id='" + word_id + "' class='w" + weight + random_class + "' title='" + (word.title || "") + "'>" + inner_html + "</span>&nbsp;&nbsp;");

                // Search for the word span only once, and within the context of the container, for better performance
                var word_span = di_jq(word_selector, di_jqthis);

                var width = word_span.width();
                var height = word_span.height();
                var left = options.center.x - width / 2.0;
                var top = options.center.y - height / 2.0;

                // Save a reference to the style property, for better performance
                var word_style = word_span[0].style;
                word_style.position = "absolute";
                word_style.left = left + "px";
                word_style.top = top + "px";

                while (hitTest(z(word_id), already_placed_words)) {
                    radius += step;
                    angle += (index % 2 === 0 ? 1 : -1) * step;

                    left = options.center.x - (width / 2.0) + (radius * Math.cos(angle)) * aspect_ratio;
                    top = options.center.y + radius * Math.sin(angle) - (height / 2.0);

                    word_style.left = left + "px";
                    word_style.top = top + "px";
                }
                already_placed_words.push(z(word_id));
            }

            var drawOneWordDelayed = function (index) {
                index = index || 0;
                if (index < word_array.length) {
                    drawOneWord(index, word_array[index]);
                    setTimeout(function () { drawOneWordDelayed(index + 1); }, 10);
                } else {
                    if (typeof options.callback === 'function') {
                        options.callback.call(this);
                    }
                }
            }

            // Iterate drawOneWord on every word. The way the iteration is done depends on the drawing mode (delayedMode is true or false)
            if (options.delayedMode || options.delayed_mode) {
                drawOneWordDelayed();
            }
            else {
                di_jq.each(word_array, drawOneWord);
                if (typeof options.callback === 'function') {
                    options.callback.call(this);
                }
            }
        };

        // Delay execution so that the browser can render the page before the computatively intensive word cloud drawing
        setTimeout(function () { drawWordCloud(); }, 10);
        return this;
    };
})(jQuery);

var AreaID_LevelKeyValuePair = [];

function SaveAreaId_LevelPair(QdsSelectedAreasString) {
    AreaID_LevelKeyValuePair = [];
    AreaID_LevelKeyValuePair = QdsSelectedAreasString.clone();
}

function GetExplodedResults(DBNId, IndicatorNId, UnitNId, AreaID, IdUniquePart) {

    var spanLoading, spanLoadingOuter, divQDSResults, spanTimeCounter;
    var InputParam = '';

    spanLoading = z('spanLoading');
    spanLoadingOuter = z('spanLoadingOuter');
    divQDSResults = z('divExplodedResults_' + IdUniquePart);
    spanTimeCounter = z('spanTimeCounter');

    if (divQDSResults.style.display != "none") {
        di_jq('#' + divQDSResults.id).hide('slow');
        return;
    }
    else {
        di_jq('#' + divQDSResults.id).show('slow');
        if (divQDSResults.innerHTML != "") return;
    }

    InputParam = IndicatorNId;
    InputParam += ParamDelimiter + '';
    InputParam += ParamDelimiter + AreaID;
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    InputParam += ParamDelimiter + DBNId;
    InputParam += ParamDelimiter + 'true';

    StopGalleryLoadingProcess = true;
    spanLoadingOuter.style.display = "inline";
//    spanLoading.innerHTML = "Searching the database...";
    spanLoading.innerHTML = z('langSearchingDatabase').value;
    divQDSResults.innerHTML = "";
    z('divDatabaseFullInfo').style.display = 'none';
    spanTimeCounter.innerHTML = '';

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '4', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {

                    var TimeTaken = data.split(ParamDelimiter)[0];
                    var ResultsData = data.split(ParamDelimiter)[1];

                    var jsonData = di_jq.parseJSON(ResultsData);

                    RenderTimeTaken(jsonData[""].length, TimeTaken);

                    RenderResults(jsonData, divQDSResults.id, spanLoadingOuter.id, spanLoading.id, true, null, "","");

                }
                catch (ex) {
                    RenderTimeTaken(0, 0);
                    divQDSResults.innerHTML = "";
                    spanLoadingOuter.style.display = "none";
                    spanLoading.innerHTML = "";
                }
            },
            error: function () {
//                
            },
            cache: false
        });
    }
    catch (ex) {
    }

}


function GetExplodedResultsTable(DBNId, IndicatorNId, UnitNId, AreaID, IdUniquePart, AdaptationURL) {

    var spanLoading, spanLoadingOuter, divQDSResults, spanTimeCounter, anchorLoader;
    var InputParam = '';

    anchorLoader = z('ExpandedLoader_' + IdUniquePart);
    spanLoading = z('spanLoading');
    spanLoadingOuter = z('spanLoadingOuter');
    divQDSResults = z('divExplodedResults_' + IdUniquePart);
    spanTimeCounter = z('spanTimeCounter');

    if (divQDSResults.style.display != "none") {
        di_jq('#' + divQDSResults.id).hide('slow');
        return;
    }
    else {
        di_jq('#' + divQDSResults.id).show('slow');
        if (divQDSResults.innerHTML != "") {
            anchorLoader.className = 'qds_icon6 ds_block_txt';
            return;
        }
    }

    InputParam = IndicatorNId;
    InputParam += ParamDelimiter + '';
    InputParam += ParamDelimiter + AreaID;
    InputParam += ParamDelimiter + z('hlngcodedb').value;
    InputParam += ParamDelimiter + DBNId;
    InputParam += ParamDelimiter + 'true';
    InputParam += ParamDelimiter + AdaptationURL;

    StopGalleryLoadingProcess = true;
    //spanLoadingOuter.style.display = "inline";
    if (spanLoading != null) 
//    spanLoading.innerHTML = "Searching the database...";
    spanLoading.innerHTML = z('langSearchingDatabase').value;
    divQDSResults.innerHTML = "";
    
    if (z('hdsby').value == 'share') CallBackPageName = "../../../libraries/aspx/Callback.aspx";

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '4', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {

                    var TimeTaken = data.split(ParamDelimiter)[0];
                    var ResultsData = data.split(ParamDelimiter)[1];

                    anchorLoader.className = 'qds_icon6 ds_block_txt';

                    var jsonData = di_jq.parseJSON(ResultsData);

                    //RenderTimeTaken(jsonData[""].length, TimeTaken);

                    var jsonFilteredForTable = getFilteredJson(jsonData, UnitNId);

                    RenderExplodedTabularResults(jsonFilteredForTable, divQDSResults.id, AdaptationURL);

                    di_jq('.sparkline').sparkline('html', { type: 'bar' });

                    if(spanLoadingOuter != null) spanLoadingOuter.style.display = "none";
                    if(spanLoading != null) spanLoading.innerHTML = "";

//                    StopGalleryLoadingProcess = false;

//                    if (isSearchResultsGalleryEnabled == true) GetGalleryThumbnails(0, 1);
                }
                catch (ex) {
                    //RenderTimeTaken(0, 0);
                    //divQDSResults.innerHTML = "";
                    //spanLoadingOuter.style.display = "none";
                    //spanLoading.innerHTML = "";
                }
            },
            error: function () {
//                
            },
            cache: false
        });
    }
    catch (ex) {
    }

}

function getFilteredJson(jsonFullData, UnitNId) {

    var childObj = [];   
     
    for (var i = 0; i < jsonFullData[""].length; i++) {
        if (jsonFullData[""][i].UnitNId == UnitNId) {
            childObj.push(jsonFullData[""][i]);                        
          }
    }

    jsonFullData[""] = [];

    for (var i = 0; i < childObj.length; i++) {
        jsonFullData[""].push(childObj[i]);
    }
    return jsonFullData;

}

function RenderExplodedTabularResults(jsonData, divQDSResults, AdaptationUrl) {
    var tmpExplodedBody = ce('tbody');
    var tableResults = ce('table');
    tableResults.style.marginLeft = '35px';
    tableResults.style.marginTop = '15px';
    var defaultSubgroup = "";
    var All_IDs = '';

    for (var i = 0; i < jsonData[""].length; i++) {

        var jsonResult = jsonData[""][i];
        var tmpDvSeries = sortDvSeries(jsonResult.DVSeries, jsonResult.IsAreaNumeric);
        var IdUniquePart = getIdUniquePart(jsonResult);

        All_IDs += IdUniquePart + '#';

        var tmpTR = ce('tr');
        tmpTR.style.height = '25px';
        tmpTR.id = 'DIUA_' + IdUniquePart;

        var tmpTdName = ce('td');
        tmpTdName.className = 'ds_block_sub_tbl_txt';
        tmpTdName.innerHTML = jsonResult.Area;

        tmpTR.appendChild(tmpTdName);

        var tmpTdDataValue = ce('td');
        tmpTdDataValue.align = 'right';

        var tmpSpanDataValue = document.createElement('span');
        tmpSpanDataValue.className = 'heading2 ds_red_txt';
        tmpSpanDataValue.innerHTML = jsonResult.MRD;
        if (jsonResult.DefaultSG != '') {
            defaultSubgroup = jsonResult.DefaultSG.split("[@@@@]")[1];
        }

        tmpSpanDataValue.onmouseover = function (event) {
            ShowCallout('divCallout', z('langMRDFor').value + "<br>" + defaultSubgroup, event);
        };
        tmpSpanDataValue.onmouseout = function (event) {
            HideCallout('divCallout');
        };

        tmpTdDataValue.appendChild(tmpSpanDataValue);

        tmpTR.appendChild(tmpTdDataValue);

        var tmpTdDvTP = document.createElement('td');

        var tmpSpanDvTP = document.createElement('span');
        tmpSpanDvTP.className = 'content ds_red_yr_txt';
        tmpSpanDvTP.innerHTML = '(' + jsonResult.MRDTP + ')';
        if (jsonResult.DefaultSG != '') {
            defaultSubgroup = jsonResult.DefaultSG.split("[@@@@]")[1];
        }

        tmpSpanDvTP.onmouseover = function (event) {
            ShowCallout('divCallout', z('langMRDFor').value + "<br>" + defaultSubgroup, event);
        };
        tmpSpanDvTP.onmouseout = function (event) {
            HideCallout('divCallout');
        };
        tmpTdDvTP.appendChild(tmpSpanDvTP);

        tmpTR.appendChild(tmpTdDvTP);

        var tmpTdChart = ce('td');

        var tmpDivJsonData = ce('div');
        tmpDivJsonData.id = "JsonData_" + IdUniquePart;
        tmpDivJsonData.style.display = 'none';
        tmpDivJsonData.innerHTML = JSON.stringify(jsonResult);

        tmpTdChart.appendChild(tmpDivJsonData);

        var tmpDivVisualizer = ce('div');
        tmpDivVisualizer.id = 'Visualizer_' + IdUniquePart;
        tmpDivVisualizer.className = 'ds_grph_contnr';

        var tmpDefaultSG = jsonResult.DefaultSG.split('[@@@@]')[1];

        tmpDivVisualizer.innerHTML = Get_Visualizer_Creation_Data(tmpDefaultSG, tmpDvSeries.split(':')[0], tmpDvSeries.split(':')[1]);

        var tmpDvSeriesVals = tmpDvSeries.split(':')[1];

        var spanSparkLine = ce('span');
        spanSparkLine.className = 'sparkline';
        spanSparkLine.style.marginLeft = '10px';
        spanSparkLine.setAttribute('values', tmpDvSeriesVals);
        spanSparkLine.title = get_DvSeries_Min_Max(jsonResult.DVSeries);

        if (tmpDvSeriesVals.indexOf(',') != -1) tmpTdChart.appendChild(spanSparkLine);

        // Don't render HighCharts in tabular visualizations
        //tmpTdChart.appendChild(tmpDivVisualizer);

        tmpTR.appendChild(tmpTdChart);

        var tmpTdAddToCart = ce('td');

        
            create_AddToCart_Visualize_Share_Buttons(jsonResult, tmpTdAddToCart, false, AdaptationUrl);
        

        tmpTR.appendChild(tmpTdAddToCart);
        tmpExplodedBody.appendChild(tmpTR);
        tableResults.appendChild(tmpExplodedBody);

        //        tableResults.appendChild(tmpTR);
    }

    z(divQDSResults).appendChild(tableResults);

    //Draw chart for each row in table

    //RenderManyCharts(All_IDs,0);
    
}

function RenderManyCharts(IdUniqueParts, nowPointer) {

    var arrIDs = IdUniqueParts.split('#');

    var nowID = arrIDs[nowPointer];

    RenderChart(nowID);
    nowPointer += 1;

    if(nowPointer < arrIDs.length - 1) setTimeout('RenderManyCharts("' + IdUniqueParts + '",' + nowPointer + ')', 20);    
    
}

function create_AddToCart_Visualize_Share_Buttons(jsonResult, divResult, isAddShareSpan, AdaptationUrl)  {
     // isBlock Issue : Change in the name of parameter to avoid confusion.
    var IdUniquePart = getIdUniquePart(jsonResult);

    if (!isCatalogResults) {
        if (IsShowElements == true || IsShowElements == undefined || IsShowElements == null) {
            var tmpSpanAddToCart = ce('span');
            //tmpSpanAddToCart.style.display = 'block';
            tmpSpanAddToCart.id = 'ATDV_' + IdUniquePart;
            tmpSpanAddToCart.className = 'qds_icon2 ds_add_vis_shr_spc';
            tmpSpanAddToCart.title = z('langResultsAddToCart').value; // lang(en):  'Add to Cart';

            var aAddToCart = ce('a');
            aAddToCart.className = 'ds_add_vis_shr_txt_spc';
            if (divResult.id != "") {
                aAddToCart.innerHTML = z('langResultsAddToCart').value; // lang(en):  'Add to Cart';
                tmpSpanAddToCart.isExploded = 'false';
            }
            else {
                aAddToCart.innerHTML = '&nbsp;';
                tmpSpanAddToCart.isExploded = 'true';
            }
            aAddToCart.href = 'javascript:void(0);';
            

            tmpSpanAddToCart.appendChild(aAddToCart);

            tmpSpanAddToCart.onclick = function (event) {
                AddtoDataView(IdUniquePart, z('hdbnid').value, false);     // isBlock Issue : Do not pass isBlock parameter
            }

            if (z('hdsby').value != "share") divResult.appendChild(tmpSpanAddToCart);
        }
        var tmpSpanVisualize = ce('span');
        //tmpSpanVisualize.style.display = 'block';
        tmpSpanVisualize.className = 'qds_icon3 ds_add_vis_shr_spc';
        tmpSpanVisualize.title = z('langResultsVisualize').value; // lang(en): 'Visualize';

        var tmpA_Visualize = ce('a');
        tmpA_Visualize.className = 'ds_add_vis_shr_txt_spc';
        if (divResult.id != "") tmpA_Visualize.innerHTML = z('langResultsVisualize').value; // lang(en): 'Visualize';
        else tmpA_Visualize.innerHTML = '&nbsp;';
        tmpA_Visualize.href = 'javascript:void(0);';

        tmpSpanVisualize.appendChild(tmpA_Visualize);
        //tmpSpanVisualize.attr("target", "_blank");

        tmpSpanVisualize.onclick = function (event) {
            PostToDataViewFromVisualize(IdUniquePart, z('hdbnid').value); // isBlock Issue : Do not pass isBlock parameter
        }

        divResult.appendChild(tmpSpanVisualize);

    }

    if (IsShowElements == true || IsShowElements == undefined || IsShowElements == null) {
        var tmpSpanShare = ce('span');
        //tmpSpanShare.style.display = 'block';
        tmpSpanShare.title = z('langResultsShare').value; // lang(en) : 'Share'
        tmpSpanShare.className = 'qds_icon4 ds_add_vis_shr_spc';

        var tmpA_share = ce('a');
        tmpA_share.className = 'ds_add_vis_shr_txt_spc';
        if (divResult.id != "") tmpA_share.innerHTML = z('langResultsShare').value; // lang(en) : 'Share'
        else tmpA_share.innerHTML = '&nbsp;';
        tmpA_share.href = 'javascript:void(0);';

        tmpSpanShare.appendChild(tmpA_share);


        tmpSpanShare.onclick = function (event) {
            ShareResult(IdUniquePart, AdaptationUrl); // isBlock Issue : Do not pass isBlock parameter
        }

        if (z('hdsby').value != "share" && !isAddShareSpan) divResult.appendChild(tmpSpanShare);  // isBlock Issue : Changed the name from siBlock to isAddShareSpan
        // Added to resolve expansion add to cart bug
        var anchorViewData;
        var arrCartNow = [];
        var strCartCookie = readCookie('QdsCart');
        if (strCartCookie != null && strCartCookie != '') {
            arrCartNow = strCartCookie.split(',');
            for (var i = 0; i < arrCartNow.length; i++) {
                if (arrCartNow[i] == IdUniquePart)
                    tmpSpanAddToCart.style.display = 'none';
            }
        }
    }

}
//Added last parameter for Simul Gallery home page
function RenderResult(jsonResult, divForResults, isExploded, AdaptationUrl, GalleryXML_Singl) {


    if ((jsonResult.AreaNId.indexOf('BL_') == 0) || (jsonResult.AreaNId.indexOf('QS_') == 0)) {
        RenderQsResult(jsonResult, divForResults, isExploded, AdaptationUrl, GalleryXML_Singl);
        //Added last parameter for Simul Gallery Home Page
        return;
    }

    var divQDSResults = z(divForResults); //  z('divQDSResults');
    var ResultRow = jsonResult;
    var RetVal = ''; // Stores Markup HTML for that Result's div
    var ChartType = 'line';
    var DBNId = z('hdbnid').value;

    var strJsonData = JSON.stringify(jsonResult);
    var IdUniquePart = getIdUniquePart(jsonResult);
    var tmpDvSeries = sortDvSeries(jsonResult.DVSeries, jsonResult.IsAreaNumeric);

    var tmpDivQdsResult = document.createElement('div');
    tmpDivQdsResult.id = 'DIUA_' + IdUniquePart;
    tmpDivQdsResult.className = 'qds_result_cont';

    var tmpBody = ce('tbody');
    
    var tmpTableQdsResultChart = ce('table');
    //tmpTableQdsResultChart.border = '1px';
    tmpTableQdsResultChart.style.width = '100%';

    var tmpTrQdsResultChart = ce('tr');

    var TdLeft = ce('td');

    var tmpTdLeft = ce('span');
    tmpTdLeft.className = 'flt_lft';


    //#> Left Content          


    var tmpSpanAreaInd = document.createElement('span');
    tmpSpanAreaInd.className = 'ds_main_hdn';
    tmpSpanAreaInd.innerHTML = jsonResult.Area + ' - ' + jsonResult.Indicator;

    if (IsShowElements==true) {
        if (!isCatalogResults) {
            tmpSpanAreaInd.style.cursor = "pointer";
            tmpSpanAreaInd.onclick = function (event) {
                PostToDataViewFromVisualize(IdUniquePart, z('hdbnid').value); // isBlock Issue : Do not pass isBlock parameter
            }
        }
    }

    tmpTdLeft.appendChild(tmpSpanAreaInd);

    var tmpSpanUnit = document.createElement('span');
    tmpSpanUnit.className = 'ds_sub_hdn';
    tmpSpanUnit.innerHTML = ', ' + jsonResult.Unit;

    tmpTdLeft.appendChild(tmpSpanUnit);

    if (jsonResult.DefaultSG != '' && jsonResult.MRD != '' && jsonResult.MRDTP != '') {

        var tmpDataValue;
        var defaultSubgroup;
        var NumericMRD = parseFloat(jsonResult.MRD);

        if (!isNaN(NumericMRD)) {
            //tmpDataValue = CommaFormatted(NumericMRD.toString());//commented to resolve decimal point bug
            tmpDataValue = CommaFormatted(jsonResult.MRD);

        }
        else {
            tmpDataValue = jsonResult.MRD;
        }

        var tmpSpanDataValue = document.createElement('span');
        tmpSpanDataValue.className = 'heading2 ds_red_txt';
        tmpSpanDataValue.innerHTML = tmpDataValue;
       
        var tmpSpanDvTP = document.createElement('span');
        tmpSpanDvTP.className = 'content ds_red_yr_txt';
        tmpSpanDvTP.innerHTML = '(' + jsonResult.MRDTP + ')';

        tmpSpanDataValue.appendChild(tmpSpanDvTP);

        tmpTdLeft.appendChild(tmpSpanDataValue);
        if (jsonResult.DefaultSG != '') {
            defaultSubgroup = jsonResult.DefaultSG.split("[@@@@]")[1];
        }

        tmpSpanDataValue.onmouseover = function (event) {
            ShowCallout('divCallout', z('langMRDFor').value + "<br>" + defaultSubgroup, event);
        };
        tmpSpanDataValue.onmouseout = function (event) {
            HideCallout('divCallout');
        };
    }

    var tmpDvSeriesVals = tmpDvSeries.split(':')[1];
    if (jsonResult.AreaNIds == "") {
        var spanSparkLine = ce('span');
        spanSparkLine.className = 'sparkline';
        spanSparkLine.style.marginLeft = '10px';
        spanSparkLine.setAttribute('values', tmpDvSeriesVals);
        spanSparkLine.title = get_DvSeries_Min_Max(jsonResult.DVSeries);

        if (tmpDvSeriesVals.indexOf(',') != -1) tmpTdLeft.appendChild(spanSparkLine);
    }

    tmpTdLeft.appendChild(ce('br'));

    var tmpI_IndDesc = ce('i');
    tmpI_IndDesc.className = 'content ds_itl_txt';

    var shortIndicatorDesc = jsonResult.IndicatorDescription;

    if (jsonResult.IndicatorDescription.length > 100) {

        var FullIndDesc = ReplaceAll(jsonResult.IndicatorDescription, '\"', '&quot;');
        tmpI_IndDesc.onmouseover = function (event) {
            ShowCallout('divCallout', FullIndDesc, event);
        };
        tmpI_IndDesc.onmouseout = function (event) {
            HideCallout('divCallout');
        };

        shortIndicatorDesc = jsonResult.IndicatorDescription.substring(0, 100) + "...";
    }

    tmpI_IndDesc.innerHTML = shortIndicatorDesc;

    tmpTdLeft.appendChild(tmpI_IndDesc);
    tmpTdLeft.appendChild(ce('br'));

    if (jsonResult.IC_Name != '') {
        var tmpSpanIC = ce('span');
        tmpSpanIC.style.lineHeight = '20px';
        tmpSpanIC.style.color = '#999999';
        tmpSpanIC.className = 'content';
        tmpSpanIC.innerHTML = 'Searched by : ' + jsonResult.IC_Name;

        tmpTdLeft.appendChild(tmpSpanIC);
    }
    //#<

    var tmpDivVisualizer = ce('div');
    tmpDivVisualizer.id = 'Visualizer_' + IdUniquePart;
    tmpDivVisualizer.className = 'ds_grph_contnr';

    var tmpDefaultSG = jsonResult.DefaultSG.split('[@@@@]')[1];

    tmpDivVisualizer.innerHTML = Get_Visualizer_Creation_Data(tmpDefaultSG, tmpDvSeries.split(':')[0], tmpDvSeries.split(':')[1]);


    TdLeft.appendChild(tmpTdLeft);
    // Remove HighCharts Visualizations from QDS search results
    //TdLeft.appendChild(tmpDivVisualizer);    

    tmpTrQdsResultChart.appendChild(TdLeft);

    //tmpTableQdsResultChart.appendChild(tmpTrQdsResultChart);
    tmpBody.appendChild(tmpTrQdsResultChart);
    tmpTableQdsResultChart.appendChild(tmpBody);
    tmpDivQdsResult.appendChild(tmpTableQdsResultChart);

    //#region Display dimensions

    //Display dimensions

    if (jsonResult.Dimensions != null) {

        var AllDimensions = jsonResult.Dimensions.split(',');

        var tmpSpanDimensions = ce('span');
        tmpSpanDimensions.innerHTML = AllDimensions.length + ' ' + z('langResultsDimensions').value + ' | '; // lang(en):  ' Dimensions | ';
        if (!isCatalogResults) {
            tmpSpanDimensions.style.cursor = 'pointer';
        }
        tmpSpanDimensions.id = "tmpSpanDimensions";
        tmpSpanDimensions.onclick = function (event) {
            //#region Show Dimensions pop-up
            var divDimesions = z('divDimensionsList');

            LiDimensionsArr = [];
            while (divDimesions.hasChildNodes()) {
                divDimesions.removeChild(divDimesions.lastChild);
            }

            for (var d = 0; d < AllDimensions.length; d++) {
                var liDimension = ce('li');
                var LiId = "li_" + AllDimensions[d];
                liDimension.id = LiId;
                LiDimensionsArr.push(LiId);

                if (!isCatalogResults) {
                    var aDimension;
                    if (IsShowElements == true || IsShowElements == undefined || IsShowElements == null) {
                    aDimension = ce('a');
                    aDimension.innerHTML = AllDimensions[d];
                    aDimension.rel = AllDimensions[d];
                        aDimension.href = 'javascript:void(0);';
                        aDimension.onclick = function (event) {
                            SelectedDimension = this.rel;
                            PostToDataView(IdUniquePart, z('hdbnid').value);    //isBlock Issue : Removed parameter false used as isBlock 
                        }
                    }
                    else {
                        aDimension = ce('span');
                        aDimension.innerHTML = AllDimensions[d];
                        aDimension.rel = AllDimensions[d];
                    }


                    liDimension.appendChild(aDimension);
                }
                else liDimension.innerHTML = AllDimensions[d];

                divDimesions.appendChild(liDimension);
            }

            SetCalloutPosition(event, z('divCallout_qds'), 20, 0);
            //            if (z('divCallout_qds').style.display == 'block') {
            //                z('divCallout_qds').style.display = 'none';
            //            }
            //            else {
            //                z('divCallout_qds').style.display = 'block';
            //            }

            //#endregion
        }

        tmpDivQdsResult.appendChild(tmpSpanDimensions);
    }

    //#endregion


    var tmpSpanDV_count = ce('span');
    tmpSpanDV_count.innerHTML = jsonResult.DVCount + ' ' + z('langResultsDataValues').value; // lang(en): ' data values | ';

    tmpDivQdsResult.appendChild(tmpSpanDV_count);

    
        create_AddToCart_Visualize_Share_Buttons(jsonResult, tmpDivQdsResult, false, AdaptationUrl);
    

    tmpDivQdsResult.appendChild(ce('br'));

    if (jsonResult.jsonBlock != null) {

        var expJsonBlock = jsonResult.jsonBlock;

        var tmpDivBreak = ce('div');
        tmpDivBreak.className = 'navls_top_pg';
        tmpDivQdsResult.appendChild(tmpDivBreak);
        //tmpDivQdsResult.appendChild(ce('br'));

        var tmpA_AreaBlockExpanded = ce('a');
        //if (z('hdsby').value != "share") {
        tmpA_AreaBlockExpanded.id = "ExpandedLoader_" + getIdUniquePart(expJsonBlock);
        tmpA_AreaBlockExpanded.className = 'qds_icon5 ds_block_txt';
        tmpA_AreaBlockExpanded.href = 'javascript:void(0);';
        tmpA_AreaBlockExpanded.onclick = function (event) {

            if (this.className == 'qds_icon5 ds_block_txt') this.className = 'qds_icon9 ds_block_txt';
            else this.className = 'qds_icon5 ds_block_txt';

            var nowAdaptationUrl = '';
            if (AdaptationUrl != null) nowAdaptationUrl = AdaptationUrl;

            GetExplodedResultsTable(z('hdbnid').value, expJsonBlock.IndicatorNId, expJsonBlock.UnitNId, expJsonBlock.AreaNIds, getIdUniquePart(expJsonBlock), AdaptationUrl);

        }
        //}       

        tmpA_AreaBlockExpanded.innerHTML = expJsonBlock.Area + ' ' + z('langAreas').value; // lang(en):  ' areas';

        tmpDivQdsResult.appendChild(tmpA_AreaBlockExpanded);

        
            var tmpPipeSeparator = document.createTextNode(' |');
            tmpDivQdsResult.appendChild(tmpPipeSeparator);
            create_AddToCart_Visualize_Share_Buttons(expJsonBlock, tmpDivQdsResult, true, AdaptationUrl);
        

        var expA_CloudView = ce('a');
        expA_CloudView.title = z('langResultsCloudView').value; // lang(en): 'Cloud view';
        expA_CloudView.className = 'qds_icon1 ds_add_vis_shr_spc ds_add_vis_shr_txt_spc';
        expA_CloudView.innerHTML = z('langResultsCloudView').value; // lang(en): 'Cloud view';
        expA_CloudView.href = 'javascript:void(0);';

        expA_CloudView.onclick = function (event) {
            ShowCloud(getIdUniquePart(expJsonBlock));
        }
        if(z('show_cloud').value == "true")
        tmpDivQdsResult.appendChild(expA_CloudView);

        // Display dimensions
        if (expJsonBlock.Dimensions != null) {
            var expAllDimensions = expJsonBlock.Dimensions.split(',');

            var expSpanDimensions = ce('span');
            expSpanDimensions.style.cursor = 'pointer';
            expSpanDimensions.id = "expSpanDimensions";
            expSpanDimensions.innerHTML = ' | ' + expAllDimensions.length + ' ' + z('langResultsDimensions').value + ' | '; // lang(en): ' Dimensions | ';
            expSpanDimensions.onclick = function (event) {
                //#region Show Dimensions pop-up
                var divDimesions = z('divDimensionsList');

                while (divDimesions.hasChildNodes()) {
                    divDimesions.removeChild(divDimesions.lastChild);
                }

                LiDimensionsArr = [];
                var arrDimensions = expJsonBlock.Dimensions.split(',');
                for (var d = 0; d < arrDimensions.length; d++) {
                    var liDimension = ce('li');
                    var LiId = "li_" + arrDimensions[d];
                    liDimension.id = LiId;
                    LiDimensionsArr.push(LiId);

                    var aDimension = ce('a');
                    aDimension.innerHTML = arrDimensions[d];
                    aDimension.href = 'javascript:void(0);';
                    aDimension.rel = arrDimensions[d];
                    aDimension.onclick = function (event) {
                        SelectedDimension = this.rel;
                        PostToDataView(getIdUniquePart(expJsonBlock), z('hdbnid').value);  	 //isBlock Issue : Removed parameter true used as isBlock
                    }

                    liDimension.appendChild(aDimension);


                    divDimesions.appendChild(liDimension);
                }

                SetCalloutPosition(event, z('divCallout_qds'), 20, 0);
                //                if (z('divCallout_qds').style.display == 'block') z('divCallout_qds').style.display = 'none';
                //                else z('divCallout_qds').style.display = 'block';

                //#endregion
            }
            tmpDivQdsResult.appendChild(expSpanDimensions);
        }

        var expSpanDV_count = ce('span');
        expSpanDV_count.innerHTML = expJsonBlock.DVCount + ' ' + z('langResultsDataValues').value; // lang(en): ' data values | ';

        tmpDivQdsResult.appendChild(expSpanDV_count);

        tmpDivQdsResult.appendChild(ce('br'));

        var tmpDivExplodedResultsTabular = ce('div');
        tmpDivExplodedResultsTabular.id = 'divExplodedResults_' + getIdUniquePart(expJsonBlock);
        tmpDivExplodedResultsTabular.style.display = 'none';

        tmpDivQdsResult.appendChild(tmpDivExplodedResultsTabular);

        var tmpDivExplodedJsonData = ce('div');
        tmpDivExplodedJsonData.id = "JsonData_" + getIdUniquePart(expJsonBlock);
        tmpDivExplodedJsonData.style.display = 'none';
        tmpDivExplodedJsonData.innerHTML = JSON.stringify(jsonResult);

        tmpDivQdsResult.appendChild(tmpDivExplodedJsonData);

        var expDvSeries = sortDvSeries(expJsonBlock.DVSeries, expJsonBlock.IsAreaNumeric);

        var expDivVisualizer = ce('div');
        expDivVisualizer.id = 'VisualizerCloud_' + getIdUniquePart(expJsonBlock);
        expDivVisualizer.className = 'ds_cloud_div';

        var tmpDefaultSG = expJsonBlock.DefaultSG.split('[@@@@]')[1];

        expDivVisualizer.innerHTML = Get_Visualizer_Creation_Data(tmpDefaultSG, expDvSeries.split(':')[0], expDvSeries.split(':')[1]);

        tmpDivQdsResult.appendChild(expDivVisualizer);
    }

    RenderGalleryItems(tmpDivQdsResult, getIdUniquePart(jsonResult));

    var tmpDivJsonData = ce('div');
    tmpDivJsonData.id = "JsonData_" + getIdUniquePart(jsonResult);
    tmpDivJsonData.style.display = 'none';
    tmpDivJsonData.innerHTML = JSON.stringify(jsonResult);

    tmpDivQdsResult.appendChild(tmpDivJsonData);

    divQDSResults.appendChild(tmpDivQdsResult);

    //RenderChart(IdUniquePart);
    //Added for Simul Gallery home page

    StopGalleryLoadingProcess = false;
    GetGalleryFromID(GalleryXML_Singl, getIdUniquePart(jsonResult));

    //Till here
}


function HideDimentionsCallOutDiv(e) {
    var clickId;
    var CtrlsIdArr = ['divCallout_qds'];

    try {

        clickId = e.target.id;

        if (clickId == "expSpanDimensions" || clickId == "tmpSpanDimensions") {
            if (z('divCallout_qds').style.display == 'block') {
                z('divCallout_qds').style.display = 'none';
            }
            else {
                z('divCallout_qds').style.display = 'block';
//                SetCalloutPosition(event, z('divCallout_qds'), 20, 0, event.clientX, event.clientY);
            }
        }
        else
        {
            if (jQuery.inArray(clickId, CtrlsIdArr) == -1 && jQuery.inArray(clickId, LiDimensionsArr) == -1) {
                z('divCallout_qds').style.display = 'none';
            }
        }
    }
    catch (err) { }
}

function RenderQsResult(jsonResult, divForResults, isExploded, AdaptationUrl, GalleryXML_Singl){
     //Added last parameter for Simul Gallery home page
    var divQDSResults = z(divForResults); //  z('divQDSResults');
    var ResultRow = jsonResult;
    var RetVal = ''; // Stores Markup HTML for that Result's div
    var ChartType = 'line';
    var DBNId = z('hdbnid').value;
    
    var strJsonData = JSON.stringify(jsonResult);
    var IdUniquePart = getIdUniquePart(jsonResult);
    var tmpDvSeries = sortDvSeries(jsonResult.DVSeries, jsonResult.IsAreaNumeric);

    var tmpDivQdsResult = document.createElement('div');
    tmpDivQdsResult.id = 'DIUA_' + IdUniquePart;
    tmpDivQdsResult.className = 'qds_result_cont';
    
    var tmpQdsBody = ce('tbody');

    var tmpTableQdsResultChart = ce('table');
    tmpTableQdsResultChart.style.width = '100%';
    
    var tmpTrQdsResultChart = ce('tr');

    var TdLeft = ce('td');

    var tmpTdLeft = ce('span');
    tmpTdLeft.className = 'flt_lft';


    //#> Left Content          


    var tmpSpanAreaInd = document.createElement('span');
    tmpSpanAreaInd.className = 'ds_main_hdn';
    tmpSpanAreaInd.innerHTML = jsonResult.Area + ' - ' + jsonResult.Indicator;
    if (!isCatalogResults) {
        tmpSpanAreaInd.style.cursor = "pointer";
        tmpSpanAreaInd.onclick = function (event) {
            PostToDataViewFromVisualize(IdUniquePart, z('hdbnid').value); // isBlock Issue : Do not pass isBlock parameter
        }
    }
    tmpTdLeft.appendChild(tmpSpanAreaInd);

    var tmpSpanUnit = document.createElement('span');
    tmpSpanUnit.className = 'ds_sub_hdn';
    tmpSpanUnit.innerHTML = ', ' + jsonResult.Unit;

    tmpTdLeft.appendChild(tmpSpanUnit);
        
    tmpTdLeft.appendChild(ce('br'));

    var tmpI_IndDesc = ce('i');
    tmpI_IndDesc.className = 'content ds_itl_txt';

    var shortIndicatorDesc = jsonResult.IndicatorDescription;

    if (jsonResult.IndicatorDescription.length > 100) {

        var FullIndDesc = ReplaceAll(jsonResult.IndicatorDescription, '\"', '&quot;');
        tmpI_IndDesc.onmouseover = function (event) {
            ShowCallout('divCallout', FullIndDesc, event);
        };
        tmpI_IndDesc.onmouseout = function (event) {
            HideCallout('divCallout');
        };

        shortIndicatorDesc = jsonResult.IndicatorDescription.substring(0, 100) + "...";
    }

    tmpI_IndDesc.innerHTML = shortIndicatorDesc;

    if (jsonResult.IndicatorDescription != '') {
        tmpTdLeft.appendChild(tmpI_IndDesc);
        tmpTdLeft.appendChild(ce('br'));
    }
    

    if (jsonResult.IC_Name != '') {
        var tmpSpanIC = ce('span');
        tmpSpanIC.style.lineHeight = '20px';
        tmpSpanIC.style.color = '#999999';
        tmpSpanIC.className = 'content';
        tmpSpanIC.innerHTML = 'Searched by : ' + jsonResult.IC_Name;

        tmpTdLeft.appendChild(tmpSpanIC);
    }
    //#<
    
    var tmpDivVisualizer = ce('div');
    tmpDivVisualizer.id = 'Visualizer_' + IdUniquePart;
    tmpDivVisualizer.className = 'ds_grph_contnr';
    tmpDivVisualizer.style.display = 'none';

    var tmpDefaultSG = jsonResult.DefaultSG.split('[@@@@]')[1];

    tmpDivVisualizer.innerHTML = Get_Visualizer_Creation_Data(tmpDefaultSG, tmpDvSeries.split(':')[0], tmpDvSeries.split(':')[1]);
    
    TdLeft.appendChild(tmpTdLeft);
    TdLeft.appendChild(tmpDivVisualizer);

    tmpTrQdsResultChart.appendChild(TdLeft);

    //tmpTableQdsResultChart.appendChild(tmpTrQdsResultChart);
    tmpQdsBody.appendChild(tmpTrQdsResultChart);
    tmpTableQdsResultChart.appendChild(tmpQdsBody);

    tmpDivQdsResult.appendChild(tmpTableQdsResultChart);


    tmpDivQdsResult.appendChild(tmpTableQdsResultChart);

    var expJsonBlock = jsonResult;

    var tmpDivBreak = ce('div');
    tmpDivBreak.className = 'navls_top_pg';
    tmpDivQdsResult.appendChild(tmpDivBreak);

    var tmpA_AreaBlockExpanded = ce('a');
    tmpA_AreaBlockExpanded.id = "ExpandedLoader_" + getIdUniquePart(expJsonBlock);
    tmpA_AreaBlockExpanded.className = 'qds_icon5 ds_block_txt';
    tmpA_AreaBlockExpanded.href = 'javascript:void(0);';
    if (expJsonBlock.AreaNId.indexOf("BL_") == 0) {
        tmpA_AreaBlockExpanded.innerHTML = expJsonBlock.Area + ' ' + z('langAreas').value; // lang(en):  ' areas';
    }
    else {
        tmpA_AreaBlockExpanded.innerHTML = expJsonBlock.Area;
    }

    tmpA_AreaBlockExpanded.onclick = function (event) {
        if (this.className == 'qds_icon5 ds_block_txt') this.className = 'qds_icon9 ds_block_txt';
        else this.className = 'qds_icon5 ds_block_txt';

        var nowAdaptationUrl = '';
        if (AdaptationUrl != null) nowAdaptationUrl = AdaptationUrl;

        GetExplodedResultsTable(z('hdbnid').value, expJsonBlock.IndicatorNId, expJsonBlock.UnitNId, expJsonBlock.AreaNIds, getIdUniquePart(expJsonBlock), nowAdaptationUrl);

    }

    tmpDivQdsResult.appendChild(tmpA_AreaBlockExpanded);

    
        var tmpPipeSeparator = document.createTextNode(' |');
        tmpDivQdsResult.appendChild(tmpPipeSeparator);
        create_AddToCart_Visualize_Share_Buttons(expJsonBlock, tmpDivQdsResult, false, AdaptationUrl);
    

    var expA_CloudView = ce('a');
    expA_CloudView.title = z('langResultsCloudView').value; // lang(en): 'Cloud view';
    expA_CloudView.className = 'qds_icon1 ds_add_vis_shr_spc ds_add_vis_shr_txt_spc';
    expA_CloudView.innerHTML = z('langResultsCloudView').value; // lang(en): 'Cloud view';
    expA_CloudView.href = 'javascript:void(0);';

    expA_CloudView.onclick = function (event) {
        ShowCloud(getIdUniquePart(expJsonBlock));
    }
    if (z('show_cloud').value == "true")
    tmpDivQdsResult.appendChild(expA_CloudView);    

    // Display dimensions
    if (expJsonBlock.Dimensions != null) {
        var expAllDimensions = expJsonBlock.Dimensions.split(',');

        var expSpanDimensions = ce('span');
        if (!isCatalogResults) {
            expSpanDimensions.style.cursor = 'pointer';
        }
        expSpanDimensions.id = "expSpanDimensions";
        expSpanDimensions.innerHTML = ' | ' + expAllDimensions.length + ' ' + z('langResultsDimensions').value + ' | '; // lang(en): ' Dimensions | ';
        expSpanDimensions.onclick = function (event) {
            //#region Show Dimensions pop-up
            var divDimesions = z('divDimensionsList');

            while (divDimesions.hasChildNodes()) {
                divDimesions.removeChild(divDimesions.lastChild);
            }

            LiDimensionsArr = [];
            var arrDimensions = expJsonBlock.Dimensions.split(',');
            for (var d = 0; d < arrDimensions.length; d++) {
                var liDimension = ce('li');
                var LiId = "li_" + arrDimensions[d];
                liDimension.id = LiId;
                LiDimensionsArr.push(LiId);
                
                var aDimension = ce('a');
                aDimension.innerHTML = arrDimensions[d];
                aDimension.href = 'javascript:void(0);';
                aDimension.rel = arrDimensions[d];
                aDimension.onclick = function (event) {
                    SelectedDimension = this.rel;
                    PostToDataView(IdUniquePart, z('hdbnid').value);  				//isBlock Issue : Removed parameter true used as isBlock
                }

                liDimension.appendChild(aDimension);


                divDimesions.appendChild(liDimension);
            }

            SetCalloutPosition(event, z('divCallout_qds'), 20, 0);
//                        if (z('divCallout_qds').style.display == 'block') z('divCallout_qds').style.display = 'none';
//                        else z('divCallout_qds').style.display = 'block';

            //#endregion
        }
        tmpDivQdsResult.appendChild(expSpanDimensions);
    }

    var expSpanDV_count = ce('span');
    expSpanDV_count.innerHTML = expJsonBlock.DVCount + ' ' + z('langResultsDataValues').value; // lang(en):  ' data values | ';

    tmpDivQdsResult.appendChild(expSpanDV_count);    

    tmpDivQdsResult.appendChild(ce('br'));

    var tmpDivExplodedResultsTabular = ce('div');
    tmpDivExplodedResultsTabular.id = 'divExplodedResults_' + getIdUniquePart(expJsonBlock);
    tmpDivExplodedResultsTabular.style.display = 'none';

    tmpDivQdsResult.appendChild(tmpDivExplodedResultsTabular);

    RenderGalleryItems(tmpDivQdsResult, getIdUniquePart(expJsonBlock));

    var tmpDivExplodedJsonData = ce('div');
    tmpDivExplodedJsonData.id = "JsonData_" + getIdUniquePart(expJsonBlock);
    tmpDivExplodedJsonData.style.display = 'none';
    tmpDivExplodedJsonData.innerHTML = JSON.stringify(jsonResult);

    tmpDivQdsResult.appendChild(tmpDivExplodedJsonData);

    var expDvSeries = sortDvSeries(expJsonBlock.DVSeries, expJsonBlock.IsAreaNumeric);

    var expDivVisualizer = ce('div');
    expDivVisualizer.id = 'VisualizerCloud_' + getIdUniquePart(expJsonBlock);
    expDivVisualizer.className = 'ds_cloud_div';

    var tmpDefaultSG = expJsonBlock.DefaultSG.split('[@@@@]')[1];

    expDivVisualizer.innerHTML = Get_Visualizer_Creation_Data(tmpDefaultSG, expDvSeries.split(':')[0], expDvSeries.split(':')[1]);

    tmpDivQdsResult.appendChild(expDivVisualizer);
    
    divQDSResults.appendChild(tmpDivQdsResult);

    //RenderChart(IdUniquePart);
    //Added for Simul Gallery home page
    StopGalleryLoadingProcess = false;
    GetGalleryFromID(GalleryXML_Singl, getIdUniquePart(jsonResult));
    //Till here
}

function RenderGalleryItems(divQdsResult, IdUniquePart) {

    var tmpA_btnGallery = ce('a');
    tmpA_btnGallery.id = 'btnGallery_' + IdUniquePart;
    tmpA_btnGallery.className = 'collapse';
    tmpA_btnGallery.style.display = 'none';
    tmpA_btnGallery.style.cursor = 'pointer';
    tmpA_btnGallery.style.width = '100%';
    tmpA_btnGallery.style.marginTop = '10px';
    tmpA_btnGallery.style.paddingLeft = '40px';
    tmpA_btnGallery.innerHTML = 'Gallery Objects';
    tmpA_btnGallery.onclick = function (event) {
        ToggleExpandCollapse(event);
    }

    divQdsResult.appendChild(tmpA_btnGallery);

    var tmpDivGallery = ce('div');
    tmpDivGallery.id = 'Gallery_' + IdUniquePart;
    tmpDivGallery.style.paddingLeft = '40px';

    divQdsResult.appendChild(tmpDivGallery);

    var tmpDivGalleryBuffer = ce('div');
    tmpDivGalleryBuffer.id = 'GalleryBuffer_' + IdUniquePart;
    tmpDivGalleryBuffer.style.paddingLeft = '5px';
    tmpDivGalleryBuffer.style.display = 'none';

    divQdsResult.appendChild(tmpDivGalleryBuffer);

}


function getIdUniquePart(jsonResult) {

    var result = '';
    var tmpAreaNId = jsonResult.AreaNId;

    if (jsonResult.IsAreaNumeric == "False") {
        while (tmpAreaNId.indexOf('_') != -1) {
            tmpAreaNId = tmpAreaNId.replace('_', '');
        }
    }

    result = z('hdbnid').value + "_" + jsonResult.IndicatorNId + "_" + jsonResult.UnitNId + "_" + tmpAreaNId;

    if (z(result) != null) {
        result = result;
    }

    return result;
}

function RenderResultOld(jsonResult, divForResults, isExploded) {

    var divQDSResults = z(divForResults); //  z('divQDSResults');
    var ResultRow = jsonResult;
    var RetVal = ''; // Stores Markup HTML for that Result's div
    var ChartType = 'line';
    var strJsonData = JSON.stringify(jsonResult);
    var IdUniquePart;
    var DBNId = z('hdbnid').value;
    var bgColor = 'padding-bottom: 10px;width:70%;';

    var strStockPath = '../../stock/';

    if (z('hdsby').value == "share") strStockPath = '../../../stock/';

    var tmpAreaNId = jsonResult.AreaNId;

    if (jsonResult.IsAreaNumeric == "False") {
        ChartType = 'column';
        while (tmpAreaNId.indexOf('_') != -1) {
            tmpAreaNId = tmpAreaNId.replace('_', '');
        }
    }

    var tmpDvSeries = sortDvSeries(jsonResult.DVSeries, jsonResult.IsAreaNumeric);

    if (isExploded) bgColor = 'padding-bottom: 10px;background-color: #efefef; padding-left: 5px;width:100%;';
    IdUniquePart = DBNId + "_" + jsonResult.IndicatorNId + "_" + jsonResult.UnitNId + "_" + tmpAreaNId;

    //RetVal += "<div id=\"DIUA_" + IdUniquePart + "\" style=\"" + bgColor + "float: left;\">";
    RetVal += "<div class=\"qds_result_cont\" id=\"DIUA_" + IdUniquePart + "\">";
    if (!isExploded) RetVal += "<div class=\"qds_result_lft_contnt\" id=\"DIUAleft_" + IdUniquePart + "\">";

    RetVal += "<a style=\"line-height:20px; cursor:pointer;text-decoration:underline;\" onclick=\"javascript:PostToDataView('" + DBNId + "', '" + jsonResult.NId + "','" + IdUniquePart + "');\">";

    RetVal += "<span class=\"heading2\" id=\"Area_" + IdUniquePart + "\">" + jsonResult.Area + "</span>";
    RetVal += " - <span class=\"heading2\" id=\"Indicator_" + IdUniquePart + "\">" + jsonResult.Indicator + "</span>";
    RetVal += "</a>";
    RetVal += ", <span class=\"content\" id=\"Unit_" + IdUniquePart + "\">" + jsonResult.Unit + " </span>";


    if (jsonResult.DefaultSG != '' && jsonResult.MRD != '' && jsonResult.MRDTP != '') {
        RetVal += "&nbsp;&nbsp;";
        RetVal += "<span style=\"color:#c00000\">";
        RetVal += "<span class=\"heading2\">";

        var NumericMRD = parseFloat(jsonResult.MRD);

        if (!isNaN(NumericMRD)) {
            RetVal += CommaFormatted(NumericMRD.toString());
        }
        else {
            RetVal += jsonResult.MRD;
        }

        RetVal += "</span>";
        RetVal += "<span class=\"content\"> (" + jsonResult.MRDTP + ")</span>";
        RetVal += "</span>";
    }

    if (tmpDvSeries != '' && tmpDvSeries.indexOf("|") != -1 && ChartType == 'line') {
        RetVal += "&nbsp;&nbsp;";
        RetVal += "<span class=\"sparkline\" values=\"" + tmpDvSeries.split(':')[1] + "\"></span>";
    }

    RetVal += "<div><i id=\"IDesc_" + IdUniquePart + "\"";

    if (jsonResult.IndicatorDescription.length > 100) {

        jsonResult.IndicatorDescription = ReplaceAll(jsonResult.IndicatorDescription, '\"', '&quot;');
        RetVal += "onmouseover=\"ShowCallout('divCallout', '" + jsonResult.IndicatorDescription + "', event);\"";
        RetVal += " onmouseout=\"HideCallout('divCallout');\"";

        jsonResult.IndicatorDescription = jsonResult.IndicatorDescription.substring(0, 100) + "...";
    }

    RetVal += "style=\"line-height:20px; color:#999999\" class=\"content\">";
    RetVal += jsonResult.IndicatorDescription;
    RetVal += "</i></div>";

    if (jsonResult.IC_Name != '') {
        RetVal += "<div><span id=\"IC_Name_" + IdUniquePart + "\"";
        RetVal += "style=\"line-height:20px; color:#999999\" class=\"content\">";
        RetVal += "Searched by : " + jsonResult.IC_Name;
        RetVal += "</span></div>";
    }


    //RetVal += this.GetICNameDiv(ICName);
    RetVal += "<div style=\"line-height:20px;cursor:default;\" class=\"content\">";

    if (ChartType == 'column') {
        RetVal += " <img src=\"" + strStockPath + "themes/default/images/Cloud.png\" style=\"cursor:pointer;\" width=\"19px\" height=\"12px\" onclick=\"javascript:ShowCloud('" + IdUniquePart + "');\"/>";

        RetVal += " <span style='cursor: pointer;' onmouseover=\"GetAreas('" + DBNId + "','" + jsonResult.AreaNIds + "', event, this);\"";
        RetVal += " onclick=\"GetExplodedResults('" + DBNId + "','" + jsonResult.IndicatorNId + "','" + jsonResult.UnitNId + "','" + jsonResult.AreaNIds + "','" + IdUniquePart + "');\" onmouseout=\"HideCallout('divCallout');\">" + jsonResult.AreaCount + " " + z('langAreas').value + "</span> | ";
    }

    RetVal += " <span onmouseover=\"GetSGs('" + DBNId + "','" + jsonResult.SGNIds + "', event, this);\"";
    RetVal += " onmouseout=\"HideCallout('divCallout');\">" + jsonResult.SGCount + " " + z('langSubGroup').value + ",</span>";

    RetVal += " <span onmouseover=\"GetSources('" + DBNId + "','" + jsonResult.SourceNIds + "', event, this);\"";
    RetVal += " onmouseout=\"HideCallout('divCallout');\">" + jsonResult.SourceCount + " " + z('langSources').value + ",</span>";

    RetVal += " <span onmouseover=\"GetTPs('" + DBNId + "','" + jsonResult.TPNIds + "', event, this);\"";
    RetVal += " onmouseout=\"HideCallout('divCallout');\">" + jsonResult.TPCount + " " + z('langResultsTimePeriod').value.toLowerCase() + "</span>";

    RetVal += " | <a onclick=\"javascript:PostToDataView('" + DBNId + "', '" + jsonResult.NId + "','" + IdUniquePart + "');\" style=\"cursor:pointer;\"><u>" + jsonResult.DVCount + " " + z('langResultsDataValues').value + "</u></a> | ";
    if (z('hdsby').value != "share") {
        RetVal += "<a onclick=\"javascript:ShareResult('" + IdUniquePart + "');\" style=\"cursor:pointer;\"><u>" + z('langResultsShare').value + "</u></a>";

        RetVal += "&nbsp;&nbsp;<img align=\"top\" src=\"../../stock/themes/default/images/add_to_cart.png\" style=\"cursor:pointer;\" id=\"ATDV_" + IdUniquePart + "\" value=\"add to data view\" onclick=\"javascript:AddtoDataView('" +
                  IdUniquePart + "', '" + jsonResult.Area + "', '" + jsonResult.Indicator + "', '" + jsonResult.Unit + "', '" + DBNId + "', '" + jsonResult.NId + "');\"/>";

    }

    RetVal += "</div>";

    RetVal += "<div style=\"padding:5px;\"></div>";

    RetVal += "<a id=\"btnGallery_" + IdUniquePart + "\" class=\"collapse\" onclick=\"ToggleExpandCollapse(event);\" style=\"display:none;cursor:pointer;width:100%;padding-left:40px;\">Gallery Objects</a>";
    RetVal += "<div id=\"Gallery_" + IdUniquePart + "\" style=\"padding-left:40px;\"></div>";
    RetVal += "<div  id=\"GalleryBuffer_" + IdUniquePart + "\"style=\"padding:5px;display:none;\"></div>";

    //RetVal += "<div style=\"padding:5px;\"></div>";


    if (!isExploded) {
        RetVal += "</div>"; // End of Left DIUA div

        RetVal += "<div class=\"qds_result_rgt_grph\" id=\"DIUAright_" + IdUniquePart + "\">"; // Start of Right DIUA div

        if (tmpDvSeries != '' && tmpDvSeries.indexOf(",") != -1 && ChartType == 'column') {
            RetVal += "<div id=\"Visualizer_" + IdUniquePart + "\" type=\"" + ChartType + "\" style=\"display:none;width:100%;height:70px;\">" + Get_Visualizer_Creation_Data(jsonResult.DefaultSG, tmpDvSeries.split(':')[0], tmpDvSeries.split(':')[1]) + "</div>";
            //RetVal += "<div  id=\"VisualizerBuffer_" + IdUniquePart + "\"style=\"padding:5px;display:none;\"></div>";
        }
        else {
            RetVal += "<div id=\"Visualizer_" + IdUniquePart + "\" type=\"column\" style=\"display:none;\">" +
                      "" + "</div>";
        }

        RetVal += "</div>"; // End of Right DIUA div
    }

    RetVal += "<div id=\"JsonData_" + IdUniquePart + "\" style=\"display:none;\">";

    RetVal += strJsonData;

    RetVal += "</div>"; // End of hidden JsonData div


    // Container for Exploded results  // background-color: Gray; padding:5px;
    RetVal += "<div  id=\"divExplodedResults_" + IdUniquePart + "\"style=\"display:none; margin-left: 10px; float: left;\"></div>";


    RetVal += "</div>"; // End of wrapper DIUA div

    divQDSResults.innerHTML += RetVal;

    AllIdUniquePart.push(IdUniquePart);

    //RenderChart(IdUniquePart);

}
function AppendAreasOrNot(IdUniquePart, jsonData) {
    var append = false;
    if ((IdUniquePart.indexOf("_QS") != -1) && (jsonData.jsonBlock != null)) {
        append = false;
    }
    else if ((IdUniquePart.indexOf("_BL") != -1) && (jsonData.jsonBlock != null)) {
        append = true;
    }
    else if ((IdUniquePart.indexOf("_QS") != -1) && (jsonData.jsonBlock == null)) {
        append = false;
    }
    else if ((IdUniquePart.indexOf("_BL") != -1) && (jsonData.jsonBlock == null)) {
        append = true;
    }
    else if ((IdUniquePart.indexOf("_BL") == -1) && (IdUniquePart.indexOf("_QS") == -1) && (jsonData.jsonBlock != null)) {
        append = false;
    }
    else if ((IdUniquePart.indexOf("_BL") == -1) && (IdUniquePart.indexOf("_QS") == -1) && (jsonData.jsonBlock == null)) {
        append = false;
    }
    return append;
}

function JSONStructure(IdUniquePart, jsonData) {
    var jsonStructure = "";
    if ((IdUniquePart.indexOf("_QS") != -1) && (jsonData.jsonBlock != null)) {
        jsonStructure = jsonData.jsonBlock;
    }
    else if ((IdUniquePart.indexOf("_BL") != -1) && (jsonData.jsonBlock != null)) {
        jsonStructure = jsonData.jsonBlock;
    }
    else if ((IdUniquePart.indexOf("_QS") != -1) && (jsonData.jsonBlock == null)) {
        jsonStructure = jsonData;
    }
    else if ((IdUniquePart.indexOf("_BL") != -1) && (jsonData.jsonBlock == null)) {
        jsonStructure = jsonData;
    }
    else if ((IdUniquePart.indexOf("_BL") == -1) && (IdUniquePart.indexOf("_QS") == -1)) {
        jsonStructure = jsonData;
    }
    return jsonStructure;
}
//Added for Simul Gallery Home Page
function GetGalleryFromID(GalleryXML, IDUniquePart) {
    var divGallery, btnGallery, divVisualizer, spanIndicator, spanArea, divJsonData;
    var Indicator, Area;
    var spanLoading, spanLoadingOuter, divQDSResults;
    var anchorViewData;
    var IdUniquePart;
    var jsonData;
    if (z('hdsby').value == "share")
        isSearchResultsGalleryEnabled = true;
    if (StopGalleryLoadingProcess == false && isSearchResultsGalleryEnabled) {
        spanLoading = z('spanLoading');
        spanLoadingOuter = z('spanLoadingOuter');
        divQDSResults = z('divQDSResults');

        divGallery = z("Gallery_" + IDUniquePart);
        btnGallery = z("btnGallery_" + IDUniquePart);
        divVisualizer = z("Visualizer_" + IDUniquePart);
        divGallery.innerHTML = "";

        anchorViewData = z("ATDV_" + IDUniquePart);

        if (IsAlreadyInDataCart(IDUniquePart) == true) {
            anchorViewData.style.display = "none";
        }

        try {
            if (StopGalleryLoadingProcess == false && GalleryXML !="") {
                callbackGalleryInitQDS(GalleryXML, divGallery.id);
                btnGallery.style.display = "block";
                divGallery.style.display = "none";
            }
            else
            {
            btnGallery.style.display = "none";
            divGallery.style.display = "none";
            }
        }
        catch (ex) {
        }
    }
}
function ce(ElementType) {
    var tmpElement = document.createElement(ElementType);
    return tmpElement;
}
function hideDropDownDiv(e) {

    var clickId = e.target.id;
    var clickClass = e.target.className;

    // for download div
    var downLoadArr = ['galleryDownloadBtn', 'toggleDownload', 'vcLangLblDlPDF', 'vcLangLblDlPNG', 'vcLangLblDlSVG'];
    if (jQuery.inArray(clickId, downLoadArr) == -1) {
        di_jq('#toggleDownload').hide('slow');
    }
}
