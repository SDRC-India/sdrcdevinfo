var jsonAreas;
var jsonThemes;
var jsonAdaptations;

var filteredAreas = [];
var filteredThemes = [];
var QdsFilteredAdptNId = [];
var FilteredAreaNIdArr = [];
var IsQdsFiltered = false;

var SelectedAdaptationNId = "";
var jsonAdaptationVersions = "";
var AdptVerCtrlsIds = [];
var CurSelFltrVerTexts = "";

var JsonQDSAdptations;

var IsCallFromAdminForEdit = false;

CallbackPageName = "../Callback.aspx";


function onPageLoad(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hLoggedInUserNId, hLoggedInUserName) {
    var hsgcount = 10;
    isCatalogResults = true;


    try {
        di_jq("#apDiv2").css("display", "none");


        createFormHiddenInputs("frmCatalog", "POST");
        SetCommonLinksHref("frmCatalog", "POST");

        setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName);

        CallbackPageName = "Callback.aspx";

        if (GetLanguageCounts() > 1) {
            z("LanguagePipeLi").style.display = "";
            ShowLanguageComponent(getAbsURL('stock'), 'lngcode_div', hlngcode);
        }
    }
    catch (err) { }
}

function ShowQDSComponentAndCatalogs() {
    var IndText = "";
    var AreaText = "";

    try {
        IndText = getQueryStr("IT");
        AreaText = getQueryStr("AT");

        if (IndText != undefined) {
            IndText = decodeURIComponent(IndText);
        }

        if (AreaText != undefined) {
            AreaText = decodeURIComponent(AreaText);
        }

        SetNShowQDSComponent(IndText, AreaText);

        if ((IndText != "" || AreaText != "") && (IndText != undefined || AreaText != undefined)) {
            qdsHotSelection();
        }
        else {
            startRenderingAdaptations();
        }

    } catch (err) { }
}

function SetNShowQDSComponent(indText, areaText) {
    try {
        if (indText == undefined) {
            indText = "";
        }

        if (areaText == undefined) {
            areaText = "";
        }

        ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div', z('hdbnid').value, z('hlngcodedb').value, z('langQdsIndCaption').value, z('langQdsAreaCaption').value, '100%', '100px', 'qdsHotSelection', indText, areaText, '', '', true, z('langQdsSerchBtnText').value, z('langQdsAreaBlockText').value);
    }
    catch (err) { }
}

function ShowCatalogNavTab(tab) {
    try {
        if (tab == 'global') {
            z('tab_country').style.display = 'none';
            z('tab_global').style.display = 'block';

            di_jq('#tab_global_selected').attr('class', 'tab_panel_selected');
            di_jq('#tab_country_selected').attr('class', 'tab_panel');
        }
        else if (tab == 'country') {

            z('tab_global').style.display = 'none';
            z('tab_country').style.display = 'block';

            di_jq('#tab_country_selected').attr('class', 'tab_panel_selected');
            di_jq('#tab_global_selected').attr('class', 'tab_panel');
        }
    }
    catch (err) { }
}

function startRenderingAdaptations(isEditable, jsonMatchedAdaptations) {

    try {
        ApplyMaskingDiv();
        ShowLoadingDiv();

        var divGlobal = z('global_div');

        di_jq("#imgSearchArea").attr("src", getAbsURL('stock') + "themes/default/images/cross.gif");

        if (jsonMatchedAdaptations == "NotFound") {
            // Clear off children of adaptations div        
            while (divGlobal.hasChildNodes()) {
                divGlobal.removeChild(divGlobal.lastChild);
            }
        }
        else {
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '275', 'param1': '' },
                async: true,
                success: function (data) {
                    try {
                        if (data != "") {

                            IsCallFromAdminForEdit = isEditable;

                            var jsonAll = di_jq.parseJSON(data);
                            jsonAreas = jsonAll.Areas;
                            jsonThemes = jsonAll.Themes;
                            jsonAdaptations = jsonAll.Adaptations;

                            // Clear off children of areas div
                            var divAreas = z('cata_reg_country');
                            while (divAreas.hasChildNodes()) {
                                divAreas.removeChild(divAreas.lastChild);
                            }

                            // Clear off children of adaptations div
                            while (divGlobal.hasChildNodes()) {
                                divGlobal.removeChild(divGlobal.lastChild);
                            }

                            SetFilterAdaptationVersion(jsonAdaptationVersions);

                            if (jsonMatchedAdaptations == undefined || jsonMatchedAdaptations == "") {

                                for (var i = 0; i < jsonAreas.length; i++) {
                                    var tmpArea = getAreaLi(jsonAreas[i], false);
                                    divAreas.appendChild(tmpArea);
                                }

                                for (var i = 0; i < jsonAdaptations.length; i++) {
                                    var tmpDivAdaptation = getAdaptationDiv(jsonAdaptations[i], isEditable);
                                    divGlobal.appendChild(tmpDivAdaptation);
                                }
                                updateAdaptationsCount(jsonAdaptations.length);

                                IsQdsFiltered = false;
                            }
                            else {
                                var tmpAdaptationsCount = 0;
                                FilteredAreaNIdArr = [];
                                QdsFilteredAdptNId = [];
                                IsQdsFiltered = true;

                                for (var i = 0; i < jsonAdaptations.length; i++) {
                                    var FoundJson = GetMatchedAdaptationJson(jsonMatchedAdaptations.MatchedResults, jsonAdaptations[i].NId);
                                    if (FoundJson != undefined) {
                                        var tmpDivAdaptation = getAdaptationDiv(jsonAdaptations[i], isEditable, FoundJson);
                                        divGlobal.appendChild(tmpDivAdaptation);
                                        tmpAdaptationsCount += 1;

                                        FilteredAreaNIdArr.push(jsonAdaptations[i].Area_NId);
                                        QdsFilteredAdptNId.push(jsonAdaptations[i].NId);
                                    }
                                }
                                updateAdaptationsCount(tmpAdaptationsCount);

                                //Filter and show areas by searched adaptations
                                SetFilterAdaptationQDSAreas();
                            }

                            SelectMatchAreas();

                            HideLoadingDiv();
                            RemoveMaskingDiv();
                        }
                    }
                    catch (ex) {
                        HideLoadingDiv();
                        RemoveMaskingDiv();
                        //alert("message:" + ex.message);
                    }
                },
                error: function () {
                    
                },
                cache: false
            });
        }
    }
    catch (err) {

    }
}

function SetFilterAdaptationQDSAreas() {
    var divAreas;

    try {
        divAreas = z('cata_reg_country');

        for (var i = 0; i < jsonAreas.length; i++) {
            var ParentNId = jsonAreas[i].PARENTNID;
            var AreaNId = jsonAreas[i].AREANID;
            if (ParentNId == -1 || jQuery.inArray(AreaNId, FilteredAreaNIdArr) > -1) {
                var tmpArea = getAreaLi(jsonAreas[i], false);
                divAreas.appendChild(tmpArea);
            }
        }
    }
    catch (err) { }
}

function SetFilterAdaptationVersion() {
    try {
        if (jsonAdaptationVersions == "") {
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '287' },
                async: true,
                success: function (jsonData) {
                    try {
                        if (jsonData != "") {
                            jsonAdaptationVersions = jsonData;
                            SetFilterAdaptationVersionHTML(jsonAdaptationVersions);
                        }
                        else {
                            alert("Error while updating.");
                        }
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
            SetFilterAdaptationVersionHTML(jsonAdaptationVersions);
        }
    }
    catch (err) { }
}

function SetFilterAdaptationVersionHTML(jsonData) {
    var AdptVerHtml = "";
    var LiCtrlId = "";

    try {
        jsonData = di_jq.parseJSON(jsonData);

        for (var i = 0; i < jsonData.AdaptationVersion.length; i++) {
            VerName = jsonData.AdaptationVersion[i].Ver_Name;

            LiCtrlId = "li" + VerName + "ctlog";

            AdptVerHtml += "<li><input type='checkbox' onclick='SetCheckedVerText(this, \"" + VerName + "\"); renderFilteredAdaptations();' /><label id='lang_ver7'>" + VerName + "</label></li>";
        }

        if (AdptVerHtml != "") {
            di_jq("#ulFilterAdaptationVersions").html(AdptVerHtml);
        }
    }
    catch (err) { }
}

function GetMatchedAdaptationJson(matchedAdaptationsJson, adptNid) {
    var RetVal = undefined;

    try {
        for (var i = 0; i < matchedAdaptationsJson.length; i++) {
            if (matchedAdaptationsJson[i].Adaptation_NId == adptNid) {
                RetVal = matchedAdaptationsJson[i];
                break;
            }
        }

    } catch (err) { }

    return RetVal;
}

function RemoveDelimiter(val) {
    return ReplaceAll1(val, ParamDelimiter, ", ");
}

function getAdaptationDiv(jsonAdaptation, isEditable, foundJson) {
    var result;
    var HTMLStr = "";


    try {
        result = document.createElement('div');
        result.className = "glo_white_bg";

        var div_qds_results = document.createElement('div');
        div_qds_results.id = 'divQDSResults' + jsonAdaptation.NId;
        div_qds_results.style.marginLeft = '50px';

        var div_cata_main_contnr = document.createElement('div');
        div_cata_main_contnr.className = "cata_main_contnr";

        result.appendChild(div_cata_main_contnr);

        var div_cata_img_frame = document.createElement('div');
        div_cata_img_frame.className = "cata_img_frame";

        var chk_cata_inpt = document.createElement("input");
        chk_cata_inpt.className = "cata_inpt";
        chk_cata_inpt.type = "checkbox";

        var div_cata_img_thumb = document.createElement("div");
        div_cata_img_thumb.className = "cata_img_thumb";

        var img_space = document.createElement("img");
        img_space.src = jsonAdaptation.Thumbnail_Image_URL;
        img_space.style.width = "135px";
        img_space.style.height = "auto";
        img_space.border = "0";

        if (jsonAdaptation.Is_Online.toLowerCase() == 'true') {
            img_space.style.cursor = "pointer";
            img_space.onclick = function (event) {
                window.open(jsonAdaptation.Online_URL, "_self");
            }
        }

        var span_img_space = document.createElement('span');
        span_img_space.className = 'cata_img_thumb_attrb';
        span_img_space.appendChild(img_space);
        div_cata_img_thumb.appendChild(span_img_space);

        div_cata_img_frame.appendChild(div_cata_img_thumb);


        var div_global_heading = document.createElement('div');
        div_global_heading.className = "global_heading pos_rel";


        var span_global_heading = document.createElement("span");


        span_global_heading.className = "cata_name_hdn";
        span_global_heading.innerHTML = jsonAdaptation.Name;

        if (jsonAdaptation.Is_Online.toLowerCase() == 'true') {
            span_global_heading.style.cursor = 'pointer';
            span_global_heading.onclick = function (event) {
                window.open(jsonAdaptation.Online_URL, "_self");
            }
        }

        div_global_heading.appendChild(span_global_heading);
        //Start - Editable image

        if (isEditable) {
            var img_global_edit = document.createElement('img');
            img_global_edit.style.width = "16px";
            img_global_edit.style.height = "16px";
            img_global_edit.className = 'cata_i_icon_edit';
            img_global_edit.src = getAbsURL('stock') + "themes/default/images/spacer.gif";
            img_global_edit.onclick = function (event) {
                EditCatalogAdaptation(jsonAdaptation.NId);
            }
            div_global_heading.appendChild(img_global_edit);

            var img_global_delete = document.createElement('img');
            img_global_delete.style.width = "16px";
            img_global_delete.style.height = "16px";
            img_global_delete.className = 'cata_i_icon_delete';
            img_global_delete.src = getAbsURL('stock') + "themes/default/images/spacer.gif";
            img_global_delete.onclick = function (event) {
                DeleteCatalogAdaptation(jsonAdaptation.NId);
            }
            div_global_heading.appendChild(img_global_delete);
        }

        //End - Editable image       
        // Set Region Name

        // End Set Adaptation Information 
        var div_global_heading_txt = document.createElement('div');
        div_global_heading_txt.className = "global_heading_txt";




        // Start Set Adaptation Information 
        displayAdaptationInformation(jsonAdaptation.NId, div_global_heading);

        var p_global_heading_keywords = document.createElement('p');
        p_global_heading_keywords.style.paddingTop = "5px";
        var b_global_heading_keywords = document.createElement('b');
        b_global_heading_keywords.innerHTML = z("langKeywords").value + ': ';

        var a_global_heading_keywords_topic = document.createElement('a');
        a_global_heading_keywords_topic.href = 'javascript:void(0);';


        var span_global_heading_keywords_separator = document.createElement('span');
        span_global_heading_keywords_separator.innerHTML = ', ';

        var a_global_heading_keywords_area = document.createElement('a');
        a_global_heading_keywords_area.href = 'javascript:void(0);';



        if (foundJson != undefined) {
            var IndicatorIC = RemoveDelimiter(foundJson.Indicator_NIds);
            var Qds_Areas = RemoveDelimiter(foundJson.Area_NIds);
            var IndicatorNames = RemoveDelimiter(foundJson.Indicator_Names);
            var AreaNames = RemoveDelimiter(foundJson.Area_Names);

            p_global_heading_keywords.appendChild(b_global_heading_keywords);
            if (IndicatorIC != null && IndicatorIC != '') {
                var IndicatorTitle = IndicatorNames;

                if (IndicatorNames.length > 30) {
                    IndicatorNames = IndicatorNames.substr(0, 29) + "...";
                }

                a_global_heading_keywords_topic.innerHTML = IndicatorNames;
                a_global_heading_keywords_topic.title = IndicatorTitle;

                p_global_heading_keywords.appendChild(a_global_heading_keywords_topic);
            }

            if (IndicatorNames != "" && AreaNames != "") {
                p_global_heading_keywords.appendChild(span_global_heading_keywords_separator);
            }

            if (Qds_Areas != null && Qds_Areas != '') {
                var AreaTitle = AreaNames;

                if (AreaNames.length > 30) {
                    AreaNames = AreaNames.substr(0, 29) + "...";
                }

                a_global_heading_keywords_area.innerHTML = AreaNames;
                a_global_heading_keywords_area.title = AreaTitle;

                p_global_heading_keywords.appendChild(a_global_heading_keywords_area);
            }
        }

        var p_Preview_goto = document.createElement('p');
        var span_global_heading_show_data = document.createElement('span');
        span_global_heading_show_data.className = 'qds_icon5';
        span_global_heading_show_data.style.cursor = 'pointer';
        span_global_heading_show_data.style.paddingLeft = '20px';
        span_global_heading_show_data.innerHTML = z("langPreviewData").value;
        span_global_heading_show_data.id = 'spanShowData_' + jsonAdaptation.NId;
        span_global_heading_show_data.onclick = function (event) {
            if (this.className == 'qds_icon5') {

                if (z(div_qds_results.id).innerHTML != '') {
                    di_jq('#' + div_qds_results.id).show('slow');
                    this.className = 'qds_icon6';
                    this.innerHTML = z("langHideData").value;
                }
                else {
                    this.className = 'qds_icon9';
                    var qds_areas = '';
                    var qds_indicators = '';

                    if (di_qds_get_area_search_data() != null && di_qds_get_area_search_data() != '') {
                        qds_areas = RemoveDelimiter(foundJson.Area_NIds);
                    }

                    if (di_qds_get_ind_search_data() != null && di_qds_get_ind_search_data() != '') {
                        qds_indicators = RemoveDelimiter(foundJson.Indicator_NIds);
                    }

                    GetAdaptationQdsResults(div_qds_results.id, jsonAdaptation.Online_URL, qds_areas, qds_indicators, this.id);
                }
            }
            else {
                this.className = 'qds_icon5';
                this.innerHTML = z("langPreviewData").value;
                di_jq('#' + div_qds_results.id).hide('slow');
            }
        }


        if (foundJson != undefined) {
            p_Preview_goto.appendChild(span_global_heading_show_data);
        }

        var span_global_heading_goto_data = document.createElement('span');
        span_global_heading_goto_data.className = 'qds_icon10';
        span_global_heading_goto_data.style.cursor = 'pointer';
        span_global_heading_goto_data.style.paddingLeft = '20px';
        span_global_heading_goto_data.style.marginLeft = '50px';
        span_global_heading_goto_data.innerHTML = z("langGotoData").value;
        span_global_heading_goto_data.id = 'spanGoToData_' + jsonAdaptation.NId;

        span_global_heading_goto_data.onclick = function (event) {
            GoToAdaptation(jsonAdaptation.Online_URL, foundJson);
        }

        if (foundJson != undefined) {
            p_Preview_goto.appendChild(span_global_heading_goto_data);
        }

        div_global_heading_txt.appendChild(p_global_heading_keywords);
        div_global_heading_txt.appendChild(p_Preview_goto);


        // Call method to display catalog datail grid
        displayAdaptationInfoGrid(jsonAdaptation.NId, div_global_heading);

        div_cata_main_contnr.appendChild(div_global_heading);
        div_global_heading.appendChild(div_global_heading_txt);
        div_global_heading.appendChild(div_cata_img_frame);

        result.appendChild(div_qds_results);
    }
    catch (err) {
    }

    return result;
}

function displayAdaptationInformation(AdaptationNId, divContainer) {
    var adapNid = parseInt(AdaptationNId);
    var nowAdap;
    var AdaptationRegion = "";
    var LastUpdated = "";
    var AdaptationYear = "";
    var DbLanguagesCodes = "";
    var DbLanguagesNames = "";
    var FileName = "";
    var LngCode = "";
    var LngValue = "";
    var Description = "";
    try {

        for (var i = 0; i < jsonAdaptations.length; i++) {
            if (jsonAdaptations[i].NId == adapNid) nowAdap = jsonAdaptations[i];
        }
        DbLanguagesCodes = nowAdap.Db_Languages
        for (var i = 0; i < jsonAreas.length; i++) {
            var tmpArea = jsonAreas[i];
            if (nowAdap.Area_NId == '-1' && tmpArea.PARENTNID == '-1') {
                AdaptationRegion = tmpArea.AREANAME
            }
            else if (nowAdap.Area_NId != '-1' && tmpArea.AREANID == nowAdap.Area_NId) {
                AdaptationRegion = tmpArea.AREANAME;
            }
        }
        LastUpdated = nowAdap.Last_Modified;
        if (DbLanguagesCodes != "") {
            FileName = getAbsURL('stock') + "language/Destination_Language.xml";

            di_jq.ajax({
                type: "GET",
                url: FileName,
                dataType: "xml",
                async: false,
                success: function (xml) {
                    DbLanguagesCodes = DbLanguagesCodes + ",";

                    try {
                        di_jq(xml).find("LANGUAGE_CODE").each(function () {
                            LngCode = di_jq(this).text();

                            if (DbLanguagesCodes.indexOf(LngCode + ",") > -1) {
                                LngValue = di_jq.trim(di_jq(this).siblings("LANGUAGE_NAME").text().split("[")[0]);

                                DbLanguagesNames += ", " + LngValue;
                            }
                        });

                        if (DbLanguagesNames != "") {
                            DbLanguagesNames = DbLanguagesNames.substr(2);
                            spnDbLanguagesVal.innerHTML = DbLanguagesNames;
                        }
                    }
                    catch (err) { }
                }
            });
        }
        else {
            DbLanguagesNames = DbLanguagesNames;
            // spnDbLanguagesVal.innerHTML = DbLanguagesNames;
        }

        Description = nowAdap.Description;

        // Start  Create row for adaptation region name

        var ParRegion = document.createElement("p");
        var spnAdaptationRegion = document.createElement("b");
        spnAdaptationRegion.innerHTML = z("langCountryName").value + '/' + z("langRegionName").value + ' ';
        var spnAdaptationRegionVal = document.createElement("span");
        spnAdaptationRegionVal.innerHTML = AdaptationRegion;
        ParRegion.appendChild(spnAdaptationRegion);
        ParRegion.appendChild(spnAdaptationRegionVal);
        ParRegion.style.paddingBottom = "5px";
        ParRegion.style.paddingTop = "10px";

        // Start  Create row for LastUpdated
        var ParLastUpdated = document.createElement("p");
        var spnLastUpdated = document.createElement("b");
        spnLastUpdated.innerHTML = z("langLastUpdated").value + ' ';
        var spnLastUpdatedVal = document.createElement("span");
        spnLastUpdatedVal.innerHTML = LastUpdated;
        ParLastUpdated.appendChild(spnLastUpdated);
        ParLastUpdated.appendChild(spnLastUpdatedVal);
        ParLastUpdated.style.paddingBottom = "5px";

        // Start Create Li for AdaptationYear
        var ParAdaptYear = document.createElement("p");
        var spnAdaptationYear = document.createElement("b");
        spnAdaptationYear.innerHTML = z("langAdaptationYear").value + ' ';
        var spnAdaptationYearVal = document.createElement("span");
        spnAdaptationYearVal.innerHTML = nowAdap.Adaptation_Year;
        ParAdaptYear.appendChild(spnAdaptationYear);
        ParAdaptYear.appendChild(spnAdaptationYearVal);


        // Start Create Li for Database languages
        var ParDbLang = document.createElement("p");
        var spnDbLanguages = document.createElement("b");
        spnDbLanguages.innerHTML = z("langDBLanguage").value + ' ';
        var spnDbLanguagesVal = document.createElement("span");
        spnDbLanguagesVal.innerHTML = DbLanguagesNames;
        ParDbLang.appendChild(spnDbLanguages);
        ParDbLang.appendChild(spnDbLanguagesVal);
        ParDbLang.style.paddingBottom = "5px";

        var DivMain = document.createElement("div");
        DivMain.className = "global_heading_txt";

        var p_global_heading_txt = document.createElement("p");
        p_global_heading_txt.innerHTML = Description;


        DivMain.appendChild(ParRegion);
        DivMain.appendChild(ParLastUpdated);
        DivMain.appendChild(ParDbLang);
        DivMain.appendChild(ParAdaptYear);
        DivMain.appendChild(p_global_heading_txt);

        divContainer.appendChild(DivMain);
    }
    catch (err) { }
}

function displayAdaptationInfoGrid(AdaptationNId, divContainer) {
    var adapNid = parseInt(AdaptationNId);
    var nowAdap;
    var GeographicalAreas = "";
    var Variables = "";
    var TimePeriod = "";
    var Data = "";
    var DbAdminContact = "";
    var Email = "";
    var DownloadCSV = "";
    var DbAdmName = "";
    var DbAdmInstitution = "";
    var DbAdminContactTooltip = "";
    var StartIndex = -1;
    var EndIndex = -1;
    try {

        for (var i = 0; i < jsonAdaptations.length; i++) {
            if (jsonAdaptations[i].NId == adapNid) nowAdap = jsonAdaptations[i];
        }

        GeographicalAreas = nowAdap.Area_Count;
        Variables = nowAdap.IUS_Count;
        TimePeriod = nowAdap.Start_Year + "-" + nowAdap.End_Year;
        Data = nowAdap.Data_Values_Count;

        var DownloadCSVHtml = CreateLinkForDownloadCSV(nowAdap.Online_URL, nowAdap.LangCode_CSVFiles, nowAdap.Name, nowAdap.Adaptation_Year);
        if (DownloadCSVHtml != null && DownloadCSVHtml != '') {
            DownloadCSV = DownloadCSVHtml;
            StartIndex = DownloadCSV.indexOf("<span");
            EndIndex = DownloadCSV.indexOf("</p");
            if (StartIndex > -1 && EndIndex > -1) {
                DownloadCSV = DownloadCSV.substring(StartIndex, EndIndex);
            }
            else {
                DownloadCSV = "";
            }
        }

        DbAdmName = nowAdap.Db_Adm_Name;
        DbAdmInstitution = nowAdap.Db_Adm_Institution;

        if (DbAdmName != "" && DbAdmInstitution != "") {
            DbAdminContact = DbAdmName + " - " + DbAdmInstitution;
        }
        else if (DbAdmName != "") {
            DbAdminContact = DbAdmName;
        }
        else if (DbAdmInstitution != "") {
            DbAdminContact = DbAdmInstitution;
        }

        if (DbAdminContact.length > 85) {
            DbAdminContactTooltip = DbAdminContact;
            DbAdminContact = DbAdminContact.substr(0, 84) + "...";
        }
        Email = nowAdap.Db_Adm_Email;
        if (Email.length > 0) {
            Email = "<a href=mailto:" + Email + ">" + Email + "<a>";
        }

        // Create left Div
        var DivLeftCol = document.createElement("Div");
        DivLeftCol.className = "rcol";

        // Create element for database adamin contact
        var PDbContact = document.createElement("p");
        var dbContact = document.createElement("b");
        dbContact.innerHTML = z("LangDbAdminContact").value;
        var spnDbContactVal = document.createElement("span");
        spnDbContactVal.innerHTML = DbAdminContact;
        var brTag1 = document.createElement("br");
        PDbContact.appendChild(dbContact);
        PDbContact.appendChild(brTag1);
        PDbContact.appendChild(spnDbContactVal);
        PDbContact.style.paddingBottom = "5px";
        DivLeftCol.appendChild(PDbContact);

        // Create element for adamin email
        var PEmail = document.createElement("p");
        var bEmail = document.createElement("b");
        bEmail.innerHTML = z("LangEmail").value + ' ';
        var spnEmail = document.createElement("span");
        spnEmail.innerHTML = Email;
        PEmail.appendChild(bEmail);
        PEmail.appendChild(spnEmail);
        DivLeftCol.appendChild(PEmail);


        // Create Mid Div
        var DivMidCol = document.createElement("Div");
        DivMidCol.className = "rcol2";

        // Create list for Geographical Area
        var PGeographicalArea = document.createElement("p");
        var bGeographicalArea = document.createElement("b");
        bGeographicalArea.innerHTML = z("langGeographicalAreas").value + ' ';
        var spnGeographicalArea = document.createElement("span");
        spnGeographicalArea.innerHTML = GeographicalAreas;
        PGeographicalArea.appendChild(bGeographicalArea);
        PGeographicalArea.appendChild(spnGeographicalArea);
        PGeographicalArea.style.paddingBottom = "5px";
        DivMidCol.appendChild(PGeographicalArea);

        // Create list for Variables
        var PVariables = document.createElement("p");
        var bVariables = document.createElement("b");
        bVariables.innerHTML = z("langVariables").value + ' ';
        var spnVariablesVal = document.createElement("span");
        spnVariablesVal.innerHTML = Variables;
        PVariables.appendChild(bVariables);
        PVariables.appendChild(spnVariablesVal);
        PVariables.style.paddingBottom = "5px";
        DivMidCol.appendChild(PVariables);


        // Create list for Time Period
        var PTimePeriod = document.createElement("p");
        var bTimePeriod = document.createElement("b");
        bTimePeriod.innerHTML = z("langTimePeriod").value + ' ';
        var spnTimePeriodVal = document.createElement("span");
        spnTimePeriodVal.innerHTML = TimePeriod;
        PTimePeriod.appendChild(bTimePeriod);
        PTimePeriod.appendChild(spnTimePeriodVal);
        PTimePeriod.style.paddingBottom = "5px";
        DivMidCol.appendChild(PTimePeriod);

        // Create list for Data
        var PData = document.createElement("p");
        var bDataVal = document.createElement("b");
        bDataVal.innerHTML = z("langData").value + ' ';
        var spnDataVal = document.createElement("span");
        spnDataVal.innerHTML = Data;
        PData.appendChild(bDataVal);
        PData.appendChild(spnDataVal);

        DivMidCol.appendChild(PData);

        // Create Right Div
        var DivRightCol = document.createElement("Div");
        DivRightCol.className = "rcol3";
        var PR1 = document.createElement("p");
        var bCsvVal = document.createElement("b");
        bCsvVal.innerHTML = z("langDownloadCSVFile").value;
        var spnCsvVal = document.createElement("span");
        spnCsvVal.innerHTML = DownloadCSV;
        var brTag2 = document.createElement("p");
        if (DownloadCSV.length > 0) {
            PR1.appendChild(bCsvVal);
            PR1.appendChild(brTag2);
            PR1.appendChild(brTag2);
            PR1.appendChild(brTag2);
            PR1.appendChild(spnCsvVal);
        }
        DivRightCol.appendChild(PR1);

        var DivMain = document.createElement("div");
        DivMain.className = "global_heading_txt global_heading_resultdetail";


        DivMain.appendChild(DivLeftCol);
        DivMain.appendChild(DivMidCol);
        DivMain.appendChild(DivRightCol);

        divContainer.appendChild(DivMain);
    }
    catch (err) { }
}

function GoToAdaptation(AdaptationURL, jsonDataForAdaptation) {
    var strJson;
    var MaxLength = 500;

    try {
        strJson = JSON.stringify(jsonDataForAdaptation);

        if (strJson.length > MaxLength) {
            strJson = GetMinifiedJsonAdaptationString(jsonDataForAdaptation, MaxLength);
        }
        strJson = strJson.replace("Adaptation_NId", "apn");
        strJson = strJson.replace("Indicator_Names", "i");
        strJson = strJson.replace("Indicator_NIds", "i_n");
        strJson = strJson.replace("Area_Names", "a");
        strJson = strJson.replace("Area_NIds", "a_n");

        window.location = AdaptationURL + "/libraries/aspx/home.aspx?refer_url=catalog&jsonAreasTopics=" + strJson;
    }
    catch (err) { }
}

function GetMinifiedJsonAdaptationString(jsonData, maxLength) {
    var RetVal = "";

    var NewJsonData = { "Adaptation_NId": "", "Indicator_Names": "", "Indicator_NIds": "", "Area_Names": "", "Area_NIds": "" };

    var IndicatorNamesArr = [];
    var IndicatorNIdsArr = [];
    var AreaNamesArr = [];
    var AreaNIdsArr = [];

    var IndStartIndex = 0;
    var AreaStartIndex = 0

    var IndicatorNamesStr = "";
    var IndicatorNIdsStr = "";
    var AreaNamesStr = "";
    var AreaNIdsStr = "";

    var JsonString = "";
    var JsonStringLenght = 0;

    try {

        NewJsonData.Adaptation_NId = jsonData.Adaptation_NId;
        JsonString = JSON.stringify(NewJsonData);

        IndicatorNamesArr = jsonData.Indicator_Names.split(ParamDelimiter);
        IndicatorNIdsArr = jsonData.Indicator_NIds.split(ParamDelimiter);
        AreaNamesArr = jsonData.Area_Names.split(ParamDelimiter);
        AreaNIdsArr = jsonData.Area_NIds.split(ParamDelimiter);

        while (JsonString.length < maxLength) {

            RetVal = JsonString;

            if (IndStartIndex == IndicatorNamesArr.length && AreaStartIndex == AreaNamesArr.length) {
                break;
            }

            if (IndicatorNamesArr[IndStartIndex] != undefined) {

                if (IndStartIndex > 0) {
                    IndicatorNamesStr += ParamDelimiter;
                    IndicatorNIdsStr += ParamDelimiter;
                }

                IndicatorNamesStr += IndicatorNamesArr[IndStartIndex];
                IndicatorNIdsStr += IndicatorNIdsArr[IndStartIndex];

                IndStartIndex++;

                NewJsonData.Indicator_Names = IndicatorNamesStr;
                NewJsonData.Indicator_NIds = IndicatorNIdsStr;
            }

            if (AreaNamesArr[AreaStartIndex] != undefined) {

                if (AreaStartIndex > 0) {
                    AreaNamesStr += ParamDelimiter;
                    AreaNIdsStr += ParamDelimiter;
                }

                AreaNamesStr += AreaNamesArr[AreaStartIndex];
                AreaNIdsStr += AreaNIdsArr[AreaStartIndex];

                AreaStartIndex++;

                NewJsonData.Area_Names = AreaNamesStr;
                NewJsonData.Area_NIds = AreaNIdsStr;
            }

            JsonString = JSON.stringify(NewJsonData);
        }

    }
    catch (err) { }

    return RetVal;
}

function getAreaLi(jsonArea, isChecked) {
    var result;

    try {
        result = document.createElement('li');

        var chk_area = document.createElement('input');
        chk_area.type = 'checkbox';
        chk_area.value = jsonArea.AREANID;
        chk_area.defaultChecked = isChecked;
        chk_area.onclick = function (event) { filterArea(this); }

        if (isChecked) filteredAreas.push(jsonArea.AREANID);

        var lbl_area = document.createElement('label');
        lbl_area.innerHTML = jsonArea.AREANAME;
        if (jsonArea.SortingOrder == 2 || jsonArea.SortingOrder == 1) {
            lbl_area.className = "cata_natadpt_name_clo";
        }
        result.appendChild(chk_area);
        result.appendChild(lbl_area);
    }
    catch (err) { }

    return result;
}

function getThemeLi(jsonTheme) {
    var result;

    try {
        result = document.createElement('li');

        var chk_theme = document.createElement('input');
        chk_theme.type = 'checkbox';
        chk_theme.value = jsonTheme.ThemeNId;
        chk_theme.defaultChecked = true;
        chk_theme.onclick = function (event) { filterTheme(this); }

        filteredThemes.push(jsonTheme.ThemeNId);

        var lbl_theme = document.createElement('label');
        lbl_theme.innerHTML = jsonTheme.ThemeName;

        result.appendChild(chk_theme);
        result.appendChild(lbl_theme);
    }
    catch (err) { }

    return result;
}

function filterArea(chkArea) {
    filteredAreas = [];

    try {
        di_jq("#cata_reg_country input").each(function () {
            if (di_jq(this).attr("checked")) {
                filteredAreas.push(di_jq(this).val());
            }
        })

        renderFilteredAdaptations();
    }
    catch (err) { }
}

function filterTheme(chkTheme) {
    var tmpThemeNId;

    try {
        tmpThemeNId = parseInt(chkTheme.value);

        if (chkTheme.checked) {
            filteredThemes.push(tmpThemeNId);
        }
        else {
            var tmpIndexOfThemeNId = di_jq.inArray(tmpThemeNId, filteredThemes);
            filteredThemes.splice(tmpIndexOfThemeNId, 1);
        }
        renderFilteredAdaptations();
    }
    catch (err) { }
}

function SetCheckedVerText(objChk, val) {
    var TempStr = "";

    try {
        if (objChk.checked) {
            CurSelFltrVerTexts += val + ",";
        }
        else {
            TempStr = CurSelFltrVerTexts.replace(val + ",");

            if (TempStr == undefined || TempStr == "") {
                CurSelFltrVerTexts = "";
            }
            else {
                CurSelFltrVerTexts = TempStr;
            }
        }
    }
    catch (err) { }
}

function renderFilteredAdaptations() {

    var SelectedAdaptationsCount = 0;
    var SelectedAvailabilityCount = 0;

    var IsVersion7Checked = false;
    var IsDevInfoChecked = false;
    var IsOnlineChecked = false;

    var IsVersion7Val = false;
    var IsDevInfoVal = false;
    var IsOnlineVal = false;

    var FilteredAdaptationsCount = 0;

    var adaptationArea;
    var isAmongAreas;
    var showAdaptation;

    try {
        //Get the count of selected Adaptations and Availability
        SelectedAdaptationsCount = di_jq("#cata_reg_country input:checked").length;
        SelectedAvailabilityCount = di_jq("#cata_availbty input:checked").length;

        ApplyMaskingDiv();
        ShowLoadingDiv();

        if (SelectedAvailabilityCount > 0) {

            if (di_jq("#ulFilterAdaptationVersions input:checked").length > 0) {
                IsVersion7Checked = true;
            }

            if (z('chk_cata_avlb_devinfo').checked) {
                IsDevInfoChecked = true;
            }

            if (z('chk_cata_aval_onln').checked) {
                IsOnlineChecked = true;
            }
        }

        // Clear off children of adaptations div
        var divGlobal = z('global_div');
        while (divGlobal.hasChildNodes()) {
            divGlobal.removeChild(divGlobal.lastChild);
        }


        for (var i = 0; i < jsonAdaptations.length; i++) {

            isAmongAreas = false;

            if (IsQdsFiltered) {
                if (jQuery.inArray(jsonAdaptations[i].NId, QdsFilteredAdptNId) > -1) {

                    adaptationArea = jsonAdaptations[i].Area_NId;

                    if (adaptationArea == '-1') adaptationArea = getAreaNId(adaptationArea);

                    if (SelectedAdaptationsCount == 0) {
                        isAmongAreas = true;
                    }
                    else {

                        if (filteredAreas.indexOf(adaptationArea) != -1) {
                            isAmongAreas = true;
                        }
                    }
                }
            }
            else {
                adaptationArea = jsonAdaptations[i].Area_NId;

                if (adaptationArea == '-1') adaptationArea = getAreaNId(adaptationArea);

                if (SelectedAdaptationsCount == 0) {
                    isAmongAreas = true;
                }
                else {

                    if (filteredAreas.indexOf(adaptationArea) != -1) {
                        isAmongAreas = true;
                    }
                }
            }

            showAdaptation = false;

            if (isAmongAreas) {

                if (SelectedAvailabilityCount == 0) {
                    showAdaptation = true;
                }
                else {

                    //Reset values
                    IsVersion7Val = false;
                    IsDevInfoVal = false;
                    IsOnlineVal = false;

                    //Get new values from Json data                        
                    if (CurSelFltrVerTexts.indexOf(jsonAdaptations[i].DI_Version + ",") > -1) {
                        IsVersion7Val = true;
                    }

                    if (jsonAdaptations[i].Online_URL.indexOf("http://www.devinfo.info") > -1) {
                        IsDevInfoVal = true;
                    }

                    if (jsonAdaptations[i].Is_Online.toLowerCase() == "true") {
                        IsOnlineVal = true;
                    }


                    if (IsVersion7Checked && IsDevInfoChecked && IsOnlineChecked) { //All 3 combinations are checked
                        if (IsVersion7Val && IsDevInfoVal && IsOnlineVal) {
                            showAdaptation = true;
                        }
                    }
                    else if (IsVersion7Checked && IsDevInfoChecked) {   //Any 2 combinations are checked
                        if (IsDevInfoVal && IsOnlineVal) {
                            showAdaptation = true;
                        }
                    }
                    else if (IsVersion7Checked && IsOnlineChecked) {
                        if (IsVersion7Val && IsOnlineVal) {
                            showAdaptation = true;
                        }
                    }
                    else if (IsDevInfoChecked && IsOnlineChecked) {
                        if (IsDevInfoVal && IsOnlineVal) {
                            showAdaptation = true;
                        }
                    }
                    else if (IsDevInfoChecked && IsVersion7Checked) {
                        if (IsDevInfoVal && IsVersion7Val) {
                            showAdaptation = true;
                        }
                    }
                    else if (IsOnlineChecked && IsVersion7Checked) {
                        if (IsOnlineVal && IsVersion7Val) {
                            showAdaptation = true;
                        }
                    }
                    else if (IsOnlineChecked && IsDevInfoChecked) {
                        if (IsOnlineVal && IsDevInfoVal) {
                            showAdaptation = true;
                        }
                    }
                    else if (IsVersion7Checked) {   //Any 1 is checked
                        if (IsVersion7Val) {
                            showAdaptation = true;
                        }
                    }
                    else if (IsDevInfoChecked) {
                        if (IsDevInfoVal) {
                            showAdaptation = true;
                        }
                    }
                    else if (IsOnlineChecked) {
                        if (IsOnlineVal) {
                            showAdaptation = true;
                        }
                    }
                }
            }

            if (showAdaptation) {
                var tmpDivAdaptation;

                if (IsQdsFiltered) {
                    var FoundJson = GetMatchedAdaptationJson(JsonQDSAdptations.MatchedResults, jsonAdaptations[i].NId);
                    tmpDivAdaptation = getAdaptationDiv(jsonAdaptations[i], false, FoundJson);
                }
                else {
                    tmpDivAdaptation = getAdaptationDiv(jsonAdaptations[i], IsCallFromAdminForEdit);
                }

                divGlobal.appendChild(tmpDivAdaptation);
                FilteredAdaptationsCount += 1;
            }
        }

        SelectMatchAreas();

        updateAdaptationsCount(FilteredAdaptationsCount);

        HideLoadingDiv();
        RemoveMaskingDiv();
        //}        
    }
    catch (err) { }
}

function getAreaNId(AreaParentNId) {
    var result = '';

    try {
        for (var i = 0; i < jsonAreas.length; i++) {
            if (jsonAreas[i].PARENTNID == AreaParentNId) {
                result = jsonAreas[i].AREANID;
                break;
            }
        }
    }
    catch (err) { }

    return result;
}

function displayCallOutDiv(AdaptationNId, evt) {

    var adapNid = parseInt(AdaptationNId);
    var nowAdap;
    var DbAdmName = "";
    var DbAdmInstitution = "";
    var NameInstitution = "";
    var NameInstitutionTooltip = "";
    var DbLanguagesCodes = "";
    var DbLanguagesNames = "";
    var FileName = "";
    var LngCode = "";
    var LngValue = "";
    var ClientXVal = 0;
    var ClientYVal = 0;

    try {

        ClientXVal = evt.clientX;
        ClientYVal = evt.clientY;

        for (var i = 0; i < jsonAdaptations.length; i++) {
            if (jsonAdaptations[i].NId == adapNid) nowAdap = jsonAdaptations[i];
        }

        z("liCallout_AvailableDI").className = "divCallout_icn_crss";
        z("liCallout_AvailableOnline").className = "divCallout_icn_crss";

        if (nowAdap.Online_URL.indexOf("http://www.devinfo.info") == 0) z("liCallout_AvailableDI").className = 'divCallout_icn_tck';
        if (nowAdap.Is_Online.toLowerCase() == 'true') z("liCallout_AvailableOnline").className = 'divCallout_icn_tck';

        z('spnAdaptationName').innerHTML = nowAdap.Name;

        var SpnRegions = z('spnRegions');

        var allAreaNIds = nowAdap.Area_NId.split(',');

        for (var i = 0; i < jsonAreas.length; i++) {
            var tmpArea = jsonAreas[i];
            if (nowAdap.Area_NId == '-1' && tmpArea.PARENTNID == '-1') {
                SpnRegions.innerHTML = "(" + tmpArea.AREANAME + ")";
                break;
            }
            else if (nowAdap.Area_NId != '-1' && tmpArea.AREANID == nowAdap.Area_NId) {
                SpnRegions.innerHTML = "(" + tmpArea.AREANAME + ")";
                break;
            }
        }

        DbAdmName = nowAdap.Db_Adm_Name;
        DbAdmInstitution = nowAdap.Db_Adm_Institution;

        if (DbAdmName != "" && DbAdmInstitution != "") {
            NameInstitution = DbAdmName + " - " + DbAdmInstitution;
        }
        else if (DbAdmName != "") {
            NameInstitution = DbAdmName;
        }
        else if (DbAdmInstitution != "") {
            NameInstitution = DbAdmInstitution;
        }

        if (NameInstitution.length > 85) {
            NameInstitutionTooltip = NameInstitution;
            NameInstitution = NameInstitution.substr(0, 84) + "...";
        }

        z('spnDbAdmNameInstitution').innerHTML = NameInstitution;
        z('spnDbAdmNameInstitution').title = NameInstitutionTooltip;
        z('spnDbAdmEmail').innerHTML = nowAdap.Db_Adm_Email;

        z('spnUnciefRegionVal').innerHTML = nowAdap.Unicef_Region;
        z('spnAdaptationYearVal').innerHTML = nowAdap.Adaptation_Year;

        z('spnUnciefRegionVal').style.display = "none";
        z('spnUnciefRegion').style.display = "none";

        DbLanguagesCodes = nowAdap.Db_Languages

        if (DbLanguagesCodes != "") {
            FileName = getAbsURL('stock') + "language/Destination_Language.xml";

            di_jq.ajax({
                type: "GET",
                url: FileName,
                dataType: "xml",
                async: true,
                success: function (xml) {
                    DbLanguagesCodes = DbLanguagesCodes + ",";

                    try {
                        di_jq(xml).find("LANGUAGE_CODE").each(function () {
                            LngCode = di_jq(this).text();

                            if (DbLanguagesCodes.indexOf(LngCode + ",") > -1) {
                                LngValue = di_jq.trim(di_jq(this).siblings("LANGUAGE_NAME").text().split("[")[0]);

                                DbLanguagesNames += ", " + LngValue;
                            }
                        });

                        if (DbLanguagesNames != "") {
                            DbLanguagesNames = DbLanguagesNames.substr(2);
                        }

                        z('spnDatabaseLanguagesVal').innerHTML = DbLanguagesNames;

                        //SetCalloutPosition(evt, z('divCallout_cata'), 20, 0);                        
                        SetCalloutPosition(evt, z('divCallout_cata'), 20, 0, ClientXVal, ClientYVal);

                        z('divCallout_cata').style.display = 'block';
                    }
                    catch (err) { }
                }
            });
        }
        else {
            z('spnDatabaseLanguagesVal').innerHTML = DbLanguagesNames;

            //SetCalloutPosition(evt, z('divCallout_cata'), 20, 0);
            SetCalloutPosition(evt, z('divCallout_cata'), 20, 0, ClientXVal, ClientYVal);

            z('divCallout_cata').style.display = 'block';
        }

        if (jsonAdaptationVersions == "") {
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '287' },
                async: true,
                success: function (jsonData) {
                    try {
                        if (jsonData != "") {
                            jsonAdaptationVersions = jsonData;
                            SetAdaptationVersionHTML(jsonAdaptationVersions, nowAdap.DI_Version);
                        }
                        else {
                            alert("Error while updating.");
                        }
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
            SetAdaptationVersionHTML(jsonAdaptationVersions, nowAdap.DI_Version);
        }
    }
    catch (err) { }
}

function SetAdaptationVersionHTML(jsonData, diVersion) {
    var AdptVerHtml = "";
    var LiClassName = "";
    var LiCtrlId = "";

    try {

        jsonData = di_jq.parseJSON(jsonData);
        AdptVerCtrlsIds = [];

        for (var i = 0; i < jsonData.AdaptationVersion.length; i++) {
            VerName = jsonData.AdaptationVersion[i].Ver_Name;

            LiCtrlId = "li" + VerName + "ctlog";

            if (VerName == diVersion) {
                LiClassName = "divCallout_icn_tck";
            }
            else {
                LiClassName = "divCallout_icn_crss";
            }

            AdptVerHtml += "<li id='" + LiCtrlId + "' class='" + LiClassName + "'>" + VerName + "</li>";

            AdptVerCtrlsIds.push(LiCtrlId);
        }

        if (AdptVerHtml != "") {
            di_jq("#ulAdaptationVersions").html(AdptVerHtml);
        }
    }
    catch (err) { }
}

function HideAdapCallOutDiv(e) {
    //    var clickId;
    //    var CtrlsIdArr = ['divCallout_cata', 'liDetails1', 'spnAdaptationName', 'spnRegions', 'liDetails2', 'spnDbAdmNameInstitution', 'spnDbAdmEmail', 'liDetails3', 'spnAdaptationYear', 'spnAdaptationYearVal', 'spnDatabaseLanguages', 'spnDatabaseLanguagesVal', 'liCallout_AvailableDI', 'liCallout_AvailableOnline'];

    //    try {

    //        clickId = e.target.id;

    //        if (jQuery.inArray(clickId, CtrlsIdArr) == -1 && jQuery.inArray(clickId, AdptVerCtrlsIds) == -1) {
    //            z('divCallout_cata').style.display = 'none';
    //        }
    //    }
    //    catch (err) { }
}

function qdsHotSelection() {
    var InputParam, IndicatorIC, Areas;
    var spanLoading, spanLoadingOuter, divQDSResults, spanTimeCounter;

    try {
        ApplyMaskingDiv();
        ShowLoadingDiv();

        IndicatorIC = di_qds_get_ind_search_data();
        Areas = di_qds_get_area_search_data();

        if ((IndicatorIC != null && IndicatorIC != "") || (Areas != null && Areas != "")) {
            GetCatalogSearchResults(Areas, IndicatorIC);
        }
        else {
            startRenderingAdaptations(false, '');
        }

    }
    catch (err) { }
}

function GetCatalogSearchResults(seachAreas, searchIndicators) {
    var InputParam;

    try {
        InputParam = seachAreas;
        InputParam += ParamDelimiter + searchIndicators;
        InputParam += ParamDelimiter + readCookie('hlngcodedb');

        di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '276', 'param1': InputParam },
            async: true,
            success: function (jsonAdaptationsData) {
                try {
                    if (jsonAdaptationsData != "") {
                        try {                                                       
                            JsonQDSAdptations = di_jq.parseJSON(jsonAdaptationsData);
                            startRenderingAdaptations(false, JsonQDSAdptations);
                        }
                        catch (err) {
                            alert("Results not found.");
                            HideLoadingDiv();
                            RemoveMaskingDiv();
                        }
                    }
                    else {
                        startRenderingAdaptations(false, "NotFound");
                    }
                }
                catch (ex) {
                    //alert("message:" + ex.message);
                }
            },
            error: function () {

            },
            cache: false
        });

    }
    catch (err) { }
}

function GetAdaptationQdsResults(divQdsResults, AdaptationURL, seachAreas, searchIndicators, spanLoadingID) {
    var InputParam;

    try {
        InputParam = searchIndicators;
        InputParam += ParamDelimiter + ""; // Blank IC NIds
        InputParam += ParamDelimiter + seachAreas;
        InputParam += ParamDelimiter + z('hlngcode').value; //+  "en";
        InputParam += ParamDelimiter + AdaptationURL;

        di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '131', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    if (data != "") {

                        var TimeTaken = data.split(ParamDelimiter)[0];
                        var ResultsData = data.split(ParamDelimiter)[1];
                        //For Gallery existence
                        var GalleryExistence = data.split(ParamDelimiter)[2];
                        //                        if (isSearchResultsGalleryEnabled) {
                        //                            if (!GalleryExistence)
                        //                            { isSearchResultsGalleryEnabled = false; }
                        //                        }
                        var jsonData = di_jq.parseJSON(ResultsData);

                        RenderTimeTaken(jsonData[""].length, TimeTaken);

                        var spanLoading = z('spanLoading');
                        var spanLoadingOuter = z('spanLoadingOuter');
                        var spanTimeCounter = z('spanTimeCounter');

                        RenderResults(jsonData, divQdsResults, spanLoadingOuter.id, spanLoading.id, false, TimeTaken, AdaptationURL, "");

                        var spanLoader = z(spanLoadingID);
                        spanLoader.className = 'qds_icon6';
                        spanLoader.innerHTML = z("langHideData").value;

                    }
                    else {

                    }
                }
                catch (ex) {
                    //alert("Error in Adapdation's webservice: " + ex.message);
                }
            },
            error: function () {
                
            },
            cache: false
        });

    }
    catch (err) { }
}

function EditCatalogAdaptation(adptNId) {
    var Area_NId;
    var Sub_Nation;

    try {
        ApplyMaskNLoader();

        ShowDivAddAdptDetails(true);

        for (var i = 0; i < jsonAdaptations.length; i++) {
            if (jsonAdaptations[i].NId == adptNId) {

                SelectedAdaptationNId = adptNId;

                di_jq("#txtAdaptationName").val(jsonAdaptations[i].Name);
                di_jq("#txtDescription").val(jsonAdaptations[i].Description);

                di_jq("#ddlVersion option:contains(" + jsonAdaptations[i].DI_Version + ")").attr('selected', true);

                di_jq("#ddlUnicefRegion").val(jsonAdaptations[i].Unicef_Region);

                di_jq("#txtAdaptationYear").val(jsonAdaptations[i].Adaptation_Year);
                di_jq("#txtDatabaseLanguages").val(jsonAdaptations[i].Db_Languages);

                di_jq("#chkDesktop").attr("checked", (jsonAdaptations[i].Is_Desktop.toLowerCase() == "true" ? true : false));
                di_jq("#chkWeb").attr("checked", (jsonAdaptations[i].Is_Online.toLowerCase() == "true" ? true : false));

                di_jq("#txtWebURL").val(jsonAdaptations[i].Online_URL);
                if (di_jq("#chkWeb").attr("checked")) {
                    di_jq("#txtWebURL").attr("disabled", false);
                }

                di_jq("#txtAreaCount").val(jsonAdaptations[i].Area_Count);
                di_jq("#txtIUSCount").val(jsonAdaptations[i].IUS_Count);
                di_jq("#txtTimePeriodsCount").val(jsonAdaptations[i].Time_Periods_Count);
                di_jq("#txtDataValuesCount").val(jsonAdaptations[i].Data_Values_Count);
                di_jq("#txtStartYear").val(jsonAdaptations[i].Start_Year);
                di_jq("#txtEndYear").val(jsonAdaptations[i].End_Year);
                di_jq("#txtLastModifiedOn").val(jsonAdaptations[i].Last_Modified);

                di_jq("#txtDbAdmName").val(jsonAdaptations[i].Db_Adm_Name);
                di_jq("#txtDbAdmInstitution").val(jsonAdaptations[i].Db_Adm_Institution);
                di_jq("#txtDbAdmEmail").val(jsonAdaptations[i].Db_Adm_Email);

                Area_NId = jsonAdaptations[i].Area_NId
                di_jq("#txtSubNation").val("");

                if (Area_NId == -1) {
                    di_jq("#rbGlobal").attr("checked", true);
                    di_jq("#ddlSelectCountry").attr("disabled", true);
                    RemoveLoaderNMask();
                }
                else {
                    di_jq("#ddlSelectCountry").attr("disabled", false);

                    if (di_jq("#ddlSelectCountry").find("option").length == 0) {
                        di_jq.ajax({
                            type: "POST",
                            url: CallbackPageName,
                            data: { 'callback': '270' },
                            async: true,
                            success: function (jsonData) {
                                try {
                                    if (jsonData != "") {

                                        FillCountryDdl(jsonData, "ddlSelectCountry", Area_NId);
                                    }

                                    RemoveLoaderNMask();
                                }
                                catch (ex) {
                                    alert("message:" + ex.message);
                                    RemoveLoaderNMask();
                                }
                            },
                            error: function () {
                                
                                RemoveLoaderNMask();
                            },
                            cache: false
                        });
                    }
                    else {
                        di_jq("#ddlSelectCountry").val(Area_NId);
                        RemoveLoaderNMask();
                    }



                    Sub_Nation = jsonAdaptations[i].Sub_Nation;

                    if (Sub_Nation != "") {
                        di_jq("#rbSubNational").attr("checked", true)
                        di_jq("#txtSubNation").val(Sub_Nation);
                        di_jq("#txtSubNation").attr("disabled", false);
                    }
                    else {
                        di_jq("#rbNational").attr("checked", true);
                    }
                }

                CurrentAdaptationLogoUrl = jsonAdaptations[i].Thumbnail_Image_URL;

                di_jq("#imgPreview").attr("src", CurrentAdaptationLogoUrl);

                di_jq("#flCatalogImage")[0].value = CurrentAdaptationLogoUrl;

                break;
            }
        }

    }
    catch (err) { }
}

function DeleteCatalogAdaptation(adptNId) {
    var InputParam;

    try {
        if (adptNId != "") {

            if (confirm("Confirm to delete ?")) {

                ApplyMaskNLoader();

                SelectedAdaptationNId = adptNId;

                InputParam = SelectedAdaptationNId;

                di_jq.ajax({
                    type: "POST",
                    url: CallbackPageName,
                    data: { 'callback': '281', 'param1': InputParam },
                    async: true,
                    success: function (data) {
                        if (data != "") {
                            ShowCatalogLists(true);

                            alert("Delete successfully.")
                        }
                        else {
                            alert("Error while deleting.");
                        }

                        RemoveLoaderNMask();
                    },
                    error: function () {
                        
                        RemoveLoaderNMask();
                    },
                    cache: false
                });
            }
        }
    }
    catch (err) { }
}

function UpdateAdaptation() {
    var InputParam;

    try {

        if (ValidateAdaptation()) {
            ApplyMaskNLoader();

            InputParam = di_jq("#txtAdaptationName").val(); ;
            InputParam += ParamDelimiter + di_jq("#txtDescription").val();
            InputParam += ParamDelimiter + di_jq("#ddlVersion").val();
            InputParam += ParamDelimiter + di_jq("#chkDesktop").attr("checked");
            InputParam += ParamDelimiter + di_jq("#chkWeb").attr("checked");
            InputParam += ParamDelimiter + di_jq("#txtWebURL").val();
            InputParam += ParamDelimiter + di_jq("#txtAreaCount").val();
            InputParam += ParamDelimiter + di_jq("#txtIUSCount").val();
            InputParam += ParamDelimiter + di_jq("#txtTimePeriodsCount").val();
            InputParam += ParamDelimiter + di_jq("#txtDataValuesCount").val();
            InputParam += ParamDelimiter + di_jq("#txtStartYear").val();
            InputParam += ParamDelimiter + di_jq("#txtEndYear").val();
            InputParam += ParamDelimiter + di_jq("#txtLastModifiedOn").val();

            if (di_jq("#rbGlobal").attr("checked")) {
                InputParam += ParamDelimiter + "-1";
            }
            else {
                InputParam += ParamDelimiter + di_jq("#ddlSelectCountry").val();
            }

            InputParam += ParamDelimiter + di_jq("#txtSubNation").val();
            InputParam += ParamDelimiter + CurrentAdaptationLogoUrl;
            InputParam += ParamDelimiter + SelectedAdaptationNId;

            InputParam += ParamDelimiter + di_jq("#txtDbAdmName").val();
            InputParam += ParamDelimiter + di_jq("#txtDbAdmInstitution").val();
            InputParam += ParamDelimiter + di_jq("#txtDbAdmEmail").val();
            InputParam += ParamDelimiter + di_jq("#ddlUnicefRegion").val();
            InputParam += ParamDelimiter + di_jq("#txtAdaptationYear").val();
            InputParam += ParamDelimiter + di_jq("#txtDatabaseLanguages").val();

            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '282', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data != "") {
                            ShowCatalogLists(true);

                            alert("Adaptation update successfully.")
                        }
                        else {
                            alert("Error while updating.");
                        }

                        RemoveLoaderNMask();
                    }
                    catch (ex) {
                        alert("message:" + ex.message);
                        RemoveLoaderNMask();
                    }
                },
                error: function () {
                    
                    RemoveLoaderNMask();
                },
                cache: false
            });
        }
    }
    catch (err) { }
}

function toggle_region_country(isCheckedAllArea) {
    var divAreas;

    try {
        filteredAreas = [];
        divAreas = z('cata_reg_country');

        while (divAreas.hasChildNodes()) {
            divAreas.removeChild(divAreas.lastChild);
        }

        if (IsQdsFiltered) {
            //Filter and show areas by searched adaptations
            SetFilterAdaptationQDSAreas();
        }
        else {
            for (var i = 0; i < jsonAreas.length; i++) {
                var tmpArea = getAreaLi(jsonAreas[i], isCheckedAllArea);
                divAreas.appendChild(tmpArea);
            }
        }

        renderFilteredAdaptations();

        if (isCheckedAllArea) {
            CheckedAllAdaptationAreas(true);
        }
    }
    catch (err) { }

}

function updateAdaptationsCount(AdaptationsCount) {
    try {
        z('spanAdaptationsCount').innerHTML = AdaptationsCount;
    }
    catch (err) { }
}

function ClearAreaSearchTxt() {
    try {
        di_jq("#txtAreaSearch").val('');
        ClearSelectedAreaLi();
    }
    catch (err) { }
}

function ClearSelectedAreaLi() {
    try {
        di_jq("#cata_reg_country li").each(function () {
            di_jq(this)[0].style.backgroundColor = "#fff";
        })
    } catch (err) { }
}

function SelectMatchAreas(event) {
    var SearchText = "";
    var AreaText = "";

    try {
        SearchText = di_jq.trim(di_jq("#txtAreaSearch").val().toLowerCase());
        if (SearchText != "") {
            di_jq("#cata_reg_country li label").each(function () {
                AreaText = "  " + di_jq(this).html().toLowerCase();
                if (AreaText.indexOf("  " + SearchText) > -1) {
                    di_jq(this).parent()[0].style.backgroundColor = "#BFBFBF";
                    di_jq("#cata_reg_country").scrollTop(parseInt(di_jq(this).parent()[0].offsetTop) - 443);
                }
                else {
                    di_jq(this).parent()[0].style.backgroundColor = "#fff";
                }
            })
        }
        else {
            ClearSelectedAreaLi();
        }
    }
    catch (err) { }
}

function FillAdaptationVersionDdl() {
    var VerNId;
    var VerName;

    try {
        if (di_jq("#ddlVersion").find("option").length == 0) {
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '287' },
                async: true,
                success: function (jsonData) {
                    try {
                        if (jsonData != "") {

                            jsonAdaptationVersions = jsonData;
                            jsonData = di_jq.parseJSON(jsonData)

                            //Fill dropdown with area JSON data
                            for (var i = 0; i < jsonData.AdaptationVersion.length; i++) {

                                VerNId = jsonData.AdaptationVersion[i].Ver_NId;
                                VerName = jsonData.AdaptationVersion[i].Ver_Name;

                                //Create and append option for dropdown
                                di_jq("#ddlVersion").append(di_jq('<option></option>').val(VerNId).html(VerName));
                            }
                        }
                        else {
                            alert("Error while updating.");
                        }
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
    }
    catch (err) { }
}

function CheckedAllAdaptationAreas(isChecked) {
    try {

        di_jq("#cata_reg_country input").each(function () {
            //di_jq(this).defaultChecked = isChecked;
            di_jq(this).attr("checked", isChecked)
        })
    }
    catch (err) { }
}

function CheckGlobalAdaptation() {
    try {
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '1009' },
            async: true,
            success: function (data) {
                try {
                    if (data == "false") {
                        z('aGotoGlobalCatalog').style.display = "none";
                        z('lang_box_blw_txt_GCatalog').style.display = "none";
                    }
                    else {
                        z('aGotoGlobalCatalog').style.display = "none"; //"";
                        z('aGotoGlobalCatalog').href = data;
                        z('aGotoGlobalCatalog').innerHTML = z('hGotoGlobalCatalog').value;
                        z('lang_box_blw_txt_GCatalog').style.display = "none"; //"";



                    }
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
    catch (err) { }
}