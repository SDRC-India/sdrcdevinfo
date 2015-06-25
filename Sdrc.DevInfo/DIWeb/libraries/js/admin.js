var SelectedDbId = "";
var PrevCategorySearchText = '';
var IsCategoryHover = false;
var CategorySelectedIndex = -1;
var PrevCategoryText = '';
var CloseImgSrc = "../../../stock/themes/default/images/close.png";

var DICategoryName = "DevInfo";

var ChkXmlSelectAll = "chkXmlSelectAll";
var ChkXmlICIUS = "chkXmlICIUS";
var ChkXmlIUS = "chkXmlIUS";
var ChkXmlIC = "chkXmlIC";
var ChkXmlArea = "chkXmlArea";
var ChkXmlTimePeriods = "chkXmlTimePeriods";
var ChkXmlMetadata = "chkXmlMetadata";
var ChkXmlAreaQuickSearch = "chkXmlAreaQuickSearch";
var ChkXmlFootnotes = "chkXmlFootnotes";

var TxtEmail = "#txtEmail";
var TxtFirstName = "#txtFirstName";
var DdlCountry = "#ddlSelectCountryUser";
var TxtPassword = "#txtPassword";
var TxtConfirmPassword = "#txtConfirmPassword";
var ChkSendDevInfoUpdates = "#chkSendDevInfoUpdates";

var TxtFPEmail = "#txtFPEmail";

var SelectedUserNId = "";

var DivLanguageGrid = "#divLanguageGrid";
var BtnLngSave = "#langSave";
var BtnLngCancel = "#langCancel";
var SelectedTxtId = "";
var LanguageTableName = "Lng_Table";

CallbackPageName = "../Callback.aspx";

var ErrorMessage = '';
var gCurrentPage;
var btnLangexport = "#lang_lm_ExportXML";
//-----------Begin Common methods---------------------------------------


function AddCPH(ctrlName) {
    return "ContentPlaceHolder1_" + ctrlName;
}

function ApplyMaskNLoader(zIndex) {
    ApplyMaskingDiv(zIndex);
    ShowLoadingDiv();
}

function RemoveLoaderNMask() {
    HideLoadingDiv();
    RemoveMaskingDiv();
}

String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, "");
}

String.prototype.ltrim = function () {
    return this.replace(/^\s+/, "");
}

String.prototype.rtrim = function () {
    return this.replace(/\s+$/, "");
}


function SelectAdaptedFor(adaptedtype) {
    SetAdaptedFor(adaptedtype, "", "");
}


function SetAdaptedFor(adaptedtype, areaNId, subNation) {
    var InputParam = "";
    var AreaNId;
    var AreaName;

    try {
        ApplyMaskNLoader();

        adaptedtype = adaptedtype.toLowerCase();

        di_jq("#ddlSelectCountry").attr("disabled", false);

        di_jq("#txtSubNation").val(subNation);
        di_jq("#txtSubNation").attr("disabled", true);

        switch (adaptedtype) {
            case "global":
                di_jq("#ddlSelectCountry").attr("disabled", true);
                break;

            case "national":
                break;

            case "sub-national":
                di_jq("#txtSubNation").attr("disabled", false);
                break;
        }


        if (adaptedtype == "global") {
            RemoveLoaderNMask();
        }
        else if (di_jq("#ddlSelectCountry").find("option").length == 0) {
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '270', 'param1': InputParam },
                async: true,
                success: function (jsonData) {
                    try {
                        if (jsonData != "") {
                            FillCountryDdl(jsonData, "ddlSelectCountry", areaNId);
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
            RemoveLoaderNMask();
        }
    }
    catch (err) { }
}

function FillCountryDdl(data, ddlId, selectedId) {
    try {
        //Remove all options of dropdown            
        di_jq("#" + ddlId).find("option").remove()

        jsonData = di_jq.parseJSON(data);

        //Fill dropdown with area JSON data
        for (var i = 0; i < jsonData.Areas.length; i++) {

            AreaNId = jsonData.Areas[i].AREANID;
            AreaName = jsonData.Areas[i].AREANAME;

            //Create and append option for dropdown
            di_jq("#" + ddlId).append(di_jq('<option></option>').val(AreaNId).html(AreaName));
        }

        if (selectedId != "") {
            di_jq("#" + ddlId).val(selectedId);
        }
    }
    catch (err) { }
}


function FillCategoryScheme(ddlId) {
    try {
        //Remove all options of dropdown            
        di_jq("#" + ddlId).find("option").remove()
        //Create and append option for dropdown
        di_jq("#" + ddlId).append(di_jq('<option></option>').val("ALL").html("All"));
        di_jq("#" + ddlId).append(di_jq('<option></option>').val("SC").html("Sector"));
        di_jq("#" + ddlId).append(di_jq('<option></option>').val("GL").html("Goal"));
        di_jq("#" + ddlId).append(di_jq('<option></option>').val("IT").html("Institution"));
        di_jq("#" + ddlId).append(di_jq('<option></option>').val("TH").html("Theme"));
        di_jq("#" + ddlId).append(di_jq('<option></option>').val("SR").html("Source"));
        di_jq("#" + ddlId).append(di_jq('<option></option>').val("CV").html("Convention"));
        di_jq("#" + ddlId).val("SC");
    }
    catch (err) { }
}
//-----------End Common methods---------------------------------------



//-----------Begin Menu---------------------------------------

function ClearMenuSelection() {
    try {
        z("aConfig").className = "";
        z("aDbConnec").className = "";
        z("aLangSetngs").className = "";
        z("aMaintenanceAgency").className = "";
        z("aUser").className = "";
        z("aCatalog").className = "";
    }
    catch (err) { }
}

function SelectLeftMenuItem(menuType) {
    var CtrlName = "";
    try {
        ClearMenuSelection();

        switch (menuType) {
            case "AppSettings":
                CtrlName = "aConfig";
                break;
            case "DbSettings":
                CtrlName = "aDbConnec";
                break;
            case "LngSettings":
                CtrlName = "aLangSetngs";
                break;
            case "User":
                CtrlName = "aUser";
                break;
            case "Catalog":
                CtrlName = "aCatalog";
                break;
            case "Header":
                CtrlName = "aHeaderDetails";
                break;
        }

        z(CtrlName).className = "adm_lft_nav_seld";
    }
    catch (err) { }
}

//-----------End Menu---------------------------------------


//-----------Begin Configuration Settings-----------------

function SetAppSettingsAdaptedArea(adaptedFor, areaNId, subNation) {
    try {
        adaptedFor = adaptedFor.toLowerCase();

        switch (adaptedFor) {
            case "global":
                di_jq("#rbGlobal").attr("checked", true)
                break;

            case "national":
                di_jq("#rbNational").attr("checked", true)
                break;

            case "sub-national":
                di_jq("#rbSubNational").attr("checked", true)
                break;
        }

        SetAdaptedFor(adaptedFor, areaNId, subNation);

        RemoveLoaderNMask();
    }
    catch (err) { }
}

function ValidateConfiguration() {
    var RetVal = true;

    try {
        if (di_jq("#" + AddCPH("txtAdaptationName")).val() == '') {
            alert("Enter adaptation name");
            di_jq("#" + AddCPH("txtAdaptationName")).focus();
            RetVal = false;
        }
        else if (di_jq("#rbSubNational").attr("checked") && di_jq("#txtSubNation").val() == '') {
            alert("Enter sub nation");
            di_jq("#txtSubNation").focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("ddlLanguage")).val() == -1) {
            alert("Select default language");
            di_jq("#" + AddCPH("ddlLanguage")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("ddlTheme")).val() == -1) {
            alert("Select theme");
            di_jq("#" + AddCPH("ddlTheme")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtSliderCount")).val() == '') {
            alert("Enter slider count");
            di_jq("#" + AddCPH("txtSliderCount")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtMrdThreshold")).val() == '') {
            alert("Enter MRD Threshold Value");
            di_jq("#" + AddCPH("txtMrdThreshold")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtJSVersion")).val() == '') {
            alert("Enter JS version");
            di_jq("#" + AddCPH("txtJSVersion")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtAdaptationYear")).val() == '') {
            alert("Enter adaptation year");
            di_jq("#" + AddCPH("txtAdaptationYear")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("ddlUnicefRegion")).val() == -1) {
            alert("Select unicef region");
            di_jq("#" + AddCPH("ddlUnicefRegion")).focus();
            RetVal = false;
        }


        //        //Contact of database administrator
        //        else if (di_jq("#" + AddCPH("txtAdmName")).val() == '') {
        //            alert("Enter administrator name");
        //            di_jq("#" + AddCPH("txtAdmName")).focus();
        //            RetVal = false;
        //        }
        //        else if (di_jq("#" + AddCPH("txtAdmInstitution")).val() == '') {
        //            alert("Enter administrator Institution");
        //            di_jq("#" + AddCPH("txtAdmInstitution")).focus();
        //            RetVal = false;
        //        }
        //        else if (di_jq("#" + AddCPH("txtAdmEmail")).val() == '') {
        //            alert("Enter administrator email");
        //            di_jq("#" + AddCPH("txtAdmEmail")).focus();
        //            RetVal = false;
        //        }
        //        else if(validateEmail(di_jq("#" + AddCPH("txtAdmEmail")).val()) == false) {
        //            alert("Incorrect email");
        //            di_jq("#" + AddCPH("txtAdmEmail")).focus();
        //            RetVal = false;
        //        }

        //Web Components
        else if (di_jq("#" + AddCPH("txtComponentVersion")).val() == '') {
            alert("Enter component version");
            di_jq("#" + AddCPH("txtVerstxtComponentVersionion")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtDiUiLibUrl")).val() == '') {
            alert("Enter component library URL");
            di_jq("#" + AddCPH("txtDiUiLibUrl")).focus();
            RetVal = false;
        }

        //Share
        else if (di_jq("#" + AddCPH("txtFBAppID")).val() == '') {
            alert("Enter facebook application id");
            di_jq("#" + AddCPH("txtFBAppID")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtFBAppSecret")).val() == '') {
            alert("Enter facebook secret key");
            di_jq("#" + AddCPH("txtFBAppSecret")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtTwAppID")).val() == '') {
            alert("Enter twitter application id");
            di_jq("#" + AddCPH("txtTwAppID")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtTwAppSecret")).val() == '') {
            alert("Enter twitter secret key");
            di_jq("#" + AddCPH("txtTwAppSecret")).focus();
            RetVal = false;
        }
        //Registry
        else if (di_jq("#" + AddCPH("txtAreaLevel")).val() == '') {
            alert("Enter Area Level");
            di_jq("#" + AddCPH("txtAreaLevel")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtMSDAreaId")).val() == '') {
            alert("Enter MSD Area Id");
            di_jq("#" + AddCPH("txtMSDAreaId")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtAgeDefaultvalue")).val() == '') {
            alert("Enter Default Value Of Age For Mapping");
            di_jq("#" + AddCPH("txtAgeDefaultvalue")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtSexDefaultvalue")).val() == '') {
            alert("Enter Default Value Of Sex For Mapping");
            di_jq("#" + AddCPH("txtSexDefaultvalue")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtLocationDefaultvalue")).val() == '') {
            alert("Enter Default Value Of Location For Mapping");
            di_jq("#" + AddCPH("txtLocationDefaultvalue")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtFrequencyDefaultvalue")).val() == '') {
            alert("Enter Default Value Of Frequency For Mapping");
            di_jq("#" + AddCPH("txtFrequencyDefaultvalue")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtSourceDefaultvalue")).val() == '') {
            alert("Enter Default Value Of Source For Mapping");
            di_jq("#" + AddCPH("txtSourceDefaultvalue")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtNatureDefaultvalue")).val() == '') {
            alert("Enter Default Value Of Nature For Mapping");
            di_jq("#" + AddCPH("txtNatureDefaultvalue")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtUnitMultDefaultvalue")).val() == '') {
            alert("Enter Default Value Of Unit Multiplier For Mapping");
            di_jq("#" + AddCPH("txtUnitMultDefaultvalue")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtGoogleAnalyticsId")).val() == '') {
            alert("Enter Google Analytics id");
            di_jq("#" + AddCPH("txtGoogleAnalyticsId")).focus();
            RetVal = false;
        }

    }
    catch (err) { }

    return RetVal;
}

function UpdateConfiguration() {
    var InputParam;
    var ShowSliders = "false";
    var HideSourceColumn = "false";
    var DSDSelection = "false";
    var QDSCache = "false";
    var ShowDIB = "false";
    var StandaloneRegistry = "false";
    var IsDesktopVerstionAvailble = "false";
    var QDSGallery = "false";
    var NewsMenuEnabled = "false"
    var InnovationsMenuEnabled = "false";
    var QDSCloud = "false";
    var ContactUs = "false";
    var Support = "false";
    var DownloadsLinkEnabled = "false";
    var TrainingLinkEnabled = "false";
    var MapLibraryLinkEnabled = "false";
    var RSSFeedsLinkEnabled = "false";
    var DiWorldWideLinkEnabled = "false";
    var FAQVisible = "false";
    var KBVisible = "false";
    var HowToVideo = "false";
    var Sitemap = "false";
    var Country = "";
    try {
        if (ValidateConfiguration()) {
            ApplyMaskNLoader();

            if (di_jq("#" + AddCPH("chkShowSliders")).attr('checked')) {
                ShowSliders = "true";
            }

            if (di_jq("#" + AddCPH("chkHideSrc")).attr('checked')) {
                HideSourceColumn = "true";
            }

            if (di_jq("#" + AddCPH("chkDSDSel")).attr('checked')) {
                DSDSelection = "true";
            }

            if (di_jq("#" + AddCPH("chkQDSCache")).attr('checked')) {
                QDSCache = "true";
            }

            if (di_jq("#" + AddCPH("chkDIB")).attr('checked')) {
                ShowDIB = "true";
            }

            if (di_jq("#" + AddCPH("rbAdpModeRegistry")).attr('checked')) {
                StandaloneRegistry = "true";
            }

            if (di_jq("#" + AddCPH("chkDesktopVerAvailable")).attr('checked')) {
                IsDesktopVerstionAvailble = "true";
            }

            if (di_jq("#" + AddCPH("chkQDSGallery")).attr('checked')) {
                QDSGallery = "true";
            }

            if (di_jq("#" + AddCPH("chkNewsMenu")).attr('checked')) {
                NewsMenuEnabled = "true";
            }

            if (di_jq("#" + AddCPH("chkInnovationsMenu")).attr('checked')) {
                InnovationsMenuEnabled = "true";
            }

            if (di_jq("#" + AddCPH("chkCloudQDS")).attr('checked')) {
                QDSCloud = "true";
            }
            if (di_jq("#" + AddCPH("chkContactUs")).attr('checked')) {
                ContactUs = "true";
            }
            if (di_jq("#" + AddCPH("chkSupport")).attr('checked')) {
                Support = "true";
            }
            if (di_jq("#" + AddCPH("chkDownloads")).attr('checked')) {
                DownloadsLinkEnabled = "true";
            }
            if (di_jq("#" + AddCPH("chkTraining")).attr('checked')) {
                TrainingLinkEnabled = "true";
            }
            if (di_jq("#" + AddCPH("chkMapLibrary")).attr('checked')) {
                MapLibraryLinkEnabled = "true";
            }
            if (di_jq("#" + AddCPH("chkRSSFeeds")).attr('checked')) {
                RSSFeedsLinkEnabled = "true";
            }
            if (di_jq("#" + AddCPH("chkDIWorldwide")).attr('checked')) {
                DiWorldWideLinkEnabled = "true";
            }
            if (di_jq("#" + AddCPH("chkFAQ")).attr('checked')) {
                FAQVisible = "true";
            }
            if (di_jq("#" + AddCPH("chkKB")).attr('checked')) {
                KBVisible = "true";
            }
            if (di_jq("#" + AddCPH("chkHowToVideo")).attr('checked')) {
                HowToVideo = "true";
            }
            if (di_jq("#" + AddCPH("chkSitemap")).attr('checked')) {
                Sitemap = "true";
            }

            InputParam = di_jq("#" + AddCPH("txtAdaptationName")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("ddlLanguage")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtComponentVersion")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtDiUiLibUrl")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("ddlTheme")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtFBAppID")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtFBAppSecret")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtTwAppID")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtTwAppSecret")).val();
            InputParam += ParamDelimiter + ShowSliders;
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtMrdThreshold")).val();
            InputParam += ParamDelimiter + HideSourceColumn;

            InputParam += ParamDelimiter + DSDSelection;
            InputParam += ParamDelimiter + QDSCache;
            InputParam += ParamDelimiter + ShowDIB;
            InputParam += ParamDelimiter + StandaloneRegistry;
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtJSVersion")).val();

            if (di_jq("#rbGlobal").attr("checked")) {
                InputParam += ParamDelimiter + "global";
                InputParam += ParamDelimiter + "-1";
                Country = "global";
            }
            else if (di_jq("#rbNational").attr("checked")) {
                InputParam += ParamDelimiter + "national";
                InputParam += ParamDelimiter + di_jq("#ddlSelectCountry").val();
                Country = di_jq("#ddlSelectCountry :selected").text();
            }
            else if (di_jq("#rbSubNational").attr("checked")) {
                InputParam += ParamDelimiter + "sub-national";
                InputParam += ParamDelimiter + di_jq("#ddlSelectCountry").val();
                Country = di_jq("#ddlSelectCountry :selected").text();
            }

            InputParam += ParamDelimiter + di_jq("#txtSubNation").val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtSliderCount")).val();

            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtAdaptationYear")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("ddlUnicefRegion")).val();
            InputParam += ParamDelimiter + IsDesktopVerstionAvailble;

            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtAdmName")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtAdmInstitution")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtAdmEmail")).val();
            InputParam += ParamDelimiter + QDSGallery;

            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtAreaLevel")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtMSDAreaId")).val();
            if (di_jq("#" + AddCPH("chkNotifyViaEmail")).attr('checked')) {
                InputParam += ParamDelimiter + "true";
            }
            else {
                InputParam += ParamDelimiter + "false";
            }
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtAgeDefaultvalue")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtSexDefaultvalue")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtLocationDefaultvalue")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtFrequencyDefaultvalue")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtSourceDefaultvalue")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtNatureDefaultvalue")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtUnitMultDefaultvalue")).val();

            InputParam += ParamDelimiter + NewsMenuEnabled;
            InputParam += ParamDelimiter + InnovationsMenuEnabled;
            InputParam += ParamDelimiter + QDSCloud;
            InputParam += ParamDelimiter + ContactUs;
            InputParam += ParamDelimiter + Support;
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtApplicationVersion")).val();
            InputParam += ParamDelimiter + DownloadsLinkEnabled;
            InputParam += ParamDelimiter + TrainingLinkEnabled;
            InputParam += ParamDelimiter + MapLibraryLinkEnabled;
            InputParam += ParamDelimiter + RSSFeedsLinkEnabled;
            InputParam += ParamDelimiter + DiWorldWideLinkEnabled;
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtGoogleAnalyticsId")).val();
            InputParam += ParamDelimiter + Country;
            InputParam += ParamDelimiter + di_jq("#selCategoryScheme").val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtAnimationTime")).val();
            InputParam += ParamDelimiter + FAQVisible;
            InputParam += ParamDelimiter + Sitemap;
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("MetaTag_Desc_Val")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("MetaTag_Kw_Val")).val();
            InputParam += ParamDelimiter + KBVisible;
            InputParam += ParamDelimiter + HowToVideo;


            //Add Param values for HowToVideoUpdate
            var PrmHowTovideo = "";
            //If language are set from Code Behind of AppSettings
            if (VAribaleDeclaredInpage != "") {
                //Break it into array and make a loop
                var LangArr = VAribaleDeclaredInpage.split(",");
                for (var i = 0; i < LangArr.length; i++) {
                    if (z(LangArr[i])) {
                        PrmHowTovideo = PrmHowTovideo + "[**]" + LangArr[i] + "[*]" + z("chkHowToVideo" + LangArr[i] + "H").checked + "[*]" + z("chkHowToVideo" + LangArr[i] + "V").checked + "[*]" + di_jq("#txtHowToVideoLink" + LangArr[i] + "H").val() + "[*]" + di_jq("#txtHowToVideoLink" + LangArr[i] + "V").val();
                    }
                }
            }
            InputParam += ParamDelimiter + PrmHowTovideo;

            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '18', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data == "true") {
                            //Save show slider status in cookie
                            createCookie("ShowSliders", ShowSliders);
                            alert("Saved successfully.");
                        }
                        else {
                            alert("Error while saving.");
                        }

                        RemoveLoaderNMask();
                    }
                    catch (ex) {
                        alert("Error : " + ex.message);
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

//-----------End Configuration Settings-----------------

//-----------Begin Database Settings Connection Details-----------------

function ValidateDbConnection() {
    var RetVal = true;

    try {
        if (di_jq("#" + AddCPH("txtConnName")).val() == '') {
            alert("Enter connection name");
            di_jq("#" + AddCPH("txtConnName")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("ddlDatabaseType")).val() == -1) {
            alert("Select database type");
            di_jq("#" + AddCPH("ddlDatabaseType")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtServerName")).val() == '') {
            alert("Enter sever/host name");
            di_jq("#" + AddCPH("txtServerName")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtDatabaseName")).val() == '') {
            alert("Enter database name");
            di_jq("#" + AddCPH("txtDatabaseName")).focus();
            RetVal = false;
        }
        else if (di_jq("#" + AddCPH("txtUserName")).val() == '') {
            alert("Enter user name");
            di_jq("#" + AddCPH("txtUserName")).focus();
            RetVal = false;
        }
    }
    catch (err) { }

    return RetVal;
}

function TestConnection() {
    var InputParam;

    try {
        if (ValidateDbConnection()) {
            ApplyMaskNLoader();
            InputParam = di_jq("#" + AddCPH("ddlDatabaseType")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtServerName")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtDatabaseName")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtUserName")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtPassword")).val();

            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '35', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data == "true") {
                            alert("Connection Successful !");
                            //di_jq("#btnConDetRegisterDb").attr("disabled", false);
                            EnableButton("#btnConDetRegisterDb");
                            if (z('btnConDetRegisterDb').style.display != "none") {
                                z('IsCatalogReg').style.display = "inline";
                                z('DefnCatlog').style.paddingLeft = "190px";
                            }
                            else {
                                z('IsCatalogReg').style.display = "none";
                                z('DefnCatlog').style.paddingLeft = "128px";
                            }
                        }
                        else {
                            alert("Can`t Create Connection !");
                        }
                        RemoveLoaderNMask();
                    }
                    catch (ex) {
                        alert("Error : " + ex.message);
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


function RegisterDatabase() {
    var InputParam;

    try {
        if (ValidateDbConnection()) {
            ApplyMaskNLoader();

            InputParam = DICategoryName;
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtConnName")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("ddlDatabaseType")).val() + "||" + di_jq("#" + AddCPH("txtServerName")).val() + "||" + di_jq("#" + AddCPH("txtDatabaseName")).val() + "||" + di_jq("#" + AddCPH("txtUserName")).val() + "||" + di_jq("#" + AddCPH("txtPassword")).val() + "||";

            InputParam += ParamDelimiter + "";
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtDesc")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("chkDefault")).attr("checked");
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("chkRegCatalog")).attr("checked");

            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '37', 'param1': encodeURIComponent(InputParam) },
                async: true,
                success: function (data) {
                    try {
                        if (data == "exists") {
                            alert("Connection name already exists.");
                        }
                        else if (data != "") {
                            SelectedDbId = data;

                            if (di_jq("#" + AddCPH("chkDefault")).attr("checked") == true) {
                                DefDbNId = data;
                            }

                            //di_jq("#btnConDetRegisterDb").attr("disabled", true);
                            DisableButton("#btnConDetRegisterDb");
                            z('IsCatalogReg').style.display = "none";
                            z('DefnCatlog').style.paddingLeft = "128px";


                            //di_jq("#btnConDetNext").attr("disabled", false);
                            EnableButton("#btnConDetNext");

                            alert("Database Registered Successfully !");
                        }
                        else {
                            alert("Error while Registering the Database !");
                        }

                        RemoveLoaderNMask();
                    }
                    catch (ex) {
                        alert("Error : " + ex.message);
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

function IsItDefaultDb() {
    var RetVal = false;
    var Tbl = z("db_Table");
    var Row;

    try {
        if (Tbl) {
            for (i = 1; i < Tbl.rows.length; i++) {
                Row = Tbl.rows[i];

                if (this.GetTblCellString(Row, 0).checked == true) {
                    if (this.GetTblCellValue(Row, 4).toLowerCase() == "true") {
                        RetVal = true;
                    }

                    break;
                }
            }
        }
    }
    catch (err) { }

    return RetVal;
}

function DeleteConnection() {
    var InputParam;

    try {
        if (di_jq("#" + AddCPH("divDbList") + " :checked").length > 0) {
            if (ConfirmToDelete('Delete this Connection?')) {
                if (IsItDefaultDb() == true) {
                    alert("It's a default database, first change default database to another database.");
                }
                else {
                    ApplyMaskingDiv();
                    ShowLoadingDiv();


                    di_jq("#" + AddCPH("divDbList") + " :checked").each(function () {
                        InputParam = di_jq(this).context.id.substr(3);
                    });


                    di_jq.ajax({
                        type: "POST",
                        url: CallbackPageName,
                        data: { 'callback': '36', 'param1': InputParam },
                        async: true,
                        success: function (data) {
                            try {
                                if (data == "true") {
                                    GetAllDbConnections();
                                    alert("Delete Successful !");
                                    ShowAddNewConnectionButton();
                                    // CheckForExistingConnections();
                                }
                                else {
                                    alert("Can`t Delete Connection !");
                                }

                                RemoveLoaderNMask();
                            }
                            catch (ex) {
                                alert("Error : " + ex.message);
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
        }
        else {
            alert("Select a connection.");
        }
    }
    catch (err) { }
}

function GetAllDbConnections() {
    var InputParam;

    try {
        InputParam = DICategoryName;

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '38', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    di_jq("#" + AddCPH("divDbList")).html(data);
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
    catch (err) { }
}


function ClearControlsValues() {
    try {
        di_jq("#" + AddCPH("txtCategory")).val('');
        di_jq("#" + AddCPH("ddlDatabaseType")).val('-1');
        di_jq("#" + AddCPH("txtConnName")).val('');
        di_jq("#" + AddCPH("txtServerName")).val('');
        di_jq("#" + AddCPH("txtDatabaseName")).val('');
        di_jq("#" + AddCPH("txtUserName")).val('');
        di_jq("#" + AddCPH("txtPassword")).val('');
        di_jq("#" + AddCPH("txtDesc")).val('');
        //di_jq("#" + AddCPH("chkDefault")).attr("checked", false);
    }
    catch (err) { }
}

function MoveOnConnectionDetails() {
    try {
        if (SelectedDbId == "") {
            SelectedDbId = getQueryStr("id");
        }

        window.location = "ConnectionDetails.aspx?id=" + SelectedDbId;
    }
    catch (err) { }
}

function MoveOnProviderDetails() {
    try {
        if (SelectedDbId == "") {
            SelectedDbId = getQueryStr("id");
        }

        window.location = "ProviderDetails.aspx?id=" + SelectedDbId;
    }
    catch (err) { }
}

function MoveOnOptimizeDb() {
    try {
        if (SelectedDbId == "") {
            SelectedDbId = getQueryStr("id");
        }

        window.location = "OptimizeDb.aspx?id=" + SelectedDbId;
    }
    catch (err) { }
}

function MoveOnDefaultArea() {
    try {
        if (SelectedDbId == "") {
            SelectedDbId = getQueryStr("id");
        }

        window.location = "DefaultArea.aspx?id=" + SelectedDbId;
    }
    catch (err) { }
}

function EditConnectionDetails() {
    var dbId = "";

    try {
        if (di_jq("#" + AddCPH("divDbList") + " :checked").length > 0) {
            di_jq("#" + AddCPH("divDbList") + " :checked").each(function () {
                dbId = di_jq(this).context.id;
            });

            window.location = "ConnectionDetails.aspx?id=" + dbId.substr(3);
        }
        else {
            alert("Select a connection.");
        }
    }
    catch (err) { }
}

function CheckForExistingConnections() {
    try {
        var noOfConnections = CheckforDBConnections();
        if (noOfConnections == 0) {
            z('langNew').style.display = "";
            z('liNewPipeline').style.display = "";
        }
        //window.location = "ConnectionDetails.aspx";

        else {
            //alert(z('langNewConnectionCheck').value);
            z('langNew').style.display = "none";
            z('liNewPipeline').style.display = "none";
        }
    }
    catch (err) { }
}


function ShowAddNewConnectionButton() {
    try {

        z('langNew').style.display = "";
        z('liNewPipeline').style.display = "";
    }
    catch (err) { }
}

function CheckforDBConnections() {
    var XmlPageURL = getAbsURL('stock') + "db.xml" + "?v=" + AppVersion;
    var count = 0;
    di_jq.ajax({
        type: "GET",
        url: XmlPageURL,
        dataType: "xml",
        async: false,
        timeout: 5000,
        error: function (request, error) {
        },
        success: function (xml) {

            count = di_jq(xml).find('db').length;
        }
    });
    return count;
}

function ShowHideConDetButton() {
    var dbId = "";

    try {
        dbId = getQueryStr("id");

        if (dbId != undefined) {
            SelectedDbId = dbId;
            di_jq("#btnConDetRegisterDb").css("display", "none");
            di_jq("#btnConDetUpdate").css("display", "block");
            di_jq("#LangUpdateNote").css("display", "block");
            z('IsCatalogReg').style.display = "none";
            z('DefnCatlog').style.paddingLeft = "128px";

            //di_jq("#btnConDetNext").attr("disabled", false);
            EnableButton("#btnConDetNext");
        }
        else {
            di_jq("#btnConDetRegisterDb").css("display", "block");
            di_jq("#btnConDetUpdate").css("display", "none");
            di_jq("#LangUpdateNote").css("display", "none");

        }
    }
    catch (err) { }
}

function UpdateDbConnection() {
    var InputParam;

    try {
        if (ValidateDbConnection()) {
            ApplyMaskNLoader();

            InputParam = SelectedDbId;
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtConnName")).val();

            InputParam += ParamDelimiter + di_jq("#" + AddCPH("ddlDatabaseType")).val() + "||" + di_jq("#" + AddCPH("txtServerName")).val() + "||" + di_jq("#" + AddCPH("txtDatabaseName")).val() + "||" + di_jq("#" + AddCPH("txtUserName")).val() + "||";
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtPassword")).val();

            InputParam += ParamDelimiter + di_jq("#" + AddCPH("txtDesc")).val();
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("chkDefault")).attr("checked");
            InputParam += ParamDelimiter + DICategoryName;

            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '24', 'param1': encodeURIComponent(InputParam) },
                async: true,
                success: function (data) {
                    try {
                        if (data == "exists") {
                            alert("Connection name already exists.");
                        }
                        else if (data == "true") {
                            if (di_jq("#" + AddCPH("chkDefault")).attr("checked") == true) {
                                DefDbNId = SelectedDbId;
                            }

                            alert("Saved successfully.");
                        }
                        else {
                            alert("Error while saving.");
                        }

                        RemoveLoaderNMask();
                    }
                    catch (ex) {
                        alert("Error : " + ex.message);
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


function CategoryTxtClick() {
    try {
        PrevCategoryText = di_jq("#" + AddCPH("txtCategory")).val();
        di_jq("#" + AddCPH("txtCategory")).val('');
    }
    catch (err) { }
}

function SearchCategory(objTxt, event) {
    var SearchString = di_jq.trim(objTxt.value);
    var CategoryListDiv = di_jq("#CategoryListDiv");
    var AreaTxtWidth;

    var FileName = '';
    var SearchPatern = '';
    var CategoryName = '';

    var SearchIndex = -1;
    var CategoryNameHTML = '';
    var SBHtml = '';
    var data = '';

    var i;
    var id;
    var TotalLICount;
    var DispAreaLength = -1;

    try {
        if (event.keyCode == 27) {
            //For escape button handling
            CategoryListDiv.hide();
            di_jq("#" + AddCPH("txtCategory")).val('');
            CategorySelectedIndex = -1;
        }
        else if (event.keyCode == 40) {
            //Down arrow key handling

            //Count total li in current selection div
            TotalLICount = di_jq("#divCategoryLI li").length;

            //Check any previous selection is persist, then remove selection
            if (CategorySelectedIndex > -1) {
                di_jq("#divCategoryLI li")[CategorySelectedIndex].className = '';
            }
            else if (CategorySelectedIndex == -1) {
                di_jq("#divCategoryLI").scrollTop(0);
            }

            CategorySelectedIndex++;

            //Reset current index to 0 if current is last index
            if (CategorySelectedIndex == TotalLICount) {
                CategorySelectedIndex = 0;
                di_jq("#divCategoryLI").scrollTop(0);
            }


            //Apply hover class on selected li
            di_jq("#divCategoryLI li")[CategorySelectedIndex].className = 'di_gui_label_hover';

            //Scroll vertical bar down
            if (CategorySelectedIndex > 8) {
                di_jq("#divCategoryLI").scrollTop(di_jq("#divCategoryLI").scrollTop() + 23);
            }
        }
        else if (event.keyCode == 38) {
            //Up arrow key handling

            //Count total li in current selection div
            TotalLICount = di_jq("#divCategoryLI li").length;

            if (CategorySelectedIndex > -1) {
                di_jq("#divCategoryLI li")[CategorySelectedIndex].className = '';
            }
            else if (CategorySelectedIndex == -1) {
                CategorySelectedIndex = 0;
            }

            CategorySelectedIndex--;

            //Reset current index to 0 if current is last index
            if (CategorySelectedIndex == -1) {
                CategorySelectedIndex = TotalLICount - 1;
                di_jq("#divCategoryLI").scrollTop(di_jq("#divCategoryLI").height());
            }

            di_jq("#divCategoryLI li")[CategorySelectedIndex].className = 'di_gui_label_hover';

            //Scroll vertical bar up
            if (CategorySelectedIndex < TotalLICount - 9) {
                di_jq("#divCategoryLI").scrollTop(di_jq("#divCategoryLI").scrollTop() - 23);
            }
        }
        else if (event.keyCode == 13) {
            di_jq("#divCategoryLI li")[CategorySelectedIndex].onclick();
        }
        else {
            if (SearchString != "") {
                //Start searching only when search characters lenght is more than 1
                if (SearchString.length > 0) {
                    FileName = getAbsURL('stock') + "db.xml";

                    di_jq.ajax({
                        type: "GET",
                        url: FileName,
                        dataType: "xml",
                        async: true,
                        success: function (xml) {
                            try {
                                SearchPatern = 'category';

                                di_jq(xml).find(SearchPatern).each(function () {
                                    CategoryName = di_jq(this).attr("name");

                                    SearchIndex = (" " + CategoryName.toLowerCase()).indexOf(" " + SearchString.toLowerCase());

                                    // Set display text
                                    if (SearchIndex >= 0) {
                                        // Create LI HTML for each area
                                        SBHtml += "<li onclick='SelectCategory(\"" + CategoryName + "\");' onmouseover='CategoryLiOnMouseover(this);' onmouseout='CategoryLiOnMouseout(this);' style='padding:3px;'>" + CategoryName + "</li>";
                                    }
                                });

                                // If HTML LI exist for areas                              
                                if (SBHtml != '') {
                                    data = "<table id='tblSelArea' cellspacing='0' cellpadding='0' width='100%'><tr><td><div id='divCategoryLI' class='di_qds_listview_li_div di_gui_body' style='padding-top:0px;'>" + SBHtml + "</div></td></tr></table>";
                                }

                                // Show selection list div if html data exists for searched text        
                                if (data != "") {
                                    CategoryListDiv.slideDown("fast");
                                    CategoryListDiv.width(di_jq("#" + AddCPH("txtCategory")).width());
                                    CategoryListDiv.html(data);

                                    //Select 1st index of category list
                                    CategorySelectedIndex = 0;
                                    di_jq("#divCategoryLI li")[CategorySelectedIndex].className = 'di_gui_label_hover';
                                }
                                else {
                                    CategoryListDiv.html("");
                                    CategoryListDiv.hide();
                                }
                            }
                            catch (ex) {
                                alert("message:" + ex.message);
                            }
                        },
                        error: function () {
                            CategoryListDiv.html("");
                            CategoryListDiv.hide();
                        },
                        cache: false
                    });
                }
            }
            else {
                CategoryListDiv.html("");
                CategoryListDiv.hide();
            }

            PrevCategorySearchText = SearchString;
        }
    }
    catch (err) { }
}

function SelectCategory(categoryText) {
    try {
        di_jq("#" + AddCPH("txtCategory")).val(categoryText);
        di_jq("#CategoryListDiv").hide();
    }
    catch (err) { }
}

/* Call on li mouseover event */
function CategoryLiOnMouseover(obj_li) {
    try {
        di_jq(obj_li).addClass('di_gui_label_hover');
        di_jq(obj_li).css("cursor", "pointer");
        IsCategoryHover = true;
    }
    catch (err) { }
}

/* Call on li mouseout event */
function CategoryLiOnMouseout(obj_li) {
    try {
        di_jq(obj_li).removeClass('di_gui_label_hover');
        IsCategoryHover = false;
    }
    catch (err) { }
}

function CategoryTxtBlur() {
    try {
        if (!IsCategoryHover) {
            di_jq("#CategoryListDiv").hide();
        }

        if (di_jq("#" + AddCPH("txtCategory")).val() == '') {
            di_jq("#" + AddCPH("txtCategory")).val(PrevCategoryText);
        }
    }
    catch (err) { }
}

//-----------End Database Settings Connection Details-----------------


//-----------Begin Database Settings Optimize Database-----------------
function CheckCustomParams() {
    var divCustomParams;
    divCustomParams = z('divCustomParams');
    if (z("chkDBCacheResultsGeneration").checked) {
        divCustomParams.style.display = "inline";
        if (z('txtCustomParams').value == "") {
            z('txtCustomParams').value = z('hcustomParams').value;
        }
    }
    else {
        divCustomParams.style.display = "none";
    }


}
function OptimizeDatabase() {
    var dbId = "";
    try {
        dbId = getQueryStr("id");

        if (dbId != undefined) {
            ApplyMaskingDiv();

            //            if (z("chkDBGeneration").checked) {
            //                DBScriptGeneration(dbId);
            //            }
            if (z("chkXMLGeneration").checked) {
                XMLGeneration(dbId);
            }
            else if (z("chkMapGeneration").checked) {
                MapFilesGeneration(dbId);
            }
            else if (z("chkSDMXGeneration").checked) {
                SDMXArtefactsGeneration(dbId);
            }
            else if (z("chkSDMXMLGeneration").checked) {
                SDMXMLGeneration(dbId);
            }
            else if (z("chkMetadataGeneration").checked) {
                MetadataGeneration(dbId);
            }
            else if (z("chkDBCacheResultsGeneration").checked) {
                if (z('txtCustomParams').value == "")
                    GenerateCache(dbId, z('hcustomParams').value);
                else
                    GenerateCache(dbId, z('txtCustomParams').value);
            }
            else if (z("chkSDMXMLRegistration").checked) {
                SDMXMLRegistration(dbId);
            }
            else if (z("chkMRRegistration").checked) {
                MetadataReportRegistration(dbId);
            }

            else if (z("chkGenerateCSVFile").checked) {
                GenerateCSVFile(dbId);
            }
            else if (z("chkPublishSDMXData").checked) {
                PublishSDMXData(dbId, "DevInfo");
            }
            else if (di_jq("input[id$=chkPublishSDMXDataCountryData]").attr("checked")) {
                PublishSDMXCountryData(dbId, "CountryData");
            }

            else {
                RemoveMaskingDiv();
            }
            CreateLogsForOptimizeDatabase();
        }
    }
    catch (err) { }
}

//-----------Begin Create CSV file Logs for Optimise DataBase------------
function WriteLogToOptimizeDatabase(CheckedItems) {
    try {
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '1016', 'param1': CheckedItems },
            async: true,
            success: function (data) {
                try {
                    if (data == "true") {
                    }
                }
                catch (ex) {

                }
            },
            error: function () {
                di_jq("#imgDBProcessing").css("display", "none");
                di_jq("#imgDBError").css("display", "block");


                RemoveMaskingDiv();
            },
            cache: false
        });

    }
    catch (err) { }
}

function CreateLogsForOptimizeDatabase() {
    var SelectedCheckBoxes = [];
    var CheckedItemList = '';
    if (z("chkDBGeneration").checked) {
        ChkBoxSelection(SelectedCheckBoxes, z("langUpdateDBSchema").innerHTML);
    }
    if (z("chkXMLGeneration").checked) {
        ChkBoxSelection(SelectedCheckBoxes, z("langXMLGeneration").innerHTML);
    }
    if (z("chkMapGeneration").checked) {
        ChkBoxSelection(SelectedCheckBoxes, z("langMapFilesGeneration").innerHTML);
    }

    if (z("chkDBCacheResultsGeneration").checked) {
        ChkBoxSelection(SelectedCheckBoxes, z("langGenerateCacheResults").innerHTML);
    }
    if (z("chkGenerateCSVFile").checked) {
        ChkBoxSelection(SelectedCheckBoxes, z("LangGenerateCSVFile").innerHTML);
    }
    if (z("chkPublishSDMXData").checked) {
        ChkBoxSelection(SelectedCheckBoxes, z("LangPublishSDMXData").innerHTML);
    }
    if (SelectedCheckBoxes.length > 0) {
        CheckedItemList = FormatInputToList(SelectedCheckBoxes);
        WriteLogToOptimizeDatabase(CheckedItemList);
    }
}

function ChkBoxSelection(selectedArry, val) {
    var index = -1;
    try {
        index = GetArrayIndex(selectedArry, val);
        if (index == -1) {
            selectedArry.push(val);
        }
        else {
            selectedArry.splice(index, 1);
        }
    }
    catch (err) {
        HandleJsError(err, "SelectCountries");
    }
}

function GetArrayIndex(arr, val) {
    var index = -1;
    for (var i = 0; i < arr.length; i++)
        if (arr[i] == val) {
            index = i;
            break;
        }
    return index;
}

function FormatInputToList(InputArr) {
    var RetVal = "";
    try {
        for (var i = 0; i < InputArr.length; i++) {
            RetVal += "," + InputArr[i];
        }
        if (RetVal.length > 0) {
            RetVal = RetVal.substr(1);
        }
        return RetVal;
    }
    catch (err) {
        HandleJsError(err, "FormatInputToList");
    }
}
//-----------End Create CSV file Logs for Optimise DataBase------------

function XMLGeneration(dbId) {
    var InputParam = "";
    var SelectedDataTypes = "";
    var CommaDelimiter = ",";
    var QuickSelectionType = "";
    var AreaOrderBy = "";

    try {
        di_jq("#imgXMLError").css("display", "none");
        di_jq("#imgXMLTickGray").css("display", "none");
        di_jq("#imgXMLTick").css("display", "none");
        di_jq("#imgXMLProcessing").css("display", "block");


        InputParam = dbId + ParamDelimiter;

        if (z(ChkXmlArea).checked) {
            SelectedDataTypes += CommaDelimiter + z(ChkXmlArea).value;

            if (z("rbSortAreaName").checked) {
                AreaOrderBy = z("rbSortAreaName").value;
            }
            else if (z("rbSortAreaId").checked) {
                AreaOrderBy = z("rbSortAreaId").value;
            }
        }

        if (z(ChkXmlFootnotes).checked) {
            SelectedDataTypes += CommaDelimiter + z(ChkXmlFootnotes).value;
        }

        if (z(ChkXmlIC).checked) {
            SelectedDataTypes += CommaDelimiter + z(ChkXmlIC).value;
        }

        if (z(ChkXmlICIUS).checked) {
            SelectedDataTypes += CommaDelimiter + z(ChkXmlICIUS).value;
        }

        if (z(ChkXmlIUS).checked) {
            SelectedDataTypes += CommaDelimiter + z(ChkXmlIUS).value;
        }

        if (z(ChkXmlMetadata).checked) {
            SelectedDataTypes += CommaDelimiter + z(ChkXmlMetadata).value;
        }

        if (z(ChkXmlAreaQuickSearch).checked) {
            SelectedDataTypes += CommaDelimiter + z(ChkXmlAreaQuickSearch).value;

            if (z("rbQSImmediate").checked) {
                QuickSelectionType = z("rbQSImmediate").value;
            }
            else if (z("rbQSAll").checked) {
                QuickSelectionType = z("rbQSAll").value;
            }
            else if (z("rbQSNone").checked) {
                QuickSelectionType = z("rbQSNone").value;
            }
        }

        if (z(ChkXmlTimePeriods).checked) {
            SelectedDataTypes += CommaDelimiter + z(ChkXmlTimePeriods).value;
        }

        if (SelectedDataTypes != "") {
            InputParam += SelectedDataTypes.substr(1) + ParamDelimiter + AreaOrderBy + ParamDelimiter + QuickSelectionType;
        }


        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '39', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    di_jq("#imgXMLProcessing").css("display", "none");

                    if (data == "true") {
                        di_jq("#imgXMLTick").css("display", "block");

                        if (z("chkMapGeneration").checked) {
                            MapFilesGeneration(dbId);
                        }
                        else if (z("chkSDMXGeneration").checked) {
                            SDMXArtefactsGeneration(dbId);
                        }
                        else if (z("chkSDMXMLGeneration").checked) {
                            SDMXMLGeneration(dbId);
                        }
                        else if (z("chkMetadataGeneration").checked) {
                            MetadataGeneration(dbId);
                        }
                        else if (z("chkDBCacheResultsGeneration").checked) {
                            if (z('txtCustomParams').value == "")
                                GenerateCache(dbId, z('hcustomParams').value);
                            else
                                GenerateCache(dbId, z('txtCustomParams').value);
                        }
                        else if (z("chkSDMXMLRegistration").checked) {
                            SDMXMLRegistration(dbId);
                        }
                        else if (z("chkMRRegistration").checked) {
                            MetadataReportRegistration(dbId);
                        }
                        else if (z("chkGenerateCSVFile").checked) {
                            GenerateCSVFile(dbId);
                        }
                        //                        else if (z("chkPublishSDMXData").checked) {
                        //                            PublishSDMXData(dbId);
                        //                        }
                        else if (z("chkPublishSDMXData").checked) {
                            PublishSDMXData(dbId, "DevInfo");
                        }
                        else if (di_jq("input[id$=chkPublishSDMXDataCountryData]").attr("checked")) {
                            PublishSDMXCountryData(dbId, "CountryData");
                        }
                        else {
                            //di_jq("#btnOptDbNext").attr("disabled", false);
                            EnableButton("#btnOptDbNext");
                            alert("Generated successfully");
                            RemoveMaskingDiv();
                        }
                    }
                    else {
                        di_jq("#imgXMLError").css("display", "block");
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    di_jq("#imgXMLProcessing").css("display", "none");
                    di_jq("#imgXMLError").css("display", "block");
                    alert("Error : " + ex.message);
                    RemoveMaskingDiv();
                }
            },
            error: function () {
                di_jq("#imgXMLProcessing").css("display", "none");
                di_jq("#imgXMLError").css("display", "block");

                RemoveMaskingDiv();
            },
            cache: false
        });

    }
    catch (err) { }
}

function MapFilesGeneration(dbId) {
    try {
        di_jq("#imgMapError").css("display", "none");
        di_jq("#imgMapTickGray").css("display", "none");
        di_jq("#imgMapTick").css("display", "none");
        di_jq("#imgMapProcessing").css("display", "block");

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '74', 'param1': dbId },
            async: true,
            success: function (data) {
                try {
                    di_jq("#imgMapProcessing").css("display", "none");

                    if (data == "true") {
                        di_jq("#imgMapTick").css("display", "block");

                        if (z("chkSDMXGeneration").checked) {
                            SDMXArtefactsGeneration(dbId);
                        }
                        else if (z("chkSDMXMLGeneration").checked) {
                            SDMXMLGeneration(dbId);
                        }
                        else if (z("chkMetadataGeneration").checked) {
                            MetadataGeneration(dbId);
                        }
                        else if (z("chkDBCacheResultsGeneration").checked) {
                            if (z('txtCustomParams').value == "")
                                GenerateCache(dbId, z('hcustomParams').value);
                            else
                                GenerateCache(dbId, z('txtCustomParams').value);
                        }
                        else if (z("chkSDMXMLRegistration").checked) {
                            SDMXMLRegistration(dbId);
                        }
                        else if (z("chkMRRegistration").checked) {
                            MetadataReportRegistration(dbId);
                        }
                        else if (z("chkGenerateCSVFile").checked) {
                            GenerateCSVFile(dbId);
                        }
                        //                        else if (z("chkPublishSDMXData").checked) {
                        //                            PublishSDMXData(dbId);
                        //                        }
                        else if (z("chkPublishSDMXData").checked) {
                            PublishSDMXData(dbId, "DevInfo");
                        }
                        else if (di_jq("input[id$=chkPublishSDMXDataCountryData]").attr("checked")) {
                            PublishSDMXCountryData(dbId, "CountryData");
                        }
                        else {
                            //di_jq("#btnOptDbNext").attr("disabled", false);
                            EnableButton("#btnOptDbNext");
                            alert("Generated successfully");
                            RemoveMaskingDiv();
                        }
                    }
                    else {
                        di_jq("#imgMapError").css("display", "block");
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    di_jq("#imgMapProcessing").css("display", "none");
                    di_jq("#imgMapError").css("display", "block");
                    alert("Error : " + ex.message);

                    RemoveMaskingDiv();
                }
            },
            error: function () {
                di_jq("#imgMapProcessing").css("display", "none");
                di_jq("#imgMapXError").css("display", "block");


                RemoveMaskingDiv();
            },
            cache: false
        });

    }
    catch (err) { }
}

function SDMXArtefactsGeneration(dbId) {
    try {
        di_jq("#imgSDMXError").css("display", "none");
        di_jq("#imgSDMXTickGray").css("display", "none");
        di_jq("#imgSDMXTick").css("display", "none");
        di_jq("#imgSDMXProcessing").css("display", "block");

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '47', 'param1': dbId },
            async: true,
            success: function (data) {
                try {
                    di_jq("#imgSDMXProcessing").css("display", "none");

                    if (data == "true") {
                        di_jq("#imgSDMXTick").css("display", "block");

                        if (z("chkSDMXMLGeneration").checked) {
                            SDMXMLGeneration(dbId);
                        }
                        else if (z("chkMetadataGeneration").checked) {
                            MetadataGeneration(dbId);
                        }
                        else if (z("chkDBCacheResultsGeneration").checked) {
                            if (z('txtCustomParams').value == "")
                                GenerateCache(dbId, z('hcustomParams').value);
                            else
                                GenerateCache(dbId, z('txtCustomParams').value);
                        }
                        else if (z("chkSDMXMLRegistration").checked) {
                            SDMXMLRegistration(dbId);
                        }
                        else if (z("chkMRRegistration").checked) {
                            MetadataReportRegistration(dbId);
                        }
                        else if (z("chkGenerateCSVFile").checked) {
                            GenerateCSVFile(dbId);
                        }
                        //                        else if (z("chkPublishSDMXData").checked) {
                        //                            PublishSDMXData(dbId);
                        //                        }
                        else if (z("chkPublishSDMXData").checked) {
                            PublishSDMXData(dbId, "DevInfo");
                        }
                        else if (di_jq("input[id$=chkPublishSDMXDataCountryData]").attr("checked")) {
                            PublishSDMXCountryData(dbId, "CountryData");
                        }
                        else {
                            //di_jq("#btnOptDbNext").attr("disabled", false);
                            EnableButton("#btnOptDbNext");
                            alert("Generated successfully");
                            RemoveMaskingDiv();
                        }
                    }
                    else {
                        di_jq("#imgSDMXError").css("display", "block");
                        alert(data);
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    di_jq("#imgSDMXProcessing").css("display", "none");
                    di_jq("#imgSDMXError").css("display", "block");
                    alert("Error : " + ex.message);

                    RemoveMaskingDiv();
                }
            },
            error: function () {
                di_jq("#imgSDMXProcessing").css("display", "none");
                di_jq("#imgSDMXError").css("display", "block");


                RemoveMaskingDiv();
            },
            cache: false
        });

    }
    catch (err) { }
}

function SDMXMLGeneration(dbId) {
    try {
        di_jq("#imgSDMXMLError").css("display", "none");
        di_jq("#imgSDMXMLTickGray").css("display", "none");
        di_jq("#imgSDMXMLTick").css("display", "none");
        di_jq("#imgSDMXMLProcessing").css("display", "block");

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '86', 'param1': dbId },
            async: true,
            success: function (data) {
                try {
                    di_jq("#imgSDMXMLProcessing").css("display", "none");

                    if (data == "true") {
                        di_jq("#imgSDMXMLTick").css("display", "block");

                        if (z("chkMetadataGeneration").checked) {
                            MetadataGeneration(dbId);
                        }
                        else if (z("chkDBCacheResultsGeneration").checked) {
                            if (z('txtCustomParams').value == "")
                                GenerateCache(dbId, z('hcustomParams').value);
                            else
                                GenerateCache(dbId, z('txtCustomParams').value);
                        }
                        else if (z("chkSDMXMLRegistration").checked) {
                            SDMXMLRegistration(dbId);
                        }
                        else if (z("chkMRRegistration").checked) {
                            MetadataReportRegistration(dbId);
                        }
                        else if (z("chkGenerateCSVFile").checked) {
                            GenerateCSVFile(dbId);
                        }
                        //                        else if (z("chkPublishSDMXData").checked) {
                        //                            PublishSDMXData(dbId);
                        //                        }
                        else if (z("chkPublishSDMXData").checked) {
                            PublishSDMXData(dbId, "DevInfo");
                        }
                        else if (di_jq("input[id$=chkPublishSDMXDataCountryData]").attr("checked")) {
                            PublishSDMXCountryData(dbId, "CountryData");
                        }
                        else {
                            //di_jq("#btnOptDbNext").attr("disabled", false);
                            EnableButton("#btnOptDbNext");
                            alert("Generated successfully");
                            RemoveMaskingDiv();
                        }
                    }
                    else {
                        di_jq("#imgSDMXMLError").css("display", "block");
                        alert(data);
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    di_jq("#imgSDMXMLProcessing").css("display", "none");
                    di_jq("#imgSDMXMLError").css("display", "block");
                    alert("Error : " + ex.message);

                    RemoveMaskingDiv();
                }
            },
            error: function () {
                di_jq("#imgSDMXMLProcessing").css("display", "none");
                di_jq("#imgSDMXMLError").css("display", "block");


                RemoveMaskingDiv();
            },
            cache: false
        });

    }
    catch (err) { }
}

function MetadataGeneration(dbId) {
    try {
        di_jq("#imgMetadataError").css("display", "none");
        di_jq("#imgMetadataTickGray").css("display", "none");
        di_jq("#imgMetadataTick").css("display", "none");
        di_jq("#imgMetadataProcessing").css("display", "block");

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '94', 'param1': dbId },
            async: true,
            success: function (data) {
                try {
                    di_jq("#imgMetadataProcessing").css("display", "none");

                    if (data == "true") {
                        di_jq("#imgMetadataTick").css("display", "block");

                        if (z("chkDBCacheResultsGeneration").checked) {
                            if (z('txtCustomParams').value == "")
                                GenerateCache(dbId, z('hcustomParams').value);
                            else
                                GenerateCache(dbId, z('txtCustomParams').value);
                        }
                        else if (z("chkSDMXMLRegistration").checked) {
                            SDMXMLRegistration(dbId);
                        }
                        else if (z("chkMRRegistration").checked) {
                            MetadataReportRegistration(dbId);
                        }
                        else if (z("chkGenerateCSVFile").checked) {
                            GenerateCSVFile(dbId);
                        }
                        //                        else if (z("chkPublishSDMXData").checked) {
                        //                            PublishSDMXData(dbId);
                        //                        }
                        else if (z("chkPublishSDMXData").checked) {
                            PublishSDMXData(dbId, "DevInfo");
                        }
                        else if (di_jq("input[id$=chkPublishSDMXDataCountryData]").attr("checked")) {
                            PublishSDMXCountryData(dbId, "CountryData");
                        }
                        else {
                            //di_jq("#btnOptDbNext").attr("disabled", false);
                            EnableButton("#btnOptDbNext");
                            alert("Generated successfully");
                            RemoveMaskingDiv();
                        }
                    }
                    else {
                        di_jq("#imgMetadataError").css("display", "block");
                        //alert(data);
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    di_jq("#imgMetadataProcessing").css("display", "none");
                    di_jq("#imgMetadataError").css("display", "block");
                    alert("Error : " + ex.message);

                    RemoveMaskingDiv();
                }
            },
            error: function () {
                di_jq("#imgMetadataProcessing").css("display", "none");
                di_jq("#imgMetadataError").css("display", "block");


                RemoveMaskingDiv();
            },
            cache: false
        });

    }
    catch (err) { }
}

function GenerateCache(dbId, AdditionalParam) {
    var InputParam = "";

    try {
        di_jq("#imgCacheError").css("display", "none");
        di_jq("#imgCacheTickGray").css("display", "none");
        di_jq("#imgCacheTick").css("display", "none");
        di_jq("#imgCacheProcessing").css("display", "block");

        InputParam = dbId;
        InputParam += ParamDelimiter + AdditionalParam;

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '92', 'param1': InputParam },
            async: true,
            timeout: 9000000,
            success: function (data) {
                di_jq("#imgCacheProcessing").css("display", "none");

                try {
                    if (data == "true") {

                        di_jq("#imgCacheTick").css("display", "block");

                        if (z("chkSDMXMLRegistration").checked) {
                            SDMXMLRegistration(dbId);
                        }
                        else if (z("chkMRRegistration").checked) {
                            MetadataReportRegistration(dbId);
                        }
                        else if (z("chkGenerateCSVFile").checked) {
                            GenerateCSVFile(dbId);
                        }
                        //                        else if (z("chkPublishSDMXData").checked) {
                        //                            PublishSDMXData(dbId);
                        //                        }
                        else if (z("chkPublishSDMXData").checked) {
                            PublishSDMXData(dbId, "DevInfo");
                        }
                        else if (di_jq("input[id$=chkPublishSDMXDataCountryData]").attr("checked")) {
                            PublishSDMXCountryData(dbId, "CountryData");
                        }
                        else {
                            alert("Generated successfully");
                            RemoveMaskingDiv();
                        }
                    }
                    else {
                        di_jq("#imgCacheError").css("display", "block");
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    di_jq("#imgCacheProcessing").css("display", "none");
                    di_jq("#imgCacheError").css("display", "block");
                    alert("Error : " + ex.message);
                    RemoveMaskingDiv();
                }
            },
            error: function (x, t, m) {
                di_jq("#imgCacheProcessing").css("display", "none");
                di_jq("#imgCacheError").css("display", "block");

                if (t === "timeout") {
                    alert("Timeout occured!");
                } else {

                }

                RemoveLoaderNMask();
            },
            cache: false
        });
    }
    catch (err) { }
}

function SDMXMLRegistration(dbId) {
    try {
        di_jq("#imgRegisterSDMXMLError").css("display", "none");
        di_jq("#imgRegisterSDMXMLTickGray").css("display", "none");
        di_jq("#imgRegisterSDMXMLTick").css("display", "none");
        di_jq("#imgRegisterSDMXMLProcessing").css("display", "block");

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '87', 'param1': dbId },
            async: true,
            success: function (data) {
                try {
                    di_jq("#imgRegisterSDMXMLProcessing").css("display", "none");

                    if (data == "true") {
                        di_jq("#imgRegisterSDMXMLTick").css("display", "block");

                        if (z("chkMRRegistration").checked) {
                            MetadataReportRegistration(dbId);
                        }
                        else if (z("chkGenerateCSVFile").checked) {
                            GenerateCSVFile(dbId);
                        }
                        //                        else if (z("chkPublishSDMXData").checked) {
                        //                            PublishSDMXData(dbId);
                        //                        }
                        else if (z("chkPublishSDMXData").checked) {
                            PublishSDMXData(dbId, "DevInfo");
                        }
                        else if (di_jq("input[id$=chkPublishSDMXDataCountryData]").attr("checked")) {
                            PublishSDMXCountryData(dbId, "CountryData");
                        }
                        else {
                            //di_jq("#btnOptDbNext").attr("disabled", false);
                            EnableButton("#btnOptDbNext");
                            alert("Generated successfully");
                            RemoveMaskingDiv();
                        }
                    }
                    else {
                        di_jq("#imgRegisterSDMXMLError").css("display", "block");
                        alert(data);
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    di_jq("#imgRegisterSDMXMLProcessing").css("display", "none");
                    di_jq("#imgRegisterSDMXMLError").css("display", "block");
                    alert("Error : " + ex.message);

                    RemoveMaskingDiv();
                }
            },
            error: function () {
                di_jq("#imgRegisterSDMXMLProcessing").css("display", "none");
                di_jq("#imgRegisterSDMXMLError").css("display", "block");


                RemoveMaskingDiv();
            },
            cache: false
        });

    }
    catch (err) { }
}

function MetadataReportRegistration(dbId) {
    try {
        di_jq("#imgRegisterMRError").css("display", "none");
        di_jq("#imgRegisterMRTickGray").css("display", "none");
        di_jq("#imgRegisterMRTick").css("display", "none");
        di_jq("#imgRegisterMRProcessing").css("display", "block");

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '104', 'param1': dbId },
            async: true,
            success: function (data) {
                try {
                    di_jq("#imgRegisterMRProcessing").css("display", "none");

                    if (data == "true") {
                        di_jq("#imgRegisterMRTick").css("display", "block");

                        if (z("chkGenerateCSVFile").checked) {
                            GenerateCSVFile(dbId);
                        }
                        //                        else if (z("chkPublishSDMXData").checked) {
                        //                            PublishSDMXData(dbId);
                        //                        }
                        else if (z("chkPublishSDMXData").checked) {
                            PublishSDMXData(dbId, "DevInfo");
                        }
                        else if (di_jq("input[id$=chkPublishSDMXDataCountryData]").attr("checked")) {
                            PublishSDMXCountryData(dbId, "CountryData");
                        }
                        else {
                            //di_jq("#btnOptDbNext").attr("disabled", false);
                            EnableButton("#btnOptDbNext");
                            alert("Generated successfully");
                            RemoveMaskingDiv();
                        }

                    }
                    else {
                        di_jq("#imgRegisterMRError").css("display", "block");
                        alert(data);
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    di_jq("#imgRegisterMRProcessing").css("display", "none");
                    di_jq("#imgRegisterMRError").css("display", "block");
                    alert("Error : " + ex.message);

                    RemoveMaskingDiv();
                }
            },
            error: function () {
                di_jq("#imgRegisterMRProcessing").css("display", "none");
                di_jq("#imgRegisterMRError").css("display", "block");


                RemoveMaskingDiv();
            },
            cache: false
        });

    }
    catch (err) { }
}

//function DBScriptGeneration(dbId) {
//    try {
//        di_jq("#imgDBError").css("display", "none");
//        di_jq("#imgDBTickGray").css("display", "none");
//        di_jq("#imgDBTick").css("display", "none");
//        di_jq("#imgDBProcessing").css("display", "block");

//        di_jq.ajax({
//            type: "POST",
//            url: CallbackPageName,
//            data: { 'callback': '75', 'param1': dbId },
//            async: true,
//            success: function (data) {
//                try {
//                    di_jq("#imgDBProcessing").css("display", "none");

//                    if (data == "true") {
//                        di_jq("#imgDBTick").css("display", "block");

//                        if (z("chkXMLGeneration").checked) {
//                            XMLGeneration(dbId);
//                        }
//                        else if (z("chkMapGeneration").checked) {
//                            MapFilesGeneration(dbId);
//                        }
//                        else if (z("chkSDMXGeneration").checked) {
//                            SDMXArtefactsGeneration(dbId);
//                        }
//                        else if (z("chkSDMXMLGeneration").checked) {
//                            SDMXMLGeneration(dbId);
//                        }
//                        else if (z("chkMetadataGeneration").checked) {
//                            MetadataGeneration(dbId);
//                        }
//                        else if (z("chkDBCacheResultsGeneration").checked) {
//                            if (z('txtCustomParams').value == "")
//                                GenerateCache(dbId, z('hcustomParams').value);
//                            else
//                                GenerateCache(dbId, z('txtCustomParams').value);
//                        }
//                        else if (z("chkSDMXMLRegistration").checked) {
//                            SDMXMLRegistration(dbId);
//                        }
//                        else if (z("chkMRRegistration").checked) {
//                            MetadataReportRegistration(dbId);
//                        }
//                        else if (z("chkGenerateCSVFile").checked) {
//                            GenerateCSVFile(dbId);
//                        }
//                        else if (z("chkGenerateSiteMap").checked) {
//                            GenerateSiteMap(dbId);
//                        }
//                        else {
//                            //di_jq("#btnOptDbNext").attr("disabled", false);
//                            EnableButton("#btnOptDbNext");
//                            alert("Generated successfully");
//                            RemoveMaskingDiv();
//                        }
//                    }
//                    else {
//                        di_jq("#imgDBError").css("display", "block");
//                        RemoveMaskingDiv();
//                    }
//                }
//                catch (ex) {
//                    di_jq("#imgDBProcessing").css("display", "none");
//                    di_jq("#imgDBError").css("display", "block");
//                    alert("Error : " + ex.message);

//                    RemoveMaskingDiv();
//                }
//            },
//            error: function () {
//                di_jq("#imgDBProcessing").css("display", "none");
//                di_jq("#imgDBError").css("display", "block");
//                

//                RemoveMaskingDiv();
//            },
//            cache: false
//        });

//    }
//    catch (err) { }
//}

function GenerateCSVFile(dbId) {
    try {
        di_jq("#imgGenerateCSVError").css("display", "none");
        di_jq("#imgGenerateCSVTickGray").css("display", "none");
        di_jq("#imgGenerateCSVTick").css("display", "none");
        di_jq("#imgGenerateCSVProcessing").css("display", "block");
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '295' },
            async: true,
            success: function (data) {
                try {
                    di_jq("#imgGenerateCSVProcessing").css("display", "none");

                    if (data == "true") {
                        di_jq("#imgGenerateCSVTick").css("display", "block");

                        //                        if (z("chkPublishSDMXData").checked) {
                        //                            PublishSDMXData(dbId);
                        //                        }
                        if (z("chkPublishSDMXData").checked) {
                            PublishSDMXData(dbId, "DevInfo");
                        }
                        else if (di_jq("input[id$=chkPublishSDMXDataCountryData]").attr("checked")) {
                            PublishSDMXCountryData(dbId, "CountryData");
                        }
                        else {
                            EnableButton("#btnOptDbNext");
                            alert("Generated successfully");
                            RemoveMaskingDiv();
                        }
                    }
                    else {
                        di_jq("#imgGenerateCSVError").css("display", "block");
                        alert('Error in Generating CSV files');
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    di_jq("#imgGenerateCSVProcessing").css("display", "none");
                    di_jq("#imgGenerateCSVError").css("display", "block");
                    alert("Error : " + ex.message);

                    RemoveMaskingDiv();
                }
            },
            error: function () {
                di_jq("#imgGenerateCSVProcessing").css("display", "none");
                di_jq("#imgGenerateCSVError").css("display", "block");


                RemoveMaskingDiv();
            },
            cache: false
        });
    }
    catch (err) { }
}

function GenerateSiteMap() {
    try {
        var dbId;
        dbId = getQueryStr("id");
        if (dbId != undefined) {
            ApplyMaskingDiv();
            di_jq("#imgGenerateSiteMapError").css("display", "none");
            di_jq("#imgGenerateSiteMapTickGray").css("display", "none");
            di_jq("#imgGenerateSiteMapTick").css("display", "none");
            di_jq("#imgGenerateSiteMapProcessing").css("display", "block");
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '1021', 'param1': dbId },
                async: true,
                success: function (data) {
                    try {
                        di_jq("#imgGenerateSiteMapProcessing").css("display", "none");

                        if (data == "true") {
                            di_jq("#imgGenerateSiteMapTick").css("display", "block");
                            EnableButton("#btnOptDbNext");
                            alert("Generated successfully");
                            RemoveMaskingDiv();
                        }
                        else {
                            di_jq("#imgGenerateSiteMapError").css("display", "block");
                            alert('Error in Generating Site Map');
                            RemoveMaskingDiv();
                        }
                    }
                    catch (ex) {
                        di_jq("#imgGenerateSiteMapProcessing").css("display", "none");
                        di_jq("#imgGenerateSiteMapError").css("display", "block");
                        alert("Error : " + ex.message);
                        RemoveMaskingDiv();
                    }
                },
                error: function () {
                    di_jq("#imgGenerateSiteMapProcessing").css("display", "none");
                    di_jq("#imgGenerateSiteMapError").css("display", "block");

                    RemoveMaskingDiv();
                },
                cache: false
            });
        }
    }
    catch (err) { }
}


function PublishSDMXData(DatabaseId, DBORCD) {
    try {
        var dbId;
        var DevInfoOrCountryData = DBORCD.toString();

        dbId = getQueryStr("id");

        if (dbId != undefined) {
            ApplyMaskingDiv();

            di_jq("#imgPublishSDMXDataError").css("display", "none");
            di_jq("#imgPublishSDMXDataTickGray").css("display", "none");
            di_jq("#imgPublishSDMXDataTick").css("display", "none");
            di_jq("#imgPublishSDMXDataProcessing").css("display", "block");

            // di_jq("#imgPublishSDMXDataProcessing").css("display", "block");

            //  di_jq("#imgPublishSDMXDataProcessingCD").css("display", "block");
            var InputParam = dbId;
            InputParam += "|" + DevInfoOrCountryData;
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '113', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        di_jq("#imgPublishSDMXDataProcessing").css("display", "none");

                        if (data == "true") {
                            di_jq("#imgPublishSDMXDataTick").css("display", "block");


                            if (di_jq("input[id$=chkPublishSDMXDataCountryData]").attr("checked")) {
                                PublishSDMXCountryData(dbId, "CountryData");
                            }
                            else {
                                EnableButton("#btnOptDbNext");
                                alert("Generated successfully");
                                RemoveMaskingDiv();
                            }
                        }
                        else {
                            di_jq("#imgPublishSDMXDataError").css("display", "block");
                            alert('Error in Publishing SDMX Data');
                            RemoveMaskingDiv();
                        }
                    }
                    catch (ex) {
                        di_jq("#imgPublishSDMXDataProcessing").css("display", "none");
                        di_jq("#imgPublishSDMXDataError").css("display", "block");
                        alert("Error : " + ex.message);

                        RemoveMaskingDiv();
                    }
                },
                error: function () {
                    di_jq("#imgPublishSDMXDataProcessing").css("display", "none");
                    di_jq("#imgPublishSDMXDataError").css("display", "block");
                    RemoveMaskingDiv();
                },
                cache: false
            });
        }
    }
    catch (err) { }

}

function PublishSDMXCountryData(DatabaseId, DBORCD) {
    try {
        var dbId;
        var DevInfoOrCountryData = DBORCD.toString();

        dbId = getQueryStr("id");

        if (dbId != undefined) {
            ApplyMaskingDiv();

            di_jq("#imgPublishSDMXDataProcessingCD").css("display", "block");
            di_jq("#imgPublishSDMXDataErrorCD").css("display", "none");
            di_jq("#imgPublishSDMXDataTickGrayCD").css("display", "none");
            di_jq("#imgPublishSDMXDataTickCD").css("display", "none");

            var InputParam = dbId;
            InputParam += "|" + DevInfoOrCountryData;
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '113', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        di_jq("#imgPublishSDMXDataProcessingCD").css("display", "none");

                        if (data == "true") {
                            di_jq("#imgPublishSDMXDataTickCD").css("display", "block");
                            EnableButton("#btnOptDbNext");
                            alert("SDMX Data Published successfully");
                            RemoveMaskingDiv();
                        }
                        else {
                            di_jq("#imgPublishSDMXDataErrorCD").css("display", "block");
                            alert('Error in Publishing SDMX Data');
                            RemoveMaskingDiv();
                        }
                    }
                    catch (ex) {

                        di_jq("#imgPublishSDMXDataProcessingCD").css("display", "none");
                        di_jq("#imgPublishSDMXDataErrorCD").css("display", "block");
                        alert("Error : " + ex.message);

                        RemoveMaskingDiv();
                    }
                },
                error: function () {
                    di_jq("#imgPublishSDMXDataProcessingCD").css("display", "none");
                    di_jq("#imgPublishSDMXDataErrorCD").css("display", "block");

                    RemoveMaskingDiv();
                },
                cache: false
            });
        }
    }
    catch (err) { }

}
function SelectAllXmlGeneration(objChk) {
    var ChechedSatus = false;

    try {
        ChechedSatus = objChk.checked;

        CheckedXmlChks(ChechedSatus);
    }
    catch (err) { }
}

function CheckedXmlChks(chkStatus) {
    try {
        z(ChkXmlArea).defaultChecked = chkStatus;
        z(ChkXmlFootnotes).defaultChecked = chkStatus;
        z(ChkXmlIC).defaultChecked = chkStatus;
        z(ChkXmlICIUS).defaultChecked = chkStatus;
        z(ChkXmlIUS).defaultChecked = chkStatus;
        z(ChkXmlMetadata).defaultChecked = chkStatus;
        z(ChkXmlAreaQuickSearch).defaultChecked = chkStatus;
        z(ChkXmlTimePeriods).defaultChecked = chkStatus;
    }
    catch (err) { }
}

function SelectAllChkStatus() {
    try {
        z(ChkXmlSelectAll).defaultChecked = false;
    }
    catch (err) { }
}

//-----------End Database Settings Optimize Database-----------------


//-----------Begin Database Settings Default Area-----------------

/* function to go on Area selection page */
function DoIndicatorSelection() {
    try {
        di_jq('#indicator_div_popup').html("");

        ApplyMaskingDiv(200);

        di_jq('#IndOuterBox').show('slow');

        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#IndOuterBox'), 'HideIndAreaPopup', CloseImgSrc);

        GetWindowCentered(z('IndOuterBox'), 700, 520);

        ShowIndicatorComponent(getAbsURL('stock'), 'indicator_div_popup', getQueryStr("id"), readCookie("hlngcodedb"), ReplaceAll(di_jq("#" + AddCPH("hdnSelIndO")).val(), "'", "\""), '700', '400', '10', '', false, false);
        di_loading('hide');
    }
    catch (err) { }
}

/* function to go on Area selection page */
function DoAreaSelection() {
    try {
        di_jq("#popup_sel_areas").hide();

        // empty area tab html first
        di_jq('#area_div_popup').html("");

        ApplyMaskingDiv(200);

        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#AreaOuterBox'), 'HideIndAreaPopup', CloseImgSrc);

        di_jq('#AreaOuterBox').show('slow');
        GetWindowCentered(z('AreaOuterBox'), 700, 520);

        var strCompInput = di_jq("#" + AddCPH("hdnSelAreaO")).val(); // ReplaceAll(, '\"', '"');        

        ShowAreaComponent(getAbsURL('stock'), 'area_div_popup', getQueryStr("id"), readCookie("hlngcodedb"), strCompInput, '700', '400', 'tree');

        di_loading('hide');
    }
    catch (err) { }
}

/* function to hide pop */
function HideIndAreaPopup() {
    try {
        RemoveMaskingDiv();
        di_jq('#AreaOuterBox').hide('slow');
        di_jq('#IndOuterBox').hide('slow');
        di_loading('hide');
    }
    catch (err) { }
}

function SaveDefaultArea() {
    var InputParam;
    var dbId = "";
    var DefaultAreas = "";

    try {
        RemoveMaskingDiv();
        ApplyMaskNLoader(1700);

        dbId = getQueryStr("id");

        if (dbId != undefined) {
            var AreaDataDelemeter = "||{~~}||";
            var areaData = AreaCompObj.getSelectedAreas().split(AreaDataDelemeter);

            if (areaData[0] == '' || areaData[0] == null) {
                alertMessage('Please select area');
                RemoveLoaderNMask();
                return false;
            }

            else {
                //removed it because this code is removing last char which is required
                //DefaultAreas = areaData[0].substr(0, areaData[0].length - 1);
                DefaultAreas = areaData[0];
            }

            InputParam = dbId;
            InputParam += ParamDelimiter + DefaultAreas;    //Selected Area
            InputParam += ParamDelimiter + areaData[1];     //Selected Area JSON

            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '40', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data == "true") {
                            di_jq("#" + AddCPH("lblAreaCounts")).html(DefaultAreas.split(",").length);
                            di_jq("#" + AddCPH("hdnSelAreaO")).val(areaData[1]);

                            ShowSelectedAreas(areaData[1]);

                            alert("Default area save successfully.");
                        }
                        else {
                            alert("Error while saving.");
                        }

                        RemoveLoaderNMask();
                        HideIndAreaPopup();
                    }
                    catch (ex) {
                        alert("Error : " + ex.message);
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

function SaveDefaultIndicator() {
    var InputParam;
    var dbId = "";

    try {
        RemoveMaskingDiv();
        ApplyMaskNLoader(1700);

        dbId = getQueryStr("id");

        if (dbId != undefined) {
            var IndDataDelemeter = "||{~~}||";
            var IndData = IndCompObj.getSelectedData().split(IndDataDelemeter);
            if (IndData[0] == '' || IndData[0] == null) {
                alertMessage('Please select indicator');
                RemoveLoaderNMask();
                return false;
            }

            InputParam = dbId;
            InputParam += ParamDelimiter + IndData[0];                           //Selected Indicator
            InputParam += ParamDelimiter + ReplaceAll(IndData[1], "\"", "'");    //Selected Indicaror JSON

            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '260', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data == "true") {
                            di_jq("#" + AddCPH("lblIndCounts")).html(IndData[0].split(",").length);
                            di_jq("#" + AddCPH("hdnSelIndO")).val(IndData[1]);

                            ShowSelectedIndicators(IndData[1]);

                            alert("Default indicator save successfully.");
                        }
                        else {
                            alert("Error while saving.");
                        }

                        RemoveLoaderNMask();
                        HideIndAreaPopup();
                    }
                    catch (ex) {
                        alert("Error : " + ex.message);
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

function ShowSelectedIndicators(indJSON) {
    var IndDataHtml = "<ul>";
    var IndRowArr;
    var Ind;

    try {
        IndRowArr = indJSON.split('iusnid');
        IndRowArr = IndRowArr[0].split("||");


        //        IndRowArr = di_jq.parseJSON(indJSON);
        //        

        //        var iuslist = IndRowArr.iu;
        //        for (var i = 0; i < iuslist.length; i++) {
        //            Ind = iuslist[i].split("||")[1];
        //            Ind = Ind.replace("~", ", ");
        //            IndDataHtml += "<li>" + Ind + "</li>";
        //        }

        for (var i = 1; i < IndRowArr.length; i++) {

            Ind = IndRowArr[i].split("',")[0];
            Ind = Ind.split("\",")[0];
            Ind = Ind.split("']")[0];
            Ind = Ind.split("\"]")[0];

            Ind = Ind.replace("~", ", ");

            IndDataHtml += "<li>" + Ind + "</li>";
        }

        IndDataHtml += "</ul>";

        di_jq("#divSelectedInd").html(IndDataHtml);
    }
    catch (err) { }
}

function ShowSelectedAreas(areaJSON) {

    var AreaDataHtml = "<ul>";
    var AreaRowArr;
    var Area;

    try {
        AreaRowArr = areaJSON.split("||");

        for (var i = 1; i < AreaRowArr.length; i++) {

            Area = AreaRowArr[i].split("\",")[0];
            Area = Area.split("\"]")[0];

            AreaDataHtml += "<li>" + Area + "</li>";
        }

        AreaDataHtml += "</ul>";

        di_jq("#divSelectedArea").html(AreaDataHtml);
    }
    catch (err) { }
}

//-----------End Database Settings Default Area-----------------


//-----------Begin Language---------------------------------------

function ShowNewLanguageDiv() {
    try {
        ApplyMaskingDiv();

        UpdateMaskZIndex(100);

        //Clear controls values
        di_jq("#txtLanguageName").val('');
        di_jq("#txtLanguageCode").val('');

        //Set close button at right corner of popup div
        var ImgSrc = "../../../stock/themes/default/images/close.png";
        SetCloseButtonInPopupDiv(di_jq('#NewLngDiv'), 'HideNewLanguageDiv', ImgSrc);

        di_jq('#NewLngDiv').show('slow');
        GetWindowCentered(z('NewLngDiv'), 400, 230);

        di_jq("#txtLanguageName").focus();
    }
    catch (err) { }
}

function HideNewLanguageDiv() {
    try {
        RemoveMaskingDiv();
        di_jq('#NewLngDiv').hide('slow');
    }
    catch (err) { }
}

function ValidateLanguage() {
    var RetVal = true;
    var NameNCode = "";
    var LngVal = "";
    var LngName = "";
    var LngCode = "";

    try {
        if (di_jq("#txtLanguageName").val() == '') {
            alert("Enter language name");
            di_jq("#txtLanguageName").focus();
            RetVal = false;
        }
        else if (di_jq("#txtLanguageCode").val() == '') {
            alert("Enter language code");
            di_jq("#txtLanguageCode").focus();
            RetVal = false;
        }
        else {
            NameNCode = di_jq("#txtLanguageName").val() + " [" + di_jq("#txtLanguageCode").val() + "]";
            NameNCode = NameNCode.toLowerCase();

            //Check language name and code already exists
            for (var i = 1; i < di_jq("#" + AddCPH("ddlTrgLng option")).length; i++) {
                LngVal = di_jq("#" + AddCPH("ddlTrgLng option"))[i].value.toLowerCase();

                LngName = LngVal.split("[")[0].trim();
                LngCode = LngVal.substr(LngVal.indexOf("[") + 1, 2)

                if (LngName == di_jq("#txtLanguageName").val()) {
                    alert("Language name already exists");
                    RetVal = false;
                    break;
                }
                else if (LngCode == di_jq("#txtLanguageCode").val()) {
                    alert("Language code already exists");
                    RetVal = false;
                    break;
                }
                else if (LngVal == NameNCode) {
                    alert("Language name and code already exists");
                    RetVal = false;
                    break;
                }
            }
        }
    }
    catch (err) { }

    return RetVal;
}

function AddNewLanguage() {
    var InputParam;
    var LanguageName;
    var LanguageCode;
    var LngNCodeName;

    try {
        if (ValidateLanguage()) {
            UpdateMaskZIndex(200);
            ShowLoadingDiv();

            LanguageName = di_jq("#txtLanguageName").val();
            LanguageCode = di_jq("#txtLanguageCode").val();

            InputParam = LanguageName;
            InputParam += ParamDelimiter + LanguageCode;
            InputParam += ParamDelimiter + di_jq("#" + AddCPH("ddlPageDir")).val();

            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '46', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data == "true") {
                            GetAllLanguageList();

                            alert("Saved successfully.");
                        }
                        else if (data == "exists") {
                            LngNCodeName = LanguageName + " [" + LanguageCode + "]";
                            alert(LngNCodeName + " language already exits.");
                        }
                        else {
                            alert("Error while saving.");
                        }

                        HideNewLanguageDiv();
                        RemoveLoaderNMask();
                    }
                    catch (ex) {
                        alert("Error : " + ex.message);
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

function ChangeLanguage() {
    var SrcLngVal = "";
    var TrgLngVal = "";

    try {
        di_jq(DivLanguageGrid).html("");

        SrcLngVal = di_jq("#" + AddCPH("ddlSrcLng")).val();
        TrgLngVal = di_jq("#" + AddCPH("ddlTrgLng")).val();

        if (SrcLngVal != "-1" && TrgLngVal != "-1") {
            GetLanguageGridHTML(SrcLngVal, TrgLngVal);
        }
        else {
            DisableButton(BtnLngSave);
            DisableButton(BtnLngCancel);
            DisableButton(btnLangexport);
        }
    }
    catch (err) { }
}

function GetLanguageGridHTML(srcLngVal, trgLngVal) {
    var InputParam;
    try {

        ApplyMaskNLoader();

        InputParam = srcLngVal;
        InputParam += ParamDelimiter + trgLngVal;

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '278', 'param1': InputParam },
            async: true,
            success: function (htmlData) {
                try {
                    if (htmlData != "") {
                        di_jq(DivLanguageGrid).html(htmlData);
                        EnableButton(BtnLngSave);
                        EnableButton(BtnLngCancel);
                        EnableButton(btnLangexport)
                    }
                    else {
                        htmlData1 = "";
                        DisableButton(BtnLngSave);
                        DisableButton(BtnLngCancel);
                        DisableButton(btnLangexport);
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
    catch (err) { }
}

function CLickExportLanguage() {
    var SrcLngVal = "";
    var TrgLngVal = "";

    try {
        SrcLngVal = di_jq("#" + AddCPH("ddlSrcLng")).val();
        TrgLngVal = di_jq("#" + AddCPH("ddlTrgLng")).val();

        if (SrcLngVal != "-1" && TrgLngVal != "-1") {
            if (SrcLngVal == TrgLngVal) {
                alert("Please select different languages");
            }
            else {
                ExportLanguagestoXLS(SrcLngVal, TrgLngVal);
            }
        }
    }
    catch (err) { }
}
function ExportLanguagestoXLS(srcLngVal, trgLngVal) {
    var InputParam;
    try {
        ApplyMaskNLoader();
        InputParam = srcLngVal;
        InputParam += ParamDelimiter + trgLngVal;

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '1015', 'param1': InputParam },
            async: false,
            success: function (Data) {
                try {
                    if (Data.split(ParamDelimiter)[0] == "true") {
                        if (Data.split(ParamDelimiter)[1] != null && Data.split(ParamDelimiter)[1] != undefined) {
                            var LanguageXlsFilePath = getAbsURL('stock') + "LanguageXML/" + Data.split(ParamDelimiter)[1] + ".xls"
                            window.location.replace(LanguageXlsFilePath);
                        }
                    }
                    else {
                        alert("Error in Export Language XlS.");
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
    catch (err) { }
}


function ShowTextboxForEnterTargetValue(tdThis, evt) {
    try {
        if (SelectedTxtId != ("txt" + tdThis.id)) {
            RemoveGridClickedRowFormatting();

            SetGridClickedRowFormatting(tdThis)

            tdThis.style.paddingLeft = "0px";

            var tdVal = tdThis.innerHTML;
            SelectedTxtId = "txt" + tdThis.id;

            tdThis.innerHTML = "<Textarea type='text' id='" + SelectedTxtId + "' Class='normalText' style='height:" + tdThis.offsetHeight + "px' onblur='OnBlurTargetTxtChangeValue();' onKeyPress='DisableEnterKeyPress(event);' />";

            z(SelectedTxtId).value = tdVal;

            if (IsIE7() || IsIE6()) {
                var txtWidth = tdThis.offsetWidth - 13;
                z(SelectedTxtId).style.width = txtWidth + "px";
            }
            else {
                //$$(SelectedTxtId).style.width = tdThis.offsetWidth + "px";
                var txtWidth = tdThis.offsetWidth - 8;
                z(SelectedTxtId).style.width = txtWidth + "px";
            }

            z(SelectedTxtId).focus();

            SelectedTd = tdThis;
        }
    }
    catch (err) { }

    return false;
}

function OnBlurTargetTxtChangeValue() {
    try {
        SelectedTd.style.paddingLeft = "5px";
        SelectedTd.innerHTML = z(SelectedTxtId).value;
        RemoveGridClickedRowFormatting();

        SelectedTxtId = "";

    }
    catch (err) { }
}

function SetGridClickedRowFormatting(tdThis) {
    try {
        tdThis.style.fontWeight = 'bold';
        tdThis.parentNode.style.border = "2px Black Solid";
        tdThis.parentNode.getElementsByTagName("TD")[0].style.backgroundColor = 'Silver';
        //tdThis.parentNode.getElementsByTagName("TD")[1].style.backgroundColor = 'Silver';
    }
    catch (err) { }
}

function RemoveGridClickedRowFormatting() {
    try {
        if (SelectedTd) {
            SelectedTd.style.fontWeight = '';
            SelectedTd.parentNode.style.border = "1px Silver Solid";
            SelectedTd.parentNode.getElementsByTagName("TD")[0].style.backgroundColor = '';
            SelectedTd.parentNode.getElementsByTagName("TD")[0].vAlign = "middle";
            //SelectedTd.parentNode.getElementsByTagName("TD")[1].style.backgroundColor = '';
        }
    }
    catch (err) { }
}


function GetLanguageKeyNValue(tblName) {
    var RetVal = '';
    var Tbl = z(tblName);
    var Row;

    try {
        if (Tbl) {
            for (i = 1; i < Tbl.rows.length; i++) {
                Row = Tbl.rows[i];


                if (RetVal != '') {
                    RetVal += RowDelimiter;
                }

                RetVal += z(LanguageTableName).rows[i].cells[1].id + ColumnDelimiter + z(LanguageTableName).rows[i].cells[1].innerHTML;

            }
        }
    }
    catch (err) { }

    return RetVal;
}


function SaveLanguageChanges() {
    var InputParam;
    var KayNValueStr;

    try {
        ApplyMaskNLoader();

        KayNValueStr = GetLanguageKeyNValue(LanguageTableName);

        InputParam = KayNValueStr;
        InputParam += ParamDelimiter + di_jq("#" + AddCPH("ddlTrgLng")).val();
        InputParam += ParamDelimiter + di_jq("#" + AddCPH("ddlSrcLng")).val();

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '279', 'param1': InputParam },
            async: true,
            success: function (htmlData) {
                try {
                    if (htmlData != "") {
                        alert("Language saved successfully.");
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
    catch (err) { }

    return false;
}

function AddNewLanguageDefaultButton(event) {
    try {
        // Set default button on press of enter key.
        if (event.keyCode == 13) {
            if (di_jq("#NewLngDiv").css("display") != "none") {
                di_jq("#btnSave").click();
            }

            if (window.ActiveXObject) {
                event.returnValue = false;
                event.cancel = true;
            }
            else {
                event.preventDefault();
            }
        }
    }
    catch (err) { }
}

function ShowOptimizeStatus(isXmlGenerated, isXmlAreaGenerated, isXmlFootnotesGenerated, isXmlICGenerated, isXmlICIUSGenerated, isXmlIUSGenerated, isXmlMetadataGenerated, IsXmlQuickSearchGenerated, isXmlTimePeriodsGenerated, isMapGenerated, isSDMXGenerated, isSDMXMLGenerated, isMetadataGenerated, isCacheResultGenerated, isSDMXMLRegistered, IsCsvFileGenerated, IsSDMXDataPublished, IsSDMXDataPublishedCountryData) {
    try {
        if (isXmlGenerated == "true") {
            di_jq("#imgXMLTick").css("display", "block");

            if (isXmlAreaGenerated == "true") {
                z(ChkXmlArea).defaultChecked = true;
            }

            if (isXmlFootnotesGenerated == "true") {
                z(ChkXmlFootnotes).defaultChecked = true;
            }

            if (isXmlICGenerated == "true") {
                z(ChkXmlIC).defaultChecked = true;
            }

            if (isXmlICIUSGenerated == "true") {
                z(ChkXmlICIUS).defaultChecked = true;
            }

            if (isXmlIUSGenerated == "true") {
                z(ChkXmlIUS).defaultChecked = true;
            }

            if (isXmlMetadataGenerated == "true") {
                z(ChkXmlMetadata).defaultChecked = true;
            }

            if (IsXmlQuickSearchGenerated == "true") {
                z(ChkXmlAreaQuickSearch).defaultChecked = true;
            }

            if (isXmlTimePeriodsGenerated == "true") {
                z(ChkXmlTimePeriods).defaultChecked = true;
            }
        }
        else {
            di_jq("#imgXMLTickGray").css("display", "block");

            z(ChkXmlSelectAll).defaultChecked = true;
            CheckedXmlChks(true);
        }
        if (isMapGenerated == "true") {
            di_jq("#imgMapTick").css("display", "block");
        }
        else {
            di_jq("#imgMapTickGray").css("display", "block");
        }

        if (isSDMXGenerated == "true") {
            di_jq("#imgSDMXTick").css("display", "block");
        }
        else {
            di_jq("#imgSDMXTickGray").css("display", "block");
        }

        if (isMetadataGenerated == "true") {
            di_jq("#imgMetadataTick").css("display", "block");
        }
        else {
            di_jq("#imgMetadataTickGray").css("display", "block");
        }

        if (isCacheResultGenerated == "true") {
            di_jq("#imgCacheTick").css("display", "block");
        }
        else {
            di_jq("#imgCacheTickGray").css("display", "block");
        }

        if (isSDMXMLGenerated == "true") {
            di_jq("#imgSDMXMLTick").css("display", "block");
        }
        else {
            di_jq("#imgSDMXMLTickGray").css("display", "block");
        }

        if (isSDMXMLRegistered == "true") {
            di_jq("#imgRegisterSDMXMLTick").css("display", "block");
        }
        else {
            di_jq("#imgRegisterSDMXMLTickGray").css("display", "block");
        }
        if (IsCsvFileGenerated == "true") {
            di_jq("#imgGenerateCSVTick").css("display", "block");
        }
        else {
            di_jq("#imgGenerateCSVTickGray").css("display", "block");
        }
        if (IsSDMXDataPublished == "true") {
            di_jq("#imgPublishSDMXDataTick").css("display", "block");
        }
        else {
            di_jq("#imgPublishSDMXDataTickGray").css("display", "block");
        }
        if (IsSDMXDataPublishedCountryData == "true") {
            di_jq("#imgPublishSDMXDataTickCD").css("display", "block");
        }
        else {
            di_jq("#imgPublishSDMXDataTickGrayCD").css("display", "block");
        }
    }
    catch (err) { }
}

function ShowSiteMapOptimizeStatus(IsSiteMapGenerated) {
    if (IsSiteMapGenerated == "true") {
        di_jq("#imgGenerateSiteMapTick").css("display", "block");
    }
    else {
        di_jq("#imgGenerateSiteMapTickGray").css("display", "block");
    }
}

function GetAllLanguageList() {
    try {
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '50' },
            async: false,
            success: function (data) {
                try {
                    di_jq("#" + AddCPH("divLngList")).html(data);
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
    catch (err) { }
}

function DeleteLanguage() {
    var InputParam;

    try {
        if (di_jq("#" + AddCPH("divLngList") + " :checked").length > 0) {
            if (ConfirmToDelete('Delete this Language?')) {
                ApplyMaskNLoader();

                di_jq("#" + AddCPH("divLngList") + " :checked").each(function () {
                    InputParam = di_jq(this).context.id.substr(4);
                    InputParam += ParamDelimiter + di_jq(this).parent().parent()[0].getElementsByTagName("td")[1].innerHTML;
                });

                di_jq.ajax({
                    type: "POST",
                    url: CallbackPageName,
                    data: { 'callback': '51', 'param1': InputParam },
                    async: true,
                    success: function (data) {
                        try {
                            if (data == "true") {
                                GetAllLanguageList();
                                alert("Delete Successful !");
                            }
                            else {
                                alert("Can`t Delete Connection !");
                            }

                            RemoveLoaderNMask();
                        }
                        catch (ex) {
                            alert("Error : " + ex.message);
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
        else {
            alert("Select a language.");
        }
    }
    catch (err) { }
}

function EditLanguage() {
    var LngName = "";

    try {
        di_jq("#" + AddCPH("divLngList") + " :checked").each(function () {
            LngName = di_jq(this).parent().parent()[0].getElementsByTagName("td")[1].innerHTML;
        });

        if (LngName != "") {
            window.location = "Language.aspx?n=" + di_jq.trim(LngName);
        }
        else {
            alert("Select a language.");
        }
    }
    catch (err) { }
}

function GenerateAllPagesXML() {
    try {
        ApplyMaskNLoader();

        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '52' },
            async: true,
            success: function (data) {
                try {
                    if (data == "true") {
                        GetAllLanguageList();
                        alert("Page XMLs generated successfully!");
                    }
                    else {
                        alert("Error While Generating Page XMLs!");
                    }

                    RemoveLoaderNMask();
                }
                catch (ex) {
                    alert("Error : " + ex.message);
                    RemoveLoaderNMask();
                }
            },
            error: function () {

                RemoveLoaderNMask();
            },
            cache: false
        });
    }
    catch (err) { }
}

//-----------End Language---------------------------------------


//-----------Begin Login---------------------------------------

function ValidateCredential() {
    var InputParam; alert(InputParam);
    //var LoginErrorMsgArr = [z("lngAccountNotActivatedMsg").innerHTML, z("lngSuccessfullyLoginMsg").innerHTML, z("lngRegenerateLinkMsg").innerHTML, z("lngInvalidCredentialsMsg").innerHTML, z("lngInternalErrorMsg").innerHTML];
    try {
        if ((z('txtEmail').value != null) && (z('txtEmail').value != "") && (z('txtHavePassword').value != null) && (z('txtHavePassword').value != "")) {
            InputParam = z('txtEmail').value;
            InputParam += ParamDelimiter + z('txtHavePassword').value;

            try {
                
                di_jq.ajax({
                    type: "POST",
                    url: CallbackPageName,
                    data: { 'callback': '262', 'param1': InputParam },
                    async: true,
                    success: function (data) {
                        try {
                            if (data.split(ParamDelimiter)[0] == "true") {
                                window.location = "AppSettings.aspx";
                            }
                            else {
                                //alert("LoginErrorMsgArr[data.split(ParamDelimiter)[1]]");
                                alert("Invalid Credentials");
                                z('txtEmail').focus();
                                z('txtHavePassword').value = "";
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
            catch (ex) {
            }
        }
        else {
            alert("Please enter email id and password!");
        }
    }
    catch (err) { }
}

function ClickValidateCredential(event) {
    try {
        // Set default button on press of enter key.
        if (event.keyCode == 13) {
            z("btnLogin").click();

            if (window.ActiveXObject) {
                event.returnValue = false;
                event.cancel = true;
            }
            else {
                event.preventDefault();
            }
        }
    }
    catch (err) { }
}

//-----------End Login---------------------------------------


//-----------Begin User---------------------------------------

function FillUserDdlCountries() {
    try {
        ApplyMaskNLoader();

        if (di_jq(DdlCountry).find("option").length == 0) {
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '270' },
                async: true,
                success: function (jsonData) {
                    try {
                        if (jsonData != "") {
                            FillCountryDdl(jsonData, "ddlSelectCountryUser", -1);
                        }
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
    catch (err) {
        RemoveLoaderNMask();
    }

    RemoveLoaderNMask();
}

function ShowNewUserDialog() {
    try {
        ApplyMaskingDiv(200);

        di_jq('#divUserOuterBox').show('slow');

        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#divUserOuterBox'), 'HideUserPopup', CloseImgSrc);

        GetWindowCentered(z('divUserOuterBox'), 700, 520);
    }
    catch (err) { }
}

function HideUserPopup() {
    try {
        di_jq('#divUserOuterBox').hide('slow');
        RemoveMaskingDiv();
    }
    catch (err) { }
}

function NewUserDetails() {
    try {
        ClearUserCtrlsValues();
        ShowNewUserDialog();

        di_jq("#hdnNewUser").show();
        di_jq("#hdnEditUser").hide();

        di_jq("#btnNewUser").show();
        di_jq("#btnEditUser").hide();

        di_jq(DdlCountry).selectedIndex = 0;

    }
    catch (err) { }
}

function EditUserDetails() {
    var SelectedUserRow;

    try {

        if (di_jq("#" + AddCPH("divUsersList") + " :checked").length > 0) {
            di_jq("#" + AddCPH("divUsersList") + " :checked").each(function () {
                SelectedUserNId = di_jq(this).context.id.replace("usr_", "");
                SelectedUserRow = di_jq(this)[0].parentNode.parentNode;
            });

            SetSelectedUserValuesInCtrls(SelectedUserRow);

            ShowNewUserDialog();

            di_jq("#hdnNewUser").hide();
            di_jq("#hdnEditUser").show();

            di_jq("#btnNewUser").hide();
            di_jq("#btnEditUser").show();
        }
        else {
            alert("Select a user.");
        }

    }
    catch (err) { }
}

function ClearUserCtrlsValues() {
    try {
        di_jq(TxtEmail).val('');
        di_jq(TxtFirstName).val('');
        di_jq(DdlCountry).val('0');
        //di_jq(DdlCountry).selectedIndex = 0;
        di_jq(TxtPassword).val('');
        di_jq(TxtConfirmPassword).val('');
        di_jq(ChkSendDevInfoUpdates).attr("checked", false);
    }
    catch (err) { }
}

function SetSelectedUserValuesInCtrls(row) {
    try {
        ClearUserCtrlsValues();

        // Set values into contorls
        di_jq(TxtEmail).val(GetTblCellValue(row, 1));
        di_jq(TxtFirstName).val(GetTblCellValue(row, 2));
        di_jq(DdlCountry + " option:contains(" + di_jq.trim(ConvertHtmlCodeToText(GetTblCellValue(row, 3))) + ")").attr('selected', 'selected');

        di_jq(TxtPassword).val('');
        di_jq(TxtConfirmPassword).val('');
        di_jq(ChkSendDevInfoUpdates).attr("checked", GetTblCellValue(row, 4).toLowerCase() == "true" ? true : false);
    }
    catch (err) { }
}

function ClickInsertOrUpdateUser(event) {
    try {
        // Set default button on press of enter key.
        if (event.keyCode == 13) {

            if (z("btnNewUser").style.display == "" || z("btnNewUser").style.display == "inline-block" || z("btnNewUser").style.display == "block") {
                z("btnNewUser").click();
            }
            else {
                z("btnEditUser").click();
            }

            if (window.ActiveXObject) {
                event.returnValue = false;
                event.cancel = true;
            }
            else {
                event.preventDefault();
            }
        }
    }
    catch (err) { }
}


function ValidateUserInputs() {
    var RetVal = true;

    try {
        if ((di_jq(TxtEmail).val() == null) || (di_jq(TxtEmail).val() == "")) {
            RetVal = false;
            alert("Email is a mandatory field!");
        }
        else if (validateEmail(di_jq(TxtEmail).val()) == false) {
            RetVal = false;
            alert("Email id format is not correct!");
        }
        else if ((di_jq(TxtFirstName).val() == null) || (di_jq(TxtFirstName).val() == "")) {
            RetVal = false;
            alert("First name is a mandatory field!");
        }
        else if ((di_jq(TxtPassword).val() == null) || (di_jq(TxtPassword).val() == "")) {
            RetVal = false;
            alert("Please enter the password!");
        }
        else if ((di_jq(TxtConfirmPassword).val() == null) || (di_jq(TxtConfirmPassword).val() == "")) {
            RetVal = false;
            alert("Please confirm your password!");
        }
        else if (di_jq(TxtPassword).val() != di_jq(TxtConfirmPassword).val()) {
            di_jq(TxtPassword).val('');
            di_jq(TxtConfirmPassword).val('');
            RetVal = false;
            alert("Password confirmation fails. Please re-enter password and confirm.");

        }
    }
    catch (err) { }

    return RetVal;
}


function CreateNewUser() {
    var InputParam;

    try {

        if (ValidateUserInputs()) {

            RemoveMaskingDiv();
            ApplyMaskNLoader(1700);

            InputParam = di_jq(TxtEmail).val();
            InputParam += ParamDelimiter + di_jq(TxtFirstName).val();
            InputParam += ParamDelimiter + di_jq(DdlCountry).val();
            InputParam += ParamDelimiter + di_jq(TxtPassword).val();

            InputParam += ParamDelimiter + "false";    // Provider
            InputParam += ParamDelimiter + "false"; //Admin

            if (di_jq(ChkSendDevInfoUpdates).attr("checked") == true) {
                InputParam += ParamDelimiter + "true";
            }
            else {
                InputParam += ParamDelimiter + "false";
            }

            InputParam += ParamDelimiter + readCookie("hlngcode");
            InputParam += ParamDelimiter + "false";

            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '48', 'param1': InputParam },
                async: true,
                success: function (data) {
                    if (data.split(ParamDelimiter)[0] == "true") {
                        alert("Sign up successful!");
                        GetAllUsersGrid();
                        //HideUserPopup();
                    }
                    else {
                        alert(data.split(ParamDelimiter)[1]);
                        HideUserPopup();
                        RemoveLoaderNMask();
                    }
                },
                error: function () {

                    HideUserPopup();
                    RemoveLoaderNMask();
                },
                cache: false
            });
        }
    }
    catch (err) {
        HideUserPopup();
        RemoveLoaderNMask();
    }
}

function UpdateUser() {
    var InputParam;

    try {

        if (ValidateUserInputs()) {

            RemoveMaskingDiv();
            ApplyMaskNLoader(1700);

            InputParam = SelectedUserNId + "|false";
            InputParam += ParamDelimiter + di_jq(TxtEmail).val();
            InputParam += ParamDelimiter + di_jq(TxtFirstName).val();
            InputParam += ParamDelimiter + di_jq(DdlCountry).val();
            InputParam += ParamDelimiter + di_jq(TxtPassword).val();

            InputParam += ParamDelimiter + "false";     //Register provider       

            if (di_jq(ChkSendDevInfoUpdates).attr("checked") == true) {
                InputParam += ParamDelimiter + "true";
            }
            else {
                InputParam += ParamDelimiter + "false";
            }

            InputParam += ParamDelimiter + readCookie("hlngcode");

            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '68', 'param1': InputParam },
                async: true,
                success: function (data) {
                    if (data.split(ParamDelimiter) == "true") {
                        alert("Details Updated!");
                        GetAllUsersGrid();
                        //HideUserPopup();
                    }
                    else {
                        alert(data.split(ParamDelimiter)[1]);
                        HideUserPopup();
                        RemoveLoaderNMask();
                    }
                },
                error: function () {

                    HideUserPopup();
                    RemoveLoaderNMask();
                },
                cache: false
            });
        }
    }
    catch (err) {
        HideUserPopup();
        RemoveLoaderNMask();
    }
}

function GetAllUsersGrid() {
    try {
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '263' },
            async: true,
            success: function (data) {
                if (data != "") {
                    di_jq("#" + AddCPH("divUsersList")).html(data);
                    HideUserPopup();
                    RemoveLoaderNMask();
                }
                else {
                }
            },
            error: function () {

            },
            cache: false
        });
    }
    catch (err) { }
}

function ShowForgotPasswordDialog() {
    try {

        ApplyMaskingDiv(200);

        di_jq('#divForgotPwdOuterBox').show('slow');

        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#divForgotPwdOuterBox'), 'HideForgotPwdPopup', CloseImgSrc);

        GetWindowCentered(z('divForgotPwdOuterBox'), 350, 150);
    }
    catch (err) { }
}

function HideForgotPwdPopup() {
    try {
        di_jq('#divForgotPwdOuterBox').hide('slow');
        RemoveMaskingDiv();
    }
    catch (err) { }
}

function SendForgotPassword() {
    var InputParam;

    try {

        if ((di_jq(TxtFPEmail).val() != null) && (di_jq(TxtFPEmail).val() != "")) {

            RemoveMaskingDiv();
            ApplyMaskNLoader(1700);

            InputParam = di_jq(TxtFPEmail).val();
            InputParam += ParamDelimiter + readCookie("hlngcode");
            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '70', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        alert(data.split(ParamDelimiter)[1]);
                        HideForgotPwdPopup();
                        RemoveLoaderNMask();
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
            alert("Please enter Email Id!");
        }
    }
    catch (ex) {
    }
}

function ClickForgotPwd(event) {
    try {
        // Set default button on press of enter key.
        if (event.keyCode == 13) {

            z("btnFPOk").click();

            if (window.ActiveXObject) {
                event.returnValue = false;
                event.cancel = true;
            }
            else {
                event.preventDefault();
            }
        }
    }
    catch (err) { }
}

function DeleteUser() {
    var InputParam;
    var UserNId;
    var IsSiteAdminUser = false;
    var Row;

    try {

        if (di_jq("#" + AddCPH("divUsersList") + " :checked").length > 0) {

            if (confirm("Confirm to delete ?")) {

                ApplyMaskNLoader();

                di_jq("#" + AddCPH("divUsersList") + " :checked").each(function () {
                    UserNId = di_jq(this).context.id.replace("usr_", "");
                    Row = di_jq(this)[0].parentNode.parentNode;
                    IsSiteAdminUser = GetTblCellValue(Row, 6).toLowerCase() != "true" ? false : true;
                });

                if (IsSiteAdminUser) {
                    alert("Site admin user can not be deleted.");
                    RemoveLoaderNMask();
                }
                else {
                    InputParam = UserNId;

                    di_jq.ajax({
                        type: "POST",
                        url: CallbackPageName,
                        data: { 'callback': '264', 'param1': InputParam },
                        async: true,
                        success: function (data) {
                            try {
                                if (data == "true") {
                                    GetAllUsersGrid();
                                    alert("User deleted sucessfully.");
                                }
                                else {

                                }
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
        }
        else {
            alert("Select a user.");
        }
    }
    catch (err) { }
}

//-----------End User---------------------------------------


//-----------Begin Catalog---------------------------------------

var SelectedAdaptedFor;

function ShowDivAddAdptDetails(isEditable) {
    try {
        di_jq("#divCatalog").css("display", "none");
        di_jq("#divAddAdptDetails").css("display", "block");

        di_jq("#lang_Edit_Adaptation_Details").css("display", "none");
        di_jq("#lang_Add_Adaptation_Details").css("display", "none");

        di_jq("#lang_Save_Adaptation").css("display", "none");
        di_jq("#lang_Update_Adaptation").css("display", "none");


        ClearAdaptationCtrlValues();

        if (isEditable) {
            di_jq("#lang_Edit_Adaptation_Details").css("display", "block");
            di_jq("#lang_Update_Adaptation").css("display", "");
        }
        else {
            di_jq("#lang_Add_Adaptation_Details").css("display", "block");
            di_jq("#lang_Save_Adaptation").css("display", "");
            SetAdaptedFor("global");
        }
    }
    catch (err) { }
}

function EnableWebURL(chkStatus) {
    try {
        di_jq("#txtWebURL").attr("disabled", !(chkStatus));
    }
    catch (err) { }
}

function ClearAdaptationCtrlValues() {
    try {

        di_jq("#txtAdaptationName").val("");
        di_jq("#txtDescription").val("");

        di_jq("#chkDesktop").attr("checked", false);
        di_jq("#chkWeb").attr("checked", false);
        di_jq("#txtWebURL").attr("disabled", true);
        di_jq("#txtWebURL").val("");

        di_jq("#txtAreaCount").val("");
        di_jq("#txtIUSCount").val("");
        di_jq("#txtTimePeriodsCount").val("");
        di_jq("#txtDataValuesCount").val("");
        di_jq("#txtStartYear").val("");
        di_jq("#txtEndYear").val("");
        di_jq("#txtLastModifiedOn").val("");

        di_jq("#rbGlobal").attr("checked", true);

        //di_jq("#flCatalogImage").val("");
        //di_jq("#flCatalogImage")[0].value = "";

        di_jq("#imgPreview").attr("src", getAbsURL('stock') + "themes/default/images/spacer.gif");
        CurrentAdaptationLogoUrl = "";
    }
    catch (err) { }
}


function ValidateAdaptation() {
    var RetVal = true;

    try {
        if (di_jq("#txtAdaptationName").val() == '') {
            alert("Enter adaptation name");
            di_jq("#txtAdaptationName").focus();
            RetVal = false;
        }
        else if (di_jq("#txtAdaptationYear").val() == '') {
            alert("Enter adaptation year");
            di_jq("#txtAdaptationYear").focus();
            RetVal = false;
        }
        else if (di_jq("#txtDatabaseLanguages").val() == '') {
            alert("Enter database languages");
            di_jq("#txtDatabaseLanguages").focus();
            RetVal = false;
        }
        else if (di_jq("#chkDesktop").attr("checked") == false && di_jq("#chkWeb").attr("checked") == false) {
            alert("Select available on plateform");
            di_jq("#chkDesktop").focus();
            RetVal = false;
        }
        else if (di_jq("#chkWeb").attr("checked") && di_jq("#txtWebURL").val() == '') {
            alert("Enter web URL");
            di_jq("#txtWebURL").focus();
            RetVal = false;
        }
        else if (di_jq("#txtAreaCount").val() == '') {
            alert("Enter area count");
            di_jq("#txtAreaCount").focus();
            RetVal = false;
        }
        else if (di_jq("#txtIUSCount").val() == '') {
            alert("Enter IUS count");
            di_jq("#txtIUSCount").focus();
            RetVal = false;
        }
        else if (di_jq("#txtTimePeriodsCount").val() == '') {
            alert("Enter time periods count");
            di_jq("#txtTimePeriodsCount").focus();
            RetVal = false;
        }
        else if (di_jq("#txtDataValuesCount").val() == '') {
            alert("Enter data values count");
            di_jq("#txtDataValuesCount").focus();
            RetVal = false;
        }
        else if (di_jq("#txtStartYear").val() == '') {
            alert("Enter start year");
            di_jq("#txtStartYear").focus();
            RetVal = false;
        }
        else if (di_jq("#txtEndYear").val() == '') {
            alert("Enter end year");
            di_jq("#txtEndYear").focus();
            RetVal = false;
        }
        else if (di_jq("#txtLastModifiedOn").val() == '') {
            alert("Enter last modified on");
            di_jq("#txtLastModifiedOn").focus();
            RetVal = false;
        }
        else if (di_jq("#txtDbAdmEmail").val() != '') {
            if (!validateEmail(di_jq("#txtDbAdmEmail").val())) {
                alert("Invalid email");
                di_jq("#txtDbAdmEmail").focus();
                RetVal = false;
            }
        }
        else if (di_jq("#rbSubNational").attr("checked") && di_jq("#txtSubNation").val() == '') {
            alert("Enter sub nation");
            di_jq("#txtSubNation").focus();
            RetVal = false;
        }
        else if (CurrentAdaptationLogoUrl == '') {
            alert("Select and upload a catalog image");
            di_jq("#flCatalogImage").focus();
            RetVal = false;
        }


    }
    catch (err) { }

    return RetVal;
}


function SaveAdaptation() {
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
            //InputParam += ParamDelimiter + di_jq("#flCatalogImage").val();            
            //InputParam += ParamDelimiter + getAbsURL('stock') + "themes/default/images/cust/logo.png";
            InputParam += ParamDelimiter + CurrentAdaptationLogoUrl;


            di_jq.ajax({
                type: "POST",
                url: CallbackPageName,
                data: { 'callback': '271', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data != "") {
                            ShowCatalogLists(true);

                            alert("Adaptation saved successfully.")
                        }
                        else {
                            alert("Error while saving.");
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


function ShowCatalogLists(isRefreshCatalogLists) {
    try {
        di_jq("#divCatalog").css("display", "block");
        di_jq("#divAddAdptDetails").css("display", "none");

        if (isRefreshCatalogLists) {
            startRenderingAdaptations(true);
        }

        di_jq("#imgPreview").attr("src", "");
    }
    catch (err) { }
}


//-----------End Catalog---------------------------------------



var ErrorMessage = '';

function onPageLoad(hlngcode, hLoggedInUserNId) {
    try {
        var QSisRegisterAdmin = getQueryStr('isRegisterAdmin')
        if (QSisRegisterAdmin != null && QSisRegisterAdmin != undefined && QSisRegisterAdmin == 'true') {
            if (hLoggedInUserNId != null && hLoggedInUserNId != "") {
                ShowLoginOrRegisterForm(true);
            }
            else {
                HideShowLoginRegisterForm();
            }
        }
        else {
            createHiddenInputs();
            // set site language - hidden input
            z('hlngcode').value = hlngcode;
            // set logged in user nid.
            z("hLoggedInUserNId").value = hLoggedInUserNId;
            //z('radioHavePassword').checked = true;
            if (hLoggedInUserNId != null && hLoggedInUserNId != "") {
                var isAdminUpdate = getQueryStr('Update')
                if (isAdminUpdate != null && isAdminUpdate != '' && isAdminUpdate != undefined && isAdminUpdate == "UpdateAdmin") {
                    z('tblRegister').style.display = "block";
                    z('login_Password').style.display = "none";
                    z('txtHavePassword').style.display = "none";
                    z('aForgotPassword').style.display = "none";
                    z('btnLogin').style.display = "none";
                    z('spanEmailMandatory').style.display = "";
                    FillM49Countries("../Callback.aspx");
                    GetUserDetails(hLoggedInUserNId);
                }
                else {
                    HideShowLoginRegisterForm();
                }
            }
            else {
                HideShowLoginRegisterForm();
            }
        }
    }
    catch (err) { }
}

/*function to craete form and hidden fields */
function createHiddenInputs() {
    // For data posting                
    document.write("<input type='hidden' name='hLoggedInUserNId' id='hLoggedInUserNId' value='' />");
}

function Login() {

    var InputParam;
  
    if ((z('txtEmail').value != null) && (z('txtEmail').value != "") && (z('txtHavePassword').value != null) && (z('txtHavePassword').value != "")) {
        InputParam = z('txtEmail').value;
        InputParam += ParamDelimiter + z('txtHavePassword').value;
       
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '49', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        if (data.split(ParamDelimiter)[0] == "true") {
                            LoginLogoutUser(data.split(ParamDelimiter)[1], data.split(ParamDelimiter)[2]);

                            if (z('hdsby').value != null && z('hdsby').value != '' && z('hdsby').value.split('|')[0] != 'Login.aspx') {
                                PostData("frmLogin", z('hdsby').value.split('|')[0], "POST");
                            }
                            else {
                                PostData("frmLogin", "home.aspx", "POST");
                            }
                        }
                        else {
                            alert(data.split(ParamDelimiter)[1]);
                            z('txtEmail').focus();
                            z('txtHavePassword').value = "";
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
        catch (ex) {
        }
    }
    else {
        alert("Please enter email id and password!");
    }
}

function SignUp() {
    if (di_jq("#ddlCountry").val() == "-1") {
        alert("Please select a country");
        return;
    }
    var InputParam;

    ApplyMaskNLoader();

    if (ValidateInputs() == "true") {

        InputParam = z('txtEmail').value;
        InputParam += ParamDelimiter + z('txtFirstName').value;
        //InputParam += ParamDelimiter + z('txtLastName').value;
        InputParam += ParamDelimiter + di_jq('#ddlCountry').val();
        InputParam += ParamDelimiter + z('txtPassword').value;

        /*if (z('chkRegisterProvider').checked == true) {
        InputParam += ParamDelimiter + "true";
        }
        else {
        InputParam += ParamDelimiter + "false";
        }*/
        InputParam += ParamDelimiter + "false";
        InputParam += ParamDelimiter + "true";
        InputParam += ParamDelimiter + "false";
        InputParam += ParamDelimiter + document.getElementById('hlngcode').value;
        InputParam += ParamDelimiter + "false";
        CallBackPageName = "../CallBack.aspx";
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '48', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data.split(ParamDelimiter)[0] == "true") {
                            alert("Sign up successful! Please login!");
                            window.location.href = "Adminlogin.aspx";
                        }
                        else {
                            alert(data.split(ParamDelimiter)[1]);
                            //Reset();
                        }
                    }
                    catch (ex) {
                        RemoveLoaderNMask();
                        alert("message:" + ex.message);
                    }
                },
                error: function () {
                    RemoveLoaderNMask();

                },
                cache: false
            });
        }
        catch (ex) {
            RemoveLoaderNMask();
        }
    }
    else {
        alert(ErrorMessage);
    }

    RemoveLoaderNMask();
}

function Update() {
    var InputParam;
    if (di_jq("#ddlCountry").val() == "-1") {
        alert("Please select a country");
        return;
    }
    ApplyMaskingDiv();
    ShowLoadingDiv();

    if (ValidateInputs() == "true") {
        InputParam = z('hLoggedInUserNId').value;
        InputParam += ParamDelimiter + z('txtEmail').value;
        InputParam += ParamDelimiter + z('txtFirstName').value;
        InputParam += ParamDelimiter + di_jq('#ddlCountry').val();
        InputParam += ParamDelimiter + z('txtPassword').value;
        InputParam += ParamDelimiter + "false";
        InputParam += ParamDelimiter + "false";
        InputParam += ParamDelimiter + document.getElementById('hlngcode').value;
        CallBackPageName = "../Callback.aspx";
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '68', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        if (data.split(ParamDelimiter)[0].toLowerCase() == "true") {
                            alert("Details Updated! Please login!");
                            AdminLogout();
                        }
                        else {
                            alert(data.split(ParamDelimiter)[1]);
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
        catch (ex) {
        }
    }
    else {
        alert(ErrorMessage);
    }

    HideLoadingDiv();
    RemoveMaskingDiv();
}

function AdminLogout() {
    CallBackPageName = "../callback.aspx";
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: CallBackPageName,
        data: { 'callback': '280' },
        async: false,
        success: function (data) {
            try {
                if (data.toLowerCase() == "true") {
                    window.location.reload();
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
function ForgotPassword() {
    var InputParam;

    ApplyMaskingDiv();
    ShowLoadingDiv();
    CallBackPageName = "../callback.aspx"
    if ((z('txtEmail').value != null) && (z('txtEmail').value != "")) {
        InputParam = z('txtEmail').value;
        InputParam += ParamDelimiter + readCookie("hlngcode");
        try {
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: CallBackPageName,
                data: { 'callback': '70', 'param1': InputParam },
                async: false,
                success: function (data) {
                    try {
                        alert(data.split(ParamDelimiter)[1]);
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
        alert("Please enter Email Id!");
    }

    HideLoadingDiv();
    RemoveMaskingDiv();
}

function HandleEnter(event) {
    if (event.keyCode == 13) {
        z('btnLogin').click();
    }
}

function GetUserDetails(hLoggedInUserNId) {
    InputParam = hLoggedInUserNId.split('|')[0];
    CallBackPageName = "../callback.aspx";
    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '67', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    z('radioRegister').checked = true;
                    HideShowRegisterForm();
                    // HideShowLoginRegisterForm();
                    z('txtEmail').value = data.split(ParamDelimiter)[0];
                    //z('txtPassword').value = z("txtConfirmPassword").value = data.split(ParamDelimiter)[1];
                    z('txtPassword').value = z("txtConfirmPassword").value = "";
                    z('txtFirstName').value = data.split(ParamDelimiter)[2];
                    di_jq('#ddlCountry').val(data.split(ParamDelimiter)[4]);

                    /*if (data.split(ParamDelimiter)[5].toLowerCase() == "true") {
                    z('chkRegisterProvider').defaultChecked = true;
                    }
                    else {
                    z('chkRegisterProvider').defaultChecked = false;
                    }*/

                    z('btnSignup').style.display = "none";
                    z('btnUpdate').style.display = "inline";

                    z('radioRegister').style.display = "none";
                    //z('radioHavePassword').style.display = "none";
                    z('spanRegister').style.display = "none";
                    //z('spanHavePassword').style.display = "none";
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

function ValidateInputs() {
    var RetVal;

    RetVal = "true"

    if ((z('txtEmail').value == null) || (z('txtEmail').value == "")) {
        RetVal = "false";
        ErrorMessage = "Email is a mandatory field!";
    }
    else if (validateEmail(z('txtEmail').value) == false) {
        RetVal = "false";
        ErrorMessage = "Email id format is not correct!";
    }
    else if ((z('txtFirstName').value == null) || (z('txtFirstName').value == "")) {
        RetVal = "false";
        ErrorMessage = "First name is a mandatory field!";
    }
    else if ((z('txtPassword').value == null) || (z('txtPassword').value == "")) {
        RetVal = "false";
        ErrorMessage = "Please enter the password!";
    }
    else if (z('txtPassword').value.length < 8) {
        RetVal = "false";
        ErrorMessage = "Please enter a password longer than 8 characters!";
    }
    else if ((z('txtConfirmPassword').value == null) || (z('txtConfirmPassword').value == "")) {
        RetVal = "false";
        ErrorMessage = "Please confirm your password!";
    }
    else if (z('txtPassword').value != z('txtConfirmPassword').value) {
        RetVal = "false";
        ErrorMessage = "Password confirmation fails. Please re-enter password and confirm.";
        z('txtPassword').value = "";
        z('txtConfirmPassword').value = "";
    }

    return RetVal;
}

function HideShowRegisterForm() {
    Reset();
    if (z('radioRegister').checked == true) {
        z('tblRegister').style.display = "block";
        z('txtHavePassword').style.display = "none";
        z('aForgotPassword').style.display = "none";
        z('btnLogin').style.display = "none";
        z('spanEmailMandatory').style.display = "";
    }
    else if (z('radioHavePassword').checked == true) {
        z('tblRegister').style.display = "none";
        z('txtHavePassword').style.display = "block";
        z('aForgotPassword').style.display = "none";
        z('btnLogin').style.display = "block";
        z('spanEmailMandatory').style.display = "none";
    }
}

function Reset() {
    try {
        z('txtEmail').value = "";
        z('txtHavePassword').value = "";
        z('txtFirstName').value = "";
        di_jq('#ddlCountry').selectedIndex = 0;
        z('txtPassword').value = "";
        z('txtConfirmPassword').value = "";
        z('chkRegisterProvider').defaultChecked = false;

        z('btnSignup').style.display = "inline";
        z('btnUpdate').style.display = "none";

        z('radioRegister').style.display = "inline";
        //z('radioHavePassword').style.display = "inline";
        z('spanRegister').style.display = "inline";
        //  z('spanHavePassword').style.display = "inline";
    }
    catch (err) {
    }
}

function FillM49Countries(callbackurl) {
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: callbackurl,
        data: { 'callback': 270 },
        async: false,
        success: function (data) {
            try {
                if (data != "") {
                    var SelectObj = z("ddlCountry");
                    var MenuObj = document.createElement("option");
                    MenuObj.text = "Select";
                    MenuObj.value = "-1";
                    SelectObj.add(MenuObj);
                    var AreaData = jQuery.parseJSON(data);
                    for (var i = 0; i < AreaData.Areas.length; i++) {
                        var MenuObj = document.createElement("option");
                        var AreaNid = AreaData.Areas[i].AREANID;
                        var AreaName = AreaData.Areas[i].AREANAME;
                        MenuObj.text = AreaName;
                        MenuObj.value = AreaNid;
                        SelectObj.add(MenuObj);
                    }
                }
            }
            catch (ex) {

            }
        },
        error: function () {

        },
        cache: false
    });
}

function HideShowLoginRegisterForm() {
    z('txtEmail').value = "";
    z('txtHavePassword').value = "";
    z('txtFirstName').value = "";
    //z('txtLastName').value = "";
    di_jq('#ddlCountry').selectedIndex = 0;
    z('txtPassword').value = "";
    z('txtConfirmPassword').value = "";
    z('login_Password').checked = "";

    z('btnSignup').style.display = "inline";
    z('btnUpdate').style.display = "none";

    //z('radioRegister').style.display = "inline";
    //z('radioHavePassword').style.display = "inline";
    z('spanRegister').style.display = "inline";
    //z('spanHavePassword').style.display = "inline";

    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "../Callback.aspx",
        data: { 'callback': 277 },
        async: false,
        success: function (isAdminRegistered) {
            try {
                if (isAdminRegistered != "") {
                    if (isAdminRegistered.toLowerCase() == "false") {
                        z('tblRegister').style.display = "block";
                        z('login_Password').style.display = "none";
                        z('txtHavePassword').style.display = "none";
                        z('aForgotPassword').style.display = "none";
                        z('btnLogin').style.display = "none";
                        z('spanEmailMandatory').style.display = "";
                        FillM49Countries("../Callback.aspx");
                    }
                    else if (isAdminRegistered.toLowerCase() == "true") {
                        z('tblRegister').style.display = "none";
                        z('login_Password').style.display = "block";
                        z('txtHavePassword').style.display = "block";
                        z('aForgotPassword').style.display = "none";
                        z('btnLogin').style.display = "block";
                        z('spanEmailMandatory').style.display = "none";
                    }
                }
            }
            catch (ex) {

            }
        },
        error: function () {

        },
        cache: false
    });


}


function ShowLoginOrRegisterForm(IsShowRegForm) {
    z('txtEmail').value = "";
    z('txtHavePassword').value = "";
    z('txtFirstName').value = "";
    di_jq('#ddlCountry').selectedIndex = 0;
    z('txtPassword').value = "";
    z('txtConfirmPassword').value = "";
    z('login_Password').checked = "";

    z('btnSignup').style.display = "inline";
    z('btnUpdate').style.display = "none";

    z('spanRegister').style.display = "inline";
    try {
        if (IsShowRegForm != null && IsShowRegForm) {

            z('tblRegister').style.display = "block";
            z('login_Password').style.display = "none";
            z('txtHavePassword').style.display = "none";
            z('aForgotPassword').style.display = "none";
            z('btnLogin').style.display = "none";
            z('spanEmailMandatory').style.display = "";
            FillM49Countries("../Callback.aspx");
        }
        else {
            z('tblRegister').style.display = "none";
            z('login_Password').style.display = "block";
            z('txtHavePassword').style.display = "block";
            z('aForgotPassword').style.display = "none";
            z('btnLogin').style.display = "block";
            z('spanEmailMandatory').style.display = "none";

        }

    }
    catch (err) { }
}
function GetAllAdaptations() {
    try {
        alert(getAbsURL('$').split('$')[0]);
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '1004' },
            async: true,
            success: function (data) {
                if (data != "") {
                    FillAdaptationDdl(data, "ddlAdaptations", "");
                    GetAllUsers(0, 1, "ASC", "UserName")
                }
                else {
                }
            },
            error: function () {

            },
            cache: false
        });
    }
    catch (err) { }
}

//Select all users of current Adaptation
function GetCurrentAdaptationUsers(SarchKeyWord) {
    try {
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '1004' },
            async: true,
            success: function (data) {
                if (data != "") {
                    GetAllUsers(0, 1, "DESC", "Is Admin", SarchKeyWord)
                }
                else {
                }
            },
            error: function () {

            },
            cache: false
        });
    }
    catch (err) { }
}



function FillAdaptationDdl(data, ddlId, selectedId) {
    try {
        //Remove all options of dropdown            
        di_jq("#" + ddlId).find("option").remove()

        jsonData = di_jq.parseJSON(data);
        //        di_jq("#" + ddlId).append(di_jq('<option></option>').val(-1).html("--Select--"));
        di_jq("#" + ddlId).append(di_jq('<option></option>').val(0).html("All"));
        //Fill dropdown with area JSON data
        for (var i = 0; i < jsonData[""].length; i++) {

            AdaptationNId = jsonData[""][i].NId;
            AdaptationName = jsonData[""][i].Name;

            //Create and append option for dropdown
            di_jq("#" + ddlId).append(di_jq('<option></option>').val(AdaptationNId).html(AdaptationName));
        }

        if (selectedId != "") {
            di_jq("#" + ddlId).val(selectedId);
        }
    }
    catch (err) { }
}
function AdaptationChange(selectObj) {
    try {
        var idx = selectObj.selectedIndex;
        var value = selectObj.options[idx].value;
        if (value != -1) {
            GetAllUsers(value, 1, "ASC", "UserName");
        }
        else {
            di_jq("#" + AddCPH("divAdapUsersList")).html("");
            z('lang_ChangePassword').style.display = "none";
            z('divUserPaging').style.display = "none";
        }
    }
    catch (err) { }

}
function GetAllUsers(value, CurrentPage, sortType, sortExp, SearchStr) {
    gCurrentPage = CurrentPage;
    var idCurrentPage = "a_" + CurrentPage;
    //    if (z(idCurrentPage) != undefined)
    //        z(idCurrentPage).className = 'paginationCurrent';
    var InputParam = "";
    InputParam = value;
    InputParam += ParamDelimiter + CurrentPage;
    InputParam += ParamDelimiter + sortType;
    InputParam += ParamDelimiter + sortExp;
    if (SearchStr != null && SearchStr != undefined && SearchStr != '') {
        InputParam += ParamDelimiter + SearchStr;
    }

    try {
        di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: { 'callback': '1011', 'param1': InputParam },
            async: true,
            success: function (data) {
                if (data != "") {
                    di_jq("#" + AddCPH("divAdapUsersList")).html(data.split("[****]")[0]);
                    ManagePaginationBar('divUserPaging', data.split("[****]")[1], CurrentPage, SearchStr);
                    if (data.split("[****]")[1] == 0) {
                        z('lang_ChangePassword').style.display = "none";
                        z('lang_SetAsAdmin').style.display = "none";
                        z('pipeSetAsAdmin').style.display = "none";
                        z('pipeChangePassword').style.display = "none";

                    }
                    else {
                        z('lang_ChangePassword').style.display = "inline";
                        z('lang_SetAsAdmin').style.display = "inline";
                        z('pipeSetAsAdmin').style.display = "inline";
                        z('pipeChangePassword').style.display = "inline";
                    }
                }
                else {
                }
            },
            error: function () {

            },
            cache: false
        });
    }
    catch (err) { }
}

function ChangeAdapUserPassword() {
    var SelectedUserRow;
    try {

        if (di_jq("#" + AddCPH("divAdapUsersList") + " :checked").length > 0) {
            di_jq("#" + AddCPH("divAdapUsersList") + " :checked").each(function () {
                SelectedUserNId = di_jq(this).context.id.replace("AdapUser_", "");
                SelectedUserRow = di_jq(this)[0].parentNode.parentNode;
            });

            CallChangepassword(SelectedUserRow, SelectedUserNId);
        }
    }
    catch (err) { }
}

//Section for setting user as Admin start-----------------------------------------

// function for showing and hiding popup for Setting Admin

function ConfirmAlertReturn(msg) {
    return confirm(msg);
}
function SetAsAdminPopUp() {
    var IsUserAdmin;
    var ConfirmAlertMsg = '';
    try {
        if (di_jq("#" + AddCPH("divAdapUsersList") + " :checked").length > 0) {

            di_jq("#" + AddCPH("divAdapUsersList") + " :checked").each(function () {
                SelectedUserNId = di_jq(this).context.id.replace("AdapUser_", "");
                SelectedUserRow = di_jq(this)[0].parentNode.parentNode;
                IsUserAdmin = GetTblCellValue(SelectedUserRow, 4)
            });
            if (IsUserAdmin == "True") {
                alert("Selected user is already admin, please select different user.");
            }
            else {
                ConfirmAlertMsg = 'There can only be one Admin for this adaptation. Click OK to change the admin account.'
                if (ConfirmAlertReturn(ConfirmAlertMsg))
                    CallSetUserAsAdmin(SelectedUserRow, SelectedUserNId);
            }
        }
        else {
            alert("Select a user.");
        }
    }
    catch (err) { }
}
// function for setting new Admin

function AddNewAdminPopUp() {
    var ConfirmAlertMsg = '';
    try {
        ConfirmAlertMsg = 'There can only be one Admin for this adaptation. Click OK to create new admin account.'
        if (ConfirmAlertReturn(ConfirmAlertMsg)) {
            window.location.href = "Adminlogin.aspx?isRegisterAdmin=true";
        }
    }
    catch (err) { }
}

// Call method to set new user as admin
function CallSetUserAsAdmin(row, SelectedUserNId) {
    try {
        var InputParam;

        var UserNid;
        var UserName = GetTblCellValue(row, 1)
        var EmailID = GetTblCellValue(row, 2)
        UserNid = SelectedUserNId;
        try {
            InputParam = UserNid + ParamDelimiter + readCookie("hlngcode") + ParamDelimiter + UserName + ParamDelimiter + EmailID;
            var htmlResp = di_jq.ajax({
                type: "GET",
                url: "../" + CallBackPageName,
                data: { 'callback': '1012', 'param1': InputParam },
                async: false,
                success: function (result) {
                    try {
                        if (result == "True") {
                            alert("Admin login has been changed. Click Ok to login using the new account");
                            location.reload();
                        }
                    }
                    catch (ex) {
                        alert("message:" + ex.message);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {

                },
                cache: false
            });
        }
        catch (ex) {
        }
    }
    catch (err) { }
}

//Section for setting user as Admin end-----------------------------------------
function ChangePasswordPopup(isOpen) {
    try {
        if (isOpen) {
            if (di_jq("#" + AddCPH("divAdapUsersList") + " :checked").length > 0) {
                ApplyMaskingDiv();
                SetCloseButtonInPopupDiv(di_jq('#ChangePasswodPopup'), 'ChangePasswordPopup', CloseImgSrc);
                di_jq('#ChangePasswodPopup').show('slow');
                GetWindowCentered(z("ChangePasswodPopup"), 710, 450);
                di_jq('#txtNewPwdCP').val('');
                di_jq('#txtConfirmPwdCP').val('');
            }
            else {
                alert("Select a user.");
            }
        }
        else {
            di_jq('#ChangePasswodPopup').hide('slow');
            RemoveMaskingDiv();
        }
    }
    catch (err) { }
}

function CallChangepassword(row, SelectedUserNId) {
    try {
        var InputParam;
        if (ValidatePasswordInputs() == "true") {
            var UserNid;
            var Password = z("txtNewPwdCP").value;
            var UserName = GetTblCellValue(row, 1)
            var EmailID = GetTblCellValue(row, 2)
            UserNid = SelectedUserNId;
            try {
                InputParam = UserNid + ParamDelimiter + Password + ParamDelimiter + readCookie("hlngcode") + ParamDelimiter + UserName + ParamDelimiter + EmailID;
                var htmlResp = di_jq.ajax({
                    type: "POST",
                    url: "../" + CallBackPageName,
                    data: { 'callback': '1006', 'param1': InputParam },
                    async: false,
                    success: function (result) {
                        try {
                            if (result == "True") {
                                alert("Changed Successfully");
                                ChangePasswordPopup(false);
                            }
                        }
                        catch (ex) {
                            alert("message:" + ex.message);
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {

                    },
                    cache: false
                });
            }
            catch (ex) {
            }
        }
        else {
            alert(ErrorMessage);
        }
        HideLoadingDiv();
        RemoveMaskingDiv();
    }
    catch (err) { }
}

function ValidatePasswordInputs() {
    var RetVal;

    RetVal = "true"
    if ((z('txtNewPwdCP').value == null) || (z('txtNewPwdCP').value == "")) {
        RetVal = "false";
        ErrorMessage = z("lngEnterPwdMsg").innerHTML;
    }
    else if (z('txtNewPwdCP').value.length < 8) {
        RetVal = "false";
        ErrorMessage = z("lngPwdLenMsg").innerHTML;
    }
    else if ((z('txtConfirmPwdCP').value == null) || (z('txtConfirmPwdCP').value == "")) {
        RetVal = "false";
        ErrorMessage = z("lngConfirmPwdMsg").innerHTML;
    }
    else if (z('txtNewPwdCP').value != z('txtConfirmPwdCP').value) {
        RetVal = "false";
        ErrorMessage = z("lngReenterPwdMsg").innerHTML;
        z('txtNewPwdCP').value = "";
        z('txtConfirmPwdCP').value = "";
    }

    return RetVal;
}

function UserPaginationBar(divPagingBar, NoOfPages) {
    try {
        var divPaging = z(divPagingBar);
        divPaging.innerHTML = "";
        //  var ddlAdaptation = z('ddlAdaptations');
        //var idx = ddlAdaptation.selectedIndex;
        for (var inc = 1; inc <= NoOfPages; inc++) {
            var aIPage = ce('a');
            aIPage.innerHTML = inc;
            aIPage.href = 'javascript:void(0);';
            aIPage.value = inc;
            aIPage.className = 'a';
            aIPage.id = "a_" + inc;
            aIPage.onclick = function (event) {
                GetAllUsers(0, this.value, UserSortType, UserSortExp, SearchStr);
            };
            divPaging.appendChild(aIPage);
            var spanSpace1 = ce('span');
            spanSpace1.innerHTML = " ";
            var spanSpace2 = ce('span');
            spanSpace2.innerHTML = " ";
            divPaging.appendChild(spanSpace1);
            divPaging.appendChild(spanSpace2);
        }
        z('divUserPaging').style.display = "inline";
    }
    catch (err) { }
}

function ManagePaginationBar(divPagingBar, NoOfPages, currentPageNumber, SearchStr) {
    var divPaging = z(divPagingBar);
    divPaging.innerHTML = "";
    var PageCount = NoOfPages;
    var totalPagingMenuLength = 10
    var Icount = 1;
    var StartIndex = 1;
    var LastIndex = -1;
    var HalfPageSize = totalPagingMenuLength / 2;

    LastIndex = PageCount;
    //Get Current Menu Numbers
    if (currentPageNumber <= HalfPageSize) {
        if (PageCount > totalPagingMenuLength) {
            LastIndex = totalPagingMenuLength;
        }
        else {
            LastIndex = PageCount;
        }
    }
    else {
        if (currentPageNumber > (PageCount - HalfPageSize)) {
            if (PageCount > totalPagingMenuLength) {
                StartIndex = PageCount - totalPagingMenuLength;
            }
        }
        else {
            if (PageCount > totalPagingMenuLength) {
                StartIndex = currentPageNumber - HalfPageSize;
                LastIndex = currentPageNumber + HalfPageSize;
            }
        }
    }

    CreateNextPreviousButtons(divPaging, "<<", currentPageNumber, PageCount);
    //Move Previous
    CreateNextPreviousButtons(divPaging, "<", currentPageNumber, PageCount);

    var divTag = ce('div');
    divTag.className = "pagging_div";
    divTag.id = "divPaging";


    for (var Icount = StartIndex; Icount <= LastIndex; Icount++) {
        var aIPage = ce('a');
        aIPage.innerHTML = Icount;
        aIPage.href = 'javascript:void(0);';
        aIPage.value = Icount;
        if (currentPageNumber == Icount) {
            aIPage.className = "pagination_lnk page_selected";
            aIPage.value = Icount;
        }
        else {
            aIPage.className = "pagination_lnk";
            aIPage.value = Icount;
        }
        // aIPage.className = 'a';
        aIPage.id = "a_" + Icount;
        aIPage.onclick = function (event) {
            GetAllUsers(0, this.value, UserSortType, UserSortExp, SearchStr);
        };
        divTag.appendChild(aIPage);
    }
    divPaging.appendChild(divTag);
    //Move Next
    CreateNextPreviousButtons(divPaging, ">", currentPageNumber, PageCount, SearchStr);
    //Move Last
    CreateNextPreviousButtons(divPaging, ">>", currentPageNumber, PageCount, SearchStr);

    z('divUserPaging').style.display = "inline";
}

function CreateNextPreviousButtons(divPaging, symbol, currentPageNumber, pageSize, SearchStr) {
    var aIPage = ce('input');
    var InputIPage = ce('input');
    aIPage.type = "button";
    if (symbol == "<<") {
        InputIPage.className = "dbl_lft_arrw pagging_marg_rt_fr";
        if (currentPageNumber == 1) {
            InputIPage.disabled = "disabled";
        }
        else {
            InputIPage.onclick = function (event) {
                GetAllUsers(0, 1, UserSortType, UserSortExp, SearchStr);
            };
        }
        //  InputIPage.value = 1;
        InputIPage.id = "a_" + 1;
    }
    else if (symbol == "<") {
        InputIPage.className = "sngl_lft_arrw pagging_marg_rt_tw";
        if (currentPageNumber == 1) {
            InputIPage.disabled = "disabled";
        }
        else {
            InputIPage.onclick = function (event) {
                GetAllUsers(0, currentPageNumber - 1, UserSortType, UserSortExp, SearchStr);
            };
        }
        //InputIPage.value = currentPageNumber - 1;
        InputIPage.id = "a_" + currentPageNumber - 1;
    }
    else if (symbol == ">") {
        InputIPage.className = "sngl_rgt_arrw pagging_marg_lt_tw";
        if (currentPageNumber == pageSize) {
            InputIPage.disabled = "disabled";
        }
        else {
            InputIPage.onclick = function (event) {
                GetAllUsers(0, currentPageNumber + 1, UserSortType, UserSortExp, SearchStr);
            };
        }
        InputIPage.id = "a_" + currentPageNumber + 1;
    }
    else if (symbol == ">>") {
        InputIPage.className = "dbl_rgt_arrw pagging_marg_lt_fr";
        if (currentPageNumber == pageSize) {
            InputIPage.disabled = "disabled";
        }
        else {
            InputIPage.onclick = function (event) {
                GetAllUsers(0, pageSize, UserSortType, UserSortExp, SearchStr);
            };
        }
        // InputIPage.value = pageSize;
        InputIPage.id = "a_" + pageSize;
    }
    InputIPage.href = 'javascript:void(0);';
    divPaging.appendChild(InputIPage);
}


function ce(ElementType) {
    var tmpElement = document.createElement(ElementType);
    return tmpElement;
}
var UserSortExp = "Is Admin";
var UserSortType = "DESC";
function Sort(objectId) {
    try {
        var searchStr = '';
        if (di_jq('#kwd_search').val() != "" && di_jq('#kwd_search').val() != "Enter username/email to search") {
            searchStr = di_jq('#kwd_search').val();
        }
        var sortType = objectId.split("[^^^^]_")[0];
        var sortExpression = objectId.split("[^^^^]_")[1];
        UserSortType = sortType;
        UserSortExp = sortExpression;
        //var ddlAdaptation = z('ddlAdaptations');
        //var idx = ddlAdaptation.selectedIndex;
        //GetAllUsers(ddlAdaptation.options[idx].value, gCurrentPage, sortType, sortExpression)
        GetAllUsers(0, gCurrentPage, sortType, sortExpression, searchStr)
    }
    catch (err) { }

}

function ImportXmlPopup(isOpen) {
    try {
        if (isOpen) {

            ApplyMaskingDiv();
            SetCloseButtonInPopupDiv(di_jq('#ImportXmlPopup'), 'ImportXmlPopup', CloseImgSrc);
            di_jq('#ImportXmlPopup').show('slow');
            GetWindowCentered(z("ImportXmlPopup"), 710, 450);
        }
        else {
            di_jq('#ImportXmlPopup').hide('slow');
            RemoveMaskingDiv();
        }
    }
    catch (err) { }
}

function showChangePwdDiv(isOpen) {
    if (isOpen) {
        var xPos = 480;
        var yPos = 385;
        ApplyMaskingDiv(1000);
        Youtubeclose(di_jq('#DivOuterEditPassword'), 'showChangePwdDiv', CloseImgSrc);
        di_jq('#DivOuterEditPassword').show('slow');
        GetWindowCentered(z("DivOuterEditPassword"), xPos, yPos);
    }
    else {
        di_jq('#DivOuterEditPassword').hide('slow');
        RemoveMaskingDiv();
    }
}

function ChangeDBPassword() {
    if (di_jq.trim(di_jq("#ContentPlaceHolder1_txtOPWD").val()) == "") {
        alert(msgOldPwd);
    }
    else if (di_jq.trim(di_jq("#ContentPlaceHolder1_txtNOPWD").val()) == "") {
        alert(msgNewPwd);
    }
    else if (di_jq.trim(di_jq("#ContentPlaceHolder1_txtNOPWDR").val()) == "") {
        alert(msgCNewPwd);
    }
    else if (di_jq.trim(di_jq("#ContentPlaceHolder1_txtNOPWD").val()) != di_jq.trim(di_jq("#ContentPlaceHolder1_txtNOPWDR").val())) {
        alert(msgPwdNotMatch);
    }
    else {
        InputParam = di_jq.trim(di_jq("#ContentPlaceHolder1_txtOPWD").val()) + ParamDelimiter + di_jq.trim(di_jq("#ContentPlaceHolder1_txtNOPWD").val());
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: "../" + CallBackPageName,
            data: { 'callback': '1040', 'param1': InputParam },
            async: false,
            success: function (result) {
                try {
                    if (result == "0") {
                        alert("Old password is wrong");
                    }
                    else if (result == "-1") {

                    }
                    else {
                        alert("Password updated");
                        di_jq("#" + AddCPH("txtPassword")).val(di_jq.trim(di_jq("#ContentPlaceHolder1_txtNOPWD").val()));
                        di_jq("#ContentPlaceHolder1_txtOPWD").val("");
                        di_jq("#ContentPlaceHolder1_txtNOPWD").val("");
                        di_jq("#ContentPlaceHolder1_txtNOPWDR").val("");
                        di_jq('#DivOuterEditPassword').hide('slow');
                        RemoveMaskingDiv();
                    }
                }
                catch (ex) {
                    alert("message:" + ex.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {

            },
            cache: false
        });
    }
}

//function CheckIfHeaderFileExists() {
//    var StockPath = getAbsURL('stock')
//    var DbNid = getDefaultDbId();
//    var headerFilePath = StockPath + 'data/'+DbNid+'/sdmx/header.xml';
//    if (!fileExists(headerFilePath)) {
//        return false;
//    }
//    else {
//        return true;
//    }
//}

//function fileExists(url)
//{
//    var http = new XMLHttpRequest();
//    http.open('HEAD', url, false);
//    http.send();
//    return http.status!=404;
//}
