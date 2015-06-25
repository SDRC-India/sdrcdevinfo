// JScript File
var ParamDelimiter = "[****]";
var CallBackPageName = "CallBack.aspx";
var CookiePostfixStr = "";
var AreaCompObj = '';
var IndCompObj = '';
var isSearchResultsGalleryEnabled = false;
var AppVersion = '';
var IsGlobalAdaptation = 'false';
var isGlobalAdap = "";
var LiDimensionsArr = [];
function z(ElementID) {
    var tmpElement = document.getElementById(ElementID);
    return tmpElement;
}
var IsShowElements = true;

function ConfirmToDelete(msg) {
    var RetVal = false;
    var MsgConfirmDelete = "Confirm to delete ?"
    if (msg != '') {
        MsgConfirmDelete = msg;
    }
    RetVal = confirm(MsgConfirmDelete);
    return RetVal;
}

function SetCatalogPageClass() {
    z('aCatalog').className = "navActive";
}

function CheckValidation(ctrlObj) {
    if (z("ContentPlaceHolder1_txtDatabaseName").value != "" && z("ContentPlaceHolder1_txtServerName").value != "" && z("ContentPlaceHolder1_txtUserName").value != "" && z("ContentPlaceHolder1_txtPassword").value != "") {
        try {
            var InputParam = z("ContentPlaceHolder1_txtServerName").value + "[~]" + z("ContentPlaceHolder1_txtDatabaseName").value + "[~]" + z("ContentPlaceHolder1_txtUserName").value + "[~]" + z("ContentPlaceHolder1_txtPassword").value;
            di_jq.ajax({
                type: "POST",
                url: "../Callback.aspx",
                data: { 'callback': '1017', 'param1': InputParam },
                async: true,
                success: function (data) {
                    try {
                        z("ContentPlaceHolder1_txtDesc").value = data;
                    }
                    catch (ex) {
                    }
                },
                error: function () {
                },
                cache: false
            });
        }
        catch (err) { }
    }
}

/*function to craete form and hidden fields */
function createFormHiddenInputs(formID, method) {
    // For data posting
    document.write("<form id='" + formID + "' method='" + method + "'>");
    document.write("<input type='hidden' name='hdsby' id='hdsby' value='topic' />");
    document.write("<input type='hidden' name='hdbnid' id='hdbnid' value='' />");
    document.write("<input type='hidden' name='hselarea' id='hselarea' value='' />");
    document.write("<input type='hidden' name='hselind' id='hselind' value='' />");
    document.write("<input type='hidden' name='hlngcode' id='hlngcode' value='' />");
    document.write("<input type='hidden' name='hlngcodedb' id='hlngcodedb' value='' />");
    document.write("<input type='hidden' name='hselindo' id='hselindo' value='' />");
    document.write("<input type='hidden' name='hselareao' id='hselareao' value='' />");
    document.write("<input type='hidden' name='hdvnids' id='hdvnids' value='' />");
    document.write("<input type='hidden' name='hsgcount' id='hsgcount' value='' />");
    document.write("<input type='hidden' name='hLoggedInUserNId' id='hLoggedInUserNId' value='' />");
    document.write("<input type='hidden' name='hLoggedInUserName' id='hLoggedInUserName' value='' />");
    document.write("</form>");
}

function showMapServerToolTip(elementID) {
    var toolTipMessage = '';
    var vistType = di_jq(elementID).attr('rel');
    toolTipMessage = z("MapServerToolTip").value
    ShowCalloutForMap('divCallout', toolTipMessage, elementID);
}

function SetCommonLinksHref(formID, method, sourcepage) {
    try {
        if (document.URL.indexOf("articles/") == -1) {
            z('aHeader').href = "javascript:PostData('" + formID + "','home.aspx','" + method + "');";
            z('aHome').href = "javascript:PostData('" + formID + "','home.aspx','" + method + "');";
            z('aGalleries').href = "javascript:PostData('" + formID + "','Gallery.aspx','" + method + "');";
            //  z('aInnovations').href = "javascript:PostData('" + formID + "','../../articles/innovations','" + method + "');";

            z('aMydata').href = "javascript:PostData('" + formID + "','mydata.aspx','" + method + "');";
            z('aCatalog').href = "javascript:PostData('" + formID + "','Catalog.aspx','" + method + "');";
            // For Login Link
            z('aLogin').href = "javascript:PostDataToLogin('" + formID + "','" + method + "','New');";
            z('aUser').href = "javascript:PostDataToLogin('" + formID + "','" + method + "','Update');";

            // For Registry Link
            z('hiddenFrmId').value = formID;
            z('hiddenPostMethod').value = method;
            //z('aNews').href = "javascript:PostData('" + formID + "','../../articles/'" + z(Adaptation_News).value + ",'" + method + "');";

            // For Login Link
            if (z('langDataQueryService') != null) {
                if (z('langDataQueryService').href != null && z('langDataQueryService').href != undefined) {
                    setTimeout(function () { z('langDataQueryService').href = "javascript:PostData('" + formID + "','RegDataQuery.aspx','" + method + "');"; }, 500);
                }
            }
        }
        else {
            z('aHeader').href = "javascript:PostData('" + formID + "','../libraries/aspx/home.aspx','" + method + "');";
            z('aHome').href = "javascript:PostData('" + formID + "','../libraries/aspx/home.aspx','" + method + "');";
            z('aGalleries').href = "javascript:PostData('" + formID + "','../libraries/aspx/Gallery.aspx','" + method + "');";
            // z('aNews').href = "javascript:PostData('" + formID + "','" + z(Adaptation_News).value + "','" + method + "');";
            //z('aInnovations').href = "javascript:PostData('" + formID + "','articles/innovations','" + method + "');";
            z('aMydata').href = "javascript:PostData('" + formID + "','../libraries/aspx/mydata.aspx','" + method + "');";
            z('aCatalog').href = "javascript:PostData('" + formID + "','../libraries/aspx/Catalog.aspx','" + method + "');";

            // For Login Link
            z('aLogin').href = "javascript:PostDataToLogin('" + formID + "','" + method + "','New');";
            z('aUser').href = "javascript:PostDataToLogin('" + formID + "','" + method + "','Update');";

            // For Registry Link
            z('hiddenFrmId').value = formID;
            z('hiddenPostMethod').value = method;

            // For Login Link
            if (z('langDataQueryService') != null) {
                if (z('langDataQueryService').href != null && z('langDataQueryService').href != undefined) {
                    setTimeout(function () {
                        z('langDataQueryService').href = "javascript:PostData('" + formID + "','../libraries/aspx/RegDataQuery.aspx','" + method + "');";
                    }, 500);
                }
            }
        }
    }
    catch (err)
    { }
}

/* function to set post data */
function setPostedData(hdsby, hdbnid, hselarea, hselind, hlngcode, hlngcodedb, hselindo, hselareao, hsgcount, hLoggedInUserNId, hLoggedInUserName, hdvnids) {
    // Data Search by
    z('hdsby').value = hdsby;

    // set selected database NID - hidden input
    z('hdbnid').value = hdbnid;

    // set selected areas - hidden input
    z('hselarea').value = hselarea;

    // set selected indicators - hidden input
    z('hselind').value = hselind;

    // set site language - hidden input
    z('hlngcode').value = hlngcode;

    // set database language
    z('hlngcodedb').value = hlngcodedb;

    // set selected indicator JSON object
    z('hselindo').value = hselindo;

    // set selected indicator JSON object
    z('hselareao').value = hselareao;

    // set count of subgroup will display in indicator compoennt
    z('hsgcount').value = hsgcount;

    // set logged in user nid.
    z('hLoggedInUserNId').value = hLoggedInUserNId;

    // set logged in user name.
    z('hLoggedInUserName').value = hLoggedInUserName;

    z('hdvnids').value = hdvnids;

    if (z('hdsby').value != 'share') {
        //Setting Login/Logout Information
        try {
            LoginLogoutUser(hLoggedInUserNId, hLoggedInUserName);
        } catch (e) {

        }

    }
}

function SetOriginaldbnidInForm(formId, hOriginaldbnid) {
    var hiddenField = document.createElement("input");
    hiddenField.setAttribute("type", "hidden");
    hiddenField.setAttribute("id", "hOriginaldbnid");
    hiddenField.setAttribute("name", "hOriginaldbnid");
    hiddenField.setAttribute("value", hOriginaldbnid);

    if (z(formId) != null) {
        z(formId).appendChild(hiddenField);
    }
}

function PostDataToLogin(formID, PostingMethod, Action) {
    if (formID == 'frmHome') {
        z('hdsby').value = 'home.aspx';
    }
    else if (formID == 'frmLogin') {
        z('hdsby').value = 'Login.aspx';
    }
    else if (formID == 'frm_dv') {
        z('hdsby').value = 'DataView.aspx';
    }
    else if (formID == 'frm_sdmxStructure') {
        z('hdsby').value = 'RegStructure.aspx';
    }
    else if (formID == 'frmGallery') {
        z('hdsby').value = 'Gallery.aspx';
    }
//    else if (formID == 'frm_sdmxStructureMetadata') {
//        z('hdsby').value = 'RegStructuralMD.aspx';
//    }
    else if (formID == 'frm_sdmxSubscription') {
        z('hdsby').value = 'RegSubscription.aspx';
    }
    else if (formID == 'frm_sdmxPublishData') {
        z('hdsby').value = 'RegPublishData.aspx';
    }
    else if (formID == 'frm_sdmxProvisioningMetadata') {
        z('hdsby').value = 'RegProviders.aspx';
    }
    else if (formID == 'frm_sdmxValidate') {
        z('hdsby').value = 'RegValidation.aspx';
    }
    else if (formID == 'frm_sdmxCompare') {
        z('hdsby').value = 'RegComparison.aspx';
    }
    else if (formID == 'frm_DataQuery') {
        z('hdsby').value = 'RegDataQuery.aspx';
    }
    else if (formID == 'frm_sdmxStructureMetadata-Metadata') {
        z('hdsby').value = 'RegStructuralMD-Metadata.aspx';
    }
    else if (formID == 'frm_sdmxWSDemo') {
        z('hdsby').value = 'RegWebServiceDemo.aspx';
    }
    else if (formID == 'frm_sdmxDiscover') {
        z('hdsby').value = 'RegDiscover.aspx';
    }
    else if (formID == 'frm_sdmxMapping') {
        z('hdsby').value = 'RegMapping.aspx';
    }
    else if (formID == 'frm_sdmxMaintenanceAgency') {
        z('hdsby').value = 'RegMaintenanceAgency.aspx';
    }
    else if (formID == 'frm_sdmxDataProvider') {
        z('hdsby').value = 'RegDataProvider.aspx';
    }
    else if (formID == 'frmCYV') {
        z('hdsby').value = 'MyData.aspx';
    }
    else if (formID == 'frmCatalog') {
        z('hdsby').value = 'Catalog.aspx';
    }
    else if (formID == 'frmNews') {
        z('hdsby').value = 'News.aspx?T=N&PN=diorg/di_news.html';
    }
    else if (formID == 'frmInnovations') {
        z('hdsby').value = 'Innovations.aspx?T=I&PN=diorg/di_innovations.html';
    }
    else if (formID == 'frm_sdmxDiscoverRegistrations') {
        z('hdsby').value = 'RegDiscoverRegistrations.aspx';
    }
    else if (formID == 'frmAddCMSContent') {
        z('hdsby').value = 'AddCMSContent.aspx';
    }
    else if (formID == 'frmEditCMSContent') {
        z('hdsby').value = 'EditCMSContent.aspx';
    }
    else if (formID == 'frmArticles') {
        z('hdsby').value = 'home.aspx';
    }
    else if (formID == 'frmArticleImport') {
        z('hdsby').value = 'ArticleImport.aspx';
    }
    else if (formID == 'frmEditCMSContents') {
        z('hdsby').value = 'frmEditCMSContents.aspx';
    }
    else if (formID == 'frmFAQ') {
        z('hdsby').value = 'FAQ.aspx?T=A&PN=diorg/di_FAQ_General.html';
    }
    else if (formID == 'frmManageCategories') {
        z('hdsby').value = 'ManageCategories.aspx';
    }
    else if (formID == 'frmWorldwide') {
        z('hdsby').value = 'DevInfoWorldwide.aspx?T=W1&PN=diorg/worldwide.html';
    }
    else if (formID == 'frmHwToVideo') {
        z('hdsby').value = 'HowToVideo.aspx?T=DF&PN=diorg/how_to_video.html';
    }
    else if (formID == 'frmSiteMap') {
        z('hdsby').value = 'SiteMap.aspx';
    }
    else if (formID == 'frm_sdmxDatabaseManagement') {
        z('hdsby').value = 'RegDatabaseManagement.aspx';
    }
    else if (formID == 'frm_sdmxTools') {
        z('hdsby').value = 'RegTools.aspx';
    }
    else if (formID == 'frm_sdmxUpload') {
        z('hdsby').value = 'RegUpload.aspx';
    }
    else if (formID == 'frm_sdmxData') {
        z('hdsby').value = 'RegData.aspx';
    }
    else if (formID == 'frm_regHeaderDetail') {
        z('hdsby').value = 'RegHeaderDetail.aspx';
    }
    z('hdsby').value = z('hdsby').value + "|" + Action;

    if (document.URL.indexOf("articles/") == -1) {
        PostData(formID, 'Login.aspx', PostingMethod);
    }

    else {
        PostData(formID, '../libraries/aspx/Login.aspx', PostingMethod);
    }
}

//&& pgName != 'RegStructuralMD.aspx'
/* function to post data */
function PostData(formID, pgName, PostingMethod) {
    if (z('hOriginaldbnid') != null && pgName != 'RegData.aspx' && pgName != 'RegStructure.aspx' && pgName != 'RegStructuralMD-Metadata.aspx' &&
    pgName != 'RegSubscription.aspx' && pgName != 'RegPublishData.aspx' && pgName != 'RegProviders.aspx' &&
    pgName != 'RegValidation.aspx' && pgName != 'RegComparison.aspx' && pgName != 'RegWebServiceDemo.aspx' &&
    pgName != 'RegDiscover.aspx' && pgName != 'Login.aspx' && pgName != 'RegMapping.aspx' &&
    pgName != 'RegMaintenanceAgency.aspx' && pgName != 'RegDatabaseManagement.aspx' && pgName != 'RegUpload.aspx' && pgName != 'RegDataProvider.aspx' && pgName != 'RegDiscoverRegistrations.aspx' && pgName != 'RegDataQuery.aspx' && pgName != 'RegTools.aspx') {
        z('hdbnid').value = z('hOriginaldbnid').value;
    }

    z(formID).action = pgName;
    z(formID).method = PostingMethod;
    z(formID).submit();
}

function LoginLogoutUser(UserNId, UserFirstName, isLogout) {
    var loginLogoutFlag;
    var URL = '';
    if (isLogout == true) {
        if (GetStandalonRegistryStatus().toLowerCase() != "true") {
            z('aLogout').innerHTML = "Logging Out ...";
            z('aLogout').style.textDecoration = "none";
            z('aLogout').style.cursor = "default";

            if (document.URL.indexOf("articles/") != -1) {
                URL = "../libraries/aspx/Callback.aspx";
            }
            else {
                URL = "Callback.aspx";
            }
            var htmlResp = di_jq.ajax({
                type: "POST",
                url: URL,
                data: "callback=1002",
                async: false,
                success: function () {
                    try {
                        if (document.URL.indexOf("articles/") != -1) {
                            location.reload();
                        }
                        else {
                            window.location.href = "home.aspx";
                        }
                    }
                    catch (ex) {
                        // alert("Error : " + ex.message);
                    }
                },
                error: function () {
                    //
                },
                cache: false
            });
        }
    }

    if (UserNId != "" && UserNId != null && UserFirstName != "" && UserFirstName != null) {
        z('hLoggedInUserNId').value = UserNId;
        z('hLoggedInUserName').value = UserFirstName;

        if (document.URL.indexOf("articles/") != -1) {
            if (CheckIfMasterAccount(z('hLoggedInUserNId').value) != 'true') {
                z('aUser').innerHTML = UserFirstName;
                z('aUser').style.display = "inline";
                z('liUser').style.display = "inline";
            }
        }
        else if (z('aUser').style.display == "none") {
            if (z('hLoggedInUserNId').value != '' && z('hLoggedInUserNId').value.split('|').length > 1) {
                if (CheckIfMasterAccount(z('hLoggedInUserNId').value.split('|')[0]) != 'true') {
                    z('aUser').innerHTML = UserFirstName;
                    z('aUser').style.display = "inline";
                    z('liUser').style.display = "inline";
                }
            }
        }
        z('aLogin').style.display = "none";
        z('aLogout').style.display = "inline";
        if (z("aAdminPanel").style.display == "none") {
            if (CheckIfSiteAdminLoggedInShowSpecificTabs() == 'true') {
                z('aAdminPanel').style.display = "inline";
                z('liAdminPanel').style.display = "inline";
            }
        }
        loginLogoutFlag = true;
    }
    else {
        z('hLoggedInUserNId').value = "";
        z('hLoggedInUserName').value = "";

        z('aUser').innerHTML = "";
        z('aUser').style.display = "none";
        z('liUser').style.display = "none";

        z('aLogin').style.display = "inline";
        z('aLogout').style.display = "none";

        z('aAdminPanel').style.display = "none";
        z('liAdminPanel').style.display = "none";

        if (z('hiddenFrmId').value == 'frmLogin') {
            z('txtEmail').value = "";
            z('radioHavePassword').checked = true;
            z('chkRegisterProvider').style.display = z('spanRegisterProvider').style.display = z('imghelpRegisterProvider').style.display = "";
            HideShowRegisterForm();
        }
        loginLogoutFlag = false;
    }

    //z('hiddenFrmId').value == 'frm_sdmxStructureMetadata' ||
    if (z('hiddenFrmId').value == 'frm_sdmxSubscription' || z('hiddenFrmId').value == 'frm_sdmxPublishData' ||
         z('hiddenFrmId').value == 'frm_sdmxProvisioningMetadata' ||
        z('hiddenFrmId').value == 'frm_sdmxValidate' || z('hiddenFrmId').value == 'frm_sdmxCompare' ||
        z('hiddenFrmId').value == 'frm_sdmxStructureMetadata-Metadata' || z('hiddenFrmId').value == 'frm_sdmxWSDemo'
        || z('hiddenFrmId').value == 'frm_sdmxDiscover' || z('hiddenFrmId').value == 'frm_sdmxMapping' ||
        z('hiddenFrmId').value == 'frm_sdmxDatabaseManagement' ||
        z('hiddenFrmId').value == 'frm_sdmxUpload' ||
        z('hiddenFrmId').value == 'frm_sdmxData' ||
        z('hiddenFrmId').value == 'frm_sdmxMaintenanceAgency' || z('hiddenFrmId').value == 'frm_sdmxDataProvider' || z('hiddenFrmId').value == 'frm_sdmxDiscoverRegistrations' ||
        z('hiddenFrmId').value == 'frm_sdmxTools' || z('hiddenFrmId').value == 'frm_sdmxStructure') {
        HandleLoginForRegistry(loginLogoutFlag);
    }
}

function CheckIfMasterAccount(hLoggedInUserNId) {
    var RetVal;
    var CallbackPageName;
    if (document.URL.indexOf("articles/") != -1) {
        CallbackPageName = "../libraries/aspx/Callback.aspx";
    }
    else {
        CallbackPageName = "Callback.aspx";
    }
    var InputParam = hLoggedInUserNId; // 
    var htmlResp = di_jq.ajax({
        type: "POST",
        url: CallbackPageName,
        data: "callback=1013&param1=" + InputParam,
        async: false,
        success: function (data) {
            try {
                RetVal = data;
            }
            catch (ex) {
                alert("Error : " + ex.message);
            }
        },
        error: function () {
            

        },
        cache: false
    });
    return RetVal;
}

function CheckIfSiteAdminLoggedInShowSpecificTabs() {
    var RetVal;
    var InputParam;
    var CallbackPageName;
    if (document.URL.indexOf("articles/") != -1) {
        InputParam = z('hLoggedInUserNId').value;
        CallbackPageName = "../libraries/aspx/Callback.aspx";
    }
    else if (z('hLoggedInUserNId').value != '') {
        if (z('hLoggedInUserNId').value.split('|').length > 1) {
            InputParam = z('hLoggedInUserNId').value.split('|')[0];
        }
        else {
            InputParam = z('hLoggedInUserNId').value;
        }
        CallbackPageName = "Callback.aspx";
    }


    if (InputParam != '' && InputParam != undefined) {
        // var InputParam = z('hLoggedInUserNId').value.split('|')[0];
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallbackPageName,
            data: "callback=129&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    RetVal = data;
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
    else {
        RetVal = 'false'
    }
    return RetVal;
}

function CheckIfSiteAdminLoggedIn(hLoggedInUserNId) {
    var RetVal;

    if (hLoggedInUserNId != undefined && hLoggedInUserNId != null) {
        var InputParam = hLoggedInUserNId;
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: "Callback.aspx",
            data: "callback=129&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    RetVal = data;
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
    else {
        RetVal = 'false'
    }
    return RetVal;
}

function GoToRegistry() {
    // PostData(z('hiddenFrmId').value, 'RegStructuralMD.aspx', z('hiddenPostMethod').value);
    PostData(z('hiddenFrmId').value, 'RegStructure.aspx', z('hiddenPostMethod').value);
}

function GoToDataQuery() {
    PostData(z('hiddenFrmId').value, 'RegDataQuery.aspx', z('hiddenPostMethod').value);
}

/* function to get absolute path */
function getAbsURL(sFolder) {
    var current_page_virtual_path = "";
    var url = window.location.href;
    var url_parts;

    if (url.indexOf("libraries") > -1) {
        url_parts = url.split("libraries");
    }
    else if (url.indexOf("articles/") > -1) {
        url_parts = url.split("articles");
    }
    else {
        url_parts = url.split("Index.htm");
    }

    current_page_virtual_path = url_parts[0] + sFolder + "/";
    return current_page_virtual_path;
}

/* function to get location query string */
function getQueryStr(qSName) {
    var url = window.location.search.substring(1);
    var url = url.split("&");
    for (var i = 0; i < url.length; i++) {
        var strVal = url[i].split("=");
        if (strVal[0] == qSName) {
            return strVal[1];
        }
    }
}

/* function to craete cookie */
function createCookie(name, value) {
    name = name + CookiePostfixStr;
    var days = 7;
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    }
    else var expires = "";
    document.cookie = name + "=" + value + expires + "; path=/";
}

/* function to read cookie */
function readCookie(name) {
    name = name + CookiePostfixStr;
    var nameEQ = name + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
    }
    return null;
}

/* function to get HTTP request */
function getXMLHTTPRequest() {
    if (window.XMLHttpRequest) {
        return new window.XMLHttpRequest();
    }
    else {
        for (i = 0; i < xmlVersions.length; i++) {
            try {
                return new ActiveXObject(xmlVersions[i]);
            }
            catch (ex) {
            }
        }
    }
}

function HotLanguageFunction() {
    var selected_hlngcode;
    var StockPath = "";
    var InputParam;
    var isRTL;

    try {

        try {
            selected_hlngcode = di_lc_getSelectedLanguage();

        }
        catch (err) { }

        if (selected_hlngcode == undefined) {
            selected_hlngcode = readCookie('hlngcode');
        }
        InputParam = readCookie('hdbnid');
        InputParam += ParamDelimiter + selected_hlngcode;
        di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: "callback=66&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    createCookie('hlngcodedb', data);

                    try {
                        // set site language - hidden input
                        z('hlngcode').value = selected_hlngcode;

                        // set database language
                        z("hlngcodedb").value = data;
                    }
                    catch (err) { }
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

        createCookie('hlngcode', selected_hlngcode);

        if (document.URL.indexOf("articles/") != -1) {
            location.reload();
        }

        else if (GetStandalonRegistryStatus().toLowerCase() != "true") {
            window.location.href = "home.aspx";
        }
        else {
            //eval(z('aHeader').href.replace("home.aspx", "RegStructuralMD.aspx"));
            eval(z('aHeader').href.replace("home.aspx", "RegStructure.aspx"));
        }
    }
    catch (err) { }
}


/* function to get language code */
function onLanguageSelection(isCallFromAdminPanel) {
    var selected_hlngcode;
    var StockPath = "";
    var InputParam;
    var isRTL;

    try {
        if (readCookie('hlngcode') == null) {
            selected_hlngcode = z('hlngcode').value;
        }
        else selected_hlngcode = readCookie('hlngcode');
        if (selected_hlngcode == "ar") {
            isRTL = true;
        }

        if (isRTL) {
            StockPath = getAbsURL('stock')
            //removejscssfile(StockPath + "themes/default/css/style.css", "css");
            loadjscssfile(StockPath + "themes/default/css/style_rtl.css", "css");
            if ((screen.width >= 1152) && (screen.width < 1280)) {
                loadjscssfile(StockPath + "themes/default/css/v_slider/style1152_rtl.css", "css");
            }
            if ((screen.width >= 1280) && (screen.width < 1360)) {
                loadjscssfile(StockPath + "themes/default/css/v_slider/style1280_rtl.css", "css");
            }
            if (screen.width >= 1360) {
                loadjscssfile(StockPath + "themes/default/css/v_slider/style1366_rtl.css", "css");
            }

            try {
                z("pp_bttn").style.left = ((VisibleSlidersCount * 17) + 20) + "px";
                //di_jq("#bg_image_rtl").attr("src", "../../stock/themes/default/images/ar/toppanel_background.jpg");

            }
            catch (err) { }
        }
        else {
            removejscssfile(StockPath + "themes/default/css/style_rtl.css", "css");
            removejscssfile(StockPath + "themes/default/css/v_slider/style1152_rtl.css", "css");
            removejscssfile(StockPath + "themes/default/css/v_slider/style1280_rtl.css", "css");
            removejscssfile(StockPath + "themes/default/css/v_slider/style1366_rtl.css", "css");
            try {
                di_jq('#pp_bttn').css('left', '0px');
            }
            catch (err) { }
        }

        LoadLanguage(selected_hlngcode);
        LoadLanguage(selected_hlngcode, "HomeMaster");
        LoadLanguage(selected_hlngcode, "RegistryMaster");
    }
    catch (err) { }
}
function RefreshOrRedirect() {
    var PageName = '';
    var tmpNowPageName = getCurrentPageName();
    tmpNowPageName = tmpNowPageName.substring(0, tmpNowPageName.length - 5);
    PageName = tmpNowPageName;
}

function LoadLanguage(LangCode, masterPageName) {
    var PageName = '';
    if (masterPageName == undefined) {
        var tmpNowPageName = getCurrentPageName();
        tmpNowPageName = tmpNowPageName.substring(0, tmpNowPageName.length - 5);
        PageName = tmpNowPageName;
    }
    else {
        tmpNowPageName = masterPageName;
    }
    tmpNowPageName += ".xml";

    //Create dynamically XmlPage Url
    var XmlPageURL = getAbsURL('stock') + "language/" + LangCode + "/" + tmpNowPageName + "?v=" + AppVersion;
    di_jq.ajax({
        type: "GET",
        url: XmlPageURL,
        dataType: "xml",       
        async: false,
        timeout: 5000,
        error: function (request, error) {
            //alert('Error reading language XML is : ' + error); 
        },
        success: function (xml) {
            path = "/Language/lng/@val | /Language/lng/@id | /Language/lng/@prop";
            // code for IE
            if (window.ActiveXObject) {
                
                var nodes = xml.selectNodes(path);
                for (i = 0; i < nodes.length; i++) {
                    var id, prop, val;
                    var tmpString = nodes[i].childNodes[0].nodeValue;
                    switch (nodes[i].nodeName) {
                        case "id":
                            id = tmpString;
                            break;
                        case "prop":
                            prop = tmpString;
                            break;
                        case "val":
                            val = tmpString.trim();
                            break;
                    }
                    if ((i + 1) % 3 == 0) {
                        try {
                            di_jq("#" + id).attr(prop, val);
                        }
                        catch (err) {
                            var msg = err;
                        }
                        id = ""; prop = ""; val = "";
                    }
                }
            }
            // code for Chrome
            else if (GetRunningBrowserName() == "Chrome" || GetRunningBrowserName() == "Safari") {
                var pathVAL = "/Language/lng/@val";
                var pathID = "/Language/lng/@id";
                var pathPROP = "/Language/lng/@prop";

                var nodesVAL = xml.evaluate(path, xml, null, XPathResult.ANY_TYPE, null);
                var nodesID = xml.evaluate(pathID, xml, null, XPathResult.ANY_TYPE, null);
                var nodesPROP = xml.evaluate(pathPROP, xml, null, XPathResult.ANY_TYPE, null);

                var resultVAL = nodesVAL.iterateNext();
                var resultID = nodesID.iterateNext();
                var resultPROP = nodesPROP.iterateNext();

                var i = 0;
                while (resultVAL) {
                    var id, prop, val;

                    try {
                        val = resultVAL.childNodes[0].nodeValue.trim();
                        id = resultID.childNodes[0].nodeValue;
                        prop = resultPROP.childNodes[0].nodeValue;

                        if (di_jq("#" + id).length > 0) {
                            di_jq("#" + id).attr(prop, val);
                        }
                    }
                    catch (err) {
                        var errText = err;
                    }

                    resultVAL = nodesVAL.iterateNext();
                    resultID = nodesID.iterateNext();
                    resultPROP = nodesPROP.iterateNext();

                }
            }

            // code for Mozilla, Firefox, Opera, etc.
            else if (document.implementation && document.implementation.createDocument) {
                var nodes = xml.evaluate(path, xml, null, XPathResult.ANY_TYPE, null);
                var result = nodes.iterateNext();

                var i = 0;
                while (result) {
                    var id, prop, val;
                    var tmpString = result.value; //.childNodes[0].nodeValue;
                    if (navigator.appVersion < 14) {
                        tmpString = result.childNodes[0].nodeValue;
                    }
                    else {
                        tmpString = result.value; //.childNodes[0].nodeValue;
                    }
                    switch (result.nodeName) {
                        case "id":
                            id = tmpString;
                            break;
                        case "prop":
                            prop = tmpString;
                            break;
                        case "val":
                            val = tmpString; //.trim();
                            break;
                    }

                    if ((i + 1) % 3 == 0) {
                        di_jq("#" + id).attr(prop, val);
                        id = ""; prop = ""; val = "";
                        i = 0;
                    }
                    else {
                        i++;
                    }
                    //alert(result.childNodes[0].nodeValue);
                    result = nodes.iterateNext();

                }
            }
        }
    });

    // Load Indicator/Area Component

    if (PageName.toLowerCase() == 'home') {
        // ************************************************
        // Load QDS Component
        // ************************************************

        var TopicBlockDetails = '';
        var AreaBlockDetails = '';

        try {
            if (readCookie('QdsBoxTopics') != null) TopicBlockDetails = decodeURIComponent(readCookie('QdsBoxTopics'));
            if (readCookie('QdsBoxAreas') != null) AreaBlockDetails = decodeURIComponent(readCookie('QdsBoxAreas'));
            var strQueryAreasIndicatorsJson = getQueryStr('jsonAreasTopics');
            var showelements = getQueryStr('showqdsonly');
            if (showelements != null && showelements != '') {
                if (showelements == "y") {
                    IsShowElements = false;                                 
                    $(".langAdvancedSearch").hide();
                    $(".bttn_splt_grd_mid").hide();                    
                    $(".bttn_splt_grd_lft").hide();
                    $(".bttn_splt_grd_rgt").hide();
                    $(".MainContent").css('padding-top','0px');                                      
                    $(".clear").hide();
                    $(".hm_vid_pnl_contnr").hide();
                }
            }


            if (strQueryAreasIndicatorsJson != null && strQueryAreasIndicatorsJson != '') {
                strQueryAreasIndicatorsJson = decodeURI(strQueryAreasIndicatorsJson);
                TopicBlockDetails = '';
                AreaBlockDetails = '';

                var jsonQueryAreasIndicators = di_jq.parseJSON(strQueryAreasIndicatorsJson);

                if (jsonQueryAreasIndicators.i != '') {
                    var arrTopics = jsonQueryAreasIndicators.i.split(ParamDelimiter);
                    var arrTopicNIds = jsonQueryAreasIndicators.i_n.split(ParamDelimiter);
                    for (var i = 0; i < arrTopics.length; i++) {
                        TopicBlockDetails += di_jq.trim(arrTopicNIds[i]) + '~~' + di_jq.trim(arrTopics[i]) + '||';
                    }
                    TopicBlockDetails = TopicBlockDetails.substr(TopicBlockDetails, TopicBlockDetails.length - 2);
                }
                if (jsonQueryAreasIndicators.a != '') {
                    var arrAreas = jsonQueryAreasIndicators.a.split(ParamDelimiter);
                    var arrAreaNIds = jsonQueryAreasIndicators.a_n.split(ParamDelimiter);
                    for (var i = 0; i < arrAreas.length; i++) {
                        AreaBlockDetails += di_jq.trim(arrAreaNIds[i]) + '~~' + di_jq.trim(arrAreas[i]) + '||';
                    }
                    AreaBlockDetails = AreaBlockDetails.substr(AreaBlockDetails, AreaBlockDetails.length - 2);
                }
                createCookie('QdsCart', '');
            }
            else if (z('isQdsToBeRendered').value == 'false') {
                TopicBlockDetails = '';
                AreaBlockDetails = '';
                createCookie('QdsBoxAreas', AreaBlockDetails);
                createCookie('QdsBoxTopics', TopicBlockDetails);
                createCookie('QdsCart', '');
            }

            ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div', z('hdbnid').value, z('hlngcodedb').value, z('langQdsIndCaption').value, z('langQdsAreaCaption').value, '100%', '100px', 'qdsHotSelection', '', '', TopicBlockDetails, AreaBlockDetails, false, z('langQdsSerchBtnText').value, z('langQdsAreaBlockText').value, 'qdsHotClearance');

            if (TopicBlockDetails != '' || AreaBlockDetails != '') {
                qdsHotSelection(false);
            }

            $(".ellipsis").ellipsis(); //ADDED THIS LINE FOR ELLIPSIS FUNCTION   

            //Set slider HTML page on basis of current selected interface language
            if ($("#slider1").length > 0) {
                //SetSliderHtmlPageOnCurrentLanguageBasis();
            }

            SetShowSliderStatus();

            // Get and set current db info into div
            GetDatabaseInfoHTML(z('hdbnid').value, z('hlngcodedb').value, "divDatabaseFullInfo");
     

        }
        catch (err) { }

    }
    else if (PageName.toLowerCase() == 'gallery') {
        ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div', z('hdbnid').value, z('hlngcodedb').value, z('langQdsIndCaption').value, z('langQdsAreaCaption').value, '100%', '100%', 'GetQDSResult', "", "", "", "", false, z('langQdsSerchBtnText').value, z('langQdsAreaBlockText').value);
    }
    else if (PageName.toLowerCase() == 'catalog') {
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

            di_jq("#spanPageTitle").html(CatalogText);
        }
        catch (err) { }

        try {
            var IsCountriesDisplay = "block";

            if (IsGlobalAdaptation.toLowerCase() == 'false') {
                IsCountriesDisplay = "none";
            }

            di_jq("#liCountries").css("display", IsCountriesDisplay);

        }
        catch (err) { }

    }
    else if (PageName.toLowerCase() == 'dataview') {
        try {
            changeHeaderText();
            changeFeaturesText();
            changeHideColumnsText();
        }
        catch (err) { }
    }
    else if (masterPageName != undefined && masterPageName.toLowerCase() == 'homemaster') {
        di_jq("#aCatalog").html(CatalogText);
    }
    else if ((PageName.toLowerCase() == 'regstructure') || (PageName.toLowerCase() == 'regstructuralmd') || (PageName.toLowerCase() == 'regstructuralmd-metadata') || (PageName.toLowerCase() == 'regproviders')) {
        SetHiddenFieldValues();
    }
    else if (PageName.toLowerCase() == 'regpublishdata') {
        LanguageHandlingForAlerts();
    }
    else if (masterPageName != undefined && masterPageName.toLowerCase() == 'registrymaster') {
        if (z('divRegistryMaster') != null) {
            z('divRegistryMaster').style.display = "block";
        }
    }
    else if (PageName.toLowerCase() == 'about') {
        ChangeHTM();
    }
}

String.prototype.trim = function () {
    return this.replace(/^\s+|\s+$/g, "");
}


function SetSliderHtmlPageOnCurrentLanguageBasis() {
    var IframeSrc;
    var IframeSrcArr;
    var IframeSrcArrLength = -1;

    try {
        IframeSrc = $("#slider1")[0].getElementsByTagName("iframe")[0].src;

        if (IframeSrc != "") {
            IframeSrcArr = IframeSrc.split("/");
            IframeSrcArrLength = IframeSrcArr.length;

            if (IframeSrcArr.length > 0) {

                if (readCookie('hlngcode') == null || readCookie('hlngcode') == "" || readCookie('hlngcode') == undefined) {
                    readCookie('hlngcode') = "en";
                }

                IframeSrcArr[IframeSrcArrLength - 2] = readCookie('hlngcode');

                IframeSrc = IframeSrcArr.join("/");

                $("#slider1")[0].getElementsByTagName("iframe")[0].src = IframeSrc;
            }
        }
    }
    catch (err) { }
}


/* 
callback function that is called by Indicator component for getting ius nids where data exist. 
InputParam: iu nids (indicator_unit nids comma separated)
OutPut: iu nids where data exist comma separated or false
*/
function get_ind_where_data_exists(iu_nids) {
    var returnStr = '';
    try {
        // Use Area NIds from selected items in component, not from hidden field. Since hidden fields are modified by "Add to Cart" buttons also.
        var area_nids = ''; // z('hselarea').value;
        if ((area_nids == '' || area_nids == null) && AreaCompObj != '') {
            var area_data = AreaCompObj.getSelectedAreas();
            area_data = area_data.split("||{~~}||");
            if (area_data[0] != '') area_nids = area_data[0];
        }

        if (area_nids == '') {
            return 'falseWOArea';
        }

        var InputParam = area_nids; 				        // AreaNids
        InputParam += ParamDelimiter + iu_nids; 	            // IU_Nids
        InputParam += ParamDelimiter + z('hdbnid').value; 	// DbNid
        InputParam += ParamDelimiter + z('hlngcode').value; 	// UI language

        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: "callback=1&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    returnStr = data;
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
    catch (ex) { }

    return returnStr;
}

/* 
callback function that is called by Area component for getting area nids where data exist. 
InputParam: parent_id (Area NId or Area Level. Area nid will be numeric and level will be such as L_2)
OutPut: Area NIds where data exist comma separated or false
*/
function get_area_where_data_exists(parent_id) {
    var returnStr = '';
    try {
        // Use IUS NIds from selected items in component, not from hidden field. Since hidden fields are modified by "Add to Cart" buttons also.
        var ius_nid = ''; //  z('hselind').value;
        if ((ius_nid == '' || ius_nid == null) && IndCompObj != '') {
            var ius_data = IndCompObj.getSelectedData();
            ius_data = ius_data.split("||{~~}||");
            if (ius_data[0] != '') ius_nid = ius_data[0];
        }
        var area_id = parent_id;

        if (ius_nid == '') {
            return 'false';
        }

        var InputParam = area_id;
        InputParam += ParamDelimiter + ius_nid;
        InputParam += ParamDelimiter + z('hdbnid').value;
        InputParam += ParamDelimiter + z('hlngcode').value;

        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: "callback=2&param1=" + InputParam,
            async: false,
            success: function (data) {
                try {
                    returnStr = data;
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
    catch (ex) { }

    return returnStr;
}

/* callback hot selection function on clicking on Dataabse Component "Select" button */
function getSelectedDatabaseId() {
    var selected_dbnid = di_db_getSelectedDB();
    if (selected_dbnid) {
        var currentLocation = window.location.href.split("?");
        if (currentLocation[1] && currentLocation[1].indexOf("hdbnid") == -1) {
            window.location = currentLocation[0] + '?' + currentLocation[1] + '&hdbnid=' + selected_dbnid;
        }
        else if (currentLocation[1]) {
            var newQString = currentLocation[1].replace('hdbnid=' + getQueryStr('hdbnid'), 'hdbnid=' + selected_dbnid);
            window.location = currentLocation[0] + '?' + newQString;
        }
        else {
            window.location = currentLocation[0] + '?hdbnid=' + selected_dbnid;
        }
    }
    return;
}


/* **************************** */
/* Component creation functions */
/* **************************** */

// function to show language component
function ShowLanguageComponent(xmlDataBasePath, divId, langCode) { //di_lc_setLanguage

    var LanguageFile = xmlDataBasePath + 'language.xml?v=' + AppVersion;
    //    var tmpSpanLanguage = ce('span');
    //    tmpSpanLanguage.innerHTML = "English";
    //    tmpSpanLanguage.style.cursor = 'pointer';
    //    tmpSpanLanguage.id = "tmpSpanLanguage";
    //    z(divId).appendChild(tmpSpanLanguage);
    //    tmpSpanLanguage.onclick = function (event) {
    //        var divLanguageList = z('divLanguageList');
    //        LiLanguageArr = [];
    //        while (divLanguageList.hasChildNodes()) {
    //            divLanguageList.removeChild(divLanguageList.lastChild);
    //        }
    //        di_jq.ajax({
    //            type: "GET",
    //            url: LanguageFile,
    //            dataType: "xml",
    //            async: false,
    //            timeout: 5000,
    //            error: function (request, error) {
    //            },
    //            success: function (xml) {
    //                di_jq(parseXml(xml)).find('lng').each(function () {
    //                    var liLanguage = ce('li');
    //                    var LiId = "li_" + di_jq(this).attr("code");
    //                    liLanguage.id = LiId;
    //                    LiLanguageArr.push(LiId);
    //                    var aLanguage = ce('a');
    //                    aLanguage.innerHTML = di_jq(this).attr("n");
    //                    aLanguage.id = di_jq(this).attr("code");
    //                    //                    aLanguage.rel = AllDimensions[d];
    //                    aLanguage.href = 'javascript:void(0);';
    //                    aLanguage.onclick = function (event) {
    //                        HotLanguageFunction();
    //                    }
    //                    liLanguage.appendChild(aLanguage);
    //                    divLanguageList.appendChild(liLanguage);
    //                });

    //            }
    //        });
    //        SetCalloutPosition(event, z('divCallout_lang'), 20, 0);
    //    }



    /* **************************************************************/
    /* Calling function to draw Language component */
    /* **************************************************************/
    di_lc_drawLanguageSelectionComponent(
	divId, 									// id of the div in which the component HTML will be drawn.
	LanguageFile, 				// Database File Path
	langCode, 										// Interface Language
	'HotLanguageFunction()'								// Hot function selection
	);
}

// function to show database component
function InitializeDatabaseComponent(xmlDataBasePath, divId, dataBaseID, langCode, width, height, multiSelect, componentOkMethodName, componentCloseMethodName) {

    var DbFileNameWithPath = xmlDataBasePath + 'db.xml' + "?v=" + AppVersion;

    /* **************************************************************/
    /* Calling function to draw database selection*/
    /* **************************************************************/
    di_db_drawDbSelectionControl(
	divId, 									// id of the div in which the component HTML will be drawn.
	'Select Database', 						// Title TODO hardcoded
	width, 									// Width
	height, 									// Height
	DbFileNameWithPath, 				// Database File Path
	'Select Database', 						//Caption
	'Search', 								//Text for Search Box
	'Select', 								//Select Button
	'In Use', 								//In Use Button
	dataBaseID, 								//Database Id
	'Indicators', 							// Indicators Text
	'Areas', 								// Areas Text
	'Sources', 								// Sources Text
	'Data Values', 							// Data Values Text
	'updated on',                            // Last modified date
	'getSelectedDatabaseId()', 				// function name which will be called while clicking on select button
	langCode, 								// Application UI lng code
	multiSelect, 							// Multiple selection
	'OK', 									// label of OK (use on button)
	'Cancel', 								// label of Cancel (use on button)
	componentOkMethodName,                      // Parent function name to ok component
	componentCloseMethodName                    // Parent function name to close component
	);
    // destroy var
    delete codeListPath;

} // end database component


/* function to show quick search data component */
function ShowQuickDSComponent(xmlDataBasePath, divId, dataBaseID, db_langCode, indTitle, areaTitle, width, height, HotSelectionFunctionName, indSearchText, areaSearchText, indBlockDetails, areaBlockDetils, advanceSearch, searchBtnText, areaBlockText, clearAllClickFunctionName) {
    /* **************************************************************/
    /* Calling function to draw Quick Data Search component */
    /* **************************************************************/

    var codeListPath = xmlDataBasePath + 'data/' + dataBaseID + '/ds/' + db_langCode + '/';
    var search_img = xmlDataBasePath + "themes/default/images/search_button.gif";

    di_draw_quick_data_search(
	divId, 							                    // id of the div in which the component HTML will be drawn.	
	indTitle, 						                        // Caption of indicator selector
	areaTitle, 						                        // Caption of area selection
	width, 												// Width
	height,                                                 //Height
	codeListPath + 'ius/iussearch',                         // Indicator codelist URL
	codeListPath + 'area/areasearch',                     // Area coldelists foder path
	'1', 												// Indicator display type - 1:Indicator, 2:Indicator and Unit
	'1', 												// Indicator value type - 1:Indicator NId, 2:Indicator NId and Unit NId
	'1', 												// Area display type - 1:Area Name, 2:Area Name and ID
	'1', 												// Area value type - 1:Area NID, 2:Area NID and ID
	HotSelectionFunctionName, 						    // Callback function name on search button click
	'single',  											// Selection mode - single/multiple	 
	z('langQdsIndDefText').value,              // Indicator defalut text
	z('langQdsAreaDefText').value,         // Area defalut text
	search_img,                                             // Search button image
	indSearchText,                                          // Indicator search text
	areaSearchText,                                         // Area search text 
	indBlockDetails,                                        // Indicator block details
	areaBlockDetils,                                         // Area block details
	advanceSearch, 										// Advance search - true/false
	'', 													// suggestion list (blank-for all, NONE-for indicators only, IC_SC,IC_GL-for selected)
    searchBtnText,                                          // Search button text
	areaBlockText,                                          // Area isblock text
    clearAllClickFunctionName                               // Callback function name on clear all cross image click
	);

} // end Quick Data Search component


// INDICATOR SELECTION
function ShowIndicatorComponent(xmlDataBasePath, divId, dataBaseID, db_langCode, jsonData, width, height, subgroupCount, default_mode, allow_sg_sel, ondemand) {
    di_jq('#indicator_div_popup').html("");

    var codeListPath = xmlDataBasePath + 'data/' + dataBaseID + '/ds/' + db_langCode; // + "/";
    if (default_mode == '' || default_mode == undefined) default_mode = 'list';

    /* Calling function to draw indicator selection list */
    /*di_draw_indicator_selection_list(
    '',												// Caption
    width,											// Width
    height,											// Height
    codeListPath,									// Indicator codelist path
    divId,											// Div id of the div in which the component HTML will be drawn.
    true,										    // Multiselect - true / false
    true,											// Allow select all - true / false
    allow_sg_sel,									// Allow subgroup selection - true / false
    true,											// Show view selection panel - true / false
    true,											// Show metadata icon - true / false
    ondemand,										// Load classifications on demand - true / false
    'both',											// Display mode - tree / list / both
    'nid',											// Output type - nid/name/nid_name (will return i~u nid or name)
    z('langHiddenTree').value,						// Label of tree view
    z('langHiddenAtoZ').value,						// Label of list view
    z('langHiddenSearch').value,					// Label of search help text
    z('langHiddenViewselections').value,			// Label of view selection
    z('langHiddenSector').value,					// Label of sector
    z('langHiddenGoal').value,						// Label of Goal
    z('langHiddenSource').value,					// Label of data sources
    z('langHiddenInstitution').value,				// Label of institution
    z('langHiddenTheme').value,						// Label of theme
    z('langHiddenConvention').value,				// label of convention
    z('langHiddenConceptualFramework').value,		// label of conceptual framework
    z('langHiddenselectsubgroup').value,			// label of select subgroup
    z('langHiddenSelectAll').value,					// label of select all
    z('langHiddenShowwheredataexist').value,		// label of where data exist
    z('langHiddenOK').value,						// label of Ok (use on button)
    z('langHiddenClear').value,						// label of clear (use on button)
    z('langHiddenClose').value,						// label of close (use on button)
    z('langHiddenSubgroupDimensions').value,		// label of subgroup dimentsion (use on subgroup popup)
    'hot_selection_func("Testing")',				// callback function on hover over event
    'get_ind_where_data_exists',					// callback function for Show where data exist
    jsonData,										// json data for selections
    subgroupCount,									// number of count for subgroup list
    z('langHiddenMore').value,						// label of more link on subgroup list
    z('langHiddenMetadata').value,					// label of metadata link list
    default_mode									// default mode list/tree_<default ic type SC, GL etc> (for 'both' case)
    );*/


    var iopts = {
        title: '', 													// title
        width: width, 												// width
        height: height, 												// height
        codelistPath: codeListPath, 									// xml path
        mode: 'both', 												// mode both/tree/list
        defaultMode: default_mode, 									// default mode tree_<sector code>/list
        multiple: true, 												// allow multiple true/false
        selectAll: true, 											// allow select all true/false
        sgSelection: allow_sg_sel, 									// allow subgroup selection
        sgLimit: subgroupCount, 										// list count for default subgroup list true/false
        showViewSel: true, 											// show view selection true/false
        showMetadata: true, 											// show metadata true/false
        collapsed: ondemand, 										// load tree ondemand true/false
        outputFormat: 'nid', 										// output format
        jsonSelection: jsonData, 									// json data for persisting
        sectors: {														// sector list labels
            SC: z('langHiddenSector').value,
            GL: z('langHiddenGoal').value,
            SR: z('langHiddenSource').value,
            IT: z('langHiddenInstitution').value,
            TH: z('langHiddenTheme').value,
            CN: z('langHiddenConvention').value,
            CF: z('langHiddenConceptualFramework').value
        },
        callbacks: {													// callbacks
            hotSel: 'hot_selection_func("Testing")', 			// hot selection function if multiple false
            whereDataExist: 'get_ind_where_data_exists'			// function where data exist
        },
        labels: {														// labels
            selectAll: z('langHiddenSelectAll').value,
            byList: z('langHiddenAlphabetically').value,
            byTree: z('langHiddenTree').value,
            search: z('langHiddenSearch').value,
            viewSel: z('langHiddenViewselections').value,
            metadata: z('langHiddenMetadata').value,
            dataExist: z('langHiddenShowwheredataexist').value,
            clear: z('langHiddenClear').value,
            close: z('langHiddenClose').value,
            ok: z('langHiddenOK').value,
            cancel: 'Cancel',
            more: z('langHiddenMore').value,
            sgDim: z('langHiddenSubgroupDimensions').value
        }
    };

    IndCompObj = new di_drawIndicatorList(divId, iopts);

} // end INDICATOR SELECTION

function getCurrentPageName() {
    var sPath = window.location.pathname;
    return sPath.substring(sPath.lastIndexOf('/') + 1);
}
// AREA SELECTION
function ShowAreaComponent(xmlDataBasePath, divId, dataBaseID, db_langCode, jsonData, width, height, default_mode) {
    di_jq('#area_div_popup').html("");

    var codeListPath = xmlDataBasePath + 'data/' + dataBaseID + '/ds/' + db_langCode + "/";
    if (default_mode == '' || default_mode == undefined) default_mode = 'tree';

    /* **************************************************************/
    /* Calling function to draw Area selection list */
    /* **************************************************************/

    /*di_draw_area_selection_list(
    '',																// Title Caption  
    false,															// show close icon on right of the title box
    width,															// Width
    height,															// Height
    codeListPath + 'area',											// Area Codelist Path
    codeListPath + 'metadata/',										// Metadata folder path
    true,															// Multiselect - true/false
    'both',															// Display mode - both, tree, list
    false,                                                           // Show metadata icon - true/false
    '',																// Country Icon URL
    'nid',															// output format - aid, nid, name, nid_name, aid_name, aid_nid_name
    true,                                                           // Show area id - true/false
    true,															// Quick Selection - Provide options for quick selection true/false
    'HRZ',                                                          // Quick Selection Arrangement Mode - HRZ|VERT
    divId,															// id of the div in which the component HTML will be drawn.
    false,															// true/false to show footer buttons (OK and Cancel)
    true,															// Expandded or Collapsed nodes
    true,                                                           // Show view selection panel - true/false
    'hot_selection_func("Testing")', 								// callback function on hover over event     
    jsonData,														// area nids for persist selections
    'get_area_where_data_exists',									// callback function for where data exist
    z('langHiddenSelectAll').value,									// Label of select all
    z('langHiddenSelectarealevel').value,							// Label of select area level
    z('langHiddenByTree').value,									// Label of by tree
    z('langHiddenListAtoZ').value,									// Label of by list
    z('langHiddenByMap').value,										// Label of by map
    z('langHiddenSearch').value,									// Label of search
    z('langHiddenViewselections').value,							// Label of view selections
    z('langHiddenMetadata').value,									// Label of metadata
    z('langHiddenShowwheredataexist').value,						// Label of show where data exist
    z('langHiddenClear').value,										// Label of clear button
    z('langHiddenClose').value,										// Label of close button	
    z('langHiddenFirst').value,										// Label of first link in paging
    z('langHiddenBack').value,										// Label of back link in paging
    z('langHiddenNext').value,										// Label of next link in paging
    z('langHiddenLast').value,										// Label of last link in paging
    z('langHiddenPage').value,										// Label of page in paging dropdown
    default_mode,													// default display mode
    'MapSelector.aspx?dbnid='+dataBaseID+'&lngcode='+db_langCode	// Callback URL for Area Map (leave blank hide "By Map" tab)
    );*/

    var opts = {
        title: '', 											// Title caption  
        iconClose: false, 									// Show close icon on right of the title box
        width: width, 										// Width
        height: height, 										// Height
        codelistPath: codeListPath + 'area', 				// Area Codelist Path
        metadataPath: codeListPath + 'metadata/', 			// Metadata folder path
        flagIconPath: '', 									// Country Icon URL
        multiple: true, 										// Multiselect - true/false
        mode: 'both', 										// Display mode - both, tree, list
        defaultMode: default_mode, 							// default display mode tree/list_level
        outputFormat: 'nid', 								// aid, nid, name, nid_name, aid_name, aid_nid_name
        showAreaId: true, 									// Show Area ID after area name. true/false
        quickSel: true, 										// Provide options for quick selection true/false
        quickSelMode: 'HRZ', 								// Quick Selection Arrangement Mode - HRZ|VERT
        showFooter: false, 									// Show footer with OK|Cancel buttons,
        showViewSel: true, 									// Show view selection panel
        callbacks: {
            hotSel: 'hot_selection_func("Testing")', 	// hot selection function if multiple false
            whereDataExist: 'get_area_where_data_exists', // function where data exist
            areaMap: ''										// Callback URl for Area Map. (keep null for hiding)
        }, 											// callback functions
        jsonSelection: jsonData, 							// JSON format to persist area selection default
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

    AreaCompObj = new di_drawAreaSelectionList(divId, opts);

} // end area component


/* function to display alert message */
function alertMessage(message) {
    if (message != '') {
        alert(message);
    }
}

/* function to get iusNids from iuNids */
function di_get_ius_from_iu(iu_list, codelist_url) {
    var di_tmp_all_isunid = new Array();
    var iu_list_arr;
    var di_iu;
    var FileNameWithPath = "";

    try {
        iu_list_arr = iu_list.split(",");

        for (var i = 0; i < iu_list_arr.length; i++) {
            di_iu = iu_list_arr[i];

            FileNameWithPath = codelist_url + "/ius/ius_" + di_iu + ".xml" + "?v=" + AppVersion;

            di_jq.ajax({
                type: "GET",
                url: FileNameWithPath,
                dataType: "xml",
                async: false,
                success: function (xml) {
                    var di_iu_brk = di_iu.split("_");
                    di_jq(xml).find('iu[inid="' + di_iu_brk[0] + '"]').each(function () {
                        var di_defisunid = di_jq(this).attr("defiusnid");
                        if (di_defisunid != '' && di_defisunid != null) {
                            di_tmp_all_isunid.push(di_defisunid);
                        }
                        else {
                            di_jq(xml).find('ius').each(function () {
                                di_tmp_all_isunid.push(di_jq(this).attr("iusnid"));
                            });
                        }
                    });
                }
            });
        }
    }
    catch (err) { }

    return di_tmp_all_isunid;
}

if (!Array.indexOf) {
    Array.prototype.indexOf = function (obj) {
        for (var i = 0; i < this.length; i++) {
            if (this[i] == obj) {
                return i;
            }
        }
        return -1;
    }
}


Array.prototype.clone = function () {
    var arr1 = new Array();
    for (var property in this) {
        arr1[property] = typeof (this[property]) == 'object' ? this[property].clone() : this[property]
    }
    return arr1;
}


String.prototype.encode = function () {
    return this.replace(/([^x01-x7E])/g, function (s) { return "&#" + s.charCodeAt(0) + ";" });
}


// Function for CallOut Div showing and positioning. 
// NOTE : Make sure to allocate Textual div's name to start samely as divIND and end with "Text"

function ShowCallout(divID, calloutText, evt) {
    var divCallout = z(divID);
    evt = evt || window.event;
    z(divID + "Text").innerHTML = calloutText;
    z(divID + "Text").style.wordWrap = "break-word";
    z(divID).style.width = "auto";
    SetCalloutPosition(evt, divCallout, 20, 0);
    divCallout.style.display = "block";
}


function ShowCalloutForMap(divID, calloutText, evt) {
    var divCallout = z(divID);
    evt = evt || window.event;
    z(divID + "Text").innerHTML = calloutText;
    z(divID).style.width = "332px";
    z(divID).style.align = "justify";
    z(divID + "Text").style.wordWrap = "break-word";
    SetCalloutPosition(evt, divCallout, 20, 0);
    divCallout.style.display = "block";
}
function ShowCalloutMenu(divID, calloutText, evt, aID) {
    var divCallout = z(divID);
    z(divID + "Text").innerHTML = calloutText;
    divCallout.style.display = "block";
    if (isGlobalAdap == "local") {
        divCallout.style.top = "69px";
    }
    else {
        divCallout.style.top = "39px";
    }
}

function HideCallout(divID) {

    if (divID == undefined) divID = 'divCallout';
    z(divID).style.display = "none";
}

function SetCalloutPosition(sEvent, divCallOut, marginLeft, marginTop, x, y) {
    try {
        sEvent = sEvent || window.event;
        if (divCallOut) {
            // Get x and y coordinates to show callout at mouse position    
            var xPos = 0; var yPos = 0;
            if (sEvent.pageX || sEvent.pageY) {
                xPos = sEvent.pageX;
                yPos = sEvent.pageY + 20; ;
            }
            else if (sEvent.clientX || sEvent.clientY) {
                xPos = sEvent.clientX + document.documentElement.scrollLeft + 10;
                yPos = sEvent.clientY + document.documentElement.scrollTop + 20;
            }
            else if (x || y) {
                xPos = x + document.documentElement.scrollLeft + 10;
                yPos = y + document.documentElement.scrollTop + 20;
            }
            //            else if (sEvent.offsetLeft || sEvent.offsetTop) {
            //                xPos = sEvent.offsetLeft + document.documentElement.scrollLeft + 10;
            //                yPos = sEvent.offsetTop + document.documentElement.scrollTop;
            //            }
            else if (sEvent.getBoundingClientRect() != null || sEvent.getBoundingClientRect() != undefined) {
                var rect = sEvent.getBoundingClientRect();
                if (rect.left || rect.top) {
                    xPos = rect.left + document.documentElement.scrollLeft + 10;
                    yPos = rect.top + document.documentElement.scrollTop + 20;
                }
            }


            divCallOut.style.left = xPos - marginLeft + "px";
            divCallOut.style.top = yPos - marginTop + "px";
        }
    }
    catch (err) { }
}

function SetCalloutPositionMenu(sEvent, divCallOut, marginLeft, marginTop, x, y) {
    try {
        sEvent = sEvent || window.event;

        if (divCallOut) {
            // Get x and y coordinates to show callout at mouse position    
            var xPos = 0; var yPos = 0;
            if (sEvent.pageX || sEvent.pageY) {
                xPos = sEvent.pageX;
                yPos = sEvent.pageY;
            }
            else if (sEvent.clientX || sEvent.clientY) {
                xPos = sEvent.clientX + document.documentElement.scrollLeft + 10;
                yPos = sEvent.clientY + document.documentElement.scrollTop;
            }
            else if (x || y) {
                xPos = x + document.documentElement.scrollLeft + 10;
                yPos = y + document.documentElement.scrollTop;
            }

            divCallOut.style.left = 600 + marginLeft + "px";
            divCallOut.style.top = marginTop + "px";
        }
    }
    catch (err) { }
}




/*-----------------------------------------------------------------------------*/
/*                                   Start  Masking                            */
/*-----------------------------------------------------------------------------*/

function ApplyMaskingDiv(zIndex) {
    var MaskedDiv;

    try {
        MaskedDiv = z('MaskedDiv');

        if (MaskedDiv == null) {
            MaskedDiv = document.createElement('MaskedDiv');
            MaskedDiv.id = 'MaskedDiv';
            document.body.appendChild(MaskedDiv);

        }

        // include a temporary iframe to make the ddl invisible/un-accessible
        var iFrameHtml = "<iframe style='position: absolute; display: block; background-color: #ffffff; " +
                            "z-index: -1; width: 100%; height: 100%; top: 0; left: 0; filter: mask();' >" + "</iframe>";

        // Masking the whole screen    
        if (IsIEBrowser()) {
            MaskedDiv.innerHTML += iFrameHtml;
        }
        MaskedDiv.style.display = '';
        MaskedDiv.style.visibility = 'visible';
        MaskedDiv.style.top = '0px';
        MaskedDiv.style.left = '0px';
        MaskedDiv.style.width = document.documentElement.clientWidth + 'px';
        //MaskedDiv.style.height = document.documentElement.scrollHeight + 'px';

        var PageHeight = document.documentElement.clientHeight;
        if (document.documentElement.scrollHeight > document.documentElement.clientHeight) {
            PageHeight = document.documentElement.scrollHeight;
        }
        MaskedDiv.style.height = PageHeight + 'px';

        if (zIndex) {
            MaskedDiv.style.zIndex = zIndex;
        }
    }
    catch (err) { }
}

function RemoveMaskingDiv(zIndex) {
    try {
        var MaskedDiv = z('MaskedDiv');

        // Remove the masking from the screen    
        MaskedDiv.style.display = 'none';
        MaskedDiv.innerHTML = "";

        if (zIndex) {
            MaskedDiv.style.zIndex = zIndex;
        }
    }
    catch (err) { }
}

function UpdateMaskZIndex(zIndex) {
    try {
        z("MaskedDiv").style.zIndex = zIndex;
    }
    catch (err) { }
}

function ShowLoadingDiv() {
    try {
        // Show the loading image        
        var divLoading = z("divLoading");

        if (divLoading == null) {
            divLoading = document.createElement('divLoading');
            divLoading.id = 'divLoading';
            document.body.appendChild(divLoading);

        }

        divLoading.style.display = "block";
    }
    catch (err) { }
}

function HideLoadingDiv() {
    try {
        // Hide the loading image
        var divLoading = z("divLoading");
        if (divLoading != null) {
            divLoading.style.display = "none";
        }
    }
    catch (err) { }
}

function IsIEBrowser() {
    var RetVal = true;

    try {
        if (/MSIE (\d+\.\d+);/.test(navigator.userAgent)) {
            //test for MSIE x.x;
            var ieversion = new Number(RegExp.$1) // capture x.x portion and store as a number
            if (ieversion >= 8) {
                // IE8 or above
                RetVal = true;
            }
            else if (ieversion >= 7) {
                // IE7.x
                RetVal = true;
            }
            else if (ieversion >= 6) {
                // IE6.x
                RetVal = true;
            }
            else if (ieversion >= 5) {
                //IE5.x
                RetVal = true;
            }
        }
        else {
            RetVal = false;
        }
    }
    catch (err) { }

    return RetVal;
}

/*-----------------------------------------------------------------------------*/
/*                                   End Masking                              */
/*-----------------------------------------------------------------------------*/

//Set a div at center of screen
function GetWindowCentered(ctlPopup, popupWidth, popupHeight) {
    var availHeight = 0;
    var availWidth = 0;
    var ScrollHeight = 0;

    try {
        if (typeof (window.innerWidth) == 'number') {
            //Chrome brower
            availHeight = window.innerHeight;
            availWidth = window.innerWidth;
            ScrollHeight = 0;
        } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
            //IE browser
            availHeight = document.documentElement.clientHeight;
            availWidth = document.documentElement.clientWidth;
            ScrollHeight = document.body.scrollTop;
        } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
            //Firefox browser
            availHeight = document.body.clientHeight;
            availWidth = document.body.clientWidth;
            ScrollHeight = document.body.scrollTop;
        }

        var WinTop = (availHeight - popupHeight) / 2;
        var WinLeft = (availWidth - popupWidth) / 2;

        if (WinTop < 0) WinTop = 0;
        if (WinLeft < 0) WinLeft = 0;

        ctlPopup.style.top = WinTop + ScrollHeight + "px";
        
        ctlPopup.style.left = WinLeft + "px";

        HideLoadingDiv();
    }
    catch (err) { }
}

/* Replace all occurance of a stirng */

function ReplaceAll(txt, replace, with_this) {
    return txt.replace(new RegExp(replace, 'g'), with_this);
}

function ReplaceAll1(Source, stringToFind, stringToReplace) {
    var RetVal = "";
    var TempArr;

    try {
        TempArr = Source.split(stringToFind);
        RetVal = TempArr.join(stringToReplace);
    }
    catch (err) { }

    return RetVal;
}


function Tweet() {
    var myTweet = z('txtTwitterShare').value + ' - ' + z('valShareLink').value;
    window.open("../aspx/tweet/Default.aspx?tweet=" + myTweet, '_blank', '', '');
}

function fbWallPost() {
    var parameters = "";
    var WallPost = z('txtFacebookShare').value + ' - ' + z('valShareLink').value;
    parameters += "wall_post=" + WallPost;
    if (z('valPictureLink') != null && z('valPictureLink') != undefined && z('valPictureLink').value != 'undefined' && z('valPictureLink').value != "") {
        parameters += "&picture_url=" + z('valPictureLink').value;
    }
    if (z('valTitle') != null && z('valTitle') != undefined && z('valTitle').value != 'undefined' && z('valTitle').value != "") {
        parameters += "&title=" + z('valTitle').value;
    }
    if (z('valDesc') != null && z('valDesc') != undefined && z('valDesc').value != 'undefined' && z('valDesc').value != "") {
        parameters += "&desc=" + z('valDesc').value;
    }
    window.open("../aspx/fb/Default.aspx?" + parameters, '_blank', '', '');
}

function SocialSharing(PreText, AppName, ShareLink, PictureLink, Title, Desc) {
    var EmailSent = z('lblEmailSent');
    EmailSent.style.display = "none";
    var EmbeddURL = '<iframe src="' + ShareLink + '" style="width:100%;height:500px;border:0px;"></iframe>';
    z('valShareLink').value = ShareLink;
    z('EmbeddLink').value = EmbeddURL;
    z('URL').value = ShareLink;
    z('valPictureLink').value = PictureLink;
    z('valTitle').value = Title;
    z('valDesc').value = Desc;
    z('txtTwitterShare').onkeypress = function () //Tweet 140 length check
    {
        var tmpLink = ShareLink;
        checkTweetLength(tmpLink);
    }
    z('txtTwitterShare').value = PreText + " - " + AppName; // + " - " + ShareLink;
    z('txtFacebookShare').value = PreText + " - " + AppName; // + " - " + ShareLink;

    ApplyMaskingDiv();
    GetWindowCentered(z("divEmbeddLink"), 600, 420);

    //Set close button at right corner of popup div
    SetCloseButtonInPopupDiv(di_jq('#divEmbeddLink'), 'HideSocialSharing');

    di_jq('#divEmbeddLink').show('slow');
}

function HideSocialSharing() {
    try {
        RemoveMaskingDiv();
        di_jq('#divEmbeddLink').hide('slow');
    }
    catch (err) { }
}


function ShareByEmail() {

    if (z('txtEmailShare').value == '') {
        alert("Please enter E-Mail Id");
        return;
    }
    var ShareLink = z('valShareLink').value; // z('EmbeddLink').value.replace('<iframe src="','').replace('" style="width:100%; border:0px;"></iframe>','');
    var InputParam = z('txtEmailShare').value.trim() + ParamDelimiter + ShareLink;

    var htmlResp = di_jq.ajax({
        type: "POST",
        url: "Callback.aspx",
        data: { 'callback': '16', 'param1': InputParam },
        async: false,
        success: function (data) {
            try {
                // TODO : Handle server response properly on email sending acknowledgement. For now it is being hard coded below.
                //if(data=='true')
                //                alert('Email sent.');
                var EmailSent = z('lblEmailSent');
                EmailSent.style.display = "inline";
                EmailSent.style.color = "#FF0000";
                EmailSent.style.fontWeight = "normal";
                //else 
                //alert('Email sending failed');               

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

function outerHTML(node) {
    return node.outerHTML ||
  (
      function (n) {
          var div = document.createElement('div'), h;
          div.appendChild(n.cloneNode(true));
          h = div.innerHTML;
          div = null;
          return h;
      }
   )(node);
}


//------------ Begin Database Details Component ------------------------------------------//

function di_draw_database_details_component(draw_div_id, width, height, database_name, database_details) {
    var di_html_data = '';

    try {
        // Outer Div
        di_html_data = '<div style="width:' + width + '; height:' + height + 'px;">';

        di_html_data += '<div class="di_dbs_main_cnt">';

        di_html_data += '<div class="di_dbs_spcn" >';

        //di_html_data += '<div>';

        //Database name row
        di_html_data += '<div class="di_dbs_name heading1">' + database_name + '</div>';

        //Database details row	    
        di_html_data += '<div class="di_dbs_details qds_textfield_default">' + database_details + '<div class="di_dbs_details_cmpt"><a href="javascript:ShowDbComponent();"><img src="../../stock/themes/default/images/down_arrow.png" /></a></div></div>'

        //di_html_data += '</div>';	 	    

        di_html_data += '</div></div>';

        // Set component HTML into div
        di_jq("#" + draw_div_id).html(di_html_data);
    }
    catch (err) { }
}

function ShowDbComponent() {
    ApplyMaskingDiv();
    ShowLoadingDiv();

    var dbNIdsArr;
    var i = 0;

    try {
        //Set close button at right corner of popup div
        SetCloseButtonInPopupDiv(di_jq('#popup_db_details'), 'CloseDbComponent');

        //Clear all checked databases
        di_jq('#popup_db_details').find(':checkbox').attr('checked', status);

        if (di_db_getAllSelDbNIds() != null && di_db_getAllSelDbNIds() != '') {
            dbNIdsArr = di_db_getAllSelDbNIds().split(",");
        }
        else {
            dbNIdsArr = z('hdbnid').value.split(",");
        }

        for (i = 0; i < dbNIdsArr.length; i++) {
            di_jq('#' + dbNIdsArr[i]).attr('checked', true);
        }

        di_jq("#popup_db_details").show('slow');

        //Set popup window at center of screen        
        GetWindowCentered(z('popup_db_details'), 552, 492);

        HideLoadingDiv();
    }
    catch (err) { }
}

function CloseDbComponent() {
    try {
        di_jq("#popup_db_details").hide('slow');

        RemoveMaskingDiv();
    }
    catch (err) { }
}

//------------ End Database Details Component ------------------------------------------//

function DrawDatabaseDetailsComponent(DbNIds, DivName) {
    var InputParam;

    InputParam = DbNIds;
    InputParam += ParamDelimiter + z('hlngcodedb').value;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '26', 'param1': InputParam },
            async: false,
            success: function (data) {
                try {
                    di_draw_database_details_component(DivName, '100%', 65, data.split(ParamDelimiter)[0], data.split(ParamDelimiter)[1]);
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

function OkDbComponent() {
    var DbNIds;
    var QDSHotSelFunctionName = '';

    try {
        DbNIds = di_db_getAllSelDbNIds();

        if (DbNIds.split(',').length == 1) {
            z('hdbnid').value = DbNIds;

            if (z('hdsby').value == "") //only for home
            {
                QDSHotSelFunctionName = 'qdsHotSelection';
            }
            else {
                QDSHotSelFunctionName = 'GetQDSResult';
            }

            ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div', z('hdbnid').value, z('hlngcodedb').value, z('langQdsIndCaption').value, z('langQdsAreaCaption').value, '100%', '100px', QDSHotSelFunctionName, "", "", "", "", false, z('langQdsSerchBtnText').value, z('langQdsAreaBlockText').value);

            //save current dbNId in cookie
            createCookie('hdbnid', DbNIds);
        }
        else {
            ShowQuickDSComponent(getAbsURL('stock'), 'quick_data_search_div', z('hdbnid').value, z('hlngcodedb').value, z('langQdsIndCaption').value, z('langQdsAreaCaption').value, '100%', '100px', 'GetASResult', "", "", "", "", true, z('langQdsSerchBtnText').value, z('langQdsAreaBlockText').value);
        }

        //Regenerate database details component for new database nids
        DrawDatabaseDetailsComponent(DbNIds, 'db_details_div');

        if (z('hdsby').value == "") //only for home
        {
            //Update indicator component
            ShowIndicatorComponent(getAbsURL('stock'), 'indicator_div', z('hdbnid').value, z('hlngcodedb').value, z("hselindo").value, '100%', '400', z("hsgcount").value);

            //Update area component
            ShowAreaComponent(getAbsURL('stock'), 'area_div', z('hdbnid').value, z('hlngcodedb').value, z('hselareao').value, '100%', '400');

            // Set default area count of selected database
            z("lblDefalutAreaCount").innerHTML = GetDefaultAreaCountOfSelDb();
        }
    }
    catch (err) { }
}


function SetCloseButtonInPopupDiv(divObj, closeMethodName, imgSrc) {
    if (imgSrc == undefined) {
        imgSrc = "../../stock/themes/default/images/close.png";
    }

    try {
        var elemAppend = '<a href="javascript:' + closeMethodName + '();" class="close"><img src="' + imgSrc + '" class="btn_close" title="Close Window" alt="Close"  border="0" /></a>';
        var exist = false;
        var divHtml = divObj.html();
        di_jq(divHtml).find('img').each(function () {
            if (di_jq(this).attr('class') == 'btn_close') exist = true;
        });
        if (exist == false)
            divObj.fadeIn().prepend(elemAppend);
    }
    catch (err) { }
}

function Youtubeclose(divObj, closeMethodName, imgSrc) {
    if (imgSrc == undefined) {
        imgSrc = "../../stock/themes/default/images/close.png";
    }

    try {
        var elemAppend = '<a href="javascript:' + closeMethodName + '();" class="close"><img src="' + imgSrc + '" class="btn_close" title="Close Window" alt="Close"  border="0" /></a>';
        var exist = false;
        var divHtml = divObj.html();
        di_jq(divHtml).find('img').each(function () {
            if (di_jq(this).attr('class') == 'btn_close') exist = true;
        });
        if (exist == false) {
            divObj.fadeIn().prepend(elemAppend);
        }
    }
    catch (err) { }
}

// Return name of running browser
function GetRunningBrowserName() {
    var agt = navigator.userAgent.toLowerCase();

    if (agt.indexOf("chrome") != -1) return 'Chrome';
    if (agt.indexOf("opera") != -1) return 'Opera';
    if (agt.indexOf("staroffice") != -1) return 'Star Office';
    if (agt.indexOf("webtv") != -1) return 'WebTV';
    if (agt.indexOf("beonex") != -1) return 'Beonex';
    if (agt.indexOf("chimera") != -1) return 'Chimera';
    if (agt.indexOf("netpositive") != -1) return 'NetPositive';
    if (agt.indexOf("phoenix") != -1) return 'Phoenix';
    if (agt.indexOf("firefox") != -1) return 'Firefox';
    if (agt.indexOf("safari") != -1) return 'Safari';
    if (agt.indexOf("skipstone") != -1) return 'SkipStone';
    if (agt.indexOf("msie") != -1) return 'Internet Explorer';
    if (agt.indexOf("netscape") != -1) return 'Netscape';
    if (agt.indexOf("mozilla/5.0") != -1) return 'Mozilla';

    if (agt.indexOf('\/') != -1) {
        if (agt.substr(0, agt.indexOf('\/')) != 'mozilla') {
            return navigator.userAgent.substr(0, agt.indexOf('\/'));
        }
        else {
            return 'Netscape';
        }
    }
    else if (agt.indexOf(' ') != -1) {
        return navigator.userAgent.substr(0, agt.indexOf(' '));
    }
    else {
        return navigator.userAgent;
    }
}

function SaveGalleryItem(CategoryNId, Indicators, Areas, UDK, Name, Desc, Type, ChartType, ImgData, SettingData) {
    var RetVal;
    var InputParam;

    InputParam = z('hdbnid').value;
    InputParam += ParamDelimiter + z('hlngcode').value; ;
    InputParam += ParamDelimiter + z('husernid').value; ;
    InputParam += ParamDelimiter + CategoryNId;
    InputParam += ParamDelimiter + Indicators;
    InputParam += ParamDelimiter + Areas;
    InputParam += ParamDelimiter + UDK;
    InputParam += ParamDelimiter + Name;
    InputParam += ParamDelimiter + Desc;
    InputParam += ParamDelimiter + Type;
    InputParam += ParamDelimiter + ChartType;
    InputParam += ParamDelimiter + ImgData;
    InputParam += ParamDelimiter + SettingData;

    try {
        var htmlResp = di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '34', 'param1': InputParam },
            async: true,
            success: function (data) {
                try {
                    RetVal = data;
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

    return RetVal;
}


function GetTblCellString(row, colIndex) {
    var RetVal = null;
    if (window.ActiveXObject) {
        RetVal = row.cells[colIndex].all.item(0);
    }
    else {
        RetVal = row.cells[colIndex].childNodes[0];
    }

    return RetVal;
}

// Return the value of specified column of row
function GetTblCellValue(row, colIndex) {
    var RetVal = null;
    if (window.ActiveXObject) {
        RetVal = row.cells[colIndex].innerText;
    }
    else {
        RetVal = row.cells[colIndex].innerHTML.trim();
        //RetVal=row.cells[colIndex].innerHTML;
    }
    return RetVal;
}

function validateEmail(elementValue) {
    var emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    return emailPattern.test(elementValue);
}

var isShift = false;

function isAlphaNumeric(keyCode) {
    if (keyCode == 16)
        isShift = true;

    return (((keyCode >= 48 && keyCode <= 57) && isShift == false) || (keyCode >= 65 && keyCode <= 90) || keyCode == 8 || keyCode == 9 || keyCode == 35 || keyCode == 36 || keyCode == 37 || keyCode == 39 || keyCode == 46 || (keyCode >= 96 && keyCode <= 105));

    // 8  - Backspace Key
    // 9 - Tab key
    // 35 - Home Key
    // 36 - End Key
    // 37 - Left Arrow Key
    // 39 - Right Arrow Key
    // 46 - Del Key  
}

function KeyUp(keyCode) {
    if (keyCode == 16)
        isShift = false;
}

/*

########################### START OF CODE FOR CHANGING CSS FILE PATH AT RUN TIME ######################

*/


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

function removejscssfile(oldfilename, filetype) {
    var targetelement = (filetype == "js") ? "script" : (filetype == "css") ? "link" : "none" //determine element type to create nodelist using
    var targetattr = (filetype == "js") ? "src" : (filetype == "css") ? "href" : "none" //determine corresponding attribute to test for
    var allsuspects = document.getElementsByTagName(targetelement)
    for (var i = allsuspects.length; i >= 0; i--) { //search backwards within nodelist for matching elements to remove
        if (allsuspects[i] && allsuspects[i].getAttribute(targetattr) != null && allsuspects[i].getAttribute(targetattr).indexOf(oldfilename) != -1) {
            allsuspects[i].parentNode.removeChild(allsuspects[i])
        }
    }
}

function checkloadjscssfile(filename, filetype) {
    if (filesadded.indexOf("[" + filename + "]") == -1) {
        loadjscssfile(filename, filetype)
        filesadded += "[" + filename + "]" //List of files added in the form "[filename1],[filename2],etc"
    }
    //else      alert("file already added!")
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



function clickSortOrder() {

    replacejscssfile("../../stock/themes/default/css/style.css", "../../stock/themes/default/css/Copy of style.css", "css");

}


/*

########################### END OF CODE FOR CHANGING CSS FILE PATH AT RUN TIME ######################

*/



//Get page width
function pageWidth() {
    return window.innerWidth != null ? window.innerWidth : document.documentElement && document.documentElement.clientWidth ? document.documentElement.clientWidth : document.body != null ? document.body.clientWidth : null;
}

function PageHeight() {
    return window.innerHeight != null ? window.innerHeight : document.documentElement && document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body != null ? document.body.clientHeight : null;
}

function GetLanguageCounts() {
    var RetVal = -1;
    var LanguageFile = "";

    try {
        LanguageFile = getAbsURL('stock') + 'language.xml?v=' + AppVersion;

        di_jq.ajax({
            type: "GET",
            url: LanguageFile,
            dataType: "xml",
            async: false,
            timeout: 5000,
            success: function (xml) {
                RetVal = di_jq(xml).find('lng').length
            }
        });
    }
    catch (err) { }

    return RetVal;
}

/* function get xml object for ie */
function parseXml(xml) {
    if (di_jq.browser.msie && (parseFloat(jQuery.browser.version) < 6.9999)) {
        var xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
        xmlDoc.loadXML(xml);
        xml = xmlDoc;
    }
    return xml;
}

function stopEventBubbling(event) {
    if (event.stopPropagation) {
        event.stopPropagation();
    }
    else if (window.event) {
        window.event.cancelBubble = true;
    }
}

function ConvertHtmlCodeToText(val) {
    val = val.replace(new RegExp(['&nbsp;'], "g"), "");
    return val;
}

function GetStandalonRegistryStatus() {
    var RetVal = "false";
    var FileNameWithPath = "";

    try {
        FileNameWithPath = getAbsURL('stock') + "appsettings.xml" + "?v=" + AppVersion;

        di_jq.ajax({
            type: "GET",
            url: FileNameWithPath,
            dataType: "xml",
            async: false,
            success: function (xml) {
                di_jq(xml).find('item[n="standalone_registry"]').each(function () {
                    RetVal = di_jq(this)[0].getAttribute("v");
                });
            }
        });
    }
    catch (err) { }

    return RetVal;
}

function SetAdaptationModeFromAdmin() {
    try {
        if (GetStandalonRegistryStatus() == "true") {
            //di_jq("#aHeader").attr("href", "../RegStructuralMD.aspx");
            di_jq("#aHeader").attr("href", "../RegStructure.aspx");
            di_jq(".bg_image").css("top", "0px");
            di_jq(".bg_image").css("z-index", "-1");
            di_jq("#img_adaptation_logo").addClass("logo_glbl_pos");
            di_jq("#MainContent").addClass("MainContent_glbl");
            di_jq(".MainContent").css("margin-top", "0px");
            di_jq(".MainContent").css("background", "none");
            di_jq(".hm_vid_pnl_contnr").addClass("hm_vid_pnl_contnr_exp_pos");
        }
        else {
            di_jq("#aHeader").attr("href", "../Home.aspx")
        }
    }
    catch (err) { }
}

function AllowOnlyDigits(e, txtObj) {
    var RetVal = true;

    e = (e) ? e : window.event;
    var charCode = (e.which) ? e.which : e.keyCode;

    // ? , End, Home, Left, Right
    if (charCode == 8 || charCode == 35 || charCode == 36 || charCode == 37 || charCode == 39) {
        RetVal = true;
    }
    else if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        RetVal = false;
    }

    return RetVal;
}

function EnableDisableLinks(link, disable) {
    var attribute = null;

    if (disable) {
        link.disabled = "disabled";

        attribute = link.getAttribute("onclick");
        link.setAttribute("back", attribute);
        link.removeAttribute("onclick");

        link.style.color = "gray";
    }
    else {
        link.disabled = "";

        attribute = link.getAttribute("back");

        if (attribute != null) {
            link.setAttribute("onclick", attribute);
            link.removeAttribute("back");
        }

        link.style.color = "#5c86ad";
    }
}


function EnableButton(btnId) {
    try {
        di_jq(btnId).removeAttr("disabled");
        di_jq(btnId)[0].className = "di_gui_button";
    } catch (ex) { }
}


function DisableButton(btnId) {
    try {
        di_jq(btnId).attr("disabled", "disabled");
        di_jq(btnId)[0].className = "di_gui_button_disabled";
    } catch (ex) { }
}

function IsIE7() {
    var RetVal = false;
    if (/MSIE (\d+\.\d+);/.test(navigator.userAgent)) {
        var ieversion = new Number(RegExp.$1);
        RetVal = (ieversion >= 7 && ieversion <= 7);
    }

    return RetVal;
}

function IsIE6() {
    var RetVal = false;
    if (/MSIE (\d+\.\d+);/.test(navigator.userAgent)) {
        var ieversion = new Number(RegExp.$1);
        RetVal = (ieversion >= 6 && ieversion <= 6);
    }

    return RetVal;
}

function DisableEnterKeyPress(event) {
    if (event.keyCode == 13) {
        if (window.ActiveXObject) {
            event.returnValue = false;
            event.cancel = true;
        }
        else {
            event.preventDefault();
        }
    }
}

function ShowInnovationHorizontalMenu() {
    var InnovationDisplayVal = "";
    var NewsDisplayVal = "";
    var ContactUseVal = "";
    var SupportVal = "";
    var DownloadsVal = "";
    var TrainingVal = "";
    var MapLibraryVal = "";
    var langFAQVal = "";
    var langHowToVideos = "";
    var RSSFeedsVal = "";
    var DiWorldWideVal = "";
    var Sitemaplink = "";
    var langKB = "";

    try {

        InnovationDisplayVal = (isInnovationsMenuVisible.toLowerCase() == "false") ? "none" : "";
        NewsDisplayVal = (isNewsMenuVisible.toLowerCase() == "false") ? "none" : "";
        ContactUseVal = (isContactUsMenuVisible.toLowerCase() == "false") ? "none" : "";
        SupportVal = (isSupportMenuVisible.toLowerCase() == "false") ? "none" : "";
        DownloadsVal = (isDownloadsLinkVisible.toLowerCase() == "false") ? "none" : "";
        TrainingVal = (isTrainingLinkVisible.toLowerCase() == "false") ? "none" : "";
        MapLibraryVal = (isMapLibraryLinkVisible.toLowerCase() == "false") ? "none" : "";
        langFAQVal = (islangFAQVisible.toLowerCase() == "false") ? "none" : "";
        langHowToVideos = (isHowtoVideoVisible.toLowerCase() == "false") ? "none" : "";
        RSSFeedsVal = (isRSSFeedsLinkVisible.toLowerCase() == "false") ? "none" : "";
        DiWorldWideVal = (isDiWorldWideLinkVisible.toLowerCase() == "false") ? "none" : "";
        Sitemaplink = (isSitemapLinkVisible.toLowerCase() == "false") ? "none" : "";
        langKB = (islangKBVisible.toLowerCase() == "false") ? "none" : "";

        z("LiInnovations").style.display = InnovationDisplayVal;
        z("aInnovations").style.display = InnovationDisplayVal;
        z("footerInnovations").style.display = InnovationDisplayVal;

        z("LiNews").style.display = NewsDisplayVal;
        z("aNews").style.display = NewsDisplayVal;
        z("footerNews").style.display = NewsDisplayVal;
        z("liSupport").style.display = SupportVal;
        z("LangSupport").style.display = SupportVal;
        z("LanguagePipeSupport").style.display = SupportVal;
        z("liContactUs").style.display = ContactUseVal;
        z("LangContactUs").style.display = ContactUseVal;
        z("LanguagePipeLi").style.display = ContactUseVal;
        z("langDIDownloads").parentElement.style.display = DownloadsVal;
        z("langDITraining").parentElement.style.display = TrainingVal;
        z("langDIMapLibrary").parentElement.style.display = MapLibraryVal;
        z("langFAQ").parentElement.style.display = langFAQVal;
        z("langKB").parentElement.style.display = langKB;
        z("langHowToVideos").parentElement.style.display = langHowToVideos;
        z("aRSSFeed").style.display = RSSFeedsVal;
        z("langDIWorldWide").parentElement.style.display = DiWorldWideVal;
        z("langSiteMap").parentElement.style.display = Sitemaplink;
        var languageCount = GetLanguageCounts();
        if (languageCount <= 1) {
            z("liUserpeipeLine").style.display = "none";
        }
        if (GetStandalonRegistryStatus() == "true" && IsShowElements==true) {
            z("headerDIVMenu").parentElement.style.display = "block";
        }
        try {
            var Url = document.URL;
            if (Url.indexOf("sitemap.aspx") != -1) {
                z("slangDIDownloads").parentElement.style.display = DownloadsVal;
                z("slangDITraining").parentElement.style.display = TrainingVal;
                z("slangDIMapLibrary").parentElement.style.display = MapLibraryVal;
                z("slangDIWorldWide").parentElement.style.display = DiWorldWideVal;
                z("slangFAQ").parentElement.style.display = langFAQVal;
                z("slangKB").parentElement.style.display = langKB;
                z("slangHowToVideos").parentElement.style.display = langHowToVideos;
                z("slangSiteMap").parentElement.style.display = Sitemaplink;
                z("cphMainContent_sfooterNews").style.display = NewsDisplayVal;
                z("cphMainContent_footerInnovations").style.display = InnovationDisplayVal;
            }

        } catch (e) {

        }

    }
    catch (err) { }
}

function GetBrowserLanguage() {
    var RetVal = "";
    try {
        var language;
        di_jq.ajax({
            url: "http://ajaxhttpheaders.appspot.com",
            dataType: 'jsonp',
            success: function (headers) {
                language = headers['Accept-Language'];
                if (language.length > 1) {
                    RetVal = language.substr(0, 2);
                }
                else {
                    RetVal = language;
                }
                // hlngcode = RetVal;
            },
            error: function () {
                alert("error");
            }
        });

    }
    catch (ex) { }
    return RetVal;
}

/* Remove invalid characters from filename
Parameters: invalidFilename - invalid filename
*/
function removeInvalidCharacters(invalidFilename) {
    var invalidCharString = "/:|*\"?<>\\";
    var validFilename = "";

    try {
        for (var i = 0; i < invalidFilename.length; i++) {
            if (invalidCharString.indexOf(invalidFilename[i]) == -1) {
                validFilename += invalidFilename[i];
            }
        }
    }
    catch (err) { }

    return validFilename;
}

// Set up Equal Height of Columns	
function equalHeight(group) {
    try {
        tallest = 0;
        group.each(function () {
            thisHeight = $(this).height();
            if (thisHeight > tallest) {
                tallest = thisHeight;
            }
        });
        group.height(tallest);
    }
    catch (err) { }
}
function getDefaultLanguage() {
    var XmlPageURL = getAbsURL('stock') + "db.xml" + "?v=" + AppVersion;
    var defLang = "";
    di_jq.ajax({
        type: "GET",
        url: XmlPageURL,
        dataType: "xml",
        async: false,
        timeout: 5000,
        error: function (request, error) {
        },
        success: function (xml) {
            var id = "";

            di_jq(parseXml(xml)).find('dbinfo').each(function () {
                id = di_jq(this).attr("def");
            });
            di_jq(parseXml(xml)).find('db').each(function () {
                var currentid = di_jq(this).attr("id");
                if (currentid == id) {
                    defLang = di_jq(this).attr("deflng");
                    return;
                }
            });
        }
    });
    return defLang;
}


function ChangeHTM() {
    var selected_hlngcode = "";
    if (readCookie('hlngcode') == null) {
        selected_hlngcode = z('hlngcode').value;
    }
    else selected_hlngcode = readCookie('hlngcode');
    z('frame_auto').src = getAbsURL('stock') + "Adaptation/About/" + selected_hlngcode + ".htm";
}

function GetAdaptitionYear() {
    var RetVal = "false";
    var FileNameWithPath = "";

    try {
        FileNameWithPath = getAbsURL('stock') + "appsettings.xml" + "?v=" + AppVersion;

        di_jq.ajax({
            type: "GET",
            url: FileNameWithPath,
            dataType: "xml",
            async: false,
            success: function (xml) {
                di_jq(xml).find('item[n="adaptation_year"]').each(function () {
                    RetVal = di_jq(this)[0].getAttribute("v");
                });
            }
        });
    }
    catch (err) { }

    return RetVal;
}

function GetAdaptitionName() {
    var RetVal = "false";
    var FileNameWithPath = "";

    try {
        FileNameWithPath = getAbsURL('stock') + "appsettings.xml" + "?v=" + AppVersion;

        di_jq.ajax({
            type: "GET",
            url: FileNameWithPath,
            dataType: "xml",
            async: false,
            success: function (xml) {
                di_jq(xml).find('item[n="adaptation_name"]').each(function () {
                    RetVal = di_jq(this)[0].getAttribute("v");
                });
            }
        });
    }
    catch (err) { }

    return RetVal;
}

/* function to create link for downloading CSV file and return html */
function CreateLinkForDownloadCSV(AdaptationUrl, LinkNamesCatalog, Cat_AdaptName, Cat_AdaptationYear) {
    var HTMLStr = '';
    var CountFiles = 0;
    var LinkPath = '';
    var LinkNames = "";
    var linkNametoShow = "";
    var CSVFileName = '';
    try {
        var FileNameToShowOnTool = '';
        var AdaptitionName = GetAdaptitionName();
        //        var CSVFileStartName = AdaptitionName.replace(/\s/g, "_") + "_" + GetAdaptitionYear() + "_";
        var CSVFileStartName = AdaptitionName + "_" + GetAdaptitionYear() + "_";
        if (LinkNamesCatalog != null) {
            LinkNames = LinkNamesCatalog;
        }
        else {
            LinkNames = getCsvFilesName();
        }

        if (LinkNames != undefined && LinkNames != null && LinkNames != '') {
            if (AdaptationUrl != undefined && AdaptationUrl != '') {
                HTMLStr += "<p class='para_txt' style='font-size: 11px; font-family: Arial, Helvetica, sans-serif'>" + z('langDownloadCSVFile').value + "&nbsp;&nbsp;";
            }
            else {
                HTMLStr += "<p class='para_txt' style='padding-top:8px; font-size: 12px;'>" + z('langDownloadCSVFile').value + "&nbsp;" + ":" + "&nbsp;";
            }
            HTMLStr += "<span style='font-style:normal'>" + GetAdaptitionYear() + " " + "</span>";
            CountFiles = LinkNames.split(",");

            var HTMLStr1;
            if (CountFiles != 0) {

                for (var iCount = 0; iCount < CountFiles.length; iCount++) {
                    linkNametoShow = CountFiles[iCount].split("_")[CountFiles[iCount].split("_").length - 1];
                    if (AdaptationUrl != undefined && AdaptationUrl != '') {
                        CSVFileStartName = Cat_AdaptName.replace(/\s+/g, "_") + "_" + Cat_AdaptationYear + "_";
                        CSVFileName = CSVFileStartName + CountFiles[iCount] + ".zip";
                        LinkPath = AdaptationUrl + "/stock/data/CSV_DataFiles/" + CSVFileName;
                        FileNameToShowOnTool = Cat_AdaptName + "_" + Cat_AdaptationYear + "_" + CountFiles[iCount];
                    }
                    else {
                        FileNameToShowOnTool = AdaptitionName + "_" + GetAdaptitionYear() + "_" + CountFiles[iCount];
                        CSVFileName = CSVFileStartName + CountFiles[iCount] + ".zip";
                        LinkPath = getAbsURL('stock') + "data/CSV_DataFiles/" + CSVFileName;
                        FileNameToShowOnTool = AdaptitionName + "_" + GetAdaptitionYear() + "_" + CountFiles[iCount];
                    }
                    HTMLStr += "<span style='font-style:normal' id='a" + CountFiles[iCount] + "' onmouseover='ShowCallout(\"divCallout\", \"" + FileNameToShowOnTool + "\", event);' onmouseout='HideCallout(\"divCallout\");'><a href='" + LinkPath + "'>[" + linkNametoShow + "]</a>&nbsp;&nbsp;&nbsp;</span>";
                }
            }
            HTMLStr += "</p>";
        }
    }
    catch (ex) { }
    return HTMLStr;
}
/* function used to get name of link for downloading CSV file from xml */
function getCsvFilesName() {
    var XmlPageURL = getAbsURL('stock') + "db.xml" + "?v=" + AppVersion;
    var defCsvFileNames = "";
    di_jq.ajax({
        type: "GET",
        url: XmlPageURL,
        dataType: "xml",
        async: false,
        timeout: 5000,
        error: function (request, error) {
        },
        success: function (xml) {
            var id = "";

            di_jq(parseXml(xml)).find('dbinfo').each(function () {
                id = di_jq(this).attr("def");
            });
            di_jq(parseXml(xml)).find('db').each(function () {
                var currentid = di_jq(this).attr("id");
                if (currentid == id) {
                    defCsvFileNames = di_jq(this).attr("langcode_csvfiles");
                    return;
                }
            });
        }
    });
    return defCsvFileNames;
}


function SelectNewsMenuOption() {
    var CtrlName, CtrlNameLNav = "";
    var menuType = getQueryStr("T");
    try {
        ClearNewsMenuSelection();
        CtrlName = "aNews";
        switch (menuType) {
            case "N":
                //CtrlName = "aNews";                
                CtrlNameLNav = "cphMainContent_aNews";
                break;
            case "A":
                //CtrlName = "aDIA";
                CtrlNameLNav = "cphMainContent_aDIA";
                break;
            case "F":
                //CtrlName = "aFOW";
                CtrlNameLNav = "cphMainContent_aFOW";
                break;
        }

        if (z(CtrlNameLNav) != null && z(CtrlNameLNav).className != null) {
            z(CtrlNameLNav).className = "adm_lft_nav_seld";
        }
        if (z(CtrlName) != null && z(CtrlName).className != null) {
            z(CtrlName).className = "navActive";
        }
    }
    catch (err) { }
}

// document.getElementById('<%=EditPage.ClientID%>').href = Url + "EditCmsContents.aspx?" + "[" + '"' + "PN=" + '"' + "]" + urlArray[1];
function SelectCMSNewsMenuOption() {
    var CtrlName, CtrlNameLNav = "";
    try {
        ClearNewsMenuSelection();
        CtrlName = "aNews";

        var MenuCategory = '';
        if (GetArticleMenuCategory() != null && GetArticleMenuCategory() != undefined) {
            MenuCategory = GetArticleMenuCategory();
        }
        if (MenuCategory != "") {
            if (MenuCategory.indexOf("news") != -1) {
                CtrlNameLNav = "cphMainContent_aNews";
                SpnMenuCategoryHeading = "Article_News";
            }
            if (MenuCategory.indexOf("action") != -1) {
                CtrlNameLNav = "cphMainContent_aDIA";
                SpnMenuCategoryHeading = "Article_Action";
            }
            if (MenuCategory.indexOf("facts") != -1) {
                CtrlNameLNav = "cphMainContent_aFOW";
                SpnMenuCategoryHeading = "Article_Facts";
            }
        }
        if (document.getElementById("SpanMenuCategoryHeading") != null && document.getElementById("SpanMenuCategoryHeading") != undefined) {
            document.getElementById("SpanMenuCategoryHeading").innerText = document.getElementById(SpnMenuCategoryHeading).value;
        }
        if (z(CtrlNameLNav) != null && z(CtrlNameLNav).className != null) {
            z(CtrlNameLNav).className = "adm_lft_nav_seld";
        }
        if (z(CtrlName) != null && z(CtrlName).className != null && z(CtrlName).className != undefined) {
            z(CtrlName).className = "navActive";
        }
    }
    catch (err) { }
}

function ClearNewsMenuSelection() {
    try {
        if (z("aNews") != null && z("aNews").className != null) {
            z("aNews").className = "nav";
        }
        if (z("aFOW") != null && z("aFOW").className != null) {
            z("aDIA").className = "";
        }
        if (z("aFOW") != null && z("aFOW").className != null) {
            z("aFOW").className = "";
        }

    }
    catch (err) { }
}

function SelectInnovationMenuOption() {
    var CtrlName, CtrlNameMenu = "";
    var menuType = getQueryStr("T");
    var InnovationMainHead = '';
    var InnovationSubHead = '';
    try {
        //ClearNewsMenuSelection();
        ClearInnovationsMenuSelection();
        CtrlName = "aInnovations";
        switch (menuType) {
            case "M":
                //CtrlName = "aMonitoring";
                CtrlNameMenu = "cphMainContent_aMonitoring";
                z(CtrlName).className = "navActive";
                z(CtrlNameMenu).className = "adm_lft_nav_seld";
                //Set Header Text
                InnovationMainHead = z(CtrlNameMenu).innerHTML;
                InnovationSubHead = "";
                break;
            case "P":
                //CtrlName = "aProfile";
                CtrlNameMenu = "cphMainContent_aProfile";
                z(CtrlName).className = "navActive";
                z(CtrlNameMenu).className = "adm_lft_nav_seld";
                //Set Header Text
                InnovationMainHead = z(CtrlNameMenu).innerHTML;
                InnovationSubHead = "";
                break;
            case "G":
                //CtrlName = "aGameworks";
                CtrlNameMenu = "cphMainContent_aGameworks";
                z(CtrlName).className = "navActive";
                z(CtrlNameMenu).className = "adm_lft_nav_seld";
                break;
            case "D":
                //CtrlName = "aDashboard";
                CtrlNameMenu = "cphMainContent_aDashboard";
                z(CtrlName).className = "navActive";
                z(CtrlNameMenu).className = "adm_lft_nav_seld";
                //Set Header Text
                InnovationMainHead = z(CtrlNameMenu).innerHTML;
                InnovationSubHead = "";
                break;
            case "I":
                //CtrlName = "aDashboard";
                z(CtrlName).className = "navActive";
                //Set Header Text
                InnovationMainHead = "Innovations";
                InnovationSubHead = "di Monitoring, di Profile, di Gameworks, di Dashboard";
                break;


        }
        if (di_jq(InnoMainHeading) != null && di_jq(InnoMainHeading).innerHTML != null) {
            di_jq(InnoMainHeading).innerHTML = InnovationMainHead;
        }
        else if (di_jq(InnoMainHeading) != null && di_jq(InnoMainHeading).context.innerHTML != null) {
            di_jq(InnoMainHeading).context.innerHTML = InnovationMainHead;
        }
        if (di_jq(InnoSubHeading) != null && di_jq(InnoSubHeading).innerHTML != null) {
            di_jq(InnoSubHeading).innerHTML = InnovationSubHead;
        }
        else if (di_jq(InnoSubHeading) != null && di_jq(InnoSubHeading).context.innerHTML != null) {
            di_jq(InnoSubHeading).context.innerHTML = InnovationSubHead;
        }
    }
    catch (err) { }
}



function SelectFAQMenuOption() {
    var CtrlName, CtrlNameMenu = "";
    var menuType = getQueryStr("T");
    try {
        //ClearNewsMenuSelection();
        ClearInnovationsMenuSelection();
        //  CtrlName = "aInnovations";
        switch (menuType) {
            case "A":
                //CtrlName = "aMonitoring";
                CtrlNameMenu = "cphMainContent_aGeneralFAQ";
                //z(CtrlName).className = "navActive";
                z(CtrlNameMenu).className = "adm_lft_nav_seld";
                break;
            case "G":
                //CtrlName = "aProfile";
                CtrlNameMenu = "cphMainContent_aApplicationSpecificFAQ";
                //  z(CtrlName).className = "navActive";
                z(CtrlNameMenu).className = "adm_lft_nav_seld";
                break;
        }
    }
    catch (err) { }
}


function ClearInnovationsMenuSelection() {
    try {
        if (z("aMonitoring") != null && z("aMonitoring").className != null) {
            z("aMonitoring").className = "";
        }
        if (z("aProfile") != null && z("aProfile").className != null) {
            z("aProfile").className = "";
        }
        if (z("aGameworks") != null && z("aGameworks").className != null) {
            z("aGameworks").className = "";
        }
        if (z("aDashboard") != null && z("aDashboard").className != null) {
            z("aDashboard").className = "";
        }
    }
    catch (err) { }
}

function CheckIsDI7ORGAdaptation_CMS() {
    try {
        di_jq.ajax({
            type: "POST",
            url: "../libraries/aspx/" + CallBackPageName,
            data: { 'callback': '1020' },
            async: true,
            success: function (data) {
                try {
                    // For Global 
                    if (data == "false") {
                        isGlobalAdap = "global";
                        //di_jq(".headerlink1").css("background", "none");
                        z("headerDIVMenu").style.display = "block";
                        di_jq(".bg_image").css("top", "0px");
                        di_jq(".bg_image").css("z-index", "-1");
                        di_jq("#img_adaptation_logo").addClass("logo_glbl_pos");
                        di_jq("#MainContent").addClass("MainContent_glbl");
                        di_jq(".MainContent").css("margin-top", "0px");
                        di_jq(".MainContent").css("background", "none");
                        di_jq(".hm_vid_pnl_contnr").addClass("hm_vid_pnl_contnr_exp_pos");
                        //                        z("divCountryMenu").style.display = "none";

                    }
                    //For Country
                    else {
                        if (GetStandalonRegistryStatus() == "false") {

                            isGlobalAdap = "local";
                            //di_jq("#top_menu_link2 li").css("margin", "30px 0px 0px");
                            z("headerDIVMenu").style.display = "block";
                            di_jq(".headerlink1").css("background", "#E5E5E5");
                            di_jq("#top_menu_link2 li").addClass("expcntry_li_pos");
                            di_jq("#langCountryMenuDevinfo").addClass("expntry_selctd");
                            di_jq("#aGlobalCatalog").addClass("expntry_selctd");

                            var isOpen = false;
                            di_jq("#divCountryMenu").mouseenter(function () {
                                //n += 1;
                                //$(this).find("span").text("mouse enter x " + n);
                                if (!isOpen) {
                                    jQuery("#CountryETxt_hs").slideDown('slow', function () { isOpen = true; });
                                }
                            }).mouseleave(function () {
                                if (isOpen) {
                                    //$(this).find("span").text("mouse leave");
                                    jQuery("#CountryETxt_hs").slideUp('slow', function () { isOpen = false; });
                                }
                                else {

                                }
                            });
                            di_jq("#body").mousemove(function (e) {
                                if (isOpen) {
                                    if (!IsTargetExistIntoList(e.target.id)) {
                                        jQuery("#CountryETxt_hs").slideUp('slow', function () { isOpen = false; });
                                    }
                                }
                            });
                            //});

                            function IsTargetExistIntoList(targetId) {
                                var RetVal = false;
                                var DivArr = ['divCountryMenu', 'divCountryMenuMain', 'LiGlobalURL', 'hvr_ww_txt', 'aGoto', 'langCountryMenuDevinfo', 'langCountryMenuWorldWide', 'LiGlobalCatalog', 'aGlobalCatalog', 'LiGlobalNews', 'aGlobalNews', 'LiGlobalInnovations', 'aGlobalInnovations', 'CountryETxt_hs', 'aGotoDesc', 'aGlobalNewsDesc', 'aGlobalInnovationsDesc', 'aGlobalCatalogDesc', 'di_ww_icn'];
                                //var DivArr = ['divCountryMenu'];        
                                if (DivArr.indexOf(targetId) > -1) {
                                    RetVal = true;
                                }
                                return RetVal;
                            }
                        }
                        //z("divCountryMenu").style.display = "";
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

function CheckIsDI7ORGAdaptation() {
    try {
        di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '1020' },
            async: true,
            success: function (data) {
                try {
                    // For Global 
                    if (data == "false") {
                        isGlobalAdap = "global";
                        //di_jq(".headerlink1").css("background", "none");
                        if (IsShowElements == true) {
                            z("headerDIVMenu").style.display = "block";
                        }
                        else {
                            z("headerDIVMenu").style.display = "none";
                        }
                        di_jq(".bg_image").css("top", "0px");
                        di_jq(".bg_image").css("z-index", "-1");
                        di_jq("#img_adaptation_logo").addClass("logo_glbl_pos");
                        di_jq("#MainContent").addClass("MainContent_glbl");
                        di_jq(".MainContent").css("margin-top", "0px");
                        di_jq(".MainContent").css("background", "#009999");
                        di_jq(".hm_vid_pnl_contnr").addClass("hm_vid_pnl_contnr_exp_pos");
                        //z("divCountryMenu").style.display = "none";


                    }
                    //For Country
                    else {
                        if (GetStandalonRegistryStatus() == "false") {

                            isGlobalAdap = "local";
                            //di_jq("#top_menu_link2 li").css("margin", "30px 0px 0px");
                            if (IsShowElements == true) {
                                z("headerDIVMenu").style.display = "block";
                            }
                            else {
                                z("headerDIVMenu").style.display = "none";
                            }
                            di_jq(".headerlink1").css("background", "#009999");   //#E5E5E5
                            di_jq("#top_menu_link2 li").addClass("expcntry_li_pos");
                            di_jq("#langCountryMenuDevinfo").addClass("expntry_selctd");
                            di_jq("#aGlobalCatalog").addClass("expntry_selctd");

                            var isOpen = false;
                            di_jq("#divCountryMenu").mouseenter(function () {
                                //n += 1;
                                //$(this).find("span").text("mouse enter x " + n);
                                if (!isOpen) {
                                    jQuery("#CountryETxt_hs").slideDown('slow', function () { isOpen = true; });
                                }
                            }).mouseleave(function () {
                                if (isOpen) {
                                    //$(this).find("span").text("mouse leave");
                                    jQuery("#CountryETxt_hs").slideUp('slow', function () { isOpen = false; });
                                }
                                else {

                                }
                            });
                            di_jq("#body").mousemove(function (e) {
                                if (isOpen) {
                                    if (!IsTargetExistIntoList(e.target.id)) {
                                        jQuery("#CountryETxt_hs").slideUp('slow', function () { isOpen = false; });
                                    }
                                }
                            });
                            //});

                            function IsTargetExistIntoList(targetId) {
                                var RetVal = false;
                                var DivArr = ['divCountryMenu', 'divCountryMenuMain', 'LiGlobalURL', 'hvr_ww_txt', 'aGoto', 'langCountryMenuDevinfo', 'langCountryMenuWorldWide', 'LiGlobalCatalog', 'aGlobalCatalog', 'LiGlobalNews', 'aGlobalNews', 'LiGlobalInnovations', 'aGlobalInnovations', 'CountryETxt_hs', 'aGotoDesc', 'aGlobalNewsDesc', 'aGlobalInnovationsDesc', 'aGlobalCatalogDesc', 'di_ww_icn'];
                                //var DivArr = ['divCountryMenu'];        
                                if (DivArr.indexOf(targetId) > -1) {
                                    RetVal = true;
                                }
                                return RetVal;
                            }

                        }
                        //z("divCountryMenu").style.display = "";
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

function CheckIfGlobalAdaptation() {
    try {
        di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '1009' },
            async: true,
            success: function (data) {
                try {
                    // For Global 
                    if (data == "false") {
                        isGlobalAdap = "global";
                        //di_jq(".headerlink1").css("background", "none");
                        z("headerDIVMenu").style.display = "block";
                        di_jq(".bg_image").css("top", "0px");
                        di_jq(".bg_image").css("z-index", "-1");
                        di_jq("#img_adaptation_logo").addClass("logo_glbl_pos");
                        di_jq("#MainContent").addClass("MainContent_glbl");
                        di_jq(".MainContent").css("margin-top", "0px");
                        di_jq(".MainContent").css("background", "none");
                        di_jq(".hm_vid_pnl_contnr").addClass("hm_vid_pnl_contnr_exp_pos");
                        //                        z("divCountryMenu").style.display = "none";
                    }
                    //For Country
                    else {
                        if (GetStandalonRegistryStatus() == "false") {

                            isGlobalAdap = "local";
                            //di_jq("#top_menu_link2 li").css("margin", "30px 0px 0px");
                            z("headerDIVMenu").style.display = "block";
                            di_jq(".headerlink1").css("background", "#E5E5E5");
                            di_jq("#top_menu_link2 li").addClass("expcntry_li_pos");
                            di_jq("#langCountryMenuDevinfo").addClass("expntry_selctd");
                            di_jq("#aGlobalCatalog").addClass("expntry_selctd");

                            var isOpen = false;
                            di_jq("#divCountryMenu").mouseenter(function () {
                                //n += 1;
                                //$(this).find("span").text("mouse enter x " + n);
                                if (!isOpen) {
                                    jQuery("#CountryETxt_hs").slideDown('slow', function () { isOpen = true; });
                                }
                            }).mouseleave(function () {
                                if (isOpen) {
                                    //$(this).find("span").text("mouse leave");
                                    jQuery("#CountryETxt_hs").slideUp('slow', function () { isOpen = false; });
                                }
                                else {

                                }
                            });
                            di_jq("#body").mousemove(function (e) {
                                if (isOpen) {
                                    if (!IsTargetExistIntoList(e.target.id)) {
                                        jQuery("#CountryETxt_hs").slideUp('slow', function () { isOpen = false; });
                                    }
                                }
                            });
                            //});

                            function IsTargetExistIntoList(targetId) {
                                var RetVal = false;
                                var DivArr = ['divCountryMenu', 'divCountryMenuMain', 'LiGlobalURL', 'hvr_ww_txt', 'aGoto', 'langCountryMenuDevinfo', 'langCountryMenuWorldWide', 'LiGlobalCatalog', 'aGlobalCatalog', 'LiGlobalNews', 'aGlobalNews', 'LiGlobalInnovations', 'aGlobalInnovations', 'CountryETxt_hs', 'aGotoDesc', 'aGlobalNewsDesc', 'aGlobalInnovationsDesc', 'aGlobalCatalogDesc', 'di_ww_icn'];
                                //var DivArr = ['divCountryMenu'];        
                                if (DivArr.indexOf(targetId) > -1) {
                                    RetVal = true;
                                }
                                return RetVal;
                            }

                        }
                        //z("divCountryMenu").style.display = "";
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


function SelectWorldwideMenuOption() {
    var CtrlName, CtrlNameLNav = "";
    var menuType = getQueryStr("T");
    try {
        ClearWorldwideMenuSelection();
        CtrlName = "Worldwide1";
        switch (menuType) {
            case "W1":
                CtrlNameLNav = "cphMainContent_Worldwide1";
                break;
            case "W2":
                CtrlNameLNav = "cphMainContent_Worldwide2";
                break;
            case "W3":
                CtrlNameLNav = "cphMainContent_Worldwide3";
                break;
            case "W4":
                CtrlNameLNav = "cphMainContent_Worldwide4";
                break;
        }
        z(CtrlNameLNav).className = "adm_lft_nav_seld";
        z(CtrlName).className = "navActive";
    }
    catch (err) { }
}
function ClearWorldwideMenuSelection() {
    try {
        z("Worldwide1").className = "nav";
        z("Worldwide2").className = "";
        z("Worldwide3").className = "";
        z("Worldwide4").className = "";
    }
    catch (err) { }
}


function SelectDownloadsMenuOption() {
    var CtrlName, CtrlNameLNav = "";
    var menuType = getQueryStr("T");
    try {
        ClearNewsMenuSelection();
        CtrlName = "aDownload7";
        switch (menuType) {
            case "DL7":
                CtrlNameLNav = "cphMainContent_aDownload7";
                break;
            case "DL6":
                CtrlNameLNav = "cphMainContent_aDownload6";
                break;
        }

        z(CtrlNameLNav).className = "adm_lft_nav_seld";
        z(CtrlName).className = "navActive";
    }
    catch (err) { }
}
function ClearDownloadsMenuSelection() {
    try {
        z("aDownload7").className = "nav";
        z("aDownload6").className = "";
    }
    catch (err) { }
}

function SelectTrainingMenuOption() {
    var CtrlName, CtrlNameLNav = "";
    var menuType = getQueryStr("T");
    try {
        //ClearTrainingMenuSelection();
        //CtrlName = "aDownload7";
        switch (menuType) {
            case "TCW":
                CtrlNameLNav = "aTrainingCourses";
                break;
            case "EL":
                CtrlNameLNav = "aElearningCourses";
                break;
            case "AR":
                CtrlNameLNav = "aAdditionalCourses";
                break;
            case "TS":
                CtrlNameLNav = "aTrainingSchedule";
                break;
        }

        z(CtrlNameLNav).className = "adm_lft_nav_seld";
        //z(CtrlName).className = "navActive";
    }
    catch (err) { }
}


/*
function ShowDescMenus() {
try {
// Show - slide down.
di_jq("#CountryETxt_hs").slideDown();
}
catch (err) { }
}

function HideDescMenus() {
try {
       
// Hide - slide up.
di_jq("#CountryETxt_hs").slideUp();
}
catch (err) { }
}
*/

function GetGlobalAdaptationURL() {
    try {
        di_jq.ajax({
            type: "POST",
            url: CallBackPageName,
            data: { 'callback': '1010' },
            async: true,
            success: function (data) {
                try {
                    if (data != undefined && data != null) {
                        //                        z('aGenerateDataSnapshots').href = data + "innovations.aspx?T=D&PN=diorg/di_dashboards.html";
                        window.open(data + "innovations.aspx?T=D&PN=diorg/di_dashboards.html", '_blank', '', '');
                    }
                    else {
                        z('aGenerateDataSnapshots').href = "#";
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
    catch (err) { }
}
function x() {
    alert("hello");
}



function GetYouTubeVideo(GetLinkFor) {
    var RetUrl = '';
    var CurrentLanguage = z('hlngcode').value;
    var Data;
    var FileNameWithPath = "";
    try {
        FileNameWithPath = getAbsURL('stock') + "appsettings.xml" + "?v=" + AppVersion;
        di_jq.ajax({
            type: "GET",
            url: FileNameWithPath,
            dataType: "xml",
            async: false,
            success: function (xml) {
                di_jq(xml).find('item[n="HowToVideoData"]').each(function () {
                    Data = di_jq(this)[0].getAttribute("v");
                    Data = Data.replace(/'/g, '"');
                    var ObjVideoData = di_jq.parseJSON(Data);
                    for (var dictKey in ObjVideoData) {
                        if (CurrentLanguage == dictKey) {
                            var VideoData = ObjVideoData[dictKey];
                            if (GetLinkFor == "Home") {
                                RetUrl = VideoData[0].Homelink;
                                if (RetUrl.indexOf("youtu.be/") > -1) {
                                    RetUrl = RetUrl.replace("youtu.be/", "www.youtube.com/watch?v=");
                                }
                            }
                            else if (GetLinkFor == "visulization") {
                                RetUrl = VideoData[0].Visualizerlink;
                                if (RetUrl.indexOf("youtu.be/") > -1) {
                                    RetUrl = RetUrl.replace("youtu.be/", "www.youtube.com/watch?v=");
                                }
                            }
                            break;
                        }
                    }
                });
            }
        });
    }
    catch (err) { }
    return RetUrl;
}


function CheckIsVideoOptionIsEnabled(CheckVideoOptEnabledFor) {
    var IsVideoLinkVisible = false;
    var CurrentLanguage = z('hlngcode').value;
    var Data;
    var FileNameWithPath = "";
    try {
        FileNameWithPath = getAbsURL('stock') + "appsettings.xml" + "?v=" + AppVersion;
        di_jq.ajax({
            type: "GET",
            url: FileNameWithPath,
            dataType: "xml",
            async: false,
            success: function (xml) {
                di_jq(xml).find('item[n="HowToVideoData"]').each(function () {
                    Data = di_jq(this)[0].getAttribute("v");
                    Data = Data.replace(/'/g, '"');
                    var ObjVideoData = di_jq.parseJSON(Data);
                    for (var dictKey in ObjVideoData) {
                        if (CurrentLanguage == dictKey) {
                            var VideoData = ObjVideoData[dictKey];
                            if (CheckVideoOptEnabledFor == "Home") {
                                IsVideoLinkVisible = VideoData[0].HomeVisible;
                            }
                            else if (CheckVideoOptEnabledFor == "visulization") {
                                IsVideoLinkVisible = VideoData[0].VisualizerVisible;
                            }
                            break;
                        }
                    }
                });
            }
        });
    }
    catch (err) { }
    return IsVideoLinkVisible;
}

function PlayHowToVideoPopUp(isOpen, PlayVideoFor) {
    try {
        var xPos = 0; var yPos = 0;
        //xPos = Math.floor(window.innerWidth / 2);
        //yPos = Math.floor(window.innerHeight / 2);
        xPos = 480;
        yPos = 385;
        if (isOpen) {
            ApplyMaskingDiv();
            Youtubeclose(di_jq('#DivHowToVideo'), 'PlayHowToVideoPopUp', CloseImgSrc);
            var GetLinkFor = PlayVideoFor;

            if (PlayVideoFor == "Home") {
                VideoUrl = GetYouTubeVideo(GetLinkFor);
            }
            else if (PlayVideoFor == "visulization") {
                VideoUrl = GetYouTubeVideo(GetLinkFor);
            }
            if (VideoUrl.indexOf('watch?v=') != -1) {
                var id = VideoUrl.split('watch?v=')[1];
                var YouTubeVideoUrl = 'http://www.youtube.com/v/' + id;
            }
            else {
                YouTubeVideoUrl = VideoUrl;
            }
            if (YouTubeVideoUrl.indexOf("embed/") > -1) {
                di_jq("#dvHowToVdeo").html("<iframe width='480' id='ytplayer' height='385' src='" + YouTubeVideoUrl + "' frameborder='0' allowfullscreen></iframe>");
            }
            else {
                di_jq("#dvHowToVdeo").html("<object width='480' height='385'><param name='movie' value=" + YouTubeVideoUrl + "></param><param name='allowFullScreen' value='true'></param><param name='allowscriptaccess' value='always'></param><embed src=" + YouTubeVideoUrl + " type='application/x-shockwave-flash' allowscriptaccess='always' allowfullscreen='true' width='480' height='385'></embed></object>");
            }
            di_jq('#DivHowToVideo').show('slow');

            GetWindowCentered(z("DivHowToVideo"), xPos, yPos);
        }
        else {
            di_jq('#dvHowToVdeo').html("");
            di_jq('#DivHowToVideo').hide('slow');
            RemoveMaskingDiv();
        }
    }
    catch (err) { }
}


function ShowHidePlayHowToVideoButton(ShowHideForPage, ControlId) {
    try {
        var IsVideoLinkVisible = false;
        IsVideoLinkVisible = CheckIsVideoOptionIsEnabled(ShowHideForPage);

        if (IsVideoLinkVisible == "true" && ControlId != null && ControlId != undefined) {

            ControlId.style.display = "";
        }
        else if (ControlId != null && ControlId != undefined) {
            ControlId.style.display = "none";

        }
    }
    catch (err) { }
}


function toggleVideo(state, dvHowToVdeo) {
    // if state == 'hide', hide. Else: show video
    var div = document.getElementById("dvHowToVdeo");
    if (div.getElementsByTagName("iframe")[0] != null && div.getElementsByTagName("iframe")[0] != undefined) {
        var iframe = div.getElementsByTagName("iframe")[0].contentWindow;
        div.style.display = state == 'hide' ? 'none' : '';
        func = state == 'hide' ? 'pauseVideo' : 'playVideo';
        iframe.postMessage('{"event":"command","func":"' + func + '","args":""}', '*');
    }
    else
        di_jq('#dvHowToVdeo').html("");
}

/* function used to get db nid of default database*/
function getDefaultDbId() {
    var XmlPageURL = getAbsURL('stock') + "db.xml" + "?v=" + AppVersion;
    var DefDbId = "";
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
                DefDbId = di_jq(this).attr("def");
                return;
            });
        }
    });
    return DefDbId;
}

/* function used to get the name of default database dsd */
function getDefaultDSDName(DbId) {
    var XmlPageURL = getAbsURL('stock') + "db.xml" + "?v=" + AppVersion;
    var DSDName = "";
    di_jq.ajax({
        type: "GET",
        url: XmlPageURL,
        dataType: "xml",
        async: false,
        timeout: 5000,
        error: function (request, error) {
        },
        success: function (xml) {

            di_jq(parseXml(xml)).find('db').each(function () {
                var currentid = di_jq(this).attr("id");
                if (currentid == DbId) {
                    DSDName = di_jq(this).attr("n");
                    return;
                }
            });
        }
    });
    return DSDName;
}