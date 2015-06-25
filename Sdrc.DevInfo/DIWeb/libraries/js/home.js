var homeSliderData = [];
var DbAdmName = "";
var DbAdmInstitution = "";
var DbAdmEmail = "";
var isQdsSearching = false;

// set the variables and the page events
function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, dbname, dbsummary, dbdesc, dbareacount, hLoggedInUserNId, hLoggedInUserName, hIsGalleryEnabled, hdvnids) {
    try {
        var hLoggedInUserNIdPart = "";
        if (z("aAdminPanel").style.display == "none") {

            if (hLoggedInUserNId != null && hLoggedInUserNId != '') {
                if (hLoggedInUserNId.split('|').length > 1) {
                    hLoggedInUserNIdPart = hLoggedInUserNId.split('|')[0];
                }
                else {
                    hLoggedInUserNIdPart = hLoggedInUserNId;
                }
                if (CheckIfSiteAdminLoggedIn(hLoggedInUserNIdPart) == 'true') {
                    z("aAdminPanel").style.display = "inline";
                    z("liAdminPanel").style.display = "inline";
                }
            }

        }
        if (z('aUser').style.display == "none") {
            if (hLoggedInUserNId != null && hLoggedInUserNId != '') {
                if (hLoggedInUserNId.split('|').length > 1) {
                    hLoggedInUserNIdPart = hLoggedInUserNId.split('|')[0];
                }
                else {
                    hLoggedInUserNIdPart = hLoggedInUserNId;
                }
                if (CheckIfMasterAccount(hLoggedInUserNIdPart) != 'true') {
                    z('aUser').innerHTML = hLoggedInUserName;
                    z('aUser').style.display = "inline";
                    z('liUser').style.display = "inline";
                }
            }            
        }
        ShowLoadingDiv();

        isSearchResultsGalleryEnabled = (hIsGalleryEnabled === "true");
        var hsgcount = 10;
        // Set the active selection to Data Search Navigation link    
        di_jq("#aHome").attr("class", "navActive")

        // Hide background color div
        di_jq("#apDiv3").css("display", "none");

        // Set default area count of selected database
        z("lblDefalutAreaCount").innerHTML = dbareacount;
        //LoadLanguage(hlngcode);


        // ************************************************
        // create Form Tag with hidden input boxes
        // ************************************************
        createFormHiddenInputs("frmHome", "POST");
        SetCommonLinksHref("frmHome", "POST");

        // ************************************************
        // set page level variables based on selections or defaults
        // ************************************************
        setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName, hdvnids);

        // ************************************************
        // Load Language Component
        // ************************************************
        if (GetLanguageCounts() > 1) {
            z("LanguagePipeLi").style.display = "";
            ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);
        }

        // ************************************************
        // Database component 
        // ************************************************
        //InitializeDatabaseComponent(getAbsURL('stock'), 'database_div', hdbnid, hlngcodedb, 550, 460, false, "OkDbComponent", "CloseDbComponent");		
        //DrawDatabaseDetailsComponent(hdbnid, "db_details_div");
        //hlngcode = GetBrowserLanguage();
        //alert(hlngcode);

        if (readCookie('SlideStartIndex') == null) {
            createCookie('SlideStartIndex', 0);
        }

        // start for home page slider
        CreateSliderData(hlngcode);

        HideLoadingDiv();

        z('hselindo').value = "";
        z('hselareao').value = "";

        z('hselind').value = "";
        z('hselarea').value = "";
        z('hdsby').value = "qds";
        var InputParam;
        InputParam = getAbsURL('stock') + "themes/default/images/cust/logo.png";        
        //Create an entry in Global Catalog for this adaptation
        try {
            di_jq.ajax({
                type: "POST",
                url: "Callback.aspx",
                data: { 'callback': '1014', 'param1': InputParam },
                async: false,
                success: function (data) {
                },
                error: function () {
                    
                },
                cache: false
            });
        }
        catch (err) { }
// Till here
    }
    catch (err) { }
}

// PlayPause Button Position	
function rtlPlayPauseBPos() {
    try {
        z("pp_bttn").style.left = ((VisibleSlidersCount * 17) + 20) + "px";
    }
    catch (err) { }
}

function CreateSliderData(langVal) {
    var SliderXmlFile;
    var icount = 0;
    var StartIndex = 0;
    var StoreNextStartIndex;
    var TotalSlidersCount;
    var i_data = "";

    try {

        SliderXmlFile = getAbsURL('stock') + "Adaptation/slider_html/" + langVal + "/slidermaster.xml" + "?v=" + AppVersion;

        di_jq.ajax({
            type: "GET",
            url: SliderXmlFile,
            dataType: "xml",
            async: false,
            success: function (xml) {

                var Sliders = di_jq(xml).find("item");
                TotalSlidersCount = Sliders.length;

                StartIndex = parseInt(readCookie('SlideStartIndex'));

                if (StartIndex + 1 >= TotalSlidersCount) {
                    StartIndex = 0;
                }

                if (VisibleSlidersCount > TotalSlidersCount) {
                    VisibleSlidersCount = TotalSlidersCount;
                }

				if(VisibleSlidersCount==TotalSlidersCount) {
					StartIndex = 0;
				}

                homeSliderData = [];

                for (var i = StartIndex; i < TotalSlidersCount; i++) {
                    if (icount < VisibleSlidersCount) {

                        if (GetRunningBrowserName() == "Internet Explorer") {
                            i_data = Sliders[i].text;
                        }
                        else {
                            i_data = Sliders[i].textContent;
                        }

                        homeSliderData[icount] = { data: i_data };
                        StoreNextStartIndex = i;
                        icount++;

                        if (icount < VisibleSlidersCount && i + 1 == TotalSlidersCount) {
                            i = -1;
                        }
                    }
                }

                createCookie('SlideStartIndex', StoreNextStartIndex + 1);
            }
        });
		//homeSliderData[0] = { data: 'slide_01.htm' };
    }
    catch (ex) { }
}

/* function to render in slider view */
function renderSliderData(sdata, act, fc) {
    var returnData = '';
    var HTMLData;
    var SliderHtmlFileName = "";

    try {
        for (var i = 0; i < sdata.length; i++) {
            if (act == "html") {
                returnData += '<li><div class="li_img_pos" id="li_img_ld_isue' + i + '"></div></li>';
            }
            else if (act == "data") {

                HTMLData = sdata[i].data;
                //                HTMLData = HTMLData.replace("xx", readCookie('hlngcode'));

                //                if (isNaN(fc)) {
                //                    di_jq('#li_img_ld_isue' + i).html(HTMLData);
                //                }
                //                else {
                //                    di_jq('#li_img_ld_isue' + fc).html(HTMLData);
                //                }

                SliderHtmlFileName = getAbsURL('stock') + "Adaptation/slider_html/" + readCookie('hlngcode') + "/" + HTMLData + "?v=" + AppVersion;

                di_jq.ajax({
                    type: "GET",
                    url: SliderHtmlFileName,
                    dataType: "text",
                    async: false,
                    success: function (data) {
                        if (data != "") {

                            data = data.replace("xx", readCookie('hlngcode'));

                            if (isNaN(fc)) {
                                di_jq('#li_img_ld_isue' + i).html(data);
                            }
                            else {
                                di_jq('#li_img_ld_isue' + fc).html(data);
                            }
                        }
                    }
                });

            } // end else

        } // end for
    } catch (err) { }

    return returnData;
}

function SetSliders() {
    var res = 1;
    var currentLocation = "";
    var reData = [];

    try {
        // render slider html
        di_jq('#slider1').html(renderSliderData(homeSliderData, 'html'));

        // find out witch slide image to be loaded        
        currentLocation = window.location.href.split("#")[1];
        if (currentLocation != undefined && currentLocation != '' && currentLocation != null) {
            res = currentLocation.split("-")[1];
        }
        
        reData[0] = homeSliderData[eval(res - 1)];
        renderSliderData(reData, 'data', eval(res - 1));
        SliderAnimationSpeed = SliderAnimationSpeed * 1000;
    
        $('#slider1').anythingSlider({
            theme: 'metallic',
            easing: 'easeInOutBack',
			delay: SliderAnimationSpeed,//6000,
            //autoPlayLocked  : true,  // If true, user changing slides will not stop the slideshow
            //resumeDelay     : 10000, // Resume slideshow after user interaction, only if autoplayLocked is true (in milliseconds).
			onSlideComplete: function (slider) {
			    
                renderSliderData(homeSliderData, 'data');
            }
        });
    }
    catch (err) { }
}

function GetDatabaseInfoHTML(dbNId, lngCode, divId) {

    var HTMLStr = "";
    var DbXmlFile;
    var SearchPatern;
    var DbDescriptionAttribute;
    var DbDescription = "";
    var CountsArr;
    var AdaptationYear;
    try {

        
        DbXmlFile = getAbsURL('stock') + "db.xml" + "?v=" + AppVersion + "&rd=" + Math.floor((Math.random() * 10) + 1);
        

        di_jq.ajax({
            type: "GET",
            url: DbXmlFile,
            dataType: "xml",
            async: true,
            success: function (xml) {
                SearchPatern = 'db[id=' + dbNId + ']';
                DbDescriptionAttribute = "desc_" + lngCode;
                di_jq(xml).find(SearchPatern).each(function () {

                    HTMLStr = "<h1>" + di_jq(this).attr("n") + "</h1>";

                    HTMLStr += "<p class='para_txt'>";

                    DbDescription = di_jq(this).attr(DbDescriptionAttribute);

                    if (DbDescription != "") {
                        HTMLStr += DbDescription + "<br />";
                    }

                    var CountsArr = di_jq(this).attr("count").split("_");

                    HTMLStr += "<span>" + z('langIndicators').value + " " + CountsArr[1] + " " + z('langAreasColon').value + " " + CountsArr[0] + " " + z('langSourcesColon').value + " " + CountsArr[2] + " " + z('langDataValues').value + " " + CountsArr[3] + " " + z('langUpdatedOn').value + " " + di_jq(this).attr("lastmod") + "</span></p>";
                    /*call method for getting html to create downlink. this method is written in common.js file*/
                    var DownloadCSVHtml = CreateLinkForDownloadCSV('', null,'','');
                    if (DownloadCSVHtml != null && DownloadCSVHtml != '') {
                        HTMLStr += DownloadCSVHtml;
                    }
                    if (DbAdmName != '' || DbAdmInstitution != '' || DbAdmEmail != '') {
                        HTMLStr += "<p class='para_txt' style='padding-top:8px;'><b>" + z('langLblContactDbAdm').value + "</b></p>";

                        if (DbAdmName != '') {
                            HTMLStr += "<p class='para_txt para_txt_spc'><b>" + z('langLblDbAdmName').value + "</b> " + DbAdmName + "</p>";
                        }

                        if (DbAdmInstitution != '') {
                            HTMLStr += "<p class='para_txt para_txt_spc'><b>" + z('langLblDbAdmInstitution').value + "</b> " + DbAdmInstitution + "</p>";
                        }

                        if (DbAdmEmail != '') {
                            HTMLStr += "<p class='para_txt para_txt_spc'><b>" + z('langLblDbAdmEmail').value + "</b> <a href='mailto:" + DbAdmEmail + "' >" + DbAdmEmail + "</a> </p>";
                        }
                    }

                    // set html into div
                    $("#" + divId).html(HTMLStr);
                });
            },
            error: function () {
                alert("error in ajax call.");
            },
            cache: true
        });
    }
    catch (ex) { }
}

function GetSearchResults(AreaNIds, IndicatorNIds, IC_NIds) {
    
    if (isQdsSearching) {
        return;
    }
    else {
        isQdsSearching = true;
    }
    
    var spanLoading, spanLoadingOuter, divQDSResults, spanTimeCounter;
    var InputParam = '';
    var userLoginId = ""; //gallery Existence
    if (z('hLoggedInUserNId').value != "") {
        userLoginId = z('hLoggedInUserNId').value.split("|")[0];
    }
    if (userLoginId == "")
        userLoginId = "-1";
    //End of Code added to check for gallery Existence
    try {
        spanLoading = z('spanLoading');
        spanLoadingOuter = z('spanLoadingOuter');
        divQDSResults = z('divQDSResults');
        spanTimeCounter = z('spanTimeCounter');

        InputParam = IndicatorNIds;
        InputParam += ParamDelimiter + IC_NIds;
        InputParam += ParamDelimiter + AreaNIds;
        InputParam += ParamDelimiter + z('hlngcode').value;
        InputParam += ParamDelimiter + z('hdbnid').value;
        InputParam += ParamDelimiter + 'false';
        InputParam += ParamDelimiter + ''; // Host application's URL
        InputParam += ParamDelimiter + userLoginId; // Added to check for Gallery Existence


        StopGalleryLoadingProcess = true;
        z('glry_LoadingOuter').style.display = 'block';
        z('divTimeCounter').style.display = 'block';
        spanLoadingOuter.style.display = "inline";
//                spanLoading.innerHTML = "Searching the database...";
        spanLoading.innerHTML = z('langSearchingDatabase').value;
        divQDSResults.innerHTML = "";
        //z('divDatabaseFullInfo').style.display = 'none';

        spanTimeCounter.style.display = 'block';
        spanTimeCounter.innerHTML = '';

        z('spanAddAllToCartMain').style.display = 'none';
        z('spanRemoveAllFromCartMain').style.display = 'none';

   
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '4', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    isQdsSearching = false;
                    var TimeTaken = data.split(ParamDelimiter)[0];
                    var ResultsData = data.split(ParamDelimiter)[1];
                    var NID_GalleryXML = data.split(ParamDelimiter)[3]; //Added for Simul Gallery HomePage

                    //For gallery Existence
                    //var GalleryExistence = data.split(ParamDelimiter)[2];
//                    if (isSearchResultsGalleryEnabled) {
//                        if (GalleryExistence=="False")
//                        { isSearchResultsGalleryEnabled = false; }
//                    }
                    //End of Code added to check for gallery Existence
                    var jsonData = di_jq.parseJSON(ResultsData);

                    z(divQDSResults.id).style.display = 'block';

                    RenderTimeTaken(jsonData[""].length, TimeTaken);

                    RenderResults(jsonData, divQDSResults.id, spanLoadingOuter.id, spanLoading.id, false, TimeTaken, "", NID_GalleryXML); //Added last parameter for Simul Gallery HomePage


                }
                catch (ex) {
                    RenderTimeTaken(0, 0);
                    divQDSResults.innerHTML = "";
                    spanLoadingOuter.style.display = "none";
                    spanLoading.innerHTML = "";
                }
            },
            error: function () {
                isQdsSearching = false;
                //
            },
            cache: false
        });
    }
    catch (ex) {
    }
    }

/* Quick data search hot selection function */
function qdsHotSelection(autosearch) {
    var InputParam, IndicatorIC, Areas;
    var spanLoading, spanLoadingOuter, divQDSResults, spanTimeCounter;

    try {

        // Hiding banner on search button
//          if (readCookie('ShowSliders') == "true") {
//            createCookie('ShowSliders', "false");
//        }
        //        SetShowSliderStatus();
        if (autosearch !=false) {
            di_jq("#sh_slider").hide("slow");
            di_jq("#but_sh_img_exp_down").css("display", "block");
            di_jq("#but_sh_img_colp_up").css("display", "none");
            di_jq("#but_sh").html(di_jq("#langShow").val());
        }

        IndicatorIC = di_get_selected_indicators_values();
        Areas = di_get_selected_areas_values(); //di_get_selected_areas_block_details(); //  // CHANGES

        //#region Storing QDS box selections in cookies(cache)
        var arrTopics = di_get_selected_ind_block_det();
        var strTopics = '';
        if (arrTopics.length > 0) {
            for (var i = 0; i < arrTopics.length; i++) {
                strTopics += di_qds_blockTextDelimiter + arrTopics[i];
            }
            strTopics = encodeURIComponent(strTopics.replace(di_qds_blockTextDelimiter, ''));
        }

        var arrAreas = di_get_selected_area_block_det();
        var strAreas = '';
        if (arrAreas.length > 0) {
            for (var i = 0; i < arrAreas.length; i++) {
                strAreas += di_qds_blockTextDelimiter + arrAreas[i];
            }
            strAreas = encodeURIComponent(strAreas.replace(di_qds_blockTextDelimiter, ''));
        }
        //#endregion

        createCookie('QdsBoxAreas', strAreas);
        createCookie('QdsBoxTopics', strTopics);

        if ((IndicatorIC != null && IndicatorIC != "") || (Areas != null && Areas != "")) {
            GetSearchResults(
                Areas,
                Get_Indicators_from_components_IndicatorIC(IndicatorIC),
                Get_IC_from_components_IndicatorIC(IndicatorIC)
            );
        }
    }
    catch (err) { }
}

function qdsHotClearance() {
    try {
        var divResults = z('divQDSResults');
        while (divResults.hasChildNodes()) {
            divResults.removeChild(divResults.lastChild);
        }
        z('spanAddAllToCartMain').style.display = 'none';
        z('spanRemoveAllFromCartMain').style.display = 'none';
        z('spanMultipleDbSearchMain').style.display = 'none';
        z('spanTimeCounter').style.display = 'none';

        //Added to clear cookies on clearing QDS box
//        var strAreaCookie = readCookie('QdsBoxAreas');
//        var strIndCookie = readCookie('QdsBoxTopics');
//        createCookie('QdsBoxAreas', "");
//        createCookie('QdsBoxTopics', "");
    }
    catch (err) {
        var errMsg = err.Message;
    }
}

function AdvancedSearch() {
    try {
        z('hdsby').value = "as";
        z('hselindo').value = di_qds_get_ind_search_data() + "[****]" + "";
        z('hselareao').value = di_qds_get_area_search_data() + "[****]" + "";
        PostData("frmHome", "QDS.aspx", "POST");
    }
    catch (err) { }
}

/* function to open Video Popup page */
function videoPopup() {
    try {
        ApplyMaskingDiv();

        GetWindowCentered(z('VideoPopupOuterBox'), 520, 440);

        ShowLoadingDiv();
        var params = {};
        // params.quality = "high";
        // params.bgcolor = "#ffffff";
        // params.allowscriptaccess = "sameDomain";
        params.allowfullscreen = "true";
        params.allowScale = "false";
        // params.wmode = "transparent";
        swfobject.embedSWF("../../stock/themes/default/images/devinfo_video.swf", "flashcontent", "500", "420", "10.0.0", "playerProductInstall.swf", null, params, null);

        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#VideoPopupOuterBox'), 'closevideoPopup');

        di_jq('#VideoPopupOuterBox').show();
    }
    catch (err) { }
}

/* function to close to Video Popup page */
function closevideoPopup() {
    try {
        di_jq('#VideoPopupOuterBox').hide('slow');
        RemoveMaskingDiv();
        HideLoadingDiv();
    }
    catch (err) { }
}

/* function to go on Area selection page */
function doAreaSelection() {
    try {
        di_jq("#popup_sel_areas").hide();

        // empty area tab html first
        //di_jq('#area_div').html("");

        ApplyMaskingDiv();

        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#AreaOuterBox'), 'hidePopupSelection');

        di_jq('#AreaOuterBox').show('slow');
        GetWindowCentered(z('AreaOuterBox'), 767, 520);

        //if(di_jq('#area_div_popup').html()=='') {
        ShowAreaComponent(getAbsURL('stock'), 'area_div_popup', z("hdbnid").value, z("hlngcodedb").value, '', '767', '400', 'tree');
        di_loading('hide');
        //}	
    }
    catch (err) { }
}

/* function to go on Area selection page */
function doIndicatorSelection() {
    try {
        // empty area tab html first
        di_jq('#indicator_div').html("");

        ApplyMaskingDiv();

        di_jq('#IndOuterBox').show('slow');

        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#IndOuterBox'), 'hidePopupSelection');

        GetWindowCentered(z('IndOuterBox'), 767, 520);
        //if(di_jq('#indicator_div_popup').html()=='') {
        ShowIndicatorComponent(getAbsURL('stock'), 'indicator_div_popup', z("hdbnid").value, z("hlngcodedb").value, '', '767', '400', z("hsgcount").value, 'tree', false, false);
        di_loading('hide');
        //}	
    }
    catch (err) { }
}

/* function to go to DataView page when click on "ok" button of area op-up */
function onGotoQDSResult(type) {
    try {
        // Set selected areas and selected indicator
        var delemeter = "||{~~}||";

        ClearAllIndBlocks();
        ClearAllAreaBlocks();

        var AreaNIds = '';
        var IndicatorNIds = '';

        if (type == 'area') {

            //var areaData = di_get_selected_areas().split(delemeter); // getting area data from the Area component
            var areaData = AreaCompObj.getSelectedAreas().split(delemeter);
            z('hselarea').value = areaData[0];
            z('hselareao').value = areaData[1];
          
            AreaNIds = areaData[0];
            
            if (AreaNIds == '') {
                alertMessage('Please select at least one Area.');
                return false;
            }
            else {
                z('hselind').value = '';
                z('hselindo').value = '';
            }
        }
        else if (type == 'ind') {
			//var selectedline = IndCompObj.getSelectedData().replace(/\\'/g,"''");
            //var indData = selectedline.split(delemeter); // getting area data from the Area component
			var selectedline = IndCompObj.getSelectedData();
            var indData = selectedline.split(delemeter); // getting area data from the Area component
            z('hselind').value = indData[0];
            z('hselindo').value = indData[1];

            var tmpIUs = '';

            tmpIUs = di_jq.parseJSON(indData[1]);

            for (var i = 0; i < tmpIUs.iu.length; i++) {
                IndicatorNIds += ',' + tmpIUs.iu[i].split('~')[0];
            }

            if (IndicatorNIds.length > 0) IndicatorNIds = IndicatorNIds.substr(1);

            if (IndicatorNIds == '') {
                alertMessage('Please select at least one Topic.');
                return false;
            }
            else {
                z('hselarea').value = '';
                z('hselareao').value = '';
            }

        }

        hidePopupSelection();
      
        GetSearchResults(AreaNIds, IndicatorNIds, '');
        
        return true;

    }
    catch (err) { }
}

/* function to hide pop */
function hidePopupSelection() {
    try {
        //	di_jq('#GradOutDiv').hide();
        RemoveMaskingDiv();
        di_jq('#AreaOuterBox').hide('slow');
        di_jq('#IndOuterBox').hide('slow');
        di_jq('#advSearchOuterBox').hide('slow');
        di_loading('hide');
    }
    catch (err) { }
}

/* function to go on Indicator selection page */
function onGotoIndSelectionPg() {
    try {
        // Set page selection data in hidden text boxes
        z('hdsby').value = "area";
        //z('hselarea').value = AreaCompObj.getSelectedAreas();

        //var sAreaSel = di_get_selected_areas().split("||{~~}||");
        var sAreaSel = AreaCompObj.getSelectedAreas().split("||{~~}||");
        z('hselarea').value = sAreaSel[0];
        z('hselareao').value = sAreaSel[1];

        var sIndSel = IndCompObj.getSelectedData().split("||{~~}||");
        z('hselind').value = sIndSel[0];
        z('hselindo').value = sIndSel[1];

        // Post the data
        PostData("frmHome", "selection.aspx", "POST");
    }
    catch (err) { }
}

/* function to go on View Data page */
function onGotoDataViewPg() {
    try {
        // Set selected areas and selected indicator
        var delemeter = "||{~~}||";
        var areaVals = '';
        var indVals = '';

        z('hdsby').value = "as";

        //var areaData = di_get_selected_areas().split(delemeter); // getting area data from the Area component
        if (AreaCompObj != '' && AreaCompObj != undefined) {
            var areaData = AreaCompObj.getSelectedAreas().split(delemeter);
            areaVals = areaData[0];
            z('hselarea').value = areaData[0];
            z('hselareao').value = areaData[1];
        }

        if (IndCompObj != '' && IndCompObj != undefined) {
            var indData = IndCompObj.getSelectedData().split(delemeter); // getting area data from the Area component
            indVals = indData[0];
            z('hselind').value = indData[0];
            z('hselindo').value = indData[1];
        }

        if (indVals == '' && areaVals == '') {
            // alert message
            alertMessage('Please select indicator or area');
            return false;
        }
        else {
            // Post the data
            PostData("frmHome", "dataview.aspx", "POST");
        }
    }
    catch (err) { }
}

function showNavTab(tab) {
    try {
        if (tab == 'ar') {
            //z('lf_cbt_sec').style.display='none';
            z('tab_indicator').style.display = 'none';
            z('tab_area').style.display = 'block';
            //z('tab_area_selected').setAttribute("class", 'tab_panel_selected');
            //z('tab_ind_selected').setAttribute("class", 'tab_panel');
            di_jq('#tab_area_selected').attr('class', 'tab_panel_selected');
            di_jq('#tab_ind_selected').attr('class', 'tab_panel');

            if (z("area_div").innerHTML.trim() == '') {
                //z("area_div_popup").innerHTML == '';
                ShowAreaComponent(getAbsURL('stock'), 'area_div', z("hdbnid").value, z("hlngcodedb").value, '', '100%', '420', 'tree');
            }
        }
        if (tab == 'ind') {
            //z('lf_cbt_sec').style.display='block';
            z('tab_area').style.display = 'none';
            z('tab_indicator').style.display = 'block';
            //z('tab_ind_selected').setAttribute("class", 'tab_panel_selected');
            //z('tab_area_selected').setAttribute("class", 'tab_panel');
            di_jq('#tab_ind_selected').attr('class', 'tab_panel_selected');
            di_jq('#tab_area_selected').attr('class', 'tab_panel');

            if (z("indicator_div").innerHTML.trim() == '') {
                z("indicator_div_popup").innerHTML == '';
                ShowIndicatorComponent(getAbsURL('stock'), 'indicator_div', z("hdbnid").value, z("hlngcodedb").value, '', '700', '420', z("hsgcount").value, 'list', true, true);
            }
        }
    }
    catch (err) { }
}

function ShowSelectedAreas() {
    var InputParam;
    var AreaNamesArr;
    var i = 0;
    var LIStr = '';

    try {
        ApplyMaskingDiv();
        ShowLoadingDiv();

        InputParam = z("hdbnid").value;
        InputParam += ParamDelimiter + readCookie('hlngcodedb');

        di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: { 'callback': '27', 'param1': InputParam },
            async: false,
            success: function (data) {
                if (data != "false") {
                    //Set close button at right corner of popup div
                    SetCloseButtonInPopupDiv(di_jq('#popup_sel_areas'), 'CloseSelectedAreas');

                    //Set area count                          
                    di_jq("#popup_sel_areas_count").html(z("lblDefalutAreaCount").innerHTML + " default areas selected");
                    //Creaet area names li string
                    AreaNamesArr = data.split(", ");

                    LIStr += "<ul class='popup_d_areas_listin'>";
                    for (i = 0; i < AreaNamesArr.length; i++) {
                        LIStr += "<li>" + AreaNamesArr[i] + "</li>";
                    }
                    LIStr += "</ul>";

                    //Set area names li
                    di_jq("#popup_sel_areas_names").html(LIStr);

                    ApplyMaskingDiv(1600);
                    di_jq("#popup_sel_areas").css('z-index', 1700);

                    di_jq("#popup_sel_areas").show("slow");

                    //Set popup window at center of screen        
                    GetWindowCentered(z('popup_sel_areas'), 430, 346);

                    HideLoadingDiv();
                }
            },
            error: function () {
//                
            },
            cache: false
        });
    }
    catch (err) { }
}

function CloseSelectedAreas() {
    try {
        di_jq("#popup_sel_areas").hide("slow");
        //RemoveMaskingDiv();
        ApplyMaskingDiv(1200);
    }
    catch (err) { }
}

function GetDefaultAreaCountOfSelDb() {
    // Set default area count of selected database
    //z("lblDefalutAreaCount").innerHTML = dbareacount;

    var RetVal = "";
    var InputParam;

    try {
        InputParam = z("hdbnid").value;

        di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: { 'callback': '29', 'param1': InputParam },
            async: false,
            success: function (data) {
                if (data != "false") {
                    RetVal = data;
                }
            },
            error: function () {
                
            },
            cache: false
        });
    }
    catch (err) { }

    return RetVal;
}


function onGotoQdsPageForIndAreaComponent() {
    var delemeter = "||{~~}||";
    var i = 0;
    var j = 0;

    var SelIndicators = "";
    var IndData;
    var IndObj;
    var IndDataArr;
    var IndDataDetailArr;
    var IndNid = "";
    var IndText = "";
    var IndBlockData = "";

    var SelAreas = "";
    var AreaData;
    var AreaObj;
    var AreaDataArr;
    var AreaLevelDataArr;
    var AreaDataDetailArr;
    var AreaNid = "";
    var AreaText = "";
    var AraaBlockData = "";

    try {
        // Create indicator	block data
        SelIndicators = IndCompObj.getSelectedData();
        if (SelIndicators != delemeter) {
            IndData = SelIndicators.split(delemeter); // getting indicator data from the indicator component
            IndObj = di_jq.parseJSON(IndData[1]);
            IndDataArr = IndObj.iu;

            for (i = 0; i < IndDataArr.length; i++) {
                IndDataDetailArr = IndDataArr[i].split("||");
                IndNid = IndDataDetailArr[0].split("~")[0];
                IndText = IndDataDetailArr[1];
                IndBlockData += "||" + IndNid + "_1" + "~~" + IndText;
            }

            if (IndBlockData != "") {
                IndBlockData = IndBlockData.substr(2);
            }
        }


        // Create area block data
        //SelAreas = di_get_selected_areas();
        SelAreas = AreaCompObj.getSelectedAreas();
        if (SelAreas != delemeter) {
            AreaData = SelAreas.split(delemeter); // getting area data from the Area component
            AreaObj = di_jq.parseJSON(AreaData[1]);
            AreaDataArr = AreaObj.area;

            i = 1;
            while (true) {
                AreaLevelDataArr = AreaDataArr[i];

                if (AreaLevelDataArr == undefined) {
                    break;
                }

                j = 0;
                while (true) {
                    if (AreaLevelDataArr[j] == undefined) {
                        break;
                    }

                    AreaDataDetailArr = AreaLevelDataArr[j].split("||");
                    AreaNid = AreaDataDetailArr[0];
                    AreaText = AreaDataDetailArr[1];
                    AraaBlockData += "||" + AreaNid + "~~" + AreaText;
                    j++;
                }

                i++;
            }

            if (AraaBlockData != "") {
                AraaBlockData = AraaBlockData.substr(2);
            }
        }

        z('hdsby').value = "qds";
        z('hselindo').value = "" + "[****]" + IndBlockData;
        z('hselareao').value = "" + "[****]" + AraaBlockData;
        PostData("frmHome", "DataView.aspx", "POST");
    }
    catch (err) { }
}

/* function to open advance selection pop-up for the indicator&area component */
function doAdvanceSelection() {
    try {
        z('lf_cbt_sec').style.display = 'none';

        di_jq('#indicator_div_popup').html(""); // empty indicator pop-up
        di_jq('#area_div_popup').html(""); // empty area pop-up

        ApplyMaskingDiv();
        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#advSearchOuterBox'), 'hidePopupSelection');

        di_jq('#advSearchOuterBox').show('slow');
        GetWindowCentered(z('advSearchOuterBox'), 700, 620);

        //if(di_jq('#indicator_div_popup').html()=='') {
        if (di_jq('#tab_area_selected').attr('class') != 'tab_panel_selected') {
            var json = '{"iu":[],"sg_dim":{},"sg_dim_val":{},"iusnid":{}}';
            ShowIndicatorComponent(getAbsURL('stock'), 'indicator_div', z("hdbnid").value, z("hlngcodedb").value, json, '100%', '420', z("hsgcount").value, 'list', true, true);
        }
        //}

        //if(di_jq('#area_div_popup').html()=='') {
        //var ajson = '{"area" : {}}';
        //ShowAreaComponent( getAbsURL('stock'), 'area_div', z("hdbnid").value, z("hlngcodedb").value, ajson, '700', '410', 'tree');
        //di_loading('hide');
        //}	
    }
    catch (err) { }
}


function ShowHideSlider() {
    try {
        di_jq("#sh_slider").slideToggle("slow", function () {
            //di_jq("#but_sh").html($(this).is(":hidden") ? "Show" : "Hide");
            di_jq("#but_sh").html($(this).is(":hidden") ? di_jq("#langShow").val() : di_jq("#langHide").val());

            if (readCookie('ShowSliders') == "true") {
                di_jq("#but_sh_img_exp_down").css("display", "block");
                di_jq("#but_sh_img_colp_up").css("display", "none");
            }
            else {
                di_jq("#but_sh_img_exp_down").css("display", "none");
                di_jq("#but_sh_img_colp_up").css("display", "block");
            }


            if (readCookie('ShowSliders') == "true") {
                createCookie('ShowSliders', "false");
            }
            else {
                createCookie('ShowSliders', "true");
            }
        });
    }
    catch (err) { }
}

function SetShowSliderStatus() {
    try {
        if (readCookie('ShowSliders') == "true") {
            di_jq("#sh_slider").show("slow");
            di_jq("#but_sh_img_exp_down").css("display", "none");
            di_jq("#but_sh_img_colp_up").css("display", "block");
            di_jq("#but_sh").html(di_jq("#langHide").val());

        }
        else {
            di_jq("#sh_slider").hide("slow");
            di_jq("#but_sh_img_exp_down").css("display", "block");
            di_jq("#but_sh_img_colp_up").css("display", "none");
            di_jq("#but_sh").html(di_jq("#langShow").val());
        }
    }
    catch (err) { }
}

function MoveOnCatalogForSearchMultipleDatabases() {
    var IndText = "";
    var AreaText = "";

    try {
        IndText = encodeURIComponent(ReplaceAll1(di_qds_get_all_ind_block_text(), "||", ","));
        AreaText = encodeURIComponent(ReplaceAll1(di_qds_get_all_area_block_text(), "||", ","));

        window.location = "catalog.aspx?IT=" + IndText + "&AT=" + AreaText;
    }
    catch (err) { }
}