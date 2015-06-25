var GalleryFilter = 'A';
var PageNumber = 1;
var objDataGlobal = [];
var dividGlobal;
var xmlGlobal;
var ImagesperPage;
var g_key;
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName) {
    var hsgcount = 10;
    // Set the active selection to Data Search Navigation link    
    di_jq("#aGalleries").attr("class", "navActive")

    // Hide background color div
    di_jq("#apDiv2").css("display", "none");


    createFormHiddenInputs("frmGallery", "POST");
    SetCommonLinksHref("frmGallery", "POST");

    setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

    if (hdsby != "share") {
        InitializeDatabaseComponent(getAbsURL('stock'), 'database_div', hdbnid, hlngcodedb, 550, 460, true, "OkDbComponent", "CloseDbComponent");
        if (GetLanguageCounts() > 1) {
            ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);
        }
        
        ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div', hdbnid, hlngcodedb, z('langQdsIndCaption').value, z('langQdsAreaCaption').value, '100%', '100px', 'GetQDSResult', "", "", "", "", false, z('langQdsSerchBtnText').value, z('langQdsAreaBlockText').value);
        DrawDatabaseDetailsComponent(hdbnid, 'db_details_div');
    }
    di_qds_search_data();
    ImagesperPage = parseInt(z('hPageSize').value);
    //z("keyword_txt").value = "(" + z("langkeywords").innerHTML + ")";
}

function SearchGallery(GalleryItemType) {
    var DbNIds;
    var aGalleryAll, aGalleryTables, aGalleryGraphs, aGalleryMaps;

    aGalleryAll = z('aGalleryAll');
    aGalleryTables = z('aGalleryTables');
    aGalleryGraphs = z('aGalleryGraphs');
    aGalleryMaps = z('aGalleryMaps');

//    fillerResult(GalleryItemType, 'divQDSResults');
    GalleryFilter = GalleryItemType;
    if (GalleryItemType == 'A') {
        aGalleryAll.className = "navActive";
        aGalleryTables.className = "nav";
        aGalleryGraphs.className = "nav";
        aGalleryMaps.className = "nav";
    }
    else if (GalleryItemType == 'T') {
        aGalleryAll.className = "nav";
        aGalleryTables.className = "navActive";
        aGalleryGraphs.className = "nav";
        aGalleryMaps.className = "nav";
    }
    else if (GalleryItemType == 'G') {
        aGalleryAll.className = "nav";
        aGalleryTables.className = "nav";
        aGalleryGraphs.className = "navActive";
        aGalleryMaps.className = "nav";
    }
    else if (GalleryItemType == 'M') {
        aGalleryAll.className = "nav";
        aGalleryTables.className = "nav";
        aGalleryGraphs.className = "nav";
        aGalleryMaps.className = "navActive";
    }

//    if (di_db_getAllSelDbNIds().split(',').length > 1) {
//        GetASResultForGalleryItemType(GalleryItemType);
//    }
//    else {
//        GetQDSResultForGalleryItemType(GalleryItemType);
    //    }
    if (di_db_getAllSelDbNIds().split(',').length > 1) {
        GetASResultForGalleryItemType(GalleryItemType);
    }
    else {
        var keywordTxtBoxValue = z("keyword_txt").value;
        if (keywordTxtBoxValue.indexOf("(") != 0 && keywordTxtBoxValue != "") {
            GetGalleryResultByKeywords(GalleryItemType);
        }
        else {
            GetQDSResultForGalleryItemType(GalleryItemType);
        }
    }

    var keywordTxtBoxValue = z("keyword_txt").value;
    if (keywordTxtBoxValue.indexOf("(") != 0 && keywordTxtBoxValue != "") {
        GetGalleryResultByKeywords(GalleryItemType);
    }
}

function GetQDSResult() {
    GetQDSResultForGalleryItemType(GalleryFilter);
}

function GetQDSResultForGalleryItemType(GalleryItemType) {
    z("keyword_txt").value = "";
    z("img_keyword_cross").style.display = "none";     
    var InputParam;
    var IndicatorIC, Areas, IndicatorICText, AreasText, IndicatorText, IndicatorBlockText, AreaText, AreaBlockText;
    var spanLoading, spanLoadingOuter, tblQDSResults, divQDSResults, spanTimeCounter, glry_LoadingOuter;
    var TotalRecords, TimeTakenInSecs;
    IndicatorIC = di_get_selected_indicators_values();
    Areas = di_get_selected_areas_values();

    IndicatorText = di_qds_get_ind_search_data();
    IndicatorBlockText = di_qds_get_all_ind_block_data();
    AreaText = di_qds_get_area_search_data();
    AreaBlockText = di_qds_get_all_area_block_data();

    IndicatorICText = GetTextAndBlockTextCombinedValue(IndicatorText, IndicatorBlockText);
    AreasText = GetTextAndBlockTextCombinedValue(AreaText, AreaBlockText);
    glry_LoadingOuter = z('glry_LoadingOuter');
    spanLoading = z('spanLoading');
    spanLoadingOuter = z('spanLoadingOuter');
    divQDSResults = z('divQDSResults');
    tblQDSResults = z('tblQDSResults');
    spanTimeCounter = z('spanTimeCounter');

    //if ((IndicatorIC != null && IndicatorIC != "") || (Areas != null && Areas != "")) {
    spanLoadingOuter.style.display = "block";
    spanLoading.style.display = "block";
    glry_LoadingOuter.style.display = "block";
        spanLoading.innerHTML = "Searching the gallery...";
        divQDSResults.innerHTML = "";
        spanTimeCounter.innerHTML = "";

        InputParam = di_qds_get_all_ind_block_text();
        InputParam += ParamDelimiter + Get_IC_from_components_IndicatorIC(IndicatorIC);
        InputParam += ParamDelimiter + di_qds_get_all_area_block_text();
        InputParam += ParamDelimiter + z('hlngcode').value;
        InputParam += ParamDelimiter + z('hdbnid').value;
        InputParam += ParamDelimiter + GalleryItemType;
        InputParam += ParamDelimiter + IndicatorICText + "||" + AreasText;
        var userLoginId = "";
        if (z('hLoggedInUserNId').value != "") {
            userLoginId = z('hLoggedInUserNId').value.split("|")[0];
        }
        if (userLoginId == "")
            userLoginId = "-1";

        InputParam += ParamDelimiter + userLoginId;
        InputParam += ParamDelimiter + "null";
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '41', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data.split(ParamDelimiter)[0] != null && data.split(ParamDelimiter)[0] != '') {
                            tblQDSResults.style.display = "";
                            if (GalleryItemType == 'A') {
                                z('aGalleryAll').className = "navActive";
                            }
                        }
                        TotalRecords = data.split(ParamDelimiter)[1];
                        TimeTakenInSecs = data.split(ParamDelimiter)[2];

                        PageNumber = 1;
                        callbackGalleryInit(data.split(ParamDelimiter)[0], 'divQDSResults');
                        //                        spanTimeCounter.innerHTML = data.split(ParamDelimiter)[1];

                        spanTimeCounter.innerHTML = z('langGalleryResults').value + " : " + TotalRecords + ", " + z('langGalleryTimeTaken').value + " : " + TimeTakenInSecs + " " + z('langGallerySeconds').value;
                        di_qds_resize_ind_blocks();
                        di_qds_resize_area_blocks();

                        spanLoadingOuter.style.display = "none";
                        spanLoading.innerHTML = "";
                        glry_LoadingOuter.style.display = "none";
                    }
                    catch (ex) {
                        alert("message:" + ex.message);
                    }
                },
                error: function () {
                    
                },
                cache: false
            });
        }
        catch (ex) {
        }
    /*}
    else {
        tblQDSResults.style.display = "none";
        divQDSResults.innerHTML = "";
        spanTimeCounter.innerHTML = "";
        spanLoadingOuter.style.display = "none";
        spanLoading.innerHTML = "";
    }*/
}

function GetASResult() {
    GetASResultForGalleryItemType("A");
}

function GetASResultForGalleryItemType(GalleryItemType) {
    var IndicatorIC, Areas, LngCode, ASDatabaseIds;
    var IndicatorText, IndicatorBlockText, AreaText, AreaBlockText;
    var spanLoading, spanLoadingOuter, tblQDSResults, divQDSResults, spanTimeCounter, glry_LoadingOuter ;
    IndicatorText = di_qds_get_ind_search_data();
    IndicatorBlockText = di_qds_get_all_ind_block_data();
    AreaText = di_qds_get_area_search_data();
    AreaBlockText = di_qds_get_all_area_block_data();

    spanLoading = z('spanLoading');
    spanLoadingOuter = z('spanLoadingOuter');
    spanTimeCounter = z('spanTimeCounter');
    divQDSResults = z('divQDSResults');
    tblQDSResults = z('tblQDSResults');
    glry_LoadingOuter = z('glry_LoadingOuter');
    IndicatorIC = GetTextAndBlockTextCombinedValue(IndicatorText, IndicatorBlockText);
    Areas = GetTextAndBlockTextCombinedValue(AreaText, AreaBlockText);
    LngCode = z('hlngcode').value;
    ASDatabaseIds = di_db_getAllSelDbNIds();

    if (((IndicatorIC != null && IndicatorIC != "") || (Areas != null && Areas != "")) &&
        (ASDatabaseIds != null && ASDatabaseIds != "")) {
        spanLoadingOuter.style.display = "block";
        spanLoading.innerHTML = "Searching gallery...";
        divQDSResults.innerHTML = "";
        spanTimeCounter.innerHTML = "";

        InputParam = IndicatorIC;
        InputParam += ParamDelimiter + Areas;
        InputParam += ParamDelimiter + LngCode;
        InputParam += ParamDelimiter + ASDatabaseIds;
        InputParam += ParamDelimiter + GalleryItemType;
        InputParam += ParamDelimiter + IndicatorIC + "||" + Areas;

        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '42', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data.split(ParamDelimiter)[0] != null && data.split(ParamDelimiter)[0] != '') {
                            tblQDSResults.style.display = "";
                            if (GalleryItemType == 'A') {
                                z('aGalleryAll').className = "navActive";
                            }
                        }

                        callbackGalleryInit(data.split(ParamDelimiter)[0], 'divQDSResults');
                        spanTimeCounter.innerHTML = data.split(ParamDelimiter)[1];

                        di_qds_resize_ind_blocks();
                        di_qds_resize_area_blocks();

                        spanLoadingOuter.style.display = "none";
                        spanLoading.innerHTML = "";
                        glry_LoadingOuter.style.display = "none";
                    }
                    catch (ex) {
                        alert("message:" + ex.message);
                    }
                },
                error: function () {
                    
                },
                cache: false
            });
        }
        catch (ex) {
        }
    }
    else {
        tblQDSResults.style.display = "none";
        divQDSResults.innerHTML = "";
        spanTimeCounter.innerHTML = "";
        spanLoadingOuter.style.display = "none";
        spanLoading.innerHTML = "";
        glry_LoadingOuter.style.display = "none";
    }
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
        if (IndicatorIC[i].split('_')[1] == 1) {
            Indicators = Indicators + IndicatorIC[i].split('_')[0] + ",";
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

/************************************************/
var GlobalStoageForGallery = [];

/* function to callback init for QDS without callback input data-xml data, divid-div id where markup will render */
function callbackGalleryInitQDS(data, divid) {

    try {
        var storeGallXmlData = '';
        var storeGallPage = [];
        var initui = '';
        /* **********************************
        Start code for IE7 Fix
        ********************************** */
        if (Browser.Version() < 9) {
            // if client is using IE8 or lower, run this code
            var obj = di_jq(parseXmlNBrowser(data.replace(/&/g, "&amp;"))).find('g');
            var objB = di_jq(parseXmlNBrowser(data.replace(/&/g, "&amp;"))).find('base_path');
        }
        /* **********************************
        End code for IE7 Fix
        ********************************** */
        else {
            var obj = di_jq(parseXml(data)).find('g');
            var objB = di_jq(parseXml(data)).find('base_path');
        }

        var basePath = '';
        var callingPage = getCurrentPageName(); //Changes done to implement pop up in home page--way2        
        storeGallXmlData = data;
        objB.each(function () {
            basePath = di_jq(this).attr("path");
        });
        obj.each(function () {
            var g_type = di_jq(this).attr("p");
            var g_src = di_jq(this).attr("s");
            var g_title = di_jq(this).attr("t");
            var g_nid = di_jq(this).attr("i");
            basePath = di_jq(this).attr("fpath");
            storeGallPage.push(g_nid);
            initui += '<span class="thmb_pos"><img id="img_' + g_nid + '" src="' + basePath + g_src + '" onclick=\'callbackGalleryClick(this,"' + g_nid + '", "yes",null)\' class="gallimgBox" title="' + g_title + '"/><div class="gallimgBox_titl">' + g_title + '</div></span>';

        });
        initui += '</div>';
        di_jq('#' + divid).html(initui);
        //Changes done to implement pop up in home page--way2
        if (callingPage == "Gallery.aspx") {
            showTitle(z("showtitlechkbox").checked);
        }
        setGalleryDataFromGlobalStorage(divid, data, storeGallPage);
    }
    catch (err) { }

}

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

function parseXmlNBrowser(xml) {
    if (jQuery.browser.msie) {
        var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.loadXML(xml);
        xml = xmlDoc;
    }
    return xml;
}
/* **********************************
End code for IE7 Fix
********************************** */

/* function to callback init without callback input data-xml data, divid-div id where markup will render */
function callbackGalleryInit(data, divid) {

    try {
        var btnFirstPage = z('btnFirstPage');
        var btnLastPage = z('btnLastPage');
        var btnDecPage = z('btnDecPage');
        var btnIncPage = z('btnIncPage');
        btnFirstPage.style.visibility = "visible";
        btnLastPage.style.visibility = "visible";
        btnIncPage.style.visibility = "visible";
        btnDecPage.style.visibility = "visible";
       
        btnFirstPage.disabled = false;
        btnLastPage.disabled = false;
        btnIncPage.disabled = false;
        btnDecPage.disabled = false;
        if (PageNumber == 1) {
            btnFirstPage.disabled = true;
            btnDecPage.disabled = true;
        }
        if (PageNumber == Math.ceil(objDataGlobal.length / ImagesperPage)) {
            btnLastPage.disabled = true;
            btnIncPage.disabled = true;
        }

        var divPaging = z('divPaging');
        divPaging.innerHTML = "";
        var storeGallXmlData = '';
        var storeGallPage = [];
        var initui = '';
        var PageSize = ImagesperPage;
        var currentRenderingEnd = 0;
        var currentRenderingStart = 0;
        objDataGlobal = [];
        if (PageNumber == 1)
            currentRenderingStart = 1;
        else {
            currentRenderingStart = ((PageNumber - 1) * ImagesperPage) + 1;
            PageSize = currentRenderingStart + ImagesperPage-1;
        }
        /* **********************************
        Start code for IE7 Fix
        ********************************** */
        if (Browser.Version() < 9) {
            // if client is using IE8 or lower, run this code
            var obj = di_jq(parseXmlNBrowser(data.split(ParamDelimiter)[0].replace(/&/g, "&amp;"))).find('g');
        }
        /* **********************************
        End code for IE7 Fix
        ********************************** */
        else {
            var obj = di_jq(parseXml(data.split(ParamDelimiter)[0])).find('g');
        }

        for (var j = 0; j < obj.length; j++) {
            objDataGlobal.push(obj[j]);
        }
        dividGlobal = divid;
        xmlGlobal = data;
        if (obj.length < PageSize)
            currentRenderingEnd = obj.length;
        else
            currentRenderingEnd = PageSize;
        var objCurrentRender = [];
        for (var i = currentRenderingStart-1; i < currentRenderingEnd; i++) {
            objCurrentRender.push(obj[i]);
        }
        var PaginationData = Pagination(objCurrentRender);

        var initui = PaginationData[0];
        storeGallPage = PaginationData[1];

        var callingPage = getCurrentPageName(); //Changes done to implement pop up in home page--way2        
        storeGallXmlData = data;

        initui += '</div>';
        di_jq('#' + divid).html(initui);

        PaginationBar('divPaging', Math.ceil(objDataGlobal.length / ImagesperPage), PageNumber);

        //Changes done to implement pop up in home page--way2
        if (callingPage == "Gallery.aspx") {
            showTitle(z("showtitlechkbox").checked);
        }
        setGalleryDataFromGlobalStorage(divid, data, storeGallPage);
        if (objDataGlobal.length == 0) {
            btnFirstPage.style.visibility = "hidden";
            btnLastPage.style.visibility = "hidden";
            btnIncPage.style.visibility = "hidden";
            btnDecPage.style.visibility = "hidden";
        }
    }
    catch (err) { }

}


function PaginationBar(divPagingBar, EndLimit, CurrentPage) {
    if (EndLimit > 0) {

        var divPaging = z(divPagingBar);

        var spanCurrentLink = ce('span');
        spanCurrentLink.innerHTML = '<b>' + CurrentPage + '</b>';
        spanCurrentLink.style.fontSize = "12px";
        spanCurrentLink.className = 'paginationCurrent';
        divPaging.appendChild(spanCurrentLink);
        var spanSpace1 = ce('span');
        spanSpace1.innerHTML = " ";
        var spanSpace2 = ce('span');
        spanSpace2.innerHTML = " ";
        divPaging.appendChild(spanSpace1);
        divPaging.appendChild(spanSpace2);


        var counterPage = 1;

        for (var inc = 1; inc <= 10; inc++) {

            if (CurrentPage + inc <= EndLimit) {
                var aIPage = ce('a');
                aIPage.innerHTML = CurrentPage + inc;
                aIPage.href = 'javascript:void(0);';
                aIPage.value = CurrentPage + inc;
                aIPage.className = 'a';
                aIPage.onclick = function (event) {
                    SetPage(this.value);
                }
                divPaging.appendChild(aIPage);
                var spanSpace1 = ce('span');
                spanSpace1.innerHTML = " ";
                var spanSpace2 = ce('span');
                spanSpace2.innerHTML = " ";
                divPaging.appendChild(spanSpace1);
                divPaging.appendChild(spanSpace2);
                counterPage = counterPage + 1;
                if (counterPage == 10) break;

            }


            if (CurrentPage - inc > 0) {
                var prePageLink = ce('a');
                prePageLink.innerHTML = CurrentPage - inc;
                prePageLink.href = 'javascript:void(0);';
                prePageLink.value = CurrentPage - inc;
                prePageLink.className = 'a';
                prePageLink.onclick = function (event) {
                    SetPage(this.value);
                }
                var spanSpace1 = ce('span');
                spanSpace1.innerHTML = " ";
                var spanSpace2 = ce('span');
                spanSpace2.innerHTML = " ";
                divPaging.insertBefore(spanSpace1, divPaging.firstChild);
                divPaging.insertBefore(spanSpace2, divPaging.firstChild);
                divPaging.insertBefore(prePageLink, divPaging.firstChild);
                counterPage = counterPage + 1;
                if (counterPage == 10) break;
            }
        }
    }

}

function DecrementPageNumber() {
    if (PageNumber > 1)
        PageNumber = PageNumber - 1;
    else
        PageNumber = 1;
    callbackGalleryInit(xmlGlobal, dividGlobal);
}
function IncrementPageNumber() {
    var LastPage = Math.ceil(objDataGlobal.length / ImagesperPage);
    if (PageNumber < LastPage)
        PageNumber = PageNumber + 1;
    else
        PageNumber = LastPage;
    callbackGalleryInit(xmlGlobal, dividGlobal);
}
function FirstPage() {
    PageNumber = 1;
    callbackGalleryInit(xmlGlobal, dividGlobal);
}
function LastPage() {
    PageNumber = Math.ceil(objDataGlobal.length / ImagesperPage);
    callbackGalleryInit(xmlGlobal, dividGlobal);
}
function SetPage(n) {
    PageNumber = n;
    callbackGalleryInit(xmlGlobal, dividGlobal);
}
function ce(ElementType) {
    var tmpElement = document.createElement(ElementType);
    return tmpElement;
}

function Pagination(obj) {

    var RetVal = [];
    var tmpGalleryPage = [];
    var initui = '';
    for (var i = 0; i < obj.length; i++) {
        var tmpObj = obj[i];
        var g_type = di_jq(tmpObj).attr("p");
        var g_src = di_jq(tmpObj).attr("s");
        var g_title = di_jq(tmpObj).attr("t");
        var g_nid = di_jq(tmpObj).attr("i");
        var basePath = di_jq(tmpObj).attr("fpath");
        tmpGalleryPage.push(g_nid);
        
        initui += '<span class="thmb_pos"><img id="img_' + g_nid + '" src="' + basePath + g_src + '" onclick=\'callbackGalleryClick(this,"' + g_nid + '", "yes",null)\' class="gallimgBox" title="' + g_title + '"/><div class="gallimgBox_titl">' + g_title + '</div></span>';
    }

    RetVal.push(initui);
    RetVal.push(tmpGalleryPage);

    return RetVal;

}
/* function to filter search result */
function fillerResult(act, divid) {

    try {
        PageNumber = 1;
        var initui = ''; storeGallPage = [];
        var obj;
        objDataGlobal = [];
        var GalleryItemData = getGalleryDataFromGlobalStorage(divid);
        var storeGallXmlData = GalleryItemData.data;
        var storeGallPage = GalleryItemData.pagging;
        if (act != '' && act != null && act != undefined) {            
            if (act == 'A') {
                obj = di_jq(parseXml(storeGallXmlData)).find('g');
            }
            else {
                obj = di_jq(parseXml(storeGallXmlData)).find('g[p="' + act + '"]');
            }
            for (var j = 0; j < obj.length; j++) {
                objDataGlobal.push(obj[j]);
            }
            dividGlobal = divid;
//            var PageSize = 5;
//            var currentRenderingEnd = 0;
//            var currentRenderingStart = 0;

//            if (PageNumber == 1)
//                currentRenderingStart = 1;
//            else {
//                currentRenderingStart = ((PageNumber - 1) * 5) + 1;
//                PageSize = currentRenderingStart + 4;
//            }

//            if (obj.length < PageSize)
//                currentRenderingEnd = obj.length;
//            else
//                currentRenderingEnd = PageSize;
//            var objCurrentRender = [];
//            for (var i = currentRenderingStart-1; i < currentRenderingEnd; i++) {
//                objCurrentRender.push(obj[i]);
//            }
//            var PaginationData = Pagination(objCurrentRender);

//            var initui = PaginationData[0];
//            storeGallPage = PaginationData[1];

//            di_jq('#' + divid).html(initui);
        }
       
    }
    catch (err) { }
}

function getGalleryDataFromGlobalStorage(KeyText) {
    var GalleryData = null;
    if (GlobalStoageForGallery.length > 0) {
        for (var i = 0; i < GlobalStoageForGallery.length; i++) {
            GalleryData = GlobalStoageForGallery[i];
            if (GalleryData.key == KeyText) {
                break;
            }
        }
    }
    return GalleryData;
}

function setGalleryDataFromGlobalStorage(KeyText, data, GallPage) {
    var IsFound = false;
    if (GlobalStoageForGallery.length > 0) {
        for (var i = 0; i < GlobalStoageForGallery.length; i++) {            
            if (GlobalStoageForGallery[i].key == KeyText) {
                GlobalStoageForGallery[i].data = data;
                GlobalStoageForGallery[i].pagging = GallPage;
                IsFound = true;
                break;
            }
        }        
    }
    if(!IsFound || GlobalStoageForGallery.length==0){
        // Store Gallery items into collection
        var galleryData = new Object();
        galleryData.key = KeyText;
        galleryData.data = data;
        galleryData.pagging = GallPage;
        GlobalStoageForGallery.push(galleryData);
    }    
}

/* function to callback Click */
function callbackGalleryClick(currentObj, nid, maskflag,divEleObj) {
    if (nid != null && nid != "") {
        try {
            g_key = "";
            var ImgUrl = "../../stock/themes/default/images/dataview/";                        
            var storeGallXmlData = '';
            var storeGallPage = [];
            var divEle;
            if (divEleObj == null)
                divEle = currentObj.parentNode.parentNode;
            else
                divEle = divEleObj;
            var GalleryItemData = getGalleryDataFromGlobalStorage(divEle.id);
            storeGallXmlData = GalleryItemData.data;
            storeGallPage = GalleryItemData.pagging;
            var indexVal = jQuery.inArray(nid, storeGallPage);
            //Changes done to implement pop up in home page--Way2
            var callingPage = getCurrentPageName();
            var displayProp = "inline";
            var userLoginId;
            if (callingPage == "home.aspx" || z('hLoggedInUserNId').value == "") {
                    displayProp = "none";
                }
            //
            var BackBtnClass = "di_gui_button";
            var NextBtnClass = "di_gui_button";
            if (indexVal == 0) {
                BackBtnClass = "di_gui_button_disabled";
            }
            if (indexVal == storeGallPage.length - 1) {
                NextBtnClass = "di_gui_button_disabled";
            }

            if (maskflag == 'yes') {
                ApplyMaskingDiv();
                if (z('hdsby').value == "share") {
                    SetCloseButtonInPopupDiv(di_jq('#PopupOuterBox'), 'hideGalleryPopup', "../../../stock/themes/default/images/close.png");
                }
                else {
                    SetCloseButtonInPopupDiv(di_jq('#PopupOuterBox'), 'hideGalleryPopup');
                }
                di_jq('#PopupOuterBox').css("position","fixed");
                di_jq('#PopupOuterBox').show('slow');
                GetWindowCentered(z('PopupOuterBox'), 700, 520);
            }
            di_jq('#PopupOuterBox').attr("rel", nid);

            /* **********************************
            Start code for IE7 Fix
            ********************************** */
            if (Browser.Version() < 9) {
                // if client is using IE8 or lower, run this code
                var objB = di_jq(parseXmlNBrowser(storeGallXmlData.replace(/&/g, "&amp;"))).find('base_path');
                var obj = di_jq(parseXmlNBrowser(storeGallXmlData.replace(/&/g, "&amp;"))).find('g[i="' + nid + '"]');
            }
            /* **********************************
            End code for IE7 Fix
            ********************************** */
            else {
                var objB = di_jq(parseXml(storeGallXmlData)).find('base_path');
                var obj = di_jq(parseXml(storeGallXmlData)).find('g[i="' + nid + '"]');
            }

            var basePath = '';
           
            objB.each(function () {
                basePath = di_jq(this).attr("path");
            });
            var dtui = '';
            obj.each(function () {

                dtui = '<div style="width:100%">';

                var g_type = di_jq(this).attr("p");
                var g_src = di_jq(this).attr("sbig");
                var g_title = di_jq(this).attr("t");
                var g_desc = di_jq(this).attr("desc");
                var g_date = di_jq(this).attr("md");
                //                var g_key = di_jq(this).attr("k");

                var InputParam;
                InputParam = di_jq(this).attr("kid");
                InputParam += ParamDelimiter + di_jq(this).attr("k");
                if (z('hdsby').value != "share") {
                    var htmlResp = di_jq.ajax({
                        type: "POST",
                        url: CallBackPageName,
                        data: { 'callback': '1003', 'param1': InputParam },
                        async: false,
                        success: function (data) {
                            try {
                                SetKeywordString(data);
                            }

                            catch (ex) {
                                alert("message:" + ex.message);
                            }
                        },
                        error: function () {
                            
                        },
                        cache: false
                    });
                }
                else {
                    SetKeywordString("");
                }


                var g_keyid = di_jq(obj).attr("kid");
                var g_shtml = di_jq(obj).attr("shtml");
                var g_vtype = di_jq(obj).attr("vtype");
                var g_pnid = di_jq(obj).attr("pnid");
                basePath = di_jq(obj).attr("fpath");
                var titleText = g_title;
                if (g_title.length > 55) {
                    titleText = g_title.substr(0, 55) + "...";
                }
                dtui += '<div class="popup_heading1" style="padding-bottom:0px;"><table cellpadding="0" cellspacing="0" width="100%" height="100%"><tr><td style="width:85%;" title="' + g_title + '">' + titleText+'</td>';
                dtui += '<td style="width:15%;"><image id="pop_viewimg" src="' + ImgUrl + 'view.png" title = "' + z('GalView').value + '" style="width:16px;height:16px;cursor:pointer;" onclick=\'openLiveVisualizer("';
                if (g_vtype == "map") {
                    dtui += basePath + g_src;
                }
                else {
                    dtui += basePath + g_shtml;
                }

                dtui += '")\'/>&nbsp;';
                if (g_vtype != "treemap") { // Hide download icon in case of table & treemap
                    dtui += '<image id="galleryDownloadBtn" src="' + ImgUrl + 'download_icon.png" title = "' + z('GalDownload').value + '" style="width:16px;height:16px;cursor:pointer;" onclick="toggleDownload()"/>&nbsp;';
                }
                dtui += '<image id="pop_shareimg" src="' + ImgUrl + 'share.png" title = "' + z('GalShare').value + '" style="width:16px;height:16px;cursor:pointer;" onclick=\'shareGallery("';
                if (g_vtype == "map") {
                    dtui += basePath + g_src;
                }
                else {
                    dtui += basePath + g_shtml;
                }
                //Changes done to implement pop up in home page--Way2
                dtui += '")\'/>&nbsp<image id="pop_deleteimg" src="' + ImgUrl + 'delete.png" title = "' + z('GalDelete').value + '" style="width:16px;height:16px;cursor:pointer;display:' + displayProp + ';" onclick=\'deletePresentation("' + g_pnid + '")\'/></td></tr></table></div>';


                dtui += '<div id="toggleDownload" class="fullViewContainer2" style="border: 1px solid #CCCCCE;display:none;z-index:1200;margin-left:580px;background-color:#fff;height: auto;margin-top: 0px;position: absolute;width: 120px;" onblur="toggleDownload()">';
                if (g_vtype == "table")// Start links for Chart
                {
                    /*else if(g_vtype == "treemap" || g_vtype == "cloud")
                    {
                    }*/
                    //z("galleryDownloadBtn").style.display = "none";
                    // Start links for Table 
                    dtui += '<p id="tblLangLblDlXLS"><a href="javascript:void(0)" onclick=\'downloadFile("' + basePath + g_src.replace("png", "xls") + '","' + g_title + '","xls")\'>' + z("lang_Download_XLS").value + '</a></p>';
                }
                else if (g_vtype == "map") {
                    // Start links for MAP 
                    dtui += '<p id="mapLangLblDlPNG"><a href="javascript:void(0)" onclick=\'downloadFile("' + basePath + g_src + '","' + g_title + '","png")\'>' + z("lang_Download_Image").value + '</a></p>';
                    dtui += '<p id="mapLangLblDlXLS"><a href="javascript:void(0)" onclick=\'downloadFile("' + basePath + g_src.replace("png", "xls") + '","' + g_title + '","xls")\'>' + z("lang_Download_XLS").value + '</a></p>';
                    dtui += '<p id="mapLangLblDlKML"><a href="javascript:void(0)" onclick=\'downloadFile("' + basePath + g_src.replace("png", "kmz") + '","' + g_title + '","kml")\'>Download as KML</a></p>';
                }
                else if (g_vtype == "radar" || g_vtype == "scatter3d") {
                    // Start links for MAP 
                    dtui += '<p id="mapLangLblDlPNG"><a href="javascript:void(0)" onclick=\'downloadFile("' + basePath + g_src + '","' + g_title + '","png")\'>' + z("lang_Download_Image").value + '</a></p>';
                    dtui += '<p id="mapLangLblDlXLS"><a href="javascript:void(0)" onclick=\'downloadFile("' + basePath + g_src.replace("png", "xls") + '","' + g_title + '","xls")\'>' + z("lang_Download_XLS").value + '</a></p>';
                }
                else {
                    dtui += '<p id="vcLangLblDlPDF"><a href="javascript:void(0)" onclick=\'downloadFile("' + basePath + g_src.replace("png", "xls") + '","' + g_title + '","xls")\'>' + z("lang_Download_XLS").value + '</a></p>';
                 
                    dtui += '<p id="vcLangLblDlPNG"><a href="javascript:void(0)" onclick=\'downloadFile("' + basePath + g_src + '","' + g_title + '","png")\'>' + z("lang_Download_PNG").value + '</a></p>';
                    dtui += '<p id="vcLangLblDlSVG"><a href="javascript:void(0)" onclick=\'downloadFile("' + basePath + g_src.replace("png", "svg") + '","' + g_title + '","svg")\'>' + z("lang_Download_SVG").value + '</a></p>';
                }

                // End links for MAP	                
                dtui += '</div></div>';
                //dtui += '<div class="gallDescription" style="padding-top:18px;">' + g_desc + '</div>';
                if (g_vtype == "table" || g_vtype == "treemap" || g_vtype == "cloud") {
                    dtui += '<div class="gallBigimage"><div id="glBigImageId"><iframe src="' + basePath + g_shtml + '" frameborder="0" style="height:330px;width:698px"/></div></div>';
                }
                else {
                    dtui += '<div class="gallBigimage"><div id="glBigImageId"><img width="700px" height="326"  src="' + basePath + g_src + '"></div></div>';
                }
                dtui += '<div class="gallDescription gall_mod_txt" style="padding-bottom:0px;"><b>' + z('langGalleryModifiedOn').value + '</b> ' + g_date + '</div>';

                dtui += '<div class="gallDescription gallitemdescp"><b>' + z('langGalleryDescription').value + '</b>' + g_desc + '</div>';
                var KeywordString = g_key.replace(/\|\|/g, ", ");
                if (KeywordString.length > 100) {
                    KeywordString = KeywordString.substr(0, 96) + "...";
                }
                dtui += '<div class="gallDescription gall_keywd_txt" style="height:20px;padding-bottom:30px;" id="divGalKeyowrds"><b>' + z("langkeywords").innerHTML + '</b> ' + KeywordString + ' &nbsp;&nbsp; <a href="javascript:void(0)" onclick=\'moreKeywords("' + g_keyid + '")\' id="moreId">' + z('langGalleryMore').value + '</a></div>';

                /*dtui += '<div class="gall_bttn_lft_sec"><input type="button" value="View" onclick=\'openLiveVisualizer("' + basePath + g_shtml + '")\' class="submit_button">&nbsp<input id="galleryDownloadBtn" type="button" value="Download" onclick="" class="submit_button">&nbsp;<input type="button" value="Share" class="submit_button" onclick=\'shareGallery("' + basePath + g_shtml + '")\'></div>';*/
                dtui += '<div class="gall_bttn_rgt_sec" style="width:100%;"><input id="PopupBack" type="button" value="' + z("lang_ppup_Prev").value + '" onclick=\'gallPaging("pre", "' + nid + '","' + g_vtype + '","' + divEle.id + '")\' class="' + BackBtnClass + '">&nbsp<input id="PopupNext" type="button" value="' + z("lang_ppup_Next").value + '" onclick=\'gallPaging("next", "' + nid + '","' + g_vtype + '","' + divEle.id + '")\' class="' + NextBtnClass + '"></div>';

            

            });
di_jq('#popupContent').html(dtui);

            if (z('hdsby').value == "share") {
                //ImgUrl = "../stock/themes/default/images/dataview/";
                z("pop_viewimg").style.display = "none";
                z("galleryDownloadBtn").style.display = "none";
                z("pop_shareimg").style.display = "none";
                z("pop_deleteimg").style.display = "none";
                z("moreId").style.display = "none";
                z("divGalKeyowrds").style.display = "none";

            }
        }
        catch (ex) {
            
        }
    }
}

function deletePresentation(presId) {
    var confirmBox = confirm(z("delete_confirmation_txt").value);
    if (confirmBox == true) {
        var userLoginId = "";
        if (z('hLoggedInUserNId').value != "") {
            userLoginId = z('hLoggedInUserNId').value.split("|")[0];
        }
        if (userLoginId == "") {
            userLoginId = "-1";
            alert(z("delete_right_txt").value);
            return;
        }
        di_jq('#divShowLoadingText').html("Deleting...");
        di_jq('#divShowLoading').show();
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: { 'callback': 265, 'param1': userLoginId, 'param2': presId },
            async: false,
            success: function (data) {
                try {
                    if (data == "true") {
                        alert(z("delete_presentation_txt").value);
                        hideGalleryPopup();
                        di_qds_search_data();
                    }
                    else if (data == "false") {
                        alert(z("delete_right_txt").value);
                    }
                }
                catch (ex) {
                    alert("Error : " + ex.message);
                }
            },
            error: function () {
                
            },
            cache: false
        });
        di_jq('#divShowLoading').hide();
    }
}

/* function to toggle download option */
function toggleDownload() {
    di_jq('#toggleDownload').slideToggle();

}
function downloadFile(fileurl, filename, fileType) {
    
    var form = document.createElement("form");
    form.setAttribute("method", "post");
    form.setAttribute("action", "download.aspx");

    // export data
    var fileIdField = document.createElement("input");
    fileIdField.setAttribute("type", "hidden");
    fileIdField.setAttribute("name", "fileId");
    fileIdField.setAttribute("value", fileurl);
    form.appendChild(fileIdField);
    filename = removeInvalidCharacters(filename);
    // new filename
    var fileNameField = document.createElement("input");
    fileNameField.setAttribute("type", "hidden");
    fileNameField.setAttribute("name", "fileName");
    fileNameField.setAttribute("value", filename);
    form.appendChild(fileNameField);
    // new fileType
    var fileTypeField = document.createElement("input");
    fileTypeField.setAttribute("type", "hidden");
    fileTypeField.setAttribute("name", "fileType");
    fileTypeField.setAttribute("value", fileType);
    form.appendChild(fileTypeField);
    
    document.body.appendChild(form);
    form.submit();
}
function openLiveVisualizer(url) {
    window.open(url, '_blank');
}
/* function for paging */
function gallPaging(act, nid, vtype, divId) {
    var storeGallXmlData = '';
    var storeGallPage = [];
    var GalleryItemData = getGalleryDataFromGlobalStorage(divId);
    storeGallXmlData = GalleryItemData.data;
    storeGallPage = GalleryItemData.pagging;
    var indexVal = jQuery.inArray(nid, storeGallPage);
    var direct = "right";    
    if (indexVal > -1) {
        if (act == 'next') {
            if (indexVal == storeGallPage.length-1)
                return;
            indexVal = parseInt(indexVal + 1);
        }
        else {
            if (indexVal == 0)
                return;
            indexVal = parseInt(indexVal - 1);
        }
        if (indexVal > -1 && indexVal < storeGallPage.length) {

            callbackGalleryClick(null, storeGallPage[indexVal], "no", z(divId));

            if (act == 'pre') direct = "left";

            //di_jq('#popupContent').show("slide", { direction: direct }, 500);
            if (vtype == "table" || vtype == "treemap") {

                di_jq('#glBigImageId').show("slide", { direction: direct }, 10);


            }
            else {

                di_jq('#glBigImageId').show("slide", { direction: direct }, 800);
            }
        }
        z("PopupBack").className = "di_gui_button";
        z("PopupNext").className = "di_gui_button";                 
        if (indexVal == 0) {
            z("PopupBack").className = "di_gui_button_disabled";
            z("PopupNext").className = "di_gui_button";            
        }
        else if (indexVal == storeGallPage.length - 1) {
            z("PopupBack").className = "di_gui_button";
            z("PopupNext").className = "di_gui_button_disabled";                       
        }        
    }  

}
/* function to close popup */
function hideGalleryPopup() {

    RemoveMaskingDiv();
    di_jq('#PopupOuterBox').css("position", "absolute");
    var imageId = "#img_" + di_jq('#PopupOuterBox').attr('rel');
    var position = di_jq(imageId).position();
    var top = position.top + 50;
    var left = position.left + di_jq(imageId).width() - 50;
    di_jq('#PopupOuterBox').animate(
		{ "opacity": "hide", "top": top + "px", "left": left + "px", width: 0, height: 0},
		700,
		function () {
		    di_jq('#PopupOuterBox').removeAttr('style');
		}
	);

    //di_jq('#PopupOuterBox').hide('slow');
}
/* function to close more popup */
function hidePopupMore() {
    UpdateMaskZIndex(1000);
    di_jq('#PopupMoreBox').hide('slow');
}

/* function ro share gallery */
function shareGallery(url) {
    SocialSharing("You should check this out!", "DevInfo", url);
}

/* function ro share gallery */
function moreKeywords(keyids) {

    if (keyids != null && keyids != "") {
        try {
            ApplyMaskingDiv();
            UpdateMaskZIndex(1600);
            SetCloseButtonInPopupDiv(di_jq('#PopupMoreBox'), 'hidePopupMore');
            di_jq('#PopupMoreBox').show('slow');
            GetWindowCentered(z('PopupMoreBox'), 400, 300);
            var InputParam;
            InputParam = keyids;
            InputParam += ParamDelimiter + z('hlngcode').value;
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '43', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        var dtui = '';
                        var txt1, txt2, txt3, val1, val2, val3;
                        di_jq(parseXml(data)).find('ar').each(function () {
                            txt1 = di_jq(this).attr("txt");
                            val1 = di_jq(this).attr("val");
                        });
                        di_jq(parseXml(data)).find('ind').each(function () {
                            txt2 = di_jq(this).attr("txt");
                            val2 = di_jq(this).attr("val");
                        });
                        di_jq(parseXml(data)).find('udk').each(function () {
                            txt3 = di_jq(this).attr("txt");
                            val3 = di_jq(this).attr("val");
                        });

                        dtui = '<div class="gall_more_subpop">';

                        dtui += '<div class="gall_more_subpop_spc"></div>';
                        dtui += '<div class="popup_heading1">' + txt1 + '</div>';
                        dtui += '<div class="gallDescription">' + val1 + '</div>';
                        dtui += '<div class="gall_more_subpop_spc"></div>';

                        dtui += '<div class="popup_heading1">' + txt2 + '</div>';
                        dtui += '<div class="gallDescription">' + val2 + '</div>';
                        dtui += '<div class="gall_more_subpop_spc"></div>';

                        dtui += '<div class="popup_heading1">' + txt3 + '</div>';
                        dtui += '<div class="gallDescription">' + val3 + '</div>';
                        dtui += '<div class="gall_more_subpop_spc"></div>';

                        dtui += '<div>';

                        di_jq('#moreContent').html(dtui);
                    }
                    catch (ex) {
                        alert("message:" + ex.message);
                    }
                },
                error: function () {
                    
                },
                cache: false
            });
        }
        catch (ex) {
            
        }
    } // end if
}

/* function to show title*/
function showTitle(val) {
    if (val == true) {
        di_jq('#divQDSResults div').each(function () {
            di_jq(this).show();
        });
    }
    else {
        di_jq('#divQDSResults div').each(function () {
            di_jq(this).hide();
        });
    }
}

function text_keyword_blur() {
    var keywordTxtBoxVal = z("keyword_txt").value;
    if (keywordTxtBoxVal.indexOf("(") == 0 || keywordTxtBoxVal=="") {
        z("keyword_txt").value = "(" + z("langkeywords").innerHTML + ")";
        z("img_keyword_cross").style.display = "none";        
    }
}

function text_keyword_focus() {
    var keywordTxtBoxVal = z("keyword_txt").value;
    if (keywordTxtBoxVal.indexOf("(") == 0) {
        z("keyword_txt").value = "";
        z("img_keyword_cross").style.display = "none";        
    }
}

function ClearKeyword() {    
    z("keyword_txt").value="";
    z("keyword_txt").focus();
    z("img_keyword_cross").style.display = "none";
    SearchGallery('A');
}

function text_kewordPressout(event) {
    if (event.keyCode == 13) {
        //Enter key handling, call search button click method
        GetGalleryResultByKeywords(GalleryFilter);
        return;
    }
    z("img_keyword_cross").style.display = "";
}

function GetKeywordsText() {
    var keywords = "null";
    var delemeter = "||";
    var keywordTxtBoxVal = z("keyword_txt").value;
    if (keywordTxtBoxVal.indexOf("(") != 0 && keywordTxtBoxVal != "") {
        if (keywordTxtBoxVal.indexOf(",") > -1) {
            var keywordList = keywordTxtBoxVal.split(",");
            for (var i = 0; i < keywordList.length; i++) {
                keywords += delemeter + keywordList[i];
            }
        }
        else {
            keywords = keywordTxtBoxVal;
        }
    }
    return keywords;
}


function GetGalleryResultByKeywords(GalleryItemType) {
    ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div', z('hdbnid').value, z('hlngcodedb').value, z('langQdsIndCaption').value, z('langQdsAreaCaption').value, '100%', '100px', 'GetQDSResult', "", "", "", "", false, z('langQdsSerchBtnText').value, z('langQdsAreaBlockText').value);
    if (GalleryItemType == null)
        GalleryItemType = GalleryFilter;
    var InputParam;
    var spanLoading, spanLoadingOuter, tblQDSResults, divQDSResults, spanTimeCounter, glry_LoadingOuter;
    glry_LoadingOuter = z('glry_LoadingOuter');
    spanLoading = z('spanLoading');
    spanLoadingOuter = z('spanLoadingOuter');
    divQDSResults = z('divQDSResults');
    tblQDSResults = z('tblQDSResults');
    spanTimeCounter = z('spanTimeCounter');

    //if ((IndicatorIC != null && IndicatorIC != "") || (Areas != null && Areas != "")) {
    spanLoadingOuter.style.display = "block";
    spanLoading.innerHTML = "Searching the gallery...";
    divQDSResults.innerHTML = "";
    spanTimeCounter.innerHTML = "";

    InputParam = di_qds_get_all_ind_block_text();
    InputParam += ParamDelimiter + "";
    InputParam += ParamDelimiter + "";
    InputParam += ParamDelimiter + z('hlngcode').value;
    InputParam += ParamDelimiter + z('hdbnid').value;
    InputParam += ParamDelimiter + GalleryItemType;
    InputParam += ParamDelimiter + GetKeywordsText();
    var userLoginId = "";
    if (z('hLoggedInUserNId').value != "") {
        userLoginId = z('hLoggedInUserNId').value.split("|")[0];
    }
    if (userLoginId == "")
        userLoginId = "-1";

    InputParam += ParamDelimiter + userLoginId;
    InputParam += ParamDelimiter + GetKeywordsText();
    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '41', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    if (data.split(ParamDelimiter)[0] != null && data.split(ParamDelimiter)[0] != '') {
                        tblQDSResults.style.display = "";
                        if (GalleryItemType == 'A') {
                            z('aGalleryAll').className = "navActive";
                        }
                    }
                    var TotalRecords = data.split(ParamDelimiter)[1];
                    var TimeTakenInSecs = data.split(ParamDelimiter)[2];
                    PageNumber = 1;
                    callbackGalleryInit(data.split(ParamDelimiter)[0], 'divQDSResults');
                    spanTimeCounter.innerHTML = z('langGalleryResults').value + " : " + TotalRecords + ", " + z('langGalleryTimeTaken').value + " : " + TimeTakenInSecs + " " + z('langGallerySeconds').value;

                    di_qds_resize_ind_blocks();
                    di_qds_resize_area_blocks();

                    spanLoadingOuter.style.display = "none";
                    spanLoading.innerHTML = "";
                    glry_LoadingOuter.style.display = "none";
                }
                catch (ex) {
                    alert("message:" + ex.message);
                }
            },
            error: function () {
                
            },
            cache: false
        });
    }
    catch (ex) {
    }    
}

function ApplyFilterFormat(obj) {
    var elementsArr = ["aGalleryAll", "aGalleryTables", "aGalleryGraphs", "aGalleryMaps"];
    var colorCode;
    for (var i = 0; i < elementsArr.length; i++) {
        colorCode = "#666666";
        if (elementsArr[i] == obj.id) {
            colorCode = "#0F78BC";
        }        
        z(elementsArr[i]).style.color = colorCode;
    }
}

function SetKeywordString(data) {
    g_key = data;
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